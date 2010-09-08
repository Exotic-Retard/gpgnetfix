namespace GPG.UI
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class MapLocation
    {
        public string Classification = "";
        public string DataType = "";
        public bool DrawImageBorder;
        public string ExtendedInformation = "";
        public List<Image> Images = new List<Image>();
        public string Information = "";
        public LocationTypes LocationType = LocationTypes.Dot;
        private bool mAnimate;
        public Color MapColor = Color.Yellow;
        private int mCreateTick = Environment.TickCount;
        private bool mHideLocation;
        private int mIndex = -1;
        private float mLatitude;
        private float mLongitude;
        private Image mMapImage;
        private float mMapX = -1f;
        private float mMapY = -1f;
        private int mStartTick;
        public float Strength = 1f;
        public float StringHeight;
        public RectangleF StringRect = new RectangleF();
        public float StringWidth;
        public float StringXoffset;
        public float StringYoffset;
        public object Tag;
        public float UnknownLocXPos;
        public float UnknownLocYPos;
        public bool Visible = true;

        public string GetSingleInfo()
        {
            if (this.Information != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    string[] strArray = this.Information.Replace("\r", "").Split("\n".ToCharArray());
                    if (this.mIndex < strArray.Length)
                    {
                        this.mIndex++;
                    }
                    else
                    {
                        this.mIndex = 0;
                    }
                    if (strArray[this.mIndex].Trim() != "")
                    {
                        return strArray[this.mIndex];
                    }
                }
            }
            return this.Information;
        }

        public bool Animate
        {
            get
            {
                return this.mAnimate;
            }
            set
            {
                this.mAnimate = value;
                if (this.mAnimate)
                {
                    this.mStartTick = Environment.TickCount;
                }
            }
        }

        public int CreateTick
        {
            get
            {
                return this.mCreateTick;
            }
        }

        public bool HideLocation
        {
            get
            {
                return this.mHideLocation;
            }
            set
            {
                this.mHideLocation = value;
            }
        }

        public float Latitude
        {
            get
            {
                if (this.mHideLocation)
                {
                    return 0f;
                }
                return this.mLatitude;
            }
            set
            {
                this.mMapY = -1f;
                this.mLatitude = value;
            }
        }

        public float Longitude
        {
            get
            {
                if (this.mHideLocation)
                {
                    return 0f;
                }
                return this.mLongitude;
            }
            set
            {
                this.mMapX = -1f;
                this.mLongitude = value;
            }
        }

        public Image MapImage
        {
            get
            {
                if (this.Images.Count <= 0)
                {
                    return this.mMapImage;
                }
                if (!this.mAnimate)
                {
                    return this.Images[this.Images.Count - 1];
                }
                int num = Environment.TickCount - this.mStartTick;
                int num2 = num / 0x3e8;
                if (num2 < (this.Images.Count - 1))
                {
                    Image original = this.Images[num2];
                    Image image2 = this.Images[num2 + 1];
                    float amount = ((float) (num % 0x3e8)) / 1000f;
                    float num4 = 1f - amount;
                    amount += (0.5f - Math.Abs((float) (0.5f - amount))) / 1f;
                    num4 += (0.5f - Math.Abs((float) (0.5f - num4))) / 1f;
                    Bitmap image = new Bitmap(original.Size.Width, original.Size.Height);
                    Graphics graphics = Graphics.FromImage(image);
                    graphics.DrawImage(DrawUtil.GetTransparentImage(num4, original), 0, 0);
                    graphics.DrawImage(DrawUtil.GetTransparentImage(amount, image2), 0, 0);
                    graphics.Dispose();
                    return image;
                }
                if (num2 < this.Images.Count)
                {
                    return this.Images[this.Images.Count - 1];
                }
                if (num2 < (this.Images.Count + 100))
                {
                    Image image3 = this.Images[this.Images.Count - 1];
                    Bitmap bitmap2 = new Bitmap(image3.Size.Width, image3.Size.Height);
                    float num5 = (100f - (num2 - this.Images.Count)) / 100f;
                    this.MapColor = Color.FromArgb((int) (255f * num5), this.MapColor);
                    Graphics graphics2 = Graphics.FromImage(bitmap2);
                    graphics2.DrawImage(DrawUtil.GetTransparentImage(num5, image3), 0, 0);
                    graphics2.Dispose();
                    return bitmap2;
                }
                Image image4 = this.Images[this.Images.Count - 1];
                return new Bitmap(image4.Size.Width, image4.Size.Height);
            }
            set
            {
                this.mMapImage = value;
            }
        }

        public float MapX
        {
            get
            {
                if (this.mMapX == -1f)
                {
                    this.mMapX = ((this.mLongitude + 180f) / 360f) * 21600f;
                }
                return this.mMapX;
            }
            set
            {
                this.mLongitude = ((value * 360f) / 21600f) - 180f;
                this.mMapX = -1f;
            }
        }

        public float MapY
        {
            get
            {
                if (this.mMapY == -1f)
                {
                    this.mMapY = ((90f - this.mLatitude) / 180f) * 10800f;
                }
                return this.mMapY;
            }
            set
            {
                this.mLatitude = 90f - ((value * 180f) / 10800f);
                this.mMapY = -1f;
            }
        }
    }
}

