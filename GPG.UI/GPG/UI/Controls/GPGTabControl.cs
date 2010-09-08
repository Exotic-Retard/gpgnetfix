namespace GPG.UI.Controls
{
    using DevExpress.UserSkins;
    using DevExpress.XtraTab;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGTabControl : XtraTabControl
    {
        private IContainer components;
        private bool mFirstTime;

        public static  event EventHandler OnStyleControl;

        public GPGTabControl()
        {
            this.mFirstTime = true;
            this.InitializeComponent();
        }

        public GPGTabControl(IContainer container)
        {
            this.mFirstTime = true;
            container.Add(this);
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
            this.components = new Container();
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
                else
                {
                    this.LookAndFeel.UseDefaultLookAndFeel = false;
                    BonusSkins.Register();
                    this.LookAndFeel.SkinName = "London Liquid Sky";
                }
            }
            base.OnPaint(e);
        }
    }
}

