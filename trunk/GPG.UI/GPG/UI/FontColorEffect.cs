namespace GPG.UI
{
    using System;
    using System.Drawing;

    public class FontColorEffect : ITextEffect
    {
        private Color mEffectColor;
        private Font mEffectFont;

        public FontColorEffect(Color color)
        {
            this.mEffectColor = Color.Empty;
            this.mEffectColor = color;
        }

        public FontColorEffect(Font font)
        {
            this.mEffectColor = Color.Empty;
            this.mEffectFont = font;
        }

        public FontColorEffect(Color color, Font font)
        {
            this.mEffectColor = Color.Empty;
            this.mEffectColor = color;
            this.mEffectFont = font;
        }

        public Color ChangeColor(Color color)
        {
            if (this.EffectColor != Color.Empty)
            {
                return this.EffectColor;
            }
            return color;
        }

        public Font ChangeFont(Font font)
        {
            if (this.EffectFont != null)
            {
                return this.EffectFont;
            }
            return font;
        }

        public string ChangeText(string text)
        {
            return text;
        }

        public Color EffectColor
        {
            get
            {
                return this.mEffectColor;
            }
        }

        public Font EffectFont
        {
            get
            {
                return this.mEffectFont;
            }
        }
    }
}

