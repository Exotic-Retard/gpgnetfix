namespace GPG.Multiplayer.Client
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Content_Upload
    {
        private List<string> mCachedLocations = new List<string>();
        private string mMyUploads = (Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\MyContent\Uploads");
        private List<string> mUploadPaths = new List<string>(new string[] { Environment.GetFolderPath(Environment.SpecialFolder.Personal) });

        [field: NonSerialized]
        public event PropertyChangedEventHandler MyUploadsChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler UploadPathsChanged;

        [Browsable(false)]
        public List<string> CachedLocations
        {
            get
            {
                return this.mCachedLocations;
            }
        }

        [DisplayName("<LOC>MyUploads"), Category("<LOC>Misc"), Description("<LOC>")]
        public string MyUploads
        {
            get
            {
                return this.mMyUploads;
            }
            set
            {
                this.mMyUploads = value;
                if (this.mMyUploadsChanged != null)
                {
                    this.mMyUploadsChanged(this, new PropertyChangedEventArgs("MyUploads"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Misc"), DisplayName("<LOC>Upload Paths")]
        public List<string> UploadPaths
        {
            get
            {
                return this.mUploadPaths;
            }
            set
            {
                this.mUploadPaths = value;
                if (this.mUploadPathsChanged != null)
                {
                    this.mUploadPathsChanged(this, new PropertyChangedEventArgs("UploadPaths"));
                }
            }
        }
    }
}

