﻿namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class ResetPassword : IClientHandler
    {
        public static  event ResetPasswordResponse OnResetPassword;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnResetPassword != null)
            {
                object[] msgParams = message.GetParams();
                OnResetPassword((bool)msgParams[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.ResetPassword;
            }
        }
    }
}

