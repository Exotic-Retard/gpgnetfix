namespace GPG.Multiplayer.Client.Controls
{
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class IconStats : List<MultiVal<Image, int>>, ISelfPaint
    {
        public void PaintSelf(object painter, Graphics g, Rectangle bounds)
        {
            if (painter is GPGStatRow)
            {
                Font font = (painter as GPGStatRow).Font;
                Color dataForeColor = (painter as GPGStatRow).DataForeColor;
                int num = bounds.Width / base.Count;
                int num2 = 4;
                int num3 = 4;
                for (int i = 0; i < base.Count; i++)
                {
                    g.DrawImage(base[i].Value1, (int) (i * num), (int) (bounds.Top + num3));
                    using (Brush brush = new SolidBrush(dataForeColor))
                    {
                        g.DrawString(base[i].Value2.ToString(), font, brush, (float) (base[i].Value1.Width + (num2 + (i * num))), (float) (bounds.Top + (DrawUtil.Half((int) (base[i].Value1.Height + num3)) - DrawUtil.Half((double) DrawUtil.MeasureString(g, base[i].Value2.ToString(), font).Height))));
                    }
                }
            }
        }
    }
}

