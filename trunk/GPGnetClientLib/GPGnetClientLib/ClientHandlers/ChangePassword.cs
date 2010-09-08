namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class ChangePassword : IClientHandler
    {
        public static  event ChangePasswordResponse OnChangePassword;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnChangePassword != null)
            {
                object[] msgParams = message.GetParams();
                OnChangePassword((bool)msgParams[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.ChangePassword;
            }
        }
    }
}

