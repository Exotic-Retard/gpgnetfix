namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;

    public class UserStatus
    {
        public static readonly UserStatus Away = new UserStatus("away", "<LOC>Away", StatusIcons.idle);
        public static readonly UserStatus Canadian = new UserStatus("canadian", "<LOC>Canadian", StatusIcons.canada);
        public static readonly UserStatus Chicken = new UserStatus("chicken", "<LOC>Chicken", StatusIcons.chicken);
        public static readonly UserStatus DND = new UserStatus("dnd", "<LOC>Do Not Disturb", StatusIcons.dnd);
        public static readonly UserStatus Ignored = new UserStatus("ignored", "<LOC>Ignored", StatusIcons.ignored);
        private static Dictionary<string, UserStatus> mAll = null;
        private string mDescription;
        private Image mIcon;
        private string mName;
        public static readonly UserStatus None = new UserStatus("none", "<LOC>Online", StatusIcons.online);
        public static readonly UserStatus Offline = new UserStatus("offline", "<LOC>Offline", StatusIcons.offline);

        public UserStatus(string name, string description, Image icon)
        {
            this.mName = name;
            this.mDescription = description;
            this.mIcon = icon;
        }

        public static UserStatus FindByDescription(string description)
        {
            return All[description];
        }

        public static UserStatus GetStatus(User user)
        {
            if (!(user.Visible && user.Online))
            {
                return Offline;
            }
            if (user.IsIgnored)
            {
                return Ignored;
            }
            if (user.IsDND)
            {
                return DND;
            }
            if (user.IsAway)
            {
                return Away;
            }
            if ((user.Equals(User.Current) && (ChatEffects.Current != null)) && ChatEffects.Current.Equals(ChatEffects.Chicken))
            {
                return Chicken;
            }
            if ((user.Equals(User.Current) && (ChatEffects.Current != null)) && ChatEffects.Current.Equals(ChatEffects.Canadian))
            {
                return Canadian;
            }
            return None;
        }

        public static Dictionary<string, UserStatus> All
        {
            get
            {
                if (mAll == null)
                {
                    FieldInfo[] fields = typeof(UserStatus).GetFields(BindingFlags.Public | BindingFlags.Static);
                    mAll = new Dictionary<string, UserStatus>(fields.Length);
                    foreach (FieldInfo info in fields)
                    {
                        if (info.FieldType == typeof(UserStatus))
                        {
                            UserStatus status = info.GetValue(null) as UserStatus;
                            mAll.Add(status.Name, status);
                        }
                    }
                }
                return mAll;
            }
        }

        public static UserStatus Current
        {
            get
            {
                return GetStatus(User.Current);
            }
        }

        public string Description
        {
            get
            {
                return Loc.Get(this.mDescription);
            }
        }

        public Image Icon
        {
            get
            {
                return this.mIcon;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }
    }
}

