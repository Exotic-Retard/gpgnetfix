namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ConnectionInfo
    {
        public string IPAddress;
        public int Port;
        public ConnectionType ConnectionKind;
        public override string ToString()
        {
            return (this.IPAddress + " " + this.Port.ToString() + " " + this.ConnectionKind.ToString());
        }
    }
}

