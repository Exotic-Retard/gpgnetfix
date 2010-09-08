namespace GPG.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class GPGGroupBox : GroupBox
    {
        private IContainer components;

        public GPGGroupBox()
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

