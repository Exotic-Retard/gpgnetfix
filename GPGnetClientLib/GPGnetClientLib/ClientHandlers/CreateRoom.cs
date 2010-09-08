namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class CreateRoom : IClientHandler
    {
        public static  event CreateGatheringResponse OnCreateGathering;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnCreateGathering != null)
            {
                object[] msgParams = message.GetParams();
                OnCreateGathering((bool)msgParams[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.CreateRoom;
            }
        }
    }
}

