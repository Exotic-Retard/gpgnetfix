namespace GPG.Multiplayer.Client.Volunteering
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Runtime.CompilerServices;

    public class VolunteerEffort : MappedObject
    {
        [FieldMap("begin_date")]
        private DateTime mBeginDate;
        [FieldMap("description")]
        private string mDescription;
        [FieldMap("desired_days")]
        private string mDesiredDays;
        [FieldMap("desired_times")]
        private string mDesiredTimes;
        [FieldMap("detail_kb")]
        private int mDetailKB;
        [FieldMap("end_date")]
        private DateTime mEndDate;
        [FieldMap("has_volunteered")]
        private bool mHasVolunteered;
        [FieldMap("head_of_effort")]
        private string mHeadOfEffort;
        [FieldMap("volunteer_effort_id")]
        private int mID;
        [FieldMap("is_active")]
        private bool mIsActive;
        [FieldMap("legal_kb")]
        private int mLegalKB;
        [FieldMap("min_age")]
        private int mMinimumAge;
        [FieldMap("requires_days")]
        private bool mRequiresDays;
        [FieldMap("requires_times")]
        private bool mRequiresTimes;
        [FieldMap("time_zone")]
        private string mTimeZone;

        internal event EventHandler Volunteered;

        public VolunteerEffort(DataRecord record) : base(record)
        {
        }

        public static bool HasVolunteeredForEffort(string effortName)
        {
            return new QuazalQuery("HasVolunteeredForEffort", new object[] { effortName }).GetBool();
        }

        internal void OnVolunteered()
        {
            if (this.Volunteered != null)
            {
                this.Volunteered(this, EventArgs.Empty);
            }
        }

        public DateTime BeginDate
        {
            get
            {
                return this.mBeginDate;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public int[] DesiredDays
        {
            get
            {
                if (this.mDesiredDays == null)
                {
                    return new int[0];
                }
                string[] strArray = this.mDesiredDays.Split(new char[] { ',' });
                int[] numArray = new int[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    numArray[i] = int.Parse(strArray[i]);
                }
                return numArray;
            }
        }

        public int[] DesiredTimes
        {
            get
            {
                if (this.mDesiredTimes == null)
                {
                    return new int[0];
                }
                string[] strArray = this.mDesiredTimes.Split(new char[] { ',' });
                int[] numArray = new int[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    numArray[i] = int.Parse(strArray[i]);
                }
                return numArray;
            }
        }

        public int DetailKB
        {
            get
            {
                return this.mDetailKB;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.mEndDate;
            }
        }

        public bool HasVolunteered
        {
            get
            {
                return this.mHasVolunteered;
            }
        }

        public string HeadOfEffort
        {
            get
            {
                return this.mHeadOfEffort;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.mIsActive;
            }
        }

        public int LegalKB
        {
            get
            {
                return this.mLegalKB;
            }
        }

        public int MinimumAge
        {
            get
            {
                return this.mMinimumAge;
            }
        }

        public bool RequiresDays
        {
            get
            {
                return this.mRequiresDays;
            }
        }

        public bool RequiresTimes
        {
            get
            {
                return this.mRequiresTimes;
            }
        }

        public string TimeZone
        {
            get
            {
                return this.mTimeZone;
            }
        }
    }
}

