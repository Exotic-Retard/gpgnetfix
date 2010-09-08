Imports System
Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Globalization
Imports System.Resources
Imports System.Runtime.CompilerServices

Namespace GPG.Multiplayer.Statistics.Properties
    <GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), DebuggerNonUserCode, CompilerGenerated> _
    Friend Class Resources
        ' Methods
        Friend Sub New()
        End Sub


        ' Properties
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Friend Shared Property Culture As CultureInfo
            Get
                Return Resources.resourceCulture
            End Get
            Set(ByVal value As CultureInfo)
                Resources.resourceCulture = value
            End Set
        End Property

        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Friend Shared ReadOnly Property ResourceManager As ResourceManager
            Get
                If (Resources.resourceMan Is Nothing) Then
                    Dim manager As New ResourceManager("GPG.Multiplayer.Statistics.Properties.Resources", GetType(Resources).Assembly)
                    Resources.resourceMan = manager
                End If
                Return Resources.resourceMan
            End Get
        End Property


        ' Fields
        Private Shared resourceCulture As CultureInfo
        Private Shared resourceMan As ResourceManager
    End Class
End Namespace

