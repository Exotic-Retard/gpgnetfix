namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class UserPrefs_Chat_Appearance_PrivateChat
    {
        private Color mOtherColor = Color.White;
        private Font mOtherFont = DEF_FONT;
        private Color mSelfColor = Color.FromArgb(0xff, 0x66, 0);
        private Font mSelfFont = DEF_FONT;

        [field: NonSerialized]
        public event PropertyChangedEventHandler AppearanceChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler OtherColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler OtherFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SelfColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SelfFontChanged;

        private static Font DEF_FONT
        {
            get
            {
                return UserPrefs_Appearance_Text.DEF_FONT;
            }
        }

        [DisplayName("<LOC>Other Player's Color"), Category("<LOC>Colors"), Description("<LOC>The text color of the player with whom you are private messaging.")]
        public Color OtherColor
        {
            get
            {
                return this.mOtherColor;
            }
            set
            {
                this.mOtherColor = value;
                if (this.mOtherColorChanged != null)
                {
                    this.mOtherColorChanged(this, new PropertyChangedEventArgs("OtherColor"));
                }
                if (this.mAppearanceChanged != null)
                {
                    this.mAppearanceChanged(this, new PropertyChangedEventArgs("OtherColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), DisplayName("<LOC>Other Player's Font"), Description("<LOC>The text font of the player with whom you are private messaging.")]
        public Font OtherFont
        {
            get
            {
                return this.mOtherFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mOtherFont = value;
                if (this.mOtherFontChanged != null)
                {
                    this.mOtherFontChanged(this, new PropertyChangedEventArgs("OtherFont"));
                }
                if (this.mAppearanceChanged != null)
                {
                    this.mAppearanceChanged(this, new PropertyChangedEventArgs("OtherFont"));
                }
            }
        }

        [Category("<LOC>Colors"), DisplayName("<LOC>Self Color"), Description("<LOC>The color of your messages when private messaging.")]
        public Color SelfColor
        {
            get
            {
                return this.mSelfColor;
            }
            set
            {
                this.mSelfColor = value;
                if (this.mSelfColorChanged != null)
                {
                    this.mSelfColorChanged(this, new PropertyChangedEventArgs("SelfColor"));
                }
                if (this.mAppearanceChanged != null)
                {
                    this.mAppearanceChanged(this, new PropertyChangedEventArgs("SelfColor"));
                }
            }
        }

        [DisplayName("<LOC>Self Font"), Category("<LOC>Fonts"), Description("<LOC>The font of your messages when private messaging.")]
        public Font SelfFont
        {
            get
            {
                return this.mSelfFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mSelfFont = value;
                if (this.mSelfFontChanged != null)
                {
                    this.mSelfFontChanged(this, new PropertyChangedEventArgs("SelfFont"));
                }
                if (this.mAppearanceChanged != null)
                {
                    this.mAppearanceChanged(this, new PropertyChangedEventArgs("SelfFont"));
                }
            }
        }
    }
}

