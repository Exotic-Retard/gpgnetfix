namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using System;

    public class LadderDegradation : MappedObject
    {
        [FieldMap("degrade_amount")]
        private int mDegradeAmount;
        [FieldMap("degrade_day_interval")]
        private int mDegradeDayInterval;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("ladder_degradation_id")]
        private int mID;
        [FieldMap("ladder_definition_id")]
        private int mLadderDefinitionID;
        [FieldMap("rank_from")]
        private int mRankFrom;
        [FieldMap("rank_to")]
        private int mRankTo;

        public LadderDegradation(DataRecord record) : base(record)
        {
        }

        public int DegradeAmount
        {
            get
            {
                return this.mDegradeAmount;
            }
        }

        public int DegradeDayInterval
        {
            get
            {
                return this.mDegradeDayInterval;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public int LadderDefinitionID
        {
            get
            {
                return this.mLadderDefinitionID;
            }
        }

        public int RankFrom
        {
            get
            {
                return this.mRankFrom;
            }
        }

        public int? RankTo
        {
            get
            {
                if (this.mRankTo <= 0)
                {
                    return null;
                }
                return new int?(this.mRankTo);
            }
        }
    }
}

