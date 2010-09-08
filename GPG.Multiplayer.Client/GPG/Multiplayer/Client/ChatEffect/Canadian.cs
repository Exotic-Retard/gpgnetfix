namespace GPG.Multiplayer.Client.ChatEffect
{
    using GPG;
    using GPG.Multiplayer.Client;
    using System;
    using System.Drawing;

    public class Canadian : ChatEffectBase
    {
        private string[] mRandomText = new string[] { Loc.Get("Celine Dion is great"), Loc.Get("Wayne Gretzky is great"), Loc.Get("How about those Canucks") };

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
            if (new Random(Environment.TickCount).Next(0x1388) < 0x3e8)
            {
            }
            text = text.Replace("ou", "oo");
            text = text.TrimEnd(new char[] { ' ', '.', '?', '!' });
            text = text + " eh?";
            return text;
        }

        public override string Name
        {
            get
            {
                return Loc.Get("Canadian");
            }
        }

        public string[] RandomText
        {
            get
            {
                return this.mRandomText;
            }
        }

        public override UserStatus Status
        {
            get
            {
                return UserStatus.Canadian;
            }
        }
    }
}

