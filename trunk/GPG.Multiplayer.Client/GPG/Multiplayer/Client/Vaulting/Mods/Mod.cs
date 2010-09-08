namespace GPG.Multiplayer.Client.Vaulting.Mods
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.ImageConverter;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault;
    using GPG.UI;
    using java.io;
    using java.util;
    using java.util.zip;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;

    [Serializable]
    public class Mod : AdditionalContent, IAdditionalContent, IContentTypeProvider
    {
        [NonSerialized]
        private Service _WS = new Service();
        [NonSerialized]
        private static GridView BoundGridView = null;
        private static Dictionary<int, string> CachedImageLocations = new Dictionary<int, string>();
        internal static Dictionary<string, Mod> CachedModLookups = new Dictionary<string, Mod>();
        [FieldMap("conflicts")]
        private string mConflicts = "";
        [FieldMap("copyright")]
        private string mCopyright;
        [FieldMap("developer")]
        private string mDeveloperName;
        [FieldMap("exclusive")]
        private bool mExclusive;
        [FieldMap("guid")]
        private string mGuid;
        [FieldMap("has_preview")]
        private bool mHasPreview;
        [FieldMap("mod_id")]
        private int mModID;
        [FieldMap("mod_name")]
        private string mModName;
        [FieldMap("number_of_files")]
        private int mNumberOfFiles;
        private Image mPreviewImage50 = null;
        [FieldMap("requirement_names")]
        private string mRequirementNames = "";
        [FieldMap("requirements")]
        private string mRequirements = "";
        [FieldMap("ui_only")]
        private bool? mUIOnly = false;
        [FieldMap("website")]
        private string mWebsite;
        private static Image Random50 = null;

        [field: NonSerialized]
        public static  event EventHandler PreviewImageLoaded;

        private Mod()
        {
        }

        public Mod(DataRecord record) : base(record)
        {
        }

        public IAdditionalContent CreateEmptyInstance()
        {
            Mod mod = new Mod();
            mod.ContentType = base.ContentType;
            return mod;
        }

        public QuazalQuery CreateSearchQuery(int page, int pageSize)
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return QuazalQuery.Empty;
            }
            try
            {
                int num = DataAccess.FormatBool(this.UIOnly);
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
                list2.Add(this.Guid);
                list2.Add(this.Guid);
                list2.Add(this.DeveloperName);
                list2.Add("%" + this.DeveloperName + "%");
                list2.Add(num);
                list2.Add(num);
                list2.Add(page * pageSize);
                list2.Add(pageSize);
                return new QuazalQuery(ConfigSettings.GetString("CustomModSearch", "SearchCustomMods"), list2.ToArray());
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return QuazalQuery.Empty;
            }
        }

        public string CreateUploadFile()
        {
            string str10;
            try
            {
                if (!base.ContentType.CurrentUserCanUpload)
                {
                    return null;
                }
                if ((this.Guid == null) || (this.Guid.Length < 1))
                {
                    bool resolved = false;
                    Program.MainForm.Invoke((VGen0)delegate {
                        if (new DlgYesNo(Program.MainForm, "<LOC>Unique ID", "<LOC>This mod does not contain the required Unique ID field, GPGnet can create one for you or you can use the Guid generation tool included in the GPGnet install directory. Would you like GPGnet to generate a UID for you?").ShowDialog() == DialogResult.Yes)
                        {
                            this.Guid = System.Guid.NewGuid().ToString();
                            resolved = true;
                            Clipboard.SetText(this.Guid);
                            DlgMessage.ShowDialog(string.Format(Loc.Get("<LOC>Your generated UID is: {0}, and has been copied to your clipboard. It is highly recommended you paste this into your mod now."), this.Guid), "<LOC>Unique ID");
                        }
                    });
                    if (!resolved)
                    {
                        throw new Exception(Loc.Get("<LOC>Mod does not contain the required UID field."));
                    }
                }
                try
                {
                    System.Guid guid = new System.Guid(this.Guid);
                }
                catch
                {
                    bool resolved = false;
                    Program.MainForm.Invoke((VGen0)delegate {
                        if (new DlgYesNo(Program.MainForm, "<LOC>Unique ID", "<LOC>The Unique ID field for this mod is not the correct Guid format, GPGnet can create one for you or you can use the Guid generation tool included in the GPGnet install directory. Would you like GPGnet to generate a UID for you?").ShowDialog() == DialogResult.Yes)
                        {
                            this.Guid = System.Guid.NewGuid().ToString();
                            resolved = true;
                            Clipboard.SetText(this.Guid);
                            DlgMessage.ShowDialog(string.Format(Loc.Get("<LOC>Your generated UID is: {0}, and has been copied to your clipboard. It is highly recommended you paste this into your mod now."), this.Guid), "<LOC>Unique ID");
                        }
                    });
                    if (!resolved)
                    {
                        throw new Exception(Loc.Get("<LOC>UID field must be a well formed Guid. Use the included GuidGenerator tool for assistance."));
                    }
                }
                string extension = Path.GetExtension(base.LocalFilePath);
                string path = Path.GetTempPath() + System.Guid.NewGuid().ToString();
                Directory.CreateDirectory(path);
                if (!Path.HasExtension(base.LocalFilePath))
                {
                    foreach (string str3 in Directory.GetFiles(base.LocalFilePath, "*.*", SearchOption.AllDirectories))
                    {
                        string str4 = Path.Combine(path, str3.Replace(base.LocalFilePath, "").TrimStart(@"\".ToCharArray()));
                        if (!Directory.Exists(Path.GetDirectoryName(str4)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(str4));
                        }
                        System.IO.File.Copy(str3, str4);
                    }
                }
                else
                {
                    switch (extension)
                    {
                        case ".scd":
                        case ".zip":
                        {
                            string str6 = Path.GetTempPath() + System.Guid.NewGuid().ToString();
                            if (!Directory.Exists(str6))
                            {
                                Directory.CreateDirectory(str6);
                            }
                            Compression.Unzip(base.LocalFilePath, path);
                            break;
                        }
                    }
                }
                string rootDirectory = path;
                List<string> list = new List<string>();
                foreach (string str3 in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    if (str3.EndsWith("mod_info.lua"))
                    {
                        this.ValidateModFile(str3);
                        rootDirectory = Path.GetDirectoryName(str3);
                    }
                    list.Add(str3);
                }
                if (this.HasPreview)
                {
                    string filename = rootDirectory + @"\preview.png";
                    this.PreviewImage50.Save(filename, ImageFormat.Png);
                    list.Add(filename);
                }
                string tempFileName = Path.GetTempFileName();
                Compression.ZipFiles(tempFileName, rootDirectory, list.ToArray(), false);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                if (!System.IO.File.Exists(tempFileName))
                {
                    return null;
                }
                str10 = tempFileName;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                throw exception;
            }
            return str10;
        }

        public QuazalQuery CreateVersionQuery()
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return QuazalQuery.Empty;
            }
            return new QuazalQuery("GetModHistory", new object[] { base.Name });
        }

        private void CustomMap_PreviewImageLoaded(object sender, EventArgs e)
        {
            if (BoundGridView != null)
            {
                if (((BoundGridView.GridControl == null) || BoundGridView.GridControl.Disposing) || BoundGridView.GridControl.IsDisposed)
                {
                    BoundGridView = null;
                }
                else
                {
                    Mod mod = sender as Mod;
                    try
                    {
                        IAdditionalContent[] dataSource = BoundGridView.DataSource as IAdditionalContent[];
                        if ((dataSource != null) && (dataSource.Length >= 1))
                        {
                            if (dataSource[0].TypeID != mod.TypeID)
                            {
                                BoundGridView = null;
                            }
                            else
                            {
                                VGen0 method = null;
                                for (int i = 0; i < dataSource.Length; i++)
                                {
                                    if (dataSource[i].ID == mod.ID)
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

        public bool DeleteMyContent(bool allVersions)
        {
            try
            {
                string downloadPath = this.GetDownloadPath(true);
                if (Directory.Exists(downloadPath))
                {
                    Directory.Delete(downloadPath, true);
                    if (allVersions)
                    {
                        string str2 = this.GetDownloadPath().Remove(this.GetDownloadPath().Length - 6);
                        foreach (DirectoryInfo info in new DirectoryInfo(this.GetDownloadPath()).Parent.GetDirectories())
                        {
                            if (info.Name.Remove(info.Name.Length - 6) == str2)
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
                return new QuazalQuery("DeleteCustomModVersions", new object[] { base.TypeID, base.Name }).ExecuteNonQuery();
            }
            return new QuazalQuery("DeleteCustomMod", new object[] { base.ID }).ExecuteNonQuery();
        }

        public bool FromID(int contentId, out IAdditionalContent content)
        {
            Mod mod;
            if (!base.ContentType.CurrentUserCanUpload)
            {
                content = null;
                return false;
            }
            bool flag = new QuazalQuery("GetModById", new object[] { contentId }).GetObject<Mod>(out mod);
            content = mod;
            return flag;
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
                string tempFileName;
                string str6;
                string str7;
                string extension = Path.GetExtension(file);
                XmlDocument document = null;
                switch (extension)
                {
                    case ".scd":
                    case ".zip":
                    {
                        ZipFile file2 = null;
                        try
                        {
                            file2 = new ZipFile(file);
                            Enumeration enumeration = file2.entries();
                            List<ZipEntry> list = new List<ZipEntry>();
                            while (enumeration.hasMoreElements())
                            {
                                list.Add(enumeration.nextElement() as ZipEntry);
                            }
                            foreach (ZipEntry entry in list)
                            {
                                if (!entry.isDirectory())
                                {
                                    this.NumberOfFiles++;
                                }
                                if (entry.getName().ToLower().EndsWith("mod_info.lua"))
                                {
                                    string path = entry.getName();
                                    string directoryName = Path.GetDirectoryName(path);
                                    if ((directoryName == null) || (directoryName.Length < 1))
                                    {
                                        directoryName = Path.GetDirectoryName(file);
                                    }
                                    this.mModName = new DirectoryInfo(directoryName).Name;
                                    path = Path.GetFileName(path);
                                    InputStream stream = null;
                                    try
                                    {
                                        stream = file2.getInputStream(entry);
                                        byte[] bytes = new byte[0x1000];
                                        bool flag = false;
                                        string contents = "";
                                        while (!flag)
                                        {
                                            int index = 0;
                                            while (index < bytes.Length)
                                            {
                                                int num = stream.read();
                                                if (num < 0)
                                                {
                                                    flag = true;
                                                    break;
                                                }
                                                bytes[index] = (byte) num;
                                                index++;
                                            }
                                            contents = contents + Encoding.UTF8.GetString(bytes, 0, index);
                                        }
                                        tempFileName = Path.GetTempFileName();
                                        System.IO.File.WriteAllText(tempFileName, contents);
                                        str7 = LuaUtil.ModToXml(tempFileName, out str6);
                                        if ((str6 != null) && (str6.Length > 0))
                                        {
                                            ErrorLog.WriteLine("Error parsing Mod file: {0}", new object[] { str6 });
                                            content = null;
                                            return false;
                                        }
                                        document = new XmlDocument();
                                        document.LoadXml(str7);
                                        base.LocalFilePath = file;
                                        System.IO.File.Delete(tempFileName);
                                    }
                                    finally
                                    {
                                        if (stream != null)
                                        {
                                            stream.close();
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            file2.close();
                        }
                        break;
                    }
                    default:
                        if (extension == ".lua")
                        {
                            if (!file.EndsWith("mod_info.lua"))
                            {
                                content = null;
                                return false;
                            }
                            str7 = LuaUtil.ModToXml(file, out str6);
                            if ((str6 != null) && (str6.Length > 0))
                            {
                                ErrorLog.WriteLine("Error parsing Mod file: {0}", new object[] { str6 });
                                content = null;
                                return false;
                            }
                            document = new XmlDocument();
                            document.LoadXml(str7);
                            base.LocalFilePath = Path.GetDirectoryName(file);
                            this.mNumberOfFiles = Directory.GetFiles(base.LocalFilePath, "*.*", SearchOption.AllDirectories).Length;
                            this.mModName = new DirectoryInfo(Path.GetDirectoryName(file)).Name;
                        }
                        break;
                }
                if (document == null)
                {
                    content = null;
                    return false;
                }
                base.Name = document["mod"]["name"].InnerText;
                base.Description = document["mod"]["description"].InnerText;
                if (document["mod"]["copyright"] != null)
                {
                    this.Copyright = document["mod"]["copyright"].InnerText.Replace("\x00a9", "").Replace("\x00ef\x00bf\x00bd", "");
                }
                if (document["mod"]["author"] != null)
                {
                    this.DeveloperName = document["mod"]["author"].InnerText;
                }
                if (document["mod"]["url"] != null)
                {
                    this.Website = document["mod"]["url"].InnerText;
                }
                this.Exclusive = this.ParseBool(document["mod"]["exclusive"].InnerText);
                this.UIOnly = new bool?(this.ParseBool(document["mod"]["ui_only"].InnerText));
                this.Guid = document["mod"]["uid"].InnerText;
                if (document["mod"]["requires"] != null)
                {
                    this.mRequirements = document["mod"]["requires"].InnerText;
                }
                if (document["mod"]["requiresNames"] != null)
                {
                    this.mRequirementNames = document["mod"]["requiresNames"].InnerText;
                }
                if (document["mod"]["conflicts"] != null)
                {
                    this.mConflicts = document["mod"]["conflicts"].InnerText;
                }
                if (((document["mod"]["icon"] != null) && (document["mod"]["icon"].InnerText != null)) && (document["mod"]["icon"].InnerText.Length > 0))
                {
                    string targetFile = document["mod"]["icon"].InnerText.Replace("/", @"\");
                    if (extension == ".lua")
                    {
                        if (targetFile.IndexOf(this.ModName) >= 0)
                        {
                            targetFile = targetFile.Remove(0, targetFile.IndexOf(this.ModName)).TrimStart(@"\".ToCharArray());
                        }
                        string str9 = Path.GetDirectoryName(base.LocalFilePath);
                        string str10 = targetFile;
                        string str11 = Path.Combine(str9, str10);
                        if (Path.GetExtension(str11) == ".dds")
                        {
                            this.mPreviewImage50 = ConvertDDS.ToBitmap(str11);
                        }
                        else
                        {
                            this.mPreviewImage50 = Image.FromFile(str11);
                        }
                    }
                    else
                    {
                        tempFileName = Path.GetTempFileName();
                        Compression.Unzip(file, targetFile, tempFileName);
                        if (Path.GetExtension(targetFile) == ".dds")
                        {
                            this.mPreviewImage50 = ConvertDDS.ToBitmap(tempFileName);
                        }
                        else
                        {
                            this.mPreviewImage50 = Image.FromFile(tempFileName);
                        }
                        System.IO.File.Delete(tempFileName);
                    }
                    this.mHasPreview = true;
                }
                else
                {
                    this.mPreviewImage50 = null;
                }
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
            return new ContentOptions("<LOC>Mod Details", new PnlModDetailsView(this));
        }

        public string GetDirectURL()
        {
            return "";
        }

        public ContentOptions GetDownloadOptions()
        {
            return new ContentOptions("<LOC>Mod Download Options", new PnlModDownloadOptions(this));
        }

        public string GetDownloadPath()
        {
            return this.GetDownloadPath(false);
        }

        public string GetDownloadPath(bool truePath)
        {
            if (truePath)
            {
                return string.Format(@"{0}\{1}", base.ContentType.DownloadPath, this.ModName);
            }
            string str = base.Version.ToString();
            str = "v" + str.PadLeft(5 - str.Length, '0');
            return string.Format(@"{0}\{1}\{2}", base.ContentType.DownloadPath, this.ModName, str);
        }

        public IAdditionalContent[] GetMyDownloads()
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return new IAdditionalContent[0];
            }
            QuazalQuery query = new QuazalQuery("GetMyDownloadedMods", new object[0]);
            return query.GetObjects<Mod>().ToArray();
        }

        public IAdditionalContent[] GetMyUploads()
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return new IAdditionalContent[0];
            }
            QuazalQuery query = new QuazalQuery("GetMyUploadedMods", new object[0]);
            return query.GetObjects<Mod>().ToArray();
        }

        public ContentOptions GetUploadOptions()
        {
            return new ContentOptions("<LOC>Mod Upload Options", new PnlModUploadOptions(this));
        }

        public bool IsContentUnique()
        {
            return !new QuazalQuery("DoesModExist", new object[] { base.TypeID, base.Name, this.ModName, this.Guid }).GetBool();
        }

        public void MergeWithLocalVersion(IAdditionalContent localVersion)
        {
            if (base.ContentType.CurrentUserCanUpload && (localVersion is Mod))
            {
                Mod mod = localVersion as Mod;
                base.LocalFilePath = mod.LocalFilePath;
                this.ModName = mod.ModName;
                this.mPreviewImage50 = mod.PreviewImage50;
                this.Copyright = mod.Copyright;
                this.DeveloperName = mod.DeveloperName;
                this.Website = mod.Website;
                this.UIOnly = mod.UIOnly;
                this.Exclusive = mod.Exclusive;
                this.Guid = mod.Guid;
                this.mRequirements = mod.mRequirements;
                this.mConflicts = mod.mConflicts;
                if (base.ID < 1)
                {
                    base.Name = mod.Name;
                }
            }
        }

        private bool ParseBool(string value)
        {
            if ((value == null) || (value.Length < 1))
            {
                return false;
            }
            return bool.Parse(value);
        }

        public void Run()
        {
        }

        public bool SaveDownload(IStatusProvider statusProvider)
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                if (statusProvider != null)
                {
                    statusProvider.SetStatus("<LOC>You do not have permission to download this content", new object[0]);
                }
                return false;
            }
            try
            {
                string downloadPath = this.GetDownloadPath();
                string path = downloadPath + @"\content.partial";
                bool flag = false;
                foreach (string str3 in Directory.GetDirectories(this.GetDownloadPath(true)))
                {
                    if (!(str3 == downloadPath))
                    {
                        flag = true;
                        Directory.Delete(str3, true);
                    }
                }
                foreach (string str4 in Directory.GetFiles(this.GetDownloadPath(true), "*.*", SearchOption.TopDirectoryOnly))
                {
                    flag = true;
                    System.IO.File.Delete(str4);
                }
                if (flag)
                {
                    AdditionalContent.DeleteMyContent(this, true, false, true);
                    foreach (IAdditionalContent content in AdditionalContent.MyContent.ToArray())
                    {
                        if (((content.TypeID == base.TypeID) && (content.Name == base.Name)) && (content.Version != base.Version))
                        {
                            AdditionalContent.MyContent.Remove(content);
                        }
                    }
                }
                if (!System.IO.File.Exists(path))
                {
                    ErrorLog.WriteLine("The expected download file: {0} was not found.", new object[] { path });
                    if (statusProvider != null)
                    {
                        statusProvider.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                    }
                    return false;
                }
                statusProvider.SetStatus("<LOC>Extracting File(s)...", new object[0]);
                Compression.Unzip(path, this.GetDownloadPath(true));
                if (!Directory.Exists(this.GetDownloadPath()))
                {
                    Directory.CreateDirectory(this.GetDownloadPath());
                }
                System.IO.File.WriteAllText(Path.Combine(this.GetDownloadPath(), string.Format("{0}.version", this.ModName)), "Do not delete this file, it is required to maintain this mod's version");
                Thread.Sleep(50);
                DateTime now = DateTime.Now;
                while (System.IO.File.Exists(path))
                {
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch
                    {
                        if ((DateTime.Now - now) > TimeSpan.FromSeconds(3.0))
                        {
                            break;
                        }
                        Thread.Sleep(50);
                    }
                }
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
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return false;
            }
            return DataAccess.ExecuteQuery("UploadCustomMod", new object[] { base.ID, this.ModName, this.Guid, this.UIOnly, this.DeveloperName, this.Copyright, this.Website, this.Exclusive, this.NumberOfFiles, this.HasPreview, this.mRequirements, this.mConflicts });
        }

        public void SetGridView(GridView view)
        {
            if (base.ContentType.CurrentUserCanDownload)
            {
                BoundGridView = view;
                GridColumn column = new GridColumn();
                column.Caption = Loc.Get("<LOC>Preview");
                column.FieldName = "PreviewImage50";
                column.ColumnEdit = new RepositoryItemPictureEdit();
                view.Columns.Add(column);
                column.Visible = true;
                GridColumn column2 = new GridColumn();
                column2.Caption = Loc.Get("<LOC>UI Only");
                column2.FieldName = "UIOnlyDisplay";
                column2.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
                column2.AppearanceCell.Options.UseTextOptions = true;
                view.Columns.Add(column2);
                column2.Visible = true;
                GridColumn column3 = new GridColumn();
                column3.Caption = Loc.Get("<LOC>Developer");
                column3.FieldName = "Developer";
                column3.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
                column3.AppearanceCell.Options.UseTextOptions = true;
                view.Columns.Add(column3);
                column3.Visible = true;
                view.Columns["Download"].VisibleIndex = 0;
                column.VisibleIndex = 1;
                view.Columns["Name"].VisibleIndex = 2;
                column2.VisibleIndex = 3;
                column3.VisibleIndex = 4;
                view.Columns["Version"].VisibleIndex = 5;
                view.Columns["OwnerName"].VisibleIndex = 6;
                view.Columns["Downloads"].VisibleIndex = 7;
                view.Columns["RatingImageSmall"].VisibleIndex = 8;
                MultiVal<string, string>[] valArray = new MultiVal<string, string>[] { new MultiVal<string, string>("<LOC>Number of Files", "NumberOfFiles"), new MultiVal<string, string>("<LOC>Website", this.Website), new MultiVal<string, string>("<LOC>Copyright", "Copyright"), new MultiVal<string, string>("<LOC>Unique ID", "Guid") };
                foreach (MultiVal<string, string> val in valArray)
                {
                    GridColumn column4 = new GridColumn();
                    column4.Caption = Loc.Get(val.Value1);
                    column4.FieldName = val.Value2;
                    column4.Visible = false;
                    view.Columns.Add(column4);
                }
                PreviewImageLoaded -= new EventHandler(this.CustomMap_PreviewImageLoaded);
                PreviewImageLoaded += new EventHandler(this.CustomMap_PreviewImageLoaded);
            }
        }

        private void ValidateModFile(string modInfoFile)
        {
            string str;
            string contents = LuaUtil.ValidateMod(modInfoFile, this.Guid, base.Version, out str);
            if ((str != null) && (str.Length > 0))
            {
                throw new Exception(str);
            }
            System.IO.File.WriteAllText(modInfoFile, contents);
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
                        this.mPreviewImage50 = Image.FromStream(stream);
                    }
                    if (PreviewImageLoaded != null)
                    {
                        PreviewImageLoaded(this, EventArgs.Empty);
                    }
                    if (Program.Settings.Content.Download.CachePreviewImages)
                    {
                        string path = string.Format(@"{0}\vault preview images\mods\{1}.png", AppDomain.CurrentDomain.BaseDirectory, base.ID);
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        this.mPreviewImage50.Save(path, ImageFormat.Png);
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
                return false;
            }
        }

        public bool CanVersion
        {
            get
            {
                return true;
            }
        }

        public string[] Conflicts
        {
            get
            {
                if ((this.mConflicts == null) || (this.mConflicts.Length < 1))
                {
                    return new string[0];
                }
                return this.mConflicts.Split(",".ToCharArray());
            }
        }

        public override int[] ContentDependencies
        {
            get
            {
                try
                {
                    int[] contentDependencies = base.ContentDependencies;
                    if (((this.Requirements == null) || (this.Requirements.Length < 1)) && ((contentDependencies == null) || (contentDependencies.Length < 1)))
                    {
                        return new int[0];
                    }
                    List<int> list = new List<int>(contentDependencies.Length + this.Requirements.Length);
                    list.AddRange(contentDependencies);
                    foreach (string str in this.Requirements)
                    {
                        if (!CachedModLookups.ContainsKey(str))
                        {
                            Mod mod;
                            if (!new QuazalQuery("GetModByGuid", new object[] { str }).GetObject<Mod>(out mod))
                            {
                                goto Label_00F0;
                            }
                            CachedModLookups[str] = mod;
                        }
                        Mod mod2 = CachedModLookups[str];
                        if (list.IndexOf(mod2.ID) < 0)
                        {
                            list.Add(mod2.ID);
                        }
                    Label_00F0:;
                    }
                    return list.ToArray();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return new int[0];
                }
            }
        }

        public string Copyright
        {
            get
            {
                return this.mCopyright;
            }
            set
            {
                this.mCopyright = value;
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

        public bool Exclusive
        {
            get
            {
                return this.mExclusive;
            }
            set
            {
                this.mExclusive = value;
            }
        }

        public string Guid
        {
            get
            {
                return this.mGuid;
            }
            set
            {
                this.mGuid = value;
            }
        }

        public bool HasPreview
        {
            get
            {
                return this.mHasPreview;
            }
            set
            {
                this.mHasPreview = value;
            }
        }

        public bool IsDirectHTTPDownload
        {
            get
            {
                return false;
            }
        }

        public int ModID
        {
            get
            {
                return this.mModID;
            }
            set
            {
                this.mModID = value;
            }
        }

        public string ModName
        {
            get
            {
                return this.mModName;
            }
            set
            {
                this.mModName = value;
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

        public Image PreviewImage50
        {
            get
            {
                if (this.mPreviewImage50 == null)
                {
                    try
                    {
                        if (!this.HasPreview)
                        {
                            if (Random50 == null)
                            {
                                Random50 = DrawUtil.ResizeImage(Resources.random, 50, 50);
                            }
                            return Random50;
                        }
                        if (Program.Settings.Content.Download.CachePreviewImages && CachedImageLocations.ContainsKey(base.ID))
                        {
                            this.mPreviewImage50 = Image.FromFile(CachedImageLocations[base.ID]);
                        }
                        else
                        {
                            if ((((base.TypeID < 1) || (base.Name == null)) || (base.Name.Length < 1)) || (base.Version < 1))
                            {
                                if (Random50 == null)
                                {
                                    Random50 = DrawUtil.ResizeImage(Resources.random, 50, 50);
                                }
                                return Random50;
                            }
                            this.WS.GetPreviewImageCompleted += new GetPreviewImageCompletedEventHandler(this.WS_GetPreviewImageCompleted);
                            this.WS.GetPreviewImageAsync(AdditionalContent.VaultServerKey, base.ContentType.Name, base.Name, base.Version, System.Guid.NewGuid());
                            if (Random50 == null)
                            {
                                Random50 = DrawUtil.ResizeImage(Resources.random, 50, 50);
                            }
                            return Random50;
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                        if (Random50 == null)
                        {
                            Random50 = DrawUtil.ResizeImage(Resources.random, 50, 50);
                        }
                        return Random50;
                    }
                }
                return this.mPreviewImage50;
            }
        }

        public string[] RequirementNames
        {
            get
            {
                if ((this.mRequirementNames == null) || (this.mRequirementNames.Length < 1))
                {
                    return new string[0];
                }
                return this.mRequirementNames.Split(",".ToCharArray());
            }
        }

        public string[] Requirements
        {
            get
            {
                if ((this.mRequirements == null) || (this.mRequirements.Length < 1))
                {
                    return new string[0];
                }
                return this.mRequirements.Split(",".ToCharArray());
            }
        }

        public string RunImagePath
        {
            get
            {
                return null;
            }
        }

        public string RunTooltip
        {
            get
            {
                return null;
            }
        }

        public bool? UIOnly
        {
            get
            {
                return this.mUIOnly;
            }
            set
            {
                this.mUIOnly = value;
            }
        }

        public string UIOnlyDisplay
        {
            get
            {
                if (this.UIOnly.GetValueOrDefault(false))
                {
                    return Loc.Get("<LOC>Yes");
                }
                return Loc.Get("<LOC>No");
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

