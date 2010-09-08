namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using DevExpress.Utils;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgTournamentRegistration : DlgBase
    {
        public SkinButton btnManage;
        public SkinButton btnRegister;
        private GPGContextMenu cmDirector;
        private IContainer components;
        private GPGBorderPanel gpgBorderPanel1;
        private GPGBorderPanel gpgBorderPanel2;
        private GPGCalendar gpgCalendar1;
        private GPGLabel lAssistants;
        private GPGLabel lAssistantsCaption;
        private ListBox lbMaps;
        private GPGLabel lDirector;
        private GPGLabel lDirectorCaption;
        private GPGLabel lFaction;
        private TextBox lInfo;
        private GPGLabel lMap;
        private GPGLabel lNumberOfRounds;
        private GPGLabel lNumberOfRoundsCaption;
        private GPGLabel lRoundLength;
        private GPGLabel lRoundLengthCaption;
        private GPGLabel lSignups;
        private GPGLabel lSignupsCaption;
        private GPGLabel lStartTime;
        private GPGLabel lStartTimeCaption;
        private GPGLabel lStatus;
        private GPGLabel lStatusCaption;
        private SkinLabel lTournamentDetails;
        private GPGLabel lType;
        private GPGLabel lTypeCaption;
        private GPGLabel lWebsite;
        private GPGLabel lWebsiteCaption;
        private bool mCanRefresh;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private DataRecord mSelectedRecord;
        private GPGPictureBox pbFaction;
        private GPGPictureBox pbMap;
        private SkinLabel skinLabel1;

        public DlgTournamentRegistration()
        {
            this.mCanRefresh = true;
            this.mSelectedRecord = null;
            this.components = null;
            this.InitializeComponent();
            this.InitMonths();
            this.RefreshDates();
        }

        public DlgTournamentRegistration(int tournamentID)
        {
            this.mCanRefresh = true;
            this.mSelectedRecord = null;
            this.components = null;
            this.InitializeComponent();
            this.InitMonths();
            this.RefreshDates(tournamentID);
        }

        private void btnManage_Click_1(object sender, EventArgs e)
        {
            this.cmDirector.Show(this.btnManage, new Point(0, 0));
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            WaitCallback callback2 = null;
            if (((int) this.btnRegister.Tag) == 0)
            {
                if (this.mSelectedRecord != null)
                {
                    this.btnRegister.Enabled = false;
                    if (!this.CheckMap(this.mSelectedRecord["map"]))
                    {
                        DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>You are missing this map.  Please download it from the vault first."));
                    }
                    else
                    {
                        if (callBack == null)
                        {
                            callBack = delegate (object o) {
                                VGen0 method = null;
                                VGen0 gen3 = null;
                                DataList queryData = DataAccess.GetQueryData("Tournament Status Check", new object[] { this.mSelectedRecord["tournament_id"] });
                                if ((queryData.Count == 0) || (queryData[0][0].ToUpper() != "REGISTRATION"))
                                {
                                    if (method == null)
                                    {
                                        method = delegate {
                                            DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>This tournament has already started.  You can no longer join."));
                                        };
                                    }
                                    base.Invoke(method);
                                }
                                else
                                {
                                    if (ConfigSettings.GetBool("CheckCustomTournyFilters", true))
                                    {
                                        VGen0 gen = null;
                                        bool flag = false;
                                        string gamedesc = "";
                                        foreach (GameInformation information in GameInformation.Games)
                                        {
                                            if (information.GameID.ToString() == this.mSelectedRecord["gpgnet_game_id"])
                                            {
                                                gamedesc = information.GameDescription;
                                                if ((information.CDKey != "") || User.Current.IsAdmin)
                                                {
                                                    flag = true;
                                                }
                                            }
                                        }
                                        if (!flag)
                                        {
                                            if (gen == null)
                                            {
                                                gen = delegate {
                                                    DlgMessage.ShowDialog(this.MainForm, Loc.Get("<LOC>You are not eligible for this tournament.  This tournament is for " + gamedesc + " and you do not have a valid CD key for it."));
                                                };
                                            }
                                            base.Invoke(gen);
                                            return;
                                        }
                                        if (((this.mSelectedRecord["custom_filter_query_name"].Trim() != "") && (this.mSelectedRecord["custom_filter_query_name"].Trim().ToUpper() != "(NULL)")) && (DataAccess.GetQueryData(this.mSelectedRecord["custom_filter_query_name"], new object[0]).Count == 0))
                                        {
                                            if (gen3 == null)
                                            {
                                                gen3 = delegate {
                                                    DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>You are not eligible for this tournament.  Please check the tournament details for eligibility."));
                                                };
                                            }
                                            base.Invoke(gen3);
                                            return;
                                        }
                                    }
                                    DataAccess.ExecuteQuery("Tournament Register", new object[] { this.mSelectedRecord["tournament_id"] });
                                    DataAccess.ExecuteQuery("Tournament Player Count", new object[] { this.mSelectedRecord["tournament_id"] });
                                    base.Invoke(delegate {
                                        this.mSelectedRecord.SetValue("registered", "1");
                                        int result = 0;
                                        if (int.TryParse(this.mSelectedRecord["player_count"], out result))
                                        {
                                            this.mSelectedRecord.SetValue("player_count", (result + 1).ToString());
                                        }
                                        else
                                        {
                                            this.mSelectedRecord.SetValue("player_count", "1");
                                        }
                                        this.btnRegister.Enabled = true;
                                        this.CheckSelection();
                                        this.CheckUnderlines();
                                    });
                                }
                            };
                        }
                        ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                    }
                }
            }
            else if (((int) this.btnRegister.Tag) == 1)
            {
                if (this.mSelectedRecord != null)
                {
                    this.btnRegister.Enabled = false;
                    if (callback2 == null)
                    {
                        callback2 = delegate (object o) {
                            VGen0 method = null;
                            DataList queryData = DataAccess.GetQueryData("Tournament Status Check", new object[] { this.mSelectedRecord["tournament_id"] });
                            if ((queryData.Count == 0) || (queryData[0][0].ToUpper() != "REGISTRATION"))
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>This tournament has already started.  You can no longer join."));
                                    };
                                }
                                base.Invoke(method);
                            }
                            else
                            {
                                DataAccess.ExecuteQuery("Tournament Unregister", new object[] { this.mSelectedRecord["tournament_id"] });
                                DataAccess.ExecuteQuery("Tournament Player Count", new object[] { this.mSelectedRecord["tournament_id"] });
                                base.Invoke(delegate {
                                    this.mSelectedRecord.SetValue("registered", "0");
                                    int result = 0;
                                    if (int.TryParse(this.mSelectedRecord["player_count"], out result))
                                    {
                                        this.mSelectedRecord.SetValue("player_count", (result - 1).ToString());
                                    }
                                    else
                                    {
                                        this.mSelectedRecord.SetValue("player_count", "0");
                                    }
                                    this.btnRegister.Enabled = true;
                                    this.CheckSelection();
                                    this.CheckUnderlines();
                                });
                            }
                        };
                    }
                    ThreadQueue.QueueUserWorkItem(callback2, new object[0]);
                }
            }
            else if (((int) this.btnRegister.Tag) == 3)
            {
                new DlgTournamentResults(Convert.ToInt32(this.mSelectedRecord["tournament_id"])).Show();
            }
            else if (this.mSelectedRecord != null)
            {
                DlgTournamentCreation creation = new DlgTournamentCreation();
                creation.MainForm = base.MainForm;
                creation.LoadTournament(Convert.ToInt32(this.mSelectedRecord["tournament_id"]));
                creation.FormClosed += new FormClosedEventHandler(this.dlg_FormClosed);
                creation.Show();
            }
        }

        private bool CheckMap(string checkmap)
        {
            if (checkmap.ToUpper() == "LADDER MAPS")
            {
                return true;
            }
            foreach (SupcomMap map in SupcomMapList.Maps)
            {
                if (map.MapName.ToUpper() == checkmap.ToUpper())
                {
                    return true;
                }
                if (map.MapID.ToUpper() == checkmap.ToUpper())
                {
                    return true;
                }
                if (map.Path.ToUpper() == checkmap.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckSelectedItems()
        {
            this.ClearDetails();
            this.lbMaps.Items.Clear();
            foreach (CalendarDate date in this.gpgCalendar1.SelectedDates)
            {
                try
                {
                    this.lbMaps.Items.Add(date);
                    if (this.lbMaps.SelectedItem == null)
                    {
                        this.lbMaps.SelectedIndex = 0;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void CheckSelection()
        {
            WaitCallback callBack = null;
            this.ClearDetails();
            if (this.lbMaps.SelectedItem != null)
            {
                this.btnRegister.Enabled = true;
                CalendarDate selectedItem = this.lbMaps.SelectedItem as CalendarDate;
                DataRecord state = selectedItem.Value as DataRecord;
                this.mSelectedRecord = state;
                this.SetButtonStatus();
                this.lTournamentDetails.Text = Loc.Get("<LOC>Tournament Details: " + state["name"]);
                this.lInfo.Text = state["description"] + "\r\n\r\n" + state["legal"];
                this.lSignups.Text = state["player_count"];
                this.lStatus.Text = state["status"];
                this.lAssistants.Text = "";
                foreach (DataRecord record2 in selectedItem.Tag)
                {
                    this.lAssistants.Text = this.lAssistants.Text + record2["name"] + " ";
                }
                if (this.lInfo.Text.Length > 100)
                {
                    this.lInfo.ScrollBars = ScrollBars.Vertical;
                }
                else
                {
                    this.lInfo.ScrollBars = ScrollBars.None;
                }
                this.lStartTime.Text = selectedItem.Date.ToShortTimeString();
                this.lType.Text = state["type"];
                this.lDirector.Text = state["td_name"];
                this.lWebsite.Text = state["url"];
                if (this.lWebsite.Text.ToUpper().IndexOf("HTTP://") != 0)
                {
                    this.lWebsite.Text = "http://" + this.lWebsite.Text;
                }
                this.lSignups.ForeColor = this.lWebsite.ForeColor;
                this.lSignupsCaption.ForeColor = this.lWebsite.ForeColor;
                if (Convert.ToInt32(state["round_length"]) > 0)
                {
                    this.lRoundLength.Text = state["round_length"] + " minutes";
                }
                else
                {
                    this.lRoundLength.Text = Loc.Get("<LOC>Until all games are finished");
                }
                if (Convert.ToInt32(state["round_count"]) > 0)
                {
                    this.lNumberOfRounds.Text = state["round_count"];
                }
                else
                {
                    this.lNumberOfRounds.Text = Loc.Get("<LOC>Automatic");
                }
                if (state["faction"].ToUpper() == "AEON")
                {
                    this.pbFaction.Image = DlgTournamentCreation.mAeonImage;
                }
                else if (state["faction"].ToUpper() == "CYBRAN")
                {
                    this.pbFaction.Image = DlgTournamentCreation.mCybranImage;
                }
                else if (state["faction"].ToUpper() == "UEF")
                {
                    this.pbFaction.Image = DlgTournamentCreation.mUEFImage;
                }
                else
                {
                    this.pbFaction.Image = DlgTournamentCreation.mAnyImage;
                }
                this.lFaction.Text = state["faction"];
                this.pbMap.Image = DlgTournamentCreation.mAnyMap;
                this.lMap.Text = state["map"];
                if (callBack == null)
                {
                    callBack = delegate (object o) {
                        DataRecord record = o as DataRecord;
                        try
                        {
                            using (List<SupcomMap>.Enumerator enumerator = SupcomMapList.Maps.GetEnumerator())
                            {
                                VGen0 method = null;
                                while (enumerator.MoveNext())
                                {
                                    SupcomMap map = enumerator.Current;
                                    if (map.ToString() == record["map"])
                                    {
                                        if (method == null)
                                        {
                                            method = delegate {
                                                this.pbMap.Image = map.Image;
                                                this.lMap.Text = map.MapName;
                                            };
                                        }
                                        base.Invoke(method);
                                    }
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack, state);
            }
        }

        private void CheckUnderlines()
        {
            foreach (CalendarDate date in this.gpgCalendar1.Dates)
            {
                DataRecord record = date.Value as DataRecord;
                if (record["registered"] == "1")
                {
                    date.HighlightColor = Color.FromArgb(0x7d, 0x7d, 0xff);
                }
                else
                {
                    date.HighlightColor = Color.Empty;
                }
            }
            this.gpgCalendar1.Invalidate();
        }

        private void ClearDetails()
        {
            this.btnManage.Visible = false;
            this.btnRegister.Tag = 0;
            this.mSelectedRecord = null;
            this.btnRegister.Enabled = false;
            this.lInfo.Text = "";
            this.lStartTime.Text = "";
            this.lType.Text = "";
            this.lDirector.Text = "";
            this.lWebsite.Text = "";
            this.lRoundLength.Text = "";
            this.lNumberOfRounds.Text = "";
            this.lAssistants.Text = "";
            this.lStatus.Text = "";
            this.lSignups.Text = "";
            this.pbFaction.Image = null;
            this.pbMap.Image = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.RefreshDates();
        }

        private void gpgCalendar1_Click(object sender, EventArgs e)
        {
            this.CheckSelectedItems();
        }

        private void gpgCalendar1_OnRefreshClick(object sender, EventArgs e)
        {
            if (this.mSelectedRecord != null)
            {
                this.RefreshDates(Convert.ToInt32(this.mSelectedRecord["tournament_id"]));
            }
            else
            {
                this.RefreshDates();
            }
        }

        private void InitializeComponent()
        {
            this.gpgCalendar1 = new GPGCalendar();
            this.gpgBorderPanel1 = new GPGBorderPanel();
            this.gpgBorderPanel2 = new GPGBorderPanel();
            this.lbMaps = new ListBox();
            this.skinLabel1 = new SkinLabel();
            this.lTournamentDetails = new SkinLabel();
            this.lStartTimeCaption = new GPGLabel();
            this.lTypeCaption = new GPGLabel();
            this.lDirectorCaption = new GPGLabel();
            this.lWebsiteCaption = new GPGLabel();
            this.lStartTime = new GPGLabel();
            this.lType = new GPGLabel();
            this.lDirector = new GPGLabel();
            this.lWebsite = new GPGLabel();
            this.lAssistants = new GPGLabel();
            this.lNumberOfRounds = new GPGLabel();
            this.lRoundLength = new GPGLabel();
            this.lAssistantsCaption = new GPGLabel();
            this.lNumberOfRoundsCaption = new GPGLabel();
            this.lRoundLengthCaption = new GPGLabel();
            this.btnRegister = new SkinButton();
            this.pbFaction = new GPGPictureBox();
            this.pbMap = new GPGPictureBox();
            this.lInfo = new TextBox();
            this.lFaction = new GPGLabel();
            this.lMap = new GPGLabel();
            this.lSignups = new GPGLabel();
            this.lSignupsCaption = new GPGLabel();
            this.lStatus = new GPGLabel();
            this.lStatusCaption = new GPGLabel();
            this.btnManage = new SkinButton();
            this.cmDirector = new GPGContextMenu();
            this.menuItem1 = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.menuItem3 = new MenuItem();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            ((ISupportInitialize) this.pbFaction).BeginInit();
            ((ISupportInitialize) this.pbMap).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x349, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgCalendar1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgCalendar1.BackgroundImage = Resources._06_jun;
            this.gpgCalendar1.LeftMonthButton = 0;
            this.gpgCalendar1.Location = new Point(12, 0x3f);
            this.gpgCalendar1.Month = 5;
            this.gpgCalendar1.Name = "gpgCalendar1";
            this.gpgCalendar1.RefreshButton = 0;
            this.gpgCalendar1.RightMonthButton = 0;
            this.gpgCalendar1.Size = new Size(0x36d, 0x143);
            base.ttDefault.SetSuperTip(this.gpgCalendar1, null);
            this.gpgCalendar1.TabIndex = 7;
            this.gpgCalendar1.Text = "gpgCalendar1";
            this.gpgCalendar1.OnRefreshClick += new EventHandler(this.gpgCalendar1_OnRefreshClick);
            this.gpgCalendar1.Click += new EventHandler(this.gpgCalendar1_Click);
            this.gpgBorderPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgBorderPanel1.GPGBorderStyle = GPGBorderStyle.Web;
            this.gpgBorderPanel1.Location = new Point(0x144, 0x188);
            this.gpgBorderPanel1.Name = "gpgBorderPanel1";
            this.gpgBorderPanel1.Size = new Size(0x234, 160);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel1, null);
            this.gpgBorderPanel1.TabIndex = 8;
            this.gpgBorderPanel1.Text = "gpgBorderPanel1";
            this.gpgBorderPanel2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgBorderPanel2.GPGBorderStyle = GPGBorderStyle.Rectangle;
            this.gpgBorderPanel2.Location = new Point(12, 0x188);
            this.gpgBorderPanel2.Name = "gpgBorderPanel2";
            this.gpgBorderPanel2.Size = new Size(0x132, 160);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel2, null);
            this.gpgBorderPanel2.TabIndex = 9;
            this.gpgBorderPanel2.Text = "gpgBorderPanel2";
            this.lbMaps.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lbMaps.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.lbMaps.BorderStyle = BorderStyle.None;
            this.lbMaps.ForeColor = Color.White;
            this.lbMaps.FormattingEnabled = true;
            this.lbMaps.Location = new Point(14, 0x19f);
            this.lbMaps.Name = "lbMaps";
            this.lbMaps.Size = new Size(0x12e, 130);
            base.ttDefault.SetSuperTip(this.lbMaps, null);
            this.lbMaps.TabIndex = 10;
            this.lbMaps.SelectedValueChanged += new EventHandler(this.lbMaps_SelectedValueChanged);
            this.skinLabel1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(14, 0x18a);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0x12e, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 11;
            this.skinLabel1.Text = "<LOC>Tournaments";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(0);
            this.lTournamentDetails.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lTournamentDetails.AutoStyle = false;
            this.lTournamentDetails.BackColor = Color.Transparent;
            this.lTournamentDetails.DrawEdges = true;
            this.lTournamentDetails.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.lTournamentDetails.ForeColor = Color.White;
            this.lTournamentDetails.HorizontalScalingMode = ScalingModes.Tile;
            this.lTournamentDetails.IsStyled = false;
            this.lTournamentDetails.Location = new Point(330, 0x18d);
            this.lTournamentDetails.Name = "lTournamentDetails";
            this.lTournamentDetails.Size = new Size(0x228, 20);
            this.lTournamentDetails.SkinBasePath = @"Controls\Background Label\Round Top";
            base.ttDefault.SetSuperTip(this.lTournamentDetails, null);
            this.lTournamentDetails.TabIndex = 12;
            this.lTournamentDetails.Text = "<LOC>Tournament Details:";
            this.lTournamentDetails.TextAlign = ContentAlignment.MiddleLeft;
            this.lTournamentDetails.TextPadding = new Padding(0);
            this.lStartTimeCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lStartTimeCaption.AutoGrowDirection = GrowDirections.None;
            this.lStartTimeCaption.AutoSize = true;
            this.lStartTimeCaption.AutoStyle = true;
            this.lStartTimeCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lStartTimeCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lStartTimeCaption.IgnoreMouseWheel = false;
            this.lStartTimeCaption.IsStyled = false;
            this.lStartTimeCaption.Location = new Point(0x14b, 0x1e4);
            this.lStartTimeCaption.Name = "lStartTimeCaption";
            this.lStartTimeCaption.Size = new Size(0x75, 13);
            base.ttDefault.SetSuperTip(this.lStartTimeCaption, null);
            this.lStartTimeCaption.TabIndex = 14;
            this.lStartTimeCaption.Text = "<LOC>Start Time";
            this.lStartTimeCaption.TextAlign = ContentAlignment.TopRight;
            this.lStartTimeCaption.TextStyle = TextStyles.ColoredBold;
            this.lTypeCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lTypeCaption.AutoGrowDirection = GrowDirections.None;
            this.lTypeCaption.AutoSize = true;
            this.lTypeCaption.AutoStyle = true;
            this.lTypeCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lTypeCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lTypeCaption.IgnoreMouseWheel = false;
            this.lTypeCaption.IsStyled = false;
            this.lTypeCaption.Location = new Point(0x14b, 0x1f1);
            this.lTypeCaption.Name = "lTypeCaption";
            this.lTypeCaption.Size = new Size(0x51, 13);
            base.ttDefault.SetSuperTip(this.lTypeCaption, null);
            this.lTypeCaption.TabIndex = 15;
            this.lTypeCaption.Text = "<LOC>Type";
            this.lTypeCaption.TextAlign = ContentAlignment.TopRight;
            this.lTypeCaption.TextStyle = TextStyles.ColoredBold;
            this.lDirectorCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lDirectorCaption.AutoGrowDirection = GrowDirections.None;
            this.lDirectorCaption.AutoSize = true;
            this.lDirectorCaption.AutoStyle = true;
            this.lDirectorCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lDirectorCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lDirectorCaption.IgnoreMouseWheel = false;
            this.lDirectorCaption.IsStyled = false;
            this.lDirectorCaption.Location = new Point(0x14b, 510);
            this.lDirectorCaption.Name = "lDirectorCaption";
            this.lDirectorCaption.Size = new Size(0x66, 13);
            base.ttDefault.SetSuperTip(this.lDirectorCaption, null);
            this.lDirectorCaption.TabIndex = 0x10;
            this.lDirectorCaption.Text = "<LOC>Director";
            this.lDirectorCaption.TextAlign = ContentAlignment.TopRight;
            this.lDirectorCaption.TextStyle = TextStyles.ColoredBold;
            this.lWebsiteCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lWebsiteCaption.AutoGrowDirection = GrowDirections.None;
            this.lWebsiteCaption.AutoSize = true;
            this.lWebsiteCaption.AutoStyle = true;
            this.lWebsiteCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lWebsiteCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lWebsiteCaption.IgnoreMouseWheel = false;
            this.lWebsiteCaption.IsStyled = false;
            this.lWebsiteCaption.Location = new Point(0x14b, 0x218);
            this.lWebsiteCaption.Name = "lWebsiteCaption";
            this.lWebsiteCaption.Size = new Size(0x65, 13);
            base.ttDefault.SetSuperTip(this.lWebsiteCaption, null);
            this.lWebsiteCaption.TabIndex = 0x11;
            this.lWebsiteCaption.Text = "<LOC>Website";
            this.lWebsiteCaption.TextAlign = ContentAlignment.TopRight;
            this.lWebsiteCaption.TextStyle = TextStyles.ColoredBold;
            this.lStartTime.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lStartTime.AutoGrowDirection = GrowDirections.None;
            this.lStartTime.AutoSize = true;
            this.lStartTime.AutoStyle = true;
            this.lStartTime.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lStartTime.ForeColor = Color.White;
            this.lStartTime.IgnoreMouseWheel = false;
            this.lStartTime.IsStyled = false;
            this.lStartTime.Location = new Point(0x1bd, 0x1e4);
            this.lStartTime.Name = "lStartTime";
            this.lStartTime.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lStartTime, null);
            this.lStartTime.TabIndex = 0x12;
            this.lStartTime.Text = "-";
            this.lStartTime.TextAlign = ContentAlignment.TopRight;
            this.lStartTime.TextStyle = TextStyles.Default;
            this.lType.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lType.AutoGrowDirection = GrowDirections.None;
            this.lType.AutoSize = true;
            this.lType.AutoStyle = true;
            this.lType.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lType.ForeColor = Color.White;
            this.lType.IgnoreMouseWheel = false;
            this.lType.IsStyled = false;
            this.lType.Location = new Point(0x1a2, 0x1f1);
            this.lType.Name = "lType";
            this.lType.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lType, null);
            this.lType.TabIndex = 0x13;
            this.lType.Text = "-";
            this.lType.TextAlign = ContentAlignment.TopRight;
            this.lType.TextStyle = TextStyles.Default;
            this.lDirector.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lDirector.AutoGrowDirection = GrowDirections.None;
            this.lDirector.AutoSize = true;
            this.lDirector.AutoStyle = true;
            this.lDirector.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lDirector.ForeColor = Color.White;
            this.lDirector.IgnoreMouseWheel = false;
            this.lDirector.IsStyled = false;
            this.lDirector.Location = new Point(0x1b4, 510);
            this.lDirector.Name = "lDirector";
            this.lDirector.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lDirector, null);
            this.lDirector.TabIndex = 20;
            this.lDirector.Text = "-";
            this.lDirector.TextAlign = ContentAlignment.TopRight;
            this.lDirector.TextStyle = TextStyles.Default;
            this.lWebsite.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lWebsite.AutoGrowDirection = GrowDirections.None;
            this.lWebsite.AutoSize = true;
            this.lWebsite.AutoStyle = true;
            this.lWebsite.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lWebsite.ForeColor = Color.White;
            this.lWebsite.IgnoreMouseWheel = false;
            this.lWebsite.IsStyled = false;
            this.lWebsite.Location = new Point(0x1b4, 0x219);
            this.lWebsite.Name = "lWebsite";
            this.lWebsite.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lWebsite, null);
            this.lWebsite.TabIndex = 0x15;
            this.lWebsite.Text = "-";
            this.lWebsite.TextAlign = ContentAlignment.TopRight;
            this.lWebsite.TextStyle = TextStyles.Link;
            this.lWebsite.Click += new EventHandler(this.lWebsite_Click);
            this.lAssistants.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lAssistants.AutoGrowDirection = GrowDirections.None;
            this.lAssistants.AutoSize = true;
            this.lAssistants.AutoStyle = true;
            this.lAssistants.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lAssistants.ForeColor = Color.White;
            this.lAssistants.IgnoreMouseWheel = false;
            this.lAssistants.IsStyled = false;
            this.lAssistants.Location = new Point(0x253, 510);
            this.lAssistants.Name = "lAssistants";
            this.lAssistants.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lAssistants, null);
            this.lAssistants.TabIndex = 0x1b;
            this.lAssistants.Text = "-";
            this.lAssistants.TextAlign = ContentAlignment.TopRight;
            this.lAssistants.TextStyle = TextStyles.Default;
            this.lNumberOfRounds.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lNumberOfRounds.AutoGrowDirection = GrowDirections.None;
            this.lNumberOfRounds.AutoSize = true;
            this.lNumberOfRounds.AutoStyle = true;
            this.lNumberOfRounds.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lNumberOfRounds.ForeColor = Color.White;
            this.lNumberOfRounds.IgnoreMouseWheel = false;
            this.lNumberOfRounds.IsStyled = false;
            this.lNumberOfRounds.Location = new Point(0x28e, 0x1f1);
            this.lNumberOfRounds.Name = "lNumberOfRounds";
            this.lNumberOfRounds.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lNumberOfRounds, null);
            this.lNumberOfRounds.TabIndex = 0x1a;
            this.lNumberOfRounds.Text = "-";
            this.lNumberOfRounds.TextAlign = ContentAlignment.TopRight;
            this.lNumberOfRounds.TextStyle = TextStyles.Default;
            this.lRoundLength.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lRoundLength.AutoGrowDirection = GrowDirections.None;
            this.lRoundLength.AutoSize = true;
            this.lRoundLength.AutoStyle = true;
            this.lRoundLength.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lRoundLength.ForeColor = Color.White;
            this.lRoundLength.IgnoreMouseWheel = false;
            this.lRoundLength.IsStyled = false;
            this.lRoundLength.Location = new Point(0x26f, 0x1e4);
            this.lRoundLength.Name = "lRoundLength";
            this.lRoundLength.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lRoundLength, null);
            this.lRoundLength.TabIndex = 0x19;
            this.lRoundLength.Text = "-";
            this.lRoundLength.TextAlign = ContentAlignment.TopRight;
            this.lRoundLength.TextStyle = TextStyles.Default;
            this.lAssistantsCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lAssistantsCaption.AutoGrowDirection = GrowDirections.None;
            this.lAssistantsCaption.AutoSize = true;
            this.lAssistantsCaption.AutoStyle = true;
            this.lAssistantsCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lAssistantsCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lAssistantsCaption.IgnoreMouseWheel = false;
            this.lAssistantsCaption.IsStyled = false;
            this.lAssistantsCaption.Location = new Point(480, 510);
            this.lAssistantsCaption.Name = "lAssistantsCaption";
            this.lAssistantsCaption.Size = new Size(0x74, 13);
            base.ttDefault.SetSuperTip(this.lAssistantsCaption, null);
            this.lAssistantsCaption.TabIndex = 0x18;
            this.lAssistantsCaption.Text = "<LOC>Assistants";
            this.lAssistantsCaption.TextAlign = ContentAlignment.TopRight;
            this.lAssistantsCaption.TextStyle = TextStyles.ColoredBold;
            this.lNumberOfRoundsCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lNumberOfRoundsCaption.AutoGrowDirection = GrowDirections.None;
            this.lNumberOfRoundsCaption.AutoSize = true;
            this.lNumberOfRoundsCaption.AutoStyle = true;
            this.lNumberOfRoundsCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lNumberOfRoundsCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lNumberOfRoundsCaption.IgnoreMouseWheel = false;
            this.lNumberOfRoundsCaption.IsStyled = false;
            this.lNumberOfRoundsCaption.Location = new Point(480, 0x1f1);
            this.lNumberOfRoundsCaption.Name = "lNumberOfRoundsCaption";
            this.lNumberOfRoundsCaption.Size = new Size(0xa8, 13);
            base.ttDefault.SetSuperTip(this.lNumberOfRoundsCaption, null);
            this.lNumberOfRoundsCaption.TabIndex = 0x17;
            this.lNumberOfRoundsCaption.Text = "<LOC>Number of Rounds";
            this.lNumberOfRoundsCaption.TextAlign = ContentAlignment.TopRight;
            this.lNumberOfRoundsCaption.TextStyle = TextStyles.ColoredBold;
            this.lRoundLengthCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lRoundLengthCaption.AutoGrowDirection = GrowDirections.None;
            this.lRoundLengthCaption.AutoSize = true;
            this.lRoundLengthCaption.AutoStyle = true;
            this.lRoundLengthCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lRoundLengthCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lRoundLengthCaption.IgnoreMouseWheel = false;
            this.lRoundLengthCaption.IsStyled = false;
            this.lRoundLengthCaption.Location = new Point(480, 0x1e4);
            this.lRoundLengthCaption.Name = "lRoundLengthCaption";
            this.lRoundLengthCaption.Size = new Size(0x89, 13);
            base.ttDefault.SetSuperTip(this.lRoundLengthCaption, null);
            this.lRoundLengthCaption.TabIndex = 0x16;
            this.lRoundLengthCaption.Text = "<LOC>Round Length";
            this.lRoundLengthCaption.TextAlign = ContentAlignment.TopRight;
            this.lRoundLengthCaption.TextStyle = TextStyles.ColoredBold;
            this.btnRegister.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnRegister.AutoStyle = true;
            this.btnRegister.BackColor = Color.Black;
            this.btnRegister.ButtonState = 0;
            this.btnRegister.DialogResult = DialogResult.None;
            this.btnRegister.DisabledForecolor = Color.Gray;
            this.btnRegister.DrawColor = Color.White;
            this.btnRegister.DrawEdges = true;
            this.btnRegister.FocusColor = Color.Yellow;
            this.btnRegister.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnRegister.ForeColor = Color.White;
            this.btnRegister.HorizontalScalingMode = ScalingModes.Tile;
            this.btnRegister.IsStyled = true;
            this.btnRegister.Location = new Point(0x2f5, 0x1fd);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new Size(0x7d, 0x16);
            this.btnRegister.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnRegister, null);
            this.btnRegister.TabIndex = 0x1c;
            this.btnRegister.TabStop = true;
            this.btnRegister.Text = "<LOC>Register";
            this.btnRegister.TextAlign = ContentAlignment.MiddleCenter;
            this.btnRegister.TextPadding = new Padding(0);
            this.btnRegister.Click += new EventHandler(this.btnRegister_Click);
            this.pbFaction.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.pbFaction.Location = new Point(0x2f5, 420);
            this.pbFaction.Name = "pbFaction";
            this.pbFaction.Size = new Size(50, 50);
            this.pbFaction.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbFaction, null);
            this.pbFaction.TabIndex = 0x1d;
            this.pbFaction.TabStop = false;
            this.pbMap.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.pbMap.Location = new Point(0x33d, 0x1a5);
            this.pbMap.Name = "pbMap";
            this.pbMap.Size = new Size(50, 50);
            this.pbMap.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbMap, null);
            this.pbMap.TabIndex = 30;
            this.pbMap.TabStop = false;
            this.lInfo.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lInfo.BackColor = Color.Black;
            this.lInfo.BorderStyle = BorderStyle.None;
            this.lInfo.ForeColor = Color.White;
            this.lInfo.Location = new Point(0x14e, 0x1a7);
            this.lInfo.Multiline = true;
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new Size(0x19e, 60);
            base.ttDefault.SetSuperTip(this.lInfo, null);
            this.lInfo.TabIndex = 0x1f;
            this.lFaction.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.lFaction.AutoGrowDirection = GrowDirections.None;
            this.lFaction.AutoStyle = true;
            this.lFaction.Font = new Font("Arial", 8f);
            this.lFaction.ForeColor = Color.White;
            this.lFaction.IgnoreMouseWheel = false;
            this.lFaction.IsStyled = false;
            this.lFaction.Location = new Point(0x2f5, 0x1d7);
            this.lFaction.Name = "lFaction";
            this.lFaction.Size = new Size(50, 0x33);
            base.ttDefault.SetSuperTip(this.lFaction, null);
            this.lFaction.TabIndex = 0x20;
            this.lFaction.Text = "Any Faction";
            this.lFaction.TextAlign = ContentAlignment.TopCenter;
            this.lFaction.TextStyle = TextStyles.Small;
            this.lMap.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.lMap.AutoGrowDirection = GrowDirections.None;
            this.lMap.AutoStyle = true;
            this.lMap.Font = new Font("Arial", 8f);
            this.lMap.ForeColor = Color.White;
            this.lMap.IgnoreMouseWheel = false;
            this.lMap.IsStyled = false;
            this.lMap.Location = new Point(0x338, 0x1d5);
            this.lMap.Name = "lMap";
            this.lMap.Size = new Size(60, 0x35);
            base.ttDefault.SetSuperTip(this.lMap, null);
            this.lMap.TabIndex = 0x21;
            this.lMap.Text = "Any Ladder map";
            this.lMap.TextAlign = ContentAlignment.TopCenter;
            this.lMap.TextStyle = TextStyles.Small;
            this.lSignups.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lSignups.AutoGrowDirection = GrowDirections.None;
            this.lSignups.AutoSize = true;
            this.lSignups.AutoStyle = true;
            this.lSignups.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lSignups.ForeColor = Color.White;
            this.lSignups.IgnoreMouseWheel = false;
            this.lSignups.IsStyled = false;
            this.lSignups.Location = new Point(0x253, 0x20b);
            this.lSignups.Name = "lSignups";
            this.lSignups.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lSignups, null);
            this.lSignups.TabIndex = 0x25;
            this.lSignups.Text = "-";
            this.lSignups.TextAlign = ContentAlignment.TopRight;
            this.lSignups.TextStyle = TextStyles.Default;
            this.lSignups.Click += new EventHandler(this.lSignupsCaption_Click);
            this.lSignupsCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lSignupsCaption.AutoGrowDirection = GrowDirections.None;
            this.lSignupsCaption.AutoSize = true;
            this.lSignupsCaption.AutoStyle = true;
            this.lSignupsCaption.Font = new Font("Verdana", 8.25f, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lSignupsCaption.ForeColor = Color.Red;
            this.lSignupsCaption.IgnoreMouseWheel = false;
            this.lSignupsCaption.IsStyled = false;
            this.lSignupsCaption.Location = new Point(480, 0x20b);
            this.lSignupsCaption.Name = "lSignupsCaption";
            this.lSignupsCaption.Size = new Size(100, 13);
            base.ttDefault.SetSuperTip(this.lSignupsCaption, null);
            this.lSignupsCaption.TabIndex = 0x24;
            this.lSignupsCaption.Text = "<LOC>Signups";
            this.lSignupsCaption.TextAlign = ContentAlignment.TopRight;
            this.lSignupsCaption.TextStyle = TextStyles.ColoredBold;
            this.lSignupsCaption.Click += new EventHandler(this.lSignupsCaption_Click);
            this.lStatus.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lStatus.AutoGrowDirection = GrowDirections.None;
            this.lStatus.AutoSize = true;
            this.lStatus.AutoStyle = true;
            this.lStatus.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lStatus.ForeColor = Color.White;
            this.lStatus.IgnoreMouseWheel = false;
            this.lStatus.IsStyled = false;
            this.lStatus.Location = new Point(0x1b4, 0x20b);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new Size(12, 13);
            base.ttDefault.SetSuperTip(this.lStatus, null);
            this.lStatus.TabIndex = 0x23;
            this.lStatus.Text = "-";
            this.lStatus.TextAlign = ContentAlignment.TopRight;
            this.lStatus.TextStyle = TextStyles.Default;
            this.lStatusCaption.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lStatusCaption.AutoGrowDirection = GrowDirections.None;
            this.lStatusCaption.AutoSize = true;
            this.lStatusCaption.AutoStyle = true;
            this.lStatusCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lStatusCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lStatusCaption.IgnoreMouseWheel = false;
            this.lStatusCaption.IsStyled = false;
            this.lStatusCaption.Location = new Point(0x14b, 0x20b);
            this.lStatusCaption.Name = "lStatusCaption";
            this.lStatusCaption.Size = new Size(90, 13);
            base.ttDefault.SetSuperTip(this.lStatusCaption, null);
            this.lStatusCaption.TabIndex = 0x22;
            this.lStatusCaption.Text = "<LOC>Status";
            this.lStatusCaption.TextAlign = ContentAlignment.TopRight;
            this.lStatusCaption.TextStyle = TextStyles.ColoredBold;
            this.btnManage.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnManage.AutoStyle = true;
            this.btnManage.BackColor = Color.Black;
            this.btnManage.ButtonState = 0;
            this.btnManage.DialogResult = DialogResult.None;
            this.btnManage.DisabledForecolor = Color.Gray;
            this.btnManage.DrawColor = Color.White;
            this.btnManage.DrawEdges = true;
            this.btnManage.FocusColor = Color.Yellow;
            this.btnManage.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnManage.ForeColor = Color.White;
            this.btnManage.HorizontalScalingMode = ScalingModes.Tile;
            this.btnManage.IsStyled = true;
            this.btnManage.Location = new Point(0x272, 0x1fd);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new Size(0x7d, 0x16);
            this.btnManage.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnManage, null);
            this.btnManage.TabIndex = 0x26;
            this.btnManage.TabStop = true;
            this.btnManage.Text = "<LOC>Director Options";
            this.btnManage.TextAlign = ContentAlignment.MiddleCenter;
            this.btnManage.TextPadding = new Padding(0);
            this.btnManage.Visible = false;
            this.btnManage.Click += new EventHandler(this.btnManage_Click_1);
            this.cmDirector.MenuItems.AddRange(new MenuItem[] { this.menuItem1, this.menuItem4, this.menuItem2, this.menuItem3 });
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Tournament Manager";
            this.menuItem1.Click += new EventHandler(this.menuItem1_Click);
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "Link to Chat";
            this.menuItem4.Click += new EventHandler(this.menuItem4_Click);
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "Assign New Director";
            this.menuItem2.Click += new EventHandler(this.menuItem2_Click);
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "Delete Tournament";
            this.menuItem3.Click += new EventHandler(this.menuItem3_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(900, 600);
            base.Controls.Add(this.btnManage);
            base.Controls.Add(this.lSignups);
            base.Controls.Add(this.lSignupsCaption);
            base.Controls.Add(this.lStatus);
            base.Controls.Add(this.lStatusCaption);
            base.Controls.Add(this.lInfo);
            base.Controls.Add(this.pbMap);
            base.Controls.Add(this.pbFaction);
            base.Controls.Add(this.btnRegister);
            base.Controls.Add(this.lAssistants);
            base.Controls.Add(this.lNumberOfRounds);
            base.Controls.Add(this.lRoundLength);
            base.Controls.Add(this.lAssistantsCaption);
            base.Controls.Add(this.lNumberOfRoundsCaption);
            base.Controls.Add(this.lRoundLengthCaption);
            base.Controls.Add(this.lWebsite);
            base.Controls.Add(this.lDirector);
            base.Controls.Add(this.lType);
            base.Controls.Add(this.lStartTime);
            base.Controls.Add(this.lWebsiteCaption);
            base.Controls.Add(this.lDirectorCaption);
            base.Controls.Add(this.lTypeCaption);
            base.Controls.Add(this.lStartTimeCaption);
            base.Controls.Add(this.lTournamentDetails);
            base.Controls.Add(this.skinLabel1);
            base.Controls.Add(this.lbMaps);
            base.Controls.Add(this.gpgBorderPanel2);
            base.Controls.Add(this.gpgCalendar1);
            base.Controls.Add(this.lFaction);
            base.Controls.Add(this.lMap);
            base.Controls.Add(this.gpgBorderPanel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(900, 600);
            base.Name = "DlgTournamentRegistration";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Tournament Calendar";
            base.Controls.SetChildIndex(this.gpgBorderPanel1, 0);
            base.Controls.SetChildIndex(this.lMap, 0);
            base.Controls.SetChildIndex(this.lFaction, 0);
            base.Controls.SetChildIndex(this.gpgCalendar1, 0);
            base.Controls.SetChildIndex(this.gpgBorderPanel2, 0);
            base.Controls.SetChildIndex(this.lbMaps, 0);
            base.Controls.SetChildIndex(this.skinLabel1, 0);
            base.Controls.SetChildIndex(this.lTournamentDetails, 0);
            base.Controls.SetChildIndex(this.lStartTimeCaption, 0);
            base.Controls.SetChildIndex(this.lTypeCaption, 0);
            base.Controls.SetChildIndex(this.lDirectorCaption, 0);
            base.Controls.SetChildIndex(this.lWebsiteCaption, 0);
            base.Controls.SetChildIndex(this.lStartTime, 0);
            base.Controls.SetChildIndex(this.lType, 0);
            base.Controls.SetChildIndex(this.lDirector, 0);
            base.Controls.SetChildIndex(this.lWebsite, 0);
            base.Controls.SetChildIndex(this.lRoundLengthCaption, 0);
            base.Controls.SetChildIndex(this.lNumberOfRoundsCaption, 0);
            base.Controls.SetChildIndex(this.lAssistantsCaption, 0);
            base.Controls.SetChildIndex(this.lRoundLength, 0);
            base.Controls.SetChildIndex(this.lNumberOfRounds, 0);
            base.Controls.SetChildIndex(this.lAssistants, 0);
            base.Controls.SetChildIndex(this.btnRegister, 0);
            base.Controls.SetChildIndex(this.pbFaction, 0);
            base.Controls.SetChildIndex(this.pbMap, 0);
            base.Controls.SetChildIndex(this.lInfo, 0);
            base.Controls.SetChildIndex(this.lStatusCaption, 0);
            base.Controls.SetChildIndex(this.lStatus, 0);
            base.Controls.SetChildIndex(this.lSignupsCaption, 0);
            base.Controls.SetChildIndex(this.lSignups, 0);
            base.Controls.SetChildIndex(this.btnManage, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            ((ISupportInitialize) this.pbFaction).EndInit();
            ((ISupportInitialize) this.pbMap).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitMonths()
        {
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\01-jan.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\02-feb.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\03-mar.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\04-apr.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\05-may.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\06-jun.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\07-jul.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\08-aug.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\09-sep.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\10-oct.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\11-november.jpg"));
            this.gpgCalendar1.MontlyBackgrounds.Add(SkinManager.GetImage(@"Controls\Calendar\12-dec.jpg"));
            this.gpgCalendar1.Month = DateTime.Now.Month;
        }

        private bool IsSuperTD()
        {
            return (ConfigSettings.GetString("Super TD", "Admin,SonOfShagrat").ToUpper().IndexOf(User.Current.Name.ToUpper()) > 0);
        }

        private void lbMaps_SelectedValueChanged(object sender, EventArgs e)
        {
            this.CheckSelection();
        }

        private void lSignupsCaption_Click(object sender, EventArgs e)
        {
            if (this.mSelectedRecord != null)
            {
                new DlgTournamentParticipants(Convert.ToInt32(this.mSelectedRecord["tournament_id"])).Show();
            }
        }

        private void lWebsite_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(this.lWebsite.Text);
            }
            catch
            {
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            if (this.mSelectedRecord != null)
            {
                int tournamentid = Convert.ToInt32(this.mSelectedRecord["tournament_id"]);
                DlgManageTournament tournament = new DlgManageTournament();
                tournament.MainForm = base.MainForm;
                tournament.LoadTournament(tournamentid);
                tournament.Show();
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (this.mSelectedRecord != null)
            {
                WaitCallback callBack = null;
                int tournamentid = Convert.ToInt32(this.mSelectedRecord["tournament_id"]);
                string result = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>Who would you like to assign as TD?"));
                if (result != "")
                {
                    if (callBack == null)
                    {
                        callBack = delegate (object o) {
                            VGen0 method = null;
                            VGen0 gen2 = null;
                            VGen0 gen3 = null;
                            try
                            {
                                DataList queryData = DataAccess.GetQueryData("GetPlayerIDByName", new object[] { result });
                                if (queryData.Count == 0)
                                {
                                    if (method == null)
                                    {
                                        method = delegate {
                                            DlgMessage.Show(this.MainForm, Loc.Get("<LOC>This player does not exist."));
                                        };
                                    }
                                    this.Invoke(method);
                                }
                                else if (AccessControlList.GetByName("TournamentDirectors").FindMember(Convert.ToInt32(queryData[0][0])) == null)
                                {
                                    if (gen2 == null)
                                    {
                                        gen2 = delegate {
                                            DlgMessage.Show(this.MainForm, Loc.Get("<LOC>This player is not a tournament director."));
                                        };
                                    }
                                    this.Invoke(gen2);
                                }
                                else
                                {
                                    DataAccess.ExecuteQuery("Tournament Reassign TD", new object[] { result, tournamentid });
                                    if (gen3 == null)
                                    {
                                        gen3 = delegate {
                                            this.RefreshDates();
                                        };
                                    }
                                    this.Invoke(gen3);
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                }
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (this.mSelectedRecord != null)
            {
                WaitCallback callBack = null;
                int tournamentid = Convert.ToInt32(this.mSelectedRecord["tournament_id"]);
                DlgYesNo no = new DlgYesNo(base.MainForm, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>Are you sure you want to delete this tournament?"));
                if (no.ShowDialog() == DialogResult.Yes)
                {
                    if (callBack == null)
                    {
                        callBack = delegate (object o) {
                            VGen0 method = null;
                            try
                            {
                                DataAccess.ExecuteQuery("Tournament Delete", new object[] { tournamentid });
                                if (method == null)
                                {
                                    method = delegate {
                                        this.RefreshDates();
                                    };
                                }
                                this.Invoke(method);
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                }
            }
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            if (this.mSelectedRecord != null)
            {
                int num = Convert.ToInt32(this.mSelectedRecord["tournament_id"]);
                string str = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>Text to display in chat before the tournament link."));
                if (str != "")
                {
                    Messaging.SendGathering(string.Concat(new object[] { str, " tournament:\"", num.ToString(), '\x0003', this.mSelectedRecord["name"], "\"" }));
                    base.MainForm.AddChat(User.Current, string.Concat(new object[] { str, " tournament:\"", num.ToString(), '\x0003', this.mSelectedRecord["name"], "\"" }));
                }
            }
        }

        public static DateTime MySQLToDateTime(string data)
        {
            string[] strArray = data.Split("-: ".ToCharArray());
            int hour = Convert.ToInt32(strArray[3]);
            int minute = Convert.ToInt32(strArray[4]);
            int second = Convert.ToInt32(strArray[5]);
            int year = Convert.ToInt32(strArray[0]);
            int month = Convert.ToInt32(strArray[1]);
            int day = Convert.ToInt32(strArray[2]);
            DateTime time = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
            return time.ToLocalTime();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.lStartTime.Left = this.lStartTimeCaption.Right;
            this.lType.Left = this.lTypeCaption.Right;
            this.lDirector.Left = this.lDirectorCaption.Right;
            this.lWebsite.Left = this.lWebsiteCaption.Right;
            this.lRoundLength.Left = this.lRoundLengthCaption.Right;
            this.lNumberOfRounds.Left = this.lNumberOfRoundsCaption.Right;
            this.lAssistants.Left = this.lAssistantsCaption.Right;
            this.lSignups.Left = this.lSignupsCaption.Right;
            this.lStatus.Left = this.lStatusCaption.Right;
            this.lTournamentDetails.Text = Loc.Get("<LOC>Tournament Details:");
        }

        private void RefreshDates()
        {
            this.RefreshDates(-1);
        }

        public void RefreshDates(int selectedTournmanentID)
        {
            WaitCallback callBack = null;
            if (this.mCanRefresh)
            {
                this.btnRegister.Visible = false;
                this.mCanRefresh = false;
                this.gpgCalendar1.Dates.Clear();
                this.lbMaps.Items.Clear();
                this.ClearDetails();
                this.lbMaps.Visible = false;
                if (callBack == null)
                {
                    callBack = delegate (object o) {
                        VGen1 method = null;
                        VGen0 gen2 = null;
                        try
                        {
                            DataList queryData = DataAccess.GetQueryData("Tournament Assistant List", new object[0]);
                            GPG.Logging.EventLog.WriteLine("Got tournament assistants....", new object[0]);
                            DataList list2 = DataAccess.GetQueryData("Tournament List", new object[0]);
                            GPG.Logging.EventLog.WriteLine("Got tournament list....", new object[0]);
                            foreach (DataRecord record in list2)
                            {
                                GPG.Logging.EventLog.WriteLine("Got a tournament row....", new object[0]);
                                DateTime time = MySQLToDateTime(record["tournament_date"]);
                                CalendarDate date = new CalendarDate {
                                    Date = time,
                                    Value = record,
                                    Label = record["name"]
                                };
                                if (Convert.ToInt32(record["tournament_id"]) == selectedTournmanentID)
                                {
                                    this.gpgCalendar1.SetDate(time);
                                }
                                foreach (DataRecord record2 in queryData)
                                {
                                    if (record2["tournament_id"] == record["tournament_id"])
                                    {
                                        date.Tag.Add(record2);
                                    }
                                }
                                if (method == null)
                                {
                                    method = delegate (object oitem) {
                                        this.gpgCalendar1.Dates.Add(oitem as CalendarDate);
                                        this.gpgCalendar1.Invalidate();
                                        this.CheckSelectedItems();
                                        this.CheckUnderlines();
                                    };
                                }
                                this.Invoke(method, new object[] { date });
                            }
                            if (gen2 == null)
                            {
                                gen2 = delegate {
                                    this.lbMaps.Visible = true;
                                    try
                                    {
                                        for (int i = 0; i < this.lbMaps.Items.Count; i++)
                                        {
                                            CalendarDate date = this.lbMaps.Items[i] as CalendarDate;
                                            DataRecord record = date.Value as DataRecord;
                                            if (record["tournament_id"] == selectedTournmanentID.ToString())
                                            {
                                                this.lbMaps.SelectedIndex = i;
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        ErrorLog.WriteLine(exception);
                                    }
                                    this.btnRegister.Visible = true;
                                };
                            }
                            this.Invoke(gen2);
                            this.mCanRefresh = true;
                        }
                        catch (Exception exception)
                        {
                            this.btnRegister.Visible = true;
                            this.mCanRefresh = true;
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
            }
        }

        private void SetButtonStatus()
        {
            if (this.mSelectedRecord != null)
            {
                if (this.mSelectedRecord["status"].ToUpper() == "REGISTRATION")
                {
                    if ((this.mSelectedRecord["principal_id"] == User.Current.ID.ToString()) || this.IsSuperTD())
                    {
                        this.btnManage.Visible = true;
                        this.btnRegister.Tag = 2;
                        this.btnRegister.Text = Loc.Get("<LOC>Update");
                    }
                    else if (this.mSelectedRecord["registered"] == "1")
                    {
                        this.btnRegister.Tag = 1;
                        this.btnRegister.Text = Loc.Get("<LOC>Unregister");
                    }
                    else
                    {
                        this.btnRegister.Tag = 0;
                        this.btnRegister.Text = Loc.Get("<LOC>Register");
                    }
                }
                else if ((this.mSelectedRecord["status"].ToUpper() != "COMPLETE") && ((this.mSelectedRecord["principal_id"] == User.Current.ID.ToString()) || this.IsSuperTD()))
                {
                    this.btnManage.Visible = true;
                    this.btnRegister.Tag = 2;
                    this.btnRegister.Text = Loc.Get("<LOC>Update");
                }
                else
                {
                    this.btnRegister.Tag = 3;
                    this.btnRegister.Text = Loc.Get("<LOC>View Results");
                }
            }
        }
    }
}

