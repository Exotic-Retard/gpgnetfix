namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetClientLib;
    using GPGnetCommunicationsLib;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AdhocSQL : IClientHandler
    {
        public static  event AdhocSQLResponse OnAdhocSQL;

        public void HandleCommand(int connectionID, CommandMessage message, ClientManager manager)
        {
            if (OnAdhocSQL != null)
            {
                object[] params = message.GetParams();
                OnAdhocSQL((List<string>) params[0], (List<List<object>>) params[1]);
            }
        }

        public Commands command
        {
            get
            {
                return Commands.AdhocSQL;
            }
        }
    }
}

