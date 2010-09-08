namespace GPG.Multiplayer.Statistics
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;

    public class AwardCategory : MappedObject
    {
        public static Dictionary<int, AwardCategory> mAllAwardCategories;
        [FieldMap("award_category_id")]
        private int mID;
        [FieldMap("name")]
        private string mName;
        [FieldMap("sort_order")]
        private int mSortOrder = -1;

        public AwardCategory(DataRecord record) : base(record)
        {
        }

        public static void ClearCachedData()
        {
            mAllAwardCategories = null;
        }

        public static Dictionary<int, AwardCategory> AllAwardCategories
        {
            get
            {
                if (mAllAwardCategories == null)
                {
                    mAllAwardCategories = new Dictionary<int, AwardCategory>();
                    foreach (AwardCategory category in new QuazalQuery("GetAllAwardCategories", new object[0]).GetObjects<AwardCategory>())
                    {
                        mAllAwardCategories.Add(category.ID, category);
                    }
                }
                return mAllAwardCategories;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public int SortOrder
        {
            get
            {
                return this.mSortOrder;
            }
        }
    }
}

