namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class TextMessageRoom : IClientHandler
    {
        public static  event TextMessageResponse OnTextMessage;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnTextMessage != null)
            {
                object[] params = message.GetParams();
                OnTextMessage((string) params[0], (string) params[1], (Credentials) params[2]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.TextMessageRoom;
            }
        }
    }
}

