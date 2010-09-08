namespace GPG.Multiplayer.Client
{
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgPictureView : DlgBase
    {
        private IContainer components = null;
        private GPGButton gpgButtonOK;
        private GPGPictureBox gpgPictureBoxImage;
        private System.Drawing.Image mImage = null;

        public DlgPictureView(System.Drawing.Image img)
        {
            this.InitializeComponent();
            this.mImage = img;
            Size size = this.gpgPictureBoxImage.Size;
            this.gpgPictureBoxImage.Image = this.Image;
            if (this.Image.Size.Width > this.gpgPictureBoxImage.Size.Width)
            {
                this.gpgPictureBoxImage.Width = this.Image.Size.Width;
            }
            if (this.Image.Size.Height > this.gpgPictureBoxImage.Size.Height)
            {
                this.gpgPictureBoxImage.Height = this.Image.Size.Height;
            }
            Size size2 = this.gpgPictureBoxImage.Size - size;
            base.Size += size2;
            this.MaximumSize = base.Size;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgButtonOK_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void InitializeComponent()
        {
            this.gpgPictureBoxImage = new GPGPictureBox();
            this.gpgButtonOK = new GPGButton();
            ((ISupportInitialize) this.gpgPictureBoxImage).BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgPictureBoxImage.Location = new Point(12, 0x53);
            this.gpgPictureBoxImage.Name = "gpgPictureBoxImage";
            this.gpgPictureBoxImage.Size = new Size(0x11a, 0x73);
            this.gpgPictureBoxImage.SizeMode = PictureBoxSizeMode.CenterImage;
            this.gpgPictureBoxImage.TabIndex = 4;
            this.gpgPictureBoxImage.TabStop = false;
            this.gpgButtonOK.Anchor = AnchorStyles.Bottom;
            this.gpgButtonOK.Appearance.ForeColor = Color.Black;
            this.gpgButtonOK.Appearance.Options.UseForeColor = true;
            this.gpgButtonOK.DialogResult = DialogResult.Cancel;
            this.gpgButtonOK.Location = new Point(0x77, 0xd7);
            this.gpgButtonOK.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgButtonOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgButtonOK.Name = "gpgButtonOK";
            this.gpgButtonOK.Size = new Size(0x45, 0x17);
            this.gpgButtonOK.TabIndex = 5;
            this.gpgButtonOK.Text = "<LOC>OK";
            this.gpgButtonOK.UseVisualStyleBackColor = false;
            this.gpgButtonOK.Click += new EventHandler(this.gpgButtonOK_Click);
            base.AcceptButton = this.gpgButtonOK;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.gpgButtonOK;
            base.ClientSize = new Size(0x132, 0x120);
            base.Controls.Add(this.gpgPictureBoxImage);
            base.Controls.Add(this.gpgButtonOK);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x132, 0x120);
            base.Name = "DlgPictureView";
            this.Text = "<LOC>Picture Viewer";
            base.Controls.SetChildIndex(this.gpgButtonOK, 0);
            base.Controls.SetChildIndex(this.gpgPictureBoxImage, 0);
            ((ISupportInitialize) this.gpgPictureBoxImage).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.mImage;
            }
        }
    }
}

