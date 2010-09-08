Imports GPG
Imports GPG.DataAccess
Imports GPG.Logging
Imports GPG.Multiplayer.Quazal
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Net
Imports System.Threading

Namespace GPG.Multiplayer.Statistics
    Public Class Award
        Inherits MappedObject
        ' Methods
        Public Sub New(ByVal record As DataRecord)
            MyBase.New(record)
            Me.SmallImageMutex = New Object
            Me.LargeImageMutex = New Object
        End Sub

        Public Shared Sub ClearCachedData()
            Award.mAllAwards = Nothing
        End Sub


        ' Properties
        Public ReadOnly Property AchievementDescription As String
            Get
                Return Loc.Get(("<LOC>" & Me.mAchievementDescription))
            End Get
        End Property

        Public ReadOnly Property AchievementQuery As String
            Get
                Return Me.mAchievementQuery
            End Get
        End Property

        Public Shared ReadOnly Property AllAwards As Dictionary(Of Integer, Award)
            Get
                SyncLock Award.AllAwardsMutex
                    If (Award.mAllAwards Is Nothing) Then
                        Award.mAllAwards = New Dictionary(Of Integer, Award)
                        Dim award As Award
                        For Each award In New QuazalQuery("GetAllAwards", New Object(0  - 1) {}).GetObjects(Of Award)
                            Award.mAllAwards.Add(award.ID, award)
                        Next
                    End If
                    Return Award.mAllAwards
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property AwardDegree As Integer
            Get
                Return Me.mAwardDegree
            End Get
        End Property

        Public ReadOnly Property AwardSet As AwardSet
            Get
                Return AwardSet.AllAwardSets.Item(Me.AwardSetID)
            End Get
        End Property

        Public ReadOnly Property AwardSetID As Integer
            Get
                Return Me.mAwardSetID
            End Get
        End Property

        Public ReadOnly Property CacheFileName As String
            Get
                Dim path As String = (Environment.GetFolderPath(SpecialFolder.LocalApplicationData) & "\Gas Powered Games\GPGnet\")
                If Not Directory.Exists(path) Then
                    Directory.CreateDirectory(path)
                End If
                Return String.Concat(New String() { path, "CachedAward-", Me.AwardSet.Name, "-", Me.AwardDegree.ToString, ".png" })
            End Get
        End Property

        Public ReadOnly Property ID As Integer
            Get
                Return Me.mID
            End Get
        End Property

        Public ReadOnly Property IsLocalResource As Boolean
            Get
                Return Me.mIsLocalResource
            End Get
        End Property

        Public ReadOnly Property LargeImage As Image
            Get
                Dim mLargeImage As Image
                Dim obj2 As Object
                Monitor.Enter(obj2 = Me.LargeImageMutex)
                Try 
                    If (Me.mLargeImage Is Nothing) Then
                        If Me.IsLocalResource Then
                            Me.mLargeImage = TryCast(AwardImages.ResourceManager.GetObject(String.Format("_{0}{1}", Me.AwardSet.Name.Replace(" ", ""), Me.AwardDegree)),Bitmap)
                        Else
                            Using stream As MemoryStream = New MemoryStream(New WebClient().DownloadData(Me.LargeImageUrl))
                                Me.mLargeImage = Image.FromStream(stream)
                            End Using
                        End If
                    End If
                    mLargeImage = Me.mLargeImage
                Catch exception As Exception
                    ErrorLog.WriteLine(exception)
                    mLargeImage = Nothing
                Finally
                    Monitor.Exit(obj2)
                End Try
                Return mLargeImage
            End Get
        End Property

        Public ReadOnly Property LargeImageUrl As String
            Get
                Return String.Format("{0}/{1}/{2}.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/awards"), Me.AwardSet.Name, Me.AwardDegree)
            End Get
        End Property

        Public ReadOnly Property SmallImage As Image
            Get
                Dim callBack As WaitCallback = Nothing
                Dim mSmallImage As Image
                Dim obj2 As Object
                Monitor.Enter(obj2 = Me.SmallImageMutex)
                Try 
                    If ((Not Me.mSmallImage Is Nothing) AndAlso (Me.mSmallImage.Width = 10)) Then
                        Me.mSmallImage = Nothing
                    End If
                    If (callBack Is Nothing) Then
                        callBack = Function (ByVal o As Object) 
                            Try 
                                If (Me.mSmallImage Is Nothing) Then
                                    If Me.IsLocalResource Then
                                        Me.mSmallImage = TryCast(AwardImages.ResourceManager.GetObject(String.Format("_{0}{1}small", Me.AwardSet.Name.Replace(" ", ""), Me.AwardDegree)),Bitmap)
                                    ElseIf (File.Exists(Me.CacheFileName) AndAlso (New FileInfo(Me.CacheFileName).CreationTime > (DateTime.Now - New TimeSpan(1, 0, 0, 0)))) Then
                                        Dim stream As New FileStream(Me.CacheFileName, FileMode.Open)
                                        Me.mSmallImage = TryCast(Image.FromStream(stream),Bitmap)
                                        stream.Close
                                    Else
                                        Using stream2 As MemoryStream = New MemoryStream(New WebClient().DownloadData(Me.SmallImageUrl))
                                            Me.mSmallImage = Image.FromStream(stream2)
                                        End Using
                                        Try 
                                            Me.mSmallImage.Save(Me.CacheFileName)
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
                    Do While ((i < 50) AndAlso (Me.mSmallImage Is Nothing))
                        Thread.Sleep(10)
                        i += 1
                    Loop
                    If (Me.mSmallImage Is Nothing) Then
                        If File.Exists(Me.CacheFileName) Then
                            Dim stream As New FileStream(Me.CacheFileName, FileMode.Open)
                            Me.mSmallImage = TryCast(Image.FromStream(stream),Bitmap)
                            stream.Close
                        Else
                            Me.mSmallImage = New Bitmap(10, 10)
                        End If
                    End If
                    mSmallImage = Me.mSmallImage
                Catch exception As Exception
                    ErrorLog.WriteLine(exception)
                    mSmallImage = Nothing
                Finally
                    Monitor.Exit(obj2)
                End Try
                Return mSmallImage
            End Get
        End Property

        Public ReadOnly Property SmallImageUrl As String
            Get
                Return String.Format("{0}/{1}/{2}small.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/awards"), Me.AwardSet.Name, Me.AwardDegree)
            End Get
        End Property


        ' Fields
        Private Shared AllAwardsMutex As Object = New Object
        Private LargeImageMutex As Object
        <FieldMap("achievement_description")> _
        Private mAchievementDescription As String
        <FieldMap("achievement_query")> _
        Private mAchievementQuery As String
        Public Shared mAllAwards As Dictionary(Of Integer, Award) = Nothing
        <FieldMap("award_degree")> _
        Private mAwardDegree As Integer
        <FieldMap("award_set_id")> _
        Private mAwardSetID As Integer
        <FieldMap("award_id")> _
        Private mID As Integer
        <FieldMap("is_local_resource")> _
        Private mIsLocalResource As Boolean
        Private mLargeImage As Image
        Private mSmallImage As Image
        Private SmallImageMutex As Object
    End Class
End Namespace

