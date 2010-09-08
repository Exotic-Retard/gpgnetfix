namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Timers;
    using System.Windows.Forms;

    public class DlgAdhocSQL : DlgBase
    {
        private GPGButton btnCreateForm;
        private GPGButton btnExecute;
        private GPGButton btnExport;
        private GPGButton btnGetDBSchema;
        private GPGButton btnImport;
        private GPGButton btnSaveQuery;
        private IContainer components;
        private GPGChatGrid gpgAdhocGrid;
        private GridView gvAdhoc;
        private GPGLabel lStatus;
        private MemoEdit meQuery;
        private string[] mImportColumns;
        private int mImportCount;
        private char mImportDelimiter;
        private StreamReader mImportReader;
        private string mImportTableName;
        private DataTable mTable;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        private System.Timers.Timer StatusTimer;

        public DlgAdhocSQL() : this(Program.MainForm)
        {
        }

        public DlgAdhocSQL(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.StatusTimer = new System.Timers.Timer(1000.0);
            this.mTable = new DataTable();
            this.mImportCount = 0;
            this.mImportReader = null;
            this.mImportDelimiter = '|';
            this.mImportTableName = "";
            this.InitializeComponent();
            Loc.LocObject(this);
            this.StatusTimer.Elapsed += new ElapsedEventHandler(this.StatusTimer_Elapsed);
        }

        private void btnCreateForm_Click(object sender, EventArgs e)
        {
            string str = DlgAskQuestion.AskQuestion(base.MainForm, "Describe this query in 60 chars or less.");
            if (str != "")
            {
                string str2 = DlgAskQuestion.AskQuestion(base.MainForm, "Enter extended help about this query.");
                if (str2 != "")
                {
                    string str3 = DlgAskQuestion.AskQuestion(base.MainForm, "What is the parent menu item? (miCustomAdmin)");
                    if (str3 != "")
                    {
                        string str4 = DlgAskQuestion.AskQuestion(base.MainForm, "Name this form (10 chars or less).");
                        if (str4 != "")
                        {
                            string str5 = "FORM" + str4;
                            string str6 = "miCust" + str4;
                            List<string> list = new List<string>();
                            for (int i = 0; i < (this.meQuery.Text.Length - 2); i++)
                            {
                                if (this.meQuery.Text.Substring(i, 2) == "%s")
                                {
                                    string item = DlgAskQuestion.AskQuestion(base.MainForm, "What would you like to name parameter " + ((list.Count + 1)).ToString() + "?");
                                    if (item == "")
                                    {
                                        return;
                                    }
                                    list.Add(item);
                                }
                            }
                            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "SaveQuery", null, null, new object[] { str5, this.meQuery.Text });
                            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "AdhocQuery", null, null, new object[] { "INSERT INTO server_forms (form_id, caption, help_text, query_name, width, height) VALUES ('" + str4 + "', '" + str + "', '" + str2 + "', '" + str5 + "', 640, 480)" });
                            int num2 = 0;
                            foreach (string str7 in list)
                            {
                                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "AdhocQuery", null, null, new object[] { "INSERT INTO server_form_params (form_id, sort_position, caption, validation_type, validation_regex) VALUES ('" + str4 + "', " + num2.ToString() + ", '" + str7.Replace("'", "'") + "', 'string', '')" });
                                num2++;
                            }
                            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "AdhocQuery", null, null, new object[] { "INSERT INTO menu_items (menu_id, caption, parent_menu_id, form_id, non_admin) VALUES ('" + str6 + "', '" + str + "', '" + str3 + "', '" + str4 + "', 0)" });
                            DlgMessage.ShowDialog("Basic form generated.");
                        }
                    }
                }
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            this.Execute();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel (*.xls)|*.xls|Text (*.txt)|*.txt|PDF (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName.ToUpper().IndexOf(".XLS") > 0)
                {
                    this.gvAdhoc.ExportToXls(dialog.FileName);
                    Process.Start(dialog.FileName);
                }
                else if (dialog.FileName.ToUpper().IndexOf(".TXT") > 0)
                {
                    this.gvAdhoc.ExportToText(dialog.FileName, "|");
                    Process.Start(dialog.FileName);
                }
                else if (dialog.FileName.ToUpper().IndexOf(".PDF") > 0)
                {
                    this.gvAdhoc.ExportToPdf(dialog.FileName);
                    Process.Start(dialog.FileName);
                }
            }
        }

        private void btnGetDBSchema_Click(object sender, EventArgs e)
        {
            this.lStatus.Text = Loc.Get("Executing Query");
            this.lStatus.BringToFront();
            this.SetState(false);
            this.StatusTimer.Start();
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "GetServerData", this, "SchemaResults", new object[0]);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.mImportCount = 0;
            this.lStatus.Text = "";
            this.lStatus.Visible = true;
            this.mImportTableName = "";
            string str = "";
            foreach (string str2 in this.meQuery.Text.Split(" ,()".ToCharArray()))
            {
                if (str2.Trim() != "")
                {
                    if (str.ToUpper() == "FROM")
                    {
                        this.mImportTableName = str2;
                    }
                    str = str2;
                }
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text (*.txt)|*.txt|Comma Seperated Values (*.csv)|*.csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.mImportReader = new StreamReader(dialog.FileName);
                this.mImportDelimiter = '|';
                if (dialog.FileName.ToUpper().IndexOf(".CSV") > 0)
                {
                    this.mImportDelimiter = ',';
                    this.mImportColumns = new string[this.gvAdhoc.VisibleColumns.Count];
                    for (int i = 0; i < this.gvAdhoc.VisibleColumns.Count; i++)
                    {
                        this.mImportColumns[i] = this.gvAdhoc.VisibleColumns[i].FieldName;
                    }
                }
                else
                {
                    this.mImportColumns = this.ParseValue(this.mImportReader.ReadLine()).Split(new char[] { this.mImportDelimiter });
                }
                this.ParseRow();
            }
        }

        private void btnSaveQuery_Click(object sender, EventArgs e)
        {
            DlgSaveQuery query = new DlgSaveQuery();
            if (query.ShowDialog() == DialogResult.OK)
            {
                this.lStatus.Text = Loc.Get("Saving Query");
                this.lStatus.BringToFront();
                this.SetState(false);
                this.StatusTimer.Start();
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "SaveQuery", this, "SaveResults", new object[] { query.tbQueryname.Text, this.meQuery.Text });
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

        private void Execute()
        {
            this.lStatus.Text = Loc.Get("Executing Query");
            this.lStatus.BringToFront();
            this.SetState(false);
            this.StatusTimer.Start();
            ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "AdhocQuery", this, "QueryResults", new object[] { this.meQuery.Text });
        }

        private void ImportRow(bool result, List<string> columns, List<List<string>> data)
        {
            this.mImportCount++;
            this.lStatus.Text = this.mImportCount.ToString() + Loc.Get(" rows imported.");
            this.ParseRow();
        }

        private void InitializeComponent()
        {
            this.gpgAdhocGrid = new GPGChatGrid();
            this.gvAdhoc = new GridView();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.meQuery = new MemoEdit();
            this.btnExecute = new GPGButton();
            this.lStatus = new GPGLabel();
            this.btnSaveQuery = new GPGButton();
            this.btnExport = new GPGButton();
            this.btnImport = new GPGButton();
            this.btnGetDBSchema = new GPGButton();
            this.btnCreateForm = new GPGButton();
            this.gpgAdhocGrid.BeginInit();
            this.gvAdhoc.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.meQuery.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgAdhocGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgAdhocGrid.CustomizeStyle = false;
            this.gpgAdhocGrid.EmbeddedNavigator.Name = "";
            this.gpgAdhocGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgAdhocGrid.IgnoreMouseWheel = false;
            this.gpgAdhocGrid.Location = new Point(12, 0x53);
            this.gpgAdhocGrid.LookAndFeel.SkinName = "Money Twins";
            this.gpgAdhocGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgAdhocGrid.MainView = this.gvAdhoc;
            this.gpgAdhocGrid.Name = "gpgAdhocGrid";
            this.gpgAdhocGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3 });
            this.gpgAdhocGrid.ShowOnlyPredefinedDetails = true;
            this.gpgAdhocGrid.Size = new Size(700, 0xf2);
            this.gpgAdhocGrid.TabIndex = 11;
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
            this.meQuery.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.meQuery.EditValue = "show tables;";
            this.meQuery.Location = new Point(13, 0x14b);
            this.meQuery.Name = "meQuery";
            this.meQuery.Properties.Appearance.BackColor = Color.Black;
            this.meQuery.Properties.Appearance.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.meQuery.Properties.Appearance.ForeColor = Color.White;
            this.meQuery.Properties.Appearance.Options.UseBackColor = true;
            this.meQuery.Properties.Appearance.Options.UseFont = true;
            this.meQuery.Properties.Appearance.Options.UseForeColor = true;
            this.meQuery.Size = new Size(700, 0x66);
            this.meQuery.TabIndex = 12;
            this.meQuery.KeyUp += new KeyEventHandler(this.meQuery_KeyUp);
            this.btnExecute.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnExecute.Appearance.ForeColor = Color.Black;
            this.btnExecute.Appearance.Options.UseForeColor = true;
            this.btnExecute.Location = new Point(0x261, 0x1b7);
            this.btnExecute.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnExecute.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new Size(0x68, 0x17);
            this.btnExecute.TabIndex = 13;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
            this.lStatus.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lStatus.AutoStyle = true;
            this.lStatus.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lStatus.ForeColor = Color.White;
            this.lStatus.IgnoreMouseWheel = false;
            this.lStatus.IsStyled = false;
            this.lStatus.Location = new Point(0x19, 0x3b);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new Size(0x2a2, 0x15);
            this.lStatus.TabIndex = 0x17;
            this.lStatus.Text = "Executing Query";
            this.lStatus.TextAlign = ContentAlignment.TopCenter;
            this.lStatus.TextStyle = TextStyles.Default;
            this.lStatus.Visible = false;
            this.btnSaveQuery.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnSaveQuery.Appearance.ForeColor = Color.Black;
            this.btnSaveQuery.Appearance.Options.UseForeColor = true;
            this.btnSaveQuery.Location = new Point(0x1f3, 0x1b7);
            this.btnSaveQuery.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnSaveQuery.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnSaveQuery.Name = "btnSaveQuery";
            this.btnSaveQuery.Size = new Size(0x68, 0x17);
            this.btnSaveQuery.TabIndex = 0x18;
            this.btnSaveQuery.Text = "Save Query";
            this.btnSaveQuery.UseVisualStyleBackColor = true;
            this.btnSaveQuery.Click += new EventHandler(this.btnSaveQuery_Click);
            this.btnExport.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnExport.Appearance.ForeColor = Color.Black;
            this.btnExport.Appearance.Options.UseForeColor = true;
            this.btnExport.Location = new Point(0x185, 0x1b7);
            this.btnExport.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnExport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(0x68, 0x17);
            this.btnExport.TabIndex = 0x19;
            this.btnExport.Text = "Export Results";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.btnImport.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnImport.Appearance.ForeColor = Color.Black;
            this.btnImport.Appearance.Options.UseForeColor = true;
            this.btnImport.Location = new Point(0x117, 0x1b7);
            this.btnImport.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnImport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new Size(0x68, 0x17);
            this.btnImport.TabIndex = 0x1a;
            this.btnImport.Text = "Import Results";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new EventHandler(this.btnImport_Click);
            this.btnGetDBSchema.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnGetDBSchema.Appearance.ForeColor = Color.Black;
            this.btnGetDBSchema.Appearance.Options.UseForeColor = true;
            this.btnGetDBSchema.Location = new Point(0xa9, 0x1b7);
            this.btnGetDBSchema.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnGetDBSchema.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnGetDBSchema.Name = "btnGetDBSchema";
            this.btnGetDBSchema.Size = new Size(0x68, 0x17);
            this.btnGetDBSchema.TabIndex = 0x1b;
            this.btnGetDBSchema.Text = "Get Schema";
            this.btnGetDBSchema.UseVisualStyleBackColor = true;
            this.btnGetDBSchema.Click += new EventHandler(this.btnGetDBSchema_Click);
            this.btnCreateForm.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCreateForm.Appearance.ForeColor = Color.Black;
            this.btnCreateForm.Appearance.Options.UseForeColor = true;
            this.btnCreateForm.Location = new Point(0x3b, 0x1b7);
            this.btnCreateForm.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnCreateForm.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCreateForm.Name = "btnCreateForm";
            this.btnCreateForm.Size = new Size(0x68, 0x17);
            this.btnCreateForm.TabIndex = 0x1c;
            this.btnCreateForm.Text = "Create Form";
            this.btnCreateForm.UseVisualStyleBackColor = true;
            this.btnCreateForm.Click += new EventHandler(this.btnCreateForm_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x2d4, 0x1fd);
            base.Controls.Add(this.btnCreateForm);
            base.Controls.Add(this.btnGetDBSchema);
            base.Controls.Add(this.btnImport);
            base.Controls.Add(this.btnExport);
            base.Controls.Add(this.btnSaveQuery);
            base.Controls.Add(this.btnExecute);
            base.Controls.Add(this.meQuery);
            base.Controls.Add(this.gpgAdhocGrid);
            base.Controls.Add(this.lStatus);
            base.Location = new Point(0, 0);
            base.Name = "DlgAdhocSQL";
            this.Text = "Adhoc SQL Query";
            base.Controls.SetChildIndex(this.lStatus, 0);
            base.Controls.SetChildIndex(this.gpgAdhocGrid, 0);
            base.Controls.SetChildIndex(this.meQuery, 0);
            base.Controls.SetChildIndex(this.btnExecute, 0);
            base.Controls.SetChildIndex(this.btnSaveQuery, 0);
            base.Controls.SetChildIndex(this.btnExport, 0);
            base.Controls.SetChildIndex(this.btnImport, 0);
            base.Controls.SetChildIndex(this.btnGetDBSchema, 0);
            base.Controls.SetChildIndex(this.btnCreateForm, 0);
            this.gpgAdhocGrid.EndInit();
            this.gvAdhoc.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.rimMemoEdit3.EndInit();
            this.meQuery.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void meQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F5) || (e.Control && ((e.KeyCode == Keys.Return) || (e.KeyCode == Keys.Return))))
            {
                this.Execute();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!User.Current.IsAdmin)
            {
                base.Close();
            }
            else
            {
                base.OnLoad(e);
            }
        }

        private void ParseRow()
        {
            string str = "REPLACE INTO " + this.mImportTableName + " SET ";
            string str2 = "";
            if (this.mImportReader.Peek() >= 0)
            {
                string[] strArray = this.ParseValue(this.mImportReader.ReadLine()).Split(new char[] { this.mImportDelimiter });
                DataRow row = this.mTable.NewRow();
                for (int i = 0; i < this.mImportColumns.Length; i++)
                {
                    row[this.mImportColumns[i]] = strArray[i];
                    str = str + str2 + " " + this.mImportColumns[i] + "='" + strArray[i] + "'";
                    str2 = ",";
                }
                this.mTable.Rows.Add(row);
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "AdhocQuery", this, "ImportRow", new object[] { str });
            }
            else
            {
                this.mImportReader.Close();
            }
        }

        private string ParseValue(string data)
        {
            return data.Replace("\"", "");
        }

        private void QueryResults(bool result, List<string> columns, List<List<string>> data)
        {
            this.gpgAdhocGrid.DataSource = null;
            this.gvAdhoc.Columns.Clear();
            this.mTable = new DataTable();
            int num = 0;
            foreach (string str in columns)
            {
                DataColumn column = this.mTable.Columns.Add(str);
            }
            foreach (List<string> list in data)
            {
                DataRow row = this.mTable.NewRow();
                num = 0;
                foreach (string str2 in list)
                {
                    row[num] = str2;
                    num++;
                }
                this.mTable.Rows.Add(row);
            }
            this.gpgAdhocGrid.DataSource = this.mTable;
            num = 0;
            foreach (GridColumn column2 in this.gvAdhoc.Columns)
            {
                column2.OptionsColumn.AllowEdit = false;
                num++;
                if (num > 7)
                {
                    column2.Visible = false;
                }
            }
            this.StatusTimer.Stop();
            this.SetState(true);
        }

        private void SaveResults(bool result)
        {
            this.StatusTimer.Stop();
            this.SetState(true);
            if (!result)
            {
                DlgMessage.ShowDialog(Loc.Get("The query failed to save."), Loc.Get("Error"));
            }
            else
            {
                DlgMessage.ShowDialog(Loc.Get("Your query has been saved."), Loc.Get("Success"));
            }
        }

        private void SchemaResults(object result)
        {
            this.meQuery.Text = result.ToString();
            this.StatusTimer.Stop();
            this.SetState(true);
        }

        private void SetState(bool state)
        {
            this.btnExecute.Enabled = state;
            this.btnSaveQuery.Enabled = state;
            this.btnExport.Enabled = state;
            this.btnImport.Enabled = state;
            this.btnGetDBSchema.Enabled = state;
            this.meQuery.Enabled = state;
            this.lStatus.Visible = !state;
        }

        private void StatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            base.BeginInvoke((VGen0)delegate {
                if (!(base.Disposing || base.IsDisposed))
                {
                    this.lStatus.Text = this.lStatus.Text + ".";
                }
            });
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return User.Current.IsAdmin;
            }
        }
    }
}

