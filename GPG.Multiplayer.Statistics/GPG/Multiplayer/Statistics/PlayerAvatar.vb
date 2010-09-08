Imports GPG.DataAccess
Imports GPG.Multiplayer.Quazal
Imports System

Namespace GPG.Multiplayer.Statistics
    Public Class PlayerAvatar
        Inherits MappedObject
        ' Methods
        Private Sub New()
        End Sub

        Public Sub New(ByVal record As DataRecord)
            MyBase.New(record)
        End Sub

        Public Shared Function GetPlayerAvatars(ByVal playerId As Integer) As PlayerAvatar()
            Dim item As New PlayerAvatar
            item.mPlayerID = playerId
            item.mAvatarID = 0
            Dim objects As MappedObjectList(Of PlayerAvatar) = New QuazalQuery("GetPlayerAvatarsByID", New Object() { playerId }).GetObjects(Of PlayerAvatar)
            objects.Insert(0, item)
            Return objects.ToArray
        End Function


        ' Properties
        Public ReadOnly Property Avatar As Avatar
            Get
                If (Me.mAvatarID <= 0) Then
                    Return Avatar.Default
                End If
                Return Avatar.AllAvatars.Item(Me.mAvatarID)
            End Get
        End Property

        Public ReadOnly Property ID As Integer
            Get
                Return Me.mID
            End Get
        End Property

        Public ReadOnly Property ManualAssignment As Boolean
            Get
                Return Me.mManualAssignment
            End Get
        End Property

        Public ReadOnly Property PlayerID As Integer
            Get
                Return Me.mPlayerID
            End Get
        End Property


        ' Fields
        <FieldMap("avatar_id")> _
        Private mAvatarID As Integer
        <FieldMap("player_avatar_id")> _
        Private mID As Integer
        <FieldMap("manual_assignment")> _
        Private mManualAssignment As Boolean
        <FieldMap("player_id")> _
        Private mPlayerID As Integer
    End Class
End Namespace

