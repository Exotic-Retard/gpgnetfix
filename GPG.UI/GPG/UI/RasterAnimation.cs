namespace GPG.UI
{
    using GPG;
    using GPG.Logging;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class RasterAnimation : PictureBox
    {
        private bool CancelRun;
        private bool CanRun;
        private IContainer components;
        private RasterAnimationFrame CurrentFrame;
        private double Elapsed;
        private bool mAutoRun;
        private float mDuration;
        private RasterAnimationFrameCollection mFrames;
        private string mImagePath;
        private bool mIsRunning;
        private bool mLoop;
        private bool mPaused;

        public RasterAnimation()
        {
            this.mFrames = new RasterAnimationFrameCollection();
            this.mAutoRun = true;
            this.mLoop = true;
            this.mDuration = 1f;
            this.InitializeComponent();
            base.Size = new Size(0x20, 0x20);
            base.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public RasterAnimation(RasterAnimationFrame[] frames) : this()
        {
            RasterAnimationFrame frame = null;
            foreach (RasterAnimationFrame frame2 in frames)
            {
                if (frame != null)
                {
                    frame.NextFrame = frame2;
                    frame2.PreviousFrame = frame;
                }
                frame = frame2;
            }
            this.Frames.InnerList.AddRange(frames);
            base.Image = frames[0].Image;
        }

        public RasterAnimation(string imgPath) : this(imgPath, 1f)
        {
        }

        public RasterAnimation(string imgPath, float duration) : this()
        {
            this.ImagePath = imgPath;
            this.Duration = duration;
            base.Image = this.Frames[0].Image;
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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.CanRun = true;
        }

        public void Pause()
        {
            this.mPaused = !this.Paused;
        }

        private void Run()
        {
            if (this.Frames.Count >= 1)
            {
                ThreadPool.QueueUserWorkItem(delegate (object state) {
                    VGen0 method = null;
                    try
                    {
                        this.CancelRun = true;
                        while (this.IsRunning)
                        {
                            Thread.Sleep(20);
                        }
                        this.CancelRun = false;
                        this.mIsRunning = true;
                        if (this.Frames.Count >= 2)
                        {
                            this.CurrentFrame = this.Frames[1];
                            DateTime now = DateTime.Now;
                            this.Elapsed = 0.0;
                            while (this.IsRunning)
                            {
                                if (this.CancelRun)
                                {
                                    return;
                                }
                                if (!this.CanRun || this.Paused)
                                {
                                    Thread.Sleep(20);
                                    continue;
                                }
                                TimeSpan span = (TimeSpan) (DateTime.Now - now);
                                this.Elapsed += span.TotalSeconds;
                                if (this.Elapsed < this.CurrentFrame.Duration)
                                {
                                    goto Label_0150;
                                }
                                if (this.CurrentFrame.NextFrame == null)
                                {
                                    if (this.Loop)
                                    {
                                        this.CurrentFrame = this.Frames[1];
                                        goto Label_0107;
                                    }
                                    this.Stop();
                                    continue;
                                }
                                this.CurrentFrame = this.CurrentFrame.NextFrame;
                            Label_0107:
                                if (this.CancelRun || ((!base.IsHandleCreated || base.Disposing) || base.IsDisposed))
                                {
                                    return;
                                }
                                if (method == null)
                                {
                                    method = delegate {
                                        base.Image = this.CurrentFrame.Image;
                                    };
                                }
                                base.Invoke(method);
                                this.Elapsed = 0.0;
                            Label_0150:
                                now = DateTime.Now;
                                Thread.Sleep(10);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                    finally
                    {
                        this.mIsRunning = false;
                    }
                });
            }
        }

        public void Start()
        {
            this.mPaused = false;
            this.Run();
        }

        public void Stop()
        {
            this.mPaused = false;
            this.mIsRunning = false;
            base.Invoke((VGen0)delegate {
                if (this.Frames.Count >= 1)
                {
                    base.Image = this.Frames[0].Image;
                }
            });
        }

        public bool AutoRun
        {
            get
            {
                return this.mAutoRun;
            }
            set
            {
                this.mAutoRun = value;
            }
        }

        public float Duration
        {
            get
            {
                float num = 0f;
                foreach (RasterAnimationFrame frame in this.Frames)
                {
                    num += frame.Duration;
                }
                return num;
            }
            set
            {
                this.mDuration = value;
                foreach (RasterAnimationFrame frame in this.Frames)
                {
                    frame.Duration = value / ((float) this.Frames.Count);
                }
            }
        }

        public RasterAnimationFrameCollection Frames
        {
            get
            {
                return this.mFrames;
            }
        }

        public string ImagePath
        {
            get
            {
                return this.mImagePath;
            }
            set
            {
                this.mImagePath = value;
                if (this.IsRunning)
                {
                    this.CancelRun = true;
                    while (this.IsRunning)
                    {
                        Thread.Sleep(10);
                    }
                }
                float duration = this.Duration;
                this.Frames.Clear();
                if (this.ImagePath != null)
                {
                    if (!Path.IsPathRooted(this.ImagePath))
                    {
                        this.mImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.ImagePath);
                    }
                    if (Directory.Exists(this.ImagePath))
                    {
                        if (duration <= 0f)
                        {
                            duration = 1f;
                        }
                        RasterAnimationFrame frame = null;
                        foreach (string str in Directory.GetFiles(value, "*.png", SearchOption.TopDirectoryOnly))
                        {
                            RasterAnimationFrame frame2 = new RasterAnimationFrame(this, str);
                            if (frame != null)
                            {
                                frame.NextFrame = frame2;
                                frame2.PreviousFrame = frame;
                            }
                            this.Frames.Add(frame2);
                            frame = frame2;
                        }
                        this.Duration = duration;
                        if (this.AutoRun)
                        {
                            this.Start();
                        }
                    }
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.mIsRunning;
            }
        }

        public bool Loop
        {
            get
            {
                return this.mLoop;
            }
            set
            {
                this.mLoop = value;
            }
        }

        public bool Paused
        {
            get
            {
                return this.mPaused;
            }
        }
    }
}

