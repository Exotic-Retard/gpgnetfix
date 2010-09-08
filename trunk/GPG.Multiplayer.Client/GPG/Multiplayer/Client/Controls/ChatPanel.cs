namespace GPG.Multiplayer.Client.Controls
{
    using DevExpress.LookAndFeel;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.ChatEffect;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Games.SupremeCommander.tournaments;
    using GPG.Multiplayer.Quazal;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ChatPanel : PnlBase
    {
        private Dictionary<int, int> ChatRowHeights = new Dictionary<int, int>();
        private Dictionary<int, Rectangle> ChatRowPoints = new Dictionary<int, Rectangle>();
        private MenuItem ciChatText_Ban;
        private MenuItem ciChatText_ClanInvite;
        private MenuItem ciChatText_ClanRemove;
        private MenuItem ciChatText_ClanRequest;
        private MenuItem ciChatText_Clear;
        private MenuItem ciChatText_Copy;
        private MenuItem ciChatText_Demote;
        private MenuItem ciChatText_Filter;
        private MenuItem ciChatText_Filter_Admin;
        private MenuItem ciChatText_Filter_Clan;
        private MenuItem ciChatText_Filter_Errors;
        private MenuItem ciChatText_Filter_Events;
        private MenuItem ciChatText_Filter_Friends;
        private MenuItem ciChatText_Filter_Game;
        private MenuItem ciChatText_Filter_Other;
        private MenuItem ciChatText_Filter_Self;
        private MenuItem ciChatText_Filter_System;
        private MenuItem ciChatText_Filters_Reset;
        private MenuItem ciChatText_FriendInvite;
        private MenuItem ciChatText_FriendRemove;
        private MenuItem ciChatText_Ignore;
        private MenuItem ciChatText_Kick;
        private MenuItem ciChatText_LeaveClan;
        private MenuItem ciChatText_PrivateMsg;
        private MenuItem ciChatText_Promote;
        private MenuItem ciChatText_Replays;
        private MenuItem ciChatText_ShowEmotes;
        private MenuItem ciChatText_Solution;
        private MenuItem ciChatText_Unignore;
        private MenuItem ciChatText_ViewClan;
        private MenuItem ciChatText_ViewPlayer;
        private MenuItem ciChatText_ViewRank;
        private MenuItem ciChatText_WebStats;
        private MenuItem ciEmote_Animate;
        private MenuItem ciEmote_Delete;
        private MenuItem ciEmote_Hide;
        private MenuItem ciEmote_Manager;
        private MenuItem ciEmote_Share;
        private GridColumn colIcon;
        private GridColumn colPlayer;
        private GridColumn colText;
        private GridColumn colTimeStamp;
        private IContainer components = null;
        private bool CountEmotes = true;
        private uint CurrentFrame = 1;
        private int Divider = 1;
        private int EmoteCount = 0;
        private GridColumn gcVisible;
        public GPGChatGrid gpgChatGrid;
        private GPGContextMenu gpgContextMenuChatText;
        private GPGContextMenu gpgContextMenuEmote;
        private GPGPanel gpgPanel1;
        public GPGTextList gpgTextListCommands;
        private GridView gvChat;
        private int HistoryIndex = -1;
        private int LastTick = 0;
        private LinkedList<string> mChatHistory = new LinkedList<string>();
        private BindingList<ChatLine> mChatLines = new BindingList<ChatLine>();
        private MappedObjectList<User> mChatParticipants = new MappedObjectList<User>();
        private MenuItem menuItem11;
        private MenuItem menuItem12;
        private MenuItem menuItem13;
        private MenuItem menuItem18;
        private MenuItem menuItem23;
        private MenuItem menuItem24;
        private MenuItem menuItem25;
        private MenuItem menuItem26;
        private MenuItem menuItem27;
        private MenuItem menuItem28;
        private MenuItem menuItem29;
        private MenuItem menuItem4;
        private MenuItem menuItem6;
        private MenuItem menuItem9;
        private MenuItem menuItm15;
        private bool mFirstChatDraw = true;
        private MenuItem miShowColumns;
        private MenuItem miTranslate;
        private Hashtable mLastChatContent = Hashtable.Synchronized(new Hashtable());
        private Hashtable mLastChatTimes = Hashtable.Synchronized(new Hashtable());
        private GridView mSelectedParticipantView = null;
        private bool mShowSlashCommands = true;
        private bool PaintPending = false;
        private Dictionary<string, ChatEffectBase> PlayerChatEffects = new Dictionary<string, ChatEffectBase>();
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        public const int ScrollWidth = 10;
        private Emote SelectedEmote = null;
        private bool SelectingTextList = false;
        private SplitContainer splitContainerChatAndInput;
        internal GPGTextArea textBoxMsg;

        public event UserEventHandler AddSpeaker;

        public event StringEventHandler SendChatMessage;

        public event ChatLineStyleEventHandler StyleChatLine;

        public ChatPanel()
        {
            this.InitializeComponent();
            this.mChatHistory = new LinkedList<string>();
            this.mChatLines = new BindingList<ChatLine>();
            this.gpgChatGrid.DataSource = this.mChatLines;
        }

        internal void AddChat(User user, string message)
        {
            try
            {
                VGen0 method = null;
                VGen1 gen2 = null;
                bool scroll;
                if (!user.IsIgnored)
                {
                    if (message.IndexOf("I am the effect: ") >= 0)
                    {
                        this.RecieveChatEffect(user.Name, message.Replace("I am the effect: ", ""));
                    }
                    else
                    {
                        message = Profanity.MaskProfanity(message);
                        message = message.Replace("\t", "  ");
                        if (Program.Settings.Sound.Speech.EnableSpeech)
                        {
                            Speech.Speak(string.Format("{0} says,, {1}", user.Name, message), false);
                        }
                        scroll = true;
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    scroll = !this.gvChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvChat);
                                };
                            }
                            base.Invoke(method);
                        }
                        else if (!(base.Disposing || base.IsDisposed))
                        {
                            scroll = !this.gvChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvChat);
                        }
                        ChatLine line = new ChatLine(this.gpgChatGrid);
                        line.Tag = user;
                        line.PlayerInfo = user.Name;
                        line.Text = message;
                        line.Icon = null;
                        line.TextColor = Program.Settings.Chat.Appearance.DefaultColor;
                        line.TextFont = Program.Settings.Chat.Appearance.DefaultFont;
                        line.Filters["Self"] = user.IsCurrent;
                        line.Filters["System"] = user.Equals(User.System);
                        line.Filters["Game"] = user.Equals(User.Game);
                        line.Filters["Event"] = user.Equals(User.Event);
                        line.Filters["Error"] = user.Equals(User.Error);
                        line.Filters["Admin"] = !user.IsCurrent && user.IsAdmin;
                        line.Filters["Clan"] = !user.IsCurrent && user.IsClanMate;
                        line.Filters["Friend"] = user.IsFriend;
                        line.Filters["Other"] = ((!user.IsSystem && !user.IsAdmin) && (!user.IsClanMate && !user.IsFriend)) && !user.IsCurrent;
                        line.Filters["Speaking"] = true;
                        line.Filters[user.Name] = true;
                        this.OnStyleChatLine(line);
                        if (this.PlayerChatEffects.ContainsKey(user.Name))
                        {
                            foreach (TextSegment segment in line.TextSegments)
                            {
                                segment.Effect = this.PlayerChatEffects[user.Name];
                            }
                        }
                        if ((user != User.System) && (this.AddSpeaker != null))
                        {
                            this.AddSpeaker(this, user);
                        }
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (gen2 == null)
                            {
                                gen2 = delegate (object objline) {
                                    this.mChatLines.Add((ChatLine) objline);
                                    if (scroll)
                                    {
                                        this.gvChat.MoveLastVisible();
                                    }
                                };
                            }
                            base.BeginInvoke(gen2, new object[] { line });
                        }
                        else if (!base.Disposing && !base.IsDisposed)
                        {
                            this.mChatLines.Add(line);
                            if ((!base.IsDisposed && !base.Disposing) && scroll)
                            {
                                this.gvChat.MoveLastVisible();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void AddChat(int userId, string message)
        {
        }

        public void AddChat(string name, string text)
        {
            User record = null;
            if (!this.ChatParticipants.TryFindByIndex("name", name, out record))
            {
                if (ConfigSettings.GetBool("ForceLookup", false) && base.MainForm.TryFindUser(name, true, out record))
                {
                    this.ChatParticipants.Add(record);
                }
                else
                {
                    if (User.Current.IsAdmin && ConfigSettings.GetBool("ShowAdminSpew", false))
                    {
                        this.SystemMessage("Dropped Message: <" + name + "> " + text, new object[0]);
                    }
                    this.AddChat(User.MakeFakeUser(name), text);
                    ErrorLog.WriteLine("Recieved Message from unknown user: {0}", new object[] { text });
                    return;
                }
            }
            if ((record != null) && !User.IsUserIgnored(record.ID))
            {
                ChatLink[] linkArray = ChatLink.FindLinks(text, ChatLink.Emote);
                for (int i = 0; i < linkArray.Length; i++)
                {
                    string charSequence = EmoteLinkMask.GetCharSequence(linkArray[i]);
                    if ((charSequence == null) || (charSequence.Length < 1))
                    {
                        text = text.Replace(ChatLink.Emote.LinkWord, "");
                    }
                    else if (Emote.AllEmotes.ContainsKey(charSequence))
                    {
                        text = text.Replace(linkArray[i].FullUrl, charSequence);
                    }
                }
                if (text.Length > 300)
                {
                    text = text.Substring(0, 300);
                }
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                if (text.IndexOf("I am the effect: ") < 0)
                {
                    if (this.mLastChatTimes.ContainsKey(record))
                    {
                        DateTime time = (DateTime) this.mLastChatTimes[record];
                        TimeSpan span = (TimeSpan) (DateTime.Now - time);
                        flag = span.TotalMilliseconds < (Program.Settings.Chat.SpamInterval * 1000.0);
                        flag2 = span.TotalSeconds < 5.0;
                    }
                    if (flag2 && this.mLastChatContent.ContainsKey(record))
                    {
                        flag3 = text == this.mLastChatContent[record].ToString();
                    }
                }
                if ((record.Name == TournamentCommands.sDirectorName) || record.IsAdmin)
                {
                    flag = false;
                    flag3 = false;
                }
                if (!flag && !flag3)
                {
                    if (text.IndexOf("I am the effect: ") < 0)
                    {
                        this.mLastChatTimes[record] = DateTime.Now;
                        this.mLastChatContent[record] = text;
                    }
                    this.AddChat(record, text);
                }
            }
        }

        public void AddChatParticipant(User user)
        {
            this.ChatParticipants.Add(user);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.SendMessage();
        }

        private void CalculateColumnWidth()
        {
            VGen0 method = null;
            string text = "WWWWWWWWWWWWWWWWWWWWWW";
            int w = -1;
            int num = 0;
            using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
            {
                Font[] fontArray = new Font[] { Program.Settings.Chat.Appearance.AdminFont, Program.Settings.Chat.Appearance.ClanFont, Program.Settings.Chat.Appearance.DefaultFont, Program.Settings.Chat.Appearance.FriendsFont, Program.Settings.Chat.Appearance.SelfFont };
                for (int i = 0; i < fontArray.Length; i++)
                {
                    num = Convert.ToInt32(DrawUtil.MeasureString(graphics, text, fontArray[i]).Width);
                    if (num > w)
                    {
                        w = num;
                    }
                }
                num = Convert.ToInt32(DrawUtil.MeasureString(graphics, Loc.Get("<LOC>System"), Program.Settings.Chat.Appearance.SystemFont).Width);
                if (num > w)
                {
                    w = num;
                }
                num = Convert.ToInt32(DrawUtil.MeasureString(graphics, Loc.Get("<LOC>Error"), Program.Settings.Chat.Appearance.ErrorFont).Width);
                if (num > w)
                {
                    w = num;
                }
                num = Convert.ToInt32(DrawUtil.MeasureString(graphics, Loc.Get("<LOC>Event"), Program.Settings.Chat.Appearance.EventFont).Width);
                if (num > w)
                {
                    w = num;
                }
            }
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.gvChat.Columns["PlayerInfo"].Width = w;
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.gvChat.Columns["PlayerInfo"].Width = w;
            }
        }

        private void CalculateNameColumnWidth()
        {
            if (!this.gpgChatGrid.Disposing && !this.gpgChatGrid.IsDisposed)
            {
                GridView gvChat = this.gvChat;
                using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                {
                    VGen0 method = null;
                    int w = -1;
                    int num = 0;
                    int num2 = 0;
                    for (int i = 0; i < gvChat.RowCount; i++)
                    {
                        int visibleRowHandle = gvChat.GetVisibleRowHandle(i);
                        if (this.gvChat.IsRowVisible(visibleRowHandle) != RowVisibleState.Hidden)
                        {
                            ChatLine row = gvChat.GetRow(visibleRowHandle) as ChatLine;
                            num = Convert.ToInt32(DrawUtil.MeasureString(graphics, row.PlayerInfo, row.TextFont).Width);
                            if (num > w)
                            {
                                w = num;
                            }
                            num2++;
                        }
                    }
                    w += 10;
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                if (this.gvChat.Columns["PlayerInfo"].Width != w)
                                {
                                    this.gvChat.Columns["PlayerInfo"].Width = w;
                                    this.gpgChatGrid.Refresh();
                                }
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (this.gvChat.Columns["PlayerInfo"].Width != w)
                    {
                        this.gvChat.Columns["PlayerInfo"].Width = w;
                        this.gpgChatGrid.Refresh();
                    }
                    EventLog.WriteLine("Measured {0} lines", new object[] { num2 });
                }
            }
        }

        private void CheckUserState()
        {
            if ((UserStatus.Current != null) && (UserStatus.Current.Name.ToLower() != "none"))
            {
                Messaging.SendGathering("I am the effect: " + UserStatus.Current.Name);
            }
        }

        private void ciChatText_Clear_Click(object sender, EventArgs e)
        {
            this.ChatRowPoints.Clear();
            this.ChatRowHeights.Clear();
            this.mChatLines.Clear();
            this.gvChat.RefreshData();
        }

        private void ciChatText_Copy_Click(object sender, EventArgs e)
        {
            this.gvChat.CopyToClipboard();
        }

        private void ciChatText_Filter_Admin_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Admin.Checked = !this.ciChatText_Filter_Admin.Checked;
            Program.Settings.Chat.Filters.FilterAdmin = this.ciChatText_Filter_Admin.Checked;
        }

        private void ciChatText_Filter_Clan_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Clan.Checked = !this.ciChatText_Filter_Clan.Checked;
            Program.Settings.Chat.Filters.FilterClan = this.ciChatText_Filter_Clan.Checked;
        }

        private void ciChatText_Filter_Errors_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Errors.Checked = !this.ciChatText_Filter_Errors.Checked;
            Program.Settings.Chat.Filters.FilterSystemErrors = this.ciChatText_Filter_Errors.Checked;
        }

        private void ciChatText_Filter_Events_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Events.Checked = !this.ciChatText_Filter_Events.Checked;
            Program.Settings.Chat.Filters.FilterSystemEvents = this.ciChatText_Filter_Events.Checked;
        }

        private void ciChatText_Filter_Friends_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Friends.Checked = !this.ciChatText_Filter_Friends.Checked;
            Program.Settings.Chat.Filters.FilterFriends = this.ciChatText_Filter_Friends.Checked;
        }

        private void ciChatText_Filter_Game_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Game.Checked = !this.ciChatText_Filter_Game.Checked;
            Program.Settings.Chat.Filters.FilterGameMessages = this.ciChatText_Filter_Game.Checked;
        }

        private void ciChatText_Filter_Other_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Other.Checked = !this.ciChatText_Filter_Other.Checked;
            Program.Settings.Chat.Filters.FilterOther = this.ciChatText_Filter_Other.Checked;
        }

        private void ciChatText_Filter_Self_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_Self.Checked = !this.ciChatText_Filter_Self.Checked;
            Program.Settings.Chat.Filters.FilterSelf = this.ciChatText_Filter_Self.Checked;
        }

        private void ciChatText_Filter_System_Click(object sender, EventArgs e)
        {
            this.ciChatText_Filter_System.Checked = !this.ciChatText_Filter_System.Checked;
            Program.Settings.Chat.Filters.FilterSystemMessages = this.ciChatText_Filter_System.Checked;
        }

        private void ciEmote_Animate_Click(object sender, EventArgs e)
        {
            Program.Settings.Chat.Emotes.AnimateEmotes = !Program.Settings.Chat.Emotes.AnimateEmotes;
        }

        private void ciEmote_Delete_Click(object sender, EventArgs e)
        {
            if (this.SelectedEmote != null)
            {
                this.SelectedEmote.Delete(true, base.MainForm);
            }
        }

        private void ciEmote_Hide_Click(object sender, EventArgs e)
        {
            Program.Settings.Chat.Emotes.ShowEmotes = !Program.Settings.Chat.Emotes.ShowEmotes;
        }

        private void ciEmote_Manager_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowDlgEmotes();
        }

        private void ciEmote_Share_Click(object sender, EventArgs e)
        {
            Program.Settings.Chat.Emotes.AutoShareEmotes = !Program.Settings.Chat.Emotes.AutoShareEmotes;
        }

        private void ClearChatRowCache()
        {
            this.ChatRowPoints.Clear();
            this.ChatRowHeights.Clear();
        }

        private void CommandSelected(object sender, TextValEventArgs e)
        {
            if (e.Value != null)
            {
                string str = e.Value.Value.ToString();
                UserAction action = e.Value as UserAction;
                for (int i = 0; i < action.CommandWords.Length; i++)
                {
                    if (action.CommandWords[i].ToLower() == this.textBoxMsg.Text.ToLower())
                    {
                        str = action.CommandWords[i];
                        break;
                    }
                    if (action.CommandWords[i].ToLower().StartsWith(this.textBoxMsg.Text.ToLower()))
                    {
                        str = action.CommandWords[i];
                    }
                }
                this.textBoxMsg.Text = str.TrimEnd(new char[] { ' ' });
            }
            this.textBoxMsg.Select();
            this.textBoxMsg.Select(this.textBoxMsg.Text.Length, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoMessaging_MessageRecieved(MessageEventArgs e)
        {
            string[] strArray = e.Message.Split(">".ToCharArray(), 2);
            string name = strArray[0].Replace("<", "");
            if (name != User.Current.Name)
            {
                string text = strArray[1].Remove(0, 1);
                if (strArray.Length == 2)
                {
                    this.AddChat(name, text);
                }
            }
        }

        internal void ErrorMessage(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            this.AddChat(User.Error, msg);
        }

        private VGen0 FindLinks(string msg, out string result)
        {
            ChatLink[] linkArray = ChatLink.FindLinks(msg);
            if (linkArray.Length > 0)
            {
                foreach (ChatLink link in linkArray)
                {
                    msg = msg.Replace(link.FullUrl, link.DisplayText);
                }
                result = msg;
                return new VGen0(linkArray[0].OnClick);
            }
            result = msg;
            return null;
        }

        internal void GameEvent(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            this.AddChat(User.Game, msg);
        }

        private void gvChat_CalcRowHeight(object sender, RowHeightEventArgs e)
        {
            try
            {
                if (this.ChatRowHeights.ContainsKey(e.RowHandle))
                {
                    e.RowHeight = this.ChatRowHeights[e.RowHandle];
                }
                else
                {
                    ChatLine row = this.gvChat.GetRow(e.RowHandle) as ChatLine;
                    if (row != null)
                    {
                        IText text = row.TextSegments[0];
                        if (text != null)
                        {
                            int index;
                            Font textFont = text.TextFont;
                            string str = text.Text;
                            string str2 = null;
                            Dictionary<int, ChatLink> dictionary = null;
                            List<string> list = null;
                            List<MultiVal<int, int>> list2 = null;
                            SortedList<int, Emote> list3 = null;
                            if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                            {
                                dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                row.ContainsLinks = new bool?((dictionary != null) && (dictionary.Count > 0));
                            }
                            if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                            {
                                list = new List<string>();
                                list2 = new List<MultiVal<int, int>>();
                                list3 = new SortedList<int, Emote>();
                                SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                foreach (Emote emote in Emote.AllEmotes.Values)
                                {
                                    list4.Add(emote.CharSequence.Length, emote);
                                }
                                foreach (Emote emote in list4.Values)
                                {
                                    index = str.IndexOf(emote.CharSequence);
                                    while (index >= 0)
                                    {
                                        bool flag = false;
                                        if (dictionary != null)
                                        {
                                            foreach (ChatLink link in dictionary.Values)
                                            {
                                                if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                {
                                                    flag = true;
                                                }
                                            }
                                        }
                                        bool flag2 = false;
                                        foreach (KeyValuePair<int, Emote> pair in list3)
                                        {
                                            if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                            {
                                                flag2 = true;
                                                break;
                                            }
                                        }
                                        if (!(flag || flag2))
                                        {
                                            list2.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                            list3.Add(index, emote);
                                        }
                                        index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                        if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                        {
                                            if (str2 == null)
                                            {
                                                str2 = str;
                                            }
                                            str2 = str2.Replace(emote.CharSequence, "");
                                        }
                                    }
                                }
                                row.ContainsEmotes = new bool?((list3 != null) && (list3.Count > 0));
                            }
                            using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                            {
                                float num6;
                                float num7;
                                float num8;
                                int num9;
                                int num10;
                                ChatLink link2;
                                SolidBrush brush;
                                string[] strArray;
                                int num11;
                                SizeF ef;
                                Brush brush2;
                                Font linkFont;
                                if ((list3 != null) && (list3.Count > 0))
                                {
                                    int num2;
                                    list2.Add(new MultiVal<int, int>(str.Length, 0));
                                    list3.Add(str.Length, null);
                                    SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                    list5[-1] = new MultiVal<int, int>(0, 0);
                                    foreach (MultiVal<int, int> val in list2)
                                    {
                                        list5[val.Value1] = val;
                                    }
                                    for (num2 = 1; num2 < list5.Count; num2++)
                                    {
                                        int num3 = list5.Values[num2 - 1].Value1;
                                        index = list5.Values[num2].Value1;
                                        int num4 = list5.Values[num2 - 1].Value2;
                                        int num5 = list5.Values[num2].Value2;
                                        list.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                    }
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                    num6 = textFont.Height + 3;
                                    num7 = 0f;
                                    num8 = num6;
                                    num9 = this.colText.VisibleWidth - 10;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        for (num2 = 0; num2 < list.Count; num2++)
                                        {
                                            if (list[num2].Length > 0)
                                            {
                                                strArray = DrawUtil.SplitString(list[num2], " ");
                                                for (num11 = 0; num11 < strArray.Length; num11++)
                                                {
                                                    if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                    {
                                                        link2 = dictionary[num10];
                                                    }
                                                    if (link2 != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link2.LinkColor))
                                                        {
                                                            linkFont = link2.LinkFont;
                                                            ef = DrawUtil.MeasureString(graphics, strArray[num11], linkFont);
                                                            if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                            {
                                                                num7 = 0f;
                                                                num8 += num6;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                        if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                        {
                                                            num7 = 0f;
                                                            num8 += num6;
                                                        }
                                                    }
                                                    num10 += strArray[num11].Length;
                                                    num7 += ef.Width;
                                                    if ((link2 != null) && (num10 >= link2.EndIndex))
                                                    {
                                                        link2 = null;
                                                    }
                                                }
                                            }
                                            if (list3.Values[num2] != null)
                                            {
                                                float num12 = 1f;
                                                if (num12 < 1f)
                                                {
                                                    num12 = 1f;
                                                }
                                                float num13 = list3.Values[num2].Image.Width * num12;
                                                if ((num7 + num13) > num9)
                                                {
                                                    num7 = 0f;
                                                    num8 += num6;
                                                }
                                                num7 += num13;
                                            }
                                        }
                                    }
                                    e.RowHeight = Convert.ToInt32(Math.Round((double) num8, MidpointRounding.AwayFromZero));
                                }
                                else if ((dictionary != null) && (dictionary.Count > 0))
                                {
                                    num6 = textFont.Height + 3;
                                    num7 = 0f;
                                    num8 = num6;
                                    num9 = this.colText.VisibleWidth - 10;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        strArray = DrawUtil.SplitString(str, " ");
                                        num11 = 0;
                                        while (num11 < strArray.Length)
                                        {
                                            if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                            {
                                                link2 = dictionary[num10];
                                            }
                                            if (link2 != null)
                                            {
                                                using (brush2 = new SolidBrush(link2.LinkColor))
                                                {
                                                    linkFont = link2.LinkFont;
                                                    ef = DrawUtil.MeasureString(graphics, strArray[num11], linkFont);
                                                    if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                    {
                                                        num7 = 0f;
                                                        num8 += num6;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                {
                                                    num7 = 0f;
                                                    num8 += num6;
                                                }
                                            }
                                            num10 += strArray[num11].Length;
                                            num7 += ef.Width;
                                            if ((link2 != null) && (num10 >= link2.EndIndex))
                                            {
                                                link2 = null;
                                            }
                                            num11++;
                                        }
                                    }
                                    e.RowHeight = Convert.ToInt32(Math.Round((double) num8, MidpointRounding.AwayFromZero));
                                }
                                else
                                {
                                    num6 = textFont.Height + 3;
                                    num7 = 0f;
                                    num8 = num6;
                                    num9 = this.colText.VisibleWidth - 10;
                                    num10 = 0;
                                    strArray = DrawUtil.SplitString(str, " ");
                                    for (num11 = 0; num11 < strArray.Length; num11++)
                                    {
                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                        if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                        {
                                            num7 = 0f;
                                            num8 += num6;
                                        }
                                        num10 += strArray[num11].Length;
                                        num7 += ef.Width;
                                    }
                                    e.RowHeight = Convert.ToInt32(Math.Round((double) num8, MidpointRounding.AwayFromZero));
                                }
                            }
                        }
                    }
                    this.ChatRowHeights[e.RowHandle] = e.RowHeight;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvChat_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (this.mFirstChatDraw)
                {
                    this.mFirstChatDraw = false;
                    Program.Settings.StylePreferences.StyleControl(this.gvChat, EventArgs.Empty);
                }
                if ((e.Column.Name != this.colPlayer.Name) && (e.Column.Name != this.colText.Name))
                {
                    e.Handled = false;
                }
                else
                {
                    this.ChatRowPoints[e.RowHandle] = e.Bounds;
                    if (e.Column.AbsoluteIndex > 0)
                    {
                        ChatLine row = this.gvChat.GetRow(e.RowHandle) as ChatLine;
                        if (row != null)
                        {
                            IText text = null;
                            if ((e.Column.AbsoluteIndex == 1) && (row.PlayerSegments.Count > 0))
                            {
                                text = row.PlayerSegments[0];
                            }
                            else if ((e.Column.AbsoluteIndex == 2) && (row.TextSegments.Count > 0))
                            {
                                text = row.TextSegments[0];
                            }
                            if (text != null)
                            {
                                int num;
                                int index;
                                float num6;
                                float x;
                                float y;
                                int num9;
                                int num10;
                                ChatLink link2;
                                SolidBrush brush;
                                string[] strArray;
                                int num11;
                                SizeF ef;
                                Brush brush2;
                                Font linkFont;
                                Font textFont = text.TextFont;
                                Rectangle bounds = e.Bounds;
                                ChatLink[] linkArray = null;
                                Dictionary<int, ChatLink> dictionary = null;
                                SortedList<int, Emote> list = null;
                                List<string> list2 = null;
                                List<MultiVal<int, int>> list3 = null;
                                string str = text.Text;
                                string str2 = null;
                                if (e.Column.AbsoluteIndex == 2)
                                {
                                    if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                    {
                                        linkArray = ChatLink.FindLinks(text.Text, ChatLink.Emote);
                                        for (num = 0; num < linkArray.Length; num++)
                                        {
                                            if (Emote.AllEmotes.ContainsKey(linkArray[num].DisplayText))
                                            {
                                                text.Text = text.Text.Replace(linkArray[num].FullUrl, linkArray[num].DisplayText);
                                                str = str.Replace(linkArray[num].FullUrl, linkArray[num].DisplayText);
                                                row.ContainsEmotes = true;
                                            }
                                        }
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                        row.ContainsLinks = new bool?((dictionary != null) && (dictionary.Count > 0));
                                    }
                                    if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                                    {
                                        list2 = new List<string>();
                                        list3 = new List<MultiVal<int, int>>();
                                        list = new SortedList<int, Emote>();
                                        SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                        foreach (Emote emote in Emote.AllEmotes.Values)
                                        {
                                            list4.Add(emote.CharSequence.Length, emote);
                                        }
                                        foreach (Emote emote in list4.Values)
                                        {
                                            index = str.IndexOf(emote.CharSequence);
                                            while (index >= 0)
                                            {
                                                bool flag = false;
                                                if (dictionary != null)
                                                {
                                                    foreach (ChatLink link in dictionary.Values)
                                                    {
                                                        if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                        {
                                                            flag = true;
                                                        }
                                                    }
                                                }
                                                bool flag2 = false;
                                                foreach (KeyValuePair<int, Emote> pair in list)
                                                {
                                                    if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                                    {
                                                        flag2 = true;
                                                        break;
                                                    }
                                                }
                                                if (!(flag || flag2))
                                                {
                                                    list3.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                                    list.Add(index, emote);
                                                }
                                                index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                                if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                                {
                                                    if (str2 == null)
                                                    {
                                                        str2 = str;
                                                    }
                                                    str2 = str2.Replace(emote.CharSequence, "");
                                                }
                                            }
                                        }
                                        row.ContainsEmotes = new bool?((list != null) && (list.Count > 0));
                                    }
                                }
                                if ((list != null) && (list.Count > 0))
                                {
                                    list3.Add(new MultiVal<int, int>(str.Length, 0));
                                    list.Add(str.Length, null);
                                    SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list3.Count);
                                    list5[-1] = new MultiVal<int, int>(0, 0);
                                    foreach (MultiVal<int, int> val in list3)
                                    {
                                        list5[val.Value1] = val;
                                    }
                                    for (num = 1; num < list5.Count; num++)
                                    {
                                        int num3 = list5.Values[num - 1].Value1;
                                        index = list5.Values[num].Value1;
                                        int num4 = list5.Values[num - 1].Value2;
                                        int num5 = list5.Values[num].Value2;
                                        list2.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                    }
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                    num6 = textFont.Height + 3;
                                    x = bounds.X;
                                    y = bounds.Y;
                                    num9 = (this.colText.VisibleWidth - 10) + bounds.X;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        for (num = 0; num < list2.Count; num++)
                                        {
                                            if (list2[num].Length > 0)
                                            {
                                                strArray = DrawUtil.SplitString(list2[num], " ");
                                                for (num11 = 0; num11 < strArray.Length; num11++)
                                                {
                                                    if (link2 == null)
                                                    {
                                                        if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                        {
                                                            link2 = dictionary[num10];
                                                        }
                                                        if (link2 != null)
                                                        {
                                                            using (brush2 = new SolidBrush(link2.LinkColor))
                                                            {
                                                                linkFont = link2.LinkFont;
                                                                ef = DrawUtil.MeasureString(e.Graphics, link2.DisplayText + " ", linkFont);
                                                                if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                                {
                                                                    x = bounds.X;
                                                                    y += num6;
                                                                }
                                                                e.Graphics.DrawString(link2.DisplayText, linkFont, brush2, x, y);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ef = DrawUtil.MeasureString(e.Graphics, strArray[num11], textFont);
                                                            if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                            {
                                                                x = bounds.X;
                                                                y += num6;
                                                            }
                                                            e.Graphics.DrawString(strArray[num11], textFont, brush, x, y);
                                                        }
                                                        num10 += strArray[num11].Length;
                                                        x += ef.Width;
                                                    }
                                                    else
                                                    {
                                                        num10 += strArray[num11].Length;
                                                    }
                                                    if ((link2 != null) && (num10 >= link2.EndIndex))
                                                    {
                                                        link2 = null;
                                                    }
                                                }
                                            }
                                            if (list.Values[num] != null)
                                            {
                                                if (list.Values[num].Image.Height == 1)
                                                {
                                                }
                                                float num12 = 1f;
                                                if (num12 < 1f)
                                                {
                                                    num12 = 1f;
                                                }
                                                float width = list.Values[num].Image.Width * num12;
                                                float height = list.Values[num].Image.Height * num12;
                                                if ((x + width) > num9)
                                                {
                                                    x = bounds.X;
                                                    y += num6;
                                                }
                                                if (this.CountEmotes && list.Values[num].CanAnimate)
                                                {
                                                    this.EmoteCount++;
                                                }
                                                e.Graphics.DrawImage(list.Values[num].Image, x, y, width, height);
                                                x += width;
                                            }
                                        }
                                    }
                                }
                                else if ((dictionary != null) && (dictionary.Count > 0))
                                {
                                    num6 = textFont.Height + 3;
                                    x = bounds.X;
                                    y = bounds.Y;
                                    num9 = (this.colText.VisibleWidth - 10) + bounds.X;
                                    num10 = 0;
                                    link2 = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        strArray = DrawUtil.SplitString(str, " ");
                                        for (num11 = 0; num11 < strArray.Length; num11++)
                                        {
                                            if (link2 == null)
                                            {
                                                if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                {
                                                    link2 = dictionary[num10];
                                                }
                                                if (link2 != null)
                                                {
                                                    using (brush2 = new SolidBrush(link2.LinkColor))
                                                    {
                                                        linkFont = link2.LinkFont;
                                                        ef = DrawUtil.MeasureString(e.Graphics, link2.DisplayText + " ", linkFont);
                                                        if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                        {
                                                            x = bounds.X;
                                                            y += num6;
                                                        }
                                                        e.Graphics.DrawString(link2.DisplayText, linkFont, brush2, x, y);
                                                    }
                                                }
                                                else
                                                {
                                                    ef = DrawUtil.MeasureString(e.Graphics, strArray[num11], textFont);
                                                    if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                    {
                                                        x = bounds.X;
                                                        y += num6;
                                                    }
                                                    e.Graphics.DrawString(strArray[num11], textFont, brush, x, y);
                                                }
                                                num10 += strArray[num11].Length;
                                                x += ef.Width;
                                            }
                                            else
                                            {
                                                num10 += strArray[num11].Length;
                                            }
                                            if ((link2 != null) && (num10 >= link2.EndIndex))
                                            {
                                                link2 = null;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        num6 = textFont.Height + 3;
                                        x = bounds.Left;
                                        y = bounds.Top;
                                        num9 = (this.colText.VisibleWidth - 10) + bounds.X;
                                        num10 = 0;
                                        strArray = DrawUtil.SplitString(str, " ");
                                        for (num11 = 0; num11 < strArray.Length; num11++)
                                        {
                                            ef = DrawUtil.MeasureString(e.Graphics, strArray[num11], textFont);
                                            if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                            {
                                                x = bounds.Left;
                                                y += num6;
                                            }
                                            e.Graphics.DrawString(strArray[num11], textFont, brush, x, y);
                                            num10 += strArray[num11].Length;
                                            x += ef.Width;
                                        }
                                    }
                                }
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gvChat_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && (sender is GridView))
            {
                this.mSelectedParticipantView = sender as GridView;
                this.mSelectedParticipantView.Focus();
                GridHitInfo info = this.mSelectedParticipantView.CalcHitInfo(e.Location);
                if (this.mSelectedParticipantView.OptionsSelection.MultiSelect && (this.mSelectedParticipantView.OptionsSelection.MultiSelectMode == GridMultiSelectMode.RowSelect))
                {
                    int[] selectedRows = this.mSelectedParticipantView.GetSelectedRows();
                    for (int i = 0; i < selectedRows.Length; i++)
                    {
                        this.mSelectedParticipantView.UnselectRow(selectedRows[i]);
                    }
                }
                if (info.InRow)
                {
                    (sender as GridView).SelectRow(info.RowHandle);
                }
            }
        }

        private void gvChat_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo info = (sender as GridView).CalcHitInfo(e.X, e.Y);
                if (info.InRow && this.ChatRowPoints.ContainsKey(info.RowHandle))
                {
                    Rectangle rectangle = this.ChatRowPoints[info.RowHandle];
                    if (rectangle.Contains(e.X, e.Y))
                    {
                        ChatLine row = (sender as GridView).GetRow(info.RowHandle) as ChatLine;
                        if (row != null)
                        {
                            IText text = row.TextSegments[0];
                            if (text != null)
                            {
                                int index;
                                Font textFont = text.TextFont;
                                string str = text.Text;
                                string str2 = null;
                                Dictionary<int, ChatLink> dictionary = null;
                                List<string> list = null;
                                List<MultiVal<int, int>> list2 = null;
                                SortedList<int, Emote> list3 = null;
                                if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                {
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                }
                                if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                                {
                                    list = new List<string>();
                                    list2 = new List<MultiVal<int, int>>();
                                    list3 = new SortedList<int, Emote>();
                                    SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                    foreach (Emote emote in Emote.AllEmotes.Values)
                                    {
                                        list4.Add(emote.CharSequence.Length, emote);
                                    }
                                    foreach (Emote emote in list4.Values)
                                    {
                                        index = str.IndexOf(emote.CharSequence);
                                        while (index >= 0)
                                        {
                                            bool flag = false;
                                            if (dictionary != null)
                                            {
                                                foreach (ChatLink link in dictionary.Values)
                                                {
                                                    if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                    {
                                                        flag = true;
                                                    }
                                                }
                                            }
                                            bool flag2 = false;
                                            foreach (KeyValuePair<int, Emote> pair in list3)
                                            {
                                                if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                                {
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                            if (!(flag || flag2))
                                            {
                                                list2.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                                list3.Add(index, emote);
                                            }
                                            index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                            if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                            {
                                                if (str2 == null)
                                                {
                                                    str2 = str;
                                                }
                                                str2 = str2.Replace(emote.CharSequence, "");
                                            }
                                        }
                                    }
                                }
                                using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                                {
                                    float num6;
                                    float x;
                                    float y;
                                    int num9;
                                    int num10;
                                    ChatLink link2;
                                    SolidBrush brush;
                                    string[] strArray;
                                    int num11;
                                    SizeF ef;
                                    Brush brush2;
                                    Font linkFont;
                                    RectangleF ef2;
                                    if ((list3 != null) && (list3.Count > 0))
                                    {
                                        int num2;
                                        list2.Add(new MultiVal<int, int>(str.Length, 0));
                                        list3.Add(str.Length, null);
                                        SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                        list5[-1] = new MultiVal<int, int>(0, 0);
                                        foreach (MultiVal<int, int> val in list2)
                                        {
                                            list5[val.Value1] = val;
                                        }
                                        for (num2 = 1; num2 < list5.Count; num2++)
                                        {
                                            int num3 = list5.Values[num2 - 1].Value1;
                                            index = list5.Values[num2].Value1;
                                            int num4 = list5.Values[num2 - 1].Value2;
                                            int num5 = list5.Values[num2].Value2;
                                            list.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                        }
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            for (num2 = 0; num2 < list.Count; num2++)
                                            {
                                                if (list[num2].Length > 0)
                                                {
                                                    strArray = DrawUtil.SplitString(list[num2], " ");
                                                    for (num11 = 0; num11 < strArray.Length; num11++)
                                                    {
                                                        if (link2 == null)
                                                        {
                                                            if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                            {
                                                                link2 = dictionary[num10];
                                                            }
                                                            if (link2 != null)
                                                            {
                                                                using (brush2 = new SolidBrush(link2.LinkColor))
                                                                {
                                                                    linkFont = link2.LinkFont;
                                                                    ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                                    if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                    {
                                                                        x = rectangle.X;
                                                                        y += num6;
                                                                    }
                                                                    ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                                    if (ef2.Contains((float) e.X, (float) e.Y))
                                                                    {
                                                                        this.Cursor = Cursors.Hand;
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                                if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                {
                                                                    x = rectangle.X;
                                                                    y += num6;
                                                                }
                                                            }
                                                            num10 += strArray[num11].Length;
                                                            x += ef.Width;
                                                        }
                                                        else
                                                        {
                                                            num10 += strArray[num11].Length;
                                                        }
                                                        if ((link2 != null) && (num10 >= link2.EndIndex))
                                                        {
                                                            link2 = null;
                                                        }
                                                    }
                                                }
                                                if (list3.Values[num2] != null)
                                                {
                                                    float num12 = 1f;
                                                    if (num12 < 1f)
                                                    {
                                                        num12 = 1f;
                                                    }
                                                    float num13 = list3.Values[num2].Image.Width * num12;
                                                    if ((x + num13) > num9)
                                                    {
                                                        x = rectangle.X;
                                                        y += num6;
                                                    }
                                                    x += num13;
                                                }
                                            }
                                        }
                                    }
                                    else if ((dictionary != null) && (dictionary.Count > 0))
                                    {
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            strArray = DrawUtil.SplitString(str, " ");
                                            for (num11 = 0; num11 < strArray.Length; num11++)
                                            {
                                                if (link2 == null)
                                                {
                                                    if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                    {
                                                        link2 = dictionary[num10];
                                                    }
                                                    if (link2 != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link2.LinkColor))
                                                        {
                                                            linkFont = link2.LinkFont;
                                                            ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                            if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                            {
                                                                x = rectangle.X;
                                                                y += num6;
                                                            }
                                                            ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                            if (ef2.Contains((float) e.X, (float) e.Y))
                                                            {
                                                                this.Cursor = Cursors.Hand;
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                        if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                        {
                                                            x = rectangle.X;
                                                            y += num6;
                                                        }
                                                    }
                                                    num10 += strArray[num11].Length;
                                                    x += ef.Width;
                                                }
                                                else
                                                {
                                                    num10 += strArray[num11].Length;
                                                }
                                                if ((link2 != null) && (num10 >= link2.EndIndex))
                                                {
                                                    link2 = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void gvChat_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                int num2;
                if (this.ChatRowPoints.ContainsKey((sender as GridView).FocusedRowHandle))
                {
                    Rectangle rectangle = this.ChatRowPoints[(sender as GridView).FocusedRowHandle];
                    if (rectangle.Contains(e.X, e.Y))
                    {
                        ChatLine row = (sender as GridView).GetRow((sender as GridView).FocusedRowHandle) as ChatLine;
                        if (row != null)
                        {
                            IText text = row.TextSegments[0];
                            if (text != null)
                            {
                                int index;
                                Font textFont = text.TextFont;
                                string str = text.Text;
                                string str2 = null;
                                Dictionary<int, ChatLink> dictionary = null;
                                List<string> list = null;
                                List<MultiVal<int, int>> list2 = null;
                                SortedList<int, Emote> list3 = null;
                                if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                {
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                }
                                if (!(!Program.Settings.Chat.Emotes.ShowEmotes || (!row.ContainsEmotes.HasValue ? false : !row.ContainsEmotes.Value)))
                                {
                                    list = new List<string>();
                                    list2 = new List<MultiVal<int, int>>();
                                    list3 = new SortedList<int, Emote>();
                                    SortedList<int, Emote> list4 = new SortedList<int, Emote>(new EmoteLengthComparer());
                                    foreach (Emote emote in Emote.AllEmotes.Values)
                                    {
                                        list4.Add(emote.CharSequence.Length, emote);
                                    }
                                    foreach (Emote emote in list4.Values)
                                    {
                                        index = str.IndexOf(emote.CharSequence);
                                        while (index >= 0)
                                        {
                                            bool flag = false;
                                            if (dictionary != null)
                                            {
                                                foreach (ChatLink link in dictionary.Values)
                                                {
                                                    if ((index >= link.StartIndex) && (index <= link.EndIndex))
                                                    {
                                                        flag = true;
                                                    }
                                                }
                                            }
                                            bool flag2 = false;
                                            foreach (KeyValuePair<int, Emote> pair in list3)
                                            {
                                                if ((index >= pair.Key) && (index < (pair.Key + pair.Value.CharSequence.Length)))
                                                {
                                                    flag2 = true;
                                                    break;
                                                }
                                            }
                                            if (!(flag || flag2))
                                            {
                                                list2.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                                list3.Add(index, emote);
                                            }
                                            index = str.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                            if (!(!Program.Settings.Chat.Links.ShowChatLinks || (!row.ContainsLinks.HasValue ? false : !row.ContainsLinks.Value)))
                                            {
                                                if (str2 == null)
                                                {
                                                    str2 = str;
                                                }
                                                str2 = str2.Replace(emote.CharSequence, "");
                                            }
                                        }
                                    }
                                }
                                using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                                {
                                    float num6;
                                    float x;
                                    float y;
                                    int num9;
                                    int num10;
                                    ChatLink link2;
                                    SolidBrush brush;
                                    string[] strArray;
                                    int num11;
                                    SizeF ef;
                                    Brush brush2;
                                    Font linkFont;
                                    RectangleF ef2;
                                    if ((list3 != null) && (list3.Count > 0))
                                    {
                                        list2.Add(new MultiVal<int, int>(str.Length, 0));
                                        list3.Add(str.Length, null);
                                        SortedList<int, MultiVal<int, int>> list5 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                        list5[-1] = new MultiVal<int, int>(0, 0);
                                        foreach (MultiVal<int, int> val in list2)
                                        {
                                            list5[val.Value1] = val;
                                        }
                                        for (num2 = 1; num2 < list5.Count; num2++)
                                        {
                                            int num3 = list5.Values[num2 - 1].Value1;
                                            index = list5.Values[num2].Value1;
                                            int num4 = list5.Values[num2 - 1].Value2;
                                            int num5 = list5.Values[num2].Value2;
                                            list.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                        }
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            num2 = 0;
                                            while (num2 < list.Count)
                                            {
                                                if (list[num2].Length > 0)
                                                {
                                                    strArray = DrawUtil.SplitString(list[num2], " ");
                                                    for (num11 = 0; num11 < strArray.Length; num11++)
                                                    {
                                                        if (link2 == null)
                                                        {
                                                            if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                            {
                                                                link2 = dictionary[num10];
                                                            }
                                                            if (link2 != null)
                                                            {
                                                                using (brush2 = new SolidBrush(link2.LinkColor))
                                                                {
                                                                    linkFont = link2.LinkFont;
                                                                    ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                                    if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                    {
                                                                        x = rectangle.X;
                                                                        y += num6;
                                                                    }
                                                                    ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                                    if (ef2.Contains((float) e.X, (float) e.Y))
                                                                    {
                                                                        link2.OnClick();
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                                if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                                {
                                                                    x = rectangle.X;
                                                                    y += num6;
                                                                }
                                                            }
                                                            num10 += strArray[num11].Length;
                                                            x += ef.Width;
                                                        }
                                                        else
                                                        {
                                                            num10 += strArray[num11].Length;
                                                        }
                                                        if ((link2 != null) && (num10 >= link2.EndIndex))
                                                        {
                                                            link2 = null;
                                                        }
                                                    }
                                                }
                                                if (list3.Values[num2] != null)
                                                {
                                                    float num12 = 1f;
                                                    float num13 = list3.Values[num2].Image.Width * num12;
                                                    if ((x + num13) > num9)
                                                    {
                                                        x = rectangle.X;
                                                        y += num6;
                                                    }
                                                    if ((e.Button == MouseButtons.Right) && ConfigSettings.GetBool("Emotes", true))
                                                    {
                                                        Rectangle rectangle2 = new Rectangle((int) x, (int) y, list3.Values[num2].Image.Width, list3.Values[num2].Image.Height);
                                                        if (rectangle2.Contains(e.Location))
                                                        {
                                                            this.ciEmote_Animate.Checked = Program.Settings.Chat.Emotes.AnimateEmotes;
                                                            this.ciEmote_Share.Checked = Program.Settings.Chat.Emotes.AutoShareEmotes;
                                                            this.SelectedEmote = list3.Values[num2];
                                                            this.gpgContextMenuEmote.Show(this.gpgChatGrid, e.Location);
                                                            return;
                                                        }
                                                    }
                                                    x += num13;
                                                }
                                                num2++;
                                            }
                                        }
                                    }
                                    else if ((dictionary != null) && (dictionary.Count > 0))
                                    {
                                        num6 = textFont.Height + 3;
                                        x = rectangle.X;
                                        y = rectangle.Y;
                                        num9 = (this.colText.VisibleWidth - 10) + rectangle.X;
                                        num10 = 0;
                                        link2 = null;
                                        using (brush = new SolidBrush(text.TextColor))
                                        {
                                            strArray = DrawUtil.SplitString(str, " ");
                                            for (num11 = 0; num11 < strArray.Length; num11++)
                                            {
                                                if (link2 == null)
                                                {
                                                    if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                    {
                                                        link2 = dictionary[num10];
                                                    }
                                                    if (link2 != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link2.LinkColor))
                                                        {
                                                            linkFont = link2.LinkFont;
                                                            ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", linkFont);
                                                            if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                            {
                                                                x = rectangle.X;
                                                                y += num6;
                                                            }
                                                            ef2 = new RectangleF(x, y, ef.Width, ef.Height);
                                                            if (ef2.Contains((float) e.X, (float) e.Y))
                                                            {
                                                                if (e.Button == MouseButtons.Left)
                                                                {
                                                                    link2.OnClick();
                                                                    goto Label_0C82;
                                                                }
                                                                if (e.Button == MouseButtons.Right)
                                                                {
                                                                    goto Label_0C82;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                        if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                        {
                                                            x = rectangle.X;
                                                            y += num6;
                                                        }
                                                    }
                                                    num10 += strArray[num11].Length;
                                                    x += ef.Width;
                                                }
                                                else
                                                {
                                                    num10 += strArray[num11].Length;
                                                }
                                                if ((link2 != null) && (num10 >= link2.EndIndex))
                                                {
                                                    link2 = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            Label_0C82:
                if (e.Button == MouseButtons.Right)
                {
                    if ((this.SelectedChatTextSender == null) || this.SelectedChatTextSender.IsSystem)
                    {
                        this.ciChatText_ShowEmotes.Visible = !Program.Settings.Chat.Emotes.ShowEmotes;
                        this.ciChatText_ViewPlayer.Visible = false;
                        this.ciChatText_ViewRank.Visible = false;
                        this.ciChatText_PrivateMsg.Visible = false;
                        this.ciChatText_ClanInvite.Visible = false;
                        this.ciChatText_ClanRequest.Visible = false;
                        this.ciChatText_ViewClan.Visible = false;
                        this.ciChatText_Ignore.Visible = false;
                        this.ciChatText_Unignore.Visible = false;
                        this.ciChatText_WebStats.Visible = false;
                        this.ciChatText_Replays.Visible = false;
                        this.ciChatText_FriendInvite.Visible = false;
                        this.ciChatText_FriendRemove.Visible = false;
                        this.ciChatText_Promote.Visible = false;
                        this.ciChatText_Demote.Visible = false;
                        this.ciChatText_ClanRemove.Visible = false;
                        this.ciChatText_LeaveClan.Visible = false;
                        this.ciChatText_Ban.Visible = false;
                        this.ciChatText_Kick.Visible = false;
                        this.ciChatText_Solution.Visible = false;
                    }
                    else
                    {
                        User selectedChatTextSender = this.SelectedChatTextSender;
                        this.ciChatText_ShowEmotes.Visible = !Program.Settings.Chat.Emotes.ShowEmotes;
                        this.ciChatText_ViewPlayer.Visible = true;
                        this.ciChatText_ViewRank.Visible = false;
                        this.ciChatText_PrivateMsg.Visible = !selectedChatTextSender.IsCurrent && selectedChatTextSender.Online;
                        this.ciChatText_ClanInvite.Visible = (User.Current.IsInClan && !selectedChatTextSender.IsInClan) && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_ClanRequest.Visible = (!User.Current.IsInClan && selectedChatTextSender.IsInClan) && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_ViewClan.Visible = selectedChatTextSender.IsInClan;
                        this.ciChatText_Ignore.Visible = !selectedChatTextSender.IsCurrent && !selectedChatTextSender.IsIgnored;
                        this.ciChatText_Unignore.Visible = !selectedChatTextSender.IsCurrent && selectedChatTextSender.IsIgnored;
                        this.ciChatText_WebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
                        this.ciChatText_Replays.Visible = true;
                        this.ciChatText_FriendInvite.Visible = !selectedChatTextSender.IsFriend && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_FriendRemove.Visible = selectedChatTextSender.IsFriend && !selectedChatTextSender.IsCurrent;
                        this.ciChatText_Promote.Visible = selectedChatTextSender.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Promote, selectedChatTextSender.Name);
                        this.ciChatText_Demote.Visible = selectedChatTextSender.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Demote, selectedChatTextSender.Name);
                        this.ciChatText_ClanRemove.Visible = selectedChatTextSender.IsClanMate && ClanMember.Current.CanTargetAbility(ClanAbility.Remove, selectedChatTextSender.Name);
                        this.ciChatText_LeaveClan.Visible = User.Current.Equals(selectedChatTextSender) && User.Current.IsInClan;
                        if ((Chatroom.Current != null) && Chatroom.Current.IsClanRoom)
                        {
                            this.ciChatText_Ban.Visible = ((User.Current.IsAdmin || ClanMember.Current.CanTargetAbility(ClanAbility.Ban, selectedChatTextSender.Name)) && !selectedChatTextSender.IsCurrent) && !selectedChatTextSender.IsAdmin;
                            this.ciChatText_Kick.Visible = ((User.Current.IsAdmin || ClanMember.Current.CanTargetAbility(ClanAbility.Kick, selectedChatTextSender.Name)) && !selectedChatTextSender.IsCurrent) && !selectedChatTextSender.IsAdmin;
                        }
                        else
                        {
                            this.ciChatText_Ban.Visible = (((Chatroom.InChatroom && Chatroom.GatheringParticipants.ContainsIndex("name", selectedChatTextSender.Name)) && (User.Current.IsAdmin || (!Chatroom.Current.IsPersistent && User.Current.IsChannelOperator))) && !selectedChatTextSender.IsCurrent) && !selectedChatTextSender.IsAdmin;
                            this.ciChatText_Kick.Visible = this.ciChatText_Ban.Visible;
                        }
                        this.ciChatText_Solution.Visible = true;
                    }
                    int num14 = -1;
                    for (num2 = 0; num2 < this.gpgContextMenuChatText.MenuItems.Count; num2++)
                    {
                        if (this.gpgContextMenuChatText.MenuItems[num2].Text == "-")
                        {
                            if ((num14 < 0) || (this.gpgContextMenuChatText.MenuItems[num14].Text == "-"))
                            {
                                this.gpgContextMenuChatText.MenuItems[num2].Visible = false;
                            }
                            else if (num2 == (this.gpgContextMenuChatText.MenuItems.Count - 1))
                            {
                                this.gpgContextMenuChatText.MenuItems[num2].Visible = false;
                            }
                            else
                            {
                                this.gpgContextMenuChatText.MenuItems[num2].Visible = true;
                            }
                        }
                        if (this.gpgContextMenuChatText.MenuItems[num2].Visible)
                        {
                            num14 = num2;
                        }
                    }
                    if ((num14 >= 0) && (this.gpgContextMenuChatText.MenuItems[num14].Text == "-"))
                    {
                        this.gpgContextMenuChatText.MenuItems[num14].Visible = false;
                    }
                    this.gpgContextMenuChatText.Show(this.gvChat.GridControl, e.Location);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void InitializeComponent()
        {
            this.splitContainerChatAndInput = new SplitContainer();
            this.gpgTextListCommands = new GPGTextList();
            this.textBoxMsg = new GPGTextArea();
            this.gpgChatGrid = new GPGChatGrid();
            this.gvChat = new GridView();
            this.colIcon = new GridColumn();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.colPlayer = new GridColumn();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.colText = new GridColumn();
            this.gcVisible = new GridColumn();
            this.colTimeStamp = new GridColumn();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.gpgContextMenuChatText = new GPGContextMenu();
            this.ciChatText_Clear = new MenuItem();
            this.ciChatText_Copy = new MenuItem();
            this.ciChatText_Filter = new MenuItem();
            this.ciChatText_Filter_Self = new MenuItem();
            this.ciChatText_Filter_System = new MenuItem();
            this.ciChatText_Filter_Events = new MenuItem();
            this.ciChatText_Filter_Errors = new MenuItem();
            this.ciChatText_Filter_Game = new MenuItem();
            this.ciChatText_Filter_Friends = new MenuItem();
            this.ciChatText_Filter_Clan = new MenuItem();
            this.ciChatText_Filter_Admin = new MenuItem();
            this.ciChatText_Filter_Other = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.ciChatText_Filters_Reset = new MenuItem();
            this.miShowColumns = new MenuItem();
            this.ciChatText_ShowEmotes = new MenuItem();
            this.menuItm15 = new MenuItem();
            this.ciChatText_PrivateMsg = new MenuItem();
            this.ciChatText_Ignore = new MenuItem();
            this.ciChatText_Unignore = new MenuItem();
            this.ciChatText_ViewRank = new MenuItem();
            this.ciChatText_WebStats = new MenuItem();
            this.ciChatText_ViewPlayer = new MenuItem();
            this.ciChatText_Replays = new MenuItem();
            this.menuItem6 = new MenuItem();
            this.ciChatText_FriendInvite = new MenuItem();
            this.ciChatText_FriendRemove = new MenuItem();
            this.menuItem11 = new MenuItem();
            this.ciChatText_ClanInvite = new MenuItem();
            this.ciChatText_ClanRequest = new MenuItem();
            this.ciChatText_ClanRemove = new MenuItem();
            this.ciChatText_Promote = new MenuItem();
            this.ciChatText_Demote = new MenuItem();
            this.ciChatText_ViewClan = new MenuItem();
            this.ciChatText_LeaveClan = new MenuItem();
            this.menuItem18 = new MenuItem();
            this.ciChatText_Kick = new MenuItem();
            this.ciChatText_Ban = new MenuItem();
            this.menuItem12 = new MenuItem();
            this.ciChatText_Solution = new MenuItem();
            this.miTranslate = new MenuItem();
            this.menuItem23 = new MenuItem();
            this.menuItem24 = new MenuItem();
            this.menuItem25 = new MenuItem();
            this.menuItem26 = new MenuItem();
            this.menuItem27 = new MenuItem();
            this.menuItem28 = new MenuItem();
            this.menuItem29 = new MenuItem();
            this.gpgContextMenuEmote = new GPGContextMenu();
            this.ciEmote_Manager = new MenuItem();
            this.menuItem13 = new MenuItem();
            this.ciEmote_Hide = new MenuItem();
            this.ciEmote_Share = new MenuItem();
            this.ciEmote_Animate = new MenuItem();
            this.menuItem9 = new MenuItem();
            this.ciEmote_Delete = new MenuItem();
            this.gpgPanel1 = new GPGPanel();
            this.splitContainerChatAndInput.Panel1.SuspendLayout();
            this.splitContainerChatAndInput.Panel2.SuspendLayout();
            this.splitContainerChatAndInput.SuspendLayout();
            this.textBoxMsg.Properties.BeginInit();
            this.gpgChatGrid.BeginInit();
            this.gvChat.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.gpgPanel1.SuspendLayout();
            base.SuspendLayout();
            this.splitContainerChatAndInput.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerChatAndInput.BackColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.splitContainerChatAndInput.Location = new Point(2, 2);
            this.splitContainerChatAndInput.Name = "splitContainerChatAndInput";
            this.splitContainerChatAndInput.Orientation = Orientation.Horizontal;
            this.splitContainerChatAndInput.Panel1.Controls.Add(this.gpgTextListCommands);
            this.splitContainerChatAndInput.Panel1.Controls.Add(this.gpgChatGrid);
            this.splitContainerChatAndInput.Panel2.Controls.Add(this.textBoxMsg);
            this.splitContainerChatAndInput.Size = new Size(0x193, 0xe2);
            this.splitContainerChatAndInput.SplitterDistance = 0xc4;
            this.splitContainerChatAndInput.SplitterWidth = 2;
            this.splitContainerChatAndInput.TabIndex = 1;
            this.gpgTextListCommands.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextListCommands.AnchorControl = this.textBoxMsg;
            this.gpgTextListCommands.AutoScroll = true;
            this.gpgTextListCommands.AutoSize = true;
            this.gpgTextListCommands.BackColor = Color.Black;
            this.gpgTextListCommands.Location = new Point(0, 180);
            this.gpgTextListCommands.Margin = new Padding(0);
            this.gpgTextListCommands.MaxLines = 6;
            this.gpgTextListCommands.Name = "gpgTextListCommands";
            this.gpgTextListCommands.SelectedIndex = -1;
            this.gpgTextListCommands.Size = new Size(0x193, 20);
            this.gpgTextListCommands.TabIndex = 14;
            this.gpgTextListCommands.TextLines = null;
            this.gpgTextListCommands.Visible = false;
            this.textBoxMsg.BorderColor = Color.White;
            this.textBoxMsg.Dock = DockStyle.Fill;
            this.textBoxMsg.Location = new Point(0, 0);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Properties.AcceptsReturn = false;
            this.textBoxMsg.Properties.Appearance.BackColor = Color.Black;
            this.textBoxMsg.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.textBoxMsg.Properties.Appearance.ForeColor = Color.White;
            this.textBoxMsg.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxMsg.Properties.Appearance.Options.UseBorderColor = true;
            this.textBoxMsg.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxMsg.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x2e, 0x2e, 0x49);
            this.textBoxMsg.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.textBoxMsg.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.textBoxMsg.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.textBoxMsg.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.textBoxMsg.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.textBoxMsg.Properties.BorderStyle = BorderStyles.NoBorder;
            this.textBoxMsg.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.textBoxMsg.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.textBoxMsg.Properties.MaxLength = 0x400;
            this.textBoxMsg.Size = new Size(0x193, 0x1c);
            this.textBoxMsg.TabIndex = 0;
            this.textBoxMsg.EditValueChanging += new ChangingEventHandler(this.textBoxMsg_EditValueChanging);
            this.textBoxMsg.KeyDown += new KeyEventHandler(this.textBoxMsg_KeyDown);
            this.gpgChatGrid.CustomizeStyle = true;
            this.gpgChatGrid.Dock = DockStyle.Fill;
            this.gpgChatGrid.EmbeddedNavigator.Name = "";
            this.gpgChatGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgChatGrid.IgnoreMouseWheel = false;
            this.gpgChatGrid.Location = new Point(0, 0);
            this.gpgChatGrid.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgChatGrid.LookAndFeel.Style = LookAndFeelStyle.UltraFlat;
            this.gpgChatGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgChatGrid.MainView = this.gvChat;
            this.gpgChatGrid.Name = "gpgChatGrid";
            this.gpgChatGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3 });
            this.gpgChatGrid.ShowOnlyPredefinedDetails = true;
            this.gpgChatGrid.Size = new Size(0x193, 0xc4);
            this.gpgChatGrid.TabIndex = 10;
            this.gpgChatGrid.ViewCollection.AddRange(new BaseView[] { this.gvChat });
            this.gvChat.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvChat.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvChat.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvChat.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvChat.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvChat.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvChat.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvChat.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvChat.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvChat.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvChat.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvChat.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvChat.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvChat.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvChat.Appearance.Empty.BackColor = Color.Black;
            this.gvChat.Appearance.Empty.Options.UseBackColor = true;
            this.gvChat.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvChat.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvChat.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvChat.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvChat.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvChat.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvChat.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvChat.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvChat.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvChat.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvChat.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvChat.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvChat.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvChat.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvChat.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvChat.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvChat.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvChat.Appearance.FocusedCell.Options.UseFont = true;
            this.gvChat.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvChat.Appearance.FocusedRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvChat.Appearance.FocusedRow.BackColor2 = Color.Black;
            this.gvChat.Appearance.FocusedRow.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            this.gvChat.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvChat.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvChat.Appearance.FocusedRow.Options.UseFont = true;
            this.gvChat.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvChat.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvChat.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvChat.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvChat.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvChat.Appearance.FooterPanel.Options.UseFont = true;
            this.gvChat.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupButton.BackColor = Color.Black;
            this.gvChat.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvChat.Appearance.GroupButton.ForeColor = Color.White;
            this.gvChat.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvChat.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvChat.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvChat.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvChat.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvChat.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvChat.Appearance.GroupPanel.BackColor2 = Color.White;
            this.gvChat.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvChat.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvChat.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupPanel.Options.UseFont = true;
            this.gvChat.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvChat.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.GroupRow.ForeColor = Color.White;
            this.gvChat.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvChat.Appearance.GroupRow.Options.UseFont = true;
            this.gvChat.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvChat.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvChat.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvChat.Appearance.HeaderPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvChat.Appearance.HeaderPanel.ForeColor = Color.White;
            this.gvChat.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvChat.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvChat.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvChat.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvChat.Appearance.HideSelectionRow.BackColor = Color.Black;
            this.gvChat.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.HideSelectionRow.ForeColor = Color.Black;
            this.gvChat.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvChat.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvChat.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvChat.Appearance.HorzLine.BackColor = Color.Yellow;
            this.gvChat.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvChat.Appearance.Preview.BackColor = Color.White;
            this.gvChat.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.Preview.ForeColor = Color.Purple;
            this.gvChat.Appearance.Preview.Options.UseBackColor = true;
            this.gvChat.Appearance.Preview.Options.UseFont = true;
            this.gvChat.Appearance.Preview.Options.UseForeColor = true;
            this.gvChat.Appearance.Row.BackColor = Color.Black;
            this.gvChat.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0xb2);
            this.gvChat.Appearance.Row.ForeColor = Color.White;
            this.gvChat.Appearance.Row.Options.UseBackColor = true;
            this.gvChat.Appearance.Row.Options.UseFont = true;
            this.gvChat.Appearance.Row.Options.UseForeColor = true;
            this.gvChat.Appearance.RowSeparator.BackColor = Color.Black;
            this.gvChat.Appearance.RowSeparator.BackColor2 = Color.Black;
            this.gvChat.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvChat.Appearance.SelectedRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvChat.Appearance.SelectedRow.BackColor2 = Color.Black;
            this.gvChat.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvChat.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvChat.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvChat.Appearance.SelectedRow.Options.UseFont = true;
            this.gvChat.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvChat.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvChat.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvChat.Appearance.TopNewRow.Options.UseFont = true;
            this.gvChat.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvChat.Appearance.VertLine.BackColor = Color.Yellow;
            this.gvChat.Appearance.VertLine.Options.UseBackColor = true;
            this.gvChat.BorderStyle = BorderStyles.NoBorder;
            this.gvChat.Columns.AddRange(new GridColumn[] { this.colIcon, this.colPlayer, this.colText, this.gcVisible, this.colTimeStamp });
            this.gvChat.FocusRectStyle = DrawFocusRectStyle.None;
            this.gvChat.GridControl = this.gpgChatGrid;
            this.gvChat.GroupPanelText = "<LOC>Drag a column header here to group by that column.";
            this.gvChat.Name = "gvChat";
            this.gvChat.OptionsDetail.AllowZoomDetail = false;
            this.gvChat.OptionsDetail.EnableMasterViewMode = false;
            this.gvChat.OptionsDetail.ShowDetailTabs = false;
            this.gvChat.OptionsDetail.SmartDetailExpand = false;
            this.gvChat.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvChat.OptionsSelection.MultiSelect = true;
            this.gvChat.OptionsView.RowAutoHeight = true;
            this.gvChat.OptionsView.ShowColumnHeaders = false;
            this.gvChat.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            this.gvChat.OptionsView.ShowGroupPanel = false;
            this.gvChat.OptionsView.ShowHorzLines = false;
            this.gvChat.OptionsView.ShowIndicator = false;
            this.gvChat.OptionsView.ShowVertLines = false;
            this.gvChat.PaintStyleName = "Web";
            this.gvChat.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvChat_CustomDrawCell);
            this.gvChat.MouseDown += new MouseEventHandler(this.gvChat_MouseDown);
            this.gvChat.MouseUp += new MouseEventHandler(this.gvChat_MouseUp);
            this.gvChat.MouseMove += new MouseEventHandler(this.gvChat_MouseMove);
            this.gvChat.CalcRowHeight += new RowHeightEventHandler(this.gvChat_CalcRowHeight);
            this.colIcon.Caption = "<LOC>Player Icon";
            this.colIcon.ColumnEdit = this.rimPictureEdit3;
            this.colIcon.FieldName = "Icon";
            this.colIcon.MinWidth = 0x2a;
            this.colIcon.Name = "colIcon";
            this.colIcon.OptionsColumn.AllowEdit = false;
            this.colIcon.OptionsColumn.FixedWidth = true;
            this.colIcon.OptionsColumn.ReadOnly = true;
            this.colIcon.Width = 0x2a;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.colPlayer.Caption = "<LOC>Player Name";
            this.colPlayer.ColumnEdit = this.rimMemoEdit3;
            this.colPlayer.FieldName = "PlayerInfo";
            this.colPlayer.Name = "colPlayer";
            this.colPlayer.OptionsColumn.AllowEdit = false;
            this.colPlayer.OptionsColumn.FixedWidth = true;
            this.colPlayer.OptionsColumn.ReadOnly = true;
            this.colPlayer.Visible = true;
            this.colPlayer.VisibleIndex = 0;
            this.colPlayer.Width = 150;
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
            this.colText.Caption = "<LOC>Chat Content";
            this.colText.ColumnEdit = this.rimMemoEdit3;
            this.colText.FieldName = "Text";
            this.colText.Name = "colText";
            this.colText.OptionsColumn.AllowEdit = false;
            this.colText.OptionsColumn.ReadOnly = true;
            this.colText.Visible = true;
            this.colText.VisibleIndex = 1;
            this.colText.Width = 0x188;
            this.gcVisible.Caption = "Visible";
            this.gcVisible.FieldName = "IsVisible";
            this.gcVisible.Name = "gcVisible";
            this.gcVisible.OptionsColumn.ShowInCustomizationForm = false;
            this.colTimeStamp.AppearanceCell.ForeColor = Color.White;
            this.colTimeStamp.AppearanceCell.Options.UseForeColor = true;
            this.colTimeStamp.Caption = "<LOC>Time";
            this.colTimeStamp.FieldName = "TimeStamp";
            this.colTimeStamp.Name = "colTimeStamp";
            this.colTimeStamp.OptionsColumn.AllowEdit = false;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.gpgContextMenuChatText.MenuItems.AddRange(new MenuItem[] { 
                this.ciChatText_Clear, this.ciChatText_Copy, this.ciChatText_Filter, this.miShowColumns, this.ciChatText_ShowEmotes, this.menuItm15, this.ciChatText_PrivateMsg, this.ciChatText_Ignore, this.ciChatText_Unignore, this.ciChatText_ViewRank, this.ciChatText_WebStats, this.ciChatText_ViewPlayer, this.ciChatText_Replays, this.menuItem6, this.ciChatText_FriendInvite, this.ciChatText_FriendRemove, 
                this.menuItem11, this.ciChatText_ClanInvite, this.ciChatText_ClanRequest, this.ciChatText_ClanRemove, this.ciChatText_Promote, this.ciChatText_Demote, this.ciChatText_ViewClan, this.ciChatText_LeaveClan, this.menuItem18, this.ciChatText_Kick, this.ciChatText_Ban, this.menuItem12, this.ciChatText_Solution, this.miTranslate
             });
            this.ciChatText_Clear.Index = 0;
            this.ciChatText_Clear.Text = "<LOC>Clear Chat";
            this.ciChatText_Clear.Click += new EventHandler(this.ciChatText_Clear_Click);
            this.ciChatText_Copy.Index = 1;
            this.ciChatText_Copy.Text = "<LOC>Copy to Clipboard";
            this.ciChatText_Copy.Click += new EventHandler(this.ciChatText_Copy_Click);
            this.ciChatText_Filter.Index = 2;
            this.ciChatText_Filter.MenuItems.AddRange(new MenuItem[] { this.ciChatText_Filter_Self, this.ciChatText_Filter_System, this.ciChatText_Filter_Events, this.ciChatText_Filter_Errors, this.ciChatText_Filter_Game, this.ciChatText_Filter_Friends, this.ciChatText_Filter_Clan, this.ciChatText_Filter_Admin, this.ciChatText_Filter_Other, this.menuItem4, this.ciChatText_Filters_Reset });
            this.ciChatText_Filter.Text = "<LOC>Filter by";
            this.ciChatText_Filter_Self.Checked = true;
            this.ciChatText_Filter_Self.Index = 0;
            this.ciChatText_Filter_Self.Text = "<LOC>Self";
            this.ciChatText_Filter_System.Checked = true;
            this.ciChatText_Filter_System.Index = 1;
            this.ciChatText_Filter_System.Text = "<LOC>System Messages";
            this.ciChatText_Filter_Events.Checked = true;
            this.ciChatText_Filter_Events.Index = 2;
            this.ciChatText_Filter_Events.Text = "<LOC>System Events";
            this.ciChatText_Filter_Errors.Checked = true;
            this.ciChatText_Filter_Errors.Index = 3;
            this.ciChatText_Filter_Errors.Text = "<LOC>System Errors";
            this.ciChatText_Filter_Game.Checked = true;
            this.ciChatText_Filter_Game.Index = 4;
            this.ciChatText_Filter_Game.Text = "<LOC>Game Events";
            this.ciChatText_Filter_Friends.Checked = true;
            this.ciChatText_Filter_Friends.Index = 5;
            this.ciChatText_Filter_Friends.Text = "<LOC>Friends";
            this.ciChatText_Filter_Clan.Checked = true;
            this.ciChatText_Filter_Clan.Index = 6;
            this.ciChatText_Filter_Clan.Text = "<LOC>Clan";
            this.ciChatText_Filter_Admin.Checked = true;
            this.ciChatText_Filter_Admin.Index = 7;
            this.ciChatText_Filter_Admin.Text = "<LOC>Admins";
            this.ciChatText_Filter_Other.Checked = true;
            this.ciChatText_Filter_Other.Index = 8;
            this.ciChatText_Filter_Other.Text = "<LOC>Other";
            this.menuItem4.Index = 9;
            this.menuItem4.Text = "-";
            this.ciChatText_Filters_Reset.Index = 10;
            this.ciChatText_Filters_Reset.Text = "<LOC>Reset Filters";
            this.miShowColumns.Index = 3;
            this.miShowColumns.Text = "<LOC>Show Columns";
            this.ciChatText_ShowEmotes.Index = 4;
            this.ciChatText_ShowEmotes.Text = "<LOC>Show emotes";
            this.menuItm15.Index = 5;
            this.menuItm15.Text = "-";
            this.ciChatText_PrivateMsg.Index = 6;
            this.ciChatText_PrivateMsg.Text = "<LOC>Send private message";
            this.ciChatText_Ignore.Index = 7;
            this.ciChatText_Ignore.Text = "<LOC>Ignore this player";
            this.ciChatText_Unignore.Index = 8;
            this.ciChatText_Unignore.Text = "<LOC>Unignore this player";
            this.ciChatText_ViewRank.Index = 9;
            this.ciChatText_ViewRank.Text = "<LOC>View in ranking ladder";
            this.ciChatText_ViewRank.Visible = false;
            this.ciChatText_WebStats.Index = 10;
            this.ciChatText_WebStats.Text = "<LOC>View web statistics";
            this.ciChatText_ViewPlayer.Index = 11;
            this.ciChatText_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChatText_Replays.Index = 12;
            this.ciChatText_Replays.Text = "<LOC>View this player's Replays";
            this.menuItem6.Index = 13;
            this.menuItem6.Text = "-";
            this.ciChatText_FriendInvite.Index = 14;
            this.ciChatText_FriendInvite.Text = "<LOC>Invite this player to join Friends list";
            this.ciChatText_FriendRemove.Index = 15;
            this.ciChatText_FriendRemove.Text = "<LOC>Remove this player from Friends list";
            this.menuItem11.Index = 0x10;
            this.menuItem11.Text = "-";
            this.ciChatText_ClanInvite.Index = 0x11;
            this.ciChatText_ClanInvite.Text = "<LOC>Invite this player to join clan";
            this.ciChatText_ClanRequest.Index = 0x12;
            this.ciChatText_ClanRequest.Text = "<LOC>Request to join this player's clan";
            this.ciChatText_ClanRemove.Index = 0x13;
            this.ciChatText_ClanRemove.Text = "<LOC>Remove this player from clan";
            this.ciChatText_Promote.Index = 20;
            this.ciChatText_Promote.Text = "<LOC>Promote";
            this.ciChatText_Demote.Index = 0x15;
            this.ciChatText_Demote.Text = "<LOC>Demote";
            this.ciChatText_ViewClan.Index = 0x16;
            this.ciChatText_ViewClan.Text = "<LOC>View this clan's profile";
            this.ciChatText_LeaveClan.Index = 0x17;
            this.ciChatText_LeaveClan.Text = "<LOC>Leave clan";
            this.menuItem18.Index = 0x18;
            this.menuItem18.Text = "-";
            this.ciChatText_Kick.Index = 0x19;
            this.ciChatText_Kick.Text = "<LOC>Kick";
            this.ciChatText_Ban.Index = 0x1a;
            this.ciChatText_Ban.Text = "<LOC>Ban";
            this.menuItem12.Index = 0x1b;
            this.menuItem12.Text = "-";
            this.ciChatText_Solution.Index = 0x1c;
            this.ciChatText_Solution.Text = "<LOC>Point user to solution";
            this.miTranslate.Index = 0x1d;
            this.miTranslate.MenuItems.AddRange(new MenuItem[] { this.menuItem23, this.menuItem24, this.menuItem25, this.menuItem26, this.menuItem27, this.menuItem28, this.menuItem29 });
            this.miTranslate.Text = "Translate to English";
            this.miTranslate.Visible = false;
            this.menuItem23.Index = 0;
            this.menuItem23.Text = "German ";
            this.menuItem24.Index = 1;
            this.menuItem24.Text = "French";
            this.menuItem25.Index = 2;
            this.menuItem25.Text = "Itialian";
            this.menuItem26.Index = 3;
            this.menuItem26.Text = "Spanish";
            this.menuItem27.Index = 4;
            this.menuItem27.Text = "Russian";
            this.menuItem28.Index = 5;
            this.menuItem28.Text = "Korean";
            this.menuItem29.Index = 6;
            this.menuItem29.Text = "Japanese";
            this.gpgContextMenuEmote.MenuItems.AddRange(new MenuItem[] { this.ciEmote_Manager, this.menuItem13, this.ciEmote_Hide, this.ciEmote_Share, this.ciEmote_Animate, this.menuItem9, this.ciEmote_Delete });
            this.ciEmote_Manager.Index = 0;
            this.ciEmote_Manager.Text = "<LOC>Emote manager...";
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "-";
            this.ciEmote_Hide.Index = 2;
            this.ciEmote_Hide.Text = "<LOC>Hide emotes";
            this.ciEmote_Share.Index = 3;
            this.ciEmote_Share.Text = "<LOC>Share emotes";
            this.ciEmote_Animate.Index = 4;
            this.ciEmote_Animate.Text = "<LOC>Animate emotes";
            this.menuItem9.Index = 5;
            this.menuItem9.Text = "-";
            this.ciEmote_Delete.Index = 6;
            this.ciEmote_Delete.Text = "<LOC>Delete emote";
            this.gpgPanel1.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel1.BorderThickness = 2;
            this.gpgPanel1.Controls.Add(this.splitContainerChatAndInput);
            this.gpgPanel1.Dock = DockStyle.Fill;
            this.gpgPanel1.DrawBorder = true;
            this.gpgPanel1.Location = new Point(0, 0);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(0x197, 230);
            this.gpgPanel1.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gpgPanel1);
            base.Name = "ChatPanel";
            base.Size = new Size(0x197, 230);
            this.splitContainerChatAndInput.Panel1.ResumeLayout(false);
            this.splitContainerChatAndInput.Panel1.PerformLayout();
            this.splitContainerChatAndInput.Panel2.ResumeLayout(false);
            this.splitContainerChatAndInput.ResumeLayout(false);
            this.textBoxMsg.Properties.EndInit();
            this.gpgChatGrid.EndInit();
            this.gvChat.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimMemoEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.gpgPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override void Localize()
        {
            base.Localize();
            this.gpgContextMenuChatText.Localize();
            this.gpgContextMenuEmote.Localize();
        }

        private void Messaging_MessageRecieved(MessageEventArgs e)
        {
            VGen1 method = null;
            if ((!base.Disposing && !base.IsDisposed) && base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate (object objargs) {
                        this.DoMessaging_MessageRecieved(e);
                    };
                }
                base.BeginInvoke(method, new object[] { e });
            }
            else
            {
                this.DoMessaging_MessageRecieved(e);
            }
        }

        private void OnFrameChange(object s, EventArgs e)
        {
        }

        private void OnSendMessage(string message)
        {
            if (this.SendChatMessage != null)
            {
                this.SendChatMessage(message);
            }
        }

        private void OnStyleChatLine(TextLine line)
        {
            if (line != null)
            {
                User tag = line.Tag as User;
                if (tag != null)
                {
                    ITextEffect effect = null;
                    if (this.StyleChatLine != null)
                    {
                        effect = this.StyleChatLine(this, tag, line);
                    }
                    if (effect == null)
                    {
                        effect = new FontColorEffect(Program.Settings.Chat.Appearance.DefaultColor, Program.Settings.Chat.Appearance.DefaultFont);
                    }
                    line.Effect = effect;
                }
            }
        }

        private void RecieveChatEffect(string playerName, string effectName)
        {
            if ((effectName == null) || (effectName.Length < 1))
            {
                this.PlayerChatEffects[playerName] = null;
            }
            else
            {
                ChatEffectBase base2;
                if (ChatEffects.TryFindByDescription(effectName, out base2))
                {
                }
            }
        }

        private void RefreshEmoteAnimations()
        {
            try
            {
                if (ConfigSettings.GetBool("AnimateEmotes", true))
                {
                    foreach (Emote emote in Emote.AllEmotes.Values)
                    {
                        if (!(emote.Animated || !ImageAnimator.CanAnimate(emote.Image)))
                        {
                            emote.Animated = true;
                            emote.CanAnimate = true;
                            emote.AnimationFrames = emote.Image.GetFrameCount(new FrameDimension(emote.Image.FrameDimensionsList[0]));
                            ImageAnimator.Animate(emote.Image, new EventHandler(this.OnFrameChange));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void SendMessage()
        {
            if (base.MainForm.Connected)
            {
                if ((this.textBoxMsg.Text != null) && (this.textBoxMsg.Text.Trim().Length > 0))
                {
                    this.textBoxMsg.Text = this.textBoxMsg.Text.TrimEnd(new char[] { ' ' });
                    this.ChatHistory.AddFirst(this.textBoxMsg.Text);
                    while (this.ChatHistory.Count > Program.Settings.Chat.ChatHistoryLength)
                    {
                        this.ChatHistory.RemoveLast();
                    }
                    this.HistoryIndex = -1;
                    if (this.textBoxMsg.Text.StartsWith("/"))
                    {
                        string[] sourceArray = this.textBoxMsg.Text.TrimEnd(new char[] { ' ' }).Split(new char[] { ' ' });
                        this.textBoxMsg.Text = "";
                        if (sourceArray[0] == "/?")
                        {
                            foreach (UserAction action in UserAction.AllActions)
                            {
                                this.SystemMessage(action.ToString(), new object[0]);
                            }
                        }
                        else if (!UserAction.TryGetByPartialName(sourceArray[0], out action))
                        {
                            this.SystemMessage("<LOC>Unknown command. Enter /? to see a list of valid commands.", new object[0]);
                        }
                        else
                        {
                            string[] destinationArray = new string[sourceArray.Length - 1];
                            if (destinationArray.Length > 0)
                            {
                                Array.ConstrainedCopy(sourceArray, 1, destinationArray, 0, destinationArray.Length);
                            }
                            if (action.CheckFormat(destinationArray))
                            {
                                base.MainForm.ProcessUserAction(action.Command, destinationArray);
                            }
                            else
                            {
                                this.SystemMessage("<LOC>Improperly formatted command, the proper format for this command is:", new object[0]);
                                this.SystemMessage(action.ToString(), new object[0]);
                            }
                        }
                    }
                    else
                    {
                        string text = this.textBoxMsg.Text;
                        this.textBoxMsg.Text = "";
                        ChatLink[] linkArray = ChatLink.FindLinks(text, ChatLink.Emote);
                        for (int i = 0; i < linkArray.Length; i++)
                        {
                            string charSequence = EmoteLinkMask.GetCharSequence(linkArray[i]);
                            if ((charSequence == null) || (charSequence.Length < 1))
                            {
                                text = text.Replace(ChatLink.Emote.LinkWord, "");
                            }
                            else if (Emote.AllEmotes.ContainsKey(charSequence))
                            {
                                text = text.Replace(linkArray[i].FullUrl, charSequence);
                            }
                        }
                        this.AddChat(User.Current, text);
                        this.gvChat.MoveLastVisible();
                        if (ConfigSettings.GetBool("Emotes", true) && Program.Settings.Chat.Emotes.AutoShareEmotes)
                        {
                            ChatLink[] linkArray2 = ChatLink.FindLinks(text);
                            foreach (Emote emote in Emote.AllEmotes.Values)
                            {
                                int index = text.IndexOf(emote.CharSequence);
                                string str3 = string.Format("{0}\"{1}{2}", ChatLink.Emote.LinkWord, User.Current.Name, '\x0003');
                                while (index >= 0)
                                {
                                    bool flag = false;
                                    foreach (ChatLink link in linkArray2)
                                    {
                                        if ((index > link.StartIndex) && (index < link.EndIndex))
                                        {
                                            flag = true;
                                        }
                                    }
                                    if (flag)
                                    {
                                        index = text.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                    }
                                    else
                                    {
                                        text = text.Insert(index, str3);
                                        text = text.Insert((index + str3.Length) + emote.CharSequence.Length, "\"");
                                        index = text.IndexOf(emote.CharSequence, (int) ((index + str3.Length) + emote.CharSequence.Length));
                                    }
                                }
                            }
                        }
                        base.MainForm.OnAction();
                        this.CheckUserState();
                        this.OnSendMessage(text);
                    }
                }
            }
            else
            {
                this.SystemMessage(Loc.Get("<LOC>*** YOU HAVE LOST YOUR GPGNET CONNECTION ***"), new object[0]);
            }
        }

        internal void SendMessage(string msg)
        {
            if (base.MainForm.Connected)
            {
                if ((msg != null) && (msg.Trim().Length > 0))
                {
                    msg = msg.TrimEnd(new char[] { ' ' });
                    this.ChatHistory.AddFirst(msg);
                    while (this.ChatHistory.Count > Program.Settings.Chat.ChatHistoryLength)
                    {
                        this.ChatHistory.RemoveLast();
                    }
                    this.HistoryIndex = -1;
                    if (msg.StartsWith("/"))
                    {
                        string[] sourceArray = msg.TrimEnd(new char[] { ' ' }).Split(new char[] { ' ' });
                        msg = "";
                        if (sourceArray[0] == "/?")
                        {
                            foreach (UserAction action in UserAction.AllActions)
                            {
                                this.SystemMessage(action.ToString(), new object[0]);
                            }
                        }
                        else if (!UserAction.TryGetByPartialName(sourceArray[0], out action))
                        {
                            this.SystemMessage("<LOC>Unknown command. Enter /? to see a list of valid commands.", new object[0]);
                        }
                        else
                        {
                            string[] destinationArray = new string[sourceArray.Length - 1];
                            if (destinationArray.Length > 0)
                            {
                                Array.ConstrainedCopy(sourceArray, 1, destinationArray, 0, destinationArray.Length);
                            }
                            if (action.CheckFormat(destinationArray))
                            {
                                base.MainForm.ProcessUserAction(action.Command, destinationArray);
                            }
                            else
                            {
                                this.SystemMessage("<LOC>Improperly formatted command, the proper format for this command is:", new object[0]);
                                this.SystemMessage(action.ToString(), new object[0]);
                            }
                        }
                    }
                    else
                    {
                        ChatLink[] linkArray = ChatLink.FindLinks(msg, ChatLink.Emote);
                        for (int i = 0; i < linkArray.Length; i++)
                        {
                            string charSequence = EmoteLinkMask.GetCharSequence(linkArray[i]);
                            if ((charSequence == null) || (charSequence.Length < 1))
                            {
                                msg = msg.Replace(ChatLink.Emote.LinkWord, "");
                            }
                            else if (Emote.AllEmotes.ContainsKey(charSequence))
                            {
                                msg = msg.Replace(linkArray[i].FullUrl, charSequence);
                            }
                        }
                        this.AddChat(User.Current, msg);
                        this.gvChat.MoveLastVisible();
                        if (ConfigSettings.GetBool("Emotes", true) && Program.Settings.Chat.Emotes.AutoShareEmotes)
                        {
                            ChatLink[] linkArray2 = ChatLink.FindLinks(msg);
                            foreach (Emote emote in Emote.AllEmotes.Values)
                            {
                                int index = msg.IndexOf(emote.CharSequence);
                                string str2 = string.Format("{0}\"{1}{2}", ChatLink.Emote.LinkWord, User.Current.Name, '\x0003');
                                while (index >= 0)
                                {
                                    bool flag = false;
                                    foreach (ChatLink link in linkArray2)
                                    {
                                        if ((index > link.StartIndex) && (index < link.EndIndex))
                                        {
                                            flag = true;
                                        }
                                    }
                                    if (flag)
                                    {
                                        index = msg.IndexOf(emote.CharSequence, (int) (index + emote.CharSequence.Length));
                                    }
                                    else
                                    {
                                        msg = msg.Insert(index, str2);
                                        msg = msg.Insert((index + str2.Length) + emote.CharSequence.Length, "\"");
                                        index = msg.IndexOf(emote.CharSequence, (int) ((index + str2.Length) + emote.CharSequence.Length));
                                    }
                                }
                            }
                        }
                        base.MainForm.OnAction();
                        this.CheckUserState();
                        this.OnSendMessage(msg);
                    }
                }
            }
            else
            {
                this.SystemMessage(Loc.Get("<LOC>*** YOU HAVE LOST YOUR GPGNET CONNECTION ***"), new object[0]);
            }
        }

        internal void SystemEvent(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            this.AddChat(User.Event, msg);
        }

        internal void SystemMessage(string msg, params object[] args)
        {
            msg = Loc.Get(msg);
            if ((args != null) && (args.Length > 0))
            {
                msg = string.Format(msg, args);
            }
            this.AddChat(User.System, msg);
        }

        private void textBoxMsg_EditValueChanging(object sender, ChangingEventArgs e)
        {
            e.NewValue = e.NewValue.ToString().Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }

        public void textBoxMsg_KeyDown(object sender, KeyEventArgs e)
        {
            IEnumerator enumerator;
            int num;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (!this.textBoxMsg.Text.StartsWith("/"))
                    {
                        if (this.HistoryIndex < this.ChatHistory.Count)
                        {
                            this.HistoryIndex++;
                            enumerator = this.ChatHistory.GetEnumerator();
                            for (num = 0; enumerator.MoveNext(); num++)
                            {
                                if (num == this.HistoryIndex)
                                {
                                    this.textBoxMsg.Text = enumerator.Current.ToString();
                                    this.textBoxMsg.Select(0, 0);
                                    e.Handled = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                    this.SelectingTextList = true;
                    this.gpgTextListCommands.Focus();
                    this.SelectingTextList = false;
                    break;

                case Keys.Right:
                    return;

                case Keys.Down:
                    if (this.HistoryIndex > -1)
                    {
                        this.HistoryIndex--;
                        if (this.HistoryIndex != -1)
                        {
                            enumerator = this.ChatHistory.GetEnumerator();
                            for (num = 0; enumerator.MoveNext(); num++)
                            {
                                if (num == this.HistoryIndex)
                                {
                                    this.textBoxMsg.Text = enumerator.Current.ToString();
                                    this.textBoxMsg.Select(0, 0);
                                    e.Handled = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            this.textBoxMsg.Text = "";
                        }
                    }
                    e.SuppressKeyPress = true;
                    return;

                case Keys.Space:
                    if ((this.textBoxMsg.Text.StartsWith("/") && (this.textBoxMsg.Text.Split(new char[] { ' ' }).Length < 2)) && (this.gpgTextListCommands.SelectedValue != null))
                    {
                        this.CommandSelected(this.gpgTextListCommands, new TextValEventArgs(this.gpgTextListCommands.SelectedValue));
                    }
                    return;

                case Keys.Return:
                    if ((this.textBoxMsg.Text.StartsWith("/") && (this.textBoxMsg.Text.Split(new char[] { ' ' }).Length < 2)) && (this.gpgTextListCommands.SelectedValue != null))
                    {
                        UserAction action;
                        if (UserAction.TryGetByName(this.textBoxMsg.Text, out action))
                        {
                            this.SendMessage();
                            e.SuppressKeyPress = true;
                        }
                        else
                        {
                            this.CommandSelected(this.gpgTextListCommands, new TextValEventArgs(this.gpgTextListCommands.SelectedValue));
                            this.textBoxMsg.Text = this.textBoxMsg.Text + " ";
                            this.textBoxMsg.Select(this.textBoxMsg.Text.Length, 0);
                        }
                    }
                    else
                    {
                        this.HistoryIndex = -1;
                        this.SendMessage();
                        e.SuppressKeyPress = true;
                    }
                    return;

                case Keys.Escape:
                    this.textBoxMsg.Text = "";
                    this.HistoryIndex = -1;
                    return;

                default:
                    return;
            }
            e.SuppressKeyPress = true;
        }

        private void textBoxMsg_LostFocus(object sender, EventArgs e)
        {
            if (!((!this.gpgTextListCommands.Visible || this.SelectingTextList) || this.gpgTextListCommands.Bounds.Contains(base.PointToClient(Cursor.Position))))
            {
                this.gpgTextListCommands.Visible = false;
            }
        }

        private void UpdateCommandPopup(object sender, EventArgs e)
        {
            try
            {
                if ((this.textBoxMsg.Text != null) && this.textBoxMsg.Text.StartsWith("/"))
                {
                    this.gpgTextListCommands.TextLines = UserAction.GetFilteredActions(this.textBoxMsg.Text);
                    for (int i = 0; i < this.gpgTextListCommands.TextLines.Length; i++)
                    {
                        UserAction action = this.gpgTextListCommands.TextLines[i] as UserAction;
                        for (int j = 0; j < action.CommandWords.Length; j++)
                        {
                            if (action.CommandWords[j].ToLower() == this.textBoxMsg.Text.ToLower())
                            {
                                this.gpgTextListCommands.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    this.gpgTextListCommands.Visible = this.gpgTextListCommands.TextLines.Length > 0;
                }
                else
                {
                    this.gpgTextListCommands.Visible = false;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public LinkedList<string> ChatHistory
        {
            get
            {
                return this.mChatHistory;
            }
        }

        public MappedObjectList<User> ChatParticipants
        {
            get
            {
                return this.mChatParticipants;
            }
        }

        public User SelectedChatTextSender
        {
            get
            {
                try
                {
                    int[] selectedRows = this.gvChat.GetSelectedRows();
                    if (selectedRows.Length > 0)
                    {
                        TextLine row = this.gvChat.GetRow(selectedRows[0]) as TextLine;
                        if ((row != null) && ((row.Tag != null) && (row.Tag is User)))
                        {
                            return (row.Tag as User);
                        }
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

        public GridView SelectedParticipantView
        {
            get
            {
                return this.mSelectedParticipantView;
            }
        }

        public bool ShowSlashCommands
        {
            get
            {
                return this.mShowSlashCommands;
            }
            set
            {
                this.mShowSlashCommands = value;
                this.gpgTextListCommands.Visible = value;
            }
        }
    }
}

