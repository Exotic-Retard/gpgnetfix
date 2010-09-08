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
                if (this.mArmyChanged != null)
                {
                    this.mArmyChanged(this, new PropertyChangedEventArgs("Army"));
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
                if (this.mBuiltChanged != null)
                {
                    this.mBuiltChanged(this, new PropertyChangedEventArgs("Built"));
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
                if (this.mColorChanged != null)
                {
                    this.mColorChanged(this, new PropertyChangedEventArgs("Color"));
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
                if (this.mDamageDoneChanged != null)
                {
                    this.mDamageDoneChanged(this, new PropertyChangedEventArgs("DamageDone"));
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
                if (this.mDamageReceivedChanged != null)
                {
                    this.mDamageReceivedChanged(this, new PropertyChangedEventArgs("DamageReceived"));
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
                if (this.mEnergyConsumedChanged != null)
                {
                    this.mEnergyConsumedChanged(this, new PropertyChangedEventArgs("EnergyConsumed"));
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
                if (this.mEnergyProducedChanged != null)
                {
                    this.mEnergyProducedChanged(this, new PropertyChangedEventArgs("EnergyProduced"));
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
                if (this.mFactionChanged != null)
                {
                    this.mFactionChanged(this, new PropertyChangedEventArgs("Faction"));
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
                if (this.mKillsChanged != null)
                {
                    this.mKillsChanged(this, new PropertyChangedEventArgs("Kills"));
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
                if (this.mLostChanged != null)
                {
                    this.mLostChanged(this, new PropertyChangedEventArgs("Lost"));
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
                if (this.mMassConsumedChanged != null)
                {
                    this.mMassConsumedChanged(this, new PropertyChangedEventArgs("MassConsumed"));
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
                if (this.mMassProducedChanged != null)
                {
                    this.mMassProducedChanged(this, new PropertyChangedEventArgs("MassProduced"));
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
                if (this.mPlayerIDChanged != null)
                {
                    this.mPlayerIDChanged(this, new PropertyChangedEventArgs("PlayerID"));
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
                if (this.mPlayerNameChanged != null)
                {
                    this.mPlayerNameChanged(this, new PropertyChangedEventArgs("PlayerName"));
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
                if (this.mStartSpotChanged != null)
                {
                    this.mStartSpotChanged(this, new PropertyChangedEventArgs("StartSpot"));
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
                if (this.mStatusChanged != null)
                {
                    this.mStatusChanged(this, new PropertyChangedEventArgs("Status"));
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
                if (this.mTeamChanged != null)
                {
                    this.mTeamChanged(this, new PropertyChangedEventArgs("Team"));
                }
            }
        }
    }
}

