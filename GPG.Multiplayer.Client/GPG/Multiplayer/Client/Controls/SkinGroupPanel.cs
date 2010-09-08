namespace GPG.Multiplayer.Client.Controls
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class SkinGroupPanel : PnlBase, IStyledControl
    {
        private bool CancelGrow = false;
        private IContainer components = null;
        public GPGLabel gpgLabelHeader;
        private int GrowIncrement;
        private int GrowSpeed = 6;
        private bool IsGrowing = false;
        private int MaxHeight;
        private bool mCutCorner = true;
        private Image mHeaderImage = GroupPanelImages.default_header;
        private bool mIsExpanded = true;
        private SkinButton skinButtonExpandCollapse;
        private int TargetHeight;

        public SkinGroupPanel()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
        }

        private void CalculateRegion()
        {
            Image bR;
            if (this.CutCorner)
            {
                bR = GroupPanelImages.BR_CUT;
            }
            else
            {
                bR = GroupPanelImages.BR;
            }
            DrawUtil.MakeTranparent(this, GroupPanelImages.TL, GroupPanelImages.TR, GroupPanelImages.BL, bR);
        }

        private void Contract()
        {
            this.TargetHeight = this.MinHeight;
            this.GrowIncrement = -this.GrowSpeed;
            this.PerformGrow();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Expand()
        {
            this.TargetHeight = this.MaxHeight;
            this.GrowIncrement = this.GrowSpeed;
            this.PerformGrow();
        }

        private void InitializeComponent()
        {
            this.gpgLabelHeader = new GPGLabel();
            this.skinButtonExpandCollapse = new SkinButton();
            base.SuspendLayout();
            this.gpgLabelHeader.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelHeader.AutoStyle = true;
            this.gpgLabelHeader.BackColor = Color.Transparent;
            this.gpgLabelHeader.Dock = DockStyle.Top;
            this.gpgLabelHeader.Font = new System.Drawing.Font("Verdana", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelHeader.ForeColor = Color.White;
            this.gpgLabelHeader.IgnoreMouseWheel = false;
            this.gpgLabelHeader.IsStyled = false;
            this.gpgLabelHeader.Location = new Point(0, 0);
            this.gpgLabelHeader.Name = "gpgLabelHeader";
            this.gpgLabelHeader.Padding = new Padding(8, 0, 0, 0);
            this.gpgLabelHeader.Size = new Size(0xef, 0x25);
            this.gpgLabelHeader.TabIndex = 0;
            this.gpgLabelHeader.Text = "header";
            this.gpgLabelHeader.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabelHeader.TextStyle = TextStyles.Custom;
            this.skinButtonExpandCollapse.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.skinButtonExpandCollapse.AutoStyle = true;
            this.skinButtonExpandCollapse.BackColor = Color.Transparent;
            this.skinButtonExpandCollapse.ButtonState = 0;
            this.skinButtonExpandCollapse.DialogResult = DialogResult.OK;
            this.skinButtonExpandCollapse.DisabledForecolor = Color.Gray;
            this.skinButtonExpandCollapse.DrawColor = Color.White;
            this.skinButtonExpandCollapse.DrawEdges = false;
            this.skinButtonExpandCollapse.Enabled = false;
            this.skinButtonExpandCollapse.FocusColor = Color.Yellow;
            this.skinButtonExpandCollapse.Font = new System.Drawing.Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonExpandCollapse.ForeColor = Color.White;
            this.skinButtonExpandCollapse.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonExpandCollapse.IsStyled = true;
            this.skinButtonExpandCollapse.Location = new Point(0xd4, 4);
            this.skinButtonExpandCollapse.Name = "skinButtonExpandCollapse";
            this.skinButtonExpandCollapse.Size = new Size(0x18, 0x12);
            this.skinButtonExpandCollapse.SkinBasePath = @"Controls\GroupPanel\BtnCollapse";
            this.skinButtonExpandCollapse.TabIndex = 0x26;
            this.skinButtonExpandCollapse.TabStop = true;
            this.skinButtonExpandCollapse.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonExpandCollapse.TextPadding = new Padding(0);
            this.skinButtonExpandCollapse.Visible = false;
            this.skinButtonExpandCollapse.Click += new EventHandler(this.skinButtonExpandCollapse_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.skinButtonExpandCollapse);
            base.Controls.Add(this.gpgLabelHeader);
            this.DoubleBuffered = true;
            base.Name = "SkinGroupPanel";
            base.Size = new Size(0xef, 0xca);
            base.ResumeLayout(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgLabelHeader.SendToBack();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.PaintHeader(e.Graphics);
            base.OnPaint(e);
            this.PaintBorder(e.Graphics);
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            base.Invalidate(false);
            base.Update();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!this.IsGrowing)
            {
                this.CalculateRegion();
                this.MaxHeight = base.Height;
                this.GrowSpeed = (int) ((((float) this.MaxHeight) / 200f) * this.GrowSpeed);
            }
        }

        public void PaintBorder(Graphics g)
        {
            TextureBrush brush;
            Image bR;
            using (brush = new TextureBrush(this.HeaderImage, new Rectangle(0, 0, this.HeaderImage.Width, this.HeaderImage.Height)))
            {
                g.FillRectangle(brush, new Rectangle(0, 0, base.Width, this.HeaderImage.Height));
            }
            if (this.CutCorner)
            {
                bR = GroupPanelImages.BR_CUT;
            }
            else
            {
                bR = GroupPanelImages.BR;
            }
            g.DrawImage(GroupPanelImages.TL, new Rectangle(0, 0, GroupPanelImages.TL.Width, GroupPanelImages.TL.Height));
            g.DrawImage(GroupPanelImages.TR, new Rectangle(base.Width - GroupPanelImages.TR.Width, 0, GroupPanelImages.TR.Width, GroupPanelImages.TR.Height));
            g.DrawImage(GroupPanelImages.BL, new Rectangle(0, base.Height - GroupPanelImages.BL.Height, GroupPanelImages.BL.Width, GroupPanelImages.BL.Height));
            g.DrawImage(bR, new Rectangle(base.Width - bR.Width, base.Height - bR.Height, bR.Width, bR.Height));
            using (brush = new TextureBrush(GroupPanelImages.TC, new Rectangle(0, 0, GroupPanelImages.TC.Width, GroupPanelImages.TC.Height)))
            {
                g.FillRectangle(brush, new Rectangle(GroupPanelImages.TL.Width, 0, base.Width - (GroupPanelImages.TL.Width + GroupPanelImages.TR.Width), GroupPanelImages.TC.Height));
            }
            using (brush = new TextureBrush(GroupPanelImages.ML, new Rectangle(0, 0, GroupPanelImages.ML.Width, GroupPanelImages.ML.Height)))
            {
                g.FillRectangle(brush, new Rectangle(0, GroupPanelImages.TL.Height, GroupPanelImages.ML.Width, base.Height - (GroupPanelImages.TL.Height + GroupPanelImages.BL.Height)));
            }
            using (brush = new TextureBrush(GroupPanelImages.MR, new Rectangle(0, 0, GroupPanelImages.MR.Width, GroupPanelImages.MR.Height)))
            {
                g.FillRectangle(brush, new Rectangle(base.Width - GroupPanelImages.MR.Width, GroupPanelImages.TR.Height, GroupPanelImages.MR.Width, base.Height - (GroupPanelImages.TR.Height + bR.Height)));
            }
            using (brush = new TextureBrush(GroupPanelImages.BC, new Rectangle(0, 0, GroupPanelImages.BC.Width, GroupPanelImages.BC.Height)))
            {
                g.FillRectangle(brush, new Rectangle(GroupPanelImages.BL.Width, base.Height - GroupPanelImages.BC.Height, base.Width - (GroupPanelImages.BL.Width + bR.Width), GroupPanelImages.BC.Height));
            }
        }

        public void PaintHeader(Graphics g)
        {
            using (TextureBrush brush = new TextureBrush(this.HeaderImage, new Rectangle(0, 0, this.HeaderImage.Width, this.HeaderImage.Height)))
            {
                g.FillRectangle(brush, new Rectangle(0, 0, base.Width, this.HeaderImage.Height));
            }
        }

        private void PerformGrow()
        {
        }

        private void skinButtonExpandCollapse_Click(object sender, EventArgs e)
        {
            if (this.IsExpanded)
            {
                this.mIsExpanded = !this.IsExpanded;
                this.skinButtonExpandCollapse.SkinBasePath = @"Controls\GroupPanel\BtnExpand";
                this.Contract();
            }
            else
            {
                this.mIsExpanded = !this.IsExpanded;
                this.skinButtonExpandCollapse.SkinBasePath = @"Controls\GroupPanel\BtnCollapse";
                this.Expand();
            }
        }

        [Browsable(false)]
        public bool AutoStyle
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Browsable(true)]
        public bool CutCorner
        {
            get
            {
                return this.mCutCorner;
            }
            set
            {
                this.mCutCorner = value;
                this.CalculateRegion();
                this.Refresh();
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return this.gpgLabelHeader.Font;
            }
            set
            {
                base.Font = value;
                this.gpgLabelHeader.Font = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true)]
        public override Color ForeColor
        {
            get
            {
                return this.gpgLabelHeader.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                this.gpgLabelHeader.ForeColor = value;
            }
        }

        [Browsable(true)]
        public Image HeaderImage
        {
            get
            {
                return this.mHeaderImage;
            }
            set
            {
                this.mHeaderImage = value;
                this.gpgLabelHeader.Height = this.HeaderImage.Height;
            }
        }

        [Browsable(true)]
        public GPGLabel HeaderLabel
        {
            get
            {
                return this.gpgLabelHeader;
            }
        }

        public bool IsExpanded
        {
            get
            {
                return this.mIsExpanded;
            }
        }

        [Browsable(false)]
        public bool IsStyled
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        private int MinHeight
        {
            get
            {
                return this.HeaderImage.Height;
            }
        }

        protected override bool ScaleChildren
        {
            get
            {
                return false;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Browsable(true)]
        public override string Text
        {
            get
            {
                return this.gpgLabelHeader.Text;
            }
            set
            {
                base.Text = value;
                this.gpgLabelHeader.Text = value;
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ContentAlignment TextAlign
        {
            get
            {
                return this.gpgLabelHeader.TextAlign;
            }
            set
            {
                this.gpgLabelHeader.TextAlign = value;
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Padding TextPadding
        {
            get
            {
                return this.gpgLabelHeader.Padding;
            }
            set
            {
                this.gpgLabelHeader.Padding = value;
            }
        }
    }
}

