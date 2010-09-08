namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class EmailUsername : IClientHandler
    {
        public static  event EmailUsernameResponse OnEmailUsername;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnEmailUsername != null)
            {
                object[] params = message.GetParams();
                OnEmailUsername((bool) params[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.EmailUsername;
            }
        }
    }
}

