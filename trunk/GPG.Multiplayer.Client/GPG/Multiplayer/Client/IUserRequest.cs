namespace GPG.Multiplayer.Client
{
    using System;

    public interface IUserRequest
    {
        string Description { get; }

        int RecieverID { get; }

        string RecieverName { get; }

        DateTime RequestDate { get; }

        int RequestorID { get; }

        string RequestorName { get; }
    }
}

