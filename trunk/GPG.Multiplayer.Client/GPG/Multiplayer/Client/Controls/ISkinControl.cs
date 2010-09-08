namespace GPG.Multiplayer.Client.Controls
{
    using GPG.UI.Controls;
    using System;

    public interface ISkinControl : IStyledControl
    {
        string SkinBasePath { get; set; }

        bool SkinLoaded { get; }

        bool UseDefaultStyle { get; }
    }
}

