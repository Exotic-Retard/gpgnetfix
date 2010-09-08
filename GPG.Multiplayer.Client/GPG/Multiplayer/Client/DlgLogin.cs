namespace GPG.Multiplayer.Client
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;

    public class DlgLogin : DlgBase
    {
        public static bool _formLoading = false;
        private BarAndDockingController barAndDockingController1;
        private CheckBox cbAutlogin;
        private CheckBox checkBoxRemember;
        private IContainer components = null;
        public GPGDropDownList ddGame;
        public GPGDropDownList ddServer;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabelChangeAccount;
        private GPGLabel gpgLabelChangePwd;
        public GPGLabel gpgLabelCreateAcct;
        private GPGLabel gpgLabelLostPwd;
        private GPGPictureBox gpgPictureBox1;
        private GPGLabel labelError;
        private GPGLabel labelLogin;
        public static string LastPass = "";
        private GPGLabel lGame;
        private GPGLabel lLostUsername;
        public GPGLabel lManageGames;
        private ToolStripMenuItem lOCChangePasswordToolStripMenuItem;
        private ToolStripMenuItem lOCRegisterNewUserToolStripMenuItem;
        private ToolStripMenuItem lOCResetPasswordToolStripMenuItem;
        private bool LoggingIn = false;
        private System.Timers.Timer LoginTimer = new System.Timers.Timer(1000.0);
        private GPGLabel lServer;
        private bool mFirstTime = false;
        private ToolStripMenuItem miMainGroup;
        private GPGMenuStrip msMainMenu;
        private GPGPictureBox pbCurrentGame;
        private List<QuazalServerInfo> servers = QuazalServerInfo.GetServers();
        public static bool ShowCreate = false;
        public SkinButton skinButtonCancel;
        public SkinButton skinButtonOK;
        public GPGTextBox textBoxPassword;
        public GPGTextBox textBoxUsername;
        private ToolTipController toolTipController1;
        private int WaitSeconds = 0;

        public DlgLogin()
        {
            _formLoading = true;
            this.InitializeComponent();
            GameInformation.LoadCachedGames();
            base.DialogResult = DialogResult.Abort;
            this.checkBoxRemember.Checked = Program.Settings.Login.RememberCredentials;
            this.cbAutlogin.Checked = (Program.Settings.Login.AutoLogin && (Program.Settings.Login.DefaultUsername != "")) && (Program.Settings.Login.DefaultPassword != "");
            if (this.checkBoxRemember.Checked)
            {
                this.textBoxUsername.Text = Program.Settings.Login.DefaultUsername;
                this.textBoxPassword.Text = Program.Settings.Login.DefaultPassword;
            }
            else
            {
                Program.Settings.Login.DefaultUsername = null;
                Program.Settings.Login.DefaultPassword = null;
                this.textBoxUsername.Text = "";
            }
            this.cbAutlogin.DataBindings.Add("Visible", this.checkBoxRemember, "Checked");
            this.LoginTimer.AutoReset = true;
            this.LoginTimer.Elapsed += new ElapsedEventHandler(this.LoginTimer_Elapsed);
            Loc.LocObject(this);
            base.StyleMenu(this.msMainMenu);
            this.ddServer.Items.Clear();
            this.ddServer.SelectedItem = null;
            string defaultServerName = Program.Settings.Login.DefaultServerName;
            foreach (QuazalServerInfo info in this.servers)
            {
                this.ddServer.Items.Add(info);
                if (this.ddServer.SelectedItem == null)
                {
                    this.ddServer.SelectedItem = info;
                }
                if (info.Description == defaultServerName)
                {
                    this.ddServer.SelectedItem = info;
                }
                Program.RefreshMemUsage();
            }
            User.SetAccessKey(Program.Settings.Login.AccessKey);
            if (this.ddServer.Items.Count == 1)
            {
                this.lServer.Visible = false;
                this.ddServer.Visible = false;
                this.MinimumSize = new Size(this.MinimumSize.Width, base.Size.Height - 50);
                this.MaximumSize = this.MinimumSize;
            }
            this.CheckServer();
            this.CheckGames();
            this.CheckSelectedServer();
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                try
                {
                    new WebClient().DownloadFile("http://gpgnet.gaspowered.com/gpgnetemergency3.html", "emergency.html");
                    StreamReader reader = new StreamReader("emergency.html");
                    bool flag = reader.ReadToEnd().IndexOf("GPGnet message") >= 0;
                    reader.Close();
                    if (flag)
                    {
                        Process.Start("emergency.html");
                    }
                }
                catch (Exception)
                {
                }
            });
            _formLoading = false;
        }

        private void cbAutlogin_CheckedChanged(object sender, EventArgs e)
        {
            if (!(this.checkBoxRemember.Checked || !this.cbAutlogin.Checked))
            {
                this.cbAutlogin.Checked = false;
            }
            Program.Settings.Login.AutoLogin = this.cbAutlogin.Checked;
        }

        private void ChangePassword()
        {
            DlgChangePassword password = new DlgChangePassword();
            password.tbUsername.Text = this.textBoxUsername.Text;
            base.Visible = false;
            if (password.ShowDialog() == DialogResult.OK)
            {
                this.textBoxUsername.Text = password.tbUsername.Text;
                this.textBoxPassword.Text = "";
                base.Visible = true;
                DlgMessage.ShowDialog("<LOC>Your password has been successfully updated.", "<LOC>Success");
                this.LoggingIn = true;
                this.EndLogin(true);
            }
            else
            {
                base.Visible = true;
            }
        }

        private void CheckGames()
        {
            this.ddGame.Items.Clear();
            foreach (GameInformation information in GameInformation.Games)
            {
                if ((information.GameLocation != "") && (information.GameDescription.ToUpper() != "GPGNET"))
                {
                    this.ddGame.Items.Add(information);
                    if ((Program.Settings.Login.DefaultGameID == information.GameID) && System.IO.File.Exists(information.GameLocation))
                    {
                        this.ddGame.SelectedItem = information;
                    }
                }
            }
            this.ddGame.Invalidate();
            this.pbCurrentGame.Invalidate();
        }

        private void CheckSelectedServer()
        {
            bool flag = false;
            foreach (GameInformation information in GameInformation.Games)
            {
                if (information.IsSpaceSiege && (Program.Settings.Login.DefaultGameID == -1))
                {
                    foreach (QuazalServerInfo info in this.ddServer.Items)
                    {
                        if (info.Description == "Space Siege Server")
                        {
                            this.ddServer.SelectedItem = info;
                            flag = true;
                        }
                    }
                }
            }
            if (flag)
            {
                this.CheckServer();
            }
        }

        public void CheckServer()
        {
            QuazalServerInfo selectedItem = this.ddServer.SelectedItem as QuazalServerInfo;
            if (selectedItem != null)
            {
                Program.Settings.Login.DefaultServer = selectedItem.Address;
                Program.Settings.Login.DefaultPort = selectedItem.Port;
                Program.Settings.Login.DefaultServerName = selectedItem.Description;
                Program.Settings.Login.AccessKey = selectedItem.AccessKey;
                User.SetAccessKey(Program.Settings.Login.AccessKey);
            }
            else
            {
                this.ddServer.SelectedItem = this.servers[0];
                this.CheckServer();
            }
        }

        private void ddGame_SelectedValueChanged(object sender, EventArgs e)
        {
            GameInformation.SelectedGame = this.ddGame.SelectedItem as GameInformation;
            this.pbCurrentGame.Image = GameInformation.SelectedGame.GameIcon;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgLogin_Load(object sender, EventArgs e)
        {
        }

        private void DoLogin()
        {
            LastPass = this.textBoxPassword.Text;
            this.textBoxUsername.Enabled = false;
            this.textBoxPassword.Enabled = false;
            this.skinButtonOK.Enabled = false;
            this.LoggingIn = true;
            ThreadQueue.Quazal.Enqueue(typeof(User), "Login", this, "EndLogin", new object[] { this.textBoxUsername.Text, this.textBoxPassword.Text, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort });
            this.LoginTimer.Start();
            this.labelLogin.Visible = true;
            this.labelError.Visible = false;
        }

        private void EndLogin(bool result)
        {
            if (this.LoggingIn)
            {
                this.LoggingIn = false;
                this.LoginTimer.Stop();
                this.WaitSeconds = 0;
                this.labelLogin.Visible = false;
                if (result)
                {
                    if (this.checkBoxRemember.Checked)
                    {
                        Program.Settings.Login.DefaultUsername = this.textBoxUsername.Text;
                        Program.Settings.Login.DefaultPassword = this.textBoxPassword.Text;
                    }
                    else
                    {
                        Program.Settings.Login.DefaultUsername = null;
                        Program.Settings.Login.DefaultPassword = null;
                    }
                    Program.Settings.Login.RememberCredentials = this.checkBoxRemember.Checked;
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                else
                {
                    QuazalError error = QuazalErrorCodes.GetError();
                    if (error.errorcode != -1)
                    {
                        this.labelError.Text = error.message;
                    }
                    else
                    {
                        this.labelError.Text = Loc.Get("<LOC>Login failed, please verify username and password and try again.");
                    }
                    this.labelError.Visible = true;
                    this.textBoxUsername.Enabled = true;
                    this.textBoxPassword.Enabled = true;
                    this.skinButtonOK.Enabled = true;
                }
            }
        }

        private void gpgDropDownList1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!_formLoading)
            {
                this.CheckServer();
                this.ResetGames();
                this.CheckGames();
            }
        }

        private void gpgLabelChangeAccount_Click(object sender, EventArgs e)
        {
            base.Visible = false;
            if (new DlgChangeName().ShowDialog() == DialogResult.OK)
            {
                base.Close();
            }
            else
            {
                base.Visible = true;
            }
        }

        private void gpgLabelChangePwd_Click(object sender, EventArgs e)
        {
            this.ChangePassword();
        }

        private void gpgLabelCreateAcct_Click(object sender, EventArgs e)
        {
            this.RegisterUser();
        }

        private void gpgLabelLostPwd_Click(object sender, EventArgs e)
        {
            base.Hide();
            new DlgResetPwd().ShowDialog(this);
            Application.Restart();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgLogin));
            this.barAndDockingController1 = new BarAndDockingController(this.components);
            this.toolTipController1 = new ToolTipController(this.components);
            this.checkBoxRemember = new CheckBox();
            this.cbAutlogin = new CheckBox();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgLabelLostPwd = new GPGLabel();
            this.gpgLabelChangePwd = new GPGLabel();
            this.gpgLabelCreateAcct = new GPGLabel();
            this.ddServer = new GPGDropDownList();
            this.lServer = new GPGLabel();
            this.labelError = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.labelLogin = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.msMainMenu = new GPGMenuStrip(this.components);
            this.gpgLabelChangeAccount = new GPGLabel();
            this.lLostUsername = new GPGLabel();
            this.gpgPictureBox1 = new GPGPictureBox();
            this.ddGame = new GPGDropDownList();
            this.lGame = new GPGLabel();
            this.lManageGames = new GPGLabel();
            this.pbCurrentGame = new GPGPictureBox();
            this.miMainGroup = new ToolStripMenuItem();
            this.lOCRegisterNewUserToolStripMenuItem = new ToolStripMenuItem();
            this.lOCChangePasswordToolStripMenuItem = new ToolStripMenuItem();
            this.lOCResetPasswordToolStripMenuItem = new ToolStripMenuItem();
            this.textBoxPassword = new GPGTextBox();
            this.textBoxUsername = new GPGTextBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.barAndDockingController1.BeginInit();
            ((ISupportInitialize) this.gpgPictureBox1).BeginInit();
            ((ISupportInitialize) this.pbCurrentGame).BeginInit();
            this.textBoxPassword.Properties.BeginInit();
            this.textBoxUsername.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x12f, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            this.toolTipController1.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            this.checkBoxRemember.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxRemember.AutoSize = true;
            this.checkBoxRemember.Checked = true;
            this.checkBoxRemember.CheckState = CheckState.Checked;
            this.checkBoxRemember.Location = new Point(0x18, 0x13b);
            this.checkBoxRemember.Margin = new Padding(3, 4, 3, 4);
            this.checkBoxRemember.Name = "checkBoxRemember";
            this.checkBoxRemember.Size = new Size(0xe2, 0x11);
            this.toolTipController1.SetSuperTip(this.checkBoxRemember, null);
            base.ttDefault.SetSuperTip(this.checkBoxRemember, null);
            this.checkBoxRemember.TabIndex = 2;
            this.checkBoxRemember.Text = "<LOC>Remember my account info";
            this.checkBoxRemember.UseVisualStyleBackColor = true;
            this.cbAutlogin.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbAutlogin.AutoSize = true;
            this.cbAutlogin.Location = new Point(0x18, 0x14c);
            this.cbAutlogin.Margin = new Padding(3, 4, 3, 4);
            this.cbAutlogin.Name = "cbAutlogin";
            this.cbAutlogin.Size = new Size(0x115, 0x11);
            this.toolTipController1.SetSuperTip(this.cbAutlogin, null);
            base.ttDefault.SetSuperTip(this.cbAutlogin, null);
            this.cbAutlogin.TabIndex = 13;
            this.cbAutlogin.Text = "<LOC>Login automatically with this account";
            this.cbAutlogin.UseVisualStyleBackColor = true;
            this.cbAutlogin.CheckedChanged += new EventHandler(this.cbAutlogin_CheckedChanged);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawColor = Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x9f, 0x1c9);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5d, 0x1c);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.toolTipController1.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 0x16;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>Login";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
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
            this.skinButtonCancel.Location = new Point(0x101, 0x1c9);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5d, 0x1c);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.toolTipController1.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 0x17;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgLabelLostPwd.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelLostPwd.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelLostPwd.AutoSize = true;
            this.gpgLabelLostPwd.AutoStyle = true;
            this.gpgLabelLostPwd.Font = new Font("Arial", 9.75f);
            this.gpgLabelLostPwd.ForeColor = Color.White;
            this.gpgLabelLostPwd.IgnoreMouseWheel = false;
            this.gpgLabelLostPwd.IsStyled = false;
            this.gpgLabelLostPwd.Location = new Point(13, 0x18c);
            this.gpgLabelLostPwd.Name = "gpgLabelLostPwd";
            this.gpgLabelLostPwd.Size = new Size(170, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelLostPwd, null);
            this.toolTipController1.SetSuperTip(this.gpgLabelLostPwd, null);
            this.gpgLabelLostPwd.TabIndex = 0x15;
            this.gpgLabelLostPwd.Text = "<LOC>Lost your password?";
            this.gpgLabelLostPwd.TextStyle = TextStyles.Link;
            this.gpgLabelLostPwd.Click += new EventHandler(this.gpgLabelLostPwd_Click);
            this.gpgLabelChangePwd.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelChangePwd.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelChangePwd.AutoSize = true;
            this.gpgLabelChangePwd.AutoStyle = true;
            this.gpgLabelChangePwd.Font = new Font("Arial", 9.75f);
            this.gpgLabelChangePwd.ForeColor = Color.White;
            this.gpgLabelChangePwd.IgnoreMouseWheel = false;
            this.gpgLabelChangePwd.IsStyled = false;
            this.gpgLabelChangePwd.Location = new Point(13, 380);
            this.gpgLabelChangePwd.Name = "gpgLabelChangePwd";
            this.gpgLabelChangePwd.Size = new Size(0xb6, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelChangePwd, null);
            this.toolTipController1.SetSuperTip(this.gpgLabelChangePwd, null);
            this.gpgLabelChangePwd.TabIndex = 20;
            this.gpgLabelChangePwd.Text = "<LOC>Change your password";
            this.gpgLabelChangePwd.TextStyle = TextStyles.Link;
            this.gpgLabelChangePwd.Click += new EventHandler(this.gpgLabelChangePwd_Click);
            this.gpgLabelCreateAcct.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelCreateAcct.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelCreateAcct.AutoSize = true;
            this.gpgLabelCreateAcct.AutoStyle = true;
            this.gpgLabelCreateAcct.Font = new Font("Arial", 9.75f);
            this.gpgLabelCreateAcct.ForeColor = Color.White;
            this.gpgLabelCreateAcct.IgnoreMouseWheel = false;
            this.gpgLabelCreateAcct.IsStyled = false;
            this.gpgLabelCreateAcct.Location = new Point(13, 0x16c);
            this.gpgLabelCreateAcct.Name = "gpgLabelCreateAcct";
            this.gpgLabelCreateAcct.Size = new Size(0xb0, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelCreateAcct, null);
            this.toolTipController1.SetSuperTip(this.gpgLabelCreateAcct, null);
            this.gpgLabelCreateAcct.TabIndex = 0x13;
            this.gpgLabelCreateAcct.Text = "<LOC>Create a new account";
            this.gpgLabelCreateAcct.TextStyle = TextStyles.Link;
            this.gpgLabelCreateAcct.Click += new EventHandler(this.gpgLabelCreateAcct_Click);
            this.ddServer.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.ddServer.BackColor = Color.Black;
            this.ddServer.BorderColor = Color.Black;
            this.ddServer.DoValidate = true;
            this.ddServer.FlatStyle = FlatStyle.Flat;
            this.ddServer.FocusBackColor = Color.White;
            this.ddServer.FocusBorderColor = Color.White;
            this.ddServer.ForeColor = Color.White;
            this.ddServer.FormattingEnabled = true;
            this.ddServer.Items.AddRange(new object[] { "Beta Server", "GPG Test Server 1", "GPG Test Server 2", "Test Staging Server" });
            this.ddServer.Location = new Point(0x10, 0x8a);
            this.ddServer.Name = "ddServer";
            this.ddServer.Size = new Size(0x144, 0x15);
            base.ttDefault.SetSuperTip(this.ddServer, null);
            this.toolTipController1.SetSuperTip(this.ddServer, null);
            this.ddServer.TabIndex = 0x11;
            this.ddServer.SelectedValueChanged += new EventHandler(this.gpgDropDownList1_SelectedValueChanged);
            this.lServer.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lServer.AutoGrowDirection = GrowDirections.None;
            this.lServer.AutoSize = true;
            this.lServer.AutoStyle = true;
            this.lServer.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            this.lServer.ForeColor = Color.White;
            this.lServer.IgnoreMouseWheel = false;
            this.lServer.IsStyled = false;
            this.lServer.Location = new Point(13, 0x77);
            this.lServer.Name = "lServer";
            this.lServer.Size = new Size(0x5d, 0x10);
            base.ttDefault.SetSuperTip(this.lServer, null);
            this.toolTipController1.SetSuperTip(this.lServer, null);
            this.lServer.TabIndex = 0x10;
            this.lServer.Text = "<LOC>Server";
            this.lServer.TextStyle = TextStyles.Title;
            this.labelError.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.labelError.AutoGrowDirection = GrowDirections.None;
            this.labelError.AutoStyle = true;
            this.labelError.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelError.ForeColor = Color.Red;
            this.labelError.IgnoreMouseWheel = false;
            this.labelError.IsStyled = false;
            this.labelError.Location = new Point(0x12, 0x1d7);
            this.labelError.Name = "labelError";
            this.labelError.Size = new Size(0x142, 0x40);
            base.ttDefault.SetSuperTip(this.labelError, null);
            this.toolTipController1.SetSuperTip(this.labelError, null);
            this.labelError.TabIndex = 8;
            this.labelError.Text = "<LOC>This is a test error.  This is not usually visible.  It only happens when a login error occurs.";
            this.labelError.TextAlign = ContentAlignment.MiddleCenter;
            this.labelError.TextStyle = TextStyles.Error;
            this.labelError.Visible = false;
            this.gpgLabel1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(13, 0xd8);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x73, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.toolTipController1.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 1;
            this.gpgLabel1.Text = "<LOC>Username";
            this.gpgLabel1.TextStyle = TextStyles.Title;
            this.labelLogin.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.labelLogin.AutoGrowDirection = GrowDirections.None;
            this.labelLogin.AutoStyle = true;
            this.labelLogin.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelLogin.ForeColor = Color.White;
            this.labelLogin.IgnoreMouseWheel = false;
            this.labelLogin.IsStyled = false;
            this.labelLogin.Location = new Point(0x11, 0x1ec);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new Size(0x149, 0x15);
            base.ttDefault.SetSuperTip(this.labelLogin, null);
            this.toolTipController1.SetSuperTip(this.labelLogin, null);
            this.labelLogin.TabIndex = 7;
            this.labelLogin.Text = "<LOC>Logging In";
            this.labelLogin.TextAlign = ContentAlignment.TopCenter;
            this.labelLogin.TextStyle = TextStyles.Status;
            this.labelLogin.Visible = false;
            this.gpgLabel2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(13, 0x10a);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x70, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.toolTipController1.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 3;
            this.gpgLabel2.Text = "<LOC>Password";
            this.gpgLabel2.TextStyle = TextStyles.Title;
            this.msMainMenu.BackColor = Color.Transparent;
            this.msMainMenu.BackgroundImage = (Image) manager.GetObject("msMainMenu.BackgroundImage");
            this.msMainMenu.Dock = DockStyle.None;
            this.msMainMenu.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.msMainMenu.Location = new Point(0x31, 0x1b);
            this.msMainMenu.Name = "msMainMenu";
            this.msMainMenu.Padding = new Padding(0, 2, 0, 2);
            this.msMainMenu.Size = new Size(0xca, 0x18);
            this.toolTipController1.SetSuperTip(this.msMainMenu, null);
            base.ttDefault.SetSuperTip(this.msMainMenu, null);
            this.msMainMenu.TabIndex = 12;
            this.msMainMenu.Text = "menuStrip1";
            this.msMainMenu.Visible = false;
            this.gpgLabelChangeAccount.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelChangeAccount.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelChangeAccount.AutoSize = true;
            this.gpgLabelChangeAccount.AutoStyle = true;
            this.gpgLabelChangeAccount.Font = new Font("Arial", 9.75f);
            this.gpgLabelChangeAccount.ForeColor = Color.White;
            this.gpgLabelChangeAccount.IgnoreMouseWheel = false;
            this.gpgLabelChangeAccount.IsStyled = false;
            this.gpgLabelChangeAccount.Location = new Point(13, 0x19c);
            this.gpgLabelChangeAccount.Name = "gpgLabelChangeAccount";
            this.gpgLabelChangeAccount.Size = new Size(180, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelChangeAccount, null);
            this.toolTipController1.SetSuperTip(this.gpgLabelChangeAccount, null);
            this.gpgLabelChangeAccount.TabIndex = 0x18;
            this.gpgLabelChangeAccount.Text = "<LOC>Change account name";
            this.gpgLabelChangeAccount.TextStyle = TextStyles.Link;
            this.gpgLabelChangeAccount.Click += new EventHandler(this.gpgLabelChangeAccount_Click);
            this.lLostUsername.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.lLostUsername.AutoGrowDirection = GrowDirections.None;
            this.lLostUsername.AutoSize = true;
            this.lLostUsername.AutoStyle = true;
            this.lLostUsername.Font = new Font("Arial", 9.75f);
            this.lLostUsername.ForeColor = Color.White;
            this.lLostUsername.IgnoreMouseWheel = false;
            this.lLostUsername.IsStyled = false;
            this.lLostUsername.Location = new Point(13, 0x1ac);
            this.lLostUsername.Name = "lLostUsername";
            this.lLostUsername.Size = new Size(0xac, 0x10);
            base.ttDefault.SetSuperTip(this.lLostUsername, null);
            this.toolTipController1.SetSuperTip(this.lLostUsername, null);
            this.lLostUsername.TabIndex = 0x19;
            this.lLostUsername.Text = "<LOC>Lost your username?";
            this.lLostUsername.TextStyle = TextStyles.Link;
            this.lLostUsername.Click += new EventHandler(this.lLostUsername_Click);
            this.gpgPictureBox1.Image = Resources.gpgnetlogo;
            this.gpgPictureBox1.Location = new Point(0x41, 0x45);
            this.gpgPictureBox1.Name = "gpgPictureBox1";
            this.gpgPictureBox1.Size = new Size(0xeb, 0x2d);
            this.gpgPictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.gpgPictureBox1, null);
            this.toolTipController1.SetSuperTip(this.gpgPictureBox1, null);
            this.gpgPictureBox1.TabIndex = 0x1a;
            this.gpgPictureBox1.TabStop = false;
            this.ddGame.BackColor = Color.Black;
            this.ddGame.BorderColor = Color.Black;
            this.ddGame.DoValidate = true;
            this.ddGame.FlatStyle = FlatStyle.Flat;
            this.ddGame.FocusBackColor = Color.White;
            this.ddGame.FocusBorderColor = Color.White;
            this.ddGame.ForeColor = Color.White;
            this.ddGame.FormattingEnabled = true;
            this.ddGame.Items.AddRange(new object[] { "Supreme Commander" });
            this.ddGame.Location = new Point(0x10, 0xba);
            this.ddGame.Name = "ddGame";
            this.ddGame.Size = new Size(0x123, 0x15);
            base.ttDefault.SetSuperTip(this.ddGame, null);
            this.toolTipController1.SetSuperTip(this.ddGame, null);
            this.ddGame.TabIndex = 0x1c;
            this.ddGame.SelectedValueChanged += new EventHandler(this.ddGame_SelectedValueChanged);
            this.lGame.AutoGrowDirection = GrowDirections.None;
            this.lGame.AutoSize = true;
            this.lGame.AutoStyle = true;
            this.lGame.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            this.lGame.ForeColor = Color.White;
            this.lGame.IgnoreMouseWheel = false;
            this.lGame.IsStyled = false;
            this.lGame.Location = new Point(13, 0xa7);
            this.lGame.Name = "lGame";
            this.lGame.Size = new Size(0x59, 0x10);
            base.ttDefault.SetSuperTip(this.lGame, null);
            this.toolTipController1.SetSuperTip(this.lGame, null);
            this.lGame.TabIndex = 0x1b;
            this.lGame.Text = "<LOC>Game";
            this.lGame.TextStyle = TextStyles.Title;
            this.lManageGames.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lManageGames.AutoGrowDirection = GrowDirections.None;
            this.lManageGames.AutoStyle = true;
            this.lManageGames.Font = new Font("Arial", 9.75f);
            this.lManageGames.ForeColor = Color.White;
            this.lManageGames.IgnoreMouseWheel = false;
            this.lManageGames.IsStyled = false;
            this.lManageGames.Location = new Point(110, 0xa7);
            this.lManageGames.Name = "lManageGames";
            this.lManageGames.Size = new Size(0xc5, 0x10);
            this.toolTipController1.SetSuperTip(this.lManageGames, null);
            base.ttDefault.SetSuperTip(this.lManageGames, null);
            this.lManageGames.TabIndex = 0x1d;
            this.lManageGames.Text = "<LOC>Manage Games";
            this.lManageGames.TextAlign = ContentAlignment.TopRight;
            this.lManageGames.TextStyle = TextStyles.Link;
            this.lManageGames.Click += new EventHandler(this.lManageGames_Click);
            this.pbCurrentGame.Location = new Point(0x139, 0xae);
            this.pbCurrentGame.Name = "pbCurrentGame";
            this.pbCurrentGame.Size = new Size(0x20, 0x20);
            base.ttDefault.SetSuperTip(this.pbCurrentGame, null);
            this.toolTipController1.SetSuperTip(this.pbCurrentGame, null);
            this.pbCurrentGame.TabIndex = 30;
            this.pbCurrentGame.TabStop = false;
            this.miMainGroup.DropDownItems.AddRange(new ToolStripItem[] { this.lOCRegisterNewUserToolStripMenuItem, this.lOCChangePasswordToolStripMenuItem, this.lOCResetPasswordToolStripMenuItem });
            this.miMainGroup.Name = "miMainGroup";
            this.miMainGroup.Padding = new Padding(0, 0, 4, 0);
            this.miMainGroup.Size = new Size(0x6a, 20);
            this.miMainGroup.Text = "<LOC>Account";
            this.lOCRegisterNewUserToolStripMenuItem.Name = "lOCRegisterNewUserToolStripMenuItem";
            this.lOCRegisterNewUserToolStripMenuItem.Size = new Size(210, 0x16);
            this.lOCRegisterNewUserToolStripMenuItem.Text = "<LOC>Register New User";
            this.lOCRegisterNewUserToolStripMenuItem.Click += new EventHandler(this.lOCRegisterNewUserToolStripMenuItem_Click);
            this.lOCChangePasswordToolStripMenuItem.Name = "lOCChangePasswordToolStripMenuItem";
            this.lOCChangePasswordToolStripMenuItem.Size = new Size(210, 0x16);
            this.lOCChangePasswordToolStripMenuItem.Text = "<LOC>Change Password";
            this.lOCChangePasswordToolStripMenuItem.Click += new EventHandler(this.lOCChangePasswordToolStripMenuItem_Click);
            this.lOCResetPasswordToolStripMenuItem.Name = "lOCResetPasswordToolStripMenuItem";
            this.lOCResetPasswordToolStripMenuItem.Size = new Size(210, 0x16);
            this.lOCResetPasswordToolStripMenuItem.Text = "<LOC>Email Password";
            this.textBoxPassword.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.textBoxPassword.Location = new Point(0x10, 0x11c);
            this.textBoxPassword.Margin = new Padding(3, 4, 3, 4);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Properties.Appearance.BackColor = Color.Black;
            this.textBoxPassword.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.textBoxPassword.Properties.Appearance.ForeColor = Color.White;
            this.textBoxPassword.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxPassword.Properties.Appearance.Options.UseBorderColor = true;
            this.textBoxPassword.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxPassword.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.textBoxPassword.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(4, 6, 9);
            this.textBoxPassword.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.textBoxPassword.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.textBoxPassword.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.textBoxPassword.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.textBoxPassword.Properties.BorderStyle = BorderStyles.Simple;
            this.textBoxPassword.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.textBoxPassword.Properties.LookAndFeel.Style = LookAndFeelStyle.Office2003;
            this.textBoxPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.textBoxPassword.Properties.LookAndFeel.UseWindowsXPTheme = true;
            this.textBoxPassword.Properties.PasswordChar = '*';
            this.textBoxPassword.Size = new Size(0x144, 20);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxUsername.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.textBoxUsername.Location = new Point(0x10, 0xec);
            this.textBoxUsername.Margin = new Padding(3, 4, 3, 4);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Properties.Appearance.BackColor = Color.Black;
            this.textBoxUsername.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.textBoxUsername.Properties.Appearance.ForeColor = Color.White;
            this.textBoxUsername.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxUsername.Properties.Appearance.Options.UseBorderColor = true;
            this.textBoxUsername.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxUsername.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.textBoxUsername.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(4, 6, 9);
            this.textBoxUsername.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.textBoxUsername.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.textBoxUsername.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.textBoxUsername.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.textBoxUsername.Properties.BorderStyle = BorderStyles.Simple;
            this.textBoxUsername.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.textBoxUsername.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.textBoxUsername.Size = new Size(0x144, 20);
            this.textBoxUsername.TabIndex = 0;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.Black;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x16a, 0x23f);
            base.Controls.Add(this.pbCurrentGame);
            base.Controls.Add(this.lManageGames);
            base.Controls.Add(this.ddGame);
            base.Controls.Add(this.lGame);
            base.Controls.Add(this.gpgPictureBox1);
            base.Controls.Add(this.lLostUsername);
            base.Controls.Add(this.gpgLabelChangeAccount);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgLabelLostPwd);
            base.Controls.Add(this.gpgLabelChangePwd);
            base.Controls.Add(this.gpgLabelCreateAcct);
            base.Controls.Add(this.ddServer);
            base.Controls.Add(this.lServer);
            base.Controls.Add(this.cbAutlogin);
            base.Controls.Add(this.labelError);
            base.Controls.Add(this.textBoxPassword);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.labelLogin);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.textBoxUsername);
            base.Controls.Add(this.checkBoxRemember);
            base.Controls.Add(this.msMainMenu);
            this.DoubleBuffered = true;
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Margin = new Padding(3, 5, 3, 5);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x16a, 0x23f);
            this.MinimumSize = new Size(0x16a, 0x23f);
            base.Name = "DlgLogin";
            this.toolTipController1.SetSuperTip(this, null);
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Sign in to GPGnet: Supreme Commander";
            base.Load += new EventHandler(this.DlgLogin_Load);
            base.Controls.SetChildIndex(this.msMainMenu, 0);
            base.Controls.SetChildIndex(this.checkBoxRemember, 0);
            base.Controls.SetChildIndex(this.textBoxUsername, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.labelLogin, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.textBoxPassword, 0);
            base.Controls.SetChildIndex(this.labelError, 0);
            base.Controls.SetChildIndex(this.cbAutlogin, 0);
            base.Controls.SetChildIndex(this.lServer, 0);
            base.Controls.SetChildIndex(this.ddServer, 0);
            base.Controls.SetChildIndex(this.gpgLabelCreateAcct, 0);
            base.Controls.SetChildIndex(this.gpgLabelChangePwd, 0);
            base.Controls.SetChildIndex(this.gpgLabelLostPwd, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgLabelChangeAccount, 0);
            base.Controls.SetChildIndex(this.lLostUsername, 0);
            base.Controls.SetChildIndex(this.gpgPictureBox1, 0);
            base.Controls.SetChildIndex(this.lGame, 0);
            base.Controls.SetChildIndex(this.ddGame, 0);
            base.Controls.SetChildIndex(this.lManageGames, 0);
            base.Controls.SetChildIndex(this.pbCurrentGame, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.barAndDockingController1.EndInit();
            ((ISupportInitialize) this.gpgPictureBox1).EndInit();
            ((ISupportInitialize) this.pbCurrentGame).EndInit();
            this.textBoxPassword.Properties.EndInit();
            this.textBoxUsername.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lLostUsername_Click(object sender, EventArgs e)
        {
            base.Hide();
            new DlgEmailUsername().ShowDialog(this);
            Application.Restart();
        }

        private void lManageGames_Click(object sender, EventArgs e)
        {
            new DlgManageGames().ShowDialog();
            this.CheckGames();
        }

        private void lOCChangePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChangePassword();
        }

        private void lOCRegisterNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RegisterUser();
        }

        private void LoginTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GenericDelegate method = null;
            this.WaitSeconds++;
            string str = Loc.Get("<LOC>Logging In");
            for (int i = 0; i < this.WaitSeconds; i++)
            {
                str = str + ".";
            }
            if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    if (method == null)
                    {
                        method = delegate (object[] p) {
                            this.labelLogin.Text = (string) p[0];
                            return null;
                        };
                    }
                    base.BeginInvoke(method, new object[] { new object[] { str } });
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.textBoxUsername.Select();
            this.textBoxUsername.SelectionStart = this.textBoxUsername.Text.Length;
            if (FrmSplash.sFrmSplash != null)
            {
                FrmSplash.sFrmSplash.Close();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            base.Activate();
            base.Focus();
            if (ShowCreate)
            {
                this.RegisterUser();
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!this.mFirstTime && base.Visible)
            {
                this.mFirstTime = true;
                bool ignoreAutolaunch = Program.Settings.Login.IgnoreAutolaunch;
                Program.Settings.Login.IgnoreAutolaunch = false;
                if (!(ignoreAutolaunch || !Program.Settings.Login.AutoLogin))
                {
                    this.skinButtonOK.PerformClick();
                }
            }
        }

        private void RegisterUser()
        {
            DlgCreateUser user = new DlgCreateUser();
            user.tbUsername.Text = this.textBoxUsername.Text;
            base.Visible = false;
            if (user.ShowDialog() == DialogResult.OK)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.Visible = true;
            }
        }

        private void ResetGames()
        {
            GameInformation.Games.Clear();
            GameInformation.LoadCachedGames();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            if (this.LoggingIn)
            {
                this.LoggingIn = false;
                this.LoginTimer.Stop();
                this.WaitSeconds = 0;
                this.labelLogin.Visible = false;
                this.textBoxUsername.Enabled = true;
                this.textBoxPassword.Enabled = true;
                this.skinButtonOK.Enabled = true;
            }
            else
            {
                this.LoginTimer.Stop();
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if ((this.textBoxUsername.Text == null) || (this.textBoxUsername.Text.Length < 1))
            {
                base.Error(this.textBoxUsername, Loc.Get("<LOC>Your username cannot be blank."), new object[0]);
            }
            else if ((this.textBoxUsername.Text.Length < 3) && (this.textBoxUsername.Text.ToUpper() != "O"))
            {
                base.Error(this.textBoxUsername, Loc.Get("<LOC>Your username must be 3-22 characters in length."), new object[0]);
            }
            else if ((this.textBoxPassword.Text == null) || (this.textBoxPassword.Text.Length < 1))
            {
                base.Error(this.textBoxPassword, Loc.Get("<LOC>Your password cannot be blank."), new object[0]);
            }
            else if (this.textBoxPassword.Text.Length < 6)
            {
                base.Error(this.textBoxPassword, Loc.Get("<LOC>Password must be at least 6 characters long."), new object[0]);
            }
            else
            {
                this.DoLogin();
            }
        }
    }
}

