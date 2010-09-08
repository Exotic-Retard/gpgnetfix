namespace GPG.UI.Controls.Docking
{
    using GPG;
    using GPG.UI.Controls;
    using GPG.UI.Controls.Skinning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class DockContainerForm : Form, IDockableContainer
    {
        private MouseEventHandler _Mouse_ButtonDown;
        private MouseEventHandler _Mouse_ButtonUp;
        private MouseEventHandler _Mouse_Moved;
        private IContainer components;
        private bool Dragging;
        private bool DragPaused;
        private Point DragStart;
        public const int ICON_SIZE = 0x10;
        private int LastDrag;
        private Image mBorderBottom;
        private Image mBorderBottomLeft;
        private Image mBorderBottomRight;
        private Image mBorderLeft;
        private Image mBorderRight;
        private Control mContainerControl;
        private DockController mController;
        private DockStyles mDockStyle;
        private IDockableContainer mDockTarget;
        private int mFormBorderCurve;
        private int mHeightDiff;
        private bool mIsClosing;
        private bool mIsResizing;
        private bool mLayoutSuspended;
        private int mOutterDockThreshhold;
        private List<DockPanel> mPanels;
        private Color mPreviewColor;
        private int mPreviewOpacity;
        private DockStyles mPreviewStyle;
        private ResizeDirections mResizeDirection;
        private int mResizeThreshold;
        private Padding mTextPadding;
        private DockContainerTitleBar mTitleBar;
        private int mWidthDiff;
        private Point ResizeStartLoc;
        private Size ResizeStartSize;
        private Control RootContainer;
        private DockContainerRootPanel RootPanel;
        private GPGPanel TitleBarPanel;

        public event EventHandler Docked;

        public event EventHandler Undocked;

        public DockContainerForm(DockController controller, DockPanel initialPanel)
        {
            EventHandler handler = null;
            this.mTextPadding = new Padding(6, 8, 0, 0);
            this.mFormBorderCurve = 15;
            this.mPanels = new List<DockPanel>();
            this.mOutterDockThreshhold = 30;
            this.mPreviewStyle = DockStyles.None;
            this.mPreviewColor = Color.SkyBlue;
            this.mPreviewOpacity = 100;
            this.DragPaused = true;
            this.mResizeThreshold = 8;
            this.mDockStyle = DockStyles.Fill;
            this.InitializeComponent();
            this.mController = controller;
            this.Controller.Containers.Add(this);
            this.RootContainer = this.RootPanel;
            initialPanel.ContainerControl = this.RootContainer;
            initialPanel.ContainerForm = this;
            initialPanel.DockStyle = DockStyles.Floating;
            initialPanel.OnDocked();
            this.mTitleBar = new DockContainerTitleBar(this);
            this.TitleBarPanel.Height = this.TitleBar.Height;
            if (handler == null)
            {
                handler = delegate (object s, EventArgs e) {
                    this.TitleBarPanel.Height = this.TitleBar.Height;
                };
            }
            this.TitleBar.SizeChanged += handler;
            this.TitleBarPanel.Controls.Add(this.TitleBar);
            this.LoadBorder();
            this.OnDocked(initialPanel);
            this.RootContainer.Controls.Add(initialPanel);
            this._Mouse_Moved = new MouseEventHandler(this.Mouse_Moved);
            GlobalHook.Mouse.Moved += this._Mouse_Moved;
            this._Mouse_ButtonDown = new MouseEventHandler(this.Mouse_ButtonDown);
            GlobalHook.Mouse.ButtonDown += this._Mouse_ButtonDown;
            this._Mouse_ButtonUp = new MouseEventHandler(this.Mouse_ButtonUp);
            GlobalHook.Mouse.ButtonUp += this._Mouse_ButtonUp;
        }

        internal void BeginDrag(Point pt)
        {
            this.Dragging = true;
            this.DragStart = pt;
        }

        [DllImport("GDI32.DLL")]
        private static extern int CreateRectRgn(int x1, int y1, int x2, int y2);
        [DllImport("GDI32.DLL")]
        private static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DockTo(IDockableContainer target, DockStyles orientation)
        {
            throw new Exception("A DockContainerForm may only be the target of docking operations, not the source.");
        }

        internal void DragMove(Point pt)
        {
            pt = base.PointToScreen(pt);
            base.Left = pt.X - this.DragStart.X;
            base.Top = pt.Y - this.DragStart.Y;
        }

        private void DrawBorder(Graphics g)
        {
            g.DrawImage(this.BorderLeft, new Rectangle(0, this.TitleBarPanel.Height, this.BorderLeft.Width, base.Height - (this.TitleBarPanel.Height + this.BorderBottomLeft.Height)));
            g.DrawImage(this.BorderRight, new Rectangle(base.Width - this.BorderRight.Width, this.TitleBarPanel.Height, this.BorderRight.Width, base.Height - (this.TitleBarPanel.Height + this.BorderBottomRight.Height)));
            int num = base.Width / this.BorderBottom.Width;
            if ((base.Width % num) > 0)
            {
                num++;
            }
            for (int i = 0; i < num; i++)
            {
                g.DrawImageUnscaledAndClipped(this.BorderBottom, new Rectangle(i * this.BorderBottom.Width, base.Height - this.BorderBottom.Height, this.BorderBottom.Width, this.BorderBottom.Height));
            }
            g.DrawImage(this.BorderBottomLeft, new Rectangle(0, base.Height - this.BorderBottomLeft.Height, this.BorderBottomLeft.Width, this.BorderBottomLeft.Height));
            g.DrawImage(this.BorderBottomRight, new Rectangle(base.Width - this.BorderBottomRight.Width, base.Height - this.BorderBottomRight.Height, this.BorderBottomRight.Width, this.BorderBottomRight.Height));
        }

        internal void DrawDockPreview(Point pt)
        {
            DockStyles none = DockStyles.None;
            bool flag = false;
            DockPanel panel = null;
            Point point = base.PointToClient(pt);
            if (this.Panels.Count > 0)
            {
                if (point.X >= (base.Width - this.OutterDockThreshhold))
                {
                    flag = true;
                    none = DockStyles.Right;
                }
                else if (point.X <= this.OutterDockThreshhold)
                {
                    flag = true;
                    none = DockStyles.Left;
                }
                else if (point.Y >= (base.Height - this.OutterDockThreshhold))
                {
                    flag = true;
                    none = DockStyles.Bottom;
                }
                else if (point.Y < this.OutterDockThreshhold)
                {
                    flag = true;
                    none = DockStyles.Top;
                }
                if (!flag)
                {
                    foreach (DockPanel panel2 in this.Panels)
                    {
                        Point point2 = panel2.PointToClient(pt);
                        if (panel2.Bounds.Contains(point2))
                        {
                            panel = panel2;
                            if (point2.X >= (panel2.Width - this.OutterDockThreshhold))
                            {
                                none = DockStyles.Right;
                            }
                            else if (point2.X <= this.OutterDockThreshhold)
                            {
                                none = DockStyles.Left;
                            }
                            else if (point2.Y >= (panel2.Height - this.OutterDockThreshhold))
                            {
                                none = DockStyles.Bottom;
                            }
                            else if (point2.Y < this.OutterDockThreshhold)
                            {
                                none = DockStyles.Top;
                            }
                            else
                            {
                                none = DockStyles.Tabbed;
                            }
                            break;
                        }
                    }
                }
                Rectangle rect = new Rectangle();
                if (flag)
                {
                    int num = 0;
                    int num2 = 0;
                    foreach (DockPanel panel3 in this.Panels)
                    {
                        switch (panel3.DockStyle)
                        {
                            case DockStyles.Left:
                            case DockStyles.Right:
                            {
                                num2++;
                                continue;
                            }
                            case DockStyles.Top:
                            case DockStyles.Bottom:
                            {
                                num++;
                                continue;
                            }
                        }
                    }
                    switch (none)
                    {
                        case DockStyles.Left:
                            rect.Width = this.OutterDockThreshhold;
                            rect.Height = base.Height;
                            break;

                        case DockStyles.Right:
                            rect.Width = this.OutterDockThreshhold;
                            rect.Height = base.Height;
                            rect.X = base.Width - this.OutterDockThreshhold;
                            break;

                        case DockStyles.Top:
                            rect.Width = base.Width;
                            rect.Height = this.OutterDockThreshhold;
                            break;

                        case DockStyles.Bottom:
                            rect.Width = base.Width;
                            rect.Height = this.OutterDockThreshhold;
                            rect.Y = base.Height - this.OutterDockThreshhold;
                            break;
                    }
                    using (Graphics graphics = base.CreateGraphics())
                    {
                        using (Brush brush = new SolidBrush(this.PreviewColor))
                        {
                            graphics.FillRectangle(brush, rect);
                        }
                        return;
                    }
                }
                if (panel != null)
                {
                    switch (none)
                    {
                        case DockStyles.Left:
                            rect.Width = panel.Width / 2;
                            rect.Height = panel.Height;
                            break;

                        case DockStyles.Right:
                            rect.Width = panel.Width / 2;
                            rect.Height = panel.Height;
                            rect.X = panel.Width / 2;
                            break;

                        case DockStyles.Top:
                            rect.Width = panel.Width;
                            rect.Height = panel.Height / 2;
                            break;

                        case DockStyles.Bottom:
                            rect.Width = panel.Width;
                            rect.Height = panel.Height / 2;
                            rect.Y = panel.Height / 2;
                            break;

                        case DockStyles.Tabbed:
                            rect = panel.ClientRectangle;
                            break;
                    }
                    using (Graphics graphics2 = panel.CreateGraphics())
                    {
                        using (Brush brush2 = new SolidBrush(this.PreviewColor))
                        {
                            graphics2.FillRectangle(brush2, rect);
                        }
                    }
                }
            }
        }

        internal void EndDrag()
        {
            if ((this.Controller.ActivePanel != null) && this.Controller.ActivePanel.Equals(this))
            {
                this.Controller.OnEndDockDrag();
            }
            this.Dragging = false;
        }

        private void InitializeComponent()
        {
            this.RootPanel = new DockContainerRootPanel();
            this.TitleBarPanel = new GPGPanel();
            base.SuspendLayout();
            this.RootPanel.BackColor = Color.Black;
            this.RootPanel.Location = new Point(0x2d, 0x2e);
            this.RootPanel.Name = "RootPanel";
            this.RootPanel.Size = new Size(0x11c, 0x116);
            this.RootPanel.TabIndex = 1;
            this.TitleBarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleBarPanel.Location = new Point(0, 0);
            this.TitleBarPanel.Name = "TitleBarPanel";
            this.TitleBarPanel.Size = new Size(400, 0x19);
            this.TitleBarPanel.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.ControlText;
            base.ClientSize = new Size(400, 0x16d);
            base.Controls.Add(this.RootPanel);
            base.Controls.Add(this.TitleBarPanel);
            base.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(100, 100);
            base.Name = "DockContainerForm";
            base.ShowIcon = false;
            this.Text = "DockForm";
            base.ResumeLayout(false);
        }

        private void LoadBorder()
        {
            this.mBorderLeft = SkinManager.Docking.GetImage("border_left");
            this.mBorderRight = SkinManager.Docking.GetImage("border_right");
            this.mBorderBottom = SkinManager.Docking.GetImage("border_bottom");
            this.mBorderBottomLeft = SkinManager.Docking.GetImage("border_bottomleft");
            this.mBorderBottomRight = SkinManager.Docking.GetImage("border_bottomright");
            this.RootPanel.Left = this.BorderLeft.Width;
            this.RootPanel.Width = base.Width - (this.BorderLeft.Width + this.BorderRight.Width);
            this.RootPanel.Top = this.TitleBarPanel.Height;
            this.RootPanel.Height = base.Height - (this.BorderBottom.Height + this.TitleBarPanel.Height);
            this.RootPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
        }

        public void Minimize()
        {
            if ((base.WindowState == FormWindowState.Normal) || (base.WindowState == FormWindowState.Maximized))
            {
                base.WindowState = FormWindowState.Minimized;
            }
            else if (base.WindowState == FormWindowState.Minimized)
            {
                this.Restore();
            }
        }

        private void Mouse_ButtonDown(object sender, MouseEventArgs e)
        {
            if ((((e.Button == MouseButtons.Left) && !this.IsResizing) && (!base.IsDisposed && !base.Disposing)) && base.Bounds.Contains(e.Location))
            {
                Point point = base.PointToClient(e.Location);
                if ((point.X < this.ResizeThreshold) && (point.Y < this.ResizeThreshold))
                {
                    this.Cursor = Cursors.SizeNWSE;
                    this.mResizeDirection = ResizeDirections.TopLeft;
                    this.mIsResizing = true;
                }
                else if ((point.X > (base.Width - this.ResizeThreshold)) && (point.Y < this.ResizeThreshold))
                {
                    this.Cursor = Cursors.SizeNESW;
                    this.mResizeDirection = ResizeDirections.TopRight;
                    this.mIsResizing = true;
                }
                else if ((point.X > (base.Width - this.ResizeThreshold)) && (point.Y > (base.Height - this.ResizeThreshold)))
                {
                    this.Cursor = Cursors.SizeNWSE;
                    this.mResizeDirection = ResizeDirections.BottomRight;
                    this.mIsResizing = true;
                }
                else if ((point.X < this.ResizeThreshold) && (point.Y > (base.Height - this.ResizeThreshold)))
                {
                    this.Cursor = Cursors.SizeNESW;
                    this.mResizeDirection = ResizeDirections.BottomLeft;
                    this.mIsResizing = true;
                }
                else if (point.X < this.ResizeThreshold)
                {
                    this.Cursor = Cursors.SizeWE;
                    this.mResizeDirection = ResizeDirections.Left;
                    this.mIsResizing = true;
                }
                else if (point.Y < this.ResizeThreshold)
                {
                    this.Cursor = Cursors.SizeNS;
                    this.mResizeDirection = ResizeDirections.Top;
                    this.mIsResizing = true;
                }
                else if (point.X > (base.Width - this.ResizeThreshold))
                {
                    this.Cursor = Cursors.SizeWE;
                    this.mResizeDirection = ResizeDirections.Right;
                    this.mIsResizing = true;
                }
                else if (point.Y > (base.Height - this.ResizeThreshold))
                {
                    this.Cursor = Cursors.SizeNS;
                    this.mResizeDirection = ResizeDirections.Bottom;
                    this.mIsResizing = true;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    this.mResizeDirection = ResizeDirections.None;
                    this.mIsResizing = false;
                }
                if (this.IsResizing)
                {
                    this.LastDrag = Environment.TickCount;
                    base.SuspendLayout();
                    this.mLayoutSuspended = true;
                    if (this.Dragging)
                    {
                        this.EndDrag();
                    }
                    this.ResizeStartLoc = e.Location;
                    this.ResizeStartSize = base.Size;
                }
            }
        }

        private void Mouse_ButtonUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.IsResizing)
            {
                base.ResumeLayout(true);
                this.mLayoutSuspended = false;
                this.Refresh();
                this.mIsResizing = false;
                this.Cursor = Cursors.Default;
                this.mResizeDirection = ResizeDirections.None;
            }
        }

        private void Mouse_Moved(object sender, MouseEventArgs e)
        {
            if ((e.Button != MouseButtons.Left) || !this.IsResizing)
            {
                if (((base.WindowState == FormWindowState.Normal) && !base.IsDisposed) && !base.Disposing)
                {
                    Point point = base.PointToClient(e.Location);
                    if ((point.X < this.ResizeThreshold) && (point.Y < this.ResizeThreshold))
                    {
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else if ((point.X > (base.Width - this.ResizeThreshold)) && (point.Y < this.ResizeThreshold))
                    {
                        this.Cursor = Cursors.SizeNESW;
                    }
                    else if ((point.X > (base.Width - this.ResizeThreshold)) && (point.Y > (base.Height - this.ResizeThreshold)))
                    {
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else if ((point.X < this.ResizeThreshold) && (point.Y > (base.Height - this.ResizeThreshold)))
                    {
                        this.Cursor = Cursors.SizeNESW;
                    }
                    else if (point.X < this.ResizeThreshold)
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                    else if (point.Y < this.ResizeThreshold)
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                    else if (point.X > (base.Width - this.ResizeThreshold))
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                    else if (point.Y > (base.Height - this.ResizeThreshold))
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                    else if (!this.Cursor.Equals(Cursors.Default))
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            else
            {
                bool flag = base.Width > this.MinimumSize.Width;
                bool flag2 = base.Height > this.MinimumSize.Height;
                Size size = new Size(e.Location.X - this.ResizeStartLoc.X, e.Location.Y - this.ResizeStartLoc.Y);
                switch (this.ResizeDirection)
                {
                    case ResizeDirections.TopLeft:
                        if (flag)
                        {
                            base.Left = this.ResizeStartLoc.X + size.Width;
                        }
                        if (flag2)
                        {
                            base.Top = this.ResizeStartLoc.Y + size.Height;
                        }
                        base.Width = this.ResizeStartSize.Width - size.Width;
                        base.Height = this.ResizeStartSize.Height - size.Height;
                        break;

                    case ResizeDirections.Top:
                        if (flag2)
                        {
                            base.Top = this.ResizeStartLoc.Y + size.Height;
                        }
                        base.Height = this.ResizeStartSize.Height - size.Height;
                        break;

                    case ResizeDirections.TopRight:
                        if (flag2)
                        {
                            base.Top = this.ResizeStartLoc.Y + size.Height;
                        }
                        base.Width = this.ResizeStartSize.Width + size.Width;
                        base.Height = this.ResizeStartSize.Height - size.Height;
                        break;

                    case ResizeDirections.Right:
                        base.Width = this.ResizeStartSize.Width + size.Width;
                        break;

                    case ResizeDirections.BottomRight:
                        base.Width = this.ResizeStartSize.Width + size.Width;
                        base.Height = this.ResizeStartSize.Height + size.Height;
                        break;

                    case ResizeDirections.Bottom:
                        base.Height = this.ResizeStartSize.Height + size.Height;
                        break;

                    case ResizeDirections.BottomLeft:
                        if (flag)
                        {
                            base.Left = this.ResizeStartLoc.X + size.Width;
                        }
                        base.Width = this.ResizeStartSize.Width - size.Width;
                        base.Height = this.ResizeStartSize.Height + size.Height;
                        break;

                    case ResizeDirections.Left:
                        if (flag)
                        {
                            base.Left = this.ResizeStartLoc.X + size.Width;
                        }
                        base.Width = this.ResizeStartSize.Width - size.Width;
                        break;
                }
                if ((Environment.TickCount - this.LastDrag) > 0x3e8)
                {
                    base.ResumeLayout(true);
                    Application.DoEvents();
                    base.SuspendLayout();
                }
                this.LastDrag = Environment.TickCount;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!this.IsClosing)
            {
                this.mIsClosing = true;
                foreach (DockPanel panel in this.Panels.ToArray())
                {
                    panel.Close();
                }
                this.Controller.DestroyContainer(this);
                GlobalHook.Mouse.Moved -= this._Mouse_Moved;
                GlobalHook.Mouse.ButtonDown -= this._Mouse_ButtonDown;
                GlobalHook.Mouse.ButtonUp -= this._Mouse_ButtonUp;
                base.OnClosing(e);
            }
        }

        internal void OnDocked(DockPanel panel)
        {
            this.Panels.Add(panel);
            if (this.Docked != null)
            {
                this.Docked(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.LayoutSuspended)
            {
                this.DrawBorder(e.Graphics);
            }
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.SetRegion();
        }

        internal void OnUndocked(DockPanel panel)
        {
            this.Panels.Remove(panel);
            if (this.Undocked != null)
            {
                this.Undocked(this, EventArgs.Empty);
            }
        }

        internal void PauseDrag()
        {
            base.InvokeLostFocus(this, EventArgs.Empty);
            this.DragPaused = true;
        }

        public void Restore()
        {
            if (base.WindowState == FormWindowState.Normal)
            {
                base.WindowState = FormWindowState.Maximized;
            }
            else if ((base.WindowState == FormWindowState.Maximized) || (base.WindowState == FormWindowState.Minimized))
            {
                base.WindowState = FormWindowState.Normal;
            }
        }

        internal void ResumeDrag()
        {
            this.DragPaused = false;
        }

        private void SetRegion()
        {
            Rectangle clientRectangle = base.ClientRectangle;
            Rectangle rectangle2 = new Rectangle(base.PointToScreen(new Point(0, 0)).X, base.PointToScreen(new Point(0, 0)).Y, base.Width, base.Height);
            this.mWidthDiff = rectangle2.X - base.Bounds.X;
            this.mHeightDiff = rectangle2.Y - base.Bounds.Y;
            int hRgn = CreateRoundRectRgn(clientRectangle.Left, clientRectangle.Top, clientRectangle.Right + 1, clientRectangle.Bottom, this.FormBorderCurve, this.FormBorderCurve);
            SetWindowRgn(base.Handle.ToInt32(), hRgn, 1);
        }

        [DllImport("user32.DLL")]
        private static extern int SetWindowRgn(int hWnd, int hRgn, int bRedraw);
        public void Undock()
        {
            throw new Exception("A DockContainerForm may only be the target of docking operations, not the source.");
        }

        public Image BorderBottom
        {
            get
            {
                return this.mBorderBottom;
            }
            set
            {
                this.mBorderBottom = value;
            }
        }

        public Image BorderBottomLeft
        {
            get
            {
                return this.mBorderBottomLeft;
            }
            set
            {
                this.mBorderBottomLeft = value;
            }
        }

        public Image BorderBottomRight
        {
            get
            {
                return this.mBorderBottomRight;
            }
            set
            {
                this.mBorderBottomRight = value;
            }
        }

        public Image BorderLeft
        {
            get
            {
                return this.mBorderLeft;
            }
            set
            {
                this.mBorderLeft = value;
            }
        }

        public Image BorderRight
        {
            get
            {
                return this.mBorderRight;
            }
        }

        public Control ContainerControl
        {
            get
            {
                if (this.mContainerControl == null)
                {
                    this.mContainerControl = this;
                }
                return this.mContainerControl;
            }
            set
            {
                this.mContainerControl = value;
            }
        }

        public DockContainerForm ContainerForm
        {
            get
            {
                return this;
            }
            set
            {
            }
        }

        public DockController Controller
        {
            get
            {
                return this.mController;
            }
            internal set
            {
                this.mController = value;
            }
        }

        public DockStyles DockStyle
        {
            get
            {
                return this.mDockStyle;
            }
            set
            {
                this.mDockStyle = value;
            }
        }

        public IDockableContainer DockTarget
        {
            get
            {
                return this.mDockTarget;
            }
            set
            {
                this.mDockTarget = value;
            }
        }

        public virtual int FormBorderCurve
        {
            get
            {
                return this.mFormBorderCurve;
            }
            set
            {
                this.mFormBorderCurve = value;
            }
        }

        public bool IsClosing
        {
            get
            {
                return this.mIsClosing;
            }
        }

        public bool IsResizing
        {
            get
            {
                return this.mIsResizing;
            }
        }

        public bool LayoutSuspended
        {
            get
            {
                return this.mLayoutSuspended;
            }
        }

        public virtual int OutterDockThreshhold
        {
            get
            {
                return this.mOutterDockThreshhold;
            }
        }

        public List<DockPanel> Panels
        {
            get
            {
                return this.mPanels;
            }
        }

        public virtual Color PreviewColor
        {
            get
            {
                return this.mPreviewColor;
            }
        }

        public bool PreviewingDockStyle
        {
            get
            {
                return (((this.PreviewStyle != DockStyles.None) && (this.PreviewStyle != DockStyles.Tabbed)) && (this.PreviewStyle != DockStyles.Floating));
            }
        }

        public virtual int PreviewOpacity
        {
            get
            {
                return this.mPreviewOpacity;
            }
        }

        internal DockStyles PreviewStyle
        {
            get
            {
                return this.mPreviewStyle;
            }
            set
            {
                this.mPreviewStyle = value;
                this.Refresh();
            }
        }

        public ResizeDirections ResizeDirection
        {
            get
            {
                return this.mResizeDirection;
            }
        }

        public virtual int ResizeThreshold
        {
            get
            {
                return this.mResizeThreshold;
            }
        }

        public Stack<DockStyles> StyleStack
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Padding TextPadding
        {
            get
            {
                return this.mTextPadding;
            }
            set
            {
                this.mTextPadding = value;
            }
        }

        public DockContainerTitleBar TitleBar
        {
            get
            {
                return this.mTitleBar;
            }
        }
    }
}

