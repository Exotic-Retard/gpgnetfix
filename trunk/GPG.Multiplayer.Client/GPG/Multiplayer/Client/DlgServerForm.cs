namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgServerForm : DlgBase
    {
        private IContainer components = null;
        private GPGChatGrid gpgAdhocGrid;
        private GridView gvAdhoc;
        private string mFormID = "";
        private int mHeight = 70;
        private List<GPGTextBox> mParameters = new List<GPGTextBox>();
        private string mQueryName = "";
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;

        public DlgServerForm()
        {
            this.InitializeComponent();
        }

        private void CancelClick(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GenerateData(object objlist)
        {
            DataTable table;
            DataColumn column;
            DataRow row;
            DataList list = objlist as DataList;
            this.gpgAdhocGrid.DataSource = null;
            this.gvAdhoc.Columns.Clear();
            if (list.Count > 0)
            {
                table = new DataTable();
                foreach (object obj2 in list[0].InnerHash.Keys)
                {
                    column = new DataColumn();
                    column.ColumnName = obj2.ToString();
                    column.Caption = obj2.ToString();
                    table.Columns.Add(column);
                }
                foreach (DataRecord record in list)
                {
                    row = table.NewRow();
                    foreach (DataColumn curcolumn in table.Columns)
                    {
                        row[curcolumn] = record[curcolumn.ColumnName];
                    }
                    table.Rows.Add(row);
                }
                this.gpgAdhocGrid.DataSource = table;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "GenerateFields", new object[] { "GetServerFormFields", new object[] { this.mFormID } });
            }
            else
            {
                table = new DataTable();
                column = new DataColumn();
                column.ColumnName = Loc.Get("Info");
                column.Caption = Loc.Get("Info");
                table.Columns.Add(column);
                row = table.NewRow();
                row[column] = Loc.Get("No results were returned: ") + DateTime.Now.ToString();
                table.Rows.Add(row);
                this.gpgAdhocGrid.DataSource = table;
            }
        }

        private void GenerateFields(object objlist)
        {
            DataList list = objlist as DataList;
        }

        private void GenerateForm(object objlist)
        {
            DataList list = objlist as DataList;
            string str = list[0]["caption"];
            string str2 = list[0]["help_text"];
            string str3 = list[0]["query_name"];
            int num = Convert.ToInt32(list[0]["width"]);
            int num2 = Convert.ToInt32(list[0]["height"]);
            this.mQueryName = str3;
            if (num > 0)
            {
                base.Width = num;
            }
            if (num2 > 0)
            {
                base.Height = num2;
            }
            this.MinimumSize = new Size(base.Width, base.Height);
            this.Text = str;
            if (str2 != "None")
            {
                GPGLabel label = new GPGLabel();
                label.Text = str2;
                label.Top = 80;
                label.Left = 20;
                label.Width = base.Width - 40;
                label.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                label.AutoSize = false;
                label.Height = 50;
                this.mHeight += 60;
                base.Controls.Add(label);
            }
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "GenerateParams", new object[] { "GetServerFormParams", new object[] { this.mFormID } });
        }

        private void GenerateParams(object objlist)
        {
            DataList list = objlist as DataList;
            foreach (DataRecord record in list)
            {
                string str = record["caption"];
                string str2 = record["validation_type"];
                string str3 = record["validation_regex"];
                GPGLabel label = new GPGLabel();
                label.Text = str;
                label.Top = this.mHeight;
                label.Left = 20;
                label.Width = base.Width - 40;
                label.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                label.AutoSize = false;
                label.Height = 15;
                this.mHeight += 20;
                base.Controls.Add(label);
                GPGTextBox item = new GPGTextBox();
                item.Top = this.mHeight;
                item.Left = 20;
                item.Width = base.Width - 40;
                item.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                this.mHeight += 0x19;
                this.mParameters.Add(item);
                base.Controls.Add(item);
            }
            SkinButton button = new SkinButton();
            button.Text = Loc.Get("Get Data");
            button.Top = this.mHeight;
            button.Left = base.Width - 330;
            button.Width = 150;
            button.Height = 20;
            button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            button.Click += new EventHandler(this.GetDataClick);
            base.Controls.Add(button);
            button = new SkinButton();
            button.Text = Loc.Get("Cancel");
            button.Top = this.mHeight;
            button.Left = base.Width - 180;
            button.Width = 150;
            button.Height = 20;
            button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            button.Click += new EventHandler(this.CancelClick);
            base.Controls.Add(button);
            this.mHeight += 0x19;
            this.gpgAdhocGrid.Top = this.mHeight;
            this.gpgAdhocGrid.Height = (base.Height - this.mHeight) - 50;
            base.Show();
        }

        private void GetDataClick(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (GPGTextBox box in this.mParameters)
            {
                list.Add(box.Text);
            }
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "GenerateData", new object[] { this.mQueryName, list.ToArray() });
        }

        private void InitializeComponent()
        {
            this.gpgAdhocGrid = new GPGChatGrid();
            this.gvAdhoc = new GridView();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.gpgAdhocGrid.BeginInit();
            this.gvAdhoc.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.rimMemoEdit3.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgAdhocGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgAdhocGrid.CustomizeStyle = false;
            this.gpgAdhocGrid.EmbeddedNavigator.Name = "";
            this.gpgAdhocGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgAdhocGrid.IgnoreMouseWheel = false;
            this.gpgAdhocGrid.Location = new Point(12, 0x67);
            this.gpgAdhocGrid.LookAndFeel.SkinName = "Money Twins";
            this.gpgAdhocGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgAdhocGrid.MainView = this.gvAdhoc;
            this.gpgAdhocGrid.Name = "gpgAdhocGrid";
            this.gpgAdhocGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3 });
            this.gpgAdhocGrid.ShowOnlyPredefinedDetails = true;
            this.gpgAdhocGrid.Size = new Size(0x268, 0x13b);
            this.gpgAdhocGrid.TabIndex = 12;
            this.gpgAdhocGrid.ViewCollection.AddRange(new BaseView[] { this.gvAdhoc });
            this.gvAdhoc.ActiveFilterString = "";
            this.gvAdhoc.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvAdhoc.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvAdhoc.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvAdhoc.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvAdhoc.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvAdhoc.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvAdhoc.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvAdhoc.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvAdhoc.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvAdhoc.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvAdhoc.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.Empty.BackColor = Color.Black;
            this.gvAdhoc.Appearance.Empty.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvAdhoc.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvAdhoc.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvAdhoc.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvAdhoc.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvAdhoc.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvAdhoc.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvAdhoc.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvAdhoc.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvAdhoc.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvAdhoc.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvAdhoc.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvAdhoc.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvAdhoc.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvAdhoc.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.FocusedCell.Options.UseFont = true;
            this.gvAdhoc.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.FocusedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvAdhoc.Appearance.FocusedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvAdhoc.Appearance.FocusedRow.Font = new Font("Arial", 9.75f);
            this.gvAdhoc.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvAdhoc.Appearance.FocusedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvAdhoc.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.FocusedRow.Options.UseFont = true;
            this.gvAdhoc.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvAdhoc.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvAdhoc.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvAdhoc.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvAdhoc.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvAdhoc.Appearance.FooterPanel.Options.UseFont = true;
            this.gvAdhoc.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.GroupButton.BackColor = Color.Black;
            this.gvAdhoc.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvAdhoc.Appearance.GroupButton.ForeColor = Color.White;
            this.gvAdhoc.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvAdhoc.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvAdhoc.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvAdhoc.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvAdhoc.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvAdhoc.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvAdhoc.Appearance.GroupPanel.BackColor2 = Color.Black;
            this.gvAdhoc.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvAdhoc.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvAdhoc.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.GroupPanel.Options.UseFont = true;
            this.gvAdhoc.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvAdhoc.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvAdhoc.Appearance.GroupRow.ForeColor = Color.White;
            this.gvAdhoc.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.GroupRow.Options.UseFont = true;
            this.gvAdhoc.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvAdhoc.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvAdhoc.Appearance.HeaderPanel.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gvAdhoc.Appearance.HeaderPanel.ForeColor = Color.Black;
            this.gvAdhoc.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvAdhoc.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvAdhoc.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.HideSelectionRow.BackColor = Color.Gray;
            this.gvAdhoc.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvAdhoc.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvAdhoc.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvAdhoc.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.HorzLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvAdhoc.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.Preview.BackColor = Color.White;
            this.gvAdhoc.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvAdhoc.Appearance.Preview.ForeColor = Color.Purple;
            this.gvAdhoc.Appearance.Preview.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.Preview.Options.UseFont = true;
            this.gvAdhoc.Appearance.Preview.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.Row.BackColor = Color.Black;
            this.gvAdhoc.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xb2);
            this.gvAdhoc.Appearance.Row.ForeColor = Color.White;
            this.gvAdhoc.Appearance.Row.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.Row.Options.UseFont = true;
            this.gvAdhoc.Appearance.Row.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.RowSeparator.BackColor = Color.White;
            this.gvAdhoc.Appearance.RowSeparator.BackColor2 = Color.White;
            this.gvAdhoc.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.SelectedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvAdhoc.Appearance.SelectedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvAdhoc.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvAdhoc.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvAdhoc.Appearance.SelectedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvAdhoc.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvAdhoc.Appearance.SelectedRow.Options.UseFont = true;
            this.gvAdhoc.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvAdhoc.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvAdhoc.Appearance.TopNewRow.Options.UseFont = true;
            this.gvAdhoc.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvAdhoc.Appearance.VertLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvAdhoc.Appearance.VertLine.Options.UseBackColor = true;
            this.gvAdhoc.BorderStyle = BorderStyles.NoBorder;
            this.gvAdhoc.GridControl = this.gpgAdhocGrid;
            this.gvAdhoc.Name = "gvAdhoc";
            this.gvAdhoc.OptionsDetail.AllowZoomDetail = false;
            this.gvAdhoc.OptionsDetail.EnableMasterViewMode = false;
            this.gvAdhoc.OptionsDetail.ShowDetailTabs = false;
            this.gvAdhoc.OptionsDetail.SmartDetailExpand = false;
            this.gvAdhoc.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvAdhoc.OptionsSelection.MultiSelect = true;
            this.gvAdhoc.OptionsView.RowAutoHeight = true;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.gpgAdhocGrid);
            base.Location = new Point(0, 0);
            base.Name = "DlgServerForm";
            this.Text = "DlgServerForm";
            base.Controls.SetChildIndex(this.gpgAdhocGrid, 0);
            this.gpgAdhocGrid.EndInit();
            this.gvAdhoc.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.rimMemoEdit3.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadForm(string formID)
        {
            this.mFormID = formID;
            base.Width = 640;
            base.Height = 480;
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetQueryData", this, "GenerateForm", new object[] { "GetServerForm", new object[] { formID } });
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

