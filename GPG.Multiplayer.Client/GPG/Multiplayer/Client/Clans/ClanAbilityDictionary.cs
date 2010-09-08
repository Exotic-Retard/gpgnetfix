namespace GPG.Multiplayer.Client.Clans
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class ClanAbilityDictionary : DictionaryBase
    {
        public void Add(ClanAbility ability)
        {
            base.InnerHashtable.Add(ability.Description, ability);
        }

        public bool Contains(ClanAbility ability)
        {
            return this.Contains(ability.Description);
        }

        public bool Contains(string name)
        {
            return base.InnerHashtable.ContainsKey(name);
        }

        public void Remove(ClanAbility ability)
        {
            this.Remove(ability.Description);
        }

        public void Remove(string desc)
        {
            base.InnerHashtable.Remove(desc);
        }

        public ClanAbility[] ToArray()
        {
            ClanAbility[] array = new ClanAbility[base.InnerHashtable.Count];
            base.InnerHashtable.Values.CopyTo(array, 0);
            return array;
        }

        public ClanAbility this[string name]
        {
            get
            {
                return (base.InnerHashtable[name] as ClanAbility);
            }
        }
    }
}

