namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Collections.Generic;

    public class SupComStatsInfo
    {
        public int built;
        public double damageDone;
        public double damageReceived;
        public double duration;
        public double energyConsumed;
        public double energyProduced;
        public string faction = "";
        public string gametype = "1v1";
        public int kills;
        public int lost;
        public string map = "";
        public double massConsumed;
        public double massProduced;
        public string playername = "";
        public string result = "Playing";
        public List<SupcomStatUnitInfo> UnitInfo = new List<SupcomStatUnitInfo>();
    }
}

