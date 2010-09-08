namespace GPGnetCommunicationsLib.SupcomInfo
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class GameReporting
    {
        private double mDuration;
        private int mGameID;
        private string mGameType;
        private int mGPGnetGameID;
        private string mMap;
        private List<ReportSucpomPlayerInfo> mPlayerInfo = new List<ReportSucpomPlayerInfo>();

        public double Duration
        {
            get
            {
                return this.mDuration;
            }
            set
            {
                this.mDuration = value;
            }
        }

        public int GameID
        {
            get
            {
                return this.mGameID;
            }
            set
            {
                this.mGameID = value;
            }
        }

        public string GameType
        {
            get
            {
                return this.mGameType;
            }
            set
            {
                this.mGameType = value;
            }
        }

        public int GPGnetGameID
        {
            get
            {
                return this.mGPGnetGameID;
            }
            set
            {
                this.mGPGnetGameID = value;
            }
        }

        public string Map
        {
            get
            {
                return this.mMap;
            }
            set
            {
                this.mMap = value;
            }
        }

        public List<ReportSucpomPlayerInfo> PlayerInfo
        {
            get
            {
                return this.mPlayerInfo;
            }
            set
            {
                this.mPlayerInfo = value;
            }
        }
    }
}

