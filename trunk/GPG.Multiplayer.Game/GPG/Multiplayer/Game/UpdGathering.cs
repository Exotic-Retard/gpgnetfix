namespace GPG.Multiplayer.Game
{
    using GPG.Multiplayer.Game.Network;
    using GPG.Multiplayer.Quazal;
    using System;

    public class UpdGathering
    {
        private UdpConnection mConnection;
        private TrafficManager mManager;
        private int mPort;

        public UpdGathering(int port)
        {
            this.mPort = port;
            this.mManager = new TrafficManager();
            this.mConnection = new UdpConnection();
            this.mConnection.Bind(this.mPort, User.Current.ID);
            this.mManager.SetConnection(this.mConnection);
        }

        public void HostGathering(string name)
        {
        }

        public void JoinGathering(string name)
        {
        }
    }
}

