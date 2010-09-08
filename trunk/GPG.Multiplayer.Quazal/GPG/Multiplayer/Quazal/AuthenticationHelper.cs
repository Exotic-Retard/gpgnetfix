namespace GPG.Multiplayer.Quazal
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class AuthenticationHelper
    {
        private Hashtable cachedNameList = new Hashtable();
        private Hashtable cachedPlayerList = new Hashtable();
        public bool IsAdministrator;
        public string LoggedInUser = "";
        public int Port;
        public string Server = "";

        public event AuthenticationEventHandler PlayerLogin;

        internal AuthenticationHelper()
        {
        }

        [DllImport("MultiplayerBackend.dll", EntryPoint="CreateUserEx")]
        private static extern bool _CreateUser([MarshalAs(UnmanagedType.LPStr)] string username, [MarshalAs(UnmanagedType.LPStr)] string password, [MarshalAs(UnmanagedType.LPStr)] string email, [MarshalAs(UnmanagedType.LPStr)] string server, int port);
        [DllImport("MultiplayerBackend.dll", EntryPoint="FindIDByPlayerName")]
        private static extern int _FindIDByPlayerName([MarshalAs(UnmanagedType.LPStr)] string playerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="FindPlayerByID")]
        private static extern IntPtr _FindPlayerByID(int playerid);
        [DllImport("MultiplayerBackend.dll", EntryPoint="IsAdministrator")]
        private static extern bool _IsAdministrator();
        [DllImport("MultiplayerBackend.dll", EntryPoint="LoginUserEx")]
        private static extern bool _LoginUser([MarshalAs(UnmanagedType.LPStr)] string username, [MarshalAs(UnmanagedType.LPStr)] string password, [MarshalAs(UnmanagedType.LPStr)] string server, int port);
        public bool CreateUser(string username, string password, string email, string server, int port)
        {
            bool flag;
            try
            {
                if (Lobby.sProtocol != null)
                {
                    flag = Lobby.sProtocol.CreateUser(username, password, email, server, port);
                }
                else
                {
                    EventLog.WriteLine("Server: " + server + ":" + port.ToString(), LogCategory.Get("Lobby"), new object[0]);
                    Lobby.Lock();
                    try
                    {
                        if ((email.IndexOf('@') < 0) || (email.IndexOf('.') < 0))
                        {
                            return false;
                        }
                        if (password.Length < 6)
                        {
                            return false;
                        }
                        if (username.IndexOfAny(" |".ToCharArray()) > 0)
                        {
                            return false;
                        }
                        Lobby.IncDBCallCount(new object[0]);
                        flag = _CreateUser(username, password, email, server, port);
                    }
                    finally
                    {
                        Lobby.Unlock();
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                flag = false;
            }
            return flag;
        }

        public int FindIDByPlayerName(string playerName)
        {
            int num2;
            Lobby.Lock();
            try
            {
                if (this.cachedPlayerList[playerName] != null)
                {
                    return Convert.ToInt32(this.cachedPlayerList[playerName]);
                }
                int num = Convert.ToInt32(Lobby.GetLobby().directSQLHelper.GetQueryDataset("GetPlayerIDFromName", new string[] { playerName }).Split(new char[] { '\x0003' })[1]);
                this.cachedPlayerList.Add(playerName, num);
                Lobby.IncDBCallCount(new object[0]);
                num2 = num;
            }
            finally
            {
                Lobby.Unlock();
            }
            return num2;
        }

        public string FindPlayerByID(int playerid)
        {
            string str2;
            Lobby.Lock();
            try
            {
                if (this.cachedNameList[playerid] != null)
                {
                    return this.cachedNameList[playerid].ToString();
                }
                string str = "";
                if (Lobby.sProtocol != null)
                {
                    str = Lobby.sProtocol.FindPlayerByID(playerid);
                }
                else
                {
                    str = Marshal.PtrToStringAnsi(_FindPlayerByID(playerid));
                }
                this.cachedNameList.Add(playerid, str);
                Lobby.IncDBCallCount(new object[0]);
                str2 = str;
            }
            finally
            {
                Lobby.Unlock();
            }
            return str2;
        }

        public bool LoginUser(string username, string password, string server, int port)
        {
            bool flag2;
            if (Lobby.sProtocol != null)
            {
                return Lobby.sProtocol.LoginUser(username, password, server, port);
            }
            this.Server = server;
            this.Port = port;
            EventLog.WriteLine("Calling the login function.", new object[0]);
            EventLog.WriteLine("Server: " + server + ":" + port.ToString(), LogCategory.Get("Lobby"), new object[0]);
            try
            {
                Lobby.Lock();
                try
                {
                    Lobby.ResetLobby();
                    bool flag = _LoginUser(username, password, server, port);
                    if (flag)
                    {
                        this.LoggedInUser = username;
                        this.IsAdministrator = _IsAdministrator();
                        Lobby.IncDBCallCount(new object[0]);
                        return flag;
                    }
                    this.IsAdministrator = false;
                    if (flag && (this.PlayerLogin != null))
                    {
                        this.PlayerLogin(username);
                    }
                    EventLog.WriteLine("Login function called fine.  Result was: " + flag.ToString(), new object[0]);
                    flag2 = flag;
                }
                finally
                {
                    Lobby.Unlock();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                flag2 = false;
            }
            return flag2;
        }

        [DllImport("MultiplayerBackend.dll")]
        public static extern void SetLoginName([MarshalAs(UnmanagedType.LPStr)] string username);

        public int LoggedInPlayerID
        {
            get
            {
                return this.FindIDByPlayerName(this.LoggedInUser);
            }
        }
    }
}

