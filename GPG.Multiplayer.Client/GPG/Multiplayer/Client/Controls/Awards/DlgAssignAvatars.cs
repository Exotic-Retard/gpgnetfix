namespace GPG.Multiplayer.Client.Controls.Awards
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Statistics;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgAssignAvatars : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGPictureBox gpgPictureBoxAvatar;
        private GPGTextBox gpgTextBoxPlayer;
        private Avatar SelectedAvatar = null;
        private SkinButton skinButtonAward;
        private SkinButton skinButtonAwardClan;
        private SkinButton skinButtonRemoveAll;
        private SkinButton skinButtonRemoveClan;
        private SkinButton skinButtonRemovePlayer;
        private SkinButton skinButtonSetAvatar;

        public DlgAssignAvatars()
        {
            this.InitializeComponent();
            this.gpgPictureBoxAvatar.Image = Avatar.Default.Image;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgPictureBoxAvatar_Click(object sender, EventArgs e)
        {
            Avatar.ClearCachedData();
            DlgAvatarPicker picker = new DlgAvatarPicker(null);
            if (picker.ShowDialog() == DialogResult.OK)
            {
                this.SelectedAvatar = picker.SelectedAvatar;
                if (this.SelectedAvatar != null)
                {
                    this.gpgPictureBoxAvatar.Image = this.SelectedAvatar.Image;
                }
            }
        }

        private void InitializeComponent()
        {
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxPlayer = new GPGTextBox();
            this.skinButtonAward = new SkinButton();
            this.gpgPictureBoxAvatar = new GPGPictureBox();
            this.gpgLabel2 = new GPGLabel();
            this.skinButtonSetAvatar = new SkinButton();
            this.skinButtonRemovePlayer = new SkinButton();
            this.skinButtonRemoveAll = new SkinButton();
            this.skinButtonAwardClan = new SkinButton();
            this.skinButtonRemoveClan = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxPlayer.Properties.BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxAvatar).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x16b, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 90);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x66, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "Avatar To Award";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxPlayer.Location = new Point(12, 0xa8);
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
            this.gpgTextBoxPlayer.Size = new Size(0x165, 20);
            this.gpgTextBoxPlayer.TabIndex = 8;
            this.skinButtonAward.AutoStyle = true;
            this.skinButtonAward.BackColor = Color.Transparent;
            this.skinButtonAward.ButtonState = 0;
            this.skinButtonAward.DialogResult = DialogResult.OK;
            this.skinButtonAward.DisabledForecolor = Color.Gray;
            this.skinButtonAward.DrawColor = Color.White;
            this.skinButtonAward.DrawEdges = true;
            this.skinButtonAward.FocusColor = Color.Yellow;
            this.skinButtonAward.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAward.ForeColor = Color.White;
            this.skinButtonAward.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAward.IsStyled = true;
            this.skinButtonAward.Location = new Point(12, 0xd1);
            this.skinButtonAward.Name = "skinButtonAward";
            this.skinButtonAward.Size = new Size(0xc5, 0x1a);
            this.skinButtonAward.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonAward, null);
            this.skinButtonAward.TabIndex = 9;
            this.skinButtonAward.TabStop = true;
            this.skinButtonAward.Text = "Award Avatar To Player";
            this.skinButtonAward.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAward.TextPadding = new Padding(0);
            this.skinButtonAward.Click += new EventHandler(this.skinButtonAward_Click);
            this.gpgPictureBoxAvatar.Cursor = Cursors.Hand;
            this.gpgPictureBoxAvatar.Location = new Point(12, 0x6d);
            this.gpgPictureBoxAvatar.Name = "gpgPictureBoxAvatar";
            this.gpgPictureBoxAvatar.Size = new Size(40, 20);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxAvatar, null);
            this.gpgPictureBoxAvatar.TabIndex = 10;
            this.gpgPictureBoxAvatar.TabStop = false;
            this.gpgPictureBoxAvatar.Click += new EventHandler(this.gpgPictureBoxAvatar_Click);
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0x95);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x2a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 11;
            this.gpgLabel2.Text = "Name";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.skinButtonSetAvatar.AutoStyle = true;
            this.skinButtonSetAvatar.BackColor = Color.Transparent;
            this.skinButtonSetAvatar.ButtonState = 0;
            this.skinButtonSetAvatar.DialogResult = DialogResult.OK;
            this.skinButtonSetAvatar.DisabledForecolor = Color.Gray;
            this.skinButtonSetAvatar.DrawColor = Color.White;
            this.skinButtonSetAvatar.DrawEdges = true;
            this.skinButtonSetAvatar.FocusColor = Color.Yellow;
            this.skinButtonSetAvatar.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSetAvatar.ForeColor = Color.White;
            this.skinButtonSetAvatar.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSetAvatar.IsStyled = true;
            this.skinButtonSetAvatar.Location = new Point(0xd7, 0xd1);
            this.skinButtonSetAvatar.Name = "skinButtonSetAvatar";
            this.skinButtonSetAvatar.Size = new Size(0xc5, 0x1a);
            this.skinButtonSetAvatar.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSetAvatar, null);
            this.skinButtonSetAvatar.TabIndex = 12;
            this.skinButtonSetAvatar.TabStop = true;
            this.skinButtonSetAvatar.Text = "Set Players Avatar";
            this.skinButtonSetAvatar.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSetAvatar.TextPadding = new Padding(0);
            this.skinButtonSetAvatar.Click += new EventHandler(this.skinButtonSetAvatar_Click);
            this.skinButtonRemovePlayer.AutoStyle = true;
            this.skinButtonRemovePlayer.BackColor = Color.Transparent;
            this.skinButtonRemovePlayer.ButtonState = 0;
            this.skinButtonRemovePlayer.DialogResult = DialogResult.OK;
            this.skinButtonRemovePlayer.DisabledForecolor = Color.Gray;
            this.skinButtonRemovePlayer.DrawColor = Color.White;
            this.skinButtonRemovePlayer.DrawEdges = true;
            this.skinButtonRemovePlayer.FocusColor = Color.Yellow;
            this.skinButtonRemovePlayer.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRemovePlayer.ForeColor = Color.White;
            this.skinButtonRemovePlayer.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRemovePlayer.IsStyled = true;
            this.skinButtonRemovePlayer.Location = new Point(12, 0x112);
            this.skinButtonRemovePlayer.Name = "skinButtonRemovePlayer";
            this.skinButtonRemovePlayer.Size = new Size(0xc5, 0x1a);
            this.skinButtonRemovePlayer.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRemovePlayer, null);
            this.skinButtonRemovePlayer.TabIndex = 10;
            this.skinButtonRemovePlayer.TabStop = true;
            this.skinButtonRemovePlayer.Text = "Remove Avatar From Player";
            this.skinButtonRemovePlayer.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRemovePlayer.TextPadding = new Padding(0);
            this.skinButtonRemovePlayer.Click += new EventHandler(this.skinButtonRemovePlayer_Click);
            this.skinButtonRemoveAll.AutoStyle = true;
            this.skinButtonRemoveAll.BackColor = Color.Transparent;
            this.skinButtonRemoveAll.ButtonState = 0;
            this.skinButtonRemoveAll.DialogResult = DialogResult.OK;
            this.skinButtonRemoveAll.DisabledForecolor = Color.Gray;
            this.skinButtonRemoveAll.DrawColor = Color.White;
            this.skinButtonRemoveAll.DrawEdges = true;
            this.skinButtonRemoveAll.FocusColor = Color.Yellow;
            this.skinButtonRemoveAll.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRemoveAll.ForeColor = Color.White;
            this.skinButtonRemoveAll.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRemoveAll.IsStyled = true;
            this.skinButtonRemoveAll.Location = new Point(0xd7, 0x112);
            this.skinButtonRemoveAll.Name = "skinButtonRemoveAll";
            this.skinButtonRemoveAll.Size = new Size(0xc5, 0x1a);
            this.skinButtonRemoveAll.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRemoveAll, null);
            this.skinButtonRemoveAll.TabIndex = 13;
            this.skinButtonRemoveAll.TabStop = true;
            this.skinButtonRemoveAll.Text = "Remove Avatar From All";
            this.skinButtonRemoveAll.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRemoveAll.TextPadding = new Padding(0);
            this.skinButtonRemoveAll.Click += new EventHandler(this.skinButtonRemoveAll_Click);
            this.skinButtonAwardClan.AutoStyle = true;
            this.skinButtonAwardClan.BackColor = Color.Transparent;
            this.skinButtonAwardClan.ButtonState = 0;
            this.skinButtonAwardClan.DialogResult = DialogResult.OK;
            this.skinButtonAwardClan.DisabledForecolor = Color.Gray;
            this.skinButtonAwardClan.DrawColor = Color.White;
            this.skinButtonAwardClan.DrawEdges = true;
            this.skinButtonAwardClan.FocusColor = Color.Yellow;
            this.skinButtonAwardClan.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAwardClan.ForeColor = Color.White;
            this.skinButtonAwardClan.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAwardClan.IsStyled = true;
            this.skinButtonAwardClan.Location = new Point(12, 0xf1);
            this.skinButtonAwardClan.Name = "skinButtonAwardClan";
            this.skinButtonAwardClan.Size = new Size(0xc5, 0x1a);
            this.skinButtonAwardClan.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonAwardClan, null);
            this.skinButtonAwardClan.TabIndex = 14;
            this.skinButtonAwardClan.TabStop = true;
            this.skinButtonAwardClan.Text = "Award Avatar To Clan";
            this.skinButtonAwardClan.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAwardClan.TextPadding = new Padding(0);
            this.skinButtonAwardClan.Click += new EventHandler(this.skinButtonAwardClan_Click);
            this.skinButtonRemoveClan.AutoStyle = true;
            this.skinButtonRemoveClan.BackColor = Color.Transparent;
            this.skinButtonRemoveClan.ButtonState = 0;
            this.skinButtonRemoveClan.DialogResult = DialogResult.OK;
            this.skinButtonRemoveClan.DisabledForecolor = Color.Gray;
            this.skinButtonRemoveClan.DrawColor = Color.White;
            this.skinButtonRemoveClan.DrawEdges = true;
            this.skinButtonRemoveClan.FocusColor = Color.Yellow;
            this.skinButtonRemoveClan.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRemoveClan.ForeColor = Color.White;
            this.skinButtonRemoveClan.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRemoveClan.IsStyled = true;
            this.skinButtonRemoveClan.Location = new Point(0xd7, 0xf1);
            this.skinButtonRemoveClan.Name = "skinButtonRemoveClan";
            this.skinButtonRemoveClan.Size = new Size(0xc5, 0x1a);
            this.skinButtonRemoveClan.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRemoveClan, null);
            this.skinButtonRemoveClan.TabIndex = 15;
            this.skinButtonRemoveClan.TabStop = true;
            this.skinButtonRemoveClan.Text = "Remove Avatar From Clan";
            this.skinButtonRemoveClan.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRemoveClan.TextPadding = new Padding(0);
            this.skinButtonRemoveClan.Click += new EventHandler(this.skinButtonRemoveClan_Click);
            base.AcceptButton = this.skinButtonAward;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1a6, 0x182);
            base.Controls.Add(this.skinButtonRemoveClan);
            base.Controls.Add(this.skinButtonAwardClan);
            base.Controls.Add(this.skinButtonRemoveAll);
            base.Controls.Add(this.skinButtonRemovePlayer);
            base.Controls.Add(this.skinButtonSetAvatar);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgPictureBoxAvatar);
            base.Controls.Add(this.skinButtonAward);
            base.Controls.Add(this.gpgTextBoxPlayer);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MinimumSize = new Size(0x1a6, 0x150);
            base.Name = "DlgAssignAvatars";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Assign Avatars";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPlayer, 0);
            base.Controls.SetChildIndex(this.skinButtonAward, 0);
            base.Controls.SetChildIndex(this.gpgPictureBoxAvatar, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.skinButtonSetAvatar, 0);
            base.Controls.SetChildIndex(this.skinButtonRemovePlayer, 0);
            base.Controls.SetChildIndex(this.skinButtonRemoveAll, 0);
            base.Controls.SetChildIndex(this.skinButtonAwardClan, 0);
            base.Controls.SetChildIndex(this.skinButtonRemoveClan, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxPlayer.Properties.EndInit();
            ((ISupportInitialize) this.gpgPictureBoxAvatar).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonAward_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (this.SelectedAvatar == null)
            {
                base.Error(this.gpgPictureBoxAvatar, "You must select an avatar", new object[0]);
            }
            else if ((this.gpgTextBoxPlayer.Text == null) || (this.gpgTextBoxPlayer.Text.Length < 1))
            {
                base.Error(this.gpgTextBoxPlayer, "You must provide a player name", new object[0]);
            }
            else
            {
                int @int = new QuazalQuery("GetPlayerIDFromName", new object[] { this.gpgTextBoxPlayer.Text }).GetInt();
                if (@int <= 0)
                {
                    base.Error(this.gpgTextBoxPlayer, "Unable to locate {0}", new object[] { this.gpgTextBoxPlayer.Text });
                }
                else if (new QuazalQuery("GiveAvatarToPlayer", new object[] { @int, this.SelectedAvatar.ID }).ExecuteNonQuery())
                {
                    base.SetStatus("Avatar sucessfully awarded to {0}", new object[] { this.gpgTextBoxPlayer.Text });
                }
                else
                {
                    base.SetStatus("Error awarding avatar to {0}, they may already have it", new object[] { this.gpgTextBoxPlayer.Text });
                }
            }
        }

        private void skinButtonAwardClan_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (this.SelectedAvatar == null)
            {
                base.Error(this.gpgPictureBoxAvatar, "You must select an avatar", new object[0]);
            }
            else if (((this.gpgTextBoxPlayer.Text == null) || (this.gpgTextBoxPlayer.Text.Length < 1)) || (this.gpgTextBoxPlayer.Text.Length > 3))
            {
                base.Error(this.gpgTextBoxPlayer, "You must provide a clan abbreviation", new object[0]);
            }
            else
            {
                string text = this.gpgTextBoxPlayer.Text;
                if (new QuazalQuery("AwardAvatarToClan", new object[] { this.SelectedAvatar.ID, text }).ExecuteNonQuery())
                {
                    base.SetStatus("Avatar sucessfully awarded to clan {0}", new object[] { text });
                }
                else
                {
                    base.SetStatus("Error awarding avatar to clan {0}, they may already have it", new object[] { text });
                }
            }
        }

        private void skinButtonRemoveAll_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (this.SelectedAvatar == null)
            {
                base.Error(this.gpgPictureBoxAvatar, "You must select an avatar", new object[0]);
            }
            else if (new QuazalQuery("RemoveAvatarFromAll", new object[] { this.SelectedAvatar.ID, this.SelectedAvatar.ID }).ExecuteNonQuery())
            {
                base.SetStatus("Avatar sucessfully removed from all players.", new object[] { this.gpgTextBoxPlayer.Text });
            }
            else
            {
                base.SetStatus("Error removing avatar from all players.", new object[] { this.gpgTextBoxPlayer.Text });
            }
        }

        private void skinButtonRemoveClan_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (this.SelectedAvatar == null)
            {
                base.Error(this.gpgPictureBoxAvatar, "You must select an avatar", new object[0]);
            }
            else if (((this.gpgTextBoxPlayer.Text == null) || (this.gpgTextBoxPlayer.Text.Length < 1)) || (this.gpgTextBoxPlayer.Text.Length > 3))
            {
                base.Error(this.gpgTextBoxPlayer, "You must provide a clan abbreviation", new object[0]);
            }
            else
            {
                string text = this.gpgTextBoxPlayer.Text;
                if (new QuazalQuery("RemoveAvatarFromClan", new object[] { text, this.SelectedAvatar.ID, text, this.SelectedAvatar.ID }).ExecuteNonQuery())
                {
                    base.SetStatus("Avatar sucessfully removed from clan {0}", new object[] { text });
                }
                else
                {
                    base.SetStatus("Error removing avatar from clan {0}, they may not have it", new object[] { text });
                }
            }
        }

        private void skinButtonRemovePlayer_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (this.SelectedAvatar == null)
            {
                base.Error(this.gpgPictureBoxAvatar, "You must select an avatar", new object[0]);
            }
            else if ((this.gpgTextBoxPlayer.Text == null) || (this.gpgTextBoxPlayer.Text.Length < 1))
            {
                base.Error(this.gpgTextBoxPlayer, "You must provide a player name", new object[0]);
            }
            else
            {
                int @int = new QuazalQuery("GetPlayerIDFromName", new object[] { this.gpgTextBoxPlayer.Text }).GetInt();
                if (@int <= 0)
                {
                    base.Error(this.gpgTextBoxPlayer, "Unable to locate {0}", new object[] { this.gpgTextBoxPlayer.Text });
                }
                else if (new QuazalQuery("RemoveAvatarFromPlayer", new object[] { @int, this.SelectedAvatar.ID, @int, this.SelectedAvatar.ID }).ExecuteNonQuery())
                {
                    base.SetStatus("Avatar sucessfully removed from {0}", new object[] { this.gpgTextBoxPlayer.Text });
                }
                else
                {
                    base.SetStatus("Error removing avatar from {0}, they may not have it", new object[] { this.gpgTextBoxPlayer.Text });
                }
            }
        }

        private void skinButtonSetAvatar_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (this.SelectedAvatar == null)
            {
                base.Error(this.gpgPictureBoxAvatar, "You must select an avatar", new object[0]);
            }
            else if ((this.gpgTextBoxPlayer.Text == null) || (this.gpgTextBoxPlayer.Text.Length < 1))
            {
                base.Error(this.gpgTextBoxPlayer, "You must provide a player name", new object[0]);
            }
            else
            {
                User user;
                if (!base.MainForm.TryFindUser(this.gpgTextBoxPlayer.Text, false, out user))
                {
                    base.Error(this.gpgTextBoxPlayer, "Unable to locate {0}", new object[] { this.gpgTextBoxPlayer.Text });
                }
                else
                {
                    user.Avatar = this.SelectedAvatar.ID;
                    if (new QuazalQuery("SetAvatarByPlayerID", new object[] { this.SelectedAvatar.ID, user.ID }).ExecuteNonQuery())
                    {
                        base.MainForm.RefreshGathering(user, null);
                        base.SetStatus("Avatar sucessfully awarded to {0}", new object[] { this.gpgTextBoxPlayer.Text });
                    }
                    else
                    {
                        base.SetStatus("Error awarding avatar to {0}, they may already have it", new object[] { this.gpgTextBoxPlayer.Text });
                    }
                }
            }
        }
    }
}

