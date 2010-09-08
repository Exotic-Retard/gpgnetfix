namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Profiles
    {
        private bool mShowWebVersions = false;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowWebVersionsChanged;

        [Description("<LOC>If true, clan profiles will display their website within their profile when one is available. Warning: websites shown are not pre-screened, view them at your own risk."), Category("<LOC>Clan"), DisplayName("<LOC>Show Web Versions")]
        public bool ShowWebVersions
        {
            get
            {
                return this.mShowWebVersions;
            }
            set
            {
                this.mShowWebVersions = value;
                if (this.ShowWebVersionsChanged != null)
                {
                    this.ShowWebVersionsChanged(this, new PropertyChangedEventArgs("ShowWebVersions"));
                }
            }
        }
    }
}

