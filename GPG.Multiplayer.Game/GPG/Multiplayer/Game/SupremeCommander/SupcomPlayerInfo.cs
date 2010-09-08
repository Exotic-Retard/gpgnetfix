namespace GPG.Multiplayer.Game.SupremeCommander
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class SupcomPlayerInfo
    {
        private int mArmy = -1;
        private int mBuilt;
        private string mColor;
        private double mDamageDone;
        private double mDamageReceived;
        private double mEnergyConsumed;
        private double mEnergyProduced;
        private string mFaction;
        private int mKills;
        private int mLost;
        private double mMassConsumed;
        private double mMassProduced;
        private int mPlayerID;
        private string mPlayerName;
        private int mStartSpot;
        private string mStatus = "Playing";
        private string mTeam;
        public List<SupcomUnitInfo> UnitInfo = new List<SupcomUnitInfo>();

        [field: NonSerialized]
        public event PropertyChangedEventHandler ArmyChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler BuiltChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler ColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DamageDoneChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DamageReceivedChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EnergyConsumedChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EnergyProducedChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FactionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler KillsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler LostChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MassConsumedChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MassProducedChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PlayerIDChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PlayerNameChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StartSpotChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler StatusChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler TeamChanged;

        public int Army
        {
            get
            {
                return this.mArmy;
            }
            set
            {
                this.mArmy = value;
                if (this.ArmyChanged != null)
                {
                    this.ArmyChanged(this, new PropertyChangedEventArgs("Army"));
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>Built"), Description("<LOC>")]
        public int Built
        {
            get
            {
                return this.mBuilt;
            }
            set
            {
                this.mBuilt = value;
                if (this.BuiltChanged != null)
                {
                    this.BuiltChanged(this, new PropertyChangedEventArgs("Built"));
                }
            }
        }

        public string Color
        {
            get
            {
                return this.mColor;
            }
            set
            {
                this.mColor = value;
                if (this.ColorChanged != null)
                {
                    this.ColorChanged(this, new PropertyChangedEventArgs("Color"));
                }
            }
        }

        [Category("<LOC>Misc"), DisplayName("<LOC>DamageDone"), Description("<LOC>")]
        public double DamageDone
        {
            get
            {
                return this.mDamageDone;
            }
            set
            {
                this.mDamageDone = value;
                if (this.DamageDoneChanged != null)
                {
                    this.DamageDoneChanged(this, new PropertyChangedEventArgs("DamageDone"));
                }
            }
        }

        [DisplayName("<LOC>DamageReceived"), Description("<LOC>"), Category("<LOC>Misc")]
        public double DamageReceived
        {
            get
            {
                return this.mDamageReceived;
            }
            set
            {
                this.mDamageReceived = value;
                if (this.DamageReceivedChanged != null)
                {
                    this.DamageReceivedChanged(this, new PropertyChangedEventArgs("DamageReceived"));
                }
            }
        }

        [Description("<LOC>"), DisplayName("<LOC>EnergyConsumed"), Category("<LOC>Misc")]
        public double EnergyConsumed
        {
            get
            {
                return this.mEnergyConsumed;
            }
            set
            {
                this.mEnergyConsumed = value;
                if (this.EnergyConsumedChanged != null)
                {
                    this.EnergyConsumedChanged(this, new PropertyChangedEventArgs("EnergyConsumed"));
                }
            }
        }

        [DisplayName("<LOC>EnergyProduced"), Category("<LOC>Misc"), Description("<LOC>")]
        public double EnergyProduced
        {
            get
            {
                return this.mEnergyProduced;
            }
            set
            {
                this.mEnergyProduced = value;
                if (this.EnergyProducedChanged != null)
                {
                    this.EnergyProducedChanged(this, new PropertyChangedEventArgs("EnergyProduced"));
                }
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
                if (this.FactionChanged != null)
                {
                    this.FactionChanged(this, new PropertyChangedEventArgs("Faction"));
                }
            }
        }

        [Description("<LOC>"), DisplayName("<LOC>Kills"), Category("<LOC>Misc")]
        public int Kills
        {
            get
            {
                return this.mKills;
            }
            set
            {
                this.mKills = value;
                if (this.KillsChanged != null)
                {
                    this.KillsChanged(this, new PropertyChangedEventArgs("Kills"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Misc"), DisplayName("<LOC>Lost")]
        public int Lost
        {
            get
            {
                return this.mLost;
            }
            set
            {
                this.mLost = value;
                if (this.LostChanged != null)
                {
                    this.LostChanged(this, new PropertyChangedEventArgs("Lost"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Misc"), DisplayName("<LOC>MassConsumed")]
        public double MassConsumed
        {
            get
            {
                return this.mMassConsumed;
            }
            set
            {
                this.mMassConsumed = value;
                if (this.MassConsumedChanged != null)
                {
                    this.MassConsumedChanged(this, new PropertyChangedEventArgs("MassConsumed"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Misc"), DisplayName("<LOC>MassProduced")]
        public double MassProduced
        {
            get
            {
                return this.mMassProduced;
            }
            set
            {
                this.mMassProduced = value;
                if (this.MassProducedChanged != null)
                {
                    this.MassProducedChanged(this, new PropertyChangedEventArgs("MassProduced"));
                }
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
            set
            {
                this.mPlayerID = value;
                if (this.PlayerIDChanged != null)
                {
                    this.PlayerIDChanged(this, new PropertyChangedEventArgs("PlayerID"));
                }
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
                if (this.PlayerNameChanged != null)
                {
                    this.PlayerNameChanged(this, new PropertyChangedEventArgs("PlayerName"));
                }
            }
        }

        public int StartSpot
        {
            get
            {
                return this.mStartSpot;
            }
            set
            {
                this.mStartSpot = value;
                if (this.StartSpotChanged != null)
                {
                    this.StartSpotChanged(this, new PropertyChangedEventArgs("StartSpot"));
                }
            }
        }

        public string Status
        {
            get
            {
                return this.mStatus;
            }
            set
            {
                this.mStatus = value;
                if (this.StatusChanged != null)
                {
                    this.StatusChanged(this, new PropertyChangedEventArgs("Status"));
                }
            }
        }

        public string Team
        {
            get
            {
                return this.mTeam;
            }
            set
            {
                this.mTeam = value;
                if (this.TeamChanged != null)
                {
                    this.TeamChanged(this, new PropertyChangedEventArgs("Team"));
                }
            }
        }
    }
}

