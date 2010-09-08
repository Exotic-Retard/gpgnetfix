namespace GPG.Multiplayer.Client.Clans
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Controls.UserList;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Client.Volunteering;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgClanProfileEx : DlgBase
    {
        private CheckBox checkBoxEmbedWebsite;
        private Dictionary<string, ClanView> ClanNameLookup = new Dictionary<string, ClanView>();
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel10;
        private GPGLabel gpgLabel11;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGLabel gpgLabelDescription;
        private GPGLabel gpgLabelFounded;
        private GPGLabel gpgLabelWebsite;
        private GPGPanel gpgPanelDescription;
        private GPGPanel gpgPanelWebsite;
        private GPGPictureBox gpgPictureBoxAvatar;
        private GPGPictureBox gpgPictureBoxCancel;
        private GPGPictureBox gpgPictureBoxEdit;
        private GPGPictureBox gpgPictureBoxSave;
        private GPGPictureBox gpgPictureBoxWebsite;
        private GPGTextArea gpgTextAreaDescription;
        private GPGTextBox gpgTextBoxWebsite;
        private ClanView mCurrentClan = null;
        private bool? mHasVolunteeredForEmbeddedWeb = null;
        private LinkedList<ClanView> mViewList = new LinkedList<ClanView>();
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Dictionary<string, ClanView> PlayerNameLookup = new Dictionary<string, ClanView>();
        private PnlUserList pnlUserListClan;
        private SkinButton skinButtonNextProfile;
        private SkinButton skinButtonPreviousProfile;
        private SkinButton skinButtonWebVersion;
        private SkinGroupPanel skinGroupPanelClan;
        private SkinLabel skinLabel1;
        private SkinLabel skinLabelHeader;
        private SkinLabel skinLabelRoster;
        private SplitContainer splitContainerMain;
        private GPGLabel statAverageRating;
        private GPGLabel statDraws;
        private GPGLabel statLosses;
        private GPGLabel statNumberPlayers;
        private GPGLabel statRank;
        private GPGLabel statRating;
        private GPGLabel statTotalGames;
        private GPGLabel statWinPct;
        private GPGLabel statWins;
        private bool UserCheck = true;
        private WebBrowser webBrowserProfile;

        public DlgClanProfileEx()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            this.skinButtonWebVersion.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
            this.skinButtonNextProfile.MakeEdgesTransparent();
            this.skinButtonPreviousProfile.MakeEdgesTransparent();
        }

        private void checkBoxEmbedWebsite_CheckedChanged(object sender, EventArgs e)
        {
            if (this.UserCheck && !this.HasVolunteeredForEmbeddedWeb)
            {
                try
                {
                    VolunteerEffort effort;
                    this.UserCheck = false;
                    if (new QuazalQuery("GetVolunteerEffortByName", new object[] { this.EmbedWebEffortName }).GetObject<VolunteerEffort>(out effort))
                    {
                        DlgVolunteer volunteer = new DlgVolunteer(effort);
                        if (volunteer.ShowDialog() == DialogResult.OK)
                        {
                            this.mHasVolunteeredForEmbeddedWeb = true;
                        }
                        else
                        {
                            this.checkBoxEmbedWebsite.Checked = false;
                        }
                    }
                    else
                    {
                        this.checkBoxEmbedWebsite.Checked = false;
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                finally
                {
                    this.UserCheck = true;
                }
            }
        }

        internal void Construct()
        {
            if (GameInformation.SelectedGame.IsSpaceSiege || GameInformation.SelectedGame.IsChatOnly)
            {
                this.skinLabel1.Hide();
                this.gpgLabel1.Hide();
                this.gpgLabel2.Hide();
                this.gpgLabel3.Hide();
                this.gpgLabel6.Hide();
                this.gpgLabel7.Hide();
                this.gpgLabel8.Hide();
                this.gpgLabel9.Hide();
                this.gpgLabel10.Hide();
                this.gpgLabel11.Hide();
                this.statAverageRating.Hide();
                this.statDraws.Hide();
                this.statLosses.Hide();
                this.statNumberPlayers.Hide();
                this.statRank.Hide();
                this.statRating.Hide();
                this.statTotalGames.Hide();
                this.statWinPct.Hide();
                this.statWins.Hide();
                this.skinButtonWebVersion.Hide();
            }
            if (!this.ViewList.Contains(this.CurrentClan))
            {
                this.ViewList.AddLast(this.CurrentClan);
            }
            this.skinButtonPreviousProfile.Visible = !this.ViewList.First.Value.Equals(this.CurrentClan);
            this.skinButtonNextProfile.Visible = !this.ViewList.Last.Value.Equals(this.CurrentClan);
            Clan clan = this.CurrentClan.Clan;
            MappedObjectList<ClanMember> members = this.CurrentClan.Members;
            this.skinLabelRoster.Text = string.Format(Loc.Get("<LOC>Clan Roster ({0})"), members.Count);
            ThreadPool.QueueUserWorkItem(delegate (object state) {
                this.pnlUserListClan.ClanMembers = members;
            });
            this.statAverageRating.Text = this.CurrentClan.Clan.Rating.AvgRating.ToString();
            this.statDraws.Text = this.CurrentClan.Clan.Rating.Draws.ToString();
            this.statLosses.Text = this.CurrentClan.Clan.Rating.Losses.ToString();
            this.statNumberPlayers.Text = this.CurrentClan.Clan.Rating.People.ToString();
            this.statRank.Text = this.CurrentClan.Clan.Rating.Rank.ToString();
            this.statRating.Text = this.CurrentClan.Clan.Rating.Rating.ToString();
            this.statTotalGames.Text = this.CurrentClan.Clan.Rating.TotalGames.ToString();
            this.statWinPct.Text = this.CurrentClan.Clan.Rating.WinPercentage.ToString();
            this.statWins.Text = this.CurrentClan.Clan.Rating.Wins.ToString();
            this.skinGroupPanelClan.Text = clan.ToString();
            this.gpgLabelFounded.Text = string.Format(Loc.Get("<LOC>Founded {0}"), clan.CreateDate.ToLongDateString());
            this.gpgLabelFounded.ForeColor = Color.Yellow;
            this.Text = string.Format(Loc.Get("<LOC>Clan Profile: {0}"), clan);
            this.gpgTextAreaDescription.Text = clan.Description;
            this.gpgTextAreaDescription.Visible = false;
            this.ToggleProfileBody();
            this.gpgPictureBoxSave.Visible = false;
            this.gpgPictureBoxCancel.Visible = false;
            this.gpgTextAreaDescription.Visible = false;
            this.gpgTextBoxWebsite.Visible = false;
            this.checkBoxEmbedWebsite.Visible = false;
            if ((((ClanMember.Current != null) && (Clan.Current != null)) && Clan.Current.Equals(clan)) && ClanMember.Current.HasAbility(ClanAbility.ChangeProfile))
            {
                this.gpgPictureBoxEdit.Visible = true;
                this.gpgPictureBoxEdit.Enabled = true;
                this.gpgPictureBoxSave.Enabled = true;
                this.gpgPictureBoxCancel.Enabled = true;
            }
            else
            {
                this.gpgPictureBoxEdit.Visible = false;
                this.gpgPictureBoxEdit.Enabled = false;
                this.gpgPictureBoxSave.Enabled = false;
                this.gpgPictureBoxCancel.Enabled = false;
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

        private void gpgLabelWebsite_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowWebPage(this.gpgLabelWebsite.Text);
        }

        private void gpgPictureBoxCancel_Click(object sender, EventArgs e)
        {
            this.skinButtonWebVersion.Enabled = true;
            this.gpgPictureBoxEdit.Visible = true;
            this.gpgPictureBoxSave.Visible = false;
            this.gpgPictureBoxCancel.Visible = false;
            this.gpgTextAreaDescription.Visible = false;
            this.gpgTextBoxWebsite.Visible = false;
            this.checkBoxEmbedWebsite.Visible = false;
            this.ToggleProfileBody();
        }

        private void gpgPictureBoxEdit_Click(object sender, EventArgs e)
        {
            this.gpgPictureBoxEdit.Visible = false;
            this.gpgPictureBoxSave.Visible = true;
            this.gpgPictureBoxCancel.Visible = true;
            this.gpgTextAreaDescription.Text = Clan.Current.Description;
            this.gpgTextAreaDescription.Visible = true;
            this.gpgTextAreaDescription.BringToFront();
            this.gpgTextBoxWebsite.Text = Clan.Current.Website;
            this.gpgTextBoxWebsite.Visible = true;
            this.gpgTextBoxWebsite.BringToFront();
            this.checkBoxEmbedWebsite.Visible = true;
            this.checkBoxEmbedWebsite.BringToFront();
            this.checkBoxEmbedWebsite.Checked = Clan.Current.EmbedWebsite;
            this.skinButtonWebVersion.Enabled = false;
        }

        private void gpgPictureBoxSave_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if ((((ClanMember.Current != null) && (Clan.Current != null)) && Clan.Current.Equals(this.CurrentClan.Clan)) && ClanMember.Current.HasAbility(ClanAbility.ChangeProfile))
            {
                if (Profanity.ContainsProfanity(this.gpgTextAreaDescription.Text))
                {
                    DlgMessage.ShowDialog("<LOC>Your clan profile may not contain profanity. Please enter a valid profile description.", "<LOC>Error");
                }
                else if (!DataAccess.ExecuteQuery("UpdateClanProfile", new object[] { this.gpgTextAreaDescription.Text, this.gpgTextBoxWebsite.Text, this.checkBoxEmbedWebsite.Checked, Clan.Current.ID }))
                {
                    DlgMessage.ShowDialog("<LOC>Failed to save clan profile", "<LOC>Error");
                }
                else
                {
                    Clan.Current.Description = this.gpgTextAreaDescription.Text;
                    Clan.Current.Website = this.gpgTextBoxWebsite.Text;
                    Clan.Current.EmbedWebsite = this.checkBoxEmbedWebsite.Checked;
                    this.skinButtonWebVersion.Enabled = true;
                    this.gpgPictureBoxEdit.Visible = true;
                    this.gpgPictureBoxSave.Visible = false;
                    this.gpgPictureBoxCancel.Visible = false;
                    this.skinButtonWebVersion.Enabled = true;
                    this.gpgTextAreaDescription.Visible = false;
                    this.gpgTextBoxWebsite.Visible = false;
                    this.checkBoxEmbedWebsite.Visible = false;
                    this.ToggleProfileBody();
                }
            }
            else
            {
                DlgMessage.ShowDialog("<LOC>You do not have permission to change this clan profile.", "<LOC>Error");
            }
        }

        private void gpgPictureBoxWebsite_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowWebPage(this.gpgLabelWebsite.Text);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgClanProfileEx));
            this.splitContainerMain = new SplitContainer();
            this.skinButtonNextProfile = new SkinButton();
            this.skinButtonPreviousProfile = new SkinButton();
            this.skinGroupPanelClan = new SkinGroupPanel();
            this.gpgLabelFounded = new GPGLabel();
            this.gpgPanelWebsite = new GPGPanel();
            this.checkBoxEmbedWebsite = new CheckBox();
            this.gpgPictureBoxWebsite = new GPGPictureBox();
            this.gpgLabelWebsite = new GPGLabel();
            this.gpgTextBoxWebsite = new GPGTextBox();
            this.gpgPictureBoxAvatar = new GPGPictureBox();
            this.gpgPictureBoxEdit = new GPGPictureBox();
            this.skinLabel1 = new SkinLabel();
            this.gpgPictureBoxCancel = new GPGPictureBox();
            this.gpgPictureBoxSave = new GPGPictureBox();
            this.gpgPanelDescription = new GPGPanel();
            this.webBrowserProfile = new WebBrowser();
            this.gpgLabelDescription = new GPGLabel();
            this.gpgTextAreaDescription = new GPGTextArea();
            this.statWinPct = new GPGLabel();
            this.statAverageRating = new GPGLabel();
            this.gpgLabel11 = new GPGLabel();
            this.gpgLabel10 = new GPGLabel();
            this.statNumberPlayers = new GPGLabel();
            this.statDraws = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.statLosses = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.statWins = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.statRank = new GPGLabel();
            this.statRating = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.statTotalGames = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonWebVersion = new SkinButton();
            this.skinLabelHeader = new SkinLabel();
            this.pnlUserListClan = new PnlUserList();
            this.skinLabelRoster = new SkinLabel();
            this.pictureBox3 = new PictureBox();
            this.pictureBox2 = new PictureBox();
            this.pictureBox1 = new PictureBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.skinGroupPanelClan.SuspendLayout();
            this.gpgPanelWebsite.SuspendLayout();
            ((ISupportInitialize) this.gpgPictureBoxWebsite).BeginInit();
            this.gpgTextBoxWebsite.Properties.BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxAvatar).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxEdit).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxCancel).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxSave).BeginInit();
            this.gpgPanelDescription.SuspendLayout();
            this.gpgTextAreaDescription.Properties.BeginInit();
            this.skinLabelRoster.SuspendLayout();
            ((ISupportInitialize) this.pictureBox3).BeginInit();
            ((ISupportInitialize) this.pictureBox2).BeginInit();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x315, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.splitContainerMain.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerMain.BackColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.splitContainerMain.Location = new Point(7, 0x3d);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Panel1.BackgroundImage = (Image) manager.GetObject("splitContainerMain.Panel1.BackgroundImage");
            this.splitContainerMain.Panel1.Controls.Add(this.skinButtonNextProfile);
            this.splitContainerMain.Panel1.Controls.Add(this.skinButtonPreviousProfile);
            this.splitContainerMain.Panel1.Controls.Add(this.skinGroupPanelClan);
            this.splitContainerMain.Panel1.Controls.Add(this.skinButtonWebVersion);
            this.splitContainerMain.Panel1.Controls.Add(this.skinLabelHeader);
            base.ttDefault.SetSuperTip(this.splitContainerMain.Panel1, null);
            this.splitContainerMain.Panel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerMain.Panel2.Controls.Add(this.pnlUserListClan);
            this.splitContainerMain.Panel2.Controls.Add(this.skinLabelRoster);
            base.ttDefault.SetSuperTip(this.splitContainerMain.Panel2, null);
            this.splitContainerMain.Size = new Size(0x342, 0x22c);
            this.splitContainerMain.SplitterDistance = 0x245;
            this.splitContainerMain.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainerMain, null);
            this.splitContainerMain.TabIndex = 7;
            this.skinButtonNextProfile.Anchor = AnchorStyles.Bottom;
            this.skinButtonNextProfile.AutoStyle = true;
            this.skinButtonNextProfile.BackColor = Color.Transparent;
            this.skinButtonNextProfile.ButtonState = 0;
            this.skinButtonNextProfile.DialogResult = DialogResult.OK;
            this.skinButtonNextProfile.DisabledForecolor = Color.Gray;
            this.skinButtonNextProfile.DrawColor = Color.White;
            this.skinButtonNextProfile.DrawEdges = false;
            this.skinButtonNextProfile.FocusColor = Color.Yellow;
            this.skinButtonNextProfile.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNextProfile.ForeColor = Color.White;
            this.skinButtonNextProfile.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonNextProfile.IsStyled = true;
            this.skinButtonNextProfile.Location = new Point(0x183, 0x20a);
            this.skinButtonNextProfile.Name = "skinButtonNextProfile";
            this.skinButtonNextProfile.Size = new Size(30, 30);
            this.skinButtonNextProfile.SkinBasePath = @"Dialog\PlayerProfile\NextProfile";
            base.ttDefault.SetSuperTip(this.skinButtonNextProfile, null);
            this.skinButtonNextProfile.TabIndex = 4;
            this.skinButtonNextProfile.TabStop = true;
            this.skinButtonNextProfile.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNextProfile.TextPadding = new Padding(0);
            this.skinButtonNextProfile.Click += new EventHandler(this.skinButtonNextProfile_Click);
            this.skinButtonPreviousProfile.Anchor = AnchorStyles.Bottom;
            this.skinButtonPreviousProfile.AutoStyle = true;
            this.skinButtonPreviousProfile.BackColor = Color.Transparent;
            this.skinButtonPreviousProfile.ButtonState = 0;
            this.skinButtonPreviousProfile.DialogResult = DialogResult.OK;
            this.skinButtonPreviousProfile.DisabledForecolor = Color.Gray;
            this.skinButtonPreviousProfile.DrawColor = Color.White;
            this.skinButtonPreviousProfile.DrawEdges = false;
            this.skinButtonPreviousProfile.FocusColor = Color.Yellow;
            this.skinButtonPreviousProfile.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonPreviousProfile.ForeColor = Color.White;
            this.skinButtonPreviousProfile.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonPreviousProfile.IsStyled = true;
            this.skinButtonPreviousProfile.Location = new Point(0xa6, 0x20a);
            this.skinButtonPreviousProfile.Name = "skinButtonPreviousProfile";
            this.skinButtonPreviousProfile.Size = new Size(30, 30);
            this.skinButtonPreviousProfile.SkinBasePath = @"Dialog\PlayerProfile\PreviousProfile";
            base.ttDefault.SetSuperTip(this.skinButtonPreviousProfile, null);
            this.skinButtonPreviousProfile.TabIndex = 3;
            this.skinButtonPreviousProfile.TabStop = true;
            this.skinButtonPreviousProfile.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonPreviousProfile.TextPadding = new Padding(0);
            this.skinButtonPreviousProfile.Click += new EventHandler(this.skinButtonPreviousProfile_Click);
            this.skinGroupPanelClan.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.skinGroupPanelClan.AutoStyle = false;
            this.skinGroupPanelClan.BackColor = Color.Black;
            this.skinGroupPanelClan.BackgroundImage = (Image) manager.GetObject("skinGroupPanelClan.BackgroundImage");
            this.skinGroupPanelClan.BackgroundImageLayout = ImageLayout.Stretch;
            this.skinGroupPanelClan.Controls.Add(this.gpgLabelFounded);
            this.skinGroupPanelClan.Controls.Add(this.gpgPanelWebsite);
            this.skinGroupPanelClan.Controls.Add(this.gpgPictureBoxAvatar);
            this.skinGroupPanelClan.Controls.Add(this.gpgPictureBoxEdit);
            this.skinGroupPanelClan.Controls.Add(this.skinLabel1);
            this.skinGroupPanelClan.Controls.Add(this.gpgPictureBoxCancel);
            this.skinGroupPanelClan.Controls.Add(this.gpgPictureBoxSave);
            this.skinGroupPanelClan.Controls.Add(this.gpgPanelDescription);
            this.skinGroupPanelClan.Controls.Add(this.statWinPct);
            this.skinGroupPanelClan.Controls.Add(this.statAverageRating);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel11);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel10);
            this.skinGroupPanelClan.Controls.Add(this.statNumberPlayers);
            this.skinGroupPanelClan.Controls.Add(this.statDraws);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel8);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel9);
            this.skinGroupPanelClan.Controls.Add(this.statLosses);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel7);
            this.skinGroupPanelClan.Controls.Add(this.statWins);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel6);
            this.skinGroupPanelClan.Controls.Add(this.statRank);
            this.skinGroupPanelClan.Controls.Add(this.statRating);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel3);
            this.skinGroupPanelClan.Controls.Add(this.statTotalGames);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel2);
            this.skinGroupPanelClan.Controls.Add(this.gpgLabel1);
            this.skinGroupPanelClan.CutCorner = false;
            this.skinGroupPanelClan.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.skinGroupPanelClan.ForeColor = Color.White;
            this.skinGroupPanelClan.HeaderImage = (Image) manager.GetObject("skinGroupPanelClan.HeaderImage");
            this.skinGroupPanelClan.IsStyled = true;
            this.skinGroupPanelClan.Location = new Point(6, 0x43);
            this.skinGroupPanelClan.Margin = new Padding(7, 6, 7, 6);
            this.skinGroupPanelClan.Name = "skinGroupPanelClan";
            this.skinGroupPanelClan.Size = new Size(0x238, 0x1bb);
            base.ttDefault.SetSuperTip(this.skinGroupPanelClan, null);
            this.skinGroupPanelClan.TabIndex = 2;
            this.skinGroupPanelClan.Text = "clan";
            this.skinGroupPanelClan.TextAlign = ContentAlignment.MiddleLeft;
            this.skinGroupPanelClan.TextPadding = new Padding(4, 0, 0, 0);
            this.gpgLabelFounded.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelFounded.AutoSize = true;
            this.gpgLabelFounded.AutoStyle = true;
            this.gpgLabelFounded.BackColor = Color.Transparent;
            this.gpgLabelFounded.Font = new Font("Verdana", 8.25f, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelFounded.ForeColor = Color.Yellow;
            this.gpgLabelFounded.IgnoreMouseWheel = false;
            this.gpgLabelFounded.IsStyled = false;
            this.gpgLabelFounded.Location = new Point(6, 0x29);
            this.gpgLabelFounded.Margin = new Padding(0);
            this.gpgLabelFounded.Name = "gpgLabelFounded";
            this.gpgLabelFounded.Size = new Size(0x48, 13);
            base.ttDefault.SetSuperTip(this.gpgLabelFounded, null);
            this.gpgLabelFounded.TabIndex = 0x27;
            this.gpgLabelFounded.Text = "founded...";
            this.gpgLabelFounded.TextStyle = TextStyles.Custom;
            this.gpgPanelWebsite.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanelWebsite.BackColor = Color.Transparent;
            this.gpgPanelWebsite.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelWebsite.BorderThickness = 2;
            this.gpgPanelWebsite.Controls.Add(this.checkBoxEmbedWebsite);
            this.gpgPanelWebsite.Controls.Add(this.gpgPictureBoxWebsite);
            this.gpgPanelWebsite.Controls.Add(this.gpgLabelWebsite);
            this.gpgPanelWebsite.Controls.Add(this.gpgTextBoxWebsite);
            this.gpgPanelWebsite.DrawBorder = false;
            this.gpgPanelWebsite.Location = new Point(6, 0x3d);
            this.gpgPanelWebsite.Name = "gpgPanelWebsite";
            this.gpgPanelWebsite.Size = new Size(0x217, 0x15);
            base.ttDefault.SetSuperTip(this.gpgPanelWebsite, null);
            this.gpgPanelWebsite.TabIndex = 40;
            this.checkBoxEmbedWebsite.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.checkBoxEmbedWebsite.AutoSize = true;
            this.checkBoxEmbedWebsite.Font = new Font("Verdana", 8f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.checkBoxEmbedWebsite.Location = new Point(310, 3);
            this.checkBoxEmbedWebsite.Name = "checkBoxEmbedWebsite";
            this.checkBoxEmbedWebsite.Size = new Size(0xd9, 0x11);
            base.ttDefault.SetSuperTip(this.checkBoxEmbedWebsite, null);
            this.checkBoxEmbedWebsite.TabIndex = 3;
            this.checkBoxEmbedWebsite.Text = "<LOC>Allow embedded browsing";
            base.ttDefault.SetToolTip(this.checkBoxEmbedWebsite, "<LOC>If selected this clan profile website will be viewable from within GPGnet");
            this.checkBoxEmbedWebsite.UseVisualStyleBackColor = true;
            this.checkBoxEmbedWebsite.CheckedChanged += new EventHandler(this.checkBoxEmbedWebsite_CheckedChanged);
            this.gpgPictureBoxWebsite.Cursor = Cursors.Hand;
            this.gpgPictureBoxWebsite.Image = Resources.new_window;
            this.gpgPictureBoxWebsite.Location = new Point(3, 5);
            this.gpgPictureBoxWebsite.Name = "gpgPictureBoxWebsite";
            this.gpgPictureBoxWebsite.Size = new Size(8, 8);
            this.gpgPictureBoxWebsite.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.gpgPictureBoxWebsite, null);
            this.gpgPictureBoxWebsite.TabIndex = 2;
            this.gpgPictureBoxWebsite.TabStop = false;
            this.gpgPictureBoxWebsite.Click += new EventHandler(this.gpgPictureBoxWebsite_Click);
            this.gpgLabelWebsite.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelWebsite.AutoSize = true;
            this.gpgLabelWebsite.AutoStyle = true;
            this.gpgLabelWebsite.Font = new Font("Arial", 9.75f);
            this.gpgLabelWebsite.ForeColor = Color.White;
            this.gpgLabelWebsite.IgnoreMouseWheel = false;
            this.gpgLabelWebsite.IsStyled = false;
            this.gpgLabelWebsite.Location = new Point(11, 2);
            this.gpgLabelWebsite.Name = "gpgLabelWebsite";
            this.gpgLabelWebsite.Size = new Size(0x34, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelWebsite, null);
            this.gpgLabelWebsite.TabIndex = 0;
            this.gpgLabelWebsite.Text = "website";
            this.gpgLabelWebsite.TextStyle = TextStyles.Link;
            this.gpgLabelWebsite.Click += new EventHandler(this.gpgLabelWebsite_Click);
            this.gpgTextBoxWebsite.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxWebsite.Location = new Point(0, 0);
            this.gpgTextBoxWebsite.Name = "gpgTextBoxWebsite";
            this.gpgTextBoxWebsite.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxWebsite.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxWebsite.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxWebsite.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxWebsite.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxWebsite.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxWebsite.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxWebsite.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxWebsite.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxWebsite.Size = new Size(0x130, 20);
            this.gpgTextBoxWebsite.TabIndex = 1;
            this.gpgTextBoxWebsite.Visible = false;
            this.gpgPictureBoxAvatar.BackColor = Color.Transparent;
            this.gpgPictureBoxAvatar.Location = new Point(0x209, 9);
            this.gpgPictureBoxAvatar.Name = "gpgPictureBoxAvatar";
            this.gpgPictureBoxAvatar.Size = new Size(40, 20);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxAvatar, null);
            this.gpgPictureBoxAvatar.TabIndex = 0x1c;
            this.gpgPictureBoxAvatar.TabStop = false;
            this.gpgPictureBoxAvatar.Visible = false;
            this.gpgPictureBoxEdit.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxEdit.Cursor = Cursors.Hand;
            this.gpgPictureBoxEdit.Image = Resources.edit_small;
            this.gpgPictureBoxEdit.Location = new Point(0x221, 0x2b);
            this.gpgPictureBoxEdit.Name = "gpgPictureBoxEdit";
            this.gpgPictureBoxEdit.Size = new Size(0x10, 0x10);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxEdit, null);
            this.gpgPictureBoxEdit.TabIndex = 0x19;
            this.gpgPictureBoxEdit.TabStop = false;
            base.ttDefault.SetToolTip(this.gpgPictureBoxEdit, "<LOC>Edit");
            this.gpgPictureBoxEdit.Click += new EventHandler(this.gpgPictureBoxEdit_Click);
            this.skinLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(6, 340);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0x22b, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 1;
            this.skinLabel1.Text = "Vital Statistics";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(0);
            this.gpgPictureBoxCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxCancel.Cursor = Cursors.Hand;
            this.gpgPictureBoxCancel.Image = Resources.cancel_small;
            this.gpgPictureBoxCancel.Location = new Point(0x221, 0x42);
            this.gpgPictureBoxCancel.Name = "gpgPictureBoxCancel";
            this.gpgPictureBoxCancel.Size = new Size(0x10, 0x10);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxCancel, null);
            this.gpgPictureBoxCancel.TabIndex = 0x1b;
            this.gpgPictureBoxCancel.TabStop = false;
            base.ttDefault.SetToolTip(this.gpgPictureBoxCancel, "<LOC>Cancel");
            this.gpgPictureBoxCancel.Click += new EventHandler(this.gpgPictureBoxCancel_Click);
            this.gpgPictureBoxSave.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxSave.Cursor = Cursors.Hand;
            this.gpgPictureBoxSave.Image = Resources.save_small;
            this.gpgPictureBoxSave.Location = new Point(0x221, 0x2b);
            this.gpgPictureBoxSave.Name = "gpgPictureBoxSave";
            this.gpgPictureBoxSave.Size = new Size(0x10, 0x10);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxSave, null);
            this.gpgPictureBoxSave.TabIndex = 0x1a;
            this.gpgPictureBoxSave.TabStop = false;
            base.ttDefault.SetToolTip(this.gpgPictureBoxSave, "<LOC>Save");
            this.gpgPictureBoxSave.Click += new EventHandler(this.gpgPictureBoxSave_Click);
            this.gpgPanelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelDescription.AutoScroll = true;
            this.gpgPanelDescription.BackColor = Color.Transparent;
            this.gpgPanelDescription.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelDescription.BorderThickness = 2;
            this.gpgPanelDescription.Controls.Add(this.webBrowserProfile);
            this.gpgPanelDescription.Controls.Add(this.gpgLabelDescription);
            this.gpgPanelDescription.Controls.Add(this.gpgTextAreaDescription);
            this.gpgPanelDescription.DrawBorder = false;
            this.gpgPanelDescription.Location = new Point(6, 0x58);
            this.gpgPanelDescription.Name = "gpgPanelDescription";
            this.gpgPanelDescription.Size = new Size(0x22b, 0xf6);
            base.ttDefault.SetSuperTip(this.gpgPanelDescription, null);
            this.gpgPanelDescription.TabIndex = 2;
            this.webBrowserProfile.Dock = DockStyle.Fill;
            this.webBrowserProfile.Location = new Point(0, 0);
            this.webBrowserProfile.MinimumSize = new Size(20, 20);
            this.webBrowserProfile.Name = "webBrowserProfile";
            this.webBrowserProfile.ScriptErrorsSuppressed = true;
            this.webBrowserProfile.Size = new Size(0x22b, 0xf6);
            base.ttDefault.SetSuperTip(this.webBrowserProfile, null);
            this.webBrowserProfile.TabIndex = 2;
            this.webBrowserProfile.Visible = false;
            this.gpgLabelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(0, 0);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x217, 15);
            base.ttDefault.SetSuperTip(this.gpgLabelDescription, null);
            this.gpgLabelDescription.TabIndex = 0;
            this.gpgLabelDescription.Text = "description";
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            this.gpgTextAreaDescription.BorderColor = Color.White;
            this.gpgTextAreaDescription.Dock = DockStyle.Fill;
            this.gpgTextAreaDescription.Location = new Point(0, 0);
            this.gpgTextAreaDescription.Name = "gpgTextAreaDescription";
            this.gpgTextAreaDescription.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaDescription.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaDescription.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaDescription.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaDescription.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaDescription.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaDescription.Size = new Size(0x22b, 0xf6);
            this.gpgTextAreaDescription.TabIndex = 1;
            this.gpgTextAreaDescription.Visible = false;
            this.statWinPct.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statWinPct.AutoGrowDirection = GrowDirections.None;
            this.statWinPct.AutoSize = true;
            this.statWinPct.AutoStyle = true;
            this.statWinPct.BackColor = Color.Transparent;
            this.statWinPct.Font = new Font("Arial", 9.75f);
            this.statWinPct.ForeColor = Color.White;
            this.statWinPct.IgnoreMouseWheel = false;
            this.statWinPct.IsStyled = false;
            this.statWinPct.Location = new Point(0x112, 0x17f);
            this.statWinPct.Name = "statWinPct";
            this.statWinPct.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statWinPct, null);
            this.statWinPct.TabIndex = 0x16;
            this.statWinPct.Text = "2258";
            this.statWinPct.TextStyle = TextStyles.Bold;
            this.statAverageRating.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statAverageRating.AutoGrowDirection = GrowDirections.None;
            this.statAverageRating.AutoSize = true;
            this.statAverageRating.AutoStyle = true;
            this.statAverageRating.BackColor = Color.Transparent;
            this.statAverageRating.Font = new Font("Arial", 9.75f);
            this.statAverageRating.ForeColor = Color.White;
            this.statAverageRating.IgnoreMouseWheel = false;
            this.statAverageRating.IsStyled = false;
            this.statAverageRating.Location = new Point(0x17a, 0x17f);
            this.statAverageRating.Name = "statAverageRating";
            this.statAverageRating.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statAverageRating, null);
            this.statAverageRating.TabIndex = 0x15;
            this.statAverageRating.Text = "2258";
            this.statAverageRating.TextStyle = TextStyles.Bold;
            this.gpgLabel11.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel11.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel11.AutoStyle = true;
            this.gpgLabel11.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel11.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel11.ForeColor = Color.White;
            this.gpgLabel11.IgnoreMouseWheel = false;
            this.gpgLabel11.IsStyled = false;
            this.gpgLabel11.Location = new Point(0x17a, 0x16b);
            this.gpgLabel11.Name = "gpgLabel11";
            this.gpgLabel11.Size = new Size(0x6c, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel11, null);
            this.gpgLabel11.TabIndex = 20;
            this.gpgLabel11.Text = "<LOC>Average Rating";
            this.gpgLabel11.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel11.TextStyle = TextStyles.Custom;
            this.gpgLabel10.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel10.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel10.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(0x112, 0x191);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0x11e, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel10, null);
            this.gpgLabel10.TabIndex = 0x12;
            this.gpgLabel10.Text = "<LOC>Number of Ranked Players";
            this.gpgLabel10.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel10.TextStyle = TextStyles.Custom;
            this.statNumberPlayers.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statNumberPlayers.AutoGrowDirection = GrowDirections.None;
            this.statNumberPlayers.AutoSize = true;
            this.statNumberPlayers.AutoStyle = true;
            this.statNumberPlayers.BackColor = Color.Transparent;
            this.statNumberPlayers.Font = new Font("Arial", 9.75f);
            this.statNumberPlayers.ForeColor = Color.White;
            this.statNumberPlayers.IgnoreMouseWheel = false;
            this.statNumberPlayers.IsStyled = false;
            this.statNumberPlayers.Location = new Point(0x112, 0x1a5);
            this.statNumberPlayers.Name = "statNumberPlayers";
            this.statNumberPlayers.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statNumberPlayers, null);
            this.statNumberPlayers.TabIndex = 0x13;
            this.statNumberPlayers.Text = "2258";
            this.statNumberPlayers.TextStyle = TextStyles.Bold;
            this.statDraws.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statDraws.AutoGrowDirection = GrowDirections.None;
            this.statDraws.AutoSize = true;
            this.statDraws.AutoStyle = true;
            this.statDraws.BackColor = Color.Transparent;
            this.statDraws.Font = new Font("Arial", 9.75f);
            this.statDraws.ForeColor = Color.White;
            this.statDraws.IgnoreMouseWheel = false;
            this.statDraws.IsStyled = false;
            this.statDraws.Location = new Point(0xb8, 0x1a5);
            this.statDraws.Name = "statDraws";
            this.statDraws.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statDraws, null);
            this.statDraws.TabIndex = 0x11;
            this.statDraws.Text = "2258";
            this.statDraws.TextStyle = TextStyles.Bold;
            this.gpgLabel8.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel8.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0xb6, 0x191);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel8, null);
            this.gpgLabel8.TabIndex = 15;
            this.gpgLabel8.Text = "<LOC>Draws";
            this.gpgLabel8.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel8.TextStyle = TextStyles.Custom;
            this.gpgLabel9.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel9.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0x112, 0x16b);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel9, null);
            this.gpgLabel9.TabIndex = 0x10;
            this.gpgLabel9.Text = "<LOC>Win %";
            this.gpgLabel9.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel9.TextStyle = TextStyles.Custom;
            this.statLosses.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statLosses.AutoGrowDirection = GrowDirections.None;
            this.statLosses.AutoSize = true;
            this.statLosses.AutoStyle = true;
            this.statLosses.BackColor = Color.Transparent;
            this.statLosses.Font = new Font("Arial", 9.75f);
            this.statLosses.ForeColor = Color.White;
            this.statLosses.IgnoreMouseWheel = false;
            this.statLosses.IsStyled = false;
            this.statLosses.Location = new Point(0x58, 0x1a5);
            this.statLosses.Name = "statLosses";
            this.statLosses.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statLosses, null);
            this.statLosses.TabIndex = 14;
            this.statLosses.Text = "2258";
            this.statLosses.TextStyle = TextStyles.Bold;
            this.gpgLabel7.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel7.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x54, 0x191);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 13;
            this.gpgLabel7.Text = "<LOC>Losses";
            this.gpgLabel7.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel7.TextStyle = TextStyles.Custom;
            this.statWins.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statWins.AutoGrowDirection = GrowDirections.None;
            this.statWins.AutoSize = true;
            this.statWins.AutoStyle = true;
            this.statWins.BackColor = Color.Transparent;
            this.statWins.Font = new Font("Arial", 9.75f);
            this.statWins.ForeColor = Color.White;
            this.statWins.IgnoreMouseWheel = false;
            this.statWins.IsStyled = false;
            this.statWins.Location = new Point(6, 0x1a5);
            this.statWins.Name = "statWins";
            this.statWins.Size = new Size(15, 0x10);
            base.ttDefault.SetSuperTip(this.statWins, null);
            this.statWins.TabIndex = 12;
            this.statWins.Text = "1";
            this.statWins.TextStyle = TextStyles.Bold;
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel6.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(6, 0x191);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x22b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 11;
            this.gpgLabel6.Text = "<LOC>Wins";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.statRank.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statRank.AutoGrowDirection = GrowDirections.None;
            this.statRank.AutoSize = true;
            this.statRank.AutoStyle = true;
            this.statRank.BackColor = Color.Transparent;
            this.statRank.Font = new Font("Arial", 9.75f);
            this.statRank.ForeColor = Color.White;
            this.statRank.IgnoreMouseWheel = false;
            this.statRank.IsStyled = false;
            this.statRank.Location = new Point(6, 0x17f);
            this.statRank.Name = "statRank";
            this.statRank.Size = new Size(15, 0x10);
            base.ttDefault.SetSuperTip(this.statRank, null);
            this.statRank.TabIndex = 4;
            this.statRank.Text = "1";
            this.statRank.TextStyle = TextStyles.Bold;
            this.statRating.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statRating.AutoGrowDirection = GrowDirections.None;
            this.statRating.AutoSize = true;
            this.statRating.AutoStyle = true;
            this.statRating.BackColor = Color.Transparent;
            this.statRating.Font = new Font("Arial", 9.75f);
            this.statRating.ForeColor = Color.White;
            this.statRating.IgnoreMouseWheel = false;
            this.statRating.IsStyled = false;
            this.statRating.Location = new Point(0x58, 0x17f);
            this.statRating.Name = "statRating";
            this.statRating.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statRating, null);
            this.statRating.TabIndex = 7;
            this.statRating.Text = "2258";
            this.statRating.TextStyle = TextStyles.Bold;
            this.gpgLabel3.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel3.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0xb6, 0x16b);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x75, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 6;
            this.gpgLabel3.Text = "<LOC>Total Games";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.statTotalGames.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.statTotalGames.AutoGrowDirection = GrowDirections.None;
            this.statTotalGames.AutoSize = true;
            this.statTotalGames.AutoStyle = true;
            this.statTotalGames.BackColor = Color.Transparent;
            this.statTotalGames.Font = new Font("Arial", 9.75f);
            this.statTotalGames.ForeColor = Color.White;
            this.statTotalGames.IgnoreMouseWheel = false;
            this.statTotalGames.IsStyled = false;
            this.statTotalGames.Location = new Point(0xb8, 0x17f);
            this.statTotalGames.Name = "statTotalGames";
            this.statTotalGames.Size = new Size(0x1d, 0x10);
            base.ttDefault.SetSuperTip(this.statTotalGames, null);
            this.statTotalGames.TabIndex = 8;
            this.statTotalGames.Text = "152";
            this.statTotalGames.TextStyle = TextStyles.Bold;
            this.gpgLabel2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel2.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x54, 0x16b);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 5;
            this.gpgLabel2.Text = "<LOC>Rating";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel1.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(6, 0x16b);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x22b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 3;
            this.gpgLabel1.Text = "<LOC>Rank";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.skinButtonWebVersion.Anchor = AnchorStyles.Bottom;
            this.skinButtonWebVersion.AutoStyle = true;
            this.skinButtonWebVersion.BackColor = Color.Transparent;
            this.skinButtonWebVersion.ButtonState = 0;
            this.skinButtonWebVersion.DialogResult = DialogResult.OK;
            this.skinButtonWebVersion.DisabledForecolor = Color.Gray;
            this.skinButtonWebVersion.DrawColor = Color.White;
            this.skinButtonWebVersion.DrawEdges = true;
            this.skinButtonWebVersion.FocusColor = Color.Yellow;
            this.skinButtonWebVersion.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonWebVersion.ForeColor = Color.White;
            this.skinButtonWebVersion.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonWebVersion.IsStyled = true;
            this.skinButtonWebVersion.Location = new Point(0xca, 0x20a);
            this.skinButtonWebVersion.Name = "skinButtonWebVersion";
            this.skinButtonWebVersion.Size = new Size(0xb3, 0x1a);
            this.skinButtonWebVersion.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonWebVersion, null);
            this.skinButtonWebVersion.TabIndex = 1;
            this.skinButtonWebVersion.TabStop = true;
            this.skinButtonWebVersion.Text = "<LOC>Show Web Versions";
            this.skinButtonWebVersion.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonWebVersion.TextPadding = new Padding(0);
            this.skinButtonWebVersion.Click += new EventHandler(this.skinButtonWebVersions_Click);
            this.skinLabelHeader.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabelHeader.AutoStyle = false;
            this.skinLabelHeader.BackColor = Color.Transparent;
            this.skinLabelHeader.DrawEdges = true;
            this.skinLabelHeader.Font = new Font("Verdana", 18f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.skinLabelHeader.ForeColor = Color.White;
            this.skinLabelHeader.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelHeader.IsStyled = false;
            this.skinLabelHeader.Location = new Point(6, 8);
            this.skinLabelHeader.Name = "skinLabelHeader";
            this.skinLabelHeader.Size = new Size(0x238, 0x2f);
            this.skinLabelHeader.SkinBasePath = @"Dialog\ClanProfile\Header";
            base.ttDefault.SetSuperTip(this.skinLabelHeader, null);
            this.skinLabelHeader.TabIndex = 0;
            this.skinLabelHeader.Text = "<LOC>Clan Profile";
            this.skinLabelHeader.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabelHeader.TextPadding = new Padding(8, 0, 0, 0);
            this.pnlUserListClan.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlUserListClan.AutoRefresh = true;
            this.pnlUserListClan.AutoScroll = true;
            this.pnlUserListClan.BackColor = Color.FromArgb(0x24, 0x23, 0x23);
            this.pnlUserListClan.Location = new Point(0, 0x15);
            this.pnlUserListClan.Name = "pnlUserListClan";
            this.pnlUserListClan.Size = new Size(0xfb, 0x217);
            this.pnlUserListClan.Style = UserListStyles.Clan;
            base.ttDefault.SetSuperTip(this.pnlUserListClan, null);
            this.pnlUserListClan.TabIndex = 11;
            this.skinLabelRoster.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabelRoster.AutoStyle = false;
            this.skinLabelRoster.BackColor = Color.Transparent;
            this.skinLabelRoster.Controls.Add(this.pictureBox3);
            this.skinLabelRoster.Controls.Add(this.pictureBox2);
            this.skinLabelRoster.Controls.Add(this.pictureBox1);
            this.skinLabelRoster.DrawEdges = true;
            this.skinLabelRoster.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelRoster.ForeColor = Color.White;
            this.skinLabelRoster.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelRoster.IsStyled = false;
            this.skinLabelRoster.Location = new Point(0, 0);
            this.skinLabelRoster.Name = "skinLabelRoster";
            this.skinLabelRoster.Size = new Size(250, 20);
            this.skinLabelRoster.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabelRoster, null);
            this.skinLabelRoster.TabIndex = 10;
            this.skinLabelRoster.Text = "<LOC>Clan Roster ({0})";
            this.skinLabelRoster.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabelRoster.TextPadding = new Padding(0x24, 0, 0, 0);
            this.pictureBox3.Image = (Image) manager.GetObject("pictureBox3.Image");
            this.pictureBox3.Location = new Point(0x17, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Size(10, 15);
            this.pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pictureBox3, null);
            this.pictureBox3.TabIndex = 13;
            this.pictureBox3.TabStop = false;
            this.pictureBox2.Image = (Image) manager.GetObject("pictureBox2.Image");
            this.pictureBox2.Location = new Point(13, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(10, 15);
            this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pictureBox2, null);
            this.pictureBox2.TabIndex = 12;
            this.pictureBox2.TabStop = false;
            this.pictureBox1.Image = (Image) manager.GetObject("pictureBox1.Image");
            this.pictureBox1.Location = new Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(10, 15);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pictureBox1, null);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x350, 0x288);
            base.Controls.Add(this.splitContainerMain);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(750, 550);
            base.Name = "DlgClanProfileEx";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Clan Profile";
            base.Controls.SetChildIndex(this.splitContainerMain, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.skinGroupPanelClan.ResumeLayout(false);
            this.skinGroupPanelClan.PerformLayout();
            this.gpgPanelWebsite.ResumeLayout(false);
            this.gpgPanelWebsite.PerformLayout();
            ((ISupportInitialize) this.gpgPictureBoxWebsite).EndInit();
            this.gpgTextBoxWebsite.Properties.EndInit();
            ((ISupportInitialize) this.gpgPictureBoxAvatar).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxEdit).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxCancel).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxSave).EndInit();
            this.gpgPanelDescription.ResumeLayout(false);
            this.gpgTextAreaDescription.Properties.EndInit();
            this.skinLabelRoster.ResumeLayout(false);
            this.skinLabelRoster.PerformLayout();
            ((ISupportInitialize) this.pictureBox3).EndInit();
            ((ISupportInitialize) this.pictureBox2).EndInit();
            ((ISupportInitialize) this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            WaitCallback callBack = null;
            base.OnSizeChanged(e);
            if (this.pnlUserListClan != null)
            {
                if (callBack == null)
                {
                    callBack = delegate (object state) {
                        this.pnlUserListClan.RefreshData();
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
        }

        internal bool SetCurrentClanByName(string clanName)
        {
            if (this.ClanNameLookup.ContainsKey(clanName))
            {
                this.mCurrentClan = this.ClanNameLookup[clanName];
                return true;
            }
            if (((Clan.Current != null) && (ClanMember.Current != null)) && (clanName == Clan.Current.Name))
            {
                this.mCurrentClan = new ClanView(Clan.Current, Clan.CurrentMembers, clanName, false);
                this.ClanNameLookup.Add(clanName, this.CurrentClan);
                return true;
            }
            MappedObjectList<Clan> objects = DataAccess.GetObjects<Clan>("GetClanByName", new object[] { clanName });
            if (objects.Count > 0)
            {
                MappedObjectList<ClanMember> currentMembers;
                Clan clan = objects[0];
                if ((Clan.Current != null) && clan.Equals(Clan.Current))
                {
                    currentMembers = Clan.CurrentMembers;
                }
                else
                {
                    currentMembers = DataAccess.GetObjects<ClanMember>("GetClanMembers", new object[] { clan.ID });
                }
                this.mCurrentClan = new ClanView(clan, currentMembers, clanName, false);
                this.ClanNameLookup.Add(clanName, this.CurrentClan);
                return true;
            }
            base.MainForm.ErrorMessage(Loc.Get("<LOC>Unable to locate clan '{0}'"), new object[] { clanName });
            if (this.ViewList.Count < 1)
            {
                base.Dispose();
            }
            return false;
        }

        internal bool SetCurrentClanByPlayer(string playerName)
        {
            if (this.PlayerNameLookup.ContainsKey(playerName))
            {
                this.mCurrentClan = this.PlayerNameLookup[playerName];
                return true;
            }
            if (((Clan.Current != null) && (ClanMember.Current != null)) && (playerName == ClanMember.Current.Name))
            {
                this.mCurrentClan = new ClanView(Clan.Current, Clan.CurrentMembers, playerName, true);
                this.PlayerNameLookup.Add(playerName, this.CurrentClan);
                return true;
            }
            MappedObjectList<Clan> objects = DataAccess.GetObjects<Clan>("GetClanByMember2", new object[] { playerName });
            if (objects.Count > 0)
            {
                MappedObjectList<ClanMember> currentMembers;
                Clan clan = objects[0];
                if ((Clan.Current != null) && clan.Equals(Clan.Current))
                {
                    currentMembers = Clan.CurrentMembers;
                }
                else
                {
                    currentMembers = DataAccess.GetObjects<ClanMember>("GetClanMembers", new object[] { clan.ID });
                }
                this.mCurrentClan = new ClanView(clan, currentMembers, playerName, true);
                this.PlayerNameLookup.Add(playerName, this.CurrentClan);
                return true;
            }
            base.MainForm.ErrorMessage(Loc.Get("<LOC>Unable to locate clan for player '{0}'"), new object[] { playerName });
            if (this.ViewList.Count < 1)
            {
                base.Dispose();
            }
            return false;
        }

        private void skinButtonNextProfile_Click(object sender, EventArgs e)
        {
            this.mCurrentClan = this.ViewList.Find(this.CurrentClan).Next.Value;
            this.Construct();
        }

        private void skinButtonPreviousProfile_Click(object sender, EventArgs e)
        {
            this.mCurrentClan = this.ViewList.Find(this.CurrentClan).Previous.Value;
            this.Construct();
        }

        private void skinButtonWebVersions_Click(object sender, EventArgs e)
        {
            Program.Settings.Profiles.ShowWebVersions = !Program.Settings.Profiles.ShowWebVersions;
            this.ToggleProfileBody();
        }

        private void ToggleProfileBody()
        {
            if (Program.Settings.Profiles.ShowWebVersions)
            {
                this.skinButtonWebVersion.Text = Loc.Get("<LOC>Hide Web Versions");
            }
            else
            {
                this.skinButtonWebVersion.Text = Loc.Get("<LOC>Show Web Versions");
            }
            if ((this.CurrentClan.Clan.Website != null) && (this.CurrentClan.Clan.Website.Length > 0))
            {
                this.gpgLabelWebsite.Text = this.CurrentClan.Clan.Website;
                this.gpgLabelWebsite.Refresh();
                this.gpgPictureBoxWebsite.Show();
                if (Program.Settings.Profiles.ShowWebVersions && this.CurrentClan.Clan.EmbedWebsite)
                {
                    this.gpgLabelDescription.Text = null;
                    this.gpgLabelDescription.Visible = false;
                    this.webBrowserProfile.Navigate(this.CurrentClan.Clan.Website);
                    this.webBrowserProfile.Show();
                    this.webBrowserProfile.BringToFront();
                }
                else
                {
                    this.gpgLabelDescription.Text = this.CurrentClan.Clan.Description;
                    this.gpgLabelDescription.Visible = true;
                    this.webBrowserProfile.Url = null;
                    this.webBrowserProfile.Hide();
                }
            }
            else
            {
                this.gpgLabelDescription.Text = this.CurrentClan.Clan.Description;
                this.gpgLabelDescription.Visible = true;
                this.gpgLabelWebsite.Text = null;
                this.gpgPictureBoxWebsite.Hide();
                this.webBrowserProfile.Url = null;
                this.webBrowserProfile.Hide();
            }
        }

        public ClanView CurrentClan
        {
            get
            {
                return this.mCurrentClan;
            }
        }

        private string EmbedWebEffortName
        {
            get
            {
                return ConfigSettings.GetString("EmbedWebEffortName", "Embedded Clan Website");
            }
        }

        internal bool HasVolunteeredForEmbeddedWeb
        {
            get
            {
                if (!this.mHasVolunteeredForEmbeddedWeb.HasValue)
                {
                    this.mHasVolunteeredForEmbeddedWeb = new bool?(VolunteerEffort.HasVolunteeredForEffort(this.EmbedWebEffortName));
                }
                return this.mHasVolunteeredForEmbeddedWeb.Value;
            }
        }

        public LinkedList<ClanView> ViewList
        {
            get
            {
                return this.mViewList;
            }
        }
    }
}

