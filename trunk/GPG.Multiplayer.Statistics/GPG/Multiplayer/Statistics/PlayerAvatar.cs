namespace GPG.Multiplayer.Statistics
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;

    public class PlayerAvatar : MappedObject
    {
        [FieldMap("avatar_id")]
        private int mAvatarID;
        [FieldMap("player_avatar_id")]
        private int mID;
        [FieldMap("manual_assignment")]
        private bool mManualAssignment;
        [FieldMap("player_id")]
        private int mPlayerID;

        private PlayerAvatar()
        {
        }

        public PlayerAvatar(DataRecord record) : base(record)
        {
        }

        public static PlayerAvatar[] GetPlayerAvatars(int playerId)
        {
            PlayerAvatar item = new PlayerAvatar();
            item.mPlayerID = playerId;
            item.mAvatarID = 0;
            MappedObjectList<PlayerAvatar> objects = new QuazalQuery("GetPlayerAvatarsByID", new object[] { playerId }).GetObjects<PlayerAvatar>();
            objects.Insert(0, item);
            return objects.ToArray();
        }

        public GPG.Multiplayer.Statistics.Avatar Avatar
        {
            get
            {
                if (this.mAvatarID <= 0)
                {
                    return GPG.Multiplayer.Statistics.Avatar.Default;
                }
                return GPG.Multiplayer.Statistics.Avatar.AllAvatars[this.mAvatarID];
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool ManualAssignment
        {
            get
            {
                return this.mManualAssignment;
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

