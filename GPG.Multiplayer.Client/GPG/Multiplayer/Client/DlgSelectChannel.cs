namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgSelectChannel : DlgBase
    {
        private IContainer components;
        private GridColumn gcCreated;
        private GridColumn gcDescription;
        private GridColumn gcIsPublic;
        private GridColumn gcMaxPop;
        private GridColumn gcOwner;
        private GridColumn gcPassword;
        private GridColumn gcPopulation;
        private GPGDataGrid gpgDataGridRooms;
        private GPGGroupBox gpgGroupBox1;
        private GPGGroupBox gpgGroupBox2;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGTextBox gpgTextBoxChannel;
        private GridView gvChannels;
        private MappedObjectList<CustomRoom> mChannels;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemTextEdit repositoryItemTextEdit1;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private SkinButton skinButtonrefresh;

        public DlgSelectChannel() : this(Program.MainForm)
        {
        }

        public DlgSelectChannel(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gvChannels.Columns["IsPublic"].FilterInfo = new ColumnFilterInfo("[IsPublic] = true");
            this.gvChannels.DoubleClick += new EventHandler(this.gvChannels_DoubleClick);
            this.RefreshChannels();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gvChannels_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Value is bool)
            {
                if ((bool) e.Value)
                {
                    e.DisplayText = Loc.Get("<LOC>Yes");
                }
                else
                {
                    e.DisplayText = Loc.Get("<LOC>No");
                }
            }
        }

        private void gvChannels_DoubleClick(object sender, EventArgs e)
        {
            this.OnChannelSelect(true);
        }

        private void InitializeComponent()
        {
            this.gpgDataGridRooms = new GPGDataGrid();
            this.gvChannels = new GridView();
            this.gcDescription = new GridColumn();
            this.gcPopulation = new GridColumn();
            this.gcMaxPop = new GridColumn();
            this.gcPassword = new GridColumn();
            this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
            this.gcOwner = new GridColumn();
            this.gcCreated = new GridColumn();
            this.gcIsPublic = new GridColumn();
            this.repositoryItemComboBox1 = new RepositoryItemComboBox();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.skinButtonrefresh = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxChannel = new GPGTextBox();
            this.gpgGroupBox1 = new GPGGroupBox();
            this.gpgGroupBox2 = new GPGGroupBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgDataGridRooms.BeginInit();
            this.gvChannels.BeginInit();
            this.repositoryItemTextEdit1.BeginInit();
            this.repositoryItemComboBox1.BeginInit();
            this.gpgTextBoxChannel.Properties.BeginInit();
            this.gpgGroupBox1.SuspendLayout();
            this.gpgGroupBox2.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x25a, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgDataGridRooms.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgDataGridRooms.CustomizeStyle = false;
            this.gpgDataGridRooms.EmbeddedNavigator.Name = "";
            this.gpgDataGridRooms.Location = new Point(3, 0x31);
            this.gpgDataGridRooms.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgDataGridRooms.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgDataGridRooms.MainView = this.gvChannels;
            this.gpgDataGridRooms.Name = "gpgDataGridRooms";
            this.gpgDataGridRooms.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemComboBox1, this.repositoryItemTextEdit1 });
            this.gpgDataGridRooms.ShowOnlyPredefinedDetails = true;
            this.gpgDataGridRooms.Size = new Size(0x278, 0x83);
            this.gpgDataGridRooms.TabIndex = 9;
            this.gpgDataGridRooms.ViewCollection.AddRange(new BaseView[] { this.gvChannels });
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
            this.gvChannels.Columns.AddRange(new GridColumn[] { this.gcDescription, this.gcPopulation, this.gcMaxPop, this.gcPassword, this.gcOwner, this.gcCreated, this.gcIsPublic });
            this.gvChannels.FocusRectStyle = DrawFocusRectStyle.None;
            this.gvChannels.GridControl = this.gpgDataGridRooms;
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
            this.gvChannels.CustomColumnDisplayText += new CustomColumnDisplayTextEventHandler(this.gvChannels_CustomColumnDisplayText);
            this.gcDescription.AppearanceCell.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcDescription.AppearanceCell.ForeColor = Color.White;
            this.gcDescription.AppearanceCell.Options.UseFont = true;
            this.gcDescription.AppearanceCell.Options.UseForeColor = true;
            this.gcDescription.AppearanceCell.Options.UseTextOptions = true;
            this.gcDescription.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDescription.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcDescription.AppearanceHeader.Options.UseFont = true;
            this.gcDescription.Caption = "<LOC>Channel name";
            this.gcDescription.FieldName = "Description";
            this.gcDescription.Name = "gcDescription";
            this.gcDescription.OptionsColumn.AllowEdit = false;
            this.gcDescription.Visible = true;
            this.gcDescription.VisibleIndex = 0;
            this.gcDescription.Width = 0x8e;
            this.gcPopulation.AppearanceCell.ForeColor = Color.White;
            this.gcPopulation.AppearanceCell.Options.UseForeColor = true;
            this.gcPopulation.AppearanceCell.Options.UseTextOptions = true;
            this.gcPopulation.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcPopulation.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcPopulation.AppearanceHeader.Options.UseFont = true;
            this.gcPopulation.Caption = "<LOC>Players";
            this.gcPopulation.FieldName = "Population";
            this.gcPopulation.Name = "gcPopulation";
            this.gcPopulation.OptionsColumn.AllowEdit = false;
            this.gcPopulation.Visible = true;
            this.gcPopulation.VisibleIndex = 1;
            this.gcPopulation.Width = 0x40;
            this.gcMaxPop.AppearanceCell.ForeColor = Color.White;
            this.gcMaxPop.AppearanceCell.Options.UseForeColor = true;
            this.gcMaxPop.AppearanceCell.Options.UseTextOptions = true;
            this.gcMaxPop.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcMaxPop.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcMaxPop.AppearanceHeader.Options.UseFont = true;
            this.gcMaxPop.Caption = "<LOC>Max players";
            this.gcMaxPop.FieldName = "MaxPopulation";
            this.gcMaxPop.Name = "gcMaxPop";
            this.gcMaxPop.OptionsColumn.AllowEdit = false;
            this.gcMaxPop.Visible = true;
            this.gcMaxPop.VisibleIndex = 2;
            this.gcMaxPop.Width = 0x58;
            this.gcPassword.Caption = "<LOC>Password required";
            this.gcPassword.ColumnEdit = this.repositoryItemTextEdit1;
            this.gcPassword.FieldName = "PasswordProtected";
            this.gcPassword.GroupFormat.FormatString = "{0}";
            this.gcPassword.GroupFormat.FormatType = FormatType.Custom;
            this.gcPassword.Name = "gcPassword";
            this.gcPassword.OptionsColumn.AllowEdit = false;
            this.gcPassword.Visible = true;
            this.gcPassword.VisibleIndex = 3;
            this.gcPassword.Width = 0x8a;
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.gcOwner.AppearanceCell.ForeColor = Color.White;
            this.gcOwner.AppearanceCell.Options.UseForeColor = true;
            this.gcOwner.AppearanceCell.Options.UseTextOptions = true;
            this.gcOwner.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcOwner.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcOwner.AppearanceHeader.Options.UseFont = true;
            this.gcOwner.Caption = "<LOC>Channel operator";
            this.gcOwner.FieldName = "Owner";
            this.gcOwner.Name = "gcOwner";
            this.gcOwner.OptionsColumn.AllowEdit = false;
            this.gcOwner.Visible = true;
            this.gcOwner.VisibleIndex = 4;
            this.gcOwner.Width = 0x79;
            this.gcCreated.AppearanceCell.ForeColor = Color.White;
            this.gcCreated.AppearanceCell.Options.UseForeColor = true;
            this.gcCreated.AppearanceCell.Options.UseTextOptions = true;
            this.gcCreated.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcCreated.AppearanceHeader.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcCreated.AppearanceHeader.Options.UseFont = true;
            this.gcCreated.Caption = "<LOC>Created";
            this.gcCreated.FieldName = "Created";
            this.gcCreated.Name = "gcCreated";
            this.gcCreated.OptionsColumn.AllowEdit = false;
            this.gcCreated.Visible = true;
            this.gcCreated.VisibleIndex = 5;
            this.gcCreated.Width = 0x4f;
            this.gcIsPublic.Caption = "IsPublic";
            this.gcIsPublic.FieldName = "IsPublic";
            this.gcIsPublic.Name = "gcIsPublic";
            this.gcIsPublic.OptionsColumn.ShowInCustomizationForm = false;
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x22b, 0x162);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5e, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 10;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawColor = Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x1c7, 0x162);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5e, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 11;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>Join";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonrefresh.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonrefresh.AutoStyle = true;
            this.skinButtonrefresh.BackColor = Color.Black;
            this.skinButtonrefresh.ButtonState = 0;
            this.skinButtonrefresh.DialogResult = DialogResult.OK;
            this.skinButtonrefresh.DisabledForecolor = Color.Gray;
            this.skinButtonrefresh.DrawColor = Color.White;
            this.skinButtonrefresh.DrawEdges = true;
            this.skinButtonrefresh.FocusColor = Color.Yellow;
            this.skinButtonrefresh.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonrefresh.ForeColor = Color.White;
            this.skinButtonrefresh.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonrefresh.IsStyled = true;
            this.skinButtonrefresh.Location = new Point(0x163, 0x162);
            this.skinButtonrefresh.Name = "skinButtonrefresh";
            this.skinButtonrefresh.Size = new Size(0x5e, 0x1a);
            this.skinButtonrefresh.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonrefresh, null);
            this.skinButtonrefresh.TabIndex = 12;
            this.skinButtonrefresh.TabStop = true;
            this.skinButtonrefresh.Text = "<LOC>Refresh";
            this.skinButtonrefresh.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonrefresh.TextPadding = new Padding(0);
            this.skinButtonrefresh.Click += new EventHandler(this.skinButtonrefresh_Click);
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(6, 30);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xcf, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 13;
            this.gpgLabel1.Text = "<LOC>Select an available channel";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(3, 30);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x16c, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 14;
            this.gpgLabel2.Text = "<LOC>Enter a private channel name, or create a new channel";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgTextBoxChannel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextBoxChannel.Location = new Point(6, 50);
            this.gpgTextBoxChannel.Name = "gpgTextBoxChannel";
            this.gpgTextBoxChannel.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxChannel.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxChannel.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxChannel.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxChannel.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxChannel.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxChannel.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxChannel.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxChannel.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxChannel.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxChannel.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxChannel.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxChannel.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxChannel.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxChannel.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxChannel.Properties.MaxLength = 60;
            this.gpgTextBoxChannel.Size = new Size(0x271, 20);
            this.gpgTextBoxChannel.TabIndex = 15;
            this.gpgGroupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgGroupBox1.Controls.Add(this.gpgLabel2);
            this.gpgGroupBox1.Controls.Add(this.gpgTextBoxChannel);
            this.gpgGroupBox1.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgGroupBox1.Location = new Point(12, 0x109);
            this.gpgGroupBox1.Name = "gpgGroupBox1";
            this.gpgGroupBox1.Size = new Size(0x27d, 0x4c);
            base.ttDefault.SetSuperTip(this.gpgGroupBox1, null);
            this.gpgGroupBox1.TabIndex = 0x10;
            this.gpgGroupBox1.TabStop = false;
            this.gpgGroupBox1.Text = "<LOC>Private Channel";
            this.gpgGroupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGroupBox2.Controls.Add(this.gpgLabel1);
            this.gpgGroupBox2.Controls.Add(this.gpgDataGridRooms);
            this.gpgGroupBox2.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgGroupBox2.Location = new Point(12, 0x4c);
            this.gpgGroupBox2.Name = "gpgGroupBox2";
            this.gpgGroupBox2.Size = new Size(0x27e, 0xb7);
            base.ttDefault.SetSuperTip(this.gpgGroupBox2, null);
            this.gpgGroupBox2.TabIndex = 0x11;
            this.gpgGroupBox2.TabStop = false;
            this.gpgGroupBox2.Text = "<LOC>Public Channel";
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x295, 0x1bb);
            base.Controls.Add(this.gpgGroupBox2);
            base.Controls.Add(this.gpgGroupBox1);
            base.Controls.Add(this.skinButtonrefresh);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(320, 0x178);
            base.Name = "DlgSelectChannel";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Select Channel";
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonrefresh, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox1, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox2, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgDataGridRooms.EndInit();
            this.gvChannels.EndInit();
            this.repositoryItemTextEdit1.EndInit();
            this.repositoryItemComboBox1.EndInit();
            this.gpgTextBoxChannel.Properties.EndInit();
            this.gpgGroupBox1.ResumeLayout(false);
            this.gpgGroupBox1.PerformLayout();
            this.gpgGroupBox2.ResumeLayout(false);
            this.gpgGroupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void OnChannelSelect(bool fromList)
        {
            if (fromList)
            {
                int[] selectedRows = this.gvChannels.GetSelectedRows();
                if (selectedRows.Length > 0)
                {
                    CustomRoom row = this.gvChannels.GetRow(selectedRows[0]) as CustomRoom;
                    if (row != null)
                    {
                        base.MainForm.JoinChat(row.Description);
                    }
                }
            }
            else
            {
                base.MainForm.CreateChannelIfNonExist(this.gpgTextBoxChannel.Text);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            KeyEventHandler handler = null;
            base.OnLoad(e);
            this.gpgTextBoxChannel.Select();
            foreach (Control control in base.Controls)
            {
                if (handler == null)
                {
                    handler = delegate (object s, KeyEventArgs ke) {
                        if (ke.KeyCode == Keys.F5)
                        {
                            this.RefreshChannels();
                        }
                    };
                }
                control.KeyDown += handler;
            }
        }

        private void RefreshChannels()
        {
            VGen0 target = null;
            VGen0 callback = null;
            try
            {
                base.SetStatus("<LOC>Refreshing List...", new object[0]);
                this.skinButtonOK.Enabled = false;
                this.skinButtonrefresh.Enabled = false;
                if (target == null)
                {
                    target = delegate {
                        try
                        {
                            this.mChannels = DataAccess.GetObjects<CustomRoom>("GetCustomChannels", new object[0]);
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                if (callback == null)
                {
                    callback = delegate {
                        VGen0 method = null;
                        try
                        {
                            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.gpgDataGridRooms.DataSource = null;
                                        this.gpgDataGridRooms.DataSource = this.Channels;
                                        this.skinButtonOK.Enabled = true;
                                        this.skinButtonrefresh.Enabled = true;
                                    };
                                }
                                base.Invoke(method);
                            }
                            else if (!(base.Disposing || base.IsDisposed))
                            {
                                this.gpgDataGridRooms.DataSource = null;
                                this.gpgDataGridRooms.DataSource = this.Channels;
                                this.skinButtonOK.Enabled = true;
                                this.skinButtonrefresh.Enabled = true;
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                        finally
                        {
                            base.ClearStatus();
                        }
                    };
                }
                ThreadQueue.Quazal.Enqueue(target, callback, new object[0]);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                base.ClearStatus();
            }
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            this.OnChannelSelect((this.gpgTextBoxChannel.Text == null) || (this.gpgTextBoxChannel.Text.Length < 1));
        }

        private void skinButtonrefresh_Click(object sender, EventArgs e)
        {
            this.RefreshChannels();
        }

        public MappedObjectList<CustomRoom> Channels
        {
            get
            {
                return this.mChannels;
            }
        }
    }
}

