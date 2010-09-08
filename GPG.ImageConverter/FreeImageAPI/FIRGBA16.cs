namespace FreeImageAPI
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class FIRGBA16
    {
        public ushort red;
        public ushort green;
        public ushort blue;
        public ushort alpha;
    }
}

