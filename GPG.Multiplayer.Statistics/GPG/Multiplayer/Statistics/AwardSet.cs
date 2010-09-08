namespace GPG.Multiplayer.Statistics
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Threading;

    public class AwardSet : MappedObject
    {
        private static object AllAwardSetsMutex = new object();
        private object BaseImageMutex;
        public static Dictionary<int, AwardSet> mAllAwardSets = null;
        [FieldMap("always_show")]
        private bool mAlwaysShow;
        private List<Award> mAwards;
        private Image mBaseImage;
        [FieldMap("category")]
        private int mCategory;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("award_set_id")]
        private int mID;
        [FieldMap("is_local_resource")]
        private bool mIsLocalResource;
        [FieldMap("name")]
        private string mName;
        [FieldMap("sort_order")]
        private int mSortOrder;

        public AwardSet(DataRecord record) : base(record)
        {
            this.mCategory = 1;
            this.mAlwaysShow = true;
            this.BaseImageMutex = new object();
        }

        public static void ClearCachedData()
        {
            mAllAwardSets = null;
        }

        public static Dictionary<int, AwardSet> AllAwardSets
        {
            get
            {
                lock (AllAwardSetsMutex)
                {
                    if (mAllAwardSets == null)
                    {
                        mAllAwardSets = new Dictionary<int, AwardSet>();
                        foreach (AwardSet set in new QuazalQuery("GetAllAwardSets", new object[0]).GetObjects<AwardSet>())
                        {
                            mAllAwardSets.Add(set.ID, set);
                        }
                    }
                    return mAllAwardSets;
                }
            }
        }

        public bool AlwaysShow
        {
            get
            {
                return this.mAlwaysShow;
            }
        }

        public List<Award> Awards
        {
            get
            {
                if (this.mAwards == null)
                {
                    this.mAwards = new List<Award>(3);
                    foreach (Award award in Award.AllAwards.Values)
                    {
                        if (award.AwardSetID == this.ID)
                        {
                            this.mAwards.Add(award);
                        }
                    }
                }
                return this.mAwards;
            }
        }

        public Image BaseImage
        {
            get
            {
                Image mBaseImage;
                object obj2;
                Monitor.Enter(obj2 = this.BaseImageMutex);
                try
                {
                    if (this.mBaseImage == null)
                    {
                        if (this.IsLocalResource)
                        {
                            this.mBaseImage = AwardImages.ResourceManager.GetObject(string.Format("_{0}0", this.Name.Replace(" ", ""))) as Bitmap;
                        }
                        else
                        {
                            using (MemoryStream stream = new MemoryStream(new WebClient().DownloadData(this.BaseImageUrl)))
                            {
                                this.mBaseImage = Image.FromStream(stream);
                            }
                        }
                    }
                    mBaseImage = this.mBaseImage;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    mBaseImage = null;
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
                return mBaseImage;
            }
        }

        public string BaseImageUrl
        {
            get
            {
                return string.Format("{0}/{1}/0.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/awards"), this.Name);
            }
        }

        public AwardCategory Category
        {
            get
            {
                return AwardCategory.AllAwardCategories[this.mCategory];
            }
        }

        public string Description
        {
            get
            {
                return Loc.Get("<LOC>" + this.mDescription);
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool IsLocalResource
        {
            get
            {
                return this.mIsLocalResource;
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

