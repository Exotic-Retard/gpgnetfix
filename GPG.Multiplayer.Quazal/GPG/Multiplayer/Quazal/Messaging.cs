namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.Logging;
    using GPG.Network;
    using GPG.Threading;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;

    public static class Messaging
    {
        public const char ArgumentSeparator = '\x001f';
        public const int CHUNK_SIZE = 0x7d0;
        private static bool mPolling;
        private static Thread PollThread;
        private static Hashtable sTranslateMap = Hashtable.Synchronized(new Hashtable());
        private static Dictionary<int, NetTransfer> Transfers = new Dictionary<int, NetTransfer>();

        public static  event MessageEventHandler CommandRecieved;

        public static  event MessageEventHandler CustomCommandRecieved;

        public static  event MessageEventHandler MessageRecieved;

        public static  event NetDataEventHandler NetDataRecieved;

        public static void AutoTranslate(string name, string lang)
        {
            sTranslateMap[name] = lang;
        }

        public static string DecodeMessage(string message)
        {
            if (message.IndexOf('\x001e') >= 0)
            {
                string[] strArray = message.Split(new char[] { '\x001e' }, 2);
                byte[] bytes = Convert.FromBase64String(strArray[1]);
                return (strArray[0] + Encoding.UTF8.GetString(bytes));
            }
            return message;
        }

        public static string EncodeMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            if (bytes.Length > message.Length)
            {
                return ('\x001e' + Convert.ToBase64String(bytes));
            }
            return message;
        }

        private static void OnPollChat()
        {
            while (Polling)
            {
                try
                {
                    if (Lobby.LoggingOut)
                    {
                        StopPolling();
                        break;
                    }
                    if (Lobby.GetLobby().IsConnected())
                    {
                        for (string str = DecodeMessage(Lobby.GetLobby().chatroomHelper.GetNextMessage()); (str != null) && (str.Length > 0); str = DecodeMessage(Lobby.GetLobby().chatroomHelper.GetNextMessage()))
                        {
                            EventLog.WriteLine("Message Recieved: {0}", new object[] { str });
                            MessageEventArgs e = new MessageEventArgs(str);
                            if (e.IsCommand)
                            {
                                if (CommandRecieved != null)
                                {
                                    CommandRecieved(e);
                                }
                            }
                            else if (MessageRecieved != null)
                            {
                                MessageRecieved(e);
                            }
                        }
                    }
                    else
                    {
                        MessageEventArgs args2 = new MessageEventArgs("//DISCONNECT");
                        if (CommandRecieved != null)
                        {
                            CommandRecieved(args2);
                            CommandRecieved(args2);
                            CommandRecieved(args2);
                        }
                        StopPolling();
                    }
                    Thread.Sleep(100);
                    continue;
                }
                catch (ThreadInterruptedException exception)
                {
                    ErrorLog.WriteLine(exception);
                    EventLog.WriteLine("Exited out of the chat thread through an interrupt.", new object[0]);
                    break;
                }
                catch (Exception exception2)
                {
                    ErrorLog.WriteLine(exception2);
                    Thread.Sleep(100);
                    continue;
                }
            }
        }

        public static void PollChat()
        {
            mPolling = true;
            PollThread = new Thread(new ThreadStart(Messaging.OnPollChat));
            PollThread.IsBackground = true;
            PollThread.Start();
            EventLog.WriteLine("Now polling messages", new object[0]);
        }

        public static void RecieveData(string sender, int dataCode, int chunk, int totalChunks, string data)
        {
            if (!Transfers.ContainsKey(dataCode))
            {
                Transfers[dataCode] = new NetTransfer(totalChunks);
            }
            Transfers[dataCode].AddChunk(chunk, data);
            EventLog.WriteLine("Recieved chunk {0} of {1}, length: {2} from {3}", new object[] { chunk + 1, totalChunks, data.Length, sender });
            if (Transfers[dataCode].IsComplete)
            {
                try
                {
                    byte[] bin = Transfers[dataCode].GetBin();
                    Transfers[dataCode] = null;
                    Transfers.Remove(dataCode);
                    if (NetDataRecieved != null)
                    {
                        NetDataRecieved(new Node(sender), bin);
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public static void SendCustomCommand(CustomCommands cmd, params object[] args)
        {
            string msg = string.Format("/CUST {0} {1} {2} ", (uint) cmd, User.Current.ID, User.Current.Name);
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] != null)
                    {
                        msg = msg + '\x001f' + args[i].ToString();
                    }
                }
            }
            if (CustomCommandRecieved != null)
            {
                CustomCommandRecieved(new MessageEventArgs(msg));
            }
            SendGathering(msg);
        }

        public static void SendCustomCommand(string recipient, CustomCommands cmd, params object[] args)
        {
            SendCustomCommand(new string[] { recipient }, cmd, args);
        }

        public static void SendCustomCommand(string[] recipients, CustomCommands cmd, params object[] args)
        {
            if (User.Current != null)
            {
                string message = string.Format("/CUST {0} {1} {2} ", (uint) cmd, User.Current.ID, User.Current.Name);
                if (args != null)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] != null)
                        {
                            message = message + '\x001f' + args[i].ToString();
                        }
                    }
                }
                SendPlayers(recipients, message);
            }
        }

        public static void SendData(string recipient, INetData data)
        {
            SendData(new string[] { recipient }, data);
        }

        public static void SendData(string[] recipients, INetData data)
        {
            MemoryStream serializationStream = new MemoryStream();
            try
            {
                new BinaryFormatter().Serialize(serializationStream, data);
                byte[] inArray = serializationStream.ToArray();
                serializationStream.Close();
                serializationStream = null;
                string[] chunks = NetUtil.GetChunks(Convert.ToBase64String(inArray, Base64FormattingOptions.None), 0x7d0);
                for (int i = 0; i < chunks.Length; i++)
                {
                    EventLog.WriteLine("Sending chunk {0} of {1}", new object[] { i + 1, chunks.Length });
                    SendCustomCommand(recipients, CustomCommands.NetData, new object[] { data.GetHashCode(), i, chunks.Length, chunks[i] });
                }
            }
            finally
            {
                if (serializationStream != null)
                {
                    serializationStream.Close();
                    serializationStream = null;
                }
            }
        }

        public static void SendGathering(string message)
        {
            EventLog.WriteLine("Gathering Message: {0}", new object[] { message });
            ThreadQueue.Quazal.Enqueue(Lobby.GetLobby().chatroomHelper, "MessageChatroom", null, null, new object[] { EncodeMessage(message) });
        }

        public static void SendPlayer(string player, string message)
        {
            SendPlayers(new string[] { player }, message);
        }

        public static void SendPlayers(string[] players, string message)
        {
            for (int i = 0; i < players.Length; i++)
            {
                EventLog.WriteLine("Player Message: ({0}) {1}", new object[] { players[i], message });
                if ((players[i] != null) && (players[i].Length > 0))
                {
                    ThreadQueue.Quazal.Enqueue(Lobby.GetLobby().chatroomHelper, "CommandMessagePlayer", null, null, new object[] { players[i], EncodeMessage(message) });
                }
            }
        }

        public static void StopPolling()
        {
            try
            {
                EventLog.WriteLine("Stopping message polling", new object[0]);
                mPolling = false;
                if (PollThread != null)
                {
                    PollThread.Interrupt();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public static string Translate(string name, string message)
        {
            if (sTranslateMap.Contains(name))
            {
                return TranslateUtil.GetTranslatedText(message, "en", sTranslateMap[name].ToString());
            }
            return message;
        }

        public static bool Polling
        {
            get
            {
                return mPolling;
            }
        }
    }
}

