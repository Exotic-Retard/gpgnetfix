namespace GPG.UI.Controls
{
    using GPG.UI.Controls.Skinning;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class GPGCheckBox : CheckBox
    {
        private IContainer components;
        private SkinManager mManager = SkinManager.Default;
        private bool mUsesBG;

        public GPGCheckBox()
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

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (!base.DesignMode && this.mUsesBG)
            {
                Brush brush;
                Image image;
                Graphics graphics = pe.Graphics;
                if (this.BackColor == Color.Transparent)
                {
                    brush = new SolidBrush(Color.Black);
                }
                else
                {
                    brush = new SolidBrush(this.BackColor);
                }
                Brush brush2 = new SolidBrush(this.ForeColor);
                if (this.UsesBG)
                {
                    graphics.DrawImage(this.mManager.GetImage(@"default\brushbg"), 0, 0);
                    graphics.FillRectangle(brush, new Rectangle(0x17, 0, base.Width - 0x18, base.Height));
                    graphics.DrawString(this.Text, this.Font, brush2, new PointF(27f, 3f));
                    Brush brush3 = new LinearGradientBrush(new Point(0, 0), new Point(0, base.Height), Color.Black, Color.White);
                    Pen pen = new Pen(brush3);
                    graphics.DrawLine(pen, 0x18, 0, base.Width - 1, 0);
                    graphics.DrawLine(pen, 0x18, base.Height - 1, base.Width - 1, base.Height - 1);
                    graphics.DrawLine(pen, 0x18, 0, 0x18, base.Height);
                    graphics.DrawLine(pen, base.Width - 1, 0, base.Width - 1, base.Height - 1);
                    pen.Dispose();
                    brush3.Dispose();
                }
                if (base.Checked)
                {
                    image = this.mManager.GetImage(@"default\checkbox_checked");
                }
                else
                {
                    image = this.mManager.GetImage(@"default\checkbox");
                }
                graphics.DrawImage(image, 0, 0);
                brush2.Dispose();
                brush.Dispose();
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                if (!this.mUsesBG || base.DesignMode)
                {
                    base.Font = value;
                }
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if ((!this.mUsesBG || (value != Color.White)) || base.DesignMode)
                {
                    base.ForeColor = value;
                }
            }
        }

        public bool UsesBG
        {
            get
            {
                return this.mUsesBG;
            }
            set
            {
                this.mUsesBG = value;
            }
        }
    }
}

