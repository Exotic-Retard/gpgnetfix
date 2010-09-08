namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG.Multiplayer.Client.Games;
    using System;
    using System.Drawing;
    using System.IO;

    public class SupcomMap
    {
        public bool IsBuiltIn = false;
        private System.Drawing.Image mImage;
        private string mMapID = "";
        private string mMapName = "";
        private string mMapSize;
        private int mMaxPlayers;
        private string mPath;

        public static SupcomMap LoadMap(string path)
        {
            SupcomMap map = new SupcomMap();
            map.mPath = path;
            if (!File.Exists(path))
            {
                map.mImage = GameInformation.SelectedGame.GameIcon;
                return map;
            }
            Bitmap bitmap = new Bitmap(0x100, 0x100);
            FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000);
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
            map.mImage = bitmap;
            string str = path.Replace(".scmap", "_scenario.lua");
            if (File.Exists(str))
            {
                string[] strArray;
                StreamReader reader2 = new StreamReader(str);
                while (reader2.Peek() >= 0)
                {
                    string str2 = reader2.ReadLine();
                    string str3 = str2.Replace(" ", "");
                    if (str3.IndexOf("size={") >= 0)
                    {
                        map.mMapSize = str3.Replace("size={", "").Replace("},", "").Replace("}", "").Trim();
                    }
                    if (str2.IndexOf("ARMY_1") >= 0)
                    {
                        map.mMaxPlayers = 1;
                    }
                    if (str2.IndexOf("ARMY_2") >= 0)
                    {
                        map.mMaxPlayers = 2;
                    }
                    if (str2.IndexOf("ARMY_3") >= 0)
                    {
                        map.mMaxPlayers = 3;
                    }
                    if (str2.IndexOf("ARMY_4") >= 0)
                    {
                        map.mMaxPlayers = 4;
                    }
                    if (str2.IndexOf("ARMY_5") >= 0)
                    {
                        map.mMaxPlayers = 5;
                    }
                    if (str2.IndexOf("ARMY_6") >= 0)
                    {
                        map.mMaxPlayers = 6;
                    }
                    if (str2.IndexOf("ARMY_7") >= 0)
                    {
                        map.mMaxPlayers = 7;
                    }
                    if (str2.IndexOf("ARMY_8") >= 0)
                    {
                        map.mMaxPlayers = 8;
                    }
                    if ((((str3.IndexOf("name=") >= 0) && (map.MapName == "")) && (str3.ToUpper().IndexOf("ARMIES") < 0)) && (str3.ToUpper().IndexOf("FFA") < 0))
                    {
                        map.mMapName = str2.Replace("name", "").Replace("=", "").Replace("'", "").Replace(",", "").Replace("\"", "").Trim();
                    }
                }
                reader2.Close();
                if (path.ToUpper().IndexOf(".V") >= 0)
                {
                    strArray = path.Split(@"\".ToCharArray());
                    map.mMapID = "";
                    bool flag = false;
                    foreach (string str4 in strArray)
                    {
                        if (str4.ToUpper() == "MAPS")
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            map.mMapID = map.mMapID + "/" + str4;
                        }
                    }
                    map.mMapID = map.mMapID.Replace(".scmap", "_scenario.lua");
                }
                if (!(map.mMapID == ""))
                {
                    return map;
                }
                if ((path.ToUpper().IndexOf("SCMP") >= 0) || (path.ToUpper().IndexOf("X1MP") >= 0))
                {
                    strArray = path.Split(@"\".ToCharArray());
                    foreach (string str4 in strArray)
                    {
                        if ((str4.ToUpper().IndexOf("SCMP") >= 0) || (str4.ToUpper().IndexOf("X1MP") >= 0))
                        {
                            map.MapID = str4;
                            map.IsBuiltIn = true;
                            break;
                        }
                    }
                }
                if (map.mMapID == "")
                {
                    map.mMapID = map.mMapName;
                }
            }
            return map;
        }

        public override string ToString()
        {
            if (this.IsBuiltIn)
            {
                return this.MapName;
            }
            return this.MapID;
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.mImage;
            }
        }

        public string MapCheckName
        {
            get
            {
                if (this.IsBuiltIn)
                {
                    return this.MapName;
                }
                return (this.Version + " " + this.MapName);
            }
        }

        public string MapID
        {
            get
            {
                return this.mMapID;
            }
            set
            {
                this.mMapID = value;
            }
        }

        public string MapName
        {
            get
            {
                return this.mMapName;
            }
        }

        public string MapSize
        {
            get
            {
                return this.mMapSize;
            }
        }

        public int MaxPlayers
        {
            get
            {
                return this.mMaxPlayers;
            }
        }

        public string Path
        {
            get
            {
                return this.mPath;
            }
        }

        private string Version
        {
            get
            {
                if (this.MapID.ToUpper().IndexOf(".V") >= 0)
                {
                    return this.MapID.Split(new char[] { '.' })[1].Split(new char[] { '/' })[0].Replace(".V", "");
                }
                return "1";
            }
        }
    }
}

