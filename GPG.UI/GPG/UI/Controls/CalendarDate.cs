namespace GPG.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class CalendarDate
    {
        private DateTime mDate;
        private Color mHighlightColor = Color.Empty;
        private string mLabel = "";
        private List<object> mTag = new List<object>();
        private object mValue;

        public override string ToString()
        {
            return this.Label;
        }

        public DateTime Date
        {
            get
            {
                return this.mDate;
            }
            set
            {
                this.mDate = value;
            }
        }

        public Color HighlightColor
        {
            get
            {
                return this.mHighlightColor;
            }
            set
            {
                this.mHighlightColor = value;
            }
        }

        public string Label
        {
            get
            {
                if ((this.mLabel != null) && (this.mLabel.Trim() != ""))
                {
                    return this.mLabel;
                }
                if (this.mValue != null)
                {
                    return this.mValue.ToString();
                }
                return "";
            }
            set
            {
                this.mLabel = value;
            }
        }

        public List<object> Tag
        {
            get
            {
                return this.mTag;
            }
            set
            {
                this.mTag = value;
            }
        }

        public object Value
        {
            get
            {
                return this.mValue;
            }
            set
            {
                this.mValue = value;
            }
        }
    }
}

