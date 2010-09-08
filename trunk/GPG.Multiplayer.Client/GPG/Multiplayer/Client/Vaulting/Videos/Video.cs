namespace GPG.Multiplayer.Client.Vaulting.Videos
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault;
    using GPG.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;

    [Serializable]
    public class Video : AdditionalContent, IAdditionalContent, IContentTypeProvider
    {
        [NonSerialized]
        private Service _WS = new Service();
        [NonSerialized]
        private static GridView BoundGridView = null;
        [FieldMap("length_hours")]
        private int mLengthHours;
        [FieldMap("length_minutes")]
        private int mLengthMinutes;
        [FieldMap("length_seconds")]
        private int mLengthSeconds;
        private string mLocalPreviewImagePath;
        private Image mPreviewImage128 = null;
        private Image mPreviewImage50 = null;
        [FieldMap("quality")]
        private string mQuality;
        [FieldMap("format")]
        private string mVideoFormat;

        [field: NonSerialized]
        public static  event EventHandler PreviewImageLoaded;

        private Video()
        {
        }

        public Video(DataRecord record) : base(record)
        {
        }

        public IAdditionalContent CreateEmptyInstance()
        {
            Video video = new Video();
            video.ContentType = base.ContentType;
            return video;
        }

        public QuazalQuery CreateSearchQuery(int page, int pageSize)
        {
            try
            {
                if (!base.ContentType.CurrentUserCanDownload)
                {
                    return QuazalQuery.Empty;
                }
                string[] collection = new string[0];
                if (base.SearchKeywords != null)
                {
                    collection = base.SearchKeywords.Split(" ".ToCharArray());
                }
                List<string> list = new List<string>(collection);
                list.RemoveAll(delegate (string s) {
                    return s == " ";
                });
                for (int i = list.Count; i < ConfigSettings.GetInt("MaxKeywordSearch", 4); i++)
                {
                    list.Add("");
                }
                ArrayList list2 = new ArrayList(30);
                list2.Add(base.Name);
                list2.Add("%" + base.Name + "%");
                list2.Add(base.OwnerName);
                list2.Add(base.OwnerName);
                foreach (string str in list)
                {
                    list2.Add(str);
                    list2.Add("%" + str + "%");
                }
                list2.Add(this.Quality);
                list2.Add(this.Quality);
                list2.Add(this.VideoFormat);
                list2.Add(this.VideoFormat);
                list2.Add(page * pageSize);
                list2.Add(pageSize);
                return new QuazalQuery(ConfigSettings.GetString("VideoSearch", "SearchVideos"), list2.ToArray());
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return QuazalQuery.Empty;
            }
        }

        public string CreateUploadFile()
        {
            string str3;
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return null;
            }
            string path = Path.GetTempPath() + Guid.NewGuid().ToString();
            string tempFileName = Path.GetTempFileName();
            try
            {
                Directory.CreateDirectory(path);
                List<string> list = new List<string>();
                list.Add(base.LocalFilePath);
                if (this.PreviewImage128 != null)
                {
                    this.PreviewImage128.Save(Path.Combine(path, "preview.png"), ImageFormat.Png);
                    list.Add(Path.Combine(path, "preview.png"));
                }
                Compression.ZipFiles(tempFileName, null, list.ToArray(), true);
                str3 = tempFileName;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                str3 = null;
            }
            finally
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            return str3;
        }

        public QuazalQuery CreateVersionQuery()
        {
            return QuazalQuery.Empty;
        }

        public bool DeleteMyContent(bool allVersions)
        {
            try
            {
                if (Directory.Exists(this.GetDownloadPath()))
                {
                    Directory.Delete(this.GetDownloadPath(), true);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMyUpload(bool allVersions)
        {
            return new QuazalQuery("DeleteVideo", new object[] { base.ID }).ExecuteNonQuery();
        }

        public bool FromID(int contentId, out IAdditionalContent content)
        {
            Video video;
            if (!base.ContentType.CurrentUserCanDownload)
            {
                content = null;
                return false;
            }
            bool flag = new QuazalQuery("GetVideoById", new object[] { contentId }).GetObject<Video>(out video);
            content = video;
            return flag;
        }

        public bool FromLocalFile(string file, out IAdditionalContent content)
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                content = null;
                return false;
            }
            string extension = Path.GetExtension(file);
            base.LocalFilePath = file;
            base.Name = Path.GetFileName(file);
            base.Name = base.Name.Remove(base.Name.Length - extension.Length, extension.Length);
            this.VideoFormat = extension;
            content = this;
            return true;
        }

        public ContentOptions GetDetailsView()
        {
            return new ContentOptions("<LOC>Video Details", new PnlVideoDetailsView(this));
        }

        public string GetDirectURL()
        {
            return "";
        }

        public ContentOptions GetDownloadOptions()
        {
            return new ContentOptions("<LOC>Video Options", new PnlVideoDownloadOptions(this));
        }

        public string GetDownloadPath()
        {
            return AdditionalContent.DefaultDownloadPath(this);
        }

        public IAdditionalContent[] GetMyDownloads()
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return new IAdditionalContent[0];
            }
            QuazalQuery query = new QuazalQuery("GetMyDownloadedVideos", new object[0]);
            return query.GetObjects<Video>().ToArray();
        }

        public IAdditionalContent[] GetMyUploads()
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return new IAdditionalContent[0];
            }
            QuazalQuery query = new QuazalQuery("GetMyUploadedVideos", new object[0]);
            return query.GetObjects<Video>().ToArray();
        }

        public ContentOptions GetUploadOptions()
        {
            return new ContentOptions("<LOC>Video Upload Options", new PnlVideoUploadOptions(this));
        }

        public bool IsContentUnique()
        {
            return !new QuazalQuery("DoesContentExist", new object[] { base.ContentType.ID, base.Name }).GetBool();
        }

        public void MergeWithLocalVersion(IAdditionalContent localVersion)
        {
            Video video = localVersion as Video;
            base.LocalFilePath = localVersion.LocalFilePath;
            this.VideoFormat = video.VideoFormat;
        }

        public void Run()
        {
            IntPtr hwnd = Program.MainForm.Handle;
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                try
                {
                    Process.Start(new ProcessStartInfo(Path.Combine(this.GetDownloadPath(), this.Name + this.VideoFormat)) { ErrorDialog = true, ErrorDialogParentHandle = hwnd, UseShellExecute = true });
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        public bool SaveDownload(IStatusProvider statusProvider)
        {
            Exception exception;
            try
            {
                if (!base.ContentType.CurrentUserCanDownload)
                {
                    return false;
                }
                string downloadPath = this.GetDownloadPath();
                string path = string.Format(@"{0}\content.partial", downloadPath);
                if (!File.Exists(path))
                {
                    ErrorLog.WriteLine("The expected download file: {0} was not found.", new object[] { path });
                    if (statusProvider != null)
                    {
                        statusProvider.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                    }
                    return false;
                }
                statusProvider.SetStatus("<LOC>Extracting File(s)...", new object[0]);
                Compression.Unzip(path);
                File.Delete(path);
                try
                {
                    this.Run();
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                }
                return true;
            }
            catch (Exception exception2)
            {
                exception = exception2;
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public bool SaveDownloadData()
        {
            return new QuazalQuery("UploadVideo", new object[] { base.ID, this.Quality, this.LengthHours, this.LengthMinutes, this.LengthSeconds, this.VideoFormat }).ExecuteNonQuery();
        }

        public void SetGridView(GridView view)
        {
            BoundGridView = view;
            view.Columns["Version"].Visible = false;
            view.Columns["VersionDate"].Visible = false;
            MultiVal<string, string, bool>[] valArray = new MultiVal<string, string, bool>[] { new MultiVal<string, string, bool>("<LOC>Quality", "QualityDisplay", true), new MultiVal<string, string, bool>("<LOC>Video Length", "LengthDisplay", true), new MultiVal<string, string, bool>("<LOC>Video Format", "VideoFormat", true) };
            foreach (MultiVal<string, string, bool> val in valArray)
            {
                GridColumn column = new GridColumn();
                column.Caption = Loc.Get(val.Value1);
                column.FieldName = val.Value2;
                column.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
                column.AppearanceCell.Options.UseTextOptions = true;
                view.Columns.Add(column);
                column.Visible = val.Value3;
            }
            GridColumn column2 = new GridColumn();
            column2.Caption = Loc.Get("<LOC>Preview");
            column2.FieldName = "PreviewImage50";
            column2.ColumnEdit = new RepositoryItemPictureEdit();
            view.Columns.Add(column2);
            column2.Visible = true;
            view.Columns["Download"].VisibleIndex = 0;
            column2.VisibleIndex = 1;
            view.Columns["Name"].VisibleIndex = 2;
            view.Columns["QualityDisplay"].VisibleIndex = 3;
            view.Columns["LengthDisplay"].VisibleIndex = 4;
            view.Columns["VideoFormat"].VisibleIndex = 5;
            view.Columns["OwnerName"].VisibleIndex = 6;
            view.Columns["Downloads"].VisibleIndex = 7;
            view.Columns["RatingImageSmall"].VisibleIndex = 8;
            PreviewImageLoaded -= new EventHandler(this.Video_PreviewImageLoaded);
            PreviewImageLoaded += new EventHandler(this.Video_PreviewImageLoaded);
        }

        private void Video_PreviewImageLoaded(object sender, EventArgs e)
        {
            if (BoundGridView != null)
            {
                if (((BoundGridView.GridControl == null) || BoundGridView.GridControl.Disposing) || BoundGridView.GridControl.IsDisposed)
                {
                    BoundGridView = null;
                }
                else
                {
                    Video video = sender as Video;
                    try
                    {
                        IAdditionalContent[] dataSource = BoundGridView.DataSource as IAdditionalContent[];
                        if ((dataSource != null) && (dataSource.Length >= 1))
                        {
                            if (dataSource[0].TypeID != video.TypeID)
                            {
                                BoundGridView = null;
                            }
                            else
                            {
                                VGen0 method = null;
                                for (int i = 0; i < dataSource.Length; i++)
                                {
                                    if (dataSource[i].ID == video.ID)
                                    {
                                        if ((BoundGridView.GridControl.InvokeRequired && !BoundGridView.GridControl.Disposing) && !BoundGridView.GridControl.IsDisposed)
                                        {
                                            if (method == null)
                                            {
                                                method = delegate {
                                                    BoundGridView.RefreshRow(BoundGridView.GetRowHandle(i));
                                                };
                                            }
                                            BoundGridView.GridControl.BeginInvoke(method);
                                        }
                                        else if (!BoundGridView.GridControl.Disposing && !BoundGridView.GridControl.IsDisposed)
                                        {
                                            BoundGridView.RefreshRow(BoundGridView.GetRowHandle(i));
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }
            }
        }

        private void WS_GetPreviewImageCompleted(object sender, GetPreviewImageCompletedEventArgs e)
        {
            try
            {
                byte[] result = e.Result;
                if ((result != null) && (result.Length > 0))
                {
                    using (MemoryStream stream = new MemoryStream(result))
                    {
                        this.mPreviewImage50 = null;
                        this.mPreviewImage128 = Image.FromStream(stream);
                    }
                    if (PreviewImageLoaded != null)
                    {
                        PreviewImageLoaded(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public bool CanRun
        {
            get
            {
                return true;
            }
        }

        public bool CanVersion
        {
            get
            {
                return false;
            }
        }

        public bool IsDirectHTTPDownload
        {
            get
            {
                return false;
            }
        }

        public string LengthDisplay
        {
            get
            {
                return TimeSpan.FromSeconds((double) (((this.LengthHours * 0xe10) + (this.LengthMinutes * 60)) + this.LengthSeconds)).ToString();
            }
        }

        public int LengthHours
        {
            get
            {
                return this.mLengthHours;
            }
            set
            {
                this.mLengthHours = value;
            }
        }

        public int LengthMinutes
        {
            get
            {
                return this.mLengthMinutes;
            }
            set
            {
                this.mLengthMinutes = value;
            }
        }

        public int LengthSeconds
        {
            get
            {
                return this.mLengthSeconds;
            }
            set
            {
                this.mLengthSeconds = value;
            }
        }

        public string LocalPreviewImagePath
        {
            get
            {
                return this.mLocalPreviewImagePath;
            }
            set
            {
                this.mPreviewImage128 = null;
                this.mPreviewImage50 = null;
                this.mLocalPreviewImagePath = value;
            }
        }

        public Image PreviewImage128
        {
            get
            {
                if (this.mPreviewImage128 == null)
                {
                    if ((this.LocalPreviewImagePath == null) || !File.Exists(this.LocalPreviewImagePath))
                    {
                        if ((((base.TypeID >= 1) && (base.Name != null)) && (base.Name.Length >= 1)) && (base.Version >= 1))
                        {
                            this.WS.GetPreviewImageCompleted += new GetPreviewImageCompletedEventHandler(this.WS_GetPreviewImageCompleted);
                            this.WS.GetPreviewImageAsync(AdditionalContent.VaultServerKey, base.ContentType.Name, base.Name, base.Version, Guid.NewGuid());
                        }
                        return Resources.random;
                    }
                    this.mPreviewImage128 = DrawUtil.ResizeImage(Image.FromFile(this.LocalPreviewImagePath), 0x80, 0x80);
                }
                return this.mPreviewImage128;
            }
        }

        public Image PreviewImage50
        {
            get
            {
                if ((this.mPreviewImage50 == null) && (this.PreviewImage128 != null))
                {
                    this.mPreviewImage50 = DrawUtil.ResizeImage(this.PreviewImage128, 50, 50);
                }
                return this.mPreviewImage50;
            }
        }

        public string Quality
        {
            get
            {
                return this.mQuality;
            }
            set
            {
                this.mQuality = value;
            }
        }

        public string QualityDisplay
        {
            get
            {
                return Loc.Get(this.Quality);
            }
        }

        public string RunImagePath
        {
            get
            {
                return @"Dialog\ContentManager\BtnPlay";
            }
        }

        public string RunTooltip
        {
            get
            {
                return "<LOC>Play Video";
            }
        }

        public string VideoFormat
        {
            get
            {
                return this.mVideoFormat;
            }
            set
            {
                this.mVideoFormat = value;
            }
        }

        private Service WS
        {
            get
            {
                if (this._WS == null)
                {
                    this._WS = new Service();
                }
                return this._WS;
            }
        }
    }
}

