namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class TournamentCommands
    {
        public static string sChatroom = "";
        private static int sDirectorID = 0;
        public static string sDirectorName = "";
        public static string sFaction = "Any Faction";
        public static string sMap = "Ladder Maps";
        public static Dictionary<string, string> sRespondingUsers = new Dictionary<string, string>();
        private static int sTournamentID = 0;

        public static void GameStarted()
        {
            Messaging.SendCustomCommand(sDirectorName, CustomCommands.TournamentGameStarted, new object[0]);
        }

        public static void PostedResults(string result)
        {
            Messaging.SendCustomCommand(sDirectorName, CustomCommands.TournamentResult, new object[] { result });
            DataAccess.QueueExecuteQuery("Tournament Player Result", new object[] { result, sTournamentID });
        }

        public static void ProcessCustomCommand(FrmMain main, CustomCommands cmd, int senderID, string senderName, string[] args)
        {
            VGen0 method = null;
            VGen0 gen2 = null;
            VGen0 gen3 = null;
            VGen0 gen4 = null;
            VGen0 gen5 = null;
            VGen0 gen6 = null;
            EventLog.WriteLine("Running command " + cmd.ToString() + " " + senderID.ToString() + " " + senderName, LogCategory.Get("TournamentCommands"), args);
            if (cmd == CustomCommands.TournamentLaunchGame)
            {
                sDirectorName = senderName;
                sDirectorID = senderID;
                sTournamentID = Convert.ToInt32(args[1]);
                try
                {
                    Thread.Sleep(Convert.ToInt32(args[2]));
                }
                catch
                {
                }
                sFaction = args[3].ToLower();
                sMap = args[4];
                foreach (KeyValuePair<string, string> pair in GameItem.MapNameLookup)
                {
                    if (pair.Value.ToUpper() == sMap.ToUpper())
                    {
                        sMap = pair.Key;
                    }
                }
                sMap = SupcomAutomatch.CheckRenameMap(sMap);
                Random random = new Random();
                Thread.Sleep(random.Next(10, 0xfa0));
                if (method == null)
                {
                    method = delegate {
                        main.PlayRankedGame(true, args[0], new List<string>(), "", true);
                    };
                }
                main.Invoke(method);
            }
            else if (cmd == CustomCommands.TournamentResult)
            {
                if (DlgManageTournament.Current != null)
                {
                    if (gen2 == null)
                    {
                        gen2 = delegate {
                            DlgManageTournament.Current.UpdateStats(senderID, args[0]);
                        };
                    }
                    DlgManageTournament.Current.Invoke(gen2);
                }
            }
            else if (cmd == CustomCommands.TournamentChatroom)
            {
                sChatroom = args[0];
                if (gen3 == null)
                {
                    gen3 = delegate {
                        main.SystemMessage(Loc.Get("<LOC>You are being rerouted to a new chatroom for the tournament you signed up for."), new object[0]);
                        main.JoinChat(args[0]);
                    };
                }
                main.Invoke(gen3);
            }
            else if (cmd == CustomCommands.TournamentRequestStatus)
            {
                if (gen4 == null)
                {
                    gen4 = delegate {
                        Messaging.SendCustomCommand(senderName, CustomCommands.TournamentAcknowledgeStatus, new object[] { SupcomAutomatch.GetSupcomAutomatch().State.ToString() });
                    };
                }
                main.Invoke(gen4);
            }
            else if (cmd == CustomCommands.TournamentChatmessage)
            {
                if (gen5 == null)
                {
                    gen5 = delegate {
                        main.SystemEvent(args[0], new object[0]);
                    };
                }
                main.Invoke(gen5);
            }
            else if (cmd == CustomCommands.TournamentEndMatch)
            {
                main.Invoke(delegate {
                    try
                    {
                        SupcomAutomatch.GetSupcomAutomatch().GetManager().ForceCloseGame(Loc.Get("<LOC>The Tournament Director has ended your game."));
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                });
            }
            else if (cmd == CustomCommands.TournamentAcknowledgeStatus)
            {
                if (gen6 == null)
                {
                    gen6 = delegate {
                        sRespondingUsers.Add(senderName, args[0]);
                        if (DlgManageTournament.Current != null)
                        {
                            DlgManageTournament.Current.UpdateItemStatus(senderName, args[0]);
                        }
                    };
                }
                main.Invoke(gen6);
            }
        }
    }
}

