namespace GPG.Multiplayer.Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct FLASHWINFO
    {
        public ushort cbSize;
        public IntPtr hwnd;
        public uint dwFlags;
        public ushort uCount;
        public uint dwTimeout;
    }
}

