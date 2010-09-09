namespace GPG.Multiplayer.Game.Network
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class UdpConnection : BaseConnection, INetConnection
    {
        private int mConnectionID = -1;
        private bool mIsBound;
        private Thread mManagerThread;
        private EventWaitHandle mEventQueue = new EventWaitHandle(false, EventResetMode.AutoReset);
        private System.Collections.Queue mPacketQueue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
        private int mPort = -1;
        private bool mProcessing;
        private UdpClient mUdpClient;

        public event ReceiveMessage OnReceiveMessage;

        public bool Bind(int port, int connectionid)
        {
            if (this.mIsBound)
            {
                return false;
            }
            this.mIsBound = true;
            try
            {
                this.mUdpClient = new UdpClient(port);
                this.mUdpClient.Close();
                this.mUdpClient = null;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine("Unable to bind the udp socket.  Reason: " + exception.Message, new object[0]);
                return false;
            }
            this.mPort = port;
            this.mConnectionID = connectionid;
            ThreadStart start = new ThreadStart(this.Process);
            this.mManagerThread = new Thread(start);
            this.mManagerThread.Start();
            return true;
        }

        public void Close()
        {
            this.mProcessing = false;
            this.mIsBound = false;
            this.mManagerThread.Interrupt();
            if (!this.mManagerThread.Join(0x1388))
            {
                ErrorLog.WriteLine("Unable to stop the UdpConnection on port " + this.mPort.ToString() + " with id " + this.mConnectionID.ToString(), new object[0]);
            }
        }

        private void Process()
        {
            this.mProcessing = true;
            this.mUdpClient = new UdpClient(this.mPort);
            this.SetUpReceive();
            while (this.mProcessing)
            {
                mEventQueue.WaitOne();
                while (this.mPacketQueue.Count > 0)
                {
                    PacketData data = (PacketData) this.mPacketQueue.Dequeue();
                    this.mUdpClient.Send(data.data, data.data.Length, data.address, data.port);
                }
            }
            this.mUdpClient.Close();
            this.mUdpClient = null;
        }

        public void ReceiveCallback(IAsyncResult ar)
        {
            if (this.mProcessing)
            {
                UdpClient client = (ar.AsyncState as UdpState).client;
                IPEndPoint endpoint = (ar.AsyncState as UdpState).endpoint;
                byte[] receiveBytes = client.EndReceive(ar, ref endpoint);
                if (this.OnReceiveMessage != null)
                {
                    NetMessage message = new NetMessage(receiveBytes, endpoint);
                    this.OnReceiveMessage(message);
                }
                this.SetUpReceive();
            }
        }

        public void SendMessage(byte[] data, string address, int port)
        {
            if ((this.mUdpClient != null) && this.mIsBound)
            {
                PacketData data2 = new PacketData {
                    address = address,
                    data = data,
                    port = port
                };
                this.mPacketQueue.Enqueue(data2);
                this.mEventQueue.Set();
            }
        }

        private void SetUpReceive()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
            UdpState state = new UdpState {
                endpoint = point,
                client = this.mUdpClient
            };
            this.mUdpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), state);
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
                return this.mIsBound;
            }
        }

        public int Port
        {
            get
            {
                return this.mPort;
            }
        }
    }
}

