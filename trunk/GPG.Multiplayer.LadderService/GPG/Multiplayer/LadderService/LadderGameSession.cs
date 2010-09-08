namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using System;

    public class LadderGameSession : MappedObject
    {
        [FieldMap("entity_id")]
        private int mEntityID;
        [FieldMap("entity_name")]
        private string mEntityName;
        [FieldMap("game_id")]
        private int mGameID;
        [FieldMap("has_reported")]
        private bool mHasReported;
        [FieldMap("is_automatch")]
        private bool mIsAutomatch;
        [FieldMap("ladder_instance_id")]
        private int mLadderInstanceID;
        [FieldMap("start_date")]
        private DateTime mStartDate;
        [FieldMap("rank")]
        private int mStartRank;

        public LadderGameSession(DataRecord record) : base(record)
        {
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

        public bool HasReported
        {
            get
            {
                return this.mHasReported;
            }
        }

        public bool IsAutomatch
        {
            get
            {
                return this.mIsAutomatch;
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

        public DateTime StartDate
        {
            get
            {
                return this.mStartDate;
            }
        }

        public int StartRank
        {
            get
            {
                return this.mStartRank;
            }
        }
    }
}

