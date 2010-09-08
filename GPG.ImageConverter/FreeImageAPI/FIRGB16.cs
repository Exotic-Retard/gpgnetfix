namespace FreeImageAPI
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class FIRGB16
    {
        public ushort red;
        public ushort green;
        public ushort blue;
    }
}

