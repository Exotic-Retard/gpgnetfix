namespace GPG.Multiplayer.Client.Registration
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgRegisterSupcom : DlgBase
    {
        public SkinButton btnCancel;
        public SkinButton btnContinue;
        public SkinButton btnCreateAccount;
        private IContainer components = null;
        private FrmMain frm = null;
        private GPGCheckBox gpgCheckBoxMailer;
        private GPGLabel gpgLabel1;
        private GPGPictureBox gpgPictureBox1;
        private GPGLabel labelError;
        private GPGLabel lCDKey;
        private GPGLabel lEmail;
        private GPGLabel lKeyInfo;
        private DlgLogin mLogin = new DlgLogin();
        private GPGTextBox tbEmailAddress;

        public DlgRegisterSupcom()
        {
            this.InitializeComponent();
            Loc.LocObject(this);
            string name = @"Software\THQ\Gas Powered Games\Supreme Commander";
            if (Program.HasArg("/registerfa"))
            {
                name = @"Software\THQ\Gas Powered Games\Supreme Commander - Forged Alliance";
            }
            string str2 = "";
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(name);
                if (key != null)
                {
                    str2 = (string) key.GetValue("KEY", "");
                }
                if ((str2 == null) || (str2 == ""))
                {
                    key = Registry.LocalMachine.OpenSubKey(name);
                    if (key != null)
                    {
                        str2 = (string) key.GetValue("KEY", "");
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            switch (str2)
            {
                case null:
                case "":
                    str2 = "####-####-####-####-####";
                    break;
            }
            this.lKeyInfo.Text = str2;
            FrmMain.OnCancelLoad += new EventHandler(this.frm_OnCancelLoad);
            this.lKeyInfo.ForeColor = Program.Settings.Chat.Links.ChatColor;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DlgYesNo no = new DlgYesNo(null, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>Are you sure you want to cancel registration?"));
            if (no.ShowDialog() == DialogResult.Yes)
            {
                this.tbEmailAddress.Text = "";
                this.SubmitData();
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            this.SubmitData();
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            DlgLogin.ShowCreate = true;
            this.mLogin.Dispose();
            this.mLogin = null;
            base.Visible = false;
            this.frm = new FrmMain();
            this.frm.FormClosed += new FormClosedEventHandler(this.frm_FormClosed);
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
            this.labelError.TextStyle = TextStyles.Error;
            this.labelError.Text = Loc.Get(error);
            this.labelError.Visible = true;
            this.labelError.Refresh();
        }

        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.Close();
        }

        private void frm_OnCancelLoad(object sender, EventArgs e)
        {
            base.Close();
        }

        private void InitializeComponent()
        {
            this.lCDKey = new GPGLabel();
            this.tbEmailAddress = new GPGTextBox();
            this.btnContinue = new SkinButton();
            this.lKeyInfo = new GPGLabel();
            this.lEmail = new GPGLabel();
            this.gpgCheckBoxMailer = new GPGCheckBox();
            this.btnCreateAccount = new SkinButton();
            this.btnCancel = new SkinButton();
            this.labelError = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgPictureBox1 = new GPGPictureBox();
            this.tbEmailAddress.Properties.BeginInit();
            ((ISupportInitialize) this.gpgPictureBox1).BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.lCDKey.AutoSize = true;
            this.lCDKey.AutoStyle = true;
            this.lCDKey.Font = new Font("Arial", 9.75f);
            this.lCDKey.ForeColor = System.Drawing.Color.White;
            this.lCDKey.IgnoreMouseWheel = false;
            this.lCDKey.IsStyled = false;
            this.lCDKey.Location = new Point(0x2a, 0xd3);
            this.lCDKey.Name = "lCDKey";
            this.lCDKey.Size = new Size(0x5f, 0x10);
            this.lCDKey.TabIndex = 7;
            this.lCDKey.Text = "<LOC>CD Key";
            this.lCDKey.TextStyle = TextStyles.Default;
            this.tbEmailAddress.Location = new Point(0x2d, 0x117);
            this.tbEmailAddress.Name = "tbEmailAddress";
            this.tbEmailAddress.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.tbEmailAddress.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(0x52, 0x83, 190);
            this.tbEmailAddress.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.tbEmailAddress.Properties.Appearance.Options.UseBackColor = true;
            this.tbEmailAddress.Properties.Appearance.Options.UseBorderColor = true;
            this.tbEmailAddress.Properties.Appearance.Options.UseForeColor = true;
            this.tbEmailAddress.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbEmailAddress.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.FromArgb(0, 0, 0);
            this.tbEmailAddress.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbEmailAddress.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbEmailAddress.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbEmailAddress.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbEmailAddress.Properties.BorderStyle = BorderStyles.Simple;
            this.tbEmailAddress.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbEmailAddress.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbEmailAddress.Size = new Size(0x132, 20);
            this.tbEmailAddress.TabIndex = 8;
            this.btnContinue.AutoStyle = true;
            this.btnContinue.BackColor = System.Drawing.Color.Black;
            this.btnContinue.DialogResult = DialogResult.OK;
            this.btnContinue.DisabledForecolor = System.Drawing.Color.Gray;
            this.btnContinue.DrawEdges = true;
            this.btnContinue.FocusColor = System.Drawing.Color.Yellow;
            this.btnContinue.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.HorizontalScalingMode = ScalingModes.Tile;
            this.btnContinue.IsStyled = true;
            this.btnContinue.Location = new Point(0x2d, 0x176);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new Size(150, 0x1c);
            this.btnContinue.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnContinue.TabIndex = 0x17;
            this.btnContinue.Text = "<LOC>Finish";
            this.btnContinue.TextAlign = ContentAlignment.MiddleCenter;
            this.btnContinue.TextPadding = new Padding(0);
            this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
            this.lKeyInfo.AutoSize = true;
            this.lKeyInfo.AutoStyle = true;
            this.lKeyInfo.Font = new Font("Arial", 9.75f, FontStyle.Italic);
            this.lKeyInfo.ForeColor = System.Drawing.Color.White;
            this.lKeyInfo.IgnoreMouseWheel = false;
            this.lKeyInfo.IsStyled = false;
            this.lKeyInfo.Location = new Point(0x2b, 0xe3);
            this.lKeyInfo.Name = "lKeyInfo";
            this.lKeyInfo.Size = new Size(0xcc, 0x10);
            this.lKeyInfo.TabIndex = 0x18;
            this.lKeyInfo.Text = "XXXX-XXXX-XXXX-XXXX-XXXX";
            this.lKeyInfo.TextStyle = TextStyles.Title;
            this.lEmail.AutoSize = true;
            this.lEmail.AutoStyle = true;
            this.lEmail.Font = new Font("Arial", 9.75f);
            this.lEmail.ForeColor = System.Drawing.Color.White;
            this.lEmail.IgnoreMouseWheel = false;
            this.lEmail.IsStyled = false;
            this.lEmail.Location = new Point(0x2a, 260);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new Size(0x87, 0x10);
            this.lEmail.TabIndex = 0x19;
            this.lEmail.Text = "<LOC>Email Address";
            this.lEmail.TextStyle = TextStyles.Default;
            this.gpgCheckBoxMailer.CheckAlign = ContentAlignment.TopLeft;
            this.gpgCheckBoxMailer.Checked = true;
            this.gpgCheckBoxMailer.CheckState = CheckState.Checked;
            this.gpgCheckBoxMailer.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgCheckBoxMailer.Location = new Point(0x2e, 0x131);
            this.gpgCheckBoxMailer.Name = "gpgCheckBoxMailer";
            this.gpgCheckBoxMailer.Size = new Size(0x131, 0x24);
            this.gpgCheckBoxMailer.TabIndex = 0x1a;
            this.gpgCheckBoxMailer.Text = "<LOC>Send me exclusive Supreme Commander and Gas Powered Games updates via email";
            this.gpgCheckBoxMailer.TextAlign = ContentAlignment.TopLeft;
            this.gpgCheckBoxMailer.UseVisualStyleBackColor = true;
            this.btnCreateAccount.AutoStyle = true;
            this.btnCreateAccount.BackColor = System.Drawing.Color.Black;
            this.btnCreateAccount.DialogResult = DialogResult.OK;
            this.btnCreateAccount.DisabledForecolor = System.Drawing.Color.Gray;
            this.btnCreateAccount.DrawEdges = true;
            this.btnCreateAccount.FocusColor = System.Drawing.Color.Yellow;
            this.btnCreateAccount.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCreateAccount.ForeColor = System.Drawing.Color.White;
            this.btnCreateAccount.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCreateAccount.IsStyled = true;
            this.btnCreateAccount.Location = new Point(0x2d, 0x198);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new Size(0x132, 0x1c);
            this.btnCreateAccount.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnCreateAccount.TabIndex = 0x1b;
            this.btnCreateAccount.Text = "<LOC>Create my GPGnet account now";
            this.btnCreateAccount.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCreateAccount.TextPadding = new Padding(0);
            this.btnCreateAccount.Click += new EventHandler(this.btnCreateAccount_Click);
            this.btnCancel.AutoStyle = true;
            this.btnCancel.BackColor = System.Drawing.Color.Black;
            this.btnCancel.DialogResult = DialogResult.OK;
            this.btnCancel.DisabledForecolor = System.Drawing.Color.Gray;
            this.btnCancel.DrawEdges = true;
            this.btnCancel.FocusColor = System.Drawing.Color.Yellow;
            this.btnCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCancel.IsStyled = true;
            this.btnCancel.Location = new Point(0xc9, 0x176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(150, 0x1c);
            this.btnCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnCancel.TabIndex = 0x18;
            this.btnCancel.Text = "<LOC>Cancel";
            this.btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCancel.TextPadding = new Padding(0);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.labelError.AutoStyle = true;
            this.labelError.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.IgnoreMouseWheel = false;
            this.labelError.IsStyled = false;
            this.labelError.Location = new Point(0x2b, 0x157);
            this.labelError.Name = "labelError";
            this.labelError.Size = new Size(0x134, 0x3b);
            this.labelError.TabIndex = 0x1c;
            this.labelError.Text = "<LOC>This is a test error.  This is not usually visible.  It only happens when a login error occurs.";
            this.labelError.TextStyle = TextStyles.Status;
            this.labelError.Visible = false;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = System.Drawing.Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x2b, 0x91);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x134, 0x42);
            this.gpgLabel1.TabIndex = 0x1d;
            this.gpgLabel1.Text = "<LOC id=_9e9ed56d2db37471cd3003807992eb67>Please register your copy of Supreme Commander!  Keep in contact with us by optionally providing your email address.";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgPictureBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPictureBox1.Image = Resources.fakebanner;
            this.gpgPictureBox1.Location = new Point(6, 0x3d);
            this.gpgPictureBox1.Name = "gpgPictureBox1";
            this.gpgPictureBox1.Size = new Size(0x180, 0x42);
            this.gpgPictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            this.gpgPictureBox1.TabIndex = 30;
            this.gpgPictureBox1.TabStop = false;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x18c, 0x1f5);
            base.Controls.Add(this.gpgPictureBox1);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.tbEmailAddress);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnCreateAccount);
            base.Controls.Add(this.gpgCheckBoxMailer);
            base.Controls.Add(this.lKeyInfo);
            base.Controls.Add(this.btnContinue);
            base.Controls.Add(this.lCDKey);
            base.Controls.Add(this.labelError);
            base.Controls.Add(this.lEmail);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgRegisterSupcom";
            this.Text = "<LOC>Supreme Commander Registration";
            base.Controls.SetChildIndex(this.lEmail, 0);
            base.Controls.SetChildIndex(this.labelError, 0);
            base.Controls.SetChildIndex(this.lCDKey, 0);
            base.Controls.SetChildIndex(this.btnContinue, 0);
            base.Controls.SetChildIndex(this.lKeyInfo, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxMailer, 0);
            base.Controls.SetChildIndex(this.btnCreateAccount, 0);
            base.Controls.SetChildIndex(this.btnCancel, 0);
            base.Controls.SetChildIndex(this.tbEmailAddress, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgPictureBox1, 0);
            this.tbEmailAddress.Properties.EndInit();
            ((ISupportInitialize) this.gpgPictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SubmitData()
        {
            string pwd = "fakeact";
            string username = "";
            if (this.tbEmailAddress.Text != "")
            {
                if ((((this.tbEmailAddress.Text.IndexOf("@") < 0) || (this.tbEmailAddress.Text.IndexOf(".") < 0)) || (this.tbEmailAddress.Text.Length < 8)) || (this.tbEmailAddress.Text.Length > 60))
                {
                    this.Error("<LOC>Your email address does not appear to be valid. Please provide a valid email address.");
                    return;
                }
                username = this.tbEmailAddress.Text;
            }
            else
            {
                username = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", "");
            }
            username = "_" + username;
            string text = this.tbEmailAddress.Text;
            if (text == "")
            {
                text = "unknown@gaspowered.com";
            }
            try
            {
                bool flag = User.CreateLogin(username, pwd, text, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort);
                User.Login(username, pwd, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort);
                int num = 0;
                if (this.gpgCheckBoxMailer.Checked)
                {
                    num = 1;
                }
                DataAccess.ExecuteQuery("RegisterCDKey", new object[] { this.tbEmailAddress.Text, this.lKeyInfo.Text, num });
                base.Close();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                base.Close();
            }
        }
    }
}

