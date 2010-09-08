namespace GPGnetCommunicationsLib.SupcomInfo
{
    using System;

    [Serializable]
    public class ReportSupcomUnitInfo
    {
        private int mBuilt;
        private double mDamageDone;
        private double mDamageReceived;
        private int mKilled;
        private int mLost;
        private string mUnitID;

        public int Built
        {
            get
            {
                return this.mBuilt;
            }
            set
            {
                this.mBuilt = value;
            }
        }

        public double DamageDone
        {
            get
            {
                return this.mDamageDone;
            }
            set
            {
                this.mDamageDone = value;
            }
        }

        public double DamageReceived
        {
            get
            {
                return this.mDamageReceived;
            }
            set
            {
                this.mDamageReceived = value;
            }
        }

        public int Killed
        {
            get
            {
                return this.mKilled;
            }
            set
            {
                this.mKilled = value;
            }
        }

        public int Lost
        {
            get
            {
                return this.mLost;
            }
            set
            {
                this.mLost = value;
            }
        }

        public string UnitID
        {
            get
            {
                return this.mUnitID;
            }
            set
            {
                this.mUnitID = value;
            }
        }
    }
}

