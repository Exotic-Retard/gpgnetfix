namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_WebBrowsing
    {
        private bool mEmbedBrowser = false;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EmbedBrowserChanged;

        [DisplayName("<LOC>Embedded Browser"), Description("<LOC>If true, displays web pages within GPGnet: Supreme Commander. If false, opens in default web browser."), Category("<LOC>Misc")]
        public bool EmbedBrowser
        {
            get
            {
                return this.mEmbedBrowser;
            }
            set
            {
                this.mEmbedBrowser = value;
                if (this.EmbedBrowserChanged != null)
                {
                    this.EmbedBrowserChanged(this, new PropertyChangedEventArgs("EmbedBrowser"));
                }
            }
        }
    }
}

