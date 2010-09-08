namespace GPG
{
    using System;

    public class MultiVal<t1, t2, t3>
    {
        private t1 mValue1;
        private t2 mValue2;
        private t3 mValue3;

        public MultiVal(t1 val1, t2 val2, t3 val3)
        {
            this.mValue1 = val1;
            this.mValue2 = val2;
            this.mValue3 = val3;
        }

        public override bool Equals(object obj)
        {
            return (obj.GetHashCode() == this.GetHashCode());
        }

        public override int GetHashCode()
        {
            return ((this.Value1.GetHashCode() ^ this.Value2.GetHashCode()) ^ this.Value3.GetHashCode());
        }

        public override string ToString()
        {
            return string.Format("1: {0}, 2: {1}, 3: {2}", this.Value1, this.Value2, this.Value3);
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

        public t3 Value3
        {
            get
            {
                return this.mValue3;
            }
            set
            {
                this.mValue3 = value;
            }
        }
    }
}

