Imports GPG
Imports GPG.DataAccess
Imports GPG.Logging
Imports GPG.Multiplayer.Quazal
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading

Namespace GPG.Multiplayer.Statistics
    Public Class Avatar
        Inherits MappedObject
        ' Methods
        Private Sub New()
            Me.mIsLocalResource = True
            Me.ImageMutex = New Object
        End Sub

        Public Sub New(ByVal record As DataRecord)
            MyBase.New(record)
            Me.mIsLocalResource = True
            Me.ImageMutex = New Object
        End Sub

        Public Shared Sub CalculateAvatars()
            Avatar.CalculateAvatars(Nothing)
        End Sub

        Public Shared Sub CalculateAvatars(ByVal statusProvider As IStatusProvider)
            Dim now As DateTime = DateTime.Now
            Dim sql As New StringBuilder(100)
            Avatar.ClearCachedData
            Try 
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Clearing old temp data if it exists", New Object(0  - 1) {})
                End If
                Thread.Sleep(500)
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_avatar;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_tournament_results;", New Object(0  - 1) {})
                sql.AppendFormat("CREATE TABLE temp_player_ids (", New Object(0  - 1) {})
                sql.AppendFormat("player_id INTEGER UNSIGNED NOT NULL,", New Object(0  - 1) {})
                sql.AppendFormat("PRIMARY KEY(player_id)", New Object(0  - 1) {})
                sql.AppendFormat(") SELECT principal AS player_id FROM principal_info;", New Object(0  - 1) {})
                Avatar.ExecuteQuery(sql)
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
                Avatar.ExecuteQuery(sql)
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
                sql.AppendFormat(") SELECT * FROM player_avatar WHERE manual_assignment = 1;", New Object(0  - 1) {})
                Avatar.ExecuteQuery(sql)
                Dim avatar As Avatar
                For Each avatar In Avatar.AllAvatars.Values
                    If (((Not avatar.AchievementQuery Is Nothing) AndAlso (avatar.AchievementQuery.Length >= 1)) AndAlso (avatar.AchievementQuery <> "(null)")) Then
                        Dim time2 As DateTime = DateTime.Now
                        If (Not statusProvider Is Nothing) Then
                            statusProvider.SetStatus("Calculating achievement status for avatar: {0}", New Object() { avatar.Description })
                        End If
                        sql.AppendFormat("INSERT INTO temp_player_avatar (player_id, avatar_id, manual_assignment) (SELECT player_id, {0}, 0 FROM temp_player_ids WHERE ({1}) > 0);", avatar.ID, avatar.AchievementQuery)
                        Avatar.ExecuteQuery(sql)
                        Dim span As TimeSpan = DirectCast((DateTime.Now - time2), TimeSpan)
                        If (Not statusProvider Is Nothing) Then
                            statusProvider.SetStatus("Finished calculating avatar: {0} in {1} seconds", New Object() { avatar.Description, span.TotalSeconds })
                        End If
                        Thread.Sleep(500)
                    End If
                Next
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Removing unachieved display avatars", New Object(0  - 1) {})
                End If
                sql.AppendFormat("UPDATE player_display_awards SET avatar = 0 WHERE ", New Object(0  - 1) {})
                sql.AppendFormat("(SELECT COUNT(*) FROM temp_player_avatar WHERE ", New Object(0  - 1) {})
                sql.AppendFormat("temp_player_avatar.player_id = player_display_awards.player_id AND ", New Object(0  - 1) {})
                sql.AppendFormat("temp_player_avatar.avatar_id = player_display_awards.avatar) = 0;", New Object(0  - 1) {})
                Avatar.ExecuteQuery(sql)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Moving temp avatar data to live data", New Object(0  - 1) {})
                End If
                Thread.Sleep(&H3E8)
                sql.AppendFormat("RENAME TABLE player_avatar TO old_player_avatar, temp_player_avatar TO player_avatar;", New Object(0  - 1) {})
                Avatar.ExecuteQuery(sql)
            Catch exception As Exception
                ErrorLog.WriteLine("Failed to calculate player awards due to the following exception:", New Object(0  - 1) {})
                ErrorLog.WriteLine(exception)
            Finally
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Deleting old data", New Object(0  - 1) {})
                End If
                sql.AppendFormat("DROP TABLE IF EXISTS old_player_avatar;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_player_ids;", New Object(0  - 1) {})
                sql.AppendFormat("DROP TABLE IF EXISTS temp_tournament_results;", New Object(0  - 1) {})
                Avatar.ExecuteQuery(sql)
                Dim span2 As TimeSpan = DirectCast((DateTime.Now - now), TimeSpan)
                If (Not statusProvider Is Nothing) Then
                    statusProvider.SetStatus("Awards processing completed in {0} seconds", &H1388, New Object() { span2.TotalSeconds })
                End If
            End Try
        End Sub

        Public Shared Sub ClearCachedData()
            Avatar.mAllAvatars = Nothing
        End Sub

        Private Shared Function ExecuteQuery(ByVal sql As StringBuilder) As Boolean
            Dim list As List(Of String)
            Dim list2 As List(Of List(Of String))
            Dim flag As Boolean = Avatar.ExecuteQuery(sql.ToString, list, list2)
            sql.Remove(0, sql.Length)
            Return flag
        End Function

        Private Shared Function ExecuteQuery(ByVal query As String, <Out> ByRef columns As List(Of String), <Out> ByRef rows As List(Of List(Of String))) As Boolean
            Return DataAccess.AdhocQuery(query, columns, rows)
        End Function


        ' Properties
        Public ReadOnly Property AchievementQuery As String
            Get
                Return Me.mAchievementQuery
            End Get
        End Property

        Public Shared ReadOnly Property AllAvatars As Dictionary(Of Integer, Avatar)
            Get
                SyncLock Avatar.AllAvatarsMutex
                    If (Avatar.mAllAvatars Is Nothing) Then
                        Avatar.mAllAvatars = New Dictionary(Of Integer, Avatar)
                        Dim avatar As Avatar
                        For Each avatar In New QuazalQuery("GetAllAvatars", New Object(0  - 1) {}).GetObjects(Of Avatar)
                            Avatar.AllAvatars.Add(avatar.ID, avatar)
                        Next
                    End If
                    Return Avatar.mAllAvatars
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property CacheFileName As String
            Get
                Dim path As String = (Environment.GetFolderPath(SpecialFolder.LocalApplicationData) & "\Gas Powered Games\GPGnet\")
                If Not Directory.Exists(path) Then
                    Directory.CreateDirectory(path)
                End If
                Return (path & "CachedAvatar" & Me.ResourceKey & ".png")
            End Get
        End Property

        Public Shared ReadOnly Property [Default] As Avatar
            Get
                If (Avatar.mDefault Is Nothing) Then
                    Avatar.mDefault = New Avatar
                    Avatar.mDefault.mIsLocalResource = True
                    Avatar.mDefault.mDescription = "Default Avatar"
                    Avatar.mDefault.mResourceKey = "Default"
                End If
                Return Avatar.mDefault
            End Get
        End Property

        Public ReadOnly Property Description As String
            Get
                Return Loc.Get(("<LOC>" & Me.mDescription))
            End Get
        End Property

        Public Shared ReadOnly Property GPGIcon As Image
            Get
                Return AvatarImages.GPG
            End Get
        End Property

        Public ReadOnly Property ID As Integer
            Get
                Return Me.mID
            End Get
        End Property

        Public ReadOnly Property Image As Bitmap
            Get
                Dim callBack As WaitCallback = Nothing
                Dim mImage As Bitmap
                Dim obj2 As Object
                Monitor.Enter(obj2 = Me.ImageMutex)
                Try 
                    If ((Not Me.mImage Is Nothing) AndAlso (Me.mImage.Width = 10)) Then
                        Me.mImage = Nothing
                    End If
                    If (callBack Is Nothing) Then
                        callBack = Function (ByVal o As Object) 
                            Try 
                                If (Me.mImage Is Nothing) Then
                                    If Me.IsLocalResource Then
                                        Me.mImage = TryCast(AvatarImages.ResourceManager.GetObject(Me.ResourceKey),Bitmap)
                                    ElseIf (File.Exists(Me.CacheFileName) AndAlso (New FileInfo(Me.CacheFileName).CreationTime > (DateTime.Now - New TimeSpan(1, 0, 0, 0)))) Then
                                        Dim stream As New FileStream(Me.CacheFileName, FileMode.Open)
                                        Me.mImage = TryCast(Image.FromStream(stream),Bitmap)
                                        stream.Close
                                    Else
                                        Using stream2 As MemoryStream = New MemoryStream(New WebClient().DownloadData(Me.ImageUrl))
                                            Me.mImage = TryCast(Image.FromStream(stream2),Bitmap)
                                        End Using
                                        Try 
                                            Me.mImage.Save(Me.CacheFileName)
                                        Catch exception As Exception
                                            ErrorLog.WriteLine(exception)
                                        End Try
                                    End If
                                End If
                            Catch exception2 As Exception
                                ErrorLog.WriteLine(exception2)
                            End Try
                        End Function
                    End If
                    ThreadPool.QueueUserWorkItem(callBack)
                    Dim i As Integer = 0
                    Do While ((i < 50) AndAlso (Me.mImage Is Nothing))
                        Thread.Sleep(10)
                        i += 1
                    Loop
                    If (Me.mImage Is Nothing) Then
                        If File.Exists(Me.CacheFileName) Then
                            Dim stream As New FileStream(Me.CacheFileName, FileMode.Open)
                            Me.mImage = TryCast(Image.FromStream(stream),Bitmap)
                            stream.Close
                        Else
                            Me.mImage = New Bitmap(10, 10)
                        End If
                    End If
                    mImage = Me.mImage
                Catch exception As Exception
                    ErrorLog.WriteLine(exception)
                    mImage = Nothing
                Finally
                    Monitor.Exit(obj2)
                End Try
                Return mImage
            End Get
        End Property

        Public ReadOnly Property ImageUrl As String
            Get
                Return String.Format("{0}/{1}.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/avatars"), Me.ResourceKey)
            End Get
        End Property

        Public ReadOnly Property IsLocalResource As Boolean
            Get
                Return Me.mIsLocalResource
            End Get
        End Property

        Public ReadOnly Property ResourceKey As String
            Get
                Return Me.mResourceKey
            End Get
        End Property


        ' Fields
        Private Shared AllAvatarsMutex As Object = New Object
        Private ImageMutex As Object
        <FieldMap("achievement_query")> _
        Private mAchievementQuery As String
        Private Shared mAllAvatars As Dictionary(Of Integer, Avatar) = Nothing
        Private Shared mDefault As Avatar = Nothing
        <FieldMap("description")> _
        Private mDescription As String
        <FieldMap("avatar_id")> _
        Private mID As Integer
        Private mImage As Bitmap
        <FieldMap("is_local_resource")> _
        Private mIsLocalResource As Boolean
        <FieldMap("resource_key")> _
        Private mResourceKey As String
    End Class
End Namespace

