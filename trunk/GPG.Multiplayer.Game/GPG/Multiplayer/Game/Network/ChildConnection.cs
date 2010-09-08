namespace GPG.Multiplayer.Game.Network
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal class ChildConnection
    {
        public int LocalPort = -1;
        public UdpClient LocalUdp;
        private ConnectionManager mConnections;
        private System.Collections.Queue mPacketQueue = System.Collections.Queue.Synchronized(new System.Collections.Queue());
        private bool mProcessing;
        public string RemoteAddr = "Unassigned";
        public int RemotePort = -1;
        public int UserID = -1;
        public string UserName = "";

        public ChildConnection(ConnectionManager connections, string username, int uid)
        {
            this.mConnections = connections;
            this.UserName = username;
            this.UserID = uid;
        }

        private void Process()
        {
            this.mProcessing = true;
            this.LocalUdp = new UdpClient(0);
            this.LocalPort = (this.LocalUdp.Client.RemoteEndPoint as IPEndPoint).Port;
            this.SetUpReceive();
            while (this.mProcessing)
            {
                while (this.mPacketQueue.Count > 0)
                {
                    PacketData data = (PacketData) this.mPacketQueue.Dequeue();
                    this.LocalUdp.Send(data.data, data.data.Length, data.address, data.port);
                }
                Thread.Sleep(20);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient client = (ar.AsyncState as UdpState).client;
            IPEndPoint endpoint = (ar.AsyncState as UdpState).endpoint;
            byte[] data = client.EndReceive(ar, ref endpoint);
            this.mConnections.SendMessage(endpoint.Address.ToString(), endpoint.Port, data);
            this.SetUpReceive();
        }

        public void SendMessage(string address, int port, byte[] data)
        {
            PacketData data2 = new PacketData {
                address = address,
                data = data,
                port = port
            };
            this.mPacketQueue.Enqueue(data2);
        }

        private void SetUpReceive()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);
            UdpState state = new UdpState {
                endpoint = point,
                client = this.LocalUdp
            };
            this.LocalUdp.BeginReceive(new AsyncCallback(this.ReceiveCallback), state);
        }

        public bool IsConnected
        {
            get
            {
                return (((this.RemotePort != -1) && (this.RemoteAddr != "Unassigned")) && (this.LocalPort != -1));
            }
        }
    }
}

