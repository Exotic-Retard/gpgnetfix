namespace GPG.UI.Controls
{
    using DevExpress.XtraCharts;
    using System;
    using System.ComponentModel;

    public class GPGChartControl : ChartControl
    {
        private IContainer components;

        public GPGChartControl()
        {
            this.InitializeComponent();
        }

        public GPGChartControl(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
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
    }
}

