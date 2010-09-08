namespace GPG.Multiplayer.Quazal
{
    using GPG.DataAccess;
    using GPG.Threading;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class QuazalQuery
    {
        private object[] mArgs;
        private string mName;

        public QuazalQuery()
        {
        }

        public QuazalQuery(string name, params object[] args)
        {
            this.mName = name;
            this.mArgs = args;
        }

        public bool ExecuteNonQuery()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool? result = null;
                    ThreadQueue.QueueUserWorkItem(delegate (object s) {
                        result = new bool?(DataAccess.ExecuteQuery(this.Name, this.Args));
                    }, new object[0]);
                    DateTime now = DateTime.Now;
                    while (!result.HasValue && ((DateTime.Now - now) < DefaultQueryTimeout))
                    {
                        Thread.Sleep(10);
                    }
                    if (result.HasValue)
                    {
                        return result.Value;
                    }
                    return false;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            return DataAccess.ExecuteQuery(this.Name, this.Args);
        }

        public void ExecuteNonQueryAsync()
        {
            WaitCallback callBack = null;
            if (this.InvokeRequired)
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        this.ExecuteNonQueryAsync();
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
            }
            else
            {
                DataAccess.ExecuteQuery(this.Name, this.Args);
            }
        }

        public bool GetBool()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool? result = null;
                    ThreadQueue.QueueUserWorkItem(delegate (object s) {
                        result = new bool?(DataAccess.GetBool(this.Name, this.Args));
                    }, new object[0]);
                    DateTime now = DateTime.Now;
                    while (!result.HasValue && ((DateTime.Now - now) < DefaultQueryTimeout))
                    {
                        Thread.Sleep(10);
                    }
                    if (result.HasValue)
                    {
                        return result.Value;
                    }
                    return false;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            return DataAccess.GetBool(this.Name, this.Args);
        }

        public DataList GetData()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool resultReturned = false;
                    DataList result = null;
                    ThreadQueue.QueueUserWorkItem(delegate (object s) {
                        result = DataAccess.GetQueryData(this.Name, this.Args);
                        resultReturned = true;
                    }, new object[0]);
                    DateTime now = DateTime.Now;
                    while (!resultReturned && ((DateTime.Now - now) < DefaultQueryTimeout))
                    {
                        Thread.Sleep(10);
                    }
                    if (resultReturned)
                    {
                        return result;
                    }
                    return null;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            return DataAccess.GetQueryData(this.Name, this.Args);
        }

        public int GetInt()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int? result = null;
                    ThreadQueue.QueueUserWorkItem(delegate (object s) {
                        result = new int?(DataAccess.GetNumber(this.Name, this.Args));
                    }, new object[0]);
                    DateTime now = DateTime.Now;
                    while (!result.HasValue && ((DateTime.Now - now) < DefaultQueryTimeout))
                    {
                        Thread.Sleep(10);
                    }
                    if (result.HasValue)
                    {
                        return result.Value;
                    }
                    return -1;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            return DataAccess.GetNumber(this.Name, this.Args);
        }

        public type GetObject<type>() where type: MappedObject
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool resultReturned = false;
                    type result = default(type);
                    ThreadQueue.QueueUserWorkItem(delegate (object s) {
                        result = DataAccess.GetObject<type>(this.Name, this.Args);
                        resultReturned = true;
                    }, new object[0]);
                    DateTime now = DateTime.Now;
                    while (!resultReturned && ((DateTime.Now - now) < DefaultQueryTimeout))
                    {
                        Thread.Sleep(10);
                    }
                    if (resultReturned)
                    {
                        return result;
                    }
                    return default(type);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            return DataAccess.GetObject<type>(this.Name, this.Args);
        }

        public bool GetObject<type>(out type obj) where type: MappedObject
        {
            obj = this.GetObject<type>();
            return (((type) obj) != null);
        }

        public MappedObjectList<type> GetObjects<type>() where type: MappedObject
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool resultReturned = false;
                    MappedObjectList<type> result = null;
                    ThreadQueue.QueueUserWorkItem(delegate (object s) {
                        result = DataAccess.GetObjects<type>(this.Name, this.Args);
                        resultReturned = true;
                    }, new object[0]);
                    DateTime now = DateTime.Now;
                    while (!resultReturned && ((DateTime.Now - now) < DefaultQueryTimeout))
                    {
                        Thread.Sleep(10);
                    }
                    if (resultReturned)
                    {
                        return result;
                    }
                    return null;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            return DataAccess.GetObjects<type>(this.Name, this.Args);
        }

        public bool GetObjects<type>(out MappedObjectList<type> obj) where type: MappedObject
        {
            obj = this.GetObjects<type>();
            return (obj != null);
        }

        public string GetString()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    bool resultReturned = false;
                    string result = null;
                    ThreadQueue.QueueUserWorkItem(delegate (object s) {
                        result = DataAccess.GetString(this.Name, this.Args);
                        resultReturned = true;
                    }, new object[0]);
                    DateTime now = DateTime.Now;
                    while (!resultReturned && ((DateTime.Now - now) < DefaultQueryTimeout))
                    {
                        Thread.Sleep(10);
                    }
                    if (resultReturned)
                    {
                        return result;
                    }
                    return null;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            return DataAccess.GetString(this.Name, this.Args);
        }

        public object[] Args
        {
            get
            {
                return this.mArgs;
            }
            set
            {
                this.mArgs = value;
            }
        }

        public static TimeSpan DefaultQueryTimeout
        {
            get
            {
                return TimeSpan.FromSeconds((double) ConfigSettings.GetInt("DefaultQueryTimeout", 10));
            }
        }

        public static QuazalQuery Empty
        {
            get
            {
                return new QuazalQuery();
            }
        }

        public bool InvokeRequired
        {
            get
            {
                return DataAccess.InvokeRequired;
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (this.Name != null)
                {
                    return (this.Name.Length < 1);
                }
                return true;
            }
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
    }
}

