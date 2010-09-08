namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.Multiplayer.Client;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class SkinLabel : SkinControl
    {
        private IContainer components;

        public SkinLabel() : base(@"Controls\Background Label\Rectangle")
        {
            this.components = null;
            this.InitializeComponent();
            base.Height = 20;
            base.Width = 0x7d;
            this.Font = new Font(Program.Settings.Appearance.Text.MasterFont.FontFamily, 10f, FontStyle.Bold);
            this.LoadSkin();
            this.Refresh();
        }

        public SkinLabel(string skinBasePath) : base(skinBasePath)
        {
            this.components = null;
            this.InitializeComponent();
            this.SkinBasePath = skinBasePath;
            base.Height = 20;
            base.Width = 0x7d;
            this.Font = new Font(Program.Settings.Appearance.Text.MasterFont.FontFamily, 10f, FontStyle.Bold);
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

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            EventHandler handler = null;
            base.OnParentChanged(e);
            if (base.Parent != null)
            {
                if (handler == null)
                {
                    handler = delegate (object s, EventArgs e1) {
                        VGen0 method = null;
                        if (base.InvokeRequired)
                        {
                            if (!base.Disposing && !base.IsDisposed)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.Refresh();
                                    };
                                }
                                base.BeginInvoke(method);
                            }
                        }
                        else
                        {
                            this.Refresh();
                        }
                    };
                }
                base.Parent.SizeChanged += handler;
            }
        }

        public override bool AutoStyle
        {
            get
            {
                return false;
            }
            set
            {
            }
        }
    }
}

