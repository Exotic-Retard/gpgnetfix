namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    [Serializable]
    public class UserPrefs_Chat_Filters
    {
        private bool mFilterAdmin = true;
        private bool mFilterClan = true;
        private bool mFilterFriends = true;
        private bool mFilterGameMessages = true;
        private bool mFilterOther = true;
        private bool mFilterSelf = true;
        private bool mFiltersSet = true;
        private bool mFilterSystemErrors = true;
        private bool mFilterSystemEvents = true;
        private bool mFilterSystemMessages = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterAdminChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterClanChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterFriendsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterGameMessagesChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterOtherChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterSelfChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterSystemErrorsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterSystemEventsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FilterSystemMessagesChanged;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (!this.mFiltersSet)
            {
                this.mFilterSystemMessages = true;
                this.mFilterFriends = true;
                this.mFilterClan = true;
                this.mFilterAdmin = true;
                this.mFilterSelf = true;
                this.mFilterOther = true;
                this.mFiltersSet = true;
            }
        }

        [Description("<LOC>Toggles visibility of messages from admins in chat."), Category("<LOC>Filters"), DisplayName("<LOC>Show Admin Messages")]
        public bool FilterAdmin
        {
            get
            {
                return this.mFilterAdmin;
            }
            set
            {
                this.mFilterAdmin = value;
                if (this.mFilterAdminChanged != null)
                {
                    this.mFilterAdminChanged(this, new PropertyChangedEventArgs("FilterAdmin"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterAdmin"));
                }
            }
        }

        [Description("<LOC>Toggles visibility of messages from clan members in chat."), Category("<LOC>Filters"), DisplayName("<LOC>Show Clan Messages")]
        public bool FilterClan
        {
            get
            {
                return this.mFilterClan;
            }
            set
            {
                this.mFilterClan = value;
                if (this.mFilterClanChanged != null)
                {
                    this.mFilterClanChanged(this, new PropertyChangedEventArgs("FilterClan"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterClan"));
                }
            }
        }

        [Category("<LOC>Filters"), Description("<LOC>Toggles visibility of messages from Friends in chat."), DisplayName("<LOC>Show Friends Messages")]
        public bool FilterFriends
        {
            get
            {
                return this.mFilterFriends;
            }
            set
            {
                this.mFilterFriends = value;
                if (this.mFilterFriendsChanged != null)
                {
                    this.mFilterFriendsChanged(this, new PropertyChangedEventArgs("FilterFriends"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterFriends"));
                }
            }
        }

        [Description("<LOC>Toggles visibility of game-related messages in chat."), DisplayName("<LOC>Show Game Messages"), Category("<LOC>Filters")]
        public bool FilterGameMessages
        {
            get
            {
                return this.mFilterGameMessages;
            }
            set
            {
                this.mFilterGameMessages = value;
                if (this.mFilterGameMessagesChanged != null)
                {
                    this.mFilterGameMessagesChanged(this, new PropertyChangedEventArgs("FilterGameMessages"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterGameMessages"));
                }
            }
        }

        [Category("<LOC>Filters"), Description("<LOC>Toggles visibility of messages from all other non-filtered users in chat."), DisplayName("<LOC>Show Other Messages")]
        public bool FilterOther
        {
            get
            {
                return this.mFilterOther;
            }
            set
            {
                this.mFilterOther = value;
                if (this.mFilterOtherChanged != null)
                {
                    this.mFilterOtherChanged(this, new PropertyChangedEventArgs("FilterOther"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterOther"));
                }
            }
        }

        [Category("<LOC>Filters"), DisplayName("<LOC>Show Self Messages"), Description("<LOC>Toggles visibility of messages from yourself in chat.")]
        public bool FilterSelf
        {
            get
            {
                return this.mFilterSelf;
            }
            set
            {
                this.mFilterSelf = value;
                if (this.mFilterSelfChanged != null)
                {
                    this.mFilterSelfChanged(this, new PropertyChangedEventArgs("FilterSelf"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterSelf"));
                }
            }
        }

        [Description("<LOC>Toggles visibility of system error messages in chat."), Category("<LOC>Filters"), DisplayName("<LOC>Show System Errors")]
        public bool FilterSystemErrors
        {
            get
            {
                return this.mFilterSystemErrors;
            }
            set
            {
                this.mFilterSystemErrors = value;
                if (this.mFilterSystemErrorsChanged != null)
                {
                    this.mFilterSystemErrorsChanged(this, new PropertyChangedEventArgs("FilterSystemErrors"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterSystemErrors"));
                }
            }
        }

        [Category("<LOC>Filters"), Description("<LOC>Toggles visibility of system-triggered event messages in chat."), DisplayName("<LOC>Show System Events")]
        public bool FilterSystemEvents
        {
            get
            {
                return this.mFilterSystemEvents;
            }
            set
            {
                this.mFilterSystemEvents = value;
                if (this.mFilterSystemEventsChanged != null)
                {
                    this.mFilterSystemEventsChanged(this, new PropertyChangedEventArgs("FilterSystemEvents"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterSystemEvents"));
                }
            }
        }

        [DisplayName("<LOC>Show System Messages"), Category("<LOC>Filters"), Description("<LOC>Toggles visibility of system messages in chat.")]
        public bool FilterSystemMessages
        {
            get
            {
                return this.mFilterSystemMessages;
            }
            set
            {
                this.mFilterSystemMessages = value;
                if (this.mFilterSystemMessagesChanged != null)
                {
                    this.mFilterSystemMessagesChanged(this, new PropertyChangedEventArgs("FilterSystemMessages"));
                }
                if (this.mFilterChanged != null)
                {
                    this.mFilterChanged(this, new PropertyChangedEventArgs("FilterSystemMessages"));
                }
            }
        }
    }
}

