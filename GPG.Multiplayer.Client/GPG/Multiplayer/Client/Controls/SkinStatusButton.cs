namespace GPG.Multiplayer.Client.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class SkinStatusButton : SkinButton
    {
        private IContainer components;

        public SkinStatusButton() : base(@"Controls\Button\StatusCancel")
        {
            this.components = null;
            this.InitializeComponent();
            base.Height = 0x13;
            base.Width = 80;
            this.HorizontalScalingMode = ScalingModes.Tile;
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

