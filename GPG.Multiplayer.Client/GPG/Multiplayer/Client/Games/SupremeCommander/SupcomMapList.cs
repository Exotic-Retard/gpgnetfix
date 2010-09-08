namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG.Logging;
    using GPG.Multiplayer.Client.Games;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class SupcomMapList
    {
        private static List<SupcomMap> mMaps = new List<SupcomMap>();

        public static string CustomMapDir()
        {
            string str = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\My Games\Gas Powered Games\";
            if (GameInformation.SelectedGame.GameID == 2)
            {
                return (str + @"SupremeCommander\");
            }
            if (GameInformation.SelectedGame.GameID == 0x12)
            {
                return (str + @"Supreme Commander Forged Alliance\");
            }
            if (GameInformation.SelectedGame.GameID == 0x11)
            {
                return (str + @"Supreme Commander Forged Alliance\");
            }
            return (str + GameInformation.SelectedGame.ExeName.Replace(".exe", "") + @"\");
        }

        public static bool HasMap(string path)
        {
            foreach (SupcomMap map in mMaps)
            {
                if (map.Path == path)
                {
                    return true;
                }
            }
            return false;
        }

        public static void RefreshMaps()
        {
            Exception exception;
            try
            {
                string path = GameInformation.SelectedGame.GameLocation.Replace(@"\bin\" + GameInformation.SelectedGame.ExeName, @"\maps\");
                if (Directory.Exists(path))
                {
                    foreach (string str2 in Directory.GetFiles(path, "*.scmap", SearchOption.AllDirectories))
                    {
                        if (((str2.IndexOf("SCMP") >= 0) || (str2.IndexOf("X1MP") >= 0)) && !HasMap(str2))
                        {
                            mMaps.Add(SupcomMap.LoadMap(str2));
                        }
                    }
                    path = CustomMapDir();
                    if (Directory.Exists(path))
                    {
                        foreach (string str3 in Directory.GetDirectories(path))
                        {
                            try
                            {
                                foreach (string str2 in Directory.GetFiles(str3, "*.scmap", SearchOption.AllDirectories))
                                {
                                    if (!HasMap(str2))
                                    {
                                        mMaps.Add(SupcomMap.LoadMap(str2));
                                    }
                                }
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                ErrorLog.WriteLine(exception);
                            }
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                ErrorLog.WriteLine(exception);
            }
        }

        public static List<SupcomMap> Maps
        {
            get
            {
                if (mMaps.Count == 0)
                {
                    try
                    {
                        RefreshMaps();
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }
                return mMaps;
            }
        }
    }
}

