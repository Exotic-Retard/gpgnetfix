namespace GPG.Multiplayer.Quazal
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    internal class ChatroomHelper
    {
        public static string BAN = "/ban";
        public static string CHICKEN = "/chicken";
        public static string CLAN_SHOUT = "/clanshout";
        public static string CLEAR = "/clear";
        public static string COLOR = "/color";
        public static string COW = "/cow";
        public static string DND_MESSAGE = "is in do not disturb mode.";
        public static string DONOTDISTURB = "/dnd";
        public static string EMOTE = "/em";
        public static string FRIEND = "/friend";
        public static string FRIEND_OFFLINE = "//FRIENDLOGOFF";
        public static string FRIEND_ONLINE = "//FRIENDONLINE";
        public static string FRIEND_ONLINE_RESPONSE = "//FRIENDONLINERESPONSE";
        public static string FRIEND_SHOUT = "/friendshout";
        public static string GATHERING = "/gathering";
        public static string HELP = "/help";
        public static string IGNORE = "/ignore";
        public static string JOIN = "/join";
        public static string KICK = "/kick";
        public static string LIST_FRIENDS = "/list";
        private string mContainername = "default";
        private string mLastChatRoomName;
        private ArrayList mMessageList = ArrayList.Synchronized(new ArrayList());
        public static string MOTD = "Message of the day";
        private int mPollStartDelay = 0x3e8;
        public static string MUTE = "/mute";
        public static string REPLY = "/r";
        public static string STATS = "/stats";
        public static string UNFRIEND = "/unfriend";
        public static string UNIGNORE = "/unignore";
        public static string WELCOME = "/welcome";
        public static string WHISPER = "/w";

        internal ChatroomHelper(string containerName)
        {
            this.mContainername = containerName;
        }

        public string ActiveChanelOperator()
        {
            string str2;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.GetChannelOperator(this.mLastChatRoomName);
                }
                str2 = Marshal.PtrToStringAnsi(mActiveChanelOperator(this.mContainername));
            }
            finally
            {
                Lobby.Unlock();
            }
            return str2;
        }

        public bool CommandMessagePlayer(string playerName, string message)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                if (Lobby.GetLobby().IsConnected())
                {
                    if (Lobby.sProtocol != null)
                    {
                        return Lobby.sProtocol.CommandMessagePlayer(playerName, message);
                    }
                    return mCommandMessagePlayer(playerName, message, this.mContainername);
                }
                flag = false;
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

        public bool CreateChatroom(string ChatroomName)
        {
            bool flag2;
            Lobby.Lock();
            try
            {
                this.LastChatRoomName = ChatroomName;
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.CreateChatroom(ChatroomName);
                }
                bool flag = mCreateChatroom(ChatroomName, this.mContainername);
                Lobby.IncDBCallCount(new object[0]);
                flag2 = flag;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag2;
        }

        public bool DeletePersistMessage(int messageId)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.DeletePersistantMessage(messageId, this.mLastChatRoomName);
                }
                flag = mDeletePersistMessage(messageId, this.mContainername);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public string GetNextMessage()
        {
            string str3;
            if (Lobby.LoggingOut)
            {
                return "";
            }
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.GetNextMessage(this.mLastChatRoomName);
                }
                string str = Marshal.PtrToStringAnsi(mGetNextMessage(this.mContainername));
                if (str.IndexOf("COMMANDMESSAGE") == 0)
                {
                    string[] strArray = str.Split(" ".ToCharArray(), 3);
                    str = strArray[2];
                    if (str.IndexOf("/CUST") >= 0)
                    {
                        int num = Lobby.GetLobby().authenticationHelper.FindIDByPlayerName(strArray[1]);
                        int index = str.IndexOf(' ', 6);
                        int startIndex = str.IndexOf(' ', index + 1);
                        str = str.Substring(0, index).Trim() + " " + num.ToString() + " " + str.Substring(startIndex).Trim();
                    }
                }
                str3 = str;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                str3 = "";
            }
            finally
            {
                Lobby.Unlock();
            }
            return str3;
        }

        public string GetNextPersistentMessage()
        {
            string str2;
            Lobby.Lock();
            try
            {
                string str = "";
                try
                {
                    if (Lobby.sProtocol != null)
                    {
                        return Lobby.sProtocol.GetNextPersistentMessage(this.mLastChatRoomName);
                    }
                    str = Marshal.PtrToStringAnsi(mGetNextPersistentMessage(this.mContainername));
                    this.mMessageList.Add(str);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                str2 = str;
            }
            finally
            {
                Lobby.Unlock();
            }
            return str2;
        }

        public bool GetPersistentMessages()
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.GetPersistentMessages(this.mLastChatRoomName);
                }
                this.mMessageList.Clear();
                flag = mGetPersistentMessages(this.mContainername);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool JoinChatroom(string ChatroomName)
        {
            bool flag2;
            Lobby.Lock();
            try
            {
                this.LastChatRoomName = ChatroomName;
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.JoinChatroom(ChatroomName);
                }
                bool flag = mJoinChatroom(ChatroomName, this.mContainername);
                Lobby.IncDBCallCount(new object[0]);
                flag2 = flag;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag2;
        }

        public bool LeaveChatroom()
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.LeaveChatroom(this.mLastChatRoomName);
                }
                this.LastChatRoomName = "";
                flag = mLeaveChatroom(this.mContainername);
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

        [DllImport("MultiplayerBackend.dll", EntryPoint="ActiveChanelOperator", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr mActiveChanelOperator([MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="CommandMessageChatroom", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mCommandMessageChatroom([MarshalAs(UnmanagedType.LPStr)] string Message, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="CommandMessagePlayer", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mCommandMessagePlayer([MarshalAs(UnmanagedType.LPStr)] string playerName, [MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="CreateChatroom", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mCreateChatroom([MarshalAs(UnmanagedType.LPStr)] string ChatroomName, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="DeletePersistMessage", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mDeletePersistMessage(int messageId, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        public bool MessageChatroom(string Message)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                if (Message.StartsWith("/"))
                {
                    if (Lobby.sProtocol != null)
                    {
                        return Lobby.sProtocol.CommandMessageChatroom(this.mLastChatRoomName, Message);
                    }
                    return mCommandMessageChatroom(Message, this.mContainername);
                }
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.TextMessageChatroom(this.mLastChatRoomName, Message);
                }
                flag = mMessageChatroom(Message, this.mContainername);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool MessagePlayer(string playerName, string message)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.MessagePlayer(playerName, message);
                }
                flag = mMessagePlayer(playerName, message, this.mContainername);
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

        [DllImport("MultiplayerBackend.dll", EntryPoint="GetNextMessage", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr mGetNextMessage([MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetNextPersistentMessage", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr mGetNextPersistentMessage([MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="GetPersistentMessages", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mGetPersistentMessages([MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="JoinChatroom", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Unicode, SetLastError=true)]
        private static extern bool mJoinChatroom([MarshalAs(UnmanagedType.LPStr)] string ChatroomName, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="LeaveChatroom", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mLeaveChatroom([MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="MessageChatroom", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mMessageChatroom([MarshalAs(UnmanagedType.LPStr)] string Message, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="MessagePlayer", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mMessagePlayer([MarshalAs(UnmanagedType.LPStr)] string playerName, [MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        [DllImport("MultiplayerBackend.dll", EntryPoint="PersistMessageChatroom", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mPersistMessageChatroom([MarshalAs(UnmanagedType.LPStr)] string message, [MarshalAs(UnmanagedType.LPStr)] string ContainerName);
        public bool PersistMessageChatroom(string message)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.PersistMessageChatroom(this.mLastChatRoomName, message);
                }
                if (this.GetPersistentMessages())
                {
                    while (this.GetNextPersistentMessage() != "")
                    {
                    }
                }
                if (message.IndexOf(WELCOME) >= 0)
                {
                    foreach (string str in this.mMessageList)
                    {
                        if ((str.IndexOf(WELCOME) >= 0) && (str.IndexOf(" ") >= 0))
                        {
                            int messageId = Convert.ToInt32(str.Split(new char[] { ' ' })[0]);
                            this.DeletePersistMessage(messageId);
                        }
                    }
                }
                if (message.IndexOf(CLEAR) >= 0)
                {
                    string[] strArray = message.Split(new char[] { ' ' });
                    if (strArray.Length >= 2)
                    {
                        string str2 = strArray[1];
                        foreach (string str3 in this.mMessageList)
                        {
                            if (str3.ToUpper().IndexOf(str2.ToUpper()) >= 0)
                            {
                                int num2 = Convert.ToInt32(str3.Split(new char[] { ' ' })[0]);
                                this.DeletePersistMessage(num2);
                            }
                        }
                    }
                }
                Lobby.IncDBCallCount(new object[0]);
                flag = mPersistMessageChatroom(message, this.mContainername);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public string LastChatRoomName
        {
            get
            {
                return this.mLastChatRoomName;
            }
            set
            {
                this.mLastChatRoomName = value;
            }
        }
    }
}

