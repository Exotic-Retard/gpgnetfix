namespace GPG.Multiplayer.Client
{
    using GPG;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class UserPrefs_Appearance
    {
        private bool mFadeWindows = true;
        private UserPrefs_Appearance_Layout mLayout = new UserPrefs_Appearance_Layout();
        private UserPrefs_Appearance_Menus mMenus = new UserPrefs_Appearance_Menus();
        private Point mStartLocation = new Point(40, 40);
        private Size mStartSize = new Size(0x335, 0x266);
        private UserPrefs_Appearance_Text mText = new UserPrefs_Appearance_Text();
        private int mWindowMoveTransparency = 0x19;
        private UserPrefs RootPrefs = null;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FadeWindowsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StartLocationChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StartSizeChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler WindowMoveTransparencyChanged;

        public UserPrefs_Appearance(UserPrefs root)
        {
            this.RootPrefs = root;
        }

        [Browsable(false), OptionsRoot("<LOC>Chat")]
        public UserPrefs_Chat_Appearance Chat
        {
            get
            {
                if (this.RootPrefs != null)
                {
                    return this.RootPrefs.Chat.Appearance;
                }
                return null;
            }
        }

        [DisplayName("<LOC>Fade Windows"), Category("<LOC>Misc"), Description("<LOC>If true, dialogs and windows will fade in and out.")]
        private bool FadeWindows
        {
            get
            {
                return this.mFadeWindows;
            }
            set
            {
                this.mFadeWindows = value;
                if (this.FadeWindowsChanged != null)
                {
                    this.FadeWindowsChanged(this, new PropertyChangedEventArgs("FadeWindows"));
                }
            }
        }

        [Browsable(false), OptionsRoot("<LOC>Layout")]
        public UserPrefs_Appearance_Layout Layout
        {
            get
            {
                if (this.mLayout == null)
                {
                    this.mLayout = new UserPrefs_Appearance_Layout();
                }
                return this.mLayout;
            }
        }

        [Browsable(false), OptionsRoot("<LOC>Menus")]
        public UserPrefs_Appearance_Menus Menus
        {
            get
            {
                if (this.mMenus == null)
                {
                    this.mMenus = new UserPrefs_Appearance_Menus();
                }
                return this.mMenus;
            }
        }

        [DisplayName("<LOC>Initial Location"), Category("<LOC>Misc"), Description("")]
        public Point StartLocation
        {
            get
            {
                return this.mStartLocation;
            }
            set
            {
                this.mStartLocation = value;
                if (this.StartLocationChanged != null)
                {
                    this.StartLocationChanged(this, new PropertyChangedEventArgs("StartLocation"));
                }
            }
        }

        [Description(""), Category("<LOC>Misc"), DisplayName("<LOC>Initial Size")]
        public Size StartSize
        {
            get
            {
                return this.mStartSize;
            }
            set
            {
                this.mStartSize = value;
                if (this.StartSizeChanged != null)
                {
                    this.StartSizeChanged(this, new PropertyChangedEventArgs("StartSize"));
                }
            }
        }

        [Browsable(false), OptionsRoot("<LOC>Text")]
        public UserPrefs_Appearance_Text Text
        {
            get
            {
                if (this.mText == null)
                {
                    this.mText = new UserPrefs_Appearance_Text();
                }
                return this.mText;
            }
        }

        internal double WindowMoveOpacity
        {
            get
            {
                double num = ((double) this.WindowMoveTransparency) / 100.0;
                if (num >= 1.0)
                {
                    num = 0.99;
                }
                else if (num < 0.0)
                {
                    num = 0.0;
                }
                return (0.99 - num);
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Window Move Transparency"), Description("<LOC>The percentage amout that windows are transparent when dragging them around.")]
        private int WindowMoveTransparency
        {
            get
            {
                return this.mWindowMoveTransparency;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 100)
                {
                    value = 100;
                }
                this.mWindowMoveTransparency = value;
                if (this.WindowMoveTransparencyChanged != null)
                {
                    this.WindowMoveTransparencyChanged(this, new PropertyChangedEventArgs("WindowMoveTransparency"));
                }
            }
        }
    }
}

