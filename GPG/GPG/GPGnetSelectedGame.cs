namespace GPG
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class GPGnetSelectedGame
    {
        public static bool IsAdmin = false;
        private static string mProfileName = "";
        private static List<IGameInformation> sGameList = new List<IGameInformation>();
        private static IGameInformation sSelectedGame = null;

        public static  event EventHandler OnGameChanged;

        public static void TestFactions(out bool hasOriginal, out bool hasExpansion)
        {
            hasOriginal = false;
            hasExpansion = false;
            foreach (IGameInformation information in GameList)
            {
                if ((information.GameDescription == "Supreme Commander") && (information.CDKey != ""))
                {
                    hasOriginal = true;
                }
                if ((information.GameDescription == "Forged Alliance Beta") && (information.CDKey != ""))
                {
                    hasExpansion = true;
                }
                if ((information.GameDescription == "Forged Alliance") && (information.CDKey != ""))
                {
                    hasExpansion = true;
                }
            }
            if (IsAdmin)
            {
                hasExpansion = true;
                hasOriginal = true;
            }
            if (SelectedGame.GameDescription == "Supreme Commander")
            {
                hasExpansion = false;
            }
        }

        public static List<IGameInformation> GameList
        {
            get
            {
                return sGameList;
            }
            set
            {
                sGameList = value;
            }
        }

        public static bool IsChatOnly
        {
            get
            {
                if (SelectedGame == null)
                {
                    return false;
                }
                return (SelectedGame.GameID == -1);
            }
        }

        public static bool IsSpaceSiege
        {
            get
            {
                if (SelectedGame == null)
                {
                    return false;
                }
                return (SelectedGame.GameID == 15);
            }
        }

        public static string ProfileName
        {
            get
            {
                return mProfileName;
            }
            set
            {
                mProfileName = value;
            }
        }

        public static IGameInformation SelectedGame
        {
            get
            {
                return sSelectedGame;
            }
            set
            {
                sSelectedGame = value;
                if (OnGameChanged != null)
                {
                    OnGameChanged(sSelectedGame, EventArgs.Empty);
                }
            }
        }
    }
}

