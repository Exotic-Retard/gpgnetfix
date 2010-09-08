namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SQLData : IClientHandler
    {
        public static  event SQLDataResponse OnSqlData;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnSqlData != null)
            {
                object[] params = message.GetParams();
                OnSqlData((List<string>) params[0], (List<List<object>>) params[1]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.SQLData;
            }
        }
    }
}

