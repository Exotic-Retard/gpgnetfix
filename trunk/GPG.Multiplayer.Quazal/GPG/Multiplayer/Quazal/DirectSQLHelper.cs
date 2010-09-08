namespace GPG.Multiplayer.Quazal
{
    using GPG.Logging;
    using System;
    using System.Runtime.InteropServices;

    internal class DirectSQLHelper
    {
        [DllImport("MultiplayerBackend.dll", EntryPoint="AddQuery", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _AddQuery([MarshalAs(UnmanagedType.LPStr)] string queryname, [MarshalAs(UnmanagedType.LPStr)] string query);
        [DllImport("MultiplayerBackend.dll", EntryPoint="ExecuteCommand", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _ExecuteCommand([MarshalAs(UnmanagedType.LPStr)] string SQL);
        [DllImport("MultiplayerBackend.dll", EntryPoint="ExecuteQuery", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _ExecuteQuery([MarshalAs(UnmanagedType.LPStr)] string queryname, [MarshalAs(UnmanagedType.LPStr)] string paramvalues);
        [DllImport("MultiplayerBackend.dll", EntryPoint="ExecuteReader", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _ExecuteReader([MarshalAs(UnmanagedType.LPStr)] string SQL);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetColumns", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr _GetColumns([MarshalAs(UnmanagedType.LPStr)] string Delimiter);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetNextRow", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr _GetNextRow([MarshalAs(UnmanagedType.LPStr)] string Delimiter);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetQueryData", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr _GetQueryData([MarshalAs(UnmanagedType.LPStr)] string queryname, [MarshalAs(UnmanagedType.LPStr)] string paramvalues);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetQueryDataset", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr _GetQueryDataset([MarshalAs(UnmanagedType.LPStr)] string queryname, [MarshalAs(UnmanagedType.LPStr)] string paramvalues);
        public bool AddQuery(string queryname, string query)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.AddQuery(queryname, query);
                }
                flag = _AddQuery(queryname, query);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                flag = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool ExecuteCommand(string SQL)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.AdhocExecuteCommand(SQL);
                }
                flag = _ExecuteCommand(SQL);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool ExecuteQuery(string queryname, string[] paramlist)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                string paramvalues = "";
                string str2 = "";
                foreach (string str3 in paramlist)
                {
                    string str4;
                    if (str3 == null)
                    {
                        str4 = "";
                    }
                    else
                    {
                        str4 = str3.Replace("'", @"\'");
                    }
                    paramvalues = paramvalues + str2 + str4;
                    str2 = "|";
                }
                Lobby.IncDBCallCount(new object[] { queryname, paramvalues });
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.ExecuteQuery(queryname, paramlist);
                }
                flag = _ExecuteQuery(queryname, paramvalues);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                flag = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool ExecuteReader(string SQL)
        {
            bool flag2;
            Lobby.Lock();
            try
            {
                bool flag = true;
                foreach (string str2 in SQL.Replace(@"\;", "__SEMI__").Split(new char[] { ';' }))
                {
                    string sQL = str2.Replace("__SEMI__", ";");
                    if (sQL.Trim() != "")
                    {
                        Lobby.IncDBCallCount(new object[0]);
                        if (Lobby.sProtocol != null)
                        {
                            return Lobby.sProtocol.AdhocExecuteReader(SQL);
                        }
                        flag = _ExecuteReader(sQL) | flag;
                    }
                }
                flag2 = flag;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                flag2 = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag2;
        }

        public string GetColumns(string Delimiter)
        {
            string str;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.GetColumns(Delimiter);
                }
                str = Marshal.PtrToStringAnsi(_GetColumns(Delimiter));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                str = "";
            }
            finally
            {
                Lobby.Unlock();
            }
            return str;
        }

        public string GetNextRow(string Delimiter)
        {
            string str;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.GetNextRow(Delimiter);
                }
                str = Marshal.PtrToStringAnsi(_GetNextRow(Delimiter));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                str = "";
            }
            finally
            {
                Lobby.Unlock();
            }
            return str;
        }

        public string GetQueryDataset(string queryname)
        {
            return this.GetQueryDataset(queryname, null);
        }

        public string GetQueryDataset(string queryname, string[] paramlist)
        {
            bool useMaster = ConfigSettings.GetString("MasterQueries", " ").IndexOf(queryname + " ") >= 0;
            return this.GetQueryDataset(queryname, paramlist, useMaster);
        }

        public string GetQueryDataset(string queryname, string[] paramlist, bool useMaster)
        {
            string str6;
            Lobby.Lock();
            try
            {
                IntPtr ptr;
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.GetQueryDataset(queryname, paramlist, useMaster);
                }
                string paramvalues = "";
                string str2 = "";
                if (paramlist != null)
                {
                    foreach (string str3 in paramlist)
                    {
                        string str4;
                        if (str3 == null)
                        {
                            str4 = "";
                        }
                        else
                        {
                            str4 = str3.Replace("'", "'");
                        }
                        paramvalues = paramvalues + str2 + str4;
                        str2 = "|";
                    }
                }
                if (useMaster)
                {
                    ptr = _GetQueryData(queryname, paramvalues);
                }
                else
                {
                    ptr = _GetQueryDataset(queryname, paramvalues);
                }
                string str5 = Marshal.PtrToStringAnsi(ptr);
                Lobby.IncDBCallCount(new object[] { queryname, paramvalues });
                str6 = str5.Replace(@"\'", "'").Replace("\\\"", "\"");
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                str6 = "";
            }
            finally
            {
                Lobby.Unlock();
            }
            return str6;
        }
    }
}

