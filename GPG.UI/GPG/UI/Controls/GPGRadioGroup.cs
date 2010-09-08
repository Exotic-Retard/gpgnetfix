namespace GPG.UI.Controls
{
    using DevExpress.XtraEditors;
    using System;
    using System.ComponentModel;

    public class GPGRadioGroup : RadioGroup
    {
        private IContainer components;

        public GPGRadioGroup()
        {
            this.InitializeComponent();
        }

        public GPGRadioGroup(IContainer container)
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

