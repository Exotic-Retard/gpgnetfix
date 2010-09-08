namespace GPGnetClientLib
{
    using GPGnetCommunicationsLib;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class MessageBroker
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private const int port = 0x2af8;
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static string response = string.Empty;
        private static StateObject sClient;
        private static Socket sClientSocket = null;
        private static ManualResetEvent sendDone = new ManualResetEvent(false);

        public static  event CommandMessageDelegate OnCommandMessage;

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                connectDone.Set();
                Socket asyncState = (Socket) ar.AsyncState;
                asyncState.EndConnect(ar);
                asyncState.BeginReceive(sClient.buffer, 0, 0x1000, SocketFlags.None, new AsyncCallback(MessageBroker.ReceiveCallback), sClient);
                ClientManager.LogData(string.Format("Socket connected to {0}", asyncState.RemoteEndPoint.ToString()));
                Send(sClient, new CommandMessage { CommandName = Commands.RequestConnectionID }.FormatMessage());
            }
            catch (Exception exception)
            {
                ClientManager.LogData(exception.ToString());
            }
        }

        public static void Disconnect()
        {
            if (sClientSocket != null)
            {
                sClientSocket.Disconnect(false);
                sClientSocket = null;
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject asyncState = (StateObject) ar.AsyncState;
                Socket workSocket = asyncState.workSocket;
                if (workSocket.Connected && !asyncState.ConnectionError)
                {
                    int length = 0;
                    try
                    {
                        length = workSocket.EndReceive(ar);
                    }
                    catch (ObjectDisposedException)
                    {
                        asyncState.ConnectionError = true;
                        ClientManager.LogData("A connection has been disposed and is no longer valid.");
                        return;
                    }
                    catch (SocketException)
                    {
                        asyncState.ConnectionError = true;
                        ClientManager.LogData("The server has shut down.");
                        if (OnCommandMessage != null)
                        {
                            CommandMessage message = new CommandMessage {
                                CommandName = Commands.TextMessageRoom
                            };
                            Credentials credentials = new Credentials {
                                Name = "Disconnect",
                                PrincipalID = -1
                            };
                            message.SetParams(new object[] { "Disconnect", "Connection to the GPGnet server has been lost.", credentials });
                            OnCommandMessage(asyncState.ConnectionID, message);
                            message.SetParams(new object[] { "Disconnect", "Please restart GPGnet once network connectivity has been restored.", credentials });
                            OnCommandMessage(asyncState.ConnectionID, message);
                        }
                        return;
                    }
                    if (length > 0)
                    {
                        byte[] destinationArray = new byte[length];
                        Array.Copy(asyncState.buffer, destinationArray, length);
                        asyncState.Stream.Position = asyncState.Stream.Length;
                        asyncState.Stream.Write(destinationArray, 0, destinationArray.Length);
                        try
                        {
                            for (CommandMessage message2 = asyncState.GetNextMessage(); message2 != null; message2 = asyncState.GetNextMessage())
                            {
                                if (OnCommandMessage != null)
                                {
                                    OnCommandMessage(asyncState.ConnectionID, message2);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            asyncState.ResetState();
                            ClientManager.LogData(exception);
                        }
                        workSocket.BeginReceive(asyncState.buffer, 0, 0x1000, SocketFlags.None, new AsyncCallback(MessageBroker.ReceiveCallback), asyncState);
                    }
                    else
                    {
                        asyncState.ConnectionError = true;
                        ClientManager.LogData("Received a 0 byte disconnect from " + asyncState.ConnectionID.ToString());
                    }
                }
            }
            catch (Exception exception2)
            {
                ClientManager.LogData(exception2.ToString());
            }
        }

        private static void Send(StateObject state, byte[] data)
        {
            if (((state != null) && state.workSocket.Connected) && !state.ConnectionError)
            {
                state.workSocket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(MessageBroker.SendCallback), state);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            StateObject asyncState = (StateObject) ar.AsyncState;
            try
            {
                if (!asyncState.ConnectionError && asyncState.workSocket.Connected)
                {
                    int num = asyncState.workSocket.EndSend(ar);
                    ClientManager.LogData(string.Format("Sent {0} bytes to server.", num));
                    sendDone.Set();
                }
            }
            catch (Exception exception)
            {
                asyncState.ConnectionError = true;
                ClientManager.LogData(exception.ToString());
            }
        }

        public static void SendMessage(CommandMessage message)
        {
            Send(sClient, message.FormatMessage());
        }

        public static void StartClient(string host, int port)
        {
            try
            {
                IPAddress address = Dns.Resolve(host).AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(address, port);
                Socket state = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sClientSocket = state;
                sClient = new StateObject();
                sClient.workSocket = state;
                state.BeginConnect(remoteEP, new AsyncCallback(MessageBroker.ConnectCallback), state);
                connectDone.WaitOne();
            }
            catch (Exception exception)
            {
                ClientManager.LogData(exception.ToString());
            }
        }
    }
}

