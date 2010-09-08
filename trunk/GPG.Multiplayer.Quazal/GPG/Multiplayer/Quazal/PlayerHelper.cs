namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class PlayerHelper
    {
        private Image mCurrentIcon;
        private PlayerInfo mCurrentPlayer;
        public Hashtable playerHash = new Hashtable();

        [DllImport("MultiplayerBackend.dll", EntryPoint="GetConnections", CallingConvention=CallingConvention.Cdecl)]
        private static extern int _GetConnections([MarshalAs(UnmanagedType.LPStr)] string playername);
        [return: MarshalAs(UnmanagedType.LPStr)]
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetNextConnection", CallingConvention=CallingConvention.Cdecl)]
        private static extern string _GetNextConnection();
        [return: MarshalAs(UnmanagedType.LPStruct)]
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetPlayerInfo", CallingConvention=CallingConvention.Cdecl)]
        private static extern PlayerInfo _GetPlayerInfo([MarshalAs(UnmanagedType.LPStr)] string playername);
        [return: MarshalAs(UnmanagedType.BStr)]
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetPlayerPicture", CallingConvention=CallingConvention.Cdecl)]
        private static extern string _GetPlayerPicture([MarshalAs(UnmanagedType.LPStr)] string playername);
        [DllImport("MultiplayerBackend.dll", EntryPoint="SetPlayerInfo", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _SetPlayerInfo([MarshalAs(UnmanagedType.LPStr)] string icon, double rating, int wins, int losses, int draws, [MarshalAs(UnmanagedType.LPStr)] string description, [MarshalAs(UnmanagedType.LPStr)] string postalcode);
        [DllImport("MultiplayerBackend.dll", EntryPoint="SubmitFeedback", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool _SubmitFeedback([MarshalAs(UnmanagedType.LPStr)] string playername, [MarshalAs(UnmanagedType.LPStr)] string feedbacktype, [MarshalAs(UnmanagedType.LPStr)] string content);
        public int GetConnections(string playername)
        {
            int num;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                num = _GetConnections(playername);
            }
            finally
            {
                Lobby.Unlock();
            }
            return num;
        }

        public ConnectionInfo GetNextConnection()
        {
            ConnectionInfo info2;
            Lobby.Lock();
            try
            {
                ConnectionInfo info = new ConnectionInfo {
                    ConnectionKind = ConnectionType.Private
                };
                foreach (string str2 in _GetNextConnection().Split(new char[] { ';' }))
                {
                    switch (str2.Split(new char[] { '=' })[0])
                    {
                        case "prudp:/address":
                            info.IPAddress = str2.Split(new char[] { '=' })[1];
                            break;

                        case "port":
                            info.Port = Convert.ToInt32(str2.Split(new char[] { '=' })[1]);
                            break;

                        case "type":
                            if (str2.Split(new char[] { '=' })[1] == "3")
                            {
                                info.ConnectionKind = ConnectionType.Public;
                            }
                            if (str2.Split(new char[] { '=' })[1] == "2")
                            {
                                info.ConnectionKind = ConnectionType.Public;
                            }
                            break;
                    }
                }
                info2 = info;
            }
            finally
            {
                Lobby.Unlock();
            }
            return info2;
        }

        public string GetPlayerInfo(string playername)
        {
            string str = "";
            string file = this.PlayerPath + playername + ".txt";
            string str3 = this.PlayerPath + playername + ".png";
            this.persist.GetFile(str3, 2, playername);
            this.persist.GetFile(file, 3, playername);
            if (File.Exists(file))
            {
                StreamReader reader = new StreamReader(file);
                try
                {
                    str = reader.ReadToEnd();
                }
                finally
                {
                    reader.Close();
                }
            }
            return str;
        }

        public PlayerInfo GetPlayerInfo(string playername, bool UseCache)
        {
            PlayerInfo info2;
            Lobby.Lock();
            try
            {
                PlayerInfo info = null;
                if (UseCache)
                {
                    info = this.playerHash[playername] as PlayerInfo;
                }
                if (info == null)
                {
                    try
                    {
                        Lobby.IncDBCallCount(new object[0]);
                        info = _GetPlayerInfo(playername);
                    }
                    catch
                    {
                        info = new PlayerInfo {
                            Description = ""
                        };
                    }
                    if (this.playerHash[playername] != null)
                    {
                        this.playerHash.Remove(playername);
                    }
                    this.playerHash.Add(playername, info);
                }
                info2 = info;
            }
            finally
            {
                Lobby.Unlock();
            }
            return info2;
        }

        public string GetPlayerPicture(string playername)
        {
            string str;
            Lobby.Lock();
            try
            {
                str = _GetPlayerPicture(playername);
            }
            finally
            {
                Lobby.Unlock();
            }
            return str;
        }

        public void SetPlayerInfo(string playername, string description)
        {
            string path = this.PlayerPath + playername + ".txt";
            string file = this.PlayerPath + playername + ".png";
            StreamWriter writer = new StreamWriter(path);
            try
            {
                writer.Write(description);
            }
            finally
            {
                writer.Close();
            }
            this.persist.AddFile(path, 3, playername);
            this.persist.AddFile(file, 2, playername);
        }

        public bool SetPlayerInfo(string icon, double rating, int wins, int losses, int draws, string description, string postalcode)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                flag = _SetPlayerInfo(icon, rating, wins, losses, draws, description, postalcode);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool SubmitFeedback(string playername, string feedbacktype, string content)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                flag = _SubmitFeedback(playername, feedbacktype, content);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public Image CurrentIcon
        {
            get
            {
                if (this.mCurrentIcon == null)
                {
                    try
                    {
                        MemoryStream stream = new MemoryStream(Convert.FromBase64String(this.CurrentPlayer.Icon));
                        this.mCurrentIcon = Image.FromStream(stream);
                    }
                    catch
                    {
                        this.mCurrentIcon = null;
                    }
                }
                return this.mCurrentIcon;
            }
        }

        public PlayerInfo CurrentPlayer
        {
            get
            {
                if (this.mCurrentPlayer == null)
                {
                    this.mCurrentPlayer = this.GetPlayerInfo(Lobby.GetLobby().authenticationHelper.LoggedInUser, true);
                }
                return this.mCurrentPlayer;
            }
        }

        private PersistentStoreHelper persist
        {
            get
            {
                return Lobby.GetLobby().persistentStoreHelper;
            }
        }

        public string PlayerPath
        {
            get
            {
                string path = Application.StartupPath + @"\playerdata\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}

