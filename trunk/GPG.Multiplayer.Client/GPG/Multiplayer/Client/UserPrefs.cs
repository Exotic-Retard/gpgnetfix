namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Forms;

    [Serializable]
    public class UserPrefs : ProgramSettings
    {
        private UserPrefs_Appearance mAppearance = null;
        private UserPrefs_Awards mAwards = null;
        private UserPrefs_Chat mChat = new UserPrefs_Chat();
        private UserPrefs_Content mContent = new UserPrefs_Content();
        private UserPrefs_Login mLogin = new UserPrefs_Login();
        private UserPrefs_Miscellaneous mMiscellaneous = new UserPrefs_Miscellaneous();
        private PopupPrefs mPopupPreferences = new PopupPrefs();
        private UserPrefs_Profiles mProfiles = new UserPrefs_Profiles();
        private string mSkinName = "default";
        private UserPrefs_Sound mSound;
        private StylePrefs mStylePreferences = new StylePrefs();
        private GPG.Multiplayer.Client.Games.SupremeCommander.SupcomPrefs mSupcomPrefs = new GPG.Multiplayer.Client.Games.SupremeCommander.SupcomPrefs();
        private UserPrefs_WebBrowsing mWebBrowsing = new UserPrefs_WebBrowsing();

        [field: NonSerialized]
        public event PropertyChangedEventHandler SkinNameChanged;

        public static UserPrefs Load()
        {
            return LoadFrom(ProgramSettings.SAVE_DEFAULT);
        }

        public static UserPrefs LoadFrom(string path)
        {
            ProgramSettings settings = ProgramSettings.LoadFrom(path);
            if ((settings != null) && (settings is UserPrefs))
            {
                UserPrefs prefs = settings as UserPrefs;
                prefs.LoadSkin();
                return prefs;
            }
            return new UserPrefs();
        }

        private void LoadSkin()
        {
            if ((this.mSkinName == null) || (this.mSkinName.Trim() == ""))
            {
                this.mSkinName = "default";
            }
            string path = (Application.StartupPath + @"\skins\" + this.mSkinName + @"\") + "skindata.gpg";
            if (File.Exists(path))
            {
                ProgramSettings settings = ProgramSettings.LoadFrom(path);
                if ((settings != null) && (settings is StylePrefs))
                {
                    this.mStylePreferences = settings as StylePrefs;
                }
                else
                {
                    this.mStylePreferences = new StylePrefs();
                }
            }
            else
            {
                this.mStylePreferences = new StylePrefs();
            }
        }

        public override void Save()
        {
            this.SaveAs(base.FilePath);
        }

        public override void SaveAs(string path)
        {
            try
            {
                this.SaveSkin();
                base.SaveAs(path);
                EventLog.WriteLine("Settings Saved", new object[0]);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void SaveSkin()
        {
            string path = Application.StartupPath + @"\skins\" + this.mSkinName + @"\";
            string str2 = path + "skindata.gpg";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.StylePreferences.SaveAs(str2);
        }

        [OptionsRoot("<LOC>Appearance")]
        public UserPrefs_Appearance Appearance
        {
            get
            {
                if (this.mAppearance == null)
                {
                    this.mAppearance = new UserPrefs_Appearance(this);
                }
                return this.mAppearance;
            }
        }

        [OptionsRoot("<LOC>Awards")]
        public UserPrefs_Awards Awards
        {
            get
            {
                if (this.mAwards == null)
                {
                    this.mAwards = new UserPrefs_Awards();
                }
                return this.mAwards;
            }
        }

        [OptionsRoot("<LOC>Chat")]
        public UserPrefs_Chat Chat
        {
            get
            {
                if (this.mChat == null)
                {
                    this.mChat = new UserPrefs_Chat();
                }
                return this.mChat;
            }
        }

        [Browsable(false)]
        public UserPrefs_Content Content
        {
            get
            {
                if (this.mContent == null)
                {
                    this.mContent = new UserPrefs_Content();
                }
                return this.mContent;
            }
            set
            {
                this.mContent = value;
            }
        }

        [OptionsRoot("<LOC>Login")]
        public UserPrefs_Login Login
        {
            get
            {
                if (this.mLogin == null)
                {
                    this.mLogin = new UserPrefs_Login();
                }
                return this.mLogin;
            }
        }

        [OptionsRoot("<LOC>Miscellaneous")]
        public UserPrefs_Miscellaneous Miscellaneous
        {
            get
            {
                if (this.mMiscellaneous == null)
                {
                    this.mMiscellaneous = new UserPrefs_Miscellaneous();
                }
                return this.mMiscellaneous;
            }
        }

        [OptionsRoot("<LOC>Popup Dialog")]
        public PopupPrefs PopupPreferences
        {
            get
            {
                if (this.mPopupPreferences == null)
                {
                    this.mPopupPreferences = new PopupPrefs();
                }
                return this.mPopupPreferences;
            }
        }

        [OptionsRoot("<LOC>Profiles")]
        public UserPrefs_Profiles Profiles
        {
            get
            {
                if (this.mProfiles == null)
                {
                    this.mProfiles = new UserPrefs_Profiles();
                }
                return this.mProfiles;
            }
        }

        [DisplayName("<LOC>Skin Name"), Category("<LOC>Misc"), Description("<LOC>The name of the skin for GPGnet: Supreme Commander")]
        public string SkinName
        {
            get
            {
                return this.mSkinName;
            }
            set
            {
                this.SaveSkin();
                this.mSkinName = value;
                this.LoadSkin();
                if (this.SkinNameChanged != null)
                {
                    this.SkinNameChanged(this, new PropertyChangedEventArgs("SkinName"));
                }
            }
        }

        public UserPrefs_Sound Sound
        {
            get
            {
                if (this.mSound == null)
                {
                    this.mSound = new UserPrefs_Sound();
                }
                return this.mSound;
            }
        }

        [OptionsRoot("<LOC>Application Styling")]
        public StylePrefs StylePreferences
        {
            get
            {
                if (this.mStylePreferences == null)
                {
                    this.mStylePreferences = new StylePrefs();
                }
                return this.mStylePreferences;
            }
        }

        [OptionsRoot("<LOC>Game Settings")]
        public GPG.Multiplayer.Client.Games.SupremeCommander.SupcomPrefs SupcomPrefs
        {
            get
            {
                if (this.mSupcomPrefs == null)
                {
                    this.mSupcomPrefs = new GPG.Multiplayer.Client.Games.SupremeCommander.SupcomPrefs();
                }
                return this.mSupcomPrefs;
            }
        }

        public UserPrefs_WebBrowsing WebBrowsing
        {
            get
            {
                if (this.mWebBrowsing == null)
                {
                    this.mWebBrowsing = new UserPrefs_WebBrowsing();
                }
                return this.mWebBrowsing;
            }
        }
    }
}

