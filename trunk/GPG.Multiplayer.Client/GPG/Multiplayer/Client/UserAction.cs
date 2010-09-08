namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Multiplayer.Quazal;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class UserAction : ITextVal
    {
        public static readonly UserAction AllowBypassDND = new UserAction("<LOC>Allow Friends / Clanmates to Bypass Do Not Disturb", UserActionCommands.AllowFriends, new string[] { Loc.Get("<LOC>/allow") }, new UserActionArgument[0]);
        public static readonly UserAction Away = new UserAction("<LOC>Away", UserActionCommands.Away, new string[] { Loc.Get("<LOC>/a"), Loc.Get("<LOC>/away"), Loc.Get("<LOC>/afk") }, new UserActionArgument[0]);
        public static readonly UserAction Ban = new UserAction("<LOC>Ban", UserActionCommands.Ban, new string[] { Loc.Get("<LOC>/ban") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction ClanDemote = new UserAction("<LOC>Demote", UserActionCommands.ClanDemote, new string[] { Loc.Get("<LOC>/demote") }, new UserActionArgument[] { new UserActionArgument("<LOC>Clan Member Name") });
        public static readonly UserAction ClanInvite = new UserAction("<LOC>Invite to Clan", UserActionCommands.ClanInvite, new string[] { Loc.Get("<LOC>/claninvite"), Loc.Get("<LOC>/cinvite") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction ClanPromote = new UserAction("<LOC>Promote", UserActionCommands.ClanPromote, new string[] { Loc.Get("<LOC>/promote") }, new UserActionArgument[] { new UserActionArgument("<LOC>Clan Member Name") });
        public static readonly UserAction ClanRemove = new UserAction("<LOC>Clan Kick", UserActionCommands.ClanRemove, new string[] { Loc.Get("<LOC>/clankick"), Loc.Get("<LOC>/clanremove") }, new UserActionArgument[] { new UserActionArgument("<LOC>Clan Member Name") });
        public static readonly UserAction ClanRequest = new UserAction("<LOC>Request Clan Invitation", UserActionCommands.ClanRequest, new string[] { Loc.Get("<LOC>/clanrequest"), Loc.Get("<LOC>/clanreq"), Loc.Get("<LOC>/creq") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction CreateClan = new UserAction("<LOC>Create Clan", UserActionCommands.CreateClan, new string[] { Loc.Get("<LOC>/createclan"), Loc.Get("<LOC>/formclan"), Loc.Get("<LOC>/newclan") }, new UserActionArgument[0]);
        public static readonly UserAction DoNotDisturb = new UserAction("<LOC>Do Not Disturb", UserActionCommands.DoNotDisturb, new string[] { Loc.Get("<LOC>/dnd") }, new UserActionArgument[0]);
        public static readonly UserAction Emote = new UserAction("<LOC>Speak in 3rd person", UserActionCommands.Emote, new string[] { Loc.Get("<LOC>/me"), Loc.Get("<LOC>/em"), Loc.Get("<LOC>/yell") }, new UserActionArgument[0]);
        public static readonly UserAction Exit = new UserAction("<LOC>Exit Program", UserActionCommands.Exit, new string[] { Loc.Get("<LOC>/exit"), Loc.Get("<LOC>/quit") }, new UserActionArgument[0]);
        public static readonly UserAction Friend = new UserAction("<LOC>Friend", UserActionCommands.Friend, new string[] { Loc.Get("<LOC>/friend"), Loc.Get("<LOC>/finvite") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction Ignore = new UserAction("<LOC>Ignore", UserActionCommands.Ignore, new string[] { Loc.Get("<LOC>/ignore") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction IPBan = AdminAction("***ADMIN ONLY*** Issue an IP Ban.  This must be an exact address.  Ex: /ipban 125.32.65.23", UserActionCommands.IPBan, new string[] { Loc.Get("<LOC>/ipban") }, new UserActionArgument[0]);
        public static readonly UserAction IPBanCheck = AdminAction("***ADMIN ONLY*** This will give a list of all the banned IP Addresses.", UserActionCommands.IPBanCheck, new string[] { Loc.Get("<LOC>/ipcheck") }, new UserActionArgument[0]);
        public static readonly UserAction IPLookup = AdminAction("***ADMIN ONLY*** Looks up a users IP Address, and all accounts associated with that IP Address.  Ex: /iplookup IN-Agent911", UserActionCommands.IPLookup, new string[] { Loc.Get("<LOC>/iplookup") }, new UserActionArgument[0]);
        public static readonly UserAction IPUnBan = AdminAction("***ADMIN ONLY*** Removes an IP Ban.  This must be an exact address.  Ex: /ipunban 125.32.65.23", UserActionCommands.IPUnBan, new string[] { Loc.Get("<LOC>/ipunban") }, new UserActionArgument[0]);
        public static readonly UserAction JoinChat = new UserAction("<LOC>Chatroom", UserActionCommands.Chatroom, new string[] { Loc.Get("<LOC>/join"), Loc.Get("<LOC>/chat"), Loc.Get("/channel") }, new UserActionArgument[] { new UserActionArgument("<LOC>Room Name", false) });
        public static readonly UserAction Kick = new UserAction("<LOC>Kick", UserActionCommands.Kick, new string[] { Loc.Get("<LOC>/kick") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction LeaveClan = new UserAction("<LOC>Leave Clan", UserActionCommands.LeaveClan, new string[] { Loc.Get("<LOC>/leaveclan"), Loc.Get("<LOC>/clanleave") }, new UserActionArgument[0]);
        public static readonly UserAction Locate = new UserAction("<LOC>Locate Player", UserActionCommands.Locate, new string[] { Loc.Get("<LOC>/locate"), Loc.Get("<LOC>/where") }, new UserActionArgument[] { new UserActionArgument(Loc.Get("<LOC>Player"), true) });
        private bool mAdminCommand;
        private static UserAction[] mAllActions = null;
        private UserActionArgument[] mArgs;
        private UserActionCommands mCommand;
        private string[] mCommandWords;
        private string mDescription;
        public static readonly UserAction Stats = AdminAction("***ADMIN ONLY*** Get Call Statistics that you made to Quazal.", UserActionCommands.Stats, new string[] { Loc.Get("<LOC>/stats") }, new UserActionArgument[0]);
        public static readonly UserAction TagTranslate = AdminAction("***ADMIN ONLY*** Tag a user for auto translation.  Ex: /tag IN-Agent911 de", UserActionCommands.TagTranslate, new string[] { Loc.Get("<LOC>/tag") }, new UserActionArgument[0]);
        public static readonly UserAction ToggleDirectChallenge = new UserAction("<LOC>Toggle Direct Challenges", UserActionCommands.IgnoreDirectChallenge, new string[] { Loc.Get("<LOC>/challenges") }, new UserActionArgument[0]);
        public static readonly UserAction Translate = AdminAction("***ADMIN ONLY*** Machine translate message.  Ex: /translate de Hello German Friends!", UserActionCommands.Translate, new string[] { Loc.Get("<LOC>/translate") }, new UserActionArgument[0]);
        public static readonly UserAction Unban = new UserAction("<LOC>Unban", UserActionCommands.Unban, new string[] { Loc.Get("<LOC>/unban") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction Unignore = new UserAction("<LOC>Unignore", UserActionCommands.Unignore, new string[] { Loc.Get("<LOC>/unignore") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction Unrfriend = new UserAction("<LOC>Remove Friend", UserActionCommands.Unfriend, new string[] { Loc.Get("<LOC>/unfriend"), Loc.Get("<LOC>/removefriend") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target Name") });
        public static readonly UserAction ViewClan = new UserAction("<LOC>View Clan", UserActionCommands.ViewClan, new string[] { Loc.Get("<LOC>/viewclan"), Loc.Get("<LOC>/clanview"), Loc.Get("<LOC>/clan") }, new UserActionArgument[] { new UserActionArgument("<LOC>Clan Member Name") });
        public static readonly UserAction ViewPlayer = new UserAction("<LOC>View Player", UserActionCommands.ViewPlayer, new string[] { Loc.Get("<LOC>/viewplayer"), Loc.Get("<LOC>/playerview"), Loc.Get("<LOC>/player"), Loc.Get("<LOC>/who"), Loc.Get("<LOC>/whois") }, new UserActionArgument[] { new UserActionArgument("<LOC>Player Name") });
        public static readonly UserAction Whisper = new UserAction("<LOC>Private Message", UserActionCommands.Whisper, new string[] { Loc.Get("<LOC>/pm"), Loc.Get("<LOC>/message") }, new UserActionArgument[] { new UserActionArgument("<LOC>Target"), new UserActionArgument("<LOC>Message", false) });

        public UserAction(string description, UserActionCommands command, string[] cmdWords, params UserActionArgument[] args)
        {
            this.mDescription = Loc.Get(description);
            this.mCommand = command;
            this.mCommandWords = cmdWords;
            this.mArgs = args;
            this.mAdminCommand = false;
        }

        public static UserAction AdminAction(string description, UserActionCommands command, string[] cmdWords, params UserActionArgument[] args)
        {
            UserAction action = new UserAction(description, command, cmdWords, args);
            action.mAdminCommand = true;
            return action;
        }

        public bool CheckFormat(string[] args)
        {
            if ((args == null) || (args.Length <= 0))
            {
                if ((this.Args == null) || (this.Args.Length == 0))
                {
                    return true;
                }
                foreach (UserActionArgument argument in this.Args)
                {
                    if (argument.Required)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static UserAction[] GetFilteredActions(string filter)
        {
            filter = filter.ToLower();
            if (filter.Split(new char[] { ' ' }).Length > 1)
            {
                string str = filter.Split(new char[] { ' ' })[0];
                List<UserAction> list = new List<UserAction>();
                foreach (UserAction action in AllActions)
                {
                    foreach (string str2 in action.CommandWords)
                    {
                        if (((str == str2) && (!action.AdminCommand || User.Current.IsAdmin)) && (ConfigSettings.GetString("SlashIgnore", "chicken canada canadian ").IndexOf(action.Description.ToLower() + " ") < 0))
                        {
                            list.Add(action);
                            break;
                        }
                    }
                }
                return list.ToArray();
            }
            SortedList<string, UserAction> list2 = new SortedList<string, UserAction>();
            foreach (UserAction action in AllActions)
            {
                foreach (string str2 in action.CommandWords)
                {
                    if ((str2.StartsWith(filter) && (!action.AdminCommand || User.Current.IsAdmin)) && (ConfigSettings.GetString("SlashIgnore", "chicken canada canadian  ").IndexOf(action.Description.ToLower() + " ") < 0))
                    {
                        list2[action.Description] = action;
                        break;
                    }
                }
            }
            UserAction[] array = new UserAction[list2.Count];
            list2.Values.CopyTo(array, 0);
            Array.Reverse(array);
            return array;
        }

        public override string ToString()
        {
            string str = this.Description + " (";
            foreach (string str2 in this.CommandWords)
            {
                str = str + string.Format("{0} ", str2);
            }
            str = str.TrimEnd(new char[] { ' ' }) + ")";
            if (this.Args != null)
            {
                for (int i = 0; i < this.Args.Length; i++)
                {
                    if (this.Args[i].Required)
                    {
                        str = str + string.Format(" [{0}]", this.Args[i].Name);
                    }
                    else
                    {
                        str = str + string.Format(" <{0}>", this.Args[i].Name);
                    }
                }
            }
            return str;
        }

        public static bool TryGetByName(string commandName, out UserAction action)
        {
            for (int i = 0; i < AllActions.Length; i++)
            {
                if (Array.IndexOf<string>(AllActions[i].CommandWords, commandName.ToLower()) >= 0)
                {
                    action = AllActions[i];
                    return true;
                }
            }
            action = null;
            return false;
        }

        public static bool TryGetByPartialName(string commandName, out UserAction action)
        {
            UserAction[] filteredActions = GetFilteredActions(commandName);
            if (filteredActions.Length > 0)
            {
                action = filteredActions[0];
                return true;
            }
            action = null;
            return false;
        }

        public bool AdminCommand
        {
            get
            {
                return this.mAdminCommand;
            }
            set
            {
                this.mAdminCommand = value;
            }
        }

        public static UserAction[] AllActions
        {
            get
            {
                if (mAllActions == null)
                {
                    FieldInfo[] fields = typeof(UserAction).GetFields(BindingFlags.Public | BindingFlags.Static);
                    mAllActions = new UserAction[fields.Length];
                    int index = 0;
                    foreach (FieldInfo info in fields)
                    {
                        mAllActions[index] = fields[index].GetValue(null) as UserAction;
                        index++;
                    }
                }
                return mAllActions;
            }
        }

        public UserActionArgument[] Args
        {
            get
            {
                return this.mArgs;
            }
        }

        public UserActionCommands Command
        {
            get
            {
                return this.mCommand;
            }
        }

        public string[] CommandWords
        {
            get
            {
                return this.mCommandWords;
            }
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }

        public string Text
        {
            get
            {
                return this.ToString();
            }
        }

        public object Value
        {
            get
            {
                return this.CommandWords[0];
            }
        }
    }
}

