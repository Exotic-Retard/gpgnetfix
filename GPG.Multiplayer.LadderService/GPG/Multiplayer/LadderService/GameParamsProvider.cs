namespace GPG.Multiplayer.LadderService
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class GameParamsProvider
    {
        private string Faction;
        private string Map;
        private bool ParamsGathered;

        protected GameParamsProvider()
        {
        }

        protected virtual bool GatherParams(out string map, out string faction)
        {
            map = null;
            faction = null;
            return false;
        }

        public string GetFaction()
        {
            if (!this.ParamsGathered && this.GatherParams(out this.Map, out this.Faction))
            {
                this.ParamsGathered = true;
            }
            return this.Faction;
        }

        public string GetMap()
        {
            if (!this.ParamsGathered && this.GatherParams(out this.Map, out this.Faction))
            {
                this.ParamsGathered = true;
            }
            return this.Map;
        }

        protected internal virtual void ProcessArgs(string args)
        {
        }

        public virtual void Reset()
        {
            this.Map = null;
            this.Faction = null;
            this.ParamsGathered = false;
        }
    }
}

