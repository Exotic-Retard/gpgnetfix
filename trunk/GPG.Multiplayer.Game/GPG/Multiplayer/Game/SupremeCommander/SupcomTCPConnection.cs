namespace GPG.Multiplayer.Game.SupremeCommander
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Game;
    using GPG.Multiplayer.Game.Network;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SupcomTCPConnection : INetConnection
    {
        private int mConnectionID;
        private Driver mDriver;
        private bool mExited;
        private Thread mIsRunningThread;
        private Socket mListener;
        private object mMutex;
        private Process mProcess;
        private Socket mSocket;
        private int mSupcomPort;

        public event EventHandler OnExit;

        public event EventHandler<Driver.InputEventArgs> OnGetCommand;

        public event ReceiveMessage OnReceiveMessage;

        public SupcomTCPConnection(bool automatch, string path, string arguements, string address, int port, bool isReplay, bool isHost)
        {
            AsyncCallback callback = null;
            this.mMutex = new object();
            this.mSupcomPort = -1;
            this.mConnectionID = -1;
            string str = path;
            if (ConfigSettings.GetBool("UseHidden", false))
            {
                str = str.Replace("SupremeCommander.exe", "GalLoader.exe");
                if (!System.IO.File.Exists(str))
                {
                    str = path;
                }
            }
            if (!isReplay)
            {
                this.mListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.mListener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
                this.mListener.Listen(1);
                IPEndPoint localEndPoint = (IPEndPoint) this.mListener.LocalEndPoint;
                string arguments = "";
                if ((GPGnetSelectedGame.SelectedGame != null) && (GPGnetSelectedGame.SelectedGame.ForcedCommandLineArgs != ""))
                {
                    arguments = GPGnetSelectedGame.SelectedGame.ForcedCommandLineArgs.Replace("%STANDARDARGS", arguements).Replace("%PLAYERNAME", User.Current.Name).Replace("%GPGNET", localEndPoint.ToString());
                }
                else
                {
                    arguments = string.Format("{0} /profile {1} /gpgnet {2}", arguements, User.Current.Name, localEndPoint);
                }
                if ((GPGnetSelectedGame.SelectedGame != null) && (GPGnetSelectedGame.SelectedGame.UserForcedCommandLineArgs != ""))
                {
                    arguments = arguments + " " + GPGnetSelectedGame.SelectedGame.UserForcedCommandLineArgs;
                }
                if ((GPGnetSelectedGame.SelectedGame != null) && (GPGnetSelectedGame.SelectedGame.GameID == 2))
                {
                    arguments = arguments.Replace("/seraphim", "/uef");
                }
                ProcessStartInfo startInfo = new ProcessStartInfo(str, arguments);
                GPG.Logging.EventLog.WriteLine("Application Name: " + path, LogCategory.Get("TCP"), new object[0]);
                GPG.Logging.EventLog.WriteLine("Application arguements: " + arguments, LogCategory.Get("TCP"), new object[0]);
                startInfo.UseShellExecute = true;
                string str3 = path.Substring(0, path.LastIndexOf(@"\"));
                GPG.Logging.EventLog.WriteLine("Working Directory: " + str3, LogCategory.Get("TCP"), new object[0]);
                startInfo.WorkingDirectory = str3;
                this.mProcess = Process.Start(startInfo);
                this.mIsRunningThread = new Thread(new ThreadStart(this.CheckIsRunning));
                this.mIsRunningThread.IsBackground = true;
                this.mIsRunningThread.Start();
                if (callback == null)
                {
                    callback = delegate (IAsyncResult result) {
                        Socket socket = this.mListener.EndAccept(result);
                        lock (this.mMutex)
                        {
                            this.mSocket = socket;
                            this.mDriver = new Driver(this.mSocket);
                            this.mDriver.OnInput += new EventHandler<Driver.InputEventArgs>(this.mDriver_OnInput);
                            this.mDriver.Start();
                            this.SendMessage("Test", new object[0]);
                            int num = automatch ? 1 : 0;
                            if (GPGnetSelectedGame.ProfileName == "")
                            {
                                GPGnetSelectedGame.ProfileName = User.Current.Name;
                            }
                            if (isHost && GPGnetSelectedGame.IsSpaceSiege)
                            {
                                if (ConfigSettings.GetBool("GetSpaceSiegeHostOldWay", false))
                                {
                                    this.SendMessage("CreateLobby", new object[] { num, SupcomStdInOut.GamePort, User.Current.Name, 1 });
                                }
                                else
                                {
                                    this.SendMessage("CreateLobby", new object[] { num, SupcomStdInOut.GamePort, User.Current.Name, User.Current.ID, GPGnetSelectedGame.ProfileName });
                                }
                            }
                            else if (GPGnetSelectedGame.IsSpaceSiege)
                            {
                                if (ConfigSettings.GetBool("GetSpaceSiegeHostOldWay", false))
                                {
                                    this.SendMessage("CreateLobby", new object[] { num, SupcomStdInOut.GamePort, User.Current.Name, User.Current.ID });
                                }
                                else
                                {
                                    this.SendMessage("CreateLobby", new object[] { num, SupcomStdInOut.GamePort, User.Current.Name, User.Current.ID, GPGnetSelectedGame.ProfileName });
                                }
                            }
                            else
                            {
                                bool hasOriginal = false;
                                bool hasExpansion = false;
                                GPGnetSelectedGame.TestFactions(out hasOriginal, out hasExpansion);
                                if ((GPGnetSelectedGame.SelectedGame.GameDescription == "Forged Alliance") || (GPGnetSelectedGame.SelectedGame.GameDescription == "Forged Alliance Beta"))
                                {
                                    this.SendMessage("CreateLobby", new object[] { num, SupcomStdInOut.GamePort, User.Current.Name, User.Current.ID, Convert.ToInt32(hasOriginal) });
                                }
                                else
                                {
                                    this.SendMessage("CreateLobby", new object[] { num, SupcomStdInOut.GamePort, User.Current.Name, User.Current.ID });
                                }
                            }
                            this.CheckSpaceSiegeMessages();
                        }
                    };
                }
                this.mListener.BeginAccept(callback, null);
            }
            else
            {
                string str4 = string.Format("{0} /profile {1}", arguements, User.Current.Name);
                ProcessStartInfo info2 = new ProcessStartInfo(str, str4);
                GPG.Logging.EventLog.WriteLine("REPLAY Application Name: " + path, LogCategory.Get("TCP"), new object[0]);
                GPG.Logging.EventLog.WriteLine("REPLAY Application arguements: " + str4, LogCategory.Get("TCP"), new object[0]);
                info2.UseShellExecute = true;
                string str5 = path.Substring(0, path.LastIndexOf(@"\"));
                GPG.Logging.EventLog.WriteLine("REPLAY Working Directory: " + str5, LogCategory.Get("TCP"), new object[0]);
                info2.WorkingDirectory = str5;
                this.mProcess = Process.Start(info2);
                this.mIsRunningThread = new Thread(new ThreadStart(this.CheckIsRunning));
                this.mIsRunningThread.IsBackground = true;
                this.mIsRunningThread.Start();
            }
        }

        public bool Bind(int port, int connectionid)
        {
            this.mSupcomPort = port;
            this.mConnectionID = connectionid;
            return true;
        }

        private void CheckIsRunning()
        {
            try
            {
                while (!this.mProcess.HasExited)
                {
                    Thread.Sleep(0x3e8);
                }
            }
            catch (ThreadInterruptedException exception)
            {
                GPG.Logging.EventLog.WriteLine("TCP Check process thread interrupted.", LogCategory.Get("TCP"), new object[] { exception });
            }
            this.Close();
        }

        private void CheckSpaceSiegeMessages()
        {
        }

        private void CheckSupcomMessages()
        {
            bool hasOriginal = false;
            bool hasExpansion = false;
            GPGnetSelectedGame.TestFactions(out hasOriginal, out hasExpansion);
            this.SendMessage("HasSupcom", new object[] { Convert.ToInt32(hasOriginal) });
            this.SendMessage("HasForgedAlliance", new object[] { Convert.ToInt32(hasExpansion) });
        }

        public void Close()
        {
            GPG.Logging.EventLog.WriteLine("Closing Socket", LogCategory.Get("TCP"), new object[0]);
            lock (this.mMutex)
            {
                if (this.mDriver != null)
                {
                    this.mDriver.CloseOutput();
                }
                this.mSupcomPort = -1;
                this.mConnectionID = -1;
                this.DoExit();
            }
        }

        private void DoExit()
        {
            if (!this.mExited)
            {
                this.mExited = true;
                if (this.OnExit != null)
                {
                    this.OnExit(this, EventArgs.Empty);
                }
            }
        }

        public void ForceClose()
        {
            GPG.Logging.EventLog.WriteLine("Called Force Close", LogCategory.Get("TCP"), new object[0]);
            try
            {
                if (!this.mProcess.HasExited)
                {
                    this.mProcess.Kill();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            this.DoExit();
        }

        private void mDriver_OnInput(object sender, Driver.InputEventArgs e)
        {
            string command = e.Command;
            foreach (object obj2 in e.Args)
            {
                if (obj2 != null)
                {
                    command = command + " " + obj2.ToString();
                }
                else
                {
                    command = command + " (NULL)";
                }
            }
            GPG.Logging.EventLog.WriteLine(command, LogCategory.Get("TCP"), new object[0]);
            if (e != null)
            {
                if (this.OnReceiveMessage != null)
                {
                    if (e.Command == "ProcessNatPacket")
                    {
                        string address = (string) e.Args[0];
                        byte[] sourceArray = (byte[]) e.Args[1];
                        byte[] destinationArray = new byte[sourceArray.Length + 1];
                        Array.Copy(sourceArray, 0, destinationArray, 1, sourceArray.Length);
                        destinationArray[0] = 8;
                        NetMessage message = new NetMessage(destinationArray, NetworkUtils.ConvertAddress(address));
                        byte[] buffer3 = new byte[destinationArray.Length - 4];
                        Array.Copy(destinationArray, 4, buffer3, 0, destinationArray.Length - 4);
                        message.Buffer = buffer3;
                        this.OnReceiveMessage(message);
                    }
                    else
                    {
                        this.OnGetCommand(this, e);
                    }
                }
                else
                {
                    GPG.Logging.EventLog.WriteLine("There is nothing attached to OnReceiveMessage.", LogCategory.Get("TCP"), new object[] { e });
                }
            }
        }

        private void process_Exited(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SendMessage(string message, params object[] args)
        {
            lock (this.mMutex)
            {
                if (this.mDriver == null)
                {
                    for (int i = 0; i < ConfigSettings.GetInt("TCPNullCounter", 5); i++)
                    {
                        Thread.Sleep(10);
                        if (this.mDriver != null)
                        {
                            break;
                        }
                    }
                    if (this.mDriver == null)
                    {
                        ErrorLog.WriteLine("Unable to send a message via TCP because mDriver is still null.", new object[0]);
                        goto Label_0085;
                    }
                }
                GPG.Logging.EventLog.WriteLine("Sent Message To Game: " + message, LogCategory.Get("TCP"), args);
                this.mDriver.Send(message, args);
            Label_0085:;
            }
        }

        public void SendMessage(byte[] data, string address, int port)
        {
            this.SendMessage("SendNatPacket", new object[] { address + ":" + port.ToString(), data });
        }

        public int ConnectionID
        {
            get
            {
                return this.mConnectionID;
            }
        }

        public bool IsBound
        {
            get
            {
                return (this.mSupcomPort != -1);
            }
        }

        public int Port
        {
            get
            {
                return this.mSupcomPort;
            }
        }
    }
}

