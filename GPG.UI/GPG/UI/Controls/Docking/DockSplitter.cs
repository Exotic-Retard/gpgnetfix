namespace GPG.UI.Controls.Docking
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DockSplitter : SplitContainer
    {
        private IContainer components;
        private Control mContainerControl;

        public DockSplitter(Orientation orientation)
        {
            this.InitializeComponent();
            this.BackColor = Color.Gray;
            this.ForeColor = Color.Gray;
            base.SplitterWidth = 4;
            base.Orientation = orientation;
            base.Dock = DockStyle.Fill;
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

        public Control ContainerControl
        {
            get
            {
                return this.mContainerControl;
            }
            internal set
            {
                this.mContainerControl = value;
            }
        }
    }
}

