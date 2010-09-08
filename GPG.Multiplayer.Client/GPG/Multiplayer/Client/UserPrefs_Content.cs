namespace GPG.Multiplayer.Client
{
    using GPG;
    using System;
    using System.Collections.Generic;
    using GPG.Multiplayer.Client.Vaulting;

    [Serializable]
    public class UserPrefs_Content
    {
        private UserPrefs_Content_Download mDownload;
        private UserPrefs_Content_Upload mUpload;
        internal List<IVaultOperation> ResumeOperations = new List<IVaultOperation>();

        [OptionsRoot("<LOC>Download")]
        public UserPrefs_Content_Download Download
        {
            get
            {
                if (this.mDownload == null)
                {
                    this.mDownload = new UserPrefs_Content_Download();
                }
                return this.mDownload;
            }
        }

        [OptionsRoot("<LOC>Upload")]
        public UserPrefs_Content_Upload Upload
        {
            get
            {
                if (this.mUpload == null)
                {
                    this.mUpload = new UserPrefs_Content_Upload();
                }
                return this.mUpload;
            }
        }
    }
}

