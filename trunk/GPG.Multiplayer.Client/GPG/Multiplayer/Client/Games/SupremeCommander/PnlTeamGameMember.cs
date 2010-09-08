namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PnlTeamGameMember : PnlBase
    {
        private IContainer components = null;
        private FactionPicker factionPicker;
        private SkinButton gpgLabelDisband;
        private SkinButton gpgLabelInvite;
        private SkinButton gpgLabelKick;
        private SkinButton gpgLabelLeave;
        private GPGLabel gpgLabelName;
        private GPGPictureBox gpgPictureBoxReady;
        private TeamGame mTeam = null;
        private TeamGame.TeamGameMember mTeamMember = null;
        private PingIndicator pingIndicator;

        public event TeamGameMemberEventHandler DisbandClick;

        public event TeamGameMemberEventHandler InviteClick;

        public event TeamGameMemberEventHandler KickClick;

        public event TeamGameMemberEventHandler LeaveClick;

        public event TeamGameMemberEventHandler MemberChanged;

        public PnlTeamGameMember(TeamGame team)
        {
            this.InitializeComponent();
            this.factionPicker.SelectedFactionChanged += new EventHandler(this.factionPicker_SelectedFactionChanged);
            this.mTeam = team;
        }

        public void Disable()
        {
            this.factionPicker.Enabled = false;
        }

        public void DisableButtons()
        {
            this.gpgLabelDisband.Enabled = false;
            this.gpgLabelInvite.Enabled = false;
            this.gpgLabelKick.Enabled = false;
            this.gpgLabelLeave.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Enable()
        {
            this.factionPicker.Enabled = true;
        }

        public void EnableButtons()
        {
            this.gpgLabelDisband.Enabled = true;
            this.gpgLabelInvite.Enabled = true;
            this.gpgLabelKick.Enabled = true;
            this.gpgLabelLeave.Enabled = true;
        }

        private void factionPicker_SelectedFactionChanged(object sender, EventArgs e)
        {
            if (!this.IsOpen)
            {
                this.TeamMember.Faction = this.factionPicker.SelectedFaction;
                if (this.MemberChanged != null)
                {
                    this.MemberChanged(this, this.TeamMember);
                }
            }
        }

        private void gpgLabelDisband_Click(object sender, EventArgs e)
        {
            if (this.DisbandClick != null)
            {
                this.DisbandClick(this, this.TeamMember);
            }
        }

        private void gpgLabelInvite_Click(object sender, EventArgs e)
        {
            if (this.InviteClick != null)
            {
                this.InviteClick(this, this.TeamMember);
            }
        }

        private void gpgLabelKick_Click(object sender, EventArgs e)
        {
            if (this.KickClick != null)
            {
                this.KickClick(this, this.TeamMember);
            }
        }

        private void gpgLabelLeave_Click(object sender, EventArgs e)
        {
            if (this.LeaveClick != null)
            {
                this.LeaveClick(this, this.TeamMember);
            }
        }

        private void gpgPictureBoxReady_MouseUp(object sender, MouseEventArgs e)
        {
            if (((!this.IsOpen && this.IsSelf) && !this.Team.HasLaunched) && (e.Button == MouseButtons.Left))
            {
                this.TeamMember.IsReady = !this.TeamMember.IsReady;
                this.UpdatePlayer();
                if (this.MemberChanged != null)
                {
                    this.MemberChanged(this, this.TeamMember);
                }
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PnlTeamGameMember));
            this.gpgLabelName = new GPGLabel();
            this.gpgPictureBoxReady = new GPGPictureBox();
            this.factionPicker = new FactionPicker();
            this.pingIndicator = new PingIndicator();
            this.gpgLabelInvite = new SkinButton();
            this.gpgLabelDisband = new SkinButton();
            this.gpgLabelKick = new SkinButton();
            this.gpgLabelLeave = new SkinButton();
            ((ISupportInitialize) this.gpgPictureBoxReady).BeginInit();
            ((ISupportInitialize) this.pingIndicator).BeginInit();
            base.SuspendLayout();
            this.gpgLabelName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelName.AutoStyle = true;
            this.gpgLabelName.Font = new Font("Arial", 9.75f);
            this.gpgLabelName.ForeColor = Color.ForestGreen;
            this.gpgLabelName.IgnoreMouseWheel = false;
            this.gpgLabelName.IsStyled = false;
            this.gpgLabelName.Location = new Point(3, 9);
            this.gpgLabelName.Name = "gpgLabelName";
            this.gpgLabelName.Size = new Size(0x7d, 0x10);
            this.gpgLabelName.TabIndex = 0;
            this.gpgLabelName.Text = "<LOC>Open";
            this.gpgLabelName.TextStyle = TextStyles.ColoredBold;
            this.gpgPictureBoxReady.Location = new Point(0x13d, 5);
            this.gpgPictureBoxReady.Name = "gpgPictureBoxReady";
            this.gpgPictureBoxReady.Size = new Size(0x18, 0x18);
            this.gpgPictureBoxReady.SizeMode = PictureBoxSizeMode.CenterImage;
            this.gpgPictureBoxReady.TabIndex = 9;
            this.gpgPictureBoxReady.TabStop = false;
            this.gpgPictureBoxReady.MouseUp += new MouseEventHandler(this.gpgPictureBoxReady_MouseUp);
            this.factionPicker.AutoStyle = false;
            this.factionPicker.BackColor = Color.Black;
            this.factionPicker.DialogResult = DialogResult.OK;
            this.factionPicker.DisabledForecolor = Color.Gray;
            this.factionPicker.DrawEdges = true;
            this.factionPicker.FocusColor = Color.Yellow;
            this.factionPicker.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.factionPicker.HorizontalScalingMode = ScalingModes.Tile;
            this.factionPicker.Icon = (Image) manager.GetObject("factionPicker.Icon");
            this.factionPicker.IsStyled = true;
            this.factionPicker.Location = new Point(0xab, 4);
            this.factionPicker.Name = "factionPicker";
            this.factionPicker.SearchMode = false;
            this.factionPicker.Size = new Size(0x88, 0x1a);
            this.factionPicker.SkinBasePath = @"Controls\Button\ChatroomList";
            this.factionPicker.TabIndex = 7;
            this.factionPicker.Text = "factionPicker1";
            this.factionPicker.TextAlign = ContentAlignment.MiddleLeft;
            this.factionPicker.TextPadding = new Padding(0x20, 0, 0, 0);
            this.pingIndicator.Image = (Image) manager.GetObject("pingIndicator.Image");
            this.pingIndicator.Location = new Point(0x169, 7);
            this.pingIndicator.Name = "pingIndicator";
            this.pingIndicator.PingMonitor = null;
            this.pingIndicator.Size = new Size(40, 20);
            this.pingIndicator.TabIndex = 10;
            this.pingIndicator.TabStop = false;
            this.pingIndicator.Visible = false;
            this.gpgLabelInvite.AutoStyle = true;
            this.gpgLabelInvite.BackColor = Color.Black;
            this.gpgLabelInvite.DialogResult = DialogResult.OK;
            this.gpgLabelInvite.DisabledForecolor = Color.Gray;
            this.gpgLabelInvite.DrawEdges = true;
            this.gpgLabelInvite.FocusColor = Color.Yellow;
            this.gpgLabelInvite.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgLabelInvite.HorizontalScalingMode = ScalingModes.Tile;
            this.gpgLabelInvite.IsStyled = true;
            this.gpgLabelInvite.Location = new Point(0x169, 8);
            this.gpgLabelInvite.Name = "gpgLabelInvite";
            this.gpgLabelInvite.Size = new Size(0x48, 0x12);
            this.gpgLabelInvite.SkinBasePath = @"Controls\Button\Round Edge";
            this.gpgLabelInvite.TabIndex = 11;
            this.gpgLabelInvite.Text = "<LOC>Invite";
            this.gpgLabelInvite.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelInvite.TextPadding = new Padding(0);
            this.gpgLabelInvite.Visible = false;
            this.gpgLabelInvite.Click += new EventHandler(this.gpgLabelInvite_Click);
            this.gpgLabelDisband.AutoStyle = true;
            this.gpgLabelDisband.BackColor = Color.Black;
            this.gpgLabelDisband.DialogResult = DialogResult.OK;
            this.gpgLabelDisband.DisabledForecolor = Color.Gray;
            this.gpgLabelDisband.DrawEdges = true;
            this.gpgLabelDisband.FocusColor = Color.Yellow;
            this.gpgLabelDisband.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgLabelDisband.ForeColor = Color.White;
            this.gpgLabelDisband.HorizontalScalingMode = ScalingModes.Tile;
            this.gpgLabelDisband.IsStyled = true;
            this.gpgLabelDisband.Location = new Point(0x169, 8);
            this.gpgLabelDisband.Name = "gpgLabelDisband";
            this.gpgLabelDisband.Size = new Size(0x48, 0x12);
            this.gpgLabelDisband.SkinBasePath = @"Controls\Button\Round Edge";
            this.gpgLabelDisband.TabIndex = 12;
            this.gpgLabelDisband.Text = "<LOC>Disband";
            this.gpgLabelDisband.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelDisband.TextPadding = new Padding(0);
            this.gpgLabelDisband.Click += new EventHandler(this.gpgLabelDisband_Click);
            this.gpgLabelKick.AutoStyle = true;
            this.gpgLabelKick.BackColor = Color.Black;
            this.gpgLabelKick.DialogResult = DialogResult.OK;
            this.gpgLabelKick.DisabledForecolor = Color.Gray;
            this.gpgLabelKick.DrawEdges = true;
            this.gpgLabelKick.FocusColor = Color.Yellow;
            this.gpgLabelKick.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgLabelKick.HorizontalScalingMode = ScalingModes.Tile;
            this.gpgLabelKick.IsStyled = true;
            this.gpgLabelKick.Location = new Point(0x169, 8);
            this.gpgLabelKick.Name = "gpgLabelKick";
            this.gpgLabelKick.Size = new Size(0x48, 0x12);
            this.gpgLabelKick.SkinBasePath = @"Controls\Button\Round Edge";
            this.gpgLabelKick.TabIndex = 13;
            this.gpgLabelKick.Text = "<LOC>Kick";
            this.gpgLabelKick.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelKick.TextPadding = new Padding(0);
            this.gpgLabelKick.Visible = false;
            this.gpgLabelKick.Click += new EventHandler(this.gpgLabelKick_Click);
            this.gpgLabelLeave.AutoStyle = true;
            this.gpgLabelLeave.BackColor = Color.Black;
            this.gpgLabelLeave.DialogResult = DialogResult.OK;
            this.gpgLabelLeave.DisabledForecolor = Color.Gray;
            this.gpgLabelLeave.DrawEdges = true;
            this.gpgLabelLeave.FocusColor = Color.Yellow;
            this.gpgLabelLeave.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgLabelLeave.HorizontalScalingMode = ScalingModes.Tile;
            this.gpgLabelLeave.IsStyled = true;
            this.gpgLabelLeave.Location = new Point(0x169, 8);
            this.gpgLabelLeave.Name = "gpgLabelLeave";
            this.gpgLabelLeave.Size = new Size(0x48, 0x12);
            this.gpgLabelLeave.SkinBasePath = @"Controls\Button\Round Edge";
            this.gpgLabelLeave.TabIndex = 14;
            this.gpgLabelLeave.Text = "<LOC>Leave";
            this.gpgLabelLeave.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelLeave.TextPadding = new Padding(0);
            this.gpgLabelLeave.Visible = false;
            this.gpgLabelLeave.Click += new EventHandler(this.gpgLabelLeave_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLabelDisband);
            base.Controls.Add(this.gpgLabelLeave);
            base.Controls.Add(this.gpgLabelKick);
            base.Controls.Add(this.gpgLabelInvite);
            base.Controls.Add(this.pingIndicator);
            base.Controls.Add(this.gpgPictureBoxReady);
            base.Controls.Add(this.factionPicker);
            base.Controls.Add(this.gpgLabelName);
            base.Name = "PnlTeamGameMember";
            base.Size = new Size(0x1db, 0x22);
            ((ISupportInitialize) this.gpgPictureBoxReady).EndInit();
            ((ISupportInitialize) this.pingIndicator).EndInit();
            base.ResumeLayout(false);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            this.gpgLabelName.BackColor = this.BackColor;
        }

        public void SetPlayer(TeamGame.TeamGameMember member)
        {
            this.mTeamMember = member;
            this.pingIndicator.PingMonitor = this.TeamMember;
            if (this.TeamMember != null)
            {
                if (this.TeamMember.IsSelf && this.TeamMember.IsTeamLeader)
                {
                    if (!this.Team.HasLaunched)
                    {
                        this.Enable();
                    }
                    this.gpgLabelInvite.Visible = false;
                    this.gpgLabelKick.Visible = false;
                    this.gpgLabelLeave.Visible = false;
                    this.gpgLabelDisband.Visible = true;
                }
                else if (this.TeamMember.IsSelf)
                {
                    if (!this.Team.HasLaunched)
                    {
                        this.Enable();
                    }
                    this.gpgLabelDisband.Visible = false;
                    this.gpgLabelInvite.Visible = false;
                    this.gpgLabelKick.Visible = false;
                    this.gpgLabelLeave.Visible = true;
                }
                else if (this.Team.TeamLeader.IsSelf)
                {
                    this.Disable();
                    this.gpgLabelDisband.Visible = false;
                    this.gpgLabelInvite.Visible = false;
                    this.gpgLabelLeave.Visible = false;
                    this.gpgLabelKick.Visible = true;
                }
                else
                {
                    this.Disable();
                    this.gpgLabelDisband.Visible = false;
                    this.gpgLabelInvite.Visible = false;
                    this.gpgLabelKick.Visible = false;
                    this.gpgLabelLeave.Visible = false;
                }
                this.UpdatePlayer();
                this.gpgLabelName.ForeColor = Color.ForestGreen;
            }
            else
            {
                if (this.Team.TeamLeader.IsSelf)
                {
                    this.Disable();
                    this.gpgLabelDisband.Visible = false;
                    this.gpgLabelKick.Visible = false;
                    this.gpgLabelLeave.Visible = false;
                    this.gpgLabelInvite.Visible = true;
                }
                else
                {
                    this.Disable();
                    this.gpgLabelDisband.Visible = false;
                    this.gpgLabelInvite.Visible = false;
                    this.gpgLabelKick.Visible = false;
                    this.gpgLabelLeave.Visible = false;
                }
                this.gpgLabelName.Text = Loc.Get("<LOC>Open");
                this.gpgLabelName.ForeColor = Color.DodgerBlue;
                this.factionPicker.SelectItem(SupcomLookups._Factions.Any);
                this.factionPicker.Text = Loc.Get("<LOC>Open");
                this.gpgPictureBoxReady.Image = ControlRes.open_ready;
            }
        }

        public void UpdatePlayer()
        {
            this.gpgLabelName.Text = this.TeamMember.Name;
            this.factionPicker.SelectItem(this.TeamMember.Faction);
            if (this.TeamMember.IsReady)
            {
                this.gpgPictureBoxReady.Image = ControlRes.ready;
            }
            else
            {
                this.gpgPictureBoxReady.Image = ControlRes.not_ready;
            }
        }

        public bool IsOpen
        {
            get
            {
                return (this.TeamMember == null);
            }
        }

        public bool IsSelf
        {
            get
            {
                return (!this.IsOpen && this.TeamMember.IsSelf);
            }
        }

        public TeamGame Team
        {
            get
            {
                return this.mTeam;
            }
        }

        public TeamGame.TeamGameMember TeamMember
        {
            get
            {
                return this.mTeamMember;
            }
        }

        public delegate void TeamGameMemberEventHandler(PnlTeamGameMember sender, TeamGame.TeamGameMember member);
    }
}

