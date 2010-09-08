namespace GPG.Network
{
    using System;

    public class Node
    {
        private string mIpAddress;
        private string mName;
        private int mPort;

        public Node(string name)
        {
            this.mName = name;
        }

        public Node(string ipAdress, int port)
        {
            this.mIpAddress = ipAdress;
            this.mPort = port;
        }

        public override bool Equals(object obj)
        {
            return (obj.GetHashCode() == this.GetHashCode());
        }

        public override int GetHashCode()
        {
            if (this.mName == null)
            {
                return (this.IpAddress.GetHashCode() ^ this.Port.GetHashCode());
            }
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            if (this.mName == null)
            {
                return string.Format("{0}:{1}", this.IpAddress, this.Port);
            }
            return this.mName;
        }

        public string IpAddress
        {
            get
            {
                return this.mIpAddress;
            }
            set
            {
                this.mIpAddress = value;
            }
        }

        public string Name
        {
            get
            {
                return this.ToString();
            }
            set
            {
                this.mName = value;
            }
        }

        public int Port
        {
            get
            {
                return this.mPort;
            }
            set
            {
                this.mPort = value;
            }
        }
    }
}

