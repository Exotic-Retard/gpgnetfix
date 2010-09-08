namespace GPG.Multiplayer.Client.Games
{
    using System;

    public interface IGameLobby
    {
        void Close();
        void RecieveMessage(string senderName, int senderId, uint messageType, string[] args);
    }
}

