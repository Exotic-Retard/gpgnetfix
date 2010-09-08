namespace GPG.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGBorderPanel : Panel
    {
        private IContainer components;
        private GPG.UI.Controls.GPGBorderStyle mGPGBorderStyle;

        public GPGBorderPanel()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.ContainerControl, true);
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
            Brush brush;
            Pen pen;
            base.OnPaint(pe);
            Graphics graphics = pe.Graphics;
            if (base.Enabled)
            {
                brush = new SolidBrush(Color.FromArgb(0x98, 0x9a, 0xff));
                pen = new Pen(brush, 2f);
            }
            else
            {
                brush = new SolidBrush(Color.FromArgb(0x4b, 0x4b, 0x4b));
                pen = new Pen(brush, 2f);
            }
            List<Point> list = new List<Point>();
            if (this.GPGBorderStyle == GPG.UI.Controls.GPGBorderStyle.Web)
            {
                list.Add(new Point(1, 4));
                list.Add(new Point(4, 1));
                list.Add(new Point(base.Width - 4, 1));
                list.Add(new Point(base.Width - 1, 4));
                list.Add(new Point(base.Width - 1, base.Height - 20));
                list.Add(new Point(base.Width - 4, base.Height - 0x11));
                list.Add(new Point(base.Width - 0x43, base.Height - 0x11));
                list.Add(new Point(base.Width - 0x47, base.Height - 1));
                list.Add(new Point(4, base.Height - 1));
                list.Add(new Point(1, base.Height - 4));
                list.Add(new Point(1, 4));
            }
            else
            {
                list.Add(new Point(1, 1));
                list.Add(new Point(base.Width - 1, 1));
                list.Add(new Point(base.Width - 1, base.Height - 1));
                list.Add(new Point(1, base.Height - 1));
                list.Add(new Point(1, 1));
            }
            graphics.DrawPolygon(pen, list.ToArray());
            brush.Dispose();
            brush.Dispose();
        }

        public GPG.UI.Controls.GPGBorderStyle GPGBorderStyle
        {
            get
            {
                return this.mGPGBorderStyle;
            }
            set
            {
                this.mGPGBorderStyle = value;
            }
        }
    }
}

