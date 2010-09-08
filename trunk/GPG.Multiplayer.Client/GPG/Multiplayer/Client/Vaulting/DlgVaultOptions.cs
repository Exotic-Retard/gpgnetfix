namespace GPG.Multiplayer.Client.Vaulting
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public class DlgVaultOptions : DlgBase
    {
        private GPGPanel ActivePanel;
        private IContainer components;
        private GPGCheckBox gpgCheckBoxCachePreviews;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel4;
        private GPGPanel gpgPanelDownload;
        private GPGPanel gpgPanelUpload;
        private GPGTextBox gpgTextBoxMyDownloads;
        private ListBox listBoxUploadPaths;
        private UserPrefs_Content mVaultOptions;
        private SkinButton skinButtonAddUploadPath;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private SkinButton skinButtonRemoveUploadPath;
        private SkinButton skinButtonSelectMydownloads;
        private SkinButton skinButtonTabDownload;
        private SkinButton skinButtonTabUpload;

        public DlgVaultOptions() : this(VaultOptionTabs.Upload)
        {
        }

        public DlgVaultOptions(VaultOptionTabs tab)
        {
            this.mVaultOptions = null;
            this.ActivePanel = null;
            this.components = null;
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            this.listBoxUploadPaths.DataSource = Program.Settings.Content.Upload.UploadPaths;
            this.gpgTextBoxMyDownloads.Text = Program.Settings.Content.Download.MyDownloads;
            this.skinButtonTabDownload.Tag = VaultOptionTabs.Download;
            this.skinButtonTabDownload.Click += new EventHandler(this.ChangePanel);
            this.skinButtonTabUpload.Tag = VaultOptionTabs.Upload;
            this.skinButtonTabUpload.Click += new EventHandler(this.ChangePanel);
            this.skinButtonRemoveUploadPath.Enabled = this.listBoxUploadPaths.SelectedIndex >= 0;
            this.ChangePanel(tab);
            this.gpgCheckBoxCachePreviews.Checked = Program.Settings.Content.Download.CachePreviewImages;
            base.ttDefault.SetToolTip(this.gpgCheckBoxCachePreviews, Loc.Get("<LOC>Caches preview images on your local hard drive which speed up image retrieval in exchange for a small amount of disk space."));
        }

        private void ActivePanel_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Program.Settings.StylePreferences.HighlightColor3, 2f))
            {
                e.Graphics.DrawRectangle(pen, this.ActivePanel.ClientRectangle);
            }
        }

        private void ChangePanel(VaultOptionTabs tab)
        {
            if (this.ActivePanel != null)
            {
                this.ActivePanel.Paint -= new PaintEventHandler(this.ActivePanel_Paint);
            }
            if (tab == VaultOptionTabs.Upload)
            {
                this.ActivePanel = this.gpgPanelUpload;
            }
            this.ActivePanel.Paint += new PaintEventHandler(this.ActivePanel_Paint);
            this.ActivePanel.BringToFront();
            this.ActivePanel.Invalidate();
        }

        private void ChangePanel(object sender, EventArgs e)
        {
            this.ChangePanel((VaultOptionTabs) (sender as Control).Tag);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgCheckBoxCachePreviews_CheckedChanged(object sender, EventArgs e)
        {
            this.VaultOptions.Download.CachePreviewImages = this.gpgCheckBoxCachePreviews.Checked;
        }

        private void InitializeComponent()
        {
            this.gpgPanelUpload = new GPGPanel();
            this.gpgCheckBoxCachePreviews = new GPGCheckBox();
            this.gpgLabel2 = new GPGLabel();
            this.skinButtonRemoveUploadPath = new SkinButton();
            this.skinButtonAddUploadPath = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.listBoxUploadPaths = new ListBox();
            this.skinButtonTabUpload = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonTabDownload = new SkinButton();
            this.gpgPanelDownload = new GPGPanel();
            this.skinButtonSelectMydownloads = new SkinButton();
            this.gpgLabel4 = new GPGLabel();
            this.gpgTextBoxMyDownloads = new GPGTextBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgPanelUpload.SuspendLayout();
            this.gpgPanelDownload.SuspendLayout();
            this.gpgTextBoxMyDownloads.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x182, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgPanelUpload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelUpload.AutoScroll = true;
            this.gpgPanelUpload.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelUpload.BorderThickness = 2;
            this.gpgPanelUpload.Controls.Add(this.gpgCheckBoxCachePreviews);
            this.gpgPanelUpload.Controls.Add(this.gpgLabel2);
            this.gpgPanelUpload.Controls.Add(this.skinButtonRemoveUploadPath);
            this.gpgPanelUpload.Controls.Add(this.skinButtonAddUploadPath);
            this.gpgPanelUpload.Controls.Add(this.gpgLabel1);
            this.gpgPanelUpload.Controls.Add(this.listBoxUploadPaths);
            this.gpgPanelUpload.DrawBorder = false;
            this.gpgPanelUpload.Location = new Point(12, 0x53);
            this.gpgPanelUpload.Name = "gpgPanelUpload";
            this.gpgPanelUpload.Size = new Size(0x1a5, 280);
            base.ttDefault.SetSuperTip(this.gpgPanelUpload, null);
            this.gpgPanelUpload.TabIndex = 7;
            this.gpgCheckBoxCachePreviews.AutoSize = true;
            this.gpgCheckBoxCachePreviews.Location = new Point(6, 0x7b);
            this.gpgCheckBoxCachePreviews.Name = "gpgCheckBoxCachePreviews";
            this.gpgCheckBoxCachePreviews.Size = new Size(0xc6, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxCachePreviews, null);
            this.gpgCheckBoxCachePreviews.TabIndex = 5;
            this.gpgCheckBoxCachePreviews.Text = "<LOC>Cache preview images";
            this.gpgCheckBoxCachePreviews.UsesBG = false;
            this.gpgCheckBoxCachePreviews.UseVisualStyleBackColor = true;
            this.gpgCheckBoxCachePreviews.CheckedChanged += new EventHandler(this.gpgCheckBoxCachePreviews_CheckedChanged);
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0xc1, 6);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0xdd, 0x1f);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 4;
            this.gpgLabel2.Text = "<LOC>For best performance, do not use root directories";
            this.gpgLabel2.TextStyle = TextStyles.Descriptor;
            this.skinButtonRemoveUploadPath.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.skinButtonRemoveUploadPath.AutoStyle = true;
            this.skinButtonRemoveUploadPath.BackColor = Color.Black;
            this.skinButtonRemoveUploadPath.ButtonState = 0;
            this.skinButtonRemoveUploadPath.DialogResult = DialogResult.OK;
            this.skinButtonRemoveUploadPath.DisabledForecolor = Color.Gray;
            this.skinButtonRemoveUploadPath.DrawColor = Color.White;
            this.skinButtonRemoveUploadPath.DrawEdges = true;
            this.skinButtonRemoveUploadPath.FocusColor = Color.Yellow;
            this.skinButtonRemoveUploadPath.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRemoveUploadPath.ForeColor = Color.White;
            this.skinButtonRemoveUploadPath.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRemoveUploadPath.IsStyled = true;
            this.skinButtonRemoveUploadPath.Location = new Point(0x156, 0x59);
            this.skinButtonRemoveUploadPath.Name = "skinButtonRemoveUploadPath";
            this.skinButtonRemoveUploadPath.Size = new Size(0x48, 0x16);
            this.skinButtonRemoveUploadPath.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRemoveUploadPath, null);
            this.skinButtonRemoveUploadPath.TabIndex = 3;
            this.skinButtonRemoveUploadPath.TabStop = true;
            this.skinButtonRemoveUploadPath.Text = "<LOC>Remove";
            this.skinButtonRemoveUploadPath.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRemoveUploadPath.TextPadding = new Padding(0);
            this.skinButtonRemoveUploadPath.Click += new EventHandler(this.skinButtonRemoveUploadPath_Click);
            this.skinButtonAddUploadPath.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.skinButtonAddUploadPath.AutoStyle = true;
            this.skinButtonAddUploadPath.BackColor = Color.Black;
            this.skinButtonAddUploadPath.ButtonState = 0;
            this.skinButtonAddUploadPath.DialogResult = DialogResult.OK;
            this.skinButtonAddUploadPath.DisabledForecolor = Color.Gray;
            this.skinButtonAddUploadPath.DrawColor = Color.White;
            this.skinButtonAddUploadPath.DrawEdges = true;
            this.skinButtonAddUploadPath.FocusColor = Color.Yellow;
            this.skinButtonAddUploadPath.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAddUploadPath.ForeColor = Color.White;
            this.skinButtonAddUploadPath.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAddUploadPath.IsStyled = true;
            this.skinButtonAddUploadPath.Location = new Point(0x108, 0x59);
            this.skinButtonAddUploadPath.Name = "skinButtonAddUploadPath";
            this.skinButtonAddUploadPath.Size = new Size(0x48, 0x16);
            this.skinButtonAddUploadPath.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonAddUploadPath, null);
            this.skinButtonAddUploadPath.TabIndex = 2;
            this.skinButtonAddUploadPath.TabStop = true;
            this.skinButtonAddUploadPath.Text = "<LOC>Add";
            this.skinButtonAddUploadPath.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAddUploadPath.TextPadding = new Padding(0);
            this.skinButtonAddUploadPath.Click += new EventHandler(this.skinButtonAddUploadPath_Click);
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(3, 0x15);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x9c, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 1;
            this.gpgLabel1.Text = "<LOC>Upload Directories";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.listBoxUploadPaths.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.listBoxUploadPaths.FormattingEnabled = true;
            this.listBoxUploadPaths.Location = new Point(6, 40);
            this.listBoxUploadPaths.Name = "listBoxUploadPaths";
            this.listBoxUploadPaths.Size = new Size(0x198, 0x2b);
            base.ttDefault.SetSuperTip(this.listBoxUploadPaths, null);
            this.listBoxUploadPaths.TabIndex = 0;
            this.listBoxUploadPaths.SelectedIndexChanged += new EventHandler(this.listBoxUploadPaths_SelectedIndexChanged);
            this.skinButtonTabUpload.AutoStyle = true;
            this.skinButtonTabUpload.BackColor = Color.Black;
            this.skinButtonTabUpload.ButtonState = 0;
            this.skinButtonTabUpload.DialogResult = DialogResult.OK;
            this.skinButtonTabUpload.DisabledForecolor = Color.Gray;
            this.skinButtonTabUpload.DrawColor = Color.White;
            this.skinButtonTabUpload.DrawEdges = true;
            this.skinButtonTabUpload.FocusColor = Color.Yellow;
            this.skinButtonTabUpload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonTabUpload.ForeColor = Color.White;
            this.skinButtonTabUpload.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonTabUpload.IsStyled = true;
            this.skinButtonTabUpload.Location = new Point(0x69, 0x53);
            this.skinButtonTabUpload.Name = "skinButtonTabUpload";
            this.skinButtonTabUpload.Size = new Size(0x5d, 20);
            this.skinButtonTabUpload.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonTabUpload, null);
            this.skinButtonTabUpload.TabIndex = 8;
            this.skinButtonTabUpload.TabStop = true;
            this.skinButtonTabUpload.Text = "<LOC>Upload";
            this.skinButtonTabUpload.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonTabUpload.TextPadding = new Padding(0);
            this.skinButtonTabUpload.Visible = false;
            this.skinButtonTabUpload.Click += new EventHandler(this.ChangePanel);
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
            this.skinButtonOK.Location = new Point(0x101, 0x171);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x55, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 9;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
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
            this.skinButtonCancel.Location = new Point(0x15c, 0x171);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x55, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 11;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonTabDownload.AutoStyle = true;
            this.skinButtonTabDownload.BackColor = Color.Black;
            this.skinButtonTabDownload.ButtonState = 0;
            this.skinButtonTabDownload.DialogResult = DialogResult.OK;
            this.skinButtonTabDownload.DisabledForecolor = Color.Gray;
            this.skinButtonTabDownload.DrawColor = Color.White;
            this.skinButtonTabDownload.DrawEdges = true;
            this.skinButtonTabDownload.FocusColor = Color.Yellow;
            this.skinButtonTabDownload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonTabDownload.ForeColor = Color.White;
            this.skinButtonTabDownload.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonTabDownload.IsStyled = true;
            this.skinButtonTabDownload.Location = new Point(12, 0x53);
            this.skinButtonTabDownload.Name = "skinButtonTabDownload";
            this.skinButtonTabDownload.Size = new Size(0x5d, 20);
            this.skinButtonTabDownload.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonTabDownload, null);
            this.skinButtonTabDownload.TabIndex = 9;
            this.skinButtonTabDownload.TabStop = true;
            this.skinButtonTabDownload.Text = "<LOC>Download";
            this.skinButtonTabDownload.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonTabDownload.TextPadding = new Padding(0);
            this.skinButtonTabDownload.Visible = false;
            this.gpgPanelDownload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelDownload.AutoScroll = true;
            this.gpgPanelDownload.BackColor = Color.Transparent;
            this.gpgPanelDownload.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelDownload.BorderThickness = 2;
            this.gpgPanelDownload.Controls.Add(this.skinButtonSelectMydownloads);
            this.gpgPanelDownload.Controls.Add(this.gpgLabel4);
            this.gpgPanelDownload.Controls.Add(this.gpgTextBoxMyDownloads);
            this.gpgPanelDownload.DrawBorder = false;
            this.gpgPanelDownload.Location = new Point(12, 0x67);
            this.gpgPanelDownload.Name = "gpgPanelDownload";
            this.gpgPanelDownload.Size = new Size(0x1a5, 260);
            base.ttDefault.SetSuperTip(this.gpgPanelDownload, null);
            this.gpgPanelDownload.TabIndex = 12;
            this.skinButtonSelectMydownloads.AutoStyle = true;
            this.skinButtonSelectMydownloads.BackColor = Color.Black;
            this.skinButtonSelectMydownloads.ButtonState = 0;
            this.skinButtonSelectMydownloads.DialogResult = DialogResult.OK;
            this.skinButtonSelectMydownloads.DisabledForecolor = Color.Gray;
            this.skinButtonSelectMydownloads.DrawColor = Color.White;
            this.skinButtonSelectMydownloads.DrawEdges = true;
            this.skinButtonSelectMydownloads.FocusColor = Color.Yellow;
            this.skinButtonSelectMydownloads.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSelectMydownloads.ForeColor = Color.White;
            this.skinButtonSelectMydownloads.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSelectMydownloads.IsStyled = true;
            this.skinButtonSelectMydownloads.Location = new Point(0x18a, 40);
            this.skinButtonSelectMydownloads.Margin = new Padding(0);
            this.skinButtonSelectMydownloads.Name = "skinButtonSelectMydownloads";
            this.skinButtonSelectMydownloads.Size = new Size(20, 20);
            this.skinButtonSelectMydownloads.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSelectMydownloads, null);
            this.skinButtonSelectMydownloads.TabIndex = 11;
            this.skinButtonSelectMydownloads.TabStop = true;
            this.skinButtonSelectMydownloads.Text = "...";
            this.skinButtonSelectMydownloads.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSelectMydownloads.TextPadding = new Padding(0);
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(3, 0x15);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x77, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 10;
            this.gpgLabel4.Text = "<LOC>My Uploads";
            this.gpgLabel4.TextStyle = TextStyles.Bold;
            this.gpgTextBoxMyDownloads.Location = new Point(6, 40);
            this.gpgTextBoxMyDownloads.Name = "gpgTextBoxMyDownloads";
            this.gpgTextBoxMyDownloads.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxMyDownloads.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxMyDownloads.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxMyDownloads.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxMyDownloads.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxMyDownloads.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxMyDownloads.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxMyDownloads.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxMyDownloads.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxMyDownloads.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxMyDownloads.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxMyDownloads.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxMyDownloads.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxMyDownloads.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxMyDownloads.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxMyDownloads.Size = new Size(0x181, 20);
            this.gpgTextBoxMyDownloads.TabIndex = 9;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1bd, 0x1c4);
            base.Controls.Add(this.gpgPanelUpload);
            base.Controls.Add(this.gpgPanelDownload);
            base.Controls.Add(this.skinButtonTabDownload);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonTabUpload);
            base.Controls.Add(this.skinButtonOK);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x1bd, 0x1c4);
            base.Name = "DlgVaultOptions";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Vault Options";
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonTabUpload, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonTabDownload, 0);
            base.Controls.SetChildIndex(this.gpgPanelDownload, 0);
            base.Controls.SetChildIndex(this.gpgPanelUpload, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgPanelUpload.ResumeLayout(false);
            this.gpgPanelUpload.PerformLayout();
            this.gpgPanelDownload.ResumeLayout(false);
            this.gpgPanelDownload.PerformLayout();
            this.gpgTextBoxMyDownloads.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listBoxUploadPaths_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.skinButtonRemoveUploadPath.Enabled = this.listBoxUploadPaths.SelectedIndex >= 0;
        }

        private void skinButtonAddUploadPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!this.VaultOptions.Upload.UploadPaths.Contains(dialog.SelectedPath))
                {
                    this.VaultOptions.Upload.UploadPaths.Add(dialog.SelectedPath);
                }
                this.listBoxUploadPaths.DataSource = null;
                this.listBoxUploadPaths.DataSource = this.VaultOptions.Upload.UploadPaths;
            }
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            if (this.mVaultOptions != null)
            {
                Program.Settings.Content = this.VaultOptions;
                Program.Settings.Save();
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void skinButtonRemoveUploadPath_Click(object sender, EventArgs e)
        {
            if (this.listBoxUploadPaths.SelectedIndex >= 0)
            {
                this.VaultOptions.Upload.UploadPaths.RemoveAt(this.listBoxUploadPaths.SelectedIndex);
                this.listBoxUploadPaths.DataSource = null;
                this.listBoxUploadPaths.DataSource = this.VaultOptions.Upload.UploadPaths;
            }
        }

        private UserPrefs_Content VaultOptions
        {
            get
            {
                if (this.mVaultOptions == null)
                {
                    MemoryStream serializationStream = null;
                    try
                    {
                        serializationStream = new MemoryStream();
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(serializationStream, Program.Settings.Content);
                        serializationStream.Position = 0L;
                        this.mVaultOptions = formatter.Deserialize(serializationStream) as UserPrefs_Content;
                    }
                    finally
                    {
                        if (serializationStream != null)
                        {
                            serializationStream.Close();
                        }
                    }
                }
                return this.mVaultOptions;
            }
        }
    }
}

