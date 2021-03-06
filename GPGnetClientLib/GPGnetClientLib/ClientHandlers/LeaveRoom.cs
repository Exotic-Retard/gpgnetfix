﻿namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    internal class LeaveRoom : IClientHandler
    {
        public static  event LeaveGatheringResponse OnLeaveGathering;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnLeaveGathering != null)
            {
                object[] msgParams = message.GetParams();
                OnLeaveGathering((bool)msgParams[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.LeaveRoom;
            }
        }
    }
}

