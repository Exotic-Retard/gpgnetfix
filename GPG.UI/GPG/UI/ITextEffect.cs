namespace GPG.UI
{
    using System;
    using System.Drawing;

    public interface ITextEffect
    {
        Color ChangeColor(Color color);
        Font ChangeFont(Font font);
        string ChangeText(string text);
    }
}

