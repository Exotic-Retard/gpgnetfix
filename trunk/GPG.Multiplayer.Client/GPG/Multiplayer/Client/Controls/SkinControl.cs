namespace GPG.Multiplayer.Client.Controls
{
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

    public class SkinControl : Panel, ISkinControl, IStyledControl
    {
        private bool mAutoStyle = true;
        private string mControlState = null;
        private bool mDrawEdges = true;
        private ScalingModes mHorizontalScalingMode = ScalingModes.Tile;
        private bool mIsPainted = false;
        private bool mIsStyled = false;
        protected Image mLeftImage = null;
        protected Image mMidImage = null;
        protected Image mRightImage = null;
        private string mSkinBasePath = null;
        private ContentAlignment mTextAlign = ContentAlignment.MiddleLeft;
        private Padding mTextPadding = new Padding(0);
        private Dictionary<string, SkinControlState> SkinStates = new Dictionary<string, SkinControlState>();
        private SizeF TextSize = new SizeF();

        protected SkinControl(string skinBasePath)
        {
            this.Font = Program.Settings.Appearance.Text.MasterFont;
            this.ForeColor = Program.Settings.Appearance.Text.MasterColor;
            base.ResizeRedraw = true;
            this.mSkinBasePath = skinBasePath;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.UpdateStyles();
            this.BackColor = Color.Transparent;
        }

        public void ClearSkins()
        {
            this.SkinStates.Clear();
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

        protected virtual void LoadSkin()
        {
            this.LoadState();
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
                        if (this.MidImage.Height != base.Height)
                        {
                            this.mMidImage = DrawUtil.ResizeImage(this.MidImage, this.MidImage.Width, base.Height);
                        }
                        this.SkinStates[state] = new SkinControlState(this.SkinBasePath, this.LeftImage, this.MidImage, this.RightImage);
                    }
                    this.mControlState = state.ToLower().TrimStart("_".ToCharArray());
                    this.Refresh();
                }
                else if ((state != null) && (state.Length > 0))
                {
                    this.LoadState();
                }
            }
        }

        public void MakeEdgesTransparent()
        {
            if (this.DrawEdges)
            {
                DrawUtil.MakeTranparentHorizontal(this, this.LeftImage, this.RightImage);
            }
            else
            {
                DrawUtil.MakeTranparentHorizontal(this, this.MidImage);
            }
        }

        protected virtual void OnAutoStyled()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SizeF ef;
            if (!(!this.AutoStyle || this.mIsPainted))
            {
                Program.Settings.StylePreferences.StyleControl(this);
                this.mIsPainted = true;
                this.mIsStyled = true;
                this.OnAutoStyled();
            }
            if (!base.DesignMode && !this.SkinLoaded)
            {
                base.OnPaint(e);
                return;
            }
            if (base.DesignMode)
            {
                using (Brush brush = new SolidBrush(Color.DarkGray))
                {
                    e.Graphics.FillRectangle(brush, base.ClientRectangle);
                }
            }
            else
            {
                TextureBrush brush2;
                Point point;
                if (!this.DrawEdges)
                {
                    switch (this.HorizontalScalingMode)
                    {
                        case ScalingModes.None:
                            e.Graphics.DrawImage(this.MidImage, 0, 0, this.MidImage.Size.Width, this.MidImage.Size.Height);
                            goto Label_03D8;

                        case ScalingModes.Tile:
                            using (brush2 = new TextureBrush(this.mMidImage, WrapMode.Tile))
                            {
                                e.Graphics.FillRectangle(brush2, base.ClientRectangle);
                            }
                            goto Label_03D8;

                        case ScalingModes.Stretch:
                            e.Graphics.DrawImage(this.MidImage, base.ClientRectangle);
                            goto Label_03D8;

                        case ScalingModes.Center:
                            point = DrawUtil.Center(base.ClientRectangle, this.MidImage.Size);
                            e.Graphics.DrawImage(this.MidImage, point.X, point.Y, this.MidImage.Size.Width, this.MidImage.Size.Height);
                            goto Label_03D8;
                    }
                }
                else
                {
                    if (this.mLeftImage != null)
                    {
                        e.Graphics.DrawImage(this.mLeftImage, 0, 0, this.mLeftImage.Width, base.Height);
                    }
                    switch (this.HorizontalScalingMode)
                    {
                        case ScalingModes.None:
                            e.Graphics.DrawImage(this.MidImage, 0, 0, this.MidImage.Size.Width, this.MidImage.Size.Height);
                            break;

                        case ScalingModes.Tile:
                            using (brush2 = new TextureBrush(this.mMidImage, WrapMode.Tile))
                            {
                                e.Graphics.FillRectangle(brush2, new Rectangle(this.mLeftImage.Width, 0, base.Width - (this.mLeftImage.Width + this.mRightImage.Width), base.Height));
                            }
                            break;

                        case ScalingModes.Stretch:
                            e.Graphics.DrawImage(this.MidImage, this.mLeftImage.Width, 0, base.Width - (this.mLeftImage.Width + this.mRightImage.Width), base.Height);
                            break;

                        case ScalingModes.Center:
                            point = DrawUtil.Center(base.ClientRectangle, this.MidImage.Size);
                            e.Graphics.DrawImage(this.MidImage, point.X, point.Y, this.MidImage.Size.Width, this.MidImage.Size.Height);
                            break;
                    }
                    if (this.mRightImage != null)
                    {
                        e.Graphics.DrawImage(this.mRightImage, base.Width - this.mRightImage.Width, 0, this.mRightImage.Width, base.Height);
                    }
                }
            }
        Label_03D8:
            ef = DrawUtil.MeasureString(e.Graphics, this.Text, this.Font);
            int n = Convert.ToInt32(ef.Width) + 4;
            int num2 = Convert.ToInt32(ef.Height);
            Point point2 = new Point();
            switch (this.TextAlign)
            {
                case ContentAlignment.TopLeft:
                    point2 = new Point(this.TextPadding.Left, this.TextPadding.Top);
                    break;

                case ContentAlignment.TopCenter:
                    point2 = new Point(DrawUtil.Half(base.Width) - DrawUtil.Half(n), this.TextPadding.Top);
                    break;

                case ContentAlignment.TopRight:
                    point2 = new Point((base.Width - n) - this.TextPadding.Right, this.TextPadding.Top);
                    break;

                case ContentAlignment.MiddleLeft:
                    point2 = new Point(this.TextPadding.Left, DrawUtil.Half(base.Height) - DrawUtil.Half(num2));
                    break;

                case ContentAlignment.MiddleCenter:
                    point2 = new Point(DrawUtil.Half(base.Width) - DrawUtil.Half(n), DrawUtil.Half(base.Height) - DrawUtil.Half(num2));
                    break;

                case ContentAlignment.BottomCenter:
                    point2 = new Point(DrawUtil.Half(base.Width) - DrawUtil.Half(n), (base.Height - num2) - this.TextPadding.Bottom);
                    break;

                case ContentAlignment.BottomRight:
                    point2 = new Point((base.Width - n) - this.TextPadding.Right, (base.Height - num2) - this.TextPadding.Bottom);
                    break;

                case ContentAlignment.MiddleRight:
                    point2 = new Point((base.Width - n) - this.TextPadding.Right, DrawUtil.Half(base.Height) - DrawUtil.Half(num2));
                    break;

                case ContentAlignment.BottomLeft:
                    point2 = new Point(this.TextPadding.Left, (base.Height - num2) - this.TextPadding.Bottom);
                    break;
            }
            using (Brush brush3 = new SolidBrush(this.ForeColor))
            {
                e.Graphics.DrawString(this.Text, this.Font, brush3, (PointF) point2);
            }
            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if ((this.MidImage != null) && (this.MidImage.Height != base.Height))
            {
                this.mMidImage = DrawUtil.ResizeImage(this.MidImage, this.MidImage.Width, base.Height);
            }
            foreach (SkinControlState state in this.SkinStates.Values)
            {
                if (state.MidImage.Height != base.Height)
                {
                    state.MidImage = DrawUtil.ResizeImage(state.MidImage, state.MidImage.Width, base.Height);
                }
            }
            base.OnSizeChanged(e);
            this.LoadState();
        }

        private void SetText(string value)
        {
            base.Text = value;
            using (Graphics graphics = base.CreateGraphics())
            {
                this.TextSize = DrawUtil.MeasureString(graphics, this.Text, this.Font);
            }
            this.Refresh();
        }

        public virtual bool AutoStyle
        {
            get
            {
                return this.mAutoStyle;
            }
            set
            {
                this.mAutoStyle = value;
            }
        }

        public string ControlState
        {
            get
            {
                return this.mControlState;
            }
        }

        [Browsable(true)]
        public virtual bool DrawEdges
        {
            get
            {
                return this.mDrawEdges;
            }
            set
            {
                this.mDrawEdges = value;
                this.Refresh();
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [Browsable(true)]
        public virtual ScalingModes HorizontalScalingMode
        {
            get
            {
                return this.mHorizontalScalingMode;
            }
            set
            {
                this.mHorizontalScalingMode = value;
            }
        }

        [Browsable(false)]
        public bool IsStyled
        {
            get
            {
                return this.mIsStyled;
            }
            set
            {
                this.mIsStyled = value;
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

        [Browsable(true)]
        public virtual string SkinBasePath
        {
            get
            {
                return this.mSkinBasePath;
            }
            set
            {
                this.mSkinBasePath = value;
                this.mControlState = null;
                this.ClearSkins();
                this.LoadSkin();
                this.Refresh();
            }
        }

        [Browsable(false)]
        public bool SkinLoaded
        {
            get
            {
                return (((this.mLeftImage != null) && (this.mMidImage != null)) && (this.mRightImage != null));
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                try
                {
                    if (!((!base.InvokeRequired || base.Disposing) || base.IsDisposed))
                    {
                        base.BeginInvoke(new StringEventHandler(this.SetText), new object[] { value });
                    }
                    else if (!base.Disposing && !base.IsDisposed)
                    {
                        using (Graphics graphics = base.CreateGraphics())
                        {
                            this.TextSize = DrawUtil.MeasureString(graphics, value, this.Font);
                        }
                        base.Text = value;
                        this.Refresh();
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        [Browsable(true)]
        public ContentAlignment TextAlign
        {
            get
            {
                return this.mTextAlign;
            }
            set
            {
                this.mTextAlign = value;
                this.Refresh();
            }
        }

        [Browsable(true)]
        public int TextHeight
        {
            get
            {
                return Convert.ToInt32(Math.Round((double) this.TextSize.Height, MidpointRounding.AwayFromZero));
            }
        }

        public virtual Padding TextPadding
        {
            get
            {
                return this.mTextPadding;
            }
            set
            {
                this.mTextPadding = value;
                this.Refresh();
            }
        }

        [Browsable(true)]
        public int TextWidth
        {
            get
            {
                return Convert.ToInt32(Math.Round((double) this.TextSize.Width, MidpointRounding.AwayFromZero));
            }
        }

        public virtual bool UseDefaultStyle
        {
            get
            {
                return false;
            }
        }
    }
}

