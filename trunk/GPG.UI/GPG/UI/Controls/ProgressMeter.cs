namespace GPG.UI.Controls
{
    using GPG;
    using GPG.Network;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ProgressMeter : Control
    {
        private IContainer components;
        private int LastRefresh = Environment.TickCount;
        private Color mBorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
        private float mBorderWidth = 2f;
        private Color mEmptyColor = Color.Gray;
        private Color mFillColor = Color.ForestGreen;
        private IProgressMonitor mMonitor;
        private int mProgress;
        private int mRefreshInterval = 500;

        public event ProgressChangeEventHandler ProgressChanged;

        public ProgressMeter()
        {
            this.InitializeComponent();
            base.Size = new Size(200, 0x11);
            this.DoubleBuffered = true;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (ProgressBarImages.left_bg != null)
            {
                e.Graphics.DrawImage(ProgressBarImages.left_bg, 0, 0, ProgressBarImages.left_bg.Width, base.Height);
            }
            using (TextureBrush brush = new TextureBrush(ProgressBarImages.mid_bg, WrapMode.Tile))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(ProgressBarImages.left_bg.Width, 0, base.Width - (ProgressBarImages.left_bg.Width + ProgressBarImages.right_bg.Width), base.Height));
            }
            if (ProgressBarImages.right_bg != null)
            {
                e.Graphics.DrawImage(ProgressBarImages.right_bg, base.Width - ProgressBarImages.right_bg.Width, 0, ProgressBarImages.right_bg.Width, base.Height);
            }
            Rectangle clientRectangle = base.ClientRectangle;
            clientRectangle.Width = (int) (clientRectangle.Width * (((float) this.Progress) / 100f));
            if (clientRectangle.Width > ProgressBarImages.left_progress.Width)
            {
                using (TextureBrush brush2 = new TextureBrush(ProgressBarImages.mid_progress, WrapMode.Tile))
                {
                    Rectangle rect = clientRectangle;
                    rect.X += ProgressBarImages.left_progress.Width;
                    if (clientRectangle.Width >= (base.Width - ProgressBarImages.right_progress.Width))
                    {
                        rect.Width = base.Width - (ProgressBarImages.left_progress.Width + ProgressBarImages.right_progress.Width);
                    }
                    else
                    {
                        rect.Width -= ProgressBarImages.left_progress.Width;
                    }
                    e.Graphics.FillRectangle(brush2, rect);
                }
            }
            if (clientRectangle.Width < ProgressBarImages.left_progress.Width)
            {
                e.Graphics.DrawImageUnscaledAndClipped(ProgressBarImages.left_progress, clientRectangle);
            }
            else
            {
                e.Graphics.DrawImageUnscaledAndClipped(ProgressBarImages.left_progress, new Rectangle(new Point(0, 0), ProgressBarImages.left_progress.Size));
            }
            if (clientRectangle.Width >= (base.Width - ProgressBarImages.right_progress.Width))
            {
                e.Graphics.DrawImageUnscaledAndClipped(ProgressBarImages.right_progress, new Rectangle(base.Width - ProgressBarImages.right_progress.Width, 0, clientRectangle.Width - (base.Width - ProgressBarImages.right_progress.Width), ProgressBarImages.mid_progress.Height));
            }
        }

        private void ProgressMonitor_Finished(object sender, EventArgs e)
        {
            this.LastRefresh = 0;
            this.Progress = 100;
        }

        public void ProgressMonitor_ProgressChanged(object sender, int progress)
        {
            this.Progress = progress;
        }

        [Browsable(true)]
        public Color BorderColor
        {
            get
            {
                return this.mBorderColor;
            }
            set
            {
                this.mBorderColor = value;
            }
        }

        [Browsable(true)]
        public float BorderWidth
        {
            get
            {
                return this.mBorderWidth;
            }
            set
            {
                this.mBorderWidth = value;
            }
        }

        [Browsable(true)]
        public Color EmptyColor
        {
            get
            {
                return this.mEmptyColor;
            }
            set
            {
                this.mEmptyColor = value;
            }
        }

        [Browsable(true)]
        public Color FillColor
        {
            get
            {
                return this.mFillColor;
            }
            set
            {
                this.mFillColor = value;
            }
        }

        public IProgressMonitor Monitor
        {
            get
            {
                return this.mMonitor;
            }
            set
            {
                if (this.mMonitor != null)
                {
                    this.mMonitor.ProgressChanged -= new ProgressChangeEventHandler(this.ProgressMonitor_ProgressChanged);
                    this.mMonitor.Finished -= new EventHandler(this.ProgressMonitor_Finished);
                }
                this.mMonitor = value;
                if (value != null)
                {
                    this.mMonitor.ProgressChanged += new ProgressChangeEventHandler(this.ProgressMonitor_ProgressChanged);
                    this.mMonitor.Finished += new EventHandler(this.ProgressMonitor_Finished);
                }
            }
        }

        [Browsable(false)]
        public int Progress
        {
            get
            {
                return this.mProgress;
            }
            set
            {
                VGen0 method = null;
                this.mProgress = value;
                if (this.ProgressChanged != null)
                {
                    this.ProgressChanged(this, this.Progress);
                }
                if (!base.DesignMode && ((this.LastRefresh == 0) || (Math.Abs((int) (Environment.TickCount - this.LastRefresh)) >= this.RefreshInterval)))
                {
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.Refresh();
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (!base.Disposing && !base.IsDisposed)
                    {
                        this.Refresh();
                    }
                    this.LastRefresh = Environment.TickCount;
                }
            }
        }

        public int RefreshInterval
        {
            get
            {
                return this.mRefreshInterval;
            }
            set
            {
                this.mRefreshInterval = value;
            }
        }
    }
}

