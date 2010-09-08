namespace GPG.UI.Controls
{
    using GPG;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGPictureBox : PictureBox
    {
        private IContainer components;

        public GPGPictureBox()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void DoMouseMove(Point pt)
        {
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            VGen0 method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.OnPaint(pe);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                base.OnPaint(pe);
            }
        }
    }
}

