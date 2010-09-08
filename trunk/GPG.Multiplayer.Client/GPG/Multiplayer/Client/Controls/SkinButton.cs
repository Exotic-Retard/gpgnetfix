namespace GPG.Multiplayer.Client.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SkinButton : SkinControl, IButtonControl
    {
        private IContainer components;
        private bool FirstMouseEnter;
        private int mButtonState;
        private System.Windows.Forms.DialogResult mDialogResult;
        private Color mDisabledForecolor;
        private Color mDrawColor;
        private Color mFocusColor;
        private bool mIsDefaultButton;
        private bool mMouseDown;
        private bool mMouseOver;

        public SkinButton() : base(@"Controls\Button\Round Edge")
        {
            this.components = null;
            this.mButtonState = 0;
            this.mMouseOver = false;
            this.mMouseDown = false;
            this.mDisabledForecolor = Color.Gray;
            this.FirstMouseEnter = true;
            this.mIsDefaultButton = false;
            this.mDialogResult = System.Windows.Forms.DialogResult.OK;
            this.InitializeComponent();
            this.Construct();
        }

        public SkinButton(string basePath) : base(basePath)
        {
            this.components = null;
            this.mButtonState = 0;
            this.mMouseOver = false;
            this.mMouseDown = false;
            this.mDisabledForecolor = Color.Gray;
            this.FirstMouseEnter = true;
            this.mIsDefaultButton = false;
            this.mDialogResult = System.Windows.Forms.DialogResult.OK;
            this.InitializeComponent();
            this.Construct();
        }

        private void Construct()
        {
            base.TabStop = true;
            this.DoubleBuffered = true;
            base.Height = 0x1a;
            base.Width = 0x7d;
            this.FocusColor = Color.Yellow;
            this.Font = new System.Drawing.Font(base.Font, FontStyle.Bold);
            base.TextAlign = ContentAlignment.MiddleCenter;
            this.LoadSkin();
            this.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DoClick()
        {
            this.OnClick(EventArgs.Empty);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void LoadSkin()
        {
            if (this.DrawDisabled)
            {
                this.ForeColor = this.DisabledForecolor;
                this.LoadState("disabled");
            }
            else if (this.mMouseDown)
            {
                this.ForeColor = this.FocusColor;
                this.LoadState("down");
            }
            else if (this.MouseOver)
            {
                this.ForeColor = this.FocusColor;
                this.LoadState("over");
            }
            else if (this.IsDefaultButton)
            {
                this.ForeColor = this.DrawColor;
                this.LoadState("active");
            }
            else
            {
                this.ForeColor = this.DrawColor;
                base.LoadState();
            }
        }

        public void NotifyDefault(bool value)
        {
            this.mIsDefaultButton = value;
            this.LoadSkin();
        }

        protected override void OnAutoStyled()
        {
            base.OnAutoStyled();
            this.DrawColor = this.ForeColor;
        }

        protected override void OnClick(EventArgs e)
        {
            base.Invoke(delegate {
                if (base.Enabled)
                {
                    base.Focus();
                }
            });
            base.OnClick(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            this.LoadSkin();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.mMouseDown = e.Button == MouseButtons.Left;
            this.LoadSkin();
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.FirstMouseEnter)
            {
                this.DrawColor = this.ForeColor;
                this.FirstMouseEnter = false;
            }
            this.mMouseOver = true;
            this.LoadSkin();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.mMouseOver = false;
            this.LoadSkin();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.mMouseDown = e.Button == MouseButtons.Left;
            this.LoadSkin();
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mMouseDown = e.Button != MouseButtons.Left;
            this.LoadSkin();
            base.OnMouseUp(e);
        }

        public void PerformClick()
        {
            this.OnClick(EventArgs.Empty);
        }

        public int ButtonState
        {
            get
            {
                return this.mButtonState;
            }
            set
            {
                this.mButtonState = value;
            }
        }

        public System.Windows.Forms.DialogResult DialogResult
        {
            get
            {
                return this.mDialogResult;
            }
            set
            {
                this.mDialogResult = value;
            }
        }

        public Color DisabledForecolor
        {
            get
            {
                return this.mDisabledForecolor;
            }
            set
            {
                this.mDisabledForecolor = value;
                this.LoadSkin();
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DrawColor
        {
            get
            {
                return this.mDrawColor;
            }
            set
            {
                this.mDrawColor = value;
                this.LoadSkin();
            }
        }

        protected virtual bool DrawDisabled
        {
            get
            {
                return !base.Enabled;
            }
        }

        protected virtual bool DrawFocused
        {
            get
            {
                return this.MouseOver;
            }
        }

        public Color FocusColor
        {
            get
            {
                return this.mFocusColor;
            }
            set
            {
                this.mFocusColor = value;
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = new System.Drawing.Font(value, FontStyle.Bold);
            }
        }

        public bool IsDefaultButton
        {
            get
            {
                return this.mIsDefaultButton;
            }
        }

        public virtual bool MouseOver
        {
            get
            {
                return this.mMouseOver;
            }
        }

        public override string SkinBasePath
        {
            get
            {
                return base.SkinBasePath;
            }
            set
            {
                base.SkinBasePath = value;
                this.LoadSkin();
            }
        }
    }
}

