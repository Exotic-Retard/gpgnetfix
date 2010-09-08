namespace GPG
{
    using System;

    public class MultiVal<t1, t2>
    {
        private t1 mValue1;
        private t2 mValue2;

        public MultiVal(t1 val1, t2 val2)
        {
            this.mValue1 = val1;
            this.mValue2 = val2;
        }

        public override bool Equals(object obj)
        {
            return (obj.GetHashCode() == this.GetHashCode());
        }

        public override int GetHashCode()
        {
            return (this.Value1.GetHashCode() ^ this.Value2.GetHashCode());
        }

        public override string ToString()
        {
            return this.Value1.ToString();
        }

        public t1 Value1
        {
            get
            {
                return this.mValue1;
            }
            set
            {
                this.mValue1 = value;
            }
        }

        public t2 Value2
        {
            get
            {
                return this.mValue2;
            }
            set
            {
                this.mValue2 = value;
            }
        }
    }
}

