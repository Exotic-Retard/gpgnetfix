namespace GPG.Multiplayer.Quazal
{
    using System;

    internal class NetTransfer
    {
        private int mChunksRecieved;
        private char[] mData;
        private int mLength;
        private int mTotalChunks;

        public NetTransfer(int totalChunks)
        {
            this.mTotalChunks = totalChunks;
            this.mData = new char[0x7d0 * this.TotalChunks];
        }

        public void AddChunk(int chunk, string data)
        {
            this.mChunksRecieved++;
            this.mLength += data.Length;
            Array.ConstrainedCopy(data.ToCharArray(), 0, this.mData, chunk * 0x7d0, data.Length);
        }

        public byte[] GetBin()
        {
            return Convert.FromBase64CharArray(this.Data, 0, this.Length);
        }

        public int ChunksRecieved
        {
            get
            {
                return this.mChunksRecieved;
            }
        }

        public char[] Data
        {
            get
            {
                return this.mData;
            }
        }

        public bool IsComplete
        {
            get
            {
                return (this.ChunksRecieved >= this.TotalChunks);
            }
        }

        public int Length
        {
            get
            {
                return this.mLength;
            }
        }

        public int TotalChunks
        {
            get
            {
                return this.mTotalChunks;
            }
        }
    }
}

