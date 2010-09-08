namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    internal class JoinRoom : IClientHandler
    {
        public static  event JoinGatheringResponse OnJoinGathering;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnJoinGathering != null)
            {
                object[] msgParams = message.GetParams();
                OnJoinGathering((bool)msgParams[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.JoinRoom;
            }
        }
    }
}

