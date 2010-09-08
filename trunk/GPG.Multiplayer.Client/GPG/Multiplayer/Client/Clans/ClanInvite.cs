namespace GPG.Multiplayer.Client.Clans
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using System;

    [IndexMap("mRequestorName", "name")]
    public class ClanInvite : MappedObject, IUserRequest
    {
        [FieldMap("clan_abbr")]
        private string mClanAbbreviation;
        [FieldMap("clan_id")]
        private int mClanID;
        [FieldMap("clan_name")]
        private string mClanName;
        [FieldMap("recipient_id")]
        private int mRecieverID;
        [FieldMap("recipient_name")]
        private string mRecieverName;
        [FieldMap("invite_date")]
        private DateTime mRequestDate;
        [FieldMap("requestor_id")]
        private int mRequestorID;
        [FieldMap("requestor_name")]
        private string mRequestorName;

        public ClanInvite(DataRecord record) : base(record)
        {
            this.mClanAbbreviation = null;
        }

        public ClanInvite(int reqId, string reqName, int recId, string recName, int clanId, string clanName, DateTime date)
        {
            this.mClanAbbreviation = null;
            this.mRequestorID = reqId;
            this.mRequestorName = reqName;
            this.mRecieverID = recId;
            this.mRecieverName = recName;
            this.mClanID = clanId;
            this.mClanName = clanName;
            this.mRequestDate = date;
        }

        public override bool Equals(object obj)
        {
            if (obj is ClanInvite)
            {
                return (obj.GetHashCode() == this.GetHashCode());
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.RequestorID ^ this.RecieverID);
        }

        public string ClanAbbreviation
        {
            get
            {
                return this.mClanAbbreviation;
            }
        }

        public int ClanID
        {
            get
            {
                return this.mClanID;
            }
        }

        public string ClanName
        {
            get
            {
                return this.mClanName;
            }
        }

        public string Description
        {
            get
            {
                return this.ClanName;
            }
        }

        public int RecieverID
        {
            get
            {
                return this.mRecieverID;
            }
        }

        public string RecieverName
        {
            get
            {
                return this.mRecieverName;
            }
        }

        public DateTime RequestDate
        {
            get
            {
                return this.mRequestDate;
            }
        }

        public int RequestorID
        {
            get
            {
                return this.mRequestorID;
            }
        }

        public string RequestorName
        {
            get
            {
                return this.mRequestorName;
            }
        }
    }
}

