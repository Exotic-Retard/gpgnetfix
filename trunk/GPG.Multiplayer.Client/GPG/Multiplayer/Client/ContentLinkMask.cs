namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.UI;
    using System;
    using System.Drawing;

    public class ContentLinkMask : ITextEffect
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
                int index = text.IndexOf('\x0003');
                return text.Substring(index + 1);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return text;
            }
        }

        public static string FormatLink(IAdditionalContent content)
        {
            return string.Format("content:\"{0}{1}{2}\"", content.ID, '\x0003', content.Name);
        }

        public static int GetContentID(ChatLink link)
        {
            try
            {
                int index = link.Url.IndexOf('\x0003');
                if (index < 0)
                {
                    return int.Parse(link.Url);
                }
                return int.Parse(link.Url.Substring(0, index));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return -1;
            }
        }
    }
}

