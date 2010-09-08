namespace GPG.DataAccess
{
    using GPG.Logging;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class MappedObjectList<type> : BindingList<type>, IIndexTable<type> where type: class
    {
        private FieldIndex[] mIndexedFields;
        private Dictionary<string, Dictionary<object, type>> mIndexes;

        public MappedObjectList()
        {
            this.mIndexes = new Dictionary<string, Dictionary<object, type>>();
            this.Construct(null);
        }

        public MappedObjectList(DataList data)
        {
            this.mIndexes = new Dictionary<string, Dictionary<object, type>>();
            this.Construct(data);
        }

        public MappedObjectList(string data)
        {
            this.mIndexes = new Dictionary<string, Dictionary<object, type>>();
            this.Construct(new DataList(data));
        }

        private void ClearIndexes()
        {
            foreach (Dictionary<object, type> dictionary in this.mIndexes.Values)
            {
                dictionary.Clear();
            }
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            this.ClearIndexes();
        }

        private void Construct(DataList data)
        {
            this.CreateIndexes();
            if (data != null)
            {
                foreach (DataRecord record in data)
                {
                    type item = MappedObject.FromData(typeof(type), record) as type;
                    base.Add(item);
                    if (this.IndexedFields.Length > 0)
                    {
                        this.IndexObject(item);
                    }
                }
            }
        }

        public bool ContainsIndex(string indexName, object value)
        {
            if (((indexName == null) || (value == null)) || !this.Indexes.ContainsKey(indexName))
            {
                return false;
            }
            if (value is string)
            {
                value = ((string) value).ToLower();
            }
            return this.Indexes[indexName].ContainsKey(value);
        }

        private void CreateIndexes()
        {
            object[] customAttributes = typeof(type).GetCustomAttributes(typeof(IndexMapAttribute), true);
            if (customAttributes.Length > 0)
            {
                this.mIndexedFields = (customAttributes[0] as IndexMapAttribute).Indexes;
                for (int i = 0; i < this.mIndexedFields.Length; i++)
                {
                    this.Indexes[this.IndexedFields[i].IndexName] = new Dictionary<object, type>();
                }
            }
            else
            {
                this.mIndexedFields = new FieldIndex[0];
            }
        }

        public type FindByIndex(string indexName, object indexValue)
        {
            type local;
            if (this.TryFindByIndex(indexName, indexValue, out local))
            {
                return local;
            }
            return default(type);
        }

        public void IndexObject(type obj)
        {
            try
            {
                for (int i = 0; i < this.IndexedFields.Length; i++)
                {
                    object obj2 = typeof(type).GetField(this.IndexedFields[i].FieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(obj);
                    if (obj2 != null)
                    {
                        if (obj2 is string)
                        {
                            obj2 = ((string) obj2).ToLower();
                        }
                        this.Indexes[this.IndexedFields[i].IndexName][obj2] = obj;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public bool RemoveByIndex(string indexName, object indexValue)
        {
            type local;
            if (!this.TryFindByIndex(indexName, indexValue, out local))
            {
                return false;
            }
            base.Remove(local);
            if (indexValue is string)
            {
                indexValue = ((string) indexValue).ToLower();
            }
            this.Indexes[indexName].Remove(indexValue);
            return true;
        }

        public void RemoveIndexes(type obj)
        {
            try
            {
                for (int i = 0; i < this.IndexedFields.Length; i++)
                {
                    object key = typeof(type).GetField(this.IndexedFields[i].FieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(obj);
                    if (key != null)
                    {
                        if (key is string)
                        {
                            key = ((string) key).ToLower();
                        }
                        this.Indexes[this.IndexedFields[i].IndexName].Remove(key);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void ReplaceIntoByIndex(string indexName, object indexValue, type record)
        {
            try
            {
                if (((indexName != null) && (indexValue != null)) && this.Indexes.ContainsKey(indexName))
                {
                    if (indexValue is string)
                    {
                        indexValue = ((string) indexValue).ToLower();
                    }
                    this.Indexes[indexName][indexValue] = record;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                ErrorLog.WriteLine("Index Name: " + indexName, new object[0]);
                ErrorLog.WriteLine("Index Value: " + indexValue.ToString(), new object[0]);
                if (record != null)
                {
                    ErrorLog.WriteLine("Record is not null", new object[0]);
                }
                else
                {
                    ErrorLog.WriteLine("Record is NULL", new object[0]);
                }
            }
        }

        public type[] ToArray()
        {
            type[] localArray = new type[base.Count];
            for (int i = 0; i < base.Count; i++)
            {
                localArray[i] = base[i];
            }
            return localArray;
        }

        public bool TryFindByIndex(string indexName, object indexValue, out type record)
        {
            if (((indexName != null) && (indexValue != null)) && this.Indexes.ContainsKey(indexName))
            {
                if (indexValue is string)
                {
                    indexValue = ((string) indexValue).ToLower();
                }
                if (this.Indexes[indexName].ContainsKey(indexValue))
                {
                    record = this.Indexes[indexName][indexValue];
                    return true;
                }
                record = default(type);
                return false;
            }
            record = default(type);
            return false;
        }

        public FieldIndex[] IndexedFields
        {
            get
            {
                return this.mIndexedFields;
            }
        }

        public Dictionary<string, Dictionary<object, type>> Indexes
        {
            get
            {
                return this.mIndexes;
            }
        }
    }
}

