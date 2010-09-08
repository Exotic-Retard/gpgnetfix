namespace GPG.UI.Controls.Docking
{
    using GPG.UI;
    using GPG.UI.Controls.Skinning;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class TabButton : Control
    {
        public const int ICON_SIZE = 0x10;
        private Image mLeftImage;
        private SkinManager mManager;
        private Image mMidImage;
        private TabOrientations mOrientation;
        private DockPanel mPanel;
        private Image mRightImage;
        private bool mSelected = true;
        private Color mSelectedForeColor = Color.Black;
        private SizeF TextSize;

        public TabButton(TabOrientations orientation, DockPanel panel)
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.LightGray;
            this.mOrientation = orientation;
            this.mPanel = panel;
            this.mManager = SkinManager.Docking;
            this.LoadImages();
            this.TextSize = DrawUtil.MeasureString(base.CreateGraphics(), this.Panel.Title, this.Font);
            base.Width = (this.LeftImage.Width + this.RightImage.Width) + ((int) this.TextSize.Width);
            if (this.Panel.Icon != null)
            {
                base.Width += this.Panel.Icon.Width;
            }
            base.Height = this.MidImage.Height;
        }

        private void LoadImages()
        {
            string str;
            if (this.Selected)
            {
                str = "active";
            }
            else
            {
                str = "inactive";
            }
            string str2 = this.Orientation.ToString().ToLower();
            switch (this.Orientation)
            {
                case TabOrientations.Top:
                case TabOrientations.Bottom:
                    this.mLeftImage = this.Manager.GetImage(string.Format("tab_{0}_{1}_left", str2, str));
                    this.mRightImage = this.Manager.GetImage(string.Format("tab_{0}_{1}_right", str2, str));
                    this.mMidImage = this.Manager.GetImage(string.Format("tab_{0}_{1}_mid", str2, str));
                    break;
            }
            this.Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!this.Selected)
            {
                this.Select();
            }
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Color selectedForeColor;
            Font font = this.Font;
            e.Graphics.DrawImage(this.LeftImage, 0, 0, this.LeftImage.Width, this.LeftImage.Height);
            int num = (base.Width - (this.LeftImage.Width + this.RightImage.Width)) / this.MidImage.Width;
            if (((base.Width - (this.LeftImage.Width + this.RightImage.Width)) % this.MidImage.Width) > 0)
            {
                num++;
            }
            for (int i = 0; i < num; i++)
            {
                e.Graphics.DrawImage(this.MidImage, this.LeftImage.Width + (i * this.MidImage.Width), 0, this.MidImage.Width, this.MidImage.Height);
            }
            if (this.Panel.Icon != null)
            {
                e.Graphics.DrawImage(this.Panel.Icon, new Rectangle(this.LeftImage.Width, (base.Height - 0x10) / 2, 0x10, 0x10));
            }
            if (this.Selected)
            {
                selectedForeColor = this.SelectedForeColor;
            }
            else
            {
                selectedForeColor = this.ForeColor;
            }
            using (SolidBrush brush = new SolidBrush(selectedForeColor))
            {
                int width = this.LeftImage.Width;
                if (this.Panel.Icon != null)
                {
                    width += 20;
                }
                e.Graphics.DrawString(this.Panel.Title, font, brush, (float) width, (float) ((base.Height / 2) - (font.Height / 2)));
            }
            e.Graphics.DrawImage(this.RightImage, base.Width - this.RightImage.Width, 0, this.RightImage.Width, this.RightImage.Height);
        }

        internal void Select()
        {
            this.mSelected = true;
            this.LoadImages();
        }

        internal void Unselect()
        {
            this.mSelected = false;
            this.LoadImages();
        }

        public Image LeftImage
        {
            get
            {
                return this.mLeftImage;
            }
        }

        public SkinManager Manager
        {
            get
            {
                return this.mManager;
            }
        }

        public Image MidImage
        {
            get
            {
                return this.mMidImage;
            }
        }

        public TabOrientations Orientation
        {
            get
            {
                return this.mOrientation;
            }
        }

        public DockPanel Panel
        {
            get
            {
                return this.mPanel;
            }
        }

        public Image RightImage
        {
            get
            {
                return this.mRightImage;
            }
        }

        public bool Selected
        {
            get
            {
                return this.mSelected;
            }
        }

        public Color SelectedForeColor
        {
            get
            {
                return this.mSelectedForeColor;
            }
            set
            {
                this.mSelectedForeColor = value;
            }
        }
    }
}

