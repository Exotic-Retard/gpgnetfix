namespace GPG.Multiplayer.Client.Vaulting.Applications
{
    using System;

    [Serializable]
    public class FTPInfo
    {
        private string mDir;
        private string mFTPPass;
        private string mFTPUser;
        private string mServer;
        private bool mUploaded;
        private string mURL;

        public string Dir
        {
            get
            {
                return this.mDir;
            }
            set
            {
                this.mDir = value;
            }
        }

        public string FTPUser
        {
            get
            {
                return this.mFTPUser;
            }
            set
            {
                this.mFTPUser = value;
            }
        }

        public string Pass
        {
            get
            {
                return this.mFTPPass;
            }
            set
            {
                this.mFTPPass = value;
            }
        }

        public string Server
        {
            get
            {
                return this.mServer;
            }
            set
            {
                this.mServer = value;
            }
        }

        public bool Uploaded
        {
            get
            {
                return this.mUploaded;
            }
            set
            {
                this.mUploaded = value;
            }
        }

        public string URL
        {
            get
            {
                return this.mURL;
            }
            set
            {
                this.mURL = value;
            }
        }
    }
}

