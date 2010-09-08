namespace GPG.Logging
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IStatusProvider
    {
        event StatusProviderEventHandler StatusChanged;

        void SetStatus(string status, params object[] args);
        void SetStatus(string status, int timeout, params object[] args);
    }
}

