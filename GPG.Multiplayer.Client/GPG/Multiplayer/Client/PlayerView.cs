namespace GPG.Multiplayer.Client
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;

    public class PlayerView
    {
        private MappedObjectList<User> mFriends;
        private User mPlayer;

        public PlayerView(User player)
        {
            this.mPlayer = player;
        }

        public PlayerView(User player, MappedObjectList<User> friends)
        {
            this.mPlayer = player;
            this.mFriends = friends;
        }

        public override bool Equals(object obj)
        {
            return (obj.GetHashCode() == this.GetHashCode());
        }

        public override int GetHashCode()
        {
            return this.Player.ID;
        }

        public MappedObjectList<User> Friends
        {
            get
            {
                return this.mFriends;
            }
            set
            {
                this.mFriends = value;
            }
        }

        public User Player
        {
            get
            {
                return this.mPlayer;
            }
            set
            {
                this.mPlayer = value;
            }
        }

        public PlayerRating Stats
        {
            get
            {
                return this.Player.Rating_1v1;
            }
        }
    }
}

