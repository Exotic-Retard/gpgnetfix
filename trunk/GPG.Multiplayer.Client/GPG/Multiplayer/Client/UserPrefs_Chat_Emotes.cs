namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Chat_Emotes
    {
        private bool mAnimateEmotes = false;
        private int mAnimationThreshhold = 100;
        private bool mAutoShareEmotes = true;
        private string mEmoteImageDir = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private bool mShowCreateWarning = true;
        private bool mShowEmotes = true;
        private bool mShowViewWarning = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AnimateEmotesChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AnimationThreshholdChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AutoShareEmotesChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EmoteImageDirChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowCreateWarningChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowEmotesChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowViewWarningChanged;

        [Category("<LOC>Misc"), Description("<LOC>If true, emotes with animation sets will be animated in chat. WARNING: This can have an effect on performance if too many animations play at once. Try adjusting the Animation Threshhold for better performance."), DisplayName("<LOC>AnimateEmotes")]
        public bool AnimateEmotes
        {
            get
            {
                return this.mAnimateEmotes;
            }
            set
            {
                this.mAnimateEmotes = value;
                if (this.AnimateEmotesChanged != null)
                {
                    this.AnimateEmotesChanged(this, new PropertyChangedEventArgs("AnimateEmotes"));
                }
            }
        }

        [Description("<LOC>The number of animated emotes that can be shown before the animation sets start skipping frames. Adjust to a lower number for improved performance but choppier animation in large quantities of animated emotes."), Category("<LOC>Misc"), DisplayName("<LOC>Animation Threshhold")]
        public int AnimationThreshhold
        {
            get
            {
                return this.mAnimationThreshhold;
            }
            set
            {
                if (value < 10)
                {
                    value = 10;
                }
                else if (value > 0x3e8)
                {
                    value = 0x3e8;
                }
                this.mAnimationThreshhold = value;
                if (this.AnimationThreshholdChanged != null)
                {
                    this.AnimationThreshholdChanged(this, new PropertyChangedEventArgs("AnimationThreshhold"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>Automatically shares custom emotes that you have created by prodiving a link to other users that see your chat."), DisplayName("<LOC>Auto Share Emotes")]
        public bool AutoShareEmotes
        {
            get
            {
                return this.mAutoShareEmotes;
            }
            set
            {
                this.mAutoShareEmotes = value;
                if (this.AutoShareEmotesChanged != null)
                {
                    this.AutoShareEmotesChanged(this, new PropertyChangedEventArgs("AutoShareEmotes"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>Creating new emotes; default directory for images."), DisplayName("<LOC>Image Input Folder")]
        public string EmoteImageDir
        {
            get
            {
                return this.mEmoteImageDir;
            }
            set
            {
                this.mEmoteImageDir = value;
                if (this.EmoteImageDirChanged != null)
                {
                    this.EmoteImageDirChanged(this, new PropertyChangedEventArgs("EmoteImageDir"));
                }
            }
        }

        [Browsable(false), Category("<LOC>Misc"), DisplayName("<LOC>ShowCreateWarning"), Description("<LOC>")]
        public bool ShowCreateWarning
        {
            get
            {
                return this.mShowCreateWarning;
            }
            set
            {
                this.mShowCreateWarning = value;
                if (this.ShowCreateWarningChanged != null)
                {
                    this.ShowCreateWarningChanged(this, new PropertyChangedEventArgs("ShowCreateWarning"));
                }
            }
        }

        [Description("<LOC>Toggles visibility of emotes in chat."), Category("<LOC>Misc"), DisplayName("<LOC>Show Emotes")]
        public bool ShowEmotes
        {
            get
            {
                return this.mShowEmotes;
            }
            set
            {
                this.mShowEmotes = value;
                if (this.ShowEmotesChanged != null)
                {
                    this.ShowEmotesChanged(this, new PropertyChangedEventArgs("ShowEmotes"));
                }
            }
        }

        [Category("<LOC>Misc"), Browsable(false), DisplayName("<LOC>ShowViewWarning"), Description("<LOC>")]
        public bool ShowViewWarning
        {
            get
            {
                return this.mShowViewWarning;
            }
            set
            {
                this.mShowViewWarning = value;
                if (this.ShowViewWarningChanged != null)
                {
                    this.ShowViewWarningChanged(this, new PropertyChangedEventArgs("ShowViewWarning"));
                }
            }
        }
    }
}

