namespace GPG.Multiplayer.Client.Clans
{
    using GPG;
    using System;
    using System.Runtime.InteropServices;

    public class ClanAbility
    {
        private static ClanAbilityDictionary _All;
        private string _Description;
        private int _MinPriority;
        public static readonly ClanAbility AddEnemy = new ClanAbility("<LOC>Add Enemy", 2);
        public static readonly ClanAbility Ban = new ClanAbility("<LOC>Ban", 3);
        public static readonly ClanAbility ChangeProfile = new ClanAbility("<LOC>Change Profile", 1);
        public static readonly ClanAbility Demote = new ClanAbility("<LOC>Demote", 1);
        public static readonly ClanAbility Invite = new ClanAbility("<LOC>Invite", 2);
        public static readonly ClanAbility Kick = new ClanAbility("<LOC>Kick", 4);
        public static readonly ClanAbility Promote = new ClanAbility("<LOC>Promote", 2);
        public static readonly ClanAbility Remove = new ClanAbility("<LOC>Remove", 3);
        public static readonly ClanAbility Unban = new ClanAbility("<LOC>Unban", 3);

        private ClanAbility(string desc, int minPriority)
        {
            this._Description = Loc.Get(desc);
            this._MinPriority = minPriority;
        }

        public override bool Equals(object obj)
        {
            return ((obj is ClanAbility) && ((obj as ClanAbility).Description == this.Description));
        }

        public override int GetHashCode()
        {
            return this.Description.GetHashCode();
        }

        public bool IsAvailable(ClanMember member)
        {
            return this.IsAvailable(member.Rank);
        }

        public bool IsAvailable(int rank)
        {
            return (rank <= this.MinPriority);
        }

        public override string ToString()
        {
            return this.Description;
        }

        public static bool TryGetAbility(string name, out ClanAbility ability)
        {
            if (All.Contains(name))
            {
                ability = All[name];
                return true;
            }
            ability = null;
            return false;
        }

        public static ClanAbilityDictionary All
        {
            get
            {
                if (_All == null)
                {
                    _All = new ClanAbilityDictionary();
                    _All.Add(Invite);
                    _All.Add(Kick);
                    _All.Add(Ban);
                    _All.Add(Promote);
                    _All.Add(AddEnemy);
                    _All.Add(Demote);
                    _All.Add(ChangeProfile);
                }
                return _All;
            }
        }

        public string Description
        {
            get
            {
                return this._Description;
            }
        }

        public int MinPriority
        {
            get
            {
                return this._MinPriority;
            }
        }
    }
}

