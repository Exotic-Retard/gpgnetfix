namespace GPG.Multiplayer.Client.Ladders
{
    using GPG.DataAccess;
    using System;

    public class LadderParticipantComment : MappedObject
    {
        [FieldMap("comment_date")]
        private DateTime mCommentDate;
        [FieldMap("commenter_id")]
        private int mCommenterID;
        [FieldMap("commenter_name")]
        private string mCommenterName;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("entity_id")]
        private int mEntityID;
        [FieldMap("game_id")]
        private int mGameID;
        [FieldMap("ladder_participant_comment_id")]
        private int mID;

        public LadderParticipantComment(DataRecord record) : base(record)
        {
        }

        public DateTime CommentDate
        {
            get
            {
                return DateTime.SpecifyKind(this.mCommentDate, DateTimeKind.Utc).ToLocalTime();
            }
        }

        public int CommenterID
        {
            get
            {
                return this.mCommenterID;
            }
        }

        public string CommenterName
        {
            get
            {
                return this.mCommenterName;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public int EntityID
        {
            get
            {
                return this.mEntityID;
            }
        }

        public int GameID
        {
            get
            {
                return this.mGameID;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }
    }
}

