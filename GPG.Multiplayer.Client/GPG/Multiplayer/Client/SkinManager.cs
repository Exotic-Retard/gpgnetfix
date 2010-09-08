namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;

    internal static class SkinManager
    {
        internal static Image GetImage(string imageName)
        {
            Exception exception;
            string path = SkinDirectory + imageName;
            if (File.Exists(path))
            {
                try
                {
                    return Image.FromFile(path);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                    ErrorLog.WriteLine("Corrupt or incorrectly formatted skin file: {0}", new object[] { SkinDirectory + imageName });
                    return null;
                }
            }
            string str2 = path.Replace(Program.Settings.SkinName, "default");
            if (File.Exists(str2))
            {
                try
                {
                    return Image.FromFile(str2);
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    ErrorLog.WriteLine(exception);
                    ErrorLog.WriteLine("Corrupt or incorrectly formatted skin file: {0}", new object[] { SkinDirectory + imageName });
                    return null;
                }
            }
            ErrorLog.WriteLine("Missing skin file: {0}", new object[] { SkinDirectory + imageName });
            return null;
        }

        public static bool ImageExists(string imageName)
        {
            return File.Exists(SkinDirectory + imageName);
        }

        internal static bool TryGetImage(string imageName, out Image image)
        {
            Exception exception;
            string path = SkinDirectory + imageName;
            if (File.Exists(path))
            {
                try
                {
                    image = Image.FromFile(path);
                    return true;
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    ErrorLog.WriteLine(exception);
                    ErrorLog.WriteLine("Corrupt or incorrectly formatted skin file: {0}", new object[] { SkinDirectory + imageName });
                    image = null;
                    return false;
                }
            }
            string str2 = path.Replace(Program.Settings.SkinName, "default");
            if (File.Exists(str2))
            {
                try
                {
                    image = Image.FromFile(str2);
                    return true;
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    ErrorLog.WriteLine(exception);
                    ErrorLog.WriteLine("Corrupt or incorrectly formatted skin file: {0}", new object[] { SkinDirectory + imageName });
                    image = null;
                    return false;
                }
            }
            ErrorLog.WriteLine("Missing skin file: {0}", new object[] { SkinDirectory + imageName });
            image = null;
            return false;
        }

        public static string SkinDirectory
        {
            get
            {
                return string.Format(@"{0}skins\{1}\", AppDomain.CurrentDomain.BaseDirectory, Program.Settings.StylePreferences.SkinName);
            }
        }
    }
}

