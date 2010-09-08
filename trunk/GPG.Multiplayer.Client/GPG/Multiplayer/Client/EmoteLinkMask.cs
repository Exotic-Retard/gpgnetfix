namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using GPG.UI;
    using System;
    using System.Drawing;

    public class EmoteLinkMask : ITextEffect
    {
        public const char SEPARATOR = '\x0003';

        public Color ChangeColor(Color color)
        {
            return color;
        }

        public Font ChangeFont(Font font)
        {
            return font;
        }

        public string ChangeText(string text)
        {
            try
            {
                char ch = '\x0003';
                if (text.Contains(ch.ToString()))
                {
                    string[] strArray = text.Split(new char[] { '\x0003' }, 2);
                    string str = strArray[0];
                    return strArray[1];
                }
                return text;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return text;
            }
        }

        public static string GetCharSequence(ChatLink link)
        {
            try
            {
                char ch = '\x0003';
                if (link.Url.Contains(ch.ToString()))
                {
                    return link.Url.Split(new char[] { '\x0003' })[1];
                }
                return null;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }
    }
}

