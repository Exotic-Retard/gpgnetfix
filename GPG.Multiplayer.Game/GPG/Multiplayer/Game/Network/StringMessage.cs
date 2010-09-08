namespace GPG.Multiplayer.Game.Network
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void StringMessage(string type, string srcaddr, int srcport, string destaddr, int destport, string message);
}

