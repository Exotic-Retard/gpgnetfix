Imports GPG.DataAccess
Imports GPG.Multiplayer.Quazal
Imports System

Namespace GPG.Multiplayer.Statistics
    Public Class PlayerDisplayAwards
        Inherits MappedObject
        ' Methods
        Public Sub New(ByVal record As DataRecord)
            MyBase.New(record)
        End Sub

        Public Sub New(ByVal player As User)
            Me.mPlayerID = player.ID
            Me.mAward1 = player.Award1
            Me.mAward2 = player.Award2
            Me.mAward3 = player.Award3
            Me.mAvatarID = player.Avatar
        End Sub


        ' Properties
        Public ReadOnly Property Avatar As Avatar
            Get
                If ((Me.mAvatarID > 0) AndAlso Avatar.AllAvatars.ContainsKey(Me.mAvatarID)) Then
                    Return Avatar.AllAvatars.Item(Me.mAvatarID)
                End If
                Return Avatar.Default
            End Get
        End Property

        Public ReadOnly Property Award1 As Award
            Get
                If ((Me.mAward1 > 0) AndAlso Award.AllAwards.ContainsKey(Me.mAward1)) Then
                    Return Award.AllAwards.Item(Me.mAward1)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property Award1Specified As Boolean
            Get
                Return (Not Me.Award1 Is Nothing)
            End Get
        End Property

        Public ReadOnly Property Award2 As Award
            Get
                If ((Me.mAward2 > 0) AndAlso Award.AllAwards.ContainsKey(Me.mAward2)) Then
                    Return Award.AllAwards.Item(Me.mAward2)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property Award2Specified As Boolean
            Get
                Return (Not Me.Award2 Is Nothing)
            End Get
        End Property

        Public ReadOnly Property Award3 As Award
            Get
                If ((Me.mAward3 > 0) AndAlso Award.AllAwards.ContainsKey(Me.mAward3)) Then
                    Return Award.AllAwards.Item(Me.mAward3)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property Award3Specified As Boolean
            Get
                Return (Not Me.Award3 Is Nothing)
            End Get
        End Property

        Public ReadOnly Property ID As Integer
            Get
                Return Me.mID
            End Get
        End Property

        Public ReadOnly Property PlayerID As Integer
            Get
                Return Me.mPlayerID
            End Get
        End Property


        ' Fields
        <FieldMap("avatar")> _
        Private mAvatarID As Integer
        <FieldMap("award1")> _
        Private mAward1 As Integer
        <FieldMap("award2")> _
        Private mAward2 As Integer
        <FieldMap("award3")> _
        Private mAward3 As Integer
        <FieldMap("player_display_awards_id")> _
        Private mID As Integer
        <FieldMap("player_id")> _
        Private mPlayerID As Integer
    End Class
End Namespace

