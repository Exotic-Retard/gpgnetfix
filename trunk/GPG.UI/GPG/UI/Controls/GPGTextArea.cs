namespace GPG.UI.Controls
{
    using DevExpress.UserSkins;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.ViewInfo;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGTextArea : MemoEdit
    {
        private IContainer components;
        private Color mBorderColor = Color.White;
        private bool mFirstTime = true;

        public static  event EventHandler OnStyleControl;

        public GPGTextArea()
        {
            this.InitializeComponent();
            base.Properties.Appearance.BackColor = Color.Black;
            base.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            base.Properties.Appearance.ForeColor = Color.White;
            base.Properties.Appearance.Options.UseBackColor = true;
            base.Properties.Appearance.Options.UseBorderColor = true;
            base.Properties.Appearance.Options.UseForeColor = true;
            base.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            base.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            base.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            base.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            base.Properties.AppearanceFocused.Options.UseBackColor = true;
            base.Properties.AppearanceFocused.Options.UseBorderColor = true;
            base.Properties.BorderStyle = BorderStyles.Simple;
            base.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.mFirstTime)
            {
                this.mFirstTime = false;
                if (OnStyleControl != null)
                {
                    OnStyleControl(this, EventArgs.Empty);
                }
                else
                {
                    this.LookAndFeel.UseDefaultLookAndFeel = false;
                    BonusSkins.Register();
                    this.LookAndFeel.SkinName = "London Liquid Sky";
                }
            }
            base.OnPaint(e);
            Pen pen = new Pen(this.BorderColor);
            e.Graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
            e.Graphics.DrawRectangle(pen, 1, 1, base.Width - 3, base.Height - 3);
            pen.Dispose();
        }

        public Color BorderColor
        {
            get
            {
                return this.mBorderColor;
            }
            set
            {
                this.mBorderColor = value;
            }
        }

        public bool IsMaxScrolled
        {
            get
            {
                ScrollBarViewInfo info = this.VScrollBar.GetType().GetProperty("ViewInfo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.VScrollBar, null) as ScrollBarViewInfo;
                return (info.VisibleValue == info.VisibleMaximum);
            }
        }

        public override string Text
        {
            get
            {
                return base.Text.Replace(@"\r", "\r").Replace(@"\n", "\n");
            }
            set
            {
                base.Text = value;
            }
        }

        public ScrollBarBase VScrollBar
        {
            get
            {
                return base.scrollHelper.VScroll;
            }
        }
    }
}

