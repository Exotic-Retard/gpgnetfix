namespace GPG.UI.Controls
{
    using GPG;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGContextMenu : ContextMenu, ILocalizable
    {
        private IContainer components;

        public static  event EventHandler OnStyleControl;

        public GPGContextMenu()
        {
            this.InitializeComponent();
            if (OnStyleControl != null)
            {
                OnStyleControl(this, EventArgs.Empty);
            }
        }

        private void _Localize(MenuItem root)
        {
            root.Text = Loc.Get(root.Text);
            foreach (MenuItem item in root.MenuItems)
            {
                this._Localize(item);
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

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public void Localize()
        {
            foreach (MenuItem item in base.MenuItems)
            {
                this._Localize(item);
            }
        }
    }
}

