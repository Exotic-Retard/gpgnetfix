namespace GPG.Multiplayer.Game.SupremeCommander
{
    using GPG.Logging;
    using GPG.Multiplayer.Game.Network;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SupcomStdInOut : INetConnection
    {
        private const int count = 0x5d;
        private static int mBasePort = 0x17e0;
        private int mConnectionID = -1;
        private Process mProcess;
        private bool mProcessing;
        private Thread mStdErrThread;
        private Thread mStdOutThread;
        private const int offset = 0x21;

        public event EventHandler OnExit;

        public event ReceiveMessage OnReceiveMessage;

        public SupcomStdInOut(string application, string arguements)
        {
            ParameterizedThreadStart start = new ParameterizedThreadStart(this.StartGame);
            new Thread(start) { IsBackground = true }.Start(new string[] { application, arguements });
        }

        public bool Bind(int port, int connectionid)
        {
            GamePort = port;
            this.mConnectionID = connectionid;
            return true;
        }

        public void Close()
        {
            if (this.mProcessing)
            {
                GPG.Logging.EventLog.WriteLine("Closing StdInOut", LogCategory.Get("SupcomStdInOut"), new object[0]);
                try
                {
                    throw new Exception("Stacktrace on closing stdout.");
                }
                catch (Exception exception)
                {
                    GPG.Logging.EventLog.WriteLine(exception.StackTrace, LogCategory.Get("SupcomStdInOut"), new object[0]);
                }
                this.mProcessing = false;
                this.mStdOutThread.Interrupt();
                this.mStdErrThread.Interrupt();
            }
        }

        private byte[] Decode(string data)
        {
            byte[] buffer = new byte[data.Length / 2];
            int index = 0;
            for (int i = 0; i < data.Length; i += 2)
            {
                if ((i + 1) < data.Length)
                {
                    int num3 = ((Convert.ToByte(data[i]) - 0x21) * 0x5d) + (Convert.ToByte(data[i + 1]) - 0x21);
                    buffer[index] = (byte) num3;
                    index++;
                }
            }
            return buffer;
        }

        private string Encode(byte[] data)
        {
            string str = "";
            foreach (byte num in data)
            {
                str = str + ((char) ((num / 0x5d) + 0x21)) + ((char) ((num % 0x5d) + 0x21));
            }
            return str;
        }

        public void ForceClose()
        {
            if (this.mProcessing)
            {
                GPG.Logging.EventLog.WriteLine("Called Force Close", LogCategory.Get("SupcomStdInOut"), new object[0]);
                this.mProcessing = false;
                this.mStdOutThread.Interrupt();
                this.mStdErrThread.Interrupt();
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
                if (this.OnExit != null)
                {
                    this.OnExit(this, EventArgs.Empty);
                }
            }
        }

        private void mProcess_Exited(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProcessStdErr()
        {
            while (this.mProcessing)
            {
                try
                {
                    string str = this.mProcess.StandardError.ReadLine();
                    if (str != null)
                    {
                        GPG.Logging.EventLog.WriteLine("STDERR: " + str, LogCategory.Get("SupcomStdInOut"), new object[0]);
                    }
                    else
                    {
                        try
                        {
                            Thread.Sleep(100);
                        }
                        catch (ThreadInterruptedException exception)
                        {
                            GPG.Logging.EventLog.WriteLine("The thread was woken up: " + exception.Message, LogCategory.Get("SupcomStdInOut"), new object[0]);
                        }
                    }
                    continue;
                }
                catch (ThreadInterruptedException exception2)
                {
                    GPG.Logging.EventLog.WriteLine("The thread was woken up: " + exception2.Message, LogCategory.Get("SupcomStdInOut"), new object[0]);
                    continue;
                }
            }
        }

        private void ProcessStdOut()
        {
            GPG.Logging.EventLog.WriteLine("StdOut Started", LogCategory.Get("SupcomStdInOut"), new object[0]);
            while (this.mProcessing)
            {
                int num = 0;
                try
                {
                    GPG.Logging.EventLog.WriteLine("Waiting to do a readline", LogCategory.Get("SupcomStdInOut"), new object[0]);
                    string str = this.mProcess.StandardOutput.ReadLine();
                    GPG.Logging.EventLog.WriteLine("Readline finished", LogCategory.Get("SupcomStdInOut"), new object[0]);
                    if (str != null)
                    {
                        GPG.Logging.EventLog.WriteLine("STDOUT: " + str, LogCategory.Get("SupcomStdInOut"), new object[0]);
                        num = 0;
                        NetMessage message = new NetMessage {
                            EndPoint = new IPEndPoint(IPAddress.Any, GamePort)
                        };
                        message.Buffer = new byte[0];
                        if (str.IndexOf("/NAT ") == 0)
                        {
                            string data = str.Replace("/NAT ", "");
                            byte[] buffer2 = this.Decode(data);
                            GPG.Logging.EventLog.WriteLine("Raw NAT Data: " + BitConverter.ToString(buffer2), LogCategory.Get("SupcomStdInOut"), new object[0]);
                            if (buffer2.Length > 9)
                            {
                                byte[] destinationArray = new byte[buffer2.Length - 9];
                                Array.Copy(buffer2, 9, destinationArray, 0, destinationArray.Length);
                                byte[] buffer4 = new byte[buffer2.Length - 5];
                                Array.Copy(buffer2, 5, buffer4, 0, buffer4.Length);
                                buffer4[0] = 8;
                                int port = (Convert.ToInt32(buffer2[5].ToString()) * 0x100) + Convert.ToInt32(buffer2[4].ToString());
                                string ipString = buffer2[3].ToString() + "." + buffer2[2].ToString() + "." + buffer2[1].ToString() + "." + buffer2[0].ToString();
                                GPG.Logging.EventLog.WriteLine("Raw Address: " + ipString, LogCategory.Get("SupcomStdInOut"), new object[0]);
                                message.EndPoint = new IPEndPoint(IPAddress.Parse(ipString), port);
                                GPG.Logging.EventLog.WriteLine("New Endpoint: " + message.EndPoint.ToString(), LogCategory.Get("SupcomStdInOut"), new object[0]);
                                message.Buffer = destinationArray;
                                if (buffer2[8] == 0)
                                {
                                    message.MessageType = NetMessageTypes.Nat;
                                }
                                else if (buffer2[8] == 1)
                                {
                                    message = new NetMessage(buffer4, message.EndPoint);
                                }
                                else if (buffer2[8] == 2)
                                {
                                    message = new NetMessage(buffer4, message.EndPoint);
                                }
                            }
                        }
                        else
                        {
                            message.MessageType = NetMessageTypes.GameCommand;
                            message.Text = str;
                        }
                        if (this.OnReceiveMessage != null)
                        {
                            this.OnReceiveMessage(message);
                        }
                    }
                    else
                    {
                        GPG.Logging.EventLog.WriteLine("Setting Priority...", LogCategory.Get("SupcomStdInOut"), new object[0]);
                        this.mStdOutThread.Priority = ThreadPriority.Normal;
                        Thread.Sleep(100);
                        num++;
                        if (num > 100)
                        {
                            this.mProcessing = false;
                        }
                    }
                    continue;
                }
                catch (ThreadInterruptedException exception)
                {
                    GPG.Logging.EventLog.WriteLine("The thread was woken up: " + exception.Message, LogCategory.Get("SupcomStdInOut"), new object[0]);
                    continue;
                }
                catch (Exception exception2)
                {
                    ErrorLog.WriteLine(exception2.Message + "\r\n" + exception2.StackTrace, new object[0]);
                    try
                    {
                        Thread.Sleep(100);
                    }
                    catch (ThreadInterruptedException exception3)
                    {
                        GPG.Logging.EventLog.WriteLine("The thread was woken up: " + exception3.Message, LogCategory.Get("SupcomStdInOut"), new object[0]);
                    }
                    continue;
                }
            }
            try
            {
                if (this.OnExit != null)
                {
                    this.OnExit(this, EventArgs.Empty);
                }
            }
            catch (ThreadInterruptedException exception4)
            {
                GPG.Logging.EventLog.WriteLine("The thread was woken up: " + exception4.Message, LogCategory.Get("SupcomStdInOut"), new object[0]);
            }
            GPG.Logging.EventLog.WriteLine("StdOut Finished", LogCategory.Get("SupcomStdInOut"), new object[0]);
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            throw new Exception("This method should never be needed for stdin and out.  Processing happens on those threads.");
        }

        public void SendMessage(byte[] data, string address, int port)
        {
            if (this.mProcessing)
            {
                byte[] buffer = new byte[] { (byte) (port / 0x100), (byte) (port % 0x100) };
                string str = "/NAT " + this.Encode(IPAddress.Parse(address).GetAddressBytes()) + this.Encode(buffer) + this.Encode(data);
                this.mProcess.StandardInput.WriteLine(str);
                GPG.Logging.EventLog.WriteLine("StdIn sent nat message: " + str, LogCategory.Get("StdInOut"), new object[] { address, port, data });
            }
        }

        public void SendStdIn(string data)
        {
            this.mProcess.StandardInput.WriteLine(data);
            this.mProcess.StandardInput.Flush();
            GPG.Logging.EventLog.WriteLine("STDIN: " + data, LogCategory.Get("SupcomStdInOut"), new object[0]);
        }

        private void StartGame(object objargs)
        {
            string[] strArray = objargs as string[];
            string fileName = strArray[0];
            string arguments = strArray[1];
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName, arguments);
            GPG.Logging.EventLog.WriteLine("Application Name: " + fileName, LogCategory.Get("SupcomStdInOut"), new object[0]);
            GPG.Logging.EventLog.WriteLine("Application arguements: " + arguments, LogCategory.Get("SupcomStdInOut"), new object[0]);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            string str3 = fileName.Substring(0, fileName.LastIndexOf(@"\"));
            GPG.Logging.EventLog.WriteLine("Working Directory: " + str3, LogCategory.Get("SupcomStdInOut"), new object[0]);
            startInfo.WorkingDirectory = str3;
            this.mProcess = Process.Start(startInfo);
            this.mProcess.Exited += new EventHandler(this.mProcess_Exited);
            this.mProcessing = true;
            GPG.Logging.EventLog.WriteLine("Creating out", LogCategory.Get("SupcomStdInOut"), new object[0]);
            this.mStdOutThread = new Thread(new ThreadStart(this.ProcessStdOut));
            this.mStdOutThread.Priority = ThreadPriority.Normal;
            this.mStdOutThread.IsBackground = true;
            this.mStdOutThread.Start();
            GPG.Logging.EventLog.WriteLine("Creating error", LogCategory.Get("SupcomStdInOut"), new object[0]);
            this.mStdErrThread = new Thread(new ThreadStart(this.ProcessStdErr));
            this.mStdErrThread.IsBackground = true;
            this.mStdErrThread.Start();
            GPG.Logging.EventLog.WriteLine("Finished creating StdInOut", LogCategory.Get("SupcomStdInOut"), new object[0]);
        }

        public int ConnectionID
        {
            get
            {
                return this.mConnectionID;
            }
        }

        public static int GamePort
        {
            get
            {
                int mBasePort = SupcomStdInOut.mBasePort;
                if (ConfigSettings.GetBool("AutoPort", false))
                {
                    try
                    {
                        new UdpClient(SupcomStdInOut.mBasePort).Close();
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                        mBasePort = 0;
                    }
                }
                return mBasePort;
            }
            set
            {
                mBasePort = value;
            }
        }

        public bool IsBound
        {
            get
            {
                return (this.mConnectionID != -1);
            }
        }

        public int Port
        {
            get
            {
                return GamePort;
            }
        }
    }
}

