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
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgVaultQuickSearch : DlgBase
    {
        private IContainer components = null;
        private SearchViews CurrentSearchView = SearchViews.Basic;
        private GPGDataGrid dataGridSearchResults;
        private GridColumn[] DefaultGridColumns = null;
        private bool DefaultListPicked = false;
        private IAdditionalContent DlMouseDown = null;
        private bool DownloadMouseUpHooked = false;
        private Font DownloadSizeFont = new Font("Verdana", 7f, FontStyle.Bold);
        private GridColumn gcDate;
        private GridColumn gcDownload;
        private GridColumn gcDownloads;
        private GridColumn gcName;
        private GridColumn gcOwner;
        private GridColumn gcRating;
        private GridColumn gcVersion;
        private SkinGroupPanel gpgGroupBoxCriteria;
        private SkinGroupPanel gpgGroupBoxResults;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel16;
        private GPGLabel gpgLabel21;
        private GPGLabel gpgLabelSearchType;
        private GPGPanel gpgPanel1;
        private GPGPanel gpgPanelSearchCriteria;
        private GPGTextBox gpgTextBoxSearchCreator;
        private GPGTextBox gpgTextBoxSearchKeywords;
        private GPGTextBox gpgTextBoxSearchName;
        private GridView gvResults;
        private ImageList imageListContentTypes;
        private GridHitInfo LastDownloadHit = null;
        private Point LastMouseLoc;
        private IAdditionalContent MouseOverDownload = null;
        private int mPageCount;
        private IAdditionalContent mSelectedContent = null;
        private RepositoryItemHyperLinkEdit repositoryItemChatLink;
        private RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private RepositoryItemPictureEdit repositoryItemRatingStars;
        private RepositoryItemHyperLinkEdit repositoryItemVersionLink;
        private GridColumn[] ResultColumns = null;
        private bool Searching = false;
        private int SearchPage = 0;
        private IAdditionalContent[] SearchResults = null;
        private ContentList SelectedContentList = null;
        private IAdditionalContent SelectedDownloadContent = null;
        private Control SelectedDownloadOptions = null;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private SkinButton skinButtonRunSearch;
        private SkinButton skinButtonSearchNext;
        private SkinButton skinButtonSearchPrevious;
        private SkinButton skinButtonSearchStart;
        private SkinButton skinButtonSearchType;
        private SkinLabel skinLabel1;
        private SkinLabel skinLabel2;
        private SkinLabel skinLabelSearchPage;
        private SplitContainer splitContainerDownload;
        private TreeView treeViewDownloadType;
        private TreeView treeViewSavedSearches;
        private bool ViewingVersionHistory = false;

        public DlgVaultQuickSearch()
        {
            this.InitializeComponent();
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
            }
        }

        private void dataGridSearchResults_DoubleClick(object sender, EventArgs e)
        {
            if (this.MouseOverDownload != null)
            {
                if (this.mSelectedContent == null)
                {
                    this.mSelectedContent = this.MouseOverDownload;
                }
                base.DialogResult = DialogResult.OK;
                base.Close();
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
                        base.SetStatus("Searching...", new object[0]);
                        this.ViewingVersionHistory = false;
                        ThreadPool.QueueUserWorkItem(delegate (object s) {
                            VGen0 method = null;
                            try
                            {
                                DataList data = query.GetData();
                                if ((data.Count == 0) && (revertPage >= 0))
                                {
                                    this.SearchPage = revertPage;
                                    this.ClearStatus();
                                    this.Searching = false;
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
                                    if ((this.IsHandleCreated && !this.Disposing) && !this.IsDisposed)
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
                                            };
                                        }
                                        this.BeginInvoke(method);
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgVaultQuickSearch));
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
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
            this.gpgPanel1 = new GPGPanel();
            this.imageListContentTypes = new ImageList(this.components);
            ((ISupportInitialize) base.pbBottom).BeginInit();
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
            this.gpgPanel1.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x374, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Transparent;
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
            this.skinButtonOK.Location = new Point(0x2a3, 0x288);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x7d, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 0x11;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
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
            this.skinButtonCancel.Location = new Point(0x326, 0x288);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x7d, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 0x12;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.splitContainerDownload.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerDownload.BackColor = Color.FromArgb(0xcc, 0xcc, 0xff);
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
            this.splitContainerDownload.Size = new Size(920, 0x22f);
            this.splitContainerDownload.SplitterDistance = 0xcf;
            this.splitContainerDownload.SplitterWidth = 2;
            base.ttDefault.SetSuperTip(this.splitContainerDownload, null);
            this.splitContainerDownload.TabIndex = 0x13;
            this.treeViewDownloadType.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.treeViewDownloadType.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewDownloadType.BorderStyle = BorderStyle.None;
            this.treeViewDownloadType.HideSelection = false;
            this.treeViewDownloadType.Location = new Point(0, 0x1a);
            this.treeViewDownloadType.Name = "treeViewDownloadType";
            this.treeViewDownloadType.Size = new Size(0xcf, 0x80);
            base.ttDefault.SetSuperTip(this.treeViewDownloadType, null);
            this.treeViewDownloadType.TabIndex = 0;
            this.treeViewDownloadType.AfterSelect += new TreeViewEventHandler(this.treeViewDownloadType_AfterSelect);
            this.treeViewSavedSearches.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeViewSavedSearches.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewSavedSearches.BorderStyle = BorderStyle.None;
            this.treeViewSavedSearches.HideSelection = false;
            this.treeViewSavedSearches.Location = new Point(0, 180);
            this.treeViewSavedSearches.Name = "treeViewSavedSearches";
            this.treeViewSavedSearches.Size = new Size(0xcf, 0x17b);
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
            this.skinLabel1.Size = new Size(0xcf, 20);
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
            this.skinLabel2.Size = new Size(0xcf, 20);
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
            this.gpgGroupBoxResults.Size = new Size(700, 0x1ad);
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
            this.skinButtonSearchNext.Location = new Point(0x290, 0x193);
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
            this.skinLabelSearchPage.Location = new Point(0x54, 0x193);
            this.skinLabelSearchPage.Name = "skinLabelSearchPage";
            this.skinLabelSearchPage.Size = new Size(0x23c, 0x16);
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
            this.dataGridSearchResults.Size = new Size(0x2b4, 0x179);
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
            this.skinButtonSearchStart.Location = new Point(4, 0x193);
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
            this.skinButtonSearchPrevious.Location = new Point(0x2c, 0x193);
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
            this.gpgGroupBoxCriteria.Controls.Add(this.skinButtonRunSearch);
            this.gpgGroupBoxCriteria.Controls.Add(this.gpgPanelSearchCriteria);
            this.gpgGroupBoxCriteria.CutCorner = false;
            this.gpgGroupBoxCriteria.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgGroupBoxCriteria.HeaderImage = GroupPanelImages.blue_gradient;
            this.gpgGroupBoxCriteria.IsStyled = true;
            this.gpgGroupBoxCriteria.Location = new Point(4, 5);
            this.gpgGroupBoxCriteria.Margin = new Padding(4, 3, 4, 3);
            this.gpgGroupBoxCriteria.Name = "gpgGroupBoxCriteria";
            this.gpgGroupBoxCriteria.Size = new Size(700, 0x72);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxCriteria, null);
            this.gpgGroupBoxCriteria.TabIndex = 15;
            this.gpgGroupBoxCriteria.Text = "<LOC>Search Criteria";
            this.gpgGroupBoxCriteria.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgGroupBoxCriteria.TextPadding = new Padding(8, 0, 0, 0);
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
            this.skinButtonRunSearch.Location = new Point(0x238, 0x51);
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
            this.gpgPanelSearchCriteria.Size = new Size(0x2b3, 0x34);
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
            this.gpgPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanel1.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel1.BorderThickness = 2;
            this.gpgPanel1.Controls.Add(this.splitContainerDownload);
            this.gpgPanel1.DrawBorder = true;
            this.gpgPanel1.Location = new Point(10, 0x42);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(0x39c, 0x233);
            base.ttDefault.SetSuperTip(this.gpgPanel1, null);
            this.gpgPanel1.TabIndex = 20;
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
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x3af, 0x2d4);
            base.Controls.Add(this.gpgPanel1);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgVaultQuickSearch";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "DlgVaultQuickSearch";
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgPanel1, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
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
            this.gpgPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LoadDownloadsScreen()
        {
            AdditionalContent.BeginDownloadContent += new ContentOperationCallback(this.DownloadStarted);
            AdditionalContent.FinishDownloadContent += new ContentOperationCallback(this.DownloadCompleted);
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.LoadDownloadsScreen();
        }

        public void RefreshContentLists()
        {
            this.treeViewSavedSearches.Nodes.Clear();
            if ((this.SelectedDownloadContent != null) && (this.SelectedDownloadContent.ContentType != null))
            {
                ThreadPool.QueueUserWorkItem(delegate (object s) {
                    try
                    {
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
                                        if ((!base.IsHandleCreated || base.Disposing) || base.IsDisposed)
                                        {
                                            return;
                                        }
                                        base.Invoke(delegate {
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
                                if ((!base.IsHandleCreated || base.Disposing) || base.IsDisposed)
                                {
                                    return;
                                }
                                base.Invoke(delegate {
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
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                });
            }
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

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            int[] selectedRows = this.gvResults.GetSelectedRows();
            if ((selectedRows != null) && (selectedRows.Length > 0))
            {
                this.mSelectedContent = (IAdditionalContent) this.gvResults.GetRow(selectedRows[0]);
            }
            base.DialogResult = DialogResult.OK;
            base.Close();
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

        private void treeViewDownloadType_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.BindToDownloadType();
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

        private void UnhookMouseUpForDownloadButton(Control control)
        {
            control.MouseUp -= new MouseEventHandler(this.Mouse_ButtonUp);
            foreach (Control control2 in control.Controls)
            {
                this.UnhookMouseUpForDownloadButton(control2);
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

        private int SearchPageSize
        {
            get
            {
                return ConfigSettings.GetInt("VaultSearchPageSize", 10);
            }
        }

        public IAdditionalContent SelectedContent
        {
            get
            {
                return this.mSelectedContent;
            }
        }
    }
}

