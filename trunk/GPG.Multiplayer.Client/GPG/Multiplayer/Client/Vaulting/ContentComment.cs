namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG.DataAccess;
    using System;

    public class ContentComment : MappedObject
    {
        [FieldMap("comment_text")]
        private string mCommentText;
        [FieldMap("content_id")]
        private int mContentID;
        [FieldMap("comment_date")]
        private DateTime mCreateDate;
        [FieldMap("edit_count")]
        private int mEditCount;
        [FieldMap("content_comment_id")]
        private int mID;
        [FieldMap("last_edit_by")]
        private int mLastEditByID;
        [FieldMap("last_edit_name")]
        private string mLastEditByName;
        [FieldMap("last_edit_date")]
        private DateTime mLastEditDate;
        [FieldMap("player_id")]
        private int mPlayerID;
        [FieldMap("name")]
        private string mPlayerName;

        public ContentComment(DataRecord record) : base(record)
        {
            this.mPlayerName = "";
        }

        public string CommentText
        {
            get
            {
                return this.mCommentText;
            }
            set
            {
                this.mCommentText = value;
            }
        }

        public int ContentID
        {
            get
            {
                return this.mContentID;
            }
        }

        public DateTime CreateDate
        {
            get
            {
                return this.mCreateDate;
            }
        }

        public int EditCount
        {
            get
            {
                return this.mEditCount;
            }
            set
            {
                this.mEditCount = value;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public int LastEditByID
        {
            get
            {
                return this.mLastEditByID;
            }
            set
            {
                this.mLastEditByID = value;
            }
        }

        public string LastEditByName
        {
            get
            {
                return this.mLastEditByName;
            }
            set
            {
                this.mLastEditByName = value;
            }
        }

        public DateTime LastEditDate
        {
            get
            {
                return this.mLastEditDate;
            }
            set
            {
                this.mLastEditDate = value;
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
        }
    }
}

