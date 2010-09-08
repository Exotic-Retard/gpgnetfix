namespace GPG.Multiplayer.Quazal.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
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

        [DefaultSettingValue("http://gpgnet.gaspowered.com/gamereport/Service.asmx"), DebuggerNonUserCode, ApplicationScopedSetting, SpecialSetting(SpecialSetting.WebServiceUrl)]
        public string GPG_Multiplayer_Quazal_com_gaspowered_gpgnet_Service
        {
            get
            {
                return (string) this["GPG_Multiplayer_Quazal_com_gaspowered_gpgnet_Service"];
            }
        }

        [DebuggerNonUserCode, DefaultSettingValue("http://thevault.gaspowered.com/vault/Service.asmx"), SpecialSetting(SpecialSetting.WebServiceUrl), ApplicationScopedSetting]
        public string GPG_Multiplayer_Quazal_com_gaspowered_gpgnet_vault_Service
        {
            get
            {
                return (string) this["GPG_Multiplayer_Quazal_com_gaspowered_gpgnet_vault_Service"];
            }
        }

        [SpecialSetting(SpecialSetting.WebServiceUrl), ApplicationScopedSetting, DebuggerNonUserCode, DefaultSettingValue("http://gpgnet.gaspowered.com/SubmitBetaReplay/service.asmx")]
        public string GPG_Multiplayer_Quazal_com_gaspowered_gpgnet1_Service
        {
            get
            {
                return (string) this["GPG_Multiplayer_Quazal_com_gaspowered_gpgnet1_Service"];
            }
        }

        [SpecialSetting(SpecialSetting.WebServiceUrl), DefaultSettingValue("http://gpgnet.gaspowered.com/quazal/Service.asmx"), DebuggerNonUserCode, ApplicationScopedSetting]
        public string GPG_Multiplayer_Quazal_SolutionsLib_Service
        {
            get
            {
                return (string) this["GPG_Multiplayer_Quazal_SolutionsLib_Service"];
            }
        }
    }
}

