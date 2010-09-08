namespace GPG.Multiplayer.Statistics
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;

    public class PlayerDisplayAwards : MappedObject
    {
        [FieldMap("avatar")]
        private int mAvatarID;
        [FieldMap("award1")]
        private int mAward1;
        [FieldMap("award2")]
        private int mAward2;
        [FieldMap("award3")]
        private int mAward3;
        [FieldMap("player_display_awards_id")]
        private int mID;
        [FieldMap("player_id")]
        private int mPlayerID;

        public PlayerDisplayAwards(DataRecord record) : base(record)
        {
        }

        public PlayerDisplayAwards(User player)
        {
            this.mPlayerID = player.ID;
            this.mAward1 = player.Award1;
            this.mAward2 = player.Award2;
            this.mAward3 = player.Award3;
            this.mAvatarID = player.Avatar;
        }

        public GPG.Multiplayer.Statistics.Avatar Avatar
        {
            get
            {
                if ((this.mAvatarID > 0) && GPG.Multiplayer.Statistics.Avatar.AllAvatars.ContainsKey(this.mAvatarID))
                {
                    return GPG.Multiplayer.Statistics.Avatar.AllAvatars[this.mAvatarID];
                }
                return GPG.Multiplayer.Statistics.Avatar.Default;
            }
        }

        public Award Award1
        {
            get
            {
                if ((this.mAward1 > 0) && Award.AllAwards.ContainsKey(this.mAward1))
                {
                    return Award.AllAwards[this.mAward1];
                }
                return null;
            }
        }

        public bool Award1Specified
        {
            get
            {
                return (this.Award1 != null);
            }
        }

        public Award Award2
        {
            get
            {
                if ((this.mAward2 > 0) && Award.AllAwards.ContainsKey(this.mAward2))
                {
                    return Award.AllAwards[this.mAward2];
                }
                return null;
            }
        }

        public bool Award2Specified
        {
            get
            {
                return (this.Award2 != null);
            }
        }

        public Award Award3
        {
            get
            {
                if ((this.mAward3 > 0) && Award.AllAwards.ContainsKey(this.mAward3))
                {
                    return Award.AllAwards[this.mAward3];
                }
                return null;
            }
        }

        public bool Award3Specified
        {
            get
            {
                return (this.Award3 != null);
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
    }
}

