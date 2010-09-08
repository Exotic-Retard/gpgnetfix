namespace GPG.Multiplayer.Client.Clans
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ClanRankingDictionary : DictionaryBase
    {
        public void Add(ClanRanking rank)
        {
            base.InnerHashtable.Add(rank.Description, rank);
        }

        public bool Contains(ClanRanking rank)
        {
            return this.Contains(rank.Description);
        }

        public bool Contains(string desc)
        {
            return base.InnerHashtable.ContainsKey(desc);
        }

        public void Remove(ClanRanking rank)
        {
            this.Remove(rank.Description);
        }

        public void Remove(string desc)
        {
            base.InnerHashtable.Remove(desc);
        }

        public ClanRanking[] ToArray()
        {
            ClanRanking[] array = new ClanRanking[base.InnerHashtable.Count];
            base.InnerHashtable.Values.CopyTo(array, 0);
            return array;
        }

        public ClanRanking this[int seniority]
        {
            get
            {
                ClanRanking[] rankingArray = this.ToArray();
                for (int i = 0; i < rankingArray.Length; i++)
                {
                    if (rankingArray[i].Seniority == seniority)
                    {
                        return rankingArray[i];
                    }
                }
                return null;
            }
        }

        public ClanRanking this[string description]
        {
            get
            {
                return (base.InnerHashtable[description] as ClanRanking);
            }
        }
    }
}

