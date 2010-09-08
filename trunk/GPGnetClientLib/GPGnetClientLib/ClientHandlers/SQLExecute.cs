namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public class SQLExecute : IClientHandler
    {
        public static  event SQLExecuteResponse OnSQLExecute;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnSQLExecute != null)
            {
                object[] params = message.GetParams();
                OnSQLExecute((int) params[0]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.SQLExecute;
            }
        }
    }
}

