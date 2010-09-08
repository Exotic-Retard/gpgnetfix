namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraCharts;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public class DlgConnectionsGraph : DlgBase
    {
        private GPGButton btnCalculate;
        private IContainer components = null;
        private DateEdit deDate;
        private GPGChartControl gpgChart;
        private GPGLabel gpgLabel2;
        private GPGLabel lStatus;
        private BindingList<TimePoint> mData = new BindingList<TimePoint>();
        private DateTime mStartTime = DateTime.Now;
        private GPGTextBox tbMinutes;

        public DlgConnectionsGraph()
        {
            this.InitializeComponent();
            this.deDate.DateTime = DateTime.Now - TimeSpan.FromDays(5.0);
        }

        private void BeginTick()
        {
            if (this.mStartTime > DateTime.Now)
            {
                this.lStatus.Visible = false;
                this.btnCalculate.Enabled = true;
                this.gpgChart.Series[0].ScaleType = ScaleType.Qualitative;
                this.gpgChart.Series[0].DataSource = this.mData;
                this.gpgChart.Series[0].ArgumentDataMember = "Date";
                this.gpgChart.Series[0].ValueDataMembers[0] = "Count";
                this.gpgChart.ExportToImage(@"c:\temp\chart.png", ImageFormat.Png);
            }
            else
            {
                string str = this.mStartTime.Year.ToString() + "-" + this.mStartTime.Month.ToString().PadLeft(2, '0') + "-" + this.mStartTime.Day.ToString() + " " + this.mStartTime.Hour.ToString().PadLeft(2, '0') + ":" + this.mStartTime.Minute.ToString().PadLeft(2, '0') + ":" + this.mStartTime.Second.ToString().PadLeft(2, '0');
                string str2 = "select count(*) from connectionstats where logindate < '" + str + "' and ((logoutdate > '" + str + "') or (logoutdate is null))";
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "AdhocQuery", this, "QueryResults", new object[] { str2 });
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            this.gpgChart.Series[0].DataSource = null;
            this.mData.Clear();
            this.mStartTime = this.deDate.DateTime;
            this.btnCalculate.Enabled = false;
            this.lStatus.Text = "";
            this.lStatus.Visible = true;
            this.BeginTick();
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
            XYDiagram diagram = new XYDiagram();
            Series series = new Series();
            LineSeriesView view = new LineSeriesView();
            LineSeriesView view2 = new LineSeriesView();
            this.gpgChart = new GPGChartControl(this.components);
            this.btnCalculate = new GPGButton();
            this.tbMinutes = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.lStatus = new GPGLabel();
            this.deDate = new DateEdit();
            ((ISupportInitialize) this.gpgChart).BeginInit();
            ((ISupportInitialize) diagram).BeginInit();
            ((ISupportInitialize) series).BeginInit();
            ((ISupportInitialize) view).BeginInit();
            ((ISupportInitialize) view2).BeginInit();
            this.tbMinutes.Properties.BeginInit();
            this.deDate.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgChart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgChart.AppearanceName = "Northern Lights";
            diagram.AxisX.Tickmarks.MinorVisible = false;
            diagram.AxisX.GridLines.Visible = true;
            diagram.AxisX.GridSpacingAuto = false;
            diagram.AxisX.Label.Angle = 90;
            diagram.AxisX.Label.Visible = false;
            diagram.AxisX.Title.Visible = true;
            diagram.AxisX.Title.Text = "User Connections";
            diagram.AxisX.GridSpacing = 0.01;
            diagram.AxisX.MinorCount = 1;
            diagram.AxisX.Interlaced = true;
            this.gpgChart.Diagram = diagram;
            this.gpgChart.Legend.Visible = false;
            this.gpgChart.Location = new Point(0x13, 0x65);
            this.gpgChart.Name = "gpgChart";
            series.PointOptionsTypeName = "PointOptions";
            series.View = view;
            series.Name = "Logins";
            this.gpgChart.Series.AddRange(new Series[] { series });
            this.gpgChart.SeriesTemplate.PointOptionsTypeName = "PointOptions";
            this.gpgChart.SeriesTemplate.View = view2;
            this.gpgChart.Size = new Size(0x256, 0x120);
            this.gpgChart.TabIndex = 7;
            this.btnCalculate.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCalculate.Location = new Point(0x1ee, 0x18b);
            this.btnCalculate.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnCalculate.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new Size(0x7f, 0x17);
            this.btnCalculate.TabIndex = 8;
            this.btnCalculate.Text = "<LOC>Calculate Stats";
            this.btnCalculate.UseVisualStyleBackColor = false;
            this.btnCalculate.Click += new EventHandler(this.btnCalculate_Click);
            this.tbMinutes.EditValue = "60";
            this.tbMinutes.Location = new Point(0x6c, 0x4b);
            this.tbMinutes.Name = "tbMinutes";
            this.tbMinutes.Properties.Appearance.BackColor = Color.Black;
            this.tbMinutes.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbMinutes.Properties.Appearance.ForeColor = Color.White;
            this.tbMinutes.Properties.Appearance.Options.UseBackColor = true;
            this.tbMinutes.Properties.Appearance.Options.UseBorderColor = true;
            this.tbMinutes.Properties.Appearance.Options.UseForeColor = true;
            this.tbMinutes.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbMinutes.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbMinutes.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbMinutes.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbMinutes.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbMinutes.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbMinutes.Properties.BorderStyle = BorderStyles.Simple;
            this.tbMinutes.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            this.tbMinutes.Properties.EditFormat.FormatType = FormatType.Numeric;
            this.tbMinutes.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbMinutes.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbMinutes.Size = new Size(100, 20);
            this.tbMinutes.TabIndex = 10;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(20, 0x4c);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x52, 0x10);
            this.gpgLabel2.TabIndex = 12;
            this.gpgLabel2.Text = "Minute Ticks";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.lStatus.AutoSize = true;
            this.lStatus.AutoStyle = true;
            this.lStatus.Font = new Font("Arial", 9.75f);
            this.lStatus.ForeColor = Color.White;
            this.lStatus.IgnoreMouseWheel = false;
            this.lStatus.IsStyled = false;
            this.lStatus.Location = new Point(0x16b, 0x4f);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new Size(0x43, 0x10);
            this.lStatus.TabIndex = 13;
            this.lStatus.Text = "gpgLabel1";
            this.lStatus.TextStyle = TextStyles.Default;
            this.lStatus.Visible = false;
            this.deDate.EditValue = null;
            this.deDate.Location = new Point(0xd6, 0x4b);
            this.deDate.Name = "deDate";
            this.deDate.Properties.Appearance.BackColor = Color.Black;
            this.deDate.Properties.Appearance.ForeColor = Color.White;
            this.deDate.Properties.Appearance.Options.UseBackColor = true;
            this.deDate.Properties.Appearance.Options.UseForeColor = true;
            this.deDate.Properties.BorderStyle = BorderStyles.Simple;
            this.deDate.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.deDate.Size = new Size(0x8f, 20);
            this.deDate.TabIndex = 14;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.deDate);
            base.Controls.Add(this.lStatus);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.tbMinutes);
            base.Controls.Add(this.btnCalculate);
            base.Controls.Add(this.gpgChart);
            base.Location = new Point(0, 0);
            base.Name = "DlgConnectionsGraph";
            this.Text = "DlgConnectionsGraph";
            base.Controls.SetChildIndex(this.gpgChart, 0);
            base.Controls.SetChildIndex(this.btnCalculate, 0);
            base.Controls.SetChildIndex(this.tbMinutes, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.lStatus, 0);
            base.Controls.SetChildIndex(this.deDate, 0);
            ((ISupportInitialize) diagram).EndInit();
            ((ISupportInitialize) view).EndInit();
            ((ISupportInitialize) series).EndInit();
            ((ISupportInitialize) view2).EndInit();
            ((ISupportInitialize) this.gpgChart).EndInit();
            this.tbMinutes.Properties.EndInit();
            this.deDate.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void QueryResults(bool result, List<string> columns, List<List<string>> data)
        {
            int num = Convert.ToInt32(data[0][0]);
            TimePoint item = new TimePoint();
            item.Date = this.mStartTime;
            item.Count = num;
            this.lStatus.Text = this.mStartTime.ToString() + " with " + num.ToString() + " users online.";
            this.mData.Add(item);
            this.mStartTime += TimeSpan.FromMinutes((double) Convert.ToInt32(this.tbMinutes.Text));
            this.BeginTick();
        }
    }
}

