namespace GPG
{
    using System;

    [Flags]
    public enum SoundFlags
    {
        SND_ALIAS = 0x10000,
        SND_ALIAS_ID = 0x110000,
        SND_ASYNC = 1,
        SND_FILENAME = 0x20000,
        SND_LOOP = 8,
        SND_MEMORY = 4,
        SND_NODEFAULT = 2,
        SND_NOSTOP = 0x10,
        SND_NOWAIT = 0x2000,
        SND_PURGE = 0x40,
        SND_RESOURCE = 0x40004,
        SND_SYNC = 0
    }
}

