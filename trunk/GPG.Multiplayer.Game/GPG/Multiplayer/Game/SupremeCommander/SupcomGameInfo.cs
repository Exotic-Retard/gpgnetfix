namespace GPG.Multiplayer.Game.SupremeCommander
{
    using GPG;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class SupcomGameInfo
    {
        private string mMap;
        private List<SupcomPlayerInfo> mPlayers = new List<SupcomPlayerInfo>();
        private DateTime mStartTime = DateTime.Now;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MapChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PlayersChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StartTimeChanged;

        public int GetMaxPlayers()
        {
            int @int = 8;
            if (this.IsRegularRankedGame())
            {
                @int = ConfigSettings.GetInt(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " MaxPlayers", 100);
            }
            return @int;
        }

        private bool IsRegularRankedGame()
        {
            return ((ConfigSettings.GetString("Automatch GameIDs", "27") + " ").IndexOf(GPGnetSelectedGame.SelectedGame.GameID.ToString() + " ") >= 0);
        }

        public SupcomPlayerInfo PlayerByName(string playerName)
        {
            foreach (SupcomPlayerInfo info in this.Players)
            {
                if (info.PlayerName.ToUpper() == playerName.ToUpper())
                {
                    return info;
                }
            }
            SupcomPlayerInfo item = new SupcomPlayerInfo {
                PlayerName = playerName
            };
            this.Players.Add(item);
            return item;
        }

        public void RemoveByPlayerID(int playerID)
        {
            SupcomPlayerInfo item = null;
            foreach (SupcomPlayerInfo info2 in this.Players)
            {
                if (info2.PlayerID == playerID)
                {
                    item = info2;
                }
            }
            if (item != null)
            {
                this.Players.Remove(item);
            }
        }

        public void Sort()
        {
            this.Players.Sort(new Comparison<SupcomPlayerInfo>(this.SortCriteria));
        }

        private int SortCriteria(SupcomPlayerInfo info1, SupcomPlayerInfo info2)
        {
            return (info1.Army - info2.Army);
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
                if (this.mMapChanged != null)
                {
                    this.mMapChanged(this, new PropertyChangedEventArgs("Map"));
                }
            }
        }

        public List<SupcomPlayerInfo> Players
        {
            get
            {
                return this.mPlayers;
            }
            set
            {
                this.mPlayers = value;
                if (this.mPlayersChanged != null)
                {
                    this.mPlayersChanged(this, new PropertyChangedEventArgs("Players"));
                }
            }
        }

        public DateTime StartTime
        {
            get
            {
                return this.mStartTime;
            }
            set
            {
                this.mStartTime = value;
                if (this.mStartTimeChanged != null)
                {
                    this.mStartTimeChanged(this, new PropertyChangedEventArgs("StartTime"));
                }
            }
        }
    }
}

