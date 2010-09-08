namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class TextMessageUser : IClientHandler
    {
        public static  event TextMessageUserResponse OnTextUserMessage;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnTextUserMessage != null)
            {
                object[] msgParams = message.GetParams();
                OnTextUserMessage((string)msgParams[0], (Credentials)msgParams[1]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.TextMessageUser;
            }
        }
    }
}

