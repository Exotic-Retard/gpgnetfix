namespace GPGnetClientLib.Lobby
{
    using GPGnetClientLib;
    using GPGnetClientLib.ClientHandlers;
    using GPGnetCommunicationsLib;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class SQL
    {
        private static List<string> sAdhocColumns;
        private static List<List<object>> sAdhocRows;
        private static bool sGettingAdhocSQL = false;
        private static bool sGettingSQLData = false;
        private static bool sGettingSQLExecute = false;
        private static List<string> sSQLDataColumns;
        private static List<List<object>> sSQLDataRows;
        private static int sSQLResultCount = -1;

        public static bool AdhocSQL(string SQL, out List<string> columns, out List<List<object>> rows)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.AdhocSQL
            };
            command.SetParams(new object[] { SQL });
            sGettingAdhocSQL = true;
            sAdhocRows = new List<List<object>>();
            sAdhocColumns = new List<string>();
            GPGnetClientLib.ClientHandlers.AdhocSQL.OnAdhocSQL += new AdhocSQLResponse(GPGnetClientLib.Lobby.SQL.AdhocSQL_OnAdhocSQL);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sGettingAdhocSQL && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            rows = sAdhocRows;
            columns = sAdhocColumns;
            return (sAdhocRows != null);
        }

        private static void AdhocSQL_OnAdhocSQL(List<string> columns, List<List<object>> rows)
        {
            GPGnetClientLib.ClientHandlers.AdhocSQL.OnAdhocSQL -= new AdhocSQLResponse(SQL.AdhocSQL_OnAdhocSQL);
            sAdhocColumns = columns;
            sAdhocRows = rows;
            sGettingAdhocSQL = false;
        }

        public static bool SQLData(string queryname, object[] args, out List<string> columns, out List<List<object>> rows)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.SQLData
            };
            command.SetParams(new object[] { queryname, args });
            sGettingSQLData = true;
            sSQLDataRows = new List<List<object>>();
            sSQLDataColumns = new List<string>();
            GPGnetClientLib.ClientHandlers.SQLData.OnSqlData += new SQLDataResponse(SQL.SQLData_OnSqlData);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sGettingSQLData && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            rows = sSQLDataRows;
            columns = sSQLDataColumns;
            return (sSQLDataRows != null);
        }

        private static void SQLData_OnSqlData(List<string> columns, List<List<object>> rows)
        {
            GPGnetClientLib.ClientHandlers.SQLData.OnSqlData -= new SQLDataResponse(SQL.SQLData_OnSqlData);
            sSQLDataColumns = columns;
            sSQLDataRows = rows;
            sGettingSQLData = false;
        }

        public static bool SQLExecute(string queryname, object[] args, out int rowcount)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.SQLExecute
            };
            command.SetParams(new object[] { queryname, args });
            sGettingSQLExecute = true;
            sSQLResultCount = -1;
            GPGnetClientLib.ClientHandlers.SQLExecute.OnSQLExecute += new SQLExecuteResponse(SQL.SQLExecute_OnSQLExecute);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sGettingSQLExecute && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            rowcount = sSQLResultCount;
            return (sSQLResultCount != -1);
        }

        private static void SQLExecute_OnSQLExecute(int rowcount)
        {
            GPGnetClientLib.ClientHandlers.SQLExecute.OnSQLExecute -= new SQLExecuteResponse(SQL.SQLExecute_OnSQLExecute);
            sSQLResultCount = rowcount;
            sGettingSQLExecute = false;
        }
    }
}

