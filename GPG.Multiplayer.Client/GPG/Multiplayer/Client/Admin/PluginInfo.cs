namespace GPG.Multiplayer.Client.Admin
{
    using GPG.Multiplayer.Plugin;
    using System;

    public class PluginInfo : IFormPlugin
    {
        private string mAuthor = "";
        private string mFormTitle = "";
        private PluginLocation mLocation = PluginLocation.MainMenu;
        private string mMenuCaption = "";
        private int mVersion = 0;
        public Type type = null;

        public IFormPlugin CreatePlugin()
        {
            if (this.type != null)
            {
                IFormPlugin plugin = Activator.CreateInstance(this.type) as IFormPlugin;
                plugin.RegisterDataHandler(new PersistentData());
                return plugin;
            }
            return null;
        }

        public void RegisterDataHandler(IPersistentData handler)
        {
        }

        public void SetUserInformation(string username, int userid)
        {
        }

        public string Author
        {
            get
            {
                return this.mAuthor;
            }
            set
            {
                this.mAuthor = value;
            }
        }

        public string FormTitle
        {
            get
            {
                return this.mFormTitle;
            }
            set
            {
                this.mFormTitle = value;
            }
        }

        public PluginLocation Location
        {
            get
            {
                return this.mLocation;
            }
            set
            {
                this.mLocation = value;
            }
        }

        public string MenuCaption
        {
            get
            {
                return this.mMenuCaption;
            }
            set
            {
                this.mMenuCaption = value;
            }
        }

        public int Version
        {
            get
            {
                return this.mVersion;
            }
            set
            {
                this.mVersion = value;
            }
        }
    }
}

