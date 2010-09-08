namespace GPG.UI.Controls
{
    using GPG;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGStatRow : Control, ILocalizable
    {
        private Color mDataBackColor = Color.Black;
        private Color mDataForeColor = Color.Yellow;
        private int mDesignerItemCount = 3;
        private bool mFirstTime = true;
        private Color mHeaderBackColor = Color.Gray;
        private Color mHeaderForeColor = Color.White;
        private List<MultiVal<string, object>> mItems = new List<MultiVal<string, object>>(3);

        public static  event EventHandler OnStyleControl;

        public GPGStatRow()
        {
            base.Width = 200;
            base.Height = 50;
            if (AppDomain.CurrentDomain == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    this.AddItem(string.Format("Stat{0}", i), string.Format("Value{0}", i));
                }
                this.Refresh();
            }
        }

        public void AddItem(string description, object value)
        {
            this.Items.Add(new MultiVal<string, object>(description, value));
        }

        public void Localize()
        {
            foreach (MultiVal<string, object> val in this.Items)
            {
                val.Value1 = Loc.Get(val.Value1);
            }
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
            }
            if (this.Items.Count > 0)
            {
                int num = 2;
                using (Brush brush = new SolidBrush(this.HeaderBackColor))
                {
                    e.Graphics.FillRectangle(brush, new Rectangle(0, 0, base.Width, this.Font.Height + (num * 2)));
                }
                using (Brush brush2 = new SolidBrush(this.DataBackColor))
                {
                    e.Graphics.FillRectangle(brush2, new Rectangle(0, this.Font.Height + (num * 2), base.Width, base.Height - this.Font.Height));
                }
                int num2 = 0;
                int width = base.Width / this.Items.Count;
                using (Brush brush3 = new SolidBrush(this.HeaderForeColor))
                {
                    using (Brush brush4 = new SolidBrush(this.DataForeColor))
                    {
                        foreach (MultiVal<string, object> val in this.Items)
                        {
                            e.Graphics.DrawString(string.Format("{0}", val.Value1), this.Font, brush3, (float) (num2 * width), (float) num);
                            if (val.Value2 is ISelfPaint)
                            {
                                (val.Value2 as ISelfPaint).PaintSelf(this, e.Graphics, new Rectangle(num2 * width, this.Font.Height + (num * 2), width, base.Height - (this.Font.Height + (num * 2))));
                            }
                            else
                            {
                                e.Graphics.DrawString(string.Format("{0}", val.Value2), this.Font, brush4, (float) (num2 * width), (float) (this.Font.Height + (num * 2)));
                            }
                            num2++;
                        }
                    }
                }
            }
        }

        public Color DataBackColor
        {
            get
            {
                return this.mDataBackColor;
            }
            set
            {
                this.mDataBackColor = value;
            }
        }

        public Color DataForeColor
        {
            get
            {
                return this.mDataForeColor;
            }
            set
            {
                this.mDataForeColor = value;
            }
        }

        [Browsable(true)]
        public int DesignerItemCount
        {
            get
            {
                return this.mDesignerItemCount;
            }
            set
            {
                this.mDesignerItemCount = value;
            }
        }

        public Color HeaderBackColor
        {
            get
            {
                return this.mHeaderBackColor;
            }
            set
            {
                this.mHeaderBackColor = value;
            }
        }

        public Color HeaderForeColor
        {
            get
            {
                return this.mHeaderForeColor;
            }
            set
            {
                this.mHeaderForeColor = value;
            }
        }

        public List<MultiVal<string, object>> Items
        {
            get
            {
                return this.mItems;
            }
        }
    }
}

