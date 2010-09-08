namespace GPG.Multiplayer.Client.Games
{
    using GPG.Logging;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public abstract class GPGnetGame : IEnumerable
    {
        private bool mCanLaunch;
        private bool mHasLaunched;
        private GPGnetGameParticipant mHost;
        private string mID;
        private List<GPGnetGameParticipant> mMembers;
        private string mNonPreferredMap;
        private GamePositionStates[] mPositionStates;
        private string mPreferredMap;

        protected GPGnetGame()
        {
            this.mHost = null;
            this.mPreferredMap = "none";
            this.mNonPreferredMap = "none";
            this.mHasLaunched = false;
            this.mCanLaunch = false;
            this.mMembers = new List<GPGnetGameParticipant>();
            this.mPositionStates = new GamePositionStates[this.MaxGameParticipants];
            for (int i = 0; i < this.MaxGameParticipants; i++)
            {
                this.mPositionStates[i] = GamePositionStates.Open;
            }
        }

        internal GPGnetGame(GPGnetGameParticipant leader) : this()
        {
            this.mID = Guid.NewGuid().ToString();
            leader.Game = this;
            this.mHost = leader;
            this.Members.Add(this.Host);
        }

        public abstract GPGnetGameParticipant CreateMember();
        public GPGnetGameParticipant FindMember(string name)
        {
            foreach (GPGnetGameParticipant participant in this.Members)
            {
                if (participant.Name.ToLower() == name.ToLower())
                {
                    return participant;
                }
            }
            return null;
        }

        public GPGnetGameParticipant FindMemberAtPosition(int position)
        {
            foreach (GPGnetGameParticipant participant in this.Members)
            {
                if (participant.Position == position)
                {
                    return participant;
                }
            }
            return null;
        }

        public static gameType FromDataString<gameType>(string data) where gameType: GPGnetGame
        {
            return (FromDataString(Activator.CreateInstance<gameType>(), data) as gameType);
        }

        protected virtual void FromDataString(string[] tokens)
        {
            this.mID = tokens[0];
            for (int i = 0; i < this.MaxGameParticipants; i++)
            {
                int num2 = int.Parse(tokens[i + 1]);
                this.PositionStates[i] = (GamePositionStates) num2;
            }
            string[] destinationArray = new string[tokens.Length - (1 + this.MaxGameParticipants)];
            Array.ConstrainedCopy(tokens, 1 + this.MaxGameParticipants, destinationArray, 0, destinationArray.Length);
            foreach (string str in destinationArray)
            {
                GPGnetGameParticipant item = GPGnetGameParticipant.FromDataString(this, str);
                this.Members.Add(item);
                if (this.Host == null)
                {
                    this.Host = item;
                }
            }
        }

        public static GPGnetGame FromDataString(GPGnetGame game, string data)
        {
            string[] tokens = data.Split(new char[] { '|' });
            game.FromDataString(tokens);
            return game;
        }

        public static GPGnetGame FromDataString(Type gameType, string data)
        {
            return FromDataString(Activator.CreateInstance(gameType) as GPGnetGame, data);
        }

        public string[] GetAllMemberNames()
        {
            string[] strArray = new string[this.Members.Count];
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = this.Members[i].Name;
            }
            return strArray;
        }

        public IEnumerator GetEnumerator()
        {
            return this.Members.GetEnumerator();
        }

        public string[] GetOtherMemberNames()
        {
            try
            {
                string[] strArray = new string[this.Members.Count - 1];
                int index = 0;
                for (int i = 0; i < this.Members.Count; i++)
                {
                    if (!this.Members[i].IsSelf)
                    {
                        strArray[index] = this.Members[i].Name;
                        index++;
                    }
                }
                return strArray;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return new string[0];
            }
        }

        public GPGnetGameParticipant GetSelf()
        {
            foreach (GPGnetGameParticipant participant in this.Members)
            {
                if (participant.IsSelf)
                {
                    return participant;
                }
            }
            return null;
        }

        public int NextAvailablePosition()
        {
            for (int i = 0; i < this.MaxGameParticipants; i++)
            {
                if ((this.FindMemberAtPosition(i) == null) && (this.PositionStates[i] == GamePositionStates.Open))
                {
                    return i;
                }
            }
            return -1;
        }

        public virtual string ToDataString()
        {
            string str = string.Format("{0}|", this.ID);
            foreach (GamePositionStates states in this.PositionStates)
            {
                str = str + ((int) states).ToString() + "|";
            }
            foreach (GPGnetGameParticipant participant in this.Members)
            {
                str = str + participant.ToDataString() + "|";
            }
            return str.TrimEnd(new char[] { '|' });
        }

        public void UpdateMember(GPGnetGameParticipant member)
        {
            for (int i = 0; i < this.Members.Count; i++)
            {
                if (this.Members[i].Equals(member))
                {
                    this.Members[i].EndPing();
                    member.BeginPing();
                    if (this.Members[i].Equals(this.Host))
                    {
                        this.mHost = member;
                    }
                    this.Members[i] = member;
                    return;
                }
            }
            this.Members.Add(member);
            member.BeginPing();
        }

        public int AvailablePositions
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.MaxGameParticipants; i++)
                {
                    if ((this.FindMemberAtPosition(i) == null) && (this.PositionStates[i] == GamePositionStates.Open))
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public bool CanLaunch
        {
            get
            {
                return this.mCanLaunch;
            }
            set
            {
                this.mCanLaunch = value;
            }
        }

        public string GameType
        {
            get
            {
                return string.Format("{0}v{0}", this.Members.Count);
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

        public GPGnetGameParticipant Host
        {
            get
            {
                return this.mHost;
            }
            set
            {
                this.mHost = value;
            }
        }

        public string ID
        {
            get
            {
                return this.mID;
            }
        }

        public abstract int MaxGameParticipants { get; }

        public string MemberIDs
        {
            get
            {
                string str = "";
                foreach (GPGnetGameParticipant participant in this)
                {
                    str = str + participant.ID.ToString() + " ";
                }
                return str.TrimEnd(new char[] { ' ' });
            }
        }

        public List<GPGnetGameParticipant> Members
        {
            get
            {
                return this.mMembers;
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

        public GamePositionStates[] PositionStates
        {
            get
            {
                return this.mPositionStates;
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
    }
}

