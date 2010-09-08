namespace GPG.UI.Controls
{
    using DevExpress.XtraBars;
    using System;
    using System.ComponentModel;

    public class GPGPopupMenu : PopupMenu
    {
        private IContainer components;

        public GPGPopupMenu()
        {
            this.InitializeComponent();
        }

        public GPGPopupMenu(IContainer container)
        {
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
    }
}

