namespace GPG.DataAccess
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Reflection;

    public class DataRecord
    {
        private Hashtable mInnerHash = CollectionsUtil.CreateCaseInsensitiveHashtable();

        public DataRecord(string[] fields, string[] values)
        {
            this.mInnerHash = Hashtable.Synchronized(this.InnerHash);
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Length > 0)
                {
                    this.InnerHash[fields[i]] = values[i];
                }
            }
        }

        public static DataRecord FromDataString(string dataString)
        {
            try
            {
                string[] fields = dataString.Split(new char[] { '\x0003' })[0].Split(new char[] { '|' });
                return new DataRecord(fields, dataString.Split(new char[] { '\x0003' })[1].Split(new char[] { '|' }));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        public void SetValue(string field, string value)
        {
            if (!this.InnerHash.ContainsKey(field))
            {
                throw new Exception("That field does not exist.");
            }
            this.InnerHash[field] = value;
        }

        public int Count
        {
            get
            {
                IEnumerator enumerator = this.InnerHash.Values.GetEnumerator();
                int num = 0;
                while (enumerator.MoveNext())
                {
                    num++;
                }
                return num;
            }
        }

        public Hashtable InnerHash
        {
            get
            {
                return this.mInnerHash;
            }
        }

        public string this[int index]
        {
            get
            {
                if (index >= this.InnerHash.Count)
                {
                    throw new IndexOutOfRangeException(string.Format("Provided index of {0} is out of range of list count: {1}", index, this.InnerHash.Count));
                }
                IEnumerator enumerator = this.InnerHash.Values.GetEnumerator();
                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    if (i == index)
                    {
                        return (string) enumerator.Current;
                    }
                }
                throw new IndexOutOfRangeException(string.Format("Provided index of {0} is out of range of list count: {1}", index, this.InnerHash.Count));
            }
        }

        public string this[string field]
        {
            get
            {
                if (this.InnerHash.ContainsKey(field))
                {
                    return (string) this.InnerHash[field];
                }
                return null;
            }
        }
    }
}

