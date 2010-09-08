namespace GPG.Multiplayer.Plugin
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void GetMessage(string sendername, string command, params object[] args);
}

