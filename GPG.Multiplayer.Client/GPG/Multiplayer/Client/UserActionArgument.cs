namespace GPG.Multiplayer.Client
{
    using GPG;
    using System;

    public class UserActionArgument
    {
        private string mName;
        private bool mRequired;

        public UserActionArgument(string name)
        {
            this.mName = Loc.Get(name);
            this.mRequired = true;
        }

        public UserActionArgument(string name, bool required)
        {
            this.mName = Loc.Get(name);
            this.mRequired = required;
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public bool Required
        {
            get
            {
                return this.mRequired;
            }
        }
    }
}

