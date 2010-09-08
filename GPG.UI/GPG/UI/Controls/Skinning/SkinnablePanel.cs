namespace GPG.UI.Controls.Skinning
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class SkinnablePanel : PictureBox, ISkinResourceConsumer
    {
        private IContainer components;
        private bool mAutoRefreshSkin = true;
        [NonSerialized]
        private SkinManager mManager = SkinManager.Default;
        private string mResourceKey;

        public SkinnablePanel()
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
            this.components = new Container();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public virtual void RefreshSkin()
        {
            if (((this.Manager != null) && (this.ResourceKey != null)) && (this.ResourceKey.Length > 0))
            {
                base.Image = this.Manager.GetImage(this);
            }
        }

        [Browsable(true)]
        public virtual bool AutoRefreshSkin
        {
            get
            {
                return this.mAutoRefreshSkin;
            }
            set
            {
                this.mAutoRefreshSkin = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual SkinManager Manager
        {
            get
            {
                return this.mManager;
            }
            set
            {
                this.mManager = value;
                if (this.AutoRefreshSkin)
                {
                    this.RefreshSkin();
                }
            }
        }

        [Browsable(true)]
        public virtual string ResourceKey
        {
            get
            {
                return this.mResourceKey;
            }
            set
            {
                if ((value != null) && (value.Length > 0))
                {
                    this.mResourceKey = value;
                    if (this.AutoRefreshSkin)
                    {
                        this.RefreshSkin();
                    }
                }
                else
                {
                    this.mResourceKey = null;
                    base.Image = null;
                }
            }
        }
    }
}

