namespace GPG.Multiplayer.Client.Vaulting.Videos
{
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PnlVideoDetailsView : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabelLength;
        private GPGLabel gpgLabelQuality;
        private GPGLabel gpgLabelVideoFormat;
        private GPG.Multiplayer.Client.Vaulting.Videos.Video mVideo;
        private PictureBox pictureBoxPreview;

        public PnlVideoDetailsView(GPG.Multiplayer.Client.Vaulting.Videos.Video video)
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.PnlVideoDetailsView_Disposed);
            GPG.Multiplayer.Client.Vaulting.Videos.Video.PreviewImageLoaded += new EventHandler(this.Video_PreviewImageLoaded);
            this.gpgLabelVideoFormat.Text = video.VideoFormat;
            this.gpgLabelLength.Text = video.LengthDisplay;
            this.gpgLabelQuality.Text = video.QualityDisplay;
            this.pictureBoxPreview.Image = video.PreviewImage128;
            this.mVideo = video;
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
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelVideoFormat = new GPGLabel();
            this.pictureBoxPreview = new PictureBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabelQuality = new GPGLabel();
            this.gpgLabelLength = new GPGLabel();
            ((ISupportInitialize) this.pictureBoxPreview).BeginInit();
            base.SuspendLayout();
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0xc9, 0);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x21b, 0x11);
            this.gpgLabel6.TabIndex = 0x10;
            this.gpgLabel6.Text = "<LOC>Video Quality";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0xc9, 0x59);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x21b, 0x11);
            this.gpgLabel1.TabIndex = 0x1c;
            this.gpgLabel1.Text = "<LOC>Video Length";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgLabelVideoFormat.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelVideoFormat.AutoSize = true;
            this.gpgLabelVideoFormat.AutoStyle = true;
            this.gpgLabelVideoFormat.Font = new Font("Arial", 9.75f);
            this.gpgLabelVideoFormat.ForeColor = Color.White;
            this.gpgLabelVideoFormat.IgnoreMouseWheel = false;
            this.gpgLabelVideoFormat.IsStyled = false;
            this.gpgLabelVideoFormat.Location = new Point(0xcf, 0x40);
            this.gpgLabelVideoFormat.Name = "gpgLabelVideoFormat";
            this.gpgLabelVideoFormat.Size = new Size(0x2c, 0x10);
            this.gpgLabelVideoFormat.TabIndex = 0x26;
            this.gpgLabelVideoFormat.Text = "format";
            this.gpgLabelVideoFormat.TextStyle = TextStyles.Default;
            this.pictureBoxPreview.Location = new Point(3, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new Size(0xc0, 0xc0);
            this.pictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreview.TabIndex = 0x1a;
            this.pictureBoxPreview.TabStop = false;
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0xc9, 0x2f);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x21b, 0x11);
            this.gpgLabel2.TabIndex = 0x27;
            this.gpgLabel2.Text = "<LOC>Video Format";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgLabelQuality.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelQuality.AutoSize = true;
            this.gpgLabelQuality.AutoStyle = true;
            this.gpgLabelQuality.Font = new Font("Arial", 9.75f);
            this.gpgLabelQuality.ForeColor = Color.White;
            this.gpgLabelQuality.IgnoreMouseWheel = false;
            this.gpgLabelQuality.IsStyled = false;
            this.gpgLabelQuality.Location = new Point(0xcf, 0x11);
            this.gpgLabelQuality.Name = "gpgLabelQuality";
            this.gpgLabelQuality.Size = new Size(0x2c, 0x10);
            this.gpgLabelQuality.TabIndex = 40;
            this.gpgLabelQuality.Text = "format";
            this.gpgLabelQuality.TextStyle = TextStyles.Default;
            this.gpgLabelLength.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelLength.AutoSize = true;
            this.gpgLabelLength.AutoStyle = true;
            this.gpgLabelLength.Font = new Font("Arial", 9.75f);
            this.gpgLabelLength.ForeColor = Color.White;
            this.gpgLabelLength.IgnoreMouseWheel = false;
            this.gpgLabelLength.IsStyled = false;
            this.gpgLabelLength.Location = new Point(0xcf, 0x6f);
            this.gpgLabelLength.Name = "gpgLabelLength";
            this.gpgLabelLength.Size = new Size(0x2c, 0x10);
            this.gpgLabelLength.TabIndex = 0x29;
            this.gpgLabelLength.Text = "format";
            this.gpgLabelLength.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLabelLength);
            base.Controls.Add(this.gpgLabelQuality);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabelVideoFormat);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.pictureBoxPreview);
            base.Controls.Add(this.gpgLabel6);
            base.Name = "PnlVideoDetailsView";
            base.Size = new Size(0x2db, 0xc3);
            ((ISupportInitialize) this.pictureBoxPreview).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PnlVideoDetailsView_Disposed(object sender, EventArgs e)
        {
            GPG.Multiplayer.Client.Vaulting.Videos.Video.PreviewImageLoaded -= new EventHandler(this.Video_PreviewImageLoaded);
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

        public GPG.Multiplayer.Client.Vaulting.Videos.Video Video
        {
            get
            {
                return this.mVideo;
            }
        }
    }
}

