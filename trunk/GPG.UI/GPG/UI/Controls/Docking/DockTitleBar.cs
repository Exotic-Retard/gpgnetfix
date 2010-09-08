namespace GPG.UI.Controls.Docking
{
    using GPG.UI;
    using GPG.UI.Controls.Skinning;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DockTitleBar : UserControl
    {
        private DockSkinnedButton btnClose;
        private DockSkinnedButton btnDock;
        private IContainer components;
        private Image LeftImg;
        private DockPanel mContainerPanel;
        private int mDockedHeight;
        private bool mDragging;
        private int mFloatingHeight;
        private Image MidImg;
        private int mUndockMovementThreshhold;
        private Image RightImg;
        private int UndockMove;
        private Point UndockStart;

        public DockTitleBar(DockPanel containerPanel)
        {
            DockStyleEventHandler handler = null;
            this.mDockedHeight = 0x19;
            this.mFloatingHeight = 50;
            this.mUndockMovementThreshhold = 8;
            this.mContainerPanel = containerPanel;
            this.InitializeComponent();
            this.Dock = System.Windows.Forms.DockStyle.Top;
            if (handler == null)
            {
                handler = delegate (object s, DockStyles style) {
                    this.OnDockedChanged();
                };
            }
            this.ContainerPanel.DockStyleChanged += handler;
            this.OnDockedChanged();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.ContainerPanel.Close();
        }

        private void btnDock_Click(object sender, EventArgs e)
        {
        }

        private void btnDock_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Controller.OnBeginDockDrag(this.ContainerPanel);
            }
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DockTitleBar));
            this.btnDock = new DockSkinnedButton();
            this.btnClose = new DockSkinnedButton();
            ((ISupportInitialize) this.btnDock).BeginInit();
            ((ISupportInitialize) this.btnClose).BeginInit();
            base.SuspendLayout();
            this.btnDock.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnDock.AutoRefreshSkin = true;
            this.btnDock.BackColor = Color.Transparent;
            this.btnDock.Image = (Image) manager.GetObject("btnDock.Image");
            this.btnDock.Location = new Point(0x127, 6);
            this.btnDock.Name = "btnDock";
            this.btnDock.ResourceKey = "dock";
            this.btnDock.Size = new Size(0x10, 0x10);
            this.btnDock.SizeMode = PictureBoxSizeMode.StretchImage;
            this.btnDock.TabIndex = 1;
            this.btnDock.TabStop = false;
            this.btnDock.MouseDown += new MouseEventHandler(this.btnDock_MouseDown);
            this.btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnClose.AutoRefreshSkin = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Image = (Image) manager.GetObject("btnClose.Image");
            this.btnClose.Location = new Point(0x13d, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.ResourceKey = "panel_close";
            this.btnClose.Size = new Size(0x10, 0x10);
            this.btnClose.SizeMode = PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.MouseLeave += new EventHandler(this.OnButtonMouseLeave);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new EventHandler(this.OnButtonMouseEnter);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btnDock);
            base.Controls.Add(this.btnClose);
            base.Name = "DockTitleBar";
            base.Size = new Size(0x157, 50);
            ((ISupportInitialize) this.btnDock).EndInit();
            ((ISupportInitialize) this.btnClose).EndInit();
            base.ResumeLayout(false);
        }

        private void OnButtonMouseEnter(object sender, EventArgs e)
        {
            if (sender is ISkinResourceConsumer)
            {
                ISkinResourceConsumer consumer = sender as ISkinResourceConsumer;
                consumer.ResourceKey = consumer.ResourceKey + "_over";
            }
        }

        private void OnButtonMouseLeave(object sender, EventArgs e)
        {
            if (sender is ISkinResourceConsumer)
            {
                ISkinResourceConsumer consumer = sender as ISkinResourceConsumer;
                consumer.ResourceKey = consumer.ResourceKey.Replace("_over", "");
            }
        }

        internal void OnDockedChanged()
        {
            if (this.ContainerPanel.IsDocked || !DockController.IsInitialized)
            {
                base.Visible = true;
                base.Height = this.DockedHeight;
            }
            else
            {
                base.Visible = false;
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if ((this.DockStyle == DockStyles.Floating) && (this.ContainerPanel.ContainerForm != null))
            {
                this.ContainerPanel.ContainerForm.Restore();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.DockStyle == DockStyles.Floating)
                {
                    if ((!this.ContainerPanel.ContainerForm.IsResizing && !this.Dragging) && this.DragBounds.Contains(e.Location))
                    {
                        this.mDragging = true;
                        this.ContainerPanel.ContainerForm.BeginDrag(e.Location);
                    }
                }
                else
                {
                    this.UndockStart = e.Location;
                    this.UndockMove = 0;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int num = this.UndockStart.X - e.X;
                if (num < 0)
                {
                    num = -num;
                }
                int num2 = this.UndockStart.Y - e.Y;
                if (num2 < 0)
                {
                    num2 = -num2;
                }
                this.UndockMove = Math.Max(num, num2);
                if (this.ContainerPanel.IsFloating && this.Dragging)
                {
                    this.ContainerPanel.ContainerForm.DragMove(e.Location);
                }
                else if (this.ContainerPanel.IsDocked && (this.UndockMove >= this.UndockMovementThreshhold))
                {
                    this.ContainerPanel.Undock();
                    DockContainerForm form = new DockContainerForm(this.Controller, this.ContainerPanel);
                    form.Show();
                    form.Activate();
                    this.mDragging = true;
                    this.ContainerPanel.ContainerForm.BeginDrag(e.Location);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (((e.Button == MouseButtons.Left) && this.ContainerPanel.IsFloating) && this.Dragging)
            {
                this.ContainerPanel.ContainerForm.EndDrag();
                this.mDragging = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(this.LeftImg, new Point(0, 0));
            using (Brush brush = new TextureBrush(this.MidImg, WrapMode.Tile))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(this.LeftImg.Width, 0, base.Width - (this.LeftImg.Width + this.RightImg.Width), base.Height));
            }
            e.Graphics.DrawImage(this.RightImg, new Point(base.Width - this.RightImg.Width, 0));
            using (SolidBrush brush2 = new SolidBrush(this.ContainerPanel.ForeColor))
            {
                e.Graphics.DrawString(this.ContainerPanel.Title, this.ContainerPanel.Font, brush2, new PointF(8f, (float) ((base.Height / 2) - (this.ContainerPanel.Font.Height / 2))));
            }
            base.OnPaint(e);
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            if ((e.Action == DragAction.Drop) || (e.Action == DragAction.Cancel))
            {
                this.Controller.OnEndDockDrag();
                this.Cursor = Cursors.Default;
            }
            base.OnQueryContinueDrag(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.ResizeImages();
        }

        private void ResizeImages()
        {
            if (this.ContainerPanel.IsDocked || !DockController.IsInitialized)
            {
                this.LeftImg = SkinManager.Docking.GetImage("titlebar_docked_left");
                this.MidImg = SkinManager.Docking.GetImage("titlebar_docked_mid");
                this.RightImg = SkinManager.Docking.GetImage("titlebar_docked_right");
            }
            else
            {
                this.LeftImg = SkinManager.Docking.GetImage("titlebar_left");
                this.MidImg = SkinManager.Docking.GetImage("titlebar_mid");
                this.RightImg = SkinManager.Docking.GetImage("titlebar_right");
            }
            if (((this.LeftImg != null) && (this.MidImg != null)) && (this.RightImg != null))
            {
                if (this.LeftImg.Height > base.Height)
                {
                    this.LeftImg = DrawUtil.ResizeImage(this.LeftImg, new Size(this.LeftImg.Width, base.Height));
                }
                if (this.MidImg.Height > base.Height)
                {
                    this.MidImg = DrawUtil.ResizeImage(this.MidImg, new Size(this.MidImg.Width, base.Height));
                }
                if (this.RightImg.Height > base.Height)
                {
                    this.RightImg = DrawUtil.ResizeImage(this.RightImg, new Size(this.RightImg.Width, base.Height));
                }
            }
        }

        public DockPanel ContainerPanel
        {
            get
            {
                return this.mContainerPanel;
            }
        }

        public DockController Controller
        {
            get
            {
                return this.ContainerPanel.Controller;
            }
        }

        protected virtual int DockedHeight
        {
            get
            {
                return this.mDockedHeight;
            }
        }

        public DockStyles DockStyle
        {
            get
            {
                return this.ContainerPanel.DockStyle;
            }
        }

        public virtual Rectangle DragBounds
        {
            get
            {
                return base.Bounds;
            }
        }

        public bool Dragging
        {
            get
            {
                return this.mDragging;
            }
        }

        protected virtual int FloatingHeight
        {
            get
            {
                return this.mFloatingHeight;
            }
        }

        [Browsable(true)]
        public int UndockMovementThreshhold
        {
            get
            {
                return this.mUndockMovementThreshhold;
            }
            set
            {
                this.mUndockMovementThreshhold = value;
            }
        }
    }
}

