namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;

    public class QuazalServerInfo
    {
        public string AccessKey = "";
        public string Address = "";
        public string Description = "";
        public int Port = 0;

        public static void CheckWebIni(string weburl, ref List<QuazalServerInfo> servers)
        {
            Exception exception;
            try
            {
                string fileName = Application.StartupPath + @"\webservers.ini";
                try
                {
                    new WebClient().DownloadFile(weburl, fileName);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                }
                if (System.IO.File.Exists(fileName))
                {
                    QuazalServerInfo item = null;
                    StreamReader reader = new StreamReader(fileName);
                    int num = 0;
                    while (reader.Peek() >= 0)
                    {
                        string str2 = reader.ReadLine();
                        switch (num)
                        {
                            case 0:
                                item = new QuazalServerInfo();
                                item.Description = str2;
                                break;

                            case 1:
                                item.Address = str2;
                                break;

                            case 2:
                                try
                                {
                                    item.Port = Convert.ToInt32(str2);
                                }
                                catch (Exception exception2)
                                {
                                    ErrorLog.WriteLine(exception2);
                                    item.Port = 0x7684;
                                }
                                break;

                            case 3:
                            {
                                item.AccessKey = str2;
                                bool flag = false;
                                foreach (QuazalServerInfo info2 in servers)
                                {
                                    if ((info2.Description == item.Description) || ((info2.Address == item.Address) && (info2.Port == item.Port)))
                                    {
                                        flag = true;
                                        info2.Description = item.Description;
                                        info2.Address = item.Address;
                                        info2.Port = item.Port;
                                        info2.AccessKey = item.AccessKey;
                                    }
                                }
                                if (!flag)
                                {
                                    servers.Add(item);
                                }
                                break;
                            }
                        }
                        num++;
                        if (num == 5)
                        {
                            num = 0;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception exception4)
            {
                exception = exception4;
                ErrorLog.WriteLine(exception);
            }
        }

        public static List<QuazalServerInfo> GetServers()
        {
            QuazalServerInfo info;
            List<QuazalServerInfo> servers = new List<QuazalServerInfo>();
            string path = Application.StartupPath + @"\extraservers.ini";
            if (!System.IO.File.Exists(path))
            {
                path = Application.StartupPath + @"\qaservers.ini";
                if (!System.IO.File.Exists(path))
                {
                    path = Application.StartupPath + @"\servers.ini";
                }
            }
            if (System.IO.File.Exists(path))
            {
                info = null;
                StreamReader reader = new StreamReader(path);
                int num = 0;
                while (reader.Peek() >= 0)
                {
                    string str2 = reader.ReadLine();
                    switch (num)
                    {
                        case 0:
                            info = new QuazalServerInfo();
                            info.Description = str2;
                            break;

                        case 1:
                            info.Address = str2;
                            break;

                        case 2:
                            try
                            {
                                info.Port = Convert.ToInt32(str2);
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                                info.Port = 0x7684;
                            }
                            break;

                        case 3:
                            info.AccessKey = str2;
                            servers.Add(info);
                            break;
                    }
                    num++;
                    if (num == 5)
                    {
                        num = 0;
                    }
                }
                reader.Close();
            }
            else
            {
                info = new QuazalServerInfo();
                info.Description = "Retail Server";
                info.Address = "supcomlive.quazal.net";
                info.Port = 0x768e;
                info.AccessKey = "11hK6";
                servers.Add(info);
            }
            CheckWebIni("http://replay.gaspowered.com/servers.txt", ref servers);
            if (path.IndexOf("qaservers.ini") >= 0)
            {
                CheckWebIni("http://replay.gaspowered.com/qaservers.txt", ref servers);
            }
            bool flag = false;
            foreach (QuazalServerInfo curinfo in servers)
            {
                if (curinfo.Address == "supcomfabeta.quazal.net")
                {
                    flag = true;
                }
            }
            return servers;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}

