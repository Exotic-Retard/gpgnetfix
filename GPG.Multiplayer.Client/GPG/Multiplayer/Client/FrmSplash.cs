namespace GPG.Multiplayer.Client
{
    using GPG.Multiplayer.Client.Properties;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class FrmSplash : Form
    {
        private IContainer components = null;
        private Label lVersion;
        public static FrmSplash sFrmSplash = null;

        public FrmSplash()
        {
            this.InitializeComponent();
            sFrmSplash = this;
            this.lVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            DrawUtil.MakeTransparent(this, (Bitmap) this.BackgroundImage, Color.Black);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmSplash_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.lVersion = new Label();
            base.SuspendLayout();
            this.lVersion.BackColor = Color.Transparent;
            this.lVersion.Font = new Font("Verdana", 6f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lVersion.Location = new Point(0x236, 240);
            this.lVersion.Name = "lVersion";
            this.lVersion.Size = new Size(0x47, 13);
            this.lVersion.TabIndex = 1;
            this.lVersion.Text = "1.5.90.1";
            this.lVersion.TextAlign = ContentAlignment.MiddleCenter;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            this.BackgroundImage = Resources.gpgnet_splash;
            this.BackgroundImageLayout = ImageLayout.None;
            base.ClientSize = new Size(0x281, 0x101);
            base.Controls.Add(this.lVersion);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "FrmSplash";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "FrmSplash";
            base.Load += new EventHandler(this.FrmSplash_Load);
            base.ResumeLayout(false);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            sFrmSplash = null;
            base.OnClosing(e);
        }
    }
}

