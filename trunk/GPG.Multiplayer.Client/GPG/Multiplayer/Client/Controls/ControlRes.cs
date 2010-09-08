namespace GPG.Multiplayer.Client.Controls
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
    internal class ControlRes
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal ControlRes()
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

        internal static Bitmap not_ready
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("not_ready", resourceCulture);
            }
        }

        internal static Bitmap open_ping
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("open_ping", resourceCulture);
            }
        }

        internal static Bitmap open_ready
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("open_ready", resourceCulture);
            }
        }

        internal static Bitmap ping
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ping", resourceCulture);
            }
        }

        internal static Bitmap ready
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ready", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.Multiplayer.Client.Controls.ControlRes", typeof(ControlRes).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap stat_icon1
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stat_icon1", resourceCulture);
            }
        }

        internal static Bitmap stat_icon2
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stat_icon2", resourceCulture);
            }
        }

        internal static Bitmap stat_icon3
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stat_icon3", resourceCulture);
            }
        }
    }
}

