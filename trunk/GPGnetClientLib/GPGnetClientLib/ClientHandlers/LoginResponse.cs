namespace GPGnetClientLib.ClientHandlers
{
    using GPGnetCommunicationsLib;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void LoginResponse(bool loggedin, Credentials credentials);
}

