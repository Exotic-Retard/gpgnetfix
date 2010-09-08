namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class SupcomFactions
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal SupcomFactions()
        {
        }

        internal static Bitmap aeon
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("aeon", resourceCulture);
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

        internal static Bitmap cybran
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("cybran", resourceCulture);
            }
        }

        internal static Bitmap random
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("random", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.Multiplayer.Client.Games.SupremeCommander.SupcomFactions", typeof(SupcomFactions).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap seraphim
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("seraphim", resourceCulture);
            }
        }

        internal static Bitmap uef
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("uef", resourceCulture);
            }
        }
    }
}

