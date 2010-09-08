namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.Network;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TeamGame : IEnumerable
    {
        private bool mHasLaunched;
        private string mNonPreferredMap;
        private string mPreferredMap;
        private TeamGameMember mTeamLeader;
        private List<TeamGameMember> mTeamMembers;

        private TeamGame()
        {
            this.mTeamLeader = null;
            this.mPreferredMap = "none";
            this.mNonPreferredMap = "none";
            this.mHasLaunched = false;
            this.mTeamMembers = new List<TeamGameMember>();
        }

        internal TeamGame(TeamGameMember leader)
        {
            this.mTeamLeader = null;
            this.mPreferredMap = "none";
            this.mNonPreferredMap = "none";
            this.mHasLaunched = false;
            this.mTeamMembers = new List<TeamGameMember>();
            leader.Team = this;
            this.mTeamLeader = leader;
            this.TeamMembers.Add(this.TeamLeader);
        }

        public TeamGameMember FindMember(string name)
        {
            foreach (TeamGameMember member in this.TeamMembers)
            {
                if (member.Name.ToLower() == name.ToLower())
                {
                    return member;
                }
            }
            return null;
        }

        public static TeamGame FromDataString(string data)
        {
            TeamGame team = new TeamGame();
            string[] sourceArray = data.Split(new char[] { '|' });
            team.PreferredMap = sourceArray[0];
            team.NonPreferredMap = sourceArray[1];
            string[] destinationArray = new string[sourceArray.Length - 2];
            Array.ConstrainedCopy(sourceArray, 2, destinationArray, 0, destinationArray.Length);
            foreach (string str in destinationArray)
            {
                TeamGameMember item = TeamGameMember.FromDataString(team, str);
                team.TeamMembers.Add(item);
                if (team.TeamLeader == null)
                {
                    team.TeamLeader = item;
                }
            }
            return team;
        }

        public string[] GetAllMemberNames()
        {
            string[] strArray = new string[this.TeamMembers.Count];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = this.TeamMembers[i].Name;
            }
            return strArray;
        }

        public IEnumerator GetEnumerator()
        {
            return this.TeamMembers.GetEnumerator();
        }

        public string[] GetOtherMemberNames()
        {
            try
            {
                string[] strArray = new string[this.TeamMembers.Count - 1];
                int index = 0;
                for (int i = 0; i < this.TeamMembers.Count; i++)
                {
                    if (!this.TeamMembers[i].IsSelf)
                    {
                        strArray[index] = this.TeamMembers[i].Name;
                        index++;
                    }
                }
                return strArray;
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        public TeamGameMember GetSelf()
        {
            foreach (TeamGameMember member in this.TeamMembers)
            {
                if (member.IsSelf)
                {
                    return member;
                }
            }
            return null;
        }

        public string ToDataString()
        {
            string str = string.Format("{0}|{1}|", this.PreferredMap, this.NonPreferredMap);
            foreach (TeamGameMember member in this.TeamMembers)
            {
                str = str + member.ToDataString() + "|";
            }
            return str.TrimEnd(new char[] { '|' });
        }

        public void UpdateMember(TeamGameMember member)
        {
            for (int i = 0; i < this.TeamMembers.Count; i++)
            {
                if (this.TeamMembers[i].Equals(member))
                {
                    this.TeamMembers[i].EndPing();
                    member.BeginPing();
                    if (this.TeamMembers[i].Equals(this.TeamLeader))
                    {
                        this.mTeamLeader = member;
                    }
                    this.TeamMembers[i] = member;
                    return;
                }
            }
            this.TeamMembers.Add(member);
            member.BeginPing();
        }

        public string GameType
        {
            get
            {
                return string.Format("{0}v{0}", this.TeamMembers.Count);
            }
        }

        public bool HasLaunched
        {
            get
            {
                return this.mHasLaunched;
            }
            set
            {
                this.mHasLaunched = value;
            }
        }

        internal static int MAX_TEAM_MEMBERS
        {
            get
            {
                return ConfigSettings.GetInt("MaxTeamSize", 4);
            }
        }

        public string NonPreferredMap
        {
            get
            {
                return this.mNonPreferredMap;
            }
            set
            {
                this.mNonPreferredMap = value;
            }
        }

        public string PreferredMap
        {
            get
            {
                return this.mPreferredMap;
            }
            set
            {
                this.mPreferredMap = value;
            }
        }

        public string TeamIDs
        {
            get
            {
                string str = "";
                foreach (TeamGameMember member in this)
                {
                    str = str + member.ID.ToString() + " ";
                }
                return str.TrimEnd(new char[] { ' ' });
            }
        }

        public TeamGameMember TeamLeader
        {
            get
            {
                return this.mTeamLeader;
            }
            set
            {
                this.mTeamLeader = value;
            }
        }

        public List<TeamGameMember> TeamMembers
        {
            get
            {
                return this.mTeamMembers;
            }
        }

        public class TeamGameMember : IPingMonitor
        {
            private SupcomLookups._Factions mFaction;
            private int mID;
            private bool mIsReady;
            private string mName;
            private int mPingTime;
            private TeamGame mTeam;
            private bool Pinging;
            private const int PingInterval = 0x5dc;
            private int PingSendTime;

            public event EventHandler PingChanged;

            private TeamGameMember()
            {
                this.mIsReady = false;
                this.PingSendTime = 0;
                this.Pinging = false;
                this.mPingTime = 0;
            }

            internal TeamGameMember(TeamGame team)
            {
                this.mIsReady = false;
                this.PingSendTime = 0;
                this.Pinging = false;
                this.mPingTime = 0;
                this.mTeam = team;
            }

            internal TeamGameMember(string name, int id)
            {
                this.mIsReady = false;
                this.PingSendTime = 0;
                this.Pinging = false;
                this.mPingTime = 0;
                this.mName = name;
                this.mID = id;
            }

            internal TeamGameMember(TeamGame team, string name, int id)
            {
                this.mIsReady = false;
                this.PingSendTime = 0;
                this.Pinging = false;
                this.mPingTime = 0;
                this.mTeam = team;
                this.mName = name;
                this.mID = id;
            }

            public void BeginPing()
            {
                if (ConfigSettings.GetBool("AllowPing", false))
                {
                    this.Pinging = true;
                    Program.MainForm.PingResponseReceived += new StringEventHandler(this.MainForm_PingResponseReceived);
                    Program.MainForm.PingPlayer(this.Name);
                    this.PingSendTime = Environment.TickCount;
                }
            }

            public void EndPing()
            {
                this.Pinging = false;
                Program.MainForm.PingResponseReceived -= new StringEventHandler(this.MainForm_PingResponseReceived);
            }

            public override bool Equals(object obj)
            {
                return ((obj is TeamGame.TeamGameMember) && ((obj as TeamGame.TeamGameMember).ID == this.ID));
            }

            public static TeamGame.TeamGameMember FromDataString(TeamGame team, string data)
            {
                TeamGame.TeamGameMember member = new TeamGame.TeamGameMember(team);
                string[] strArray = data.Split(new char[] { ';' });
                member.mName = strArray[0];
                member.mID = int.Parse(strArray[1]);
                member.mFaction = (SupcomLookups._Factions) uint.Parse(strArray[2]);
                member.mIsReady = byte.Parse(strArray[3]) > 0;
                return member;
            }

            public override int GetHashCode()
            {
                return this.Name.ToLower().GetHashCode();
            }

            private void MainForm_PingResponseReceived(string senderName)
            {
                WaitCallback callBack = null;
                if (senderName == this.Name)
                {
                    this.PingTime = Environment.TickCount - this.PingSendTime;
                    if (this.Pinging && (this.PingTime < 0x5dc))
                    {
                        if (callBack == null)
                        {
                            callBack = delegate (object s) {
                                while (this.Pinging && ((Environment.TickCount - this.PingSendTime) < 0x5dc))
                                {
                                    Thread.Sleep(100);
                                }
                                if (this.Pinging)
                                {
                                    Program.MainForm.PingPlayer(this.Name);
                                    this.PingSendTime = Environment.TickCount;
                                }
                            };
                        }
                        ThreadPool.QueueUserWorkItem(callBack);
                    }
                    else if (this.Pinging)
                    {
                        Program.MainForm.PingPlayer(this.Name);
                        this.PingSendTime = Environment.TickCount;
                    }
                }
            }

            public string ToDataString()
            {
                byte num = 0;
                if (this.IsReady)
                {
                    num = 1;
                }
                return string.Format("{0};{1};{2};{3}", new object[] { this.Name, this.ID, (uint) this.Faction, num });
            }

            public override string ToString()
            {
                return this.Name;
            }

            public int BestPing
            {
                get
                {
                    return 600;
                }
            }

            internal SupcomLookups._Factions Faction
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

            public int ID
            {
                get
                {
                    return this.mID;
                }
                set
                {
                    this.mID = value;
                }
            }

            public bool IsReady
            {
                get
                {
                    return this.mIsReady;
                }
                set
                {
                    this.mIsReady = value;
                }
            }

            public bool IsSelf
            {
                get
                {
                    return (this.ID == User.Current.ID);
                }
            }

            public bool IsTeamLeader
            {
                get
                {
                    return this.Equals(this.Team.TeamLeader);
                }
            }

            public string Name
            {
                get
                {
                    return this.mName;
                }
                set
                {
                    this.mName = value;
                }
            }

            public int PingTime
            {
                get
                {
                    return this.mPingTime;
                }
                set
                {
                    this.mPingTime = value;
                    if (this.PingChanged != null)
                    {
                        this.PingChanged(this, EventArgs.Empty);
                    }
                }
            }

            public TeamGame Team
            {
                get
                {
                    return this.mTeam;
                }
                set
                {
                    this.mTeam = value;
                }
            }

            public int WorstPing
            {
                get
                {
                    return (this.BestPing + 0x4b0);
                }
            }
        }
    }
}

