namespace GPG.Multiplayer.LadderService
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    internal class LadderImages
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal LadderImages()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.Multiplayer.LadderService.LadderImages", typeof(LadderImages).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap star
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("star", resourceCulture);
            }
        }

        internal static Bitmap star_empty
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("star_empty", resourceCulture);
            }
        }

        internal static Bitmap star_gray
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("star_gray", resourceCulture);
            }
        }

        internal static Bitmap stars
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stars", resourceCulture);
            }
        }

        internal static Bitmap stars_gray
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stars_gray", resourceCulture);
            }
        }

        internal static Bitmap stars_large
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stars_large", resourceCulture);
            }
        }

        internal static Bitmap stars_small
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stars_small", resourceCulture);
            }
        }

        internal static Bitmap stars_small_blue
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stars_small_blue", resourceCulture);
            }
        }

        internal static Bitmap stars_small_gray
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stars_small_gray", resourceCulture);
            }
        }
    }
}

