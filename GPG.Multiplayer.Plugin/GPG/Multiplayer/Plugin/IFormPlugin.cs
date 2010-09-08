namespace GPG.Multiplayer.Plugin
{
    using System;

    public interface IFormPlugin
    {
        void RegisterDataHandler(IPersistentData handler);
        void SetUserInformation(string username, int userid);

        string Author { get; }

        string FormTitle { get; }

        PluginLocation Location { get; }

        string MenuCaption { get; }

        int Version { get; }
    }
}

