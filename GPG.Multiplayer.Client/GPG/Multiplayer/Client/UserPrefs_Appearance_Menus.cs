namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class UserPrefs_Appearance_Menus
    {
        private Color mContextItemColor = Color.Black;
        private Font mContextItemFont = DEF_FONT;
        private int mContextItemHeight = 0x19;
        private int mContextItemWidth = 0;
        private Color mDropDownItemColor = Color.WhiteSmoke;
        private Font mDropDownItemFont = new Font(DEF_FONT, FontStyle.Bold);
        private int mDropDownItemHeight = 0x19;
        private int mDropDownItemWidth = 220;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ContextItemColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ContextItemFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ContextItemHeightChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ContextItemWidthChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DropDownItemColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DropDownItemFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DropDownItemHeightChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DropDownItemWidthChanged;

        [Category("<LOC>Context Menus"), Description("<LOC>Context menu color."), DisplayName("<LOC>Context Item Color")]
        public Color ContextItemColor
        {
            get
            {
                return this.mContextItemColor;
            }
            set
            {
                this.mContextItemColor = value;
                if (this.ContextItemColorChanged != null)
                {
                    this.ContextItemColorChanged(this, new PropertyChangedEventArgs("ContextItemColor"));
                }
            }
        }

        [Description("<LOC>Context menu font."), Category("<LOC>Context Menus"), DisplayName("<LOC>Context Item Font")]
        public Font ContextItemFont
        {
            get
            {
                return this.mContextItemFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mContextItemFont = value;
                if (this.ContextItemFontChanged != null)
                {
                    this.ContextItemFontChanged(this, new PropertyChangedEventArgs("ContextItemFont"));
                }
            }
        }

        [Category("<LOC>Context Menus"), Description("<LOC>Context menu height."), DisplayName("<LOC>Height")]
        public int ContextItemHeight
        {
            get
            {
                return this.mContextItemHeight;
            }
            set
            {
                this.mContextItemHeight = value;
                if (this.ContextItemHeightChanged != null)
                {
                    this.ContextItemHeightChanged(this, new PropertyChangedEventArgs("ContextItemHeight"));
                }
            }
        }

        [Description("<LOC>Context menu height -- set to 0 for dynamic sizing."), DisplayName("<LOC>Width"), Category("<LOC>Context Menus")]
        public int ContextItemWidth
        {
            get
            {
                return this.mContextItemWidth;
            }
            set
            {
                this.mContextItemWidth = value;
                if (this.ContextItemWidthChanged != null)
                {
                    this.ContextItemWidthChanged(this, new PropertyChangedEventArgs("ContextItemWidth"));
                }
            }
        }

        private static Font DEF_FONT
        {
            get
            {
                return UserPrefs_Appearance_Text.DEF_FONT;
            }
        }

        [Category("<LOC>Drop-down Menus"), Description("<LOC>Drop-down menu color."), DisplayName("<LOC>Drop-down Item Color")]
        public Color DropDownItemColor
        {
            get
            {
                return this.mDropDownItemColor;
            }
            set
            {
                this.mDropDownItemColor = value;
                if (this.DropDownItemColorChanged != null)
                {
                    this.DropDownItemColorChanged(this, new PropertyChangedEventArgs("DropDownItemColor"));
                }
            }
        }

        [Description("<LOC>Drop-down menu font."), Category("<LOC>Drop-down Menus"), DisplayName("<LOC>Drop-down Item Font")]
        public Font DropDownItemFont
        {
            get
            {
                return this.mDropDownItemFont;
            }
            set
            {
                float emSize = 16f;
                if (value.SizeInPoints > emSize)
                {
                    value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                }
                this.mDropDownItemFont = value;
                if (this.DropDownItemFontChanged != null)
                {
                    this.DropDownItemFontChanged(this, new PropertyChangedEventArgs("DropDownItemFont"));
                }
            }
        }

        [Category("<LOC>Drop-down Menus"), DisplayName("<LOC>Height"), Description("<LOC>Drop-down menu height.")]
        public int DropDownItemHeight
        {
            get
            {
                return this.mDropDownItemHeight;
            }
            set
            {
                this.mDropDownItemHeight = value;
                if (this.DropDownItemHeightChanged != null)
                {
                    this.DropDownItemHeightChanged(this, new PropertyChangedEventArgs("DropDownItemHeight"));
                }
            }
        }

        [Description("Drop-down menu width -- set to 0 for dynamic sizing."), Category("<LOC>Drop-down Menus"), DisplayName("<LOC>Width")]
        public int DropDownItemWidth
        {
            get
            {
                return this.mDropDownItemWidth;
            }
            set
            {
                this.mDropDownItemWidth = value;
                if (this.DropDownItemWidthChanged != null)
                {
                    this.DropDownItemWidthChanged(this, new PropertyChangedEventArgs("DropDownItemWidth"));
                }
            }
        }
    }
}

