namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using System;

    public class PlayerRating : MappedObject
    {
        [FieldMap("avg_rating")]
        private float mAvgRating;
        [FieldMap("category")]
        private string mCategory;
        [FieldMap("disconnects")]
        private int mDisconnects;
        [FieldMap("draws")]
        private int mDraws;
        [FieldMap("losses")]
        private int mLosses;
        [FieldMap("people")]
        private int mPeople;
        [FieldMap("principal_id")]
        private int mPlayerID;
        [FieldMap("name")]
        private string mPlayerName;
        [FieldMap("rank")]
        private int mRank;
        [FieldMap("rating")]
        private float mRating;
        [FieldMap("topten_rating")]
        private float mTopTenRating;
        [FieldMap("wins")]
        private int mWins;

        public PlayerRating(DataRecord record) : base(record)
        {
        }

        public PlayerRating(int playerId, string category)
        {
            this.mPlayerID = playerId;
            this.mCategory = category;
        }

        public int AvgRating
        {
            get
            {
                return (int) this.mAvgRating;
            }
        }

        public string Category
        {
            get
            {
                return this.mCategory;
            }
        }

        public float DisconnectPercentage
        {
            get
            {
                if (this.TotalGames == 0)
                {
                    return 0f;
                }
                return (float) Math.Round((double) ((((float) this.Disconnects) / ((float) this.TotalGames)) * 100f), 2);
            }
        }

        public int Disconnects
        {
            get
            {
                return this.mDisconnects;
            }
        }

        public int Draws
        {
            get
            {
                return this.mDraws;
            }
        }

        public int Losses
        {
            get
            {
                return this.mLosses;
            }
        }

        public int People
        {
            get
            {
                return this.mPeople;
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

        public int Rank
        {
            get
            {
                return this.mRank;
            }
        }

        public int Rating
        {
            get
            {
                return (int) this.mRating;
            }
        }

        public int TopTenRating
        {
            get
            {
                return (int) this.mTopTenRating;
            }
        }

        public int TotalGames
        {
            get
            {
                return ((this.Wins + this.Losses) + this.Draws);
            }
        }

        public float WinPercentage
        {
            get
            {
                if (this.TotalGames == 0)
                {
                    return 0f;
                }
                return (float) Math.Round((double) (((this.Wins + (((float) this.Draws) / 2f)) / ((float) this.TotalGames)) * 100f), 2);
            }
        }

        public int Wins
        {
            get
            {
                return this.mWins;
            }
        }
    }
}

