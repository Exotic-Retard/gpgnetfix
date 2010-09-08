namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class UserPrefs_Chat_Links
    {
        private Color mChatColor = Color.Yellow;
        private Color mClanColor = Color.Green;
        private Color mContentColor = Color.DodgerBlue;
        private Color mEmailColor = Color.Silver;
        private Color mEmoteColor = Color.Orange;
        private Color mGameColor = Color.Brown;
        private Color mHelpColor = Color.Green;
        private Font mLinkFont = new Font(UserPrefs_Appearance_Text.DEF_FONT, FontStyle.Underline);
        private Color mPlayerColor = Color.Green;
        private Color mReplayColor = Color.AliceBlue;
        private bool mShowChatLinks = true;
        private Color mSolutionColor = Color.Green;
        private Color mWebColor = Color.FromArgb(0x99, 0x99, 0xff);

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ClanColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ContentColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EmailColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EmoteColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler GameColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler HelpColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LinkColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LinkFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PlayerColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ReplayColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowChatLinksChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SolutionColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler WebColorChanged;

        [DisplayName("<LOC>Chat Link Color"), Description("<LOC>The color of a link to a chat channel. (chat:name)"), Category("<LOC>Colors")]
        public Color ChatColor
        {
            get
            {
                return this.mChatColor;
            }
            set
            {
                this.mChatColor = value;
                if (this.mChatColorChanged != null)
                {
                    this.mChatColorChanged(this, new PropertyChangedEventArgs("ChatColor"));
                }
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("ChatColor"));
                }
            }
        }

        [Description("<LOC>The color of a link to a clan's profile. (clan:name)"), Category("<LOC>Colors"), DisplayName("<LOC>Clan Link Color")]
        public Color ClanColor
        {
            get
            {
                return this.mClanColor;
            }
            set
            {
                this.mClanColor = value;
                if (this.mClanColorChanged != null)
                {
                    this.mClanColorChanged(this, new PropertyChangedEventArgs("ClanColor"));
                }
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("ClanColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Colors"), DisplayName("<LOC>ContentColor")]
        public Color ContentColor
        {
            get
            {
                return this.mContentColor;
            }
            set
            {
                this.mContentColor = value;
                if (this.mContentColorChanged != null)
                {
                    this.mContentColorChanged(this, new PropertyChangedEventArgs("ContentColor"));
                }
            }
        }

        [Description("<LOC>The color of a link to an email address. (name@domain.suffix)"), Category("<LOC>Colors"), DisplayName("<LOC>Email Link Color")]
        public Color EmailColor
        {
            get
            {
                return this.mEmailColor;
            }
            set
            {
                this.mEmailColor = value;
                if (this.mEmailColorChanged != null)
                {
                    this.mEmailColorChanged(this, new PropertyChangedEventArgs("EmailColor"));
                }
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("EmailColor"));
                }
            }
        }

        [DisplayName("<LOC>Emote Link Color"), Description("<LOC>The color of a link to a custom emote."), Category("<LOC>Colors")]
        public Color EmoteColor
        {
            get
            {
                return this.mEmoteColor;
            }
            set
            {
                this.mEmoteColor = value;
                if (this.mEmoteColorChanged != null)
                {
                    this.mEmoteColorChanged(this, new PropertyChangedEventArgs("EmoteColor"));
                }
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("EmoteColor"));
                }
            }
        }

        [Description("<LOC>The color of a link to a game. (game:name)"), Category("<LOC>Colors"), DisplayName("<LOC>Game Link Color")]
        public Color GameColor
        {
            get
            {
                return this.mGameColor;
            }
            set
            {
                this.mGameColor = value;
                if (this.mGameColorChanged != null)
                {
                    this.mGameColorChanged(this, new PropertyChangedEventArgs("GameColor"));
                }
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("GameColor"));
                }
            }
        }

        [DisplayName("<LOC>Help Link Color"), Category("<LOC>Colors"), Description("")]
        public Color HelpColor
        {
            get
            {
                return this.mHelpColor;
            }
            set
            {
                this.mHelpColor = value;
                if (this.mHelpColorChanged != null)
                {
                    this.mHelpColorChanged(this, new PropertyChangedEventArgs("HelpColor"));
                }
            }
        }

        [Description("<LOC>The font of a link in chat."), Category("<LOC>Fonts"), DisplayName("<LOC>Link Font")]
        public Font LinkFont
        {
            get
            {
                return this.mLinkFont;
            }
            set
            {
                this.mLinkFont = value;
                if (this.mLinkFontChanged != null)
                {
                    this.mLinkFontChanged(this, new PropertyChangedEventArgs("LinkFont"));
                }
            }
        }

        [Category("<LOC>Colors"), Description("<LOC>The color of a link to a player's profile. (player:name)"), DisplayName("<LOC>Player Link Color")]
        public Color PlayerColor
        {
            get
            {
                return this.mPlayerColor;
            }
            set
            {
                this.mPlayerColor = value;
                if (this.mPlayerColorChanged != null)
                {
                    this.mPlayerColorChanged(this, new PropertyChangedEventArgs("PlayerColor"));
                }
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("PlayerColor"));
                }
            }
        }

        [Category("<LOC>Colors"), DisplayName("<LOC>Replay Link Color"), Description("")]
        public Color ReplayColor
        {
            get
            {
                return this.mReplayColor;
            }
            set
            {
                this.mReplayColor = value;
                if (this.mReplayColorChanged != null)
                {
                    this.mReplayColorChanged(this, new PropertyChangedEventArgs("ReplayColor"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>Toggles visibility of hyperlinks in chat."), DisplayName("<LOC>Show Chat Links")]
        public bool ShowChatLinks
        {
            get
            {
                return this.mShowChatLinks;
            }
            set
            {
                this.mShowChatLinks = value;
                if (this.mShowChatLinksChanged != null)
                {
                    this.mShowChatLinksChanged(this, new PropertyChangedEventArgs("ShowChatLinks"));
                }
            }
        }

        [Category("<LOC>Colors"), Description(""), DisplayName("<LOC>Solution Link Color")]
        public Color SolutionColor
        {
            get
            {
                return this.mSolutionColor;
            }
            set
            {
                this.mSolutionColor = value;
                if (this.mSolutionColorChanged != null)
                {
                    this.mSolutionColorChanged(this, new PropertyChangedEventArgs("SolutionColor"));
                }
            }
        }

        [DisplayName("<LOC>Web Link Color"), Description("<LOC>The color of a web url link. (www.website.com)"), Category("<LOC>Colors")]
        public Color WebColor
        {
            get
            {
                return this.mWebColor;
            }
            set
            {
                this.mWebColor = value;
                if (this.mWebColorChanged != null)
                {
                    this.mWebColorChanged(this, new PropertyChangedEventArgs("WebColor"));
                }
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("WebColor"));
                }
            }
        }
    }
}

