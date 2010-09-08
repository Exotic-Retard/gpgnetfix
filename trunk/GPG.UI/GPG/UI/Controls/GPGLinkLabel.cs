namespace GPG.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGLinkLabel : LinkLabel
    {
        private IContainer components;

        public GPGLinkLabel()
        {
            this.InitializeComponent();
            this.Font = new Font("Arial", 9.75f);
            base.LinkColor = Color.FromArgb(0xc0, 0xff, 0xc0);
            base.VisitedLinkColor = Color.FromArgb(0xc0, 0xff, 0xc0);
            base.ActiveLinkColor = Color.FromArgb(0xc0, 0xff, 0xc0);
            this.ForeColor = Color.FromArgb(0xc0, 0xff, 0xc0);
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
            this.components = new Container();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}

