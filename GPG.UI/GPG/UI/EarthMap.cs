namespace GPG.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.IO;
    using System.Windows.Forms;

    public class EarthMap : Control
    {
        private Container components;
        public string LuaDoubleClickFunction = "";
        private bool mapMoved = true;
        private const float MAX_FLAG_RADIUS = 15f;
        private const float MAX_IMG_RADIUS = 15f;
        private const float MAX_RADIUS = 50f;
        private Bitmap mCacheData;
        private byte[] mCountryData;
        private List<string> mFilters = new List<string>();
        private List<MapLocation> mGameReports = new List<MapLocation>();
        private bool mHighRes;
        private byte[] mHighResData;
        private byte[] mHighResHeader;
        private const float MIN_FLAG_RADIUS = 4f;
        private const float MIN_IMG_RADIUS = 4f;
        private const float MIN_RADIUS = 1f;
        private int mLastMouseX;
        private int mLastMouseY;
        private int mLastTick = Environment.TickCount;
        private bool mLoaded;
        private ArrayList mMapLocationList = new ArrayList();
        private float mMapScale = 0.03935185f;
        private MapLocation mMapSelectedLocation;
        private PackagedFile mMapTiles = new PackagedFile();
        private float mMapX;
        private float mMapY;
        private List<MapLocation> mNonDots = new List<MapLocation>();
        private Random mRandom = new Random();
        private ZoomInfo mRenderInfo;
        private bool mShowBorders;
        private float mSmootheMapScale = 0.03935185f;
        private float mSmootheX;
        private float mSmootheY;
        private Image mWatermarkLeft;
        private Image mWatermarkRight;
        private ArrayList mZoomInfoList = new ArrayList();
        private const bool SMOOTHE_MODE = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ShowBordersChanged;

        public EarthMap()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            base.SetStyle(ControlStyles.ContainerControl, false);
            string path = Application.StartupPath + @"\map\countries.data";
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open);
                this.mCountryData = new byte[stream.Length];
                stream.Read(this.mCountryData, 0, (int) stream.Length);
                stream.Close();
            }
            string str2 = Application.StartupPath + @"\map\map.gpgnet";
            if (File.Exists(Application.StartupPath + @"\map\xmap.bin"))
            {
                FileStream stream2 = File.Open(Application.StartupPath + @"\map\map.bin", FileMode.Open);
                this.mHighResData = new byte[(int) stream2.Length];
                stream2.Read(this.mHighResData, 0, (int) stream2.Length);
                stream2.Close();
                stream2 = File.Open(Application.StartupPath + @"\map\header.bin", FileMode.Open);
                this.mHighResHeader = new byte[(int) stream2.Length];
                stream2.Read(this.mHighResHeader, 0, (int) stream2.Length);
                stream2.Close();
                this.mLoaded = true;
                this.mHighRes = true;
            }
            else if (File.Exists(str2))
            {
                this.mLoaded = true;
                this.mMapTiles.LoadPackage(str2, true);
            }
            else
            {
                str2 = @"C:\work\rts\main\code\src\Multiplayer\MultiplayerClient\bin\Debug\map.gpgnet";
                if (File.Exists(str2))
                {
                    this.mLoaded = true;
                    this.mMapTiles.LoadPackage(str2, true);
                }
            }
            if (this.mHighRes)
            {
                ZoomInfo info = new ZoomInfo();
                info.Prefix = "0";
                info.HorizontalTiles = 320;
                info.VerticleTiles = 160;
                info.ZoomThreshold = 6.068148f;
                this.mZoomInfoList.Add(info);
                info = new ZoomInfo();
                info.Prefix = "1";
                info.HorizontalTiles = 160;
                info.VerticleTiles = 80;
                info.ZoomThreshold = 3.034074f;
                this.mZoomInfoList.Add(info);
                info = new ZoomInfo();
                info.Prefix = "2";
                info.HorizontalTiles = 80;
                info.VerticleTiles = 40;
                info.ZoomThreshold = 1.517037f;
                this.mZoomInfoList.Add(info);
                info = new ZoomInfo();
                info.Prefix = "3";
                info.HorizontalTiles = 40;
                info.VerticleTiles = 20;
                info.ZoomThreshold = 0.7585185f;
                this.mZoomInfoList.Add(info);
                info = new ZoomInfo();
                info.Prefix = "4";
                info.HorizontalTiles = 20;
                info.VerticleTiles = 10;
                info.ZoomThreshold = 0.3792593f;
                this.mZoomInfoList.Add(info);
                info = new ZoomInfo();
                info.Prefix = "5";
                info.HorizontalTiles = 10;
                info.VerticleTiles = 5;
                info.ZoomThreshold = 0.1896296f;
                this.mZoomInfoList.Add(info);
            }
            else
            {
                ZoomInfo info2 = new ZoomInfo();
                this.mZoomInfoList.Add(info2);
                ZoomInfo info3 = new ZoomInfo();
                info3.Prefix = "medium_";
                info3.HorizontalTiles = 20;
                info3.VerticleTiles = 10;
                info3.ZoomThreshold = 0.2f;
                this.mZoomInfoList.Add(info3);
                ZoomInfo info4 = new ZoomInfo();
                info4.Prefix = "large_";
                info4.HorizontalTiles = 4;
                info4.VerticleTiles = 2;
                info4.ZoomThreshold = 0.04f;
                this.mZoomInfoList.Add(info4);
                this.mRenderInfo = info4;
            }
            this.BackColor = Color.FromArgb(3, 6, 0x15);
        }

        public void AddLocation(MapLocation mapLocation)
        {
            if (mapLocation.LocationType != LocationTypes.Dot)
            {
                this.mapMoved = true;
                this.mNonDots.Add(mapLocation);
                if (mapLocation.Classification == "GameResult")
                {
                    this.mGameReports.Add(mapLocation);
                    while (this.mGameReports.Count > 20)
                    {
                        MapLocation item = this.mGameReports[0];
                        this.mGameReports.Remove(item);
                        this.mNonDots.Remove(item);
                        this.mMapLocationList.Remove(item);
                    }
                }
                base.Invalidate();
            }
            else if (mapLocation.LocationType == LocationTypes.Dot)
            {
                for (int i = this.mMapLocationList.Count - 1; i >= 0; i--)
                {
                    MapLocation location2 = this.mMapLocationList[i] as MapLocation;
                    if (((location2 != null) && (location2.Latitude == mapLocation.Latitude)) && ((location2.Longitude == mapLocation.Longitude) && (location2.Classification == mapLocation.Classification)))
                    {
                        location2.Strength += 0.05f * ((10f - location2.Strength) / 10f);
                        return;
                    }
                }
            }
            mapLocation.Visible = !this.mFilters.Contains(mapLocation.Classification);
            this.mMapLocationList.Add(mapLocation);
        }

        private void CheckBounds()
        {
            float num = 2541.177f;
            if (this.mMapScale < (((float) base.Width) / (21600f + num)))
            {
                this.mapMoved = true;
                this.mMapScale = ((float) base.Width) / (21600f + num);
            }
            if (this.mHighRes)
            {
                if (this.mMapScale > 50f)
                {
                    this.mapMoved = true;
                    this.mMapScale = 50f;
                }
            }
            else if (this.mMapScale > 1f)
            {
                this.mapMoved = true;
                this.mMapScale = 1f;
            }
            if (this.mMapX > ((21600f + num) - (((float) base.Width) / this.mMapScale)))
            {
                this.mapMoved = true;
                this.mMapX = (21600f + num) - (((float) base.Width) / this.mMapScale);
            }
            if (this.mMapX < 0f)
            {
                this.mapMoved = true;
                this.mMapX = 0f;
            }
            if (this.mMapY > (10800f - (((float) base.Height) / this.mMapScale)))
            {
                this.mapMoved = true;
                this.mMapY = 10800f - (((float) base.Height) / this.mMapScale);
            }
            if (this.mMapY < 0f)
            {
                this.mapMoved = true;
                this.mMapY = 0f;
            }
        }

        private bool CheckHit(RectangleF source, RectangleF target)
        {
            if (!this.CheckIntersection(source, target))
            {
                return this.CheckIntersection(target, source);
            }
            return true;
        }

        private bool CheckIntersection(RectangleF source, RectangleF target)
        {
            if (target.X > (source.X + source.Width))
            {
                return false;
            }
            if ((target.X + target.Width) < source.X)
            {
                return false;
            }
            if (target.Y > (source.Y + source.Height))
            {
                return false;
            }
            if ((target.Y + target.Height) < source.Y)
            {
                return false;
            }
            return true;
        }

        private MapLocation CheckMapHit(float x, float y)
        {
            if (this.mRenderInfo != null)
            {
                float num = this.mSmootheMapScale / this.mRenderInfo.ZoomThreshold;
                float num2 = this.mRenderInfo.RelativeX(this.mSmootheX) * num;
                float num3 = this.mRenderInfo.RelativeY(this.mSmootheY) * num;
                foreach (MapLocation location2 in this.mMapLocationList)
                {
                    if (!location2.Visible || (location2.LocationType == LocationTypes.Dot))
                    {
                        continue;
                    }
                    float num4 = (this.mRenderInfo.RelativeX(location2.MapX) * num) - num2;
                    float num5 = (this.mRenderInfo.RelativeY(location2.MapY) * num) - num3;
                    if (location2.LocationType == LocationTypes.Flag)
                    {
                        float num6 = 15f * this.mSmootheMapScale;
                        if (num6 < 4f)
                        {
                            num6 = 4f;
                        }
                        else if (num6 > 15f)
                        {
                            num6 = 15f;
                        }
                        if (((x > (num4 - num6)) && (x < (num4 + num6))) && ((y > (num5 - (num6 * 4f))) && (y < num5)))
                        {
                            return location2;
                        }
                    }
                    else if (location2.LocationType == LocationTypes.Image)
                    {
                        float num7 = 15f * this.mSmootheMapScale;
                        if (num7 < 4f)
                        {
                            num7 = 4f;
                        }
                        else if (num7 > 15f)
                        {
                            num7 = 15f;
                        }
                        if (((x > (num4 - (num7 * 2f))) && (x < (num4 + (num7 * 2f)))) && ((y > (num5 - (num7 * 2f))) && (y < (num5 + (num7 * 2f)))))
                        {
                            return location2;
                        }
                    }
                    RectangleF target = new RectangleF(x, y, 1f, 1f);
                    if (this.CheckHit(location2.StringRect, target))
                    {
                        return location2;
                    }
                }
            }
            return null;
        }

        public void ClearLocations()
        {
            this.mNonDots.Clear();
            this.mMapLocationList.Clear();
        }

        public void ClearLocations(string classification)
        {
            for (int i = this.mNonDots.Count - 1; i >= 0; i--)
            {
                if (this.mNonDots[i].Classification == classification)
                {
                    this.mMapLocationList.Remove(this.mNonDots[i]);
                    this.mNonDots.RemoveAt(i);
                }
            }
        }

        private Color DimColor(int factor, Color color)
        {
            int red = color.R - factor;
            if (red < 0)
            {
                red = 0;
            }
            if (red > 0xff)
            {
                red = 0xff;
            }
            int green = color.G - factor;
            if (green < 0)
            {
                green = 0;
            }
            if (green > 0xff)
            {
                green = 0xff;
            }
            int blue = color.B - factor;
            if (blue < 0)
            {
                blue = 0;
            }
            if (blue > 0xff)
            {
                blue = 0xff;
            }
            return Color.FromArgb(color.A, red, green, blue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DrawText(Graphics canvas, Graphics textcanvas, ref List<RectangleF> textBoxes, float radius, float x, float y, MapLocation mapLocation, float centerx, float centery)
        {
            Brush brush = new SolidBrush(Color.FromArgb(0xff, Color.Black));
            Brush brush2 = new SolidBrush(Color.FromArgb(200, mapLocation.MapColor));
            Font font = new Font(this.Font.FontFamily, this.GetFontSize(radius));
            SizeF size = canvas.MeasureString(mapLocation.Information, font);
            float num = x;
            float num2 = y;
            float num3 = 0f;
            float num4 = 0f;
            float num5 = 0f;
            if (!this.mapMoved)
            {
                goto Label_01F2;
            }
            RectangleF target = new RectangleF(new PointF(num, num2), size);
            int num6 = -1;
            bool flag = textBoxes.Count > 0;
        Label_01AB:
            while (flag)
            {
                num = x + num3;
                num2 = y + num4;
                target = new RectangleF(new PointF(num, num2), size);
                flag = false;
                foreach (RectangleF ef3 in textBoxes)
                {
                    if (!this.CheckHit(ef3, target))
                    {
                        continue;
                    }
                    num6++;
                    if (num6 >= 8)
                    {
                        num6 = 0;
                    }
                    if (num6 == 0)
                    {
                        num5 += 2f;
                        num3 = num5;
                        num4 = 0f;
                    }
                    else if (num6 == 1)
                    {
                        num4 = num3;
                        num3 = 0f;
                    }
                    else if (num6 == 2)
                    {
                        num3 = 0f - num4;
                        num4 = 0f;
                    }
                    else if (num6 == 3)
                    {
                        num4 = num3;
                        num3 = 0f;
                    }
                    else if (num6 == 4)
                    {
                        num3 = num4;
                    }
                    else if (num6 == 5)
                    {
                        num3 = 0f - num3;
                    }
                    else if (num6 == 6)
                    {
                        num4 = 0f - num4;
                    }
                    else if (num6 == 7)
                    {
                        num3 = 0f - num3;
                    }
                    flag = true;
                    goto Label_01AB;
                }
            }
            mapLocation.StringXoffset = num3;
            mapLocation.StringYoffset = num4;
            mapLocation.StringWidth = target.Width;
            mapLocation.StringHeight = target.Height;
            textBoxes.Add(target);
            mapLocation.StringRect = target;
        Label_01F2:
            num = x + mapLocation.StringXoffset;
            num2 = y + mapLocation.StringYoffset;
            mapLocation.StringRect.X = num;
            mapLocation.StringRect.Y = num2;
            Brush brush3 = new SolidBrush(Color.FromArgb(0x7d, 0, 0, 0));
            Pen pen = new Pen(Color.FromArgb(0x7d, mapLocation.MapColor));
            textcanvas.FillRectangle(brush3, num, num2, mapLocation.StringWidth, mapLocation.StringHeight);
            textcanvas.DrawRectangle(pen, num, num2, mapLocation.StringWidth, mapLocation.StringHeight);
            pen.Dispose();
            brush3.Dispose();
            textcanvas.DrawString(mapLocation.Information, font, brush2, num, num2);
            float num7 = num + (mapLocation.StringWidth / 2f);
            float num8 = num2 + (mapLocation.StringHeight / 2f);
            if ((num7 > num) && (centerx < num))
            {
                num7 = num;
            }
            if ((num7 < (num + mapLocation.StringWidth)) && (centerx > (num + mapLocation.StringWidth)))
            {
                num7 = num + mapLocation.StringWidth;
            }
            if ((num8 > num2) && (centery < num2))
            {
                num8 = num2;
            }
            if ((num8 < (num2 + mapLocation.StringHeight)) && (centery > (num2 + mapLocation.StringHeight)))
            {
                num8 = num2 + mapLocation.StringHeight;
            }
            Pen pen2 = new Pen(Color.FromArgb(150, mapLocation.MapColor), 2f);
            canvas.DrawLine(pen2, centerx, centery, num7, num8);
            pen2.Dispose();
            brush.Dispose();
            brush2.Dispose();
        }

        private float GetFontSize(float radius)
        {
            if (radius < 9f)
            {
                return 9f;
            }
            return radius;
        }

        private ZoomInfo GetZoomLevel()
        {
            ZoomInfo info = null;
            foreach (ZoomInfo info2 in this.mZoomInfoList)
            {
                if (info == null)
                {
                    info = info2;
                }
                else
                {
                    if (info.ZoomThreshold > (this.mSmootheMapScale + 0.5f))
                    {
                        info = info2;
                        continue;
                    }
                    if ((info2.ZoomThreshold < (this.mSmootheMapScale + 0.5f)) && (info2.ZoomThreshold > info.ZoomThreshold))
                    {
                        info = info2;
                    }
                }
            }
            this.mRenderInfo = info;
            return info;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!this.Focused)
            {
                base.Focus();
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                int num = e.X - this.mLastMouseX;
                int num2 = e.Y - this.mLastMouseY;
                float num3 = ((float) num) / this.mSmootheMapScale;
                float num4 = ((float) num2) / this.mSmootheMapScale;
                float mapX = this.MapX;
                float mapY = this.MapY;
                this.MapX -= num3;
                this.MapY -= num4;
            }
            this.mLastMouseX = e.X;
            this.mLastMouseY = e.Y;
            MapLocation location = this.CheckMapHit((float) this.mLastMouseX, (float) this.mLastMouseY);
            if ((this.mMapSelectedLocation != location) && (location != null))
            {
                this.mMapSelectedLocation = location;
                base.Invalidate();
            }
            else if ((this.mMapSelectedLocation != null) && (location == null))
            {
                this.mMapSelectedLocation = null;
                base.Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            this.mMapSelectedLocation = this.CheckMapHit((float) this.mLastMouseX, (float) this.mLastMouseY);
            float mapScale = this.MapScale;
            float num2 = this.MapScale / 3f;
            if (e.Delta < 0)
            {
                num2 = 0f - num2;
                this.mMapScale += num2;
            }
            else
            {
                this.mMapScale += num2;
            }
            this.CheckBounds();
            if (mapScale != this.MapScale)
            {
                this.mMapX -= (((float) base.Width) / 2f) / this.mMapScale;
                this.mMapY -= (((float) base.Height) / 2f) / this.mMapScale;
                this.mMapX += (((float) base.Width) / 2f) / mapScale;
                this.mMapY += (((float) base.Height) / 2f) / mapScale;
                if (e.Delta > 0)
                {
                    this.mMapX -= (e.X - (((float) base.Width) / 2f)) / this.mMapScale;
                    this.mMapY -= (e.Y - (((float) base.Height) / 2f)) / this.mMapScale;
                    this.mMapX += (e.X - (((float) base.Width) / 2f)) / mapScale;
                    this.mMapY += (e.Y - (((float) base.Height) / 2f)) / mapScale;
                }
                this.CheckBounds();
                this.mapMoved = true;
            }
            this.mSmootheX = this.mMapX;
            this.mSmootheY = this.mMapY;
            this.mSmootheMapScale = this.mMapScale;
            base.Update();
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.mLoaded)
            {
                Graphics canvas = pe.Graphics;
                this.PaintCanvas(canvas);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.CheckBounds();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.mMapTiles != null)
            {
                this.mMapTiles.ClearCache();
            }
        }

        private void PaintCanvas(Graphics canvas)
        {
            canvas.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            int tickCount = Environment.TickCount;
            canvas.SmoothingMode = SmoothingMode.HighSpeed;
            Brush brush = new SolidBrush(Color.FromArgb(3, 6, 0x15));
            canvas.FillRectangle(brush, base.ClientRectangle);
            brush.Dispose();
            ZoomInfo zoomLevel = this.GetZoomLevel();
            float num2 = this.mSmootheMapScale / zoomLevel.ZoomThreshold;
            float num3 = zoomLevel.RelativeX(this.mSmootheX) * num2;
            float num4 = zoomLevel.RelativeY(this.mSmootheY) * num2;
            float num5 = num2 * zoomLevel.TileWidth;
            float num6 = num2 * zoomLevel.TileHeight;
            int num7 = (int) (num3 / num5);
            if (num7 < 0)
            {
                num7 = 0;
            }
            int horizontalTiles = (num7 + ((int) (((float) base.Width) / num5))) + 1;
            if (horizontalTiles > zoomLevel.HorizontalTiles)
            {
                horizontalTiles = zoomLevel.HorizontalTiles;
            }
            int num9 = (int) (num4 / num6);
            if (num9 < 0)
            {
                num9 = 0;
            }
            int verticleTiles = (num9 + ((int) (((float) base.Height) / num6))) + 1;
            if (verticleTiles > zoomLevel.VerticleTiles)
            {
                verticleTiles = zoomLevel.VerticleTiles;
            }
            RectangleF srcRect = new RectangleF(0f, 0f, (float) zoomLevel.TileWidth, (float) zoomLevel.TileHeight);
            float num11 = num2;
            for (int i = num7; i <= horizontalTiles; i++)
            {
                for (int j = num9; j <= verticleTiles; j++)
                {
                    float x = (i * num5) - num3;
                    float num15 = (j * num6) - num4;
                    if (this.mHighRes)
                    {
                        srcRect = new RectangleF(0f, 0f, 256f, 256f);
                        int index = 0;
                        int num17 = Convert.ToInt32(zoomLevel.Prefix);
                        for (index *= 11; index < (this.mHighResHeader.Length - 11); index += 11)
                        {
                            byte num18 = this.mHighResHeader[index];
                            if (num18 == num17)
                            {
                                num11 = 0f;
                                short num19 = BitConverter.ToInt16(this.mHighResHeader, index + 1);
                                short num20 = BitConverter.ToInt16(this.mHighResHeader, index + 3);
                                int offset = BitConverter.ToInt32(this.mHighResHeader, index + 5);
                                short count = BitConverter.ToInt16(this.mHighResHeader, index + 9);
                                if ((num19 == i) && (num20 == j))
                                {
                                    if (count == 0)
                                    {
                                        Brush brush2 = new SolidBrush(Color.FromArgb(3, 6, 0x15));
                                        RectangleF rect = new RectangleF(x, num15, num5 + num11, num6 + num11);
                                        canvas.FillRectangle(brush2, rect);
                                        brush2.Dispose();
                                    }
                                    else
                                    {
                                        MemoryStream stream = new MemoryStream();
                                        stream.Write(this.mHighResData, offset, count);
                                        Image image = Image.FromStream(stream);
                                        RectangleF destRect = new RectangleF(x, num15, num5 + num11, num6 + num11);
                                        canvas.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string filename = zoomLevel.Prefix + "slice_" + i.ToString() + "_" + j.ToString() + ".jpg";
                        Image fileAsImage = this.mMapTiles.GetFileAsImage(filename);
                        RectangleF ef4 = new RectangleF(x, num15, num5 + num11, num6 + num11);
                        canvas.DrawImage(fileAsImage, ef4, srcRect, GraphicsUnit.Pixel);
                    }
                }
            }
            MapLocation location = new MapLocation();
            location.Latitude = -58f;
            float y = (zoomLevel.RelativeY(location.MapY) * num2) - num4;
            if ((0f < y) && (y < base.Height))
            {
                Brush brush3 = new SolidBrush(Color.FromArgb(3, 6, 0x15));
                RectangleF ef5 = new RectangleF(0f, y, (float) base.Width, base.Height - y);
                canvas.FillRectangle(brush3, ef5);
                brush3.Dispose();
            }
            if (this.WatermarkLeft != null)
            {
                canvas.DrawImage(this.WatermarkLeft, 20, (base.Height - this.WatermarkLeft.Height) - 20);
            }
            if (this.WatermarkRight != null)
            {
                canvas.DrawImage(this.WatermarkRight, (int) ((base.Width - this.WatermarkRight.Width) - 20), (int) ((base.Height - this.WatermarkRight.Height) - 20));
            }
            if (this.ShowBorders)
            {
                float num24 = 0f;
                float num25 = 0f;
                float width = (50f * this.mSmootheMapScale) / 4f;
                if (width < 1f)
                {
                    width = 1f;
                }
                else if (width > 3f)
                {
                    width = 3f;
                }
                Pen pen = new Pen(Color.FromArgb(200, 0, 0, 0), width);
                for (int k = 0; k < (this.mCountryData.Length - 8); k += 8)
                {
                    float num28 = BitConverter.ToSingle(this.mCountryData, k) + 10800f;
                    float num29 = 5400f - BitConverter.ToSingle(this.mCountryData, k + 4);
                    if ((num28 != 10802f) && (num24 != 10802f))
                    {
                        PointF tf = new PointF((zoomLevel.RelativeX(num24) * num2) - num3, (zoomLevel.RelativeY(num25) * num2) - num4);
                        PointF tf2 = new PointF((zoomLevel.RelativeX(num28) * num2) - num3, (zoomLevel.RelativeY(num29) * num2) - num4);
                        canvas.DrawLine(pen, tf, tf2);
                    }
                    num24 = num28;
                    num25 = num29;
                }
                pen.Dispose();
            }
            List<RectangleF> textBoxes = new List<RectangleF>();
            this.SetTextBoxes(textBoxes);
            float num30 = 500f;
            Bitmap bitmap = new Bitmap(base.Width, base.Height);
            Graphics textcanvas = Graphics.FromImage(bitmap);
            foreach (MapLocation location2 in this.mMapLocationList)
            {
                if (!location2.Visible)
                {
                    continue;
                }
                float centerx = 0f;
                float centery = 0f;
                if ((location2.Latitude == 0f) && (location2.Longitude == 0f))
                {
                    centerx = (zoomLevel.RelativeX(21600f) * num2) - num3;
                    centery = (zoomLevel.RelativeY(num30) * num2) - num4;
                    location2.UnknownLocYPos = centery;
                    location2.UnknownLocXPos = centerx;
                    num30 += 500f;
                }
                else
                {
                    centerx = (zoomLevel.RelativeX(location2.MapX) * num2) - num3;
                    centery = (zoomLevel.RelativeY(location2.MapY) * num2) - num4;
                }
                float radius = 50f * this.mSmootheMapScale;
                if (radius < 1f)
                {
                    radius = 1f;
                }
                else if (radius > 50f)
                {
                    radius = 50f;
                }
                if (location2.LocationType == LocationTypes.Flag)
                {
                    if (radius < 4f)
                    {
                        radius = 4f;
                    }
                    else if (radius > 15f)
                    {
                        radius = 15f;
                    }
                    Brush brush4 = new SolidBrush(Color.Black);
                    Brush brush5 = new SolidBrush(location2.MapColor);
                    Brush brush6 = new SolidBrush(this.DimColor(0x42, location2.MapColor));
                    Pen pen2 = new Pen(this.DimColor(0x21, location2.MapColor), 2f);
                    PointF[] points = new PointF[3];
                    points[0].X = centerx - radius;
                    points[0].Y = centery - (radius * 2.75f);
                    points[1].X = centerx;
                    points[1].Y = centery + (radius * 0.25f);
                    points[2].X = centerx + radius;
                    points[2].Y = centery - (radius * 2.75f);
                    canvas.FillPolygon(brush4, points);
                    canvas.FillEllipse(brush4, (float) (centerx - (radius * 1.125f)), (float) (centery - (radius * 4.125f)), (float) (radius * 2.25f), (float) (radius * 2.25f));
                    points = new PointF[3];
                    points[0].X = centerx - radius;
                    points[0].Y = centery - (radius * 3f);
                    points[1].X = centerx;
                    points[1].Y = centery;
                    points[2].X = centerx + radius;
                    points[2].Y = centery - (radius * 3f);
                    canvas.FillPolygon(brush6, points);
                    canvas.FillEllipse(brush5, (float) (centerx - radius), (float) (centery - (radius * 4f)), (float) (radius * 2f), (float) (radius * 2f));
                    canvas.DrawEllipse(pen2, (float) (centerx - radius), (float) (centery - (radius * 4f)), (float) (radius * 2f), (float) (radius * 2f));
                    if (location2.Information != null)
                    {
                        this.DrawText(canvas, textcanvas, ref textBoxes, radius, centerx + (radius * 3f), centery - (radius * 4f), location2, centerx, centery);
                    }
                }
                if (location2.LocationType == LocationTypes.Image)
                {
                    if (radius < 4f)
                    {
                        radius = 4f;
                    }
                    else if (radius > 15f)
                    {
                        radius = 15f;
                    }
                    float num34 = 1.75f;
                    RectangleF ef6 = new RectangleF(centerx - (radius * num34), centery - (radius * num34), (radius * num34) * 2f, (radius * num34) * 2f);
                    if (location2.Information != null)
                    {
                        this.DrawText(canvas, textcanvas, ref textBoxes, radius, centerx + (radius * 3f), centery - (radius * 1.5f), location2, centerx, centery);
                    }
                    if (location2 != this.SelectedLocation)
                    {
                        canvas.DrawImage(location2.MapImage, ef6);
                    }
                    if (location2.DrawImageBorder)
                    {
                        Pen pen3 = new Pen(location2.MapColor);
                        canvas.DrawRectangle(pen3, ef6.X, ef6.Y, ef6.Width, ef6.Height);
                        pen3.Dispose();
                    }
                }
                if (location2.LocationType == LocationTypes.Dot)
                {
                    radius *= location2.Strength;
                    Brush brush7 = new SolidBrush(location2.MapColor);
                    radius /= 8f;
                    canvas.FillEllipse(brush7, (float) (centerx - radius), (float) (centery - radius), (float) (radius * 2f), (float) (radius * 2f));
                }
            }
            canvas.DrawImage(bitmap, 0, 0);
            textcanvas.Dispose();
            if (this.mMapSelectedLocation != null)
            {
                float unknownLocXPos = (zoomLevel.RelativeX(this.mMapSelectedLocation.MapX) * num2) - num3;
                float unknownLocYPos = (zoomLevel.RelativeY(this.mMapSelectedLocation.MapY) * num2) - num4;
                if ((this.mMapSelectedLocation.Latitude == 0f) && (this.mMapSelectedLocation.Longitude == 0f))
                {
                    unknownLocXPos = this.mMapSelectedLocation.UnknownLocXPos;
                    unknownLocYPos = this.mMapSelectedLocation.UnknownLocYPos;
                }
                float num37 = 50f * this.mSmootheMapScale;
                if (num37 < 1f)
                {
                    num37 = 1f;
                }
                else if (num37 > 50f)
                {
                    num37 = 50f;
                }
                if (this.mMapSelectedLocation.LocationType == LocationTypes.Flag)
                {
                    if (num37 < 4f)
                    {
                        num37 = 4f;
                    }
                    else if (num37 > 15f)
                    {
                        num37 = 15f;
                    }
                    Brush brush8 = new SolidBrush(Color.FromArgb(0xff, this.mMapSelectedLocation.MapColor));
                    Brush brush9 = new SolidBrush(this.DimColor(0x21, this.mMapSelectedLocation.MapColor));
                    Pen pen4 = new Pen(Color.White, 2f);
                    PointF[] tfArray2 = new PointF[3];
                    tfArray2[0].X = unknownLocXPos - num37;
                    tfArray2[0].Y = unknownLocYPos - (num37 * 3f);
                    tfArray2[1].X = unknownLocXPos;
                    tfArray2[1].Y = unknownLocYPos;
                    tfArray2[2].X = unknownLocXPos + num37;
                    tfArray2[2].Y = unknownLocYPos - (num37 * 3f);
                    canvas.FillPolygon(brush9, tfArray2);
                    canvas.DrawLine(pen4, tfArray2[0], tfArray2[1]);
                    canvas.DrawLine(pen4, tfArray2[1], tfArray2[2]);
                    canvas.FillEllipse(brush8, (float) (unknownLocXPos - num37), (float) (unknownLocYPos - (num37 * 4f)), (float) (num37 * 2f), (float) (num37 * 2f));
                    canvas.DrawEllipse(pen4, (float) (unknownLocXPos - num37), (float) (unknownLocYPos - (num37 * 4f)), (float) (num37 * 2f), (float) (num37 * 2f));
                    if (this.mMapSelectedLocation.Information != null)
                    {
                        Brush brush10 = new SolidBrush(Color.FromArgb(0x7d, 0, 0, 0));
                        Pen pen5 = new Pen(Color.FromArgb(0xff, this.mMapSelectedLocation.MapColor));
                        canvas.FillRectangle(brush10, (unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset, (unknownLocYPos - (num37 * 4f)) + this.mMapSelectedLocation.StringYoffset, this.mMapSelectedLocation.StringWidth, this.mMapSelectedLocation.StringHeight);
                        canvas.DrawRectangle(pen5, (unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset, (unknownLocYPos - (num37 * 4f)) + this.mMapSelectedLocation.StringYoffset, this.mMapSelectedLocation.StringWidth, this.mMapSelectedLocation.StringHeight);
                        pen5.Dispose();
                        brush10.Dispose();
                        Brush brush11 = new SolidBrush(Color.Black);
                        Font font = new Font(this.Font.FontFamily, this.GetFontSize(num37));
                        canvas.DrawString(this.mMapSelectedLocation.Information, font, brush11, (float) (((unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset) + 1f), (float) (((unknownLocYPos - (num37 * 4f)) + this.mMapSelectedLocation.StringYoffset) + 1f));
                        canvas.DrawString(this.mMapSelectedLocation.Information, font, brush8, (float) ((unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset), (float) ((unknownLocYPos - (num37 * 4f)) + this.mMapSelectedLocation.StringYoffset));
                    }
                }
                if (this.mMapSelectedLocation.LocationType == LocationTypes.Image)
                {
                    if (num37 < 4f)
                    {
                        num37 = 4f;
                    }
                    else if (num37 > 15f)
                    {
                        num37 = 15f;
                    }
                    RectangleF ef7 = new RectangleF(unknownLocXPos - (num37 * 2f), unknownLocYPos - (num37 * 2f), num37 * 4f, num37 * 4f);
                    canvas.DrawImage(this.mMapSelectedLocation.MapImage, ef7);
                    if (this.mMapSelectedLocation.DrawImageBorder)
                    {
                        Pen pen6 = new Pen(this.mMapSelectedLocation.MapColor);
                        canvas.DrawRectangle(pen6, ef7.X, ef7.Y, ef7.Width, ef7.Height);
                        pen6.Dispose();
                    }
                    if (this.mMapSelectedLocation.Information != null)
                    {
                        Font font2 = new Font(this.Font.FontFamily, this.GetFontSize(num37));
                        string information = this.mMapSelectedLocation.Information;
                        if (this.mMapSelectedLocation.ExtendedInformation != "")
                        {
                            information = this.mMapSelectedLocation.ExtendedInformation;
                        }
                        SizeF ef8 = canvas.MeasureString(information, font2);
                        ef8.Width += 10f;
                        Brush brush12 = new SolidBrush(Color.FromArgb(200, this.mMapSelectedLocation.MapColor.R / 3, this.mMapSelectedLocation.MapColor.G / 3, this.mMapSelectedLocation.MapColor.B / 3));
                        Pen pen7 = new Pen(Color.FromArgb(0xff, this.mMapSelectedLocation.MapColor));
                        canvas.FillRectangle(brush12, (unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset, (unknownLocYPos - (num37 * 1.5f)) + this.mMapSelectedLocation.StringYoffset, ef8.Width, ef8.Height);
                        canvas.DrawRectangle(pen7, (unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset, (unknownLocYPos - (num37 * 1.5f)) + this.mMapSelectedLocation.StringYoffset, ef8.Width, ef8.Height);
                        pen7.Dispose();
                        brush12.Dispose();
                        Brush brush13 = new SolidBrush(Color.FromArgb(150, Color.Black));
                        Brush brush14 = new SolidBrush(Color.FromArgb(0xff, this.mMapSelectedLocation.MapColor));
                        canvas.DrawString(this.mMapSelectedLocation.Information, font2, brush13, (float) (((unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset) + 1f), (float) (((unknownLocYPos - (num37 * 1.5f)) + this.mMapSelectedLocation.StringYoffset) + 1f));
                        canvas.DrawString(this.mMapSelectedLocation.Information, font2, brush14, (float) ((unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset), (float) ((unknownLocYPos - (num37 * 1.5f)) + this.mMapSelectedLocation.StringYoffset));
                        if (information != this.mMapSelectedLocation.Information)
                        {
                            Font font3 = new Font(this.Font.FontFamily, this.GetFontSize(num37) * 0.75f);
                            information = information.Replace(this.mMapSelectedLocation.Information + "\r\n", "");
                            float num38 = (ef8.Width - canvas.MeasureString(information, font3).Width) / 2f;
                            float num39 = (ef8.Height - canvas.MeasureString(information, font3).Height) / 1.5f;
                            canvas.DrawString(information, font3, brush13, (float) ((((unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset) + 1f) + num38), (float) ((((unknownLocYPos - (num37 * 1.5f)) + this.mMapSelectedLocation.StringYoffset) + 1f) + num39));
                            canvas.DrawString(information, font3, brush14, (float) (((unknownLocXPos + (num37 * 3f)) + this.mMapSelectedLocation.StringXoffset) + num38), (float) (((unknownLocYPos - (num37 * 1.5f)) + this.mMapSelectedLocation.StringYoffset) + num39));
                            font3.Dispose();
                        }
                    }
                }
            }
            bool flag = false;
            float num40 = (1f * (tickCount - this.mLastTick)) / this.mSmootheMapScale;
            if (this.mMapX > this.mSmootheX)
            {
                this.mSmootheX += num40;
                if (this.mMapX < this.mSmootheX)
                {
                    this.mSmootheX = this.mMapX;
                }
                flag = true;
            }
            else if (this.mMapX < this.mSmootheX)
            {
                this.mSmootheX -= num40;
                if (this.mMapX > this.mSmootheX)
                {
                    this.mSmootheX = this.mMapX;
                }
                flag = true;
            }
            if (this.mMapY > this.mSmootheY)
            {
                this.mSmootheY += num40;
                if (this.mMapY < this.mSmootheY)
                {
                    this.mSmootheY = this.mMapY;
                }
                flag = true;
            }
            else if (this.mMapY < this.mSmootheY)
            {
                this.mSmootheY -= num40;
                if (this.mMapY > this.mSmootheY)
                {
                    this.mSmootheY = this.mMapY;
                }
                flag = true;
            }
            if (this.mMapScale < this.mSmootheMapScale)
            {
                this.mSmootheMapScale -= (num40 * this.mSmootheMapScale) / 5000f;
                if (this.mMapScale > this.mSmootheMapScale)
                {
                    this.mSmootheMapScale = this.mMapScale;
                }
                flag = true;
            }
            else if (this.mMapScale > this.mSmootheMapScale)
            {
                this.mSmootheMapScale += (num40 * this.mSmootheMapScale) / 5000f;
                if (this.mMapScale < this.mSmootheMapScale)
                {
                    this.mSmootheMapScale = this.mMapScale;
                }
                flag = true;
            }
            this.mapMoved = false;
            this.mLastTick = tickCount;
            if (flag)
            {
                base.Invalidate();
            }
        }

        public MapLocation RandomLocation(bool clearCache)
        {
            if ((this.mCacheData == null) || clearCache)
            {
                this.mCacheData = new Bitmap(base.Width, base.Height);
                Graphics canvas = Graphics.FromImage(this.mCacheData);
                this.PaintCanvas(canvas);
            }
            return this.RandomLocation(this.mCacheData);
        }

        public MapLocation RandomLocation(Bitmap bitmap)
        {
            if (this.mRenderInfo == null)
            {
                return null;
            }
            Color black = Color.Black;
            int x = 0;
            int y = 0;
            for (int i = 0; (((black.G < 50) || (black.R > 150)) || (black.B > 150)) && (i < 200); i++)
            {
                x = this.mRandom.Next(base.Width);
                y = this.mRandom.Next(base.Height);
                black = bitmap.GetPixel(x, y);
            }
            float num4 = this.mSmootheMapScale / this.mRenderInfo.ZoomThreshold;
            float num5 = this.mRenderInfo.RelativeX(this.mSmootheX) * num4;
            float num6 = this.mRenderInfo.RelativeY(this.mSmootheY) * num4;
            float num7 = (x + num5) / this.mSmootheMapScale;
            float num8 = (y + num6) / this.mSmootheMapScale;
            MapLocation location = new MapLocation();
            location.MapX = num7;
            location.MapY = num8;
            return location;
        }

        public void RedrawMap()
        {
            this.mapMoved = true;
            base.Invalidate();
        }

        private void SetTextBoxes(List<RectangleF> textBoxes)
        {
            ZoomInfo zoomLevel = this.GetZoomLevel();
            float num = this.mSmootheMapScale / zoomLevel.ZoomThreshold;
            float num2 = zoomLevel.RelativeX(this.mSmootheX) * num;
            float num3 = zoomLevel.RelativeY(this.mSmootheY) * num;
            foreach (MapLocation location in this.mNonDots)
            {
                if (!location.Visible)
                {
                    continue;
                }
                float num4 = (zoomLevel.RelativeX(location.MapX) * num) - num2;
                float num5 = (zoomLevel.RelativeY(location.MapY) * num) - num3;
                float num6 = 50f * this.mSmootheMapScale;
                if (num6 < 1f)
                {
                    num6 = 1f;
                }
                else if (num6 > 50f)
                {
                    num6 = 50f;
                }
                if (location.LocationType == LocationTypes.Flag)
                {
                    if (num6 < 4f)
                    {
                        num6 = 4f;
                    }
                    else if (num6 > 15f)
                    {
                        num6 = 15f;
                    }
                    RectangleF item = new RectangleF(num4 - (num6 * 1.125f), num5 - (num6 * 4.125f), num6 * 2.25f, num6 * 4.125f);
                    textBoxes.Add(item);
                }
                if (location.LocationType == LocationTypes.Image)
                {
                    if (num6 < 4f)
                    {
                        num6 = 4f;
                    }
                    else if (num6 > 15f)
                    {
                        num6 = 15f;
                    }
                    RectangleF ef2 = new RectangleF(num4 - (num6 * 2f), num5 - (num6 * 2f), num6 * 4f, num6 * 4f);
                    textBoxes.Add(ef2);
                }
            }
        }

        public List<string> Filters
        {
            set
            {
                this.mFilters = value;
                foreach (MapLocation location in this.mMapLocationList)
                {
                    location.Visible = !this.mFilters.Contains(location.Classification);
                }
                this.mapMoved = true;
                base.Invalidate();
            }
        }

        public float MapScale
        {
            get
            {
                return this.mMapScale;
            }
            set
            {
                this.mMapScale = value;
                this.CheckBounds();
                base.Invalidate();
                base.Update();
            }
        }

        public float MapX
        {
            get
            {
                return this.mMapX;
            }
            set
            {
                this.mMapX = value;
                this.CheckBounds();
                base.Invalidate();
                base.Update();
            }
        }

        public float MapY
        {
            get
            {
                return this.mMapY;
            }
            set
            {
                this.mMapY = value;
                this.CheckBounds();
                base.Invalidate();
                base.Update();
            }
        }

        public MapLocation SelectedLocation
        {
            get
            {
                return this.mMapSelectedLocation;
            }
        }

        [DisplayName("<LOC>ShowBorders"), Description("<LOC>"), Category("<LOC>Misc")]
        public bool ShowBorders
        {
            get
            {
                return this.mShowBorders;
            }
            set
            {
                this.mShowBorders = value;
                if (this.ShowBordersChanged != null)
                {
                    this.ShowBordersChanged(this, new PropertyChangedEventArgs("ShowBorders"));
                }
            }
        }

        public Image WatermarkLeft
        {
            get
            {
                return this.mWatermarkLeft;
            }
            set
            {
                this.mWatermarkLeft = value;
            }
        }

        public Image WatermarkRight
        {
            get
            {
                return this.mWatermarkRight;
            }
            set
            {
                this.mWatermarkRight = value;
            }
        }
    }
}

