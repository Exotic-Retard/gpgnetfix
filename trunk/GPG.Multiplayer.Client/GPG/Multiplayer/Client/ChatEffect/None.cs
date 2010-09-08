namespace GPG.Multiplayer.Client.ChatEffect
{
    using GPG;
    using GPG.Multiplayer.Client;
    using System;
    using System.Drawing;

    public class None : ChatEffectBase
    {
        public override Color ChangeColor(Color color)
        {
            return color;
        }

        public override Font ChangeFont(Font font)
        {
            return font;
        }

        public override string ChangeText(string text)
        {
            return text;
        }

        public override string Name
        {
            get
            {
                return Loc.Get("<LOC>None");
            }
        }

        public override UserStatus Status
        {
            get
            {
                return UserStatus.None;
            }
        }
    }
}

