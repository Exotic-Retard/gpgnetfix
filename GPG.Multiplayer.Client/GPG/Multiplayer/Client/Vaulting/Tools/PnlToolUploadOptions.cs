namespace GPG.Multiplayer.Client.Vaulting.Tools
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class PnlToolUploadOptions : PnlBase
    {
        private ComboBox comboBoxReleaseQual;
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabelNumberFiles;
        private GPGRadioButton gpgRadioButtonExeDir;
        private GPGRadioButton gpgRadioButtonExeDirSubs;
        private GPGRadioButton gpgRadioButtonExeOnly;
        private GPGTextBox gpgTextBoxDeveloper;
        private GPGTextBox gpgTextBoxExePath;
        private GPGTextBox gpgTextBoxVolunteer;
        private GPGTextBox gpgTextBoxWebsite;
        private GPG.Multiplayer.Client.Vaulting.Tools.Tool mTool;

        public PnlToolUploadOptions(GPG.Multiplayer.Client.Vaulting.Tools.Tool tool)
        {
            this.InitializeComponent();
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Unspecified"), "<LOC>Unspecified"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Untested"), "<LOC>Untested"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Alpha"), "<LOC>Alpha"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Beta"), "<LOC>Beta"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Release"), "<LOC>Release"));
            this.comboBoxReleaseQual.SelectedIndex = 0;
            if ((tool.LocalFilePath != null) && (tool.LocalFilePath.Length > 0))
            {
                if (tool.LocalFilePath.EndsWith(".zip"))
                {
                    this.gpgRadioButtonExeDir.Enabled = false;
                    this.gpgRadioButtonExeDirSubs.Enabled = false;
                    this.gpgRadioButtonExeOnly.Enabled = false;
                }
                else
                {
                    this.gpgRadioButtonExeDir.Enabled = true;
                    this.gpgRadioButtonExeDirSubs.Enabled = true;
                    this.gpgRadioButtonExeOnly.Enabled = true;
                }
            }
            this.gpgTextBoxExePath.Text = tool.ExeName;
            this.gpgTextBoxVolunteer.Text = tool.DownloadVolunteerEffort;
            this.gpgTextBoxDeveloper.Text = tool.DeveloperName;
            this.gpgTextBoxWebsite.Text = tool.Website;
            for (int i = 0; i < this.comboBoxReleaseQual.Items.Count; i++)
            {
                if ((this.comboBoxReleaseQual.Items[i] as MultiVal<string, string>).Value2 == tool.Quality)
                {
                    this.comboBoxReleaseQual.SelectedIndex = i;
                    break;
                }
            }
            this.mTool = tool;
            this.ToolAttributeChanged(null, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.gpgTextBoxDeveloper = new GPGTextBox();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabelNumberFiles = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxWebsite = new GPGTextBox();
            this.comboBoxReleaseQual = new ComboBox();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgRadioButtonExeOnly = new GPGRadioButton();
            this.gpgRadioButtonExeDir = new GPGRadioButton();
            this.gpgRadioButtonExeDirSubs = new GPGRadioButton();
            this.gpgLabel4 = new GPGLabel();
            this.gpgTextBoxVolunteer = new GPGTextBox();
            this.gpgLabel8 = new GPGLabel();
            this.gpgTextBoxExePath = new GPGTextBox();
            this.gpgTextBoxDeveloper.Properties.BeginInit();
            this.gpgTextBoxWebsite.Properties.BeginInit();
            this.gpgTextBoxVolunteer.Properties.BeginInit();
            this.gpgTextBoxExePath.Properties.BeginInit();
            base.SuspendLayout();
            this.gpgTextBoxDeveloper.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxDeveloper.Location = new Point(0, 0x9e);
            this.gpgTextBoxDeveloper.Name = "gpgTextBoxDeveloper";
            this.gpgTextBoxDeveloper.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxDeveloper.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxDeveloper.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxDeveloper.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxDeveloper.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxDeveloper.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxDeveloper.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxDeveloper.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxDeveloper.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxDeveloper.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxDeveloper.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxDeveloper.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxDeveloper.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxDeveloper.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxDeveloper.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxDeveloper.Properties.MaxLength = 0xff;
            this.gpgTextBoxDeveloper.Size = new Size(0xe2, 20);
            this.gpgTextBoxDeveloper.TabIndex = 9;
            this.gpgTextBoxDeveloper.EditValueChanged += new EventHandler(this.ToolAttributeChanged);
            this.gpgTextBoxDeveloper.TextChanged += new EventHandler(this.ToolAttributeChanged);
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0, 0x5b);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(590, 0x11);
            this.gpgLabel6.TabIndex = 0x10;
            this.gpgLabel6.Text = "<LOC>Number of files";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabelNumberFiles.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNumberFiles.AutoSize = true;
            this.gpgLabelNumberFiles.AutoStyle = true;
            this.gpgLabelNumberFiles.Font = new Font("Arial", 9.75f);
            this.gpgLabelNumberFiles.ForeColor = Color.White;
            this.gpgLabelNumberFiles.IgnoreMouseWheel = false;
            this.gpgLabelNumberFiles.IsStyled = false;
            this.gpgLabelNumberFiles.Location = new Point(0, 0x6d);
            this.gpgLabelNumberFiles.Name = "gpgLabelNumberFiles";
            this.gpgLabelNumberFiles.Size = new Size(0x43, 0x10);
            this.gpgLabelNumberFiles.TabIndex = 0x11;
            this.gpgLabelNumberFiles.Text = "gpgLabel2";
            this.gpgLabelNumberFiles.TextStyle = TextStyles.Default;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0, 0x8a);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(590, 0x11);
            this.gpgLabel1.TabIndex = 0x12;
            this.gpgLabel1.Text = "<LOC>Developer Name";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0xf5, 0x8a);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(250, 0x11);
            this.gpgLabel2.TabIndex = 0x13;
            this.gpgLabel2.Text = "<LOC>Product Website";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgTextBoxWebsite.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxWebsite.Location = new Point(0xf7, 0x9e);
            this.gpgTextBoxWebsite.Name = "gpgTextBoxWebsite";
            this.gpgTextBoxWebsite.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxWebsite.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxWebsite.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxWebsite.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxWebsite.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxWebsite.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxWebsite.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxWebsite.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxWebsite.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxWebsite.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxWebsite.Properties.MaxLength = 0xff;
            this.gpgTextBoxWebsite.Size = new Size(0x157, 20);
            this.gpgTextBoxWebsite.TabIndex = 20;
            this.gpgTextBoxWebsite.TextChanged += new EventHandler(this.ToolAttributeChanged);
            this.comboBoxReleaseQual.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxReleaseQual.FormattingEnabled = true;
            this.comboBoxReleaseQual.Location = new Point(0xa1, 0x6f);
            this.comboBoxReleaseQual.Name = "comboBoxReleaseQual";
            this.comboBoxReleaseQual.Size = new Size(0xc6, 0x15);
            this.comboBoxReleaseQual.TabIndex = 0x19;
            this.comboBoxReleaseQual.SelectedIndexChanged += new EventHandler(this.ToolAttributeChanged);
            this.gpgLabel5.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel5.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x9f, 0x5b);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(200, 0x11);
            this.gpgLabel5.TabIndex = 0x1a;
            this.gpgLabel5.Text = "<LOC>Release Quality";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.gpgLabel3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0, 0);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(590, 0x11);
            this.gpgLabel3.TabIndex = 0x1b;
            this.gpgLabel3.Text = "<LOC>Packaging Method";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.gpgRadioButtonExeOnly.AutoSize = true;
            this.gpgRadioButtonExeOnly.ForeColor = Color.White;
            this.gpgRadioButtonExeOnly.Location = new Point(4, 0x15);
            this.gpgRadioButtonExeOnly.Name = "gpgRadioButtonExeOnly";
            this.gpgRadioButtonExeOnly.Size = new Size(0x87, 0x11);
            this.gpgRadioButtonExeOnly.TabIndex = 0x1c;
            this.gpgRadioButtonExeOnly.Text = "<LOC>Executable Only";
            this.gpgRadioButtonExeOnly.UseVisualStyleBackColor = true;
            this.gpgRadioButtonExeOnly.CheckedChanged += new EventHandler(this.PackageMethodChanged);
            this.gpgRadioButtonExeDir.AutoSize = true;
            this.gpgRadioButtonExeDir.ForeColor = Color.White;
            this.gpgRadioButtonExeDir.Location = new Point(0x91, 0x15);
            this.gpgRadioButtonExeDir.Name = "gpgRadioButtonExeDir";
            this.gpgRadioButtonExeDir.Size = new Size(0x9c, 0x11);
            this.gpgRadioButtonExeDir.TabIndex = 0x1d;
            this.gpgRadioButtonExeDir.Text = "<LOC>Executable Directory";
            this.gpgRadioButtonExeDir.UseVisualStyleBackColor = true;
            this.gpgRadioButtonExeDir.CheckedChanged += new EventHandler(this.PackageMethodChanged);
            this.gpgRadioButtonExeDirSubs.AutoSize = true;
            this.gpgRadioButtonExeDirSubs.Checked = true;
            this.gpgRadioButtonExeDirSubs.ForeColor = Color.White;
            this.gpgRadioButtonExeDirSubs.Location = new Point(0x133, 0x15);
            this.gpgRadioButtonExeDirSubs.Name = "gpgRadioButtonExeDirSubs";
            this.gpgRadioButtonExeDirSubs.Size = new Size(250, 0x11);
            this.gpgRadioButtonExeDirSubs.TabIndex = 30;
            this.gpgRadioButtonExeDirSubs.TabStop = true;
            this.gpgRadioButtonExeDirSubs.Text = "<LOC>Executable Directory and Sub-directories";
            this.gpgRadioButtonExeDirSubs.UseVisualStyleBackColor = true;
            this.gpgRadioButtonExeDirSubs.CheckedChanged += new EventHandler(this.PackageMethodChanged);
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel4.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x177, 0x5b);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(200, 0x11);
            this.gpgLabel4.TabIndex = 0x1f;
            this.gpgLabel4.Text = "<LOC>Download Volunteer Effort";
            this.gpgLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel4.TextStyle = TextStyles.Custom;
            this.gpgTextBoxVolunteer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxVolunteer.Location = new Point(0x179, 0x6f);
            this.gpgTextBoxVolunteer.Name = "gpgTextBoxVolunteer";
            this.gpgTextBoxVolunteer.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxVolunteer.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxVolunteer.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxVolunteer.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxVolunteer.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxVolunteer.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxVolunteer.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxVolunteer.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxVolunteer.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxVolunteer.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxVolunteer.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxVolunteer.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxVolunteer.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxVolunteer.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxVolunteer.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxVolunteer.Properties.MaxLength = 0xff;
            this.gpgTextBoxVolunteer.Size = new Size(0xd5, 20);
            this.gpgTextBoxVolunteer.TabIndex = 0x20;
            this.gpgLabel8.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel8.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0, 0x2e);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(590, 0x11);
            this.gpgLabel8.TabIndex = 0x21;
            this.gpgLabel8.Text = "<LOC>Executable Path";
            this.gpgLabel8.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel8.TextStyle = TextStyles.Custom;
            this.gpgTextBoxExePath.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxExePath.Location = new Point(0, 0x42);
            this.gpgTextBoxExePath.Name = "gpgTextBoxExePath";
            this.gpgTextBoxExePath.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxExePath.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxExePath.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxExePath.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxExePath.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxExePath.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxExePath.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxExePath.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxExePath.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxExePath.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxExePath.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxExePath.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxExePath.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxExePath.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxExePath.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxExePath.Properties.MaxLength = 0xff;
            this.gpgTextBoxExePath.Size = new Size(0x167, 20);
            this.gpgTextBoxExePath.TabIndex = 0x22;
            this.gpgTextBoxExePath.TextChanged += new EventHandler(this.ToolAttributeChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgTextBoxExePath);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgTextBoxVolunteer);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgRadioButtonExeDirSubs);
            base.Controls.Add(this.gpgRadioButtonExeDir);
            base.Controls.Add(this.gpgRadioButtonExeOnly);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.comboBoxReleaseQual);
            base.Controls.Add(this.gpgTextBoxWebsite);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgLabelNumberFiles);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgTextBoxDeveloper);
            base.Name = "PnlToolUploadOptions";
            base.Size = new Size(590, 0xb3);
            this.gpgTextBoxDeveloper.Properties.EndInit();
            this.gpgTextBoxWebsite.Properties.EndInit();
            this.gpgTextBoxVolunteer.Properties.EndInit();
            this.gpgTextBoxExePath.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PackageMethodChanged(object sender, EventArgs e)
        {
            if (this.gpgRadioButtonExeOnly.Checked)
            {
                this.Tool.PackagingMethod = PackagingMethods.TargetOnly;
            }
            else if (this.gpgRadioButtonExeDir.Checked)
            {
                this.Tool.PackagingMethod = PackagingMethods.TargetDirectory;
            }
            else if (this.gpgRadioButtonExeDirSubs.Checked)
            {
                this.Tool.PackagingMethod = PackagingMethods.TargetDirectoryRecursive;
            }
            this.ToolAttributeChanged(null, null);
        }

        private void ToolAttributeChanged(object sender, EventArgs e)
        {
            if (this.Tool != null)
            {
                this.gpgLabelNumberFiles.Text = this.Tool.NumberOfFiles.ToString();
                this.Tool.ExeName = this.gpgTextBoxExePath.Text;
                this.Tool.DownloadVolunteerEffort = this.gpgTextBoxVolunteer.Text;
                this.Tool.DeveloperName = this.gpgTextBoxDeveloper.Text;
                this.Tool.Quality = (this.comboBoxReleaseQual.SelectedItem as MultiVal<string, string>).Value2;
                this.Tool.Website = this.gpgTextBoxWebsite.Text;
            }
        }

        public GPG.Multiplayer.Client.Vaulting.Tools.Tool Tool
        {
            get
            {
                return this.mTool;
            }
        }
    }
}

