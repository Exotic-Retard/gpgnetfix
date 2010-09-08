namespace GPG.Multiplayer.Client.Volunteering
{
    using GPG;
    using GPG.DataAccess;
    using System;

    public class Volunteer : MappedObject
    {
        [FieldMap("available_days")]
        private string mAvailableDays;
        [FieldMap("available_times")]
        private string mAvailableTimes;
        [FieldMap("effort_description")]
        private string mEffortDescription;
        [FieldMap("effort_id")]
        private int mEffortID;
        [FieldMap("email")]
        private string mEmail;
        [FieldMap("volunteer_id")]
        private int mID;
        [FieldMap("principal_id")]
        private int mPlayerID;
        [FieldMap("name")]
        private string mPlayerName;
        [FieldMap("volunteer_date")]
        private DateTime mVolunteerDate;

        public Volunteer(DataRecord record) : base(record)
        {
        }

        public string AvailableDays
        {
            get
            {
                if (this.mAvailableDays == null)
                {
                    return Loc.Get("<LOC>Any");
                }
                string[] strArray = this.mAvailableDays.Split(new char[] { ',' });
                int[] numArray = new int[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    numArray[i] = int.Parse(strArray[i]);
                }
                string str = "";
                foreach (int num2 in numArray)
                {
                    switch (num2)
                    {
                        case 1:
                            str = str + Loc.Get("<LOC>Monday") + ", ";
                            break;

                        case 2:
                            str = str + Loc.Get("<LOC>Tuesday") + ", ";
                            break;

                        case 3:
                            str = str + Loc.Get("<LOC>Wednesday") + ", ";
                            break;

                        case 4:
                            str = str + Loc.Get("<LOC>Thursday") + ", ";
                            break;

                        case 5:
                            str = str + Loc.Get("<LOC>Friday") + ", ";
                            break;

                        case 6:
                            str = str + Loc.Get("<LOC>Saturday") + ", ";
                            break;

                        case 7:
                            str = str + Loc.Get("<LOC>Sunday") + ", ";
                            break;
                    }
                }
                return str.TrimEnd(", ".ToCharArray());
            }
        }

        public string AvailableTimes
        {
            get
            {
                if (this.mAvailableTimes == null)
                {
                    return Loc.Get("<LOC>Any");
                }
                string[] strArray = this.mAvailableTimes.Split(new char[] { ',' });
                int[] numArray = new int[strArray.Length];
                for (int i = 0; i < strArray.Length; i++)
                {
                    numArray[i] = int.Parse(strArray[i]);
                }
                string str = "";
                foreach (int num2 in numArray)
                {
                    switch (num2)
                    {
                        case 1:
                            str = str + Loc.Get("<LOC>Mornings") + ", ";
                            break;

                        case 2:
                            str = str + Loc.Get("<LOC>Afternoons") + ", ";
                            break;

                        case 3:
                            str = str + Loc.Get("<LOC>Evenings") + ", ";
                            break;
                    }
                }
                return str.TrimEnd(", ".ToCharArray());
            }
        }

        public string EffortDescription
        {
            get
            {
                return this.mEffortDescription;
            }
        }

        public int EffortID
        {
            get
            {
                return this.mEffortID;
            }
        }

        public string Email
        {
            get
            {
                return this.mEmail;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
        }

        public DateTime VolunteerDate
        {
            get
            {
                return this.mVolunteerDate;
            }
        }
    }
}

