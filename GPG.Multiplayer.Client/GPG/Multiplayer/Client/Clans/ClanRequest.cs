namespace GPG.Multiplayer.Client.Clans
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using System;

    [IndexMap("mRequestorName", "name")]
    public class ClanRequest : MappedObject, IUserRequest
    {
        [FieldMap("recipient_id")]
        private int mRecieverID;
        [FieldMap("recipient_name")]
        private string mRecieverName;
        [FieldMap("request_date")]
        private DateTime mRequestDate;
        [FieldMap("requestor_id")]
        private int mRequestorID;
        [FieldMap("requestor_name")]
        private string mRequestorName;

        public ClanRequest(DataRecord record) : base(record)
        {
        }

        public ClanRequest(int reqId, string reqName, int recId, string recName, DateTime date)
        {
            this.mRequestorID = reqId;
            this.mRequestorName = reqName;
            this.mRecieverID = recId;
            this.mRecieverName = recName;
            this.mRequestDate = date;
        }

        public override bool Equals(object obj)
        {
            if (obj is ClanRequest)
            {
                return (obj.GetHashCode() == this.GetHashCode());
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.RequestorID ^ this.RecieverID);
        }

        public string Description
        {
            get
            {
                return this.RequestorName;
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

