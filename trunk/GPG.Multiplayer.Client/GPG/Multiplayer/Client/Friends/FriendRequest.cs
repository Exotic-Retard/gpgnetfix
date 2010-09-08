namespace GPG.Multiplayer.Client.Friends
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using System;

    [IndexMap("mRequestorName", "name")]
    public class FriendRequest : MappedObject, IUserRequest
    {
        [FieldMap("reciever_id")]
        private int mRecieverID = -1;
        [FieldMap("reciever_name")]
        private string mRecieverName = null;
        [FieldMap("request_date")]
        private DateTime mRequestDate = DateTime.MinValue;
        [FieldMap("requestor_id")]
        private int mRequestorID = -1;
        [FieldMap("requestor_name")]
        private string mRequestorName = null;

        public FriendRequest(DataRecord record) : base(record)
        {
        }

        public FriendRequest(int reqId, string reqName, int recId, string recName, DateTime date)
        {
            this.mRequestorID = reqId;
            this.mRequestorName = reqName;
            this.mRecieverID = recId;
            this.mRecieverName = recName;
            this.mRequestDate = date;
        }

        public override bool Equals(object obj)
        {
            if (obj is FriendRequest)
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

