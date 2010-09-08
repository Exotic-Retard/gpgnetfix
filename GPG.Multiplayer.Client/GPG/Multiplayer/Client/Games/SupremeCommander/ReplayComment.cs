namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG.DataAccess;
    using System;

    public class ReplayComment : MappedObject
    {
        [FieldMap("description")]
        private string mComment;
        [FieldMap("create_date")]
        private DateTime mCreateDate;
        [FieldMap("replay_comment_id")]
        private int mID;
        [FieldMap("player_id")]
        private int mPlayerID;
        [FieldMap("name")]
        private string mPlayerName;

        public ReplayComment(DataRecord record) : base(record)
        {
        }

        public string Comment
        {
            get
            {
                return this.mComment;
            }
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

