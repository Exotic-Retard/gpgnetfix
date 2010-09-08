namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class CreateLogin : IClientHandler
    {
        public static  event CreateLoginResponse OnCreateLogin;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnCreateLogin != null)
            {
                object[] params = message.GetParams();
                OnCreateLogin((bool) params[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.CreateLogin;
            }
        }
    }
}

