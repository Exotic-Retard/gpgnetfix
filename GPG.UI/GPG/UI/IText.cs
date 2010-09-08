namespace GPG.UI
{
    using System;
    using System.Drawing;

    public interface IText
    {
        ITextEffect Effect { get; set; }

        string Text { get; set; }

        Color TextColor { get; set; }

        Font TextFont { get; set; }
    }
}

