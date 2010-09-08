namespace FreeImageAPI
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class FIICCPROFILE
    {
        public ushort flags;
        public uint size;
        public IntPtr data;
    }
}

