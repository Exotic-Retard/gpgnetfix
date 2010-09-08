namespace GPG
{
    using IWshRuntimeLibrary;
    using System;
    using System.IO;

    public static class FileUtil
    {
        public const int GB = 0x40000000;
        public const int KB = 0x400;
        public const int MB = 0x100000;

        public static void CreateShortcut(string target, string linkPath)
        {
            if (Path.HasExtension(linkPath))
            {
                string extension = Path.GetExtension(linkPath);
                if (extension != ".lnk")
                {
                    linkPath = linkPath.Remove(linkPath.Length - extension.Length, extension.Length) + ".lnk";
                }
            }
            else
            {
                linkPath = linkPath + ".lnk";
            }
            WshShell shell = new WshShellClass();
            IWshShortcut shortcut = (IWshShortcut) shell.CreateShortcut(linkPath);
            shortcut.TargetPath = target;
            shortcut.Save();
        }

        public static string RemoveExt(string path)
        {
            string extension = Path.GetExtension(path);
            return path.Remove(path.Length - extension.Length, extension.Length);
        }

        public static void RemoveReadOnly(string file)
        {
            FileAttributes fileAttributes = System.IO.File.GetAttributes(file) & ~FileAttributes.ReadOnly;
            System.IO.File.SetAttributes(file, fileAttributes);
        }

        public static int SizeInGB(string file)
        {
            return (int) (new FileInfo(file).Length / 0x40000000L);
        }

        public static int SizeInKB(string file)
        {
            return (int) (new FileInfo(file).Length / 0x400L);
        }

        public static int SizeInMB(string file)
        {
            return (int) (new FileInfo(file).Length / 0x100000L);
        }
    }
}

