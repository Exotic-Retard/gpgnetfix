namespace GPG.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGRichLabel : RichTextBox, IStyledControl
    {
        private bool mAutoStyle = true;
        private bool mFirstTime = true;
        private bool mIsStyled;

        public static  event EventHandler OnStyleControl;

        public GPGRichLabel()
        {
            base.BorderStyle = BorderStyle.None;
            base.ReadOnly = true;
            base.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.BackColor = Color.Black;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.AutoStyle && this.mFirstTime)
            {
                this.mFirstTime = false;
                if (OnStyleControl != null)
                {
                    OnStyleControl(this, EventArgs.Empty);
                    this.mIsStyled = true;
                }
            }
            base.OnPaint(e);
        }

        [Browsable(true)]
        public bool AutoStyle
        {
            get
            {
                return this.mAutoStyle;
            }
            set
            {
                this.mAutoStyle = value;
                this.Refresh();
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
    }
}

