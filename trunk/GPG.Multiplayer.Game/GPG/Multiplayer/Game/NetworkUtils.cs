namespace GPG.Multiplayer.Game
{
    using System;
    using System.Net;

    public static class NetworkUtils
    {
        public static IPEndPoint ConvertAddress(string address)
        {
            string[] strArray = address.Split(new char[] { ':' });
            if (strArray.Length != 2)
            {
                throw new Exception("This is not a valid address.");
            }
            return new IPEndPoint(IPAddress.Parse(strArray[0]), Convert.ToUInt16(strArray[1]));
        }
    }
}

