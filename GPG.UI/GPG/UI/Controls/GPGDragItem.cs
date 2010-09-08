namespace GPG.UI.Controls
{
    using GPG.Multiplayer.Quazal;
    using GPG.UI;
    using System;
    using System.Drawing;

    public class GPGDragItem : IComparable
    {
        private int mDraws;
        private string mFaction = "Random";
        private int? mHonorPoints = null;
        private Image mIcon;
        private bool mInGame;
        private bool mIsLocked;
        private bool mIsMatched;
        private string mLabel = "";
        private int mLosses;
        private string mPingStatus = "Offline";
        private string mPlayerReport = "No Report";
        private int mSeed;
        private object mValue;
        private int mWins;
        private int mWon = -1;

        public int CompareTo(object obj)
        {
            GPGDragItem item = obj as GPGDragItem;
            if (item == null)
            {
                return -3;
            }
            if (item != this)
            {
                if (this.FractionalWins > item.FractionalWins)
                {
                    return 2;
                }
                if (this.FractionalWins < item.FractionalWins)
                {
                    return -2;
                }
                if (this.Seed > item.Seed)
                {
                    return -1;
                }
                if (this.Seed < item.Seed)
                {
                    return 1;
                }
            }
            return 0;
        }

        public void RandomizeRecord()
        {
            Random random = new Random();
            this.mWins = random.Next(0, 5);
            this.mDraws = random.Next(0, 5 - this.mWins);
            this.mLosses = (5 - this.mWins) - this.mDraws;
        }

        public int Draws
        {
            get
            {
                return this.mDraws;
            }
            set
            {
                this.mDraws = value;
            }
        }

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

        public float FractionalWins
        {
            get
            {
                return (this.Wins + (((float) this.Draws) / 2f));
            }
        }

        public int HonorPoints
        {
            get
            {
                if (!this.mHonorPoints.HasValue)
                {
                    this.mHonorPoints = new int?(new QuazalQuery("GetHonorPointsByPlayerName", new object[] { this.PlayerName }).GetInt());
                }
                return this.mHonorPoints.Value;
            }
            set
            {
                this.mHonorPoints = new int?(value);
                if (this.mHonorPoints > 10)
                {
                    this.mHonorPoints = 10;
                }
                if (this.mHonorPoints < 0)
                {
                    this.mHonorPoints = 0;
                }
            }
        }

        public Image Icon
        {
            get
            {
                if (this.PingStatus.ToUpper() == "OFFLINE")
                {
                    return GPGResources.icon_conflict_sm;
                }
                return this.mIcon;
            }
            set
            {
                this.mIcon = value;
            }
        }

        public bool InGame
        {
            get
            {
                return this.mInGame;
            }
            set
            {
                this.mInGame = value;
            }
        }

        public bool IsLocked
        {
            get
            {
                return this.mIsLocked;
            }
            set
            {
                this.mIsLocked = value;
            }
        }

        public bool IsMatched
        {
            get
            {
                return this.mIsMatched;
            }
            set
            {
                this.mIsMatched = value;
            }
        }

        public string Label
        {
            get
            {
                if ((this.mLabel != null) && (this.mLabel != ""))
                {
                    return ("#" + this.mSeed.ToString() + " " + this.mLabel + " " + this.PingStatus + " (" + this.Wins.ToString() + "-" + this.Losses.ToString() + "-" + this.Draws.ToString() + ") " + this.PlayerReport);
                }
                if (this.mValue != null)
                {
                    return this.mValue.ToString();
                }
                return "";
            }
            set
            {
                this.mLabel = value;
            }
        }

        public string LabelNoReport
        {
            get
            {
                if ((this.mLabel != null) && (this.mLabel != ""))
                {
                    return ("#" + this.mSeed.ToString() + " " + this.mLabel + " (" + this.Wins.ToString() + "-" + this.Losses.ToString() + "-" + this.Draws.ToString() + ")");
                }
                if (this.mValue != null)
                {
                    return this.mValue.ToString();
                }
                return "";
            }
            set
            {
                this.mLabel = value;
            }
        }

        public int Losses
        {
            get
            {
                return this.mLosses;
            }
            set
            {
                this.mLosses = value;
            }
        }

        public string PingStatus
        {
            get
            {
                if (this.mPingStatus.ToUpper() == "UNAVAILABLE")
                {
                    return "Chat";
                }
                return this.mPingStatus;
            }
            set
            {
                this.mPingStatus = value;
            }
        }

        public string PlayerName
        {
            get
            {
                if ((this.mLabel != null) && (this.mLabel != ""))
                {
                    return this.mLabel;
                }
                if (this.mValue != null)
                {
                    return this.mValue.ToString();
                }
                return "";
            }
        }

        public string PlayerReport
        {
            get
            {
                return this.mPlayerReport;
            }
            set
            {
                this.mPlayerReport = value;
            }
        }

        public int Seed
        {
            get
            {
                return this.mSeed;
            }
            set
            {
                this.mSeed = value;
            }
        }

        public object Value
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

        public int Wins
        {
            get
            {
                return this.mWins;
            }
            set
            {
                this.mWins = value;
            }
        }

        public int Won
        {
            get
            {
                return this.mWon;
            }
            set
            {
                this.mWon = value;
            }
        }
    }
}

