namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class LadderService
    {
        private static bool mHasErrors;
        private static IStatusProvider StatusProvider;

        internal static void Error(Exception ex)
        {
            mHasErrors = true;
            Status("ERROR: " + ex.Message, new object[0]);
        }

        internal static void Error(string error, params object[] args)
        {
            mHasErrors = true;
            Status("ERROR: " + error, args);
        }

        private static void ProcessLadderDegradation()
        {
            try
            {
                Status("Processing ladder degradation", new object[0]);
                foreach (LadderInstance instance in LadderInstance.AllInstances.Values)
                {
                    int? nullable = null;
                    foreach (LadderDegradation degradation in instance.LadderDefinition.Degradation)
                    {
                        int num = 0;
                        if (degradation.RankTo.HasValue)
                        {
                            num = degradation.RankTo.Value;
                        }
                        LadderParticipant[] participantArray = new QuazalQuery("GetLadderDegradeParticipants", new object[] { instance.ID, degradation.RankFrom, num, num, degradation.DegradeDayInterval, DateTime.UtcNow }).GetObjects<LadderParticipant>().ToArray();
                        foreach (LadderParticipant participant in participantArray)
                        {
                            Status("Degrading participant ID: {0}, Name: {1} on ladder: {2} based on degradation rules: {3}", new object[] { participant.EntityID, participant.EntityName, participant.LadderInstanceID, degradation.ID });
                            if (!nullable.HasValue)
                            {
                                nullable = new int?(new QuazalQuery("GetLastLadderRank", new object[] { instance.ID }).GetInt());
                            }
                            participant.LadderInstance.LadderDefinition.Resolver.DegradeParticipant(participant, degradation, nullable.Value);
                        }
                        if ((participantArray.Length > 0) && !new QuazalQuery("ResetLadderDegradeParticipants", new object[] { DateTime.UtcNow, instance.ID, degradation.RankFrom, num, num, degradation.DegradeDayInterval, DateTime.UtcNow }).ExecuteNonQuery())
                        {
                            Error("Failed to reset last degrade dates for participants on ladder: {0}", new object[] { instance.Description });
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Error("Processing ladder degradation", new object[0]);
                Error(exception);
            }
        }

        private static void ProcessLadderJoins()
        {
            try
            {
                Status("Processing ladder joins", new object[0]);
                foreach (DataRecord record in new QuazalQuery("GetLadderJoinQueues", new object[0]).GetData())
                {
                    int entityId = int.Parse(record["entity_id"]);
                    string entityName = record["entity_name"];
                    int ladderInstance = int.Parse(record["ladder_instance_id"]);
                    DataAccess.FormatDate(DateTime.UtcNow);
                    int @int = new QuazalQuery("GetNextLadderRank", new object[] { ladderInstance }).GetInt();
                    LadderInstance instance = LadderInstance.AllInstances[ladderInstance];
                    instance.LadderDefinition.Resolver.AddParticipant(entityId, entityName, ladderInstance, @int);
                }
                if (!new QuazalQuery("ClearLadderJoinQueues", new object[0]).ExecuteNonQuery())
                {
                    Error("Failed to clear ladder join queues after proessing them", new object[0]);
                }
            }
            catch (Exception exception)
            {
                Error("Processing ladder removals", new object[0]);
                Error(exception);
            }
        }

        public static void ProcessLadderQueue(IStatusProvider statusProvider)
        {
            try
            {
                StatusProvider = statusProvider;
                mHasErrors = false;
                DateTime dateTime = ConfigSettings.GetDateTime("LastLadderJoinProc", DateTime.UtcNow);
                TimeSpan span = TimeSpan.FromMinutes((double) ConfigSettings.GetInt("LadderJoinInterval", 5));
                if ((DateTime.UtcNow - dateTime) >= span)
                {
                    ProcessLadderJoins();
                    if (!new QuazalQuery("SetLadderConfig", new object[] { "LastLadderJoinProc", DataAccess.FormatDate(DateTime.UtcNow) }).ExecuteNonQuery())
                    {
                        Error("Failed to update the last process time for joins", new object[0]);
                    }
                }
                else
                {
                    object[] args = new object[1];
                    TimeSpan span6 = (TimeSpan) (DateTime.UtcNow - dateTime);
                    args[0] = Math.Round((double) (span.TotalMinutes - span6.TotalMinutes), 2);
                    Status("Skipping joins processing for the next {0} minutes", args);
                }
                DateTime time2 = ConfigSettings.GetDateTime("LastLadderReportsProc", DateTime.UtcNow);
                TimeSpan span2 = TimeSpan.FromMinutes((double) ConfigSettings.GetInt("LadderReportsInterval", 10));
                if ((DateTime.UtcNow - time2) >= span2)
                {
                    ProcessLadderReports();
                    if (!new QuazalQuery("SetLadderConfig", new object[] { "LastLadderReportsProc", DataAccess.FormatDate(DateTime.UtcNow) }).ExecuteNonQuery())
                    {
                        Error("Failed to update the last process time for reports", new object[0]);
                    }
                }
                else
                {
                    object[] objArray4 = new object[1];
                    TimeSpan span7 = (TimeSpan) (DateTime.UtcNow - time2);
                    objArray4[0] = Math.Round((double) (span2.TotalMinutes - span7.TotalMinutes), 2);
                    Status("Skipping reports processing for the next {0} minutes", objArray4);
                }
                DateTime time3 = ConfigSettings.GetDateTime("LastLadderRemovalProc", DateTime.UtcNow);
                TimeSpan span3 = TimeSpan.FromMinutes((double) ConfigSettings.GetInt("LadderRemovalInterval", 15));
                if ((DateTime.UtcNow - time3) >= span3)
                {
                    ProcessLadderRemovals();
                    if (!new QuazalQuery("SetLadderConfig", new object[] { "LastLadderRemovalProc", DataAccess.FormatDate(DateTime.UtcNow) }).ExecuteNonQuery())
                    {
                        Error("Failed to update the last process time for removals", new object[0]);
                    }
                }
                else
                {
                    object[] objArray6 = new object[1];
                    TimeSpan span8 = (TimeSpan) (DateTime.UtcNow - time3);
                    objArray6[0] = Math.Round((double) (span3.TotalMinutes - span8.TotalMinutes), 2);
                    Status("Skipping removals processing for the next {0} minutes", objArray6);
                }
                DateTime time4 = ConfigSettings.GetDateTime("LastLadderDegradeProc", DateTime.UtcNow);
                TimeSpan span4 = TimeSpan.FromMinutes((double) ConfigSettings.GetInt("LadderDegradeInterval", 60));
                if ((DateTime.UtcNow - time4) >= span4)
                {
                    ProcessLadderDegradation();
                    if (!new QuazalQuery("SetLadderConfig", new object[] { "LastLadderDegradeProc", DataAccess.FormatDate(DateTime.UtcNow) }).ExecuteNonQuery())
                    {
                        Error("Failed to update the last process time for degradation", new object[0]);
                    }
                }
                else
                {
                    object[] objArray8 = new object[1];
                    TimeSpan span9 = (TimeSpan) (DateTime.UtcNow - time4);
                    objArray8[0] = Math.Round((double) (span4.TotalMinutes - span9.TotalMinutes), 2);
                    Status("Skipping degradation processing for the next {0} minutes", objArray8);
                }
                DateTime time5 = ConfigSettings.GetDateTime("LastLadderSessionsProc", DateTime.UtcNow);
                TimeSpan span5 = TimeSpan.FromMinutes((double) ConfigSettings.GetInt("LadderSessionsInterval", 180));
                if ((DateTime.UtcNow - time5) >= span5)
                {
                    ProcessStaleGameSessions();
                    if (!new QuazalQuery("SetLadderConfig", new object[] { "LastLadderSessionsProc", DataAccess.FormatDate(DateTime.UtcNow) }).ExecuteNonQuery())
                    {
                        Error("Failed to update the last process time for stale sessions", new object[0]);
                    }
                }
                else
                {
                    object[] objArray10 = new object[1];
                    TimeSpan span10 = (TimeSpan) (DateTime.UtcNow - time5);
                    objArray10[0] = Math.Round((double) (span5.TotalMinutes - span10.TotalMinutes), 2);
                    Status("Skipping stale sessions processing for the next {0} minutes", objArray10);
                }
            }
            catch (Exception exception)
            {
                Error("Error processing ladder queue", new object[0]);
                Error(exception);
            }
        }

        private static void ProcessLadderRemovals()
        {
            try
            {
                Status("Processing ladder removals", new object[0]);
                LadderParticipant participant = new QuazalQuery("GetLadderParticipantRemovals", new object[] { DateTime.UtcNow, ConfigSettings.GetInt("LadderJoinIdleLimitDays", 3) }).GetObject<LadderParticipant>();
                while (participant != null)
                {
                    Status("Removing participant ID: {0}, Name: {1} from ladder: {2}", new object[] { participant.EntityID, participant.EntityName, participant.LadderInstanceID });
                    if (!new QuazalQuery("RemoveFromLadder", new object[] { participant.EntityID, participant.LadderInstanceID }).ExecuteNonQuery())
                    {
                        Error("Failed to remove participant ID: {0}, Name: {1} from ladder: {2}", new object[] { participant.EntityID, participant.EntityName, participant.LadderInstanceID });
                        return;
                    }
                    string str = string.Format("{0}(rank {1}) has left ladder: {2}", participant.EntityName, participant.Rank, participant.LadderInstance.Description);
                    if (!new QuazalQuery("CreateLadderGameResult", new object[] { participant.EntityID, "NULL", "NULL", participant.LadderInstanceID, str, participant.Rank, 0, DateTime.UtcNow }).ExecuteNonQuery())
                    {
                        Error("Failed to record ladder removal audit record for player: {0}(rank {1})", new object[] { participant.EntityName, participant.Rank });
                    }
                    if (!new QuazalQuery("DecrementLadderAbove", new object[] { participant.LadderInstanceID, participant.Rank }).ExecuteNonQuery())
                    {
                        Error("Failed to decrement ladder members above rank {0} on ladder {1} when removing participant ID: {2}, Name: {3}", new object[] { participant.Rank, participant.LadderInstanceID, participant.EntityID, participant.EntityName });
                        return;
                    }
                    participant = new QuazalQuery("GetLadderParticipantRemovals", new object[] { DateTime.UtcNow, ConfigSettings.GetInt("LadderJoinIdleLimitDays", 3) }).GetObject<LadderParticipant>();
                    Thread.Sleep(20);
                }
            }
            catch (Exception exception)
            {
                Error("Processing ladder removals", new object[0]);
                Error(exception);
            }
        }

        private static void ProcessLadderReports()
        {
            try
            {
                Status("Processing game reports", new object[0]);
                Dictionary<int, List<LadderGameReport>> dictionary = new Dictionary<int, List<LadderGameReport>>();
                foreach (LadderGameReport report in new QuazalQuery("GetLadderReports", new object[0]).GetObjects<LadderGameReport>().ToArray())
                {
                    if (!dictionary.ContainsKey(report.GameID))
                    {
                        dictionary[report.GameID] = new List<LadderGameReport>();
                    }
                    dictionary[report.GameID].Add(report);
                }
                Dictionary<int, string> dictionary2 = new Dictionary<int, string>(dictionary.Count * 2);
                List<int> list = new List<int>();
                string str = "";
                string str2 = "";
                string str3 = "";
                int num = 0;
                foreach (List<LadderGameReport> list2 in dictionary.Values)
                {
                    try
                    {
                        Dictionary<int, string> dictionary3;
                        if (list2.Count < 1)
                        {
                            continue;
                        }
                        if (list2.Count != 1)
                        {
                            goto Label_0542;
                        }
                        TimeSpan span = (TimeSpan) (DateTime.UtcNow - DateTime.SpecifyKind(list2[0].ReportDate, DateTimeKind.Utc));
                        if (span.TotalMinutes < ConfigSettings.GetDouble("LadderGameReportTimeout", 15.0))
                        {
                            continue;
                        }
                        Status("Processing report for game {0}", new object[] { list2[0].GameID });
                        LadderGameReport[] allParticipants = new QuazalQuery("GetGameSessionMembersByGameID", new object[] { list2[0].GameID }).GetObjects<LadderGameReport>().ToArray();
                        if (allParticipants.Length < 2)
                        {
                            Error("Failed to find all members of game {0} on ladder {1}", new object[] { list2[0].GameID, list2[0].LadderInstanceID });
                            str = str + str3 + list2[0].GameID.ToString();
                            str3 = ",";
                            num++;
                            continue;
                        }
                        if (list.Contains(list2[0].LadderInstanceID))
                        {
                            if (allParticipants[0].EntityID == list2[0].EntityID)
                            {
                                list2[0].Rank = allParticipants[0].Rank;
                                goto Label_02EB;
                            }
                            if (allParticipants[1].EntityID == list2[0].EntityID)
                            {
                                list2[0].Rank = allParticipants[1].Rank;
                                goto Label_02EB;
                            }
                            Error("Failed to find reporting member {0} of game {1} on ladder {2}", new object[] { list2[0].EntityID, list2[0].GameID, list2[0].LadderInstanceID });
                            list2.Clear();
                            continue;
                        }
                        list.Add(list2[0].LadderInstanceID);
                    Label_02EB:
                        list2[0].LadderInstance.LadderDefinition.Resolver.RegisterPartialReport(allParticipants, list2[0]);
                        if (!dictionary2.ContainsKey(list2[0].LadderInstanceID))
                        {
                            dictionary2[list2[0].LadderInstanceID] = "";
                        }
                        if (allParticipants[0].IsAutomatch && allParticipants[1].IsAutomatch)
                        {
                            int num6;
                            (dictionary3 = dictionary2)[num6 = list2[0].LadderInstanceID] = dictionary3[num6] + str3 + allParticipants[0].EntityID.ToString() + "," + allParticipants[1].EntityID.ToString();
                            string str5 = str2;
                            str2 = str5 + str3 + allParticipants[0].EntityID.ToString() + "," + allParticipants[1].EntityID.ToString();
                        }
                        else if (allParticipants[0].Rank < allParticipants[1].Rank)
                        {
                            Dictionary<int, string> dictionary4;
                            int num11;
                            (dictionary4 = dictionary2)[num11 = list2[0].LadderInstanceID] = dictionary4[num11] + str3 + allParticipants[0].EntityID.ToString();
                            str2 = str2 + str3 + allParticipants[0].EntityID.ToString();
                        }
                        else if (allParticipants[1].Rank < allParticipants[0].Rank)
                        {
                            Dictionary<int, string> dictionary5;
                            int num14;
                            (dictionary5 = dictionary2)[num14 = list2[0].LadderInstanceID] = dictionary5[num14] + str3 + allParticipants[1].EntityID.ToString();
                            str2 = str2 + str3 + allParticipants[1].EntityID.ToString();
                        }
                        str = str + str3 + list2[0].GameID.ToString();
                        str3 = ",";
                        num++;
                        continue;
                    Label_0542:
                        if (list2.Count == 2)
                        {
                            Status("Processing report for game {0}", new object[] { list2[0].GameID });
                            if (list.Contains(list2[0].LadderInstanceID))
                            {
                                list2[0].Rank = new QuazalQuery("GetPlayerLadderRankByID", new object[] { list2[0].LadderInstanceID, list2[0].EntityID }).GetInt();
                                list2[1].Rank = new QuazalQuery("GetPlayerLadderRankByID", new object[] { list2[1].LadderInstanceID, list2[1].EntityID }).GetInt();
                            }
                            else
                            {
                                list.Add(list2[0].LadderInstanceID);
                            }
                            LadderGameReport[] reportArray2 = list2.ToArray();
                            list2[0].LadderInstance.LadderDefinition.Resolver.RegisterFullReport(reportArray2);
                            if (!dictionary2.ContainsKey(list2[0].LadderInstanceID))
                            {
                                dictionary2[list2[0].LadderInstanceID] = "";
                            }
                            if (reportArray2[0].IsAutomatch && reportArray2[1].IsAutomatch)
                            {
                                Dictionary<int, string> dictionary6;
                                int num18;
                                (dictionary6 = dictionary2)[num18 = list2[0].LadderInstanceID] = dictionary6[num18] + str3 + reportArray2[0].EntityID.ToString() + "," + reportArray2[1].EntityID.ToString();
                                string str7 = str2;
                                str2 = str7 + str3 + reportArray2[0].EntityID.ToString() + "," + reportArray2[1].EntityID.ToString();
                            }
                            else
                            {
                                int num5;
                                if (reportArray2[0].Rank < reportArray2[1].Rank)
                                {
                                    (dictionary3 = dictionary2)[num5 = list2[0].LadderInstanceID] = dictionary3[num5] + str3 + reportArray2[0].EntityID.ToString();
                                    str2 = str2 + str3 + reportArray2[0].EntityID.ToString();
                                }
                                else if (reportArray2[1].Rank < reportArray2[0].Rank)
                                {
                                    (dictionary3 = dictionary2)[num5 = list2[0].LadderInstanceID] = dictionary3[num5] + str3 + reportArray2[1].EntityID.ToString();
                                    str2 = str2 + str3 + reportArray2[1].EntityID.ToString();
                                }
                            }
                            str = str + str3 + list2[0].GameID.ToString();
                            str3 = ",";
                            num++;
                        }
                        else
                        {
                            Error("Processing game reports", new object[0]);
                            Error("Reached an unknown state while processing ladder reports.", new object[0]);
                        }
                        continue;
                    }
                    catch (Exception exception)
                    {
                        Error("An error occured while processing report for game: {0}", new object[] { list2[0].GameID });
                        Error(exception);
                        continue;
                    }
                }
                if (num > 0)
                {
                    Status("Removing processed games from the system: {0}", new object[] { str });
                    if (!new QuazalQuery("RemoveProcessedLadderReports", new object[] { str }).ExecuteNonQuery())
                    {
                        Error("Failed to remove processed game reports: {0}", new object[] { str });
                    }
                    if (!new QuazalQuery("RemoveProcessedLadderSessions", new object[] { str }).ExecuteNonQuery())
                    {
                        Error("Failed to remove processed game sessions: {0}", new object[] { str });
                    }
                    Status("Resetting degrade timer for entities: {0}", new object[] { str2 });
                    foreach (int num3 in dictionary2.Keys)
                    {
                        if (!new QuazalQuery("SetLastLadderGame", new object[] { DateTime.UtcNow, DateTime.UtcNow, num3, dictionary2[num3] }).ExecuteNonQuery())
                        {
                            Error("Failed to update last game date for entities: {0}", new object[] { dictionary2[num3] });
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                Error("Processing game reports", new object[0]);
                Error(exception2);
            }
        }

        private static void ProcessStaleGameSessions()
        {
            try
            {
                Status("Processing stale game sessions", new object[0]);
                DateTime utcNow = DateTime.UtcNow;
                int @int = ConfigSettings.GetInt("LadderStaleGameHours", 6);
                foreach (LadderGameSession session in new QuazalQuery("GetStaleLadderGameSessions", new object[] { @int, utcNow }).GetObjects<LadderGameSession>().ToArray())
                {
                    Status("Flagging expired game session {0} for deletion and logging an unresolved result for player {1}", new object[] { session.GameID, session.EntityID });
                    string str = string.Format("No players reported any result within {0} hours so this game was not counted toward any records; player rank will remain {1}.", @int, session.StartRank);
                    if (!new QuazalQuery("CreateLadderGameResult", new object[] { session.EntityID, "NULL", session.GameID, session.LadderInstanceID, str, session.StartRank, session.StartRank, DateTime.UtcNow }).ExecuteNonQuery())
                    {
                        Error("Failed to create a unresolved game result for game: {0}, player: {1}", new object[] { session.GameID, session.EntityID });
                    }
                }
                new QuazalQuery("RemoveStaleLadderGameSessions", new object[] { @int, utcNow }).ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Error("Processing stale game sessions", new object[0]);
                Error(exception);
            }
        }

        internal static void Status(string text, params object[] args)
        {
            if (StatusProvider != null)
            {
                StatusProvider.SetStatus(text, args);
            }
        }

        public static bool HasErrors
        {
            get
            {
                return mHasErrors;
            }
        }
    }
}

