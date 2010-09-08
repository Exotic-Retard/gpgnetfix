namespace GPG.ImageConverter
{
    using FreeImageAPI;
    using GPG.Logging;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public static class ConvertDDS
    {
        [DllImport("Gdi32.dll")]
        private static extern int SetStretchBltMode(IntPtr HDC, int iStretchMode);
        [DllImport("Gdi32.dll")]
        private static extern int StretchDIBits(IntPtr HDC, int XDest, int YDest, int nDestWidth, int nDestHeight, int XSrc, int YSrc, int nSrcWidth, int nSrcHeight, IntPtr lpBits, IntPtr lpBitsInfo, uint iUsage, int dwRop);
        public static Bitmap ToBitmap(string file)
        {
            try
            {
                Bitmap image = new Bitmap(50, 50);
                uint dib = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_DDS, file, 0);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    IntPtr hdc = graphics.GetHdc();
                    SetStretchBltMode(hdc, 3);
                    StretchDIBits(hdc, 0, 0, image.Width, image.Height, 0, 0, (int) FreeImage.GetWidth(dib), (int) FreeImage.GetHeight(dib), FreeImage.GetBits(dib), FreeImage.FreeImage_GetInfo(dib), 0, 0xcc0020);
                    graphics.ReleaseHdc(hdc);
                }
                return image;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }
    }
}

