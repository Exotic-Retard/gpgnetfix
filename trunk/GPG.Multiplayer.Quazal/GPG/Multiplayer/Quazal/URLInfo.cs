namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using System;

    public class URLInfo : MappedObject
    {
        [FieldMap("id")]
        private string mId;
        [FieldMap("url")]
        private string mUrl;

        public URLInfo(DataRecord record) : base(record)
        {
        }

        public string Id
        {
            get
            {
                return this.mId;
            }
            set
            {
                this.mId = value;
            }
        }

        public string Url
        {
            get
            {
                return this.mUrl;
            }
            set
            {
                this.mUrl = value;
            }
        }
    }
}

