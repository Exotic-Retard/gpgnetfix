namespace GPG.Multiplayer.Client
{
    using System;

    internal class TimePoint
    {
        private int mCount;
        private DateTime mDate;

        public int Count
        {
            get
            {
                return this.mCount;
            }
            set
            {
                this.mCount = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.mDate;
            }
            set
            {
                this.mDate = value;
            }
        }
    }
}

