namespace GPGnetCommunicationsLib.SupcomInfo
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ReportSucpomPlayerInfo
    {
        private string mFaction;
        private string mPlayerName;
        private string mResult;
        private string mTeamName;
        private List<ReportSupcomUnitInfo> mUnits = new List<ReportSupcomUnitInfo>();

        public string Faction
        {
            get
            {
                return this.mFaction;
            }
            set
            {
                this.mFaction = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
            set
            {
                this.mPlayerName = value;
            }
        }

        public string Result
        {
            get
            {
                return this.mResult;
            }
            set
            {
                this.mResult = value;
            }
        }

        public string TeamName
        {
            get
            {
                return this.mTeamName;
            }
            set
            {
                this.mTeamName = value;
            }
        }

        public List<ReportSupcomUnitInfo> Units
        {
            get
            {
                return this.mUnits;
            }
            set
            {
                this.mUnits = value;
            }
        }
    }
}

