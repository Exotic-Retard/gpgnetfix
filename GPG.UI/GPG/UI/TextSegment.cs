namespace GPG.UI
{
    using System;
    using System.Drawing;

    public class TextSegment : IText
    {
        private ITextEffect mEffect;
        private bool mIsKeySegment;
        private IText mParent;
        private string mText;
        private Color mTextColor;
        private Font mTextFont;

        public TextSegment(string text)
        {
            this.mText = text;
        }

        internal TextSegment(IText parent, string text)
        {
            this.mParent = parent;
            this.mText = text;
        }

        public TextSegment(string text, bool isKey)
        {
            this.mText = text;
            this.mIsKeySegment = isKey;
        }

        internal TextSegment(IText parent, string text, bool isKey)
        {
            this.mParent = parent;
            this.mText = text;
            this.mIsKeySegment = isKey;
        }

        public TextSegment(string text, Color color, Font font)
        {
            this.mText = text;
            this.mTextColor = color;
            this.mTextFont = font;
        }

        internal TextSegment(IText parent, string text, Color color, Font font)
        {
            this.mParent = parent;
            this.mText = text;
            this.mTextColor = color;
            this.mTextFont = font;
        }

        public TextSegment(string text, Color color, Font font, bool isKey)
        {
            this.mText = text;
            this.mTextColor = color;
            this.mTextFont = font;
            this.mIsKeySegment = isKey;
        }

        internal TextSegment(IText parent, string text, Color color, Font font, bool isKey)
        {
            this.mParent = parent;
            this.mText = text;
            this.mTextColor = color;
            this.mTextFont = font;
            this.mIsKeySegment = isKey;
        }

        public ITextEffect Effect
        {
            get
            {
                if (this.mEffect != null)
                {
                    return this.mEffect;
                }
                return this.Parent.Effect;
            }
            set
            {
                this.mEffect = value;
            }
        }

        public bool IsKeySegment
        {
            get
            {
                return this.mIsKeySegment;
            }
            set
            {
                this.mIsKeySegment = value;
            }
        }

        public IText Parent
        {
            get
            {
                return this.mParent;
            }
            internal set
            {
                this.mParent = value;
            }
        }

        public string Text
        {
            get
            {
                if (this.Effect != null)
                {
                    return this.Effect.ChangeText(this.mText);
                }
                return this.mText;
            }
            set
            {
                this.mText = value;
            }
        }

        public Color TextColor
        {
            get
            {
                Color mTextColor;
                if (this.mTextColor != Color.Empty)
                {
                    mTextColor = this.mTextColor;
                }
                else
                {
                    mTextColor = this.Parent.TextColor;
                }
                if (this.mEffect != null)
                {
                    return this.mEffect.ChangeColor(mTextColor);
                }
                return mTextColor;
            }
            set
            {
                this.mTextColor = value;
            }
        }

        public Font TextFont
        {
            get
            {
                Font mTextFont;
                if (this.mTextFont != null)
                {
                    mTextFont = this.mTextFont;
                }
                else
                {
                    mTextFont = this.Parent.TextFont;
                }
                if (this.mEffect != null)
                {
                    return this.mEffect.ChangeFont(mTextFont);
                }
                return mTextFont;
            }
            set
            {
                this.mTextFont = value;
            }
        }
    }
}

