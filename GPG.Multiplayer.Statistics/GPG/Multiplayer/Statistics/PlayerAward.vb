Imports GPG.DataAccess
Imports GPG.Logging
Imports GPG.Multiplayer.Quazal
Imports System
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading

Namespace GPG.Multiplayer.Statistics
    Public Class PlayerAward
        Inherits MappedObject
        ' Methods
        Public Sub New(ByVal record As DataRecord)
            MyBase.New(record)
        End Sub

        Private Shared Function ExecuteQuery(ByVal sql As StringBuilder) As Boolean
            Dim list As List(Of String)
            Dim list2 As List(Of List(Of String))
            Dim flag As Boolean = PlayerAward.ExecuteQuery(sql.ToString, list, list2)
            sql.Remove(0, sql.Length)
            Return flag
        End Function

        Private Shared Function ExecuteQuery(ByVal query As String, <Out> ByRef columns As List(Of String), <Out> ByRef rows As List(Of List(Of String))) As Boolean
            Return DataAccess.AdhocQuery(query, columns, rows)
        End Function

        Public Shared Function GetPlayerAwards(ByVal player As String) As PlayerAward()
            Return New QuazalQuery("GetPlayerAwardsByName", New Object() { player }).GetObjects(Of PlayerAward).ToArray
        End Function

        Public Shared Sub ReCalculateAllAwards()
            PlayerAward.ReCalculateAllAwards(Nothing)
        End Sub

        Public Shared Sub ReCalculateAllAwards(ByVal statusProvider As IStatusProvider)
            Dim now As DateTime = DateTime.Now
            Dim sql As New StringBuilder(100)
            Dim str As String = "temp_player_awards"
            Dim str2 As String = "old_player_awards"
            Avatar.ClearCachedData
            AwardCategory.ClearCachedData
            AwardSet.ClearCachedData
            Award.ClearCachedData
            Try 
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Clearing old temp data if it exists", New Object(0  - 1) {})
                End If
                Thread.Sleep(500)
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS {0};", str2)
                sql.AppendFormat("DROP TABLE IF EXISTS {0};", str)
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_avatar;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_tournament_results;", New Object(0  - 1) {})
                sql.AppendFormat("CREATE TABLE temp_player_ids (", New Object(0  - 1) {})
                sql.AppendFormat("player_id INTEGER UNSIGNED NOT NULL,", New Object(0  - 1) {})
                sql.AppendFormat("PRIMARY KEY(player_id)", New Object(0  - 1) {})
                sql.AppendFormat(") SELECT principal AS player_id FROM principal_info;", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("UPDATE tournaments SET round = (SELECT max(wins + losses + draws) FROM tournament_round WHERE tournament_round.tournament_id = tournaments.tournament_id) WHERE (SELECT max(wins + losses + draws) FROM tournament_round WHERE tournament_round.tournament_id = tournaments.tournament_id) is not null", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("CREATE TABLE temp_tournament_results ", New Object(0  - 1) {})
                sql.AppendFormat("SELECT tournaments.tournament_id, new.principal_id, ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT count(*) FROM tournament_round old WHERE old.tournament_id = tournaments.tournament_id AND ", New Object(0  - 1) {})
                sql.AppendFormat("old.round = tournaments.round AND ", New Object(0  - 1) {})
                sql.AppendFormat("( (old.wins + (0.5 * old.draws)) >  (new.wins + (0.5 * new.draws))  OR ", New Object(0  - 1) {})
                sql.AppendFormat("( (old.wins + (0.5 * old.draws)) =  (new.wins + (0.5 * new.draws)) AND (old.seed < new.seed) ) ) ) + 1 as finish_pos ", New Object(0  - 1) {})
                sql.AppendFormat("FROM tournaments, tournament_round new ", New Object(0  - 1) {})
                sql.AppendFormat("WHERE ", New Object(0  - 1) {})
                sql.AppendFormat("tournaments.tournament_id = new.tournament_id ", New Object(0  - 1) {})
                sql.AppendFormat("AND ", New Object(0  - 1) {})
                sql.AppendFormat("tournaments.round = new.round ", New Object(0  - 1) {})
                sql.AppendFormat("ORDER BY tournaments.tournament_id DESC, (wins + (0.5 * draws)) DESC, seed ASC;", New Object(0  - 1) {})
                sql.AppendFormat("ALTER TABLE temp_tournament_results ADD index temp_tournament_results_ix1 (principal_id, finish_pos);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Creating temp schema for avatars", New Object(0  - 1) {})
                End If
                Thread.Sleep(500)
                sql.AppendFormat("CREATE TABLE temp_player_avatar (", New Object(0  - 1) {})
                sql.AppendFormat("player_avatar_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,", New Object(0  - 1) {})
                sql.AppendFormat("player_id INTEGER UNSIGNED NULL,", New Object(0  - 1) {})
                sql.AppendFormat("avatar_id INTEGER UNSIGNED NULL,", New Object(0  - 1) {})
                sql.AppendFormat("manual_assignment BOOL NULL DEFAULT 0,", New Object(0  - 1) {})
                sql.AppendFormat("PRIMARY KEY(player_avatar_id),", New Object(0  - 1) {})
                sql.AppendFormat("UNIQUE INDEX temp_player_avatar_index3199(player_id, avatar_id),", New Object(0  - 1) {})
                sql.AppendFormat("INDEX temp_player_avatar_index3203(avatar_id),", New Object(0  - 1) {})
                sql.AppendFormat("INDEX temp_player_avatar_index3208(manual_assignment)", New Object(0  - 1) {})
                sql.AppendFormat(");", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                Dim avatar As Avatar
                For Each avatar In Avatar.AllAvatars.Values
                    If (((Not avatar.AchievementQuery Is Nothing) AndAlso (avatar.AchievementQuery.Length >= 1)) AndAlso (avatar.AchievementQuery <> "(null)")) Then
                        Dim time2 As DateTime = DateTime.Now
                        If (Not statusProvider Is Nothing) Then
                            statusProvider.SetStatus("Calculating achievement status for avatar: {0}", New Object() { avatar.Description })
                        End If
                        sql.AppendFormat("INSERT INTO temp_player_avatar (player_id, avatar_id, manual_assignment) (SELECT player_id, {0}, 0 FROM temp_player_ids WHERE ({1}) > 0);", avatar.ID, avatar.AchievementQuery)
                        PlayerAward.ExecuteQuery(sql)
                        Dim span As TimeSpan = DirectCast((DateTime.Now - time2), TimeSpan)
                        If (Not statusProvider Is Nothing) Then
                            statusProvider.SetStatus("Finished calculating avatar: {0} in {1} seconds", New Object() { avatar.Description, span.TotalSeconds })
                        End If
                        Thread.Sleep(500)
                    End If
                Next
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Fetching manual assigments for avatars.", New Object(0  - 1) {})
                End If
                sql.AppendFormat("REPLACE INTO temp_player_avatar (player_id, avatar_id, manual_assignment) ", New Object(0  - 1) {})
                sql.AppendFormat("SELECT player_id, avatar_id, manual_assignment FROM player_avatar WHERE manual_assignment = 1", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Clearing player_avatars where the avatar no longer exists for the player.", New Object(0  - 1) {})
                End If
                sql.AppendFormat("UPDATE player_display_awards ", New Object(0  - 1) {})
                sql.AppendFormat("LEFT OUTER JOIN temp_player_avatar ON ", New Object(0  - 1) {})
                sql.AppendFormat("  player_display_awards.avatar = temp_player_avatar.avatar_id AND player_display_awards.player_id = temp_player_avatar.player_id ", New Object(0  - 1) {})
                sql.AppendFormat("SET player_display_awards.avatar = temp_player_avatar.avatar_id ", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Moving temp avatar data to live data", New Object(0  - 1) {})
                End If
                Thread.Sleep(&H3E8)
                sql.AppendFormat("RENAME TABLE player_avatar TO old_player_avatar, temp_player_avatar TO player_avatar;", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Creating temp schema for awards", New Object(0  - 1) {})
                End If
                sql.AppendFormat("CREATE TABLE {0} (", str)
                sql.AppendFormat("player_award_id INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,", New Object(0  - 1) {})
                sql.AppendFormat("player_id INTEGER UNSIGNED NULL,", New Object(0  - 1) {})
                sql.AppendFormat("award_id INTEGER UNSIGNED NULL,", New Object(0  - 1) {})
                sql.AppendFormat("progress INTEGER UNSIGNED NULL DEFAULT 0,", New Object(0  - 1) {})
                sql.AppendFormat("is_achieved BOOL NULL DEFAULT 0,", New Object(0  - 1) {})
                sql.AppendFormat("show_progress BOOL NULL DEFAULT 0,", New Object(0  - 1) {})
                sql.AppendFormat("PRIMARY KEY(player_award_id),", New Object(0  - 1) {})
                sql.AppendFormat("INDEX temp_player_award_status_index3095(player_id),", New Object(0  - 1) {})
                sql.AppendFormat("INDEX temp_player_award_status_index3101(is_achieved),", New Object(0  - 1) {})
                sql.AppendFormat("INDEX temp_player_award_status_index3102(show_progress),", New Object(0  - 1) {})
                sql.AppendFormat("UNIQUE INDEX temp_player_award_index3141(player_id, award_id)", New Object(0  - 1) {})
                sql.AppendFormat(");", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Creating temp units agg table", New Object(0  - 1) {})
                End If
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_units;", New Object(0  - 1) {})
                sql.AppendFormat("CREATE TABLE temp_player_units (", New Object(0  - 1) {})
                sql.AppendFormat("principal_id INT NOT NULL,", New Object(0  - 1) {})
                sql.AppendFormat("amount INT NOT NULL,", New Object(0  - 1) {})
                sql.AppendFormat("category VARCHAR(10) NOT NULL,", New Object(0  - 1) {})
                sql.AppendFormat("PRIMARY KEY(category, principal_id),", New Object(0  - 1) {})
                sql.AppendFormat("INDEX temp_player_units_ix1(category, amount, principal_id)", New Object(0  - 1) {})
                sql.AppendFormat(");", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'aeonexp', principal_id, SUM(built) FROM player_units  WHERE unit_id in ('UAL0401', 'UAS0401', 'UAA0310') GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'cybranexp', principal_id, SUM(built) FROM player_units  WHERE unit_id in ('URA0401', 'URL0401', 'URL0402') GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'uefexp', principal_id, SUM(built) FROM player_units  WHERE unit_id in ('UEB2401', 'UEL0401', 'UES0401') GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'airbuilt', principal_id, SUM(built) FROM player_units  WHERE unit_id like ('URA%') OR unit_id like ('UEA%') OR unit_id like ('UAA%') GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'landbuilt', principal_id, SUM(built) FROM player_units  WHERE unit_id like ('URL%') OR unit_id like ('UEL%') OR unit_id like ('UAL%') GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'seabuilt', principal_id, SUM(built) FROM player_units  WHERE unit_id like ('URS%') OR unit_id like ('UES%') OR unit_id like ('UAS%') GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'built', principal_id, SUM(built) FROM player_units GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'killed', principal_id, SUM(killed) FROM player_units GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("INSERT INTO temp_player_units (category, principal_id, amount) ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT 'civkilled', principal_id, SUM(killed) FROM player_units  WHERE unit_id like ('URC%') OR unit_id like ('UEC%') OR unit_id like ('UAC%') GROUP BY principal_id);", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Creating temp ratings table", New Object(0  - 1) {})
                End If
                sql.AppendFormat("DROP TABLE IF EXISTS temp_ratings_for_awards;", New Object(0  - 1) {})
                sql.AppendFormat("CREATE TABLE temp_ratings_for_awards SELECT * FROM ratings", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("ALTER TABLE temp_ratings_for_awards ADD INDEX temp_ratings_for_awards_ix1 (principal_id, category, team_name)", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("ALTER TABLE temp_ratings_for_awards ADD INDEX temp_ratings_for_awards_ix2 (rank, category, principal_id", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("ALTER TABLE temp_ratings_for_awards ADD INDEX temp_ratings_for_awards_ix3 (team_name, principal_id, category)", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                Dim award As Award
                For Each award In Award.AllAwards.Values
                    If ((award.AchievementQuery Is Nothing) OrElse (award.AchievementQuery.Length <= 0)) Then
                        Continue For
                    End If
                    Dim time3 As DateTime = DateTime.Now
                    If (Not award.AwardSet Is Nothing) Then
                        If (Not statusProvider Is Nothing) Then
                            statusProvider.SetStatus("Calculating achievement status for award: {0}, {1}", New Object() { award.AwardSet.Name, award.AwardDegree })
                        End If
                    ElseIf (Not statusProvider Is Nothing) Then
                        statusProvider.SetStatus("Calculating achievement status for award: {0}, {1}", New Object() { "Unknown Award Set", award.AwardDegree })
                    End If
                    Dim str3 As String = award.AchievementQuery.Replace("ratings", "temp_ratings_for_awards")
                    sql.AppendFormat("INSERT INTO {0} (player_id, award_id, is_achieved) (SELECT player_id, {1}, 1 FROM temp_player_ids WHERE ({2}) > 0);", str, award.ID, str3)
                    If (Not statusProvider Is Nothing) Then
                        statusProvider.SetStatus(sql.ToString, New Object(0  - 1) {})
                    End If
                    PlayerAward.ExecuteQuery(sql)
                    Dim span2 As TimeSpan = DirectCast((DateTime.Now - time3), TimeSpan)
                    If (Not statusProvider Is Nothing) Then
                        statusProvider.SetStatus("Finished calculating award: {0}, {1} in {2} seconds", New Object() { award.AwardSet.Name, award.AwardDegree, span2.TotalSeconds })
                    End If
                    Thread.Sleep(500)
                Next
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Clearing player_display_awards where the award no longer exists for the player.", New Object(0  - 1) {})
                End If
                sql.AppendFormat("UPDATE player_display_awards SET ", New Object(0  - 1) {})
                sql.AppendFormat("award1 = (SELECT award_id FROM {0} WHERE player_display_awards.player_id = {0}.player_id AND award1 = award_id LIMIT 1), ", str)
                sql.AppendFormat("award2 = (SELECT award_id FROM {0} WHERE player_display_awards.player_id = {0}.player_id AND award2 = award_id LIMIT 1), ", str)
                sql.AppendFormat("award3 = (SELECT award_id FROM {0} WHERE player_display_awards.player_id = {0}.player_id AND award3 = award_id LIMIT 1) ", str)
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Moving temp data to live data", New Object(0  - 1) {})
                End If
                sql.AppendFormat("RENAME TABLE player_award TO {0}, {1} TO player_award;", str2, str)
                PlayerAward.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Inserting Player Display record for new players.", New Object(0  - 1) {})
                End If
                sql.AppendFormat("INSERT INTO player_display_awards (player_id) SELECT principal AS player_id FROM principal_info where (SELECT COUNT(*) FROM player_display_awards WHERE player_id = principal) = 0", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
            Catch exception As Exception
                ErrorLog.WriteLine("Failed to calculate player awards due to the following exception:", New Object(0  - 1) {})
                ErrorLog.WriteLine(exception)
            Finally
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Deleting old data", New Object(0  - 1) {})
                End If
                sql.AppendFormat("DROP TABLE IF EXISTS {0};", str2)
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_ratings_for_awards;", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                sql.AppendFormat("DROP TABLE IF EXISTS tournament_final_results;", New Object(0  - 1) {})
                sql.AppendFormat("RENAME TABLE temp_tournament_results TO tournament_final_results", New Object(0  - 1) {})
                PlayerAward.ExecuteQuery(sql)
                Dim span3 As TimeSpan = DirectCast((DateTime.Now - now), TimeSpan)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Awards processing completed in {0} seconds", &H1388, New Object() { span3.TotalSeconds })
                End If
            End Try
        End Sub


        ' Properties
        Public ReadOnly Property Award As Award
            Get
                Return Award.AllAwards.Item(Me.AwardID)
            End Get
        End Property

        Public ReadOnly Property AwardID As Integer
            Get
                Return Me.mAwardID
            End Get
        End Property

        Public ReadOnly Property ID As Integer
            Get
                Return Me.mID
            End Get
        End Property

        Public ReadOnly Property IsAchieved As Boolean
            Get
                Return Me.mIsAchieved
            End Get
        End Property

        Public ReadOnly Property PlayerID As Integer
            Get
                Return Me.mPlayerID
            End Get
        End Property

        Public ReadOnly Property Progress As Integer
            Get
                Return Me.mProgress
            End Get
        End Property

        Public ReadOnly Property ShowProgress As Boolean
            Get
                Return Me.mShowProgress
            End Get
        End Property


        ' Fields
        <FieldMap("award_id")> _
        Private mAwardID As Integer
        <FieldMap("player_award_id")> _
        Private mID As Integer
        <FieldMap("is_achieved")> _
        Private mIsAchieved As Boolean
        <FieldMap("player_id")> _
        Private mPlayerID As Integer
        <FieldMap("progress")> _
        Private mProgress As Integer
        <FieldMap("show_progress")> _
        Private mShowProgress As Boolean
    End Class
End Namespace

