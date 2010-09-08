namespace FreeImageAPI
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public class FIRGBAF
    {
        public float red;
        public float green;
        public float blue;
        public float alpha;
    }
}

