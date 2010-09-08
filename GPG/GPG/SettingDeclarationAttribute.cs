namespace GPG
{
    using System;

    [Serializable]
    public class SettingDeclarationAttribute : Attribute
    {
        private Version mDeclaredVersion = Assembly.GetEntryAssembly().GetName().Version;

        public Version DeclaredVersion
        {
            get
            {
                return this.mDeclaredVersion;
            }
        }
    }
}

