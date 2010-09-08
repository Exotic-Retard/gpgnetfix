namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class FrmNotify : Form
    {
        private IContainer components = null;

        public FrmNotify()
        {
            this.InitializeComponent();
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
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x13b, 0x88);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "FrmNotify";
            this.Text = "FrmNotify";
            base.ResumeLayout(false);
        }

        public void Show()
        {
            throw new Exception("This operation is not supported.  You can only use ShowMessage() for anything deriving from FrmNotify.");
        }

        public DialogResult ShowDialog()
        {
            throw new Exception("This operation is not supported.  You can only use ShowMessage() for anything deriving from FrmNotify.");
        }

        public void ShowMessage()
        {
            Exception exception;
            VGen0 gen = null;
            VGen0 gen2 = null;
            try
            {
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (gen == null)
                    {
                        gen = delegate {
                            try
                            {
                                base.Opacity = 0.0;
                                IntPtr handle = base.Handle;
                                base.Show();
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    base.Invoke(gen);
                }
                else if (!base.Disposing && !base.IsDisposed)
                {
                    try
                    {
                        base.Opacity = 0.0;
                        IntPtr ptr = base.Handle;
                        base.Show();
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        ErrorLog.WriteLine(exception);
                        return;
                    }
                }
                if (gen2 == null)
                {
                    gen2 = delegate {
                        Exception exception;
                        VGen1 method = null;
                        VGen0 gen2 = null;
                        try
                        {
                            int tickCount = Environment.TickCount;
                            int num2 = tickCount;
                            int num3 = (int) (Program.Settings.PopupPreferences.Duration * 1000.0);
                            while ((!base.Disposing && !base.IsDisposed) && ((tickCount + num3) > Environment.TickCount))
                            {
                                int millisecondsTimeout = 10;
                                Thread.Sleep(millisecondsTimeout);
                                double num5 = ((double) (Environment.TickCount - tickCount)) / ((double) num3);
                                if (!base.Disposing && !base.IsDisposed)
                                {
                                    try
                                    {
                                        if (method == null)
                                        {
                                            method = delegate (object objopacity) {
                                                try
                                                {
                                                    double num = 1.0;
                                                    double num2 = (double) objopacity;
                                                    if (num2 < Program.Settings.PopupPreferences.FadeTimePercent)
                                                    {
                                                        num = num2 / Program.Settings.PopupPreferences.FadeTimePercent;
                                                    }
                                                    else if (num2 > (1.0 - Program.Settings.PopupPreferences.FadeTimePercent))
                                                    {
                                                        num = (1.0 - num2) / Program.Settings.PopupPreferences.FadeTimePercent;
                                                    }
                                                    base.Opacity = num;
                                                }
                                                catch (Exception exception)
                                                {
                                                    ErrorLog.WriteLine(exception);
                                                }
                                            };
                                        }
                                        base.Invoke(method, new object[] { num5 });
                                    }
                                    catch (Exception exception1)
                                    {
                                        exception = exception1;
                                        ErrorLog.WriteLine(exception);
                                        return;
                                    }
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            exception = exception2;
                            ErrorLog.WriteLine(exception);
                        }
                        finally
                        {
                            try
                            {
                                if (!base.Disposing && !base.IsDisposed)
                                {
                                    if (gen2 == null)
                                    {
                                        gen2 = delegate {
                                            try
                                            {
                                                base.Close();
                                            }
                                            catch (Exception exception)
                                            {
                                                ErrorLog.WriteLine(exception);
                                            }
                                        };
                                    }
                                    base.Invoke(gen2);
                                }
                            }
                            catch (Exception exception3)
                            {
                                exception = exception3;
                                ErrorLog.WriteLine(exception);
                            }
                        }
                    };
                }
                Thread thread = new Thread(new ThreadStart(gen2.Invoke));
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception exception3)
            {
                exception = exception3;
                ErrorLog.WriteLine(exception);
                try
                {
                    base.Close();
                }
                catch (Exception exception2)
                {
                    ErrorLog.WriteLine(exception2);
                }
            }
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }
    }
}

