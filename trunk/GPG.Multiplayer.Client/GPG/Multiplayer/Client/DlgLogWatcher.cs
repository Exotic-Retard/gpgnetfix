namespace GPG.Multiplayer.Client
{
    using BugSplatDotNet;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraVerticalGrid;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;

    public class DlgLogWatcher : DlgBase
    {
        private GPGButton btnCancel;
        private GridColumn colData;
        private GridColumn colDescription;
        private GridColumn colTime;
        private GridColumn colType;
        private IContainer components = null;
        private GPGChatGrid gpgLogGrid;
        private GridView gvLogView;
        public BindingList<LogData> mLogList = new BindingList<LogData>();
        private PopupContainerControl pcPropertyView;
        private PropertyGridControl pgData;
        private RepositoryItemMemoExEdit repositoryItemMemoExEdit1;
        private RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        private RepositoryItemPopupContainerEdit riPopup;
        public static DlgLogWatcher sLog = null;
        private static StreamWriter sLogFile = null;

        private DlgLogWatcher()
        {
            this.InitializeComponent();
            IntPtr handle = base.Handle;
            Loc.LocObject(this);
            ErrorLog.OnLogErrorEvent += new LogEvent(DlgLogWatcher.OnLogEvent);
            EventLog.OnLogEvent += new LogEvent(DlgLogWatcher.OnLogEvent);
            this.gpgLogGrid.IsCheckingExpand = false;
            this.gpgLogGrid.DataSource = this.mLogList;
            try
            {
                int num = 0;
                string path = string.Empty;
                while (sLogFile == null)
                {
                    try
                    {
                        path = string.Format(@"{0}\gpgnet{1}.log", Environment.GetEnvironmentVariable("TEMP"), num);
                        sLogFile = new StreamWriter(path);
                    }
                    catch
                    {
                        sLogFile = null;
                        num++;
                    }
                }
                sLogFile.WriteLine("The log file was initialized.");
                sLogFile.Flush();
                if (path != "")
                {
                    BugSplat.addAdditionalFile(path);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
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

        private void InitializeComponent()
        {
            this.gpgLogGrid = new GPGChatGrid();
            this.gvLogView = new GridView();
            this.colTime = new GridColumn();
            this.repositoryItemTimeEdit1 = new RepositoryItemTimeEdit();
            this.colType = new GridColumn();
            this.colDescription = new GridColumn();
            this.colData = new GridColumn();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.riPopup = new RepositoryItemPopupContainerEdit();
            this.pcPropertyView = new PopupContainerControl();
            this.pgData = new PropertyGridControl();
            this.repositoryItemMemoExEdit1 = new RepositoryItemMemoExEdit();
            this.btnCancel = new GPGButton();
            this.gpgLogGrid.BeginInit();
            this.gvLogView.BeginInit();
            this.repositoryItemTimeEdit1.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.riPopup.BeginInit();
            this.pcPropertyView.BeginInit();
            this.pcPropertyView.SuspendLayout();
            this.pgData.BeginInit();
            this.repositoryItemMemoExEdit1.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgLogGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLogGrid.CustomizeStyle = false;
            this.gpgLogGrid.EmbeddedNavigator.Name = "";
            this.gpgLogGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLogGrid.IgnoreMouseWheel = false;
            this.gpgLogGrid.Location = new Point(13, 0x4c);
            this.gpgLogGrid.LookAndFeel.SkinName = "Money Twins";
            this.gpgLogGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgLogGrid.MainView = this.gvLogView;
            this.gpgLogGrid.Name = "gpgLogGrid";
            this.gpgLogGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3, this.riPopup, this.repositoryItemTimeEdit1, this.repositoryItemMemoExEdit1 });
            this.gpgLogGrid.ShowOnlyPredefinedDetails = true;
            this.gpgLogGrid.Size = new Size(0x268, 0x138);
            this.gpgLogGrid.TabIndex = 0x1c;
            this.gpgLogGrid.ViewCollection.AddRange(new BaseView[] { this.gvLogView });
            this.gvLogView.ActiveFilterString = "";
            this.gvLogView.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvLogView.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvLogView.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvLogView.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvLogView.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvLogView.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvLogView.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvLogView.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvLogView.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvLogView.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvLogView.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvLogView.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvLogView.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvLogView.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvLogView.Appearance.Empty.BackColor = Color.Black;
            this.gvLogView.Appearance.Empty.Options.UseBackColor = true;
            this.gvLogView.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvLogView.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvLogView.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvLogView.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvLogView.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvLogView.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvLogView.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvLogView.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvLogView.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvLogView.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvLogView.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvLogView.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvLogView.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvLogView.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvLogView.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvLogView.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvLogView.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvLogView.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvLogView.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvLogView.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvLogView.Appearance.FocusedCell.Options.UseFont = true;
            this.gvLogView.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvLogView.Appearance.FocusedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvLogView.Appearance.FocusedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvLogView.Appearance.FocusedRow.Font = new Font("Arial", 9.75f);
            this.gvLogView.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvLogView.Appearance.FocusedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvLogView.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvLogView.Appearance.FocusedRow.Options.UseFont = true;
            this.gvLogView.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvLogView.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvLogView.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvLogView.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvLogView.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvLogView.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvLogView.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvLogView.Appearance.FooterPanel.Options.UseFont = true;
            this.gvLogView.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvLogView.Appearance.GroupButton.BackColor = Color.Black;
            this.gvLogView.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvLogView.Appearance.GroupButton.ForeColor = Color.White;
            this.gvLogView.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvLogView.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvLogView.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvLogView.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvLogView.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvLogView.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvLogView.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvLogView.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvLogView.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvLogView.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvLogView.Appearance.GroupPanel.BackColor2 = Color.Black;
            this.gvLogView.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvLogView.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvLogView.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvLogView.Appearance.GroupPanel.Options.UseFont = true;
            this.gvLogView.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvLogView.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvLogView.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvLogView.Appearance.GroupRow.ForeColor = Color.White;
            this.gvLogView.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvLogView.Appearance.GroupRow.Options.UseFont = true;
            this.gvLogView.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvLogView.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvLogView.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvLogView.Appearance.HeaderPanel.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gvLogView.Appearance.HeaderPanel.ForeColor = Color.Black;
            this.gvLogView.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvLogView.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvLogView.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvLogView.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvLogView.Appearance.HideSelectionRow.BackColor = Color.Gray;
            this.gvLogView.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvLogView.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvLogView.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvLogView.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvLogView.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvLogView.Appearance.HorzLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvLogView.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvLogView.Appearance.Preview.BackColor = Color.White;
            this.gvLogView.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvLogView.Appearance.Preview.ForeColor = Color.Purple;
            this.gvLogView.Appearance.Preview.Options.UseBackColor = true;
            this.gvLogView.Appearance.Preview.Options.UseFont = true;
            this.gvLogView.Appearance.Preview.Options.UseForeColor = true;
            this.gvLogView.Appearance.Row.BackColor = Color.Black;
            this.gvLogView.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xb2);
            this.gvLogView.Appearance.Row.ForeColor = Color.White;
            this.gvLogView.Appearance.Row.Options.UseBackColor = true;
            this.gvLogView.Appearance.Row.Options.UseFont = true;
            this.gvLogView.Appearance.Row.Options.UseForeColor = true;
            this.gvLogView.Appearance.RowSeparator.BackColor = Color.White;
            this.gvLogView.Appearance.RowSeparator.BackColor2 = Color.White;
            this.gvLogView.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvLogView.Appearance.SelectedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvLogView.Appearance.SelectedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvLogView.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvLogView.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvLogView.Appearance.SelectedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvLogView.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvLogView.Appearance.SelectedRow.Options.UseFont = true;
            this.gvLogView.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvLogView.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvLogView.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvLogView.Appearance.TopNewRow.Options.UseFont = true;
            this.gvLogView.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvLogView.Appearance.VertLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvLogView.Appearance.VertLine.Options.UseBackColor = true;
            this.gvLogView.BorderStyle = BorderStyles.NoBorder;
            this.gvLogView.Columns.AddRange(new GridColumn[] { this.colTime, this.colType, this.colDescription, this.colData });
            this.gvLogView.GridControl = this.gpgLogGrid;
            this.gvLogView.Name = "gvLogView";
            this.gvLogView.OptionsBehavior.AutoPopulateColumns = false;
            this.gvLogView.OptionsDetail.AllowZoomDetail = false;
            this.gvLogView.OptionsDetail.EnableMasterViewMode = false;
            this.gvLogView.OptionsDetail.ShowDetailTabs = false;
            this.gvLogView.OptionsDetail.SmartDetailExpand = false;
            this.gvLogView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLogView.OptionsSelection.MultiSelect = true;
            this.gvLogView.OptionsView.RowAutoHeight = true;
            this.colTime.Caption = "Time";
            this.colTime.ColumnEdit = this.repositoryItemTimeEdit1;
            this.colTime.FieldName = "DateTime";
            this.colTime.Name = "colTime";
            this.colTime.OptionsColumn.AllowEdit = false;
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 0;
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            this.colType.Caption = "Type";
            this.colType.FieldName = "LogType";
            this.colType.Name = "colType";
            this.colType.OptionsColumn.AllowEdit = false;
            this.colType.Visible = true;
            this.colType.VisibleIndex = 1;
            this.colDescription.Caption = "Description";
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 2;
            this.colData.Caption = "Data";
            this.colData.ColumnEdit = this.rimMemoEdit3;
            this.colData.FieldName = "Data";
            this.colData.Name = "colData";
            this.colData.Visible = true;
            this.colData.VisibleIndex = 3;
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.riPopup.AutoHeight = false;
            this.riPopup.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.riPopup.Name = "riPopup";
            this.riPopup.PopupControl = this.pcPropertyView;
            this.riPopup.Popup += new EventHandler(this.riPopup_Popup);
            this.pcPropertyView.Controls.Add(this.pgData);
            this.pcPropertyView.Location = new Point(13, 0xda);
            this.pcPropertyView.Name = "pcPropertyView";
            this.pcPropertyView.Size = new Size(0xdf, 180);
            this.pcPropertyView.TabIndex = 0x1f;
            this.pcPropertyView.Text = "popupContainerControl1";
            this.pgData.Dock = DockStyle.Fill;
            this.pgData.Location = new Point(0, 0);
            this.pgData.Name = "pgData";
            this.pgData.Size = new Size(0xdf, 180);
            this.pgData.TabIndex = 0;
            this.repositoryItemMemoExEdit1.AutoHeight = false;
            this.repositoryItemMemoExEdit1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemMemoExEdit1.Name = "repositoryItemMemoExEdit1";
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.Appearance.ForeColor = Color.Black;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.Location = new Point(0x20d, 0x18a);
            this.btnCancel.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x68, 0x17);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "<LOC>Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.pcPropertyView);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.gpgLogGrid);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgLogWatcher";
            this.Text = "Log Watcher";
            base.Controls.SetChildIndex(this.gpgLogGrid, 0);
            base.Controls.SetChildIndex(this.btnCancel, 0);
            base.Controls.SetChildIndex(this.pcPropertyView, 0);
            this.gpgLogGrid.EndInit();
            this.gvLogView.EndInit();
            this.repositoryItemTimeEdit1.EndInit();
            this.rimMemoEdit3.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.riPopup.EndInit();
            this.pcPropertyView.EndInit();
            this.pcPropertyView.ResumeLayout(false);
            this.pgData.EndInit();
            this.repositoryItemMemoExEdit1.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static void InitLog()
        {
            sLog = new DlgLogWatcher();
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                sLogFile.Close();
                base.OnClosed(e);
            }
            catch
            {
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }

        private static void OnLogEvent(string message, string classification, object data)
        {
            try
            {
                if (sLogFile != null)
                {
                    string str = "";
                    object[] objArray = data as object[];
                    if (objArray != null)
                    {
                        foreach (object obj2 in objArray)
                        {
                            if (obj2 != null)
                            {
                                str = str + obj2.ToString() + " ";
                            }
                        }
                    }
                    sLogFile.WriteLine(DateTime.Now.ToLongTimeString() + " " + classification + ": \t" + message + " \tArgs: \t" + str);
                    sLogFile.Flush();
                }
                if (((sLog != null) && !sLog.Disposing) && !sLog.IsDisposed)
                {
                    try
                    {
                        sLog.BeginInvoke((VGen3)delegate (object innermessage, object innerclassification, object innerdata) {
                            try
                            {
                                LogData item = new LogData {
                                    LogType = innerclassification.ToString(),
                                    Description = innermessage.ToString()
                                };
                                item.SetData(innerdata);
                                sLog.mLogList.Add(item);
                            }
                            catch
                            {
                            }
                        }, new object[] { message, classification, data });
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        private void riPopup_Popup(object sender, EventArgs e)
        {
            int[] selectedRows = this.gvLogView.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                LogData row = this.gvLogView.GetRow(selectedRows[0]) as LogData;
                this.pgData.Rows.Clear();
                this.pgData.SelectedObject = row.Data;
            }
        }

        public static void ShowLogWindow()
        {
            sLog.Show();
        }
    }
}

