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
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public class Avatar : MappedObject
    {
        private static object AllAvatarsMutex = new object();
        private object ImageMutex = new object();
        [FieldMap("achievement_query")]
        private string mAchievementQuery;
        private static Dictionary<int, Avatar> mAllAvatars = null;
        private static Avatar mDefault = null;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("avatar_id")]
        private int mID;
        private Bitmap mImage;
        [FieldMap("is_local_resource")]
        private bool mIsLocalResource = true;
        [FieldMap("resource_key")]
        private string mResourceKey;

        private Avatar()
        {
        }

        public Avatar(DataRecord record) : base(record)
        {
        }

        public static void CalculateAvatars()
        {
            CalculateAvatars(null);
        }

        public static void CalculateAvatars(IStatusProvider statusProvider)
        {
            DateTime now = DateTime.Now;
            StringBuilder sql = new StringBuilder(100);
            ClearCachedData();
            try
            {
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Clearing old temp data if it exists", new object[0]);
                }
                Thread.Sleep(500);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_avatar;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_tournament_results;", new object[0]);
                sql.AppendFormat("CREATE TABLE temp_player_ids (", new object[0]);
                sql.AppendFormat("player_id INTEGER UNSIGNED NOT NULL,", new object[0]);
                sql.AppendFormat("PRIMARY KEY(player_id)", new object[0]);
                sql.AppendFormat(") SELECT principal AS player_id FROM principal_info;", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("CREATE TABLE temp_tournament_results ", new object[0]);
                sql.AppendFormat("SELECT tournaments.tournament_id, new.principal_id, ", new object[0]);
                sql.AppendFormat("(SELECT count(*) FROM tournament_round old WHERE old.tournament_id = tournaments.tournament_id AND ", new object[0]);
                sql.AppendFormat("old.round = tournaments.round AND ", new object[0]);
                sql.AppendFormat("( (old.wins + (0.5 * old.draws)) >  (new.wins + (0.5 * new.draws))  OR ", new object[0]);
                sql.AppendFormat("( (old.wins + (0.5 * old.draws)) =  (new.wins + (0.5 * new.draws)) AND (old.seed < new.seed) ) ) ) + 1 as finish_pos ", new object[0]);
                sql.AppendFormat("FROM tournaments, tournament_round new ", new object[0]);
                sql.AppendFormat("WHERE ", new object[0]);
                sql.AppendFormat("tournaments.tournament_id = new.tournament_id ", new object[0]);
                sql.AppendFormat("AND ", new object[0]);
                sql.AppendFormat("tournaments.round = new.round ", new object[0]);
                sql.AppendFormat("ORDER BY tournaments.tournament_id DESC, (wins + (0.5 * draws)) DESC, seed ASC;", new object[0]);
                sql.AppendFormat("ALTER TABLE temp_tournament_results ADD index temp_tournament_results_ix1 (principal_id, finish_pos);", new object[0]);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Creating temp schema for avatars", new object[0]);
                }
                Thread.Sleep(500);
                sql.AppendFormat("CREATE TABLE temp_player_avatar (", new object[0]);
                sql.AppendFormat("player_avatar_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,", new object[0]);
                sql.AppendFormat("player_id INTEGER UNSIGNED NULL,", new object[0]);
                sql.AppendFormat("avatar_id INTEGER UNSIGNED NULL,", new object[0]);
                sql.AppendFormat("manual_assignment BOOL NULL DEFAULT 0,", new object[0]);
                sql.AppendFormat("PRIMARY KEY(player_avatar_id),", new object[0]);
                sql.AppendFormat("UNIQUE INDEX temp_player_avatar_index3199(player_id, avatar_id),", new object[0]);
                sql.AppendFormat("INDEX temp_player_avatar_index3203(avatar_id),", new object[0]);
                sql.AppendFormat("INDEX temp_player_avatar_index3208(manual_assignment)", new object[0]);
                sql.AppendFormat(") SELECT * FROM player_avatar WHERE manual_assignment = 1;", new object[0]);
                ExecuteQuery(sql);
                foreach (Avatar avatar in AllAvatars.Values)
                {
                    if (((avatar.AchievementQuery != null) && (avatar.AchievementQuery.Length >= 1)) && (avatar.AchievementQuery != "(null)"))
                    {
                        DateTime time2 = DateTime.Now;
                        if (statusProvider != null)
                        {
                            statusProvider.SetStatus("Calculating achievement status for avatar: {0}", new object[] { avatar.Description });
                        }
                        sql.AppendFormat("INSERT INTO temp_player_avatar (player_id, avatar_id, manual_assignment) (SELECT player_id, {0}, 0 FROM temp_player_ids WHERE ({1}) > 0);", avatar.ID, avatar.AchievementQuery);
                        ExecuteQuery(sql);
                        TimeSpan span = (TimeSpan) (DateTime.Now - time2);
                        if (statusProvider != null)
                        {
                            statusProvider.SetStatus("Finished calculating avatar: {0} in {1} seconds", new object[] { avatar.Description, span.TotalSeconds });
                        }
                        Thread.Sleep(500);
                    }
                }
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Removing unachieved display avatars", new object[0]);
                }
                sql.AppendFormat("UPDATE player_display_awards SET avatar = 0 WHERE ", new object[0]);
                sql.AppendFormat("(SELECT COUNT(*) FROM temp_player_avatar WHERE ", new object[0]);
                sql.AppendFormat("temp_player_avatar.player_id = player_display_awards.player_id AND ", new object[0]);
                sql.AppendFormat("temp_player_avatar.avatar_id = player_display_awards.avatar) = 0;", new object[0]);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Moving temp avatar data to live data", new object[0]);
                }
                Thread.Sleep(0x3e8);
                sql.AppendFormat("RENAME TABLE player_avatar TO old_player_avatar, temp_player_avatar TO player_avatar;", new object[0]);
                ExecuteQuery(sql);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine("Failed to calculate player awards due to the following exception:", new object[0]);
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Deleting old data", new object[0]);
                }
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_tournament_results;", new object[0]);
                ExecuteQuery(sql);
                TimeSpan span2 = (TimeSpan) (DateTime.Now - now);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Awards processing completed in {0} seconds", 0x1388, new object[] { span2.TotalSeconds });
                }
            }
        }

        public static void ClearCachedData()
        {
            mAllAvatars = null;
        }

        private static bool ExecuteQuery(StringBuilder sql)
        {
            List<string> list;
            List<List<string>> list2;
            bool flag = ExecuteQuery(sql.ToString(), out list, out list2);
            sql.Remove(0, sql.Length);
            return flag;
        }

        private static bool ExecuteQuery(string query, out List<string> columns, out List<List<string>> rows)
        {
            return DataAccess.AdhocQuery(query, out columns, out rows);
        }

        public string AchievementQuery
        {
            get
            {
                return this.mAchievementQuery;
            }
        }

        public static Dictionary<int, Avatar> AllAvatars
        {
            get
            {
                lock (AllAvatarsMutex)
                {
                    if (mAllAvatars == null)
                    {
                        mAllAvatars = new Dictionary<int, Avatar>();
                        foreach (Avatar avatar in new QuazalQuery("GetAllAvatars", new object[0]).GetObjects<Avatar>())
                        {
                            AllAvatars.Add(avatar.ID, avatar);
                        }
                    }
                    return mAllAvatars;
                }
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
                return (path + "CachedAvatar" + this.ResourceKey + ".png");
            }
        }

        public static Avatar Default
        {
            get
            {
                if (mDefault == null)
                {
                    mDefault = new Avatar();
                    mDefault.mIsLocalResource = true;
                    mDefault.mDescription = "Default Avatar";
                    mDefault.mResourceKey = "Default";
                }
                return mDefault;
            }
        }

        public string Description
        {
            get
            {
                return Loc.Get("<LOC>" + this.mDescription);
            }
        }

        public static System.Drawing.Image GPGIcon
        {
            get
            {
                return AvatarImages.GPG;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public Bitmap Image
        {
            get
            {
                WaitCallback callBack = null;
                Bitmap mImage;
                object obj2;
                Monitor.Enter(obj2 = this.ImageMutex);
                try
                {
                    if ((this.mImage != null) && (this.mImage.Width == 10))
                    {
                        this.mImage = null;
                    }
                    if (callBack == null)
                    {
                        callBack = delegate (object o) {
                            try
                            {
                                if (this.mImage == null)
                                {
                                    if (this.IsLocalResource)
                                    {
                                        this.mImage = AvatarImages.ResourceManager.GetObject(this.ResourceKey) as Bitmap;
                                    }
                                    else if (System.IO.File.Exists(this.CacheFileName) && (new FileInfo(this.CacheFileName).CreationTime > (DateTime.Now - new TimeSpan(1, 0, 0, 0))))
                                    {
                                        FileStream stream = new FileStream(this.CacheFileName, FileMode.Open);
                                        this.mImage = System.Drawing.Image.FromStream(stream) as Bitmap;
                                        stream.Close();
                                    }
                                    else
                                    {
                                        using (MemoryStream stream2 = new MemoryStream(new WebClient().DownloadData(this.ImageUrl)))
                                        {
                                            this.mImage = System.Drawing.Image.FromStream(stream2) as Bitmap;
                                        }
                                        try
                                        {
                                            this.mImage.Save(this.CacheFileName);
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
                    for (int i = 0; (i < 50) && (this.mImage == null); i++)
                    {
                        Thread.Sleep(10);
                    }
                    if (this.mImage == null)
                    {
                        if (System.IO.File.Exists(this.CacheFileName))
                        {
                            FileStream stream = new FileStream(this.CacheFileName, FileMode.Open);
                            this.mImage = System.Drawing.Image.FromStream(stream) as Bitmap;
                            stream.Close();
                        }
                        else
                        {
                            this.mImage = new Bitmap(10, 10);
                        }
                    }
                    mImage = this.mImage;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    mImage = null;
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
                return mImage;
            }
        }

        public string ImageUrl
        {
            get
            {
                return string.Format("{0}/{1}.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/avatars"), this.ResourceKey);
            }
        }

        public bool IsLocalResource
        {
            get
            {
                return this.mIsLocalResource;
            }
        }

        public string ResourceKey
        {
            get
            {
                return this.mResourceKey;
            }
        }
    }
}

