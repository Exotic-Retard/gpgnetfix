namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Timers;
    using System.Windows.Forms;

    public class DlgChangePassword : DlgBase
    {
        private IContainer components = null;
        private GPGLabel labelError;
        private GPGLabel lConfirm;
        private GPGLabel lOldPassword;
        private GPGLabel lPassword;
        private GPGLabel lStatus;
        private GPGLabel lUsername;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private System.Timers.Timer StatusTimer = new System.Timers.Timer(1000.0);
        private GPGTextBox tbConfirmPassword;
        public GPGTextBox tbOldPassword;
        public GPGTextBox tbPassword;
        public GPGTextBox tbUsername;

        public DlgChangePassword()
        {
            this.InitializeComponent();
            Loc.LocObject(this);
            this.StatusTimer.Elapsed += new ElapsedEventHandler(this.StatusTimer_Elapsed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoChangePassword()
        {
            this.labelError.Text = "";
            if (this.tbConfirmPassword.Text != this.tbPassword.Text)
            {
                this.labelError.Text = Loc.Get("<LOC>Your passwords do not match.");
                this.labelError.Visible = true;
            }
            else if (this.tbPassword.Text.Length < 6)
            {
                this.labelError.Text = Loc.Get("<LOC>Your password must be at least 6 characters long.");
                this.labelError.Visible = true;
            }
            else
            {
                this.SetEnabled(false);
                this.lStatus.Text = Loc.Get("<LOC>Changing Password");
                this.StatusTimer.Start();
                ThreadQueue.Quazal.Enqueue(typeof(User), "Login", this, "LoginState", new object[] { this.tbUsername.Text, this.tbOldPassword.Text, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort });
            }
        }

        private void FinishState(bool result)
        {
            this.StatusTimer.Stop();
            this.SetEnabled(true);
            if (result)
            {
                Program.Settings.Login.DefaultPassword = "";
                Program.Settings.Save();
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                this.labelError.Text = Loc.Get("<LOC>Unable to change your password.  Please restart GPGnet: Supreme Commander");
                this.labelError.Visible = true;
            }
        }

        private void InitializeComponent()
        {
            this.lStatus = new GPGLabel();
            this.tbConfirmPassword = new GPGTextBox();
            this.lConfirm = new GPGLabel();
            this.tbPassword = new GPGTextBox();
            this.tbUsername = new GPGTextBox();
            this.lUsername = new GPGLabel();
            this.lPassword = new GPGLabel();
            this.tbOldPassword = new GPGTextBox();
            this.lOldPassword = new GPGLabel();
            this.labelError = new GPGLabel();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.tbConfirmPassword.Properties.BeginInit();
            this.tbPassword.Properties.BeginInit();
            this.tbUsername.Properties.BeginInit();
            this.tbOldPassword.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.lStatus.AutoStyle = true;
            this.lStatus.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lStatus.ForeColor = Color.White;
            this.lStatus.IgnoreMouseWheel = false;
            this.lStatus.IsStyled = false;
            this.lStatus.Location = new Point(20, 0x3d);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new Size(0x149, 0x1c);
            this.lStatus.TabIndex = 0x16;
            this.lStatus.Text = "<LOC>Changing Password";
            this.lStatus.TextAlign = ContentAlignment.TopCenter;
            this.lStatus.TextStyle = TextStyles.Default;
            this.lStatus.Visible = false;
            this.tbConfirmPassword.Location = new Point(0x42, 0x10c);
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.Properties.Appearance.BackColor = Color.Black;
            this.tbConfirmPassword.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbConfirmPassword.Properties.Appearance.ForeColor = Color.White;
            this.tbConfirmPassword.Properties.Appearance.Options.UseBackColor = true;
            this.tbConfirmPassword.Properties.Appearance.Options.UseBorderColor = true;
            this.tbConfirmPassword.Properties.Appearance.Options.UseForeColor = true;
            this.tbConfirmPassword.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbConfirmPassword.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbConfirmPassword.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbConfirmPassword.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbConfirmPassword.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbConfirmPassword.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbConfirmPassword.Properties.BorderStyle = BorderStyles.Simple;
            this.tbConfirmPassword.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbConfirmPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbConfirmPassword.Properties.PasswordChar = '*';
            this.tbConfirmPassword.Size = new Size(0xe7, 20);
            this.tbConfirmPassword.TabIndex = 3;
            this.lConfirm.AutoSize = true;
            this.lConfirm.AutoStyle = true;
            this.lConfirm.Font = new Font("Arial", 9.75f);
            this.lConfirm.ForeColor = Color.White;
            this.lConfirm.IgnoreMouseWheel = false;
            this.lConfirm.IsStyled = false;
            this.lConfirm.Location = new Point(0x3f, 0xf9);
            this.lConfirm.Name = "lConfirm";
            this.lConfirm.Size = new Size(180, 0x10);
            this.lConfirm.TabIndex = 20;
            this.lConfirm.Text = "<LOC>Confirm new password";
            this.lConfirm.TextStyle = TextStyles.Default;
            this.tbPassword.Location = new Point(0x42, 0xd5);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Properties.Appearance.BackColor = Color.Black;
            this.tbPassword.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbPassword.Properties.Appearance.ForeColor = Color.White;
            this.tbPassword.Properties.Appearance.Options.UseBackColor = true;
            this.tbPassword.Properties.Appearance.Options.UseBorderColor = true;
            this.tbPassword.Properties.Appearance.Options.UseForeColor = true;
            this.tbPassword.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbPassword.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbPassword.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbPassword.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbPassword.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbPassword.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbPassword.Properties.BorderStyle = BorderStyles.Simple;
            this.tbPassword.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbPassword.Properties.PasswordChar = '*';
            this.tbPassword.Size = new Size(0xe7, 20);
            this.tbPassword.TabIndex = 2;
            this.tbUsername.Location = new Point(0x42, 0x6d);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Properties.Appearance.BackColor = Color.Black;
            this.tbUsername.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbUsername.Properties.Appearance.ForeColor = Color.White;
            this.tbUsername.Properties.Appearance.Options.UseBackColor = true;
            this.tbUsername.Properties.Appearance.Options.UseBorderColor = true;
            this.tbUsername.Properties.Appearance.Options.UseForeColor = true;
            this.tbUsername.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbUsername.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbUsername.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbUsername.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbUsername.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbUsername.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbUsername.Properties.BorderStyle = BorderStyles.Simple;
            this.tbUsername.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbUsername.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbUsername.Size = new Size(0xe7, 20);
            this.tbUsername.TabIndex = 0;
            this.lUsername.AutoSize = true;
            this.lUsername.AutoStyle = true;
            this.lUsername.Font = new Font("Arial", 9.75f);
            this.lUsername.ForeColor = Color.White;
            this.lUsername.IgnoreMouseWheel = false;
            this.lUsername.IsStyled = false;
            this.lUsername.Location = new Point(0x3f, 90);
            this.lUsername.Name = "lUsername";
            this.lUsername.Size = new Size(0x6d, 0x10);
            this.lUsername.TabIndex = 0x11;
            this.lUsername.Text = "<LOC>Username";
            this.lUsername.TextStyle = TextStyles.Default;
            this.lPassword.AutoSize = true;
            this.lPassword.AutoStyle = true;
            this.lPassword.Font = new Font("Arial", 9.75f);
            this.lPassword.ForeColor = Color.White;
            this.lPassword.IgnoreMouseWheel = false;
            this.lPassword.IsStyled = false;
            this.lPassword.Location = new Point(0x3f, 0xc2);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new Size(0x86, 0x10);
            this.lPassword.TabIndex = 0x12;
            this.lPassword.Text = "<LOC>New password";
            this.lPassword.TextStyle = TextStyles.Default;
            this.tbOldPassword.Location = new Point(0x42, 0xa3);
            this.tbOldPassword.Name = "tbOldPassword";
            this.tbOldPassword.Properties.Appearance.BackColor = Color.Black;
            this.tbOldPassword.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbOldPassword.Properties.Appearance.ForeColor = Color.White;
            this.tbOldPassword.Properties.Appearance.Options.UseBackColor = true;
            this.tbOldPassword.Properties.Appearance.Options.UseBorderColor = true;
            this.tbOldPassword.Properties.Appearance.Options.UseForeColor = true;
            this.tbOldPassword.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbOldPassword.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbOldPassword.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbOldPassword.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbOldPassword.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbOldPassword.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbOldPassword.Properties.BorderStyle = BorderStyles.Simple;
            this.tbOldPassword.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbOldPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbOldPassword.Properties.PasswordChar = '*';
            this.tbOldPassword.Size = new Size(0xe7, 20);
            this.tbOldPassword.TabIndex = 1;
            this.lOldPassword.AutoSize = true;
            this.lOldPassword.AutoStyle = true;
            this.lOldPassword.Font = new Font("Arial", 9.75f);
            this.lOldPassword.ForeColor = Color.White;
            this.lOldPassword.IgnoreMouseWheel = false;
            this.lOldPassword.IsStyled = false;
            this.lOldPassword.Location = new Point(0x3f, 0x90);
            this.lOldPassword.Name = "lOldPassword";
            this.lOldPassword.Size = new Size(0x81, 0x10);
            this.lOldPassword.TabIndex = 0x17;
            this.lOldPassword.Text = "<LOC>Old password";
            this.lOldPassword.TextStyle = TextStyles.Default;
            this.labelError.AutoStyle = true;
            this.labelError.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelError.ForeColor = Color.Red;
            this.labelError.IgnoreMouseWheel = false;
            this.labelError.IsStyled = false;
            this.labelError.Location = new Point(0x1f, 0x141);
            this.labelError.Name = "labelError";
            this.labelError.Size = new Size(310, 0x2e);
            this.labelError.TabIndex = 0x1b;
            this.labelError.Text = "This is a test error.  This is not usually visible.  It only happens when a login error occurs.";
            this.labelError.TextAlign = ContentAlignment.MiddleCenter;
            this.labelError.TextStyle = TextStyles.Default;
            this.labelError.Visible = false;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0xde, 0x130);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x4b, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 0x1c;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x8d, 0x130);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x4b, 0x17);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 0x1d;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x16e, 0x191);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.labelError);
            base.Controls.Add(this.tbOldPassword);
            base.Controls.Add(this.lOldPassword);
            base.Controls.Add(this.lStatus);
            base.Controls.Add(this.tbConfirmPassword);
            base.Controls.Add(this.lConfirm);
            base.Controls.Add(this.tbPassword);
            base.Controls.Add(this.tbUsername);
            base.Controls.Add(this.lUsername);
            base.Controls.Add(this.lPassword);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0x16e, 0x191);
            this.MinimumSize = new Size(0x16e, 0x191);
            base.Name = "DlgChangePassword";
            this.Text = "<LOC>Change Password";
            base.Controls.SetChildIndex(this.lPassword, 0);
            base.Controls.SetChildIndex(this.lUsername, 0);
            base.Controls.SetChildIndex(this.tbUsername, 0);
            base.Controls.SetChildIndex(this.tbPassword, 0);
            base.Controls.SetChildIndex(this.lConfirm, 0);
            base.Controls.SetChildIndex(this.tbConfirmPassword, 0);
            base.Controls.SetChildIndex(this.lStatus, 0);
            base.Controls.SetChildIndex(this.lOldPassword, 0);
            base.Controls.SetChildIndex(this.tbOldPassword, 0);
            base.Controls.SetChildIndex(this.labelError, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            this.tbConfirmPassword.Properties.EndInit();
            this.tbPassword.Properties.EndInit();
            this.tbUsername.Properties.EndInit();
            this.tbOldPassword.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoginState(bool result)
        {
            if (result)
            {
                ThreadQueue.Quazal.Enqueue(typeof(User), "ChangePassword", this, "FinishState", new object[] { this.tbPassword.Text });
            }
            else
            {
                this.StatusTimer.Stop();
                this.SetEnabled(true);
                this.labelError.Text = Loc.Get("<LOC>Login failed. Your current login name and password are invalid. Please check them and try again.");
                this.labelError.Visible = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tbUsername.Select();
            this.tbUsername.SelectionStart = this.tbUsername.Text.Length;
        }

        private void SetEnabled(bool status)
        {
            this.tbOldPassword.Enabled = status;
            this.tbUsername.Enabled = status;
            this.tbPassword.Enabled = status;
            this.tbConfirmPassword.Enabled = status;
            this.skinButtonOK.Enabled = status;
            this.lStatus.Visible = !status;
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            this.DoChangePassword();
        }

        private void StatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            base.BeginInvoke(delegate {
                if (!(base.Disposing || base.IsDisposed))
                {
                    this.lStatus.Text = this.lStatus.Text + ".";
                }
            });
        }
    }
}

