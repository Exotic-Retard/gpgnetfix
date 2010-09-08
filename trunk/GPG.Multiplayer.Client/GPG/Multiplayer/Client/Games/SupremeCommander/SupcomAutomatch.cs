namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Games.SupremeCommander.tournaments;
    using GPG.Multiplayer.Game;
    using GPG.Multiplayer.Game.SupremeCommander;
    using GPG.Multiplayer.LadderService;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class SupcomAutomatch : IDisposable
    {
        private LaunchGameDelegate CallLaunchGame;
        private bool IsLadderAutomatch = false;
        public bool IsTeamLeader = false;
        private LadderInstance Ladder = null;
        private List<string> mAllies = null;
        public string MapName = "";
        private string mFaction = "/uef";
        private Dictionary<int, string> mFFAPlayers = new Dictionary<int, string>();
        private string mHost = "";
        private string mKind = "1v1";
        private Hashtable mNotify = Hashtable.Synchronized(new Hashtable());
        private string mOpponentTeamName = "";
        private int mPollCount = 0;
        private bool mPolling = false;
        private int mRating = 0x640;
        private int mRatingThreshhold = 0;
        private SupcomAutoState mState = SupcomAutoState.Unavailable;
        private SupComGameManager mSupcomGameManager = null;
        private int mTeam = 0;
        private string mTeamArg = "";
        private string mTeamName = "";
        private static SupcomAutomatch sSupcomAutomatch = null;
        public static TeamGame sTeamGame = null;

        public event AutomatchStatsDelegate OnAutomatchStats;

        public event EventHandler OnExit;

        public event EventHandler OnLaunchGame;

        public event StringEventHandler2 OnStatusChanged;

        private SupcomAutomatch()
        {
        }

        internal void AutomatchAbortChallenge(int senderID, string senderName)
        {
            EventLog.WriteLine("A challenge has been aborted by " + senderName, LogCategory.Get("Automatch"), new object[0]);
            this.NotifyStatus(Loc.Get("<LOC>Game aborted by host.  Resuming search."));
            this.mState = SupcomAutoState.Searching;
        }

        public void AutomatchAccept(int playerID, string name)
        {
            WaitCallback callBack = null;
            this.NotifyStatus(Loc.Get("<LOC>Automatch Accept with player "));
            EventLog.WriteLine("AutomatchAccept Enter: " + name, LogCategory.Get("Automatch"), new object[0]);
            if (this.mKind == "FFA")
            {
                if (this.mState == SupcomAutoState.Searching)
                {
                    this.mFFAPlayers.Clear();
                    this.mState = SupcomAutoState.FFASetup;
                    this.CallLaunchGame = new LaunchGameDelegate(this.HostTheGame);
                    if (callBack == null)
                    {
                        callBack = delegate (object o) {
                            Thread.Sleep(ConfigSettings.GetInt("FFAWaitTime", 0x2710));
                            if (this.mFFAPlayers.Count >= ConfigSettings.GetInt("MinFFASize", 4))
                            {
                                this.CallLaunchGame(0, "");
                            }
                            else
                            {
                                foreach (KeyValuePair<int, string> pair in this.mFFAPlayers)
                                {
                                    Messaging.SendCustomCommand(pair.Value, CustomCommands.AutomatchAbortChallenge, new object[0]);
                                }
                            }
                        };
                    }
                    ThreadPool.QueueUserWorkItem(callBack);
                }
                if (this.mState == SupcomAutoState.FFASetup)
                {
                    EventLog.WriteLine("AutomatchAccept Success: " + name, LogCategory.Get("Automatch"), new object[0]);
                    Messaging.SendCustomCommand(name, CustomCommands.AutomatchAcknowledge, new object[0]);
                    this.mFFAPlayers.Add(playerID, name);
                }
                else
                {
                    Messaging.SendCustomCommand(name, CustomCommands.AutomatchBusy, new object[0]);
                }
            }
            else if (this.mState == SupcomAutoState.Searching)
            {
                EventLog.WriteLine("AutomatchAccept Success: " + name, LogCategory.Get("Automatch"), new object[0]);
                this.mState = SupcomAutoState.MatchAccept;
                Messaging.SendCustomCommand(name, CustomCommands.AutomatchAcknowledge, new object[0]);
            }
            else
            {
                Messaging.SendCustomCommand(name, CustomCommands.AutomatchBusy, new object[0]);
            }
        }

        public void AutomatchAcknowledge(int playerID, string name)
        {
            this.NotifyStatus(Loc.Get("<LOC>Game is acknowledged.  Please wait for launch from player "), true);
            this.mPolling = false;
            EventLog.WriteLine("AutomatchAcknowledge Enter: " + name, LogCategory.Get("Automatch"), new object[0]);
            if (this.mState == SupcomAutoState.MatchAccept)
            {
                EventLog.WriteLine("AutomatchAcknowledge Success: " + name, LogCategory.Get("Automatch"), new object[0]);
                this.mState = SupcomAutoState.MatchAcknowledge;
                Messaging.SendCustomCommand(name, CustomCommands.AutomatchConfirm, new object[0]);
                if (this.mKind != "FFA")
                {
                    this.RemoveMatch();
                }
            }
            else
            {
                Messaging.SendCustomCommand(name, CustomCommands.AutomatchBusy, new object[0]);
            }
        }

        public void AutomatchBusy(int playerID, string name)
        {
            EventLog.WriteLine("A player was busy: " + name, LogCategory.Get("Automatch"), new object[0]);
        }

        public void AutomatchConfirm(int playerID, string name)
        {
            if (!(this.mKind == "FFA"))
            {
                this.HostTheGame(playerID, name);
            }
        }

        public void AutomatchLaunch(int playerID, string name, string gamename, int teamid, string hostname, string faction)
        {
            if (this.mKind == "FFA")
            {
                this.RemoveMatch();
            }
            SupComGameManager.LastLocation = GameInformation.SelectedGame.GameLocation;
            ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Leave", null, null, new object[0]);
            int num = 0;
            while (Chatroom.InChatroom)
            {
                Thread.Sleep(10);
                Application.DoEvents();
                num++;
                if (num > 300)
                {
                    break;
                }
            }
            this.mHost = hostname;
            this.mTeam = teamid;
            this.mTeamArg = "/team " + teamid.ToString();
            this.NotifyStatus(Loc.Get("<LOC>Automatch Launch with player "));
            Thread thread = new Thread(delegate (object objgamename) {
                Thread.Sleep(0x3e8);
                if (ConfigSettings.GetBool("DoOldGameList", false))
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "DoJoin", new object[] { "GetGameByName", new object[] { objgamename.ToString() } });
                }
                else
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "DoJoin", new object[] { "GetGameByName2", new object[] { objgamename.ToString(), GameInformation.SelectedGame.GameID } });
                }
            });
            thread.IsBackground = true;
            thread.Start(gamename);
        }

        public void AutomatchNotifyAllies(int playerID, string name, string gamename, int teamid, string hostname)
        {
            if (this.mAllies != null)
            {
                foreach (string str in this.mAllies)
                {
                    if (str != User.Current.Name)
                    {
                        Messaging.SendCustomCommand(str, CustomCommands.AutomatchLaunch, new object[] { gamename, teamid, hostname, this.Faction });
                    }
                }
            }
        }

        public void AutomatchNotifyOpponentTeam(int playerID, string sendername, string teamname, string gamename)
        {
            if (!ConfigSettings.GetBool("IgnoreMultiNotify", false))
            {
                if (this.mNotify.ContainsKey(playerID))
                {
                    DateTime time = (DateTime) this.mNotify[playerID];
                    TimeSpan span = (TimeSpan) (DateTime.Now - time);
                    if (span.TotalMilliseconds < ConfigSettings.GetInt("NotifyDelay", 0x7530))
                    {
                        return;
                    }
                }
                this.mNotify[playerID] = DateTime.Now;
            }
            if (this.mOpponentTeamName == "")
            {
                if ((teamname != null) && (teamname.ToUpper().IndexOf(User.Current.Name.ToUpper()) < 0))
                {
                    this.mOpponentTeamName = teamname;
                    if (this.mOpponentTeamName == "")
                    {
                        this.mOpponentTeamName = playerID.ToString();
                    }
                    if (this.mAllies != null)
                    {
                        foreach (string str in this.mAllies)
                        {
                            if (str != User.Current.Name)
                            {
                                Messaging.SendCustomCommand(sendername, CustomCommands.AutomatchTeamName, new object[] { this.mOpponentTeamName });
                            }
                        }
                    }
                }
                if (this.mTeamName != "")
                {
                    Messaging.SendCustomCommand(sendername, CustomCommands.AutomatchTeamName, new object[] { this.mTeamName });
                }
            }
            if (this.mSupcomGameManager != null)
            {
                this.mSupcomGameManager.OtherTeam = this.mOpponentTeamName;
                this.mSupcomGameManager.MyTeam = this.mTeamName;
            }
        }

        public void AutomatchRequest(int playerID, string name)
        {
            this.NotifyStatus(Loc.Get("<LOC>Automatch Request with player "));
            EventLog.WriteLine("AutomatchRequest Enter: " + name, LogCategory.Get("Automatch"), new object[0]);
            if (this.mState == SupcomAutoState.Searching)
            {
                EventLog.WriteLine("AutomatchRequest Success: " + name, LogCategory.Get("Automatch"), new object[0]);
                this.mState = SupcomAutoState.MatchAccept;
                Messaging.SendCustomCommand(name, CustomCommands.AutomatchAccept, new object[0]);
            }
            else
            {
                EventLog.WriteLine("AutomatchRequest received, but my state is: " + this.mState.ToString(), LogCategory.Get("Automatch"), new object[0]);
            }
        }

        public static string CheckRenameMap(string mapname)
        {
            try
            {
                string oldValue = "";
                foreach (string str2 in mapname.Split(new char[] { '/' }))
                {
                    if (str2.ToUpper().IndexOf(".V") >= 0)
                    {
                        oldValue = str2;
                    }
                }
                if (oldValue != "")
                {
                    string str3 = oldValue.Split(new char[] { '.' })[0];
                    foreach (SupcomMap map in SupcomMapList.Maps)
                    {
                        if (map.Path.ToUpper().IndexOf(oldValue.ToUpper()) >= 0)
                        {
                            string str4 = "";
                            foreach (string str2 in map.Path.Split(@"\".ToCharArray()))
                            {
                                str4 = str4 + str2 + @"\";
                                if (str2.ToUpper() == "MAPS")
                                {
                                    break;
                                }
                            }
                            string path = str4 + @"tournament\";
                            string str6 = str4 + oldValue + @"\";
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            else
                            {
                                Directory.Delete(path, true);
                                Directory.CreateDirectory(path);
                            }
                            foreach (string str7 in Directory.GetFiles(str6, "*.*", SearchOption.AllDirectories))
                            {
                                string str8 = str7.Replace(str6, "");
                                string destFileName = (path + str8).Replace(str3, "tournament");
                                File.Copy(str7, destFileName, true);
                                if (destFileName.ToUpper().IndexOf(".LUA") >= 0)
                                {
                                    StreamReader reader = new StreamReader(destFileName);
                                    string str10 = reader.ReadToEnd();
                                    reader.Close();
                                    str10 = str10.Replace(oldValue, "tournament").Replace(str3, "tournament");
                                    StreamWriter writer = new StreamWriter(destFileName, false);
                                    writer.Write(str10);
                                    writer.Close();
                                }
                            }
                            return "tournament";
                        }
                    }
                }
                return mapname;
            }
            catch
            {
                return mapname;
            }
        }

        public void ClearTeams()
        {
            this.mTeamName = "";
            this.mOpponentTeamName = "";
        }

        public void Dispose()
        {
            this.RemoveMatch();
            Thread.Sleep(100);
        }

        private void DoJoin(DataList data)
        {
            try
            {
                try
                {
                    EventLog.WriteLine("Joining a game.", new object[0]);
                    EventLog.DoStackTrace();
                }
                catch
                {
                }
                this.NotifyStatus(Loc.Get("<LOC>Joining player "));
                this.mSupcomGameManager = new SupComGameManager();
                this.mSupcomGameManager.OnGameLaunched += new EventHandler(this.mSupcomGameManager_OnGameLaunched);
                this.mSupcomGameManager.MyTeam = this.mTeamName;
                this.mSupcomGameManager.OtherTeam = this.mOpponentTeamName;
                this.mSupcomGameManager.BeforeExit += new EventHandler(this.mSupcomGameManager_BeforeExit);
                this.mSupcomGameManager.ForceAddPlayer(this.mHost, "random");
                this.mSupcomGameManager.ForceAddPlayer(User.Current.Name, "random");
                if (this.mSupcomGameManager.OnExitCount() == 0)
                {
                    this.mSupcomGameManager.OnExit += new EventHandler(this.mSupcomGameManager_OnExit);
                }
                string str = data[0]["url"];
                string gamedesc = data[0]["description"];
                this.RegisterGameInfo(gamedesc);
                string[] strArray = str.Split("=;".ToCharArray());
                if (strArray.Length > 3)
                {
                    string address = strArray[1];
                    int port = Convert.ToInt32(strArray[3]);
                    if (!this.mSupcomGameManager.JoinGame(true, gamedesc, address, port, this.mHost, " " + this.Faction + " " + this.mTeamArg))
                    {
                    }
                    if (this.OnLaunchGame != null)
                    {
                        this.OnLaunchGame(this, EventArgs.Empty);
                    }
                }
                else
                {
                    this.AutomatchLaunch(0, "", gamedesc, this.mTeam, this.mHost, this.Faction);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                throw exception;
            }
        }

        private void FinishGetRating(object result)
        {
            try
            {
                DataList list = result as DataList;
                try
                {
                    if (list.Count < 1)
                    {
                        this.mRating = 0x640;
                    }
                    else
                    {
                        this.mRating = Convert.ToInt32(Convert.ToDouble(list[0][0]));
                    }
                }
                catch
                {
                    this.mRating = 0x640;
                }
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", this, "RegisterComplete", new object[] { "AutomatchInsert", new object[] { this.mKind, User.Current.ID, this.mRating } });
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public SupComGameManager GetManager()
        {
            return this.mSupcomGameManager;
        }

        private string GetMap()
        {
            if (this.MapName == "")
            {
                Random random = new Random();
                return ("SCMP_" + random.Next(1, 0x18).ToString().PadLeft(3, '0'));
            }
            return this.MapName;
        }

        public static SupcomAutomatch GetSupcomAutomatch()
        {
            if (sSupcomAutomatch == null)
            {
                sSupcomAutomatch = new SupcomAutomatch();
            }
            return sSupcomAutomatch;
        }

        private void HostTheGame(int playerID, string name)
        {
            ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Leave", null, null, new object[0]);
            int num = 0;
            while (Chatroom.InChatroom)
            {
                Thread.Sleep(10);
                Application.DoEvents();
                num++;
                if (num > 300)
                {
                    break;
                }
            }
            this.NotifyStatus(Loc.Get("<LOC>Automatch Confirm with player "));
            this.mPolling = false;
            EventLog.WriteLine("AutomatchConfirm Enter: " + name, LogCategory.Get("Automatch"), new object[0]);
            if (this.mState == SupcomAutoState.MatchAccept)
            {
                EventLog.WriteLine("AutomatchConfirm Success: " + name, LogCategory.Get("Automatch"), new object[0]);
                this.mState = SupcomAutoState.MatchConfirm;
                this.RemoveMatch();
                string gamename = "AUTOMATCH" + Guid.NewGuid().ToString();
                this.mSupcomGameManager = new SupComGameManager();
                this.mSupcomGameManager.OnGameLaunched += new EventHandler(this.mSupcomGameManager_OnGameLaunched);
                this.mSupcomGameManager.MyTeam = this.mTeamName;
                this.mSupcomGameManager.OtherTeam = this.mOpponentTeamName;
                this.mSupcomGameManager.BeforeExit += new EventHandler(this.mSupcomGameManager_BeforeExit);
                if (this.mSupcomGameManager.OnExitCount() == 0)
                {
                    this.mSupcomGameManager.OnExit += new EventHandler(this.mSupcomGameManager_OnExit);
                }
                string map = this.GetMap();
                string str2 = "/team 3";
                string str3 = "/players 2";
                if (this.mKind == "2v2")
                {
                    str3 = "/players 4";
                }
                else if (this.mKind == "3v3")
                {
                    str3 = "/players 6";
                }
                else if (this.mKind == "4v4")
                {
                    str3 = "/players 8";
                }
                else if (this.mKind == "FFA")
                {
                    str2 = "/team 1";
                    str3 = "/players " + this.mFFAPlayers.Count.ToString();
                }
                this.RegisterGameInfo(gamename);
                this.NotifyStatus(Loc.Get("<LOC>Automatch is being hosted vs. ") + Loc.Get("<LOC>Opponent"));
                this.mSupcomGameManager.mLaunchMap = map;
                this.mSupcomGameManager.HostGame(true, gamename, " /gpgnetmap " + map + " " + this.Faction + " " + str2 + " " + str3);
                this.mSupcomGameManager.ForceAddPlayer(User.Current.Name, "random");
                this.mSupcomGameManager.ForceAddPlayer(name, "random");
                this.mSupcomGameManager.ForceMap(map);
                if (this.OnLaunchGame != null)
                {
                    this.OnLaunchGame(this, EventArgs.Empty);
                }
                EventLog.WriteLine("AutomatchConfirm: Game has been hosted: " + gamename, LogCategory.Get("Automatch"), new object[0]);
                Thread thread = new Thread(new ThreadStart(delegate {
                    try
                    {
                        EventLog.WriteLine("AutomatchConfirm: Entered poll for lobby thread.", LogCategory.Get("Automatch"), new object[0]);
                        while (this.mSupcomGameManager.GameState != GameState.Lobby)
                        {
                            EventLog.WriteLine("AutomatchConfirm: Gamestate is not in lobby.", LogCategory.Get("Automatch"), new object[0]);
                            Thread.Sleep(0x3e8);
                        }
                        EventLog.WriteLine("AutomatchConfirm: SENT LAUNCH COMMAND TO OPPONENT.", LogCategory.Get("Automatch"), new object[0]);
                        if (this.mAllies != null)
                        {
                            Messaging.SendCustomCommand(name, CustomCommands.AutomatchNotifyAllies, new object[] { gamename, 2, User.Current.Name });
                        }
                        if (((this.mKind == "2v2") || (this.mKind == "3v3")) || (this.mKind == "4v4"))
                        {
                            Thread.Sleep(ConfigSettings.GetInt("TeamDelayTime", 0x2710));
                        }
                        if (this.mKind == "FFA")
                        {
                            foreach (KeyValuePair<int, string> pair in this.mFFAPlayers)
                            {
                                Messaging.SendCustomCommand(pair.Value, CustomCommands.AutomatchLaunch, new object[] { gamename, 1, User.Current.Name, "" });
                            }
                        }
                        else
                        {
                            Messaging.SendCustomCommand(name, CustomCommands.AutomatchLaunch, new object[] { gamename, 2, User.Current.Name, "" });
                        }
                        this.AutomatchNotifyOpponentTeam(playerID, name, this.mTeamName, gamename);
                        if (this.mAllies != null)
                        {
                            foreach (string str in this.mAllies)
                            {
                                if (str != User.Current.Name)
                                {
                                    Messaging.SendCustomCommand(str, CustomCommands.AutomatchLaunch, new object[] { gamename, 3, User.Current.Name, this.Faction });
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }.Invoke));
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                Messaging.SendCustomCommand(name, CustomCommands.AutomatchBusy, new object[0]);
            }
        }

        private void mSupcomGameManager_BeforeExit(object sender, EventArgs e)
        {
            this.UpdateTeamInfo();
            string str = "";
            EventLog.WriteLine("Exiting supcom.", LogCategory.Get("SupcomAutomatch"), new object[0]);
            bool flag = false;
            bool flag2 = false;
            EventLog.WriteLine("Entering foreach", LogCategory.Get("SupcomAutomatch"), new object[0]);
            string str2 = "";
            foreach (SupcomPlayerInfo info in this.mSupcomGameManager.GameInfo.Players)
            {
                str = string.Concat(new object[] { str, str2, info.PlayerID, ",", info.PlayerName, ",", info.Status, ",", info.Faction, ",", info.Team });
                str2 = "^";
                EventLog.WriteLine("Got a player info: " + info.PlayerName + " " + info.Status, LogCategory.Get("SupcomAutomatch"), new object[0]);
                if (ConfigSettings.GetBool("CheckVicOldWay", false))
                {
                    if ((info.PlayerName.ToUpper() == User.Current.Name.ToUpper()) && ((info.Status.ToUpper() == "VICTORY") || (info.Status.ToUpper() == "DRAW")))
                    {
                        flag = true;
                    }
                }
                else if (((info.Team + " ").IndexOf(User.Current.ID.ToString() + " ") >= 0) && ((info.Status.ToUpper() == "VICTORY") || (info.Status.ToUpper() == "DRAW")))
                {
                    flag = true;
                }
            }
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "GameEvent", new object[] { User.Current.ID, "Match Result", str, this.mSupcomGameManager.GameName, this.mSupcomGameManager.GameName } });
            if (ConfigSettings.GetBool("CheckIfReportIsValid", true))
            {
                bool flag3 = true;
                if (!flag && (this.mSupcomGameManager.GameInfo.Players.Count > 2))
                {
                    foreach (SupcomPlayerInfo info in this.mSupcomGameManager.GameInfo.Players)
                    {
                        if (((info.Team + " ").IndexOf(User.Current.ID.ToString() + " ") >= 0) && (info.Status.ToUpper() == "PLAYING"))
                        {
                            flag3 = false;
                        }
                    }
                }
                if (!flag3)
                {
                    return;
                }
            }
            string map = this.mSupcomGameManager.GameInfo.Map;
            if (ConfigSettings.GetBool("LookupMapFromDB", true))
            {
                DataList list = DataAccess.GetQueryDataSet("FindMapName", true, new object[0]);
                if (list.Count > 0)
                {
                    map = list[0]["description"];
                    if (map.ToUpper().IndexOf("SCMP") < 0)
                    {
                        map = this.mSupcomGameManager.GameInfo.Map;
                    }
                }
            }
            List<SupComStatsInfo> stats = new List<SupComStatsInfo>();
            foreach (SupcomPlayerInfo info2 in this.mSupcomGameManager.GameInfo.Players)
            {
                SupComStatsInfo item = new SupComStatsInfo();
                item.playername = info2.PlayerName;
                item.result = info2.Status.Replace("Playing", "victory");
                item.faction = info2.Faction;
                item.map = map;
                if (this.mKind.IndexOf("TOURNY") == 0)
                {
                    item.gametype = "1v1";
                }
                else
                {
                    item.gametype = this.mKind;
                }
                TimeSpan span = (TimeSpan) (DateTime.Now - this.mSupcomGameManager.GameInfo.StartTime);
                item.duration = span.TotalSeconds;
                item.kills = info2.Kills;
                item.lost = info2.Lost;
                item.built = info2.Built;
                item.damageDone = info2.DamageDone;
                item.damageReceived = info2.DamageReceived;
                item.energyConsumed = info2.EnergyConsumed;
                item.energyProduced = info2.EnergyProduced;
                item.massConsumed = info2.MassConsumed;
                item.massProduced = info2.MassProduced;
                foreach (SupcomUnitInfo info4 in info2.UnitInfo)
                {
                    SupcomStatUnitInfo info5 = new SupcomStatUnitInfo();
                    info5.built = info4.built;
                    info5.damagedealt = info4.damagedealt;
                    info5.damagereceived = info4.damagereceived;
                    info5.killed = info4.killed;
                    info5.lost = info4.lost;
                    info5.unitid = info4.unitid;
                    item.UnitInfo.Add(info5);
                }
                stats.Add(item);
            }
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            foreach (SupcomPlayerInfo info2 in this.mSupcomGameManager.GameInfo.Players)
            {
                if (info2.Status == "defeat")
                {
                    num2++;
                }
                if (info2.Status == "victory")
                {
                    num++;
                }
                if (info2.Status == "draw")
                {
                    num3++;
                }
            }
            if (num2 == this.mSupcomGameManager.GameInfo.Players.Count)
            {
                flag2 = true;
            }
            if (num3 == this.mSupcomGameManager.GameInfo.Players.Count)
            {
                flag2 = true;
            }
            if (ConfigSettings.GetBool("IgnoreReportMismatch", true))
            {
                int num4 = 0;
                int num5 = 0;
                foreach (SupcomPlayerInfo info2 in this.mSupcomGameManager.GameInfo.Players)
                {
                    if (info2.Status == "victory")
                    {
                        num4++;
                    }
                    else
                    {
                        num5++;
                    }
                    if (((info2.Status != "draw") && ConfigSettings.GetBool("ReassignTeamStatus", true)) && (this.mSupcomGameManager.GameInfo.Players.Count > 2))
                    {
                        if (flag)
                        {
                            if ((this.mSupcomGameManager.MyTeam + " ").IndexOf(info2.PlayerID.ToString() + " ") >= 0)
                            {
                                info2.Status = "victory";
                            }
                            else
                            {
                                info2.Status = "defeat";
                            }
                        }
                        else if ((this.mSupcomGameManager.MyTeam + " ").IndexOf(info2.PlayerID.ToString() + " ") >= 0)
                        {
                            info2.Status = "defeat";
                        }
                        else
                        {
                            info2.Status = "victory";
                        }
                    }
                }
                if ((!ConfigSettings.GetBool("CountAllLosses", true) || (num5 != this.mSupcomGameManager.GameInfo.Players.Count)) && (num4 > num5))
                {
                    return;
                }
            }
            if (this.mKind.IndexOf("TOURNY") == 0)
            {
                foreach (SupcomPlayerInfo info2 in this.mSupcomGameManager.GameInfo.Players)
                {
                    if (info2.PlayerName == User.Current.Name)
                    {
                        TournamentCommands.PostedResults(info2.Status);
                    }
                }
            }
            if ((this.mKind.IndexOf("TOURNY") == 0) && ConfigSettings.GetBool("IgnoreTourneyRating", false))
            {
                EventLog.WriteLine("Ignoring tournament results due to a config setting.", LogCategory.Get("SupcomAutomatch"), new object[0]);
            }
            else
            {
                EventLog.WriteLine("Victory Check", LogCategory.Get("SupcomAutomatch"), new object[0]);
                if (flag)
                {
                    EventLog.WriteLine("WE WON THE GAME!!!", LogCategory.Get("SupcomAutomatch"), new object[0]);
                    ThreadQueue.Quazal.Enqueue(typeof(Game), "ReportWin", null, null, new object[] { 0, this.mSupcomGameManager.OtherTeam, this.mSupcomGameManager.MyTeam, this.mSupcomGameManager.LastXML, stats });
                    DataAccess.QueueExecuteQuery("Update Player Units Agg", new object[0]);
                }
                else
                {
                    EventLog.WriteLine("WE LOST :(", LogCategory.Get("SupcomAutomatch"), new object[0]);
                    try
                    {
                        if ((this.mSupcomGameManager.GameInfo.Players.Count == 2) || flag2)
                        {
                            EventLog.WriteLine("Reporting a loss", LogCategory.Get("SupcomAutomatch"), new object[0]);
                            ThreadQueue.Quazal.Enqueue(typeof(Game), "ReportWin", null, null, new object[] { 0, this.mSupcomGameManager.MyTeam, this.mSupcomGameManager.OtherTeam, this.mSupcomGameManager.LastXML.Replace("Playing", "victory"), stats });
                        }
                        DataAccess.QueueExecuteQuery("Update Player Units Agg", new object[0]);
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
            if (this.OnAutomatchStats != null)
            {
                this.OnAutomatchStats(stats);
            }
        }

        private void mSupcomGameManager_OnExit(object sender, EventArgs e)
        {
            this.Ladder = null;
            if (this.mSupcomGameManager.GameState == GameState.Lobby)
            {
                this.mSupcomGameManager.MessageGame("//AUTOABORT " + User.Current.Name.ToString());
            }
            this.NotifyStatus(Loc.Get("<LOC>Automatch Complete"));
            if (this.OnExit != null)
            {
                this.OnExit(this, EventArgs.Empty);
            }
            if (ConfigSettings.GetBool("AbortState", true))
            {
                Thread.Sleep(ConfigSettings.GetInt("AutoExitSleep", 100));
                try
                {
                    this.mSupcomGameManager.SetAbort();
                    if (ConfigSettings.GetBool("DisposeManager", false))
                    {
                        this.mSupcomGameManager.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                if (ConfigSettings.GetBool("NullOutManagerManager", true))
                {
                    this.mSupcomGameManager = null;
                }
            }
        }

        private void mSupcomGameManager_OnGameLaunched(object sender, EventArgs e)
        {
            if (this.mKind.IndexOf("TOURNY") == 0)
            {
                TournamentCommands.GameStarted();
            }
            if (this.Ladder != null)
            {
                string str = DataAccess.FormatDate(DateTime.UtcNow);
                if (!new QuazalQuery("CreateLadderGameSession", new object[] { this.Ladder.ID, User.Current.ID, DateTime.UtcNow, this.IsLadderAutomatch }).ExecuteNonQuery())
                {
                    ErrorLog.WriteLine("Failed to create ladder session for automatch game.", new object[0]);
                }
            }
            this.Ladder = null;
        }

        public void NotifyStatus(string status)
        {
            this.NotifyStatus(status, false);
        }

        public void NotifyStatus(string status, bool showCancel)
        {
            if (this.OnStatusChanged != null)
            {
                this.OnStatusChanged(status, new object[] { showCancel });
            }
        }

        public void PollComplete(DataList poll)
        {
            this.NotifyStatus(Loc.Get("<LOC>Searching for game"), true);
            EventLog.WriteLine("Polling complete", LogCategory.Get("Automatch"), new object[0]);
            MappedObjectList<AutomatchPlayers> list = new MappedObjectList<AutomatchPlayers>(poll);
            if ((list != null) && ((this.mKind != "FFA") || (list.Count > 4)))
            {
                foreach (AutomatchPlayers players in list)
                {
                    Messaging.SendCustomCommand(players.Name, CustomCommands.AutomatchRequest, new object[0]);
                }
            }
        }

        private void RegisterComplete(bool result)
        {
            VGen0 gen = null;
            EventLog.WriteLine("RegisterComplete", LogCategory.Get("Automatch"), new object[0]);
            if (result)
            {
                this.mState = SupcomAutoState.Searching;
                if (gen == null)
                {
                    gen = delegate {
                        this.mPolling = true;
                        while (this.mPolling)
                        {
                            this.RetryPoll();
                            int num = new Random().Next(-5000, 0x1388);
                            Thread.Sleep((int) (0x7530 + num));
                        }
                    };
                }
                Thread thread = new Thread(new ThreadStart(gen.Invoke));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void RegisterGameInfo(string gamedesc)
        {
            int[] numArray = new int[] { -1, -1, -1, -1 };
            int index = 0;
            if ((this.mTeamName != null) && (this.mTeamName != ""))
            {
                foreach (string str in this.mTeamName.Split(new char[] { ' ' }))
                {
                    try
                    {
                        numArray[index] = Convert.ToInt32(str.Trim());
                        index++;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                numArray[0] = User.Current.ID;
            }
            DataAccess.QueueExecuteQuery("Register Automatch Info2", new object[] { numArray[0], numArray[1], numArray[2], numArray[3], gamedesc, this.Kind });
        }

        internal void RegisterLadderGame(LadderInstance ladder, bool isAutomatch)
        {
            this.Ladder = ladder;
            this.IsLadderAutomatch = isAutomatch;
        }

        public void RegisterNewGame(string kind)
        {
            this.RegisterNewGame(kind, null, User.Current.ID.ToString());
        }

        public void RegisterNewGame(string kind, List<string> allies, string teamName)
        {
            this.RegisterNewGame(kind, allies, teamName, false);
        }

        public void RegisterNewGame(string kind, List<string> allies, string teamName, bool isTournament)
        {
            this.IsTeamLeader = true;
            if (this.mState == SupcomAutoState.Unavailable)
            {
                this.mOpponentTeamName = "";
                this.mKind = kind;
                this.mAllies = allies;
                if (this.IsRanked1v1)
                {
                    this.mTeamName = User.Current.ID.ToString();
                }
                else
                {
                    this.mTeamName = teamName;
                }
                this.RemoveMatch();
                if (this.IsDirectChallenge)
                {
                    this.FinishGetRating(new DataList());
                }
                else
                {
                    EventLog.WriteLine("Register New Game", LogCategory.Get("Automatch"), new object[0]);
                    if (ConfigSettings.GetBool("DoOldGameList", false))
                    {
                        ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "InitializeTeamRating", new object[] { this.mKind, this.mTeamName } });
                        ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "FinishGetRating", new object[] { "GetTeamRating", new object[] { this.mTeamName } });
                    }
                    else
                    {
                        ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "InitializeTeamRating2", new object[] { this.mKind, this.mTeamName, GameInformation.SelectedGame.GameID } });
                        ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "FinishGetRating", new object[] { "GetTeamRating2", new object[] { this.mTeamName, GameInformation.SelectedGame.GameID } });
                    }
                }
            }
        }

        public bool RemoveMatch()
        {
            this.IsTeamLeader = false;
            this.mPolling = false;
            this.mState = SupcomAutoState.Unavailable;
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "AutomatchDelete", new object[0] });
            return true;
        }

        internal void ResetLadderGame()
        {
            this.Ladder = null;
            this.IsLadderAutomatch = false;
        }

        public void RetryPoll()
        {
            this.NotifyStatus(Loc.Get("<LOC>Polling for game"), true);
            if (this.mState != SupcomAutoState.Searching)
            {
                EventLog.WriteLine("Retry Poll ignored.  In a nonsearching state.", LogCategory.Get("Automatch"), new object[0]);
                if (this.mPollCount > 1)
                {
                    this.mState = SupcomAutoState.Searching;
                }
                this.mPollCount++;
            }
            else
            {
                this.mPollCount = 0;
                if (this.mRatingThreshhold == 0)
                {
                    this.mRatingThreshhold = ConfigSettings.GetInt("RatingThreshhold", 250);
                }
                else
                {
                    this.mRatingThreshhold += ConfigSettings.GetInt("RatingSearchGrowth", 0x19);
                }
                if (this.mRatingThreshhold > ConfigSettings.GetInt("RatingMaxThresh", 500))
                {
                    this.mRatingThreshhold = ConfigSettings.GetInt("RatingMaxThresh", 500);
                }
                EventLog.WriteLine("Retry Poll", LogCategory.Get("Automatch"), new object[0]);
                if (ConfigSettings.GetBool("DoOldGameList", false))
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "PollComplete", new object[] { "AutomatchSelect", new object[] { this.mRating - this.mRatingThreshhold, this.mRating + this.mRatingThreshhold, this.mKind } });
                }
                else
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "PollComplete", new object[] { "AutomatchSelect2", new object[] { this.mRating - this.mRatingThreshhold, this.mRating + this.mRatingThreshhold, this.mKind, GameInformation.SelectedGame.GameID } });
                }
            }
        }

        private void UpdatePlayers(int playerid, string name, string teamname)
        {
            foreach (SupcomPlayerInfo info in this.mSupcomGameManager.GameInfo.Players)
            {
                if (info.PlayerName == name)
                {
                    info.PlayerID = playerid;
                    info.Team = teamname;
                }
            }
        }

        private void UpdateTeamInfo()
        {
            bool gotdata = false;
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                try
                {
                    DataList queryData = DataAccess.GetQueryData("Get Game Description from Info", new object[0]);
                    if (queryData.Count != 0)
                    {
                        string str = queryData[0]["game_description"];
                        DataList list2 = DataAccess.GetQueryData("Automatch Team Info", new object[] { str });
                        bool flag = false;
                        bool flag2 = false;
                        foreach (DataRecord record in list2)
                        {
                            int item = Convert.ToInt32(record["player1"]);
                            int num2 = Convert.ToInt32(record["player2"]);
                            int num3 = Convert.ToInt32(record["player3"]);
                            int num4 = Convert.ToInt32(record["player4"]);
                            List<int> list3 = new List<int>();
                            if (item != -1)
                            {
                                list3.Add(item);
                            }
                            if (num2 != -1)
                            {
                                list3.Add(num2);
                            }
                            if (num3 != -1)
                            {
                                list3.Add(num3);
                            }
                            if (num4 != -1)
                            {
                                list3.Add(num4);
                            }
                            list3.Sort();
                            string teamname = "";
                            foreach (int num5 in list3)
                            {
                                teamname = teamname + num5.ToString() + " ";
                            }
                            if (item != -1)
                            {
                                this.UpdatePlayers(item, record["name1"], teamname);
                            }
                            if (num2 != -1)
                            {
                                this.UpdatePlayers(num2, record["name2"], teamname);
                            }
                            if (num3 != -1)
                            {
                                this.UpdatePlayers(num3, record["name3"], teamname);
                            }
                            if (num4 != -1)
                            {
                                this.UpdatePlayers(num4, record["name4"], teamname);
                            }
                            if (!flag && list3.Contains(User.Current.ID))
                            {
                                flag = true;
                                teamname = teamname.Trim();
                                this.mSupcomGameManager.MyTeam = teamname;
                            }
                            if (!flag2 && !list3.Contains(User.Current.ID))
                            {
                                flag2 = true;
                                teamname = teamname.Trim();
                                this.mSupcomGameManager.OtherTeam = teamname;
                            }
                        }
                    }
                }
                catch
                {
                }
                gotdata = true;
            }, new object[0]);
            for (int i = 0; !gotdata && (i < ConfigSettings.GetInt("TeamCheckSafetyCount", 200)); i++)
            {
                Thread.Sleep(50);
            }
        }

        public string Faction
        {
            get
            {
                if ((((this.mFaction == "/uef") || (this.mFaction == "/aeon")) || (this.mFaction == "/cybran")) || (this.mFaction == "/seraphim"))
                {
                    return this.mFaction;
                }
                switch ((Environment.TickCount % 4))
                {
                    case 1:
                        return "/aeon";

                    case 2:
                        return "/cybran";

                    case 3:
                        return "/seraphim";
                }
                return "/uef";
            }
            set
            {
                this.mFaction = value;
            }
        }

        public bool IsDirectChallenge
        {
            get
            {
                return ((this.mKind.IndexOf("TOURNY") == 0) || (this.mKind.IndexOf("CHALLENGE") == 0));
            }
        }

        public bool IsRanked1v1
        {
            get
            {
                return (((this.mKind == "1v1") || (this.mKind.IndexOf("TOURNY") == 0)) || (this.mKind.IndexOf("CHALLENGE") == 0));
            }
        }

        public string Kind
        {
            get
            {
                return this.mKind;
            }
            set
            {
                this.mKind = value;
            }
        }

        public SupcomAutoState State
        {
            get
            {
                return this.mState;
            }
        }
    }
}

