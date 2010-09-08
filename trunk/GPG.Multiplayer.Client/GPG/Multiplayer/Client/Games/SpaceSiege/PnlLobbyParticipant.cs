namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PnlLobbyParticipant : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabelCharacterName;
        private GPGPictureBox gpgPictureBoxReady;
        private SpaceSiegeGame mGame = null;
        private bool mIsOpen = true;
        private SpaceSiegeGameParticipant mParticipant = null;
        private int mPostition = -1;
        internal SkinButton skinButtonLaunch;
        private SkinButton skinButtonSelectCharacter;
        private SkinDropDown skinDropDownPlayer;

        public event LobbyParticipantEventHandler CharacterChanged;

        public event LobbyParticipantEventHandler ClosePosition;

        public event LobbyParticipantEventHandler DisbandLobby;

        public event StringEventHandler ErrorMessage;

        public event LobbyParticipantEventHandler InviteParticipant;

        public event LobbyParticipantEventHandler KickParticipant;

        public event LobbyParticipantEventHandler LaunchGame;

        public event LobbyParticipantEventHandler LeaveLobby;

        public event LobbyParticipantEventHandler OccupyPosition;

        public event LobbyParticipantEventHandler OpenPosition;

        public event LobbyParticipantEventHandler ReadyStateChanged;

        public PnlLobbyParticipant()
        {
            this.InitializeComponent();
            this.gpgPictureBoxReady.Image = ControlRes.open_ready;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private SkinMenuItem GetMenuItemClosed()
        {
            SkinMenuItem item = new SkinMenuItem("<LOC>Closed");
            item.Click += new EventHandler(this.miClosed_Click);
            return item;
        }

        private SkinMenuItem GetMenuItemDisband()
        {
            SkinMenuItem item = new SkinMenuItem("<LOC>Disband");
            item.Click += new EventHandler(this.miDisband_Click);
            return item;
        }

        private SkinMenuItem GetMenuItemInvite()
        {
            SkinMenuItem item = new SkinMenuItem("<LOC>Invite");
            item.Click += new EventHandler(this.miInvite_Click);
            return item;
        }

        private SkinMenuItem GetMenuItemKick()
        {
            SkinMenuItem item = new SkinMenuItem("<LOC>Kick");
            item.Click += new EventHandler(this.miKick_Click);
            return item;
        }

        private SkinMenuItem GetMenuItemLeave()
        {
            SkinMenuItem item = new SkinMenuItem("<LOC>Leave");
            item.Click += new EventHandler(this.miLeave_Click);
            return item;
        }

        private SkinMenuItem GetMenuItemOccupy()
        {
            SkinMenuItem item = new SkinMenuItem("<LOC>Occupy");
            item.Click += new EventHandler(this.miOccupy_Click);
            return item;
        }

        private SkinMenuItem GetMenuItemOpen()
        {
            SkinMenuItem item = new SkinMenuItem("<LOC>Open");
            item.Click += new EventHandler(this.miOpen_Click);
            return item;
        }

        private void gpgPictureBoxReady_Click(object sender, EventArgs e)
        {
            if ((this.Participant != null) && this.Participant.IsSelf)
            {
                if (this.Participant.Character == null)
                {
                    if (this.ErrorMessage != null)
                    {
                        this.ErrorMessage("<LOC>You must select a character before you can be ready!");
                    }
                }
                else
                {
                    this.Participant.IsReady = !this.Participant.IsReady;
                    this.SetParticipant(this.Participant);
                    if (this.ReadyStateChanged != null)
                    {
                        this.ReadyStateChanged(this);
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.skinDropDownPlayer = new SkinDropDown();
            this.gpgPictureBoxReady = new GPGPictureBox();
            this.skinButtonSelectCharacter = new SkinButton();
            this.gpgLabelCharacterName = new GPGLabel();
            this.skinButtonLaunch = new SkinButton();
            ((ISupportInitialize) this.gpgPictureBoxReady).BeginInit();
            base.SuspendLayout();
            this.skinDropDownPlayer.AutoStyle = true;
            this.skinDropDownPlayer.BackColor = Color.Black;
            this.skinDropDownPlayer.ButtonState = 0;
            this.skinDropDownPlayer.DialogResult = DialogResult.OK;
            this.skinDropDownPlayer.DisabledForecolor = Color.Gray;
            this.skinDropDownPlayer.DrawColor = Color.Black;
            this.skinDropDownPlayer.DrawEdges = true;
            this.skinDropDownPlayer.FocusColor = Color.Yellow;
            this.skinDropDownPlayer.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinDropDownPlayer.ForeColor = Color.Black;
            this.skinDropDownPlayer.HorizontalScalingMode = ScalingModes.Stretch;
            this.skinDropDownPlayer.Icon = null;
            this.skinDropDownPlayer.IsStyled = true;
            this.skinDropDownPlayer.Location = new Point(6, 6);
            this.skinDropDownPlayer.Name = "skinDropDownPlayer";
            this.skinDropDownPlayer.Size = new Size(250, 0x1a);
            this.skinDropDownPlayer.SkinBasePath = @"Controls\Button\ChatroomList";
            this.skinDropDownPlayer.TabIndex = 9;
            this.skinDropDownPlayer.TabStop = true;
            this.skinDropDownPlayer.Text = "<LOC>Open";
            this.skinDropDownPlayer.TextAlign = ContentAlignment.MiddleLeft;
            this.skinDropDownPlayer.TextPadding = new Padding(6, 0, 0, 0);
            this.gpgPictureBoxReady.Location = new Point(0x1bf, 8);
            this.gpgPictureBoxReady.Name = "gpgPictureBoxReady";
            this.gpgPictureBoxReady.Size = new Size(0x18, 0x18);
            this.gpgPictureBoxReady.SizeMode = PictureBoxSizeMode.CenterImage;
            this.gpgPictureBoxReady.TabIndex = 10;
            this.gpgPictureBoxReady.TabStop = false;
            this.gpgPictureBoxReady.Click += new EventHandler(this.gpgPictureBoxReady_Click);
            this.skinButtonSelectCharacter.AutoStyle = true;
            this.skinButtonSelectCharacter.BackColor = Color.Transparent;
            this.skinButtonSelectCharacter.ButtonState = 0;
            this.skinButtonSelectCharacter.DialogResult = DialogResult.OK;
            this.skinButtonSelectCharacter.DisabledForecolor = Color.Gray;
            this.skinButtonSelectCharacter.DrawColor = Color.White;
            this.skinButtonSelectCharacter.DrawEdges = true;
            this.skinButtonSelectCharacter.FocusColor = Color.Yellow;
            this.skinButtonSelectCharacter.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSelectCharacter.ForeColor = Color.White;
            this.skinButtonSelectCharacter.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSelectCharacter.IsStyled = true;
            this.skinButtonSelectCharacter.Location = new Point(0x1ed, 10);
            this.skinButtonSelectCharacter.Name = "skinButtonSelectCharacter";
            this.skinButtonSelectCharacter.Size = new Size(0x99, 0x12);
            this.skinButtonSelectCharacter.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonSelectCharacter.TabIndex = 11;
            this.skinButtonSelectCharacter.TabStop = true;
            this.skinButtonSelectCharacter.Text = "<LOC>Select Character";
            this.skinButtonSelectCharacter.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSelectCharacter.TextPadding = new Padding(0);
            this.skinButtonSelectCharacter.Visible = false;
            this.skinButtonSelectCharacter.Click += new EventHandler(this.skinButtonSelectCharacter_Click);
            this.gpgLabelCharacterName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelCharacterName.AutoSize = true;
            this.gpgLabelCharacterName.AutoStyle = true;
            this.gpgLabelCharacterName.Font = new Font("Arial", 9.75f);
            this.gpgLabelCharacterName.ForeColor = Color.White;
            this.gpgLabelCharacterName.IgnoreMouseWheel = false;
            this.gpgLabelCharacterName.IsStyled = false;
            this.gpgLabelCharacterName.Location = new Point(0x106, 12);
            this.gpgLabelCharacterName.Name = "gpgLabelCharacterName";
            this.gpgLabelCharacterName.Size = new Size(0xb1, 0x10);
            this.gpgLabelCharacterName.TabIndex = 12;
            this.gpgLabelCharacterName.Text = "<LOC>No character selected";
            this.gpgLabelCharacterName.TextStyle = TextStyles.ColoredBold;
            this.skinButtonLaunch.AutoStyle = true;
            this.skinButtonLaunch.BackColor = Color.Transparent;
            this.skinButtonLaunch.ButtonState = 0;
            this.skinButtonLaunch.DialogResult = DialogResult.OK;
            this.skinButtonLaunch.DisabledForecolor = Color.Gray;
            this.skinButtonLaunch.DrawColor = Color.White;
            this.skinButtonLaunch.DrawEdges = true;
            this.skinButtonLaunch.FocusColor = Color.Yellow;
            this.skinButtonLaunch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLaunch.ForeColor = Color.White;
            this.skinButtonLaunch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLaunch.IsStyled = true;
            this.skinButtonLaunch.Location = new Point(0x28c, 10);
            this.skinButtonLaunch.Name = "skinButtonLaunch";
            this.skinButtonLaunch.Size = new Size(0x6a, 0x12);
            this.skinButtonLaunch.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonLaunch.TabIndex = 12;
            this.skinButtonLaunch.TabStop = true;
            this.skinButtonLaunch.Text = "<LOC>Launch";
            this.skinButtonLaunch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLaunch.TextPadding = new Padding(0);
            this.skinButtonLaunch.Visible = false;
            this.skinButtonLaunch.Click += new EventHandler(this.skinButtonLaunch_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            base.Controls.Add(this.skinButtonLaunch);
            base.Controls.Add(this.gpgPictureBoxReady);
            base.Controls.Add(this.gpgLabelCharacterName);
            base.Controls.Add(this.skinButtonSelectCharacter);
            base.Controls.Add(this.skinDropDownPlayer);
            base.Name = "PnlLobbyParticipant";
            base.Size = new Size(0x2fc, 0x26);
            ((ISupportInitialize) this.gpgPictureBoxReady).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void miClosed_Click(object sender, EventArgs e)
        {
            if (this.ClosePosition != null)
            {
                this.ClosePosition(this);
            }
        }

        private void miDisband_Click(object sender, EventArgs e)
        {
            if (this.DisbandLobby != null)
            {
                this.DisbandLobby(this);
            }
        }

        private void miInvite_Click(object sender, EventArgs e)
        {
            if (this.InviteParticipant != null)
            {
                this.InviteParticipant(this);
            }
        }

        private void miKick_Click(object sender, EventArgs e)
        {
            if (this.KickParticipant != null)
            {
                this.KickParticipant(this);
            }
        }

        private void miLeave_Click(object sender, EventArgs e)
        {
            if (this.LeaveLobby != null)
            {
                this.LeaveLobby(this);
            }
        }

        private void miOccupy_Click(object sender, EventArgs e)
        {
            if (this.OccupyPosition != null)
            {
                this.OccupyPosition(this);
            }
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            if (this.OpenPosition != null)
            {
                this.OpenPosition(this);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.Participant != null) && this.Participant.IsHost)
            {
                this.skinDropDownPlayer.ForeColor = Color.Gold;
                this.skinDropDownPlayer.DrawColor = Color.Gold;
            }
            else
            {
                this.skinDropDownPlayer.ForeColor = Color.White;
                this.skinDropDownPlayer.DrawColor = Color.White;
            }
            base.OnPaint(e);
        }

        public void SetParticipant(SpaceSiegeGameParticipant participant)
        {
            if ((this.Game != null) && (this.Postition >= 0))
            {
                if (((participant != null) && (participant.Character == null)) && (participant.ID == User.Current.ID))
                {
                    string path = PlayerCharacter.CharacterFilePath + @"\characters.gpgnet";
                    if (File.Exists(path))
                    {
                        StreamReader reader = new StreamReader(path);
                        string str2 = reader.ReadToEnd();
                        reader.Close();
                        string[] strArray = str2.Split(new char[] { '\x000e' });
                        int index = 0;
                        while (index < strArray.Length)
                        {
                            string data = strArray[index];
                            participant.Character = PlayerCharacter.FromString(data);
                            break;
                        }
                    }
                }
                this.mParticipant = participant;
                this.skinDropDownPlayer.Menu.MenuItems.Clear();
                if (participant == null)
                {
                    this.skinButtonSelectCharacter.Visible = false;
                    this.skinButtonLaunch.Visible = false;
                    this.gpgLabelCharacterName.Visible = false;
                    if (this.IsOpen)
                    {
                        this.skinDropDownPlayer.Text = Loc.Get("<LOC>Open");
                        if (this.Game.Host.IsSelf)
                        {
                            this.skinDropDownPlayer.Menu.MenuItems.Add(this.GetMenuItemInvite());
                            this.skinDropDownPlayer.Menu.MenuItems.Add(this.GetMenuItemClosed());
                        }
                        this.gpgPictureBoxReady.Visible = true;
                        this.gpgPictureBoxReady.Image = ControlRes.open_ready;
                    }
                    else
                    {
                        if (this.Game.Host.IsSelf)
                        {
                            this.skinDropDownPlayer.Menu.MenuItems.Add(this.GetMenuItemOpen());
                        }
                        this.skinDropDownPlayer.Text = Loc.Get("<LOC>Closed");
                        this.gpgPictureBoxReady.Visible = false;
                    }
                }
                else
                {
                    this.skinButtonSelectCharacter.Visible = participant.IsSelf;
                    this.skinButtonLaunch.Visible = participant.IsSelf && participant.IsHost;
                    participant.Position = this.Postition;
                    this.skinDropDownPlayer.Text = participant.Name;
                    if (participant.Character == null)
                    {
                        this.gpgLabelCharacterName.Text = Loc.Get("<LOC>No character selected");
                        this.gpgLabelCharacterName.ForeColor = Program.Settings.StylePreferences.MasterForeColor;
                        this.gpgLabelCharacterName.Visible = true;
                    }
                    else
                    {
                        this.gpgLabelCharacterName.Text = participant.Character.CharacterName;
                        this.gpgLabelCharacterName.ForeColor = participant.Character.CharacterColor;
                        this.gpgLabelCharacterName.Visible = true;
                    }
                    if (participant.IsSelf && this.Game.Host.IsSelf)
                    {
                        this.skinButtonLaunch.Enabled = this.Game.CanLaunch && !this.Game.HasLaunched;
                        this.skinDropDownPlayer.Menu.MenuItems.Add(this.GetMenuItemDisband());
                    }
                    else if (participant.IsSelf)
                    {
                        this.skinDropDownPlayer.Menu.MenuItems.Add(this.GetMenuItemLeave());
                    }
                    else if (this.Game.Host.IsSelf)
                    {
                        this.skinDropDownPlayer.Menu.MenuItems.Add(this.GetMenuItemKick());
                    }
                    this.gpgPictureBoxReady.Visible = true;
                    if (participant.IsReady)
                    {
                        this.gpgPictureBoxReady.Image = ControlRes.ready;
                    }
                    else
                    {
                        this.gpgPictureBoxReady.Image = ControlRes.not_ready;
                    }
                }
            }
        }

        private void skinButtonLaunch_Click(object sender, EventArgs e)
        {
            if (this.Participant.IsHost && (this.LaunchGame != null))
            {
                this.LaunchGame(this);
            }
        }

        private void skinButtonSelectCharacter_Click(object sender, EventArgs e)
        {
            DlgCharacterSelect select = new DlgCharacterSelect(this.Participant);
            select.CharacterDeleted += delegate (string name) {
                if ((this.Participant.Character != null) && (name == this.Participant.Character.CharacterName))
                {
                    this.Participant.Character = null;
                    this.SetParticipant(this.Participant);
                }
            };
            if (select.ShowDialog() == DialogResult.OK)
            {
                this.Participant.Character = select.SelectedCharacter;
                this.SetParticipant(this.Participant);
                if (this.CharacterChanged != null)
                {
                    this.CharacterChanged(this);
                }
            }
        }

        public SpaceSiegeGame Game
        {
            get
            {
                return this.mGame;
            }
            set
            {
                this.mGame = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return this.mIsOpen;
            }
            set
            {
                this.mIsOpen = value;
            }
        }

        public SpaceSiegeGameParticipant Participant
        {
            get
            {
                return this.mParticipant;
            }
        }

        [Browsable(true)]
        public int Postition
        {
            get
            {
                return this.mPostition;
            }
            set
            {
                this.mPostition = value;
            }
        }

        protected override bool ScaleChildren
        {
            get
            {
                return false;
            }
        }
    }
}

