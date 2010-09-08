namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DlgSelectGame : DlgBase
    {
        private GridColumn colDescription;
        private GridColumn colHasPassword;
        private GridColumn colMapImage;
        private GridColumn colMapName;
        private GridColumn colPlayerCount;
        private GridColumn colPlayerName;
        private GridColumn colSize;
        private GridColumn colStartTime;
        private IContainer components;
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
        private static Hashtable sVaultCheckHash = new Hashtable();

        public event EventHandler JoinGame;

        public DlgSelectGame(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            Loc.LocObject(this);
        }

        private void CheckVaultMapFinish(DataList data)
        {
            if (data.Count > 0)
            {
                try
                {
                    DlgContentDetailsView view = DlgContentDetailsView.CreateOrGetExisting(Convert.ToInt32(data[0]["content_id"]));
                    view.OnDownloadComplete += new EventHandler(this.dlg_OnDownloadComplete);
                    view.Show();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    DlgMessage.ShowDialog(Loc.Get("<LOC>Unable to join this game.  You do not have this map, and it cannot be located in the vault."));
                }
                this.skinButtonRefresh.Enabled = true;
                this.skinButtonJoin.Enabled = true;
            }
            else if (ConfigSettings.GetBool("JoinGameAnyway", false))
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "FinishPopVerify", new object[] { "GetGatheringPopulation", new object[] { this.GameName } });
            }
            else
            {
                DlgMessage.ShowDialog(Loc.Get("<LOC>Unable to join this game.  You do not have this map, and it cannot be located in the vault."));
                this.skinButtonRefresh.Enabled = true;
                this.skinButtonJoin.Enabled = true;
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

        private void dlg_OnDownloadComplete(object sender, EventArgs e)
        {
            try
            {
                GameItem.MapNameLookup.Clear();
                this.RefreshList();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void FinishList(object result)
        {
            MappedObjectList<GameItem> list = result as MappedObjectList<GameItem>;
            MappedObjectList<GameItem> list2 = new MappedObjectList<GameItem>();
            foreach (GameItem item in list)
            {
                if (item.Description.IndexOf("AUTOMATCH") != 0)
                {
                    list2.Add(item);
                }
            }
            this.gpgGameGrid.DataSource = list2;
            this.skinButtonRefresh.Enabled = true;
            if (list.Count > 0)
            {
                this.skinButtonJoin.Enabled = true;
            }
        }

        private void FinishPopVerify(DataList data)
        {
            try
            {
                if (data.Count == 0)
                {
                    this.RefreshList();
                    DlgMessage.ShowDialog(Loc.Get("<LOC>The game has been started and you can no longer join."), Loc.Get("<LOC>Warning"));
                }
                else
                {
                    if (Convert.ToInt32(data[0]["count"]) > 8)
                    {
                        throw new Exception("The game is full.");
                    }
                    if (this.JoinGame != null)
                    {
                        this.JoinGame(this, EventArgs.Empty);
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                this.RefreshList();
                DlgMessage.ShowDialog(Loc.Get("<LOC>This game is now full."), Loc.Get("<LOC>Warning"));
            }
            this.skinButtonRefresh.Enabled = true;
            this.skinButtonJoin.Enabled = true;
        }

        private void gpgGameGrid_DoubleClick(object sender, EventArgs e)
        {
        }

        private void gvGameView_DoubleClick(object sender, EventArgs e)
        {
            if (this.gvGameView.GetSelectedRows().Length > 0)
            {
                this.OnSelectGame();
            }
        }

        private void gvGameView_StartSorting(object sender, EventArgs e)
        {
            try
            {
                this.gvGameView.ClearSelection();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgSelectGame));
            this.gpgGameGrid = new GPGChatGrid();
            this.gvGameView = new GridView();
            this.colHasPassword = new GridColumn();
            this.riHasPassword = new RepositoryItemCheckEdit();
            this.colDescription = new GridColumn();
            this.colPlayerName = new GridColumn();
            this.colMapImage = new GridColumn();
            this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
            this.colMapName = new GridColumn();
            this.colPlayerCount = new GridColumn();
            this.colStartTime = new GridColumn();
            this.colSize = new GridColumn();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.skinButtonRefresh = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonJoin = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgGameGrid.BeginInit();
            this.gvGameView.BeginInit();
            this.riHasPassword.BeginInit();
            this.repositoryItemPictureEdit1.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.rimMemoEdit3.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x2b8, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgGameGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGameGrid.CustomizeStyle = false;
            this.gpgGameGrid.EmbeddedNavigator.Name = "";
            this.gpgGameGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgGameGrid.IgnoreMouseWheel = false;
            this.gpgGameGrid.Location = new Point(12, 0x4c);
            this.gpgGameGrid.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgGameGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgGameGrid.MainView = this.gvGameView;
            this.gpgGameGrid.Name = "gpgGameGrid";
            this.gpgGameGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3, this.riHasPassword, this.repositoryItemPictureEdit1 });
            this.gpgGameGrid.ShowOnlyPredefinedDetails = true;
            this.gpgGameGrid.Size = new Size(0x2db, 0x138);
            this.gpgGameGrid.TabIndex = 0x1b;
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
            this.gvGameView.Columns.AddRange(new GridColumn[] { this.colHasPassword, this.colDescription, this.colPlayerName, this.colMapImage, this.colMapName, this.colPlayerCount, this.colStartTime, this.colSize });
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
            this.gvGameView.StartSorting += new EventHandler(this.gvGameView_StartSorting);
            this.gvGameView.DoubleClick += new EventHandler(this.gvGameView_DoubleClick);
            this.colHasPassword.ColumnEdit = this.riHasPassword;
            this.colHasPassword.FieldName = "HasPassword";
            this.colHasPassword.MinWidth = 0x19;
            this.colHasPassword.Name = "colHasPassword";
            this.colHasPassword.OptionsColumn.AllowSize = false;
            this.colHasPassword.OptionsColumn.FixedWidth = true;
            this.colHasPassword.OptionsColumn.ReadOnly = true;
            this.colHasPassword.OptionsColumn.ShowCaption = false;
            this.colHasPassword.Visible = true;
            this.colHasPassword.VisibleIndex = 1;
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
            this.colDescription.VisibleIndex = 2;
            this.colDescription.Width = 0x9a;
            this.colPlayerName.Caption = "<LOC>Player Name";
            this.colPlayerName.FieldName = "PlayerName";
            this.colPlayerName.Name = "colPlayerName";
            this.colPlayerName.OptionsColumn.AllowEdit = false;
            this.colPlayerName.Visible = true;
            this.colPlayerName.VisibleIndex = 3;
            this.colPlayerName.Width = 0x68;
            this.colMapImage.ColumnEdit = this.repositoryItemPictureEdit1;
            this.colMapImage.FieldName = "MapImage";
            this.colMapImage.MinWidth = 50;
            this.colMapImage.Name = "colMapImage";
            this.colMapImage.Visible = true;
            this.colMapImage.VisibleIndex = 0;
            this.colMapImage.Width = 50;
            this.repositoryItemPictureEdit1.CustomHeight = 0x18;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.ReadOnly = true;
            this.repositoryItemPictureEdit1.SizeMode = PictureSizeMode.Zoom;
            this.colMapName.Caption = "<LOC>Map Name";
            this.colMapName.FieldName = "MapName";
            this.colMapName.Name = "colMapName";
            this.colMapName.OptionsColumn.AllowEdit = false;
            this.colMapName.Visible = true;
            this.colMapName.VisibleIndex = 4;
            this.colMapName.Width = 0x60;
            this.colPlayerCount.Caption = "<LOC>Max Players";
            this.colPlayerCount.FieldName = "NumPlayers";
            this.colPlayerCount.Name = "colPlayerCount";
            this.colPlayerCount.OptionsColumn.AllowEdit = false;
            this.colPlayerCount.Visible = true;
            this.colPlayerCount.VisibleIndex = 5;
            this.colPlayerCount.Width = 0x68;
            this.colStartTime.Caption = "<LOC>Start Time";
            this.colStartTime.FieldName = "StartTime";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.OptionsColumn.AllowEdit = false;
            this.colStartTime.OptionsColumn.ReadOnly = true;
            this.colStartTime.Visible = true;
            this.colStartTime.VisibleIndex = 7;
            this.colStartTime.Width = 0x63;
            this.colSize.Caption = "<LOC>Size";
            this.colSize.FieldName = "Size";
            this.colSize.Name = "colSize";
            this.colSize.OptionsColumn.AllowEdit = false;
            this.colSize.Visible = true;
            this.colSize.VisibleIndex = 6;
            this.colSize.Width = 80;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
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
            this.skinButtonRefresh.Location = new Point(0x211, 0x18a);
            this.skinButtonRefresh.Name = "skinButtonRefresh";
            this.skinButtonRefresh.Size = new Size(0x68, 0x17);
            this.skinButtonRefresh.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRefresh, null);
            this.skinButtonRefresh.TabIndex = 0x1d;
            this.skinButtonRefresh.TabStop = true;
            this.skinButtonRefresh.Text = "<LOC>Refresh List";
            this.skinButtonRefresh.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRefresh.TextPadding = new Padding(0);
            this.skinButtonRefresh.Click += new EventHandler(this.skinButtonRefresh_Click);
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
            this.skinButtonCancel.Location = new Point(0x27f, 0x18a);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x68, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 30;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
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
            this.skinButtonJoin.Location = new Point(0x1a3, 0x18a);
            this.skinButtonJoin.Name = "skinButtonJoin";
            this.skinButtonJoin.Size = new Size(0x68, 0x17);
            this.skinButtonJoin.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonJoin, null);
            this.skinButtonJoin.TabIndex = 0x1f;
            this.skinButtonJoin.TabStop = true;
            this.skinButtonJoin.Text = "<LOC>Join Game";
            this.skinButtonJoin.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonJoin.TextPadding = new Padding(0);
            this.skinButtonJoin.Click += new EventHandler(this.skinButtonJoin_Click);
            base.AcceptButton = this.skinButtonJoin;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x2f3, 480);
            base.Controls.Add(this.skinButtonJoin);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonRefresh);
            base.Controls.Add(this.gpgGameGrid);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x1d2, 0x177);
            base.Name = "DlgSelectGame";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Join Game";
            base.Controls.SetChildIndex(this.gpgGameGrid, 0);
            base.Controls.SetChildIndex(this.skinButtonRefresh, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonJoin, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgGameGrid.EndInit();
            this.gvGameView.EndInit();
            this.riHasPassword.EndInit();
            this.repositoryItemPictureEdit1.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.rimMemoEdit3.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
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

        private void OnSelectGame()
        {
            if ((this.GameName != null) && (this.GameName.Length > 0))
            {
                if (this.Password != "")
                {
                    if (this.Password == DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>What is the password?"), true))
                    {
                        this.VerifyPopulation();
                    }
                    else
                    {
                        DlgMessage.ShowDialog(Loc.Get("<LOC>Incorrect password. Please try again."));
                    }
                }
                else
                {
                    this.VerifyPopulation();
                }
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (base.Visible)
            {
                this.RefreshList();
            }
        }

        private void RefreshList()
        {
            this.skinButtonRefresh.Enabled = false;
            this.skinButtonJoin.Enabled = false;
            ThreadQueue.Quazal.Enqueue(typeof(Game), "GetGameList", this, "FinishList", new object[0]);
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonJoin_Click(object sender, EventArgs e)
        {
            this.OnSelectGame();
        }

        private void skinButtonRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshList();
        }

        private void VerifyPopulation()
        {
            this.skinButtonRefresh.Enabled = false;
            this.skinButtonJoin.Enabled = false;
            if (((GameInformation.SelectedGame.GameID == 2) || (GameInformation.SelectedGame.GameID == 0x11)) && (((this.SelectedGame != null) && this.SelectedGame.NeedsDownload) && !sVaultCheckHash.ContainsKey(this.SelectedGame.MapName)))
            {
                string mapName = "";
                string str2 = "";
                if (this.SelectedGame.MapName.IndexOf(".") >= 0)
                {
                    string[] strArray = this.SelectedGame.MapName.Split(new char[] { '.' });
                    mapName = strArray[0];
                    for (str2 = strArray[1].ToUpper().Replace("V", ""); str2.IndexOf("0") == 0; str2 = str2.Substring(1))
                    {
                    }
                }
                else
                {
                    mapName = this.SelectedGame.MapName;
                    str2 = this.SelectedGame.Version.ToString();
                }
                sVaultCheckHash[this.SelectedGame.MapName] = true;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "CheckVaultMapFinish", new object[] { "Get Vault Map By Name", new object[] { mapName, str2 } });
            }
            else
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "FinishPopVerify", new object[] { "GetGatheringPopulation", new object[] { this.GameName } });
            }
        }

        public string GameName
        {
            get
            {
                try
                {
                    int[] selectedRows = this.gvGameView.GetSelectedRows();
                    if (selectedRows.Length > 0)
                    {
                        return (this.gvGameView.GetRow(selectedRows[0]) as GameItem).Description;
                    }
                    return "";
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return "";
                }
            }
        }

        public string HostName
        {
            get
            {
                int[] selectedRows = this.gvGameView.GetSelectedRows();
                if (selectedRows.Length > 0)
                {
                    return (this.gvGameView.GetRow(selectedRows[0]) as GameItem).PlayerName;
                }
                return "";
            }
        }

        public string Password
        {
            get
            {
                try
                {
                    int[] selectedRows = this.gvGameView.GetSelectedRows();
                    if (selectedRows.Length > 0)
                    {
                        return (this.gvGameView.GetRow(selectedRows[0]) as GameItem).Password;
                    }
                    return "";
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return "";
                }
            }
        }

        public GameItem SelectedGame
        {
            get
            {
                try
                {
                    int[] selectedRows = this.gvGameView.GetSelectedRows();
                    if (selectedRows.Length > 0)
                    {
                        return (this.gvGameView.GetRow(selectedRows[0]) as GameItem);
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

        public string URL
        {
            get
            {
                int[] selectedRows = this.gvGameView.GetSelectedRows();
                if (selectedRows.Length > 0)
                {
                    return (this.gvGameView.GetRow(selectedRows[0]) as GameItem).URL;
                }
                return "";
            }
        }
    }
}

