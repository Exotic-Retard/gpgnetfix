namespace GPGnetClientLib
{
    using GPGnetCommunicationsLib;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class UDPStunClient
    {
        private int _stunServerPort1 = 0x2390;
        private int _stunServerPort2 = 0x2391;
        private IPEndPoint epStun1;
        private IPEndPoint localEndPoint;
        private UdpClient mClient;
        private byte[] mNatHeader;
        private string mRemoteHost = string.Empty;

        public event StunResults OnStunResults;

        public UDPStunClient()
        {
            byte[] buffer = new byte[4];
            buffer[0] = 8;
            this.mNatHeader = buffer;
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            UdpClient client = ((UDPState) ar.AsyncState).Client;
            IPEndPoint endpoint = ((UDPState) ar.AsyncState).Endpoint;
            byte[] sourceArray = client.EndReceive(ar, ref endpoint);
            if ((((sourceArray.Length == 10) && (sourceArray[0] == 8)) && ((sourceArray[1] == 0) && (sourceArray[2] == 0))) && (sourceArray[3] == 0))
            {
                if (this.epStun1 == null)
                {
                    byte[] destinationArray = new byte[4];
                    Array.Copy(sourceArray, 4, destinationArray, 0, 4);
                    IPAddress address = new IPAddress(destinationArray);
                    int port = BitConverter.ToUInt16(sourceArray, 8);
                    this.epStun1 = new IPEndPoint(address, port);
                    UDPState state = new UDPState {
                        Client = this.mClient,
                        Endpoint = this.localEndPoint
                    };
                    this.mClient.BeginReceive(new AsyncCallback(this.AcceptCallback), state);
                    this.mClient.Send(this.mNatHeader, 4, this.mRemoteHost, this._stunServerPort2);
                }
                else
                {
                    byte[] buffer3 = new byte[4];
                    Array.Copy(sourceArray, 4, buffer3, 0, 4);
                    IPAddress address2 = new IPAddress(buffer3);
                    int num2 = BitConverter.ToUInt16(sourceArray, 8);
                    IPEndPoint point2 = new IPEndPoint(address2, num2);
                    this.mClient.Close();
                    if (this.OnStunResults != null)
                    {
                        this.OnStunResults(this.epStun1, point2);
                    }
                }
            }
        }

        public void CheckNAT(int port, string remotehost)
        {
            try
            {
                this.mRemoteHost = remotehost;
                IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
                this.mClient = new UdpClient(localEP);
                UDPState state = new UDPState {
                    Client = this.mClient,
                    Endpoint = localEP
                };
                this.mClient.BeginReceive(new AsyncCallback(this.AcceptCallback), state);
                this.mClient.Send(this.mNatHeader, 4, remotehost, this._stunServerPort1);
            }
            catch (Exception)
            {
            }
        }
    }
}

