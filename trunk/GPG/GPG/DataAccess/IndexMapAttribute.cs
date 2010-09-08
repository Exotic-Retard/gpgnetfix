namespace GPG.DataAccess
{
    using GPG.Logging;
    using System;

    public class IndexMapAttribute : GPGDataAccessAttribute
    {
        private FieldIndex[] mIndexes;

        public IndexMapAttribute(string name)
        {
            this.mIndexes = new FieldIndex[] { new FieldIndex(name, name) };
        }

        public IndexMapAttribute(string fieldName, string indexName)
        {
            this.mIndexes = new FieldIndex[] { new FieldIndex(fieldName, indexName) };
        }

        public IndexMapAttribute(string[] fields, string[] indexes)
        {
            if (fields.Length != indexes.Length)
            {
                throw ErrorLog.WriteLine(new ArgumentException("The length of 'fields' and 'indexes' must match when creating an IndexMapAttribute."));
            }
            this.mIndexes = new FieldIndex[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                this.mIndexes[i] = new FieldIndex(fields[i], indexes[i]);
            }
        }

        public FieldIndex[] Indexes
        {
            get
            {
                return this.mIndexes;
            }
        }
    }
}

