namespace GPG.Multiplayer.Client.Clans
{
    using GPG.DataAccess;
    using System;

    public class ClanView
    {
        private GPG.Multiplayer.Client.Clans.Clan mClan;
        private string mClanName;
        private MappedObjectList<ClanMember> mMembers;
        private string mPlayerName;

        public ClanView(GPG.Multiplayer.Client.Clans.Clan clan, MappedObjectList<ClanMember> members, string name, bool isPlayerName)
        {
            this.mClan = clan;
            this.mMembers = members;
            if (isPlayerName)
            {
                this.mPlayerName = name;
            }
            else
            {
                this.mClanName = name;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj.GetHashCode() == this.GetHashCode());
        }

        public override int GetHashCode()
        {
            return this.Clan.ID;
        }

        public GPG.Multiplayer.Client.Clans.Clan Clan
        {
            get
            {
                return this.mClan;
            }
            set
            {
                this.mClan = value;
            }
        }

        public string ClanName
        {
            get
            {
                return this.mClanName;
            }
            set
            {
                this.mClanName = value;
            }
        }

        public MappedObjectList<ClanMember> Members
        {
            get
            {
                return this.mMembers;
            }
            set
            {
                this.mMembers = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
            set
            {
                this.mPlayerName = value;
            }
        }
    }
}

