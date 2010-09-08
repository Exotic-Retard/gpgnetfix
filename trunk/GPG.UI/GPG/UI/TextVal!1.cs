namespace GPG.UI
{
    using System;

    public class TextVal<valType> : ITextVal<valType>
    {
        private string mText;
        private valType mValue;

        public TextVal(string text, valType value)
        {
            this.mValue = default(valType);
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

        public valType Value
        {
            get
            {
                return this.mValue;
            }
        }
    }
}

