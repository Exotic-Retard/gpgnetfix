namespace GPG.UI
{
    using System;
    using System.ComponentModel;

    public class BoundContainerList : BindingList<TextContainer>
    {
        public TextLine Peek(string key)
        {
            foreach (TextContainer container in this)
            {
                TextLine line = container.Peek(key);
                if (line != null)
                {
                    return line;
                }
            }
            return null;
        }

        public TextLine Pop(string key)
        {
            foreach (TextContainer container in this)
            {
                TextLine line = container.Pop(key);
                if (line != null)
                {
                    return line;
                }
            }
            return null;
        }

        public bool RemoveItem(string key)
        {
            if (key != null)
            {
                foreach (TextContainer container in this)
                {
                    if (container.Remove(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int ItemCount
        {
            get
            {
                int num = 0;
                foreach (TextContainer container in this)
                {
                    num += container.Count;
                }
                return num;
            }
        }
    }
}

