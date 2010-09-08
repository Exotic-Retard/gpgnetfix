namespace GPG.Multiplayer.Client.SolutionsLib
{
    using GPG;
    using GPG.Logging;
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
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgSolution : DlgBase
    {
        private bool AsyncInProgress;
        private MenuItem ciKeywords_AppendSearch;
        private MenuItem ciKeywords_NewSearch;
        private IContainer components;
        private GPGContextMenu gpgContextMenuKeywords;
        private GPGGroupBox gpgGroupBox1;
        private GPGGroupBox gpgGroupBox2;
        private GPGGroupBox gpgGroupBoxDescription;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabelAuthor;
        private GPGLabel gpgLabelTitle;
        private ListBox listBoxKeywords;
        private List<int> LookupsInProgress;
        private Solution mCurrentSolution;
        private GPG.Multiplayer.Quazal.SolutionsLib.Service mService;
        private LinkedList<Solution> mSolutionHistory;
        private RichTextBox richTextBoxContent;
        private SkinButton skinButtonClose;
        private SkinButton skinButtonLast;
        private SkinButton skinButtonNext;
        private SkinButton skinButtonOpen;

        public DlgSolution()
        {
            this.mSolutionHistory = new LinkedList<Solution>();
            this.AsyncInProgress = false;
            this.LookupsInProgress = new List<int>();
            this.components = null;
            this.InitializeComponent();
            this.mService = new GPG.Multiplayer.Quazal.SolutionsLib.Service();
            this.mService.Url = ConfigSettings.GetString("SolutionsLibService", "http://gpgnet.gaspowered.com/quazal/Service.asmx?WSDL");
        }

        public DlgSolution(FrmMain mainForm, Solution solution) : base(mainForm)
        {
            this.mSolutionHistory = new LinkedList<Solution>();
            this.AsyncInProgress = false;
            this.LookupsInProgress = new List<int>();
            this.components = null;
            this.InitializeComponent();
            this.mService = new GPG.Multiplayer.Quazal.SolutionsLib.Service();
            this.mService.Url = ConfigSettings.GetString("SolutionsLibService", "http://gpgnet.gaspowered.com/quazal/Service.asmx?WSDL");
            this.mCurrentSolution = solution;
            this.SolutionHistory.AddFirst(solution);
            this.LookupSolution();
        }

        public DlgSolution(FrmMain mainForm, int id) : base(mainForm)
        {
            this.mSolutionHistory = new LinkedList<Solution>();
            this.AsyncInProgress = false;
            this.LookupsInProgress = new List<int>();
            this.components = null;
            this.InitializeComponent();
            this.mService = new GPG.Multiplayer.Quazal.SolutionsLib.Service();
            this.mService.Url = ConfigSettings.GetString("SolutionsLibService", "http://gpgnet.gaspowered.com/quazal/Service.asmx?WSDL");
            this.LookupSolution(id);
        }

        public void AddSolution(Solution solution)
        {
            for (LinkedListNode<Solution> node = this.SolutionHistory.First; node != null; node = node.Next)
            {
                if (node.Value.ID == solution.ID)
                {
                    this.mCurrentSolution = node.Value;
                    this.LookupSolution();
                    return;
                }
            }
            this.mCurrentSolution = solution;
            this.SolutionHistory.AddLast(solution);
            this.LookupSolution();
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

        private void DoStraightLookup(object sender, GetSolutionDetailsCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ErrorLog.WriteLine(e.Error);
            }
            else if (!e.Cancelled)
            {
                int userState = (int) e.UserState;
                this.LookupsInProgress.Remove(userState);
                this.Service.GetSolutionDetailsCompleted -= new GetSolutionDetailsCompletedEventHandler(this.DoStraightLookup);
                Solution solution = new Solution(userState, e.title);
                solution.Author = e.author;
                solution.Description = e.Result;
                solution.Keywords = e.keywords;
                this.SolutionHistory.AddLast(solution);
                this.mCurrentSolution = solution;
                this.LookupSolution();
            }
        }

        private void gpgGroupBoxDescription_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(this.richTextBoxContent.Location, this.richTextBoxContent.Size);
            rect.Inflate(2, 2);
            using (Brush brush = new SolidBrush(Program.Settings.StylePreferences.HighlightColor3))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }

        private void InitializeComponent()
        {
            this.listBoxKeywords = new ListBox();
            this.gpgLabelTitle = new GPGLabel();
            this.richTextBoxContent = new RichTextBox();
            this.skinButtonClose = new SkinButton();
            this.skinButtonOpen = new SkinButton();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabelAuthor = new GPGLabel();
            this.skinButtonLast = new SkinButton();
            this.skinButtonNext = new SkinButton();
            this.gpgGroupBox1 = new GPGGroupBox();
            this.gpgGroupBoxDescription = new GPGGroupBox();
            this.gpgGroupBox2 = new GPGGroupBox();
            this.gpgContextMenuKeywords = new GPGContextMenu();
            this.ciKeywords_NewSearch = new MenuItem();
            this.ciKeywords_AppendSearch = new MenuItem();
            this.gpgGroupBox1.SuspendLayout();
            this.gpgGroupBoxDescription.SuspendLayout();
            this.gpgGroupBox2.SuspendLayout();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.listBoxKeywords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxKeywords.BackColor = Color.Black;
            this.listBoxKeywords.BorderStyle = BorderStyle.None;
            this.listBoxKeywords.ForeColor = Color.White;
            this.listBoxKeywords.FormattingEnabled = true;
            this.listBoxKeywords.Location = new Point(6, 0x13);
            this.listBoxKeywords.MultiColumn = true;
            this.listBoxKeywords.Name = "listBoxKeywords";
            this.listBoxKeywords.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxKeywords.Size = new Size(0x13f, 0x34);
            this.listBoxKeywords.TabIndex = 13;
            this.listBoxKeywords.DoubleClick += new EventHandler(this.listBoxKeywords_DoubleClick);
            this.listBoxKeywords.MouseDown += new MouseEventHandler(this.listBoxKeywords_MouseDown);
            this.listBoxKeywords.KeyDown += new KeyEventHandler(this.listBoxKeywords_KeyDown);
            this.gpgLabelTitle.AutoSize = true;
            this.gpgLabelTitle.AutoStyle = true;
            this.gpgLabelTitle.Font = new Font("Arial", 9.75f);
            this.gpgLabelTitle.ForeColor = Color.White;
            this.gpgLabelTitle.IgnoreMouseWheel = false;
            this.gpgLabelTitle.IsStyled = false;
            this.gpgLabelTitle.Location = new Point(9, 80);
            this.gpgLabelTitle.Name = "gpgLabelTitle";
            this.gpgLabelTitle.Size = new Size(0x20, 0x10);
            this.gpgLabelTitle.TabIndex = 12;
            this.gpgLabelTitle.Text = "Title";
            this.gpgLabelTitle.TextStyle = TextStyles.Header1;
            this.richTextBoxContent.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.richTextBoxContent.BackColor = Color.White;
            this.richTextBoxContent.BorderStyle = BorderStyle.None;
            this.richTextBoxContent.Location = new Point(8, 0x15);
            this.richTextBoxContent.Name = "richTextBoxContent";
            this.richTextBoxContent.ReadOnly = true;
            this.richTextBoxContent.Size = new Size(0x1ef, 0xc4);
            this.richTextBoxContent.TabIndex = 15;
            this.richTextBoxContent.Text = "";
            this.richTextBoxContent.LinkClicked += new LinkClickedEventHandler(this.richTextBoxContent_LinkClicked);
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
            this.skinButtonClose.Location = new Point(0x191, 0x1bc);
            this.skinButtonClose.Name = "skinButtonClose";
            this.skinButtonClose.Size = new Size(0x7c, 0x1a);
            this.skinButtonClose.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonClose.TabIndex = 0x10;
            this.skinButtonClose.Text = "<LOC>Close";
            this.skinButtonClose.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClose.TextPadding = new Padding(0);
            this.skinButtonClose.Click += new EventHandler(this.skinButtonClose_Click);
            this.skinButtonOpen.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOpen.AutoStyle = true;
            this.skinButtonOpen.BackColor = Color.Black;
            this.skinButtonOpen.DialogResult = DialogResult.OK;
            this.skinButtonOpen.DisabledForecolor = Color.Gray;
            this.skinButtonOpen.DrawEdges = true;
            this.skinButtonOpen.FocusColor = Color.Yellow;
            this.skinButtonOpen.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOpen.ForeColor = Color.White;
            this.skinButtonOpen.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOpen.IsStyled = true;
            this.skinButtonOpen.Location = new Point(0x107, 0x1bc);
            this.skinButtonOpen.Name = "skinButtonOpen";
            this.skinButtonOpen.Size = new Size(0x84, 0x1a);
            this.skinButtonOpen.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOpen.TabIndex = 0x11;
            this.skinButtonOpen.Text = "<LOC>Open as text file";
            this.skinButtonOpen.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOpen.TextPadding = new Padding(4, 0, 0, 0);
            this.skinButtonOpen.Click += new EventHandler(this.skinButtonOpen_Click);
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 8f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x16c, 110);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x49, 14);
            this.gpgLabel2.TabIndex = 0x12;
            this.gpgLabel2.Text = "<LOC>Author";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleRight;
            this.gpgLabel2.TextStyle = TextStyles.Bold;
            this.gpgLabelAuthor.AutoSize = true;
            this.gpgLabelAuthor.AutoStyle = true;
            this.gpgLabelAuthor.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabelAuthor.ForeColor = Color.White;
            this.gpgLabelAuthor.IgnoreMouseWheel = false;
            this.gpgLabelAuthor.IsStyled = false;
            this.gpgLabelAuthor.Location = new Point(0x16c, 0x7c);
            this.gpgLabelAuthor.Name = "gpgLabelAuthor";
            this.gpgLabelAuthor.Size = new Size(0x26, 14);
            this.gpgLabelAuthor.TabIndex = 0x13;
            this.gpgLabelAuthor.Text = "author";
            this.gpgLabelAuthor.TextStyle = TextStyles.Default;
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
            this.skinButtonLast.Location = new Point(6, 0x13);
            this.skinButtonLast.Name = "skinButtonLast";
            this.skinButtonLast.Size = new Size(0x45, 0x16);
            this.skinButtonLast.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonLast.TabIndex = 20;
            this.skinButtonLast.Text = "<LOC>Last";
            this.skinButtonLast.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLast.TextPadding = new Padding(0);
            this.skinButtonLast.Click += new EventHandler(this.skinButtonLast_Click);
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
            this.skinButtonNext.Location = new Point(0x51, 0x13);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(0x45, 0x16);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonNext.TabIndex = 0x15;
            this.skinButtonNext.Text = "<LOC>Next";
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.gpgGroupBox1.Controls.Add(this.listBoxKeywords);
            this.gpgGroupBox1.Location = new Point(12, 110);
            this.gpgGroupBox1.Name = "gpgGroupBox1";
            this.gpgGroupBox1.Size = new Size(0x14b, 0x4e);
            this.gpgGroupBox1.TabIndex = 0x16;
            this.gpgGroupBox1.TabStop = false;
            this.gpgGroupBox1.Text = "<LOC>Keywords in this solution";
            this.gpgGroupBoxDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGroupBoxDescription.Controls.Add(this.richTextBoxContent);
            this.gpgGroupBoxDescription.Location = new Point(12, 0xc4);
            this.gpgGroupBoxDescription.Name = "gpgGroupBoxDescription";
            this.gpgGroupBoxDescription.Size = new Size(0x201, 0xe3);
            this.gpgGroupBoxDescription.TabIndex = 0x17;
            this.gpgGroupBoxDescription.TabStop = false;
            this.gpgGroupBoxDescription.Text = "<LOC>Description";
            this.gpgGroupBox2.Controls.Add(this.skinButtonLast);
            this.gpgGroupBox2.Controls.Add(this.skinButtonNext);
            this.gpgGroupBox2.Location = new Point(0x16f, 0x8d);
            this.gpgGroupBox2.Name = "gpgGroupBox2";
            this.gpgGroupBox2.Size = new Size(0x9e, 0x2f);
            this.gpgGroupBox2.TabIndex = 0x18;
            this.gpgGroupBox2.TabStop = false;
            this.gpgGroupBox2.Text = "<LOC>View History";
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
            base.ClientSize = new Size(0x218, 0x215);
            base.Controls.Add(this.gpgGroupBox2);
            base.Controls.Add(this.skinButtonClose);
            base.Controls.Add(this.gpgGroupBoxDescription);
            base.Controls.Add(this.gpgGroupBox1);
            base.Controls.Add(this.gpgLabelAuthor);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.skinButtonOpen);
            base.Controls.Add(this.gpgLabelTitle);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x218, 0x215);
            base.Name = "DlgSolution";
            this.Text = "<LOC>Solution";
            base.Controls.SetChildIndex(this.gpgLabelTitle, 0);
            base.Controls.SetChildIndex(this.skinButtonOpen, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabelAuthor, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox1, 0);
            base.Controls.SetChildIndex(this.gpgGroupBoxDescription, 0);
            base.Controls.SetChildIndex(this.skinButtonClose, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox2, 0);
            this.gpgGroupBox1.ResumeLayout(false);
            this.gpgGroupBoxDescription.ResumeLayout(false);
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
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                if ((base.LayoutData != null) && (base.LayoutData is int[]))
                {
                    int[] layoutData = base.LayoutData as int[];
                    for (int j = 0; j < layoutData.Length; j++)
                    {
                        this.LookupSolution(layoutData[j]);
                        while (this.AsyncInProgress)
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
            });
            base.LoadLayoutData();
        }

        private void LookupSolution()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.LookupSolution();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                LinkedListNode<Solution> node = this.SolutionHistory.Find(this.CurrentSolution);
                this.skinButtonLast.Enabled = node.Previous != null;
                this.skinButtonNext.Enabled = node.Next != null;
                this.gpgLabelTitle.Text = string.Format("{0} - {1}", this.CurrentSolution.ID, this.CurrentSolution.Title);
                this.Text = string.Format(Loc.Get("<LOC>Solution {0} - {1}"), this.CurrentSolution.ID, this.CurrentSolution.Title);
                this.Refresh();
                if ((this.CurrentSolution.Description == null) || (this.CurrentSolution.Description.Length < 1))
                {
                    this.Service.GetSolutionDetailsCompleted += new GetSolutionDetailsCompletedEventHandler(this.Service_GetSolutionDetailsCompleted);
                    this.Service.GetSolutionDetailsAsync(this.CurrentSolution.ID);
                }
                else
                {
                    this.gpgLabelAuthor.Text = this.CurrentSolution.Author;
                    this.richTextBoxContent.Rtf = this.CurrentSolution.Description;
                    this.listBoxKeywords.DataSource = null;
                    this.listBoxKeywords.DataSource = this.CurrentSolution.Keywords;
                }
                this.AsyncInProgress = false;
            }
        }

        public void LookupSolution(int id)
        {
            if (!this.LookupsInProgress.Contains(id))
            {
                this.LookupsInProgress.Add(id);
                for (LinkedListNode<Solution> node = this.SolutionHistory.First; node != null; node = node.Next)
                {
                    if (node.Value.ID == id)
                    {
                        this.mCurrentSolution = node.Value;
                        this.LookupSolution();
                        return;
                    }
                }
                this.AsyncInProgress = true;
                this.Service.GetSolutionDetailsCompleted += new GetSolutionDetailsCompletedEventHandler(this.DoStraightLookup);
                this.Service.GetSolutionDetailsAsync(id, id);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgGroupBoxDescription.Paint += new PaintEventHandler(this.gpgGroupBoxDescription_Paint);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Refresh();
        }

        private void richTextBoxContent_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            base.MainForm.ShowWebPage(e.LinkText);
        }

        protected override void SaveLayoutData()
        {
            int[] numArray = new int[this.SolutionHistory.Count];
            LinkedListNode<Solution> first = this.SolutionHistory.First;
            for (int i = 0; first != null; i++)
            {
                numArray[i] = first.Value.ID;
                first = first.Next;
            }
            base.LayoutData = numArray;
        }

        private void Service_GetSolutionDetailsCompleted(object sender, GetSolutionDetailsCompletedEventArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.Service_GetSolutionDetailsCompleted(sender, e);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                if (e.Error != null)
                {
                    ErrorLog.WriteLine(e.Error);
                }
                else if (!e.Cancelled)
                {
                    this.Service.GetSolutionDetailsCompleted -= new GetSolutionDetailsCompletedEventHandler(this.Service_GetSolutionDetailsCompleted);
                    this.CurrentSolution.Author = e.author;
                    this.CurrentSolution.Description = e.Result;
                    this.CurrentSolution.Keywords = e.keywords;
                    this.gpgLabelAuthor.Text = this.CurrentSolution.Author;
                    if ((this.CurrentSolution.Description != null) && (this.CurrentSolution.Description.Length > 0))
                    {
                        this.richTextBoxContent.Rtf = this.CurrentSolution.Description;
                    }
                    this.listBoxKeywords.DataSource = null;
                    this.listBoxKeywords.DataSource = this.CurrentSolution.Keywords;
                }
            }
        }

        private void skinButtonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void skinButtonLast_Click(object sender, EventArgs e)
        {
            LinkedListNode<Solution> node = this.SolutionHistory.Find(this.CurrentSolution);
            if (node.Previous != null)
            {
                this.mCurrentSolution = node.Previous.Value;
                this.LookupSolution();
            }
        }

        private void skinButtonNext_Click(object sender, EventArgs e)
        {
            LinkedListNode<Solution> node = this.SolutionHistory.Find(this.CurrentSolution);
            if (node.Next != null)
            {
                this.mCurrentSolution = node.Next.Value;
                this.LookupSolution();
            }
        }

        private void skinButtonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.GetTempPath() + string.Format("Solution {0}.rtf", this.CurrentSolution.ID);
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                        this.richTextBoxContent.SaveFile(path, RichTextBoxStreamType.RichText);
                        Process.Start(path);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    this.richTextBoxContent.SaveFile(path, RichTextBoxStreamType.RichText);
                    Process.Start(path);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return false;
            }
        }

        public override bool AllowRestoreWindow
        {
            get
            {
                return false;
            }
        }

        public Solution CurrentSolution
        {
            get
            {
                return this.mCurrentSolution;
            }
            set
            {
                this.mCurrentSolution = value;
            }
        }

        public GPG.Multiplayer.Quazal.SolutionsLib.Service Service
        {
            get
            {
                return this.mService;
            }
        }

        public LinkedList<Solution> SolutionHistory
        {
            get
            {
                return this.mSolutionHistory;
            }
        }
    }
}

