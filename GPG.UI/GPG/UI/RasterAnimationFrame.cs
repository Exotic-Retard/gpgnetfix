namespace GPG.UI
{
    using System;
    using System.Drawing;

    [Serializable]
    public class RasterAnimationFrame
    {
        private RasterAnimation mAnimation;
        private float mDuration;
        private Bitmap mImage;
        private string mImagePath;
        private RasterAnimationFrame mNextFrame;
        private RasterAnimationFrame mPreviousFrame;

        public RasterAnimationFrame(RasterAnimation animation)
        {
            this.mAnimation = animation;
        }

        public RasterAnimationFrame(RasterAnimation animation, Bitmap image)
        {
            this.mAnimation = animation;
            this.Image = image;
        }

        public RasterAnimationFrame(RasterAnimation animation, string imgPath)
        {
            this.mAnimation = animation;
            this.ImagePath = imgPath;
        }

        public RasterAnimation Animation
        {
            get
            {
                return this.mAnimation;
            }
        }

        public float Duration
        {
            get
            {
                return this.mDuration;
            }
            set
            {
                this.mDuration = value;
            }
        }

        public Bitmap Image
        {
            get
            {
                if ((this.mImage == null) && (this.ImagePath != null))
                {
                    this.mImage = System.Drawing.Image.FromFile(this.ImagePath) as Bitmap;
                    Bitmap bitmap = this.mImage.Clone() as Bitmap;
                    this.mImage.Dispose();
                    this.mImage = bitmap;
                }
                return this.mImage;
            }
            set
            {
                this.mImage = value;
            }
        }

        public string ImagePath
        {
            get
            {
                return this.mImagePath;
            }
            set
            {
                this.mImagePath = value;
            }
        }

        public bool IsReady
        {
            get
            {
                return (this.Image != null);
            }
        }

        public RasterAnimationFrame NextFrame
        {
            get
            {
                return this.mNextFrame;
            }
            internal set
            {
                this.mNextFrame = value;
            }
        }

        public RasterAnimationFrame PreviousFrame
        {
            get
            {
                return this.mPreviousFrame;
            }
            internal set
            {
                this.mPreviousFrame = value;
            }
        }
    }
}

