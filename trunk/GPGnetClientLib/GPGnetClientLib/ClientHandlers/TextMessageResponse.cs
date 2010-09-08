namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void TextMessageResponse(string roomname, string message, Credentials sourceuser);
}

