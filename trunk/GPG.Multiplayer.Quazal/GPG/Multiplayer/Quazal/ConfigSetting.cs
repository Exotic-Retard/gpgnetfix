namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using System;

    public class ConfigSetting : MappedObject
    {
        [FieldMap("config_key")]
        private string mConfigKey;
        [FieldMap("value")]
        private string mValue;

        public ConfigSetting(DataRecord record) : base(record)
        {
        }

        public string ConfigKey
        {
            get
            {
                return this.mConfigKey;
            }
            set
            {
                this.mConfigKey = value;
            }
        }

        public string Value
        {
            get
            {
                return this.mValue;
            }
            set
            {
                this.mValue = value;
            }
        }
    }
}

