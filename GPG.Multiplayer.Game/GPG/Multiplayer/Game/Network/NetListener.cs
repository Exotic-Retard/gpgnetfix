namespace GPG.Multiplayer.Game.Network
{
    using GPG.Logging;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class NetListener
    {
        private string mAddress;
        private AsyncCallback mAsyncCallback;
        private IPEndPoint mEndPoint;
        private int mPort;
        private bool mProcessing;
        private Thread mProcessThread;
        private Socket mSocket;
        private const int NAT_HEADER = 8;

        public event ByteMessage ByteMessageHandler;

        public event StringMessage StringMessageHandler;

        public NetListener(int hostPort)
        {
            this.mPort = hostPort;
            this.mAddress = "10.10.10.10";
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                this.mAddress = address.ToString();
            }
        }

        private void NetServer_mAsyncCallback(IAsyncResult ar)
        {
            if (this.mProcessing)
            {
                StateObject asyncState = (StateObject) ar.AsyncState;
                int length = asyncState.workSocket.EndReceive(ar);
                if (length > 0)
                {
                    byte[] destinationArray = new byte[length];
                    Array.Copy(asyncState.buffer, destinationArray, length);
                    if (asyncState.buffer[9] == 0x11)
                    {
                        string srcaddr = asyncState.buffer[12].ToString() + "." + asyncState.buffer[13].ToString() + "." + asyncState.buffer[14].ToString() + "." + asyncState.buffer[15].ToString();
                        string destaddr = asyncState.buffer[0x10].ToString() + "." + asyncState.buffer[0x11].ToString() + "." + asyncState.buffer[0x12].ToString() + "." + asyncState.buffer[0x13].ToString();
                        int srcport = (destinationArray[20] * 0x100) + destinationArray[0x15];
                        int destport = (destinationArray[0x16] * 0x100) + destinationArray[0x17];
                        string type = "Unclassified";
                        if (asyncState.buffer.Length >= 0x1d)
                        {
                            if (asyncState.buffer[0x1c] == 8)
                            {
                                type = "Nat";
                            }
                            else if ((asyncState.buffer[0x1c] == 0x31) && (asyncState.buffer[0x1d] == 0x3f))
                            {
                                type = "From Quazal";
                            }
                            else if ((asyncState.buffer[0x1c] == 0x3f) && (asyncState.buffer[0x1d] == 0x31))
                            {
                                type = "To Quazal";
                            }
                        }
                        if (this.ByteMessageHandler != null)
                        {
                            if (destinationArray.Length > 0x1c)
                            {
                                byte[] buffer2 = new byte[destinationArray.Length - 0x1c];
                                Array.Copy(destinationArray, 0x1c, buffer2, 0, destinationArray.Length - 0x1c);
                                this.ByteMessageHandler(type, srcaddr, srcport, destaddr, destport, buffer2);
                            }
                            else
                            {
                                byte[] message = new byte[0];
                                this.ByteMessageHandler(type, srcaddr, srcport, destaddr, destport, message);
                            }
                        }
                        if (this.StringMessageHandler != null)
                        {
                            this.StringMessageHandler(type, srcaddr, srcport, destaddr, destport, this.ParseUDPPacket(destinationArray));
                        }
                    }
                }
                this.SetupReceive();
            }
        }

        private string ParseUDPPacket(byte[] data)
        {
            string str = "";
            if (data.Length < 0x1c)
            {
                return "Invalid UDP Packet";
            }
            int num = (data[20] * 0x100) + data[0x15];
            int num2 = (data[0x16] * 0x100) + data[0x17];
            str = "Source: " + data[12].ToString() + "." + data[13].ToString() + "." + data[14].ToString() + "." + data[15].ToString() + ":" + num.ToString() + "\r\n";
            str = str + "Dest: " + data[0x10].ToString() + "." + data[0x11].ToString() + "." + data[0x12].ToString() + "." + data[0x13].ToString() + ":" + num2.ToString() + "\r\n";
            byte[] destinationArray = new byte[data.Length - 0x1c];
            Array.Copy(data, 0x1c, destinationArray, 0, data.Length - 0x1c);
            str = (str + "String Data: " + Encoding.ASCII.GetString(destinationArray) + "\r\n") + "Raw Data: ";
            foreach (byte num3 in destinationArray)
            {
                str = str + num3.ToString() + " ";
            }
            return str;
        }

        private void ProcessData()
        {
            this.mProcessing = true;
            this.mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            this.mSocket.Blocking = false;
            this.mAsyncCallback = new AsyncCallback(this.NetServer_mAsyncCallback);
            this.mEndPoint = new IPEndPoint(IPAddress.Any, this.mPort);
            this.mSocket.Bind(new IPEndPoint(IPAddress.Parse(this.mAddress), this.mPort));
            this.mSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AcceptConnection, 1);
            byte[] buffer3 = new byte[4];
            buffer3[0] = 1;
            byte[] optionInValue = buffer3;
            byte[] optionOutValue = new byte[4];
            int ioControlCode = -1744830463;
            this.mSocket.IOControl(ioControlCode, optionInValue, optionOutValue);
            byte num1 = optionOutValue[0];
            byte num2 = optionOutValue[1];
            byte num3 = optionOutValue[2];
            byte num4 = optionOutValue[3];
            this.SetupReceive();
            while (this.mProcessing)
            {
                Thread.Sleep(100);
            }
        }

        public bool SendMessage(byte[] data, string address, int port)
        {
            try
            {
                UdpClient client = new UdpClient(0x17e0);
                client.Send(data, data.Length, address, port);
                client.Close();
                client = null;
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine("Unable to send a UDP packed.  Reason: " + exception.Message, new object[0]);
                return false;
            }
        }

        public bool SendMessage(string message, string address, int port)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            return this.SendMessage(bytes, address, port);
        }

        private void SetupReceive()
        {
            StateObject state = new StateObject {
                workSocket = this.mSocket
            };
            try
            {
                this.mSocket.BeginReceive(state.buffer, 0, 0x1000, SocketFlags.None, this.mAsyncCallback, state);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void StartServer()
        {
            this.mProcessThread = new Thread(new ThreadStart(this.ProcessData));
            this.mProcessThread.IsBackground = true;
            this.mProcessThread.Start();
        }

        public void StopServer()
        {
            if (this.mProcessing && (this.mSocket != null))
            {
                this.mProcessing = false;
                this.mProcessThread.Join(0x3e8);
                this.mSocket.Close();
                this.mSocket = null;
            }
        }
    }
}

