namespace GPG.Multiplayer.Statistics
{
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public class PlayerAward : MappedObject
    {
        [FieldMap("award_id")]
        private int mAwardID;
        [FieldMap("player_award_id")]
        private int mID;
        [FieldMap("is_achieved")]
        private bool mIsAchieved;
        [FieldMap("player_id")]
        private int mPlayerID;
        [FieldMap("progress")]
        private int mProgress;
        [FieldMap("show_progress")]
        private bool mShowProgress;

        public PlayerAward(DataRecord record) : base(record)
        {
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

        public static PlayerAward[] GetPlayerAwards(string player)
        {
            return new QuazalQuery("GetPlayerAwardsByName", new object[] { player }).GetObjects<PlayerAward>().ToArray();
        }

        public static void ReCalculateAllAwards()
        {
            ReCalculateAllAwards(null);
        }

        public static void ReCalculateAllAwards(IStatusProvider statusProvider)
        {
            DateTime now = DateTime.Now;
            StringBuilder sql = new StringBuilder(100);
            string str = "temp_player_awards";
            string str2 = "old_player_awards";
            Avatar.ClearCachedData();
            AwardCategory.ClearCachedData();
            AwardSet.ClearCachedData();
            GPG.Multiplayer.Statistics.Award.ClearCachedData();
            try
            {
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Clearing old temp data if it exists", new object[0]);
                }
                Thread.Sleep(500);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS {0};", str2);
                sql.AppendFormat("DROP TABLE IF EXISTS {0};", str);
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_avatar;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_tournament_results;", new object[0]);
                sql.AppendFormat("CREATE TABLE temp_player_ids (", new object[0]);
                sql.AppendFormat("player_id INTEGER UNSIGNED NOT NULL,", new object[0]);
                sql.AppendFormat("PRIMARY KEY(player_id)", new object[0]);
                sql.AppendFormat(") SELECT principal AS player_id FROM principal_info;", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("UPDATE tournaments SET round = (SELECT max(wins + losses + draws) FROM tournament_round WHERE tournament_round.tournament_id = tournaments.tournament_id) WHERE (SELECT max(wins + losses + draws) FROM tournament_round WHERE tournament_round.tournament_id = tournaments.tournament_id) is not null", new object[0]);
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
                sql.AppendFormat(");", new object[0]);
                ExecuteQuery(sql);
                foreach (Avatar avatar in Avatar.AllAvatars.Values)
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
                    statusProvider.SetStatus("Fetching manual assigments for avatars.", new object[0]);
                }
                sql.AppendFormat("REPLACE INTO temp_player_avatar (player_id, avatar_id, manual_assignment) ", new object[0]);
                sql.AppendFormat("SELECT player_id, avatar_id, manual_assignment FROM player_avatar WHERE manual_assignment = 1", new object[0]);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Clearing player_avatars where the avatar no longer exists for the player.", new object[0]);
                }
                sql.AppendFormat("UPDATE player_display_awards ", new object[0]);
                sql.AppendFormat("LEFT OUTER JOIN temp_player_avatar ON ", new object[0]);
                sql.AppendFormat("  player_display_awards.avatar = temp_player_avatar.avatar_id AND player_display_awards.player_id = temp_player_avatar.player_id ", new object[0]);
                sql.AppendFormat("SET player_display_awards.avatar = temp_player_avatar.avatar_id ", new object[0]);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Moving temp avatar data to live data", new object[0]);
                }
                Thread.Sleep(0x3e8);
                sql.AppendFormat("RENAME TABLE player_avatar TO old_player_avatar, temp_player_avatar TO player_avatar;", new object[0]);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Creating temp schema for awards", new object[0]);
                }
                sql.AppendFormat("CREATE TABLE {0} (", str);
                sql.AppendFormat("player_award_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,", new object[0]);
                sql.AppendFormat("player_id INTEGER UNSIGNED NULL,", new object[0]);
                sql.AppendFormat("award_id INTEGER UNSIGNED NULL,", new object[0]);
                sql.AppendFormat("progress INTEGER UNSIGNED NULL DEFAULT 0,", new object[0]);
                sql.AppendFormat("is_achieved BOOL NULL DEFAULT 0,", new object[0]);
                sql.AppendFormat("show_progress BOOL NULL DEFAULT 0,", new object[0]);
                sql.AppendFormat("PRIMARY KEY(player_award_id),", new object[0]);
                sql.AppendFormat("INDEX temp_player_award_status_index3095(player_id),", new object[0]);
                sql.AppendFormat("INDEX temp_player_award_status_index3101(is_achieved),", new object[0]);
                sql.AppendFormat("INDEX temp_player_award_status_index3102(show_progress),", new object[0]);
                sql.AppendFormat("UNIQUE INDEX temp_player_award_index3141(player_id, award_id)", new object[0]);
                sql.AppendFormat(");", new object[0]);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Creating temp units agg table", new object[0]);
                }
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_units;", new object[0]);
                sql.AppendFormat("CREATE TABLE temp_player_units (", new object[0]);
                sql.AppendFormat("principal_id INT NOT NULL,", new object[0]);
                sql.AppendFormat("amount INT NOT NULL,", new object[0]);
                sql.AppendFormat("category VARCHAR(10) NOT NULL,", new object[0]);
                sql.AppendFormat("PRIMARY KEY(category, principal_id),", new object[0]);
                sql.AppendFormat("INDEX temp_player_units_ix1(category, amount, principal_id)", new object[0]);
                sql.AppendFormat(");", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'aeonexp', principal_id, SUM(built) FROM player_units  WHERE unit_id in ('UAL0401', 'UAS0401', 'UAA0310') GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'cybranexp', principal_id, SUM(built) FROM player_units  WHERE unit_id in ('URA0401', 'URL0401', 'URL0402') GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'uefexp', principal_id, SUM(built) FROM player_units  WHERE unit_id in ('UEB2401', 'UEL0401', 'UES0401') GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'airbuilt', principal_id, SUM(built) FROM player_units  WHERE unit_id like ('URA%') OR unit_id like ('UEA%') OR unit_id like ('UAA%') GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'landbuilt', principal_id, SUM(built) FROM player_units  WHERE unit_id like ('URL%') OR unit_id like ('UEL%') OR unit_id like ('UAL%') GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'seabuilt', principal_id, SUM(built) FROM player_units  WHERE unit_id like ('URS%') OR unit_id like ('UES%') OR unit_id like ('UAS%') GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'built', principal_id, SUM(built) FROM player_units GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'killed', principal_id, SUM(killed) FROM player_units GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", new object[0]);
                sql.AppendFormat("(SELECT 'civkilled', principal_id, SUM(killed) FROM player_units  WHERE unit_id like ('URC%') OR unit_id like ('UEC%') OR unit_id like ('UAC%') GROUP BY principal_id);", new object[0]);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Creating temp ratings table", new object[0]);
                }
                sql.AppendFormat("DROP TABLE IF EXISTS temp_ratings_for_awards;", new object[0]);
                sql.AppendFormat("CREATE TABLE temp_ratings_for_awards SELECT * FROM ratings", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("ALTER TABLE temp_ratings_for_awards ADD INDEX temp_ratings_for_awards_ix1 (principal_id, category, team_name)", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("ALTER TABLE temp_ratings_for_awards ADD INDEX temp_ratings_for_awards_ix2 (rank, category, principal_id", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("ALTER TABLE temp_ratings_for_awards ADD INDEX temp_ratings_for_awards_ix3 (team_name, principal_id, category)", new object[0]);
                ExecuteQuery(sql);
                foreach (GPG.Multiplayer.Statistics.Award award in GPG.Multiplayer.Statistics.Award.AllAwards.Values)
                {
                    if ((award.AchievementQuery == null) || (award.AchievementQuery.Length <= 0))
                    {
                        continue;
                    }
                    DateTime time3 = DateTime.Now;
                    if (award.AwardSet != null)
                    {
                        if (statusProvider != null)
                        {
                            statusProvider.SetStatus("Calculating achievement status for award: {0}, {1}", new object[] { award.AwardSet.Name, award.AwardDegree });
                        }
                    }
                    else if (statusProvider != null)
                    {
                        statusProvider.SetStatus("Calculating achievement status for award: {0}, {1}", new object[] { "Unknown Award Set", award.AwardDegree });
                    }
                    string str3 = award.AchievementQuery.Replace("ratings", "temp_ratings_for_awards");
                    sql.AppendFormat("INSERT INTO {0} (player_id, award_id, is_achieved) (SELECT player_id, {1}, 1 FROM temp_player_ids WHERE ({2}) > 0);", str, award.ID, str3);
                    if (statusProvider != null)
                    {
                        statusProvider.SetStatus(sql.ToString(), new object[0]);
                    }
                    ExecuteQuery(sql);
                    TimeSpan span2 = (TimeSpan) (DateTime.Now - time3);
                    if (statusProvider != null)
                    {
                        statusProvider.SetStatus("Finished calculating award: {0}, {1} in {2} seconds", new object[] { award.AwardSet.Name, award.AwardDegree, span2.TotalSeconds });
                    }
                    Thread.Sleep(500);
                }
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Clearing player_display_awards where the award no longer exists for the player.", new object[0]);
                }
                sql.AppendFormat("UPDATE player_display_awards SET ", new object[0]);
                sql.AppendFormat("award1 = (SELECT award_id FROM {0} WHERE player_display_awards.player_id = {0}.player_id AND award1 = award_id LIMIT 1), ", str);
                sql.AppendFormat("award2 = (SELECT award_id FROM {0} WHERE player_display_awards.player_id = {0}.player_id AND award2 = award_id LIMIT 1), ", str);
                sql.AppendFormat("award3 = (SELECT award_id FROM {0} WHERE player_display_awards.player_id = {0}.player_id AND award3 = award_id LIMIT 1) ", str);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Moving temp data to live data", new object[0]);
                }
                sql.AppendFormat("RENAME TABLE player_award TO {0}, {1} TO player_award;", str2, str);
                ExecuteQuery(sql);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Inserting Player Display record for new players.", new object[0]);
                }
                sql.AppendFormat("INSERT INTO player_display_awards (player_id) SELECT principal AS player_id FROM principal_info where (SELECT COUNT(*) FROM player_display_awards WHERE player_id = principal) = 0", new object[0]);
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
                sql.AppendFormat("DROP TABLE IF EXISTS {0};", str2);
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", new object[0]);
                sql.AppendFormat("DROP TABLE IF EXISTS temp_ratings_for_awards;", new object[0]);
                ExecuteQuery(sql);
                sql.AppendFormat("DROP TABLE IF EXISTS tournament_final_results;", new object[0]);
                sql.AppendFormat("RENAME TABLE temp_tournament_results TO tournament_final_results", new object[0]);
                ExecuteQuery(sql);
                TimeSpan span3 = (TimeSpan) (DateTime.Now - now);
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("Awards processing completed in {0} seconds", 0x1388, new object[] { span3.TotalSeconds });
                }
            }
        }

        public GPG.Multiplayer.Statistics.Award Award
        {
            get
            {
                return GPG.Multiplayer.Statistics.Award.AllAwards[this.AwardID];
            }
        }

        public int AwardID
        {
            get
            {
                return this.mAwardID;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool IsAchieved
        {
            get
            {
                return this.mIsAchieved;
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
        }

        public int Progress
        {
            get
            {
                return this.mProgress;
            }
        }

        public bool ShowProgress
        {
            get
            {
                return this.mShowProgress;
            }
        }
    }
}

