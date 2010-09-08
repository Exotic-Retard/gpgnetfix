namespace GPGnetCommunicationsLib
{
    using System;

    [Serializable]
    public class Credentials
    {
        private string _connectionURL = "127.0.0.1:0";
        private string mName = "Anonymous";
        private int mPrincipalID = -1;
        private int mUserLevel = -1;

        public override bool Equals(object obj)
        {
            Credentials credentials = obj as Credentials;
            return ((credentials != null) && (credentials.PrincipalID == this.PrincipalID));
        }

        public override string ToString()
        {
            return (this.PrincipalID.ToString() + " " + this.Name + " " + this.UserLevel.ToString());
        }

        public string ConnectionURL
        {
            get
            {
                return this._connectionURL;
            }
            set
            {
                this._connectionURL = value;
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

        public int PrincipalID
        {
            get
            {
                return this.mPrincipalID;
            }
            set
            {
                this.mPrincipalID = value;
            }
        }

        public int UserLevel
        {
            get
            {
                return this.mUserLevel;
            }
            set
            {
                this.mUserLevel = value;
            }
        }
    }
}

