namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.Security;
    using System;

    [Serializable]
    public class ContentType : MappedObject
    {
        private static ContentType[] mAll;
        [FieldMap("download_acl")]
        private int mDownloadACL;
        [FieldMap("file_extensions")]
        private string mFileExtensions;
        [FieldMap("gpgnet_type")]
        private string mGPGnetType;
        private bool? mHasVolunteeredForUploads;
        [FieldMap("content_type_id")]
        private int mID;
        [FieldMap("image_index")]
        private int mImageIndex;
        [FieldMap("name")]
        private string mName;
        [FieldMap("singular_name")]
        private string mSingularName;
        [FieldMap("upload_acl")]
        private int mUploadACL;
        [FieldMap("ul_volunteer_effort")]
        private string mUploadVolunteerEffort;

        public ContentType(DataRecord record) : base(record)
        {
            this.mDownloadACL = 0;
            this.mUploadACL = 0;
            this.mUploadVolunteerEffort = null;
            this.mHasVolunteeredForUploads = null;
        }

        public static bool Assign(AdditionalContent content)
        {
            content.ContentType = FromID(content.TypeID);
            return (content.ContentType != null);
        }

        public IContentTypeProvider CreateInstance()
        {
            IContentTypeProvider provider = Activator.CreateInstance(Type.GetType(this.GPGnetType), true) as IContentTypeProvider;
            provider.ContentType = this;
            return provider;
        }

        public static ContentType FromID(int id)
        {
            foreach (ContentType type in All)
            {
                if (type.ID == id)
                {
                    return type;
                }
            }
            return null;
        }

        public bool IsValidUploadFile(string file)
        {
            foreach (string str in this.FileExtensions)
            {
                if (file.ToLower().EndsWith(str.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static ContentType[] All
        {
            get
            {
                if (mAll == null)
                {
                    mAll = DataAccess.GetObjects<ContentType>("GetAllContentTypes", new object[0]).ToArray();
                }
                return mAll;
            }
        }

        public bool CurrentUserCanDownload
        {
            get
            {
                return (User.Current.IsAdmin || ((this.DownloadACL == null) || this.DownloadACL.HasAccess()));
            }
        }

        public bool CurrentUserCanUpload
        {
            get
            {
                return (User.Current.IsAdmin || ((this.UploadACL == null) || this.UploadACL.HasAccess()));
            }
        }

        public string DisplayName
        {
            get
            {
                return Loc.Get("<LOC>" + this.Name);
            }
        }

        public AccessControlList DownloadACL
        {
            get
            {
                if (this.mDownloadACL == 0)
                {
                    return null;
                }
                return AccessControlList.GetByID(this.mDownloadACL);
            }
        }

        public string DownloadPath
        {
            get
            {
                return string.Format(@"{0}\{1}", AdditionalContent.MyContentPath, this.Name);
            }
        }

        public string[] FileExtensions
        {
            get
            {
                if (this.mFileExtensions == null)
                {
                    return new string[0];
                }
                return this.mFileExtensions.Split(new char[] { ',' });
            }
        }

        public string GPGnetType
        {
            get
            {
                return this.mGPGnetType;
            }
        }

        internal bool HasVolunteeredForUploads
        {
            get
            {
                if ((this.UploadVolunteerEffort == null) || (this.UploadVolunteerEffort.Length < 1))
                {
                    return true;
                }
                if (!this.mHasVolunteeredForUploads.HasValue)
                {
                    this.mHasVolunteeredForUploads = new bool?(new QuazalQuery("HasVolunteeredForUpload", new object[] { this.UploadVolunteerEffort }).GetBool());
                }
                return this.mHasVolunteeredForUploads.Value;
            }
            set
            {
                this.mHasVolunteeredForUploads = new bool?(value);
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public int ImageIndex
        {
            get
            {
                return this.mImageIndex;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public string SingularDisplayName
        {
            get
            {
                return Loc.Get("<LOC>" + this.SingularName);
            }
        }

        public string SingularName
        {
            get
            {
                return this.mSingularName;
            }
        }

        public AccessControlList UploadACL
        {
            get
            {
                if (this.mUploadACL == 0)
                {
                    return null;
                }
                return AccessControlList.GetByID(this.mUploadACL);
            }
        }

        public string UploadVolunteerEffort
        {
            get
            {
                return this.mUploadVolunteerEffort;
            }
        }
    }
}

