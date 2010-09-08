Imports System.CodeDom.Compiler
Imports System.Configuration
Imports System.Runtime.CompilerServices

Namespace GPG.Multiplayer.Statistics.Properties
    <GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0"), CompilerGenerated> _
    Friend NotInheritable Class Settings
        Inherits ApplicationSettingsBase
        ' Properties
        Public Shared ReadOnly Property [Default] As Settings
            Get
                Return Settings.defaultInstance
            End Get
        End Property


        ' Fields
        Private Shared defaultInstance As Settings = DirectCast(SettingsBase.Synchronized(New Settings), Settings)
    End Class
End Namespace

