namespace GPGnetClientLib
{
    using GPGnetClientLib.ClientHandlers;
    using GPGnetCommunicationsLib;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Net;

    public class ClientManager
    {
        private Dictionary<Commands, IClientHandler> mClientHandlers = new Dictionary<Commands, IClientHandler>();
        private static string sHostname = Dns.GetHostName();
        public static ClientManager sManager = null;
        private static int sPort = 0x2af8;

        public static  event LogDelegate OnLogData;

        private ClientManager()
        {
            LogData("Registering Command Handlers");
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsInterface && (type.GetInterface("IClientHandler", true) != null))
                {
                    IClientHandler handler = Activator.CreateInstance(type) as IClientHandler;
                    LogData("Registering Command Handler: " + type.Name + " " + handler.command.ToString());
                    this.mClientHandlers.Add(handler.command, handler);
                }
            }
            MessageBroker.OnCommandMessage += new CommandMessageDelegate(this.MessageBroker_OnCommandMessage);
            MessageBroker.StartClient(sHostname, sPort);
        }

        public void Disconnect()
        {
            MessageBroker.Disconnect();
        }

        public static string GetHostName()
        {
            return sHostname;
        }

        public static ClientManager GetManager()
        {
            if (sManager == null)
            {
                sManager = new ClientManager();
            }
            return sManager;
        }

        public static void LogData(object data)
        {
            if (OnLogData != null)
            {
                OnLogData(data);
            }
        }

        private void MessageBroker_OnCommandMessage(int connectionID, CommandMessage message)
        {
            LogData(Thread.CurrentThread.ManagedThreadId.ToString() + " Server request to client: " + message.CommandName.ToString());
            object[] msgParams = message.GetParams();
            if (msgParams != null)
            {
                foreach (object obj2 in msgParams)
                {
                    LogData(obj2.ToString());
                }
            }
            object[] state = new object[] { connectionID, message };
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                object[] objArray = o as object[];
                int num1 = (int) objArray[0];
                CommandMessage message1 = objArray[1] as CommandMessage;
                if (this.mClientHandlers.ContainsKey(message1.CommandName))
                {
                    this.mClientHandlers[message1.CommandName].HandleCommand(connectionID, message, this);
                }
            }, state);
        }

        public void MessageServer(CommandMessage command)
        {
            LogData(Thread.CurrentThread.ManagedThreadId.ToString() + " Client request to server: " + command.CommandName.ToString());
            object[] msgParams = command.GetParams();
            if (msgParams != null)
            {
                foreach (object obj2 in msgParams)
                {
                    LogData(obj2.ToString());
                }
            }
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                MessageBroker.SendMessage(o as CommandMessage);
            }, command);
        }

        public static void ResetManager()
        {
            sManager = null;
        }

        public static void SetConnection(string hostname, int port)
        {
            sHostname = hostname;
            sPort = port;
        }
    }
}

