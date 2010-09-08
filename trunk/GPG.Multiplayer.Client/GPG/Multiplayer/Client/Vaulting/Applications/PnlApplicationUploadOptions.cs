namespace GPG.Multiplayer.Client.Vaulting.Applications
{
    using DevExpress.XtraEditors.Controls;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class PnlApplicationUploadOptions : PnlBase
    {
        private SkinButton btnAppDirectory;
        private SkinButton btnExeName;
        private SkinButton btnVersionFile;
        private GPGCheckBox cbAlreadyUploaded;
        private GPGCheckBox cbCustomLoc;
        private GPGCheckBox cbCustomPatch;
        private GPGCheckBox cbEncryptApp;
        private GPGCheckBox cbFilePatch;
        private GPGCheckBox cbRememberFTP;
        private IContainer components = null;
        public GPGDropDownList ddGame;
        private GPGCheckBox gpgCheckBox3;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel10;
        private GPGLabel gpgLabel11;
        private GPGLabel gpgLabel12;
        private GPGLabel gpgLabel13;
        private GPGLabel gpgLabel14;
        private GPGLabel gpgLabel15;
        private GPGLabel gpgLabel16;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGTextBox gpgTextBox2;
        private GPGLabel lMD5;
        private CustomApplication mApp = null;
        private bool mLoadingFTP = false;
        private GPGBorderPanel pCustomLoc;
        private GPGBorderPanel pPatching;
        private SkinButton skinButton2;
        private GPGTextBox tbDirectory;
        private GPGTextBox tbDownloadURL;
        private GPGTextBox tbEncryptionKey;
        private GPGTextBox tbExeName;
        private GPGTextBox tbFriendlyVersion;
        private GPGTextBox tbPassword;
        private GPGTextBox tbUploadDirectory;
        private GPGTextBox tbUploadSite;
        private GPGTextBox tbUsername;
        private GPGTextBox tbVersionFile;

        public PnlApplicationUploadOptions(CustomApplication app)
        {
            this.InitializeComponent();
            this.mApp = app;
            this.tbVersionFile.TextChanged += new EventHandler(this.tbVersionFile_TextChanged);
            this.tbVersionFile.Text = this.mApp.LocalFilePath;
            this.tbExeName.Text = this.mApp.LocalFilePath;
            this.tbDirectory.Text = new FileInfo(this.mApp.LocalFilePath).DirectoryName;
            this.ddGame.Items.Clear();
            this.ddGame.Items.AddRange(GameInformation.Games.ToArray());
            this.mApp.GameInfo = this.ddGame.SelectedValue as GameInformation;
            this.LoadFTPInfo();
        }

        private void btnAppDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = this.tbDirectory.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbDirectory.Text = dialog.SelectedPath;
                this.mApp.LocalUploadDir = this.tbDirectory.Text;
            }
        }

        private void btnExeName_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Executables|*.exe";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbExeName.Text = dialog.FileName;
            }
        }

        private void btnVersionFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All Files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.tbVersionFile.Text = dialog.FileName;
            }
        }

        private void cbCustomLoc_CheckStateChanged(object sender, EventArgs e)
        {
            this.pCustomLoc.Enabled = this.cbCustomLoc.Checked;
            this.mApp.IsFTPUpload = this.cbCustomLoc.Checked;
        }

        private void cbCustomPatch_CheckStateChanged(object sender, EventArgs e)
        {
            this.cbFilePatch.Checked = false;
            this.pPatching.Enabled = this.cbCustomPatch.Checked;
            if (this.cbFilePatch.Checked)
            {
                this.mApp.VersionKind = "PATCH";
            }
            else
            {
                this.mApp.VersionKind = "INSTALL";
            }
        }

        private void cbEncryptApp_CheckStateChanged(object sender, EventArgs e)
        {
            this.SetEncryption();
        }

        private void cbFilePatch_CheckStateChanged(object sender, EventArgs e)
        {
            this.cbCustomPatch.Checked = false;
            this.pPatching.Enabled = this.cbFilePatch.Checked;
            if (this.cbFilePatch.Checked)
            {
                this.mApp.VersionKind = "FILE";
            }
            else
            {
                this.mApp.VersionKind = "INSTALL";
            }
        }

        private void cbRememberFTP_CheckedChanged(object sender, EventArgs e)
        {
            this.SaveFTPInfo();
        }

        private void ddGame_SelectedValueChanged(object sender, EventArgs e)
        {
            this.mApp.GameInfo = this.ddGame.SelectedValue as GameInformation;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetFTPFile()
        {
            return (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "gpgnetftpinfo.data");
        }

        private string GetRelPath(string sourcedir, string targetdir)
        {
            int num2;
            string[] strArray = sourcedir.Split(new char[] { '\\' });
            string[] strArray2 = targetdir.Split(new char[] { '\\' });
            string str = "";
            int num = 0;
            for (num2 = 0; num2 < strArray.Length; num2++)
            {
                if (num2 < strArray2.Length)
                {
                    if (strArray2[num2] == strArray[num2])
                    {
                        num = num2;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            num2 = num;
            while (num2 < (strArray.Length - 1))
            {
                str = str + "../";
                num2++;
            }
            for (num2 = num; num2 < strArray2.Length; num2++)
            {
                str = str + strArray2[num2];
                if (num2 < (strArray2.Length - 1))
                {
                    str = str + "/";
                }
            }
            return str;
        }

        private void gpgCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            this.SaveFTPInfo();
        }

        private void InitializeComponent()
        {
            this.tbVersionFile = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.btnVersionFile = new SkinButton();
            this.lMD5 = new GPGLabel();
            this.btnAppDirectory = new SkinButton();
            this.tbDirectory = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.cbEncryptApp = new GPGCheckBox();
            this.gpgLabel3 = new GPGLabel();
            this.tbEncryptionKey = new GPGTextBox();
            this.cbCustomLoc = new GPGCheckBox();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.tbFriendlyVersion = new GPGTextBox();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.tbUploadSite = new GPGTextBox();
            this.tbUploadDirectory = new GPGTextBox();
            this.tbPassword = new GPGTextBox();
            this.tbUsername = new GPGTextBox();
            this.gpgLabel10 = new GPGLabel();
            this.gpgLabel11 = new GPGLabel();
            this.tbDownloadURL = new GPGTextBox();
            this.gpgLabel12 = new GPGLabel();
            this.cbRememberFTP = new GPGCheckBox();
            this.skinButton2 = new SkinButton();
            this.gpgTextBox2 = new GPGTextBox();
            this.gpgLabel14 = new GPGLabel();
            this.cbCustomPatch = new GPGCheckBox();
            this.gpgCheckBox3 = new GPGCheckBox();
            this.gpgLabel15 = new GPGLabel();
            this.gpgLabel16 = new GPGLabel();
            this.pPatching = new GPGBorderPanel();
            this.gpgLabel5 = new GPGLabel();
            this.ddGame = new GPGDropDownList();
            this.pCustomLoc = new GPGBorderPanel();
            this.cbAlreadyUploaded = new GPGCheckBox();
            this.cbFilePatch = new GPGCheckBox();
            this.btnExeName = new SkinButton();
            this.tbExeName = new GPGTextBox();
            this.gpgLabel13 = new GPGLabel();
            this.tbVersionFile.Properties.BeginInit();
            this.tbDirectory.Properties.BeginInit();
            this.tbEncryptionKey.Properties.BeginInit();
            this.tbFriendlyVersion.Properties.BeginInit();
            this.tbUploadSite.Properties.BeginInit();
            this.tbUploadDirectory.Properties.BeginInit();
            this.tbPassword.Properties.BeginInit();
            this.tbUsername.Properties.BeginInit();
            this.tbDownloadURL.Properties.BeginInit();
            this.gpgTextBox2.Properties.BeginInit();
            this.pPatching.SuspendLayout();
            this.pCustomLoc.SuspendLayout();
            this.tbExeName.Properties.BeginInit();
            base.SuspendLayout();
            this.tbVersionFile.Location = new Point(3, 0x49);
            this.tbVersionFile.Name = "tbVersionFile";
            this.tbVersionFile.Properties.Appearance.BackColor = Color.Black;
            this.tbVersionFile.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbVersionFile.Properties.Appearance.ForeColor = Color.White;
            this.tbVersionFile.Properties.Appearance.Options.UseBackColor = true;
            this.tbVersionFile.Properties.Appearance.Options.UseBorderColor = true;
            this.tbVersionFile.Properties.Appearance.Options.UseForeColor = true;
            this.tbVersionFile.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbVersionFile.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbVersionFile.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbVersionFile.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbVersionFile.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbVersionFile.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbVersionFile.Properties.BorderStyle = BorderStyles.Simple;
            this.tbVersionFile.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbVersionFile.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbVersionFile.Properties.MaxLength = 0xff;
            this.tbVersionFile.Properties.ReadOnly = true;
            this.tbVersionFile.Size = new Size(0x27e, 20);
            this.tbVersionFile.TabIndex = 0x17;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(3, 0x35);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x29b, 0x11);
            this.gpgLabel2.TabIndex = 0x16;
            this.gpgLabel2.Text = "<LOC>Application Version File";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.btnVersionFile.AutoStyle = true;
            this.btnVersionFile.BackColor = Color.Transparent;
            this.btnVersionFile.ButtonState = 0;
            this.btnVersionFile.DialogResult = DialogResult.OK;
            this.btnVersionFile.DisabledForecolor = Color.Gray;
            this.btnVersionFile.DrawColor = Color.White;
            this.btnVersionFile.DrawEdges = true;
            this.btnVersionFile.FocusColor = Color.Yellow;
            this.btnVersionFile.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnVersionFile.ForeColor = Color.White;
            this.btnVersionFile.HorizontalScalingMode = ScalingModes.Tile;
            this.btnVersionFile.IsStyled = true;
            this.btnVersionFile.Location = new Point(0x285, 0x49);
            this.btnVersionFile.Name = "btnVersionFile";
            this.btnVersionFile.Size = new Size(20, 20);
            this.btnVersionFile.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnVersionFile.TabIndex = 0x1c;
            this.btnVersionFile.TabStop = true;
            this.btnVersionFile.Text = "...";
            this.btnVersionFile.TextAlign = ContentAlignment.MiddleCenter;
            this.btnVersionFile.TextPadding = new Padding(0);
            this.btnVersionFile.Click += new EventHandler(this.btnVersionFile_Click);
            this.lMD5.AutoGrowDirection = GrowDirections.None;
            this.lMD5.AutoSize = true;
            this.lMD5.AutoStyle = true;
            this.lMD5.Font = new Font("Arial", 9.75f);
            this.lMD5.ForeColor = Color.White;
            this.lMD5.IgnoreMouseWheel = false;
            this.lMD5.IsStyled = false;
            this.lMD5.Location = new Point(2, 0x7a);
            this.lMD5.Name = "lMD5";
            this.lMD5.Size = new Size(12, 0x10);
            this.lMD5.TabIndex = 0x1d;
            this.lMD5.Text = ".";
            this.lMD5.TextStyle = TextStyles.Default;
            this.btnAppDirectory.AutoStyle = true;
            this.btnAppDirectory.BackColor = Color.Transparent;
            this.btnAppDirectory.ButtonState = 0;
            this.btnAppDirectory.DialogResult = DialogResult.OK;
            this.btnAppDirectory.DisabledForecolor = Color.Gray;
            this.btnAppDirectory.DrawColor = Color.White;
            this.btnAppDirectory.DrawEdges = true;
            this.btnAppDirectory.FocusColor = Color.Yellow;
            this.btnAppDirectory.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnAppDirectory.ForeColor = Color.White;
            this.btnAppDirectory.HorizontalScalingMode = ScalingModes.Tile;
            this.btnAppDirectory.IsStyled = true;
            this.btnAppDirectory.Location = new Point(0x285, 0xaf);
            this.btnAppDirectory.Name = "btnAppDirectory";
            this.btnAppDirectory.Size = new Size(20, 20);
            this.btnAppDirectory.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnAppDirectory.TabIndex = 0x20;
            this.btnAppDirectory.TabStop = true;
            this.btnAppDirectory.Text = "...";
            this.btnAppDirectory.TextAlign = ContentAlignment.MiddleCenter;
            this.btnAppDirectory.TextPadding = new Padding(0);
            this.btnAppDirectory.Click += new EventHandler(this.btnAppDirectory_Click);
            this.tbDirectory.Location = new Point(3, 0xaf);
            this.tbDirectory.Name = "tbDirectory";
            this.tbDirectory.Properties.Appearance.BackColor = Color.Black;
            this.tbDirectory.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbDirectory.Properties.Appearance.ForeColor = Color.White;
            this.tbDirectory.Properties.Appearance.Options.UseBackColor = true;
            this.tbDirectory.Properties.Appearance.Options.UseBorderColor = true;
            this.tbDirectory.Properties.Appearance.Options.UseForeColor = true;
            this.tbDirectory.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbDirectory.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbDirectory.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbDirectory.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbDirectory.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbDirectory.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbDirectory.Properties.BorderStyle = BorderStyles.Simple;
            this.tbDirectory.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbDirectory.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbDirectory.Properties.MaxLength = 0xff;
            this.tbDirectory.Properties.ReadOnly = true;
            this.tbDirectory.Size = new Size(0x27e, 20);
            this.tbDirectory.TabIndex = 0x1f;
            this.tbDirectory.TextChanged += new EventHandler(this.tbDirectory_TextChanged);
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(3, 0x9b);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x29d, 0x11);
            this.gpgLabel1.TabIndex = 30;
            this.gpgLabel1.Text = "<LOC>Application Directory";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.cbEncryptApp.AutoSize = true;
            this.cbEncryptApp.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.cbEncryptApp.ForeColor = Color.White;
            this.cbEncryptApp.Location = new Point(5, 0xe5);
            this.cbEncryptApp.Name = "cbEncryptApp";
            this.cbEncryptApp.Size = new Size(0x75, 0x11);
            this.cbEncryptApp.TabIndex = 0x21;
            this.cbEncryptApp.Text = "Encrypt Application";
            this.cbEncryptApp.UsesBG = false;
            this.cbEncryptApp.UseVisualStyleBackColor = false;
            this.cbEncryptApp.CheckStateChanged += new EventHandler(this.cbEncryptApp_CheckStateChanged);
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(3, 0xcf);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x10b, 0x11);
            this.gpgLabel3.TabIndex = 0x22;
            this.gpgLabel3.Text = "<LOC>Application Protection";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.tbEncryptionKey.Location = new Point(270, 0xe3);
            this.tbEncryptionKey.Name = "tbEncryptionKey";
            this.tbEncryptionKey.Properties.Appearance.BackColor = Color.Black;
            this.tbEncryptionKey.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbEncryptionKey.Properties.Appearance.ForeColor = Color.White;
            this.tbEncryptionKey.Properties.Appearance.Options.UseBackColor = true;
            this.tbEncryptionKey.Properties.Appearance.Options.UseBorderColor = true;
            this.tbEncryptionKey.Properties.Appearance.Options.UseForeColor = true;
            this.tbEncryptionKey.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbEncryptionKey.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbEncryptionKey.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbEncryptionKey.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbEncryptionKey.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbEncryptionKey.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbEncryptionKey.Properties.BorderStyle = BorderStyles.Simple;
            this.tbEncryptionKey.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbEncryptionKey.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbEncryptionKey.Properties.MaxLength = 0xff;
            this.tbEncryptionKey.Size = new Size(0x18d, 20);
            this.tbEncryptionKey.TabIndex = 0x23;
            this.tbEncryptionKey.TextChanged += new EventHandler(this.tbEncryptionKey_TextChanged);
            this.cbCustomLoc.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.cbCustomLoc.ForeColor = Color.White;
            this.cbCustomLoc.Location = new Point(5, 0x1b5);
            this.cbCustomLoc.Name = "cbCustomLoc";
            this.cbCustomLoc.Size = new Size(0x299, 0x11);
            this.cbCustomLoc.TabIndex = 0x26;
            this.cbCustomLoc.Text = "<LOC>Use Custom HTTP / FTP Info (For Non Vault Items)";
            this.cbCustomLoc.UsesBG = false;
            this.cbCustomLoc.UseVisualStyleBackColor = false;
            this.cbCustomLoc.CheckStateChanged += new EventHandler(this.cbCustomLoc_CheckStateChanged);
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel4.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x110, 0xcf);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(400, 0x11);
            this.gpgLabel4.TabIndex = 0x27;
            this.gpgLabel4.Text = "<LOC>Encryption Key / Password";
            this.gpgLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel4.TextStyle = TextStyles.Custom;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(3, 0x69);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x10b, 0x11);
            this.gpgLabel6.TabIndex = 40;
            this.gpgLabel6.Text = "<LOC>Version MD5";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel7.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(270, 0x69);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(400, 0x11);
            this.gpgLabel7.TabIndex = 0x29;
            this.gpgLabel7.Text = "<LOC>Friendly Version";
            this.gpgLabel7.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel7.TextStyle = TextStyles.Custom;
            this.tbFriendlyVersion.Location = new Point(270, 0x7d);
            this.tbFriendlyVersion.Name = "tbFriendlyVersion";
            this.tbFriendlyVersion.Properties.Appearance.BackColor = Color.Black;
            this.tbFriendlyVersion.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbFriendlyVersion.Properties.Appearance.ForeColor = Color.White;
            this.tbFriendlyVersion.Properties.Appearance.Options.UseBackColor = true;
            this.tbFriendlyVersion.Properties.Appearance.Options.UseBorderColor = true;
            this.tbFriendlyVersion.Properties.Appearance.Options.UseForeColor = true;
            this.tbFriendlyVersion.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbFriendlyVersion.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbFriendlyVersion.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbFriendlyVersion.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbFriendlyVersion.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbFriendlyVersion.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbFriendlyVersion.Properties.BorderStyle = BorderStyles.Simple;
            this.tbFriendlyVersion.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbFriendlyVersion.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbFriendlyVersion.Properties.MaxLength = 0xff;
            this.tbFriendlyVersion.Size = new Size(0x18d, 20);
            this.tbFriendlyVersion.TabIndex = 0x2a;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel8.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x114, 10);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x182, 0x11);
            this.gpgLabel8.TabIndex = 0x2c;
            this.gpgLabel8.Text = "<LOC>FTP Upload Directory";
            this.gpgLabel8.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel8.TextStyle = TextStyles.Custom;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel9.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(7, 10);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x10b, 0x11);
            this.gpgLabel9.TabIndex = 0x2b;
            this.gpgLabel9.Text = "<LOC>FTP Upload Site";
            this.gpgLabel9.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel9.TextStyle = TextStyles.Custom;
            this.tbUploadSite.Location = new Point(9, 30);
            this.tbUploadSite.Name = "tbUploadSite";
            this.tbUploadSite.Properties.Appearance.BackColor = Color.Black;
            this.tbUploadSite.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbUploadSite.Properties.Appearance.ForeColor = Color.White;
            this.tbUploadSite.Properties.Appearance.Options.UseBackColor = true;
            this.tbUploadSite.Properties.Appearance.Options.UseBorderColor = true;
            this.tbUploadSite.Properties.Appearance.Options.UseForeColor = true;
            this.tbUploadSite.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbUploadSite.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbUploadSite.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbUploadSite.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbUploadSite.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbUploadSite.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbUploadSite.Properties.BorderStyle = BorderStyles.Simple;
            this.tbUploadSite.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbUploadSite.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbUploadSite.Properties.MaxLength = 0xff;
            this.tbUploadSite.Size = new Size(0x109, 20);
            this.tbUploadSite.TabIndex = 0x2d;
            this.tbUploadSite.TextChanged += new EventHandler(this.tbUploadSite_TextChanged);
            this.tbUploadDirectory.Location = new Point(0x114, 30);
            this.tbUploadDirectory.Name = "tbUploadDirectory";
            this.tbUploadDirectory.Properties.Appearance.BackColor = Color.Black;
            this.tbUploadDirectory.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbUploadDirectory.Properties.Appearance.ForeColor = Color.White;
            this.tbUploadDirectory.Properties.Appearance.Options.UseBackColor = true;
            this.tbUploadDirectory.Properties.Appearance.Options.UseBorderColor = true;
            this.tbUploadDirectory.Properties.Appearance.Options.UseForeColor = true;
            this.tbUploadDirectory.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbUploadDirectory.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbUploadDirectory.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbUploadDirectory.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbUploadDirectory.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbUploadDirectory.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbUploadDirectory.Properties.BorderStyle = BorderStyles.Simple;
            this.tbUploadDirectory.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbUploadDirectory.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbUploadDirectory.Properties.MaxLength = 0xff;
            this.tbUploadDirectory.Size = new Size(0x182, 20);
            this.tbUploadDirectory.TabIndex = 0x2e;
            this.tbUploadDirectory.TextChanged += new EventHandler(this.tbUploadDirectory_TextChanged);
            this.tbPassword.Location = new Point(0x114, 80);
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
            this.tbPassword.Properties.MaxLength = 0xff;
            this.tbPassword.Properties.PasswordChar = '*';
            this.tbPassword.Size = new Size(0x182, 20);
            this.tbPassword.TabIndex = 50;
            this.tbPassword.TextChanged += new EventHandler(this.tbPassword_TextChanged);
            this.tbUsername.Location = new Point(9, 80);
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
            this.tbUsername.Properties.MaxLength = 0xff;
            this.tbUsername.Size = new Size(0x109, 20);
            this.tbUsername.TabIndex = 0x31;
            this.tbUsername.TextChanged += new EventHandler(this.tbUsername_TextChanged);
            this.gpgLabel10.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel10.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(0x114, 60);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0x182, 0x11);
            this.gpgLabel10.TabIndex = 0x30;
            this.gpgLabel10.Text = "<LOC>FTP Password";
            this.gpgLabel10.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel10.TextStyle = TextStyles.Custom;
            this.gpgLabel11.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel11.AutoStyle = true;
            this.gpgLabel11.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel11.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel11.ForeColor = Color.White;
            this.gpgLabel11.IgnoreMouseWheel = false;
            this.gpgLabel11.IsStyled = false;
            this.gpgLabel11.Location = new Point(7, 60);
            this.gpgLabel11.Name = "gpgLabel11";
            this.gpgLabel11.Size = new Size(0x10b, 0x11);
            this.gpgLabel11.TabIndex = 0x2f;
            this.gpgLabel11.Text = "<LOC>FTP Username";
            this.gpgLabel11.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel11.TextStyle = TextStyles.Custom;
            this.tbDownloadURL.Location = new Point(12, 0xa3);
            this.tbDownloadURL.Name = "tbDownloadURL";
            this.tbDownloadURL.Properties.Appearance.BackColor = Color.Black;
            this.tbDownloadURL.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbDownloadURL.Properties.Appearance.ForeColor = Color.White;
            this.tbDownloadURL.Properties.Appearance.Options.UseBackColor = true;
            this.tbDownloadURL.Properties.Appearance.Options.UseBorderColor = true;
            this.tbDownloadURL.Properties.Appearance.Options.UseForeColor = true;
            this.tbDownloadURL.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbDownloadURL.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbDownloadURL.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbDownloadURL.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbDownloadURL.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbDownloadURL.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbDownloadURL.Properties.BorderStyle = BorderStyles.Simple;
            this.tbDownloadURL.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbDownloadURL.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbDownloadURL.Properties.MaxLength = 0xff;
            this.tbDownloadURL.Size = new Size(650, 20);
            this.tbDownloadURL.TabIndex = 0x34;
            this.tbDownloadURL.TextChanged += new EventHandler(this.tbDownloadURL_TextChanged);
            this.gpgLabel12.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel12.AutoStyle = true;
            this.gpgLabel12.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel12.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel12.ForeColor = Color.White;
            this.gpgLabel12.IgnoreMouseWheel = false;
            this.gpgLabel12.IsStyled = false;
            this.gpgLabel12.Location = new Point(10, 0x8f);
            this.gpgLabel12.Name = "gpgLabel12";
            this.gpgLabel12.Size = new Size(0x28c, 0x11);
            this.gpgLabel12.TabIndex = 0x33;
            this.gpgLabel12.Text = "<LOC>HTTP Download URL";
            this.gpgLabel12.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel12.TextStyle = TextStyles.Custom;
            this.cbRememberFTP.AutoSize = true;
            this.cbRememberFTP.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.cbRememberFTP.ForeColor = Color.White;
            this.cbRememberFTP.Location = new Point(12, 0xbd);
            this.cbRememberFTP.Name = "cbRememberFTP";
            this.cbRememberFTP.Size = new Size(0x115, 0x11);
            this.cbRememberFTP.TabIndex = 0x35;
            this.cbRememberFTP.Text = "<LOC>Remember Info (Saved to usr.gpg in plain text)";
            this.cbRememberFTP.UsesBG = false;
            this.cbRememberFTP.UseVisualStyleBackColor = false;
            this.cbRememberFTP.CheckedChanged += new EventHandler(this.cbRememberFTP_CheckedChanged);
            this.skinButton2.AutoStyle = true;
            this.skinButton2.BackColor = Color.Transparent;
            this.skinButton2.ButtonState = 0;
            this.skinButton2.DialogResult = DialogResult.OK;
            this.skinButton2.DisabledForecolor = Color.Gray;
            this.skinButton2.DrawColor = Color.White;
            this.skinButton2.DrawEdges = true;
            this.skinButton2.FocusColor = Color.Yellow;
            this.skinButton2.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButton2.ForeColor = Color.White;
            this.skinButton2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButton2.IsStyled = true;
            this.skinButton2.Location = new Point(0x282, 0x21);
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.Size = new Size(20, 20);
            this.skinButton2.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButton2.TabIndex = 0x3b;
            this.skinButton2.TabStop = true;
            this.skinButton2.Text = "...";
            this.skinButton2.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButton2.TextPadding = new Padding(0);
            this.gpgTextBox2.Location = new Point(9, 0x21);
            this.gpgTextBox2.Name = "gpgTextBox2";
            this.gpgTextBox2.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBox2.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBox2.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBox2.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBox2.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBox2.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBox2.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBox2.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBox2.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBox2.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBox2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBox2.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBox2.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBox2.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBox2.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBox2.Properties.MaxLength = 0xff;
            this.gpgTextBox2.Size = new Size(0x275, 20);
            this.gpgTextBox2.TabIndex = 0x3a;
            this.gpgLabel14.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel14.AutoStyle = true;
            this.gpgLabel14.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel14.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel14.ForeColor = Color.White;
            this.gpgLabel14.IgnoreMouseWheel = false;
            this.gpgLabel14.IsStyled = false;
            this.gpgLabel14.Location = new Point(7, 13);
            this.gpgLabel14.Name = "gpgLabel14";
            this.gpgLabel14.Size = new Size(0x28f, 0x11);
            this.gpgLabel14.TabIndex = 0x39;
            this.gpgLabel14.Text = "<LOC>Custom Patch File";
            this.gpgLabel14.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel14.TextStyle = TextStyles.Custom;
            this.cbCustomPatch.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.cbCustomPatch.ForeColor = Color.White;
            this.cbCustomPatch.Location = new Point(5, 0xfd);
            this.cbCustomPatch.Name = "cbCustomPatch";
            this.cbCustomPatch.Size = new Size(0x109, 0x11);
            this.cbCustomPatch.TabIndex = 60;
            this.cbCustomPatch.Text = "<LOC>This version is a patch";
            this.cbCustomPatch.UsesBG = false;
            this.cbCustomPatch.UseVisualStyleBackColor = false;
            this.cbCustomPatch.CheckStateChanged += new EventHandler(this.cbCustomPatch_CheckStateChanged);
            this.gpgCheckBox3.AutoSize = true;
            this.gpgCheckBox3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgCheckBox3.ForeColor = Color.White;
            this.gpgCheckBox3.Location = new Point(0x10d, 0x3b);
            this.gpgCheckBox3.Name = "gpgCheckBox3";
            this.gpgCheckBox3.Size = new Size(0x139, 0x11);
            this.gpgCheckBox3.TabIndex = 0x3d;
            this.gpgCheckBox3.Text = "<LOC>Automatically Generate Patch using GPGnet algorithm";
            this.gpgCheckBox3.UsesBG = false;
            this.gpgCheckBox3.UseVisualStyleBackColor = false;
            this.gpgLabel15.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel15.AutoStyle = true;
            this.gpgLabel15.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel15.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel15.ForeColor = Color.White;
            this.gpgLabel15.IgnoreMouseWheel = false;
            this.gpgLabel15.IsStyled = false;
            this.gpgLabel15.Location = new Point(7, 0x3b);
            this.gpgLabel15.Name = "gpgLabel15";
            this.gpgLabel15.Size = new Size(260, 0x11);
            this.gpgLabel15.TabIndex = 0x3f;
            this.gpgLabel15.Text = "<LOC>Patch MD5";
            this.gpgLabel15.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel15.TextStyle = TextStyles.Custom;
            this.gpgLabel16.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel16.AutoSize = true;
            this.gpgLabel16.AutoStyle = true;
            this.gpgLabel16.Font = new Font("Arial", 9.75f);
            this.gpgLabel16.ForeColor = Color.White;
            this.gpgLabel16.IgnoreMouseWheel = false;
            this.gpgLabel16.IsStyled = false;
            this.gpgLabel16.Location = new Point(6, 0x51);
            this.gpgLabel16.Name = "gpgLabel16";
            this.gpgLabel16.Size = new Size(12, 0x10);
            this.gpgLabel16.TabIndex = 0x3e;
            this.gpgLabel16.Text = ".";
            this.gpgLabel16.TextStyle = TextStyles.Default;
            this.pPatching.Controls.Add(this.gpgLabel5);
            this.pPatching.Controls.Add(this.ddGame);
            this.pPatching.Controls.Add(this.gpgLabel14);
            this.pPatching.Controls.Add(this.gpgLabel15);
            this.pPatching.Controls.Add(this.gpgTextBox2);
            this.pPatching.Controls.Add(this.gpgLabel16);
            this.pPatching.Controls.Add(this.skinButton2);
            this.pPatching.Controls.Add(this.gpgCheckBox3);
            this.pPatching.Enabled = false;
            this.pPatching.GPGBorderStyle = GPGBorderStyle.Web;
            this.pPatching.Location = new Point(3, 0x114);
            this.pPatching.Name = "pPatching";
            this.pPatching.Size = new Size(0x29b, 0x9b);
            this.pPatching.TabIndex = 0x40;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel5.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(7, 0x65);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x28f, 0x11);
            this.gpgLabel5.TabIndex = 0x41;
            this.gpgLabel5.Text = "<LOC>Source Application";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.ddGame.BackColor = Color.Black;
            this.ddGame.BorderColor = Color.Black;
            this.ddGame.DoValidate = true;
            this.ddGame.FlatStyle = FlatStyle.Flat;
            this.ddGame.FocusBackColor = Color.White;
            this.ddGame.FocusBorderColor = Color.White;
            this.ddGame.ForeColor = Color.White;
            this.ddGame.FormattingEnabled = true;
            this.ddGame.Items.AddRange(new object[] { "Supreme Commander" });
            this.ddGame.Location = new Point(3, 0x79);
            this.ddGame.Name = "ddGame";
            this.ddGame.Size = new Size(0x144, 0x15);
            this.ddGame.TabIndex = 0x40;
            this.ddGame.SelectedValueChanged += new EventHandler(this.ddGame_SelectedValueChanged);
            this.pCustomLoc.Controls.Add(this.cbAlreadyUploaded);
            this.pCustomLoc.Controls.Add(this.gpgLabel9);
            this.pCustomLoc.Controls.Add(this.gpgLabel8);
            this.pCustomLoc.Controls.Add(this.tbUploadSite);
            this.pCustomLoc.Controls.Add(this.tbUploadDirectory);
            this.pCustomLoc.Controls.Add(this.cbRememberFTP);
            this.pCustomLoc.Controls.Add(this.gpgLabel11);
            this.pCustomLoc.Controls.Add(this.tbDownloadURL);
            this.pCustomLoc.Controls.Add(this.gpgLabel10);
            this.pCustomLoc.Controls.Add(this.gpgLabel12);
            this.pCustomLoc.Controls.Add(this.tbUsername);
            this.pCustomLoc.Controls.Add(this.tbPassword);
            this.pCustomLoc.Enabled = false;
            this.pCustomLoc.GPGBorderStyle = GPGBorderStyle.Web;
            this.pCustomLoc.Location = new Point(3, 460);
            this.pCustomLoc.Name = "pCustomLoc";
            this.pCustomLoc.Size = new Size(0x29b, 0xd9);
            this.pCustomLoc.TabIndex = 0x41;
            this.cbAlreadyUploaded.AutoSize = true;
            this.cbAlreadyUploaded.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.cbAlreadyUploaded.ForeColor = Color.White;
            this.cbAlreadyUploaded.Location = new Point(12, 0x76);
            this.cbAlreadyUploaded.Name = "cbAlreadyUploaded";
            this.cbAlreadyUploaded.Size = new Size(0xcd, 0x11);
            this.cbAlreadyUploaded.TabIndex = 0x36;
            this.cbAlreadyUploaded.Text = "<LOC>Already Uploaded (Ignore FTP)";
            this.cbAlreadyUploaded.UsesBG = false;
            this.cbAlreadyUploaded.UseVisualStyleBackColor = false;
            this.cbAlreadyUploaded.CheckedChanged += new EventHandler(this.gpgCheckBox4_CheckedChanged);
            this.cbFilePatch.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.cbFilePatch.ForeColor = Color.White;
            this.cbFilePatch.Location = new Point(0x110, 0xfd);
            this.cbFilePatch.Name = "cbFilePatch";
            this.cbFilePatch.Size = new Size(0x18e, 0x11);
            this.cbFilePatch.TabIndex = 0x42;
            this.cbFilePatch.Text = "<LOC>This version is a file patch (example: loc patches)";
            this.cbFilePatch.UsesBG = false;
            this.cbFilePatch.UseVisualStyleBackColor = false;
            this.cbFilePatch.CheckStateChanged += new EventHandler(this.cbFilePatch_CheckStateChanged);
            this.btnExeName.AutoStyle = true;
            this.btnExeName.BackColor = Color.Transparent;
            this.btnExeName.ButtonState = 0;
            this.btnExeName.DialogResult = DialogResult.OK;
            this.btnExeName.DisabledForecolor = Color.Gray;
            this.btnExeName.DrawColor = Color.White;
            this.btnExeName.DrawEdges = true;
            this.btnExeName.FocusColor = Color.Yellow;
            this.btnExeName.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnExeName.ForeColor = Color.White;
            this.btnExeName.HorizontalScalingMode = ScalingModes.Tile;
            this.btnExeName.IsStyled = true;
            this.btnExeName.Location = new Point(0x285, 20);
            this.btnExeName.Name = "btnExeName";
            this.btnExeName.Size = new Size(20, 20);
            this.btnExeName.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnExeName.TabIndex = 0x45;
            this.btnExeName.TabStop = true;
            this.btnExeName.Text = "...";
            this.btnExeName.TextAlign = ContentAlignment.MiddleCenter;
            this.btnExeName.TextPadding = new Padding(0);
            this.btnExeName.Click += new EventHandler(this.btnExeName_Click);
            this.tbExeName.Location = new Point(3, 20);
            this.tbExeName.Name = "tbExeName";
            this.tbExeName.Properties.Appearance.BackColor = Color.Black;
            this.tbExeName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbExeName.Properties.Appearance.ForeColor = Color.White;
            this.tbExeName.Properties.Appearance.Options.UseBackColor = true;
            this.tbExeName.Properties.Appearance.Options.UseBorderColor = true;
            this.tbExeName.Properties.Appearance.Options.UseForeColor = true;
            this.tbExeName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbExeName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbExeName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbExeName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbExeName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbExeName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbExeName.Properties.BorderStyle = BorderStyles.Simple;
            this.tbExeName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbExeName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbExeName.Properties.MaxLength = 0xff;
            this.tbExeName.Properties.ReadOnly = true;
            this.tbExeName.Size = new Size(0x27e, 20);
            this.tbExeName.TabIndex = 0x44;
            this.tbExeName.TextChanged += new EventHandler(this.tbExeName_TextChanged);
            this.gpgLabel13.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel13.AutoStyle = true;
            this.gpgLabel13.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel13.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel13.ForeColor = Color.White;
            this.gpgLabel13.IgnoreMouseWheel = false;
            this.gpgLabel13.IsStyled = false;
            this.gpgLabel13.Location = new Point(3, 0);
            this.gpgLabel13.Name = "gpgLabel13";
            this.gpgLabel13.Size = new Size(0x29b, 0x11);
            this.gpgLabel13.TabIndex = 0x43;
            this.gpgLabel13.Text = "<LOC>Executable File Name";
            this.gpgLabel13.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel13.TextStyle = TextStyles.Custom;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.btnExeName);
            base.Controls.Add(this.tbExeName);
            base.Controls.Add(this.gpgLabel13);
            base.Controls.Add(this.cbFilePatch);
            base.Controls.Add(this.pCustomLoc);
            base.Controls.Add(this.pPatching);
            base.Controls.Add(this.cbCustomPatch);
            base.Controls.Add(this.tbFriendlyVersion);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.cbCustomLoc);
            base.Controls.Add(this.tbEncryptionKey);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.cbEncryptApp);
            base.Controls.Add(this.btnAppDirectory);
            base.Controls.Add(this.tbDirectory);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.lMD5);
            base.Controls.Add(this.btnVersionFile);
            base.Controls.Add(this.tbVersionFile);
            base.Controls.Add(this.gpgLabel2);
            base.Name = "PnlApplicationUploadOptions";
            base.Size = new Size(0x2a1, 0x2ae);
            this.tbVersionFile.Properties.EndInit();
            this.tbDirectory.Properties.EndInit();
            this.tbEncryptionKey.Properties.EndInit();
            this.tbFriendlyVersion.Properties.EndInit();
            this.tbUploadSite.Properties.EndInit();
            this.tbUploadDirectory.Properties.EndInit();
            this.tbPassword.Properties.EndInit();
            this.tbUsername.Properties.EndInit();
            this.tbDownloadURL.Properties.EndInit();
            this.gpgTextBox2.Properties.EndInit();
            this.pPatching.ResumeLayout(false);
            this.pPatching.PerformLayout();
            this.pCustomLoc.ResumeLayout(false);
            this.pCustomLoc.PerformLayout();
            this.tbExeName.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadFTPInfo()
        {
            this.mLoadingFTP = true;
            if (File.Exists(this.GetFTPFile()))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FTPInfo));
                FileStream stream = new FileStream(this.GetFTPFile(), FileMode.Open);
                FTPInfo info = serializer.Deserialize(stream) as FTPInfo;
                stream.Close();
                this.tbUploadDirectory.Text = info.Dir;
                this.tbUploadSite.Text = info.Server;
                this.tbUsername.Text = info.FTPUser;
                this.tbPassword.Text = info.Pass;
                this.cbRememberFTP.Checked = true;
                this.cbAlreadyUploaded.Checked = info.Uploaded;
                this.tbDownloadURL.Text = info.URL;
            }
            else
            {
                this.cbAlreadyUploaded.Checked = false;
                this.cbRememberFTP.Checked = false;
                this.tbUploadDirectory.Text = "";
                this.tbUploadSite.Text = "";
                this.tbUsername.Text = "";
                this.tbPassword.Text = "";
                this.tbDownloadURL.Text = "";
            }
            this.mLoadingFTP = false;
        }

        private void SaveFTPInfo()
        {
            FTPInfo o = new FTPInfo();
            o.Dir = this.tbUploadDirectory.Text;
            o.Server = this.tbUploadSite.Text;
            o.FTPUser = this.tbUsername.Text;
            o.Pass = this.tbPassword.Text;
            o.Uploaded = this.cbAlreadyUploaded.Checked;
            o.URL = this.tbDownloadURL.Text;
            this.mApp.FTPDir = o.Dir;
            this.mApp.FTPPassword = o.Pass;
            this.mApp.FTPServer = o.Server;
            this.mApp.FTPUser = o.FTPUser;
            this.mApp.FTPUploaded = o.Uploaded;
            this.mApp.PatchURI = o.URL;
            if (!this.mLoadingFTP)
            {
                if (this.cbRememberFTP.Checked)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(FTPInfo));
                    FileStream stream = new FileStream(this.GetFTPFile(), FileMode.Create);
                    serializer.Serialize((Stream) stream, o);
                    stream.Close();
                }
                else if (File.Exists(this.GetFTPFile()))
                {
                    File.Delete(this.GetFTPFile());
                }
            }
        }

        private void SetEncryption()
        {
            if (this.cbEncryptApp.Checked)
            {
                this.mApp.EncryptionKey = this.tbEncryptionKey.Text;
            }
            else
            {
                this.mApp.EncryptionKey = "";
            }
        }

        private void tbDirectory_TextChanged(object sender, EventArgs e)
        {
            this.mApp.LocalUploadDir = this.tbDirectory.Text;
            this.mApp.RelativeAppDirectory = this.GetRelPath(this.mApp.LocalFilePath, this.tbDirectory.Text);
        }

        private void tbDownloadURL_TextChanged(object sender, EventArgs e)
        {
            this.SaveFTPInfo();
        }

        private void tbEncryptionKey_TextChanged(object sender, EventArgs e)
        {
            this.SetEncryption();
        }

        private void tbExeName_TextChanged(object sender, EventArgs e)
        {
            this.mApp.ExeName = this.GetRelPath(this.mApp.LocalFilePath, this.tbExeName.Text);
        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {
            this.SaveFTPInfo();
        }

        private void tbUploadDirectory_TextChanged(object sender, EventArgs e)
        {
            this.SaveFTPInfo();
        }

        private void tbUploadSite_TextChanged(object sender, EventArgs e)
        {
            this.SaveFTPInfo();
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {
            this.SaveFTPInfo();
        }

        private void tbVersionFile_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.lMD5.Text = AppUtils.ChkSumFile(this.tbVersionFile.Text);
                this.mApp.RelativeVersionFile = this.GetRelPath(this.mApp.LocalFilePath, this.tbVersionFile.Text);
                this.mApp.VersionCheck = this.lMD5.Text;
            }
            catch (Exception)
            {
                this.lMD5.Text = "Error";
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

