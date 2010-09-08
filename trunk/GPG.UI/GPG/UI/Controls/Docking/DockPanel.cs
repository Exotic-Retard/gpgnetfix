namespace GPG.UI.Controls.Docking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DockPanel : UserControl, IDockableContainer
    {
        private DockAnchor[] Anchors;
        private IContainer components;
        private IButtonControl mAcceptButton;
        private IButtonControl mCancelButton;
        private Control mContainerControl;
        private DockContainerForm mContainerForm;
        private DockController mController;
        private int mDockOrder = -1;
        private DockStyles mDockStyle = DockStyles.Floating;
        private Image mIcon;
        private bool mIsPreviewing;
        private Stack<DockStyles> mStyleStack = new Stack<DockStyles>();
        private string mTitle = "";
        private DockTitleBar mTitleBar;
        private ToolTip mToolTipProvider = new ToolTip();
        private DockStyles PreviewOrientation = DockStyles.None;

        public event EventHandler Docked;

        public event DockStyleEventHandler DockStyleChanged;

        public DockPanel()
        {
            this.InitializeComponent();
            this.mTitleBar = new DockTitleBar(this);
            base.Controls.Add(this.TitleBar);
        }

        internal void BeginDockPreview(DockStyles orientation)
        {
            this.mIsPreviewing = true;
            this.PreviewOrientation = orientation;
            this.DrawDockPreview();
        }

        public void Close()
        {
            this.Undock();
            this.Controller.DestroyPanel(this);
        }

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
            DockTabGroup containerControl;
            this.Undock();
            this.DockStyle = orientation;
            DockSplitter splitter = null;
            if ((orientation != DockStyles.Tabbed) && (target.DockStyle == DockStyles.Tabbed))
            {
                target = target.ContainerControl as IDockableContainer;
            }
            switch (orientation)
            {
                case DockStyles.Left:
                    splitter = new DockSplitter(Orientation.Vertical);
                    target.ContainerControl.Controls.Remove(target as Control);
                    target.ContainerControl.Controls.Add(splitter);
                    target.ContainerControl = splitter.Panel2;
                    splitter.Panel2.Controls.Add(target as Control);
                    this.ContainerControl = splitter.Panel1;
                    splitter.Panel1.Controls.Add(this);
                    target.DockStyle = DockStyles.Right;
                    goto Label_02B7;

                case DockStyles.Right:
                    splitter = new DockSplitter(Orientation.Vertical);
                    target.ContainerControl.Controls.Remove(target as Control);
                    target.ContainerControl.Controls.Add(splitter);
                    target.ContainerControl = splitter.Panel1;
                    splitter.Panel1.Controls.Add(target as Control);
                    this.ContainerControl = splitter.Panel2;
                    splitter.Panel2.Controls.Add(this);
                    target.DockStyle = DockStyles.Left;
                    goto Label_02B7;

                case DockStyles.Top:
                    splitter = new DockSplitter(Orientation.Horizontal);
                    target.ContainerControl.Controls.Remove(target as Control);
                    target.ContainerControl.Controls.Add(splitter);
                    target.ContainerControl = splitter.Panel2;
                    splitter.Panel2.Controls.Add(target as Control);
                    this.ContainerControl = splitter.Panel1;
                    splitter.Panel1.Controls.Add(this);
                    target.DockStyle = DockStyles.Bottom;
                    goto Label_02B7;

                case DockStyles.Bottom:
                    splitter = new DockSplitter(Orientation.Horizontal);
                    target.ContainerControl.Controls.Remove(target as Control);
                    target.ContainerControl.Controls.Add(splitter);
                    target.ContainerControl = splitter.Panel1;
                    splitter.Panel1.Controls.Add(target as Control);
                    this.ContainerControl = splitter.Panel2;
                    splitter.Panel2.Controls.Add(this);
                    target.DockStyle = DockStyles.Top;
                    goto Label_02B7;

                case DockStyles.Tabbed:
                    if (target.DockStyle != DockStyles.Tabbed)
                    {
                        containerControl = new DockTabGroup(TabOrientations.Bottom);
                        containerControl.ContainerForm = target.ContainerForm;
                        containerControl.AddTab(target as DockPanel);
                        containerControl.ContainerControl = target.ContainerControl;
                        target.ContainerControl.Controls.Remove(target as Control);
                        target.ContainerControl.Controls.Add(containerControl);
                        target.ContainerControl = containerControl;
                        break;
                    }
                    containerControl = target.ContainerControl as DockTabGroup;
                    break;

                default:
                    goto Label_02B7;
            }
            containerControl.AddTab(this);
            this.ContainerControl = containerControl;
            target.DockStyle = DockStyles.Tabbed;
        Label_02B7:
            this.ContainerForm = target.ContainerForm;
            this.ContainerForm.OnDocked(this);
        }

        private void DrawDockPreview()
        {
            Rectangle region = new Rectangle();
            Rectangle clientRectangle = this.ClientRectangle;
            Point point = base.PointToScreen(base.Location);
            clientRectangle.X = point.X + this.ClientRectangle.X;
            clientRectangle.Y = point.Y + this.ClientRectangle.Y;
            switch (this.PreviewOrientation)
            {
                case DockStyles.Left:
                    region.X = clientRectangle.X;
                    region.Y = clientRectangle.Y;
                    region.Width = clientRectangle.Width / 2;
                    region.Height = clientRectangle.Height;
                    break;

                case DockStyles.Right:
                    region.X = clientRectangle.X + (clientRectangle.Width / 2);
                    region.Y = clientRectangle.Y;
                    region.Width = clientRectangle.Width / 2;
                    region.Height = clientRectangle.Height;
                    break;

                case DockStyles.Top:
                    region.X = clientRectangle.X;
                    region.Y = clientRectangle.Y;
                    region.Width = clientRectangle.Width;
                    region.Height = clientRectangle.Height / 2;
                    break;

                case DockStyles.Bottom:
                    region.X = clientRectangle.X;
                    region.Y = clientRectangle.Y + (clientRectangle.Height / 2);
                    region.Width = clientRectangle.Width;
                    region.Height = clientRectangle.Height / 2;
                    break;

                case DockStyles.Tabbed:
                    region = clientRectangle;
                    break;
            }
            this.Controller.BeginPreviewSnapshot(region);
        }

        internal void EndDockPreview()
        {
            this.mIsPreviewing = false;
            this.Controller.EndPreviewSnapshot();
        }

        internal void HideAnchors()
        {
            for (int i = 0; i < this.Anchors.Length; i++)
            {
                this.Anchors[i].Visible = false;
            }
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ForeColor = Color.White;
            base.Name = "DockPanel";
            base.ResumeLayout(false);
        }

        internal void OnDocked()
        {
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            if (this.Docked != null)
            {
                this.Docked(this, EventArgs.Empty);
            }
        }

        private void OnDockStyleChanged(DockStyles style)
        {
            if (this.DockStyleChanged != null)
            {
                this.DockStyleChanged(this, style);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.ContainerForm.AcceptButton = this.AcceptButton;
            this.ContainerForm.CancelButton = this.CancelButton;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.TitleBar.BringToFront();
            if (DockController.IsInitialized)
            {
                this.Anchors = new DockAnchor[] { new DockAnchor(DockStyles.Top), new DockAnchor(DockStyles.Bottom), new DockAnchor(DockStyles.Left), new DockAnchor(DockStyles.Right), new DockAnchor(DockStyles.Tabbed) };
                for (int i = 0; i < this.Anchors.Length; i++)
                {
                    base.Controls.Add(this.Anchors[i]);
                }
            }
        }

        internal void ShowAnchors()
        {
            for (int i = 0; i < this.Anchors.Length; i++)
            {
                this.Anchors[i].Visible = true;
                this.Anchors[i].BringToFront();
            }
        }

        public override string ToString()
        {
            if ((this.Title != null) && (this.Title.Length >= 1))
            {
                return this.Title;
            }
            return "Untitled Panel";
        }

        public void Undock()
        {
            if (this.IsDocked)
            {
                this.ContainerForm.OnUndocked(this);
                this.mContainerForm = null;
                if (this.ContainerControl is SplitterPanel)
                {
                    this.DockStyle = DockStyles.Floating;
                    Control parent = this.ContainerControl.Parent;
                    while ((parent != null) && !(parent is DockSplitter))
                    {
                        parent = parent.Parent;
                    }
                    if (parent == null)
                    {
                        throw new InvalidOperationException(string.Format("Unable to locate the parent (DockSplitter) of the containing control for {0}", this));
                    }
                    DockSplitter splitter = parent as DockSplitter;
                    Control topLevelControl = splitter.Parent;
                    if ((topLevelControl == null) && (base.TopLevelControl != null))
                    {
                        topLevelControl = base.TopLevelControl;
                    }
                    else if ((topLevelControl == null) && (base.TopLevelControl == null))
                    {
                        throw new InvalidOperationException(string.Format("Unable to locate the parent of the DockSplitter containing panel {0}", this));
                    }
                    Control control3 = splitter.Panel2;
                    if (this.ContainerControl.Equals(control3))
                    {
                        control3 = splitter.Panel1;
                    }
                    if (control3.Controls.Count < 1)
                    {
                        throw new ArgumentOutOfRangeException("Sibling panel to the target undocking has no child controls.");
                    }
                    Control control4 = control3.Controls[0];
                    topLevelControl.Controls.Clear();
                    topLevelControl.Controls.Add(control4);
                    if ((control4 is DockPanel) && (topLevelControl is SplitterPanel))
                    {
                        DockPanel panel = control4 as DockPanel;
                        panel.ContainerControl = topLevelControl;
                        parent = topLevelControl.Parent;
                        while ((parent != null) && !(parent is DockSplitter))
                        {
                            parent = parent.Parent;
                        }
                        if (parent == null)
                        {
                            throw new InvalidOperationException(string.Format("Unable to locate the parent (DockSplitter) of the containing control for {0}", this));
                        }
                        splitter = parent as DockSplitter;
                        bool flag = false;
                        if (splitter.Panel1.Equals(topLevelControl))
                        {
                            flag = true;
                        }
                        if (flag && (splitter.Orientation == Orientation.Horizontal))
                        {
                            panel.DockStyle = DockStyles.Top;
                        }
                        else if (flag && (splitter.Orientation == Orientation.Vertical))
                        {
                            panel.DockStyle = DockStyles.Left;
                        }
                        else if (!flag && (splitter.Orientation == Orientation.Horizontal))
                        {
                            panel.DockStyle = DockStyles.Bottom;
                        }
                        else
                        {
                            if (flag || (splitter.Orientation != Orientation.Vertical))
                            {
                                throw new InvalidOperationException("Unable to determing the DockStyle of the sibling panel of the undocking panel");
                            }
                            panel.DockStyle = DockStyles.Right;
                        }
                    }
                    else if ((control4 is DockPanel) && (topLevelControl is DockContainerRootPanel))
                    {
                        DockPanel panel2 = control4 as DockPanel;
                        panel2.ContainerControl = topLevelControl;
                        panel2.DockStyle = DockStyles.Floating;
                    }
                }
                else if (this.ContainerControl is DockTabGroup)
                {
                    DockTabGroup containerControl = this.ContainerControl as DockTabGroup;
                    containerControl.RemoveTab(this);
                    base.Show();
                    if (containerControl.Panels.Count == 1)
                    {
                        Control control5 = containerControl.ContainerControl;
                        DockPanel panel3 = containerControl.Panels[0];
                        control5.Controls.Remove(containerControl);
                        control5.Controls.Add(panel3);
                        if (control5 is SplitterPanel)
                        {
                            panel3.ContainerControl = control5;
                            Control control6 = control5.Parent;
                            while ((control6 != null) && !(control6 is DockSplitter))
                            {
                                control6 = control6.Parent;
                            }
                            if (control6 == null)
                            {
                                throw new InvalidOperationException(string.Format("Unable to locate the parent (DockSplitter) of the containing control for {0}", this));
                            }
                            DockSplitter splitter2 = control6 as DockSplitter;
                            bool flag2 = false;
                            if (splitter2.Panel1.Equals(control5))
                            {
                                flag2 = true;
                            }
                            if (flag2 && (splitter2.Orientation == Orientation.Horizontal))
                            {
                                panel3.DockStyle = DockStyles.Top;
                            }
                            else if (flag2 && (splitter2.Orientation == Orientation.Vertical))
                            {
                                panel3.DockStyle = DockStyles.Left;
                            }
                            else if (!flag2 && (splitter2.Orientation == Orientation.Horizontal))
                            {
                                panel3.DockStyle = DockStyles.Bottom;
                            }
                            else
                            {
                                if (flag2 || (splitter2.Orientation != Orientation.Vertical))
                                {
                                    throw new InvalidOperationException("Unable to determing the DockStyle of the sibling panel of the undocking panel");
                                }
                                panel3.DockStyle = DockStyles.Right;
                            }
                        }
                        else if (control5 is DockContainerRootPanel)
                        {
                            panel3.ContainerControl = control5;
                            panel3.DockStyle = DockStyles.Floating;
                        }
                    }
                }
                else if (this.ContainerControl is DockContainerRootPanel)
                {
                    throw new Exception("Undocking panel is the direct child of a DockContainer form, but is attempting to undock from a sibling panel");
                }
            }
            else if (this.IsFloating && (this.ContainerForm != null))
            {
                this.ContainerForm.Panels.Clear();
                this.ContainerForm.Controls.Clear();
                this.ContainerForm.Close();
            }
        }

        public IButtonControl AcceptButton
        {
            get
            {
                return this.mAcceptButton;
            }
            set
            {
                this.mAcceptButton = value;
            }
        }

        public IButtonControl CancelButton
        {
            get
            {
                return this.mCancelButton;
            }
            set
            {
                this.mCancelButton = value;
            }
        }

        public virtual Rectangle ClientRectangle
        {
            get
            {
                return new Rectangle(0, this.TitleBar.Height, base.Width, base.Height - this.TitleBar.Height);
            }
        }

        public Control ContainerControl
        {
            get
            {
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
                return this.mContainerForm;
            }
            set
            {
                this.mContainerForm = value;
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

        public int DockOrder
        {
            get
            {
                return this.mDockOrder;
            }
            internal set
            {
                this.mDockOrder = value;
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
                this.OnDockStyleChanged(this.mDockStyle);
            }
        }

        [Browsable(true)]
        public Image Icon
        {
            get
            {
                return this.mIcon;
            }
            set
            {
                this.mIcon = value;
                if (this.mIcon is Bitmap)
                {
                    (this.mIcon as Bitmap).MakeTransparent();
                }
            }
        }

        public bool IsDocked
        {
            get
            {
                if (((this.DockStyle != DockStyles.Top) && (this.DockStyle != DockStyles.Bottom)) && ((this.DockStyle != DockStyles.Left) && (this.DockStyle != DockStyles.Right)))
                {
                    return (this.DockStyle == DockStyles.Tabbed);
                }
                return true;
            }
        }

        public bool IsFloating
        {
            get
            {
                return !this.IsDocked;
            }
        }

        public bool IsPreviewing
        {
            get
            {
                return this.mIsPreviewing;
            }
        }

        public bool IsTabbed
        {
            get
            {
                return (this.DockStyle == DockStyles.Tabbed);
            }
        }

        public Stack<DockStyles> StyleStack
        {
            get
            {
                return this.mStyleStack;
            }
        }

        [Browsable(true)]
        public string Title
        {
            get
            {
                return this.mTitle;
            }
            set
            {
                this.mTitle = value;
                if (this.TitleBar != null)
                {
                    this.TitleBar.Refresh();
                }
            }
        }

        public DockTitleBar TitleBar
        {
            get
            {
                return this.mTitleBar;
            }
        }

        public ToolTip ToolTipProvider
        {
            get
            {
                return this.mToolTipProvider;
            }
        }
    }
}

