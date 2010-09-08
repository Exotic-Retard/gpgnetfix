namespace GPG.Multiplayer.Client.map
{
    using System;
    using System.ComponentModel;

    public class TickerItem
    {
        private DateTime mLogTime = DateTime.Now;
        private string mLoseFaction = null;
        private string mLoser = null;
        private string mWinFaction = null;
        private string mWinner = null;
        private string mWitty = null;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LogTimeChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LoseFactionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LoserChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler WinFactionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler WinnerChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler WittyChanged;

        [Description("<LOC>"), DisplayName("<LOC>LogTime"), Category("<LOC>Misc")]
        public DateTime LogTime
        {
            get
            {
                return this.mLogTime;
            }
            set
            {
                this.mLogTime = value;
                if (this.mLogTimeChanged != null)
                {
                    this.mLogTimeChanged(this, new PropertyChangedEventArgs("LogTime"));
                }
            }
        }

        [DisplayName("<LOC>LoseFaction"), Description("<LOC>"), Category("<LOC>Misc")]
        public string LoseFaction
        {
            get
            {
                return this.mLoseFaction;
            }
            set
            {
                this.mLoseFaction = value;
                if (this.mLoseFactionChanged != null)
                {
                    this.mLoseFactionChanged(this, new PropertyChangedEventArgs("LoseFaction"));
                }
            }
        }

        [Description("<LOC>"), DisplayName("<LOC>Loser"), Category("<LOC>Misc")]
        public string Loser
        {
            get
            {
                return this.mLoser;
            }
            set
            {
                this.mLoser = value;
                if (this.mLoserChanged != null)
                {
                    this.mLoserChanged(this, new PropertyChangedEventArgs("Loser"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Misc"), DisplayName("<LOC>WinFaction")]
        public string WinFaction
        {
            get
            {
                return this.mWinFaction;
            }
            set
            {
                this.mWinFaction = value;
                if (this.mWinFactionChanged != null)
                {
                    this.mWinFactionChanged(this, new PropertyChangedEventArgs("WinFaction"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>"), DisplayName("<LOC>Winner")]
        public string Winner
        {
            get
            {
                return this.mWinner;
            }
            set
            {
                this.mWinner = value;
                if (this.mWinnerChanged != null)
                {
                    this.mWinnerChanged(this, new PropertyChangedEventArgs("Winner"));
                }
            }
        }

        [Description("<LOC>"), DisplayName("<LOC>Witty"), Category("<LOC>Misc")]
        public string Witty
        {
            get
            {
                return this.mWitty;
            }
            set
            {
                this.mWitty = value;
                if (this.mWittyChanged != null)
                {
                    this.mWittyChanged(this, new PropertyChangedEventArgs("Witty"));
                }
            }
        }
    }
}

