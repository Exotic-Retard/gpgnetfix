namespace GPG.UI.Controls
{
    using DevExpress.XtraEditors;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGComboBox : PopupContainerEdit
    {
        private Color mBorderColor = Color.White;
        private bool mFirstTime = true;

        public static  event EventHandler OnStyleControl;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.mFirstTime)
            {
                this.mFirstTime = false;
                if (OnStyleControl != null)
                {
                    OnStyleControl(this, EventArgs.Empty);
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
    }
}

