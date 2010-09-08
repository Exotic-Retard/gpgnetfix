namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SNatMessagePtr
    {
        public IntPtr Address;
        public int Port;
        public int MessageSize;
        public IntPtr Message;
    }
}

