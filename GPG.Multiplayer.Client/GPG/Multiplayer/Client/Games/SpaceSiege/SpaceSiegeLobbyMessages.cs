namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using System;

    public enum SpaceSiegeLobbyMessages : uint
    {
        Abort = 11,
        AcceptInvite = 0x12,
        AcceptJoin = 4,
        ChangePosition = 20,
        ChatMessage = 1,
        ClosePosition = 0x16,
        DeclineInvite = 0x13,
        DeclineJoin = 5,
        Disband = 7,
        End = 0x10,
        Full = 10,
        Game = 0x11,
        Invite = 9,
        Kick = 6,
        Launch = 15,
        Leave = 8,
        OpenPosition = 0x15,
        ParticipantInfo = 13,
        RequestJoin = 3,
        Start = 14,
        Status = 2,
        Unavailable = 12
    }
}

