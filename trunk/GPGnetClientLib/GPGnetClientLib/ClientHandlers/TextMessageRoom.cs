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
                object[] msgParams = message.GetParams();
                OnTextMessage((string)msgParams[0], (string)msgParams[1], (Credentials)msgParams[2]);
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

