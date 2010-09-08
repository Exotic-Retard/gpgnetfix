namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Awards
    {
        private bool mShowAvatars = true;
        private bool mShowAwards = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowAvatarsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowAwardsChanged;

        [DisplayName("<LOC>Show Avatars"), Category("<LOC>Misc"), Description("<LOC>Toggles the display of player avatars in chat and profiles.")]
        public bool ShowAvatars
        {
            get
            {
                return this.mShowAvatars;
            }
            set
            {
                this.mShowAvatars = value;
                if (this.mShowAvatarsChanged != null)
                {
                    this.mShowAvatarsChanged(this, new PropertyChangedEventArgs("ShowAvatars"));
                }
            }
        }

        [Description("<LOC>Toggles the display of awards in chat and profiles."), Category("<LOC>Misc"), DisplayName("<LOC>Show Awards")]
        public bool ShowAwards
        {
            get
            {
                return this.mShowAwards;
            }
            set
            {
                this.mShowAwards = value;
                if (this.mShowAwardsChanged != null)
                {
                    this.mShowAwardsChanged(this, new PropertyChangedEventArgs("ShowAwards"));
                }
            }
        }
    }
}

