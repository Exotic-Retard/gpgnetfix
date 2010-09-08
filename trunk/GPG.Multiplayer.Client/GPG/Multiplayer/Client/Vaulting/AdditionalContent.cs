namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Volunteering;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class AdditionalContent : MappedObject
    {
        internal static Dictionary<int, IAdditionalContent> DependencyLookupCache = new Dictionary<int, IAdditionalContent>();
        private static bool mCheckingForUpdates;
        [FieldMap("dependencies")]
        internal string mContentDependencies;
        private GPG.Multiplayer.Client.Vaulting.ContentType mContentType;
        [FieldMap("current_version")]
        private int mCurrentVersion;
        private static bool mDeletingMyContent = false;
        private static bool mDeletingMyUpload = false;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("download_acl")]
        private int mDownloadACL;
        [FieldMap("downloads")]
        private int mDownloads;
        [FieldMap("download_size")]
        private int mDownloadSize;
        private static IAdditionalContent mDownloadTarget = null;
        [FieldMap("dl_volunteer_effort")]
        private string mDownloadVolunteerEffort;
        [FieldMap("enabled")]
        private bool mEnabled;
        private bool? mHasVolunteeredForDownload;
        [FieldMap("content_id")]
        private int mID;
        private string mLocalFilePath;
        private static List<IAdditionalContent> mMyContent = null;
        [FieldMap("name")]
        private string mName;
        [FieldMap("owner_id")]
        private int mOwnerID;
        [FieldMap("owner_name")]
        private string mOwnerName;
        private Image mRatimgImageFull;
        private Image mRatimgImageSmall;
        [FieldMap("rating_count")]
        private int mRatingCount;
        private Image mRatingImageLarge;
        [FieldMap("rating_total")]
        private float mRatingTotal;
        [FieldMap("search_keywords")]
        private string mSearchKeywords;
        [FieldMap("type_id")]
        private int mTypeID;
        private static bool mUploadingContent = false;
        [FieldMap("version")]
        private int mVersion;
        [FieldMap("version_chksum")]
        private string mVersionCheck;
        [FieldMap("version_date")]
        private DateTime mVersionDate;
        [FieldMap("version_notes")]
        private string mVersionNotes;
        private static object MyContentMutex = new object();
        private static Service WebService = new Service();

        public static  event ContentOperationCallback BeginCheckForUpdates;

        public static  event ContentOperationCallback BeginDeleteMyContent;

        public static  event ContentOperationCallback BeginDeleteMyUpload;

        public static  event ContentOperationCallback BeginDownloadContent;

        public static  event ContentOperationCallback BeginUploadContent;

        public static  event ContentOperationCallback FinishCheckForUpdates;

        public static  event ContentOperationCallback FinishDeleteMyContent;

        public static  event ContentOperationCallback FinishDeleteMyUpload;

        public static  event ContentOperationCallback FinishDownloadContent;

        public static  event ContentOperationCallback FinishUploadContent;

        protected AdditionalContent()
        {
            this.mID = 0;
            this.mTypeID = 0;
            this.mName = null;
            this.mDescription = null;
            this.mOwnerName = null;
            this.mOwnerID = 0;
            this.mDownloads = 0;
            this.mSearchKeywords = null;
            this.mRatingTotal = 0f;
            this.mRatingCount = 0;
            this.mEnabled = true;
            this.mDownloadACL = 0;
            this.mHasVolunteeredForDownload = null;
            this.mRatimgImageSmall = null;
            this.mRatingImageLarge = null;
            this.mRatimgImageFull = null;
            this.mVersion = 0;
            this.mVersionDate = DateTime.MinValue;
            this.mVersionCheck = null;
            this.mCurrentVersion = -1;
            this.mContentDependencies = null;
            this.mContentType = null;
            this.mLocalFilePath = null;
        }

        public AdditionalContent(DataRecord record) : base(record)
        {
            this.mID = 0;
            this.mTypeID = 0;
            this.mName = null;
            this.mDescription = null;
            this.mOwnerName = null;
            this.mOwnerID = 0;
            this.mDownloads = 0;
            this.mSearchKeywords = null;
            this.mRatingTotal = 0f;
            this.mRatingCount = 0;
            this.mEnabled = true;
            this.mDownloadACL = 0;
            this.mHasVolunteeredForDownload = null;
            this.mRatimgImageSmall = null;
            this.mRatingImageLarge = null;
            this.mRatimgImageFull = null;
            this.mVersion = 0;
            this.mVersionDate = DateTime.MinValue;
            this.mVersionCheck = null;
            this.mCurrentVersion = -1;
            this.mContentDependencies = null;
            this.mContentType = null;
            this.mLocalFilePath = null;
        }

        public static void AddDependency(IAdditionalContent content, int dependency)
        {
            if (!(content is AdditionalContent))
            {
                throw new Exception(string.Format("Content type {0} does not inherit from AdditionalContent base class.", content.GetType()));
            }
            string mContentDependencies = (content as AdditionalContent).mContentDependencies;
            if ((mContentDependencies == null) || (mContentDependencies.Length < 1))
            {
                mContentDependencies = dependency.ToString();
            }
            else
            {
                mContentDependencies = mContentDependencies + "," + dependency.ToString();
            }
            (content as AdditionalContent).mContentDependencies = mContentDependencies;
        }

        private static void AdditionalContent_FinishDownloadContent(ContentOperationCallbackArgs e)
        {
            FinishDownloadContent = (ContentOperationCallback) Delegate.Remove(FinishDownloadContent, new ContentOperationCallback(AdditionalContent.AdditionalContent_FinishDownloadContent));
            e.ActiveOperation.SetStatus("<LOC>Update complete.", 0xbb8, new object[0]);
        }

        public static void CheckForUpdates(IAdditionalContent content, IStatusProvider statusProvider)
        {
            if ((!CheckingforUpdates && DownloadsEnabled) && !IsDownloadingContent(content))
            {
                ThreadPool.QueueUserWorkItem(delegate (object s) {
                    try
                    {
                        mCheckingForUpdates = true;
                        statusProvider.SetStatus("<LOC>Checking for updates...", new object[0]);
                        if (BeginCheckForUpdates != null)
                        {
                            BeginCheckForUpdates(new ContentOperationCallbackArgs(content, null));
                        }
                        DataList data = new QuazalQuery("GetLatestContentVersion", new object[] { content.Name, content.TypeID }).GetData();
                        if (data.Count < 1)
                        {
                            statusProvider.SetStatus("<LOC>No updates found.", 0xbb8, new object[0]);
                            mCheckingForUpdates = false;
                            if (FinishCheckForUpdates != null)
                            {
                                FinishCheckForUpdates(new ContentOperationCallbackArgs(content, null, false, new object[0]));
                            }
                        }
                        else
                        {
                            int contentId = int.Parse(data[0]["content_id"]);
                            int num2 = int.Parse(data[0]["version"]);
                            if ((int.Parse(data[0]["has_current"]) > 0) || (content.Version == num2))
                            {
                                statusProvider.SetStatus("<LOC>{0} is up to date.", 0xbb8, new object[] { content.Name });
                                mCheckingForUpdates = false;
                                if (FinishCheckForUpdates != null)
                                {
                                    FinishCheckForUpdates(new ContentOperationCallbackArgs(content, null, true, new object[0]));
                                }
                            }
                            else
                            {
                                statusProvider.SetStatus("<LOC>Update found, preparing to download...", new object[0]);
                                FinishDownloadContent = (ContentOperationCallback) Delegate.Combine(FinishDownloadContent, new ContentOperationCallback(AdditionalContent.AdditionalContent_FinishDownloadContent));
                                DownloadUpdate(contentId, statusProvider);
                                mCheckingForUpdates = false;
                                if (FinishCheckForUpdates != null)
                                {
                                    FinishCheckForUpdates(new ContentOperationCallbackArgs(content, null, true, new object[0]));
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                        statusProvider.SetStatus("<LOC>An error occurred while checking for updates, please try again.", 0xbb8, new object[0]);
                        mCheckingForUpdates = false;
                        if (FinishCheckForUpdates != null)
                        {
                            FinishCheckForUpdates(new ContentOperationCallbackArgs(content, null, true, new object[0]));
                        }
                    }
                });
            }
        }

        public static IAdditionalContent Clone(IAdditionalContent master)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, master);
                stream.Position = 0L;
                return (new BinaryFormatter().Deserialize(stream) as IAdditionalContent);
            }
        }

        public static string DefaultDownloadPath(IAdditionalContent content)
        {
            if (content == null)
            {
                return null;
            }
            string str = content.Version.ToString();
            str = "v" + str.PadLeft(4, '0');
            return string.Format(@"{0}\{1}.{2}", content.ContentType.DownloadPath, content.Name, str);
        }

        public static void DeleteMyContent(IAdditionalContent content)
        {
            DeleteMyContent(content, false);
        }

        public static void DeleteMyContent(IAdditionalContent content, bool allVersions)
        {
            DeleteMyContent(content, allVersions, true, false);
        }

        public static void DeleteMyContent(IAdditionalContent content, bool allVersions, bool deleteLocal, bool suppressRefresh)
        {
            mDeletingMyContent = true;
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                try
                {
                    QuazalQuery query;
                    if (BeginDeleteMyContent != null)
                    {
                        BeginDeleteMyContent(new ContentOperationCallbackArgs(content, null));
                    }
                    if (deleteLocal)
                    {
                        DateTime now = DateTime.Now;
                        while (!content.DeleteMyContent(allVersions))
                        {
                            if ((DateTime.Now - now) > TimeSpan.FromSeconds(3.0))
                            {
                                break;
                            }
                            Thread.Sleep(50);
                        }
                        if (Directory.Exists(content.GetDownloadPath()))
                        {
                            DlgMessage.Show(Program.MainForm, "<LOC>Error", string.Format("<LOC>Unable to delete directory {0}, ensure it's files are not in use elsewhere.", content.GetDownloadPath()));
                            if (FinishDeleteMyContent != null)
                            {
                                FinishDeleteMyContent(new ContentOperationCallbackArgs(content, null, false, new object[] { allVersions }));
                            }
                            return;
                        }
                    }
                    if (allVersions)
                    {
                        query = new QuazalQuery("DeleteDownloadedContentVersions", new object[] { content.TypeID, content.Name });
                    }
                    else
                    {
                        query = new QuazalQuery("DeleteDownloadedContent", new object[] { content.ID });
                    }
                    bool success = query.ExecuteNonQuery();
                    mDeletingMyContent = false;
                    if (!(suppressRefresh || (FinishDeleteMyContent == null)))
                    {
                        FinishDeleteMyContent(new ContentOperationCallbackArgs(content, null, success, new object[] { allVersions }));
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    mDeletingMyContent = false;
                    if (FinishDeleteMyContent != null)
                    {
                        FinishDeleteMyContent(new ContentOperationCallbackArgs(content, null, false, new object[] { allVersions }));
                    }
                }
            });
        }

        public static void DeleteMyUpload(IAdditionalContent content)
        {
            DeleteMyUpload(content, false);
        }

        public static void DeleteMyUpload(IAdditionalContent content, bool allVersions)
        {
            mDeletingMyUpload = true;
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                try
                {
                    if (BeginDeleteMyUpload != null)
                    {
                        BeginDeleteMyUpload(new ContentOperationCallbackArgs(content, null));
                    }
                    WebService.DeleteContentCompleted += new DeleteContentCompletedEventHandler(AdditionalContent.WebService_DeleteContentCompleted);
                    WebService.DeleteContentAsync(VaultServerKey, content.ContentType.Name, content.Name, content.Version, allVersions, new object[] { content, allVersions });
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    mDeletingMyUpload = false;
                    if (FinishDeleteMyUpload != null)
                    {
                        FinishDeleteMyUpload(new ContentOperationCallbackArgs(content, null, false, new object[] { allVersions }));
                    }
                }
            });
        }

        public static void DownloadContent(IAdditionalContent content)
        {
            DownloadContent(content, null);
        }

        public static void DownloadContent(int contentId)
        {
            DownloadContent(contentId, null);
        }

        public static void DownloadContent(IAdditionalContent content, IStatusProvider statusForm)
        {
            if (DownloadsEnabled && !IsDownloadingContent(content))
            {
                if (!content.CurrentUserCanDownload)
                {
                    if (statusForm != null)
                    {
                        statusForm.SetStatus("<LOC>You do not have permission to download this content.", 0xbb8, new object[0]);
                    }
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(delegate (object s) {
                        int @int = ConfigSettings.GetInt("MaxDailyVaultDownload", 5);
                        if (!User.Current.IsAdmin && (DailyDownloadCount >= @int))
                        {
                            string message = string.Format(Loc.Get("<LOC>You have reached the maximum number of downloads allowed per day. You are allowed up to {0} downloads in any 24 hour period."), @int);
                            if (statusForm == null)
                            {
                                DlgMessage.ShowDialog(message);
                            }
                            else
                            {
                                statusForm.SetStatus(message, 0x1388, new object[0]);
                            }
                        }
                        else
                        {
                            IVaultOperation operation;
                            if (!content.HasVolunteeredForDownload)
                            {
                                if (DlgMessage.ShowDialog("<LOC>To download this file you must first agree to its legal conditions by volunteering to download it. Click OK to continue to the volunteer form.") != DialogResult.OK)
                                {
                                    return;
                                }
                                OGen0 method = null;
                                VolunteerEffort effort;
                                if (new QuazalQuery("GetVolunteerEffortByName", new object[] { content.DownloadVolunteerEffort }).GetObject<VolunteerEffort>(out effort))
                                {
                                    if (method == null)
                                    {
                                        method = delegate {
                                            if (new DlgVolunteer(effort).ShowDialog() == DialogResult.OK)
                                            {
                                                content.HasVolunteeredForDownload = true;
                                                return true;
                                            }
                                            return false;
                                        };
                                    }
                                    if (!((bool) Program.MainForm.Invoke(method)))
                                    {
                                        return;
                                    }
                                }
                            }
                            mDownloadTarget = content;
                            if ((content.ContentDependencies != null) && (content.ContentDependencies.Length > 0))
                            {
                                List<int> missing = new List<int>();
                                Dictionary<int, IAdditionalContent> dictionary = new Dictionary<int, IAdditionalContent>(MyContent.Count);
                                foreach (IAdditionalContent content1 in MyContent)
                                {
                                    dictionary[content1.ID] = content1;
                                }
                                foreach (int num2 in content.ContentDependencies)
                                {
                                    if (!dictionary.ContainsKey(num2))
                                    {
                                        missing.Add(num2);
                                    }
                                }
                                if (missing.Count > 0)
                                {
                                    DialogResult result = DialogResult.No;
                                    Program.MainForm.Invoke((VGen0)delegate {
                                        result = new DlgYesNoCancel("<LOC>Content Dependency", string.Format(Loc.Get("<LOC>The requested {0} is dependent on {1} additional vaulted download(s) that you do not have and may not operate properly without them. GPGnet can download these for you now, or you can manually download them later. Do you want GPGnet to download these dependencies now?"), content.ContentType.SingularDisplayName.ToLower(), missing.Count)).ShowDialog();
                                    });
                                    switch (result)
                                    {
                                        case DialogResult.Yes:
                                            foreach (int num3 in missing)
                                            {
                                                DownloadContent(num3, statusForm);
                                            }
                                            break;

                                        case DialogResult.Cancel:
                                            return;
                                    }
                                }
                            }
                            if (!content.IsDirectHTTPDownload)
                            {
                                operation = new VaultDownloadOperation(content);
                            }
                            else
                            {
                                operation = new VaultDirectDownloadOperation(content);
                            }
                            if (BeginDownloadContent != null)
                            {
                                BeginDownloadContent(new ContentOperationCallbackArgs(content, operation));
                            }
                            operation.OperationFinished += new ContentOperationCallback(AdditionalContent.DownloadOperation_Finished);
                            operation.Start();
                        }
                    });
                }
            }
        }

        public static void DownloadContent(int contentId, IStatusProvider statusForm)
        {
            if (DownloadsEnabled && !IsDownloadingContent(contentId))
            {
                if (statusForm != null)
                {
                    statusForm.SetStatus(Loc.Get("<LOC>Locating content..."), new object[0]);
                }
                ThreadPool.QueueUserWorkItem(delegate (object s) {
                    int id = new QuazalQuery("GetContentTypeById", new object[] { contentId }).GetInt();
                    if (id < 0)
                    {
                        if (statusForm != null)
                        {
                            statusForm.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                        }
                    }
                    else
                    {
                        IAdditionalContent content;
                        if (!GPG.Multiplayer.Client.Vaulting.ContentType.FromID(id).CreateInstance().FromID(contentId, out content))
                        {
                            if (statusForm != null)
                            {
                                statusForm.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                            }
                        }
                        else
                        {
                            DownloadContent(content, statusForm);
                        }
                    }
                });
            }
        }

        public static bool DownloadExistsLocal(IAdditionalContent content)
        {
            string downloadPath = content.GetDownloadPath();
            if (File.Exists(downloadPath))
            {
                return true;
            }
            if (Directory.Exists(downloadPath))
            {
                string[] files = Directory.GetFiles(downloadPath);
                if ((files.Length == 0) && (Directory.GetDirectories(downloadPath).Length > 0))
                {
                    return true;
                }
                if (files.Length > 1)
                {
                    return true;
                }
                if (files.Length == 1)
                {
                    if (files[0].EndsWith(".partial"))
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        private static void DownloadOperation_Finished(ContentOperationCallbackArgs e)
        {
            mDownloadTarget = null;
            if (FinishDownloadContent != null)
            {
                FinishDownloadContent(e);
            }
        }

        public static void DownloadUpdate(int contentId, IStatusProvider statusForm)
        {
            if (DownloadsEnabled && !IsDownloadingContent(contentId))
            {
                if (statusForm != null)
                {
                    statusForm.SetStatus(Loc.Get("<LOC>Locating content..."), new object[0]);
                }
                int @int = new QuazalQuery("GetContentTypeById", new object[] { contentId }).GetInt();
                if (@int < 0)
                {
                    if (statusForm != null)
                    {
                        statusForm.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                    }
                }
                else
                {
                    IAdditionalContent content;
                    if (!GPG.Multiplayer.Client.Vaulting.ContentType.FromID(@int).CreateInstance().FromID(contentId, out content))
                    {
                        if (statusForm != null)
                        {
                            statusForm.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                        }
                    }
                    else
                    {
                        DownloadContent(content, statusForm);
                    }
                }
            }
        }

        public static IAdditionalContent GetByID(int contentId)
        {
            IAdditionalContent content;
            if (contentId <= 0)
            {
                return null;
            }
            int @int = new QuazalQuery("GetContentTypeById", new object[] { contentId }).GetInt();
            if (@int < 0)
            {
                return null;
            }
            GPG.Multiplayer.Client.Vaulting.ContentType.FromID(@int).CreateInstance().FromID(contentId, out content);
            return content;
        }

        public static bool GetByID(int contentId, out IAdditionalContent content)
        {
            content = GetByID(contentId);
            return (content != null);
        }

        public static void InvalidateMyContent()
        {
            mMyContent = null;
        }

        public static bool IsDownloadingContent(IAdditionalContent content)
        {
            return IsDownloadingContent(content.ID);
        }

        public static bool IsDownloadingContent(int contentId)
        {
            foreach (IVaultOperation operation in VaultDownloadOperation.ActiveOperations)
            {
                VaultDownloadOperation operation2 = operation as VaultDownloadOperation;
                if ((operation2 != null) && !((operation2.Content.ID != contentId) || operation2.IsOperationFinished))
                {
                    return true;
                }
            }
            return false;
        }

        public static IAdditionalContent LookupDependency(int contentId)
        {
            if (!DependencyLookupCache.ContainsKey(contentId))
            {
                IAdditionalContent content;
                if (!GetByID(contentId, out content))
                {
                    return null;
                }
                DependencyLookupCache[contentId] = content;
            }
            return DependencyLookupCache[contentId];
        }

        public static bool MyContentExists(IAdditionalContent content)
        {
            foreach (IAdditionalContent content2 in MyContent)
            {
                if (content2.ID == content.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public static void RemoveDependency(IAdditionalContent content, int dependency)
        {
            if (!(content is AdditionalContent))
            {
                throw new Exception(string.Format("Content type {0} does not inherit from AdditionalContent base class.", content.GetType()));
            }
            string str = (content as AdditionalContent).mContentDependencies.Replace(dependency.ToString(), "").Replace(",,", ",");
            (content as AdditionalContent).mContentDependencies = str;
        }

        public static void UploadContent(IAdditionalContent content)
        {
            UploadContent(content, null);
        }

        public static void UploadContent(IAdditionalContent content, IStatusProvider initialForm)
        {
            if (!content.CurrentUserIsOwner && !content.IsContentUnique())
            {
                if (initialForm != null)
                {
                    initialForm.SetStatus("<LOC>Unable to upload, a {0} by this name already exists", 0xbb8, new object[] { content.ContentType.SingularDisplayName.ToLower() });
                }
            }
            else if (!content.CurrentUserCanUpload)
            {
                if (initialForm != null)
                {
                    initialForm.SetStatus("<LOC>You do not have permission to upload this content type.", 0xbb8, new object[0]);
                }
            }
            else
            {
                mUploadingContent = true;
                ThreadPool.QueueUserWorkItem(delegate (object s) {
                    content.Version++;
                    IVaultOperation activityMonitor = null;
                    if ((content is IFTPInfo) && (content as IFTPInfo).UploadFTP())
                    {
                        activityMonitor = new VaultFTPUploadOperation(content);
                    }
                    else
                    {
                        activityMonitor = new VaultUploadOperation(content);
                    }
                    if (BeginUploadContent != null)
                    {
                        BeginUploadContent(new ContentOperationCallbackArgs(content, activityMonitor));
                    }
                    activityMonitor.OperationFinished += new ContentOperationCallback(AdditionalContent.UploadOperation_Finished);
                    activityMonitor.Start();
                });
            }
        }

        private static void UploadOperation_Finished(ContentOperationCallbackArgs e)
        {
            mUploadingContent = false;
            if (FinishUploadContent != null)
            {
                FinishUploadContent(e);
            }
        }

        public static void ViewInExplorer(IAdditionalContent content)
        {
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                if (Directory.Exists(content.GetDownloadPath()))
                {
                    Process.Start(content.GetDownloadPath());
                }
            });
        }

        private static void WebService_DeleteContentCompleted(object sender, AsyncCompletedEventArgs e)
        {
            WebService.DeleteContentCompleted -= new DeleteContentCompletedEventHandler(AdditionalContent.WebService_DeleteContentCompleted);
            IAdditionalContent content = (e.UserState as object[])[0] as IAdditionalContent;
            bool allVersions = (bool) (e.UserState as object[])[1];
            try
            {
                if (e.Cancelled)
                {
                    mDeletingMyUpload = false;
                    if (FinishDeleteMyUpload != null)
                    {
                        FinishDeleteMyUpload(new ContentOperationCallbackArgs(content, null, false, new object[] { allVersions }));
                    }
                }
                if (e.Error != null)
                {
                    ErrorLog.WriteLine(e.Error);
                    mDeletingMyUpload = false;
                    if (FinishDeleteMyUpload != null)
                    {
                        FinishDeleteMyUpload(new ContentOperationCallbackArgs(content, null, false, new object[] { allVersions }));
                    }
                }
                bool success = content.DeleteMyUpload(allVersions);
                if (success)
                {
                    if (allVersions)
                    {
                        success = new QuazalQuery("DeleteUploadedContentVersions", new object[] { content.TypeID, content.Name }).ExecuteNonQuery();
                    }
                    else
                    {
                        success = new QuazalQuery("DeleteUploadedContent", new object[] { content.ID }).ExecuteNonQuery();
                    }
                }
                mDeletingMyUpload = false;
                if (FinishDeleteMyUpload != null)
                {
                    FinishDeleteMyUpload(new ContentOperationCallbackArgs(content, null, success, new object[] { allVersions }));
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                mDeletingMyUpload = false;
                if (FinishDeleteMyUpload != null)
                {
                    FinishDeleteMyUpload(new ContentOperationCallbackArgs(content, null, false, new object[] { allVersions }));
                }
            }
        }

        public static bool CheckingforUpdates
        {
            get
            {
                return mCheckingForUpdates;
            }
        }

        public static bool CommentsEnabled
        {
            get
            {
                return ConfigSettings.GetBool("VaultCommentsEnabled", true);
            }
        }

        public virtual int[] ContentDependencies
        {
            get
            {
                if ((this.mContentDependencies == null) || (this.mContentDependencies.Length < 1))
                {
                    return new int[0];
                }
                string[] strArray = this.mContentDependencies.Split(",".ToCharArray());
                List<int> list = new List<int>(strArray.Length);
                foreach (string str in strArray)
                {
                    int num;
                    if (((str != null) && (str.Length >= 1)) && int.TryParse(str, out num))
                    {
                        list.Add(num);
                    }
                }
                return list.ToArray();
            }
        }

        public GPG.Multiplayer.Client.Vaulting.ContentType ContentType
        {
            get
            {
                if ((this.mContentType == null) && (this.mTypeID > 0))
                {
                    this.mContentType = GPG.Multiplayer.Client.Vaulting.ContentType.FromID(this.mTypeID);
                }
                return this.mContentType;
            }
            set
            {
                this.mContentType = value;
            }
        }

        public bool CurrentUserCanDownload
        {
            get
            {
                return (User.Current.IsAdmin || (((this.DownloadACL == null) && ((this.ContentType != null) && this.ContentType.CurrentUserCanDownload)) || (this.DownloadACL.HasAccess() && ((this.ContentType != null) && this.ContentType.CurrentUserCanDownload))));
            }
        }

        public bool CurrentUserCanUpload
        {
            get
            {
                return (User.Current.IsAdmin || ((this.ContentType != null) && this.ContentType.CurrentUserCanUpload));
            }
        }

        public bool CurrentUserIsOwner
        {
            get
            {
                if (User.Current == null)
                {
                    return false;
                }
                return (User.Current.ID == this.OwnerID);
            }
        }

        public int CurrentVersion
        {
            get
            {
                if (this.mCurrentVersion < 0)
                {
                    this.mCurrentVersion = this.Version;
                }
                return this.mCurrentVersion;
            }
            set
            {
                this.mCurrentVersion = value;
            }
        }

        private static int DailyDownloadCount
        {
            get
            {
                return new QuazalQuery("GetDailyDownloadCount", new object[] { DataAccess.FormatDate(DateTime.Now.AddDays(-1.0).ToUniversalTime()), DataAccess.FormatDate(DateTime.Now.ToUniversalTime()) }).GetInt();
            }
        }

        public static bool DeleteUploadsEnabled
        {
            get
            {
                return ConfigSettings.GetBool("VaultDeleteUploadsEnabled", true);
            }
        }

        public static bool DeletingMyContent
        {
            get
            {
                return mDeletingMyContent;
            }
        }

        public static bool DeletingMyUpload
        {
            get
            {
                return mDeletingMyUpload;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        public string Download
        {
            get
            {
                return Loc.Get("<LOC>Download");
            }
        }

        public AccessControlList DownloadACL
        {
            get
            {
                if (this.mDownloadACL == 0)
                {
                    return null;
                }
                return AccessControlList.GetByID(this.mDownloadACL);
            }
            set
            {
                this.mDownloadACL = value.ID;
            }
        }

        public int Downloads
        {
            get
            {
                return this.mDownloads;
            }
            set
            {
                this.mDownloads = value;
            }
        }

        internal static bool DownloadsEnabled
        {
            get
            {
                return ConfigSettings.GetBool("VaultDownloadsEnabled", true);
            }
        }

        public int DownloadSize
        {
            get
            {
                return this.mDownloadSize;
            }
            set
            {
                this.mDownloadSize = value;
            }
        }

        public static IAdditionalContent DownloadTarget
        {
            get
            {
                return mDownloadTarget;
            }
        }

        public string DownloadVolunteerEffort
        {
            get
            {
                if (this.mDownloadVolunteerEffort == null)
                {
                    return "";
                }
                return this.mDownloadVolunteerEffort;
            }
            set
            {
                this.mDownloadVolunteerEffort = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.mEnabled;
            }
            set
            {
                this.mEnabled = value;
            }
        }

        public bool HasVolunteeredForDownload
        {
            get
            {
                if ((this.DownloadVolunteerEffort == null) || (this.DownloadVolunteerEffort.Length < 1))
                {
                    return true;
                }
                if (!this.mHasVolunteeredForDownload.HasValue)
                {
                    this.mHasVolunteeredForDownload = new bool?(new QuazalQuery("HasVolunteeredForUpload", new object[] { this.DownloadVolunteerEffort }).GetBool());
                }
                return this.mHasVolunteeredForDownload.Value;
            }
            set
            {
                this.mHasVolunteeredForDownload = new bool?(value);
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public string LocalFilePath
        {
            get
            {
                return this.mLocalFilePath;
            }
            set
            {
                this.mLocalFilePath = value;
            }
        }

        public static List<IAdditionalContent> MyContent
        {
            get
            {
                lock (MyContentMutex)
                {
                    if (mMyContent == null)
                    {
                        mMyContent = new List<IAdditionalContent>();
                        foreach (GPG.Multiplayer.Client.Vaulting.ContentType type in GPG.Multiplayer.Client.Vaulting.ContentType.All)
                        {
                            if (type.CurrentUserCanDownload)
                            {
                                MyContent.AddRange(type.CreateInstance().GetMyDownloads());
                            }
                        }
                    }
                    return mMyContent;
                }
            }
        }

        public static string MyContentPath
        {
            get
            {
                if ((GameInformation.SelectedGame.GameID == 0x11) || (GameInformation.SelectedGame.GameID == 0x12))
                {
                    return (Environment.GetFolderPath(Environment.SpecialFolder.Personal) + ConfigSettings.GetString("VaultDownloadPathFA", @"\My Games\Gas Powered Games\Supreme Commander Forged Alliance").TrimEnd(@"\".ToCharArray()));
                }
                return (Environment.GetFolderPath(Environment.SpecialFolder.Personal) + ConfigSettings.GetString("VaultDownloadPathDefault", @"\My Games\Gas Powered Games\SupremeCommander").TrimEnd(@"\".ToCharArray()));
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public int OwnerID
        {
            get
            {
                return this.mOwnerID;
            }
            set
            {
                this.mOwnerID = value;
            }
        }

        public string OwnerName
        {
            get
            {
                return this.mOwnerName;
            }
            set
            {
                this.mOwnerName = value;
            }
        }

        public float Rating
        {
            get
            {
                if (this.RatingCount > 0)
                {
                    return (float) Math.Round((double) (((float) this.RatingTotal) / ((float) this.RatingCount)), 2);
                }
                return 0f;
            }
        }

        public int RatingCount
        {
            get
            {
                return this.mRatingCount;
            }
            set
            {
                this.mRatingCount = value;
                this.mRatimgImageSmall = null;
            }
        }

        public static bool RatingEnabled
        {
            get
            {
                return ConfigSettings.GetBool("VaultRatingEnabled", true);
            }
        }

        public Image RatingImageFull
        {
            get
            {
                if (this.mRatimgImageFull == null)
                {
                    if (this.Rating == 0f)
                    {
                        return new Bitmap(1, 1);
                    }
                    Image stars = VaultImages.stars;
                    int width = (int) (stars.Width * (this.Rating / 5f));
                    this.mRatimgImageFull = DrawUtil.CropImageTo(stars, width, stars.Height);
                }
                return this.mRatimgImageFull;
            }
        }

        public Image RatingImageLarge
        {
            get
            {
                if (this.mRatingImageLarge == null)
                {
                    if (this.Rating == 0f)
                    {
                        return VaultImages.stars_gray;
                    }
                    Image stars = VaultImages.stars;
                    int width = (int) (stars.Width * (this.Rating / 5f));
                    this.mRatingImageLarge = DrawUtil.CropImageTo(stars, width, stars.Height);
                    DrawUtil.CopyImage(VaultImages.stars_gray, new Point(width, 0), this.mRatingImageLarge, new Point(width, 0), new Size(this.mRatingImageLarge.Width - width, this.mRatingImageLarge.Height));
                }
                return this.mRatingImageLarge;
            }
        }

        public Image RatingImageSmall
        {
            get
            {
                if (this.mRatimgImageSmall == null)
                {
                    if (this.Rating == 0f)
                    {
                        return VaultImages.stars_small_gray;
                    }
                    Image image = VaultImages.stars_small;
                    int x = (int) (image.Width * (this.Rating / 5f));
                    this.mRatimgImageSmall = image;
                    DrawUtil.CopyImage(VaultImages.stars_small_gray, new Point(x, 0), this.mRatimgImageSmall, new Point(x, 0), new Size(this.mRatimgImageSmall.Width - x, this.mRatimgImageSmall.Height));
                }
                return this.mRatimgImageSmall;
            }
        }

        public int RatingTotal
        {
            get
            {
                return (int) this.mRatingTotal;
            }
            set
            {
                this.mRatingTotal = value;
                this.mRatimgImageSmall = null;
            }
        }

        public static bool SaveSearchEnabled
        {
            get
            {
                return ConfigSettings.GetBool("VaultSaveSearchEnabled", true);
            }
        }

        public static bool SearchEnabled
        {
            get
            {
                return ConfigSettings.GetBool("VaultSearchingEnabled", true);
            }
        }

        public string SearchKeywords
        {
            get
            {
                return this.mSearchKeywords;
            }
            set
            {
                this.mSearchKeywords = value;
            }
        }

        public int TypeID
        {
            get
            {
                if ((this.mTypeID < 1) && (this.mContentType != null))
                {
                    return this.ContentType.ID;
                }
                return this.mTypeID;
            }
        }

        public static bool UploadingContent
        {
            get
            {
                return mUploadingContent;
            }
        }

        public static bool UploadsEnabled
        {
            get
            {
                return ConfigSettings.GetBool("VaultUploadsEnabled", true);
            }
        }

        public static string VaultServerKey
        {
            get
            {
                return ConfigSettings.GetString("VaultServerKey", Program.Settings.Login.DefaultServerName);
            }
        }

        public int Version
        {
            get
            {
                return this.mVersion;
            }
            set
            {
                this.mVersion = value;
            }
        }

        public string VersionCheck
        {
            get
            {
                return this.mVersionCheck;
            }
            set
            {
                this.mVersionCheck = value;
            }
        }

        public DateTime VersionDate
        {
            get
            {
                return this.mVersionDate;
            }
            set
            {
                this.mVersionDate = value;
            }
        }

        public string VersionNotes
        {
            get
            {
                if (this.mVersionNotes == null)
                {
                    return "";
                }
                return this.mVersionNotes;
            }
            set
            {
                this.mVersionNotes = value;
            }
        }
    }
}

