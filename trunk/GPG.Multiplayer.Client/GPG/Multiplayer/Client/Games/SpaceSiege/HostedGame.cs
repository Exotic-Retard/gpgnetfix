namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using GPG.DataAccess;
    using System;

    public class HostedGame : MappedObject
    {
        [FieldMap("password_hash")]
        private string mBuffer;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("host_id")]
        private int mHostID;
        [FieldMap("host_name")]
        private string mHostName;
        [FieldMap("space_siege_hosted_game_id")]
        private int mID;
        [FieldMap("max_players")]
        private string mMaxPlayers;
        [FieldMap("num_players")]
        private string mNumberOfPlayers;
        [FieldMap("avg_level")]
        private int mPlayerLevels;

        public HostedGame(DataRecord record) : base(record)
        {
        }

        public string Buffer
        {
            get
            {
                return this.mBuffer;
            }
            set
            {
                this.mBuffer = value;
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

        public bool HasPassword
        {
            get
            {
                return (this.Password != "");
            }
        }

        public int HostID
        {
            get
            {
                return this.mHostID;
            }
        }

        public string HostName
        {
            get
            {
                return this.mHostName;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public string MaxPlayers
        {
            get
            {
                return this.mMaxPlayers;
            }
        }

        public string NumberOfPlayers
        {
            get
            {
                return this.mNumberOfPlayers;
            }
        }

        public string Password
        {
            get
            {
                if (this.mBuffer == null)
                {
                    return "";
                }
                return this.mBuffer;
            }
        }

        public string PlayerCount
        {
            get
            {
                return string.Format("{0}/{1}", this.NumberOfPlayers, this.MaxPlayers);
            }
        }

        public int PlayerLevels
        {
            get
            {
                return this.mPlayerLevels;
            }
        }
    }
}

