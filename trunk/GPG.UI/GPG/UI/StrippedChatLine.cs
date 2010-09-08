namespace GPG.UI
{
    using System;
    using System.Drawing;

    public class StrippedChatLine
    {
        private Image mIcon;
        private string mPlayerInfo;
        private string mText;

        public Image Icon
        {
            get
            {
                return this.mIcon;
            }
            set
            {
                this.mIcon = value;
            }
        }

        public string PlayerInfo
        {
            get
            {
                return this.mPlayerInfo;
            }
            set
            {
                this.mPlayerInfo = value;
            }
        }

        public string Text
        {
            get
            {
                return this.mText;
            }
            set
            {
                this.mText = value;
            }
        }
    }
}

