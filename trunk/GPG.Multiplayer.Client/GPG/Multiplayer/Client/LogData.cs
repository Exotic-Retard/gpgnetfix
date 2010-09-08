namespace GPG.Multiplayer.Client
{
    using System;

    public class LogData
    {
        private object mData = "<null>";
        private System.DateTime mDateTime = System.DateTime.Now;
        private string mDescription;
        private string mLogType;

        public void SetData(object data)
        {
            if (data != null)
            {
                this.mData = data;
            }
            else
            {
                this.mData = "<null>";
            }
        }

        public string Data
        {
            get
            {
                if (this.mData is object[])
                {
                    string str = "";
                    string str2 = "";
                    foreach (object obj2 in this.mData as object[])
                    {
                        if (obj2 != null)
                        {
                            str = str + " " + obj2.ToString() + str2;
                        }
                        else
                        {
                            str = str + " <null>" + str2;
                        }
                        str2 = "\r\n";
                    }
                    return str;
                }
                if (this.mData == null)
                {
                    return "<null>";
                }
                return this.mData.ToString();
            }
            set
            {
                this.SetData(value);
            }
        }

        public System.DateTime DateTime
        {
            get
            {
                return this.mDateTime;
            }
            set
            {
                this.mDateTime = value;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        public string LogType
        {
            get
            {
                return this.mLogType;
            }
            set
            {
                this.mLogType = value;
            }
        }
    }
}

