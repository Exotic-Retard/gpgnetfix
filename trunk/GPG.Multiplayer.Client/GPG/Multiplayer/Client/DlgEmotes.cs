namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public class DlgEmotes : DlgBase
    {
        private SkinButton btnChooseFile;
        private GPGContextMenu cmIcons;
        private IContainer components;
        private SkinButton gpgButtonChooseNewIcon;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGPanel gpgPanelIcons;
        private GPGTextBox gpgTextBoxFilePath;
        private GPGTextBox gpgTextBoxKeySeq;
        private GPGTextBox gpgTextBoxNewKeySeq;
        private GPGTextBox gpgTextBoxNewOutFile;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private bool ImageSet;
        private MenuItem miDelete;
        private MenuItem miViewFull;
        private string mRestrictedEmotes;
        private PictureBox mSelectedIcon;
        private PictureBox pictureBoxNewIcon;
        private SkinButton skinButtonClose;
        private SkinButton skinButtonCopy;
        private SkinButton skinButtonCreate;

        public DlgEmotes() : this(Program.MainForm)
        {
        }

        public DlgEmotes(FrmMain mainForm) : base(mainForm)
        {
            EventHandler handler = null;
            this.ImageSet = false;
            this.mRestrictedEmotes = null;
            this.mSelectedIcon = null;
            this.components = null;
            this.InitializeComponent();
            this.gpgTextBoxNewOutFile.Text = Emote.DefaultEmoteFile;
            this.ResetForm();
            this.RefreshEmotes();
            if (handler == null)
            {
                handler = delegate (object s, EventArgs e) {
                    VGen0 method = null;
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.RefreshEmotes();
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (!(base.Disposing || base.IsDisposed))
                    {
                        this.RefreshEmotes();
                    }
                };
            }
            Emote.EmotesChanged += handler;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgButtonChooseNewIcon_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = string.Format(Loc.Get("<LOC>Image Files (*.bmp;*.jpg;*.gif;*.png){0}"), "|*.bmp;*.jpg;*.gif;*.png");
            dialog.Multiselect = false;
            dialog.InitialDirectory = Program.Settings.Chat.Emotes.EmoteImageDir;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    Image image = Image.FromFile(dialog.FileName);
                    if ((image.Width > 40) || (image.Height > 20))
                    {
                        this.pictureBoxNewIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        this.pictureBoxNewIcon.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                    this.pictureBoxNewIcon.Image = image;
                    this.ImageSet = true;
                    Program.Settings.Chat.Emotes.EmoteImageDir = Path.GetDirectoryName(dialog.FileName);
                }
                catch
                {
                    base.Error("<LOC>Unable to load image, please check the image format.", new object[0]);
                }
            }
        }

        private void gpgButtonChooseOutFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Emote.EmoteDirectory;
            dialog.OverwritePrompt = false;
            dialog.CreatePrompt = true;
            dialog.Filter = string.Format("GPG Emotes (*.{0})|*.{0}", "gpg");
            dialog.AddExtension = true;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.gpgTextBoxNewOutFile.Text = dialog.FileName;
                this.gpgTextBoxNewOutFile.Select(this.gpgTextBoxNewOutFile.Text.Length, 0);
            }
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.skinButtonCopy = new SkinButton();
            this.gpgPanelIcons = new GPGPanel();
            this.gpgTextBoxFilePath = new GPGTextBox();
            this.gpgTextBoxKeySeq = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.groupBox2 = new GroupBox();
            this.skinButtonClose = new SkinButton();
            this.skinButtonCreate = new SkinButton();
            this.gpgButtonChooseNewIcon = new SkinButton();
            this.btnChooseFile = new SkinButton();
            this.gpgLabel5 = new GPGLabel();
            this.pictureBoxNewIcon = new PictureBox();
            this.gpgTextBoxNewOutFile = new GPGTextBox();
            this.gpgLabel4 = new GPGLabel();
            this.gpgTextBoxNewKeySeq = new GPGTextBox();
            this.gpgLabel3 = new GPGLabel();
            this.cmIcons = new GPGContextMenu();
            this.miViewFull = new MenuItem();
            this.miDelete = new MenuItem();
            this.groupBox1.SuspendLayout();
            this.gpgTextBoxFilePath.Properties.BeginInit();
            this.gpgTextBoxKeySeq.Properties.BeginInit();
            this.groupBox2.SuspendLayout();
            this.btnChooseFile.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxNewIcon).BeginInit();
            this.gpgTextBoxNewOutFile.Properties.BeginInit();
            this.gpgTextBoxNewKeySeq.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.skinButtonCopy);
            this.groupBox1.Controls.Add(this.gpgPanelIcons);
            this.groupBox1.Controls.Add(this.gpgTextBoxFilePath);
            this.groupBox1.Controls.Add(this.gpgTextBoxKeySeq);
            this.groupBox1.Controls.Add(this.gpgLabel2);
            this.groupBox1.Controls.Add(this.gpgLabel1);
            this.groupBox1.Location = new Point(12, 0x53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x25d, 0xcf);
            base.ttDefault.SetSuperTip(this.groupBox1, null);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "<LOC>All Emotes";
            this.skinButtonCopy.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonCopy.AutoStyle = true;
            this.skinButtonCopy.BackColor = Color.Black;
            this.skinButtonCopy.DialogResult = DialogResult.OK;
            this.skinButtonCopy.DisabledForecolor = Color.Gray;
            this.skinButtonCopy.DrawEdges = true;
            this.skinButtonCopy.FocusColor = Color.Yellow;
            this.skinButtonCopy.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCopy.ForeColor = Color.White;
            this.skinButtonCopy.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCopy.IsStyled = true;
            this.skinButtonCopy.Location = new Point(0x108, 0xb6);
            this.skinButtonCopy.Name = "skinButtonCopy";
            this.skinButtonCopy.Size = new Size(70, 0x15);
            this.skinButtonCopy.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCopy, null);
            this.skinButtonCopy.TabIndex = 12;
            this.skinButtonCopy.Text = "<LOC>Copy";
            this.skinButtonCopy.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCopy.TextPadding = new Padding(0);
            this.skinButtonCopy.Click += new EventHandler(this.skinButtonCopy_Click);
            this.gpgPanelIcons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelIcons.AutoScroll = true;
            this.gpgPanelIcons.Location = new Point(10, 0x15);
            this.gpgPanelIcons.Name = "gpgPanelIcons";
            this.gpgPanelIcons.Size = new Size(0x246, 0x9e);
            base.ttDefault.SetSuperTip(this.gpgPanelIcons, null);
            this.gpgPanelIcons.TabIndex = 5;
            this.gpgTextBoxFilePath.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextBoxFilePath.EditValue = "N/A";
            this.gpgTextBoxFilePath.Location = new Point(0x1a6, 0xb9);
            this.gpgTextBoxFilePath.Name = "gpgTextBoxFilePath";
            this.gpgTextBoxFilePath.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxFilePath.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxFilePath.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxFilePath.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxFilePath.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxFilePath.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxFilePath.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxFilePath.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxFilePath.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxFilePath.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxFilePath.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxFilePath.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxFilePath.Properties.BorderStyle = BorderStyles.NoBorder;
            this.gpgTextBoxFilePath.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxFilePath.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxFilePath.Properties.ReadOnly = true;
            this.gpgTextBoxFilePath.Size = new Size(170, 0x12);
            this.gpgTextBoxFilePath.TabIndex = 4;
            this.gpgTextBoxFilePath.Visible = false;
            this.gpgTextBoxKeySeq.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextBoxKeySeq.EditValue = "N/A";
            this.gpgTextBoxKeySeq.Location = new Point(0x6a, 0xb7);
            this.gpgTextBoxKeySeq.Name = "gpgTextBoxKeySeq";
            this.gpgTextBoxKeySeq.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxKeySeq.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxKeySeq.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxKeySeq.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxKeySeq.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxKeySeq.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxKeySeq.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxKeySeq.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxKeySeq.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxKeySeq.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxKeySeq.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxKeySeq.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxKeySeq.Properties.BorderStyle = BorderStyles.NoBorder;
            this.gpgTextBoxKeySeq.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxKeySeq.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxKeySeq.Properties.ReadOnly = true;
            this.gpgTextBoxKeySeq.Size = new Size(0x98, 0x12);
            this.gpgTextBoxKeySeq.TabIndex = 3;
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x13a, 0xb9);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x66, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 1;
            this.gpgLabel2.Text = "<LOC>File Path";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgLabel2.Visible = false;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(7, 0xb9);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x87, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 0;
            this.gpgLabel1.Text = "<LOC>Key Sequence";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.groupBox2.Controls.Add(this.skinButtonClose);
            this.groupBox2.Controls.Add(this.skinButtonCreate);
            this.groupBox2.Controls.Add(this.gpgButtonChooseNewIcon);
            this.groupBox2.Controls.Add(this.btnChooseFile);
            this.groupBox2.Controls.Add(this.pictureBoxNewIcon);
            this.groupBox2.Controls.Add(this.gpgTextBoxNewOutFile);
            this.groupBox2.Controls.Add(this.gpgLabel4);
            this.groupBox2.Controls.Add(this.gpgTextBoxNewKeySeq);
            this.groupBox2.Controls.Add(this.gpgLabel3);
            this.groupBox2.Location = new Point(11, 0x128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x25d, 110);
            base.ttDefault.SetSuperTip(this.groupBox2, null);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "<LOC>Create New Emotes";
            this.skinButtonClose.AutoStyle = true;
            this.skinButtonClose.BackColor = Color.Black;
            this.skinButtonClose.DialogResult = DialogResult.OK;
            this.skinButtonClose.DisabledForecolor = Color.Gray;
            this.skinButtonClose.DrawEdges = true;
            this.skinButtonClose.FocusColor = Color.Yellow;
            this.skinButtonClose.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClose.ForeColor = Color.White;
            this.skinButtonClose.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClose.IsStyled = true;
            this.skinButtonClose.Location = new Point(0x1c1, 0x4f);
            this.skinButtonClose.Name = "skinButtonClose";
            this.skinButtonClose.Size = new Size(0x3e, 0x13);
            this.skinButtonClose.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClose, null);
            this.skinButtonClose.TabIndex = 12;
            this.skinButtonClose.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClose.TextPadding = new Padding(0);
            this.skinButtonClose.Visible = false;
            this.skinButtonClose.Click += new EventHandler(this.skinButtonClose_Click);
            this.skinButtonCreate.Anchor = AnchorStyles.Bottom;
            this.skinButtonCreate.AutoStyle = true;
            this.skinButtonCreate.BackColor = Color.Black;
            this.skinButtonCreate.DialogResult = DialogResult.OK;
            this.skinButtonCreate.DisabledForecolor = Color.Gray;
            this.skinButtonCreate.DrawEdges = true;
            this.skinButtonCreate.FocusColor = Color.Yellow;
            this.skinButtonCreate.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCreate.ForeColor = Color.White;
            this.skinButtonCreate.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCreate.IsStyled = true;
            this.skinButtonCreate.Location = new Point(0xee, 0x4f);
            this.skinButtonCreate.Name = "skinButtonCreate";
            this.skinButtonCreate.Size = new Size(0x88, 0x15);
            this.skinButtonCreate.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCreate, null);
            this.skinButtonCreate.TabIndex = 8;
            this.skinButtonCreate.Text = "<LOC>Create Emote";
            this.skinButtonCreate.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCreate.TextPadding = new Padding(0);
            this.skinButtonCreate.Click += new EventHandler(this.skinButtonCreate_Click);
            this.gpgButtonChooseNewIcon.Anchor = AnchorStyles.Bottom;
            this.gpgButtonChooseNewIcon.AutoStyle = true;
            this.gpgButtonChooseNewIcon.BackColor = Color.Black;
            this.gpgButtonChooseNewIcon.DialogResult = DialogResult.OK;
            this.gpgButtonChooseNewIcon.DisabledForecolor = Color.Gray;
            this.gpgButtonChooseNewIcon.DrawEdges = true;
            this.gpgButtonChooseNewIcon.FocusColor = Color.Yellow;
            this.gpgButtonChooseNewIcon.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgButtonChooseNewIcon.ForeColor = Color.White;
            this.gpgButtonChooseNewIcon.HorizontalScalingMode = ScalingModes.Tile;
            this.gpgButtonChooseNewIcon.IsStyled = true;
            this.gpgButtonChooseNewIcon.Location = new Point(0xca, 0x25);
            this.gpgButtonChooseNewIcon.Name = "gpgButtonChooseNewIcon";
            this.gpgButtonChooseNewIcon.Size = new Size(0x5d, 0x15);
            this.gpgButtonChooseNewIcon.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.gpgButtonChooseNewIcon, null);
            this.gpgButtonChooseNewIcon.TabIndex = 11;
            this.gpgButtonChooseNewIcon.Text = "<LOC>Browse...";
            this.gpgButtonChooseNewIcon.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgButtonChooseNewIcon.TextPadding = new Padding(0);
            this.gpgButtonChooseNewIcon.Click += new EventHandler(this.gpgButtonChooseNewIcon_Click);
            this.btnChooseFile.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnChooseFile.AutoStyle = true;
            this.btnChooseFile.BackColor = Color.Black;
            this.btnChooseFile.Controls.Add(this.gpgLabel5);
            this.btnChooseFile.DialogResult = DialogResult.OK;
            this.btnChooseFile.DisabledForecolor = Color.Gray;
            this.btnChooseFile.DrawEdges = true;
            this.btnChooseFile.FocusColor = Color.Yellow;
            this.btnChooseFile.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnChooseFile.ForeColor = Color.White;
            this.btnChooseFile.HorizontalScalingMode = ScalingModes.Tile;
            this.btnChooseFile.IsStyled = true;
            this.btnChooseFile.Location = new Point(0x220, 0x55);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new Size(0x2f, 0x15);
            this.btnChooseFile.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnChooseFile, null);
            this.btnChooseFile.TabIndex = 10;
            this.btnChooseFile.Text = "<LOC>Browse...";
            this.btnChooseFile.TextAlign = ContentAlignment.MiddleCenter;
            this.btnChooseFile.TextPadding = new Padding(0);
            this.btnChooseFile.Visible = false;
            this.btnChooseFile.Click += new EventHandler(this.gpgButtonChooseOutFile_Click);
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.Black;
            this.gpgLabel5.Font = new Font("Verdana", 8f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(-50, 1);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(110, 13);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 4;
            this.gpgLabel5.Text = "<LOC>Output File";
            this.gpgLabel5.TextStyle = TextStyles.Default;
            this.gpgLabel5.Visible = false;
            this.pictureBoxNewIcon.Anchor = AnchorStyles.Bottom;
            this.pictureBoxNewIcon.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBoxNewIcon.InitialImage = Resources.reject;
            this.pictureBoxNewIcon.Location = new Point(0x9d, 0x25);
            this.pictureBoxNewIcon.Name = "pictureBoxNewIcon";
            this.pictureBoxNewIcon.Size = new Size(40, 20);
            this.pictureBoxNewIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pictureBoxNewIcon, null);
            this.pictureBoxNewIcon.TabIndex = 0;
            this.pictureBoxNewIcon.TabStop = false;
            this.gpgTextBoxNewOutFile.Location = new Point(0x233, 0x30);
            this.gpgTextBoxNewOutFile.Name = "gpgTextBoxNewOutFile";
            this.gpgTextBoxNewOutFile.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxNewOutFile.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxNewOutFile.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxNewOutFile.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxNewOutFile.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxNewOutFile.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxNewOutFile.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxNewOutFile.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxNewOutFile.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxNewOutFile.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxNewOutFile.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxNewOutFile.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxNewOutFile.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxNewOutFile.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxNewOutFile.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxNewOutFile.Size = new Size(0x1c, 20);
            this.gpgTextBoxNewOutFile.TabIndex = 5;
            this.gpgTextBoxNewOutFile.Visible = false;
            this.gpgLabel4.Anchor = AnchorStyles.Bottom;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x13d, 0x13);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x87, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 3;
            this.gpgLabel4.Text = "<LOC>Key Sequence";
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.gpgTextBoxNewKeySeq.Anchor = AnchorStyles.Bottom;
            this.gpgTextBoxNewKeySeq.Location = new Point(320, 0x26);
            this.gpgTextBoxNewKeySeq.Name = "gpgTextBoxNewKeySeq";
            this.gpgTextBoxNewKeySeq.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxNewKeySeq.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxNewKeySeq.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxNewKeySeq.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxNewKeySeq.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxNewKeySeq.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxNewKeySeq.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxNewKeySeq.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxNewKeySeq.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxNewKeySeq.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxNewKeySeq.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxNewKeySeq.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxNewKeySeq.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxNewKeySeq.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxNewKeySeq.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxNewKeySeq.Size = new Size(0x84, 20);
            this.gpgTextBoxNewKeySeq.TabIndex = 2;
            this.gpgLabel3.Anchor = AnchorStyles.Bottom;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x98, 0x12);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x7a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 1;
            this.gpgLabel3.Text = "<LOC>Choose Icon";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.cmIcons.MenuItems.AddRange(new MenuItem[] { this.miViewFull, this.miDelete });
            this.miViewFull.Index = 0;
            this.miViewFull.Text = "View Full Image";
            this.miViewFull.Visible = false;
            this.miViewFull.Click += new EventHandler(this.miViewFull_Click);
            this.miDelete.Index = 1;
            this.miDelete.Text = "Delete";
            this.miDelete.Click += new EventHandler(this.miDelete_Click);
            base.AcceptButton = this.skinButtonCreate;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonClose;
            base.ClientSize = new Size(0x274, 0x1d5);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.groupBox2);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x274, 0x1d5);
            this.MinimumSize = new Size(0x274, 0x1d5);
            base.Name = "DlgEmotes";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Emote Manager";
            base.Controls.SetChildIndex(this.groupBox2, 0);
            base.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gpgTextBoxFilePath.Properties.EndInit();
            this.gpgTextBoxKeySeq.Properties.EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.btnChooseFile.ResumeLayout(false);
            this.btnChooseFile.PerformLayout();
            ((ISupportInitialize) this.pictureBoxNewIcon).EndInit();
            this.gpgTextBoxNewOutFile.Properties.EndInit();
            this.gpgTextBoxNewKeySeq.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
            (this.SelectedIcon.Tag as Emote).Delete(true, base.MainForm);
        }

        private void miViewFull_Click(object sender, EventArgs e)
        {
            if (this.SelectedIcon != null)
            {
                Emote tag = this.SelectedIcon.Tag as Emote;
                new DlgPictureView(tag.Image).Show();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            try
            {
                if (((this.gpgPanelIcons != null) && (this.gpgPanelIcons.Controls.Count > 0)) && base.Visible)
                {
                    this.RefreshEmotes();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void RefreshEmotes()
        {
            PictureBox current;
            EventHandler handler = null;
            MouseEventHandler handler2 = null;
            using (IEnumerator enumerator = this.gpgPanelIcons.Controls.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    current = (PictureBox) enumerator.Current;
                    ImageAnimator.StopAnimate(current.Image, null);
                    current.Image = null;
                    current.Dispose();
                }
            }
            this.gpgPanelIcons.Controls.Clear();
            int num = 6;
            int num2 = num;
            int num3 = num;
            int width = 40;
            foreach (Emote emote in Emote.AllEmotes.Values)
            {
                current = new PictureBox();
                current.Tag = emote;
                if (handler == null)
                {
                    handler = delegate (object sender, EventArgs e) {
                        Emote tag = (sender as PictureBox).Tag as Emote;
                        this.gpgTextBoxKeySeq.Text = tag.CharSequence;
                        this.gpgTextBoxFilePath.Text = tag.FileName;
                        this.gpgTextBoxFilePath.Select(this.gpgTextBoxFilePath.Text.Length, 0);
                    };
                }
                current.Click += handler;
                if (handler2 == null)
                {
                    handler2 = delegate (object sender, MouseEventArgs e) {
                        this.mSelectedIcon = sender as PictureBox;
                    };
                }
                current.MouseDown += handler2;
                current.Image = emote.Image;
                current.Cursor = Cursors.Hand;
                base.ttDefault.SetToolTip(current, emote.CharSequence);
                current.SizeMode = PictureBoxSizeMode.CenterImage;
                current.Size = new Size(width, width / 2);
                current.ContextMenu = this.cmIcons;
                current.Location = new Point(num2 + num, num3 + num);
                num2 += current.Width + num;
                if ((num2 + width) > (this.gpgPanelIcons.Width - num))
                {
                    num3 += width + num;
                    num2 = num;
                }
                this.gpgPanelIcons.Controls.Add(current);
            }
        }

        private void ResetForm()
        {
            this.pictureBoxNewIcon.Image = Emotes.no_img;
            this.pictureBoxNewIcon.SizeMode = PictureBoxSizeMode.CenterImage;
            this.gpgTextBoxNewKeySeq.Text = "";
            this.gpgTextBoxNewOutFile.Select(this.gpgTextBoxNewOutFile.Text.Length, 0);
        }

        private void skinButtonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void skinButtonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.gpgTextBoxKeySeq.Text);
        }

        private void skinButtonCreate_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (!((this.pictureBoxNewIcon.Image != null) && this.ImageSet))
            {
                base.Error(this.gpgButtonChooseNewIcon, "<LOC>You must choose an icon for this emote.", new object[0]);
            }
            else if ((this.gpgTextBoxNewKeySeq.Text == null) || (this.gpgTextBoxNewKeySeq.Text.Length < 2))
            {
                base.Error(this.gpgTextBoxNewKeySeq, "<LOC>You must choose a key sequence for this emote of at least 2 characters.", new object[0]);
            }
            else if (Emote.AllEmotes.ContainsKey(this.gpgTextBoxNewKeySeq.Text))
            {
                base.Error(this.gpgTextBoxNewKeySeq, "<LOC>You already have an emote with this key sequence. If you wish to replace it you must delete it first.", new object[0]);
            }
            else if (Profanity.ContainsProfanity(this.gpgTextBoxNewKeySeq.Text))
            {
                base.Error(this.gpgTextBoxNewKeySeq, "<LOC>Key sequence may not contain profanity.", new object[0]);
            }
            else if ((this.gpgTextBoxNewOutFile.Text == null) || (this.gpgTextBoxNewOutFile.Text.Length < 1))
            {
                base.Error(this.skinButtonCreate, "<LOC>You must specify the ouput file for this emote.", new object[0]);
            }
            else
            {
                for (int i = 0; i < this.RestrictedEmotes.Length; i++)
                {
                    if (this.gpgTextBoxNewKeySeq.Text.Contains(this.RestrictedEmotes[i]))
                    {
                        base.Error(this.gpgTextBoxNewKeySeq, "<LOC>'{0}' is a restricted key sequence. Please choose another.", new object[] { this.gpgTextBoxNewKeySeq.Text });
                        return;
                    }
                }
                if (Program.Settings.Chat.Emotes.ShowCreateWarning)
                {
                    string msg = "<LOC>This tool allows you to create and share emote icons with your own images. Sharing offensive or inappropriate images is grounds for deactivation of your user account and product key. You can learn more about custom emotes via the Knowledge Base. Are you sure you want to create this emote?";
                    DlgYesNo no = new DlgYesNo(base.MainForm, "<LOC>Warning", msg, new Size(350, 0x145));
                    no.DoNotShowAgainCheck = true;
                    if (no.ShowDialog() == DialogResult.Yes)
                    {
                        Program.Settings.Chat.Emotes.ShowCreateWarning = !no.DoNotShowAgainValue;
                    }
                    else
                    {
                        Program.Settings.Chat.Emotes.ShowCreateWarning = !no.DoNotShowAgainValue;
                        return;
                    }
                }
                FileStream serializationStream = null;
                try
                {
                    string text = this.gpgTextBoxNewOutFile.Text;
                    List<Emote> graph = null;
                    if (File.Exists(text))
                    {
                        serializationStream = File.Open(text, FileMode.Open, FileAccess.ReadWrite);
                        graph = new BinaryFormatter().Deserialize(serializationStream) as List<Emote>;
                        serializationStream.Close();
                        if (graph == null)
                        {
                            base.Error(this.skinButtonCreate, "<LOC>File is invalid or corrupt.", new object[0]);
                            return;
                        }
                    }
                    else
                    {
                        serializationStream = File.Create(text);
                        serializationStream.Close();
                        graph = new List<Emote>();
                    }
                    Bitmap bitmap = new Bitmap(40, 20);
                    this.pictureBoxNewIcon.DrawToBitmap(bitmap, new Rectangle(0, 0, 40, 20));
                    Emote item = new Emote(bitmap, this.gpgTextBoxNewKeySeq.Text, text);
                    graph.Add(item);
                    serializationStream = File.OpenWrite(text);
                    new BinaryFormatter().Serialize(serializationStream, graph);
                    serializationStream.Close();
                    serializationStream = null;
                    Emote.AllEmotes[item.CharSequence] = item;
                    this.ResetForm();
                    this.RefreshEmotes();
                    Emote.OnEmotesChanged();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                finally
                {
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                    }
                }
            }
        }

        internal string[] RestrictedEmotes
        {
            get
            {
                if (this.mRestrictedEmotes == null)
                {
                    this.mRestrictedEmotes = ConfigSettings.GetString("RestrictedEmotes", "solution,chat,player,clan,help,http,emote,replay,game");
                }
                if (this.mRestrictedEmotes != null)
                {
                    return this.mRestrictedEmotes.Split(new char[] { ',' });
                }
                return new string[0];
            }
        }

        public PictureBox SelectedIcon
        {
            get
            {
                return this.mSelectedIcon;
            }
        }
    }
}

