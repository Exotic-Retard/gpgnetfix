namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class Lobby : IDisposable
    {
        private static Lobby _Lobby = null;
        internal AuthenticationHelper authenticationHelper = new AuthenticationHelper();
        public const string BackendDLL = "MultiplayerBackend.dll";
        internal ChatroomHelper chatroomHelper = new ChatroomHelper("main");
        public static string[] CommandLineArgs = null;
        public CompetitionHelper competitionHelper = new CompetitionHelper();
        public static ArrayList DBCalls = ArrayList.Synchronized(new ArrayList());
        public DirectSQLHelper directSQLHelper = new DirectSQLHelper();
        public GameHelper gameHelper = new GameHelper("main");
        public LobbyExtension lobbyExtension = new LobbyExtension();
        public static bool LoggingOut = false;
        public MatchmakingHelper matchmakingHelper = new MatchmakingHelper("main");
        private Hashtable mChatroomHelpers = Hashtable.Synchronized(new Hashtable());
        private Hashtable mGameHelpers = Hashtable.Synchronized(new Hashtable());
        private Hashtable mMatchmakingHelpers = Hashtable.Synchronized(new Hashtable());
        public PersistentStoreHelper persistentStoreHelper = new PersistentStoreHelper();
        public PlayerHelper playerHelper = new PlayerHelper();
        private static int sDBCallCount = 0;
        private static object sDBCallMutext = new object();
        private static string sLastAccessKey = "";
        public static IGPGnetProtocol sProtocol = null;

        public static  event EventHandler OnResetLobby;

        private Lobby(string accesskey)
        {
            sLastAccessKey = accesskey;
            EventLog.WriteLine("Creating Lobby with Accesskey: " + accesskey, LogCategory.Get("Lobby"), new object[0]);
            if (accesskey == "GPGNET")
            {
                sProtocol = new GPGnetProtocol();
            }
            else
            {
                sProtocol = null;
                CreateLobby(accesskey);
            }
        }

        [DllImport("MultiplayerBackend.dll", EntryPoint="IsConnected", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool _IsConnected();
        internal ChatroomHelper ChatroomHelperByName(string name)
        {
            if (name == "main")
            {
                return this.chatroomHelper;
            }
            if (this.mChatroomHelpers.ContainsKey(name))
            {
                return (this.mChatroomHelpers[name] as ChatroomHelper);
            }
            ChatroomHelper helper = new ChatroomHelper(name);
            this.mChatroomHelpers.Add(name, helper);
            return helper;
        }

        [DllImport("MultiplayerBackend.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern void CreateLobby([MarshalAs(UnmanagedType.LPStr)] string accesskey);
        public void Dispose()
        {
            LogoutOfLobby();
        }

        public static object[] FetchCPlusArray(IntPtr ArrayPointer, Type type)
        {
            ArrayList list = new ArrayList();
            int ofs = 0;
            while (true)
            {
                IntPtr ptr = Marshal.ReadIntPtr(ArrayPointer, ofs);
                if (ptr == IntPtr.Zero)
                {
                    break;
                }
                ofs += IntPtr.Size;
                try
                {
                    object obj2;
                    if (type == typeof(string))
                    {
                        obj2 = Marshal.PtrToStringAnsi(ptr);
                    }
                    else
                    {
                        obj2 = Marshal.PtrToStructure(ptr, type);
                    }
                    list.Add(obj2);
                }
                catch
                {
                }
            }
            return (list.ToArray(type) as object[]);
        }

        public GameHelper GameHelperByName(string name)
        {
            if (name == "main")
            {
                return this.gameHelper;
            }
            if (this.mGameHelpers.ContainsKey(name))
            {
                return (this.mGameHelpers[name] as GameHelper);
            }
            GameHelper helper = new GameHelper(name);
            this.mGameHelpers.Add(name, helper);
            return helper;
        }

        public string GetCommandLineArguement(string argname)
        {
            foreach (string str in CommandLineArgs)
            {
                if (str.ToUpper().IndexOf(argname.ToUpper()) == 0)
                {
                    return str;
                }
            }
            return "";
        }

        public static int GetDBCallCount()
        {
            lock (sDBCallMutext)
            {
                return sDBCallCount;
            }
        }

        public static Lobby GetLobby()
        {
            if (_Lobby == null)
            {
                _Lobby = new Lobby(sLastAccessKey);
            }
            return _Lobby;
        }

        public bool HasCommandLineArguement(string argname)
        {
            foreach (string str in CommandLineArgs)
            {
                if (str.ToUpper().IndexOf(argname.ToUpper()) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static void IncDBCallCount(params object[] logparams)
        {
        }

        public bool IsConnected()
        {
            if (sProtocol != null)
            {
                return sProtocol.IsConnected();
            }
            return _IsConnected();
        }

        public static void Lock()
        {
            try
            {
                Monitor.Enter(_Lobby);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal static void Logout()
        {
            try
            {
                if (sProtocol != null)
                {
                    sProtocol.Logout();
                }
                else
                {
                    LoggingOut = true;
                    EventLog.WriteLine("Attempting to release lobby.", new object[0]);
                    LogoutOfLobby();
                    EventLog.WriteLine("Successfully destroyed lobby and logged out.", new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        [DllImport("MultiplayerBackend.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern void LogoutOfLobby();
        public MatchmakingHelper MatchmakingHelperByName(string name)
        {
            if (name == "main")
            {
                return this.matchmakingHelper;
            }
            if (this.mMatchmakingHelpers.ContainsKey(name))
            {
                return (this.mMatchmakingHelpers[name] as MatchmakingHelper);
            }
            MatchmakingHelper helper = new MatchmakingHelper(name);
            this.mMatchmakingHelpers.Add(name, helper);
            return helper;
        }

        [DllImport("MultiplayerBackend.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern void ReleaseLobby();
        public static void ResetLobby()
        {
            if (_Lobby != null)
            {
                try
                {
                    if (OnResetLobby != null)
                    {
                        OnResetLobby(null, EventArgs.Empty);
                    }
                    LogoutOfLobby();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                CreateLobby(sLastAccessKey);
            }
        }

        public static void SetAccessKey(string accesskey)
        {
            sLastAccessKey = accesskey;
            if (accesskey == "GPGNET")
            {
                sProtocol = new GPGnetProtocol();
            }
            else
            {
                sProtocol = null;
            }
            EventLog.WriteLine("Setting Accesskey: " + accesskey, LogCategory.Get("Lobby"), new object[0]);
        }

        [DllImport("MultiplayerBackend.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern void TestCustomGathering();
        public static void Unlock()
        {
            try
            {
                Monitor.Exit(_Lobby);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public static void UpdateIP()
        {
            if (sProtocol != null)
            {
                sProtocol.RegisterAddress(GPGnetSelectedGame.SelectedGame.Port);
            }
        }
    }
}

