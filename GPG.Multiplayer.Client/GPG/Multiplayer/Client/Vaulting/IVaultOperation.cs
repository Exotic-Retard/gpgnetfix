namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG.Logging;
    using GPG.Network;
    using System;
    using System.Runtime.CompilerServices;

    public interface IVaultOperation : IActivityMonitor, IProgressMonitor, IStatusProvider, IDisposable
    {
        event ContentOperationCallback OperationFinished;

        ActivityMonitor CreateActivityMonitor();
        void Resume();
        void Start();
        void Stop();

        bool IsOperationFinished { get; }

        bool IsProgressFinished { get; }

        string LastStatus { get; }
    }
}

