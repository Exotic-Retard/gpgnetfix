namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Content_Download
    {
        private bool mCachePreviewImages = true;
        private bool mGetLatestVersion = false;
        private string mMyDownloads = (Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\MyContent\Downloads");
        private string mSearchType = null;

        [field: NonSerialized]
        public event PropertyChangedEventHandler CachePreviewImagesChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler GetLatestVersionChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MyDownloadsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SearchTypeChanged;

        [DisplayName("<LOC>Cache Preview Images"), Category("<LOC>Misc"), Description("<LOC>Caches preview images on your local hard drive which speed up image retrieval in exchange for a small amount of disk space.")]
        public bool CachePreviewImages
        {
            get
            {
                return this.mCachePreviewImages;
            }
            set
            {
                this.mCachePreviewImages = value;
                if (this.CachePreviewImagesChanged != null)
                {
                    this.CachePreviewImagesChanged(this, new PropertyChangedEventArgs("CachePreviewImages"));
                }
            }
        }

        [DisplayName("<LOC>Always Get Latest Version"), Description("<LOC>"), Category("<LOC>Misc")]
        public bool GetLatestVersion
        {
            get
            {
                return this.mGetLatestVersion;
            }
            set
            {
                this.mGetLatestVersion = value;
                if (this.GetLatestVersionChanged != null)
                {
                    this.GetLatestVersionChanged(this, new PropertyChangedEventArgs("GetLatestVersion"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Misc"), DisplayName("<LOC>MyDownloads")]
        public string MyDownloads
        {
            get
            {
                return this.mMyDownloads;
            }
            set
            {
                this.mMyDownloads = value;
                if (this.MyDownloadsChanged != null)
                {
                    this.MyDownloadsChanged(this, new PropertyChangedEventArgs("MyDownloads"));
                }
            }
        }

        [Description("<LOC>"), DisplayName("<LOC>SearchType"), Category("<LOC>Misc")]
        public string SearchType
        {
            get
            {
                return this.mSearchType;
            }
            set
            {
                this.mSearchType = value;
                if (this.SearchTypeChanged != null)
                {
                    this.SearchTypeChanged(this, new PropertyChangedEventArgs("SearchType"));
                }
            }
        }
    }
}

