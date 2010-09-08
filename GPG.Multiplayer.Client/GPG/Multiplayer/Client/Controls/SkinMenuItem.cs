namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class SkinMenuItem : MenuItem, ISkinControl, IStyledControl
    {
        private bool ClearGraphics;
        private IContainer components;
        private string mControlState;
        private SkinDropDown mDropDown;
        private Image mIcon;
        protected Image mLeftImage;
        protected Image mMidImage;
        protected Image mRightImage;
        private string mSkinBasePath;
        private Dictionary<string, SkinControlState> SkinStates;

        public SkinMenuItem(Image icon)
        {
            this.components = null;
            this.mControlState = null;
            this.SkinStates = new Dictionary<string, SkinControlState>();
            this.ClearGraphics = true;
            this.mLeftImage = null;
            this.mMidImage = null;
            this.mRightImage = null;
            this.mIcon = null;
            this.mSkinBasePath = @"Controls\Menus\DropDownItem";
            this.InitializeComponent();
            this.mIcon = icon;
            base.OwnerDraw = true;
        }

        public SkinMenuItem(string text)
        {
            this.components = null;
            this.mControlState = null;
            this.SkinStates = new Dictionary<string, SkinControlState>();
            this.ClearGraphics = true;
            this.mLeftImage = null;
            this.mMidImage = null;
            this.mRightImage = null;
            this.mIcon = null;
            this.mSkinBasePath = @"Controls\Menus\DropDownItem";
            this.InitializeComponent();
            base.Text = Loc.Get(text);
            base.OwnerDraw = true;
        }

        public SkinMenuItem(SkinDropDown dropDown, Image icon)
        {
            this.components = null;
            this.mControlState = null;
            this.SkinStates = new Dictionary<string, SkinControlState>();
            this.ClearGraphics = true;
            this.mLeftImage = null;
            this.mMidImage = null;
            this.mRightImage = null;
            this.mIcon = null;
            this.mSkinBasePath = @"Controls\Menus\DropDownItem";
            this.InitializeComponent();
            this.mDropDown = dropDown;
            this.mIcon = icon;
            base.OwnerDraw = true;
        }

        public SkinMenuItem(SkinDropDown dropDown, string text)
        {
            this.components = null;
            this.mControlState = null;
            this.SkinStates = new Dictionary<string, SkinControlState>();
            this.ClearGraphics = true;
            this.mLeftImage = null;
            this.mMidImage = null;
            this.mRightImage = null;
            this.mIcon = null;
            this.mSkinBasePath = @"Controls\Menus\DropDownItem";
            this.InitializeComponent();
            this.mDropDown = dropDown;
            base.Text = Loc.Get(text);
            base.OwnerDraw = true;
        }

        public SkinMenuItem(string text, Image icon)
        {
            this.components = null;
            this.mControlState = null;
            this.SkinStates = new Dictionary<string, SkinControlState>();
            this.ClearGraphics = true;
            this.mLeftImage = null;
            this.mMidImage = null;
            this.mRightImage = null;
            this.mIcon = null;
            this.mSkinBasePath = @"Controls\Menus\DropDownItem";
            this.InitializeComponent();
            base.Text = Loc.Get(text);
            this.mIcon = icon;
            base.OwnerDraw = true;
        }

        public SkinMenuItem(SkinDropDown dropDown, string text, Image icon)
        {
            this.components = null;
            this.mControlState = null;
            this.SkinStates = new Dictionary<string, SkinControlState>();
            this.ClearGraphics = true;
            this.mLeftImage = null;
            this.mMidImage = null;
            this.mRightImage = null;
            this.mIcon = null;
            this.mSkinBasePath = @"Controls\Menus\DropDownItem";
            this.InitializeComponent();
            base.Text = Loc.Get(text);
            this.mDropDown = dropDown;
            this.mIcon = icon;
            base.OwnerDraw = true;
        }

        public void ClearSkins()
        {
            this.SkinStates.Clear();
            this.ClearGraphics = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public override bool Equals(object obj)
        {
            if (obj is MenuItem)
            {
                return (obj.GetHashCode() == this.GetHashCode());
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.Text.GetHashCode();
        }

        public bool HasState(string state)
        {
            if (this.SkinStates.ContainsKey(state))
            {
                return true;
            }
            if (this.DrawEdges)
            {
                return ((SkinManager.ImageExists(string.Format(@"{0}\left{1}.png", this.SkinBasePath, state)) && SkinManager.ImageExists(string.Format(@"{0}\mid{1}.png", this.SkinBasePath, state))) && SkinManager.ImageExists(string.Format(@"{0}\right{1}.png", this.SkinBasePath, state)));
            }
            return SkinManager.ImageExists(string.Format(@"{0}\mid{1}.png", this.SkinBasePath, state));
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public void LoadState()
        {
            this.LoadState("");
        }

        public virtual void LoadState(string state)
        {
            if (this.ControlState != state.ToLower())
            {
                if ((state != null) && (state.Length > 0))
                {
                    state = "_" + state;
                }
                if (this.HasState(state))
                {
                    if (this.SkinStates.ContainsKey(state))
                    {
                        SkinControlState state2 = this.SkinStates[state];
                        this.mLeftImage = state2.LeftImage;
                        this.mMidImage = state2.MidImage;
                        this.mRightImage = state2.RightImage;
                    }
                    else
                    {
                        Image image = null;
                        Image image2 = null;
                        Image image3 = null;
                        SkinManager.TryGetImage(string.Format(@"{0}\mid{1}.png", this.SkinBasePath, state), out image2);
                        if (this.DrawEdges)
                        {
                            SkinManager.TryGetImage(string.Format(@"{0}\left{1}.png", this.SkinBasePath, state), out image);
                            SkinManager.TryGetImage(string.Format(@"{0}\right{1}.png", this.SkinBasePath, state), out image3);
                        }
                        if (image != null)
                        {
                            this.mLeftImage = image;
                        }
                        if (image2 != null)
                        {
                            this.mMidImage = image2;
                        }
                        if (image3 != null)
                        {
                            this.mRightImage = image3;
                        }
                        if (this.MidImage.Height != this.Height)
                        {
                            this.mMidImage = DrawUtil.ResizeImage(this.MidImage, this.MidImage.Width, this.Height);
                        }
                        this.SkinStates[state] = new SkinControlState(this.SkinBasePath, this.LeftImage, this.MidImage, this.RightImage);
                    }
                    this.mControlState = state.ToLower().TrimStart("_".ToCharArray());
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (this.DropDown != null)
            {
                this.DropDown.Icon = this.Icon;
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            try
            {
                TextureBrush brush2;
                int num;
                if (this.ClearGraphics)
                {
                    if (e.Index == 0)
                    {
                        e.Graphics.Clear(Program.Settings.StylePreferences.MasterBackColor);
                    }
                    using (Brush brush = new SolidBrush(Program.Settings.StylePreferences.MasterBackColor))
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                    this.ClearGraphics = false;
                }
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    this.LoadState("over");
                }
                else
                {
                    this.LoadState();
                }
                if (this.DrawEdges)
                {
                    e.Graphics.DrawImage(this.mLeftImage, 0, 0, this.mLeftImage.Width, this.Height);
                    switch (this.HorizontalScalingMode)
                    {
                        case ScalingModes.Tile:
                            using (brush2 = new TextureBrush(this.mMidImage, WrapMode.Tile))
                            {
                                e.Graphics.FillRectangle(brush2, new Rectangle(this.mLeftImage.Width, 0, this.Width - (this.mLeftImage.Width + this.mRightImage.Width), this.Height));
                            }
                            break;

                        case ScalingModes.Stretch:
                            e.Graphics.DrawImage(this.MidImage, this.mLeftImage.Width, 0, this.Width - (this.mLeftImage.Width + this.mRightImage.Width), this.Height);
                            break;
                    }
                    e.Graphics.DrawImage(this.mRightImage, this.Width - this.mRightImage.Width, 0, this.mRightImage.Width, this.Height);
                }
                else
                {
                    switch (this.HorizontalScalingMode)
                    {
                        case ScalingModes.Tile:
                            using (brush2 = new TextureBrush(this.mMidImage, WrapMode.Tile))
                            {
                                e.Graphics.FillRectangle(brush2, e.Bounds);
                            }
                            goto Label_0264;

                        case ScalingModes.Stretch:
                            e.Graphics.DrawImage(this.MidImage, e.Bounds);
                            goto Label_0264;
                    }
                }
            Label_0264:
                num = 8;
                int childX = 0;
                if (this.Icon != null)
                {
                    childX = this.Icon.Width + (num * 2);
                    e.Graphics.DrawImage(this.Icon, num, e.Bounds.Top + DrawUtil.CenterV(this.Height, this.Icon.Height, num).Y, this.Icon.Width, this.Icon.Height);
                }
                else
                {
                    childX = num;
                }
                Font dropDownItemFont = Program.Settings.Appearance.Menus.DropDownItemFont;
                using (Brush brush3 = new SolidBrush(Program.Settings.Appearance.Menus.DropDownItemColor))
                {
                    e.Graphics.DrawString(base.Text, dropDownItemFont, brush3, (float) childX, (float) (e.Bounds.Top + DrawUtil.CenterV(this.Height, Convert.ToInt32(DrawUtil.MeasureString(e.Graphics, base.Text, dropDownItemFont).Height), childX).Y));
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            e.ItemHeight = this.Height;
            e.ItemWidth = this.Width;
        }

        public override string ToString()
        {
            return base.Text;
        }

        public bool AutoStyle
        {
            get
            {
                return false;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string ControlState
        {
            get
            {
                return this.mControlState;
            }
        }

        [Browsable(false)]
        public bool DrawEdges
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public SkinDropDown DropDown
        {
            get
            {
                return this.mDropDown;
            }
            set
            {
                this.mDropDown = value;
            }
        }

        public int Height
        {
            get
            {
                return Program.Settings.Appearance.Menus.DropDownItemHeight;
            }
        }

        [Browsable(false)]
        public virtual ScalingModes HorizontalScalingMode
        {
            get
            {
                return ScalingModes.Stretch;
            }
            set
            {
            }
        }

        public Image Icon
        {
            get
            {
                return this.mIcon;
            }
            set
            {
                this.mIcon = value;
            }
        }

        public bool IsStyled
        {
            get
            {
                return true;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Image LeftImage
        {
            get
            {
                return this.mLeftImage;
            }
        }

        public Image MidImage
        {
            get
            {
                return this.mMidImage;
            }
        }

        public Image RightImage
        {
            get
            {
                return this.mRightImage;
            }
        }

        public string SkinBasePath
        {
            get
            {
                return this.mSkinBasePath;
            }
            set
            {
                this.mSkinBasePath = value;
            }
        }

        public bool SkinLoaded
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool UseDefaultStyle
        {
            get
            {
                return false;
            }
        }

        public int Width
        {
            get
            {
                return Program.Settings.Appearance.Menus.DropDownItemWidth;
            }
        }
    }
}

