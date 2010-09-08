namespace GPG.Multiplayer.Client.Admin
{
    using DevExpress.Utils;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgViewGraph : DlgBase
    {
        public SkinButton btnExecute;
        private IContainer components = null;
        private GPGChartControl gpgChart;

        public DlgViewGraph()
        {
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
            this.gpgChart = new GPGChartControl(this.components);
            this.btnExecute = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            ((ISupportInitialize) this.gpgChart).BeginInit();
            base.SuspendLayout();
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgChart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgChart.AppearanceName = "Northern Lights";
            this.gpgChart.Location = new Point(12, 0x4b);
            this.gpgChart.Name = "gpgChart";
            this.gpgChart.SeriesTemplate.PointOptionsTypeName = "PointOptions";
            this.gpgChart.Size = new Size(0x269, 0x134);
            base.ttDefault.SetSuperTip(this.gpgChart, null);
            this.gpgChart.TabIndex = 0x19;
            this.btnExecute.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnExecute.AutoStyle = true;
            this.btnExecute.BackColor = Color.Black;
            this.btnExecute.ButtonState = 0;
            this.btnExecute.DialogResult = DialogResult.OK;
            this.btnExecute.DisabledForecolor = Color.Gray;
            this.btnExecute.DrawColor = Color.White;
            this.btnExecute.DrawEdges = true;
            this.btnExecute.FocusColor = Color.Yellow;
            this.btnExecute.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnExecute.ForeColor = Color.White;
            this.btnExecute.HorizontalScalingMode = ScalingModes.Tile;
            this.btnExecute.IsStyled = true;
            this.btnExecute.Location = new Point(0x218, 0x185);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new Size(0x5d, 0x1c);
            this.btnExecute.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnExecute, null);
            this.btnExecute.TabIndex = 0x1a;
            this.btnExecute.TabStop = true;
            this.btnExecute.Text = "<LOC>Execute";
            this.btnExecute.TextAlign = ContentAlignment.MiddleCenter;
            this.btnExecute.TextPadding = new Padding(0);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.btnExecute);
            base.Controls.Add(this.gpgChart);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgViewGraph";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "DlgViewGraph";
            base.Controls.SetChildIndex(this.gpgChart, 0);
            base.Controls.SetChildIndex(this.btnExecute, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            ((ISupportInitialize) this.gpgChart).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

