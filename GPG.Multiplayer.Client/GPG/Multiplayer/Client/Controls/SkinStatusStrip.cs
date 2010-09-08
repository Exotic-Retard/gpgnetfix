namespace GPG.Multiplayer.Client.Controls
{
    using GPG.Logging;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class SkinStatusStrip : SkinLabel
    {
        private IContainer components = null;
        private List<SkinControl> Items = new List<SkinControl>();
        private int LastPaintCount = 0;
        private int mItemPadding = 6;

        public SkinStatusStrip()
        {
            this.InitializeComponent();
            this.TextPadding = new Padding(0, 0, 8, 0);
        }

        public void Add(SkinControl item)
        {
            this.Items.Add(item);
            this.ArrangeItems();
            base.Controls.Add(item);
        }

        private void ArrangeItems()
        {
            int num = (base.TextWidth + this.TextPadding.Left) + this.TextPadding.Right;
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].Left = num;
                this.Items[i].Top = DrawUtil.Center(this, this.Items[i]).Y;
                num += this.Items[i].Width + this.ItemPadding;
            }
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

        public void InsertItem(int index, SkinControl item)
        {
            this.Items.Insert(index, item);
            this.ArrangeItems();
            base.Controls.Add(item);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.LastPaintCount = this.Items.Count;
            base.OnPaint(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.ArrangeItems();
            base.OnTextChanged(e);
        }

        public void Remove(SkinControl item)
        {
            try
            {
                this.Items.Remove(item);
                this.ArrangeItems();
                base.Controls.Remove(item);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public int ItemPadding
        {
            get
            {
                return this.mItemPadding;
            }
            set
            {
                this.mItemPadding = value;
            }
        }

        private bool ListChanged
        {
            get
            {
                return (this.LastPaintCount != this.Items.Count);
            }
        }
    }
}

