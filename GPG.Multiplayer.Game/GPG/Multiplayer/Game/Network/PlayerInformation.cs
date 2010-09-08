namespace GPG.Multiplayer.Game.Network
{
    using System;
    using System.Net;

    public class PlayerInformation
    {
        public IPEndPoint EndPoint;
        private static PlayerInformation mEmpty = new PlayerInformation("Empty -1");
        public int PlayerID = -1;
        public string PlayerName = "";

        public PlayerInformation(string data)
        {
            string[] strArray = data.Split(" ".ToCharArray(), 2);
            if (strArray.Length == 2)
            {
                try
                {
                    this.PlayerID = Convert.ToInt32(strArray[1]);
                    this.PlayerName = strArray[0];
                }
                catch
                {
                    this.PlayerID = Convert.ToInt32(strArray[0]);
                    this.PlayerName = strArray[1];
                }
            }
        }

        public static PlayerInformation Empty()
        {
            return mEmpty;
        }
    }
}

