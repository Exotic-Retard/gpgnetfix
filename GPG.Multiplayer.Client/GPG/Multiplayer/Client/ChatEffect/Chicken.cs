namespace GPG.Multiplayer.Client.ChatEffect
{
    using GPG;
    using GPG.Multiplayer.Client;
    using System;
    using System.Drawing;

    public class Chicken : ChatEffectBase
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
            string[] strArray = text.Split(new char[] { ' ' });
            string str = "";
            foreach (string str2 in strArray)
            {
                if (str2.Length < 4)
                {
                    str = str + "bawk ";
                }
                else
                {
                    str = str + "b";
                    for (int i = 0; i < (str2.Length - 3); i++)
                    {
                        str = str + "a";
                    }
                    str = str + "wk ";
                }
            }
            str.TrimEnd(new char[] { ' ' });
            return (str + "!");
        }

        public override string Name
        {
            get
            {
                return Loc.Get("<LOC>Chicken");
            }
        }

        public override UserStatus Status
        {
            get
            {
                return UserStatus.Chicken;
            }
        }
    }
}

