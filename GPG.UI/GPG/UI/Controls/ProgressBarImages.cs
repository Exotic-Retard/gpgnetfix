namespace GPG.UI.Controls
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
    internal class ProgressBarImages
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal ProgressBarImages()
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

        internal static Bitmap left_bg
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("left_bg", resourceCulture);
            }
        }

        internal static Bitmap left_progress
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("left_progress", resourceCulture);
            }
        }

        internal static Bitmap mid_bg
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("mid_bg", resourceCulture);
            }
        }

        internal static Bitmap mid_progress
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("mid_progress", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("GPG.UI.Controls.ProgressBarImages", typeof(ProgressBarImages).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap right_bg
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("right_bg", resourceCulture);
            }
        }

        internal static Bitmap right_progress
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("right_progress", resourceCulture);
            }
        }
    }
}

