namespace GPG.Multiplayer.Client.Clans
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    [IndexMap(new string[] { "mName", "mID", "mAbbreviation" }, new string[] { "name", "id", "abbr" })]
    public class Clan : MappedObject
    {
        private int LastCache = 0;
        [FieldMap("abbreviation")]
        private string mAbbreviation=null;
        [FieldMap("date_founded")]
        private DateTime mCreateDate = DateTime.MinValue;
        private static Clan mCurrent = null;
        private static MappedObjectList<ClanMember> mCurrentMembers = new MappedObjectList<ClanMember>();
        [FieldMap("description")]
        private string mDescription=null;
        [FieldMap("embed_website")]
        private bool mEmbedWebsite;
        [FieldMap("clan_id")]
        private int mID=-1;
        private System.Drawing.Image mImage = null;
        [FieldMap("name")]
        private string mName=null;
        private PlayerRating mRating = null;
        [FieldMap("website")]
        private string mWebsite = null;

        public Clan(DataRecord record) : base(record)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is Clan)
            {
                return ((obj as Clan).ID == this.ID);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.Abbreviation);
        }

        public string Abbreviation
        {
            get
            {
                return this.mAbbreviation;
            }
        }

        public DateTime CreateDate
        {
            get
            {
                return this.mCreateDate;
            }
        }

        public static Clan Current
        {
            get
            {
                return mCurrent;
            }
            set
            {
                mCurrent = value;
            }
        }

        public static MappedObjectList<ClanMember> CurrentMembers
        {
            get
            {
                if (mCurrentMembers == null)
                {
                    return new MappedObjectList<ClanMember>();
                }
                return mCurrentMembers;
            }
            set
            {
                mCurrentMembers = value;
            }
        }

        public static string[] CurrentMembersMsgList
        {
            get
            {
                if ((CurrentMembers == null) || (CurrentMembers.Count <= 1))
                {
                    return new string[0];
                }
                List<string> list = new List<string>(CurrentMembers.Count - 1);
                for (int i = 0; i < CurrentMembers.Count; i++)
                {
                    if (CurrentMembers[i].Online && (CurrentMembers[i].Name != User.Current.Name))
                    {
                        list.Add(CurrentMembers[i].Name);
                    }
                }
                return list.ToArray();
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

        public bool EmbedWebsite
        {
            get
            {
                return this.mEmbedWebsite;
            }
            set
            {
                this.mEmbedWebsite = value;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.mImage;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public PlayerRating Rating
        {
            get
            {
                if ((Environment.TickCount - this.LastCache) >= (ConfigSettings.GetInt("StatsCacheTimeout", 5) * 0xea60))
                {
                    this.mRating = null;
                }
                if ((this.mRating == null) && !new QuazalQuery("GetClanRating", new object[] { this.ID }).GetObject<PlayerRating>(out this.mRating))
                {
                    this.mRating = new PlayerRating(this.ID, "Clan");
                }
                return this.mRating;
            }
        }

        public string Website
        {
            get
            {
                return this.mWebsite;
            }
            set
            {
                this.mWebsite = value;
            }
        }
    }
}

