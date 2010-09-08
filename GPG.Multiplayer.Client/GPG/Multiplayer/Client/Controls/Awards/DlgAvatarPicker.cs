namespace GPG.Multiplayer.Client.Controls.Awards
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Statistics;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgAvatarPicker : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGPanel gpgPanelAvatars;
        private bool IsInitialized = false;
        private User mPlayer;
        private Avatar mSelectedAvatar = null;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonClear;

        public DlgAvatarPicker(User player)
        {
            this.InitializeComponent();
            this.mPlayer = player;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DoLayout()
        {
            int num = 0;
            int num2 = 0;
            foreach (PictureBox box in this.gpgPanelAvatars.Controls)
            {
                box.Left = num;
                box.Top = num2;
                num += box.Width + 6;
                if ((num + box.Width) > this.gpgPanelAvatars.Width)
                {
                    num2 += box.Height + 6;
                    num = 0;
                }
            }
        }

        private void img_Click(object sender, EventArgs e)
        {
            this.mSelectedAvatar = (sender as PictureBox).Tag as Avatar;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void InitializeComponent()
        {
            this.skinButtonClear = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.gpgPanelAvatars = new GPGPanel();
            this.skinButtonCancel = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x16a, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.skinButtonClear.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonClear.AutoStyle = true;
            this.skinButtonClear.BackColor = Color.Transparent;
            this.skinButtonClear.ButtonState = 0;
            this.skinButtonClear.DialogResult = DialogResult.OK;
            this.skinButtonClear.DisabledForecolor = Color.Gray;
            this.skinButtonClear.DrawColor = Color.White;
            this.skinButtonClear.DrawEdges = true;
            this.skinButtonClear.FocusColor = Color.Yellow;
            this.skinButtonClear.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClear.ForeColor = Color.White;
            this.skinButtonClear.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClear.IsStyled = true;
            this.skinButtonClear.Location = new Point(0x7d, 0x15f);
            this.skinButtonClear.Name = "skinButtonClear";
            this.skinButtonClear.Size = new Size(0x99, 0x1a);
            this.skinButtonClear.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClear, null);
            this.skinButtonClear.TabIndex = 11;
            this.skinButtonClear.TabStop = true;
            this.skinButtonClear.Text = "<LOC>Reset Avatar";
            this.skinButtonClear.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClear.TextPadding = new Padding(0);
            this.skinButtonClear.Click += new EventHandler(this.skinButtonClear_Click);
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x4a);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x94, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 13;
            this.gpgLabel1.Text = "<LOC>Available Avatars";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgPanelAvatars.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelAvatars.AutoScroll = true;
            this.gpgPanelAvatars.BackColor = Color.Transparent;
            this.gpgPanelAvatars.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelAvatars.BorderThickness = 2;
            this.gpgPanelAvatars.DrawBorder = false;
            this.gpgPanelAvatars.Location = new Point(12, 0x5d);
            this.gpgPanelAvatars.Name = "gpgPanelAvatars";
            this.gpgPanelAvatars.Size = new Size(0x18d, 0xfc);
            base.ttDefault.SetSuperTip(this.gpgPanelAvatars, null);
            this.gpgPanelAvatars.TabIndex = 12;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Transparent;
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
            this.skinButtonCancel.Location = new Point(0x11c, 0x15f);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x7d, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 10;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1a5, 430);
            base.Controls.Add(this.skinButtonClear);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgPanelAvatars);
            base.Controls.Add(this.skinButtonCancel);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x135, 0x113);
            base.Name = "DlgAvatarPicker";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Select an Avatar";
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgPanelAvatars, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonClear, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                VGen0 method = null;
                try
                {
                    if (this.Player != null)
                    {
                        foreach (PlayerAvatar playerAvatar in PlayerAvatar.GetPlayerAvatars(this.Player.ID))
                        {
                            if (this.Player.Avatar != playerAvatar.Avatar.ID)
                            {
                                Image avatarImg = playerAvatar.Avatar.Image;
                                if (base.Disposing || base.IsDisposed)
                                {
                                    return;
                                }
                                base.Invoke(delegate {
                                    try
                                    {
                                        PictureBox control = new PictureBox {
                                            Cursor = Cursors.Hand,
                                            SizeMode = PictureBoxSizeMode.AutoSize,
                                            Image = avatarImg,
                                            Tag = playerAvatar.Avatar
                                        };
                                        this.ttDefault.SetToolTip(control, Loc.Get(playerAvatar.Avatar.Description));
                                        control.Click += new EventHandler(this.img_Click);
                                        this.gpgPanelAvatars.Controls.Add(control);
                                    }
                                    catch (Exception exception)
                                    {
                                        ErrorLog.WriteLine(exception);
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        using (Dictionary<int, Avatar>.ValueCollection.Enumerator enumerator = Avatar.AllAvatars.Values.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Avatar avatar = enumerator.Current;
                                Image avatarImg = avatar.Image;
                                if (base.Disposing || base.IsDisposed)
                                {
                                    return;
                                }
                                base.Invoke(delegate {
                                    try
                                    {
                                        PictureBox control = new PictureBox {
                                            Cursor = Cursors.Hand,
                                            SizeMode = PictureBoxSizeMode.AutoSize,
                                            Image = avatarImg,
                                            Tag = avatar
                                        };
                                        this.ttDefault.SetToolTip(control, Loc.Get(avatar.Description));
                                        control.Click += new EventHandler(this.img_Click);
                                        this.gpgPanelAvatars.Controls.Add(control);
                                    }
                                    catch (Exception exception)
                                    {
                                        ErrorLog.WriteLine(exception);
                                    }
                                });
                            }
                        }
                    }
                    if (!base.Disposing && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                if (this.Player == null)
                                {
                                    this.skinButtonClear.Visible = false;
                                }
                                this.DoLayout();
                                this.IsInitialized = true;
                            };
                        }
                        base.Invoke(method);
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.IsInitialized)
            {
                this.DoLayout();
            }
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonClear_Click(object sender, EventArgs e)
        {
            this.mSelectedAvatar = null;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public User Player
        {
            get
            {
                return this.mPlayer;
            }
        }

        public Avatar SelectedAvatar
        {
            get
            {
                return this.mSelectedAvatar;
            }
        }
    }
}

