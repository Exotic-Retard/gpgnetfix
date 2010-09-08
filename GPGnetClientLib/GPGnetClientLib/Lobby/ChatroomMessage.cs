namespace GPGnetClientLib.Lobby
{
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void ChatroomMessage(Credentials sourceuser, string roomname, string message);
}

