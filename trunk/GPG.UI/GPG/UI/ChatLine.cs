namespace GPG.UI
{
    using System;
    using System.Collections.Generic;

    public class ChatLine : TextLine
    {
        private bool? mContainsEmotes;
        private bool? mContainsLinks;
        private ITextEffect mPlayerEffect;
        private List<TextSegment> mPlayerSegments;
        private DateTime mTimeStamp;

        public ChatLine(IFilterControl parent) : base(parent)
        {
            this.mContainsEmotes = null;
            this.mContainsLinks = null;
            this.mPlayerSegments = new List<TextSegment>();
            this.mTimeStamp = DateTime.Now;
        }

        public bool? ContainsEmotes
        {
            get
            {
                return this.mContainsEmotes;
            }
            set
            {
                this.mContainsEmotes = value;
            }
        }

        public bool? ContainsLinks
        {
            get
            {
                return this.mContainsLinks;
            }
            set
            {
                this.mContainsLinks = value;
            }
        }

        public ITextEffect PlayerEffect
        {
            get
            {
                return this.mPlayerEffect;
            }
            set
            {
                this.mPlayerEffect = value;
            }
        }

        public string PlayerInfo
        {
            get
            {
                string text = "";
                foreach (TextSegment segment in this.PlayerSegments)
                {
                    text = text + segment.Text;
                }
                if (this.mPlayerEffect != null)
                {
                    return this.mPlayerEffect.ChangeText(text);
                }
                return text;
            }
            set
            {
                this.PlayerSegments.Clear();
                this.PlayerSegments.Add(new TextSegment(this, value));
            }
        }

        public List<TextSegment> PlayerSegments
        {
            get
            {
                return this.mPlayerSegments;
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                string str = value;
                if (str.IndexOf("YELL") == 0)
                {
                    str = str.Replace("YELL", this.PlayerInfo + " ");
                    this.PlayerInfo = "";
                    base.Text = str;
                    foreach (TextSegment segment in base.TextSegments)
                    {
                        segment.Effect = new CallOutEffect();
                    }
                }
                else
                {
                    base.Text = str;
                }
            }
        }

        public string TimeStamp
        {
            get
            {
                return this.mTimeStamp.ToLongTimeString();
            }
        }
    }
}

