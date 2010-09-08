namespace GPG.Multiplayer.Game.Network
{
    using System;
    using System.Net.Sockets;
    using System.Text;

    public class StateObject
    {
        public byte[] buffer = new byte[0x1000];
        public const int BufferSize = 0x1000;
        public bool connected;
        public string id = string.Empty;
        public Socket partnerSocket;
        public StringBuilder sb = new StringBuilder();
        public DateTime TimeStamp;
        public Socket workSocket;
    }
}

