namespace GPG.Multiplayer.UI.Controls
{
    using GPG;
    using GPG.Logging;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGLabel : Label, IWdnProcControl, IStyledControl
    {
        private GrowDirections mAutoGrowDirection;
        private bool mAutoStyle = true;
        private bool mFirstTime = true;
        private bool mIgnoreMouseWheel;
        private bool mIsStyled;
        private SizeF? mTextSize = null;
        private TextStyles mTextStyle;

        public static  event EventHandler OnStyleControl;

        public event VGen1 OnWndProc;

        public event PropertyChangedEventHandler TextStyleChanged;

        public GPGLabel()
        {
            this.Font = new Font("Arial", 9.75f);
            this.ForeColor = Color.White;
        }

        public void DoClick()
        {
            this.OnClick(EventArgs.Empty);
        }

        protected override void OnClick(EventArgs e)
        {
            if (base.Enabled || (this.TextStyle != TextStyles.Link))
            {
                base.OnClick(e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.AutoStyle && this.mFirstTime)
            {
                this.mFirstTime = false;
                if (OnStyleControl != null)
                {
                    OnStyleControl(this, EventArgs.Empty);
                    this.mIsStyled = true;
                }
            }
            base.OnPaint(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (!this.IgnoreMouseWheel || (m.Msg != 0x20a))
            {
                base.WndProc(ref m);
            }
            else if (this.OnWndProc != null)
            {
                this.OnWndProc((Message) m);
            }
        }

        [Browsable(true)]
        public GrowDirections AutoGrowDirection
        {
            get
            {
                return this.mAutoGrowDirection;
            }
            set
            {
                this.mAutoGrowDirection = value;
            }
        }

        [Browsable(true)]
        public bool AutoStyle
        {
            get
            {
                return this.mAutoStyle;
            }
            set
            {
                this.mAutoStyle = value;
                this.Refresh();
            }
        }

        public bool IgnoreMouseWheel
        {
            get
            {
                return this.mIgnoreMouseWheel;
            }
            set
            {
                this.mIgnoreMouseWheel = value;
            }
        }

        public bool IsStyled
        {
            get
            {
                return this.mIsStyled;
            }
            set
            {
                this.mIsStyled = value;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.mTextSize = null;
                try
                {
                    if (this.AutoGrowDirection == GrowDirections.Vertical)
                    {
                        using (Graphics graphics = base.CreateGraphics())
                        {
                            base.Height = (int) DrawUtil.MeasureString(graphics, this.Text, this.Font, (float) base.Width).Height;
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        [Browsable(true)]
        public int TextHeight
        {
            get
            {
                return Convert.ToInt32(Math.Round((double) this.TextSize.Height, MidpointRounding.AwayFromZero));
            }
        }

        private SizeF TextSize
        {
            get
            {
                VGen0 method = null;
                if (!this.mTextSize.HasValue)
                {
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                using (Graphics graphics = base.CreateGraphics())
                                {
                                    this.mTextSize = new SizeF?(DrawUtil.MeasureString(graphics, this.Text, this.Font));
                                }
                            };
                        }
                        base.Invoke(method);
                    }
                    else if (!base.Disposing && !base.IsDisposed)
                    {
                        using (Graphics graphics = base.CreateGraphics())
                        {
                            this.mTextSize = new SizeF?(DrawUtil.MeasureString(graphics, this.Text, this.Font));
                        }
                    }
                }
                return this.mTextSize.Value;
            }
        }

        public TextStyles TextStyle
        {
            get
            {
                return this.mTextStyle;
            }
            set
            {
                this.mTextStyle = value;
                if (this.TextStyleChanged != null)
                {
                    this.TextStyleChanged(this, new PropertyChangedEventArgs("TextStyle"));
                    this.Refresh();
                }
            }
        }

        [Browsable(true)]
        public int TextWidth
        {
            get
            {
                return Convert.ToInt32(Math.Round((double) this.TextSize.Width, MidpointRounding.AwayFromZero));
            }
        }
    }
}

