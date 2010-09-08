namespace GPG.Multiplayer.Client.Vaulting.Tools
{
    using DevExpress.Utils;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.Quazal;
    using java.util;
    using java.util.zip;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class Tool : AdditionalContent, IAdditionalContent, IContentTypeProvider
    {
        [FieldMap("developer_name")]
        private string mDeveloperName;
        [FieldMap("exe_name")]
        private string mExeName;
        [FieldMap("extract_to_supcom")]
        private bool mExtractToSupcom;
        [FieldMap("num_files")]
        private int mNumberOfFiles;
        private PackagingMethods mPackagingMethod;
        [FieldMap("quality")]
        private string mQuality;
        [FieldMap("website")]
        private string mWebsite;

        public Tool()
        {
            this.mExtractToSupcom = false;
            this.mPackagingMethod = PackagingMethods.TargetDirectoryRecursive;
        }

        public Tool(DataRecord record) : base(record)
        {
            this.mExtractToSupcom = false;
            this.mPackagingMethod = PackagingMethods.TargetDirectoryRecursive;
        }

        public IAdditionalContent CreateEmptyInstance()
        {
            GPG.Multiplayer.Client.Vaulting.Tools.Tool tool = new GPG.Multiplayer.Client.Vaulting.Tools.Tool();
            tool.ContentType = base.ContentType;
            return tool;
        }

        public QuazalQuery CreateSearchQuery(int page, int pageSize)
        {
            try
            {
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
                System.Collections.ArrayList list2 = new System.Collections.ArrayList(30);
                list2.Add(base.Name);
                list2.Add("%" + base.Name + "%");
                list2.Add(base.OwnerName);
                list2.Add(base.OwnerName);
                foreach (string str in list)
                {
                    list2.Add(str);
                    list2.Add("%" + str + "%");
                }
                list2.Add(this.DeveloperName);
                list2.Add("%" + this.DeveloperName + "%");
                list2.Add(this.Quality);
                list2.Add("%" + this.Quality + "%");
                list2.Add(page * pageSize);
                list2.Add(pageSize);
                return new QuazalQuery(ConfigSettings.GetString("ToolSearch", "SearchTools"), list2.ToArray());
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return QuazalQuery.Empty;
            }
        }

        public string CreateUploadFile()
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return null;
            }
            try
            {
                string extension = Path.GetExtension(base.LocalFilePath);
                if (extension == ".exe")
                {
                    string tempFileName = Path.GetTempFileName();
                    List<string> list = new List<string>();
                    switch (this.PackagingMethod)
                    {
                        case PackagingMethods.TargetOnly:
                            list.Add(base.LocalFilePath);
                            break;

                        case PackagingMethods.TargetDirectory:
                            list.AddRange(Directory.GetFiles(Path.GetDirectoryName(base.LocalFilePath), "*.*", SearchOption.TopDirectoryOnly));
                            break;

                        case PackagingMethods.TargetDirectoryRecursive:
                            list.AddRange(Directory.GetFiles(Path.GetDirectoryName(base.LocalFilePath), "*.*", SearchOption.AllDirectories));
                            break;
                    }
                    Compression.ZipFiles(tempFileName, null, list.ToArray(), true);
                    return tempFileName;
                }
                if (extension == ".zip")
                {
                    return base.LocalFilePath;
                }
                return null;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        public QuazalQuery CreateVersionQuery()
        {
            return new QuazalQuery("GetToolHistory", new object[] { base.Name });
        }

        public bool DeleteMyContent(bool allVersions)
        {
            try
            {
                string directoryName;
                if (this.ExtractToSupcom)
                {
                    if (((GameInformation.SelectedGame.GameLocation == null) || (GameInformation.SelectedGame.GameLocation.Length < 1)) && !Program.MainForm.LocateExe("SupremeCommander", true))
                    {
                        return false;
                    }
                    directoryName = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation);
                }
                else
                {
                    directoryName = this.GetDownloadPath();
                }
                if (Directory.Exists(directoryName))
                {
                    string str2 = string.Format("{0} v{1}.lnk", base.Name, base.Version).Trim(Path.GetInvalidPathChars());
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), str2);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    if (this.ExtractToSupcom && File.Exists(Path.Combine(directoryName, this.ExeName)))
                    {
                        File.Delete(Path.Combine(directoryName, this.ExeName));
                    }
                    Directory.Delete(this.GetDownloadPath(), true);
                    if (allVersions)
                    {
                        string str4 = this.GetDownloadPath().Remove(this.GetDownloadPath().Length - 6);
                        foreach (DirectoryInfo info in new DirectoryInfo(this.GetDownloadPath()).Parent.GetDirectories())
                        {
                            if (info.Name.Remove(info.Name.Length - 6) == str4)
                            {
                                info.Delete(true);
                            }
                        }
                    }
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
            if (allVersions)
            {
                return new QuazalQuery("DeleteToolVersions", new object[] { base.TypeID, base.Name }).ExecuteNonQuery();
            }
            return new QuazalQuery("DeleteTool", new object[] { base.ID }).ExecuteNonQuery();
        }

        public bool FromID(int contentId, out IAdditionalContent content)
        {
            GPG.Multiplayer.Client.Vaulting.Tools.Tool tool;
            if (!base.ContentType.CurrentUserCanDownload)
            {
                content = null;
                return false;
            }
            bool flag = new QuazalQuery("GetToolById", new object[] { contentId }).GetObject<GPG.Multiplayer.Client.Vaulting.Tools.Tool>(out tool);
            content = tool;
            return flag;
        }

        public bool FromLocalFile(string file, out IAdditionalContent content)
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                content = null;
                return false;
            }
            if (file.EndsWith(".exe"))
            {
                base.Name = Path.GetFileName(file);
                this.ExeName = base.Name;
                base.Name = base.Name.Remove(base.Name.Length - ".exe".Length, ".exe".Length);
                this.NumberOfFiles = Directory.GetFiles(Path.GetDirectoryName(file), "*.*", SearchOption.AllDirectories).Length;
            }
            else if (file.EndsWith(".zip"))
            {
                ZipFile file2 = null;
                base.Name = Path.GetFileName(file);
                base.Name = base.Name.Remove(base.Name.Length - ".zip".Length, ".zip".Length);
                try
                {
                    file2 = new ZipFile(file);
                    Enumeration enumeration = file2.entries();
                    List<ZipEntry> list = new List<ZipEntry>();
                    while (enumeration.hasMoreElements())
                    {
                        list.Add(enumeration.nextElement() as ZipEntry);
                    }
                    bool flag = false;
                    int num = 0;
                    foreach (ZipEntry entry in list)
                    {
                        if (!entry.isDirectory())
                        {
                            num++;
                        }
                        if (entry.getName().EndsWith(".exe") || entry.getName().EndsWith(".bat"))
                        {
                            this.ExeName = entry.getName();
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        content = null;
                        return false;
                    }
                    this.NumberOfFiles = num;
                }
                finally
                {
                    file2.close();
                }
            }
            base.LocalFilePath = file;
            content = this;
            return true;
        }

        public ContentOptions GetDetailsView()
        {
            return new ContentOptions("<LOC>Tool Details", new PnlToolDetailsView(this));
        }

        public string GetDirectURL()
        {
            return "";
        }

        public ContentOptions GetDownloadOptions()
        {
            return new ContentOptions("<LOC>Tool Options", new PnlToolDownloadOptions(this));
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
            QuazalQuery query = new QuazalQuery("GetMyDownloadedTools", new object[0]);
            return query.GetObjects<GPG.Multiplayer.Client.Vaulting.Tools.Tool>().ToArray();
        }

        public IAdditionalContent[] GetMyUploads()
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return new IAdditionalContent[0];
            }
            QuazalQuery query = new QuazalQuery("GetMyUploadedTools", new object[0]);
            return query.GetObjects<GPG.Multiplayer.Client.Vaulting.Tools.Tool>().ToArray();
        }

        public ContentOptions GetUploadOptions()
        {
            return new ContentOptions("<LOC>Tool Upload Options", new PnlToolUploadOptions(this));
        }

        public bool IsContentUnique()
        {
            return !new QuazalQuery("DoesContentExist", new object[] { base.ContentType.ID, base.Name }).GetBool();
        }

        public void MergeWithLocalVersion(IAdditionalContent localVersion)
        {
            GPG.Multiplayer.Client.Vaulting.Tools.Tool tool = localVersion as GPG.Multiplayer.Client.Vaulting.Tools.Tool;
            base.LocalFilePath = tool.LocalFilePath;
            this.ExeName = tool.ExeName;
        }

        private void RefreshFileCount()
        {
            if (base.LocalFilePath.EndsWith(".zip"))
            {
                ZipFile file = null;
                try
                {
                    file = new ZipFile(base.LocalFilePath);
                    Enumeration enumeration = file.entries();
                    List<ZipEntry> list = new List<ZipEntry>();
                    while (enumeration.hasMoreElements())
                    {
                        list.Add(enumeration.nextElement() as ZipEntry);
                    }
                    int num = 0;
                    foreach (ZipEntry entry in list)
                    {
                        if (!entry.isDirectory())
                        {
                            num++;
                        }
                        if (entry.getName().EndsWith(".exe"))
                        {
                            base.Name = Path.GetFileName(entry.getName());
                            this.ExeName = base.Name;
                            base.Name = base.Name.Remove(base.Name.Length - ".exe".Length, ".exe".Length);
                        }
                    }
                    this.NumberOfFiles = num;
                }
                finally
                {
                    file.close();
                }
            }
            else
            {
                switch (this.PackagingMethod)
                {
                    case PackagingMethods.TargetOnly:
                        this.NumberOfFiles = 1;
                        return;

                    case PackagingMethods.TargetDirectory:
                        this.NumberOfFiles = Directory.GetFiles(Path.GetDirectoryName(base.LocalFilePath), "*.*", SearchOption.TopDirectoryOnly).Length;
                        return;

                    case PackagingMethods.TargetDirectoryRecursive:
                        this.NumberOfFiles = Directory.GetFiles(Path.GetDirectoryName(base.LocalFilePath), "*.*", SearchOption.AllDirectories).Length;
                        return;
                }
            }
        }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                try
                {
                    Process.Start(Path.Combine(this.GetDownloadPath(), FileUtil.RemoveExt(this.ExeName)));
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        public bool SaveDownload(IStatusProvider statusProvider)
        {
            try
            {
                string unzipPath;
                if (this.ExtractToSupcom)
                {
                    if (((GameInformation.SelectedGame.GameLocation == null) || (GameInformation.SelectedGame.GameLocation.Length < 1)) || !File.Exists(GameInformation.SelectedGame.GameLocation))
                    {
                        GameInformation.SelectedGame.GameLocation = null;
                        if (!Program.MainForm.LocateExe("SupremeCommander", true))
                        {
                            return false;
                        }
                    }
                    unzipPath = Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation);
                }
                else
                {
                    unzipPath = this.GetDownloadPath();
                }
                string path = string.Format(@"{0}\content.partial", this.GetDownloadPath());
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
                Compression.Unzip(path, this.GetDownloadPath());
                File.Delete(path);
                if (this.ExtractToSupcom)
                {
                    string sourceFileName = Path.Combine(this.GetDownloadPath(), this.ExeName);
                    foreach (string str3 in Directory.GetFiles(this.GetDownloadPath()))
                    {
                        if (str3 == sourceFileName)
                        {
                            if (File.Exists(Path.Combine(unzipPath, this.ExeName)))
                            {
                                File.Delete(Path.Combine(unzipPath, this.ExeName));
                            }
                            File.Move(sourceFileName, Path.Combine(unzipPath, this.ExeName));
                        }
                    }
                    FileUtil.CreateShortcut(Path.Combine(unzipPath, this.ExeName), Path.Combine(this.GetDownloadPath(), base.Name.Trim(Path.GetInvalidPathChars())));
                }
                Program.MainForm.BeginInvoke((VGen0)delegate {
                    if (new DlgYesNo(Program.MainForm, "<LOC>Create Shortcut?", "<LOC>Would you like to create a desktop shortcut this tool?").ShowDialog() == DialogResult.Yes)
                    {
                        string str = string.Format("{0} v{1}", this.Name, this.Version).Trim(Path.GetInvalidPathChars());
                        FileUtil.CreateShortcut(Path.Combine(unzipPath, this.ExeName), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), str));
                    }
                });
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public bool SaveDownloadData()
        {
            return new QuazalQuery("UploadTool", new object[] { base.ID, this.ExeName, this.DeveloperName, this.Website, this.Quality, this.NumberOfFiles }).ExecuteNonQuery();
        }

        public void SetGridView(GridView view)
        {
            MultiVal<string, string, bool>[] valArray = new MultiVal<string, string, bool>[] { new MultiVal<string, string, bool>("<LOC>Developer", "DeveloperName", true), new MultiVal<string, string, bool>("<LOC>Product Website", "Website", false), new MultiVal<string, string, bool>("<LOC>Quality", "QualityDisplay", true), new MultiVal<string, string, bool>("<LOC>Executable Name", "ExeName", false), new MultiVal<string, string, bool>("<LOC>Number of Files", "NumberOfFiles", false) };
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
            view.Columns["Download"].VisibleIndex = 0;
            view.Columns["Name"].VisibleIndex = 1;
            view.Columns["DeveloperName"].VisibleIndex = 2;
            view.Columns["QualityDisplay"].VisibleIndex = 3;
            view.Columns["Version"].VisibleIndex = 4;
            view.Columns["OwnerName"].VisibleIndex = 5;
            view.Columns["Downloads"].VisibleIndex = 6;
            view.Columns["RatingImageSmall"].VisibleIndex = 7;
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
                return base.ContentType.CurrentUserCanUpload;
            }
        }

        public string DeveloperName
        {
            get
            {
                return this.mDeveloperName;
            }
            set
            {
                this.mDeveloperName = value;
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

        public bool ExtractToSupcom
        {
            get
            {
                return this.mExtractToSupcom;
            }
        }

        public bool IsDirectHTTPDownload
        {
            get
            {
                return false;
            }
        }

        public int NumberOfFiles
        {
            get
            {
                return this.mNumberOfFiles;
            }
            set
            {
                this.mNumberOfFiles = value;
            }
        }

        public PackagingMethods PackagingMethod
        {
            get
            {
                return this.mPackagingMethod;
            }
            set
            {
                this.mPackagingMethod = value;
                this.RefreshFileCount();
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
                return @"Dialog\ContentManager\BtnLaunch";
            }
        }

        public string RunTooltip
        {
            get
            {
                return "<LOC>Launch Tool";
            }
        }

        public string Website
        {
            get
            {
                return this.mWebsite;
            }
            set
            {
                this.mWebsite = value;
            }
        }
    }
}

