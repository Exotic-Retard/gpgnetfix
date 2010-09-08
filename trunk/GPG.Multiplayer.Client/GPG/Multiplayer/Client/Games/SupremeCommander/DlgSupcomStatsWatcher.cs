namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.XtraCharts;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraTreeList.Columns;
    using DevExpress.XtraTreeList.Nodes;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Game.SupremeCommander;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    public class DlgSupcomStatsWatcher : DlgBase
    {
        private GPGButton btnGetStats;
        private TreeListColumn colFilterName;
        private TreeListColumn colVisible;
        private IContainer components = null;
        private GPGChartControl gpgChartControl;
        private GPGTreeList gpgTreeList;
        private SupComGameManager mLastManager = null;
        private Hashtable mSeries = new Hashtable();
        private int mTime = 0;
        private RepositoryItemCheckEdit riVisible;

        public DlgSupcomStatsWatcher()
        {
            this.InitializeComponent();
            this.gpgChartControl.Legend.Font = new Font("Verdana", 8f);
            this.gpgTreeList.OnNewNode += new NodeDelegate(this.gpgTreeList_OnNewNode);
            Loc.LocObject(this);
        }

        private void btnGetStats_Click(object sender, EventArgs e)
        {
            if ((base.MainForm != null) && (base.MainForm.GetSupcomGameManager() != null))
            {
                this.btnGetStats.Enabled = false;
                if (this.mLastManager != base.MainForm.GetSupcomGameManager())
                {
                    this.mLastManager = base.MainForm.GetSupcomGameManager();
                    this.mLastManager.OnStatsXML += new StatsXML(this.mLastManager_OnStatsXML);
                }
                this.mLastManager.GetStats();
            }
        }

        private void CheckNode(TreeListNode node, bool ignoreChildren)
        {
            if (!((node.ParentNode == null) || ignoreChildren))
            {
                node.ParentNode[this.colVisible] = null;
                this.CheckNode(node.ParentNode, true);
            }
            if (node[this.colVisible] != null)
            {
                if (!(!node.HasChildren || ignoreChildren))
                {
                    this.FlagChildren(node.Nodes, (bool) node[this.colVisible]);
                }
                Series tag = node.Tag as Series;
                if (tag != null)
                {
                    tag.Visible = (bool) node[this.colVisible];
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

        private void FlagChildren(TreeListNodes nodes, bool value)
        {
            foreach (TreeListNode node in nodes)
            {
                if (node.HasChildren)
                {
                    this.FlagChildren(node.Nodes, value);
                }
                node[this.colVisible] = value;
                this.CheckNode(node, true);
            }
        }

        private void gpgTreeList_OnNewNode(TreeListNode node)
        {
            if ((node.ParentNode != null) && ((bool) node.ParentNode[this.colVisible]))
            {
                node[this.colVisible] = true;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            LineSeriesView view = new LineSeriesView();
            this.btnGetStats = new GPGButton();
            this.gpgChartControl = new GPGChartControl(this.components);
            this.gpgTreeList = new GPGTreeList(this.components);
            this.colVisible = new TreeListColumn();
            this.riVisible = new RepositoryItemCheckEdit();
            this.colFilterName = new TreeListColumn();
            ((ISupportInitialize) this.gpgChartControl).BeginInit();
            ((ISupportInitialize) view).BeginInit();
            this.gpgTreeList.BeginInit();
            this.riVisible.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.btnGetStats.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnGetStats.Location = new Point(0x1f3, 0x18c);
            this.btnGetStats.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnGetStats.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnGetStats.Name = "btnGetStats";
            this.btnGetStats.Size = new Size(130, 0x17);
            this.btnGetStats.TabIndex = 8;
            this.btnGetStats.Text = "<LOC>Get Stats";
            this.btnGetStats.UseVisualStyleBackColor = false;
            this.btnGetStats.Click += new EventHandler(this.btnGetStats_Click);
            this.gpgChartControl.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgChartControl.AppearanceName = "Pastel Kit";
            this.gpgChartControl.Location = new Point(240, 0x73);
            this.gpgChartControl.Name = "gpgChartControl";
            this.gpgChartControl.SeriesTemplate.PointOptionsTypeName = "PointOptions";
            this.gpgChartControl.SeriesTemplate.View = view;
            this.gpgChartControl.Size = new Size(0x184, 0x113);
            this.gpgChartControl.TabIndex = 9;
            this.gpgTreeList.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgTreeList.Columns.AddRange(new TreeListColumn[] { this.colVisible, this.colFilterName });
            this.gpgTreeList.Location = new Point(12, 0x73);
            this.gpgTreeList.Name = "gpgTreeList";
            this.gpgTreeList.BeginUnboundLoad();
            this.gpgTreeList.AppendNode(new object[] { false, "Unit Stats" }, -1);
            this.gpgTreeList.AppendNode(new object[] { false, "Summary Stats" }, -1);
            this.gpgTreeList.AppendNode(new object[] { false, "Economy Stats" }, -1);
            this.gpgTreeList.AppendNode(new object[] { true, "Totals Stats" }, -1);
            this.gpgTreeList.EndUnboundLoad();
            this.gpgTreeList.RepositoryItems.AddRange(new RepositoryItem[] { this.riVisible });
            this.gpgTreeList.Size = new Size(0xde, 0x113);
            this.gpgTreeList.TabIndex = 10;
            this.colVisible.Caption = "Visible";
            this.colVisible.ColumnEdit = this.riVisible;
            this.colVisible.FieldName = "Visible";
            this.colVisible.MinWidth = 0x1c;
            this.colVisible.Name = "colVisible";
            this.colVisible.VisibleIndex = 0;
            this.colVisible.Width = 50;
            this.riVisible.AutoHeight = false;
            this.riVisible.Name = "riVisible";
            this.riVisible.CheckedChanged += new EventHandler(this.riVisible_CheckedChanged);
            this.colFilterName.Caption = "Filter Name";
            this.colFilterName.FieldName = "FilterName";
            this.colFilterName.Name = "colFilterName";
            this.colFilterName.VisibleIndex = 1;
            this.colFilterName.Width = 0x97;
            base.AutoScaleDimensions = new SizeF(7f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.gpgTreeList);
            base.Controls.Add(this.gpgChartControl);
            base.Controls.Add(this.btnGetStats);
            base.Location = new Point(0, 0);
            base.Name = "DlgSupcomStatsWatcher";
            this.Text = "DlgSupcomStatsWatcher";
            base.Controls.SetChildIndex(this.btnGetStats, 0);
            base.Controls.SetChildIndex(this.gpgChartControl, 0);
            base.Controls.SetChildIndex(this.gpgTreeList, 0);
            ((ISupportInitialize) view).EndInit();
            ((ISupportInitialize) this.gpgChartControl).EndInit();
            this.gpgTreeList.EndInit();
            this.riVisible.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void mLastManager_OnStatsXML(string xml)
        {
            VGen1 method = null;
            ThreadStart start = null;
            if (!base.IsDisposed && !base.Disposing)
            {
                if (method == null)
                {
                    method = delegate (object innerxml) {
                        this.mTime++;
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(innerxml.ToString());
                        foreach (XmlNode node in document["GameStats"])
                        {
                            int num6;
                            int num8;
                            string playername = node.Attributes["name"].Value;
                            string armyid = node.Attributes["index"].Value;
                            int num = 0;
                            int num2 = 0;
                            int num3 = 0;
                            double num4 = 0.0;
                            double num5 = 0.0;
                            foreach (XmlNode node2 in node["UnitStats"])
                            {
                                string str3 = node2.Attributes["id"].Value;
                                num6 = Convert.ToInt32(node2.Attributes["built"].Value);
                                int num7 = Convert.ToInt32(node2.Attributes["lost"].Value);
                                num8 = Convert.ToInt32(node2.Attributes["killed"].Value);
                                string description = node2.Attributes["type"].Value.Replace(str3 + "_desc", "");
                                double num9 = Convert.ToDouble(node2.Attributes["damagedealt"].Value);
                                double num10 = Convert.ToDouble(node2.Attributes["damagereceived"].Value);
                                this.UpdateSeries(armyid, playername, "Unit", "built", description, (double) num6);
                                this.UpdateSeries(armyid, playername, "Unit", "lost", description, (double) num7);
                                this.UpdateSeries(armyid, playername, "Unit", "killed", description, (double) num8);
                                this.UpdateSeries(armyid, playername, "Unit", "damagedealt", description, num9);
                                this.UpdateSeries(armyid, playername, "Unit", "damagereceived", description, num10);
                                num += num6;
                                num2 += num7;
                                num3 += num8;
                                num4 += num9;
                                num5 += num10;
                            }
                            this.UpdateSeries(armyid, playername, "Totals", "built", "", (double) num);
                            this.UpdateSeries(armyid, playername, "Totals", "lost", "", (double) num2);
                            this.UpdateSeries(armyid, playername, "Totals", "killed", "", (double) num3);
                            this.UpdateSeries(armyid, playername, "Totals", "damagedealt", "", num4);
                            this.UpdateSeries(armyid, playername, "Totals", "damagereceived", "", num5);
                            foreach (XmlNode node3 in node["SummaryStats"])
                            {
                                string str5 = node3.Attributes["type"].Value;
                                num6 = Convert.ToInt32(node3.Attributes["built"].Value);
                                num8 = Convert.ToInt32(node3.Attributes["killed"].Value);
                                this.UpdateSeries(armyid, playername, "Summary", "built", str5, (double) num6);
                                this.UpdateSeries(armyid, playername, "Summary", "killed", str5, (double) num8);
                            }
                            foreach (XmlNode node4 in node["EconomyStats"])
                            {
                                string localName = node4.LocalName;
                                double num11 = Convert.ToInt32(node4.Attributes["produced"].Value);
                                double num12 = Convert.ToInt32(node4.Attributes["consumed"].Value);
                                double num13 = Convert.ToInt32(node4.Attributes["storage"].Value);
                                this.UpdateSeries(armyid, playername, "Economy", "produced", localName, num11);
                                this.UpdateSeries(armyid, playername, "Economy", "consumed", localName, num12);
                                this.UpdateSeries(armyid, playername, "Economy", "storage", localName, num13);
                            }
                        }
                    };
                }
                base.Invoke(method, new object[] { xml });
                if (start == null)
                {
                    start = delegate {
                        Thread.Sleep(0xea60);
                        if (this.mLastManager != null)
                        {
                            this.mLastManager.GetStats();
                        }
                    };
                }
                new Thread(start).Start();
            }
        }

        private TreeListNode NodeByName(string name)
        {
            return this.NodeByName(this.gpgTreeList.Nodes, name);
        }

        private TreeListNode NodeByName(TreeListNodes nodes, string name)
        {
            TreeListNode node = null;
            foreach (TreeListNode node2 in nodes)
            {
                if (node2.HasChildren && (node == null))
                {
                    node = this.NodeByName(node2.Nodes, name);
                }
                if (node2.GetValue(this.colFilterName).ToString() == name)
                {
                    return node2;
                }
            }
            return node;
        }

        private void riVisible_CheckedChanged(object sender, EventArgs e)
        {
            if (this.gpgTreeList.FocusedNode != null)
            {
                this.gpgTreeList.FocusedNode[this.colVisible] = (sender as CheckEdit).Checked;
                this.CheckNode(this.gpgTreeList.FocusedNode, false);
            }
        }

        private void UpdateSeries(string armyid, string playername, string category, string type, string description, double value)
        {
            string key = armyid + " " + playername + " " + category + " " + type + " " + description;
            Series series = null;
            if (!this.mSeries.ContainsKey(key))
            {
                series = this.gpgChartControl.Series[this.gpgChartControl.Series.Add(key, ViewType.Line)];
                series.Label.Text = playername + " " + description;
                series.Label.Visible = false;
                series.Visible = false;
                this.mSeries.Add(key, series);
                string name = armyid + " " + playername + " " + category + " Stats";
                if (this.NodeByName(name) == null)
                {
                    this.gpgTreeList.AppendNode(new object[] { false, name }, this.NodeByName(category + " Stats"));
                }
                string str3 = armyid + " " + playername + " " + type;
                if (this.NodeByName(str3) == null)
                {
                    TreeListNode node = this.gpgTreeList.AppendNode(new object[] { false, str3 }, this.NodeByName(name));
                    if (description == "")
                    {
                        node.Tag = series;
                    }
                }
                if (description != "")
                {
                    string str4 = armyid + " " + playername + " " + description + " " + type;
                    if (this.NodeByName(str4) == null)
                    {
                        this.gpgTreeList.AppendNode(new object[] { false, str4 }, this.NodeByName(str3)).Tag = series;
                        series.Visible = (bool) this.NodeByName(str4)[this.colVisible];
                    }
                }
            }
            else
            {
                series = this.mSeries[key] as Series;
            }
            SeriesPoint point = new SeriesPoint((double) this.mTime, new double[] { value });
            series.Points.Add(point);
        }
    }
}

