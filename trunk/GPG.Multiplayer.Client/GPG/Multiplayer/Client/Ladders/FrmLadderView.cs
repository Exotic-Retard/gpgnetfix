namespace GPG.Multiplayer.Client.Ladders
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.LadderService;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class FrmLadderView : DlgBase
    {
        internal static Dictionary<int, bool> AcceptingChallengesLookup = new Dictionary<int, bool>();
        private SkinLabel backLabelTitle;
        public ToolStripMenuItem btnAuditTrail;
        public ToolStripMenuItem btnAutomatch;
        public ToolStripMenuItem btnChallenge;
        public ToolStripMenuItem btnComments;
        public ToolStripMenuItem btnHelp;
        public ToolStripMenuItem btnJoinLeave;
        public ToolStripMenuItem btnMore;
        public ToolStripMenuItem btnSearchOther;
        public ToolStripMenuItem btnSearchSelf;
        public ToolStripMenuItem btnToggleChallengers;
        public ToolStripMenuItem btnToggleMyChallenges;
        private IContainer components;
        private GridColumn gcConflicts;
        private GridColumn gcCurrentStreak;
        private GridColumn gcDaysSinceChallenge;
        private GridColumn gcDaysUntilDegrade;
        private GridColumn gcDraws;
        private GridColumn gcLosses;
        private GridColumn gcName;
        private GridColumn gcNonReports;
        private GridColumn gcPrevRank;
        private GridColumn gcRank;
        private GridColumn gcRecordStreak;
        private GridColumn gcRepRater;
        private GridColumn gcRepRating;
        private GridColumn gcWinPercentage;
        private GridColumn gcWins;
        private GPGDataGrid gpgDataGridLadder;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabelDates;
        private GPGLabel gpgLabelDescription;
        private GPGLabel gpgLabelRules;
        private GPGLabel gpgLabelType;
        private GPGPanel gpgPanelLinkPages;
        private GridView gvLadder;
        private bool IsJoinPending;
        internal bool IsLadderParticipant;
        private bool IsLeavePending;
        private bool IsViewingChallengers;
        private MappedObjectList<LadderParticipant> mCurrentDataSet;
        private int mCurrentPage;
        private List<ToolStripItem> mCustomPaint;
        private LadderInstance mLadder;
        private int mLastRating;
        private LadderParticipant mSelectedParticipant;
        private GPGMenuStrip msQuickButtons;
        private const int PAGE_LINK_SIZE = 60;
        internal static readonly int PAGE_SIZE = ConfigSettings.GetInt("LadderPageSize", 50);
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private RepositoryItemPictureEdit repositoryItemPictureEdit2;
        private SkinButton skinButtonEnd;
        private SkinButton skinButtonLast;
        private SkinButton skinButtonNext;
        private SkinButton skinButtonStart;
        private SkinGroupPanel skinGroupPanel1;
        private bool ToolstripSizeChanged;

        static FrmLadderView()
        {
            LadderDefinition.DefaultParamsProvider = typeof(SupcomParamsProvider);
        }

        public FrmLadderView(LadderInstance ladder)
        {
            EventHandler handler = null;
            VGen0 method = null;
            this.components = null;
            this.mLadder = null;
            this.IsLadderParticipant = false;
            this.IsJoinPending = false;
            this.IsLeavePending = false;
            this.IsViewingChallengers = false;
            this.mSelectedParticipant = null;
            this.mCurrentPage = 0;
            this.mLastRating = -1;
            this.mCurrentDataSet = null;
            this.ToolstripSizeChanged = false;
            this.mCustomPaint = new List<ToolStripItem>();
            this.mLadder = ladder;
            this.InitializeComponent();
            base.MainForm.LadderGamePlayed += new EventHandler(this.MainForm_LadderGamePlayed);
            this.gpgLabelDates.Text = string.Format("{0} - {1}", this.Ladder.StartDate.ToShortDateString(), this.Ladder.EndDate.ToShortDateString());
            this.backLabelTitle.Text = this.Ladder.Description;
            this.gpgLabelRules.Text = this.Ladder.LadderDefinition.RulesDescription;
            this.gpgLabelDescription.Text = string.Format("{0}. {1}", this.Ladder.LadderDefinition.Name, this.Ladder.LadderDefinition.Description);
            this.Text = string.Format("<LOC>Ladder View - {0}", this.Ladder.LadderDefinition.Name);
            string data = "<LOC>Individual Ladder";
            if (this.Ladder.LadderDefinition.IsTeam)
            {
                data = "<LOC>Team Ladder";
            }
            else if (this.Ladder.LadderDefinition.IsClan)
            {
                data = "<LOC>Clan Ladder";
            }
            if (!this.AcceptingChallenges)
            {
                this.btnToggleMyChallenges.Image = SkinManager.GetImage(@"Dialog\LadderView\decline_challenges.png");
                this.btnToggleMyChallenges.ToolTipText = Loc.Get("<LOC>Declining Challenges");
            }
            else
            {
                this.btnToggleMyChallenges.Image = SkinManager.GetImage(@"Dialog\LadderView\accept_challenges.png");
                this.btnToggleMyChallenges.ToolTipText = Loc.Get("<LOC>Accepting Challenges");
            }
            this.gpgLabelType.Text = Loc.Get(data);
            if (handler == null)
            {
                handler = delegate (object o, EventArgs e) {
                    this.backLabelTitle.Refresh();
                    this.ResizePageLinks();
                };
            }
            base.SizeChanged += handler;
            this.IsLadderParticipant = new QuazalQuery("IsLadderParticipant", new object[] { User.Current.ID, this.Ladder.ID }).GetBool();
            this.IsJoinPending = new QuazalQuery("IsLadderJoinPending", new object[] { User.Current.ID, this.Ladder.ID }).GetBool();
            this.IsLeavePending = new QuazalQuery("IsLadderRemovePending", new object[] { User.Current.ID, this.Ladder.ID }).GetBool();
            if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
            {
                if (method == null)
                {
                    method = delegate {
                        if (this.IsJoinPending)
                        {
                            this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Join Pending");
                        }
                        else if (this.IsLeavePending)
                        {
                            this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Leave Pending");
                        }
                        else if (this.IsLadderParticipant)
                        {
                            this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Leave This Ladder");
                            this.btnJoinLeave.Image = this.btnJoinLeave.Image;
                        }
                        else
                        {
                            this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Join This Ladder");
                            this.btnJoinLeave.Image = this.btnJoinLeave.Image;
                        }
                        this.RefreshToolstrip();
                    };
                }
                base.BeginInvoke(method);
            }
            this.RefreshDataSynchronous();
        }

        private void _ChangePage(int page, int selectRow)
        {
            if (((page != this.CurrentPage) && (page >= 0)) && (page <= this.LastPage))
            {
                this.CurrentPage = page;
                this._RefreshData(selectRow);
            }
        }

        private void _OnRefreshData(DataList data, int selectRow)
        {
            VGen0 method = null;
            if (((base.InvokeRequired && !base.Disposing) && !base.IsDisposed) && base.IsHandleCreated)
            {
                if (method == null)
                {
                    method = delegate {
                        this._OnRefreshData(data, selectRow);
                    };
                }
                base.BeginInvoke(method);
            }
            else if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
            {
                if ((data != null) && (data.Count > 0))
                {
                    this.mCurrentDataSet = new MappedObjectList<LadderParticipant>(data);
                    this.gpgDataGridLadder.DataSource = null;
                    this.gpgDataGridLadder.DataSource = this.CurrentDataSet;
                    this.gvLadder.RefreshData();
                }
                else
                {
                    this.gpgDataGridLadder.DataSource = null;
                    this.gvLadder.RefreshData();
                }
                if (this.gvLadder.IsValidRowHandle(selectRow))
                {
                    this.gvLadder.MakeRowVisible(selectRow, true);
                    this.gvLadder.FocusedRowHandle = selectRow;
                }
                else
                {
                    this.gvLadder_FocusedRowChanged(null, null);
                }
                this.RefreshToolstrip();
                this.ResizePageLinks();
            }
        }

        private void _RefreshData(int selectRow)
        {
            WaitCallback callBack = null;
            WaitCallback callback2 = null;
            int start = this.CurrentPage * PAGE_SIZE;
            int end = (this.CurrentPage + 1) * PAGE_SIZE;
            if (this.IsViewingChallengers)
            {
                if (callBack == null)
                {
                    callBack = delegate (object state) {
                        DataList data = DataAccess.GetQueryData("GetLadderChallengers", new object[] { this.Ladder.ID, start, end });
                        this._OnRefreshData(data, selectRow);
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
            }
            else
            {
                if (callback2 == null)
                {
                    callback2 = delegate (object state) {
                        DataList queryData = DataAccess.GetQueryData("GetLadderParticipants", new object[] { this.Ladder.ID, start, end });
                        this._OnRefreshData(queryData, selectRow);
                    };
                }
                ThreadQueue.QueueUserWorkItem(callback2, new object[0]);
            }
        }

        private void btnAuditTrail_Click(object sender, EventArgs e)
        {
            if (this.SelectedParticipant != null)
            {
                new DlgLadderHistory(this.SelectedParticipant, false).Show();
            }
        }

        private void btnAutomatch_Click(object sender, EventArgs e)
        {
            GameParamsProvider provider = this.Ladder.LadderDefinition.CreateParamsProvider();
            string map = provider.GetMap();
            if (map != null)
            {
                string faction = provider.GetFaction();
                if ((map != null) && (faction != null))
                {
                    SupcomAutomatch supcomAutomatch = SupcomAutomatch.GetSupcomAutomatch();
                    supcomAutomatch.MapName = map;
                    supcomAutomatch.Faction = faction;
                    supcomAutomatch.RegisterLadderGame(this.Ladder, true);
                    base.MainForm.PlayRankedGame(true, this.Ladder.AutomatchType, null, null);
                }
            }
        }

        private void btnChallenge_Click(object sender, EventArgs e)
        {
            if (this.SelectedParticipant != null)
            {
                if (base.MainForm.CanLadderChallengeUp(this.Ladder.ID) || (base.MainForm.GetLadderParticipantRank(this.Ladder.ID, User.Current.ID) < this.SelectedParticipant.Rank))
                {
                    if (this.Ladder.LadderDefinition.IsIndiviual && (this.SelectedParticipant.EntityID == User.Current.ID))
                    {
                        DlgMessage.ShowDialog("<LOC>You cannot challenge yourself.");
                    }
                    else
                    {
                        Messaging.SendCustomCommand(this.SelectedParticipant.EntityName, CustomCommands.LadderChallengeRequest, new object[] { this.Ladder.ID });
                        base.SetStatus("<LOC>A request has been sent.", 0x7d0, new object[0]);
                    }
                }
                else
                {
                    DlgMessage.ShowDialog("<LOC>You cannot challenge upward now. You must automatch or challenge downward for 50% of your games to be able to challenge upward on ths ladder.");
                }
            }
        }

        private void btnComments_Click(object sender, EventArgs e)
        {
            if (this.SelectedParticipant != null)
            {
                new DlgLadderHistory(this.SelectedParticipant, true).Show();
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowDlgSolution(ConfigSettings.GetInt("LadderHelpSolution", 0x50b));
        }

        private void btnJoinLeave_Click(object sender, EventArgs e)
        {
            if (this.IsLadderParticipant)
            {
                if (new DlgYesNo(base.MainForm, "<LOC>Confirmation", "<LOC>This action is permanent and your record cannot be recovered at a later time. Do you really want to leave this ladder?").ShowDialog() == DialogResult.Yes)
                {
                    if (new QuazalQuery("LeaveLadder", new object[] { User.Current.ID, this.Ladder.ID }).ExecuteNonQuery())
                    {
                        DlgMessage.ShowDialog("<LOC>Your leave request has been recieved and will be processed shortly. Check back periodically to see your leave status with this ladder.", "<LOC>Success");
                        this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Leave Pending");
                        this.IsLadderParticipant = false;
                        this.IsLeavePending = true;
                        this.RefreshToolstrip();
                    }
                    else
                    {
                        DlgMessage.ShowDialog("<LOC>An error occured while attempting to leave this ladder, please try again later.", "<LOC>Error");
                    }
                }
            }
            else if (!new QuazalQuery("IsPlayerRanked", new object[0]).GetBool())
            {
                DlgMessage.ShowDialog("<LOC>You must play at least 1 ranked game to participate in ladders.");
            }
            else if (!(((this.Ladder.LadderDefinition.EntryCriteriaQuery == null) || (this.Ladder.LadderDefinition.EntryCriteriaQuery.Length <= 0)) || new QuazalQuery(this.Ladder.LadderDefinition.EntryCriteriaQuery, new object[] { User.Current.ID }).GetBool()))
            {
                DlgMessage.ShowDialog("<LOC>You do not meet the entry criteria for this ladder. For a detailed explanation of this criteria, click the link at the top-right of this form.");
            }
            else
            {
                string name = null;
                DialogResult oK = DialogResult.OK;
                if (this.Ladder.LadderDefinition.IsIndiviual)
                {
                    name = User.Current.Name;
                }
                else if (this.Ladder.LadderDefinition.IsClan)
                {
                    name = Clan.Current.Name;
                }
                else
                {
                    name = DlgAskQuestion.AskQuestion(base.MainForm, "<LOC>Please enter a name for this team.", "<LOC>Name", false, out oK);
                }
                if (oK == DialogResult.OK)
                {
                    if (new QuazalQuery("JoinLadderQueue", new object[] { User.Current.ID, name, this.Ladder.ID }).ExecuteNonQuery())
                    {
                        DlgMessage.ShowDialog("<LOC>Your join request has been accepted and will be processed shortly. Check back periodically to see your join status with this ladder.", "<LOC>Success");
                        this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Join Pending");
                        this.IsLeavePending = true;
                        this.RefreshToolstrip();
                    }
                    else
                    {
                        DlgMessage.ShowDialog("<LOC>An error occured while attempting to join this ladder, please try again later.", "<LOC>Error");
                    }
                }
            }
        }

        private void btnSearchOther_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string name = DlgAskQuestion.AskQuestion(base.MainForm, "<LOC>Enter a participants name to search for", "<LOC>Search", false, out result);
            if (result == DialogResult.OK)
            {
                this.JumpTo(name);
            }
        }

        private void btnSearchSelf_Click(object sender, EventArgs e)
        {
            this.JumpTo(User.Current.Name);
        }

        private void btnToggleChallengers_Click(object sender, EventArgs e)
        {
            this.IsViewingChallengers = !this.IsViewingChallengers;
            if (this.IsViewingChallengers)
            {
                this.btnToggleChallengers.Image = SkinManager.GetImage(@"Dialog\LadderView\show_challengers.png");
                this.btnToggleChallengers.ToolTipText = Loc.Get("<LOC>Showing only participants who are accepting challenges");
            }
            else
            {
                this.btnToggleChallengers.Image = SkinManager.GetImage(@"Dialog\LadderView\show_all.png");
                this.btnToggleChallengers.ToolTipText = Loc.Get("<LOC>Showing all participants");
            }
            this.RefreshData();
        }

        private void btnToggleMyChallenges_Click(object sender, EventArgs e)
        {
            this.AcceptingChallenges = !this.AcceptingChallenges;
            this.RefreshChallengeToggle();
            ThreadQueue.QueueUserWorkItem(delegate (object state) {
                DataAccess.ExecuteQuery("ToggleChallengeStatus", new object[] { this.AcceptingChallenges, User.Current.ID, this.Ladder.ID });
            }, new object[0]);
        }

        private void ChangePage(int page)
        {
            if (((page != this.CurrentPage) && (page >= 0)) && (page <= this.LastPage))
            {
                this.CurrentPage = page;
                this.RefreshData();
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

        private void gpgLabelEntryCriteria_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowDlgSolution(this.Ladder.LadderDefinition.CriteriaSolution);
        }

        private void gvLadder_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            LadderParticipant row = this.gvLadder.GetRow(e.RowHandle) as LadderParticipant;
            if ((row != null) && row.ReportPending)
            {
                int[] selectedRows = this.gvLadder.GetSelectedRows();
                if (((selectedRows != null) && (selectedRows.Length > 0)) && (Array.IndexOf<int>(selectedRows, e.RowHandle) >= 0))
                {
                    e.Appearance.BackColor = Color.FromArgb(0xad, 0xb0, 0xd4);
                    e.Appearance.BackColor2 = Color.FromArgb(0xad, 0xb0, 0xd4);
                }
                else
                {
                    e.Appearance.BackColor = Color.FromArgb(0x45, 0x48, 0x6d);
                    e.Appearance.BackColor2 = Color.FromArgb(0x45, 0x48, 0x6d);
                }
            }
        }

        private void gvLadder_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int[] selectedRows = this.gvLadder.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                this.mSelectedParticipant = this.gvLadder.GetRow(this.gvLadder.GetRowHandle(selectedRows[0])) as LadderParticipant;
                this.RefreshToolstrip();
            }
            else
            {
                this.mSelectedParticipant = null;
                this.RefreshToolstrip();
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmLadderView));
            this.gpgLabelType = new GPGLabel();
            this.gpgLabelDates = new GPGLabel();
            this.msQuickButtons = new GPGMenuStrip(this.components);
            this.btnToggleChallengers = new ToolStripMenuItem();
            this.btnToggleMyChallenges = new ToolStripMenuItem();
            this.btnAutomatch = new ToolStripMenuItem();
            this.btnSearchSelf = new ToolStripMenuItem();
            this.btnSearchOther = new ToolStripMenuItem();
            this.btnComments = new ToolStripMenuItem();
            this.btnAuditTrail = new ToolStripMenuItem();
            this.btnChallenge = new ToolStripMenuItem();
            this.btnJoinLeave = new ToolStripMenuItem();
            this.btnHelp = new ToolStripMenuItem();
            this.btnMore = new ToolStripMenuItem();
            this.gpgDataGridLadder = new GPGDataGrid();
            this.gvLadder = new GridView();
            this.gcRank = new GridColumn();
            this.gcPrevRank = new GridColumn();
            this.gcName = new GridColumn();
            this.gcWins = new GridColumn();
            this.gcLosses = new GridColumn();
            this.gcDraws = new GridColumn();
            this.gcWinPercentage = new GridColumn();
            this.gcNonReports = new GridColumn();
            this.gcConflicts = new GridColumn();
            this.gcCurrentStreak = new GridColumn();
            this.gcRecordStreak = new GridColumn();
            this.gcDaysSinceChallenge = new GridColumn();
            this.gcDaysUntilDegrade = new GridColumn();
            this.gcRepRating = new GridColumn();
            this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
            this.gcRepRater = new GridColumn();
            this.repositoryItemPictureEdit2 = new RepositoryItemPictureEdit();
            this.gpgPanelLinkPages = new GPGPanel();
            this.skinButtonEnd = new SkinButton();
            this.skinButtonStart = new SkinButton();
            this.skinButtonNext = new SkinButton();
            this.skinButtonLast = new SkinButton();
            this.backLabelTitle = new SkinLabel();
            this.gpgLabelDescription = new GPGLabel();
            this.skinGroupPanel1 = new SkinGroupPanel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelRules = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.msQuickButtons.SuspendLayout();
            this.gpgDataGridLadder.BeginInit();
            this.gvLadder.BeginInit();
            this.repositoryItemPictureEdit1.BeginInit();
            this.repositoryItemPictureEdit2.BeginInit();
            this.skinGroupPanel1.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x31f, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabelType.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelType.AutoSize = true;
            this.gpgLabelType.AutoStyle = true;
            this.gpgLabelType.BackColor = Color.Transparent;
            this.gpgLabelType.Font = new Font("Arial", 9.75f);
            this.gpgLabelType.ForeColor = Color.White;
            this.gpgLabelType.IgnoreMouseWheel = false;
            this.gpgLabelType.IsStyled = false;
            this.gpgLabelType.Location = new Point(3, 0x2e);
            this.gpgLabelType.Name = "gpgLabelType";
            this.gpgLabelType.Size = new Size(0x21, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelType, null);
            this.gpgLabelType.TabIndex = 9;
            this.gpgLabelType.Text = "type";
            this.gpgLabelType.TextStyle = TextStyles.Default;
            this.gpgLabelDates.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDates.AutoSize = true;
            this.gpgLabelDates.AutoStyle = true;
            this.gpgLabelDates.BackColor = Color.Transparent;
            this.gpgLabelDates.Font = new Font("Arial", 9.75f);
            this.gpgLabelDates.ForeColor = Color.White;
            this.gpgLabelDates.IgnoreMouseWheel = false;
            this.gpgLabelDates.IsStyled = false;
            this.gpgLabelDates.Location = new Point(0xba, 0x2e);
            this.gpgLabelDates.Name = "gpgLabelDates";
            this.gpgLabelDates.Size = new Size(40, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelDates, null);
            this.gpgLabelDates.TabIndex = 12;
            this.gpgLabelDates.Text = "dates";
            this.gpgLabelDates.TextStyle = TextStyles.Default;
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.msQuickButtons.AutoSize = false;
            this.msQuickButtons.BackgroundImage = (Image) manager.GetObject("msQuickButtons.BackgroundImage");
            this.msQuickButtons.Dock = DockStyle.None;
            this.msQuickButtons.GripMargin = new Padding(0);
            this.msQuickButtons.ImageScalingSize = new Size(0x2d, 0x2d);
            this.msQuickButtons.Items.AddRange(new ToolStripItem[] { this.btnToggleChallengers, this.btnToggleMyChallenges, this.btnAutomatch, this.btnSearchSelf, this.btnSearchOther, this.btnComments, this.btnAuditTrail, this.btnChallenge, this.btnJoinLeave, this.btnHelp, this.btnMore });
            this.msQuickButtons.Location = new Point(9, 0x26e);
            this.msQuickButtons.Name = "msQuickButtons";
            this.msQuickButtons.Padding = new Padding(0, 0, 10, 0);
            this.msQuickButtons.RenderMode = ToolStripRenderMode.Professional;
            this.msQuickButtons.ShowItemToolTips = true;
            this.msQuickButtons.Size = new Size(840, 0x34);
            base.ttDefault.SetSuperTip(this.msQuickButtons, null);
            this.msQuickButtons.TabIndex = 13;
            this.msQuickButtons.Paint += new PaintEventHandler(this.msQuickButtons_Paint);
            this.msQuickButtons.SizeChanged += new EventHandler(this.msQuickButtons_SizeChanged);
            this.btnToggleChallengers.AutoSize = false;
            this.btnToggleChallengers.AutoToolTip = true;
            this.btnToggleChallengers.Enabled = false;
            this.btnToggleChallengers.Image = (Image) manager.GetObject("btnToggleChallengers.Image");
            this.btnToggleChallengers.ImageScaling = ToolStripItemImageScaling.None;
            this.btnToggleChallengers.Name = "btnToggleChallengers";
            this.btnToggleChallengers.ShortcutKeys = Keys.F8;
            this.btnToggleChallengers.Size = new Size(0x25, 0x34);
            this.btnToggleChallengers.ToolTipText = "<LOC>Show only participants who are accepting challenges";
            this.btnToggleChallengers.Click += new EventHandler(this.btnToggleChallengers_Click);
            this.btnToggleMyChallenges.AutoSize = false;
            this.btnToggleMyChallenges.AutoToolTip = true;
            this.btnToggleMyChallenges.Enabled = false;
            this.btnToggleMyChallenges.Image = (Image) manager.GetObject("btnToggleMyChallenges.Image");
            this.btnToggleMyChallenges.ImageScaling = ToolStripItemImageScaling.None;
            this.btnToggleMyChallenges.Name = "btnToggleMyChallenges";
            this.btnToggleMyChallenges.ShortcutKeys = Keys.F8;
            this.btnToggleMyChallenges.Size = new Size(0x25, 0x34);
            this.btnToggleMyChallenges.Click += new EventHandler(this.btnToggleMyChallenges_Click);
            this.btnAutomatch.AutoSize = false;
            this.btnAutomatch.AutoToolTip = true;
            this.btnAutomatch.Enabled = false;
            this.btnAutomatch.Image = (Image) manager.GetObject("btnAutomatch.Image");
            this.btnAutomatch.ImageScaling = ToolStripItemImageScaling.None;
            this.btnAutomatch.Name = "btnAutomatch";
            this.btnAutomatch.ShortcutKeys = Keys.F8;
            this.btnAutomatch.Size = new Size(0x25, 0x34);
            this.btnAutomatch.ToolTipText = "<LOC>Play Automatch";
            this.btnAutomatch.Click += new EventHandler(this.btnAutomatch_Click);
            this.btnSearchSelf.AutoSize = false;
            this.btnSearchSelf.AutoToolTip = true;
            this.btnSearchSelf.Enabled = false;
            this.btnSearchSelf.Image = (Image) manager.GetObject("btnSearchSelf.Image");
            this.btnSearchSelf.ImageScaling = ToolStripItemImageScaling.None;
            this.btnSearchSelf.Name = "btnSearchSelf";
            this.btnSearchSelf.ShortcutKeys = Keys.F8;
            this.btnSearchSelf.Size = new Size(0x25, 0x34);
            this.btnSearchSelf.ToolTipText = "<LOC>Search for yourself";
            this.btnSearchSelf.Click += new EventHandler(this.btnSearchSelf_Click);
            this.btnSearchOther.AutoSize = false;
            this.btnSearchOther.AutoToolTip = true;
            this.btnSearchOther.Enabled = false;
            this.btnSearchOther.Image = (Image) manager.GetObject("btnSearchOther.Image");
            this.btnSearchOther.ImageScaling = ToolStripItemImageScaling.None;
            this.btnSearchOther.Name = "btnSearchOther";
            this.btnSearchOther.ShortcutKeys = Keys.F8;
            this.btnSearchOther.Size = new Size(0x25, 0x34);
            this.btnSearchOther.ToolTipText = "<LOC>Search for a participant";
            this.btnSearchOther.Click += new EventHandler(this.btnSearchOther_Click);
            this.btnComments.AutoSize = false;
            this.btnComments.AutoToolTip = true;
            this.btnComments.Enabled = false;
            this.btnComments.Image = (Image) manager.GetObject("btnComments.Image");
            this.btnComments.ImageScaling = ToolStripItemImageScaling.None;
            this.btnComments.Name = "btnComments";
            this.btnComments.ShortcutKeys = Keys.F7;
            this.btnComments.Size = new Size(0x25, 0x34);
            this.btnComments.ToolTipText = "<LOC>View comments about this player";
            this.btnComments.Click += new EventHandler(this.btnComments_Click);
            this.btnAuditTrail.AutoSize = false;
            this.btnAuditTrail.AutoToolTip = true;
            this.btnAuditTrail.Enabled = false;
            this.btnAuditTrail.Image = (Image) manager.GetObject("btnAuditTrail.Image");
            this.btnAuditTrail.ImageScaling = ToolStripItemImageScaling.None;
            this.btnAuditTrail.Name = "btnAuditTrail";
            this.btnAuditTrail.ShortcutKeys = Keys.F7;
            this.btnAuditTrail.Size = new Size(0x25, 0x34);
            this.btnAuditTrail.ToolTipText = "<LOC>View this players ladder history";
            this.btnAuditTrail.Click += new EventHandler(this.btnAuditTrail_Click);
            this.btnChallenge.AutoSize = false;
            this.btnChallenge.AutoToolTip = true;
            this.btnChallenge.Enabled = false;
            this.btnChallenge.Image = (Image) manager.GetObject("btnChallenge.Image");
            this.btnChallenge.ImageScaling = ToolStripItemImageScaling.None;
            this.btnChallenge.Name = "btnChallenge";
            this.btnChallenge.ShortcutKeys = Keys.F2;
            this.btnChallenge.Size = new Size(0x25, 0x34);
            this.btnChallenge.ToolTipText = "<LOC>Challenge Selected Participant";
            this.btnChallenge.Click += new EventHandler(this.btnChallenge_Click);
            this.btnJoinLeave.AutoSize = false;
            this.btnJoinLeave.AutoToolTip = true;
            this.btnJoinLeave.Enabled = false;
            this.btnJoinLeave.Image = (Image) manager.GetObject("btnJoinLeave.Image");
            this.btnJoinLeave.ImageScaling = ToolStripItemImageScaling.None;
            this.btnJoinLeave.Name = "btnJoinLeave";
            this.btnJoinLeave.ShortcutKeys = Keys.F2;
            this.btnJoinLeave.Size = new Size(0x25, 0x34);
            this.btnJoinLeave.Click += new EventHandler(this.btnJoinLeave_Click);
            this.btnHelp.AutoSize = false;
            this.btnHelp.AutoToolTip = true;
            this.btnHelp.Image = (Image) manager.GetObject("btnHelp.Image");
            this.btnHelp.ImageScaling = ToolStripItemImageScaling.None;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.ShortcutKeys = Keys.F2;
            this.btnHelp.Size = new Size(0x25, 0x34);
            this.btnHelp.ToolTipText = "<LOC>Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnMore.AutoSize = false;
            this.btnMore.AutoToolTip = true;
            this.btnMore.Image = (Image) manager.GetObject("btnMore.Image");
            this.btnMore.ImageScaling = ToolStripItemImageScaling.None;
            this.btnMore.Name = "btnMore";
            this.btnMore.ShortcutKeys = Keys.F6;
            this.btnMore.Size = new Size(20, 0x34);
            this.btnMore.ToolTipText = "<LOC>More...";
            this.btnMore.Visible = false;
            this.gpgDataGridLadder.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgDataGridLadder.CustomizeStyle = false;
            this.gpgDataGridLadder.EmbeddedNavigator.Name = "";
            this.gpgDataGridLadder.Location = new Point(9, 0x106);
            this.gpgDataGridLadder.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgDataGridLadder.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgDataGridLadder.MainView = this.gvLadder;
            this.gpgDataGridLadder.Name = "gpgDataGridLadder";
            this.gpgDataGridLadder.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemPictureEdit1, this.repositoryItemPictureEdit2 });
            this.gpgDataGridLadder.ShowOnlyPredefinedDetails = true;
            this.gpgDataGridLadder.Size = new Size(840, 340);
            this.gpgDataGridLadder.TabIndex = 14;
            this.gpgDataGridLadder.ViewCollection.AddRange(new BaseView[] { this.gvLadder });
            this.gvLadder.Appearance.Empty.BackColor = Color.Black;
            this.gvLadder.Appearance.Empty.Options.UseBackColor = true;
            this.gvLadder.Appearance.EvenRow.BackColor = Color.Black;
            this.gvLadder.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvLadder.Appearance.FocusedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvLadder.Appearance.FocusedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvLadder.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvLadder.Appearance.FocusedRow.Options.UseFont = true;
            this.gvLadder.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvLadder.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvLadder.Appearance.HideSelectionRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvLadder.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvLadder.Appearance.OddRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvLadder.Appearance.OddRow.Options.UseBackColor = true;
            this.gvLadder.Appearance.Preview.BackColor = Color.Black;
            this.gvLadder.Appearance.Preview.Options.UseBackColor = true;
            this.gvLadder.Appearance.Row.BackColor = Color.Black;
            this.gvLadder.Appearance.Row.ForeColor = Color.White;
            this.gvLadder.Appearance.Row.Options.UseBackColor = true;
            this.gvLadder.Appearance.RowSeparator.BackColor = Color.Black;
            this.gvLadder.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvLadder.Appearance.SelectedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvLadder.Appearance.SelectedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvLadder.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvLadder.Appearance.SelectedRow.Options.UseFont = true;
            this.gvLadder.AppearancePrint.Row.ForeColor = Color.White;
            this.gvLadder.AppearancePrint.Row.Options.UseForeColor = true;
            this.gvLadder.BorderStyle = BorderStyles.NoBorder;
            this.gvLadder.ColumnPanelRowHeight = 30;
            this.gvLadder.Columns.AddRange(new GridColumn[] { this.gcRank, this.gcPrevRank, this.gcName, this.gcWins, this.gcLosses, this.gcDraws, this.gcWinPercentage, this.gcNonReports, this.gcConflicts, this.gcCurrentStreak, this.gcRecordStreak, this.gcDaysSinceChallenge, this.gcDaysUntilDegrade, this.gcRepRating, this.gcRepRater });
            this.gvLadder.GridControl = this.gpgDataGridLadder;
            this.gvLadder.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvLadder.Name = "gvLadder";
            this.gvLadder.OptionsMenu.EnableColumnMenu = false;
            this.gvLadder.OptionsMenu.EnableFooterMenu = false;
            this.gvLadder.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvLadder.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLadder.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvLadder.OptionsView.EnableAppearanceEvenRow = true;
            this.gvLadder.OptionsView.EnableAppearanceOddRow = true;
            this.gvLadder.OptionsView.ShowHorzLines = false;
            this.gvLadder.OptionsView.ShowIndicator = false;
            this.gvLadder.OptionsView.ShowVertLines = false;
            this.gvLadder.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvLadder_CustomDrawCell);
            this.gvLadder.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gvLadder_FocusedRowChanged);
            this.gcRank.AppearanceCell.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcRank.AppearanceCell.ForeColor = Color.Gold;
            this.gcRank.AppearanceCell.Options.UseFont = true;
            this.gcRank.AppearanceCell.Options.UseForeColor = true;
            this.gcRank.AppearanceCell.Options.UseTextOptions = true;
            this.gcRank.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcRank.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcRank.AppearanceHeader.Options.UseFont = true;
            this.gcRank.Caption = "<LOC>Rank";
            this.gcRank.FieldName = "Rank";
            this.gcRank.Name = "gcRank";
            this.gcRank.OptionsColumn.AllowEdit = false;
            this.gcRank.Visible = true;
            this.gcRank.VisibleIndex = 0;
            this.gcPrevRank.AppearanceCell.ForeColor = Color.White;
            this.gcPrevRank.AppearanceCell.Options.UseForeColor = true;
            this.gcPrevRank.AppearanceCell.Options.UseTextOptions = true;
            this.gcPrevRank.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcPrevRank.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcPrevRank.AppearanceHeader.Options.UseFont = true;
            this.gcPrevRank.Caption = "<LOC>Prev Rank";
            this.gcPrevRank.FieldName = "LastRank";
            this.gcPrevRank.Name = "gcPrevRank";
            this.gcPrevRank.OptionsColumn.AllowEdit = false;
            this.gcPrevRank.Visible = true;
            this.gcPrevRank.VisibleIndex = 1;
            this.gcName.AppearanceCell.ForeColor = Color.White;
            this.gcName.AppearanceCell.Options.UseForeColor = true;
            this.gcName.AppearanceCell.Options.UseTextOptions = true;
            this.gcName.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcName.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcName.AppearanceHeader.Options.UseFont = true;
            this.gcName.Caption = "<LOC>Name";
            this.gcName.FieldName = "EntityName";
            this.gcName.Name = "gcName";
            this.gcName.OptionsColumn.AllowEdit = false;
            this.gcName.Visible = true;
            this.gcName.VisibleIndex = 2;
            this.gcWins.AppearanceCell.ForeColor = Color.SeaGreen;
            this.gcWins.AppearanceCell.Options.UseForeColor = true;
            this.gcWins.AppearanceCell.Options.UseTextOptions = true;
            this.gcWins.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcWins.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcWins.AppearanceHeader.Options.UseFont = true;
            this.gcWins.Caption = "<LOC>Wins";
            this.gcWins.FieldName = "Wins";
            this.gcWins.Name = "gcWins";
            this.gcWins.OptionsColumn.AllowEdit = false;
            this.gcWins.Visible = true;
            this.gcWins.VisibleIndex = 3;
            this.gcLosses.AppearanceCell.ForeColor = Color.Red;
            this.gcLosses.AppearanceCell.Options.UseForeColor = true;
            this.gcLosses.AppearanceCell.Options.UseTextOptions = true;
            this.gcLosses.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcLosses.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcLosses.AppearanceHeader.Options.UseFont = true;
            this.gcLosses.Caption = "<LOC>Losses";
            this.gcLosses.FieldName = "Losses";
            this.gcLosses.Name = "gcLosses";
            this.gcLosses.OptionsColumn.AllowEdit = false;
            this.gcLosses.Visible = true;
            this.gcLosses.VisibleIndex = 4;
            this.gcDraws.AppearanceCell.ForeColor = Color.White;
            this.gcDraws.AppearanceCell.Options.UseForeColor = true;
            this.gcDraws.AppearanceCell.Options.UseTextOptions = true;
            this.gcDraws.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDraws.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcDraws.AppearanceHeader.Options.UseFont = true;
            this.gcDraws.Caption = "<LOC>Draws";
            this.gcDraws.FieldName = "Draws";
            this.gcDraws.Name = "gcDraws";
            this.gcDraws.OptionsColumn.AllowEdit = false;
            this.gcDraws.Visible = true;
            this.gcDraws.VisibleIndex = 5;
            this.gcWinPercentage.AppearanceCell.ForeColor = Color.White;
            this.gcWinPercentage.AppearanceCell.Options.UseForeColor = true;
            this.gcWinPercentage.AppearanceCell.Options.UseTextOptions = true;
            this.gcWinPercentage.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcWinPercentage.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcWinPercentage.AppearanceHeader.Options.UseFont = true;
            this.gcWinPercentage.Caption = "<LOC>Win %";
            this.gcWinPercentage.DisplayFormat.FormatString = "{0}%";
            this.gcWinPercentage.DisplayFormat.FormatType = FormatType.Custom;
            this.gcWinPercentage.FieldName = "WinPercentage";
            this.gcWinPercentage.Name = "gcWinPercentage";
            this.gcWinPercentage.OptionsColumn.AllowEdit = false;
            this.gcWinPercentage.Visible = true;
            this.gcWinPercentage.VisibleIndex = 6;
            this.gcNonReports.AppearanceCell.ForeColor = Color.White;
            this.gcNonReports.AppearanceCell.Options.UseForeColor = true;
            this.gcNonReports.AppearanceCell.Options.UseTextOptions = true;
            this.gcNonReports.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcNonReports.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcNonReports.AppearanceHeader.Options.UseFont = true;
            this.gcNonReports.Caption = "<LOC>NR";
            this.gcNonReports.FieldName = "NonReportCount";
            this.gcNonReports.Name = "gcNonReports";
            this.gcNonReports.OptionsColumn.AllowEdit = false;
            this.gcNonReports.Visible = true;
            this.gcNonReports.VisibleIndex = 7;
            this.gcConflicts.AppearanceCell.ForeColor = Color.White;
            this.gcConflicts.AppearanceCell.Options.UseForeColor = true;
            this.gcConflicts.AppearanceCell.Options.UseTextOptions = true;
            this.gcConflicts.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcConflicts.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcConflicts.AppearanceHeader.Options.UseFont = true;
            this.gcConflicts.Caption = "<LOC>Conflicts";
            this.gcConflicts.FieldName = "ConflictReportCount";
            this.gcConflicts.Name = "gcConflicts";
            this.gcConflicts.OptionsColumn.AllowEdit = false;
            this.gcConflicts.Visible = true;
            this.gcConflicts.VisibleIndex = 8;
            this.gcCurrentStreak.AppearanceCell.ForeColor = Color.White;
            this.gcCurrentStreak.AppearanceCell.Options.UseForeColor = true;
            this.gcCurrentStreak.AppearanceCell.Options.UseTextOptions = true;
            this.gcCurrentStreak.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcCurrentStreak.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcCurrentStreak.AppearanceHeader.Options.UseFont = true;
            this.gcCurrentStreak.Caption = "<LOC>Current Streak";
            this.gcCurrentStreak.FieldName = "CurrentStreak";
            this.gcCurrentStreak.Name = "gcCurrentStreak";
            this.gcCurrentStreak.OptionsColumn.AllowEdit = false;
            this.gcCurrentStreak.Visible = true;
            this.gcCurrentStreak.VisibleIndex = 9;
            this.gcRecordStreak.AppearanceCell.ForeColor = Color.White;
            this.gcRecordStreak.AppearanceCell.Options.UseForeColor = true;
            this.gcRecordStreak.AppearanceCell.Options.UseTextOptions = true;
            this.gcRecordStreak.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcRecordStreak.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcRecordStreak.AppearanceHeader.Options.UseFont = true;
            this.gcRecordStreak.Caption = "<LOC>Record Streak";
            this.gcRecordStreak.FieldName = "RecordStreak";
            this.gcRecordStreak.Name = "gcRecordStreak";
            this.gcRecordStreak.OptionsColumn.AllowEdit = false;
            this.gcRecordStreak.Visible = true;
            this.gcRecordStreak.VisibleIndex = 10;
            this.gcDaysSinceChallenge.AppearanceCell.ForeColor = Color.White;
            this.gcDaysSinceChallenge.AppearanceCell.Options.UseForeColor = true;
            this.gcDaysSinceChallenge.AppearanceCell.Options.UseTextOptions = true;
            this.gcDaysSinceChallenge.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDaysSinceChallenge.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcDaysSinceChallenge.AppearanceHeader.Options.UseFont = true;
            this.gcDaysSinceChallenge.Caption = "<LOC>Days Since Challenge";
            this.gcDaysSinceChallenge.FieldName = "DaysSinceChallenge";
            this.gcDaysSinceChallenge.Name = "gcDaysSinceChallenge";
            this.gcDaysSinceChallenge.OptionsColumn.AllowEdit = false;
            this.gcDaysSinceChallenge.Visible = true;
            this.gcDaysSinceChallenge.VisibleIndex = 11;
            this.gcDaysUntilDegrade.AppearanceCell.ForeColor = Color.White;
            this.gcDaysUntilDegrade.AppearanceCell.Options.UseForeColor = true;
            this.gcDaysUntilDegrade.AppearanceCell.Options.UseTextOptions = true;
            this.gcDaysUntilDegrade.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDaysUntilDegrade.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcDaysUntilDegrade.AppearanceHeader.Options.UseFont = true;
            this.gcDaysUntilDegrade.Caption = "<LOC>Days Until Degrade";
            this.gcDaysUntilDegrade.FieldName = "DaysUntilDegrade";
            this.gcDaysUntilDegrade.Name = "gcDaysUntilDegrade";
            this.gcDaysUntilDegrade.OptionsColumn.AllowEdit = false;
            this.gcDaysUntilDegrade.Visible = true;
            this.gcDaysUntilDegrade.VisibleIndex = 12;
            this.gcRepRating.AppearanceCell.Options.UseTextOptions = true;
            this.gcRepRating.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcRepRating.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcRepRating.AppearanceHeader.Options.UseFont = true;
            this.gcRepRating.Caption = "<LOC>Rating Score";
            this.gcRepRating.ColumnEdit = this.repositoryItemPictureEdit1;
            this.gcRepRating.FieldName = "RatingImageSmall";
            this.gcRepRating.Name = "gcRepRating";
            this.gcRepRating.OptionsColumn.AllowEdit = false;
            this.gcRepRating.Visible = true;
            this.gcRepRating.VisibleIndex = 13;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.gcRepRater.AppearanceCell.Options.UseTextOptions = true;
            this.gcRepRater.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcRepRater.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcRepRater.AppearanceHeader.Options.UseFont = true;
            this.gcRepRater.Caption = "<LOC>Rater Score";
            this.gcRepRater.ColumnEdit = this.repositoryItemPictureEdit2;
            this.gcRepRater.FieldName = "RaterImageSmall";
            this.gcRepRater.Name = "gcRepRater";
            this.gcRepRater.OptionsColumn.AllowEdit = false;
            this.gcRepRater.Visible = true;
            this.gcRepRater.VisibleIndex = 14;
            this.repositoryItemPictureEdit2.Name = "repositoryItemPictureEdit2";
            this.gpgPanelLinkPages.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgPanelLinkPages.BackColor = Color.DarkGray;
            this.gpgPanelLinkPages.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelLinkPages.BorderThickness = 2;
            this.gpgPanelLinkPages.DrawBorder = false;
            this.gpgPanelLinkPages.Location = new Point(0x58, 0x260);
            this.gpgPanelLinkPages.Margin = new Padding(0);
            this.gpgPanelLinkPages.Name = "gpgPanelLinkPages";
            this.gpgPanelLinkPages.Size = new Size(0x2aa, 0x16);
            base.ttDefault.SetSuperTip(this.gpgPanelLinkPages, null);
            this.gpgPanelLinkPages.TabIndex = 0x17;
            this.skinButtonEnd.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonEnd.AutoStyle = true;
            this.skinButtonEnd.BackColor = Color.Black;
            this.skinButtonEnd.ButtonState = 0;
            this.skinButtonEnd.DialogResult = DialogResult.OK;
            this.skinButtonEnd.DisabledForecolor = Color.Gray;
            this.skinButtonEnd.DrawColor = Color.White;
            this.skinButtonEnd.DrawEdges = false;
            this.skinButtonEnd.FocusColor = Color.Yellow;
            this.skinButtonEnd.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonEnd.ForeColor = Color.White;
            this.skinButtonEnd.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonEnd.IsStyled = true;
            this.skinButtonEnd.Location = new Point(0x329, 0x260);
            this.skinButtonEnd.Name = "skinButtonEnd";
            this.skinButtonEnd.Size = new Size(40, 0x16);
            this.skinButtonEnd.SkinBasePath = @"Controls\Button\Last";
            base.ttDefault.SetSuperTip(this.skinButtonEnd, null);
            this.skinButtonEnd.TabIndex = 0x16;
            this.skinButtonEnd.TabStop = true;
            this.skinButtonEnd.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonEnd.TextPadding = new Padding(0);
            this.skinButtonEnd.Click += new EventHandler(this.skinButtonEnd_Click);
            this.skinButtonStart.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonStart.AutoStyle = true;
            this.skinButtonStart.BackColor = Color.Black;
            this.skinButtonStart.ButtonState = 0;
            this.skinButtonStart.DialogResult = DialogResult.OK;
            this.skinButtonStart.DisabledForecolor = Color.Gray;
            this.skinButtonStart.DrawColor = Color.White;
            this.skinButtonStart.DrawEdges = false;
            this.skinButtonStart.FocusColor = Color.Yellow;
            this.skinButtonStart.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonStart.ForeColor = Color.White;
            this.skinButtonStart.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonStart.IsStyled = true;
            this.skinButtonStart.Location = new Point(9, 0x260);
            this.skinButtonStart.Name = "skinButtonStart";
            this.skinButtonStart.Size = new Size(40, 0x16);
            this.skinButtonStart.SkinBasePath = @"Controls\Button\First";
            base.ttDefault.SetSuperTip(this.skinButtonStart, null);
            this.skinButtonStart.TabIndex = 0x15;
            this.skinButtonStart.TabStop = true;
            this.skinButtonStart.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonStart.TextPadding = new Padding(0);
            this.skinButtonStart.Click += new EventHandler(this.skinButtonStart_Click);
            this.skinButtonNext.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonNext.AutoStyle = true;
            this.skinButtonNext.BackColor = Color.Black;
            this.skinButtonNext.ButtonState = 0;
            this.skinButtonNext.DialogResult = DialogResult.OK;
            this.skinButtonNext.DisabledForecolor = Color.Gray;
            this.skinButtonNext.DrawColor = Color.White;
            this.skinButtonNext.DrawEdges = false;
            this.skinButtonNext.FocusColor = Color.Yellow;
            this.skinButtonNext.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNext.ForeColor = Color.White;
            this.skinButtonNext.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNext.IsStyled = true;
            this.skinButtonNext.Location = new Point(0x301, 0x260);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(40, 0x16);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Next";
            base.ttDefault.SetSuperTip(this.skinButtonNext, null);
            this.skinButtonNext.TabIndex = 20;
            this.skinButtonNext.TabStop = true;
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.skinButtonLast.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonLast.AutoStyle = true;
            this.skinButtonLast.BackColor = Color.Black;
            this.skinButtonLast.ButtonState = 0;
            this.skinButtonLast.DialogResult = DialogResult.OK;
            this.skinButtonLast.DisabledForecolor = Color.Gray;
            this.skinButtonLast.DrawColor = Color.White;
            this.skinButtonLast.DrawEdges = false;
            this.skinButtonLast.FocusColor = Color.Yellow;
            this.skinButtonLast.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLast.ForeColor = Color.White;
            this.skinButtonLast.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLast.IsStyled = true;
            this.skinButtonLast.Location = new Point(0x31, 0x260);
            this.skinButtonLast.Name = "skinButtonLast";
            this.skinButtonLast.Size = new Size(40, 0x16);
            this.skinButtonLast.SkinBasePath = @"Controls\Button\Previous";
            base.ttDefault.SetSuperTip(this.skinButtonLast, null);
            this.skinButtonLast.TabIndex = 0x13;
            this.skinButtonLast.TabStop = true;
            this.skinButtonLast.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLast.TextPadding = new Padding(0);
            this.skinButtonLast.Click += new EventHandler(this.skinButtonLast_Click);
            this.backLabelTitle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.backLabelTitle.AutoStyle = false;
            this.backLabelTitle.BackColor = Color.Transparent;
            this.backLabelTitle.DrawEdges = true;
            this.backLabelTitle.Font = new Font("Arial", 20f, FontStyle.Bold);
            this.backLabelTitle.ForeColor = Color.White;
            this.backLabelTitle.HorizontalScalingMode = ScalingModes.Tile;
            this.backLabelTitle.IsStyled = false;
            this.backLabelTitle.Location = new Point(7, 0x3e);
            this.backLabelTitle.Name = "backLabelTitle";
            this.backLabelTitle.Size = new Size(0x34e, 0x3b);
            this.backLabelTitle.SkinBasePath = @"Controls\Background Label\Ladders";
            base.ttDefault.SetSuperTip(this.backLabelTitle, null);
            this.backLabelTitle.TabIndex = 0x18;
            this.backLabelTitle.Text = "<LOC>Ladder Ratings";
            this.backLabelTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.backLabelTitle.TextPadding = new Padding(10, 0, 0, 0);
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.BackColor = Color.Transparent;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(3, 0x59);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(410, 0x23);
            base.ttDefault.SetSuperTip(this.gpgLabelDescription, null);
            this.gpgLabelDescription.TabIndex = 0x19;
            this.gpgLabelDescription.Text = "description";
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            this.skinGroupPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinGroupPanel1.AutoStyle = false;
            this.skinGroupPanel1.BackColor = Color.Black;
            this.skinGroupPanel1.Controls.Add(this.gpgLabel3);
            this.skinGroupPanel1.Controls.Add(this.gpgLabelRules);
            this.skinGroupPanel1.Controls.Add(this.gpgLabel4);
            this.skinGroupPanel1.Controls.Add(this.gpgLabel2);
            this.skinGroupPanel1.Controls.Add(this.gpgLabel1);
            this.skinGroupPanel1.Controls.Add(this.gpgLabelDescription);
            this.skinGroupPanel1.Controls.Add(this.gpgLabelType);
            this.skinGroupPanel1.Controls.Add(this.gpgLabelDates);
            this.skinGroupPanel1.CutCorner = true;
            this.skinGroupPanel1.Font = new Font("Verdana", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.skinGroupPanel1.HeaderImage = GroupPanelImages.blue_gradient;
            this.skinGroupPanel1.IsStyled = true;
            this.skinGroupPanel1.Location = new Point(13, 0x83);
            this.skinGroupPanel1.Margin = new Padding(4, 3, 4, 3);
            this.skinGroupPanel1.Name = "skinGroupPanel1";
            this.skinGroupPanel1.Size = new Size(0x340, 0x81);
            base.ttDefault.SetSuperTip(this.skinGroupPanel1, null);
            this.skinGroupPanel1.TabIndex = 0x1a;
            this.skinGroupPanel1.Text = "<LOC>Ladder Information";
            this.skinGroupPanel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinGroupPanel1.TextPadding = new Padding(4, 0, 0, 0);
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(3, 0x43);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(410, 0x12);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 0x29;
            this.gpgLabel3.Text = "<LOC>Description";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgLabelRules.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelRules.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelRules.AutoStyle = true;
            this.gpgLabelRules.BackColor = Color.Transparent;
            this.gpgLabelRules.Font = new Font("Arial", 9.75f);
            this.gpgLabelRules.ForeColor = Color.White;
            this.gpgLabelRules.IgnoreMouseWheel = false;
            this.gpgLabelRules.IsStyled = false;
            this.gpgLabelRules.Location = new Point(0x1ad, 0x2e);
            this.gpgLabelRules.Name = "gpgLabelRules";
            this.gpgLabelRules.Size = new Size(410, 0x43);
            base.ttDefault.SetSuperTip(this.gpgLabelRules, null);
            this.gpgLabelRules.TabIndex = 0x2b;
            this.gpgLabelRules.Text = "description";
            this.gpgLabelRules.TextStyle = TextStyles.Default;
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x1ad, 0x18);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(400, 0x12);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 0x2a;
            this.gpgLabel4.Text = "<LOC>Rules and Criteria";
            this.gpgLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0xba, 0x18);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0xb3, 0x12);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 40;
            this.gpgLabel2.Text = "<LOC>Season Dates";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(3, 0x18);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(410, 0x12);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 0x27;
            this.gpgLabel1.Text = "<LOC>Type";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x35a, 0x2ad);
            base.Controls.Add(this.skinGroupPanel1);
            base.Controls.Add(this.backLabelTitle);
            base.Controls.Add(this.gpgPanelLinkPages);
            base.Controls.Add(this.skinButtonEnd);
            base.Controls.Add(this.skinButtonStart);
            base.Controls.Add(this.skinButtonNext);
            base.Controls.Add(this.skinButtonLast);
            base.Controls.Add(this.gpgDataGridLadder);
            base.Controls.Add(this.msQuickButtons);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x1c8, 0x180);
            base.Name = "FrmLadderView";
            base.Opacity = 1.0;
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Ladder View";
            base.Controls.SetChildIndex(this.msQuickButtons, 0);
            base.Controls.SetChildIndex(this.gpgDataGridLadder, 0);
            base.Controls.SetChildIndex(this.skinButtonLast, 0);
            base.Controls.SetChildIndex(this.skinButtonNext, 0);
            base.Controls.SetChildIndex(this.skinButtonStart, 0);
            base.Controls.SetChildIndex(this.skinButtonEnd, 0);
            base.Controls.SetChildIndex(this.gpgPanelLinkPages, 0);
            base.Controls.SetChildIndex(this.backLabelTitle, 0);
            base.Controls.SetChildIndex(this.skinGroupPanel1, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.msQuickButtons.ResumeLayout(false);
            this.msQuickButtons.PerformLayout();
            this.gpgDataGridLadder.EndInit();
            this.gvLadder.EndInit();
            this.repositoryItemPictureEdit1.EndInit();
            this.repositoryItemPictureEdit2.EndInit();
            this.skinGroupPanel1.ResumeLayout(false);
            this.skinGroupPanel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void item_Paint(object sender, PaintEventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
            e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(this.msQuickButtons.BackgroundImage.Width, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
            if (item.Enabled)
            {
                if (item.Selected)
                {
                    e.Graphics.DrawImage(DrawUtil.AdjustColors(0.8f, 0.8f, 1f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                }
                else
                {
                    e.Graphics.DrawImage(item.Image, new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                }
            }
            else
            {
                e.Graphics.DrawImage(DrawUtil.GetTransparentImage(0.5f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
            }
        }

        internal void JumpTo(int rank)
        {
            try
            {
                if (rank > 0)
                {
                    int rowHandle = this.gvLadder.GetRowHandle((rank - 1) % PAGE_SIZE);
                    this._ChangePage((rank - 1) / PAGE_SIZE, rowHandle);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void JumpTo(string name)
        {
            try
            {
                int @int = new QuazalQuery("GetPlayerLadderRankByName", new object[] { this.Ladder.ID, name }).GetInt();
                if (@int > 0)
                {
                    this.JumpTo(@int);
                    this.gvLadder.RefreshData();
                }
                else
                {
                    DlgMessage.ShowDialog(string.Format(Loc.Get("<LOC>Unable to locate a participant by the name {0}"), name));
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void MainForm_LadderGamePlayed(object sender, EventArgs e)
        {
            if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
            {
                ThreadPool.QueueUserWorkItem(delegate (object state) {
                    VGen0 method = null;
                    this.IsLadderParticipant = new QuazalQuery("IsLadderParticipant", new object[] { User.Current.ID, this.Ladder.ID }).GetBool();
                    this.IsJoinPending = new QuazalQuery("IsLadderJoinPending", new object[] { User.Current.ID, this.Ladder.ID }).GetBool();
                    this.IsLeavePending = new QuazalQuery("IsLadderRemovePending", new object[] { User.Current.ID, this.Ladder.ID }).GetBool();
                    if ((!base.Disposing && !base.IsDisposed) && base.IsHandleCreated)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                if (this.IsJoinPending)
                                {
                                    this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Join Pending");
                                }
                                else if (this.IsLeavePending)
                                {
                                    this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Leave Pending");
                                }
                                else if (this.IsLadderParticipant)
                                {
                                    this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Leave This Ladder");
                                    this.btnJoinLeave.Image = this.btnJoinLeave.Image;
                                }
                                else
                                {
                                    this.btnJoinLeave.ToolTipText = Loc.Get("<LOC>Join This Ladder");
                                    this.btnJoinLeave.Image = this.btnJoinLeave.Image;
                                }
                                this.RefreshToolstrip();
                            };
                        }
                        base.BeginInvoke(method);
                    }
                });
            }
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
                if (num3 > 0)
                {
                    this.btnMore.Visible = true;
                    this.btnMore.DropDownDirection = ToolStripDropDownDirection.AboveRight;
                    this.btnMore.DropDown.AutoSize = false;
                    this.btnMore.DropDown.Width = this.msQuickButtons.Items[0].Width;
                    this.btnMore.DropDown.Height = (this.msQuickButtons.Items[0].Height * num3) + 3;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.msQuickButtons.Location = new Point(0, 0);
            this.msQuickButtons.Width = base.pbBottom.Width - 0x12;
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            base.pbBottom.Controls.Add(this.msQuickButtons);
            this.RefreshToolstrip();
        }

        private void OnRefreshData(DataList data)
        {
            if ((data != null) && (data.Count > 0))
            {
                this.mCurrentDataSet = new MappedObjectList<LadderParticipant>(data);
                this.gpgDataGridLadder.DataSource = null;
                this.gpgDataGridLadder.DataSource = this.CurrentDataSet;
                this.gvLadder.RefreshData();
            }
            else
            {
                this.gpgDataGridLadder.DataSource = null;
                this.gvLadder.RefreshData();
            }
            this.gvLadder_FocusedRowChanged(null, null);
            this.RefreshToolstrip();
            this.ResizePageLinks();
        }

        internal void RefreshChallengeToggle()
        {
            if (this.AcceptingChallenges)
            {
                this.btnToggleMyChallenges.Image = SkinManager.GetImage(@"Dialog\LadderView\accept_challenges.png");
                this.btnToggleMyChallenges.ToolTipText = Loc.Get("<LOC>Accepting Challenges");
            }
            else
            {
                this.btnToggleMyChallenges.Image = SkinManager.GetImage(@"Dialog\LadderView\decline_challenges.png");
                this.btnToggleMyChallenges.ToolTipText = Loc.Get("<LOC>Declining Challenges");
            }
        }

        private void RefreshData()
        {
            int num = this.CurrentPage * PAGE_SIZE;
            int num2 = (this.CurrentPage + 1) * PAGE_SIZE;
            if (this.IsViewingChallengers)
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshData", new object[] { "GetLadderChallengers", new object[] { this.Ladder.ID, num, num2 } });
            }
            else
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshData", new object[] { "GetLadderParticipants", new object[] { this.Ladder.ID, num, num2 } });
            }
        }

        private void RefreshDataSynchronous()
        {
            DataList data;
            int num = this.CurrentPage * PAGE_SIZE;
            int num2 = (this.CurrentPage + 1) * PAGE_SIZE;
            if (this.IsViewingChallengers)
            {
                data = new QuazalQuery("GetLadderChallengers", new object[] { this.Ladder.ID, num, num2 }).GetData();
                this.OnRefreshData(data);
            }
            else
            {
                data = new QuazalQuery("GetLadderParticipants", new object[] { this.Ladder.ID, num, num2 }).GetData();
                this.OnRefreshData(data);
            }
        }

        internal void RefreshToolstrip()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RefreshToolstrip();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    this.btnChallenge.Enabled = ((this.IsLadderParticipant && (this.SelectedParticipant != null)) && (this.SelectedParticipant.AcceptingChallenges && !base.MainForm.SearchingForAutomatch)) && (this.SelectedParticipant.EntityID != User.Current.ID);
                    this.btnAutomatch.Enabled = this.IsLadderParticipant && !base.MainForm.SearchingForAutomatch;
                    this.btnToggleMyChallenges.Enabled = this.IsLadderParticipant;
                    this.btnComments.Enabled = this.SelectedParticipant != null;
                    this.btnAuditTrail.Enabled = this.SelectedParticipant != null;
                    this.btnToggleChallengers.Enabled = (this.CurrentDataSet != null) && (this.CurrentDataSet.Count > 0);
                    this.btnSearchOther.Enabled = (this.CurrentDataSet != null) && (this.CurrentDataSet.Count > 0);
                    this.btnSearchSelf.Enabled = this.IsLadderParticipant;
                    this.btnJoinLeave.Enabled = !this.IsJoinPending && !this.IsLeavePending;
                    if (!(!this.IsJoinPending && this.IsLadderParticipant))
                    {
                        this.btnJoinLeave.Image = SkinManager.GetImage(@"Dialog\LadderView\join_ladder.png");
                    }
                    else if (this.IsLeavePending || this.IsLadderParticipant)
                    {
                        this.btnJoinLeave.Image = SkinManager.GetImage(@"Dialog\LadderView\leave_ladder.png");
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void ResizePageLinks()
        {
            EventHandler handler = null;
            try
            {
                int num5;
                this.gpgPanelLinkPages.Controls.Clear();
                List<int> list = new List<int>();
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                using (Graphics graphics = base.CreateGraphics())
                {
                    using (new SkinLabel().Font)
                    {
                        while (num < this.gpgPanelLinkPages.Width)
                        {
                            int width = (int) DrawUtil.MeasureString(graphics, string.Format("{0} - {1}  ", (this.CurrentPage + num2) * PAGE_SIZE, ((this.CurrentPage + num2) + 1) * PAGE_SIZE), this.Font).Width;
                            if ((num + width) > this.gpgPanelLinkPages.Width)
                            {
                                num3 = this.gpgPanelLinkPages.Width - num;
                                goto Label_012F;
                            }
                            list.Add(width);
                            num += width;
                            num2++;
                            if (num2 >= this.LastPage)
                            {
                                num3 = this.gpgPanelLinkPages.Width - num;
                                goto Label_012F;
                            }
                        }
                    }
                }
            Label_012F:
                num5 = num2;
                if (num5 >= 1)
                {
                    int num6 = this.LastPage + 1;
                    int num7 = ((this.CurrentPage + (num5 / 2)) + (num5 % 2)) - num6;
                    int num8 = this.CurrentPage - (num5 / 2);
                    int num9 = num3 / num5;
                    int num10 = num3 % num5;
                    if (num7 > 0)
                    {
                        num8 -= num7;
                    }
                    if (num8 < 0)
                    {
                        num8 = 0;
                    }
                    int x = 0;
                    for (int i = 0; i < num5; i++)
                    {
                        SkinLabel label = new SkinLabel();
                        label.SkinBasePath = @"Controls\BackgroundLabel\BlackBar";
                        label.DrawEdges = false;
                        if (i == (num5 - 1))
                        {
                            label.Size = new Size((list[i] + num9) + num10, this.gpgPanelLinkPages.Height);
                        }
                        else
                        {
                            label.Size = new Size(list[i] + num9, this.gpgPanelLinkPages.Height);
                        }
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.DrawEdges = false;
                        label.Location = new Point(x, 0);
                        label.Tag = i + num8;
                        x += list[i] + num9;
                        if ((this.CurrentDataSet == null) || (this.CurrentDataSet.Count < 1))
                        {
                            label.Text = "0";
                        }
                        else if ((i + num8) == this.LastPage)
                        {
                            label.Text = string.Format("{0} - {1}", ((i + num8) * PAGE_SIZE) + 1, ((i + num8) * PAGE_SIZE) + (this.LastRating - ((i + num8) * PAGE_SIZE)));
                        }
                        else
                        {
                            label.Text = string.Format("{0} - {1}", ((i + num8) * PAGE_SIZE) + 1, ((i + 1) + num8) * PAGE_SIZE);
                        }
                        if ((i + num8) == this.CurrentPage)
                        {
                            label.Font = new Font(Program.Settings.StylePreferences.MasterFont, FontStyle.Underline);
                            label.ForeColor = Color.White;
                        }
                        else
                        {
                            label.Font = new Font(Program.Settings.StylePreferences.MasterFont, FontStyle.Underline);
                            label.Cursor = Cursors.Hand;
                            label.ForeColor = Color.Gold;
                            if (handler == null)
                            {
                                handler = delegate (object l, EventArgs e) {
                                    this.ChangePage((int) (l as SkinControl).Tag);
                                };
                            }
                            label.Click += handler;
                        }
                        this.gpgPanelLinkPages.Controls.Add(label);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public override void SetSkin(string name)
        {
            base.SetSkin(name);
            this.msQuickButtons.BackgroundImage = SkinManager.GetImage(@"Dialog\ButtonStrip\bottom.png");
            this.msQuickButtons.Height = this.msQuickButtons.BackgroundImage.Height;
            this.btnMore.DropDown.BackgroundImage = DrawUtil.ResizeImage(SkinManager.GetImage("brushbg.png"), this.msQuickButtons.Items[0].Size);
            this.btnToggleChallengers.Image = SkinManager.GetImage(@"Dialog\LadderView\show_all.png");
            this.btnComments.Image = SkinManager.GetImage(@"Dialog\LadderView\comments.png");
            this.btnAuditTrail.Image = SkinManager.GetImage(@"Dialog\LadderView\ladder_history.png");
            this.btnSearchOther.Image = SkinManager.GetImage(@"Dialog\LadderView\search_other.png");
            this.btnSearchSelf.Image = SkinManager.GetImage(@"Dialog\LadderView\search_self.png");
            this.btnAutomatch.Image = SkinManager.GetImage(@"Dialog\LadderView\automatch.png");
            this.btnChallenge.Image = SkinManager.GetImage(@"Dialog\LadderView\challenge.png");
            this.btnHelp.Image = SkinManager.GetImage(@"Dialog\LadderView\help.png");
            this.btnMore.Image = SkinManager.GetImage("nav-more.png");
            foreach (ToolStripItem item in this.msQuickButtons.Items)
            {
                item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                item.Height = this.msQuickButtons.BackgroundImage.Height;
            }
        }

        private void skinButtonEnd_Click(object sender, EventArgs e)
        {
            if (this.CurrentPage != this.LastPage)
            {
                this.ChangePage(this.LastPage);
            }
        }

        private void skinButtonLast_Click(object sender, EventArgs e)
        {
            if (this.CurrentPage > 0)
            {
                this.ChangePage(this.CurrentPage - 1);
            }
        }

        private void skinButtonNext_Click(object sender, EventArgs e)
        {
            if ((this.CurrentDataSet != null) && (this.CurrentDataSet.Count >= (PAGE_SIZE - 1)))
            {
                this.ChangePage(this.CurrentPage + 1);
            }
        }

        private void skinButtonStart_Click(object sender, EventArgs e)
        {
            if (this.CurrentPage != 0)
            {
                this.ChangePage(0);
            }
        }

        public bool AcceptingChallenges
        {
            get
            {
                if (!AcceptingChallengesLookup.ContainsKey(this.Ladder.ID))
                {
                    AcceptingChallengesLookup[this.Ladder.ID] = new QuazalQuery("IsAcceptingLadderChallenges", new object[] { this.Ladder.ID, User.Current.ID }).GetBool();
                }
                return AcceptingChallengesLookup[this.Ladder.ID];
            }
            set
            {
                AcceptingChallengesLookup[this.Ladder.ID] = value;
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        protected override bool BottomMenuStrip
        {
            get
            {
                return true;
            }
        }

        public MappedObjectList<LadderParticipant> CurrentDataSet
        {
            get
            {
                return this.mCurrentDataSet;
            }
            set
            {
                this.mCurrentDataSet = value;
            }
        }

        public int CurrentPage
        {
            get
            {
                return this.mCurrentPage;
            }
            set
            {
                this.mCurrentPage = value;
            }
        }

        public LadderInstance Ladder
        {
            get
            {
                return this.mLadder;
            }
        }

        public int LastPage
        {
            get
            {
                return (this.LastRating / PAGE_SIZE);
            }
        }

        public int LastRating
        {
            get
            {
                if (this.mLastRating < 0)
                {
                    this.mLastRating = DataAccess.GetNumber("GetLastLadderRank", new object[] { this.Ladder.ID });
                }
                return this.mLastRating;
            }
        }

        public LadderParticipant SelectedParticipant
        {
            get
            {
                return this.mSelectedParticipant;
            }
        }
    }
}

