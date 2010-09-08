namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Games_Supcom_RankedGames
    {
        private string mFaction = null;
        private SupcomMapInfo[] mMaps = null;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FactionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MapsChanged;

        public string FetchFaction()
        {
            string faction = this.Faction;
            if (this.Faction == Loc.Get("<LOC>random"))
            {
                return this.RandomFaction();
            }
            int num = 0;
            while (ConfigSettings.GetBool("CheckFactions", true) && (num < 100))
            {
                if (!(ConfigSettings.GetBool("NO_CYBRAN", true) || !(faction == "/cybran")))
                {
                    faction = this.RandomFaction();
                }
                else
                {
                    if (!(ConfigSettings.GetBool("NO_AEON", true) || !(faction == "/aeon")))
                    {
                        faction = this.RandomFaction();
                        continue;
                    }
                    if (!(ConfigSettings.GetBool("NO_UEF", true) || !(faction == "/uef")))
                    {
                        faction = this.RandomFaction();
                        continue;
                    }
                    num++;
                }
            }
            bool hasOriginal = true;
            bool hasExpansion = true;
            SupcomPrefs.TestFactions(out hasOriginal, out hasExpansion);
            if (!(hasOriginal || hasExpansion))
            {
                throw new Exception("Supreme Commander And Forged Alliance are both not available.  Unable to continue.");
            }
            if (!hasOriginal)
            {
                faction = "/seraphim";
            }
            for (int i = 0; (!hasExpansion && (faction == "/seraphim")) && (i < 100); i++)
            {
                faction = this.RandomFaction();
            }
            return faction;
        }

        public string FetchMap()
        {
            Random random = new Random();
            string mapID = "SCMP_019";
            int num = 0;
            foreach (SupcomMapInfo info in this.mMaps)
            {
                int num2 = 0;
                if (info.Priority == true)
                {
                    num2 = random.Next(1, 0x3e8);
                }
                else if (!info.Priority.HasValue)
                {
                    num2 = random.Next(1, 500);
                }
                else
                {
                    num2 = random.Next(1, 100);
                }
                if (num2 > num)
                {
                    mapID = info.MapID;
                    num = num2;
                }
            }
            return mapID;
        }

        public string RandomFaction()
        {
            Random random = new Random();
            switch (random.Next(1, 5))
            {
                case 1:
                    return "/aeon";

                case 2:
                    return "/cybran";

                case 3:
                    return "/seraphim";
            }
            return "/uef";
        }

        [Description(""), DisplayName("<LOC>Faction"), Category("<LOC>Misc")]
        public string Faction
        {
            get
            {
                if ((this.mFaction == null) || (this.mFaction.Length < 1))
                {
                    this.mFaction = "random";
                }
                return this.mFaction;
            }
            set
            {
                this.mFaction = value;
                if (this.FactionChanged != null)
                {
                    this.FactionChanged(this, new PropertyChangedEventArgs("Faction"));
                }
            }
        }

        [Description(""), Category("<LOC>Misc"), DisplayName("<LOC>Maps")]
        public SupcomMapInfo[] Maps
        {
            get
            {
                return this.mMaps;
            }
            set
            {
                this.mMaps = value;
                if (this.MapsChanged != null)
                {
                    this.MapsChanged(this, new PropertyChangedEventArgs("Maps"));
                }
            }
        }
    }
}

