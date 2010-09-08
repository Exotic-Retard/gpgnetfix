namespace GPG.Multiplayer.LadderService
{
    using GPG;
    using GPG.DataAccess;
    using System;

    public class LadderGameResult : MappedObject
    {
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("entity_id")]
        private int mEntityID;
        [FieldMap("entity_name")]
        private string mEntityName;
        [FieldMap("game_id")]
        private int mGameID;
        [FieldMap("ladder_game_result_id")]
        private int mID;
        [FieldMap("ladder_instance_id")]
        private int mLadderInstanceID;
        [FieldMap("opponent_id")]
        private int mOpponentID;
        [FieldMap("opponent_name")]
        private string mOpponentName;
        [FieldMap("player_report_date")]
        private DateTime mPlayerReportDate;
        [FieldMap("post_rank")]
        private int mPostRank;
        [FieldMap("pre_rank")]
        private int mPreRank;
        [FieldMap("report_process_date")]
        private DateTime mReportProcessDate;

        public LadderGameResult(DataRecord record) : base(record)
        {
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public int EntityID
        {
            get
            {
                return this.mEntityID;
            }
        }

        public string EntityName
        {
            get
            {
                return this.mEntityName;
            }
        }

        public int GameID
        {
            get
            {
                return this.mGameID;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public int LadderInstanceID
        {
            get
            {
                return this.mLadderInstanceID;
            }
        }

        public int OpponentID
        {
            get
            {
                return this.mOpponentID;
            }
        }

        public string OpponentName
        {
            get
            {
                return this.mOpponentName;
            }
        }

        public DateTime PlayerReportDate
        {
            get
            {
                return DateTime.SpecifyKind(this.mPlayerReportDate, DateTimeKind.Utc).ToLocalTime();
            }
        }

        public string PlayerReportDateFull
        {
            get
            {
                return this.PlayerReportDate.ToString();
            }
        }

        public int PostRank
        {
            get
            {
                return this.mPostRank;
            }
        }

        public int PreRank
        {
            get
            {
                return this.mPreRank;
            }
        }

        public string RankChange
        {
            get
            {
                if (this.PreRank == this.PostRank)
                {
                    return Loc.Get("<LOC>No Change");
                }
                return string.Format(Loc.Get("<LOC>From {0} To {1}"), this.PreRank, this.PostRank);
            }
        }

        public DateTime ReportProcessDate
        {
            get
            {
                return DateTime.SpecifyKind(this.mReportProcessDate, DateTimeKind.Utc).ToLocalTime();
            }
        }

        public string ReportProcessDateFull
        {
            get
            {
                return this.ReportProcessDate.ToString();
            }
        }

        public string ShortDescription
        {
            get
            {
                return string.Format("{0}...", this.Description.Substring(0, 100));
            }
        }
    }
}

