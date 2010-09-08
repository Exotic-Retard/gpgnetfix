namespace GPG.Multiplayer.Client.Admin
{
    using DevExpress.Utils;
    using DevExpress.XtraCharts;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgAdhocGraph : DlgBase
    {
        public SkinButton btnExecute;
        public SkinButton btnSave;
        private GPGCheckBox cbLabels;
        public GPGDropDownList cbViewType;
        private IContainer components = null;
        private GPGChartControl gpgChart;
        private GPGTextArea tbQuery;

        public DlgAdhocGraph()
        {
            this.InitializeComponent();
            this.cbViewType.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(ViewType)))
            {
                this.cbViewType.Items.Add(str);
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            this.gpgChart.Series.Clear();
            this.cbViewType.Text = "Line";
            this.btnExecute.Enabled = false;
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                try
                {
                    VGen0 method = null;
                    List<string> cols;
                    List<List<string>> rows;
                    if (DataAccess.AdhocQuery(this.tbQuery.Text, out cols, out rows) && (rows.Count > 0))
                    {
                        if (method == null)
                        {
                            method = delegate {
                                try
                                {
                                    Series series;
                                    string str;
                                    SeriesPoint point;
                                    if (cols.Count == 2)
                                    {
                                        series = new Series("Raw Data", ViewType.Line) {
                                            Label = { Visible = false },
                                            LegendText = cols[0]
                                        };
                                        (series.View as LineSeriesView).LineMarkerOptions.Size = 2;
                                        foreach (List<string> list in rows)
                                        {
                                            str = list[0];
                                            point = new SeriesPoint(str, new double[] { Convert.ToDouble(list[1]) });
                                            series.Points.Add(point);
                                        }
                                        this.gpgChart.Series.Add(series);
                                    }
                                    else if (cols.Count == 3)
                                    {
                                        Hashtable hashtable = new Hashtable();
                                        foreach (List<string> list in rows)
                                        {
                                            series = null;
                                            if (hashtable.ContainsKey(list[1]))
                                            {
                                                series = hashtable[list[1]] as Series;
                                            }
                                            else
                                            {
                                                series = new Series("Raw Data", ViewType.Line) {
                                                    Label = { Visible = false },
                                                    LegendText = list[1]
                                                };
                                                (series.View as LineSeriesView).LineMarkerOptions.Size = 2;
                                                hashtable.Add(list[1], series);
                                                this.gpgChart.Series.Add(series);
                                            }
                                            str = list[0];
                                            point = new SeriesPoint(str, new double[] { Convert.ToDouble(list[2]) });
                                            series.Points.Add(point);
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            };
                        }
                        base.Invoke(method);
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                base.Invoke((VGen0)delegate {
                    this.btnExecute.Enabled = true;
                });
            }, new object[0]);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "HTML|*.html";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.gpgChart.ExportToHtml(dialog.FileName);
                Process.Start(dialog.FileName);
            }
        }

        private void cbLabels_CheckStateChanged(object sender, EventArgs e)
        {
            foreach (Series series in this.gpgChart.Series)
            {
                series.Label.Visible = this.cbLabels.Checked;
            }
        }

        private void cbViewType_SelectedValueChanged(object sender, EventArgs e)
        {
            foreach (string str in Enum.GetNames(typeof(ViewType)))
            {
                if (str == this.cbViewType.Text)
                {
                    foreach (Series series in this.gpgChart.Series)
                    {
                        series.ChangeView((ViewType) Enum.Parse(typeof(ViewType), str));
                    }
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

        private void InitializeComponent()
        {
            this.components = new Container();
            XYDiagram diagram = new XYDiagram();
            RectangleGradientFillOptions options = new RectangleGradientFillOptions();
            Series series = new Series();
            LineSeriesView view = new LineSeriesView();
            this.btnExecute = new SkinButton();
            this.gpgChart = new GPGChartControl(this.components);
            this.tbQuery = new GPGTextArea();
            this.btnSave = new SkinButton();
            this.cbViewType = new GPGDropDownList();
            this.cbLabels = new GPGCheckBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            ((ISupportInitialize) this.gpgChart).BeginInit();
            ((ISupportInitialize) diagram).BeginInit();
            ((ISupportInitialize) series).BeginInit();
            ((ISupportInitialize) view).BeginInit();
            this.tbQuery.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
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
            this.btnExecute.Location = new Point(0x217, 0x185);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new Size(0x5d, 0x1c);
            this.btnExecute.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnExecute, null);
            this.btnExecute.TabIndex = 0x17;
            this.btnExecute.TabStop = true;
            this.btnExecute.Text = "<LOC>Execute";
            this.btnExecute.TextAlign = ContentAlignment.MiddleCenter;
            this.btnExecute.TextPadding = new System.Windows.Forms.Padding(0);
            this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
            this.gpgChart.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgChart.AppearanceName = "Pastel Kit";
            this.gpgChart.BackColor = Color.Black;
            this.gpgChart.Border.Color = Color.White;
            diagram.BackColor = Color.Black;
            diagram.AxisY.Color = Color.White;
            diagram.AxisY.InterlacedColor = Color.White;
            diagram.AxisY.Label.TextColor = Color.White;
            diagram.AxisY.Title.TextColor = Color.White;
            diagram.AxisX.Color = Color.White;
            diagram.AxisX.GridLines.Color = Color.White;
            diagram.AxisX.InterlacedColor = Color.White;
            diagram.AxisX.Label.TextColor = Color.White;
            diagram.AxisX.Title.TextColor = Color.White;
            diagram.BorderColor = Color.White;
            diagram.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Gradient;
            options.Color2 = Color.FromArgb(0x40, 0x40, 0x40);
            options.GradientMode = RectangleGradientMode.BottomRightToTopLeft;
            diagram.FillStyle.Options = options;
            diagram.Shadow.Color = Color.White;
            this.gpgChart.Diagram = diagram;
            this.gpgChart.Legend.BackColor = Color.Black;
            this.gpgChart.Legend.Border.Color = Color.Black;
            this.gpgChart.Legend.TextColor = Color.White;
            this.gpgChart.Location = new Point(12, 0x53);
            this.gpgChart.Name = "gpgChart";
            this.gpgChart.RuntimeRotation = true;
            this.gpgChart.RuntimeSelection = true;
            series.PointOptionsTypeName = "PointOptions";
            view.LineMarkerOptions.Size = 5;
            series.View = view;
            series.Name = "Series 1";
            this.gpgChart.Series.AddRange(new Series[] { series });
            this.gpgChart.SeriesTemplate.PointOptionsTypeName = "PointOptions";
            this.gpgChart.Size = new Size(0x269, 0xca);
            base.ttDefault.SetSuperTip(this.gpgChart, null);
            this.gpgChart.TabIndex = 0x18;
            this.tbQuery.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.tbQuery.BorderColor = Color.White;
            this.tbQuery.Location = new Point(12, 0x123);
            this.tbQuery.Name = "tbQuery";
            this.tbQuery.Properties.Appearance.BackColor = Color.Black;
            this.tbQuery.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbQuery.Properties.Appearance.ForeColor = Color.White;
            this.tbQuery.Properties.Appearance.Options.UseBackColor = true;
            this.tbQuery.Properties.Appearance.Options.UseBorderColor = true;
            this.tbQuery.Properties.Appearance.Options.UseForeColor = true;
            this.tbQuery.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbQuery.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbQuery.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbQuery.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbQuery.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbQuery.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbQuery.Properties.BorderStyle = BorderStyles.Simple;
            this.tbQuery.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbQuery.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbQuery.Size = new Size(0x269, 0x60);
            this.tbQuery.TabIndex = 0x19;
            this.btnSave.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnSave.AutoStyle = true;
            this.btnSave.BackColor = Color.Black;
            this.btnSave.ButtonState = 0;
            this.btnSave.DialogResult = DialogResult.OK;
            this.btnSave.DisabledForecolor = Color.Gray;
            this.btnSave.DrawColor = Color.White;
            this.btnSave.DrawEdges = true;
            this.btnSave.FocusColor = Color.Yellow;
            this.btnSave.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.HorizontalScalingMode = ScalingModes.Tile;
            this.btnSave.IsStyled = true;
            this.btnSave.Location = new Point(0x1b4, 0x185);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(0x5d, 0x1c);
            this.btnSave.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnSave, null);
            this.btnSave.TabIndex = 0x1a;
            this.btnSave.TabStop = true;
            this.btnSave.Text = "<LOC>Save";
            this.btnSave.TextAlign = ContentAlignment.MiddleCenter;
            this.btnSave.TextPadding = new System.Windows.Forms.Padding(0);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.cbViewType.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbViewType.BackColor = Color.Black;
            this.cbViewType.BorderColor = Color.Black;
            this.cbViewType.DoValidate = true;
            this.cbViewType.FlatStyle = FlatStyle.Flat;
            this.cbViewType.FocusBackColor = Color.White;
            this.cbViewType.FocusBorderColor = Color.White;
            this.cbViewType.ForeColor = Color.White;
            this.cbViewType.FormattingEnabled = true;
            this.cbViewType.Items.AddRange(new object[] { "Beta Server", "GPG Test Server 1", "GPG Test Server 2", "Test Staging Server" });
            this.cbViewType.Location = new Point(12, 0x185);
            this.cbViewType.Name = "cbViewType";
            this.cbViewType.Size = new Size(0xe3, 0x15);
            base.ttDefault.SetSuperTip(this.cbViewType, null);
            this.cbViewType.TabIndex = 0x1b;
            this.cbViewType.SelectedValueChanged += new EventHandler(this.cbViewType_SelectedValueChanged);
            this.cbLabels.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbLabels.AutoSize = true;
            this.cbLabels.Location = new Point(0xf5, 0x189);
            this.cbLabels.Name = "cbLabels";
            this.cbLabels.Size = new Size(0x89, 0x11);
            base.ttDefault.SetSuperTip(this.cbLabels, null);
            this.cbLabels.TabIndex = 0x1c;
            this.cbLabels.Text = "Show Series Labels";
            this.cbLabels.UsesBG = false;
            this.cbLabels.UseVisualStyleBackColor = true;
            this.cbLabels.CheckStateChanged += new EventHandler(this.cbLabels_CheckStateChanged);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.cbLabels);
            base.Controls.Add(this.cbViewType);
            base.Controls.Add(this.btnSave);
            base.Controls.Add(this.tbQuery);
            base.Controls.Add(this.gpgChart);
            base.Controls.Add(this.btnExecute);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgAdhocGraph";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "DlgAdhocGraph";
            base.Controls.SetChildIndex(this.btnExecute, 0);
            base.Controls.SetChildIndex(this.gpgChart, 0);
            base.Controls.SetChildIndex(this.tbQuery, 0);
            base.Controls.SetChildIndex(this.btnSave, 0);
            base.Controls.SetChildIndex(this.cbViewType, 0);
            base.Controls.SetChildIndex(this.cbLabels, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            ((ISupportInitialize) diagram).EndInit();
            ((ISupportInitialize) view).EndInit();
            ((ISupportInitialize) series).EndInit();
            ((ISupportInitialize) this.gpgChart).EndInit();
            this.tbQuery.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }
    }
}

