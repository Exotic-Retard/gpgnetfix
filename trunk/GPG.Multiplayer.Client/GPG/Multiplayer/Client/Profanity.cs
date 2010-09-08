namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class Profanity
    {
        private static string[] mProfanityFilter = null;
        private static char[] ProfanityMaskChars = new char[] { '!', '@', '#', '*', '&', '?' };

        public static bool ContainsProfanity(string text)
        {
            text = text.ToLower();
            for (int i = 0; i < ProfanityFilter.Length; i++)
            {
                string str = ProfanityFilter[i].ToLower();
                if (str.StartsWith("[literal]"))
                {
                    str = str.Replace("[literal]", "");
                    if (text.IndexOf(str) >= 0)
                    {
                        return true;
                    }
                }
                else
                {
                    Regex regex = new Regex(str.ToLower());
                    if (regex.IsMatch(text))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static string GetProfanityMask(int length)
        {
            string str = "";
            for (int i = 0; i < length; i++)
            {
                str = str + '*';
            }
            return str;
        }

        private static string GetProfanityMask(string filter)
        {
            return GetProfanityMask(filter.Length);
        }

        public static string MaskProfanity(string text)
        {
            try
            {
                if (Program.Settings.Chat.ProfanityFilter)
                {
                    string input = text.ToLower();
                    for (int i = 0; i < ProfanityFilter.Length; i++)
                    {
                        string str2 = ProfanityFilter[i].ToLower();
                        if (str2 != null)
                        {
                            if (str2.StartsWith("[literal]"))
                            {
                                str2 = str2.Replace("[literal]", "");
                                if ((str2.Length > 0) && (str2.Length <= text.Length))
                                {
                                    for (int j = input.IndexOf(str2); j >= 0; j = input.IndexOf(str2, (int) (j + str2.Length)))
                                    {
                                        text = text.Remove(j, str2.Length);
                                        text = text.Insert(j, GetProfanityMask(str2));
                                    }
                                }
                            }
                            else if (str2.Length > 0)
                            {
                                MatchCollection matchs = new Regex(str2, RegexOptions.IgnoreCase).Matches(input);
                                foreach (Match match in matchs)
                                {
                                    text = text.Remove(match.Index, match.Length);
                                    text = text.Insert(match.Index, GetProfanityMask(match.Length));
                                }
                            }
                        }
                    }
                }
                return text;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return text;
            }
        }

        public static string[] ProfanityFilter
        {
            get
            {
                if (mProfanityFilter == null)
                {
                    mProfanityFilter = Encoding.ASCII.GetString((byte[]) ProfanityResources.ResourceManager.GetObject("profanity")).Replace("\r", "").Split("\n".ToCharArray());
                }
                return mProfanityFilter;
            }
        }
    }
}

