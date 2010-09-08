namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Games_Supcom_Replays
    {
        private ReplayDownloadActions mDefaultDownloadAction = ReplayDownloadActions.None;
        private bool mRememberDownloadAction = false;
        private string mReplaysDirectory = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Gas Powered Games\SupremeCommander\replays\");
        private bool mShowReplayDialog = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DefaultDownloadActionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler RememberDownloadActionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ReplaysDirectoryChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowReplayDialogChanged;

        [Browsable(false)]
        public ReplayDownloadActions DefaultDownloadAction
        {
            get
            {
                return this.mDefaultDownloadAction;
            }
            set
            {
                this.mDefaultDownloadAction = value;
                if (this.DefaultDownloadActionChanged != null)
                {
                    this.DefaultDownloadActionChanged(this, new PropertyChangedEventArgs("DefaultDownloadAction"));
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Remember Download Action"), Description("<LOC>Toggles the visibility of the open/save dialog that shows when you download a replay.")]
        public bool RememberDownloadAction
        {
            get
            {
                return this.mRememberDownloadAction;
            }
            set
            {
                this.mRememberDownloadAction = value;
                if (this.RememberDownloadActionChanged != null)
                {
                    this.RememberDownloadActionChanged(this, new PropertyChangedEventArgs("RememberDownloadAction"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>The default directory to save replays to."), DisplayName("<LOC>Replays Directory")]
        public string ReplaysDirectory
        {
            get
            {
                return this.mReplaysDirectory;
            }
            set
            {
                this.mReplaysDirectory = value;
                if (this.ReplaysDirectoryChanged != null)
                {
                    this.ReplaysDirectoryChanged(this, new PropertyChangedEventArgs("ReplaysDirectory"));
                }
            }
        }

        [Description("<LOC>Toggles the visibility of the submit replay dialog that shows at the end of a game."), Category("<LOC>Misc"), DisplayName("<LOC>Show Replay Dialog")]
        public bool ShowReplayDialog
        {
            get
            {
                return this.mShowReplayDialog;
            }
            set
            {
                this.mShowReplayDialog = value;
                if (this.ShowReplayDialogChanged != null)
                {
                    this.ShowReplayDialogChanged(this, new PropertyChangedEventArgs("ShowReplayDialog"));
                }
            }
        }
    }
}

