namespace GPG.UI
{
    using System;

    public class TextVal : ITextVal
    {
        private string mText;
        private object mValue;

        public TextVal(string text, object value)
        {
            this.mText = text;
            this.mValue = value;
        }

        public override bool Equals(object obj)
        {
            return this.Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Text;
        }

        public string Text
        {
            get
            {
                return this.mText;
            }
        }

        public object Value
        {
            get
            {
                return this.mValue;
            }
        }
    }
}

