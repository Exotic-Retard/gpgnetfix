namespace GPG.Multiplayer.Client
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
    internal class PopulationIcons
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal PopulationIcons()
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

        internal static Bitmap empty
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("empty", resourceCulture);
            }
        }

        internal static Bitmap full
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("full", resourceCulture);
            }
        }

        internal static Bitmap low
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("low", resourceCulture);
            }
        }

        internal static Bitmap medium
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("medium", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.Multiplayer.Client.PopulationIcons", typeof(PopulationIcons).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }
    }
}

