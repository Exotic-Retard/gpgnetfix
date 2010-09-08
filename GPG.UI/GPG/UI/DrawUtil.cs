namespace GPG.UI
{
    using GPG.Logging;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public sealed class DrawUtil
    {
        private DrawUtil()
        {
        }

        public static Image AdjustColors(float red, float green, float blue, Image original)
        {
            Bitmap image = new Bitmap(original);
            Rectangle destRect = new Rectangle(0, 0, image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(image);
            new SolidBrush(Color.FromArgb(50, 0, 0, 0));
            float[][] numArray2 = new float[5][];
            float[] numArray3 = new float[5];
            numArray3[0] = red;
            numArray2[0] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[1] = green;
            numArray2[1] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[2] = blue;
            numArray2[2] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[3] = 1f;
            numArray2[3] = numArray6;
            float[] numArray7 = new float[5];
            numArray7[4] = 1f;
            numArray2[4] = numArray7;
            float[][] newColorMatrix = numArray2;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(original, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            graphics.Dispose();
            return image;
        }

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        public static Point Center(Rectangle parent, Rectangle child)
        {
            return Center(parent.Size, child.Size);
        }

        public static Point Center(Rectangle parent, Size child)
        {
            Point point = Center(parent.Size, child);
            return new Point(point.X + parent.X, point.Y + parent.Y);
        }

        public static Point Center(Size parent, Size child)
        {
            return new Point(Half(parent.Width) - Half(child.Width), Half(parent.Height) - Half(child.Height));
        }

        public static Point Center(Control parent, Control child)
        {
            return Center(parent.ClientRectangle.Size, child.ClientRectangle.Size);
        }

        public static Point CenterAbs(Rectangle parent, Rectangle child)
        {
            return new Point((parent.X + (parent.Width / 2)) - (child.Width / 2), (parent.Y + (parent.Height / 2)) - (child.Height / 2));
        }

        public static Point CenterAbs(Control parent, Control child)
        {
            return CenterAbs(parent.Bounds, child.Bounds);
        }

        public static Point CenterH(Rectangle parent, Rectangle child)
        {
            return CenterH(parent.Width, child.Width, child.Y);
        }

        public static Point CenterH(Control parent, Control child)
        {
            return CenterH(parent.ClientRectangle.Size.Width, child.Bounds.Size.Width, child.Location.Y);
        }

        public static Point CenterH(int parentWidth, int childWidth, int childY)
        {
            return new Point(Half(parentWidth) - Half(childWidth), childY);
        }

        public static Point CenterScreen(Rectangle bounds)
        {
            return CenterAbs(Screen.PrimaryScreen.WorkingArea, bounds);
        }

        public static Point CenterScreen(Control control)
        {
            return CenterScreen(control.Bounds);
        }

        public static Point CenterV(Rectangle parent, Rectangle child)
        {
            return CenterV(parent.Height, child.Height, child.X);
        }

        public static Point CenterV(Control parent, Control child)
        {
            return CenterV(parent.ClientRectangle.Size.Height, child.Bounds.Size.Height, child.Location.X);
        }

        public static Point CenterV(int parentHeight, int childHeight, int childX)
        {
            return new Point(childX, Half(parentHeight) - Half(childHeight));
        }

        public static Image CopyImage(Image source)
        {
            return (source.Clone() as Image);
        }

        public static void CopyImage(Image source, Point sourcePt, Image dest, Point destPt, Size size)
        {
            using (Graphics graphics = Graphics.FromImage(dest))
            {
                graphics.DrawImage(source, new Rectangle(sourcePt, size), new Rectangle(destPt, size), GraphicsUnit.Pixel);
            }
        }

        public static Image CropImageBy(Image img, int width, int height)
        {
            if (img != null)
            {
                return CropImageTo(img, img.Width - width, img.Height - height);
            }
            return img;
        }

        public static Image CropImageTo(Image img, int width, int height)
        {
            if ((img != null) && (img is Bitmap))
            {
                Bitmap image = img as Bitmap;
                Bitmap bitmap2 = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap2);
                graphics.DrawImage(image, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), 0, 0, bitmap2.Width, bitmap2.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
                return bitmap2;
            }
            return img;
        }

        public static Rectangle GetScreenClipping(Control control)
        {
            return GetScreenClipping(Screen.FromControl(control).WorkingArea.Size, control.Bounds);
        }

        public static Rectangle GetScreenClipping(Size screenSize, Rectangle bounds)
        {
            int y = 0;
            int x = 0;
            int height = 0;
            int width = 0;
            if (bounds.Top < 0)
            {
                y = 1;
            }
            if (bounds.Left < 0)
            {
                x = 1;
            }
            if ((bounds.Top + bounds.Height) > screenSize.Height)
            {
                height = 1;
            }
            if ((bounds.Left + bounds.Width) > screenSize.Width)
            {
                width = 1;
            }
            return new Rectangle(x, y, width, height);
        }

        public static Image GetTransparentImage(float amount, Image original)
        {
            Bitmap image = new Bitmap(original);
            Rectangle destRect = new Rectangle(0, 0, image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(image);
            new SolidBrush(Color.FromArgb(50, 0, 0, 0));
            float[][] numArray2 = new float[5][];
            float[] numArray3 = new float[5];
            numArray3[0] = 1f;
            numArray2[0] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[1] = 1f;
            numArray2[1] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[2] = 1f;
            numArray2[2] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[3] = amount;
            numArray2[3] = numArray6;
            float[] numArray7 = new float[5];
            numArray7[4] = 1f;
            numArray2[4] = numArray7;
            float[][] newColorMatrix = numArray2;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(original, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            graphics.Dispose();
            return image;
        }

        public static int Half(double n)
        {
            return Convert.ToInt32((double) (n * 0.5));
        }

        public static int Half(int n)
        {
            return (n / 2);
        }

        public static void MakeTranparent(Control control, Image topLeft, Image topRight, Image bottomLeft, Image bottomRight)
        {
            Bitmap image = new Bitmap(control.Width + 1, control.Height + 1);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImage(topLeft, 0, 0, topLeft.Width, topLeft.Height);
            graphics.DrawImage(topRight, image.Width - topRight.Width, 0, topRight.Width, topRight.Height);
            graphics.DrawImage(bottomLeft, 0, image.Height - bottomLeft.Height, bottomLeft.Width, bottomLeft.Height);
            graphics.DrawImage(bottomRight, image.Width - bottomRight.Width, image.Height - bottomRight.Height, bottomRight.Width, bottomRight.Height);
            using (Bitmap bitmap2 = new Bitmap(2, 2))
            {
                bitmap2.SetPixel(0, 0, Color.White);
                bitmap2.SetPixel(0, 1, Color.White);
                bitmap2.SetPixel(1, 0, Color.White);
                bitmap2.SetPixel(1, 1, Color.White);
                using (TextureBrush brush = new TextureBrush(bitmap2, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height)))
                {
                    graphics.FillRectangle(brush, new Rectangle(topLeft.Width, 0, image.Width - (topLeft.Width + topRight.Width), 2));
                    graphics.FillRectangle(brush, new Rectangle(0, topLeft.Height, 2, image.Height - (topLeft.Height + bottomLeft.Height)));
                    graphics.FillRectangle(brush, new Rectangle(image.Width - 2, topRight.Height, 2, image.Height - (topRight.Height + bottomRight.Height)));
                    graphics.FillRectangle(brush, new Rectangle(bottomLeft.Width, image.Height - 2, image.Width - (bottomLeft.Width + bottomRight.Width), 2));
                }
            }
            MakeTransparent(control, image);
        }

        public static void MakeTranparentHorizontal(Control control, Image midImage)
        {
            Bitmap image = new Bitmap(control.Width, control.Height);
            Graphics.FromImage(image).DrawImage(midImage, 0, 0, midImage.Width, midImage.Height);
            MakeTransparent(control, image);
        }

        public static void MakeTranparentHorizontal(Control control, Image leftImage, Image rightImage)
        {
            Bitmap image = new Bitmap(control.Width, control.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImage(leftImage, 0, 0, leftImage.Width, leftImage.Height);
            graphics.DrawImage(rightImage, image.Width - rightImage.Width, 0, rightImage.Width, rightImage.Height);
            MakeTransparent(control, image);
        }

        public static void MakeTransparent(Control control)
        {
            MakeTransparent(control, Snapshot(control), Color.Transparent);
        }

        public static void MakeTransparent(Control control, Bitmap img)
        {
            MakeTransparent(control, img, Color.Transparent);
        }

        public static void MakeTransparent(Control control, Color transparentColor)
        {
            MakeTransparent(control, Snapshot(control), transparentColor);
        }

        public static void MakeTransparent(Control control, Bitmap img, Color transparentColor)
        {
            int capacity = img.Width * img.Height;
            capacity -= capacity / 10;
            List<Point> list = new List<Point>(capacity);
            for (int i = 0; i < img.Height; i++)
            {
                for (int k = 0; k < img.Width; k++)
                {
                    Color pixel = img.GetPixel(k, i);
                    if (!pixel.Equals(transparentColor) && (pixel.A > 0))
                    {
                        list.Add(new Point(k, i));
                        break;
                    }
                }
            }
            for (int j = img.Height - 1; j >= 0; j--)
            {
                for (int m = img.Width - 1; m >= 0; m--)
                {
                    Color color2 = img.GetPixel(m, j);
                    if (!color2.Equals(transparentColor) && (color2.A > 0))
                    {
                        list.Add(new Point(m, j));
                        break;
                    }
                }
            }
            if (list.Count >= 1)
            {
                list.Add(list[0]);
                GraphicsPath path = new GraphicsPath();
                path.AddLines(list.ToArray());
                Region region = new Region(control.ClientRectangle);
                region.Intersect(path);
                control.Region = region;
            }
        }

        public static SizeF MeasureString(Graphics g, string text, Font font)
        {
            return MeasureString(g, text, font, float.MaxValue, float.MaxValue);
        }

        public static SizeF MeasureString(Graphics g, string text, Font font, SizeF size)
        {
            return MeasureString(g, text, font, size.Width, size.Height);
        }

        public static SizeF MeasureString(Graphics g, string text, Font font, float width)
        {
            return MeasureString(g, text, font, width, float.MaxValue);
        }

        public static SizeF MeasureString(Graphics g, string text, Font font, float width, float height)
        {
            if ((text == null) || (text.Length < 1))
            {
                return new SizeF(0f, 0f);
            }
            CharacterRange[] ranges = new CharacterRange[] { new CharacterRange(0, text.Length) };
            StringFormat stringFormat = new StringFormat();
            stringFormat.SetMeasurableCharacterRanges(ranges);
            RectangleF layoutRect = new RectangleF(0f, 0f, width, height);
            Region[] regionArray = g.MeasureCharacterRanges(text, font, layoutRect, stringFormat);
            int num = text.Length - text.Replace(" ", "").Length;
            if (num > 0)
            {
                float num2 = g.MeasureString(" ", font).Width;
                SizeF size = regionArray[0].GetBounds(g).Size;
                return new SizeF(size.Width + (num2 * num), size.Height);
            }
            return regionArray[0].GetBounds(g).Size;
        }

        public static Image ResizeImage(Image img, Size newSize)
        {
            return ResizeImage(img, newSize.Width, newSize.Height);
        }

        public static Image ResizeImage(Image img, int newWidth, int newHeight)
        {
            try
            {
                if (img == null)
                {
                    return null;
                }
                if ((img.Width == newWidth) && (img.Height == newHeight))
                {
                    return img;
                }
                return new Bitmap(img, new Size(newWidth, newHeight));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        public static void RotateImage(Image img, bool clockwise)
        {
            if (clockwise)
            {
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else
            {
                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
        }

        public static Bitmap Snapshot(Control control)
        {
            Graphics g = control.CreateGraphics();
            Bitmap image = new Bitmap(control.ClientRectangle.Width, control.ClientRectangle.Height, g);
            Graphics graphics2 = Graphics.FromImage(image);
            IntPtr hdc = g.GetHdc();
            IntPtr hdcDest = graphics2.GetHdc();
            BitBlt(hdcDest, 0, 0, control.ClientRectangle.Width, control.ClientRectangle.Height, hdc, 0, 0, 0xcc0020);
            g.ReleaseHdc(hdc);
            graphics2.ReleaseHdc(hdcDest);
            return image;
        }

        public static string[] SplitString(string text, int chunkSize)
        {
            List<string> list = new List<string>((text.Length / chunkSize) + 1);
            int startIndex = 0;
            while (startIndex < text.Length)
            {
                if (chunkSize >= (text.Length - startIndex))
                {
                    list.Add(text.Substring(startIndex, chunkSize));
                    startIndex += chunkSize;
                }
                else
                {
                    list.Add(text.Substring(startIndex, text.Length - startIndex));
                    startIndex += text.Length - startIndex;
                }
            }
            return list.ToArray();
        }

        public static string[] SplitString(string text, string splitter)
        {
            List<string> list = new List<string>();
            int startIndex = 0;
            int index = text.IndexOf(splitter, startIndex);
            while (index >= 0)
            {
                list.Add(text.Substring(startIndex, (index + 1) - startIndex));
                startIndex = index + 1;
                index = text.IndexOf(splitter, startIndex);
            }
            if (index < text.Length)
            {
                index = text.Length;
                list.Add(text.Substring(startIndex, index - startIndex));
            }
            return list.ToArray();
        }
    }
}

