namespace GPG.Multiplayer.Client.Games
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.Network;
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class GPGnetGameParticipant : IPingMonitor
    {
        private GPGnetGame mGame;
        private int mID;
        private bool mIsReady;
        private string mName;
        private int mPingTime;
        private int mPosition;
        private bool Pinging;
        private const int PingInterval = 0x5dc;
        private int PingSendTime;

        public event EventHandler PingChanged;

        private GPGnetGameParticipant()
        {
            this.mIsReady = false;
            this.mPosition = -1;
            this.PingSendTime = 0;
            this.Pinging = false;
            this.mPingTime = 0;
        }

        internal GPGnetGameParticipant(GPGnetGame game)
        {
            this.mIsReady = false;
            this.mPosition = -1;
            this.PingSendTime = 0;
            this.Pinging = false;
            this.mPingTime = 0;
            this.mGame = game;
        }

        internal GPGnetGameParticipant(string name, int id)
        {
            this.mIsReady = false;
            this.mPosition = -1;
            this.PingSendTime = 0;
            this.Pinging = false;
            this.mPingTime = 0;
            this.mName = name;
            this.mID = id;
        }

        internal GPGnetGameParticipant(GPGnetGame game, string name, int id)
        {
            this.mIsReady = false;
            this.mPosition = -1;
            this.PingSendTime = 0;
            this.Pinging = false;
            this.mPingTime = 0;
            this.mGame = game;
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
            return ((obj is GPGnetGameParticipant) && ((obj as GPGnetGameParticipant).ID == this.ID));
        }

        protected virtual void FromDataString(string[] tokens)
        {
            this.mName = tokens[0];
            this.mID = int.Parse(tokens[1]);
            this.mIsReady = byte.Parse(tokens[2]) > 0;
            int.TryParse(tokens[3], out this.mPosition);
        }

        public static GPGnetGameParticipant FromDataString(GPGnetGame game, string data)
        {
            GPGnetGameParticipant participant = game.CreateMember();
            string[] tokens = data.Split(new char[] { ';' });
            participant.FromDataString(tokens);
            return participant;
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

        public virtual void MergeParticipants(GPGnetGameParticipant participant)
        {
            this.mName = participant.Name;
            this.mID = participant.ID;
            this.mIsReady = participant.IsReady;
            this.mPosition = participant.Position;
        }

        public virtual string ToDataString()
        {
            byte num = 0;
            if (this.IsReady)
            {
                num = 1;
            }
            return string.Format("{0};{1};{2};{3}", new object[] { this.Name, this.ID, num, this.Position });
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

        public GPGnetGame Game
        {
            get
            {
                return this.mGame;
            }
            set
            {
                this.mGame = value;
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

        public bool IsHost
        {
            get
            {
                return this.Equals(this.Game.Host);
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

        public int Position
        {
            get
            {
                return this.mPosition;
            }
            set
            {
                this.mPosition = value;
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

