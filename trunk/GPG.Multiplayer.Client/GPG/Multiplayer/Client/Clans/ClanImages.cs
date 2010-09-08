namespace GPG.Multiplayer.Client.Clans
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), DebuggerNonUserCode]
    internal class ClanImages
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal ClanImages()
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

        internal static Bitmap rank_cmd
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("rank_cmd", resourceCulture);
            }
        }

        internal static Bitmap rank_msgt
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("rank_msgt", resourceCulture);
            }
        }

        internal static Bitmap rank_pfc
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("rank_pfc", resourceCulture);
            }
        }

        internal static Bitmap rank_pvt
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("rank_pvt", resourceCulture);
            }
        }

        internal static Bitmap rank_sgt
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("rank_sgt", resourceCulture);
            }
        }

        internal static Bitmap rank_supcom
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("rank_supcom", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.Multiplayer.Client.Clans.ClanImages", typeof(ClanImages).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }
    }
}

