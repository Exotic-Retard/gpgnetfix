namespace GPG.UI.Controls
{
    using System;
    using System.Drawing;

    public class PanelButton
    {
        private System.Drawing.Image mImage;
        private string mTooltip;

        public event EventHandler Click;

        public PanelButton(System.Drawing.Image img)
        {
            this.mImage = img;
        }

        public PanelButton(System.Drawing.Image img, string tooltip)
        {
            this.mImage = img;
            this.mTooltip = tooltip;
        }

        internal void OnClick()
        {
            if (this.mClick != null)
            {
                this.mClick(this, EventArgs.Empty);
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.mImage;
            }
            set
            {
                this.mImage = value;
            }
        }

        public string Tooltip
        {
            get
            {
                return this.mTooltip;
            }
            set
            {
                this.mTooltip = value;
            }
        }
    }
}

