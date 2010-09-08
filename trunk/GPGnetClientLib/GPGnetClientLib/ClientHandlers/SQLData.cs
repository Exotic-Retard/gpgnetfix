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
                object[] msgParams = message.GetParams();
                OnSqlData((List<string>)msgParams[0], (List<List<object>>)msgParams[1]);
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

