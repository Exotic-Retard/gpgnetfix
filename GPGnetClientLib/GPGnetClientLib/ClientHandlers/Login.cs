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
                object[] params = message.GetParams();
                OnLogin((bool) params[0], (Credentials) params[1]);
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

