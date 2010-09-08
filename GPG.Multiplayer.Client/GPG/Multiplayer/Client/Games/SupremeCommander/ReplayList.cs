namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG.DataAccess;
    using System;

    public class ReplayList : MappedObject
    {
        [FieldMap("create_date")]
        private DateTime mCreateDate;
        [FieldMap("replay_list_id")]
        private int mID;
        [FieldMap("is_paged")]
        private bool mIsPaged;
        [FieldMap("owner_id")]
        private int mOwner;
        [FieldMap("page_size")]
        private int mPageSize;
        [FieldMap("query")]
        private string mQuery;
        [FieldMap("sort_index")]
        private int mSortIndex;
        [FieldMap("title")]
        private string mTitle;

        public ReplayList(DataRecord record) : base(record)
        {
        }

        public DateTime CreateDate
        {
            get
            {
                return this.mCreateDate;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool IsPaged
        {
            get
            {
                return this.mIsPaged;
            }
        }

        public bool IsQuery
        {
            get
            {
                return (((this.Query != null) && (this.Query.Length > 0)) && (this.Query != "(null)"));
            }
        }

        public int Owner
        {
            get
            {
                return this.mOwner;
            }
        }

        public int PageSize
        {
            get
            {
                return this.mPageSize;
            }
        }

        public string Query
        {
            get
            {
                return this.mQuery;
            }
        }

        public int SortIndex
        {
            get
            {
                return this.mSortIndex;
            }
        }

        public string Title
        {
            get
            {
                return this.mTitle;
            }
        }
    }
}

