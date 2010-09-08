namespace GPG.Multiplayer.Client
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraBars.Docking;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Mask;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Admin;
    using GPG.Multiplayer.Client.ChatEffect;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Controls.Awards;
    using GPG.Multiplayer.Client.Controls.UserList;
    using GPG.Multiplayer.Client.Friends;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Games.SpaceSiege;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.Client.Games.SupremeCommander.tournaments;
    using GPG.Multiplayer.Client.Ladders;
    using GPG.Multiplayer.Client.map;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Client.Security;
    using GPG.Multiplayer.Client.SolutionsLib;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.Client.Vaulting.Applications;
    using GPG.Multiplayer.Client.Volunteering;
    using GPG.Multiplayer.Game;
    using GPG.Multiplayer.Game.SupremeCommander;
    using GPG.Multiplayer.LadderService;
    using GPG.Multiplayer.Plugin;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.Multiplayer.Statistics;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Network;
    using GPG.Security;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Deployment.Application;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;

    public class FrmMain : FrmBase, IStatusProvider
    {
        private VGen _AddUser;
        private VGen _RemoveUser;
        private VGen _UpdateUser;
        private List<string> ActiveDialogRequests;
        private ToolStripMenuItem adhocChartsToolStripMenuItem;
        public bool AllowDirectChallenge;
        public static bool AllowFriendsToBypassDND = false;
        private bool AwaitingLogin;
        private StateTimer AwayTimer;
        internal string BeforeTeamGameChatroom;
        private bool BetweenChannels;
        public ToolStripMenuItem btnArrangedTeam;
        public ToolStripMenuItem btnChat;
        private SkinButton btnChatTab;
        private SkinButton btnClanTab;
        public ToolStripMenuItem btnFeedback;
        private SkinButton btnFriendsTab;
        public ToolStripMenuItem btnHome;
        public ToolStripMenuItem btnHostGame;
        public ToolStripMenuItem btnJoinGame;
        public ToolStripMenuItem btnMore;
        public ToolStripMenuItem btnOptions;
        public ToolStripMenuItem btnPlayNow;
        public ToolStripMenuItem btnRankedGame;
        public ToolStripMenuItem btnReplayVault;
        private Button btnSend;
        public ToolStripMenuItem btnVault;
        public ToolStripMenuItem btnViewRankings;
        public ToolStripMenuItem btnWorldMap;
        private bool CancelFade;
        private Dictionary<string, TextContainer<StatusTextLine>> ChatContainerLookup;
        private BoundContainerList<StatusTextLine> ChatContainers;
        private ThreadQueue ChatroomQueue;
        private Dictionary<int, int> ChatRowHeights;
        private Dictionary<int, Rectangle> ChatRowPoints;
        private System.Timers.Timer ChatSyncTimer;
        private MenuItem ciChat_Ban;
        private MenuItem ciChat_DemoteClan;
        private MenuItem ciChat_IgnorePlayer;
        private MenuItem ciChat_InviteFriend;
        private MenuItem ciChat_InviteToClan;
        private MenuItem ciChat_Kick;
        private MenuItem ciChat_LeaveClan;
        private MenuItem ciChat_PromoteClan;
        private MenuItem ciChat_RemoveClan;
        private MenuItem ciChat_RemoveFriend;
        private MenuItem ciChat_RequestClanInvite;
        private MenuItem ciChat_TeamInvite;
        private MenuItem ciChat_UnignorePlayer;
        private MenuItem ciChat_ViewClan;
        private MenuItem ciChat_ViewPlayer;
        private MenuItem ciChat_ViewRank;
        private MenuItem ciChat_WebStats;
        private MenuItem ciChat_WhisperPlayer;
        private MenuItem ciChatText_Ban;
        private MenuItem ciChatText_ClanInvite;
        private MenuItem ciChatText_ClanRemove;
        private MenuItem ciChatText_ClanRequest;
        private MenuItem ciChatText_Clear;
        private MenuItem ciChatText_Copy;
        private MenuItem ciChatText_Demote;
        private MenuItem ciChatText_Filter;
        private MenuItem ciChatText_Filter_Admin;
        private MenuItem ciChatText_Filter_Clan;
        private MenuItem ciChatText_Filter_Errors;
        private MenuItem ciChatText_Filter_Events;
        private MenuItem ciChatText_Filter_Friends;
        private MenuItem ciChatText_Filter_Game;
        private MenuItem ciChatText_Filter_Other;
        private MenuItem ciChatText_Filter_Self;
        private MenuItem ciChatText_Filter_System;
        private MenuItem ciChatText_Filters_Reset;
        private MenuItem ciChatText_FriendInvite;
        private MenuItem ciChatText_FriendRemove;
        private MenuItem ciChatText_Ignore;
        private MenuItem ciChatText_Kick;
        private MenuItem ciChatText_LeaveClan;
        private MenuItem ciChatText_PrivateMsg;
        private MenuItem ciChatText_Promote;
        private MenuItem ciChatText_Replays;
        private MenuItem ciChatText_ShowEmotes;
        private MenuItem ciChatText_Solution;
        private MenuItem ciChatText_Unignore;
        private MenuItem ciChatText_ViewClan;
        private MenuItem ciChatText_ViewPlayer;
        private MenuItem ciChatText_ViewRank;
        private MenuItem ciChatText_WebStats;
        private MenuItem ciEmote_Animate;
        private MenuItem ciEmote_Delete;
        private MenuItem ciEmote_Hide;
        private MenuItem ciEmote_Manager;
        private MenuItem ciEmote_Share;
        private Dictionary<string, TextContainer> ClanContainerLookup;
        private BoundContainerList ClanContainers;
        private MappedObjectList<ClanInvite> ClanInvites;
        private bool ClanLoaded;
        private DlgClanProfileEx ClanProfileForm;
        private MappedObjectList<ClanRequest> ClanRequests;
        private GridColumn colIcon;
        private GridColumn colPlayer;
        private GridColumn colText;
        private GridColumn colTimeStamp;
        private IContainer components;
        private bool? ContinueLoading;
        private bool CountEmotes;
        private uint CurrentFrame;
        private int Divider;
        private GPG.Multiplayer.Client.DlgAbout DlgAbout;
        internal GPG.Multiplayer.Client.Volunteering.DlgActiveEfforts DlgActiveEfforts;
        private GPG.Multiplayer.Client.Vaulting.DlgContentManager DlgContentManager;
        private GPG.Multiplayer.Client.DlgEmotes DlgEmotes;
        private GPG.Multiplayer.Client.DlgGameKeys DlgGameKeys;
        private GPG.Multiplayer.Client.SolutionsLib.DlgKeywordSearch DlgKeywordSearch;
        private GPG.Multiplayer.Client.DlgOptions DlgOptions;
        private GPG.Multiplayer.Client.Games.SupremeCommander.DlgSearchReplays DlgSearchReplays;
        private GPG.Multiplayer.Client.SolutionsLib.DlgSearchResults DlgSearchResults;
        private GPG.Multiplayer.Client.DlgSelectChannel DlgSelectChannel;
        internal GPG.Multiplayer.Client.DlgSelectGame DlgSelectGame;
        private GPG.Multiplayer.Client.SolutionsLib.DlgSolution DlgSolution;
        internal GPG.Multiplayer.Client.Games.SupremeCommander.DlgTeamGame DlgTeamGame;
        private DlgGameUpdate DlgUpdate;
        private GPG.Multiplayer.Client.DlgUserFeedback DlgUserFeedback;
        private DockManager dockManager;
        private int EmoteCount;
        private double FadeIncrement;
        private bool FirstPaint;
        private ToolStripMenuItem forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem;
        private Dictionary<string, TextContainer> FriendContainerLookup;
        private BoundContainerList FriendContainers;
        private MappedObjectList<FriendRequest> FriendRequests;
        private bool FriendsLoaded;
        internal List<FrmStatsLadder> FrmStatsLadders;
        public const string GAME_SUPCOM = "SupremeCommander";
        private GridColumn gcVisible;
        public GPGChatGrid gpgChatGrid;
        private GPGContextMenu gpgContextMenuChat;
        private GPGContextMenu gpgContextMenuChatText;
        private GPGContextMenu gpgContextMenuEmote;
        private GPGLabel gpgLabelClanInviteCount;
        private GPGLabel gpgLabelClanInvites;
        private GPGLabel gpgLabelClanRequestCount;
        private GPGLabel gpgLabelClanRequests;
        private GPGLabel gpgLabelCreateClan;
        private GPGLabel gpgLabelFriendInvites2;
        private GPGLabel gpgLabelFriendInvitesCount2;
        private GPGLabel gpgLabelNoClan;
        private GPGLabel gpgLabelNoFriends;
        private GPGPanel gpgPanel2;
        private GPGPanel gpgPanelChatAndInput;
        private GPGPanel gpgPanelClan;
        private GPGPanel gpgPanelFriends;
        private GPGPanel gpgPanelGathering;
        private GPGPanel gpgPanelGatheringDropDown;
        private GPGScrollPanel gpgScrollPanelClan;
        private GPGScrollPanel gpgScrollPanelFriends;
        private GPGScrollPanel gpgScrollPanelNoClan;
        public GPGTextList gpgTextListCommands;
        private GridColumn gridColumn1;
        private GridColumn gridColumn2;
        private GridColumn gridColumn3;
        private GridColumn gridColumnMembers;
        private GridColumn gridColumnTitle;
        private GridView gvChat;
        private bool HasFadeStarted;
        private int HistoryIndex;
        private ImageList ilIcons;
        private ImageList ilMenuItems;
        private bool InviteAccepted;
        private bool InvitePending;
        private bool IsFading;
        private bool IsGPGNetPatching;
        public bool IsLatestVer;
        private bool IsSortingChatroom;
        private bool IsSynchronizingChatroom;
        private bool LadderInvitePending;
        internal Dictionary<int, FrmLadderView> LadderViews;
        private DateTime LastAction;
        private Dictionary<int, bool> LastGameChallenges;
        private int LastTick;
        private ToolStripMenuItem lOC2v2RankingsToolStripMenuItem;
        private ToolStripMenuItem lOC3v3RankingsToolStripMenuItem;
        private ToolStripMenuItem lOC4v4RankingsToolStripMenuItem;
        private ToolStripMenuItem lOCArrangedTeamGameToolStripMenuItem;
        private ToolStripMenuItem lOCClanRankingsToolStripMenuItem;
        private List<DlgBase> mActiveDialogs;
        private DlgBase mActiveWindow;
        private ToolStripMenuItem manageServerGamesToolStripMenuItem;
        private string mAutoStatus;
        private const double MaxFade = 1.0;
        private string mBeforeGameChatroom;
        private bool mCalcGridHeight;
        private bool mCanDirectChallenge;
        private LinkedList<string> mChatHistory;
        private BindingList<ChatLine> mChatLines;
        private int mConfigSleepRate;
        private Thread mConfigThread;
        private bool mConnected;
        private IGameLobby mCurrentGameLobby;
        private List<ToolStripItem> mCustomPaint;
        private string mDirectChallengePlayer;
        private DlgWorldMap mDlgWorldMap;
        private MenuItem menuItem1;
        private MenuItem menuItem10;
        private MenuItem menuItem11;
        private MenuItem menuItem12;
        private MenuItem menuItem13;
        private MenuItem menuItem18;
        private MenuItem menuItem2;
        private MenuItem menuItem23;
        private MenuItem menuItem24;
        private MenuItem menuItem25;
        private MenuItem menuItem26;
        private MenuItem menuItem27;
        private MenuItem menuItem28;
        private MenuItem menuItem29;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem menuItem8;
        private MenuItem menuItem9;
        private MenuItem menuItm15;
        private bool mFirstChatDraw;
        private bool mFirstConfig;
        private bool mFirstRankedSupcomGame;
        private static bool mFirstRankedSupcomTeamGame = true;
        private DateTime mGameLinkClickTime;
        private List<ToolStripMenuItem> mGameMenuItems;
        public string mGameName;
        public string mGameURL;
        private int mHeightDiff;
        public string mHostName;
        private ToolStripMenuItem miAdhocSQL;
        private ToolStripMenuItem miAdmin;
        private ToolStripMenuItem miAdmin_Avatars;
        private ToolStripMenuItem miAdmin_CreateVolunteerEffort;
        private ToolStripMenuItem miAdmin_Security;
        private ToolStripMenuItem miAdmin_ViewVolunteers;
        private ToolStripMenuItem miAutomatch;
        private ToolStripMenuItem miChangeEmail;
        private ToolStripMenuItem miConsolidateAccounts;
        private ToolStripMenuItem miCreateTournament;
        private ToolStripMenuItem miCustomAdmin;
        private ToolStripMenuItem miCustomGame;
        private ToolStripMenuItem miForums;
        private ToolStripMenuItem miGame_RedeemPrize;
        private ToolStripMenuItem miGame_Vault;
        private ToolStripMenuItem miGameGroup;
        private ToolStripMenuItem miHelp;
        private ToolStripMenuItem miHelp_About;
        private ToolStripMenuItem miHelp_GPGHome;
        private ToolStripMenuItem miHelp_ReportIssue;
        private ToolStripMenuItem miHelp_Solutions;
        private ToolStripMenuItem miHelp_SupComHome;
        private ToolStripMenuItem miHelp_Volunteer;
        private ToolStripMenuItem miJoinGame;
        private ToolStripMenuItem miLadders;
        private ToolStripMenuItem miLadders_AcceptAll;
        private ToolStripMenuItem miLadders_DeclineAll;
        private ToolStripMenuItem miMain_Exit;
        private ToolStripMenuItem miMain_Logout;
        private ToolStripMenuItem miMainGroup;
        private ToolStripMenuItem miManageGames;
        private const double MinFade = 0.0;
        private Point MinimizeLoc;
        private Size MinimizeSize;
        private ToolStripMenuItem miPacketSniffer;
        private ToolStripMenuItem miRankings;
        private ToolStripMenuItem miRankings_1v1;
        private bool mIsActiveWindow;
        private MenuItem miShowColumns;
        private ToolStripMenuItem miShowEventLog;
        private bool mIsInGame;
        private bool mIsMaximized;
        private bool mIsMoving;
        private ToolStripMenuItem miSpaceSiegeWeb;
        private bool mIsResizing;
        private bool mIsSupComPatching;
        private bool? mIsSupComVersionEnforced;
        private MenuItem miStatus_Away;
        private MenuItem miStatus_DND;
        private MenuItem miStatus_Online;
        private ToolStripMenuItem miTools;
        private ToolStripMenuItem miTools_Chat;
        private ToolStripMenuItem miTools_Chat_Emotes;
        private ToolStripMenuItem miTools_ContentManager;
        private ToolStripMenuItem miTools_Feedback;
        private ToolStripMenuItem miTools_GameKeys;
        private ToolStripMenuItem miTools_LocPatches;
        private ToolStripMenuItem miTools_Options;
        private ToolStripMenuItem miTournamentSchedule;
        private MenuItem miTranslate;
        private MenuItem miViewReplays;
        private Hashtable mLastChatContent;
        private Hashtable mLastChatTimes;
        private DateTime mLastCheckTime;
        private int mLastHeight;
        private string mLastMOTD;
        private int mLastMouseX;
        private int mLastMouseY;
        private ToolStripItem mLastMoveItem;
        private string mLastProcessList;
        private GameInformation mLastSelectedGame;
        private string mLastStatus;
        private int mLastWidth;
        private int mLastX;
        private int mLastY;
        private string mMainChatrom;
        private string mNotFound;
        private string mPassword;
        private string mPatchFileName;
        private Dictionary<string, FrmPrivateChat> mPrivateChats;
        private bool mRanInitializeChat;
        private Rectangle mResizeRect;
        private Rectangle mRestore;
        private bool mSearchingForAutomatch;
        private IUser mSelectedChatParticipant;
        private GridView mSelectedParticipantView;
        private SkinButton mSelectedTab;
        private string mServerGameVersion;
        private bool mShuttingDown;
        public GPGMenuStrip msMainMenu;
        private Dictionary<string, StateTimer> mSpeakingTimers;
        private GPGMenuStrip msQuickButtons;
        private bool mStartResizing;
        private SupComGameManager mSupcomGameManager;
        private bool mTeamAutomatchExit;
        private WelcomePage mWelcomePage;
        private int mWidthDiff;
        private bool PaintPending;
        private PictureBox pbBottom;
        private PictureBox pbBottomLeft;
        private PictureBox pbBottomRight;
        private PictureBox pbClose;
        private PictureBox pbLeftBorder;
        private SkinStatusStrip pbMiddle;
        private PictureBox pbMinimize;
        private Panel pBottom;
        private PictureBox pbRestore;
        private PictureBox pbRightBorder;
        private PictureBox pbTop;
        private PictureBox pbTopLeft;
        private PictureBox pbTopRight;
        private PanelControl pcTextEntry;
        private string PendingJoinFrom;
        private Dictionary<string, ChatEffectBase> PlayerChatEffects;
        private DlgPlayerProfileEx PlayerProfileForm;
        private bool PlayNowMatch;
        private GPGPanel pManualTabs;
        private List<string> PMTargets;
        private PnlUserList pnlUserListChat;
        private PnlUserList pnlUserListClan;
        private PnlUserList pnlUserListFriends;
        private Panel pTop;
        private bool RefreshData;
        private RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private int RESIZE_RECT;
        private RepositoryItemMemoEdit rimMemoEdit;
        private RepositoryItemMemoEdit rimMemoEdit2;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit;
        private RepositoryItemPictureEdit rimPictureEdit2;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        public const int ScrollWidth = 10;
        private Emote SelectedEmote;
        private bool SelectingTextList;
        private StateTimer SelfLeaveTimer;
        public SkinDropDown skinDropDownStatus;
        private SkinGatheringDisplay skinGatheringDisplayChat;
        private SkinLabel skinLabelClanName;
        private ToolStripMenuItem spaceSiegeLobbyToolStripMenuItem;
        private SplitContainer splitContainerBody;
        private SplitContainer splitContainerChatAndInput;
        private SplitContainer splitContainerClan;
        private SplitContainer splitContainerFriends;
        public SkinStatusButton StatusButtonRankedGameCancel;
        private StateTimer StatusTimer;
        internal bool StayActive;
        private Thread SupComPatchThread;
        private SkinButton tabChatroom;
        private double TargetFade;
        internal GPGTextArea textBoxMsg;
        public static int ThreadID = 0;
        private bool ToolstripSizeChanged;
        private ThreadQueue UIQueue;
        private WebBrowser wbMain;
        public MappedObjectList<URLInfo> WebURL;

        public event EventHandler GameHosted;

        internal event EventHandler LadderGamePlayed;

        public static  event EventHandler OnCancelLoad;

        internal event StringEventHandler PingResponseReceived;

        public event StatusProviderEventHandler StatusChanged;

        public FrmMain()
        {
            WaitCallback callBack = null;
            MessageEventHandler handler = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            this.components = null;
            this.WebURL = null;
            this.IsGPGNetPatching = true;
            this.mNotFound = "";
            this.IsLatestVer = false;
            this.mPatchFileName = "";
            this.mServerGameVersion = null;
            this.mLastSelectedGame = null;
            this.mIsSupComVersionEnforced = null;
            this.mIsSupComPatching = false;
            this.DlgUpdate = null;
            this.SupComPatchThread = null;
            this.FriendsLoaded = false;
            this.FriendContainers = new BoundContainerList();
            this.FriendContainerLookup = new Dictionary<string, TextContainer>();
            this.ActiveDialogRequests = new List<string>();
            this.FriendRequests = new MappedObjectList<FriendRequest>();
            this.AwaitingLogin = true;
            this.ContinueLoading = null;
            this.LadderViews = new Dictionary<int, FrmLadderView>();
            this.mGameMenuItems = new List<ToolStripMenuItem>();
            this.mLastMoveItem = null;
            this.mConfigSleepRate = 0x493e0;
            this.mFirstConfig = true;
            this.mLastMOTD = "";
            this.mLastProcessList = "";
            this.mConfigThread = null;
            this.mConnected = true;
            this.mShuttingDown = false;
            this.CancelFade = false;
            this.HasFadeStarted = false;
            this.IsFading = false;
            this.TargetFade = 0.0;
            this.FadeIncrement = 0.0;
            this.StayActive = false;
            this.mIsActiveWindow = false;
            this.mWelcomePage = null;
            this.mRanInitializeChat = false;
            this.mHeightDiff = 0;
            this.mWidthDiff = 0;
            this.mLastX = 0;
            this.mLastY = 0;
            this.mLastMouseX = 0;
            this.mLastMouseY = 0;
            this.mLastWidth = 0;
            this.mLastHeight = 0;
            this.mIsMoving = false;
            this.mIsResizing = false;
            this.mStartResizing = false;
            this.RESIZE_RECT = 40;
            this.mResizeRect = new Rectangle();
            this.FirstPaint = true;
            this.mIsMaximized = false;
            this.mRestore = new Rectangle(0, 0, 100, 100);
            this.MinimizeLoc = new Point(-32000, -32000);
            this.MinimizeSize = new Size(640, 500);
            this.StatusTimer = null;
            this.mActiveDialogs = new List<DlgBase>();
            this.mActiveWindow = null;
            this.DlgAbout = null;
            this.DlgOptions = null;
            this.DlgUserFeedback = null;
            this.FrmStatsLadders = new List<FrmStatsLadder>();
            this.DlgGameKeys = null;
            this.DlgEmotes = null;
            this.DlgSelectChannel = null;
            this.DlgKeywordSearch = null;
            this.DlgSearchResults = null;
            this.DlgSolution = null;
            this.DlgSearchReplays = null;
            this.DlgTeamGame = null;
            this.DlgSelectGame = null;
            this.DlgActiveEfforts = null;
            this.DlgContentManager = null;
            this.mSearchingForAutomatch = false;
            this.mLastStatus = "";
            this.mCalcGridHeight = false;
            this.ToolstripSizeChanged = false;
            this.mCustomPaint = new List<ToolStripItem>();
            this.mLastCheckTime = DateTime.Now;
            this.mDlgWorldMap = null;
            this.mSelectedTab = null;
            this.mGameLinkClickTime = DateTime.Now;
            this.ChatRowPoints = new Dictionary<int, Rectangle>();
            this.ChatRowHeights = new Dictionary<int, int>();
            this.mFirstChatDraw = true;
            this.SelectedEmote = null;
            this.LastTick = 0;
            this.Divider = 1;
            this.CurrentFrame = 1;
            this.EmoteCount = 0;
            this.CountEmotes = true;
            this.PaintPending = false;
            this.mSelectedParticipantView = null;
            this.UIQueue = new ThreadQueue(true);
            this.IsSortingChatroom = false;
            this.IsSynchronizingChatroom = false;
            this.ChatSyncTimer = null;
            this._AddUser = null;
            this._UpdateUser = null;
            this._RemoveUser = null;
            this.mSelectedChatParticipant = null;
            this.ChatContainerLookup = new Dictionary<string, TextContainer<StatusTextLine>>();
            this.ChatContainers = new BoundContainerList<StatusTextLine>();
            this.RefreshData = false;
            this.BetweenChannels = false;
            this.mMainChatrom = "";
            this.ChatroomQueue = new ThreadQueue(true);
            this.mLastChatTimes = Hashtable.Synchronized(new Hashtable());
            this.mLastChatContent = Hashtable.Synchronized(new Hashtable());
            this.mChatLines = new BindingList<ChatLine>();
            this.PlayerChatEffects = new Dictionary<string, ChatEffectBase>();
            this.mSpeakingTimers = new Dictionary<string, StateTimer>();
            this.AwayTimer = new StateTimer();
            this.SelfLeaveTimer = new StateTimer();
            this.LastAction = DateTime.Now;
            this.mChatHistory = new LinkedList<string>();
            this.HistoryIndex = -1;
            this.SelectingTextList = false;
            this.PMTargets = new List<string>();
            this.mPrivateChats = new Dictionary<string, FrmPrivateChat>();
            this.PlayerProfileForm = null;
            this.ClanLoaded = false;
            this.ClanContainers = new BoundContainerList();
            this.ClanContainerLookup = new Dictionary<string, TextContainer>();
            this.ClanRequests = null;
            this.ClanInvites = null;
            this.ClanProfileForm = null;
            this.mBeforeGameChatroom = "";
            this.mIsInGame = false;
            this.StatusButtonRankedGameCancel = new SkinStatusButton();
            this.mGameName = "";
            this.mGameURL = "";
            this.mHostName = "";
            this.mSupcomGameManager = null;
            this.mPassword = "";
            this.mFirstRankedSupcomGame = true;
            this.PlayNowMatch = false;
            this.mAutoStatus = Loc.Get("<LOC>Searching for game ");
            this.LastGameChallenges = new Dictionary<int, bool>();
            this.LadderInvitePending = false;
            this.mCanDirectChallenge = true;
            this.mDirectChallengePlayer = "";
            this.AllowDirectChallenge = true;
            this.BeforeTeamGameChatroom = null;
            this.mTeamAutomatchExit = false;
            this.InviteAccepted = false;
            this.InvitePending = false;
            this.PendingJoinFrom = null;
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            if (new DlgLogin().ShowDialog() == DialogResult.OK)
            {
                if (callBack == null)
                {
                    callBack = delegate (object p) {
                        GameInformation.LoadGamesFromDB();
                        new QuazalQuery("RemoveSpaceSiegeGame", new object[0]).ExecuteNonQuery();
                        ConfigSettings.LoadSettings(DataAccess.GetQueryData("GetAllConfigs", new object[0]));
                        try
                        {
                            DataList queryData = null;
                            queryData = DataAccess.GetQueryData("GetTOSPrompt", new object[0]);
                            if ((queryData != null) && (queryData.Count < 1))
                            {
                                DataAccess.ExecuteQuery("CreatePlayerInfo", new object[0]);
                                queryData = DataAccess.GetQueryData("GetTOSPrompt", new object[0]);
                            }
                            if ((queryData != null) && (queryData.Count > 0))
                            {
                                if (Convert.ToInt32(queryData[0]["term_of_service_prompt"]) > 0)
                                {
                                    if (new DlgTermsOfService(this, queryData[0]["url"]).ShowDialog() == DialogResult.OK)
                                    {
                                        ThreadQueue.Quazal.Enqueue((VGen0)delegate {
                                            DataAccess.ExecuteQuery("SetTOSPrompt", new object[] { 0 });
                                        }, new object[0]);
                                        this.ContinueLoading = true;
                                    }
                                    else
                                    {
                                        this.ContinueLoading = false;
                                    }
                                }
                                else
                                {
                                    this.ContinueLoading = true;
                                }
                            }
                            else
                            {
                                AuditLog.WriteLine("Unable to find Terms of Service agreement.", new object[0]);
                                this.ContinueLoading = true;
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                            this.ContinueLoading = false;
                        }
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                while (!this.ContinueLoading.HasValue)
                {
                    Thread.Sleep(100);
                }
                if (this.ContinueLoading.Value)
                {
                    if (ConfigSettings.GetBool("DoOldGameList", false))
                    {
                        if (((User.Current.IsAdmin || (GameInformation.SelectedGame.GameID == -1)) || (GameKey.BetaKeys.Count > 0)) || ((GameKey.BetaKeys.Count < 1) && (new DlgNoBetaKey(this).ShowDialog() == DialogResult.OK)))
                        {
                            this.AwaitingLogin = false;
                        }
                        else
                        {
                            User.Logout();
                            this.ContinueLoading = false;
                            this.AwaitingLogin = false;
                        }
                    }
                    else if (((User.Current.IsAdmin || (GameInformation.SelectedGame.GameID == -1)) || (GameInformation.SelectedGame.CDKey != "")) || (new DlgNoBetaKey(this).ShowDialog() == DialogResult.OK))
                    {
                        this.AwaitingLogin = false;
                    }
                    else
                    {
                        User.Logout();
                        this.ContinueLoading = false;
                        this.AwaitingLogin = false;
                    }
                }
                else
                {
                    Application.Exit();
                    this.AwaitingLogin = false;
                }
            }
            else
            {
                this.AwaitingLogin = false;
                if (OnCancelLoad != null)
                {
                    OnCancelLoad(this, EventArgs.Empty);
                }
            }
            if (((User.Current == null) || base.Disposing) || base.IsDisposed)
            {
                if (OnCancelLoad != null)
                {
                    OnCancelLoad(this, EventArgs.Empty);
                }
                base.Close();
            }
            else if (!(!this.ContinueLoading.HasValue ? false : this.ContinueLoading.Value))
            {
                if (OnCancelLoad != null)
                {
                    OnCancelLoad(this, EventArgs.Empty);
                }
                base.Close();
            }
            else
            {
                Exception exception;
                this.InitializeComponent();
                this.InitializePlugins();
                this.SetToolTips();
                this.gpgChatGrid.IsCheckingExpand = false;
                DlgMessage.RegistermainForm(this);
                DlgSupcomTeamSelection.sFrmMain = this;
                if (this.GatheringDisplaycontrol is SkinGatheringDisplay)
                {
                    (this.GatheringDisplaycontrol as SkinGatheringDisplay).MainForm = this;
                }
                this.ResetLobby();
                base.ShowInTaskbar = false;
                IntPtr handle = base.Handle;
                this.DoubleBuffered = true;
                this.SetSkin(Program.Settings.SkinName);
                this.StyleApplication(this, null);
                this.StyleChatroom(this, null);
                base.FormBorderStyle = FormBorderStyle.Sizable;
                User.LoggedOut += new EventHandler(this.LogoutLobby);
                this.SetRegion();
                this.PopulateLadderMenuItems();
                this.InitStatusDropdown();
                this.ChatFiltersChanged();
                this.SetLogo(this);
                this.ciChat_TeamInvite.DrawItem += new DrawItemEventHandler(this.miArrangedTeamsPopup_DrawItem);
                this.skinDropDownStatus.Enabled = false;
                this.textBoxMsg.Properties.MaxLength = 300;
                this.GatheringDisplaycontrol.Popup += new EventHandler(this.comboBoxGatherings_Popup);
                this.CheckControl(this);
                Messaging.NetDataRecieved += new NetDataEventHandler(FrmMain.Messaging_NetDataRecieved);
                Messaging.MessageRecieved += new MessageEventHandler(this.Messaging_MessageRecieved);
                Messaging.CommandRecieved += new MessageEventHandler(this.Messaging_CommandRecieved);
                if (handler == null)
                {
                    handler = delegate (MessageEventArgs e) {
                        this.ParseCustomCommand(e.Command, e.CommandArgs);
                    };
                }
                Messaging.CustomCommandRecieved += handler;
                if (handler2 == null)
                {
                    handler2 = delegate (object s, EventArgs e) {
                        base.Invalidate(false);
                    };
                }
                base.SizeChanged += handler2;
                if (handler3 == null)
                {
                    handler3 = delegate (object s, EventArgs e) {
                        if (this.ActiveWindow != null)
                        {
                            this.ActiveWindow = null;
                        }
                    };
                }
                base.Activated += handler3;
                base.MouseDown += new MouseEventHandler(this.this_MouseDown);
                this.pbTop.MouseDown += new MouseEventHandler(this.this_MouseDown);
                this.pbTopLeft.MouseDown += new MouseEventHandler(this.this_MouseDown);
                this.pbTopRight.MouseDown += new MouseEventHandler(this.this_MouseDown);
                this.pbBottomRight.MouseDown += new MouseEventHandler(this.this_MouseDown);
                base.MouseUp += new MouseEventHandler(this.this_MouseUp);
                this.pbTop.MouseUp += new MouseEventHandler(this.this_MouseUp);
                this.pbTopLeft.MouseUp += new MouseEventHandler(this.this_MouseUp);
                this.pbTopRight.MouseUp += new MouseEventHandler(this.this_MouseUp);
                this.gvChat.RowSeparatorHeight = Program.Settings.Chat.Appearance.ChatLineSpacing;
                this.BindToSettings();
                this.PreLoadChat();
                while (this.AwaitingLogin && (!base.Disposing && !base.IsDisposed))
                {
                    Thread.Sleep(100);
                }
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnGetConfigs", new object[] { "GetAllConfigs" });
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "AddMenuItems", new object[] { "GetMenuItems" });
                try
                {
                    ThreadQueue.QueueUserWorkItem(delegate (object state) {
                        DataAccess.ExecuteQuery("SetIP2", new object[] { User.HashPassword(DlgLogin.LastPass) });
                        DlgLogin.LastPass = "";
                    }, new object[0]);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                }
                ThreadQueue.QueueUserWorkItem(delegate (object state) {
                    DataAccess.ExecuteQuery("AcceptAllLadderChallenges", new object[] { User.Current.ID });
                }, new object[0]);
                if (User.Current.IsAdmin)
                {
                    this.miTranslate.Visible = true;
                }
                if (AccessControlList.HasAccessTo("PatchAdmins"))
                {
                    this.miTools_LocPatches.Visible = true;
                    this.miTools_LocPatches.Enabled = true;
                }
                if (AccessControlList.HasAccessTo("PrizeWinners"))
                {
                    this.miGame_RedeemPrize.Visible = true;
                    this.miGame_RedeemPrize.Enabled = true;
                }
                try
                {
                    this.CreateGameList();
                    this.RefreshGameList();
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                }
            }
        }

        private void _TeamAutomatchExit()
        {
            try
            {
                this.mSearchingForAutomatch = false;
                this.mIsInGame = false;
                this.ShowWelcome();
                this.mSupcomGameManager = null;
                this.ChangeStatus("<LOC>Online", StatusIcons.online);
                this.SetStatusButtons(0);
                this.EnableControls();
                if (this.IsInTeamGame)
                {
                    this.DisableGameButtons();
                    this.DlgTeamGame.Automatch_OnExit();
                }
                if (this.BeforeTeamGameChatroom != null)
                {
                    this.CloseHomePage();
                    this.JoinChat(this.BeforeTeamGameChatroom);
                    this.BeforeTeamGameChatroom = null;
                }
                this.SubmitReplay();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void AbortChallenge(string senderName, string custommessage)
        {
            base.Invoke((VGen0)delegate {
                this.mCanDirectChallenge = true;
                if (custommessage == "Standard")
                {
                    DlgMessage.Show(this, senderName + " " + Loc.Get("<LOC>is not accepting your challenge at this time."));
                    this.SystemEvent(senderName + " " + Loc.Get("<LOC>is not accepting your challenge at this time."), new object[0]);
                }
                else
                {
                    SupcomAutomatch.GetSupcomAutomatch().RemoveMatch();
                    SupcomAutomatch.GetSupcomAutomatch().ResetLadderGame();
                    DlgMessage.Show(this, Loc.Get(custommessage));
                    this.SystemEvent(Loc.Get(custommessage), new object[0]);
                }
            });
        }

        private void aboutGPGNetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowDlgAbout();
        }

        internal void AcceptGameInvite(string hostName)
        {
            this.PendingJoinFrom = hostName;
            Messaging.SendCustomCommand(hostName, CustomCommands.GameLobbyMessage, new object[] { 0x12 });
        }

        internal void AddChat(User user, string message)
        {
            try
            {
                VGen0 method = null;
                VGen1 gen2 = null;
                bool scroll;
                if (!user.IsIgnored)
                {
                    if (message.IndexOf("I am the effect: ") >= 0)
                    {
                        this.RecieveChatEffect(user.Name, message.Replace("I am the effect: ", ""));
                    }
                    else
                    {
                        message = Profanity.MaskProfanity(message);
                        message = message.Replace("\t", "  ");
                        if (Program.Settings.Sound.Speech.EnableSpeech)
                        {
                            Speech.Speak(string.Format("{0} says,, {1}", user.Name, message), false);
                        }
                        scroll = true;
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    scroll = !this.gvChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvChat);
                                };
                            }
                            base.Invoke(method);
                        }
                        else if (!(base.Disposing || base.IsDisposed))
                        {
                            scroll = !this.gvChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvChat);
                        }
                        ChatLine line = new ChatLine(this.gpgChatGrid);
                        line.Tag = user;
                        line.PlayerInfo = user.Name;
                        line.Text = message;
                        line.Icon = null;
                        line.TextColor = Program.Settings.Chat.Appearance.DefaultColor;
                        line.TextFont = Program.Settings.Chat.Appearance.DefaultFont;
                        line.Filters["Self"] = user.IsCurrent;
                        line.Filters["System"] = user.Equals(User.System);
                        line.Filters["Game"] = user.Equals(User.Game);
                        line.Filters["Event"] = user.Equals(User.Event);
                        line.Filters["Error"] = user.Equals(User.Error);
                        line.Filters["Admin"] = !user.IsCurrent && user.IsAdmin;
                        line.Filters["Clan"] = !user.IsCurrent && user.IsClanMate;
                        line.Filters["Friend"] = user.IsFriend;
                        line.Filters["Other"] = ((!user.IsSystem && !user.IsAdmin) && (!user.IsClanMate && !user.IsFriend)) && !user.IsCurrent;
                        line.Filters["Speaking"] = true;
                        line.Filters[user.Name] = true;
                        this.StyleChatLine(line);
                        if (this.PlayerChatEffects.ContainsKey(user.Name))
                        {
                            foreach (TextSegment segment in line.TextSegments)
                            {
                                segment.Effect = this.PlayerChatEffects[user.Name];
                            }
                        }
                        if (user != User.System)
                        {
                            this.AddSpeaker(user);
                        }
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (gen2 == null)
                            {
                                gen2 = delegate (object objline) {
                                    this.mChatLines.Add((ChatLine) objline);
                                    if (scroll)
                                    {
                                        this.gvChat.MoveLastVisible();
                                    }
                                };
                            }
                            base.BeginInvoke(gen2, new object[] { line });
                        }
                        else if (!base.Disposing && !base.IsDisposed)
                        {
                            this.mChatLines.Add(line);
                            if ((!base.IsDisposed && !base.Disposing) && scroll)
                            {
                                this.gvChat.MoveLastVisible();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void AddChatParticipant(User user)
        {
            VGen0 target = null;
            try
            {
                if (!Chatroom.InChatroom)
                {
                    ErrorLog.WriteLine("Attempt to add user {0} to chat while not in a chatroom", new object[] { user });
                }
                else if ((user.Online && user.Visible) || User.Current.IsAdmin)
                {
                    if (target == null)
                    {
                        target = delegate {
                            this.pnlUserListChat.AddUser(user);
                        };
                    }
                    this.ChatroomQueue.Enqueue(target, new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void AddClanParticipant(ClanMember member)
        {
            this.pnlUserListClan.RefreshData();
        }

        private void AddFriendParticipant(User friend)
        {
            this.pnlUserListFriends.AddUser(friend);
        }

        private void AddMenuItems(DataList result)
        {
            foreach (DataRecord record in result)
            {
                if (record.Count == 4)
                {
                    string str = record[0];
                    string str2 = record[1];
                    string name = record[2];
                    string str4 = record[3];
                    ToolStripMenuItem menuItem = this.GetMenuItem(this.msMainMenu.Items, name);
                    ToolStripMenuItem item2 = new ToolStripMenuItem();
                    item2.Text = str2;
                    item2.Tag = str4;
                    item2.Click += new EventHandler(this.ServerFormMenuItemClick);
                    item2.Name = "miFORM" + str;
                    if (menuItem != null)
                    {
                        menuItem.DropDownItems.Add(item2);
                    }
                    else
                    {
                        this.msMainMenu.Items.Add(item2);
                    }
                }
            }
            this.ShowWelcome();
        }

        public void AddMiddleButton(SkinStatusButton button)
        {
            this.pbMiddle.Add(button);
        }

        private void AddSpeaker(User user)
        {
            ElapsedEventHandler handler = null;
            if (Program.Settings.Chat.ShowSpeaking && Chatroom.InChatroom)
            {
                if (this.SpeakingTimers.ContainsKey(user.Name))
                {
                    this.SpeakingTimers[user.Name].Interval = this.SpeakingInterval;
                }
                else
                {
                    TextContainer<StatusTextLine> container;
                    VGen0 gen = null;
                    StatusTextLine userLine = this.ChatContainers.Pop(user.Name, out container);
                    if (userLine != null)
                    {
                        if (container.Count < 1)
                        {
                            this.RefreshData = true;
                        }
                        if (gen == null)
                        {
                            gen = delegate {
                                if (this.ChatContainerLookup["Speaking"].Count < 1)
                                {
                                    this.RefreshData = true;
                                }
                                this.ChatContainerLookup["Speaking"].Add(userLine);
                            };
                        }
                        this.ChatroomQueue.Enqueue(gen, new object[0]);
                    }
                    StateTimer timer = new StateTimer((double) this.SpeakingInterval);
                    timer.AutoReset = false;
                    timer.State = user;
                    if (handler == null)
                    {
                        handler = delegate (object sender, ElapsedEventArgs e) {
                            VGen0 target = null;
                            User _user = (User) (sender as StateTimer).State;
                            if (this.SpeakingTimers.ContainsKey(_user.Name))
                            {
                                this.SpeakingTimers.Remove(_user.Name);
                                if (this.ChatContainerLookup["Speaking"].Remove(_user.Name))
                                {
                                    if (this.ChatContainerLookup["Speaking"].Count < 1)
                                    {
                                        this.RefreshData = true;
                                    }
                                    if (target == null)
                                    {
                                        target = delegate {
                                            this.AddChatParticipant(_user);
                                        };
                                    }
                                    this.ChatroomQueue.Enqueue(target, new object[0]);
                                }
                            }
                        };
                    }
                    timer.Elapsed += handler;
                    this.SpeakingTimers[user.Name] = timer;
                    timer.Start();
                }
            }
        }

        private void AddUser(object[] args)
        {
            try
            {
                if (!Chatroom.InChatroom)
                {
                    ErrorLog.WriteLine("Attempt to add user {0} to chat while not in a chatroom", new object[] { args[0] });
                }
                else
                {
                    User item = MappedObject.FromDataString<User>((string) args[0]);
                    item.IsFriend = User.CurrentFriends.ContainsIndex("name", item.Name);
                    int index = Chatroom.GatheringParticipants.IndexOf(item);
                    if (index >= 0)
                    {
                        Chatroom.GatheringParticipants.IndexObject(item);
                        Chatroom.GatheringParticipants[index] = item;
                    }
                    else
                    {
                        Chatroom.GatheringParticipants.IndexObject(item);
                        Chatroom.GatheringParticipants.Add(item);
                    }
                    if (args.Length > 1)
                    {
                        this.SetUserStatus(item.Name, (string) args[1]);
                    }
                    this.AddChatParticipant(item);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void adhocChartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (User.Current.Name == "IN-Agent911")
            {
                new DlgAdhocGraph().Show();
            }
        }

        private void AlignChatTabs()
        {
            if (this.pManualTabs != null)
            {
                this.pManualTabs.Left = this.splitContainerBody.SplitterDistance + 10;
            }
        }

        private void AutomatchExit(object sender, EventArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.CancelRankedGame();
                        this.SupcomGameExit();
                        this.mIsInGame = false;
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.CancelRankedGame();
                this.SupcomGameExit();
                this.mIsInGame = false;
            }
        }

        private void AutomatchGame()
        {
            this.BeforeGameChatroom = Chatroom.CurrentName;
            this.PlayRankedGame(false);
        }

        private void AutomatchLaunchGame(object sender, EventArgs e)
        {
            VGen0 method = null;
            this.mIsInGame = true;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.btnRankedGame.Enabled = false;
                        this.btnArrangedTeam.Enabled = false;
                        this.btnPlayNow.Enabled = false;
                        this.pbMiddle.Remove(this.StatusButtonRankedGameCancel);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.btnRankedGame.Enabled = false;
                this.btnArrangedTeam.Enabled = false;
                this.btnPlayNow.Enabled = false;
                this.pbMiddle.Remove(this.StatusButtonRankedGameCancel);
            }
            this.SetStatus("", new object[0]);
            this.ChangeStatus("<LOC>Supreme Commander", StatusIcons.in_game);
            this.mSearchingForAutomatch = false;
            this.ShowWelcome(this.UrlById("GAME"));
            this.DisableControls();
        }

        private void AutomatchStatusChanged(string text, object[] args)
        {
            VGen1 method = null;
            this.mAutoStatus = text;
            if (!base.IsDisposed && !base.Disposing)
            {
                if (method == null)
                {
                    method = delegate (object arg) {
                        this.StatusButtonRankedGameCancel.Visible = (bool) arg;
                        if (this.PlayNowMatch)
                        {
                            this.btnPlayNow.Enabled = (bool) arg;
                        }
                        else
                        {
                            this.btnRankedGame.Enabled = (bool) arg;
                        }
                    };
                }
                base.BeginInvoke(method, new object[] { args[0] });
            }
        }

        private void AwayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            VGen0 method = null;
            if (!base.Disposing && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        try
                        {
                            TimeSpan span = (TimeSpan) (e.SignalTime - this.LastAction);
                            int totalMilliseconds = (int) span.TotalMilliseconds;
                            if (totalMilliseconds >= this.AwayTimer.Interval)
                            {
                                this.SetAwayStatus(true);
                            }
                            else
                            {
                                this.AwayTimer.Interval = this.AwayInterval - totalMilliseconds;
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                base.BeginInvoke(method);
            }
        }

        internal bool BanUser(string playerName)
        {
            User user;
            if (!Chatroom.InChatroom)
            {
                this.ErrorMessage("<LOC>You are not in a chatroom.", new object[0]);
                return false;
            }
            if (User.Current.Name.ToLower() == playerName.ToLower())
            {
                this.ErrorMessage("<LOC>You cannot ban yourself from chat.", new object[0]);
                return false;
            }
            if (!Chatroom.InChatroom)
            {
                this.ErrorMessage("<LOC>You are not in a chatroom.", new object[0]);
                return false;
            }
            if (!this.TryFindUser(playerName, true, out user))
            {
                this.ErrorMessage("<LOC>Unable to locate user {0}", new object[] { playerName });
                return false;
            }
            if (user.IsAdmin)
            {
                this.ErrorMessage("<LOC>You do not have permission to ban {0}, you cannot ban administrators from chat.", new object[] { playerName });
                return false;
            }
            if (Chatroom.Current.IsClanRoom)
            {
                if (!((User.Current.IsAdmin || User.Current.IsModerator) || ClanMember.Current.CanTargetAbility(ClanAbility.Ban, user.Name)))
                {
                    this.ErrorMessage("<LOC>You do not have permission to ban {0}, you must be an administrator or higher ranking clan member to ban players from chat.", new object[] { playerName });
                    return false;
                }
                if (DataAccess.ExecuteQuery("BanPlayer", new object[] { user.Name, Chatroom.CurrentName }))
                {
                    Messaging.SendCustomCommand(playerName, CustomCommands.Kick, new object[0]);
                    this.SystemMessage("<LOC>You have banned {0} from {1}", new object[] { user.Name, Chatroom.CurrentName });
                    return true;
                }
                this.ErrorMessage("<LOC>{0} has already been banned from this chatroom.", new object[] { user.Name });
                return false;
            }
            if ((!User.Current.IsAdmin && !User.Current.IsModerator) && (Chatroom.Current.IsPersistent || !User.Current.IsChannelOperator))
            {
                this.ErrorMessage("<LOC>You do not have permission to ban {0}, you must be an administrator or channel operator to ban players from chat.", new object[] { playerName });
                return false;
            }
            if (DataAccess.ExecuteQuery("BanPlayer", new object[] { user.Name, Chatroom.CurrentName }))
            {
                Messaging.SendCustomCommand(playerName, CustomCommands.Kick, new object[0]);
                this.SystemMessage("<LOC>You have banned {0} from {1}", new object[] { user.Name, Chatroom.CurrentName });
                return true;
            }
            this.ErrorMessage("<LOC>{0} has already been banned from this chatroom.", new object[] { user.Name });
            return false;
        }

        internal void BeginLadderChallengeMatch(string sessionId, int ladderId)
        {
            VGen0 method = null;
            if (((base.InvokeRequired && !base.Disposing) && !base.IsDisposed) && base.IsHandleCreated)
            {
                if (method == null)
                {
                    method = delegate {
                        this.BeginLadderChallengeMatch(sessionId, ladderId);
                    };
                }
                base.BeginInvoke(method);
            }
            else if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
            {
                if (LadderDefinition.DefaultParamsProvider == null)
                {
                    LadderDefinition.DefaultParamsProvider = typeof(SupcomParamsProvider);
                }
                LadderInstance ladder = LadderInstance.AllInstances[ladderId];
                GameParamsProvider provider = ladder.LadderDefinition.CreateParamsProvider();
                string map = provider.GetMap();
                if (map != null)
                {
                    string faction = provider.GetFaction();
                    if ((map != null) && (faction != null))
                    {
                        SupcomAutomatch supcomAutomatch = SupcomAutomatch.GetSupcomAutomatch();
                        supcomAutomatch.MapName = map;
                        supcomAutomatch.Faction = faction;
                        supcomAutomatch.RegisterLadderGame(ladder, false);
                        this.PlayRankedGame(true, sessionId, null, null);
                    }
                }
            }
        }

        private void BeginLogout()
        {
            ThreadQueue.Quazal.Enqueue(typeof(User), "Logout", this, "EndLogout", new object[0]);
            Thread thread = new Thread((ThreadStart)delegate {
                Thread.Sleep(0x2710);
                ErrorLog.WriteLine("Forced to kill process to quit.", new object[0]);
                Process.GetCurrentProcess().Kill();
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void BindToSettings()
        {
            Program.Settings.SupcomPrefs.RankedGames.FactionChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.ResetPlayNowTooltip();
            };
            Program.Settings.SupcomPrefs.RankedGames.MapsChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.ResetPlayNowTooltip();
            };
            Program.Settings.SupcomPrefs.GamePathChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.UpdateSupCom();
            };
            Program.Settings.SkinNameChanged += delegate (object s, PropertyChangedEventArgs e) {
                VGen0 method = null;
                if (base.InvokeRequired)
                {
                    if (!base.Disposing && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.SetSkin(Program.Settings.SkinName);
                            };
                        }
                        base.BeginInvoke(method);
                    }
                }
                else
                {
                    this.SetSkin(Program.Settings.SkinName);
                }
            };
            Program.Settings.Chat.Appearance.ChatLineSpacingChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.gvChat.RowSeparatorHeight = Program.Settings.Chat.Appearance.ChatLineSpacing;
            };
            Program.Settings.Chat.Filters.FilterChanged += delegate (object sender, PropertyChangedEventArgs e) {
                this.ChatFiltersChanged();
            };
            Program.Settings.StylePreferences.MasterBackColorChanged += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.MasterFontChanged += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.MasterForeColorChanged += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.HighlightColor1Changed += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.HighlightColor2Changed += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.HighlightColor3Changed += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.MenuForeColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Program.Settings.StylePreferences.StyleControl(base.MainMenuStrip);
            };
            Program.Settings.StylePreferences.MenuFontChanged += delegate (object s, PropertyChangedEventArgs e) {
                Program.Settings.StylePreferences.StyleControl(base.MainMenuStrip);
            };
            Program.Settings.Chat.Appearance.ColorsChanged += new PropertyChangedEventHandler(this.StyleChatroom);
            Program.Settings.Chat.Appearance.FontsChanged += new PropertyChangedEventHandler(this.StyleChatroom);
            Program.Settings.Chat.Links.ShowChatLinksChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.ChatRowHeights.Clear();
                this.ChatRowPoints.Clear();
                this.gvChat.RefreshData();
                foreach (ChatLine line in this.mChatLines)
                {
                    line.ContainsLinks = null;
                }
            };
            Program.Settings.Chat.Emotes.ShowEmotesChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.ChatRowHeights.Clear();
                this.ChatRowPoints.Clear();
                this.gvChat.RefreshData();
                foreach (ChatLine line in this.mChatLines)
                {
                    line.ContainsEmotes = null;
                }
            };
            Program.Settings.Chat.Links.LinkColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.gvChat.RefreshData();
            };
            Program.Settings.Chat.Links.LinkFontChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.gvChat.RefreshData();
            };
            Program.Settings.Sound.Speech.SpeechRateChanged += delegate (object s, PropertyChangedEventArgs e) {
                Speech.Rate = Program.Settings.Sound.Speech.SpeechRate;
            };
            Program.Settings.Sound.Speech.VolumeChanged += delegate (object s, PropertyChangedEventArgs e) {
                Speech.Volume = Program.Settings.Sound.Speech.Volume;
            };
            Program.Settings.Sound.Speech.VoiceChanged += delegate (object s, PropertyChangedEventArgs e) {
                Speech.Voice = Program.Settings.Sound.Speech.Voice;
            };
        }

        private void btnArrangedTeam_Click(object sender, EventArgs e)
        {
            if (this.CheckClick())
            {
                if (ConfigSettings.GetBool("UseTeamLobby", false))
                {
                    this.ShowDlgTeamGame();
                }
                else if (!DlgSupcomTeamSelection.sIsJoining)
                {
                    DlgSupcomTeamSelection.ShowSelection();
                }
            }
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            this.CloseHomePage();
        }

        private void btnChatTab_Click(object sender, EventArgs e)
        {
            this.SelectedTab.DrawColor = System.Drawing.Color.White;
            this.mSelectedTab = this.btnChatTab;
            this.SelectedTab.DrawColor = System.Drawing.Color.Black;
            this.btnChatTab.SkinBasePath = @"Controls\Button\TabSmallActive";
            this.btnFriendsTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.btnClanTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.LayoutTabs();
            this.gpgPanelGathering.BringToFront();
        }

        private void btnClanTab_Click(object sender, EventArgs e)
        {
            this.SelectedTab.DrawColor = System.Drawing.Color.White;
            this.mSelectedTab = this.btnClanTab;
            this.SelectedTab.DrawColor = System.Drawing.Color.Black;
            this.btnChatTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.btnFriendsTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.btnClanTab.SkinBasePath = @"Controls\Button\TabSmallActive";
            this.LayoutTabs();
            this.gpgPanelClan.BringToFront();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            this.ShowDlgUserFeedback();
        }

        private void btnFriendsTab_Click(object sender, EventArgs e)
        {
            this.SelectedTab.DrawColor = System.Drawing.Color.White;
            this.mSelectedTab = this.btnFriendsTab;
            this.SelectedTab.DrawColor = System.Drawing.Color.Black;
            this.btnChatTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.btnFriendsTab.SkinBasePath = @"Controls\Button\TabSmallActive";
            this.btnClanTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.LayoutTabs();
            this.gpgPanelFriends.BringToFront();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.ShowWelcome();
            this.mWelcomePage.Show();
        }

        private void btnHostGame_Click(object sender, EventArgs e)
        {
            if (this.CheckClick())
            {
                if (GameInformation.SelectedGame.IsSpaceSiege)
                {
                    this.HostGameSpaceSiege();
                }
                else
                {
                    this.HostGame();
                }
            }
        }

        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            if (this.CheckClick())
            {
                this.JoinGame();
            }
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            this.ShowDlgOptions();
        }

        private void btnPlayNow_Click(object sender, EventArgs e)
        {
            if (this.CheckClick())
            {
                if (this.mSearchingForAutomatch)
                {
                    this.CancelRankedGame();
                }
                else
                {
                    this.BeforeGameChatroom = Chatroom.CurrentName;
                    this.PlayRankedGame(true);
                }
            }
        }

        private void btnRankedGame_Click(object sender, EventArgs e)
        {
            if (!GameInformation.SelectedGame.IsSpaceSiege && this.CheckClick())
            {
                if (this.mSearchingForAutomatch)
                {
                    this.CancelRankedGame();
                }
                else
                {
                    this.AutomatchGame();
                }
            }
        }

        private void btnReplayVault_Click(object sender, EventArgs e)
        {
            this.ShowDlgSearchReplays();
        }

        private void btnVault_Click(object sender, EventArgs e)
        {
            this.ShowDlgContentManager();
        }

        private void btnViewRankings_Click(object sender, EventArgs e)
        {
            this.ShowFrmStatsLadder("1v1");
        }

        private void btnWorldMap_Click(object sender, EventArgs e)
        {
            if (DlgWorldMap.CurrentMap == null)
            {
                DlgWorldMap map = new DlgWorldMap();
                map.MainForm = this;
                map.Show();
                map.GetData();
            }
            else
            {
                DlgWorldMap.CurrentMap.Show();
                DlgWorldMap.CurrentMap.GetData();
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.SendMessage();
        }

        private void CalcGridHeight()
        {
        }

        private void CalcGridHeight(object sender, int rowHeight)
        {
            if (!this.mCalcGridHeight)
            {
                try
                {
                    if ((sender is GridView) && ((sender as GridView).GridControl != null))
                    {
                        int num3;
                        GridView view = (sender as GridView).GridControl.Views[0] as GridView;
                        int rowCount = view.RowCount;
                        for (int i = 0; i < view.RowCount; i++)
                        {
                            BaseView detailView = view.GetDetailView(view.GetVisibleRowHandle(i), 0);
                            if (detailView != null)
                            {
                                rowCount += detailView.RowCount;
                                GridView view3 = detailView as GridView;
                                if (view3 != null)
                                {
                                    num3 = -1;
                                    if (view3.SelectedRowsCount > 0)
                                    {
                                        num3 = view3.GetSelectedRows()[view3.SelectedRowsCount - 1];
                                    }
                                    view3.MoveFirst();
                                    if (num3 != -1)
                                    {
                                        view3.MoveBy(num3);
                                    }
                                }
                            }
                        }
                        view.GridControl.Height = rowCount * rowHeight;
                        if (!view.IsFirstRow)
                        {
                            num3 = -1;
                            if (view.SelectedRowsCount > 0)
                            {
                                num3 = view.GetSelectedRows()[view.SelectedRowsCount - 1];
                            }
                            view.MoveFirst();
                            if (num3 != -1)
                            {
                                view.MoveBy(num3);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                finally
                {
                    this.mCalcGridHeight = false;
                }
            }
        }

        private void CalculateClanGridHeight(object sender, EventArgs e)
        {
            this.CalcGridHeight(sender, 0x16);
        }

        private void CalculateColumnWidth()
        {
            VGen0 method = null;
            string text = "WWWWWWWWWWWWWWWWWWWWWW";
            int w = -1;
            int num = 0;
            using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
            {
                Font[] fontArray = new Font[] { Program.Settings.Chat.Appearance.AdminFont, Program.Settings.Chat.Appearance.ClanFont, Program.Settings.Chat.Appearance.DefaultFont, Program.Settings.Chat.Appearance.FriendsFont, Program.Settings.Chat.Appearance.SelfFont };
                for (int i = 0; i < fontArray.Length; i++)
                {
                    num = Convert.ToInt32(DrawUtil.MeasureString(graphics, text, fontArray[i]).Width);
                    if (num > w)
                    {
                        w = num;
                    }
                }
                num = Convert.ToInt32(DrawUtil.MeasureString(graphics, Loc.Get("<LOC>System"), Program.Settings.Chat.Appearance.SystemFont).Width);
                if (num > w)
                {
                    w = num;
                }
                num = Convert.ToInt32(DrawUtil.MeasureString(graphics, Loc.Get("<LOC>Error"), Program.Settings.Chat.Appearance.ErrorFont).Width);
                if (num > w)
                {
                    w = num;
                }
                num = Convert.ToInt32(DrawUtil.MeasureString(graphics, Loc.Get("<LOC>Event"), Program.Settings.Chat.Appearance.EventFont).Width);
                if (num > w)
                {
                    w = num;
                }
            }
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.gvChat.Columns["PlayerInfo"].Width = w;
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.gvChat.Columns["PlayerInfo"].Width = w;
            }
        }

        private void CalculateGridHeight(object sender, EventArgs e)
        {
            this.CalcGridHeight(sender, 20);
        }

        private void CalculateNameColumnWidth()
        {
            if (!this.gpgChatGrid.Disposing && !this.gpgChatGrid.IsDisposed)
            {
                GridView gvChat = this.gvChat;
                using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                {
                    VGen0 method = null;
                    int w = -1;
                    int num = 0;
                    int num2 = 0;
                    for (int i = 0; i < gvChat.RowCount; i++)
                    {
                        int visibleRowHandle = gvChat.GetVisibleRowHandle(i);
                        if (this.gvChat.IsRowVisible(visibleRowHandle) != RowVisibleState.Hidden)
                        {
                            ChatLine row = gvChat.GetRow(visibleRowHandle) as ChatLine;
                            num = Convert.ToInt32(DrawUtil.MeasureString(graphics, row.PlayerInfo, row.TextFont).Width);
                            if (num > w)
                            {
                                w = num;
                            }
                            num2++;
                        }
                    }
                    w += 10;
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                if (this.gvChat.Columns["PlayerInfo"].Width != w)
                                {
                                    this.gvChat.Columns["PlayerInfo"].Width = w;
                                    this.gpgChatGrid.Refresh();
                                }
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (this.gvChat.Columns["PlayerInfo"].Width != w)
                    {
                        this.gvChat.Columns["PlayerInfo"].Width = w;
                        this.gpgChatGrid.Refresh();
                    }
                    GPG.Logging.EventLog.WriteLine("Measured {0} lines", new object[] { num2 });
                }
            }
        }

        public void CancelRankedGame()
        {
            VGen0 method = null;
            try
            {
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.CancelRankedGame();
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!(base.Disposing || base.IsDisposed))
                {
                    this.btnFeedback.Enabled = true;
                    this.btnHostGame.Enabled = true;
                    this.btnJoinGame.Enabled = true;
                    this.btnRankedGame.Enabled = true;
                    this.btnPlayNow.Enabled = true;
                    this.btnViewRankings.Enabled = true;
                    this.btnArrangedTeam.Enabled = !this.IsInTeamGame;
                    this.miGameGroup.Enabled = true;
                    this.miRankings.Enabled = true;
                    this.btnRankedGame.Image = SkinManager.GetImage("nav-ranked_game.png");
                    this.btnPlayNow.Image = SkinManager.GetImage("nav-play_now.png");
                    this.btnRankedGame.ToolTipText = Loc.Get("<LOC>Play Ranked Game");
                    this.ResetPlayNowTooltip();
                    SupcomAutomatch.GetSupcomAutomatch().RemoveMatch();
                    SupcomAutomatch.GetSupcomAutomatch().ResetLadderGame();
                    this.mSearchingForAutomatch = false;
                    Thread.Sleep(100);
                    this.ChangeStatus("<LOC>Online", StatusIcons.online);
                    this.SetStatusButtons(0);
                    this.StatusButtonRankedGameCancel.Click -= new EventHandler(this.StatusButtonRankedGameCancel_Click);
                    this.pbMiddle.Remove(this.StatusButtonRankedGameCancel);
                    this.SetStatus("", new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal bool CanJoinChannel(string name)
        {
            CustomRoom room;
            if (((name != null) && (name.Length >= 1)) && DataAccess.TryGetObject<CustomRoom>("GetCustomChannel", out room, new object[] { name }))
            {
                if (room.Population >= room.MaxPopulation)
                {
                    this.SystemMessage("<LOC>That channel is full.", new object[0]);
                    return false;
                }
                if (room.PasswordProtected)
                {
                    string plainText = DlgAskQuestion.AskQuestion(this, "<LOC>This channel is password protected, please enter the password.", "<LOC>Password", true);
                    if ((plainText == null) || (plainText.Length < 1))
                    {
                        this.ErrorMessage("<LOC>Incorrect password.", new object[0]);
                        return false;
                    }
                    if (new GPG.Security.Hash().Encrypt(plainText) != room.Password)
                    {
                        this.ErrorMessage("<LOC>Incorrect password.", new object[0]);
                        return false;
                    }
                    return true;
                }
                return true;
            }
            return true;
        }

        public bool CanLadderChallengeUp(int ladderInstanceId)
        {
            if (!this.LastGameChallenges.ContainsKey(ladderInstanceId))
            {
                this.LastGameChallenges[ladderInstanceId] = new QuazalQuery("CanLadderChallengeUp", new object[] { ladderInstanceId, User.Current.ID }).GetBool();
            }
            return this.LastGameChallenges[ladderInstanceId];
        }

        internal void ChangeLocation(string username, Location location)
        {
            try
            {
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void ChangeStatus(string tooltip, Image icon)
        {
            base.ttDefault.SetToolTip(this.skinDropDownStatus, Loc.Get(tooltip));
            this.skinDropDownStatus.Icon = icon;
            this.skinDropDownStatus.Text = "";
        }

        private void ChatFiltersChanged()
        {
            this.gpgChatGrid.Filters["System"] = Program.Settings.Chat.Filters.FilterSystemMessages;
            this.gpgChatGrid.Filters["Event"] = Program.Settings.Chat.Filters.FilterSystemEvents;
            this.gpgChatGrid.Filters["Error"] = Program.Settings.Chat.Filters.FilterSystemErrors;
            this.gpgChatGrid.Filters["Game"] = Program.Settings.Chat.Filters.FilterGameMessages;
            this.gpgChatGrid.Filters["Admin"] = Program.Settings.Chat.Filters.FilterAdmin;
            this.gpgChatGrid.Filters["Friend"] = Program.Settings.Chat.Filters.FilterFriends;
            this.gpgChatGrid.Filters["Clan"] = Program.Settings.Chat.Filters.FilterClan;
            this.gpgChatGrid.Filters["Other"] = Program.Settings.Chat.Filters.FilterOther;
            this.gpgChatGrid.Filters["Self"] = Program.Settings.Chat.Filters.FilterSelf;
            this.gpgChatGrid.Filters["Speaking"] = false;
            this.ciChatText_Filter_Admin.Checked = Program.Settings.Chat.Filters.FilterAdmin;
            this.ciChatText_Filter_Clan.Checked = Program.Settings.Chat.Filters.FilterClan;
            this.ciChatText_Filter_Friends.Checked = Program.Settings.Chat.Filters.FilterFriends;
            this.ciChatText_Filter_Other.Checked = Program.Settings.Chat.Filters.FilterOther;
            this.ciChatText_Filter_Self.Checked = Program.Settings.Chat.Filters.FilterSelf;
            this.ciChatText_Filter_System.Checked = Program.Settings.Chat.Filters.FilterSystemMessages;
            this.ciChatText_Filter_Events.Checked = Program.Settings.Chat.Filters.FilterSystemEvents;
            this.ciChatText_Filter_Errors.Checked = Program.Settings.Chat.Filters.FilterSystemErrors;
            this.ciChatText_Filter_Game.Checked = Program.Settings.Chat.Filters.FilterGameMessages;
            this.gvChat.RefreshData();
        }

        private void ChatGridDoubleClick(object sender, EventArgs e)
        {
            if ((this.SelectedChatParticipant != null) && this.SelectedChatParticipant.Online)
            {
                this.OnSendWhisper(this.SelectedChatParticipant.Name, null);
            }
        }

        private void ChatGridMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TextLine row = (sender as GridView).GetRow((sender as GridView).FocusedRowHandle) as TextLine;
                if ((row != null) && ((row.Tag != null) && (row.Tag is User)))
                {
                    User tag = row.Tag as User;
                    this.ciChat_WhisperPlayer.Visible = !User.Current.Equals(tag) && tag.Online;
                    this.ciChat_InviteToClan.Visible = (User.Current.IsInClan && !tag.IsInClan) && !tag.IsCurrent;
                    this.ciChat_RequestClanInvite.Visible = (!User.Current.IsInClan && tag.IsInClan) && !tag.IsCurrent;
                    this.ciChat_ViewClan.Visible = tag.IsInClan;
                    this.ciChat_IgnorePlayer.Visible = !tag.IsCurrent && !tag.IsIgnored;
                    this.ciChat_UnignorePlayer.Visible = !tag.IsCurrent && tag.IsIgnored;
                    this.ciChat_WebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
                    this.ciChat_InviteFriend.Visible = !tag.IsFriend && !tag.IsCurrent;
                    this.ciChat_RemoveFriend.Visible = tag.IsFriend && !tag.IsCurrent;
                    this.ciChat_PromoteClan.Visible = tag.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Promote, tag.Name);
                    this.ciChat_DemoteClan.Visible = tag.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Demote, tag.Name);
                    this.ciChat_RemoveClan.Visible = tag.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Remove, tag.Name);
                    this.ciChat_LeaveClan.Visible = User.Current.Equals(tag) && User.Current.IsInClan;
                    if ((((((this.DlgSelectGame != null) && !this.DlgSelectGame.Disposing) && !this.DlgSelectGame.IsDisposed) || (!User.Current.IsAdmin && !this.IsGameCurrent)) || (this.IsInGame || (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable))) || ((this.DlgTeamGame != null) && ((tag.IsCurrent || (this.DlgTeamGame.Team.TeamMembers.Count >= TeamGame.MAX_TEAM_MEMBERS)) || !this.DlgTeamGame.Team.TeamLeader.IsSelf)))
                    {
                        this.ciChat_TeamInvite.Visible = false;
                    }
                    else
                    {
                        this.ciChat_TeamInvite.Visible = true;
                    }
                    if ((Chatroom.Current != null) && Chatroom.Current.IsClanRoom)
                    {
                        this.ciChat_Ban.Visible = (((User.Current.IsAdmin || User.Current.IsSubAdmin) || ClanMember.Current.CanTargetAbility(ClanAbility.Ban, tag.Name)) && !tag.IsCurrent) && !tag.IsAdmin;
                        this.ciChat_Kick.Visible = (((User.Current.IsAdmin || User.Current.IsSubAdmin) || ClanMember.Current.CanTargetAbility(ClanAbility.Kick, tag.Name)) && !tag.IsCurrent) && !tag.IsAdmin;
                    }
                    else
                    {
                        this.ciChat_Ban.Visible = (((Chatroom.InChatroom && Chatroom.GatheringParticipants.ContainsIndex("name", tag.Name)) && ((User.Current.IsAdmin || User.Current.IsSubAdmin) || (!Chatroom.Current.IsPersistent && User.Current.IsChannelOperator))) && !tag.IsCurrent) && !tag.IsAdmin;
                        this.ciChat_Kick.Visible = this.ciChat_Ban.Visible;
                    }
                    this.DisableMenuItems();
                    int num = -1;
                    for (int i = 0; i < this.gpgContextMenuChat.MenuItems.Count; i++)
                    {
                        if (this.gpgContextMenuChat.MenuItems[i].Text == "-")
                        {
                            if ((num < 0) || (this.gpgContextMenuChat.MenuItems[num].Text == "-"))
                            {
                                this.gpgContextMenuChat.MenuItems[i].Visible = false;
                            }
                            else if (i == (this.gpgContextMenuChat.MenuItems.Count - 1))
                            {
                                this.gpgContextMenuChat.MenuItems[i].Visible = false;
                            }
                            else
                            {
                                this.gpgContextMenuChat.MenuItems[i].Visible = true;
                            }
                        }
                        if (this.gpgContextMenuChat.MenuItems[i].Visible)
                        {
                            num = i;
                        }
                    }
                    if ((num >= 0) && (this.gpgContextMenuChat.MenuItems[num].Text == "-"))
                    {
                        this.gpgContextMenuChat.MenuItems[num].Visible = false;
                    }
                    this.gpgContextMenuChat.Show((sender as GridView).GridControl, e.Location);
                }
            }
        }

        private void CheckChatroomName(object sender, EventArgs e)
        {
            this.mMainChatrom = DataAccess.GetString("GetMainChatRoom", new object[] { GameInformation.SelectedGame.GameID });
        }

        private bool CheckClick()
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - this.mLastCheckTime);
            if (span.TotalSeconds < 1.0)
            {
                return false;
            }
            this.mLastCheckTime = DateTime.Now;
            return true;
        }

        private void CheckConfigs()
        {
            VGen0 method = null;
            this.mConfigSleepRate = ConfigSettings.GetInt("ConfigRefreshRate", 0x493e0);
            while (true)
            {
                Thread.Sleep(this.mConfigSleepRate);
                if (!base.IsDisposed && !base.Disposing)
                {
                    if (method == null)
                    {
                        method = delegate {
                            WaitCallback callBack = null;
                            if (!this.mFirstConfig)
                            {
                                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnGetConfigs", new object[] { "GetAllConfigs" });
                            }
                            else
                            {
                                if (callBack == null)
                                {
                                    callBack = delegate (object o) {
                                        this.OnGetConfigs(null);
                                    };
                                }
                                ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                            }
                        };
                    }
                    base.BeginInvoke(method);
                }
            }
        }

        private void CheckControl(Control control)
        {
            foreach (Control control2 in control.Controls)
            {
                this.CheckControl(control2);
            }
            control.KeyPress += new KeyPressEventHandler(this.control_KeyPress);
            control.MouseMove += new MouseEventHandler(this.OnMouseMove);
            control.MouseUp += new MouseEventHandler(this.OnMouseUp);
        }

        private bool CheckDNDOverride(string senderName)
        {
            return (AllowFriendsToBypassDND && this.IsFriendOrClanmate(senderName));
        }

        internal void CheckGameOptions()
        {
            if (Program.Settings.SupcomPrefs.LocationChanged)
            {
                Program.Settings.SupcomPrefs.LocationChanged = false;
                if (System.IO.File.Exists(Program.Settings.SupcomPrefs.GamePath))
                {
                    this.UpdateSupCom();
                    ThreadPool.QueueUserWorkItem(delegate (object o) {
                        try
                        {
                            Thread.Sleep(0x3e8);
                            SupcomMapList.RefreshMaps();
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    });
                }
                else
                {
                    this.DisableGameButtons();
                }
            }
        }

        private void CheckMenuButtons()
        {
            this.btnVault.Visible = true;
            this.miCreateTournament.Visible = true;
            this.miViewReplays.Visible = true;
            this.btnWorldMap.Visible = true;
            this.miTools_ContentManager.Visible = true;
            this.miHelp_SupComHome.Visible = true;
            this.miHelp_Volunteer.Visible = true;
            this.miConsolidateAccounts.Visible = true;
            this.miGame_Vault.Visible = true;
            this.miHelp_SupComHome.Visible = true;
            this.miForums.Visible = true;
            this.miSpaceSiegeWeb.Visible = false;
            bool flag = (((GameInformation.SelectedGame.GameDescription == ConfigSettings.GetString("Visible Supreme Commander", "Supreme Commander")) || (GameInformation.SelectedGame.GameDescription == ConfigSettings.GetString("Visible Forged Alliance", "Forged Alliance"))) || (GameInformation.SelectedGame.GameDescription == ConfigSettings.GetString("Visible Forged Alliance Beta", "Forged Alliance Beta"))) || this.IsRegularRankedGame();
            this.btnArrangedTeam.Visible = !this.IsRegularRankedGame() && flag;
            this.btnRankedGame.Visible = flag;
            this.btnReplayVault.Visible = flag;
            this.btnPlayNow.Visible = !this.IsRegularRankedGame() && flag;
            this.btnViewRankings.Visible = flag;
            this.miAutomatch.Visible = flag;
            this.miTournamentSchedule.Visible = flag;
            this.miViewReplays.Visible = flag;
            this.miRankings.Visible = flag;
            this.lOCArrangedTeamGameToolStripMenuItem.Visible = flag;
            if (GameInformation.SelectedGame.GameDescription == "Forged Alliance Beta")
            {
                this.btnReplayVault.Visible = true;
            }
            flag = GameInformation.SelectedGame.GameID != -1;
            this.btnHostGame.Visible = flag;
            this.btnJoinGame.Visible = flag;
            this.miGameGroup.Visible = flag;
            int num = 2;
            foreach (ToolStripMenuItem item in this.miLadders.DropDownItems)
            {
                if (item.Tag != null)
                {
                    LadderInstance tag = (LadderInstance) item.Tag;
                    item.Visible = tag.GameID == GameInformation.SelectedGame.GameID;
                    if (tag.GameID != GameInformation.SelectedGame.GameID)
                    {
                        num++;
                    }
                }
            }
            if (num == this.miLadders.DropDownItems.Count)
            {
                this.miLadders.Visible = false;
            }
            else
            {
                this.miLadders.Visible = flag;
            }
            if (GameInformation.SelectedGame.IsSpaceSiege || GameInformation.SelectedGame.IsChatOnly)
            {
                this.btnVault.Visible = false;
                this.btnWorldMap.Visible = false;
                this.miConsolidateAccounts.Visible = false;
                this.miCreateTournament.Visible = false;
                this.miForums.Visible = false;
                this.miGame_RedeemPrize.Visible = false;
                this.miGame_Vault.Visible = false;
                this.miHelp_ReportIssue.Visible = false;
                this.miHelp_SupComHome.Visible = false;
                this.miHelp_SupComHome.Visible = false;
                this.miHelp_Volunteer.Visible = false;
                this.miTools_ContentManager.Visible = false;
                this.miViewReplays.Visible = false;
                this.miHelp_GPGHome.Text = "gaspowered.com";
                if (GameInformation.SelectedGame.IsSpaceSiege)
                {
                    this.miSpaceSiegeWeb.Visible = true;
                }
            }
        }

        public string CheckSum(string file)
        {
            return Encryption.MD5(file);
        }

        private void CheckTitle()
        {
            this.pbTop.Invalidate();
        }

        private void CheckUpdate()
        {
            VGen0 method = null;
            GPG.Logging.EventLog.WriteLine("**** Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ****", new object[0]);
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.miGameGroup.Enabled = false;
                        this.btnHostGame.Enabled = false;
                        this.btnJoinGame.Enabled = false;
                        this.btnRankedGame.Enabled = false;
                        this.btnArrangedTeam.Enabled = false;
                        this.btnPlayNow.Enabled = false;
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.miGameGroup.Enabled = false;
                this.btnHostGame.Enabled = false;
                this.btnJoinGame.Enabled = false;
                this.btnRankedGame.Enabled = false;
                this.btnArrangedTeam.Enabled = false;
                this.btnPlayNow.Enabled = false;
            }
            this.SetStatus("<LOC>Checking for GPGnet: Supreme Commander updates...", new object[0]);
            this.Refresh();
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnGetURLs", new object[] { "GetUrls" });
        }

        private void CheckUserState()
        {
            if ((UserStatus.Current != null) && (UserStatus.Current.Name.ToLower() != "none"))
            {
                Messaging.SendGathering("I am the effect: " + UserStatus.Current.Name);
            }
        }

        private void ciChat_Ban_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.BanUser(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_DemoteClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.DemoteClanMember(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_IgnorePlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.IgnorePlayer(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_InviteFriend_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.InviteAsFriend(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_InviteToClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.InviteToClan(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_Kick_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.OnSendKick(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_LeaveClan_Click(object sender, EventArgs e)
        {
            this.OnLeaveClan();
        }

        private void ciChat_PromoteClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.PromoteClanMember(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_RemoveClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.RemoveClanMember(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_RemoveFriend_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.RemoveFriend(this.SelectedChatParticipant.Name, this.SelectedChatParticipant.ID);
            }
        }

        private void ciChat_RequestClanInvite_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.RequestClanInvite(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_TeamInvite_Click(object sender, EventArgs e)
        {
            if (ConfigSettings.GetBool("UseTeamLobby", false))
            {
                if (this.SelectedChatParticipant != null)
                {
                    this.InviteToTeamGame(this.SelectedChatParticipant.Name);
                }
            }
            else if (!DlgSupcomTeamSelection.sIsJoining && (this.SelectedChatParticipant != null))
            {
                DlgSupcomTeamSelection.ShowSelection().AddUser(this.SelectedChatParticipant);
            }
        }

        private void ciChat_UnignorePlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.UnignorePlayer(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_ViewClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.OnViewClanProfileByPlayer(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_ViewPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.OnViewPlayerProfile(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_ViewRank_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.FindOnLadder(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_WebStats_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.ShowWebStats(this.SelectedChatParticipant.ID);
            }
        }

        private void ciChat_WhisperPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.OnSendWhisper(this.SelectedChatParticipant.Name, null);
            }
        }

        private void ciChatText_Ban_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.BanUser(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_ClanInvite_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.InviteToClan(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_ClanRemove_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.RemoveClanMember(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_ClanRequest_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.RequestClanInvite(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_Clear_Click(object sender, EventArgs e)
        {
            this.ChatRowPoints.Clear();
            this.ChatRowHeights.Clear();
            this.mChatLines.Clear();
            this.gvChat.RefreshData();
        }

        private void ciChatText_Copy_Click(object sender, EventArgs e)
        {
            this.gvChat.CopyToClipboard();
        }

        private void ciChatText_Demote_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.DemoteClanMember(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_Filter_Admin_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Admin.Checked = !this.ciChatText_Filter_Admin.Checked;
            Program.Settings.Chat.Filters.FilterAdmin = this.ciChatText_Filter_Admin.Checked;
        }

        private void ciChatText_Filter_Clan_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Clan.Checked = !this.ciChatText_Filter_Clan.Checked;
            Program.Settings.Chat.Filters.FilterClan = this.ciChatText_Filter_Clan.Checked;
        }

        private void ciChatText_Filter_Errors_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Errors.Checked = !this.ciChatText_Filter_Errors.Checked;
            Program.Settings.Chat.Filters.FilterSystemErrors = this.ciChatText_Filter_Errors.Checked;
        }

        private void ciChatText_Filter_Events_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Events.Checked = !this.ciChatText_Filter_Events.Checked;
            Program.Settings.Chat.Filters.FilterSystemEvents = this.ciChatText_Filter_Events.Checked;
        }

        private void ciChatText_Filter_Friends_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Friends.Checked = !this.ciChatText_Filter_Friends.Checked;
            Program.Settings.Chat.Filters.FilterFriends = this.ciChatText_Filter_Friends.Checked;
        }

        private void ciChatText_Filter_Game_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Game.Checked = !this.ciChatText_Filter_Game.Checked;
            Program.Settings.Chat.Filters.FilterGameMessages = this.ciChatText_Filter_Game.Checked;
        }

        private void ciChatText_Filter_Other_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Other.Checked = !this.ciChatText_Filter_Other.Checked;
            Program.Settings.Chat.Filters.FilterOther = this.ciChatText_Filter_Other.Checked;
        }

        private void ciChatText_Filter_Self_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Self.Checked = !this.ciChatText_Filter_Self.Checked;
            Program.Settings.Chat.Filters.FilterSelf = this.ciChatText_Filter_Self.Checked;
        }

        private void ciChatText_Filter_System_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_System.Checked = !this.ciChatText_Filter_System.Checked;
            Program.Settings.Chat.Filters.FilterSystemMessages = this.ciChatText_Filter_System.Checked;
        }

        private void ciChatText_Filters_Reset_Click(object sender, EventArgs e)
        {
            this.ResetChatFilters();
            this.ChatFiltersChanged();
        }

        private void ciChatText_FriendInvite_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.InviteAsFriend(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_FriendRemove_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.RemoveFriend(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_Ignore_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.IgnorePlayer(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_Kick_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.OnSendKick(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_LeaveClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.OnLeaveClan();
            }
        }

        private void ciChatText_PrivateMsg_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.OnSendWhisper(this.SelectedChatTextSender.Name, null);
            }
        }

        private void ciChatText_Promote_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.PromoteClanMember(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_Replays_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.ShowDlgSearchReplays(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_ShowEmotes_Click(object sender, EventArgs e)
        {
            Program.Settings.Chat.Emotes.ShowEmotes = !Program.Settings.Chat.Emotes.ShowEmotes;
        }

        private void ciChatText_Solution_Click(object sender, EventArgs e)
        {
            DlgSolutionLink link = new DlgSolutionLink(this);
            if (link.ShowDialog() == DialogResult.OK)
            {
                int solutionID = link.SolutionID;
                link.Dispose();
                link = null;
                if (this.SelectedChatTextSender != null)
                {
                    Messaging.SendCustomCommand(CustomCommands.PlayerMessage, new object[] { "<LOC>{0}: Check knowledge base no. {1}{2} for help.", this.SelectedChatTextSender.Name, "solution:", solutionID });
                }
                else
                {
                    Messaging.SendCustomCommand(CustomCommands.PlayerMessage, new object[] { "<LOC>Check knowledge base no. {1}{2} for help.", this.SelectedChatTextSender.Name, "solution:", solutionID });
                }
            }
        }

        private void ciChatText_Unignore_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.UnignorePlayer(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_ViewClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.OnViewClanProfileByPlayer(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_ViewPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.OnViewPlayerProfile(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_ViewRank_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatTextSender != null)
            {
                this.FindOnLadder(this.SelectedChatTextSender.Name);
            }
        }

        private void ciChatText_WebStats_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.ShowWebStats(this.SelectedChatParticipant.ID);
            }
        }

        private void ciEmote_Animate_Click(object sender, EventArgs e)
        {
            Program.Settings.Chat.Emotes.AnimateEmotes = !Program.Settings.Chat.Emotes.AnimateEmotes;
        }

        private void ciEmote_Delete_Click(object sender, EventArgs e)
        {
            if (this.SelectedEmote != null)
            {
                this.SelectedEmote.Delete(true, this);
            }
        }

        private void ciEmote_Hide_Click(object sender, EventArgs e)
        {
            Program.Settings.Chat.Emotes.ShowEmotes = !Program.Settings.Chat.Emotes.ShowEmotes;
        }

        private void ciEmote_Manager_Click(object sender, EventArgs e)
        {
            this.ShowDlgEmotes();
        }

        private void ciEmote_Share_Click(object sender, EventArgs e)
        {
            Program.Settings.Chat.Emotes.AutoShareEmotes = !Program.Settings.Chat.Emotes.AutoShareEmotes;
        }

        private bool ClanChangeRank(ClanMember member, int newRank)
        {
            if (DataAccess.ExecuteQuery("ChangeRank", new object[] { newRank, member.ID, Clan.Current.ID }))
            {
                member.Rank = newRank;
                return true;
            }
            this.SystemMessage("<LOC>Unable to change the clan member's rank at this time.", new object[0]);
            return false;
        }

        private void ClanGridRowExpandCollapse(object sender, CustomMasterRowEventArgs e)
        {
            this.CalcGridHeight(sender, 0x16);
        }

        private void ClearChatRowCache()
        {
            this.ChatRowPoints.Clear();
            this.ChatRowHeights.Clear();
        }

        private void ClearParticipants()
        {
            Chatroom.GatheringParticipants.Clear();
            this.ChatroomQueue.Enqueue((VGen0)delegate {
                this.pnlUserListChat.ClearData();
            }, new object[0]);
        }

        public void ClearStatus()
        {
            try
            {
                this.SetStatus("", new object[0]);
                for (int i = this.pbMiddle.Controls.Count - 1; i >= 0; i--)
                {
                    SkinStatusButton item = this.pbMiddle.Controls[i] as SkinStatusButton;
                    if (item != null)
                    {
                        this.pbMiddle.Remove(item);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            VGen0 method = null;
            if (!e.Cancelled)
            {
                if (method == null)
                {
                    method = delegate {
                        base.Visible = false;
                    };
                }
                base.Invoke(method);
                this.SetStatus("<LOC>GPGnet: Supreme Commander update found", new object[0]);
                DlgMessage.ShowDialog(Loc.Get("<LOC id=_75d3b0e16893eb26c0d481d02cd84e85>An updated version of GPGnet is available. Click OK to shut down and automatically download and install the update."));
                ProcessStartInfo startInfo = new ProcessStartInfo(this.mPatchFileName);
                startInfo.UseShellExecute = false;
                startInfo.WorkingDirectory = Path.GetDirectoryName(this.mPatchFileName);
                Process process = Process.Start(startInfo);
                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }
        }

        private void CloseClick(object sender, EventArgs e)
        {
            try
            {
                if (ConfigSettings.GetBool("AbortSupcomOnClose", true) && this.mIsInGame)
                {
                    if (this.mSupcomGameManager != null)
                    {
                        this.mSupcomGameManager.ForceCloseGame(Loc.Get("<LOC>Aborting game due to GPGnet closing."));
                    }
                    else if (SupcomAutomatch.GetSupcomAutomatch().GetManager() != null)
                    {
                        SupcomAutomatch.GetSupcomAutomatch().GetManager().ForceCloseGame(Loc.Get("<LOC>Aborting game due to GPGnet closing."));
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            GPG.Logging.EventLog.WriteLine("Exiting application", new object[0]);
            DlgSupcomTeamSelection.LeaveTeam();
            Thread.Sleep(100);
            foreach (object obj2 in Process.GetCurrentProcess().Threads)
            {
                Thread thread = obj2 as Thread;
                if (thread != null)
                {
                    thread.Abort();
                }
            }
            this.mShuttingDown = true;
            Application.Exit();
        }

        internal void CloseHomePage()
        {
            this.CloseHomePage(this.MainChatroom);
        }

        internal void CloseHomePage(string chatroom)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.EnableControls();
                if (this.IsInTeamGame || this.IsViewingCustomGames)
                {
                    this.DisableGameButtons();
                }
                if (!this.mRanInitializeChat)
                {
                    this.InitializeChat(chatroom);
                }
                this.mWelcomePage.Hide();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void comboBoxGatherings_Popup(object sender, EventArgs e)
        {
            this.GatheringDisplaycontrol.Popup -= new EventHandler(this.comboBoxGatherings_Popup);
            if (ConfigSettings.GetBool("DoOldGameList", false))
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshGatherings", new object[] { "GetPersistentRooms" });
            }
            else
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshGatherings", new object[] { "GetPersistentRooms2", new object[] { GameInformation.SelectedGame.GameID } });
            }
        }

        private void CommandSelected(object sender, TextValEventArgs e)
        {
            if (e.Value != null)
            {
                string str = e.Value.Value.ToString();
                UserAction action = e.Value as UserAction;
                for (int i = 0; i < action.CommandWords.Length; i++)
                {
                    if (action.CommandWords[i].ToLower() == this.textBoxMsg.Text.ToLower())
                    {
                        str = action.CommandWords[i];
                        break;
                    }
                    if (action.CommandWords[i].ToLower().StartsWith(this.textBoxMsg.Text.ToLower()))
                    {
                        str = action.CommandWords[i];
                    }
                }
                this.textBoxMsg.Text = str.TrimEnd(new char[] { ' ' });
            }
            this.textBoxMsg.Select();
            this.textBoxMsg.Select(this.textBoxMsg.Text.Length, 0);
        }

        private void ConfirmChallenge(string senderName, string gamedesc)
        {
            VGen0 method = null;
            if (senderName == this.mDirectChallengePlayer)
            {
                this.mCanDirectChallenge = true;
                if (method == null)
                {
                    method = delegate {
                        this.PlayRankedGame(false, gamedesc, null, "", false);
                    };
                }
                base.Invoke(method);
            }
            else
            {
                Messaging.SendCustomCommand(senderName, CustomCommands.AutomatchAbortDirectChallenge, new object[] { "<LOC>This challenge has timed out and will no longer be accepted." });
            }
        }

        private void control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((sender != this.textBoxMsg) && !(sender is MaskBox)) && (!(base.ActiveControl is MaskBox) && !(base.ActiveControl is TextBoxMaskBox)))
            {
                this.textBoxMsg.Focus();
                if (TextUtil.IsDisplayChar(e.KeyChar))
                {
                    this.textBoxMsg.Text = this.textBoxMsg.Text + e.KeyChar;
                }
                this.textBoxMsg.SelectionStart = this.textBoxMsg.Text.Length;
            }
        }

        [DllImport("kernel32", EntryPoint="RtlMoveMemory")]
        public static extern void CopyMemA(int dest, IntPtr src, int bytes);
        [DllImport("kernel32", EntryPoint="RtlMoveMemory")]
        public static extern void CopyMemB(IntPtr dest, IntPtr src, int bytes);
        internal void CreateChannelIfNonExist(string channel)
        {
            if (DataAccess.GetBool("CustomChannelExists", new object[] { channel }))
            {
                this.JoinChat(channel);
            }
            else
            {
                new DlgCreateChannel(this, channel).ShowDialog();
            }
        }

        private void CreateGameList()
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Enabled = false;
            item.Image = new Bitmap(10, 0x19);
            item.BackgroundImage = this.btnFeedback.BackgroundImage;
            item.Width = 10;
            item.Height = 0x34;
            item.AutoSize = false;
            this.msQuickButtons.Items.Insert(0, item);
            foreach (GameInformation information in GameInformation.Games)
            {
                if (information.GameDescription.ToUpper() != "GPGNET")
                {
                    item = new ToolStripMenuItem();
                    item.AutoSize = false;
                    item.ToolTipText = information.GameDescription;
                    item.AutoToolTip = true;
                    item.Text = information.GameDescription;
                    item.Image = information.GameIcon;
                    item.BackgroundImage = this.btnFeedback.BackgroundImage;
                    item.Width = 0x23;
                    item.Height = 0x34;
                    item.Tag = information;
                    item.Click += new EventHandler(this.item_Click);
                    item.Visible = System.IO.File.Exists(information.GameLocation);
                    this.msQuickButtons.Items.Insert(0, item);
                    this.mGameMenuItems.Add(item);
                }
            }
            item = new ToolStripMenuItem();
            item.Enabled = false;
            item.Image = new Bitmap(10, 0x19);
            item.BackgroundImage = this.btnFeedback.BackgroundImage;
            item.Width = 10;
            item.Height = 0x34;
            item.AutoSize = false;
            this.msQuickButtons.Items.Insert(0, item);
            GameInformation.OnNewGameLocation += new EventHandler(this.GameInformation_OnNewGameLocation);
            this.msQuickButtons.ShowItemToolTips = false;
            this.msQuickButtons.ShowItemToolTips = true;
            GameInformation.OnSelectedGameChange += new EventHandler(this.GameInformation_OnSelectedGameChange);
            GameInformation.SetServerSelectedGame();
            this.RefreshGameList();
            this.CheckMenuButtons();
            this.CheckTitle();
        }

        [DllImport("GDI32.DLL")]
        private static extern int CreateRectRgn(int x1, int y1, int x2, int y2);
        [DllImport("GDI32.DLL")]
        private static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);
        internal void DemoteClanMember(string memberName)
        {
            if (((Clan.Current == null) || (Clan.CurrentMembers == null)) || (ClanMember.Current == null))
            {
                this.ErrorMessage("<LOC>You are not in a clan.", new object[0]);
            }
            else
            {
                ClanMember member;
                if (!Clan.CurrentMembers.TryFindByIndex("name", memberName, out member))
                {
                    this.ErrorMessage("<LOC>{0} is not a member of your clan.", new object[] { memberName });
                }
                else if (!ClanMember.Current.CanTargetAbility(ClanAbility.Demote, memberName))
                {
                    this.ErrorMessage("<LOC>You cannot demote {0} because he/she outranks you.", new object[] { memberName });
                }
                else if (this.ClanChangeRank(member, member.Rank + 1))
                {
                    this.RefreshClan(string.Format(Loc.Get("<LOC>{0} has demoted {1} to the rank of {2}."), User.Current.Name, memberName, member.GetRanking().Description), "<LOC>{0} has demoted {1} to the rank of {2}.", new object[] { User.Current.Name, memberName, member.GetRanking().Description });
                }
                else
                {
                    this.ErrorMessage("<LOC>A problem was encountered demoting {0}", new object[] { memberName });
                }
            }
        }

        internal void DestroyGameLobby()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.DestroyGameLobby();
                    };
                }
                base.Invoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                if (this.CurrentGameLobby != null)
                {
                    this.CurrentGameLobby.Close();
                    this.CurrentGameLobby = null;
                }
                this.EnableGameButtons();
            }
        }

        public void DirectChallengePlayer(string name)
        {
            if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
            {
                DlgMessage.Show(this, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>You cannot challenge while in the ranked queue."));
            }
            if (!this.mCanDirectChallenge)
            {
                DlgMessage.Show(this, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>You cannot issue multiple challenges."));
            }
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                VGen0 method = null;
                VGen0 gen2 = null;
                VGen0 gen3 = null;
                WaitCallback callBack = null;
                VGen0 gen4 = null;
                try
                {
                    DataList queryData;
                    bool flag = true;
                    if (!ConfigSettings.GetBool("IGNORELASTCHALLENGE", false))
                    {
                        queryData = DataAccess.GetQueryData("Last Automatch Game", new object[] { User.Current.Name });
                        if ((queryData.Count > 0) && (queryData[0][0].ToUpper().IndexOf("CHALLENGE") >= 0))
                        {
                            flag = false;
                            if (method == null)
                            {
                                method = delegate {
                                    DlgMessage.Show(this, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>Your last game was a direct challenge.  You must have 1 random automatch between each direct challenge."));
                                };
                            }
                            this.Invoke(method);
                        }
                        if (flag)
                        {
                            queryData = DataAccess.GetQueryData("Last Automatch Game", new object[] { name });
                            if ((queryData.Count > 0) && (queryData[0][0].ToUpper().IndexOf("CHALLENGE") >= 0))
                            {
                                flag = false;
                                if (gen2 == null)
                                {
                                    gen2 = delegate {
                                        DlgMessage.Show(this, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>This player cannot be challenged to a direct match.  Their last game was a direct challenge and they must first play a random automatch."));
                                    };
                                }
                                this.Invoke(gen2);
                            }
                        }
                    }
                    if (flag)
                    {
                        queryData = DataAccess.GetQueryData("Check Last Played Ranked Games", new object[] { name, name, User.Current.Name, User.Current.Name });
                        foreach (DataRecord record in queryData)
                        {
                            if ((record["player_name"] == name) || (record["player_name"] == User.Current.Name))
                            {
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            this.mCanDirectChallenge = false;
                            Messaging.SendCustomCommand(name, CustomCommands.AutomatchDirectChallenge, new object[0]);
                            this.mDirectChallengePlayer = name;
                            if (gen3 == null)
                            {
                                gen3 = delegate {
                                    this.SystemEvent(Loc.Get("<LOC>A direct challenge has been issued to " + name + "."), new object[0]);
                                };
                            }
                            this.Invoke(gen3);
                            if (callBack == null)
                            {
                                callBack = delegate (object throwaway) {
                                    Thread.Sleep(0x7530);
                                    if (!this.mCanDirectChallenge)
                                    {
                                        this.mCanDirectChallenge = true;
                                        this.mDirectChallengePlayer = "";
                                        this.SystemEvent(Loc.Get("<LOC>A direct challenge has timed out to " + name + "."), new object[0]);
                                    }
                                };
                            }
                            ThreadPool.QueueUserWorkItem(callBack);
                        }
                        else
                        {
                            if (gen4 == null)
                            {
                                gen4 = delegate {
                                    DlgMessage.Show(this, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>You can not challenge this player to ranked game.  You have played this player too recently and must first play other people before you can challenge again."));
                                };
                            }
                            this.Invoke(gen4);
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }, new object[0]);
        }

        private void DisableControls()
        {
            VGen0 method = null;
            if (!base.IsDisposed && !base.Disposing)
            {
                if (method == null)
                {
                    method = delegate {
                        this.miGameGroup.Enabled = false;
                        this.msQuickButtons.Enabled = false;
                        this.skinDropDownStatus.Enabled = false;
                    };
                }
                base.BeginInvoke(method);
            }
        }

        public void DisableGameButtons()
        {
            this.miGameGroup.Enabled = false;
            this.btnHostGame.Enabled = false;
            this.btnJoinGame.Enabled = false;
            this.btnRankedGame.Enabled = false;
            this.btnPlayNow.Enabled = false;
            this.btnArrangedTeam.Enabled = false;
        }

        private void DisableGameSelectButtons()
        {
            foreach (ToolStripMenuItem item in this.mGameMenuItems)
            {
                item.Enabled = false;
            }
        }

        private void DisableMenuItems()
        {
            try
            {
                foreach (string str in ConfigSettings.GetString("DisableMenus", "").Split(new char[] { ';' }))
                {
                    foreach (MenuItem item in this.gpgContextMenuChat.MenuItems)
                    {
                        if ((item.Text.ToUpper() == str.ToUpper()) || (item.Name.ToUpper() == str.ToUpper()))
                        {
                            item.Visible = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoMessaging_MessageRecieved(MessageEventArgs e)
        {
            try
            {
                string[] strArray = e.Message.Split(">".ToCharArray(), 2);
                string indexValue = strArray[0].Replace("<", "");
                if (indexValue != User.Current.Name)
                {
                    string message = strArray[1].Remove(0, 1);
                    if (strArray.Length == 2)
                    {
                        VGen0 target = null;
                        User user = null;
                        if (!Chatroom.GatheringParticipants.TryFindByIndex("name", indexValue, out user))
                        {
                            if (ConfigSettings.GetBool("ForceLookup", false) && this.TryFindUser(indexValue, true, out user))
                            {
                                if (target == null)
                                {
                                    target = delegate {
                                        this.AddChatParticipant(user);
                                    };
                                }
                                this.ChatroomQueue.Enqueue(target, new object[0]);
                            }
                            else
                            {
                                if (User.Current.IsAdmin && ConfigSettings.GetBool("ShowAdminSpew", false))
                                {
                                    this.SystemMessage("Dropped Message: <" + indexValue + "> " + message, new object[0]);
                                }
                                this.AddChat(User.MakeFakeUser(indexValue), message);
                                ErrorLog.WriteLine("Received Message from unknown user: {0}", new object[] { message });
                                return;
                            }
                        }
                        if ((user != null) && !User.IsUserIgnored(user.ID))
                        {
                            ChatLink[] linkArray = ChatLink.FindLinks(message, ChatLink.Emote);
                            for (int i = 0; i < linkArray.Length; i++)
                            {
                                string charSequence = EmoteLinkMask.GetCharSequence(linkArray[i]);
                                if ((charSequence == null) || (charSequence.Length < 1))
                                {
                                    message = message.Replace(ChatLink.Emote.LinkWord, "");
                                }
                                else if (Emote.AllEmotes.ContainsKey(charSequence))
                                {
                                    message = message.Replace(linkArray[i].FullUrl, charSequence);
                                }
                            }
                            if (message.Length > 300)
                            {
                                message = message.Substring(0, 300);
                            }
                            bool flag = false;
                            bool flag2 = false;
                            bool flag3 = false;
                            if (message.IndexOf("I am the effect: ") < 0)
                            {
                                if (this.mLastChatTimes.ContainsKey(user))
                                {
                                    DateTime time = (DateTime) this.mLastChatTimes[user];
                                    TimeSpan span = (TimeSpan) (DateTime.Now - time);
                                    flag = span.TotalMilliseconds < (Program.Settings.Chat.SpamInterval * 1000.0);
                                    flag2 = span.TotalSeconds < 5.0;
                                }
                                if (flag2 && this.mLastChatContent.ContainsKey(user))
                                {
                                    flag3 = e.Message == this.mLastChatContent[user].ToString();
                                }
                            }
                            if ((user.Name == TournamentCommands.sDirectorName) || user.IsAdmin)
                            {
                                flag = false;
                                flag3 = false;
                            }
                            if (!flag && !flag3)
                            {
                                if (message.IndexOf("I am the effect: ") < 0)
                                {
                                    this.mLastChatTimes[user] = DateTime.Now;
                                    this.mLastChatContent[user] = e.Message;
                                }
                                this.AddChat(user, message);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void DoParseCustomCommand(string cmd, string[] args)
        {
            CustomCommands commands = (CustomCommands) Convert.ToInt32(args[0]);
            int senderID = Convert.ToInt32(args[1]);
            string senderName = args[2];
            string[] destinationArray = new string[args.Length - 3];
            if (destinationArray.Length > 0)
            {
                Array.ConstrainedCopy(args, 3, destinationArray, 0, destinationArray.Length);
            }
            try
            {
                this.ProcessCustomCommand(commands, senderID, senderName, destinationArray);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void DoPlayerExit(string playerName)
        {
            this.RemoveChatParticipant(playerName);
        }

        private void DoPlayerJoin(string playerName)
        {
        }

        private void DoSizing()
        {
            try
            {
                this.SetRegion();
                if (this.wbMain != null)
                {
                    this.wbMain.Left = this.pbLeftBorder.Right;
                    this.wbMain.Width = (base.ClientSize.Width - this.pbLeftBorder.Width) - this.pbRightBorder.Width;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void DownloadPatch(object url)
        {
            ThreadQueue.QueueUserWorkItem(delegate (object p) {
                VGen1 method = null;
                bool isCurrent = false;
                string text = "";
                try
                {
                    DataList queryData = DataAccess.GetQueryData("GetGPGNetVersion", new object[0]);
                    if (queryData.Count == 1)
                    {
                        string str2 = queryData[0][0];
                        if (Assembly.GetExecutingAssembly().GetName().Version.ToString() == str2)
                        {
                            isCurrent = true;
                            this.IsLatestVer = true;
                        }
                        else
                        {
                            DataList list2;
                            string[] strArray = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split(new char[] { '.' });
                            string[] strArray2 = str2.Split(new char[] { '.' });
                            if (((Convert.ToInt32(strArray[0]) > Convert.ToInt32(strArray2[0])) || (Convert.ToInt32(strArray[1]) > Convert.ToInt32(strArray2[1]))) || (Convert.ToInt32(strArray[2]) > Convert.ToInt32(strArray2[2])))
                            {
                                this.IsLatestVer = true;
                                isCurrent = true;
                                list2 = DataAccess.GetQueryData("DowngradeGPGNetPatchURL", new object[] { Assembly.GetExecutingAssembly().GetName().Version.ToString() });
                                if (list2.Count == 1)
                                {
                                    DlgMessage.ShowDialog(this, "Warning", "You are being forced to downgrade your version of GPGnet.  This version of GPGnet is not compatible with this server.");
                                    text = list2[0][0];
                                    isCurrent = false;
                                    this.IsLatestVer = false;
                                }
                                else
                                {
                                    DlgMessage.Show(this, "Warning", "You are using a pre-release version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "  of GPGnet.  This server is expecting version " + str2 + " of GPGnet.  GPGnet may behave unexpectedly.");
                                }
                            }
                            else
                            {
                                list2 = DataAccess.GetQueryData("GetGPGNetPatchURL", new object[] { Assembly.GetExecutingAssembly().GetName().Version.ToString() });
                                if (list2.Count == 1)
                                {
                                    text = list2[0][0];
                                }
                            }
                        }
                    }
                }
                catch (Exception exception1)
                {
                    GPG.Logging.EventLog.WriteLine("GPGnet patching failed.", new object[0]);
                    ErrorLog.WriteLine(exception1);
                }
                if (!isCurrent)
                {
                    WebClient client = new WebClient();
                    int num = Convert.ToInt32(Application.ProductVersion.Split(new char[] { '.' })[2]) + 1;
                    string str3 = Application.StartupPath + @"\patch.exe";
                    if (text != "")
                    {
                        GPG.Logging.EventLog.WriteLine("Patch information", new object[0]);
                        GPG.Logging.EventLog.WriteLine(str3, new object[0]);
                        GPG.Logging.EventLog.WriteLine(text, new object[0]);
                        try
                        {
                            bool flag2 = true;
                            isCurrent = false;
                            flag2 = false;
                            if (method == null)
                            {
                                method = delegate (object objclient) {
                                    try
                                    {
                                        DlgDownloadProgress progress = new DlgDownloadProgress(Loc.Get("<LOC>GPGnet: Supreme Commander Patch"), objclient as WebClient);
                                        progress.DownloadCancelled += new EventHandler(this.progress_OnCancelDownload);
                                        progress.Show();
                                        this.msQuickButtons.Enabled = false;
                                        this.msMainMenu.Enabled = false;
                                        this.mWelcomePage.CanNavigate = false;
                                    }
                                    catch (Exception exception)
                                    {
                                        isCurrent = true;
                                        ErrorLog.WriteLine(exception);
                                    }
                                };
                            }
                            base.Invoke(method, new object[] { client });
                            this.mPatchFileName = str3;
                            client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.client_DownloadFileCompleted);
                            client.DownloadFileAsync(new Uri(text), str3);
                        }
                        catch (Exception exception2)
                        {
                            ErrorLog.WriteLine(exception2);
                            isCurrent = true;
                        }
                    }
                    else
                    {
                        isCurrent = true;
                    }
                }
                if (isCurrent)
                {
                    this.IsGPGNetPatching = false;
                    this.UpdateSupCom();
                }
            }, new object[] { url });
        }

        private void EmoteMessageRoom(string message)
        {
            string str = "YELL" + message;
            this.CheckUserState();
            Messaging.SendGathering(str);
            this.AddChat(User.Current, str);
        }

        private void EnableControls()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.IsDisposed) && !base.Disposing)
            {
                if (method == null)
                {
                    method = delegate {
                        this.EnableControls();
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                if (!((this.IsGPGNetPatching || this.IsSupComPatching) || this.mSearchingForAutomatch))
                {
                    this.btnFeedback.Enabled = true;
                    this.btnHostGame.Enabled = true;
                    this.btnJoinGame.Enabled = true;
                    this.btnRankedGame.Enabled = true;
                    this.btnArrangedTeam.Enabled = true;
                    this.btnPlayNow.Enabled = true;
                    this.btnViewRankings.Enabled = true;
                    this.miGameGroup.Enabled = true;
                    this.miRankings.Enabled = true;
                    this.btnRankedGame.Image = SkinManager.GetImage("nav-ranked_game.png");
                    this.btnRankedGame.ToolTipText = Loc.Get("<LOC>Play Ranked Game");
                    this.btnArrangedTeam.Image = SkinManager.GetImage("nav-ranked_team.png");
                    this.btnArrangedTeam.ToolTipText = Loc.Get("<LOC>Play Arranged Team Game");
                }
                this.msQuickButtons.Enabled = true;
                this.skinDropDownStatus.Enabled = true;
            }
        }

        public void EnableGameButtons()
        {
            this.miGameGroup.Enabled = true;
            this.btnHostGame.Enabled = true;
            this.btnJoinGame.Enabled = true;
            this.btnRankedGame.Enabled = true;
            this.btnPlayNow.Enabled = true;
            this.btnArrangedTeam.Enabled = true;
        }

        private void EnableGameSelectButtons()
        {
            foreach (ToolStripMenuItem item in this.mGameMenuItems)
            {
                item.Enabled = true;
            }
        }

        private void EndLogout()
        {
            Application.Exit();
        }

        internal void ErrorMessage(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            if (this.mRanInitializeChat)
            {
                this.AddChat(User.Error, msg);
            }
            else
            {
                VGen0 onClick = this.FindLinks(msg, out msg);
                FrmSimpleMessage.DoNotification("<LOC>Error", msg, onClick, new object[0]);
            }
        }

        internal void ExitApplication()
        {
            GPG.Logging.EventLog.WriteLine("Exiting application", new object[0]);
            this.mShuttingDown = true;
            Application.Exit();
        }

        public void FadeIn()
        {
            this.TargetFade = 1.0;
            this.FadeIncrement = 0.04;
            this.PerformFade();
        }

        public void FadeOut()
        {
            this.TargetFade = 0.0;
            this.FadeIncrement = -0.04;
            this.PerformFade();
        }

        private VGen0 FindLinks(string msg, out string result)
        {
            ChatLink[] linkArray = ChatLink.FindLinks(msg);
            if (linkArray.Length > 0)
            {
                foreach (ChatLink link in linkArray)
                {
                    msg = msg.Replace(link.FullUrl, link.DisplayText);
                }
                result = msg;
                return new VGen0(linkArray[0].OnClick);
            }
            result = msg;
            return null;
        }

        internal void FindOnLadder(string name)
        {
            FrmStatsLadder ladder = null;
            foreach (FrmStatsLadder ladder2 in this.FrmStatsLadders)
            {
                if (ladder2.Category == "1v1")
                {
                    ladder = ladder2;
                    break;
                }
            }
            if (ladder == null)
            {
                ladder = this.ShowFrmStatsLadder("1v1");
            }
            else
            {
                ladder.Activate();
            }
            ladder.JumpTo(name);
        }

        private void forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void forceSyncRoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Chatroom.GatheringParticipants.Clear();
            this.SyncChatroom(this, null);
        }

        private void FrmMain_Chat(string chatroom)
        {
            this.mRanInitializeChat = true;
            string msg = ConfigSettings.GetString("MOTD", Loc.Get("<LOC>Welcome to GPGnet!"));
            if (msg != this.mLastMOTD)
            {
                this.SystemMessage(msg, new object[0]);
                this.mLastMOTD = msg;
            }
            this.SystemMessage("<LOC>Initializing Chat...", new object[0]);
            if (ConfigSettings.GetBool("DoOldGameList", false))
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshGatherings", new object[] { "GetPersistentRooms" });
            }
            else
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshGatherings", new object[] { "GetPersistentRooms2", new object[] { GameInformation.SelectedGame.GameID } });
            }
            this.JoinChat(chatroom);
            this.textBoxMsg.Select();
        }

        private void FrmMain_Clan()
        {
            this.FrmMain_Clan(null);
        }

        private void FrmMain_Clan(VGen0 callback)
        {
            ThreadQueue.QueueUserWorkItem(delegate (object none) {
                User user;
                VGen0 gen = null;
                VGen0 gen2 = null;
                VGen0 gen3 = null;
                VGen0 gen4 = null;
                VGen0 gen5 = null;
                VGen0 gen6 = null;
                MappedObjectList<Clan> objects = DataAccess.GetObjects<Clan>("GetClanByMember2", new object[] { User.Current.Name });
                if ((objects == null) || (objects.Count <= 0))
                {
                    User.Current.ClanName = null;
                    User.Current.ClanAbbreviation = null;
                    if (Chatroom.InChatroom && Chatroom.GatheringParticipants.TryFindByIndex("name", User.Current.Name, out user))
                    {
                        user.ClanName = null;
                        user.ClanAbbreviation = null;
                    }
                    this.ClanLoaded = true;
                    if (this.FriendsLoaded)
                    {
                        this.NotifyOnLogin();
                    }
                    if (this.ClanInvites == null)
                    {
                        this.ClanInvites = DataAccess.GetObjects<ClanInvite>("GetAllClanInvites", new object[0]);
                        if (gen5 == null)
                        {
                            gen5 = delegate {
                                this.RefreshClanInviteCount();
                            };
                        }
                        this.Invoke(gen5);
                    }
                    if (gen6 == null)
                    {
                        gen6 = delegate {
                            VGen0 method = null;
                            if ((((Clan.Current != null) || (ClanMember.Current != null)) || (Clan.CurrentMembers != null)) || (Clan.CurrentMembersMsgList.Length > 0))
                            {
                                User.Current.ClanAbbreviation = null;
                                User.Current.ClanName = null;
                                Clan.Current = null;
                                Clan.CurrentMembers = null;
                                ClanMember.Current = null;
                                this.RefreshGathering();
                            }
                            if ((this.InvokeRequired && !this.Disposing) && !this.IsDisposed)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.skinLabelClanName.Text = "";
                                        this.skinLabelClanName.Cursor = Cursors.Default;
                                        this.pnlUserListClan.Visible = false;
                                        this.gpgScrollPanelClan.Visible = false;
                                        this.gpgScrollPanelNoClan.Visible = true;
                                        this.gpgScrollPanelNoClan.BringToFront();
                                    };
                                }
                                this.BeginInvoke(method);
                            }
                            else if (!(this.Disposing || this.IsDisposed))
                            {
                                this.skinLabelClanName.Text = "";
                                this.skinLabelClanName.Cursor = Cursors.Default;
                                this.pnlUserListClan.Visible = false;
                                this.gpgScrollPanelClan.Visible = false;
                                this.gpgScrollPanelNoClan.Visible = true;
                                this.gpgScrollPanelNoClan.BringToFront();
                            }
                            if (Chatroom.InChatroom && Chatroom.Current.IsClanRoom)
                            {
                                this.SystemMessage("<LOC>You have been removed from clan chat.", new object[0]);
                                this.LeaveChat();
                            }
                        };
                    }
                    this.Invoke(gen6);
                }
                else
                {
                    Clan.Current = objects[0];
                    Clan.CurrentMembers = DataAccess.GetObjects<ClanMember>("GetClanMembers", new object[] { Clan.Current.ID });
                    User.Current.ClanName = Clan.Current.Name;
                    User.Current.ClanAbbreviation = Clan.Current.Abbreviation;
                    if (Chatroom.InChatroom && Chatroom.GatheringParticipants.TryFindByIndex("name", User.Current.Name, out user))
                    {
                        user.ClanName = Clan.Current.Name;
                        user.ClanAbbreviation = Clan.Current.Abbreviation;
                    }
                    foreach (ClanMember member in Clan.CurrentMembers)
                    {
                        if (member.ID == User.Current.ID)
                        {
                            ClanMember.Current = member;
                            break;
                        }
                    }
                    if (!(!this.FriendsLoaded || this.ClanLoaded))
                    {
                        this.NotifyOnLogin();
                    }
                    this.ClanLoaded = true;
                    this.pnlUserListClan.RefreshData();
                    if (this.ClanRequests == null)
                    {
                        this.ClanRequests = DataAccess.GetObjects<ClanRequest>("GetAllClanRequests", new object[0]);
                        if (gen == null)
                        {
                            gen = delegate {
                                this.RefreshClanRequestCount();
                            };
                        }
                        this.Invoke(gen);
                    }
                    if (gen2 == null)
                    {
                        gen2 = delegate {
                            this.pnlUserListClan.AutoRefresh = true;
                            this.ClanContainers.Clear();
                            this.ClanContainerLookup.Clear();
                            this.ClanContainerLookup["Online"] = new TextContainer(Loc.Get("<LOC>Online"));
                            this.ClanContainers.Add(this.ClanContainerLookup["Online"]);
                            this.ClanContainerLookup["Offline"] = new TextContainer(Loc.Get("<LOC>Offline"));
                            this.ClanContainers.Add(this.ClanContainerLookup["Offline"]);
                        };
                    }
                    this.Invoke(gen2);
                    if ((this.InvokeRequired && !this.Disposing) && !this.IsDisposed)
                    {
                        if (gen3 == null)
                        {
                            gen3 = delegate {
                                this.pnlUserListClan.Visible = true;
                                this.gpgScrollPanelClan.Visible = true;
                                this.gpgScrollPanelNoClan.Visible = false;
                                this.skinLabelClanName.Text = Clan.Current.Name;
                                this.skinLabelClanName.ForeColor = Program.Settings.Chat.Appearance.ClanColor;
                                this.skinLabelClanName.Cursor = Cursors.Hand;
                            };
                        }
                        this.BeginInvoke(gen3);
                    }
                    else
                    {
                        if (gen4 == null)
                        {
                            gen4 = delegate {
                                this.pnlUserListClan.Visible = true;
                                this.gpgScrollPanelClan.Visible = true;
                                this.gpgScrollPanelNoClan.Visible = false;
                                this.skinLabelClanName.Text = Clan.Current.Name;
                                this.skinLabelClanName.ForeColor = Program.Settings.Chat.Appearance.ClanColor;
                                this.skinLabelClanName.Cursor = Cursors.Hand;
                            };
                        }
                        this.Invoke(gen4);
                    }
                }
                if (callback != null)
                {
                    callback();
                }
            }, new object[0]);
        }

        private void FrmMain_Friends()
        {
            ThreadQueue.Quazal.Enqueue((OGen0)delegate {
                this.FriendRequests = DataAccess.GetObjects<FriendRequest>("GetFriendRequests", new object[0]);
                return null;
            }, (VGen1)delegate (object o) {
                this.RefreshFriendInvites();
            }, new object[0]);
            ThreadQueue.QueueUserWorkItem(delegate (object none) {
                MappedObjectList<User> friends = DataAccess.GetObjects<User>("GetFriendsByPlayerID", new object[] { User.Current.ID });
                User.CurrentFriends = friends;
                if (!(!this.ClanLoaded || this.FriendsLoaded))
                {
                    this.NotifyOnLogin();
                }
                this.FriendsLoaded = true;
                ThreadPool.QueueUserWorkItem(delegate (object state) {
                    bool flag = false;
                    foreach (User user in friends)
                    {
                        User user2;
                        if (this.FriendRequests.ContainsIndex("name", user.Name))
                        {
                            flag = true;
                            this.FriendRequests.RemoveByIndex("name", user.Name);
                            ThreadQueue.Quazal.Enqueue((VGen1)delegate (object o) {
                                DataAccess.ExecuteQuery("RemoveFriendRequest", new object[] { o });
                            }, new object[] { user.ID });
                        }
                        if (Chatroom.GatheringParticipants.TryFindByIndex("name", user.Name, out user2) && !user2.IsFriend)
                        {
                            user2.IsFriend = true;
                            this.UpdateUser(user2);
                        }
                        user.IsFriend = true;
                    }
                    this.pnlUserListFriends.AddUsers(friends, false);
                    if (flag)
                    {
                        this.RefreshFriendInvites();
                    }
                });
                base.Invoke((VGen0)delegate {
                    VGen0 method = null;
                    VGen0 gen2 = null;
                    if (friends.Count > 0)
                    {
                        this.FriendContainers = new BoundContainerList();
                        this.FriendContainerLookup = new Dictionary<string, TextContainer>();
                        this.FriendContainers.Clear();
                        this.FriendContainerLookup.Clear();
                        this.FriendContainerLookup["Online"] = new TextContainer(Loc.Get("<LOC>Friends Online"));
                        this.FriendContainerLookup["Online"].AutoSort = false;
                        this.FriendContainers.Add(this.FriendContainerLookup["Online"]);
                        this.FriendContainerLookup["Offline"] = new TextContainer(Loc.Get("<LOC>Friends Offline"));
                        this.FriendContainerLookup["Offline"].AutoSort = false;
                        this.FriendContainers.Add(this.FriendContainerLookup["Offline"]);
                        if ((this.InvokeRequired && !this.Disposing) && !this.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.pnlUserListFriends.Visible = true;
                                    this.gpgLabelNoFriends.Visible = false;
                                };
                            }
                            this.BeginInvoke(method);
                        }
                        else
                        {
                            this.pnlUserListFriends.Visible = true;
                            this.gpgLabelNoFriends.Visible = false;
                        }
                        foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                        {
                            if (!(chat.Disposing || chat.IsDisposed))
                            {
                                chat.ChatTarget.IsFriend = friends.ContainsIndex("name", chat.ChatTarget.Name);
                                chat.RefreshToolstrip();
                            }
                        }
                    }
                    else
                    {
                        if ((this.InvokeRequired && !this.Disposing) && !this.IsDisposed)
                        {
                            if (gen2 == null)
                            {
                                gen2 = delegate {
                                    this.pnlUserListFriends.Visible = false;
                                    this.gpgLabelNoFriends.Visible = true;
                                };
                            }
                            this.BeginInvoke(gen2);
                        }
                        else
                        {
                            this.pnlUserListFriends.Visible = false;
                            this.gpgLabelNoFriends.Visible = true;
                        }
                        foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                        {
                            if (!(chat.Disposing || chat.IsDisposed))
                            {
                                chat.ChatTarget.IsFriend = false;
                                chat.RefreshToolstrip();
                            }
                        }
                    }
                });
            }, new object[0]);
        }

        private void FrmMain_OnAutomatchStats(List<SupComStatsInfo> stats)
        {
            VGen0 method = null;
            try
            {
                if (method == null)
                {
                    method = delegate {
                        try
                        {
                            SupcomAutomatch.GetSupcomAutomatch().OnAutomatchStats -= new AutomatchStatsDelegate(this.FrmMain_OnAutomatchStats);
                            this.SystemMessage(Loc.Get("<LOC>You have exited a ranked game.  Here are the results."), new object[0]);
                            bool flag = false;
                            foreach (SupComStatsInfo info in stats)
                            {
                                if ((info.result.ToUpper() == "DRAW") || (info.result.ToUpper() == "DEFEAT"))
                                {
                                    flag = true;
                                }
                            }
                            if (flag)
                            {
                                foreach (SupComStatsInfo info in stats)
                                {
                                    this.SystemMessage(info.playername + ": " + Loc.Get("<LOC>" + info.result), new object[0]);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                base.Invoke(method);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void FrmMain_VisibleChanged(object sender, EventArgs e)
        {
            base.ActiveControl = this.textBoxMsg;
            this.textBoxMsg.Focus();
        }

        internal void GameEvent(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            if (this.mRanInitializeChat)
            {
                this.AddChat(User.Game, msg);
            }
            else
            {
                VGen0 onClick = this.FindLinks(msg, out msg);
                FrmSimpleMessage.DoNotification("<LOC>Game Event", msg, onClick, new object[0]);
            }
        }

        private void GameInformation_OnNewGameLocation(object sender, EventArgs e)
        {
            this.RefreshGameList();
        }

        private void GameInformation_OnSelectedGameChange(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            try
            {
                GameInformation.SetServerSelectedGame();
                this.SystemMessage(string.Format(Loc.Get("<LOC>GPGnet is now using: {0}"), GameInformation.SelectedGame.GameDescription), new object[0]);
                this.CheckMenuButtons();
                this.CheckTitle();
                this.msQuickButtons.Invalidate();
                this.mMainChatrom = "";
                if (!this.mWelcomePage.Visible)
                {
                    if (callBack == null)
                    {
                        callBack = delegate (object o) {
                            if (ConfigSettings.GetBool("DoOldGameList", false))
                            {
                                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshGatherings", new object[] { "GetPersistentRooms" });
                            }
                            else
                            {
                                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshGatherings", new object[] { "GetPersistentRooms2", new object[] { GameInformation.SelectedGame.GameID } });
                            }
                            string mainChatroom = this.MainChatroom;
                            base.BeginInvoke((VGen1)delegate (object ochatroom) {
                                this.JoinChat(ochatroom.ToString());
                                this.textBoxMsg.Select();
                            }, new object[] { mainChatroom });
                        };
                    }
                    ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void GameKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.S))
            {
                this.mSupcomGameManager.GetStats();
            }
        }

        internal void GatherLadderReports()
        {
            ThreadQueue.QueueUserWorkItem(delegate (object state) {
                try
                {
                    MappedObjectList<LadderGameSession> objects = new QuazalQuery("GetUnreportedLadderGameSessions", new object[] { User.Current.ID }).GetObjects<LadderGameSession>();
                    foreach (LadderGameSession session in objects)
                    {
                        VGen0 method = null;
                        MappedObjectList<LadderGameSession> allParticipants = new QuazalQuery("GetGameSessionMembersByGameID", new object[] { session.GameID }).GetObjects<LadderGameSession>();
                        if (((base.InvokeRequired && !base.Disposing) && !base.IsDisposed) && base.IsHandleCreated)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    if (this.LadderGamePlayed != null)
                                    {
                                        this.LadderGamePlayed(this, EventArgs.Empty);
                                    }
                                    new DlgLadderReport(allParticipants.ToArray()).ShowDialog();
                                };
                            }
                            base.BeginInvoke(method);
                        }
                        else if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
                        {
                            if (this.LadderGamePlayed != null)
                            {
                                this.LadderGamePlayed(this, EventArgs.Empty);
                            }
                            new DlgLadderReport(allParticipants.ToArray()).ShowDialog();
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine("Error gathering ladder reports.", new object[0]);
                    ErrorLog.WriteLine(exception);
                }
            }, new object[0]);
        }

        private string GetDefaultBrowser()
        {
            string str = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);
                str = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                if (!str.EndsWith("exe"))
                {
                    str = str.Substring(0, str.LastIndexOf(".exe") + 4);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return "iexplore.exe";
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
            }
            return str;
        }

        public int GetLadderParticipantRank(int ladderInstanceId, int entityId)
        {
            return new QuazalQuery("GetPlayerLadderRankByID", new object[] { ladderInstanceId, entityId }).GetInt();
        }

        public int GetLadderParticipantRank(int ladderInstanceId, string entityName)
        {
            return new QuazalQuery("GetPlayerLadderRankByName", new object[] { ladderInstanceId, entityName }).GetInt();
        }

        private string GetLocalPatchFile()
        {
            return (new FileInfo(GameInformation.SelectedGame.GameLocation).DirectoryName + @"\webpatch.exe");
        }

        public int GetMembersByRank(int rank, out ClanMember[] rMembers)
        {
            ClanMember[] memberArray = Clan.CurrentMembers.ToArray();
            List<ClanMember> list = new List<ClanMember>();
            for (int i = 0; i < memberArray.Length; i++)
            {
                if (memberArray[i].Rank == rank)
                {
                    list.Add(memberArray[i]);
                }
            }
            rMembers = list.ToArray();
            return rMembers.Length;
        }

        private ToolStripMenuItem GetMenuItem(ToolStripItemCollection items, string name)
        {
            ToolStripMenuItem menuItem = null;
            foreach (ToolStripMenuItem item2 in items)
            {
                if (item2.Name == name)
                {
                    return item2;
                }
                if ((menuItem == null) && item2.HasDropDownItems)
                {
                    menuItem = this.GetMenuItem(item2.DropDownItems, name);
                }
            }
            return menuItem;
        }

        public static GameInfo[] GetRunningGames()
        {
            List<GameInfo> list = new List<GameInfo>();
            foreach (Process process in Process.GetProcesses())
            {
                IntPtr mainWindowHandle = process.MainWindowHandle;
                if (!mainWindowHandle.Equals(IntPtr.Zero) && (IsWindowVisible(mainWindowHandle) && !IsIconic(mainWindowHandle)))
                {
                    WINDOWINFO structure = new WINDOWINFO();
                    structure.cbSize = (uint) Marshal.SizeOf(structure);
                    GetWindowInfo(mainWindowHandle, ref structure);
                    GPG.Multiplayer.Client.RECT rcClient = structure.rcClient;
                    if (((rcClient.Right - rcClient.Left) >= GetSystemMetrics(SystemMetrics.SM_CXSCREEN)) && ((rcClient.Bottom - rcClient.Top) >= GetSystemMetrics(SystemMetrics.SM_CYSCREEN)))
                    {
                        GameInfo item = new GameInfo(process);
                        GPG.Logging.EventLog.WriteLine("Game {0} is running.", new object[] { item });
                        list.Add(item);
                    }
                }
            }
            return list.ToArray();
        }

        private IUser GetSelectedUser()
        {
            if (this.SelectedParticipantView != null)
            {
                int[] selectedRows = this.SelectedParticipantView.GetSelectedRows();
                if (selectedRows.Length > 0)
                {
                    TextLine row = this.SelectedParticipantView.GetRow(selectedRows[0]) as TextLine;
                    if ((row != null) && ((row.Tag != null) && (row.Tag is IUser)))
                    {
                        return (row.Tag as IUser);
                    }
                }
            }
            return null;
        }

        public SupComGameManager GetSupcomGameManager()
        {
            return this.mSupcomGameManager;
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SystemMetrics smIndex);
        [DllImport("user32.dll")]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out GPG.Multiplayer.Client.RECT lpRect);
        private void gpgLabelClanInvites_Click(object sender, EventArgs e)
        {
            this.ViewClanInvites();
        }

        private void gpgLabelClanRequests_Click(object sender, EventArgs e)
        {
            this.ViewClanRequests();
        }

        private void gpgLabelCreateClan_Click(object sender, EventArgs e)
        {
            this.OnCreateClan();
        }

        private void gpgLabelFriendInvites_Click(object sender, EventArgs e)
        {
            this.ViewFriendInvites();
        }

        private void gpgLabelFriendInvites2_Click(object sender, EventArgs e)
        {
            this.ViewFriendInvites();
        }

        private void gpgPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void GridRowExpandCollapse(object sender, int rowHeight)
        {
            if (sender is GridView)
            {
                this.CalcGridHeight(sender, rowHeight);
            }
        }

        private void gvChat_CalcRowHeight(object sender, RowHeightEventArgs e)
        {
            try
            {
                if (this.ChatRowHeights.ContainsKey(e.RowHandle))
                {
                    e.RowHeight = this.ChatRowHeights[e.RowHandle];
                }
                else
                {
                    ChatLine row = this.gvChat.GetRow(e.RowHandle) as ChatLine;
                    if (row != null)
                    {
                        IText text = row.TextSegments[0];
                        if (text != null)
                        {
                            int index;
                            Font textFont = text.TextFont;
                            string str = text.Text;
                            string str2 = null;
                            Dictionary<int, ChatLink> dictionary = null;
                            List<string> list = null;
                            List<MultiVal<int, int>> list2 = null;
                            SortedList<int, Emote> list3 = null;
                            if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                            {
                                dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                row.ContainsLinks = new bool?((dictionary != null) && (dictionary.Count > 0));
                            }
                            if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                            {
                                list = new List<string>();
                                list2 = new List<MultiVal<int, int>>();
                                list3 = new SortedList<int, Emote>();
                                SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                foreach (Emote emote in Emote.AllEmotes.Values)
                                {
                                    list4.Add(emote.CharSequence.Length, emote);
                                }
                                foreach (Emote emote in list4.Values)
                                {
                                    index = str.IndexOf(emote.CharSequence);
                                    while (index >= 0)
                                    {
                                        bool flag = false;
                                        if (dictionary != null)
                                        {
                                            foreach (ChatLink link in dictionary.Values)
                                            {
                                                if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                {
                                                    flag = true;
                                                }
                                            }
                                        }
                                        bool flag2 = false;
                                        foreach (KeyValuePair<int, Emote> pair in list3)
                                        {
                                            if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                            {
                                                flag2 = true;
                                                break;
                                            }
                                        }
                                        if (!(flag || flag2))
                                        {
                                            list2.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                            list3.Add(index, emote);
                                        }
                                        index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                        if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                        {
                                            if (str2 == null)
                                            {
                                                str2 = str;
                                            }
                                            str2 = str2.Replace(emote.CharSequence, "");
                                        }
                                    }
                                }
                                row.ContainsEmotes = new bool?((list3 != null) && (list3.Count > 0));
                            }
                            using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                            {
                                float num6;
                                float num7;
                                float num8;
                                int num9;
                                int num10;
                                ChatLink link2;
                                SolidBrush brush;
                                string[] strArray;
                                int num11;
                                SizeF ef;
                                Brush brush2;
                                Font linkFont;
                                if ((list3 != null) && (list3.Count > 0))
                                {
                                    int num2;
                                    list2.Add(new MultiVal<int, int>(str.Length, 0));
                                    list3.Add(str.Length, null);
                                    SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                    list5[-1] = new MultiVal<int, int>(0, 0);
                                    foreach (MultiVal<int, int> val in list2)
                                    {
                                        list5[val.Value1] = val;
                                    }
                                    for (num2 = 1; num2 < list5.Count; num2++)
                                    {
                                        int num3 = list5.Values[num2 - 1].Value1;
                                        index = list5.Values[num2].Value1;
                                        int num4 = list5.Values[num2 - 1].Value2;
                                        int num5 = list5.Values[num2].Value2;
                                        list.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                    }
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                    num6 = textFont.Height + 3;
                                    num7 = 0f;
                                    num8 = num6;
                                    num9 = this.colText.VisibleWidth - 10;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        for (num2 = 0; num2 < list.Count; num2++)
                                        {
                                            if (list[num2].Length > 0)
                                            {
                                                strArray = DrawUtil.SplitString(list[num2], " ");
                                                for (num11 = 0; num11 < strArray.Length; num11++)
                                                {
                                                    if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                    {
                                                        link2 = dictionary[num10];
                                                    }
                                                    if (link2 != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link2.LinkColor))
                                                        {
                                                            linkFont = link2.LinkFont;
                                                            ef = DrawUtil.MeasureString(graphics, strArray[num11], linkFont);
                                                            if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                            {
                                                                num7 = 0f;
                                                                num8 += num6;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                        if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                        {
                                                            num7 = 0f;
                                                            num8 += num6;
                                                        }
                                                    }
                                                    num10 += strArray[num11].Length;
                                                    num7 += ef.Width;
                                                    if ((link2 != null) && (num10 >= link2.EndIndex))
                                                    {
                                                        link2 = null;
                                                    }
                                                }
                                            }
                                            if (list3.Values[num2] != null)
                                            {
                                                float num12 = 1f;
                                                if (num12 < 1f)
                                                {
                                                    num12 = 1f;
                                                }
                                                float num13 = list3.Values[num2].Image.Width * num12;
                                                if ((num7 + num13) > num9)
                                                {
                                                    num7 = 0f;
                                                    num8 += num6;
                                                }
                                                num7 += num13;
                                            }
                                        }
                                    }
                                    e.RowHeight = Convert.ToInt32(Math.Round((double) num8, MidpointRounding.AwayFromZero));
                                }
                                else if ((dictionary != null) && (dictionary.Count > 0))
                                {
                                    num6 = textFont.Height + 3;
                                    num7 = 0f;
                                    num8 = num6;
                                    num9 = this.colText.VisibleWidth - 10;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        strArray = DrawUtil.SplitString(str, " ");
                                        num11 = 0;
                                        while (num11 < strArray.Length)
                                        {
                                            if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                            {
                                                link2 = dictionary[num10];
                                            }
                                            if (link2 != null)
                                            {
                                                using (brush2 = new SolidBrush(link2.LinkColor))
                                                {
                                                    linkFont = link2.LinkFont;
                                                    ef = DrawUtil.MeasureString(graphics, strArray[num11], linkFont);
                                                    if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                    {
                                                        num7 = 0f;
                                                        num8 += num6;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                {
                                                    num7 = 0f;
                                                    num8 += num6;
                                                }
                                            }
                                            num10 += strArray[num11].Length;
                                            num7 += ef.Width;
                                            if ((link2 != null) && (num10 >= link2.EndIndex))
                                            {
                                                link2 = null;
                                            }
                                            num11++;
                                        }
                                    }
                                    e.RowHeight = Convert.ToInt32(Math.Round((double) num8, MidpointRounding.AwayFromZero));
                                }
                                else
                                {
                                    num6 = textFont.Height + 3;
                                    num7 = 0f;
                                    num8 = num6;
                                    num9 = this.colText.VisibleWidth - 10;
                                    num10 = 0;
                                    strArray = DrawUtil.SplitString(str, " ");
                                    for (num11 = 0; num11 < strArray.Length; num11++)
                                    {
                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                        if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                        {
                                            num7 = 0f;
                                            num8 += num6;
                                        }
                                        num10 += strArray[num11].Length;
                                        num7 += ef.Width;
                                    }
                                    e.RowHeight = Convert.ToInt32(Math.Round((double) num8, MidpointRounding.AwayFromZero));
                                }
                            }
                        }
                    }
                    this.ChatRowHeights[e.RowHandle] = e.RowHeight;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvChat_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (this.mFirstChatDraw)
                {
                    this.mFirstChatDraw = false;
                    Program.Settings.StylePreferences.StyleControl(this.gvChat, EventArgs.Empty);
                }
                if ((e.Column.Name != this.colPlayer.Name) && (e.Column.Name != this.colText.Name))
                {
                    e.Handled = false;
                }
                else
                {
                    this.ChatRowPoints[e.RowHandle] = e.Bounds;
                    if (e.Column.AbsoluteIndex > 0)
                    {
                        ChatLine row = this.gvChat.GetRow(e.RowHandle) as ChatLine;
                        if (row != null)
                        {
                            IText text = null;
                            if ((e.Column.AbsoluteIndex == 1) && (row.PlayerSegments.Count > 0))
                            {
                                text = row.PlayerSegments[0];
                            }
                            else if ((e.Column.AbsoluteIndex == 2) && (row.TextSegments.Count > 0))
                            {
                                text = row.TextSegments[0];
                            }
                            if (text != null)
                            {
                                int num;
                                int index;
                                float num6;
                                float x;
                                float y;
                                int num9;
                                int num10;
                                ChatLink link2;
                                SolidBrush brush;
                                string[] strArray;
                                int num11;
                                SizeF ef;
                                Brush brush2;
                                Font linkFont;
                                Font textFont = text.TextFont;
                                Rectangle bounds = e.Bounds;
                                ChatLink[] linkArray = null;
                                Dictionary<int, ChatLink> dictionary = null;
                                SortedList<int, Emote> list = null;
                                List<string> list2 = null;
                                List<MultiVal<int, int>> list3 = null;
                                string str = text.Text;
                                string str2 = null;
                                if (e.Column.AbsoluteIndex == 2)
                                {
                                    if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                    {
                                        linkArray = ChatLink.FindLinks(text.Text, ChatLink.Emote);
                                        for (num = 0; num < linkArray.Length; num++)
                                        {
                                            if (Emote.AllEmotes.ContainsKey(linkArray[num].DisplayText))
                                            {
                                                text.Text = text.Text.Replace(linkArray[num].FullUrl, linkArray[num].DisplayText);
                                                str = str.Replace(linkArray[num].FullUrl, linkArray[num].DisplayText);
                                                row.ContainsEmotes = true;
                                            }
                                        }
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                        row.ContainsLinks = new bool?((dictionary != null) && (dictionary.Count > 0));
                                    }
                                    if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                                    {
                                        list2 = new List<string>();
                                        list3 = new List<MultiVal<int, int>>();
                                        list = new SortedList<int, Emote>();
                                        SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                        foreach (Emote emote in Emote.AllEmotes.Values)
                                        {
                                            list4.Add(emote.CharSequence.Length, emote);
                                        }
                                        foreach (Emote emote in list4.Values)
                                        {
                                            index = str.IndexOf(emote.CharSequence);
                                            while (index >= 0)
                                            {
                                                bool flag = false;
                                                if (dictionary != null)
                                                {
                                                    foreach (ChatLink link in dictionary.Values)
                                                    {
                                                        if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                        {
                                                            flag = true;
                                                        }
                                                    }
                                                }
                                                bool flag2 = false;
                                                foreach (KeyValuePair<int, Emote> pair in list)
                                                {
                                                    if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                                    {
                                                        flag2 = true;
                                                        break;
                                                    }
                                                }
                                                if (!(flag || flag2))
                                                {
                                                    list3.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                                    list.Add(index, emote);
                                                }
                                                index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                                if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                                {
                                                    if (str2 == null)
                                                    {
                                                        str2 = str;
                                                    }
                                                    str2 = str2.Replace(emote.CharSequence, "");
                                                }
                                            }
                                        }
                                        row.ContainsEmotes = new bool?((list != null) && (list.Count > 0));
                                    }
                                }
                                if ((list != null) && (list.Count > 0))
                                {
                                    list3.Add(new MultiVal<int, int>(str.Length, 0));
                                    list.Add(str.Length, null);
                                    SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list3.Count);
                                    list5[-1] = new MultiVal<int, int>(0, 0);
                                    foreach (MultiVal<int, int> val in list3)
                                    {
                                        list5[val.Value1] = val;
                                    }
                                    for (num = 1; num < list5.Count; num++)
                                    {
                                        int num3 = list5.Values[num - 1].Value1;
                                        index = list5.Values[num].Value1;
                                        int num4 = list5.Values[num - 1].Value2;
                                        int num5 = list5.Values[num].Value2;
                                        list2.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                    }
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                    num6 = textFont.Height + 3;
                                    x = bounds.X;
                                    y = bounds.Y;
                                    num9 = (this.colText.VisibleWidth - 10) + bounds.X;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        for (num = 0; num < list2.Count; num++)
                                        {
                                            if (list2[num].Length > 0)
                                            {
                                                strArray = DrawUtil.SplitString(list2[num], " ");
                                                for (num11 = 0; num11 < strArray.Length; num11++)
                                                {
                                                    if (link2 == null)
                                                    {
                                                        if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                        {
                                                            link2 = dictionary[num10];
                                                        }
                                                        if (link2 != null)
                                                        {
                                                            using (brush2 = new SolidBrush(link2.LinkColor))
                                                            {
                                                                linkFont = link2.LinkFont;
                                                                ef = DrawUtil.MeasureString(e.Graphics, link2.DisplayText + " ", linkFont);
                                                                if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                                {
                                                                    x = bounds.X;
                                                                    y += num6;
                                                                }
                                                                e.Graphics.DrawString(link2.DisplayText, linkFont, brush2, x, y);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ef = DrawUtil.MeasureString(e.Graphics, strArray[num11], textFont);
                                                            if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                            {
                                                                x = bounds.X;
                                                                y += num6;
                                                            }
                                                            e.Graphics.DrawString(strArray[num11], textFont, brush, x, y);
                                                        }
                                                        num10 += strArray[num11].Length;
                                                        x += ef.Width;
                                                    }
                                                    else
                                                    {
                                                        num10 += strArray[num11].Length;
                                                    }
                                                    if ((link2 != null) && (num10 >= link2.EndIndex))
                                                    {
                                                        link2 = null;
                                                    }
                                                }
                                            }
                                            if (list.Values[num] != null)
                                            {
                                                if (list.Values[num].Image.Height == 1)
                                                {
                                                }
                                                float num12 = 1f;
                                                if (num12 < 1f)
                                                {
                                                    num12 = 1f;
                                                }
                                                float width = list.Values[num].Image.Width * num12;
                                                float height = list.Values[num].Image.Height * num12;
                                                if ((x + width) > num9)
                                                {
                                                    x = bounds.X;
                                                    y += num6;
                                                }
                                                if (this.CountEmotes && list.Values[num].CanAnimate)
                                                {
                                                    this.EmoteCount++;
                                                }
                                                e.Graphics.DrawImage(list.Values[num].Image, x, y, width, height);
                                                x += width;
                                            }
                                        }
                                    }
                                }
                                else if ((dictionary != null) && (dictionary.Count > 0))
                                {
                                    num6 = textFont.Height + 3;
                                    x = bounds.X;
                                    y = bounds.Y;
                                    num9 = (this.colText.VisibleWidth - 10) + bounds.X;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        strArray = DrawUtil.SplitString(str, " ");
                                        for (num11 = 0; num11 < strArray.Length; num11++)
                                        {
                                            if (link2 == null)
                                            {
                                                if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                {
                                                    link2 = dictionary[num10];
                                                }
                                                if (link2 != null)
                                                {
                                                    using (brush2 = new SolidBrush(link2.LinkColor))
                                                    {
                                                        linkFont = link2.LinkFont;
                                                        ef = DrawUtil.MeasureString(e.Graphics, link2.DisplayText + " ", linkFont);
                                                        if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                        {
                                                            x = bounds.X;
                                                            y += num6;
                                                        }
                                                        e.Graphics.DrawString(link2.DisplayText, linkFont, brush2, x, y);
                                                    }
                                                }
                                                else
                                                {
                                                    ef = DrawUtil.MeasureString(e.Graphics, strArray[num11], textFont);
                                                    if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                    {
                                                        x = bounds.X;
                                                        y += num6;
                                                    }
                                                    e.Graphics.DrawString(strArray[num11], textFont, brush, x, y);
                                                }
                                                num10 += strArray[num11].Length;
                                                x += ef.Width;
                                            }
                                            else
                                            {
                                                num10 += strArray[num11].Length;
                                            }
                                            if ((link2 != null) && (num10 >= link2.EndIndex))
                                            {
                                                link2 = null;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        num6 = textFont.Height + 3;
                                        x = bounds.Left;
                                        y = bounds.Top;
                                        num9 = (this.colText.VisibleWidth - 10) + bounds.X;
                                        num10 = 0;
                                        strArray = DrawUtil.SplitString(str, " ");
                                        for (num11 = 0; num11 < strArray.Length; num11++)
                                        {
                                            ef = DrawUtil.MeasureString(e.Graphics, strArray[num11], textFont);
                                            if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                            {
                                                x = bounds.Left;
                                                y += num6;
                                            }
                                            e.Graphics.DrawString(strArray[num11], textFont, brush, x, y);
                                            num10 += strArray[num11].Length;
                                            x += ef.Width;
                                        }
                                    }
                                }
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvChat_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && (sender is GridView))
            {
                this.mSelectedParticipantView = sender as GridView;
                this.mSelectedParticipantView.Focus();
                GridHitInfo info = this.mSelectedParticipantView.CalcHitInfo(e.Location);
                if (this.mSelectedParticipantView.OptionsSelection.MultiSelect && (this.mSelectedParticipantView.OptionsSelection.MultiSelectMode == GridMultiSelectMode.RowSelect))
                {
                    int[] selectedRows = this.mSelectedParticipantView.GetSelectedRows();
                    for (int i = 0; i < selectedRows.Length; i++)
                    {
                        this.mSelectedParticipantView.UnselectRow(selectedRows[i]);
                    }
                }
                if (info.InRow)
                {
                    (sender as GridView).SelectRow(info.RowHandle);
                }
            }
        }

        private void gvChat_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo info = (sender as GridView).CalcHitInfo(e.X, e.Y);
                if (info.InRow && this.ChatRowPoints.ContainsKey(info.RowHandle))
                {
                    Rectangle rectangle = this.ChatRowPoints[info.RowHandle];
                    if (rectangle.Contains(e.X, e.Y))
                    {
                        ChatLine row = (sender as GridView).GetRow(info.RowHandle) as ChatLine;
                        if (row != null)
                        {
                            IText text = row.TextSegments[0];
                            if (text != null)
                            {
                                int index;
                                Font textFont = text.TextFont;
                                string str = text.Text;
                                string str2 = null;
                                Dictionary<int, ChatLink> dictionary = null;
                                List<string> list = null;
                                List<MultiVal<int, int>> list2 = null;
                                SortedList<int, Emote> list3 = null;
                                if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                {
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                }
                                if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                                {
                                    list = new List<string>();
                                    list2 = new List<MultiVal<int, int>>();
                                    list3 = new SortedList<int, Emote>();
                                    SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                    foreach (Emote emote in Emote.AllEmotes.Values)
                                    {
                                        list4.Add(emote.CharSequence.Length, emote);
                                    }
                                    foreach (Emote emote in list4.Values)
                                    {
                                        index = str.IndexOf(emote.CharSequence);
                                        while (index >= 0)
                                        {
                                            bool flag = false;
                                            if (dictionary != null)
                                            {
                                                foreach (ChatLink link in dictionary.Values)
                                                {
                                                    if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                    {
                                                        flag = true;
                                                    }
                                                }
                                            }
                                            bool flag2 = false;
                                            foreach (KeyValuePair<int, Emote> pair in list3)
                                            {
                                                if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                                {
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                            if (!(flag || flag2))
                                            {
                                                list2.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                                list3.Add(index, emote);
                                            }
                                            index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                            if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                            {
                                                if (str2 == null)
                                                {
                                                    str2 = str;
                                                }
                                                str2 = str2.Replace(emote.CharSequence, "");
                                            }
                                        }
                                    }
                                }
                                using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                                {
                                    float num6;
                                    float x;
                                    float y;
                                    int num9;
                                    int num10;
                                    ChatLink link2;
                                    SolidBrush brush;
                                    string[] strArray;
                                    int num11;
                                    SizeF ef;
                                    Brush brush2;
                                    Font linkFont;
                                    RectangleF ef2;
                                    if ((list3 != null) && (list3.Count > 0))
                                    {
                                        int num2;
                                        list2.Add(new MultiVal<int, int>(str.Length, 0));
                                        list3.Add(str.Length, null);
                                        SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                        list5[-1] = new MultiVal<int, int>(0, 0);
                                        foreach (MultiVal<int, int> val in list2)
                                        {
                                            list5[val.Value1] = val;
                                        }
                                        for (num2 = 1; num2 < list5.Count; num2++)
                                        {
                                            int num3 = list5.Values[num2 - 1].Value1;
                                            index = list5.Values[num2].Value1;
                                            int num4 = list5.Values[num2 - 1].Value2;
                                            int num5 = list5.Values[num2].Value2;
                                            list.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                        }
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            for (num2 = 0; num2 < list.Count; num2++)
                                            {
                                                if (list[num2].Length > 0)
                                                {
                                                    strArray = DrawUtil.SplitString(list[num2], " ");
                                                    for (num11 = 0; num11 < strArray.Length; num11++)
                                                    {
                                                        if (link2 == null)
                                                        {
                                                            if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                            {
                                                                link2 = dictionary[num10];
                                                            }
                                                            if (link2 != null)
                                                            {
                                                                using (brush2 = new SolidBrush(link2.LinkColor))
                                                                {
                                                                    linkFont = link2.LinkFont;
                                                                    ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                                    if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                    {
                                                                        x = rectangle.X;
                                                                        y += num6;
                                                                    }
                                                                    ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                                    if (ef2.Contains((float) e.X, (float) e.Y))
                                                                    {
                                                                        this.Cursor = Cursors.Hand;
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                                if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                {
                                                                    x = rectangle.X;
                                                                    y += num6;
                                                                }
                                                            }
                                                            num10 += strArray[num11].Length;
                                                            x += ef.Width;
                                                        }
                                                        else
                                                        {
                                                            num10 += strArray[num11].Length;
                                                        }
                                                        if ((link2 != null) && (num10 >= link2.EndIndex))
                                                        {
                                                            link2 = null;
                                                        }
                                                    }
                                                }
                                                if (list3.Values[num2] != null)
                                                {
                                                    float num12 = 1f;
                                                    if (num12 < 1f)
                                                    {
                                                        num12 = 1f;
                                                    }
                                                    float num13 = list3.Values[num2].Image.Width * num12;
                                                    if ((x + num13) > num9)
                                                    {
                                                        x = rectangle.X;
                                                        y += num6;
                                                    }
                                                    x += num13;
                                                }
                                            }
                                        }
                                    }
                                    else if ((dictionary != null) && (dictionary.Count > 0))
                                    {
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            strArray = DrawUtil.SplitString(str, " ");
                                            for (num11 = 0; num11 < strArray.Length; num11++)
                                            {
                                                if (link2 == null)
                                                {
                                                    if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                    {
                                                        link2 = dictionary[num10];
                                                    }
                                                    if (link2 != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link2.LinkColor))
                                                        {
                                                            linkFont = link2.LinkFont;
                                                            ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                            if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                            {
                                                                x = rectangle.X;
                                                                y += num6;
                                                            }
                                                            ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                            if (ef2.Contains((float) e.X, (float) e.Y))
                                                            {
                                                                this.Cursor = Cursors.Hand;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                        if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                        {
                                                            x = rectangle.X;
                                                            y += num6;
                                                        }
                                                    }
                                                    num10 += strArray[num11].Length;
                                                    x += ef.Width;
                                                }
                                                else
                                                {
                                                    num10 += strArray[num11].Length;
                                                }
                                                if ((link2 != null) && (num10 >= link2.EndIndex))
                                                {
                                                    link2 = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void gvChat_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                int num2;
                if (this.ChatRowPoints.ContainsKey((sender as GridView).FocusedRowHandle))
                {
                    Rectangle rectangle = this.ChatRowPoints[(sender as GridView).FocusedRowHandle];
                    if (rectangle.Contains(e.X, e.Y))
                    {
                        ChatLine row = (sender as GridView).GetRow((sender as GridView).FocusedRowHandle) as ChatLine;
                        if (row != null)
                        {
                            IText text = row.TextSegments[0];
                            if (text != null)
                            {
                                int index;
                                Font textFont = text.TextFont;
                                string str = text.Text;
                                string str2 = null;
                                Dictionary<int, ChatLink> dictionary = null;
                                List<string> list = null;
                                List<MultiVal<int, int>> list2 = null;
                                SortedList<int, Emote> list3 = null;
                                if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                {
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                }
                                if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                                {
                                    list = new List<string>();
                                    list2 = new List<MultiVal<int, int>>();
                                    list3 = new SortedList<int, Emote>();
                                    SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                    foreach (Emote emote in Emote.AllEmotes.Values)
                                    {
                                        list4.Add(emote.CharSequence.Length, emote);
                                    }
                                    foreach (Emote emote in list4.Values)
                                    {
                                        index = str.IndexOf(emote.CharSequence);
                                        while (index >= 0)
                                        {
                                            bool flag = false;
                                            if (dictionary != null)
                                            {
                                                foreach (ChatLink link in dictionary.Values)
                                                {
                                                    if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                    {
                                                        flag = true;
                                                    }
                                                }
                                            }
                                            bool flag2 = false;
                                            foreach (KeyValuePair<int, Emote> pair in list3)
                                            {
                                                if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                                {
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                            if (!(flag || flag2))
                                            {
                                                list2.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                                list3.Add(index, emote);
                                            }
                                            index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                            if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                            {
                                                if (str2 == null)
                                                {
                                                    str2 = str;
                                                }
                                                str2 = str2.Replace(emote.CharSequence, "");
                                            }
                                        }
                                    }
                                }
                                using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                                {
                                    float num6;
                                    float x;
                                    float y;
                                    int num9;
                                    int num10;
                                    ChatLink link2;
                                    SolidBrush brush;
                                    string[] strArray;
                                    int num11;
                                    SizeF ef;
                                    Brush brush2;
                                    Font linkFont;
                                    RectangleF ef2;
                                    if ((list3 != null) && (list3.Count > 0))
                                    {
                                        list2.Add(new MultiVal<int, int>(str.Length, 0));
                                        list3.Add(str.Length, null);
                                        SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                        list5[-1] = new MultiVal<int, int>(0, 0);
                                        foreach (MultiVal<int, int> val in list2)
                                        {
                                            list5[val.Value1] = val;
                                        }
                                        for (num2 = 1; num2 < list5.Count; num2++)
                                        {
                                            int num3 = list5.Values[num2 - 1].Value1;
                                            index = list5.Values[num2].Value1;
                                            int num4 = list5.Values[num2 - 1].Value2;
                                            int num5 = list5.Values[num2].Value2;
                                            list.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                        }
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            num2 = 0;
                                            while (num2 < list.Count)
                                            {
                                                if (list[num2].Length > 0)
                                                {
                                                    strArray = DrawUtil.SplitString(list[num2], " ");
                                                    for (num11 = 0; num11 < strArray.Length; num11++)
                                                    {
                                                        if (link2 == null)
                                                        {
                                                            if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                            {
                                                                link2 = dictionary[num10];
                                                            }
                                                            if (link2 != null)
                                                            {
                                                                using (brush2 = new SolidBrush(link2.LinkColor))
                                                                {
                                                                    linkFont = link2.LinkFont;
                                                                    ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                                    if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                    {
                                                                        x = rectangle.X;
                                                                        y += num6;
                                                                    }
                                                                    ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                                    if (ef2.Contains((float) e.X, (float) e.Y))
                                                                    {
                                                                        link2.OnClick();
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                                if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                {
                                                                    x = rectangle.X;
                                                                    y += num6;
                                                                }
                                                            }
                                                            num10 += strArray[num11].Length;
                                                            x += ef.Width;
                                                        }
                                                        else
                                                        {
                                                            num10 += strArray[num11].Length;
                                                        }
                                                        if ((link2 != null) && (num10 >= link2.EndIndex))
                                                        {
                                                            link2 = null;
                                                        }
                                                    }
                                                }
                                                if (list3.Values[num2] != null)
                                                {
                                                    float num12 = 1f;
                                                    float num13 = list3.Values[num2].Image.Width * num12;
                                                    if ((x + num13) > num9)
                                                    {
                                                        x = rectangle.X;
                                                        y += num6;
                                                    }
                                                    if ((e.Button == MouseButtons.Right) && ConfigSettings.GetBool("Emotes", true))
                                                    {
                                                        Rectangle rectangle2 = new Rectangle((int) x, (int) y, list3.Values[num2].Image.Width, list3.Values[num2].Image.Height);
                                                        if (rectangle2.Contains(e.Location))
                                                        {
                                                            this.ciEmote_Animate.Checked = Program.Settings.Chat.Emotes.AnimateEmotes;
                                                            this.ciEmote_Share.Checked = Program.Settings.Chat.Emotes.AutoShareEmotes;
                                                            this.SelectedEmote = list3.Values[num2];
                                                            this.gpgContextMenuEmote.Show(this.gpgChatGrid, e.Location);
                                                            return;
                                                        }
                                                    }
                                                    x += num13;
                                                }
                                                num2++;
                                            }
                                        }
                                    }
                                    else if ((dictionary != null) && (dictionary.Count > 0))
                                    {
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            strArray = DrawUtil.SplitString(str, " ");
                                            for (num11 = 0; num11 < strArray.Length; num11++)
                                            {
                                                if (link2 == null)
                                                {
                                                    if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                    {
                                                        link2 = dictionary[num10];
                                                    }
                                                    if (link2 != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link2.LinkColor))
                                                        {
                                                            linkFont = link2.LinkFont;
                                                            ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                            if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                            {
                                                                x = rectangle.X;
                                                                y += num6;
                                                            }
                                                            ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                            if (ef2.Contains((float) e.X, (float) e.Y))
                                                            {
                                                                if (e.Button == MouseButtons.Left)
                                                                {
                                                                    link2.OnClick();
                                                                    goto Label_0C82;
                                                                }
                                                                if (e.Button == MouseButtons.Right)
                                                                {
                                                                    goto Label_0C82;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                        if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                        {
                                                            x = rectangle.X;
                                                            y += num6;
                                                        }
                                                    }
                                                    num10 += strArray[num11].Length;
                                                    x += ef.Width;
                                                }
                                                else
                                                {
                                                    num10 += strArray[num11].Length;
                                                }
                                                if ((link2 != null) && (num10 >= link2.EndIndex))
                                                {
                                                    link2 = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            Label_0C82:
                if (e.Button == MouseButtons.Right)
                {
                    if ((this.SelectedChatTextSender == null) || this.SelectedChatTextSender.IsSystem)
                    {
                        this.ciChatText_ShowEmotes.Visible = !Program.Settings.Chat.Emotes.ShowEmotes;
                        this.ciChatText_ViewPlayer.Visible = false;
                        this.ciChatText_ViewRank.Visible = false;
                        this.ciChatText_PrivateMsg.Visible = false;
                        this.ciChatText_ClanInvite.Visible = false;
                        this.ciChatText_ClanRequest.Visible = false;
                        this.ciChatText_ViewClan.Visible = false;
                        this.ciChatText_Ignore.Visible = false;
                        this.ciChatText_Unignore.Visible = false;
                        this.ciChatText_WebStats.Visible = false;
                        this.ciChatText_Replays.Visible = false;
                        this.ciChatText_FriendInvite.Visible = false;
                        this.ciChatText_FriendRemove.Visible = false;
                        this.ciChatText_Promote.Visible = false;
                        this.ciChatText_Demote.Visible = false;
                        this.ciChatText_ClanRemove.Visible = false;
                        this.ciChatText_LeaveClan.Visible = false;
                        this.ciChatText_Ban.Visible = false;
                        this.ciChatText_Kick.Visible = false;
                        this.ciChatText_Solution.Visible = false;
                    }
                    else
                    {
                        User selectedChatTextSender = this.SelectedChatTextSender;
                        this.ciChatText_ShowEmotes.Visible = !Program.Settings.Chat.Emotes.ShowEmotes;
                        this.ciChatText_ViewPlayer.Visible = true;
                        this.ciChatText_ViewRank.Visible = false;
                        this.ciChatText_PrivateMsg.Visible = !selectedChatTextSender.IsCurrent && selectedChatTextSender.Online;
                        this.ciChatText_ClanInvite.Visible = (User.Current.IsInClan && !selectedChatTextSender.IsInClan) && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_ClanRequest.Visible = (!User.Current.IsInClan && selectedChatTextSender.IsInClan) && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_ViewClan.Visible = selectedChatTextSender.IsInClan;
                        this.ciChatText_Ignore.Visible = !selectedChatTextSender.IsCurrent && !selectedChatTextSender.IsIgnored;
                        this.ciChatText_Unignore.Visible = !selectedChatTextSender.IsCurrent && selectedChatTextSender.IsIgnored;
                        this.ciChatText_WebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
                        this.ciChatText_Replays.Visible = true;
                        this.ciChatText_FriendInvite.Visible = !selectedChatTextSender.IsFriend && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_FriendRemove.Visible = selectedChatTextSender.IsFriend && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_Promote.Visible = selectedChatTextSender.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Promote, selectedChatTextSender.Name);
                        this.ciChatText_Demote.Visible = selectedChatTextSender.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Demote, selectedChatTextSender.Name);
                        this.ciChatText_ClanRemove.Visible = selectedChatTextSender.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Remove, selectedChatTextSender.Name);
                        this.ciChatText_LeaveClan.Visible = User.Current.Equals(selectedChatTextSender) && User.Current.IsInClan;
                        if ((Chatroom.Current != null) && Chatroom.Current.IsClanRoom)
                        {
                            this.ciChatText_Ban.Visible = ((User.Current.IsAdmin || ClanMember.Current.CanTargetAbility(ClanAbility.Ban, selectedChatTextSender.Name)) && !selectedChatTextSender.IsCurrent) && !selectedChatTextSender.IsAdmin;
                            this.ciChatText_Kick.Visible = ((User.Current.IsAdmin || ClanMember.Current.CanTargetAbility(ClanAbility.Kick, selectedChatTextSender.Name)) && !selectedChatTextSender.IsCurrent) && !selectedChatTextSender.IsAdmin;
                        }
                        else
                        {
                            this.ciChatText_Ban.Visible = (((Chatroom.InChatroom && Chatroom.GatheringParticipants.ContainsIndex("name", selectedChatTextSender.Name)) && (User.Current.IsAdmin || (!Chatroom.Current.IsPersistent && User.Current.IsChannelOperator))) && !selectedChatTextSender.IsCurrent) && !selectedChatTextSender.IsAdmin;
                            this.ciChatText_Kick.Visible = this.ciChatText_Ban.Visible;
                        }
                        this.ciChatText_Solution.Visible = true;
                    }
                    int num14 = -1;
                    for (num2 = 0; num2 < this.gpgContextMenuChatText.MenuItems.Count; num2++)
                    {
                        if (this.gpgContextMenuChatText.MenuItems[num2].Text == "-")
                        {
                            if ((num14 < 0) || (this.gpgContextMenuChatText.MenuItems[num14].Text == "-"))
                            {
                                this.gpgContextMenuChatText.MenuItems[num2].Visible = false;
                            }
                            else if (num2 == (this.gpgContextMenuChatText.MenuItems.Count - 1))
                            {
                                this.gpgContextMenuChatText.MenuItems[num2].Visible = false;
                            }
                            else
                            {
                                this.gpgContextMenuChatText.MenuItems[num2].Visible = true;
                            }
                        }
                        if (this.gpgContextMenuChatText.MenuItems[num2].Visible)
                        {
                            num14 = num2;
                        }
                    }
                    if ((num14 >= 0) && (this.gpgContextMenuChatText.MenuItems[num14].Text == "-"))
                    {
                        this.gpgContextMenuChatText.MenuItems[num14].Visible = false;
                    }
                    this.gpgContextMenuChatText.Show(this.gvChat.GridControl, e.Location);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvChat_RowCountChanged(object sender, EventArgs e)
        {
        }

        private void gvChat_TopRowChanged(object sender, EventArgs e)
        {
        }

        private void gvClan_Member_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if ((e.Button == MouseButtons.Right) && ((sender != null) && (sender is GridView)))
                {
                    TextLine row = (sender as GridView).GetRow((sender as GridView).FocusedRowHandle) as TextLine;
                    if ((row != null) && ((row.Tag != null) && (row.Tag is IUser)))
                    {
                        IUser tag = row.Tag as IUser;
                        if ((tag != null) && (this.gpgContextMenuChat != null))
                        {
                            this.ciChat_WhisperPlayer.Visible = !User.Current.Equals(tag) && tag.Online;
                            this.ciChat_InviteToClan.Visible = false;
                            this.ciChat_RequestClanInvite.Visible = false;
                            this.ciChat_IgnorePlayer.Visible = !tag.IsCurrent && !tag.IsIgnored;
                            this.ciChat_UnignorePlayer.Visible = !tag.IsCurrent && tag.IsIgnored;
                            this.ciChat_WebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
                            this.ciChat_InviteFriend.Visible = !User.CurrentFriends.ContainsIndex("name", tag.Name) && (tag != User.Current);
                            this.ciChat_RemoveFriend.Visible = User.CurrentFriends.ContainsIndex("name", tag.Name) && (tag != User.Current);
                            this.ciChat_ViewClan.Visible = true;
                            this.ciChat_PromoteClan.Visible = ClanMember.Current.CanTargetAbility(ClanAbility.Promote, tag.Name);
                            this.ciChat_DemoteClan.Visible = ClanMember.Current.CanTargetAbility(ClanAbility.Demote, tag.Name);
                            this.ciChat_RemoveClan.Visible = ClanMember.Current.CanTargetAbility(ClanAbility.Remove, tag.Name);
                            this.ciChat_LeaveClan.Visible = User.Current.Equals(tag) && User.Current.IsInClan;
                            this.ciChat_Ban.Visible = false;
                            this.ciChat_Kick.Visible = false;
                            int num = -1;
                            for (int i = 0; i < this.gpgContextMenuChat.MenuItems.Count; i++)
                            {
                                if (this.gpgContextMenuChat.MenuItems[i].Text == "-")
                                {
                                    if (this.gpgContextMenuChat.MenuItems[num].Text == "-")
                                    {
                                        this.gpgContextMenuChat.MenuItems[i].Visible = false;
                                    }
                                    else if (i == (this.gpgContextMenuChat.MenuItems.Count - 1))
                                    {
                                        this.gpgContextMenuChat.MenuItems[i].Visible = false;
                                    }
                                    else
                                    {
                                        this.gpgContextMenuChat.MenuItems[i].Visible = true;
                                    }
                                }
                                if (this.gpgContextMenuChat.MenuItems[i].Visible)
                                {
                                    num = i;
                                }
                            }
                            if ((num >= 0) && (this.gpgContextMenuChat.MenuItems[num].Text == "-"))
                            {
                                this.gpgContextMenuChat.MenuItems[num].Visible = false;
                            }
                            this.gpgContextMenuChat.Show((sender as GridView).GridControl, e.Location);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvParticipantMembers_RowCountChanged(object sender, EventArgs e)
        {
            this.CalcGridHeight();
            try
            {
                (sender as GridView).RefreshData();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvParticipants_Container_MasterRowGetLevelDefaultView(object sender, MasterRowGetLevelDefaultViewEventArgs e)
        {
        }

        private void gvParticipants_Container_RowCountChanged(object sender, EventArgs e)
        {
            this.CalcGridHeight();
        }

        private bool HitTest(int x, int y, int left, int top, int right, int bottom)
        {
            return ((((x >= left) && (x <= right)) && (y >= top)) && (y <= bottom));
        }

        public void HostGame()
        {
            this.HostGame("", "");
        }

        public void HostGame(string gamename)
        {
            this.HostGame(gamename, "");
        }

        public void HostGame(string gamename, string password)
        {
            Game.UpdateIP();
            if (!this.mConnected)
            {
                this.SystemMessage(Loc.Get("<LOC>You cannot host games while disconnected from GPGnet.  Please relog in."), new object[0]);
            }
            else
            {
                string gameName = gamename;
                string gamePassword = password;
                if (gameName == "")
                {
                    GPG.Multiplayer.Client.DlgHostGame game = new GPG.Multiplayer.Client.DlgHostGame(this);
                    if (game.ShowDialog() == DialogResult.OK)
                    {
                        gameName = game.GameName;
                        gamePassword = game.GamePassword;
                    }
                }
                if (gameName != "")
                {
                    this.BeforeGameChatroom = Chatroom.CurrentName;
                    this.mGameName = gameName;
                    this.mPassword = gamePassword;
                    ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Leave", this, "OnHostGame", new object[0]);
                    int num = 0;
                    while (Chatroom.InChatroom)
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        num++;
                        if (num > 300)
                        {
                            break;
                        }
                    }
                    this.SetStatusButtons(1);
                }
            }
        }

        public void HostGameSpaceSiege()
        {
            Game.UpdateIP();
            GPG.Multiplayer.Client.Games.SpaceSiege.DlgHostGame game = new GPG.Multiplayer.Client.Games.SpaceSiege.DlgHostGame();
            if (game.ShowDialog() == DialogResult.OK)
            {
                DlgGameLobby lobby = DlgGameLobby.HostGame();
                this.CurrentGameLobby = lobby;
                lobby.Show();
            }
        }

        internal void IgnorePlayer(string name)
        {
            try
            {
                if (name.ToLower() == User.Current.Name.ToLower())
                {
                    this.SystemMessage("<LOC>You cannot ignore yourself.", new object[0]);
                }
                else
                {
                    User user;
                    if (this.TryFindUser(name, out user))
                    {
                        if (!user.IsIgnored)
                        {
                            if (DataAccess.ExecuteQuery("IgnorePlayer", new object[] { name }))
                            {
                                User.IgnoredPlayers.Add(user.ID);
                                this.SystemMessage("<LOC>You are now ignoring {0}.", new object[] { name });
                                this.RefreshChatParticipant(name);
                                foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                                {
                                    if ((!chat.Disposing && !chat.IsDisposed) && chat.ChatTarget.Equals(user))
                                    {
                                        chat.Close();
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                ErrorLog.WriteLine("Error ignoring player {0}, they may already be ignored.", new object[] { name });
                            }
                        }
                    }
                    else
                    {
                        this.ErrorMessage("<LOC>Unable to find {0}.", new object[] { name });
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void InitChatContainers()
        {
            this.ChatContainerLookup = new Dictionary<string, TextContainer<StatusTextLine>>(7);
            this.ChatContainers = new BoundContainerList<StatusTextLine>();
            this.ChatContainerLookup["Admin"] = new TextContainer<StatusTextLine>(Loc.Get("<LOC>Admin"));
            this.ChatContainers.Add(this.ChatContainerLookup["Admin"]);
            this.ChatContainerLookup["Speaking"] = new TextContainer<StatusTextLine>(Loc.Get("<LOC>Speaking"));
            this.ChatContainers.Add(this.ChatContainerLookup["Speaking"]);
            this.ChatContainerLookup["Clan"] = new TextContainer<StatusTextLine>(Loc.Get("<LOC>Clan"));
            this.ChatContainers.Add(this.ChatContainerLookup["Clan"]);
            this.ChatContainerLookup["Friend"] = new TextContainer<StatusTextLine>(Loc.Get("<LOC>Friend"));
            this.ChatContainers.Add(this.ChatContainerLookup["Friend"]);
            this.ChatContainerLookup["Online"] = new TextContainer<StatusTextLine>(Loc.Get("<LOC>Online"));
            this.ChatContainers.Add(this.ChatContainerLookup["Online"]);
            this.ChatContainerLookup["Away"] = new TextContainer<StatusTextLine>(Loc.Get("<LOC>Away"));
            this.ChatContainers.Add(this.ChatContainerLookup["Away"]);
        }

        private void InitializeChat(string chatroom)
        {
            this.FrmMain_Chat(chatroom);
            this.FrmMain_Friends();
            this.FrmMain_Clan();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmMain));
            this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
            this.repositoryItemMemoEdit1 = new RepositoryItemMemoEdit();
            this.skinGatheringDisplayChat = new SkinGatheringDisplay();
            this.pnlUserListChat = new PnlUserList();
            this.gridColumnTitle = new GridColumn();
            this.gridColumnMembers = new GridColumn();
            this.btnSend = new Button();
            this.pBottom = new Panel();
            this.skinDropDownStatus = new SkinDropDown();
            this.msQuickButtons = new GPGMenuStrip(this.components);
            this.btnHome = new ToolStripMenuItem();
            this.btnChat = new ToolStripMenuItem();
            this.btnHostGame = new ToolStripMenuItem();
            this.btnJoinGame = new ToolStripMenuItem();
            this.btnRankedGame = new ToolStripMenuItem();
            this.btnArrangedTeam = new ToolStripMenuItem();
            this.btnPlayNow = new ToolStripMenuItem();
            this.btnViewRankings = new ToolStripMenuItem();
            this.btnVault = new ToolStripMenuItem();
            this.btnWorldMap = new ToolStripMenuItem();
            this.btnReplayVault = new ToolStripMenuItem();
            this.btnFeedback = new ToolStripMenuItem();
            this.btnOptions = new ToolStripMenuItem();
            this.btnMore = new ToolStripMenuItem();
            this.pbBottomRight = new PictureBox();
            this.pbBottom = new PictureBox();
            this.pbBottomLeft = new PictureBox();
            this.pTop = new Panel();
            this.msMainMenu = new GPGMenuStrip(this.components);
            this.miMainGroup = new ToolStripMenuItem();
            this.miMain_Logout = new ToolStripMenuItem();
            this.miMain_Exit = new ToolStripMenuItem();
            this.miChangeEmail = new ToolStripMenuItem();
            this.miConsolidateAccounts = new ToolStripMenuItem();
            this.miAdmin = new ToolStripMenuItem();
            this.miCustomAdmin = new ToolStripMenuItem();
            this.miAdhocSQL = new ToolStripMenuItem();
            this.adhocChartsToolStripMenuItem = new ToolStripMenuItem();
            this.miPacketSniffer = new ToolStripMenuItem();
            this.miShowEventLog = new ToolStripMenuItem();
            this.miAdmin_CreateVolunteerEffort = new ToolStripMenuItem();
            this.miAdmin_ViewVolunteers = new ToolStripMenuItem();
            this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem = new ToolStripMenuItem();
            this.miAdmin_Security = new ToolStripMenuItem();
            this.miAdmin_Avatars = new ToolStripMenuItem();
            this.spaceSiegeLobbyToolStripMenuItem = new ToolStripMenuItem();
            this.manageServerGamesToolStripMenuItem = new ToolStripMenuItem();
            this.miGameGroup = new ToolStripMenuItem();
            this.miCustomGame = new ToolStripMenuItem();
            this.miJoinGame = new ToolStripMenuItem();
            this.miAutomatch = new ToolStripMenuItem();
            this.lOCArrangedTeamGameToolStripMenuItem = new ToolStripMenuItem();
            this.miGame_Vault = new ToolStripMenuItem();
            this.miCreateTournament = new ToolStripMenuItem();
            this.miTournamentSchedule = new ToolStripMenuItem();
            this.miGame_RedeemPrize = new ToolStripMenuItem();
            this.miRankings = new ToolStripMenuItem();
            this.miRankings_1v1 = new ToolStripMenuItem();
            this.lOC2v2RankingsToolStripMenuItem = new ToolStripMenuItem();
            this.lOC3v3RankingsToolStripMenuItem = new ToolStripMenuItem();
            this.lOC4v4RankingsToolStripMenuItem = new ToolStripMenuItem();
            this.lOCClanRankingsToolStripMenuItem = new ToolStripMenuItem();
            this.miLadders = new ToolStripMenuItem();
            this.miLadders_AcceptAll = new ToolStripMenuItem();
            this.miLadders_DeclineAll = new ToolStripMenuItem();
            this.miTools = new ToolStripMenuItem();
            this.miTools_Feedback = new ToolStripMenuItem();
            this.miTools_GameKeys = new ToolStripMenuItem();
            this.miTools_Chat = new ToolStripMenuItem();
            this.miTools_Chat_Emotes = new ToolStripMenuItem();
            this.miTools_ContentManager = new ToolStripMenuItem();
            this.miTools_Options = new ToolStripMenuItem();
            this.miTools_LocPatches = new ToolStripMenuItem();
            this.miManageGames = new ToolStripMenuItem();
            this.miHelp = new ToolStripMenuItem();
            this.miHelp_Solutions = new ToolStripMenuItem();
            this.miHelp_Volunteer = new ToolStripMenuItem();
            this.miHelp_SupComHome = new ToolStripMenuItem();
            this.miHelp_GPGHome = new ToolStripMenuItem();
            this.miForums = new ToolStripMenuItem();
            this.miHelp_ReportIssue = new ToolStripMenuItem();
            this.miHelp_About = new ToolStripMenuItem();
            this.miSpaceSiegeWeb = new ToolStripMenuItem();
            this.pbClose = new PictureBox();
            this.pbRestore = new PictureBox();
            this.pbMinimize = new PictureBox();
            this.pbTopRight = new PictureBox();
            this.pbTop = new PictureBox();
            this.pbTopLeft = new PictureBox();
            this.wbMain = new WebBrowser();
            this.splitContainerFriends = new SplitContainer();
            this.gpgLabelFriendInvites2 = new GPGLabel();
            this.gpgLabelFriendInvitesCount2 = new GPGLabel();
            this.gpgScrollPanelFriends = new GPGScrollPanel();
            this.pnlUserListFriends = new PnlUserList();
            this.gpgLabelNoFriends = new GPGLabel();
            this.splitContainerClan = new SplitContainer();
            this.gpgLabelClanRequests = new GPGLabel();
            this.gpgLabelClanRequestCount = new GPGLabel();
            this.gpgScrollPanelClan = new GPGScrollPanel();
            this.pnlUserListClan = new PnlUserList();
            this.gpgScrollPanelNoClan = new GPGScrollPanel();
            this.gpgLabelClanInvites = new GPGLabel();
            this.gpgLabelCreateClan = new GPGLabel();
            this.gpgLabelClanInviteCount = new GPGLabel();
            this.gpgLabelNoClan = new GPGLabel();
            this.skinLabelClanName = new SkinLabel();
            this.pbLeftBorder = new PictureBox();
            this.pbRightBorder = new PictureBox();
            this.rimPictureEdit = new RepositoryItemPictureEdit();
            this.rimMemoEdit = new RepositoryItemMemoEdit();
            this.rimPictureEdit2 = new RepositoryItemPictureEdit();
            this.rimMemoEdit2 = new RepositoryItemMemoEdit();
            this.gpgChatGrid = new GPGChatGrid();
            this.gvChat = new GridView();
            this.colIcon = new GridColumn();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.colPlayer = new GridColumn();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.colText = new GridColumn();
            this.gcVisible = new GridColumn();
            this.colTimeStamp = new GridColumn();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.ilIcons = new ImageList(this.components);
            this.gridColumn1 = new GridColumn();
            this.gridColumn2 = new GridColumn();
            this.gridColumn3 = new GridColumn();
            this.pcTextEntry = new PanelControl();
            this.gpgContextMenuChat = new GPGContextMenu();
            this.ciChat_WhisperPlayer = new MenuItem();
            this.ciChat_IgnorePlayer = new MenuItem();
            this.ciChat_UnignorePlayer = new MenuItem();
            this.ciChat_ViewRank = new MenuItem();
            this.ciChat_WebStats = new MenuItem();
            this.ciChat_ViewPlayer = new MenuItem();
            this.miViewReplays = new MenuItem();
            this.menuItem10 = new MenuItem();
            this.ciChat_InviteFriend = new MenuItem();
            this.ciChat_RemoveFriend = new MenuItem();
            this.menuItem8 = new MenuItem();
            this.ciChat_InviteToClan = new MenuItem();
            this.ciChat_RequestClanInvite = new MenuItem();
            this.ciChat_ViewClan = new MenuItem();
            this.ciChat_PromoteClan = new MenuItem();
            this.ciChat_DemoteClan = new MenuItem();
            this.ciChat_RemoveClan = new MenuItem();
            this.ciChat_LeaveClan = new MenuItem();
            this.menuItem3 = new MenuItem();
            this.ciChat_Kick = new MenuItem();
            this.ciChat_Ban = new MenuItem();
            this.menuItem7 = new MenuItem();
            this.ciChat_TeamInvite = new MenuItem();
            this.textBoxMsg = new GPGTextArea();
            this.pbMiddle = new SkinStatusStrip();
            this.miStatus_Online = new MenuItem();
            this.miStatus_Away = new MenuItem();
            this.miStatus_DND = new MenuItem();
            this.menuItem1 = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.gpgContextMenuChatText = new GPGContextMenu();
            this.ciChatText_Clear = new MenuItem();
            this.ciChatText_Copy = new MenuItem();
            this.ciChatText_Filter = new MenuItem();
            this.ciChatText_Filter_Self = new MenuItem();
            this.ciChatText_Filter_System = new MenuItem();
            this.ciChatText_Filter_Events = new MenuItem();
            this.ciChatText_Filter_Errors = new MenuItem();
            this.ciChatText_Filter_Game = new MenuItem();
            this.ciChatText_Filter_Friends = new MenuItem();
            this.ciChatText_Filter_Clan = new MenuItem();
            this.ciChatText_Filter_Admin = new MenuItem();
            this.ciChatText_Filter_Other = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.ciChatText_Filters_Reset = new MenuItem();
            this.miShowColumns = new MenuItem();
            this.ciChatText_ShowEmotes = new MenuItem();
            this.menuItm15 = new MenuItem();
            this.ciChatText_PrivateMsg = new MenuItem();
            this.ciChatText_Ignore = new MenuItem();
            this.ciChatText_Unignore = new MenuItem();
            this.ciChatText_ViewRank = new MenuItem();
            this.ciChatText_WebStats = new MenuItem();
            this.ciChatText_ViewPlayer = new MenuItem();
            this.ciChatText_Replays = new MenuItem();
            this.menuItem6 = new MenuItem();
            this.ciChatText_FriendInvite = new MenuItem();
            this.ciChatText_FriendRemove = new MenuItem();
            this.menuItem11 = new MenuItem();
            this.ciChatText_ClanInvite = new MenuItem();
            this.ciChatText_ClanRequest = new MenuItem();
            this.ciChatText_ClanRemove = new MenuItem();
            this.ciChatText_Promote = new MenuItem();
            this.ciChatText_Demote = new MenuItem();
            this.ciChatText_ViewClan = new MenuItem();
            this.ciChatText_LeaveClan = new MenuItem();
            this.menuItem18 = new MenuItem();
            this.ciChatText_Kick = new MenuItem();
            this.ciChatText_Ban = new MenuItem();
            this.menuItem12 = new MenuItem();
            this.ciChatText_Solution = new MenuItem();
            this.miTranslate = new MenuItem();
            this.menuItem23 = new MenuItem();
            this.menuItem24 = new MenuItem();
            this.menuItem25 = new MenuItem();
            this.menuItem26 = new MenuItem();
            this.menuItem27 = new MenuItem();
            this.menuItem28 = new MenuItem();
            this.menuItem29 = new MenuItem();
            this.dockManager = new DockManager();
            this.gpgContextMenuEmote = new GPGContextMenu();
            this.ciEmote_Manager = new MenuItem();
            this.menuItem13 = new MenuItem();
            this.ciEmote_Hide = new MenuItem();
            this.ciEmote_Share = new MenuItem();
            this.ciEmote_Animate = new MenuItem();
            this.menuItem9 = new MenuItem();
            this.ciEmote_Delete = new MenuItem();
            this.pManualTabs = new GPGPanel();
            this.btnChatTab = new SkinButton();
            this.btnFriendsTab = new SkinButton();
            this.btnClanTab = new SkinButton();
            this.ilMenuItems = new ImageList(this.components);
            this.gpgPanelChatAndInput = new GPGPanel();
            this.splitContainerChatAndInput = new SplitContainer();
            this.gpgTextListCommands = new GPGTextList();
            this.gpgPanelGathering = new GPGPanel();
            this.gpgPanelGatheringDropDown = new GPGPanel();
            this.splitContainerBody = new SplitContainer();
            this.gpgPanelClan = new GPGPanel();
            this.gpgPanelFriends = new GPGPanel();
            this.gpgPanel2 = new GPGPanel();
            this.tabChatroom = new SkinButton();
            this.repositoryItemPictureEdit1.BeginInit();
            this.repositoryItemMemoEdit1.BeginInit();
            this.pBottom.SuspendLayout();
            this.msQuickButtons.SuspendLayout();
            ((ISupportInitialize) this.pbBottomRight).BeginInit();
            ((ISupportInitialize) this.pbBottom).BeginInit();
            ((ISupportInitialize) this.pbBottomLeft).BeginInit();
            this.pTop.SuspendLayout();
            this.msMainMenu.SuspendLayout();
            ((ISupportInitialize) this.pbClose).BeginInit();
            ((ISupportInitialize) this.pbRestore).BeginInit();
            ((ISupportInitialize) this.pbMinimize).BeginInit();
            ((ISupportInitialize) this.pbTopRight).BeginInit();
            ((ISupportInitialize) this.pbTop).BeginInit();
            ((ISupportInitialize) this.pbTopLeft).BeginInit();
            this.splitContainerFriends.Panel1.SuspendLayout();
            this.splitContainerFriends.Panel2.SuspendLayout();
            this.splitContainerFriends.SuspendLayout();
            this.gpgScrollPanelFriends.SuspendLayout();
            this.splitContainerClan.Panel1.SuspendLayout();
            this.splitContainerClan.Panel2.SuspendLayout();
            this.splitContainerClan.SuspendLayout();
            this.gpgScrollPanelClan.SuspendLayout();
            this.gpgScrollPanelNoClan.SuspendLayout();
            ((ISupportInitialize) this.pbLeftBorder).BeginInit();
            ((ISupportInitialize) this.pbRightBorder).BeginInit();
            this.rimPictureEdit.BeginInit();
            this.rimMemoEdit.BeginInit();
            this.rimPictureEdit2.BeginInit();
            this.rimMemoEdit2.BeginInit();
            this.gpgChatGrid.BeginInit();
            this.gvChat.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.pcTextEntry.BeginInit();
            this.textBoxMsg.Properties.BeginInit();
            this.dockManager.BeginInit();
            this.pManualTabs.SuspendLayout();
            this.gpgPanelChatAndInput.SuspendLayout();
            this.splitContainerChatAndInput.Panel1.SuspendLayout();
            this.splitContainerChatAndInput.Panel2.SuspendLayout();
            this.splitContainerChatAndInput.SuspendLayout();
            this.gpgPanelGathering.SuspendLayout();
            this.gpgPanelGatheringDropDown.SuspendLayout();
            this.splitContainerBody.Panel1.SuspendLayout();
            this.splitContainerBody.Panel2.SuspendLayout();
            this.splitContainerBody.SuspendLayout();
            this.gpgPanelClan.SuspendLayout();
            this.gpgPanelFriends.SuspendLayout();
            this.gpgPanel2.SuspendLayout();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.PictureAlignment = ContentAlignment.TopCenter;
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.skinGatheringDisplayChat.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinGatheringDisplayChat.AutoStyle = true;
            this.skinGatheringDisplayChat.BackColor = System.Drawing.Color.Black;
            this.skinGatheringDisplayChat.ButtonState = 0;
            this.skinGatheringDisplayChat.CurrentRoom = null;
            this.skinGatheringDisplayChat.DialogResult = DialogResult.OK;
            this.skinGatheringDisplayChat.DisabledForecolor = System.Drawing.Color.Gray;
            this.skinGatheringDisplayChat.DrawColor = System.Drawing.Color.WhiteSmoke;
            this.skinGatheringDisplayChat.DrawEdges = true;
            this.skinGatheringDisplayChat.FocusColor = System.Drawing.Color.FromArgb(0x40, 0x40, 0x40);
            this.skinGatheringDisplayChat.HorizontalScalingMode = ScalingModes.Stretch;
            this.skinGatheringDisplayChat.Icon = null;
            this.skinGatheringDisplayChat.IsStyled = true;
            this.skinGatheringDisplayChat.Location = new Point(7, 0);
            this.skinGatheringDisplayChat.Name = "skinGatheringDisplayChat";
            this.skinGatheringDisplayChat.Size = new Size(0xf8, 0x19);
            this.skinGatheringDisplayChat.SkinBasePath = @"Controls\Button\ChatroomList";
            base.ttDefault.SetSuperTip(this.skinGatheringDisplayChat, null);
            this.skinGatheringDisplayChat.TabIndex = 12;
            this.skinGatheringDisplayChat.TabStop = true;
            this.skinGatheringDisplayChat.Text = "Loading...";
            this.skinGatheringDisplayChat.TextAlign = ContentAlignment.MiddleLeft;
            this.skinGatheringDisplayChat.TextPadding = new Padding(0x24, 0, 0, 0);
            this.pnlUserListChat.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlUserListChat.AutoRefresh = false;
            this.pnlUserListChat.AutoScroll = true;
            this.pnlUserListChat.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.pnlUserListChat.Location = new Point(2, 0x25);
            this.pnlUserListChat.Name = "pnlUserListChat";
            this.pnlUserListChat.Size = new Size(0x106, 420);
            this.pnlUserListChat.Style = UserListStyles.Chatroom;
            base.ttDefault.SetSuperTip(this.pnlUserListChat, null);
            this.pnlUserListChat.TabIndex = 3;
            this.gridColumnTitle.Caption = "gridColumnTitle";
            this.gridColumnTitle.FieldName = "Title";
            this.gridColumnTitle.Name = "gridColumnTitle";
            this.gridColumnTitle.Visible = true;
            this.gridColumnTitle.VisibleIndex = 0;
            this.gridColumnMembers.Caption = "gridColumnMembers";
            this.gridColumnMembers.FieldName = "Members";
            this.gridColumnMembers.Name = "gridColumnMembers";
            this.gridColumnMembers.Visible = true;
            this.gridColumnMembers.VisibleIndex = 1;
            this.btnSend.Location = new Point(0x200, 0x22d);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new Size(0x3b, 0x17);
            base.ttDefault.SetSuperTip(this.btnSend, null);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new EventHandler(this.buttonSend_Click);
            this.pBottom.Controls.Add(this.skinDropDownStatus);
            this.pBottom.Controls.Add(this.msQuickButtons);
            this.pBottom.Controls.Add(this.pbBottomRight);
            this.pBottom.Controls.Add(this.pbBottom);
            this.pBottom.Controls.Add(this.pbBottomLeft);
            this.pBottom.Dock = DockStyle.Bottom;
            this.pBottom.Location = new Point(0, 0x296);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new Size(0x3e8, 0x58);
            base.ttDefault.SetSuperTip(this.pBottom, null);
            this.pBottom.TabIndex = 5;
            this.pBottom.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.skinDropDownStatus.AutoStyle = true;
            this.skinDropDownStatus.BackColor = System.Drawing.Color.Black;
            this.skinDropDownStatus.ButtonState = 0;
            this.skinDropDownStatus.DialogResult = DialogResult.OK;
            this.skinDropDownStatus.DisabledForecolor = System.Drawing.Color.Gray;
            this.skinDropDownStatus.DrawColor = System.Drawing.Color.Black;
            this.skinDropDownStatus.DrawEdges = true;
            this.skinDropDownStatus.FocusColor = System.Drawing.Color.FromArgb(0x40, 0x40, 0x40);
            this.skinDropDownStatus.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinDropDownStatus.ForeColor = System.Drawing.Color.Black;
            this.skinDropDownStatus.HorizontalScalingMode = ScalingModes.Tile;
            this.skinDropDownStatus.Icon = null;
            this.skinDropDownStatus.IsStyled = true;
            this.skinDropDownStatus.Location = new Point(0x3d, 0x2b);
            this.skinDropDownStatus.Name = "skinDropDownStatus";
            this.skinDropDownStatus.Size = new Size(0x4e, 0x1a);
            this.skinDropDownStatus.SkinBasePath = @"Controls\Button\Dropdown";
            base.ttDefault.SetSuperTip(this.skinDropDownStatus, null);
            this.skinDropDownStatus.TabIndex = 12;
            this.skinDropDownStatus.TabStop = true;
            this.skinDropDownStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.skinDropDownStatus.TextPadding = new Padding(6, 0, 0, 0);
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.msQuickButtons.AutoSize = false;
            this.msQuickButtons.BackgroundImage = (Image) manager.GetObject("msQuickButtons.BackgroundImage");
            this.msQuickButtons.Dock = DockStyle.None;
            this.msQuickButtons.GripMargin = new Padding(0);
            this.msQuickButtons.ImageScalingSize = new Size(0x2d, 0x2d);
            this.msQuickButtons.Items.AddRange(new ToolStripItem[] { this.btnHome, this.btnChat, this.btnHostGame, this.btnJoinGame, this.btnRankedGame, this.btnArrangedTeam, this.btnPlayNow, this.btnViewRankings, this.btnVault, this.btnWorldMap, this.btnReplayVault, this.btnFeedback, this.btnOptions, this.btnMore });
            this.msQuickButtons.Location = new Point(0x90, 0x21);
            this.msQuickButtons.Name = "msQuickButtons";
            this.msQuickButtons.Padding = new Padding(0, 0, 10, 0);
            this.msQuickButtons.RenderMode = ToolStripRenderMode.Professional;
            this.msQuickButtons.ShowItemToolTips = true;
            this.msQuickButtons.Size = new Size(0x335, 0x34);
            base.ttDefault.SetSuperTip(this.msQuickButtons, null);
            this.msQuickButtons.TabIndex = 3;
            this.msQuickButtons.Paint += new PaintEventHandler(this.msQuickButtons_Paint);
            this.msQuickButtons.SizeChanged += new EventHandler(this.msQuickButtons_SizeChanged);
            this.msQuickButtons.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.msQuickButtons.ItemClicked += new ToolStripItemClickedEventHandler(this.msQuickButtons_ItemClicked);
            this.msQuickButtons.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.msQuickButtons.Click += new EventHandler(this.msQuickButtons_Click);
            this.btnHome.AutoSize = false;
            this.btnHome.AutoToolTip = true;
            this.btnHome.BackColor = System.Drawing.Color.Transparent;
            this.btnHome.Image = (Image) manager.GetObject("btnHome.Image");
            this.btnHome.ImageScaling = ToolStripItemImageScaling.None;
            this.btnHome.Name = "btnHome";
            this.btnHome.ShortcutKeys = Keys.F1;
            this.btnHome.Size = new Size(0x34, 0x34);
            this.btnHome.ToolTipText = "<LOC>Home";
            this.btnHome.Click += new EventHandler(this.btnHome_Click);
            this.btnChat.AutoSize = false;
            this.btnChat.AutoToolTip = true;
            this.btnChat.Image = (Image) manager.GetObject("btnChat.Image");
            this.btnChat.ImageScaling = ToolStripItemImageScaling.None;
            this.btnChat.Name = "btnChat";
            this.btnChat.ShortcutKeys = Keys.F2;
            this.btnChat.Size = new Size(0x34, 0x34);
            this.btnChat.ToolTipText = "<LOC>Chat";
            this.btnChat.Click += new EventHandler(this.btnChat_Click);
            this.btnHostGame.AutoSize = false;
            this.btnHostGame.AutoToolTip = true;
            this.btnHostGame.Enabled = false;
            this.btnHostGame.Image = (Image) manager.GetObject("btnHostGame.Image");
            this.btnHostGame.ImageScaling = ToolStripItemImageScaling.None;
            this.btnHostGame.Name = "btnHostGame";
            this.btnHostGame.ShortcutKeys = Keys.F3;
            this.btnHostGame.Size = new Size(0x34, 0x34);
            this.btnHostGame.ToolTipText = "<LOC>Host Custom Game";
            this.btnHostGame.Click += new EventHandler(this.btnHostGame_Click);
            this.btnJoinGame.AutoSize = false;
            this.btnJoinGame.AutoToolTip = true;
            this.btnJoinGame.Enabled = false;
            this.btnJoinGame.Image = (Image) manager.GetObject("btnJoinGame.Image");
            this.btnJoinGame.ImageScaling = ToolStripItemImageScaling.None;
            this.btnJoinGame.Name = "btnJoinGame";
            this.btnJoinGame.ShortcutKeys = Keys.F4;
            this.btnJoinGame.Size = new Size(0x34, 0x34);
            this.btnJoinGame.ToolTipText = "<LOC>Join Custom Game";
            this.btnJoinGame.Click += new EventHandler(this.btnJoinGame_Click);
            this.btnRankedGame.AutoSize = false;
            this.btnRankedGame.AutoToolTip = true;
            this.btnRankedGame.Enabled = false;
            this.btnRankedGame.Image = (Image) manager.GetObject("btnRankedGame.Image");
            this.btnRankedGame.ImageScaling = ToolStripItemImageScaling.None;
            this.btnRankedGame.Name = "btnRankedGame";
            this.btnRankedGame.ShortcutKeys = Keys.F5;
            this.btnRankedGame.Size = new Size(0x34, 0x34);
            this.btnRankedGame.ToolTipText = "<LOC>Play Ranked Game";
            this.btnRankedGame.Click += new EventHandler(this.btnRankedGame_Click);
            this.btnArrangedTeam.AutoSize = false;
            this.btnArrangedTeam.AutoToolTip = true;
            this.btnArrangedTeam.Enabled = false;
            this.btnArrangedTeam.Image = (Image) manager.GetObject("btnArrangedTeam.Image");
            this.btnArrangedTeam.ImageScaling = ToolStripItemImageScaling.None;
            this.btnArrangedTeam.Name = "btnArrangedTeam";
            this.btnArrangedTeam.ShortcutKeys = Keys.F8;
            this.btnArrangedTeam.Size = new Size(0x34, 0x34);
            this.btnArrangedTeam.ToolTipText = "<LOC>Play Arranged Team Game";
            this.btnArrangedTeam.Click += new EventHandler(this.btnArrangedTeam_Click);
            this.btnPlayNow.AutoSize = false;
            this.btnPlayNow.AutoToolTip = true;
            this.btnPlayNow.Enabled = false;
            this.btnPlayNow.Image = (Image) manager.GetObject("btnPlayNow.Image");
            this.btnPlayNow.ImageScaling = ToolStripItemImageScaling.None;
            this.btnPlayNow.Name = "btnPlayNow";
            this.btnPlayNow.ShortcutKeys = Keys.F8;
            this.btnPlayNow.Size = new Size(0x34, 0x34);
            this.btnPlayNow.ToolTipText = "<LOC>Play ranked game with your last used preferences.";
            this.btnPlayNow.Click += new EventHandler(this.btnPlayNow_Click);
            this.btnViewRankings.AutoSize = false;
            this.btnViewRankings.AutoToolTip = true;
            this.btnViewRankings.Image = (Image) manager.GetObject("btnViewRankings.Image");
            this.btnViewRankings.ImageScaling = ToolStripItemImageScaling.None;
            this.btnViewRankings.Name = "btnViewRankings";
            this.btnViewRankings.ShortcutKeys = Keys.F6;
            this.btnViewRankings.Size = new Size(0x34, 0x34);
            this.btnViewRankings.ToolTipText = "<LOC>View Rankings";
            this.btnViewRankings.Click += new EventHandler(this.btnViewRankings_Click);
            this.btnVault.AutoSize = false;
            this.btnVault.AutoToolTip = true;
            this.btnVault.Image = (Image) manager.GetObject("btnVault.Image");
            this.btnVault.ImageScaling = ToolStripItemImageScaling.None;
            this.btnVault.Name = "btnVault";
            this.btnVault.ShortcutKeys = Keys.F8;
            this.btnVault.Size = new Size(0x34, 0x34);
            this.btnVault.ToolTipText = "<LOC>The Vault";
            this.btnVault.Click += new EventHandler(this.btnVault_Click);
            this.btnWorldMap.AutoSize = false;
            this.btnWorldMap.AutoToolTip = true;
            this.btnWorldMap.Image = (Image) manager.GetObject("btnWorldMap.Image");
            this.btnWorldMap.ImageScaling = ToolStripItemImageScaling.None;
            this.btnWorldMap.Name = "btnWorldMap";
            this.btnWorldMap.ShortcutKeys = Keys.F8;
            this.btnWorldMap.Size = new Size(0x34, 0x34);
            this.btnWorldMap.ToolTipText = "<LOC>World Map";
            this.btnWorldMap.Click += new EventHandler(this.btnWorldMap_Click);
            this.btnReplayVault.AutoSize = false;
            this.btnReplayVault.AutoToolTip = true;
            this.btnReplayVault.Image = (Image) manager.GetObject("btnReplayVault.Image");
            this.btnReplayVault.ImageScaling = ToolStripItemImageScaling.None;
            this.btnReplayVault.Name = "btnReplayVault";
            this.btnReplayVault.ShortcutKeys = Keys.F8;
            this.btnReplayVault.Size = new Size(0x34, 0x34);
            this.btnReplayVault.ToolTipText = "<LOC>Replay Vault";
            this.btnReplayVault.Click += new EventHandler(this.btnReplayVault_Click);
            this.btnFeedback.AutoSize = false;
            this.btnFeedback.AutoToolTip = true;
            this.btnFeedback.Image = (Image) manager.GetObject("btnFeedback.Image");
            this.btnFeedback.ImageScaling = ToolStripItemImageScaling.None;
            this.btnFeedback.Name = "btnFeedback";
            this.btnFeedback.ShortcutKeys = Keys.F7;
            this.btnFeedback.Size = new Size(0x34, 0x34);
            this.btnFeedback.ToolTipText = "<LOC>Submit Feedback";
            this.btnFeedback.Click += new EventHandler(this.btnFeedback_Click);
            this.btnOptions.AutoSize = false;
            this.btnOptions.AutoToolTip = true;
            this.btnOptions.Image = (Image) manager.GetObject("btnOptions.Image");
            this.btnOptions.ImageScaling = ToolStripItemImageScaling.None;
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.ShortcutKeys = Keys.F8;
            this.btnOptions.Size = new Size(0x34, 0x34);
            this.btnOptions.ToolTipText = "<LOC>Options";
            this.btnOptions.Click += new EventHandler(this.btnOptions_Click);
            this.btnMore.AutoSize = false;
            this.btnMore.AutoToolTip = true;
            this.btnMore.Image = (Image) manager.GetObject("btnMore.Image");
            this.btnMore.ImageScaling = ToolStripItemImageScaling.None;
            this.btnMore.Name = "btnMore";
            this.btnMore.ShortcutKeys = Keys.F6;
            this.btnMore.Size = new Size(20, 0x34);
            this.btnMore.ToolTipText = "<LOC>More...";
            this.btnMore.Click += new EventHandler(this.btnMore_Click);
            this.pbBottomRight.Dock = DockStyle.Right;
            this.pbBottomRight.Image = (Image) manager.GetObject("pbBottomRight.Image");
            this.pbBottomRight.Location = new Point(0x3aa, 0);
            this.pbBottomRight.Name = "pbBottomRight";
            this.pbBottomRight.Size = new Size(0x3e, 0x58);
            this.pbBottomRight.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbBottomRight, null);
            this.pbBottomRight.TabIndex = 2;
            this.pbBottomRight.TabStop = false;
            this.pbBottomRight.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbBottomRight.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbBottom.Dock = DockStyle.Fill;
            this.pbBottom.Image = (Image) manager.GetObject("pbBottom.Image");
            this.pbBottom.Location = new Point(0x3d, 0);
            this.pbBottom.Name = "pbBottom";
            this.pbBottom.Size = new Size(0x3ab, 0x58);
            this.pbBottom.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbBottom, null);
            this.pbBottom.TabIndex = 1;
            this.pbBottom.TabStop = false;
            this.pbBottom.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbBottom.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbBottomLeft.Dock = DockStyle.Left;
            this.pbBottomLeft.Image = (Image) manager.GetObject("pbBottomLeft.Image");
            this.pbBottomLeft.Location = new Point(0, 0);
            this.pbBottomLeft.Name = "pbBottomLeft";
            this.pbBottomLeft.Size = new Size(0x3d, 0x58);
            this.pbBottomLeft.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbBottomLeft, null);
            this.pbBottomLeft.TabIndex = 0;
            this.pbBottomLeft.TabStop = false;
            this.pbBottomLeft.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbBottomLeft.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pTop.Controls.Add(this.msMainMenu);
            this.pTop.Controls.Add(this.pbClose);
            this.pTop.Controls.Add(this.pbRestore);
            this.pTop.Controls.Add(this.pbMinimize);
            this.pTop.Controls.Add(this.pbTopRight);
            this.pTop.Controls.Add(this.pbTop);
            this.pTop.Controls.Add(this.pbTopLeft);
            this.pTop.Dock = DockStyle.Top;
            this.pTop.Location = new Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new Size(0x3e8, 0xc9);
            base.ttDefault.SetSuperTip(this.pTop, null);
            this.pTop.TabIndex = 4;
            this.pTop.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.msMainMenu.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.msMainMenu.AutoSize = false;
            this.msMainMenu.BackColor = System.Drawing.Color.Transparent;
            this.msMainMenu.BackgroundImage = (Image) manager.GetObject("msMainMenu.BackgroundImage");
            this.msMainMenu.Dock = DockStyle.None;
            this.msMainMenu.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.msMainMenu.ImageScalingSize = new Size(0, 0);
            this.msMainMenu.Items.AddRange(new ToolStripItem[] { this.miMainGroup, this.miAdmin, this.miGameGroup, this.miRankings, this.miLadders, this.miTools, this.miHelp });
            this.msMainMenu.Location = new Point(0x3d, 0x2e);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Size = new Size(0x35f, 0x18);
            base.ttDefault.SetSuperTip(this.msMainMenu, null);
            this.msMainMenu.TabIndex = 9;
            this.msMainMenu.Text = "menuStrip1";
            this.miMainGroup.DropDownItems.AddRange(new ToolStripItem[] { this.miMain_Logout, this.miMain_Exit, this.miChangeEmail, this.miConsolidateAccounts });
            this.miMainGroup.Name = "miMainGroup";
            this.miMainGroup.Size = new Size(90, 20);
            this.miMainGroup.Text = "<LOC>Main";
            this.miMainGroup.TextAlign = ContentAlignment.BottomCenter;
            this.miMain_Logout.Name = "miMain_Logout";
            this.miMain_Logout.Size = new Size(0xfb, 0x16);
            this.miMain_Logout.Text = "<LOC>Logout";
            this.miMain_Logout.Click += new EventHandler(this.miMain_Logout_Click);
            this.miMain_Exit.Name = "miMain_Exit";
            this.miMain_Exit.Size = new Size(0xfb, 0x16);
            this.miMain_Exit.Text = "<LOC>Exit";
            this.miMain_Exit.Click += new EventHandler(this.miMain_Exit_Click);
            this.miChangeEmail.Name = "miChangeEmail";
            this.miChangeEmail.Size = new Size(0xfb, 0x16);
            this.miChangeEmail.Text = "<LOC>Change Email Address";
            this.miChangeEmail.Click += new EventHandler(this.mChangeEmail_Click);
            this.miConsolidateAccounts.Name = "miConsolidateAccounts";
            this.miConsolidateAccounts.Size = new Size(0xfb, 0x16);
            this.miConsolidateAccounts.Text = "<LOC>Consolidate Accounts";
            this.miConsolidateAccounts.Click += new EventHandler(this.miConsolidateAccounts_Click);
            this.miAdmin.DropDownItems.AddRange(new ToolStripItem[] { this.miCustomAdmin, this.miAdhocSQL, this.adhocChartsToolStripMenuItem, this.miPacketSniffer, this.miShowEventLog, this.miAdmin_CreateVolunteerEffort, this.miAdmin_ViewVolunteers, this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem, this.miAdmin_Security, this.miAdmin_Avatars, this.spaceSiegeLobbyToolStripMenuItem, this.manageServerGamesToolStripMenuItem });
            this.miAdmin.Name = "miAdmin";
            this.miAdmin.Size = new Size(0x63, 20);
            this.miAdmin.Text = "<LOC>Admin";
            this.miAdmin.TextAlign = ContentAlignment.BottomCenter;
            this.miCustomAdmin.Name = "miCustomAdmin";
            this.miCustomAdmin.Size = new Size(0x18e, 0x16);
            this.miCustomAdmin.Text = "Custom Tools";
            this.miAdhocSQL.Name = "miAdhocSQL";
            this.miAdhocSQL.Size = new Size(0x18e, 0x16);
            this.miAdhocSQL.Text = "Adhoc SQL";
            this.miAdhocSQL.Click += new EventHandler(this.miAdhocSQL_Click);
            this.adhocChartsToolStripMenuItem.Name = "adhocChartsToolStripMenuItem";
            this.adhocChartsToolStripMenuItem.Size = new Size(0x18e, 0x16);
            this.adhocChartsToolStripMenuItem.Text = "Adhoc Charts";
            this.adhocChartsToolStripMenuItem.Click += new EventHandler(this.adhocChartsToolStripMenuItem_Click);
            this.miPacketSniffer.Name = "miPacketSniffer";
            this.miPacketSniffer.Size = new Size(0x18e, 0x16);
            this.miPacketSniffer.Tag = "Dev";
            this.miPacketSniffer.Text = "UDP Packet Sniffer";
            this.miPacketSniffer.Click += new EventHandler(this.miPacketSniffer_Click);
            this.miShowEventLog.Name = "miShowEventLog";
            this.miShowEventLog.Size = new Size(0x18e, 0x16);
            this.miShowEventLog.Tag = "Dev";
            this.miShowEventLog.Text = "Show Event Log";
            this.miShowEventLog.Click += new EventHandler(this.miEventLog_Click);
            this.miAdmin_CreateVolunteerEffort.Name = "miAdmin_CreateVolunteerEffort";
            this.miAdmin_CreateVolunteerEffort.Size = new Size(0x18e, 0x16);
            this.miAdmin_CreateVolunteerEffort.Text = "Create Volunteer Effort";
            this.miAdmin_CreateVolunteerEffort.Click += new EventHandler(this.miAdmin_CreateVolunteerEffort_Click);
            this.miAdmin_ViewVolunteers.Name = "miAdmin_ViewVolunteers";
            this.miAdmin_ViewVolunteers.Size = new Size(0x18e, 0x16);
            this.miAdmin_ViewVolunteers.Text = "View Volunteers";
            this.miAdmin_ViewVolunteers.Click += new EventHandler(this.miAdmin_ViewVolunteers_Click);
            this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem.Name = "forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem";
            this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem.Size = new Size(0x18e, 0x16);
            this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem.Tag = "Dev";
            this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem.Text = "Force all users to restart GPGnet (must be THE Admin)";
            this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem.Click += new EventHandler(this.forceAllUsersToRestartGPGnetmustBeTHEAdminToolStripMenuItem_Click);
            this.miAdmin_Security.Name = "miAdmin_Security";
            this.miAdmin_Security.Size = new Size(0x18e, 0x16);
            this.miAdmin_Security.Text = "Security";
            this.miAdmin_Security.Click += new EventHandler(this.miAdmin_Security_Click);
            this.miAdmin_Avatars.Name = "miAdmin_Avatars";
            this.miAdmin_Avatars.Size = new Size(0x18e, 0x16);
            this.miAdmin_Avatars.Text = "Assign Player Avatars";
            this.miAdmin_Avatars.Click += new EventHandler(this.miAdmin_Avatars_Click);
            this.spaceSiegeLobbyToolStripMenuItem.Name = "spaceSiegeLobbyToolStripMenuItem";
            this.spaceSiegeLobbyToolStripMenuItem.Size = new Size(0x18e, 0x16);
            this.spaceSiegeLobbyToolStripMenuItem.Text = "Space Siege Lobby";
            this.spaceSiegeLobbyToolStripMenuItem.Click += new EventHandler(this.spaceSiegeLobbyToolStripMenuItem_Click);
            this.manageServerGamesToolStripMenuItem.Name = "manageServerGamesToolStripMenuItem";
            this.manageServerGamesToolStripMenuItem.Size = new Size(0x18e, 0x16);
            this.manageServerGamesToolStripMenuItem.Text = "Manage Server Games";
            this.manageServerGamesToolStripMenuItem.Click += new EventHandler(this.manageServerGamesToolStripMenuItem_Click);
            this.miGameGroup.DropDownItems.AddRange(new ToolStripItem[] { this.miCustomGame, this.miJoinGame, this.miAutomatch, this.lOCArrangedTeamGameToolStripMenuItem, this.miGame_Vault, this.miCreateTournament, this.miTournamentSchedule, this.miGame_RedeemPrize });
            this.miGameGroup.Enabled = false;
            this.miGameGroup.Name = "miGameGroup";
            this.miGameGroup.Size = new Size(0x61, 20);
            this.miGameGroup.Text = "<LOC>Game";
            this.miGameGroup.TextAlign = ContentAlignment.BottomCenter;
            this.miCustomGame.Image = GPG.Multiplayer.Client.Properties.Resources.host;
            this.miCustomGame.Name = "miCustomGame";
            this.miCustomGame.Size = new Size(0x110, 0x16);
            this.miCustomGame.Text = "<LOC>Host Custom Game";
            this.miCustomGame.Click += new EventHandler(this.miCustomGame_Click);
            this.miJoinGame.Image = GPG.Multiplayer.Client.Properties.Resources.jointeam2;
            this.miJoinGame.Name = "miJoinGame";
            this.miJoinGame.Size = new Size(0x110, 0x16);
            this.miJoinGame.Text = "<LOC>Join Custom Game";
            this.miJoinGame.Click += new EventHandler(this.miJoinGame_Click);
            this.miAutomatch.Name = "miAutomatch";
            this.miAutomatch.Size = new Size(0x110, 0x16);
            this.miAutomatch.Text = "<LOC>Play Ranked Game";
            this.miAutomatch.Click += new EventHandler(this.miAutomatch_Click);
            this.lOCArrangedTeamGameToolStripMenuItem.Name = "lOCArrangedTeamGameToolStripMenuItem";
            this.lOCArrangedTeamGameToolStripMenuItem.Size = new Size(0x110, 0x16);
            this.lOCArrangedTeamGameToolStripMenuItem.Text = "<LOC>Arranged Team Game";
            this.lOCArrangedTeamGameToolStripMenuItem.Click += new EventHandler(this.lOCArrangedTeamGameToolStripMenuItem_Click);
            this.miGame_Vault.Name = "miGame_Vault";
            this.miGame_Vault.Size = new Size(0x110, 0x16);
            this.miGame_Vault.Text = "<LOC>Replay Vault";
            this.miGame_Vault.Click += new EventHandler(this.miGame_Vault_Click);
            this.miCreateTournament.Name = "miCreateTournament";
            this.miCreateTournament.Size = new Size(0x110, 0x16);
            this.miCreateTournament.Text = "<LOC>Create Tournament";
            this.miCreateTournament.Visible = false;
            this.miCreateTournament.Click += new EventHandler(this.miCreateTournament_Click);
            this.miTournamentSchedule.Name = "miTournamentSchedule";
            this.miTournamentSchedule.Size = new Size(0x110, 0x16);
            this.miTournamentSchedule.Text = "<LOC>Tournament Schedule";
            this.miTournamentSchedule.Click += new EventHandler(this.miTournamentSchedule_Click);
            this.miGame_RedeemPrize.Enabled = false;
            this.miGame_RedeemPrize.Name = "miGame_RedeemPrize";
            this.miGame_RedeemPrize.Size = new Size(0x110, 0x16);
            this.miGame_RedeemPrize.Text = "<LOC>Redeem Tournament Prize";
            this.miGame_RedeemPrize.Visible = false;
            this.miGame_RedeemPrize.Click += new EventHandler(this.miGame_RedeemPrize_Click);
            this.miRankings.DropDownItems.AddRange(new ToolStripItem[] { this.miRankings_1v1, this.lOC2v2RankingsToolStripMenuItem, this.lOC3v3RankingsToolStripMenuItem, this.lOC4v4RankingsToolStripMenuItem, this.lOCClanRankingsToolStripMenuItem });
            this.miRankings.Name = "miRankings";
            this.miRankings.Size = new Size(0x74, 20);
            this.miRankings.Text = "<LOC>Rankings";
            this.miRankings.TextAlign = ContentAlignment.BottomCenter;
            this.miRankings_1v1.Image = GPG.Multiplayer.Client.Properties.Resources.rankings1;
            this.miRankings_1v1.Name = "miRankings_1v1";
            this.miRankings_1v1.Size = new Size(0xd6, 0x16);
            this.miRankings_1v1.Text = "<LOC>1v1 Rankings...";
            this.miRankings_1v1.Click += new EventHandler(this.miRankings_1v1_Click);
            this.lOC2v2RankingsToolStripMenuItem.Name = "lOC2v2RankingsToolStripMenuItem";
            this.lOC2v2RankingsToolStripMenuItem.Size = new Size(0xd6, 0x16);
            this.lOC2v2RankingsToolStripMenuItem.Text = "<LOC>2v2 Rankings...";
            this.lOC2v2RankingsToolStripMenuItem.Click += new EventHandler(this.lOC2v2RankingsToolStripMenuItem_Click);
            this.lOC3v3RankingsToolStripMenuItem.Name = "lOC3v3RankingsToolStripMenuItem";
            this.lOC3v3RankingsToolStripMenuItem.Size = new Size(0xd6, 0x16);
            this.lOC3v3RankingsToolStripMenuItem.Text = "<LOC>3v3 Rankings...";
            this.lOC3v3RankingsToolStripMenuItem.Click += new EventHandler(this.lOC3v3RankingsToolStripMenuItem_Click);
            this.lOC4v4RankingsToolStripMenuItem.Name = "lOC4v4RankingsToolStripMenuItem";
            this.lOC4v4RankingsToolStripMenuItem.Size = new Size(0xd6, 0x16);
            this.lOC4v4RankingsToolStripMenuItem.Text = "<LOC>4v4 Rankings...";
            this.lOC4v4RankingsToolStripMenuItem.Click += new EventHandler(this.lOC4v4RankingsToolStripMenuItem_Click);
            this.lOCClanRankingsToolStripMenuItem.Name = "lOCClanRankingsToolStripMenuItem";
            this.lOCClanRankingsToolStripMenuItem.Size = new Size(0xd6, 0x16);
            this.lOCClanRankingsToolStripMenuItem.Text = "<LOC>Clan Rankings...";
            this.lOCClanRankingsToolStripMenuItem.Click += new EventHandler(this.lOCClanRankingsToolStripMenuItem_Click);
            this.miLadders.DropDownItems.AddRange(new ToolStripItem[] { this.miLadders_AcceptAll, this.miLadders_DeclineAll });
            this.miLadders.Name = "miLadders";
            this.miLadders.Size = new Size(0x6c, 20);
            this.miLadders.Text = "<LOC>Ladders";
            this.miLadders.TextAlign = ContentAlignment.BottomCenter;
            this.miLadders_AcceptAll.Name = "miLadders_AcceptAll";
            this.miLadders_AcceptAll.Size = new Size(0xf8, 0x16);
            this.miLadders_AcceptAll.Text = "<LOC>Accept All Challenges";
            this.miLadders_AcceptAll.Click += new EventHandler(this.miLadders_AcceptAll_Click);
            this.miLadders_DeclineAll.Name = "miLadders_DeclineAll";
            this.miLadders_DeclineAll.Size = new Size(0xf8, 0x16);
            this.miLadders_DeclineAll.Text = "<LOC>Decline All Challenges";
            this.miLadders_DeclineAll.Click += new EventHandler(this.miLadders_DeclineAll_Click);
            this.miTools.DropDownItems.AddRange(new ToolStripItem[] { this.miTools_Feedback, this.miTools_GameKeys, this.miTools_Chat, this.miTools_ContentManager, this.miTools_Options, this.miTools_LocPatches, this.miManageGames });
            this.miTools.Name = "miTools";
            this.miTools.Size = new Size(0x5d, 20);
            this.miTools.Text = "<LOC>Tools";
            this.miTools.TextAlign = ContentAlignment.BottomCenter;
            this.miTools_Feedback.Name = "miTools_Feedback";
            this.miTools_Feedback.Size = new Size(210, 0x16);
            this.miTools_Feedback.Text = "<LOC>Feedback";
            this.miTools_Feedback.Click += new EventHandler(this.miTools_Feedback_Click);
            this.miTools_GameKeys.Name = "miTools_GameKeys";
            this.miTools_GameKeys.Size = new Size(210, 0x16);
            this.miTools_GameKeys.Text = "<LOC>Game Keys...";
            this.miTools_GameKeys.Visible = false;
            this.miTools_GameKeys.Click += new EventHandler(this.miTools_GameKeys_Click);
            this.miTools_Chat.DropDownItems.AddRange(new ToolStripItem[] { this.miTools_Chat_Emotes });
            this.miTools_Chat.Name = "miTools_Chat";
            this.miTools_Chat.Size = new Size(210, 0x16);
            this.miTools_Chat.Text = "<LOC>Chat";
            this.miTools_Chat_Emotes.Name = "miTools_Chat_Emotes";
            this.miTools_Chat_Emotes.Size = new Size(0xaf, 0x16);
            this.miTools_Chat_Emotes.Text = "<LOC>Emotes...";
            this.miTools_Chat_Emotes.Click += new EventHandler(this.miTools_Chat_Emotes_Click);
            this.miTools_ContentManager.Name = "miTools_ContentManager";
            this.miTools_ContentManager.Size = new Size(210, 0x16);
            this.miTools_ContentManager.Text = "<LOC>The Vault";
            this.miTools_ContentManager.Click += new EventHandler(this.miTools_ContentManager_Click);
            this.miTools_Options.Name = "miTools_Options";
            this.miTools_Options.Size = new Size(210, 0x16);
            this.miTools_Options.Text = "<LOC>Options...";
            this.miTools_Options.Click += new EventHandler(this.miTools_Options_Click);
            this.miTools_LocPatches.Enabled = false;
            this.miTools_LocPatches.Name = "miTools_LocPatches";
            this.miTools_LocPatches.Size = new Size(210, 0x16);
            this.miTools_LocPatches.Text = "Loc Patches";
            this.miTools_LocPatches.Visible = false;
            this.miTools_LocPatches.Click += new EventHandler(this.miTools_LocPatches_Click);
            this.miManageGames.Name = "miManageGames";
            this.miManageGames.Size = new Size(210, 0x16);
            this.miManageGames.Text = "<LOC>Manage Games";
            this.miManageGames.Click += new EventHandler(this.miManageGames_Click);
            this.miHelp.DropDownItems.AddRange(new ToolStripItem[] { this.miHelp_Solutions, this.miHelp_Volunteer, this.miHelp_SupComHome, this.miHelp_GPGHome, this.miForums, this.miHelp_ReportIssue, this.miSpaceSiegeWeb, this.miHelp_About });
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new Size(0x58, 20);
            this.miHelp.Text = "<LOC>Help";
            this.miHelp.TextAlign = ContentAlignment.BottomCenter;
            this.miHelp_Solutions.Name = "miHelp_Solutions";
            this.miHelp_Solutions.Size = new Size(0x151, 0x16);
            this.miHelp_Solutions.Text = "<LOC>GPGnet Knowledge Base";
            this.miHelp_Solutions.Click += new EventHandler(this.miHelp_Solutions_Click);
            this.miHelp_Volunteer.Name = "miHelp_Volunteer";
            this.miHelp_Volunteer.Size = new Size(0x151, 0x16);
            this.miHelp_Volunteer.Text = "<LOC>Volunteer Opportunities";
            this.miHelp_Volunteer.Click += new EventHandler(this.miHelp_Volunteer_Click);
            this.miHelp_SupComHome.Name = "miHelp_SupComHome";
            this.miHelp_SupComHome.Size = new Size(0x151, 0x16);
            this.miHelp_SupComHome.Text = "<LOC>SupremeCommander.com";
            this.miHelp_SupComHome.Click += new EventHandler(this.miHelp_SupComHome_Click);
            this.miHelp_GPGHome.Name = "miHelp_GPGHome";
            this.miHelp_GPGHome.Size = new Size(0x151, 0x16);
            this.miHelp_GPGHome.Text = "<LOC>GasPowered.com";
            this.miHelp_GPGHome.Click += new EventHandler(this.miHelp_GPGHome_Click);
            this.miForums.Name = "miForums";
            this.miForums.Size = new Size(0x151, 0x16);
            this.miForums.Text = "<LOC>Forums.GasPowered.com";
            this.miForums.Click += new EventHandler(this.miForums_Click);
            this.miHelp_ReportIssue.Name = "miHelp_ReportIssue";
            this.miHelp_ReportIssue.Size = new Size(0x151, 0x16);
            this.miHelp_ReportIssue.Text = "<LOC>Report an Issue";
            this.miHelp_ReportIssue.Click += new EventHandler(this.miHelp_ReportIssue_Click);
            this.miHelp_About.Name = "miHelp_About";
            this.miHelp_About.Size = new Size(0x151, 0x16);
            this.miHelp_About.Text = "<LOC>About GPGnet: Supreme Commander";
            this.miHelp_About.Click += new EventHandler(this.aboutGPGNetToolStripMenuItem_Click);
            this.miSpaceSiegeWeb.Name = "miSpaceSiegeWeb";
            this.miSpaceSiegeWeb.Size = new Size(0x151, 0x16);
            this.miSpaceSiegeWeb.Text = "<LOC>spacesiege.com";
            this.miSpaceSiegeWeb.Visible = false;
            this.miSpaceSiegeWeb.Click += new EventHandler(this.lOCToolStripMenuItem_Click);
            this.pbClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pbClose.Image = (Image) manager.GetObject("pbClose.Image");
            this.pbClose.Location = new Point(0x39a, 10);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new Size(0x21, 0x20);
            this.pbClose.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbClose, null);
            this.pbClose.TabIndex = 6;
            this.pbClose.TabStop = false;
            this.pbClose.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbClose.Click += new EventHandler(this.pbClose_Click);
            this.pbRestore.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pbRestore.Image = (Image) manager.GetObject("pbRestore.Image");
            this.pbRestore.Location = new Point(0x37b, 10);
            this.pbRestore.Name = "pbRestore";
            this.pbRestore.Size = new Size(0x21, 0x20);
            this.pbRestore.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbRestore, null);
            this.pbRestore.TabIndex = 5;
            this.pbRestore.TabStop = false;
            this.pbRestore.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbRestore.Click += new EventHandler(this.pbRestore_Click);
            this.pbMinimize.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pbMinimize.Image = (Image) manager.GetObject("pbMinimize.Image");
            this.pbMinimize.Location = new Point(0x35b, 10);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new Size(0x21, 0x20);
            this.pbMinimize.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbMinimize, null);
            this.pbMinimize.TabIndex = 4;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbMinimize.Click += new EventHandler(this.pbMinimize_Click);
            this.pbTopRight.Dock = DockStyle.Right;
            this.pbTopRight.Image = (Image) manager.GetObject("pbTopRight.Image");
            this.pbTopRight.Location = new Point(0x359, 0);
            this.pbTopRight.Name = "pbTopRight";
            this.pbTopRight.Size = new Size(0x8f, 0xc9);
            this.pbTopRight.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbTopRight, null);
            this.pbTopRight.TabIndex = 3;
            this.pbTopRight.TabStop = false;
            this.pbTopRight.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbTopRight.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbTop.Dock = DockStyle.Fill;
            this.pbTop.Image = (Image) manager.GetObject("pbTop.Image");
            this.pbTop.Location = new Point(0xa3, 0);
            this.pbTop.Name = "pbTop";
            this.pbTop.Size = new Size(0x345, 0xc9);
            this.pbTop.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbTop, null);
            this.pbTop.TabIndex = 2;
            this.pbTop.TabStop = false;
            this.pbTop.DoubleClick += new EventHandler(this.pbRestore_Click);
            this.pbTop.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbTop.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbTopLeft.Dock = DockStyle.Left;
            this.pbTopLeft.Image = (Image) manager.GetObject("pbTopLeft.Image");
            this.pbTopLeft.Location = new Point(0, 0);
            this.pbTopLeft.Name = "pbTopLeft";
            this.pbTopLeft.Size = new Size(0xa3, 0xc9);
            this.pbTopLeft.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbTopLeft, null);
            this.pbTopLeft.TabIndex = 0;
            this.pbTopLeft.TabStop = false;
            this.pbTopLeft.DoubleClick += new EventHandler(this.pbRestore_Click);
            this.pbTopLeft.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbTopLeft.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.wbMain.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.wbMain.Location = new Point(0x31, 0x48);
            this.wbMain.MinimumSize = new Size(20, 20);
            this.wbMain.Name = "wbMain";
            this.wbMain.Size = new Size(0x38b, 90);
            base.ttDefault.SetSuperTip(this.wbMain, null);
            this.wbMain.TabIndex = 8;
            this.wbMain.Url = new Uri("", UriKind.Relative);
            this.splitContainerFriends.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerFriends.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.splitContainerFriends.FixedPanel = FixedPanel.Panel1;
            this.splitContainerFriends.IsSplitterFixed = true;
            this.splitContainerFriends.Location = new Point(2, 2);
            this.splitContainerFriends.Name = "splitContainerFriends";
            this.splitContainerFriends.Orientation = Orientation.Horizontal;
            this.splitContainerFriends.Panel1.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.splitContainerFriends.Panel1.Controls.Add(this.gpgLabelFriendInvites2);
            this.splitContainerFriends.Panel1.Controls.Add(this.gpgLabelFriendInvitesCount2);
            base.ttDefault.SetSuperTip(this.splitContainerFriends.Panel1, null);
            this.splitContainerFriends.Panel1MinSize = 20;
            this.splitContainerFriends.Panel2.Controls.Add(this.gpgScrollPanelFriends);
            base.ttDefault.SetSuperTip(this.splitContainerFriends.Panel2, null);
            this.splitContainerFriends.Size = new Size(0x106, 0x1c7);
            this.splitContainerFriends.SplitterDistance = 0x17;
            this.splitContainerFriends.SplitterWidth = 1;
            base.ttDefault.SetSuperTip(this.splitContainerFriends, null);
            this.splitContainerFriends.TabIndex = 5;
            this.gpgLabelFriendInvites2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelFriendInvites2.AutoSize = true;
            this.gpgLabelFriendInvites2.AutoStyle = true;
            this.gpgLabelFriendInvites2.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgLabelFriendInvites2.Font = new Font("Arial", 9.75f);
            this.gpgLabelFriendInvites2.ForeColor = System.Drawing.Color.White;
            this.gpgLabelFriendInvites2.IgnoreMouseWheel = false;
            this.gpgLabelFriendInvites2.IsStyled = false;
            this.gpgLabelFriendInvites2.Location = new Point(3, 4);
            this.gpgLabelFriendInvites2.Name = "gpgLabelFriendInvites2";
            this.gpgLabelFriendInvites2.Size = new Size(0x8b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelFriendInvites2, null);
            this.gpgLabelFriendInvites2.TabIndex = 12;
            this.gpgLabelFriendInvites2.Text = "<LOC>View Invitations";
            this.gpgLabelFriendInvites2.TextStyle = TextStyles.Link;
            this.gpgLabelFriendInvites2.Click += new EventHandler(this.gpgLabelFriendInvites2_Click);
            this.gpgLabelFriendInvitesCount2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelFriendInvitesCount2.AutoSize = true;
            this.gpgLabelFriendInvitesCount2.AutoStyle = true;
            this.gpgLabelFriendInvitesCount2.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabelFriendInvitesCount2.ForeColor = System.Drawing.Color.White;
            this.gpgLabelFriendInvitesCount2.IgnoreMouseWheel = false;
            this.gpgLabelFriendInvitesCount2.IsStyled = false;
            this.gpgLabelFriendInvitesCount2.Location = new Point(0x7b, 3);
            this.gpgLabelFriendInvitesCount2.Margin = new Padding(0);
            this.gpgLabelFriendInvitesCount2.Name = "gpgLabelFriendInvitesCount2";
            this.gpgLabelFriendInvitesCount2.Size = new Size(0x17, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelFriendInvitesCount2, null);
            this.gpgLabelFriendInvitesCount2.TabIndex = 11;
            this.gpgLabelFriendInvitesCount2.Text = "(0)";
            this.gpgLabelFriendInvitesCount2.TextStyle = TextStyles.Default;
            this.gpgScrollPanelFriends.AutoScroll = true;
            this.gpgScrollPanelFriends.BackColor = System.Drawing.Color.Black;
            this.gpgScrollPanelFriends.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgScrollPanelFriends.BorderThickness = 2;
            this.gpgScrollPanelFriends.ChildControl = null;
            this.gpgScrollPanelFriends.Controls.Add(this.pnlUserListFriends);
            this.gpgScrollPanelFriends.Controls.Add(this.gpgLabelNoFriends);
            this.gpgScrollPanelFriends.Dock = DockStyle.Fill;
            this.gpgScrollPanelFriends.DrawBorder = false;
            this.gpgScrollPanelFriends.Location = new Point(0, 0);
            this.gpgScrollPanelFriends.Name = "gpgScrollPanelFriends";
            this.gpgScrollPanelFriends.Size = new Size(0x106, 0x1af);
            base.ttDefault.SetSuperTip(this.gpgScrollPanelFriends, null);
            this.gpgScrollPanelFriends.TabIndex = 0;
            this.pnlUserListFriends.AutoRefresh = true;
            this.pnlUserListFriends.AutoScroll = true;
            this.pnlUserListFriends.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.pnlUserListFriends.Dock = DockStyle.Fill;
            this.pnlUserListFriends.Location = new Point(0, 0);
            this.pnlUserListFriends.Name = "pnlUserListFriends";
            this.pnlUserListFriends.Size = new Size(0x106, 0x1af);
            this.pnlUserListFriends.Style = UserListStyles.OnlineOffline;
            base.ttDefault.SetSuperTip(this.pnlUserListFriends, null);
            this.pnlUserListFriends.TabIndex = 0;
            this.gpgLabelNoFriends.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNoFriends.AutoStyle = true;
            this.gpgLabelNoFriends.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgLabelNoFriends.Dock = DockStyle.Fill;
            this.gpgLabelNoFriends.Font = new Font("Arial", 9.75f);
            this.gpgLabelNoFriends.ForeColor = System.Drawing.Color.White;
            this.gpgLabelNoFriends.IgnoreMouseWheel = false;
            this.gpgLabelNoFriends.IsStyled = false;
            this.gpgLabelNoFriends.Location = new Point(0, 0);
            this.gpgLabelNoFriends.Name = "gpgLabelNoFriends";
            this.gpgLabelNoFriends.Size = new Size(0x106, 0x1af);
            base.ttDefault.SetSuperTip(this.gpgLabelNoFriends, null);
            this.gpgLabelNoFriends.TabIndex = 1;
            this.gpgLabelNoFriends.Text = manager.GetString("gpgLabelNoFriends.Text");
            this.gpgLabelNoFriends.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelNoFriends.TextStyle = TextStyles.Default;
            this.splitContainerClan.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerClan.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.splitContainerClan.FixedPanel = FixedPanel.Panel1;
            this.splitContainerClan.IsSplitterFixed = true;
            this.splitContainerClan.Location = new Point(2, 0x16);
            this.splitContainerClan.Margin = new Padding(0);
            this.splitContainerClan.Name = "splitContainerClan";
            this.splitContainerClan.Orientation = Orientation.Horizontal;
            this.splitContainerClan.Panel1.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.splitContainerClan.Panel1.Controls.Add(this.gpgLabelClanRequests);
            this.splitContainerClan.Panel1.Controls.Add(this.gpgLabelClanRequestCount);
            base.ttDefault.SetSuperTip(this.splitContainerClan.Panel1, null);
            this.splitContainerClan.Panel1MinSize = 20;
            this.splitContainerClan.Panel2.Controls.Add(this.gpgScrollPanelClan);
            this.splitContainerClan.Panel2.Controls.Add(this.gpgScrollPanelNoClan);
            base.ttDefault.SetSuperTip(this.splitContainerClan.Panel2, null);
            this.splitContainerClan.Size = new Size(0x106, 0x1b3);
            this.splitContainerClan.SplitterDistance = 20;
            this.splitContainerClan.SplitterWidth = 1;
            base.ttDefault.SetSuperTip(this.splitContainerClan, null);
            this.splitContainerClan.TabIndex = 6;
            this.gpgLabelClanRequests.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelClanRequests.AutoSize = true;
            this.gpgLabelClanRequests.AutoStyle = true;
            this.gpgLabelClanRequests.Font = new Font("Arial", 9.75f);
            this.gpgLabelClanRequests.ForeColor = System.Drawing.Color.White;
            this.gpgLabelClanRequests.IgnoreMouseWheel = false;
            this.gpgLabelClanRequests.IsStyled = false;
            this.gpgLabelClanRequests.Location = new Point(-1, 0);
            this.gpgLabelClanRequests.Name = "gpgLabelClanRequests";
            this.gpgLabelClanRequests.Size = new Size(0x89, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelClanRequests, null);
            this.gpgLabelClanRequests.TabIndex = 2;
            this.gpgLabelClanRequests.Text = "<LOC>View Requests";
            this.gpgLabelClanRequests.TextStyle = TextStyles.Link;
            this.gpgLabelClanRequests.Click += new EventHandler(this.gpgLabelClanRequests_Click);
            this.gpgLabelClanRequestCount.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelClanRequestCount.AutoSize = true;
            this.gpgLabelClanRequestCount.AutoStyle = true;
            this.gpgLabelClanRequestCount.Font = new Font("Arial", 9.75f);
            this.gpgLabelClanRequestCount.ForeColor = System.Drawing.Color.White;
            this.gpgLabelClanRequestCount.IgnoreMouseWheel = false;
            this.gpgLabelClanRequestCount.IsStyled = false;
            this.gpgLabelClanRequestCount.Location = new Point(140, 0);
            this.gpgLabelClanRequestCount.Name = "gpgLabelClanRequestCount";
            this.gpgLabelClanRequestCount.Size = new Size(0x17, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelClanRequestCount, null);
            this.gpgLabelClanRequestCount.TabIndex = 1;
            this.gpgLabelClanRequestCount.Text = "(0)";
            this.gpgLabelClanRequestCount.TextStyle = TextStyles.Default;
            this.gpgScrollPanelClan.AutoScroll = true;
            this.gpgScrollPanelClan.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgScrollPanelClan.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgScrollPanelClan.BorderThickness = 2;
            this.gpgScrollPanelClan.ChildControl = null;
            this.gpgScrollPanelClan.Controls.Add(this.pnlUserListClan);
            this.gpgScrollPanelClan.Dock = DockStyle.Fill;
            this.gpgScrollPanelClan.DrawBorder = false;
            this.gpgScrollPanelClan.Location = new Point(0, 0);
            this.gpgScrollPanelClan.Name = "gpgScrollPanelClan";
            this.gpgScrollPanelClan.Size = new Size(0x106, 0x19e);
            base.ttDefault.SetSuperTip(this.gpgScrollPanelClan, null);
            this.gpgScrollPanelClan.TabIndex = 1;
            this.pnlUserListClan.AutoRefresh = true;
            this.pnlUserListClan.AutoScroll = true;
            this.pnlUserListClan.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.pnlUserListClan.Dock = DockStyle.Fill;
            this.pnlUserListClan.Location = new Point(0, 0);
            this.pnlUserListClan.Name = "pnlUserListClan";
            this.pnlUserListClan.Size = new Size(0x106, 0x19e);
            this.pnlUserListClan.Style = UserListStyles.Clan;
            base.ttDefault.SetSuperTip(this.pnlUserListClan, null);
            this.pnlUserListClan.TabIndex = 0;
            this.gpgScrollPanelNoClan.AutoScroll = true;
            this.gpgScrollPanelNoClan.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgScrollPanelNoClan.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgScrollPanelNoClan.BorderThickness = 2;
            this.gpgScrollPanelNoClan.ChildControl = null;
            this.gpgScrollPanelNoClan.Controls.Add(this.gpgLabelClanInvites);
            this.gpgScrollPanelNoClan.Controls.Add(this.gpgLabelCreateClan);
            this.gpgScrollPanelNoClan.Controls.Add(this.gpgLabelClanInviteCount);
            this.gpgScrollPanelNoClan.Controls.Add(this.gpgLabelNoClan);
            this.gpgScrollPanelNoClan.Dock = DockStyle.Fill;
            this.gpgScrollPanelNoClan.DrawBorder = false;
            this.gpgScrollPanelNoClan.Location = new Point(0, 0);
            this.gpgScrollPanelNoClan.Name = "gpgScrollPanelNoClan";
            this.gpgScrollPanelNoClan.Size = new Size(0x106, 0x19e);
            base.ttDefault.SetSuperTip(this.gpgScrollPanelNoClan, null);
            this.gpgScrollPanelNoClan.TabIndex = 5;
            this.gpgLabelClanInvites.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelClanInvites.AutoSize = true;
            this.gpgLabelClanInvites.AutoStyle = true;
            this.gpgLabelClanInvites.Font = new Font("Arial", 9.75f);
            this.gpgLabelClanInvites.ForeColor = System.Drawing.Color.White;
            this.gpgLabelClanInvites.IgnoreMouseWheel = false;
            this.gpgLabelClanInvites.IsStyled = false;
            this.gpgLabelClanInvites.Location = new Point(4, 0x5c);
            this.gpgLabelClanInvites.Name = "gpgLabelClanInvites";
            this.gpgLabelClanInvites.Size = new Size(0x8b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelClanInvites, null);
            this.gpgLabelClanInvites.TabIndex = 9;
            this.gpgLabelClanInvites.Text = "<LOC>View Invitations";
            this.gpgLabelClanInvites.TextStyle = TextStyles.Link;
            this.gpgLabelClanInvites.Click += new EventHandler(this.gpgLabelClanInvites_Click);
            this.gpgLabelCreateClan.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelCreateClan.AutoSize = true;
            this.gpgLabelCreateClan.AutoStyle = true;
            this.gpgLabelCreateClan.Font = new Font("Arial", 9.75f);
            this.gpgLabelCreateClan.ForeColor = System.Drawing.Color.White;
            this.gpgLabelCreateClan.IgnoreMouseWheel = false;
            this.gpgLabelCreateClan.IsStyled = false;
            this.gpgLabelCreateClan.Location = new Point(4, 0x43);
            this.gpgLabelCreateClan.Name = "gpgLabelCreateClan";
            this.gpgLabelCreateClan.Size = new Size(0x76, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelCreateClan, null);
            this.gpgLabelCreateClan.TabIndex = 8;
            this.gpgLabelCreateClan.Text = "<LOC>Create Clan";
            this.gpgLabelCreateClan.TextStyle = TextStyles.Link;
            this.gpgLabelCreateClan.Click += new EventHandler(this.gpgLabelCreateClan_Click);
            this.gpgLabelClanInviteCount.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelClanInviteCount.AutoSize = true;
            this.gpgLabelClanInviteCount.AutoStyle = true;
            this.gpgLabelClanInviteCount.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabelClanInviteCount.ForeColor = System.Drawing.Color.White;
            this.gpgLabelClanInviteCount.IgnoreMouseWheel = false;
            this.gpgLabelClanInviteCount.IsStyled = false;
            this.gpgLabelClanInviteCount.Location = new Point(0x7b, 0x5c);
            this.gpgLabelClanInviteCount.Margin = new Padding(0);
            this.gpgLabelClanInviteCount.Name = "gpgLabelClanInviteCount";
            this.gpgLabelClanInviteCount.Size = new Size(0x17, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelClanInviteCount, null);
            this.gpgLabelClanInviteCount.TabIndex = 7;
            this.gpgLabelClanInviteCount.Text = "(0)";
            this.gpgLabelClanInviteCount.TextStyle = TextStyles.Default;
            this.gpgLabelNoClan.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNoClan.AutoStyle = true;
            this.gpgLabelNoClan.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgLabelNoClan.Dock = DockStyle.Top;
            this.gpgLabelNoClan.Font = new Font("Arial", 9.75f);
            this.gpgLabelNoClan.ForeColor = System.Drawing.Color.White;
            this.gpgLabelNoClan.IgnoreMouseWheel = false;
            this.gpgLabelNoClan.IsStyled = false;
            this.gpgLabelNoClan.Location = new Point(0, 0);
            this.gpgLabelNoClan.Name = "gpgLabelNoClan";
            this.gpgLabelNoClan.Size = new Size(0x106, 0x35);
            base.ttDefault.SetSuperTip(this.gpgLabelNoClan, null);
            this.gpgLabelNoClan.TabIndex = 4;
            this.gpgLabelNoClan.Text = "<LOC id=_310c9adeef7ceaff3fb510305d0d2d37>You are not currently a member of a clan.  To join one, choose from the following options";
            this.gpgLabelNoClan.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabelNoClan.TextStyle = TextStyles.Default;
            this.skinLabelClanName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabelClanName.AutoStyle = false;
            this.skinLabelClanName.BackColor = System.Drawing.Color.Black;
            this.skinLabelClanName.Cursor = Cursors.Hand;
            this.skinLabelClanName.DrawEdges = true;
            this.skinLabelClanName.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelClanName.ForeColor = System.Drawing.Color.White;
            this.skinLabelClanName.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelClanName.IsStyled = false;
            this.skinLabelClanName.Location = new Point(2, 2);
            this.skinLabelClanName.Margin = new Padding(0);
            this.skinLabelClanName.Name = "skinLabelClanName";
            this.skinLabelClanName.Size = new Size(0x106, 20);
            this.skinLabelClanName.SkinBasePath = @"Controls\Background Label\Rectangle";
            base.ttDefault.SetSuperTip(this.skinLabelClanName, null);
            this.skinLabelClanName.TabIndex = 3;
            this.skinLabelClanName.TextAlign = ContentAlignment.MiddleCenter;
            this.skinLabelClanName.TextPadding = new Padding(0);
            this.skinLabelClanName.Click += new EventHandler(this.skinLabelClanName_Click);
            this.pbLeftBorder.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pbLeftBorder.Image = (Image) manager.GetObject("pbLeftBorder.Image");
            this.pbLeftBorder.Location = new Point(0, 0xc9);
            this.pbLeftBorder.Name = "pbLeftBorder";
            this.pbLeftBorder.Size = new Size(6, 0x200);
            this.pbLeftBorder.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbLeftBorder, null);
            this.pbLeftBorder.TabIndex = 6;
            this.pbLeftBorder.TabStop = false;
            this.pbLeftBorder.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbRightBorder.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pbRightBorder.Image = (Image) manager.GetObject("pbRightBorder.Image");
            this.pbRightBorder.Location = new Point(0x3e3, 0xc9);
            this.pbRightBorder.Name = "pbRightBorder";
            this.pbRightBorder.Size = new Size(5, 0x1f7);
            this.pbRightBorder.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbRightBorder, null);
            this.pbRightBorder.TabIndex = 7;
            this.pbRightBorder.TabStop = false;
            this.pbRightBorder.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.rimPictureEdit.Name = "rimPictureEdit";
            this.rimMemoEdit.Name = "rimMemoEdit";
            this.rimPictureEdit2.Name = "rimPictureEdit2";
            this.rimMemoEdit2.Name = "rimMemoEdit2";
            this.gpgChatGrid.CustomizeStyle = true;
            this.gpgChatGrid.Dock = DockStyle.Fill;
            this.gpgChatGrid.EmbeddedNavigator.Name = "";
            this.gpgChatGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgChatGrid.IgnoreMouseWheel = false;
            this.gpgChatGrid.Location = new Point(0, 0);
            this.gpgChatGrid.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgChatGrid.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            this.gpgChatGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgChatGrid.MainView = this.gvChat;
            this.gpgChatGrid.Name = "gpgChatGrid";
            this.gpgChatGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3 });
            this.gpgChatGrid.ShowOnlyPredefinedDetails = true;
            this.gpgChatGrid.Size = new Size(0x2bd, 0x1a0);
            this.gpgChatGrid.TabIndex = 10;
            this.gpgChatGrid.ViewCollection.AddRange(new BaseView[] { this.gvChat });
            this.gvChat.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.FromArgb(20, 20, 20);
            this.gvChat.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.Gray;
            this.gvChat.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvChat.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvChat.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvChat.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.gvChat.Appearance.ColumnFilterButtonActive.BackColor2 = System.Drawing.Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvChat.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.gvChat.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Blue;
            this.gvChat.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvChat.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvChat.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvChat.Appearance.Empty.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.Empty.Options.UseBackColor = true;
            this.gvChat.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(0xd4, 0xd0, 200);
            this.gvChat.Appearance.FilterCloseButton.BackColor2 = System.Drawing.Color.FromArgb(90, 90, 90);
            this.gvChat.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(0xd4, 0xd0, 200);
            this.gvChat.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvChat.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvChat.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvChat.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvChat.Appearance.FilterPanel.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.FromArgb(0xd4, 0xd0, 200);
            this.gvChat.Appearance.FilterPanel.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvChat.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvChat.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvChat.Appearance.FocusedCell.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvChat.Appearance.FocusedCell.Options.UseFont = true;
            this.gvChat.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvChat.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(0x40, 0x40, 0x40);
            this.gvChat.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.Black;
            this.gvChat.Appearance.FocusedRow.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            this.gvChat.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvChat.Appearance.FocusedRow.Options.UseFont = true;
            this.gvChat.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvChat.Appearance.FooterPanel.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.FooterPanel.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvChat.Appearance.FooterPanel.Options.UseFont = true;
            this.gvChat.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupButton.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.GroupButton.BorderColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.GroupButton.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvChat.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(10, 10, 10);
            this.gvChat.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(10, 10, 10);
            this.gvChat.Appearance.GroupFooter.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvChat.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupPanel.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvChat.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvChat.Appearance.GroupPanel.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupPanel.Options.UseFont = true;
            this.gvChat.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupRow.BackColor = System.Drawing.Color.Gray;
            this.gvChat.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupRow.Options.UseFont = true;
            this.gvChat.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvChat.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.HeaderPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvChat.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvChat.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvChat.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvChat.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvChat.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvChat.Appearance.HorzLine.BackColor = System.Drawing.Color.Yellow;
            this.gvChat.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvChat.Appearance.Preview.BackColor = System.Drawing.Color.White;
            this.gvChat.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.Preview.ForeColor = System.Drawing.Color.Purple;
            this.gvChat.Appearance.Preview.Options.UseBackColor = true;
            this.gvChat.Appearance.Preview.Options.UseFont = true;
            this.gvChat.Appearance.Preview.Options.UseForeColor = true;
            this.gvChat.Appearance.Row.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0xb2);
            this.gvChat.Appearance.Row.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.Row.Options.UseBackColor = true;
            this.gvChat.Appearance.Row.Options.UseFont = true;
            this.gvChat.Appearance.Row.Options.UseForeColor = true;
            this.gvChat.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvChat.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.Black;
            this.gvChat.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvChat.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(0x40, 0x40, 0x40);
            this.gvChat.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.Black;
            this.gvChat.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvChat.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvChat.Appearance.SelectedRow.Options.UseFont = true;
            this.gvChat.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvChat.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.TopNewRow.ForeColor = System.Drawing.Color.White;
            this.gvChat.Appearance.TopNewRow.Options.UseFont = true;
            this.gvChat.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvChat.Appearance.VertLine.BackColor = System.Drawing.Color.Yellow;
            this.gvChat.Appearance.VertLine.Options.UseBackColor = true;
            this.gvChat.BorderStyle = BorderStyles.NoBorder;
            this.gvChat.Columns.AddRange(new GridColumn[] { this.colIcon, this.colPlayer, this.colText, this.gcVisible, this.colTimeStamp });
            this.gvChat.FocusRectStyle = DrawFocusRectStyle.None;
            this.gvChat.GridControl = this.gpgChatGrid;
            this.gvChat.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvChat.Name = "gvChat";
            this.gvChat.OptionsDetail.AllowZoomDetail = false;
            this.gvChat.OptionsDetail.EnableMasterViewMode = false;
            this.gvChat.OptionsDetail.ShowDetailTabs = false;
            this.gvChat.OptionsDetail.SmartDetailExpand = false;
            this.gvChat.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvChat.OptionsSelection.MultiSelect = true;
            this.gvChat.OptionsView.RowAutoHeight = true;
            this.gvChat.OptionsView.ShowColumnHeaders = false;
            this.gvChat.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            this.gvChat.OptionsView.ShowGroupPanel = false;
            this.gvChat.OptionsView.ShowHorzLines = false;
            this.gvChat.OptionsView.ShowIndicator = false;
            this.gvChat.OptionsView.ShowVertLines = false;
            this.gvChat.PaintStyleName = "Web";
            this.gvChat.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvChat_CustomDrawCell);
            this.gvChat.RowCountChanged += new EventHandler(this.gvChat_RowCountChanged);
            this.gvChat.TopRowChanged += new EventHandler(this.gvChat_TopRowChanged);
            this.gvChat.CalcRowHeight += new RowHeightEventHandler(this.gvChat_CalcRowHeight);
            this.colIcon.Caption = "<LOC>Player Icon";
            this.colIcon.ColumnEdit = this.rimPictureEdit3;
            this.colIcon.FieldName = "Icon";
            this.colIcon.MinWidth = 0x2a;
            this.colIcon.Name = "colIcon";
            this.colIcon.OptionsColumn.AllowEdit = false;
            this.colIcon.OptionsColumn.FixedWidth = true;
            this.colIcon.OptionsColumn.ReadOnly = true;
            this.colIcon.Width = 0x2a;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.colPlayer.Caption = "<LOC>Player Name";
            this.colPlayer.ColumnEdit = this.rimMemoEdit3;
            this.colPlayer.FieldName = "PlayerInfo";
            this.colPlayer.Name = "colPlayer";
            this.colPlayer.OptionsColumn.AllowEdit = false;
            this.colPlayer.OptionsColumn.FixedWidth = true;
            this.colPlayer.OptionsColumn.ReadOnly = true;
            this.colPlayer.Visible = true;
            this.colPlayer.VisibleIndex = 0;
            this.colPlayer.Width = 150;
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
            this.colText.Caption = "<LOC>Chat Content";
            this.colText.ColumnEdit = this.rimMemoEdit3;
            this.colText.FieldName = "Text";
            this.colText.Name = "colText";
            this.colText.OptionsColumn.AllowEdit = false;
            this.colText.OptionsColumn.ReadOnly = true;
            this.colText.Visible = true;
            this.colText.VisibleIndex = 1;
            this.colText.Width = 0x188;
            this.gcVisible.Caption = "Visible";
            this.gcVisible.FieldName = "IsVisible";
            this.gcVisible.Name = "gcVisible";
            this.gcVisible.OptionsColumn.ShowInCustomizationForm = false;
            this.colTimeStamp.AppearanceCell.ForeColor = System.Drawing.Color.White;
            this.colTimeStamp.AppearanceCell.Options.UseForeColor = true;
            this.colTimeStamp.Caption = "<LOC>Time";
            this.colTimeStamp.FieldName = "TimeStamp";
            this.colTimeStamp.Name = "colTimeStamp";
            this.colTimeStamp.OptionsColumn.AllowEdit = false;
            this.colTimeStamp.OptionsColumn.ReadOnly = true;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.ilIcons.ImageStream = (ImageListStreamer) manager.GetObject("ilIcons.ImageStream");
            this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilIcons.Images.SetKeyName(0, "pdahlke-netlab19.png");
            this.gridColumn1.Caption = "Player Icon";
            this.gridColumn1.ColumnEdit = this.repositoryItemPictureEdit1;
            this.gridColumn1.FieldName = "Icon";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.FixedWidth = true;
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 40;
            this.gridColumn2.Caption = "Player Name";
            this.gridColumn2.ColumnEdit = this.repositoryItemMemoEdit1;
            this.gridColumn2.FieldName = "PlayerInfo";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.FixedWidth = true;
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 100;
            this.gridColumn3.Caption = "Chat Content";
            this.gridColumn3.ColumnEdit = this.repositoryItemMemoEdit1;
            this.gridColumn3.FieldName = "Text";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 0x192;
            this.pcTextEntry.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.pcTextEntry.Appearance.BackColor = System.Drawing.Color.Black;
            this.pcTextEntry.Appearance.Options.UseBackColor = true;
            this.pcTextEntry.BorderStyle = BorderStyles.NoBorder;
            this.pcTextEntry.Location = new Point(0x18, 0x28e);
            this.pcTextEntry.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.pcTextEntry.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pcTextEntry.LookAndFeel.UseWindowsXPTheme = true;
            this.pcTextEntry.Name = "pcTextEntry";
            this.pcTextEntry.Size = new Size(0x2cc, 0x18);
            base.ttDefault.SetSuperTip(this.pcTextEntry, null);
            this.pcTextEntry.TabIndex = 12;
            this.pcTextEntry.Text = "panelControl1";
            this.gpgContextMenuChat.MenuItems.AddRange(new MenuItem[] { 
                this.ciChat_WhisperPlayer, this.ciChat_IgnorePlayer, this.ciChat_UnignorePlayer, this.ciChat_ViewRank, this.ciChat_WebStats, this.ciChat_ViewPlayer, this.miViewReplays, this.menuItem10, this.ciChat_InviteFriend, this.ciChat_RemoveFriend, this.menuItem8, this.ciChat_InviteToClan, this.ciChat_RequestClanInvite, this.ciChat_ViewClan, this.ciChat_PromoteClan, this.ciChat_DemoteClan, 
                this.ciChat_RemoveClan, this.ciChat_LeaveClan, this.menuItem3, this.ciChat_Kick, this.ciChat_Ban, this.menuItem7, this.ciChat_TeamInvite
             });
            this.ciChat_WhisperPlayer.Index = 0;
            this.ciChat_WhisperPlayer.Text = "<LOC>Send private message";
            this.ciChat_WhisperPlayer.Click += new EventHandler(this.ciChat_WhisperPlayer_Click);
            this.ciChat_IgnorePlayer.Index = 1;
            this.ciChat_IgnorePlayer.Text = "<LOC>Ignore this player";
            this.ciChat_IgnorePlayer.Click += new EventHandler(this.ciChat_IgnorePlayer_Click);
            this.ciChat_UnignorePlayer.Index = 2;
            this.ciChat_UnignorePlayer.Text = "<LOC>Unignore this player";
            this.ciChat_UnignorePlayer.Click += new EventHandler(this.ciChat_UnignorePlayer_Click);
            this.ciChat_ViewRank.Index = 3;
            this.ciChat_ViewRank.Text = "<LOC>View in ranking ladder";
            this.ciChat_ViewRank.Visible = false;
            this.ciChat_ViewRank.Click += new EventHandler(this.ciChat_ViewRank_Click);
            this.ciChat_WebStats.Index = 4;
            this.ciChat_WebStats.Text = "<LOC>View web statistics";
            this.ciChat_WebStats.Click += new EventHandler(this.ciChat_WebStats_Click);
            this.ciChat_ViewPlayer.Index = 5;
            this.ciChat_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChat_ViewPlayer.Click += new EventHandler(this.ciChat_ViewPlayer_Click);
            this.miViewReplays.Index = 6;
            this.miViewReplays.Text = "<LOC>View this player's Replays";
            this.miViewReplays.Click += new EventHandler(this.miViewReplays_Click);
            this.menuItem10.Index = 7;
            this.menuItem10.Text = "-";
            this.ciChat_InviteFriend.Index = 8;
            this.ciChat_InviteFriend.Text = "<LOC>Invite player to join Friends list";
            this.ciChat_InviteFriend.Click += new EventHandler(this.ciChat_InviteFriend_Click);
            this.ciChat_RemoveFriend.Index = 9;
            this.ciChat_RemoveFriend.Text = "<LOC>Remove player from Friends list";
            this.ciChat_RemoveFriend.Click += new EventHandler(this.ciChat_RemoveFriend_Click);
            this.menuItem8.Index = 10;
            this.menuItem8.Text = "-";
            this.ciChat_InviteToClan.Index = 11;
            this.ciChat_InviteToClan.Text = "<LOC>Invite this player to join clan";
            this.ciChat_InviteToClan.Click += new EventHandler(this.ciChat_InviteToClan_Click);
            this.ciChat_RequestClanInvite.Index = 12;
            this.ciChat_RequestClanInvite.Text = "<LOC>Request to join this player's clan";
            this.ciChat_RequestClanInvite.Click += new EventHandler(this.ciChat_RequestClanInvite_Click);
            this.ciChat_ViewClan.Index = 13;
            this.ciChat_ViewClan.Text = "<LOC>View this clan's profile";
            this.ciChat_ViewClan.Click += new EventHandler(this.ciChat_ViewClan_Click);
            this.ciChat_PromoteClan.Index = 14;
            this.ciChat_PromoteClan.Text = "<LOC>Promote";
            this.ciChat_PromoteClan.Click += new EventHandler(this.ciChat_PromoteClan_Click);
            this.ciChat_DemoteClan.Index = 15;
            this.ciChat_DemoteClan.Text = "<LOC>Demote";
            this.ciChat_DemoteClan.Click += new EventHandler(this.ciChat_DemoteClan_Click);
            this.ciChat_RemoveClan.Index = 0x10;
            this.ciChat_RemoveClan.Text = "<LOC>Remove from clan";
            this.ciChat_RemoveClan.Click += new EventHandler(this.ciChat_RemoveClan_Click);
            this.ciChat_LeaveClan.Index = 0x11;
            this.ciChat_LeaveClan.Text = "<LOC>Leave clan";
            this.ciChat_LeaveClan.Click += new EventHandler(this.ciChat_LeaveClan_Click);
            this.menuItem3.Index = 0x12;
            this.menuItem3.Text = "-";
            this.ciChat_Kick.Index = 0x13;
            this.ciChat_Kick.Text = "<LOC>Kick";
            this.ciChat_Kick.Click += new EventHandler(this.ciChat_Kick_Click);
            this.ciChat_Ban.Index = 20;
            this.ciChat_Ban.Text = "<LOC>Ban";
            this.ciChat_Ban.Click += new EventHandler(this.ciChat_Ban_Click);
            this.menuItem7.Index = 0x15;
            this.menuItem7.Text = "-";
            this.ciChat_TeamInvite.Index = 0x16;
            this.ciChat_TeamInvite.Text = "<LOC>Invite to Arranged Team";
            this.ciChat_TeamInvite.Click += new EventHandler(this.ciChat_TeamInvite_Click);
            this.textBoxMsg.BorderColor = System.Drawing.Color.White;
            this.textBoxMsg.Dock = DockStyle.Fill;
            this.textBoxMsg.Location = new Point(0, 0);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Properties.AcceptsReturn = false;
            this.textBoxMsg.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.textBoxMsg.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(0x52, 0x83, 190);
            this.textBoxMsg.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.textBoxMsg.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxMsg.Properties.Appearance.Options.UseBorderColor = true;
            this.textBoxMsg.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxMsg.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(0x2e, 0x2e, 0x49);
            this.textBoxMsg.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.FromArgb(0, 0, 0);
            this.textBoxMsg.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.textBoxMsg.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.textBoxMsg.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.textBoxMsg.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.textBoxMsg.Properties.BorderStyle = BorderStyles.NoBorder;
            this.textBoxMsg.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.textBoxMsg.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.textBoxMsg.Properties.MaxLength = 0x400;
            this.textBoxMsg.Size = new Size(0x2bd, 0x25);
            this.textBoxMsg.TabIndex = 0;
            this.textBoxMsg.EditValueChanging += new ChangingEventHandler(this.textBoxMsg_EditValueChanging);
            this.pbMiddle.AutoStyle = false;
            this.pbMiddle.BackColor = System.Drawing.Color.Black;
            this.pbMiddle.DrawEdges = true;
            this.pbMiddle.Font = new Font("Arial", 8f);
            this.pbMiddle.ForeColor = System.Drawing.Color.White;
            this.pbMiddle.HorizontalScalingMode = ScalingModes.Tile;
            this.pbMiddle.IsStyled = false;
            this.pbMiddle.ItemPadding = 6;
            this.pbMiddle.Location = new Point(0x26, 0xf2);
            this.pbMiddle.Name = "pbMiddle";
            this.pbMiddle.Size = new Size(0x7d, 0x19);
            this.pbMiddle.SkinBasePath = @"Controls\Background Label\Rectangle";
            base.ttDefault.SetSuperTip(this.pbMiddle, null);
            this.pbMiddle.TabIndex = 0x10;
            this.pbMiddle.TextAlign = ContentAlignment.MiddleLeft;
            this.pbMiddle.TextPadding = new Padding(20, 0, 0, 0);
            this.miStatus_Online.Index = -1;
            this.miStatus_Online.Text = "<LOC>Online";
            this.miStatus_Away.Index = -1;
            this.miStatus_Away.Text = "Away";
            this.miStatus_DND.Index = -1;
            this.miStatus_DND.Text = "DND";
            this.menuItem1.Index = -1;
            this.menuItem1.Text = "<LOC>Online";
            this.menuItem2.Index = -1;
            this.menuItem2.Text = "<LOC>Online";
            this.gpgContextMenuChatText.MenuItems.AddRange(new MenuItem[] { 
                this.ciChatText_Clear, this.ciChatText_Copy, this.ciChatText_Filter, this.miShowColumns, this.ciChatText_ShowEmotes, this.menuItm15, this.ciChatText_PrivateMsg, this.ciChatText_Ignore, this.ciChatText_Unignore, this.ciChatText_ViewRank, this.ciChatText_WebStats, this.ciChatText_ViewPlayer, this.ciChatText_Replays, this.menuItem6, this.ciChatText_FriendInvite, this.ciChatText_FriendRemove, 
                this.menuItem11, this.ciChatText_ClanInvite, this.ciChatText_ClanRequest, this.ciChatText_ClanRemove, this.ciChatText_Promote, this.ciChatText_Demote, this.ciChatText_ViewClan, this.ciChatText_LeaveClan, this.menuItem18, this.ciChatText_Kick, this.ciChatText_Ban, this.menuItem12, this.ciChatText_Solution, this.miTranslate
             });
            this.ciChatText_Clear.Index = 0;
            this.ciChatText_Clear.Text = "<LOC>Clear Chat";
            this.ciChatText_Clear.Click += new EventHandler(this.ciChatText_Clear_Click);
            this.ciChatText_Copy.Index = 1;
            this.ciChatText_Copy.Text = "<LOC>Copy to Clipboard";
            this.ciChatText_Copy.Click += new EventHandler(this.ciChatText_Copy_Click);
            this.ciChatText_Filter.Index = 2;
            this.ciChatText_Filter.MenuItems.AddRange(new MenuItem[] { this.ciChatText_Filter_Self, this.ciChatText_Filter_System, this.ciChatText_Filter_Events, this.ciChatText_Filter_Errors, this.ciChatText_Filter_Game, this.ciChatText_Filter_Friends, this.ciChatText_Filter_Clan, this.ciChatText_Filter_Admin, this.ciChatText_Filter_Other, this.menuItem4, this.ciChatText_Filters_Reset });
            this.ciChatText_Filter.Text = "<LOC>Filter by";
            this.ciChatText_Filter_Self.Checked = true;
            this.ciChatText_Filter_Self.Index = 0;
            this.ciChatText_Filter_Self.Text = "<LOC>Self";
            this.ciChatText_Filter_Self.Click += new EventHandler(this.ciChatText_Filter_Self_Click);
            this.ciChatText_Filter_System.Checked = true;
            this.ciChatText_Filter_System.Index = 1;
            this.ciChatText_Filter_System.Text = "<LOC>System Messages";
            this.ciChatText_Filter_System.Click += new EventHandler(this.ciChatText_Filter_System_Click);
            this.ciChatText_Filter_Events.Checked = true;
            this.ciChatText_Filter_Events.Index = 2;
            this.ciChatText_Filter_Events.Text = "<LOC>System Events";
            this.ciChatText_Filter_Events.Click += new EventHandler(this.ciChatText_Filter_Events_Click);
            this.ciChatText_Filter_Errors.Checked = true;
            this.ciChatText_Filter_Errors.Index = 3;
            this.ciChatText_Filter_Errors.Text = "<LOC>System Errors";
            this.ciChatText_Filter_Errors.Click += new EventHandler(this.ciChatText_Filter_Errors_Click);
            this.ciChatText_Filter_Game.Checked = true;
            this.ciChatText_Filter_Game.Index = 4;
            this.ciChatText_Filter_Game.Text = "<LOC>Game Events";
            this.ciChatText_Filter_Game.Click += new EventHandler(this.ciChatText_Filter_Game_Click);
            this.ciChatText_Filter_Friends.Checked = true;
            this.ciChatText_Filter_Friends.Index = 5;
            this.ciChatText_Filter_Friends.Text = "<LOC>Friends";
            this.ciChatText_Filter_Friends.Click += new EventHandler(this.ciChatText_Filter_Friends_Click);
            this.ciChatText_Filter_Clan.Checked = true;
            this.ciChatText_Filter_Clan.Index = 6;
            this.ciChatText_Filter_Clan.Text = "<LOC>Clan";
            this.ciChatText_Filter_Clan.Click += new EventHandler(this.ciChatText_Filter_Clan_Click);
            this.ciChatText_Filter_Admin.Checked = true;
            this.ciChatText_Filter_Admin.Index = 7;
            this.ciChatText_Filter_Admin.Text = "<LOC>Admins";
            this.ciChatText_Filter_Admin.Click += new EventHandler(this.ciChatText_Filter_Admin_Click);
            this.ciChatText_Filter_Other.Checked = true;
            this.ciChatText_Filter_Other.Index = 8;
            this.ciChatText_Filter_Other.Text = "<LOC>Other";
            this.ciChatText_Filter_Other.Click += new EventHandler(this.ciChatText_Filter_Other_Click);
            this.menuItem4.Index = 9;
            this.menuItem4.Text = "-";
            this.ciChatText_Filters_Reset.Index = 10;
            this.ciChatText_Filters_Reset.Text = "<LOC>Reset Filters";
            this.ciChatText_Filters_Reset.Click += new EventHandler(this.ciChatText_Filters_Reset_Click);
            this.miShowColumns.Index = 3;
            this.miShowColumns.Text = "<LOC>Show Columns";
            this.miShowColumns.Click += new EventHandler(this.miShowColumns_Click);
            this.ciChatText_ShowEmotes.Index = 4;
            this.ciChatText_ShowEmotes.Text = "<LOC>Show emotes";
            this.ciChatText_ShowEmotes.Click += new EventHandler(this.ciChatText_ShowEmotes_Click);
            this.menuItm15.Index = 5;
            this.menuItm15.Text = "-";
            this.ciChatText_PrivateMsg.Index = 6;
            this.ciChatText_PrivateMsg.Text = "<LOC>Send private message";
            this.ciChatText_PrivateMsg.Click += new EventHandler(this.ciChatText_PrivateMsg_Click);
            this.ciChatText_Ignore.Index = 7;
            this.ciChatText_Ignore.Text = "<LOC>Ignore this player";
            this.ciChatText_Ignore.Click += new EventHandler(this.ciChatText_Ignore_Click);
            this.ciChatText_Unignore.Index = 8;
            this.ciChatText_Unignore.Text = "<LOC>Unignore this player";
            this.ciChatText_Unignore.Click += new EventHandler(this.ciChatText_Unignore_Click);
            this.ciChatText_ViewRank.Index = 9;
            this.ciChatText_ViewRank.Text = "<LOC>View in ranking ladder";
            this.ciChatText_ViewRank.Visible = false;
            this.ciChatText_ViewRank.Click += new EventHandler(this.ciChatText_ViewRank_Click);
            this.ciChatText_WebStats.Index = 10;
            this.ciChatText_WebStats.Text = "<LOC>View web statistics";
            this.ciChatText_WebStats.Click += new EventHandler(this.ciChatText_WebStats_Click);
            this.ciChatText_ViewPlayer.Index = 11;
            this.ciChatText_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChatText_ViewPlayer.Click += new EventHandler(this.ciChatText_ViewPlayer_Click);
            this.ciChatText_Replays.Index = 12;
            this.ciChatText_Replays.Text = "<LOC>View this player's Replays";
            this.ciChatText_Replays.Click += new EventHandler(this.ciChatText_Replays_Click);
            this.menuItem6.Index = 13;
            this.menuItem6.Text = "-";
            this.ciChatText_FriendInvite.Index = 14;
            this.ciChatText_FriendInvite.Text = "<LOC>Invite this player to join Friends list";
            this.ciChatText_FriendInvite.Click += new EventHandler(this.ciChatText_FriendInvite_Click);
            this.ciChatText_FriendRemove.Index = 15;
            this.ciChatText_FriendRemove.Text = "<LOC>Remove this player from Friends list";
            this.ciChatText_FriendRemove.Click += new EventHandler(this.ciChatText_FriendRemove_Click);
            this.menuItem11.Index = 0x10;
            this.menuItem11.Text = "-";
            this.ciChatText_ClanInvite.Index = 0x11;
            this.ciChatText_ClanInvite.Text = "<LOC>Invite this player to join clan";
            this.ciChatText_ClanInvite.Click += new EventHandler(this.ciChatText_ClanInvite_Click);
            this.ciChatText_ClanRequest.Index = 0x12;
            this.ciChatText_ClanRequest.Text = "<LOC>Request to join this player's clan";
            this.ciChatText_ClanRequest.Click += new EventHandler(this.ciChatText_ClanRequest_Click);
            this.ciChatText_ClanRemove.Index = 0x13;
            this.ciChatText_ClanRemove.Text = "<LOC>Remove this player from clan";
            this.ciChatText_ClanRemove.Click += new EventHandler(this.ciChatText_ClanRemove_Click);
            this.ciChatText_Promote.Index = 20;
            this.ciChatText_Promote.Text = "<LOC>Promote";
            this.ciChatText_Promote.Click += new EventHandler(this.ciChatText_Promote_Click);
            this.ciChatText_Demote.Index = 0x15;
            this.ciChatText_Demote.Text = "<LOC>Demote";
            this.ciChatText_Demote.Click += new EventHandler(this.ciChatText_Demote_Click);
            this.ciChatText_ViewClan.Index = 0x16;
            this.ciChatText_ViewClan.Text = "<LOC>View this clan's profile";
            this.ciChatText_ViewClan.Click += new EventHandler(this.ciChatText_ViewClan_Click);
            this.ciChatText_LeaveClan.Index = 0x17;
            this.ciChatText_LeaveClan.Text = "<LOC>Leave clan";
            this.ciChatText_LeaveClan.Click += new EventHandler(this.ciChatText_LeaveClan_Click);
            this.menuItem18.Index = 0x18;
            this.menuItem18.Text = "-";
            this.ciChatText_Kick.Index = 0x19;
            this.ciChatText_Kick.Text = "<LOC>Kick";
            this.ciChatText_Kick.Click += new EventHandler(this.ciChatText_Kick_Click);
            this.ciChatText_Ban.Index = 0x1a;
            this.ciChatText_Ban.Text = "<LOC>Ban";
            this.ciChatText_Ban.Click += new EventHandler(this.ciChatText_Ban_Click);
            this.menuItem12.Index = 0x1b;
            this.menuItem12.Text = "-";
            this.ciChatText_Solution.Index = 0x1c;
            this.ciChatText_Solution.Text = "<LOC>Point user to solution";
            this.ciChatText_Solution.Click += new EventHandler(this.ciChatText_Solution_Click);
            this.miTranslate.Index = 0x1d;
            this.miTranslate.MenuItems.AddRange(new MenuItem[] { this.menuItem23, this.menuItem24, this.menuItem25, this.menuItem26, this.menuItem27, this.menuItem28, this.menuItem29 });
            this.miTranslate.Text = "Translate to English";
            this.miTranslate.Visible = false;
            this.menuItem23.Index = 0;
            this.menuItem23.Text = "German ";
            this.menuItem23.Click += new EventHandler(this.menuItem23_Click);
            this.menuItem24.Index = 1;
            this.menuItem24.Text = "French";
            this.menuItem24.Click += new EventHandler(this.menuItem24_Click);
            this.menuItem25.Index = 2;
            this.menuItem25.Text = "Itialian";
            this.menuItem25.Click += new EventHandler(this.menuItem25_Click);
            this.menuItem26.Index = 3;
            this.menuItem26.Text = "Spanish";
            this.menuItem26.Click += new EventHandler(this.menuItem26_Click);
            this.menuItem27.Index = 4;
            this.menuItem27.Text = "Russian";
            this.menuItem27.Click += new EventHandler(this.menuItem27_Click);
            this.menuItem28.Index = 5;
            this.menuItem28.Text = "Korean";
            this.menuItem28.Click += new EventHandler(this.menuItem28_Click);
            this.menuItem29.Index = 6;
            this.menuItem29.Text = "Japanese";
            this.menuItem29.Click += new EventHandler(this.menuItem29_Click);
            this.gpgContextMenuEmote.MenuItems.AddRange(new MenuItem[] { this.ciEmote_Manager, this.menuItem13, this.ciEmote_Hide, this.ciEmote_Share, this.ciEmote_Animate, this.menuItem9, this.ciEmote_Delete });
            this.ciEmote_Manager.Index = 0;
            this.ciEmote_Manager.Text = "<LOC>Emote manager...";
            this.ciEmote_Manager.Click += new EventHandler(this.ciEmote_Manager_Click);
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "-";
            this.ciEmote_Hide.Index = 2;
            this.ciEmote_Hide.Text = "<LOC>Hide emotes";
            this.ciEmote_Hide.Click += new EventHandler(this.ciEmote_Hide_Click);
            this.ciEmote_Share.Index = 3;
            this.ciEmote_Share.Text = "<LOC>Share emotes";
            this.ciEmote_Share.Click += new EventHandler(this.ciEmote_Share_Click);
            this.ciEmote_Animate.Index = 4;
            this.ciEmote_Animate.Text = "<LOC>Animate emotes";
            this.ciEmote_Animate.Click += new EventHandler(this.ciEmote_Animate_Click);
            this.menuItem9.Index = 5;
            this.menuItem9.Text = "-";
            this.ciEmote_Delete.Index = 6;
            this.ciEmote_Delete.Text = "<LOC>Delete emote";
            this.ciEmote_Delete.Click += new EventHandler(this.ciEmote_Delete_Click);
            this.pManualTabs.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pManualTabs.BackColor = System.Drawing.Color.Transparent;
            this.pManualTabs.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.pManualTabs.BorderThickness = 2;
            this.pManualTabs.Controls.Add(this.btnChatTab);
            this.pManualTabs.Controls.Add(this.btnFriendsTab);
            this.pManualTabs.Controls.Add(this.btnClanTab);
            this.pManualTabs.DrawBorder = false;
            this.pManualTabs.Location = new Point(0x2cb, 0x1b);
            this.pManualTabs.Name = "pManualTabs";
            this.pManualTabs.Size = new Size(0xeb, 0x16);
            base.ttDefault.SetSuperTip(this.pManualTabs, null);
            this.pManualTabs.TabIndex = 0x11;
            this.btnChatTab.AutoStyle = true;
            this.btnChatTab.BackColor = System.Drawing.Color.Black;
            this.btnChatTab.ButtonState = 0;
            this.btnChatTab.DialogResult = DialogResult.OK;
            this.btnChatTab.DisabledForecolor = System.Drawing.Color.Gray;
            this.btnChatTab.DrawColor = System.Drawing.Color.White;
            this.btnChatTab.DrawEdges = true;
            this.btnChatTab.FocusColor = System.Drawing.Color.Yellow;
            this.btnChatTab.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnChatTab.ForeColor = System.Drawing.Color.White;
            this.btnChatTab.HorizontalScalingMode = ScalingModes.Tile;
            this.btnChatTab.IsStyled = true;
            this.btnChatTab.Location = new Point(0, 0);
            this.btnChatTab.Name = "btnChatTab";
            this.btnChatTab.Size = new Size(0x54, 0x16);
            this.btnChatTab.SkinBasePath = @"Controls\Button\TabSmallActive";
            base.ttDefault.SetSuperTip(this.btnChatTab, null);
            this.btnChatTab.TabIndex = 0;
            this.btnChatTab.TabStop = true;
            this.btnChatTab.Text = "<LOC>Chat";
            this.btnChatTab.TextAlign = ContentAlignment.MiddleCenter;
            this.btnChatTab.TextPadding = new Padding(0);
            this.btnChatTab.Click += new EventHandler(this.btnChatTab_Click);
            this.btnFriendsTab.AutoStyle = true;
            this.btnFriendsTab.BackColor = System.Drawing.Color.Black;
            this.btnFriendsTab.ButtonState = 0;
            this.btnFriendsTab.DialogResult = DialogResult.OK;
            this.btnFriendsTab.DisabledForecolor = System.Drawing.Color.Gray;
            this.btnFriendsTab.DrawColor = System.Drawing.Color.White;
            this.btnFriendsTab.DrawEdges = true;
            this.btnFriendsTab.FocusColor = System.Drawing.Color.Yellow;
            this.btnFriendsTab.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnFriendsTab.ForeColor = System.Drawing.Color.White;
            this.btnFriendsTab.HorizontalScalingMode = ScalingModes.Tile;
            this.btnFriendsTab.IsStyled = true;
            this.btnFriendsTab.Location = new Point(0x4b, 0);
            this.btnFriendsTab.Name = "btnFriendsTab";
            this.btnFriendsTab.Size = new Size(0x54, 0x16);
            this.btnFriendsTab.SkinBasePath = @"Controls\Button\TabSmall";
            base.ttDefault.SetSuperTip(this.btnFriendsTab, null);
            this.btnFriendsTab.TabIndex = 1;
            this.btnFriendsTab.TabStop = true;
            this.btnFriendsTab.Text = "<LOC>Friends";
            this.btnFriendsTab.TextAlign = ContentAlignment.MiddleCenter;
            this.btnFriendsTab.TextPadding = new Padding(0);
            this.btnFriendsTab.Click += new EventHandler(this.btnFriendsTab_Click);
            this.btnClanTab.AutoStyle = true;
            this.btnClanTab.BackColor = System.Drawing.Color.Black;
            this.btnClanTab.ButtonState = 0;
            this.btnClanTab.DialogResult = DialogResult.OK;
            this.btnClanTab.DisabledForecolor = System.Drawing.Color.Gray;
            this.btnClanTab.DrawColor = System.Drawing.Color.White;
            this.btnClanTab.DrawEdges = true;
            this.btnClanTab.FocusColor = System.Drawing.Color.Yellow;
            this.btnClanTab.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnClanTab.ForeColor = System.Drawing.Color.White;
            this.btnClanTab.HorizontalScalingMode = ScalingModes.Tile;
            this.btnClanTab.IsStyled = true;
            this.btnClanTab.Location = new Point(150, 0);
            this.btnClanTab.Name = "btnClanTab";
            this.btnClanTab.Size = new Size(0x54, 0x16);
            this.btnClanTab.SkinBasePath = @"Controls\Button\TabSmall";
            base.ttDefault.SetSuperTip(this.btnClanTab, null);
            this.btnClanTab.TabIndex = 2;
            this.btnClanTab.TabStop = true;
            this.btnClanTab.Text = "<LOC>Clan";
            this.btnClanTab.TextAlign = ContentAlignment.MiddleCenter;
            this.btnClanTab.TextPadding = new Padding(0);
            this.btnClanTab.Click += new EventHandler(this.btnClanTab_Click);
            this.ilMenuItems.ImageStream = (ImageListStreamer) manager.GetObject("ilMenuItems.ImageStream");
            this.ilMenuItems.TransparentColor = System.Drawing.Color.Transparent;
            this.ilMenuItems.Images.SetKeyName(0, "rankings.png");
            this.gpgPanelChatAndInput.BackColor = System.Drawing.Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgPanelChatAndInput.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelChatAndInput.BorderThickness = 2;
            this.gpgPanelChatAndInput.Controls.Add(this.splitContainerChatAndInput);
            this.gpgPanelChatAndInput.Dock = DockStyle.Fill;
            this.gpgPanelChatAndInput.DrawBorder = true;
            this.gpgPanelChatAndInput.Location = new Point(0, 0);
            this.gpgPanelChatAndInput.Name = "gpgPanelChatAndInput";
            this.gpgPanelChatAndInput.Size = new Size(0x2c1, 0x1cb);
            base.ttDefault.SetSuperTip(this.gpgPanelChatAndInput, null);
            this.gpgPanelChatAndInput.TabIndex = 0x12;
            this.splitContainerChatAndInput.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerChatAndInput.Location = new Point(2, 2);
            this.splitContainerChatAndInput.Name = "splitContainerChatAndInput";
            this.splitContainerChatAndInput.Orientation = Orientation.Horizontal;
            this.splitContainerChatAndInput.Panel1.Controls.Add(this.gpgTextListCommands);
            this.splitContainerChatAndInput.Panel1.Controls.Add(this.gpgChatGrid);
            base.ttDefault.SetSuperTip(this.splitContainerChatAndInput.Panel1, null);
            this.splitContainerChatAndInput.Panel2.Controls.Add(this.textBoxMsg);
            base.ttDefault.SetSuperTip(this.splitContainerChatAndInput.Panel2, null);
            this.splitContainerChatAndInput.Size = new Size(0x2bd, 0x1c7);
            this.splitContainerChatAndInput.SplitterDistance = 0x1a0;
            this.splitContainerChatAndInput.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainerChatAndInput, null);
            this.splitContainerChatAndInput.TabIndex = 0;
            this.splitContainerChatAndInput.Paint += new PaintEventHandler(this.splitContainerChatAndInput_Paint);
            this.gpgTextListCommands.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextListCommands.AnchorControl = this.textBoxMsg;
            this.gpgTextListCommands.AutoScroll = true;
            this.gpgTextListCommands.AutoSize = true;
            this.gpgTextListCommands.BackColor = System.Drawing.Color.Black;
            this.gpgTextListCommands.Location = new Point(0, 0x18c);
            this.gpgTextListCommands.Margin = new Padding(0);
            this.gpgTextListCommands.MaxLines = 6;
            this.gpgTextListCommands.Name = "gpgTextListCommands";
            this.gpgTextListCommands.SelectedIndex = -1;
            this.gpgTextListCommands.Size = new Size(0x2bd, 20);
            base.ttDefault.SetSuperTip(this.gpgTextListCommands, null);
            this.gpgTextListCommands.TabIndex = 14;
            this.gpgTextListCommands.TextLines = null;
            this.gpgTextListCommands.Visible = false;
            this.gpgPanelGathering.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgPanelGathering.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelGathering.BorderThickness = 2;
            this.gpgPanelGathering.Controls.Add(this.gpgPanelGatheringDropDown);
            this.gpgPanelGathering.Controls.Add(this.pnlUserListChat);
            this.gpgPanelGathering.Dock = DockStyle.Fill;
            this.gpgPanelGathering.DrawBorder = true;
            this.gpgPanelGathering.Location = new Point(0, 0);
            this.gpgPanelGathering.Name = "gpgPanelGathering";
            this.gpgPanelGathering.Size = new Size(0x10a, 0x1cb);
            base.ttDefault.SetSuperTip(this.gpgPanelGathering, null);
            this.gpgPanelGathering.TabIndex = 0x13;
            this.gpgPanelGathering.Paint += new PaintEventHandler(this.gpgPanel1_Paint);
            this.gpgPanelGatheringDropDown.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanelGatheringDropDown.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgPanelGatheringDropDown.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelGatheringDropDown.BorderThickness = 2;
            this.gpgPanelGatheringDropDown.Controls.Add(this.skinGatheringDisplayChat);
            this.gpgPanelGatheringDropDown.DrawBorder = false;
            this.gpgPanelGatheringDropDown.Location = new Point(2, 7);
            this.gpgPanelGatheringDropDown.Name = "gpgPanelGatheringDropDown";
            this.gpgPanelGatheringDropDown.Size = new Size(0x106, 0x19);
            base.ttDefault.SetSuperTip(this.gpgPanelGatheringDropDown, null);
            this.gpgPanelGatheringDropDown.TabIndex = 13;
            this.splitContainerBody.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerBody.Location = new Point(11, 0xe9);
            this.splitContainerBody.Name = "splitContainerBody";
            this.splitContainerBody.Panel1.Controls.Add(this.gpgPanelChatAndInput);
            base.ttDefault.SetSuperTip(this.splitContainerBody.Panel1, null);
            this.splitContainerBody.Panel2.Controls.Add(this.gpgPanelGathering);
            this.splitContainerBody.Panel2.Controls.Add(this.gpgPanelClan);
            this.splitContainerBody.Panel2.Controls.Add(this.gpgPanelFriends);
            base.ttDefault.SetSuperTip(this.splitContainerBody.Panel2, null);
            this.splitContainerBody.Size = new Size(0x3d1, 0x1cb);
            this.splitContainerBody.SplitterDistance = 0x2c1;
            this.splitContainerBody.SplitterWidth = 6;
            base.ttDefault.SetSuperTip(this.splitContainerBody, null);
            this.splitContainerBody.TabIndex = 20;
            this.splitContainerBody.SplitterMoved += new SplitterEventHandler(this.splitContainerBody_SplitterMoved);
            this.gpgPanelClan.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgPanelClan.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelClan.BorderThickness = 2;
            this.gpgPanelClan.Controls.Add(this.splitContainerClan);
            this.gpgPanelClan.Controls.Add(this.skinLabelClanName);
            this.gpgPanelClan.Dock = DockStyle.Fill;
            this.gpgPanelClan.DrawBorder = true;
            this.gpgPanelClan.Location = new Point(0, 0);
            this.gpgPanelClan.Name = "gpgPanelClan";
            this.gpgPanelClan.Size = new Size(0x10a, 0x1cb);
            base.ttDefault.SetSuperTip(this.gpgPanelClan, null);
            this.gpgPanelClan.TabIndex = 0x15;
            this.gpgPanelFriends.BackColor = System.Drawing.Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgPanelFriends.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelFriends.BorderThickness = 2;
            this.gpgPanelFriends.Controls.Add(this.splitContainerFriends);
            this.gpgPanelFriends.Dock = DockStyle.Fill;
            this.gpgPanelFriends.DrawBorder = true;
            this.gpgPanelFriends.Location = new Point(0, 0);
            this.gpgPanelFriends.Name = "gpgPanelFriends";
            this.gpgPanelFriends.Size = new Size(0x10a, 0x1cb);
            base.ttDefault.SetSuperTip(this.gpgPanelFriends, null);
            this.gpgPanelFriends.TabIndex = 0x15;
            this.gpgPanel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanel2.BackgroundImage = (Image) manager.GetObject("gpgPanel2.BackgroundImage");
            this.gpgPanel2.BorderColor = System.Drawing.Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel2.BorderThickness = 2;
            this.gpgPanel2.Controls.Add(this.tabChatroom);
            this.gpgPanel2.Controls.Add(this.pManualTabs);
            this.gpgPanel2.DrawBorder = false;
            this.gpgPanel2.Location = new Point(5, 0xb9);
            this.gpgPanel2.Name = "gpgPanel2";
            this.gpgPanel2.Size = new Size(990, 0x30);
            base.ttDefault.SetSuperTip(this.gpgPanel2, null);
            this.gpgPanel2.TabIndex = 0x15;
            this.tabChatroom.AutoStyle = true;
            this.tabChatroom.BackColor = System.Drawing.Color.Black;
            this.tabChatroom.ButtonState = 0;
            this.tabChatroom.DialogResult = DialogResult.OK;
            this.tabChatroom.DisabledForecolor = System.Drawing.Color.Gray;
            this.tabChatroom.DrawColor = System.Drawing.Color.White;
            this.tabChatroom.DrawEdges = true;
            this.tabChatroom.FocusColor = System.Drawing.Color.Yellow;
            this.tabChatroom.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.tabChatroom.ForeColor = System.Drawing.Color.White;
            this.tabChatroom.HorizontalScalingMode = ScalingModes.Tile;
            this.tabChatroom.IsStyled = true;
            this.tabChatroom.Location = new Point(4, 0x15);
            this.tabChatroom.Name = "tabChatroom";
            this.tabChatroom.Size = new Size(0x89, 0x1c);
            this.tabChatroom.SkinBasePath = @"Dialog\ContentManager\TabLargeActive";
            base.ttDefault.SetSuperTip(this.tabChatroom, null);
            this.tabChatroom.TabIndex = 0x16;
            this.tabChatroom.TabStop = true;
            this.tabChatroom.TextAlign = ContentAlignment.MiddleLeft;
            this.tabChatroom.TextPadding = new Padding(6, 0, 0, 0);
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(0x3e8, 750);
            base.Controls.Add(this.gpgPanel2);
            base.Controls.Add(this.splitContainerBody);
            base.Controls.Add(this.pbMiddle);
            base.Controls.Add(this.wbMain);
            base.Controls.Add(this.pcTextEntry);
            base.Controls.Add(this.pBottom);
            base.Controls.Add(this.pbLeftBorder);
            base.Controls.Add(this.pbRightBorder);
            base.Controls.Add(this.pTop);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MainMenuStrip = this.msMainMenu;
            this.MinimumSize = new Size(640, 500);
            base.Name = "FrmMain";
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>GPGnet: Supreme Commander";
            base.MouseUp += new MouseEventHandler(this.OnMouseUp);
            base.VisibleChanged += new EventHandler(this.FrmMain_VisibleChanged);
            base.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.repositoryItemPictureEdit1.EndInit();
            this.repositoryItemMemoEdit1.EndInit();
            this.pBottom.ResumeLayout(false);
            this.pBottom.PerformLayout();
            this.msQuickButtons.ResumeLayout(false);
            this.msQuickButtons.PerformLayout();
            ((ISupportInitialize) this.pbBottomRight).EndInit();
            ((ISupportInitialize) this.pbBottom).EndInit();
            ((ISupportInitialize) this.pbBottomLeft).EndInit();
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            this.msMainMenu.ResumeLayout(false);
            this.msMainMenu.PerformLayout();
            ((ISupportInitialize) this.pbClose).EndInit();
            ((ISupportInitialize) this.pbRestore).EndInit();
            ((ISupportInitialize) this.pbMinimize).EndInit();
            ((ISupportInitialize) this.pbTopRight).EndInit();
            ((ISupportInitialize) this.pbTop).EndInit();
            ((ISupportInitialize) this.pbTopLeft).EndInit();
            this.splitContainerFriends.Panel1.ResumeLayout(false);
            this.splitContainerFriends.Panel1.PerformLayout();
            this.splitContainerFriends.Panel2.ResumeLayout(false);
            this.splitContainerFriends.ResumeLayout(false);
            this.gpgScrollPanelFriends.ResumeLayout(false);
            this.splitContainerClan.Panel1.ResumeLayout(false);
            this.splitContainerClan.Panel1.PerformLayout();
            this.splitContainerClan.Panel2.ResumeLayout(false);
            this.splitContainerClan.ResumeLayout(false);
            this.gpgScrollPanelClan.ResumeLayout(false);
            this.gpgScrollPanelNoClan.ResumeLayout(false);
            this.gpgScrollPanelNoClan.PerformLayout();
            ((ISupportInitialize) this.pbLeftBorder).EndInit();
            ((ISupportInitialize) this.pbRightBorder).EndInit();
            this.rimPictureEdit.EndInit();
            this.rimMemoEdit.EndInit();
            this.rimPictureEdit2.EndInit();
            this.rimMemoEdit2.EndInit();
            this.gpgChatGrid.EndInit();
            this.gvChat.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimMemoEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.pcTextEntry.EndInit();
            this.textBoxMsg.Properties.EndInit();
            this.dockManager.EndInit();
            this.pManualTabs.ResumeLayout(false);
            this.gpgPanelChatAndInput.ResumeLayout(false);
            this.splitContainerChatAndInput.Panel1.ResumeLayout(false);
            this.splitContainerChatAndInput.Panel1.PerformLayout();
            this.splitContainerChatAndInput.Panel2.ResumeLayout(false);
            this.splitContainerChatAndInput.ResumeLayout(false);
            this.gpgPanelGathering.ResumeLayout(false);
            this.gpgPanelGatheringDropDown.ResumeLayout(false);
            this.splitContainerBody.Panel1.ResumeLayout(false);
            this.splitContainerBody.Panel2.ResumeLayout(false);
            this.splitContainerBody.ResumeLayout(false);
            this.gpgPanelClan.ResumeLayout(false);
            this.gpgPanelFriends.ResumeLayout(false);
            this.gpgPanel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void InitializePlugins()
        {
            Plugins.LoadPlugins();
            foreach (PluginInfo info in Plugins.GetPlugins())
            {
                if ((info.Location == PluginLocation.Both) || (info.Location == PluginLocation.MainMenu))
                {
                    string menuCaption = info.MenuCaption;
                    if (menuCaption.IndexOf(",") < 0)
                    {
                        menuCaption = "Plugins," + menuCaption;
                    }
                    string[] strArray = menuCaption.Split(new char[] { ',' });
                    ToolStripItemCollection items = base.MainMenuStrip.Items;
                    ToolStripMenuItem item = null;
                    for (int i = 0; i < (strArray.Length - 1); i++)
                    {
                        ToolStripMenuItem current;
                        string str2 = strArray[i];
                        bool flag = false;
                        using (IEnumerator<object> enumerator2 = (IEnumerator<object>)items.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                current = (ToolStripMenuItem) enumerator2.Current;
                                if (current.Text.ToUpper() == str2.ToUpper())
                                {
                                    flag = true;
                                    items = current.DropDown.Items;
                                    current.Visible = true;
                                    item = current;
                                    goto Label_012E;
                                }
                            }
                        }
                    Label_012E:
                        if (!flag)
                        {
                            current = new ToolStripMenuItem(str2);
                            current.Size = new Size(0x110, 20);
                            if (item == null)
                            {
                                current.TextAlign = ContentAlignment.BottomCenter;
                                base.MainMenuStrip.Items.Add(current);
                            }
                            else
                            {
                                item.DropDown.Items.Add(current);
                            }
                            item = current;
                        }
                    }
                    string text = strArray[strArray.Length - 1];
                    ToolStripMenuItem item3 = new ToolStripMenuItem(text);
                    item3.Size = new Size(0x110, 0x16);
                    item3.Tag = info;
                    item3.Click += new EventHandler(this.MenuPluginItem);
                    item.DropDown.Items.Add(item3);
                }
            }
        }

        private void InitStatusDropdown()
        {
            EventHandler handler = null;
            SkinMenuItem item = new SkinMenuItem(Loc.Get("<LOC>Online"), StatusIcons.online);
            item.Click += delegate (object s, EventArgs e) {
                this.SetAwayStatus(false);
                this.SetDNDStatus(false);
                this.UpdateOnlineStatus(true);
            };
            item.Tag = 0;
            this.skinDropDownStatus.Menu.MenuItems.Add(item);
            SkinMenuItem item2 = new SkinMenuItem(Loc.Get("<LOC>Away"), StatusIcons.idle);
            item2.Click += delegate (object s, EventArgs e) {
                this.skinDropDownStatus.Text = "";
                this.SetAwayStatus(true);
            };
            item2.Tag = 0;
            this.skinDropDownStatus.Menu.MenuItems.Add(item2);
            SkinMenuItem item3 = new SkinMenuItem(Loc.Get("<LOC>Do Not Disturb"), StatusIcons.dnd);
            item3.Click += delegate (object s, EventArgs e) {
                this.skinDropDownStatus.Text = "";
                this.SetDNDStatus(true);
            };
            item3.Tag = 0;
            this.skinDropDownStatus.Menu.MenuItems.Add(item3);
            if (User.Current.IsAdmin)
            {
                SkinMenuItem item4 = new SkinMenuItem(Loc.Get("<LOC>Offline"), StatusIcons.offline);
                if (handler == null)
                {
                    handler = delegate (object s, EventArgs e) {
                        this.ChangeStatus("<LOC>Offline", StatusIcons.offline);
                        this.UpdateOnlineStatus(false);
                    };
                }
                item4.Click += handler;
                item4.Tag = 0;
                this.skinDropDownStatus.Menu.MenuItems.Add(item4);
            }
            SkinMenuItem item5 = new SkinMenuItem(Loc.Get("<LOC>Cancel"), StatusIcons.cancel);
            item5.Click += delegate (object s, EventArgs e) {
                if (this.mSupcomGameManager != null)
                {
                    this.mSupcomGameManager.AbortGame();
                    this.mSupcomGameManager = null;
                }
                else
                {
                    this.CancelRankedGame();
                }
            };
            item5.Tag = 1;
            item5.Visible = false;
            this.skinDropDownStatus.Menu.MenuItems.Add(item5);
            this.ChangeStatus("<LOC>Online", StatusIcons.online);
        }

        internal void InviteAsFriend(string target)
        {
            if (target.ToLower() == User.Current.Name.ToLower())
            {
                this.SystemMessage("<LOC>You cannot send yourself an invitation.", new object[0]);
            }
            else if (User.CurrentFriends.ContainsIndex("name", target))
            {
                this.SystemMessage("<LOC>This user is already on your Friends list.", new object[0]);
            }
            else
            {
                User user;
                if (!Chatroom.GatheringParticipants.TryFindByIndex("name", target, out user))
                {
                    MappedObjectList<User> objects = DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { target });
                    if (objects.Count == 0)
                    {
                        this.SystemMessage("<LOC>No record of this user. Please check the name and try again.", new object[0]);
                        return;
                    }
                    user = objects[0];
                }
                if (user.IsFriend)
                {
                    this.SystemMessage("<LOC>Target is already a friend.", new object[0]);
                }
                else if (user.Online)
                {
                    Messaging.SendCustomCommand(user.Name, CustomCommands.FriendInvite, new object[0]);
                    this.SystemMessage("<LOC>Invitation sent.", new object[0]);
                }
                else if (!DataAccess.ExecuteQuery("AddFriendRequest", new object[] { User.Current.ID, user.ID }))
                {
                    this.SystemMessage("<LOC>User is currently unavailable and an offline invitation was unable to be sent. Ensure you do not already have a pending request.", new object[0]);
                }
                else
                {
                    this.SystemMessage("<LOC>Invitation sent. The user is currently offline, but can review your invitation the next time he is online.", new object[0]);
                }
            }
        }

        internal bool InviteToClan(string target)
        {
            User user;
            if ((Clan.Current == null) || (ClanMember.Current == null))
            {
                this.ErrorMessage("<LOC>You are not in a clan.", new object[0]);
                return false;
            }
            if (!Chatroom.GatheringParticipants.TryFindByIndex("name", target, out user) && !User.CurrentFriends.TryFindByIndex("name", target, out user))
            {
                MappedObjectList<User> objects = DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { target });
                if (objects.Count > 0)
                {
                    user = objects[0];
                }
            }
            if (user == null)
            {
                this.ErrorMessage("<LOC>Unable to locate user {0}", new object[] { target });
                return false;
            }
            if (user.IsInClan)
            {
                this.ErrorMessage("<LOC>Could not send invitation. This player is already in a clan.", new object[0]);
                return false;
            }
            if (user.Online)
            {
                Messaging.SendCustomCommand(target, CustomCommands.InviteToClan, new object[] { Clan.Current.ID, Clan.Current.Name, Clan.Current.Abbreviation });
                this.SystemMessage("<LOC>Invitation sent.", new object[0]);
                return true;
            }
            if (DataAccess.ExecuteQuery("MakeOfflineClanInvite", new object[] { user.ID, Clan.Current.ID }))
            {
                this.SystemMessage("<LOC>Invitation sent. The user is currently offline, but can review your invitation the next time he is online.", new object[0]);
                return true;
            }
            this.ErrorMessage("<LOC>Unable to send invitation at this time. The user is currently offline.", new object[0]);
            return false;
        }

        internal void InviteToTeamGame(string name)
        {
            this.ShowDlgTeamGame();
            this.DlgTeamGame.OnInviteMember(name);
        }

        private bool IsFriendOrClanmate(string senderName)
        {
            try
            {
                if (User.CurrentFriends.ContainsIndex("name", senderName))
                {
                    return true;
                }
                if (Clan.CurrentMembers.ContainsIndex("name", senderName))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool IsFullScreenAppRunning()
        {
            return (GetRunningGames().Length > 0);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        private bool IsRegularRankedGame()
        {
            return ((ConfigSettings.GetString("Automatch GameIDs", "27") + " ").IndexOf(GameInformation.SelectedGame.GameID.ToString() + " ") >= 0);
        }

        public bool IsSpeaking(User user)
        {
            return this.IsSpeaking(user.Name);
        }

        public bool IsSpeaking(string userName)
        {
            return this.SpeakingTimers.ContainsKey(userName);
        }

        public bool IsUrlEmbedded(string url)
        {
            if (this.WebURL != null)
            {
                foreach (URLInfo info in this.WebURL)
                {
                    if (info.Url.ToUpper() == url.ToUpper())
                    {
                        return true;
                    }
                }
                return (url == this.mNotFound);
            }
            return true;
        }

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr hWnd);
        private void item_Click(object sender, EventArgs e)
        {
            GameInformation selectedGame = GameInformation.SelectedGame;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                GameInformation tag = item.Tag as GameInformation;
                if (tag != null)
                {
                    GameInformation.SelectedGame = tag;
                    this.msQuickButtons.Refresh();
                    if (((User.Current.IsAdmin || (GameInformation.SelectedGame.GameID == -1)) || (GameInformation.SelectedGame.CDKey != "")) || (new DlgNoBetaKey(this).ShowDialog() == DialogResult.OK))
                    {
                        this.UpdateSupCom();
                    }
                    else
                    {
                        GameInformation.SelectedGame = selectedGame;
                    }
                }
            }
        }

        private void item_MouseHover(object sender, EventArgs e)
        {
        }

        private void item_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void item_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                ToolStripItem item = sender as ToolStripItem;
                e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
                e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(this.msQuickButtons.BackgroundImage.Width, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
                int y = 10;
                if (item.Image.Height == 0x2d)
                {
                    y = 6;
                }
                GameInformation tag = item.Tag as GameInformation;
                if (item.Enabled)
                {
                    if (tag == GameInformation.SelectedGame)
                    {
                        Brush brush = new SolidBrush(System.Drawing.Color.White);
                        e.Graphics.FillRectangle(brush, new Rectangle(1, y - 1, item.Image.Width + 1, item.Image.Height + 1));
                        brush.Dispose();
                        e.Graphics.DrawImage(DrawUtil.AdjustColors(0.5f, 0.5f, 1f, item.Image), new Rectangle(2, y, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                        Pen pen = new Pen(Program.Settings.StylePreferences.MasterBackColor, 1f);
                        e.Graphics.DrawRectangle(pen, new Rectangle(1, y - 1, item.Image.Width + 1, item.Image.Height + 1));
                        pen.Dispose();
                    }
                    else if (item.Selected)
                    {
                        e.Graphics.DrawImage(DrawUtil.AdjustColors(0.8f, 0.8f, 1f, item.Image), new Rectangle(2, y, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        e.Graphics.DrawImage(item.Image, new Rectangle(2, y, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                    }
                }
                else
                {
                    e.Graphics.DrawImage(DrawUtil.GetTransparentImage(0.5f, item.Image), new Rectangle(2, y, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void JoinChat(string chatroom)
        {
            SupComGameManager.sCanCheckConnections = false;
            GPG.Logging.EventLog.WriteLine("Joining chatroom: " + chatroom, new object[] { chatroom });
            GPG.Logging.EventLog.DoStackTrace();
            ThreadQueue.QueueUserWorkItem(delegate (object none) {
                VGen0 method = null;
                try
                {
                    if (!this.mRanInitializeChat)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.CloseHomePage(chatroom);
                            };
                        }
                        this.Invoke(method);
                    }
                    else if (this.CanJoinChannel(chatroom))
                    {
                        string newchat = chatroom;
                        this.Invoke((VGen0)delegate {
                            this.skinGatheringDisplayChat.Enabled = false;
                            if ((chatroom == null) || (chatroom.Trim() == ""))
                            {
                                newchat = this.MainChatroom;
                            }
                            this.GatheringDisplaycontrol.GatheringSelected -= new StringEventHandler(this.JoinChat);
                            Chatroom.LeftChat -= new StringEventHandler(this.OnLeaveChat);
                            this.BetweenChannels = true;
                            this.ChatSyncTimer.Stop();
                            this.GatheringDisplaycontrol.CurrentRoom = new Chatroom(newchat, -1);
                            this.SystemMessage("<LOC>Joining {0}...", new object[] { newchat });
                        });
                        ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Join", this, "OnJoinChat", new object[] { newchat });
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }, new object[0]);
        }

        internal void JoinChatSynchronous(string chatroom)
        {
            try
            {
                if (this.CanJoinChannel(chatroom))
                {
                    string str2;
                    string name = chatroom;
                    if ((chatroom == null) || (chatroom.Trim() == ""))
                    {
                        name = this.MainChatroom;
                    }
                    this.GatheringDisplaycontrol.GatheringSelected -= new StringEventHandler(this.JoinChat);
                    Chatroom.LeftChat -= new StringEventHandler(this.OnLeaveChat);
                    this.BetweenChannels = true;
                    this.ChatSyncTimer.Stop();
                    this.GatheringDisplaycontrol.CurrentRoom = new Chatroom(name, -1);
                    this.SystemMessage("<LOC>Joining {0}...", new object[] { name });
                    bool result = Chatroom.Join(name, out str2);
                    this.OnJoinChat(result, str2);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void JoinClan(int clanId, string clanName, string clanAbbr)
        {
            WaitCallback callBack = null;
            try
            {
                if ((((Clan.Current == null) && (ClanMember.Current == null)) && (User.Current.ClanName == null)) && (User.Current.ClanAbbreviation == null))
                {
                    if (!DataAccess.ExecuteQuery("JoinClan", new object[] { User.Current.ID, clanId, ClanRanking.MinValue.Seniority }))
                    {
                        this.ErrorMessage("<LOC>Unable to join the clan {0}\"{1}\", ensure you are not already a member of a clan.", new object[] { "clan:", clanName });
                    }
                    else
                    {
                        this.ClanInvites.Clear();
                        DataAccess.ExecuteQuery("ClearClanInvites", new object[0]);
                        this.RefreshClanInviteCount();
                        this.RefreshClan(string.Format(Loc.Get("<LOC>You have joined the clan {0}\"{1}\"."), "clan:", clanName), "<LOC>{0} has joined your clan.", new object[] { User.Current.Name });
                        DataAccess.ExecuteQuery("ClearClanInvites", new object[0]);
                        if (callBack == null)
                        {
                            callBack = delegate (object state) {
                                try
                                {
                                    MappedObjectList<Chatroom> persistentRooms = null;
                                    if (ConfigSettings.GetBool("DoOldGameList", false))
                                    {
                                        persistentRooms = new QuazalQuery("GetPersistentRooms", new object[0]).GetObjects<Chatroom>();
                                    }
                                    else
                                    {
                                        persistentRooms = new QuazalQuery("GetPersistentRooms2", new object[] { GameInformation.SelectedGame.GameID }).GetObjects<Chatroom>();
                                    }
                                    base.Invoke((VGen0)delegate {
                                        this.GatheringDisplaycontrol.RefreshGatherings(persistentRooms, true);
                                    });
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            };
                        }
                        ThreadPool.QueueUserWorkItem(callBack);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void JoinGame()
        {
            Game.UpdateIP();
            if (GameInformation.SelectedGame.IsSpaceSiege)
            {
                new DlgJoinGame().ShowDialog();
            }
            else
            {
                this.ShowDlgSelectGame();
            }
        }

        public void JoinGame(string gamename)
        {
            this.JoinGame(gamename, "");
        }

        public void JoinGame(string gamename, string password)
        {
            int num = 0;
            while (num < 3)
            {
                OGen0 method = null;
                GameItem game;
                GPG.Logging.EventLog.WriteLine("Attempting to locate game {0} with password {1}", new object[] { gamename, password });
                if (DataAccess.TryGetObject<GameItem>("GetGameByName2", out game, new object[] { gamename, GameInformation.SelectedGame.GameID }))
                {
                    if (((game.Password != null) && (game.Password.Length > 0)) && (game.Password != password))
                    {
                        GPG.Logging.EventLog.WriteLine("Game is password protected, prompting for password", new object[0]);
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    if (game.Password != DlgAskQuestion.AskQuestion(this, Loc.Get("<LOC>What is the password?"), true))
                                    {
                                        DlgMessage.ShowDialog(Loc.Get("<LOC>That password is not correct."));
                                        return false;
                                    }
                                    return true;
                                };
                            }
                            if (!((bool) base.Invoke(method)))
                            {
                                break;
                            }
                        }
                        else if ((!base.Disposing && !base.IsDisposed) && (game.Password != DlgAskQuestion.AskQuestion(this, Loc.Get("<LOC>What is the password?"), true)))
                        {
                            DlgMessage.ShowDialog(Loc.Get("<LOC>That password is not correct."));
                            break;
                        }
                    }
                    GPG.Logging.EventLog.WriteLine("Located game {0}, attempting join", new object[] { gamename });
                    this.BeforeGameChatroom = Chatroom.CurrentName;
                    this.mGameName = game.Description;
                    this.mGameURL = game.URL;
                    this.mHostName = game.PlayerName;
                    ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Leave", this, "OnJoinGame", new object[0]);
                    int num2 = 0;
                    while (Chatroom.InChatroom)
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                        num2++;
                        if (num2 > 300)
                        {
                            break;
                        }
                    }
                    this.SetStatusButtons(1);
                    break;
                }
                ErrorLog.WriteLine("Failed to locate game {0} with password {1}, retrying...", new object[] { gamename, password });
                num++;
                Thread.Sleep(0x3e8);
            }
        }

        private void LadderMenuItem_Click(object sender, EventArgs e)
        {
            FormClosedEventHandler handler = null;
            LadderInstance ladder = (LadderInstance) (sender as ToolStripMenuItem).Tag;
            if (!this.LadderViews.ContainsKey(ladder.ID))
            {
                this.LadderViews[ladder.ID] = new FrmLadderView(ladder);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e1) {
                        this.LadderViews.Remove(ladder.ID);
                    };
                }
                this.LadderViews[ladder.ID].FormClosed += handler;
                this.LadderViews[ladder.ID].Show();
            }
            else
            {
                this.LadderViews[ladder.ID].BringToFront();
            }
        }

        private void LayoutTabs()
        {
            Control[] controlArray = new Control[] { this.btnClanTab, this.btnFriendsTab, this.btnChatTab };
            foreach (Control control in controlArray)
            {
                control.BringToFront();
            }
            this.SelectedTab.BringToFront();
        }

        internal void LeaveChat()
        {
            this.LeaveChat(false);
        }

        internal void LeaveChat(bool silent)
        {
            VGen0 method = null;
            VGen0 target = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.LeaveChat(silent);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    Chatroom.LeftChat -= new StringEventHandler(this.OnLeaveChat);
                    if (Chatroom.Leave())
                    {
                        this.ChatSyncTimer.Stop();
                        this.ChatroomQueue.Clear();
                        if (!silent)
                        {
                            this.SystemMessage("<LOC>You are no longer in a chat room.", new object[0]);
                        }
                        this.ChatroomQueue.WaitUntilEmpty(0x7d0);
                        if (target == null)
                        {
                            target = delegate {
                                this.ClearParticipants();
                            };
                        }
                        this.ChatroomQueue.Enqueue(target, new object[0]);
                        this.SpeakingTimers.Clear();
                        this.GatheringDisplaycontrol.CurrentRoom = Chatroom.None;
                        this.tabChatroom.Text = Loc.Get("<LOC>None");
                        this.ResetTitle();
                    }
                    else
                    {
                        ErrorLog.WriteLine("Error leaving chatroom {0}", new object[] { Chatroom.Current });
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                finally
                {
                    Chatroom.LeftChat += new StringEventHandler(this.OnLeaveChat);
                }
            }
        }

        private void lOC2v2RankingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowFrmStatsLadder("2v2");
        }

        private void lOC3v3RankingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowFrmStatsLadder("3v3");
        }

        private void lOC4v4RankingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowFrmStatsLadder("4v4");
        }

        protected override void Localize()
        {
            base.Localize();
            this.gpgContextMenuChat.Localize();
            this.gpgContextMenuChatText.Localize();
            this.gpgContextMenuEmote.Localize();
            this.msQuickButtons.Localize();
        }

        private void lOCArrangedTeamGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConfigSettings.GetBool("UseTeamLobby", false))
            {
                this.ShowDlgTeamGame();
            }
            else if (!DlgSupcomTeamSelection.sIsJoining)
            {
                DlgSupcomTeamSelection.ShowSelection();
            }
        }

        private DlgBase LocateDialog(System.Type type)
        {
            foreach (DlgBase base2 in this.ActiveDialogs)
            {
                if ((!base2.Disposing && !base2.IsDisposed) && (base2.GetType() == type))
                {
                    return base2;
                }
            }
            return null;
        }

        internal bool LocateExe(string gameName, bool block)
        {
            Exception exception;
            try
            {
                string fileName = gameName + ".exe";
                string str2 = "";
                try
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\THQ\Gas Powered Games\Supreme Commander");
                    if (key == null)
                    {
                        key = Registry.LocalMachine.OpenSubKey(@"Software\THQ\Gas Powered Games\Supreme Commander");
                    }
                    if (key != null)
                    {
                        str2 = (string) key.GetValue("InstallationDirectory");
                        str2 = str2 + @"\Supreme Commander\bin\";
                    }
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine("Unable to locate SupCom install path:\r\n", new object[0]);
                    ErrorLog.WriteLine(exception);
                }
                GPG.Logging.EventLog.WriteLine("Looking for registry path: {0}", new object[] { str2 });
                if (Program.Settings.SupcomPrefs.AutoDetectExe)
                {
                    string str3 = gameName;
                    if ((str3 != null) && (str3 == "SupremeCommander"))
                    {
                        string[] strArray = new string[] { str2, @"C:\Program Files\THQ\Gas Powered Games\Supreme Commander\bin\", @"D:\Program Files\THQ\Gas Powered Games\Supreme Commander\bin\", @"E:\Program Files\THQ\Gas Powered Games\Supreme Commander\bin\", @"C:\work\rts\main\purebuild\bin\", @"D:\work\rts\main\purebuild\bin\", @"C:\work\rts\main\cdimage\bin\", @"D:\work\rts\main\cdimage\bin\" };
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (System.IO.File.Exists(strArray[i] + fileName))
                            {
                                GameInformation.SelectedGame.GameLocation = strArray[i] + fileName;
                                return true;
                            }
                        }
                    }
                }
                if (block)
                {
                    return (new DlgLocateExe(this, fileName).ShowDialog() == DialogResult.OK);
                }
                new DlgLocateExe(this, fileName).Show(this);
                return false;
            }
            catch (Exception exception2)
            {
                exception = exception2;
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        internal void LocatePlayer(string name)
        {
            UserLocation location;
            if (DataAccess.TryGetObject<UserLocation>("LocatePlayer", out location, new object[] { name }))
            {
                this.SystemMessage("{0}", new object[] { location });
            }
            else
            {
                this.SystemMessage("<LOC>Unable to locate player {0}", new object[] { name });
            }
        }

        private void lOCClanRankingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowFrmStatsLadder("Clan");
        }

        private void lOCConnectionStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DlgConnectionsGraph().Show();
        }

        private void lOCCrashAppTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new ConstraintException("This is a test crash for bugsplat.");
        }

        private void lOCGetStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgStatsTest test = new DlgStatsTest();
            test.MainForm = this;
            test.Show();
        }

        private void lOCTestLeaveChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SystemMessage(Loc.Get("<LOC>You have been removed from chat for inactivity."), new object[0]);
            this.LeaveChat();
        }

        private void lOCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowWebPage(ConfigSettings.GetString("SSURL", "www.spacesiege.com"));
        }

        private void LogoutLobby(object s, EventArgs e)
        {
            Clan.Current = null;
            Clan.CurrentMembers = new MappedObjectList<ClanMember>();
            User.CurrentFriends = new MappedObjectList<User>();
        }

        [DllImport("kernel32", EntryPoint="lstrcpyA")]
        public static extern int lstrcpy([MarshalAs(UnmanagedType.LPTStr)] string dest, IntPtr src);
        [DllImport("kernel32", EntryPoint="lstrlenA")]
        public static extern int lstrlenByNum(IntPtr str);
        private void manager_OnExit(object sender, EventArgs e)
        {
            this.mIsInGame = false;
            this.DestroyGameLobby();
            this.SupcomGameExit();
        }

        private void manageServerGamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DlgGameVersionManager().Show();
        }

        private void mChangeEmail_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string str = DlgAskQuestion.AskQuestion(this, Loc.Get("<LOC>What is your new email address?"), Loc.Get("<LOC>Change Email"), false, out result);
            if (((result == DialogResult.OK) && (str.IndexOf("@") > 0)) && (str.IndexOf(".") > 0))
            {
                DataAccess.QueueExecuteQuery("Change Email Address", new object[] { str });
                DlgMessage.Show(this, Loc.Get("<LOC>Your email address has been updated."));
            }
            else
            {
                DlgMessage.Show(this, Loc.Get("<LOC>Your email address has not been changed."));
            }
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
        }

        private void menuItem15_Click(object sender, EventArgs e)
        {
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {
        }

        private void menuItem20_Click(object sender, EventArgs e)
        {
        }

        private void menuItem21_Click(object sender, EventArgs e)
        {
        }

        private void menuItem23_Click(object sender, EventArgs e)
        {
            this.Translate("DE");
        }

        private void menuItem24_Click(object sender, EventArgs e)
        {
            this.Translate("FR");
        }

        private void menuItem25_Click(object sender, EventArgs e)
        {
            this.Translate("IS");
        }

        private void menuItem26_Click(object sender, EventArgs e)
        {
            this.Translate("ES");
        }

        private void menuItem27_Click(object sender, EventArgs e)
        {
            this.Translate("RU");
        }

        private void menuItem28_Click(object sender, EventArgs e)
        {
            this.Translate("KR");
        }

        private void menuItem29_Click(object sender, EventArgs e)
        {
            this.Translate("JP");
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
        }

        private void MenuPluginItem(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            IFormPlugin plugin = (item.Tag as PluginInfo).CreatePlugin();
            UserControl control = plugin as UserControl;
            if (control != null)
            {
                DlgBase base2 = new DlgBase();
                base2.Show();
                base2.Width = control.Width + 40;
                base2.Height = control.Height + 100;
                base2.Text = plugin.FormTitle;
                control.Top = 50;
                control.Left = 20;
                base2.Controls.Add(control);
                control.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                control.Width = base2.Width - 40;
                control.Height = base2.Height - 100;
            }
        }

        private void Messaging_CommandRecieved(MessageEventArgs e)
        {
            if (!base.IsDisposed && !base.Disposing)
            {
                try
                {
                    MessageEventArgs args = e;
                    GPG.Logging.EventLog.WriteLine("Processing Command: {0}", new object[] { e.Message });
                    this.ProcessSystemCommand(args.Command, args.CommandArgs);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void Messaging_MessageRecieved(MessageEventArgs e)
        {
            VGen1 method = null;
            if ((!base.Disposing && !base.IsDisposed) && base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate (object objargs) {
                        this.DoMessaging_MessageRecieved(e);
                    };
                }
                base.BeginInvoke(method, new object[] { e });
            }
            else
            {
                this.DoMessaging_MessageRecieved(e);
            }
        }

        private static void Messaging_NetDataRecieved(Node sender, byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                Console.WriteLine("Inside Messaging_NetDataRecieved");
                GPG.Logging.EventLog.WriteLine("Recieved net data length: {0} from : {1}", new object[] { data.Length, sender });
                INetData data2 = new BinaryFormatter().Deserialize(stream) as INetData;
                if (data2 != null)
                {
                    data2.OnRecieve(sender);
                }
            }
        }

        private void miAdhocSQL_Click(object sender, EventArgs e)
        {
            new DlgAdhocSQL(this).Show();
        }

        private void miAdmin_Avatars_Click(object sender, EventArgs e)
        {
            new DlgAssignAvatars().Show();
        }

        private void miAdmin_CreateVolunteerEffort_Click(object sender, EventArgs e)
        {
            new DlgCreateVolunteerEffort().Show();
        }

        private void miAdmin_CustomGame_Click(object sender, EventArgs e)
        {
        }

        private void miAdmin_Security_Click(object sender, EventArgs e)
        {
            new DlgManageACLs().Show();
        }

        private void miAdmin_TestAwards_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                PlayerAward.ReCalculateAllAwards(this);
            });
        }

        private void miAdmin_TestSound_Click(object sender, EventArgs e)
        {
            this.ShowDlgTeamGame();
        }

        private void miAdmin_ViewVolunteers_Click(object sender, EventArgs e)
        {
            new DlgViewVolunteers().Show();
        }

        private void miAdming_AddPlayerAwards_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string str = DlgAskQuestion.AskQuestion(this, "What is the player's name?", "", false, out result);
            int @int = new QuazalQuery("GetPlayerIDFromName", new object[] { str }).GetInt();
            if (@int > 0)
            {
                new QuazalQuery("AddPlayerPendingAwards", new object[] { @int }).ExecuteNonQuery();
            }
        }

        private void miArrangedTeamsPopup_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (this.ciChat_TeamInvite.Enabled != DlgSupcomTeamSelection.sIsJoining)
            {
                this.ciChat_TeamInvite.Enabled = DlgSupcomTeamSelection.sIsJoining;
            }
        }

        private void miAutomatch_Click(object sender, EventArgs e)
        {
            this.AutomatchGame();
        }

        private void miClan_Invite_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.InviteToClan(this.SelectedChatParticipant.Name);
            }
            else
            {
                this.ErrorMessage("<LOC>No target selected.", new object[0]);
            }
        }

        private void miClan_RequestInvite_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant == null)
            {
                this.ErrorMessage("<LOC>No target selected.", new object[0]);
            }
            else if (!this.SelectedChatParticipant.IsInClan)
            {
                this.ErrorMessage("<LOC>Target is not in a clan.", new object[0]);
            }
            else
            {
                this.RequestClanInvite(this.SelectedChatParticipant.Name);
            }
        }

        private void miClan_ViewProfile_Click(object sender, EventArgs e)
        {
            this.OnViewClanProfileByPlayer(User.Current.Name);
        }

        private void miConsolidateAccounts_Click(object sender, EventArgs e)
        {
            if (GameInformation.SelectedGame.GameID <= 2)
            {
                DlgMessage.Show(this, Loc.Get("<LOC>You're game mode must be something other than Chat Only and Supreme Commander in order to consolidate keys."));
            }
            else
            {
                ThreadPool.QueueUserWorkItem(delegate (object o) {
                    VGen0 method = null;
                    bool flag = false;
                    DataList queryData = DataAccess.GetQueryData("Check For Other Keys", new object[0]);
                    foreach (DataRecord record in queryData)
                    {
                        if (record["gpgnet_game_id"] == "2")
                        {
                            bool doupdate = false;
                            string playername = record["name"];
                            base.Invoke((VGen0)delegate {
                                DlgYesNo no = new DlgYesNo(this, Loc.Get("<LOC>Consolidate Account"), Loc.Get("<LOC>Would you like to consolidate the CD key attached to this account to {0}?  Ratings and other information from this account will not transfer with your key, and this account will still exist.").Replace("{0}", playername));
                                if (no.ShowDialog() == DialogResult.Yes)
                                {
                                    doupdate = true;
                                }
                            });
                            flag = true;
                            if (doupdate)
                            {
                                DataAccess.ExecuteQuery("Consolidate CDKey", new object[] { playername });
                                base.Invoke((VGen0)delegate {
                                    GameInformation.LoadGamesFromDB();
                                    GameInformation.SelectedGame = GameInformation.GPGNetChat;
                                });
                                return;
                            }
                        }
                    }
                    if (!flag)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                DlgMessage.Show(this, Loc.Get("<LOC>We found no accounts to consolidate.  You must have matching emails to consolidate accounts.  You can change account emails under Main -> Change Email Address."));
                            };
                        }
                        base.Invoke(method);
                    }
                });
            }
        }

        private void miCreateTournament_Click(object sender, EventArgs e)
        {
            DlgTournamentCreation creation = new DlgTournamentCreation();
            creation.MainForm = this;
            creation.Show();
        }

        private void miCustomGame_Click(object sender, EventArgs e)
        {
            if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
            {
                DlgMessage.ShowDialog(Loc.Get("<LOC>You are searching for a ranked game and must cancel it first."));
            }
            else if (GameInformation.SelectedGame.IsSpaceSiege)
            {
                this.HostGameSpaceSiege();
            }
            else
            {
                this.HostGame();
            }
        }

        private void miEventLog_Click(object sender, EventArgs e)
        {
            DlgLogWatcher.ShowLogWindow();
        }

        private void miForums_Click(object sender, EventArgs e)
        {
            this.ShowWebPage(ConfigSettings.GetString("ForumURL", "forums.gaspowered.com"));
        }

        private void miFriends_InviteFriend_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.InviteAsFriend(this.SelectedChatParticipant.Name);
            }
        }

        private void miGame_RedeemPrize_Click(object sender, EventArgs e)
        {
            new DlgPrizeWinner().ShowDialog();
        }

        private void miGame_Vault_Click(object sender, EventArgs e)
        {
            this.ShowDlgSearchReplays();
        }

        private void miHelp_BetaForums_Click(object sender, EventArgs e)
        {
            this.ShowWebPage("http://www.forumplanet.com/SupremeCommander");
        }

        private void miHelp_GPGHome_Click(object sender, EventArgs e)
        {
            this.ShowWebPage("www.gaspowered.com");
        }

        private void miHelp_ReportBug_Click(object sender, EventArgs e)
        {
            this.ShowWebPage("http://www.fileplanet.com/promotions/supremecommander/bugsubmission.aspx");
        }

        private void miHelp_ReportIssue_Click(object sender, EventArgs e)
        {
            new DlgReportIssue().ShowDialog();
        }

        private void miHelp_Solutions_Click(object sender, EventArgs e)
        {
            this.ShowDlgKeywordSearch();
        }

        private void miHelp_SupComHome_Click(object sender, EventArgs e)
        {
            this.ShowWebPage("www.supremecommander.com");
        }

        private void miHelp_Volunteer_Click(object sender, EventArgs e)
        {
            this.ShowDlgActiveEfforts();
        }

        private void miJoinGame_Click(object sender, EventArgs e)
        {
            if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
            {
                DlgMessage.ShowDialog(Loc.Get("<LOC>You are searching for a ranked game and must cancel it first."));
            }
            else
            {
                this.JoinGame();
            }
        }

        private void miLadders_AcceptAll_Click(object sender, EventArgs e)
        {
            if (new QuazalQuery("AcceptAllLadderChallenges", new object[] { User.Current.ID }).ExecuteNonQuery())
            {
                foreach (FrmLadderView view in this.LadderViews.Values)
                {
                    if (view.IsLadderParticipant)
                    {
                        view.AcceptingChallenges = true;
                        view.RefreshChallengeToggle();
                    }
                }
            }
            else
            {
                ErrorLog.WriteLine("Error accepting all challenges.", new object[0]);
            }
        }

        private void miLadders_DeclineAll_Click(object sender, EventArgs e)
        {
            if (new QuazalQuery("DeclineAllLadderChallenges", new object[] { User.Current.ID }).ExecuteNonQuery())
            {
                foreach (FrmLadderView view in this.LadderViews.Values)
                {
                    view.AcceptingChallenges = false;
                    view.RefreshChallengeToggle();
                }
            }
            else
            {
                ErrorLog.WriteLine("Error declining all challenges.", new object[0]);
            }
        }

        private void miMain_Exit_Click(object sender, EventArgs e)
        {
            this.ExitApplication();
        }

        private void miMain_Logout_Click(object sender, EventArgs e)
        {
            Program.Settings.Login.IgnoreAutolaunch = true;
            Program.Settings.Save();
            Application.Restart();
        }

        private void miManageGames_Click(object sender, EventArgs e)
        {
            new DlgManageGames().ShowDialog();
            if (GameInformation.SelectedGame.GameLocation == "")
            {
                GameInformation.SelectedGame = GameInformation.GPGNetChat;
                this.msQuickButtons.Invalidate();
            }
            this.UpdateSupCom();
        }

        private void Minimize(object sender, EventArgs e)
        {
            this.MinimizeLoc = base.Location;
            this.MinimizeSize = base.Size;
            base.WindowState = FormWindowState.Minimized;
        }

        private void miPacketSniffer_Click(object sender, EventArgs e)
        {
            new DlgUDPSniffer().Show();
        }

        private void miRankings_1v1_Click(object sender, EventArgs e)
        {
            this.ShowFrmStatsLadder("1v1");
        }

        private void miShowColumns_Click(object sender, EventArgs e)
        {
            if (!this.miShowColumns.Checked)
            {
                this.gvChat.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(0x19, 0x19, 0x19);
                this.gvChat.OptionsView.ShowColumnHeaders = true;
            }
            else
            {
                this.gvChat.OptionsView.ShowColumnHeaders = false;
            }
            this.miShowColumns.Checked = !this.miShowColumns.Checked;
        }

        private void miSupcomStatsChart_Click(object sender, EventArgs e)
        {
            DlgSupcomStatsWatcher watcher = new DlgSupcomStatsWatcher();
            watcher.MainForm = this;
            watcher.Show();
        }

        private void miTools_Chat_Emotes_Click(object sender, EventArgs e)
        {
            this.ShowDlgEmotes();
        }

        private void miTools_ContentManager_Click(object sender, EventArgs e)
        {
            this.ShowDlgContentManager();
        }

        private void miTools_Feedback_Click(object sender, EventArgs e)
        {
            this.ShowFeedback();
        }

        private void miTools_GameKeys_Click(object sender, EventArgs e)
        {
            this.ShowDlgGameKeys();
        }

        private void miTools_LocPatches_Click(object sender, EventArgs e)
        {
            new DlgLocPatches().ShowDialog();
        }

        private void miTools_Options_Click(object sender, EventArgs e)
        {
            this.ShowDlgOptions();
        }

        private void miTournamentSchedule_Click(object sender, EventArgs e)
        {
            DlgTournamentRegistration registration = new DlgTournamentRegistration();
            registration.MainForm = this;
            registration.Show();
        }

        private void miViewReplays_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.ShowDlgSearchReplays(this.SelectedChatParticipant.Name);
            }
        }

        private void msQuickButtons_Click(object sender, EventArgs e)
        {
        }

        private void msQuickButtons_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void msQuickButtons_Paint(object sender, PaintEventArgs e)
        {
            if (this.ToolstripSizeChanged)
            {
                int num2;
                foreach (ToolStripItem item in this.btnMore.DropDownItems)
                {
                    item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                }
                int count = this.btnMore.DropDown.Items.Count;
                for (num2 = 0; num2 < count; num2++)
                {
                    this.msQuickButtons.Items.Insert(this.msQuickButtons.Items.Count - 1, this.btnMore.DropDown.Items[0]);
                }
                int num3 = 0;
                foreach (ToolStripItem item in this.msQuickButtons.Items)
                {
                    if (item.Visible)
                    {
                        if (!this.mCustomPaint.Contains(item))
                        {
                            this.mCustomPaint.Add(item);
                            item.Paint += new PaintEventHandler(this.item_Paint);
                        }
                        if (item != this.btnMore)
                        {
                            int num4 = ((item.Bounds.Right + this.btnMore.Width) + item.Padding.Right) + this.btnMore.Padding.Horizontal;
                            if (num4 > this.msQuickButtons.Width)
                            {
                                num3++;
                            }
                        }
                    }
                }
                if (num3 > 0)
                {
                    this.btnMore.Visible = true;
                    this.btnMore.DropDownDirection = ToolStripDropDownDirection.AboveRight;
                    this.btnMore.DropDown.AutoSize = false;
                    this.btnMore.DropDown.Width = this.btnVault.Width;
                    this.btnMore.DropDown.Height = (this.btnVault.Height * num3) + 3;
                    this.btnMore.DropDown.Padding = new Padding(0);
                    this.btnMore.DropDown.Margin = new Padding(0);
                    for (num2 = 0; num2 < num3; num2++)
                    {
                        this.btnMore.DropDownItems.Add(this.msQuickButtons.Items[(this.msQuickButtons.Items.Count - 1) - (num3 - num2)]);
                    }
                    foreach (ToolStripItem item in this.btnMore.DropDownItems)
                    {
                        item.BackgroundImage = this.btnMore.DropDown.BackgroundImage;
                    }
                }
                else if (this.ToolstripSizeChanged)
                {
                    this.btnMore.Visible = false;
                }
                this.ToolstripSizeChanged = false;
            }
        }

        private void msQuickButtons_SizeChanged(object sender, EventArgs e)
        {
            this.ToolstripSizeChanged = true;
        }

        private void mSupcomGameManager_OnAbortGame(string text)
        {
            try
            {
                this.SystemMessage(text, new object[0]);
                DlgMessage.ShowDialog(Loc.Get("<LOC id=ab2fb3167b572d1f2216992b723c4e67>Your game was canceled: ") + " \r\n" + Loc.Get(text));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void mWelcomePage_OnJoinChat(object sender, EventArgs e)
        {
            this.CloseHomePage();
        }

        private bool mWelcomePage_OnWebLinkClick(string link, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                if ((link != null) && (link.Length >= 1))
                {
                    string[] strArray = link.Replace("\"", "").Split("/".ToCharArray());
                    string name = strArray[strArray.Length - 1];
                    if ((name.IndexOf('.') < 0) && (name.Trim() != ""))
                    {
                        ToolStripMenuItem menuItem = this.GetMenuItem(this.msMainMenu.Items, name);
                        if ((menuItem != null) && menuItem.Enabled)
                        {
                            menuItem.PerformClick();
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return true;
            }
        }

        private void NormalGridRowExpandCollapse(object sender, CustomMasterRowEventArgs e)
        {
            this.CalcGridHeight(sender, 20);
        }

        private void NotifyOnClosing(object sender, EventArgs e)
        {
            this.NotifyOnLogout();
        }

        private void NotifyOnLogin()
        {
            Program.Closing -= new EventHandler(this.NotifyOnClosing);
            Program.Closing += new EventHandler(this.NotifyOnClosing);
            string[] allMessageList = this.AllMessageList;
            string text = "Notifying the following players of login: ";
            foreach (string str2 in allMessageList)
            {
                text = text + string.Format("{0}, ", str2);
            }
            text.TrimEnd(new char[] { ',', ' ' });
            GPG.Logging.EventLog.WriteLine(text, new object[0]);
            Messaging.SendCustomCommand(allMessageList, CustomCommands.OnlineStatus, new object[] { 1 });
        }

        private void NotifyOnLogout()
        {
            try
            {
                GameInformation.SaveGames();
                List<string> list = new List<string>();
                foreach (User user in User.CurrentFriends)
                {
                    if (user.Online)
                    {
                        list.Add(user.Name);
                    }
                }
                foreach (ClanMember member in Clan.CurrentMembers)
                {
                    if (!(!member.Online || list.Contains(member.Name)))
                    {
                        list.Add(member.Name);
                    }
                }
                foreach (string str in this.PMTargets)
                {
                    if (!list.Contains(str))
                    {
                        list.Add(str);
                    }
                }
                string text = "Notifying the following players of exit: ";
                foreach (string str3 in list)
                {
                    text = text + string.Format("{0}, ", str3);
                }
                text.TrimEnd(new char[] { ',', ' ' });
                GPG.Logging.EventLog.WriteLine(text, new object[0]);
                Messaging.SendCustomCommand(list.ToArray(), CustomCommands.OnlineStatus, new object[] { 0 });
                list = null;
                if (!ThreadQueue.Quazal.WaitUntilEmpty(0x7d0))
                {
                    ErrorLog.WriteLine("Operation timed out attempting to notify chat participants of exit", new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void OnAction()
        {
            this.LastAction = DateTime.Now;
            this.SetAwayStatus(false);
        }

        internal void OnAddFriend(string friendName, int friendId)
        {
            if (User.CurrentFriends.ContainsIndex("name", friendName))
            {
                this.ErrorMessage("<LOC>{0} is already on your Friends list.", new object[] { friendName });
            }
            else if (!(DataAccess.ExecuteQuery("AddFriend", new object[] { User.Current.ID, friendId }) && DataAccess.ExecuteQuery("AddFriend", new object[] { friendId, User.Current.ID })))
            {
                this.ErrorMessage("<LOC>Unable to add {0} as a friend at this time. Ensure they are not already a friend.", new object[] { friendName });
            }
            else
            {
                User user;
                if (User.CurrentFriends.Count > 0)
                {
                    user = null;
                    if (Chatroom.GatheringParticipants.TryFindByIndex("name", friendName, out user) || DataAccess.TryGetObject<User>("GetPlayerDetails", out user, new object[] { friendName }))
                    {
                        this.SystemMessage("<LOC>{0} is now on your Friends list.", new object[] { friendName });
                        user.IsFriend = true;
                        User.CurrentFriends.Add(user);
                        User.CurrentFriends.IndexObject(user);
                        this.AddFriendParticipant(user);
                        this.RefreshChatParticipant(user);
                        foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                        {
                            if ((!chat.Disposing && !chat.IsDisposed) && (chat.ChatTarget.Name.ToLower() == friendName.ToLower()))
                            {
                                chat.ChatTarget.IsFriend = true;
                                chat.RefreshToolstrip();
                            }
                        }
                    }
                    else
                    {
                        this.ErrorMessage("<LOC>Unable to add {0} to your Friends list at this time.", new object[] { friendName });
                    }
                }
                else
                {
                    user = null;
                    if (Chatroom.GatheringParticipants.TryFindByIndex("name", friendName, out user) || DataAccess.TryGetObject<User>("GetPlayerDetails", out user, new object[] { friendName }))
                    {
                        this.SystemMessage("<LOC>{0} is now your friend.", new object[] { friendName });
                        user.IsFriend = true;
                        User.CurrentFriends.Add(user);
                        User.CurrentFriends.IndexObject(user);
                        this.UpdateFriends(null);
                        this.RefreshChatParticipant(user);
                        foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                        {
                            if ((!chat.Disposing && !chat.IsDisposed) && (chat.ChatTarget.Name.ToLower() == friendName.ToLower()))
                            {
                                chat.ChatTarget.IsFriend = true;
                                chat.RefreshToolstrip();
                            }
                        }
                    }
                    else
                    {
                        this.ErrorMessage("<LOC>Unable to add {0} as a friend at this time.", new object[] { friendName });
                    }
                }
                Messaging.SendCustomCommand(friendName, CustomCommands.RefreshFriends, new object[] { "<LOC>{0} is now your friend.", User.Current.Name });
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.mShuttingDown = true;
            base.OnClosing(e);
        }

        private void OnCreateClan()
        {
            WaitCallback callBack = null;
            if (!User.Current.IsInClan)
            {
                if (new DlgCreateClan(this).ShowDialog(this) == DialogResult.OK)
                {
                    this.ClanInvites.Clear();
                    DataAccess.ExecuteQuery("ClearClanInvites", new object[0]);
                    this.RefreshClanInviteCount();
                    this.RefreshClan(string.Format(Loc.Get("<LOC>You have formed the clan {0}\"{1}\""), "clan:", User.Current.ClanName), null, new object[0]);
                    this.RefreshPMWindows();
                    this.RefreshGathering();
                    DataAccess.ExecuteQuery("ClearClanInvites", new object[0]);
                    if (callBack == null)
                    {
                        callBack = delegate (object state) {
                            MappedObjectList<Chatroom> persistentRooms = null;
                            if (ConfigSettings.GetBool("DoOldGameList", false))
                            {
                                persistentRooms = new QuazalQuery("GetPersistentRooms", new object[0]).GetObjects<Chatroom>();
                            }
                            else
                            {
                                persistentRooms = new QuazalQuery("GetPersistentRooms2", new object[] { GameInformation.SelectedGame.GameID }).GetObjects<Chatroom>();
                            }
                            base.Invoke((VGen0)delegate {
                                this.GatheringDisplaycontrol.RefreshGatherings(persistentRooms, true);
                            });
                        };
                    }
                    ThreadPool.QueueUserWorkItem(callBack);
                }
            }
            else
            {
                this.ErrorMessage("<LOC>You are already in a clan.", new object[0]);
            }
        }

        protected override void OnDeactivate(EventArgs e)
        {
            if (!this.StayActive)
            {
                base.OnDeactivate(e);
                Program.RefreshMemUsage();
            }
        }

        private void OnDisconnect()
        {
            VGen0 method = null;
            this.mConnected = false;
            this.LeaveChat();
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.btnFeedback.Enabled = false;
                        this.btnHostGame.Enabled = false;
                        this.btnJoinGame.Enabled = false;
                        this.btnRankedGame.Enabled = false;
                        this.btnArrangedTeam.Enabled = false;
                        this.btnPlayNow.Enabled = false;
                        this.btnViewRankings.Enabled = false;
                        this.miGameGroup.Enabled = false;
                        this.miRankings.Enabled = false;
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.btnFeedback.Enabled = false;
                this.btnHostGame.Enabled = false;
                this.btnJoinGame.Enabled = false;
                this.btnRankedGame.Enabled = false;
                this.btnArrangedTeam.Enabled = false;
                this.btnPlayNow.Enabled = false;
                this.btnViewRankings.Enabled = false;
                this.miGameGroup.Enabled = false;
                this.miRankings.Enabled = false;
            }
        }

        private void OnFrameChange(object s, EventArgs e)
        {
        }

        private void OnGetConfigs(DataList data)
        {
            Exception exception;
            if (!this.mFirstConfig)
            {
                ConfigSettings.LoadSettings(data);
            }
            GPG.Logging.EventLog.WriteLine("Data config settings have been loaded.", new object[0]);
            Emote.ValidateEmotes();
            if (this.mFirstConfig)
            {
                if (ConfigSettings.GetBool("ShowChatImage", true))
                {
                    this.colIcon.VisibleIndex = 0;
                }
                this.mFirstConfig = false;
            }
            try
            {
                string str = ConfigSettings.GetString("ProcessList", "");
                if (str != this.mLastProcessList)
                {
                    this.mLastProcessList = str;
                    foreach (string str2 in this.mLastProcessList.Split(new char[] { ';' }))
                    {
                        try
                        {
                            Process[] processesByName = Process.GetProcessesByName(str2);
                            if ((processesByName.Length > 0) && (new DlgYesNo(this, "<LOC>Warning", Loc.Get("<LOC>We have detected that you are running ") + str2 + Loc.Get("<LOC> and it may interfere with running Supreme Commander.  Would you like to try to close this process?")).ShowDialog() == DialogResult.Yes))
                            {
                                try
                                {
                                    for (int i = processesByName.Length - 1; i >= 0; i--)
                                    {
                                        processesByName[i].Kill();
                                    }
                                }
                                catch (Exception exception1)
                                {
                                    exception = exception1;
                                    DlgMessage.Show(this, Loc.Get("<LOC>Unable to shut down ") + str2 + Loc.Get(".  Please try to close it manually."));
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            ErrorLog.WriteLine(exception);
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                exception = exception3;
                ErrorLog.WriteLine(exception);
            }
            DateTime dateTime = ConfigSettings.GetDateTime("SHUTDOWN", DateTime.MinValue);
            if ((dateTime != DateTime.MinValue) && (DateTime.Now.ToUniversalTime() < dateTime))
            {
                this.mConfigSleepRate = ConfigSettings.GetInt("ShutdownRefreshRate", 0xea60);
                this.SystemMessage(Loc.Get("<LOC>GPGnet will be shutting down at {0}.", new object[] { dateTime.ToString() }), new object[0]);
            }
            foreach (string str3 in ConfigSettings.GetString("DisableMenus", "").Split(new char[] { ';' }))
            {
                try
                {
                    ToolStripMenuItem menuItem = this.GetMenuItem(this.msMainMenu.Items, str3);
                    if (menuItem != null)
                    {
                        menuItem.Visible = false;
                    }
                    else
                    {
                        menuItem = this.GetMenuItem(this.msQuickButtons.Items, str3);
                        if (menuItem != null)
                        {
                            menuItem.Visible = false;
                        }
                        else
                        {
                            foreach (MenuItem item2 in this.gpgContextMenuChat.MenuItems)
                            {
                                if ((item2.Text.ToUpper() == str3.ToUpper()) || (item2.Name.ToUpper() == str3.ToUpper()))
                                {
                                    item2.Visible = false;
                                    break;
                                }
                            }
                        }
                    }
                    this.miTools_Chat.Visible = ConfigSettings.GetBool("Emotes", true);
                }
                catch (Exception exception4)
                {
                    exception = exception4;
                    ErrorLog.WriteLine(exception);
                }
            }
            if (ConfigSettings.GetString("AuthorizedSQLAccounts", "admin,").ToLower().IndexOf(User.Current.Name.ToLower() + ",") >= 0)
            {
                this.miAdhocSQL.Visible = true;
            }
            if (this.wbMain.Height != ConfigSettings.GetInt("AddHeight", 0x48))
            {
                this.wbMain.Height = ConfigSettings.GetInt("AddHeight", 0x48);
                this.pbMiddle.Top = this.wbMain.Bottom;
            }
            if (this.mConfigThread == null)
            {
                this.mConfigThread = new Thread(new ThreadStart(this.CheckConfigs));
                this.mConfigThread.IsBackground = true;
                this.mConfigThread.Start();
            }
        }

        private void OnGetURLs(DataList data)
        {
            this.WebURL = new MappedObjectList<URLInfo>(data);
            if (this.mWelcomePage == null)
            {
                this.ShowWelcome();
            }
            this.mWelcomePage.MainForm = this;
            this.mWelcomePage.HomePage = this.HOME_URL;
            this.wbMain.Navigate(this.UrlById("BANNER"));
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                ParameterizedThreadStart start = new ParameterizedThreadStart(this.DownloadPatch);
                Thread thread = new Thread(start);
                thread.IsBackground = true;
                thread.Start(this.UrlById("PATCH"));
            }
            else
            {
                this.IsGPGNetPatching = false;
                this.UpdateSupCom();
            }
        }

        private void OnHostGame(bool result)
        {
            EventHandler handler = null;
            SupComGameManager.GameArgs = Program.Settings.SupcomPrefs.CommandLineArgs;
            if ((((GameInformation.SelectedGame.GameLocation != null) && (GameInformation.SelectedGame.GameLocation.Length > 0)) && System.IO.File.Exists(GameInformation.SelectedGame.GameLocation)) || this.LocateExe(GameInformation.SelectedGame.ExeName, true))
            {
                if (!(User.Current.IsAdmin || this.IsGameCurrent))
                {
                    this.UpdateSupCom();
                }
                else
                {
                    Chatroom.Leave();
                    SupComGameManager.LastLocation = GameInformation.SelectedGame.GameLocation;
                    this.ShowWelcome(this.UrlById("GAME"));
                    this.DisableControls();
                    this.mSupcomGameManager = new SupComGameManager();
                    this.mSupcomGameManager.SetPassword(this.mPassword);
                    this.mSupcomGameManager.OnAbortGame += new StringEventHandler(this.mSupcomGameManager_OnAbortGame);
                    if (handler == null)
                    {
                        handler = delegate (object s, EventArgs e) {
                            if (this.GameHosted != null)
                            {
                                this.GameHosted(this, EventArgs.Empty);
                            }
                            if (!GameInformation.SelectedGame.IsSpaceSiege)
                            {
                                Messaging.SendCustomCommand(this.FriendsMessageList, CustomCommands.GameEvent, new object[] { "<LOC>Your friend {0} has just hosted the game {1}\"{2}\"", User.Current.Name, " game:", this.mGameName });
                                Messaging.SendCustomCommand(this.UniqueClanMessageList, CustomCommands.GameEvent, new object[] { "<LOC>Your clan member {0} has just hosted the game {1}\"{2}\"", User.Current.Name, " game:", this.mGameName });
                            }
                        };
                    }
                    this.mSupcomGameManager.GameHosted += handler;
                    if (this.mSupcomGameManager.HostGame(false, this.mGameName))
                    {
                        this.mIsInGame = true;
                        this.ChangeStatus("<LOC>In Game", StatusIcons.in_game);
                        this.mSupcomGameManager.OnExit += new EventHandler(this.manager_OnExit);
                        base.KeyDown += new KeyEventHandler(this.GameKeyDown);
                    }
                }
            }
        }

        private void OnJoinChat(bool result, string error)
        {
            WaitCallback callBack = null;
            try
            {
                if (result)
                {
                    DateTime now = DateTime.Now;
                    while (this.IsSortingChatroom)
                    {
                        Thread.Sleep(20);
                        if ((DateTime.Now - now) > TimeSpan.FromSeconds(3.0))
                        {
                            break;
                        }
                    }
                    if (error != null)
                    {
                        this.SystemMessage(error, new object[0]);
                    }
                    else
                    {
                        this.SystemMessage("<LOC>You have joined {0}", new object[] { Chatroom.CurrentName });
                    }
                    this.ClearParticipants();
                    this.SpeakingTimers.Clear();
                    this.OnAction();
                    this.ResetTitle();
                    Messaging.SendCustomCommand(CustomCommands.EnterChannel, new object[] { User.Current.ToDataString() });
                    if (callBack == null)
                    {
                        callBack = delegate (object s) {
                            try
                            {
                                MappedObjectList<User> data = DataAccess.GetObjects<User>("GatheringPlayers", new object[] { Chatroom.CurrentName });
                                this.tabChatroom.Text = Chatroom.CurrentName;
                                this.ChatroomQueue.Suspend();
                                this.ChatroomQueue.Enqueue((VGen0)delegate {
                                    this.pnlUserListChat.AddUsers(data);
                                }, new object[0]);
                                this.ChatroomQueue.Resume();
                                this.GatheringDisplaycontrol.CurrentRoom = Chatroom.Current;
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                            finally
                            {
                                this.ChatSyncTimer.Start();
                                this.BetweenChannels = false;
                            }
                        };
                    }
                    ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                }
                else
                {
                    this.ErrorMessage(error, new object[0]);
                    if (!(Chatroom.InChatroom && !Chatroom.Current.Equals(Chatroom.None)))
                    {
                        this.GatheringDisplaycontrol.CurrentRoom = Chatroom.None;
                    }
                    else
                    {
                        this.GatheringDisplaycontrol.CurrentRoom = Chatroom.Current;
                        this.ResetTitle();
                    }
                }
                this.GatheringDisplaycontrol.GatheringSelected += new StringEventHandler(this.JoinChat);
                Chatroom.LeftChat += new StringEventHandler(this.OnLeaveChat);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                this.skinGatheringDisplayChat.Enabled = true;
            }
        }

        private void OnJoinGame(bool result)
        {
            base.KeyDown += new KeyEventHandler(this.GameKeyDown);
            SupComGameManager.GameArgs = Program.Settings.SupcomPrefs.CommandLineArgs;
            if ((((GameInformation.SelectedGame.GameLocation != null) && (GameInformation.SelectedGame.GameLocation.Length > 0)) && System.IO.File.Exists(GameInformation.SelectedGame.GameLocation)) || this.LocateExe(GameInformation.SelectedGame.ExeName, true))
            {
                if (!(User.Current.IsAdmin || this.IsGameCurrent))
                {
                    this.UpdateSupCom();
                }
                else
                {
                    GPG.Logging.EventLog.WriteLine("Entered OnJoinChat method", new object[0]);
                    Chatroom.Leave();
                    SupComGameManager.LastLocation = GameInformation.SelectedGame.GameLocation;
                    this.ShowWelcome(this.UrlById("GAME"));
                    this.DisableControls();
                    this.mSupcomGameManager = new SupComGameManager();
                    this.mSupcomGameManager.OnAbortGame += new StringEventHandler(this.mSupcomGameManager_OnAbortGame);
                    string[] strArray = this.mGameURL.Replace("udp:/", "").Split("=;".ToCharArray());
                    string address = strArray[1];
                    int port = Convert.ToInt32(strArray[3]);
                    if (this.mSupcomGameManager.JoinGame(false, this.mGameName, address, port, this.mHostName))
                    {
                        this.mIsInGame = true;
                        this.ChangeStatus("<LOC>In Game", StatusIcons.in_game);
                        this.mSupcomGameManager.OnExit += new EventHandler(this.manager_OnExit);
                        if (!GameInformation.SelectedGame.IsSpaceSiege)
                        {
                            Messaging.SendCustomCommand(this.FriendsMessageList, CustomCommands.GameEvent, new object[] { "<LOC>Your friend {0} has just joined the game {1}\"{2}\"", User.Current.Name, "game:", this.mGameName });
                            Messaging.SendCustomCommand(this.UniqueClanMessageList, CustomCommands.GameEvent, new object[] { "<LOC>Your clan member {0} has just joined the game {1}\"{2}\"", User.Current.Name, "game:", this.mGameName });
                        }
                    }
                    else
                    {
                        ErrorLog.WriteLine("Failed to join game {0}!", new object[] { this.mGameName });
                    }
                }
            }
        }

        private void OnLeaveChat(string room)
        {
            this.LeaveChat();
        }

        internal void OnLeaveClan()
        {
            WaitCallback callBack = null;
            try
            {
                if (Clan.Current != null)
                {
                    if (new DlgYesNo(this, "<LOC>Leave Clan", "<LOC>Are you sure you want to leave your clan?").ShowDialog(this) == DialogResult.Yes)
                    {
                        ClanMember member = null;
                        if ((ClanMember.Current != null) && (Clan.Current != null))
                        {
                            ClanMember current = ClanMember.Current;
                            DataAccess.ExecuteQuery("LeaveClan", new object[] { User.Current.ID, Clan.Current.ID });
                            if (Clan.CurrentMembers.Count == 1)
                            {
                                DataAccess.ExecuteQuery("RemoveClan", new object[] { Clan.Current.ID });
                                DataAccess.ExecuteQuery("ClearEmptyClanRooms", new object[0]);
                            }
                            else if (current.Rank == ClanRanking.MaxValue.Seniority)
                            {
                                ClanMember[] memberArray;
                                int rank = 1;
                                while (this.GetMembersByRank(rank, out memberArray) < 1)
                                {
                                    rank++;
                                    if (rank > ClanRanking.MinValue.Seniority)
                                    {
                                        DataAccess.ExecuteQuery("RemoveClan", new object[] { Clan.Current.ID });
                                        break;
                                    }
                                }
                                if (memberArray.Length > 1)
                                {
                                    Array.Sort<ClanMember>(memberArray, new SeniorityComparer());
                                    member = memberArray[0];
                                }
                                else if (memberArray.Length == 1)
                                {
                                    member = memberArray[0];
                                }
                            }
                            string[] currentMembersMsgList = Clan.CurrentMembersMsgList;
                            if (member != null)
                            {
                                DataAccess.ExecuteQuery("ChangeRank", new object[] { ClanRanking.MaxValue.Seniority, member.ID, Clan.Current.ID });
                                this.RefreshClan(Loc.Get("<LOC>You have left your clan."), "<LOC>{0} has left your clan. {1} has been promoted to {2}", new object[] { User.Current.Name, member.Name, ClanRanking.MaxValue.Description });
                            }
                            else
                            {
                                this.RefreshClan(Loc.Get("<LOC>You have left your clan."), "<LOC>{0} has left your clan.", new object[] { User.Current.Name });
                                this.RefreshPMWindows();
                            }
                            Clan.Current = null;
                            ClanMember.Current = null;
                            Clan.CurrentMembers.Clear();
                            User.Current.ClanAbbreviation = null;
                            User.Current.ClanName = null;
                            this.RefreshGathering();
                            DataAccess.ExecuteQuery("ClearClanRequests", new object[0]);
                            for (int i = 0; i < currentMembersMsgList.Length; i++)
                            {
                                this.RefreshChatParticipant(currentMembersMsgList[i]);
                            }
                            this.ClanRequests.Clear();
                            this.RefreshClanRequestCount();
                            if (callBack == null)
                            {
                                callBack = delegate (object state) {
                                    try
                                    {
                                        MappedObjectList<Chatroom> persistentRooms = null;
                                        if (ConfigSettings.GetBool("DoOldGameList", false))
                                        {
                                            persistentRooms = new QuazalQuery("GetPersistentRooms", new object[0]).GetObjects<Chatroom>();
                                        }
                                        else
                                        {
                                            persistentRooms = new QuazalQuery("GetPersistentRooms2", new object[] { GameInformation.SelectedGame.GameID }).GetObjects<Chatroom>();
                                        }
                                        base.Invoke((VGen0)delegate {
                                            this.GatheringDisplaycontrol.RefreshGatherings(persistentRooms, true);
                                        });
                                    }
                                    catch (Exception exception)
                                    {
                                        ErrorLog.WriteLine(exception);
                                    }
                                };
                            }
                            ThreadPool.QueueUserWorkItem(callBack);
                        }
                    }
                }
                else
                {
                    this.ErrorMessage("<LOC>You are not in a clan.", new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.Opacity = 1.0;
            base.OnLoad(e);
            this.tabChatroom.DrawColor = System.Drawing.Color.Black;
            this.btnChatTab.DrawColor = System.Drawing.Color.Black;
            this.AlignChatTabs();
            this.tabChatroom.MakeEdgesTransparent();
            this.btnChatTab.MakeEdgesTransparent();
            this.btnClanTab.MakeEdgesTransparent();
            this.btnFriendsTab.MakeEdgesTransparent();
            this.GatherLadderReports();
            if (User.Current.IsAdmin)
            {
                this.skinDropDownStatus.Enabled = true;
            }
            this.RefreshEmoteAnimations();
            Program.RefreshMemUsage();
            Speech.Rate = Program.Settings.Sound.Speech.SpeechRate;
            Speech.Voice = Program.Settings.Sound.Speech.Voice;
            Speech.Volume = Program.Settings.Sound.Speech.Volume;
            base.Size = Program.Settings.Appearance.StartSize;
            if (Program.Settings.Appearance.StartLocation != new Point(-32000, -32000))
            {
                base.Location = Program.Settings.Appearance.StartLocation;
            }
            Application.ApplicationExit += delegate (object s, EventArgs e1) {
                this.mShuttingDown = true;
                if (base.WindowState == FormWindowState.Minimized)
                {
                    Program.Settings.Appearance.StartLocation = this.MinimizeLoc;
                    Program.Settings.Appearance.StartSize = this.MinimizeSize;
                }
                else
                {
                    Program.Settings.Appearance.StartLocation = base.Location;
                    Program.Settings.Appearance.StartSize = base.Size;
                }
            };
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point point = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
            if (this.mIsMaximized)
            {
                if (this.Cursor != Cursors.Default)
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                if (this.mIsResizing || this.mStartResizing)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                else if ((e.Button == MouseButtons.None) && this.HitTest(point.X, point.Y, base.Width - this.RESIZE_RECT, base.Height - (this.RESIZE_RECT + 20), base.Width, base.Height))
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if ((this.Cursor == Cursors.SizeNWSE) || (this.Cursor == Cursors.SizeAll))
                {
                    this.Cursor = Cursors.Default;
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (this.mIsMoving)
                    {
                        base.Left = (Control.MousePosition.X - this.mLastX) - this.mWidthDiff;
                        base.Top = (Control.MousePosition.Y - this.mLastY) - this.mHeightDiff;
                    }
                    else if (this.mIsResizing || this.mStartResizing)
                    {
                        if (this.mIsResizing)
                        {
                            ControlPaint.DrawReversibleFrame(this.mResizeRect, System.Drawing.Color.Empty, FrameStyle.Thick);
                        }
                        this.mIsResizing = true;
                        this.mStartResizing = false;
                        this.mResizeRect.X = base.PointToScreen(new Point(0, 0)).X;
                        this.mResizeRect.Y = base.PointToScreen(new Point(0, 0)).Y;
                        this.mResizeRect.Width = this.mLastWidth + ((Control.MousePosition.X - this.mLastMouseX) - this.mWidthDiff);
                        this.mResizeRect.Height = this.mLastHeight + ((Control.MousePosition.Y - this.mLastMouseY) - this.mHeightDiff);
                        if (this.mResizeRect.Width < this.MinimumSize.Width)
                        {
                            this.mResizeRect.Width = this.MinimumSize.Width;
                        }
                        if (this.mResizeRect.Height < this.MinimumSize.Height)
                        {
                            this.mResizeRect.Height = this.MinimumSize.Height;
                        }
                        ControlPaint.DrawReversibleFrame(this.mResizeRect, System.Drawing.Color.Empty, FrameStyle.Thick);
                    }
                }
                else
                {
                    this.SizeToFit(point);
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!(base.IsDisposed || base.Disposing))
            {
                Point point = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                this.SizeToFit(point);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (User.LoggedIn)
            {
                if (!this.mIsResizing)
                {
                    base.OnPaint(e);
                }
            }
            else
            {
                base.Hide();
            }
        }

        private IntPtr OnPatchUpdate(int id, IntPtr parm)
        {
            string str;
            GPG.Logging.EventLog.WriteLine("Patch Update - ID: {0}, MSG: {1}", new object[] { id, parm });
            IntPtr ptr = Marshal.StringToHGlobalAnsi("");
            switch (id)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    str = Marshal.PtrToStringAnsi(parm);
                    this.SetStatus(str, new object[0]);
                    return ptr;

                case 5:
                {
                    int dest = 0;
                    CopyMemA(dest, parm, 2);
                    this.SetStatus("Patching Supreme Commander: {0}%", dest, new object[0]);
                    return ptr;
                }
                case 6:
                {
                    int num2 = -1;
                    CopyMemA(num2, parm, 4);
                    this.SetStatus("Updating {0} files", num2, new object[0]);
                    return ptr;
                }
                case 7:
                    return ptr;

                case 8:
                    return ptr;

                case 9:
                case 10:
                case 11:
                case 12:
                    str = Marshal.PtrToStringAnsi(parm);
                    this.SetStatus(str, new object[0]);
                    return ptr;

                case 13:
                    this.SetStatus("Patch Aborted on Patch File Location", new object[0]);
                    return ptr;

                case 14:
                    this.SetStatus("Patch Aborted on Invalid Patch File", new object[0]);
                    return ptr;

                case 15:
                    this.SetStatus("Patch Aborted on Password Dialog", new object[0]);
                    return new IntPtr(0);

                case 0x10:
                    this.SetStatus("Patch Aborted on Invalid Password", new object[0]);
                    return new IntPtr(0);

                case 0x11:
                    this.SetStatus("Patch Aborted on Next Disk Dialog", new object[0]);
                    return new IntPtr(0);

                case 0x12:
                    this.SetStatus("Patch Aborted on Invalid Disk Alert", new object[0]);
                    return new IntPtr(0);

                case 0x13:
                    return ptr;

                case 20:
                    this.SetStatus("Patch Aborted on Location Dialog", new object[0]);
                    return new IntPtr(0);

                case 0x15:
                    return ptr;

                case 0x16:
                    return ptr;
            }
            return ptr;
        }

        internal void OnRecieveKick(string senderName)
        {
            if (!Chatroom.InChatroom)
            {
                AuditLog.WriteLine("User {0} received a kick command from {1} while not in a chatroom.", new object[] { User.Current.Name, senderName });
            }
            else if (User.Current.IsAdmin)
            {
                AuditLog.WriteLine("Administrator {0} received a /kick command from unauthorized user {1}", new object[] { User.Current.Name, senderName });
            }
            else
            {
                User user;
                if (!Chatroom.GatheringParticipants.TryFindByIndex("name", senderName, out user))
                {
                    AuditLog.WriteLine("Received /kick command from unknown user {0}", new object[] { senderName });
                }
                else
                {
                    bool flag = false;
                    if (Chatroom.InChatroom && Chatroom.Current.IsClanRoom)
                    {
                        ClanMember member;
                        if (Clan.CurrentMembers.TryFindByIndex("name", senderName, out member))
                        {
                            if (!((user.IsAdmin || user.IsModerator) || member.CanTargetAbility(ClanAbility.Kick, ClanMember.Current)))
                            {
                                AuditLog.WriteLine("Received a clan chatroom /kick command from unauthorized user {0}", new object[] { senderName });
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                        else
                        {
                            AuditLog.WriteLine("Received a clan chatroom /kick command from non-clan user {0}", new object[] { senderName });
                        }
                    }
                    else if ((!user.IsAdmin && !user.IsModerator) && (Chatroom.Current.IsPersistent || !user.IsChannelOperator))
                    {
                        AuditLog.WriteLine("Received /kick command from non-admin non channel operator user {0}", new object[] { senderName });
                    }
                    else
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        Messaging.SendCustomCommand(CustomCommands.SystemEvent, new object[] { "{0} has been kicked out of {1} by {2}", User.Current.Name, Chatroom.CurrentName, senderName });
                        if (!ThreadQueue.Quazal.WaitUntilEmpty(0xbb8))
                        {
                            ErrorLog.WriteLine("Failed to notify chatroom of exit while being kicked out.", new object[0]);
                        }
                        this.LeaveChat();
                    }
                }
            }
        }

        internal void OnRecieveWhisper(string senderName, string msg)
        {
            VGen0 method = null;
            try
            {
                if (senderName != User.Current.Name)
                {
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                try
                                {
                                    string key = senderName.ToLower();
                                    if ((!this.PrivateChats.ContainsKey(key) || this.PrivateChats[key].Disposing) || this.PrivateChats[key].IsDisposed)
                                    {
                                        this.PrivateChats[key] = new FrmPrivateChat(this, senderName);
                                        this.PrivateChats[key].FormClosing += new FormClosingEventHandler(this.PrivateChatClosing);
                                        this.PrivateChats[key].Show();
                                    }
                                    if (!this.PMTargets.Contains(senderName))
                                    {
                                        this.PMTargets.Add(senderName);
                                    }
                                    if ((msg != null) && (msg.Length > 0))
                                    {
                                        this.PrivateChats[key].OnMessageRecieved(msg);
                                    }
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else
                    {
                        string str = senderName.ToLower();
                        if ((!this.PrivateChats.ContainsKey(str) || this.PrivateChats[str].Disposing) || this.PrivateChats[str].IsDisposed)
                        {
                            this.PrivateChats[str] = new FrmPrivateChat(this, senderName);
                            this.PrivateChats[str].FormClosing += new FormClosingEventHandler(this.PrivateChatClosing);
                            this.PrivateChats[str].Show();
                        }
                        if ((msg != null) && (msg.Length > 0))
                        {
                            this.PrivateChats[str].OnMessageRecieved(msg);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnRefreshGatherings(DataList data)
        {
            this.OnRefreshGatherings(data, false);
        }

        private void OnRefreshGatherings(DataList data, bool clearFirst)
        {
            MappedObjectList<Chatroom> rooms = new MappedObjectList<Chatroom>(data);
            int num = 0;
            if (ConfigSettings.GetBool("ShowChannels", false))
            {
                num = 2;
            }
            clearFirst = this.GatheringDisplaycontrol.RoomCount != (rooms.Count + num);
            this.GatheringDisplaycontrol.RefreshGatherings(rooms, clearFirst);
            this.GatheringDisplaycontrol.Popup += new EventHandler(this.comboBoxGatherings_Popup);
        }

        protected override void OnResize(EventArgs e)
        {
            if (base.WindowState == FormWindowState.Maximized)
            {
                base.WindowState = FormWindowState.Normal;
                this.mIsMaximized = false;
                this.pbRestore_Click(this.pbRestore, EventArgs.Empty);
            }
            base.OnResize(e);
            this.DoSizing();
            this.AlignChatTabs();
        }

        private void OnSelectCustomGame(object sender, EventArgs e)
        {
            this.BeforeGameChatroom = Chatroom.CurrentName;
            this.mGameName = this.DlgSelectGame.GameName;
            this.mGameURL = this.DlgSelectGame.URL;
            this.mHostName = this.DlgSelectGame.HostName;
            ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Leave", this, "OnJoinGame", new object[0]);
            this.SetStatusButtons(1);
        }

        internal void OnSendKick(string playerName)
        {
            if (!Chatroom.InChatroom)
            {
                this.ErrorMessage("<LOC>You are not in a chatroom.", new object[0]);
            }
            else if (User.Current.Name.ToLower() == playerName.ToLower())
            {
                this.ErrorMessage("<LOC>You cannot kick yourself out of chat.", new object[0]);
            }
            else
            {
                User user;
                if (!Chatroom.GatheringParticipants.TryFindByIndex("name", playerName, out user))
                {
                    this.ErrorMessage("<LOC>Unable to locate user {0}", new object[] { playerName });
                }
                else if (user.IsAdmin)
                {
                    this.ErrorMessage("<LOC>You do not have permission to kick {0}, you cannot kick administrators out of chat.", new object[] { playerName });
                }
                else if (Chatroom.Current.IsClanRoom)
                {
                    if (!((User.Current.IsAdmin || User.Current.IsModerator) || ClanMember.Current.CanTargetAbility(ClanAbility.Kick, user.Name)))
                    {
                        this.ErrorMessage("<LOC>You do not have permission to kick {0}, you must be an administrator or higher ranking clan member to kick players out of chat.", new object[] { playerName });
                    }
                    else
                    {
                        Messaging.SendCustomCommand(playerName, CustomCommands.Kick, new object[0]);
                    }
                }
                else if ((!User.Current.IsAdmin && !User.Current.IsModerator) && (Chatroom.Current.IsPersistent || !User.Current.IsChannelOperator))
                {
                    this.ErrorMessage("<LOC>You do not have permission to kick {0}, you must be an administrator to kick players out of chat.", new object[] { playerName });
                }
                else
                {
                    Messaging.SendCustomCommand(playerName, CustomCommands.Kick, new object[0]);
                }
            }
        }

        internal void OnSendWhisper(string targetName, string msg)
        {
            try
            {
                if (User.Current.Name.ToLower() == targetName.ToLower())
                {
                    this.ErrorMessage("<LOC>You cannot send private messages to yourself.", new object[0]);
                }
                else
                {
                    IUser user;
                    if (this.TryFindMember(targetName, false, out user))
                    {
                        if (!user.Online)
                        {
                            this.SystemMessage("<LOC id=_70a4b0e2dc7b08ed21dcf08ac09fbff9>{0} is offline and cannot receive private messages.", new object[] { targetName });
                            return;
                        }
                        if (!(!user.IsDND || this.IsFriendOrClanmate(user.Name)))
                        {
                            this.SystemMessage("<LOC>Could not send message to {0} because the user is in Do Not Disturb mode.", new object[] { targetName });
                            return;
                        }
                    }
                    if (targetName != User.Current.Name)
                    {
                        VGen0 method = null;
                        VGen0 gen2 = null;
                        string key = targetName.ToLower();
                        if ((!this.PrivateChats.ContainsKey(key) || this.PrivateChats[key].Disposing) || this.PrivateChats[key].IsDisposed)
                        {
                            if (base.InvokeRequired)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.PrivateChats[key] = new FrmPrivateChat(this, targetName, msg);
                                        this.PrivateChats[key].FormClosing += new FormClosingEventHandler(this.PrivateChatClosing);
                                        this.PrivateChats[key].Show();
                                        this.PrivateChats[key].Activate();
                                        this.PrivateChats[key].FocusInput();
                                    };
                                }
                                base.BeginInvoke(method);
                            }
                            else
                            {
                                this.PrivateChats[key] = new FrmPrivateChat(this, targetName, msg);
                                this.PrivateChats[key].FormClosing += new FormClosingEventHandler(this.PrivateChatClosing);
                                this.PrivateChats[key].Show();
                                this.PrivateChats[key].Activate();
                                this.PrivateChats[key].FocusInput();
                            }
                        }
                        else if (!this.PrivateChats[key].Disposing && !this.PrivateChats[key].IsDisposed)
                        {
                            if (base.InvokeRequired)
                            {
                                if (gen2 == null)
                                {
                                    gen2 = delegate {
                                        this.PrivateChats[key].SendMessage(msg);
                                    };
                                }
                                base.BeginInvoke(gen2);
                            }
                            else
                            {
                                this.PrivateChats[key].SendMessage(msg);
                            }
                        }
                        else
                        {
                            this.PrivateChats.Remove(key);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            base.ShowInTaskbar = true;
            this.SetRegion();
            base.Activate();
            base.BringToFront();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            VGen0 method = null;
            base.OnSizeChanged(e);
            this.AlignChatTabs();
            if (this.gvChat != null)
            {
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            try
                            {
                                if (this.gvChat.RowCount > 0)
                                {
                                    GPGGridView.ScrollToBottom(this.gvChat);
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!base.Disposing && !base.IsDisposed)
                {
                    try
                    {
                        if (this.gvChat.RowCount > 0)
                        {
                            GPGGridView.ScrollToBottom(this.gvChat);
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
        }

        internal void OnTeamAutomatchExit()
        {
            VGen0 method = null;
            GPG.Logging.EventLog.DoStackTrace();
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        Thread.Sleep(100);
                    }
                    catch (ThreadInterruptedException exception)
                    {
                        GPG.Logging.EventLog.WriteLine("The thread was woken up: " + exception.Message, new object[0]);
                    }
                }
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this._TeamAutomatchExit();
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!(base.Disposing || base.IsDisposed))
                {
                    this._TeamAutomatchExit();
                }
            }
            catch (Exception exception2)
            {
                ErrorLog.WriteLine(exception2);
            }
        }

        internal void OnTeamAutomatchLaunchGame()
        {
            VGen0 method = null;
            this.mIsInGame = true;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.TeamAutomatchLaunchGame(null, null);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    this.DlgTeamGame.OnTeamGameStart();
                    this.btnRankedGame.Enabled = false;
                    this.btnArrangedTeam.Enabled = false;
                    this.btnPlayNow.Enabled = false;
                    this.pbMiddle.Remove(this.StatusButtonRankedGameCancel);
                    this.SetStatus("", new object[0]);
                    this.ChangeStatus("<LOC>Supreme Commander", StatusIcons.in_game);
                    this.mSearchingForAutomatch = false;
                    this.ShowWelcome(this.UrlById("GAME"));
                    this.DisableControls();
                    this.mTeamAutomatchExit = false;
                    SupcomAutomatch.GetSupcomAutomatch().OnExit += new EventHandler(this.TeamAutomatchExit);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        internal void OnViewClanProfileByName(string clanName)
        {
            if ((this.ClanProfileForm == null) || this.ClanProfileForm.IsDisposed)
            {
                this.ClanProfileForm = new DlgClanProfileEx();
                if (this.ClanProfileForm.SetCurrentClanByName(clanName))
                {
                    this.ClanProfileForm.Construct();
                    this.ClanProfileForm.Show();
                }
            }
            else if (this.ClanProfileForm.SetCurrentClanByName(clanName))
            {
                this.ClanProfileForm.Construct();
            }
        }

        internal void OnViewClanProfileByPlayer(string playerName)
        {
            if ((this.ClanProfileForm == null) || this.ClanProfileForm.IsDisposed)
            {
                this.ClanProfileForm = new DlgClanProfileEx();
                if (this.ClanProfileForm.SetCurrentClanByPlayer(playerName))
                {
                    this.ClanProfileForm.Construct();
                    this.ClanProfileForm.Show();
                }
            }
            else if (this.ClanProfileForm.SetCurrentClanByPlayer(playerName))
            {
                this.ClanProfileForm.Construct();
            }
        }

        internal void OnViewPlayerProfile(string playerName)
        {
            if ((this.PlayerProfileForm == null) || this.PlayerProfileForm.IsDisposed)
            {
                this.PlayerProfileForm = new DlgPlayerProfileEx();
                if (this.PlayerProfileForm.SetTargetPlayer(playerName))
                {
                    this.PlayerProfileForm.Construct();
                    this.PlayerProfileForm.Show();
                }
            }
            else if (this.PlayerProfileForm.SetTargetPlayer(playerName))
            {
                this.PlayerProfileForm.Construct();
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (!User.LoggedIn)
            {
                base.Visible = false;
            }
            else
            {
                base.OnVisibleChanged(e);
            }
        }

        private void ParseCustomCommand(string cmd, string[] args)
        {
            this.DoParseCustomCommand(cmd, args);
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.CloseClick(sender, e);
        }

        private void pbMinimize_Click(object sender, EventArgs e)
        {
            this.Minimize(sender, e);
        }

        private void pbPaint(object sender, PaintEventArgs e)
        {
            PictureBox box = sender as PictureBox;
            Graphics graphics = e.Graphics;
            for (int i = 0; i < box.Width; i += box.Image.Width)
            {
                Rectangle rect = new Rectangle(i, 0, box.Image.Width, box.Image.Height);
                graphics.DrawImage(box.Image, rect);
            }
            if (sender == this.pbTop)
            {
                Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(0x7d, 0, 0, 0));
                e.Graphics.DrawString(GameInformation.SelectedGame.GameDescription, Program.Settings.Appearance.Chat.SelectedGameFont, brush, (float) 30f, (float) 5f);
                brush.Dispose();
            }
        }

        private void pbRestore_Click(object sender, EventArgs e)
        {
            base.Opacity = 1.0;
            this.Restore(sender, e);
        }

        private void PerformFade()
        {
        }

        internal void PingPlayer(string name)
        {
            Messaging.SendCustomCommand(name, CustomCommands.PingRequest, new object[0]);
        }

        private void PingResponse(string senderName)
        {
            if (this.PingResponseReceived != null)
            {
                this.PingResponseReceived(senderName);
            }
            if (this.PrivateChats.ContainsKey(senderName))
            {
                FrmPrivateChat chat = this.PrivateChats[senderName];
                if (!chat.IsDisposed && !chat.Disposing)
                {
                    if (!chat.ChatTarget.Online)
                    {
                        chat.SystemMessage("<LOC>{0} is online.", new object[] { senderName });
                    }
                    chat.ChatTarget.Online = true;
                }
            }
        }

        public void PlayRankedGame(bool playNow)
        {
            this.PlayRankedGame(playNow, "1v1", new List<string>(), "");
        }

        public void PlayRankedGame(bool playNow, string kind, List<string> allies, string teamname)
        {
            this.PlayRankedGame(playNow, kind, allies, teamname, false);
        }

        public void PlayRankedGame(bool playNow, string kind, List<string> allies, string teamname, bool isTournament)
        {
            ThreadStart start = null;
            try
            {
                this.PlayNowMatch = playNow;
                if (this.mSearchingForAutomatch || (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable))
                {
                    DlgMessage.ShowDialog(Loc.Get("<LOC>You are already in the ranked game queue."));
                }
                else if ((((GameInformation.SelectedGame.GameLocation != null) && (GameInformation.SelectedGame.GameLocation.Length > 0)) && System.IO.File.Exists(GameInformation.SelectedGame.GameLocation)) || this.LocateExe(GameInformation.SelectedGame.ExeName, true))
                {
                    if (!(User.Current.IsAdmin || this.IsGameCurrent))
                    {
                        this.UpdateSupCom();
                    }
                    else
                    {
                        DlgSupcomMapOptions options;
                        SupComGameManager.LastLocation = GameInformation.SelectedGame.GameLocation;
                        SupComGameManager.GameArgs = Program.Settings.SupcomPrefs.CommandLineArgs;
                        if (isTournament)
                        {
                            if (TournamentCommands.sFaction.ToUpper().IndexOf("ANY FACTION") >= 0)
                            {
                                playNow = false;
                            }
                            if (TournamentCommands.sMap.ToUpper().IndexOf("LADDER MAPS") >= 0)
                            {
                                playNow = false;
                            }
                        }
                        if ((playNow && !isTournament) && ((((Program.Settings.SupcomPrefs.RankedGames.Maps == null) || (Program.Settings.SupcomPrefs.RankedGames.Maps.Length < 1)) || (Program.Settings.SupcomPrefs.RankedGames.Faction == null)) || (Program.Settings.SupcomPrefs.RankedGames.Faction.Length < 1)))
                        {
                            playNow = false;
                        }
                        if (isTournament)
                        {
                            options = new DlgSupcomMapOptions("1v1");
                            if (TournamentCommands.sFaction.ToUpper().IndexOf("ANY FACTION") < 0)
                            {
                                options.rbAeon.Enabled = false;
                                options.rbUEF.Enabled = false;
                                options.rbCybran.Enabled = false;
                                options.rbRandom.Enabled = false;
                            }
                            if (TournamentCommands.sMap.ToUpper().IndexOf("LADDER MAPS") < 0)
                            {
                                options.gpgMapSelectGrid.Enabled = false;
                            }
                        }
                        else if (kind.ToUpper().IndexOf("CHALLENGE") == 0)
                        {
                            options = new DlgSupcomMapOptions("1v1");
                        }
                        else
                        {
                            options = new DlgSupcomMapOptions(kind);
                        }
                        bool flag = this.IsRegularRankedGame();
                        if ((playNow || flag) || (options.ShowDialog() == DialogResult.OK))
                        {
                            this.StatusButtonRankedGameCancel.Click += new EventHandler(this.StatusButtonRankedGameCancel_Click);
                            this.mAutoStatus = Loc.Get("<LOC>Searching for game ");
                            this.StatusButtonRankedGameCancel.Text = Loc.Get("<LOC>Cancel");
                            this.pbMiddle.Add(this.StatusButtonRankedGameCancel);
                            this.mSearchingForAutomatch = true;
                            this.miGameGroup.Enabled = false;
                            this.btnHostGame.Enabled = false;
                            this.btnJoinGame.Enabled = false;
                            this.btnArrangedTeam.Enabled = false;
                            if (playNow)
                            {
                                this.btnPlayNow.Image = SkinManager.GetImage("nav-cancel_play_now.png");
                                this.btnPlayNow.ToolTipText = Loc.Get("<LOC>Cancel Ranked Game Search");
                                this.btnRankedGame.Enabled = false;
                            }
                            else
                            {
                                this.btnRankedGame.Image = SkinManager.GetImage("nav-cancel_ranked_game.png");
                                this.btnRankedGame.ToolTipText = Loc.Get("<LOC>Cancel Ranked Game Search");
                                this.btnPlayNow.Enabled = false;
                            }
                            this.mLastStatus = this.skinDropDownStatus.Text;
                            this.SetStatusButtons(1);
                            if (start == null)
                            {
                                start = delegate {
                                    VGen1 method = null;
                                    int num = 0;
                                    while (this.mSearchingForAutomatch)
                                    {
                                        try
                                        {
                                            num++;
                                            if (!base.Disposing && !base.IsDisposed)
                                            {
                                                if (method == null)
                                                {
                                                    method = delegate (object objcount) {
                                                        try
                                                        {
                                                            int seconds = (int) objcount;
                                                            TimeSpan span = new TimeSpan(0, 0, seconds);
                                                            this.ChangeStatus("<LOC>Searching", StatusIcons.search);
                                                            this.SetStatus(this.mAutoStatus + " (" + span.ToString() + ")", new object[0]);
                                                        }
                                                        catch (Exception exception)
                                                        {
                                                            ErrorLog.WriteLine(exception);
                                                        }
                                                    };
                                                }
                                                base.BeginInvoke(method, new object[] { num });
                                            }
                                            else
                                            {
                                                this.mSearchingForAutomatch = false;
                                            }
                                            Thread.Sleep(0x3e8);
                                        }
                                        catch (Exception exception)
                                        {
                                            ErrorLog.WriteLine(exception);
                                        }
                                    }
                                };
                            }
                            Thread thread = new Thread(start);
                            thread.IsBackground = true;
                            thread.Start();
                            if (kind == "1v1")
                            {
                                SupcomAutomatch.GetSupcomAutomatch().RegisterNewGame(kind);
                            }
                            else if ((kind.IndexOf("TOURNY") == 0) || (kind.IndexOf("CHALLENGE") == 0))
                            {
                                SupcomAutomatch.GetSupcomAutomatch().RegisterNewGame(kind, null, "", true);
                            }
                            else
                            {
                                SupcomAutomatch.GetSupcomAutomatch().RegisterNewGame(kind, allies, teamname);
                            }
                            SupcomAutomatch.GetSupcomAutomatch().OnAutomatchStats += new AutomatchStatsDelegate(this.FrmMain_OnAutomatchStats);
                            if (!this.IsRegularRankedGame())
                            {
                                if (isTournament)
                                {
                                    if (TournamentCommands.sFaction.ToUpper().IndexOf("RANDOM FACTION") >= 0)
                                    {
                                        SupcomAutomatch.GetSupcomAutomatch().Faction = Program.Settings.SupcomPrefs.RankedGames.RandomFaction();
                                    }
                                    else if (TournamentCommands.sFaction.ToUpper().IndexOf("ANY FACTION") < 0)
                                    {
                                        SupcomAutomatch.GetSupcomAutomatch().Faction = "/" + TournamentCommands.sFaction;
                                    }
                                    else
                                    {
                                        SupcomAutomatch.GetSupcomAutomatch().Faction = Program.Settings.SupcomPrefs.RankedGames.FetchFaction();
                                    }
                                    if (TournamentCommands.sMap.ToUpper().IndexOf("LADDER MAPS") < 0)
                                    {
                                        SupcomAutomatch.GetSupcomAutomatch().MapName = TournamentCommands.sMap;
                                    }
                                    else
                                    {
                                        SupcomAutomatch.GetSupcomAutomatch().MapName = Program.Settings.SupcomPrefs.RankedGames.FetchMap();
                                    }
                                }
                                else
                                {
                                    SupcomAutomatch.GetSupcomAutomatch().Faction = Program.Settings.SupcomPrefs.RankedGames.FetchFaction();
                                    SupcomAutomatch.GetSupcomAutomatch().MapName = Program.Settings.SupcomPrefs.RankedGames.FetchMap();
                                }
                            }
                            if (this.mFirstRankedSupcomGame)
                            {
                                this.mFirstRankedSupcomGame = false;
                                SupcomAutomatch.GetSupcomAutomatch().OnLaunchGame += new EventHandler(this.AutomatchLaunchGame);
                                SupcomAutomatch.GetSupcomAutomatch().OnStatusChanged += new StringEventHandler2(this.AutomatchStatusChanged);
                                SupcomAutomatch.GetSupcomAutomatch().OnExit += new EventHandler(this.AutomatchExit);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void PlayRankedTeamGame(string kind, List<string> allies, string teamname)
        {
            ThreadStart start = null;
            try
            {
                if (this.mSearchingForAutomatch || (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable))
                {
                    DlgMessage.ShowDialog(Loc.Get("<LOC>You are already in the ranked game queue."));
                }
                else if ((((GameInformation.SelectedGame.GameLocation != null) && (GameInformation.SelectedGame.GameLocation.Length > 0)) && System.IO.File.Exists(GameInformation.SelectedGame.GameLocation)) || this.LocateExe("SupremeCommander", true))
                {
                    if (!(User.Current.IsAdmin || this.IsGameCurrent))
                    {
                        this.UpdateSupCom();
                    }
                    else
                    {
                        SupComGameManager.LastLocation = GameInformation.SelectedGame.GameLocation;
                        SupComGameManager.GameArgs = Program.Settings.SupcomPrefs.CommandLineArgs;
                        this.mLastStatus = this.skinDropDownStatus.Text;
                        this.SetStatusButtons(1);
                        if (start == null)
                        {
                            start = delegate {
                                VGen1 method = null;
                                int num = 0;
                                while (this.mSearchingForAutomatch)
                                {
                                    try
                                    {
                                        num++;
                                        if (!base.Disposing && !base.IsDisposed)
                                        {
                                            if (method == null)
                                            {
                                                method = delegate (object objcount) {
                                                    try
                                                    {
                                                        int seconds = (int) objcount;
                                                        TimeSpan span = new TimeSpan(0, 0, seconds);
                                                        this.ChangeStatus("<LOC>Searching", StatusIcons.search);
                                                        this.SetStatus(this.mAutoStatus + " (" + span.ToString() + ")", new object[0]);
                                                    }
                                                    catch (Exception exception)
                                                    {
                                                        ErrorLog.WriteLine(exception);
                                                    }
                                                };
                                            }
                                            base.BeginInvoke(method, new object[] { num });
                                        }
                                        else
                                        {
                                            this.mSearchingForAutomatch = false;
                                        }
                                        Thread.Sleep(0x3e8);
                                    }
                                    catch (Exception exception)
                                    {
                                        ErrorLog.WriteLine(exception);
                                    }
                                }
                            };
                        }
                        new Thread(start).Start();
                        if (kind.ToLower() == "1v1")
                        {
                            SupcomAutomatch.GetSupcomAutomatch().RegisterNewGame(kind);
                        }
                        else
                        {
                            SupcomAutomatch.GetSupcomAutomatch().RegisterNewGame(kind, allies, teamname);
                        }
                        SupcomAutomatch.GetSupcomAutomatch().OnAutomatchStats += new AutomatchStatsDelegate(this.FrmMain_OnAutomatchStats);
                        SupcomAutomatch.GetSupcomAutomatch().Faction = Program.Settings.SupcomPrefs.RankedGames.FetchFaction();
                        SupcomAutomatch.GetSupcomAutomatch().MapName = Program.Settings.SupcomPrefs.RankedGames.FetchMap();
                        if (mFirstRankedSupcomTeamGame)
                        {
                            mFirstRankedSupcomTeamGame = false;
                            SupcomAutomatch.GetSupcomAutomatch().OnLaunchGame += new EventHandler(this.TeamAutomatchLaunchGame);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void PopulateLadderMenuItems()
        {
            LadderInstance[] array = new QuazalQuery("GetActiveLadderInstances", new object[0]).GetObjects<LadderInstance>().ToArray();
            try
            {
                Array.Sort<LadderInstance>(array, new LadderMenuItemSorter());
            }
            catch
            {
                ErrorLog.WriteLine("Failed to sort ladder menu items, defaulting to database row order.", new object[0]);
            }
            ToolStripMenuItem[] itemArray = new ToolStripMenuItem[array.Length];
            for (int i = array.Length; i > 0; i--)
            {
                LadderInstance instance = array[i - 1];
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = "miLadder_" + instance.ID.ToString();
                item.Visible = true;
                item.Size = new Size(210, 20);
                item.Text = instance.Description;
                item.Tag = instance;
                item.Click += new EventHandler(this.LadderMenuItem_Click);
                this.miLadders.DropDownItems.Insert(0, item);
            }
        }

        private void PreLoadChat()
        {
            ThreadQueue.QueueUserWorkItem(delegate (object p) {
                VGen0 method = null;
                EventHandler handler = null;
                PaintEventHandler handler2 = null;
                MouseEventHandler handler3 = null;
                EventHandler handler4 = null;
                ChatLinkEventHandler handler5 = null;
                ChatLinkEventHandler handler6 = null;
                ChatLinkEventHandler handler7 = null;
                ChatLinkEventHandler handler8 = null;
                ChatLinkEventHandler handler9 = null;
                ChatLinkEventHandler handler10 = null;
                ChatLinkEventHandler handler11 = null;
                ChatLinkEventHandler handler12 = null;
                ChatLinkEventHandler handler13 = null;
                ChatLinkEventHandler handler14 = null;
                ChatLinkEventHandler handler15 = null;
                EventHandler handler16 = null;
                EventHandler handler17 = null;
                EventHandler handler18 = null;
                EventHandler handler19 = null;
                try
                {
                    this.InitChatContainers();
                    if (!this.CancelLoad && base.InvokeRequired)
                    {
                        if (!base.Disposing && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.mChatHistory = new LinkedList<string>();
                                    this.mChatLines = new BindingList<ChatLine>();
                                    this.gpgChatGrid.DataSource = this.mChatLines;
                                };
                            }
                            base.BeginInvoke(method);
                        }
                    }
                    else
                    {
                        this.mChatHistory = new LinkedList<string>();
                        this.mChatLines = new BindingList<ChatLine>();
                        this.gpgChatGrid.DataSource = this.mChatLines;
                    }
                    ThreadQueue.Quazal.Enqueue((VGen0)delegate {
                        DataList queryData = DataAccess.GetQueryData("GetIgnoredPlayers", new object[0]);
                        foreach (DataRecord record in queryData)
                        {
                            User.IgnoredPlayers.Add(Convert.ToInt32(record["id"]));
                        }
                    }, new object[0]);
                    foreach (TextContainer<StatusTextLine> container in this.ChatContainers)
                    {
                        container.Clear();
                    }
                    this.textBoxMsg.KeyDown += new KeyEventHandler(this.textBoxMsg_KeyDown);
                    this.textBoxMsg.TextChanged += new EventHandler(this.UpdateCommandPopup);
                    this.textBoxMsg.LostFocus += new EventHandler(this.textBoxMsg_LostFocus);
                    this.gpgTextListCommands.ValueSelected += new EventHandler<TextValEventArgs>(this.CommandSelected);
                    this.gvChat.MouseDown += new MouseEventHandler(this.gvChat_MouseDown);
                    this.gvChat.MouseMove += new MouseEventHandler(this.gvChat_MouseMove);
                    this.gvChat.MouseUp += new MouseEventHandler(this.gvChat_MouseUp);
                    if (handler == null)
                    {
                        handler = delegate (object s, EventArgs e) {
                            this.EmoteCount = 0;
                            this.CountEmotes = true;
                        };
                    }
                    this.gvChat.RowCountChanged += handler;
                    if (handler2 == null)
                    {
                        handler2 = delegate (object s, PaintEventArgs e) {
                            this.CountEmotes = false;
                            if (this.PaintPending)
                            {
                                ImageAnimator.UpdateFrames();
                                this.PaintPending = false;
                            }
                        };
                    }
                    this.gpgChatGrid.Paint += handler2;
                    this.ChatFiltersChanged();
                    if (handler3 == null)
                    {
                        handler3 = delegate (object s, MouseEventArgs e) {
                            this.textBoxMsg.Select();
                            GPGGridView.ScrollBarFocus(this.gvChat);
                        };
                    }
                    GPGGridView.ScrollBarClick(this.gvChat, handler3);
                    this.ResetAwayTimer();
                    if (handler4 == null)
                    {
                        handler4 = delegate (object s, EventArgs e) {
                            if (this.gpgTextListCommands.Visible)
                            {
                                this.gpgTextListCommands.BringToFront();
                            }
                        };
                    }
                    this.gpgTextListCommands.VisibleChanged += handler4;
                    if (handler5 == null)
                    {
                        handler5 = delegate (ChatLink sender, string url) {
                            this.ShowWebPage(url);
                        };
                    }
                    ChatLink.Web.Click += handler5;
                    if (handler6 == null)
                    {
                        handler6 = delegate (ChatLink sender, string url) {
                            this.ShowWebPage(string.Format("mailto:{0}", url.Replace("mailto:", "")));
                        };
                    }
                    ChatLink.Email.Click += handler6;
                    if (handler7 == null)
                    {
                        handler7 = delegate (ChatLink sender, string url) {
                            this.OnViewPlayerProfile(url);
                        };
                    }
                    ChatLink.Player.Click += handler7;
                    if (handler8 == null)
                    {
                        handler8 = delegate (ChatLink sender, string url) {
                            this.OnViewClanProfileByName(url);
                        };
                    }
                    ChatLink.Clan.Click += handler8;
                    if (handler9 == null)
                    {
                        handler9 = delegate (ChatLink sender, string url) {
                            if (!this.mRanInitializeChat)
                            {
                                this.CloseHomePage(url);
                            }
                            else
                            {
                                this.CreateChannelIfNonExist(url);
                            }
                        };
                    }
                    ChatLink.Chat.Click += handler9;
                    if (handler10 == null)
                    {
                        handler10 = delegate (ChatLink sender, string url) {
                            try
                            {
                                bool flag = true;
                                if (Program.Settings.Chat.Emotes.ShowViewWarning)
                                {
                                    string msg = "<LOC>Clicking this link will trigger this user's custom emote for the first time. Although abuse of the system is prohibited, the emote displayed has been created by this user and has not been pre-screened. You can manage the appearance of custom emotes via Tools > Chat > Emotes and learn more about them via the Knowledge Base. Do you want to accept this custom emote?";
                                    DlgYesNo no = new DlgYesNo(this, "<LOC>Warning", msg, new Size(350, 350)) {
                                        DoNotShowAgainCheck = true
                                    };
                                    if (no.ShowDialog() != DialogResult.Yes)
                                    {
                                        flag = false;
                                    }
                                    Program.Settings.Chat.Emotes.ShowViewWarning = !no.DoNotShowAgainValue;
                                }
                                if (flag)
                                {
                                    char ch = '\x0003';
                                    if (url.Contains(ch.ToString()))
                                    {
                                        string[] strArray = url.Split(new char[] { '\x0003' });
                                        string recipient = strArray[0];
                                        string str3 = strArray[1];
                                        Messaging.SendCustomCommand(recipient, CustomCommands.RequestEmotes, new object[] { str3 });
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    ChatLink.Emote.Click += handler10;
                    if (handler11 == null)
                    {
                        handler11 = delegate (ChatLink sender, string url) {
                            TimeSpan span = (TimeSpan) (DateTime.Now - this.mGameLinkClickTime);
                            if (span.TotalSeconds >= 2.0)
                            {
                                this.mGameLinkClickTime = DateTime.Now;
                                if (this.mSearchingForAutomatch)
                                {
                                    this.ErrorMessage("<LOC>You cannot join a custom game while searching for a ranked game.", new object[0]);
                                }
                                else
                                {
                                    GameItem item;
                                    bool flag = false;
                                    if (ConfigSettings.GetBool("DoOldGameList", false))
                                    {
                                        flag = DataAccess.TryGetObject<GameItem>("GetGameByName", out item, new object[] { url.Replace("'", @"\'") });
                                    }
                                    else
                                    {
                                        flag = DataAccess.TryGetObject<GameItem>("GetGameByName2", out item, new object[] { url.Replace("'", @"\'"), GameInformation.SelectedGame.GameID });
                                    }
                                    if (!flag)
                                    {
                                        this.ErrorMessage(Loc.Get("<LOC>The game '{0}' does not exist. Click the Join Game button for a list of currently available games.", new object[] { url }), new object[0]);
                                        this.gvChat.MoveLastVisible();
                                    }
                                    else if (((item.Password != null) && (item.Password.Length > 0)) && (item.Password != DlgAskQuestion.AskQuestion(this, Loc.Get("<LOC>What is the password?"), true)))
                                    {
                                        DlgMessage.ShowDialog(Loc.Get("<LOC>That password is not correct."));
                                    }
                                    else
                                    {
                                        this.BeforeGameChatroom = Chatroom.CurrentName;
                                        this.mGameName = item.Description;
                                        this.mGameURL = item.URL;
                                        this.mHostName = item.PlayerName;
                                        ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Leave", this, "OnJoinGame", new object[0]);
                                        int num = 0;
                                        while (Chatroom.InChatroom)
                                        {
                                            Thread.Sleep(10);
                                            Application.DoEvents();
                                            num++;
                                            if (num > 300)
                                            {
                                                break;
                                            }
                                        }
                                        this.SetStatusButtons(1);
                                    }
                                }
                            }
                        };
                    }
                    ChatLink.Game.Click += handler11;
                    if (handler12 == null)
                    {
                        handler12 = delegate (ChatLink sender, string url) {
                            this.ShowDlgKeywordSearch(url, false);
                        };
                    }
                    ChatLink.Help.Click += handler12;
                    if (handler13 == null)
                    {
                        handler13 = delegate (ChatLink sender, string url) {
                            int num;
                            if (int.TryParse(url, out num))
                            {
                                this.ShowDlgSolution(num);
                            }
                        };
                    }
                    ChatLink.Solution.Click += handler13;
                    ChatLink.Replay.Click += delegate (ChatLink sender, string url) {
                        try
                        {
                            if ((url != null) && (url.Length > 0))
                            {
                                ReplayInfo.Download(url);
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                    if (handler14 == null)
                    {
                        handler14 = delegate (ChatLink sender, string url) {
                            ThreadQueue.QueueUserWorkItem(delegate (object objurl) {
                                try
                                {
                                    string str = ((object[]) objurl)[0].ToString();
                                    DataList queryDataSafe = new DataList();
                                    if (str.IndexOf(" ") > 0)
                                    {
                                        string str2 = str.Split(new char[] { ' ' })[0];
                                        int num = Convert.ToInt32(str.Split(new char[] { ' ' })[1]);
                                        queryDataSafe = DataAccess.GetQueryDataSafe("GetReplay", new object[] { str2, num });
                                    }
                                    else
                                    {
                                        queryDataSafe = DataAccess.GetQueryDataSafe("GetLastReplay", new object[] { str });
                                    }
                                    if (queryDataSafe.Count > 0)
                                    {
                                        if (((GameInformation.SelectedGame.GameLocation != null) && (GameInformation.SelectedGame.GameLocation.Length >= 1)) || this.LocateExe("SupremeCommander", true))
                                        {
                                            int @int = ConfigSettings.GetInt("Live Replay GameID", 0x12);
                                            foreach (GameInformation information in GameInformation.Games)
                                            {
                                                if (information.GameID == @int)
                                                {
                                                    SupComGameManager.LastLocation = information.GameLocation;
                                                    this.mSupcomGameManager = new SupComGameManager();
                                                    this.mSupcomGameManager.WatchReplay("", queryDataSafe[0][0]);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DlgMessage.Show(this, Loc.Get("<LOC>Unable to view replay."));
                                    }
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            }, new object[] { url });
                        };
                    }
                    ChatLink.LiveReplay.Click += handler14;
                    ChatLink.Content.Click += delegate (ChatLink sender, string url) {
                        DlgContentDetailsView view = DlgContentDetailsView.CreateOrGetExisting(ContentLinkMask.GetContentID(sender));
                        view.Show();
                        view.BringToFront();
                    };
                    if (handler15 == null)
                    {
                        handler15 = delegate (ChatLink sender, string url) {
                            int contentID = TournamentLinkMask.GetContentID(sender);
                            DlgTournamentRegistration registration = this.LocateDialog(typeof(DlgTournamentRegistration)) as DlgTournamentRegistration;
                            if (registration == null)
                            {
                                registration = new DlgTournamentRegistration(contentID);
                                registration.Show();
                            }
                            else
                            {
                                registration.RefreshDates(contentID);
                                registration.Visible = true;
                                registration.BringToFront();
                            }
                        };
                    }
                    ChatLink.Tournament.Click += handler15;
                    if (handler16 == null)
                    {
                        handler16 = delegate (object sender, EventArgs e) {
                            this.ChatRowHeights.Clear();
                            this.ChatRowPoints.Clear();
                            foreach (ChatLine line in this.mChatLines)
                            {
                                line.ContainsEmotes = null;
                            }
                            this.RefreshEmoteAnimations();
                            this.gvChat.RefreshData();
                        };
                    }
                    Emote.EmotesChanged += handler16;
                    if (handler17 == null)
                    {
                        handler17 = delegate (object s, EventArgs e) {
                            this.ChatRowPoints.Clear();
                            this.ChatRowHeights.Clear();
                        };
                    }
                    this.gpgChatGrid.SizeChanged += handler17;
                    this.ChatSyncTimer = new System.Timers.Timer((double) ConfigSettings.GetInt("ChatSyncTimer", 0x493e0));
                    this.ChatSyncTimer.AutoReset = true;
                    this.ChatSyncTimer.Elapsed += new ElapsedEventHandler(this.SyncChatroom);
                    if (handler18 == null)
                    {
                        handler18 = delegate (object s, EventArgs e) {
                            if (!this.IsSortingChatroom && !this.BetweenChannels)
                            {
                                this.IsSortingChatroom = true;
                                try
                                {
                                    while (this.IsSynchronizingChatroom)
                                    {
                                        Thread.Sleep(20);
                                    }
                                    GPG.Logging.EventLog.WriteLine("Chatroom queue emptied, sorting chatroom.", new object[0]);
                                    this.pnlUserListChat.RefreshData();
                                    GPG.Logging.EventLog.WriteLine("Chatroom queue sorted and refreshed.", new object[0]);
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                                finally
                                {
                                    this.IsSortingChatroom = false;
                                }
                            }
                        };
                    }
                    this.ChatroomQueue.Emptied += handler18;
                    if (handler19 == null)
                    {
                        handler19 = delegate (object s, EventArgs e) {
                            this.ChatroomQueue.Enqueue((VGen0)delegate {
                            }, new object[0]);
                        };
                    }
                    this.pnlUserListChat.RequestRefresh += handler19;
                    this._AddUser = new VGen(this.AddUser);
                    this._UpdateUser = new VGen(this.UpdateUser);
                    this._RemoveUser = new VGen(this.RemoveUser);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }, new object[0]);
        }

        private void PreLoadFriends()
        {
        }

        private void PrivateChatClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.ShuttingDown && ((sender != null) && (sender is FrmPrivateChat)))
            {
                Messaging.SendCustomCommand((sender as FrmPrivateChat).ChatTarget.Name, CustomCommands.EndPrivateMessage, new object[0]);
                this.PrivateChats.Remove((sender as FrmPrivateChat).ChatTarget.Name);
                sender = null;
            }
        }

        private void ProcessCustomCommand(CustomCommands cmd, int senderID, string senderName, string[] args)
        {
            Exception exception;
            VGen0 method = null;
            VGen0 target = null;
            try
            {
                User user;
                string str2;
                object[] objArray;
                int num3;
                bool flag;
                ClanMember current;
                string str3;
                string[] strArray3;
                string str7;
                CustomCommands commands = cmd;
                if (commands <= CustomCommands.TeamGameEnd)
                {
                    string str6;
                    switch (commands)
                    {
                        case CustomCommands.UpdateSender:
                            if (senderID != User.Current.ID)
                            {
                                goto Label_0447;
                            }
                            this.RemoveChatParticipant(User.Current.Name);
                            this.AddChatParticipant(User.Current);
                            goto Label_0458;

                        case CustomCommands.OnlineStatus:
                            this.SetOnlineStatus(senderName, Convert.ToInt32(args[0]) > 0);
                            return;

                        case ((CustomCommands) 4):
                        case CustomCommands.SetChatEffect:
                        case CustomCommands.UpdateClanMember:
                        case CustomCommands.InviteToClan:
                        case CustomCommands.RequestClanInvite:
                        case CustomCommands.FriendInvite:
                        case CustomCommands.Whisper:
                        case CustomCommands.Ban:
                        case CustomCommands.SystemEvent:
                        case CustomCommands.GameEvent:
                        case (CustomCommands.SetStatus | CustomCommands.EndPrivateMessage):
                        case (CustomCommands.SetChatEffect | CustomCommands.EndPrivateMessage):
                        case (CustomCommands.SetChatEffect | CustomCommands.SystemMessage | CustomCommands.EndPrivateMessage):
                        case (CustomCommands.EmoteUnavailable | CustomCommands.SystemMessage):
                        case (CustomCommands.EmoteUnavailable | CustomCommands.UpdateSender):
                        case (CustomCommands.EmoteUnavailable | CustomCommands.SystemMessage | CustomCommands.UpdateSender):
                        case ((CustomCommands) 0x2c):
                        case (CustomCommands.EmoteUnavailable | CustomCommands.SetStatus):
                        case (CustomCommands.EmoteUnavailable | CustomCommands.SetChatEffect):
                        case (CustomCommands.EmoteUnavailable | CustomCommands.SetChatEffect | CustomCommands.SystemMessage):
                        case (CustomCommands.Unknown | CustomCommands.EndPrivateMessage | CustomCommands.Kick):
                        case (CustomCommands.SystemMessage | CustomCommands.EndPrivateMessage | CustomCommands.Kick):
                        case (CustomCommands.SetStatus | CustomCommands.EndPrivateMessage | CustomCommands.Kick):
                        case (CustomCommands.SetChatEffect | CustomCommands.EndPrivateMessage | CustomCommands.Kick):
                        case (CustomCommands.SetChatEffect | CustomCommands.SystemMessage | CustomCommands.EndPrivateMessage | CustomCommands.Kick):
                        case (CustomCommands.RequestEmotes | CustomCommands.EndPrivateMessage):
                        case (CustomCommands.RequestEmotes | CustomCommands.SystemMessage | CustomCommands.EndPrivateMessage):
                        case (CustomCommands.RequestEmotes | CustomCommands.UpdateSender | CustomCommands.EndPrivateMessage):
                        case (CustomCommands.RequestEmotes | CustomCommands.SystemMessage | CustomCommands.UpdateSender | CustomCommands.EndPrivateMessage):
                        case CustomCommands.AutomatchRequestAlliance:
                        case CustomCommands.AutomatchConfirmAlliance:
                        case CustomCommands.AutomatchCancelAlliance:
                        case CustomCommands.AutomatchEndAlliance:
                        case CustomCommands.AutomatchStatusMessage:
                        case CustomCommands.AutomatchDecline:
                        case CustomCommands.AutomatchAcknowledgeTeam:
                        case CustomCommands.AutomatchTeamMembers:
                        case (CustomCommands.AutomatchTeamMembers | CustomCommands.SystemMessage):
                        case (CustomCommands.AutomatchTeamMembers | CustomCommands.UpdateSender):
                        case (CustomCommands.AutomatchTeamMembers | CustomCommands.SystemMessage | CustomCommands.UpdateSender):
                        case ((CustomCommands) 0x94):
                        case (CustomCommands.AutomatchTeamMembers | CustomCommands.SetStatus):
                            goto Label_12EC;

                        case CustomCommands.SetStatus:
                            goto Label_077A;

                        case CustomCommands.UpdateClan:
                            if (args.Length <= 0)
                            {
                                goto Label_0918;
                            }
                            if (args.Length <= 1)
                            {
                                goto Label_08FF;
                            }
                            objArray = new object[args.Length - 1];
                            num3 = 1;
                            goto Label_08D7;

                        case CustomCommands.ClanMemberOnline:
                            goto Label_0980;

                        case CustomCommands.RefreshFriends:
                            if (args.Length <= 0)
                            {
                                goto Label_0AA4;
                            }
                            if (args.Length <= 1)
                            {
                                goto Label_0A8F;
                            }
                            objArray = new object[args.Length - 1];
                            num3 = 1;
                            goto Label_0A62;

                        case CustomCommands.RemoveFriend:
                            this.RemoveFriend(senderName, senderID, false);
                            return;

                        case CustomCommands.Kick:
                            this.OnRecieveKick(senderName);
                            return;

                        case CustomCommands.AutomatchRequest:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchRequest(senderID, senderName);
                            return;

                        case CustomCommands.AutomatchAccept:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchAccept(senderID, senderName);
                            return;

                        case CustomCommands.AutomatchAcknowledge:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchAcknowledge(senderID, senderName);
                            return;

                        case CustomCommands.AutomatchConfirm:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchConfirm(senderID, senderName);
                            return;

                        case CustomCommands.AutomatchBusy:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchBusy(senderID, senderName);
                            return;

                        case CustomCommands.AutomatchLaunch:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchLaunch(senderID, senderName, args[0], Convert.ToInt32(args[1]), args[2], args[3]);
                            return;

                        case CustomCommands.RequestEmotes:
                        {
                            if ((args.Length <= 0) || !Emote.AllEmotes.ContainsKey(args[0]))
                            {
                                goto Label_0B5A;
                            }
                            Emote data = Emote.AllEmotes[args[0]];
                            Messaging.SendData(senderName, data);
                            return;
                        }
                        case CustomCommands.NetData:
                            if ((args != null) && (args.Length >= 4))
                            {
                                int dataCode = Convert.ToInt32(args[0]);
                                int chunk = Convert.ToInt32(args[1]);
                                int totalChunks = Convert.ToInt32(args[2]);
                                string str5 = args[3];
                                Messaging.RecieveData(senderName, dataCode, chunk, totalChunks, str5);
                            }
                            return;

                        case CustomCommands.EventExit:
                            if ((args != null) && (args.Length > 0))
                            {
                                str6 = args[0];
                                this.DoPlayerExit(str6);
                            }
                            return;

                        case CustomCommands.EventOperator:
                            if ((args != null) && (args.Length > 0))
                            {
                                str6 = args[0];
                                this.DoPlayerExit(str6);
                            }
                            return;

                        case CustomCommands.EventDisconnect:
                            if ((args != null) && (args.Length > 0))
                            {
                                str6 = args[0];
                                this.DoPlayerExit(str6);
                            }
                            return;

                        case CustomCommands.ChangeLocation:
                            goto Label_07F7;

                        case CustomCommands.EndPrivateMessage:
                            this.PMTargets.Remove(senderName);
                            return;

                        case CustomCommands.UpdateUser:
                            goto Label_05D9;

                        case CustomCommands.EnterChannel:
                            goto Label_06AB;

                        case CustomCommands.AddUser:
                            goto Label_0630;

                        case CustomCommands.ChangeName:
                            goto Label_0497;

                        case CustomCommands.EmoteUnavailable:
                            if (args.Length > 0)
                            {
                                this.SystemMessage("<LOC>The emote '{0}' is no longer available from {1}.", new object[] { args[0], senderName });
                            }
                            return;

                        case CustomCommands.PingRequest:
                            Messaging.SendCustomCommand(senderName, CustomCommands.PingResponse, new object[0]);
                            return;

                        case CustomCommands.PingResponse:
                            this.PingResponse(senderName);
                            return;

                        case CustomCommands.PlayerMessage:
                            if ((args.Length <= 0) || !this.TryFindUser(senderName, out user))
                            {
                                return;
                            }
                            str2 = args[0];
                            if (args.Length <= 1)
                            {
                                goto Label_03F6;
                            }
                            objArray = new object[args.Length - 1];
                            num3 = 1;
                            goto Label_03C9;

                        case CustomCommands.AM1:
                            if (args.Length > 0)
                            {
                                this.SetUserVisibility(senderName, bool.Parse(args[0]));
                            }
                            return;

                        case CustomCommands.AutomatchNotifyAllies:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchNotifyAllies(senderID, senderName, args[0], Convert.ToInt32(args[1]), args[2]);
                            return;

                        case CustomCommands.AutomatchTeamName:
                            if (args.Length >= 2)
                            {
                                SupcomAutomatch.GetSupcomAutomatch().AutomatchNotifyOpponentTeam(senderID, senderName, args[0], args[1]);
                            }
                            return;

                        case CustomCommands.AutomatchDirectChallenge:
                            this.RequestDirectChallenge(senderName);
                            return;

                        case CustomCommands.AutomatchConfirmChallenge:
                            this.ConfirmChallenge(senderName, args[0]);
                            return;

                        case CustomCommands.AutomatchAbortChallenge:
                            SupcomAutomatch.GetSupcomAutomatch().AutomatchAbortChallenge(senderID, senderName);
                            return;

                        case CustomCommands.TeamGameMessage:
                            if ((this.DlgTeamGame != null) && (args.Length > 0))
                            {
                                this.DlgTeamGame.OnMessageRecieved(senderName, args[0]);
                            }
                            return;

                        case CustomCommands.TeamGame:
                            if (args.Length > 0)
                            {
                                this.ReceiveTeamGame(args[0]);
                            }
                            return;

                        case CustomCommands.TeamGameMember:
                            if ((this.DlgTeamGame != null) && (args.Length > 0))
                            {
                                this.DlgTeamGame.UpdateMember(args[0]);
                            }
                            return;

                        case CustomCommands.TeamGameLaunch:
                            if (this.DlgTeamGame != null)
                            {
                                this.DlgTeamGame.GameLaunched = true;
                            }
                            return;

                        case CustomCommands.TeamGameDisband:
                            if (this.DlgTeamGame != null)
                            {
                                this.DlgTeamGame.DisbandTeam();
                            }
                            return;

                        case CustomCommands.TeamGameKick:
                            if (this.DlgTeamGame != null)
                            {
                                if (args.Length <= 0)
                                {
                                    goto Label_11A3;
                                }
                                str7 = args[0];
                                if (!(str7.ToLower() == User.Current.Name.ToLower()))
                                {
                                    goto Label_117B;
                                }
                                this.DlgTeamGame.ReceiveKick();
                            }
                            return;

                        case CustomCommands.TeamGameLeave:
                            if (this.DlgTeamGame != null)
                            {
                                this.DlgTeamGame.RemoveMember(senderName, "<LOC>{0} has left the team game.", new object[] { senderName });
                            }
                            return;

                        case CustomCommands.TeamGameInvite:
                            this.ReceiveTeamInvite(senderName);
                            return;

                        case CustomCommands.TeamGameAcceptInvite:
                            if (this.DlgTeamGame == null)
                            {
                                goto Label_0ED7;
                            }
                            this.DlgTeamGame.AddMember(senderName, senderID);
                            return;

                        case CustomCommands.TeamGameDeclineInvite:
                            if (this.DlgTeamGame == null)
                            {
                                return;
                            }
                            if ((args == null) || (args.Length <= 0))
                            {
                                goto Label_0F92;
                            }
                            str2 = args[0];
                            strArray3 = new string[args.Length - 1];
                            num3 = 1;
                            goto Label_0F6C;

                        case CustomCommands.TeamGameFull:
                            this.SystemMessage("<LOC>{0}'s game is now {1} and you can no longer join it.", new object[] { senderName, "full" });
                            return;

                        case CustomCommands.TeamGameMap:
                            if ((this.DlgTeamGame != null) && (args.Length == 2))
                            {
                                this.DlgTeamGame.ChangeMaps(args[0], args[1]);
                            }
                            return;

                        case CustomCommands.TeamGameStatus:
                            if ((this.DlgTeamGame != null) && (args.Length > 0))
                            {
                                string status = args[0];
                                strArray3 = new string[args.Length - 1];
                                Array.ConstrainedCopy(args, 1, strArray3, 0, strArray3.Length);
                                this.DlgTeamGame.SetStatus(status, strArray3);
                            }
                            return;

                        case CustomCommands.TeamGameAbort:
                            if (this.DlgTeamGame != null)
                            {
                                this.DlgTeamGame.GameLaunched = false;
                            }
                            return;

                        case CustomCommands.TeamGameUnavailable:
                            this.SystemMessage("<LOC>{0}'s game is now {1} and you can no longer join it.", new object[] { senderName, "unavailable" });
                            return;

                        case CustomCommands.TeamGameStart:
                            this.OnTeamAutomatchLaunchGame();
                            return;

                        case CustomCommands.TeamGameEnd:
                            this.OnTeamAutomatchExit();
                            return;
                    }
                }
                else
                {
                    int num;
                    switch (commands)
                    {
                        case CustomCommands.LadderChallengeRequest:
                            num = int.Parse(args[0]);
                            this.RecieveLadderChallengeRequest(senderName, num);
                            return;

                        case CustomCommands.LadderChallengeAccept:
                        {
                            string sessionId = args[0];
                            num = int.Parse(args[1]);
                            this.BeginLadderChallengeMatch(sessionId, num);
                            return;
                        }
                        case CustomCommands.LadderChallengeDecline:
                            if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        if ((args != null) && (args.Length > 0))
                                        {
                                            string format = args[0];
                                            string[] strArray = new string[args.Length - 1];
                                            for (int j = 1; j < args.Length; j++)
                                            {
                                                strArray[j - 1] = Loc.Get(args[j]);
                                            }
                                            DlgMessage.ShowDialog(string.Format(format, (object[]) strArray));
                                        }
                                        else
                                        {
                                            DlgMessage.ShowDialog(string.Format("<LOC>{0} has declined your ladder challenge.", senderName));
                                        }
                                    };
                                }
                                base.BeginInvoke(method);
                            }
                            return;

                        case CustomCommands.GameLobbyMessage:
                            if ((args != null) && (args.Length >= 1))
                            {
                                uint message = uint.Parse(args[0]);
                                string[] destinationArray = new string[args.Length - 1];
                                Array.ConstrainedCopy(args, 1, destinationArray, 0, destinationArray.Length);
                                this.RecieveGameLobbyMessage(senderName, senderID, message, destinationArray);
                            }
                            return;

                        case CustomCommands.AutomatchAbortDirectChallenge:
                            this.AbortChallenge(senderName, args[0]);
                            return;
                    }
                }
                goto Label_12EC;
            Label_03AB:
                objArray[num3 - 1] = Loc.Get(args[num3]);
                num3++;
            Label_03C9:
                if (num3 < args.Length)
                {
                    goto Label_03AB;
                }
                this.AddChat(user, string.Format(Loc.Get(str2), objArray));
                return;
            Label_03F6:
                this.AddChat(user, Loc.Get(str2));
                return;
            Label_0447:
                this.RefreshChatParticipant(senderName, true);
            Label_0458:
                if (args.Length > 0)
                {
                    this.SetUserStatus(senderName, args[0]);
                }
                this.pnlUserListClan.RefreshData();
                return;
            Label_0497:
                flag = false;
                foreach (User user2 in User.CurrentFriends)
                {
                    if (user2.ID == senderID)
                    {
                        this.SystemMessage("<LOC>Your friend {0} is now known as {1}. This change has been automatically updated on your system.", new object[] { user2.Name, senderName });
                        flag = true;
                        user2.Name = senderName;
                        this.FrmMain_Friends();
                        break;
                    }
                }
                using (IEnumerator<ClanMember> enumerator2 = Clan.CurrentMembers.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        current = enumerator2.Current;
                        if (current.ID == senderID)
                        {
                            if (!flag)
                            {
                                this.SystemMessage("<LOC>Your clan member {0} is now known as {1}. This change has been automatically updated on your system.", new object[] { current.Name, senderName });
                            }
                            current.Name = senderName;
                            this.FrmMain_Clan();
                            return;
                        }
                    }
                }
                return;
            Label_05D9:;
                try
                {
                    if (args.Length > 0)
                    {
                        this.ChatroomQueue.Enqueue(this._UpdateUser, new object[] { args });
                    }
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                }
                return;
            Label_0630:;
                try
                {
                    if (args.Length > 0)
                    {
                        this.ChatroomQueue.Enqueue(this._AddUser, new object[] { args });
                    }
                    else
                    {
                        ErrorLog.WriteLine("Malformed Add User Command from: {0}", new object[] { senderName });
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    ErrorLog.WriteLine(exception);
                }
                return;
            Label_06AB:;
                try
                {
                    if (args.Length > 0)
                    {
                        this.ChatroomQueue.Enqueue(this._AddUser, new object[] { args });
                    }
                    else
                    {
                        ErrorLog.WriteLine("Malformed Enter Channel Command from: {0}", new object[] { senderName });
                    }
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    ErrorLog.WriteLine(exception);
                }
                return;
            Label_077A:
                str3 = args[0];
                if (this.PlayerChatEffects.ContainsKey(senderName) && (this.PlayerChatEffects[senderName].Status.Name == str3))
                {
                    str3 = "none";
                }
                this.SetUserStatus(senderName, str3);
                this.RecieveChatEffect(senderName, str3);
                return;
            Label_07F7:;
                try
                {
                    if (args.Length == 2)
                    {
                        GPG.Multiplayer.Client.LocationTypes type = (GPG.Multiplayer.Client.LocationTypes) Convert.ToUInt32(args[0]);
                        string str4 = args[1];
                        this.ChangeLocation(senderName, new Location(type, str4));
                    }
                }
                catch (Exception exception4)
                {
                    exception = exception4;
                    ErrorLog.WriteLine(exception);
                }
                return;
            Label_08B9:
                objArray[num3 - 1] = Loc.Get(args[num3]);
                num3++;
            Label_08D7:
                if (num3 < args.Length)
                {
                    goto Label_08B9;
                }
                this.SystemMessage(args[0], objArray);
                goto Label_0918;
            Label_08FF:
                this.SystemMessage(args[0], new object[0]);
            Label_0918:
                if (Clan.Current != null)
                {
                    string[] currentMembersMsgList = Clan.CurrentMembersMsgList;
                    this.FrmMain_Clan();
                    if (Clan.Current == null)
                    {
                        num3 = 0;
                        while (num3 < currentMembersMsgList.Length)
                        {
                            this.RefreshChatParticipant(currentMembersMsgList[num3]);
                            num3++;
                        }
                    }
                }
                else
                {
                    this.FrmMain_Clan();
                }
                return;
            Label_0980:
                if ((Clan.Current != null) && Clan.CurrentMembers.ContainsIndex("name", senderName))
                {
                    bool flag2 = Convert.ToInt32(args[0]) > 0;
                    if (Clan.CurrentMembers.TryFindByIndex("name", senderName, out current))
                    {
                        current.Online = flag2;
                        this.pnlUserListClan.RefreshData();
                    }
                }
                return;
            Label_0A44:
                objArray[num3 - 1] = Loc.Get(args[num3]);
                num3++;
            Label_0A62:
                if (num3 < args.Length)
                {
                    goto Label_0A44;
                }
                this.UpdateFriends(string.Format(args[0], objArray));
                goto Label_0AAE;
            Label_0A8F:
                this.UpdateFriends(args[0]);
                goto Label_0AAE;
            Label_0AA4:
                this.UpdateFriends(null);
            Label_0AAE:
                if (target == null)
                {
                    target = delegate {
                        this.ChatContainers.RemoveItem(senderName);
                        this.AddChatParticipant(DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { senderName })[0]);
                        this.pnlUserListClan.RefreshData();
                    };
                }
                this.ChatroomQueue.Enqueue(target, new object[0]);
                return;
            Label_0B5A:;
                Messaging.SendCustomCommand(senderName, CustomCommands.EmoteUnavailable, new object[] { args[0] });
                return;
            Label_0ED7:
                Messaging.SendCustomCommand(senderName, CustomCommands.TeamGameUnavailable, new object[0]);
                return;
            Label_0F4E:
                strArray3[num3 - 1] = Loc.Get(args[num3]);
                num3++;
            Label_0F6C:
                if (num3 < args.Length)
                {
                    goto Label_0F4E;
                }
                this.DlgTeamGame.SystemMessage(str2, strArray3);
                return;
            Label_0F92:;
                this.DlgTeamGame.SystemMessage("<LOC>{0} has declined your request to join your team.", new object[] { senderName });
                return;
            Label_117B:;
                this.DlgTeamGame.RemoveMember(str7, "<LOC>{0} has been kicked out of your team game.", new object[] { str7 });
                return;
            Label_11A3:
                this.DlgTeamGame.ReceiveKick();
                return;
            Label_12EC:
                if ((cmd >= CustomCommands.TournamentLaunchGame) && (cmd <= CustomCommands.TournamentReservedCmd9))
                {
                    TournamentCommands.ProcessCustomCommand(this, cmd, senderID, senderName, args);
                }
                else if (User.IsUserIgnored(senderID))
                {
                    if (cmd != CustomCommands.GameEvent)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.SystemMessage, new object[] { "<LOC>{0} is not receiving your messages.", User.Current });
                    }
                }
                else if (User.Current.IsDND && !this.CheckDNDOverride(senderName))
                {
                    if (cmd != CustomCommands.GameEvent)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.SystemMessage, new object[] { "<LOC>Could not send message to {0} because user is in Do Not Disturb mode.", User.Current });
                    }
                }
                else if (this.mIsInGame)
                {
                    if (cmd != CustomCommands.GameEvent)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.SystemMessage, new object[] { "<LOC>{0} is playing a game and not receiving messages at this time.", User.Current });
                    }
                }
                else
                {
                    switch (cmd)
                    {
                        case CustomCommands.AutomatchRequestAlliance:
                            DlgSupcomTeamSelection.ProcessCommand(this, cmd, senderID, senderName, args);
                            return;

                        case CustomCommands.AutomatchConfirmAlliance:
                            DlgSupcomTeamSelection.ProcessCommand(this, cmd, senderID, senderName, args);
                            return;

                        case CustomCommands.AutomatchCancelAlliance:
                            DlgSupcomTeamSelection.ProcessCommand(this, cmd, senderID, senderName, args);
                            return;

                        case CustomCommands.AutomatchEndAlliance:
                            DlgSupcomTeamSelection.ProcessCommand(this, cmd, senderID, senderName, args);
                            return;

                        case CustomCommands.AutomatchStatusMessage:
                            DlgSupcomTeamSelection.ProcessCommand(this, cmd, senderID, senderName, args);
                            return;

                        case CustomCommands.AutomatchTeamName:
                        case CustomCommands.AutomatchDirectChallenge:
                        case CustomCommands.AutomatchConfirmChallenge:
                        case CustomCommands.AutomatchAbortChallenge:
                        case CustomCommands.UpdateClan:
                        case CustomCommands.UpdateClanMember:
                        case CustomCommands.ClanMemberOnline:
                            return;

                        case CustomCommands.AutomatchDecline:
                            DlgSupcomTeamSelection.ProcessCommand(this, cmd, senderID, senderName, args);
                            return;

                        case CustomCommands.AutomatchAcknowledgeTeam:
                            DlgSupcomTeamSelection.ProcessCommand(this, cmd, senderID, senderName, args);
                            return;

                        case CustomCommands.AutomatchTeamMembers:
                            if (args.Length == 2)
                            {
                                this.SetStatus(Loc.Get(args[0]) + args[1], new object[0]);
                            }
                            return;

                        case CustomCommands.GameEvent:
                            if (args.Length > 0)
                            {
                                str2 = args[0];
                                if (args.Length <= 1)
                                {
                                    goto Label_16B5;
                                }
                                objArray = new object[args.Length - 1];
                                num3 = 1;
                                while (num3 < args.Length)
                                {
                                    objArray[num3 - 1] = Loc.Get(args[num3]);
                                    num3++;
                                }
                                this.GameEvent(str2, objArray);
                            }
                            return;

                        case CustomCommands.SystemEvent:
                            if (args.Length > 0)
                            {
                                str2 = args[0];
                                if (args.Length > 1)
                                {
                                    objArray = new object[args.Length - 1];
                                    for (num3 = 1; num3 < args.Length; num3++)
                                    {
                                        objArray[num3 - 1] = Loc.Get(args[num3]);
                                    }
                                    this.SystemEvent(str2, objArray);
                                }
                                else
                                {
                                    this.SystemEvent(str2, new object[0]);
                                }
                            }
                            return;

                        case CustomCommands.SetChatEffect:
                            this.SetChatEffect(senderName, args[0]);
                            return;

                        case CustomCommands.InviteToClan:
                        {
                            int clanId = Convert.ToInt32(args[0]);
                            string clanName = args[1];
                            string clanAbbr = args[2];
                            this.RecieveClanInvite(senderName, senderID, clanId, clanName, clanAbbr);
                            return;
                        }
                        case CustomCommands.RequestClanInvite:
                            this.RecieveClanInviteRequest(senderName, senderID);
                            return;

                        case CustomCommands.FriendInvite:
                            this.RecieveFriendRequest(senderName, senderID);
                            return;

                        case CustomCommands.Whisper:
                            goto Label_1737;

                        case CustomCommands.SystemMessage:
                            if (args.Length > 0)
                            {
                                str2 = args[0];
                                if (args.Length > 1)
                                {
                                    objArray = new object[args.Length - 1];
                                    for (num3 = 1; num3 < args.Length; num3++)
                                    {
                                        objArray[num3 - 1] = Loc.Get(args[num3]);
                                    }
                                    this.SystemMessage(str2, objArray);
                                }
                                else
                                {
                                    this.SystemMessage(str2, new object[0]);
                                }
                            }
                            return;
                    }
                }
                return;
            Label_16B5:
                this.GameEvent(str2, new object[0]);
                return;
            Label_1737:
                str2 = null;
                if (args.Length > 0)
                {
                    str2 = args[0];
                }
                this.OnRecieveWhisper(senderName, str2);
            }
            catch (Exception exception5)
            {
                exception = exception5;
                ErrorLog.WriteLine(exception);
            }
        }

        private void ProcessSystemCommand(string cmd, string[] args)
        {
            try
            {
                string str5;
                string str7 = cmd.ToLower();
                if (str7 != null)
                {
                    if (!(str7 == "event"))
                    {
                        if (str7 == "cust")
                        {
                            goto Label_01E2;
                        }
                        if (str7 == "notifyjoin")
                        {
                            goto Label_020D;
                        }
                        if (str7 == "notifyexit")
                        {
                            goto Label_0249;
                        }
                        if (str7 == "dnd")
                        {
                            goto Label_0286;
                        }
                        if (str7 == "disconnect")
                        {
                            goto Label_0290;
                        }
                    }
                    else
                    {
                        string[] strArray = args[0].Split(",=".ToCharArray());
                        if (strArray.Length > 9)
                        {
                            int num = Convert.ToInt32(strArray[1]);
                            string str = strArray[3];
                            string str2 = strArray[5];
                            string str3 = strArray[7];
                            string str4 = strArray[9];
                            int num2 = Convert.ToInt32(strArray[11]);
                            GPG.Logging.EventLog.WriteLine(args[0].ToString(), LogCategory.Get("//EVENT"), new object[0]);
                            try
                            {
                                User participantByID;
                                if (((num == 3) && ((str4 == "2") || (str4 == "7"))) && (str3 != User.Current.ID.ToString()))
                                {
                                    participantByID = Chatroom.GetParticipantByID(Convert.ToInt32(str3));
                                    if (participantByID != null)
                                    {
                                        Messaging.SendCustomCommand(CustomCommands.EventDisconnect, new object[] { participantByID.Name });
                                        this.DoPlayerExit(participantByID.Name);
                                    }
                                }
                                if ((num == 4) && (str4 == "0"))
                                {
                                    participantByID = Chatroom.GetParticipantByID(Convert.ToInt32(str3));
                                    if (participantByID != null)
                                    {
                                        Chatroom.RoomOperator = participantByID.Name;
                                        this.RefreshChatParticipant(participantByID.Name, false);
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        }
                    }
                }
                return;
            Label_01E2:
                if (Convert.ToInt32(args[1]) != User.Current.ID)
                {
                    this.ParseCustomCommand(cmd, args);
                }
                return;
            Label_020D:
                str5 = args[0];
                string playerName = args[1];
                if (playerName.ToUpper() != User.Current.Name.ToUpper())
                {
                    this.DoPlayerJoin(playerName);
                }
                return;
            Label_0249:
                str5 = args[0];
                playerName = args[1];
                if (playerName.ToUpper() != User.Current.Name.ToUpper())
                {
                    this.DoPlayerExit(args[1]);
                }
                return;
            Label_0286:
                this.ToggleDNDStatus();
                return;
            Label_0290:
                this.SystemMessage(Loc.Get("<LOC>*** YOU HAVE LOST YOUR GPGNET CONNECTION ***"), new object[0]);
            }
            catch (Exception exception2)
            {
                ErrorLog.WriteLine(exception2);
            }
        }

        internal void ProcessUserAction(UserActionCommands action, string[] args)
        {
            Exception exception;
            try
            {
                string str;
                int num;
                string str2;
                string str3;
                string str4;
                string str5;
                string[] strArray;
                int num2;
                switch (action)
                {
                    case UserActionCommands.DoNotDisturb:
                        this.ToggleDNDStatus();
                        return;

                    case UserActionCommands.Away:
                        this.SetAwayStatus(true);
                        return;

                    case UserActionCommands.Friend:
                        this.InviteAsFriend(args[0]);
                        return;

                    case UserActionCommands.Unfriend:
                        this.RemoveFriend(args[0]);
                        return;

                    case UserActionCommands.Whisper:
                        str = "";
                        num = 1;
                        goto Label_00FB;

                    case UserActionCommands.Kick:
                        this.OnSendKick(args[0]);
                        return;

                    case UserActionCommands.Ban:
                        this.BanUser(args[0]);
                        return;

                    case UserActionCommands.Chatroom:
                        str2 = "";
                        num = 0;
                        goto Label_014D;

                    case UserActionCommands.ViewPlayer:
                        this.OnViewPlayerProfile(args[0]);
                        return;

                    case UserActionCommands.ClanInvite:
                        this.InviteToClan(args[0]);
                        return;

                    case UserActionCommands.ClanRequest:
                        this.RequestClanInvite(args[0]);
                        return;

                    case UserActionCommands.LeaveClan:
                        this.OnLeaveClan();
                        return;

                    case UserActionCommands.CreateClan:
                        this.OnCreateClan();
                        return;

                    case UserActionCommands.ViewClan:
                        this.OnViewClanProfileByPlayer(args[0]);
                        return;

                    case UserActionCommands.ClanPromote:
                        this.PromoteClanMember(args[0]);
                        return;

                    case UserActionCommands.ClanDemote:
                        this.DemoteClanMember(args[0]);
                        return;

                    case UserActionCommands.ClanRemove:
                        this.RemoveClanMember(args[0]);
                        return;

                    case UserActionCommands.Chicken:
                        if (args.Length != 0)
                        {
                            goto Label_0395;
                        }
                        ChatEffects.Current = ChatEffects.Chicken;
                        this.SystemMessage("You are now a chicken.", new object[0]);
                        Messaging.SendCustomCommand(CustomCommands.SetStatus, new object[] { UserStatus.Chicken.Name });
                        return;

                    case UserActionCommands.Canadian:
                        if (args.Length != 0)
                        {
                            goto Label_02E6;
                        }
                        ChatEffects.Current = ChatEffects.Canadian;
                        this.SystemMessage("You are now a Canadian (if you weren't already).", new object[0]);
                        Messaging.SendCustomCommand(CustomCommands.SetStatus, new object[] { UserStatus.Canadian.Name });
                        return;

                    case UserActionCommands.Exit:
                        this.ExitApplication();
                        return;

                    case UserActionCommands.Unban:
                        this.UnbanUser(args[0]);
                        return;

                    case UserActionCommands.Ignore:
                        this.IgnorePlayer(args[0]);
                        return;

                    case UserActionCommands.Unignore:
                        this.UnignorePlayer(args[0]);
                        return;

                    case UserActionCommands.Emote:
                        str = "";
                        strArray = args;
                        num2 = 0;
                        goto Label_042C;

                    case UserActionCommands.Stats:
                        if (User.Current.IsAdmin)
                        {
                            this.SystemMessage("*** STATISTICS DUMP ***", new object[0]);
                            ArrayList logData = DataAccess.GetLogData();
                            foreach (object obj2 in logData)
                            {
                                this.SystemMessage(obj2.ToString(), new object[0]);
                            }
                            this.SystemMessage("*** STATISTICS DUMP ***", new object[0]);
                            this.SystemMessage("DB Calls: " + logData.Count.ToString(), new object[0]);
                            this.SystemMessage("*** STATISTICS DUMP ***", new object[0]);
                        }
                        return;

                    case UserActionCommands.Translate:
                        if (!User.Current.IsAdmin)
                        {
                            return;
                        }
                        str4 = args[0];
                        str5 = "";
                        num = 1;
                        goto Label_0562;

                    case UserActionCommands.Locate:
                        if ((args != null) && (args.Length > 0))
                        {
                            this.LocatePlayer(args[0]);
                        }
                        return;

                    case UserActionCommands.TagTranslate:
                        Messaging.AutoTranslate(args[0], args[1]);
                        return;

                    case UserActionCommands.IgnoreDirectChallenge:
                        this.ToggleDirectChallenge();
                        return;

                    case UserActionCommands.AllowFriends:
                        this.ToggleAllowFriends();
                        return;

                    case UserActionCommands.IPBan:
                        DataAccess.ExecuteQuery("IP Ban", new object[] { args[0] });
                        this.SystemMessage("Address " + args[0] + " has been banned.", new object[0]);
                        return;

                    case UserActionCommands.IPLookup:
                        this.SystemMessage("Here is the IP address and account list for player " + args[0] + ".", new object[0]);
                        try
                        {
                            foreach (DataRecord record in DataAccess.GetQueryData("IP Player Lookup", new object[] { args[0] }))
                            {
                                this.SystemMessage(record[0] + " " + record[1], new object[0]);
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            ErrorLog.WriteLine(exception);
                        }
                        return;

                    case UserActionCommands.IPBanCheck:
                        this.SystemMessage("Here is the current ban list.", new object[0]);
                        try
                        {
                            foreach (DataRecord record in DataAccess.GetQueryData("IP Banned List", new object[0]))
                            {
                                this.SystemMessage(record[0], new object[0]);
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            ErrorLog.WriteLine(exception);
                        }
                        return;

                    case UserActionCommands.IPUnBan:
                        DataAccess.ExecuteQuery("IP UnBan", new object[] { args[0] });
                        this.SystemMessage("Address " + args[0] + " has been unbanned.", new object[0]);
                        return;

                    default:
                        goto Label_0771;
                }
            Label_00E6:
                str = str + args[num] + " ";
                num++;
            Label_00FB:
                if (num < args.Length)
                {
                    goto Label_00E6;
                }
                str = str.TrimEnd(" ".ToCharArray());
                this.OnSendWhisper(args[0], str);
                return;
            Label_0133:
                str2 = str2 + string.Format("{0} ", args[num]);
                num++;
            Label_014D:
                if (num < args.Length)
                {
                    goto Label_0133;
                }
                str2 = str2.TrimEnd(new char[] { ' ' });
                if ((str2 == null) || (str2.Trim().Length < 1))
                {
                    this.ShowDlgSelectChannel();
                }
                else
                {
                    this.CreateChannelIfNonExist(str2);
                }
                return;
            Label_02E6:
                if (User.Current.IsAdmin)
                {
                    Messaging.SendCustomCommand(args[0], CustomCommands.SetChatEffect, new object[] { ChatEffects.Canadian.Name });
                }
                else
                {
                    this.ErrorMessage(">You do not have permission to make {0} Canadian.", new object[] { args[0] });
                }
                return;
            Label_0395:
                if (User.Current.IsAdmin)
                {
                    Messaging.SendCustomCommand(args[0], CustomCommands.SetChatEffect, new object[] { ChatEffects.Chicken.Name });
                }
                else
                {
                    this.ErrorMessage("You do not have permission to make {0} a chicken.", new object[] { args[0] });
                }
                return;
            Label_0411:
                str3 = strArray[num2];
                str = str + str3 + " ";
                num2++;
            Label_042C:
                if (num2 < strArray.Length)
                {
                    goto Label_0411;
                }
                this.EmoteMessageRoom(str.Trim());
                return;
            Label_054B:
                str5 = str5 + args[num] + " ";
                num++;
            Label_0562:
                if (num < args.Length)
                {
                    goto Label_054B;
                }
                string msg = TranslateUtil.GetTranslatedText(str5, str4, "en");
                this.SendMessage(msg);
                return;
            Label_0771:
                this.SystemMessage("<LOC>Unknown Command.", new object[0]);
            }
            catch (Exception exception3)
            {
                exception = exception3;
                ErrorLog.WriteLine(exception);
            }
        }

        private void progress_OnCancelDownload(object sender, EventArgs e)
        {
            base.Close();
        }

        internal void PromoteClanMember(string memberName)
        {
            if (((Clan.Current == null) || (Clan.CurrentMembers == null)) || (ClanMember.Current == null))
            {
                this.ErrorMessage("<LOC>You are not in a clan.", new object[0]);
            }
            else
            {
                ClanMember member;
                if (!Clan.CurrentMembers.TryFindByIndex("name", memberName, out member))
                {
                    this.ErrorMessage("<LOC>{0} is not a member of your clan.", new object[] { memberName });
                }
                else if (!ClanMember.Current.CanTargetAbility(ClanAbility.Promote, memberName))
                {
                    this.ErrorMessage("<LOC>You do not have sufficient rank within your clan to promote {0}.", new object[] { memberName });
                }
                else if (this.ClanChangeRank(member, member.Rank - 1))
                {
                    this.RefreshClan(string.Format(Loc.Get("<LOC>{0} has promoted {1} to the rank of {2}."), User.Current.Name, memberName, member.GetRanking().Description), "<LOC>{0} has promoted {1} to the rank of {2}.", new object[] { User.Current.Name, memberName, member.GetRanking().Description });
                }
                else
                {
                    this.ErrorMessage("<LOC>A problem was encountered promoting {0}", new object[] { memberName });
                }
            }
        }

        internal void ReceiveTeamGame(string teamGame)
        {
            VGen0 method = null;
            if (this.InviteAccepted)
            {
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.ReceiveTeamGame(teamGame);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!(base.Disposing || base.IsDisposed))
                {
                    this.InviteAccepted = false;
                    SupcomAutomatch.GetSupcomAutomatch().RemoveMatch();
                    SupcomAutomatch.GetSupcomAutomatch().ClearTeams();
                    this.ShowDlgTeamGame(TeamGame.FromDataString(teamGame));
                }
            }
        }

        internal void ReceiveTeamInvite(string sender)
        {
            VGen0 gen = null;
            if (this.InvitePending)
            {
                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>{0} already has a team invite pending.", User.Current.Name });
            }
            else if (!((this.DlgTeamGame == null) || this.DlgTeamGame.IsDisposed))
            {
                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>{0} is already in a team game.", User.Current.Name });
            }
            else if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
            {
                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>{0} is already in the ranked game queue.", User.Current.Name });
            }
            else if (this.mIsInGame)
            {
                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>{0} is playing a game and cannot join your game at this time.", User.Current.Name });
            }
            else if (!(this.IsGameCurrent || User.Current.IsAdmin))
            {
                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>{0} has a different version of the game and cannot join your game.", User.Current.Name });
            }
            else if (User.Current.IsDND)
            {
                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>{0} is in Do Not Disturb mode and cannot join your game.", User.Current.Name });
            }
            else
            {
                DataList queryData = DataAccess.GetQueryData("GetCurrentGame", new object[] { sender });
                if ((queryData.Count == 0) || (queryData[0][0] != GPGnetSelectedGame.SelectedGame.GameID.ToString()))
                {
                    Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>{0} is using game {1} and cannot join your team.", User.Current.Name, GPGnetSelectedGame.SelectedGame.GameDescription });
                }
                else if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (gen == null)
                    {
                        gen = delegate {
                            this.ReceiveTeamInvite(sender);
                        };
                    }
                    base.BeginInvoke(gen);
                }
                else if (!base.Disposing && !base.IsDisposed)
                {
                    this.InvitePending = true;
                    try
                    {
                        DlgYesNo dlg = new DlgYesNo(this, Loc.Get("<LOC>Arranged Team Request"), string.Format(Loc.Get("<LOC>{0} has invited you to play in a rated team game. Do you wish to join?"), sender));
                        ThreadPool.QueueUserWorkItem(delegate (object s) {
                            VGen0 method = null;
                            try
                            {
                                Thread.Sleep((int) (Program.Settings.Chat.PopupTimeout * 0x3e8));
                                if (((dlg != null) && !dlg.Disposing) && !dlg.IsDisposed)
                                {
                                    if (method == null)
                                    {
                                        method = delegate {
                                            try
                                            {
                                                dlg.DialogResult = DialogResult.Cancel;
                                            }
                                            catch (Exception exception)
                                            {
                                                ErrorLog.WriteLine(exception);
                                            }
                                        };
                                    }
                                    this.BeginInvoke(method);
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        });
                        switch (dlg.ShowDialog())
                        {
                            case DialogResult.Yes:
                                this.InviteAccepted = true;
                                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameAcceptInvite, new object[0]);
                                break;

                            case DialogResult.No:
                                this.InviteAccepted = false;
                                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[0]);
                                break;

                            case DialogResult.Cancel:
                                this.InviteAccepted = false;
                                Messaging.SendCustomCommand(sender, CustomCommands.TeamGameDeclineInvite, new object[] { "<LOC>Your team game invite with {0} has timed out and was automatically declined.", User.Current.Name });
                                break;
                        }
                        if (!(dlg.Disposing || dlg.IsDisposed))
                        {
                            dlg.Dispose();
                        }
                        dlg = null;
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                    finally
                    {
                        this.InvitePending = false;
                    }
                }
            }
        }

        private void RecieveChatEffect(string playerName, string effectName)
        {
            if ((effectName == null) || (effectName.Length < 1))
            {
                this.PlayerChatEffects[playerName] = null;
            }
            else
            {
                ChatEffectBase base2;
                if (ChatEffects.TryFindByDescription(effectName, out base2))
                {
                    this.PlayerChatEffects[playerName] = base2;
                }
            }
        }

        private void RecieveClanInvite(string senderName, int senderId, int clanId, string clanName, string clanAbbr)
        {
            VGen0 method = null;
            if ((this.ClanInvites != null) && this.ClanInvites.ContainsIndex("name", senderName))
            {
                Messaging.SendCustomCommand(senderName, CustomCommands.SystemMessage, new object[] { "<LOC>You already have a pending invite with {0}.", User.Current.Name });
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        string msg = string.Format(Loc.Get("<LOC>{0} has invited you to join a clan. You may accept the request, decline it, or save it and decide later."), senderName, clanName, clanAbbr);
                        switch (new DlgYesNoLater(this, "<LOC>Clan Invite", msg, true).ShowDialog(this))
                        {
                            case DialogResult.Ignore:
                            {
                                if (!DataAccess.ExecuteQuery("SaveOfflineClanInvite", new object[] { senderId, clanId }))
                                {
                                    this.ErrorMessage("<LOC>Unable to save invitation, ensure you do not already have a pending request from this target.", new object[0]);
                                    break;
                                }
                                if (this.ClanInvites == null)
                                {
                                    this.ClanInvites = new MappedObjectList<ClanInvite>();
                                }
                                ClanInvite item = new ClanInvite(senderId, senderName, User.Current.ID, User.Current.Name, clanId, clanName, DateTime.Now);
                                this.ClanInvites.Add(item);
                                this.ClanInvites.IndexObject(item);
                                this.RefreshClanInviteCount();
                                this.SystemMessage("<LOC>This invitation has been saved for future action. Please check the Clan tab to review it.", new object[0]);
                                Messaging.SendCustomCommand(senderName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has decided to save your invitation and review it later.", User.Current.Name });
                                break;
                            }
                            case DialogResult.Yes:
                                this.JoinClan(clanId, clanName, clanAbbr);
                                break;

                            default:
                                Messaging.SendCustomCommand(senderName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has declined the offer to join your clan.", User.Current.Name });
                                this.SystemMessage("<LOC>A rejection notification has been sent.", new object[0]);
                                break;
                        }
                    };
                }
                base.BeginInvoke(method);
            }
        }

        private void RecieveClanInviteRequest(string senderName, int senderId)
        {
            VGen0 method = null;
            if ((this.ClanRequests != null) && this.ClanRequests.ContainsIndex("name", senderName))
            {
                Messaging.SendCustomCommand(senderName, CustomCommands.SystemMessage, new object[] { "<LOC>You already have a pending request with {0}.", User.Current.Name });
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        string msg = string.Format(Loc.Get("<LOC>{0} has requested to join your clan. You may accept the request, decline it, or save it and decide later."), senderName);
                        switch (new DlgYesNoLater(this, "<LOC>Clan Invitation Request", msg, true).ShowDialog(this))
                        {
                            case DialogResult.Ignore:
                            {
                                if (!DataAccess.ExecuteQuery("SaveOfflineClanRequest", new object[] { senderId }))
                                {
                                    this.ErrorMessage("<LOC>Unable to save request, ensure you do not already have a pending request from this target.", new object[0]);
                                    break;
                                }
                                if (this.ClanRequests == null)
                                {
                                    this.ClanRequests = new MappedObjectList<ClanRequest>();
                                }
                                ClanRequest item = new ClanRequest(senderId, senderName, User.Current.ID, User.Current.Name, DateTime.Now);
                                this.ClanRequests.Add(item);
                                this.ClanRequests.IndexObject(item);
                                this.RefreshClanRequestCount();
                                this.SystemMessage("<LOC>This request has been saved for future action. Please check the Clan tab to review it.", new object[0]);
                                Messaging.SendCustomCommand(senderName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has decided to save your request and review it later..", User.Current.Name });
                                break;
                            }
                            case DialogResult.Yes:
                                this.InviteToClan(senderName);
                                break;

                            default:
                                Messaging.SendCustomCommand(senderName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has declined your request.", User.Current.Name });
                                this.SystemMessage("<LOC>A rejection notification has been sent.", new object[0]);
                                break;
                        }
                    };
                }
                base.BeginInvoke(method);
            }
        }

        private void RecieveFriendRequest(string senderName, int senderID)
        {
            VGen0 method = null;
            if (this.FriendRequests.ContainsIndex("name", senderName) || this.ActiveDialogRequests.Contains(senderName))
            {
                Messaging.SendCustomCommand(senderName, CustomCommands.SystemMessage, new object[] { "<LOC>You already have a pending friend request with {0}.", User.Current.Name });
            }
            else if (User.CurrentFriends.ContainsIndex("name", senderName))
            {
                Messaging.SendCustomCommand(senderName, CustomCommands.SystemMessage, new object[] { "<LOC>{0} is already your friend.", User.Current.Name });
            }
            else
            {
                this.ActiveDialogRequests.Add(senderName);
                if (method == null)
                {
                    method = delegate {
                        string msg = string.Format(Loc.Get("<LOC>{0} has invited you to be a Friends list contact. You may accept the invitation, reject it, or save it and decide later."), senderName);
                        switch (new DlgYesNoLater(this, "<LOC>Friends Invitation", msg, true).ShowDialog())
                        {
                            case DialogResult.Ignore:
                                if (!DataAccess.ExecuteQuery("AddFriendRequest", new object[] { senderID, User.Current.ID }))
                                {
                                    this.ErrorMessage("<LOC>Unable to save request, ensure you do not already have a pending request from {0}.", new object[] { senderName });
                                    break;
                                }
                                this.FriendRequests.Add(new FriendRequest(senderID, senderName, User.Current.ID, User.Current.Name, DateTime.Now));
                                this.RefreshFriendInvites();
                                this.SystemMessage("<LOC>This request has been saved for future action. Please check the Friends tab to review it.", new object[0]);
                                Messaging.SendCustomCommand(senderName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has saved your invitation for later review.", User.Current.Name });
                                break;

                            case DialogResult.Yes:
                                this.OnAddFriend(senderName, senderID);
                                break;

                            default:
                                this.SystemMessage("<LOC>A rejection notification has been sent.", new object[0]);
                                Messaging.SendCustomCommand(senderName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has declined your invitation.", User.Current.Name });
                                break;
                        }
                        this.ActiveDialogRequests.Remove(senderName);
                    };
                }
                base.BeginInvoke(method);
            }
        }

        internal void RecieveGameLobbyMessage(string senderName, int senderId, uint message, string[] args)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RecieveGameLobbyMessage(senderName, senderId, message, args);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    if (message < 0x17)
                    {
                        switch (((GameLobbyMessages) message))
                        {
                            case GameLobbyMessages.Invite:
                                if (this.CurrentGameLobby == null)
                                {
                                    goto Label_0125;
                                }
                                Messaging.SendCustomCommand(senderName, CustomCommands.GameLobbyMessage, new object[] { 0x13, "<LOC>{0} is already in another game.", User.Current.Name });
                                return;

                            case GameLobbyMessages.Full:
                                this.PendingJoinFrom = null;
                                DlgMessage.Show(this, "<LOC>Game Full", "<LOC>The requested game is full.");
                                goto Label_02C1;

                            case GameLobbyMessages.Game:
                                if (this.PendingJoinFrom == senderName)
                                {
                                    this.PendingJoinFrom = null;
                                    DlgGameLobby lobby = DlgGameLobby.JoinGame(GPGnetGame.FromDataString<SpaceSiegeGame>(args[0]));
                                    this.CurrentGameLobby = lobby;
                                    lobby.Show();
                                }
                                goto Label_02C1;
                        }
                    }
                    goto Label_02C1;
                Label_0125:
                    if (this.PendingJoinFrom != null)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.GameLobbyMessage, new object[] { 0x13, "<LOC>{0} already has a pending invitation for a game.", User.Current.Name });
                        return;
                    }
                    if (User.Current.IsDND)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.GameLobbyMessage, new object[] { 0x13, "<LOC>{0} is in Do Not Disturb mode and cannot join your game.", User.Current.Name });
                        return;
                    }
                    if (new DlgYesNo(this, "<LOC>Game Invite", string.Format("<LOC>{0} is inviting you to join a game, do you wish to join?", senderName)).ShowDialog() == DialogResult.Yes)
                    {
                        this.AcceptGameInvite(senderName);
                    }
                    else
                    {
                        this.PendingJoinFrom = null;
                        Messaging.SendCustomCommand(senderName, CustomCommands.GameLobbyMessage, new object[] { 0x13, "<LOC>{0} has declined your game invitation.", User.Current.Name });
                    }
                Label_02C1:
                    if (this.CurrentGameLobby != null)
                    {
                        this.CurrentGameLobby.RecieveMessage(senderName, senderId, message, args);
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        internal void RecieveLadderChallengeRequest(string senderName, int ladder)
        {
            VGen0 method = null;
            if (((base.InvokeRequired && !base.Disposing) && !base.IsDisposed) && base.IsHandleCreated)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RecieveLadderChallengeRequest(senderName, ladder);
                    };
                }
                base.BeginInvoke(method);
            }
            else if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
            {
                try
                {
                    if (!(!FrmLadderView.AcceptingChallengesLookup.ContainsKey(ladder) ? true : !FrmLadderView.AcceptingChallengesLookup[ladder]))
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} is currently declining challenges, you may need to refresh your ladder view.", User.Current.Name });
                    }
                    else if (this.LadderInvitePending)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} already has a ladder invite pending.", User.Current.Name });
                    }
                    else if (!((this.DlgTeamGame == null) || this.DlgTeamGame.IsDisposed))
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} is playing a game and cannot accept your challenge at this time.", User.Current.Name });
                    }
                    else if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} is already in the ranked game queue.", User.Current.Name });
                    }
                    else if (this.mIsInGame)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} is playing a game and cannot accept your challenge at this time.", User.Current.Name });
                    }
                    else if (!(this.IsGameCurrent || User.Current.IsAdmin))
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} has a different version of the game and cannot accept your challenge at this time.", User.Current.Name });
                    }
                    else if (User.Current.IsDND)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} is in Do Not Disturb mode and cannot join your game.", User.Current.Name });
                    }
                    else if (GameInformation.SelectedGame.GameID != LadderInstance.AllInstances[ladder].GameID)
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} is playing a different game and cannot accept your challenge at this time.", User.Current.Name });
                    }
                    else if (!(this.CanLadderChallengeUp(ladder) || (this.GetLadderParticipantRank(ladder, User.Current.ID) <= this.GetLadderParticipantRank(ladder, senderName))))
                    {
                        Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[] { "<LOC>{0} cannot accept an upward challenge at this time.", User.Current.Name });
                    }
                    else
                    {
                        this.LadderInvitePending = true;
                        if (new DlgYesNo(this, "<LOC>Ladder Challenge", string.Format("<LOC>{0} has challenged you to a ranked ladder game on {1}. Do you wish to accept?", senderName, LadderInstance.AllInstances[ladder].Description)).ShowDialog() == DialogResult.Yes)
                        {
                            string sessionId = Guid.NewGuid().ToString();
                            Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeAccept, new object[] { sessionId, ladder });
                            this.LadderInvitePending = false;
                            this.BeginLadderChallengeMatch(sessionId, ladder);
                        }
                        else
                        {
                            Messaging.SendCustomCommand(senderName, CustomCommands.LadderChallengeDecline, new object[0]);
                            this.LadderInvitePending = false;
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public void RefreshChat()
        {
            this.gvChat.RefreshData();
        }

        internal void RefreshChatParticipant(User user)
        {
            this.UpdateUser(user);
        }

        internal void RefreshChatParticipant(string name)
        {
            this.RefreshChatParticipant(name, false);
        }

        internal void RefreshChatParticipant(string name, bool doLookup)
        {
            if (name != null)
            {
                User user;
                if (doLookup)
                {
                    if (DataAccess.TryGetObject<User>("GetPlayerDetails", out user, new object[] { name }))
                    {
                        this.RefreshChatParticipant(user);
                    }
                }
                else if (this.TryFindUser(name, false, out user))
                {
                    this.RefreshChatParticipant(user);
                }
            }
        }

        private void RefreshClan(string userMessage, string clanMessage, params object[] clanArgs)
        {
            VGen0 callback = null;
            if ((userMessage != null) && (userMessage.Length > 0))
            {
                this.SystemMessage(userMessage, new object[0]);
            }
            if (clanArgs == null)
            {
                clanArgs = new object[0];
            }
            object[] mArgs = new object[clanArgs.Length + 1];
            mArgs[0] = clanMessage;
            Array.ConstrainedCopy(clanArgs, 0, mArgs, 1, clanArgs.Length);
            if (Clan.Current == null)
            {
                if (callback == null)
                {
                    callback = delegate {
                        Messaging.SendCustomCommand(Clan.CurrentMembersMsgList, CustomCommands.UpdateClan, mArgs);
                        this.RefreshChatParticipant(User.Current);
                        this.pnlUserListClan.RefreshData();
                        foreach (string str in Clan.CurrentMembersMsgList)
                        {
                            User user;
                            if (Chatroom.GatheringParticipants.TryFindByIndex("name", str, out user))
                            {
                                user.ClanAbbreviation = Clan.Current.Abbreviation;
                                user.ClanName = Clan.Current.Name;
                                this.RefreshChatParticipant(str);
                            }
                            this.RefreshPMWindows();
                            this.RefreshGathering();
                        }
                    };
                }
                this.FrmMain_Clan(callback);
            }
            else
            {
                Messaging.SendCustomCommand(Clan.CurrentMembersMsgList, CustomCommands.UpdateClan, mArgs);
                this.FrmMain_Clan();
            }
        }

        internal void RefreshClanInviteCount()
        {
            try
            {
                if (this.ClanInvites != null)
                {
                    this.splitContainerClan.Panel1Collapsed = true;
                    if (this.ClanInvites.Count > 0)
                    {
                        this.gpgLabelClanInvites.Visible = true;
                        this.gpgLabelClanInviteCount.Visible = false;
                        this.gpgLabelClanInvites.Text = string.Format(Loc.Get("View Invitations ({0})"), this.ClanInvites.Count);
                    }
                    else
                    {
                        this.gpgLabelClanInvites.Visible = false;
                        this.gpgLabelClanInviteCount.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void RefreshClanRequestCount()
        {
            try
            {
                if (this.ClanRequests != null)
                {
                    if (this.ClanRequests.Count > 0)
                    {
                        this.gpgLabelClanRequestCount.Visible = false;
                        this.splitContainerClan.Panel1Collapsed = false;
                        this.gpgLabelClanRequests.Text = string.Format(Loc.Get("<LOC>View Requests ({0})"), this.ClanRequests.Count);
                        this.gpgLabelClanRequests.Visible = true;
                    }
                    else
                    {
                        this.gpgLabelClanRequestCount.Visible = false;
                        this.gpgLabelClanRequests.Visible = false;
                        this.splitContainerClan.Panel1Collapsed = true;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void RefreshEmoteAnimations()
        {
            try
            {
                if (ConfigSettings.GetBool("AnimateEmotes", true))
                {
                    foreach (Emote emote in Emote.AllEmotes.Values)
                    {
                        if (!(emote.Animated || !ImageAnimator.CanAnimate(emote.Image)))
                        {
                            emote.Animated = true;
                            emote.CanAnimate = true;
                            emote.AnimationFrames = emote.Image.GetFrameCount(new FrameDimension(emote.Image.FrameDimensionsList[0]));
                            ImageAnimator.Animate(emote.Image, new EventHandler(this.OnFrameChange));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void RefreshFriendInvites()
        {
            VGen0 method = null;
            foreach (User user in User.CurrentFriends)
            {
                if (this.FriendRequests.ContainsIndex("name", user.Name))
                {
                    this.FriendRequests.RemoveByIndex("name", user.Name);
                    ThreadQueue.Quazal.Enqueue((VGen1)delegate (object o) {
                        DataAccess.ExecuteQuery("RemoveFriendRequest", new object[] { o });
                    }, new object[] { user.ID });
                }
            }
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        if ((this.FriendRequests != null) && (this.FriendRequests.Count > 0))
                        {
                            this.gpgLabelFriendInvitesCount2.Visible = false;
                            this.splitContainerFriends.Panel1Collapsed = false;
                            this.gpgLabelFriendInvites2.Text = string.Format(Loc.Get("View Invitations ({0})"), this.FriendRequests.Count);
                        }
                        else
                        {
                            this.gpgLabelFriendInvitesCount2.Visible = false;
                            this.splitContainerFriends.Panel1Collapsed = true;
                        }
                    };
                }
                base.BeginInvoke(method);
            }
            else if ((this.FriendRequests != null) && (this.FriendRequests.Count > 0))
            {
                this.gpgLabelFriendInvitesCount2.Visible = false;
                this.splitContainerFriends.Panel1Collapsed = false;
                this.gpgLabelFriendInvites2.Text = string.Format(Loc.Get("View Invitations ({0})"), this.FriendRequests.Count);
            }
            else
            {
                this.gpgLabelFriendInvitesCount2.Visible = false;
                this.splitContainerFriends.Panel1Collapsed = true;
            }
        }

        private void RefreshGameList()
        {
            foreach (ToolStripMenuItem item in this.mGameMenuItems)
            {
                GameInformation tag = item.Tag as GameInformation;
                item.Visible = System.IO.File.Exists(tag.GameLocation);
            }
        }

        internal void RefreshGathering()
        {
            this.RefreshGathering(null);
        }

        internal void RefreshGathering(string status)
        {
            this.RefreshGathering(User.Current, status);
        }

        internal void RefreshGathering(User user, string status)
        {
            Messaging.SendCustomCommand(CustomCommands.UpdateUser, new object[] { user.ToDataString(), status });
        }

        internal void RefreshPMWindows()
        {
            foreach (FrmPrivateChat chat in this.PrivateChats.Values)
            {
                if (!(chat.Disposing || chat.IsDisposed))
                {
                    chat.RefreshToolstrip();
                }
            }
        }

        internal void RefreshView(GridView view)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        GPGGridView.RefreshView(view);
                        this.CalcGridHeight(view, 20);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                GPGGridView.RefreshView(view);
                this.CalcGridHeight(view, 20);
            }
        }

        private bool RemoveChatParticipant(string name)
        {
            try
            {
                User user;
                if (Chatroom.GatheringParticipants.TryFindByIndex("name", name, out user))
                {
                    this.ChatroomQueue.Enqueue(this._RemoveUser, new object[] { new object[] { user } });
                }
                if (!Chatroom.GatheringParticipants.RemoveByIndex("name", name))
                {
                    GPG.Logging.EventLog.WriteLine("Failed to find player: {0} in user list for removal.", new object[] { name });
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        internal bool RemoveClanMember(string memberName)
        {
            ClanMember member;
            if (((Clan.Current == null) || (Clan.CurrentMembers == null)) || (ClanMember.Current == null))
            {
                this.ErrorMessage("<LOC>You are not in a clan.", new object[0]);
                return false;
            }
            if (!Clan.CurrentMembers.TryFindByIndex("name", memberName, out member))
            {
                this.ErrorMessage("<LOC>{0} is not a member of your clan.", new object[] { memberName });
                return false;
            }
            if (!ClanMember.Current.CanTargetAbility(ClanAbility.Remove, member))
            {
                this.ErrorMessage("<LOC>You cannot remove {0} from your clan.", new object[] { memberName });
                return false;
            }
            string msg = string.Format(Loc.Get("<LOC>Are you sure you want to remove {0} from your clan?"), memberName);
            if (new DlgYesNo(this, "<LOC>Remove from Clan", msg).ShowDialog(this) == DialogResult.Yes)
            {
                if (!DataAccess.ExecuteQuery("LeaveClan", new object[] { member.ID, Clan.Current.ID }))
                {
                    this.ErrorMessage("<LOC>Unable to remove clan member at this time.", new object[0]);
                    return false;
                }
                this.SystemMessage("<LOC>You have removed {0} from your clan.", new object[] { memberName });
                Messaging.SendCustomCommand(Clan.CurrentMembersMsgList, CustomCommands.UpdateClan, new object[] { "<LOC>{0} has removed {1} from your clan.", User.Current.Name, memberName });
                Clan.CurrentMembers.RemoveByIndex("name", memberName);
                this.pnlUserListClan.RefreshData();
                return true;
            }
            return false;
        }

        internal void RemoveFriend(string friendName)
        {
            User user;
            if (User.CurrentFriends.TryFindByIndex("name", friendName, out user))
            {
                this.RemoveFriend(user.Name, user.ID);
            }
            else
            {
                this.ErrorMessage("<LOC>{0} is not on your Friends list.", new object[] { friendName });
            }
        }

        internal void RemoveFriend(string friendName, int friendId)
        {
            if (this.RemoveFriend(friendName, friendId, true))
            {
                Messaging.SendCustomCommand(friendName, CustomCommands.RemoveFriend, new object[0]);
            }
        }

        internal bool RemoveFriend(string friendName, int friendId, bool prompt)
        {
            string msg = string.Format(Loc.Get("<LOC>Are you sure you want to remove {0} from your Friends list?"), friendName);
            if (!prompt || (new DlgYesNo(this, "<LOC>Remove Friend", msg).ShowDialog() == DialogResult.Yes))
            {
                try
                {
                    if (DataAccess.ExecuteQuery("RemoveFriend", new object[] { friendId, friendId }))
                    {
                        User user;
                        this.SystemMessage("<LOC>{0} has been removed from your Friends list.", new object[] { friendName });
                        if (!User.CurrentFriends.RemoveByIndex("name", friendName))
                        {
                            ErrorLog.WriteLine("Unable to find friend for removal, name: {0}", new object[] { friendName });
                            this.ErrorMessage("<LOC>An error occured while removing {0} as a friend, you may need to restart the application for the changes to take effect.", new object[] { friendName });
                        }
                        if (Chatroom.GatheringParticipants.TryFindByIndex("name", friendName, out user))
                        {
                            user.IsFriend = false;
                            this.RefreshChatParticipant(user);
                        }
                        if (User.CurrentFriends.Count < 1)
                        {
                            this.FrmMain_Friends();
                        }
                        foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                        {
                            if ((!chat.Disposing && !chat.IsDisposed) && (chat.ChatTarget.Name.ToLower() == friendName.ToLower()))
                            {
                                chat.ChatTarget.IsFriend = false;
                                chat.RefreshToolstrip();
                            }
                        }
                        return true;
                    }
                    this.ErrorMessage("<LOC>Unable to remove {0} from your Friends list at this time.", new object[] { friendName });
                    ErrorLog.WriteLine("Failed to execute RemoveFriend", new object[0]);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    this.ErrorMessage("<LOC>Unable to remove {0} from your Friends list at this time.", new object[] { friendName });
                }
            }
            return false;
        }

        public void RemoveMiddleButton(SkinStatusButton button)
        {
            this.pbMiddle.Remove(button);
        }

        private void RemoveUser(object[] args)
        {
            try
            {
                if (!Chatroom.InChatroom)
                {
                    ErrorLog.WriteLine("Attempt to remove user {0} from chat while not in a chatroom", new object[] { args[0] });
                }
                else
                {
                    User user = (User) args[0];
                    this.pnlUserListChat.RemoveUser(user);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void RequestClanInvite(string target)
        {
            if ((Clan.Current != null) || (ClanMember.Current != null))
            {
                this.ErrorMessage("<LOC>You are already in a clan.", new object[0]);
            }
            else
            {
                User user;
                if (!Chatroom.GatheringParticipants.TryFindByIndex("name", target, out user) && !User.CurrentFriends.TryFindByIndex("name", target, out user))
                {
                    MappedObjectList<User> objects = DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { target });
                    if (objects.Count > 0)
                    {
                        user = objects[0];
                    }
                }
                if (user == null)
                {
                    this.ErrorMessage("<LOC>Unable to locate user {0}", new object[] { target });
                }
                else if (!user.IsInClan)
                {
                    this.ErrorMessage("<LOC>This player is not in a clan.", new object[0]);
                }
                else if (user.Online)
                {
                    Messaging.SendCustomCommand(target, CustomCommands.RequestClanInvite, new object[0]);
                    this.SystemMessage("<LOC>A request has been sent.", new object[0]);
                }
                else if (DataAccess.ExecuteQuery("MakeOfflineClanRequest", new object[] { user.ID }))
                {
                    this.SystemMessage("<LOC>A request has been sent to this user. The user is currently offline, but can review your request the next time he is online.", new object[0]);
                }
                else
                {
                    this.ErrorMessage("<LOC>Unable to send request at this time. The user is currently offline.", new object[0]);
                }
            }
        }

        private void RequestDirectChallenge(string senderName)
        {
            VGen0 method = null;
            if (!(!User.Current.IsDND && this.AllowDirectChallenge))
            {
                Messaging.SendCustomCommand(senderName, CustomCommands.AutomatchAbortDirectChallenge, new object[] { "Standard" });
            }
            else if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
            {
                Messaging.SendCustomCommand(senderName, CustomCommands.AutomatchAbortDirectChallenge, new object[] { "Standard" });
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        if (this.mCanDirectChallenge)
                        {
                            this.mCanDirectChallenge = false;
                            DlgYesNo no = new DlgYesNo(this, Loc.Get("<LOC>Ranked Challenge"), senderName + " " + Loc.Get("<LOC>has challenged you to a ranked game.  Would you like to accept?"));
                            if (no.ShowDialog() == DialogResult.Yes)
                            {
                                string kind = "CHALLENGE" + Guid.NewGuid().ToString();
                                Messaging.SendCustomCommand(senderName, CustomCommands.AutomatchConfirmChallenge, new object[] { kind });
                                this.PlayRankedGame(false, kind, null, "", false);
                            }
                            else
                            {
                                Messaging.SendCustomCommand(senderName, CustomCommands.AutomatchAbortDirectChallenge, new object[] { "Standard" });
                            }
                            this.mCanDirectChallenge = true;
                        }
                        else
                        {
                            Messaging.SendCustomCommand(senderName, CustomCommands.AutomatchAbortDirectChallenge, new object[] { "Standard" });
                        }
                    };
                }
                base.Invoke(method);
            }
        }

        public void ResetAwayTimer()
        {
            WaitCallback callBack = null;
            if (base.InvokeRequired)
            {
                if (!User.Current.IsAdmin && !User.Current.IsModerator)
                {
                    this.AwayTimer = new StateTimer((double) this.AwayInterval);
                    this.AwayTimer.AutoReset = false;
                    this.AwayTimer.Elapsed += new ElapsedEventHandler(this.AwayTimer_Elapsed);
                    this.AwayTimer.Start();
                    this.SelfLeaveTimer = new StateTimer((double) this.SelfLeaveInterval);
                    this.SelfLeaveTimer.AutoReset = false;
                    this.SelfLeaveTimer.Elapsed += new ElapsedEventHandler(this.SelfLeaveTimer_Elapsed);
                    this.SelfLeaveTimer.Start();
                }
            }
            else
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        this.ResetAwayTimer();
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
        }

        internal void ResetChatFilters()
        {
            Program.Settings.Chat.Filters.FilterSystemMessages = true;
            Program.Settings.Chat.Filters.FilterSystemEvents = true;
            Program.Settings.Chat.Filters.FilterSystemErrors = true;
            Program.Settings.Chat.Filters.FilterGameMessages = true;
            Program.Settings.Chat.Filters.FilterAdmin = true;
            Program.Settings.Chat.Filters.FilterFriends = true;
            Program.Settings.Chat.Filters.FilterClan = true;
            Program.Settings.Chat.Filters.FilterOther = true;
            Program.Settings.Chat.Filters.FilterSelf = true;
        }

        private void ResetLobby()
        {
            if (((!User.Current.IsAdmin && (GameInformation.SelectedGame.GameID != -1)) && (GameKey.BetaKeys.Count < 1)) && (new DlgNoBetaKey(this).ShowDialog() != DialogResult.OK))
            {
                base.Close();
                Application.Exit();
            }
            this.CheckUpdate();
            this.mRanInitializeChat = false;
            this.SetStatusButtons(0);
            this.ResetTitle();
            this.miAdmin.Visible = User.Current.IsAdmin;
            this.miCreateTournament.Visible = User.Current.IsAdmin;
            AccessControlList byName = AccessControlList.GetByName("TournamentDirectors");
            if ((byName != null) && byName.HasAccess())
            {
                this.miCreateTournament.Visible = true;
            }
            if (Program.Settings.Sound.Speech.EnableSpeech)
            {
                Speech.Speak(string.Format("Welcome to G P G net, Supreme Commander, {0}", User.Current.Name));
            }
            Messaging.PollChat();
        }

        private void ResetPlayNowTooltip()
        {
            try
            {
                if ((((Program.Settings.SupcomPrefs.RankedGames.Maps == null) || (Program.Settings.SupcomPrefs.RankedGames.Maps.Length < 1)) || (Program.Settings.SupcomPrefs.RankedGames.Faction == null)) || (Program.Settings.SupcomPrefs.RankedGames.Faction.Length < 1))
                {
                    this.btnPlayNow.ToolTipText = Loc.Get("<LOC>Play Now\r\nNo preferences set for ranked games.\r\nClick here to setup your ranked game preferences, and start playing a ranked game.").Replace(@"\r\n", "\r\n");
                }
                else
                {
                    string str;
                    if (Program.Settings.SupcomPrefs.RankedGames.Faction == "/uef")
                    {
                        str = Program.Settings.SupcomPrefs.RankedGames.Faction.TrimStart("/".ToCharArray()).ToUpper();
                    }
                    else
                    {
                        str = TextUtil.Capitalize(Program.Settings.SupcomPrefs.RankedGames.Faction.TrimStart("/".ToCharArray()));
                    }
                    string str2 = string.Format(Loc.Get("<LOC>Play Now\r\nPlay a ranked game right now with the following preferences:\r\n\r\nPreferred Faction - {0}\r\n\r\nMap Preferences:\r\n").Replace(@"\r\n", "\r\n"), str);
                    bool flag = false;
                    foreach (SupcomMapInfo info in Program.Settings.SupcomPrefs.RankedGames.Maps)
                    {
                        if ((info != null) && info.Priority.HasValue)
                        {
                            string str3;
                            flag = true;
                            if (info.Priority.Value)
                            {
                                str3 = Loc.Get("<LOC>Thumbs Up");
                            }
                            else
                            {
                                str3 = Loc.Get("<LOC>Thumbs Down");
                            }
                            str2 = str2 + string.Format("{0} - {1}\r\n", info.Description, str3);
                        }
                    }
                    if (!flag)
                    {
                        str2 = str2 + Loc.Get("<LOC>No Preference");
                    }
                    str2.TrimEnd("\r\n".ToCharArray());
                    this.btnPlayNow.ToolTipText = str2;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void ResetTitle()
        {
            VGen0 method = null;
            if (((base.InvokeRequired && !base.Disposing) && !base.IsDisposed) && base.IsHandleCreated)
            {
                if (method == null)
                {
                    method = delegate {
                        this.ResetTitle();
                    };
                }
                base.BeginInvoke(method);
            }
            else if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
            {
                if (Chatroom.InChatroom)
                {
                    this.Text = string.Format(Loc.Get("<LOC>GPGnet: Supreme Commander - {0} Chatting in: {1}"), User.Current.Name, Chatroom.Current.Description);
                }
                else
                {
                    this.Text = string.Format(Loc.Get("<LOC>GPGnet: Supreme Commander - {0}"), User.Current.Name);
                }
            }
        }

        private void Restore(object sender, EventArgs e)
        {
            try
            {
                if (base.WindowState == FormWindowState.Minimized)
                {
                    base.WindowState = FormWindowState.Normal;
                    if (this.mIsMaximized)
                    {
                        this.pbRestore.Image = SkinManager.GetImage("restore.png");
                        this.pbBottomRight.Image = SkinManager.GetImage("bottomright_fixed.png");
                        this.mRestore = base.Bounds;
                        this.mIsMaximized = true;
                        base.Bounds = Screen.GetWorkingArea(this);
                        base.Top -= this.mHeightDiff;
                        base.Height = (base.Height + this.mHeightDiff) + 7;
                        base.Left -= 4;
                        base.Width += 9;
                    }
                }
                else if (this.mIsMaximized)
                {
                    this.pbRestore.Image = SkinManager.GetImage("maximize.png");
                    this.pbBottomRight.Image = SkinManager.GetImage("bottomright.png");
                    this.mIsMaximized = false;
                    if (base.Bounds == this.mRestore)
                    {
                        base.Width = (int) (((double) this.mRestore.Width) / 1.25);
                        base.Height = (int) (((double) this.mRestore.Height) / 1.25);
                    }
                    else
                    {
                        base.Bounds = this.mRestore;
                    }
                }
                else
                {
                    this.pbRestore.Image = SkinManager.GetImage("restore.png");
                    this.pbBottomRight.Image = SkinManager.GetImage("bottomright_fixed.png");
                    this.mRestore = base.Bounds;
                    this.mIsMaximized = true;
                    base.Bounds = Screen.GetWorkingArea(this);
                    base.Top -= this.mHeightDiff;
                    base.Height = (base.Height + this.mHeightDiff) + 7;
                    base.Left -= 4;
                    base.Width += 9;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        [DllImport("patchw32.dll", EntryPoint="RTPatchApply32@12")]
        public static extern uint RTPatchApply([MarshalAs(UnmanagedType.LPTStr)] string cmd, [MarshalAs(UnmanagedType.FunctionPtr)] PatchCallback callback, bool waitFlag);
        private void SelfLeaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            VGen0 method = null;
            if ((!User.Current.IsAdmin && !User.Current.IsModerator) && (!base.Disposing && !base.IsDisposed))
            {
                if (method == null)
                {
                    method = delegate {
                        try
                        {
                            TimeSpan span = (TimeSpan) (e.SignalTime - this.LastAction);
                            int totalMilliseconds = (int) span.TotalMilliseconds;
                            if ((!Chatroom.InChatroom || this.mIsInGame) || (Chatroom.Current == Chatroom.None))
                            {
                                this.SelfLeaveTimer.Interval = this.SelfLeaveInterval;
                            }
                            else if (totalMilliseconds >= this.SelfLeaveTimer.Interval)
                            {
                                this.SystemMessage(Loc.Get("<LOC>You have been temporarily removed from chat due to being away for an extended period. To return to a chat room, select one from the menu above."), new object[0]);
                                this.LeaveChat();
                                this.SelfLeaveTimer.Interval = this.SelfLeaveInterval;
                            }
                            else
                            {
                                this.SelfLeaveTimer.Interval = this.SelfLeaveInterval - totalMilliseconds;
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                base.BeginInvoke(method);
            }
        }

        private void SendMessage()
        {
            if (this.Connected)
            {
                if ((this.textBoxMsg.Text != null) && (this.textBoxMsg.Text.Trim().Length > 0))
                {
                    this.textBoxMsg.Text = this.textBoxMsg.Text.TrimEnd(new char[] { ' ' });
                    this.ChatHistory.AddFirst(this.textBoxMsg.Text);
                    while (this.ChatHistory.Count > Program.Settings.Chat.ChatHistoryLength)
                    {
                        this.ChatHistory.RemoveLast();
                    }
                    this.HistoryIndex = -1;
                    if (this.textBoxMsg.Text.StartsWith("/"))
                    {
                        UserAction action;
                        string[] sourceArray = this.textBoxMsg.Text.TrimEnd(new char[] { ' ' }).Split(new char[] { ' ' });
                        this.textBoxMsg.Text = "";
                        if (sourceArray[0] == "/?")
                        {
                            foreach (UserAction curaction in UserAction.AllActions)
                            {
                                this.SystemMessage(curaction.ToString(), new object[0]);
                            }
                        }
                        else if (!UserAction.TryGetByPartialName(sourceArray[0], out action))
                        {
                            this.SystemMessage("<LOC>Unknown command. Enter /? to see a list of valid commands.", new object[0]);
                        }
                        else
                        {
                            string[] destinationArray = new string[sourceArray.Length - 1];
                            if (destinationArray.Length > 0)
                            {
                                Array.ConstrainedCopy(sourceArray, 1, destinationArray, 0, destinationArray.Length);
                            }
                            if (action.CheckFormat(destinationArray))
                            {
                                this.ProcessUserAction(action.Command, destinationArray);
                            }
                            else
                            {
                                this.SystemMessage("<LOC>Improperly formatted command, the proper format for this command is:", new object[0]);
                                this.SystemMessage(action.ToString(), new object[0]);
                            }
                        }
                    }
                    else if (Chatroom.InChatroom)
                    {
                        string text = this.textBoxMsg.Text;
                        this.textBoxMsg.Text = "";
                        ChatLink[] linkArray = ChatLink.FindLinks(text, ChatLink.Emote);
                        for (int i = 0; i < linkArray.Length; i++)
                        {
                            string charSequence = EmoteLinkMask.GetCharSequence(linkArray[i]);
                            if ((charSequence == null) || (charSequence.Length < 1))
                            {
                                text = text.Replace(ChatLink.Emote.LinkWord, "");
                            }
                            else if (Emote.AllEmotes.ContainsKey(charSequence))
                            {
                                text = text.Replace(linkArray[i].FullUrl, charSequence);
                            }
                        }
                        this.AddChat(User.Current, text);
                        this.gvChat.MoveLastVisible();
                        if (ConfigSettings.GetBool("Emotes", true) && Program.Settings.Chat.Emotes.AutoShareEmotes)
                        {
                            ChatLink[] linkArray2 = ChatLink.FindLinks(text);
                            foreach (Emote emote in Emote.AllEmotes.Values)
                            {
                                int index = text.IndexOf(emote.CharSequence);
                                string str3 = string.Format("{0}\"{1}{2}", ChatLink.Emote.LinkWord, User.Current.Name, '\x0003');
                                while (index >= 0)
                                {
                                    bool flag = false;
                                    foreach (ChatLink link in linkArray2)
                                    {
                                        if ((index > link.StartIndex) && (index < link.EndIndex))
                                        {
                                            flag = true;
                                        }
                                    }
                                    if (flag)
                                    {
                                        index = text.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                    }
                                    else
                                    {
                                        text = text.Insert(index, str3);
                                        text = text.Insert((index + str3.Length) + emote.CharSequence.Length, "\"");
                                        index = text.IndexOf(emote.CharSequence, (int) ((index + str3.Length) + emote.CharSequence.Length));
                                    }
                                }
                            }
                        }
                        this.OnAction();
                        this.CheckUserState();
                        Messaging.SendGathering(text);
                    }
                    else
                    {
                        this.ErrorMessage("<LOC>You are not in a chatroom.", new object[0]);
                    }
                }
            }
            else
            {
                this.SystemMessage(Loc.Get("<LOC>*** YOU HAVE LOST YOUR GPGNET CONNECTION ***"), new object[0]);
            }
        }

        internal void SendMessage(string msg)
        {
            if (this.Connected)
            {
                if ((msg != null) && (msg.Trim().Length > 0))
                {
                    msg = msg.TrimEnd(new char[] { ' ' });
                    this.ChatHistory.AddFirst(msg);
                    while (this.ChatHistory.Count > Program.Settings.Chat.ChatHistoryLength)
                    {
                        this.ChatHistory.RemoveLast();
                    }
                    this.HistoryIndex = -1;
                    if (msg.StartsWith("/"))
                    {
                        UserAction action;
                        string[] sourceArray = msg.TrimEnd(new char[] { ' ' }).Split(new char[] { ' ' });
                        msg = "";
                        if (sourceArray[0] == "/?")
                        {
                            foreach (UserAction curaction in UserAction.AllActions)
                            {
                                this.SystemMessage(curaction.ToString(), new object[0]);
                            }
                        }
                        else if (!UserAction.TryGetByPartialName(sourceArray[0], out action))
                        {
                            this.SystemMessage("<LOC>Unknown command. Enter /? to see a list of valid commands.", new object[0]);
                        }
                        else
                        {
                            string[] destinationArray = new string[sourceArray.Length - 1];
                            if (destinationArray.Length > 0)
                            {
                                Array.ConstrainedCopy(sourceArray, 1, destinationArray, 0, destinationArray.Length);
                            }
                            if (action.CheckFormat(destinationArray))
                            {
                                this.ProcessUserAction(action.Command, destinationArray);
                            }
                            else
                            {
                                this.SystemMessage("<LOC>Improperly formatted command, the proper format for this command is:", new object[0]);
                                this.SystemMessage(action.ToString(), new object[0]);
                            }
                        }
                    }
                    else if (Chatroom.InChatroom)
                    {
                        ChatLink[] linkArray = ChatLink.FindLinks(msg, ChatLink.Emote);
                        for (int i = 0; i < linkArray.Length; i++)
                        {
                            string charSequence = EmoteLinkMask.GetCharSequence(linkArray[i]);
                            if ((charSequence == null) || (charSequence.Length < 1))
                            {
                                msg = msg.Replace(ChatLink.Emote.LinkWord, "");
                            }
                            else if (Emote.AllEmotes.ContainsKey(charSequence))
                            {
                                msg = msg.Replace(linkArray[i].FullUrl, charSequence);
                            }
                        }
                        this.AddChat(User.Current, msg);
                        this.gvChat.MoveLastVisible();
                        if (ConfigSettings.GetBool("Emotes", true) && Program.Settings.Chat.Emotes.AutoShareEmotes)
                        {
                            ChatLink[] linkArray2 = ChatLink.FindLinks(msg);
                            foreach (Emote emote in Emote.AllEmotes.Values)
                            {
                                int index = msg.IndexOf(emote.CharSequence);
                                string str2 = string.Format("{0}\"{1}{2}", ChatLink.Emote.LinkWord, User.Current.Name, '\x0003');
                                while (index >= 0)
                                {
                                    bool flag = false;
                                    foreach (ChatLink link in linkArray2)
                                    {
                                        if ((index > link.StartIndex) && (index < link.EndIndex))
                                        {
                                            flag = true;
                                        }
                                    }
                                    if (flag)
                                    {
                                        index = msg.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                    }
                                    else
                                    {
                                        msg = msg.Insert(index, str2);
                                        msg = msg.Insert((index + str2.Length) + emote.CharSequence.Length, "\"");
                                        index = msg.IndexOf(emote.CharSequence, (int) ((index + str2.Length) + emote.CharSequence.Length));
                                    }
                                }
                            }
                        }
                        this.OnAction();
                        this.CheckUserState();
                        Messaging.SendGathering(msg);
                    }
                    else
                    {
                        this.ErrorMessage("<LOC>You are not in a chatroom.", new object[0]);
                    }
                }
            }
            else
            {
                this.SystemMessage(Loc.Get("<LOC>*** YOU HAVE LOST YOUR GPGNET CONNECTION ***"), new object[0]);
            }
        }

        private void ServerFormMenuItemClick(object sender, EventArgs e)
        {
            new DlgServerForm().LoadForm((sender as ToolStripMenuItem).Tag.ToString());
        }

        public void SetAwayStatus(bool away)
        {
            try
            {
                if ((User.Current != null) && (away != User.Current.IsAway))
                {
                    byte num;
                    User.Current.IsAway = away;
                    if (away)
                    {
                        this.ChangeStatus("<LOC>Away", StatusIcons.idle);
                        num = 1;
                        this.AwayTimer.Stop();
                    }
                    else
                    {
                        if (User.Current.IsDND)
                        {
                            this.ChangeStatus("<LOC>Do Not Disturb", StatusIcons.dnd);
                        }
                        else
                        {
                            this.ChangeStatus("<LOC>Online", StatusIcons.online);
                        }
                        num = 0;
                        this.AwayTimer.Interval = this.AwayInterval;
                        this.AwayTimer.Start();
                    }
                    if (DataAccess.ExecuteQuery("SetAwayStatus", new object[] { num }))
                    {
                        this.RefreshGathering();
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void SetChatEffect(string senderName, string effect)
        {
            User user;
            if (Chatroom.GatheringParticipants.TryFindByIndex("name", senderName, out user))
            {
                if (user.IsAdmin || user.Equals(User.Current))
                {
                    if ((ChatEffects.Current != null) && ((ChatEffects.Current.Name == effect) || (effect == ChatEffects.None.Name)))
                    {
                        this.SystemMessage("<LOC>{0} has returned you to normal.", new object[] { senderName, effect });
                        ChatEffects.Current = ChatEffects.None;
                    }
                    else
                    {
                        this.SystemMessage("<LOC>{0} has applied the {1} effect to you.", new object[] { senderName, effect });
                        ChatEffects.Current = ChatEffects.FindByDescription(effect);
                    }
                    Messaging.SendCustomCommand(CustomCommands.SetStatus, new object[] { ChatEffects.Current.Status.Name });
                }
                else
                {
                    AuditLog.WriteLine("Received effect command from non-admin user", new object[0]);
                }
            }
            else
            {
                AuditLog.WriteLine("Received effect command from unknown user", new object[0]);
            }
        }

        private void SetCurrentGridView(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is GridView)
                {
                    this.mSelectedParticipantView = sender as GridView;
                    this.mSelectedParticipantView.Focus();
                    GridHitInfo info = this.mSelectedParticipantView.CalcHitInfo(e.Location);
                    if (info.InRow)
                    {
                        this.mSelectedParticipantView.SelectRow(info.RowHandle);
                        this.mSelectedChatParticipant = (this.mSelectedParticipantView.GetRow(info.RowHandle) as TextLine).Tag as IUser;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void SetDNDStatus(bool dnd)
        {
            if (dnd != User.Current.IsDND)
            {
                byte num;
                User.Current.IsDND = dnd;
                if (dnd)
                {
                    this.ChangeStatus("<LOC>Do Not Disturb", StatusIcons.dnd);
                    num = 1;
                }
                else
                {
                    if (User.Current.IsAway)
                    {
                        this.ChangeStatus("<LOC>Away", StatusIcons.idle);
                    }
                    else
                    {
                        this.ChangeStatus("<LOC>Online", StatusIcons.online);
                    }
                    num = 0;
                }
                if (DataAccess.ExecuteQuery("SetDNDStatus", new object[] { num }))
                {
                    this.RefreshGathering(UserStatus.Current.Name);
                }
            }
        }

        public void SetFocusImage(object sender, EventArgs e)
        {
            if (!this.IsActiveWindow)
            {
                this.mIsActiveWindow = true;
                this.pbTopLeft.Image = SkinManager.GetImage("topleft_focus.png");
                this.pbTopLeft.Refresh();
                this.pbClose.Image = SkinManager.GetImage("close.png");
                this.pbMinimize.Image = SkinManager.GetImage("minimize.png");
                if (this.mIsMaximized)
                {
                    this.pbRestore.Image = SkinManager.GetImage("restore.png");
                }
                else
                {
                    this.pbRestore.Image = SkinManager.GetImage("maximize.png");
                }
            }
        }

        public void SetLastLadderGameType(int ladderInstanceId, bool isDownChallengeOrAutomatch)
        {
            this.LastGameChallenges[ladderInstanceId] = isDownChallengeOrAutomatch;
            if (!new QuazalQuery("SetLastLadderChallenge", new object[] { isDownChallengeOrAutomatch, ladderInstanceId, User.Current.ID }).ExecuteNonQuery())
            {
                ErrorLog.WriteLine("Failed to change last ladder game challenge type.", new object[0]);
            }
        }

        private void SetLogo(Control parent)
        {
            parent.GotFocus += new EventHandler(this.SetFocusImage);
            parent.LostFocus += new EventHandler(this.SetUnfocusImage);
            foreach (Control control in parent.Controls)
            {
                this.SetLogo(control);
            }
        }

        private void SetOnlineStatus(string playerName, bool online)
        {
            User user;
            ClanMember member;
            VGen0 method = null;
            VGen1 onClick = null;
            VGen0 gen3 = null;
            VGen1 gen4 = null;
            if (online)
            {
                if (this.PrivateChats.ContainsKey(playerName))
                {
                    this.PrivateChats[playerName].ChatTarget.Online = true;
                    this.PrivateChats[playerName].SystemMessage("<LOC>{0} is online.", new object[] { playerName });
                }
                if (User.CurrentFriends.TryFindByIndex("name", playerName, out user))
                {
                    if (!this.IsInGame)
                    {
                        if (base.InvokeRequired)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    FrmSimpleMessage.DoNotification(Loc.Get("<LOC>Friends"), string.Format(Loc.Get("<LOC>{0} is online."), playerName), (VGen1)delegate (object sender) {
                                        this.OnSendWhisper((string) sender, null);
                                    }, new object[] { playerName });
                                };
                            }
                            base.BeginInvoke(method);
                        }
                        else
                        {
                            if (onClick == null)
                            {
                                onClick = delegate (object sender) {
                                    this.OnSendWhisper((string) sender, null);
                                };
                            }
                            FrmSimpleMessage.DoNotification(Loc.Get("<LOC>Friends"), string.Format(Loc.Get("<LOC>{0} is online."), playerName), onClick, new object[] { playerName });
                        }
                    }
                    user.Online = true;
                    this.pnlUserListFriends.UpdateUser(user);
                }
                if (Clan.CurrentMembers.TryFindByIndex("name", playerName, out member))
                {
                    if (!this.IsInGame)
                    {
                        if (base.InvokeRequired)
                        {
                            if (gen3 == null)
                            {
                                gen3 = delegate {
                                    FrmSimpleMessage.DoNotification(Loc.Get("<LOC>Clan Login"), string.Format(Loc.Get("<LOC>{0} is online."), playerName), (VGen1)delegate (object sender) {
                                        this.OnSendWhisper((string) sender, null);
                                    }, new object[] { playerName });
                                };
                            }
                            base.BeginInvoke(gen3);
                        }
                        else
                        {
                            if (gen4 == null)
                            {
                                gen4 = delegate (object sender) {
                                    this.OnSendWhisper((string) sender, null);
                                };
                            }
                            FrmSimpleMessage.DoNotification(Loc.Get("<LOC>Clan Login"), string.Format(Loc.Get("<LOC>{0} is online."), playerName), gen4, new object[] { playerName });
                        }
                    }
                    member.Online = true;
                    this.pnlUserListClan.RefreshData();
                }
            }
            else
            {
                if (this.PrivateChats.ContainsKey(playerName))
                {
                    this.PrivateChats[playerName].ChatTarget.Online = false;
                    this.PrivateChats[playerName].SystemMessage(Loc.Get("<LOC>{0} is offline and will not recieve messages."), new object[] { playerName });
                }
                if (User.CurrentFriends.TryFindByIndex("name", playerName, out user))
                {
                    user.Online = false;
                    this.pnlUserListFriends.UpdateUser(user);
                }
                if (Clan.CurrentMembers.TryFindByIndex("name", playerName, out member))
                {
                    member.Online = false;
                    this.pnlUserListClan.RefreshData();
                }
            }
        }

        private void SetPatchButtonsCompleted()
        {
            this.mIsSupComPatching = false;
            this.SupComPatchThread = null;
            base.BeginInvoke((VGen0)delegate {
                this.btnFeedback.Enabled = true;
                this.btnHostGame.Enabled = true;
                this.btnJoinGame.Enabled = true;
                this.btnRankedGame.Enabled = true;
                this.btnArrangedTeam.Enabled = true;
                this.btnPlayNow.Enabled = true;
                this.btnViewRankings.Enabled = true;
                this.EnableGameSelectButtons();
                this.miGameGroup.Enabled = true;
                this.miRankings.Enabled = true;
                this.btnRankedGame.Image = SkinManager.GetImage("nav-ranked_game.png");
                this.btnRankedGame.ToolTipText = Loc.Get("<LOC>Play Ranked Game");
                this.btnArrangedTeam.Image = SkinManager.GetImage("nav-ranked_team.png");
                this.btnArrangedTeam.ToolTipText = Loc.Get("<LOC>Play Arranged Team Game");
            });
        }

        private void SetRegion()
        {
            if (!(base.IsDisposed || base.Disposing))
            {
                Rectangle rectangle = new Rectangle(base.PointToScreen(new Point(0, 0)).X, base.PointToScreen(new Point(0, 0)).Y, base.Width, base.Height);
                this.mWidthDiff = rectangle.X - base.Bounds.X;
                this.mHeightDiff = rectangle.Y - base.Bounds.Y;
                Rectangle rectangle2 = new Rectangle(this.mWidthDiff, this.mHeightDiff, base.ClientRectangle.Width, base.ClientRectangle.Height);
                int hRgn = CreateRoundRectRgn(rectangle2.Left, rectangle2.Top, rectangle2.Right + 1, rectangle2.Bottom, Program.Settings.StylePreferences.FormBorderCurve, Program.Settings.StylePreferences.FormBorderCurve);
                SetWindowRgn(base.Handle.ToInt32(), hRgn, 1);
            }
        }

        public void SetSkin(string name)
        {
            EventHandler handler = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            string path = Application.StartupPath + @"\skins\" + name + @"\";
            if (Directory.Exists(path))
            {
                this.pbTopLeft.Image = base.GetImage(path + "topleft.png");
                this.pbTop.Image = base.GetImage(path + "top.png");
                this.pTop.Height = this.pbTop.Image.Height;
                this.pbTopRight.Image = base.GetImage(path + "topright.png");
                this.pbLeftBorder.Image = base.GetImage(path + "left.png");
                this.pbLeftBorder.Width = this.pbLeftBorder.Image.Width;
                this.pbLeftBorder.Top = this.pTop.Bottom;
                this.pbLeftBorder.Height = base.Height - this.pbTop.Bottom;
                this.pbRightBorder.Image = base.GetImage(path + "right.png");
                this.pbRightBorder.Width = this.pbRightBorder.Image.Width;
                this.pbRightBorder.Top = this.pTop.Bottom;
                this.pbRightBorder.Left = base.Width - this.pbRightBorder.Width;
                this.pbRightBorder.Height = base.Height - this.pbTop.Bottom;
                this.pbBottomLeft.Image = base.GetImage(path + "bottomleft.png");
                this.pbBottomRight.Image = base.GetImage(path + "bottomright.png");
                this.pbBottomRight.Width = this.pbBottomRight.Image.Width;
                this.pbBottom.Image = base.GetImage(path + "bottom.png");
                this.pBottom.Height = this.pbBottom.Image.Height;
                this.pbBottom.Paint += new PaintEventHandler(this.pbPaint);
                this.pbTop.Paint += new PaintEventHandler(this.pbPaint);
                this.pbMiddle.SendToBack();
                this.pbMiddle.Left = this.pbLeftBorder.Width;
                this.pbMiddle.Width = (base.Width - this.pbLeftBorder.Width) - this.pbRightBorder.Width;
                this.pbMiddle.Top = this.wbMain.Bottom;
                this.pbMiddle.Height = 0x19;
                this.pbMiddle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                this.msMainMenu.BackgroundImage = base.GetImage(path + "menubg.png");
                this.msMainMenu.ForeColor = Program.Settings.StylePreferences.MenuForeColor;
                this.msMainMenu.Left = Program.Settings.StylePreferences.MenuLeft;
                this.msMainMenu.Top = Program.Settings.StylePreferences.MenuTop;
                this.pManualTabs.Left = this.splitContainerBody.SplitterDistance + 12;
                this.pbMinimize.Image = base.GetImage(path + "minimize.png");
                this.pbMinimize.Top = Program.Settings.StylePreferences.NavTop;
                this.pbMinimize.Left = base.Width - Program.Settings.StylePreferences.NavInsetFromRight;
                this.pbRestore.Image = base.GetImage(path + "maximize.png");
                this.pbRestore.Top = this.pbMinimize.Top;
                this.pbRestore.Left = this.pbMinimize.Right + 1;
                this.pbClose.Image = base.GetImage(path + "close.png");
                this.pbClose.Top = this.pbRestore.Top;
                this.pbClose.Left = this.pbRestore.Right + 1;
                this.msQuickButtons.BackgroundImage = base.GetImage(path + "bottom.png");
                this.btnMore.DropDown.BackgroundImage = DrawUtil.ResizeImage(SkinManager.GetImage("brushbg.png"), this.msQuickButtons.Items[0].Size);
                foreach (ToolStripItem item in this.msQuickButtons.Items)
                {
                    item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                }
                this.btnChat.Image = base.GetImage(path + "nav-chat.png");
                this.btnHome.Image = base.GetImage(path + "nav-home.png");
                this.btnFeedback.Image = base.GetImage(path + "nav-feedback.png");
                this.btnHostGame.Image = base.GetImage(path + "nav-host_game.png");
                this.btnJoinGame.Image = base.GetImage(path + "nav-join_game.png");
                this.btnRankedGame.Image = base.GetImage(path + "nav-ranked_game.png");
                this.btnArrangedTeam.Image = base.GetImage(path + "nav-ranked_team.png");
                this.btnPlayNow.Image = base.GetImage(path + "nav-play_now.png");
                this.btnViewRankings.Image = base.GetImage(path + "nav-rankings.png");
                this.btnOptions.Image = base.GetImage(path + "nav-options.png");
                this.btnWorldMap.Image = base.GetImage(path + "nav-map.png");
                this.btnVault.Image = base.GetImage(path + "nav-vault.png");
                this.btnReplayVault.Image = base.GetImage(path + "nav-replays.png");
                this.btnMore.Image = SkinManager.GetImage("nav-more.png");
                this.ResetPlayNowTooltip();
                this.skinDropDownStatus.Left = 12;
                this.skinDropDownStatus.Top = ((this.pBottom.Height - this.skinDropDownStatus.Height) / 2) - 1;
                this.skinDropDownStatus.ForeColor = System.Drawing.Color.Black;
                this.msQuickButtons.Width += this.msQuickButtons.Left - (this.skinDropDownStatus.Right + 3);
                this.msQuickButtons.Left = this.skinDropDownStatus.Right + 3;
                if (handler == null)
                {
                    handler = delegate (object s, EventArgs e) {
                        this.pbClose.Image = SkinManager.GetImage("close_over.png");
                    };
                }
                this.pbClose.MouseEnter += handler;
                if (handler2 == null)
                {
                    handler2 = delegate (object s, EventArgs e) {
                        this.pbMinimize.Image = SkinManager.GetImage("minimize_over.png");
                    };
                }
                this.pbMinimize.MouseEnter += handler2;
                if (handler3 == null)
                {
                    handler3 = delegate (object s, EventArgs e) {
                        if (this.mIsMaximized)
                        {
                            this.pbRestore.Image = SkinManager.GetImage("restore_over.png");
                        }
                        else
                        {
                            this.pbRestore.Image = SkinManager.GetImage("maximize_over.png");
                        }
                    };
                }
                this.pbRestore.MouseEnter += handler3;
                if (handler4 == null)
                {
                    handler4 = delegate (object s, EventArgs e) {
                        if (this.IsActiveWindow)
                        {
                            this.pbClose.Image = SkinManager.GetImage("close.png");
                        }
                        else
                        {
                            this.pbClose.Image = SkinManager.GetImage("close_inactive.png");
                        }
                    };
                }
                this.pbClose.MouseLeave += handler4;
                if (handler5 == null)
                {
                    handler5 = delegate (object s, EventArgs e) {
                        if (this.IsActiveWindow)
                        {
                            this.pbMinimize.Image = SkinManager.GetImage("minimize.png");
                        }
                        else
                        {
                            this.pbMinimize.Image = SkinManager.GetImage("minimize_inactive.png");
                        }
                    };
                }
                this.pbMinimize.MouseLeave += handler5;
                if (handler6 == null)
                {
                    handler6 = delegate (object s, EventArgs e) {
                        if (this.mIsMaximized)
                        {
                            if (this.IsActiveWindow)
                            {
                                this.pbRestore.Image = SkinManager.GetImage("restore.png");
                            }
                            else
                            {
                                this.pbRestore.Image = SkinManager.GetImage("restore_inactive.png");
                            }
                        }
                        else if (this.IsActiveWindow)
                        {
                            this.pbRestore.Image = SkinManager.GetImage("maximize.png");
                        }
                        else
                        {
                            this.pbRestore.Image = SkinManager.GetImage("maximize_inactive.png");
                        }
                    };
                }
                this.pbRestore.MouseLeave += handler6;
            }
        }

        public void SetStatus(string status, params object[] args)
        {
            this.SetStatus(status, 0, args);
        }

        public void SetStatus(string status, int timeout, params object[] args)
        {
            VGen0 method = null;
            ElapsedEventHandler handler = null;
            try
            {
                if (!base.Disposing && !base.IsDisposed)
                {
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.pbMiddle.Text = string.Format(Loc.Get(status), args);
                                if (this.StatusChanged != null)
                                {
                                    this.StatusChanged(this, this.pbMiddle.Text);
                                }
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else
                    {
                        this.pbMiddle.Text = string.Format(Loc.Get(status), args);
                        if (this.StatusChanged != null)
                        {
                            this.StatusChanged(this, this.pbMiddle.Text);
                        }
                    }
                    if (timeout > 0)
                    {
                        this.StatusTimer = new StateTimer((double) timeout);
                        this.StatusTimer.AutoReset = false;
                        if (handler == null)
                        {
                            handler = delegate (object sender, ElapsedEventArgs e) {
                                this.SetStatus("", new object[0]);
                            };
                        }
                        this.StatusTimer.Elapsed += handler;
                        this.StatusTimer.Start();
                    }
                    else
                    {
                        this.StatusTimer = null;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void SetStatusButtons(int group)
        {
            foreach (MenuItem item in this.skinDropDownStatus.Menu.MenuItems)
            {
                item.Visible = Convert.ToInt32(item.Tag) == group;
            }
            this.skinDropDownStatus.Refresh();
        }

        private void SetToolTips()
        {
            this.msQuickButtons.ShowItemToolTips = false;
            foreach (ToolStripItem item in this.msQuickButtons.Items)
            {
                item.MouseHover += new EventHandler(this.item_MouseHover);
                item.MouseMove += new MouseEventHandler(this.item_MouseMove);
            }
        }

        public void SetUnfocusImage(object sender, EventArgs e)
        {
            if (this.IsActiveWindow)
            {
                this.mIsActiveWindow = false;
                this.pbTopLeft.Image = SkinManager.GetImage("topleft.png");
                this.pbTopLeft.Refresh();
                this.pbClose.Image = SkinManager.GetImage("close_inactive.png");
                this.pbMinimize.Image = SkinManager.GetImage("minimize_inactive.png");
                if (this.mIsMaximized)
                {
                    this.pbRestore.Image = SkinManager.GetImage("restore_inactive.png");
                }
                else
                {
                    this.pbRestore.Image = SkinManager.GetImage("maximize_inactive.png");
                }
            }
        }

        private void SetUserStatus(string senderName, string status)
        {
            StatusTextLine line = this.ChatContainers.Peek(senderName);
            if (line != null)
            {
                UserStatus status2 = UserStatus.FindByDescription(status);
                line.StatusIcon = status2.Icon;
            }
            this.ChatroomQueue.Enqueue((VGen0)delegate {
                this.pnlUserListChat.RefreshData();
            }, new object[0]);
        }

        internal void SetUserVisibility(string name, bool visible)
        {
            VGen0 target = null;
            User user;
            if (this.TryFindUser(name, out user))
            {
                if (!user.IsAdmin)
                {
                    AuditLog.WriteLine("AM1 command attempted by non-admin member {0}", new object[] { name });
                }
                else
                {
                    user.Visible = visible;
                    user.Online = visible;
                    if (Chatroom.InChatroom)
                    {
                        if (target == null)
                        {
                            target = delegate {
                                if (visible)
                                {
                                    Chatroom current = Chatroom.Current;
                                    current.HiddenPlayers--;
                                    this.AddChatParticipant(user);
                                }
                                else
                                {
                                    Chatroom chatroom2 = Chatroom.Current;
                                    chatroom2.HiddenPlayers++;
                                    this.RemoveChatParticipant(user.Name);
                                    if (User.Current.IsAdmin)
                                    {
                                        this.AddChatParticipant(user);
                                    }
                                }
                            };
                        }
                        this.ChatroomQueue.Enqueue(target, new object[0]);
                    }
                }
            }
        }

        [DllImport("user32.DLL")]
        private static extern int SetWindowRgn(int hWnd, int hRgn, int bRedraw);
        internal void ShowDlgAbout()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgAbout == null) || this.DlgAbout.Disposing) || this.DlgAbout.IsDisposed)
            {
                this.DlgAbout = new GPG.Multiplayer.Client.DlgAbout(this);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgAbout = null;
                    };
                }
                this.DlgAbout.FormClosed += handler;
                this.DlgAbout.ShowDialog(this);
            }
        }

        internal void ShowDlgActiveEfforts()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgActiveEfforts == null) || this.DlgActiveEfforts.Disposing) || this.DlgActiveEfforts.IsDisposed)
            {
                this.DlgActiveEfforts = new GPG.Multiplayer.Client.Volunteering.DlgActiveEfforts();
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgActiveEfforts = null;
                    };
                }
                this.DlgActiveEfforts.FormClosed += handler;
                this.DlgActiveEfforts.Show();
            }
            else
            {
                this.DlgActiveEfforts.BringToFront();
            }
        }

        internal void ShowDlgContentManager()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgContentManager == null) || this.DlgContentManager.Disposing) || this.DlgContentManager.IsDisposed)
            {
                this.DlgContentManager = new GPG.Multiplayer.Client.Vaulting.DlgContentManager();
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgContentManager = null;
                    };
                }
                this.DlgContentManager.FormClosed += handler;
                this.DlgContentManager.Show();
            }
            else
            {
                if (this.DlgContentManager.WindowState == FormWindowState.Minimized)
                {
                    this.DlgContentManager.WindowState = FormWindowState.Normal;
                }
                this.DlgContentManager.BringToFront();
            }
        }

        internal void ShowDlgEmotes()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgEmotes == null) || this.DlgEmotes.Disposing) || this.DlgEmotes.IsDisposed)
            {
                this.DlgEmotes = new GPG.Multiplayer.Client.DlgEmotes(this);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgEmotes = null;
                    };
                }
                this.DlgEmotes.FormClosed += handler;
                this.DlgEmotes.Show();
            }
            else
            {
                this.DlgEmotes.BringToFront();
            }
        }

        internal void ShowDlgGameKeys()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgGameKeys == null) || this.DlgGameKeys.Disposing) || this.DlgGameKeys.IsDisposed)
            {
                this.DlgGameKeys = new GPG.Multiplayer.Client.DlgGameKeys(this);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgGameKeys = null;
                    };
                }
                this.DlgGameKeys.FormClosed += handler;
                this.DlgGameKeys.Show();
            }
            else
            {
                this.DlgGameKeys.BringToFront();
            }
        }

        internal void ShowDlgKeywordSearch()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgKeywordSearch == null) || this.DlgKeywordSearch.Disposing) || this.DlgKeywordSearch.IsDisposed)
            {
                foreach (DlgBase base2 in this.ActiveDialogs.ToArray())
                {
                    if (base2 is GPG.Multiplayer.Client.SolutionsLib.DlgKeywordSearch)
                    {
                        this.ActiveDialogs.Remove(base2);
                    }
                }
                this.DlgKeywordSearch = new GPG.Multiplayer.Client.SolutionsLib.DlgKeywordSearch();
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.ActiveDialogs.Remove(this.DlgKeywordSearch);
                        this.DlgKeywordSearch = null;
                    };
                }
                this.DlgKeywordSearch.FormClosed += handler;
                this.DlgKeywordSearch.Show();
            }
            else
            {
                this.DlgKeywordSearch.BringToFront();
            }
        }

        internal void ShowDlgKeywordSearch(string search, bool append)
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgKeywordSearch == null) || this.DlgKeywordSearch.Disposing) || this.DlgKeywordSearch.IsDisposed)
            {
                foreach (DlgBase base2 in this.ActiveDialogs.ToArray())
                {
                    if (base2 is GPG.Multiplayer.Client.SolutionsLib.DlgKeywordSearch)
                    {
                        this.ActiveDialogs.Remove(base2);
                    }
                }
                this.DlgKeywordSearch = new GPG.Multiplayer.Client.SolutionsLib.DlgKeywordSearch(this, search);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.ActiveDialogs.Remove(this.DlgKeywordSearch);
                        this.DlgKeywordSearch = null;
                    };
                }
                this.DlgKeywordSearch.FormClosed += handler;
                this.DlgKeywordSearch.Show();
            }
            else
            {
                this.DlgKeywordSearch.DoSearch(search, append);
                this.DlgKeywordSearch.BringToFront();
            }
        }

        internal void ShowDlgOptions()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgOptions == null) || this.DlgOptions.Disposing) || this.DlgOptions.IsDisposed)
            {
                this.DlgOptions = new GPG.Multiplayer.Client.DlgOptions(this);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgOptions = null;
                    };
                }
                this.DlgOptions.FormClosed += handler;
                this.DlgOptions.Show();
            }
            else
            {
                this.DlgOptions.BringToFront();
            }
        }

        internal void ShowDlgSearchReplays()
        {
            this.ShowDlgSearchReplays(null);
        }

        internal void ShowDlgSearchReplays(string playerName)
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgSearchReplays == null) || this.DlgSearchReplays.Disposing) || this.DlgSearchReplays.IsDisposed)
            {
                this.DlgSearchReplays = new GPG.Multiplayer.Client.Games.SupremeCommander.DlgSearchReplays(this, playerName);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgSearchReplays = null;
                    };
                }
                this.DlgSearchReplays.FormClosed += handler;
                this.DlgSearchReplays.Show();
            }
            else
            {
                this.DlgSearchReplays.BringToFront();
                this.DlgSearchReplays.DoPlayerSearch(playerName);
            }
        }

        internal void ShowDlgSearchResults(Solution[] results)
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgSearchResults == null) || this.DlgSearchResults.Disposing) || this.DlgSearchResults.IsDisposed)
            {
                this.DlgSearchResults = new GPG.Multiplayer.Client.SolutionsLib.DlgSearchResults(this, results);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.ActiveDialogs.Remove(this.DlgSearchResults);
                        this.DlgSearchResults = null;
                    };
                }
                this.DlgSearchResults.FormClosed += handler;
                this.DlgSearchResults.Show();
            }
            else
            {
                this.DlgSearchResults.ResultSet = results;
                this.DlgSearchResults.RefreshResults();
                this.DlgSearchResults.BringToFront();
            }
        }

        internal void ShowDlgSelectChannel()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgSelectChannel == null) || this.DlgSelectChannel.Disposing) || this.DlgSelectChannel.IsDisposed)
            {
                this.DlgSelectChannel = new GPG.Multiplayer.Client.DlgSelectChannel(this);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgSelectChannel = null;
                    };
                }
                this.DlgSelectChannel.FormClosed += handler;
                this.DlgSelectChannel.Show();
            }
            else
            {
                this.DlgSelectChannel.BringToFront();
            }
        }

        internal void ShowDlgSelectGame()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgSelectGame == null) || this.DlgSelectGame.Disposing) || this.DlgSelectGame.IsDisposed)
            {
                this.DisableGameButtons();
                this.DlgSelectGame = new GPG.Multiplayer.Client.DlgSelectGame(this);
                this.DlgSelectGame.JoinGame += new EventHandler(this.OnSelectCustomGame);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgSelectGame.JoinGame -= new EventHandler(this.OnSelectCustomGame);
                        this.DlgSelectGame = null;
                    };
                }
                this.DlgSelectGame.FormClosed += handler;
                this.DlgSelectGame.Show();
            }
            else
            {
                this.DlgSelectGame.BringToFront();
            }
        }

        internal void ShowDlgSolution(Solution solution)
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgSolution == null) || this.DlgSolution.Disposing) || this.DlgSolution.IsDisposed)
            {
                this.DlgSolution = new GPG.Multiplayer.Client.SolutionsLib.DlgSolution(this, solution);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.ActiveDialogs.Remove(this.DlgKeywordSearch);
                        this.DlgKeywordSearch = null;
                    };
                }
                this.DlgSolution.FormClosed += handler;
                this.DlgSolution.Show();
            }
            else
            {
                this.DlgSolution.AddSolution(solution);
                this.DlgSolution.BringToFront();
            }
        }

        internal void ShowDlgSolution(int id)
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgSolution == null) || this.DlgSolution.Disposing) || this.DlgSolution.IsDisposed)
            {
                this.DlgSolution = new GPG.Multiplayer.Client.SolutionsLib.DlgSolution(this, id);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgKeywordSearch = null;
                    };
                }
                this.DlgSolution.FormClosed += handler;
                this.DlgSolution.Show();
            }
            else
            {
                this.DlgSolution.LookupSolution(id);
                this.DlgSolution.BringToFront();
            }
        }

        internal void ShowDlgTeamGame()
        {
            this.ShowDlgTeamGame(null);
        }

        internal void ShowDlgTeamGame(TeamGame team)
        {
            FormClosedEventHandler handler = null;
            if (!(User.Current.IsAdmin || this.IsGameCurrent))
            {
                this.UpdateSupCom();
            }
            else if (!this.IsSupComPatching)
            {
                if (this.IsInTeamGame)
                {
                    this.DlgTeamGame.BringToFront();
                }
                else
                {
                    if (team != null)
                    {
                        this.DlgTeamGame = new GPG.Multiplayer.Client.Games.SupremeCommander.DlgTeamGame(team);
                    }
                    else
                    {
                        this.DlgTeamGame = new GPG.Multiplayer.Client.Games.SupremeCommander.DlgTeamGame();
                    }
                    if (handler == null)
                    {
                        handler = delegate (object s, FormClosedEventArgs e) {
                            this.DlgTeamGame = null;
                        };
                    }
                    this.DlgTeamGame.FormClosed += handler;
                    this.DlgTeamGame.Show();
                }
            }
        }

        internal void ShowDlgUserFeedback()
        {
            FormClosedEventHandler handler = null;
            if (((this.DlgUserFeedback == null) || this.DlgUserFeedback.Disposing) || this.DlgUserFeedback.IsDisposed)
            {
                this.DlgUserFeedback = new GPG.Multiplayer.Client.DlgUserFeedback(this);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.DlgUserFeedback = null;
                    };
                }
                this.DlgUserFeedback.FormClosed += handler;
                this.DlgUserFeedback.Show(this);
            }
            else
            {
                this.DlgUserFeedback.BringToFront();
            }
        }

        private void ShowFeedback()
        {
            this.ShowDlgUserFeedback();
        }

        internal FrmStatsLadder ShowFrmStatsLadder(string kind)
        {
            FormClosedEventHandler handler = null;
            bool flag = false;
            FrmStatsLadder item = null;
            foreach (FrmStatsLadder ladder2 in this.FrmStatsLadders)
            {
                if (ladder2.Category == kind)
                {
                    item = ladder2;
                    flag = true;
                    break;
                }
            }
            if (!(flag && ((item == null) || (!item.Disposing && !item.IsDisposed))))
            {
                item = new FrmStatsLadder(this, kind);
                if (handler == null)
                {
                    handler = delegate (object s, FormClosedEventArgs e) {
                        this.FrmStatsLadders.Remove(s as FrmStatsLadder);
                    };
                }
                item.FormClosed += handler;
                this.FrmStatsLadders.Add(item);
                item.Show();
                return item;
            }
            item.BringToFront();
            return item;
        }

        private void ShowPatchMessage()
        {
            VGen0 method = null;
            if (Program.Settings.SupcomPrefs.ShowPatchMsg)
            {
                try
                {
                    if (method == null)
                    {
                        method = delegate {
                            if (((this.DlgUpdate == null) || this.DlgUpdate.Disposing) || this.DlgUpdate.IsDisposed)
                            {
                                if ((GameInformation.SelectedGame.GameDescription != null) && (GameInformation.SelectedGame.GameDescription != string.Empty))
                                {
                                    this.DlgUpdate = new DlgGameUpdate(this, GameInformation.SelectedGame.GameDescription);
                                }
                                else
                                {
                                    this.DlgUpdate = new DlgGameUpdate(this, "Selected Product");
                                }
                                this.DlgUpdate.Show(this);
                            }
                            else
                            {
                                this.DlgUpdate.BringToFront();
                            }
                        };
                    }
                    base.BeginInvoke(method);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    this.DlgUpdate = null;
                }
            }
        }

        internal void ShowWebPage(string url)
        {
            this.ShowWebPage(url, null);
        }

        internal void ShowWebPage(string url, WebBrowserNavigatingEventArgs e)
        {
            ThreadStart start = null;
            if ((url != null) && (url.Length >= 1))
            {
                try
                {
                    if (start == null)
                    {
                        start = delegate {
                            try
                            {
                                Process.Start(new ProcessStartInfo(this.GetDefaultBrowser()) { Arguments = url, UseShellExecute = false });
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    Thread thread = new Thread(start);
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public void ShowWebStats(int id)
        {
            if (ConfigSettings.GetBool("WebStatsEnabled", false))
            {
                string url = string.Format(ConfigSettings.GetString("WebStatProfile", "http://gpgnet.gaspowered.com/scStats/PlayerProfile.aspx?PlayerID={0}"), id);
                this.ShowWebPage(url);
            }
        }

        private void ShowWelcome()
        {
            this.ShowWelcome(this.HOME_URL);
        }

        private void ShowWelcome(string url)
        {
            VGen1 method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate (object objurl) {
                        this.ShowWelcome(objurl.ToString());
                    };
                }
                base.BeginInvoke(method, new object[] { url });
            }
            else
            {
                try
                {
                    if (this.mWelcomePage == null)
                    {
                        this.mWelcomePage = new WelcomePage();
                        this.mWelcomePage.OnJoinChat += new EventHandler(this.mWelcomePage_OnJoinChat);
                        this.mWelcomePage.OnWebLinkClick += new WebLinkClick(this.mWelcomePage_OnWebLinkClick);
                        this.mWelcomePage.Visible = false;
                        this.mWelcomePage.Parent = this;
                        this.mWelcomePage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                    }
                    this.mWelcomePage.Left = this.pbLeftBorder.Right;
                    this.mWelcomePage.Top = this.pbMiddle.Bottom;
                    this.mWelcomePage.Height = (base.ClientRectangle.Height - this.pbMiddle.Bottom) - this.pBottom.Height;
                    this.mWelcomePage.Width = (base.ClientRectangle.Width - this.pbLeftBorder.Width) - this.pbRightBorder.Width;
                    this.mWelcomePage.BringToFront();
                    if (url != this.mWelcomePage.CurrentUrl)
                    {
                        if (url != "")
                        {
                            this.mWelcomePage.ChangeURL(url);
                        }
                        else
                        {
                            this.mWelcomePage.ChangeURL(this.mNotFound);
                        }
                    }
                    else
                    {
                        this.mWelcomePage.RefreshPage();
                    }
                    this.mWelcomePage.Visible = true;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int showType);
        private void SizeToFit(Point point)
        {
            if (this.mIsResizing)
            {
                this.mIsResizing = false;
                ControlPaint.DrawReversibleFrame(this.mResizeRect, System.Drawing.Color.Empty, FrameStyle.Thick);
                base.Width = this.mResizeRect.Width + this.mWidthDiff;
                base.Height = this.mResizeRect.Height + this.mHeightDiff;
            }
            this.mIsMoving = false;
            this.mLastX = point.X;
            this.mLastY = point.Y;
            this.mLastMouseX = Control.MousePosition.X;
            this.mLastMouseY = Control.MousePosition.Y;
            this.mLastWidth = base.Width;
            this.mLastHeight = base.Height;
        }

        private void skinLabelClanName_Click(object sender, EventArgs e)
        {
            if (Clan.Current != null)
            {
                this.OnViewClanProfileByName(Clan.Current.Name);
            }
        }

        private void SmartPatchComplete(object sender, AsyncCompletedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                if (!e.Cancelled)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(this.GetLocalPatchFile()) {
                        Arguments = "/APP " + GameInformation.SelectedGame.GameDescription + " /URL " + GameInformation.SelectedGame.PatchURL + " /EXE " + GameInformation.SelectedGame.GameLocation + " /VERSION " + GameInformation.SelectedGame.CurrentVersion,
                        UseShellExecute = true
                    };
                    this.SetStatus("<LOC>Downloading {0} update...", new object[] { GameInformation.SelectedGame.GameDescription });
                    Process.Start(startInfo).WaitForExit();
                    this.SetStatus("", new object[0]);
                    this.SetPatchButtonsCompleted();
                }
                else
                {
                    this.SetStatus("<LOC>Download failed, aborting update...", new object[0]);
                }
            });
        }

        private void SortChatParticipants()
        {
            try
            {
                lock (this.ChatContainers)
                {
                    foreach (TextContainer<StatusTextLine> container in this.ChatContainers)
                    {
                        if (container.Modified)
                        {
                            lock (container)
                            {
                                container.Sort();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void spaceSiegeLobbyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgGameLobby lobby = DlgGameLobby.HostGame();
            this.CurrentGameLobby = lobby;
            lobby.Show();
        }

        private void splitContainerBody_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.AlignChatTabs();
        }

        private void splitContainerChatAndInput_Paint(object sender, PaintEventArgs e)
        {
            this.splitContainerChatAndInput.BackColor = Program.Settings.StylePreferences.HighlightColor3;
        }

        private void statsPivotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DlgSupcomStats().Show();
        }

        private void StatusButtonRankedGameCancel_Click(object sender, EventArgs e)
        {
            this.CancelRankedGame();
        }

        private void StyleApplication(object sender, PropertyChangedEventArgs e)
        {
            Program.Settings.StylePreferences.StyleChildControl(this);
            this.textBoxMsg.Properties.Appearance.BorderColor = Program.Settings.StylePreferences.HighlightColor1;
            this.gvChat.Appearance.Empty.BackColor = Program.Settings.StylePreferences.MasterBackColor;
            this.gvChat.Appearance.Row.BackColor = Program.Settings.StylePreferences.MasterBackColor;
            this.gvChat.Appearance.HideSelectionRow.BackColor = Program.Settings.StylePreferences.MasterBackColor;
            this.ClearChatRowCache();
            this.gpgChatGrid.Size = this.gpgChatGrid.Size;
            this.gvChat.RefreshData();
            this.Refresh();
        }

        private void StyleChatLine(TextLine line)
        {
            if (line != null)
            {
                User tag = line.Tag as User;
                if (tag != null)
                {
                    if (tag.Equals(User.Current))
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.SelfColor, Program.Settings.Chat.Appearance.SelfFont);
                    }
                    else if (tag.IsSystem)
                    {
                        if (tag.Equals(User.Error))
                        {
                            line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.ErrorColor, Program.Settings.Chat.Appearance.ErrorFont);
                        }
                        else if (tag.Equals(User.Event))
                        {
                            line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.EventColor, Program.Settings.Chat.Appearance.EventFont);
                        }
                        else if (tag.Equals(User.System))
                        {
                            line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.SystemColor, Program.Settings.Chat.Appearance.SystemFont);
                        }
                        else if (tag.Equals(User.Game))
                        {
                            line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.GameColor, Program.Settings.Chat.Appearance.GameFont);
                        }
                    }
                    else if (tag.IsAdmin)
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.AdminColor, Program.Settings.Chat.Appearance.AdminFont);
                    }
                    else if (tag.IsModerator)
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.ModeratorColor, Program.Settings.Chat.Appearance.ModeratorFont);
                    }
                    else if (tag.IsClanMate)
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.ClanColor, Program.Settings.Chat.Appearance.ClanFont);
                    }
                    else if (tag.IsFriend)
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.FriendsColor, Program.Settings.Chat.Appearance.FriendsFont);
                    }
                    else
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.DefaultColor, Program.Settings.Chat.Appearance.DefaultFont);
                    }
                }
            }
        }

        internal void StyleChatroom(object sender, PropertyChangedEventArgs e)
        {
            foreach (ChatLine line in this.mChatLines)
            {
                this.StyleChatLine(line);
            }
            this.gvChat.Invalidate();
            foreach (TextContainer<StatusTextLine> container in this.ChatContainers)
            {
                foreach (StatusTextLine line2 in container)
                {
                    this.StyleParticipantLine(line2);
                }
            }
            foreach (TextContainer container2 in this.FriendContainers)
            {
                foreach (TextLine line3 in container2)
                {
                    this.StyleFriendLine(line3);
                }
            }
            foreach (TextContainer container2 in this.ClanContainers)
            {
                foreach (TextLine line3 in container2)
                {
                    this.StyleClanLine(line3);
                }
            }
            this.GatheringDisplaycontrol.Refresh();
            this.Refresh();
        }

        private void StyleClanLine(TextLine line)
        {
            if ((line != null) && (line.TextSegments.Count > 0))
            {
                ClanMember tag = line.Tag as ClanMember;
                if (tag != null)
                {
                    if (tag.Online)
                    {
                        line.TextColor = Program.Settings.Chat.Appearance.ClanColor;
                        line.TextSegments[0].TextColor = Program.Settings.Chat.Appearance.ClanColor;
                    }
                    else
                    {
                        line.TextColor = Program.Settings.Chat.Appearance.UnavailableColor;
                        line.TextSegments[0].TextColor = Program.Settings.Chat.Appearance.UnavailableColor;
                    }
                }
            }
        }

        private void StyleFriendLine(TextLine line)
        {
            if ((line != null) && (line.TextSegments.Count > 0))
            {
                User tag = line.Tag as User;
                if (tag != null)
                {
                    if (tag.Online)
                    {
                        line.TextFont = Program.Settings.StylePreferences.MasterFont;
                        line.TextColor = Program.Settings.Chat.Appearance.FriendsColor;
                    }
                    else
                    {
                        line.TextColor = Program.Settings.Chat.Appearance.UnavailableColor;
                        line.TextFont = Program.Settings.StylePreferences.MasterFont;
                    }
                    if (tag.IsInClan)
                    {
                        if (line.TextSegments.Count < 2)
                        {
                            line.AddSegment(new TextSegment(tag.ClanAbbreviation, Program.Settings.Chat.Appearance.ClanColor, Program.Settings.Chat.Appearance.ClanTagFont));
                        }
                        else
                        {
                            line.TextSegments[1].TextColor = Program.Settings.Chat.Appearance.ClanColor;
                            line.TextSegments[1].TextFont = Program.Settings.Chat.Appearance.ClanTagFont;
                        }
                    }
                }
            }
        }

        private void StyleParticipantLine(TextLine line)
        {
            try
            {
                if ((line != null) && (line.TextSegments.Count > 0))
                {
                    User tag = line.Tag as User;
                    if (tag != null)
                    {
                        if ((tag.IsAway || tag.IsDND) || tag.IsIgnored)
                        {
                            line.TextSegments[0].TextColor = Program.Settings.Chat.Appearance.UnavailableColor;
                            line.TextSegments[0].TextFont = Program.Settings.Chat.Appearance.DefaultFont;
                        }
                        else if (tag.IsAdmin || ((Chatroom.InChatroom && !Chatroom.Current.IsPersistent) && tag.IsChannelOperator))
                        {
                            line.TextSegments[0].TextColor = Program.Settings.Chat.Appearance.AdminColor;
                            line.TextSegments[0].TextFont = new Font(Program.Settings.Chat.Appearance.DefaultFont, FontStyle.Bold);
                        }
                        else
                        {
                            line.TextSegments[0].TextColor = Program.Settings.Chat.Appearance.DefaultColor;
                            line.TextSegments[0].TextFont = Program.Settings.Chat.Appearance.DefaultFont;
                        }
                        if (tag.IsInClan)
                        {
                            if (line.TextSegments.Count < 2)
                            {
                                line.AddSegment(new TextSegment(tag.ClanAbbreviation, Program.Settings.Chat.Appearance.ClanColor, Program.Settings.Chat.Appearance.ClanTagFont));
                            }
                            else
                            {
                                line.TextSegments[1].TextColor = Program.Settings.Chat.Appearance.ClanColor;
                                line.TextSegments[1].TextFont = Program.Settings.Chat.Appearance.ClanTagFont;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void SubmitReplay()
        {
            SupComGameManager mSupcomGameManager = this.mSupcomGameManager;
            if (mSupcomGameManager == null)
            {
                mSupcomGameManager = SupcomAutomatch.GetSupcomAutomatch().GetManager();
            }
            if (((mSupcomGameManager == null) || ((mSupcomGameManager.GameState != GameState.Lobby) && (mSupcomGameManager.GameState != GameState.StartingApp))) && (ConfigSettings.GetBool("SubmitReplays", true) && (((mSupcomGameManager.GameName == null) || (this.mGameName.IndexOf("AUTOMATCH") >= 0)) || ConfigSettings.GetBool("SubmitCustomReplays", true))))
            {
                string path = null;
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string str3 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                if (GameInformation.SelectedGame.GameID == 2)
                {
                    folderPath = (folderPath + @"\Gas Powered Games\SupremeCommander\Replays\" + User.Current.Name) + @"\LastGame.SupremeCommanderReplay";
                }
                else if (GameInformation.SelectedGame.GameID == 0x11)
                {
                    folderPath = (folderPath + @"\Gas Powered Games\Supreme Commander Forged Alliance\Replays\" + User.Current.Name) + @"\LastGame.SCFAReplay";
                }
                else if (GameInformation.SelectedGame.GameID == 0x12)
                {
                    folderPath = (folderPath + @"\Gas Powered Games\Supreme Commander Forged Alliance Beta\Replays\" + User.Current.Name) + @"\LastGame.SCFABReplay";
                }
                if (GameInformation.SelectedGame.GameID == 2)
                {
                    str3 = (str3 + @"\My Games\Gas Powered Games\SupremeCommander\Replays\" + User.Current.Name) + @"\LastGame.SupremeCommanderReplay";
                }
                else if (GameInformation.SelectedGame.GameID == 0x11)
                {
                    str3 = (str3 + @"\My Games\Gas Powered Games\Supreme Commander Forged Alliance\Replays\" + User.Current.Name) + @"\LastGame.SCFAReplay";
                }
                else if (GameInformation.SelectedGame.GameID == 0x12)
                {
                    str3 = (str3 + @"\My Games\Gas Powered Games\Supreme Commander Forged Alliance Beta\Replays\" + User.Current.Name) + @"\LastGame.SCFABReplay";
                }
                if (System.IO.File.Exists(folderPath) && System.IO.File.Exists(str3))
                {
                    if (new FileInfo(folderPath).LastWriteTime > new FileInfo(str3).LastWriteTime)
                    {
                        path = folderPath;
                    }
                    else
                    {
                        path = str3;
                    }
                }
                else if (!(!System.IO.File.Exists(folderPath) || System.IO.File.Exists(str3)))
                {
                    path = folderPath;
                }
                else if (!(System.IO.File.Exists(folderPath) || !System.IO.File.Exists(str3)))
                {
                    path = str3;
                }
                else
                {
                    path = null;
                }
                if ((((path != null) && System.IO.File.Exists(path)) && Program.Settings.SupcomPrefs.Replays.ShowReplayDialog) && !mSupcomGameManager.DoReplay)
                {
                    DlgSubmitReplay replay = new DlgSubmitReplay();
                    try
                    {
                        if (mSupcomGameManager.GameName.IndexOf("AUTOMATCH") >= 0)
                        {
                            try
                            {
                                string str4 = "Ranked" + SupcomAutomatch.GetSupcomAutomatch().Kind;
                                replay.GameType = (SupcomLookups._GameTypes) System.Enum.Parse(typeof(SupcomLookups._GameTypes), str4, true);
                            }
                            catch
                            {
                                replay.GameType = SupcomLookups._GameTypes.Custom;
                            }
                        }
                        else
                        {
                            replay.GameType = SupcomLookups._GameTypes.Custom;
                        }
                        if (replay.GameType == SupcomLookups._GameTypes.Ranked1v1)
                        {
                            foreach (SupcomPlayerInfo info in mSupcomGameManager.GameInfo.Players)
                            {
                                if (info.PlayerName.ToLower() == User.Current.Name.ToLower())
                                {
                                    try
                                    {
                                        replay.PlayerFaction = (SupcomLookups._Factions) System.Enum.Parse(typeof(SupcomLookups._Factions), info.Faction, true);
                                    }
                                    catch
                                    {
                                        replay.PlayerFaction = SupcomLookups._Factions.Any;
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        replay.OpponentFaction = (SupcomLookups._Factions) System.Enum.Parse(typeof(SupcomLookups._Factions), info.Faction, true);
                                    }
                                    catch
                                    {
                                        replay.OpponentFaction = SupcomLookups._Factions.Any;
                                    }
                                }
                            }
                        }
                        replay.SetMap(mSupcomGameManager.mLaunchMap);
                        TimeSpan span = (TimeSpan) (DateTime.Now - mSupcomGameManager.GameInfo.StartTime);
                        replay.GameLength = (int) span.TotalSeconds;
                        string opponentname = "";
                        string str6 = "";
                        foreach (SupcomPlayerInfo info2 in mSupcomGameManager.GameInfo.Players)
                        {
                            if (info2.PlayerName != User.Current.Name)
                            {
                                opponentname = opponentname + str6 + info2.PlayerName;
                                str6 = ", ";
                            }
                        }
                        replay.SetOpponent(opponentname);
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                    replay.SetFile(path);
                    replay.Show();
                }
            }
        }

        private void SupcomGameExit()
        {
            VGen0 method = null;
            this.mSearchingForAutomatch = false;
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        Thread.Sleep(100);
                    }
                    catch (ThreadInterruptedException exception)
                    {
                        GPG.Logging.EventLog.WriteLine("The thread was woken up: " + exception.Message, new object[0]);
                    }
                }
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            try
                            {
                                this.CloseHomePage();
                                if ((this.BeforeGameChatroom != null) && (this.BeforeGameChatroom.Length > 0))
                                {
                                    this.JoinChat(this.BeforeGameChatroom);
                                }
                                this.SubmitReplay();
                                this.GatherLadderReports();
                                this.mSupcomGameManager = null;
                                this.ChangeStatus("<LOC>Online", StatusIcons.online);
                                this.SetStatusButtons(0);
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!base.Disposing && !base.IsDisposed)
                {
                    this.CloseHomePage();
                    if (this.BeforeGameChatroom != null)
                    {
                        this.JoinChat(this.BeforeGameChatroom);
                    }
                    this.SubmitReplay();
                    this.GatherLadderReports();
                    this.mSupcomGameManager = null;
                    this.ChangeStatus("<LOC>Online", StatusIcons.online);
                    this.SetStatusButtons(0);
                }
            }
            catch (Exception exception2)
            {
                ErrorLog.WriteLine(exception2);
            }
        }

        private void SyncChatroom(object sender, ElapsedEventArgs e)
        {
            VGen1 target = null;
            VGen1 gen2 = null;
            try
            {
                if (Chatroom.InChatroom)
                {
                    this.IsSynchronizingChatroom = true;
                    while (this.IsSortingChatroom)
                    {
                        Thread.Sleep(20);
                    }
                    GPG.Logging.EventLog.WriteLine("Synchronizing chatroom...", new object[0]);
                    MappedObjectList<User> objects = DataAccess.GetObjects<User>("GatheringPlayers", new object[] { Chatroom.CurrentName });
                    try
                    {
                        foreach (User user in Chatroom.GatheringParticipants.ToArray())
                        {
                            if (!objects.ContainsIndex("name", user.Name))
                            {
                                GPG.Logging.EventLog.WriteLine("{0} was out of sync (stuck in chat)", new object[] { user.Name });
                                if (target == null)
                                {
                                    target = delegate (object objname) {
                                        this.RemoveChatParticipant(objname.ToString());
                                    };
                                }
                                this.ChatroomQueue.Enqueue(target, new object[] { user.Name });
                            }
                        }
                    }
                    finally
                    {
                    }
                    foreach (User user in objects)
                    {
                        if (!Chatroom.GatheringParticipants.ContainsIndex("name", user.Name))
                        {
                            GPG.Logging.EventLog.WriteLine("{0} was out of sync (not in chat)", new object[] { user.Name });
                            if (gen2 == null)
                            {
                                gen2 = delegate (object objuser) {
                                    this.AddChatParticipant((User) objuser);
                                };
                            }
                            this.ChatroomQueue.Enqueue(gen2, new object[] { user });
                        }
                    }
                    objects = null;
                    GPG.Logging.EventLog.WriteLine("Room sync complete", new object[0]);
                }
                else if (!((Chatroom.Current != null) && Chatroom.Current.Equals(Chatroom.None)))
                {
                    this.LeaveChat();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                this.IsSynchronizingChatroom = false;
            }
        }

        internal void SystemEvent(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            if (this.mRanInitializeChat)
            {
                this.AddChat(User.Event, msg);
            }
            else
            {
                VGen0 onClick = this.FindLinks(msg, out msg);
                FrmSimpleMessage.DoNotification("<LOC>System Event", msg, onClick, new object[0]);
            }
        }

        internal void SystemMessage(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            if (this.mRanInitializeChat)
            {
                this.AddChat(User.System, msg);
            }
            else
            {
                VGen0 onClick = this.FindLinks(msg, out msg);
                FrmSimpleMessage.DoNotification("<LOC>System Message", msg, onClick, new object[0]);
            }
        }

        private void tcParticipants_Paint(object sender, PaintEventArgs e)
        {
        }

        private void TeamAutomatchExit(object sender, EventArgs e)
        {
            try
            {
                if (!this.mTeamAutomatchExit)
                {
                    this.mTeamAutomatchExit = true;
                    SupcomAutomatch.GetSupcomAutomatch().OnExit -= new EventHandler(this.TeamAutomatchExit);
                    this.OnTeamAutomatchExit();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void TeamAutomatchLaunchGame(object sender, EventArgs e)
        {
            try
            {
                if (this.IsInTeamGame)
                {
                    this.OnTeamAutomatchLaunchGame();
                    if (this.DlgTeamGame.Team.TeamLeader.IsSelf)
                    {
                        Messaging.SendCustomCommand(this.DlgTeamGame.Team.GetOtherMemberNames(), CustomCommands.TeamGameStart, new object[0]);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void testGameDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StateTimer timer = new StateTimer(5000.0);
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
            timer.Start();
        }

        private void textBoxMsg_EditValueChanging(object sender, ChangingEventArgs e)
        {
            e.NewValue = e.NewValue.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }

        public void textBoxMsg_KeyDown(object sender, KeyEventArgs e)
        {
            IEnumerator enumerator;
            int num;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (!this.textBoxMsg.Text.StartsWith("/"))
                    {
                        if (this.HistoryIndex < this.ChatHistory.Count)
                        {
                            this.HistoryIndex++;
                            enumerator = this.ChatHistory.GetEnumerator();
                            for (num = 0; enumerator.MoveNext(); num++)
                            {
                                if (num == this.HistoryIndex)
                                {
                                    this.textBoxMsg.Text = enumerator.Current.ToString();
                                    this.textBoxMsg.Select(0, 0);
                                    e.Handled = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                    this.SelectingTextList = true;
                    this.gpgTextListCommands.Focus();
                    this.SelectingTextList = false;
                    break;

                case Keys.Right:
                    return;

                case Keys.Down:
                    if (this.HistoryIndex > -1)
                    {
                        this.HistoryIndex--;
                        if (this.HistoryIndex != -1)
                        {
                            enumerator = this.ChatHistory.GetEnumerator();
                            for (num = 0; enumerator.MoveNext(); num++)
                            {
                                if (num == this.HistoryIndex)
                                {
                                    this.textBoxMsg.Text = enumerator.Current.ToString();
                                    this.textBoxMsg.Select(0, 0);
                                    e.Handled = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this.textBoxMsg.Text = "";
                        }
                    }
                    e.SuppressKeyPress = true;
                    return;

                case Keys.Space:
                    if ((this.textBoxMsg.Text.StartsWith("/") && (this.textBoxMsg.Text.Split(new char[] { ' ' }).Length < 2)) && (this.gpgTextListCommands.SelectedValue != null))
                    {
                        this.CommandSelected(this.gpgTextListCommands, new TextValEventArgs(this.gpgTextListCommands.SelectedValue));
                    }
                    return;

                case Keys.Return:
                    if ((this.textBoxMsg.Text.StartsWith("/") && (this.textBoxMsg.Text.Split(new char[] { ' ' }).Length < 2)) && (this.gpgTextListCommands.SelectedValue != null))
                    {
                        UserAction action;
                        if (UserAction.TryGetByName(this.textBoxMsg.Text, out action))
                        {
                            this.SendMessage();
                            e.SuppressKeyPress = true;
                        }
                        else
                        {
                            this.CommandSelected(this.gpgTextListCommands, new TextValEventArgs(this.gpgTextListCommands.SelectedValue));
                            this.textBoxMsg.Text = this.textBoxMsg.Text + " ";
                            this.textBoxMsg.Select(this.textBoxMsg.Text.Length, 0);
                        }
                    }
                    else
                    {
                        this.HistoryIndex = -1;
                        this.SendMessage();
                        e.SuppressKeyPress = true;
                    }
                    return;

                case Keys.Escape:
                    this.textBoxMsg.Text = "";
                    this.HistoryIndex = -1;
                    return;

                default:
                    return;
            }
            e.SuppressKeyPress = true;
        }

        private void textBoxMsg_LostFocus(object sender, EventArgs e)
        {
            if (!((!this.gpgTextListCommands.Visible || this.SelectingTextList) || this.gpgTextListCommands.ContainsFocus))
            {
                this.gpgTextListCommands.Visible = false;
            }
        }

        protected void this_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.mIsMaximized && (e.Button == MouseButtons.Left))
            {
                Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                if (this.HitTest(pt.X, pt.Y, 0, 0, base.Width, 50))
                {
                    if (!((this.pbClose.ClientRectangle.Contains(pt) || this.pbMinimize.ClientRectangle.Contains(pt)) || this.pbRestore.ClientRectangle.Contains(pt)))
                    {
                        this.mLastX = pt.X;
                        this.mLastY = pt.Y;
                        this.mIsMoving = true;
                    }
                }
                else if (this.HitTest(pt.X, pt.Y, base.Width - this.RESIZE_RECT, base.Height - (this.RESIZE_RECT + 20), base.Width, base.Height))
                {
                    this.mLastX = pt.X;
                    this.mLastY = pt.Y;
                    this.mStartResizing = true;
                }
            }
        }

        protected void this_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.mIsMaximized && (e.Button == MouseButtons.Left))
            {
                Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                if (this.HitTest(pt.X, pt.Y, 0, 0, base.Width, 50) && !((this.pbClose.ClientRectangle.Contains(pt) || this.pbMinimize.ClientRectangle.Contains(pt)) || this.pbRestore.ClientRectangle.Contains(pt)))
                {
                    this.mIsMoving = false;
                }
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.SystemMessage("{0}", new object[] { IsFullScreenAppRunning() });
        }

        private void ToggleAllowFriends()
        {
            AllowFriendsToBypassDND = !AllowFriendsToBypassDND;
            if (AllowFriendsToBypassDND)
            {
                this.SystemMessage(Loc.Get("<LOC>DND no longer applies to your friends."), new object[0]);
            }
            else
            {
                this.SystemMessage(Loc.Get("<LOC>DND now affects your friends."), new object[0]);
            }
        }

        public void ToggleDirectChallenge()
        {
            this.AllowDirectChallenge = !this.AllowDirectChallenge;
            if (this.AllowDirectChallenge)
            {
                this.SystemMessage(Loc.Get("<LOC>You are accepting direct ladder challenges."), new object[0]);
            }
            else
            {
                this.SystemMessage(Loc.Get("<LOC>You are no longer accepting direct ladder challenges."), new object[0]);
            }
        }

        public void ToggleDNDStatus()
        {
            this.SetDNDStatus(!User.Current.IsDND);
        }

        private void Translate(string source)
        {
            foreach (int num in this.gvChat.GetSelectedRows())
            {
                ChatLine row = this.gvChat.GetRow(num) as ChatLine;
                if (row != null)
                {
                    row.Text = TranslateUtil.GetTranslatedText(row.Text, "en", source.ToLower());
                    Messaging.AutoTranslate(row.PlayerInfo, source);
                }
            }
            this.gvChat.RefreshData();
        }

        public bool TryFindMember(string name, out IUser user)
        {
            return this.TryFindMember(name, true, out user);
        }

        public bool TryFindMember(string name, bool doLookup, out IUser user)
        {
            User user2;
            ClanMember member;
            if (Chatroom.InChatroom && Chatroom.GatheringParticipants.TryFindByIndex("name", name, out user2))
            {
                user = user2;
                return true;
            }
            if (User.CurrentFriends.TryFindByIndex("name", name, out user2))
            {
                user = user2;
                return true;
            }
            if (Clan.CurrentMembers.TryFindByIndex("name", name, out member))
            {
                user = member;
                return true;
            }
            if (doLookup && DataAccess.TryGetObject<User>("GetPlayerDetails", out user2, new object[] { name }))
            {
                user = user2;
                return true;
            }
            user = null;
            return false;
        }

        public bool TryFindUser(string name, out User user)
        {
            return this.TryFindUser(name, true, out user);
        }

        public bool TryFindUser(string name, bool doLookup, out User user)
        {
            if (Chatroom.InChatroom && Chatroom.GatheringParticipants.TryFindByIndex("name", name, out user))
            {
                return true;
            }
            if (User.CurrentFriends.TryFindByIndex("name", name, out user))
            {
                return true;
            }
            if (doLookup && DataAccess.TryGetObject<User>("GetPlayerDetails", out user, new object[] { name }))
            {
                return true;
            }
            user = null;
            return false;
        }

        internal bool UnbanUser(string playerName)
        {
            User user;
            if (User.Current.Name.ToLower() == playerName.ToLower())
            {
                this.ErrorMessage("<LOC>You cannot unban yourself from chat.", new object[0]);
                return false;
            }
            if (!Chatroom.InChatroom)
            {
                this.ErrorMessage("<LOC>You are not in a chatroom.", new object[0]);
                return false;
            }
            if (!this.TryFindUser(playerName, true, out user))
            {
                this.ErrorMessage("<LOC>Unable to locate user {0}", new object[] { playerName });
                return false;
            }
            if (user.IsAdmin)
            {
                this.ErrorMessage("<LOC>You cannot unban {0}, administrators cannot be banned from chat.", new object[] { playerName });
                return false;
            }
            if (Chatroom.Current.IsClanRoom)
            {
                if (!((User.Current.IsAdmin || User.Current.IsModerator) || ClanMember.Current.CanTargetAbility(ClanAbility.Ban, user.Name)))
                {
                    this.ErrorMessage("<LOC>You do not have permission to unban {0}, you must be an administrator or higher ranking clan member to unban players from chat.", new object[] { playerName });
                    return false;
                }
                if (!DataAccess.ExecuteQuery("UnbanPlayer", new object[] { user.Name, Chatroom.CurrentName }))
                {
                    this.ErrorMessage("<LOC>{0} has not been banned from this chatroom.", new object[] { user.Name });
                    return false;
                }
                this.SystemMessage("<LOC>You have unbanned {0} from {1}", new object[] { user.Name, Chatroom.CurrentName });
                Messaging.SendCustomCommand(user.Name, CustomCommands.SystemEvent, new object[] { "<LOC>You have been unbanned from {0}", Chatroom.CurrentName });
                return true;
            }
            if ((!User.Current.IsAdmin && !User.Current.IsModerator) && (Chatroom.Current.IsPersistent || !User.Current.IsChannelOperator))
            {
                this.ErrorMessage("<LOC>You do not have permission to unban {0}, you must be and administrator or channel operator to unban players from chat.", new object[] { playerName });
                return false;
            }
            if (!DataAccess.ExecuteQuery("UnbanPlayer", new object[] { user.Name, Chatroom.CurrentName }))
            {
                this.ErrorMessage("<LOC>{0} has not been banned from this chatroom.", new object[] { user.Name });
                return false;
            }
            this.SystemMessage("<LOC>You have unbanned {0} from {1}", new object[] { user.Name, Chatroom.CurrentName });
            Messaging.SendCustomCommand(user.Name, CustomCommands.SystemEvent, new object[] { "<LOC>You have been unbanned from {0}", Chatroom.CurrentName });
            return true;
        }

        internal void UnignorePlayer(string name)
        {
            try
            {
                User user;
                if (this.TryFindUser(name, out user))
                {
                    if (user.IsIgnored)
                    {
                        if (DataAccess.ExecuteQuery("UnignorePlayer", new object[] { name }))
                        {
                            User.IgnoredPlayers.Remove(user.ID);
                            this.SystemMessage("<LOC>You are no longer ignoring {0}.", new object[] { name });
                            this.RefreshChatParticipant(name);
                            foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                            {
                                if ((!chat.Disposing && !chat.IsDisposed) && chat.ChatTarget.Equals(user))
                                {
                                    chat.RefreshToolstrip();
                                }
                            }
                        }
                        else
                        {
                            ErrorLog.WriteLine("Error unignoring player {0}, they may already be ignored.", new object[] { name });
                        }
                    }
                }
                else
                {
                    this.ErrorMessage("<LOC>Unable to find {0}.", new object[] { name });
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void UpdateCommandPopup(object sender, EventArgs e)
        {
            try
            {
                if ((this.textBoxMsg.Text != null) && this.textBoxMsg.Text.StartsWith("/"))
                {
                    this.gpgTextListCommands.TextLines = UserAction.GetFilteredActions(this.textBoxMsg.Text);
                    for (int i = 0; i < this.gpgTextListCommands.TextLines.Length; i++)
                    {
                        UserAction action = this.gpgTextListCommands.TextLines[i] as UserAction;
                        for (int j = 0; j < action.CommandWords.Length; j++)
                        {
                            if (action.CommandWords[j].ToLower() == this.textBoxMsg.Text.ToLower())
                            {
                                this.gpgTextListCommands.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    this.gpgTextListCommands.Visible = this.gpgTextListCommands.TextLines.Length > 0;
                }
                else
                {
                    this.gpgTextListCommands.Visible = false;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void UpdateFriends(string msg)
        {
            if (msg != null)
            {
                this.SystemMessage(msg, new object[0]);
            }
            this.FrmMain_Friends();
            this.pnlUserListChat.RefreshData();
            this.pnlUserListFriends.RefreshData();
        }

        private void UpdateOnlineStatus(bool online)
        {
            if (User.Current.IsAdmin && (User.Current.Online != online))
            {
                User.Current.Online = online;
                User.Current.Visible = online;
                Messaging.SendCustomCommand(CustomCommands.AM1, new object[] { online });
                if (User.Current.Online)
                {
                    this.ChangeStatus("<LOC>Online", StatusIcons.online);
                    this.NotifyOnLogin();
                }
                else
                {
                    this.ChangeStatus("<LOC>Offline", StatusIcons.offline);
                    this.NotifyOnLogout();
                }
            }
        }

        private void UpdateSupCom()
        {
            VGen0 gen = null;
            VGen0 gen2 = null;
            ThreadStart start = null;
            ThreadStart start2 = null;
            if (GameInformation.SelectedGame.GameID == -1)
            {
                this.SetStatus("", new object[0]);
            }
            else if (GameInformation.SelectedGame.CurrentVersion == "NOCHECK")
            {
                this.SetStatus("This game is not being versioned.", 0x2710, new object[0]);
                Thread.Sleep(0xbb8);
                try
                {
                    if (gen == null)
                    {
                        gen = delegate {
                            this.btnFeedback.Enabled = true;
                            this.btnHostGame.Enabled = true;
                            this.btnJoinGame.Enabled = true;
                            this.EnableGameSelectButtons();
                            this.miGameGroup.Enabled = true;
                        };
                    }
                    base.BeginInvoke(gen);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
            else if (!this.IsSupComPatching && (this.SupComPatchThread == null))
            {
                this.mIsSupComPatching = true;
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (gen2 == null)
                    {
                        gen2 = delegate {
                            this.miGameGroup.Enabled = false;
                            this.btnHostGame.Enabled = false;
                            this.btnJoinGame.Enabled = false;
                            this.btnRankedGame.Enabled = false;
                            this.btnArrangedTeam.Enabled = false;
                            this.btnPlayNow.Enabled = false;
                            this.DisableGameSelectButtons();
                        };
                    }
                    base.BeginInvoke(gen2);
                }
                else
                {
                    this.miGameGroup.Enabled = false;
                    this.btnHostGame.Enabled = false;
                    this.btnJoinGame.Enabled = false;
                    this.btnRankedGame.Enabled = false;
                    this.btnArrangedTeam.Enabled = false;
                    this.btnPlayNow.Enabled = false;
                    this.DisableGameSelectButtons();
                }
                if (this.IsRemoteVersion)
                {
                    if (this.LocalGameVersion != this.ServerGameVersion)
                    {
                        if (start == null)
                        {
                            start = delegate {
                                WebDownloader downloader = new WebDownloader();
                                downloader.OnDownloadCompleted += new AsyncCompletedEventHandler(this.SmartPatchComplete);
                                string weburl = GameInformation.SelectedGame.PatcherAppURL;
                                downloader.BeginDownloadFile(weburl, this.GetLocalPatchFile(), true);
                            };
                        }
                        this.SupComPatchThread = new Thread(start);
                    }
                    else
                    {
                        this.SetPatchButtonsCompleted();
                        this.SetStatus("", new object[0]);
                    }
                }
                else
                {
                    if (start2 == null)
                    {
                        start2 = delegate {
                            string str;
                            EventHandler handler = null;
                            OGen0 gen3 = null;
                            OGen0 gen4 = null;
                            VGen0 method = null;
                            VGen0 gen6 = null;
                            bool failed = false;
                            try
                            {
                                this.SetStatus("<LOC>Checking for {0} updates...", new object[] { GameInformation.SelectedGame.GameDescription });
                                if (handler == null)
                                {
                                    handler = delegate (object s, EventArgs e) {
                                        this.SupComPatchThread = null;
                                    };
                                }
                                base.Disposed += handler;
                                Process[] processesByName = Process.GetProcessesByName("patch");
                                if (processesByName.Length > 0)
                                {
                                    this.SetStatus("<LOC>Patching {0}...", new object[] { GameInformation.SelectedGame.GameDescription });
                                    processesByName[0].WaitForExit();
                                }
                                if (((GameInformation.SelectedGame.GameLocation == null) || (GameInformation.SelectedGame.GameLocation.Length <= 0)) || !System.IO.File.Exists(GameInformation.SelectedGame.GameLocation))
                                {
                                }
                                if (((gen3 == null) && !((bool) base.Invoke(gen3 = delegate {
                                    return this.LocateExe("SupremeCommander", false);
                                }))) || !this.IsSupComVersionEnforced)
                                {
                                    return;
                                }
                                int num = 0;
                                bool flag = false;
                                int count = -1;
                                int num3 = 0;
                                int num4 = 0;
                                bool flag2 = this.LocalGameVersion != this.ServerGameVersion;
                                try
                                {
                                    if (flag2 && (this.ServerGameVersion.ToUpper() == "MULTI"))
                                    {
                                        DataList queryDataSafe = DataAccess.GetQueryDataSafe("Get Multi Version", new object[] { GameInformation.SelectedGame.GameID });
                                        foreach (DataRecord record in queryDataSafe)
                                        {
                                            if (record["build_chksum"] == this.LocalGameVersion)
                                            {
                                                flag2 = false;
                                                this.mServerGameVersion = record["build_chksum"];
                                                goto Label_0E15;
                                            }
                                        }
                                    }
                                }
                                catch (Exception exception1)
                                {
                                    ErrorLog.WriteLine(exception1);
                                }
                            Label_0E15:
                                while (flag2 || ((count < 0) || (num3 < count)))
                                {
                                    try
                                    {
                                        if (num4 > 4)
                                        {
                                            this.SetStatus("<LOC>An error orrured while trying to patch, please try again later.", new object[0]);
                                            Thread.Sleep(0xfa0);
                                            return;
                                        }
                                        if (num >= 2)
                                        {
                                            this.SetStatus("<LOC>Download failed, aborting update...", new object[0]);
                                            Thread.Sleep(0x7d0);
                                            failed = true;
                                            return;
                                        }
                                        DataList list2 = DataAccess.GetQueryDataSafe("GetVersionPatchURI", new object[] { this.LocalGameVersion });
                                        count = list2.Count;
                                        if (((list2 == null) || (list2.Count < 1)) || (num3 >= list2.Count))
                                        {
                                            if (!this.IsGameCurrent)
                                            {
                                                this.SetStatus("<LOC>{0} appears to be corrupt and/or modified. Please re-install {0}.", new object[] { GameInformation.SelectedGame.GameDescription });
                                                Thread.Sleep(0xfa0);
                                            }
                                            return;
                                        }
                                        foreach (DataRecord record2 in list2)
                                        {
                                            IOException exception2;
                                            VGen0 curgen = null;
                                            VGen0 curgen2 = null;
                                            string uri;
                                            bool flag3 = false;
                                            bool flag4 = false;
                                            flag3 = (record2.InnerHash.ContainsKey("patch_uri") && (record2["patch_uri"] != null)) && (record2["patch_uri"] != "(null)");
                                            flag4 = (record2.InnerHash.ContainsKey("file_uri") && (record2["file_uri"] != null)) && (record2["file_uri"] != "(null)");
                                            string str2 = null;
                                            if (flag3)
                                            {
                                                uri = record2["patch_uri"];
                                                str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\" + GameInformation.SelectedGame.GameDescription + @"\";
                                                if (Directory.Exists(str))
                                                {
                                                    str = str + "patch.exe";
                                                }
                                                else
                                                {
                                                    str = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation) + @"\patch.exe";
                                                }
                                                if ((record2.InnerHash.ContainsKey("patch_check") && (record2["patch_check"] != null)) && (record2["patch_check"] != "(null)"))
                                                {
                                                    str2 = record2["patch_check"];
                                                }
                                            }
                                            else if (flag4)
                                            {
                                                uri = record2["file_uri"];
                                                str = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation) + @"\" + record2["file_path"];
                                                if ((record2.InnerHash.ContainsKey("file_check") && (record2["file_check"] != null)) && (record2["file_check"] != "(null)"))
                                                {
                                                    str2 = record2["file_check"];
                                                }
                                            }
                                            else
                                            {
                                                ErrorLog.WriteLine("Invalid patch record for {0} found.", new object[] { GameInformation.SelectedGame.GameDescription });
                                                num3++;
                                                continue;
                                            }
                                            WebClient dlClient = null;
                                            bool flag5 = false;
                                            bool flag6 = true;
                                            if (System.IO.File.Exists(str) && (str2 != null))
                                            {
                                                if (flag3)
                                                {
                                                    flag6 = this.CheckSum(str) != str2;
                                                }
                                                else
                                                {
                                                    flag6 = this.CheckSum(str) != str2;
                                                }
                                            }
                                            else if (System.IO.File.Exists(str) && (str2 == null))
                                            {
                                                flag6 = false;
                                            }
                                            else if (!(System.IO.File.Exists(str) || (str2 == null)))
                                            {
                                                flag6 = false;
                                            }
                                            else
                                            {
                                                flag6 = true;
                                                if (flag3)
                                                {
                                                    try
                                                    {
                                                        System.IO.File.Delete(str);
                                                    }
                                                    catch (Exception exception5)
                                                    {
                                                        ErrorLog.WriteLine(exception5);
                                                        this.SetStatus("<LOC>Invalid patch file exists and cannot be deleted. Please restart program.", new object[0]);
                                                        Thread.Sleep(0xbb8);
                                                        return;
                                                    }
                                                }
                                            }
                                            DlgDownloadProgress dlg = null;
                                            if (flag6)
                                            {
                                                int lastResponse = Environment.TickCount;
                                                bool finished = false;
                                                dlClient = new WebClient();
                                                bool downloading = true;
                                                if (curgen == null)
                                                {
                                                    curgen = delegate
                                                    {
                                                        dlg = new DlgDownloadProgress(uri, dlClient);
                                                    };
                                                }
                                                base.Invoke(curgen);
                                                dlClient.DownloadProgressChanged += delegate (object s, System.Net.DownloadProgressChangedEventArgs e) {
                                                    if ((e.ProgressPercentage == 100) && (e.BytesReceived == e.TotalBytesToReceive))
                                                    {
                                                        finished = true;
                                                    }
                                                    lastResponse = Environment.TickCount;
                                                    this.SetStatus("<LOC>Downloading {1} update: {0}%", new object[] { e.ProgressPercentage, GameInformation.SelectedGame.GameDescription });
                                                };
                                                dlClient.DownloadFileCompleted += delegate (object s, AsyncCompletedEventArgs e) {
                                                    finished = true;
                                                    downloading = false;
                                                    lastResponse = Environment.TickCount;
                                                };
                                                if (flag3)
                                                {
                                                    try
                                                    {
                                                        if (System.IO.File.Exists(str))
                                                        {
                                                            System.IO.File.Delete(str);
                                                        }
                                                    }
                                                    catch (IOException exception6)
                                                    {
                                                        exception2 = exception6;
                                                        ErrorLog.WriteLine(exception2);
                                                    }
                                                }
                                                this.SetStatus("<LOC>Downloading {0} update...", new object[] { GameInformation.SelectedGame.GameDescription });
                                                if (curgen2 == null)
                                                {
                                                    curgen2 = delegate
                                                    {
                                                        dlg.Show();
                                                        dlg.BringToFront();
                                                    };
                                                }
                                                base.BeginInvoke(curgen2);
                                                if (!flag)
                                                {
                                                    this.ShowPatchMessage();
                                                    flag = true;
                                                }
                                                if (flag3)
                                                {
                                                    dlClient.DownloadFileAsync(new Uri(uri), str);
                                                }
                                                else
                                                {
                                                    dlClient.DownloadFileAsync(new Uri(uri), str + ".exe");
                                                }
                                                lastResponse = Environment.TickCount;
                                                while (downloading)
                                                {
                                                    if (!(finished || ((Environment.TickCount - lastResponse) <= 0x7530)))
                                                    {
                                                        flag5 = true;
                                                        if (dlClient != null)
                                                        {
                                                            dlClient.CancelAsync();
                                                            dlClient.Dispose();
                                                            dlClient = null;
                                                        }
                                                        break;
                                                    }
                                                    Thread.Sleep(100);
                                                }
                                            }
                                            else
                                            {
                                                num3++;
                                                continue;
                                            }
                                            base.BeginInvoke((VGen0)delegate {
                                                if ((((dlg != null) && !dlg.Disposing) && !dlg.IsDisposed) && dlg.Visible)
                                                {
                                                    try
                                                    {
                                                        dlg.Close();
                                                        dlg = null;
                                                    }
                                                    catch (Exception exception)
                                                    {
                                                        ErrorLog.WriteLine(exception);
                                                    }
                                                }
                                            });
                                            if (flag5 && (num >= 2))
                                            {
                                                this.SetStatus("<LOC>Download failed, aborting update...", new object[0]);
                                                Thread.Sleep(0x7d0);
                                                return;
                                            }
                                            if (flag5)
                                            {
                                                this.SetStatus("<LOC>Download timed out, retrying...", new object[0]);
                                                if (dlClient != null)
                                                {
                                                    dlClient.CancelAsync();
                                                    dlClient.Dispose();
                                                    dlClient = null;
                                                }
                                                num++;
                                                if (flag3)
                                                {
                                                    try
                                                    {
                                                        System.IO.File.Delete(str);
                                                    }
                                                    catch (Exception exception7)
                                                    {
                                                        ErrorLog.WriteLine(exception7);
                                                    }
                                                }
                                                Thread.Sleep(0x7d0);
                                            }
                                            else
                                            {
                                                bool flag7 = false;
                                                num = 0;
                                                if (flag3 || flag4)
                                                {
                                                    while (!flag7 && (num < 2))
                                                    {
                                                        this.SetStatus("<LOC>Patching {0}...", new object[] { GameInformation.SelectedGame.GameDescription });
                                                        bool flag8 = false;
                                                        string[] strArray = new string[] { "SupremeCommander", "SupremeCommanderR", "SupremeCommanderD" };
                                                        for (int j = 0; j < strArray.Length; j++)
                                                        {
                                                            Process[] processArray2 = Process.GetProcessesByName(strArray[j]);
                                                            if (processArray2.Length > 0)
                                                            {
                                                                if (!flag8)
                                                                {
                                                                }
                                                                if ((gen4 != null) || (((DialogResult) base.Invoke(gen4 = delegate {
                                                                    return new DlgYesNo(this, GameInformation.SelectedGame.GameDescription, "<LOC>" + GameInformation.SelectedGame.GameDescription + " is currently running and must be closed for patching to continue. OK to close " + GameInformation.SelectedGame.GameDescription + "?").ShowDialog(this);
                                                                })) == DialogResult.Yes))
                                                                {
                                                                    flag8 = true;
                                                                    foreach (Process process in processArray2)
                                                                    {
                                                                        process.Kill();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    this.SetStatus("<LOC>" + GameInformation.SelectedGame.GameDescription + " failed to patch!", new object[0]);
                                                                    Thread.Sleep(0x7d0);
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                        if (flag4)
                                                        {
                                                            str = str + ".exe";
                                                        }
                                                        ProcessStartInfo startInfo = new ProcessStartInfo(str);
                                                        if (flag3)
                                                        {
                                                            startInfo.WorkingDirectory = new FileInfo(str).DirectoryName;
                                                        }
                                                        Process process2 = Process.Start(startInfo);
                                                        if (method == null)
                                                        {
                                                            method = delegate {
                                                                base.Show();
                                                                base.BringToFront();
                                                            };
                                                        }
                                                        base.Invoke(method);
                                                        process2.WaitForExit();
                                                        GameInformation.CheckMissingPaths();
                                                        int exitCode = process2.ExitCode;
                                                        if (exitCode == 0)
                                                        {
                                                            ErrorLog.WriteLine("Patching of SupCom succeeded via code {0}", new object[] { exitCode });
                                                            try
                                                            {
                                                                if (System.IO.File.Exists(str))
                                                                {
                                                                    System.IO.File.Delete(str);
                                                                }
                                                            }
                                                            catch (IOException exception8)
                                                            {
                                                                exception2 = exception8;
                                                                ErrorLog.WriteLine(exception2);
                                                            }
                                                            flag7 = true;
                                                            this.SetStatus("<LOC>" + GameInformation.SelectedGame.GameDescription + " patching complete.", new object[0]);
                                                            Thread.Sleep(0x7d0);
                                                        }
                                                        else
                                                        {
                                                            ErrorLog.WriteLine("Patching of SupCom failed via error code {0}", new object[] { exitCode });
                                                            num++;
                                                            if (num > 1)
                                                            {
                                                                this.SetStatus("<LOC>" + GameInformation.SelectedGame.GameDescription + " failed to patch!", new object[0]);
                                                                try
                                                                {
                                                                    if (System.IO.File.Exists(str))
                                                                    {
                                                                        System.IO.File.Delete(str);
                                                                    }
                                                                }
                                                                catch (IOException exception9)
                                                                {
                                                                    exception2 = exception9;
                                                                    ErrorLog.WriteLine(exception2);
                                                                }
                                                                Thread.Sleep(0x7d0);
                                                                return;
                                                            }
                                                            this.SetStatus("<LOC>Patch failed, retrying...", new object[0]);
                                                            Thread.Sleep(0x7d0);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        if (System.IO.File.Exists(str))
                                                        {
                                                            System.IO.File.Replace(str + ".dl", str, str + ".back");
                                                        }
                                                        else
                                                        {
                                                            System.IO.File.Move(str + ".dl", str);
                                                        }
                                                    }
                                                    catch (Exception exception4)
                                                    {
                                                        ErrorLog.WriteLine(exception4);
                                                        this.SetStatus("<LOC>Error updating file: {0}, ensure it is not in use or read-only.", new object[] { Path.GetFileName(str) });
                                                        Thread.Sleep(0xfa0);
                                                        if (System.IO.File.Exists(str + ".back"))
                                                        {
                                                            System.IO.File.Move(str + ".back", str);
                                                        }
                                                    }
                                                    finally
                                                    {
                                                        if (System.IO.File.Exists(str + ".dl"))
                                                        {
                                                            System.IO.File.Delete(str + ".dl");
                                                        }
                                                        if (System.IO.File.Exists(str + ".back"))
                                                        {
                                                            System.IO.File.Delete(str + ".back");
                                                        }
                                                    }
                                                    num3++;
                                                    break;
                                                }
                                            }
                                        }
                                        continue;
                                    }
                                    catch (Exception exception11)
                                    {
                                        ErrorLog.WriteLine(exception11);
                                        num++;
                                        this.SetStatus("<LOC>Patch failed, retrying...", new object[0]);
                                        Thread.Sleep(0x7d0);
                                    }
                                    finally
                                    {
                                        num4++;
                                    }
                                }
                            }
                            catch (Exception exception12)
                            {
                                ErrorLog.WriteLine(exception12);
                            }
                            finally
                            {
                                this.SupComPatchThread = null;
                                GPG.Logging.EventLog.WriteLine("<LOC>Supreme Commander patching complete.", new object[0]);
                                Thread.Sleep(0x7d0);
                                if ((GameInformation.SelectedGame.GameLocation != null) && (GameInformation.SelectedGame.GameLocation.Length > 0))
                                {
                                    str = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation) + @"\patch.exe";
                                    if (System.IO.File.Exists(str))
                                    {
                                        try
                                        {
                                            System.IO.File.Delete(str);
                                        }
                                        catch (Exception exception13)
                                        {
                                            ErrorLog.WriteLine(exception13);
                                        }
                                    }
                                }
                                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                                {
                                    try
                                    {
                                        if (gen6 == null)
                                        {
                                            gen6 = delegate {
                                                if (!failed)
                                                {
                                                    this.btnFeedback.Enabled = true;
                                                    this.btnHostGame.Enabled = true;
                                                    this.btnJoinGame.Enabled = true;
                                                    this.btnRankedGame.Enabled = true;
                                                    this.btnArrangedTeam.Enabled = true;
                                                    this.btnPlayNow.Enabled = true;
                                                    this.btnViewRankings.Enabled = true;
                                                    this.EnableGameSelectButtons();
                                                    this.miGameGroup.Enabled = true;
                                                    this.miRankings.Enabled = true;
                                                    this.btnRankedGame.Image = SkinManager.GetImage("nav-ranked_game.png");
                                                    this.btnRankedGame.ToolTipText = Loc.Get("<LOC>Play Ranked Game");
                                                    this.btnArrangedTeam.Image = SkinManager.GetImage("nav-ranked_team.png");
                                                    this.btnArrangedTeam.ToolTipText = Loc.Get("<LOC>Play Arranged Team Game");
                                                }
                                            };
                                        }
                                        base.BeginInvoke(gen6);
                                    }
                                    catch (Exception exception14)
                                    {
                                        ErrorLog.WriteLine(exception14);
                                    }
                                }
                                this.mIsSupComPatching = false;
                                this.SetStatus("", new object[0]);
                            }
                        };
                    }
                    this.SupComPatchThread = new Thread(start2);
                }
                if (this.SupComPatchThread != null)
                {
                    this.SupComPatchThread.IsBackground = true;
                    this.SupComPatchThread.Start();
                }
            }
        }

        private void UpdateUser(object[] args)
        {
            try
            {
                if (!Chatroom.InChatroom)
                {
                    ErrorLog.WriteLine("Attempt to add user {0} to chat while not in a chatroom", new object[] { args[0] });
                }
                else
                {
                    User user = MappedObject.FromDataString<User>((string) args[0]);
                    if (args.Length > 1)
                    {
                        this.UpdateUser(user, (string) args[1]);
                    }
                    else
                    {
                        this.UpdateUser(user);
                    }
                    if (User.CurrentFriends.ContainsIndex("name", user.Name))
                    {
                        this.pnlUserListFriends.UpdateUser(user);
                        this.pnlUserListFriends.RefreshData();
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void UpdateUser(User user)
        {
            this.UpdateUser(user, null);
        }

        internal void UpdateUser(User user, string status)
        {
            VGen0 target = null;
            try
            {
                user.IsFriend = User.CurrentFriends.ContainsIndex("name", user.Name);
                if (target == null)
                {
                    target = delegate {
                        this.pnlUserListChat.UpdateUser(user);
                    };
                }
                this.ChatroomQueue.Enqueue(target, new object[0]);
                int index = Chatroom.GatheringParticipants.IndexOf(user);
                if (index >= 0)
                {
                    Chatroom.GatheringParticipants.IndexObject(user);
                    Chatroom.GatheringParticipants[index] = user;
                }
                else
                {
                    Chatroom.GatheringParticipants.IndexObject(user);
                    Chatroom.GatheringParticipants.Add(user);
                }
                foreach (FrmPrivateChat chat in this.PrivateChats.Values)
                {
                    if ((!chat.Disposing && !chat.IsDisposed) && chat.ChatTarget.Equals(user))
                    {
                        chat.ChatTarget = user;
                        chat.RefreshToolstrip();
                    }
                }
                if (status != null)
                {
                    this.SetUserStatus(user.Name, status);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public string UrlById(string name)
        {
            if (this.mNotFound == "")
            {
                this.mNotFound = Application.StartupPath + @"\notfound.html";
                StreamWriter writer = new StreamWriter(this.mNotFound);
                writer.WriteLine("<HTML><BODY bgcolor=black text=white><TABLE border=0 width=100% height=100%><TR><TD align=center valign=center>Loading Page...</TD></TR></TABLE></BODY></HTML>");
                writer.Close();
            }
            if (this.WebURL != null)
            {
                foreach (URLInfo info in this.WebURL)
                {
                    if (info.Id.ToUpper() == name.ToUpper())
                    {
                        return info.Url;
                    }
                }
            }
            return this.mNotFound;
        }

        private void ViewClanInvites()
        {
            UserRequestEventHandler handler = null;
            if (this.ClanInvites.Count > 0)
            {
                DlgClanRequests<ClanInvite> dlg = new DlgClanRequests<ClanInvite>(this, this.ClanInvites);
                dlg.AcceptRequest += delegate (object sender, UserRequestEventArgs e) {
                    if (e.Request is ClanInvite)
                    {
                        ClanInvite request = e.Request as ClanInvite;
                        this.JoinClan(request.ClanID, request.ClanName, request.ClanAbbreviation);
                        this.ClanInvites.Clear();
                        dlg.Close();
                        return true;
                    }
                    this.ErrorMessage("<LOC>Unable to accept invitation at this time.", new object[0]);
                    return false;
                };
                if (handler == null)
                {
                    handler = delegate (object sender, UserRequestEventArgs e) {
                        if (e.Request is ClanInvite)
                        {
                            ClanInvite request = e.Request as ClanInvite;
                            if (DataAccess.ExecuteQuery("RemoveClanInvite", new object[] { request.ClanName }))
                            {
                                Messaging.SendCustomCommand(e.Request.RequestorName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has turned down your clan invitaion.", User.Current.Name });
                                this.SystemMessage("<LOC>A rejection notification has been sent.", new object[0]);
                                return true;
                            }
                            this.ErrorMessage("<LOC>Unable to remove invitation at this time.", new object[0]);
                            return false;
                        }
                        this.ErrorMessage("<LOC>Unable to remove invitation at this time.", new object[0]);
                        return false;
                    };
                }
                dlg.RejectRequest += handler;
                dlg.Show();
            }
        }

        private void ViewClanRequests()
        {
            UserRequestEventHandler handler = null;
            UserRequestEventHandler handler2 = null;
            if ((this.ClanRequests != null) && (this.ClanRequests.Count > 0))
            {
                DlgClanRequests<ClanRequest> requests = new DlgClanRequests<ClanRequest>(this, this.ClanRequests);
                if (handler == null)
                {
                    handler = delegate (object sender, UserRequestEventArgs e) {
                        if (this.InviteToClan(e.Request.RequestorName))
                        {
                            if (!DataAccess.ExecuteQuery("RemoveClanRequest", new object[] { e.Request.RequestorName }))
                            {
                                this.ErrorMessage("<LOC>Unable to remove request at this time.", new object[0]);
                                return false;
                            }
                            return true;
                        }
                        return false;
                    };
                }
                requests.AcceptRequest += handler;
                if (handler2 == null)
                {
                    handler2 = delegate (object sender, UserRequestEventArgs e) {
                        if (DataAccess.ExecuteQuery("RemoveClanRequest", new object[] { e.Request.RequestorName }))
                        {
                            Messaging.SendCustomCommand(e.Request.RequestorName, CustomCommands.SystemEvent, new object[] { "<LOC>{0} has turned down your request for a clan invitaion.", e.Request.RequestorName });
                            this.SystemMessage("<LOC>A rejection notification has been sent.", new object[0]);
                            return true;
                        }
                        this.ErrorMessage("<LOC>Unable to remove request at this time.", new object[0]);
                        return false;
                    };
                }
                requests.RejectRequest += handler2;
                requests.Show();
            }
        }

        private void ViewFriendInvites()
        {
            if (this.FriendRequests.Count > 0)
            {
                new DlgFriendRequests(this, this.FriendRequests).Show();
            }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == 0x7e)
                {
                    if (this.msMainMenu != null)
                    {
                        this.msMainMenu.Refresh();
                    }
                    this.Refresh();
                }
                else if (m.Msg == 0x313)
                {
                    GPGContextMenu menu = new GPGContextMenu();
                    MenuItem item = new MenuItem("<LOC>Restore");
                    item.Click += new EventHandler(this.Restore);
                    item.Enabled = this.mIsMaximized || (base.WindowState == FormWindowState.Minimized);
                    menu.MenuItems.Add(item);
                    item = new MenuItem("<LOC>Maximize");
                    item.Click += new EventHandler(this.Restore);
                    item.Enabled = (this.pbRestore.Visible && !this.mIsMaximized) && (base.WindowState == FormWindowState.Normal);
                    menu.MenuItems.Add(item);
                    item = new MenuItem("<LOC>Minimize");
                    item.Click += new EventHandler(this.Minimize);
                    item.Enabled = base.WindowState != FormWindowState.Minimized;
                    menu.MenuItems.Add(item);
                    menu.MenuItems.Add("-");
                    item = new MenuItem("<LOC>Close");
                    item.Click += new EventHandler(this.CloseClick);
                    menu.MenuItems.Add(item);
                    menu.Localize();
                    menu.Show(this, new Point(Control.MousePosition.X - base.Left, Control.MousePosition.Y - base.Top));
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal List<DlgBase> ActiveDialogs
        {
            get
            {
                return this.mActiveDialogs;
            }
        }

        public DlgBase ActiveWindow
        {
            get
            {
                return this.mActiveWindow;
            }
            set
            {
                this.mActiveWindow = value;
                if (value == null)
                {
                    GPG.Logging.EventLog.WriteLine("Active window set to {0}", new object[] { this });
                }
                else
                {
                    GPG.Logging.EventLog.WriteLine("Active window set to {0}", new object[] { value });
                }
            }
        }

        private string[] AllMessageList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (User user in User.CurrentFriends)
                {
                    if (user.Online)
                    {
                        list.Add(user.Name);
                    }
                }
                foreach (ClanMember member in Clan.CurrentMembers)
                {
                    if (!((!member.Online || member.IsCurrent) || list.Contains(member.Name)))
                    {
                        list.Add(member.Name);
                    }
                }
                return list.ToArray();
            }
        }

        private int AwayInterval
        {
            get
            {
                return (Program.Settings.Chat.AwayTimeout * 0xea60);
            }
        }

        public string BeforeGameChatroom
        {
            get
            {
                if (TournamentCommands.sChatroom != "")
                {
                    return TournamentCommands.sChatroom;
                }
                if ((this.mBeforeGameChatroom == null) || (this.mBeforeGameChatroom.Trim() == ""))
                {
                    return this.MainChatroom;
                }
                return this.mBeforeGameChatroom;
            }
            set
            {
                if ((value != null) && (value.Trim() != ""))
                {
                    this.mBeforeGameChatroom = value;
                }
            }
        }

        private bool CancelLoad
        {
            get
            {
                return (!this.AwaitingLogin && (!this.ContinueLoading.HasValue ? false : !this.ContinueLoading.Value));
            }
        }

        public LinkedList<string> ChatHistory
        {
            get
            {
                return this.mChatHistory;
            }
        }

        private string[] ClanMessageList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (ClanMember member in Clan.CurrentMembers)
                {
                    if (!(!member.Online || member.IsCurrent))
                    {
                        list.Add(member.Name);
                    }
                }
                return list.ToArray();
            }
        }

        public bool Connected
        {
            get
            {
                return this.mConnected;
            }
        }

        public IGameLobby CurrentGameLobby
        {
            get
            {
                return this.mCurrentGameLobby;
            }
            internal set
            {
                this.mCurrentGameLobby = value;
                if (value != null)
                {
                    this.DisableGameButtons();
                }
            }
        }

        private string[] FriendsMessageList
        {
            get
            {
                List<string> list = new List<string>();
                foreach (User user in User.CurrentFriends)
                {
                    if (user.Online)
                    {
                        list.Add(user.Name);
                    }
                }
                return list.ToArray();
            }
        }

        public IGatheringDisplay GatheringDisplaycontrol
        {
            get
            {
                return this.skinGatheringDisplayChat;
            }
        }

        private string HOME_URL
        {
            get
            {
                return this.UrlById("HOME");
            }
        }

        public bool IsActiveWindow
        {
            get
            {
                return this.mIsActiveWindow;
            }
        }

        public bool IsGameCurrent
        {
            get
            {
                if (!this.IsSupComVersionEnforced)
                {
                    return true;
                }
                string localGameVersion = this.LocalGameVersion;
                return ((localGameVersion != null) && (this.ServerGameVersion == localGameVersion));
            }
        }

        public bool IsInGame
        {
            get
            {
                return this.mIsInGame;
            }
        }

        internal bool IsInTeamGame
        {
            get
            {
                return (((this.DlgTeamGame != null) && !this.DlgTeamGame.Disposing) && !this.DlgTeamGame.IsDisposed);
            }
        }

        internal bool IsMoving
        {
            get
            {
                return this.mIsMoving;
            }
        }

        public bool IsRemoteVersion
        {
            get
            {
                return (GameInformation.SelectedGame.PatcherAppURL != "");
            }
        }

        internal bool IsResizing
        {
            get
            {
                return this.mIsResizing;
            }
        }

        public bool IsSupComPatching
        {
            get
            {
                return this.mIsSupComPatching;
            }
        }

        private bool IsSupComVersionEnforced
        {
            get
            {
                if (GameInformation.SelectedGame.CurrentVersion == "NOCHECK")
                {
                    return false;
                }
                if (Program.HasArg("/noenforce"))
                {
                    return false;
                }
                if (!this.mIsSupComVersionEnforced.HasValue)
                {
                    this.mIsSupComVersionEnforced = new bool?(DataAccess.GetBool("IsCurrentVersionEnforced", new object[0]));
                }
                return this.mIsSupComVersionEnforced.Value;
            }
        }

        internal bool IsViewingCustomGames
        {
            get
            {
                return (((this.DlgSelectGame != null) && !this.DlgSelectGame.Disposing) && !this.DlgSelectGame.IsDisposed);
            }
        }

        internal string LocalGameVersion
        {
            get
            {
                try
                {
                    string path = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation) + @"\" + GameInformation.SelectedGame.VersionFile;
                    if (this.IsRemoteVersion)
                    {
                        path = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation) + @"\gpgnetversion.txt";
                        if (path.ToLower().IndexOf("gpgnetversion.txt") >= 0)
                        {
                            if (!System.IO.File.Exists(path))
                            {
                                return "0.0.0.0";
                            }
                            StreamReader reader = new StreamReader(path);
                            string str2 = reader.ReadLine();
                            reader.Close();
                            return str2;
                        }
                    }
                    if (!System.IO.File.Exists(path))
                    {
                        path = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation) + @"\MohoEngine.dll";
                    }
                    if (System.IO.File.Exists(path))
                    {
                        return this.CheckSum(path);
                    }
                    return null;
                }
                catch
                {
                    if (System.IO.File.Exists(GameInformation.SelectedGame.GameLocation))
                    {
                        return this.CheckSum(GameInformation.SelectedGame.GameLocation);
                    }
                    return null;
                }
            }
        }

        internal string MainChatroom
        {
            get
            {
                if (ConfigSettings.GetBool("DoOldGameList", false))
                {
                    return ConfigSettings.GetString("MainChatroom", "Main Chat");
                }
                if (this.mMainChatrom == "")
                {
                    this.mMainChatrom = DataAccess.GetString("GetMainChatRoom", new object[] { GameInformation.SelectedGame.GameID });
                    GameInformation.OnSelectedGameChange += new EventHandler(this.CheckChatroomName);
                }
                if ((this.mMainChatrom == "") || (this.mMainChatrom == null))
                {
                    this.mMainChatrom = "Main Chat";
                }
                return this.mMainChatrom;
            }
        }

        public Dictionary<string, FrmPrivateChat> PrivateChats
        {
            get
            {
                return this.mPrivateChats;
            }
        }

        internal bool RanInitializeChat
        {
            get
            {
                return this.mRanInitializeChat;
            }
        }

        internal bool SearchingForAutomatch
        {
            get
            {
                return this.mSearchingForAutomatch;
            }
        }

        public IUser SelectedChatParticipant
        {
            get
            {
                try
                {
                    return this.mSelectedChatParticipant;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return null;
                }
            }
        }

        public User SelectedChatTextSender
        {
            get
            {
                try
                {
                    int[] selectedRows = this.gvChat.GetSelectedRows();
                    if (selectedRows.Length > 0)
                    {
                        TextLine row = this.gvChat.GetRow(selectedRows[0]) as TextLine;
                        if ((row != null) && ((row.Tag != null) && (row.Tag is User)))
                        {
                            return (row.Tag as User);
                        }
                    }
                    return null;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return null;
                }
            }
        }

        public GridView SelectedParticipantView
        {
            get
            {
                return this.mSelectedParticipantView;
            }
        }

        public SkinButton SelectedTab
        {
            get
            {
                if (this.mSelectedTab == null)
                {
                    this.mSelectedTab = this.btnChatTab;
                }
                return this.mSelectedTab;
            }
        }

        private int SelfLeaveInterval
        {
            get
            {
                return ConfigSettings.GetInt("SelfLeaveTimer", 0xdbba0);
            }
        }

        public string ServerGameVersion
        {
            get
            {
                if ((this.mLastSelectedGame != GameInformation.SelectedGame) || (this.mServerGameVersion == null))
                {
                    if (GameInformation.SelectedGame.GameDescription == "Supreme Commander")
                    {
                        this.mServerGameVersion = DataAccess.GetString("GetSupComVersion", new object[0]);
                    }
                    else
                    {
                        this.mServerGameVersion = DataAccess.GetString("Get Current Game Version", new object[] { GameInformation.SelectedGame.GameID });
                    }
                    this.mLastSelectedGame = GameInformation.SelectedGame;
                }
                return this.mServerGameVersion;
            }
        }

        public bool ShuttingDown
        {
            get
            {
                return this.mShuttingDown;
            }
        }

        public int SpeakingInterval
        {
            get
            {
                return (Program.Settings.Chat.SpeakingTimeout * 0x3e8);
            }
        }

        public Dictionary<string, StateTimer> SpeakingTimers
        {
            get
            {
                return this.mSpeakingTimers;
            }
        }

        private string[] UniqueClanMessageList
        {
            get
            {
                List<string> list = new List<string>();
                List<string> list2 = new List<string>();
                foreach (User user in User.CurrentFriends)
                {
                    if (user.Online)
                    {
                        list.Add(user.Name);
                    }
                }
                foreach (ClanMember member in Clan.CurrentMembers)
                {
                    if (!((!member.Online || member.IsCurrent) || list.Contains(member.Name)))
                    {
                        list2.Add(member.Name);
                    }
                }
                list = null;
                return list2.ToArray();
            }
        }

        private string[] UniqueFriendsMessageList
        {
            get
            {
                List<string> list = new List<string>();
                List<string> list2 = new List<string>();
                foreach (ClanMember member in Clan.CurrentMembers)
                {
                    if (member.Online)
                    {
                        list.Add(member.Name);
                    }
                }
                foreach (User user in User.CurrentFriends)
                {
                    if (!(!user.Online || list.Contains(user.Name)))
                    {
                        list2.Add(user.Name);
                    }
                }
                list = null;
                return list2.ToArray();
            }
        }
    }
}

