namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using System;

    internal class RatingsReport : MappedObject
    {
        [FieldMap("game_id")]
        private int mGameID;
        [FieldMap("player_status")]
        private string mOutcome;
        [FieldMap("principal_id")]
        private int mPlayerID;
        [FieldMap("player_name")]
        private string mPlayerName;
        private int[] mTeamIDs;
        [FieldMap("team")]
        private string Team;

        public RatingsReport(DataRecord record) : base(record)
        {
        }

        public int GameID
        {
            get
            {
                return this.mGameID;
            }
        }

        public RatingsReportOutcomes Outcome
        {
            get
            {
                return (RatingsReportOutcomes) Enum.Parse(typeof(RatingsReportOutcomes), this.mOutcome, true);
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
        }

        public int[] TeamIDs
        {
            get
            {
                if (this.mTeamIDs == null)
                {
                    string[] strArray = this.Team.Split(" ".ToCharArray());
                    this.mTeamIDs = new int[strArray.Length];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        this.mTeamIDs[i] = int.Parse(strArray[i]);
                    }
                }
                return this.mTeamIDs;
            }
        }
    }
}

