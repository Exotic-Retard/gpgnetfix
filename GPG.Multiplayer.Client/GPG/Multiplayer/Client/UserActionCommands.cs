namespace GPG.Multiplayer.Client
{
    using System;

    public enum UserActionCommands
    {
        AllowFriends = 30,
        Away = 2,
        Ban = 7,
        Canadian = 0x13,
        Chatroom = 8,
        Chicken = 0x12,
        ClanDemote = 0x10,
        ClanInvite = 10,
        ClanPromote = 15,
        ClanRemove = 0x11,
        ClanRequest = 11,
        CreateClan = 13,
        DoNotDisturb = 1,
        Emote = 0x18,
        Exit = 20,
        Friend = 3,
        Ignore = 0x16,
        IgnoreDirectChallenge = 0x1d,
        IPBan = 0x1f,
        IPBanCheck = 0x21,
        IPLookup = 0x20,
        IPUnBan = 0x22,
        Kick = 6,
        LeaveClan = 12,
        Locate = 0x1b,
        Stats = 0x19,
        TagTranslate = 0x1c,
        Translate = 0x1a,
        Unban = 0x15,
        Unfriend = 4,
        Unignore = 0x17,
        ViewClan = 14,
        ViewPlayer = 9,
        Whisper = 5
    }
}

