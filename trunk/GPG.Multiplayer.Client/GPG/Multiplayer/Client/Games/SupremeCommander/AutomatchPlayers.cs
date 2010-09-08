namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG.DataAccess;
    using System;

    public class AutomatchPlayers : MappedObject
    {
        [FieldMap("name")]
        private string mName;
        [FieldMap("principal_id")]
        private int mPlayerID;

        public AutomatchPlayers()
        {
        }

        public AutomatchPlayers(DataRecord record) : base(record)
        {
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
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
    }
}

