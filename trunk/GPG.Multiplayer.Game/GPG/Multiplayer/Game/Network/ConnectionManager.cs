namespace GPG.Multiplayer.Game.Network
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    public class ConnectionManager : BaseConnection
    {
        private Hashtable mConnections;
        private Thread mManagerThread;
        private string mName;
        private System.Collections.Queue mPacketQueue;
        private EventWaitHandle mEventQueue;
        private int mPort;
        private bool mProcessing;
        private UdpClient mRemoteUdp;
        private int mUserID;

        public ConnectionManager(string name, int uid) : this(name, uid, 0x17e0)
        {
        }

        public ConnectionManager(string name, int uid, int port)
        {
            this.mName = "";
            this.mUserID = -1;
            this.mConnections = Hashtable.Synchronized(new Hashtable());
            this.mPacketQueue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
            this.mEventQueue = new EventWaitHandle(false, EventResetMode.AutoReset);
            this.mPort = port;
            this.mName = name;
            this.mUserID = uid;
            ThreadStart start = new ThreadStart(this.Process);
            this.mManagerThread = new Thread(start);
            this.mManagerThread.Start();
        }

        private bool CheckNatMessage(byte[] data, IPEndPoint endpoint)
        {
            if (((data.Length > 4) && (data[0] == 8)) && ((data[1] == 0) && (data[2] == 0)))
            {
                EventLog.WriteLine("Got a nat message from: " + endpoint.ToString(), new object[0]);
                return true;
            }
            return false;
        }

        public void Close()
        {
            this.mProcessing = false;
            this.mManagerThread.Join();
        }

        private byte[] Packdata(byte command, params string[] args)
        {
            int num = 4;
            foreach (string str in args)
            {
                num = (num + str.Length) + 1;
            }
            num++;
            byte[] array = new byte[num];
            array[0] = command;
            array[1] = 0;
            array[2] = 0;
            array[3] = 0;
            int index = 4;
            foreach (string str2 in args)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(str2);
                bytes.CopyTo(array, index);
                index += bytes.Length;
                array[index] = 3;
                index++;
            }
            array[index] = 4;
            return null;
        }

        private void Process()
        {
            this.mProcessing = true;
            this.mRemoteUdp = new UdpClient(this.mPort);
            this.SetUpReceive();
            while (this.mProcessing)
            {
                this.mEventQueue.WaitOne();
                while (this.mPacketQueue.Count > 0)
                {
                    PacketData data = (PacketData) this.mPacketQueue.Dequeue();
                    this.mRemoteUdp.Send(data.data, data.data.Length, data.address, data.port);
                }
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient client = (ar.AsyncState as UdpState).client;
            IPEndPoint endpoint = (ar.AsyncState as UdpState).endpoint;
            byte[] data = client.EndReceive(ar, ref endpoint);
            if ((data != null) && (data.Length > 0))
            {
                this.CheckNatMessage(data, endpoint);
            }
            this.SetUpReceive();
        }

        private void SendHello(IPEndPoint endpoint)
        {
            this.SendMessage(endpoint.Address.ToString(), endpoint.Port, null);
        }

        public void SendMessage(string address, int port, byte[] data)
        {
            PacketData data2 = new PacketData {
                address = address,
                data = data,
                port = port
            };
            this.mPacketQueue.Enqueue(data2);
            this.mEventQueue.Set();
        }

        private void SetUpReceive()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
            UdpState state = new UdpState {
                endpoint = point,
                client = this.mRemoteUdp
            };
            this.mRemoteUdp.BeginReceive(new AsyncCallback(this.ReceiveCallback), state);
        }
    }
}

