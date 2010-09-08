namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential)]
    public struct SNatMessage
    {
        [MarshalAs(UnmanagedType.LPStr)]
        private string mAddress;
        private int mPort;
        private int mMessageSize;
        private byte[] mMessage;
        public string Address
        {
            get
            {
                return this.mAddress;
            }
            set
            {
                this.mAddress = value;
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
        public int MessageSize
        {
            get
            {
                return this.mMessageSize;
            }
            set
            {
                this.mMessageSize = value;
            }
        }
        public byte[] Message
        {
            get
            {
                return this.mMessage;
            }
            set
            {
                this.mMessage = value;
            }
        }
        public string ByteData()
        {
            string str = "";
            foreach (byte num in this.Message)
            {
                str = str + num.ToString() + " ";
            }
            return (str + Encoding.ASCII.GetString(this.Message));
        }

        public bool IsNatMessage()
        {
            return ((this.Message.Length > 0) && (this.Message[0] == MatchmakingHelper.NatHeader));
        }
    }
}

