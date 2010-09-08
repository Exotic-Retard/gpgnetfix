namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using DevExpress.Data;
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgJoinGame : DlgBase
    {
        private GridColumn colDescription;
        private GridColumn colHasPassword;
        private GridColumn colHostName;
        private GridColumn colPlayerCount;
        private GridColumn colPlayerLevels;
        private IContainer components = null;
        private GPGChatGrid gpgGameGrid;
        private GridView gvGameView;
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private RepositoryItemCheckEdit riHasPassword;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonJoin;
        private SkinButton skinButtonRefresh;

        public DlgJoinGame()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgGameGrid_DoubleClick(object sender, EventArgs e)
        {
            this.JoinGame();
        }

        private void gvGameView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.skinButtonJoin.Enabled = this.SelectedGame != null;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgJoinGame));
            this.skinButtonJoin = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonRefresh = new SkinButton();
            this.gpgGameGrid = new GPGChatGrid();
            this.gvGameView = new GridView();
            this.colHasPassword = new GridColumn();
            this.riHasPassword = new RepositoryItemCheckEdit();
            this.colDescription = new GridColumn();
            this.colHostName = new GridColumn();
            this.colPlayerCount = new GridColumn();
            this.colPlayerLevels = new GridColumn();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgGameGrid.BeginInit();
            this.gvGameView.BeginInit();
            this.riHasPassword.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.repositoryItemPictureEdit1.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x264, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.skinButtonJoin.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonJoin.AutoStyle = true;
            this.skinButtonJoin.BackColor = Color.Black;
            this.skinButtonJoin.ButtonState = 0;
            this.skinButtonJoin.DialogResult = DialogResult.OK;
            this.skinButtonJoin.DisabledForecolor = Color.Gray;
            this.skinButtonJoin.DrawColor = Color.White;
            this.skinButtonJoin.DrawEdges = true;
            this.skinButtonJoin.Enabled = false;
            this.skinButtonJoin.FocusColor = Color.Yellow;
            this.skinButtonJoin.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonJoin.ForeColor = Color.White;
            this.skinButtonJoin.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonJoin.IsStyled = true;
            this.skinButtonJoin.Location = new Point(0x150, 430);
            this.skinButtonJoin.Name = "skinButtonJoin";
            this.skinButtonJoin.Size = new Size(0x68, 0x17);
            this.skinButtonJoin.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonJoin, null);
            this.skinButtonJoin.TabIndex = 0x23;
            this.skinButtonJoin.TabStop = true;
            this.skinButtonJoin.Text = "<LOC>Join Game";
            this.skinButtonJoin.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonJoin.TextPadding = new Padding(0);
            this.skinButtonJoin.Click += new EventHandler(this.skinButtonJoin_Click);
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x22c, 430);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x68, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 0x22;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonRefresh.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonRefresh.AutoStyle = true;
            this.skinButtonRefresh.BackColor = Color.Black;
            this.skinButtonRefresh.ButtonState = 0;
            this.skinButtonRefresh.DialogResult = DialogResult.OK;
            this.skinButtonRefresh.DisabledForecolor = Color.Gray;
            this.skinButtonRefresh.DrawColor = Color.White;
            this.skinButtonRefresh.DrawEdges = true;
            this.skinButtonRefresh.FocusColor = Color.Yellow;
            this.skinButtonRefresh.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRefresh.ForeColor = Color.White;
            this.skinButtonRefresh.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRefresh.IsStyled = true;
            this.skinButtonRefresh.Location = new Point(0x1be, 430);
            this.skinButtonRefresh.Name = "skinButtonRefresh";
            this.skinButtonRefresh.Size = new Size(0x68, 0x17);
            this.skinButtonRefresh.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRefresh, null);
            this.skinButtonRefresh.TabIndex = 0x21;
            this.skinButtonRefresh.TabStop = true;
            this.skinButtonRefresh.Text = "<LOC>Refresh List";
            this.skinButtonRefresh.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRefresh.TextPadding = new Padding(0);
            this.skinButtonRefresh.Click += new EventHandler(this.skinButtonRefresh_Click);
            this.gpgGameGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGameGrid.CustomizeStyle = false;
            this.gpgGameGrid.EmbeddedNavigator.Name = "";
            this.gpgGameGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgGameGrid.IgnoreMouseWheel = false;
            this.gpgGameGrid.Location = new Point(12, 0x53);
            this.gpgGameGrid.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgGameGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgGameGrid.MainView = this.gvGameView;
            this.gpgGameGrid.Name = "gpgGameGrid";
            this.gpgGameGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3, this.riHasPassword, this.repositoryItemPictureEdit1 });
            this.gpgGameGrid.ShowOnlyPredefinedDetails = true;
            this.gpgGameGrid.Size = new Size(0x287, 0x155);
            this.gpgGameGrid.TabIndex = 0x20;
            this.gpgGameGrid.ViewCollection.AddRange(new BaseView[] { this.gvGameView });
            this.gpgGameGrid.DoubleClick += new EventHandler(this.gpgGameGrid_DoubleClick);
            this.gvGameView.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvGameView.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvGameView.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvGameView.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvGameView.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvGameView.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvGameView.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvGameView.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvGameView.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvGameView.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvGameView.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvGameView.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvGameView.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvGameView.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvGameView.Appearance.Empty.BackColor = Color.Black;
            this.gvGameView.Appearance.Empty.Options.UseBackColor = true;
            this.gvGameView.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvGameView.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvGameView.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvGameView.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvGameView.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvGameView.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvGameView.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvGameView.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvGameView.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvGameView.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvGameView.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvGameView.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvGameView.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvGameView.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvGameView.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvGameView.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvGameView.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvGameView.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvGameView.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvGameView.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvGameView.Appearance.FocusedCell.Options.UseFont = true;
            this.gvGameView.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvGameView.Appearance.FocusedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvGameView.Appearance.FocusedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvGameView.Appearance.FocusedRow.Font = new Font("Arial", 9.75f);
            this.gvGameView.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvGameView.Appearance.FocusedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvGameView.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvGameView.Appearance.FocusedRow.Options.UseFont = true;
            this.gvGameView.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvGameView.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvGameView.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvGameView.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvGameView.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvGameView.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvGameView.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvGameView.Appearance.FooterPanel.Options.UseFont = true;
            this.gvGameView.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvGameView.Appearance.GroupButton.BackColor = Color.Black;
            this.gvGameView.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvGameView.Appearance.GroupButton.ForeColor = Color.White;
            this.gvGameView.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvGameView.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvGameView.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvGameView.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvGameView.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvGameView.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvGameView.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvGameView.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvGameView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvGameView.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvGameView.Appearance.GroupPanel.BackColor2 = Color.Black;
            this.gvGameView.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvGameView.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvGameView.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvGameView.Appearance.GroupPanel.Options.UseFont = true;
            this.gvGameView.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvGameView.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvGameView.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvGameView.Appearance.GroupRow.ForeColor = Color.White;
            this.gvGameView.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvGameView.Appearance.GroupRow.Options.UseFont = true;
            this.gvGameView.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvGameView.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvGameView.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvGameView.Appearance.HeaderPanel.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gvGameView.Appearance.HeaderPanel.ForeColor = Color.Black;
            this.gvGameView.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvGameView.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvGameView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvGameView.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvGameView.Appearance.HideSelectionRow.BackColor = Color.Gray;
            this.gvGameView.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvGameView.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvGameView.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvGameView.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvGameView.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvGameView.Appearance.HorzLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvGameView.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvGameView.Appearance.Preview.BackColor = Color.White;
            this.gvGameView.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvGameView.Appearance.Preview.ForeColor = Color.Purple;
            this.gvGameView.Appearance.Preview.Options.UseBackColor = true;
            this.gvGameView.Appearance.Preview.Options.UseFont = true;
            this.gvGameView.Appearance.Preview.Options.UseForeColor = true;
            this.gvGameView.Appearance.Row.BackColor = Color.Black;
            this.gvGameView.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xb2);
            this.gvGameView.Appearance.Row.ForeColor = Color.White;
            this.gvGameView.Appearance.Row.Options.UseBackColor = true;
            this.gvGameView.Appearance.Row.Options.UseFont = true;
            this.gvGameView.Appearance.Row.Options.UseForeColor = true;
            this.gvGameView.Appearance.RowSeparator.BackColor = Color.White;
            this.gvGameView.Appearance.RowSeparator.BackColor2 = Color.White;
            this.gvGameView.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvGameView.Appearance.SelectedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvGameView.Appearance.SelectedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvGameView.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvGameView.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvGameView.Appearance.SelectedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvGameView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvGameView.Appearance.SelectedRow.Options.UseFont = true;
            this.gvGameView.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvGameView.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvGameView.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvGameView.Appearance.TopNewRow.Options.UseFont = true;
            this.gvGameView.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvGameView.Appearance.VertLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvGameView.Appearance.VertLine.Options.UseBackColor = true;
            this.gvGameView.BorderStyle = BorderStyles.NoBorder;
            this.gvGameView.Columns.AddRange(new GridColumn[] { this.colHasPassword, this.colDescription, this.colHostName, this.colPlayerCount, this.colPlayerLevels });
            this.gvGameView.GridControl = this.gpgGameGrid;
            this.gvGameView.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvGameView.Name = "gvGameView";
            this.gvGameView.OptionsBehavior.AutoPopulateColumns = false;
            this.gvGameView.OptionsCustomization.AllowRowSizing = true;
            this.gvGameView.OptionsDetail.AllowZoomDetail = false;
            this.gvGameView.OptionsDetail.EnableMasterViewMode = false;
            this.gvGameView.OptionsDetail.ShowDetailTabs = false;
            this.gvGameView.OptionsDetail.SmartDetailExpand = false;
            this.gvGameView.OptionsMenu.EnableColumnMenu = false;
            this.gvGameView.OptionsMenu.EnableFooterMenu = false;
            this.gvGameView.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvGameView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvGameView.OptionsSelection.MultiSelect = true;
            this.gvGameView.OptionsView.RowAutoHeight = true;
            this.gvGameView.RowHeight = 50;
            this.gvGameView.SelectionChanged += new SelectionChangedEventHandler(this.gvGameView_SelectionChanged);
            this.colHasPassword.ColumnEdit = this.riHasPassword;
            this.colHasPassword.FieldName = "HasPassword";
            this.colHasPassword.MinWidth = 0x19;
            this.colHasPassword.Name = "colHasPassword";
            this.colHasPassword.OptionsColumn.AllowSize = false;
            this.colHasPassword.OptionsColumn.FixedWidth = true;
            this.colHasPassword.OptionsColumn.ReadOnly = true;
            this.colHasPassword.OptionsColumn.ShowCaption = false;
            this.colHasPassword.Visible = true;
            this.colHasPassword.VisibleIndex = 0;
            this.colHasPassword.Width = 0x19;
            this.riHasPassword.AutoHeight = false;
            this.riHasPassword.CheckStyle = CheckStyles.UserDefined;
            this.riHasPassword.Name = "riHasPassword";
            this.riHasPassword.PictureChecked = (Image) manager.GetObject("riHasPassword.PictureChecked");
            this.riHasPassword.PictureGrayed = (Image) manager.GetObject("riHasPassword.PictureGrayed");
            this.riHasPassword.PictureUnchecked = (Image) manager.GetObject("riHasPassword.PictureUnchecked");
            this.colDescription.Caption = "<LOC>Game Name";
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 1;
            this.colDescription.Width = 0x9a;
            this.colHostName.Caption = "<LOC>Host Name";
            this.colHostName.FieldName = "HostName";
            this.colHostName.Name = "colHostName";
            this.colHostName.OptionsColumn.AllowEdit = false;
            this.colHostName.Visible = true;
            this.colHostName.VisibleIndex = 2;
            this.colHostName.Width = 0x68;
            this.colPlayerCount.Caption = "<LOC>Player Count";
            this.colPlayerCount.FieldName = "PlayerCount";
            this.colPlayerCount.Name = "colPlayerCount";
            this.colPlayerCount.OptionsColumn.AllowEdit = false;
            this.colPlayerCount.Visible = true;
            this.colPlayerCount.VisibleIndex = 3;
            this.colPlayerCount.Width = 0x68;
            this.colPlayerLevels.Caption = "<LOC>Avg. Player Level";
            this.colPlayerLevels.FieldName = "PlayerLevels";
            this.colPlayerLevels.Name = "colPlayerLevels";
            this.colPlayerLevels.Visible = true;
            this.colPlayerLevels.VisibleIndex = 4;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
            this.repositoryItemPictureEdit1.CustomHeight = 0x18;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.ReadOnly = true;
            this.repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
            base.AcceptButton = this.skinButtonJoin;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x29f, 0x204);
            base.Controls.Add(this.skinButtonJoin);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonRefresh);
            base.Controls.Add(this.gpgGameGrid);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgJoinGame";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Available Games";
            base.Controls.SetChildIndex(this.gpgGameGrid, 0);
            base.Controls.SetChildIndex(this.skinButtonRefresh, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonJoin, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgGameGrid.EndInit();
            this.gvGameView.EndInit();
            this.riHasPassword.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.rimMemoEdit3.EndInit();
            this.repositoryItemPictureEdit1.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void JoinGame()
        {
            if (this.SelectedGame != null)
            {
                bool flag = false;
                MappedObjectList<HostedGame> objects = new QuazalQuery("GetHostedSpaceSiegeGames", new object[0]).GetObjects<HostedGame>();
                foreach (HostedGame game in objects)
                {
                    if (game.ID == this.SelectedGame.ID)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    DlgMessage.ShowDialog(Loc.Get("<LOC>The game has been started and you can no longer join."), Loc.Get("<LOC>Warning"));
                }
                else if (this.SelectedGame.Password != "")
                {
                    DialogResult result;
                    if (this.SelectedGame.Password == DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>This game is password protected, please enter the password below."), Loc.Get("<LOC>Password"), true, out result))
                    {
                        base.MainForm.AcceptGameInvite(this.SelectedGame.HostName);
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                    else if (result != DialogResult.Cancel)
                    {
                        DlgMessage.ShowDialog(Loc.Get("<LOC>Incorrect password. Please try again."));
                    }
                }
                else
                {
                    base.MainForm.AcceptGameInvite(this.SelectedGame.HostName);
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (base.DialogResult != DialogResult.OK)
            {
                base.MainForm.EnableGameButtons();
                base.MainForm.RefreshPMWindows();
            }
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RefreshList();
        }

        private void RefreshList()
        {
            this.skinButtonRefresh.Enabled = false;
            this.skinButtonJoin.Enabled = false;
            this.gpgGameGrid.DataSource = null;
            this.gpgGameGrid.RefreshDataSource();
            ThreadQueue.QueueUserWorkItem(delegate (object state) {
                MappedObjectList<HostedGame> data = new QuazalQuery("GetHostedSpaceSiegeGames", new object[0]).GetObjects<HostedGame>();
                base.BeginInvoke((VGen0)delegate {
                    this.gpgGameGrid.DataSource = data;
                    this.skinButtonRefresh.Enabled = true;
                    if (data.Count > 0)
                    {
                        this.skinButtonJoin.Enabled = true;
                    }
                });
            }, new object[0]);
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonJoin_Click(object sender, EventArgs e)
        {
            this.JoinGame();
        }

        private void skinButtonRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshList();
        }

        public HostedGame SelectedGame
        {
            get
            {
                try
                {
                    int[] selectedRows = this.gvGameView.GetSelectedRows();
                    if (selectedRows.Length > 0)
                    {
                        return (this.gvGameView.GetRow(selectedRows[0]) as HostedGame);
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
    }
}

