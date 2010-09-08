namespace GPG.Multiplayer.Client
{
    using System;
    using System.Text;

    public class PacketInfo
    {
        private byte[] mData;
        private string mDestAddress;
        private int mDestPort;
        private string mPacketType;
        private string mSourceAddress;
        private int mSourcePort;
        private DateTime mTime = DateTime.Now;

        public void SetData(byte[] data)
        {
            this.mData = data;
        }

        public string ByteList
        {
            get
            {
                return BitConverter.ToString(this.mData);
            }
        }

        public string Data
        {
            get
            {
                return Encoding.ASCII.GetString(this.mData);
            }
        }

        public string DestAddress
        {
            get
            {
                return this.mDestAddress;
            }
            set
            {
                this.mDestAddress = value;
            }
        }

        public int DestPort
        {
            get
            {
                return this.mDestPort;
            }
            set
            {
                this.mDestPort = value;
            }
        }

        public string PacketType
        {
            get
            {
                return this.mPacketType;
            }
            set
            {
                this.mPacketType = value;
            }
        }

        public string SourceAddress
        {
            get
            {
                return this.mSourceAddress;
            }
            set
            {
                this.mSourceAddress = value;
            }
        }

        public int SourcePort
        {
            get
            {
                return this.mSourcePort;
            }
            set
            {
                this.mSourcePort = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.mTime;
            }
            set
            {
                this.mTime = value;
            }
        }
    }
}

