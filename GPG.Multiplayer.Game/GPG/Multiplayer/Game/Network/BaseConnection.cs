namespace GPG.Multiplayer.Game.Network
{
    using System;

    public abstract class BaseConnection
    {
        public const byte GPG_BINARY_MSG = 2;
        public const byte GPG_TEXT_MSG = 1;
        public const byte NAT_HEADER = 8;
        public const byte QUAZAL_NAT_MSG = 0;
        public const int RETRY_DELAY = 500;

        protected BaseConnection()
        {
        }

        public void SendMessage(NetMessage message)
        {
            INetConnection connection = this as INetConnection;
            if (connection == null)
            {
                throw new InvalidOperationException("This connection did not impliment INetConnection and you cannot use this call.");
            }
            connection.SendMessage(message.FormatMessage(), message.EndPoint.Address.ToString(), message.EndPoint.Port);
        }
    }
}

