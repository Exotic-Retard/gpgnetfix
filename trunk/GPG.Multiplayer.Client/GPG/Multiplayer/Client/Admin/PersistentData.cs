namespace GPG.Multiplayer.Client.Admin
{
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Plugin;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PersistentData : IPersistentData
    {
        public event GetMessage OnGetMessage;

        public PersistentData()
        {
            Messaging.MessageRecieved += new MessageEventHandler(this.Messaging_MessageRecieved);
            Messaging.CommandRecieved += new MessageEventHandler(this.Messaging_CommandRecieved);
        }

        public List<List<string>> AdhocData(string query)
        {
            List<string> columns = new List<string>();
            return this.AdhocData(query, out columns);
        }

        public List<List<string>> AdhocData(string query, out List<string> columns)
        {
            List<List<string>> data = new List<List<string>>();
            DataAccess.AdhocQuery(query, out columns, out data);
            return data;
        }

        public object[] CustomCommand(string commandname, params object[] data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Execute(string queryName, params object[] data)
        {
            if (DataAccess.ExecuteQuery(queryName, data))
            {
                return 1;
            }
            return 0;
        }

        public List<List<string>> GetData(string queryName, params object[] data)
        {
            List<List<string>> list = new List<List<string>>();
            DataList queryData = DataAccess.GetQueryData(queryName, data);
            foreach (DataRecord record in queryData)
            {
                List<string> item = new List<string>();
                for (int i = 0; i < record.Count; i++)
                {
                    item.Add(record[i]);
                }
                list.Add(item);
            }
            return list;
        }

        public List<string> GetRow(string queryName, params object[] data)
        {
            List<string> list = new List<string>();
            DataList queryData = DataAccess.GetQueryData(queryName, data);
            foreach (DataRecord record in queryData)
            {
                for (int i = 0; i < record.Count; i++)
                {
                    list.Add(record[i]);
                }
                return list;
            }
            return list;
        }

        private void Messaging_CommandRecieved(MessageEventArgs e)
        {
            try
            {
                if (this.OnGetMessage != null)
                {
                    int num2;
                    string str;
                    if (e.Command == "CUST")
                    {
                        int num = Convert.ToInt32(e.CommandArgs[0]);
                        num2 = Convert.ToInt32(e.CommandArgs[1]);
                        str = e.CommandArgs[2];
                        string str2 = e.CommandArgs[3];
                        switch (num)
                        {
                            case 0x21:
                            case 0x22:
                                this.OnGetMessage(str, "CUSTOM", new object[] { num.ToString(), num2, str2 });
                                break;
                        }
                    }
                    else if (e.Command == "NOTIFYJOIN")
                    {
                        num2 = Convert.ToInt32(e.CommandArgs[0]);
                        str = e.CommandArgs[1];
                        this.OnGetMessage(str, "NOTIFYJOIN", new object[] { num2 });
                    }
                    else if (e.Command == "NOTIFYEXIT")
                    {
                        num2 = Convert.ToInt32(e.CommandArgs[0]);
                        str = e.CommandArgs[1];
                        this.OnGetMessage(str, "NOTIFYEXIT", new object[] { num2 });
                    }
                }
            }
            catch
            {
            }
        }

        private void Messaging_MessageRecieved(MessageEventArgs e)
        {
            if (!e.IsCommand && (this.OnGetMessage != null))
            {
                string[] strArray = e.Message.Split(" ".ToCharArray(), 2);
                if (strArray.Length == 2)
                {
                    this.OnGetMessage(strArray[0].Replace("<", "").Replace(">", ""), "CHAT", new object[] { strArray[1] });
                }
            }
        }

        public bool PlayGame(string gamename, string gametype)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool SendMessage(string targetPlayerName, string command, params object[] args)
        {
            try
            {
                CustomCommands cmd = (CustomCommands) Enum.Parse(typeof(CustomCommands), command, true);
                Messaging.SendCustomCommand(targetPlayerName, cmd, args);
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }
    }
}

