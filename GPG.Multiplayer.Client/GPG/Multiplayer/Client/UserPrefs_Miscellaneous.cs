namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Miscellaneous
    {
        private bool mAutoCloseDownloadDialog = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AutoCloseDownloadDialogChanged;

        [DisplayName("<LOC>Auto-Close Download Dialog"), Description("<LOC>"), Category("<LOC>Misc")]
        public bool AutoCloseDownloadDialog
        {
            get
            {
                return this.mAutoCloseDownloadDialog;
            }
            set
            {
                this.mAutoCloseDownloadDialog = value;
                if (this.mAutoCloseDownloadDialogChanged != null)
                {
                    this.mAutoCloseDownloadDialogChanged(this, new PropertyChangedEventArgs("AutoCloseDownloadDialog"));
                }
            }
        }
    }
}

