namespace GPG.Multiplayer.Game.Network
{
    using System;

    public enum NetMessageTypes
    {
        Nat,
        GPGText,
        GPGBinary,
        GPGAck,
        GameCommand,
        Unknown
    }
}

