namespace GPG.Network
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IProgressMonitor
    {
        event EventHandler Finished;

        event ProgressChangeEventHandler ProgressChanged;
    }
}

