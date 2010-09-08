namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;

    [Serializable]
    public class ContentList : MappedObject
    {
        [FieldMap("category")]
        private string mCategory;
        [FieldMap("category_image_index")]
        private int mCategoryImageIndex;
        [FieldMap("content_type_id")]
        private int mContentTypeID;
        [FieldMap("content_list_id")]
        private int mID;
        [FieldMap("list_image_index")]
        private int mListImageIndex;
        [FieldMap("list_type")]
        private int mListType;
        [FieldMap("name")]
        private string mName;
        [FieldMap("owner_id")]
        private int mOwnerID;
        [FieldMap("owner_name")]
        private string mOwnerName;
        private QuazalQuery mQuery = null;
        [FieldMap("tag")]
        private string Tag;

        private ContentList()
        {
        }

        public ContentList(DataRecord record) : base(record)
        {
        }

        public string Category
        {
            get
            {
                return this.mCategory;
            }
        }

        public int CategoryImageIndex
        {
            get
            {
                return this.mCategoryImageIndex;
            }
        }

        public int ContentTypeID
        {
            get
            {
                return this.mContentTypeID;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public int ListImageIndex
        {
            get
            {
                return this.mListImageIndex;
            }
        }

        public ContentListTypes ListType
        {
            get
            {
                return (ContentListTypes) this.mListType;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public int OwnerID
        {
            get
            {
                return this.mOwnerID;
            }
        }

        public string OwnerName
        {
            get
            {
                return this.mOwnerName;
            }
        }

        public QuazalQuery Query
        {
            get
            {
                if ((this.mQuery == null) && (this.ListType == ContentListTypes.StoredQuery))
                {
                    this.mQuery = new QuazalQuery(this.Tag, new object[0]);
                }
                return this.mQuery;
            }
        }
    }
}

