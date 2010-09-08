namespace GPG.Multiplayer.Client.Vaulting
{
    using System;

    [Flags]
    internal enum ContentVaultTabs : byte
    {
        Activity = 8,
        Download = 2,
        MyContent = 1,
        None = 0,
        Upload = 4
    }
}

