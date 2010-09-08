namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal.Security;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    public class GameItem : MappedObject
    {
        [FieldMap("buffer")]
        private string mBuffer;
        [FieldMap("description")]
        private string mDescription;
        private Hashtable mFileLocs;
        [FieldMap("type")]
        private string mGatheringType;
        [FieldMap("id")]
        private int mId;
        [FieldMap("ip_opt_out")]
        private int mIPOptOut;
        [FieldMap("maxparticipants")]
        private string mMaxParticipants;
        private int mMaxPlayers;
        private bool mNeedsDownload;
        [FieldMap("NumOfPlayers")]
        private string mNumberOfPlayers;
        [FieldMap("ownerpid")]
        private int mOwnerID;
        [FieldMap("PlayerName")]
        private string mPlayerName;
        private string mSize;
        [FieldMap("start")]
        private string mStartTime;
        [FieldMap("state")]
        private string mState;
        [FieldMap("url")]
        private string mURL;
        private int mVersion;
        private static Hashtable sMapImages = Hashtable.Synchronized(new Hashtable());
        private static string sSupcomExe;
        private static string sSupcomPath;

        public GameItem(DataRecord record) : base(record)
        {
            this.mFileLocs = new Hashtable();
            this.mVersion = -1;
            this.mSize = "Unknown";
            this.mMaxPlayers = -1;
        }

        public static string CustomMapDir()
        {
            string str = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\My Games\Gas Powered Games\";
            if (GPGnetSelectedGame.SelectedGame.GameID == 2)
            {
                return (str + @"SupremeCommander\");
            }
            if (GPGnetSelectedGame.SelectedGame.GameID == 0x12)
            {
                return (str + @"Supreme Commander Forged Alliance\");
            }
            if (GPGnetSelectedGame.SelectedGame.GameID == 0x11)
            {
                return (str + @"Supreme Commander Forged Alliance\");
            }
            return (str + GPGnetSelectedGame.SelectedGame.ExeName.Replace(".exe", "") + @"\");
        }

        private string GetFileLocation()
        {
            try
            {
                if (this.mFileLocs.ContainsKey(this.GetMapID()))
                {
                    return this.mFileLocs[this.GetMapID()].ToString();
                }
                string supcomPath = SupcomPath;
                supcomPath = GPGnetSelectedGame.SelectedGame.GameLocation.Replace(@"\bin\" + GPGnetSelectedGame.SelectedGame.ExeName, @"\maps\");
                FileInfo info = null;
                foreach (string str2 in Directory.GetFiles(supcomPath, this.GetMapID() + "*.scmap", SearchOption.AllDirectories))
                {
                    FileInfo info2 = new FileInfo(str2);
                    if (info == null)
                    {
                        info = info2;
                    }
                    else if (info.CreationTime < info2.CreationTime)
                    {
                        info = info2;
                    }
                }
                try
                {
                    foreach (string str3 in Directory.GetFiles(CustomMapDir(), this.GetMapID() + "*.scmap", SearchOption.AllDirectories))
                    {
                        FileInfo info3 = new FileInfo(str3);
                        if ((this.mVersion > 0) && (info3.FullName.ToUpper().IndexOf(this.GetMapID().ToUpper() + ".V" + this.mVersion.ToString().PadLeft(4, '0')) >= 0))
                        {
                            info = info3;
                            goto Label_0172;
                        }
                        if (info == null)
                        {
                            info = info3;
                        }
                        else if (info.CreationTime < info3.CreationTime)
                        {
                            info = info3;
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            Label_0172:
                if (info != null)
                {
                    if (this.mVersion >= 0)
                    {
                        string str4 = "V" + this.mVersion.ToString().PadLeft(4, '0');
                        if (info.FullName.ToUpper().IndexOf(str4) < 0)
                        {
                            this.mNeedsDownload = true;
                            return "";
                        }
                        this.mFileLocs.Add(this.GetMapID(), info.FullName);
                        return info.FullName;
                    }
                    this.mFileLocs.Add(this.GetMapID(), info.FullName);
                    return info.FullName;
                }
                this.mFileLocs.Add(this.GetMapID(), "");
                return "";
            }
            catch (Exception exception2)
            {
                ErrorLog.WriteLine(SupcomPath, new object[0]);
                ErrorLog.WriteLine(exception2);
                return "";
            }
        }

        private string GetMapID()
        {
            string str = "";
            if (this.mBuffer != null)
            {
                foreach (string str2 in this.mBuffer.Split(new char[] { '\\' }))
                {
                    if (str2.IndexOf("SCMP") >= 0)
                    {
                        str = str2.Replace("x00", "");
                        if (str.ToUpper().IndexOf(".V") < 0)
                        {
                            return str;
                        }
                        string[] strArray2 = str.Split(".".ToCharArray());
                        if (strArray2.Length == 2)
                        {
                            try
                            {
                                string str3 = strArray2[1];
                                str3 = str3.ToLower().Replace("v", "");
                                this.mVersion = Convert.ToInt32(str3);
                            }
                            catch
                            {
                            }
                            return strArray2[0];
                        }
                    }
                    else if (str2.IndexOf("_scenario.lua") >= 0)
                    {
                        string[] strArray3 = str2.Split("./".ToCharArray());
                        try
                        {
                            string str4 = strArray3[strArray3.Length - 3];
                            str4 = str4.ToLower().Replace("v", "");
                            this.mVersion = Convert.ToInt32(str4);
                        }
                        catch
                        {
                        }
                        return strArray3[strArray3.Length - 2].ToLower().Replace("_scenario", "");
                    }
                }
            }
            return str;
        }

        private string GetName()
        {
            string str = "";
            string mapID = this.GetMapID();
            string path = this.GetFileLocation().Replace(".scmap", "_scenario.lua");
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                while (reader.Peek() >= 0)
                {
                    string str4 = reader.ReadLine().Replace(" ", "");
                    if ((str4.IndexOf("name=") >= 0) && (str4.ToUpper().IndexOf("ARMIES") < 0))
                    {
                        str = str4.Replace("name=", "").Replace("\"", "").Trim();
                    }
                }
            }
            if (str == "")
            {
                str = mapID;
                this.mNeedsDownload = true;
            }
            MapNameLookup.Add(mapID, str);
            return str;
        }

        private bool IsRegularRankedGame()
        {
            return ((ConfigSettings.GetString("Automatch GameIDs", "27") + " ").IndexOf(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " ") >= 0);
        }

        private Image LocateImage()
        {
            string mapID = this.GetMapID();
            if (sMapImages.ContainsKey(mapID))
            {
                return (sMapImages[mapID] as Image);
            }
            string fileLocation = this.GetFileLocation();
            if (!File.Exists(fileLocation))
            {
                return null;
            }
            Bitmap bitmap = new Bitmap(0x100, 0x100);
            FileStream input = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000);
            BinaryReader reader = new BinaryReader(input);
            try
            {
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
            }
            catch (Exception)
            {
            }
            sMapImages.Add(mapID, bitmap);
            return bitmap;
        }

        public string Buffer
        {
            get
            {
                return this.mBuffer;
            }
            set
            {
                this.mBuffer = value;
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

        public string GatheringType
        {
            get
            {
                return this.mGatheringType;
            }
            set
            {
                this.mGatheringType = value;
            }
        }

        public bool HasPassword
        {
            get
            {
                return (this.Password != "");
            }
        }

        public int Id
        {
            get
            {
                return this.mId;
            }
            set
            {
                this.mId = value;
            }
        }

        public int IPOptOut
        {
            get
            {
                return this.mIPOptOut;
            }
            set
            {
                this.mIPOptOut = value;
            }
        }

        public Image MapImage
        {
            get
            {
                Image image = this.LocateImage();
                if (image == null)
                {
                    return new Bitmap(GPGnetSelectedGame.SelectedGame.GameIcon);
                }
                return image;
            }
        }

        public string MapName
        {
            get
            {
                string mapID = this.GetMapID();
                if (MapNameLookup.ContainsKey(mapID))
                {
                    return MapNameLookup[mapID];
                }
                return this.GetName();
            }
        }

        public static Dictionary<string, string> MapNameLookup
        {
            get
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                dictionary.Add("SCMP_001", "Burial Mounds");
                dictionary.Add("SCMP_002", "Concord Lake");
                dictionary.Add("SCMP_003", "Drakes Ravine");
                dictionary.Add("SCMP_004", "Emerald Crater");
                dictionary.Add("SCMP_005", "Gentlemans Reef");
                dictionary.Add("SCMP_006", "Ians Cross");
                dictionary.Add("SCMP_007", "Open Palms");
                dictionary.Add("SCMP_008", "Seraphim Glaciers");
                dictionary.Add("SCMP_009", "Setons Clutch");
                dictionary.Add("SCMP_010", "Sung Island");
                dictionary.Add("SCMP_011", "The Great Void");
                dictionary.Add("SCMP_012", "Theta Passage");
                dictionary.Add("SCMP_013", "Winter Duel");
                dictionary.Add("SCMP_014", "The Bermuda Locket");
                dictionary.Add("SCMP_015", "Fields of Isis");
                dictionary.Add("SCMP_016", "Canis River");
                dictionary.Add("SCMP_017", "Syrtis Major");
                dictionary.Add("SCMP_018", "Sentry Point");
                dictionary.Add("SCMP_019", "Finns Revenge");
                dictionary.Add("SCMP_020", "Roanoke Abyss");
                dictionary.Add("SCMP_021", "Alpha 7 Quarantine");
                dictionary.Add("SCMP_022", "Arctic Refuge");
                dictionary.Add("SCMP_023", "Varga Pass");
                dictionary.Add("SCMP_024", "Crossfire Canal");
                dictionary.Add("SCMP_025", "Saltrock Colony");
                dictionary.Add("SCMP_026", "Vya-3 Protectorate");
                dictionary.Add("SCMP_027", "The Scar");
                dictionary.Add("SCMP_028", "Hanna Oasis");
                dictionary.Add("SCMP_029", "Betrayal Ocean");
                dictionary.Add("SCMP_030", "Frostmill Ruins");
                dictionary.Add("SCMP_031", "Four-Leaf Clover");
                dictionary.Add("SCMP_032", "The Wilderness");
                dictionary.Add("SCMP_033", "White Fire");
                dictionary.Add("SCMP_034", "High Noon");
                dictionary.Add("SCMP_035", "Paradise");
                dictionary.Add("SCMP_036", "Blasted Rock");
                dictionary.Add("SCMP_037", "Sludge");
                dictionary.Add("SCMP_038", "Ambush Pass");
                dictionary.Add("SCMP_039", "Four-Corners");
                dictionary.Add("SCMP_040", "The Ditch");
                foreach (string str in ConfigSettings.GetString("AdditionalMaps", "").Split(new char[] { ';' }))
                {
                    string[] strArray = str.Split(new char[] { ',' });
                    if (strArray.Length == 2)
                    {
                        dictionary.Add(strArray[0], strArray[1]);
                    }
                }
                return dictionary;
            }
        }

        public string MaxParticipants
        {
            get
            {
                return this.mMaxParticipants;
            }
            set
            {
                this.mMaxParticipants = value;
            }
        }

        public int MaxPlayers
        {
            get
            {
                if (this.mMaxPlayers == -1)
                {
                    this.GetMapID();
                    string path = this.GetFileLocation().Replace(".scmap", "_scenario.lua");
                    if (File.Exists(path))
                    {
                        StreamReader reader = new StreamReader(path);
                        while (reader.Peek() >= 0)
                        {
                            string str2 = reader.ReadLine();
                            if (str2.IndexOf("ARMY_1") >= 0)
                            {
                                this.mMaxPlayers = 1;
                            }
                            if (str2.IndexOf("ARMY_2") >= 0)
                            {
                                this.mMaxPlayers = 2;
                            }
                            if (str2.IndexOf("ARMY_3") >= 0)
                            {
                                this.mMaxPlayers = 3;
                            }
                            if (str2.IndexOf("ARMY_4") >= 0)
                            {
                                this.mMaxPlayers = 4;
                            }
                            if (str2.IndexOf("ARMY_5") >= 0)
                            {
                                this.mMaxPlayers = 5;
                            }
                            if (str2.IndexOf("ARMY_6") >= 0)
                            {
                                this.mMaxPlayers = 6;
                            }
                            if (str2.IndexOf("ARMY_7") >= 0)
                            {
                                this.mMaxPlayers = 7;
                            }
                            if (str2.IndexOf("ARMY_8") >= 0)
                            {
                                this.mMaxPlayers = 8;
                            }
                        }
                    }
                }
                if (this.mMaxPlayers == -1)
                {
                    this.mMaxPlayers = 8;
                    if (this.IsRegularRankedGame())
                    {
                        if (GPGnetSelectedGame.SelectedGame.GameID == 0x1b)
                        {
                            if (AccessControlList.GetByName("TournamentDirectors").FindMember(this.OwnerID) != null)
                            {
                                this.mMaxPlayers = 100;
                            }
                        }
                        else
                        {
                            this.mMaxPlayers = ConfigSettings.GetInt(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " MaxPlayers", 100);
                        }
                    }
                }
                return this.mMaxPlayers;
            }
        }

        public bool NeedsDownload
        {
            get
            {
                return this.mNeedsDownload;
            }
            set
            {
                this.mNeedsDownload = value;
            }
        }

        public string NumberOfPlayers
        {
            get
            {
                return this.mNumberOfPlayers;
            }
            set
            {
                this.mNumberOfPlayers = value;
            }
        }

        public string NumPlayers
        {
            get
            {
                return (this.NumberOfPlayers + "/" + this.MaxPlayers.ToString());
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

        public string Password
        {
            get
            {
                foreach (string str in this.mBuffer.Split(new char[] { '\\' }))
                {
                    if (str.IndexOf("x00password=") >= 0)
                    {
                        return str.Replace("x00password=", "");
                    }
                }
                return "";
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
            set
            {
                this.mPlayerName = value;
            }
        }

        public string Size
        {
            get
            {
                if (this.mSize == "Unknown")
                {
                    this.GetMapID();
                    string path = this.GetFileLocation().Replace(".scmap", "_scenario.lua");
                    if (File.Exists(path))
                    {
                        StreamReader reader = new StreamReader(path);
                        while (reader.Peek() >= 0)
                        {
                            string str2 = reader.ReadLine().Replace(" ", "");
                            if (str2.IndexOf("size={") >= 0)
                            {
                                this.mSize = str2.Replace("size={", "").Replace("},", "").Replace("}", "").Trim();
                            }
                        }
                    }
                }
                if (this.mSize == "Unknown")
                {
                    this.mSize = "";
                }
                return this.mSize;
            }
        }

        public string StartTime
        {
            get
            {
                return this.mStartTime;
            }
            set
            {
                this.mStartTime = value;
            }
        }

        public string State
        {
            get
            {
                return this.mState;
            }
            set
            {
                this.mState = value;
            }
        }

        public static string SupcomExe
        {
            get
            {
                return sSupcomExe;
            }
            set
            {
                if ((value != null) && (value != ""))
                {
                    sSupcomExe = value;
                }
            }
        }

        public static string SupcomPath
        {
            get
            {
                return sSupcomPath;
            }
            set
            {
                if (((value != null) && (value != "")) && File.Exists(value))
                {
                    sSupcomPath = value;
                }
            }
        }

        public string URL
        {
            get
            {
                return this.mURL;
            }
            set
            {
                this.mURL = value;
            }
        }

        public int Version
        {
            get
            {
                return this.mVersion;
            }
        }
    }
}

