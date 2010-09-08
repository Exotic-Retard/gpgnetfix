namespace GPG.Multiplayer.Game.SupremeCommander
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Game;
    using GPG.Multiplayer.Game.Network;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class SupComGameManager : IGameManager, IDisposable
    {
        private const string FakeParams = "";
        public static string GameArgs = "";
        public static string LastLocation = "";
        private int mBottleNeckCooldown;
        private Hashtable mConnections = Hashtable.Synchronized(new Hashtable());
        private SupcomGameInfo mGameInfo = new SupcomGameInfo();
        private string mGameName = "";
        private GPG.Multiplayer.Game.GameState mGameState;
        private string mHostAddress = "";
        private string mHostName = "";
        private bool mIsAborting;
        private bool mIsHost;
        private bool mIsJoined;
        private string mMapName = "";
        private string mMyTeam = "";
        private string mOtherTeam = "";
        private string mPassword = "";
        private string mReplayID = "";
        private List<Driver.InputEventArgs> mResultsQueue = new List<Driver.InputEventArgs>();
        private string mStats = "";
        private bool mStatsShutdown;
        private StatsWatcher mStatsWatcher;
        private Hashtable mStdConnectsIssued = Hashtable.Synchronized(new Hashtable());
        private SupcomTCPConnection mSupcomTCPConnection;
        private TrafficManager mTrafficManager = new TrafficManager();
        private INetConnection mUdp;
        public static bool sCanCheckConnections = false;
        public static bool sLiveReplayMode = false;

        public event EventHandler BeforeExit;

        public event EventHandler GameHosted;

        public event StringEventHandler OnAbortGame;

        public event EventHandler OnExit;

        public event EventHandler OnGameLaunched;

        public event StatsXML OnStatsXML;

        public SupComGameManager()
        {
            this.mTrafficManager.OnReceiveMessage += new GPG.Multiplayer.Game.Network.ReceiveMessage(this.ReceiveMessage);
            this.mTrafficManager.OnNewPlayer += new NewPlayer(this.mTrafficManager_OnNewPlayer);
            this.mTrafficManager.OnRemovePlayer += new RemovePlayer(this.mTrafficManager_OnRemovePlayer);
            this.mTrafficManager.OnCommandMessage += new MessageEventHandler(this.mTrafficManager_OnCommandMessage);
            this.mStatsWatcher = new StatsWatcher(this);
        }

        public void AbortGame()
        {
            this.mSupcomTCPConnection.Close();
            this.mTrafficManager.Dispose();
            this.mTrafficManager = null;
        }

        private void CheckGameGatheringConnections()
        {
            int @int = ConfigSettings.GetInt("GameRebroadcastMaxAttempts", 60);
            int num2 = 0;
            sCanCheckConnections = true;
            while (((this.GameState != GPG.Multiplayer.Game.GameState.Launching) && (this.GameState != GPG.Multiplayer.Game.GameState.Playing)) && ((this.GameState != GPG.Multiplayer.Game.GameState.Aborted) && sCanCheckConnections))
            {
                if (User.GetProtocol() != null)
                {
                    Thread.Sleep(ConfigSettings.GetInt("GameRebroadcastTime", 0x7d0));
                    num2++;
                    if (num2 <= @int)
                    {
                        goto Label_0056;
                    }
                    continue;
                }
                Thread.Sleep(ConfigSettings.GetInt("BroadcastTime", 0x4e20));
            Label_0056:
                if ((this.GameState != GPG.Multiplayer.Game.GameState.Launching) && (this.GameState != GPG.Multiplayer.Game.GameState.Playing))
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "GetGameIps", this, "FinishConnectivity", new object[0]);
                }
                if (this.mIsHost)
                {
                    this.MessageGame("//MAP " + this.mGameInfo.Map);
                }
                if (ConfigSettings.GetBool("ForcePollConnections", false))
                {
                    sCanCheckConnections = true;
                }
            }
        }

        private void ConnectPeer(string address, int port, string playerName, int playerID)
        {
            EventLog.WriteLine(string.Concat(new object[] { "Called connect peer ", address, " ", playerName, " ", playerID }), LogCategory.Get("SupComGameManager"), new object[0]);
            EventLog.WriteLine("Var info... Host Addr:" + this.mHostAddress + " HostName:" + this.mHostName + " Current User:" + User.Current.ID.ToString(), LogCategory.Get("SupComGameManager"), new object[0]);
            if (playerID != User.Current.ID)
            {
                EventLog.WriteLine("playerID does not equal UserID", LogCategory.Get("SupComGameManager"), new object[0]);
                if (playerName != this.mHostName)
                {
                    EventLog.WriteLine("Player is not the host", LogCategory.Get("SupComGameManager"), new object[0]);
                    if (!this.mStdConnectsIssued.ContainsKey(playerID))
                    {
                        EventLog.WriteLine("Adding connection.", LogCategory.Get("SupComGameManager"), new object[0]);
                        this.mStdConnectsIssued.Add(playerID, null);
                        this.mGameInfo.PlayerByName(playerName).PlayerID = playerID;
                        this.mSupcomTCPConnection.SendMessage("ConnectToPeer", new object[] { address + ":" + port.ToString(), playerName, playerID });
                    }
                }
            }
        }

        private uint convertIp(string address)
        {
            uint num = 0;
            string[] strArray = address.Split(new char[] { '.' });
            if (strArray.Length == 4)
            {
                num = (uint) ((((((Convert.ToInt32(strArray[0]) * 0x100) * 0x100) * 0x100) + ((Convert.ToInt32(strArray[1]) * 0x100) * 0x100)) + (Convert.ToInt32(strArray[2]) * 0x100)) + Convert.ToInt32(strArray[3]));
            }
            return num;
        }

        private void DisconnectPeer(int playerID)
        {
            if (this.mStdConnectsIssued.ContainsKey(playerID))
            {
                if (this.GameState == GPG.Multiplayer.Game.GameState.Lobby)
                {
                    this.mGameInfo.RemoveByPlayerID(playerID);
                }
                this.mStdConnectsIssued.Remove(playerID);
            }
            this.mSupcomTCPConnection.SendMessage("DisconnectFromPeer", new object[] { playerID });
        }

        public void Dispose()
        {
            this.mTrafficManager.Dispose();
            this.mTrafficManager = null;
        }

        private void EjectFromGame(int playerID)
        {
            if (this.mStdConnectsIssued.ContainsKey(playerID))
            {
                this.mStdConnectsIssued.Remove(playerID);
            }
            this.mSupcomTCPConnection.SendMessage("EjectPlayer", new object[] { playerID });
        }

        private void EndGame()
        {
            this.mSupcomTCPConnection.SendMessage("Shutdown", new object[0]);
            this.mSupcomTCPConnection.Close();
            if (this.mIsHost)
            {
                Game.EndGame();
            }
            this.mStatsWatcher.Stop();
        }

        private void EstablishConnections()
        {
            if (this.GameState != GPG.Multiplayer.Game.GameState.Aborted)
            {
                EventLog.WriteLine("Called establish connections", LogCategory.Get("SupComGameManager"), new object[0]);
                if (this.mIsHost)
                {
                    if (!this.mIsJoined || ConfigSettings.GetBool("ForceHostAnyway", false))
                    {
                        this.mSupcomTCPConnection.SendMessage("HostGame", new object[] { this.mLaunchMap });
                    }
                    this.mIsJoined = true;
                }
                else
                {
                    foreach (PlayerInformation information in this.mConnections.Values)
                    {
                        if ((((information != PlayerInformation.Empty()) && (information.PlayerID != User.Current.ID)) && ((this.mHostName != null) && (information.PlayerName.ToUpper() == this.mHostName.ToUpper()))) && !this.mIsJoined)
                        {
                            EventLog.WriteLine("Found a player to join.", LogCategory.Get("SupComGameManager"), new object[0]);
                            this.mIsJoined = true;
                            this.mSupcomTCPConnection.SendMessage("JoinGame", new object[] { information.EndPoint.Address.ToString() + ":" + information.EndPoint.Port.ToString(), information.PlayerName, information.PlayerID });
                        }
                    }
                }
                if (this.mIsJoined)
                {
                    foreach (PlayerInformation information2 in this.mConnections.Values)
                    {
                        if (information2 != PlayerInformation.Empty())
                        {
                            this.ConnectPeer(information2.EndPoint.Address.ToString(), information2.EndPoint.Port, information2.PlayerName, information2.PlayerID);
                        }
                    }
                }
                if (this.MyTeam != "")
                {
                    this.MessageGame("//TEAMNAME " + this.MyTeam);
                }
            }
        }

        private void FinishConnectivity(object result)
        {
            try
            {
                DataList list = result as DataList;
                if ((list != null) && (this.mTrafficManager != null))
                {
                    foreach (DataRecord record in list)
                    {
                        Convert.ToInt32(record["pid"]);
                        string[] strArray = record["status"].Split(";,".ToCharArray());
                        if (strArray.Length == 6)
                        {
                            string ipaddr = strArray[0];
                            int port = Convert.ToInt32(strArray[1]);
                            this.mTrafficManager.CreateNewPlayer(ipaddr, port);
                            if (ConfigSettings.GetBool("GetPrivateAddr", false))
                            {
                                ipaddr = strArray[3];
                                port = Convert.ToInt32(strArray[4]);
                                this.mTrafficManager.CreateNewPlayer(ipaddr, port);
                            }
                        }
                        else if (strArray.Length >= 3)
                        {
                            string str2 = strArray[0];
                            int num2 = Convert.ToInt32(strArray[1]);
                            this.mTrafficManager.CreateNewPlayer(str2, num2);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            this.EstablishConnections();
        }

        private void FinishCreateGame(bool result)
        {
            if (result)
            {
                this.EstablishConnections();
                if (this.GameHosted != null)
                {
                    this.GameHosted(this, EventArgs.Empty);
                }
            }
        }

        private void FinishJoinGame(string url)
        {
            if (url == "NONE")
            {
                this.ForceCloseGame(Loc.Get("<LOC>This game can no longer be joined."));
            }
            else
            {
                this.mHostAddress = url.Split("/;".ToCharArray())[1].Split("=".ToCharArray())[1];
                if (url == "")
                {
                    throw new Exception("Failed to join the game.");
                }
                this.MessageGame("//ATTEMPTJOIN " + User.Current.Name);
                ThreadQueue.Quazal.Enqueue(typeof(Game), "ProbeStations", this, "FinishProbeStations", new object[0]);
            }
        }

        private void FinishProbeStations(int result)
        {
            ThreadQueue.Quazal.Enqueue(typeof(Game), "GetGameIps", this, "FinishConnectivity", new object[0]);
        }

        public void ForceAddPlayer(string name, string faction)
        {
            int num = this.mGameInfo.Players.Count + 1;
            SupcomPlayerInfo item = new SupcomPlayerInfo {
                PlayerName = name,
                PlayerID = num,
                StartSpot = num,
                Color = num.ToString(),
                Team = "FFA",
                Faction = faction,
                Army = num - 1
            };
            this.mGameInfo.Players.Add(item);
        }

        public void ForceCloseGame(string message)
        {
            if (ConfigSettings.GetBool("CanForceClose", true) && !this.mIsAborting)
            {
                this.mIsAborting = true;
                EventLog.WriteLine("ForceClose: " + message, LogCategory.Get("Command"), new object[0]);
                if (this.mIsHost)
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "ForceClose: " + message, this.mGameName, this.mGameName, this.mGameName } });
                }
                if (this.mSupcomTCPConnection != null)
                {
                    this.mSupcomTCPConnection.ForceClose();
                    if (this.OnAbortGame != null)
                    {
                        this.OnAbortGame(message);
                    }
                }
            }
        }

        public void ForceMap(string mapname)
        {
            this.mMapName = mapname;
            if (this.mGameInfo != null)
            {
                this.mGameInfo.Map = mapname;
            }
        }

        public string GetGameLocation()
        {
            if (LastLocation != "")
            {
                return LastLocation;
            }
            OpenFileDialog dialog = new OpenFileDialog {
                Filter = "All Executables|*.exe"
            };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                throw new Exception(Loc.Get("<LOC>There is no valid game location to launch."));
            }
            LastLocation = dialog.FileName;
            return dialog.FileName;
        }

        private string GetParams()
        {
            return ("password=" + this.mPassword);
        }

        public void GetStats()
        {
        }

        public bool HostGame(bool automatch, string gameName)
        {
            return this.HostGame(automatch, gameName, "");
        }

        public bool HostGame(bool automatch, string gameName, string commandargs)
        {
            this.mGameName = gameName;
            this.mIsHost = true;
            string arguements = commandargs + " " + GameArgs;
            if (this.DoReplay)
            {
                this.mReplayID = Guid.NewGuid().ToString();
                string str2 = User.Current.Name + "." + gameName.Replace(" ", "_.scrply");
                arguements = arguements + " /savereplay " + this.GPGNetURL + str2;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "ReplayHosted", new object[] { User.Current.ID, str2, this.mReplayID } });
            }
            if (User.GetProtocol() != null)
            {
                User.GetProtocol().RegisterUDPConnection();
            }
            this.mSupcomTCPConnection = new SupcomTCPConnection(automatch, this.GetGameLocation(), arguements, "localhost", 0, false, true);
            this.mSupcomTCPConnection.OnGetCommand += new EventHandler<Driver.InputEventArgs>(this.mSupcomTCPConnection_OnGetCommand);
            this.mSupcomTCPConnection.Bind(this.Port, User.Current.ID);
            this.mSupcomTCPConnection.OnExit += new EventHandler(this.mSupcomTCPConnection_OnExit);
            this.mTrafficManager.SetConnection(this.mSupcomTCPConnection);
            this.RegisterPrivateURL();
            this.SetUpAddressPoll();
            return true;
        }

        private bool IsRegularRankedGame()
        {
            return ((ConfigSettings.GetString("Automatch GameIDs", "27") + " ").IndexOf(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " ") >= 0);
        }

        public bool JoinGame(bool automatch, string gameName, string address, int port, string hostname)
        {
            return this.JoinGame(automatch, gameName, address, port, hostname, "");
        }

        public bool JoinGame(bool automatch, string gameName, string address, int port, string hostname, string commandargs)
        {
            this.mHostName = hostname;
            this.mGameName = gameName.Replace(@"\\'", "'").Replace("\\\\\"", "\"");
            string arguements = commandargs + " " + GameArgs;
            if (this.DoReplay)
            {
                this.mReplayID = Guid.NewGuid().ToString();
                string str2 = User.Current.Name + "." + gameName.Replace(" ", "_.scrply");
                arguements = arguements + " /savereplay " + this.GPGNetURL + str2;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "ReplayJoined", new object[] { User.Current.ID, str2, this.mReplayID } });
            }
            if (User.GetProtocol() != null)
            {
                User.GetProtocol().RegisterUDPConnection();
            }
            this.mSupcomTCPConnection = new SupcomTCPConnection(automatch, this.GetGameLocation(), arguements, "localhost", 0, false, false);
            this.mSupcomTCPConnection.OnGetCommand += new EventHandler<Driver.InputEventArgs>(this.mSupcomTCPConnection_OnGetCommand);
            this.mSupcomTCPConnection.Bind(this.Port, User.Current.ID);
            this.mSupcomTCPConnection.OnExit += new EventHandler(this.mSupcomTCPConnection_OnExit);
            this.mTrafficManager.SetConnection(this.mSupcomTCPConnection);
            this.RegisterPrivateURL();
            this.SetUpAddressPoll();
            return true;
        }

        public void MessageGame(string message)
        {
            if (this.GameState != GPG.Multiplayer.Game.GameState.Aborted)
            {
                EventLog.WriteLine("Sending Game Message: " + message, LogCategory.Get("SupComGameManager"), new object[0]);
                ThreadQueue.Quazal.Enqueue(typeof(Game), "MessageGame", null, null, new object[] { message });
            }
        }

        private void mSupcomTCPConnection_OnExit(object sender, EventArgs e)
        {
            try
            {
                EventLog.WriteLine("Exiting supcom.", LogCategory.Get("SupComGameManager"), new object[0]);
                if (this.BeforeExit != null)
                {
                    this.BeforeExit(this, EventArgs.Empty);
                }
                EventLog.WriteLine("BeforeExit Complete.", LogCategory.Get("SupComGameManager"), new object[0]);
                try
                {
                    Thread.Sleep(0x3e8);
                }
                catch (ThreadInterruptedException exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                try
                {
                    Thread.Sleep(0x3e8);
                }
                catch (ThreadInterruptedException exception2)
                {
                    ErrorLog.WriteLine(exception2);
                }
                try
                {
                    Thread.Sleep(0x3e8);
                }
                catch (ThreadInterruptedException exception3)
                {
                    ErrorLog.WriteLine(exception3);
                }
                this.MessageGame("//NOTIFYEXIT " + User.Current.ID.ToString() + " " + User.Current.Name);
                if (this.mGameState != GPG.Multiplayer.Game.GameState.Playing)
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "StartGame", null, null, new object[0]);
                }
                if (this.mIsHost)
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "ExitFromPlaying: " + System.Enum.GetName(typeof(GPG.Multiplayer.Game.GameState), this.mGameState), this.mGameName, this.mGameName, this.mGameName } });
                }
                int num = 0;
                foreach (SupcomPlayerInfo info in this.GameInfo.Players)
                {
                    if (info.Status.ToUpper() != "PLAYING")
                    {
                        num++;
                    }
                }
                if (ConfigSettings.GetBool("IgnoreResultCount", false))
                {
                    num = 0;
                }
                if (ConfigSettings.GetBool("AlwaysKeepGathering", true))
                {
                    num = 1;
                }
                if ((ConfigSettings.GetBool("DoEndGame", false) || (this.GameName.IndexOf("AUTOMATCH") < 0)) && ((num == 0) || (this.GameName.IndexOf("AUTOMATCH") < 0)))
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "EndGame", null, null, new object[0]);
                }
                if (ConfigSettings.GetBool("OriginalLeaveGame", true))
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "LeaveGame", null, null, new object[0]);
                }
                if (this.DoReplay)
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "ReplayCompleted", new object[] { this.mReplayID } });
                }
                EventLog.WriteLine("Calling On Exit.", LogCategory.Get("SupComGameManager"), new object[0]);
                if (this.OnExit != null)
                {
                    this.OnExit(this, EventArgs.Empty);
                }
                if (!ConfigSettings.GetBool("OriginalLeaveGame", true))
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "LeaveGame", null, null, new object[0]);
                }
                if (this.mTrafficManager != null)
                {
                    this.mConnections.Clear();
                    this.mTrafficManager.Dispose();
                    this.mTrafficManager = null;
                }
            }
            catch (Exception exception4)
            {
                ErrorLog.WriteLine(exception4);
            }
        }

        private void mSupcomTCPConnection_OnGetCommand(object sender, Driver.InputEventArgs e)
        {
            EventLog.WriteLine("Got a command from the game: ", LogCategory.Get("SupComGameManager"), new object[] { e });
            if (!(e.Command == "GameOption"))
            {
                if (e.Command == "Terminate")
                {
                    string message = "";
                    foreach (string str3 in e.Args)
                    {
                        message = message + str3 + " ";
                    }
                    if (message == "")
                    {
                        message = "The game has been closed with the Terminate arguement.  Please provide a reason after the Terminate command to provide context.";
                    }
                    this.ForceCloseGame(message);
                }
                else if (e.Command == "PlayerOption")
                {
                    string[] strArray2 = ("/PLAYEROPTION " + ((string) e.Args[0])).Split(" ".ToCharArray());
                    if (strArray2.Length >= 4)
                    {
                        string playerName = strArray2[2];
                        if (playerName.IndexOf("<") != 0)
                        {
                            for (int i = 3; i < (strArray2.Length - 2); i++)
                            {
                                playerName = playerName + " " + strArray2[i];
                            }
                            string str5 = strArray2[1];
                            string str6 = strArray2[strArray2.Length - 2];
                            string str7 = strArray2[strArray2.Length - 1];
                            this.mGameInfo.PlayerByName(playerName).Army = Convert.ToInt32(str6);
                            switch (str5)
                            {
                                case "faction":
                                {
                                    string str8 = "UEF";
                                    switch (str7)
                                    {
                                        case "2":
                                            str8 = "Aeon";
                                            break;

                                        case "3":
                                            str8 = "Cybran";
                                            break;

                                        case "4":
                                            str8 = "Seraphim";
                                            break;
                                    }
                                    this.mGameInfo.PlayerByName(playerName).Faction = str8;
                                    return;
                                }
                                case "color":
                                    this.mGameInfo.PlayerByName(playerName).Color = str7;
                                    return;

                                case "team":
                                {
                                    string str9 = "FFA";
                                    switch (str7)
                                    {
                                        case "2":
                                            str9 = "Team 1";
                                            break;

                                        case "3":
                                            str9 = "Team 2";
                                            break;

                                        case "4":
                                            str9 = "Team 3";
                                            break;

                                        case "5":
                                            str9 = "Team 4";
                                            break;
                                    }
                                    this.mGameInfo.PlayerByName(playerName).Team = str9;
                                    return;
                                }
                                case "startspot":
                                    this.mGameInfo.PlayerByName(playerName).StartSpot = Convert.ToInt32(str7);
                                    return;
                            }
                        }
                    }
                }
                else if (e.Command == "Stats")
                {
                    string str10 = (string) e.Args[0];
                    str10 = str10.Replace("<LOC", "&lt;LOC").Replace("_desc>", "_desc&gt;");
                    this.mStats = str10;
                    foreach (SupcomPlayerInfo info in this.mGameInfo.Players)
                    {
                        info.UnitInfo.Clear();
                    }
                    try
                    {
                        string str11 = "";
                        foreach (string str12 in str10.Split(new char[] { '\n' }))
                        {
                            if (str12.ToUpper().IndexOf("<ARMY") >= 0)
                            {
                                string[] strArray4 = str12.Split(new char[] { '"' });
                                if (strArray4.Length >= 3)
                                {
                                    int num2 = Convert.ToInt32(strArray4[1]);
                                    string str13 = strArray4[3];
                                    str11 = str13;
                                    foreach (SupcomPlayerInfo info2 in this.mGameInfo.Players)
                                    {
                                        if (info2.PlayerName == str13)
                                        {
                                            info2.Army = num2;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if ((str12.ToUpper().IndexOf("<ENERGY PRODUCED=\"") >= 0) && (str11 != ""))
                            {
                                double num3 = 0.0;
                                double num4 = 0.0;
                                foreach (string str14 in str12.Split(new char[] { ' ' }))
                                {
                                    try
                                    {
                                        string[] strArray5 = str14.Split("=".ToCharArray(), 2);
                                        if (strArray5.Length == 2)
                                        {
                                            try
                                            {
                                                if (strArray5[0].ToLower() == "produced")
                                                {
                                                    num4 += Convert.ToDouble(strArray5[1].Replace("\"", ""));
                                                }
                                                else if (strArray5[0].ToLower() == "consumed")
                                                {
                                                    num3 += Convert.ToDouble(strArray5[1].Replace("\"", ""));
                                                }
                                            }
                                            catch (Exception exception)
                                            {
                                                ErrorLog.WriteLine(exception);
                                            }
                                        }
                                    }
                                    catch (Exception exception2)
                                    {
                                        ErrorLog.WriteLine(exception2);
                                    }
                                }
                                foreach (SupcomPlayerInfo info3 in this.mGameInfo.Players)
                                {
                                    if (info3.PlayerName == str11)
                                    {
                                        info3.EnergyConsumed = num3;
                                        info3.EnergyProduced = num4;
                                        break;
                                    }
                                }
                            }
                            else if ((str12.ToUpper().IndexOf("<MASS PRODUCED=\"") >= 0) && (str11 != ""))
                            {
                                double num5 = 0.0;
                                double num6 = 0.0;
                                foreach (string str15 in str12.Split(new char[] { ' ' }))
                                {
                                    try
                                    {
                                        string[] strArray6 = str15.Split("=".ToCharArray(), 2);
                                        if (strArray6.Length == 2)
                                        {
                                            try
                                            {
                                                if (strArray6[0].ToLower() == "produced")
                                                {
                                                    num6 += Convert.ToDouble(strArray6[1].Replace("\"", ""));
                                                }
                                                else if (strArray6[0].ToLower() == "consumed")
                                                {
                                                    num5 += Convert.ToDouble(strArray6[1].Replace("\"", ""));
                                                }
                                            }
                                            catch (Exception exception3)
                                            {
                                                ErrorLog.WriteLine(exception3);
                                            }
                                        }
                                    }
                                    catch (Exception exception4)
                                    {
                                        ErrorLog.WriteLine(exception4);
                                    }
                                }
                                foreach (SupcomPlayerInfo info4 in this.mGameInfo.Players)
                                {
                                    if (info4.PlayerName == str11)
                                    {
                                        info4.MassConsumed = num5;
                                        info4.MassProduced = num6;
                                        break;
                                    }
                                }
                            }
                            else if ((str12.ToUpper().IndexOf("<UNIT ID=\"") >= 0) && (str11 != ""))
                            {
                                string str16 = "";
                                int num7 = 0;
                                int num8 = 0;
                                int num9 = 0;
                                double num10 = 0.0;
                                double num11 = 0.0;
                                foreach (string str17 in str12.Split(new char[] { ' ' }))
                                {
                                    try
                                    {
                                        string[] strArray7 = str17.Split("=".ToCharArray(), 2);
                                        if (strArray7.Length == 2)
                                        {
                                            try
                                            {
                                                if (strArray7[0].ToLower() == "id")
                                                {
                                                    str16 = strArray7[1].Replace("\"", "");
                                                }
                                                else if (strArray7[0].ToLower() == "built")
                                                {
                                                    num7 += Convert.ToInt32(strArray7[1].Replace("\"", ""));
                                                }
                                                else if (strArray7[0].ToLower() == "lost")
                                                {
                                                    num8 += Convert.ToInt32(strArray7[1].Replace("\"", ""));
                                                }
                                                else if (strArray7[0].ToLower() == "killed")
                                                {
                                                    num9 += Convert.ToInt32(strArray7[1].Replace("\"", ""));
                                                }
                                                else if (strArray7[0].ToLower() == "damagedealt")
                                                {
                                                    num10 += Convert.ToDouble(strArray7[1].Replace("\"", ""));
                                                }
                                                else if (strArray7[0].ToLower() == "damagereceived")
                                                {
                                                    num11 += Convert.ToDouble(strArray7[1].Replace("\"", ""));
                                                }
                                            }
                                            catch (Exception exception5)
                                            {
                                                ErrorLog.WriteLine(exception5);
                                            }
                                        }
                                    }
                                    catch (Exception exception6)
                                    {
                                        ErrorLog.WriteLine(exception6);
                                    }
                                }
                                num7 += num8;
                                foreach (SupcomPlayerInfo info5 in this.mGameInfo.Players)
                                {
                                    if (info5.PlayerName == str11)
                                    {
                                        SupcomUnitInfo item = new SupcomUnitInfo {
                                            unitid = str16,
                                            built = num7,
                                            lost = num8,
                                            killed = num9,
                                            damagedealt = num10,
                                            damagereceived = num11
                                        };
                                        info5.UnitInfo.Add(item);
                                    }
                                }
                                if ((str12.ToUpper().IndexOf("<UNIT ID=\"UEL0001\"") >= 0) && (str11 != ""))
                                {
                                    if ((str12.ToUpper().IndexOf("BUILT=\"1\"") > 0) || (str12.ToUpper().IndexOf("LOST=\"1\"") > 0))
                                    {
                                        foreach (SupcomPlayerInfo info7 in this.mGameInfo.Players)
                                        {
                                            if (info7.PlayerName == str11)
                                            {
                                                info7.Faction = "UEF";
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if ((str12.ToUpper().IndexOf("<UNIT ID=\"UAL0001\"") >= 0) && (str11 != ""))
                                {
                                    if ((str12.ToUpper().IndexOf("BUILT=\"1\"") > 0) || (str12.ToUpper().IndexOf("LOST=\"1\"") > 0))
                                    {
                                        foreach (SupcomPlayerInfo info8 in this.mGameInfo.Players)
                                        {
                                            if (info8.PlayerName == str11)
                                            {
                                                info8.Faction = "Aeon";
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if ((str12.ToUpper().IndexOf("<UNIT ID=\"URL0001\"") >= 0) && (str11 != ""))
                                {
                                    if ((str12.ToUpper().IndexOf("BUILT=\"1\"") > 0) || (str12.ToUpper().IndexOf("LOST=\"1\"") > 0))
                                    {
                                        foreach (SupcomPlayerInfo info9 in this.mGameInfo.Players)
                                        {
                                            if (info9.PlayerName == str11)
                                            {
                                                info9.Faction = "Cybran";
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (((str12.ToUpper().IndexOf("<UNIT ID=\"XSL0001\"") >= 0) && (str11 != "")) && ((str12.ToUpper().IndexOf("BUILT=\"1\"") > 0) || (str12.ToUpper().IndexOf("LOST=\"1\"") > 0)))
                                {
                                    foreach (SupcomPlayerInfo info10 in this.mGameInfo.Players)
                                    {
                                        if (info10.PlayerName == str11)
                                        {
                                            info10.Faction = "Seraphim";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        this.ProcessGameResults();
                    }
                    catch (Exception exception7)
                    {
                        ErrorLog.WriteLine(exception7);
                    }
                    if (!this.IsRegularRankedGame())
                    {
                        TimeSpan span = (TimeSpan) (DateTime.Now - this.mGameInfo.StartTime);
                        string oldValue = "<GameStats xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";
                        string newValue = oldValue + "\r\n  <GameInfo map=\"" + this.mGameInfo.Map + "\" starttime=\"" + this.mGameInfo.StartTime.ToUniversalTime().ToString() + "\" duration=\"" + span.TotalSeconds.ToString() + "\" gametype=\"Custom\">\r\n    <PlayerInformation>\r\n";
                        if (this.mGameInfo != null)
                        {
                            foreach (SupcomPlayerInfo info11 in this.mGameInfo.Players)
                            {
                                newValue = newValue + "      <Player name=\"" + info11.PlayerName + "\" faction=\"" + info11.Faction + "\" team=\"" + info11.Team + "\" status=\"" + info11.Status + "\" startposition=\"" + info11.StartSpot.ToString() + "\" color=\"" + info11.Color + "\"/>\r\n";
                            }
                        }
                        newValue = newValue + "    </PlayerInformation>\r\n" + "  </GameInfo>\r\n";
                        this.mStats = this.mStats.Replace(oldValue, newValue);
                    }
                    else
                    {
                        foreach (SupcomPlayerInfo info12 in this.mGameInfo.Players)
                        {
                            foreach (string str20 in this.mStats.Split("\r\n".ToCharArray()))
                            {
                                if ((str20.IndexOf("\"" + info12.PlayerName + "\"") >= 0) && (str20.IndexOf("victory") >= 0))
                                {
                                    info12.Status = "victory";
                                }
                                if ((str20.IndexOf("\"" + info12.PlayerName + "\"") >= 0) && (str20.IndexOf("defeat") >= 0))
                                {
                                    info12.Status = "defeat";
                                }
                                if (str20.IndexOf("GameInfo") >= 0)
                                {
                                    foreach (string str21 in str20.Split(new char[] { ' ' }))
                                    {
                                        if (str21.IndexOf("starttime") >= 0)
                                        {
                                            try
                                            {
                                                this.mGameInfo.StartTime = DateTime.Parse(str21.Replace("starttime", "").Replace("\"", "").Trim());
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.OnStatsXML != null)
                    {
                        this.OnStatsXML(this.mStats);
                    }
                }
                else if (e.Command == "Desync")
                {
                    this.ForceCloseGame(Loc.Get("<LOC>The game has desynced and has been shut down."));
                }
                else
                {
                    if (e.Command == "Bottleneck")
                    {
                        if (ConfigSettings.GetBool("AutoDisconnect", true))
                        {
                            try
                            {
                                if (Convert.ToDouble(e.Args[3]) > ConfigSettings.GetInt("AutoTime", 0x7530))
                                {
                                    this.MessageGame("//DISCPLAYER " + e.Args[2].ToString());
                                }
                            }
                            catch (Exception exception8)
                            {
                                ErrorLog.WriteLine(exception8);
                            }
                        }
                        if (!ConfigSettings.GetBool("LogBottlenecks", true))
                        {
                            return;
                        }
                        try
                        {
                            string str22 = "";
                            foreach (object obj2 in e.Args)
                            {
                                str22 = str22 + obj2.ToString() + " ";
                            }
                            double num13 = Convert.ToDouble(e.Args[3]);
                            if ((Environment.TickCount - this.mBottleNeckCooldown) > ConfigSettings.GetInt("BottleNeckCooldown", 0x7530))
                            {
                                if (num13 > ConfigSettings.GetInt("AutoTime", 0x7530))
                                {
                                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "Bottleneck", str22, this.GameName, this.GameName } });
                                }
                                this.mBottleNeckCooldown = Environment.TickCount;
                            }
                            return;
                        }
                        catch (Exception exception9)
                        {
                            ErrorLog.WriteLine(exception9);
                            return;
                        }
                    }
                    if (e.Command == "GameState")
                    {
                        string str23 = (string) e.Args[0];
                        if (str23 != "Idle")
                        {
                            if (((str23 == "Lobby") && (this.mGameState != GPG.Multiplayer.Game.GameState.Lobby)) && (this.mGameState != GPG.Multiplayer.Game.GameState.Launching))
                            {
                                if (this.MyTeam != "")
                                {
                                    this.MessageGame("//TEAMNAME " + this.MyTeam);
                                }
                                if (Chatroom.InChatroom)
                                {
                                    Chatroom.JoinGame();
                                }
                                this.mGameState = GPG.Multiplayer.Game.GameState.Lobby;
                                if (this.mIsHost)
                                {
                                    ThreadQueue.Quazal.Enqueue(typeof(Game), "CreateGame", this, "FinishCreateGame", new object[] { this.mGameName, "Unknown Map", 0x5dc, 8, "SupCom", "1.0", this.GetParams() });
                                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "HostedGame", this.mGameName, this.mGameName, this.mGameName } });
                                }
                                else
                                {
                                    ThreadQueue.Quazal.Enqueue(typeof(Game), "JoinGame", this, "FinishJoinGame", new object[] { this.mGameName });
                                }
                            }
                            else if ((str23 == "Running") && (this.mGameState != GPG.Multiplayer.Game.GameState.Playing))
                            {
                                this.mGameState = GPG.Multiplayer.Game.GameState.Playing;
                                this.MessageGame("//PLAYING " + User.Current.Name);
                                this.mGameInfo.StartTime = DateTime.Now;
                            }
                            else if ((str23 == "Launching") && (this.mGameState != GPG.Multiplayer.Game.GameState.Launching))
                            {
                                ThreadQueue.Quazal.Enqueue(typeof(Game), "StartGame", null, null, new object[0]);
                                this.mGameState = GPG.Multiplayer.Game.GameState.Launching;
                                if (this.OnGameLaunched != null)
                                {
                                    this.OnGameLaunched(this, EventArgs.Empty);
                                }
                                this.MessageGame("//PLAYERINFO " + User.Current.ID.ToString() + " " + User.Current.Name + " -1");
                                if (this.mIsHost)
                                {
                                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "LaunchedGame: " + this.mGameInfo.Map, this.mGameName, this.mGameName, this.mGameName } });
                                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "Map", this.mGameInfo.Map, this.mGameName, this.mGameName } });
                                    this.MessageGame("//MAP " + this.mGameInfo.Map);
                                    if (this.MyTeam != "")
                                    {
                                        this.MessageGame("//TEAMNAME " + this.MyTeam);
                                    }
                                    foreach (SupcomPlayerInfo info13 in this.mGameInfo.Players)
                                    {
                                        this.MessageGame("//PLAYERINFO " + info13.PlayerID.ToString() + " " + info13.PlayerName + " " + info13.Army.ToString());
                                    }
                                }
                                else if (ConfigSettings.GetBool("LogJoins", true))
                                {
                                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "JoinedLaunchedGame: " + this.mGameInfo.Map, this.mGameName, this.mGameName, this.mGameName } });
                                }
                                this.MessageGame("//LAUNCHING " + User.Current.Name);
                                if (this.DoReplay)
                                {
                                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "ReplayLaunched", new object[] { this.mReplayID } });
                                }
                            }
                        }
                    }
                    else if (e.Command == "GameResult")
                    {
                        this.mResultsQueue.Add(e);
                    }
                }
            }
            else if (((string) e.Args[0]) == "ScenarioFile")
            {
                string[] strArray = ((string) e.Args[1]).Split(new char[] { '/' });
                this.mGameInfo.Map = (string) e.Args[1];
                foreach (string str in strArray)
                {
                    if (str.ToUpper().IndexOf("SCMP") == 0)
                    {
                        this.mGameInfo.Map = str.ToUpper();
                        break;
                    }
                }
                ThreadQueue.Quazal.Enqueue(typeof(Game), "UpdateGame", null, null, new object[] { this.mGameInfo.Map, 0, this.mGameInfo.GetMaxPlayers(), "", "", "" });
            }
        }

        private void mTrafficManager_OnCommandMessage(MessageEventArgs e)
        {
            VGen0 gen = null;
            VGen0 gen2 = null;
            EventLog.WriteLine("A TrafficManager Message Command was received: " + e.Command, LogCategory.Get("Command"), e.CommandArgs);
            foreach (string str in e.CommandArgs)
            {
                EventLog.WriteLine("Arguement: " + str, LogCategory.Get("Command"), new object[0]);
            }
            string command = e.Command;
            string[] commandArgs = e.CommandArgs;
            if (this.mSupcomTCPConnection != null)
            {
                switch (command.ToLower())
                {
                    case "attemptjoin":
                        this.mSupcomTCPConnection.SendMessage("Chat", new object[] { commandArgs[0] + Loc.Get("<LOC> is attempting to join the game.") });
                        if (this.mIsHost && ((this.GameState == GPG.Multiplayer.Game.GameState.Launching) || (this.GameState == GPG.Multiplayer.Game.GameState.Playing)))
                        {
                            this.MessageGame("//LAUNCHING " + User.Current.Name.ToString());
                        }
                        return;

                    case "notifyexit":
                        if (((this.mHostName.ToUpper() == commandArgs[1].ToUpper()) && (this.GameState != GPG.Multiplayer.Game.GameState.Launching)) && (this.GameState != GPG.Multiplayer.Game.GameState.Playing))
                        {
                            this.ForceCloseGame(Loc.Get("<LOC>The host has left the game."));
                        }
                        this.mSupcomTCPConnection.SendMessage("Chat", new object[] { commandArgs[1] + Loc.Get("<LOC> has left the game.") });
                        return;

                    case "launching":
                        if (gen == null)
                        {
                            gen = delegate {
                                Thread.Sleep(ConfigSettings.GetInt("Game Abort Timeout", 0x3a98));
                                if ((this.GameState != GPG.Multiplayer.Game.GameState.Launching) && (this.GameState != GPG.Multiplayer.Game.GameState.Playing))
                                {
                                    this.ForceCloseGame(Loc.Get("<LOC>The game has been launched and you can no longer join."));
                                }
                            };
                        }
                        new Thread(new ThreadStart(gen.Invoke)) { IsBackground = true }.Start();
                        return;

                    case "autoabort":
                        if (commandArgs[0].ToUpper() != User.Current.Name)
                        {
                            this.ForceCloseGame(Loc.Get("<LOC>Your opponent has aborted the automatch."));
                        }
                        return;

                    case "playing":
                        if (gen2 == null)
                        {
                            gen2 = delegate {
                                Thread.Sleep(0xbb8);
                                if ((this.GameState != GPG.Multiplayer.Game.GameState.Launching) && (this.GameState != GPG.Multiplayer.Game.GameState.Playing))
                                {
                                    this.ForceCloseGame(Loc.Get("<LOC>The game has been started and you can no longer join."));
                                }
                            };
                        }
                        new Thread(new ThreadStart(gen2.Invoke)) { IsBackground = true }.Start();
                        return;

                    case "teamname":
                    {
                        string str3 = "";
                        foreach (string str4 in commandArgs)
                        {
                            str3 = str3 + str4 + " ";
                        }
                        if (str3.IndexOf(User.Current.ID.ToString() + " ") >= 0)
                        {
                            if ((this.MyTeam == "") || (this.MyTeam.Split(new char[] { ' ' }).Length < str3.Trim().Split(new char[] { ' ' }).Length))
                            {
                                this.MyTeam = str3.Trim();
                                return;
                            }
                        }
                        else if ((this.OtherTeam == "") || (this.OtherTeam.Split(new char[] { ' ' }).Length < str3.Trim().Split(new char[] { ' ' }).Length))
                        {
                            this.OtherTeam = str3.Trim();
                            return;
                        }
                        return;
                    }
                    case "map":
                        try
                        {
                            if (((this.mMapName == "") || (this.mGameInfo.Map == "")) || ((this.mGameInfo.Map == null) || (this.mMapName == null)))
                            {
                                this.mGameInfo.Map = commandArgs[0];
                                this.mMapName = commandArgs[0];
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                        return;

                    case "playerinfo":
                        try
                        {
                            if (this.mGameInfo != null)
                            {
                                SupcomPlayerInfo info = this.mGameInfo.PlayerByName(commandArgs[1]);
                                int num = Convert.ToInt32(commandArgs[0]);
                                if ((num > 0x3e8) || (info.PlayerID <= 0))
                                {
                                    info.PlayerID = num;
                                }
                                if (commandArgs[2] != "-1")
                                {
                                    info.Army = Convert.ToInt32(commandArgs[2]);
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            ErrorLog.WriteLine(exception2);
                        }
                        return;

                    case "discplayer":
                        this.EjectFromGame(Convert.ToInt32(commandArgs[0]));
                        return;

                    default:
                        return;
                }
            }
        }

        private void mTrafficManager_OnNewPlayer(PlayerInformation info)
        {
            EventLog.WriteLine("Got a new player: " + info.EndPoint.Address.ToString() + " " + info.PlayerName, LogCategory.Get("SupComGameManager"), new object[0]);
            if (info.PlayerID != -1)
            {
                if (!this.mIsJoined)
                {
                    this.mConnections[info.PlayerID] = info;
                    this.EstablishConnections();
                }
                else if (this.mGameState == GPG.Multiplayer.Game.GameState.Lobby)
                {
                    this.ConnectPeer(info.EndPoint.Address.ToString(), info.EndPoint.Port, info.PlayerName, info.PlayerID);
                }
                this.mConnections[info.PlayerID] = info;
            }
        }

        private void mTrafficManager_OnRemovePlayer(int connectionid)
        {
            if (this.mGameState == GPG.Multiplayer.Game.GameState.Lobby)
            {
                this.DisconnectPeer(connectionid);
            }
            this.mConnections[connectionid] = PlayerInformation.Empty();
        }

        private void NoResult(bool result)
        {
        }

        private void OnCheckConnection(object result)
        {
            this.EstablishConnections();
        }

        public int OnExitCount()
        {
            if (this.OnExit == null)
            {
                return 0;
            }
            return this.OnExit.GetInvocationList().Length;
        }

        private void ProcessGameResults()
        {
            foreach (Driver.InputEventArgs args in this.mResultsQueue)
            {
                this.mStatsWatcher.Stop();
                this.mGameInfo.Sort();
                try
                {
                    this.mGameInfo.Players[((int) args.Args[0]) - 1].Status = (string) args.Args[1];
                    int num = 0;
                    foreach (SupcomPlayerInfo info in this.mGameInfo.Players)
                    {
                        if (info.Status.ToUpper() == "DEFEAT")
                        {
                            num++;
                        }
                    }
                    if (ConfigSettings.GetBool("ForceCheckVictory", false) && ((num + 1) == this.mGameInfo.Players.Count))
                    {
                        foreach (SupcomPlayerInfo info2 in this.mGameInfo.Players)
                        {
                            if (info2.Status.ToUpper() != "DEFEAT")
                            {
                                info2.Status = "victory";
                            }
                        }
                    }
                    foreach (SupcomPlayerInfo info3 in this.mGameInfo.Players)
                    {
                        if (info3.PlayerName.ToUpper() == User.Current.Name.ToUpper())
                        {
                            this.mStatsWatcher.Stop();
                        }
                    }
                    this.GetStats();
                    foreach (SupcomPlayerInfo info4 in this.mGameInfo.Players)
                    {
                        this.MessageGame("//PLAYERINFO " + info4.PlayerID.ToString() + " " + info4.PlayerName + " " + info4.Army.ToString());
                    }
                    if (ConfigSettings.GetBool("RecordCustomGames", true) && (this.GameName.ToUpper().IndexOf("AUTOMATCH") < 0))
                    {
                        foreach (SupcomPlayerInfo info5 in this.mGameInfo.Players)
                        {
                            if (info5.PlayerName.ToUpper() == User.Current.Name.ToUpper())
                            {
                                TimeSpan span = (TimeSpan) (DateTime.Now - this.mGameInfo.StartTime);
                                if (ConfigSettings.GetBool("DoOldGameList", false))
                                {
                                    DataAccess.QueueExecuteQuery("Set Custom Game Result", new object[] { info5.Faction, this.mGameInfo.Map, info5.Status, span.TotalSeconds });
                                    DataAccess.QueueExecuteQuery("Update Custom Game Record", new object[0]);
                                }
                                else
                                {
                                    DataAccess.QueueExecuteQuery("Set Custom Game Result2", new object[] { info5.Faction, this.mGameInfo.Map, info5.Status, span.TotalSeconds, GPGnetSelectedGame.SelectedGame.GameID });
                                    DataAccess.QueueExecuteQuery("Update Custom Game Record2", new object[0]);
                                }
                            }
                        }
                    }
                    continue;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return;
                }
            }
            this.mResultsQueue.Clear();
        }

        public void ReceiveMessage(NetMessage message)
        {
            EventLog.WriteLine("Game Message: " + message.EndPoint.Address.ToString() + " " + message.Text, LogCategory.Get("SupComGameManager"), new object[0]);
            if (message.Text.IndexOf("/GAMEOPTION") == 0)
            {
                string[] strArray = message.Text.Split(" ".ToCharArray());
                if (strArray.Length >= 4)
                {
                    string playerName = strArray[2];
                    if (playerName.IndexOf("<") != 0)
                    {
                        for (int i = 3; i < (strArray.Length - 2); i++)
                        {
                            playerName = playerName + " " + strArray[i];
                        }
                        string str2 = strArray[1];
                        string str3 = strArray[strArray.Length - 2];
                        string str4 = strArray[strArray.Length - 1];
                        this.mGameInfo.PlayerByName(playerName).Army = Convert.ToInt32(str3);
                        switch (str2)
                        {
                            case "faction":
                            {
                                string str5 = "UEF";
                                switch (str4)
                                {
                                    case "2":
                                        str5 = "Aeon";
                                        break;

                                    case "3":
                                        str5 = "Cybran";
                                        break;
                                }
                                this.mGameInfo.PlayerByName(playerName).Faction = str5;
                                return;
                            }
                            case "color":
                                this.mGameInfo.PlayerByName(playerName).Color = str4;
                                return;

                            case "team":
                            {
                                string str6 = "FFA";
                                switch (str4)
                                {
                                    case "2":
                                        str6 = "Team 1";
                                        break;

                                    case "3":
                                        str6 = "Team 2";
                                        break;

                                    case "4":
                                        str6 = "Team 3";
                                        break;

                                    case "5":
                                        str6 = "Team 4";
                                        break;
                                }
                                this.mGameInfo.PlayerByName(playerName).Team = str6;
                                return;
                            }
                            case "startspot":
                                this.mGameInfo.PlayerByName(playerName).StartSpot = Convert.ToInt32(str4);
                                return;
                        }
                    }
                }
            }
            else if (message.Text.IndexOf("/MAP") == 0)
            {
                string[] strArray2 = message.Text.Split(new char[] { '/' });
                this.mGameInfo.Map = strArray2[strArray2.Length - 1].Replace(".scmap", "");
                ThreadQueue.Quazal.Enqueue(typeof(Game), "UpdateGame", null, null, new object[] { this.mGameInfo.Map, 0, this.mGameInfo.GetMaxPlayers(), "", "", "" });
            }
            else if (message.Text.IndexOf("/STATS") == 0)
            {
                if (message.Text != "/STATS End Stats")
                {
                    if (message.Text == "/STATS Begin Stats")
                    {
                        this.mStats = "";
                    }
                    else
                    {
                        string str9 = message.Text.Replace("/STATS ", "") + "\r\n";
                        while (str9.IndexOf("<LOC") >= 0)
                        {
                            int index = str9.IndexOf("<LOC");
                            str9 = str9.Remove(index, 4);
                            if (str9.IndexOf(">") > 0)
                            {
                                str9 = str9.Remove(str9.IndexOf(">"), 1);
                            }
                        }
                        this.mStats = this.mStats + str9;
                    }
                }
                else
                {
                    TimeSpan span = (TimeSpan) (DateTime.Now - this.mGameInfo.StartTime);
                    string oldValue = "<GameStats xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";
                    string newValue = oldValue + "\r\n  <GameInfo map=\"" + this.mGameInfo.Map + "\" starttime=\"" + this.mGameInfo.StartTime.ToUniversalTime().ToString() + "\" duration=\"" + span.TotalSeconds.ToString() + "\" gametype=\"Custom\">\r\n    <PlayerInformation>\r\n";
                    foreach (SupcomPlayerInfo info in this.mGameInfo.Players)
                    {
                        newValue = newValue + "      <Player name=\"" + info.PlayerName + "\" faction=\"" + info.Faction + "\" team=\"" + info.Team + "\" status=\"" + info.Status + "\" startposition=\"" + info.StartSpot.ToString() + "\" color=\"" + info.Color + "\"/>\r\n";
                    }
                    newValue = newValue + "    </PlayerInformation>\r\n" + "  </GameInfo>\r\n";
                    this.mStats = this.mStats.Replace(oldValue, newValue);
                    if (this.OnStatsXML != null)
                    {
                        this.OnStatsXML(this.mStats);
                    }
                    if (this.mStatsShutdown)
                    {
                        this.EndGame();
                    }
                }
            }
            else if (((message.Text.ToUpper() == "/GAMESTATE LOBBY") && (this.mGameState != GPG.Multiplayer.Game.GameState.Lobby)) && (this.mGameState != GPG.Multiplayer.Game.GameState.Launching))
            {
                if (Chatroom.InChatroom)
                {
                    Chatroom.JoinGame();
                }
                this.mGameState = GPG.Multiplayer.Game.GameState.Lobby;
                this.mSupcomTCPConnection.SendMessage("LUA", new object[] { "LOG('This is a test.')" });
                if (this.mIsHost)
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "CreateGame", this, "FinishCreateGame", new object[] { this.mGameName, "Unknown Map", 0x5dc, 8, "SupCom", "1.0", this.GetParams() });
                }
                else
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "JoinGame", this, "FinishJoinGame", new object[] { this.mGameName });
                }
            }
            else if ((message.Text.ToUpper() == "/GAMESTATE GAME IS RUNNING") && (this.mGameState != GPG.Multiplayer.Game.GameState.Playing))
            {
                this.mGameState = GPG.Multiplayer.Game.GameState.Playing;
                this.MessageGame("//PLAYING " + User.Current.Name);
                this.mGameInfo.StartTime = DateTime.Now;
            }
            else if ((message.Text.ToUpper() == "/GAMESTATE LAUNCHING") && (this.mGameState != GPG.Multiplayer.Game.GameState.Launching))
            {
                ThreadQueue.Quazal.Enqueue(typeof(Game), "StartGame", null, null, new object[0]);
                this.mGameState = GPG.Multiplayer.Game.GameState.Launching;
                this.MessageGame("//LAUNCHING " + User.Current.Name);
            }
            else if (message.Text.ToUpper().IndexOf("/GAMERESULT") == 0)
            {
                this.mStatsWatcher.Stop();
                string[] strArray3 = message.Text.Split(" ".ToCharArray(), 3);
                if (strArray3.Length == 3)
                {
                    this.mGameInfo.Sort();
                    this.mGameInfo.Players[Convert.ToInt32(strArray3[1]) - 1].Status = strArray3[2];
                }
                int num3 = 0;
                foreach (SupcomPlayerInfo info2 in this.mGameInfo.Players)
                {
                    if (info2.Status.ToUpper() == "DEFEAT")
                    {
                        num3++;
                    }
                }
                if ((num3 + 1) == this.mGameInfo.Players.Count)
                {
                    foreach (SupcomPlayerInfo info3 in this.mGameInfo.Players)
                    {
                        if (info3.Status.ToUpper() != "DEFEAT")
                        {
                            info3.Status = "victory";
                        }
                    }
                }
                foreach (SupcomPlayerInfo info4 in this.mGameInfo.Players)
                {
                    if (info4.PlayerName.ToUpper() == User.Current.Name.ToUpper())
                    {
                        this.mStatsWatcher.Stop();
                    }
                }
                this.GetStats();
            }
            else if (message.Text.ToUpper() == "/SENDSHUTDOWN")
            {
                this.EndGame();
            }
        }

        private void RegisterPrivateURL()
        {
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (((address.ToString() != "127.0.0.1") && (address.ToString().IndexOf(".") >= 0)) && (address.ToString().IndexOf(":") < 0))
                {
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "AddPrivateURL", this, "NoResult", new object[] { address.ToString(), Convert.ToInt32(this.Port) });
                    return;
                }
            }
        }

        public void SetAbort()
        {
            this.mGameState = GPG.Multiplayer.Game.GameState.Aborted;
        }

        public void SetPassword(string password)
        {
            this.mPassword = password;
        }

        private void SetUpAddressPoll()
        {
            if (ConfigSettings.GetBool("BroadcastJoin", true))
            {
                new Thread(new ThreadStart(this.CheckGameGatheringConnections)) { IsBackground = true }.Start();
            }
        }

        public bool WatchReplay(string commandargs, string url)
        {
            string arguements = GameArgs + " " + commandargs + " /replay " + url;
            this.mSupcomTCPConnection = new SupcomTCPConnection(false, this.GetGameLocation(), arguements, "localhost", 0, true, true);
            this.mSupcomTCPConnection.OnGetCommand += new EventHandler<Driver.InputEventArgs>(this.mSupcomTCPConnection_OnGetCommand);
            this.mSupcomTCPConnection.Bind(this.Port, User.Current.ID);
            this.mTrafficManager.SetConnection(this.mSupcomTCPConnection);
            return true;
        }

        public bool DoReplay
        {
            get
            {
                return ((GPGnetSelectedGame.SelectedGame.GameID == 0x11) && sLiveReplayMode);
            }
        }

        public SupcomGameInfo GameInfo
        {
            get
            {
                return this.mGameInfo;
            }
        }

        public string GameName
        {
            get
            {
                return this.mGameName;
            }
        }

        public GPG.Multiplayer.Game.GameState GameState
        {
            get
            {
                return this.mGameState;
            }
        }

        private string GPGNetURL
        {
            get
            {
                return ConfigSettings.GetString("ReplayURL", "GPGNET://replay.gaspowered.com/");
            }
        }

        public string LastXML
        {
            get
            {
                if (this.mStatsWatcher != null)
                {
                    return this.mStatsWatcher.Xml;
                }
                return "";
            }
        }

        public string mLaunchMap
        {
            get
            {
                string map = "";
                if (this.mGameInfo != null)
                {
                    map = this.mGameInfo.Map;
                }
                else
                {
                    map = this.mMapName;
                }
                if ((map != "") && (map != null))
                {
                    return map;
                }
                return ConfigSettings.GetString("DefaultLobbyMap", "SCMP_007");
            }
            set
            {
                if (this.mGameInfo != null)
                {
                    this.mGameInfo.Map = value;
                }
                this.mMapName = value;
            }
        }

        public string MyTeam
        {
            get
            {
                try
                {
                    if ((this.mMyTeam == "") || (this.mMyTeam == null))
                    {
                        return "";
                    }
                    List<int> list = new List<int>();
                    foreach (string str in this.mMyTeam.Trim().Split(new char[] { ' ' }))
                    {
                        list.Add(Convert.ToInt32(str));
                    }
                    list.Sort();
                    string str2 = "";
                    foreach (int num in list)
                    {
                        str2 = str2 + num.ToString() + " ";
                    }
                    return str2.Trim();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return this.mMyTeam;
                }
            }
            set
            {
                if (value != "")
                {
                    this.mMyTeam = value;
                }
            }
        }

        public string OtherTeam
        {
            get
            {
                try
                {
                    if ((this.mOtherTeam == "") || (this.mOtherTeam == null))
                    {
                        return "";
                    }
                    List<int> list = new List<int>();
                    foreach (string str in this.mOtherTeam.Trim().Split(new char[] { ' ' }))
                    {
                        list.Add(Convert.ToInt32(str));
                    }
                    list.Sort();
                    string str2 = "";
                    foreach (int num in list)
                    {
                        str2 = str2 + num.ToString() + " ";
                    }
                    return str2.Trim();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return this.mOtherTeam;
                }
            }
            set
            {
                if (value != "")
                {
                    this.mOtherTeam = value;
                }
            }
        }

        private int Port
        {
            get
            {
                return SupcomStdInOut.GamePort;
            }
        }
    }
}

