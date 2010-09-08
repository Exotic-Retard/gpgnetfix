namespace GPG.Multiplayer.Client.Controls.Awards
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
    internal class AwardsImages
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal AwardsImages()
        {
        }

        internal static Bitmap achieved
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("achieved", resourceCulture);
            }
        }

        internal static Bitmap award_empty
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("award_empty", resourceCulture);
            }
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
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.Multiplayer.Client.Controls.Awards.AwardsImages", typeof(AwardsImages).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }
    }
}

