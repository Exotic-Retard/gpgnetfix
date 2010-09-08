namespace GPG.UI
{
    using System;
    using System.Drawing;

    public interface ISelfPaint
    {
        void PaintSelf(object painter, Graphics g, Rectangle bounds);
    }
}

