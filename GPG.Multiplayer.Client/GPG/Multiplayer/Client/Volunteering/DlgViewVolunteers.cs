namespace GPG.Multiplayer.Client.Volunteering
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgViewVolunteers : DlgBase
    {
        private MenuItem ciChat_ViewPlayer;
        private MenuItem ciExport;
        private MenuItem ciExport_HTML;
        private MenuItem ciExport_PDF;
        private MenuItem ciExport_Text;
        private MenuItem ciExport_XLS;
        private IContainer components = null;
        private GridColumn gcDate;
        private GridColumn gcDays;
        private GridColumn gcEffortName;
        private GridColumn gcEmail;
        private GridColumn gcPlayerName;
        private GridColumn gcTimes;
        private GPGContextMenu gpgContextMenuChat;
        private GPGDataGrid gpgDataGridVolunteers;
        private GPGLabel gpgLabel1;
        private GridView gvChannels;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemTextEdit repositoryItemTextEdit1;
        private Volunteer SelectedVolunteer = null;
        private SkinButton skinButtonRefresh;

        public DlgViewVolunteers()
        {
            this.InitializeComponent();
            this.RefreshData();
        }

        private void ciChat_ViewPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedVolunteer != null)
            {
                base.MainForm.OnViewPlayerProfile(this.SelectedVolunteer.PlayerName);
            }
        }

        private void ciExport_HTML_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>HTML Document (*.htm;*.html)|*.htm;*.html");
            dialog.InitialDirectory = @"C:\";
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Volunteers, {0}.{1}.{2}.htm", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.gpgDataGridVolunteers.ExportToHtml(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
            }
        }

        private void ciExport_PDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>PDF Document (*.pdf)|*.pdf");
            dialog.InitialDirectory = @"C:\";
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Volunteers, {0}.{1}.{2}.pdf", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.gpgDataGridVolunteers.ExportToPdf(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
            }
        }

        private void ciExport_Text_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>Text Document (*.txt)|*.txt");
            dialog.InitialDirectory = @"C:\";
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Volunteers, {0}.{1}.{2}.txt", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.gpgDataGridVolunteers.ExportToText(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
            }
        }

        private void ciExport_XLS_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Loc.Get("<LOC>Excel Spreadsheet (*.xls)|*.xls");
            dialog.InitialDirectory = @"C:\";
            dialog.OverwritePrompt = true;
            dialog.FileName = string.Format("Volunteers, {0}.{1}.{2}.xls", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.gpgDataGridVolunteers.ExportToXls(dialog.FileName);
                EventLog.WriteLine("{0} succesfully exported.", new object[] { dialog.FileName });
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

        private void gvChannels_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo info;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    (sender as GridView).Focus();
                    info = (sender as GridView).CalcHitInfo(e.Location);
                    if (info.InRow)
                    {
                        this.SelectedVolunteer = (sender as GridView).GetRow(info.RowHandle) as Volunteer;
                    }
                    break;

                case MouseButtons.Right:
                    (sender as GridView).Focus();
                    info = (sender as GridView).CalcHitInfo(e.Location);
                    if (info.InRow)
                    {
                        this.SelectedVolunteer = (sender as GridView).GetRow(info.RowHandle) as Volunteer;
                        int[] selectedRows = this.gvChannels.GetSelectedRows();
                        for (int i = 0; i < selectedRows.Length; i++)
                        {
                            this.gvChannels.UnselectRow(selectedRows[i]);
                        }
                        (sender as GridView).SelectRow(info.RowHandle);
                    }
                    break;
            }
            base.OnMouseDown(e);
        }

        private void gvChannels_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.gpgContextMenuChat.Show(this.gpgDataGridVolunteers, e.Location);
            }
        }

        private void InitializeComponent()
        {
            this.gpgDataGridVolunteers = new GPGDataGrid();
            this.gvChannels = new GridView();
            this.gcPlayerName = new GridColumn();
            this.gcEmail = new GridColumn();
            this.gcEffortName = new GridColumn();
            this.gcDays = new GridColumn();
            this.gcTimes = new GridColumn();
            this.gcDate = new GridColumn();
            this.repositoryItemComboBox1 = new RepositoryItemComboBox();
            this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonRefresh = new SkinButton();
            this.gpgContextMenuChat = new GPGContextMenu();
            this.ciExport = new MenuItem();
            this.ciExport_PDF = new MenuItem();
            this.ciExport_HTML = new MenuItem();
            this.ciExport_Text = new MenuItem();
            this.ciExport_XLS = new MenuItem();
            this.ciChat_ViewPlayer = new MenuItem();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgDataGridVolunteers.BeginInit();
            this.gvChannels.BeginInit();
            this.repositoryItemComboBox1.BeginInit();
            this.repositoryItemTextEdit1.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x20d, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgDataGridVolunteers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgDataGridVolunteers.CustomizeStyle = false;
            this.gpgDataGridVolunteers.EmbeddedNavigator.Name = "";
            this.gpgDataGridVolunteers.Location = new Point(12, 0x68);
            this.gpgDataGridVolunteers.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgDataGridVolunteers.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgDataGridVolunteers.MainView = this.gvChannels;
            this.gpgDataGridVolunteers.Name = "gpgDataGridVolunteers";
            this.gpgDataGridVolunteers.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemComboBox1, this.repositoryItemTextEdit1 });
            this.gpgDataGridVolunteers.ShowOnlyPredefinedDetails = true;
            this.gpgDataGridVolunteers.Size = new Size(560, 0x13d);
            this.gpgDataGridVolunteers.TabIndex = 10;
            this.gpgDataGridVolunteers.ViewCollection.AddRange(new BaseView[] { this.gvChannels });
            this.gvChannels.Appearance.Empty.BackColor = Color.Black;
            this.gvChannels.Appearance.Empty.Options.UseBackColor = true;
            this.gvChannels.Appearance.EvenRow.BackColor = Color.Black;
            this.gvChannels.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvChannels.Appearance.FocusedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvChannels.Appearance.FocusedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvChannels.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvChannels.Appearance.FocusedRow.Options.UseFont = true;
            this.gvChannels.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvChannels.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvChannels.Appearance.HideSelectionRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvChannels.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvChannels.Appearance.OddRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvChannels.Appearance.OddRow.Options.UseBackColor = true;
            this.gvChannels.Appearance.Preview.BackColor = Color.Black;
            this.gvChannels.Appearance.Preview.Options.UseBackColor = true;
            this.gvChannels.Appearance.Row.BackColor = Color.Black;
            this.gvChannels.Appearance.Row.ForeColor = Color.White;
            this.gvChannels.Appearance.Row.Options.UseBackColor = true;
            this.gvChannels.Appearance.RowSeparator.BackColor = Color.Black;
            this.gvChannels.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvChannels.Appearance.SelectedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvChannels.Appearance.SelectedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvChannels.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvChannels.Appearance.SelectedRow.Options.UseFont = true;
            this.gvChannels.AppearancePrint.Row.ForeColor = Color.White;
            this.gvChannels.AppearancePrint.Row.Options.UseForeColor = true;
            this.gvChannels.BestFitMaxRowCount = 1;
            this.gvChannels.BorderStyle = BorderStyles.NoBorder;
            this.gvChannels.ColumnPanelRowHeight = 30;
            this.gvChannels.Columns.AddRange(new GridColumn[] { this.gcPlayerName, this.gcEmail, this.gcEffortName, this.gcDays, this.gcTimes, this.gcDate });
            this.gvChannels.FocusRectStyle = DrawFocusRectStyle.None;
            this.gvChannels.GridControl = this.gpgDataGridVolunteers;
            this.gvChannels.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvChannels.Name = "gvChannels";
            this.gvChannels.OptionsBehavior.Editable = false;
            this.gvChannels.OptionsCustomization.AllowFilter = false;
            this.gvChannels.OptionsCustomization.AllowGroup = false;
            this.gvChannels.OptionsFilter.AllowFilterEditor = false;
            this.gvChannels.OptionsMenu.EnableColumnMenu = false;
            this.gvChannels.OptionsMenu.EnableFooterMenu = false;
            this.gvChannels.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvChannels.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvChannels.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvChannels.OptionsSelection.MultiSelect = true;
            this.gvChannels.OptionsView.EnableAppearanceEvenRow = true;
            this.gvChannels.OptionsView.EnableAppearanceOddRow = true;
            this.gvChannels.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            this.gvChannels.OptionsView.ShowGroupPanel = false;
            this.gvChannels.OptionsView.ShowHorzLines = false;
            this.gvChannels.OptionsView.ShowIndicator = false;
            this.gvChannels.OptionsView.ShowVertLines = false;
            this.gvChannels.MouseDown += new MouseEventHandler(this.gvChannels_MouseDown);
            this.gvChannels.MouseUp += new MouseEventHandler(this.gvChannels_MouseUp);
            this.gcPlayerName.Caption = "Name";
            this.gcPlayerName.FieldName = "PlayerName";
            this.gcPlayerName.Name = "gcPlayerName";
            this.gcPlayerName.Visible = true;
            this.gcPlayerName.VisibleIndex = 0;
            this.gcEmail.Caption = "Email";
            this.gcEmail.FieldName = "Email";
            this.gcEmail.Name = "gcEmail";
            this.gcEmail.Visible = true;
            this.gcEmail.VisibleIndex = 1;
            this.gcEffortName.Caption = "Effort Description";
            this.gcEffortName.FieldName = "EffortDescription";
            this.gcEffortName.Name = "gcEffortName";
            this.gcEffortName.Visible = true;
            this.gcEffortName.VisibleIndex = 2;
            this.gcDays.Caption = "Available Days";
            this.gcDays.FieldName = "AvailableDays";
            this.gcDays.Name = "gcDays";
            this.gcDays.Visible = true;
            this.gcDays.VisibleIndex = 3;
            this.gcTimes.Caption = "Available Times";
            this.gcTimes.FieldName = "AvailableTimes";
            this.gcTimes.Name = "gcTimes";
            this.gcTimes.Visible = true;
            this.gcTimes.VisibleIndex = 4;
            this.gcDate.Caption = "Volunteer Date";
            this.gcDate.FieldName = "VolunteerDate";
            this.gcDate.Name = "gcDate";
            this.gcDate.Visible = true;
            this.gcDate.VisibleIndex = 5;
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(13, 0x52);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(70, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 11;
            this.gpgLabel1.Text = "Volunteers";
            this.gpgLabel1.TextStyle = TextStyles.Header1;
            this.skinButtonRefresh.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonRefresh.AutoStyle = true;
            this.skinButtonRefresh.BackColor = Color.Black;
            this.skinButtonRefresh.DialogResult = DialogResult.OK;
            this.skinButtonRefresh.DisabledForecolor = Color.Gray;
            this.skinButtonRefresh.DrawEdges = true;
            this.skinButtonRefresh.FocusColor = Color.Yellow;
            this.skinButtonRefresh.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRefresh.ForeColor = Color.White;
            this.skinButtonRefresh.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRefresh.IsStyled = true;
            this.skinButtonRefresh.Location = new Point(0x1bf, 0x1b4);
            this.skinButtonRefresh.Name = "skinButtonRefresh";
            this.skinButtonRefresh.Size = new Size(0x7d, 0x1a);
            this.skinButtonRefresh.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRefresh, null);
            this.skinButtonRefresh.TabIndex = 12;
            this.skinButtonRefresh.Text = "Refresh";
            this.skinButtonRefresh.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRefresh.TextPadding = new Padding(0);
            this.skinButtonRefresh.Click += new EventHandler(this.skinButtonRefresh_Click);
            this.gpgContextMenuChat.MenuItems.AddRange(new MenuItem[] { this.ciExport, this.ciChat_ViewPlayer });
            this.ciExport.Index = 0;
            this.ciExport.MenuItems.AddRange(new MenuItem[] { this.ciExport_PDF, this.ciExport_HTML, this.ciExport_Text, this.ciExport_XLS });
            this.ciExport.Text = "<LOC>Export";
            this.ciExport_PDF.Index = 0;
            this.ciExport_PDF.Text = "<LOC>To PDF";
            this.ciExport_PDF.Click += new EventHandler(this.ciExport_PDF_Click);
            this.ciExport_HTML.Index = 1;
            this.ciExport_HTML.Text = "<LOC>To HTML";
            this.ciExport_HTML.Click += new EventHandler(this.ciExport_HTML_Click);
            this.ciExport_Text.Index = 2;
            this.ciExport_Text.Text = "<LOC>To Text";
            this.ciExport_Text.Click += new EventHandler(this.ciExport_Text_Click);
            this.ciExport_XLS.Index = 3;
            this.ciExport_XLS.Text = "<LOC>To XLS";
            this.ciExport_XLS.Click += new EventHandler(this.ciExport_XLS_Click);
            this.ciChat_ViewPlayer.Index = 1;
            this.ciChat_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChat_ViewPlayer.Click += new EventHandler(this.ciChat_ViewPlayer_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x248, 0x20d);
            base.Controls.Add(this.skinButtonRefresh);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgDataGridVolunteers);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgViewVolunteers";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "View Volunteers";
            base.Controls.SetChildIndex(this.gpgDataGridVolunteers, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonRefresh, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgDataGridVolunteers.EndInit();
            this.gvChannels.EndInit();
            this.repositoryItemComboBox1.EndInit();
            this.repositoryItemTextEdit1.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void Localize()
        {
            base.Localize();
            this.gpgContextMenuChat.Localize();
        }

        internal void RefreshData()
        {
            base.SetStatus("Refreshing...", new object[0]);
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                VGen0 method = null;
                MappedObjectList<Volunteer> volunteers = DataAccess.GetObjects<Volunteer>("ViewVolunteers", new object[0]);
                if (!base.Disposing && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.gpgDataGridVolunteers.DataSource = null;
                            this.gpgDataGridVolunteers.DataSource = volunteers;
                            this.ClearStatus();
                        };
                    }
                    base.BeginInvoke(method);
                }
            });
        }

        private void skinButtonRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshData();
        }
    }
}

