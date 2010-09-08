namespace GPG.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGDropDownList : ComboBox
    {
        private IContainer components;
        private Color mBorderColor = Color.White;
        private bool mDoValidate = true;
        private bool mFirstTime = true;
        private Color mFocusBackColor = Color.White;
        private Color mFocusBorderColor = Color.White;

        public static  event EventHandler OnStyleControl;

        public GPGDropDownList()
        {
            this.InitializeComponent();
            base.FlatStyle = FlatStyle.Flat;
            this.BorderColor = Color.Black;
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 15)
            {
                if ((base.SelectedItem == null) && (base.Items.Count > 0))
                {
                    this.SelectedIndex = 0;
                }
                else if (this.mDoValidate)
                {
                    bool flag = false;
                    foreach (object obj2 in base.Items)
                    {
                        if (obj2 == base.SelectedItem)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag && (base.Items.Count > 0))
                    {
                        this.SelectedIndex = 0;
                    }
                }
                if (this.mFirstTime)
                {
                    this.mFirstTime = false;
                    if (OnStyleControl != null)
                    {
                        OnStyleControl(this, EventArgs.Empty);
                    }
                }
                Graphics graphics = base.CreateGraphics();
                if (this.Focused)
                {
                    Pen pen = new Pen(this.FocusBorderColor);
                    if (this.BackColor != this.FocusBackColor)
                    {
                        this.BackColor = this.FocusBackColor;
                    }
                    graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
                    graphics.DrawRectangle(pen, 1, 1, base.Width - 3, base.Height - 3);
                    pen.Dispose();
                }
                else
                {
                    Pen pen2 = new Pen(this.BorderColor);
                    if (this.BackColor != Color.Black)
                    {
                        this.BackColor = Color.Black;
                    }
                    graphics.DrawRectangle(pen2, 0, 0, base.Width - 1, base.Height - 1);
                    graphics.DrawRectangle(pen2, 1, 1, base.Width - 3, base.Height - 3);
                    pen2.Dispose();
                }
                graphics.Dispose();
            }
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

        public bool DoValidate
        {
            get
            {
                return this.mDoValidate;
            }
            set
            {
                this.mDoValidate = value;
            }
        }

        public Color FocusBackColor
        {
            get
            {
                return this.mFocusBackColor;
            }
            set
            {
                this.mFocusBackColor = value;
            }
        }

        public Color FocusBorderColor
        {
            get
            {
                return this.mFocusBorderColor;
            }
            set
            {
                this.mFocusBorderColor = value;
            }
        }
    }
}

