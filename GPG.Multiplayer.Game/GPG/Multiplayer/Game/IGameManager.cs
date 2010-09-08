namespace GPG.Multiplayer.Game
{
    using GPG.Multiplayer.Game.Network;
    using System;

    public interface IGameManager
    {
        string GetGameLocation();
        bool HostGame(bool automatch, string gameName);
        bool JoinGame(bool automatch, string gameName, string address, int port, string hostname);
        void ReceiveMessage(NetMessage message);
    }
}

