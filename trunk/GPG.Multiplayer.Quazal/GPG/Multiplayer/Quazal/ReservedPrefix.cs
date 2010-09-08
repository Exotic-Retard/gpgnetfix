namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using System;

    public class ReservedPrefix : MappedObject
    {
        [FieldMap("prefix")]
        private string mPrefix;
        [FieldMap("reason")]
        private string mReason;

        public ReservedPrefix(DataRecord record) : base(record)
        {
        }

        public string Prefix
        {
            get
            {
                return this.mPrefix;
            }
        }

        public string Reason
        {
            get
            {
                return this.mReason;
            }
        }
    }
}

