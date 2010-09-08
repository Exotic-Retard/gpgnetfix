namespace GPGnetCommunicationsLib
{
    using System;

    [Serializable]
    public class TestSerializer
    {
        private int mId;
        private string mValue;

        public override string ToString()
        {
            return (this.mId.ToString() + " " + this.Value.ToString());
        }

        public int Id
        {
            get
            {
                return this.mId;
            }
            set
            {
                this.mId = value;
            }
        }

        public string Value
        {
            get
            {
                return this.mValue;
            }
            set
            {
                this.mValue = value;
            }
        }
    }
}

