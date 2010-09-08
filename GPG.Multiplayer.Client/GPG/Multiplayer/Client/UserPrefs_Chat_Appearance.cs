namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.Serialization;

    [Serializable]
    public class UserPrefs_Chat_Appearance
    {
        private Color mAdminColor = Color.FromArgb(0xff, 0xcc, 0x33);
        private Font mAdminFont = new Font(DEF_FONT, FontStyle.Bold);
        private int mChatLineSpacing = 0;
        private Color mChatroomColor = Color.WhiteSmoke;
        private Font mChatroomFont = new Font(DEF_FONT, FontStyle.Bold);
        private Size mChatroomIconSize = new Size(0x18, 12);
        private int mChatroomListHeight = 0x19;
        private int mChatroomListWidth = 220;
        private Color mClanColor = Color.FromArgb(0xcc, 0x66, 0x66);
        private Font mClanFont = DEF_FONT;
        private Font mClanTagFont = new Font(DEF_FONT.FontFamily, 6f, FontStyle.Bold);
        private Color mDefaultColor = Color.FromArgb(0xff, 0xff, 0xff);
        private Font mDefaultFont = DEF_FONT;
        private Color mErrorColor = Color.Red;
        private Font mErrorFont = new Font(DEF_FONT, FontStyle.Bold);
        private Color mEventColor = Color.FromArgb(0xff, 0xff, 0x99);
        private Font mEventFont = new Font(DEF_FONT, FontStyle.Bold);
        private Color mFriendsColor = Color.FromArgb(0xff, 0x99, 0x66);
        private Font mFriendsFont = DEF_FONT;
        private Color mGameColor = Color.DodgerBlue;
        private Font mGameFont = new Font(DEF_FONT, FontStyle.Bold);
        private Color mMeColor = Color.FromArgb(0xcc, 0x66, 0x66);
        private Color mModeratorColor = Color.FromArgb(0xaf, 0x48, 0x8e);
        private Font mModeratorFont = new Font(DEF_FONT, FontStyle.Bold);
        private UserPrefs_Chat_Appearance_PrivateChat mPrivateMessaging;
        private Font mSelectedGameFont = new Font("Verdana", 20f, FontStyle.Bold);
        private Color mSelfColor = Color.FromArgb(0xff, 0x66, 0);
        private Font mSelfFont = new Font(DEF_FONT, FontStyle.Bold);
        private bool mShowChatroomIcons = true;
        private Color mSystemColor = Color.FromArgb(0xff, 0xff, 0);
        private Font mSystemFont = new Font(DEF_FONT, FontStyle.Bold);
        private Color mUnavailableColor = Color.FromArgb(0x66, 0x66, 0x66);
        private Font mUnavailableFont = DEF_FONT;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AdminColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AdminFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatLineSpacingChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatroomColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatroomFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatroomIconSizeChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatroomListHeightChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ChatroomListWidthChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ClanColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ClanFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ClanTagFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ColorsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DefaultColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DefaultFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ErrorColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ErrorFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EventColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EventFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FontsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FriendsColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FriendsFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler GameColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler GameFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MeColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ModeratorColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ModeratorFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SelectedGameFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SelfColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SelfFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowChatroomIconsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SystemColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SystemFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler UnavailableColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler UnavailableFontChanged;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Color meColor = this.MeColor;
            if (this.MeColor != Color.Empty)
            {
                CallOutEffect.sColor = this.MeColor;
            }
            else
            {
                this.MeColor = Color.FromArgb(0xcc, 0x66, 0x66);
            }
        }

        [Category("<LOC>Colors"), Description("<LOC>"), DisplayName("<LOC>Admin Color")]
        public Color AdminColor
        {
            get
            {
                return this.mAdminColor;
            }
            set
            {
                this.mAdminColor = value;
                if (this.AdminColorChanged != null)
                {
                    this.AdminColorChanged(this, new PropertyChangedEventArgs("AdminColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("AdminColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>Admin Font")]
        public Font AdminFont
        {
            get
            {
                return this.mAdminFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mAdminFont = value;
                if (this.AdminFontChanged != null)
                {
                    this.AdminFontChanged(this, new PropertyChangedEventArgs("AdminFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("AdminFont"));
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Chat Line Spacing"), Description("<LOC>Number of pixels between chat lines.")]
        public int ChatLineSpacing
        {
            get
            {
                return this.mChatLineSpacing;
            }
            set
            {
                this.mChatLineSpacing = value;
                if (this.ChatLineSpacingChanged != null)
                {
                    this.ChatLineSpacingChanged(this, new PropertyChangedEventArgs("ChatLineSpacing"));
                }
            }
        }

        [Category("<LOC>Colors"), Description("<LOC>Chatroom dropdown text color."), DisplayName("<LOC>Chatroom Color")]
        public Color ChatroomColor
        {
            get
            {
                return this.mChatroomColor;
            }
            set
            {
                this.mChatroomColor = value;
                if (this.ChatroomColorChanged != null)
                {
                    this.ChatroomColorChanged(this, new PropertyChangedEventArgs("ChatroomColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("ChatroomColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), DisplayName("<LOC>Chatroom Font"), Description("<LOC>Chatroom dropdown font.")]
        public Font ChatroomFont
        {
            get
            {
                return this.mChatroomFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mChatroomFont = value;
                if (this.ChatroomFontChanged != null)
                {
                    this.ChatroomFontChanged(this, new PropertyChangedEventArgs("ChatroomFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("ChatroomFont"));
                }
            }
        }

        [Category("<LOC>Chatroom List"), DisplayName("<LOC>Icon Size"), Description("<LOC>Chatroom icon size, if Show Icons is set true.")]
        public Size ChatroomIconSize
        {
            get
            {
                return this.mChatroomIconSize;
            }
            set
            {
                this.mChatroomIconSize = value;
                if (this.ChatroomIconSizeChanged != null)
                {
                    this.ChatroomIconSizeChanged(this, new PropertyChangedEventArgs("ChatroomIconSize"));
                }
            }
        }

        [DisplayName("<LOC>Chatroom List Height"), Category("<LOC>Chatroom List"), Description("<LOC>Chatroom drop-down list height.")]
        public int ChatroomListHeight
        {
            get
            {
                return this.mChatroomListHeight;
            }
            set
            {
                this.mChatroomListHeight = value;
                if (this.ChatroomListHeightChanged != null)
                {
                    this.ChatroomListHeightChanged(this, new PropertyChangedEventArgs("ChatroomListHeight"));
                }
            }
        }

        [Category("<LOC>Chatroom List"), DisplayName("<LOC>Chatroom List Width"), Description("<LOC>Chatroom drop-down list width.")]
        public int ChatroomListWidth
        {
            get
            {
                return this.mChatroomListWidth;
            }
            set
            {
                this.mChatroomListWidth = value;
                if (this.ChatroomListWidthChanged != null)
                {
                    this.ChatroomListWidthChanged(this, new PropertyChangedEventArgs("ChatroomListWidth"));
                }
            }
        }

        [Category("<LOC>Colors"), Description("<LOC>The color of clan members in chat."), DisplayName("<LOC>Clan Color")]
        public Color ClanColor
        {
            get
            {
                return this.mClanColor;
            }
            set
            {
                this.mClanColor = value;
                if (this.ClanColorChanged != null)
                {
                    this.ClanColorChanged(this, new PropertyChangedEventArgs("ClanColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("ClanColor"));
                }
            }
        }

        [Description("<LOC>The font of clan members in chat."), Category("<LOC>Fonts"), DisplayName("<LOC>Clan Font")]
        public Font ClanFont
        {
            get
            {
                return this.mClanFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mClanFont = value;
                if (this.ClanFontChanged != null)
                {
                    this.ClanFontChanged(this, new PropertyChangedEventArgs("ClanFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("ClanFont"));
                }
            }
        }

        [Category("<LOC>Fonts"), Description("<LOC>Clan abbreviation tag font."), DisplayName("<LOC>Clan Tag Font")]
        public Font ClanTagFont
        {
            get
            {
                return this.mClanTagFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mClanTagFont = value;
                if (this.ClanTagFontChanged != null)
                {
                    this.ClanTagFontChanged(this, new PropertyChangedEventArgs("ClanTagFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("ClanTagFont"));
                }
            }
        }

        private static Font DEF_FONT
        {
            get
            {
                return UserPrefs_Appearance_Text.DEF_FONT;
            }
        }

        [Description("<LOC>The default color of text in chat."), DisplayName("<LOC>Default Color"), Category("<LOC>Colors")]
        public Color DefaultColor
        {
            get
            {
                return this.mDefaultColor;
            }
            set
            {
                this.mDefaultColor = value;
                if (this.DefaultColorChanged != null)
                {
                    this.DefaultColorChanged(this, new PropertyChangedEventArgs("DefaultColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("DefaultColor"));
                }
            }
        }

        [Description("<LOC>The default font of text in chat."), Category("<LOC>Fonts"), DisplayName("<LOC>Default Font")]
        public Font DefaultFont
        {
            get
            {
                return this.mDefaultFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mDefaultFont = value;
                if (this.DefaultFontChanged != null)
                {
                    this.DefaultFontChanged(this, new PropertyChangedEventArgs("DefaultFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("DefaultFont"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Colors"), DisplayName("<LOC>Error Color")]
        public Color ErrorColor
        {
            get
            {
                return this.mErrorColor;
            }
            set
            {
                this.mErrorColor = value;
                if (this.ErrorColorChanged != null)
                {
                    this.ErrorColorChanged(this, new PropertyChangedEventArgs("ErrorColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("ErrorColor"));
                }
            }
        }

        [DisplayName("<LOC>Error Font"), Category("<LOC>Fonts"), Description("<LOC>")]
        public Font ErrorFont
        {
            get
            {
                return this.mErrorFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mErrorFont = value;
                if (this.ErrorFontChanged != null)
                {
                    this.ErrorFontChanged(this, new PropertyChangedEventArgs("ErrorFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("ErrorFont"));
                }
            }
        }

        [Category("<LOC>Colors"), DisplayName("<LOC>system Event Color"), Description("<LOC>")]
        public Color EventColor
        {
            get
            {
                return this.mEventColor;
            }
            set
            {
                this.mEventColor = value;
                if (this.EventColorChanged != null)
                {
                    this.EventColorChanged(this, new PropertyChangedEventArgs("EventColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("EventColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>System Event Font")]
        public Font EventFont
        {
            get
            {
                return this.mEventFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mEventFont = value;
                if (this.EventFontChanged != null)
                {
                    this.EventFontChanged(this, new PropertyChangedEventArgs("EventFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("EventFont"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Colors"), DisplayName("<LOC>Friends Color")]
        public Color FriendsColor
        {
            get
            {
                return this.mFriendsColor;
            }
            set
            {
                this.mFriendsColor = value;
                if (this.FriendsColorChanged != null)
                {
                    this.FriendsColorChanged(this, new PropertyChangedEventArgs("FriendsColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("FriendsColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), Description("<LOC>"), DisplayName("<LOC>Friends Font")]
        public Font FriendsFont
        {
            get
            {
                return this.mFriendsFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mFriendsFont = value;
                if (this.FriendsFontChanged != null)
                {
                    this.FriendsFontChanged(this, new PropertyChangedEventArgs("FriendsFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("FriendsFont"));
                }
            }
        }

        [Description(""), Category("<LOC>Colors"), DisplayName("<LOC>Game Event Color")]
        public Color GameColor
        {
            get
            {
                return this.mGameColor;
            }
            set
            {
                this.mGameColor = value;
                if (this.GameColorChanged != null)
                {
                    this.GameColorChanged(this, new PropertyChangedEventArgs("GameColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), Description(""), DisplayName("<LOC>Game Event Font")]
        public Font GameFont
        {
            get
            {
                return this.mGameFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mGameFont = value;
                if (this.GameFontChanged != null)
                {
                    this.GameFontChanged(this, new PropertyChangedEventArgs("GameFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("GameFont"));
                }
            }
        }

        [Category("<LOC>Colors"), Description("<LOC>"), DisplayName("<LOC>Me Color")]
        public Color MeColor
        {
            get
            {
                return this.mMeColor;
            }
            set
            {
                this.mMeColor = value;
                if (this.MeColorChanged != null)
                {
                    this.MeColorChanged(this, new PropertyChangedEventArgs("MeColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("MeColor"));
                }
                CallOutEffect.sColor = this.MeColor;
            }
        }

        [Category("<LOC>Colors"), Description("<LOC>"), DisplayName("<LOC>Moderator Color")]
        public Color ModeratorColor
        {
            get
            {
                return this.mModeratorColor;
            }
            set
            {
                this.mModeratorColor = value;
                if (this.ModeratorColorChanged != null)
                {
                    this.ModeratorColorChanged(this, new PropertyChangedEventArgs("ModeratorColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("ModeratorColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>Moderator Font")]
        public Font ModeratorFont
        {
            get
            {
                return this.mModeratorFont;
            }
            set
            {
                this.mModeratorFont = value;
                if (this.ModeratorFontChanged != null)
                {
                    this.ModeratorFontChanged(this, new PropertyChangedEventArgs("ModeratorFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("ModeratorFont"));
                }
            }
        }

        [Browsable(false), OptionsRoot("<LOC>Private Messaging")]
        public UserPrefs_Chat_Appearance_PrivateChat PrivateMessaging
        {
            get
            {
                if (this.mPrivateMessaging == null)
                {
                    this.mPrivateMessaging = new UserPrefs_Chat_Appearance_PrivateChat();
                }
                return this.mPrivateMessaging;
            }
        }

        [Description("<LOC>The font of unavailable players in chat."), Category("<LOC>Fonts"), DisplayName("<LOC>Unavailable Font")]
        public Font SelectedGameFont
        {
            get
            {
                if (this.mSelectedGameFont == null)
                {
                    this.mSelectedGameFont = new Font("Verdana", 20f, FontStyle.Bold);
                }
                return this.mSelectedGameFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mSelectedGameFont = value;
                if (this.SelectedGameFontChanged != null)
                {
                    this.SelectedGameFontChanged(this, new PropertyChangedEventArgs("SelectedGameFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("SelectedGameFont"));
                }
            }
        }

        [DisplayName("<LOC>Self Color"), Description("<LOC>The color of your text in chat."), Category("<LOC>Colors")]
        public Color SelfColor
        {
            get
            {
                return this.mSelfColor;
            }
            set
            {
                this.mSelfColor = value;
                if (this.SelfColorChanged != null)
                {
                    this.SelfColorChanged(this, new PropertyChangedEventArgs("SelfColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("SelfColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), DisplayName("<LOC>Self Font"), Description("<LOC>The font of your text in chat.")]
        public Font SelfFont
        {
            get
            {
                return this.mSelfFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mSelfFont = value;
                if (this.SelfFontChanged != null)
                {
                    this.SelfFontChanged(this, new PropertyChangedEventArgs("SelfFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("SelfFont"));
                }
            }
        }

        [DisplayName("<LOC>Show Icons"), Category("<LOC>Chatroom List"), Description("<LOC>Toggles visibility of chatroom icons.")]
        public bool ShowChatroomIcons
        {
            get
            {
                return this.mShowChatroomIcons;
            }
            set
            {
                this.mShowChatroomIcons = value;
                if (this.ShowChatroomIconsChanged != null)
                {
                    this.ShowChatroomIconsChanged(this, new PropertyChangedEventArgs("ShowChatroomIcons"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Colors"), DisplayName("<LOC>System Color")]
        public Color SystemColor
        {
            get
            {
                return this.mSystemColor;
            }
            set
            {
                this.mSystemColor = value;
                if (this.SystemColorChanged != null)
                {
                    this.SystemColorChanged(this, new PropertyChangedEventArgs("SystemColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("SystemColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>System Font")]
        public Font SystemFont
        {
            get
            {
                return this.mSystemFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mSystemFont = value;
                if (this.SystemFontChanged != null)
                {
                    this.SystemFontChanged(this, new PropertyChangedEventArgs("SystemFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("SystemFont"));
                }
            }
        }

        [Description("<LOC>The color of unavailable players in chat."), Category("<LOC>Colors"), DisplayName("<LOC>Unavailable Color")]
        public Color UnavailableColor
        {
            get
            {
                return this.mUnavailableColor;
            }
            set
            {
                this.mUnavailableColor = value;
                if (this.UnavailableColorChanged != null)
                {
                    this.UnavailableColorChanged(this, new PropertyChangedEventArgs("UnavailableColor"));
                }
                if (this.ColorsChanged != null)
                {
                    this.ColorsChanged(this, new PropertyChangedEventArgs("UnavailableColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), Description("<LOC>The font of unavailable players in chat."), DisplayName("<LOC>Unavailable Font")]
        public Font UnavailableFont
        {
            get
            {
                return this.mUnavailableFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mUnavailableFont = value;
                if (this.UnavailableFontChanged != null)
                {
                    this.UnavailableFontChanged(this, new PropertyChangedEventArgs("UnavailableFont"));
                }
                if (this.FontsChanged != null)
                {
                    this.FontsChanged(this, new PropertyChangedEventArgs("UnavailableFont"));
                }
            }
        }
    }
}

