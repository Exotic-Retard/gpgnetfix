namespace GPG.UI.Controls.Skinning
{
    using System;

    public interface ISkinResourceConsumer
    {
        void RefreshSkin();

        SkinManager Manager { get; set; }

        string ResourceKey { get; set; }
    }
}

