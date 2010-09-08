namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG;
    using GPG.Multiplayer.Game.SupremeCommander;
    using GPG.Multiplayer.Quazal;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    [Serializable]
    public class SupcomPrefs
    {
        public bool LocationChanged = false;
        private bool mAutoDetectExe = true;
        private string mCommandLineArgs = "";
        private string mGamePath = "";
        private Dictionary<string, string> mGamePaths = new Dictionary<string, string>();
        private int mGamePort = 0x17e0;
        private UserPrefs_Games_Supcom_RankedGames mRankedGames = new UserPrefs_Games_Supcom_RankedGames();
        private bool mRecordStats = false;
        private UserPrefs_Games_Supcom_Replays mReplays = new UserPrefs_Games_Supcom_Replays();
        private bool mSaveLiveReplay = false;
        private bool mShowPatchMsg = true;
        private string mStatsDir = (Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Games\Gas Powered Games\Game Stats");
        [NonSerialized]
        private string tempGamePath = "";

        [field: NonSerialized]
        public event PropertyChangedEventHandler AutoDetectExeChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler CommandLineArgsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler GamePathChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler GamePortChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler RecordStatsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SaveLiveReplayChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowPatchMsgChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StatsDirChanged;

        [OnDeserialized]
        private void Deserialized(StreamingContext context)
        {
            SupComGameManager.sLiveReplayMode = this.mSaveLiveReplay;
            SupcomStdInOut.GamePort = this.mGamePort;
            SupComGameManager.GameArgs = this.mCommandLineArgs;
            StatsWatcher.StatsLocation = this.mStatsDir;
            StatsWatcher.SaveStats = this.mRecordStats;
            GameItem.SupcomPath = this.GetGamePath();
            GameItem.SupcomExe = this.GameExe;
        }

        private string GetGamePath()
        {
            string mGamePath = this.mGamePath;
            try
            {
                switch (mGamePath)
                {
                    case null:
                        mGamePath = "";
                        break;

                    case "":
                        if (File.Exists(this.GetRegistryPath()))
                        {
                            mGamePath = this.GetRegistryPath();
                        }
                        else if (File.Exists(@"C:\Program Files\THQ\Gas Powered Games\Supreme Commander\bin\SupremeCommander.exe"))
                        {
                            mGamePath = @"C:\Program Files\THQ\Gas Powered Games\Supreme Commander\bin\SupremeCommander.exe";
                        }
                        else if (File.Exists(@"D:\Program Files\THQ\Gas Powered Games\Supreme Commander\bin\SupremeCommander.exe"))
                        {
                            mGamePath = @"D:\Program Files\THQ\Gas Powered Games\Supreme Commander\bin\SupremeCommander.exe";
                        }
                        goto Label_0079;
                }
            Label_0079:
                if (mGamePath != "")
                {
                    if (File.Exists(mGamePath))
                    {
                        GameItem.SupcomPath = mGamePath;
                        this.tempGamePath = mGamePath;
                    }
                    else
                    {
                        mGamePath = "";
                    }
                }
                this.mGamePath = mGamePath;
                return mGamePath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string GetRegistryPath()
        {
            string str = "";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\THQ\Gas Powered Games\Supreme Commander");
            if (key == null)
            {
                key = Registry.LocalMachine.OpenSubKey(@"Software\THQ\Gas Powered Games\Supreme Commander");
            }
            if (key != null)
            {
                str = (string) key.GetValue("InstallationDirectory");
                str = str + @"\Supreme Commander\bin\SupremeCommander.exe";
            }
            return str;
        }

        public static void TestFactions(out bool hasOriginal, out bool hasExpansion)
        {
            GPGnetSelectedGame.TestFactions(out hasOriginal, out hasExpansion);
        }

        [Description("<LOC>Toggles automatic detection of the game executable from common locations."), DisplayName("<LOC>Auto Detect Executable"), Category("<LOC>Misc")]
        public bool AutoDetectExe
        {
            get
            {
                return this.mAutoDetectExe;
            }
            set
            {
                this.mAutoDetectExe = value;
                if (this.mAutoDetectExeChanged != null)
                {
                    this.mAutoDetectExeChanged(this, new PropertyChangedEventArgs("AutoDetectExe"));
                }
            }
        }

        [DisplayName("<LOC>Command Line Arguments"), Category("<LOC>Misc")]
        public string CommandLineArgs
        {
            get
            {
                if (this.mCommandLineArgs == null)
                {
                    this.mCommandLineArgs = "";
                }
                return this.mCommandLineArgs;
            }
            set
            {
                this.mCommandLineArgs = value;
                SupComGameManager.GameArgs = this.mCommandLineArgs;
                if (this.mCommandLineArgsChanged != null)
                {
                    this.mCommandLineArgsChanged(this, new PropertyChangedEventArgs("CommandLineArgs"));
                }
            }
        }

        public string GameExe
        {
            get
            {
                try
                {
                    string gamePath = this.GetGamePath();
                    GameItem.SupcomPath = gamePath;
                    string[] strArray = gamePath.Split(@"/\".ToCharArray());
                    string str2 = "";
                    if (strArray.Length > 0)
                    {
                        str2 = strArray[strArray.Length - 1];
                    }
                    else
                    {
                        str2 = gamePath;
                    }
                    GameItem.SupcomExe = str2;
                    return str2;
                }
                catch
                {
                    return "";
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Game Path")]
        public string GamePath
        {
            get
            {
                string gamePath = this.GetGamePath();
                GameItem.SupcomExe = this.GameExe;
                return gamePath;
            }
            set
            {
                if (this.mGamePath != value)
                {
                    this.LocationChanged = true;
                    this.mGamePath = value;
                    GameItem.SupcomExe = this.GameExe;
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Game Port"), Description("")]
        public int GamePort
        {
            get
            {
                if (this.mGamePort == 0)
                {
                    this.mGamePort = 0x17e0;
                }
                return this.mGamePort;
            }
            set
            {
                this.mGamePort = value;
                if (this.mGamePortChanged != null)
                {
                    this.mGamePortChanged(this, new PropertyChangedEventArgs("GamePort"));
                }
                SupcomStdInOut.GamePort = this.mGamePort;
            }
        }

        [Browsable(false)]
        public UserPrefs_Games_Supcom_RankedGames RankedGames
        {
            get
            {
                if (this.mRankedGames == null)
                {
                    this.mRankedGames = new UserPrefs_Games_Supcom_RankedGames();
                }
                return this.mRankedGames;
            }
        }

        [Description("<LOC>Save XML statistics from all games.  This will appear in your Statistics Save Folder."), DisplayName("<LOC>Save Statistics"), Category("<LOC>Misc")]
        public bool RecordStats
        {
            get
            {
                return this.mRecordStats;
            }
            set
            {
                this.mRecordStats = value;
                if (this.mRecordStatsChanged != null)
                {
                    this.mRecordStatsChanged(this, new PropertyChangedEventArgs("RecordStats"));
                }
                StatsWatcher.SaveStats = this.mRecordStats;
            }
        }

        [OptionsRoot("<LOC>Replays"), Browsable(false)]
        public UserPrefs_Games_Supcom_Replays Replays
        {
            get
            {
                if (this.mReplays == null)
                {
                    this.mReplays = new UserPrefs_Games_Supcom_Replays();
                }
                return this.mReplays;
            }
        }

        [Description("<LOC>Saving a Live Replay will stream through ReplayBot, and stops regular replays from saving."), Category("<LOC>Misc"), DisplayName("<LOC>Save Live Replay")]
        public bool SaveLiveReplay
        {
            get
            {
                return this.mSaveLiveReplay;
            }
            set
            {
                this.mSaveLiveReplay = value;
                if (this.mSaveLiveReplayChanged != null)
                {
                    this.mSaveLiveReplayChanged(this, new PropertyChangedEventArgs("SaveLiveReplay"));
                    SupComGameManager.sLiveReplayMode = this.mSaveLiveReplay;
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Show Patch Message")]
        public bool ShowPatchMsg
        {
            get
            {
                return this.mShowPatchMsg;
            }
            set
            {
                this.mShowPatchMsg = value;
                if (this.mShowPatchMsgChanged != null)
                {
                    this.mShowPatchMsgChanged(this, new PropertyChangedEventArgs("ShowPatchMsg"));
                }
            }
        }

        [Description("<LOC>The default location to save statistics to."), DisplayName("<LOC>Statistics Save Folder"), Category("<LOC>Misc")]
        public string StatsDir
        {
            get
            {
                return this.mStatsDir;
            }
            set
            {
                this.mStatsDir = value;
                if (this.mStatsDirChanged != null)
                {
                    this.mStatsDirChanged(this, new PropertyChangedEventArgs("StatsDir"));
                }
                StatsWatcher.StatsLocation = this.mStatsDir;
            }
        }
    }
}

