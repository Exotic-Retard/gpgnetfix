namespace GPG.UI.Controls
{
    using GPG;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGMenuStrip : MenuStrip, ILocalizable
    {
        private IContainer components;
        private bool mFirstTime;

        public static  event EventHandler OnStyleControl;

        public GPGMenuStrip()
        {
            this.mFirstTime = true;
            base.ItemAdded += new ToolStripItemEventHandler(this.GPGMenuStrip_ItemAdded);
            this.InitializeComponent();
        }

        public GPGMenuStrip(IContainer container)
        {
            this.mFirstTime = true;
            container.Add(this);
            base.ItemAdded += new ToolStripItemEventHandler(this.GPGMenuStrip_ItemAdded);
            this.InitializeComponent();
        }

        private void _Localize(ToolStripItem root)
        {
            root.Text = Loc.Get(root.Text);
            root.ToolTipText = Loc.Get(root.ToolTipText);
            if (root is ToolStripDropDownItem)
            {
                foreach (ToolStripItem item in (root as ToolStripDropDownItem).DropDownItems)
                {
                    this._Localize(item);
                }
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

        private void GPGMenuStrip_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            e.Item.Paint += new PaintEventHandler(this.Item_Paint);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private void Item_Paint(object sender, PaintEventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (((item != null) && (item.Selected || item.Pressed)) && item.Enabled)
            {
                Brush brush = new SolidBrush(Color.Black);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Far;
                Rectangle clipRectangle = e.ClipRectangle;
                clipRectangle.X--;
                e.Graphics.DrawString(item.Text, item.Font, brush, clipRectangle, format);
                brush.Dispose();
            }
        }

        public void Localize()
        {
            foreach (ToolStripItem item in this.Items)
            {
                this._Localize(item);
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
            base.OnPaint(e);
        }
    }
}

