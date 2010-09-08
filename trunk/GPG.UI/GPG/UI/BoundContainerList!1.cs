namespace GPG.UI
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public class BoundContainerList<lineType> : BindingList<TextContainer<lineType>> where lineType: TextLine
    {
        public BoundContainerList<lineType> Clone()
        {
            BoundContainerList<lineType> list = new BoundContainerList<lineType>();
            TextContainer<lineType>[] array = new TextContainer<lineType>[base.Count];
            base.CopyTo(array, 0);
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(array[i]);
            }
            return list;
        }

        public lineType Peek(string key)
        {
            foreach (TextContainer<lineType> container in this)
            {
                lineType local = container.Peek(key);
                if (local != null)
                {
                    return local;
                }
            }
            return default(lineType);
        }

        public lineType Pop(string key)
        {
            foreach (TextContainer<lineType> container in this)
            {
                lineType local = container.Pop(key);
                if (local != null)
                {
                    return local;
                }
            }
            return default(lineType);
        }

        public lineType Pop(string key, out TextContainer<lineType> c)
        {
            foreach (TextContainer<lineType> container in this)
            {
                lineType local = container.Pop(key);
                if (local != null)
                {
                    c = container;
                    return local;
                }
            }
            c = null;
            return default(lineType);
        }

        public bool RemoveItem(string key)
        {
            if (key != null)
            {
                foreach (TextContainer<lineType> container in this)
                {
                    if (container.Remove(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveItem(string key, out TextContainer<lineType> c)
        {
            if (key != null)
            {
                foreach (TextContainer<lineType> container in this)
                {
                    if (container.Remove(key))
                    {
                        c = container;
                        return true;
                    }
                }
            }
            c = null;
            return false;
        }

        public int ItemCount
        {
            get
            {
                int num = 0;
                foreach (TextContainer<lineType> container in this)
                {
                    num += container.Count;
                }
                return num;
            }
        }
    }
}

