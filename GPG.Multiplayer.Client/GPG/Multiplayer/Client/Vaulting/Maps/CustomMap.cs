namespace GPG.Multiplayer.Client.Vaulting.Maps
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games;
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
    public class CustomMap : AdditionalContent, IAdditionalContent, IContentTypeProvider
    {
        [NonSerialized]
        private Service _WS = new Service();
        [NonSerialized]
        private static GridView BoundGridView = null;
        private static Dictionary<int, string> CachedImageLocations = new Dictionary<int, string>();
        [NonSerialized]
        private static DlgPreviewMap CurrentPreview = null;
        [NonSerialized]
        private static DateTime LastPreviewClose;
        [FieldMap("has_custom_ruleset")]
        private bool? mHasCustomRuleset = null;
        [FieldMap("has_water")]
        private bool? mHasWater = null;
        [FieldMap("height")]
        private int mHeight;
        [FieldMap("is_mission")]
        private bool? mIsMission = null;
        [FieldMap("is_rush")]
        private bool? mIsRushMap = null;
        [FieldMap("is_separated")]
        private bool? mIsSeparated = null;
        [FieldMap("map_description")]
        private string mMapDescription = null;
        [FieldMap("map_name")]
        private string mMapName;
        private string mMaxPlayerComparisonType = "=";
        [FieldMap("max_players")]
        private int mMaxPlayers;
        private string mNonPackagedFiles = ConfigSettings.GetString("VaultNonUploadFiles", ".exe,.bat,preview.png");
        private Image mPreviewImage128 = null;
        private Image mPreviewImage256 = null;
        private Image mPreviewImage50 = null;
        [FieldMap("size")]
        private int mSize;
        private string mSizeComparisonType = "=";
        [FieldMap("terrain_type")]
        private string mTerrainType;
        [FieldMap("width")]
        private int mWidth;
        [NonSerialized]
        private static Dictionary<int, Rectangle> PreviewCellBounds = new Dictionary<int, Rectangle>();

        [field: NonSerialized]
        public static  event EventHandler PreviewImageLoaded;

        static CustomMap()
        {
            try
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\vault preview images"))
                {
                    if (Program.Settings.Content.Download.CachePreviewImages)
                    {
                        string[] strArray = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\vault preview images", "*.png", SearchOption.TopDirectoryOnly);
                        foreach (string str in strArray)
                        {
                            int num;
                            if (int.TryParse(Path.GetFileNameWithoutExtension(str), out num))
                            {
                                CachedImageLocations[num] = str;
                            }
                        }
                    }
                    else
                    {
                        Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\vault preview images", true);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private CustomMap()
        {

        }

        public CustomMap(DataRecord record) : base(record)
        {
        }

        public IAdditionalContent CreateEmptyInstance()
        {
            CustomMap map = new CustomMap();
            map.ContentType = base.ContentType;
            return map;
        }

        public QuazalQuery CreateSearchQuery(int page, int pageSize)
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return QuazalQuery.Empty;
            }
            try
            {
                int num = DataAccess.FormatBool(this.HasWater);
                int num2 = DataAccess.FormatBool(this.IsSeparated);
                int num3 = DataAccess.FormatBool(this.IsRushMap);
                int num4 = DataAccess.FormatBool(this.HasCustomRuleset);
                int num5 = DataAccess.FormatBool(this.IsMission);
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
                list2.Add(this.Size);
                list2.Add(this.SizeComparisonType);
                list2.Add(this.Size);
                list2.Add(this.MaxPlayers);
                list2.Add(this.MaxPlayerComparisonType);
                list2.Add(this.MaxPlayers);
                list2.Add(this.TerrainType);
                list2.Add("%" + this.TerrainType + "%");
                list2.Add(num);
                list2.Add(num);
                list2.Add(num2);
                list2.Add(num2);
                list2.Add(num3);
                list2.Add(num3);
                list2.Add(num4);
                list2.Add(num4);
                list2.Add(num5);
                list2.Add(num5);
                list2.Add(page * pageSize);
                list2.Add(pageSize);
                return new QuazalQuery(ConfigSettings.GetString("CustomMapSearch", "SearchCustomMaps"), list2.ToArray());
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return QuazalQuery.Empty;
            }
        }

        public string CreateUploadFile()
        {
            string str12;
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return null;
            }
            try
            {
                string extension = Path.GetExtension(base.LocalFilePath);
                string path = Path.GetTempPath() + Guid.NewGuid().ToString();
                Directory.CreateDirectory(path);
                if (!Path.HasExtension(base.LocalFilePath))
                {
                    foreach (string str3 in Directory.GetFiles(base.LocalFilePath))
                    {
                        System.IO.File.Copy(str3, Path.Combine(path, Path.GetFileName(str3)));
                    }
                }
                else
                {
                    switch (extension)
                    {
                        case ".scd":
                        case ".zip":
                        {
                            string str5 = Path.GetTempPath() + Guid.NewGuid().ToString();
                            if (!Directory.Exists(str5))
                            {
                                Directory.CreateDirectory(str5);
                            }
                            Compression.Unzip(base.LocalFilePath, path);
                            break;
                        }
                    }
                }
                bool flag = false;
                foreach (string str3 in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    FileUtil.RemoveReadOnly(str3);
                    if (str3.EndsWith(".scmap"))
                    {
                        flag = true;
                    }
                }
                List<string> list = new List<string>();
                foreach (string str3 in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    string str6;
                    if (str3.EndsWith("_scenario.lua"))
                    {
                        string str7;
                        str6 = Path.Combine(path, this.MapName + "_scenario.lua");
                        if (!this.ValidateScenarioFile(str3, out str7))
                        {
                            throw new Exception(Loc.Get("<LOC>Error validating map for upload. Check your GPGnet log or run the map diagnostics tool for help."));
                        }
                        System.IO.File.WriteAllText(str6, str7);
                        list.Add(str6);
                        if (!flag)
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(LuaUtil.ScenarioToXml(str3));
                            string innerText = document["scenario"]["map"].InnerText;
                            if (((GameInformation.SelectedGame.GameLocation == null) || (GameInformation.SelectedGame.GameLocation.Length < 1)) && !Program.MainForm.LocateExe("SupremeCommander", true))
                            {
                                if (Directory.Exists(path))
                                {
                                    Directory.Delete(path, true);
                                }
                                throw new Exception(Loc.Get("<LOC>The selected map requires a Supreme Commander resource that could not be located."));
                            }
                            innerText = string.Format(@"{0}\..\{1}", Path.GetDirectoryName(GameInformation.SelectedGame.GameLocation), innerText.Replace("/", @"\"));
                            string destFileName = Path.Combine(path, this.MapName + ".scmap");
                            System.IO.File.Copy(innerText, destFileName);
                            FileUtil.RemoveReadOnly(destFileName);
                            list.Add(destFileName);
                        }
                    }
                    else if (str3.EndsWith("_script.lua"))
                    {
                        str6 = Path.Combine(path, this.MapName + "_script.lua");
                        System.IO.File.WriteAllText(str6, this.ValidateScriptFile(str3));
                        list.Add(str6);
                    }
                    else if (str3.EndsWith("_save.lua"))
                    {
                        if (Path.GetFileName(str3) != (this.MapName + "_save.lua"))
                        {
                            System.IO.File.Move(str3, Path.Combine(Path.GetDirectoryName(str3), this.MapName + "_save.lua"));
                        }
                        list.Add(Path.Combine(Path.GetDirectoryName(str3), this.MapName + "_save.lua"));
                    }
                    else if (str3.EndsWith(".scmap"))
                    {
                        if (Path.GetFileName(str3) != (this.MapName + ".scmap"))
                        {
                            System.IO.File.Move(str3, Path.Combine(Path.GetDirectoryName(str3), this.MapName + ".scmap"));
                        }
                        list.Add(Path.Combine(Path.GetDirectoryName(str3), this.MapName + ".scmap"));
                        flag = true;
                    }
                    else if (!this.IsPackagedFile(str3))
                    {
                        ErrorLog.WriteLine("Attempted to package non-uploadable file: {0}.", new object[] { str3 });
                    }
                    else
                    {
                        list.Add(str3);
                    }
                }
                if (this.PreviewImage256 != null)
                {
                    string filename = path + @"\preview.png";
                    this.PreviewImage256.Save(filename, ImageFormat.Png);
                    list.Add(filename);
                }
                string tempFileName = Path.GetTempFileName();
                Compression.ZipFiles(tempFileName, null, list.ToArray(), true);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                if (!System.IO.File.Exists(tempFileName))
                {
                    return null;
                }
                str12 = tempFileName;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                throw exception;
            }
            return str12;
        }

        public QuazalQuery CreateVersionQuery()
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return QuazalQuery.Empty;
            }
            return new QuazalQuery("GetMapHistory", new object[] { base.Name });
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
                    CustomMap map = sender as CustomMap;
                    try
                    {
                        IAdditionalContent[] dataSource = BoundGridView.DataSource as IAdditionalContent[];
                        if ((dataSource != null) && (dataSource.Length >= 1))
                        {
                            if (dataSource[0].TypeID != map.TypeID)
                            {
                                BoundGridView = null;
                            }
                            else
                            {
                                VGen0 method = null;
                                for (int i = 0; i < dataSource.Length; i++)
                                {
                                    if (dataSource[i].ID == map.ID)
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
                string downloadPath = this.GetDownloadPath();
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
                return new QuazalQuery("DeleteCustomMapVersions", new object[] { base.TypeID, base.Name }).ExecuteNonQuery();
            }
            return new QuazalQuery("DeleteCustomMap", new object[] { base.ID }).ExecuteNonQuery();
        }

        public static Image ExtractImage(string luaFile, string mappath)
        {
            FileStream input = null;
            Image image;
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(luaFile), Path.GetFileName(mappath));
                if (System.IO.File.Exists(path))
                {
                    input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000);
                }
                if (((GameInformation.SelectedGame.GameLocation == null) || (GameInformation.SelectedGame.GameLocation.Length < 1)) && !Program.MainForm.LocateExe("SupremeCommander", true))
                {
                    return null;
                }
                mappath = GameInformation.SelectedGame.GameLocation.Replace(@"\bin\" + Program.Settings.SupcomPrefs.GameExe, mappath.Replace("/", @"\"));
                if (System.IO.File.Exists(mappath))
                {
                    input = new FileStream(mappath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000);
                }
                if (input == null)
                {
                    return null;
                }
                Bitmap bitmap = new Bitmap(0x100, 0x100);
                BinaryReader reader = new BinaryReader(input);
                byte[] buffer = new byte[0x40000];
                int index = 0;
                reader.ReadBytes(0xa5);
                while (index < 0x40000)
                {
                    reader.Read(buffer, index, 0x1000);
                    index += 0x1000;
                }
                index = 0;
                int x = 0;
                int y = 0;
                while (index < 0x40000)
                {
                    Color color = Color.FromArgb(buffer[index], buffer[index + 3], buffer[index + 2], buffer[index + 1]);
                    bitmap.SetPixel(x, y, color);
                    x++;
                    if (x > 0xff)
                    {
                        x = 0;
                        y++;
                    }
                    index += 4;
                }
                image = bitmap;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                image = null;
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
            }
            return image;
        }

        public static Image ExtractZippedImage(string zipFile, string mappath)
        {
            InputStream stream = null;
            ZipFile file = null;
            Image image;
            try
            {
                file = new ZipFile(zipFile);
                Enumeration enumeration = file.entries();
                List<ZipEntry> list = new List<ZipEntry>();
                while (enumeration.hasMoreElements())
                {
                    list.Add(enumeration.nextElement() as ZipEntry);
                }
                foreach (ZipEntry entry in list)
                {
                    if (entry.getName().EndsWith(Path.GetFileName(mappath)))
                    {
                        stream = file.getInputStream(entry);
                        break;
                    }
                }
                if (stream == null)
                {
                    return ExtractImage(zipFile, mappath);
                }
                Bitmap bitmap = new Bitmap(0x100, 0x100);
                byte[] buffer = new byte[0x40000];
                int index = 0;
                stream.read(new sbyte[0xa5], 0, 0xa5);
                while (index < 0x40000)
                {
                    for (int i = 0; i < 0x1000; i++)
                    {
                        buffer[index + i] = (byte) stream.read();
                    }
                    index += 0x1000;
                }
                index = 0;
                int x = 0;
                int y = 0;
                while (index < 0x40000)
                {
                    Color color = Color.FromArgb(buffer[index], buffer[index + 3], buffer[index + 2], buffer[index + 1]);
                    bitmap.SetPixel(x, y, color);
                    x++;
                    if (x > 0xff)
                    {
                        x = 0;
                        y++;
                    }
                    index += 4;
                }
                image = bitmap;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                image = null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.close();
                }
                if (file != null)
                {
                    file.close();
                }
            }
            return image;
        }

        public bool FromID(int contentId, out IAdditionalContent content)
        {
            CustomMap map;
            if (!base.ContentType.CurrentUserCanUpload)
            {
                content = null;
                return false;
            }
            bool flag = new QuazalQuery("GetMapById", new object[] { contentId }).GetObject<CustomMap>(out map);
            content = map;
            return flag;
        }

        public bool FromLocalFile(string file, out IAdditionalContent content)
        {
            Exception exception;
            if (!base.ContentType.CurrentUserCanUpload)
            {
                content = null;
                return false;
            }
            try
            {
                byte[] buffer;
                int num2;
                string extension = Path.GetExtension(file);
                string contents = "";
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
                            bool flag = false;
                            foreach (ZipEntry entry in list)
                            {
                                if (entry.getName().EndsWith("_scenario.lua"))
                                {
                                    string fileName = Path.GetFileName(entry.getName());
                                    this.mMapName = fileName.Remove(fileName.IndexOf("_scenario.lua"), "_scenario.lua".Length);
                                    InputStream stream = null;
                                    try
                                    {
                                        string str5;
                                        stream = file2.getInputStream(entry);
                                        buffer = new byte[0x1000];
                                        bool flag2 = false;
                                        while (!flag2)
                                        {
                                            num2 = 0;
                                            while (num2 < buffer.Length)
                                            {
                                                int num = stream.read();
                                                if (num < 0)
                                                {
                                                    flag2 = true;
                                                    break;
                                                }
                                                buffer[num2] = (byte) num;
                                                num2++;
                                            }
                                            contents = contents + Encoding.UTF8.GetString(buffer, 0, num2);
                                        }
                                        string tempFileName = Path.GetTempFileName();
                                        System.IO.File.WriteAllText(tempFileName, contents);
                                        if (!LuaUtil.VerifyScenario(tempFileName, out str5))
                                        {
                                            ErrorLog.WriteLine("Error loading custom map file {0}: {1}", new object[] { Path.GetFileName(tempFileName), str5 });
                                            content = null;
                                            return false;
                                        }
                                        new XmlDocument().LoadXml(LuaUtil.ScenarioToXml(tempFileName));
                                        System.IO.File.Delete(tempFileName);
                                        base.LocalFilePath = file;
                                        flag = true;
                                        break;
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
                            if (!flag)
                            {
                                content = null;
                                return false;
                            }
                        }
                        finally
                        {
                            file2.close();
                        }
                        goto Label_03C3;
                    }
                    default:
                    {
                        if (!(extension == ".lua"))
                        {
                            goto Label_03C3;
                        }
                        if (!file.EndsWith("_scenario.lua"))
                        {
                            content = null;
                            return false;
                        }
                        this.mMapName = Path.GetFileName(file);
                        this.mMapName = this.mMapName.Remove(this.mMapName.IndexOf("_scenario.lua"), "_scenario.lua".Length);
                        document = new XmlDocument();
                        document.LoadXml(LuaUtil.ScenarioToXml(file));
                        FileStream stream2 = null;
                        for (num2 = 0; num2 < 3; num2++)
                        {
                            try
                            {
                                bool flag4;
                                base.LocalFilePath = Path.GetDirectoryName(file);
                                stream2 = System.IO.File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                                buffer = new byte[0x1000];
                                int count = 0;
                                goto Label_034E;
                            Label_0316:
                                count = stream2.Read(buffer, 0, buffer.Length);
                                if (count < 1)
                                {
                                    break;
                                }
                                contents = contents + Encoding.UTF8.GetString(buffer, 0, count);
                            Label_034E:
                                flag4 = true;
                                goto Label_0316;
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                ErrorLog.WriteLine(exception);
                                Thread.Sleep(0x3e8);
                            }
                            finally
                            {
                                if (stream2 != null)
                                {
                                    stream2.Close();
                                }
                            }
                        }
                        break;
                    }
                }
                if ((contents == null) || (contents.Length < 1))
                {
                    content = null;
                    return false;
                }
            Label_03C3:
                if (document == null)
                {
                    content = null;
                    return false;
                }
                base.Name = document["scenario"]["name"].InnerText;
                this.mMapDescription = document["scenario"]["description"].InnerText;
                this.mWidth = int.Parse(document["scenario"]["size"]["width"].InnerText);
                this.mHeight = int.Parse(document["scenario"]["size"]["height"].InnerText);
                this.mSize = (this.Width + this.Height) / 2;
                this.mMaxPlayers = int.Parse(document["scenario"]["max_players"].InnerText);
                string innerText = document["scenario"]["map"].InnerText;
                if ((extension == ".scd") || (extension == ".zip"))
                {
                    this.mPreviewImage256 = ExtractZippedImage(file, innerText);
                }
                else
                {
                    this.mPreviewImage256 = ExtractImage(file, innerText);
                }
                this.mPreviewImage50 = null;
                this.mPreviewImage128 = null;
                content = this;
                return true;
            }
            catch (Exception exception2)
            {
                exception = exception2;
                ErrorLog.WriteLine(exception);
                content = null;
                return false;
            }
        }

        public ContentOptions GetDetailsView()
        {
            return new ContentOptions(Loc.Get("<LOC>Map Info"), new PnlMapDetailsView(this));
        }

        public string GetDirectURL()
        {
            return "";
        }

        public ContentOptions GetDownloadOptions()
        {
            return new ContentOptions(Loc.Get("<LOC>Map Criteria"), new PnlMapDownloadOptions(this));
        }

        public string GetDownloadPath()
        {
            string str = base.Version.ToString();
            str = "v" + str.PadLeft(5 - str.Length, '0');
            return string.Format(@"{0}\{1}.{2}", base.ContentType.DownloadPath, this.MapName, str);
        }

        public IAdditionalContent[] GetMyDownloads()
        {
            if (!base.ContentType.CurrentUserCanDownload)
            {
                return new IAdditionalContent[0];
            }
            QuazalQuery query = new QuazalQuery("GetMyDownloadedMaps", new object[0]);
            return query.GetObjects<CustomMap>().ToArray();
        }

        public IAdditionalContent[] GetMyUploads()
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return new IAdditionalContent[0];
            }
            MappedObjectList<CustomMap> objects = new QuazalQuery("GetMyUploadedMaps", new object[0]).GetObjects<CustomMap>();
            if (objects == null)
            {
                return new CustomMap[0];
            }
            return objects.ToArray();
        }

        public ContentOptions GetUploadOptions()
        {
            if (!base.ContentType.CurrentUserCanUpload)
            {
                return ContentOptions.None;
            }
            return new ContentOptions(Loc.Get("<LOC>Map Upload Options"), new PnlMapUploadOptions(this));
        }

        public bool IsContentUnique()
        {
            return !new QuazalQuery("DoesMapExist", new object[] { base.TypeID, base.Name, this.MapName }).GetBool();
        }

        private bool IsPackagedFile(string file)
        {
            foreach (string str in this.NonPackagedFiles)
            {
                if (file.EndsWith(str))
                {
                    return false;
                }
            }
            return true;
        }

        public void MergeWithLocalVersion(IAdditionalContent localVersion)
        {
            if (base.ContentType.CurrentUserCanUpload && (localVersion is CustomMap))
            {
                CustomMap map = localVersion as CustomMap;
                base.LocalFilePath = map.LocalFilePath;
                this.mHeight = map.Height;
                this.mMaxPlayers = map.MaxPlayers;
                this.mPreviewImage256 = map.mPreviewImage256;
                this.mPreviewImage50 = null;
                this.mPreviewImage128 = null;
                this.mSize = map.Size;
                this.mWidth = map.Width;
                this.MapDescription = map.MapDescription;
                if (base.ID < 1)
                {
                    base.Name = map.Name;
                }
            }
        }

        public void Run()
        {
            Program.MainForm.HostGame();
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
                string path = this.GetDownloadPath() + @"\content.partial";
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
                Compression.Unzip(path);
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
                        if ((DateTime.Now - now) > TimeSpan.FromSeconds(5.0))
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
            return DataAccess.ExecuteQuery("UploadCustomMap", new object[] { base.ID, this.Width, this.Height, this.Size, this.MaxPlayers, this.TerrainType, this.HasWater, this.IsSeparated, this.IsRushMap, this.HasCustomRuleset, this.IsMission, this.MapDescription, this.MapName });
        }

        public void SetGridView(GridView view)
        {
            if (base.ContentType.CurrentUserCanDownload)
            {
                BoundGridView = view;
                view.Columns["VersionDate"].Visible = false;
                view.PreviewFieldName = null;
                GridColumn column = new GridColumn();
                column.Caption = Loc.Get("<LOC>Preview");
                column.FieldName = "PreviewImage50";
                column.ColumnEdit = new RepositoryItemPictureEdit();
                view.Columns.Add(column);
                column.Visible = true;
                GridColumn column2 = new GridColumn();
                column2.Caption = Loc.Get("<LOC>Players");
                column2.FieldName = "MaxPlayers";
                column2.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
                column2.AppearanceCell.Options.UseTextOptions = true;
                view.Columns.Add(column2);
                column2.Visible = true;
                GridColumn column3 = new GridColumn();
                column3.Caption = Loc.Get("<LOC>Size");
                column3.FieldName = "SizeDisplay";
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
                MultiVal<string, string>[] valArray = new MultiVal<string, string>[] { new MultiVal<string, string>("<LOC>Terrain Type", "TerrainType"), new MultiVal<string, string>("<LOC>Has Water", "HasWater"), new MultiVal<string, string>("<LOC>Separated", "IsSeparated"), new MultiVal<string, string>("<LOC>Rushing", "IsRushMap"), new MultiVal<string, string>("<LOC>Custom Rules", "HasCustomRuleset"), new MultiVal<string, string>("<LOC>Mission", "IsMission") };
                foreach (MultiVal<string, string> val in valArray)
                {
                    GridColumn column4 = new GridColumn();
                    column4.Caption = Loc.Get(val.Value1);
                    column4.FieldName = val.Value2;
                    column4.Visible = false;
                    view.Columns.Add(column4);
                }
                view.CustomDrawCell -= new RowCellCustomDrawEventHandler(CustomMap.view_CustomDrawCell);
                view.MouseMove -= new MouseEventHandler(CustomMap.view_MouseMove);
                view.CustomDrawCell += new RowCellCustomDrawEventHandler(CustomMap.view_CustomDrawCell);
                view.MouseMove += new MouseEventHandler(CustomMap.view_MouseMove);
                PreviewImageLoaded -= new EventHandler(this.CustomMap_PreviewImageLoaded);
                PreviewImageLoaded += new EventHandler(this.CustomMap_PreviewImageLoaded);
            }
        }

        private void ValidateMapFile(string mapPath, string oldPath, string newPath)
        {
            try
            {
                string directoryName = Path.GetDirectoryName(mapPath);
                string[] resourceFileTypes = this.ResourceFileTypes;
                string[] files = Directory.GetFiles(directoryName);
                bool flag = false;
                foreach (string str2 in files)
                {
                    foreach (string str3 in resourceFileTypes)
                    {
                        if (str2.EndsWith(str3))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    string path = mapPath + ".tmp";
                    oldPath = oldPath.Replace("/" + Path.GetFileName(mapPath), "");
                    newPath = newPath.Replace("/" + Path.GetFileName(mapPath), "");
                    byte[] bytes = Encoding.UTF8.GetBytes(oldPath);
                    FileStream stream = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.None);
                    FileStream stream2 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    try
                    {
                        long num = 0L;
                        int length = bytes.Length;
                        while (num < stream.Length)
                        {
                            stream.Position = num;
                            byte[] buffer = new byte[length * 2];
                            int num3 = stream.Read(buffer, 0, length * 2);
                            bool flag2 = false;
                            for (int i = 0; i < (num3 - length); i++)
                            {
                                if (Encoding.UTF8.GetString(buffer, i, oldPath.Length) == oldPath)
                                {
                                    flag2 = true;
                                    byte[] sourceArray = Encoding.UTF8.GetBytes(newPath);
                                    byte[] destinationArray = new byte[i + sourceArray.Length];
                                    Array.Copy(buffer, destinationArray, i);
                                    Array.Copy(sourceArray, 0, destinationArray, i, sourceArray.Length);
                                    stream2.Write(destinationArray, 0, destinationArray.Length);
                                    stream2.Flush();
                                    num = (num + i) + length;
                                    break;
                                }
                            }
                            if (!flag2)
                            {
                                stream2.Write(buffer, 0, length);
                                stream2.Flush();
                                num += length;
                            }
                        }
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Close();
                        }
                        if (stream2 != null)
                        {
                            stream2.Close();
                        }
                    }
                    System.IO.File.Delete(mapPath);
                    System.IO.File.Move(path, mapPath);
                    if (System.IO.File.Exists(mapPath + ".backup"))
                    {
                        System.IO.File.Delete(mapPath + ".backup");
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private bool ValidateScenarioFile(string scenarioPath, out string result)
        {
            string str;
            if (!LuaUtil.VerifyScenario(scenarioPath, out str))
            {
                ErrorLog.WriteLine("An error occured validating map {0}: {1}", new object[] { Path.GetFileName(scenarioPath), str });
                result = null;
                return false;
            }
            result = LuaUtil.VersionScenario(scenarioPath, base.Version);
            if ((result == null) || (result.Length < 1))
            {
                ErrorLog.WriteLine("An error occured validating map {0}: Versioning of map failed.", new object[] { Path.GetFileName(scenarioPath) });
                result = null;
                return false;
            }
            return true;
        }

        private string ValidateScriptFile(string scriptPath)
        {
            return System.IO.File.ReadAllText(scriptPath).Replace("/lua/ScenarioUtilities.lua", "/lua/sim/ScenarioUtilities.lua").Replace("/lua/modules/ScenarioFramework.lua", "/lua/ScenarioFramework.lua");
        }

        private static void view_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "PreviewImage50")
            {
                CustomMap row = view.GetRow(e.RowHandle) as CustomMap;
                if (row != null)
                {
                    PreviewCellBounds[row.ID] = e.Bounds;
                }
            }
        }

        private static void view_MouseMove(object sender, MouseEventArgs e)
        {
            if ((CurrentPreview != null) && CurrentPreview.Visible)
            {
                if (CurrentPreview.Bounds.Contains(Control.MousePosition))
                {
                    return;
                }
                CurrentPreview.Hide();
            }
            if ((DateTime.Now - LastPreviewClose) >= TimeSpan.FromSeconds(0.4))
            {
                GridView view = sender as GridView;
                if (view != null)
                {
                    GridHitInfo info = view.CalcHitInfo(e.Location);
                    if (info.InRowCell)
                    {
                        CustomMap row = view.GetRow(info.RowHandle) as CustomMap;
                        if ((((row != null) && (row.PreviewImage50 != null)) && (info.Column.FieldName == "PreviewImage50")) && PreviewCellBounds.ContainsKey(row.ID))
                        {
                            Rectangle parent = PreviewCellBounds[row.ID];
                            Rectangle rectangle2 = new Rectangle(DrawUtil.Center(parent, row.PreviewImage50.Size), row.PreviewImage50.Size);
                            if (rectangle2.Contains(e.Location))
                            {
                                if (CurrentPreview == null)
                                {
                                    DlgPreviewMap map2 = new DlgPreviewMap(row);
                                    map2.VisibleChanged += delegate (object s, EventArgs e1) {
                                        if (!((CurrentPreview == null) || CurrentPreview.Visible))
                                        {
                                            LastPreviewClose = DateTime.Now;
                                        }
                                    };
                                    map2.Left = Control.MousePosition.X - 30;
                                    map2.Top = Control.MousePosition.Y - 30;
                                    map2.Show();
                                    CurrentPreview = map2;
                                }
                                else if (!CurrentPreview.Visible)
                                {
                                    CurrentPreview.SetMap(row);
                                    CurrentPreview.Left = Control.MousePosition.X - 30;
                                    CurrentPreview.Top = Control.MousePosition.Y - 30;
                                    CurrentPreview.Show();
                                }
                                CurrentPreview.BringToFront();
                            }
                        }
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
                        this.mPreviewImage128 = null;
                        this.mPreviewImage256 = Image.FromStream(stream);
                    }
                    if (PreviewImageLoaded != null)
                    {
                        PreviewImageLoaded(this, EventArgs.Empty);
                    }
                    if (Program.Settings.Content.Download.CachePreviewImages)
                    {
                        string path = string.Format(@"{0}\vault preview images\{1}.png", AppDomain.CurrentDomain.BaseDirectory, base.ID);
                        if (!Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                        }
                        this.mPreviewImage256.Save(path, ImageFormat.Png);
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
                return base.ContentType.CurrentUserCanUpload;
            }
        }

        public bool? HasCustomRuleset
        {
            get
            {
                return this.mHasCustomRuleset;
            }
            set
            {
                this.mHasCustomRuleset = value;
            }
        }

        public bool? HasWater
        {
            get
            {
                return this.mHasWater;
            }
            set
            {
                this.mHasWater = value;
            }
        }

        public int Height
        {
            get
            {
                return this.mHeight;
            }
        }

        public bool IsDirectHTTPDownload
        {
            get
            {
                return false;
            }
        }

        public bool? IsMission
        {
            get
            {
                return this.mIsMission;
            }
            set
            {
                this.mIsMission = value;
            }
        }

        public bool? IsRushMap
        {
            get
            {
                return this.mIsRushMap;
            }
            set
            {
                this.mIsRushMap = value;
            }
        }

        public bool? IsSeparated
        {
            get
            {
                return this.mIsSeparated;
            }
            set
            {
                this.mIsSeparated = value;
            }
        }

        public string MapDescription
        {
            get
            {
                return this.mMapDescription;
            }
            set
            {
                this.mMapDescription = value;
            }
        }

        public string MapName
        {
            get
            {
                return this.mMapName;
            }
            set
            {
                this.mMapName = value;
            }
        }

        public string MaxPlayerComparisonType
        {
            get
            {
                return this.mMaxPlayerComparisonType;
            }
            set
            {
                this.mMaxPlayerComparisonType = value;
            }
        }

        public int MaxPlayers
        {
            get
            {
                return this.mMaxPlayers;
            }
            set
            {
                this.mMaxPlayers = value;
            }
        }

        public string[] NonPackagedFiles
        {
            get
            {
                return this.mNonPackagedFiles.Split(new char[] { ',' });
            }
        }

        public Image PreviewImage128
        {
            get
            {
                if ((this.mPreviewImage128 == null) && (this.PreviewImage256 != null))
                {
                    this.mPreviewImage128 = DrawUtil.ResizeImage(this.PreviewImage256, 0x80, 0x80);
                }
                return this.mPreviewImage128;
            }
        }

        public Image PreviewImage256
        {
            get
            {
                if (this.mPreviewImage256 == null)
                {
                    try
                    {
                        if (Program.Settings.Content.Download.CachePreviewImages && CachedImageLocations.ContainsKey(base.ID))
                        {
                            this.mPreviewImage256 = Image.FromFile(CachedImageLocations[base.ID]);
                        }
                        else
                        {
                            if ((((base.TypeID >= 1) && (base.Name != null)) && (base.Name.Length >= 1)) && (base.Version >= 1))
                            {
                                this.WS.GetPreviewImageCompleted += new GetPreviewImageCompletedEventHandler(this.WS_GetPreviewImageCompleted);
                                this.WS.GetPreviewImageAsync(AdditionalContent.VaultServerKey, base.ContentType.Name, base.Name, base.Version, Guid.NewGuid());
                            }
                            return Resources.random;
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                        return Resources.random;
                    }
                }
                return this.mPreviewImage256;
            }
        }

        public Image PreviewImage50
        {
            get
            {
                if ((this.mPreviewImage50 == null) && (this.PreviewImage256 != null))
                {
                    this.mPreviewImage50 = DrawUtil.ResizeImage(this.PreviewImage256, 50, 50);
                }
                return this.mPreviewImage50;
            }
        }

        public string[] ResourceFileTypes
        {
            get
            {
                return ConfigSettings.GetString("MapResourceFileTypes", ".dds").Split(new char[] { ',' });
            }
        }

        public string RunImagePath
        {
            get
            {
                return @"Dialog\ContentManager\BtnHost";
            }
        }

        public string RunTooltip
        {
            get
            {
                return "<LOC>Host Game";
            }
        }

        public int Size
        {
            get
            {
                return this.mSize;
            }
            set
            {
                this.mSize = value;
            }
        }

        public string SizeComparisonType
        {
            get
            {
                return this.mSizeComparisonType;
            }
            set
            {
                this.mSizeComparisonType = value;
            }
        }

        public string SizeDisplay
        {
            get
            {
                return string.Format("{0}{1}", (int) Math.Round((double) (((double) (this.Size * 2)) / 100.0)), Loc.Get("<LOC>km"));
            }
        }

        public string TerrainType
        {
            get
            {
                return this.mTerrainType;
            }
            set
            {
                this.mTerrainType = value;
            }
        }

        public int Width
        {
            get
            {
                return this.mWidth;
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

