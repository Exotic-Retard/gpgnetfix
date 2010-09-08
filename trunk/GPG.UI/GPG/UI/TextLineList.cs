namespace GPG.UI
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class TextLineList : CollectionBase
    {
        public event PropertyChangedEventHandler CountChanged;

        public void Add(TextLine line)
        {
            base.InnerList.Add(line);
            if (this.CountChanged != null)
            {
                this.CountChanged(this, new PropertyChangedEventArgs("Count"));
            }
        }

        public void Insert(int index, TextLine line)
        {
            base.InnerList.Insert(index, line);
            if (this.CountChanged != null)
            {
                this.CountChanged(this, new PropertyChangedEventArgs("Count"));
            }
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            if (this.CountChanged != null)
            {
                this.CountChanged(this, new PropertyChangedEventArgs("Count"));
            }
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            if (this.CountChanged != null)
            {
                this.CountChanged(this, new PropertyChangedEventArgs("Count"));
            }
        }

        public void Remove(TextLine line)
        {
            base.InnerList.Remove(line);
        }

        public TextLine[] ToArray()
        {
            return (base.InnerList.ToArray(typeof(TextLine)) as TextLine[]);
        }

        public TextLine this[int index]
        {
            get
            {
                return (base.InnerList[index] as TextLine);
            }
        }
    }
}

