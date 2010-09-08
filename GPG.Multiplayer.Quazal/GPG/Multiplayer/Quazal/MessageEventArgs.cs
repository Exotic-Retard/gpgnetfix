namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Collections.Generic;

    public class MessageEventArgs : EventArgs
    {
        private string mCommand;
        private string[] mCommandArgs;
        private bool mIsCommand;
        private string mMessage;

        public MessageEventArgs(string msg)
        {
            this.mMessage = msg;
            if (msg != null)
            {
                this.mIsCommand = msg.StartsWith("/");
            }
            if (this.IsCommand)
            {
                string[] sourceArray = msg.Split(new char[] { ' ' });
                this.mCommand = sourceArray[0].TrimStart(new char[] { '/' });
                if (this.mCommand.ToLower() == "cust")
                {
                    List<string> list = new List<string> {
                        sourceArray[1],
                        sourceArray[2],
                        sourceArray[3]
                    };
                    int index = msg.IndexOf('\x001f');
                    if (index >= 0)
                    {
                        string str = msg.Remove(0, index + 1);
                        list.AddRange(str.Split(new char[] { '\x001f' }));
                    }
                    this.mCommandArgs = list.ToArray();
                }
                else
                {
                    this.mCommandArgs = new string[sourceArray.Length - 1];
                    Array.ConstrainedCopy(sourceArray, 1, this.mCommandArgs, 0, this.mCommandArgs.Length);
                }
            }
        }

        public string Command
        {
            get
            {
                return this.mCommand;
            }
        }

        public string[] CommandArgs
        {
            get
            {
                return this.mCommandArgs;
            }
        }

        public bool IsCommand
        {
            get
            {
                return this.mIsCommand;
            }
        }

        public string Message
        {
            get
            {
                return this.mMessage;
            }
        }
    }
}

