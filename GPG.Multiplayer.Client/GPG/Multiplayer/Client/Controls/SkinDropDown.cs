namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SkinDropDown : SkinButton
    {
        private IContainer components;
        private const int LPAD = 6;
        private Image mIcon;
        private GPGContextMenu mMenu;
        private bool Refreshing;

        public SkinDropDown() : base(@"Controls\Button\Dropdown")
        {
            this.components = null;
            this.mMenu = new GPGContextMenu();
            this.Refreshing = false;
            this.mIcon = null;
            this.InitializeComponent();
            base.TextAlign = ContentAlignment.MiddleLeft;
            this.TextPadding = new Padding(4, 0, 0, 0);
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

        protected override void OnAutoStyled()
        {
            this.ForeColor = Color.Black;
            base.OnAutoStyled();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Menu.Show(this, new Point(0, base.Height));
                this.OnMouseUp(e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Icon != null)
            {
                e.Graphics.DrawImage(this.Icon, new Rectangle(DrawUtil.CenterV(base.Height, this.Icon.Height, 6), new Size(this.Icon.Width, this.Icon.Height)));
            }
        }

        public Image Icon
        {
            get
            {
                return this.mIcon;
            }
            set
            {
                VGen0 method = null;
                this.mIcon = value;
                if (!this.Refreshing)
                {
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.Refreshing = true;
                                this.Refresh();
                                this.Refreshing = false;
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (!(base.Disposing || base.IsDisposed))
                    {
                        this.Refreshing = true;
                        this.Refresh();
                        this.Refreshing = false;
                    }
                }
            }
        }

        [Browsable(false)]
        public GPGContextMenu Menu
        {
            get
            {
                return this.mMenu;
            }
        }

        public override Padding TextPadding
        {
            get
            {
                if (this.Icon != null)
                {
                    return new Padding(this.Icon.Width + 12, 0, 0, 0);
                }
                return new Padding(6, 0, 0, 0);
            }
            set
            {
            }
        }
    }
}

