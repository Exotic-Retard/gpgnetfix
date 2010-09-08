namespace GPG.Multiplayer.LadderService
{
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;

    public class ReportResolver
    {
        private Dictionary<int, LadderGameOutcomes[]> Reports = new Dictionary<int, LadderGameOutcomes[]>();

        public virtual void AddParticipant(int entityId, string entityName, int ladderInstance, int rank)
        {
            GPG.Multiplayer.LadderService.LadderService.Status("Adding participant ID: {0}, Name: {1} to ladder: {2}", new object[] { entityId, entityName, ladderInstance });
            if (new QuazalQuery("JoinLadder", new object[] { entityId, entityName, ladderInstance, rank, rank, DateTime.UtcNow, DateTime.UtcNow }).ExecuteNonQuery())
            {
                string str = string.Format("{0} has joined ladder: {1}", entityName, LadderInstance.AllInstances[ladderInstance].Description);
                if (!new QuazalQuery("CreateLadderGameResult", new object[] { entityId, "NULL", "NULL", ladderInstance, str, 0, rank, DateTime.UtcNow }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("Failed to record ladder addition audit record for player: {0}(rank {1})", new object[] { entityName, rank });
                }
                new QuazalQuery("CreateLadderReputation", new object[] { entityId }).ExecuteNonQuery();
            }
            else
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to add participant ID: {0}, Name: {1} to ladder: {2}", new object[] { entityId, entityName, ladderInstance });
            }
        }

        protected virtual void AdjustParticipantRank(LadderGameReport winnerReport, LadderGameReport loserReport)
        {
            GPG.Multiplayer.LadderService.LadderService.Status("Result of game {0} is entity: {1}({2}) defeated entity: {3}({4})", new object[] { winnerReport.GameID, winnerReport.EntityName, winnerReport.Rank, loserReport.EntityName, loserReport.Rank });
            if (!new QuazalQuery("IncrementLadderWin", new object[] { winnerReport.EntityID, winnerReport.LadderInstanceID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment win for entity: {0}({1}) on ladder: {2}", new object[] { winnerReport.EntityName, winnerReport.EntityID, winnerReport.LadderInstanceID });
            }
            if (!new QuazalQuery("IncrementLadderLoss", new object[] { loserReport.EntityID, loserReport.LadderInstanceID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment loss for entity: {0}({1}) on ladder: {2}", new object[] { loserReport.EntityName, loserReport.EntityID, loserReport.LadderInstanceID });
            }
            if (winnerReport.Rank > loserReport.Rank)
            {
                int num = winnerReport.Rank - loserReport.Rank;
                if (num > 1)
                {
                    if ((num % 2) > 0)
                    {
                        num = (num / 2) + 1;
                    }
                    else
                    {
                        num /= 2;
                    }
                }
                if (num <= 0)
                {
                    num = 1;
                }
                int num2 = winnerReport.Entity.Rank - num;
                if (!new QuazalQuery("AdjustLadderParticipantTemp", new object[] { winnerReport.LadderInstanceID, winnerReport.EntityID }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 1 while adjusting rank to {0} for winner {1} on ladder {2}", new object[] { num2, winnerReport.EntityID, winnerReport.LadderInstanceID });
                }
                else if (!new QuazalQuery("IncrementLadderBetween", new object[] { winnerReport.LadderInstanceID, num2, winnerReport.Rank }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 2 while adjusting rank to {0} for winner {1} on ladder {2}", new object[] { num2, winnerReport.EntityID, winnerReport.LadderInstanceID });
                }
                else if (!new QuazalQuery("AdjustLadderParticipant", new object[] { num2, winnerReport.LadderInstanceID, winnerReport.EntityID }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 3 while adjusting rank to {0} for winner {1} on ladder {2}", new object[] { num2, winnerReport.EntityID, winnerReport.LadderInstanceID });
                }
                else
                {
                    winnerReport.Rank = num2;
                    if (num2 == loserReport.Rank)
                    {
                        loserReport.Rank++;
                    }
                }
            }
        }

        public virtual void DegradeParticipant(LadderParticipant participant, LadderDegradation degrade, int lastRank)
        {
            LadderDefinition ladderDefinition = participant.LadderInstance.LadderDefinition;
            int num = participant.Rank + degrade.DegradeAmount;
            if (num > lastRank)
            {
                num = lastRank;
            }
            else if (num <= 0)
            {
                GPG.Multiplayer.LadderService.LadderService.Error("An error occured determining degraded rank for participant {0} on ladder {1}", new object[] { participant.EntityID, participant.LadderInstanceID });
                return;
            }
            if (!new QuazalQuery("AdjustLadderParticipantTemp", new object[] { participant.LadderInstanceID, participant.EntityID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 1 while degrading rank to {0} for participant {1} on ladder {2}", new object[] { num, participant.EntityID, participant.LadderInstanceID });
            }
            else if (!new QuazalQuery("DecrementLadderBetween", new object[] { participant.LadderInstanceID, participant.Rank, num }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 2 while degrading rank to {0} for participant {1} on ladder {2}", new object[] { num, participant.EntityID, participant.LadderInstanceID });
            }
            else if (!new QuazalQuery("AdjustLadderParticipant", new object[] { num, participant.LadderInstanceID, participant.EntityID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 3 while degrading rank to {0} for participant {1} on ladder {2}", new object[] { num, participant.EntityID, participant.LadderInstanceID });
            }
            else
            {
                string str = string.Format("{0}(rank {1}) has sat idle on ladder: {2} for {3} days or longer. Per ladder rules, {0} is being degraded on the ladder by {4} positions moving from rank {1} to rank {5}.", new object[] { participant.EntityName, participant.Rank, participant.LadderInstance.Description, degrade.DegradeDayInterval, degrade.DegradeAmount, num });
                if (!new QuazalQuery("CreateLadderGameResult", new object[] { participant.EntityID, "NULL", "NULL", participant.LadderInstanceID, str, participant.Rank, num, DateTime.UtcNow }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("Failed to record degrade audit record for player: {0}(rank {1})", new object[] { participant.EntityName, participant.Rank });
                }
            }
        }

        protected virtual void OnDraw(LadderGameReport entity1, LadderGameReport entity2)
        {
            GPG.Multiplayer.LadderService.LadderService.Status("Result of game {0} is entity: {1}({2}) drew against entity: {3}({4}), no rating was affected.", new object[] { entity1.GameID, entity1.EntityID, entity1.EntityName, entity2.EntityID, entity2.EntityName });
            if (!new QuazalQuery("IncrementLadderDraw", new object[] { entity1.EntityID, entity1.LadderInstanceID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment win for entity: {0}({1}) on ladder: {2}", new object[] { entity1.EntityName, entity1.EntityID, entity1.LadderInstanceID });
            }
            if (!new QuazalQuery("IncrementLadderDraw", new object[] { entity2.EntityID, entity2.LadderInstanceID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment loss for entity: {0}({1}) on ladder: {2}", new object[] { entity2.EntityName, entity2.EntityID, entity2.LadderInstanceID });
            }
        }

        protected virtual void RecordResult(LadderGameReport entity, LadderGameReport opponent, int gameId, int ladderInstanceId, string reportDescription, int preRank, int opponentPreRank, DateTime reportDate)
        {
            string str;
            int rank = entity.Rank;
            LadderInstance instance = LadderInstance.AllInstances[ladderInstanceId];
            if (preRank == rank)
            {
                str = "Rank was not affected.";
            }
            else
            {
                str = string.Format("Rank changed from {0} to {1}.", preRank, rank);
            }
            string str2 = string.Format("Match between {0}(rank {1}) and {2}(rank {3}) on ladder: {4}.", new object[] { entity.EntityName, preRank, opponent.EntityName, opponentPreRank, instance.Description });
            string str3 = string.Format("{0} {1} {2}", str2, reportDescription, str);
            if (!new QuazalQuery("CreateLadderGameResult", new object[] { entity.EntityID, opponent.EntityID, gameId, ladderInstanceId, str3, preRank, rank, reportDate }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to create a unresolved game result for game: {0}, player: {1}", new object[] { gameId, gameId });
            }
        }

        public virtual void RegisterFullReport(LadderGameReport[] allParticipants)
        {
            if ((allParticipants == null) || (allParticipants.Length < 2))
            {
                throw new Exception("The parameter allParticipants must contain all original participants in a game according to the participant_count field in the database.");
            }
            LadderGameReport winnerReport = allParticipants[0];
            LadderGameReport loserReport = allParticipants[1];
            GPG.Multiplayer.LadderService.LadderService.Status("Processing full report for game {0} between entities {1} and {2}", new object[] { winnerReport.GameID, winnerReport.EntityID, loserReport.EntityID });
            if (winnerReport.IsWin && loserReport.IsLoss)
            {
                int rank = winnerReport.Rank;
                int opponentPreRank = loserReport.Rank;
                this.AdjustParticipantRank(winnerReport, loserReport);
                string reportDescription = string.Format("{0}({1}) reported a win, {2}({3}) reported a loss. Result of game is {0} wins.", new object[] { winnerReport.EntityName, rank, loserReport.EntityName, opponentPreRank });
                this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, reportDescription, rank, opponentPreRank, DateTime.UtcNow);
                reportDescription = string.Format("{0}({1}) reported a loss, {2}({3}) reported a win. Result of game is {2} wins.", new object[] { loserReport.EntityName, opponentPreRank, winnerReport.EntityName, rank });
                this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, reportDescription, opponentPreRank, rank, DateTime.UtcNow);
            }
            else if (winnerReport.IsLoss && loserReport.IsWin)
            {
                int preRank = winnerReport.Rank;
                int num4 = loserReport.Rank;
                this.AdjustParticipantRank(loserReport, winnerReport);
                string str2 = string.Format("{0}({1}) reported a loss, {2}({3}) reported a win. Result of game is {2} wins.", new object[] { winnerReport.EntityName, preRank, loserReport.EntityName, num4 });
                this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str2, preRank, num4, DateTime.UtcNow);
                str2 = string.Format("{0}({1}) reported a win, {2}({3}) reported a loss. Result of game is {0} wins.", new object[] { loserReport.EntityName, num4, winnerReport.EntityName, preRank });
                this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str2, num4, preRank, DateTime.UtcNow);
            }
            else if (winnerReport.IsDraw && loserReport.IsDraw)
            {
                this.OnDraw(winnerReport, loserReport);
                string str3 = string.Format("{0}({1}) reported a draw, {2}({3}) reported a draw. Result of game is draw.", new object[] { winnerReport.EntityName, winnerReport.Rank, loserReport.EntityName, loserReport.Rank });
                this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str3, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                str3 = string.Format("{0}({1}) reported a draw, {2}({3}) reported a draw. Result of game is draw.", new object[] { loserReport.EntityName, loserReport.Rank, winnerReport.EntityName, winnerReport.Rank });
                this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str3, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
            }
            else
            {
                if (!new QuazalQuery("IncrementLadderConflictReportCount", new object[] { winnerReport.EntityID, winnerReport.LadderInstanceID }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment conflict report count for entity: {0}({1}) on ladder: {2}", new object[] { winnerReport.EntityName, winnerReport.EntityID, winnerReport.LadderInstanceID });
                }
                if (!new QuazalQuery("IncrementLadderConflictReportCount", new object[] { loserReport.EntityID, loserReport.LadderInstanceID }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment conflict report count for entity: {0}({1}) on ladder: {2}", new object[] { loserReport.EntityName, loserReport.EntityID, loserReport.LadderInstanceID });
                }
                if (winnerReport.IsDraw && loserReport.IsLoss)
                {
                    this.OnDraw(winnerReport, loserReport);
                    string str4 = string.Format("{0}({1}) reported a draw, {2}({3}) reported a loss. Result of game is draw.", new object[] { winnerReport.EntityName, winnerReport.Rank, loserReport.EntityName, loserReport.Rank });
                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str4, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                    str4 = string.Format("{0}({1}) reported a loss, {2}({3}) reported a draw. Result of game is draw.", new object[] { loserReport.EntityName, loserReport.Rank, winnerReport.EntityName, winnerReport.Rank });
                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str4, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
                }
                else if (winnerReport.IsLoss && loserReport.IsDraw)
                {
                    this.OnDraw(winnerReport, loserReport);
                    string str5 = string.Format("{0}({1}) reported a loss, {2}({3}) reported a draw. Result of game is draw.", new object[] { winnerReport.EntityName, winnerReport.Rank, loserReport.EntityName, loserReport.Rank });
                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str5, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                    str5 = string.Format("{0}({1}) reported a draw, {2}({3}) reported a loss. Result of game is draw.", new object[] { loserReport.EntityName, loserReport.Rank, winnerReport.EntityName, winnerReport.Rank });
                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str5, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
                }
                else
                {
                    RatingsReport[] reportArray = new QuazalQuery("GetRatingsReportsByGameID", new object[] { winnerReport.GameID }).GetObjects<RatingsReport>().ToArray();
                    LadderDefinition ladderDefinition = winnerReport.LadderInstance.LadderDefinition;
                    string str6 = "win";
                    if (winnerReport.IsLoss)
                    {
                        str6 = "loss";
                    }
                    else if (winnerReport.IsDraw)
                    {
                        str6 = "draw";
                    }
                    string str7 = "win";
                    if (loserReport.IsLoss)
                    {
                        str7 = "loss";
                    }
                    else if (loserReport.IsDraw)
                    {
                        str7 = "draw";
                    }
                    if (ladderDefinition.IsIndiviual)
                    {
                        if ((reportArray != null) && (reportArray.Length == 2))
                        {
                            if ((reportArray[0].Outcome == RatingsReportOutcomes.Victory) && (reportArray[1].Outcome == RatingsReportOutcomes.Defeat))
                            {
                                if (winnerReport.EntityID == reportArray[0].PlayerID)
                                {
                                    int num5 = winnerReport.Rank;
                                    int num6 = loserReport.Rank;
                                    this.AdjustParticipantRank(winnerReport, loserReport);
                                    string str8 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {0} wins.", new object[] { winnerReport.EntityName, num5, str6, loserReport.EntityName, num6, str7 });
                                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str8, num5, num6, DateTime.UtcNow);
                                    str8 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {3} wins.", new object[] { loserReport.EntityName, num6, str7, winnerReport.EntityName, num5, str6 });
                                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str8, num6, num5, DateTime.UtcNow);
                                }
                                else if (loserReport.EntityID == reportArray[0].PlayerID)
                                {
                                    int num7 = winnerReport.Rank;
                                    int num8 = loserReport.Rank;
                                    this.AdjustParticipantRank(loserReport, winnerReport);
                                    string str9 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {3} wins.", new object[] { winnerReport.EntityName, num7, str6, loserReport.EntityName, num8, str7 });
                                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str9, num7, num8, DateTime.UtcNow);
                                    str9 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {0} wins.", new object[] { loserReport.EntityName, num8, str7, winnerReport.EntityName, num7, str6 });
                                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str9, num8, num7, DateTime.UtcNow);
                                }
                                else
                                {
                                    GPG.Multiplayer.LadderService.LadderService.Error("Ladder reports for game {0} conflicted, but the ratings reports conflicted as well resulting in no resolution.", new object[0]);
                                    string str10 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { winnerReport.EntityName, winnerReport.Rank, str6, loserReport.EntityName, loserReport.Rank, str7 });
                                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str10, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                                    str10 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { loserReport.EntityName, loserReport.Rank, str7, winnerReport.EntityName, winnerReport.Rank, str6 });
                                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str10, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
                                }
                            }
                            else if ((reportArray[0].Outcome == RatingsReportOutcomes.Defeat) && (reportArray[1].Outcome == RatingsReportOutcomes.Victory))
                            {
                                if (winnerReport.EntityID == reportArray[1].PlayerID)
                                {
                                    int num9 = winnerReport.Rank;
                                    int num10 = loserReport.Rank;
                                    this.AdjustParticipantRank(winnerReport, loserReport);
                                    string str11 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {0} wins.", new object[] { winnerReport.EntityName, num9, str6, loserReport.EntityName, num10, str7 });
                                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str11, num9, num10, DateTime.UtcNow);
                                    str11 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {3} wins.", new object[] { loserReport.EntityName, num10, str7, winnerReport.EntityName, num9, str6 });
                                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str11, num10, num9, DateTime.UtcNow);
                                }
                                else if (loserReport.EntityID == reportArray[1].PlayerID)
                                {
                                    int num11 = winnerReport.Rank;
                                    int num12 = loserReport.Rank;
                                    this.AdjustParticipantRank(loserReport, winnerReport);
                                    string str12 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {3} wins.", new object[] { winnerReport.EntityName, num11, str6, loserReport.EntityName, num12, str7 });
                                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str12, num11, num12, DateTime.UtcNow);
                                    str12 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is {0} wins.", new object[] { loserReport.EntityName, num12, str7, winnerReport.EntityName, num11, str6 });
                                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str12, num12, num11, DateTime.UtcNow);
                                }
                                else
                                {
                                    GPG.Multiplayer.LadderService.LadderService.Error("Ladder reports for game {0} conflicted, but the ratings reports conflicted as well resulting in no resolution.", new object[] { winnerReport.GameID });
                                    string str13 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { winnerReport.EntityName, winnerReport.Rank, str6, loserReport.EntityName, loserReport.Rank, str7 });
                                    this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str13, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                                    str13 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { loserReport.EntityName, loserReport.EntityID, str7, winnerReport.EntityName, winnerReport.EntityID, str6 });
                                    this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str13, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
                                }
                            }
                            else if ((reportArray[0].Outcome == RatingsReportOutcomes.Draw) && (reportArray[1].Outcome == RatingsReportOutcomes.Draw))
                            {
                                this.OnDraw(winnerReport, loserReport);
                                string str14 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is a draw.", new object[] { winnerReport.EntityName, winnerReport.Rank, str6, loserReport.EntityName, loserReport.Rank, str7 });
                                this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str14, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                                str14 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Result of game is a draw.", new object[] { loserReport.EntityName, loserReport.Rank, str7, winnerReport.EntityName, winnerReport.Rank, str6 });
                                this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str14, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
                            }
                            else
                            {
                                GPG.Multiplayer.LadderService.LadderService.Error("Ladder reports for game {0} conflicted, but the ratings reports conflicted as well resulting in no resolution.", new object[] { winnerReport.GameID });
                                string str15 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { winnerReport.EntityName, winnerReport.Rank, str6, loserReport.EntityName, loserReport.Rank, str7 });
                                this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str15, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                                str15 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { loserReport.EntityName, loserReport.Rank, str7, winnerReport.EntityName, winnerReport.Rank, str6 });
                                this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str15, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
                            }
                        }
                        else
                        {
                            GPG.Multiplayer.LadderService.LadderService.Error("Ladder reports for game {0} conflicted, but the ratings reports conflicted as well resulting in no resolution.", new object[] { winnerReport.GameID });
                            string str16 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { winnerReport.EntityName, winnerReport.Rank, str6, loserReport.EntityName, loserReport.Rank, str7 });
                            this.RecordResult(winnerReport, loserReport, winnerReport.GameID, winnerReport.LadderInstanceID, str16, winnerReport.Rank, loserReport.Rank, DateTime.UtcNow);
                            str16 = string.Format("{0}({1}) reported a {2}, {3}({4}) reported a {5}. Conflict in report, using ratings report as fallback. Not enough data was present in ratings report to determine an outcome. Result of game is inconclusive.", new object[] { loserReport.EntityName, loserReport.Rank, str7, winnerReport.EntityName, winnerReport.Rank, str6 });
                            this.RecordResult(loserReport, winnerReport, loserReport.GameID, loserReport.LadderInstanceID, str16, loserReport.Rank, winnerReport.Rank, DateTime.UtcNow);
                        }
                    }
                    else if (!ladderDefinition.IsClan && !ladderDefinition.IsTeam)
                    {
                        throw new Exception("Ladder is not defined as type: individual, clan or team. It must fall under one of these categories for reporting conflict resolution.");
                    }
                }
            }
        }

        public virtual void RegisterPartialReport(LadderGameReport[] allParticipants, LadderGameReport partialReport)
        {
            LadderGameReport report;
            if ((allParticipants == null) || (allParticipants.Length < 2))
            {
                throw new Exception("The parameter allParticipants must contain all original participants in a game according to the participant_count field in the database.");
            }
            if (allParticipants[0].EntityID == partialReport.EntityID)
            {
                report = allParticipants[1];
            }
            else
            {
                report = allParticipants[0];
            }
            if (!new QuazalQuery("IncrementLadderNonReportCount", new object[] { report.EntityID, report.LadderInstanceID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment non-report count for entity: {0}({1}) on ladder: {2}", new object[] { report.EntityName, report.EntityID, report.LadderInstanceID });
            }
            if (partialReport.IsWin)
            {
                int rank = partialReport.Rank;
                int opponentPreRank = report.Rank;
                this.AdjustParticipantRank(partialReport, report);
                string reportDescription = string.Format("{0}({1}) reported a win, {2}({3}) did not report a result. Result of game is {0} wins.", new object[] { partialReport.EntityName, rank, report.EntityName, opponentPreRank });
                this.RecordResult(partialReport, report, partialReport.GameID, partialReport.LadderInstanceID, reportDescription, rank, opponentPreRank, DateTime.UtcNow);
                reportDescription = string.Format("{0}({1}) did not report a result, {2}({3}) reported a win. Result of game is {2} wins.", new object[] { report.EntityName, opponentPreRank, partialReport.EntityName, rank });
                this.RecordResult(report, partialReport, report.GameID, report.LadderInstanceID, reportDescription, opponentPreRank, rank, DateTime.UtcNow);
            }
            else if (partialReport.IsLoss)
            {
                int preRank = partialReport.Rank;
                int num4 = report.Rank;
                this.AdjustParticipantRank(report, partialReport);
                string str2 = string.Format("{0}({1}) reported a loss, {2}({3}) did not report a result. Result of game is {2} wins.", new object[] { partialReport.EntityName, preRank, report.EntityName, num4 });
                this.RecordResult(partialReport, report, partialReport.GameID, partialReport.LadderInstanceID, str2, preRank, num4, DateTime.UtcNow);
                str2 = string.Format("{0}({1}) did not report a result, {2}({3}) reported a loss. Result of game is {0} wins.", new object[] { report.EntityName, num4, partialReport.EntityName, preRank });
                this.RecordResult(report, partialReport, report.GameID, report.LadderInstanceID, str2, num4, preRank, DateTime.UtcNow);
            }
            else if (partialReport.IsDraw)
            {
                this.OnDraw(partialReport, report);
                string str3 = string.Format("{0}({1}) reported a draw, {2}({3}) did not report a result. Result of game is draw.", new object[] { partialReport.EntityName, partialReport.Rank, report.EntityName, report.Rank });
                this.RecordResult(partialReport, report, partialReport.GameID, partialReport.LadderInstanceID, str3, partialReport.Rank, report.Rank, DateTime.UtcNow);
                str3 = string.Format("{0}({1}) did not report a result, {2}({3}) reported a draw. Result of game is draw.", new object[] { report.EntityName, report.Rank, partialReport.EntityName, partialReport.Rank });
                this.RecordResult(report, partialReport, report.GameID, report.LadderInstanceID, str3, report.Rank, partialReport.Rank, DateTime.UtcNow);
            }
            else
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Unable to determine the result for game {0}", new object[] { partialReport.GameID });
            }
        }
    }
}

