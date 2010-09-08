namespace GPG.Multiplayer.Client.Vaulting.Applications
{
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.Quazal;
    using GPG.Security;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class CustomApplication : AdditionalContent, IAdditionalContent, IContentTypeProvider, IFTPInfo
    {
        private int mAmt = 0;
        private int mBlockCount = 0;
        [FieldMap("build")]
        private string mBuild;
        [FieldMap("build_result")]
        private string mBuildResult;
        [FieldMap("forced_command_line_args")]
        private string mCommandLineArgs;
        [FieldMap("current_build")]
        private string mCurrentBuild;
        [FieldMap("current_friendly_build")]
        private string mCurrentFriendlyBuild;
        private string mEncryptionKey;
        [FieldMap("exe_name")]
        private string mExeName;
        [FieldMap("file_check")]
        private string mFileCheck;
        [FieldMap("file_path")]
        private string mFilePath;
        [FieldMap("file_uri")]
        private string mFileURI;
        [FieldMap("friendly_build")]
        private string mFriendlyBuild;
        [FieldMap("friendly_build_result")]
        private string mFriendlyBuildResult;
        private string mFTPDir;
        private string mFTPPassword;
        private string mFTPServer;
        private bool mFTPUploaded;
        private string mFTPUser;
        [FieldMap("game_description")]
        private string mGameDescription;
        [FieldMap("gpgnet_game_id")]
        private int mGameID;
        [NonSerialized]
        private GameInformation mGameInfo = null;
        [FieldMap("install_check")]
        private string mInstallCheck;
        [FieldMap("install_uri")]
        private string mInstallURI;
        private bool mIsFTPUpload = false;
        [NonSerialized]
        private string mLocalUploadDir;
        [FieldMap("gpgnet_patch_algorithm")]
        private int mPatchAlgorithm;
        [FieldMap("patch_check")]
        private string mPatchCheck;
        [FieldMap("patch_uri")]
        private string mPatchURI;
        [FieldMap("relative_app_directory")]
        private string mRelativeAppDirectory;
        [FieldMap("relative_version_file")]
        private string mRelativeVersionFile;
        private IStatusProvider mStatusProvider = null;
        private string mUploadFile = "";
        [FieldMap("version_kind")]
        private string mVersionKind = "INSTALL";

        private CustomApplication()
        {
        }

        public CustomApplication(DataRecord record) : base(record)
        {
            foreach (GameInformation information in GameInformation.Games)
            {
                if (this.GameID == information.GameID)
                {
                    this.mGameInfo = information;
                }
            }
            try
            {
                File.Delete(this.GetLocalPatchFile());
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public bool AlreadyUploaded()
        {
            return this.FTPUploaded;
        }

        public IAdditionalContent CreateEmptyInstance()
        {
            CustomApplication application = new CustomApplication();
            application.ContentType = base.ContentType;
            return application;
        }

        public QuazalQuery CreateSearchQuery(int page, int pageSize)
        {
            try
            {
                if (!base.ContentType.CurrentUserCanDownload)
                {
                    return QuazalQuery.Empty;
                }
                return new QuazalQuery(ConfigSettings.GetString("ApplicationSearchQuery", "Application Search2"), new object[0]);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return QuazalQuery.Empty;
            }
        }

        public string CreateUploadFile()
        {
            string aFileName = this.MakeUploadFile();
            if ((this.EncryptionKey != null) && (this.EncryptionKey != ""))
            {
                try
                {
                    string str3;
                    Crypto crypto = new Crypto();
                    crypto.OnEncryptAmount += new ProgressDelegate(this.crypto_OnEncryptAmount);
                    crypto.EncryptFileString(this.PadKey(this.EncryptionKey), this.GetIV(), aFileName);
                    string fileName = aFileName + ".enc";
                    if (ConfigSettings.GetBool("Fake Extension", true))
                    {
                        str3 = fileName.Replace(".enc", ".zip");
                        if (File.Exists(str3))
                        {
                            File.Delete(str3);
                        }
                        new FileInfo(fileName).MoveTo(str3);
                        this.mUploadFile = str3;
                    }
                    else
                    {
                        str3 = fileName + ".zip";
                        Compression.ZipFiles(str3, new FileInfo(fileName).DirectoryName, new string[] { fileName }, false);
                        this.mUploadFile = str3;
                    }
                    return this.mUploadFile;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    this.mUploadFile = "";
                    return null;
                }
            }
            this.mUploadFile = aFileName;
            return this.mUploadFile;
        }

        public QuazalQuery CreateVersionQuery()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void crypto_OnDecryptAmount(int bytecount)
        {
            try
            {
                this.mAmt++;
                if (this.mAmt > 0xf4240)
                {
                    this.mBlockCount++;
                    this.mAmt = 0;
                    if (this.mStatusProvider != null)
                    {
                        this.mStatusProvider.SetStatus("<LOC>Decrypting File Block " + this.mBlockCount.ToString(), new object[0]);
                    }
                    Application.DoEvents();
                }
            }
            catch
            {
            }
        }

        private void crypto_OnEncryptAmount(int bytecount)
        {
        }

        public bool DeleteMyContent(bool allVersions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool DeleteMyUpload(bool allVersions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void FinishedDownloader(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                foreach (GameInformation information in GameInformation.Games)
                {
                    if (information.GameID == base.ID)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(this.GetLocalPatchFile());
                        startInfo.Arguments = "/APP " + information.GameDescription + " /URL " + information.PatchURL + " /INSTALL " + information.ExeName + " /VERSION " + information.CurrentVersion + " /PATH " + information.InstallDirectory.Replace("/", @"\");
                        startInfo.UseShellExecute = true;
                        Process.Start(startInfo).WaitForExit();
                        break;
                    }
                }
                GameInformation.LoadGamesFromDB();
            }
            string locfile = this.GetLocalPatchFile();
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                try
                {
                    Thread.Sleep(0x3e8);
                    File.Delete(locfile);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        public bool FromID(int contentId, out IAdditionalContent content)
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                content = null;
                return false;
            }
            content = this.CreateEmptyInstance();
            return true;
        }

        public bool FromLocalFile(string file, out IAdditionalContent content)
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                content = null;
                return false;
            }
            try
            {
                string extension = Path.GetExtension(file);
                string str2 = Path.GetFileName(file).Replace(extension, "");
                base.Name = str2;
                base.LocalFilePath = file;
                content = this;
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                content = null;
                return false;
            }
        }

        public ContentOptions GetDetailsView()
        {
            return ContentOptions.None;
        }

        public string GetDirectURL()
        {
            foreach (GameInformation information in GameInformation.Games)
            {
                if (information.GameID == base.ID)
                {
                    return information.PatcherAppURL;
                }
            }
            return "";
        }

        public ContentOptions GetDownloadOptions()
        {
            return ContentOptions.None;
        }

        public string GetDownloadPath()
        {
            return this.GetLocalPatchFile();
        }

        public string GetFTPDirectory()
        {
            return this.FTPDir;
        }

        public string GetFTPPass()
        {
            return this.FTPPassword;
        }

        public string GetFTPServer()
        {
            return this.FTPServer;
        }

        public string GetFTPUser()
        {
            return this.FTPUser;
        }

        private string GetIV()
        {
            if (base.Name.Length >= 0x10)
            {
                return base.Name.Substring(0, 0x10);
            }
            return base.Name.PadRight(0x10, '0');
        }

        private string GetLocalPatchFile()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Gas Powered Games\WebPatch\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return (path + "webpatch.exe");
        }

        public IAdditionalContent[] GetMyDownloads()
        {
            return new IAdditionalContent[0];
        }

        public IAdditionalContent[] GetMyUploads()
        {
            return new IAdditionalContent[0];
        }

        public ContentOptions GetUploadOptions()
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return ContentOptions.None;
            }
            return new ContentOptions(Loc.Get("<LOC>Application Upload Options"), new PnlApplicationUploadOptions(this));
        }

        public bool IsContentUnique()
        {
            return !new QuazalQuery("DoesContentExist", new object[] { base.ContentType.ID, base.Name }).GetBool();
        }

        private string MakeUploadFile()
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return null;
            }
            if (!this.IsPatch)
            {
                string outputName = Path.GetTempPath() + base.Name + ".zip";
                try
                {
                    Compression.ZipFiles(outputName, this.LocalUploadDir, Directory.GetFiles(this.LocalUploadDir, "*.*", SearchOption.AllDirectories), false);
                    return outputName;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return null;
                }
            }
            return base.LocalFilePath;
        }

        public void MergeWithLocalVersion(IAdditionalContent localVersion)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private string PadKey(string key)
        {
            if (key.Trim().Length >= 0x10)
            {
                return key.Trim().Substring(0, 0x10);
            }
            return key.Trim().PadRight(0x10, '0');
        }

        public void Run()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(base.LocalFilePath);
            startInfo.UseShellExecute = true;
            Process process = Process.Start(startInfo);
        }

        public bool SaveDownload(IStatusProvider statusProvider)
        {
            WebDownloader downloader = new WebDownloader();
            downloader.OnDownloadCompleted += new AsyncCompletedEventHandler(this.FinishedDownloader);
            downloader.BeginDownloadFile(this.GetDirectURL(), this.GetLocalPatchFile(), true);
            return true;
        }

        public bool SaveDownloadData()
        {
            if (this.VersionKind.ToUpper() == "INSTALL")
            {
                new QuazalQuery("Application New GPGnet Game", new object[] { base.Name, this.ExeName, base.VersionCheck, this.FriendlyBuild, this.RelativeVersionFile, this.RelativeAppDirectory, this.CommandLineArgs }).ExecuteNonQuery();
                new QuazalQuery("Application New Game Version Install", new object[] { base.Name, this.GetDirectURL(), base.VersionCheck, "INSTALL", base.ID, this.EncryptionKey }).ExecuteNonQuery();
                GameInformation.LoadGamesFromDB();
            }
            else
            {
                if (this.VersionKind.ToUpper() == "PATCH")
                {
                    throw new Exception("Patches are not yet supported.");
                }
                if (this.VersionKind.ToUpper() == "FILE")
                {
                    throw new Exception("Files are not yet supported.");
                }
            }
            return false;
        }

        public void SetGridView(GridView view)
        {
            GridColumn column = new GridColumn();
            column.Caption = Loc.Get("");
            column.ColumnEdit = new RepositoryItemPictureEdit();
            column.FieldName = "DownloadIcon";
            view.Columns.Add(column);
            column.Visible = true;
            column.VisibleIndex = 0;
        }

        public bool UploadFTP()
        {
            return this.IsFTPUpload;
        }

        public string Build
        {
            get
            {
                return this.mBuild;
            }
            set
            {
                this.mBuild = value;
            }
        }

        public string BuildResult
        {
            get
            {
                return this.mBuildResult;
            }
            set
            {
                this.mBuildResult = value;
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
                return true;
            }
        }

        public string CommandLineArgs
        {
            get
            {
                return this.mCommandLineArgs;
            }
            set
            {
                this.mCommandLineArgs = value;
            }
        }

        public string CurrentBuild
        {
            get
            {
                return this.mCurrentBuild;
            }
            set
            {
                this.mCurrentBuild = value;
            }
        }

        public string CurrentFriendlyBuild
        {
            get
            {
                return this.mCurrentFriendlyBuild;
            }
            set
            {
                this.mCurrentFriendlyBuild = value;
            }
        }

        public Image DownloadIcon
        {
            get
            {
                if (this.VersionKind == "INSTALL")
                {
                    return Resources.NewApp;
                }
                return Resources.Patch;
            }
        }

        public string EncryptionKey
        {
            get
            {
                if (this.mEncryptionKey == null)
                {
                    this.mEncryptionKey = "";
                }
                return this.mEncryptionKey;
            }
            set
            {
                this.mEncryptionKey = value;
            }
        }

        public string ExeName
        {
            get
            {
                return this.mExeName;
            }
            set
            {
                this.mExeName = value;
            }
        }

        public string FileCheck
        {
            get
            {
                return this.mFileCheck;
            }
            set
            {
                this.mFileCheck = value;
            }
        }

        public string FilePath
        {
            get
            {
                return this.mFilePath;
            }
            set
            {
                this.mFilePath = value;
            }
        }

        public string FileURI
        {
            get
            {
                if (this.mFileURI == null)
                {
                    this.mFileURI = "";
                }
                return this.mFileURI;
            }
            set
            {
                this.mFileURI = value;
            }
        }

        public string FriendlyBuild
        {
            get
            {
                return this.mFriendlyBuild;
            }
            set
            {
                this.mFriendlyBuild = value;
            }
        }

        public string FriendlyBuildResult
        {
            get
            {
                return this.mFriendlyBuildResult;
            }
            set
            {
                this.mFriendlyBuildResult = value;
            }
        }

        public string FTPDir
        {
            get
            {
                return this.mFTPDir;
            }
            set
            {
                this.mFTPDir = value;
            }
        }

        public string FTPPassword
        {
            get
            {
                return this.mFTPPassword;
            }
            set
            {
                this.mFTPPassword = value;
            }
        }

        public string FTPServer
        {
            get
            {
                return this.mFTPServer;
            }
            set
            {
                this.mFTPServer = value;
            }
        }

        public bool FTPUploaded
        {
            get
            {
                return this.mFTPUploaded;
            }
            set
            {
                this.mFTPUploaded = value;
            }
        }

        public string FTPUser
        {
            get
            {
                return this.mFTPUser;
            }
            set
            {
                this.mFTPUser = value;
            }
        }

        public string GameDescription
        {
            get
            {
                return this.mGameDescription;
            }
            set
            {
                this.mGameDescription = value;
            }
        }

        public int GameID
        {
            get
            {
                return this.mGameID;
            }
            set
            {
                this.mGameID = value;
            }
        }

        public GameInformation GameInfo
        {
            get
            {
                return this.mGameInfo;
            }
            set
            {
                this.mGameInfo = value;
                if (this.mGameInfo != null)
                {
                    this.GameID = this.mGameInfo.GameID;
                }
            }
        }

        public string InstallCheck
        {
            get
            {
                return this.mInstallCheck;
            }
            set
            {
                this.mInstallCheck = value;
            }
        }

        public string InstallURI
        {
            get
            {
                if (this.mInstallURI == null)
                {
                    this.mInstallURI = "";
                }
                return this.mInstallURI;
            }
            set
            {
                this.mInstallURI = value;
            }
        }

        public bool IsDirectHTTPDownload
        {
            get
            {
                return true;
            }
        }

        public bool IsFTPUpload
        {
            get
            {
                return this.mIsFTPUpload;
            }
            set
            {
                this.mIsFTPUpload = value;
            }
        }

        public bool IsPatch
        {
            get
            {
                return ((this.VersionKind == "PATCH") || (this.VersionKind == "FILE"));
            }
        }

        public string LocalUploadDir
        {
            get
            {
                return this.mLocalUploadDir;
            }
            set
            {
                this.mLocalUploadDir = value;
            }
        }

        public int PatchAlgorithm
        {
            get
            {
                return this.mPatchAlgorithm;
            }
            set
            {
                this.mPatchAlgorithm = value;
            }
        }

        public string PatchCheck
        {
            get
            {
                return this.mPatchCheck;
            }
            set
            {
                this.mPatchCheck = value;
            }
        }

        public string PatchURI
        {
            get
            {
                if (this.mPatchURI == null)
                {
                    this.mPatchURI = "";
                }
                return this.mPatchURI;
            }
            set
            {
                this.mPatchURI = value;
            }
        }

        public string RelativeAppDirectory
        {
            get
            {
                return this.mRelativeAppDirectory;
            }
            set
            {
                this.mRelativeAppDirectory = value;
            }
        }

        public string RelativeVersionFile
        {
            get
            {
                return this.mRelativeVersionFile;
            }
            set
            {
                this.mRelativeVersionFile = value;
            }
        }

        public string RunImagePath
        {
            get
            {
                return @"Dialog\ContentManager\BtnLaunch";
            }
        }

        public string RunTooltip
        {
            get
            {
                return "<LOC>Launch Game";
            }
        }

        public string VersionKind
        {
            get
            {
                return this.mVersionKind;
            }
            set
            {
                this.mVersionKind = value.ToUpper();
            }
        }
    }
}

