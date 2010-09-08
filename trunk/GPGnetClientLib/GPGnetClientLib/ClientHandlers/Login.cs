namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class Login : IClientHandler
    {
        public static  event LoginResponse OnLogin;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnLogin != null)
            {
                object[] msgParams = message.GetParams();
                OnLogin((bool)msgParams[0], (Credentials)msgParams[1]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.Login;
            }
        }
    }
}

