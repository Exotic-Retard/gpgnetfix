namespace GPG.Multiplayer.Client.Controls
{
    using System;
    using System.Drawing;

    public class SkinControlState
    {
        protected Image mLeftImage = null;
        protected Image mMidImage = null;
        protected Image mRightImage = null;
        private string mSkinPath;

        public SkinControlState(string skinPath, Image left, Image mid, Image right)
        {
            this.mSkinPath = skinPath;
            this.mLeftImage = left;
            this.mMidImage = mid;
            this.mRightImage = right;
        }

        public Image LeftImage
        {
            get
            {
                return this.mLeftImage;
            }
            set
            {
                this.mLeftImage = value;
            }
        }

        public Image MidImage
        {
            get
            {
                return this.mMidImage;
            }
            set
            {
                this.mMidImage = value;
            }
        }

        public Image RightImage
        {
            get
            {
                return this.mRightImage;
            }
            set
            {
                this.mRightImage = value;
            }
        }

        public string SkinPath
        {
            get
            {
                return this.mSkinPath;
            }
        }
    }
}

