namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Chatroom : MappedObject
    {
        public static readonly Chatroom Game = new Chatroom(Loc.Get("<LOC>In Game"), -1);
        public const int MaxNameLength = 0x20;
        [FieldMap("clan_id")]
        private int mClanID = -1;
        private static Chatroom mCurrent;
        [FieldMap("room_desc")]
        private string mDescription;
        [FieldMap("dynamic_root")]
        private string mDynamicRoot;
        private static MappedObjectList<User> mGatheringParticipants = new MappedObjectList<User>();
        [FieldMap("gpgnet_game_id")]
        private int mGPGnetGameID = -1;
        private int mHiddenPlayers;
        [FieldMap("is_dynamic")]
        private bool mIsDynamic;
        private bool mIsPersistent;
        [FieldMap("is_secure")]
        private bool mIsSecure;
        private static string mMainChatNames = null;
        private static int mMaxRoomSize = -1;
        [FieldMap("count")]
        private int mPopulation = -1;
        private static string mRoomOperator = null;
        private static int mRoomRolloverSize = -1;
        public static readonly Chatroom None = new Chatroom(Loc.Get("<LOC>Not in a chatroom"), -1);

        public static  event StringEventHandler LeftChat;

        public event PropertyChangedEventHandler PopulationChanged;

        public Chatroom(DataRecord record) : base(record)
        {
        }

        public Chatroom(string name, int population)
        {
            this.Construct(name, population, false);
        }

        public Chatroom(string name, int population, bool isPersistent)
        {
            this.Construct(name, population, isPersistent);
        }

        private void Construct(string name, int population, bool isPersistent)
        {
            this.mDescription = name;
            this.mPopulation = population;
            this.mIsPersistent = isPersistent;
        }

        public static bool Create(string chatroom, out string error)
        {
            bool @bool = DataAccess.GetBool("RoomExists", new object[] { chatroom });
            bool flag2 = false;
            if (!@bool)
            {
                MappedObjectList<ReservedPrefix> objects = DataAccess.GetObjects<ReservedPrefix>("GetReservedPrefixes", new object[0]);
                for (int i = 0; i < objects.Count; i++)
                {
                    if (chatroom.ToLower().StartsWith(objects[i].Prefix.ToLower()))
                    {
                        error = string.Format(Loc.Get("<LOC>Unable to create room with prefix '{0}', {1}."), objects[i].Prefix, objects[i].Reason);
                        return false;
                    }
                }
            }
            flag2 = Lobby.GetLobby().chatroomHelper.CreateChatroom(chatroom);
            if (!flag2)
            {
                error = string.Format(Loc.Get("<LOC>Unable to create room {0}."), chatroom);
                return flag2;
            }
            error = null;
            return flag2;
        }

        public override bool Equals(object obj)
        {
            return ((obj is Chatroom) && (this.Description.ToLower() == (obj as Chatroom).Description.ToLower()));
        }

        public override int GetHashCode()
        {
            return this.ToString().ToLower().GetHashCode();
        }

        public static User GetParticipantByID(int id)
        {
            foreach (User user in mGatheringParticipants)
            {
                if (user.ID == id)
                {
                    return user;
                }
            }
            return null;
        }

        public static bool Join(string chatroom, out string error)
        {
            chatroom = chatroom.TrimEnd(" ".ToCharArray());
            if (DataAccess.GetBool("IsPlayerBanned", new object[] { User.Current.Name, chatroom }))
            {
                error = string.Format(Loc.Get("<LOC>You have been banned from {0}!"), chatroom);
                return false;
            }
            if ((CurrentName != null) && (CurrentName.ToLower() == chatroom.ToLower()))
            {
                error = null;
                RoomOperator = null;
                return true;
            }
            if (chatroom.ToLower().StartsWith("clan") && ((User.Current.ClanName == null) || !chatroom.ToLower().EndsWith(User.Current.ClanName.ToLower())))
            {
                error = Loc.Get("<LOC>Sorry, you cannot join or create a clan chat room unless you are a member of that clan.");
                EventLog.WriteLine(error, new object[0]);
                return false;
            }
            if (chatroom.ToLower() == None.Description.ToLower())
            {
                error = null;
                return false;
            }
            MappedObjectList<Chatroom> objects = DataAccess.GetObjects<Chatroom>("GetRoomDetails", new object[] { chatroom });
            Chatroom chatroom2 = null;
            if (objects.Count > 0)
            {
                chatroom2 = objects[0];
            }
            if (((chatroom2 != null) && chatroom2.IsSecure) && !User.Current.IsAdmin)
            {
                string gameDescription = "Unknown";
                bool flag = false;
                foreach (IGameInformation information in GPGnetSelectedGame.GameList)
                {
                    if (information.GameID == chatroom2.GPGnetGameID)
                    {
                        gameDescription = information.GameDescription;
                        if (information.CDKey != "")
                        {
                            flag = true;
                        }
                    }
                }
                if (!flag)
                {
                    error = string.Format(Loc.Get("<LOC>Could not enter the chat room {0}. You must have an active CD key for {1} to enter this room."), chatroom, gameDescription);
                    return false;
                }
            }
            if (InChatroom)
            {
                Leave();
            }
            if (objects.Count > 0)
            {
                chatroom2.mIsPersistent = true;
                if (chatroom2.Population >= MaxRoomSize)
                {
                    if (!chatroom2.HasDynamicRoot)
                    {
                        error = string.Format(Loc.Get("<LOC>Could not enter the chat room {0}. The chat room is full. Please try again later."), chatroom);
                        return false;
                    }
                    try
                    {
                        DataAccess.ExecuteQuery("ClearEmptyDynamicRooms", new object[0]);
                        MappedObjectList<Chatroom> list2 = DataAccess.GetObjects<Chatroom>("GetDynamicRooms", new object[] { chatroom2.DynamicRoot });
                        bool flag2 = false;
                        List<int> list3 = new List<int>();
                        int item = 1;
                        foreach (Chatroom chatroom3 in list2)
                        {
                            if (chatroom3.Population < MaxRoomSize)
                            {
                                chatroom = chatroom3.Description;
                                flag2 = true;
                                break;
                            }
                            list3.Add(item);
                            item++;
                        }
                        if (!flag2)
                        {
                            int num2 = 1;
                            while (num2 < (list3.Count + 2))
                            {
                                if (!list3.Contains(num2))
                                {
                                    break;
                                }
                                num2++;
                            }
                            if ((MainChatNames == null) || (MainChatNames.Length < (num2 - 1)))
                            {
                                chatroom = string.Format("{0} {1}", chatroom2.DynamicRoot, num2);
                                EventLog.WriteLine("Creating dynamic chatroom {0}", new object[] { chatroom });
                                if (!DataAccess.ExecuteQuery("CreateMainChatroom", new object[] { num2 }))
                                {
                                    error = string.Format(Loc.Get("<LOC>Error creating chatroom {0}."), chatroom);
                                    ErrorLog.WriteLine(error, new object[0]);
                                    return false;
                                }
                            }
                            else
                            {
                                try
                                {
                                    for (int i = 0; i < item; i++)
                                    {
                                        if (!list3.Contains(i + 1))
                                        {
                                            chatroom = MainChatNames[num2];
                                            flag2 = true;
                                            break;
                                        }
                                    }
                                    if (flag2)
                                    {
                                        EventLog.WriteLine("Creating dynamic chatroom {0}", new object[] { chatroom });
                                        if (!DataAccess.ExecuteQuery("CreateMainChatroom1", new object[] { chatroom }))
                                        {
                                            error = string.Format(Loc.Get("<LOC>Error creating chatroom {0}."), chatroom);
                                            ErrorLog.WriteLine(error, new object[0]);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        chatroom = string.Format("{0} {1}", chatroom2.DynamicRoot, num2);
                                        error = string.Format(Loc.Get("<LOC>Error creating chatroom {0}."), chatroom);
                                        ErrorLog.WriteLine(error, new object[0]);
                                        return false;
                                    }
                                }
                                catch
                                {
                                    chatroom = string.Format("{0} {1}", chatroom2.DynamicRoot, num2);
                                    EventLog.WriteLine("Creating dynamic chatroom {0}", new object[] { chatroom });
                                    if (!DataAccess.ExecuteQuery("CreateMainChatroom", new object[] { num2 }))
                                    {
                                        error = string.Format(Loc.Get("<LOC>Error creating chatroom {0}."), chatroom);
                                        ErrorLog.WriteLine(error, new object[0]);
                                        return false;
                                    }
                                }
                            }
                            if (!Create(chatroom, out error))
                            {
                                ErrorLog.WriteLine(error, new object[0]);
                                return false;
                            }
                        }
                        if (!Lobby.GetLobby().chatroomHelper.JoinChatroom(chatroom))
                        {
                            ErrorLog.WriteLine("Unable to join {0}.", new object[] { chatroom });
                            error = string.Format(Loc.Get("<LOC>Unable to join {0}."), chatroom);
                            return false;
                        }
                        EventLog.WriteLine("Joined Chatroom {0}.", new object[] { chatroom });
                        EventLog.WriteLine("Entering Chatroom {0}.", new object[] { chatroom });
                        mCurrent = new Chatroom(chatroom, 1, true);
                        error = string.Format(Loc.Get("<LOC>Sorry, that chat room is full. Redirecting you to {0}"), chatroom);
                        RoomOperator = null;
                        return true;
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                        ErrorLog.WriteLine("Unable to join {0}", new object[] { chatroom });
                        error = string.Format(Loc.Get("<LOC>Unable to join {0}."), chatroom);
                        return false;
                    }
                }
                if (chatroom2.Population <= 0)
                {
                    if (!Create(chatroom, out error))
                    {
                        ErrorLog.WriteLine(error, new object[0]);
                        return false;
                    }
                    if (!Lobby.GetLobby().chatroomHelper.JoinChatroom(chatroom))
                    {
                        ErrorLog.WriteLine("Unable to join {0}", new object[] { chatroom });
                        error = string.Format(Loc.Get("<LOC>Unable to join {0}."), chatroom);
                        return false;
                    }
                    EventLog.WriteLine("Joined Chatroom {0}", new object[] { chatroom });
                    mCurrent = chatroom2;
                    error = null;
                    RoomOperator = null;
                    return true;
                }
                if (!Lobby.GetLobby().chatroomHelper.JoinChatroom(chatroom))
                {
                    ErrorLog.WriteLine("Unable to join {0}", new object[] { chatroom });
                    error = string.Format(Loc.Get("<LOC>Unable to join {0}."), chatroom);
                    return false;
                }
                EventLog.WriteLine("Joined Chatroom {0}", new object[] { chatroom });
                mCurrent = chatroom2;
                error = null;
                RoomOperator = null;
                return true;
            }
            if (!DataAccess.GetBool("CustomChannelExists", new object[] { chatroom }) && !Create(chatroom, out error))
            {
                ErrorLog.WriteLine(error, new object[0]);
                return false;
            }
            if (!Lobby.GetLobby().chatroomHelper.JoinChatroom(chatroom))
            {
                ErrorLog.WriteLine("Unable to join {0}", new object[] { chatroom });
                error = string.Format(Loc.Get("<LOC>Unable to join {0}."), chatroom);
                return false;
            }
            EventLog.WriteLine("Joined Chatroom {0}", new object[] { chatroom });
            mCurrent = new Chatroom(chatroom, 1, false);
            error = null;
            RoomOperator = null;
            return true;
        }

        public static void JoinGame()
        {
            Leave();
            mCurrent = Game;
        }

        public static bool Leave()
        {
            try
            {
                if (((Current != null) && !Current.Equals(None)) && (Lobby.GetLobby() != null))
                {
                    string currentName = CurrentName;
                    bool flag = Current.IsDynamic && ((Current.Population - 1) < 1);
                    EventLog.WriteLine("Leaving chatroom {0}", new object[] { Current });
                    Lobby.GetLobby().chatroomHelper.LeaveChatroom();
                    mCurrent = None;
                    if (flag)
                    {
                        DataAccess.ExecuteQuery("ClearEmptyDynamicRooms", new object[0]);
                    }
                    if (LeftChat != null)
                    {
                        LeftChat(currentName);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Description, this.Population);
        }

        public int ClanID
        {
            get
            {
                return this.mClanID;
            }
        }

        public static Chatroom Current
        {
            get
            {
                return mCurrent;
            }
            internal set
            {
                mCurrent = value;
            }
        }

        public static string CurrentName
        {
            get
            {
                if (Current != null)
                {
                    return Current.Description;
                }
                return null;
            }
        }

        public static int CurrentPopulation
        {
            get
            {
                return GatheringParticipants.Count;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public string DynamicRoot
        {
            get
            {
                return this.mDynamicRoot;
            }
        }

        public static MappedObjectList<User> GatheringParticipants
        {
            get
            {
                return mGatheringParticipants;
            }
            set
            {
                mGatheringParticipants = value;
            }
        }

        public int GPGnetGameID
        {
            get
            {
                return this.mGPGnetGameID;
            }
        }

        public bool HasDynamicRoot
        {
            get
            {
                return ((this.mDynamicRoot != null) && (this.mDynamicRoot.Length > 0));
            }
        }

        public int HiddenPlayers
        {
            get
            {
                return this.mHiddenPlayers;
            }
            set
            {
                this.mHiddenPlayers = value;
            }
        }

        public static bool InChatroom
        {
            get
            {
                return ((Current != null) && (Current.Population >= 0));
            }
        }

        public bool IsClanRoom
        {
            get
            {
                return (this.mClanID >= 0);
            }
        }

        public bool IsDynamic
        {
            get
            {
                return this.mIsDynamic;
            }
        }

        public bool IsPersistent
        {
            get
            {
                return this.mIsPersistent;
            }
        }

        public bool IsSecure
        {
            get
            {
                return this.mIsSecure;
            }
        }

        internal static string[] MainChatNames
        {
            get
            {
                if (mMainChatNames == null)
                {
                    mMainChatNames = ConfigSettings.GetString("MainChatNames", null);
                }
                if (mMainChatNames != null)
                {
                    return mMainChatNames.Split(",".ToCharArray());
                }
                return null;
            }
        }

        public static int MaxRoomSize
        {
            get
            {
                if ((!User.Current.IsAdmin && !User.Current.IsModerator) && !(User.Current.Name == "GPGBot"))
                {
                    return (mMaxRoomSize = DataAccess.GetNumber("GetChatroomMax", new object[0]));
                }
                return (mMaxRoomSize = DataAccess.GetNumber("GetChatroomMax", new object[0]) + 0x3e8);
            }
        }

        public int Population
        {
            get
            {
                return this.mPopulation;
            }
            set
            {
                this.mPopulation = value;
                if (this.PopulationChanged != null)
                {
                    this.PopulationChanged(this, new PropertyChangedEventArgs("Population"));
                }
            }
        }

        public static string RoomOperator
        {
            get
            {
                if ((mRoomOperator == null) && InChatroom)
                {
                    if (Lobby.GetLobby() == null)
                    {
                        return null;
                    }
                    mRoomOperator = Lobby.GetLobby().chatroomHelper.ActiveChanelOperator();
                }
                return mRoomOperator;
            }
            set
            {
                mRoomOperator = value;
            }
        }

        public static int RoomRolloverSize
        {
            get
            {
                if (mRoomRolloverSize < 0)
                {
                    mRoomRolloverSize = DataAccess.GetNumber("GetChatroomRollover", new object[0]);
                }
                return mRoomRolloverSize;
            }
        }
    }
}

