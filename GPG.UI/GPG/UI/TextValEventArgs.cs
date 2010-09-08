namespace GPG.UI
{
    using System;

    public class TextValEventArgs : EventArgs
    {
        private ITextVal mValue;

        public TextValEventArgs(ITextVal value)
        {
            this.mValue = value;
        }

        public ITextVal Value
        {
            get
            {
                return this.mValue;
            }
        }
    }
}

