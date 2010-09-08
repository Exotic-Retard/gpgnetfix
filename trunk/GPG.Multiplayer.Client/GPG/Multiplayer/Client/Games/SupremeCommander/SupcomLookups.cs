namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;

    public static class SupcomLookups
    {
        internal static readonly MultiVal<string, int>[] Factions = new MultiVal<string, int>[] { new MultiVal<string, int>(Loc.Get("<LOC>Any Faction"), 0), new MultiVal<string, int>(Loc.Get("<LOC>Aeon"), 1), new MultiVal<string, int>(Loc.Get("<LOC>Cybran"), 2), new MultiVal<string, int>(Loc.Get("<LOC>UEF"), 3), new MultiVal<string, int>(Loc.Get("<LOC>Seraphim"), 4) };
        internal static readonly MultiVal<string, int>[] GameTypes = new MultiVal<string, int>[] { new MultiVal<string, int>(Loc.Get("<LOC>All Types"), 0), new MultiVal<string, int>(Loc.Get("<LOC>Custom Game"), 1), new MultiVal<string, int>(Loc.Get("<LOC>1v1 Ranked"), 2), new MultiVal<string, int>(Loc.Get("<LOC>2v2 Ranked"), 3), new MultiVal<string, int>(Loc.Get("<LOC>3v3 Ranked"), 4), new MultiVal<string, int>(Loc.Get("<LOC>4v4 Ranked"), 5) };
        internal static Dictionary<string, string> MapNameLookup = new Dictionary<string, string>();
        internal static readonly MultiVal<string, string>[] Maps;

        static SupcomLookups()
        {
            MapNameLookup = GameItem.MapNameLookup;
            Maps = new MultiVal<string, string>[MapNameLookup.Count + 1];
            Maps[0] = new MultiVal<string, string>(Loc.Get("<LOC>All Maps"), "");
            int index = 1;
            foreach (KeyValuePair<string, string> pair in MapNameLookup)
            {
                Maps[index] = new MultiVal<string, string>(pair.Value, pair.Key);
                index++;
            }
        }

        public static string TranslateFaction(_Factions faction)
        {
            return TranslateFaction((int) faction);
        }

        public static string TranslateFaction(int faction)
        {
            foreach (MultiVal<string, int> val in Factions)
            {
                if (val.Value2 == faction)
                {
                    return val.Value1;
                }
            }
            return faction.ToString();
        }

        public static string TranslateGameType(_GameTypes gameType)
        {
            return TranslateGameType((int) gameType);
        }

        public static string TranslateGameType(int gameType)
        {
            foreach (MultiVal<string, int> val in GameTypes)
            {
                if (val.Value2 == gameType)
                {
                    return val.Value1;
                }
            }
            return gameType.ToString();
        }

        public static string TranslateMapCode(string mapCode)
        {
            if (MapNameLookup.ContainsKey(mapCode))
            {
                return MapNameLookup[mapCode];
            }
            return mapCode;
        }

        public static string TranslateMapName(string mapName)
        {
            if (MapNameLookup.ContainsValue(mapName))
            {
                foreach (KeyValuePair<string, string> pair in MapNameLookup)
                {
                    if (pair.Value == mapName)
                    {
                        return pair.Key;
                    }
                }
            }
            return mapName;
        }

        public enum _Factions : uint
        {
            Aeon = 1,
            Any = 0,
            Cybran = 2,
            Seraphim = 4,
            UEF = 3
        }

        public enum _GameTypes : uint
        {
            All = 0,
            Custom = 1,
            Ranked1v1 = 2,
            Ranked2v2 = 3,
            Ranked3v3 = 4,
            Ranked4v4 = 5
        }
    }
}

