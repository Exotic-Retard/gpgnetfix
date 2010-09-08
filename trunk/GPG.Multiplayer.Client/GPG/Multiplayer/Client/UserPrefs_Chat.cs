namespace GPG.Multiplayer.Client
{
    using GPG;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Chat
    {
        private UserPrefs_Chat_Appearance mAppearance;
        private int mAwayTimeout = 10;
        private int mChatHistoryLength = 15;
        private UserPrefs_Chat_Emotes mEmotes;
        private UserPrefs_Chat_Filters mFilters;
        private UserPrefs_Chat_Links mLinks;
        private int mPopupTimeout = 30;
        private bool mProfanityFilter = true;
        private bool mShowSpeaking = false;
        private double mSpamInterval = 0.5;
        private double mSpamInterval2 = 0.005;
        private int mSpeakingTimeout = 180;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AwayTimeoutChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatHistoryLengthChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PopupTimeoutChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ProfanityFilterChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowSpeakingChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SpamIntervalChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SpeakingTimeoutChanged;

        [Browsable(false)]
        public UserPrefs_Chat_Appearance Appearance
        {
            get
            {
                if (this.mAppearance == null)
                {
                    this.mAppearance = new UserPrefs_Chat_Appearance();
                }
                return this.mAppearance;
            }
        }

        [Description("<LOC>Number of minutes idle before triggering Away status; must be between 1 and 60."), DisplayName("<LOC>Auto-Away Timeout"), Category("<LOC>Misc")]
        public int AwayTimeout
        {
            get
            {
                return this.mAwayTimeout;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                else if (value > 60)
                {
                    value = 60;
                }
                this.mAwayTimeout = value;
                if (this.AwayTimeoutChanged != null)
                {
                    this.AwayTimeoutChanged(this, new PropertyChangedEventArgs("AwayTimeout"));
                }
            }
        }

        [Description("<LOC>The number of sent chat messages the system will remember, accessed via the up and down arrows from the chat textbox."), DisplayName("<LOC>Chat Send History"), Category("<LOC>Misc")]
        public int ChatHistoryLength
        {
            get
            {
                return this.mChatHistoryLength;
            }
            set
            {
                this.mChatHistoryLength = value;
                if (this.ChatHistoryLengthChanged != null)
                {
                    this.ChatHistoryLengthChanged(this, new PropertyChangedEventArgs("ChatHistoryLength"));
                }
            }
        }

        [Browsable(false), OptionsRoot("<LOC>Emotes")]
        public UserPrefs_Chat_Emotes Emotes
        {
            get
            {
                if (this.mEmotes == null)
                {
                    this.mEmotes = new UserPrefs_Chat_Emotes();
                }
                return this.mEmotes;
            }
        }

        [Browsable(false), OptionsRoot("<LOC>Filters")]
        public UserPrefs_Chat_Filters Filters
        {
            get
            {
                if (this.mFilters == null)
                {
                    this.mFilters = new UserPrefs_Chat_Filters();
                }
                return this.mFilters;
            }
        }

        [OptionsRoot("<LOC>Links"), Browsable(false)]
        public UserPrefs_Chat_Links Links
        {
            get
            {
                if (this.mLinks == null)
                {
                    this.mLinks = new UserPrefs_Chat_Links();
                }
                return this.mLinks;
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Chat Dialog Auto-Close"), Description("<LOC>Number of seconds to display a chat dialog or popup before it is automatically closed.")]
        public int PopupTimeout
        {
            get
            {
                return this.mPopupTimeout;
            }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                else if (value > 300)
                {
                    value = 300;
                }
                this.mPopupTimeout = value;
                if (this.PopupTimeoutChanged != null)
                {
                    this.PopupTimeoutChanged(this, new PropertyChangedEventArgs("PopupTimeout"));
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Profanity Filter On"), Description("<LOC>Filters offensive language out of chat.")]
        public bool ProfanityFilter
        {
            get
            {
                return this.mProfanityFilter;
            }
            set
            {
                this.mProfanityFilter = value;
                if (this.ProfanityFilterChanged != null)
                {
                    this.ProfanityFilterChanged(this, new PropertyChangedEventArgs("ProfanityFilter"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>Show players who are speaking in chat in a separate chat group."), DisplayName("<LOC>Show Speaking Players")]
        public bool ShowSpeaking
        {
            get
            {
                return this.mShowSpeaking;
            }
            set
            {
                this.mShowSpeaking = value;
                if (this.ShowSpeakingChanged != null)
                {
                    this.ShowSpeakingChanged(this, new PropertyChangedEventArgs("ShowSpeaking"));
                }
            }
        }

        [DisplayName("<LOC>Spam Interval"), Description("<LOC>Interval in seconds between messages to trigger spam detection."), Category("<LOC>Misc")]
        public double SpamInterval
        {
            get
            {
                if (this.mSpamInterval2 == 0.0)
                {
                    this.mSpamInterval = 0.005;
                }
                return this.mSpamInterval2;
            }
            set
            {
                this.mSpamInterval2 = value;
                if (this.SpamIntervalChanged != null)
                {
                    this.SpamIntervalChanged(this, new PropertyChangedEventArgs("SpamInterval"));
                }
            }
        }

        [Description("<LOC>When 'Show Speaking Players' is true, controls how long they are in separate group."), DisplayName("<LOC>Speaking Timeout"), Category("<LOC>Misc")]
        public int SpeakingTimeout
        {
            get
            {
                return this.mSpeakingTimeout;
            }
            set
            {
                if (value < 5)
                {
                    value = 5;
                }
                else if (value >= (this.AwayTimeout * 60))
                {
                    value = (this.AwayTimeout * 60) - 5;
                }
                this.mSpeakingTimeout = value;
                if (this.SpeakingTimeoutChanged != null)
                {
                    this.SpeakingTimeoutChanged(this, new PropertyChangedEventArgs("SpeakingTimeout"));
                }
            }
        }
    }
}

