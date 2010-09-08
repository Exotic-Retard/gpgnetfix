namespace GPG.Multiplayer.Client.Controls.Awards
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Statistics;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgAwardSetDetails : Form
    {
        private bool CancelFade = false;
        private IContainer components = null;
        private double FadeIncrement = 0.0;
        private bool IsFading = false;
        private const double MaxFade = 1.0;
        private const double MinFade = 0.0;
        private static DlgAwardSetDetails mSingleton = new DlgAwardSetDetails();
        private double TargetFade = 0.0;

        public DlgAwardSetDetails()
        {
            this.InitializeComponent();
        }

        public void BindToAward(AwardSet awardSet, int degree)
        {
            base.SuspendLayout();
            PnlAwardSetDetails details = new PnlAwardSetDetails(awardSet, degree);
            base.Controls.Clear();
            base.Size = details.Size;
            base.Controls.Add(details);
            base.ResumeLayout(true);
            if (!base.Visible)
            {
                base.Show();
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

        public void FadeIn()
        {
            this.TargetFade = 1.0;
            this.FadeIncrement = 0.04;
            this.PerformFade();
        }

        public void FadeOut()
        {
            this.TargetFade = 0.0;
            this.FadeIncrement = -0.04;
            this.PerformFade();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.ClientSize = new Size(0x124, 0x10a);
            this.DoubleBuffered = true;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "DlgAwardSetDetails";
            base.Opacity = 0.0;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "<LOC>Award Details";
            base.TopMost = true;
            base.ResumeLayout(false);
        }

        private void PerformFade()
        {
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                VGen0 method = null;
                VGen0 gen2 = null;
                try
                {
                    this.CancelFade = true;
                    while (this.IsFading)
                    {
                        Thread.Sleep(20);
                    }
                    this.CancelFade = false;
                    this.IsFading = true;
                    if (!base.Visible)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                try
                                {
                                    base.Opacity = 0.0;
                                    base.Show();
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                    this.CancelFade = true;
                                }
                            };
                        }
                        base.Invoke(method);
                    }
                    while (base.Opacity != this.TargetFade)
                    {
                        if (this.CancelFade)
                        {
                            return;
                        }
                        if (gen2 == null)
                        {
                            gen2 = delegate {
                                try
                                {
                                    if (!this.CancelFade)
                                    {
                                        if ((base.Opacity + this.FadeIncrement) > 1.0)
                                        {
                                            base.Opacity = 1.0;
                                        }
                                        else if ((base.Opacity + this.FadeIncrement) < 0.0)
                                        {
                                            base.Opacity = 0.0;
                                        }
                                        else
                                        {
                                            base.Opacity += this.FadeIncrement;
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                    this.CancelFade = true;
                                }
                            };
                        }
                        base.Invoke(gen2);
                        Thread.Sleep(10);
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                finally
                {
                    this.IsFading = false;
                }
            });
        }

        public static DlgAwardSetDetails Singleton
        {
            get
            {
                try
                {
                    if (((mSingleton == null) || mSingleton.IsDisposed) || mSingleton.Disposing)
                    {
                        mSingleton = new DlgAwardSetDetails();
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                return mSingleton;
            }
            set
            {
                mSingleton = value;
            }
        }
    }
}

