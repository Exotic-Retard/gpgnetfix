namespace GPG.Multiplayer.Game.Network
{
    using System;
    using System.Runtime.CompilerServices;

    public interface INetConnection
    {
        event ReceiveMessage OnReceiveMessage;

        bool Bind(int port, int connectionid);
        void Close();
        void ReceiveCallback(IAsyncResult ar);
        void SendMessage(byte[] data, string address, int port);

        int ConnectionID { get; }

        bool IsBound { get; }

        int Port { get; }
    }
}

