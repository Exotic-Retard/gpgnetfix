namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.Network;
    using GPG.Threading;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    [IndexMap(new string[] { "mName", "mID" }, new string[] { "name", "id" })]
    public class User : MappedObject, IUser, IPingMonitor
    {
        private const char CustomDataDelim = '\x0016';
        private int LastCache;
        private int LastPing;
        [FieldMap("avatar")]
        private int mAvatar;
        [FieldMap("award1")]
        private int mAward1;
        [FieldMap("award2")]
        private int mAward2;
        [FieldMap("award3")]
        private int mAward3;
        public const int MaxNameLength = 0x16;
        [FieldMap("clan_id")]
        private string mClanAbbreviation;
        [FieldMap("clan_name")]
        private string mClanName;
        private static User mCurrent;
        private static MappedObjectList<User> mCurrentFriends = new MappedObjectList<User>();
        [FieldMap("custom_data")]
        private string mCustomData;
        [NotTransferred, FieldMap("description")]
        private string mDescription;
        private string mEmail;
        private static User mError;
        private static User mEvent;
        private static User mGame;
        [FieldMap("groups")]
        private int mGroups;
        [FieldMap("id")]
        private int mID;
        private static List<int> mIgnoredPlayers = new List<int>();
        public const int MinNameLength = 3;
        public const int MinPasswordLength = 6;
        private string mIpAddress;
        [FieldMap("is_away")]
        private bool mIsAway;
        [FieldMap("is_dnd")]
        private bool mIsDND;
        [FieldMap("friend")]
        private bool mIsFriend;
        public bool? mIsModerator;
        private bool mIsSystem;
        [FieldMap("name")]
        private string mName;
        [FieldMap("online")]
        private bool mOnline;
        private int mPingTime;
        private Image mPlayerIcon;
        private int mPort;
        [FieldMap("rank")]
        private int mRank;
        private PlayerRating mRating_1v1;
        private static User mSystem;
        private static User mUnknown;
        private bool mVisible;
        private System.Net.NetworkInformation.Ping Ping;
        private const int PingTimeout = 0x1388;
        private const int PingWait = 0x3e8;

        public static  event EventHandler LoggedOut;

        public static  event EventHandler LoggingOut;

        public event EventHandler PingChanged;

        public User()
        {
            this.mGroups = -1;
            this.mOnline = true;
            this.mVisible = true;
            this.mRank = -1;
            this.mCustomData = "";
            this.mIsModerator = null;
            this.Ping = new System.Net.NetworkInformation.Ping();
        }

        public User(DataRecord record) : base(record)
        {
            this.mGroups = -1;
            this.mOnline = true;
            this.mVisible = true;
            this.mRank = -1;
            this.mCustomData = "";
            this.mIsModerator = null;
            this.Ping = new System.Net.NetworkInformation.Ping();
        }

        internal User(int id, string name)
        {
            this.mGroups = -1;
            this.mOnline = true;
            this.mVisible = true;
            this.mRank = -1;
            this.mCustomData = "";
            this.mIsModerator = null;
            this.Ping = new System.Net.NetworkInformation.Ping();
            this.mID = id;
            this.mName = name;
            if (id < 0)
            {
                this.IsSystem = true;
            }
        }

        public bool AquireIpAddress()
        {
            if (this.mIpAddress == null)
            {
                try
                {
                    DataList queryData = DataAccess.GetQueryData("GetConnectionInfo", new object[] { this.ID });
                    foreach (DataRecord record in queryData)
                    {
                        string str = record["url"];
                        if ((str == null) || (str.Length < 1))
                        {
                            this.mIpAddress = string.Empty;
                            continue;
                        }
                        string[] strArray = str.Split(new char[] { ';' });
                        Dictionary<string, string> dictionary = new Dictionary<string, string>(strArray.Length);
                        foreach (string str2 in strArray)
                        {
                            string[] strArray2 = str2.Split(new char[] { '=' });
                            dictionary.Add(strArray2[0], strArray2[1]);
                        }
                        if (dictionary.ContainsKey("type"))
                        {
                            if (dictionary["type"] == "3")
                            {
                                goto Label_0103;
                            }
                            continue;
                        }
                        if (queryData.Count > 1)
                        {
                            continue;
                        }
                    Label_0103:
                        this.mIpAddress = dictionary["prudp:/address"];
                        this.mPort = int.Parse(dictionary["port"]);
                        goto Label_015B;
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return false;
                }
            }
        Label_015B:
            if ((this.mIpAddress != null) && !(this.mIpAddress == string.Empty))
            {
                return true;
            }
            return false;
        }

        public static bool ChangePassword(string newpwd)
        {
            return Lobby.GetLobby().lobbyExtension.ChangePassword(newpwd);
        }

        public static bool CreateLogin(string username, string pwd, string email, string server, int port)
        {
            return Lobby.GetLobby().authenticationHelper.CreateUser(username, pwd, email, server, port);
        }

        public override bool Equals(object obj)
        {
            if (obj is IUser)
            {
                return (this.ID == (obj as IUser).ID);
            }
            return base.Equals(obj);
        }

        protected override void FromDataTransfer(DataRecord record)
        {
            try
            {
                this.mID = Convert.ToInt32(record["id"]);
                this.mName = record["name"];
                this.mGroups = Convert.ToInt32(record["groups"]);
                this.mIsDND = Convert.ToInt32(record["is_dnd"]) > 0;
                this.mIsAway = Convert.ToInt32(record["is_away"]) > 0;
                this.mClanAbbreviation = record["clan_id"];
                this.mClanName = record["clan_name"];
                this.mOnline = Convert.ToInt32(record["online"]) > 0;
                int.TryParse(record["rank"], out this.mRank);
                this.mCustomData = record["custom_data"];
                if (record.InnerHash.ContainsKey("avatar"))
                {
                    int.TryParse(record["avatar"], out this.mAvatar);
                }
                if (record.InnerHash.ContainsKey("award1"))
                {
                    int.TryParse(record["award1"], out this.mAward1);
                }
                if (record.InnerHash.ContainsKey("award2"))
                {
                    int.TryParse(record["award2"], out this.mAward2);
                }
                if (record.InnerHash.ContainsKey("award3"))
                {
                    int.TryParse(record["award3"], out this.mAward3);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private string GetCustomData(string name)
        {
            if (this.mCustomData != null)
            {
                foreach (string str in this.mCustomData.Split(new char[] { '\x0016' }))
                {
                    if (str.IndexOf(name + "=") == 0)
                    {
                        return str.Replace(name + "=", "");
                    }
                }
            }
            return "";
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

        public static IGPGnetProtocol GetProtocol()
        {
            return Lobby.sProtocol;
        }

        public static string HashPassword(string password)
        {
            byte[] bytes = Encoding.Default.GetBytes(password);
            try
            {
                byte[] buffer2 = new MD5CryptoServiceProvider().ComputeHash(bytes);
                string str = "";
                foreach (byte num in buffer2)
                {
                    if (num < 0x10)
                    {
                        str = str + "0" + num.ToString("x");
                    }
                    else
                    {
                        str = str + num.ToString("x");
                    }
                }
                return str;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        public static bool IsUserIgnored(User user)
        {
            return IsUserIgnored(user.ID);
        }

        public static bool IsUserIgnored(int id)
        {
            return IgnoredPlayers.Contains(id);
        }

        public static bool Login(string username, string pwd, string server, int port)
        {
            return Login(username, pwd, server, port, false);
        }

        public static bool Login(string username, string pwd, string server, int port, bool ignoreStatus)
        {
            VGen0 target = null;
            EventLog.WriteLine("Beginning Login Function.", new object[0]);
            try
            {
                if (Lobby.GetLobby().authenticationHelper.LoginUser(username, pwd, server, port))
                {
                    EventLog.WriteLine("Finished Login Function.", new object[0]);
                    if ((GetProtocol() != null) && (username.ToUpper().IndexOf("_T_E_M_P_") >= 0))
                    {
                        mCurrent = MakeFakeUser("Guest");
                        return true;
                    }
                    if (!ignoreStatus)
                    {
                        mCurrent = DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { username })[0];
                    }
                    else
                    {
                        mCurrent = MakeFakeUser(username);
                    }
                    if (target == null)
                    {
                        target = delegate {
                            if (!ignoreStatus)
                            {
                                DataAccess.ExecuteQuery("SetAwayStatus", new object[] { 0 });
                                DataAccess.ExecuteQuery("SetDNDStatus", new object[] { 0 });
                            }
                            mCurrent.IsAway = false;
                            mCurrent.IsDND = false;
                            if (Lobby.sProtocol == null)
                            {
                                AuthenticationHelper.SetLoginName(Current.Name);
                            }
                        };
                    }
                    ThreadQueue.Quazal.Enqueue(target, new object[0]);
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public static bool Logout()
        {
            try
            {
                if (Current != null)
                {
                    ThreadQueue.LoggingOut = true;
                    Lobby.LoggingOut = true;
                    Thread.Sleep(500);
                    EventLog.WriteLine("Logging out {0}", new object[] { Current });
                    if (LoggingOut != null)
                    {
                        LoggingOut(null, EventArgs.Empty);
                    }
                    ThreadQueue.Quazal.Stop();
                    ThreadQueue.Quazal.WaitUntilEmpty(100);
                    Messaging.StopPolling();
                    Thread.Sleep(100);
                    Chatroom.Leave();
                    Lobby.Logout();
                    mCurrent = null;
                    Chatroom.Current = null;
                    Chatroom.GatheringParticipants.Clear();
                    if (LoggedOut != null)
                    {
                        LoggedOut(null, EventArgs.Empty);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public static User MakeFakeUser(string name)
        {
            return new User { mName = name, mOnline = false, mID = -2, mGroups = 0x21, mIsFriend = false, mIsSystem = false };
        }

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            WaitCallback callBack = null;
            if (e.Reply.Status == IPStatus.TimedOut)
            {
                this.mPingTime = this.WorstPing;
            }
            else
            {
                this.mPingTime = (int) e.Reply.RoundtripTime;
            }
            if (this.PingChanged != null)
            {
                this.PingChanged(this, e);
            }
            if ((Environment.TickCount - this.LastPing) < 0x3e8)
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        while ((Environment.TickCount - this.LastPing) < 0x3e8)
                        {
                            Thread.Sleep(100);
                        }
                        this.SendPing();
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
            else
            {
                this.SendPing();
            }
        }

        public static bool ResetPassword(string user, string email)
        {
            return Lobby.GetLobby().lobbyExtension.ResetPassword(user, email);
        }

        private void SendPing()
        {
            this.LastPing = Environment.TickCount;
            this.Ping.SendAsync(IPAddress.Parse(this.IpAddress), 0x1388, null);
        }

        public static void SetAccessKey(string accesskey)
        {
            Lobby.SetAccessKey(accesskey);
        }

        public void StartPing()
        {
            if (ConfigSettings.GetBool("AllowPing", false))
            {
                this.Ping.PingCompleted += new PingCompletedEventHandler(this.Ping_PingCompleted);
                this.SendPing();
            }
        }

        public void StopPing()
        {
            this.Ping.SendAsyncCancel();
            this.Ping.PingCompleted -= new PingCompletedEventHandler(this.Ping_PingCompleted);
        }

        public override string ToDataString()
        {
            try
            {
                StringBuilder builder = new StringBuilder("id|name|groups|is_dnd|is_away|clan_id|clan_name|online|rank|custom_data|avatar|award1|award2|award3");
                builder.Append('\x0003');
                builder.Append(this.ID);
                builder.Append('|');
                builder.Append(this.Name);
                builder.Append('|');
                builder.Append(this.Groups);
                builder.Append('|');
                builder.Append(Convert.ToInt32(this.IsDND));
                builder.Append('|');
                builder.Append(Convert.ToInt32(this.IsAway));
                builder.Append('|');
                builder.Append(this.ClanAbbreviation);
                builder.Append('|');
                builder.Append(this.ClanName);
                builder.Append('|');
                builder.Append(Convert.ToInt32(this.Online));
                builder.Append('|');
                builder.Append(this.Rank);
                builder.Append('|');
                builder.Append(this.mCustomData);
                builder.Append('|');
                builder.Append(this.Avatar);
                builder.Append('|');
                builder.Append(this.Award1);
                builder.Append('|');
                builder.Append(this.Award2);
                builder.Append('|');
                builder.Append(this.Award3);
                return builder.ToString();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return base.ToDataString();
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        private void UpdateCustomData(string name, string value)
        {
            string str = "";
            foreach (string str2 in this.mCustomData.Split(new char[] { '\x0016' }))
            {
                if (str2.IndexOf(name + "=") == 0)
                {
                    str = string.Concat(new object[] { str, name, "=", value, '\x0016' });
                }
                else
                {
                    str = str + str2 + '\x0016';
                }
            }
            this.mCustomData = str;
        }

        public int Avatar
        {
            get
            {
                return this.mAvatar;
            }
            set
            {
                this.mAvatar = value;
            }
        }

        public int Award1
        {
            get
            {
                return this.mAward1;
            }
            set
            {
                this.mAward1 = value;
            }
        }

        public int Award2
        {
            get
            {
                return this.mAward2;
            }
            set
            {
                this.mAward2 = value;
            }
        }

        public int Award3
        {
            get
            {
                return this.mAward3;
            }
            set
            {
                this.mAward3 = value;
            }
        }

        public int BestPing
        {
            get
            {
                return 0;
            }
        }

        public string ClanAbbreviation
        {
            get
            {
                return this.mClanAbbreviation;
            }
            set
            {
                this.mClanAbbreviation = value;
            }
        }

        public string ClanName
        {
            get
            {
                return this.mClanName;
            }
            set
            {
                this.mClanName = value;
            }
        }

        public static User Current
        {
            get
            {
                return mCurrent;
            }
        }

        public static MappedObjectList<User> CurrentFriends
        {
            get
            {
                return mCurrentFriends;
            }
            set
            {
                mCurrentFriends = value;
            }
        }

        public string Description
        {
            get
            {
                if (this.mDescription != null)
                {
                    return this.mDescription.Replace("%s", "");
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.mDescription = value.Replace("'", "`");
                }
                else
                {
                    this.mDescription = value;
                }
            }
        }

        public string Email
        {
            get
            {
                if (this.mEmail == null)
                {
                    this.mEmail = DataAccess.GetString("GetUserEmail", new object[] { this.ID });
                }
                return this.mEmail;
            }
        }

        public static User Error
        {
            get
            {
                if (mError == null)
                {
                    mError = new User(-3, Loc.Get("<LOC>Error"));
                }
                return mError;
            }
        }

        public static User Event
        {
            get
            {
                if (mEvent == null)
                {
                    mEvent = new User(-2, Loc.Get("<LOC>Event"));
                }
                return mEvent;
            }
        }

        public static User Game
        {
            get
            {
                if (mGame == null)
                {
                    mGame = new User(-4, Loc.Get("<LOC>Game Event"));
                }
                return mGame;
            }
        }

        public int Groups
        {
            get
            {
                return this.mGroups;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public static List<int> IgnoredPlayers
        {
            get
            {
                return mIgnoredPlayers;
            }
            set
            {
                mIgnoredPlayers = value;
            }
        }

        public string ImageInfo
        {
            get
            {
                return this.GetCustomData("image");
            }
        }

        public string IpAddress
        {
            get
            {
                if (this.mIpAddress == null)
                {
                    this.AquireIpAddress();
                }
                return this.mIpAddress;
            }
        }

        public bool IsAdmin
        {
            get
            {
                GPGnetSelectedGame.IsAdmin = this.Groups == 5;
                return (this.Groups == 5);
            }
        }

        public bool IsAway
        {
            get
            {
                return this.mIsAway;
            }
            set
            {
                this.mIsAway = value;
            }
        }

        public bool IsChannelOperator
        {
            get
            {
                return (Chatroom.RoomOperator.ToLower() == this.Name.ToLower());
            }
        }

        public bool IsClanMate
        {
            get
            {
                if ((this.ClanAbbreviation == null) || (this.ClanAbbreviation.Length < 1))
                {
                    return false;
                }
                return ((Current != null) && (Current.ClanAbbreviation == this.ClanAbbreviation));
            }
        }

        public bool IsCurrent
        {
            get
            {
                if (Current == null)
                {
                    return false;
                }
                return this.Equals(Current);
            }
        }

        public bool IsDND
        {
            get
            {
                return this.mIsDND;
            }
            set
            {
                this.mIsDND = value;
            }
        }

        public bool IsFriend
        {
            get
            {
                return this.mIsFriend;
            }
            set
            {
                this.mIsFriend = value;
            }
        }

        public bool IsIgnored
        {
            get
            {
                return IsUserIgnored(this.ID);
            }
        }

        public bool IsInClan
        {
            get
            {
                return ((this.ClanAbbreviation != null) && (this.ClanAbbreviation.Length > 0));
            }
        }

        public bool IsModerator
        {
            get
            {
                if (!this.mIsModerator.HasValue)
                {
                    this.mIsModerator = new bool?(this.IsAdmin || AccessControlList.HasAccessTo("Moderators", this));
                }
                return this.mIsModerator.Value;
            }
        }

        public bool IsMvp
        {
            get
            {
                return (this.GetCustomData("mvp").ToLower() == "true");
            }
        }

        public bool IsSubAdmin
        {
            get
            {
                return (this.GetCustomData("subadmin").ToLower() == "true");
            }
        }

        public bool IsSystem
        {
            get
            {
                return this.mIsSystem;
            }
            set
            {
                this.mIsSystem = value;
            }
        }

        public static bool LoggedIn
        {
            get
            {
                return (Current != null);
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public bool Online
        {
            get
            {
                return this.mOnline;
            }
            set
            {
                this.mOnline = value;
            }
        }

        public int PingTime
        {
            get
            {
                return this.mPingTime;
            }
        }

        public Image PlayerIcon
        {
            get
            {
                return this.mPlayerIcon;
            }
            set
            {
                this.mPlayerIcon = value;
            }
        }

        public int Port
        {
            get
            {
                return this.mPort;
            }
        }

        public int Rank
        {
            get
            {
                return this.mRank;
            }
            set
            {
                this.mRank = value;
            }
        }

        public PlayerRating Rating_1v1
        {
            get
            {
                if ((Environment.TickCount - this.LastCache) >= (ConfigSettings.GetInt("StatsCacheTimeout", 5) * 0xea60))
                {
                    this.mRating_1v1 = null;
                }
                if (this.mRating_1v1 == null)
                {
                    if (ConfigSettings.GetBool("DoOldGameList", false) && DataAccess.TryGetObject<PlayerRating>("GetPlayerRating", out this.mRating_1v1, new object[] { this.Name, "1v1" }))
                    {
                        this.LastCache = Environment.TickCount;
                    }
                    else if (!ConfigSettings.GetBool("DoOldGameList", false) && DataAccess.TryGetObject<PlayerRating>("GetPlayerRating2", out this.mRating_1v1, new object[] { this.Name, "1v1", GPGnetSelectedGame.SelectedGame.GameID }))
                    {
                        this.LastCache = Environment.TickCount;
                    }
                    else
                    {
                        this.mRating_1v1 = new PlayerRating(this.ID, "1v1");
                    }
                }
                return this.mRating_1v1;
            }
        }

        public static User System
        {
            get
            {
                if (mSystem == null)
                {
                    mSystem = new User(-1, Loc.Get("<LOC>System"));
                }
                return mSystem;
            }
        }

        public static User Unknown
        {
            get
            {
                if (mUnknown == null)
                {
                    mUnknown = new User(-5, Loc.Get("<LOC>Unknown"));
                }
                return mUnknown;
            }
        }

        public bool Visible
        {
            get
            {
                return this.mVisible;
            }
            set
            {
                this.mVisible = value;
            }
        }

        public int WorstPing
        {
            get
            {
                return 0x3e8;
            }
        }
    }
}

