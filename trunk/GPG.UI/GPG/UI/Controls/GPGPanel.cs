namespace GPG.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGPanel : Panel
    {
        private Color mBorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
        private int mBorderThickness = 2;
        private bool mDrawBorder;

        public GPGPanel()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.DrawBorder)
            {
                using (Pen pen = new Pen(this.BorderColor, (float) (this.BorderThickness * 2)))
                {
                    e.Graphics.DrawRectangle(pen, base.ClientRectangle);
                }
            }
        }

        [Browsable(true)]
        public Color BorderColor
        {
            get
            {
                return this.mBorderColor;
            }
            set
            {
                this.mBorderColor = value;
                this.Refresh();
            }
        }

        [Browsable(false)]
        private System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return System.Windows.Forms.BorderStyle.None;
            }
        }

        [Browsable(true)]
        public int BorderThickness
        {
            get
            {
                return this.mBorderThickness;
            }
            set
            {
                this.mBorderThickness = value;
                this.Refresh();
            }
        }

        [Browsable(true)]
        public bool DrawBorder
        {
            get
            {
                return this.mDrawBorder;
            }
            set
            {
                this.mDrawBorder = value;
                this.Refresh();
            }
        }
    }
}

