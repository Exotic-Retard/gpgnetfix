namespace GPG.Multiplayer.LadderService
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;

    public class LadderInstance : MappedObject
    {
        private static Dictionary<int, LadderInstance> mAllInstances;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("end_date")]
        private DateTime mEndDate;
        [FieldMap("game_id")]
        private int mGameID;
        [FieldMap("ladder_instance_id")]
        private int mID;
        [FieldMap("is_active")]
        private bool mIsActive;
        [FieldMap("ladder_definition_id")]
        private int mLadderDefinitionID;
        [FieldMap("sort_order")]
        private int mSortOrder;
        [FieldMap("start_date")]
        private DateTime mStartDate;

        public LadderInstance(DataRecord record) : base(record)
        {
        }

        public static Dictionary<int, LadderInstance> AllInstances
        {
            get
            {
                if (mAllInstances == null)
                {
                    mAllInstances = new Dictionary<int, LadderInstance>();
                    foreach (LadderInstance instance in new QuazalQuery("GetActiveLadderInstances", new object[0]).GetObjects<LadderInstance>())
                    {
                        mAllInstances[instance.ID] = instance;
                    }
                }
                return mAllInstances;
            }
        }

        public string AutomatchType
        {
            get
            {
                return string.Format("ladder_{0}", this.ID);
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.mEndDate;
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

        public bool IsActive
        {
            get
            {
                return this.mIsActive;
            }
        }

        public GPG.Multiplayer.LadderService.LadderDefinition LadderDefinition
        {
            get
            {
                if (GPG.Multiplayer.LadderService.LadderDefinition.AllDefinitions.ContainsKey(this.LadderDefinitionID))
                {
                    return GPG.Multiplayer.LadderService.LadderDefinition.AllDefinitions[this.LadderDefinitionID];
                }
                return null;
            }
        }

        public int LadderDefinitionID
        {
            get
            {
                return this.mLadderDefinitionID;
            }
        }

        public int SortOrder
        {
            get
            {
                return this.mSortOrder;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.mStartDate;
            }
        }
    }
}

