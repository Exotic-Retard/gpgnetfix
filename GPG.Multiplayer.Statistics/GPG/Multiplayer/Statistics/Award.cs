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

    public class Award : MappedObject
    {
        private static object AllAwardsMutex = new object();
        private object LargeImageMutex;
        [FieldMap("achievement_description")]
        private string mAchievementDescription;
        [FieldMap("achievement_query")]
        private string mAchievementQuery;
        public static Dictionary<int, Award> mAllAwards = null;
        [FieldMap("award_degree")]
        private int mAwardDegree;
        [FieldMap("award_set_id")]
        private int mAwardSetID;
        [FieldMap("award_id")]
        private int mID;
        [FieldMap("is_local_resource")]
        private bool mIsLocalResource;
        private Image mLargeImage;
        private Image mSmallImage;
        private object SmallImageMutex;

        public Award(DataRecord record) : base(record)
        {
            this.SmallImageMutex = new object();
            this.LargeImageMutex = new object();
        }

        public static void ClearCachedData()
        {
            mAllAwards = null;
        }

        public string AchievementDescription
        {
            get
            {
                return Loc.Get("<LOC>" + this.mAchievementDescription);
            }
        }

        public string AchievementQuery
        {
            get
            {
                return this.mAchievementQuery;
            }
        }

        public static Dictionary<int, Award> AllAwards
        {
            get
            {
                lock (AllAwardsMutex)
                {
                    if (mAllAwards == null)
                    {
                        mAllAwards = new Dictionary<int, Award>();
                        foreach (Award award in new QuazalQuery("GetAllAwards", new object[0]).GetObjects<Award>())
                        {
                            mAllAwards.Add(award.ID, award);
                        }
                    }
                    return mAllAwards;
                }
            }
        }

        public int AwardDegree
        {
            get
            {
                return this.mAwardDegree;
            }
        }

        public GPG.Multiplayer.Statistics.AwardSet AwardSet
        {
            get
            {
                return GPG.Multiplayer.Statistics.AwardSet.AllAwardSets[this.AwardSetID];
            }
        }

        public int AwardSetID
        {
            get
            {
                return this.mAwardSetID;
            }
        }

        public string CacheFileName
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Gas Powered Games\GPGnet\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return (path + "CachedAward-" + this.AwardSet.Name + "-" + this.AwardDegree.ToString() + ".png");
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

        public Image LargeImage
        {
            get
            {
                Image mLargeImage;
                object obj2;
                Monitor.Enter(obj2 = this.LargeImageMutex);
                try
                {
                    if (this.mLargeImage == null)
                    {
                        if (this.IsLocalResource)
                        {
                            this.mLargeImage = AwardImages.ResourceManager.GetObject(string.Format("_{0}{1}", this.AwardSet.Name.Replace(" ", ""), this.AwardDegree)) as Bitmap;
                        }
                        else
                        {
                            using (MemoryStream stream = new MemoryStream(new WebClient().DownloadData(this.LargeImageUrl)))
                            {
                                this.mLargeImage = Image.FromStream(stream);
                            }
                        }
                    }
                    mLargeImage = this.mLargeImage;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    mLargeImage = null;
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
                return mLargeImage;
            }
        }

        public string LargeImageUrl
        {
            get
            {
                return string.Format("{0}/{1}/{2}.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/awards"), this.AwardSet.Name, this.AwardDegree);
            }
        }

        public Image SmallImage
        {
            get
            {
                WaitCallback callBack = null;
                Image mSmallImage;
                object obj2;
                Monitor.Enter(obj2 = this.SmallImageMutex);
                try
                {
                    if ((this.mSmallImage != null) && (this.mSmallImage.Width == 10))
                    {
                        this.mSmallImage = null;
                    }
                    if (callBack == null)
                    {
                        callBack = delegate (object o) {
                            try
                            {
                                if (this.mSmallImage == null)
                                {
                                    if (this.IsLocalResource)
                                    {
                                        this.mSmallImage = AwardImages.ResourceManager.GetObject(string.Format("_{0}{1}small", this.AwardSet.Name.Replace(" ", ""), this.AwardDegree)) as Bitmap;
                                    }
                                    else if (System.IO.File.Exists(this.CacheFileName) && (new FileInfo(this.CacheFileName).CreationTime > (DateTime.Now - new TimeSpan(1, 0, 0, 0))))
                                    {
                                        FileStream stream = new FileStream(this.CacheFileName, FileMode.Open);
                                        this.mSmallImage = Image.FromStream(stream) as Bitmap;
                                        stream.Close();
                                    }
                                    else
                                    {
                                        using (MemoryStream stream2 = new MemoryStream(new WebClient().DownloadData(this.SmallImageUrl)))
                                        {
                                            this.mSmallImage = Image.FromStream(stream2);
                                        }
                                        try
                                        {
                                            this.mSmallImage.Save(this.CacheFileName);
                                        }
                                        catch (Exception exception)
                                        {
                                            ErrorLog.WriteLine(exception);
                                        }
                                    }
                                }
                            }
                            catch (Exception exception2)
                            {
                                ErrorLog.WriteLine(exception2);
                            }
                        };
                    }
                    ThreadPool.QueueUserWorkItem(callBack);
                    for (int i = 0; (i < 50) && (this.mSmallImage == null); i++)
                    {
                        Thread.Sleep(10);
                    }
                    if (this.mSmallImage == null)
                    {
                        if (System.IO.File.Exists(this.CacheFileName))
                        {
                            FileStream stream = new FileStream(this.CacheFileName, FileMode.Open);
                            this.mSmallImage = Image.FromStream(stream) as Bitmap;
                            stream.Close();
                        }
                        else
                        {
                            this.mSmallImage = new Bitmap(10, 10);
                        }
                    }
                    mSmallImage = this.mSmallImage;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    mSmallImage = null;
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
                return mSmallImage;
            }
        }

        public string SmallImageUrl
        {
            get
            {
                return string.Format("{0}/{1}/{2}small.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/awards"), this.AwardSet.Name, this.AwardDegree);
            }
        }
    }
}

