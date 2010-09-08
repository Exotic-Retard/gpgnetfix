namespace GPG.Multiplayer.Client
{
    using GPG;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Login
    {
        [NonSerialized]
        private string mAccessKey = "dr67ik";
        private bool mAutoLogin = false;
        private int mDefaultGameID = -1;
        private string mDefaultPassword = "";
        [NonSerialized]
        private int mDefaultPort = 0x2a17;
        [NonSerialized]
        private string mDefaultServer = "supremecom2.quazal.net";
        private string mDefaultServerName = "Dev Server";
        private string mDefaultUsername = "";
        private bool mIgnoreAutolaunch = false;
        private bool mRememberCredentials = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AutoLoginChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DefaultUsernameChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler IgnoreAutolaunchChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler RememberCredentialsChanged;

        [Browsable(false)]
        public string AccessKey
        {
            get
            {
                return this.mAccessKey;
            }
            set
            {
                this.mAccessKey = value;
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>Determines if the application should automatically try to login on launch."), DisplayName("<LOC>Auto-Login")]
        public bool AutoLogin
        {
            get
            {
                return this.mAutoLogin;
            }
            set
            {
                this.mAutoLogin = value;
                if (this.mAutoLoginChanged != null)
                {
                    this.mAutoLoginChanged(this, new PropertyChangedEventArgs("AutoLogin"));
                }
            }
        }

        [Browsable(false)]
        public int DefaultGameID
        {
            get
            {
                return this.mDefaultGameID;
            }
            set
            {
                this.mDefaultGameID = value;
            }
        }

        [Browsable(false), SaveLoadFormatter(typeof(SaveLoad_DefaultPassword))]
        public string DefaultPassword
        {
            get
            {
                return this.mDefaultPassword;
            }
            set
            {
                this.mDefaultPassword = value;
            }
        }

        [Browsable(false)]
        public int DefaultPort
        {
            get
            {
                return this.mDefaultPort;
            }
            set
            {
                this.mDefaultPort = value;
            }
        }

        [Browsable(false)]
        public string DefaultServer
        {
            get
            {
                return this.mDefaultServer;
            }
            set
            {
                this.mDefaultServer = value;
            }
        }

        [Browsable(false)]
        public string DefaultServerName
        {
            get
            {
                if (this.mDefaultServerName == null)
                {
                    return "Dev Server";
                }
                return this.mDefaultServerName;
            }
            set
            {
                this.mDefaultServerName = value;
            }
        }

        [DisplayName("<LOC>Default Username"), Category("<LOC>Misc"), Description("<LOC>The username stored when 'Remember Credentials' is set to true.")]
        public string DefaultUsername
        {
            get
            {
                return this.mDefaultUsername;
            }
            set
            {
                this.mDefaultUsername = value;
                if (this.mDefaultUsernameChanged != null)
                {
                    this.mDefaultUsernameChanged(this, new PropertyChangedEventArgs("DefaultUsername"));
                }
            }
        }

        [Category("<LOC>Misc"), Description(""), DisplayName("<LOC>Ignore Auto Launch")]
        public bool IgnoreAutolaunch
        {
            get
            {
                return this.mIgnoreAutolaunch;
            }
            set
            {
                this.mIgnoreAutolaunch = value;
                if (this.mIgnoreAutolaunchChanged != null)
                {
                    this.mIgnoreAutolaunchChanged(this, new PropertyChangedEventArgs("IgnoreAutolaunch"));
                }
            }
        }

        [DisplayName("<LOC>Remember Credentials"), Category("<LOC>Misc"), Description("<LOC>Toggles system to remember username and password when you logout.")]
        public bool RememberCredentials
        {
            get
            {
                return this.mRememberCredentials;
            }
            set
            {
                this.mRememberCredentials = value;
                if (this.mRememberCredentialsChanged != null)
                {
                    this.mRememberCredentialsChanged(this, new PropertyChangedEventArgs("RememberCredentials"));
                }
            }
        }

        private class SaveLoad_DefaultPassword : ProgramSettings.ISaveLoadFormatter
        {
            public object OnSaveLoad(object data, ProgramSettings.SaveLoadDirections direction)
            {
                if (data != null)
                {
                    if (direction == ProgramSettings.SaveLoadDirections.Load)
                    {
                        return Encryption.Decode(data.ToString(), EncodingMethods.Base64);
                    }
                    if (direction == ProgramSettings.SaveLoadDirections.Save)
                    {
                        return Encryption.Encode(data.ToString(), EncodingMethods.Base64);
                    }
                }
                return data;
            }
        }
    }
}

