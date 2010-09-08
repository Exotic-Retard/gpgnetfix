namespace GPG.Multiplayer.Client.Vaulting
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
    using GPG.Multiplayer.Client.Vaulting.Applications;
    using GPG.Multiplayer.Client.Vaulting.Maps;
    using GPG.Multiplayer.Client.Volunteering;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgContentManager : DlgBase
    {
        private List<ActivityMonitor> ActiveDownloads = new List<ActivityMonitor>();
        private GPGPanel ActivePanel = null;
        private List<ActivityMonitor> ActiveUploads = new List<ActivityMonitor>();
        public ToolStripMenuItem btnHelp;
        public ToolStripMenuItem btnMapDiagnose;
        public ToolStripMenuItem btnMore;
        public ToolStripMenuItem btnOptions;
        private bool CancelUploadSearch = false;
        private IContainer components = null;
        private PnlContentDetailsView CurrentContentView = null;
        private SearchViews CurrentSearchView = SearchViews.Basic;
        private ContentVaultTabs CurrentTab = ContentVaultTabs.None;
        private Control CurrentUploadOptions = null;
        private GPGDataGrid dataGridSearchResults;
        private GridColumn[] DefaultGridColumns = null;
        private bool DefaultListPicked = false;
        private IAdditionalContent DlMouseDown = null;
        private bool DownloadMouseUpHooked = false;
        private Font DownloadSizeFont = new Font("Verdana", 7f, FontStyle.Bold);
        private bool FirstPaint = true;
        private GridColumn gcDate;
        private GridColumn gcDownload;
        private GridColumn gcDownloads;
        private GridColumn gcName;
        private GridColumn gcOwner;
        private GridColumn gcRating;
        private GridColumn gcVersion;
        private SkinGroupPanel gpgGroupBoxCriteria;
        private SkinGroupPanel gpgGroupBoxResults;
        private SkinGroupPanel gpgGroupBoxUploadGeneral;
        private SkinGroupPanel gpgGroupBoxUploadVersion;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel16;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel21;
        private GPGLabel gpgLabel25;
        private GPGLabel gpgLabel27;
        private GPGLabel gpgLabel29;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGLabel gpgLabelMyContent;
        private GPGLabel gpgLabelNoUploadSelected;
        private GPGLabel gpgLabelSearchType;
        private GPGLabel gpgLabelUploadCurrentVersion;
        private GPGLabel gpgLabelUploadLocation;
        private GPGLabel gpgLabelUploadLocations;
        private GPGLabel gpgLabelUploadVersionNotes;
        private GPGPanel gpgPanel2;
        private GPGPanel gpgPanelActivity;
        private GPGPanel gpgPanelDownActivity;
        private GPGPanel gpgPanelDownload;
        private GPGPanel gpgPanelMyContent;
        private GPGPanel gpgPanelSearchCriteria;
        private GPGPanel gpgPanelUpActivity;
        private GPGPanel gpgPanelUpload;
        private GPGPanel gpgPanelUploadGeneral;
        private GPGPanel gpgPanelUploadVersion;
        private GPGTextArea gpgTextAreaVersionNotes;
        private GPGTextBox gpgTextBoxSearchCreator;
        private GPGTextBox gpgTextBoxSearchKeywords;
        private GPGTextBox gpgTextBoxSearchName;
        private GPGTextBox gpgTextBoxUploadDesc;
        private GPGTextBox gpgTextBoxUploadName;
        private GPGTextBox gpgTextBoxUploadSearchKeywords;
        private GridView gvResults;
        private ImageList imageListContentTypes;
        private GridHitInfo LastDownloadHit = null;
        private Point LastMouseLoc;
        private ListBox listBoxUploadDependencies;
        private List<ToolStripItem> mCustomPaint = new List<ToolStripItem>();
        public string mInvalidUploadDirectories = AdditionalContent.MyContentPath;
        private IAdditionalContent MouseOverDownload = null;
        private int mPageCount;
        private GPGMenuStrip msQuickButtons;
        private List<IAdditionalContent> MyUploads = new List<IAdditionalContent>();
        private TreeNode NodeAvailableUploads = null;
        private TreeNode NodeMyUploads = null;
        private TreeNode NodeUploadSearch = null;
        private PictureBox pictureBoxRefreshUploads;
        private string PreviousUploadStatus = "";
        private RepositoryItemHyperLinkEdit repositoryItemChatLink;
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private RepositoryItemPictureEdit repositoryItemRatingStars;
        private RepositoryItemHyperLinkEdit repositoryItemVersionLink;
        private GridColumn[] ResultColumns = null;
        private bool ScanningForUploads = false;
        private bool Searching = false;
        private int SearchPage = 0;
        private IAdditionalContent[] SearchResults = null;
        private ContentList SelectedContentList = null;
        private IAdditionalContent SelectedDownloadContent = null;
        private Control SelectedDownloadOptions = null;
        private IAdditionalContent SelectedUpload = null;
        private TreeNode SelectedUploadTreeNode = null;
        private SkinButton skinButtonDeleteUploadAll;
        private SkinButton skinButtonDeleteUploadDependency;
        private SkinButton skinButtonDeleteUploadVersion;
        private SkinButton skinButtonRunSearch;
        private SkinButton skinButtonSaveSearch;
        private SkinButton skinButtonSearchNext;
        private SkinButton skinButtonSearchPrevious;
        private SkinButton skinButtonSearchStart;
        private SkinButton skinButtonSearchType;
        private SkinButton skinButtonUpload;
        private SkinButton skinButtonUploadDependency;
        private SkinLabel skinLabel1;
        private SkinLabel skinLabel2;
        private SkinLabel skinLabel3;
        private SkinLabel skinLabel4;
        private SkinLabel skinLabel5;
        private SkinLabel skinLabel6;
        private SkinLabel skinLabelSearchPage;
        private SplitContainer splitContainerActivity;
        private SplitContainer splitContainerDownload;
        private SplitContainer splitContainerMyContent;
        private SplitContainer splitContainerUpload;
        private SkinButton tabActivity;
        private SkinButton tabDownload;
        private SkinButton tabMyContent;
        private SkinButton tabUpload;
        private bool ToolstripSizeChanged = false;
        private Color TreeBackground = Color.FromArgb(0x33, 0x33, 0x33);
        private TreeView treeViewDownloadType;
        private TreeView treeViewMyContent;
        private TreeView treeViewSavedSearches;
        private TreeView treeViewUpload;
        private Dictionary<string, TreeNode> UploadFileNodes = new Dictionary<string, TreeNode>();
        private Dictionary<string, FileSystemWatcher> UploadMonitors = new Dictionary<string, FileSystemWatcher>();
        private bool ViewingVersionHistory = false;
        private Service WebService = new Service();

        public DlgContentManager()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            this.tabMyContent.Tag = ContentVaultTabs.None | ContentVaultTabs.MyContent;
            this.tabMyContent.Click += new EventHandler(this.ChangePanel);
            this.tabDownload.Tag = ContentVaultTabs.Download;
            this.tabDownload.Click += new EventHandler(this.ChangePanel);
            this.tabUpload.Tag = ContentVaultTabs.Upload;
            this.tabUpload.Click += new EventHandler(this.ChangePanel);
            this.tabActivity.Tag = ContentVaultTabs.Activity;
            this.tabActivity.Click += new EventHandler(this.ChangePanel);
            this.InitializeToolstrip();
            SkinButton[] buttonArray = new SkinButton[] { this.tabActivity, this.tabUpload, this.tabMyContent, this.tabDownload };
            int num = 3;
            foreach (SkinButton button in buttonArray)
            {
                button.MakeEdgesTransparent();
                button.Left -= 8 * num;
                num--;
            }
            Program.Settings.Content.Download.CachePreviewImagesChanged += new PropertyChangedEventHandler(this.Download_CachePreviewImagesChanged);
        }

        private void ActivePanel_Paint(object sender, PaintEventArgs e)
        {
            if (this.FirstPaint)
            {
                this.OnFirstPaint();
            }
            using (Pen pen = new Pen(Program.Settings.StylePreferences.HighlightColor3, 4f))
            {
                e.Graphics.DrawRectangle(pen, this.ActivePanel.ClientRectangle);
            }
        }

        private void AdditionalContent_BeginDeleteMyUpload(ContentOperationCallbackArgs e)
        {
        }

        private void AdditionalContent_BeginUploadContent(ContentOperationCallbackArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.AdditionalContent_BeginUploadContent(e);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                this.PreviousUploadStatus = this.NodeUploadSearch.Text;
                base.SetStatus("<LOC>Uploading {0}...", 0xbb8, new object[] { e.Content.ContentType.SingularDisplayName });
                this.UploadStatus("<LOC>Uploading {0}...", new object[] { e.Content.ContentType.SingularDisplayName });
                this.PreviousUploadStatus = this.NodeUploadSearch.Text;
                this.RefreshToolstrip();
                foreach (TreeNode node in this.treeViewUpload.Nodes.Find(this.TreeNodeName(e.Content), true))
                {
                    node.ImageIndex = 4;
                    node.SelectedImageIndex = 4;
                }
                this.ChangePanel(ContentVaultTabs.Activity);
            }
        }

        private void AdditionalContent_FinishDelete(ContentOperationCallbackArgs e)
        {
            this.RefreshMyContent();
            this.BindToMyContent();
        }

        private void AdditionalContent_FinishDeleteMyUpload(ContentOperationCallbackArgs e)
        {
            try
            {
                bool allVersions = (bool) e.Args[0];
                TreeNode[] nodes = this.NodeMyUploads.Nodes.Find(this.TreeNodeName(e.Content), true);
                if (!base.Disposing && !base.IsDisposed)
                {
                    base.BeginInvoke((VGen0)delegate {
                        Predicate<IAdditionalContent> match = null;
                        Predicate<IAdditionalContent> predicate2 = null;
                        string localFilePath = e.Content.LocalFilePath;
                        foreach (TreeNode node in nodes)
                        {
                            if (!e.Content.CanVersion || allVersions)
                            {
                                node.Remove();
                                if (match == null)
                                {
                                    match = delegate (IAdditionalContent content) {
                                        return (content.TypeID == e.Content.TypeID) && (content.Name == e.Content.Name);
                                    };
                                }
                                this.MyUploads.RemoveAll(match);
                            }
                            else if (!(node.Parent.Tag is ContentType) && ((node.Tag as IAdditionalContent).ID == e.Content.ID))
                            {
                                IAdditionalContent tag = node.Tag as IAdditionalContent;
                                if (tag.Version == tag.CurrentVersion)
                                {
                                    if (predicate2 == null)
                                    {
                                        predicate2 = delegate (IAdditionalContent content) {
                                            return ((content.TypeID == e.Content.TypeID) && (content.Name == e.Content.Name)) && (content.Version == content.CurrentVersion);
                                        };
                                    }
                                    this.MyUploads.RemoveAll(predicate2);
                                    if ((node.NextNode == null) || (node.Parent.Nodes.Count <= 1))
                                    {
                                        localFilePath = (node.Parent.Tag as IAdditionalContent).LocalFilePath;
                                        TreeNode root = node.Parent.Parent;
                                        node.Parent.Remove();
                                        this.RefreshToolstrip();
                                        this.TreeRootCount(root);
                                        goto Label_02BE;
                                    }
                                    IAdditionalContent content2 = AdditionalContent.Clone(node.NextNode.Tag as IAdditionalContent);
                                    content2.LocalFilePath = (node.Parent.Tag as IAdditionalContent).LocalFilePath;
                                    node.Parent.Tag = content2;
                                    this.MyUploads.Add(node.Parent.Tag as IAdditionalContent);
                                    foreach (IAdditionalContent content3 in this.MyUploads)
                                    {
                                        if ((content3.TypeID == tag.TypeID) && (content3.Name == tag.Name))
                                        {
                                            content3.CurrentVersion--;
                                        }
                                    }
                                }
                                node.Remove();
                                break;
                            }
                        }
                    Label_02BE:
                        foreach (TreeNode node3 in this.NodeMyUploads.Nodes)
                        {
                            if (node3.Nodes.Count < 1)
                            {
                                node3.Remove();
                            }
                            else
                            {
                                this.TreeRootCount(node3);
                            }
                        }
                        if (Path.HasExtension(e.Content.LocalFilePath))
                        {
                            this.ValidateUploadFile(e.Content.LocalFilePath, true);
                        }
                        else if ((e.Content.LocalFilePath != null) && (e.Content.LocalFilePath.Length > 0))
                        {
                            foreach (string str2 in Directory.GetFiles(e.Content.LocalFilePath))
                            {
                                this.ValidateUploadFile(str2, true);
                            }
                        }
                        this.RefreshToolstrip();
                    });
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void AdditionalContent_FinishUploadContent(ContentOperationCallbackArgs e)
        {
            WaitCallback callBack = null;
            VGen0 method = null;
            IAdditionalContent content = e.Content;
            try
            {
                VGen0 gen = null;
                IAdditionalContent item = AdditionalContent.Clone(content);
                this.MyUploads.Add(item);
                foreach (IAdditionalContent content2 in this.MyUploads)
                {
                    if ((content2.TypeID == content.TypeID) && (content2.Name == content.Name))
                    {
                        content2.CurrentVersion = content.Version;
                    }
                }
                this.AddUploadContentNode(this.NodeMyUploads, item, false);
                TreeNode[] availableNodes = this.NodeAvailableUploads.Nodes.Find(this.TreeNodeName(content), true);
                if (availableNodes.Length > 0)
                {
                    if (base.Disposing || base.IsDisposed)
                    {
                        return;
                    }
                    if (gen == null)
                    {
                        gen = delegate {
                            foreach (TreeNode node in availableNodes)
                            {
                                TreeNode root = node.Parent;
                                node.Remove();
                                this.TreeRootCount(root);
                                if (this.SelectedUploadTreeNode == node)
                                {
                                    this.BindToUploadContent();
                                }
                            }
                        };
                    }
                    base.Invoke(gen);
                }
                base.SetStatus("<LOC>Upload complete.", 0xbb8, new object[0]);
                this.UploadStatus("<LOC>Upload complete.", new object[0]);
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        Thread.Sleep(0xbb8);
                        this.UploadStatus(this.PreviousUploadStatus, new object[0]);
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                base.SetStatus("<LOC>An error occured during upload.", 0xbb8, new object[0]);
            }
            finally
            {
                if (!base.Disposing && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            foreach (TreeNode node in this.treeViewUpload.Nodes.Find(this.TreeNodeName(content), true))
                            {
                                node.ImageIndex = content.ContentType.ImageIndex;
                                node.SelectedImageIndex = content.ContentType.ImageIndex;
                            }
                            this.RefreshToolstrip();
                        };
                    }
                    base.BeginInvoke(method);
                }
            }
        }

        private void AddUploadContentNode(TreeNode category, IAdditionalContent content)
        {
            this.AddUploadContentNode(category, content, true);
        }

        private void AddUploadContentNode(TreeNode category, IAdditionalContent content, bool append)
        {
            try
            {
                TreeNode root;
                TreeNode contentNode;
                ContentType contentType = content.ContentType;
                bool newRoot = false;
                TreeNode[] nodeArray = category.Nodes.Find(string.Format("{0}_{1}", category.Name, contentType.Name), false);
                if (nodeArray.Length > 0)
                {
                    root = nodeArray[0];
                }
                else
                {
                    root = new TreeNode(Loc.Get(contentType.DisplayName), contentType.ImageIndex, contentType.ImageIndex);
                    root.Tag = contentType;
                    root.Name = string.Format("{0}_{1}", category.Name, contentType.Name);
                    newRoot = true;
                }
                int img = contentType.ImageIndex;
                if ((content.LocalFilePath == null) || (content.LocalFilePath.Length < 1))
                {
                    img = 7;
                }
                if (content.CurrentUserIsOwner && content.CanVersion)
                {
                    VGen0 method = null;
                    string key = this.TreeNodeName(content);
                    TreeNode[] nodes = root.Nodes.Find(key, false);
                    if (nodes.Length > 0)
                    {
                        IAdditionalContent tag = nodes[0].Tag as IAdditionalContent;
                        if (content.Version >= tag.Version)
                        {
                            nodes[0].Tag = AdditionalContent.Clone(content);
                            this.MyUploads.Add(nodes[0].Tag as IAdditionalContent);
                            if (nodes[0].ImageIndex != img)
                            {
                                if (base.Disposing || base.IsDisposed)
                                {
                                    return;
                                }
                                if (method == null)
                                {
                                    method = delegate {
                                        nodes[0].ImageIndex = img;
                                        nodes[0].SelectedImageIndex = img;
                                    };
                                }
                                base.Invoke(method);
                            }
                        }
                        root = nodes[0];
                        contentNode = new TreeNode(string.Format(Loc.Get("<LOC>Version {0}"), content.Version), img, img);
                        contentNode.Name = this.TreeNodeName(content);
                        contentNode.Tag = content;
                    }
                    else
                    {
                        contentNode = new TreeNode(content.Name, img, img);
                        contentNode.Name = this.TreeNodeName(content);
                        contentNode.Tag = AdditionalContent.Clone(content);
                        this.MyUploads.Add(contentNode.Tag as IAdditionalContent);
                        TreeNode node = new TreeNode(string.Format(Loc.Get("<LOC>Version {0}"), content.Version), img, img);
                        node.Name = this.TreeNodeName(content);
                        node.Tag = content;
                        contentNode.Nodes.Add(node);
                    }
                }
                else
                {
                    contentNode = new TreeNode(content.Name, img, img);
                    contentNode.Name = this.TreeNodeName(content);
                    contentNode.Tag = content;
                }
                if (((contentNode != null) && !base.Disposing) && !base.IsDisposed)
                {
                    if (!content.CurrentUserIsOwner)
                    {
                        foreach (TreeNode node2 in root.Nodes.Find(contentNode.Name, false))
                        {
                            if ((node2.Tag as IAdditionalContent).LocalFilePath == content.LocalFilePath)
                            {
                                return;
                            }
                        }
                    }
                    if (!base.Disposing && !base.IsDisposed)
                    {
                        base.Invoke((VGen0)delegate {
                            if (newRoot)
                            {
                                category.Nodes.Add(root);
                                if (!category.IsExpanded)
                                {
                                    category.Expand();
                                }
                            }
                            if (append)
                            {
                                root.Nodes.Add(contentNode);
                            }
                            else
                            {
                                root.Nodes.Insert(0, contentNode);
                            }
                            if (!(!(root.Tag is ContentType) || root.IsExpanded))
                            {
                                root.Expand();
                            }
                            this.TreeRootCount(root);
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void BindToDownloadType()
        {
            if (this.SelectedDownloadOptions != null)
            {
                this.gpgPanelSearchCriteria.Controls.Remove(this.SelectedDownloadOptions);
            }
            if (this.ResultColumns != null)
            {
                foreach (GridColumn column in this.ResultColumns)
                {
                    this.gvResults.Columns.Remove(column);
                }
            }
            if (((this.treeViewDownloadType.SelectedNode != null) && (this.treeViewDownloadType.SelectedNode.Tag != null)) && (this.treeViewDownloadType.SelectedNode.Tag is ContentType))
            {
                Program.Settings.Content.Download.SearchType = this.treeViewDownloadType.SelectedNode.Name;
                ContentType tag = this.treeViewDownloadType.SelectedNode.Tag as ContentType;
                this.SelectedDownloadContent = tag.CreateInstance().CreateEmptyInstance();
                this.RefreshContentLists();
                ContentOptions downloadOptions = this.SelectedDownloadContent.GetDownloadOptions();
                if (this.CurrentSearchView == SearchViews.Advanced)
                {
                    if (downloadOptions.HasOptions)
                    {
                        if (this.SelectedDownloadOptions != null)
                        {
                            int num = downloadOptions.OptionsControl.Height - this.SelectedDownloadOptions.Height;
                            this.gpgGroupBoxCriteria.Height += num;
                            this.gpgGroupBoxResults.Top += num;
                            this.gpgGroupBoxResults.Height -= num;
                        }
                    }
                    else
                    {
                        this.ToggleSearchView();
                    }
                }
                else if (downloadOptions.HasOptions)
                {
                    downloadOptions.OptionsControl.Visible = false;
                }
                if (downloadOptions.HasOptions)
                {
                    int num2 = 4;
                    this.SelectedDownloadOptions = downloadOptions.OptionsControl;
                    this.SelectedDownloadOptions.Location = new Point(this.gpgTextBoxSearchName.Left, this.gpgTextBoxSearchName.Bottom + num2);
                    this.gpgPanelSearchCriteria.Controls.Add(this.SelectedDownloadOptions);
                    this.skinButtonSearchType.Enabled = true;
                }
                else
                {
                    this.SelectedDownloadOptions = null;
                    this.skinButtonSearchType.Enabled = false;
                }
                this.gvResults.Columns.Clear();
                this.gvResults.Columns.AddRange(this.DefaultGridColumns);
                this.gvResults.PreviewFieldName = "Description";
                this.dataGridSearchResults.DataSource = null;
                this.skinLabelSearchPage.Text = Loc.Get("<LOC>No Results");
                this.SelectedDownloadContent.SetGridView(this.gvResults);
                this.gvResults.BestFitColumns();
                this.ExecuteSearch();
            }
        }

        private void BindToMyContent()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.BindToMyContent();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                if ((this.treeViewMyContent.SelectedNode == null) || (this.treeViewMyContent.SelectedNode.Tag == null))
                {
                    if (this.CurrentContentView != null)
                    {
                        this.CurrentContentView.Visible = false;
                    }
                    this.gpgLabelMyContent.Text = Loc.Get("<LOC>No valid content selected. Please select content from the browser on the left.");
                    this.gpgLabelMyContent.Visible = true;
                }
                else if (this.treeViewMyContent.SelectedNode.Tag is string)
                {
                    if (this.CurrentContentView != null)
                    {
                        this.CurrentContentView.Visible = false;
                    }
                    this.gpgLabelMyContent.Text = Loc.Get((string) this.treeViewMyContent.SelectedNode.Tag);
                    this.gpgLabelMyContent.Visible = true;
                }
                else if (this.treeViewMyContent.SelectedNode.Tag is IAdditionalContent)
                {
                    IAdditionalContent tag = this.treeViewMyContent.SelectedNode.Tag as IAdditionalContent;
                    if (this.CurrentContentView == null)
                    {
                        this.CurrentContentView = new PnlContentDetailsView(this);
                        this.CurrentContentView.Location = new Point(6, 6);
                        this.splitContainerMyContent.Panel2.Controls.Add(this.CurrentContentView);
                    }
                    this.CurrentContentView.BindToMyContent(tag);
                    this.CurrentContentView.MinimumSize = this.CurrentContentView.Size;
                    this.CurrentContentView.Visible = true;
                    this.gpgLabelMyContent.Visible = false;
                }
                else
                {
                    if (this.CurrentContentView != null)
                    {
                        this.CurrentContentView.Visible = false;
                    }
                    this.gpgLabelMyContent.Text = Loc.Get("<LOC>No valid content selected. Please select content from the browser on the left.");
                    this.gpgLabelMyContent.Visible = true;
                }
            }
        }

        private void BindToUploadContent()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.BindToUploadContent();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                this.SelectedUploadTreeNode = this.treeViewUpload.SelectedNode;
                int num = 6;
                this.SelectedUpload = null;
                if ((this.treeViewUpload.SelectedNode == null) || (this.treeViewUpload.SelectedNode.Tag == null))
                {
                    this.gpgGroupBoxUploadGeneral.Visible = false;
                    this.gpgGroupBoxUploadVersion.Visible = false;
                    this.gpgLabelNoUploadSelected.Visible = true;
                    this.skinButtonUpload.Visible = false;
                    this.skinButtonDeleteUploadAll.Visible = false;
                    this.skinButtonDeleteUploadVersion.Visible = false;
                    this.gpgLabelNoUploadSelected.Text = Loc.Get("<LOC>No valid content selected. Please select content from the browser on the left.");
                    this.gpgLabelNoUploadSelected.TextStyle = TextStyles.Default;
                }
                else
                {
                    if (this.treeViewUpload.SelectedNode.Tag is string)
                    {
                        this.gpgGroupBoxUploadGeneral.Visible = false;
                        this.gpgGroupBoxUploadVersion.Visible = false;
                        this.gpgLabelNoUploadSelected.Visible = true;
                        this.skinButtonUpload.Visible = false;
                        this.skinButtonDeleteUploadAll.Visible = false;
                        this.skinButtonDeleteUploadVersion.Visible = false;
                        this.gpgLabelNoUploadSelected.Text = (string) this.treeViewUpload.SelectedNode.Tag;
                        this.gpgLabelNoUploadSelected.TextStyle = TextStyles.Default;
                    }
                    else if (this.treeViewUpload.SelectedNode.Tag is IAdditionalContent)
                    {
                        IAdditionalContent tag = this.treeViewUpload.SelectedNode.Tag as IAdditionalContent;
                        if (tag.ContentType.HasVolunteeredForUploads)
                        {
                            ContentOptions detailsView;
                            bool currentUserIsOwner = tag.CurrentUserIsOwner;
                            bool flag2 = this.treeViewUpload.SelectedNode.Parent.Tag is ContentType;
                            this.gpgGroupBoxUploadGeneral.Visible = true;
                            this.gpgLabelNoUploadSelected.Visible = false;
                            this.skinButtonUpload.Visible = true;
                            if (!(!currentUserIsOwner || flag2))
                            {
                                this.gpgLabelUploadLocation.Text = Loc.Get("<LOC>Archived view only");
                                this.gpgLabelUploadLocation.TextStyle = TextStyles.Default;
                            }
                            else if (currentUserIsOwner && flag2)
                            {
                                if ((tag.LocalFilePath == null) || ((tag.LocalFilePath.Length < 1) && (tag.Version == tag.CurrentVersion)))
                                {
                                    this.gpgLabelUploadLocation.Text = Loc.Get("<LOC>Not found, click here to locate");
                                }
                                else
                                {
                                    this.gpgLabelUploadLocation.Text = tag.LocalFilePath;
                                }
                                this.gpgLabelUploadLocation.TextStyle = TextStyles.Link;
                            }
                            else
                            {
                                this.gpgLabelUploadLocation.Text = tag.LocalFilePath;
                                this.gpgLabelUploadLocation.TextStyle = TextStyles.Default;
                            }
                            this.gpgTextBoxUploadDesc.Text = tag.Description;
                            this.gpgTextBoxUploadName.Text = tag.Name;
                            this.gpgTextBoxUploadSearchKeywords.Text = tag.SearchKeywords;
                            if (this.CurrentUploadOptions != null)
                            {
                                this.gpgGroupBoxUploadGeneral.Controls.Remove(this.CurrentUploadOptions);
                                this.gpgGroupBoxUploadGeneral.Height -= this.CurrentUploadOptions.Height + num;
                                this.CurrentUploadOptions = null;
                            }
                            if (!flag2)
                            {
                                detailsView = tag.GetDetailsView();
                            }
                            else
                            {
                                detailsView = tag.GetUploadOptions();
                            }
                            if (detailsView.HasOptions)
                            {
                                this.CurrentUploadOptions = detailsView.OptionsControl;
                                this.gpgGroupBoxUploadGeneral.Height += detailsView.OptionsControl.Height + num;
                                this.gpgGroupBoxUploadGeneral.Controls.Add(detailsView.OptionsControl);
                                detailsView.OptionsControl.Top = this.gpgPanelUploadGeneral.Bottom + num;
                                detailsView.OptionsControl.Left = this.gpgPanelUploadGeneral.Left;
                                detailsView.OptionsControl.Width = this.gpgPanelUploadGeneral.Width;
                                this.gpgGroupBoxUploadVersion.Top = this.gpgGroupBoxUploadGeneral.Bottom + num;
                                this.gpgGroupBoxUploadGeneral.Text = Loc.Get(detailsView.Name);
                            }
                            else
                            {
                                this.gpgGroupBoxUploadGeneral.Text = Loc.Get("<LOC>Upload Options");
                            }
                            if (currentUserIsOwner && tag.CanVersion)
                            {
                                this.gpgTextBoxUploadName.Enabled = false;
                                this.gpgGroupBoxUploadVersion.Visible = true;
                                if (flag2)
                                {
                                    this.gpgTextBoxUploadDesc.Enabled = true;
                                    this.gpgTextBoxUploadSearchKeywords.Enabled = true;
                                    this.gpgTextAreaVersionNotes.Enabled = true;
                                    this.gpgLabelUploadCurrentVersion.Text = string.Format(Loc.Get("<LOC>Current version: {0}, version date: {1}"), tag.Version, tag.VersionDate.ToShortDateString());
                                    this.gpgTextAreaVersionNotes.Text = "";
                                }
                                else
                                {
                                    this.gpgTextBoxUploadDesc.Enabled = false;
                                    this.gpgTextBoxUploadSearchKeywords.Enabled = false;
                                    this.gpgTextAreaVersionNotes.Enabled = false;
                                    this.gpgLabelUploadCurrentVersion.Text = string.Format(Loc.Get("<LOC>Version: {0}, version date: {1}"), tag.Version, tag.VersionDate.ToShortDateString());
                                    this.gpgTextAreaVersionNotes.Text = tag.VersionNotes;
                                }
                                this.skinButtonUpload.Top = this.gpgGroupBoxUploadVersion.Bottom + num;
                                this.skinButtonUpload.Text = Loc.Get("<LOC>Upload Version");
                                this.skinButtonDeleteUploadAll.Top = this.gpgGroupBoxUploadVersion.Bottom + num;
                                this.skinButtonDeleteUploadAll.Visible = true;
                                this.skinButtonDeleteUploadVersion.Top = this.gpgGroupBoxUploadVersion.Bottom + num;
                                this.skinButtonDeleteUploadVersion.Text = Loc.Get("<LOC>Delete Version");
                                this.skinButtonDeleteUploadVersion.Visible = true;
                                this.gpgGroupBoxUploadVersion.Top = this.gpgGroupBoxUploadGeneral.Bottom + num;
                                this.gpgLabelUploadVersionNotes.Text = Loc.Get("<LOC>Version notes");
                            }
                            else
                            {
                                if (tag is CustomApplication)
                                {
                                    this.gpgTextBoxUploadName.Enabled = true;
                                }
                                else if ((tag.Name != null) && (tag.Name.Length > 0))
                                {
                                    this.gpgTextBoxUploadName.Enabled = false;
                                }
                                else
                                {
                                    this.gpgTextBoxUploadName.Enabled = true;
                                }
                                this.gpgGroupBoxUploadVersion.Visible = false;
                                this.skinButtonUpload.Top = this.gpgGroupBoxUploadGeneral.Bottom + num;
                                this.skinButtonUpload.Text = Loc.Get("<LOC>Upload");
                                this.skinButtonDeleteUploadAll.Visible = false;
                                if (currentUserIsOwner)
                                {
                                    this.skinButtonDeleteUploadVersion.Top = this.gpgGroupBoxUploadGeneral.Bottom + num;
                                    this.skinButtonDeleteUploadVersion.Visible = true;
                                    this.skinButtonDeleteUploadVersion.Text = Loc.Get("<LOC>Delete");
                                }
                                else
                                {
                                    this.skinButtonDeleteUploadVersion.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            this.gpgGroupBoxUploadGeneral.Visible = false;
                            this.gpgGroupBoxUploadVersion.Visible = false;
                            this.gpgLabelNoUploadSelected.Visible = true;
                            this.skinButtonUpload.Visible = false;
                            this.skinButtonDeleteUploadAll.Visible = false;
                            this.skinButtonDeleteUploadVersion.Visible = false;
                            this.gpgLabelNoUploadSelected.Text = Loc.Get("<LOC>To upload this type of content, you must first agree to its legal conditions, click here to volunteer to be an author for this type of content.");
                            this.gpgLabelNoUploadSelected.TextStyle = TextStyles.Link;
                        }
                        this.SelectedUpload = tag;
                        this.RefreshUploadDependencies();
                    }
                    else
                    {
                        this.gpgGroupBoxUploadGeneral.Visible = false;
                        this.gpgGroupBoxUploadVersion.Visible = false;
                        this.gpgLabelNoUploadSelected.Visible = true;
                        this.skinButtonUpload.Visible = false;
                        this.skinButtonDeleteUploadAll.Visible = false;
                        this.skinButtonDeleteUploadVersion.Visible = false;
                        this.gpgLabelNoUploadSelected.Text = Loc.Get("<LOC>No valid content selected. Please select content from the browser on the left.");
                        this.gpgLabelNoUploadSelected.TextStyle = TextStyles.Default;
                    }
                    this.RefreshToolstrip();
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowDlgSolution(ConfigSettings.GetInt("ContentManagerHelp", 0x4b8));
        }

        private void btnMapDiagnose_Click(object sender, EventArgs e)
        {
            new DlgMapDiagnostics().Show();
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            new DlgVaultOptions().ShowDialog();
        }

        private void btnUploadHistory_Click(object sender, EventArgs e)
        {
        }

        private void ChangePanel(ContentVaultTabs tab)
        {
            if (this.CurrentTab != tab)
            {
                this.CurrentTab = tab;
                SkinButton tabMyContent = null;
                if (this.ActivePanel != null)
                {
                    this.ActivePanel.Paint -= new PaintEventHandler(this.ActivePanel_Paint);
                }
                switch (tab)
                {
                    case (ContentVaultTabs.None | ContentVaultTabs.MyContent):
                        this.ActivePanel = this.gpgPanelMyContent;
                        tabMyContent = this.tabMyContent;
                        break;

                    case ContentVaultTabs.Download:
                        this.ActivePanel = this.gpgPanelDownload;
                        tabMyContent = this.tabDownload;
                        break;

                    case ContentVaultTabs.Upload:
                        this.ActivePanel = this.gpgPanelUpload;
                        tabMyContent = this.tabUpload;
                        break;

                    case ContentVaultTabs.Activity:
                        this.ActivePanel = this.gpgPanelActivity;
                        tabMyContent = this.tabActivity;
                        break;
                }
                SkinButton[] buttonArray = new SkinButton[] { this.tabActivity, this.tabUpload, this.tabMyContent, this.tabDownload };
                foreach (SkinButton button2 in buttonArray)
                {
                    if (button2 != tabMyContent)
                    {
                        button2.BringToFront();
                        button2.DrawColor = Color.White;
                        button2.SkinBasePath = @"Dialog\ContentManager\TabLarge";
                    }
                }
                tabMyContent.BringToFront();
                tabMyContent.DrawColor = Color.Black;
                tabMyContent.SkinBasePath = @"Dialog\ContentManager\TabLargeActive";
                this.ActivePanel.Paint += new PaintEventHandler(this.ActivePanel_Paint);
                this.ActivePanel.BringToFront();
                this.ActivePanel.Invalidate();
                this.ToggleToolstripVisibility();
            }
        }

        private void ChangePanel(object sender, EventArgs e)
        {
            this.ChangePanel((ContentVaultTabs) (sender as Control).Tag);
        }

        private void CheckCachedLocations()
        {
            try
            {
                try
                {
                    this.ScanningForUploads = true;
                    foreach (string str in Program.Settings.Content.Upload.CachedLocations.ToArray())
                    {
                        bool flag = false;
                        foreach (string str2 in Program.Settings.Content.Upload.UploadPaths)
                        {
                            if (str.StartsWith(str2))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!(flag && this.ValidateUploadFile(str, true)))
                        {
                            Program.Settings.Content.Upload.CachedLocations.Remove(str);
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
            finally
            {
            }
        }

        private bool CheckExistence(TreeNode root)
        {
            VGen0 method = null;
            List<TreeNode> remove;
            if (!this.CancelUploadSearch)
            {
                if ((root.Tag != null) && (root.Tag is IAdditionalContent))
                {
                    IAdditionalContent tag = root.Tag as IAdditionalContent;
                    if (tag.LocalFilePath == null)
                    {
                        return false;
                    }
                    bool flag = false;
                    foreach (string str in Program.Settings.Content.Upload.UploadPaths)
                    {
                        if ((tag.LocalFilePath.TrimEnd(new char[] { '\\' }) + @"\").IndexOf(str.TrimEnd(new char[] { '\\' }) + @"\") >= 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!(flag && (Directory.Exists(tag.LocalFilePath) || File.Exists(tag.LocalFilePath))))
                    {
                        return false;
                    }
                }
                remove = new List<TreeNode>();
                foreach (TreeNode node in root.Nodes)
                {
                    if (!this.CheckExistence(node))
                    {
                        remove.Add(node);
                    }
                }
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            foreach (TreeNode node in remove)
                            {
                                root.Nodes.Remove(node);
                            }
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!base.Disposing && !base.IsDisposed)
                {
                    foreach (TreeNode node in remove)
                    {
                        root.Nodes.Remove(node);
                        if ((root.Tag != null) && (root.Tag is ContentType))
                        {
                            root.Text = string.Format("{0} ({1})", Loc.Get((root.Tag as ContentType).DisplayName), root.Nodes.Count);
                        }
                    }
                }
            }
            return true;
        }

        private void dataGridSearchResults_DoubleClick(object sender, EventArgs e)
        {
            if (this.MouseOverDownload != null)
            {
                DlgContentDetailsView view = DlgContentDetailsView.CreateOrGetExisting(this.MouseOverDownload);
                view.Show();
                view.BringToFront();
            }
        }

        private void DeleteUploadNode(IAdditionalContent content)
        {
            this.DeleteUploadNode(content.LocalFilePath);
        }

        private void DeleteUploadNode(string file)
        {
            if (this.UploadFileNodes.ContainsKey(file))
            {
                this.DeleteUploadNode(this.UploadFileNodes[file]);
            }
        }

        private void DeleteUploadNode(TreeNode node)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        node.Parent.Nodes.Remove(node);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                node.Parent.Nodes.Remove(node);
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

        private void Download_CachePreviewImagesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Program.Settings.Content.Download.CachePreviewImages && Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\vault preview images"))
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\vault preview images");
            }
        }

        private void DownloadCompleted(ContentOperationCallbackArgs e)
        {
            VGen0 method = null;
            try
            {
                base.SetStatus("<LOC>Download Complete.", 0xbb8, new object[0]);
                if (((e.CompletedSuccessfully && base.IsHandleCreated) && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.gvResults.InvalidateRows();
                        };
                    }
                    base.BeginInvoke(method);
                    this.RefreshMyContent();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void DownloadContent(IAdditionalContent content)
        {
            AdditionalContent.DownloadContent(content, this);
        }

        private void DownloadStarted(ContentOperationCallbackArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.DownloadStarted(e);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    base.SetStatus("<LOC>Downloading {0}...", 0xbb8, new object[] { e.Content.ContentType.SingularDisplayName });
                    this.gvResults.InvalidateRows();
                    this.ChangePanel(ContentVaultTabs.Activity);
                    this.Cursor = Cursors.Default;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void ExecuteSearch()
        {
            this.ExecuteSearch(-1);
        }

        private void ExecuteSearch(int revertPage)
        {
            try
            {
                QuazalQuery query;
                if ((this.SelectedDownloadContent != null) && this.SelectedDownloadContent.ContentType.CurrentUserCanDownload)
                {
                    if (this.SelectedContentList != null)
                    {
                        if (this.SelectedContentList.ListType == ContentListTypes.StoredQuery)
                        {
                            this.SelectedContentList.Query.Args = new object[] { this.SearchPage * this.SearchPageSize, this.SearchPageSize };
                        }
                        query = this.SelectedContentList.Query;
                    }
                    else
                    {
                        query = this.SelectedDownloadContent.CreateSearchQuery(this.SearchPage, this.SearchPageSize);
                    }
                    if ((query != null) && !query.IsEmpty)
                    {
                        this.Searching = true;
                        this.RefreshToolstrip();
                        base.SetStatus("Searching...", new object[0]);
                        this.ViewingVersionHistory = false;
                        ThreadPool.QueueUserWorkItem(delegate (object s) {
                            VGen0 method = null;
                            DataList data = query.GetData();
                            if (data != null)
                            {
                                if ((data.Count == 0) && (revertPage >= 0))
                                {
                                    this.SearchPage = revertPage;
                                    this.ClearStatus();
                                    this.Searching = false;
                                    this.RefreshToolstrip();
                                }
                                else
                                {
                                    List<IAdditionalContent> list2 = new List<IAdditionalContent>(data.Count);
                                    for (int j = 0; j < data.Count; j++)
                                    {
                                        IAdditionalContent item = MappedObject.FromData(this.SelectedDownloadContent.GetType(), data[j]) as IAdditionalContent;
                                        if (item.CurrentUserCanDownload)
                                        {
                                            list2.Add(item);
                                        }
                                    }
                                    this.SearchResults = list2.ToArray();
                                    if (!this.Disposing && !this.IsDisposed)
                                    {
                                        if (method == null)
                                        {
                                            method = delegate {
                                                this.dataGridSearchResults.DataSource = null;
                                                this.dataGridSearchResults.DataSource = this.SearchResults;
                                                this.gvResults.InvalidateRows();
                                                this.gpgGroupBoxResults.Text = Loc.Get("<LOC>Search Results");
                                                if (this.SearchResults.Length > 0)
                                                {
                                                    this.skinLabelSearchPage.Text = string.Format(Loc.Get("<LOC>Page {0}"), this.SearchPage + 1);
                                                }
                                                else
                                                {
                                                    this.skinLabelSearchPage.Text = Loc.Get("<LOC>No Results");
                                                }
                                                this.ClearStatus();
                                                this.Searching = false;
                                                this.RefreshToolstrip();
                                            };
                                        }
                                        this.BeginInvoke(method);
                                    }
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void ExecuteVersionSearch(IAdditionalContent content)
        {
            try
            {
                QuazalQuery query;
                this.SelectedContentList = null;
                if (content != null)
                {
                    query = content.CreateVersionQuery();
                    if (!query.IsEmpty)
                    {
                        this.Searching = true;
                        this.RefreshToolstrip();
                        base.SetStatus("Searching...", new object[0]);
                        this.ViewingVersionHistory = true;
                        ThreadPool.QueueUserWorkItem(delegate (object s) {
                            VGen0 method = null;
                            DataList data = query.GetData();
                            List<IAdditionalContent> list2 = new List<IAdditionalContent>(data.Count);
                            for (int j = 0; j < data.Count; j++)
                            {
                                IAdditionalContent item = MappedObject.FromData(this.SelectedDownloadContent.GetType(), data[j]) as IAdditionalContent;
                                if (item.CurrentUserCanDownload)
                                {
                                    list2.Add(item);
                                }
                            }
                            this.SearchResults = list2.ToArray();
                            if (!this.Disposing && !this.IsDisposed)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.dataGridSearchResults.DataSource = null;
                                        this.dataGridSearchResults.DataSource = this.SearchResults;
                                        this.gvResults.InvalidateRows();
                                        this.gpgGroupBoxResults.Text = Loc.Get("<LOC>Version History");
                                        if (this.SearchResults.Length > 0)
                                        {
                                            this.skinLabelSearchPage.Text = string.Format(Loc.Get("<LOC>Page {0}"), this.SearchPage + 1);
                                        }
                                        else
                                        {
                                            this.skinLabelSearchPage.Text = Loc.Get("<LOC>No Results");
                                        }
                                        this.ClearStatus();
                                        this.Searching = false;
                                        this.RefreshToolstrip();
                                    };
                                }
                                this.BeginInvoke(method);
                            }
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gpgLabelUploadLocation_Click(object sender, EventArgs e)
        {
            if ((this.SelectedUpload != null) && this.SelectedUpload.CurrentUserIsOwner)
            {
                IAdditionalContent content;
                OpenFileDialog dialog = new OpenFileDialog();
                string str = "";
                string str2 = "";
                foreach (string str3 in this.SelectedUpload.ContentType.FileExtensions)
                {
                    string str4 = str;
                    str = str4 + str2 + "*" + str3 + str2;
                    str2 = ";";
                }
                dialog.Filter = string.Format("{0} files ({1})|{1}", Loc.Get(this.SelectedUpload.ContentType.SingularDisplayName), str);
                dialog.Multiselect = false;
                dialog.RestoreDirectory = true;
                if ((dialog.ShowDialog() == DialogResult.OK) && this.SelectedUpload.ContentType.CreateInstance().FromLocalFile(dialog.FileName, out content))
                {
                    this.SelectedUpload.MergeWithLocalVersion(content);
                    this.BindToUploadContent();
                }
            }
        }

        private void gvResults_CalcRowHeight(object sender, RowHeightEventArgs e)
        {
            int height;
            int num = 4;
            string text = "0kb";
            using (Graphics graphics = base.CreateGraphics())
            {
                height = (int) DrawUtil.MeasureString(graphics, text, this.DownloadSizeFont).Height;
            }
            int num3 = (VaultImages.download_up.Height + height) + (num * 3);
            if (e.RowHeight < num3)
            {
                e.RowHeight = num3;
            }
        }

        private void gvResults_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            IAdditionalContent row;
            SizeF ef;
            int height;
            Image image;
            Rectangle rectangle;
            if (e.Column.FieldName == "Version")
            {
                row = this.gvResults.GetRow(e.RowHandle) as IAdditionalContent;
                Font font = new Font(this.gcVersion.AppearanceCell.Font, FontStyle.Regular);
                Color masterForeColor = Program.Settings.StylePreferences.MasterForeColor;
                if (e.Bounds.Contains(this.LastMouseLoc))
                {
                    this.Cursor = Cursors.Default;
                }
                int num = 8;
                ef = DrawUtil.MeasureString(e.Graphics, row.Version.ToString(), font);
                height = (int) ef.Height;
                int width = (int) ef.Width;
                int num4 = e.Bounds.Left + num;
                int num5 = (e.Bounds.Top + (e.Bounds.Height / 2)) - (height / 2);
                using (Brush brush = new SolidBrush(masterForeColor))
                {
                    e.Graphics.DrawString(row.Version.ToString(), font, brush, (float) num4, (float) num5);
                }
                if (!this.ViewingVersionHistory && (row.Version > 1))
                {
                    image = VaultImages.btn_history_up;
                    rectangle = new Rectangle((num4 + width) + num, DrawUtil.Center(e.Bounds, image.Size).Y, image.Size.Width, image.Size.Height);
                    if ((rectangle.Contains(this.LastMouseLoc) && (row == this.DlMouseDown)) && ((Control.MouseButtons & MouseButtons.Left) != MouseButtons.None))
                    {
                        image = VaultImages.btn_history_down;
                        if (rectangle.Contains(this.LastMouseLoc))
                        {
                            this.Cursor = Cursors.Hand;
                        }
                        else if (e.Bounds.Contains(this.LastMouseLoc))
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                    else if (rectangle.Contains(this.LastMouseLoc))
                    {
                        image = VaultImages.btn_history_over;
                        this.Cursor = Cursors.Hand;
                    }
                    else if (e.Bounds.Contains(this.LastMouseLoc))
                    {
                        this.Cursor = Cursors.Default;
                    }
                    e.Graphics.DrawImage(image, rectangle);
                }
                e.Handled = true;
            }
            else if (e.Column.FieldName == "Download")
            {
                row = this.gvResults.GetRow(e.RowHandle) as IAdditionalContent;
                int num6 = 4;
                image = VaultImages.download_up;
                string text = string.Format(Loc.Get("{0} kb"), row.DownloadSize.ToString());
                ef = DrawUtil.MeasureString(e.Graphics, text, this.DownloadSizeFont);
                height = (int) ef.Height;
                rectangle = new Rectangle(DrawUtil.Center(e.Bounds, image.Size).X, e.Bounds.Y + num6, image.Size.Width, image.Size.Height);
                if ((AdditionalContent.IsDownloadingContent(row) || AdditionalContent.DownloadExistsLocal(row)) || !AdditionalContent.DownloadsEnabled)
                {
                    image = VaultImages.download_inactive;
                    if (e.Bounds.Contains(this.LastMouseLoc))
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                else if ((rectangle.Contains(this.LastMouseLoc) && (row == this.DlMouseDown)) && ((Control.MouseButtons & MouseButtons.Left) != MouseButtons.None))
                {
                    image = VaultImages.download_down;
                    if (rectangle.Contains(this.LastMouseLoc))
                    {
                        this.Cursor = Cursors.Hand;
                    }
                    else if (e.Bounds.Contains(this.LastMouseLoc))
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                else if (rectangle.Contains(this.LastMouseLoc))
                {
                    image = VaultImages.download_over;
                    this.Cursor = Cursors.Hand;
                }
                else if (e.Bounds.Contains(this.LastMouseLoc))
                {
                    this.Cursor = Cursors.Default;
                }
                e.Graphics.DrawImage(image, rectangle);
                using (SolidBrush brush2 = new SolidBrush(Color.Gray))
                {
                    Point point = DrawUtil.Center(new Rectangle(e.Bounds.X, ((e.Bounds.Y + num6) + image.Height) + num6, e.Bounds.Width, height), new Size((int) ef.Width, height));
                    e.Graphics.DrawString(text, this.DownloadSizeFont, brush2, (PointF) point);
                }
                e.Handled = true;
            }
        }

        private void gvResults_MouseDown(object sender, MouseEventArgs e)
        {
            this.DlMouseDown = null;
            if ((this.MouseOverDownload != null) && !AdditionalContent.IsDownloadingContent(this.MouseOverDownload))
            {
                GridHitInfo info = this.gvResults.CalcHitInfo(e.Location);
                if ((info.InRowCell && ((!AdditionalContent.DownloadExistsLocal(this.MouseOverDownload) || !(info.Column.FieldName != "Version")) && ((info.Column.FieldName == "Download") || (info.Column.FieldName == "Version")))) && (this.Cursor == Cursors.Hand))
                {
                    this.DlMouseDown = this.MouseOverDownload;
                    if (!this.DownloadMouseUpHooked)
                    {
                        this.HookMouseUpForDownloadButton(this);
                        this.DownloadMouseUpHooked = true;
                    }
                }
            }
        }

        private void gvResults_MouseMove(object sender, MouseEventArgs e)
        {
            this.MouseOverDownload = null;
            this.LastMouseLoc = e.Location;
            GridHitInfo hitInfo = this.gvResults.CalcHitInfo(e.Location);
            if (!hitInfo.InRowCell)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.MouseOverDownload = this.gvResults.GetRow(hitInfo.RowHandle) as IAdditionalContent;
                this.gvResults.InvalidateHitObject(hitInfo);
                if (this.LastDownloadHit != null)
                {
                    this.gvResults.InvalidateHitObject(this.LastDownloadHit);
                }
                if ((hitInfo.Column.FieldName != "Version") && (hitInfo.Column.FieldName != "Download"))
                {
                    this.Cursor = Cursors.Default;
                }
                this.LastDownloadHit = hitInfo;
            }
        }

        private void gvResults_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.MouseOverDownload == null)
            {
                this.DlMouseDown = null;
            }
            else
            {
                GridHitInfo info = this.gvResults.CalcHitInfo(e.Location);
                if (info.InRowCell)
                {
                    if (info.Column.FieldName == "Download")
                    {
                        if ((this.Cursor == Cursors.Hand) && (this.DlMouseDown == this.MouseOverDownload))
                        {
                            this.DlMouseDown = null;
                            this.DownloadContent(this.MouseOverDownload);
                            return;
                        }
                    }
                    else if (info.Column.FieldName == "Version")
                    {
                        if (this.ViewingVersionHistory)
                        {
                            return;
                        }
                        if (this.Cursor == Cursors.Hand)
                        {
                            IAdditionalContent row = this.gvResults.GetRow(info.RowHandle) as IAdditionalContent;
                            if (row.CurrentVersion != 1)
                            {
                                this.ExecuteVersionSearch(row);
                            }
                        }
                    }
                    this.DlMouseDown = null;
                    this.gvResults.InvalidateRows();
                }
            }
        }

        private void HookMouseUpForDownloadButton(Control control)
        {
            if (!this.DownloadMouseUpHooked)
            {
                control.MouseUp += new MouseEventHandler(this.Mouse_ButtonUp);
                foreach (Control control2 in control.Controls)
                {
                    this.HookMouseUpForDownloadButton(control2);
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgContentManager));
            this.gpgPanelMyContent = new GPGPanel();
            this.splitContainerMyContent = new SplitContainer();
            this.treeViewMyContent = new TreeView();
            this.imageListContentTypes = new ImageList(this.components);
            this.gpgLabelMyContent = new GPGLabel();
            this.tabMyContent = new SkinButton();
            this.tabDownload = new SkinButton();
            this.tabUpload = new SkinButton();
            this.gpgPanelDownload = new GPGPanel();
            this.splitContainerDownload = new SplitContainer();
            this.treeViewDownloadType = new TreeView();
            this.treeViewSavedSearches = new TreeView();
            this.skinLabel1 = new SkinLabel();
            this.skinLabel2 = new SkinLabel();
            this.gpgGroupBoxResults = new SkinGroupPanel();
            this.skinButtonSearchNext = new SkinButton();
            this.skinLabelSearchPage = new SkinLabel();
            this.dataGridSearchResults = new GPGDataGrid();
            this.gvResults = new GridView();
            this.gcDownload = new GridColumn();
            this.gcName = new GridColumn();
            this.gcOwner = new GridColumn();
            this.gcVersion = new GridColumn();
            this.gcDate = new GridColumn();
            this.gcDownloads = new GridColumn();
            this.gcRating = new GridColumn();
            this.repositoryItemPictureEdit1 = new RepositoryItemPictureEdit();
            this.repositoryItemRatingStars = new RepositoryItemPictureEdit();
            this.repositoryItemChatLink = new RepositoryItemHyperLinkEdit();
            this.repositoryItemVersionLink = new RepositoryItemHyperLinkEdit();
            this.skinButtonSearchStart = new SkinButton();
            this.skinButtonSearchPrevious = new SkinButton();
            this.gpgGroupBoxCriteria = new SkinGroupPanel();
            this.skinButtonSaveSearch = new SkinButton();
            this.skinButtonRunSearch = new SkinButton();
            this.gpgPanelSearchCriteria = new GPGPanel();
            this.skinButtonSearchType = new SkinButton();
            this.gpgLabelSearchType = new GPGLabel();
            this.gpgTextBoxSearchCreator = new GPGTextBox();
            this.gpgLabel16 = new GPGLabel();
            this.gpgTextBoxSearchName = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxSearchKeywords = new GPGTextBox();
            this.gpgLabel21 = new GPGLabel();
            this.gpgPanelUpload = new GPGPanel();
            this.splitContainerUpload = new SplitContainer();
            this.gpgLabelUploadLocations = new GPGLabel();
            this.skinLabel4 = new SkinLabel();
            this.skinLabel3 = new SkinLabel();
            this.pictureBoxRefreshUploads = new PictureBox();
            this.treeViewUpload = new TreeView();
            this.gpgGroupBoxUploadVersion = new SkinGroupPanel();
            this.gpgPanelUploadVersion = new GPGPanel();
            this.gpgTextAreaVersionNotes = new GPGTextArea();
            this.gpgLabelUploadVersionNotes = new GPGLabel();
            this.gpgLabelUploadCurrentVersion = new GPGLabel();
            this.gpgGroupBoxUploadGeneral = new SkinGroupPanel();
            this.gpgPanelUploadGeneral = new GPGPanel();
            this.skinButtonDeleteUploadDependency = new SkinButton();
            this.skinButtonUploadDependency = new SkinButton();
            this.gpgTextBoxUploadSearchKeywords = new GPGTextBox();
            this.listBoxUploadDependencies = new ListBox();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.gpgLabel27 = new GPGLabel();
            this.gpgTextBoxUploadDesc = new GPGTextBox();
            this.gpgLabelUploadLocation = new GPGLabel();
            this.gpgTextBoxUploadName = new GPGTextBox();
            this.gpgLabel25 = new GPGLabel();
            this.gpgLabel29 = new GPGLabel();
            this.skinButtonDeleteUploadVersion = new SkinButton();
            this.skinButtonDeleteUploadAll = new SkinButton();
            this.skinButtonUpload = new SkinButton();
            this.gpgLabelNoUploadSelected = new GPGLabel();
            this.tabActivity = new SkinButton();
            this.msQuickButtons = new GPGMenuStrip(this.components);
            this.btnOptions = new ToolStripMenuItem();
            this.btnHelp = new ToolStripMenuItem();
            this.btnMapDiagnose = new ToolStripMenuItem();
            this.btnMore = new ToolStripMenuItem();
            this.gpgPanel2 = new GPGPanel();
            this.gpgPanelActivity = new GPGPanel();
            this.splitContainerActivity = new SplitContainer();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgPanelDownActivity = new GPGPanel();
            this.skinLabel5 = new SkinLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgPanelUpActivity = new GPGPanel();
            this.skinLabel6 = new SkinLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgPanelMyContent.SuspendLayout();
            this.splitContainerMyContent.Panel1.SuspendLayout();
            this.splitContainerMyContent.Panel2.SuspendLayout();
            this.splitContainerMyContent.SuspendLayout();
            this.gpgPanelDownload.SuspendLayout();
            this.splitContainerDownload.Panel1.SuspendLayout();
            this.splitContainerDownload.Panel2.SuspendLayout();
            this.splitContainerDownload.SuspendLayout();
            this.gpgGroupBoxResults.SuspendLayout();
            this.dataGridSearchResults.BeginInit();
            this.gvResults.BeginInit();
            this.repositoryItemPictureEdit1.BeginInit();
            this.repositoryItemRatingStars.BeginInit();
            this.repositoryItemChatLink.BeginInit();
            this.repositoryItemVersionLink.BeginInit();
            this.gpgGroupBoxCriteria.SuspendLayout();
            this.gpgPanelSearchCriteria.SuspendLayout();
            this.gpgTextBoxSearchCreator.Properties.BeginInit();
            this.gpgTextBoxSearchName.Properties.BeginInit();
            this.gpgTextBoxSearchKeywords.Properties.BeginInit();
            this.gpgPanelUpload.SuspendLayout();
            this.splitContainerUpload.Panel1.SuspendLayout();
            this.splitContainerUpload.Panel2.SuspendLayout();
            this.splitContainerUpload.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxRefreshUploads).BeginInit();
            this.gpgGroupBoxUploadVersion.SuspendLayout();
            this.gpgPanelUploadVersion.SuspendLayout();
            this.gpgTextAreaVersionNotes.Properties.BeginInit();
            this.gpgGroupBoxUploadGeneral.SuspendLayout();
            this.gpgPanelUploadGeneral.SuspendLayout();
            this.gpgTextBoxUploadSearchKeywords.Properties.BeginInit();
            this.gpgTextBoxUploadDesc.Properties.BeginInit();
            this.gpgTextBoxUploadName.Properties.BeginInit();
            this.msQuickButtons.SuspendLayout();
            this.gpgPanelActivity.SuspendLayout();
            this.splitContainerActivity.Panel1.SuspendLayout();
            this.splitContainerActivity.Panel2.SuspendLayout();
            this.splitContainerActivity.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x3c5, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgPanelMyContent.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelMyContent.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelMyContent.BorderThickness = 2;
            this.gpgPanelMyContent.Controls.Add(this.splitContainerMyContent);
            this.gpgPanelMyContent.DrawBorder = false;
            this.gpgPanelMyContent.Location = new Point(12, 110);
            this.gpgPanelMyContent.Name = "gpgPanelMyContent";
            this.gpgPanelMyContent.Size = new Size(0x3e8, 0x25a);
            base.ttDefault.SetSuperTip(this.gpgPanelMyContent, null);
            this.gpgPanelMyContent.TabIndex = 7;
            this.splitContainerMyContent.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerMyContent.BackColor = Color.DarkGray;
            this.splitContainerMyContent.Location = new Point(2, 2);
            this.splitContainerMyContent.Name = "splitContainerMyContent";
            this.splitContainerMyContent.Panel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerMyContent.Panel1.Controls.Add(this.treeViewMyContent);
            base.ttDefault.SetSuperTip(this.splitContainerMyContent.Panel1, null);
            this.splitContainerMyContent.Panel2.AutoScroll = true;
            this.splitContainerMyContent.Panel2.BackColor = Color.Black;
            this.splitContainerMyContent.Panel2.BackgroundImage = (Image) manager.GetObject("splitContainerMyContent.Panel2.BackgroundImage");
            this.splitContainerMyContent.Panel2.Controls.Add(this.gpgLabelMyContent);
            base.ttDefault.SetSuperTip(this.splitContainerMyContent.Panel2, null);
            this.splitContainerMyContent.Size = new Size(0x3e4, 0x256);
            this.splitContainerMyContent.SplitterDistance = 0xd8;
            this.splitContainerMyContent.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainerMyContent, null);
            this.splitContainerMyContent.TabIndex = 2;
            this.treeViewMyContent.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeViewMyContent.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewMyContent.BorderStyle = BorderStyle.None;
            this.treeViewMyContent.ForeColor = Color.White;
            this.treeViewMyContent.HideSelection = false;
            this.treeViewMyContent.ImageIndex = 0;
            this.treeViewMyContent.ImageList = this.imageListContentTypes;
            this.treeViewMyContent.Location = new Point(0, 4);
            this.treeViewMyContent.Name = "treeViewMyContent";
            this.treeViewMyContent.SelectedImageIndex = 0;
            this.treeViewMyContent.Size = new Size(0xd9, 0x252);
            base.ttDefault.SetSuperTip(this.treeViewMyContent, null);
            this.treeViewMyContent.TabIndex = 1;
            this.treeViewMyContent.AfterSelect += new TreeViewEventHandler(this.treeViewMyContent_AfterSelect);
            this.imageListContentTypes.ImageStream = (ImageListStreamer) manager.GetObject("imageListContentTypes.ImageStream");
            this.imageListContentTypes.TransparentColor = Color.Transparent;
            this.imageListContentTypes.Images.SetKeyName(0, "icon_map_sm.png");
            this.imageListContentTypes.Images.SetKeyName(1, "icon_mod_sm.png");
            this.imageListContentTypes.Images.SetKeyName(2, "icon_mag_glass_sm.png");
            this.imageListContentTypes.Images.SetKeyName(3, "icon_saved_sm.png");
            this.imageListContentTypes.Images.SetKeyName(4, "icon_my_upload_sm.png");
            this.imageListContentTypes.Images.SetKeyName(5, "icon_avail_upload_sm.png");
            this.imageListContentTypes.Images.SetKeyName(6, "icon_replay_sm.png");
            this.imageListContentTypes.Images.SetKeyName(7, "icon_missing_sm.png");
            this.imageListContentTypes.Images.SetKeyName(8, "icon_tools_sm.png");
            this.imageListContentTypes.Images.SetKeyName(9, "icon_video_sm.png");
            this.gpgLabelMyContent.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMyContent.AutoStyle = true;
            this.gpgLabelMyContent.BackColor = Color.Transparent;
            this.gpgLabelMyContent.Dock = DockStyle.Fill;
            this.gpgLabelMyContent.Font = new Font("Arial", 9.75f);
            this.gpgLabelMyContent.ForeColor = Color.White;
            this.gpgLabelMyContent.IgnoreMouseWheel = false;
            this.gpgLabelMyContent.IsStyled = false;
            this.gpgLabelMyContent.Location = new Point(0, 0);
            this.gpgLabelMyContent.Name = "gpgLabelMyContent";
            this.gpgLabelMyContent.Size = new Size(0x30a, 0x256);
            base.ttDefault.SetSuperTip(this.gpgLabelMyContent, null);
            this.gpgLabelMyContent.TabIndex = 0;
            this.gpgLabelMyContent.Text = "gpgLabel2";
            this.gpgLabelMyContent.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelMyContent.TextStyle = TextStyles.Default;
            this.tabMyContent.AutoStyle = true;
            this.tabMyContent.BackColor = Color.Black;
            this.tabMyContent.ButtonState = 0;
            this.tabMyContent.DialogResult = DialogResult.OK;
            this.tabMyContent.DisabledForecolor = Color.Gray;
            this.tabMyContent.DrawColor = Color.White;
            this.tabMyContent.DrawEdges = true;
            this.tabMyContent.FocusColor = Color.Yellow;
            this.tabMyContent.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.tabMyContent.ForeColor = Color.White;
            this.tabMyContent.HorizontalScalingMode = ScalingModes.Tile;
            this.tabMyContent.IsStyled = true;
            this.tabMyContent.Location = new Point(0x84, 0x53);
            this.tabMyContent.Name = "tabMyContent";
            this.tabMyContent.Size = new Size(0x7a, 0x1c);
            this.tabMyContent.SkinBasePath = @"Dialog\ContentManager\TabLarge";
            base.ttDefault.SetSuperTip(this.tabMyContent, null);
            this.tabMyContent.TabIndex = 8;
            this.tabMyContent.TabStop = true;
            this.tabMyContent.Text = "<LOC>My Content";
            this.tabMyContent.TextAlign = ContentAlignment.MiddleLeft;
            this.tabMyContent.TextPadding = new Padding(6, 0, 0, 0);
            this.tabDownload.AutoStyle = true;
            this.tabDownload.BackColor = Color.Black;
            this.tabDownload.ButtonState = 0;
            this.tabDownload.DialogResult = DialogResult.OK;
            this.tabDownload.DisabledForecolor = Color.Gray;
            this.tabDownload.DrawColor = Color.White;
            this.tabDownload.DrawEdges = true;
            this.tabDownload.FocusColor = Color.Yellow;
            this.tabDownload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.tabDownload.ForeColor = Color.White;
            this.tabDownload.HorizontalScalingMode = ScalingModes.Tile;
            this.tabDownload.IsStyled = true;
            this.tabDownload.Location = new Point(10, 0x53);
            this.tabDownload.Name = "tabDownload";
            this.tabDownload.Size = new Size(0x7a, 0x1c);
            this.tabDownload.SkinBasePath = @"Dialog\ContentManager\TabLarge";
            base.ttDefault.SetSuperTip(this.tabDownload, null);
            this.tabDownload.TabIndex = 9;
            this.tabDownload.TabStop = true;
            this.tabDownload.Text = "<LOC>Download";
            this.tabDownload.TextAlign = ContentAlignment.MiddleLeft;
            this.tabDownload.TextPadding = new Padding(6, 0, 0, 0);
            this.tabUpload.AutoStyle = true;
            this.tabUpload.BackColor = Color.Black;
            this.tabUpload.ButtonState = 0;
            this.tabUpload.DialogResult = DialogResult.OK;
            this.tabUpload.DisabledForecolor = Color.Gray;
            this.tabUpload.DrawColor = Color.White;
            this.tabUpload.DrawEdges = true;
            this.tabUpload.FocusColor = Color.Yellow;
            this.tabUpload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.tabUpload.ForeColor = Color.White;
            this.tabUpload.HorizontalScalingMode = ScalingModes.Tile;
            this.tabUpload.IsStyled = true;
            this.tabUpload.Location = new Point(0xfe, 0x53);
            this.tabUpload.Name = "tabUpload";
            this.tabUpload.Size = new Size(0x7a, 0x1c);
            this.tabUpload.SkinBasePath = @"Dialog\ContentManager\TabLarge";
            base.ttDefault.SetSuperTip(this.tabUpload, null);
            this.tabUpload.TabIndex = 10;
            this.tabUpload.TabStop = true;
            this.tabUpload.Text = "<LOC>Upload";
            this.tabUpload.TextAlign = ContentAlignment.MiddleLeft;
            this.tabUpload.TextPadding = new Padding(6, 0, 0, 0);
            this.gpgPanelDownload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelDownload.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelDownload.BorderThickness = 2;
            this.gpgPanelDownload.Controls.Add(this.splitContainerDownload);
            this.gpgPanelDownload.DrawBorder = false;
            this.gpgPanelDownload.Location = new Point(12, 110);
            this.gpgPanelDownload.Name = "gpgPanelDownload";
            this.gpgPanelDownload.Size = new Size(0x3e8, 0x25a);
            base.ttDefault.SetSuperTip(this.gpgPanelDownload, null);
            this.gpgPanelDownload.TabIndex = 8;
            this.splitContainerDownload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerDownload.Location = new Point(2, 2);
            this.splitContainerDownload.Name = "splitContainerDownload";
            this.splitContainerDownload.Panel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerDownload.Panel1.Controls.Add(this.treeViewDownloadType);
            this.splitContainerDownload.Panel1.Controls.Add(this.treeViewSavedSearches);
            this.splitContainerDownload.Panel1.Controls.Add(this.skinLabel1);
            this.splitContainerDownload.Panel1.Controls.Add(this.skinLabel2);
            base.ttDefault.SetSuperTip(this.splitContainerDownload.Panel1, null);
            this.splitContainerDownload.Panel2.BackgroundImage = (Image) manager.GetObject("splitContainerDownload.Panel2.BackgroundImage");
            this.splitContainerDownload.Panel2.Controls.Add(this.gpgGroupBoxResults);
            this.splitContainerDownload.Panel2.Controls.Add(this.gpgGroupBoxCriteria);
            base.ttDefault.SetSuperTip(this.splitContainerDownload.Panel2, null);
            this.splitContainerDownload.Size = new Size(0x3e4, 0x256);
            this.splitContainerDownload.SplitterDistance = 0xe1;
            this.splitContainerDownload.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainerDownload, null);
            this.splitContainerDownload.TabIndex = 15;
            this.treeViewDownloadType.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.treeViewDownloadType.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewDownloadType.BorderStyle = BorderStyle.None;
            this.treeViewDownloadType.HideSelection = false;
            this.treeViewDownloadType.ImageIndex = 0;
            this.treeViewDownloadType.ImageList = this.imageListContentTypes;
            this.treeViewDownloadType.Location = new Point(0, 0x1a);
            this.treeViewDownloadType.Name = "treeViewDownloadType";
            this.treeViewDownloadType.SelectedImageIndex = 0;
            this.treeViewDownloadType.Size = new Size(0xe1, 0x80);
            base.ttDefault.SetSuperTip(this.treeViewDownloadType, null);
            this.treeViewDownloadType.TabIndex = 0;
            this.treeViewDownloadType.AfterSelect += new TreeViewEventHandler(this.treeViewDownloadType_AfterSelect);
            this.treeViewSavedSearches.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeViewSavedSearches.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewSavedSearches.BorderStyle = BorderStyle.None;
            this.treeViewSavedSearches.HideSelection = false;
            this.treeViewSavedSearches.ImageIndex = 0;
            this.treeViewSavedSearches.ImageList = this.imageListContentTypes;
            this.treeViewSavedSearches.Location = new Point(0, 180);
            this.treeViewSavedSearches.Name = "treeViewSavedSearches";
            this.treeViewSavedSearches.SelectedImageIndex = 0;
            this.treeViewSavedSearches.Size = new Size(0xe1, 0x1a2);
            base.ttDefault.SetSuperTip(this.treeViewSavedSearches, null);
            this.treeViewSavedSearches.TabIndex = 1;
            this.treeViewSavedSearches.AfterSelect += new TreeViewEventHandler(this.treeViewSavedSearches_AfterSelect);
            this.skinLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(0, 0x9a);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0xe1, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 15;
            this.skinLabel1.Text = "<LOC>Saved Searches";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(0);
            this.skinLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Transparent;
            this.skinLabel2.DrawEdges = true;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(0, 0);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0xe1, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 0x10;
            this.skinLabel2.Text = "<LOC>Search For";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(0);
            this.gpgGroupBoxResults.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGroupBoxResults.AutoStyle = false;
            this.gpgGroupBoxResults.BackColor = Color.Black;
            this.gpgGroupBoxResults.Controls.Add(this.skinButtonSearchNext);
            this.gpgGroupBoxResults.Controls.Add(this.skinLabelSearchPage);
            this.gpgGroupBoxResults.Controls.Add(this.dataGridSearchResults);
            this.gpgGroupBoxResults.Controls.Add(this.skinButtonSearchStart);
            this.gpgGroupBoxResults.Controls.Add(this.skinButtonSearchPrevious);
            this.gpgGroupBoxResults.CutCorner = false;
            this.gpgGroupBoxResults.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgGroupBoxResults.HeaderImage = GroupPanelImages.blue_gradient;
            this.gpgGroupBoxResults.IsStyled = true;
            this.gpgGroupBoxResults.Location = new Point(4, 0x7d);
            this.gpgGroupBoxResults.Margin = new Padding(4, 3, 4, 3);
            this.gpgGroupBoxResults.Name = "gpgGroupBoxResults";
            this.gpgGroupBoxResults.Size = new Size(0x2f6, 0x1d4);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxResults, null);
            this.gpgGroupBoxResults.TabIndex = 0x10;
            this.gpgGroupBoxResults.Text = "<LOC>Search Results";
            this.gpgGroupBoxResults.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgGroupBoxResults.TextPadding = new Padding(8, 0, 0, 0);
            this.skinButtonSearchNext.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonSearchNext.AutoStyle = true;
            this.skinButtonSearchNext.BackColor = Color.Black;
            this.skinButtonSearchNext.ButtonState = 0;
            this.skinButtonSearchNext.DialogResult = DialogResult.OK;
            this.skinButtonSearchNext.DisabledForecolor = Color.Gray;
            this.skinButtonSearchNext.DrawColor = Color.White;
            this.skinButtonSearchNext.DrawEdges = false;
            this.skinButtonSearchNext.FocusColor = Color.Yellow;
            this.skinButtonSearchNext.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSearchNext.ForeColor = Color.White;
            this.skinButtonSearchNext.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSearchNext.IsStyled = true;
            this.skinButtonSearchNext.Location = new Point(0x2ca, 0x1ba);
            this.skinButtonSearchNext.Name = "skinButtonSearchNext";
            this.skinButtonSearchNext.Size = new Size(40, 0x16);
            this.skinButtonSearchNext.SkinBasePath = @"Controls\Button\Next_End";
            base.ttDefault.SetSuperTip(this.skinButtonSearchNext, null);
            this.skinButtonSearchNext.TabIndex = 7;
            this.skinButtonSearchNext.TabStop = true;
            this.skinButtonSearchNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearchNext.TextPadding = new Padding(0);
            this.skinButtonSearchNext.Click += new EventHandler(this.skinButtonSearchNext_Click);
            this.skinLabelSearchPage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinLabelSearchPage.AutoStyle = false;
            this.skinLabelSearchPage.BackColor = Color.Transparent;
            this.skinLabelSearchPage.DrawEdges = true;
            this.skinLabelSearchPage.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelSearchPage.ForeColor = Color.White;
            this.skinLabelSearchPage.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelSearchPage.IsStyled = false;
            this.skinLabelSearchPage.Location = new Point(0x54, 0x1ba);
            this.skinLabelSearchPage.Name = "skinLabelSearchPage";
            this.skinLabelSearchPage.Size = new Size(630, 0x16);
            this.skinLabelSearchPage.SkinBasePath = @"Controls\Background Label\BlackBar";
            base.ttDefault.SetSuperTip(this.skinLabelSearchPage, null);
            this.skinLabelSearchPage.TabIndex = 0x19;
            this.skinLabelSearchPage.Text = "<LOC>No Results";
            this.skinLabelSearchPage.TextAlign = ContentAlignment.MiddleCenter;
            this.skinLabelSearchPage.TextPadding = new Padding(0);
            this.dataGridSearchResults.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dataGridSearchResults.CustomizeStyle = false;
            this.dataGridSearchResults.EmbeddedNavigator.Name = "";
            this.dataGridSearchResults.Location = new Point(4, 0x16);
            this.dataGridSearchResults.MainView = this.gvResults;
            this.dataGridSearchResults.Name = "dataGridSearchResults";
            this.dataGridSearchResults.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemRatingStars, this.repositoryItemChatLink, this.repositoryItemPictureEdit1, this.repositoryItemVersionLink });
            this.dataGridSearchResults.ShowOnlyPredefinedDetails = true;
            this.dataGridSearchResults.Size = new Size(750, 0x1a0);
            this.dataGridSearchResults.TabIndex = 0x18;
            this.dataGridSearchResults.ViewCollection.AddRange(new BaseView[] { this.gvResults });
            this.dataGridSearchResults.DoubleClick += new EventHandler(this.dataGridSearchResults_DoubleClick);
            this.gvResults.Appearance.EvenRow.BackColor = Color.Black;
            this.gvResults.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvResults.Appearance.OddRow.BackColor = Color.Black;
            this.gvResults.Appearance.OddRow.Options.UseBackColor = true;
            this.gvResults.Appearance.Preview.ForeColor = Color.Silver;
            this.gvResults.Appearance.Preview.Options.UseForeColor = true;
            this.gvResults.Appearance.SelectedRow.BackColor = Color.FromArgb(0x33, 0x33, 0x65);
            this.gvResults.Appearance.SelectedRow.BackColor2 = Color.Black;
            this.gvResults.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvResults.Columns.AddRange(new GridColumn[] { this.gcDownload, this.gcName, this.gcOwner, this.gcVersion, this.gcDate, this.gcDownloads, this.gcRating });
            this.gvResults.GridControl = this.dataGridSearchResults;
            this.gvResults.GroupPanelText = "<LOC>Drag a column here to group by it.";
            this.gvResults.Name = "gvResults";
            this.gvResults.OptionsBehavior.Editable = false;
            this.gvResults.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvResults.OptionsView.AutoCalcPreviewLineCount = true;
            this.gvResults.OptionsView.RowAutoHeight = true;
            this.gvResults.OptionsView.ShowPreview = true;
            this.gvResults.PreviewFieldName = "Description";
            this.gvResults.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvResults_CustomDrawCell);
            this.gvResults.MouseDown += new MouseEventHandler(this.gvResults_MouseDown);
            this.gvResults.MouseUp += new MouseEventHandler(this.gvResults_MouseUp);
            this.gvResults.MouseMove += new MouseEventHandler(this.gvResults_MouseMove);
            this.gvResults.CalcRowHeight += new RowHeightEventHandler(this.gvResults_CalcRowHeight);
            this.gcDownload.AppearanceHeader.Options.UseTextOptions = true;
            this.gcDownload.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDownload.Caption = "<LOC>Download";
            this.gcDownload.FieldName = "Download";
            this.gcDownload.Name = "gcDownload";
            this.gcDownload.ToolTip = "<LOC>Download";
            this.gcDownload.Visible = true;
            this.gcDownload.VisibleIndex = 0;
            this.gcName.AppearanceCell.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.gcName.AppearanceCell.Options.UseFont = true;
            this.gcName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcName.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcName.Caption = "<LOC>Name";
            this.gcName.FieldName = "Name";
            this.gcName.Name = "gcName";
            this.gcName.Visible = true;
            this.gcName.VisibleIndex = 1;
            this.gcOwner.AppearanceHeader.Options.UseTextOptions = true;
            this.gcOwner.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcOwner.Caption = "<LOC>Creator";
            this.gcOwner.FieldName = "OwnerName";
            this.gcOwner.Name = "gcOwner";
            this.gcOwner.Visible = true;
            this.gcOwner.VisibleIndex = 2;
            this.gcVersion.AppearanceCell.Font = new Font("Tahoma", 8.25f, FontStyle.Underline | FontStyle.Bold);
            this.gcVersion.AppearanceCell.ForeColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.gcVersion.AppearanceCell.Options.UseFont = true;
            this.gcVersion.AppearanceCell.Options.UseForeColor = true;
            this.gcVersion.AppearanceCell.Options.UseTextOptions = true;
            this.gcVersion.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcVersion.AppearanceHeader.Options.UseTextOptions = true;
            this.gcVersion.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcVersion.Caption = "<LOC>Version";
            this.gcVersion.FieldName = "Version";
            this.gcVersion.Name = "gcVersion";
            this.gcVersion.Visible = true;
            this.gcVersion.VisibleIndex = 3;
            this.gcDate.AppearanceHeader.Options.UseTextOptions = true;
            this.gcDate.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDate.Caption = "<LOC>Date";
            this.gcDate.FieldName = "VersionDate";
            this.gcDate.Name = "gcDate";
            this.gcDate.Visible = true;
            this.gcDate.VisibleIndex = 4;
            this.gcDownloads.AppearanceCell.Options.UseTextOptions = true;
            this.gcDownloads.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDownloads.AppearanceHeader.Options.UseTextOptions = true;
            this.gcDownloads.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcDownloads.Caption = "<LOC>D/L";
            this.gcDownloads.FieldName = "Downloads";
            this.gcDownloads.Name = "gcDownloads";
            this.gcDownloads.Visible = true;
            this.gcDownloads.VisibleIndex = 5;
            this.gcRating.AppearanceHeader.Options.UseTextOptions = true;
            this.gcRating.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Near;
            this.gcRating.Caption = "<LOC>Rating";
            this.gcRating.ColumnEdit = this.repositoryItemPictureEdit1;
            this.gcRating.FieldName = "RatingImageSmall";
            this.gcRating.Name = "gcRating";
            this.gcRating.Visible = true;
            this.gcRating.VisibleIndex = 6;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemRatingStars.Name = "repositoryItemRatingStars";
            this.repositoryItemChatLink.Appearance.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.repositoryItemChatLink.Appearance.ForeColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.repositoryItemChatLink.Appearance.Options.UseFont = true;
            this.repositoryItemChatLink.Appearance.Options.UseForeColor = true;
            this.repositoryItemChatLink.Appearance.Options.UseTextOptions = true;
            this.repositoryItemChatLink.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
            this.repositoryItemChatLink.Appearance.TextOptions.VAlignment = VertAlignment.Center;
            this.repositoryItemChatLink.AppearanceFocused.Options.UseTextOptions = true;
            this.repositoryItemChatLink.AppearanceFocused.TextOptions.HAlignment = HorzAlignment.Center;
            this.repositoryItemChatLink.AppearanceFocused.TextOptions.VAlignment = VertAlignment.Center;
            this.repositoryItemChatLink.AutoHeight = false;
            this.repositoryItemChatLink.LinkColor = Color.FromArgb(0xc0, 0xc0, 0xff);
            this.repositoryItemChatLink.Name = "repositoryItemChatLink";
            this.repositoryItemChatLink.SingleClick = true;
            this.repositoryItemVersionLink.AutoHeight = false;
            this.repositoryItemVersionLink.Name = "repositoryItemVersionLink";
            this.skinButtonSearchStart.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonSearchStart.AutoStyle = true;
            this.skinButtonSearchStart.BackColor = Color.Black;
            this.skinButtonSearchStart.ButtonState = 0;
            this.skinButtonSearchStart.DialogResult = DialogResult.OK;
            this.skinButtonSearchStart.DisabledForecolor = Color.Gray;
            this.skinButtonSearchStart.DrawColor = Color.White;
            this.skinButtonSearchStart.DrawEdges = false;
            this.skinButtonSearchStart.FocusColor = Color.Yellow;
            this.skinButtonSearchStart.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSearchStart.ForeColor = Color.White;
            this.skinButtonSearchStart.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSearchStart.IsStyled = true;
            this.skinButtonSearchStart.Location = new Point(4, 0x1ba);
            this.skinButtonSearchStart.Name = "skinButtonSearchStart";
            this.skinButtonSearchStart.Size = new Size(40, 0x16);
            this.skinButtonSearchStart.SkinBasePath = @"Controls\Button\First";
            base.ttDefault.SetSuperTip(this.skinButtonSearchStart, null);
            this.skinButtonSearchStart.TabIndex = 5;
            this.skinButtonSearchStart.TabStop = true;
            this.skinButtonSearchStart.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearchStart.TextPadding = new Padding(0);
            this.skinButtonSearchStart.Click += new EventHandler(this.skinButtonSearchStart_Click);
            this.skinButtonSearchPrevious.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonSearchPrevious.AutoStyle = true;
            this.skinButtonSearchPrevious.BackColor = Color.Black;
            this.skinButtonSearchPrevious.ButtonState = 0;
            this.skinButtonSearchPrevious.DialogResult = DialogResult.OK;
            this.skinButtonSearchPrevious.DisabledForecolor = Color.Gray;
            this.skinButtonSearchPrevious.DrawColor = Color.White;
            this.skinButtonSearchPrevious.DrawEdges = false;
            this.skinButtonSearchPrevious.FocusColor = Color.Yellow;
            this.skinButtonSearchPrevious.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSearchPrevious.ForeColor = Color.White;
            this.skinButtonSearchPrevious.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSearchPrevious.IsStyled = true;
            this.skinButtonSearchPrevious.Location = new Point(0x2c, 0x1ba);
            this.skinButtonSearchPrevious.Name = "skinButtonSearchPrevious";
            this.skinButtonSearchPrevious.Size = new Size(40, 0x16);
            this.skinButtonSearchPrevious.SkinBasePath = @"Controls\Button\Previous";
            base.ttDefault.SetSuperTip(this.skinButtonSearchPrevious, null);
            this.skinButtonSearchPrevious.TabIndex = 6;
            this.skinButtonSearchPrevious.TabStop = true;
            this.skinButtonSearchPrevious.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearchPrevious.TextPadding = new Padding(0);
            this.skinButtonSearchPrevious.Click += new EventHandler(this.skinButtonSearchPrevious_Click);
            this.gpgGroupBoxCriteria.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgGroupBoxCriteria.AutoStyle = false;
            this.gpgGroupBoxCriteria.BackColor = Color.Black;
            this.gpgGroupBoxCriteria.Controls.Add(this.skinButtonSaveSearch);
            this.gpgGroupBoxCriteria.Controls.Add(this.skinButtonRunSearch);
            this.gpgGroupBoxCriteria.Controls.Add(this.gpgPanelSearchCriteria);
            this.gpgGroupBoxCriteria.CutCorner = false;
            this.gpgGroupBoxCriteria.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgGroupBoxCriteria.HeaderImage = GroupPanelImages.blue_gradient;
            this.gpgGroupBoxCriteria.IsStyled = true;
            this.gpgGroupBoxCriteria.Location = new Point(4, 5);
            this.gpgGroupBoxCriteria.Margin = new Padding(4, 3, 4, 3);
            this.gpgGroupBoxCriteria.Name = "gpgGroupBoxCriteria";
            this.gpgGroupBoxCriteria.Size = new Size(0x2f6, 0x72);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxCriteria, null);
            this.gpgGroupBoxCriteria.TabIndex = 15;
            this.gpgGroupBoxCriteria.Text = "<LOC>Search Criteria";
            this.gpgGroupBoxCriteria.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgGroupBoxCriteria.TextPadding = new Padding(8, 0, 0, 0);
            this.skinButtonSaveSearch.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonSaveSearch.AutoStyle = true;
            this.skinButtonSaveSearch.BackColor = Color.Black;
            this.skinButtonSaveSearch.ButtonState = 0;
            this.skinButtonSaveSearch.DialogResult = DialogResult.OK;
            this.skinButtonSaveSearch.DisabledForecolor = Color.Gray;
            this.skinButtonSaveSearch.DrawColor = Color.White;
            this.skinButtonSaveSearch.DrawEdges = true;
            this.skinButtonSaveSearch.FocusColor = Color.Yellow;
            this.skinButtonSaveSearch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSaveSearch.ForeColor = Color.White;
            this.skinButtonSaveSearch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSaveSearch.IsStyled = true;
            this.skinButtonSaveSearch.Location = new Point(0x1ef, 0x51);
            this.skinButtonSaveSearch.Name = "skinButtonSaveSearch";
            this.skinButtonSaveSearch.Size = new Size(0x7d, 0x1a);
            this.skinButtonSaveSearch.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSaveSearch, null);
            this.skinButtonSaveSearch.TabIndex = 0x1f;
            this.skinButtonSaveSearch.TabStop = true;
            this.skinButtonSaveSearch.Text = "<LOC>Save Search";
            this.skinButtonSaveSearch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSaveSearch.TextPadding = new Padding(0);
            this.skinButtonSaveSearch.Visible = false;
            this.skinButtonSaveSearch.Click += new EventHandler(this.skinButtonSaveSearch_Click);
            this.skinButtonRunSearch.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonRunSearch.AutoStyle = true;
            this.skinButtonRunSearch.BackColor = Color.Black;
            this.skinButtonRunSearch.ButtonState = 0;
            this.skinButtonRunSearch.DialogResult = DialogResult.OK;
            this.skinButtonRunSearch.DisabledForecolor = Color.Gray;
            this.skinButtonRunSearch.DrawColor = Color.White;
            this.skinButtonRunSearch.DrawEdges = true;
            this.skinButtonRunSearch.FocusColor = Color.Yellow;
            this.skinButtonRunSearch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRunSearch.ForeColor = Color.White;
            this.skinButtonRunSearch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRunSearch.IsStyled = true;
            this.skinButtonRunSearch.Location = new Point(0x272, 0x51);
            this.skinButtonRunSearch.Name = "skinButtonRunSearch";
            this.skinButtonRunSearch.Size = new Size(0x7d, 0x1a);
            this.skinButtonRunSearch.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRunSearch, null);
            this.skinButtonRunSearch.TabIndex = 30;
            this.skinButtonRunSearch.TabStop = true;
            this.skinButtonRunSearch.Text = "<LOC>Run Search";
            this.skinButtonRunSearch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRunSearch.TextPadding = new Padding(0);
            this.skinButtonRunSearch.Click += new EventHandler(this.skinButtonRunSearch_Click);
            this.gpgPanelSearchCriteria.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelSearchCriteria.AutoScroll = true;
            this.gpgPanelSearchCriteria.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelSearchCriteria.BorderThickness = 2;
            this.gpgPanelSearchCriteria.Controls.Add(this.skinButtonSearchType);
            this.gpgPanelSearchCriteria.Controls.Add(this.gpgLabelSearchType);
            this.gpgPanelSearchCriteria.Controls.Add(this.gpgTextBoxSearchCreator);
            this.gpgPanelSearchCriteria.Controls.Add(this.gpgLabel16);
            this.gpgPanelSearchCriteria.Controls.Add(this.gpgTextBoxSearchName);
            this.gpgPanelSearchCriteria.Controls.Add(this.gpgLabel1);
            this.gpgPanelSearchCriteria.Controls.Add(this.gpgTextBoxSearchKeywords);
            this.gpgPanelSearchCriteria.Controls.Add(this.gpgLabel21);
            this.gpgPanelSearchCriteria.DrawBorder = false;
            this.gpgPanelSearchCriteria.Location = new Point(2, 0x17);
            this.gpgPanelSearchCriteria.Name = "gpgPanelSearchCriteria";
            this.gpgPanelSearchCriteria.Size = new Size(0x2ed, 0x34);
            base.ttDefault.SetSuperTip(this.gpgPanelSearchCriteria, null);
            this.gpgPanelSearchCriteria.TabIndex = 0x1d;
            this.skinButtonSearchType.AutoStyle = true;
            this.skinButtonSearchType.BackColor = Color.Transparent;
            this.skinButtonSearchType.ButtonState = 0;
            this.skinButtonSearchType.DialogResult = DialogResult.OK;
            this.skinButtonSearchType.DisabledForecolor = Color.Gray;
            this.skinButtonSearchType.DrawColor = Color.White;
            this.skinButtonSearchType.DrawEdges = false;
            this.skinButtonSearchType.FocusColor = Color.Yellow;
            this.skinButtonSearchType.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSearchType.ForeColor = Color.White;
            this.skinButtonSearchType.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonSearchType.IsStyled = true;
            this.skinButtonSearchType.Location = new Point(0x295, 0x17);
            this.skinButtonSearchType.Name = "skinButtonSearchType";
            this.skinButtonSearchType.Size = new Size(0x18, 0x12);
            this.skinButtonSearchType.SkinBasePath = @"Dialog\ContentManager\BtnAdvancedSearch";
            base.ttDefault.SetSuperTip(this.skinButtonSearchType, null);
            this.skinButtonSearchType.TabIndex = 0x25;
            this.skinButtonSearchType.TabStop = true;
            this.skinButtonSearchType.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearchType.TextPadding = new Padding(0);
            this.skinButtonSearchType.Click += new EventHandler(this.skinButtonSearchType_Click);
            this.gpgLabelSearchType.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelSearchType.AutoSize = true;
            this.gpgLabelSearchType.AutoStyle = true;
            this.gpgLabelSearchType.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelSearchType.ForeColor = Color.DimGray;
            this.gpgLabelSearchType.IgnoreMouseWheel = false;
            this.gpgLabelSearchType.IsStyled = false;
            this.gpgLabelSearchType.Location = new Point(530, 0x1a);
            this.gpgLabelSearchType.Name = "gpgLabelSearchType";
            this.gpgLabelSearchType.Size = new Size(0x7e, 12);
            base.ttDefault.SetSuperTip(this.gpgLabelSearchType, null);
            this.gpgLabelSearchType.TabIndex = 0x23;
            this.gpgLabelSearchType.Text = "<LOC>Advanced Search";
            this.gpgLabelSearchType.TextStyle = TextStyles.Custom;
            this.gpgTextBoxSearchCreator.Location = new Point(0xb9, 0x16);
            this.gpgTextBoxSearchCreator.Name = "gpgTextBoxSearchCreator";
            this.gpgTextBoxSearchCreator.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxSearchCreator.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxSearchCreator.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxSearchCreator.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxSearchCreator.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxSearchCreator.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxSearchCreator.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxSearchCreator.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxSearchCreator.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxSearchCreator.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxSearchCreator.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxSearchCreator.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxSearchCreator.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxSearchCreator.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxSearchCreator.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxSearchCreator.Properties.MaxLength = 0x16;
            this.gpgTextBoxSearchCreator.Size = new Size(160, 20);
            this.gpgTextBoxSearchCreator.TabIndex = 0x22;
            this.gpgTextBoxSearchCreator.EditValueChanged += new EventHandler(this.SearchCriteriaChanged);
            this.gpgTextBoxSearchCreator.KeyDown += new KeyEventHandler(this.SearchCriteriaTextBox_KeyDown);
            this.gpgLabel16.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel16.AutoSize = true;
            this.gpgLabel16.AutoStyle = true;
            this.gpgLabel16.Font = new Font("Arial", 9.75f);
            this.gpgLabel16.ForeColor = Color.White;
            this.gpgLabel16.IgnoreMouseWheel = false;
            this.gpgLabel16.IsStyled = false;
            this.gpgLabel16.Location = new Point(0xb6, 3);
            this.gpgLabel16.Name = "gpgLabel16";
            this.gpgLabel16.Size = new Size(0x73, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel16, null);
            this.gpgLabel16.TabIndex = 0x21;
            this.gpgLabel16.Text = "<LOC>Created By";
            this.gpgLabel16.TextStyle = TextStyles.Bold;
            this.gpgTextBoxSearchName.EditValue = "";
            this.gpgTextBoxSearchName.Location = new Point(8, 0x16);
            this.gpgTextBoxSearchName.Name = "gpgTextBoxSearchName";
            this.gpgTextBoxSearchName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxSearchName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxSearchName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxSearchName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxSearchName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxSearchName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxSearchName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxSearchName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxSearchName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxSearchName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxSearchName.Properties.MaxLength = 0x100;
            this.gpgTextBoxSearchName.Size = new Size(160, 20);
            this.gpgTextBoxSearchName.TabIndex = 0x20;
            this.gpgTextBoxSearchName.EditValueChanged += new EventHandler(this.SearchCriteriaChanged);
            this.gpgTextBoxSearchName.KeyDown += new KeyEventHandler(this.SearchCriteriaTextBox_KeyDown);
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(5, 3);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x54, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 0x1f;
            this.gpgLabel1.Text = "<LOC>Name";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.gpgTextBoxSearchKeywords.Location = new Point(0x16c, 0x16);
            this.gpgTextBoxSearchKeywords.Name = "gpgTextBoxSearchKeywords";
            this.gpgTextBoxSearchKeywords.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxSearchKeywords.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxSearchKeywords.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxSearchKeywords.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxSearchKeywords.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxSearchKeywords.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxSearchKeywords.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxSearchKeywords.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxSearchKeywords.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxSearchKeywords.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxSearchKeywords.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxSearchKeywords.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxSearchKeywords.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxSearchKeywords.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxSearchKeywords.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxSearchKeywords.Properties.MaxLength = 0x100;
            this.gpgTextBoxSearchKeywords.Size = new Size(160, 20);
            this.gpgTextBoxSearchKeywords.TabIndex = 30;
            this.gpgTextBoxSearchKeywords.EditValueChanged += new EventHandler(this.SearchCriteriaChanged);
            this.gpgTextBoxSearchKeywords.KeyDown += new KeyEventHandler(this.SearchCriteriaTextBox_KeyDown);
            this.gpgLabel21.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel21.AutoSize = true;
            this.gpgLabel21.AutoStyle = true;
            this.gpgLabel21.Font = new Font("Arial", 9.75f);
            this.gpgLabel21.ForeColor = Color.White;
            this.gpgLabel21.IgnoreMouseWheel = false;
            this.gpgLabel21.IsStyled = false;
            this.gpgLabel21.Location = new Point(0x169, 3);
            this.gpgLabel21.Name = "gpgLabel21";
            this.gpgLabel21.Size = new Size(0x6b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel21, null);
            this.gpgLabel21.TabIndex = 0x1d;
            this.gpgLabel21.Text = "<LOC>Keywords";
            this.gpgLabel21.TextStyle = TextStyles.Bold;
            this.gpgPanelUpload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelUpload.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelUpload.BorderThickness = 2;
            this.gpgPanelUpload.Controls.Add(this.splitContainerUpload);
            this.gpgPanelUpload.DrawBorder = false;
            this.gpgPanelUpload.Location = new Point(12, 110);
            this.gpgPanelUpload.Name = "gpgPanelUpload";
            this.gpgPanelUpload.Size = new Size(0x3e8, 0x25a);
            base.ttDefault.SetSuperTip(this.gpgPanelUpload, null);
            this.gpgPanelUpload.TabIndex = 8;
            this.splitContainerUpload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerUpload.Location = new Point(2, 2);
            this.splitContainerUpload.Name = "splitContainerUpload";
            this.splitContainerUpload.Panel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerUpload.Panel1.Controls.Add(this.gpgLabelUploadLocations);
            this.splitContainerUpload.Panel1.Controls.Add(this.skinLabel4);
            this.splitContainerUpload.Panel1.Controls.Add(this.skinLabel3);
            this.splitContainerUpload.Panel1.Controls.Add(this.pictureBoxRefreshUploads);
            this.splitContainerUpload.Panel1.Controls.Add(this.treeViewUpload);
            base.ttDefault.SetSuperTip(this.splitContainerUpload.Panel1, null);
            this.splitContainerUpload.Panel2.AutoScroll = true;
            this.splitContainerUpload.Panel2.BackgroundImage = (Image) manager.GetObject("splitContainerUpload.Panel2.BackgroundImage");
            this.splitContainerUpload.Panel2.Controls.Add(this.gpgGroupBoxUploadVersion);
            this.splitContainerUpload.Panel2.Controls.Add(this.gpgGroupBoxUploadGeneral);
            this.splitContainerUpload.Panel2.Controls.Add(this.skinButtonDeleteUploadVersion);
            this.splitContainerUpload.Panel2.Controls.Add(this.skinButtonDeleteUploadAll);
            this.splitContainerUpload.Panel2.Controls.Add(this.skinButtonUpload);
            this.splitContainerUpload.Panel2.Controls.Add(this.gpgLabelNoUploadSelected);
            base.ttDefault.SetSuperTip(this.splitContainerUpload.Panel2, null);
            this.splitContainerUpload.Size = new Size(0x3e4, 0x256);
            this.splitContainerUpload.SplitterDistance = 0x120;
            this.splitContainerUpload.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainerUpload, null);
            this.splitContainerUpload.TabIndex = 2;
            this.gpgLabelUploadLocations.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelUploadLocations.AutoSize = true;
            this.gpgLabelUploadLocations.AutoStyle = true;
            this.gpgLabelUploadLocations.Cursor = Cursors.Hand;
            this.gpgLabelUploadLocations.Font = new Font("Verdana", 8.25f, FontStyle.Underline | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelUploadLocations.ForeColor = Color.White;
            this.gpgLabelUploadLocations.IgnoreMouseWheel = false;
            this.gpgLabelUploadLocations.IsStyled = false;
            this.gpgLabelUploadLocations.Location = new Point(1, 0x1d);
            this.gpgLabelUploadLocations.Name = "gpgLabelUploadLocations";
            this.gpgLabelUploadLocations.Size = new Size(0xd8, 13);
            base.ttDefault.SetSuperTip(this.gpgLabelUploadLocations, null);
            this.gpgLabelUploadLocations.TabIndex = 3;
            this.gpgLabelUploadLocations.Text = "<LOC>Select Upload Locations...";
            this.gpgLabelUploadLocations.TextStyle = TextStyles.Custom;
            this.gpgLabelUploadLocations.Click += new EventHandler(this.SelectUploadPath_Click);
            this.skinLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel4.AutoStyle = false;
            this.skinLabel4.BackColor = Color.Transparent;
            this.skinLabel4.DrawEdges = true;
            this.skinLabel4.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel4.ForeColor = Color.White;
            this.skinLabel4.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel4.IsStyled = false;
            this.skinLabel4.Location = new Point(0, 0x36);
            this.skinLabel4.Name = "skinLabel4";
            this.skinLabel4.Size = new Size(0x120, 20);
            this.skinLabel4.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel4, null);
            this.skinLabel4.TabIndex = 2;
            this.skinLabel4.Text = "<LOC>My Files";
            this.skinLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel4.TextPadding = new Padding(0);
            this.skinLabel3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel3.AutoStyle = false;
            this.skinLabel3.BackColor = Color.Transparent;
            this.skinLabel3.DrawEdges = true;
            this.skinLabel3.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel3.ForeColor = Color.White;
            this.skinLabel3.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel3.IsStyled = false;
            this.skinLabel3.Location = new Point(0, 0);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new Size(0x120, 20);
            this.skinLabel3.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel3, null);
            this.skinLabel3.TabIndex = 1;
            this.skinLabel3.Text = "<LOC>Uploadable Content";
            this.skinLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel3.TextPadding = new Padding(0);
            this.pictureBoxRefreshUploads.Cursor = Cursors.Hand;
            this.pictureBoxRefreshUploads.Image = (Image) manager.GetObject("pictureBoxRefreshUploads.Image");
            this.pictureBoxRefreshUploads.InitialImage = (Image) manager.GetObject("pictureBoxRefreshUploads.InitialImage");
            this.pictureBoxRefreshUploads.Location = new Point(0xe1, 0x1b);
            this.pictureBoxRefreshUploads.Name = "pictureBoxRefreshUploads";
            this.pictureBoxRefreshUploads.Size = new Size(0x10, 0x10);
            this.pictureBoxRefreshUploads.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pictureBoxRefreshUploads, null);
            this.pictureBoxRefreshUploads.TabIndex = 0;
            this.pictureBoxRefreshUploads.TabStop = false;
            base.ttDefault.SetToolTip(this.pictureBoxRefreshUploads, "<LOC>Refresh Files");
            this.pictureBoxRefreshUploads.Click += new EventHandler(this.pictureBoxRefreshUploads_Click);
            this.treeViewUpload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeViewUpload.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewUpload.BorderStyle = BorderStyle.None;
            this.treeViewUpload.HideSelection = false;
            this.treeViewUpload.ImageIndex = 0;
            this.treeViewUpload.ImageList = this.imageListContentTypes;
            this.treeViewUpload.Location = new Point(0, 80);
            this.treeViewUpload.Name = "treeViewUpload";
            this.treeViewUpload.SelectedImageIndex = 0;
            this.treeViewUpload.Size = new Size(0x120, 0x206);
            base.ttDefault.SetSuperTip(this.treeViewUpload, null);
            this.treeViewUpload.TabIndex = 1;
            this.treeViewUpload.AfterSelect += new TreeViewEventHandler(this.treeViewUpload_AfterSelect);
            this.gpgGroupBoxUploadVersion.AutoStyle = false;
            this.gpgGroupBoxUploadVersion.BackColor = Color.Black;
            this.gpgGroupBoxUploadVersion.Controls.Add(this.gpgPanelUploadVersion);
            this.gpgGroupBoxUploadVersion.CutCorner = true;
            this.gpgGroupBoxUploadVersion.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgGroupBoxUploadVersion.HeaderImage = GroupPanelImages.blue_gradient;
            this.gpgGroupBoxUploadVersion.IsStyled = true;
            this.gpgGroupBoxUploadVersion.Location = new Point(8, 0x11f);
            this.gpgGroupBoxUploadVersion.Margin = new Padding(4, 3, 4, 3);
            this.gpgGroupBoxUploadVersion.Name = "gpgGroupBoxUploadVersion";
            this.gpgGroupBoxUploadVersion.Size = new Size(0x2b0, 180);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxUploadVersion, null);
            this.gpgGroupBoxUploadVersion.TabIndex = 0x23;
            this.gpgGroupBoxUploadVersion.Text = "<LOC>Version Info";
            this.gpgGroupBoxUploadVersion.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgGroupBoxUploadVersion.TextPadding = new Padding(8, 0, 0, 0);
            this.gpgGroupBoxUploadVersion.Visible = false;
            this.gpgPanelUploadVersion.AutoScroll = true;
            this.gpgPanelUploadVersion.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelUploadVersion.BorderThickness = 2;
            this.gpgPanelUploadVersion.Controls.Add(this.gpgTextAreaVersionNotes);
            this.gpgPanelUploadVersion.Controls.Add(this.gpgLabelUploadVersionNotes);
            this.gpgPanelUploadVersion.Controls.Add(this.gpgLabelUploadCurrentVersion);
            this.gpgPanelUploadVersion.DrawBorder = false;
            this.gpgPanelUploadVersion.Location = new Point(2, 0x16);
            this.gpgPanelUploadVersion.Name = "gpgPanelUploadVersion";
            this.gpgPanelUploadVersion.Size = new Size(0x2ac, 0x88);
            base.ttDefault.SetSuperTip(this.gpgPanelUploadVersion, null);
            this.gpgPanelUploadVersion.TabIndex = 0;
            this.gpgTextAreaVersionNotes.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgTextAreaVersionNotes.BorderColor = Color.White;
            this.gpgTextAreaVersionNotes.Location = new Point(4, 0x2c);
            this.gpgTextAreaVersionNotes.Name = "gpgTextAreaVersionNotes";
            this.gpgTextAreaVersionNotes.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaVersionNotes.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaVersionNotes.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaVersionNotes.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaVersionNotes.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaVersionNotes.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaVersionNotes.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaVersionNotes.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaVersionNotes.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaVersionNotes.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaVersionNotes.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaVersionNotes.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaVersionNotes.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaVersionNotes.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaVersionNotes.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaVersionNotes.Properties.MaxLength = 0x400;
            this.gpgTextAreaVersionNotes.Size = new Size(0x2a4, 0x59);
            this.gpgTextAreaVersionNotes.TabIndex = 2;
            this.gpgTextAreaVersionNotes.EditValueChanged += new EventHandler(this.UploadGeneralOptionsChanged);
            this.gpgLabelUploadVersionNotes.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelUploadVersionNotes.AutoSize = true;
            this.gpgLabelUploadVersionNotes.AutoStyle = true;
            this.gpgLabelUploadVersionNotes.Font = new Font("Arial", 9.75f);
            this.gpgLabelUploadVersionNotes.ForeColor = Color.White;
            this.gpgLabelUploadVersionNotes.IgnoreMouseWheel = false;
            this.gpgLabelUploadVersionNotes.IsStyled = false;
            this.gpgLabelUploadVersionNotes.Location = new Point(4, 0x19);
            this.gpgLabelUploadVersionNotes.Name = "gpgLabelUploadVersionNotes";
            this.gpgLabelUploadVersionNotes.Size = new Size(0x84, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelUploadVersionNotes, null);
            this.gpgLabelUploadVersionNotes.TabIndex = 1;
            this.gpgLabelUploadVersionNotes.Text = "<LOC>Version Notes";
            this.gpgLabelUploadVersionNotes.TextStyle = TextStyles.Bold;
            this.gpgLabelUploadCurrentVersion.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelUploadCurrentVersion.AutoSize = true;
            this.gpgLabelUploadCurrentVersion.AutoStyle = true;
            this.gpgLabelUploadCurrentVersion.Font = new Font("Arial", 9.75f);
            this.gpgLabelUploadCurrentVersion.ForeColor = Color.White;
            this.gpgLabelUploadCurrentVersion.IgnoreMouseWheel = false;
            this.gpgLabelUploadCurrentVersion.IsStyled = false;
            this.gpgLabelUploadCurrentVersion.Location = new Point(4, 4);
            this.gpgLabelUploadCurrentVersion.Name = "gpgLabelUploadCurrentVersion";
            this.gpgLabelUploadCurrentVersion.Size = new Size(140, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelUploadCurrentVersion, null);
            this.gpgLabelUploadCurrentVersion.TabIndex = 0;
            this.gpgLabelUploadCurrentVersion.Text = "<LOC>Current Version";
            this.gpgLabelUploadCurrentVersion.TextStyle = TextStyles.Bold;
            this.gpgGroupBoxUploadGeneral.AutoStyle = false;
            this.gpgGroupBoxUploadGeneral.BackColor = Color.Black;
            this.gpgGroupBoxUploadGeneral.Controls.Add(this.gpgPanelUploadGeneral);
            this.gpgGroupBoxUploadGeneral.CutCorner = false;
            this.gpgGroupBoxUploadGeneral.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgGroupBoxUploadGeneral.HeaderImage = GroupPanelImages.blue_gradient;
            this.gpgGroupBoxUploadGeneral.IsStyled = true;
            this.gpgGroupBoxUploadGeneral.Location = new Point(8, 8);
            this.gpgGroupBoxUploadGeneral.Margin = new Padding(4, 3, 4, 3);
            this.gpgGroupBoxUploadGeneral.Name = "gpgGroupBoxUploadGeneral";
            this.gpgGroupBoxUploadGeneral.Size = new Size(0x2b0, 0xb7);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxUploadGeneral, null);
            this.gpgGroupBoxUploadGeneral.TabIndex = 0x22;
            this.gpgGroupBoxUploadGeneral.Text = "<LOC>General Info";
            this.gpgGroupBoxUploadGeneral.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgGroupBoxUploadGeneral.TextPadding = new Padding(8, 0, 0, 0);
            this.gpgGroupBoxUploadGeneral.Visible = false;
            this.gpgPanelUploadGeneral.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanelUploadGeneral.AutoScroll = true;
            this.gpgPanelUploadGeneral.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelUploadGeneral.BorderThickness = 2;
            this.gpgPanelUploadGeneral.Controls.Add(this.skinButtonDeleteUploadDependency);
            this.gpgPanelUploadGeneral.Controls.Add(this.skinButtonUploadDependency);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgTextBoxUploadSearchKeywords);
            this.gpgPanelUploadGeneral.Controls.Add(this.listBoxUploadDependencies);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgLabel6);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgLabel9);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgLabel27);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgTextBoxUploadDesc);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgLabelUploadLocation);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgTextBoxUploadName);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgLabel25);
            this.gpgPanelUploadGeneral.Controls.Add(this.gpgLabel29);
            this.gpgPanelUploadGeneral.DrawBorder = false;
            this.gpgPanelUploadGeneral.Location = new Point(2, 0x15);
            this.gpgPanelUploadGeneral.Name = "gpgPanelUploadGeneral";
            this.gpgPanelUploadGeneral.Size = new Size(0x2ab, 160);
            base.ttDefault.SetSuperTip(this.gpgPanelUploadGeneral, null);
            this.gpgPanelUploadGeneral.TabIndex = 3;
            this.skinButtonDeleteUploadDependency.AutoStyle = true;
            this.skinButtonDeleteUploadDependency.BackColor = Color.Black;
            this.skinButtonDeleteUploadDependency.ButtonState = 0;
            this.skinButtonDeleteUploadDependency.CausesValidation = false;
            this.skinButtonDeleteUploadDependency.DialogResult = DialogResult.OK;
            this.skinButtonDeleteUploadDependency.DisabledForecolor = Color.Gray;
            this.skinButtonDeleteUploadDependency.DrawColor = Color.White;
            this.skinButtonDeleteUploadDependency.DrawEdges = false;
            this.skinButtonDeleteUploadDependency.Enabled = false;
            this.skinButtonDeleteUploadDependency.FocusColor = Color.Yellow;
            this.skinButtonDeleteUploadDependency.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDeleteUploadDependency.ForeColor = Color.White;
            this.skinButtonDeleteUploadDependency.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonDeleteUploadDependency.IsStyled = true;
            this.skinButtonDeleteUploadDependency.Location = new Point(0x29, 0x7a);
            this.skinButtonDeleteUploadDependency.Margin = new Padding(0);
            this.skinButtonDeleteUploadDependency.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonDeleteUploadDependency.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonDeleteUploadDependency.Name = "skinButtonDeleteUploadDependency";
            this.skinButtonDeleteUploadDependency.Size = new Size(0x25, 0x25);
            this.skinButtonDeleteUploadDependency.SkinBasePath = @"Dialog\ContentManager\BtnDelete";
            base.ttDefault.SetSuperTip(this.skinButtonDeleteUploadDependency, null);
            this.skinButtonDeleteUploadDependency.TabIndex = 0x17;
            this.skinButtonDeleteUploadDependency.TabStop = true;
            this.skinButtonDeleteUploadDependency.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDeleteUploadDependency.TextPadding = new Padding(0);
            this.skinButtonDeleteUploadDependency.Click += new EventHandler(this.skinButtonDeleteUploadDependency_Click);
            this.skinButtonUploadDependency.AutoStyle = true;
            this.skinButtonUploadDependency.BackColor = Color.Black;
            this.skinButtonUploadDependency.ButtonState = 0;
            this.skinButtonUploadDependency.CausesValidation = false;
            this.skinButtonUploadDependency.DialogResult = DialogResult.OK;
            this.skinButtonUploadDependency.DisabledForecolor = Color.Gray;
            this.skinButtonUploadDependency.DrawColor = Color.White;
            this.skinButtonUploadDependency.DrawEdges = false;
            this.skinButtonUploadDependency.FocusColor = Color.Yellow;
            this.skinButtonUploadDependency.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonUploadDependency.ForeColor = Color.White;
            this.skinButtonUploadDependency.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonUploadDependency.IsStyled = true;
            this.skinButtonUploadDependency.Location = new Point(1, 0x7a);
            this.skinButtonUploadDependency.Margin = new Padding(0);
            this.skinButtonUploadDependency.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonUploadDependency.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonUploadDependency.Name = "skinButtonUploadDependency";
            this.skinButtonUploadDependency.Size = new Size(0x25, 0x25);
            this.skinButtonUploadDependency.SkinBasePath = @"Dialog\ContentManager\BtnBrowseTo";
            base.ttDefault.SetSuperTip(this.skinButtonUploadDependency, null);
            this.skinButtonUploadDependency.TabIndex = 7;
            this.skinButtonUploadDependency.TabStop = true;
            this.skinButtonUploadDependency.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonUploadDependency.TextPadding = new Padding(0);
            this.skinButtonUploadDependency.Click += new EventHandler(this.skinButtonUploadDependency_Click);
            this.gpgTextBoxUploadSearchKeywords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxUploadSearchKeywords.Location = new Point(0x15a, 0x4a);
            this.gpgTextBoxUploadSearchKeywords.MinimumSize = new Size(250, 20);
            this.gpgTextBoxUploadSearchKeywords.Name = "gpgTextBoxUploadSearchKeywords";
            this.gpgTextBoxUploadSearchKeywords.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxUploadSearchKeywords.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxUploadSearchKeywords.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxUploadSearchKeywords.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxUploadSearchKeywords.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxUploadSearchKeywords.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxUploadSearchKeywords.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxUploadSearchKeywords.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxUploadSearchKeywords.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxUploadSearchKeywords.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxUploadSearchKeywords.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxUploadSearchKeywords.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxUploadSearchKeywords.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxUploadSearchKeywords.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxUploadSearchKeywords.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxUploadSearchKeywords.Properties.MaxLength = 0x100;
            this.gpgTextBoxUploadSearchKeywords.Size = new Size(0x151, 20);
            this.gpgTextBoxUploadSearchKeywords.TabIndex = 0x16;
            this.gpgTextBoxUploadSearchKeywords.EditValueChanged += new EventHandler(this.UploadGeneralOptionsChanged);
            this.listBoxUploadDependencies.BackColor = Color.Black;
            this.listBoxUploadDependencies.BorderStyle = BorderStyle.None;
            this.listBoxUploadDependencies.FormattingEnabled = true;
            this.listBoxUploadDependencies.Location = new Point(0x52, 0x79);
            this.listBoxUploadDependencies.Name = "listBoxUploadDependencies";
            this.listBoxUploadDependencies.Size = new Size(600, 0x27);
            base.ttDefault.SetSuperTip(this.listBoxUploadDependencies, null);
            this.listBoxUploadDependencies.TabIndex = 6;
            this.listBoxUploadDependencies.SelectedIndexChanged += new EventHandler(this.listBoxUploadDependencies_SelectedIndexChanged);
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x15a, 0x35);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x98, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x15;
            this.gpgLabel6.Text = "<LOC>Search Keywords";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel9.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0, 0x62);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Padding = new Padding(6, 0, 0, 0);
            this.gpgLabel9.Size = new Size(0x2ab, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel9, null);
            this.gpgLabel9.TabIndex = 5;
            this.gpgLabel9.Text = "<LOC>Dependencies";
            this.gpgLabel9.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel9.TextStyle = TextStyles.Custom;
            this.gpgLabel27.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel27.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel27.AutoStyle = true;
            this.gpgLabel27.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel27.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel27.ForeColor = Color.White;
            this.gpgLabel27.IgnoreMouseWheel = false;
            this.gpgLabel27.IsStyled = false;
            this.gpgLabel27.Location = new Point(0xfd, 2);
            this.gpgLabel27.Name = "gpgLabel27";
            this.gpgLabel27.Size = new Size(0x66, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel27, null);
            this.gpgLabel27.TabIndex = 2;
            this.gpgLabel27.Text = "<LOC>Location";
            this.gpgLabel27.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel27.TextStyle = TextStyles.Custom;
            this.gpgTextBoxUploadDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxUploadDesc.Location = new Point(4, 0x4a);
            this.gpgTextBoxUploadDesc.MinimumSize = new Size(250, 20);
            this.gpgTextBoxUploadDesc.Name = "gpgTextBoxUploadDesc";
            this.gpgTextBoxUploadDesc.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxUploadDesc.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxUploadDesc.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxUploadDesc.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxUploadDesc.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxUploadDesc.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxUploadDesc.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxUploadDesc.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxUploadDesc.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxUploadDesc.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxUploadDesc.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxUploadDesc.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxUploadDesc.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxUploadDesc.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxUploadDesc.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxUploadDesc.Properties.MaxLength = 0x400;
            this.gpgTextBoxUploadDesc.Size = new Size(0x146, 20);
            this.gpgTextBoxUploadDesc.TabIndex = 0x13;
            this.gpgTextBoxUploadDesc.EditValueChanged += new EventHandler(this.UploadGeneralOptionsChanged);
            this.gpgLabelUploadLocation.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelUploadLocation.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelUploadLocation.AutoStyle = true;
            this.gpgLabelUploadLocation.Font = new Font("Arial", 9.75f);
            this.gpgLabelUploadLocation.ForeColor = Color.White;
            this.gpgLabelUploadLocation.IgnoreMouseWheel = false;
            this.gpgLabelUploadLocation.IsStyled = false;
            this.gpgLabelUploadLocation.Location = new Point(0xfd, 0x13);
            this.gpgLabelUploadLocation.Name = "gpgLabelUploadLocation";
            this.gpgLabelUploadLocation.Size = new Size(430, 0x22);
            base.ttDefault.SetSuperTip(this.gpgLabelUploadLocation, null);
            this.gpgLabelUploadLocation.TabIndex = 3;
            this.gpgLabelUploadLocation.Text = "gpgLabel26";
            this.gpgLabelUploadLocation.TextStyle = TextStyles.Default;
            this.gpgLabelUploadLocation.Click += new EventHandler(this.gpgLabelUploadLocation_Click);
            this.gpgTextBoxUploadName.Location = new Point(4, 0x17);
            this.gpgTextBoxUploadName.MinimumSize = new Size(150, 20);
            this.gpgTextBoxUploadName.Name = "gpgTextBoxUploadName";
            this.gpgTextBoxUploadName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxUploadName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxUploadName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxUploadName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxUploadName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxUploadName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxUploadName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxUploadName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxUploadName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxUploadName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxUploadName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxUploadName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxUploadName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxUploadName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxUploadName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxUploadName.Properties.MaxLength = 0x100;
            this.gpgTextBoxUploadName.Size = new Size(230, 20);
            this.gpgTextBoxUploadName.TabIndex = 0x10;
            this.gpgTextBoxUploadName.EditValueChanged += new EventHandler(this.UploadGeneralOptionsChanged);
            this.gpgLabel25.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel25.AutoStyle = true;
            this.gpgLabel25.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel25.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel25.ForeColor = Color.White;
            this.gpgLabel25.IgnoreMouseWheel = false;
            this.gpgLabel25.IsStyled = false;
            this.gpgLabel25.Location = new Point(0, 0x35);
            this.gpgLabel25.Name = "gpgLabel25";
            this.gpgLabel25.Padding = new Padding(6, 0, 0, 0);
            this.gpgLabel25.Size = new Size(0x2ab, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel25, null);
            this.gpgLabel25.TabIndex = 4;
            this.gpgLabel25.Text = "<LOC>Download Description";
            this.gpgLabel25.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel25.TextStyle = TextStyles.Custom;
            this.gpgLabel29.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel29.AutoStyle = true;
            this.gpgLabel29.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel29.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel29.ForeColor = Color.White;
            this.gpgLabel29.IgnoreMouseWheel = false;
            this.gpgLabel29.IsStyled = false;
            this.gpgLabel29.Location = new Point(0, 2);
            this.gpgLabel29.Name = "gpgLabel29";
            this.gpgLabel29.Padding = new Padding(6, 0, 0, 0);
            this.gpgLabel29.Size = new Size(0x2ab, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel29, null);
            this.gpgLabel29.TabIndex = 0;
            this.gpgLabel29.Text = "<LOC>Name";
            this.gpgLabel29.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel29.TextStyle = TextStyles.Custom;
            this.skinButtonDeleteUploadVersion.AutoStyle = true;
            this.skinButtonDeleteUploadVersion.BackColor = Color.Black;
            this.skinButtonDeleteUploadVersion.ButtonState = 0;
            this.skinButtonDeleteUploadVersion.DialogResult = DialogResult.OK;
            this.skinButtonDeleteUploadVersion.DisabledForecolor = Color.Gray;
            this.skinButtonDeleteUploadVersion.DrawColor = Color.White;
            this.skinButtonDeleteUploadVersion.DrawEdges = true;
            this.skinButtonDeleteUploadVersion.FocusColor = Color.Yellow;
            this.skinButtonDeleteUploadVersion.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDeleteUploadVersion.ForeColor = Color.White;
            this.skinButtonDeleteUploadVersion.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDeleteUploadVersion.IsStyled = true;
            this.skinButtonDeleteUploadVersion.Location = new Point(0x9d, 0x1d9);
            this.skinButtonDeleteUploadVersion.Name = "skinButtonDeleteUploadVersion";
            this.skinButtonDeleteUploadVersion.Size = new Size(0x91, 0x1a);
            this.skinButtonDeleteUploadVersion.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonDeleteUploadVersion, null);
            this.skinButtonDeleteUploadVersion.TabIndex = 0x21;
            this.skinButtonDeleteUploadVersion.TabStop = true;
            this.skinButtonDeleteUploadVersion.Text = "<LOC>Delete Version";
            this.skinButtonDeleteUploadVersion.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDeleteUploadVersion.TextPadding = new Padding(0);
            this.skinButtonDeleteUploadVersion.Visible = false;
            this.skinButtonDeleteUploadVersion.Click += new EventHandler(this.skinButtonDeleteUploadVersion_Click);
            this.skinButtonDeleteUploadAll.AutoStyle = true;
            this.skinButtonDeleteUploadAll.BackColor = Color.Black;
            this.skinButtonDeleteUploadAll.ButtonState = 0;
            this.skinButtonDeleteUploadAll.DialogResult = DialogResult.OK;
            this.skinButtonDeleteUploadAll.DisabledForecolor = Color.Gray;
            this.skinButtonDeleteUploadAll.DrawColor = Color.White;
            this.skinButtonDeleteUploadAll.DrawEdges = true;
            this.skinButtonDeleteUploadAll.FocusColor = Color.Yellow;
            this.skinButtonDeleteUploadAll.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDeleteUploadAll.ForeColor = Color.White;
            this.skinButtonDeleteUploadAll.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDeleteUploadAll.IsStyled = true;
            this.skinButtonDeleteUploadAll.Location = new Point(0x134, 0x1d9);
            this.skinButtonDeleteUploadAll.Name = "skinButtonDeleteUploadAll";
            this.skinButtonDeleteUploadAll.Size = new Size(0x91, 0x1a);
            this.skinButtonDeleteUploadAll.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonDeleteUploadAll, null);
            this.skinButtonDeleteUploadAll.TabIndex = 0x20;
            this.skinButtonDeleteUploadAll.TabStop = true;
            this.skinButtonDeleteUploadAll.Text = "<LOC>Delete All Versions";
            this.skinButtonDeleteUploadAll.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDeleteUploadAll.TextPadding = new Padding(0);
            this.skinButtonDeleteUploadAll.Visible = false;
            this.skinButtonDeleteUploadAll.Click += new EventHandler(this.skinButtonDeleteUploadAll_Click);
            this.skinButtonUpload.AutoStyle = true;
            this.skinButtonUpload.BackColor = Color.Black;
            this.skinButtonUpload.ButtonState = 0;
            this.skinButtonUpload.DialogResult = DialogResult.OK;
            this.skinButtonUpload.DisabledForecolor = Color.Gray;
            this.skinButtonUpload.DrawColor = Color.White;
            this.skinButtonUpload.DrawEdges = true;
            this.skinButtonUpload.FocusColor = Color.Yellow;
            this.skinButtonUpload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonUpload.ForeColor = Color.White;
            this.skinButtonUpload.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonUpload.IsStyled = true;
            this.skinButtonUpload.Location = new Point(6, 0x1d9);
            this.skinButtonUpload.Name = "skinButtonUpload";
            this.skinButtonUpload.Size = new Size(0x91, 0x1a);
            this.skinButtonUpload.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonUpload, null);
            this.skinButtonUpload.TabIndex = 0x1f;
            this.skinButtonUpload.TabStop = true;
            this.skinButtonUpload.Text = "<LOC>Upload";
            this.skinButtonUpload.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonUpload.TextPadding = new Padding(0);
            this.skinButtonUpload.Visible = false;
            this.skinButtonUpload.Click += new EventHandler(this.skinButtonUpload_Click);
            this.gpgLabelNoUploadSelected.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNoUploadSelected.AutoStyle = true;
            this.gpgLabelNoUploadSelected.BackColor = Color.Transparent;
            this.gpgLabelNoUploadSelected.Dock = DockStyle.Fill;
            this.gpgLabelNoUploadSelected.Font = new Font("Arial", 9.75f);
            this.gpgLabelNoUploadSelected.ForeColor = Color.White;
            this.gpgLabelNoUploadSelected.IgnoreMouseWheel = false;
            this.gpgLabelNoUploadSelected.IsStyled = false;
            this.gpgLabelNoUploadSelected.Location = new Point(0, 0);
            this.gpgLabelNoUploadSelected.Name = "gpgLabelNoUploadSelected";
            this.gpgLabelNoUploadSelected.Size = new Size(0x2c2, 0x256);
            base.ttDefault.SetSuperTip(this.gpgLabelNoUploadSelected, null);
            this.gpgLabelNoUploadSelected.TabIndex = 0;
            this.gpgLabelNoUploadSelected.Text = "<LOC>No valid content selected. Please select content from the browser on the left.";
            this.gpgLabelNoUploadSelected.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelNoUploadSelected.TextStyle = TextStyles.Default;
            this.tabActivity.AutoStyle = true;
            this.tabActivity.BackColor = Color.Black;
            this.tabActivity.ButtonState = 0;
            this.tabActivity.DialogResult = DialogResult.OK;
            this.tabActivity.DisabledForecolor = Color.Gray;
            this.tabActivity.DrawColor = Color.White;
            this.tabActivity.DrawEdges = true;
            this.tabActivity.FocusColor = Color.Yellow;
            this.tabActivity.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.tabActivity.ForeColor = Color.White;
            this.tabActivity.HorizontalScalingMode = ScalingModes.Tile;
            this.tabActivity.IsStyled = true;
            this.tabActivity.Location = new Point(0x178, 0x53);
            this.tabActivity.Name = "tabActivity";
            this.tabActivity.Size = new Size(0x7a, 0x1c);
            this.tabActivity.SkinBasePath = @"Dialog\ContentManager\TabLarge";
            base.ttDefault.SetSuperTip(this.tabActivity, null);
            this.tabActivity.TabIndex = 11;
            this.tabActivity.TabStop = true;
            this.tabActivity.Text = "<LOC>Activity";
            this.tabActivity.TextAlign = ContentAlignment.MiddleLeft;
            this.tabActivity.TextPadding = new Padding(6, 0, 0, 0);
            this.tabActivity.Click += new EventHandler(this.ChangePanel);
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.msQuickButtons.AutoSize = false;
            this.msQuickButtons.BackgroundImage = (Image) manager.GetObject("msQuickButtons.BackgroundImage");
            this.msQuickButtons.Dock = DockStyle.None;
            this.msQuickButtons.GripMargin = new Padding(0);
            this.msQuickButtons.ImageScalingSize = new Size(0x2d, 0x2d);
            this.msQuickButtons.Items.AddRange(new ToolStripItem[] { this.btnOptions, this.btnHelp, this.btnMapDiagnose, this.btnMore });
            this.msQuickButtons.Location = new Point(60, 0x2ca);
            this.msQuickButtons.Name = "msQuickButtons";
            this.msQuickButtons.Padding = new Padding(0, 0, 10, 0);
            this.msQuickButtons.RenderMode = ToolStripRenderMode.Professional;
            this.msQuickButtons.ShowItemToolTips = true;
            this.msQuickButtons.Size = new Size(910, 0x34);
            base.ttDefault.SetSuperTip(this.msQuickButtons, null);
            this.msQuickButtons.TabIndex = 12;
            this.msQuickButtons.Paint += new PaintEventHandler(this.msQuickButtons_Paint);
            this.msQuickButtons.SizeChanged += new EventHandler(this.msQuickButtons_SizeChanged);
            this.btnOptions.AutoSize = false;
            this.btnOptions.AutoToolTip = true;
            this.btnOptions.Image = (Image) manager.GetObject("btnOptions.Image");
            this.btnOptions.ImageScaling = ToolStripItemImageScaling.None;
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.ShortcutKeys = Keys.F8;
            this.btnOptions.Size = new Size(0x25, 0x34);
            this.btnOptions.ToolTipText = "<LOC>Options";
            this.btnOptions.Click += new EventHandler(this.btnOptions_Click);
            this.btnHelp.AutoSize = false;
            this.btnHelp.AutoToolTip = true;
            this.btnHelp.Image = (Image) manager.GetObject("btnHelp.Image");
            this.btnHelp.ImageScaling = ToolStripItemImageScaling.None;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.ShortcutKeys = Keys.F8;
            this.btnHelp.Size = new Size(0x25, 0x34);
            this.btnHelp.ToolTipText = "<LOC>Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnMapDiagnose.AutoSize = false;
            this.btnMapDiagnose.AutoToolTip = true;
            this.btnMapDiagnose.Image = (Image) manager.GetObject("btnMapDiagnose.Image");
            this.btnMapDiagnose.ImageScaling = ToolStripItemImageScaling.None;
            this.btnMapDiagnose.Name = "btnMapDiagnose";
            this.btnMapDiagnose.ShortcutKeys = Keys.F8;
            this.btnMapDiagnose.Size = new Size(0x25, 0x34);
            this.btnMapDiagnose.ToolTipText = "<LOC>Map Upload Diagnostics";
            this.btnMapDiagnose.Click += new EventHandler(this.btnMapDiagnose_Click);
            this.btnMore.AutoSize = false;
            this.btnMore.AutoToolTip = true;
            this.btnMore.Image = (Image) manager.GetObject("btnMore.Image");
            this.btnMore.ImageScaling = ToolStripItemImageScaling.None;
            this.btnMore.Name = "btnMore";
            this.btnMore.ShortcutKeys = Keys.F6;
            this.btnMore.Size = new Size(20, 0x34);
            this.btnMore.ToolTipText = "<LOC>More...";
            this.gpgPanel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanel2.BackgroundImage = (Image) manager.GetObject("gpgPanel2.BackgroundImage");
            this.gpgPanel2.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel2.BorderThickness = 2;
            this.gpgPanel2.DrawBorder = false;
            this.gpgPanel2.Location = new Point(6, 0x3e);
            this.gpgPanel2.Name = "gpgPanel2";
            this.gpgPanel2.Size = new Size(0x3f4, 0x30);
            base.ttDefault.SetSuperTip(this.gpgPanel2, null);
            this.gpgPanel2.TabIndex = 13;
            this.gpgPanelActivity.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelActivity.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelActivity.BorderThickness = 2;
            this.gpgPanelActivity.Controls.Add(this.splitContainerActivity);
            this.gpgPanelActivity.DrawBorder = false;
            this.gpgPanelActivity.Location = new Point(12, 110);
            this.gpgPanelActivity.Name = "gpgPanelActivity";
            this.gpgPanelActivity.Size = new Size(0x3e8, 0x25a);
            base.ttDefault.SetSuperTip(this.gpgPanelActivity, null);
            this.gpgPanelActivity.TabIndex = 14;
            this.splitContainerActivity.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerActivity.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerActivity.Location = new Point(2, 2);
            this.splitContainerActivity.Name = "splitContainerActivity";
            this.splitContainerActivity.Orientation = Orientation.Horizontal;
            this.splitContainerActivity.Panel1.BackColor = Color.Black;
            this.splitContainerActivity.Panel1.BackgroundImage = (Image) manager.GetObject("splitContainerActivity.Panel1.BackgroundImage");
            this.splitContainerActivity.Panel1.Controls.Add(this.gpgLabel4);
            this.splitContainerActivity.Panel1.Controls.Add(this.gpgLabel3);
            this.splitContainerActivity.Panel1.Controls.Add(this.gpgLabel2);
            this.splitContainerActivity.Panel1.Controls.Add(this.gpgPanelDownActivity);
            this.splitContainerActivity.Panel1.Controls.Add(this.skinLabel5);
            base.ttDefault.SetSuperTip(this.splitContainerActivity.Panel1, null);
            this.splitContainerActivity.Panel2.BackColor = Color.Black;
            this.splitContainerActivity.Panel2.BackgroundImage = (Image) manager.GetObject("splitContainerActivity.Panel2.BackgroundImage");
            this.splitContainerActivity.Panel2.Controls.Add(this.gpgLabel5);
            this.splitContainerActivity.Panel2.Controls.Add(this.gpgLabel7);
            this.splitContainerActivity.Panel2.Controls.Add(this.gpgLabel8);
            this.splitContainerActivity.Panel2.Controls.Add(this.gpgPanelUpActivity);
            this.splitContainerActivity.Panel2.Controls.Add(this.skinLabel6);
            base.ttDefault.SetSuperTip(this.splitContainerActivity.Panel2, null);
            this.splitContainerActivity.Size = new Size(0x3e4, 0x256);
            this.splitContainerActivity.SplitterDistance = 330;
            this.splitContainerActivity.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainerActivity, null);
            this.splitContainerActivity.TabIndex = 0;
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel4.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x285, 0x16);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x86, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 0x15;
            this.gpgLabel4.Text = "<LOC>Status";
            this.gpgLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel4.TextStyle = TextStyles.Custom;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel3.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(270, 0x16);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x86, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 20;
            this.gpgLabel3.Text = "<LOC>Progress";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel2.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0, 0x16);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Padding = new Padding(6, 0, 0, 0);
            this.gpgLabel2.Size = new Size(0x3e4, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 0x13;
            this.gpgLabel2.Text = "<LOC>Name";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgPanelDownActivity.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelDownActivity.AutoScroll = true;
            this.gpgPanelDownActivity.BackColor = Color.Transparent;
            this.gpgPanelDownActivity.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelDownActivity.BorderThickness = 2;
            this.gpgPanelDownActivity.DrawBorder = false;
            this.gpgPanelDownActivity.Location = new Point(6, 0x2c);
            this.gpgPanelDownActivity.Name = "gpgPanelDownActivity";
            this.gpgPanelDownActivity.Size = new Size(0x3d7, 0x119);
            base.ttDefault.SetSuperTip(this.gpgPanelDownActivity, null);
            this.gpgPanelDownActivity.TabIndex = 0x12;
            this.skinLabel5.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel5.AutoStyle = false;
            this.skinLabel5.BackColor = Color.Transparent;
            this.skinLabel5.DrawEdges = true;
            this.skinLabel5.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel5.ForeColor = Color.White;
            this.skinLabel5.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel5.IsStyled = false;
            this.skinLabel5.Location = new Point(0, 0);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new Size(0x3e4, 20);
            this.skinLabel5.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel5, null);
            this.skinLabel5.TabIndex = 0x11;
            this.skinLabel5.Text = "<LOC>Downloads";
            this.skinLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel5.TextPadding = new Padding(0);
            this.gpgLabel5.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel5.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x285, 0x16);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x86, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x18;
            this.gpgLabel5.Text = "<LOC>Status";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel7.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(270, 0x16);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x86, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 0x17;
            this.gpgLabel7.Text = "<LOC>Progress";
            this.gpgLabel7.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel7.TextStyle = TextStyles.Custom;
            this.gpgLabel8.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.BackColor = Color.FromArgb(0x66, 0x66, 0x66);
            this.gpgLabel8.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0, 0x16);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Padding = new Padding(6, 0, 0, 0);
            this.gpgLabel8.Size = new Size(0x3e4, 0x11);
            base.ttDefault.SetSuperTip(this.gpgLabel8, null);
            this.gpgLabel8.TabIndex = 0x16;
            this.gpgLabel8.Text = "<LOC>Name";
            this.gpgLabel8.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel8.TextStyle = TextStyles.Custom;
            this.gpgPanelUpActivity.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelUpActivity.AutoScroll = true;
            this.gpgPanelUpActivity.BackColor = Color.Transparent;
            this.gpgPanelUpActivity.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelUpActivity.BorderThickness = 2;
            this.gpgPanelUpActivity.DrawBorder = false;
            this.gpgPanelUpActivity.Location = new Point(6, 0x2c);
            this.gpgPanelUpActivity.Name = "gpgPanelUpActivity";
            this.gpgPanelUpActivity.Size = new Size(0x3d7, 0xd8);
            base.ttDefault.SetSuperTip(this.gpgPanelUpActivity, null);
            this.gpgPanelUpActivity.TabIndex = 0x13;
            this.skinLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel6.AutoStyle = false;
            this.skinLabel6.BackColor = Color.Transparent;
            this.skinLabel6.DrawEdges = true;
            this.skinLabel6.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel6.ForeColor = Color.White;
            this.skinLabel6.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel6.IsStyled = false;
            this.skinLabel6.Location = new Point(0, 0);
            this.skinLabel6.Name = "skinLabel6";
            this.skinLabel6.Size = new Size(0x3e4, 20);
            this.skinLabel6.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel6, null);
            this.skinLabel6.TabIndex = 0x12;
            this.skinLabel6.Text = "<LOC>Uploads";
            this.skinLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel6.TextPadding = new Padding(0);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x400, 0x300);
            base.Controls.Add(this.gpgPanelDownload);
            base.Controls.Add(this.gpgPanelUpload);
            base.Controls.Add(this.gpgPanelMyContent);
            base.Controls.Add(this.gpgPanelActivity);
            base.Controls.Add(this.tabMyContent);
            base.Controls.Add(this.tabDownload);
            base.Controls.Add(this.tabUpload);
            base.Controls.Add(this.tabActivity);
            base.Controls.Add(this.msQuickButtons);
            base.Controls.Add(this.gpgPanel2);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x200, 0x174);
            base.Name = "DlgContentManager";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>The Vault";
            base.Controls.SetChildIndex(this.gpgPanel2, 0);
            base.Controls.SetChildIndex(this.msQuickButtons, 0);
            base.Controls.SetChildIndex(this.tabActivity, 0);
            base.Controls.SetChildIndex(this.tabUpload, 0);
            base.Controls.SetChildIndex(this.tabDownload, 0);
            base.Controls.SetChildIndex(this.tabMyContent, 0);
            base.Controls.SetChildIndex(this.gpgPanelActivity, 0);
            base.Controls.SetChildIndex(this.gpgPanelMyContent, 0);
            base.Controls.SetChildIndex(this.gpgPanelUpload, 0);
            base.Controls.SetChildIndex(this.gpgPanelDownload, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgPanelMyContent.ResumeLayout(false);
            this.splitContainerMyContent.Panel1.ResumeLayout(false);
            this.splitContainerMyContent.Panel2.ResumeLayout(false);
            this.splitContainerMyContent.ResumeLayout(false);
            this.gpgPanelDownload.ResumeLayout(false);
            this.splitContainerDownload.Panel1.ResumeLayout(false);
            this.splitContainerDownload.Panel2.ResumeLayout(false);
            this.splitContainerDownload.ResumeLayout(false);
            this.gpgGroupBoxResults.ResumeLayout(false);
            this.dataGridSearchResults.EndInit();
            this.gvResults.EndInit();
            this.repositoryItemPictureEdit1.EndInit();
            this.repositoryItemRatingStars.EndInit();
            this.repositoryItemChatLink.EndInit();
            this.repositoryItemVersionLink.EndInit();
            this.gpgGroupBoxCriteria.ResumeLayout(false);
            this.gpgPanelSearchCriteria.ResumeLayout(false);
            this.gpgPanelSearchCriteria.PerformLayout();
            this.gpgTextBoxSearchCreator.Properties.EndInit();
            this.gpgTextBoxSearchName.Properties.EndInit();
            this.gpgTextBoxSearchKeywords.Properties.EndInit();
            this.gpgPanelUpload.ResumeLayout(false);
            this.splitContainerUpload.Panel1.ResumeLayout(false);
            this.splitContainerUpload.Panel1.PerformLayout();
            this.splitContainerUpload.Panel2.ResumeLayout(false);
            this.splitContainerUpload.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxRefreshUploads).EndInit();
            this.gpgGroupBoxUploadVersion.ResumeLayout(false);
            this.gpgPanelUploadVersion.ResumeLayout(false);
            this.gpgPanelUploadVersion.PerformLayout();
            this.gpgTextAreaVersionNotes.Properties.EndInit();
            this.gpgGroupBoxUploadGeneral.ResumeLayout(false);
            this.gpgPanelUploadGeneral.ResumeLayout(false);
            this.gpgTextBoxUploadSearchKeywords.Properties.EndInit();
            this.gpgTextBoxUploadDesc.Properties.EndInit();
            this.gpgTextBoxUploadName.Properties.EndInit();
            this.msQuickButtons.ResumeLayout(false);
            this.msQuickButtons.PerformLayout();
            this.gpgPanelActivity.ResumeLayout(false);
            this.splitContainerActivity.Panel1.ResumeLayout(false);
            this.splitContainerActivity.Panel2.ResumeLayout(false);
            this.splitContainerActivity.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolstrip()
        {
            this.msQuickButtons.BackgroundImage = SkinManager.GetImage(@"Dialog\ButtonStrip\bottom.png");
            this.msQuickButtons.Height = this.msQuickButtons.BackgroundImage.Height;
            this.btnMore.DropDown.BackgroundImage = DrawUtil.ResizeImage(SkinManager.GetImage("brushbg.png"), this.msQuickButtons.Items[0].Size);
            this.btnOptions.Image = SkinManager.GetImage(@"Dialog\ContentManager\options.png");
            this.btnHelp.Image = SkinManager.GetImage(@"Dialog\ContentManager\help.png");
            this.btnMapDiagnose.Image = SkinManager.GetImage(@"Dialog\ContentManager\map_diagnose.png");
            this.btnMore.Image = SkinManager.GetImage("nav-more.png");
            foreach (ToolStripItem item in this.msQuickButtons.Items)
            {
                item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                item.Height = this.msQuickButtons.BackgroundImage.Height;
            }
            this.btnOptions.Tag = ContentVaultTabs.Download | ContentVaultTabs.Upload | ContentVaultTabs.Activity | ContentVaultTabs.MyContent;
            this.btnHelp.Tag = ContentVaultTabs.Download | ContentVaultTabs.Upload | ContentVaultTabs.Activity | ContentVaultTabs.MyContent;
            this.btnMapDiagnose.Tag = ContentVaultTabs.Upload;
        }

        public bool IsValidUploadPath(string path)
        {
            foreach (string str in this.InvalidUploadDirectories)
            {
                if (path.StartsWith(str))
                {
                    return false;
                }
            }
            return true;
        }

        private void item_Paint(object sender, PaintEventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item.Image != null)
            {
                e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
                e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(this.msQuickButtons.BackgroundImage.Width, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
                if (item.Enabled)
                {
                    if (item.Selected)
                    {
                        e.Graphics.DrawImage(DrawUtil.AdjustColors(0.8f, 0.8f, 1f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        e.Graphics.DrawImage(item.Image, new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                    }
                }
                else
                {
                    e.Graphics.DrawImage(DrawUtil.GetTransparentImage(0.5f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                }
            }
        }

        private void listBoxUploadDependencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.skinButtonDeleteUploadDependency.Enabled = this.listBoxUploadDependencies.SelectedIndex >= 0;
        }

        private void LoadActivityScreen()
        {
            ActivityMonitor monitor;
            AdditionalContent.BeginUploadContent += new ContentOperationCallback(this.MonitorUploadContent);
            AdditionalContent.BeginDownloadContent += new ContentOperationCallback(this.MonitorDownloadContent);
            int num = 6;
            int num2 = 0;
            foreach (IVaultOperation operation in VaultDownloadOperation.ActiveOperations)
            {
                monitor = operation.CreateActivityMonitor();
                monitor.Left = 0;
                monitor.Top = num2;
                num2 += monitor.Height + num;
                monitor.RemoveMonitor += new EventHandler(this.RemoveDownloadMonitor);
                this.gpgPanelDownActivity.Controls.Add(monitor);
                monitor.Width = this.gpgPanelDownActivity.Width;
                monitor.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                this.ActiveDownloads.Add(monitor);
            }
            num2 = 0;
            foreach (VaultUploadOperation operation2 in VaultUploadOperation.ActiveOperations)
            {
                monitor = operation2.CreateActivityMonitor();
                monitor.Left = 0;
                monitor.Top = num2;
                num2 += monitor.Height + num;
                monitor.RemoveMonitor += new EventHandler(this.RemoveUploadMonitor);
                this.gpgPanelUpActivity.Controls.Add(monitor);
                monitor.Width = this.gpgPanelUpActivity.Width;
                monitor.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                this.ActiveUploads.Add(monitor);
            }
        }

        private void LoadDownloadsScreen()
        {
            AdditionalContent.BeginDownloadContent += new ContentOperationCallback(this.DownloadStarted);
            AdditionalContent.FinishDownloadContent += new ContentOperationCallback(this.DownloadCompleted);
            this.skinButtonSaveSearch.Enabled = AdditionalContent.SaveSearchEnabled;
            this.skinButtonRunSearch.Enabled = AdditionalContent.SearchEnabled;
            this.skinButtonSearchType.Tag = SearchViews.Advanced;
            this.dataGridSearchResults.FinishedStyling += delegate (object s, EventArgs e) {
                this.gcDownload.ToolTip = Loc.Get(this.gcDownload.ToolTip);
                this.gvResults.Appearance.SelectedRow.BackColor = Color.FromArgb(0x33, 0x33, 0x65);
                this.gvResults.Appearance.SelectedRow.BackColor2 = Color.FromArgb(0, 0, 0);
                this.gvResults.Appearance.SelectedRow.GradientMode = LinearGradientMode.Horizontal;
                this.gvResults.Appearance.SelectedRow.Options.UseBackColor = true;
            };
            this.treeViewDownloadType.Nodes.Clear();
            this.gcDownload.ToolTip = Loc.Get(this.gcDownload.ToolTip);
            this.DefaultGridColumns = new GridColumn[this.gvResults.Columns.Count];
            for (int i = 0; i < this.gvResults.Columns.Count; i++)
            {
                this.DefaultGridColumns[i] = this.gvResults.Columns[i];
            }
            ThreadQueue.QueueUserWorkItem(delegate (object s) {
                VGen0 method = null;
                foreach (ContentType type in ContentType.All)
                {
                    VGen0 gen = null;
                    if (type.CurrentUserCanDownload)
                    {
                        TreeNode node = new TreeNode(Loc.Get(type.DisplayName), type.ImageIndex, type.ImageIndex) {
                            Name = type.Name,
                            Tag = type
                        };
                        if (!base.Disposing && !base.IsDisposed)
                        {
                            if (gen == null)
                            {
                                gen = delegate {
                                    this.treeViewDownloadType.Nodes.Add(node);
                                    if (type.Name == ConfigSettings.GetString("DefaultVaultSearchType", "Maps"))
                                    {
                                    }
                                };
                            }
                            base.Invoke(gen);
                        }
                    }
                }
                TreeNode select = null;
                if (Program.Settings.Content.Download.SearchType == null)
                {
                    if (this.treeViewDownloadType.Nodes.Count > 0)
                    {
                        select = this.treeViewDownloadType.Nodes[0];
                    }
                }
                else
                {
                    TreeNode[] nodeArray = this.treeViewDownloadType.Nodes.Find(Program.Settings.Content.Download.SearchType, false);
                    if (nodeArray.Length > 0)
                    {
                        select = nodeArray[0];
                    }
                    else if (this.treeViewDownloadType.Nodes.Count > 0)
                    {
                        select = this.treeViewDownloadType.Nodes[0];
                    }
                }
                if (((select != null) && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.ToggleSearchView();
                            this.treeViewDownloadType.SelectedNode = select;
                        };
                    }
                    base.BeginInvoke(method);
                }
            }, new object[0]);
            this.treeViewSavedSearches.Nodes.Add(new TreeNode("No saved lists", 3, 3));
        }

        private void LoadMyContentScreen()
        {
            AdditionalContent.FinishDeleteMyContent += new ContentOperationCallback(this.AdditionalContent_FinishDelete);
            this.gpgLabelMyContent.Text = Loc.Get("<LOC>No valid content selected. Please select content from the browser on the left.");
            this.RefreshMyContent();
        }

        private void LoadUploadsScreen()
        {
            AdditionalContent.BeginUploadContent += new ContentOperationCallback(this.AdditionalContent_BeginUploadContent);
            AdditionalContent.FinishUploadContent += new ContentOperationCallback(this.AdditionalContent_FinishUploadContent);
            AdditionalContent.BeginDeleteMyUpload += new ContentOperationCallback(this.AdditionalContent_BeginDeleteMyUpload);
            AdditionalContent.FinishDeleteMyUpload += new ContentOperationCallback(this.AdditionalContent_FinishDeleteMyUpload);
            this.skinButtonUpload.Enabled = AdditionalContent.UploadsEnabled;
            this.skinButtonDeleteUploadAll.Enabled = AdditionalContent.DeleteUploadsEnabled;
            this.skinButtonDeleteUploadVersion.Enabled = AdditionalContent.DeleteUploadsEnabled;
            this.pictureBoxRefreshUploads.Left = this.gpgLabelUploadLocations.Right + 9;
            this.gpgLabelNoUploadSelected.Click += new EventHandler(this.ShowUploadVolunteerEffort);
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                this.NodeUploadSearch = new TreeNode(Loc.Get("<LOC>Loading..."), 2, 2);
                this.NodeMyUploads = new TreeNode(Loc.Get("<LOC>My Uploads"), 4, 4);
                this.NodeMyUploads.Name = "MyUploads";
                this.NodeMyUploads.Tag = Loc.Get("<LOC>My Uploads displays the files that you have already uploaded to the vault, and now belong to you to manage and update.");
                this.NodeAvailableUploads = new TreeNode(Loc.Get("<LOC>Available Uploads"), 5, 5);
                this.NodeAvailableUploads.Name = "AvailableUploads";
                this.NodeAvailableUploads.Tag = Loc.Get("<LOC>Available Uploads displays files that the vault has found on your computer that are candidates to be submitted as content. To change the location(s) that the vault searches go to the uploads page of the vault options located on the toolstrip below.");
                if (!base.Disposing && !base.IsDisposed)
                {
                    base.Invoke((VGen0)delegate {
                        this.treeViewUpload.Nodes.Add(this.NodeUploadSearch);
                        this.treeViewUpload.Nodes.Add(this.NodeMyUploads);
                        this.treeViewUpload.Nodes.Add(this.NodeAvailableUploads);
                        this.treeViewUpload.ExpandAll();
                    });
                    foreach (ContentType type in ContentType.All)
                    {
                        foreach (IAdditionalContent content in type.CreateInstance().GetMyUploads())
                        {
                            this.MyUploads.Add(content);
                            this.AddUploadContentNode(this.NodeMyUploads, content);
                        }
                    }
                    this.RefreshUploadContent();
                    Program.Settings.Content.Upload.UploadPathsChanged += new PropertyChangedEventHandler(this.Upload_UploadPathsChanged);
                }
            });
        }

        protected override void Localize()
        {
            base.Localize();
            base.ttDefault.SetToolTip(this.pictureBoxRefreshUploads, Loc.Get("<LOC>Refresh Files"));
            base.ttDefault.SetToolTip(this.skinButtonUploadDependency, Loc.Get("<LOC>Search For Dependency"));
            base.ttDefault.SetToolTip(this.skinButtonDeleteUploadDependency, Loc.Get("<LOC>Remove Dependency"));
        }

        private void MonitorDownloadContent(ContentOperationCallbackArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.MonitorDownloadContent(e);
                    };
                }
                base.Invoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                int num = 6;
                ActivityMonitor monitor = e.ActiveOperation.CreateActivityMonitor();
                monitor.Left = 0;
                monitor.Top = 0;
                monitor.RemoveMonitor += new EventHandler(this.RemoveDownloadMonitor);
                foreach (ActivityMonitor monitor2 in this.ActiveDownloads)
                {
                    monitor2.Top += monitor.Height + num;
                }
                this.gpgPanelDownActivity.Controls.Add(monitor);
                monitor.Width = this.gpgPanelDownActivity.Width;
                monitor.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                this.ActiveDownloads.Add(monitor);
            }
        }

        private void MonitorUploadContent(ContentOperationCallbackArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.MonitorUploadContent(e);
                    };
                }
                base.Invoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                int num = 6;
                ActivityMonitor monitor = e.ActiveOperation.CreateActivityMonitor();
                monitor.Left = 0;
                monitor.Top = 0;
                monitor.RemoveMonitor += new EventHandler(this.RemoveUploadMonitor);
                foreach (ActivityMonitor monitor2 in this.ActiveUploads)
                {
                    monitor2.Top += monitor.Height + num;
                }
                this.gpgPanelUpActivity.Controls.Add(monitor);
                monitor.Width = this.gpgPanelUpActivity.Width;
                monitor.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                this.ActiveUploads.Add(monitor);
            }
        }

        private void Mouse_ButtonUp(object sender, MouseEventArgs e)
        {
            if (!base.Disposing && !base.IsDisposed)
            {
                this.UnhookMouseUpForDownloadButton(this);
                this.DownloadMouseUpHooked = false;
                Point p = (sender as Control).PointToScreen(e.Location);
                p = this.dataGridSearchResults.PointToClient(p);
                if (!this.gvResults.CalcHitInfo(p).InRowCell)
                {
                    this.DlMouseDown = null;
                    this.gvResults.InvalidateRows();
                }
            }
        }

        private void msQuickButtons_Paint(object sender, PaintEventArgs e)
        {
            if (this.ToolstripSizeChanged)
            {
                int num2;
                foreach (ToolStripItem item in this.btnMore.DropDownItems)
                {
                    item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                }
                int count = this.btnMore.DropDown.Items.Count;
                for (num2 = 0; num2 < count; num2++)
                {
                    this.msQuickButtons.Items.Insert(this.msQuickButtons.Items.Count - 1, this.btnMore.DropDown.Items[0]);
                }
                int num3 = 0;
                foreach (ToolStripItem item in this.msQuickButtons.Items)
                {
                    if (!this.mCustomPaint.Contains(item))
                    {
                        this.mCustomPaint.Add(item);
                        item.Paint += new PaintEventHandler(this.item_Paint);
                    }
                    if (item != this.btnMore)
                    {
                        int num4 = ((item.Bounds.Right + this.btnMore.Width) + item.Padding.Right) + this.btnMore.Padding.Horizontal;
                        if (num4 > this.msQuickButtons.Width)
                        {
                            num3++;
                        }
                    }
                }
                if (num3 > 0)
                {
                    this.btnMore.Visible = true;
                    this.btnMore.DropDownDirection = ToolStripDropDownDirection.AboveRight;
                    this.btnMore.DropDown.AutoSize = false;
                    this.btnMore.DropDown.Width = this.msQuickButtons.Items[0].Width;
                    this.btnMore.DropDown.Height = (this.msQuickButtons.Items[0].Height * num3) + 3;
                    this.btnMore.DropDown.Padding = new Padding(0);
                    this.btnMore.DropDown.Margin = new Padding(0);
                    for (num2 = 0; num2 < num3; num2++)
                    {
                        this.btnMore.DropDownItems.Add(this.msQuickButtons.Items[(this.msQuickButtons.Items.Count - 1) - (num3 - num2)]);
                    }
                    foreach (ToolStripItem item in this.btnMore.DropDownItems)
                    {
                        item.BackgroundImage = this.btnMore.DropDown.BackgroundImage;
                    }
                }
                else if (this.ToolstripSizeChanged)
                {
                    this.btnMore.Visible = false;
                }
                this.ToolstripSizeChanged = false;
            }
        }

        private void msQuickButtons_SizeChanged(object sender, EventArgs e)
        {
            this.ToolstripSizeChanged = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.CancelUploadSearch = true;
            base.OnClosing(e);
        }

        private void OnFirstPaint()
        {
            this.splitContainerMyContent.BackColor = Program.Settings.StylePreferences.HighlightColor3;
            this.splitContainerDownload.BackColor = Program.Settings.StylePreferences.HighlightColor3;
            this.splitContainerUpload.BackColor = Program.Settings.StylePreferences.HighlightColor3;
            this.splitContainerActivity.BackColor = Program.Settings.StylePreferences.HighlightColor3;
            this.gpgLabelUploadLocations.ForeColor = Program.Settings.StylePreferences.HighlightColor3;
            this.treeViewDownloadType.BackColor = this.TreeBackground;
            this.treeViewSavedSearches.BackColor = this.TreeBackground;
            this.treeViewUpload.BackColor = this.TreeBackground;
            this.treeViewMyContent.BackColor = this.TreeBackground;
            this.splitContainerMyContent.Panel1.BackColor = this.TreeBackground;
            this.splitContainerDownload.Panel1.BackColor = this.TreeBackground;
            this.splitContainerUpload.Panel1.BackColor = this.TreeBackground;
            this.pictureBoxRefreshUploads.BackColor = this.TreeBackground;
            this.FirstPaint = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.LoadMyContentScreen();
            this.LoadDownloadsScreen();
            this.LoadUploadsScreen();
            this.LoadActivityScreen();
            this.msQuickButtons.Location = new Point(0, 0);
            this.msQuickButtons.Width = base.pbBottom.Width - 0x12;
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            base.pbBottom.Controls.Add(this.msQuickButtons);
            this.RefreshToolstrip();
            this.ChangePanel(this.tabDownload, EventArgs.Empty);
        }

        private void pictureBoxRefreshUploads_Click(object sender, EventArgs e)
        {
            this.RefreshUploadContent();
        }

        public void RefreshContentLists()
        {
            this.treeViewSavedSearches.Nodes.Clear();
            if ((this.SelectedDownloadContent != null) && (this.SelectedDownloadContent.ContentType != null))
            {
                ThreadPool.QueueUserWorkItem(delegate (object s) {
                    using (IEnumerator<ContentList> enumerator = new QuazalQuery("GetContentLists", new object[] { this.SelectedDownloadContent.ContentType.Name }).GetObjects<ContentList>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            ContentList list = enumerator.Current;
                            string[] strArray = list.Category.Split(new char[] { '.' });
                            TreeNodeCollection siblings = this.treeViewSavedSearches.Nodes;
                            TreeNode lastNode = null;
                            foreach (string str in strArray)
                            {
                                TreeNode[] nodeArray = this.treeViewSavedSearches.Nodes.Find(str, false);
                                if (nodeArray.Length > 0)
                                {
                                    lastNode = nodeArray[0];
                                    siblings = lastNode.Nodes;
                                }
                                else
                                {
                                    TreeNode node = new TreeNode(Loc.Get("<LOC>" + str), list.CategoryImageIndex, list.CategoryImageIndex) {
                                        Name = str
                                    };
                                    if (base.Disposing || base.IsDisposed)
                                    {
                                        return;
                                    }
                                    base.Invoke((VGen0)delegate {
                                        siblings.Add(node);
                                        if (node.Parent != null)
                                        {
                                            node.Parent.Expand();
                                        }
                                    });
                                    lastNode = node;
                                    siblings = lastNode.Nodes;
                                }
                            }
                            TreeNode listNode = new TreeNode(Loc.Get("<LOC>" + list.Name), list.ListImageIndex, list.ListImageIndex) {
                                Name = list.Name,
                                Tag = list
                            };
                            if (base.Disposing || base.IsDisposed)
                            {
                                return;
                            }
                            base.Invoke((VGen0)delegate {
                                lastNode.Nodes.Add(listNode);
                                if (listNode.Parent != null)
                                {
                                    listNode.Parent.Expand();
                                }
                                if (!(this.DefaultListPicked || !(list.Name == ConfigSettings.GetString("DefaultVaultSearchList", "Most Downloaded"))))
                                {
                                    this.treeViewSavedSearches.SelectedNode = listNode;
                                    this.DefaultListPicked = true;
                                }
                            });
                        }
                    }
                });
            }
        }

        private void RefreshMyContent()
        {
            VGen0 gen = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (gen == null)
                {
                    gen = delegate {
                        this.treeViewMyContent.Nodes.Clear();
                    };
                }
                base.Invoke(gen);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.treeViewMyContent.Nodes.Clear();
            }
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                VGen0 method = null;
                try
                {
                    AdditionalContent.InvalidateMyContent();
                    foreach (ContentType type in ContentType.All)
                    {
                        if (type.CurrentUserCanDownload)
                        {
                            TreeNode contentRoot = new TreeNode(type.DisplayName, type.ImageIndex, type.ImageIndex);
                            foreach (IAdditionalContent content in this.MyContent)
                            {
                                if (content.TypeID != type.ID)
                                {
                                    continue;
                                }
                                string key = this.TreeNodeName(content);
                                TreeNode[] nodeArray = contentRoot.Nodes.Find(key, false);
                                int imageIndex = type.ImageIndex;
                                if ((nodeArray.Length > 0) && content.CanVersion)
                                {
                                    IAdditionalContent content2;
                                    IAdditionalContent tag = nodeArray[0].Tag as IAdditionalContent;
                                    if (content.Version > tag.Version)
                                    {
                                        nodeArray[0].Tag = content;
                                        content2 = tag;
                                        imageIndex = type.ImageIndex;
                                        if (!AdditionalContent.DownloadExistsLocal(content2))
                                        {
                                            imageIndex = 7;
                                        }
                                        if (nodeArray[0].ImageIndex != imageIndex)
                                        {
                                            nodeArray[0].ImageIndex = imageIndex;
                                            nodeArray[0].SelectedImageIndex = imageIndex;
                                        }
                                    }
                                    else
                                    {
                                        content2 = content;
                                    }
                                    imageIndex = type.ImageIndex;
                                    if (!AdditionalContent.DownloadExistsLocal(content2))
                                    {
                                        imageIndex = 7;
                                    }
                                    TreeNode node = new TreeNode(string.Format(Loc.Get("<LOC>Version {0}"), content2.Version), imageIndex, imageIndex) {
                                        Name = this.TreeNodeName(content2) + "_v" + content2.Version.ToString(),
                                        Tag = content2
                                    };
                                    nodeArray[0].Nodes.Add(node);
                                }
                                else
                                {
                                    imageIndex = type.ImageIndex;
                                    if (!AdditionalContent.DownloadExistsLocal(content))
                                    {
                                        imageIndex = 7;
                                    }
                                    TreeNode node2 = new TreeNode(content.Name, imageIndex, imageIndex) {
                                        Name = this.TreeNodeName(content),
                                        Tag = content
                                    };
                                    contentRoot.Nodes.Add(node2);
                                }
                            }
                            if (base.Disposing || base.IsDisposed)
                            {
                                return;
                            }
                            base.Invoke((VGen0)delegate {
                                this.treeViewMyContent.Nodes.Add(contentRoot);
                                if (!contentRoot.IsExpanded)
                                {
                                    contentRoot.Expand();
                                }
                            });
                        }
                    }
                    if (!base.Disposing && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                if (this.MyContent.Count < 1)
                                {
                                    this.gpgLabelMyContent.Text = Loc.Get("<LOC>You have not downloaded any content yet, click the 'Download' tab to start downloading new content.");
                                }
                                else
                                {
                                    this.gpgLabelMyContent.Text = Loc.Get("<LOC>No valid content selected. Please select content from the browser on the left.");
                                }
                            };
                        }
                        base.Invoke(method);
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        internal void RefreshToolstrip()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RefreshToolstrip();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                switch (this.CurrentTab)
                {
                    case (ContentVaultTabs.None | ContentVaultTabs.MyContent):
                    case (ContentVaultTabs.Download | ContentVaultTabs.MyContent):
                        return;

                    case ContentVaultTabs.Download:
                        this.skinButtonRunSearch.Enabled = !this.Searching;
                        return;

                    case ContentVaultTabs.Upload:
                        if (((AdditionalContent.UploadingContent || (this.SelectedUpload == null)) || (this.SelectedUpload.LocalFilePath == null)) || (this.SelectedUpload.LocalFilePath.Length < 1))
                        {
                            this.skinButtonUpload.Enabled = false;
                        }
                        else if (!((this.SelectedUpload.Version >= this.SelectedUpload.CurrentVersion) && (this.SelectedUploadTreeNode.Parent.Tag is ContentType)))
                        {
                            this.skinButtonUpload.Enabled = false;
                        }
                        else if (!(!this.SelectedUpload.CurrentUserIsOwner || this.SelectedUpload.CanVersion))
                        {
                            this.skinButtonUpload.Enabled = false;
                        }
                        else
                        {
                            this.skinButtonUpload.Enabled = true;
                        }
                        if (((AdditionalContent.DeletingMyUpload || (this.SelectedUploadTreeNode == null)) || (this.SelectedUploadTreeNode.Parent == null)) || ((this.SelectedUploadTreeNode.Parent.Tag is ContentType) && this.SelectedUpload.CanVersion))
                        {
                            this.skinButtonDeleteUploadVersion.Enabled = false;
                        }
                        else
                        {
                            this.skinButtonDeleteUploadVersion.Enabled = true;
                        }
                        this.skinButtonDeleteUploadAll.Enabled = !AdditionalContent.DeletingMyUpload;
                        return;
                }
            }
        }

        public void RefreshUploadContent()
        {
            ThreadPool.QueueUserWorkItem(delegate (object state) {
                VGen0 method = null;
                VGen1 gen2 = null;
                try
                {
                    this.CancelUploadSearch = true;
                    while (this.ScanningForUploads)
                    {
                        Thread.Sleep(20);
                    }
                    this.CancelUploadSearch = false;
                    try
                    {
                        this.ScanningForUploads = true;
                        if (!base.Disposing && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.PreviousUploadStatus = Loc.Get("<LOC>Searching for content...");
                                    if (!AdditionalContent.UploadingContent)
                                    {
                                        this.NodeUploadSearch.Text = this.PreviousUploadStatus;
                                    }
                                };
                            }
                            IAsyncResult result = base.BeginInvoke(method);
                            FileSystemWatcher[] array = new FileSystemWatcher[this.UploadMonitors.Values.Count];
                            this.UploadMonitors.Values.CopyTo(array, 0);
                            foreach (FileSystemWatcher watcher in array)
                            {
                                if (!Program.Settings.Content.Upload.UploadPaths.Contains(watcher.Path))
                                {
                                    this.UploadMonitors.Remove(watcher.Path);
                                }
                            }
                            foreach (string str in Program.Settings.Content.Upload.UploadPaths.ToArray())
                            {
                                if (!Directory.Exists(str))
                                {
                                    Program.Settings.Content.Upload.UploadPaths.Remove(str);
                                }
                            }
                            foreach (string str in Program.Settings.Content.Upload.UploadPaths)
                            {
                                if (!this.UploadMonitors.ContainsKey(str))
                                {
                                    FileSystemWatcher watcher = new FileSystemWatcher(str);
                                    watcher.BeginInit();
                                    watcher.EnableRaisingEvents = true;
                                    watcher.IncludeSubdirectories = true;
                                    watcher.Created += new FileSystemEventHandler(this.UploadFileAdded);
                                    watcher.Deleted += new FileSystemEventHandler(this.UploadFileDeleted);
                                    watcher.Renamed += new RenamedEventHandler(this.UploadFileRenamed);
                                    watcher.Changed += new FileSystemEventHandler(this.UploadFileChanged);
                                    watcher.EndInit();
                                    this.UploadMonitors[str] = watcher;
                                }
                            }
                            foreach (TreeNode node in this.treeViewUpload.Nodes.Find("AvailableUploads", false))
                            {
                                this.CheckExistence(node);
                            }
                            this.CheckCachedLocations();
                            foreach (string str in Program.Settings.Content.Upload.UploadPaths)
                            {
                                this.RefreshUploadContent(str);
                            }
                            if (!this.CancelUploadSearch)
                            {
                                List<TreeNode> list = new List<TreeNode>();
                                foreach (TreeNode node in this.treeViewUpload.Nodes)
                                {
                                    TreeNode[] dest = new TreeNode[node.Nodes.Count];
                                    node.Nodes.CopyTo(dest, 0);
                                    list.AddRange(dest);
                                }
                                int total = 0;
                                foreach (TreeNode node in list)
                                {
                                    if (this.CancelUploadSearch)
                                    {
                                        return;
                                    }
                                    if ((node != this.NodeUploadSearch) && (node.Nodes.Count == 0))
                                    {
                                        if (base.Disposing || base.IsDisposed)
                                        {
                                            return;
                                        }
                                        base.Invoke((VGen1)delegate (object s) {
                                            (s as TreeNode).Remove();
                                        }, new object[] { node });
                                        continue;
                                    }
                                    ContentType tag = node.Tag as ContentType;
                                    if (tag != null)
                                    {
                                        if (base.Disposing || base.IsDisposed)
                                        {
                                            return;
                                        }
                                        if (gen2 == null)
                                        {
                                            gen2 = delegate (object s) {
                                                this.TreeRootCount(s as TreeNode);
                                            };
                                        }
                                        base.Invoke(gen2, new object[] { node });
                                        total += node.Nodes.Count;
                                    }
                                }
                                if ((!this.CancelUploadSearch && (result.IsCompleted || result.AsyncWaitHandle.WaitOne(0x7d0, true))) && ((!this.CancelUploadSearch && !base.Disposing) && !base.IsDisposed))
                                {
                                    base.BeginInvoke((VGen0)delegate {
                                        this.PreviousUploadStatus = Loc.Get(string.Format(Loc.Get("<LOC>Found {0} content files."), total));
                                        if (!AdditionalContent.UploadingContent)
                                        {
                                            this.NodeUploadSearch.Text = this.PreviousUploadStatus;
                                        }
                                    });
                                }
                            }
                        }
                    }
                    finally
                    {
                        this.ScanningForUploads = false;
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    this.ScanningForUploads = false;
                }
            });
        }

        private void RefreshUploadContent(string root)
        {
            if ((!this.CancelUploadSearch && this.IsValidUploadPath(root)) && Directory.Exists(root))
            {
                Exception exception;
                try
                {
                    string[] files = Directory.GetFiles(root);
                    foreach (string str in files)
                    {
                        this.ValidateUploadFile(str);
                    }
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                }
                try
                {
                    string[] directories = Directory.GetDirectories(root);
                    foreach (string str2 in directories)
                    {
                        this.RefreshUploadContent(str2);
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void RefreshUploadDependencies()
        {
            if (this.SelectedUpload != null)
            {
                this.listBoxUploadDependencies.Items.Clear();
                foreach (int num in this.SelectedUpload.ContentDependencies)
                {
                    IAdditionalContent dependency = AdditionalContent.LookupDependency(num);
                    if (dependency == null)
                    {
                        AdditionalContent.RemoveDependency(this.SelectedUpload, num);
                    }
                    else
                    {
                        this.listBoxUploadDependencies.Items.Add(new ContentDependency(dependency));
                    }
                }
                this.skinButtonDeleteUploadDependency.Enabled = this.listBoxUploadDependencies.SelectedIndex >= 0;
            }
        }

        private void ReloadUploadFile(string file)
        {
            foreach (TreeNode node in this.treeViewUpload.Nodes)
            {
                this.ReloadUploadFile(file, node);
            }
        }

        private void ReloadUploadFile(string file, TreeNode node)
        {
            try
            {
                if (((node != null) && (node.Tag != null)) && (node.Tag is IAdditionalContent))
                {
                    VGen0 method = null;
                    IAdditionalContent content = node.Tag as IAdditionalContent;
                    if ((((content.LocalFilePath == file) || (content.LocalFilePath == Path.GetDirectoryName(file))) && content.ContentType.IsValidUploadFile(file)) && content.ContentType.CreateInstance().FromLocalFile(file, out content))
                    {
                        content.MergeWithLocalVersion(content);
                        node.Name = this.TreeNodeName(content);
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    node.Text = content.Name;
                                    if ((this.SelectedUploadTreeNode != null) && (node == this.SelectedUploadTreeNode))
                                    {
                                        this.BindToUploadContent();
                                    }
                                };
                            }
                            base.BeginInvoke(method);
                        }
                        else if (!base.Disposing && !base.IsDisposed)
                        {
                            node.Text = content.Name;
                            if ((this.SelectedUploadTreeNode != null) && (node == this.SelectedUploadTreeNode))
                            {
                                this.BindToUploadContent();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            foreach (TreeNode node2 in node.Nodes)
            {
                this.ReloadUploadFile(file, node2);
            }
        }

        private void RemoveDownloadMonitor(object sender, EventArgs e)
        {
            ActivityMonitor monitor = sender as ActivityMonitor;
            int num = 6;
            int top = monitor.Top;
            this.gpgPanelDownActivity.Controls.Remove(monitor);
            this.ActiveDownloads.Remove(monitor);
            foreach (Control control in this.gpgPanelDownActivity.Controls)
            {
                if (control.Top > top)
                {
                    control.Top -= monitor.Height + num;
                }
            }
            VaultDownloadOperation.ActiveOperations.Remove(monitor.Operation as VaultDownloadOperation);
        }

        private void RemoveUploadMonitor(object sender, EventArgs e)
        {
            ActivityMonitor monitor = sender as ActivityMonitor;
            int num = 6;
            int top = monitor.Top;
            this.gpgPanelUpActivity.Controls.Remove(monitor);
            this.ActiveUploads.Remove(monitor);
            foreach (Control control in this.gpgPanelUpActivity.Controls)
            {
                if (control.Top > top)
                {
                    control.Top -= monitor.Height + num;
                }
            }
            VaultUploadOperation.ActiveOperations.Remove(monitor.Operation as VaultUploadOperation);
        }

        private void SearchCriteriaChanged(object sender, EventArgs e)
        {
            if (this.SelectedDownloadContent != null)
            {
                List<string> list = new List<string>(this.gpgTextBoxSearchName.Text.Split(" ".ToCharArray()));
                list.RemoveAll(delegate (string s) {
                    return s == "";
                });
                int @int = ConfigSettings.GetInt("MaxKeywordSearch", 4);
                if (list.Count > @int)
                {
                    base.Error(this.gpgTextBoxSearchName, "<LOC>Cannot exceed {0} keyword limit.", new object[] { @int });
                }
                else
                {
                    this.SelectedDownloadContent.Name = this.gpgTextBoxSearchName.Text;
                    this.SelectedDownloadContent.OwnerName = this.gpgTextBoxSearchCreator.Text;
                    this.SelectedDownloadContent.SearchKeywords = this.gpgTextBoxSearchKeywords.Text;
                }
            }
        }

        private void SearchCriteriaTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.SelectedContentList = null;
                this.SearchPage = 0;
                this.ExecuteSearch();
            }
        }

        private void SelectUploadPath_Click(object sender, EventArgs e)
        {
            if (new DlgVaultOptions(VaultOptionTabs.Upload).ShowDialog() == DialogResult.OK)
            {
                Program.Settings.Content.Upload.CachedLocations.Clear();
                this.RefreshUploadContent();
            }
        }

        private void ShowUploadVolunteerEffort(object sender, EventArgs e)
        {
            VolunteerEffort effort;
            if (((this.SelectedUpload != null) && !this.SelectedUpload.ContentType.HasVolunteeredForUploads) && new QuazalQuery("GetVolunteerEffortByName", new object[] { this.SelectedUpload.ContentType.UploadVolunteerEffort }).GetObject<VolunteerEffort>(out effort))
            {
                DlgVolunteer volunteer = new DlgVolunteer(effort);
                if (volunteer.ShowDialog() == DialogResult.OK)
                {
                    this.SelectedUpload.ContentType.HasVolunteeredForUploads = true;
                    this.BindToUploadContent();
                }
            }
        }

        private void skinButtonDeleteUploadAll_Click(object sender, EventArgs e)
        {
            if (this.SelectedUpload != null)
            {
                AdditionalContent.DeleteMyUpload(this.SelectedUpload, true);
                this.RefreshToolstrip();
            }
        }

        private void skinButtonDeleteUploadDependency_Click(object sender, EventArgs e)
        {
            if ((this.SelectedUpload != null) && (this.listBoxUploadDependencies.SelectedItem != null))
            {
                ContentDependency selectedItem = (ContentDependency) this.listBoxUploadDependencies.SelectedItem;
                AdditionalContent.RemoveDependency(this.SelectedUpload, selectedItem.ID);
                this.RefreshUploadDependencies();
            }
        }

        private void skinButtonDeleteUploadVersion_Click(object sender, EventArgs e)
        {
            if (this.SelectedUpload != null)
            {
                AdditionalContent.DeleteMyUpload(this.SelectedUpload, false);
                this.RefreshToolstrip();
            }
        }

        private void skinButtonRunSearch_Click(object sender, EventArgs e)
        {
            this.SelectedContentList = null;
            this.SearchPage = 0;
            this.ExecuteSearch();
        }

        private void skinButtonSaveSearch_Click(object sender, EventArgs e)
        {
        }

        private void skinButtonSearchNext_Click(object sender, EventArgs e)
        {
            if ((!this.ViewingVersionHistory && ((this.SearchResults != null) && (this.SearchResults.Length != 0))) && (this.SearchResults.Length >= this.SearchPageSize))
            {
                this.SearchPage++;
                this.ExecuteSearch(this.SearchPage - 1);
            }
        }

        private void skinButtonSearchPrevious_Click(object sender, EventArgs e)
        {
            if ((!this.ViewingVersionHistory && ((this.SearchResults != null) && (this.SearchResults.Length != 0))) && (this.SearchPage > 0))
            {
                this.SearchPage--;
                this.ExecuteSearch(this.SearchPage + 1);
            }
        }

        private void skinButtonSearchStart_Click(object sender, EventArgs e)
        {
            if (!this.ViewingVersionHistory && (this.SearchPage != 0))
            {
                this.SearchPage = 0;
                this.ExecuteSearch();
            }
        }

        private void skinButtonSearchType_Click(object sender, EventArgs e)
        {
            this.ToggleSearchView();
        }

        private void skinButtonUpload_Click(object sender, EventArgs e)
        {
            this.UploadSelected();
        }

        private void skinButtonUploadDependency_Click(object sender, EventArgs e)
        {
            if (this.SelectedUpload != null)
            {
                DlgVaultQuickSearch search = new DlgVaultQuickSearch();
                if ((search.ShowDialog() == DialogResult.OK) && (search.SelectedContent != null))
                {
                    int iD = search.SelectedContent.ID;
                    AdditionalContent.DependencyLookupCache[iD] = search.SelectedContent;
                    AdditionalContent.AddDependency(this.SelectedUpload, iD);
                    this.RefreshUploadDependencies();
                }
            }
        }

        private void ToggleSearchView()
        {
            this.ToggleSearchView((SearchViews) this.skinButtonSearchType.Tag);
        }

        private void ToggleSearchView(SearchViews currentView)
        {
            int height;
            Graphics graphics;
            if (this.SelectedDownloadOptions == null)
            {
                height = 0;
            }
            else
            {
                height = this.SelectedDownloadOptions.Height;
            }
            switch (currentView)
            {
                case SearchViews.Basic:
                    this.gpgLabelSearchType.Text = Loc.Get("<LOC>Basic Search");
                    using (graphics = base.CreateGraphics())
                    {
                        this.gpgLabelSearchType.Left = this.skinButtonSearchType.Left - (((int) DrawUtil.MeasureString(graphics, this.gpgLabelSearchType.Text, this.gpgLabelSearchType.Font).Width) + 6);
                    }
                    if (this.SelectedDownloadOptions != null)
                    {
                        this.SelectedDownloadOptions.Visible = true;
                    }
                    this.gpgGroupBoxCriteria.Height += height;
                    this.gpgGroupBoxResults.Top += height;
                    this.gpgGroupBoxResults.Height -= height;
                    this.skinButtonSearchType.Tag = SearchViews.Advanced;
                    this.CurrentSearchView = SearchViews.Advanced;
                    this.skinButtonSearchType.SkinBasePath = @"Dialog\ContentManager\BtnBasicSearch";
                    break;

                case SearchViews.Advanced:
                    this.gpgLabelSearchType.Text = Loc.Get("<LOC>Advanced Search");
                    using (graphics = base.CreateGraphics())
                    {
                        this.gpgLabelSearchType.Left = this.skinButtonSearchType.Left - (((int) DrawUtil.MeasureString(graphics, this.gpgLabelSearchType.Text, this.gpgLabelSearchType.Font).Width) + 6);
                    }
                    if (this.SelectedDownloadOptions != null)
                    {
                        this.SelectedDownloadOptions.Visible = false;
                    }
                    this.gpgGroupBoxCriteria.Height -= height;
                    this.gpgGroupBoxResults.Top -= height;
                    this.gpgGroupBoxResults.Height += height;
                    this.skinButtonSearchType.Tag = SearchViews.Basic;
                    this.CurrentSearchView = SearchViews.Basic;
                    this.skinButtonSearchType.SkinBasePath = @"Dialog\ContentManager\BtnAdvancedSearch";
                    break;
            }
        }

        private void ToggleToolstripVisibility()
        {
            foreach (ToolStripItem item in this.msQuickButtons.Items)
            {
                if (item != this.btnMore)
                {
                    item.Visible = ((byte) (((ContentVaultTabs) item.Tag) & this.CurrentTab)) != 0;
                }
            }
        }

        private string TreeNodeName(IAdditionalContent content)
        {
            return string.Format("{0}_{1}", content.ContentType.Name, content.Name);
        }

        private void TreeRootCount(TreeNode root)
        {
            if ((root.Tag != null) && (root.Tag is ContentType))
            {
                if (root.Nodes.Count < 1)
                {
                    root.Remove();
                }
                else
                {
                    ContentType tag = root.Tag as ContentType;
                    root.Text = string.Format("{0} ({1})", Loc.Get(tag.DisplayName), root.Nodes.Count);
                }
            }
        }

        private void treeViewDownloadType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.BindToDownloadType();
        }

        private void treeViewMyContent_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.BindToMyContent();
        }

        private void treeViewSavedSearches_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((e.Node != null) && (e.Node.Tag != null)) && (e.Node.Tag is ContentList))
            {
                this.SelectedContentList = e.Node.Tag as ContentList;
                this.SearchPage = 0;
                this.ExecuteSearch();
            }
        }

        private void treeViewUpload_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.BindToUploadContent();
        }

        private void UnhookMouseUpForDownloadButton(Control control)
        {
            control.MouseUp -= new MouseEventHandler(this.Mouse_ButtonUp);
            foreach (Control control2 in control.Controls)
            {
                this.UnhookMouseUpForDownloadButton(control2);
            }
        }

        private void Upload_UploadPathsChanged(object sender, PropertyChangedEventArgs e)
        {
            Program.Settings.Content.Upload.CachedLocations.Clear();
            this.RefreshUploadContent();
        }

        private void UploadFileAdded(object sender, FileSystemEventArgs e)
        {
            this.ValidateUploadFile(e.FullPath);
        }

        private void UploadFileChanged(object sender, FileSystemEventArgs e)
        {
            this.ReloadUploadFile(e.FullPath);
        }

        private void UploadFileDeleted(object sender, FileSystemEventArgs e)
        {
            this.DeleteUploadNode(e.FullPath);
        }

        private void UploadFileRenamed(object sender, RenamedEventArgs e)
        {
            if (this.UploadFileNodes.ContainsKey(e.OldFullPath))
            {
                VGen0 method = null;
                TreeNode node = this.UploadFileNodes[e.OldFullPath];
                if ((node.Tag != null) && (node.Tag is IAdditionalContent))
                {
                    IAdditionalContent tag = node.Tag as IAdditionalContent;
                    tag.LocalFilePath = e.FullPath;
                    this.UploadFileNodes.Remove(e.OldFullPath);
                    this.UploadFileNodes.Add(tag.LocalFilePath, node);
                    if (node.IsSelected)
                    {
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    this.treeViewUpload.SelectedNode = node;
                                };
                            }
                            base.BeginInvoke(method);
                        }
                        else if (!(base.Disposing || base.IsDisposed))
                        {
                            this.treeViewUpload.SelectedNode = node;
                        }
                    }
                }
            }
        }

        private void UploadGeneralOptionsChanged(object sender, EventArgs e)
        {
            if (this.SelectedUpload != null)
            {
                this.SelectedUpload.Description = this.gpgTextBoxUploadDesc.Text;
                this.SelectedUpload.Name = this.gpgTextBoxUploadName.Text;
                this.SelectedUpload.SearchKeywords = this.gpgTextBoxUploadSearchKeywords.Text;
                if (this.SelectedUpload.CurrentUserIsOwner && this.SelectedUpload.CanVersion)
                {
                    this.SelectedUpload.VersionNotes = this.gpgTextAreaVersionNotes.Text;
                }
            }
        }

        private void UploadSelected()
        {
            if (this.SelectedUpload != null)
            {
                AdditionalContent.UploadContent(this.SelectedUpload, this);
            }
        }

        private void UploadStatus(string status, params object[] args)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.UploadStatus(status, args);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.NodeUploadSearch.Text = string.Format(Loc.Get(status), args);
            }
        }

        private void ValidateUploadFile(string file)
        {
            if (this.ValidateUploadFile(file, false))
            {
                Program.Settings.Content.Upload.CachedLocations.Add(file);
            }
        }

        private bool ValidateUploadFile(string file, bool ignoreCache)
        {
            try
            {
                if (this.CancelUploadSearch)
                {
                    return false;
                }
                if (!Path.HasExtension(file))
                {
                    this.RefreshUploadContent(file);
                    return false;
                }
                if (!(ignoreCache || !Program.Settings.Content.Upload.CachedLocations.Contains(file)))
                {
                    return false;
                }
                if (!this.IsValidUploadPath(file))
                {
                    return false;
                }
                if (!File.Exists(file))
                {
                    return false;
                }
                bool flag = false;
                foreach (ContentType type in ContentType.All)
                {
                    IAdditionalContent content;
                    if ((type.CurrentUserCanUpload && type.IsValidUploadFile(file)) && type.CreateInstance().FromLocalFile(file, out content))
                    {
                        TreeNode category = null;
                        List<IAdditionalContent> list = new List<IAdditionalContent>();
                        foreach (IAdditionalContent content2 in this.MyUploads)
                        {
                            if ((content2.TypeID == type.ID) && (content.Name == content2.Name))
                            {
                                if ((content2.Version == content2.CurrentVersion) && ((content2.LocalFilePath == null) || (content2.LocalFilePath.Length < 1)))
                                {
                                    category = this.NodeMyUploads;
                                    content2.MergeWithLocalVersion(content);
                                    TreeNode[] nodes = category.Nodes.Find(this.TreeNodeName(content), true);
                                    if (nodes.Length < 1)
                                    {
                                        continue;
                                    }
                                    nodes[0].Tag = AdditionalContent.Clone(content2);
                                    list.Add(nodes[0].Tag as IAdditionalContent);
                                    if (base.Disposing || base.IsDisposed)
                                    {
                                        return false;
                                    }
                                    base.Invoke((VGen0)delegate {
                                        nodes[0].ImageIndex = type.ImageIndex;
                                        nodes[0].SelectedImageIndex = type.ImageIndex;
                                        foreach (TreeNode node in nodes[0].Nodes)
                                        {
                                            node.ImageIndex = type.ImageIndex;
                                            node.SelectedImageIndex = type.ImageIndex;
                                        }
                                        if (nodes[0].IsSelected)
                                        {
                                            this.BindToUploadContent();
                                        }
                                    });
                                }
                                flag = true;
                            }
                        }
                        foreach (IAdditionalContent content3 in list)
                        {
                            this.MyUploads.Add(content3);
                        }
                        if (category == null)
                        {
                            category = this.NodeAvailableUploads;
                        }
                        this.AddUploadContentNode(category, content);
                        flag = true;
                    }
                }
                return flag;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
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
                return true;
            }
        }

        protected override bool BottomMenuStrip
        {
            get
            {
                return true;
            }
        }

        public string[] InvalidUploadDirectories
        {
            get
            {
                return ConfigSettings.GetString("InvalidUploadDirectories", this.mInvalidUploadDirectories).Split(new char[] { ',' });
            }
        }

        private List<IAdditionalContent> MyContent
        {
            get
            {
                return AdditionalContent.MyContent;
            }
        }

        public int PageCount
        {
            get
            {
                return this.mPageCount;
            }
            set
            {
                this.mPageCount = value;
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return true;
            }
        }

        private int SearchPageSize
        {
            get
            {
                return ConfigSettings.GetInt("VaultSearchPageSize", 10);
            }
        }
    }
}

