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
    internal class StatusIcons
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal StatusIcons()
        {
        }

        internal static Bitmap canada
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("canada", resourceCulture);
            }
        }

        internal static Bitmap cancel
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("cancel", resourceCulture);
            }
        }

        internal static Bitmap chicken
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("chicken", resourceCulture);
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

        internal static Bitmap dnd
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("dnd", resourceCulture);
            }
        }

        internal static Bitmap idle
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("idle", resourceCulture);
            }
        }

        internal static Bitmap ignored
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ignored", resourceCulture);
            }
        }

        internal static Bitmap in_game
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("in_game", resourceCulture);
            }
        }

        internal static Bitmap offline
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("offline", resourceCulture);
            }
        }

        internal static Bitmap online
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("online", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.Multiplayer.Client.StatusIcons", typeof(StatusIcons).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap search
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("search", resourceCulture);
            }
        }
    }
}

