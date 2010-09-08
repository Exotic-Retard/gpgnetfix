namespace GPG.Multiplayer.Client.Clans
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Controls.UserList;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgClanProfile : DlgBase
    {
        private SkinLabel backLabelRoster;
        private SkinLabel backLabelTitle;
        private MenuItem ciChat_DemoteClan;
        private MenuItem ciChat_IgnorePlayer;
        private MenuItem ciChat_InviteFriend;
        private MenuItem ciChat_PromoteClan;
        private MenuItem ciChat_RemoveClan;
        private MenuItem ciChat_RemoveFriend;
        private MenuItem ciChat_RequestClanInvite;
        private MenuItem ciChat_UnignorePlayer;
        private MenuItem ciChat_ViewPlayer;
        private MenuItem ciChat_WebStats;
        private MenuItem ciChat_WhisperPlayer;
        private Dictionary<string, ClanView> ClanNameLookup;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private GPGContextMenu gpgContextMenuChat;
        private GPGLabel gpgLabelDescription;
        private GPGLabel gpgLabelError;
        private GPGLabel gpgLabelFounded;
        private GPGPanel gpgPanelEdit;
        private GPGPictureBox gpgPictureBoxClanIcon;
        private GPGScrollPanel gpgScrollPanelGrid;
        private GPGScrollPanel gpgScrollPanelRoster;
        private GPGTextArea gpgTextAreaDescription;
        private ClanView mCurrentClan;
        private bool mDescriptionModified;
        private MenuItem menuItem10;
        private MenuItem menuItem8;
        private GridView mSelectedParticipantView;
        private LinkedList<ClanView> mViewList;
        private Dictionary<string, ClanView> PlayerNameLookup;
        private PnlUserList pnlUserListClan;
        private Dictionary<string, TextContainer> RankContainerLookup;
        private BoundContainerList RankContainers;
        private SkinButton skinButtonCancelEdit;
        private SkinButton skinButtonClose;
        private SkinButton skinButtonEdit;
        private SkinButton skinButtonLast;
        private SkinButton skinButtonNext;
        private SkinButton skinButtonSaveEdit;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainerRoster;

        public DlgClanProfile(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.ClanNameLookup = new Dictionary<string, ClanView>();
            this.PlayerNameLookup = new Dictionary<string, ClanView>();
            this.mCurrentClan = null;
            this.mViewList = new LinkedList<ClanView>();
            this.mSelectedParticipantView = null;
            this.RankContainerLookup = new Dictionary<string, TextContainer>();
            this.RankContainers = new BoundContainerList();
            this.mDescriptionModified = false;
            this.InitializeComponent();
        }

        private void CancelEdits()
        {
            this.gpgTextAreaDescription.Text = Clan.Current.Description;
            this.gpgLabelDescription.Visible = true;
            this.gpgTextAreaDescription.Visible = false;
            this.skinButtonEdit.Visible = true;
            this.skinButtonCancelEdit.Visible = false;
            this.skinButtonSaveEdit.Visible = false;
            this.gpgLabelError.Visible = false;
            this.mDescriptionModified = false;
        }

        private void ciChat_DemoteClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.DemoteClanMember(this.SelectedClanMember.Name);
                this.RefreshCurrentProfile();
            }
        }

        private void ciChat_IgnorePlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.IgnorePlayer(this.SelectedClanMember.Name);
            }
        }

        private void ciChat_InviteFriend_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.InviteAsFriend(this.SelectedClanMember.Name);
            }
        }

        private void ciChat_PromoteClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.PromoteClanMember(this.SelectedClanMember.Name);
                this.RefreshCurrentProfile();
            }
        }

        private void ciChat_RemoveClan_Click(object sender, EventArgs e)
        {
            if ((this.SelectedClanMember != null) && base.MainForm.RemoveClanMember(this.SelectedClanMember.Name))
            {
                this.RefreshCurrentProfile();
            }
        }

        private void ciChat_RemoveFriend_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.RemoveFriend(this.SelectedClanMember.Name, this.SelectedClanMember.ID);
            }
        }

        private void ciChat_RequestClanInvite_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.RequestClanInvite(this.SelectedClanMember.Name);
            }
        }

        private void ciChat_UnignorePlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.UnignorePlayer(this.SelectedClanMember.Name);
            }
        }

        private void ciChat_ViewPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.OnViewPlayerProfile(this.SelectedClanMember.Name);
            }
        }

        private void ciChat_WebStats_Click(object sender, EventArgs e)
        {
            if (this.SelectedClanMember != null)
            {
                base.MainForm.ShowWebStats(this.SelectedClanMember.ID);
            }
        }

        private void ciChat_WhisperPlayer_Click(object sender, EventArgs e)
        {
            if ((this.SelectedClanMember != null) && this.SelectedClanMember.Online)
            {
                base.MainForm.OnSendWhisper(this.SelectedClanMember.Name, null);
            }
        }

        internal void Construct()
        {
            if (!this.ViewList.Contains(this.CurrentClan))
            {
                this.ViewList.AddLast(this.CurrentClan);
            }
            this.skinButtonLast.Visible = !this.ViewList.First.Value.Equals(this.CurrentClan);
            this.skinButtonNext.Visible = !this.ViewList.Last.Value.Equals(this.CurrentClan);
            this.gpgLabelError.ForeColor = Program.Settings.Chat.Appearance.ErrorColor;
            Clan clan = this.CurrentClan.Clan;
            MappedObjectList<ClanMember> members = this.CurrentClan.Members;
            this.pnlUserListClan.ClanMembers = members;
            this.RankContainerLookup = new Dictionary<string, TextContainer>();
            this.RankContainers = new BoundContainerList();
            this.backLabelTitle.Text = clan.Name;
            this.backLabelTitle.ForeColor = Program.Settings.Chat.Appearance.ClanColor;
            this.gpgLabelFounded.Text = string.Format(Loc.Get("<LOC>Founded {0}"), clan.CreateDate.ToLongDateString());
            this.gpgLabelFounded.ForeColor = Color.Yellow;
            this.Text = string.Format(Loc.Get("<LOC>Clan Profile: {0}"), clan);
            this.gpgLabelDescription.Visible = false;
            this.gpgLabelDescription.Text = clan.Description;
            this.gpgLabelDescription.Visible = true;
            this.gpgTextAreaDescription.Text = clan.Description;
            this.gpgTextAreaDescription.Visible = false;
            using (Graphics graphics = base.CreateGraphics())
            {
                if ((this.gpgLabelDescription.Text != null) && (this.gpgLabelDescription.Text.Length > 0))
                {
                    this.gpgLabelDescription.Height = Convert.ToInt32(DrawUtil.MeasureString(graphics, this.gpgLabelDescription.Text, this.gpgLabelDescription.Font, (float) this.gpgLabelDescription.Width).Height) + 6;
                }
                else
                {
                    this.gpgLabelDescription.Height = 0x23;
                }
            }
            if ((((ClanMember.Current != null) && (Clan.Current != null)) && Clan.Current.Equals(clan)) && ClanMember.Current.HasAbility(ClanAbility.ChangeProfile))
            {
                this.gpgPanelEdit.Visible = true;
                this.skinButtonEdit.Visible = true;
                this.skinButtonEdit.Enabled = true;
            }
            else
            {
                this.gpgPanelEdit.Visible = false;
                this.skinButtonEdit.Visible = false;
                this.skinButtonEdit.Enabled = false;
            }
            if (this.CurrentClan.Clan.Image != null)
            {
                this.gpgPictureBoxClanIcon.Visible = true;
                this.gpgPictureBoxClanIcon.Image = this.CurrentClan.Clan.Image;
            }
            else
            {
                this.gpgPictureBoxClanIcon.Visible = false;
                this.gpgPictureBoxClanIcon.Image = null;
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

        private void Error(string error)
        {
            this.gpgLabelError.Text = Loc.Get(error);
            this.gpgLabelError.Visible = true;
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            int num = 0x17;
            this.gpgLabelDescription.Width = this.flowLayoutPanel1.Width - num;
            using (Graphics graphics = base.CreateGraphics())
            {
                this.gpgLabelDescription.Height = Convert.ToInt32(DrawUtil.MeasureString(graphics, this.gpgLabelDescription.Text, this.gpgLabelDescription.Font, (float) this.gpgLabelDescription.Width).Height) + 6;
            }
            this.backLabelTitle.Width = this.flowLayoutPanel1.Width - num;
            this.gpgTextAreaDescription.Width = this.flowLayoutPanel1.Width - num;
            this.gpgPanelEdit.Width = this.flowLayoutPanel1.Width - num;
            this.Refresh();
        }

        private void gpgLabelDescription_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            base.MainForm.ShowWebPage(e.LinkText);
        }

        private void gpgTextAreaDescription_KeyDown(object sender, KeyEventArgs e)
        {
            this.mDescriptionModified = true;
            if ((e.KeyCode == Keys.S) && !((!e.Control || e.Alt) || e.Shift))
            {
                this.SaveEdits();
            }
        }

        private void gvParticipantMembers_DoubleClick(object sender, EventArgs e)
        {
            if ((this.SelectedClanMember != null) && this.SelectedClanMember.Online)
            {
                base.MainForm.OnSendWhisper(this.SelectedClanMember.Name, null);
            }
        }

        private void InitializeComponent()
        {
            this.splitContainer1 = new SplitContainer();
            this.splitContainer2 = new SplitContainer();
            this.gpgPictureBoxClanIcon = new GPGPictureBox();
            this.backLabelTitle = new SkinLabel();
            this.gpgLabelFounded = new GPGLabel();
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.gpgPanelEdit = new GPGPanel();
            this.skinButtonSaveEdit = new SkinButton();
            this.gpgLabelError = new GPGLabel();
            this.skinButtonEdit = new SkinButton();
            this.skinButtonCancelEdit = new SkinButton();
            this.gpgTextAreaDescription = new GPGTextArea();
            this.gpgLabelDescription = new GPGLabel();
            this.gpgScrollPanelGrid = new GPGScrollPanel();
            this.splitContainerRoster = new SplitContainer();
            this.backLabelRoster = new SkinLabel();
            this.gpgScrollPanelRoster = new GPGScrollPanel();
            this.pnlUserListClan = new PnlUserList();
            this.gpgContextMenuChat = new GPGContextMenu();
            this.ciChat_WhisperPlayer = new MenuItem();
            this.ciChat_WebStats = new MenuItem();
            this.ciChat_ViewPlayer = new MenuItem();
            this.ciChat_IgnorePlayer = new MenuItem();
            this.ciChat_UnignorePlayer = new MenuItem();
            this.menuItem10 = new MenuItem();
            this.ciChat_InviteFriend = new MenuItem();
            this.ciChat_RemoveFriend = new MenuItem();
            this.menuItem8 = new MenuItem();
            this.ciChat_RequestClanInvite = new MenuItem();
            this.ciChat_PromoteClan = new MenuItem();
            this.ciChat_DemoteClan = new MenuItem();
            this.ciChat_RemoveClan = new MenuItem();
            this.skinButtonLast = new SkinButton();
            this.skinButtonNext = new SkinButton();
            this.skinButtonClose = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((ISupportInitialize) this.gpgPictureBoxClanIcon).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.gpgPanelEdit.SuspendLayout();
            this.gpgTextAreaDescription.Properties.BeginInit();
            this.gpgScrollPanelGrid.SuspendLayout();
            this.splitContainerRoster.Panel1.SuspendLayout();
            this.splitContainerRoster.Panel2.SuspendLayout();
            this.splitContainerRoster.SuspendLayout();
            this.gpgScrollPanelRoster.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x27b, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.splitContainer1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainer1.FixedPanel = FixedPanel.Panel2;
            this.splitContainer1.Location = new Point(12, 0x4b);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel1, null);
            this.splitContainer1.Panel2.Controls.Add(this.gpgScrollPanelGrid);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel2, null);
            this.splitContainer1.Size = new Size(0x29f, 0x18c);
            this.splitContainer1.SplitterDistance = 460;
            base.ttDefault.SetSuperTip(this.splitContainer1, null);
            this.splitContainer1.TabIndex = 4;
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.FixedPanel = FixedPanel.Panel1;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = Orientation.Horizontal;
            this.splitContainer2.Panel1.Controls.Add(this.gpgPictureBoxClanIcon);
            this.splitContainer2.Panel1.Controls.Add(this.backLabelTitle);
            this.splitContainer2.Panel1.Controls.Add(this.gpgLabelFounded);
            base.ttDefault.SetSuperTip(this.splitContainer2.Panel1, null);
            this.splitContainer2.Panel2.AutoScroll = true;
            this.splitContainer2.Panel2.Controls.Add(this.flowLayoutPanel1);
            base.ttDefault.SetSuperTip(this.splitContainer2.Panel2, null);
            this.splitContainer2.Size = new Size(460, 0x18c);
            this.splitContainer2.SplitterDistance = 0x39;
            this.splitContainer2.SplitterWidth = 1;
            base.ttDefault.SetSuperTip(this.splitContainer2, null);
            this.splitContainer2.TabIndex = 0;
            this.gpgPictureBoxClanIcon.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxClanIcon.Location = new Point(0x193, 3);
            this.gpgPictureBoxClanIcon.Name = "gpgPictureBoxClanIcon";
            this.gpgPictureBoxClanIcon.Size = new Size(40, 20);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxClanIcon, null);
            this.gpgPictureBoxClanIcon.TabIndex = 6;
            this.gpgPictureBoxClanIcon.TabStop = false;
            this.gpgPictureBoxClanIcon.Visible = false;
            this.backLabelTitle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.backLabelTitle.AutoStyle = false;
            this.backLabelTitle.BackColor = Color.Transparent;
            this.backLabelTitle.DrawEdges = true;
            this.backLabelTitle.Font = new Font("Arial", 14f, FontStyle.Bold);
            this.backLabelTitle.ForeColor = Color.White;
            this.backLabelTitle.HorizontalScalingMode = ScalingModes.Tile;
            this.backLabelTitle.IsStyled = false;
            this.backLabelTitle.Location = new Point(0, 1);
            this.backLabelTitle.Margin = new Padding(0);
            this.backLabelTitle.Name = "backLabelTitle";
            this.backLabelTitle.Size = new Size(460, 0x18);
            this.backLabelTitle.SkinBasePath = @"Controls\Background Label\Rectangle";
            base.ttDefault.SetSuperTip(this.backLabelTitle, null);
            this.backLabelTitle.TabIndex = 5;
            this.backLabelTitle.Text = "clan name";
            this.backLabelTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.backLabelTitle.TextPadding = new Padding(0);
            this.gpgLabelFounded.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelFounded.AutoSize = true;
            this.gpgLabelFounded.AutoStyle = true;
            this.gpgLabelFounded.Font = new Font("Arial", 9.75f);
            this.gpgLabelFounded.ForeColor = Color.Yellow;
            this.gpgLabelFounded.IgnoreMouseWheel = false;
            this.gpgLabelFounded.IsStyled = false;
            this.gpgLabelFounded.Location = new Point(0, 0x1a);
            this.gpgLabelFounded.Margin = new Padding(0);
            this.gpgLabelFounded.Name = "gpgLabelFounded";
            this.gpgLabelFounded.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelFounded, null);
            this.gpgLabelFounded.TabIndex = 1;
            this.gpgLabelFounded.Text = "gpgLabel1";
            this.gpgLabelFounded.TextStyle = TextStyles.Status;
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.gpgPanelEdit);
            this.flowLayoutPanel1.Controls.Add(this.gpgTextAreaDescription);
            this.flowLayoutPanel1.Controls.Add(this.gpgLabelDescription);
            this.flowLayoutPanel1.Dock = DockStyle.Fill;
            this.flowLayoutPanel1.Location = new Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(460, 0x152);
            base.ttDefault.SetSuperTip(this.flowLayoutPanel1, null);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.Resize += new EventHandler(this.flowLayoutPanel1_Resize);
            this.gpgPanelEdit.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelEdit.BorderThickness = 2;
            this.gpgPanelEdit.Controls.Add(this.skinButtonSaveEdit);
            this.gpgPanelEdit.Controls.Add(this.gpgLabelError);
            this.gpgPanelEdit.Controls.Add(this.skinButtonEdit);
            this.gpgPanelEdit.Controls.Add(this.skinButtonCancelEdit);
            this.gpgPanelEdit.DrawBorder = false;
            this.gpgPanelEdit.Location = new Point(3, 3);
            this.gpgPanelEdit.Name = "gpgPanelEdit";
            this.gpgPanelEdit.Size = new Size(0x166, 0x38);
            base.ttDefault.SetSuperTip(this.gpgPanelEdit, null);
            this.gpgPanelEdit.TabIndex = 2;
            this.skinButtonSaveEdit.AutoStyle = true;
            this.skinButtonSaveEdit.BackColor = Color.Black;
            this.skinButtonSaveEdit.ButtonState = 0;
            this.skinButtonSaveEdit.DialogResult = DialogResult.OK;
            this.skinButtonSaveEdit.DisabledForecolor = Color.Gray;
            this.skinButtonSaveEdit.DrawColor = Color.White;
            this.skinButtonSaveEdit.DrawEdges = true;
            this.skinButtonSaveEdit.FocusColor = Color.Yellow;
            this.skinButtonSaveEdit.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSaveEdit.ForeColor = Color.White;
            this.skinButtonSaveEdit.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSaveEdit.IsStyled = true;
            this.skinButtonSaveEdit.Location = new Point(0x57, 10);
            this.skinButtonSaveEdit.Name = "skinButtonSaveEdit";
            this.skinButtonSaveEdit.Size = new Size(0x51, 20);
            this.skinButtonSaveEdit.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSaveEdit, null);
            this.skinButtonSaveEdit.TabIndex = 1;
            this.skinButtonSaveEdit.TabStop = true;
            this.skinButtonSaveEdit.Text = "<LOC>Save";
            this.skinButtonSaveEdit.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSaveEdit.TextPadding = new Padding(0);
            this.skinButtonSaveEdit.Visible = false;
            this.skinButtonSaveEdit.Click += new EventHandler(this.skinButtonSaveEdit_Click);
            this.gpgLabelError.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelError.AutoSize = true;
            this.gpgLabelError.AutoStyle = true;
            this.gpgLabelError.Font = new Font("Arial", 9.75f);
            this.gpgLabelError.ForeColor = Color.Red;
            this.gpgLabelError.IgnoreMouseWheel = false;
            this.gpgLabelError.IsStyled = false;
            this.gpgLabelError.Location = new Point(0, 0x21);
            this.gpgLabelError.Name = "gpgLabelError";
            this.gpgLabelError.Size = new Size(0x58, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelError, null);
            this.gpgLabelError.TabIndex = 3;
            this.gpgLabelError.Text = "gpgLabelError";
            this.gpgLabelError.TextStyle = TextStyles.Error;
            this.gpgLabelError.Visible = false;
            this.skinButtonEdit.AutoStyle = true;
            this.skinButtonEdit.BackColor = Color.Black;
            this.skinButtonEdit.ButtonState = 0;
            this.skinButtonEdit.DialogResult = DialogResult.OK;
            this.skinButtonEdit.DisabledForecolor = Color.Gray;
            this.skinButtonEdit.DrawColor = Color.White;
            this.skinButtonEdit.DrawEdges = true;
            this.skinButtonEdit.FocusColor = Color.Yellow;
            this.skinButtonEdit.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonEdit.ForeColor = Color.White;
            this.skinButtonEdit.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonEdit.IsStyled = true;
            this.skinButtonEdit.Location = new Point(0, 10);
            this.skinButtonEdit.Name = "skinButtonEdit";
            this.skinButtonEdit.Size = new Size(0x51, 20);
            this.skinButtonEdit.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonEdit, null);
            this.skinButtonEdit.TabIndex = 0;
            this.skinButtonEdit.TabStop = true;
            this.skinButtonEdit.Text = "<LOC>Edit";
            this.skinButtonEdit.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonEdit.TextPadding = new Padding(0);
            this.skinButtonEdit.Click += new EventHandler(this.skinButtonEdit_Click);
            this.skinButtonCancelEdit.AutoStyle = true;
            this.skinButtonCancelEdit.BackColor = Color.Transparent;
            this.skinButtonCancelEdit.ButtonState = 0;
            this.skinButtonCancelEdit.DialogResult = DialogResult.OK;
            this.skinButtonCancelEdit.DisabledForecolor = Color.Gray;
            this.skinButtonCancelEdit.DrawColor = Color.Empty;
            this.skinButtonCancelEdit.DrawEdges = true;
            this.skinButtonCancelEdit.FocusColor = Color.Yellow;
            this.skinButtonCancelEdit.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancelEdit.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancelEdit.IsStyled = false;
            this.skinButtonCancelEdit.Location = new Point(0, 10);
            this.skinButtonCancelEdit.Name = "skinButtonCancelEdit";
            this.skinButtonCancelEdit.Size = new Size(0x51, 20);
            this.skinButtonCancelEdit.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancelEdit, null);
            this.skinButtonCancelEdit.TabIndex = 2;
            this.skinButtonCancelEdit.TabStop = true;
            this.skinButtonCancelEdit.Text = "<LOC>Cancel";
            this.skinButtonCancelEdit.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancelEdit.TextPadding = new Padding(0);
            this.skinButtonCancelEdit.Visible = false;
            this.skinButtonCancelEdit.Click += new EventHandler(this.skinButtonCancelEdit_Click);
            this.gpgTextAreaDescription.BorderColor = Color.White;
            this.gpgTextAreaDescription.Location = new Point(3, 0x41);
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
            this.gpgTextAreaDescription.Size = new Size(0x166, 0x9c);
            this.gpgTextAreaDescription.TabIndex = 0;
            this.gpgTextAreaDescription.KeyDown += new KeyEventHandler(this.gpgTextAreaDescription_KeyDown);
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.BackColor = Color.Black;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(3, 0xe0);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x166, 0x9c);
            base.ttDefault.SetSuperTip(this.gpgLabelDescription, null);
            this.gpgLabelDescription.TabIndex = 3;
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            this.gpgScrollPanelGrid.AutoScroll = true;
            this.gpgScrollPanelGrid.BackColor = Color.Black;
            this.gpgScrollPanelGrid.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgScrollPanelGrid.BorderThickness = 2;
            this.gpgScrollPanelGrid.ChildControl = null;
            this.gpgScrollPanelGrid.Controls.Add(this.splitContainerRoster);
            this.gpgScrollPanelGrid.Dock = DockStyle.Fill;
            this.gpgScrollPanelGrid.DrawBorder = false;
            this.gpgScrollPanelGrid.Location = new Point(0, 0);
            this.gpgScrollPanelGrid.Name = "gpgScrollPanelGrid";
            this.gpgScrollPanelGrid.Padding = new Padding(4, 0, 0, 0);
            this.gpgScrollPanelGrid.Size = new Size(0xcf, 0x18c);
            base.ttDefault.SetSuperTip(this.gpgScrollPanelGrid, null);
            this.gpgScrollPanelGrid.TabIndex = 0;
            this.splitContainerRoster.BackColor = Color.FromArgb(0x24, 0x23, 0x23);
            this.splitContainerRoster.Dock = DockStyle.Fill;
            this.splitContainerRoster.Location = new Point(4, 0);
            this.splitContainerRoster.Margin = new Padding(0);
            this.splitContainerRoster.Name = "splitContainerRoster";
            this.splitContainerRoster.Orientation = Orientation.Horizontal;
            this.splitContainerRoster.Panel1.Controls.Add(this.backLabelRoster);
            base.ttDefault.SetSuperTip(this.splitContainerRoster.Panel1, null);
            this.splitContainerRoster.Panel2.Controls.Add(this.gpgScrollPanelRoster);
            base.ttDefault.SetSuperTip(this.splitContainerRoster.Panel2, null);
            this.splitContainerRoster.Size = new Size(0xcb, 0x18c);
            this.splitContainerRoster.SplitterDistance = 0x22;
            this.splitContainerRoster.SplitterWidth = 1;
            base.ttDefault.SetSuperTip(this.splitContainerRoster, null);
            this.splitContainerRoster.TabIndex = 5;
            this.backLabelRoster.AutoStyle = false;
            this.backLabelRoster.BackColor = Color.Transparent;
            this.backLabelRoster.Dock = DockStyle.Top;
            this.backLabelRoster.DrawEdges = true;
            this.backLabelRoster.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.backLabelRoster.ForeColor = Color.White;
            this.backLabelRoster.HorizontalScalingMode = ScalingModes.Tile;
            this.backLabelRoster.IsStyled = false;
            this.backLabelRoster.Location = new Point(0, 0);
            this.backLabelRoster.Margin = new Padding(0);
            this.backLabelRoster.Name = "backLabelRoster";
            this.backLabelRoster.Size = new Size(0xcb, 20);
            this.backLabelRoster.SkinBasePath = @"Controls\Background Label\Round Top";
            base.ttDefault.SetSuperTip(this.backLabelRoster, null);
            this.backLabelRoster.TabIndex = 0;
            this.backLabelRoster.Text = "<LOC>Roster";
            this.backLabelRoster.TextAlign = ContentAlignment.BottomCenter;
            this.backLabelRoster.TextPadding = new Padding(0);
            this.gpgScrollPanelRoster.AutoScroll = true;
            this.gpgScrollPanelRoster.BackColor = Color.Black;
            this.gpgScrollPanelRoster.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgScrollPanelRoster.BorderThickness = 2;
            this.gpgScrollPanelRoster.ChildControl = null;
            this.gpgScrollPanelRoster.Controls.Add(this.pnlUserListClan);
            this.gpgScrollPanelRoster.Dock = DockStyle.Fill;
            this.gpgScrollPanelRoster.DrawBorder = false;
            this.gpgScrollPanelRoster.Location = new Point(0, 0);
            this.gpgScrollPanelRoster.Name = "gpgScrollPanelRoster";
            this.gpgScrollPanelRoster.Size = new Size(0xcb, 0x169);
            base.ttDefault.SetSuperTip(this.gpgScrollPanelRoster, null);
            this.gpgScrollPanelRoster.TabIndex = 6;
            this.pnlUserListClan.AutoRefresh = true;
            this.pnlUserListClan.AutoScroll = true;
            this.pnlUserListClan.BackColor = Color.FromArgb(0x24, 0x23, 0x23);
            this.pnlUserListClan.Dock = DockStyle.Fill;
            this.pnlUserListClan.Location = new Point(0, 0);
            this.pnlUserListClan.Name = "pnlUserListClan";
            this.pnlUserListClan.Size = new Size(0xcb, 0x169);
            this.pnlUserListClan.Style = UserListStyles.Clan;
            base.ttDefault.SetSuperTip(this.pnlUserListClan, null);
            this.pnlUserListClan.TabIndex = 0;
            this.gpgContextMenuChat.MenuItems.AddRange(new MenuItem[] { this.ciChat_WhisperPlayer, this.ciChat_WebStats, this.ciChat_ViewPlayer, this.ciChat_IgnorePlayer, this.ciChat_UnignorePlayer, this.menuItem10, this.ciChat_InviteFriend, this.ciChat_RemoveFriend, this.menuItem8, this.ciChat_RequestClanInvite, this.ciChat_PromoteClan, this.ciChat_DemoteClan, this.ciChat_RemoveClan });
            this.ciChat_WhisperPlayer.Index = 0;
            this.ciChat_WhisperPlayer.Text = "<LOC>Send private message";
            this.ciChat_WhisperPlayer.Click += new EventHandler(this.ciChat_WhisperPlayer_Click);
            this.ciChat_WebStats.Index = 1;
            this.ciChat_WebStats.Text = "<LOC>View web statistics";
            this.ciChat_WebStats.Click += new EventHandler(this.ciChat_WebStats_Click);
            this.ciChat_ViewPlayer.Index = 2;
            this.ciChat_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChat_ViewPlayer.Click += new EventHandler(this.ciChat_ViewPlayer_Click);
            this.ciChat_IgnorePlayer.Index = 3;
            this.ciChat_IgnorePlayer.Text = "<LOC>Ignore player";
            this.ciChat_IgnorePlayer.Click += new EventHandler(this.ciChat_IgnorePlayer_Click);
            this.ciChat_UnignorePlayer.Index = 4;
            this.ciChat_UnignorePlayer.Text = "<LOC>Unignore player";
            this.ciChat_UnignorePlayer.Click += new EventHandler(this.ciChat_UnignorePlayer_Click);
            this.menuItem10.Index = 5;
            this.menuItem10.Text = "-";
            this.ciChat_InviteFriend.Index = 6;
            this.ciChat_InviteFriend.Text = "<LOC>Invite player to join Friends list";
            this.ciChat_InviteFriend.Click += new EventHandler(this.ciChat_InviteFriend_Click);
            this.ciChat_RemoveFriend.Index = 7;
            this.ciChat_RemoveFriend.Text = "<LOC>Remove player from Friends list";
            this.ciChat_RemoveFriend.Click += new EventHandler(this.ciChat_RemoveFriend_Click);
            this.menuItem8.Index = 8;
            this.menuItem8.Text = "-";
            this.ciChat_RequestClanInvite.Index = 9;
            this.ciChat_RequestClanInvite.Text = "<LOC>Request to join this player's clan";
            this.ciChat_RequestClanInvite.Click += new EventHandler(this.ciChat_RequestClanInvite_Click);
            this.ciChat_PromoteClan.Index = 10;
            this.ciChat_PromoteClan.Text = "<LOC>Promote";
            this.ciChat_PromoteClan.Click += new EventHandler(this.ciChat_PromoteClan_Click);
            this.ciChat_DemoteClan.Index = 11;
            this.ciChat_DemoteClan.Text = "<LOC>Demote";
            this.ciChat_DemoteClan.Click += new EventHandler(this.ciChat_DemoteClan_Click);
            this.ciChat_RemoveClan.Index = 12;
            this.ciChat_RemoveClan.Text = "<LOC>Remove from clan";
            this.ciChat_RemoveClan.Click += new EventHandler(this.ciChat_RemoveClan_Click);
            this.skinButtonLast.Anchor = AnchorStyles.Bottom;
            this.skinButtonLast.AutoStyle = true;
            this.skinButtonLast.BackColor = Color.Black;
            this.skinButtonLast.ButtonState = 0;
            this.skinButtonLast.DialogResult = DialogResult.OK;
            this.skinButtonLast.DisabledForecolor = Color.Gray;
            this.skinButtonLast.DrawColor = Color.White;
            this.skinButtonLast.DrawEdges = true;
            this.skinButtonLast.FocusColor = Color.Yellow;
            this.skinButtonLast.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLast.ForeColor = Color.White;
            this.skinButtonLast.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLast.IsStyled = true;
            this.skinButtonLast.Location = new Point(0xac, 0x1e5);
            this.skinButtonLast.Name = "skinButtonLast";
            this.skinButtonLast.Size = new Size(0x68, 0x1a);
            this.skinButtonLast.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonLast, null);
            this.skinButtonLast.TabIndex = 14;
            this.skinButtonLast.TabStop = true;
            this.skinButtonLast.Text = "<LOC>Last";
            this.skinButtonLast.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLast.TextPadding = new Padding(0);
            this.skinButtonLast.Click += new EventHandler(this.skinButtonLast_Click);
            this.skinButtonNext.Anchor = AnchorStyles.Bottom;
            this.skinButtonNext.AutoStyle = true;
            this.skinButtonNext.BackColor = Color.Black;
            this.skinButtonNext.ButtonState = 0;
            this.skinButtonNext.DialogResult = DialogResult.OK;
            this.skinButtonNext.DisabledForecolor = Color.Gray;
            this.skinButtonNext.DrawColor = Color.White;
            this.skinButtonNext.DrawEdges = true;
            this.skinButtonNext.FocusColor = Color.Yellow;
            this.skinButtonNext.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNext.ForeColor = Color.White;
            this.skinButtonNext.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNext.IsStyled = true;
            this.skinButtonNext.Location = new Point(0x1a2, 0x1e5);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(0x68, 0x1a);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonNext, null);
            this.skinButtonNext.TabIndex = 13;
            this.skinButtonNext.TabStop = true;
            this.skinButtonNext.Text = "<LOC>Next";
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.skinButtonClose.Anchor = AnchorStyles.Bottom;
            this.skinButtonClose.AutoStyle = true;
            this.skinButtonClose.BackColor = Color.Black;
            this.skinButtonClose.ButtonState = 0;
            this.skinButtonClose.DialogResult = DialogResult.OK;
            this.skinButtonClose.DisabledForecolor = Color.Gray;
            this.skinButtonClose.DrawColor = Color.White;
            this.skinButtonClose.DrawEdges = true;
            this.skinButtonClose.FocusColor = Color.Yellow;
            this.skinButtonClose.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClose.ForeColor = Color.White;
            this.skinButtonClose.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClose.IsStyled = true;
            this.skinButtonClose.Location = new Point(0x128, 0x1e5);
            this.skinButtonClose.Name = "skinButtonClose";
            this.skinButtonClose.Size = new Size(0x68, 0x1a);
            this.skinButtonClose.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClose, null);
            this.skinButtonClose.TabIndex = 12;
            this.skinButtonClose.TabStop = true;
            this.skinButtonClose.Text = "<LOC>Close";
            this.skinButtonClose.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClose.TextPadding = new Padding(0);
            this.skinButtonClose.Click += new EventHandler(this.skinButtonClose_Click);
            base.AcceptButton = this.skinButtonClose;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonClose;
            base.ClientSize = new Size(0x2b6, 0x22d);
            base.Controls.Add(this.skinButtonLast);
            base.Controls.Add(this.skinButtonNext);
            base.Controls.Add(this.skinButtonClose);
            base.Controls.Add(this.splitContainer1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x178, 0x178);
            base.Name = "DlgClanProfile";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "DlgClanProfile";
            base.Controls.SetChildIndex(this.splitContainer1, 0);
            base.Controls.SetChildIndex(this.skinButtonClose, 0);
            base.Controls.SetChildIndex(this.skinButtonNext, 0);
            base.Controls.SetChildIndex(this.skinButtonLast, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            ((ISupportInitialize) this.gpgPictureBoxClanIcon).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.gpgPanelEdit.ResumeLayout(false);
            this.gpgPanelEdit.PerformLayout();
            this.gpgTextAreaDescription.Properties.EndInit();
            this.gpgScrollPanelGrid.ResumeLayout(false);
            this.splitContainerRoster.Panel1.ResumeLayout(false);
            this.splitContainerRoster.Panel2.ResumeLayout(false);
            this.splitContainerRoster.ResumeLayout(false);
            this.gpgScrollPanelRoster.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void Localize()
        {
            base.Localize();
            this.gpgContextMenuChat.Localize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if ((this.DescriptionModified && (new DlgYesNo(base.MainForm, "<LOC>Save Changes", "<LOC>Do you want to save the changes to your profile?").ShowDialog(this) == DialogResult.Yes)) && !this.SaveEdits())
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if ((this.backLabelTitle != null) && (this.backLabelRoster != null))
            {
                this.backLabelTitle.Refresh();
                this.backLabelRoster.Refresh();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.gpgLabelDescription.Text = this.gpgLabelDescription.Text;
        }

        private void RefreshCurrentProfile()
        {
            if (this.CurrentClan.PlayerName != null)
            {
                this.PlayerNameLookup.Remove(this.CurrentClan.PlayerName);
            }
            else
            {
                this.ClanNameLookup.Remove(this.CurrentClan.ClanName);
            }
            LinkedListNode<ClanView> previous = this.ViewList.Find(this.CurrentClan).Previous;
            this.ViewList.Remove(this.CurrentClan);
            if (this.CurrentClan.PlayerName != null)
            {
                this.SetCurrentClanByPlayer(this.CurrentClan.PlayerName);
            }
            else
            {
                this.SetCurrentClanByName(this.CurrentClan.ClanName);
            }
            if (previous == null)
            {
                this.ViewList.AddFirst(this.CurrentClan);
            }
            else
            {
                this.ViewList.AddAfter(previous, this.CurrentClan);
            }
            this.Construct();
        }

        private bool SaveEdits()
        {
            if (Profanity.ContainsProfanity(this.gpgTextAreaDescription.Text))
            {
                this.Error("<LOC>Your clan profile may not contain profanity.\r\nPlease enter a valid profile description.");
                return false;
            }
            Clan.Current.Description = this.gpgTextAreaDescription.Text;
            this.gpgLabelDescription.Text = Clan.Current.Description;
            using (Graphics graphics = base.CreateGraphics())
            {
                if ((this.gpgLabelDescription.Text != null) && (this.gpgLabelDescription.Text.Length > 0))
                {
                    this.gpgLabelDescription.Height = Convert.ToInt32(DrawUtil.MeasureString(graphics, this.gpgLabelDescription.Text, this.gpgLabelDescription.Font, (float) this.gpgLabelDescription.Width).Height) + 6;
                }
                else
                {
                    this.gpgLabelDescription.Height = 0x23;
                }
            }
            DataAccess.ExecuteQuery("ClanDescription", new object[] { Clan.Current.Description, Clan.Current.ID });
            this.gpgLabelDescription.Visible = true;
            this.gpgTextAreaDescription.Visible = false;
            this.skinButtonEdit.Visible = true;
            this.skinButtonCancelEdit.Visible = false;
            this.skinButtonSaveEdit.Visible = false;
            this.gpgLabelError.Visible = false;
            this.mDescriptionModified = false;
            return true;
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
            MappedObjectList<Clan> objects = DataAccess.GetObjects<Clan>("GetClanByMember", new object[] { playerName });
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

        private void SetCurrentGridView(object sender, MouseEventArgs e)
        {
            if (sender is GridView)
            {
                this.mSelectedParticipantView = sender as GridView;
            }
        }

        private void skinButtonCancelEdit_Click(object sender, EventArgs e)
        {
            this.CancelEdits();
        }

        private void skinButtonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void skinButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.gpgTextAreaDescription.Text = Clan.Current.Description;
                this.gpgLabelDescription.Visible = false;
                this.gpgTextAreaDescription.Visible = true;
                this.skinButtonEdit.Visible = false;
                this.skinButtonCancelEdit.Visible = true;
                this.skinButtonSaveEdit.Visible = true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                DlgMessage.ShowDialog(Loc.Get("<LOC>Unable to locate clan '{0}'", new object[] { exception.Message }));
            }
        }

        private void skinButtonLast_Click(object sender, EventArgs e)
        {
            this.mCurrentClan = this.ViewList.Find(this.CurrentClan).Previous.Value;
            this.Construct();
        }

        private void skinButtonNext_Click(object sender, EventArgs e)
        {
            this.mCurrentClan = this.ViewList.Find(this.CurrentClan).Next.Value;
            this.Construct();
        }

        private void skinButtonSaveEdit_Click(object sender, EventArgs e)
        {
            this.SaveEdits();
        }

        public override bool AllowRestoreWindow
        {
            get
            {
                return false;
            }
        }

        public ClanView CurrentClan
        {
            get
            {
                return this.mCurrentClan;
            }
        }

        public bool DescriptionModified
        {
            get
            {
                return this.mDescriptionModified;
            }
        }

        public IUser SelectedClanMember
        {
            get
            {
                try
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

        public LinkedList<ClanView> ViewList
        {
            get
            {
                return this.mViewList;
            }
        }
    }
}

