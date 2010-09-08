namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.DataAccess;
    using System;

    public class UserLocation : MappedObject
    {
        [FieldMap("game_state")]
        private int mGameState;
        [FieldMap("in_game")]
        private bool mInGame;
        [FieldMap("location")]
        private string mLocation;
        [FieldMap("online")]
        private bool mOnline;
        [FieldMap("player_id")]
        private int mPlayerID;
        [FieldMap("player_name")]
        private string mPlayerName;

        public UserLocation(DataRecord record) : base(record)
        {
        }

        public override string ToString()
        {
            if (!this.Online)
            {
                return string.Format(Loc.Get("<LOC>{0} is offline."), this.PlayerName);
            }
            if (this.InGame)
            {
                if (this.GameState == 0)
                {
                    return string.Format(Loc.Get("<LOC>{0} is in game lobby game:'{1}'."), this.PlayerName, this.Location);
                }
                return string.Format(Loc.Get("<LOC>{0} is in an active game."), this.PlayerName);
            }
            if (this.Location != null)
            {
                return string.Format(Loc.Get("<LOC>{0} is chatting in chat:'{1}'."), this.PlayerName, this.Location);
            }
            return string.Format(Loc.Get("<LOC>{0} is online."), this.PlayerName);
        }

        public int GameState
        {
            get
            {
                return this.mGameState;
            }
            set
            {
                this.mGameState = value;
            }
        }

        public bool InGame
        {
            get
            {
                return this.mInGame;
            }
            set
            {
                this.mInGame = value;
            }
        }

        public string Location
        {
            get
            {
                return this.mLocation;
            }
            set
            {
                this.mLocation = value;
            }
        }

        public bool Online
        {
            get
            {
                return this.mOnline;
            }
            set
            {
                this.mOnline = value;
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
            set
            {
                this.mPlayerID = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
            set
            {
                this.mPlayerName = value;
            }
        }
    }
}

