namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class UserPrefs_Appearance_Text
    {
        public static readonly Font DEF_FONT = new Font("Verdana", 8f);
        public const float MAX_FONT = 16f;
        private Color mDescriptorColor = Color.FromArgb(0xff, 0x66, 0);
        private Font mDescriptorFont = new Font("Arial", 8f, FontStyle.Italic);
        private Color mErrorColor = Color.Red;
        private Font mErrorFont = DEF_FONT;
        private Color mInfoColor = Color.FromArgb(0xff, 0x66, 0);
        private Font mInfoFont = DEF_FONT;
        private Color mLinkColor = Color.FromArgb(0xff, 0x66, 0);
        private Font mLinkFont = new Font(DEF_FONT, FontStyle.Bold);
        private Color mMasterColor = Color.White;
        private Font mMasterFont = DEF_FONT;
        private Color mStatusColor = Color.Yellow;
        private Font mStatusFont = DEF_FONT;
        private Color mTitleColor = Color.White;
        private Font mTitleFont = new Font(DEF_FONT, FontStyle.Bold);

        [field: NonSerialized]
        public event PropertyChangedEventHandler DescriptorColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DescriptorFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ErrorColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ErrorFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler InfoColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler InfoFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LinkColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LinkFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MasterColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MasterFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StatusColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StatusFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler TitleColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler TitleFontChanged;

        [Category("<LOC>Colors"), Description("<LOC>"), DisplayName("<LOC>Description Color")]
        public Color DescriptorColor
        {
            get
            {
                return this.mDescriptorColor;
            }
            set
            {
                this.mDescriptorColor = value;
                if (this.mDescriptorColorChanged != null)
                {
                    this.mDescriptorColorChanged(this, new PropertyChangedEventArgs("DescriptorColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>Description Font")]
        public Font DescriptorFont
        {
            get
            {
                return this.mDescriptorFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mDescriptorFont = value;
                if (this.mDescriptorFontChanged != null)
                {
                    this.mDescriptorFontChanged(this, new PropertyChangedEventArgs("DescriptorFont"));
                }
            }
        }

        [DisplayName("<LOC>Error Color"), Description("<LOC>"), Category("<LOC>Colors")]
        public Color ErrorColor
        {
            get
            {
                return this.mErrorColor;
            }
            set
            {
                this.mErrorColor = value;
                if (this.mErrorColorChanged != null)
                {
                    this.mErrorColorChanged(this, new PropertyChangedEventArgs("ErrorColor"));
                }
            }
        }

        [DisplayName("<LOC>Error Font"), Description("<LOC>"), Category("<LOC>Fonts")]
        public Font ErrorFont
        {
            get
            {
                return this.mErrorFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mErrorFont = value;
                if (this.mErrorFontChanged != null)
                {
                    this.mErrorFontChanged(this, new PropertyChangedEventArgs("ErrorFont"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Colors"), DisplayName("<LOC>Info Color")]
        public Color InfoColor
        {
            get
            {
                return this.mInfoColor;
            }
            set
            {
                this.mInfoColor = value;
                if (this.mInfoColorChanged != null)
                {
                    this.mInfoColorChanged(this, new PropertyChangedEventArgs("InfoColor"));
                }
            }
        }

        [DisplayName("<LOC>Info Font"), Description("<LOC>"), Category("<LOC>Fonts")]
        public Font InfoFont
        {
            get
            {
                return this.mInfoFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mInfoFont = value;
                if (this.mInfoFontChanged != null)
                {
                    this.mInfoFontChanged(this, new PropertyChangedEventArgs("InfoFont"));
                }
            }
        }

        [Category("<LOC>Colors"), DisplayName("<LOC>Link Color"), Description("<LOC>")]
        public Color LinkColor
        {
            get
            {
                return this.mLinkColor;
            }
            set
            {
                this.mLinkColor = value;
                if (this.mLinkColorChanged != null)
                {
                    this.mLinkColorChanged(this, new PropertyChangedEventArgs("LinkColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>Link Font")]
        public Font LinkFont
        {
            get
            {
                return this.mLinkFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mLinkFont = value;
                if (this.mLinkFontChanged != null)
                {
                    this.mLinkFontChanged(this, new PropertyChangedEventArgs("LinkFont"));
                }
            }
        }

        [DisplayName("<LOC>Master Color"), Description("<LOC>"), Category("<LOC>Colors")]
        public Color MasterColor
        {
            get
            {
                return this.mMasterColor;
            }
            set
            {
                this.mMasterColor = value;
                if (this.mMasterColorChanged != null)
                {
                    this.mMasterColorChanged(this, new PropertyChangedEventArgs("MasterColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>Master Font")]
        public Font MasterFont
        {
            get
            {
                return this.mMasterFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mMasterFont = value;
                if (this.mMasterFontChanged != null)
                {
                    this.mMasterFontChanged(this, new PropertyChangedEventArgs("MasterFont"));
                }
            }
        }

        [Category("<LOC>Colors"), DisplayName("<LOC>Status Color"), Description("<LOC>")]
        public Color StatusColor
        {
            get
            {
                return this.mStatusColor;
            }
            set
            {
                this.mStatusColor = value;
                if (this.mStatusColorChanged != null)
                {
                    this.mStatusColorChanged(this, new PropertyChangedEventArgs("StatusColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), DisplayName("<LOC>Status Font"), Description("<LOC>")]
        public Font StatusFont
        {
            get
            {
                return this.mStatusFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mStatusFont = value;
                if (this.mStatusFontChanged != null)
                {
                    this.mStatusFontChanged(this, new PropertyChangedEventArgs("StatusFont"));
                }
            }
        }

        [Description("<LOC>"), DisplayName("<LOC>Title Color"), Category("<LOC>Colors")]
        public Color TitleColor
        {
            get
            {
                return this.mTitleColor;
            }
            set
            {
                this.mTitleColor = value;
                if (this.mTitleColorChanged != null)
                {
                    this.mTitleColorChanged(this, new PropertyChangedEventArgs("TitleColor"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>Title Font")]
        public Font TitleFont
        {
            get
            {
                return this.mTitleFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mTitleFont = value;
                if (this.mTitleFontChanged != null)
                {
                    this.mTitleFontChanged(this, new PropertyChangedEventArgs("TitleFont"));
                }
            }
        }
    }
}

