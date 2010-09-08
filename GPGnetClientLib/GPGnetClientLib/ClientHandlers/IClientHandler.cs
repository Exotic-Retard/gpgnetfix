namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;

    public interface IClientHandler
    {
        void HandleCommand(int connectionID, CommandMessage message, ClientManager manager);

        Commands command { get; }
    }
}

