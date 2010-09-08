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
    Public Class AwardSet
        Inherits MappedObject
        ' Methods
        Public Sub New(ByVal record As DataRecord)
            MyBase.New(record)
            Me.mCategory = 1
            Me.mAlwaysShow = True
            Me.BaseImageMutex = New Object
        End Sub

        Public Shared Sub ClearCachedData()
            AwardSet.mAllAwardSets = Nothing
        End Sub


        ' Properties
        Public Shared ReadOnly Property AllAwardSets As Dictionary(Of Integer, AwardSet)
            Get
                SyncLock AwardSet.AllAwardSetsMutex
                    If (AwardSet.mAllAwardSets Is Nothing) Then
                        AwardSet.mAllAwardSets = New Dictionary(Of Integer, AwardSet)
                        Dim set As AwardSet
                        For Each set In New QuazalQuery("GetAllAwardSets", New Object(0  - 1) {}).GetObjects(Of AwardSet)
                            AwardSet.mAllAwardSets.Add([set].ID, [set])
                        Next
                    End If
                    Return AwardSet.mAllAwardSets
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property AlwaysShow As Boolean
            Get
                Return Me.mAlwaysShow
            End Get
        End Property

        Public ReadOnly Property Awards As List(Of Award)
            Get
                If (Me.mAwards Is Nothing) Then
                    Me.mAwards = New List(Of Award)(3)
                    Dim award As Award
                    For Each award In Award.AllAwards.Values
                        If (award.AwardSetID = Me.ID) Then
                            Me.mAwards.Add(award)
                        End If
                    Next
                End If
                Return Me.mAwards
            End Get
        End Property

        Public ReadOnly Property BaseImage As Image
            Get
                Dim mBaseImage As Image
                Dim obj2 As Object
                Monitor.Enter(obj2 = Me.BaseImageMutex)
                Try 
                    If (Me.mBaseImage Is Nothing) Then
                        If Me.IsLocalResource Then
                            Me.mBaseImage = TryCast(AwardImages.ResourceManager.GetObject(String.Format("_{0}0", Me.Name.Replace(" ", ""))),Bitmap)
                        Else
                            Using stream As MemoryStream = New MemoryStream(New WebClient().DownloadData(Me.BaseImageUrl))
                                Me.mBaseImage = Image.FromStream(stream)
                            End Using
                        End If
                    End If
                    mBaseImage = Me.mBaseImage
                Catch exception As Exception
                    ErrorLog.WriteLine(exception)
                    mBaseImage = Nothing
                Finally
                    Monitor.Exit(obj2)
                End Try
                Return mBaseImage
            End Get
        End Property

        Public ReadOnly Property BaseImageUrl As String
            Get
                Return String.Format("{0}/{1}/0.png", ConfigSettings.GetString("AwardsBaseUrl", "http://thevault.gaspowered.com/awards"), Me.Name)
            End Get
        End Property

        Public ReadOnly Property Category As AwardCategory
            Get
                Return AwardCategory.AllAwardCategories.Item(Me.mCategory)
            End Get
        End Property

        Public ReadOnly Property Description As String
            Get
                Return Loc.Get(("<LOC>" & Me.mDescription))
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

        Public ReadOnly Property Name As String
            Get
                Return Me.mName
            End Get
        End Property

        Public ReadOnly Property SortOrder As Integer
            Get
                Return Me.mSortOrder
            End Get
        End Property


        ' Fields
        Private Shared AllAwardSetsMutex As Object = New Object
        Private BaseImageMutex As Object
        Public Shared mAllAwardSets As Dictionary(Of Integer, AwardSet) = Nothing
        <FieldMap("always_show")> _
        Private mAlwaysShow As Boolean
        Private mAwards As List(Of Award)
        Private mBaseImage As Image
        <FieldMap("category")> _
        Private mCategory As Integer
        <FieldMap("description")> _
        Private mDescription As String
        <FieldMap("award_set_id")> _
        Private mID As Integer
        <FieldMap("is_local_resource")> _
        Private mIsLocalResource As Boolean
        <FieldMap("name")> _
        Private mName As String
        <FieldMap("sort_order")> _
        Private mSortOrder As Integer
    End Class
End Namespace

