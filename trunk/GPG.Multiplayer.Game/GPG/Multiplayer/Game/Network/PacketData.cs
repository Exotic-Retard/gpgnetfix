namespace GPG.Multiplayer.Game.Network
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PacketData
    {
        public string address;
        public int port;
        public byte[] data;
    }
}

