namespace GPG.Multiplayer.Client.Games
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;

    [IndexMap("mKeyCharacters", "chars")]
    public class GameKey : MappedObject
    {
        private static MappedObjectList<GameKey> mBetaKeys = null;
        [FieldMap("description")]
        private string mGameName = null;
        [FieldMap("key_chars")]
        private string mKeyCharacters = null;

        public GameKey(DataRecord record) : base(record)
        {
        }

        public GameKey(string key, string gameName)
        {
            this.mKeyCharacters = key;
            this.mGameName = gameName;
        }

        public override bool Equals(object obj)
        {
            return (obj.GetHashCode() == this.GetHashCode());
        }

        public override int GetHashCode()
        {
            return this.KeyCharacters.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.GameName, this.KeyCharacters);
        }

        public static MappedObjectList<GameKey> BetaKeys
        {
            get
            {
                if (mBetaKeys == null)
                {
                    mBetaKeys = DataAccess.GetObjects<GameKey>("GetBetaKeys", new object[0]);
                    if (mBetaKeys == null)
                    {
                        mBetaKeys = new MappedObjectList<GameKey>();
                    }
                    for (int i = 0; i < BetaKeys.Count; i++)
                    {
                        BetaKeys.IndexObject(BetaKeys[i]);
                    }
                }
                return mBetaKeys;
            }
            set
            {
                mBetaKeys = value;
            }
        }

        public string GameName
        {
            get
            {
                return this.mGameName;
            }
        }

        public string KeyCharacters
        {
            get
            {
                return this.mKeyCharacters;
            }
        }
    }
}

