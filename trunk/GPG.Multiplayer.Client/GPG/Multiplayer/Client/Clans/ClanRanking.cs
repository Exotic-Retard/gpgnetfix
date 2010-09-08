namespace GPG.Multiplayer.Client.Clans
{
    using GPG;
    using GPG.Multiplayer.Client.Games;
    using System;
    using System.Drawing;

    public class ClanRanking
    {
        private static ClanRankingDictionary _All;
        private string _Description;
        private string _ImageSource;
        private int _Seniority;
        public static readonly ClanRanking Commander = new ClanRanking(Loc.Get("<LOC>Commander"), 1, "rank_cmd");
        public static readonly ClanRanking MasterSergeant = new ClanRanking(Loc.Get("<LOC>Master Sergeant"), 2, "rank_msgt");
        public static readonly ClanRanking Private = new ClanRanking(Loc.Get("<LOC>Private"), 5, "rank_pvt");
        public static readonly ClanRanking PrivateFirstClass = new ClanRanking(Loc.Get("<LOC>Private First Class"), 4, "rank_pfc");
        public static readonly ClanRanking Sergeant = new ClanRanking(Loc.Get("<LOC>Sergeant"), 3, "rank_sgt");
        public static readonly ClanRanking SupremeCommander = new ClanRanking(Loc.Get("<LOC>Supreme Commander"), 0, "rank_supcom");

        private ClanRanking(string desc, int seniority, string imgSrc)
        {
            if (GameInformation.SelectedGame.IsChatOnly || GameInformation.SelectedGame.IsSpaceSiege)
            {
                this._Description = desc.Replace("Supreme Commander", Loc.Get("<LOC>Leader"));
            }
            else
            {
                this._Description = desc;
            }
            this._Seniority = seniority;
            this._ImageSource = imgSrc;
        }

        public static ClanRanking FindBySeniority(int seniority)
        {
            foreach (ClanRanking ranking in All.ToArray())
            {
                if (ranking.Seniority == seniority)
                {
                    return ranking;
                }
            }
            return MinValue;
        }

        public static ClanRankingDictionary All
        {
            get
            {
                if (_All == null)
                {
                    _All = new ClanRankingDictionary();
                    _All.Add(SupremeCommander);
                    _All.Add(Commander);
                    _All.Add(MasterSergeant);
                    _All.Add(Sergeant);
                    _All.Add(PrivateFirstClass);
                    _All.Add(Private);
                }
                return _All;
            }
        }

        public string Description
        {
            get
            {
                return this._Description;
            }
        }

        public Bitmap Image
        {
            get
            {
                return (ClanImages.ResourceManager.GetObject(this.ImageSource) as Bitmap);
            }
        }

        public string ImageSource
        {
            get
            {
                return this._ImageSource;
            }
        }

        public static ClanRanking MaxValue
        {
            get
            {
                return SupremeCommander;
            }
        }

        public static ClanRanking MinValue
        {
            get
            {
                return Private;
            }
        }

        public int Seniority
        {
            get
            {
                return this._Seniority;
            }
        }
    }
}

