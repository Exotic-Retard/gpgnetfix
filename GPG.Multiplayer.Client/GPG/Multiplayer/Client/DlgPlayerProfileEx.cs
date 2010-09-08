namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Controls.Awards;
    using GPG.Multiplayer.Client.Controls.UserList;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Statistics;
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

    public class DlgPlayerProfileEx : DlgBase
    {
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
        private GPGLabel gpgLabelClanTag;
        private GPGLabel gpgLabelDescription;
        private GPGPanel gpgPanelAwards;
        private GPGPanel gpgPanelDescription;
        private GPGPictureBox gpgPictureBoxAvatar;
        private GPGPictureBox gpgPictureBoxAward1;
        private GPGPictureBox gpgPictureBoxAward2;
        private GPGPictureBox gpgPictureBoxAward3;
        private GPGPictureBox gpgPictureBoxCancel;
        private GPGPictureBox gpgPictureBoxEdit;
        private GPGPictureBox gpgPictureBoxSave;
        private GPGTextArea gpgTextAreaDescription;
        private bool IsFirstPaint = true;
        private bool IsInitialized = false;
        private PlayerView mCurrentPlayer;
        private PlayerRating mCurrentStats = null;
        private SortedList<int, LifetimeAwardInfo> mLifetimeAwards;
        private LinkedList<PlayerView> mViewList = new LinkedList<PlayerView>();
        private bool PerformingAwardsLayout = false;
        private PictureBox pictureBox1;
        private GPGPictureBox pictureBoxClanRank;
        private Dictionary<string, PlayerView> PlayerViewLookups = new Dictionary<string, PlayerView>();
        private PnlUserList pnlUserList;
        private SkinButton skinButtonNextProfile;
        private SkinButton skinButtonPreviousProfile;
        private SkinButton skinButtonWebStats;
        private SkinGroupPanel skinGroupPanelPlayer;
        private SkinLabel skinLabel1;
        private SkinLabel skinLabelFriends;
        private SkinLabel skinLabelHeader;
        private SplitContainer splitContainerMain;
        private GPGLabel statDiconnectPct;
        private GPGLabel statDisconnects;
        private GPGLabel statDraws;
        private GPGLabel statLosses;
        private GPGLabel statRank;
        private GPGLabel statRating;
        private GPGLabel statTotalGames;
        private GPGLabel statWinPct;
        private GPGLabel statWins;

        public DlgPlayerProfileEx()
        {
            this.InitializeComponent();
            this.IsInitialized = true;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;
            this.splitContainerMain.Paint += new PaintEventHandler(this.splitContainerMain_Paint);
            this.splitContainerMain.Panel2.Paint += new PaintEventHandler(this.splitContainerMain_Paint);
            if (!(GameInformation.SelectedGame.IsSpaceSiege || GameInformation.SelectedGame.IsChatOnly))
            {
                this.skinButtonWebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
            }
            this.skinButtonNextProfile.MakeEdgesTransparent();
            this.skinButtonPreviousProfile.MakeEdgesTransparent();
        }

        private void Avatar_Click(object sender, EventArgs e)
        {
            if (((this.CurrentPlayer.Player.ID == User.Current.ID) && !GameInformation.SelectedGame.IsSpaceSiege) && !GameInformation.SelectedGame.IsChatOnly)
            {
                DlgAvatarPicker picker = new DlgAvatarPicker(this.CurrentPlayer.Player);
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    Avatar avatar;
                    if (picker.SelectedAvatar == null)
                    {
                        avatar = Avatar.Default;
                    }
                    else
                    {
                        avatar = picker.SelectedAvatar;
                    }
                    this.gpgPictureBoxAvatar.Image = avatar.Image;
                    this.CurrentPlayer.Player.Avatar = avatar.ID;
                    User.Current.Avatar = avatar.ID;
                    ThreadPool.QueueUserWorkItem(delegate (object s) {
                        if (new QuazalQuery("SetAvatar", new object[] { avatar.ID }).ExecuteNonQuery())
                        {
                            this.MainForm.RefreshGathering();
                        }
                    });
                }
            }
        }

        internal void Construct()
        {
            if (GameInformation.SelectedGame.IsSpaceSiege || GameInformation.SelectedGame.IsChatOnly)
            {
                this.gpgLabel1.Hide();
                this.gpgLabel2.Hide();
                this.gpgLabel3.Hide();
                this.gpgLabel6.Hide();
                this.gpgLabel7.Hide();
                this.gpgLabel8.Hide();
                this.gpgLabel9.Hide();
                this.gpgLabel10.Hide();
                this.gpgLabel11.Hide();
                this.gpgPanelAwards.Hide();
                this.skinLabel1.Hide();
                this.statDiconnectPct.Hide();
                this.statDisconnects.Hide();
                this.statDraws.Hide();
                this.statLosses.Hide();
                this.statRank.Hide();
                this.statRating.Hide();
                this.statTotalGames.Hide();
                this.statWinPct.Hide();
                this.statWins.Hide();
                this.skinButtonWebStats.Hide();
            }
            if (!this.ViewList.Contains(this.CurrentPlayer))
            {
                this.ViewList.AddLast(new LinkedListNode<PlayerView>(this.CurrentPlayer));
            }
            if (!this.ViewList.Last.Value.Equals(this.CurrentPlayer))
            {
                this.skinButtonNextProfile.Enabled = true;
                base.ttDefault.SetToolTip(this.skinButtonNextProfile, Loc.Get(string.Format("<LOC>Forward to {0}", this.ViewList.Find(this.CurrentPlayer).Next.Value.Player.Name)));
            }
            else
            {
                this.skinButtonNextProfile.Enabled = false;
                base.ttDefault.SetToolTip(this.skinButtonNextProfile, "");
            }
            if (!this.ViewList.First.Value.Equals(this.CurrentPlayer))
            {
                this.skinButtonPreviousProfile.Enabled = true;
                base.ttDefault.SetToolTip(this.skinButtonPreviousProfile, Loc.Get(string.Format("<LOC>Back to {0}", this.ViewList.Find(this.CurrentPlayer).Previous.Value.Player.Name)));
            }
            else
            {
                this.skinButtonPreviousProfile.Enabled = false;
                base.ttDefault.SetToolTip(this.skinButtonPreviousProfile, "");
            }
            User player = this.CurrentPlayer.Player;
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                if (player.Description == null)
                {
                    player.Description = DataAccess.GetString("GetPlayerDescription", new object[] { player.ID });
                }
                if (!this.Disposing && !this.IsDisposed)
                {
                    PlayerDisplayAwards awards = new PlayerDisplayAwards(player);
                    Image avatar = awards.Avatar.Image;
                    this.Invoke((VGen0)delegate {
                        this.gpgPictureBoxAvatar.Image = avatar;
                        if (((player.Description == null) || (player.Description == "")) || (player.Description == "(null)"))
                        {
                            this.gpgLabelDescription.Text = Loc.Get("<LOC>(no description)");
                            this.gpgTextAreaDescription.Text = "";
                        }
                        else
                        {
                            this.gpgLabelDescription.Text = player.Description;
                            this.gpgTextAreaDescription.Text = player.Description;
                        }
                    });
                    this.CurrentPlayer.Friends = new QuazalQuery("GetFriendsByPlayerID", new object[] { player.ID }).GetObjects<User>();
                    this.pnlUserList.ClearData();
                    this.pnlUserList.AddUsers(this.CurrentPlayer.Friends);
                    this.pnlUserList.RefreshData();
                }
            });
            this.Text = string.Format(Loc.Get("<LOC>Profile: {0}"), player.Name);
            this.Refresh();
            this.skinGroupPanelPlayer.Text = player.Name;
            this.gpgPictureBoxSave.Visible = false;
            this.gpgPictureBoxCancel.Visible = false;
            if (player.Equals(User.Current))
            {
                this.gpgPictureBoxEdit.Visible = true;
                this.gpgPanelDescription.Width = this.gpgPictureBoxEdit.Left - (this.gpgPanelDescription.Left + 4);
                if (!(GameInformation.SelectedGame.IsSpaceSiege || GameInformation.SelectedGame.IsChatOnly))
                {
                    base.ttDefault.SetToolTip(this.gpgPictureBoxAvatar, Loc.Get("<LOC>Click here to change your avatar"));
                    this.gpgPictureBoxAvatar.Cursor = Cursors.Hand;
                }
                this.gpgPictureBoxAvatar.Click += new EventHandler(this.Avatar_Click);
            }
            else
            {
                this.gpgPictureBoxEdit.Visible = false;
                this.gpgPanelDescription.Width = this.gpgPictureBoxEdit.Right - this.gpgPanelDescription.Left;
                base.ttDefault.SetToolTip(this.gpgPictureBoxAvatar, Loc.Get(""));
                this.gpgPictureBoxAvatar.Cursor = Cursors.Default;
                this.gpgPictureBoxAvatar.Click -= new EventHandler(this.Avatar_Click);
            }
            if (player.IsInClan)
            {
                this.pictureBoxClanRank.Visible = true;
                this.gpgLabelClanTag.Visible = true;
                this.gpgLabelClanTag.Text = player.ClanAbbreviation;
                this.gpgLabelClanTag.Left = (this.skinGroupPanelPlayer.TextPadding.Left + this.skinGroupPanelPlayer.HeaderLabel.TextWidth) + 8;
                this.gpgLabelClanTag.BringToFront();
                this.gpgLabelClanTag.ForeColor = Program.Settings.Chat.Appearance.ClanColor;
                base.ttDefault.SetToolTip(this.gpgLabelClanTag, player.ClanName);
                string imageSource = ClanRanking.FindBySeniority(player.Rank).ImageSource;
                object obj2 = ClanImages.ResourceManager.GetObject(imageSource);
                this.pictureBoxClanRank.Image = obj2 as Image;
                base.ttDefault.SetToolTip(this.pictureBoxClanRank, ClanRanking.FindBySeniority(player.Rank).Description);
                this.pictureBoxClanRank.Left = this.gpgLabelClanTag.Right + 4;
            }
            else
            {
                this.pictureBoxClanRank.Visible = false;
                this.gpgLabelClanTag.Visible = false;
            }
            this.mCurrentStats = this.CurrentPlayer.Player.Rating_1v1;
            this.LoadStats();
            this.LoadLifetimeAwards();
            this.LoadDisplayAwards();
        }

        private void DisplayAwardClick(object sender, EventArgs e)
        {
            DlgAwardPicker picker = new DlgAwardPicker(this.CurrentPlayer.Player);
            if (picker.ShowDialog() == DialogResult.OK)
            {
                PictureBox box = sender as PictureBox;
                int num = int.Parse((string) box.Tag);
                int iD = 0;
                if (picker.SelectedAward != null)
                {
                    iD = picker.SelectedAward.ID;
                }
                switch (num)
                {
                    case 1:
                        if (!new QuazalQuery("SetDisplayAward", new object[] { num, iD }).ExecuteNonQuery())
                        {
                            DlgMessage.ShowDialog(Program.MainForm, "<LOC>Error", "An error occured updating awards, please try again later.");
                            break;
                        }
                        this.CurrentPlayer.Player.Award1 = iD;
                        User.Current.Award1 = iD;
                        base.MainForm.RefreshGathering();
                        break;

                    case 2:
                        if (!new QuazalQuery("SetDisplayAward", new object[] { num, iD }).ExecuteNonQuery())
                        {
                            DlgMessage.ShowDialog(Program.MainForm, "<LOC>Error", "An error occured updating awards, please try again later.");
                            break;
                        }
                        this.CurrentPlayer.Player.Award2 = iD;
                        User.Current.Award2 = iD;
                        base.MainForm.RefreshGathering();
                        break;

                    case 3:
                        if (!new QuazalQuery("SetDisplayAward", new object[] { num, iD }).ExecuteNonQuery())
                        {
                            DlgMessage.ShowDialog(Program.MainForm, "<LOC>Error", "An error occured updating awards, please try again later.");
                            break;
                        }
                        this.CurrentPlayer.Player.Award3 = iD;
                        User.Current.Award3 = iD;
                        base.MainForm.RefreshGathering();
                        break;
                }
                this.LoadDisplayAwards();
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

        private void gpgLabelClanTag_Click(object sender, EventArgs e)
        {
            base.MainForm.OnViewClanProfileByPlayer(this.CurrentPlayer.Player.Name);
        }

        private void gpgPictureBoxCancel_Click(object sender, EventArgs e)
        {
            this.gpgPictureBoxEdit.Visible = true;
            this.gpgPictureBoxSave.Visible = false;
            this.gpgPictureBoxCancel.Visible = false;
            this.gpgTextAreaDescription.Visible = false;
        }

        private void gpgPictureBoxEdit_Click(object sender, EventArgs e)
        {
            this.gpgPictureBoxEdit.Visible = false;
            this.gpgPictureBoxSave.Visible = true;
            this.gpgPictureBoxCancel.Visible = true;
            this.gpgTextAreaDescription.Text = this.gpgLabelDescription.Text;
            this.gpgTextAreaDescription.Visible = true;
            this.gpgTextAreaDescription.BringToFront();
        }

        private void gpgPictureBoxSave_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (Profanity.ContainsProfanity(this.gpgTextAreaDescription.Text))
            {
                DlgMessage.ShowDialog("<LOC>Your profile may not contain profanity.\r\nPlease enter a valid profile description.", "<LOC>Error");
            }
            else
            {
                DataAccess.ExecuteQuery("CreatePlayerInfo", new object[0]);
                if (!DataAccess.ExecuteQuery("SetPlayerDescription", new object[] { this.gpgTextAreaDescription.Text }))
                {
                    base.Error(this.gpgTextAreaDescription, "<LOC>Failed to save description", new object[0]);
                }
                else
                {
                    this.gpgPictureBoxEdit.Visible = true;
                    this.gpgPictureBoxSave.Visible = false;
                    this.gpgPictureBoxCancel.Visible = false;
                    User.Current.Description = this.gpgTextAreaDescription.Text;
                    this.CurrentPlayer.Player.Description = this.gpgTextAreaDescription.Text;
                    this.gpgLabelDescription.Text = this.gpgTextAreaDescription.Text;
                    this.gpgTextAreaDescription.Visible = false;
                }
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgPlayerProfileEx));
            this.splitContainerMain = new SplitContainer();
            this.skinButtonNextProfile = new SkinButton();
            this.skinButtonPreviousProfile = new SkinButton();
            this.skinGroupPanelPlayer = new SkinGroupPanel();
            this.gpgPictureBoxAward1 = new GPGPictureBox();
            this.gpgPictureBoxAward2 = new GPGPictureBox();
            this.gpgPictureBoxAward3 = new GPGPictureBox();
            this.gpgPictureBoxAvatar = new GPGPictureBox();
            this.gpgPictureBoxCancel = new GPGPictureBox();
            this.gpgPictureBoxSave = new GPGPictureBox();
            this.gpgPictureBoxEdit = new GPGPictureBox();
            this.pictureBoxClanRank = new GPGPictureBox();
            this.statDiconnectPct = new GPGLabel();
            this.gpgLabel11 = new GPGLabel();
            this.gpgLabelClanTag = new GPGLabel();
            this.statWinPct = new GPGLabel();
            this.gpgLabel10 = new GPGLabel();
            this.statDisconnects = new GPGLabel();
            this.statDraws = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.statLosses = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.statWins = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgPanelAwards = new GPGPanel();
            this.statRank = new GPGLabel();
            this.statRating = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.statTotalGames = new GPGLabel();
            this.skinLabel1 = new SkinLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgPanelDescription = new GPGPanel();
            this.gpgLabelDescription = new GPGLabel();
            this.gpgTextAreaDescription = new GPGTextArea();
            this.skinButtonWebStats = new SkinButton();
            this.skinLabelHeader = new SkinLabel();
            this.pnlUserList = new PnlUserList();
            this.skinLabelFriends = new SkinLabel();
            this.pictureBox1 = new PictureBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.skinGroupPanelPlayer.SuspendLayout();
            ((ISupportInitialize) this.gpgPictureBoxAward1).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxAward2).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxAward3).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxAvatar).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxCancel).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxSave).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxEdit).BeginInit();
            ((ISupportInitialize) this.pictureBoxClanRank).BeginInit();
            this.gpgPanelDescription.SuspendLayout();
            this.gpgTextAreaDescription.Properties.BeginInit();
            this.skinLabelFriends.SuspendLayout();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x315, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.splitContainerMain.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerMain.Location = new Point(7, 0x3d);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Panel1.BackgroundImage = (Image) manager.GetObject("splitContainerMain.Panel1.BackgroundImage");
            this.splitContainerMain.Panel1.Controls.Add(this.skinButtonNextProfile);
            this.splitContainerMain.Panel1.Controls.Add(this.skinButtonPreviousProfile);
            this.splitContainerMain.Panel1.Controls.Add(this.skinGroupPanelPlayer);
            this.splitContainerMain.Panel1.Controls.Add(this.skinButtonWebStats);
            this.splitContainerMain.Panel1.Controls.Add(this.skinLabelHeader);
            base.ttDefault.SetSuperTip(this.splitContainerMain.Panel1, null);
            this.splitContainerMain.Panel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerMain.Panel2.Controls.Add(this.pnlUserList);
            this.splitContainerMain.Panel2.Controls.Add(this.skinLabelFriends);
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
            this.skinButtonNextProfile.Location = new Point(0x166, 0x20a);
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
            this.skinButtonPreviousProfile.Location = new Point(0xbf, 0x20a);
            this.skinButtonPreviousProfile.Name = "skinButtonPreviousProfile";
            this.skinButtonPreviousProfile.Size = new Size(30, 30);
            this.skinButtonPreviousProfile.SkinBasePath = @"Dialog\PlayerProfile\PreviousProfile";
            base.ttDefault.SetSuperTip(this.skinButtonPreviousProfile, null);
            this.skinButtonPreviousProfile.TabIndex = 3;
            this.skinButtonPreviousProfile.TabStop = true;
            this.skinButtonPreviousProfile.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonPreviousProfile.TextPadding = new Padding(0);
            this.skinButtonPreviousProfile.Click += new EventHandler(this.skinButtonPreviousProfile_Click);
            this.skinGroupPanelPlayer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.skinGroupPanelPlayer.AutoStyle = false;
            this.skinGroupPanelPlayer.BackColor = Color.Black;
            this.skinGroupPanelPlayer.BackgroundImage = (Image) manager.GetObject("skinGroupPanelPlayer.BackgroundImage");
            this.skinGroupPanelPlayer.BackgroundImageLayout = ImageLayout.Stretch;
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPictureBoxAward1);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPictureBoxAward2);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPictureBoxAward3);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPictureBoxAvatar);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPictureBoxCancel);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPictureBoxSave);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPictureBoxEdit);
            this.skinGroupPanelPlayer.Controls.Add(this.pictureBoxClanRank);
            this.skinGroupPanelPlayer.Controls.Add(this.statDiconnectPct);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel11);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabelClanTag);
            this.skinGroupPanelPlayer.Controls.Add(this.statWinPct);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel10);
            this.skinGroupPanelPlayer.Controls.Add(this.statDisconnects);
            this.skinGroupPanelPlayer.Controls.Add(this.statDraws);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel8);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel9);
            this.skinGroupPanelPlayer.Controls.Add(this.statLosses);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel7);
            this.skinGroupPanelPlayer.Controls.Add(this.statWins);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel6);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPanelAwards);
            this.skinGroupPanelPlayer.Controls.Add(this.statRank);
            this.skinGroupPanelPlayer.Controls.Add(this.statRating);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel3);
            this.skinGroupPanelPlayer.Controls.Add(this.statTotalGames);
            this.skinGroupPanelPlayer.Controls.Add(this.skinLabel1);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel2);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgLabel1);
            this.skinGroupPanelPlayer.Controls.Add(this.gpgPanelDescription);
            this.skinGroupPanelPlayer.CutCorner = false;
            this.skinGroupPanelPlayer.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.skinGroupPanelPlayer.ForeColor = Color.White;
            this.skinGroupPanelPlayer.HeaderImage = (Image) manager.GetObject("skinGroupPanelPlayer.HeaderImage");
            this.skinGroupPanelPlayer.IsStyled = true;
            this.skinGroupPanelPlayer.Location = new Point(6, 0x43);
            this.skinGroupPanelPlayer.Margin = new Padding(7, 6, 7, 6);
            this.skinGroupPanelPlayer.Name = "skinGroupPanelPlayer";
            this.skinGroupPanelPlayer.Size = new Size(0x238, 0x1bb);
            base.ttDefault.SetSuperTip(this.skinGroupPanelPlayer, null);
            this.skinGroupPanelPlayer.TabIndex = 2;
            this.skinGroupPanelPlayer.Text = "player";
            this.skinGroupPanelPlayer.TextAlign = ContentAlignment.MiddleLeft;
            this.skinGroupPanelPlayer.TextPadding = new Padding(0x34, 0, 0, 0);
            this.gpgPictureBoxAward1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxAward1.BackColor = Color.Transparent;
            this.gpgPictureBoxAward1.Location = new Point(0x1e9, 9);
            this.gpgPictureBoxAward1.Name = "gpgPictureBoxAward1";
            this.gpgPictureBoxAward1.Size = new Size(20, 20);
            this.gpgPictureBoxAward1.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.gpgPictureBoxAward1, null);
            this.gpgPictureBoxAward1.TabIndex = 0x1f;
            this.gpgPictureBoxAward1.TabStop = false;
            this.gpgPictureBoxAward1.Tag = "1";
            this.gpgPictureBoxAward2.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxAward2.BackColor = Color.Transparent;
            this.gpgPictureBoxAward2.Location = new Point(0x203, 9);
            this.gpgPictureBoxAward2.Name = "gpgPictureBoxAward2";
            this.gpgPictureBoxAward2.Size = new Size(20, 20);
            this.gpgPictureBoxAward2.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.gpgPictureBoxAward2, null);
            this.gpgPictureBoxAward2.TabIndex = 30;
            this.gpgPictureBoxAward2.TabStop = false;
            this.gpgPictureBoxAward2.Tag = "2";
            this.gpgPictureBoxAward3.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxAward3.BackColor = Color.Transparent;
            this.gpgPictureBoxAward3.Location = new Point(0x21d, 9);
            this.gpgPictureBoxAward3.Name = "gpgPictureBoxAward3";
            this.gpgPictureBoxAward3.Size = new Size(20, 20);
            this.gpgPictureBoxAward3.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.gpgPictureBoxAward3, null);
            this.gpgPictureBoxAward3.TabIndex = 0x1d;
            this.gpgPictureBoxAward3.TabStop = false;
            this.gpgPictureBoxAward3.Tag = "3";
            this.gpgPictureBoxAvatar.BackColor = Color.Transparent;
            this.gpgPictureBoxAvatar.Location = new Point(6, 9);
            this.gpgPictureBoxAvatar.Name = "gpgPictureBoxAvatar";
            this.gpgPictureBoxAvatar.Size = new Size(40, 20);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxAvatar, null);
            this.gpgPictureBoxAvatar.TabIndex = 0x1c;
            this.gpgPictureBoxAvatar.TabStop = false;
            this.gpgPictureBoxCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxCancel.Cursor = Cursors.Hand;
            this.gpgPictureBoxCancel.Image = Resources.cancel_small;
            this.gpgPictureBoxCancel.Location = new Point(0x221, 0x58);
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
            this.gpgPictureBoxSave.Location = new Point(0x221, 0x42);
            this.gpgPictureBoxSave.Name = "gpgPictureBoxSave";
            this.gpgPictureBoxSave.Size = new Size(0x10, 0x10);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxSave, null);
            this.gpgPictureBoxSave.TabIndex = 0x1a;
            this.gpgPictureBoxSave.TabStop = false;
            base.ttDefault.SetToolTip(this.gpgPictureBoxSave, "<LOC>Save");
            this.gpgPictureBoxSave.Click += new EventHandler(this.gpgPictureBoxSave_Click);
            this.gpgPictureBoxEdit.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxEdit.Cursor = Cursors.Hand;
            this.gpgPictureBoxEdit.Image = Resources.edit_small;
            this.gpgPictureBoxEdit.Location = new Point(0x221, 0x2c);
            this.gpgPictureBoxEdit.Name = "gpgPictureBoxEdit";
            this.gpgPictureBoxEdit.Size = new Size(0x10, 0x10);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxEdit, null);
            this.gpgPictureBoxEdit.TabIndex = 0x19;
            this.gpgPictureBoxEdit.TabStop = false;
            base.ttDefault.SetToolTip(this.gpgPictureBoxEdit, "<LOC>Edit");
            this.gpgPictureBoxEdit.Click += new EventHandler(this.gpgPictureBoxEdit_Click);
            this.pictureBoxClanRank.BackColor = Color.Transparent;
            this.pictureBoxClanRank.Location = new Point(0xbd, 9);
            this.pictureBoxClanRank.Name = "pictureBoxClanRank";
            this.pictureBoxClanRank.Size = new Size(40, 20);
            base.ttDefault.SetSuperTip(this.pictureBoxClanRank, null);
            this.pictureBoxClanRank.TabIndex = 0x18;
            this.pictureBoxClanRank.TabStop = false;
            this.statDiconnectPct.AutoGrowDirection = GrowDirections.None;
            this.statDiconnectPct.AutoSize = true;
            this.statDiconnectPct.AutoStyle = true;
            this.statDiconnectPct.BackColor = Color.Transparent;
            this.statDiconnectPct.Font = new Font("Arial", 9.75f);
            this.statDiconnectPct.ForeColor = Color.White;
            this.statDiconnectPct.IgnoreMouseWheel = false;
            this.statDiconnectPct.IsStyled = false;
            this.statDiconnectPct.Location = new Point(0x17e, 0xc2);
            this.statDiconnectPct.Name = "statDiconnectPct";
            this.statDiconnectPct.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statDiconnectPct, null);
            this.statDiconnectPct.TabIndex = 0x15;
            this.statDiconnectPct.Text = "2258";
            this.statDiconnectPct.TextStyle = TextStyles.Bold;
            this.gpgLabel11.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel11.AutoStyle = true;
            this.gpgLabel11.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel11.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel11.ForeColor = Color.White;
            this.gpgLabel11.IgnoreMouseWheel = false;
            this.gpgLabel11.IsStyled = false;
            this.gpgLabel11.Location = new Point(0x17e, 0xae);
            this.gpgLabel11.Name = "gpgLabel11";
            this.gpgLabel11.Size = new Size(0x6c, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel11, null);
            this.gpgLabel11.TabIndex = 20;
            this.gpgLabel11.Text = "<LOC>Disconnect %";
            this.gpgLabel11.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel11.TextStyle = TextStyles.Custom;
            this.gpgLabelClanTag.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelClanTag.AutoSize = true;
            this.gpgLabelClanTag.AutoStyle = true;
            this.gpgLabelClanTag.BackColor = Color.Transparent;
            this.gpgLabelClanTag.Cursor = Cursors.Hand;
            this.gpgLabelClanTag.Font = new Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelClanTag.ForeColor = Color.Maroon;
            this.gpgLabelClanTag.IgnoreMouseWheel = false;
            this.gpgLabelClanTag.IsStyled = false;
            this.gpgLabelClanTag.Location = new Point(140, 9);
            this.gpgLabelClanTag.Name = "gpgLabelClanTag";
            this.gpgLabelClanTag.Size = new Size(0x2b, 0x12);
            base.ttDefault.SetSuperTip(this.gpgLabelClanTag, null);
            this.gpgLabelClanTag.TabIndex = 0x17;
            this.gpgLabelClanTag.Text = "clan";
            this.gpgLabelClanTag.TextStyle = TextStyles.Custom;
            this.gpgLabelClanTag.Click += new EventHandler(this.gpgLabelClanTag_Click);
            this.statWinPct.AutoGrowDirection = GrowDirections.None;
            this.statWinPct.AutoSize = true;
            this.statWinPct.AutoStyle = true;
            this.statWinPct.BackColor = Color.Transparent;
            this.statWinPct.Font = new Font("Arial", 9.75f);
            this.statWinPct.ForeColor = Color.White;
            this.statWinPct.IgnoreMouseWheel = false;
            this.statWinPct.IsStyled = false;
            this.statWinPct.Location = new Point(0x112, 0x9c);
            this.statWinPct.Name = "statWinPct";
            this.statWinPct.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statWinPct, null);
            this.statWinPct.TabIndex = 0x16;
            this.statWinPct.Text = "2258";
            this.statWinPct.TextStyle = TextStyles.Bold;
            this.gpgLabel10.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel10.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(0x112, 0xae);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0x68, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel10, null);
            this.gpgLabel10.TabIndex = 0x12;
            this.gpgLabel10.Text = "<LOC>Disconnects";
            this.gpgLabel10.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel10.TextStyle = TextStyles.Custom;
            this.statDisconnects.AutoGrowDirection = GrowDirections.None;
            this.statDisconnects.AutoSize = true;
            this.statDisconnects.AutoStyle = true;
            this.statDisconnects.BackColor = Color.Transparent;
            this.statDisconnects.Font = new Font("Arial", 9.75f);
            this.statDisconnects.ForeColor = Color.White;
            this.statDisconnects.IgnoreMouseWheel = false;
            this.statDisconnects.IsStyled = false;
            this.statDisconnects.Location = new Point(0x112, 0xc2);
            this.statDisconnects.Name = "statDisconnects";
            this.statDisconnects.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statDisconnects, null);
            this.statDisconnects.TabIndex = 0x13;
            this.statDisconnects.Text = "2258";
            this.statDisconnects.TextStyle = TextStyles.Bold;
            this.statDraws.AutoGrowDirection = GrowDirections.None;
            this.statDraws.AutoSize = true;
            this.statDraws.AutoStyle = true;
            this.statDraws.BackColor = Color.Transparent;
            this.statDraws.Font = new Font("Arial", 9.75f);
            this.statDraws.ForeColor = Color.White;
            this.statDraws.IgnoreMouseWheel = false;
            this.statDraws.IsStyled = false;
            this.statDraws.Location = new Point(0xb8, 0xc2);
            this.statDraws.Name = "statDraws";
            this.statDraws.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statDraws, null);
            this.statDraws.TabIndex = 0x11;
            this.statDraws.Text = "2258";
            this.statDraws.TextStyle = TextStyles.Bold;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel8.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0xb6, 0xae);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel8, null);
            this.gpgLabel8.TabIndex = 15;
            this.gpgLabel8.Text = "<LOC>Draws";
            this.gpgLabel8.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel8.TextStyle = TextStyles.Custom;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel9.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0x112, 0x88);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel9, null);
            this.gpgLabel9.TabIndex = 0x10;
            this.gpgLabel9.Text = "<LOC>Win %";
            this.gpgLabel9.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel9.TextStyle = TextStyles.Custom;
            this.statLosses.AutoGrowDirection = GrowDirections.None;
            this.statLosses.AutoSize = true;
            this.statLosses.AutoStyle = true;
            this.statLosses.BackColor = Color.Transparent;
            this.statLosses.Font = new Font("Arial", 9.75f);
            this.statLosses.ForeColor = Color.White;
            this.statLosses.IgnoreMouseWheel = false;
            this.statLosses.IsStyled = false;
            this.statLosses.Location = new Point(0x58, 0xc2);
            this.statLosses.Name = "statLosses";
            this.statLosses.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statLosses, null);
            this.statLosses.TabIndex = 14;
            this.statLosses.Text = "2258";
            this.statLosses.TextStyle = TextStyles.Bold;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel7.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x54, 0xae);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 13;
            this.gpgLabel7.Text = "<LOC>Losses";
            this.gpgLabel7.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel7.TextStyle = TextStyles.Custom;
            this.statWins.AutoGrowDirection = GrowDirections.None;
            this.statWins.AutoSize = true;
            this.statWins.AutoStyle = true;
            this.statWins.BackColor = Color.Transparent;
            this.statWins.Font = new Font("Arial", 9.75f);
            this.statWins.ForeColor = Color.White;
            this.statWins.IgnoreMouseWheel = false;
            this.statWins.IsStyled = false;
            this.statWins.Location = new Point(6, 0xc2);
            this.statWins.Name = "statWins";
            this.statWins.Size = new Size(15, 0x10);
            base.ttDefault.SetSuperTip(this.statWins, null);
            this.statWins.TabIndex = 12;
            this.statWins.Text = "1";
            this.statWins.TextStyle = TextStyles.Bold;
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel6.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(6, 0xae);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x22b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 11;
            this.gpgLabel6.Text = "<LOC>Wins";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgPanelAwards.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelAwards.AutoScroll = true;
            this.gpgPanelAwards.BackColor = Color.Transparent;
            this.gpgPanelAwards.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelAwards.BorderThickness = 2;
            this.gpgPanelAwards.DrawBorder = false;
            this.gpgPanelAwards.Location = new Point(6, 0xd9);
            this.gpgPanelAwards.Name = "gpgPanelAwards";
            this.gpgPanelAwards.Size = new Size(0x22b, 0xdf);
            base.ttDefault.SetSuperTip(this.gpgPanelAwards, null);
            this.gpgPanelAwards.TabIndex = 10;
            this.statRank.AutoGrowDirection = GrowDirections.None;
            this.statRank.AutoSize = true;
            this.statRank.AutoStyle = true;
            this.statRank.BackColor = Color.Transparent;
            this.statRank.Font = new Font("Arial", 9.75f);
            this.statRank.ForeColor = Color.White;
            this.statRank.IgnoreMouseWheel = false;
            this.statRank.IsStyled = false;
            this.statRank.Location = new Point(6, 0x9c);
            this.statRank.Name = "statRank";
            this.statRank.Size = new Size(15, 0x10);
            base.ttDefault.SetSuperTip(this.statRank, null);
            this.statRank.TabIndex = 4;
            this.statRank.Text = "1";
            this.statRank.TextStyle = TextStyles.Bold;
            this.statRating.AutoGrowDirection = GrowDirections.None;
            this.statRating.AutoSize = true;
            this.statRating.AutoStyle = true;
            this.statRating.BackColor = Color.Transparent;
            this.statRating.Font = new Font("Arial", 9.75f);
            this.statRating.ForeColor = Color.White;
            this.statRating.IgnoreMouseWheel = false;
            this.statRating.IsStyled = false;
            this.statRating.Location = new Point(0x58, 0x9c);
            this.statRating.Name = "statRating";
            this.statRating.Size = new Size(0x24, 0x10);
            base.ttDefault.SetSuperTip(this.statRating, null);
            this.statRating.TabIndex = 7;
            this.statRating.Text = "2258";
            this.statRating.TextStyle = TextStyles.Bold;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel3.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0xb6, 0x88);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x75, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 6;
            this.gpgLabel3.Text = "<LOC>Total Games";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.statTotalGames.AutoGrowDirection = GrowDirections.None;
            this.statTotalGames.AutoSize = true;
            this.statTotalGames.AutoStyle = true;
            this.statTotalGames.BackColor = Color.Transparent;
            this.statTotalGames.Font = new Font("Arial", 9.75f);
            this.statTotalGames.ForeColor = Color.White;
            this.statTotalGames.IgnoreMouseWheel = false;
            this.statTotalGames.IsStyled = false;
            this.statTotalGames.Location = new Point(0xb8, 0x9c);
            this.statTotalGames.Name = "statTotalGames";
            this.statTotalGames.Size = new Size(0x1d, 0x10);
            base.ttDefault.SetSuperTip(this.statTotalGames, null);
            this.statTotalGames.TabIndex = 8;
            this.statTotalGames.Text = "152";
            this.statTotalGames.TextStyle = TextStyles.Bold;
            this.skinLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(6, 0x71);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0x22b, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 1;
            this.skinLabel1.Text = "Vital Statistics";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(0);
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel2.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x54, 0x88);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 5;
            this.gpgLabel2.Text = "<LOC>Rating";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel1.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(6, 0x88);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x22b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 3;
            this.gpgLabel1.Text = "<LOC>Rank";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgPanelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanelDescription.AutoScroll = true;
            this.gpgPanelDescription.BackColor = Color.Transparent;
            this.gpgPanelDescription.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelDescription.BorderThickness = 2;
            this.gpgPanelDescription.Controls.Add(this.gpgLabelDescription);
            this.gpgPanelDescription.Controls.Add(this.gpgTextAreaDescription);
            this.gpgPanelDescription.DrawBorder = false;
            this.gpgPanelDescription.Location = new Point(6, 0x2d);
            this.gpgPanelDescription.Name = "gpgPanelDescription";
            this.gpgPanelDescription.Size = new Size(0x217, 60);
            base.ttDefault.SetSuperTip(this.gpgPanelDescription, null);
            this.gpgPanelDescription.TabIndex = 2;
            this.gpgLabelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(0, 0);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x203, 15);
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
            this.gpgTextAreaDescription.Size = new Size(0x217, 60);
            this.gpgTextAreaDescription.TabIndex = 1;
            this.gpgTextAreaDescription.Visible = false;
            this.skinButtonWebStats.Anchor = AnchorStyles.Bottom;
            this.skinButtonWebStats.AutoStyle = true;
            this.skinButtonWebStats.BackColor = Color.Transparent;
            this.skinButtonWebStats.ButtonState = 0;
            this.skinButtonWebStats.DialogResult = DialogResult.OK;
            this.skinButtonWebStats.DisabledForecolor = Color.Gray;
            this.skinButtonWebStats.DrawColor = Color.White;
            this.skinButtonWebStats.DrawEdges = true;
            this.skinButtonWebStats.FocusColor = Color.Yellow;
            this.skinButtonWebStats.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonWebStats.ForeColor = Color.White;
            this.skinButtonWebStats.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonWebStats.IsStyled = true;
            this.skinButtonWebStats.Location = new Point(0xe3, 0x20a);
            this.skinButtonWebStats.Name = "skinButtonWebStats";
            this.skinButtonWebStats.Size = new Size(0x7d, 0x1a);
            this.skinButtonWebStats.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonWebStats, null);
            this.skinButtonWebStats.TabIndex = 1;
            this.skinButtonWebStats.TabStop = true;
            this.skinButtonWebStats.Text = "<LOC>View Web Stats";
            this.skinButtonWebStats.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonWebStats.TextPadding = new Padding(0);
            this.skinButtonWebStats.Click += new EventHandler(this.skinButtonWebStats_Click);
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
            this.skinLabelHeader.SkinBasePath = @"Dialog\PlayerProfile\Header";
            base.ttDefault.SetSuperTip(this.skinLabelHeader, null);
            this.skinLabelHeader.TabIndex = 0;
            this.skinLabelHeader.Text = "<LOC>Player Profile";
            this.skinLabelHeader.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabelHeader.TextPadding = new Padding(8, 0, 0, 0);
            this.pnlUserList.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pnlUserList.AutoRefresh = false;
            this.pnlUserList.AutoScroll = true;
            this.pnlUserList.BackColor = Color.FromArgb(0x24, 0x23, 0x23);
            this.pnlUserList.Location = new Point(0, 0x15);
            this.pnlUserList.Name = "pnlUserList";
            this.pnlUserList.Size = new Size(0xfb, 0x217);
            this.pnlUserList.Style = UserListStyles.OnlineOffline;
            base.ttDefault.SetSuperTip(this.pnlUserList, null);
            this.pnlUserList.TabIndex = 11;
            this.skinLabelFriends.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabelFriends.AutoStyle = false;
            this.skinLabelFriends.BackColor = Color.Transparent;
            this.skinLabelFriends.Controls.Add(this.pictureBox1);
            this.skinLabelFriends.DrawEdges = true;
            this.skinLabelFriends.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelFriends.ForeColor = Color.White;
            this.skinLabelFriends.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelFriends.IsStyled = false;
            this.skinLabelFriends.Location = new Point(0, 0);
            this.skinLabelFriends.Name = "skinLabelFriends";
            this.skinLabelFriends.Size = new Size(250, 20);
            this.skinLabelFriends.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabelFriends, null);
            this.skinLabelFriends.TabIndex = 10;
            this.skinLabelFriends.Text = "<LOC>Friends";
            this.skinLabelFriends.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabelFriends.TextPadding = new Padding(0x10, 0, 0, 0);
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
            base.Name = "DlgPlayerProfileEx";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Player Profile";
            base.Controls.SetChildIndex(this.splitContainerMain, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.skinGroupPanelPlayer.ResumeLayout(false);
            this.skinGroupPanelPlayer.PerformLayout();
            ((ISupportInitialize) this.gpgPictureBoxAward1).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxAward2).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxAward3).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxAvatar).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxCancel).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxSave).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxEdit).EndInit();
            ((ISupportInitialize) this.pictureBoxClanRank).EndInit();
            this.gpgPanelDescription.ResumeLayout(false);
            this.gpgTextAreaDescription.Properties.EndInit();
            this.skinLabelFriends.ResumeLayout(false);
            this.skinLabelFriends.PerformLayout();
            ((ISupportInitialize) this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        internal void LayoutLifetimeAwards()
        {
            if ((this.LifetimeAwards != null) && !this.PerformingAwardsLayout)
            {
                this.PerformingAwardsLayout = true;
                ThreadPool.QueueUserWorkItem(delegate (object s) {
                    try
                    {
                        int num = 0;
                        int top = 0;
                        LifetimeAwardInfo[] array = new LifetimeAwardInfo[this.LifetimeAwards.Values.Count];
                        this.LifetimeAwards.Values.CopyTo(array, 0);
                        VGen0 method = null;
                        foreach (LifetimeAwardInfo awardInfo in array)
                        {
                            int num2 = 0;
                            int left = 0;
                            if (base.Disposing || base.IsDisposed)
                            {
                                return;
                            }
                            if (method == null)
                            {
                                method = delegate {
                                    awardInfo.CategoryLabel.Left = 0;
                                    awardInfo.CategoryLabel.Top = top;
                                };
                            }
                            base.Invoke(method);
                            top += awardInfo.CategoryLabel.Height + 6;
                            int lastImgBottom = 0;
                            using (IEnumerator<PictureBox> enumerator = awardInfo.Awards.Values.GetEnumerator())
                            {
                                VGen0 gen = null;
                                while (enumerator.MoveNext())
                                {
                                    PictureBox img = enumerator.Current;
                                    if (base.Disposing || base.IsDisposed)
                                    {
                                        return;
                                    }
                                    if (gen == null)
                                    {
                                        gen = delegate {
                                            img.Top = top;
                                            img.Left = left;
                                            if (img.Bottom > lastImgBottom)
                                            {
                                                lastImgBottom = img.Bottom;
                                            }
                                        };
                                    }
                                    base.Invoke(gen);
                                    if ((left + (img.Width + 6)) > this.gpgPanelAwards.Width)
                                    {
                                        top += img.Height + 6;
                                        left = 0;
                                    }
                                    else
                                    {
                                        left += img.Width + 6;
                                    }
                                    num2++;
                                }
                            }
                            top = lastImgBottom + 6;
                            num++;
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                    finally
                    {
                        this.PerformingAwardsLayout = false;
                    }
                });
            }
        }

        internal void LoadDisplayAwards()
        {
            this.gpgPictureBoxAward1.Image = null;
            base.ttDefault.SetToolTip(this.gpgPictureBoxAward1, "");
            this.gpgPictureBoxAward2.Image = null;
            base.ttDefault.SetToolTip(this.gpgPictureBoxAward2, "");
            this.gpgPictureBoxAward3.Image = null;
            base.ttDefault.SetToolTip(this.gpgPictureBoxAward3, "");
            if (!GameInformation.SelectedGame.IsSpaceSiege && !GameInformation.SelectedGame.IsChatOnly)
            {
                ThreadPool.QueueUserWorkItem(delegate (object s) {
                    PlayerDisplayAwards awards = new PlayerDisplayAwards(this.CurrentPlayer.Player);
                    Image img = null;
                    if (awards.Award1Specified)
                    {
                        img = awards.Award1.SmallImage;
                    }
                    else if (this.CurrentPlayer.Player.Equals(User.Current))
                    {
                        img = AwardsImages.award_empty;
                    }
                    base.Invoke((VGen0)delegate {
                        this.gpgPictureBoxAward1.Image = img;
                        if (awards.Award1Specified)
                        {
                            this.ttDefault.SetToolTip(this.gpgPictureBoxAward1, awards.Award1.AchievementDescription);
                        }
                        else if (this.CurrentPlayer.Player.Equals(User.Current))
                        {
                            this.ttDefault.SetToolTip(this.gpgPictureBoxAward1, Loc.Get("<LOC>Click here to set this display award."));
                        }
                        if (this.CurrentPlayer.Player.Equals(User.Current))
                        {
                            this.gpgPictureBoxAward1.Cursor = Cursors.Hand;
                            this.gpgPictureBoxAward1.Click -= new EventHandler(this.DisplayAwardClick);
                            this.gpgPictureBoxAward1.Click += new EventHandler(this.DisplayAwardClick);
                        }
                        else
                        {
                            this.gpgPictureBoxAward1.Cursor = Cursors.Default;
                            this.gpgPictureBoxAward1.Click -= new EventHandler(this.DisplayAwardClick);
                        }
                    });
                    img = null;
                    if (awards.Award2Specified)
                    {
                        img = awards.Award2.SmallImage;
                    }
                    else if (this.CurrentPlayer.Player.Equals(User.Current))
                    {
                        img = AwardsImages.award_empty;
                    }
                    base.Invoke((VGen0)delegate {
                        this.gpgPictureBoxAward2.Image = img;
                        if (awards.Award2Specified)
                        {
                            this.ttDefault.SetToolTip(this.gpgPictureBoxAward2, awards.Award2.AchievementDescription);
                        }
                        else if (this.CurrentPlayer.Player.Equals(User.Current))
                        {
                            this.ttDefault.SetToolTip(this.gpgPictureBoxAward2, Loc.Get("<LOC>Click here to set this display award."));
                        }
                        if (this.CurrentPlayer.Player.Equals(User.Current))
                        {
                            this.gpgPictureBoxAward2.Cursor = Cursors.Hand;
                            this.gpgPictureBoxAward2.Click -= new EventHandler(this.DisplayAwardClick);
                            this.gpgPictureBoxAward2.Click += new EventHandler(this.DisplayAwardClick);
                        }
                        else
                        {
                            this.gpgPictureBoxAward2.Cursor = Cursors.Default;
                            this.gpgPictureBoxAward2.Click -= new EventHandler(this.DisplayAwardClick);
                        }
                    });
                    img = null;
                    if (awards.Award3Specified)
                    {
                        img = awards.Award3.SmallImage;
                    }
                    else if (this.CurrentPlayer.Player.Equals(User.Current))
                    {
                        img = AwardsImages.award_empty;
                    }
                    base.Invoke((VGen0)delegate {
                        this.gpgPictureBoxAward3.Image = img;
                        if (awards.Award3Specified)
                        {
                            this.ttDefault.SetToolTip(this.gpgPictureBoxAward3, awards.Award3.AchievementDescription);
                        }
                        else if (this.CurrentPlayer.Player.Equals(User.Current))
                        {
                            this.ttDefault.SetToolTip(this.gpgPictureBoxAward3, Loc.Get("<LOC>Click here to set this display award."));
                        }
                        if (this.CurrentPlayer.Player.Equals(User.Current))
                        {
                            this.gpgPictureBoxAward3.Cursor = Cursors.Hand;
                            this.gpgPictureBoxAward3.Click -= new EventHandler(this.DisplayAwardClick);
                            this.gpgPictureBoxAward3.Click += new EventHandler(this.DisplayAwardClick);
                        }
                        else
                        {
                            this.gpgPictureBoxAward3.Cursor = Cursors.Default;
                            this.gpgPictureBoxAward3.Click -= new EventHandler(this.DisplayAwardClick);
                        }
                    });
                });
            }
        }

        internal void LoadLifetimeAwards()
        {
            this.gpgPanelAwards.Controls.Clear();
            this.mLifetimeAwards = new SortedList<int, LifetimeAwardInfo>();
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                try
                {
                    Dictionary<AwardCategory, Dictionary<AwardSet, Award>> dictionary = new Dictionary<AwardCategory, Dictionary<AwardSet, Award>>();
                    foreach (AwardSet set in AwardSet.AllAwardSets.Values)
                    {
                        if (!dictionary.ContainsKey(set.Category))
                        {
                            dictionary[set.Category] = new Dictionary<AwardSet, Award>();
                        }
                        dictionary[set.Category][set] = null;
                    }
                    foreach (PlayerAward award in PlayerAward.GetPlayerAwards(this.CurrentPlayer.Player.Name))
                    {
                        if (award.IsAchieved)
                        {
                            dictionary[award.Award.AwardSet.Category][award.Award.AwardSet] = award.Award;
                        }
                    }
                    using (Dictionary<AwardCategory, Dictionary<AwardSet, Award>>.Enumerator enumerator2 = dictionary.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            KeyValuePair<AwardCategory, Dictionary<AwardSet, Award>> category = enumerator2.Current;
                            VGen0 method = null;
                            bool flag = false;
                            LifetimeAwardInfo awardInfo = new LifetimeAwardInfo();
                            using (Dictionary<AwardSet, Award>.Enumerator enumerator3 = category.Value.GetEnumerator())
                            {
                                while (enumerator3.MoveNext())
                                {
                                    KeyValuePair<AwardSet, Award> award = enumerator3.Current;
                                    if ((award.Value != null) || award.Key.AlwaysShow)
                                    {
                                        Image awardImage;
                                        flag = true;
                                        if ((award.Value == null) && award.Key.AlwaysShow)
                                        {
                                            awardImage = award.Key.BaseImage;
                                        }
                                        else
                                        {
                                            awardImage = award.Value.LargeImage;
                                        }
                                        if (base.Disposing || base.IsDisposed)
                                        {
                                            return;
                                        }
                                        base.Invoke((VGen0)delegate {
                                            PictureBox box = new PictureBox {
                                                Size = new Size(0x30, 0x30),
                                                SizeMode = PictureBoxSizeMode.AutoSize,
                                                Image = awardImage,
                                                Tag = award
                                            };
                                            this.gpgPanelAwards.Controls.Add(box);
                                            box.MouseEnter += delegate (object s1, EventArgs e1) {
                                                int awardDegree;
                                                PictureBox curbox = s1 as PictureBox;
                                                KeyValuePair<AwardSet, Award> tag = (KeyValuePair<AwardSet, Award>) (s1 as Control).Tag;
                                                if (tag.Value == null)
                                                {
                                                    awardDegree = 0;
                                                }
                                                else
                                                {
                                                    awardDegree = tag.Value.AwardDegree;
                                                }
                                                if ((((DlgAwardSetDetails.Singleton == null) || !DlgAwardSetDetails.Singleton.IsHandleCreated) || DlgAwardSetDetails.Singleton.Disposing) || DlgAwardSetDetails.Singleton.IsDisposed)
                                                {
                                                    DlgAwardSetDetails.Singleton = new DlgAwardSetDetails();
                                                }
                                                DlgAwardSetDetails.Singleton.BindToAward(tag.Key, awardDegree);
                                                Point point = this.gpgPanelAwards.PointToScreen(curbox.Location);
                                                point.Offset(0, -(DlgAwardSetDetails.Singleton.Height + 4));
                                                DlgAwardSetDetails.Singleton.Location = point;
                                                DlgAwardSetDetails.Singleton.FadeIn();
                                            };
                                            box.MouseLeave += delegate (object s1, EventArgs e1) {
                                                if ((((DlgAwardSetDetails.Singleton == null) || !DlgAwardSetDetails.Singleton.IsHandleCreated) || DlgAwardSetDetails.Singleton.Disposing) || DlgAwardSetDetails.Singleton.IsDisposed)
                                                {
                                                    DlgAwardSetDetails.Singleton = new DlgAwardSetDetails();
                                                }
                                                DlgAwardSetDetails.Singleton.FadeOut();
                                            };
                                            awardInfo.Awards[award.Key.SortOrder] = box;
                                        });
                                    }
                                }
                            }
                            if (flag)
                            {
                                if (base.Disposing || base.IsDisposed)
                                {
                                    return;
                                }
                                if (method == null)
                                {
                                    method = delegate {
                                        SkinLabel label = new SkinLabel {
                                            Text = Loc.Get("<LOC>" + category.Key.Name),
                                            SkinBasePath = @"Controls\Background Label\BlueGradient",
                                            Width = this.gpgPanelAwards.Width,
                                            Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top
                                        };
                                        this.gpgPanelAwards.Controls.Add(label);
                                        awardInfo.CategoryLabel = label;
                                    };
                                }
                                base.Invoke(method);
                                this.LifetimeAwards[category.Key.SortOrder] = awardInfo;
                            }
                        }
                    }
                    this.LayoutLifetimeAwards();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        internal void LoadStats()
        {
            this.statDiconnectPct.Text = string.Format("{0}%", this.CurrentStats.DisconnectPercentage);
            this.statDisconnects.Text = this.CurrentStats.Disconnects.ToString();
            this.statDraws.Text = this.CurrentStats.Draws.ToString();
            this.statLosses.Text = this.CurrentStats.Losses.ToString();
            this.statRank.Text = this.CurrentStats.Rank.ToString();
            this.statRating.Text = this.CurrentStats.Rating.ToString();
            this.statTotalGames.Text = this.CurrentStats.TotalGames.ToString();
            this.statWinPct.Text = string.Format("{0}%", this.CurrentStats.WinPercentage);
            this.statWins.Text = this.CurrentStats.Wins.ToString();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if ((DlgAwardSetDetails.Singleton != null) && DlgAwardSetDetails.Singleton.IsHandleCreated)
            {
                try
                {
                    DlgAwardSetDetails.Singleton.Close();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
            base.OnClosing(e);
        }

        private void OnFirstPaint()
        {
            if (this.IsInitialized)
            {
                this.splitContainerMain.BackColor = Program.Settings.StylePreferences.HighlightColor3;
                this.splitContainerMain.Panel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
                this.IsFirstPaint = false;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.pnlUserList != null)
            {
                this.pnlUserList.RefreshData();
            }
            this.OnFirstPaint();
            this.LayoutLifetimeAwards();
        }

        public bool SetTargetPlayer(string playerName)
        {
            User record = null;
            if (this.PlayerViewLookups.ContainsKey(playerName))
            {
                this.mCurrentPlayer = this.PlayerViewLookups[playerName];
                return true;
            }
            if (!Chatroom.GatheringParticipants.TryFindByIndex("name", playerName, out record) && ((User.CurrentFriends == null) || !User.CurrentFriends.TryFindByIndex("name", playerName, out record)))
            {
                if (playerName == User.Current.Name)
                {
                    record = User.Current;
                }
                else
                {
                    MappedObjectList<User> objects = DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { playerName });
                    if (objects.Count > 0)
                    {
                        record = objects[0];
                    }
                }
            }
            if (record != null)
            {
                this.mCurrentPlayer = new PlayerView(record);
                this.PlayerViewLookups.Add(playerName, this.CurrentPlayer);
                return true;
            }
            base.MainForm.ErrorMessage("<LOC>Unable to locate {0}", new object[] { playerName });
            if (this.ViewList.Count < 1)
            {
                base.Dispose();
            }
            return false;
        }

        private void skinButtonNextProfile_Click(object sender, EventArgs e)
        {
            this.mCurrentPlayer = this.ViewList.Find(this.CurrentPlayer).Next.Value;
            this.Construct();
        }

        private void skinButtonPreviousProfile_Click(object sender, EventArgs e)
        {
            this.mCurrentPlayer = this.ViewList.Find(this.CurrentPlayer).Previous.Value;
            this.Construct();
        }

        private void skinButtonWebStats_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowWebStats(this.CurrentPlayer.Player.ID);
        }

        private void splitContainerMain_Paint(object sender, PaintEventArgs e)
        {
            if (this.IsFirstPaint)
            {
                this.OnFirstPaint();
            }
        }

        public PlayerView CurrentPlayer
        {
            get
            {
                return this.mCurrentPlayer;
            }
        }

        public PlayerRating CurrentStats
        {
            get
            {
                return this.mCurrentStats;
            }
        }

        internal SortedList<int, LifetimeAwardInfo> LifetimeAwards
        {
            get
            {
                return this.mLifetimeAwards;
            }
        }

        public LinkedList<PlayerView> ViewList
        {
            get
            {
                return this.mViewList;
            }
        }

        internal class LifetimeAwardInfo
        {
            internal SortedList<int, PictureBox> Awards = new SortedList<int, PictureBox>();
            internal SkinLabel CategoryLabel = null;
        }
    }
}

