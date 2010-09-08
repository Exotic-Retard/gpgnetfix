namespace GPG.Multiplayer.Client.Games
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class ExtractIcon
    {
        private ExtractIcon()
        {
        }

        public static Icon GetIcon(string strPath, bool bSmall)
        {
            SHGFI shgfi;
            SHFILEINFO structure = new SHFILEINFO(true);
            int num = Marshal.SizeOf(structure);
            if (bSmall)
            {
                shgfi = SHGFI.Icon | SHGFI.UseFileAttributes | SHGFI.SmallIcon;
            }
            else
            {
                shgfi = SHGFI.Icon | SHGFI.UseFileAttributes;
            }
            SHGetFileInfo(strPath, 0x100, out structure, (uint) num, shgfi);
            return Icon.FromHandle(structure.hIcon);
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szTypeName;
            public SHFILEINFO(bool b)
            {
                this.hIcon = IntPtr.Zero;
                this.iIcon = 0;
                this.dwAttributes = 0;
                this.szDisplayName = "";
                this.szTypeName = "";
            }
        }

        private enum SHGFI
        {
            DisplayName = 0x200,
            Icon = 0x100,
            LargeIcon = 0,
            SmallIcon = 1,
            SysIconIndex = 0x4000,
            Typename = 0x400,
            UseFileAttributes = 0x10
        }
    }
}

