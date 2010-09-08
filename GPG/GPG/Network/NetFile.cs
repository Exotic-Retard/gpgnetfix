namespace GPG.Network
{
    using GPG.Logging;
    using System;
    using System.IO;

    [Serializable]
    public class NetFile : INetData
    {
        private byte[] mData;
        private string mFilePath;

        public NetFile(string path)
        {
            this.mFilePath = path;
            if (File.Exists(path))
            {
                this.mData = File.ReadAllBytes(path);
            }
            else
            {
                ErrorLog.WriteLine("File does not exist: {0}", new object[] { path });
            }
        }

        public virtual void OnRecieve(Node sender)
        {
            if (File.Exists(this.DefaultPath))
            {
                ErrorLog.WriteLine("File already exists: {0}. To save NetFile it must first be moved.", new object[0]);
            }
            else
            {
                File.WriteAllBytes(this.DefaultPath, this.Data);
            }
        }

        public byte[] Data
        {
            get
            {
                return this.mData;
            }
        }

        public virtual string DefaultPath
        {
            get
            {
                return (AppDomain.CurrentDomain.BaseDirectory + this.FileName);
            }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(this.FilePath);
            }
        }

        public string FilePath
        {
            get
            {
                return this.mFilePath;
            }
            set
            {
                this.mFilePath = value;
            }
        }
    }
}

