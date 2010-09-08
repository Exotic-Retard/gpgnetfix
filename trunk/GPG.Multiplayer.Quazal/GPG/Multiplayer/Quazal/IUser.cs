namespace GPG.Multiplayer.Quazal
{
    using System;

    public interface IUser
    {
        int ID { get; }

        bool IsClanMate { get; }

        bool IsCurrent { get; }

        bool IsDND { get; }

        bool IsIgnored { get; }

        bool IsInClan { get; }

        bool IsSystem { get; }

        string Name { get; }

        bool Online { get; set; }
    }
}

