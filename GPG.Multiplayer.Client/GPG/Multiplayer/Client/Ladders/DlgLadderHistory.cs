namespace GPG.Multiplayer.Client.Ladders
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.LadderService;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgLadderHistory : DlgBase
    {
        private SkinButton btnCommentsTab;
        private SkinButton btnHistoryTab;
        private IContainer components = null;
        private GPGDataGrid dataGridComments;
        private GridColumn gcComment_Date;
        private GridColumn gcComment_Details;
        private GridColumn gcComment_Name;
        private GridColumn gcDescription;
        private GridColumn gcRankChange;
        private GridColumn gcReportDate;
        private GPGDataGrid gpgDataGridHistory;
        private GPGLabel gpgLabelCommentDetails;
        private GPGLabel gpgLabelDescription;
        private GPGPanel gpgPanel2;
        private GPGPanel gpgPanelComments;
        private GPGPanel gpgPanelHistory;
        private GridColumn gridColumn1;
        private GridColumn gridColumn2;
        private GridColumn gridColumn3;
        private GridView gvComments;
        private GridView gvLadder;
        private LadderParticipant mParticipant;
        private LadderParticipantComment mSelectedComment;
        private LadderGameResult mSelectedGameResult;
        private SkinButton mSelectedTab = null;
        private GPGPanel pManualTabs;
        private SkinLabel skinLabel1;
        private SkinLabel skinLabel2;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;

        public DlgLadderHistory(LadderParticipant participant, bool showCommentTab)
        {
            this.InitializeComponent();
            this.mParticipant = participant;
            this.Text = string.Format(Loc.Get("<LOC>{0} - Ladder Activity History"), this.Participant.EntityName);
            LadderGameResult[] resultArray = new QuazalQuery("GetLadderParticipantHistory", new object[] { this.Participant.LadderInstanceID, this.Participant.EntityID }).GetObjects<LadderGameResult>().ToArray();
            this.gpgDataGridHistory.DataSource = resultArray;
            this.gvLadder.RefreshData();
            LadderParticipantComment[] commentArray = new QuazalQuery("GetLadderParticipantComments", new object[] { this.Participant.EntityID }).GetObjects<LadderParticipantComment>().ToArray();
            this.dataGridComments.DataSource = commentArray;
            this.gvComments.RefreshData();
            if (showCommentTab)
            {
                this.btnCommentsTab_Click(null, null);
            }
        }

        private void btnCommentsTab_Click(object sender, EventArgs e)
        {
            this.SelectedTab.DrawColor = Color.White;
            this.mSelectedTab = this.btnCommentsTab;
            this.SelectedTab.DrawColor = Color.Black;
            this.btnHistoryTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.btnCommentsTab.SkinBasePath = @"Controls\Button\TabSmallActive";
            this.LayoutTabs();
            this.gpgPanelComments.BringToFront();
        }

        private void btnHistoryTab_Click(object sender, EventArgs e)
        {
            this.SelectedTab.DrawColor = Color.White;
            this.mSelectedTab = this.btnHistoryTab;
            this.SelectedTab.DrawColor = Color.Black;
            this.btnHistoryTab.SkinBasePath = @"Controls\Button\TabSmallActive";
            this.btnCommentsTab.SkinBasePath = @"Controls\Button\TabSmall";
            this.LayoutTabs();
            this.gpgPanelHistory.BringToFront();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gvComments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int[] selectedRows = this.gvComments.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                this.mSelectedComment = this.gvComments.GetRow(this.gvComments.GetRowHandle(selectedRows[0])) as LadderParticipantComment;
                if (this.mSelectedComment == null)
                {
                    this.gpgLabelCommentDetails.Text = "";
                }
                else
                {
                    this.gpgLabelCommentDetails.Text = this.SelectedComment.Description;
                }
            }
            else
            {
                this.mSelectedComment = null;
                this.gpgLabelCommentDetails.Text = "";
            }
        }

        private void gvLadder_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int[] selectedRows = this.gvLadder.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                this.mSelectedGameResult = this.gvLadder.GetRow(this.gvLadder.GetRowHandle(selectedRows[0])) as LadderGameResult;
                if (this.mSelectedGameResult == null)
                {
                    this.gpgLabelDescription.Text = "";
                }
                else
                {
                    this.gpgLabelDescription.Text = this.SelectedGameResult.Description;
                }
            }
            else
            {
                this.mSelectedGameResult = null;
                this.gpgLabelDescription.Text = "";
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgLadderHistory));
            this.gpgDataGridHistory = new GPGDataGrid();
            this.gvLadder = new GridView();
            this.gcReportDate = new GridColumn();
            this.gcRankChange = new GridColumn();
            this.gcDescription = new GridColumn();
            this.gpgLabelDescription = new GPGLabel();
            this.gpgPanelHistory = new GPGPanel();
            this.splitContainer1 = new SplitContainer();
            this.skinLabel1 = new SkinLabel();
            this.gpgPanel2 = new GPGPanel();
            this.pManualTabs = new GPGPanel();
            this.btnHistoryTab = new SkinButton();
            this.btnCommentsTab = new SkinButton();
            this.gpgPanelComments = new GPGPanel();
            this.splitContainer2 = new SplitContainer();
            this.dataGridComments = new GPGDataGrid();
            this.gvComments = new GridView();
            this.gcComment_Date = new GridColumn();
            this.gcComment_Name = new GridColumn();
            this.gcComment_Details = new GridColumn();
            this.skinLabel2 = new SkinLabel();
            this.gpgLabelCommentDetails = new GPGLabel();
            this.gridColumn1 = new GridColumn();
            this.gridColumn2 = new GridColumn();
            this.gridColumn3 = new GridColumn();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgDataGridHistory.BeginInit();
            this.gvLadder.BeginInit();
            this.gpgPanelHistory.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gpgPanel2.SuspendLayout();
            this.pManualTabs.SuspendLayout();
            this.gpgPanelComments.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.dataGridComments.BeginInit();
            this.gvComments.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x265, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgDataGridHistory.CustomizeStyle = false;
            this.gpgDataGridHistory.Dock = DockStyle.Fill;
            this.gpgDataGridHistory.EmbeddedNavigator.Name = "";
            this.gpgDataGridHistory.Location = new Point(0, 0);
            this.gpgDataGridHistory.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgDataGridHistory.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgDataGridHistory.MainView = this.gvLadder;
            this.gpgDataGridHistory.Name = "gpgDataGridHistory";
            this.gpgDataGridHistory.ShowOnlyPredefinedDetails = true;
            this.gpgDataGridHistory.Size = new Size(0x284, 0xee);
            this.gpgDataGridHistory.TabIndex = 15;
            this.gpgDataGridHistory.ViewCollection.AddRange(new BaseView[] { this.gvLadder });
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
            this.gvLadder.Columns.AddRange(new GridColumn[] { this.gcReportDate, this.gcRankChange, this.gcDescription });
            this.gvLadder.GridControl = this.gpgDataGridHistory;
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
            this.gvLadder.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gvLadder_FocusedRowChanged);
            this.gcReportDate.Caption = "<LOC>Report Date";
            this.gcReportDate.FieldName = "PlayerReportDateFull";
            this.gcReportDate.Name = "gcReportDate";
            this.gcReportDate.OptionsColumn.AllowEdit = false;
            this.gcReportDate.Visible = true;
            this.gcReportDate.VisibleIndex = 0;
            this.gcReportDate.Width = 0x84;
            this.gcRankChange.Caption = "<LOC>Rank Change";
            this.gcRankChange.FieldName = "RankChange";
            this.gcRankChange.Name = "gcRankChange";
            this.gcRankChange.OptionsColumn.AllowEdit = false;
            this.gcRankChange.Visible = true;
            this.gcRankChange.VisibleIndex = 1;
            this.gcRankChange.Width = 0x86;
            this.gcDescription.AppearanceCell.ForeColor = Color.White;
            this.gcDescription.AppearanceCell.Options.UseForeColor = true;
            this.gcDescription.AppearanceCell.Options.UseTextOptions = true;
            this.gcDescription.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDescription.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcDescription.AppearanceHeader.Options.UseFont = true;
            this.gcDescription.Caption = "<LOC>Click to view full description below";
            this.gcDescription.FieldName = "Description";
            this.gcDescription.Name = "gcDescription";
            this.gcDescription.OptionsColumn.AllowEdit = false;
            this.gcDescription.Visible = true;
            this.gcDescription.VisibleIndex = 2;
            this.gcDescription.Width = 0x17a;
            this.gpgLabelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.BackColor = Color.Black;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(0, 0x17);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Padding = new Padding(6);
            this.gpgLabelDescription.Size = new Size(0x282, 0x6a);
            base.ttDefault.SetSuperTip(this.gpgLabelDescription, null);
            this.gpgLabelDescription.TabIndex = 0x10;
            this.gpgLabelDescription.Text = "<LOC>No history";
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            this.gpgPanelHistory.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelHistory.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelHistory.BorderThickness = 2;
            this.gpgPanelHistory.Controls.Add(this.splitContainer1);
            this.gpgPanelHistory.DrawBorder = true;
            this.gpgPanelHistory.Location = new Point(12, 0x6a);
            this.gpgPanelHistory.Name = "gpgPanelHistory";
            this.gpgPanelHistory.Size = new Size(0x288, 0x177);
            base.ttDefault.SetSuperTip(this.gpgPanelHistory, null);
            this.gpgPanelHistory.TabIndex = 0x11;
            this.splitContainer1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainer1.BackColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.splitContainer1.Location = new Point(2, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.BackColor = Color.Black;
            this.splitContainer1.Panel1.Controls.Add(this.gpgDataGridHistory);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel1, null);
            this.splitContainer1.Panel2.BackColor = Color.Black;
            this.splitContainer1.Panel2.Controls.Add(this.skinLabel1);
            this.splitContainer1.Panel2.Controls.Add(this.gpgLabelDescription);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel2, null);
            this.splitContainer1.Size = new Size(0x284, 0x171);
            this.splitContainer1.SplitterDistance = 0xee;
            this.splitContainer1.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainer1, null);
            this.splitContainer1.TabIndex = 0x12;
            this.skinLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Black;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(0, 0);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0x285, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 0x11;
            this.skinLabel1.Text = "<LOC>Details";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(4, 0, 0, 0);
            this.gpgPanel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanel2.BackgroundImage = (Image) manager.GetObject("gpgPanel2.BackgroundImage");
            this.gpgPanel2.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel2.BorderThickness = 2;
            this.gpgPanel2.Controls.Add(this.pManualTabs);
            this.gpgPanel2.DrawBorder = false;
            this.gpgPanel2.Location = new Point(6, 0x3e);
            this.gpgPanel2.Name = "gpgPanel2";
            this.gpgPanel2.Size = new Size(660, 0x2d);
            base.ttDefault.SetSuperTip(this.gpgPanel2, null);
            this.gpgPanel2.TabIndex = 0x19;
            this.pManualTabs.BackColor = Color.Transparent;
            this.pManualTabs.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.pManualTabs.BorderThickness = 2;
            this.pManualTabs.Controls.Add(this.btnHistoryTab);
            this.pManualTabs.Controls.Add(this.btnCommentsTab);
            this.pManualTabs.DrawBorder = false;
            this.pManualTabs.Location = new Point(4, 0x17);
            this.pManualTabs.Name = "pManualTabs";
            this.pManualTabs.Size = new Size(0x102, 0x16);
            base.ttDefault.SetSuperTip(this.pManualTabs, null);
            this.pManualTabs.TabIndex = 0x11;
            this.btnHistoryTab.AutoStyle = true;
            this.btnHistoryTab.BackColor = Color.Black;
            this.btnHistoryTab.ButtonState = 0;
            this.btnHistoryTab.DialogResult = DialogResult.OK;
            this.btnHistoryTab.DisabledForecolor = Color.Gray;
            this.btnHistoryTab.DrawColor = Color.White;
            this.btnHistoryTab.DrawEdges = true;
            this.btnHistoryTab.FocusColor = Color.Yellow;
            this.btnHistoryTab.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnHistoryTab.ForeColor = Color.White;
            this.btnHistoryTab.HorizontalScalingMode = ScalingModes.Tile;
            this.btnHistoryTab.IsStyled = true;
            this.btnHistoryTab.Location = new Point(0, 0);
            this.btnHistoryTab.Name = "btnHistoryTab";
            this.btnHistoryTab.Size = new Size(0x84, 0x16);
            this.btnHistoryTab.SkinBasePath = @"Controls\Button\TabSmallActive";
            base.ttDefault.SetSuperTip(this.btnHistoryTab, null);
            this.btnHistoryTab.TabIndex = 0x16;
            this.btnHistoryTab.TabStop = true;
            this.btnHistoryTab.Text = "<LOC>Ladder History";
            this.btnHistoryTab.TextAlign = ContentAlignment.MiddleCenter;
            this.btnHistoryTab.TextPadding = new Padding(0);
            this.btnHistoryTab.Click += new EventHandler(this.btnHistoryTab_Click);
            this.btnCommentsTab.AutoStyle = true;
            this.btnCommentsTab.BackColor = Color.Black;
            this.btnCommentsTab.ButtonState = 0;
            this.btnCommentsTab.DialogResult = DialogResult.OK;
            this.btnCommentsTab.DisabledForecolor = Color.Gray;
            this.btnCommentsTab.DrawColor = Color.White;
            this.btnCommentsTab.DrawEdges = true;
            this.btnCommentsTab.FocusColor = Color.Yellow;
            this.btnCommentsTab.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCommentsTab.ForeColor = Color.White;
            this.btnCommentsTab.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCommentsTab.IsStyled = true;
            this.btnCommentsTab.Location = new Point(0x7b, 0);
            this.btnCommentsTab.Name = "btnCommentsTab";
            this.btnCommentsTab.Size = new Size(0x84, 0x16);
            this.btnCommentsTab.SkinBasePath = @"Controls\Button\TabSmall";
            base.ttDefault.SetSuperTip(this.btnCommentsTab, null);
            this.btnCommentsTab.TabIndex = 0x17;
            this.btnCommentsTab.TabStop = true;
            this.btnCommentsTab.Text = "<LOC>User Comments";
            this.btnCommentsTab.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCommentsTab.TextPadding = new Padding(0);
            this.btnCommentsTab.Click += new EventHandler(this.btnCommentsTab_Click);
            this.gpgPanelComments.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelComments.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelComments.BorderThickness = 2;
            this.gpgPanelComments.Controls.Add(this.splitContainer2);
            this.gpgPanelComments.DrawBorder = true;
            this.gpgPanelComments.Location = new Point(12, 0x6a);
            this.gpgPanelComments.Name = "gpgPanelComments";
            this.gpgPanelComments.Size = new Size(0x288, 0x177);
            base.ttDefault.SetSuperTip(this.gpgPanelComments, null);
            this.gpgPanelComments.TabIndex = 0x13;
            this.splitContainer2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainer2.BackColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.splitContainer2.Location = new Point(2, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = Orientation.Horizontal;
            this.splitContainer2.Panel1.BackColor = Color.Black;
            this.splitContainer2.Panel1.Controls.Add(this.dataGridComments);
            base.ttDefault.SetSuperTip(this.splitContainer2.Panel1, null);
            this.splitContainer2.Panel2.BackColor = Color.Black;
            this.splitContainer2.Panel2.Controls.Add(this.skinLabel2);
            this.splitContainer2.Panel2.Controls.Add(this.gpgLabelCommentDetails);
            base.ttDefault.SetSuperTip(this.splitContainer2.Panel2, null);
            this.splitContainer2.Size = new Size(0x284, 0x171);
            this.splitContainer2.SplitterDistance = 0xee;
            this.splitContainer2.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainer2, null);
            this.splitContainer2.TabIndex = 0x13;
            this.dataGridComments.CustomizeStyle = false;
            this.dataGridComments.Dock = DockStyle.Fill;
            this.dataGridComments.EmbeddedNavigator.Name = "";
            this.dataGridComments.Location = new Point(0, 0);
            this.dataGridComments.LookAndFeel.SkinName = "London Liquid Sky";
            this.dataGridComments.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dataGridComments.MainView = this.gvComments;
            this.dataGridComments.Name = "dataGridComments";
            this.dataGridComments.ShowOnlyPredefinedDetails = true;
            this.dataGridComments.Size = new Size(0x284, 0xee);
            this.dataGridComments.TabIndex = 0x10;
            this.dataGridComments.ViewCollection.AddRange(new BaseView[] { this.gvComments });
            this.gvComments.Appearance.Empty.BackColor = Color.Black;
            this.gvComments.Appearance.Empty.Options.UseBackColor = true;
            this.gvComments.Appearance.EvenRow.BackColor = Color.Black;
            this.gvComments.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvComments.Appearance.FocusedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvComments.Appearance.FocusedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvComments.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvComments.Appearance.FocusedRow.Options.UseFont = true;
            this.gvComments.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvComments.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvComments.Appearance.HideSelectionRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvComments.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvComments.Appearance.OddRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvComments.Appearance.OddRow.Options.UseBackColor = true;
            this.gvComments.Appearance.Preview.BackColor = Color.Black;
            this.gvComments.Appearance.Preview.Options.UseBackColor = true;
            this.gvComments.Appearance.Row.BackColor = Color.Black;
            this.gvComments.Appearance.Row.ForeColor = Color.White;
            this.gvComments.Appearance.Row.Options.UseBackColor = true;
            this.gvComments.Appearance.RowSeparator.BackColor = Color.Black;
            this.gvComments.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvComments.Appearance.SelectedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvComments.Appearance.SelectedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvComments.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvComments.Appearance.SelectedRow.Options.UseFont = true;
            this.gvComments.AppearancePrint.Row.ForeColor = Color.White;
            this.gvComments.AppearancePrint.Row.Options.UseForeColor = true;
            this.gvComments.BorderStyle = BorderStyles.NoBorder;
            this.gvComments.ColumnPanelRowHeight = 30;
            this.gvComments.Columns.AddRange(new GridColumn[] { this.gcComment_Date, this.gcComment_Name, this.gcComment_Details });
            this.gvComments.GridControl = this.dataGridComments;
            this.gvComments.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvComments.Name = "gvComments";
            this.gvComments.OptionsMenu.EnableColumnMenu = false;
            this.gvComments.OptionsMenu.EnableFooterMenu = false;
            this.gvComments.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvComments.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvComments.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvComments.OptionsView.EnableAppearanceEvenRow = true;
            this.gvComments.OptionsView.EnableAppearanceOddRow = true;
            this.gvComments.OptionsView.ShowHorzLines = false;
            this.gvComments.OptionsView.ShowIndicator = false;
            this.gvComments.OptionsView.ShowVertLines = false;
            this.gvComments.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gvComments_FocusedRowChanged);
            this.gcComment_Date.AppearanceCell.ForeColor = Color.White;
            this.gcComment_Date.AppearanceCell.Options.UseForeColor = true;
            this.gcComment_Date.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcComment_Date.AppearanceHeader.Options.UseFont = true;
            this.gcComment_Date.Caption = "<LOC>Comment Date";
            this.gcComment_Date.FieldName = "CommentDate";
            this.gcComment_Date.Name = "gcComment_Date";
            this.gcComment_Date.OptionsColumn.AllowEdit = false;
            this.gcComment_Date.Visible = true;
            this.gcComment_Date.VisibleIndex = 0;
            this.gcComment_Date.Width = 0x98;
            this.gcComment_Name.AppearanceCell.ForeColor = Color.White;
            this.gcComment_Name.AppearanceCell.Options.UseForeColor = true;
            this.gcComment_Name.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcComment_Name.AppearanceHeader.Options.UseFont = true;
            this.gcComment_Name.Caption = "<LOC>Commenter Name";
            this.gcComment_Name.FieldName = "CommenterName";
            this.gcComment_Name.Name = "gcComment_Name";
            this.gcComment_Name.OptionsColumn.AllowEdit = false;
            this.gcComment_Name.Visible = true;
            this.gcComment_Name.VisibleIndex = 1;
            this.gcComment_Name.Width = 160;
            this.gcComment_Details.AppearanceCell.ForeColor = Color.White;
            this.gcComment_Details.AppearanceCell.Options.UseForeColor = true;
            this.gcComment_Details.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcComment_Details.AppearanceHeader.Options.UseFont = true;
            this.gcComment_Details.Caption = "<LOC>Comment - Click to view full description below";
            this.gcComment_Details.FieldName = "Description";
            this.gcComment_Details.Name = "gcComment_Details";
            this.gcComment_Details.OptionsColumn.AllowEdit = false;
            this.gcComment_Details.Visible = true;
            this.gcComment_Details.VisibleIndex = 2;
            this.gcComment_Details.Width = 0x14c;
            this.skinLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Black;
            this.skinLabel2.DrawEdges = true;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(0, 0);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0x285, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 0x11;
            this.skinLabel2.Text = "<LOC>Details";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(4, 0, 0, 0);
            this.gpgLabelCommentDetails.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelCommentDetails.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelCommentDetails.AutoStyle = true;
            this.gpgLabelCommentDetails.BackColor = Color.Black;
            this.gpgLabelCommentDetails.Font = new Font("Arial", 9.75f);
            this.gpgLabelCommentDetails.ForeColor = Color.White;
            this.gpgLabelCommentDetails.IgnoreMouseWheel = false;
            this.gpgLabelCommentDetails.IsStyled = false;
            this.gpgLabelCommentDetails.Location = new Point(0, 0x17);
            this.gpgLabelCommentDetails.Name = "gpgLabelCommentDetails";
            this.gpgLabelCommentDetails.Padding = new Padding(6);
            this.gpgLabelCommentDetails.Size = new Size(0x282, 0x6a);
            base.ttDefault.SetSuperTip(this.gpgLabelCommentDetails, null);
            this.gpgLabelCommentDetails.TabIndex = 0x10;
            this.gpgLabelCommentDetails.Text = "<LOC>No Comments";
            this.gpgLabelCommentDetails.TextStyle = TextStyles.Default;
            this.gridColumn1.Caption = "<LOC>Report Date";
            this.gridColumn1.FieldName = "PlayerReportDateFull";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 0x84;
            this.gridColumn2.Caption = "<LOC>Rank Change";
            this.gridColumn2.FieldName = "RankChange";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 0x86;
            this.gridColumn3.AppearanceCell.ForeColor = Color.White;
            this.gridColumn3.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gridColumn3.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.Caption = "<LOC>Click to view full description below";
            this.gridColumn3.FieldName = "Description";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 0x17a;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2a0, 0x20c);
            base.Controls.Add(this.gpgPanelHistory);
            base.Controls.Add(this.gpgPanel2);
            base.Controls.Add(this.gpgPanelComments);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x1c4, 0x183);
            base.Name = "DlgLadderHistory";
            base.Opacity = 1.0;
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "";
            base.Controls.SetChildIndex(this.gpgPanelComments, 0);
            base.Controls.SetChildIndex(this.gpgPanel2, 0);
            base.Controls.SetChildIndex(this.gpgPanelHistory, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgDataGridHistory.EndInit();
            this.gvLadder.EndInit();
            this.gpgPanelHistory.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.gpgPanel2.ResumeLayout(false);
            this.pManualTabs.ResumeLayout(false);
            this.gpgPanelComments.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.dataGridComments.EndInit();
            this.gvComments.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LayoutTabs()
        {
            Control[] controlArray = new Control[] { this.btnHistoryTab, this.btnCommentsTab };
            foreach (Control control in controlArray)
            {
                control.BringToFront();
            }
            this.SelectedTab.BringToFront();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.btnHistoryTab.DrawColor = Color.Black;
            this.btnHistoryTab.MakeEdgesTransparent();
            this.btnCommentsTab.MakeEdgesTransparent();
        }

        public LadderParticipant Participant
        {
            get
            {
                return this.mParticipant;
            }
        }

        public LadderParticipantComment SelectedComment
        {
            get
            {
                return this.mSelectedComment;
            }
        }

        public LadderGameResult SelectedGameResult
        {
            get
            {
                return this.mSelectedGameResult;
            }
        }

        public SkinButton SelectedTab
        {
            get
            {
                if (this.mSelectedTab == null)
                {
                    this.mSelectedTab = this.btnHistoryTab;
                }
                return this.mSelectedTab;
            }
        }
    }
}

