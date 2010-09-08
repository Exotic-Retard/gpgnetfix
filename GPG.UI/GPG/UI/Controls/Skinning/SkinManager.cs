namespace GPG.UI.Controls.Skinning
{
    using GPG.UI.Controls.Docking;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;

    public class SkinManager
    {
        public readonly string[] AllExtensions;
        private string mBasePath;
        private static SkinManager mDefault;
        private static SkinManager mDocking;
        private string mImageFileExtension;
        [NonSerialized]
        private ResourceManager mResourceTarget;
        private SkinResourceTypes mResourceType;

        public SkinManager()
        {
            this.AllExtensions = new string[] { ".png", ".gif", ".jpg", ".bmp" };
        }

        public SkinManager(ResourceManager resourceTarget)
        {
            this.AllExtensions = new string[] { ".png", ".gif", ".jpg", ".bmp" };
            this.mResourceTarget = resourceTarget;
            this.mResourceType = SkinResourceTypes.EmbeddedResource;
        }

        public SkinManager(string basePath)
        {
            this.AllExtensions = new string[] { ".png", ".gif", ".jpg", ".bmp" };
            this.mBasePath = basePath;
            if (!this.BasePath.EndsWith(@"\"))
            {
                this.BasePath = this.BasePath + @"\";
            }
            this.mResourceType = SkinResourceTypes.File;
        }

        public Image GetImage(ISkinResourceConsumer consumer)
        {
            return this.GetImage(consumer.ResourceKey);
        }

        public Image GetImage(string key)
        {
            if ((key == null) || (key.Length < 1))
            {
                throw new ArgumentNullException("key", "Resource key cannot be null or empty.");
            }
            switch (this.ResourceType)
            {
                case SkinResourceTypes.File:
                    return this.GetImageFromFile(key);

                case SkinResourceTypes.EmbeddedResource:
                    return this.GetImageFromResource(key);
            }
            throw new NotImplementedException(string.Format("A non implemented resource type was used on resource manager: {0}", "DockSkinHelper"));
        }

        public Image GetImageFromFile(ISkinResourceConsumer consumer)
        {
            return this.GetImageFromFile(consumer.ResourceKey);
        }

        public Image GetImageFromFile(string key)
        {
            string[] allExtensions;
            if ((this.ImageFileExtension == null) || (this.ImageFileExtension.Length < 1))
            {
                allExtensions = this.AllExtensions;
            }
            else
            {
                allExtensions = this.ImageFileExtension.Split("|".ToCharArray());
            }
            key = key.Replace(".", @"\");
            key = key + this.ImageFileExtension;
            bool flag = false;
            for (int i = 0; i < allExtensions.Length; i++)
            {
                if (File.Exists(this.BasePath + key + allExtensions[i]))
                {
                    if (flag)
                    {
                        throw new FileNotFoundException("Ambiguous match found for image file {0}", key);
                    }
                    flag = true;
                    key = key + allExtensions[i];
                }
            }
            if (!flag)
            {
                throw new FileNotFoundException(string.Format("Missing image file: {0}", this.BasePath + key));
            }
            return Image.FromFile(this.BasePath + key);
        }

        public Image GetImageFromResource(ISkinResourceConsumer consumer)
        {
            return this.GetImageFromResource(consumer.ResourceKey);
        }

        public Image GetImageFromResource(string key)
        {
            if (Path.HasExtension(key))
            {
                key = key.Replace(Path.GetExtension(key), "");
            }
            object obj2 = this.ResourceTarget.GetObject(key);
            if ((obj2 == null) || !(obj2 is Image))
            {
                throw new IOException(string.Format("Missing embedded resource: {0}", key));
            }
            return (obj2 as Image);
        }

        public string BasePath
        {
            get
            {
                return this.mBasePath;
            }
            set
            {
                this.mBasePath = value;
            }
        }

        public static SkinManager Default
        {
            get
            {
                if (mDefault == null)
                {
                    mDefault = new SkinManager(AppDomain.CurrentDomain.BaseDirectory + @"Skins\");
                }
                return mDefault;
            }
            set
            {
                mDefault = value;
            }
        }

        public static SkinManager Docking
        {
            get
            {
                if (mDocking == null)
                {
                    mDocking = new SkinManager(DockRes.ResourceManager);
                }
                return mDocking;
            }
            set
            {
                mDocking = value;
            }
        }

        public string ImageFileExtension
        {
            get
            {
                return this.mImageFileExtension;
            }
            set
            {
                this.mImageFileExtension = value;
            }
        }

        public static bool IsDesignMode
        {
            get
            {
                return false;
            }
        }

        [Browsable(false)]
        public ResourceManager ResourceTarget
        {
            get
            {
                return this.mResourceTarget;
            }
            set
            {
                this.mResourceTarget = value;
            }
        }

        public SkinResourceTypes ResourceType
        {
            get
            {
                if (IsDesignMode)
                {
                    return SkinResourceTypes.EmbeddedResource;
                }
                return this.mResourceType;
            }
            set
            {
                this.mResourceType = value;
            }
        }
    }
}

