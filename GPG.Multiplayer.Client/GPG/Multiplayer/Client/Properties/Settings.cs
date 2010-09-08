namespace GPG.Multiplayer.Client.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0"), CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [DefaultSettingValue("http://replay.gaspowered.com/emailservice/service.asmx"), ApplicationScopedSetting, DebuggerNonUserCode, SpecialSetting(SpecialSetting.WebServiceUrl)]
        public string GPG_Multiplayer_Client_EmailService_Service
        {
            get
            {
                return (string) this["GPG_Multiplayer_Client_EmailService_Service"];
            }
        }

        [DebuggerNonUserCode, ApplicationScopedSetting]
        public Size Setting
        {
            get
            {
                return (Size) this["Setting"];
            }
        }

        [UserScopedSetting, DebuggerNonUserCode]
        public Color Setting1
        {
            get
            {
                return (Color) this["Setting1"];
            }
            set
            {
                this["Setting1"] = value;
            }
        }
    }
}

