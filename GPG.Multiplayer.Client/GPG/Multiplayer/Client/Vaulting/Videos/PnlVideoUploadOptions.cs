namespace GPG.Multiplayer.Client.Vaulting.Videos
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class PnlVideoUploadOptions : PnlBase
    {
        private ComboBox comboBoxQuality;
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabelVideoFormat;
        private GPGTextBox gpgTextBoxPreview;
        private GPG.Multiplayer.Client.Vaulting.Videos.Video mVideo;
        private NumericUpDown numericUpDownHours;
        private NumericUpDown numericUpDownMinutes;
        private NumericUpDown numericUpDownSeconds;
        private PictureBox pictureBoxPreview;
        private SkinButton skinButtonPreview;

        public PnlVideoUploadOptions(GPG.Multiplayer.Client.Vaulting.Videos.Video video)
        {
            this.InitializeComponent();
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Unspecified"), "<LOC>Unspecified"));
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Low"), "<LOC>Low"));
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Medium"), "<LOC>Medium"));
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>High"), "<LOC>High"));
            this.comboBoxQuality.SelectedIndex = 0;
            for (int i = 0; i < this.comboBoxQuality.Items.Count; i++)
            {
                if ((this.comboBoxQuality.Items[i] as MultiVal<string, string>).Value2 == video.Quality)
                {
                    this.comboBoxQuality.SelectedIndex = i;
                    break;
                }
            }
            this.gpgLabelVideoFormat.Text = video.VideoFormat;
            this.numericUpDownHours.Value = video.LengthHours;
            this.numericUpDownMinutes.Value = video.LengthMinutes;
            this.numericUpDownMinutes.Value = video.LengthSeconds;
            this.mVideo = video;
            base.Disposed += new EventHandler(this.PnlVideoDetailsView_Disposed);
            GPG.Multiplayer.Client.Vaulting.Videos.Video.PreviewImageLoaded += new EventHandler(this.Video_PreviewImageLoaded);
            this.VideoAttributeChanged(null, null);
            this.gpgTextBoxPreview_TextChanged(null, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgTextBoxPreview_TextChanged(object sender, EventArgs e)
        {
            if (this.Video != null)
            {
                try
                {
                    this.Video.LocalPreviewImagePath = this.gpgTextBoxPreview.Text;
                    this.pictureBoxPreview.Image = this.Video.PreviewImage128;
                }
                catch
                {
                    this.Video.LocalPreviewImagePath = null;
                    this.pictureBoxPreview.Image = null;
                }
            }
        }

        private void InitializeComponent()
        {
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxPreview = new GPGTextBox();
            this.comboBoxQuality = new ComboBox();
            this.pictureBoxPreview = new PictureBox();
            this.skinButtonPreview = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.numericUpDownMinutes = new NumericUpDown();
            this.numericUpDownSeconds = new NumericUpDown();
            this.numericUpDownHours = new NumericUpDown();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabelVideoFormat = new GPGLabel();
            this.gpgTextBoxPreview.Properties.BeginInit();
            ((ISupportInitialize) this.pictureBoxPreview).BeginInit();
            this.numericUpDownMinutes.BeginInit();
            this.numericUpDownSeconds.BeginInit();
            this.numericUpDownHours.BeginInit();
            base.SuspendLayout();
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0xc0, 0x36);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x18e, 0x11);
            this.gpgLabel6.TabIndex = 0x10;
            this.gpgLabel6.Text = "<LOC>Video Quality";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0xc0, 0);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x18e, 0x11);
            this.gpgLabel2.TabIndex = 0x13;
            this.gpgLabel2.Text = "<LOC>Preview Image";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgTextBoxPreview.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxPreview.Location = new Point(0xc6, 20);
            this.gpgTextBoxPreview.Name = "gpgTextBoxPreview";
            this.gpgTextBoxPreview.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPreview.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPreview.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPreview.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPreview.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPreview.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPreview.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPreview.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPreview.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPreview.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPreview.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPreview.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPreview.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPreview.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPreview.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPreview.Properties.MaxLength = 0xff;
            this.gpgTextBoxPreview.Size = new Size(0x157, 20);
            this.gpgTextBoxPreview.TabIndex = 20;
            this.gpgTextBoxPreview.TextChanged += new EventHandler(this.gpgTextBoxPreview_TextChanged);
            this.comboBoxQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxQuality.FormattingEnabled = true;
            this.comboBoxQuality.Location = new Point(0xc6, 0x4a);
            this.comboBoxQuality.Name = "comboBoxQuality";
            this.comboBoxQuality.Size = new Size(0xc6, 0x15);
            this.comboBoxQuality.TabIndex = 0x19;
            this.comboBoxQuality.SelectedIndexChanged += new EventHandler(this.VideoAttributeChanged);
            this.pictureBoxPreview.Location = new Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new Size(0xc0, 0xc0);
            this.pictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreview.TabIndex = 0x1a;
            this.pictureBoxPreview.TabStop = false;
            this.skinButtonPreview.AutoStyle = true;
            this.skinButtonPreview.BackColor = Color.Transparent;
            this.skinButtonPreview.ButtonState = 0;
            this.skinButtonPreview.DialogResult = DialogResult.OK;
            this.skinButtonPreview.DisabledForecolor = Color.Gray;
            this.skinButtonPreview.DrawColor = Color.White;
            this.skinButtonPreview.DrawEdges = true;
            this.skinButtonPreview.FocusColor = Color.Yellow;
            this.skinButtonPreview.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonPreview.ForeColor = Color.White;
            this.skinButtonPreview.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonPreview.IsStyled = true;
            this.skinButtonPreview.Location = new Point(0x223, 20);
            this.skinButtonPreview.Name = "skinButtonPreview";
            this.skinButtonPreview.Size = new Size(20, 20);
            this.skinButtonPreview.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonPreview.TabIndex = 0x1b;
            this.skinButtonPreview.TabStop = true;
            this.skinButtonPreview.Text = "...";
            this.skinButtonPreview.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonPreview.TextPadding = new Padding(0);
            this.skinButtonPreview.Click += new EventHandler(this.skinButtonPreview_Click);
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0xc0, 0x6d);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x18e, 0x11);
            this.gpgLabel1.TabIndex = 0x1c;
            this.gpgLabel1.Text = "<LOC>Video Length";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(300, 0x7e);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x60, 0x10);
            this.gpgLabel3.TabIndex = 0x1f;
            this.gpgLabel3.Text = "<LOC>Minutes";
            this.gpgLabel3.TextAlign = ContentAlignment.BottomLeft;
            this.gpgLabel3.TextStyle = TextStyles.Small;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x192, 0x7e);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x65, 0x10);
            this.gpgLabel4.TabIndex = 0x20;
            this.gpgLabel4.Text = "<LOC>Seconds";
            this.gpgLabel4.TextAlign = ContentAlignment.BottomLeft;
            this.gpgLabel4.TextStyle = TextStyles.Small;
            this.numericUpDownMinutes.Location = new Point(0x12f, 0x91);
            int[] bits = new int[4];
            bits[0] = 0x3b;
            this.numericUpDownMinutes.Maximum = new decimal(bits);
            this.numericUpDownMinutes.Name = "numericUpDownMinutes";
            this.numericUpDownMinutes.Size = new Size(0x5d, 20);
            this.numericUpDownMinutes.TabIndex = 0x21;
            this.numericUpDownMinutes.ValueChanged += new EventHandler(this.VideoAttributeChanged);
            this.numericUpDownSeconds.Location = new Point(410, 0x91);
            bits = new int[4];
            bits[0] = 0x3b;
            this.numericUpDownSeconds.Maximum = new decimal(bits);
            this.numericUpDownSeconds.Name = "numericUpDownSeconds";
            this.numericUpDownSeconds.Size = new Size(0x5d, 20);
            this.numericUpDownSeconds.TabIndex = 0x22;
            this.numericUpDownSeconds.ValueChanged += new EventHandler(this.VideoAttributeChanged);
            this.numericUpDownHours.Location = new Point(0xc6, 0x91);
            bits = new int[4];
            bits[0] = 6;
            this.numericUpDownHours.Maximum = new decimal(bits);
            this.numericUpDownHours.Name = "numericUpDownHours";
            this.numericUpDownHours.Size = new Size(0x5d, 20);
            this.numericUpDownHours.TabIndex = 0x24;
            this.numericUpDownHours.ValueChanged += new EventHandler(this.VideoAttributeChanged);
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0xc3, 0x7e);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x54, 0x10);
            this.gpgLabel5.TabIndex = 0x23;
            this.gpgLabel5.Text = "<LOC>Hours";
            this.gpgLabel5.TextAlign = ContentAlignment.BottomLeft;
            this.gpgLabel5.TextStyle = TextStyles.Small;
            this.gpgLabel7.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel7.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x1a5, 0x36);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0xa6, 0x11);
            this.gpgLabel7.TabIndex = 0x25;
            this.gpgLabel7.Text = "<LOC>Video Format";
            this.gpgLabel7.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel7.TextStyle = TextStyles.Custom;
            this.gpgLabelVideoFormat.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelVideoFormat.AutoSize = true;
            this.gpgLabelVideoFormat.AutoStyle = true;
            this.gpgLabelVideoFormat.Font = new Font("Arial", 9.75f);
            this.gpgLabelVideoFormat.ForeColor = Color.White;
            this.gpgLabelVideoFormat.IgnoreMouseWheel = false;
            this.gpgLabelVideoFormat.IsStyled = false;
            this.gpgLabelVideoFormat.Location = new Point(420, 0x4b);
            this.gpgLabelVideoFormat.Name = "gpgLabelVideoFormat";
            this.gpgLabelVideoFormat.Size = new Size(0x2c, 0x10);
            this.gpgLabelVideoFormat.TabIndex = 0x26;
            this.gpgLabelVideoFormat.Text = "format";
            this.gpgLabelVideoFormat.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLabelVideoFormat);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.numericUpDownHours);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.numericUpDownSeconds);
            base.Controls.Add(this.numericUpDownMinutes);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.skinButtonPreview);
            base.Controls.Add(this.pictureBoxPreview);
            base.Controls.Add(this.comboBoxQuality);
            base.Controls.Add(this.gpgTextBoxPreview);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel6);
            base.Name = "PnlVideoUploadOptions";
            base.Size = new Size(590, 0xc3);
            this.gpgTextBoxPreview.Properties.EndInit();
            ((ISupportInitialize) this.pictureBoxPreview).EndInit();
            this.numericUpDownMinutes.EndInit();
            this.numericUpDownSeconds.EndInit();
            this.numericUpDownHours.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PnlVideoDetailsView_Disposed(object sender, EventArgs e)
        {
            GPG.Multiplayer.Client.Vaulting.Videos.Video.PreviewImageLoaded -= new EventHandler(this.Video_PreviewImageLoaded);
        }

        private void skinButtonPreview_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = string.Format(Loc.Get("<LOC>Image Files (*.bmp;*.jpg;*.gif;*.png){0}"), "|*.bmp;*.jpg;*.gif;*.png");
            dialog.Multiselect = false;
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.gpgTextBoxPreview.Text = dialog.FileName;
            }
        }

        private void Video_PreviewImageLoaded(object sender, EventArgs e)
        {
            VGen0 method = null;
            GPG.Multiplayer.Client.Vaulting.Videos.Video video;
            if ((!base.Disposing && !base.IsDisposed) && (this.Video != null))
            {
                video = sender as GPG.Multiplayer.Client.Vaulting.Videos.Video;
                if (video.ID == this.Video.ID)
                {
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.pictureBoxPreview.Image = video.PreviewImage128;
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (!(base.Disposing || base.IsDisposed))
                    {
                        this.pictureBoxPreview.Image = video.PreviewImage128;
                    }
                }
            }
        }

        private void VideoAttributeChanged(object sender, EventArgs e)
        {
            if (this.Video != null)
            {
                this.Video.Quality = (this.comboBoxQuality.SelectedItem as MultiVal<string, string>).Value2;
                this.Video.LengthHours = (int) this.numericUpDownHours.Value;
                this.Video.LengthMinutes = (int) this.numericUpDownMinutes.Value;
                this.Video.LengthSeconds = (int) this.numericUpDownSeconds.Value;
            }
        }

        public GPG.Multiplayer.Client.Vaulting.Videos.Video Video
        {
            get
            {
                return this.mVideo;
            }
        }
    }
}

