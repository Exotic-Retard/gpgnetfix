namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
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
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgSearchReplays : DlgBase
    {
        private static Dictionary<string, DlgReplayInfo> ActiveReplayWindows = new Dictionary<string, DlgReplayInfo>();
        private MenuItem ciDownload_Download;
        private MenuItem ciDownload_View;
        private GridColumn colGameDate;
        private GridColumn colGameType;
        private GridColumn colMap;
        private GridColumn colOpponent;
        private GridColumn colPlayerName;
        private GridColumn colTitle;
        private System.Windows.Forms.ComboBox comboBoxGameType;
        private System.Windows.Forms.ComboBox comboBoxMaps;
        private System.Windows.Forms.ComboBox comboBoxOpponentFaction;
        private System.Windows.Forms.ComboBox comboBoxPlayerFaction;
        private IContainer components;
        private object[] CurrentCriteria;
        private int CurrentDataSize;
        private ReplayList CurrentList;
        private DateEdit dateEditEarliest;
        private DateEdit dateEditLatest;
        private GPGDataGrid dgReplays;
        private GridColumn gcChatLink;
        private GridColumn gcDownloads;
        private GridColumn gcGameLength;
        private GridColumn gcRating;
        private GPGContextMenu gpgContextMenuDownload;
        private GPGGroupBox gpgGroupBoxCriterial;
        private GPGGroupBox gpgGroupBoxReplayLists;
        private GPGGroupBox gpgGroupBoxResults;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabelMap;
        private GPGLabel gpgLabelMostRecent;
        private GPGLabel gpgLabelTopDownload;
        private GPGLabel gpgLabelTopRated;
        private GPGPanel gpgPanelReplayLists;
        private GPGTextBox gpgTextBoxKeywords;
        private GPGTextBox gpgTextBoxPlayer;
        private GridView gvReplays;
        private int? LastPage;
        private const int MAX_KEYWORDS = 4;
        private int Page;
        private RepositoryItemHyperLinkEdit repositoryItemChatLink;
        private RepositoryItemPictureEdit repositoryItemRatingStars;
        private SkinButton skinButtonNext;
        private SkinButton skinButtonPrevious;
        private SkinButton skinButtonSearch;
        private SkinButton skinButtonStart;
        private SkinLabel skinLabelPage;

        public DlgSearchReplays() : this(Program.MainForm)
        {
        }

        public DlgSearchReplays(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.Page = 0;
            this.CurrentList = null;
            this.CurrentDataSize = 0;
            this.CurrentCriteria = null;
            this.LastPage = null;
            this.InitializeComponent();
            this.Construct();
            this.ResetForm();
        }

        public DlgSearchReplays(FrmMain mainForm, string playerName) : base(mainForm)
        {
            this.components = null;
            this.Page = 0;
            this.CurrentList = null;
            this.CurrentDataSize = 0;
            this.CurrentCriteria = null;
            this.LastPage = null;
            this.InitializeComponent();
            this.Construct();
            this.ResetForm();
            this.DoPlayerSearch(playerName);
        }

        private void ChangePage(int page)
        {
            int num = this.Page;
            this.Page = page;
            if (this.CurrentList == null)
            {
                if (this.ExecuteSearch() == 0)
                {
                    this.Page = num;
                }
            }
            else if (this.ExecuteList() == 0)
            {
                this.Page = num;
            }
        }

        private void ciDownload_Download_Click(object sender, EventArgs e)
        {
            this.DownloadSelected();
        }

        private void ciDownload_View_Click(object sender, EventArgs e)
        {
            this.ViewSelected();
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            VGen1 method = null;
            if (!base.IsDisposed && !base.Disposing)
            {
                if (method == null)
                {
                    method = delegate (object objpercent) {
                        base.SetStatus(Loc.Get("<LOC>Downloading:") + " " + ((int) objpercent).ToString() + "%", new object[0]);
                    };
                }
                base.BeginInvoke(method, new object[] { e.ProgressPercentage });
            }
        }

        private void comboBoxGameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MultiVal<string, int> selectedItem = this.comboBoxGameType.SelectedItem as MultiVal<string, int>;
                if ((selectedItem != null) && (selectedItem.Value2 == 2))
                {
                    this.comboBoxOpponentFaction.Enabled = true;
                    this.comboBoxPlayerFaction.Enabled = true;
                }
                else
                {
                    this.comboBoxOpponentFaction.Enabled = false;
                    this.comboBoxPlayerFaction.Enabled = false;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                this.comboBoxOpponentFaction.Enabled = false;
                this.comboBoxPlayerFaction.Enabled = false;
            }
        }

        private void Construct()
        {
            this.comboBoxMaps.DataSource = SupcomLookups.Maps;
            this.comboBoxMaps.SelectedIndex = 0;
            this.comboBoxGameType.DataSource = SupcomLookups.GameTypes;
            this.comboBoxGameType.SelectedIndex = 0;
            this.comboBoxOpponentFaction.DataSource = SupcomLookups.Factions.Clone();
            this.comboBoxOpponentFaction.SelectedIndex = 0;
            this.comboBoxPlayerFaction.DataSource = SupcomLookups.Factions.Clone();
            this.comboBoxPlayerFaction.SelectedIndex = 0;
            this.comboBoxMaps.SelectedIndex = 0;
            this.LoadGlobalReplayLists();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DlgReplayInfo info = sender as DlgReplayInfo;
                ActiveReplayWindows.Remove(info.Replay.Location);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void DoPlayerSearch(string playerName)
        {
            if (playerName != null)
            {
                this.Page = 0;
                this.CurrentDataSize = 0;
                this.CurrentCriteria = null;
                this.ResetForm();
                this.gpgTextBoxPlayer.Text = playerName;
                if (this.ExecuteSearch() == 0)
                {
                    this.dgReplays.DataSource = null;
                    this.skinLabelPage.Text = Loc.Get("<LOC>No Results");
                }
            }
        }

        private void DownloadSelected()
        {
            WaitCallback callBack = null;
            if (this.gvReplays.GetSelectedRows().Length > 0)
            {
                ReplayInfo row = this.gvReplays.GetRow(this.gvReplays.GetSelectedRows()[0]) as ReplayInfo;
                if (callBack == null)
                {
                    callBack = delegate (object o) {
                        try
                        {
                            ReplayInfo info = (o as object[])[0] as ReplayInfo;
                            if (info != null)
                            {
                                info.Download();
                                this.gvReplays.RefreshData();
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[] { row });
            }
        }

        private int ExecuteList()
        {
            try
            {
                MappedObjectList<ReplayInfo> objects;
                if (this.CurrentList == null)
                {
                    return -1;
                }
                if (this.CurrentList.IsQuery)
                {
                    if (this.CurrentList.IsPaged)
                    {
                        objects = DataAccess.GetObjects<ReplayInfo>(this.CurrentList.Query, new object[] { this.Page * this.CurrentList.PageSize, this.CurrentList.PageSize });
                        if (objects.Count > 0)
                        {
                            this.CurrentDataSize = objects.Count;
                            this.dgReplays.DataSource = objects;
                            this.skinLabelPage.Text = string.Format("{0} - Page {1}", Loc.Get(this.CurrentList.Title), this.Page + 1);
                            return 1;
                        }
                        return 0;
                    }
                    objects = DataAccess.GetObjects<ReplayInfo>(this.CurrentList.Query, new object[0]);
                    if (objects.Count > 0)
                    {
                        this.CurrentDataSize = objects.Count;
                        this.dgReplays.DataSource = objects;
                        this.skinLabelPage.Text = Loc.Get(this.CurrentList.Title);
                        return 1;
                    }
                    return 0;
                }
                if (this.CurrentList.IsPaged)
                {
                    objects = DataAccess.GetObjects<ReplayInfo>("GetPagedReplayListById", new object[] { this.CurrentList.ID, this.Page * this.CurrentList.PageSize, this.CurrentList.PageSize });
                    if (objects.Count > 0)
                    {
                        this.CurrentDataSize = objects.Count;
                        this.dgReplays.DataSource = objects;
                        this.skinLabelPage.Text = string.Format("{0} - Page {1}", Loc.Get(this.CurrentList.Title), this.Page + 1);
                        return 1;
                    }
                    return 0;
                }
                objects = DataAccess.GetObjects<ReplayInfo>("GetReplayListById", new object[] { this.CurrentList.ID });
                if (objects.Count > 0)
                {
                    this.CurrentDataSize = objects.Count;
                    this.dgReplays.DataSource = objects;
                    this.skinLabelPage.Text = Loc.Get(this.CurrentList.Title);
                    return 1;
                }
                return 0;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return -1;
            }
        }

        private int ExecuteSearch()
        {
            int num5;
            try
            {
                base.ClearErrors();
                base.SetStatus("<LOC>Searching...", new object[0]);
                if (this.CurrentCriteria == null)
                {
                    if (!DataAccess.ValidateString(this.gpgTextBoxPlayer.Text))
                    {
                        base.Error(this.gpgTextBoxPlayer, "<LOC>This field contains invalid characters.", new object[0]);
                        return -1;
                    }
                    if (!DataAccess.ValidateString(this.gpgTextBoxKeywords.Text))
                    {
                        base.Error(this.gpgTextBoxKeywords, "<LOC>This field contains invalid characters.", new object[0]);
                        return -1;
                    }
                    string[] strArray = this.gpgTextBoxKeywords.Text.TrimEnd(new char[] { ' ' }).Split(new char[] { ' ' });
                    if (strArray.Length > 4)
                    {
                        base.Error(this.gpgTextBoxKeywords, "<LOC>You must limit the keyword search to 4 words.", new object[0]);
                        return -1;
                    }
                    string[] strArray2 = new string[] { "", "", "", "" };
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if ((strArray[i] != null) && (strArray[i].Length > 0))
                        {
                            strArray2[i] = string.Format("%{0}%", strArray[i]);
                        }
                    }
                    string text = this.gpgTextBoxPlayer.Text;
                    string str2 = DataAccess.FormatDate(this.dateEditEarliest.DateTime);
                    string str3 = DataAccess.FormatDate(this.dateEditLatest.DateTime);
                    string str4 = ((MultiVal<string, string>) this.comboBoxMaps.SelectedItem).Value2;
                    int num2 = ((MultiVal<string, int>) this.comboBoxGameType.SelectedItem).Value2;
                    int num3 = ((MultiVal<string, int>) this.comboBoxPlayerFaction.SelectedItem).Value2;
                    if (!this.comboBoxPlayerFaction.Enabled)
                    {
                        num3 = 0;
                    }
                    int num4 = ((MultiVal<string, int>) this.comboBoxOpponentFaction.SelectedItem).Value2;
                    if (!this.comboBoxOpponentFaction.Enabled)
                    {
                        num4 = 0;
                    }
                    if (ConfigSettings.GetBool("DoOldGameList", false))
                    {
                        this.CurrentCriteria = new object[] { 
                            text, text, text, strArray2[0], strArray2[0], strArray2[1], strArray2[1], strArray2[2], strArray2[2], strArray2[3], strArray2[3], str4, str4, num2, num2, num3, 
                            num3, num4, num4, str2, str3, this.Page * MAX_PAGE_SIZE, MAX_PAGE_SIZE
                         };
                    }
                    else
                    {
                        this.CurrentCriteria = new object[] { 
                            text, text, text, strArray2[0], strArray2[0], strArray2[1], strArray2[1], strArray2[2], strArray2[2], strArray2[3], strArray2[3], str4, str4, num2, num2, num3, 
                            num3, num4, num4, str2, str3, GameInformation.SelectedGame.GameID, this.Page * MAX_PAGE_SIZE, MAX_PAGE_SIZE
                         };
                    }
                }
                else
                {
                    this.CurrentCriteria[this.CurrentCriteria.Length - 2] = this.Page * MAX_PAGE_SIZE;
                }
                MappedObjectList<ReplayInfo> objects = null;
                if (ConfigSettings.GetBool("DoOldGameList", false))
                {
                    objects = DataAccess.GetObjects<ReplayInfo>("ReplaySearch", this.CurrentCriteria);
                }
                else
                {
                    objects = DataAccess.GetObjects<ReplayInfo>("ReplaySearch2", this.CurrentCriteria);
                }
                if (objects.Count > 0)
                {
                    this.CurrentDataSize = objects.Count;
                    this.dgReplays.DataSource = objects;
                    this.skinLabelPage.Text = string.Format(Loc.Get("<LOC>Page {0}"), this.Page + 1);
                    return 1;
                }
                num5 = 0;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                num5 = -1;
            }
            finally
            {
                base.SetStatus("", new object[0]);
            }
            return num5;
        }

        private void gvReplays_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.Name == this.gcRating.Name)
            {
                ReplayInfo row = this.gvReplays.GetRow(this.gvReplays.GetRowHandle(e.ListSourceRowIndex1)) as ReplayInfo;
                ReplayInfo info2 = this.gvReplays.GetRow(this.gvReplays.GetRowHandle(e.ListSourceRowIndex2)) as ReplayInfo;
                if (row.Rating < info2.Rating)
                {
                    e.Result = -1;
                }
                else
                {
                    e.Result = 1;
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void gvReplays_DoubleClick(object sender, EventArgs e)
        {
            this.ViewSelected();
        }

        private void gvReplays_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo info = this.gvReplays.CalcHitInfo(e.Location);
            if (info.InRow)
            {
                this.gvReplays.SelectRow(info.RowHandle);
            }
        }

        private void gvReplays_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                GridHitInfo info = this.gvReplays.CalcHitInfo(e.Location);
                if (info.InRowCell && (info.Column.AbsoluteIndex == this.gcChatLink.AbsoluteIndex))
                {
                    if (Chatroom.InChatroom)
                    {
                        DialogResult result;
                        string str = DlgAskQuestion.AskQuestion(base.MainForm, "<LOC>Enter chat a message to go along with this replay link.", "<LOC>Chat Message", false, out result);
                        if (result == DialogResult.OK)
                        {
                            ReplayInfo row = this.gvReplays.GetRow(info.RowHandle) as ReplayInfo;
                            string location = row.Location;
                            if ((str != null) && (str.Length > 0))
                            {
                                base.MainForm.SendMessage(string.Format("{0} : replay:{1}", str, location));
                            }
                            else
                            {
                                base.MainForm.SendMessage(string.Format("replay:{0}", location));
                            }
                        }
                    }
                    else
                    {
                        base.MainForm.ErrorMessage("<LOC>You are not in a chatroom, you must join a chatroom to share this replay link. Click here to join chat:'{0}'", new object[] { base.MainForm.MainChatroom });
                    }
                }
            }
            else if ((e.Button == MouseButtons.Right) && (this.gvReplays.GetSelectedRows().Length > 0))
            {
                this.gpgContextMenuDownload.Show(this.dgReplays, e.Location);
            }
        }

        private void InitializeComponent()
        {
            this.gpgGroupBoxResults = new GPGGroupBox();
            this.skinLabelPage = new SkinLabel();
            this.dgReplays = new GPGDataGrid();
            this.gvReplays = new GridView();
            this.colPlayerName = new GridColumn();
            this.colOpponent = new GridColumn();
            this.colTitle = new GridColumn();
            this.colMap = new GridColumn();
            this.colGameType = new GridColumn();
            this.colGameDate = new GridColumn();
            this.gcGameLength = new GridColumn();
            this.gcDownloads = new GridColumn();
            this.gcRating = new GridColumn();
            this.repositoryItemRatingStars = new RepositoryItemPictureEdit();
            this.gcChatLink = new GridColumn();
            this.repositoryItemChatLink = new RepositoryItemHyperLinkEdit();
            this.skinButtonStart = new SkinButton();
            this.skinButtonNext = new SkinButton();
            this.skinButtonPrevious = new SkinButton();
            this.gpgGroupBoxCriterial = new GPGGroupBox();
            this.gpgTextBoxKeywords = new GPGTextBox();
            this.comboBoxOpponentFaction = new System.Windows.Forms.ComboBox();
            this.gpgLabel7 = new GPGLabel();
            this.comboBoxPlayerFaction = new System.Windows.Forms.ComboBox();
            this.gpgLabel6 = new GPGLabel();
            this.comboBoxGameType = new System.Windows.Forms.ComboBox();
            this.gpgLabel5 = new GPGLabel();
            this.comboBoxMaps = new System.Windows.Forms.ComboBox();
            this.gpgLabelMap = new GPGLabel();
            this.dateEditEarliest = new DateEdit();
            this.dateEditLatest = new DateEdit();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxPlayer = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonSearch = new SkinButton();
            this.gpgContextMenuDownload = new GPGContextMenu();
            this.ciDownload_View = new MenuItem();
            this.ciDownload_Download = new MenuItem();
            this.gpgLabelTopRated = new GPGLabel();
            this.gpgLabelTopDownload = new GPGLabel();
            this.gpgLabelMostRecent = new GPGLabel();
            this.gpgGroupBoxReplayLists = new GPGGroupBox();
            this.gpgPanelReplayLists = new GPGPanel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgGroupBoxResults.SuspendLayout();
            this.dgReplays.BeginInit();
            this.gvReplays.BeginInit();
            this.repositoryItemRatingStars.BeginInit();
            this.repositoryItemChatLink.BeginInit();
            this.gpgGroupBoxCriterial.SuspendLayout();
            this.gpgTextBoxKeywords.Properties.BeginInit();
            this.dateEditEarliest.Properties.BeginInit();
            this.dateEditLatest.Properties.BeginInit();
            this.gpgTextBoxPlayer.Properties.BeginInit();
            this.gpgGroupBoxReplayLists.SuspendLayout();
            this.gpgPanelReplayLists.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x284, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgGroupBoxResults.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGroupBoxResults.Controls.Add(this.skinLabelPage);
            this.gpgGroupBoxResults.Controls.Add(this.dgReplays);
            this.gpgGroupBoxResults.Controls.Add(this.skinButtonStart);
            this.gpgGroupBoxResults.Controls.Add(this.skinButtonNext);
            this.gpgGroupBoxResults.Controls.Add(this.skinButtonPrevious);
            this.gpgGroupBoxResults.Location = new Point(12, 0x8f);
            this.gpgGroupBoxResults.Name = "gpgGroupBoxResults";
            this.gpgGroupBoxResults.Size = new Size(680, 0xd4);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxResults, null);
            this.gpgGroupBoxResults.TabIndex = 7;
            this.gpgGroupBoxResults.TabStop = false;
            this.gpgGroupBoxResults.Text = "<LOC>Search Results";
            this.skinLabelPage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinLabelPage.AutoStyle = false;
            this.skinLabelPage.BackColor = Color.Transparent;
            this.skinLabelPage.DrawEdges = true;
            this.skinLabelPage.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelPage.ForeColor = Color.White;
            this.skinLabelPage.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelPage.IsStyled = false;
            this.skinLabelPage.Location = new Point(0x55, 0xb8);
            this.skinLabelPage.Name = "skinLabelPage";
            this.skinLabelPage.Size = new Size(550, 0x16);
            this.skinLabelPage.SkinBasePath = @"Controls\Background Label\BlackBar";
            base.ttDefault.SetSuperTip(this.skinLabelPage, null);
            this.skinLabelPage.TabIndex = 0x19;
            this.skinLabelPage.Text = "<LOC>No Results";
            this.skinLabelPage.TextAlign = ContentAlignment.MiddleCenter;
            this.skinLabelPage.TextPadding = new Padding(0);
            this.dgReplays.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dgReplays.CustomizeStyle = false;
            this.dgReplays.EmbeddedNavigator.Name = "";
            this.dgReplays.Location = new Point(6, 0x13);
            this.dgReplays.MainView = this.gvReplays;
            this.dgReplays.Name = "dgReplays";
            this.dgReplays.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemRatingStars, this.repositoryItemChatLink });
            this.dgReplays.ShowOnlyPredefinedDetails = true;
            this.dgReplays.Size = new Size(0x29c, 0x9f);
            this.dgReplays.TabIndex = 0x18;
            this.dgReplays.ViewCollection.AddRange(new BaseView[] { this.gvReplays });
            this.gvReplays.Appearance.EvenRow.BackColor = Color.Black;
            this.gvReplays.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvReplays.Appearance.OddRow.BackColor = Color.Black;
            this.gvReplays.Appearance.OddRow.Options.UseBackColor = true;
            this.gvReplays.Appearance.Preview.ForeColor = Color.Silver;
            this.gvReplays.Appearance.Preview.Options.UseForeColor = true;
            this.gvReplays.Columns.AddRange(new GridColumn[] { this.colPlayerName, this.colOpponent, this.colTitle, this.colMap, this.colGameType, this.colGameDate, this.gcGameLength, this.gcDownloads, this.gcRating, this.gcChatLink });
            this.gvReplays.GridControl = this.dgReplays;
            this.gvReplays.GroupPanelText = "<LOC>Select a replay to watch.";
            this.gvReplays.Name = "gvReplays";
            this.gvReplays.OptionsBehavior.Editable = false;
            this.gvReplays.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReplays.OptionsView.AutoCalcPreviewLineCount = true;
            this.gvReplays.OptionsView.RowAutoHeight = true;
            this.gvReplays.OptionsView.ShowPreview = true;
            this.gvReplays.PreviewFieldName = "GameInfo";
            this.gvReplays.MouseDown += new MouseEventHandler(this.gvReplays_MouseDown);
            this.gvReplays.DoubleClick += new EventHandler(this.gvReplays_DoubleClick);
            this.gvReplays.CustomColumnSort += new CustomColumnSortEventHandler(this.gvReplays_CustomColumnSort);
            this.gvReplays.MouseUp += new MouseEventHandler(this.gvReplays_MouseUp);
            this.colPlayerName.AppearanceCell.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.colPlayerName.AppearanceCell.ForeColor = Color.Green;
            this.colPlayerName.AppearanceCell.Options.UseFont = true;
            this.colPlayerName.AppearanceCell.Options.UseForeColor = true;
            this.colPlayerName.Caption = "<LOC>Player Name";
            this.colPlayerName.FieldName = "PlayerName";
            this.colPlayerName.Name = "colPlayerName";
            this.colPlayerName.Visible = true;
            this.colPlayerName.VisibleIndex = 0;
            this.colOpponent.AppearanceCell.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.colOpponent.AppearanceCell.ForeColor = Color.FromArgb(0xc0, 0, 0);
            this.colOpponent.AppearanceCell.Options.UseFont = true;
            this.colOpponent.AppearanceCell.Options.UseForeColor = true;
            this.colOpponent.Caption = "<LOC>Other Players";
            this.colOpponent.FieldName = "Opponent";
            this.colOpponent.Name = "colOpponent";
            this.colOpponent.Visible = true;
            this.colOpponent.VisibleIndex = 1;
            this.colTitle.Caption = "<LOC>Title";
            this.colTitle.FieldName = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.Visible = true;
            this.colTitle.VisibleIndex = 3;
            this.colMap.Caption = "<LOC>Map";
            this.colMap.FieldName = "MapName";
            this.colMap.Name = "colMap";
            this.colMap.Visible = true;
            this.colMap.VisibleIndex = 2;
            this.colGameType.Caption = "<LOC>Game Type";
            this.colGameType.FieldName = "GameType";
            this.colGameType.Name = "colGameType";
            this.colGameType.Visible = true;
            this.colGameType.VisibleIndex = 4;
            this.colGameDate.Caption = "<LOC>Game Date";
            this.colGameDate.FieldName = "CreateDate";
            this.colGameDate.Name = "colGameDate";
            this.colGameDate.Visible = true;
            this.colGameDate.VisibleIndex = 5;
            this.gcGameLength.Caption = "<LOC>Game Length";
            this.gcGameLength.FieldName = "GameLength";
            this.gcGameLength.Name = "gcGameLength";
            this.gcGameLength.Visible = true;
            this.gcGameLength.VisibleIndex = 6;
            this.gcDownloads.AppearanceCell.Options.UseTextOptions = true;
            this.gcDownloads.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            this.gcDownloads.Caption = "<LOC>Downloads";
            this.gcDownloads.FieldName = "Downloads";
            this.gcDownloads.Name = "gcDownloads";
            this.gcDownloads.Visible = true;
            this.gcDownloads.VisibleIndex = 7;
            this.gcRating.AppearanceCell.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcRating.AppearanceCell.ForeColor = Color.Gold;
            this.gcRating.AppearanceCell.Options.UseFont = true;
            this.gcRating.AppearanceCell.Options.UseForeColor = true;
            this.gcRating.AppearanceCell.Options.UseTextOptions = true;
            this.gcRating.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            this.gcRating.Caption = "<LOC>Rating";
            this.gcRating.ColumnEdit = this.repositoryItemRatingStars;
            this.gcRating.FieldName = "RatingImage";
            this.gcRating.Name = "gcRating";
            this.gcRating.OptionsColumn.AllowSort = DefaultBoolean.True;
            this.gcRating.SortMode = ColumnSortMode.Custom;
            this.gcRating.Visible = true;
            this.gcRating.VisibleIndex = 8;
            this.repositoryItemRatingStars.Name = "repositoryItemRatingStars";
            this.gcChatLink.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            this.gcChatLink.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            this.gcChatLink.Caption = "<LOC>Link to Chat";
            this.gcChatLink.ColumnEdit = this.repositoryItemChatLink;
            this.gcChatLink.FieldName = "ChatLink";
            this.gcChatLink.ImageAlignment = StringAlignment.Center;
            this.gcChatLink.Name = "gcChatLink";
            this.gcChatLink.OptionsColumn.AllowSort = DefaultBoolean.False;
            this.gcChatLink.Visible = true;
            this.gcChatLink.VisibleIndex = 9;
            this.repositoryItemChatLink.Appearance.Options.UseTextOptions = true;
            this.repositoryItemChatLink.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            this.repositoryItemChatLink.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            this.repositoryItemChatLink.AppearanceFocused.Options.UseTextOptions = true;
            this.repositoryItemChatLink.AppearanceFocused.TextOptions.HAlignment = HorzAlignment.Center;
            this.repositoryItemChatLink.AppearanceFocused.TextOptions.VAlignment = VertAlignment.Center;
            this.repositoryItemChatLink.AutoHeight = false;
            this.repositoryItemChatLink.LinkColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.repositoryItemChatLink.Name = "repositoryItemChatLink";
            this.repositoryItemChatLink.SingleClick = true;
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
            this.skinButtonStart.Location = new Point(6, 0xb8);
            this.skinButtonStart.Name = "skinButtonStart";
            this.skinButtonStart.Size = new Size(40, 0x16);
            this.skinButtonStart.SkinBasePath = @"Controls\Button\First";
            base.ttDefault.SetSuperTip(this.skinButtonStart, null);
            this.skinButtonStart.TabIndex = 5;
            this.skinButtonStart.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonStart.TextPadding = new Padding(0);
            this.skinButtonStart.Click += new EventHandler(this.skinButtonStart_Click);
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
            this.skinButtonNext.Location = new Point(0x27a, 0xb8);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(40, 0x16);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Next_End";
            base.ttDefault.SetSuperTip(this.skinButtonNext, null);
            this.skinButtonNext.TabIndex = 7;
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.skinButtonPrevious.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonPrevious.AutoStyle = true;
            this.skinButtonPrevious.BackColor = Color.Black;
            this.skinButtonPrevious.DialogResult = DialogResult.OK;
            this.skinButtonPrevious.DisabledForecolor = Color.Gray;
            this.skinButtonPrevious.DrawEdges = false;
            this.skinButtonPrevious.FocusColor = Color.Yellow;
            this.skinButtonPrevious.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonPrevious.ForeColor = Color.White;
            this.skinButtonPrevious.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonPrevious.IsStyled = true;
            this.skinButtonPrevious.Location = new Point(0x2e, 0xb8);
            this.skinButtonPrevious.Name = "skinButtonPrevious";
            this.skinButtonPrevious.Size = new Size(40, 0x16);
            this.skinButtonPrevious.SkinBasePath = @"Controls\Button\Previous";
            base.ttDefault.SetSuperTip(this.skinButtonPrevious, null);
            this.skinButtonPrevious.TabIndex = 6;
            this.skinButtonPrevious.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonPrevious.TextPadding = new Padding(0);
            this.skinButtonPrevious.Click += new EventHandler(this.skinButtonPrevious_Click);
            this.gpgGroupBoxCriterial.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgTextBoxKeywords);
            this.gpgGroupBoxCriterial.Controls.Add(this.comboBoxOpponentFaction);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabel7);
            this.gpgGroupBoxCriterial.Controls.Add(this.comboBoxPlayerFaction);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabel6);
            this.gpgGroupBoxCriterial.Controls.Add(this.comboBoxGameType);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabel5);
            this.gpgGroupBoxCriterial.Controls.Add(this.comboBoxMaps);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabelMap);
            this.gpgGroupBoxCriterial.Controls.Add(this.dateEditEarliest);
            this.gpgGroupBoxCriterial.Controls.Add(this.dateEditLatest);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabel4);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabel3);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabel2);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgTextBoxPlayer);
            this.gpgGroupBoxCriterial.Controls.Add(this.gpgLabel1);
            this.gpgGroupBoxCriterial.Controls.Add(this.skinButtonSearch);
            this.gpgGroupBoxCriterial.Location = new Point(12, 0x169);
            this.gpgGroupBoxCriterial.Name = "gpgGroupBoxCriterial";
            this.gpgGroupBoxCriterial.Size = new Size(680, 0xa7);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxCriterial, null);
            this.gpgGroupBoxCriterial.TabIndex = 8;
            this.gpgGroupBoxCriterial.TabStop = false;
            this.gpgGroupBoxCriterial.Text = "<LOC>Search Criteria";
            this.gpgTextBoxKeywords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxKeywords.Location = new Point(5, 0x8d);
            this.gpgTextBoxKeywords.Name = "gpgTextBoxKeywords";
            this.gpgTextBoxKeywords.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxKeywords.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxKeywords.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxKeywords.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxKeywords.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxKeywords.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxKeywords.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxKeywords.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxKeywords.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxKeywords.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxKeywords.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxKeywords.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxKeywords.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxKeywords.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxKeywords.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxKeywords.Size = new Size(0x1d8, 20);
            this.gpgTextBoxKeywords.TabIndex = 0x18;
            this.comboBoxOpponentFaction.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.comboBoxOpponentFaction.BackColor = Color.Black;
            this.comboBoxOpponentFaction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxOpponentFaction.Enabled = false;
            this.comboBoxOpponentFaction.FlatStyle = FlatStyle.Flat;
            this.comboBoxOpponentFaction.FormattingEnabled = true;
            this.comboBoxOpponentFaction.ItemHeight = 13;
            this.comboBoxOpponentFaction.Location = new Point(0x156, 0x5c);
            this.comboBoxOpponentFaction.MaxDropDownItems = 12;
            this.comboBoxOpponentFaction.Name = "comboBoxOpponentFaction";
            this.comboBoxOpponentFaction.Size = new Size(0x87, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxOpponentFaction, null);
            this.comboBoxOpponentFaction.TabIndex = 0x17;
            this.gpgLabel7.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabel7.AutoSize = true;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x153, 0x49);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x99, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 0x16;
            this.gpgLabel7.Text = "<LOC>Opponent Faction";
            this.gpgLabel7.TextStyle = TextStyles.Title;
            this.comboBoxPlayerFaction.Anchor = AnchorStyles.Top;
            this.comboBoxPlayerFaction.BackColor = Color.Black;
            this.comboBoxPlayerFaction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPlayerFaction.Enabled = false;
            this.comboBoxPlayerFaction.FlatStyle = FlatStyle.Flat;
            this.comboBoxPlayerFaction.FormattingEnabled = true;
            this.comboBoxPlayerFaction.ItemHeight = 13;
            this.comboBoxPlayerFaction.Location = new Point(0xae, 0x5c);
            this.comboBoxPlayerFaction.MaxDropDownItems = 12;
            this.comboBoxPlayerFaction.Name = "comboBoxPlayerFaction";
            this.comboBoxPlayerFaction.Size = new Size(0x7a, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxPlayerFaction, null);
            this.comboBoxPlayerFaction.TabIndex = 0x15;
            this.gpgLabel6.Anchor = AnchorStyles.Top;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0xab, 0x48);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x86, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 20;
            this.gpgLabel6.Text = "<LOC>Player Faction";
            this.gpgLabel6.TextStyle = TextStyles.Title;
            this.comboBoxGameType.BackColor = Color.Black;
            this.comboBoxGameType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxGameType.FlatStyle = FlatStyle.Flat;
            this.comboBoxGameType.FormattingEnabled = true;
            this.comboBoxGameType.ItemHeight = 13;
            this.comboBoxGameType.Location = new Point(5, 0x5c);
            this.comboBoxGameType.MaxDropDownItems = 12;
            this.comboBoxGameType.Name = "comboBoxGameType";
            this.comboBoxGameType.Size = new Size(0x7a, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxGameType, null);
            this.comboBoxGameType.TabIndex = 0x13;
            this.comboBoxGameType.SelectedIndexChanged += new EventHandler(this.comboBoxGameType_SelectedIndexChanged);
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(2, 0x48);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x75, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x12;
            this.gpgLabel5.Text = "<LOC>Game Type";
            this.gpgLabel5.TextStyle = TextStyles.Title;
            this.comboBoxMaps.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.comboBoxMaps.BackColor = Color.Black;
            this.comboBoxMaps.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMaps.FlatStyle = FlatStyle.Flat;
            this.comboBoxMaps.FormattingEnabled = true;
            this.comboBoxMaps.ItemHeight = 13;
            this.comboBoxMaps.Location = new Point(0x11b, 0x2b);
            this.comboBoxMaps.MaxDropDownItems = 12;
            this.comboBoxMaps.Name = "comboBoxMaps";
            this.comboBoxMaps.Size = new Size(0xc2, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxMaps, null);
            this.comboBoxMaps.TabIndex = 0x11;
            this.gpgLabelMap.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabelMap.AutoSize = true;
            this.gpgLabelMap.AutoStyle = true;
            this.gpgLabelMap.Font = new Font("Arial", 9.75f);
            this.gpgLabelMap.ForeColor = Color.White;
            this.gpgLabelMap.IgnoreMouseWheel = false;
            this.gpgLabelMap.IsStyled = false;
            this.gpgLabelMap.Location = new Point(280, 0x1a);
            this.gpgLabelMap.Name = "gpgLabelMap";
            this.gpgLabelMap.Size = new Size(0x4b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelMap, null);
            this.gpgLabelMap.TabIndex = 0x10;
            this.gpgLabelMap.Text = "<LOC>Map";
            this.gpgLabelMap.TextStyle = TextStyles.Title;
            this.dateEditEarliest.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.dateEditEarliest.EditValue = "";
            this.dateEditEarliest.Location = new Point(0x1f0, 0x2d);
            this.dateEditEarliest.Name = "dateEditEarliest";
            this.dateEditEarliest.Properties.AllowNullInput = DefaultBoolean.False;
            this.dateEditEarliest.Properties.Appearance.BackColor = Color.Black;
            this.dateEditEarliest.Properties.Appearance.BorderColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.dateEditEarliest.Properties.Appearance.ForeColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.dateEditEarliest.Properties.Appearance.Options.UseBackColor = true;
            this.dateEditEarliest.Properties.Appearance.Options.UseBorderColor = true;
            this.dateEditEarliest.Properties.Appearance.Options.UseForeColor = true;
            this.dateEditEarliest.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.dateEditEarliest.Properties.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            this.dateEditEarliest.Properties.LookAndFeel.UseWindowsXPTheme = true;
            this.dateEditEarliest.Size = new Size(0xb1, 20);
            this.dateEditEarliest.TabIndex = 15;
            this.dateEditLatest.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.dateEditLatest.EditValue = "";
            this.dateEditLatest.Location = new Point(0x1f0, 0x5d);
            this.dateEditLatest.Name = "dateEditLatest";
            this.dateEditLatest.Properties.AllowNullInput = DefaultBoolean.False;
            this.dateEditLatest.Properties.Appearance.BackColor = Color.Black;
            this.dateEditLatest.Properties.Appearance.BorderColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.dateEditLatest.Properties.Appearance.ForeColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.dateEditLatest.Properties.Appearance.Options.UseBackColor = true;
            this.dateEditLatest.Properties.Appearance.Options.UseBorderColor = true;
            this.dateEditLatest.Properties.Appearance.Options.UseForeColor = true;
            this.dateEditLatest.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.dateEditLatest.Properties.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            this.dateEditLatest.Properties.LookAndFeel.UseWindowsXPTheme = true;
            this.dateEditLatest.Size = new Size(0xb1, 20);
            this.dateEditLatest.TabIndex = 14;
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x1ed, 0x49);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x49, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 13;
            this.gpgLabel4.Text = "<LOC>And";
            this.gpgLabel4.TextStyle = TextStyles.Title;
            this.gpgLabel3.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x1ed, 0x1a);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(100, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 11;
            this.gpgLabel3.Text = "<LOC>Between";
            this.gpgLabel3.TextStyle = TextStyles.Title;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(2, 0x7a);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x6b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "<LOC>Keywords";
            this.gpgLabel2.TextStyle = TextStyles.Title;
            this.gpgTextBoxPlayer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxPlayer.Location = new Point(5, 0x2c);
            this.gpgTextBoxPlayer.Name = "gpgTextBoxPlayer";
            this.gpgTextBoxPlayer.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPlayer.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPlayer.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPlayer.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPlayer.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPlayer.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPlayer.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPlayer.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPlayer.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPlayer.Size = new Size(0x105, 20);
            this.gpgTextBoxPlayer.TabIndex = 0;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(2, 0x1a);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x7d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>Player Name";
            this.gpgLabel1.TextStyle = TextStyles.Title;
            this.skinButtonSearch.Anchor = AnchorStyles.Right | AnchorStyles.Top;
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
            this.skinButtonSearch.Location = new Point(0x224, 0x87);
            this.skinButtonSearch.Name = "skinButtonSearch";
            this.skinButtonSearch.Size = new Size(0x7d, 0x1a);
            this.skinButtonSearch.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSearch, null);
            this.skinButtonSearch.TabIndex = 4;
            this.skinButtonSearch.Text = "<LOC>Search";
            this.skinButtonSearch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearch.TextPadding = new Padding(0);
            this.skinButtonSearch.Click += new EventHandler(this.skinButtonSearch_Click);
            this.gpgContextMenuDownload.MenuItems.AddRange(new MenuItem[] { this.ciDownload_View, this.ciDownload_Download });
            this.ciDownload_View.Index = 0;
            this.ciDownload_View.Text = "<LOC>View info";
            this.ciDownload_View.Click += new EventHandler(this.ciDownload_View_Click);
            this.ciDownload_Download.Index = 1;
            this.ciDownload_Download.Text = "<LOC>Download";
            this.ciDownload_Download.Click += new EventHandler(this.ciDownload_Download_Click);
            this.gpgLabelTopRated.AutoSize = true;
            this.gpgLabelTopRated.AutoStyle = true;
            this.gpgLabelTopRated.Font = new Font("Arial", 9.75f);
            this.gpgLabelTopRated.ForeColor = Color.White;
            this.gpgLabelTopRated.IgnoreMouseWheel = false;
            this.gpgLabelTopRated.IsStyled = false;
            this.gpgLabelTopRated.Location = new Point(3, 0);
            this.gpgLabelTopRated.Name = "gpgLabelTopRated";
            this.gpgLabelTopRated.Size = new Size(0x7f, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelTopRated, null);
            this.gpgLabelTopRated.TabIndex = 9;
            this.gpgLabelTopRated.Text = "<LOC>Top 10 Rated";
            this.gpgLabelTopRated.TextStyle = TextStyles.Link;
            this.gpgLabelTopRated.Visible = false;
            this.gpgLabelTopDownload.Anchor = AnchorStyles.Top;
            this.gpgLabelTopDownload.AutoSize = true;
            this.gpgLabelTopDownload.AutoStyle = true;
            this.gpgLabelTopDownload.Font = new Font("Arial", 9.75f);
            this.gpgLabelTopDownload.ForeColor = Color.White;
            this.gpgLabelTopDownload.IgnoreMouseWheel = false;
            this.gpgLabelTopDownload.IsStyled = false;
            this.gpgLabelTopDownload.Location = new Point(0xf3, 0);
            this.gpgLabelTopDownload.Name = "gpgLabelTopDownload";
            this.gpgLabelTopDownload.Size = new Size(0x9c, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelTopDownload, null);
            this.gpgLabelTopDownload.TabIndex = 10;
            this.gpgLabelTopDownload.Text = "<LOC>Top 10 Downloads";
            this.gpgLabelTopDownload.TextStyle = TextStyles.Link;
            this.gpgLabelTopDownload.Visible = false;
            this.gpgLabelMostRecent.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabelMostRecent.AutoSize = true;
            this.gpgLabelMostRecent.AutoStyle = true;
            this.gpgLabelMostRecent.Font = new Font("Arial", 9.75f);
            this.gpgLabelMostRecent.ForeColor = Color.White;
            this.gpgLabelMostRecent.IgnoreMouseWheel = false;
            this.gpgLabelMostRecent.IsStyled = false;
            this.gpgLabelMostRecent.Location = new Point(490, 0);
            this.gpgLabelMostRecent.Name = "gpgLabelMostRecent";
            this.gpgLabelMostRecent.Size = new Size(0xaf, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelMostRecent, null);
            this.gpgLabelMostRecent.TabIndex = 11;
            this.gpgLabelMostRecent.Text = "<LOC>Most Recent Replays";
            this.gpgLabelMostRecent.TextStyle = TextStyles.Link;
            this.gpgLabelMostRecent.Visible = false;
            this.gpgGroupBoxReplayLists.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgGroupBoxReplayLists.Controls.Add(this.gpgPanelReplayLists);
            this.gpgGroupBoxReplayLists.Location = new Point(12, 0x4c);
            this.gpgGroupBoxReplayLists.Name = "gpgGroupBoxReplayLists";
            this.gpgGroupBoxReplayLists.Size = new Size(680, 0x3d);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxReplayLists, null);
            this.gpgGroupBoxReplayLists.TabIndex = 12;
            this.gpgGroupBoxReplayLists.TabStop = false;
            this.gpgGroupBoxReplayLists.Text = "<LOC>Replay Lists";
            this.gpgPanelReplayLists.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanelReplayLists.AutoScroll = true;
            this.gpgPanelReplayLists.Controls.Add(this.gpgLabelTopRated);
            this.gpgPanelReplayLists.Controls.Add(this.gpgLabelMostRecent);
            this.gpgPanelReplayLists.Controls.Add(this.gpgLabelTopDownload);
            this.gpgPanelReplayLists.Location = new Point(5, 0x10);
            this.gpgPanelReplayLists.Name = "gpgPanelReplayLists";
            this.gpgPanelReplayLists.Size = new Size(0x29c, 0x27);
            base.ttDefault.SetSuperTip(this.gpgPanelReplayLists, null);
            this.gpgPanelReplayLists.TabIndex = 12;
            base.AcceptButton = this.skinButtonSearch;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2bf, 0x23e);
            base.Controls.Add(this.gpgGroupBoxReplayLists);
            base.Controls.Add(this.gpgGroupBoxCriterial);
            base.Controls.Add(this.gpgGroupBoxResults);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x283, 0x1e1);
            base.Name = "DlgSearchReplays";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Replay Vault";
            base.Controls.SetChildIndex(this.gpgGroupBoxResults, 0);
            base.Controls.SetChildIndex(this.gpgGroupBoxCriterial, 0);
            base.Controls.SetChildIndex(this.gpgGroupBoxReplayLists, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgGroupBoxResults.ResumeLayout(false);
            this.dgReplays.EndInit();
            this.gvReplays.EndInit();
            this.repositoryItemRatingStars.EndInit();
            this.repositoryItemChatLink.EndInit();
            this.gpgGroupBoxCriterial.ResumeLayout(false);
            this.gpgGroupBoxCriterial.PerformLayout();
            this.gpgTextBoxKeywords.Properties.EndInit();
            this.dateEditEarliest.Properties.EndInit();
            this.dateEditLatest.Properties.EndInit();
            this.gpgTextBoxPlayer.Properties.EndInit();
            this.gpgGroupBoxReplayLists.ResumeLayout(false);
            this.gpgPanelReplayLists.ResumeLayout(false);
            this.gpgPanelReplayLists.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadGlobalReplayLists()
        {
            EventHandler handler = null;
            MappedObjectList<ReplayList> objects = DataAccess.GetObjects<ReplayList>("GetGlobalReplayLists", new object[0]);
            int num = 0;
            int num2 = 0;
            int num3 = this.gpgLabelTopRated.Height + 4;
            foreach (ReplayList list2 in objects)
            {
                GPGLabel label = new GPGLabel();
                label.AutoSize = true;
                label.TextStyle = TextStyles.Link;
                label.Text = Loc.Get(list2.Title);
                label.Top = num2 * num3;
                if (num == 0)
                {
                    label.Left = this.gpgLabelTopRated.Left;
                    label.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                }
                else if (num == 1)
                {
                    label.Left = this.gpgLabelTopDownload.Left;
                    label.Anchor = AnchorStyles.Top;
                }
                else if (num == 2)
                {
                    label.Left = this.gpgLabelMostRecent.Left;
                    label.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                }
                label.Tag = list2;
                if (handler == null)
                {
                    handler = delegate (object s, EventArgs e) {
                        this.Page = 0;
                        this.CurrentList = (s as Control).Tag as ReplayList;
                        this.ExecuteList();
                    };
                }
                label.Click += handler;
                this.gpgPanelReplayLists.Controls.Add(label);
                num++;
                if (num > 2)
                {
                    num = 0;
                    num2++;
                }
            }
        }

        protected override void Localize()
        {
            base.Localize();
            this.repositoryItemChatLink.NullText = Loc.Get("<LOC>Link to Chat");
            this.gpgContextMenuDownload.Localize();
        }

        public void ResetForm()
        {
            this.gpgTextBoxPlayer.Text = "";
            this.gpgTextBoxKeywords.Text = "";
            this.comboBoxGameType.SelectedIndex = 0;
            this.comboBoxMaps.SelectedIndex = 0;
            this.comboBoxOpponentFaction.SelectedIndex = 0;
            this.comboBoxPlayerFaction.SelectedIndex = 0;
            this.dateEditLatest.DateTime = DateTime.Now.AddDays(1.0);
            this.dateEditEarliest.DateTime = this.dateEditLatest.DateTime.AddMonths(-1);
        }

        private void skinButtonLast_Click(object sender, EventArgs e)
        {
        }

        private void skinButtonNext_Click(object sender, EventArgs e)
        {
            if (this.CurrentDataSize >= MAX_PAGE_SIZE)
            {
                this.ChangePage(this.Page + 1);
            }
        }

        private void skinButtonPrevious_Click(object sender, EventArgs e)
        {
            if (this.Page > 0)
            {
                this.ChangePage(this.Page - 1);
            }
        }

        private void skinButtonSearch_Click(object sender, EventArgs e)
        {
            this.CurrentList = null;
            this.Page = 0;
            this.CurrentDataSize = 0;
            this.CurrentCriteria = null;
            if (this.ExecuteSearch() == 0)
            {
                this.dgReplays.DataSource = null;
                this.skinLabelPage.Text = Loc.Get("<LOC>No Results");
            }
        }

        private void skinButtonStart_Click(object sender, EventArgs e)
        {
            if (this.Page > 0)
            {
                this.ChangePage(0);
            }
        }

        private void ViewSelected()
        {
            try
            {
                if (this.gvReplays.GetSelectedRows().Length > 0)
                {
                    ReplayInfo row = this.gvReplays.GetRow(this.gvReplays.GetSelectedRows()[0]) as ReplayInfo;
                    if (ActiveReplayWindows.ContainsKey(row.Location))
                    {
                        if (ActiveReplayWindows[row.Location].Disposing || ActiveReplayWindows[row.Location].IsDisposed)
                        {
                            ActiveReplayWindows.Remove(row.Location);
                        }
                        else
                        {
                            ActiveReplayWindows[row.Location].BringToFront();
                            return;
                        }
                    }
                    DlgReplayInfo info2 = new DlgReplayInfo(row);
                    info2.FormClosing += new FormClosingEventHandler(this.dlg_FormClosing);
                    ActiveReplayWindows.Add(row.Location, info2);
                    info2.Show();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private static int MAX_PAGE_SIZE
        {
            get
            {
                return ConfigSettings.GetInt("ReplaySearchPageSize", 50);
            }
        }

        private class __AdditionalStrings
        {
            private string[] Strings = new string[] { "<LOC>Top Rated", "<LOC>Most Downloaded", "<LOC>Most Recent", "<LOC>Editors' Choice", "<LOC>Top 10 Players", "<LOC>Replays of the Week", "<LOC>Top Favorites", "<LOC>My Favorites", "<LOC>Most Linked", "<LOC>Most Discussed", "<LOC>Top UEF", "<LOC>Top Aeon", "<LOC>Top Cybran", "<LOC>Top Rated UEF", "<LOC>Top Rated Aeon", "<LOC>Top Rated Cybran" };
        }
    }
}

