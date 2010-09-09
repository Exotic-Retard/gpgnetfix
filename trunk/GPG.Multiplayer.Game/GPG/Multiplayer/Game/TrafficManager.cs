namespace GPG.Multiplayer.Game
{
    using GPG.Logging;
    using GPG.Multiplayer.Game.Network;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using System;
    using System.Collections;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TrafficManager : IDisposable
    {
        private Hashtable mConnectAttempts = new Hashtable();
        private INetConnection mConnection;
        private System.Collections.Queue mMessageQueue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
        private EventWaitHandle mMessageEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
        private Hashtable mPlayerInfo = Hashtable.Synchronized(new Hashtable());
        private bool mProcessing;
        private Thread mProcessThread;

        public event MessageEventHandler OnCommandMessage;

        public event NewPlayer OnNewPlayer;

        public event ReceiveMessage OnReceiveMessage;

        public event RemovePlayer OnRemovePlayer;

        public TrafficManager()
        {
            Messaging.CommandRecieved += new MessageEventHandler(this.Messaging_CommandRecieved);
            Messaging.MessageRecieved += new MessageEventHandler(this.Messaging_MessageRecieved);
            this.SetConnectivity();
            Game.OnNatMessage += new NatMessageDelegate(this.Game_OnNatMessage);
        }

        public void CreateNewPlayer(string ipaddr, int port)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipaddr), port);
            this.MakeInfoMessage(endpoint);
            NetMessage message = new NetMessage {
                EndPoint = endpoint,
                Text = "/ASKREPLY " + ipaddr + ":" + port.ToString(),
                MessageType = NetMessageTypes.GPGText
            };
            this.QueueMessage(message);
        }

        private void DisconnectPlayer(int connectionid)
        {
            this.mPlayerInfo[connectionid] = null;
            if (this.OnRemovePlayer != null)
            {
                this.OnRemovePlayer(connectionid);
            }
        }

        public void Dispose()
        {
            this.mProcessing = false;
            Game.OnNatMessage -= new NatMessageDelegate(this.Game_OnNatMessage);
            Messaging.CommandRecieved -= new MessageEventHandler(this.Messaging_CommandRecieved);
            Messaging.MessageRecieved -= new MessageEventHandler(this.Messaging_MessageRecieved);
        }

        private void Game_OnNatMessage(SNatMessage NatMessage)
        {
            IPAddress address;
            NetMessage message = new NetMessage();
            if (!IPAddress.TryParse(NatMessage.Address, out address))
            {
                address = Dns.GetHostEntry(NatMessage.Address).AddressList[0];
            }
            message.EndPoint = new IPEndPoint(address, NatMessage.Port);
            message.Buffer = NatMessage.Message;
            this.QueueMessage(message);
        }

        private NetMessage MakeInfoMessage(IPEndPoint endpoint)
        {
            return new NetMessage { EndPoint = endpoint, Text = string.Concat(new object[] { "/PLAYERID ", User.Current.ID, " ", User.Current.Name }), MessageType = NetMessageTypes.GPGText };
        }

        private void mConnection_OnReceiveMessage(NetMessage message)
        {
            EventLog.WriteLine("Traffic Manager received a message(" + message.MessageType.ToString() + "): " + message.EndPoint.Address.ToString() + " " + message.Text, LogCategory.Get("TrafficManager"), new object[0]);
            if (message.MessageType == NetMessageTypes.Nat)
            {
                Game.IncomingNatMessage("udp:/address=" + message.EndPoint.Address.ToString() + ";port=" + message.EndPoint.Port.ToString(), message.Buffer, message.Buffer.Length);
                EventLog.WriteLine("Registerd NAT traffic back to Quazal.", LogCategory.Get("TrafficManager"), new object[0]);
                if (message.Text.IndexOf("udp:/") < 0)
                {
                    this.QueueMessage(this.MakeInfoMessage(message.EndPoint));
                }
            }
            else if (message.MessageType == NetMessageTypes.GPGText)
            {
                string[] strArray = message.Text.Split(" ".ToCharArray(), 2);
                if (strArray.Length == 2)
                {
                    string str2 = strArray[0];
                    string data = strArray[1];
                    switch (str2)
                    {
                        case "/PLAYERID":
                        {
                            PlayerInformation info = new PlayerInformation(data) {
                                EndPoint = message.EndPoint
                            };
                            if (info.PlayerName != User.Current.Name)
                            {
                                try
                                {
                                    if (!this.mConnectAttempts.ContainsKey(info.PlayerName))
                                    {
                                        this.mConnectAttempts.Add(info.PlayerName, 0);
                                    }
                                    int num = (int) this.mConnectAttempts[info.PlayerName];
                                    num++;
                                    if (num < ConfigSettings.GetInt("PlayerIDResponseAttempts", 30))
                                    {
                                        this.CreateNewPlayer(message.EndPoint.Address.ToString(), message.EndPoint.Port);
                                        this.mConnectAttempts[info.PlayerName] = num;
                                    }
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            }
                            if ((!this.mPlayerInfo.ContainsKey(info.PlayerID) || (this.mPlayerInfo[info.PlayerID] == null)) && (this.OnNewPlayer != null))
                            {
                                this.OnNewPlayer(info);
                            }
                            this.mPlayerInfo[info.PlayerID] = info;
                            goto Label_02E2;
                        }
                        case "/ASKREPLY":
                        {
                            string[] strArray2 = data.Split(new char[] { ':' });
                            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(strArray2[0]), Convert.ToInt32(strArray2[1]));
                            this.QueueMessage(this.MakeInfoMessage(endpoint));
                            this.QueueMessage(this.MakeInfoMessage(message.EndPoint));
                            goto Label_02E2;
                        }
                    }
                }
            }
        Label_02E2:
            if (this.OnReceiveMessage != null)
            {
                this.OnReceiveMessage(message);
            }
        }

        private void Messaging_CommandRecieved(MessageEventArgs e)
        {
            EventLog.WriteLine("A Message Command was received: " + e.Command, LogCategory.Get("Command"), e.CommandArgs);
            foreach (string str in e.CommandArgs)
            {
                EventLog.WriteLine("Arguement: " + str, LogCategory.Get("Command"), new object[0]);
            }
            string command = e.Command;
            string[] commandArgs = e.CommandArgs;
            string str5 = command.ToLower();
            if (str5 != null)
            {
                if (!(str5 == "event"))
                {
                    if (str5 == "notifyexit")
                    {
                        this.DisconnectPlayer(Convert.ToInt32(commandArgs[0]));
                    }
                }
                else
                {
                    string[] strArray2 = commandArgs[0].Split(",=".ToCharArray());
                    if (strArray2.Length > 9)
                    {
                        int num = Convert.ToInt32(strArray2[1]);
                        string text1 = strArray2[3];
                        string text2 = strArray2[5];
                        string str3 = strArray2[7];
                        string str4 = strArray2[9];
                        int connectionid = Convert.ToInt32(strArray2[11]);
                        if (((num == 3) && ((str4 == "2") || (str4 == "7"))) && (str3 != User.Current.Name))
                        {
                            this.DisconnectPlayer(connectionid);
                        }
                    }
                }
            }
            if (this.OnCommandMessage != null)
            {
                this.OnCommandMessage(e);
            }
        }

        private void Messaging_MessageRecieved(MessageEventArgs e)
        {
            EventLog.WriteLine("Test command received: " + e.Message, new object[0]);
        }

        private void NoResult(bool result)
        {
        }

        private void Process()
        {
            this.mProcessing = true;
            while (this.mProcessing)
            {
                if (this.mMessageQueue.Count > 0)
                {
                    NetMessage message = this.mMessageQueue.Dequeue() as NetMessage;
                    if (message != null)
                    {
                        this.mConnection.SendMessage(message.FormatMessage(), message.EndPoint.Address.ToString(), message.EndPoint.Port);
                        EventLog.WriteLine("Traffic Manager Processed a Message: " + message.EndPoint.ToString() + " " + message.Text, LogCategory.Get("TrafficManager"), new object[0]);
                    }
                }
                else
                {
                    mMessageEvent.WaitOne();
                }
            }
        }

        public void QueueMessage(NetMessage message)
        {
            this.mMessageQueue.Enqueue(message);
            this.mMessageEvent.Set();
        }

        public void SetConnection(INetConnection connection)
        {
            this.SetConnection(connection, -1, -1);
        }

        public void SetConnection(INetConnection connection, int port, int connectionid)
        {
            this.mProcessing = false;
            if (this.mProcessThread != null)
            {
                this.mProcessThread.Interrupt();
                this.mProcessThread.Join();
            }
            if (this.mConnection != null)
            {
                this.mConnection.Close();
            }
            this.mConnection = connection;
            if (!this.mConnection.IsBound)
            {
                this.mConnection.Bind(port, connectionid);
            }
            this.mConnection.OnReceiveMessage += new ReceiveMessage(this.mConnection_OnReceiveMessage);
            this.StartThread();
        }

        private void SetConnectivity()
        {
            ThreadQueue.Quazal.Enqueue(typeof(Game), "SetConnectivity", null, null, new object[0]);
        }

        private void StartThread()
        {
            ThreadStart start = new ThreadStart(this.Process);
            this.mProcessThread = new Thread(start);
            this.mProcessThread.IsBackground = true;
            this.mProcessThread.Start();
        }
    }
}

