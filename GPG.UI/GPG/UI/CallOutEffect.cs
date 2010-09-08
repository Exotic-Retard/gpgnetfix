namespace GPG.UI
{
    using System;
    using System.Drawing;

    public class CallOutEffect : ITextEffect
    {
        public static Color sColor = Color.Yellow;

        public Color ChangeColor(Color color)
        {
            return sColor;
        }

        public Font ChangeFont(Font font)
        {
            return font;
        }

        public string ChangeText(string text)
        {
            return text;
        }
    }
}

