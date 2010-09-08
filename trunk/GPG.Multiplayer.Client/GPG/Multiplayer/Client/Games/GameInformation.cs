namespace GPG.Multiplayer.Client.Games
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.Quazal;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class GameInformation : IGameInformation
    {
        public static List<GameInformation> Games = new List<GameInformation>();
        public static GameInformation GPGNetChat = null;
        [NonSerialized]
        private bool isInDb = false;
        private string mAppDir = "";
        private string mCDKey = "";
        private string mCommandLineArgs = "";
        private int mCommunicationsMethod;
        private string mCurrentVersion = "";
        private string mExeName;
        private string mForcedCommandLineArgs = "";
        private string mGameDescription = "";
        [NonSerialized]
        private Bitmap mGameIcon = null;
        private int mGameID;
        private string mGameLocation = "";
        private string mIconLabel = "";
        private string mInstallDirectory;
        private string mLongDescription;
        private string mPatcherAppURL;
        private string mPatchURL;
        private int mPort = 0;
        private string mUserForcedCommandLineArgs = "";
        private string mVersionFile = "";
        private static GameInformation sSelectedGame = null;

        public static  event EventHandler OnNewGameLocation;

        public static  event EventHandler OnPathCheck;

        public static  event EventHandler OnSelectedGameChange;

        public static void CheckMissingPaths()
        {
            foreach (GameInformation information in Games)
            {
                string folderPath;
                if ((information.GameLocation == "") || !File.Exists(information.GameLocation))
                {
                    folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    folderPath = folderPath + @"\" + information.GameDescription + @"\" + information.ApplicationDirectory.Replace("/", @"\") + information.ExeName;
                    if (File.Exists(folderPath))
                    {
                        information.GameLocation = folderPath;
                    }
                    else
                    {
                        string str2;
                        RegistryKey key;
                        string str3;
                        if (information.IsSpaceSiege)
                        {
                            str2 = @"Software\SEGA\Gas Powered Games\Space Siege\";
                            key = Registry.CurrentUser.OpenSubKey(str2);
                            if (key == null)
                            {
                                key = Registry.LocalMachine.OpenSubKey(str2);
                            }
                            if (key != null)
                            {
                                str3 = (string) key.GetValue("InstallationDirectory");
                                str3 = str3 + @"\" + information.ExeName;
                                if (File.Exists(str3))
                                {
                                    information.GameLocation = str3;
                                }
                            }
                        }
                        else
                        {
                            str3 = "";
                            string str4 = information.GameDescription.Replace("Forged Alliance", "Supreme Commander - Forged Alliance");
                            str2 = @"Software\THQ\Gas Powered Games\" + str4;
                            key = Registry.CurrentUser.OpenSubKey(str2);
                            if (key == null)
                            {
                                key = Registry.LocalMachine.OpenSubKey(str2);
                            }
                            if (key != null)
                            {
                                str3 = (string) key.GetValue("InstallationDirectory");
                                str3 = str3 + @"\" + str4 + @"\bin\" + information.ExeName;
                                if (File.Exists(str3))
                                {
                                    information.GameLocation = str3;
                                }
                            }
                        }
                    }
                }
                if (information.GameDescription == "Forge")
                {
                    folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    folderPath = folderPath + @"\" + information.GameDescription + @"\" + information.ApplicationDirectory.Replace("/", @"\") + "ForgeR.exe";
                    if (File.Exists(folderPath))
                    {
                        information.GameLocation = folderPath;
                        information.ExeName = "ForgeR.exe";
                        information.VersionFile = "ForgeR.exe";
                    }
                }
            }
            if (OnPathCheck != null)
            {
                OnPathCheck(null, EventArgs.Empty);
            }
        }

        private void DoCompare(GameInformation dbinfo)
        {
            this.GameDescription = dbinfo.GameDescription;
            if ((this.ForcedCommandLineArgs != dbinfo.ForcedCommandLineArgs) && (this.ForcedCommandLineArgs == ""))
            {
                this.ForcedCommandLineArgs = dbinfo.ForcedCommandLineArgs;
                this.UserForcedCommandLineArgs = "";
            }
            this.ApplicationDirectory = dbinfo.ApplicationDirectory;
            this.VersionFile = dbinfo.VersionFile;
            this.CDKey = dbinfo.CDKey;
            this.ExeName = dbinfo.ExeName;
            this.IconLabel = dbinfo.IconLabel;
            this.isInDb = dbinfo.isInDb;
            this.PatchURL = dbinfo.PatchURL;
            this.PatcherAppURL = dbinfo.PatcherAppURL;
            this.CurrentVersion = dbinfo.CurrentVersion;
            this.InstallDirectory = dbinfo.InstallDirectory;
            this.LongDescription = dbinfo.LongDescription;
            this.ForcedCommandLineArgs = dbinfo.ForcedCommandLineArgs;
            this.mGameIcon = null;
            if (this.GameDescription.ToUpper() == "SUPREME COMMANDER")
            {
                this.GameLocation = Program.Settings.SupcomPrefs.GamePath;
            }
            else if (this.GameDescription.ToUpper() == "GPGNET")
            {
                this.GameLocation = Application.ExecutablePath;
            }
        }

        private static void GamesChanged()
        {
            GPGnetSelectedGame.GameList.Clear();
            foreach (GameInformation information in Games)
            {
                GPGnetSelectedGame.GameList.Add(information);
            }
        }

        private static string GetGameCachePath()
        {
            return (Application.StartupPath + @"\gameinfo" + Program.Settings.Login.DefaultServerName + ".gpg");
        }

        private static GameInformation GetSpaceSiegeInfo()
        {
            GameInformation information = new GameInformation();
            information.GameID = 15;
            information.GameLocation = string.Empty;
            information.ExeName = "SpaceSiege.exe";
            information.GameDescription = "Space Siege";
            information.VersionFile = information.ExeName;
            information.isInDb = true;
            string name = @"Software\SEGA\Gas Powered Games\Space Siege\";
            RegistryKey key = Registry.CurrentUser.OpenSubKey(name);
            if (key == null)
            {
                key = Registry.LocalMachine.OpenSubKey(name);
            }
            if (key != null)
            {
                string path = (string) key.GetValue("InstallationDirectory");
                path = path + @"\" + information.ExeName;
                if (File.Exists(path))
                {
                    information.GameLocation = path;
                }
                return information;
            }
            return null;
        }

        private static GameInformation GetSupcomInfo()
        {
            GameInformation information = new GameInformation();
            information.GameID = 2;
            information.GameLocation = Program.Settings.SupcomPrefs.GamePath;
            information.ExeName = "SupremeCommander.exe";
            information.GameDescription = "Supreme Commander";
            information.VersionFile = "MohoEngine.dll";
            information.isInDb = true;
            return information;
        }

        public static void LoadCachedGames()
        {
            Exception exception;
            try
            {
                GameInformation current;
                try
                {
                    if (File.Exists(GetGameCachePath()))
                    {
                        XmlSerializer serializer = new XmlSerializer(Games.GetType());
                        FileStream stream = new FileStream(GetGameCachePath(), FileMode.Open);
                        List<GameInformation> list = serializer.Deserialize(stream) as List<GameInformation>;
                        if (list != null)
                        {
                            Games = list;
                        }
                        stream.Close();
                    }
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                }
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                using (List<GameInformation>.Enumerator enumerator = Games.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.GameID == -1)
                        {
                            SetChatInfo(current);
                            flag = true;
                        }
                        if (current.GameID == 2)
                        {
                            flag2 = true;
                        }
                        if (current.GameID == 15)
                        {
                            flag3 = true;
                        }
                    }
                }
                if (!flag)
                {
                    current = new GameInformation();
                    SetChatInfo(current);
                    Games.Add(current);
                }
                if (!(flag2 || (!(Program.Settings.Login.DefaultServerName == "Retail Server") || !(Program.Settings.Login.DefaultServerName == "Supreme Commander Server"))))
                {
                    Games.Add(GetSupcomInfo());
                }
                if (!(flag3 || (!(Program.Settings.Login.DefaultServerName != "Retail Server") || !(Program.Settings.Login.DefaultServerName != "Supreme Commander Server"))))
                {
                    current = GetSpaceSiegeInfo();
                    if (current != null)
                    {
                        Games.Add(current);
                    }
                }
                CheckMissingPaths();
                GamesChanged();
            }
            catch (Exception exception2)
            {
                exception = exception2;
                ErrorLog.WriteLine(exception);
            }
        }

        public static void LoadGamesFromDB()
        {
            DataList queryData = DataAccess.GetQueryData("Application Games", new object[0]);
            DataList list2 = DataAccess.GetQueryData("Application Keys", new object[0]);
            if (queryData.Count >= 2)
            {
                foreach (DataRecord record in queryData)
                {
                    GameInformation item = new GameInformation();
                    item.GameID = Convert.ToInt32(record["gpgnet_game_id"]);
                    item.GameDescription = record["description"];
                    item.ForcedCommandLineArgs = record["forced_command_line_args"];
                    item.ApplicationDirectory = record["relative_app_directory"];
                    item.VersionFile = record["relative_version_file"];
                    item.ExeName = record["exe_name"];
                    item.IconLabel = record["icon_label"];
                    item.PatcherAppURL = record["patcher_app_url"];
                    item.PatchURL = record["patch_url"];
                    item.CurrentVersion = record["current_build"];
                    item.InstallDirectory = record["install_directory"];
                    item.LongDescription = record["long_description"];
                    item.isInDb = true;
                    foreach (DataRecord record2 in list2)
                    {
                        if (record2["gpgnet_game_id"] == record["gpgnet_game_id"])
                        {
                            item.CDKey = record2["key_chars"];
                        }
                    }
                    GameInformation information2 = null;
                    foreach (GameInformation information3 in Games)
                    {
                        if (information3.GameID.ToString() == record["gpgnet_game_id"])
                        {
                            information2 = information3;
                        }
                    }
                    if (information2 == null)
                    {
                        Games.Add(item);
                    }
                    else
                    {
                        information2.DoCompare(item);
                    }
                }
                CheckMissingPaths();
                for (int i = Games.Count - 1; i >= 0; i--)
                {
                    if (!Games[i].isInDb)
                    {
                        Games.RemoveAt(i);
                    }
                }
                SaveGames();
                GamesChanged();
            }
        }

        public static void SaveGames()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(Games.GetType());
                FileStream stream = new FileStream(GetGameCachePath(), FileMode.Create);
                serializer.Serialize((Stream) stream, Games);
                stream.Close();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private static void SetChatInfo(GameInformation info)
        {
            info.GameID = -1;
            info.GameDescription = Loc.Get("<LOC>GPGnet (Chat Only)");
            info.CDKey = "NOTNEEDED";
            info.GameLocation = Application.ExecutablePath;
            info.isInDb = true;
            GPGNetChat = info;
        }

        public static void SetServerSelectedGame()
        {
            DataAccess.ExecuteQuery("SetCurrentGame", new object[] { sSelectedGame.GameID });
        }

        public override string ToString()
        {
            return this.GameDescription;
        }

        public string ApplicationDirectory
        {
            get
            {
                if (this.mAppDir == null)
                {
                    this.mAppDir = "";
                }
                return this.mAppDir;
            }
            set
            {
                this.mAppDir = value;
            }
        }

        public string CDKey
        {
            get
            {
                if (this.mCDKey == null)
                {
                    return "";
                }
                return this.mCDKey;
            }
            set
            {
                this.mCDKey = value;
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

        public int CommunicationsMethod
        {
            get
            {
                return this.mCommunicationsMethod;
            }
            set
            {
                this.mCommunicationsMethod = value;
            }
        }

        public string CurrentVersion
        {
            get
            {
                return this.mCurrentVersion;
            }
            set
            {
                this.mCurrentVersion = value;
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

        public string ForcedCommandLineArgs
        {
            get
            {
                if (this.mForcedCommandLineArgs == null)
                {
                    this.mForcedCommandLineArgs = "";
                }
                if (this.mForcedCommandLineArgs == "(null)")
                {
                    this.mForcedCommandLineArgs = "";
                }
                if (this.mForcedCommandLineArgs == "None")
                {
                    this.mForcedCommandLineArgs = "";
                }
                return this.mForcedCommandLineArgs;
            }
            set
            {
                this.mForcedCommandLineArgs = value;
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

        public Image GameIcon
        {
            get
            {
                Graphics graphics;
                Font font;
                Brush brush;
                Brush brush2;
                StringFormat format;
                Bitmap mGameIcon = this.mGameIcon;
                try
                {
                    if (mGameIcon != null)
                    {
                        return mGameIcon;
                    }
                    if (!File.Exists(this.GameLocation))
                    {
                        return mGameIcon;
                    }
                    mGameIcon = ExtractIcon.GetIcon(this.GameLocation, false).ToBitmap();
                    if (this.IconLabel != null)
                    {
                        graphics = Graphics.FromImage(mGameIcon);
                        font = new Font("Small Fonts", 7f);
                        brush = new SolidBrush(System.Drawing.Color.White);
                        brush2 = new SolidBrush(System.Drawing.Color.Black);
                        format = new StringFormat();
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        graphics.DrawString(this.IconLabel, font, brush2, new RectangleF(1f, 1f, 32f, 32f), format);
                        graphics.DrawString(this.IconLabel, font, brush, new RectangleF(0f, 0f, 32f, 32f), format);
                        format.Dispose();
                        brush2.Dispose();
                        brush.Dispose();
                        font.Dispose();
                        graphics.Dispose();
                    }
                }
                catch (Exception)
                {
                    if (mGameIcon != null)
                    {
                        return mGameIcon;
                    }
                    mGameIcon = new Bitmap(0x20, 0x20);
                    graphics = Graphics.FromImage(mGameIcon);
                    graphics.Clear(System.Drawing.Color.Red);
                    font = new Font("Small Fonts", 7f);
                    brush = new SolidBrush(System.Drawing.Color.White);
                    brush2 = new SolidBrush(System.Drawing.Color.Black);
                    format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    string iconLabel = this.IconLabel;
                    switch (iconLabel)
                    {
                        case null:
                        case "":
                            iconLabel = this.GameDescription;
                            break;
                    }
                    graphics.DrawString(iconLabel, font, brush2, new RectangleF(1f, 1f, 32f, 32f), format);
                    graphics.DrawString(iconLabel, font, brush, new RectangleF(0f, 0f, 32f, 32f), format);
                    format.Dispose();
                    brush2.Dispose();
                    brush.Dispose();
                    font.Dispose();
                    graphics.Dispose();
                }
                return mGameIcon;
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

        public string GameLocation
        {
            get
            {
                return this.mGameLocation;
            }
            set
            {
                if (value != this.mGameLocation)
                {
                    this.mGameLocation = value;
                    this.mGameIcon = null;
                    if (OnNewGameLocation != null)
                    {
                        OnNewGameLocation(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool HasGame
        {
            get
            {
                try
                {
                    return (File.Exists(this.GameLocation) && (this.CDKey != ""));
                }
                catch
                {
                    return false;
                }
            }
        }

        public string IconLabel
        {
            get
            {
                return this.mIconLabel;
            }
            set
            {
                this.mIconLabel = value;
            }
        }

        public string InstallDirectory
        {
            get
            {
                if (this.mInstallDirectory == null)
                {
                    return "";
                }
                if (this.mInstallDirectory.ToUpper() == "NONE")
                {
                    return "";
                }
                if (this.mInstallDirectory.ToUpper() == "(NULL)")
                {
                    return "";
                }
                return this.mInstallDirectory;
            }
            set
            {
                this.mInstallDirectory = value;
            }
        }

        public bool IsChatOnly
        {
            get
            {
                return (this.GameID == -1);
            }
        }

        public bool IsSpaceSiege
        {
            get
            {
                return (this.GameID == 15);
            }
        }

        public string LongDescription
        {
            get
            {
                if (this.mLongDescription == null)
                {
                    return "";
                }
                if (this.mLongDescription.ToUpper() == "NONE")
                {
                    return "";
                }
                if (this.mLongDescription.ToUpper() == "(NULL)")
                {
                    return "";
                }
                return this.mLongDescription;
            }
            set
            {
                this.mLongDescription = value;
            }
        }

        public string PatcherAppURL
        {
            get
            {
                if (this.mPatcherAppURL == null)
                {
                    return "";
                }
                if (this.mPatcherAppURL.ToUpper() == "NONE")
                {
                    return "";
                }
                if (this.mPatcherAppURL.ToUpper() == "(NULL)")
                {
                    return "";
                }
                return this.mPatcherAppURL;
            }
            set
            {
                this.mPatcherAppURL = value;
            }
        }

        public string PatchURL
        {
            get
            {
                if (this.mPatchURL == null)
                {
                    return "";
                }
                if (this.mPatchURL.ToUpper() == "NONE")
                {
                    return "";
                }
                if (this.mPatchURL.ToUpper() == "(NULL)")
                {
                    return "";
                }
                return this.mPatchURL;
            }
            set
            {
                this.mPatchURL = value;
            }
        }

        public int Port
        {
            get
            {
                return Program.Settings.SupcomPrefs.GamePort;
            }
            set
            {
                this.mPort = value;
            }
        }

        public static GameInformation SelectedGame
        {
            get
            {
                if (sSelectedGame == null)
                {
                    return GetSupcomInfo();
                }
                return sSelectedGame;
            }
            set
            {
                if (value != sSelectedGame)
                {
                    sSelectedGame = value;
                    GPGnetSelectedGame.SelectedGame = value;
                    Program.Settings.Login.DefaultGameID = value.GameID;
                    if (OnSelectedGameChange != null)
                    {
                        OnSelectedGameChange(sSelectedGame, EventArgs.Empty);
                    }
                    ThreadPool.QueueUserWorkItem(delegate (object o) {
                        EventLog.WriteLine("Loaded maps in an off thread.", new object[0]);
                        SupcomMapList.RefreshMaps();
                        EventLog.WriteLine("Loaded " + SupcomMapList.Maps.Count.ToString(), new object[0]);
                    });
                    if (User.LoggedIn)
                    {
                        ConfigSettings.LoadSettings(DataAccess.GetQueryData("GetAllConfigs", new object[0]));
                    }
                }
            }
        }

        public string UserForcedCommandLineArgs
        {
            get
            {
                if (this.mUserForcedCommandLineArgs == null)
                {
                    this.mUserForcedCommandLineArgs = "";
                }
                if (this.mUserForcedCommandLineArgs == "(null)")
                {
                    this.mUserForcedCommandLineArgs = "";
                }
                if (this.mUserForcedCommandLineArgs == "None")
                {
                    this.mUserForcedCommandLineArgs = "";
                }
                return this.mUserForcedCommandLineArgs;
            }
            set
            {
                this.mUserForcedCommandLineArgs = value;
            }
        }

        public string VersionFile
        {
            get
            {
                return this.mVersionFile;
            }
            set
            {
                this.mVersionFile = value;
            }
        }
    }
}

