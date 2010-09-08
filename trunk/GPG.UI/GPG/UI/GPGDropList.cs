namespace GPG.UI
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGDropList : Control
    {
        private IContainer components;
        private bool mDragging;
        private int mOverLeft;
        private int mOverRight;

        public GPGDropList()
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

        public void DragOver(int screenX, int screenY)
        {
            Rectangle bounds = Screen.GetBounds(this);
            this.mOverLeft = screenX - bounds.X;
            this.mOverRight = screenY - bounds.Y;
            this.mDragging = true;
            base.Invalidate();
        }

        public void EndDrag()
        {
            this.mDragging = false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics graphics = pe.Graphics;
            Brush brush = new SolidBrush(Color.White);
            if (this.mDragging)
            {
                graphics.FillEllipse(brush, this.mOverLeft, this.mOverRight, 20, 20);
            }
            brush.Dispose();
        }
    }
}

