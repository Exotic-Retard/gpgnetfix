namespace GPG.UI
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    [Serializable]
    public class RasterAnimationFrameCollection : IEnumerable<RasterAnimationFrame>, IEnumerable
    {
        private List<RasterAnimationFrame> mInnerList = new List<RasterAnimationFrame>();

        public void Add(RasterAnimationFrame frame)
        {
            if (this.InnerList.Count > 0)
            {
                this[this.Count - 1].NextFrame = frame;
                frame.PreviousFrame = this[this.Count - 1];
            }
            this.InnerList.Add(frame);
        }

        public void Clear()
        {
            this.InnerList.Clear();
        }

        public IEnumerator<RasterAnimationFrame> GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        public void Insert(int index, RasterAnimationFrame frame)
        {
            if (this.InnerList.Count > 0)
            {
                this[index].NextFrame = frame;
                frame.PreviousFrame = this[index];
            }
            this.InnerList.Insert(index, frame);
        }

        public void Remove(RasterAnimationFrame frame)
        {
            this.RemoveAt(this.InnerList.IndexOf(frame));
        }

        public void RemoveAt(int index)
        {
            if (this.InnerList.Count > 1)
            {
                try
                {
                    this[index - 1].NextFrame = this[index + 1];
                    this[index + 1].PreviousFrame = this[index - 1];
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
            this.InnerList.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this.InnerList.Count;
            }
        }

        internal List<RasterAnimationFrame> InnerList
        {
            get
            {
                return this.mInnerList;
            }
        }

        public RasterAnimationFrame this[int index]
        {
            get
            {
                return this.InnerList[index];
            }
        }
    }
}

