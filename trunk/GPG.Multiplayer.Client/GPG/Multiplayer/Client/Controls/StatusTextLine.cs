namespace GPG.Multiplayer.Client.Controls
{
    using GPG.Multiplayer.Client.Properties;
    using GPG.UI;
    using System;
    using System.Drawing;

    public class StatusTextLine : ChatLine
    {
        private Image mStatusIcon;

        public StatusTextLine(IFilterControl parent) : base(parent)
        {
            this.mStatusIcon = Resources.transparent;
        }

        public Image StatusIcon
        {
            get
            {
                return this.mStatusIcon;
            }
            set
            {
                if (value == null)
                {
                    this.mStatusIcon = Resources.transparent;
                }
                else
                {
                    this.mStatusIcon = value;
                }
            }
        }
    }
}

