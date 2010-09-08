namespace GPG.Multiplayer.LadderService
{
    using GPG.Multiplayer.Quazal;
    using System;

    public class HardcoreLadderReportResolver : ReportResolver
    {
        protected override void AdjustParticipantRank(LadderGameReport winnerReport, LadderGameReport loserReport)
        {
            GPG.Multiplayer.LadderService.LadderService.Status("Resolving game with HardcoreLadderReportResolver", new object[0]);
            GPG.Multiplayer.LadderService.LadderService.Status("Result of game {0} is entity: {1}({2}) defeated entity: {3}({4})", new object[] { winnerReport.GameID, winnerReport.EntityName, winnerReport.Rank, loserReport.EntityName, loserReport.Rank });
            if (!new QuazalQuery("IncrementLadderWin", new object[] { winnerReport.EntityID, winnerReport.LadderInstanceID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment win for entity: {0}({1}) on ladder: {2}", new object[] { winnerReport.EntityName, winnerReport.EntityID, winnerReport.LadderInstanceID });
            }
            if (!new QuazalQuery("IncrementLadderLoss", new object[] { loserReport.EntityID, loserReport.LadderInstanceID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("Failed to increment loss for entity: {0}({1}) on ladder: {2}", new object[] { loserReport.EntityName, loserReport.EntityID, loserReport.LadderInstanceID });
            }
            int @int = new QuazalQuery("GetLastLadderRank", new object[] { loserReport.LadderInstanceID }).GetInt();
            if (!new QuazalQuery("AdjustLadderParticipantTemp", new object[] { loserReport.LadderInstanceID, loserReport.EntityID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 1 while adjusting rank to end of ladder for loser {0} on ladder {1}", new object[] { loserReport.EntityID, loserReport.LadderInstanceID });
            }
            else if (!new QuazalQuery("DecrementLadderAbove", new object[] { loserReport.LadderInstanceID, loserReport.Rank }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 2 while adjusting rank to end of ladder for loser {0} on ladder {1}", new object[] { loserReport.EntityID, loserReport.LadderInstanceID });
            }
            else if (!new QuazalQuery("AdjustLadderParticipant", new object[] { @int, loserReport.LadderInstanceID, loserReport.EntityID }).ExecuteNonQuery())
            {
                GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 3 while adjusting rank to end of ladder for loser {0} on ladder {1}", new object[] { loserReport.EntityID, loserReport.LadderInstanceID });
            }
            else
            {
                int num2 = new QuazalQuery("FindLadderStreakRank", new object[] { winnerReport.LadderInstanceID, winnerReport.LadderInstanceID, winnerReport.EntityID }).GetInt();
                if (num2 <= 0)
                {
                    num2 = 1;
                }
                else
                {
                    num2++;
                }
                if (!new QuazalQuery("AdjustLadderParticipantTemp", new object[] { winnerReport.LadderInstanceID, winnerReport.EntityID }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 4 while adjusting rank to {0} for winner {1} on ladder {2}", new object[] { num2, winnerReport.EntityID, winnerReport.LadderInstanceID });
                }
                else if (!new QuazalQuery("IncrementLadderBetween", new object[] { winnerReport.LadderInstanceID, num2, winnerReport.Rank }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 5 while adjusting rank to {0} for winner {1} on ladder {2}", new object[] { num2, winnerReport.EntityID, winnerReport.LadderInstanceID });
                }
                else if (!new QuazalQuery("AdjustLadderParticipant", new object[] { num2, winnerReport.LadderInstanceID, winnerReport.EntityID }).ExecuteNonQuery())
                {
                    GPG.Multiplayer.LadderService.LadderService.Error("An error occured on step 6 while adjusting rank to {0} for winner {1} on ladder {2}", new object[] { num2, winnerReport.EntityID, winnerReport.LadderInstanceID });
                }
                else
                {
                    loserReport.Rank = @int;
                    winnerReport.Rank = num2;
                }
            }
        }
    }
}

