namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.replay;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class Game
    {
        private static bool mConnectivitySet;

        public static  event NatMessageDelegate OnNatMessage;

        public static bool AddPrivateURL(string address, int port)
        {
            return Lobby.GetLobby().matchmakingHelper.AddPrivateURL(address, port);
        }

        public static bool CreateGame(string description, string mapName, int rating, int maxplayers, string gameName, string version, string customData)
        {
            return Lobby.GetLobby().gameHelper.CreateGame(description, mapName, rating, maxplayers, gameName, version, customData);
        }

        public static bool EndGame()
        {
            return Lobby.GetLobby().gameHelper.EndGame();
        }

        public static object GetGameIps()
        {
            return DataAccess.GetQueryData("GetGameIps", new object[0]);
        }

        public static object GetGameList()
        {
            if (ConfigSettings.GetBool("DoOldGameList", false))
            {
                return DataAccess.GetObjects<GameItem>("GetGamesEx", new object[0]);
            }
            return DataAccess.GetObjects<GameItem>("GetGamesEx2", new object[] { GPGnetSelectedGame.SelectedGame.GameID });
        }

        public static void IncomingNatMessage(string address, byte[] message, int size)
        {
            Lobby.GetLobby().matchmakingHelper.ReceiveMessage(address, message, size);
        }

        public static string JoinGame(string gameName)
        {
            return Lobby.GetLobby().gameHelper.JoinGame(gameName);
        }

        public static bool LaunchSession(string stationURL)
        {
            return Lobby.GetLobby().matchmakingHelper.LaunchSession(stationURL);
        }

        public static bool LeaveGame()
        {
            return Lobby.GetLobby().gameHelper.LeaveGame();
        }

        public static bool MessageGame(string message)
        {
            return Lobby.GetLobby().chatroomHelper.MessageChatroom(message);
        }

        private static void NatMessageHandler(SNatMessage natMessage)
        {
            if (OnNatMessage != null)
            {
                OnNatMessage(natMessage);
            }
        }

        public static int ProbeStations()
        {
            return Lobby.GetLobby().matchmakingHelper.ProbeStations();
        }

        public static bool ReportWin(int gathering, string loserteam, string winnerteam, string gamexml, List<SupComStatsInfo> playerstats)
        {
            if (gamexml == null)
            {
                EventLog.WriteLine("GameXML is null and we are returning false", LogCategory.Get("Game"), new object[0]);
            }
            else
            {
                EventLog.WriteLine("GameXML was added: " + gamexml, LogCategory.Get("Game"), new object[0]);
            }
            if ((gamexml == null) || (gamexml.Length < ConfigSettings.GetInt("MinReportLength", 200)))
            {
                return false;
            }
            try
            {
                List<ResultsInfo> list = new List<ResultsInfo>();
                ResultsInfo item = new ResultsInfo {
                    PlayerName = winnerteam,
                    WonGame = 1
                };
                list.Add(item);
                item = new ResultsInfo {
                    PlayerName = loserteam,
                    WonGame = 0
                };
                list.Add(item);
                DataList list2 = DataAccess.GetQueryDataSet("GetGameGatheringID", true, new object[0]);
                int num = -1;
                if (list2.Count > 0)
                {
                    EventLog.WriteLine("We have a data row.", LogCategory.Get("Game"), new object[0]);
                    if (list2[0].Count > 0)
                    {
                        num = Convert.ToInt32(list2[0][0]);
                    }
                }
                bool flag = false;
                if ((winnerteam == User.Current.ID.ToString()) || !flag)
                {
                    if (ConfigSettings.GetBool("ReportOldWay", false) || (playerstats.Count == 0))
                    {
                        EventLog.WriteLine("OLD Reporting Game", LogCategory.Get("Game"), new object[0]);
                        Lobby.GetLobby().competitionHelper.ReportGame(gathering, 0, 0, list.ToArray());
                        EventLog.WriteLine("OLD Getting game identification", LogCategory.Get("Game"), new object[0]);
                    }
                    else
                    {
                        EventLog.WriteLine("NEW Reporting Game", LogCategory.Get("Game"), new object[0]);
                        Lobby.GetLobby().competitionHelper.CreateGameReport(gathering, playerstats[0].map, playerstats[0].duration, playerstats[0].gametype);
                        foreach (SupComStatsInfo info2 in playerstats)
                        {
                            string teamName = winnerteam;
                            int num2 = Lobby.GetLobby().authenticationHelper.FindIDByPlayerName(info2.playername);
                            if ((teamName + " ").IndexOf(num2.ToString() + " ") < 0)
                            {
                                teamName = loserteam;
                            }
                            teamName = teamName.Trim();
                            Lobby.GetLobby().competitionHelper.AddResult(gathering, info2.playername, info2.faction, info2.kills, info2.lost, info2.built, info2.damageDone, info2.damageReceived, teamName, info2.energyConsumed, info2.energyProduced, info2.massConsumed, info2.massProduced, info2.result);
                            foreach (SupcomStatUnitInfo info3 in info2.UnitInfo)
                            {
                                Lobby.GetLobby().competitionHelper.AddUnit(gathering, info2.playername, info3.unitid, info3.built, info3.lost, info3.killed, info3.damagedealt, info3.damagereceived);
                            }
                        }
                        Lobby.GetLobby().competitionHelper.SubmitGameReport();
                        EventLog.WriteLine("NEW Getting game identification", LogCategory.Get("Game"), new object[0]);
                    }
                }
                DataAccess.ExecuteQuery("UpdateGameResult", new object[0]);
                EventLog.WriteLine("Sending game report.", LogCategory.Get("Game"), new object[0]);
                string s = gamexml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", "").Replace("<GameInfo", "<GameInfo gameid=\"" + num.ToString() + "\"");
                EventLog.WriteLine("Game XML:\r\n " + s, LogCategory.Get("Game"), new object[0]);
                if (ConfigSettings.GetBool("LogGameToQuazal", false))
                {
                    string str3 = Convert.ToBase64String(Compression.CompressData(Encoding.ASCII.GetBytes(s)));
                    EventLog.WriteLine("Updating game to Quazal.", new object[0]);
                    DataAccess.ExecuteQuery("LogGameReport", new object[] { num, str3 });
                }
                ConfigSettings.GetBool("LogDetailsToQuazal", true);
                if (DataAccess.GetBool("GetConfigSetting", new object[] { "log_games" }))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(s);
                    new GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.Service { Url = ConfigSettings.GetString("GameStatsService", "http://gpgnet.gaspowered.com/gamereport/Service.asmx?WSDL") }.SubmitGameReport(Lobby.GetLobby().authenticationHelper.Server + "_" + Lobby.GetLobby().authenticationHelper.Port.ToString() + "_" + num.ToString(), User.Current.Name, bytes);
                    DataAccess.ExecuteQuery("SubmissionComplete", new object[] { num });
                }
                return false;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public static void SetConnectivity()
        {
            if (!mConnectivitySet)
            {
                mConnectivitySet = true;
                Lobby.GetLobby().matchmakingHelper.NatMessageHandler += new NatMessageDelegate(Game.NatMessageHandler);
                Lobby.GetLobby().matchmakingHelper.RegisterAdapter(MatchmakingHelper.NatHeader);
                Lobby.GetLobby().matchmakingHelper.RegisterUDPConnection();
            }
        }

        public static bool StartGame()
        {
            return Lobby.GetLobby().gameHelper.StartGame();
        }

        public static string SubmitGame(string playername, byte[] gamedata)
        {
            return new GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.replay.Service { Url = ConfigSettings.GetString("GameReportService", "http://replay.gaspowered.com/SubmitBetaReplay/service.asmx?WSDL") }.SubmitGame(playername, gamedata);
        }

        public static bool UpdateGame(string mapName, int rating, int maxPlayers, string gameName, string version, string customData)
        {
            return Lobby.GetLobby().gameHelper.UpdateGame(mapName, rating, maxPlayers, gameName, version, customData);
        }

        public static void UpdateIP()
        {
            Lobby.UpdateIP();
        }

        public static bool UploadFileOnServer(string fileName, Uri serverUri, string username, string pass)
        {
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return false;
            }
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(serverUri);
            request.Method = "STOR";
            request.UseBinary = true;
            request.KeepAlive = true;
            request.UsePassive = true;
            FileStream stream = new FileStream(fileName, FileMode.Open);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int) stream.Length);
            stream.Close();
            request.ContentLength = buffer.Length;
            request.Credentials = new NetworkCredential(username, pass);
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();
            ((FtpWebResponse) request.GetResponse()).Close();
            return true;
        }
    }
}

