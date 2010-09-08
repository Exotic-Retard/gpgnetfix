namespace GPG.UI.Controls.Docking
{
    using GPG.UI;
    using GPG.UI.Controls.Skinning;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DockContainerTitleBar : UserControl
    {
        private EventHandler _ContainerForm_DockedChanged;
        private DockSkinnedButton btnClose;
        private DockSkinnedButton btnDock;
        private DockSkinnedButton btnMin;
        private DockSkinnedButton btnRestore;
        private IContainer components;
        private DockPanel ContainerPanel;
        private Image LeftImg;
        private DockContainerForm mContainerForm;
        private bool mDragging;
        private Image MidImg;
        private Image RightImg;

        public DockContainerTitleBar(DockContainerForm containerForm)
        {
            this.InitializeComponent();
            this.mContainerForm = containerForm;
            this.Dock = DockStyle.Fill;
            this._ContainerForm_DockedChanged = new EventHandler(this.ContainerForm_DockedChanged);
            this.ContainerForm.Docked += this._ContainerForm_DockedChanged;
            this.ContainerForm.Undocked += this._ContainerForm_DockedChanged;
            this.ResizeImages();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.ContainerForm.Close();
        }

        private void btnDock_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (this.ContainerPanel != null))
            {
                this.Controller.OnBeginDockDrag(this.ContainerPanel);
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.ContainerForm.Minimize();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            this.ContainerForm.Restore();
            if (this.ContainerForm.WindowState == FormWindowState.Maximized)
            {
                this.btnRestore.ResourceKey = "restore";
            }
            else
            {
                this.btnRestore.ResourceKey = "maximize";
            }
        }

        private void ContainerForm_DockedChanged(object sender, EventArgs e)
        {
            if (this.ContainerForm.Panels.Count > 1)
            {
                this.btnDock.Visible = false;
            }
            else if (this.ContainerForm.Panels.Count == 1)
            {
                this.btnDock.Visible = true;
                this.ContainerPanel = this.ContainerForm.Panels[0];
            }
            this.Refresh();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DockContainerTitleBar));
            this.btnRestore = new DockSkinnedButton();
            this.btnMin = new DockSkinnedButton();
            this.btnDock = new DockSkinnedButton();
            this.btnClose = new DockSkinnedButton();
            ((ISupportInitialize) this.btnRestore).BeginInit();
            ((ISupportInitialize) this.btnMin).BeginInit();
            ((ISupportInitialize) this.btnDock).BeginInit();
            ((ISupportInitialize) this.btnClose).BeginInit();
            base.SuspendLayout();
            this.btnRestore.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnRestore.AutoRefreshSkin = true;
            this.btnRestore.BackColor = Color.Transparent;
            this.btnRestore.Image = (Image) manager.GetObject("btnRestore.Image");
            this.btnRestore.Location = new Point(0x126, 5);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.ResourceKey = "maximize";
            this.btnRestore.Size = new Size(20, 20);
            this.btnRestore.SizeMode = PictureBoxSizeMode.StretchImage;
            this.btnRestore.TabIndex = 4;
            this.btnRestore.TabStop = false;
            this.btnRestore.MouseLeave += new EventHandler(this.OnButtonMouseLeave);
            this.btnRestore.Click += new EventHandler(this.btnRestore_Click);
            this.btnRestore.MouseEnter += new EventHandler(this.OnButtonMouseEnter);
            this.btnMin.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnMin.AutoRefreshSkin = true;
            this.btnMin.BackColor = Color.Transparent;
            this.btnMin.Image = (Image) manager.GetObject("btnMin.Image");
            this.btnMin.Location = new Point(0x10f, 5);
            this.btnMin.Name = "btnMin";
            this.btnMin.ResourceKey = "minimize";
            this.btnMin.Size = new Size(20, 20);
            this.btnMin.SizeMode = PictureBoxSizeMode.StretchImage;
            this.btnMin.TabIndex = 3;
            this.btnMin.TabStop = false;
            this.btnMin.MouseLeave += new EventHandler(this.OnButtonMouseLeave);
            this.btnMin.Click += new EventHandler(this.btnMin_Click);
            this.btnMin.MouseEnter += new EventHandler(this.OnButtonMouseEnter);
            this.btnDock.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnDock.AutoRefreshSkin = true;
            this.btnDock.BackColor = Color.Transparent;
            this.btnDock.Image = (Image) manager.GetObject("btnDock.Image");
            this.btnDock.Location = new Point(0xf8, 5);
            this.btnDock.Name = "btnDock";
            this.btnDock.ResourceKey = "dock";
            this.btnDock.Size = new Size(20, 20);
            this.btnDock.SizeMode = PictureBoxSizeMode.StretchImage;
            this.btnDock.TabIndex = 2;
            this.btnDock.TabStop = false;
            this.btnDock.MouseDown += new MouseEventHandler(this.btnDock_MouseDown);
            this.btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnClose.AutoRefreshSkin = true;
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.Image = (Image) manager.GetObject("btnClose.Image");
            this.btnClose.Location = new Point(0x13d, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.ResourceKey = "close";
            this.btnClose.Size = new Size(20, 20);
            this.btnClose.SizeMode = PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.MouseLeave += new EventHandler(this.OnButtonMouseLeave);
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.btnClose.MouseEnter += new EventHandler(this.OnButtonMouseEnter);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.btnRestore);
            base.Controls.Add(this.btnMin);
            base.Controls.Add(this.btnDock);
            base.Controls.Add(this.btnClose);
            base.Name = "DockContainerTitleBar";
            base.Size = new Size(0x157, 50);
            ((ISupportInitialize) this.btnRestore).EndInit();
            ((ISupportInitialize) this.btnMin).EndInit();
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

        protected override void OnDoubleClick(EventArgs e)
        {
            this.ContainerForm.Restore();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (((e.Button == MouseButtons.Left) && !this.ContainerForm.IsResizing) && (!this.Dragging && this.DragBounds.Contains(e.Location)))
            {
                this.mDragging = true;
                this.ContainerForm.BeginDrag(e.Location);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.Dragging)
            {
                this.ContainerForm.DragMove(e.Location);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.Dragging)
            {
                this.ContainerForm.EndDrag();
                this.mDragging = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.ContainerForm.LayoutSuspended)
            {
                goto Label_01DD;
            }
            if (base.DesignMode)
            {
                using (Brush brush = new SolidBrush(Color.Gray))
                {
                    e.Graphics.FillRectangle(brush, base.ClientRectangle);
                    goto Label_00DF;
                }
            }
            e.Graphics.DrawImage(this.LeftImg, new Point(0, 0));
            using (Brush brush2 = new TextureBrush(this.MidImg, WrapMode.Tile))
            {
                e.Graphics.FillRectangle(brush2, new Rectangle(this.LeftImg.Width, 0, base.Width - (this.LeftImg.Width + this.RightImg.Width), base.Height));
            }
            e.Graphics.DrawImage(this.RightImg, new Point(base.Width - this.RightImg.Width, 0));
        Label_00DF:
            if (this.ContainerForm.Panels.Count == 1)
            {
                int left = this.ContainerForm.TextPadding.Left;
                if (this.ContainerPanel.Icon != null)
                {
                    left += 20;
                    e.Graphics.DrawImage(this.ContainerPanel.Icon, this.ContainerForm.TextPadding.Left, this.ContainerForm.TextPadding.Top - ((this.ContainerPanel.Icon.Height - this.ContainerForm.Font.Height) / 2), 0x10, 0x10);
                }
                using (SolidBrush brush3 = new SolidBrush(this.ContainerForm.ForeColor))
                {
                    e.Graphics.DrawString(this.ContainerPanel.Title, this.ContainerForm.Font, brush3, new PointF((float) left, (float) this.ContainerForm.TextPadding.Top));
                }
            }
        Label_01DD:
            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.ResizeImages();
        }

        private void ResizeImages()
        {
            this.LeftImg = SkinManager.Docking.GetImage("titlebar_left");
            this.MidImg = SkinManager.Docking.GetImage("titlebar_mid");
            this.RightImg = SkinManager.Docking.GetImage("titlebar_right");
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

        public DockContainerForm ContainerForm
        {
            get
            {
                return this.mContainerForm;
            }
        }

        public DockController Controller
        {
            get
            {
                return this.ContainerForm.Controller;
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
    }
}

