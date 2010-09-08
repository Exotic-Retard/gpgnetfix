namespace GPG.Multiplayer.Client.ChatEffect
{
    using GPG.Multiplayer.Client;
    using GPG.UI;
    using System;
    using System.Drawing;

    public abstract class ChatEffectBase : ITextEffect
    {
        protected ChatEffectBase()
        {
        }

        public abstract Color ChangeColor(Color color);
        public abstract Font ChangeFont(Font font);
        public abstract string ChangeText(string text);

        public abstract string Name { get; }

        public abstract UserStatus Status { get; }
    }
}

