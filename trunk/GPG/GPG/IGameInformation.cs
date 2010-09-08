namespace GPG
{
    using System;
    using System.Drawing;

    public interface IGameInformation
    {
        string ToString();

        string ApplicationDirectory { get; set; }

        string CDKey { get; set; }

        string CommandLineArgs { get; set; }

        int CommunicationsMethod { get; set; }

        string CurrentVersion { get; set; }

        string ExeName { get; set; }

        string ForcedCommandLineArgs { get; set; }

        string GameDescription { get; set; }

        Image GameIcon { get; }

        int GameID { get; set; }

        string GameLocation { get; set; }

        bool HasGame { get; }

        int Port { get; set; }

        string UserForcedCommandLineArgs { get; set; }

        string VersionFile { get; set; }
    }
}

