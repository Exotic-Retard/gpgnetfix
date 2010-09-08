namespace GPG.Multiplayer.Client.Ladders
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.LadderService;
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class SupcomParamsProvider : GameParamsProvider
    {
        private DlgSupcomMapOptions dlg = null;
        private bool FailedToLoad = false;
        private string SpecifiedFaction = null;
        private string SpecifiedMap = null;

        private void ErrorOnLoad()
        {
            DlgMessage.ShowDialog("<LOC>You are missing a map that is required to play on this ladder. Please view the entry criteria instructions for assistance resolving this issue.", "<LOC>Error");
            this.FailedToLoad = true;
        }

        protected override bool GatherParams(out string map, out string faction)
        {
            if (this.FailedToLoad)
            {
                map = null;
                faction = null;
                return false;
            }
            if (this.dlg == null)
            {
                this.dlg = new DlgSupcomMapOptions("1v1");
            }
            if (this.dlg.ShowDialog() == DialogResult.OK)
            {
                this.SpecifiedMap = this.dlg.FetchMap();
                this.SpecifiedFaction = this.dlg.Faction;
            }
            else
            {
                map = null;
                faction = null;
                return false;
            }
            map = this.SpecifiedMap;
            faction = this.SpecifiedFaction;
            return true;
        }

        protected override void ProcessArgs(string args)
        {
            string[] strArray = args.Split(";".ToCharArray());
            this.dlg = new DlgSupcomMapOptions();
            bool flag = false;
            foreach (string str in strArray)
            {
                string str2 = str.Split("=".ToCharArray())[0].ToLower();
                string faction = str.Split("=".ToCharArray())[1].ToLower();
                string str5 = str2;
                if (str5 != null)
                {
                    if (!(str5 == "factions"))
                    {
                        if (str5 == "maps")
                        {
                            goto Label_01EA;
                        }
                    }
                    else
                    {
                        if (faction.IndexOf("aeon") < 0)
                        {
                            this.dlg.AeonEnabled = false;
                        }
                        if (faction.IndexOf("cybran") < 0)
                        {
                            this.dlg.CybranEnabled = false;
                        }
                        if (faction.IndexOf("seraphim") < 0)
                        {
                            this.dlg.SeraphimEnabled = false;
                        }
                        if (faction.IndexOf("uef") < 0)
                        {
                            this.dlg.UEFEnabled = false;
                        }
                        if (faction.IndexOf("random") < 0)
                        {
                            this.dlg.RandomEnabled = false;
                        }
                        if ((faction.IndexOf(",") < 0) && (faction != "random"))
                        {
                            this.SpecifiedFaction = "/" + faction;
                            this.dlg.SelectFaction(faction);
                        }
                        else if ((faction.IndexOf(",") < 0) && (faction == "random"))
                        {
                            this.SpecifiedFaction = faction;
                            this.dlg.SelectFaction(faction);
                        }
                        else
                        {
                            this.dlg.SelectPreferredFaction();
                        }
                    }
                }
                goto Label_028A;
            Label_01EA:
                if (faction.IndexOf(",") < 0)
                {
                    if (!this.dlg.AddMap(faction, false))
                    {
                        this.ErrorOnLoad();
                        return;
                    }
                    this.SpecifiedMap = faction;
                }
                else
                {
                    foreach (string str4 in faction.Split(",".ToCharArray()))
                    {
                        if (!this.dlg.AddMap(str4, true))
                        {
                            this.ErrorOnLoad();
                            return;
                        }
                    }
                }
                flag = true;
            Label_028A:;
            }
            if (!flag)
            {
                this.dlg.AddDefaultMaps("1v1");
            }
        }
    }
}

