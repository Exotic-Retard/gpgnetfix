namespace GPG.Multiplayer.Client.Games
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;

    public class FrmStatsLadder : DlgBase
    {
        private SkinLabel backLabelTitle;
        private MenuItem ciChat_ViewPlayer;
        private MenuItem ciChat_WebStats;
        private MenuItem ciChat_WhisperPlayer;
        private MenuItem ciClanProfile;
        private MenuItem ciExport;
        private MenuItem ciExport_HTML;
        private MenuItem ciExport_PDF;
        private MenuItem ciExport_Text;
        private MenuItem ciExport_XLS;
        private MenuItem ciSplitter1;
        private IContainer components;
        private GridColumn gcAverageRating;
        private GridColumn gcDraws;
        private GridColumn gcLosses;
        private GridColumn gcName;
        private GridColumn gcPeople;
        private GridColumn gcRank;
        private GridColumn gcRating;
        private GridColumn gcTopTenRating;
        private GridColumn gcWinPercentage;
        private GridColumn gcWins;
        private GPGContextMenu gpgContextMenuChat;
        private GPGDataGrid gpgDataGridLadder;
        private GPGLabel gpgLabel1;
        private GPGPanel gpgPanelLinkPages;
        private GPGTextBox gpgTextBoxSearchName;
        private GridView gvLadder;
        private string mCategory;
        private MappedObjectList<PlayerRating> mCurrentDataSet;
        private int mCurrentPage;
        private MenuItem miReplays;
        private int mLastRating;
        private PlayerRating mSelectedRating;
        private const int PAGE_LINK_SIZE = 60;
        internal static readonly int PAGE_SIZE = ConfigSettings.GetInt("rankings_page_size", 50);
        private SkinButton skinButtonEnd;
        private SkinButton skinButtonLast;
        private SkinButton skinButtonNext;
        private SkinButton skinButtonSearch;
        private SkinButton skinButtonStart;

        public FrmStatsLadder()
        {
            this.mSelectedRating = null;
            this.mLastRating = -1;
            this.mCategory = null;
            this.mCurrentPage = 0;
            this.mCurrentDataSet = null;
            this.components = null;
            this.InitializeComponent();
        }

        public FrmStatsLadder(FrmMain mainForm, string category) : base(mainForm)
        {
            this.mSelectedRating = null;
            this.mLastRating = -1;
            this.mCategory = null;
            this.mCurrentPage = 0;
            this.mCurrentDataSet = null;
            this.components = null;
            this.InitializeComponent();
            this.SetCategory(category, true);
        }

        private void _ChangePage(int page)
        {
            if (((page != this.CurrentPage) && (page >= 0)) && (page <= this.LastPage))
            {
                this.CurrentPage = page;
                this._RefreshData();
            }
        }

        private void _RefreshData()
        {
            if (this.mCategory.ToUpper() == "CLAN")
            {
                this.OnRefreshData(DataAccess.GetQueryData("GetRatingsListClan", new object[] { this.Category, (this.CurrentPage * PAGE_SIZE) + 1, (this.CurrentPage + 1) * PAGE_SIZE }));
            }
            else
            {
                this.OnRefreshData(DataAccess.GetQueryData("GetRatingsList", new object[] { this.Category, (this.CurrentPage * PAGE_SIZE) + 1, (this.CurrentPage + 1) * PAGE_SIZE }));
            }
        }

        private void ChangePage(int page)
        {
            if (((page != this.CurrentPage) && (page >= 0)) && (page <= this.LastPage))
            {
                this.CurrentPage = page;
                this.RefreshData();
            }
        }

        private void ciChat_ViewPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedRating != null)
            {
                base.MainForm.OnViewPlayerProfile(this.SelectedRating.PlayerName);
            }
        }

        private void ciChat_WebStats_Click(object sender, EventArgs e)
        {
            if (this.SelectedRating != null)
            {
                base.MainForm.ShowWebStats(this.SelectedRating.PlayerID);
            }
        }

        private void ciChat_WhisperPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedRating != null)
            {
                base.MainForm.OnSendWhisper(this.SelectedRating.PlayerName, null);
            }
        }

        private void ciClanProfile_Click(object sender, EventArgs e)
        {
            if (this.SelectedRating != null)
            {
                base.MainForm.OnViewClanProfileByName(this.SelectedRating.PlayerName);
            }
        }

        private void ciExport_HTML_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>HTML Document (*.htm;*.html)|*.htm;*.html");
            dialog.InitialDirectory = Program.Settings.SupcomPrefs.StatsDir;
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Ratings {0} - {1}, {2}-{3}-{4}", new object[] { (this.CurrentPage * PAGE_SIZE) + 1, (this.CurrentPage + 1) * PAGE_SIZE, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year });
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Program.Settings.SupcomPrefs.StatsDir = Path.GetDirectoryName(dialog.FileName);
                this.gpgDataGridLadder.ExportToHtml(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
            }
        }

        private void ciExport_PDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>PDF Document (*.pdf)|*.pdf");
            dialog.InitialDirectory = Program.Settings.SupcomPrefs.StatsDir;
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Ratings {0} - {1}, {2}-{3}-{4}", new object[] { (this.CurrentPage * PAGE_SIZE) + 1, (this.CurrentPage + 1) * PAGE_SIZE, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year });
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Program.Settings.SupcomPrefs.StatsDir = Path.GetDirectoryName(dialog.FileName);
                this.gpgDataGridLadder.ExportToPdf(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
            }
        }

        private void ciExport_Text_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>Text Document (*.txt)|*.txt");
            dialog.InitialDirectory = Program.Settings.SupcomPrefs.StatsDir;
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Ratings {0} - {1}, {2}-{3}-{4}", new object[] { (this.CurrentPage * PAGE_SIZE) + 1, (this.CurrentPage + 1) * PAGE_SIZE, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year });
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Program.Settings.SupcomPrefs.StatsDir = Path.GetDirectoryName(dialog.FileName);
                this.gpgDataGridLadder.ExportToText(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
            }
        }

        private void ciExport_XLS_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>Excel Spreadsheet (*.xls)|*.xls");
            dialog.InitialDirectory = Program.Settings.SupcomPrefs.StatsDir;
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Ratings {0} - {1}, {2}-{3}-{4}", new object[] { (this.CurrentPage * PAGE_SIZE) + 1, (this.CurrentPage + 1) * PAGE_SIZE, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year });
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Program.Settings.SupcomPrefs.StatsDir = Path.GetDirectoryName(dialog.FileName);
                this.gpgDataGridLadder.ExportToXls(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
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

        private void gpgTextBoxSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.skinButtonSearch_Click(sender, EventArgs.Empty);
            }
        }

        private void gvLadder_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
        }

        private void gvLadder_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.mCategory.ToUpper() == "CLAN")
                {
                    base.MainForm.OnViewClanProfileByName(this.SelectedRating.PlayerName);
                }
                else if (this.SelectedRating != null)
                {
                    foreach (string str in this.SelectedRating.PlayerName.Split(new char[] { ' ' }))
                    {
                        base.MainForm.OnViewPlayerProfile(str);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvLadder_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo info;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    (sender as GridView).Focus();
                    info = (sender as GridView).CalcHitInfo(e.Location);
                    if (info.InRow)
                    {
                        this.mSelectedRating = (sender as GridView).GetRow(info.RowHandle) as PlayerRating;
                    }
                    break;

                case MouseButtons.Right:
                    (sender as GridView).Focus();
                    info = (sender as GridView).CalcHitInfo(e.Location);
                    if (info.InRow)
                    {
                        this.mSelectedRating = (sender as GridView).GetRow(info.RowHandle) as PlayerRating;
                        int[] selectedRows = this.gvLadder.GetSelectedRows();
                        for (int i = 0; i < selectedRows.Length; i++)
                        {
                            this.gvLadder.UnselectRow(selectedRows[i]);
                        }
                        (sender as GridView).SelectRow(info.RowHandle);
                    }
                    break;
            }
            base.OnMouseDown(e);
        }

        private void gvLadder_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.gpgContextMenuChat.Show(this.gpgDataGridLadder, e.Location);
            }
            base.OnMouseUp(e);
        }

        private void InitializeComponent()
        {
            this.backLabelTitle = new SkinLabel();
            this.gpgDataGridLadder = new GPGDataGrid();
            this.gvLadder = new GridView();
            this.gcRank = new GridColumn();
            this.gcName = new GridColumn();
            this.gcWins = new GridColumn();
            this.gcLosses = new GridColumn();
            this.gcDraws = new GridColumn();
            this.gcWinPercentage = new GridColumn();
            this.gcRating = new GridColumn();
            this.gcAverageRating = new GridColumn();
            this.gcTopTenRating = new GridColumn();
            this.gcPeople = new GridColumn();
            this.skinButtonLast = new SkinButton();
            this.skinButtonNext = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxSearchName = new GPGTextBox();
            this.skinButtonSearch = new SkinButton();
            this.skinButtonStart = new SkinButton();
            this.skinButtonEnd = new SkinButton();
            this.gpgPanelLinkPages = new GPGPanel();
            this.gpgContextMenuChat = new GPGContextMenu();
            this.ciExport = new MenuItem();
            this.ciExport_PDF = new MenuItem();
            this.ciExport_HTML = new MenuItem();
            this.ciExport_Text = new MenuItem();
            this.ciExport_XLS = new MenuItem();
            this.ciSplitter1 = new MenuItem();
            this.ciChat_WhisperPlayer = new MenuItem();
            this.ciChat_WebStats = new MenuItem();
            this.ciChat_ViewPlayer = new MenuItem();
            this.ciClanProfile = new MenuItem();
            this.miReplays = new MenuItem();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgDataGridLadder.BeginInit();
            this.gvLadder.BeginInit();
            this.gpgTextBoxSearchName.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
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
            this.backLabelTitle.Size = new Size(0x272, 0x3b);
            this.backLabelTitle.SkinBasePath = @"Controls\Background Label\Ladders";
            base.ttDefault.SetSuperTip(this.backLabelTitle, null);
            this.backLabelTitle.TabIndex = 7;
            this.backLabelTitle.Text = "<LOC>Ladder Ratings";
            this.backLabelTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.backLabelTitle.TextPadding = new Padding(10, 0, 0, 0);
            this.gpgDataGridLadder.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgDataGridLadder.CustomizeStyle = false;
            this.gpgDataGridLadder.EmbeddedNavigator.Name = "";
            this.gpgDataGridLadder.Location = new Point(7, 0x7f);
            this.gpgDataGridLadder.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgDataGridLadder.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgDataGridLadder.MainView = this.gvLadder;
            this.gpgDataGridLadder.Name = "gpgDataGridLadder";
            this.gpgDataGridLadder.ShowOnlyPredefinedDetails = true;
            this.gpgDataGridLadder.Size = new Size(0x272, 0xfd);
            this.gpgDataGridLadder.TabIndex = 8;
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
            this.gvLadder.Columns.AddRange(new GridColumn[] { this.gcRank, this.gcName, this.gcWins, this.gcLosses, this.gcDraws, this.gcWinPercentage, this.gcRating, this.gcAverageRating, this.gcTopTenRating, this.gcPeople });
            this.gvLadder.GridControl = this.gpgDataGridLadder;
            this.gvLadder.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvLadder.Name = "gvLadder";
            this.gvLadder.OptionsMenu.EnableColumnMenu = false;
            this.gvLadder.OptionsMenu.EnableFooterMenu = false;
            this.gvLadder.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvLadder.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLadder.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvLadder.OptionsSelection.MultiSelect = true;
            this.gvLadder.OptionsView.EnableAppearanceEvenRow = true;
            this.gvLadder.OptionsView.EnableAppearanceOddRow = true;
            this.gvLadder.OptionsView.ShowHorzLines = false;
            this.gvLadder.OptionsView.ShowIndicator = false;
            this.gvLadder.OptionsView.ShowVertLines = false;
            this.gvLadder.CustomColumnDisplayText += new CustomColumnDisplayTextEventHandler(this.gvLadder_CustomColumnDisplayText);
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
            this.gcName.AppearanceCell.ForeColor = Color.White;
            this.gcName.AppearanceCell.Options.UseForeColor = true;
            this.gcName.AppearanceCell.Options.UseTextOptions = true;
            this.gcName.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcName.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcName.AppearanceHeader.Options.UseFont = true;
            this.gcName.Caption = "<LOC>Name";
            this.gcName.FieldName = "PlayerName";
            this.gcName.Name = "gcName";
            this.gcName.OptionsColumn.AllowEdit = false;
            this.gcName.Visible = true;
            this.gcName.VisibleIndex = 1;
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
            this.gcWins.VisibleIndex = 2;
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
            this.gcLosses.VisibleIndex = 3;
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
            this.gcDraws.VisibleIndex = 4;
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
            this.gcWinPercentage.VisibleIndex = 5;
            this.gcRating.AppearanceCell.ForeColor = Color.White;
            this.gcRating.AppearanceCell.Options.UseForeColor = true;
            this.gcRating.AppearanceCell.Options.UseTextOptions = true;
            this.gcRating.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcRating.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcRating.AppearanceHeader.Options.UseFont = true;
            this.gcRating.Caption = "<LOC>Rating";
            this.gcRating.FieldName = "Rating";
            this.gcRating.Name = "gcRating";
            this.gcRating.OptionsColumn.AllowEdit = false;
            this.gcRating.Visible = true;
            this.gcRating.VisibleIndex = 6;
            this.gcAverageRating.Caption = "<LOC>Average Rating";
            this.gcAverageRating.FieldName = "AvgRating";
            this.gcAverageRating.Name = "gcAverageRating";
            this.gcTopTenRating.Caption = "<LOC>Top Ten Rating";
            this.gcTopTenRating.FieldName = "TopTenRating";
            this.gcTopTenRating.Name = "gcTopTenRating";
            this.gcPeople.Caption = "<LOC># of People";
            this.gcPeople.FieldName = "People";
            this.gcPeople.Name = "gcPeople";
            this.skinButtonLast.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonLast.AutoStyle = true;
            this.skinButtonLast.BackColor = Color.Black;
            this.skinButtonLast.DialogResult = DialogResult.OK;
            this.skinButtonLast.DisabledForecolor = Color.Gray;
            this.skinButtonLast.DrawEdges = false;
            this.skinButtonLast.FocusColor = Color.Yellow;
            this.skinButtonLast.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLast.ForeColor = Color.White;
            this.skinButtonLast.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLast.IsStyled = true;
            this.skinButtonLast.Location = new Point(0x34, 0x181);
            this.skinButtonLast.Name = "skinButtonLast";
            this.skinButtonLast.Size = new Size(40, 0x16);
            this.skinButtonLast.SkinBasePath = @"Controls\Button\Previous";
            base.ttDefault.SetSuperTip(this.skinButtonLast, null);
            this.skinButtonLast.TabIndex = 10;
            this.skinButtonLast.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLast.TextPadding = new Padding(0);
            this.skinButtonLast.Click += new EventHandler(this.skinButtonLast_Click);
            this.skinButtonNext.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonNext.AutoStyle = true;
            this.skinButtonNext.BackColor = Color.Black;
            this.skinButtonNext.DialogResult = DialogResult.OK;
            this.skinButtonNext.DisabledForecolor = Color.Gray;
            this.skinButtonNext.DrawEdges = false;
            this.skinButtonNext.FocusColor = Color.Yellow;
            this.skinButtonNext.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNext.ForeColor = Color.White;
            this.skinButtonNext.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNext.IsStyled = true;
            this.skinButtonNext.Location = new Point(0x225, 0x181);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(40, 0x16);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Next";
            base.ttDefault.SetSuperTip(this.skinButtonNext, null);
            this.skinButtonNext.TabIndex = 11;
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.gpgLabel1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x1a1);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x72, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 12;
            this.gpgLabel1.Text = "<LOC>Search For";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxSearchName.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextBoxSearchName.Location = new Point(0x55, 0x19e);
            this.gpgTextBoxSearchName.Name = "gpgTextBoxSearchName";
            this.gpgTextBoxSearchName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxSearchName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxSearchName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxSearchName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxSearchName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxSearchName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxSearchName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxSearchName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxSearchName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxSearchName.Size = new Size(0xa8, 20);
            this.gpgTextBoxSearchName.TabIndex = 13;
            this.gpgTextBoxSearchName.KeyDown += new KeyEventHandler(this.gpgTextBoxSearchName_KeyDown);
            this.skinButtonSearch.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonSearch.AutoStyle = true;
            this.skinButtonSearch.BackColor = Color.Black;
            this.skinButtonSearch.DialogResult = DialogResult.OK;
            this.skinButtonSearch.DisabledForecolor = Color.Gray;
            this.skinButtonSearch.DrawEdges = true;
            this.skinButtonSearch.FocusColor = Color.Yellow;
            this.skinButtonSearch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSearch.ForeColor = Color.White;
            this.skinButtonSearch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSearch.IsStyled = true;
            this.skinButtonSearch.Location = new Point(0x100, 0x19e);
            this.skinButtonSearch.Name = "skinButtonSearch";
            this.skinButtonSearch.Size = new Size(0x33, 20);
            this.skinButtonSearch.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSearch, null);
            this.skinButtonSearch.TabIndex = 14;
            this.skinButtonSearch.Text = "<LOC>Go";
            this.skinButtonSearch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearch.TextPadding = new Padding(0);
            this.skinButtonSearch.Click += new EventHandler(this.skinButtonSearch_Click);
            this.skinButtonStart.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonStart.AutoStyle = true;
            this.skinButtonStart.BackColor = Color.Black;
            this.skinButtonStart.DialogResult = DialogResult.OK;
            this.skinButtonStart.DisabledForecolor = Color.Gray;
            this.skinButtonStart.DrawEdges = false;
            this.skinButtonStart.FocusColor = Color.Yellow;
            this.skinButtonStart.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonStart.ForeColor = Color.White;
            this.skinButtonStart.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonStart.IsStyled = true;
            this.skinButtonStart.Location = new Point(12, 0x181);
            this.skinButtonStart.Name = "skinButtonStart";
            this.skinButtonStart.Size = new Size(40, 0x16);
            this.skinButtonStart.SkinBasePath = @"Controls\Button\First";
            base.ttDefault.SetSuperTip(this.skinButtonStart, null);
            this.skinButtonStart.TabIndex = 0x10;
            this.skinButtonStart.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonStart.TextPadding = new Padding(0);
            this.skinButtonStart.Click += new EventHandler(this.skinButtonStart_Click);
            this.skinButtonEnd.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonEnd.AutoStyle = true;
            this.skinButtonEnd.BackColor = Color.Black;
            this.skinButtonEnd.DialogResult = DialogResult.OK;
            this.skinButtonEnd.DisabledForecolor = Color.Gray;
            this.skinButtonEnd.DrawEdges = false;
            this.skinButtonEnd.FocusColor = Color.Yellow;
            this.skinButtonEnd.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonEnd.ForeColor = Color.White;
            this.skinButtonEnd.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonEnd.IsStyled = true;
            this.skinButtonEnd.Location = new Point(0x24d, 0x181);
            this.skinButtonEnd.Name = "skinButtonEnd";
            this.skinButtonEnd.Size = new Size(40, 0x16);
            this.skinButtonEnd.SkinBasePath = @"Controls\Button\Last";
            base.ttDefault.SetSuperTip(this.skinButtonEnd, null);
            this.skinButtonEnd.TabIndex = 0x11;
            this.skinButtonEnd.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonEnd.TextPadding = new Padding(0);
            this.skinButtonEnd.Click += new EventHandler(this.skinButtonEnd_Click);
            this.gpgPanelLinkPages.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgPanelLinkPages.BackColor = Color.DarkGray;
            this.gpgPanelLinkPages.Location = new Point(0x5b, 0x181);
            this.gpgPanelLinkPages.Margin = new Padding(0);
            this.gpgPanelLinkPages.Name = "gpgPanelLinkPages";
            this.gpgPanelLinkPages.Size = new Size(0x1cb, 0x16);
            base.ttDefault.SetSuperTip(this.gpgPanelLinkPages, null);
            this.gpgPanelLinkPages.TabIndex = 0x12;
            this.gpgContextMenuChat.MenuItems.AddRange(new MenuItem[] { this.ciExport, this.ciSplitter1, this.ciChat_WhisperPlayer, this.ciChat_WebStats, this.ciChat_ViewPlayer, this.ciClanProfile, this.miReplays });
            this.ciExport.Index = 0;
            this.ciExport.MenuItems.AddRange(new MenuItem[] { this.ciExport_PDF, this.ciExport_HTML, this.ciExport_Text, this.ciExport_XLS });
            this.ciExport.Text = "<LOC>Export";
            this.ciExport_PDF.Index = 0;
            this.ciExport_PDF.Text = "<LOC>To PDF";
            this.ciExport_PDF.Click += new EventHandler(this.ciExport_PDF_Click);
            this.ciExport_HTML.Index = 1;
            this.ciExport_HTML.Text = "<LOC>To HTML";
            this.ciExport_HTML.Click += new EventHandler(this.ciExport_HTML_Click);
            this.ciExport_Text.Index = 2;
            this.ciExport_Text.Text = "<LOC>To Text";
            this.ciExport_Text.Click += new EventHandler(this.ciExport_Text_Click);
            this.ciExport_XLS.Index = 3;
            this.ciExport_XLS.Text = "<LOC>To XLS";
            this.ciExport_XLS.Click += new EventHandler(this.ciExport_XLS_Click);
            this.ciSplitter1.Index = 1;
            this.ciSplitter1.Text = "-";
            this.ciChat_WhisperPlayer.Index = 2;
            this.ciChat_WhisperPlayer.Text = "<LOC>Send private message";
            this.ciChat_WhisperPlayer.Click += new EventHandler(this.ciChat_WhisperPlayer_Click);
            this.ciChat_WebStats.Index = 3;
            this.ciChat_WebStats.Text = "<LOC>View web statistics";
            this.ciChat_WebStats.Click += new EventHandler(this.ciChat_WebStats_Click);
            this.ciChat_ViewPlayer.Index = 4;
            this.ciChat_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChat_ViewPlayer.Click += new EventHandler(this.ciChat_ViewPlayer_Click);
            this.ciClanProfile.Index = 5;
            this.ciClanProfile.Text = "<LOC>View this clan's profile";
            this.ciClanProfile.Visible = false;
            this.ciClanProfile.Click += new EventHandler(this.ciClanProfile_Click);
            this.miReplays.Index = 6;
            this.miReplays.Text = "<LOC>View this player's replays";
            this.miReplays.Click += new EventHandler(this.miReplays_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.gpgPanelLinkPages);
            base.Controls.Add(this.skinButtonEnd);
            base.Controls.Add(this.skinButtonStart);
            base.Controls.Add(this.skinButtonSearch);
            base.Controls.Add(this.gpgTextBoxSearchName);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.skinButtonNext);
            base.Controls.Add(this.skinButtonLast);
            base.Controls.Add(this.gpgDataGridLadder);
            base.Controls.Add(this.backLabelTitle);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x1c8, 0x180);
            base.Name = "FrmStatsLadder";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Ladder";
            base.Controls.SetChildIndex(this.backLabelTitle, 0);
            base.Controls.SetChildIndex(this.gpgDataGridLadder, 0);
            base.Controls.SetChildIndex(this.skinButtonLast, 0);
            base.Controls.SetChildIndex(this.skinButtonNext, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxSearchName, 0);
            base.Controls.SetChildIndex(this.skinButtonSearch, 0);
            base.Controls.SetChildIndex(this.skinButtonStart, 0);
            base.Controls.SetChildIndex(this.skinButtonEnd, 0);
            base.Controls.SetChildIndex(this.gpgPanelLinkPages, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgDataGridLadder.EndInit();
            this.gvLadder.EndInit();
            this.gpgTextBoxSearchName.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        internal void JumpTo(int rank)
        {
            try
            {
                if (rank > 0)
                {
                    this._ChangePage((rank - 1) / PAGE_SIZE);
                    int rowHandle = this.gvLadder.GetRowHandle((rank - 1) % PAGE_SIZE);
                    if (this.gvLadder.IsValidRowHandle(rowHandle))
                    {
                        this.gvLadder.MakeRowVisible(rowHandle, true);
                        this.gvLadder.SelectRow(rowHandle);
                        int[] selectedRows = this.gvLadder.GetSelectedRows();
                        for (int i = 0; i < selectedRows.Length; i++)
                        {
                            if (selectedRows[i] != rowHandle)
                            {
                                this.gvLadder.UnselectRow(selectedRows[i]);
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

        internal void JumpTo(string name)
        {
            try
            {
                int rank = -1;
                if (ConfigSettings.GetBool("DoOldGameList", false))
                {
                    rank = DataAccess.GetNumber("GetPlayerRanking", new object[] { name });
                }
                else
                {
                    rank = DataAccess.GetNumber("GetPlayerRanking2", new object[] { name, GameInformation.SelectedGame.GameID });
                }
                if (rank > 0)
                {
                    this.JumpTo(rank);
                    this.gvLadder.ClearSelection();
                    int rowHandle = -1;
                    for (int i = 0; i < this.gvLadder.RowCount; i++)
                    {
                        PlayerRating row = this.gvLadder.GetRow(i) as PlayerRating;
                        if ((row != null) && (row.PlayerName.ToUpper() == name.ToUpper()))
                        {
                            rowHandle = i;
                            break;
                        }
                    }
                    if (rowHandle >= 0)
                    {
                        this.gvLadder.SelectRow(rowHandle);
                    }
                    this.gvLadder.RefreshData();
                }
                else
                {
                    base.Error(this.skinButtonSearch, string.Format(Loc.Get("<LOC>Unable to locate {0}."), name), new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void LoadLayoutData()
        {
            try
            {
                if ((base.LayoutData != null) && (base.LayoutData is object[]))
                {
                    object[] layoutData = base.LayoutData as object[];
                    this.SetCategory((string) layoutData[0], false);
                    this.mCurrentPage = -1;
                    this.ChangePage((int) layoutData[1]);
                    base.MainForm.FrmStatsLadders.Add(this);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void Localize()
        {
            base.Localize();
            this.gpgContextMenuChat.Localize();
        }

        private void miReplays_Click(object sender, EventArgs e)
        {
            if (this.SelectedRating != null)
            {
                base.MainForm.ShowDlgSearchReplays(this.SelectedRating.PlayerName);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextBoxSearchName.Select();
        }

        private void OnRefreshData(DataList data)
        {
            if ((data != null) && (data.Count > 0))
            {
                this.mCurrentDataSet = new MappedObjectList<PlayerRating>(data);
                this.gpgDataGridLadder.DataSource = null;
                this.gpgDataGridLadder.DataSource = this.CurrentDataSet;
                this.gvLadder.RefreshData();
                this.ResizePageLinks();
            }
        }

        private void RefreshData()
        {
            int num = (this.CurrentPage * PAGE_SIZE) + 1;
            int num2 = (this.CurrentPage + 1) * PAGE_SIZE;
            if (ConfigSettings.GetBool("DoOldGameList", false))
            {
                if (this.mCategory.ToUpper() == "CLAN")
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshData", new object[] { "GetRatingsListClan", new object[] { this.Category, num, num2 } });
                }
                else
                {
                    ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshData", new object[] { "GetRatingsList", new object[] { this.Category, num, num2 } });
                }
            }
            else if (this.mCategory.ToUpper() == "CLAN")
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshData", new object[] { "GetRatingsListClan2", new object[] { this.Category, num, num2, GameInformation.SelectedGame.GameID } });
            }
            else
            {
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "OnRefreshData", new object[] { "GetRatingsList2", new object[] { this.Category, num, num2, GameInformation.SelectedGame.GameID } });
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
                                goto Label_0132;
                            }
                            list.Add(width);
                            num += width;
                            num2++;
                            if (num2 > this.LastPage)
                            {
                                num3 = this.gpgPanelLinkPages.Width - num;
                                goto Label_0132;
                            }
                        }
                    }
                }
            Label_0132:
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
                        if ((i + num8) == this.LastPage)
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

        protected override void SaveLayoutData()
        {
            base.LayoutData = new object[] { this.Category, this.CurrentPage };
        }

        private void SetCategory(string category, bool refreshData)
        {
            this.mCategory = category;
            string data = "";
            if (category.ToUpper() == "CLAN")
            {
                data = "<LOC>Clan Rankings";
                this.ciChat_WhisperPlayer.Visible = false;
                this.ciChat_WebStats.Visible = false;
                this.ciChat_ViewPlayer.Visible = false;
                this.ciClanProfile.Visible = true;
                this.ciSplitter1.Visible = true;
                this.miReplays.Visible = false;
                this.gcAverageRating.Visible = true;
                this.gcAverageRating.VisibleIndex = 8;
                this.gcPeople.Visible = true;
                this.gcPeople.VisibleIndex = 9;
                this.gcTopTenRating.Visible = true;
                this.gcTopTenRating.VisibleIndex = 10;
                base.Width += 200;
            }
            else if (category.ToUpper() == "1V1")
            {
                data = "<LOC>1v1 Rankings";
                this.ciChat_WhisperPlayer.Visible = true;
                this.ciChat_WebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
                this.ciChat_ViewPlayer.Visible = true;
                this.ciClanProfile.Visible = false;
                this.ciSplitter1.Visible = true;
                this.miReplays.Visible = ConfigSettings.GetBool("ShowRatingsReplayLink", true);
                this.gcAverageRating.Visible = false;
                this.gcPeople.Visible = false;
                this.gcTopTenRating.Visible = false;
            }
            else if (category.ToUpper() == "2V2")
            {
                data = "<LOC>2v2 Rankings";
                this.ciChat_WhisperPlayer.Visible = false;
                this.ciChat_WebStats.Visible = false;
                this.ciChat_ViewPlayer.Visible = false;
                this.ciSplitter1.Visible = false;
                this.ciClanProfile.Visible = false;
                this.miReplays.Visible = false;
                this.gcAverageRating.Visible = false;
                this.gcPeople.Visible = false;
                this.gcTopTenRating.Visible = false;
            }
            else if (category.ToUpper() == "3V3")
            {
                data = "<LOC>3v3 Rankings";
                this.ciChat_WhisperPlayer.Visible = false;
                this.ciChat_WebStats.Visible = false;
                this.ciChat_ViewPlayer.Visible = false;
                this.ciSplitter1.Visible = false;
                this.ciClanProfile.Visible = false;
                this.miReplays.Visible = false;
                this.gcAverageRating.Visible = false;
                this.gcPeople.Visible = false;
                this.gcTopTenRating.Visible = false;
            }
            else if (category.ToUpper() == "4V4")
            {
                data = "<LOC>4v4 Rankings";
                this.ciChat_WhisperPlayer.Visible = false;
                this.ciChat_WebStats.Visible = false;
                this.ciChat_ViewPlayer.Visible = false;
                this.ciSplitter1.Visible = false;
                this.ciClanProfile.Visible = false;
                this.miReplays.Visible = false;
                this.gcAverageRating.Visible = false;
                this.gcPeople.Visible = false;
                this.gcTopTenRating.Visible = false;
            }
            this.gvLadder.MouseDown += new MouseEventHandler(this.gvLadder_MouseDown);
            this.gvLadder.MouseUp += new MouseEventHandler(this.gvLadder_MouseUp);
            this.gvLadder.DoubleClick += new EventHandler(this.gvLadder_DoubleClick);
            this.backLabelTitle.Text = Loc.Get(data);
            this.Text = Loc.Get(data);
            if (refreshData)
            {
                this.RefreshData();
            }
            base.SizeChanged += delegate (object o, EventArgs e) {
                this.backLabelTitle.Refresh();
                this.ResizePageLinks();
            };
            this.gpgTextBoxSearchName.Select();
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

        private void skinButtonSearch_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if ((this.gpgTextBoxSearchName.Text != null) && (this.gpgTextBoxSearchName.Text.Length > 0))
            {
                this.JumpTo(this.gpgTextBoxSearchName.Text);
            }
        }

        private void skinButtonStart_Click(object sender, EventArgs e)
        {
            if (this.CurrentPage != 0)
            {
                this.ChangePage(0);
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        public string Category
        {
            get
            {
                return this.mCategory;
            }
        }

        public MappedObjectList<PlayerRating> CurrentDataSet
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
                    this.mLastRating = DataAccess.GetNumber("GetLastRating2", new object[] { this.Category });
                }
                return this.mLastRating;
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return true;
            }
        }

        public PlayerRating SelectedRating
        {
            get
            {
                return this.mSelectedRating;
            }
        }

        public override string SingletonName
        {
            get
            {
                return string.Format("{0}_{1}", base.SingletonName, this.Category);
            }
        }
    }
}

