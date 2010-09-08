namespace GPG.UI.Controls.Docking
{
    using GPG.UI.Controls.Skinning;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class DockSkinnedButton : SkinnablePanel
    {
        private IContainer components;

        public DockSkinnedButton()
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

        public override SkinManager Manager
        {
            get
            {
                return SkinManager.Docking;
            }
            set
            {
            }
        }
    }
}

