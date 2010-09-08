Imports GPG.DataAccess
Imports GPG.Multiplayer.Quazal
Imports System
Imports System.Collections.Generic

Namespace GPG.Multiplayer.Statistics
    Public Class AwardCategory
        Inherits MappedObject
        ' Methods
        Public Sub New(ByVal record As DataRecord)
            MyBase.New(record)
            Me.mSortOrder = -1
        End Sub

        Public Shared Sub ClearCachedData()
            AwardCategory.mAllAwardCategories = Nothing
        End Sub


        ' Properties
        Public Shared ReadOnly Property AllAwardCategories As Dictionary(Of Integer, AwardCategory)
            Get
                If (AwardCategory.mAllAwardCategories Is Nothing) Then
                    AwardCategory.mAllAwardCategories = New Dictionary(Of Integer, AwardCategory)
                    Dim category As AwardCategory
                    For Each category In New QuazalQuery("GetAllAwardCategories", New Object(0  - 1) {}).GetObjects(Of AwardCategory)
                        AwardCategory.mAllAwardCategories.Add(category.ID, category)
                    Next
                End If
                Return AwardCategory.mAllAwardCategories
            End Get
        End Property

        Public ReadOnly Property ID As Integer
            Get
                Return Me.mID
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
        Public Shared mAllAwardCategories As Dictionary(Of Integer, AwardCategory)
        <FieldMap("award_category_id")> _
        Private mID As Integer
        <FieldMap("name")> _
        Private mName As String
        <FieldMap("sort_order")> _
        Private mSortOrder As Integer
    End Class
End Namespace

