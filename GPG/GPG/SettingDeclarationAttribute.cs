namespace GPG
{
    using System;
    using System.Reflection;

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

