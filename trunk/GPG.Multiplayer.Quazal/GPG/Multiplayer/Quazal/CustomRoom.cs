namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using System;

    public class CustomRoom : MappedObject
    {
        [FieldMap("start")]
        private DateTime mCreated = DateTime.MinValue;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("is_public")]
        private bool mIsPublic;
        [FieldMap("max_populate")]
        private int mMaxPopulation = -1;
        [FieldMap("owner")]
        private string mOwner;
        [FieldMap("pwd")]
        private string mPassword;
        [FieldMap("population")]
        private int mPopulation = -1;

        public CustomRoom(DataRecord record) : base(record)
        {
        }

        public DateTime Created
        {
            get
            {
                return this.mCreated;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public bool IsPublic
        {
            get
            {
                return this.mIsPublic;
            }
        }

        public int MaxPopulation
        {
            get
            {
                return this.mMaxPopulation;
            }
        }

        public string Owner
        {
            get
            {
                return this.mOwner;
            }
        }

        public string Password
        {
            get
            {
                return this.mPassword;
            }
        }

        public bool PasswordProtected
        {
            get
            {
                return ((this.Password != null) && (this.Password.Length > 0));
            }
        }

        public int Population
        {
            get
            {
                return this.mPopulation;
            }
        }
    }
}

