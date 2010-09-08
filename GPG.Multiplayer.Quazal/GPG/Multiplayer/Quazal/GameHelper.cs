namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.Logging;
    using System;
    using System.Runtime.InteropServices;

    public class GameHelper
    {
        private string mContainerName = "default";
        private string mCustomData = string.Empty;
        private string mGameName = string.Empty;
        private bool mIsHost;
        private string mMapName = string.Empty;
        private int mMaxPlayers = 8;
        private int mRating;
        private string mVersion = string.Empty;

        internal GameHelper(string containerName)
        {
            this.mContainerName = containerName;
        }

        public bool CreateGame(string description, string mapName, int rating, int maxPlayers, string gameName, string version, string customData)
        {
            bool flag2;
            Lobby.Lock();
            try
            {
                EventLog.WriteLine("[GameHelper] Begin Create Game", new object[0]);
                this.mGameName = description;
                bool flag = false;
                if (this.IsRegularRankedGame())
                {
                    ConfigSettings.GetInt(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " MaxPlayers", 100);
                }
                if (Lobby.sProtocol != null)
                {
                    flag = Lobby.sProtocol.CreateGame(description, mapName, rating, 100, gameName, version, customData, this.mContainerName);
                }
                else
                {
                    flag = mCreateGame(description, mapName, rating, 100, gameName, version, customData, this.mContainerName);
                }
                this.mIsHost = flag;
                this.mCustomData = customData;
                this.mVersion = version;
                this.mMapName = mapName;
                this.mRating = rating;
                this.mMaxPlayers = 8;
                EventLog.WriteLine("[GameHelper] End Create Game", new object[0]);
                Lobby.IncDBCallCount(new object[0]);
                flag2 = flag;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag2;
        }

        public bool EndGame()
        {
            bool flag;
            Lobby.Lock();
            try
            {
                if (this.mIsHost)
                {
                    if (Lobby.sProtocol != null)
                    {
                        return Lobby.sProtocol.EndGame();
                    }
                    return mEndGame(this.mContainerName);
                }
                flag = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        private bool IsRegularRankedGame()
        {
            return ((ConfigSettings.GetString("Automatch GameIDs", "27") + " ").IndexOf(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " ") >= 0);
        }

        public string JoinGame(string GameName)
        {
            string str;
            Lobby.Lock();
            try
            {
                EventLog.WriteLine("[GameHelper] Begin Join Game", new object[0]);
                this.mGameName = GameName;
                this.mIsHost = false;
                if (Lobby.sProtocol != null)
                {
                    EventLog.WriteLine("[GameHelper] End Join Game", new object[0]);
                    return Lobby.sProtocol.JoinGame(GameName);
                }
                IntPtr ptr = mJoinGame(GameName, this.mContainerName);
                EventLog.WriteLine("[GameHelper] End Join Game", new object[0]);
                Lobby.IncDBCallCount(new object[0]);
                if (ptr == IntPtr.Zero)
                {
                    throw new Exception("Invalid result");
                }
                str = Marshal.PtrToStringAnsi(ptr);
            }
            catch
            {
                str = "NONE";
            }
            finally
            {
                Lobby.Unlock();
            }
            return str;
        }

        public bool LeaveGame()
        {
            bool flag2;
            Lobby.Lock();
            try
            {
                EventLog.WriteLine("[GameHelper] Begin Leave Game", new object[0]);
                this.mGameName = string.Empty;
                this.mIsHost = false;
                if (Lobby.sProtocol != null)
                {
                    EventLog.WriteLine("[GameHelper] End Leave Game", new object[0]);
                    return Lobby.sProtocol.LeaveGame();
                }
                bool flag = mLeaveGame(this.mContainerName);
                EventLog.WriteLine("[GameHelper] End Leave Game", new object[0]);
                Lobby.IncDBCallCount(new object[0]);
                flag2 = flag;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag2;
        }

        [DllImport("MultiplayerBackend.dll", EntryPoint="CreateGame", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mCreateGame([MarshalAs(UnmanagedType.LPStr)] string description, [MarshalAs(UnmanagedType.LPStr)] string mapName, int rating, int maxPlayers, [MarshalAs(UnmanagedType.LPStr)] string gameName, [MarshalAs(UnmanagedType.LPStr)] string version, [MarshalAs(UnmanagedType.LPStr)] string customData, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="EndGame", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mEndGame([MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="JoinGame", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern IntPtr mJoinGame([MarshalAs(UnmanagedType.LPStr)] string GameName, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="LeaveGame", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mLeaveGame([MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="StartGame", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mStartGame([MarshalAs(UnmanagedType.LPStr)] string containerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="UpdateGame", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mUpdateGame([MarshalAs(UnmanagedType.LPStr)] string description, [MarshalAs(UnmanagedType.LPStr)] string mapName, int rating, int maxPlayers, [MarshalAs(UnmanagedType.LPStr)] string gameName, [MarshalAs(UnmanagedType.LPStr)] string version, [MarshalAs(UnmanagedType.LPStr)] string customData, [MarshalAs(UnmanagedType.LPStr)] string containerName);
        public bool StartGame()
        {
            bool flag2;
            Lobby.Lock();
            try
            {
                EventLog.WriteLine("[GameHelper] Begin Start Game", new object[0]);
                if (this.mIsHost)
                {
                    if (Lobby.sProtocol != null)
                    {
                        EventLog.WriteLine("[GameHelper] End Start Game", new object[0]);
                        return Lobby.sProtocol.StartGame();
                    }
                    bool flag = mStartGame(this.mContainerName);
                    EventLog.WriteLine("[GameHelper] End Start Game", new object[0]);
                    Lobby.IncDBCallCount(new object[0]);
                    return flag;
                }
                EventLog.WriteLine("[GameHelper] Not the host", new object[0]);
                flag2 = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag2;
        }

        public bool UpdateGame(string mapName, int rating, int maxPlayers, string gameName, string version, string customData)
        {
            bool flag2;
            Lobby.Lock();
            try
            {
                EventLog.WriteLine("[GameHelper] Begin Update Game", new object[0]);
                if (this.mIsHost)
                {
                    if (customData != string.Empty)
                    {
                        this.mCustomData = customData;
                    }
                    if (version != string.Empty)
                    {
                        this.mVersion = version;
                    }
                    if (mapName != string.Empty)
                    {
                        this.mMapName = mapName;
                    }
                    if (rating != 0)
                    {
                        this.mRating = rating;
                    }
                    int p = 8;
                    if (this.IsRegularRankedGame())
                    {
                        p = ConfigSettings.GetInt(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " MaxPlayers", 100);
                    }
                    bool flag = false;
                    if (Lobby.sProtocol != null)
                    {
                        flag = Lobby.sProtocol.UpdateGame(this.mGameName, this.mMapName, this.mRating, p, gameName, this.mVersion, this.mCustomData, this.mContainerName);
                    }
                    else
                    {
                        flag = mUpdateGame(this.mGameName, this.mMapName, this.mRating, p, gameName, this.mVersion, this.mCustomData, this.mContainerName);
                    }
                    EventLog.WriteLine("[GameHelper] Update Game Call Success", new object[0]);
                    Lobby.IncDBCallCount(new object[0]);
                    return flag;
                }
                EventLog.WriteLine("[GameHelper] Not Host", new object[0]);
                flag2 = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag2;
        }
    }
}

