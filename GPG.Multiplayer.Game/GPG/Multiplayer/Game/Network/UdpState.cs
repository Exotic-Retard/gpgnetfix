namespace GPG.Multiplayer.Game.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class UdpState
    {
        public UdpClient client;
        public IPEndPoint endpoint;
    }
}

