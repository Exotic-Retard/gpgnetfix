namespace GPG.Multiplayer.Client.SolutionsLib
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.SolutionsLib;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgKeywordSearch : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGLabel gpgLabelToolHelp1;
        private GPGLabel gpgLabelToolHelp2;
        private GPGTextBox gpgTextBoxKeyword;
        private GPG.Multiplayer.Quazal.SolutionsLib.Service mService;
        private bool Searching;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonSearch;

        public DlgKeywordSearch()
        {
            this.components = null;
            this.Searching = false;
            this.InitializeComponent();
            this.mService = new GPG.Multiplayer.Quazal.SolutionsLib.Service();
            this.mService.Url = ConfigSettings.GetString("SolutionsLibService", "http://gpgnet.gaspowered.com/quazal/Service.asmx?WSDL");
        }

        public DlgKeywordSearch(FrmMain mainForm, string search) : base(mainForm)
        {
            this.components = null;
            this.Searching = false;
            this.InitializeComponent();
            this.mService = new GPG.Multiplayer.Quazal.SolutionsLib.Service();
            this.mService.Url = ConfigSettings.GetString("SolutionsLibService", "http://gpgnet.gaspowered.com/quazal/Service.asmx?WSDL");
            this.DoSearch(search, false);
        }

        private void CommonSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch((sender as Control).Text, false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoSearch()
        {
            WaitCallback callBack = null;
            bool flag = false;
            if ((this.gpgTextBoxKeyword.Text == null) || (this.gpgTextBoxKeyword.Text.Length < 1))
            {
                base.Error(this.gpgTextBoxKeyword, "<LOC>Keyword cannot be blank", new object[0]);
                flag = true;
            }
            if (this.gpgTextBoxKeyword.Text.Length < 2)
            {
                base.Error(this.gpgTextBoxKeyword, "<LOC>Keyword cannot be less than 2 characters", new object[0]);
                flag = true;
            }
            if (!flag)
            {
                base.ClearErrors();
                base.SetStatus("<LOC>Searching...", new object[0]);
                this.Searching = true;
                this.skinButtonSearch.Enabled = false;
                this.gpgTextBoxKeyword.Enabled = false;
                this.Service.SLKeywordSearchCompleted += new SLKeywordSearchCompletedEventHandler(this.Service_SLKeywordSearchCompleted);
                if (callBack == null)
                {
                    callBack = delegate (object p) {
                        try
                        {
                            List<string> list = new List<string>();
                            list.AddRange(this.gpgTextBoxKeyword.Text.Replace("'", "").Split(" ".ToCharArray()));
                            if (GameInformation.SelectedGame.IsSpaceSiege)
                            {
                                list.Add("SPACESIEGE");
                            }
                            else
                            {
                                list.Add("SUPCOM");
                            }
                            this.Service.SLKeywordSearchAsync(list.ToArray(), Guid.NewGuid());
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                            base.Error(this.gpgTextBoxKeyword, "<LOC>Unable to connect to the knowledge base.", new object[0]);
                        }
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
        }

        public void DoSearch(string search, bool append)
        {
            if (append)
            {
                this.gpgTextBoxKeyword.Text = this.gpgTextBoxKeyword.Text + " " + search;
            }
            else
            {
                this.gpgTextBoxKeyword.Text = search;
            }
            this.DoSearch();
        }

        private void gpgLabelToolHelp_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowDlgSolution(ConfigSettings.GetInt("GPGNetSolution", 0x3e2));
        }

        private void InitializeComponent()
        {
            this.gpgTextBoxKeyword = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonSearch = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.gpgLabelToolHelp1 = new GPGLabel();
            this.gpgLabelToolHelp2 = new GPGLabel();
            this.gpgTextBoxKeyword.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgTextBoxKeyword.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxKeyword.Location = new Point(12, 0x8d);
            this.gpgTextBoxKeyword.Name = "gpgTextBoxKeyword";
            this.gpgTextBoxKeyword.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxKeyword.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxKeyword.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxKeyword.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxKeyword.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxKeyword.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxKeyword.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxKeyword.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxKeyword.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxKeyword.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxKeyword.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxKeyword.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxKeyword.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxKeyword.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxKeyword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxKeyword.Size = new Size(0x16c, 20);
            this.gpgTextBoxKeyword.TabIndex = 0;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x69);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x127, 0x21);
            this.gpgLabel1.TabIndex = 1;
            this.gpgLabel1.Text = "<LOC>Enter keyword search (e.g. video card). English only, please.";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.skinButtonSearch.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonSearch.AutoStyle = true;
            this.skinButtonSearch.BackColor = Color.Black;
            this.skinButtonSearch.DialogResult = DialogResult.OK;
            this.skinButtonSearch.DisabledForecolor = Color.Gray;
            this.skinButtonSearch.DrawEdges = true;
            this.skinButtonSearch.FocusColor = Color.Yellow;
            this.skinButtonSearch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSearch.ForeColor = Color.White;
            this.skinButtonSearch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSearch.IsStyled = true;
            this.skinButtonSearch.Location = new Point(0xba, 0x126);
            this.skinButtonSearch.Name = "skinButtonSearch";
            this.skinButtonSearch.Size = new Size(0x5c, 0x1a);
            this.skinButtonSearch.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonSearch.TabIndex = 2;
            this.skinButtonSearch.Text = "<LOC>Search";
            this.skinButtonSearch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearch.TextPadding = new Padding(0);
            this.skinButtonSearch.Click += new EventHandler(this.skinButtonSearch_Click);
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x11c, 0x126);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5c, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 3;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0x4f);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(210, 0x10);
            this.gpgLabel2.TabIndex = 7;
            this.gpgLabel2.Text = "<LOC>Search the knowledge base";
            this.gpgLabel2.TextStyle = TextStyles.Header1;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(12, 0xc5);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0xcc, 0x10);
            this.gpgLabel3.TabIndex = 8;
            this.gpgLabel3.Text = "<LOC>Common search keywords";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x16, 0x115);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x33, 0x10);
            this.gpgLabel5.TabIndex = 10;
            this.gpgLabel5.Text = "Friends";
            this.gpgLabel5.TextStyle = TextStyles.Link;
            this.gpgLabel5.Click += new EventHandler(this.CommonSearch_Click);
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x16, 0x129);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x22, 0x10);
            this.gpgLabel6.TabIndex = 11;
            this.gpgLabel6.Text = "Clan";
            this.gpgLabel6.TextStyle = TextStyles.Link;
            this.gpgLabel6.Click += new EventHandler(this.CommonSearch_Click);
            this.gpgLabel7.AutoSize = true;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x16, 0x101);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x37, 0x10);
            this.gpgLabel7.TabIndex = 12;
            this.gpgLabel7.Text = "Network";
            this.gpgLabel7.TextStyle = TextStyles.Link;
            this.gpgLabel7.Click += new EventHandler(this.CommonSearch_Click);
            this.gpgLabel8.AutoSize = true;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.Font = new Font("Arial", 9.75f);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x16, 0xd9);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x29, 0x10);
            this.gpgLabel8.TabIndex = 13;
            this.gpgLabel8.Text = "Video";
            this.gpgLabel8.TextStyle = TextStyles.Link;
            this.gpgLabel8.Click += new EventHandler(this.CommonSearch_Click);
            this.gpgLabel9.AutoSize = true;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.Font = new Font("Arial", 9.75f);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0x16, 0xed);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x29, 0x10);
            this.gpgLabel9.TabIndex = 14;
            this.gpgLabel9.Text = "Audio";
            this.gpgLabel9.TextStyle = TextStyles.Link;
            this.gpgLabel9.Click += new EventHandler(this.CommonSearch_Click);
            this.gpgLabelToolHelp1.AutoSize = true;
            this.gpgLabelToolHelp1.AutoStyle = true;
            this.gpgLabelToolHelp1.Font = new Font("Arial", 9.75f);
            this.gpgLabelToolHelp1.ForeColor = Color.White;
            this.gpgLabelToolHelp1.IgnoreMouseWheel = false;
            this.gpgLabelToolHelp1.IsStyled = false;
            this.gpgLabelToolHelp1.Location = new Point(12, 0xa4);
            this.gpgLabelToolHelp1.Margin = new Padding(3, 0, 0, 0);
            this.gpgLabelToolHelp1.Name = "gpgLabelToolHelp1";
            this.gpgLabelToolHelp1.Size = new Size(190, 0x10);
            this.gpgLabelToolHelp1.TabIndex = 15;
            this.gpgLabelToolHelp1.Text = "<LOC>For help using this tool, ";
            this.gpgLabelToolHelp1.TextStyle = TextStyles.Default;
            this.gpgLabelToolHelp2.AutoSize = true;
            this.gpgLabelToolHelp2.AutoStyle = true;
            this.gpgLabelToolHelp2.Font = new Font("Arial", 9.75f);
            this.gpgLabelToolHelp2.ForeColor = Color.White;
            this.gpgLabelToolHelp2.IgnoreMouseWheel = false;
            this.gpgLabelToolHelp2.IsStyled = false;
            this.gpgLabelToolHelp2.Location = new Point(0xc5, 0xa4);
            this.gpgLabelToolHelp2.Margin = new Padding(0, 0, 3, 0);
            this.gpgLabelToolHelp2.Name = "gpgLabelToolHelp2";
            this.gpgLabelToolHelp2.Size = new Size(110, 0x10);
            this.gpgLabelToolHelp2.TabIndex = 0x10;
            this.gpgLabelToolHelp2.Text = "<LOC>click here.";
            this.gpgLabelToolHelp2.TextStyle = TextStyles.Link;
            this.gpgLabelToolHelp2.Click += new EventHandler(this.gpgLabelToolHelp_Click);
            base.AcceptButton = this.skinButtonSearch;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x191, 0x189);
            base.Controls.Add(this.gpgLabelToolHelp2);
            base.Controls.Add(this.gpgLabelToolHelp1);
            base.Controls.Add(this.gpgLabel9);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextBoxKeyword);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonSearch);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x191, 0x176);
            base.Name = "DlgKeywordSearch";
            this.Text = "<LOC>GPGnet Knowledge Base";
            base.Controls.SetChildIndex(this.skinButtonSearch, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxKeyword, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgLabel7, 0);
            base.Controls.SetChildIndex(this.gpgLabel8, 0);
            base.Controls.SetChildIndex(this.gpgLabel9, 0);
            base.Controls.SetChildIndex(this.gpgLabelToolHelp1, 0);
            base.Controls.SetChildIndex(this.gpgLabelToolHelp2, 0);
            this.gpgTextBoxKeyword.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void LoadLayoutData()
        {
            if (base.LayoutData != null)
            {
                this.gpgTextBoxKeyword.Text = base.LayoutData.ToString();
            }
            base.LoadLayoutData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextBoxKeyword.Select();
            this.gpgLabelToolHelp2.Left = this.gpgLabelToolHelp1.Right - 10;
        }

        protected override void SaveLayoutData()
        {
            base.LayoutData = this.gpgTextBoxKeyword.Text;
        }

        private void Service_SLKeywordSearchCompleted(object sender, SLKeywordSearchCompletedEventArgs e)
        {
            VGen0 method = null;
            if (!e.Cancelled)
            {
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.Service_SLKeywordSearchCompleted(sender, e);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!base.Disposing && !base.IsDisposed)
                {
                    try
                    {
                        if (((e.Error != null) || (e.Result == null)) || (e.Result.Length < 1))
                        {
                            base.SetStatus("<LOC>No results found.", new object[0]);
                            this.Service.SLKeywordSearchCompleted -= new SLKeywordSearchCompletedEventHandler(this.Service_SLKeywordSearchCompleted);
                        }
                        else
                        {
                            Solution[] results = new Solution[e.Result.Length];
                            string[] strArray = this.gpgTextBoxKeyword.Text.Split(" ".ToCharArray());
                            for (int i = 0; i < results.Length; i++)
                            {
                                results[i] = new Solution(e.Result[i].SolutionID, e.Result[i].Title);
                                results[i].SearchWords = strArray;
                            }
                            base.MainForm.ShowDlgSearchResults(results);
                            this.Service.SLKeywordSearchCompleted -= new SLKeywordSearchCompletedEventHandler(this.Service_SLKeywordSearchCompleted);
                            base.ClearStatus();
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                    finally
                    {
                        this.skinButtonSearch.Enabled = true;
                        this.gpgTextBoxKeyword.Enabled = true;
                        this.Searching = false;
                    }
                }
            }
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            if (this.Searching)
            {
                this.Service.SLKeywordSearchCompleted -= new SLKeywordSearchCompletedEventHandler(this.Service_SLKeywordSearchCompleted);
                this.skinButtonSearch.Enabled = true;
                this.gpgTextBoxKeyword.Enabled = true;
                this.Searching = false;
                base.ClearStatus();
            }
            else
            {
                base.Close();
            }
        }

        private void skinButtonSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch();
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return false;
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

