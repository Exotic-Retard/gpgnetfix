namespace GPG.UI
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Reflection;

    public class TextContainer<lineType> : BindingList<lineType> where lineType: TextLine
    {
        private bool _IsSorted;
        private ListSortDirection _SortDirection;
        private PropertyDescriptor _SortProperty;
        private bool mAutoSort;
        private bool mModified;
        private string mTitle;
        private Hashtable NameLookup;
        private PropertyInfo SearchProperty;
        private bool Sorting;

        public TextContainer(string title)
        {
            this.NameLookup = Hashtable.Synchronized(CollectionsUtil.CreateCaseInsensitiveHashtable());
            this.mTitle = title;
        }

        public void Add(lineType line)
        {
            lock (((TextContainer<lineType>) this))
            {
                string keyText = line.KeyText;
                if (keyText != null)
                {
                    if (this.NameLookup.ContainsKey(keyText))
                    {
                        int num = this.Find(line.ToString());
                        if (num >= 0)
                        {
                            this.NameLookup[keyText] = line;
                            this[num] = line;
                        }
                        else
                        {
                            this.NameLookup[keyText] = line;
                            base.Add(line);
                        }
                    }
                    else
                    {
                        this.NameLookup[keyText] = line;
                        base.Add(line);
                    }
                }
            }
        }

        protected override object AddNewCore()
        {
            this._IsSorted = false;
            return base.AddNewCore();
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            try
            {
                this.Sorting = true;
                ArrayList list = new ArrayList(base.Count);
                list.AddRange(this);
                list.Sort();
                if (direction == ListSortDirection.Descending)
                {
                    list.Reverse();
                }
                for (int i = 0; i < list.Count; i++)
                {
                    int num2 = this.Find(list[i].ToString());
                    if (num2 != i)
                    {
                        lineType local = this[i];
                        this[i] = this[num2];
                        this[num2] = local;
                    }
                }
                list = null;
                this._IsSorted = true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine("Chat sorting failed:", new object[0]);
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                this.mModified = false;
                this.Sorting = false;
            }
        }

        public void Clear()
        {
            lock (((TextContainer<lineType>) this))
            {
                base.Clear();
                this.NameLookup.Clear();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is TextContainer)
            {
                return (this.GetHashCode() == obj.GetHashCode());
            }
            return base.Equals(obj);
        }

        public int Find(string value)
        {
            return this.FindCore(null, value);
        }

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            try
            {
                if (this.SearchProperty == null)
                {
                    this.SearchProperty = typeof(lineType).GetProperty("KeyText");
                }
                if (key != null)
                {
                    for (int i = 0; i < base.Count; i++)
                    {
                        lineType local = this[i];
                        if (this.SearchProperty.GetValue(local, null).Equals(key))
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return -1;
            }
        }

        public override int GetHashCode()
        {
            return this.mTitle.GetHashCode();
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            try
            {
                this.mModified = true;
                if (!this.Sorting)
                {
                    if (this.AutoSort)
                    {
                        this.ApplySortCore(this.SortPropertyCore, this.SortDirectionCore);
                    }
                    base.OnListChanged(e);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public lineType Peek(string key)
        {
            if (this.NameLookup.ContainsKey(key))
            {
                int num = this.Find(key);
                if (num >= 0)
                {
                    return this[num];
                }
            }
            return default(lineType);
        }

        public lineType Pop(string key)
        {
            if (this.NameLookup.ContainsKey(key))
            {
                int index = this.Find(key);
                if (index >= 0)
                {
                    lineType local = this[index];
                    this.NameLookup.Remove(key);
                    base.RemoveAt(index);
                    return local;
                }
            }
            return default(lineType);
        }

        public lineType Pop(lineType line)
        {
            return this.Pop(line.KeyText);
        }

        public bool Remove(lineType line)
        {
            return this.Remove(line.KeyText);
        }

        public bool Remove(string line)
        {
            lock (((TextContainer<lineType>) this))
            {
                if (this.NameLookup.ContainsKey(line))
                {
                    int index = this.Find(line);
                    if (index >= 0)
                    {
                        this.NameLookup.Remove(line);
                        base.RemoveAt(index);
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        public void Sort()
        {
            this.ApplySortCore(this.SortPropertyCore, this.SortDirectionCore);
            this._IsSorted = true;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public bool AutoSort
        {
            get
            {
                return this.mAutoSort;
            }
            set
            {
                this.mAutoSort = value;
            }
        }

        protected override bool IsSortedCore
        {
            get
            {
                return this._IsSorted;
            }
        }

        public lineType this[int index]
        {
            get
            {
                lock (((TextContainer<lineType>) this))
                {
                    return base[index];
                }
            }
            set
            {
                lock (((TextContainer<lineType>) this))
                {
                    base[index] = value;
                }
            }
        }

        public TextContainer<lineType> Members
        {
            get
            {
                return (TextContainer<lineType>) this;
            }
        }

        public bool Modified
        {
            get
            {
                return this.mModified;
            }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get
            {
                return this._SortDirection;
            }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get
            {
                if (this._SortProperty == null)
                {
                    this._SortProperty = TypeDescriptor.GetProperties(typeof(TextLine)).Find("Text", false);
                }
                return this._SortProperty;
            }
        }

        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        public string Title
        {
            get
            {
                return string.Format("{0} ({1})", this.mTitle, base.Count);
            }
            set
            {
                this.mTitle = value;
            }
        }
    }
}

