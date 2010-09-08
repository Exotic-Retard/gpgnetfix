namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Properties;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Timers;
    using System.Windows.Forms;

    public class DlgBase : FrmBase, IStatusProvider
    {
        private IContainer components;
        private string dirname;
        public const uint FLASHW_ALL = 3;
        public const uint FLASHW_CAPTION = 1;
        public const uint FLASHW_STOP = 0;
        public const uint FLASHW_TIMER = 4;
        public const uint FLASHW_TIMERNOFG = 12;
        public const uint FLASHW_TRAY = 2;
        private bool HasShown;
        private Font mCaptionFont;
        private bool mChangingWindowState;
        private ErrorProvider mErrors;
        private int mHeightDiff;
        private bool mIsMaximized;
        private bool mIsMoving;
        private bool mIsResizing;
        private DateTime mLastChange;
        private int mLastHeight;
        private int mLastMouseX;
        private int mLastMouseY;
        private int mLastWidth;
        private int mLastX;
        private int mLastY;
        private object mLayoutData;
        private FrmMain mMainForm;
        private Size mMinimumSize;
        private Rectangle mResizeRect;
        private Rectangle mRestore;
        private bool mRestoreWindowMode;
        private bool mStartCenterScreen;
        private bool mStartResizing;
        private bool mTestLog;
        private bool mTileEffect;
        private int mWidthDiff;
        protected PictureBox pbBottom;
        private PictureBox pbBottomLeft;
        private PictureBox pbBottomRight;
        private PictureBox pbClose;
        private PictureBox pbLeft;
        private PictureBox pbMinimize;
        private Panel pBottom;
        private PictureBox pbRestore;
        private PictureBox pbRight;
        private SkinStatusStrip pbTop;
        private PictureBox pbTopLeft;
        private PictureBox pbTopRight;
        private Panel pTop;
        private int RESIZE_RECT;
        private StateTimer StatusTimer;

        public event StatusProviderEventHandler StatusChanged;

        public DlgBase()
        {
            this.components = null;
            this.mTestLog = false;
            this.mChangingWindowState = false;
            this.mLastChange = DateTime.Now;
            this.mMinimumSize = new Size();
            this.mRestoreWindowMode = false;
            this.mLayoutData = null;
            this.mErrors = new ErrorProvider();
            this.StatusTimer = null;
            this.mTileEffect = true;
            this.mStartCenterScreen = false;
            this.HasShown = false;
            this.dirname = "";
            this.mIsMaximized = false;
            this.mRestore = new Rectangle(0, 0, 100, 100);
            this.mHeightDiff = 0;
            this.mWidthDiff = 0;
            this.mLastX = 0;
            this.mLastY = 0;
            this.mLastMouseX = 0;
            this.mLastMouseY = 0;
            this.mLastWidth = 0;
            this.mLastHeight = 0;
            this.mIsMoving = false;
            this.mIsResizing = false;
            this.mStartResizing = false;
            this.RESIZE_RECT = 40;
            this.mResizeRect = new Rectangle();
            this.mCaptionFont = new Font("Arial", 10f, FontStyle.Bold);
            this.mMainForm = null;
            this.InitializeComponent();
            this.MainForm = Program.MainForm;
            this.SetRegion();
            this.Construct();
            base.Opacity = 1.0;
        }

        public DlgBase(FrmMain mainForm)
        {
            this.components = null;
            this.mTestLog = false;
            this.mChangingWindowState = false;
            this.mLastChange = DateTime.Now;
            this.mMinimumSize = new Size();
            this.mRestoreWindowMode = false;
            this.mLayoutData = null;
            this.mErrors = new ErrorProvider();
            this.StatusTimer = null;
            this.mTileEffect = true;
            this.mStartCenterScreen = false;
            this.HasShown = false;
            this.dirname = "";
            this.mIsMaximized = false;
            this.mRestore = new Rectangle(0, 0, 100, 100);
            this.mHeightDiff = 0;
            this.mWidthDiff = 0;
            this.mLastX = 0;
            this.mLastY = 0;
            this.mLastMouseX = 0;
            this.mLastMouseY = 0;
            this.mLastWidth = 0;
            this.mLastHeight = 0;
            this.mIsMoving = false;
            this.mIsResizing = false;
            this.mStartResizing = false;
            this.RESIZE_RECT = 40;
            this.mResizeRect = new Rectangle();
            this.mCaptionFont = new Font("Arial", 10f, FontStyle.Bold);
            this.mMainForm = null;
            this.InitializeComponent();
            this.SetRegion();
            this.MainForm = mainForm;
            this.Construct();
        }

        protected virtual void BindToSettings()
        {
            Program.Settings.SkinNameChanged += delegate (object s, PropertyChangedEventArgs e) {
                VGen0 method = null;
                if (base.InvokeRequired)
                {
                    if (!base.Disposing && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.SetSkin(Program.Settings.SkinName);
                            };
                        }
                        base.BeginInvoke(method);
                    }
                }
                else
                {
                    this.SetSkin(Program.Settings.SkinName);
                }
            };
            Program.Settings.StylePreferences.MasterBackColorChanged += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.MasterFontChanged += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.MasterForeColorChanged += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.HighlightColor1Changed += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.HighlightColor2Changed += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.StylePreferences.HighlightColor3Changed += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.Appearance.Text.StatusColorChanged += new PropertyChangedEventHandler(this.StyleApplication);
            Program.Settings.Appearance.Text.StatusFontChanged += new PropertyChangedEventHandler(this.StyleApplication);
            base.Disposed += delegate (object s, EventArgs e) {
                PropertyChangedEventHandler handler = null;
                try
                {
                    Program.Settings.StylePreferences.MasterBackColorChanged -= new PropertyChangedEventHandler(this.StyleApplication);
                    Program.Settings.StylePreferences.MasterFontChanged -= new PropertyChangedEventHandler(this.StyleApplication);
                    Program.Settings.StylePreferences.MasterForeColorChanged -= new PropertyChangedEventHandler(this.StyleApplication);
                    Program.Settings.StylePreferences.HighlightColor1Changed -= new PropertyChangedEventHandler(this.StyleApplication);
                    Program.Settings.StylePreferences.HighlightColor2Changed -= new PropertyChangedEventHandler(this.StyleApplication);
                    Program.Settings.StylePreferences.HighlightColor3Changed -= new PropertyChangedEventHandler(this.StyleApplication);
                    Program.Settings.Appearance.Text.StatusColorChanged -= new PropertyChangedEventHandler(this.StyleApplication);
                    Program.Settings.Appearance.Text.StatusFontChanged -= new PropertyChangedEventHandler(this.StyleApplication);
                    if (handler == null)
                    {
                        handler = delegate (object s1, PropertyChangedEventArgs e1) {
                            VGen0 method = null;
                            if (base.InvokeRequired)
                            {
                                if (!base.Disposing && !base.IsDisposed)
                                {
                                    if (method == null)
                                    {
                                        method = delegate {
                                            this.SetSkin(Program.Settings.SkinName);
                                        };
                                    }
                                    base.Invoke(method);
                                }
                            }
                            else
                            {
                                this.SetSkin(Program.Settings.SkinName);
                            }
                        };
                    }
                    Program.Settings.SkinNameChanged -= handler;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            };
        }

        private void CheckControl(Control control)
        {
            foreach (Control control2 in control.Controls)
            {
                this.CheckControl(control2);
            }
            control.MouseMove += new MouseEventHandler(this.OnMouseMove);
            control.MouseUp += new MouseEventHandler(this.OnMouseUp);
        }

        protected internal void ClearErrors()
        {
            this.Errors.Clear();
        }

        public void ClearStatus()
        {
            this.SetStatus("", new object[0]);
        }

        private void Construct()
        {
            MouseEventHandler handler = new MouseEventHandler(this.this_MouseDown);
            base.MouseDown += new MouseEventHandler(handler.Invoke);
            this.pbTop.MouseDown += new MouseEventHandler(handler.Invoke);
            this.pbTopLeft.MouseDown += new MouseEventHandler(handler.Invoke);
            this.pbTopRight.MouseDown += new MouseEventHandler(handler.Invoke);
            this.pbBottomRight.MouseDown += new MouseEventHandler(handler.Invoke);
            MouseEventHandler handler2 = new MouseEventHandler(this.this_MouseUp);
            base.MouseUp += new MouseEventHandler(handler2.Invoke);
            this.pbTop.MouseUp += new MouseEventHandler(handler2.Invoke);
            this.pbTopLeft.MouseUp += new MouseEventHandler(handler2.Invoke);
            this.pbTopRight.MouseUp += new MouseEventHandler(handler2.Invoke);
            if (!base.DesignMode)
            {
                base.Shown += new EventHandler(this.ReSkin);
            }
            this.BindToSettings();
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

        protected internal void Error(string error, params object[] args)
        {
            this.Error(this.pbTop, error, args);
        }

        protected internal void Error(Control[] controls, string error, params object[] args)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                this.Error(controls[i], error, args);
            }
        }

        protected internal void Error(Control control, string error, params object[] args)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.Errors.SetError(control, string.Format(Loc.Get(error), args));
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.Errors.SetError(control, string.Format(Loc.Get(error), args));
            }
        }

        public bool FlashWindow()
        {
            FLASHWINFO structure = new FLASHWINFO();
            structure.cbSize = (ushort) Marshal.SizeOf(structure);
            structure.hwnd = base.Handle;
            structure.dwFlags = 6;
            structure.uCount = 0xffff;
            structure.dwTimeout = 0;
            return (FlashWindowEx(ref structure) == 0);
        }

        [DllImport("user32.dll")]
        private static extern short FlashWindowEx(ref FLASHWINFO pwfi);
        public virtual void FocusInput()
        {
            base.Focus();
        }

        private void FrmBaseStyled_Load(object sender, EventArgs e)
        {
            this.pbRight.Top = this.pbTop.Bottom;
            this.pbRight.Left = base.Width - this.pbRight.Image.Width;
            this.pbMinimize.Left = base.ClientRectangle.Width - Program.Settings.StylePreferences.NavInsetFromRight;
            this.pbRestore.Left = this.pbMinimize.Right + 1;
            this.pbClose.Left = this.pbRestore.Right + 1;
            this.CheckControl(this);
        }

        private bool HitTest(int x, int y, int left, int top, int right, int bottom)
        {
            return ((((x >= left) && (x <= right)) && (y >= top)) && (y <= bottom));
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgBase));
            this.pbTop = new SkinStatusStrip();
            this.pTop = new Panel();
            this.pbTopRight = new PictureBox();
            this.pbTopLeft = new PictureBox();
            this.pbMinimize = new PictureBox();
            this.pbClose = new PictureBox();
            this.pbRestore = new PictureBox();
            this.pBottom = new Panel();
            this.pbBottomRight = new PictureBox();
            this.pbBottom = new PictureBox();
            this.pbBottomLeft = new PictureBox();
            this.pbLeft = new PictureBox();
            this.pbRight = new PictureBox();
            this.pTop.SuspendLayout();
            ((ISupportInitialize) this.pbTopRight).BeginInit();
            ((ISupportInitialize) this.pbTopLeft).BeginInit();
            ((ISupportInitialize) this.pbMinimize).BeginInit();
            ((ISupportInitialize) this.pbClose).BeginInit();
            ((ISupportInitialize) this.pbRestore).BeginInit();
            this.pBottom.SuspendLayout();
            ((ISupportInitialize) this.pbBottomRight).BeginInit();
            ((ISupportInitialize) this.pbBottom).BeginInit();
            ((ISupportInitialize) this.pbBottomLeft).BeginInit();
            ((ISupportInitialize) this.pbLeft).BeginInit();
            ((ISupportInitialize) this.pbRight).BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.pbTop.AutoStyle = false;
            this.pbTop.BackColor = Color.Transparent;
            this.pbTop.Dock = DockStyle.Fill;
            this.pbTop.DrawEdges = false;
            this.pbTop.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.pbTop.ForeColor = Color.White;
            this.pbTop.HorizontalScalingMode = ScalingModes.Tile;
            this.pbTop.IsStyled = false;
            this.pbTop.ItemPadding = 6;
            this.pbTop.Location = new Point(0x3f, 0);
            this.pbTop.Margin = new Padding(3, 4, 3, 4);
            this.pbTop.Name = "pbTop";
            this.pbTop.Size = new Size(0x241, 0x4c);
            this.pbTop.SkinBasePath = @"Controls\StatusStrip\Dialog";
            base.ttDefault.SetSuperTip(this.pbTop, null);
            this.pbTop.TabIndex = 1;
            this.pbTop.TextAlign = ContentAlignment.MiddleLeft;
            this.pbTop.TextPadding = new Padding(0, 0, 8, 0);
            this.pbTop.DoubleClick += new EventHandler(this.pbRestore_Click);
            this.pbTop.Paint += new PaintEventHandler(this.pbTop_Paint);
            this.pbTop.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbTop.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pTop.Controls.Add(this.pbTopRight);
            this.pTop.Controls.Add(this.pbTop);
            this.pTop.Controls.Add(this.pbTopLeft);
            this.pTop.Dock = DockStyle.Top;
            this.pTop.Location = new Point(0, 0);
            this.pTop.Margin = new Padding(3, 4, 3, 4);
            this.pTop.Name = "pTop";
            this.pTop.Size = new Size(640, 0x4c);
            base.ttDefault.SetSuperTip(this.pTop, null);
            this.pTop.TabIndex = 0;
            this.pTop.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pTop.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbTopRight.Dock = DockStyle.Right;
            this.pbTopRight.Image = Resources.dialog_topright;
            this.pbTopRight.Location = new Point(0x241, 0);
            this.pbTopRight.Margin = new Padding(3, 4, 3, 4);
            this.pbTopRight.Name = "pbTopRight";
            this.pbTopRight.Size = new Size(0x3f, 0x4c);
            this.pbTopRight.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbTopRight, null);
            this.pbTopRight.TabIndex = 2;
            this.pbTopRight.TabStop = false;
            this.pbTopRight.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbTopRight.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbTopLeft.Dock = DockStyle.Left;
            this.pbTopLeft.Image = Resources.dialog_topleft;
            this.pbTopLeft.Location = new Point(0, 0);
            this.pbTopLeft.Margin = new Padding(3, 4, 3, 4);
            this.pbTopLeft.Name = "pbTopLeft";
            this.pbTopLeft.Size = new Size(0x3f, 0x4c);
            this.pbTopLeft.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbTopLeft, null);
            this.pbTopLeft.TabIndex = 0;
            this.pbTopLeft.TabStop = false;
            this.pbTopLeft.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbTopLeft.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbMinimize.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pbMinimize.Image = Resources.dialog_minimize;
            this.pbMinimize.Location = new Point(0x20c, 4);
            this.pbMinimize.Name = "pbMinimize";
            this.pbMinimize.Size = new Size(0x17, 0x16);
            this.pbMinimize.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbMinimize, null);
            this.pbMinimize.TabIndex = 6;
            this.pbMinimize.TabStop = false;
            this.pbMinimize.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbMinimize.Click += new EventHandler(this.pbMinimize_Click);
            this.pbMinimize.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pbClose.Image = Resources.dialog_close;
            this.pbClose.Location = new Point(0x23c, 4);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new Size(0x17, 0x16);
            this.pbClose.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbClose, null);
            this.pbClose.TabIndex = 5;
            this.pbClose.TabStop = false;
            this.pbClose.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbClose.Click += new EventHandler(this.pbClose_Click);
            this.pbClose.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbRestore.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pbRestore.Image = Resources.dialog_restore;
            this.pbRestore.Location = new Point(0x224, 4);
            this.pbRestore.Name = "pbRestore";
            this.pbRestore.Size = new Size(0x17, 0x16);
            this.pbRestore.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbRestore, null);
            this.pbRestore.TabIndex = 4;
            this.pbRestore.TabStop = false;
            this.pbRestore.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbRestore.Click += new EventHandler(this.pbRestore_Click);
            this.pbRestore.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pBottom.Controls.Add(this.pbBottomRight);
            this.pBottom.Controls.Add(this.pbBottom);
            this.pBottom.Controls.Add(this.pbBottomLeft);
            this.pBottom.Dock = DockStyle.Bottom;
            this.pBottom.Location = new Point(0, 0x1a7);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new Size(640, 0x39);
            base.ttDefault.SetSuperTip(this.pBottom, null);
            this.pBottom.TabIndex = 1;
            this.pBottom.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pBottom.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbBottomRight.Dock = DockStyle.Right;
            this.pbBottomRight.Image = Resources.dialog_bottomright2;
            this.pbBottomRight.Location = new Point(0x24d, 0);
            this.pbBottomRight.Name = "pbBottomRight";
            this.pbBottomRight.Size = new Size(0x33, 0x39);
            this.pbBottomRight.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbBottomRight, null);
            this.pbBottomRight.TabIndex = 2;
            this.pbBottomRight.TabStop = false;
            this.pbBottomRight.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbBottomRight.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbBottom.Dock = DockStyle.Fill;
            this.pbBottom.Image = Resources.dailog_bottom;
            this.pbBottom.Location = new Point(0x3b, 0);
            this.pbBottom.Name = "pbBottom";
            this.pbBottom.Size = new Size(0x245, 0x39);
            this.pbBottom.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbBottom, null);
            this.pbBottom.TabIndex = 1;
            this.pbBottom.TabStop = false;
            this.pbBottom.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbBottom.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbBottomLeft.Dock = DockStyle.Left;
            this.pbBottomLeft.Image = Resources.dialog_bottomleft;
            this.pbBottomLeft.Location = new Point(0, 0);
            this.pbBottomLeft.Name = "pbBottomLeft";
            this.pbBottomLeft.Size = new Size(0x3b, 0x39);
            this.pbBottomLeft.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.pbBottomLeft, null);
            this.pbBottomLeft.TabIndex = 0;
            this.pbBottomLeft.TabStop = false;
            this.pbBottomLeft.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbBottomLeft.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbLeft.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pbLeft.Image = (Image) manager.GetObject("pbLeft.Image");
            this.pbLeft.Location = new Point(0, 0x4b);
            this.pbLeft.Name = "pbLeft";
            this.pbLeft.Size = new Size(6, 0x192);
            this.pbLeft.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbLeft, null);
            this.pbLeft.TabIndex = 2;
            this.pbLeft.TabStop = false;
            this.pbLeft.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbLeft.MouseUp += new MouseEventHandler(this.OnMouseUp);
            this.pbRight.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pbRight.Image = (Image) manager.GetObject("pbRight.Image");
            this.pbRight.Location = new Point(0x27b, 0x4a);
            this.pbRight.Name = "pbRight";
            this.pbRight.Size = new Size(5, 0x18a);
            this.pbRight.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbRight, null);
            this.pbRight.TabIndex = 3;
            this.pbRight.TabStop = false;
            this.pbRight.MouseMove += new MouseEventHandler(this.OnMouseMove);
            this.pbRight.MouseUp += new MouseEventHandler(this.OnMouseUp);
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.Black;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.pbMinimize);
            base.Controls.Add(this.pBottom);
            base.Controls.Add(this.pbClose);
            base.Controls.Add(this.pbRestore);
            base.Controls.Add(this.pTop);
            base.Controls.Add(this.pbLeft);
            base.Controls.Add(this.pbRight);
            this.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.ForeColor = Color.White;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "DlgBase";
            base.Opacity = 1.0;
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "FrmBaseStyled";
            base.Load += new EventHandler(this.FrmBaseStyled_Load);
            this.pTop.ResumeLayout(false);
            this.pTop.PerformLayout();
            ((ISupportInitialize) this.pbTopRight).EndInit();
            ((ISupportInitialize) this.pbTopLeft).EndInit();
            ((ISupportInitialize) this.pbMinimize).EndInit();
            ((ISupportInitialize) this.pbClose).EndInit();
            ((ISupportInitialize) this.pbRestore).EndInit();
            this.pBottom.ResumeLayout(false);
            this.pBottom.PerformLayout();
            ((ISupportInitialize) this.pbBottomRight).EndInit();
            ((ISupportInitialize) this.pbBottom).EndInit();
            ((ISupportInitialize) this.pbBottomLeft).EndInit();
            ((ISupportInitialize) this.pbLeft).EndInit();
            ((ISupportInitialize) this.pbRight).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void item_Click(object sender, EventArgs e)
        {
            if (this.OnCloseButton())
            {
                base.Close();
            }
        }

        protected virtual void LoadLayoutData()
        {
            try
            {
                if (this.MainForm == null)
                {
                    return;
                }
                FieldInfo field = this.MainForm.GetType().GetField(this.SingletonName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(this.MainForm, this);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            if (base.Left == -32000)
            {
                base.Left = 0;
            }
            if (base.Top == -32000)
            {
                base.Top = 0;
            }
        }

        private void MaximizeClick(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Maximized;
        }

        private void Minimize(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
            this.SaveLayout();
        }

        private void MinimizeClick(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }

        protected override void OnActivated(EventArgs e)
        {
            try
            {
                if (((this.MainForm == null) || !this.MainForm.StayActive) && (this.HasShown || !this.ShowWithoutActivation))
                {
                    base.OnActivated(e);
                    this.StopFlashing();
                    if (this.MainForm != null)
                    {
                        this.MainForm.ActiveWindow = this;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected virtual bool OnCloseButton()
        {
            return true;
        }

        protected override void OnClosed(EventArgs e)
        {
            Program.Closing -= new EventHandler(this.Program_Closing);
            base.OnClosed(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.MainForm != null)
            {
                this.MainForm.ActiveDialogs.Remove(this);
            }
            this.SaveLayout();
            base.OnClosing(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            EventHandler handler = null;
            EventHandler handler2 = null;
            try
            {
                this.SetSkin(Program.Settings.SkinName);
                if (!this.AllowMultipleInstances && (this.MainForm != null))
                {
                    foreach (DlgBase base2 in this.MainForm.ActiveDialogs)
                    {
                        if ((!base2.Disposing && !base2.IsDisposed) && (base2.GetType() == base.GetType()))
                        {
                            EventLog.WriteLine("Closing non-unique new instance of singleton form type: {0}", new object[] { base.GetType() });
                            if (base2.WindowState == FormWindowState.Minimized)
                            {
                                base2.Restore(base2, EventArgs.Empty);
                            }
                            base.Close();
                            return;
                        }
                    }
                }
                if (base.DesignMode)
                {
                    base.Location = new Point(40, 40);
                }
                else
                {
                    if (((this.RememberLayout && Program.Settings.Appearance.Layout.RememberLayout) && Program.Settings.Appearance.Layout.FormLayouts.ContainsKey(this.SingletonName)) && (Program.Settings.Appearance.Layout.FormLayouts[this.SingletonName] != null))
                    {
                        if (this.RestoreWindowMode)
                        {
                            this.LoadLayoutData();
                        }
                        Program.Settings.Appearance.Layout.FormLayouts[this.SingletonName].Restore(this);
                    }
                    else if (((this.MainForm != null) && (this.MainForm.WindowState != FormWindowState.Minimized)) && this.MainForm.Visible)
                    {
                        base.Location = DrawUtil.CenterAbs(this.MainForm, this);
                        this.mStartCenterScreen = false;
                    }
                    else if (((base.ParentForm != null) && (base.ParentForm.WindowState != FormWindowState.Minimized)) && base.ParentForm.Visible)
                    {
                        base.Location = DrawUtil.CenterAbs(base.ParentForm, this);
                        this.mStartCenterScreen = false;
                    }
                    else
                    {
                        base.Location = DrawUtil.CenterScreen(this);
                        this.mStartCenterScreen = true;
                    }
                    if (this.MainForm != null)
                    {
                        if (handler == null)
                        {
                            handler = delegate (object s, EventArgs e1) {
                                this.mTileEffect = false;
                            };
                        }
                        this.MainForm.LocationChanged += handler;
                        if (handler2 == null)
                        {
                            handler2 = delegate (object s, EventArgs e1) {
                                this.mTileEffect = false;
                            };
                        }
                        this.MainForm.SizeChanged += handler2;
                        int num = 20;
                        int left = base.Left;
                        int num3 = 40;
                        int top = base.Top;
                        foreach (DlgBase base2 in this.MainForm.ActiveDialogs)
                        {
                            if (((!base2.Disposing && !base2.IsDisposed) && base2.TileEffect) && (base2.StartCenterScreen == this.StartCenterScreen))
                            {
                                left += num;
                                top += num3;
                                if (DrawUtil.GetScreenClipping(Screen.FromControl(this).Bounds.Size, new Rectangle(new Point(base.Left + left, base.Top + top), base.Size)).Height > 0)
                                {
                                    top = base.Top;
                                    left = base.Left + 80;
                                }
                            }
                        }
                        base.Location = new Point(left, top);
                        this.MainForm.ActiveDialogs.Add(this);
                    }
                    else
                    {
                        EventLog.WriteLine("Dialog of type {0} created without reference to main form.", new object[] { base.GetType() });
                    }
                }
                if (!(base.Disposing || base.IsDisposed))
                {
                    base.OnLoad(e);
                }
                if (this.ShowWithoutActivation)
                {
                    this.MainForm.StayActive = true;
                    if (this.MainForm != null)
                    {
                        if (this.MainForm.ActiveWindow == null)
                        {
                            this.MainForm.BringToFront();
                        }
                        else
                        {
                            this.MainForm.ActiveWindow.BringToFront();
                        }
                    }
                    this.MainForm.StayActive = false;
                }
                base.LocationChanged += new EventHandler(this.SaveLayout);
                base.SizeChanged += new EventHandler(this.SaveLayout);
                Program.Closing += new EventHandler(this.Program_Closing);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            this.mTileEffect = false;
            base.OnLocationChanged(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.mIsMaximized)
            {
                if (this.Cursor != Cursors.Default)
                {
                    this.Cursor = Cursors.Default;
                }
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                Point point = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                if (this.mIsResizing || this.mStartResizing)
                {
                    this.Cursor = Cursors.SizeAll;
                }
                else if ((this.IsResizable && (e.Button == MouseButtons.None)) && this.HitTest(point.X, point.Y, base.Width - this.RESIZE_RECT, base.Height - (this.RESIZE_RECT + 20), base.Width, base.Height))
                {
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if ((this.Cursor == Cursors.SizeNWSE) || (this.Cursor == Cursors.SizeAll))
                {
                    this.Cursor = Cursors.Default;
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (this.mIsMoving)
                    {
                        base.Left = Control.MousePosition.X - this.mLastX;
                        base.Top = Control.MousePosition.Y - this.mLastY;
                    }
                    else if (this.mIsResizing || this.mStartResizing)
                    {
                        if (this.mIsResizing)
                        {
                            ControlPaint.DrawReversibleFrame(this.mResizeRect, Color.Empty, FrameStyle.Thick);
                        }
                        this.mIsResizing = true;
                        this.mStartResizing = false;
                        this.mResizeRect.X = base.PointToScreen(new Point(0, 0)).X;
                        this.mResizeRect.Y = base.PointToScreen(new Point(0, 0)).Y;
                        this.mResizeRect.Width = this.mLastWidth + (Control.MousePosition.X - this.mLastMouseX);
                        this.mResizeRect.Height = this.mLastHeight + (Control.MousePosition.Y - this.mLastMouseY);
                        if (this.mResizeRect.Width < this.MinimumSize.Width)
                        {
                            this.mResizeRect.Width = this.MinimumSize.Width;
                        }
                        if (this.mResizeRect.Height < this.MinimumSize.Height)
                        {
                            this.mResizeRect.Height = this.MinimumSize.Height;
                        }
                        if ((this.MaximumSize.Width > 0) && (this.mResizeRect.Size.Width > this.MaximumSize.Width))
                        {
                            this.mResizeRect.Width = this.MaximumSize.Width;
                        }
                        if ((this.MaximumSize.Height > 0) && (this.mResizeRect.Size.Height > this.MaximumSize.Height))
                        {
                            this.mResizeRect.Height = this.MaximumSize.Height;
                        }
                        ControlPaint.DrawReversibleFrame(this.mResizeRect, Color.Empty, FrameStyle.Thick);
                    }
                }
                else
                {
                    this.SizeToFit(point);
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!(base.Disposing || base.IsDisposed))
            {
                Point point = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                this.SizeToFit(point);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.mIsResizing)
            {
                base.OnPaint(e);
                Graphics graphics = e.Graphics;
                Pen pen = new Pen(Color.FromArgb(0x19, 0xff, 0xff, 0xff));
                for (int i = 0; i < base.Width; i += 0x1b)
                {
                    graphics.DrawLine(pen, i, 0, i, base.Height);
                }
                for (int j = 0; j < base.Height; j += 0x1b)
                {
                    graphics.DrawLine(pen, 0, j, base.Width, j);
                }
                pen.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.SetRegion();
        }

        protected override void OnShown(EventArgs e)
        {
            this.HasShown = true;
            base.OnShown(e);
            if (!this.ShowWithoutActivation)
            {
                base.Activate();
            }
            else
            {
                this.FlashWindow();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            try
            {
                if (((this.pbRight != null) && (this.pbTopRight != null)) && (this.pbRight != null))
                {
                    this.pbRight.Top = this.pbTopRight.Bottom;
                    this.pbRight.Left = base.Width - this.pbRight.Width;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.pbLeft.Height = base.Height - this.pbLeft.Top;
            this.pbRight.Height = base.Height - this.pbRight.Top;
            this.pbRight.Left = base.Width - this.pbRight.Width;
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void pbMinimize_Click(object sender, EventArgs e)
        {
            this.Minimize(sender, e);
        }

        private void pbPaint(object sender, PaintEventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox box = sender as PictureBox;
                Graphics graphics = e.Graphics;
                for (int i = 0; i < box.Width; i += box.Image.Width)
                {
                    Rectangle rect = new Rectangle(i, 0, box.Image.Width, box.Image.Height);
                    graphics.DrawImage(box.Image, rect);
                }
            }
            else if ((sender is SkinStatusStrip) && (sender == this.pbTop))
            {
                Brush brush = new SolidBrush(this.ForeColor);
                Brush brush2 = new SolidBrush(this.BackColor);
                e.Graphics.DrawString(this.Text, Program.Settings.StylePreferences.MenuFont, brush, (float) 1f, (float) 11f);
                e.Graphics.DrawString(this.Text, Program.Settings.StylePreferences.MenuFont, brush2, (float) 0f, (float) 10f);
                brush2.Dispose();
                brush.Dispose();
            }
        }

        private void pbRestore_Click(object sender, EventArgs e)
        {
            this.Restore(sender, e);
        }

        private void pbTop_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Program_Closing(object sender, EventArgs e)
        {
            this.SaveLayoutData();
            this.SaveLayout();
        }

        protected virtual void ReSkin(object sender, EventArgs e)
        {
            if (!base.DesignMode)
            {
                if (this.IsResizable)
                {
                    this.pbRestore.Visible = base.MaximizeBox;
                    this.pbMinimize.Visible = base.MinimizeBox;
                    if (this.BottomMenuStrip)
                    {
                        this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottomright.png");
                    }
                    else
                    {
                        this.pbBottomRight.Image = base.GetImage(this.dirname + "bottomright.png");
                    }
                }
                else
                {
                    this.pbRestore.Visible = false;
                    if (this.BottomMenuStrip)
                    {
                        this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottomright_fixed.png");
                    }
                    else
                    {
                        this.pbBottomRight.Image = base.GetImage(this.dirname + "bottomright_fixed.png");
                    }
                }
                this.pbMinimize.Image = base.GetImage(this.dirname + "minimize.png");
                this.pbMinimize.Top = Program.Settings.StylePreferences.NavTop;
                if (this.pbRestore.Visible)
                {
                    this.pbMinimize.Left = base.ClientRectangle.Width - Program.Settings.StylePreferences.NavInsetFromRight;
                }
                else
                {
                    this.pbMinimize.Left = ((base.ClientRectangle.Width - Program.Settings.StylePreferences.NavInsetFromRight) + this.pbMinimize.Width) + 1;
                }
            }
        }

        private void Restore(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Normal;
                if (this.mIsMaximized)
                {
                    base.Bounds = Screen.GetWorkingArea(this);
                }
            }
            else if (this.mIsMaximized)
            {
                this.pbRestore.Image = SkinManager.GetImage("maximize.png");
                if (this.BottomMenuStrip)
                {
                    this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottomright.png");
                }
                else
                {
                    this.pbBottomRight.Image = SkinManager.GetImage(@"dialog\bottomright.png");
                }
                this.mIsMaximized = false;
                if (base.Bounds == this.mRestore)
                {
                    base.Width = (int) (((double) this.mRestore.Width) / 1.25);
                    base.Height = (int) (((double) this.mRestore.Height) / 1.25);
                }
                else
                {
                    base.Bounds = this.mRestore;
                }
            }
            else if (base.MaximizeBox && this.IsResizable)
            {
                this.pbRestore.Image = SkinManager.GetImage("restore.png");
                if (this.BottomMenuStrip)
                {
                    this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottomright_fixed.png");
                }
                else
                {
                    this.pbBottomRight.Image = SkinManager.GetImage(@"dialog\bottomright_fixed.png");
                }
                this.mRestore = base.Bounds;
                this.mIsMaximized = true;
                base.Bounds = Screen.GetWorkingArea(this);
            }
            this.SaveLayout();
        }

        private void RestoreClick(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Normal;
        }

        private void SaveLayout()
        {
            try
            {
                if ((Program.IsClosing || ((!base.Disposing && !base.IsDisposed) && (base.WindowState != FormWindowState.Minimized))) && (this.RememberLayout && Program.Settings.Appearance.Layout.RememberLayout))
                {
                    Program.Settings.Appearance.Layout.FormLayouts[this.SingletonName] = new FormLayout(this);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void SaveLayout(object sender, EventArgs e)
        {
            this.SaveLayout();
        }

        protected virtual void SaveLayoutData()
        {
        }

        private void SetRegion()
        {
            Rectangle clientRectangle = base.ClientRectangle;
            Rectangle rectangle2 = new Rectangle(base.PointToScreen(new Point(0, 0)).X, base.PointToScreen(new Point(0, 0)).Y, base.Width, base.Height);
            this.mWidthDiff = rectangle2.X - base.Bounds.X;
            this.mHeightDiff = rectangle2.Y - base.Bounds.Y;
            int hRgn = CreateRoundRectRgn(clientRectangle.Left, clientRectangle.Top, clientRectangle.Right + 1, clientRectangle.Bottom, Program.Settings.StylePreferences.FormBorderCurve, Program.Settings.StylePreferences.FormBorderCurve);
            SetWindowRgn(base.Handle.ToInt32(), hRgn, 1);
        }

        public virtual void SetSkin(string name)
        {
            EventHandler handler = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            EventHandler handler4 = null;
            EventHandler handler5 = null;
            EventHandler handler6 = null;
            this.Font = Program.Settings.Appearance.Text.MasterFont;
            this.dirname = Application.StartupPath + @"\skins\" + name + @"\dialog\";
            if (Directory.Exists(this.dirname))
            {
                this.pbRestore.Visible = base.MaximizeBox;
                this.pbMinimize.Visible = base.MinimizeBox;
                this.pbTopLeft.Image = base.GetImage(this.dirname + "topleft.png");
                this.pTop.Height = this.pbTopLeft.Image.Height;
                this.pbTopRight.Image = base.GetImage(this.dirname + "topright.png");
                this.pbLeft.Image = base.GetImage(this.dirname + "left.png");
                this.pbLeft.Width = this.pbLeft.Image.Width;
                this.pbLeft.Top = this.pTop.Bottom;
                this.pbLeft.Height = base.Height - this.pbTop.Bottom;
                this.pbRight.Image = base.GetImage(this.dirname + "right.png");
                this.pbRight.Width = this.pbRight.Image.Width;
                this.pbRight.Top = this.pbTopRight.Bottom;
                this.pbRight.Left = base.Width - this.pbRight.Width;
                this.pbRight.Height = base.Height - this.pbTop.Bottom;
                if (this.BottomMenuStrip)
                {
                    this.pbBottomLeft.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottomleft.png");
                }
                else
                {
                    this.pbBottomLeft.Image = SkinManager.GetImage(@"Dialog\bottomleft.png");
                }
                if (this.BottomMenuStrip)
                {
                    if (base.DesignMode || this.IsResizable)
                    {
                        this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottomright.png");
                    }
                    else
                    {
                        this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottomright_fixed.png");
                    }
                }
                else if (base.DesignMode || this.IsResizable)
                {
                    this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\bottomright.png");
                }
                else
                {
                    this.pbBottomRight.Image = SkinManager.GetImage(@"Dialog\bottomright_fixed.png");
                }
                if (this.BottomMenuStrip)
                {
                    this.pbBottom.Image = SkinManager.GetImage(@"Dialog\ButtonStrip\bottom.png");
                }
                else
                {
                    this.pbBottom.Image = SkinManager.GetImage(@"Dialog\bottom.png");
                }
                this.pBottom.Height = this.pbBottom.Image.Height;
                this.pbBottom.Paint += new PaintEventHandler(this.pbPaint);
                this.pbTop.Paint += new PaintEventHandler(this.pbPaint);
                this.pbMinimize.Image = base.GetImage(this.dirname + "minimize.png");
                this.pbMinimize.Top = Program.Settings.StylePreferences.NavTop;
                if (this.pbRestore.Visible)
                {
                    this.pbMinimize.Left = base.ClientRectangle.Width - Program.Settings.StylePreferences.NavInsetFromRight;
                }
                else
                {
                    this.pbMinimize.Left = ((base.ClientRectangle.Width - Program.Settings.StylePreferences.NavInsetFromRight) + this.pbMinimize.Width) + 1;
                }
                if (this.mIsMaximized)
                {
                    this.pbRestore.Image = SkinManager.GetImage("restore.png");
                }
                else
                {
                    this.pbRestore.Image = SkinManager.GetImage("maximize.png");
                }
                this.pbRestore.Top = this.pbMinimize.Top;
                this.pbRestore.Left = this.pbMinimize.Right + 1;
                this.pbClose.Image = base.GetImage(this.dirname + "close.png");
                this.pbClose.Top = this.pbRestore.Top;
                this.pbClose.Left = this.pbRestore.Right + 1;
                this.pbTop.Font = Program.Settings.Appearance.Text.StatusFont;
                this.pbTop.ForeColor = Program.Settings.Appearance.Text.StatusColor;
                this.pbTop.TextAlign = ContentAlignment.BottomLeft;
                this.pbTop.TextPadding = new Padding(0, 0, 0, 10);
                if (handler == null)
                {
                    handler = delegate (object s, EventArgs e) {
                        this.pbClose.Image = SkinManager.GetImage("close_over.png");
                    };
                }
                this.pbClose.MouseEnter += handler;
                if (handler2 == null)
                {
                    handler2 = delegate (object s, EventArgs e) {
                        this.pbMinimize.Image = SkinManager.GetImage("minimize_over.png");
                    };
                }
                this.pbMinimize.MouseEnter += handler2;
                if (handler3 == null)
                {
                    handler3 = delegate (object s, EventArgs e) {
                        if (this.mIsMaximized)
                        {
                            this.pbRestore.Image = SkinManager.GetImage("restore_over.png");
                        }
                        else
                        {
                            this.pbRestore.Image = SkinManager.GetImage("maximize_over.png");
                        }
                    };
                }
                this.pbRestore.MouseEnter += handler3;
                if (handler4 == null)
                {
                    handler4 = delegate (object s, EventArgs e) {
                        this.pbClose.Image = SkinManager.GetImage("close.png");
                    };
                }
                this.pbClose.MouseLeave += handler4;
                if (handler5 == null)
                {
                    handler5 = delegate (object s, EventArgs e) {
                        this.pbMinimize.Image = SkinManager.GetImage("minimize.png");
                    };
                }
                this.pbMinimize.MouseLeave += handler5;
                if (handler6 == null)
                {
                    handler6 = delegate (object s, EventArgs e) {
                        if (this.mIsMaximized)
                        {
                            this.pbRestore.Image = SkinManager.GetImage("restore.png");
                        }
                        else
                        {
                            this.pbRestore.Image = SkinManager.GetImage("maximize.png");
                        }
                    };
                }
                this.pbRestore.MouseLeave += handler6;
            }
        }

        public void SetStatus(string status, params object[] args)
        {
            this.SetStatus(status, 0, args);
        }

        public void SetStatus(string status, int timeout, params object[] args)
        {
            VGen0 method = null;
            ElapsedEventHandler handler = null;
            try
            {
                if (!base.Disposing && !base.IsDisposed)
                {
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.pbTop.Text = string.Format(Loc.Get(status), args);
                                if (this.StatusChanged != null)
                                {
                                    this.StatusChanged(this, this.pbTop.Text);
                                }
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else
                    {
                        this.pbTop.Text = string.Format(Loc.Get(status), args);
                        if (this.StatusChanged != null)
                        {
                            this.StatusChanged(this, this.pbTop.Text);
                        }
                    }
                    if (timeout > 0)
                    {
                        this.StatusTimer = new StateTimer((double) timeout);
                        this.StatusTimer.AutoReset = false;
                        if (handler == null)
                        {
                            handler = delegate (object sender, ElapsedEventArgs e) {
                                this.SetStatus("", new object[0]);
                            };
                        }
                        this.StatusTimer.Elapsed += handler;
                        this.StatusTimer.Start();
                    }
                    else
                    {
                        this.StatusTimer = null;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        [DllImport("user32.DLL")]
        private static extern int SetWindowRgn(int hWnd, int hRgn, int bRedraw);
        private void SizeToFit(Point point)
        {
            if (this.mIsResizing)
            {
                this.mIsResizing = false;
                ControlPaint.DrawReversibleFrame(this.mResizeRect, Color.Empty, FrameStyle.Thick);
                base.Width = this.mResizeRect.Width;
                base.Height = this.mResizeRect.Height;
            }
            this.mIsMoving = false;
            this.mLastX = point.X;
            this.mLastY = point.Y;
            this.mLastMouseX = Control.MousePosition.X;
            this.mLastMouseY = Control.MousePosition.Y;
            this.mLastWidth = base.Width;
            this.mLastHeight = base.Height;
        }

        public bool StopFlashing()
        {
            FLASHWINFO structure = new FLASHWINFO();
            structure.cbSize = (ushort) Marshal.SizeOf(structure);
            structure.hwnd = base.Handle;
            structure.dwFlags = 0;
            structure.uCount = 0xffff;
            structure.dwTimeout = 0;
            return (FlashWindowEx(ref structure) == 0);
        }

        protected virtual void StyleApplication(object sender, PropertyChangedEventArgs e)
        {
            if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    Program.Settings.StylePreferences.StyleChildControl(this);
                    this.pbTop.ForeColor = Program.Settings.Appearance.Text.StatusColor;
                    this.pbTop.Font = Program.Settings.Appearance.Text.StatusFont;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public void StyleMenu(MenuStrip menu)
        {
            if (Directory.Exists(this.dirname))
            {
                menu.BackgroundImage = base.GetImage(this.dirname + "menubg.png");
                menu.Left = Program.Settings.StylePreferences.DialogMenuLeft;
                menu.Top = Program.Settings.StylePreferences.DialogMenuTop;
            }
        }

        protected void this_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.mIsMaximized && (e.Button == MouseButtons.Left))
            {
                Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                if (this.HitTest(pt.X, pt.Y, 0, 0, base.Width, 50))
                {
                    if (!((this.pbClose.ClientRectangle.Contains(pt) || this.pbMinimize.ClientRectangle.Contains(pt)) || this.pbRestore.ClientRectangle.Contains(pt)))
                    {
                        this.mLastX = pt.X;
                        this.mLastY = pt.Y;
                        this.mIsMoving = true;
                    }
                }
                else if (this.IsResizable && this.HitTest(pt.X, pt.Y, base.Width - this.RESIZE_RECT, base.Height - (this.RESIZE_RECT + 20), base.Width, base.Height))
                {
                    this.mStartResizing = true;
                }
            }
        }

        protected void this_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.mIsMaximized && (e.Button == MouseButtons.Left))
            {
                Point pt = base.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                if (this.HitTest(pt.X, pt.Y, 0, 0, base.Width, 50) && !((this.pbClose.ClientRectangle.Contains(pt) || this.pbMinimize.ClientRectangle.Contains(pt)) || this.pbRestore.ClientRectangle.Contains(pt)))
                {
                    this.mIsMoving = false;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == 0x313)
                {
                    base.WndProc(ref m);
                    this.mTestLog = true;
                    GPGContextMenu menu = new GPGContextMenu();
                    MenuItem item = new MenuItem("<LOC>Restore");
                    item.Click += new EventHandler(this.Restore);
                    item.Enabled = this.mIsMaximized || (base.WindowState == FormWindowState.Minimized);
                    menu.MenuItems.Add(item);
                    item = new MenuItem("<LOC>Maximize");
                    item.Click += new EventHandler(this.Restore);
                    item.Enabled = (this.pbRestore.Visible && !this.mIsMaximized) && (base.WindowState == FormWindowState.Normal);
                    menu.MenuItems.Add(item);
                    item = new MenuItem("<LOC>Minimize");
                    item.Click += new EventHandler(this.Minimize);
                    item.Enabled = base.WindowState != FormWindowState.Minimized;
                    menu.MenuItems.Add(item);
                    menu.MenuItems.Add("-");
                    item = new MenuItem("<LOC>Close");
                    item.Click += new EventHandler(this.item_Click);
                    menu.MenuItems.Add(item);
                    menu.Localize();
                    menu.Show(this, new Point(Control.MousePosition.X - base.Left, Control.MousePosition.Y - base.Top));
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            if (this.mTestLog)
            {
            }
        }

        public virtual bool AllowMultipleInstances
        {
            get
            {
                return false;
            }
        }

        public virtual bool AllowRestoreWindow
        {
            get
            {
                return this.RememberLayout;
            }
        }

        public Rectangle AvailableClient
        {
            get
            {
                return new Rectangle(this.pbLeft.Right, this.pbTop.Bottom, this.pbRight.Left - this.pbLeft.Right, this.pBottom.Top - this.pbTop.Bottom);
            }
        }

        protected virtual bool BottomMenuStrip
        {
            get
            {
                return false;
            }
        }

        protected ErrorProvider Errors
        {
            get
            {
                return this.mErrors;
            }
        }

        public bool IsResizable
        {
            get
            {
                if (((this.MinimumSize != new Size(0, 0)) && (this.MaximumSize != new Size(0, 0))) && (this.MinimumSize == this.MaximumSize))
                {
                    return false;
                }
                return true;
            }
        }

        internal object LayoutData
        {
            get
            {
                return this.mLayoutData;
            }
            set
            {
                this.mLayoutData = value;
            }
        }

        public FrmMain MainForm
        {
            get
            {
                return this.mMainForm;
            }
            set
            {
                this.mMainForm = value;
            }
        }

        public override Size MinimumSize
        {
            get
            {
                return base.MinimumSize;
            }
            set
            {
                EventHandler handler = null;
                if (!(!this.HasShown && this.ShowWithoutActivation))
                {
                    base.MinimumSize = value;
                }
                else
                {
                    this.mMinimumSize = value;
                    if (handler == null)
                    {
                        handler = delegate (object s, EventArgs e) {
                            Size size = base.RestoreBounds.Size;
                            this.MinimumSize = this.mMinimumSize;
                            base.Size = size;
                        };
                    }
                    base.Activated += handler;
                }
            }
        }

        protected virtual bool RememberLayout
        {
            get
            {
                return !this.AllowMultipleInstances;
            }
        }

        internal bool RestoreWindowMode
        {
            get
            {
                return this.mRestoreWindowMode;
            }
            set
            {
                this.mRestoreWindowMode = value;
            }
        }

        public virtual string SingletonName
        {
            get
            {
                return base.GetType().Name;
            }
        }

        public bool StartCenterScreen
        {
            get
            {
                return this.mStartCenterScreen;
            }
        }

        public bool TileEffect
        {
            get
            {
                return this.mTileEffect;
            }
        }
    }
}

