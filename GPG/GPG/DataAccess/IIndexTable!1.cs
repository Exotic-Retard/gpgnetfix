namespace GPG.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public interface IIndexTable<type>
    {
        bool ContainsIndex(string indexName, object value);
        type FindByIndex(string indexName, object indexValue);
        void IndexObject(type obj);
        bool RemoveByIndex(string indexName, object indexValue);
        bool TryFindByIndex(string indexName, object indexValue, out type record);

        FieldIndex[] IndexedFields { get; }

        Dictionary<string, Dictionary<object, type>> Indexes { get; }
    }
}

