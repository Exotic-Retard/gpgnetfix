namespace GPG.UI.Controls.Docking
{
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DockSnapshotForm : Form
    {
        private IContainer components;
        private GPGPictureBox gpgPictureBoxSnapshot;

        public DockSnapshotForm(Image snapshot, Point loc)
        {
            EventHandler handler = null;
            this.InitializeComponent();
            this.gpgPictureBoxSnapshot.Image = snapshot;
            if (handler == null)
            {
                handler = delegate (object s, EventArgs e) {
                    this.gpgPictureBoxSnapshot.Image.Dispose();
                };
            }
            base.Disposed += handler;
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
            this.gpgPictureBoxSnapshot = new GPGPictureBox();
            ((ISupportInitialize) this.gpgPictureBoxSnapshot).BeginInit();
            base.SuspendLayout();
            this.gpgPictureBoxSnapshot.Dock = DockStyle.Fill;
            this.gpgPictureBoxSnapshot.Location = new Point(0, 0);
            this.gpgPictureBoxSnapshot.Margin = new Padding(0);
            this.gpgPictureBoxSnapshot.Name = "gpgPictureBoxSnapshot";
            this.gpgPictureBoxSnapshot.Size = new Size(0x40, 0x40);
            this.gpgPictureBoxSnapshot.SizeMode = PictureBoxSizeMode.StretchImage;
            this.gpgPictureBoxSnapshot.TabIndex = 0;
            this.gpgPictureBoxSnapshot.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x40, 0x40);
            base.Controls.Add(this.gpgPictureBoxSnapshot);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "DockSnapshotForm";
            base.Opacity = 0.5;
            this.Text = "DockSnapshotForm";
            base.TopMost = true;
            ((ISupportInitialize) this.gpgPictureBoxSnapshot).EndInit();
            base.ResumeLayout(false);
        }

        protected override void OnShown(EventArgs e)
        {
            base.Size = this.gpgPictureBoxSnapshot.Image.Size;
            base.Location = Cursor.Position;
            base.OnShown(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        internal void SetSnapshot(Image snapshot)
        {
            this.gpgPictureBoxSnapshot.Image.Dispose();
            this.gpgPictureBoxSnapshot.Image = null;
            this.gpgPictureBoxSnapshot.Image = snapshot;
        }
    }
}

