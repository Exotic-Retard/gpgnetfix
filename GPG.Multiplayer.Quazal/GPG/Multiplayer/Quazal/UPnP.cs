namespace GPG.Multiplayer.Quazal
{
    using GPG;
    using GPG.Logging;
    using NATUPNPLib;
    using System;
    using System.Net;

    public static class UPnP
    {
        public static void OpenPort()
        {
            try
            {
                UPnPNATClass class2 = new UPnPNATClass();
                IStaticPortMappingCollection staticPortMappingCollection = class2.StaticPortMappingCollection;
                int port = GPGnetSelectedGame.SelectedGame.Port;
                if (port <= 0)
                {
                    port = 0x17e0;
                }
                if (staticPortMappingCollection != null)
                {
                    staticPortMappingCollection.Add(port, "UDP", port, Dns.GetHostName(), true, "GPGnet");
                    EventLog.WriteLine("[UPnP] Port mapped to " + port.ToString(), new object[0]);
                }
                else
                {
                    ErrorLog.WriteLine("[UPnP] StaticPortMappings not available. Unable to open and forward port " + port.ToString() + " on internet gateway device.", new object[0]);
                    ErrorLog.WriteLine("[UPnP] UPnP may be disabled or unsupported by IGD.", new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine("[UPnP] Exception thrown, UPnP is unavailable:", new object[0]);
                ErrorLog.WriteLine(exception);
            }
        }
    }
}

