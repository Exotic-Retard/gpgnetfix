namespace GPG.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class GPGWebBrowser : WebBrowser
    {
        private IContainer components;

        public GPGWebBrowser()
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
    }
}

