namespace GPG.UI.Controls
{
    using GPG;
    using GPG.Network;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PingIndicator : GPGPictureBox
    {
        private IContainer components;
        private IPingMonitor mPingMonitor;
        private ToolTip Tooltip = new ToolTip();

        public PingIndicator()
        {
            this.InitializeComponent();
            base.Image = GPGResources.ping;
            base.Size = base.Image.Size;
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
            if (base.DesignMode)
            {
                base.OnPaint(e);
            }
            else if (this.PingMonitor == null)
            {
                this.Tooltip.SetToolTip(this, "");
            }
            else
            {
                int num = this.PingMonitor.PingTime - this.PingMonitor.BestPing;
                if (num < 0)
                {
                    num = 0;
                }
                this.Tooltip.SetToolTip(this, Loc.Get(string.Format("<LOC>{0}ms", num)));
                float num2 = ((float) num) / ((float) this.PingMonitor.WorstPing);
                int width = (int) (base.Width * num2);
                e.Graphics.DrawImageUnscaledAndClipped(base.Image, new Rectangle(0, 0, width, base.Height));
            }
        }

        private void Pingmonitor_PingChanged(object sender, EventArgs e)
        {
            VGen0 method = null;
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
        }

        [Browsable(false)]
        public IPingMonitor PingMonitor
        {
            get
            {
                return this.mPingMonitor;
            }
            set
            {
                if (this.mPingMonitor != null)
                {
                    this.mPingMonitor.PingChanged -= new EventHandler(this.Pingmonitor_PingChanged);
                }
                this.mPingMonitor = value;
                if (value != null)
                {
                    this.mPingMonitor.PingChanged += new EventHandler(this.Pingmonitor_PingChanged);
                }
                else
                {
                    this.Refresh();
                }
            }
        }
    }
}

