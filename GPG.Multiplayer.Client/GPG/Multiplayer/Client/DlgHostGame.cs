namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgHostGame : DlgBase
    {
        private GPGCheckBox cbPassProtect;
        private IContainer components;
        private FlowLayoutPanel flowLayoutPanel1;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabelError;
        private GPGPanel gpgPanel1;
        private GPGLabel lGameName;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonHost;
        private GPGTextBox tbGameName;
        private GPGTextBox tbPassword;

        public DlgHostGame(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            Loc.LocObject(this);
        }

        private void cbPassProtect_CheckedChanged(object sender, EventArgs e)
        {
            this.tbPassword.Visible = this.cbPassProtect.Checked;
            if (!this.tbPassword.Visible)
            {
                this.tbPassword.Text = "";
            }
        }

        private void cbPassProtect_Click(object sender, EventArgs e)
        {
            this.tbPassword.Visible = this.cbPassProtect.Checked;
            if (!this.tbPassword.Visible)
            {
                this.tbPassword.Text = "";
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

        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new FlowLayoutPanel();
            this.gpgLabelError = new GPGLabel();
            this.gpgPanel1 = new GPGPanel();
            this.skinButtonHost = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.tbGameName = new GPGTextBox();
            this.lGameName = new GPGLabel();
            this.tbPassword = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.cbPassProtect = new GPGCheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.gpgPanel1.SuspendLayout();
            this.tbGameName.Properties.BeginInit();
            this.tbPassword.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.flowLayoutPanel1.Controls.Add(this.gpgLabelError);
            this.flowLayoutPanel1.Controls.Add(this.gpgPanel1);
            this.flowLayoutPanel1.Location = new Point(0x3b, 0xb0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new Size(0x16a, 0x39);
            this.flowLayoutPanel1.TabIndex = 0x20;
            this.gpgLabelError.AutoSize = true;
            this.gpgLabelError.AutoStyle = false;
            this.gpgLabelError.Font = new Font("Arial", 9.75f);
            this.gpgLabelError.ForeColor = Color.Red;
            this.gpgLabelError.IgnoreMouseWheel = false;
            this.gpgLabelError.IsStyled = false;
            this.gpgLabelError.Location = new Point(3, 0);
            this.gpgLabelError.Name = "gpgLabelError";
            this.gpgLabelError.Size = new Size(0x58, 0x10);
            this.gpgLabelError.TabIndex = 0x1f;
            this.gpgLabelError.Text = "gpgLabelError";
            this.gpgLabelError.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelError.TextStyle = TextStyles.Default;
            this.gpgLabelError.Visible = false;
            this.gpgPanel1.Controls.Add(this.skinButtonHost);
            this.gpgPanel1.Controls.Add(this.skinButtonCancel);
            this.gpgPanel1.Location = new Point(3, 0x13);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(0x167, 0x21);
            this.gpgPanel1.TabIndex = 0x20;
            this.skinButtonHost.AutoStyle = true;
            this.skinButtonHost.BackColor = Color.Black;
            this.skinButtonHost.DialogResult = DialogResult.OK;
            this.skinButtonHost.DisabledForecolor = Color.Gray;
            this.skinButtonHost.DrawEdges = true;
            this.skinButtonHost.FocusColor = Color.Yellow;
            this.skinButtonHost.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonHost.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonHost.IsStyled = true;
            this.skinButtonHost.Location = new Point(110, 6);
            this.skinButtonHost.Name = "skinButtonHost";
            this.skinButtonHost.Size = new Size(0x68, 0x17);
            this.skinButtonHost.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonHost.TabIndex = 30;
            this.skinButtonHost.Text = "<LOC>Host Game";
            this.skinButtonHost.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonHost.TextPadding = new Padding(0);
            this.skinButtonHost.Click += new EventHandler(this.skinButtonHost_Click);
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(220, 6);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x68, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 0x1d;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.tbGameName.Location = new Point(0x3b, 0x63);
            this.tbGameName.Name = "tbGameName";
            this.tbGameName.Properties.Appearance.BackColor = Color.Black;
            this.tbGameName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbGameName.Properties.Appearance.ForeColor = Color.White;
            this.tbGameName.Properties.Appearance.Options.UseBackColor = true;
            this.tbGameName.Properties.Appearance.Options.UseBorderColor = true;
            this.tbGameName.Properties.Appearance.Options.UseForeColor = true;
            this.tbGameName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbGameName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbGameName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbGameName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbGameName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbGameName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbGameName.Properties.BorderStyle = BorderStyles.Simple;
            this.tbGameName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbGameName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbGameName.Properties.MaxLength = 50;
            this.tbGameName.Size = new Size(0x147, 20);
            this.tbGameName.TabIndex = 30;
            this.lGameName.AutoSize = true;
            this.lGameName.AutoStyle = true;
            this.lGameName.Font = new Font("Arial", 9.75f);
            this.lGameName.ForeColor = Color.White;
            this.lGameName.IgnoreMouseWheel = false;
            this.lGameName.IsStyled = false;
            this.lGameName.Location = new Point(0x38, 80);
            this.lGameName.Name = "lGameName";
            this.lGameName.Size = new Size(0x7b, 0x10);
            this.lGameName.TabIndex = 0x1d;
            this.lGameName.Text = "<LOC>Game Name";
            this.lGameName.TextStyle = TextStyles.Default;
            this.tbPassword.Location = new Point(0x3b, 0x92);
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
            this.tbPassword.Properties.MaxLength = 50;
            this.tbPassword.Properties.PasswordChar = '*';
            this.tbPassword.Size = new Size(0x147, 20);
            this.tbPassword.TabIndex = 0x22;
            this.tbPassword.Visible = false;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x38, 0x7f);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x128, 0x10);
            this.gpgLabel1.TabIndex = 0x21;
            this.gpgLabel1.Text = "<LOC>Enable password protection for this game?";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.cbPassProtect.AutoSize = true;
            this.cbPassProtect.Location = new Point(0x2b, 0x7f);
            this.cbPassProtect.Name = "cbPassProtect";
            this.cbPassProtect.Size = new Size(15, 14);
            this.cbPassProtect.TabIndex = 0x23;
            this.cbPassProtect.UseVisualStyleBackColor = true;
            this.cbPassProtect.Click += new EventHandler(this.cbPassProtect_Click);
            this.cbPassProtect.CheckedChanged += new EventHandler(this.cbPassProtect_CheckedChanged);
            base.AcceptButton = this.skinButtonHost;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1b1, 0x125);
            base.Controls.Add(this.cbPassProtect);
            base.Controls.Add(this.tbPassword);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.tbGameName);
            base.Controls.Add(this.lGameName);
            base.Controls.Add(this.flowLayoutPanel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x1b1, 0x125);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x1b1, 0x125);
            base.Name = "DlgHostGame";
            this.Text = "<LOC>New Game";
            base.Controls.SetChildIndex(this.flowLayoutPanel1, 0);
            base.Controls.SetChildIndex(this.lGameName, 0);
            base.Controls.SetChildIndex(this.tbGameName, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.tbPassword, 0);
            base.Controls.SetChildIndex(this.cbPassProtect, 0);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.gpgPanel1.ResumeLayout(false);
            this.tbGameName.Properties.EndInit();
            this.tbPassword.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tbGameName.Select();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void skinButtonHost_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            base.ClearErrors();
            if ((this.GameName == null) || (this.GameName.Trim().Length < 1))
            {
                base.Error(this.tbGameName, "<LOC>Game name may not be blank", new object[0]);
            }
            else if (!TextUtil.IsAlphaNumericString(this.GameName.Replace(" ", "")))
            {
                base.Error(this.tbGameName, "<LOC>Game name contains invalid characters. It may contain letters, numbers, dashes and underbars.", new object[0]);
            }
            else if (this.GameName.Contains("\""))
            {
                base.Error(this.tbGameName, "<LOC>Game name may not contain quotes (i.e. \" \")", new object[0]);
            }
            else if (Profanity.ContainsProfanity(this.GameName))
            {
                base.Error(this.tbGameName, "<LOC>Game names cannot contain profanity. Please enter a valid game name.", new object[0]);
            }
            else if (!TextUtil.IsAlphaNumericString(this.GamePassword))
            {
                base.Error(this.tbPassword, "<LOC>Game password contains invalid characters. It may contain letters, numbers, dashes and underbars.", new object[0]);
            }
            else if (this.GamePassword.Contains("\""))
            {
                base.Error(this.tbPassword, "<LOC>Game password may not contain quotes (i.e. \" \")", new object[0]);
            }
            else if (Profanity.ContainsProfanity(this.GamePassword))
            {
                base.Error(this.tbPassword, "<LOC>Game password cannot contain profanity. Please enter a valid password.", new object[0]);
            }
            else if (this.cbPassProtect.Checked && (this.tbPassword.Text == ""))
            {
                base.Error(this.tbPassword, "<LOC>The password cannot be blank.", new object[0]);
            }
            else
            {
                if (callBack == null)
                {
                    callBack = delegate (object o) {
                        DataList queryData = null;
                        VGen0 method = null;
                        VGen0 gen2 = null;
                        if (ConfigSettings.GetBool("DoOldGameList", false))
                        {
                            queryData = DataAccess.GetQueryData("GetGameByName", new object[] { this.tbGameName.Text });
                        }
                        else
                        {
                            queryData = DataAccess.GetQueryData("GetGameByName2", new object[] { this.tbGameName.Text, GameInformation.SelectedGame.GameID });
                        }
                        if (queryData.Count > 0)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    base.Error(this.tbGameName, "<LOC>This game name already exists.  Please choose another.", new object[0]);
                                };
                            }
                            base.Invoke(method);
                        }
                        else
                        {
                            if (gen2 == null)
                            {
                                gen2 = delegate {
                                    base.DialogResult = DialogResult.OK;
                                };
                            }
                            base.Invoke(gen2);
                        }
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
            }
        }

        public string GameName
        {
            get
            {
                return this.tbGameName.Text.Trim();
            }
        }

        public string GamePassword
        {
            get
            {
                return this.tbPassword.Text.Trim();
            }
        }
    }
}

