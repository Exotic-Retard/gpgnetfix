namespace GPG.Multiplayer.Client.SolutionsLib
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.SolutionsLib;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgSearchResults : DlgBase
    {
        private MenuItem ciKeywords_AppendSearch;
        private MenuItem ciKeywords_NewSearch;
        private IContainer components;
        private LinkedList<SearchResultEntry> EntryList;
        private GPGContextMenu gpgContextMenuKeywords;
        private GPGGroupBox gpgGroupBox1;
        private GPGGroupBox gpgGroupBox2;
        private GPGLabel gpgLabelResults;
        private GPGPanel gpgPanelResults;
        private ListBox listBoxKeywords;
        private Solution[] mResultSet;
        private SearchResultEntry mSelectedEntry;
        private GPG.Multiplayer.Quazal.SolutionsLib.Service mService;
        private SkinButton skinButtonClose;
        private SkinButton skinButtonLast;
        private SkinButton skinButtonNext;

        public DlgSearchResults() : this(Program.MainForm, null)
        {
            this.InitializeComponent();
        }

        public DlgSearchResults(FrmMain mainForm, Solution[] results) : base(mainForm)
        {
            this.EntryList = new LinkedList<SearchResultEntry>();
            this.components = null;
            this.InitializeComponent();
            this.mService = new GPG.Multiplayer.Quazal.SolutionsLib.Service();
            this.mService.Url = ConfigSettings.GetString("SolutionsLibService", "http://gpgnet.gaspowered.com/quazal/Service.asmx?WSDL");
            this.mResultSet = results;
        }

        private void ciKeywords_AppendSearch_Click(object sender, EventArgs e)
        {
            string search = "";
            foreach (string str2 in this.listBoxKeywords.SelectedItems)
            {
                search = search + str2 + " ";
            }
            search = search.TrimEnd(" ".ToCharArray());
            base.MainForm.ShowDlgKeywordSearch(search, true);
        }

        private void ciKeywords_NewSearch_Click(object sender, EventArgs e)
        {
            string search = "";
            foreach (string str2 in this.listBoxKeywords.SelectedItems)
            {
                search = search + str2 + " ";
            }
            search = search.TrimEnd(" ".ToCharArray());
            base.MainForm.ShowDlgKeywordSearch(search, false);
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
            this.gpgPanelResults = new GPGPanel();
            this.skinButtonClose = new SkinButton();
            this.gpgLabelResults = new GPGLabel();
            this.listBoxKeywords = new ListBox();
            this.gpgGroupBox1 = new GPGGroupBox();
            this.gpgGroupBox2 = new GPGGroupBox();
            this.skinButtonNext = new SkinButton();
            this.skinButtonLast = new SkinButton();
            this.gpgContextMenuKeywords = new GPGContextMenu();
            this.ciKeywords_NewSearch = new MenuItem();
            this.ciKeywords_AppendSearch = new MenuItem();
            this.gpgGroupBox1.SuspendLayout();
            this.gpgGroupBox2.SuspendLayout();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgPanelResults.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelResults.AutoScroll = true;
            this.gpgPanelResults.Location = new Point(6, 0x13);
            this.gpgPanelResults.Name = "gpgPanelResults";
            this.gpgPanelResults.Size = new Size(0x173, 0x99);
            this.gpgPanelResults.TabIndex = 7;
            this.skinButtonClose.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonClose.AutoStyle = true;
            this.skinButtonClose.BackColor = Color.Black;
            this.skinButtonClose.DialogResult = DialogResult.OK;
            this.skinButtonClose.DisabledForecolor = Color.Gray;
            this.skinButtonClose.DrawEdges = true;
            this.skinButtonClose.FocusColor = Color.Yellow;
            this.skinButtonClose.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClose.ForeColor = Color.White;
            this.skinButtonClose.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClose.IsStyled = true;
            this.skinButtonClose.Location = new Point(0x12e, 0x181);
            this.skinButtonClose.Name = "skinButtonClose";
            this.skinButtonClose.Size = new Size(0x60, 0x1a);
            this.skinButtonClose.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonClose.TabIndex = 8;
            this.skinButtonClose.Text = "<LOC>Close";
            this.skinButtonClose.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClose.TextPadding = new Padding(0);
            this.skinButtonClose.Click += new EventHandler(this.skinButtonClose_Click);
            this.gpgLabelResults.AutoSize = true;
            this.gpgLabelResults.AutoStyle = true;
            this.gpgLabelResults.Font = new Font("Arial", 9.75f);
            this.gpgLabelResults.ForeColor = Color.White;
            this.gpgLabelResults.IgnoreMouseWheel = false;
            this.gpgLabelResults.IsStyled = false;
            this.gpgLabelResults.Location = new Point(12, 80);
            this.gpgLabelResults.Name = "gpgLabelResults";
            this.gpgLabelResults.Size = new Size(0x3f, 0x10);
            this.gpgLabelResults.TabIndex = 10;
            this.gpgLabelResults.Text = "0 Results";
            this.gpgLabelResults.TextStyle = TextStyles.Header1;
            this.listBoxKeywords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxKeywords.BackColor = Color.Black;
            this.listBoxKeywords.BorderStyle = BorderStyle.None;
            this.listBoxKeywords.ForeColor = Color.White;
            this.listBoxKeywords.FormattingEnabled = true;
            this.listBoxKeywords.Location = new Point(6, 0x13);
            this.listBoxKeywords.MultiColumn = true;
            this.listBoxKeywords.Name = "listBoxKeywords";
            this.listBoxKeywords.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxKeywords.Size = new Size(0x173, 0x34);
            this.listBoxKeywords.TabIndex = 11;
            this.listBoxKeywords.DoubleClick += new EventHandler(this.listBoxKeywords_DoubleClick);
            this.listBoxKeywords.MouseDown += new MouseEventHandler(this.listBoxKeywords_MouseDown);
            this.listBoxKeywords.KeyDown += new KeyEventHandler(this.listBoxKeywords_KeyDown);
            this.gpgGroupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGroupBox1.Controls.Add(this.gpgPanelResults);
            this.gpgGroupBox1.Location = new Point(15, 0xbf);
            this.gpgGroupBox1.Name = "gpgGroupBox1";
            this.gpgGroupBox1.Size = new Size(0x17f, 0xb2);
            this.gpgGroupBox1.TabIndex = 12;
            this.gpgGroupBox1.TabStop = false;
            this.gpgGroupBox1.Text = "<LOC>Results";
            this.gpgGroupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgGroupBox2.Controls.Add(this.listBoxKeywords);
            this.gpgGroupBox2.Location = new Point(15, 0x6c);
            this.gpgGroupBox2.Name = "gpgGroupBox2";
            this.gpgGroupBox2.Size = new Size(0x17f, 0x4d);
            this.gpgGroupBox2.TabIndex = 13;
            this.gpgGroupBox2.TabStop = false;
            this.gpgGroupBox2.Text = "<LOC>Keywords";
            this.skinButtonNext.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonNext.AutoStyle = true;
            this.skinButtonNext.BackColor = Color.Black;
            this.skinButtonNext.DialogResult = DialogResult.OK;
            this.skinButtonNext.DisabledForecolor = Color.Gray;
            this.skinButtonNext.DrawEdges = true;
            this.skinButtonNext.Enabled = false;
            this.skinButtonNext.FocusColor = Color.Yellow;
            this.skinButtonNext.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNext.ForeColor = Color.White;
            this.skinButtonNext.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNext.IsStyled = true;
            this.skinButtonNext.Location = new Point(200, 0x181);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(0x60, 0x1a);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonNext.TabIndex = 9;
            this.skinButtonNext.Text = "<LOC>Next";
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.skinButtonLast.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonLast.AutoStyle = true;
            this.skinButtonLast.BackColor = Color.Black;
            this.skinButtonLast.DialogResult = DialogResult.OK;
            this.skinButtonLast.DisabledForecolor = Color.Gray;
            this.skinButtonLast.DrawEdges = true;
            this.skinButtonLast.Enabled = false;
            this.skinButtonLast.FocusColor = Color.Yellow;
            this.skinButtonLast.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLast.ForeColor = Color.White;
            this.skinButtonLast.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLast.IsStyled = true;
            this.skinButtonLast.Location = new Point(0x62, 0x181);
            this.skinButtonLast.Name = "skinButtonLast";
            this.skinButtonLast.Size = new Size(0x60, 0x1a);
            this.skinButtonLast.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonLast.TabIndex = 9;
            this.skinButtonLast.Text = "<LOC>Last";
            this.skinButtonLast.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLast.TextPadding = new Padding(0);
            this.skinButtonLast.Click += new EventHandler(this.skinButtonLast_Click);
            this.gpgContextMenuKeywords.MenuItems.AddRange(new MenuItem[] { this.ciKeywords_NewSearch, this.ciKeywords_AppendSearch });
            this.ciKeywords_NewSearch.Index = 0;
            this.ciKeywords_NewSearch.Text = "New search on this keyword";
            this.ciKeywords_NewSearch.Click += new EventHandler(this.ciKeywords_NewSearch_Click);
            this.ciKeywords_AppendSearch.Index = 1;
            this.ciKeywords_AppendSearch.Text = "Append this keyword to current search";
            this.ciKeywords_AppendSearch.Click += new EventHandler(this.ciKeywords_AppendSearch_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonClose;
            base.ClientSize = new Size(410, 0x1da);
            base.Controls.Add(this.skinButtonLast);
            base.Controls.Add(this.skinButtonNext);
            base.Controls.Add(this.gpgGroupBox2);
            base.Controls.Add(this.gpgGroupBox1);
            base.Controls.Add(this.gpgLabelResults);
            base.Controls.Add(this.skinButtonClose);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x157, 0x16a);
            base.Name = "DlgSearchResults";
            this.Text = "<LOC>Search Results";
            base.Controls.SetChildIndex(this.skinButtonClose, 0);
            base.Controls.SetChildIndex(this.gpgLabelResults, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox1, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox2, 0);
            base.Controls.SetChildIndex(this.skinButtonNext, 0);
            base.Controls.SetChildIndex(this.skinButtonLast, 0);
            this.gpgGroupBox1.ResumeLayout(false);
            this.gpgGroupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listBoxKeywords_DoubleClick(object sender, EventArgs e)
        {
            string search = "";
            foreach (string str2 in this.listBoxKeywords.SelectedItems)
            {
                search = search + str2 + " ";
            }
            search = search.TrimEnd(" ".ToCharArray());
            base.MainForm.ShowDlgKeywordSearch(search, false);
        }

        private void listBoxKeywords_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) && (this.listBoxKeywords.SelectedItems.Count > 0))
            {
                string search = "";
                foreach (string str2 in this.listBoxKeywords.SelectedItems)
                {
                    search = search + str2 + " ";
                }
                search = search.TrimEnd(" ".ToCharArray());
                base.MainForm.ShowDlgKeywordSearch(search, false);
            }
        }

        private void listBoxKeywords_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Control.ModifierKeys != Keys.Control)
                {
                    this.listBoxKeywords.SelectedItems.Clear();
                }
                for (int i = 0; i < this.listBoxKeywords.Items.Count; i++)
                {
                    if (this.listBoxKeywords.GetItemRectangle(i).Contains(e.Location))
                    {
                        this.listBoxKeywords.SelectedIndex = i;
                        break;
                    }
                }
                this.gpgContextMenuKeywords.Show(this.listBoxKeywords, e.Location);
            }
        }

        protected override void LoadLayoutData()
        {
            if ((base.LayoutData != null) && (base.LayoutData is Solution[]))
            {
                this.mResultSet = base.LayoutData as Solution[];
            }
            base.LoadLayoutData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RefreshResults();
        }

        public void RefreshResults()
        {
            EventHandler handler = null;
            if (this.ResultSet != null)
            {
                int num;
                this.gpgLabelResults.Text = string.Format(Loc.Get("<LOC>{0} results found"), this.ResultSet.Length);
                this.EntryList.Clear();
                this.listBoxKeywords.DataSource = null;
                this.listBoxKeywords.DataSource = this.ResultSet[0].SearchWords;
                this.listBoxKeywords.SelectedIndices.Clear();
                for (num = 0; num < this.listBoxKeywords.Items.Count; num++)
                {
                    this.listBoxKeywords.SelectedIndices.Add(num);
                }
                this.gpgPanelResults.Controls.Clear();
                for (num = 0; num < this.ResultSet.Length; num++)
                {
                    SearchResultEntry entry = new SearchResultEntry(this.ResultSet[num]);
                    entry.Width = this.gpgPanelResults.Width;
                    entry.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                    entry.Location = new Point(0, num * entry.Height);
                    this.EntryList.AddLast(entry);
                    if (handler == null)
                    {
                        handler = delegate (object s, EventArgs me) {
                            this.SelectEntry(s as SearchResultEntry, true);
                        };
                    }
                    entry.SolutionSelected += handler;
                    this.gpgPanelResults.Controls.Add(entry);
                }
            }
        }

        protected override void SaveLayoutData()
        {
            base.LayoutData = this.ResultSet;
        }

        private void SelectEntry(SearchResultEntry entry, bool showSolution)
        {
            foreach (SearchResultEntry entry2 in this.gpgPanelResults.Controls)
            {
                entry2.Unselect();
            }
            entry.Select();
            this.mSelectedEntry = entry;
            LinkedListNode<SearchResultEntry> node = this.EntryList.Find(entry);
            this.skinButtonLast.Enabled = node.Previous != null;
            this.skinButtonNext.Enabled = node.Next != null;
            this.gpgPanelResults.ScrollControlIntoView(this.SelectedEntry);
            if (showSolution)
            {
                base.MainForm.ShowDlgSolution(this.SelectedEntry.Solution);
            }
        }

        private void skinButtonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void skinButtonLast_Click(object sender, EventArgs e)
        {
            LinkedListNode<SearchResultEntry> node = this.EntryList.Find(this.SelectedEntry);
            if ((node != null) && (node.Previous != null))
            {
                this.SelectEntry(node.Previous.Value, true);
            }
            else
            {
                this.skinButtonLast.Enabled = false;
            }
        }

        private void skinButtonNext_Click(object sender, EventArgs e)
        {
            LinkedListNode<SearchResultEntry> node = this.EntryList.Find(this.SelectedEntry);
            if ((node != null) && (node.Next != null))
            {
                this.SelectEntry(node.Next.Value, true);
            }
            else
            {
                this.skinButtonNext.Enabled = false;
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return false;
            }
        }

        public Solution[] ResultSet
        {
            get
            {
                return this.mResultSet;
            }
            set
            {
                this.mResultSet = value;
            }
        }

        public SearchResultEntry SelectedEntry
        {
            get
            {
                return this.mSelectedEntry;
            }
        }

        public GPG.Multiplayer.Quazal.SolutionsLib.Service Service
        {
            get
            {
                return this.mService;
            }
        }
    }
}

