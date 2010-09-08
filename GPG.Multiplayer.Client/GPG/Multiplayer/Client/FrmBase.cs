namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using GPG;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class FrmBase : Form
    {
        private IContainer components = null;
        private static int LastLeft = -99999;
        private static int LastTop = -99999;
        private bool mFirstTime = true;
        private bool mLoaded = false;
        private SelfTest mSelfTest = new SelfTest();
        protected DefaultToolTipController ttDefault;

        public event BaseFormEvent AfterLoad;

        public event BaseFormEvent AfterShown;

        public static  event EventHandler OnStyleControl;

        public FrmBase()
        {
            if ((FrmMain.ThreadID != 0) && (Thread.CurrentThread.ManagedThreadId != FrmMain.ThreadID))
            {
                throw new Exception("Form not created on the main thread.");
            }
            base.AutoScaleMode = AutoScaleMode.None;
            this.InitializeComponent();
            base.AutoScaleMode = AutoScaleMode.None;
            if (!base.DesignMode)
            {
            }
            base.Opacity = 1.0;
        }

        private void _Localize(Control root)
        {
            if ((root.Text != null) && (root.Text.Length > 0))
            {
                root.Text = Loc.Get(root.Text);
            }
            if (root is ILocalizable)
            {
                (root as ILocalizable).Localize();
            }
            string toolTip = this.ttDefault.GetToolTip(root);
            if ((toolTip != null) && (toolTip.Length > 0))
            {
                this.ttDefault.SetToolTip(root, Loc.Get(toolTip));
            }
            foreach (Control control in root.Controls)
            {
                if (control != null)
                {
                    this._Localize(control);
                }
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

        protected Image GetImage(string filename)
        {
            if (File.Exists(filename))
            {
                return Image.FromFile(filename);
            }
            string path = filename.Replace(Program.Settings.SkinName, "default");
            if (File.Exists(path))
            {
                return Image.FromFile(path);
            }
            return new Bitmap(10, 10);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmBase));
            this.ttDefault = new DefaultToolTipController(this.components);
            base.SuspendLayout();
            this.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x124, 0x10a);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "FrmBase";
            base.Opacity = 1.0;
            this.ttDefault.SetSuperTip(this, null);
            this.Text = "FrmBase";
            base.ResumeLayout(false);
        }

        protected virtual void Localize()
        {
            this._Localize(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Localize();
            if (this.AfterLoad != null)
            {
                this.AfterLoad(this);
            }
            base.Icon = base.Icon;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            LastLeft = base.Left;
            LastTop = base.Top;
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            if (base.Visible && this.mLoaded)
            {
                LastLeft = base.Left;
                LastTop = base.Top;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.mFirstTime)
            {
                this.mFirstTime = false;
                if (OnStyleControl != null)
                {
                    OnStyleControl(this, EventArgs.Empty);
                }
            }
            base.OnPaint(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.AfterShown != null)
            {
                this.AfterShown(this);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (base.Width == 0)
            {
                base.Width = 10;
            }
            if (base.Height == 0)
            {
                base.Height = 10;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!this.mLoaded)
            {
                if (LastLeft != -99999)
                {
                    base.Left = LastLeft;
                    base.Top = LastTop;
                }
                this.mLoaded = true;
            }
        }
    }
}

