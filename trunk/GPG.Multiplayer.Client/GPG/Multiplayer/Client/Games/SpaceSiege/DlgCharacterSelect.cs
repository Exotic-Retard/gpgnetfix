namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using DevExpress.Data;
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgCharacterSelect : DlgBase
    {
        private IContainer components;
        private GridColumn gcName;
        private GridColumn gcTimePlayed;
        private GridColumn gcUpgrades;
        private GPGDataGrid gpgDataGridCharacters;
        private GPGLabel gpgLabelLoading;
        private GPGPanel gpgPanelCharacters;
        private GridView gvCharacters;
        private PlayerCharacter mSelCharacter;
        private RasterAnimation rasterAnimationLoading;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemTextEdit repositoryItemTextEdit1;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonDeleteCharacter;
        private SkinButton skinButtonNewCharacter;
        private SkinButton skinButtonOK;

        public event StringEventHandler CharacterDeleted;

        public DlgCharacterSelect()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public DlgCharacterSelect(SpaceSiegeGameParticipant participant)
        {
            this.components = null;
            this.InitializeComponent();
            this.gvCharacters.SelectionChanged += new SelectionChangedEventHandler(this.gvCharacters_SelectionChanged);
            this.gvCharacters.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gvCharacters_FocusedRowChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgDataGridCharacters_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectedCharacter != null)
            {
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void gvCharacters_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            int[] selectedRows = this.gvCharacters.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                this.SelectedCharacter = this.gvCharacters.GetRow(this.gvCharacters.GetRowHandle(selectedRows[0])) as PlayerCharacter;
                this.RefreshCharacterDisplay();
                this.skinButtonDeleteCharacter.Enabled = true;
                this.skinButtonOK.Enabled = true;
            }
            else
            {
                this.SelectedCharacter = null;
                this.skinButtonDeleteCharacter.Enabled = false;
                this.skinButtonOK.Enabled = false;
            }
        }

        private void gvCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int[] selectedRows = this.gvCharacters.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                this.SelectedCharacter = this.gvCharacters.GetRow(this.gvCharacters.GetRowHandle(selectedRows[0])) as PlayerCharacter;
                this.RefreshCharacterDisplay();
                this.skinButtonDeleteCharacter.Enabled = true;
                this.skinButtonOK.Enabled = true;
            }
            else
            {
                this.SelectedCharacter = null;
                this.skinButtonDeleteCharacter.Enabled = false;
                this.skinButtonOK.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgCharacterSelect));
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.skinButtonNewCharacter = new SkinButton();
            this.skinButtonDeleteCharacter = new SkinButton();
            this.rasterAnimationLoading = new RasterAnimation();
            this.repositoryItemComboBox1 = new RepositoryItemComboBox();
            this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
            this.gpgPanelCharacters = new GPGPanel();
            this.gpgLabelLoading = new GPGLabel();
            this.gpgDataGridCharacters = new GPGDataGrid();
            this.gvCharacters = new GridView();
            this.gcName = new GridColumn();
            this.gcTimePlayed = new GridColumn();
            this.gcUpgrades = new GridColumn();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            ((ISupportInitialize) this.rasterAnimationLoading).BeginInit();
            this.repositoryItemComboBox1.BeginInit();
            this.repositoryItemTextEdit1.BeginInit();
            this.gpgPanelCharacters.SuspendLayout();
            this.gpgDataGridCharacters.BeginInit();
            this.gvCharacters.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x29c, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Transparent;
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
            this.skinButtonCancel.Location = new Point(0x25b, 0x1db);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x6f, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 11;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Transparent;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawColor = Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.Enabled = false;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x1e6, 0x1db);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x6f, 0x17);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 12;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Paint += new PaintEventHandler(this.skinButtonOK_Paint);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonNewCharacter.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonNewCharacter.AutoStyle = true;
            this.skinButtonNewCharacter.BackColor = Color.Transparent;
            this.skinButtonNewCharacter.ButtonState = 0;
            this.skinButtonNewCharacter.DialogResult = DialogResult.OK;
            this.skinButtonNewCharacter.DisabledForecolor = Color.Gray;
            this.skinButtonNewCharacter.DrawColor = Color.White;
            this.skinButtonNewCharacter.DrawEdges = true;
            this.skinButtonNewCharacter.FocusColor = Color.Yellow;
            this.skinButtonNewCharacter.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNewCharacter.ForeColor = Color.White;
            this.skinButtonNewCharacter.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNewCharacter.IsStyled = true;
            this.skinButtonNewCharacter.Location = new Point(14, 0x1db);
            this.skinButtonNewCharacter.Name = "skinButtonNewCharacter";
            this.skinButtonNewCharacter.Size = new Size(0xaf, 0x17);
            this.skinButtonNewCharacter.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonNewCharacter, null);
            this.skinButtonNewCharacter.TabIndex = 14;
            this.skinButtonNewCharacter.TabStop = true;
            this.skinButtonNewCharacter.Text = "<LOC>Create New Character";
            this.skinButtonNewCharacter.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNewCharacter.TextPadding = new Padding(0);
            this.skinButtonNewCharacter.Click += new EventHandler(this.skinButtonNewCharacter_Click);
            this.skinButtonDeleteCharacter.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonDeleteCharacter.AutoStyle = true;
            this.skinButtonDeleteCharacter.BackColor = Color.Transparent;
            this.skinButtonDeleteCharacter.ButtonState = 0;
            this.skinButtonDeleteCharacter.DialogResult = DialogResult.OK;
            this.skinButtonDeleteCharacter.DisabledForecolor = Color.Gray;
            this.skinButtonDeleteCharacter.DrawColor = Color.White;
            this.skinButtonDeleteCharacter.DrawEdges = true;
            this.skinButtonDeleteCharacter.Enabled = false;
            this.skinButtonDeleteCharacter.FocusColor = Color.Yellow;
            this.skinButtonDeleteCharacter.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDeleteCharacter.ForeColor = Color.White;
            this.skinButtonDeleteCharacter.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDeleteCharacter.IsStyled = true;
            this.skinButtonDeleteCharacter.Location = new Point(0xc3, 0x1db);
            this.skinButtonDeleteCharacter.Name = "skinButtonDeleteCharacter";
            this.skinButtonDeleteCharacter.Size = new Size(0xa2, 0x17);
            this.skinButtonDeleteCharacter.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonDeleteCharacter, null);
            this.skinButtonDeleteCharacter.TabIndex = 15;
            this.skinButtonDeleteCharacter.TabStop = true;
            this.skinButtonDeleteCharacter.Text = "<LOC>Delete Character";
            this.skinButtonDeleteCharacter.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDeleteCharacter.TextPadding = new Padding(0);
            this.skinButtonDeleteCharacter.Click += new EventHandler(this.skinButtonDeleteCharacter_Click);
            this.rasterAnimationLoading.AutoRun = true;
            this.rasterAnimationLoading.Duration = 0f;
            this.rasterAnimationLoading.Image = (Image) manager.GetObject("rasterAnimationLoading.Image");
            this.rasterAnimationLoading.ImagePath = @"D:\art\progress_spinner";
            this.rasterAnimationLoading.Location = new Point(0xa8, 0x9c);
            this.rasterAnimationLoading.Loop = true;
            this.rasterAnimationLoading.Name = "rasterAnimationLoading";
            this.rasterAnimationLoading.Size = new Size(15, 15);
            this.rasterAnimationLoading.SizeMode = PictureBoxSizeMode.AutoSize;
            base.ttDefault.SetSuperTip(this.rasterAnimationLoading, null);
            this.rasterAnimationLoading.TabIndex = 0x11;
            this.rasterAnimationLoading.TabStop = false;
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo), new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.gpgPanelCharacters.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelCharacters.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelCharacters.BorderThickness = 2;
            this.gpgPanelCharacters.Controls.Add(this.gpgLabelLoading);
            this.gpgPanelCharacters.Controls.Add(this.gpgDataGridCharacters);
            this.gpgPanelCharacters.DrawBorder = true;
            this.gpgPanelCharacters.Location = new Point(12, 0x53);
            this.gpgPanelCharacters.Name = "gpgPanelCharacters";
            this.gpgPanelCharacters.Size = new Size(0x2bf, 0x182);
            base.ttDefault.SetSuperTip(this.gpgPanelCharacters, null);
            this.gpgPanelCharacters.TabIndex = 0x13;
            this.gpgLabelLoading.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelLoading.AutoStyle = true;
            this.gpgLabelLoading.BackColor = Color.Transparent;
            this.gpgLabelLoading.Dock = DockStyle.Fill;
            this.gpgLabelLoading.Font = new Font("Arial", 9.75f);
            this.gpgLabelLoading.ForeColor = Color.White;
            this.gpgLabelLoading.IgnoreMouseWheel = false;
            this.gpgLabelLoading.IsStyled = false;
            this.gpgLabelLoading.Location = new Point(0, 0);
            this.gpgLabelLoading.Name = "gpgLabelLoading";
            this.gpgLabelLoading.Size = new Size(0x2bf, 0x182);
            base.ttDefault.SetSuperTip(this.gpgLabelLoading, null);
            this.gpgLabelLoading.TabIndex = 20;
            this.gpgLabelLoading.Text = "<LOC>Loading Characters";
            this.gpgLabelLoading.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelLoading.TextStyle = TextStyles.Default;
            this.gpgDataGridCharacters.CustomizeStyle = false;
            this.gpgDataGridCharacters.Dock = DockStyle.Fill;
            this.gpgDataGridCharacters.EmbeddedNavigator.Name = "";
            this.gpgDataGridCharacters.Location = new Point(0, 0);
            this.gpgDataGridCharacters.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgDataGridCharacters.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgDataGridCharacters.MainView = this.gvCharacters;
            this.gpgDataGridCharacters.Name = "gpgDataGridCharacters";
            this.gpgDataGridCharacters.ShowOnlyPredefinedDetails = true;
            this.gpgDataGridCharacters.Size = new Size(0x2bf, 0x182);
            this.gpgDataGridCharacters.TabIndex = 0x13;
            this.gpgDataGridCharacters.ViewCollection.AddRange(new BaseView[] { this.gvCharacters });
            this.gvCharacters.Appearance.Empty.BackColor = Color.Transparent;
            this.gvCharacters.Appearance.Empty.Options.UseBackColor = true;
            this.gvCharacters.Appearance.EvenRow.BackColor = Color.Black;
            this.gvCharacters.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvCharacters.Appearance.FocusedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvCharacters.Appearance.FocusedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvCharacters.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCharacters.Appearance.FocusedRow.Options.UseFont = true;
            this.gvCharacters.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvCharacters.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCharacters.Appearance.HideSelectionRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvCharacters.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCharacters.Appearance.OddRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvCharacters.Appearance.OddRow.Options.UseBackColor = true;
            this.gvCharacters.Appearance.Preview.BackColor = Color.Black;
            this.gvCharacters.Appearance.Preview.Options.UseBackColor = true;
            this.gvCharacters.Appearance.Row.BackColor = Color.Black;
            this.gvCharacters.Appearance.Row.ForeColor = Color.White;
            this.gvCharacters.Appearance.Row.Options.UseBackColor = true;
            this.gvCharacters.Appearance.RowSeparator.BackColor = Color.Black;
            this.gvCharacters.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvCharacters.Appearance.SelectedRow.BackColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gvCharacters.Appearance.SelectedRow.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gvCharacters.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCharacters.Appearance.SelectedRow.Options.UseFont = true;
            this.gvCharacters.AppearancePrint.Row.ForeColor = Color.White;
            this.gvCharacters.AppearancePrint.Row.Options.UseForeColor = true;
            this.gvCharacters.BestFitMaxRowCount = 1;
            this.gvCharacters.BorderStyle = BorderStyles.NoBorder;
            this.gvCharacters.ColumnPanelRowHeight = 30;
            this.gvCharacters.Columns.AddRange(new GridColumn[] { this.gcName, this.gcTimePlayed, this.gcUpgrades });
            this.gvCharacters.FocusRectStyle = DrawFocusRectStyle.None;
            this.gvCharacters.GridControl = this.gpgDataGridCharacters;
            this.gvCharacters.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvCharacters.Name = "gvCharacters";
            this.gvCharacters.OptionsBehavior.Editable = false;
            this.gvCharacters.OptionsCustomization.AllowFilter = false;
            this.gvCharacters.OptionsCustomization.AllowGroup = false;
            this.gvCharacters.OptionsFilter.AllowFilterEditor = false;
            this.gvCharacters.OptionsMenu.EnableColumnMenu = false;
            this.gvCharacters.OptionsMenu.EnableFooterMenu = false;
            this.gvCharacters.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvCharacters.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvCharacters.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gvCharacters.OptionsView.EnableAppearanceEvenRow = true;
            this.gvCharacters.OptionsView.EnableAppearanceOddRow = true;
            this.gvCharacters.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            this.gvCharacters.OptionsView.ShowGroupPanel = false;
            this.gvCharacters.OptionsView.ShowHorzLines = false;
            this.gvCharacters.OptionsView.ShowIndicator = false;
            this.gvCharacters.OptionsView.ShowVertLines = false;
            this.gvCharacters.SelectionChanged += new SelectionChangedEventHandler(this.gvCharacters_SelectionChanged);
            this.gcName.Caption = "<LOC>Name";
            this.gcName.FieldName = "CharacterName";
            this.gcName.Name = "gcName";
            this.gcName.Visible = true;
            this.gcName.VisibleIndex = 0;
            this.gcName.Width = 0x9c;
            this.gcTimePlayed.AppearanceCell.Options.UseTextOptions = true;
            this.gcTimePlayed.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            this.gcTimePlayed.Caption = "<LOC id=_70d529695c253d17e992cb9265abc57f>Missions Completed";
            this.gcTimePlayed.FieldName = "TimePlayed";
            this.gcTimePlayed.Name = "gcTimePlayed";
            this.gcTimePlayed.Visible = true;
            this.gcTimePlayed.VisibleIndex = 2;
            this.gcTimePlayed.Width = 0x6d;
            this.gcUpgrades.AppearanceCell.Options.UseTextOptions = true;
            this.gcUpgrades.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            this.gcUpgrades.Caption = "<LOC>Upgrades";
            this.gcUpgrades.FieldName = "Upgrades";
            this.gcUpgrades.Name = "gcUpgrades";
            this.gcUpgrades.Visible = true;
            this.gcUpgrades.VisibleIndex = 1;
            this.gcUpgrades.Width = 0x63;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x2d7, 0x231);
            base.Controls.Add(this.rasterAnimationLoading);
            base.Controls.Add(this.gpgPanelCharacters);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonNewCharacter);
            base.Controls.Add(this.skinButtonDeleteCharacter);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x2d7, 0x231);
            this.MinimumSize = new Size(0x2d7, 0x231);
            base.Name = "DlgCharacterSelect";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Character Select";
            base.Controls.SetChildIndex(this.skinButtonDeleteCharacter, 0);
            base.Controls.SetChildIndex(this.skinButtonNewCharacter, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.gpgPanelCharacters, 0);
            base.Controls.SetChildIndex(this.rasterAnimationLoading, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            ((ISupportInitialize) this.rasterAnimationLoading).EndInit();
            this.repositoryItemComboBox1.EndInit();
            this.repositoryItemTextEdit1.EndInit();
            this.gpgPanelCharacters.ResumeLayout(false);
            this.gpgDataGridCharacters.EndInit();
            this.gvCharacters.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ReloadCharacters();
        }

        private void RefreshCharacterDisplay()
        {
        }

        private void ReloadCharacters()
        {
            this.gvCharacters.ClearSelection();
            this.gpgPanelCharacters.Visible = false;
            this.rasterAnimationLoading.Visible = true;
            this.gpgLabelLoading.Visible = true;
            this.gpgLabelLoading.Text = Loc.Get("<LOC>Loading Characters");
            ThreadPool.QueueUserWorkItem(delegate (object state) {
                VGen0 method = null;
                VGen0 gen2 = null;
                if (!Directory.Exists(this.CharacterFilePath))
                {
                    Directory.CreateDirectory(this.CharacterFilePath);
                }
                List<PlayerCharacter> characters = new List<PlayerCharacter>();
                if (File.Exists(this.CharacterFilePath + @"\characters.gpgnet"))
                {
                    StreamReader reader = new StreamReader(this.CharacterFilePath + @"\characters.gpgnet");
                    string str2 = reader.ReadToEnd();
                    reader.Close();
                    foreach (string str3 in str2.Split(new char[] { '\x000e' }))
                    {
                        if (str3.Length > 0)
                        {
                            characters.Add(PlayerCharacter.FromString(str3));
                        }
                    }
                }
                if (characters.Count > 0)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.rasterAnimationLoading.Visible = false;
                            this.gpgLabelLoading.Visible = false;
                            this.gpgPanelCharacters.Visible = true;
                            this.gpgDataGridCharacters.DataSource = null;
                            this.gpgDataGridCharacters.DataSource = characters;
                            this.gvCharacters.RefreshData();
                            this.gvCharacters.FocusedRowHandle = this.gvCharacters.GetRowHandle(0);
                            this.gvCharacters_FocusedRowChanged(null, null);
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    if (gen2 == null)
                    {
                        gen2 = delegate {
                            this.rasterAnimationLoading.Visible = false;
                            this.gpgLabelLoading.Text = Loc.Get("<LOC>You have no characters created, click New Character to create one.");
                        };
                    }
                    base.Invoke(gen2);
                }
            });
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonDeleteCharacter_Click(object sender, EventArgs e)
        {
            if (this.SelectedCharacter != null)
            {
                DialogResult result;
                string str = DlgAskQuestion.AskQuestion(base.MainForm, "<LOC>To confirm the deletion of this character please enter the characters name (case sensitive)", "<LOC>Confirm", false, out result);
                if (result == DialogResult.OK)
                {
                    if (str == this.SelectedCharacter.CharacterName)
                    {
                        try
                        {
                            string path = PlayerCharacter.CharacterFilePath + @"\characters.gpgnet";
                            if (File.Exists(path))
                            {
                                StreamReader reader = new StreamReader(path);
                                string str3 = reader.ReadToEnd();
                                reader.Close();
                                StreamWriter writer = new StreamWriter(path);
                                foreach (string str4 in str3.Split(new char[] { '\x000e' }))
                                {
                                    if (str4.IndexOf(str + ",") != 0)
                                    {
                                        writer.Write(str4 + "\x000e");
                                    }
                                }
                                writer.Close();
                                if (ConfigSettings.GetBool("DeleteSSMPFiles", true))
                                {
                                    foreach (string str5 in Directory.GetFiles(PlayerCharacter.CharacterFilePath, str + ".ss1mpsave"))
                                    {
                                        File.Delete(str5);
                                    }
                                }
                            }
                            if (this.CharacterDeleted != null)
                            {
                                this.CharacterDeleted(this.SelectedCharacter.CharacterName);
                            }
                            this.ReloadCharacters();
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                            DlgMessage.ShowDialog("<LOC>An error occured while deleting character.", "<LOC>Error");
                        }
                    }
                    else
                    {
                        DlgMessage.ShowDialog("<LOC>The name you entered does not match the characters name, deletion cancelled.", "<LOC>Error");
                    }
                }
            }
        }

        private void skinButtonEdit_Click(object sender, EventArgs e)
        {
            if ((this.SelectedCharacter != null) && (new DlgCreateCharacter(this.SelectedCharacter).ShowDialog() == DialogResult.OK))
            {
                this.ReloadCharacters();
            }
        }

        private void skinButtonNewCharacter_Click(object sender, EventArgs e)
        {
            if (new DlgCreateCharacter().ShowDialog() == DialogResult.OK)
            {
                this.ReloadCharacters();
            }
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void skinButtonOK_Paint(object sender, PaintEventArgs e)
        {
        }

        private string CharacterFilePath
        {
            get
            {
                return PlayerCharacter.CharacterFilePath;
            }
        }

        public PlayerCharacter SelectedCharacter
        {
            get
            {
                return this.mSelCharacter;
            }
            set
            {
                this.mSelCharacter = value;
                if (this.mSelCharacter == null)
                {
                    GPGnetSelectedGame.ProfileName = User.Current.Name;
                }
                else
                {
                    GPGnetSelectedGame.ProfileName = this.mSelCharacter.CharacterName;
                }
            }
        }
    }
}

