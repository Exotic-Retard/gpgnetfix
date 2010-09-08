namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPGnetClientLib;
    using GPGnetClientLib.Lobby;
    using GPGnetCommunicationsLib;
    using GPGnetCommunicationsLib.SupcomInfo;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    public class GPGnetProtocol : IGPGnetProtocol
    {
        private string GatheringGameName;
        private List<string> mColumns;
        private IPEndPoint mEP1;
        private IPEndPoint mEP2;
        private string mGameName = string.Empty;
        private bool mIsGuest;
        private bool mIsInitialized;
        private bool mIsStrict;
        private string mLastRegIP = string.Empty;
        private IPEndPoint mLocalEP;
        private Queue<string> mMessages = new Queue<string>();
        private MatchmakingHelper.NatMessageDelegatePtr mNatMessageDelegate;
        private GameReporting mReport;
        private int mRowIndex;
        private List<List<object>> mRows;
        public string OldPassword = string.Empty;

        public bool AddPrivateURL(string address, int port, string roomName)
        {
            this.RegisterIPs();
            return true;
        }

        public bool AddQuery(string queryname, string query)
        {
            List<string> list;
            List<List<object>> list2;
            SQL.AdhocSQL("REPLACE INTO queries (query_name, query) VALUES ('" + queryname + "', '" + query.Replace("'", @"\'") + "')", out list, out list2);
            return true;
        }

        public bool AddResult(int gameID, string playerName, string faction, int kills, int deaths, int built, double damageDone, double damageReceived, string teamName, double energyConsumed, double energyProduced, double massConsumed, double massProduced, string result)
        {
            ReportSucpomPlayerInfo item = new ReportSucpomPlayerInfo {
                PlayerName = playerName,
                Faction = faction,
                Result = result
            };
            this.mReport.PlayerInfo.Add(item);
            return true;
        }

        public bool AddUnit(int gameID, string playerName, string unitID, int built, int lost, int killed, double damageDone, double damageReceived)
        {
            foreach (ReportSucpomPlayerInfo info in this.mReport.PlayerInfo)
            {
                if (info.PlayerName == playerName)
                {
                    ReportSupcomUnitInfo item = new ReportSupcomUnitInfo {
                        UnitID = unitID,
                        Built = built,
                        Lost = lost,
                        Killed = killed,
                        DamageDone = damageDone,
                        DamageReceived = damageReceived
                    };
                    info.Units.Add(item);
                    return true;
                }
            }
            return false;
        }

        public bool AdhocExecuteCommand(string SQL)
        {
            this.mRowIndex = 0;
            return GPGnetClientLib.Lobby.SQL.AdhocSQL(SQL, out this.mColumns, out this.mRows);
        }

        public bool AdhocExecuteReader(string SQL)
        {
            this.mRowIndex = 0;
            return GPGnetClientLib.Lobby.SQL.AdhocSQL(SQL, out this.mColumns, out this.mRows);
        }

        public bool ChangePassword(string password)
        {
            return Authentication.ChangePassword(this.OldPassword, password);
        }

        private void ClientManager_OnLogData(object logdata)
        {
            if (logdata != null)
            {
                EventLog.WriteLine(logdata.ToString(), LogCategory.Get("CUSTOMClientManager"), new object[0]);
            }
        }

        public bool CommandMessageChatroom(string roomName, string Message)
        {
            string roomname = roomName;
            if (roomname == null)
            {
                roomname = this.mGameName;
            }
            Gatherings.TextMessage(roomname, Message);
            return true;
        }

        public bool CommandMessagePlayer(string playerName, string message)
        {
            Gatherings.PrivateMessage(playerName, message);
            return true;
        }

        public bool CreateChatroom(string ChatroomName)
        {
            return Gatherings.CreateRoom("Gathering", ChatroomName, "");
        }

        public bool CreateGame(string description, string mapName, int rating, int p, string gameName, string version, string customData, string roomName)
        {
            this.RegisterAddress(GPGnetSelectedGame.SelectedGame.Port);
            List<string> list = new List<string> {
                mapName,
                customData
            };
            if (Gatherings.CreateRoom("CGameGathering", description, "", list.ToArray()))
            {
                Gatherings.JoinRoom(description, "");
                this.GatheringGameName = description;
                gameName = description;
                return true;
            }
            return false;
        }

        public bool CreateGameReport(int gameID, string newmap, double duration, string gametype)
        {
            this.mReport = new GameReporting();
            this.mReport.GameID = gameID;
            this.mReport.Map = newmap;
            this.mReport.Duration = duration;
            this.mReport.GameType = gametype;
            this.mReport.GPGnetGameID = GPGnetSelectedGame.SelectedGame.GameID;
            return true;
        }

        public bool CreateUser(string username, string password, string email, string server, int port)
        {
            this.mIsGuest = false;
            this.Initialize(server, port);
            if (username.ToUpper().IndexOf("_T_E_M_P_") >= 0)
            {
                this.mIsGuest = true;
                return true;
            }
            return Authentication.CreateLogin(username, password, email);
        }

        public bool DeletePersistantMessage(int messageId, string roomName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private bool EmailUsername(string cdkey)
        {
            return Authentication.EmailUsername(cdkey);
        }

        public bool EndGame()
        {
            DataAccess.ExecuteQuery("SetGatheringState", new object[] { 6, this.GatheringGameName });
            return true;
        }

        public bool ExecuteQuery(string queryname, string[] paramlist)
        {
            int num;
            if (this.mIsGuest)
            {
                return false;
            }
            List<string> list = new List<string>();
            foreach (string str in paramlist)
            {
                list.Add(str.Replace("'", @"\'"));
            }
            return SQL.SQLExecute(queryname, list.ToArray(), out num);
        }

        public string FindPlayerByID(int playerid)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void Gatherings_OnChatroomMessage(Credentials sourceuser, string roomname, string message)
        {
            lock (this.mMessages)
            {
                if (message.StartsWith("/"))
                {
                    this.mMessages.Enqueue(message);
                }
                else
                {
                    this.mMessages.Enqueue("<" + sourceuser.Name + "> " + message);
                }
            }
        }

        private void Gatherings_OnPrivateMessage(Credentials sourceuser, string message)
        {
            lock (this.mMessages)
            {
                if (message.StartsWith("/"))
                {
                    this.mMessages.Enqueue(message);
                }
                else
                {
                    this.mMessages.Enqueue("<" + sourceuser.Name + "> " + message);
                }
            }
        }

        public string GetChannelOperator(string roomName)
        {
            return "";
        }

        public string GetColumns(string Delimiter)
        {
            string str = "";
            string str2 = "";
            foreach (string str3 in this.mColumns)
            {
                str = str + str2 + str3;
                str2 = Delimiter;
            }
            return str;
        }

        public SNatMessage GetNatMessage()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetNextMessage(string roomName)
        {
            lock (this.mMessages)
            {
                if (this.mMessages.Count > 0)
                {
                    return this.mMessages.Dequeue();
                }
                return "";
            }
        }

        public string GetNextPersistentMessage(string roomName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetNextRow(string Delimiter)
        {
            if (this.mRowIndex >= this.mRows.Count)
            {
                return "";
            }
            string str = "";
            string str2 = "";
            foreach (object obj2 in this.mRows[this.mRowIndex])
            {
                str = str + str2 + obj2.ToString();
                str2 = Delimiter;
            }
            this.mRowIndex++;
            return str;
        }

        public bool GetPersistentMessages(string roomName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetQueryDataset(string queryname, string[] paramlist, bool useMaster)
        {
            List<string> list2;
            List<List<object>> list3;
            if (this.mIsGuest)
            {
                return "";
            }
            List<string> list = new List<string>();
            foreach (string str in paramlist)
            {
                list.Add(str.Replace("'", @"\'"));
            }
            if (!SQL.SQLData(queryname, list.ToArray(), out list2, out list3))
            {
                return "";
            }
            string str2 = "";
            string str3 = "";
            foreach (string str4 in list2)
            {
                str3 = str3 + str2 + str4;
                str2 = "|";
            }
            string str5 = '\x0003'.ToString();
            foreach (List<object> list4 in list3)
            {
                str2 = "";
                str3 = str3 + str5;
                foreach (object obj2 in list4)
                {
                    str3 = str3 + str2 + obj2.ToString();
                    str2 = "|";
                }
            }
            return str3;
        }

        private void GPGnetSelectedGame_OnGameChanged(object sender, EventArgs e)
        {
            this.RegisterAddress(GPGnetSelectedGame.SelectedGame.Port);
        }

        private void Initialize(string server, int port)
        {
            if (!this.mIsInitialized)
            {
                UPnP.OpenPort();
                ClientManager.ResetManager();
                ClientManager.SetConnection(server, port);
                ClientManager.GetManager();
                ClientManager.OnLogData += new LogDelegate(this.ClientManager_OnLogData);
                Gatherings.OnChatroomMessage += new ChatroomMessage(this.Gatherings_OnChatroomMessage);
                Gatherings.OnPrivateMessage += new PrivateMessage(this.Gatherings_OnPrivateMessage);
                Gatherings.Initialize();
                GPGnetSelectedGame.OnGameChanged += new EventHandler(this.GPGnetSelectedGame_OnGameChanged);
                this.mIsInitialized = true;
            }
        }

        public bool IsConnected()
        {
            return true;
        }

        public bool JoinChatroom(string ChatroomName)
        {
            return Gatherings.JoinRoom(ChatroomName, "");
        }

        public string JoinGame(string GameName)
        {
            this.RegisterAddress(GPGnetSelectedGame.SelectedGame.Port);
            if (Gatherings.JoinRoom(GameName, string.Empty))
            {
                DataList queryData = DataAccess.GetQueryData("GetGameByName2", new object[] { GameName, GPGnetSelectedGame.SelectedGame.GameID });
                if (queryData.Count > 0)
                {
                    string str = queryData[0]["url"];
                    this.GatheringGameName = str;
                    return str;
                }
            }
            return "NONE";
        }

        public bool LaunchSession(string stationURL)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool LeaveChatroom(string roomName)
        {
            return Gatherings.LeaveRoom(roomName);
        }

        public bool LeaveGame()
        {
            return this.LeaveChatroom(this.GatheringGameName);
        }

        public bool LoginUser(string username, string password, string server, int port)
        {
            this.mIsGuest = false;
            this.Initialize(server, port);
            if (username.ToUpper().IndexOf("_T_E_M_P_") >= 0)
            {
                this.mIsGuest = true;
                return true;
            }
            bool flag = Authentication.Login(username, password);
            if (flag && (GPGnetSelectedGame.SelectedGame != null))
            {
                this.RegisterAddress(GPGnetSelectedGame.SelectedGame.Port);
            }
            return flag;
        }

        public void Logout()
        {
            Authentication.Logout();
        }

        public bool MessagePlayer(string playerName, string message)
        {
            Gatherings.PrivateMessage(playerName, message);
            return true;
        }

        public bool PersistMessageChatroom(string roomName, string message)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int ProbeStations()
        {
            return 0;
        }

        public object ReceiveNatMessage(string address, byte[] message, int size)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object RegisterAdapter(int header)
        {
            return null;
        }

        public bool RegisterAddress(int port)
        {
            try
            {
                IPEndPoint point;
                IPEndPoint point2;
                if (Gatherings.GetStunResults(port, out point, out point2))
                {
                    this.mEP1 = point;
                    this.mEP2 = point2;
                    this.mIsStrict = this.mEP1.ToString() != this.mEP2.ToString();
                    if (this.mIsStrict)
                    {
                        EventLog.WriteLine("[GPGnetProtocol] WARNING: Symmetric (strict) NAT detected.", LogCategory.Get("Event"), new object[0]);
                    }
                    this.RegisterIPs();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return (this.mEP1 != null);
        }

        private void RegisterIPs()
        {
            foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (((address.AddressFamily != AddressFamily.InterNetworkV6) && (address.AddressFamily == AddressFamily.InterNetwork)) && (address.ToString() != "127.0.0.1"))
                {
                    this.mLocalEP = new IPEndPoint(address, GPGnetSelectedGame.SelectedGame.Port);
                }
            }
            string mLastRegIP = string.Empty;
            if (this.mEP1 != null)
            {
                mLastRegIP = this.mEP1.ToString().Replace(":", ";") + ";0";
                this.mLastRegIP = mLastRegIP;
            }
            else if (this.mLastRegIP != string.Empty)
            {
                mLastRegIP = this.mLastRegIP;
            }
            if (this.mLocalEP != null)
            {
                if (mLastRegIP != string.Empty)
                {
                    mLastRegIP = mLastRegIP + ";";
                }
                mLastRegIP = mLastRegIP + this.mLocalEP.ToString().Replace(":", ";") + ";0";
            }
            DataAccess.QueueExecuteQuery("SetParticipationStatus", new object[] { mLastRegIP });
        }

        public void RegisterNatResponse(MatchmakingHelper.NatMessageDelegatePtr natMessageDelegatePtr)
        {
            this.mNatMessageDelegate = natMessageDelegatePtr;
        }

        public bool RegisterUDPConnection()
        {
            return true;
        }

        public bool ReportGame(int GameID, int Kills, int Deaths, ResultsInfo[] results)
        {
            throw new Exception("Report Game is depricated.  This was GPGnet 1.5.37 original supcom gold.");
        }

        public bool ResetPassword(string username, string email)
        {
            if (email == "dummy@gaspowered.com")
            {
                return this.EmailUsername(username);
            }
            return Authentication.ResetPassword(username, email);
        }

        public bool StartGame()
        {
            DataAccess.ExecuteQuery("SetGatheringState", new object[] { 2, this.GatheringGameName });
            return true;
        }

        public bool SubmitGameReport()
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.ReportSupcomGame
            };
            command.SetParams(new object[] { this.mReport });
            ClientManager.GetManager().MessageServer(command);
            this.mReport = null;
            return true;
        }

        public bool TextMessageChatroom(string roomName, string Message)
        {
            if (roomName == null)
            {
                string mGameName = this.mGameName;
            }
            Gatherings.TextMessage(roomName, Message);
            return true;
        }

        public bool UpdateGame(string mGameName, string mMapName, int mRating, int p, string gameName, string mVersion, string mCustomData, string roomName)
        {
            return false;
        }
    }
}

