namespace GPG.Multiplayer.Client.Clans
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;

    [IndexMap(new string[] { "mID", "mName" }, new string[] { "id", "name" })]
    public class ClanMember : MappedObject, IUser
    {
        [FieldMap("clan_id")]
        private int mClanID = -1;
        private static ClanMember mCurrent;
        [FieldMap("date_joined")]
        private DateTime mDateJoined = DateTime.MinValue;
        [FieldMap("clan_principal_id")]
        private int mID = -1;
        [FieldMap("is_dnd")]
        private bool mIsDND = false;
        [FieldMap("name")]
        private string mName = null;
        [FieldMap("online")]
        private bool mOnline = false;
        [FieldMap("rank")]
        private int mRank = -1;

        public ClanMember(DataRecord record) : base(record)
        {

        }

        public bool CanTargetAbility(ClanAbility ability, ClanMember target)
        {
            if (!this.HasAbility(ability))
            {
                return false;
            }
            if (ability == ClanAbility.Promote)
            {
                return (this.Rank < (target.Rank - 1));
            }
            if (ability == ClanAbility.Demote)
            {
                return ((this.Rank < target.Rank) && (target.Rank < ClanRanking.MinValue.Seniority));
            }
            if (ability == ClanAbility.Remove)
            {
                return (this.Rank < target.Rank);
            }
            return true;
        }

        public bool CanTargetAbility(ClanAbility ability, string target)
        {
            ClanMember member;
            return (Clan.CurrentMembers.TryFindByIndex("name", target, out member) && this.CanTargetAbility(ability, member));
        }

        public override bool Equals(object obj)
        {
            if (obj is IUser)
            {
                return ((obj as IUser).ID == this.ID);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

        public ClanRanking GetRanking()
        {
            foreach (ClanRanking ranking in ClanRanking.All.ToArray())
            {
                if (ranking.Seniority == this.Rank)
                {
                    return ranking;
                }
            }
            return ClanRanking.MinValue;
        }

        public bool HasAbility(ClanAbility ability)
        {
            return ability.IsAvailable(this);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public int ClanID
        {
            get
            {
                return this.mClanID;
            }
        }

        public static ClanMember Current
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

        public DateTime DateJoined
        {
            get
            {
                return this.mDateJoined;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool IsClanMate
        {
            get
            {
                if (Current == null)
                {
                    return false;
                }
                return (this.ClanID == Current.ClanID);
            }
        }

        public bool IsCurrent
        {
            get
            {
                if (User.Current == null)
                {
                    return false;
                }
                return (this.ID == User.Current.ID);
            }
        }

        public bool IsDND
        {
            get
            {
                return this.mIsDND;
            }
            set
            {
                this.mIsDND = value;
            }
        }

        public bool IsIgnored
        {
            get
            {
                return User.IsUserIgnored(this.ID);
            }
        }

        public bool IsInClan
        {
            get
            {
                return true;
            }
        }

        public bool IsSystem
        {
            get
            {
                return false;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public bool Online
        {
            get
            {
                return this.mOnline;
            }
            set
            {
                this.mOnline = value;
            }
        }

        public int Rank
        {
            get
            {
                return this.mRank;
            }
            set
            {
                this.mRank = value;
            }
        }
    }
}

