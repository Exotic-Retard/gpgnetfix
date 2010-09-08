namespace GPG.DataAccess
{
    using System;

    public class FieldIndex
    {
        private string mFieldName;
        private string mIndexName;

        public FieldIndex(string field, string index)
        {
            this.mFieldName = field;
            this.mIndexName = index;
        }

        public string FieldName
        {
            get
            {
                return this.mFieldName;
            }
        }

        public string IndexName
        {
            get
            {
                return this.mIndexName;
            }
        }
    }
}

