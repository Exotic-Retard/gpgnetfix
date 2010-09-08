namespace GPG.Network
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IPingMonitor
    {
        event EventHandler PingChanged;

        int BestPing { get; }

        int PingTime { get; }

        int WorstPing { get; }
    }
}

