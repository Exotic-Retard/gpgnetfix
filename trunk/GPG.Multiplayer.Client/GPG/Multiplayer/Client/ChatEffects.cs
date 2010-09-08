namespace GPG.Multiplayer.Client
{
    using GPG.Multiplayer.Client.ChatEffect;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public static class ChatEffects
    {
        public static readonly GPG.Multiplayer.Client.ChatEffect.Canadian Canadian = new GPG.Multiplayer.Client.ChatEffect.Canadian();
        public static readonly GPG.Multiplayer.Client.ChatEffect.Chicken Chicken = new GPG.Multiplayer.Client.ChatEffect.Chicken();
        private static Dictionary<string, ChatEffectBase> mAll = null;
        private static ChatEffectBase mCurrent = null;
        public static readonly GPG.Multiplayer.Client.ChatEffect.None None = new GPG.Multiplayer.Client.ChatEffect.None();

        public static ChatEffectBase FindByDescription(string description)
        {
            return All[description];
        }

        public static ChatEffectBase FindByStatus(string status)
        {
            foreach (ChatEffectBase base2 in All.Values)
            {
                if (base2.Status.Name.ToLower() == status.ToLower())
                {
                    return base2;
                }
            }
            return null;
        }

        public static bool TryFindByDescription(string description, out ChatEffectBase effect)
        {
            effect = null;
            foreach (ChatEffectBase base2 in All.Values)
            {
                if (base2.Name.ToLower() == description.ToLower())
                {
                    effect = base2;
                    return true;
                }
            }
            return false;
        }

        public static bool TryFindByStatus(string status, out ChatEffectBase chatEffect)
        {
            foreach (ChatEffectBase base2 in All.Values)
            {
                if (base2.Status.Name.ToLower() == status.ToLower())
                {
                    chatEffect = base2;
                    return true;
                }
            }
            chatEffect = null;
            return false;
        }

        public static Dictionary<string, ChatEffectBase> All
        {
            get
            {
                if (mAll == null)
                {
                    FieldInfo[] fields = typeof(ChatEffects).GetFields(BindingFlags.Public | BindingFlags.Static);
                    mAll = new Dictionary<string, ChatEffectBase>(fields.Length);
                    foreach (FieldInfo info in fields)
                    {
                        if (info.FieldType.GetInterface("ITextEffect") != null)
                        {
                            ChatEffectBase base2 = info.GetValue(null) as ChatEffectBase;
                            mAll.Add(base2.Name, base2);
                        }
                    }
                }
                return mAll;
            }
        }

        public static ChatEffectBase Current
        {
            get
            {
                return mCurrent;
            }
            set
            {
                mCurrent = value;
            }
        }
    }
}

