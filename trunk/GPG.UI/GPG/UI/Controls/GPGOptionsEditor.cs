namespace GPG.UI.Controls
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraVerticalGrid;
    using DevExpress.XtraVerticalGrid.Events;
    using DevExpress.XtraVerticalGrid.Rows;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    public class GPGOptionsEditor : UserControl, ILocalizable
    {
        private IContainer components;
        private RepositoryItemColorEdit editor_Color;
        private GPGLabel gpgLabelDescription;
        private Color mBorderColor;
        private ProgramSettings mOriginalSettings;
        private ProgramSettings mSettings;
        private GPGTreeView OptionsTree;
        private PropertyGridControl pgcProps;
        private RepositoryItemColorEdit repositoryItemColorEdit1;
        private RepositoryItemComboBox repositoryItemComboBox1;
        private RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private RepositoryItemTextEdit repositoryItemTextEdit1;
        private RepositoryItemTextEdit repositoryItemTextEdit2;
        private RepositoryItemTextEdit repositoryItemTextEdit3;
        private RepositoryItemComboBox riComboBox;
        private SplitContainer splitContainerGridAndLabel;
        private SplitContainer splitContainerMain;

        public event EventHandler PropertyChanged;

        public GPGOptionsEditor()
        {
            CellValueChangedEventHandler handler = null;
            this.mBorderColor = Color.White;
            this.InitializeComponent();
            this.riComboBox.CustomDisplayText += new ConvertEditValueEventHandler(this.riComboBox_CustomDisplayText);
            this.riComboBox.ParseEditValue += new ConvertEditValueEventHandler(this.riComboBox_ParseEditValue);
            this.riComboBox.Validating += new CancelEventHandler(this.riComboBox_Validating);
            this.OptionsTree.AfterSelect += new TreeViewEventHandler(this.OptionsTree_AfterSelect);
            if (handler == null)
            {
                handler = delegate (object sender, CellValueChangedEventArgs e) {
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, EventArgs.Empty);
                    }
                };
            }
            this.pgcProps.CellValueChanged += handler;
            this.pgcProps.FocusedRowChanged += new FocusedRowChangedEventHandler(this.pgcProps_FocusedRowChanged);
            this.gpgLabelDescription.AutoStyle = false;
            this.gpgLabelDescription.BackColor = Color.LightSteelBlue;
            this.gpgLabelDescription.ForeColor = Color.Black;
            this.gpgLabelDescription.Refresh();
        }

        private TreeNode BindToSettings(object root)
        {
            TreeNode[] childNodes = this.GetChildNodes(root, "Options.");
            TreeNode node = new TreeNode(Loc.Get("<LOC>Options"), childNodes);
            node.Expand();
            return node;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private TreeNode[] GetChildNodes(object root, string rootName)
        {
            List<TreeNode> list = new List<TreeNode>();
            foreach (PropertyInfo info in root.GetType().GetProperties())
            {
                object[] customAttributes = info.GetCustomAttributes(typeof(OptionsRootAttribute), false);
                if (customAttributes.Length > 0)
                {
                    object obj2 = info.GetValue(root, null);
                    if (obj2 != null)
                    {
                        string str = rootName + root.GetType().Name;
                        if (!ConfigSettings.GetBool("DISABLE" + str.ToUpper(), false))
                        {
                            TreeNode[] childNodes = this.GetChildNodes(obj2, str + ".");
                            TreeNode item = new TreeNode((customAttributes[0] as OptionsRootAttribute).Description, childNodes);
                            item.Name = str;
                            item.Tag = obj2;
                            list.Add(item);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        private void InitializeComponent()
        {
            this.splitContainerMain = new SplitContainer();
            this.OptionsTree = new GPGTreeView();
            this.splitContainerGridAndLabel = new SplitContainer();
            this.pgcProps = new PropertyGridControl();
            this.riComboBox = new RepositoryItemComboBox();
            this.repositoryItemColorEdit1 = new RepositoryItemColorEdit();
            this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
            this.repositoryItemLookUpEdit1 = new RepositoryItemLookUpEdit();
            this.repositoryItemComboBox1 = new RepositoryItemComboBox();
            this.repositoryItemTextEdit2 = new RepositoryItemTextEdit();
            this.editor_Color = new RepositoryItemColorEdit();
            this.repositoryItemTextEdit3 = new RepositoryItemTextEdit();
            this.gpgLabelDescription = new GPGLabel();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerGridAndLabel.Panel1.SuspendLayout();
            this.splitContainerGridAndLabel.Panel2.SuspendLayout();
            this.splitContainerGridAndLabel.SuspendLayout();
            this.pgcProps.BeginInit();
            this.riComboBox.BeginInit();
            this.repositoryItemColorEdit1.BeginInit();
            this.repositoryItemTextEdit1.BeginInit();
            this.repositoryItemLookUpEdit1.BeginInit();
            this.repositoryItemComboBox1.BeginInit();
            this.repositoryItemTextEdit2.BeginInit();
            this.editor_Color.BeginInit();
            this.repositoryItemTextEdit3.BeginInit();
            base.SuspendLayout();
            this.splitContainerMain.BackColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.splitContainerMain.Dock = DockStyle.Fill;
            this.splitContainerMain.Location = new Point(2, 2);
            this.splitContainerMain.Margin = new Padding(0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Panel1.Controls.Add(this.OptionsTree);
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerGridAndLabel);
            this.splitContainerMain.Size = new Size(0x1cf, 0x12f);
            this.splitContainerMain.SplitterDistance = 0x99;
            this.splitContainerMain.TabIndex = 1;
            this.OptionsTree.BackColor = Color.Black;
            this.OptionsTree.BorderStyle = BorderStyle.None;
            this.OptionsTree.Dock = DockStyle.Fill;
            this.OptionsTree.ForeColor = Color.White;
            this.OptionsTree.HideSelection = false;
            this.OptionsTree.Location = new Point(0, 0);
            this.OptionsTree.Margin = new Padding(0);
            this.OptionsTree.Name = "OptionsTree";
            this.OptionsTree.Size = new Size(0x99, 0x12f);
            this.OptionsTree.TabIndex = 0;
            this.splitContainerGridAndLabel.Dock = DockStyle.Fill;
            this.splitContainerGridAndLabel.Location = new Point(0, 0);
            this.splitContainerGridAndLabel.Name = "splitContainerGridAndLabel";
            this.splitContainerGridAndLabel.Orientation = Orientation.Horizontal;
            this.splitContainerGridAndLabel.Panel1.BackColor = Color.LightSteelBlue;
            this.splitContainerGridAndLabel.Panel1.Controls.Add(this.pgcProps);
            this.splitContainerGridAndLabel.Panel2.Controls.Add(this.gpgLabelDescription);
            this.splitContainerGridAndLabel.Size = new Size(0x132, 0x12f);
            this.splitContainerGridAndLabel.SplitterDistance = 0xe7;
            this.splitContainerGridAndLabel.SplitterWidth = 2;
            this.splitContainerGridAndLabel.TabIndex = 0;
            this.pgcProps.Appearance.Empty.BackColor = Color.LightSteelBlue;
            this.pgcProps.Appearance.Empty.Options.UseBackColor = true;
            this.pgcProps.AutoGenerateRows = true;
            this.pgcProps.BorderStyle = BorderStyles.NoBorder;
            this.pgcProps.DefaultEditors.AddRange(new DevExpress.XtraVerticalGrid.Rows.DefaultEditor[] { new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(null, this.riComboBox), new DevExpress.XtraVerticalGrid.Rows.DefaultEditor(typeof(Color), this.repositoryItemColorEdit1) });
            this.pgcProps.Dock = DockStyle.Fill;
            this.pgcProps.Location = new Point(0, 0);
            this.pgcProps.LookAndFeel.SkinName = "Money Twins";
            this.pgcProps.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pgcProps.Margin = new Padding(0);
            this.pgcProps.Name = "pgcProps";
            this.pgcProps.OptionsBehavior.UseEnterAsTab = true;
            this.pgcProps.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemTextEdit1, this.repositoryItemLookUpEdit1, this.repositoryItemComboBox1, this.riComboBox, this.repositoryItemTextEdit2, this.editor_Color, this.repositoryItemColorEdit1, this.repositoryItemTextEdit3 });
            this.pgcProps.Size = new Size(0x132, 0xe7);
            this.pgcProps.TabIndex = 4;
            this.pgcProps.RowChanging += new RowChangingEventHandler(this.pgcProps_RowChanging);
            this.riComboBox.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.riComboBox.Items.AddRange(new object[] { "None", "True", "False" });
            this.riComboBox.Name = "riComboBox";
            this.repositoryItemColorEdit1.AutoHeight = false;
            this.repositoryItemColorEdit1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemColorEdit1.Name = "repositoryItemColorEdit1";
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemTextEdit2.Appearance.Options.UseTextOptions = true;
            this.repositoryItemTextEdit2.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
            this.repositoryItemTextEdit2.AppearanceDisabled.Options.UseTextOptions = true;
            this.repositoryItemTextEdit2.AppearanceDisabled.TextOptions.HAlignment = HorzAlignment.Near;
            this.repositoryItemTextEdit2.AppearanceFocused.Options.UseTextOptions = true;
            this.repositoryItemTextEdit2.AppearanceFocused.TextOptions.HAlignment = HorzAlignment.Near;
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.DisplayFormat.FormatType = FormatType.Numeric;
            this.repositoryItemTextEdit2.EditFormat.FormatType = FormatType.Numeric;
            this.repositoryItemTextEdit2.HideSelection = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            this.editor_Color.AutoHeight = false;
            this.editor_Color.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.editor_Color.Name = "editor_Color";
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            this.gpgLabelDescription.AutoStyle = false;
            this.gpgLabelDescription.BackColor = Color.LightSteelBlue;
            this.gpgLabelDescription.Dock = DockStyle.Fill;
            this.gpgLabelDescription.Font = new Font("Verdana", 8f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelDescription.ForeColor = Color.Black;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(0, 0);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x132, 70);
            this.gpgLabelDescription.TabIndex = 0;
            this.gpgLabelDescription.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.splitContainerMain);
            base.Margin = new Padding(0);
            base.Name = "GPGOptionsEditor";
            base.Padding = new Padding(2);
            base.Size = new Size(0x1d3, 0x133);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerGridAndLabel.Panel1.ResumeLayout(false);
            this.splitContainerGridAndLabel.Panel2.ResumeLayout(false);
            this.splitContainerGridAndLabel.ResumeLayout(false);
            this.pgcProps.EndInit();
            this.riComboBox.EndInit();
            this.repositoryItemColorEdit1.EndInit();
            this.repositoryItemTextEdit1.EndInit();
            this.repositoryItemLookUpEdit1.EndInit();
            this.repositoryItemComboBox1.EndInit();
            this.repositoryItemTextEdit2.EndInit();
            this.editor_Color.EndInit();
            this.repositoryItemTextEdit3.EndInit();
            base.ResumeLayout(false);
        }

        public void Localize()
        {
            foreach (BaseRow row in this.PropertyGrid.Rows)
            {
                this.LocalizeRow(row);
            }
        }

        private void LocalizeRow(BaseRow row)
        {
            if (row.Properties.Caption.IndexOf("<LOC>") < 0)
            {
                row.Properties.Caption = "<LOC>" + row.Properties.Caption;
            }
            row.Properties.Caption = Loc.Get(row.Properties.Caption);
            foreach (BaseRow row2 in row.ChildRows)
            {
                this.LocalizeRow(row2);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.splitContainerMain.BackColor = this.BorderColor;
            this.splitContainerGridAndLabel.BackColor = this.BorderColor;
            base.OnPaint(e);
            using (Pen pen = new Pen(this.BorderColor, 2f))
            {
                int num = Convert.ToInt32(pen.Width);
                e.Graphics.DrawRectangle(pen, new Rectangle(num / 2, num / 2, base.Width - num, base.Height - num));
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        private void OptionsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((e != null) && (e.Node != null)) && (e.Node.Tag != null))
            {
                this.pgcProps.Rows.Clear();
                this.pgcProps.SelectedObject = null;
                this.pgcProps.SelectedObject = e.Node.Tag;
                this.Localize();
            }
        }

        private void pgcProps_CustomRecordCellEdit(object sender, GetCustomRowCellEditEventArgs e)
        {
            int recordIndex = e.RecordIndex;
            if (e.Row.Properties.RowType == typeof(bool?))
            {
                e.RepositoryItem = this.riComboBox;
            }
        }

        private void pgcProps_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if ((e.Row == null) || (this.pgcProps.SelectedObject == null))
                {
                    this.gpgLabelDescription.Text = Loc.Get("<LOC>No Description");
                }
                else
                {
                    PropertyInfo property = this.pgcProps.SelectedObject.GetType().GetProperty(e.Row.Properties.FieldName);
                    if (property != null)
                    {
                        object[] customAttributes = property.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (customAttributes.Length > 0)
                        {
                            string description = (customAttributes[0] as DescriptionAttribute).Description;
                            if (((description != null) && (description.Length > 0)) && (description != "<LOC>"))
                            {
                                this.gpgLabelDescription.Text = Loc.Get(description);
                            }
                            else
                            {
                                this.gpgLabelDescription.Text = Loc.Get("<LOC>No Description");
                            }
                        }
                        else
                        {
                            this.gpgLabelDescription.Text = Loc.Get("<LOC>No Description");
                        }
                    }
                    else
                    {
                        this.gpgLabelDescription.Text = Loc.Get("<LOC>No Description");
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void pgcProps_RowChanging(object sender, RowChangingEventArgs e)
        {
        }

        private void pgcProps_ShowingEditor(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        public override void Refresh()
        {
            if (this.OptionsTree.SelectedNode != null)
            {
                this.pgcProps.SelectedObject = null;
                this.pgcProps.SelectedObject = this.OptionsTree.SelectedNode.Tag;
            }
            base.Refresh();
        }

        private void riComboBox_CustomDisplayText(object sender, ConvertEditValueEventArgs e)
        {
            if (e.Value == null)
            {
                e.Value = "None";
                e.Handled = true;
            }
        }

        private void riComboBox_ParseEditValue(object sender, ConvertEditValueEventArgs e)
        {
            if ((e.Value != null) && (e.Value.ToString() == "None"))
            {
                e.Value = null;
                e.Handled = true;
            }
        }

        private void riComboBox_Validating(object sender, CancelEventArgs e)
        {
        }

        public Color BorderColor
        {
            get
            {
                return this.mBorderColor;
            }
            set
            {
                this.mBorderColor = value;
                this.Refresh();
            }
        }

        public ProgramSettings OriginalSettings
        {
            get
            {
                return this.mOriginalSettings;
            }
        }

        public PropertyGridControl PropertyGrid
        {
            get
            {
                return this.pgcProps;
            }
        }

        [Browsable(false)]
        public ProgramSettings Settings
        {
            get
            {
                return this.mSettings;
            }
            set
            {
                if (value != null)
                {
                    string key = null;
                    if (this.mSettings != null)
                    {
                        key = this.OptionsTree.SelectedNode.Name;
                    }
                    this.mOriginalSettings = value;
                    MemoryStream serializationStream = null;
                    try
                    {
                        serializationStream = new MemoryStream();
                        new BinaryFormatter().Serialize(serializationStream, this.OriginalSettings);
                        serializationStream.Position = 0L;
                        this.mSettings = new BinaryFormatter().Deserialize(serializationStream) as ProgramSettings;
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                    finally
                    {
                        if (serializationStream != null)
                        {
                            serializationStream.Close();
                        }
                    }
                    if (!File.Exists(this.mSettings.FilePath))
                    {
                        this.mSettings.Save();
                    }
                    this.OptionsTree.Nodes.Clear();
                    this.OptionsTree.AfterSelect -= new TreeViewEventHandler(this.OptionsTree_AfterSelect);
                    try
                    {
                        this.OptionsTree.Nodes.Add(this.BindToSettings(this.Settings));
                        if (key != null)
                        {
                            TreeNode[] nodeArray = this.OptionsTree.Nodes.Find(key, true);
                            if (nodeArray.Length > 0)
                            {
                                this.OptionsTree.SelectedNode = nodeArray[0];
                            }
                            else
                            {
                                this.OptionsTree.SelectedNode = this.OptionsTree.Nodes[0];
                            }
                        }
                        else
                        {
                            this.OptionsTree.SelectedNode = this.OptionsTree.Nodes[0];
                        }
                        this.pgcProps.SelectedObject = null;
                        this.pgcProps.SelectedObject = this.OptionsTree.SelectedNode.Tag;
                        this.OptionsTree.AfterSelect += new TreeViewEventHandler(this.OptionsTree_AfterSelect);
                    }
                    catch (Exception exception2)
                    {
                        ErrorLog.WriteLine(exception2);
                    }
                    this.Refresh();
                }
            }
        }

        public GPGTreeView TreeView
        {
            get
            {
                return this.OptionsTree;
            }
        }
    }
}

