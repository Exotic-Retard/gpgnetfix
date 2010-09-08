namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.ComponentModel;

    public class ChatLink
    {
        public static readonly ChatLink Chat = new ChatLink("chat:", false, Program.Settings.Chat.Links.ChatColor, Program.Settings.Chat.Links.LinkFont);
        public static readonly ChatLink Clan = new ChatLink("clan:", false, Program.Settings.Chat.Links.ClanColor, Program.Settings.Chat.Links.LinkFont);
        public static readonly ChatLink Content = new ChatLink("content:", false, Program.Settings.Chat.Links.ContentColor, Program.Settings.Chat.Links.LinkFont, new ContentLinkMask());
        public static readonly ChatLink Email = new ChatLink(@"^([\w]+@([\w]+\.)+[a-zA-Z]{2,9}(\s*;\s*[\w]+@([\w]+\.)+[a-zA-Z]{2,9})*)$", true, Program.Settings.Chat.Links.EmailColor, Program.Settings.Chat.Links.LinkFont);
        public static readonly ChatLink Emote = new ChatLink("emote:", false, Program.Settings.Chat.Links.EmoteColor, Program.Settings.Chat.Links.LinkFont, new EmoteLinkMask());
        public static readonly ChatLink Game = new ChatLink("game:", false, Program.Settings.Chat.Links.GameColor, Program.Settings.Chat.Links.LinkFont);
        public static readonly ChatLink Help = new ChatLink("help:", false, Program.Settings.Chat.Links.HelpColor, Program.Settings.Chat.Links.LinkFont);
        public static readonly ChatLink LiveReplay = new ChatLink("livereplay:", false, Program.Settings.Chat.Links.ReplayColor, Program.Settings.Chat.Links.LinkFont);
        private static Dictionary<string, ChatLink> mAll = null;
        private ITextEffect mEffect;
        private int mEndIndex;
        private bool mIsRegEx;
        private Color mLinkColor;
        private Font mLinkFont;
        private string mLinkWord;
        private bool mShowLinkUrl;
        private int mStartIndex;
        private string mUrl;
        public static readonly ChatLink Player = new ChatLink("player:", false, Program.Settings.Chat.Links.PlayerColor, Program.Settings.Chat.Links.LinkFont);
        private ChatLink Prototype;
        public static readonly ChatLink Replay = new ChatLink("replay:", false, Program.Settings.Chat.Links.ReplayColor, Program.Settings.Chat.Links.LinkFont);
        public static readonly ChatLink Solution = new ChatLink("solution:", false, Program.Settings.Chat.Links.SolutionColor, Program.Settings.Chat.Links.LinkFont);
        public static readonly ChatLink Tournament = new ChatLink("tournament:", false, Program.Settings.Chat.Links.GameColor, Program.Settings.Chat.Links.LinkFont, new TournamentLinkMask());
        public static readonly ChatLink Web = new ChatLink("(?<http>(http:[/][/]|www.|ftp:[/][/]|https:[/][/])([a-z]|[A-Z]|[0-9]|[/.]|[~]|[%]|[?]|[=]|[:]|[_]|[-]|[&])*)", true, Program.Settings.Chat.Links.WebColor, Program.Settings.Chat.Links.LinkFont);
        private char WhiteSpaceChar;

        public event ChatLinkEventHandler Click;

        static ChatLink()
        {
            Program.Settings.Chat.Links.ChatColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Chat.LinkColor = Program.Settings.Chat.Links.ChatColor;
            };
            Program.Settings.Chat.Links.ClanColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Clan.LinkColor = Program.Settings.Chat.Links.ClanColor;
            };
            Program.Settings.Chat.Links.EmailColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Email.LinkColor = Program.Settings.Chat.Links.EmailColor;
            };
            Program.Settings.Chat.Links.EmoteColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Emote.LinkColor = Program.Settings.Chat.Links.EmoteColor;
            };
            Program.Settings.Chat.Links.GameColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Game.LinkColor = Program.Settings.Chat.Links.GameColor;
            };
            Program.Settings.Chat.Links.PlayerColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Player.LinkColor = Program.Settings.Chat.Links.PlayerColor;
            };
            Program.Settings.Chat.Links.WebColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Web.LinkColor = Program.Settings.Chat.Links.WebColor;
            };
            Program.Settings.Chat.Links.ContentColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Content.LinkColor = Program.Settings.Chat.Links.ContentColor;
            };
            Program.Settings.Chat.Links.HelpColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Help.LinkColor = Program.Settings.Chat.Links.HelpColor;
            };
            Program.Settings.Chat.Links.SolutionColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Solution.LinkColor = Program.Settings.Chat.Links.SolutionColor;
            };
            Program.Settings.Chat.Links.ReplayColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                Replay.LinkColor = Program.Settings.Chat.Links.ReplayColor;
                LiveReplay.LinkColor = Program.Settings.Chat.Links.ReplayColor;
            };
            Program.Settings.Chat.Links.LinkFontChanged += delegate (object s, PropertyChangedEventArgs e) {
                Chat.LinkFont = Program.Settings.Chat.Links.LinkFont;
                Clan.LinkFont = Program.Settings.Chat.Links.LinkFont;
                Email.LinkFont = Program.Settings.Chat.Links.LinkFont;
                Emote.LinkFont = Program.Settings.Chat.Links.LinkFont;
                Game.LinkFont = Program.Settings.Chat.Links.LinkFont;
                Player.LinkFont = Program.Settings.Chat.Links.LinkFont;
                Web.LinkFont = Program.Settings.Chat.Links.LinkFont;
            };
        }

        internal ChatLink(ChatLink prototype)
        {
            this.Prototype = null;
            this.mLinkWord = null;
            this.mIsRegEx = false;
            this.mLinkColor = new Color();
            this.mLinkFont = null;
            this.mEffect = null;
            this.WhiteSpaceChar = '\0';
            this.mShowLinkUrl = true;
            this.mUrl = null;
            this.mStartIndex = -1;
            this.mEndIndex = -1;
            this.mLinkWord = prototype.LinkWord;
            this.mIsRegEx = prototype.IsRegEx;
            this.mLinkColor = prototype.mLinkColor;
            this.mLinkFont = prototype.LinkFont;
            this.mEffect = prototype.Effect;
            this.Prototype = prototype;
        }

        public ChatLink(string word, bool isRegEx)
        {
            this.Prototype = null;
            this.mLinkWord = null;
            this.mIsRegEx = false;
            this.mLinkColor = new Color();
            this.mLinkFont = null;
            this.mEffect = null;
            this.WhiteSpaceChar = '\0';
            this.mShowLinkUrl = true;
            this.mUrl = null;
            this.mStartIndex = -1;
            this.mEndIndex = -1;
            this.mLinkWord = word;
            this.mIsRegEx = isRegEx;
        }

        public ChatLink(string word, bool isRegEx, Color color, Font font)
        {
            this.Prototype = null;
            this.mLinkWord = null;
            this.mIsRegEx = false;
            this.mLinkColor = new Color();
            this.mLinkFont = null;
            this.mEffect = null;
            this.WhiteSpaceChar = '\0';
            this.mShowLinkUrl = true;
            this.mUrl = null;
            this.mStartIndex = -1;
            this.mEndIndex = -1;
            this.mLinkWord = word;
            this.mIsRegEx = isRegEx;
            this.mLinkColor = color;
            this.mLinkFont = font;
        }

        public ChatLink(string word, bool isRegEx, Color color, Font font, ITextEffect effect)
        {
            this.Prototype = null;
            this.mLinkWord = null;
            this.mIsRegEx = false;
            this.mLinkColor = new Color();
            this.mLinkFont = null;
            this.mEffect = null;
            this.WhiteSpaceChar = '\0';
            this.mShowLinkUrl = true;
            this.mUrl = null;
            this.mStartIndex = -1;
            this.mEndIndex = -1;
            this.mLinkWord = word;
            this.mIsRegEx = isRegEx;
            this.mLinkColor = color;
            this.mLinkFont = font;
            this.mEffect = effect;
        }

        public static Dictionary<int, ChatLink> CreateCharacterIndex(ChatLink[] links)
        {
            Dictionary<int, ChatLink> dictionary = new Dictionary<int, ChatLink>(links.Length);
            for (int i = 0; i < links.Length; i++)
            {
                dictionary.Add(links[i].StartIndex, links[i]);
            }
            return dictionary;
        }

        public static ChatLink[] FindLinks(string text)
        {
            return FindLinks(text, null);
        }

        public static ChatLink[] FindLinks(string text, ChatLink linkType)
        {
            if (text == null)
            {
                return new ChatLink[0];
            }
            List<ChatLink> list = new List<ChatLink>();
            foreach (ChatLink link in All.Values)
            {
                if ((linkType == null) || link.Equals(linkType))
                {
                    if (link.IsRegEx)
                    {
                        list.AddRange(FindLinksRegEx(text, link));
                    }
                    else
                    {
                        list.AddRange(FindLinksWord(text, link));
                    }
                }
            }
            return list.ToArray();
        }

        private static ChatLink[] FindLinksRegEx(string text, ChatLink link)
        {
            List<ChatLink> list = new List<ChatLink>();
            try
            {
                MatchCollection matchs = new Regex(link.LinkWord).Matches(text);
                foreach (Match match in matchs)
                {
                    if (match.Success)
                    {
                        ChatLink item = new ChatLink(link);
                        item.StartIndex = match.Index;
                        item.EndIndex = item.StartIndex + match.Length;
                        item.Url = match.Value;
                        list.Add(item);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            return list.ToArray();
        }

        private static ChatLink[] FindLinksWord(string text, ChatLink link)
        {
            List<ChatLink> list = new List<ChatLink>();
            try
            {
                int index = text.IndexOf(link.LinkWord);
                while (index >= 0)
                {
                    ChatLink item = new ChatLink(link);
                    int startIndex = index + link.LinkWord.Length;
                    int length = -1;
                    if (text.Length > startIndex)
                    {
                        string str;
                        if (text[startIndex] == '"')
                        {
                            item.WhiteSpaceChar = '"';
                            length = text.IndexOf('"', startIndex + 1);
                            str = text.Substring(startIndex + 1, length - (startIndex + 1));
                        }
                        else if (text[startIndex] == '\'')
                        {
                            item.WhiteSpaceChar = '\'';
                            length = text.IndexOf('\'', startIndex + 1);
                            str = text.Substring(startIndex + 1, length - (startIndex + 1));
                        }
                        else
                        {
                            length = text.IndexOf(' ', startIndex + 1);
                            if (length < 0)
                            {
                                length = text.Length;
                            }
                            str = text.Substring(startIndex, length - startIndex);
                        }
                        item.Url = str;
                        item.StartIndex = startIndex - link.LinkWord.Length;
                        item.EndIndex = length;
                        list.Add(item);
                        index = text.IndexOf(link.LinkWord, length);
                    }
                    else
                    {
                        index = -1;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            return list.ToArray();
        }

        public string GetFullUrl(string url)
        {
            return string.Format("{0}{1}", this.LinkWord, url);
        }

        public void OnClick()
        {
            if (this.Prototype != null)
            {
                this.Prototype.OnClick(this);
            }
        }

        private void OnClick(ChatLink link)
        {
            if (this.Click != null)
            {
                this.Click(link, link.Url);
            }
        }

        public static Dictionary<string, ChatLink> All
        {
            get
            {
                if (mAll == null)
                {
                    FieldInfo[] fields = typeof(ChatLink).GetFields(BindingFlags.Public | BindingFlags.Static);
                    mAll = new Dictionary<string, ChatLink>(fields.Length);
                    foreach (FieldInfo info in fields)
                    {
                        ChatLink link = info.GetValue(null) as ChatLink;
                        if (link != null)
                        {
                            mAll.Add(link.LinkWord, link);
                        }
                    }
                }
                return mAll;
            }
        }

        public string DisplayText
        {
            get
            {
                if (this.Effect == null)
                {
                    return this.Url.TrimStart("\"'".ToCharArray()).TrimEnd("\"'".ToCharArray());
                }
                return this.Effect.ChangeText(this.Url);
            }
        }

        public ITextEffect Effect
        {
            get
            {
                return this.mEffect;
            }
        }

        public int EndIndex
        {
            get
            {
                return this.mEndIndex;
            }
            set
            {
                this.mEndIndex = value;
            }
        }

        public string FullUrl
        {
            get
            {
                if (this.WhiteSpaceChar == '\0')
                {
                    return string.Format("{0}{1}", this.LinkWord, this.Url);
                }
                return string.Format("{0}{1}{2}{1}", this.LinkWord, this.WhiteSpaceChar, this.Url);
            }
        }

        public bool IsRegEx
        {
            get
            {
                return this.mIsRegEx;
            }
        }

        public Color LinkColor
        {
            get
            {
                if (this.Effect == null)
                {
                    return this.mLinkColor;
                }
                return this.Effect.ChangeColor(this.mLinkColor);
            }
            set
            {
                this.mLinkColor = value;
            }
        }

        public Font LinkFont
        {
            get
            {
                if (this.Effect == null)
                {
                    return this.mLinkFont;
                }
                return this.Effect.ChangeFont(this.mLinkFont);
            }
            set
            {
                this.mLinkFont = value;
            }
        }

        public int LinkLength
        {
            get
            {
                return this.FullUrl.Length;
            }
        }

        public string LinkWord
        {
            get
            {
                return this.mLinkWord;
            }
        }

        public bool ShowLinkUrl
        {
            get
            {
                return this.mShowLinkUrl;
            }
        }

        public int StartIndex
        {
            get
            {
                return this.mStartIndex;
            }
            set
            {
                this.mStartIndex = value;
            }
        }

        public string Url
        {
            get
            {
                return this.mUrl;
            }
            set
            {
                this.mUrl = value;
            }
        }

        public int UrlLength
        {
            get
            {
                return this.Url.Length;
            }
        }
    }
}

