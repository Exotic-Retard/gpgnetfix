namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;

    public class LadderGameReport : MappedObject
    {
        private LadderParticipant mEntity;
        [FieldMap("entity_id")]
        private int mEntityID;
        [FieldMap("entity_name")]
        private string mEntityName;
        [FieldMap("game_id")]
        private int mGameID;
        [FieldMap("ladder_game_report_id")]
        private int mID;
        [FieldMap("is_automatch")]
        private bool mIsAutomatch;
        [FieldMap("is_disconnect")]
        private bool mIsDisconnect;
        [FieldMap("is_draw")]
        private bool mIsDraw;
        [FieldMap("is_loss")]
        private bool mIsLoss;
        [FieldMap("is_win")]
        private bool mIsWin;
        [FieldMap("ladder_instance_id")]
        private int mLadderInstanceID;
        [FieldMap("participant_count")]
        private int mParticipantCount;
        [FieldMap("rank")]
        private int mRank;
        [FieldMap("report_date")]
        private DateTime mReportDate;
        [FieldMap("team")]
        private int mTeam;

        public LadderGameReport(DataRecord record) : base(record)
        {
        }

        public LadderParticipant Entity
        {
            get
            {
                if (this.mEntity == null)
                {
                    this.mEntity = new QuazalQuery("GetLadderParticipantByID", new object[] { this.EntityID, this.LadderInstanceID }).GetObject<LadderParticipant>();
                }
                return this.mEntity;
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

        public bool IsAutomatch
        {
            get
            {
                return this.mIsAutomatch;
            }
        }

        public bool IsDisconnect
        {
            get
            {
                return this.mIsDisconnect;
            }
        }

        public bool IsDraw
        {
            get
            {
                return this.mIsDraw;
            }
        }

        public bool IsLoss
        {
            get
            {
                return this.mIsLoss;
            }
        }

        public bool IsWin
        {
            get
            {
                return this.mIsWin;
            }
        }

        public GPG.Multiplayer.LadderService.LadderInstance LadderInstance
        {
            get
            {
                if (GPG.Multiplayer.LadderService.LadderInstance.AllInstances.ContainsKey(this.LadderInstanceID))
                {
                    return GPG.Multiplayer.LadderService.LadderInstance.AllInstances[this.LadderInstanceID];
                }
                return null;
            }
        }

        public int LadderInstanceID
        {
            get
            {
                return this.mLadderInstanceID;
            }
        }

        public int ParticipantCount
        {
            get
            {
                return this.mParticipantCount;
            }
        }

        public int Rank
        {
            get
            {
                return this.mRank;
            }
            internal set
            {
                this.mRank = value;
            }
        }

        public DateTime ReportDate
        {
            get
            {
                return this.mReportDate;
            }
        }

        public int Team
        {
            get
            {
                return this.mTeam;
            }
        }
    }
}

