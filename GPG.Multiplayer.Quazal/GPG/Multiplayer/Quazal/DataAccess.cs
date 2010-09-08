namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Threading;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class DataAccess
    {
        private static int _ExecuteCount = 0;
        private static int _SelectCount = 0;
        private static object mSafeMutex = new object();
        private static Hashtable mSafeResult = Hashtable.Synchronized(new Hashtable());
        private static string[] QuazalTables = new string[0];
        public static int sMainThreadID = -1;

        public static bool AdhocQuery(string sql, out List<string> columns, out List<List<string>> data)
        {
            string delimiter = '\x0003'.ToString();
            data = new List<List<string>>();
            columns = new List<string>();
            if (!Lobby.GetLobby().directSQLHelper.ExecuteReader(sql))
            {
                return false;
            }
            columns.AddRange(Lobby.GetLobby().directSQLHelper.GetColumns(delimiter).Split(new char[] { '\x0003' }));
            for (string str2 = Lobby.GetLobby().directSQLHelper.GetNextRow(delimiter); str2 != ""; str2 = Lobby.GetLobby().directSQLHelper.GetNextRow(delimiter))
            {
                List<string> item = new List<string>();
                item.AddRange(str2.Split(new char[] { '\x0003' }));
                data.Add(item);
            }
            return true;
        }

        private static bool CheckTable(string checktable)
        {
            foreach (string str in QuazalTables)
            {
                if (checktable.ToUpper() == str.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ExecuteQuery(string queryName, params object[] args)
        {
            string[] strArray = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] != null)
                {
                    if (args[i] is bool)
                    {
                        if ((bool) args[i])
                        {
                            strArray[i] = "1";
                        }
                        else
                        {
                            strArray[i] = "0";
                        }
                    }
                    else if (args[i] is DateTime)
                    {
                        strArray[i] = FormatDate((DateTime) args[i]);
                    }
                    else
                    {
                        strArray[i] = args[i].ToString();
                    }
                }
            }
            string str = "";
            for (int j = 0; j < strArray.Length; j++)
            {
                str = str + strArray[j] + ",";
            }
            EventLog.WriteLine("DB ACCESS: {0}({1})", new object[] { queryName, str.TrimEnd(new char[] { ',' }) });
            _ExecuteCount++;
            return Lobby.GetLobby().directSQLHelper.ExecuteQuery(queryName, FormatArgs(strArray));
        }

        public static string[] FormatArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] != null)
                {
                    args[i] = args[i].ToString().Replace("'", "&squote");
                    args[i] = args[i].ToString().Replace(";", "&semi");
                    args[i] = args[i].ToString().Replace("\"", "&quote");
                }
            }
            return args;
        }

        public static int FormatBool(bool value)
        {
            if (value)
            {
                return 1;
            }
            return 0;
        }

        public static int FormatBool(bool? value)
        {
            if (value.HasValue)
            {
                return FormatBool(value.Value);
            }
            return -1;
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static bool GetBool(string queryName, params object[] args)
        {
            try
            {
                string str = GetQueryDataSafe(queryName, args)[0][0];
                return (Convert.ToInt32(str) > 0);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public static int GetCount(string queryName, params object[] args)
        {
            return GetNumber(queryName, args);
        }

        public static ArrayList GetLogData()
        {
            return Lobby.DBCalls;
        }

        public static int GetNumber(string queryName, params object[] args)
        {
            try
            {
                DataList queryDataSafe = GetQueryDataSafe(queryName, args);
                if (queryDataSafe.Count > 0)
                {
                    string str = queryDataSafe[0][0];
                    return Convert.ToInt32(str);
                }
                return -1;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return -1;
            }
        }

        public static type GetObject<type>(string queryName, params object[] args) where type: MappedObject
        {
            MappedObjectList<type> objects = GetObjects<type>(queryName, args);
            if (objects.Count > 0)
            {
                return objects[0];
            }
            return default(type);
        }

        public static MappedObjectList<type> GetObjects<type>(string queryName, params object[] args) where type: MappedObject
        {
            return new MappedObjectList<type>(GetQueryDataSafe(queryName, args));
        }

        public static DataList GetQueryData(string queryName, params object[] args)
        {
            return GetQueryDataSet(queryName, null, args);
        }

        public static DataList GetQueryDataSafe(string queryName, params object[] args)
        {
            if (Thread.CurrentThread.ManagedThreadId == ThreadQueue.QueueThreadID)
            {
                return GetQueryData(queryName, args);
            }
            string key = Guid.NewGuid().ToString();
            object signal = new object();
            lock (signal)
            {
                ThreadQueue.QueueUserWorkItem(delegate(object o)
                {
                    object[] objArray = o as object[];
                    mSafeResult.Add(objArray[1].ToString(), GetQueryData(objArray[0].ToString(), objArray[2] as object[]));
                    lock (signal)
                    {
                        Monitor.Pulse(signal);
                    }
                }, new object[] { queryName, key, args });
                if (!Monitor.Wait(signal, ConfigSettings.GetInt("SafeTimeout", 200) * 100))
                    return null;
                return (mSafeResult[key] as DataList);
            }
        }

        public static DataList GetQueryDataSet(string queryName, bool? useMaster, params object[] args)
        {
            string[] strArray;
            if ((args == null) || (args.Length == 0))
            {
                strArray = new string[0];
            }
            else
            {
                strArray = new string[args.Length];
                for (int j = 0; j < args.Length; j++)
                {
                    if (args[j] != null)
                    {
                        if (args[j] is bool)
                        {
                            if ((bool) args[j])
                            {
                                strArray[j] = "1";
                            }
                            else
                            {
                                strArray[j] = "0";
                            }
                        }
                        else if (args[j] is DateTime)
                        {
                            strArray[j] = FormatDate((DateTime) args[j]);
                        }
                        else
                        {
                            strArray[j] = args[j].ToString();
                        }
                    }
                }
            }
            string arg = "";
            if (!useMaster.HasValue)
            {
                arg = Lobby.GetLobby().directSQLHelper.GetQueryDataset(queryName, FormatArgs(strArray));
            }
            else if (useMaster == true)
            {
                arg = Lobby.GetLobby().directSQLHelper.GetQueryDataset(queryName, FormatArgs(strArray), true);
            }
            else
            {
                arg = Lobby.GetLobby().directSQLHelper.GetQueryDataset(queryName, FormatArgs(strArray), false);
            }
            arg = UnFormatData(arg);
            _SelectCount++;
            string str2 = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                str2 = str2 + strArray[i] + ",";
            }
            EventLog.WriteLine("DB ACCESS: {0}({1})", new object[] { queryName, str2.TrimEnd(new char[] { ',' }) });
            if ((arg != null) && (arg.Length >= 1))
            {
                return new DataList(arg.Replace(@"\r", "\r").Replace(@"\n", "\n"));
            }
            return new DataList();
        }

        public static string GetServerData()
        {
            string str = "";
            List<string> columns = null;
            List<List<string>> data = null;
            AdhocQuery("show tables;", out columns, out data);
            foreach (List<string> list3 in data)
            {
                string str2 = "";
                string str3 = "";
                string str4 = "";
                if (!CheckTable(list3[0]))
                {
                    str = str + "DROP TABLE IF EXISTS " + list3[0] + ";\r\n";
                    str = str + "CREATE TABLE " + list3[0] + "\r\n(";
                    List<string> list4 = null;
                    List<List<string>> list5 = null;
                    AdhocQuery("explain " + list3[0], out list4, out list5);
                    foreach (List<string> list6 in list5)
                    {
                        string str8 = str;
                        str = str8 + str2 + "\r\n  " + list6[0] + " " + list6[1];
                        if (list6[2] == "YES")
                        {
                            str = str + " NULL";
                        }
                        else
                        {
                            str = str + " NOT NULL";
                            if (list6[4] != "None")
                            {
                                str = str + " DEFAULT ";
                                if (list6[1].ToUpper().IndexOf("VARCHAR") >= 0)
                                {
                                    str = str + "'";
                                }
                                str = str + list6[4];
                                if (list6[1].ToUpper().IndexOf("VARCHAR") >= 0)
                                {
                                    str = str + "'";
                                }
                            }
                        }
                        str = str + " " + list6[5].ToUpper();
                        if (list6[3] == "PRI")
                        {
                            str3 = str3 + str4 + list6[0];
                            str4 = ", ";
                        }
                        str2 = ",";
                    }
                    if (str3 != "")
                    {
                        string str9 = str;
                        str = str9 + str2 + "\r\n  PRIMARY KEY(" + str3 + ")";
                    }
                    str = str + "\r\n);\r\n\r\n";
                    List<string> list7 = null;
                    List<List<string>> list8 = null;
                    AdhocQuery("SHOW INDEX FROM " + list3[0], out list7, out list8);
                    if (list8.Count > 0)
                    {
                        string str5 = "";
                        string str6 = "ALTER TABLE " + list3[0] + " ADD ";
                        string str7 = "";
                        bool flag = false;
                        foreach (List<string> list9 in list8)
                        {
                            if (list9[2] == "PRIMARY")
                            {
                                continue;
                            }
                            flag = true;
                            if (str5 != list9[2])
                            {
                                if (str5 != "")
                                {
                                    str = str + str6 + ");\r\n";
                                    str6 = "ALTER TABLE " + list3[0] + " ADD ";
                                    str7 = "";
                                }
                                if (list9[1] == "0")
                                {
                                    str6 = str6 + "UNIQUE INDEX " + list9[2] + "(";
                                }
                                else
                                {
                                    str6 = str6 + "INDEX " + list9[2] + "(";
                                }
                                str5 = list9[2];
                            }
                            str6 = str6 + str7 + list9[4];
                            str7 = ",";
                        }
                        if (flag)
                        {
                            str = str + str6.Replace("(,", "(") + ");\r\n\r\n";
                        }
                    }
                }
            }
            List<string> list10 = null;
            List<List<string>> list11 = null;
            AdhocQuery("SELECT query_name, query FROM queries ORDER BY query_name;", out list10, out list11);
            foreach (List<string> list12 in list11)
            {
                string str10 = str;
                str = str10 + "REPLACE INTO queries (query_name, query) VALUES (\"" + list12[0] + "\", \"" + list12[1].Replace("\r\n", " ").Replace("\n", " ") + "\");\r\n";
            }
            str = str + "\r\n\r\n";
            List<string> list13 = null;
            List<List<string>> list14 = null;
            AdhocQuery("SELECT id, url FROM url_info ORDER BY id;", out list13, out list14);
            foreach (List<string> list15 in list14)
            {
                string str11 = str;
                str = str11 + "REPLACE INTO url_info (id, url) VALUES (\"" + list15[0] + "\", \"" + list15[1] + "\");\r\n";
            }
            str = str + "\r\n\r\n";
            List<string> list16 = null;
            List<List<string>> list17 = null;
            AdhocQuery("SELECT feedback_category_id, description, sort_order FROM feedback_category ORDER BY feedback_category_id;", out list16, out list17);
            foreach (List<string> list18 in list17)
            {
                string str12 = str;
                str = str12 + "REPLACE INTO feedback_category (feedback_category_id, description, sort_order) VALUES (\"" + list18[0] + "\", \"" + list18[1] + "\", \"" + list18[2] + "\");\r\n";
            }
            str = str + "\r\n\r\n";
            List<string> list19 = null;
            List<List<string>> list20 = null;
            AdhocQuery("SELECT config_key, value FROM config_settings ORDER BY config_key;", out list19, out list20);
            foreach (List<string> list21 in list20)
            {
                string str13 = str;
                str = str13 + "REPLACE INTO config_settings (config_key, value) VALUES (\"" + list21[0] + "\", \"" + list21[1] + "\");\r\n";
            }
            return ((((((str + "\r\n\r\n") + GetTableSQL("gpgnet_game", "SELECT * FROM gpgnet_game ORDER BY gpgnet_game_id") + GetTableSQL("game_version", "SELECT * FROM game_version ORDER BY game_id")) + GetTableSQL("content", "SELECT * FROM content ORDER BY content_id") + GetTableSQL("content_type", "SELECT * FROM content_type ORDER BY content_type_id")) + GetTableSQL("persistent_room", "SELECT * FROM persistent_room WHERE clan_id = -1 ORDER BY persistent_room_id") + GetTableSQL("avatar", "SELECT * FROM avatar ORDER BY avatar_id")) + GetTableSQL("award", "SELECT * FROM award ORDER BY award_id") + GetTableSQL("award_set", "SELECT * FROM award_set ORDER BY award_set_id")) + GetTableSQL("award_category", "SELECT * FROM award_category ORDER BY award_category_id") + GetTableSQL("custom_replay_list", "SELECT * FROM custom_replay_list order by replay_list_id"));
        }

        public static string GetString(string queryName, params object[] args)
        {
            try
            {
                DataList queryDataSafe = GetQueryDataSafe(queryName, args);
                if (queryDataSafe.Count > 0)
                {
                    return queryDataSafe[0][0];
                }
                return null;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        private static string GetTableSQL(string tablename, string sql)
        {
            string str = "";
            List<string> columns = null;
            List<List<string>> data = null;
            AdhocQuery(sql, out columns, out data);
            string str2 = "REPLACE INTO " + tablename + "(";
            string str3 = "";
            foreach (string str4 in columns)
            {
                str2 = str2 + str3 + str4;
                str3 = ", ";
            }
            str2 = str2 + ") VALUES (";
            foreach (List<string> list3 in data)
            {
                string str5 = str2;
                str3 = "";
                foreach (string str6 in list3)
                {
                    double result = -1.0;
                    if (double.TryParse(str6, out result))
                    {
                        str5 = str5 + str3 + str6;
                    }
                    else
                    {
                        str5 = str5 + str3 + "'" + str6.Replace("'", @"\'") + "'";
                    }
                    str3 = ", ";
                }
                str5 = str5.Replace("'None'", "null") + ");";
                str = str + str5 + "\r\n";
            }
            return (str + "\r\n\r\n");
        }

        public static void QueueExecuteQuery(string queryname, params object[] args)
        {
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { queryname, args });
        }

        public static bool SaveQuery(string queryname, string query)
        {
            return Lobby.GetLobby().directSQLHelper.AddQuery(queryname, query);
        }

        public static string SubmitFile(string subpath, string file, byte[] data)
        {
            return "";
        }

        public static bool TryGetObject<type>(string queryName, out type obj, params object[] args) where type: MappedObject
        {
            MappedObjectList<type> objects = GetObjects<type>(queryName, args);
            if (objects.Count > 0)
            {
                obj = objects[0];
                return true;
            }
            obj = default(type);
            return false;
        }

        public static bool TryGetObjects<type>(string queryName, out MappedObjectList<type> data, params object[] args) where type: MappedObject
        {
            data = GetObjects<type>(queryName, args);
            return ((data != null) && (data.Count > 0));
        }

        public static string UnFormatData(string arg)
        {
            string str = arg;
            return str.ToString().Replace("&squote", "'").Replace("&semi", ";").Replace("&quote", "\"");
        }

        public static bool ValidateString(string text)
        {
            return ((!text.Contains(";") && !text.Contains("'")) && !text.Contains("\""));
        }

        public static int ExecuteCount
        {
            get
            {
                return _ExecuteCount;
            }
        }

        public static bool InvokeRequired
        {
            get
            {
                return (Thread.CurrentThread.ManagedThreadId != ThreadQueue.QueueThreadID);
            }
        }

        public static int SelectCount
        {
            get
            {
                return _SelectCount;
            }
        }
    }
}

