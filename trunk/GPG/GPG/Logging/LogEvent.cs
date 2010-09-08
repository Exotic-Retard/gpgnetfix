namespace GPG.Logging
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void LogEvent(string message, string classification, object data);
}

