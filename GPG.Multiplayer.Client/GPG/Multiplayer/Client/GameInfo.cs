namespace GPG.Multiplayer.Client
{
    using System;
    using System.Diagnostics;

    public class GameInfo
    {
        private string mDescription = null;
        private string mFilename = null;
        private bool mIsRunning = true;
        private int mProcessID = -1;
        private string mProcessName = null;

        public GameInfo(Process proc)
        {
            this.mProcessName = proc.ProcessName;
            this.mProcessID = proc.Id;
            this.mFilename = proc.StartInfo.FileName;
        }

        public override string ToString()
        {
            if (this.IsRunning)
            {
                return string.Format("Process: {0}- {1}, File: {2}", this.ProcessID, this.ProcessName, this.Filename);
            }
            return base.ToString();
        }

        public string Description
        {
            get
            {
                if (this.mDescription != null)
                {
                    return this.mDescription;
                }
                if (this.ProcessName != null)
                {
                    return this.ProcessName;
                }
                return this.Filename;
            }
        }

        public string Filename
        {
            get
            {
                return this.mFilename;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.mIsRunning;
            }
        }

        public int ProcessID
        {
            get
            {
                return this.mProcessID;
            }
        }

        public string ProcessName
        {
            get
            {
                return this.mProcessName;
            }
        }
    }
}

