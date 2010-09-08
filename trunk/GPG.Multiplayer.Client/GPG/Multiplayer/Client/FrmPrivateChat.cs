namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Grid.ViewInfo;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class FrmPrivateChat : DlgBase
    {
        public ToolStripMenuItem btnAddFriend;
        public ToolStripMenuItem btnClanInvite;
        public ToolStripMenuItem btnIgnore;
        public ToolStripMenuItem btnInviteToTeam;
        public ToolStripMenuItem btnMore;
        public ToolStripMenuItem btnRemoveFriend;
        private Dictionary<int, int> ChatRowHeights;
        private Dictionary<int, Rectangle> ChatRowPoints;
        private GridColumn colIcon;
        private GridColumn colPlayer;
        private GridColumn colText;
        private IContainer components;
        private GridColumn gcVisible;
        private GPGChatGrid gpgChatGrid;
        private GridView gvPrivateChat;
        private int HistoryIndex;
        private LinkedList<string> mChatHistory;
        private BindingList<ChatLine> mChatLines;
        private User mChatTarget;
        private List<ToolStripItem> mCustomPaint;
        private bool mFirstChatDraw;
        private bool mShowWithoutActivation;
        private GPGMenuStrip msQuickButtons;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        private SplitContainer splitContainer1;
        private GPGTextBox textBoxMsg;
        private bool ToolstripSizeChanged;

        public FrmPrivateChat(FrmMain mainForm, string targetName) : base(mainForm)
        {
            this.components = null;
            this.mShowWithoutActivation = true;
            this.ToolstripSizeChanged = false;
            this.mCustomPaint = new List<ToolStripItem>();
            this.mChatLines = new BindingList<ChatLine>();
            this.ChatRowPoints = new Dictionary<int, Rectangle>();
            this.ChatRowHeights = new Dictionary<int, int>();
            this.mFirstChatDraw = true;
            this.mChatHistory = new LinkedList<string>();
            this.HistoryIndex = -1;
            this.InitializeComponent();
            this.mShowWithoutActivation = true;
            this.Construct(targetName, null);
        }

        public FrmPrivateChat(FrmMain mainForm, string targetName, string msg) : base(mainForm)
        {
            this.components = null;
            this.mShowWithoutActivation = true;
            this.ToolstripSizeChanged = false;
            this.mCustomPaint = new List<ToolStripItem>();
            this.mChatLines = new BindingList<ChatLine>();
            this.ChatRowPoints = new Dictionary<int, Rectangle>();
            this.ChatRowHeights = new Dictionary<int, int>();
            this.mFirstChatDraw = true;
            this.mChatHistory = new LinkedList<string>();
            this.HistoryIndex = -1;
            this.InitializeComponent();
            this.mShowWithoutActivation = false;
            this.Construct(targetName, msg);
        }

        private void AddChat(IUser user, string message)
        {
            VGen0 method = null;
            VGen0 gen2 = null;
            bool scroll = true;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        scroll = !this.gvPrivateChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvPrivateChat);
                    };
                }
                base.Invoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                scroll = !this.gvPrivateChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvPrivateChat);
            }
            message = Profanity.MaskProfanity(message);
            ChatLine line = new ChatLine(this.gpgChatGrid);
            line.Tag = user;
            line.PlayerInfo = user.Name;
            line.Text = message;
            line.TextFont = this.gpgChatGrid.Font;
            line.TextColor = Program.Settings.Chat.Appearance.DefaultColor;
            line.Filters[this.ChatTarget.Name] = true;
            if (user.IsSystem)
            {
                if (user.Equals(User.System))
                {
                    line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.SystemColor, Program.Settings.Chat.Appearance.SystemFont);
                }
                else if (user.Equals(User.Event))
                {
                    line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.EventColor, Program.Settings.Chat.Appearance.EventFont);
                }
                else if (user.Equals(User.Error))
                {
                    line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.ErrorColor, Program.Settings.Chat.Appearance.ErrorFont);
                }
            }
            else if (user.IsCurrent)
            {
                line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.PrivateMessaging.SelfColor, Program.Settings.Chat.Appearance.PrivateMessaging.SelfFont);
            }
            else
            {
                line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.PrivateMessaging.OtherColor, Program.Settings.Chat.Appearance.PrivateMessaging.OtherFont);
            }
            if (base.InvokeRequired)
            {
                if (!base.Disposing && !base.IsDisposed)
                {
                    if (gen2 == null)
                    {
                        gen2 = delegate {
                            this.mChatLines.Add(line);
                            if (scroll)
                            {
                                this.gvPrivateChat.MoveLastVisible();
                            }
                        };
                    }
                    base.BeginInvoke(gen2);
                }
            }
            else
            {
                this.mChatLines.Add(line);
                if (scroll)
                {
                    this.gvPrivateChat.MoveLastVisible();
                }
                if (Form.ActiveForm != this)
                {
                    base.FlashWindow();
                }
            }
        }

        protected override void BindToSettings()
        {
            base.BindToSettings();
            Program.Settings.Chat.Appearance.ChatLineSpacingChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.gvPrivateChat.RowSeparatorHeight = Program.Settings.Chat.Appearance.ChatLineSpacing;
            };
            Program.Settings.Chat.Appearance.PrivateMessaging.AppearanceChanged += new PropertyChangedEventHandler(this.StyleChatroom);
        }

        private void btnAddFriend_Click(object sender, EventArgs e)
        {
            base.MainForm.InviteAsFriend(this.ChatTarget.Name);
            this.RefreshToolstrip();
        }

        private void btnClanInvite_Click(object sender, EventArgs e)
        {
            base.MainForm.InviteToClan(this.ChatTarget.Name);
            this.RefreshToolstrip();
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            base.MainForm.IgnorePlayer(this.ChatTarget.Name);
            this.RefreshToolstrip();
        }

        private void btnInviteToTeam_Click(object sender, EventArgs e)
        {
            base.MainForm.InviteToTeamGame(this.ChatTarget.Name);
            this.RefreshToolstrip();
        }

        private void btnRemoveFriend_Click(object sender, EventArgs e)
        {
            base.MainForm.RemoveFriend(this.ChatTarget.Name);
            this.RefreshToolstrip();
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
                        this.gvPrivateChat.Columns["PlayerInfo"].Width = w;
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.gvPrivateChat.Columns["PlayerInfo"].Width = w;
            }
        }

        private void CalculateNameColumnWidth()
        {
            if (!this.gpgChatGrid.Disposing && !this.gpgChatGrid.IsDisposed)
            {
                GridView gvPrivateChat = this.gvPrivateChat;
                using (Graphics graphics = this.gpgChatGrid.CreateGraphics())
                {
                    VGen0 method = null;
                    int w = -1;
                    int num = 0;
                    int num2 = 0;
                    for (int i = 0; i < gvPrivateChat.RowCount; i++)
                    {
                        int visibleRowHandle = gvPrivateChat.GetVisibleRowHandle(i);
                        if (this.gvPrivateChat.IsRowVisible(visibleRowHandle) != RowVisibleState.Hidden)
                        {
                            ChatLine row = gvPrivateChat.GetRow(visibleRowHandle) as ChatLine;
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
                                if (this.gvPrivateChat.Columns["PlayerInfo"].Width != w)
                                {
                                    this.gvPrivateChat.Columns["PlayerInfo"].Width = w;
                                    this.gpgChatGrid.Refresh();
                                }
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (this.gvPrivateChat.Columns["PlayerInfo"].Width != w)
                    {
                        this.gvPrivateChat.Columns["PlayerInfo"].Width = w;
                        this.gpgChatGrid.Refresh();
                    }
                    EventLog.WriteLine("Measured {0} lines", new object[] { num2 });
                }
            }
        }

        private void CheckControl(Control control)
        {
            foreach (Control control2 in control.Controls)
            {
                this.CheckControl(control2);
            }
            if (!((control is TextBoxMaskBox) || control.Equals(this.textBoxMsg)))
            {
                control.KeyPress += new KeyPressEventHandler(this.control_KeyPress);
            }
        }

        private void Construct(string targetName, string msg)
        {
            MouseEventHandler handler = null;
            if (!base.MainForm.TryFindUser(targetName, true, out this.mChatTarget))
            {
                base.MainForm.ErrorMessage("<LOC>Unable to locate player {0}.", new object[] { targetName });
                base.Dispose();
            }
            else
            {
                this.Text = string.Format(Loc.Get("<LOC>{0}: Private message session"), targetName);
                this.textBoxMsg.KeyDown += new KeyEventHandler(this.textBoxMsg_KeyDown);
                base.KeyDown += new KeyEventHandler(this.FrmPrivateChat_KeyDown);
                this.gvPrivateChat.Columns["IsVisible"].FilterInfo = new ColumnFilterInfo("[IsVisible] = true");
                this.gvPrivateChat.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvChat_CustomDrawCell);
                if (GameInformation.SelectedGame.IsSpaceSiege)
                {
                    this.btnInviteToTeam.Visible = false;
                }
                GameInformation.OnSelectedGameChange += new EventHandler(this.GameInformation_OnSelectedGameChange);
                this.gpgChatGrid.Filters[this.ChatTarget.Name] = true;
                this.gpgChatGrid.DataSource = this.ChatLines;
                this.CheckControl(this);
                if (handler == null)
                {
                    handler = delegate (object s, MouseEventArgs e) {
                        int[] selectedRows = this.gvPrivateChat.GetSelectedRows();
                        for (int j = 0; j < selectedRows.Length; j++)
                        {
                            this.gvPrivateChat.UnselectRow(selectedRows[j]);
                        }
                    };
                }
                GPGGridView.ScrollBarClick(this.gvPrivateChat, handler);
                this.gvPrivateChat.RowSeparatorHeight = Program.Settings.Chat.Appearance.ChatLineSpacing;
                if (msg != null)
                {
                    this.SendMessage(msg);
                }
                if (ConfigSettings.GetBool("Minimize Private Window", false))
                {
                    base.WindowState = FormWindowState.Minimized;
                }
            }
        }

        private void control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(base.ActiveControl is TextBoxMaskBox))
            {
                base.ActiveControl = this.textBoxMsg;
                this.textBoxMsg.Select();
                if (TextUtil.IsDisplayChar(e.KeyChar))
                {
                    this.textBoxMsg.Text = this.textBoxMsg.Text + e.KeyChar;
                }
                this.textBoxMsg.SelectionStart = this.textBoxMsg.Text.Length;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void ErrorMessage(string msg, params object[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                this.AddChat(User.Error, string.Format(Loc.Get(msg), args));
            }
            else
            {
                this.AddChat(User.Error, Loc.Get(msg));
            }
        }

        public override void FocusInput()
        {
            base.ActiveControl = this.textBoxMsg;
            this.textBoxMsg.Select();
            this.textBoxMsg.SelectionStart = this.textBoxMsg.Text.Length;
            base.Activate();
            base.BringToFront();
        }

        private void FrmPrivateChat_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Escape) && !this.textBoxMsg.Focused)
            {
                base.Close();
            }
        }

        private void GameInformation_OnSelectedGameChange(object sender, EventArgs e)
        {
            if (GameInformation.SelectedGame.IsSpaceSiege)
            {
                this.btnInviteToTeam.Visible = false;
            }
            else
            {
                this.btnInviteToTeam.Visible = true;
            }
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
                    ChatLine row = this.gvPrivateChat.GetRow(e.RowHandle) as ChatLine;
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
                                foreach (Emote emote in Emote.AllEmotes.Values)
                                {
                                    index = str.IndexOf(emote.CharSequence);
                                    while (index >= 0)
                                    {
                                        list2.Add(new MultiVal<int, int>(index, emote.CharSequence.Length));
                                        list3.Add(index, emote);
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
                                ChatLink link;
                                SolidBrush brush;
                                string[] strArray;
                                int num11;
                                SizeF ef;
                                Brush brush2;
                                Font font2;
                                if ((list3 != null) && (list3.Count > 0))
                                {
                                    int num2;
                                    list2.Add(new MultiVal<int, int>(str.Length, 0));
                                    list3.Add(str.Length, null);
                                    SortedList<int, MultiVal<int, int>> list4 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                    list4[-1] = new MultiVal<int, int>(0, 0);
                                    foreach (MultiVal<int, int> val in list2)
                                    {
                                        list4[val.Value1] = val;
                                    }
                                    for (num2 = 1; num2 < list4.Count; num2++)
                                    {
                                        int num3 = list4.Values[num2 - 1].Value1;
                                        index = list4.Values[num2].Value1;
                                        int num4 = list4.Values[num2 - 1].Value2;
                                        int num5 = list4.Values[num2].Value2;
                                        list.Add(str.Substring(num3 + num4, index - (num3 + num4)));
                                    }
                                    dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str2));
                                    num6 = textFont.Height + 3;
                                    num7 = 0f;
                                    num8 = num6;
                                    num9 = this.colText.VisibleWidth - 10;
                                    num10 = 0;
                                    link = null;
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
                                                        link = dictionary[num10];
                                                    }
                                                    if (link != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link.LinkColor))
                                                        {
                                                            using (font2 = new Font(textFont, FontStyle.Underline))
                                                            {
                                                                ef = DrawUtil.MeasureString(graphics, strArray[num11], font2);
                                                                if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                                {
                                                                    num7 = 0f;
                                                                    num8 += num6;
                                                                }
                                                                goto Label_05BC;
                                                            }
                                                        }
                                                    }
                                                    ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                    if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                    {
                                                        num7 = 0f;
                                                        num8 += num6;
                                                    }
                                                Label_05BC:
                                                    num10 += strArray[num11].Length;
                                                    num7 += ef.Width;
                                                    if ((link != null) && (num10 >= link.EndIndex))
                                                    {
                                                        link = null;
                                                    }
                                                }
                                            }
                                            if (list3.Values[num2] != null)
                                            {
                                                float num12 = ((float) textFont.Height) / ((float) list3.Values[num2].Image.Height);
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
                                    link = null;
                                    using (brush = new SolidBrush(text.TextColor))
                                    {
                                        strArray = DrawUtil.SplitString(str, " ");
                                        num11 = 0;
                                        while (num11 < strArray.Length)
                                        {
                                            if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                            {
                                                link = dictionary[num10];
                                            }
                                            if (link != null)
                                            {
                                                using (brush2 = new SolidBrush(link.LinkColor))
                                                {
                                                    using (font2 = new Font(textFont, FontStyle.Underline))
                                                    {
                                                        ef = DrawUtil.MeasureString(graphics, strArray[num11], font2);
                                                        if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                                        {
                                                            num7 = 0f;
                                                            num8 += num6;
                                                        }
                                                        goto Label_0890;
                                                    }
                                                }
                                            }
                                            ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                            if (((ef.Width <= num9) || (num7 != 0f)) && ((num7 + ef.Width) > num9))
                                            {
                                                num7 = 0f;
                                                num8 += num6;
                                            }
                                        Label_0890:
                                            num10 += strArray[num11].Length;
                                            num7 += ef.Width;
                                            if ((link != null) && (num10 >= link.EndIndex))
                                            {
                                                link = null;
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
                    Program.Settings.StylePreferences.StyleControl(this.gvPrivateChat, EventArgs.Empty);
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
                        ChatLine row = this.gvPrivateChat.GetRow(e.RowHandle) as ChatLine;
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
                                Font font2;
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
                                        foreach (Emote emote in Emote.AllEmotes.Values)
                                        {
                                            index = str.IndexOf(emote.CharSequence);
                                            while (index >= 0)
                                            {
                                                bool flag = false;
                                                if (dictionary != null)
                                                {
                                                    foreach (ChatLink link in dictionary.Values)
                                                    {
                                                        if ((index > link.StartIndex) && (index < link.EndIndex))
                                                        {
                                                            flag = true;
                                                        }
                                                    }
                                                }
                                                if (!flag)
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
                                    SortedList<int, MultiVal<int, int>> list4 = new SortedList<int, MultiVal<int, int>>(list3.Count);
                                    list4[-1] = new MultiVal<int, int>(0, 0);
                                    foreach (MultiVal<int, int> val in list3)
                                    {
                                        list4[val.Value1] = val;
                                    }
                                    for (num = 1; num < list4.Count; num++)
                                    {
                                        int num3 = list4.Values[num - 1].Value1;
                                        index = list4.Values[num].Value1;
                                        int num4 = list4.Values[num - 1].Value2;
                                        int num5 = list4.Values[num].Value2;
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
                                                                using (font2 = new Font(textFont, FontStyle.Underline))
                                                                {
                                                                    ef = DrawUtil.MeasureString(e.Graphics, link2.DisplayText + " ", font2);
                                                                    if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                                    {
                                                                        x = bounds.X;
                                                                        y += num6;
                                                                    }
                                                                    e.Graphics.DrawString(link2.DisplayText, font2, brush2, x, y);
                                                                }
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
                                                float num12 = ((float) textFont.Height) / ((float) list.Values[num].Image.Height);
                                                float width = list.Values[num].Image.Width * num12;
                                                if ((x + width) > num9)
                                                {
                                                    x = bounds.X;
                                                    y += num6;
                                                }
                                                e.Graphics.DrawImage(list.Values[num].Image, x, y, width, (float) textFont.Height);
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
                                                        using (font2 = new Font(textFont, FontStyle.Underline))
                                                        {
                                                            ef = DrawUtil.MeasureString(e.Graphics, link2.DisplayText + " ", font2);
                                                            if (((ef.Width <= (num9 - bounds.Left)) || (x != bounds.Left)) && ((x + ef.Width) > num9))
                                                            {
                                                                x = bounds.X;
                                                                y += num6;
                                                            }
                                                            e.Graphics.DrawString(link2.DisplayText, font2, brush2, x, y);
                                                        }
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
                                    foreach (Emote emote in Emote.AllEmotes.Values)
                                    {
                                        index = str.IndexOf(emote.CharSequence);
                                        while (index >= 0)
                                        {
                                            bool flag = false;
                                            if (dictionary != null)
                                            {
                                                foreach (ChatLink link in dictionary.Values)
                                                {
                                                    if ((index > link.StartIndex) && (index < link.EndIndex))
                                                    {
                                                        flag = true;
                                                    }
                                                }
                                            }
                                            if (!flag)
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
                                    Font font2;
                                    RectangleF ef2;
                                    if ((list3 != null) && (list3.Count > 0))
                                    {
                                        int num2;
                                        list2.Add(new MultiVal<int, int>(str.Length, 0));
                                        list3.Add(str.Length, null);
                                        SortedList<int, MultiVal<int, int>> list4 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                        list4[-1] = new MultiVal<int, int>(0, 0);
                                        foreach (MultiVal<int, int> val in list2)
                                        {
                                            list4[val.Value1] = val;
                                        }
                                        for (num2 = 1; num2 < list4.Count; num2++)
                                        {
                                            int num3 = list4.Values[num2 - 1].Value1;
                                            index = list4.Values[num2].Value1;
                                            int num4 = list4.Values[num2 - 1].Value2;
                                            int num5 = list4.Values[num2].Value2;
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
                                                                    using (font2 = new Font(textFont, FontStyle.Underline))
                                                                    {
                                                                        ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", font2);
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
                                                    float num12 = ((float) textFont.Height) / ((float) list3.Values[num2].Image.Height);
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
                                    else
                                    {
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                        if ((dictionary != null) && (dictionary.Count > 0))
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
                                                                using (font2 = new Font(textFont, FontStyle.Underline))
                                                                {
                                                                    ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", font2);
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
                                    foreach (Emote emote in Emote.AllEmotes.Values)
                                    {
                                        index = str.IndexOf(emote.CharSequence);
                                        while (index >= 0)
                                        {
                                            bool flag = false;
                                            if (dictionary != null)
                                            {
                                                foreach (ChatLink link in dictionary.Values)
                                                {
                                                    if ((index > link.StartIndex) && (index < link.EndIndex))
                                                    {
                                                        flag = true;
                                                    }
                                                }
                                            }
                                            if (!flag)
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
                                    Font font2;
                                    RectangleF ef2;
                                    if ((list3 != null) && (list3.Count > 0))
                                    {
                                        int num2;
                                        list2.Add(new MultiVal<int, int>(str.Length, 0));
                                        list3.Add(str.Length, null);
                                        SortedList<int, MultiVal<int, int>> list4 = new SortedList<int, MultiVal<int, int>>(list2.Count);
                                        list4[-1] = new MultiVal<int, int>(0, 0);
                                        foreach (MultiVal<int, int> val in list2)
                                        {
                                            list4[val.Value1] = val;
                                        }
                                        for (num2 = 1; num2 < list4.Count; num2++)
                                        {
                                            int num3 = list4.Values[num2 - 1].Value1;
                                            index = list4.Values[num2].Value1;
                                            int num4 = list4.Values[num2 - 1].Value2;
                                            int num5 = list4.Values[num2].Value2;
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
                                                                    using (font2 = new Font(textFont, FontStyle.Underline))
                                                                    {
                                                                        ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", font2);
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
                                                    float num12 = ((float) textFont.Height) / ((float) list3.Values[num2].Image.Height);
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
                                    else
                                    {
                                        dictionary = ChatLink.CreateCharacterIndex(ChatLink.FindLinks(str));
                                        if ((dictionary != null) && (dictionary.Count > 0))
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
                                                    if (link2 != null)
                                                    {
                                                        goto Label_0A82;
                                                    }
                                                    if ((dictionary.Count > 0) && dictionary.ContainsKey(num10))
                                                    {
                                                        link2 = dictionary[num10];
                                                    }
                                                    if (link2 != null)
                                                    {
                                                        using (brush2 = new SolidBrush(link2.LinkColor))
                                                        {
                                                            using (font2 = new Font(textFont, FontStyle.Underline))
                                                            {
                                                                ef = DrawUtil.MeasureString(graphics, link2.DisplayText + " ", font2);
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
                                                                        return;
                                                                    }
                                                                    if (e.Button == MouseButtons.Right)
                                                                    {
                                                                        return;
                                                                    }
                                                                }
                                                                goto Label_0A64;
                                                            }
                                                        }
                                                    }
                                                    ef = DrawUtil.MeasureString(graphics, strArray[num11], textFont);
                                                    if (((ef.Width <= (num9 - rectangle.Left)) || (x != rectangle.Left)) && ((x + ef.Width) > num9))
                                                    {
                                                        x = rectangle.X;
                                                        y += num6;
                                                    }
                                                Label_0A64:
                                                    num10 += strArray[num11].Length;
                                                    x += ef.Width;
                                                    goto Label_0A93;
                                                Label_0A82:
                                                    num10 += strArray[num11].Length;
                                                Label_0A93:
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
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            GridLevelNode node = new GridLevelNode();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FrmPrivateChat));
            this.splitContainer1 = new SplitContainer();
            this.gpgChatGrid = new GPGChatGrid();
            this.gvPrivateChat = new GridView();
            this.colIcon = new GridColumn();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.colPlayer = new GridColumn();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.colText = new GridColumn();
            this.gcVisible = new GridColumn();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.textBoxMsg = new GPGTextBox();
            this.msQuickButtons = new GPGMenuStrip(this.components);
            this.btnAddFriend = new ToolStripMenuItem();
            this.btnRemoveFriend = new ToolStripMenuItem();
            this.btnIgnore = new ToolStripMenuItem();
            this.btnClanInvite = new ToolStripMenuItem();
            this.btnInviteToTeam = new ToolStripMenuItem();
            this.btnMore = new ToolStripMenuItem();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gpgChatGrid.BeginInit();
            this.gvPrivateChat.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.textBoxMsg.Properties.BeginInit();
            this.msQuickButtons.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x192, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.splitContainer1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainer1.FixedPanel = FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new Point(12, 0x53);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add(this.gpgChatGrid);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel1, null);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxMsg);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel2, null);
            this.splitContainer1.Size = new Size(0x1b6, 0xcc);
            this.splitContainer1.SplitterDistance = 0xb2;
            this.splitContainer1.SplitterWidth = 1;
            base.ttDefault.SetSuperTip(this.splitContainer1, null);
            this.splitContainer1.TabIndex = 4;
            this.gpgChatGrid.CustomizeStyle = true;
            this.gpgChatGrid.Dock = DockStyle.Fill;
            this.gpgChatGrid.EmbeddedNavigator.Name = "";
            this.gpgChatGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgChatGrid.IgnoreMouseWheel = false;
            node.RelationName = "Level1";
            this.gpgChatGrid.LevelTree.Nodes.AddRange(new GridLevelNode[] { node });
            this.gpgChatGrid.Location = new Point(0, 0);
            this.gpgChatGrid.MainView = this.gvPrivateChat;
            this.gpgChatGrid.Name = "gpgChatGrid";
            this.gpgChatGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3 });
            this.gpgChatGrid.ShowOnlyPredefinedDetails = true;
            this.gpgChatGrid.Size = new Size(0x1b6, 0xb2);
            this.gpgChatGrid.TabIndex = 11;
            this.gpgChatGrid.ViewCollection.AddRange(new BaseView[] { this.gvPrivateChat });
            this.gvPrivateChat.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvPrivateChat.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvPrivateChat.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvPrivateChat.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvPrivateChat.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvPrivateChat.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvPrivateChat.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvPrivateChat.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvPrivateChat.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvPrivateChat.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.Empty.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.Empty.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPrivateChat.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvPrivateChat.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPrivateChat.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvPrivateChat.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvPrivateChat.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvPrivateChat.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPrivateChat.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvPrivateChat.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvPrivateChat.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvPrivateChat.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.FocusedCell.Options.UseFont = true;
            this.gvPrivateChat.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.FocusedRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvPrivateChat.Appearance.FocusedRow.BackColor2 = Color.Black;
            this.gvPrivateChat.Appearance.FocusedRow.Font = new Font("Arial", 9.75f, FontStyle.Bold);
            this.gvPrivateChat.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.FocusedRow.Options.UseFont = true;
            this.gvPrivateChat.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvPrivateChat.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvPrivateChat.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvPrivateChat.Appearance.FooterPanel.Options.UseFont = true;
            this.gvPrivateChat.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.GroupButton.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvPrivateChat.Appearance.GroupButton.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvPrivateChat.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvPrivateChat.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvPrivateChat.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvPrivateChat.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.GroupPanel.BackColor2 = Color.White;
            this.gvPrivateChat.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvPrivateChat.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.GroupPanel.Options.UseFont = true;
            this.gvPrivateChat.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvPrivateChat.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvPrivateChat.Appearance.GroupRow.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.GroupRow.Options.UseFont = true;
            this.gvPrivateChat.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvPrivateChat.Appearance.HeaderPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvPrivateChat.Appearance.HeaderPanel.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPrivateChat.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPrivateChat.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.HideSelectionRow.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvPrivateChat.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPrivateChat.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvPrivateChat.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.HorzLine.BackColor = Color.Yellow;
            this.gvPrivateChat.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.Preview.BackColor = Color.White;
            this.gvPrivateChat.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvPrivateChat.Appearance.Preview.ForeColor = Color.Purple;
            this.gvPrivateChat.Appearance.Preview.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.Preview.Options.UseFont = true;
            this.gvPrivateChat.Appearance.Preview.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.Row.BackColor = Color.Black;
            this.gvPrivateChat.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0xb2);
            this.gvPrivateChat.Appearance.Row.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.Row.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.Row.Options.UseFont = true;
            this.gvPrivateChat.Appearance.Row.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.RowSeparator.BackColor = Color.White;
            this.gvPrivateChat.Appearance.RowSeparator.BackColor2 = Color.White;
            this.gvPrivateChat.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.SelectedRow.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.gvPrivateChat.Appearance.SelectedRow.BackColor2 = Color.Black;
            this.gvPrivateChat.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvPrivateChat.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPrivateChat.Appearance.SelectedRow.Options.UseFont = true;
            this.gvPrivateChat.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvPrivateChat.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvPrivateChat.Appearance.TopNewRow.Options.UseFont = true;
            this.gvPrivateChat.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvPrivateChat.Appearance.VertLine.BackColor = Color.Yellow;
            this.gvPrivateChat.Appearance.VertLine.Options.UseBackColor = true;
            this.gvPrivateChat.BorderStyle = BorderStyles.NoBorder;
            this.gvPrivateChat.Columns.AddRange(new GridColumn[] { this.colIcon, this.colPlayer, this.colText, this.gcVisible });
            this.gvPrivateChat.GridControl = this.gpgChatGrid;
            this.gvPrivateChat.Name = "gvPrivateChat";
            this.gvPrivateChat.OptionsDetail.AllowZoomDetail = false;
            this.gvPrivateChat.OptionsDetail.EnableMasterViewMode = false;
            this.gvPrivateChat.OptionsDetail.ShowDetailTabs = false;
            this.gvPrivateChat.OptionsDetail.SmartDetailExpand = false;
            this.gvPrivateChat.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvPrivateChat.OptionsSelection.MultiSelect = true;
            this.gvPrivateChat.OptionsView.RowAutoHeight = true;
            this.gvPrivateChat.OptionsView.ShowColumnHeaders = false;
            this.gvPrivateChat.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            this.gvPrivateChat.OptionsView.ShowGroupPanel = false;
            this.gvPrivateChat.OptionsView.ShowHorzLines = false;
            this.gvPrivateChat.OptionsView.ShowIndicator = false;
            this.gvPrivateChat.OptionsView.ShowVertLines = false;
            this.gvPrivateChat.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvChat_CustomDrawCell);
            this.gvPrivateChat.MouseUp += new MouseEventHandler(this.gvChat_MouseUp);
            this.gvPrivateChat.CalcRowHeight += new RowHeightEventHandler(this.gvChat_CalcRowHeight);
            this.gvPrivateChat.MouseMove += new MouseEventHandler(this.gvChat_MouseMove);
            this.gvPrivateChat.MouseDown += new MouseEventHandler(this.gvChat_MouseDown);
            this.colIcon.Caption = "Player Icon";
            this.colIcon.ColumnEdit = this.rimPictureEdit3;
            this.colIcon.FieldName = "Icon";
            this.colIcon.Name = "colIcon";
            this.colIcon.OptionsColumn.AllowEdit = false;
            this.colIcon.OptionsColumn.FixedWidth = true;
            this.colIcon.OptionsColumn.ReadOnly = true;
            this.colIcon.Width = 40;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.colPlayer.Caption = "Player Name";
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
            this.colText.Caption = "Chat Content";
            this.colText.ColumnEdit = this.rimMemoEdit3;
            this.colText.FieldName = "Text";
            this.colText.Name = "colText";
            this.colText.OptionsColumn.AllowEdit = false;
            this.colText.OptionsColumn.ReadOnly = true;
            this.colText.Visible = true;
            this.colText.VisibleIndex = 1;
            this.colText.Width = 0x120;
            this.gcVisible.Caption = "gcVisible";
            this.gcVisible.FieldName = "IsVisible";
            this.gcVisible.Name = "gcVisible";
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.textBoxMsg.Dock = DockStyle.Bottom;
            this.textBoxMsg.Location = new Point(0, 5);
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Properties.Appearance.BackColor = Color.Black;
            this.textBoxMsg.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.textBoxMsg.Properties.Appearance.ForeColor = Color.White;
            this.textBoxMsg.Properties.Appearance.Options.UseBackColor = true;
            this.textBoxMsg.Properties.Appearance.Options.UseBorderColor = true;
            this.textBoxMsg.Properties.Appearance.Options.UseForeColor = true;
            this.textBoxMsg.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.textBoxMsg.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.textBoxMsg.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.textBoxMsg.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.textBoxMsg.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.textBoxMsg.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.textBoxMsg.Properties.BorderStyle = BorderStyles.Simple;
            this.textBoxMsg.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.textBoxMsg.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.textBoxMsg.Properties.MaxLength = 300;
            this.textBoxMsg.Size = new Size(0x1b6, 20);
            this.textBoxMsg.TabIndex = 0;
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.msQuickButtons.AutoSize = false;
            this.msQuickButtons.BackgroundImage = (Image) manager.GetObject("msQuickButtons.BackgroundImage");
            this.msQuickButtons.Dock = DockStyle.None;
            this.msQuickButtons.GripMargin = new Padding(0);
            this.msQuickButtons.ImageScalingSize = new Size(0x2d, 0x2d);
            this.msQuickButtons.Items.AddRange(new ToolStripItem[] { this.btnAddFriend, this.btnRemoveFriend, this.btnIgnore, this.btnClanInvite, this.btnInviteToTeam, this.btnMore });
            this.msQuickButtons.Location = new Point(9, 0xd1);
            this.msQuickButtons.Name = "msQuickButtons";
            this.msQuickButtons.Padding = new Padding(0, 0, 10, 0);
            this.msQuickButtons.RenderMode = ToolStripRenderMode.Professional;
            this.msQuickButtons.ShowItemToolTips = true;
            this.msQuickButtons.Size = new Size(0x1b9, 0x34);
            base.ttDefault.SetSuperTip(this.msQuickButtons, null);
            this.msQuickButtons.TabIndex = 7;
            this.msQuickButtons.Paint += new PaintEventHandler(this.msQuickButtons_Paint);
            this.msQuickButtons.SizeChanged += new EventHandler(this.msQuickButtons_SizeChanged);
            this.btnAddFriend.AutoSize = false;
            this.btnAddFriend.AutoToolTip = true;
            this.btnAddFriend.Image = (Image) manager.GetObject("btnAddFriend.Image");
            this.btnAddFriend.ImageScaling = ToolStripItemImageScaling.None;
            this.btnAddFriend.Name = "btnAddFriend";
            this.btnAddFriend.ShortcutKeys = Keys.F2;
            this.btnAddFriend.Size = new Size(0x25, 0x34);
            this.btnAddFriend.ToolTipText = "<LOC>Add to Friends List";
            this.btnAddFriend.Click += new EventHandler(this.btnAddFriend_Click);
            this.btnRemoveFriend.AutoSize = false;
            this.btnRemoveFriend.AutoToolTip = true;
            this.btnRemoveFriend.Image = (Image) manager.GetObject("btnRemoveFriend.Image");
            this.btnRemoveFriend.ImageScaling = ToolStripItemImageScaling.None;
            this.btnRemoveFriend.Name = "btnRemoveFriend";
            this.btnRemoveFriend.ShortcutKeys = Keys.F8;
            this.btnRemoveFriend.Size = new Size(0x25, 0x34);
            this.btnRemoveFriend.ToolTipText = "<LOC>Remove From Friends List";
            this.btnRemoveFriend.Click += new EventHandler(this.btnRemoveFriend_Click);
            this.btnIgnore.AutoSize = false;
            this.btnIgnore.AutoToolTip = true;
            this.btnIgnore.Image = (Image) manager.GetObject("btnIgnore.Image");
            this.btnIgnore.ImageScaling = ToolStripItemImageScaling.None;
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.ShortcutKeys = Keys.F7;
            this.btnIgnore.Size = new Size(0x25, 0x34);
            this.btnIgnore.ToolTipText = "<LOC>Ignore This Person";
            this.btnIgnore.Click += new EventHandler(this.btnIgnore_Click);
            this.btnClanInvite.AutoSize = false;
            this.btnClanInvite.AutoToolTip = true;
            this.btnClanInvite.Image = (Image) manager.GetObject("btnClanInvite.Image");
            this.btnClanInvite.ImageScaling = ToolStripItemImageScaling.None;
            this.btnClanInvite.Name = "btnClanInvite";
            this.btnClanInvite.ShortcutKeys = Keys.F8;
            this.btnClanInvite.Size = new Size(0x25, 0x34);
            this.btnClanInvite.ToolTipText = "<LOC>Invite To Clan";
            this.btnClanInvite.Click += new EventHandler(this.btnClanInvite_Click);
            this.btnInviteToTeam.AutoSize = false;
            this.btnInviteToTeam.AutoToolTip = true;
            this.btnInviteToTeam.Image = (Image) manager.GetObject("btnInviteToTeam.Image");
            this.btnInviteToTeam.ImageScaling = ToolStripItemImageScaling.None;
            this.btnInviteToTeam.Name = "btnInviteToTeam";
            this.btnInviteToTeam.ShortcutKeys = Keys.F8;
            this.btnInviteToTeam.Size = new Size(0x25, 0x34);
            this.btnInviteToTeam.ToolTipText = "<LOC>Invite To Arranged Team";
            this.btnInviteToTeam.Click += new EventHandler(this.btnInviteToTeam_Click);
            this.btnMore.AutoSize = false;
            this.btnMore.AutoToolTip = true;
            this.btnMore.Image = (Image) manager.GetObject("btnMore.Image");
            this.btnMore.ImageScaling = ToolStripItemImageScaling.None;
            this.btnMore.Name = "btnMore";
            this.btnMore.ShortcutKeys = Keys.F6;
            this.btnMore.Size = new Size(20, 0x34);
            this.btnMore.ToolTipText = "<LOC>More...";
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x1cd, 0x156);
            base.Controls.Add(this.msQuickButtons);
            base.Controls.Add(this.splitContainer1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(200, 0xdd);
            base.Name = "FrmPrivateChat";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "FrmPrivateChat";
            base.Controls.SetChildIndex(this.splitContainer1, 0);
            base.Controls.SetChildIndex(this.msQuickButtons, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.gpgChatGrid.EndInit();
            this.gvPrivateChat.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimMemoEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.textBoxMsg.Properties.EndInit();
            this.msQuickButtons.ResumeLayout(false);
            this.msQuickButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void item_Paint(object sender, PaintEventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
            e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(this.msQuickButtons.BackgroundImage.Width, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
            if (item.Enabled)
            {
                if (item.Selected)
                {
                    e.Graphics.DrawImage(DrawUtil.AdjustColors(0.8f, 0.8f, 1f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                }
                else
                {
                    e.Graphics.DrawImage(item.Image, new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                }
            }
            else
            {
                e.Graphics.DrawImage(DrawUtil.GetTransparentImage(0.5f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
            }
        }

        private void msQuickButtons_Paint(object sender, PaintEventArgs e)
        {
            if (this.ToolstripSizeChanged)
            {
                int num2;
                foreach (ToolStripItem item in this.btnMore.DropDownItems)
                {
                    item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                }
                int count = this.btnMore.DropDown.Items.Count;
                for (num2 = 0; num2 < count; num2++)
                {
                    this.msQuickButtons.Items.Insert(this.msQuickButtons.Items.Count - 1, this.btnMore.DropDown.Items[0]);
                }
                int num3 = 0;
                foreach (ToolStripItem item in this.msQuickButtons.Items)
                {
                    if (!this.mCustomPaint.Contains(item))
                    {
                        this.mCustomPaint.Add(item);
                        item.Paint += new PaintEventHandler(this.item_Paint);
                    }
                    if (item != this.btnMore)
                    {
                        int num4 = ((item.Bounds.Right + this.btnMore.Width) + item.Padding.Right) + this.btnMore.Padding.Horizontal;
                        if (num4 > this.msQuickButtons.Width)
                        {
                            num3++;
                        }
                    }
                }
                if (num3 > 0)
                {
                    this.btnMore.Visible = true;
                    this.btnMore.DropDownDirection = ToolStripDropDownDirection.AboveRight;
                    this.btnMore.DropDown.AutoSize = false;
                    this.btnMore.DropDown.Width = this.msQuickButtons.Items[0].Width;
                    this.btnMore.DropDown.Height = (this.msQuickButtons.Items[0].Height * num3) + 3;
                    this.btnMore.DropDown.Padding = new Padding(0);
                    this.btnMore.DropDown.Margin = new Padding(0);
                    for (num2 = 0; num2 < num3; num2++)
                    {
                        this.btnMore.DropDownItems.Add(this.msQuickButtons.Items[(this.msQuickButtons.Items.Count - 1) - (num3 - num2)]);
                    }
                    foreach (ToolStripItem item in this.btnMore.DropDownItems)
                    {
                        item.BackgroundImage = this.btnMore.DropDown.BackgroundImage;
                    }
                }
                else if (this.ToolstripSizeChanged)
                {
                    this.btnMore.Visible = false;
                }
                this.ToolstripSizeChanged = false;
            }
        }

        private void msQuickButtons_SizeChanged(object sender, EventArgs e)
        {
            this.ToolstripSizeChanged = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.msQuickButtons.Location = new Point(0, 0);
            this.msQuickButtons.Width = base.pbBottom.Width - 0x12;
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            base.pbBottom.Controls.Add(this.msQuickButtons);
            this.RefreshToolstrip();
        }

        public void OnMessageRecieved(string message)
        {
            this.AddChat(this.mChatTarget, message);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            VGen0 method = null;
            base.OnSizeChanged(e);
            if (this.gvPrivateChat != null)
            {
                if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            GPGGridView.ScrollToBottom(this.gvPrivateChat);
                        };
                    }
                    base.BeginInvoke(method);
                }
                else if (!(base.Disposing || base.IsDisposed))
                {
                    GPGGridView.ScrollToBottom(this.gvPrivateChat);
                }
            }
        }

        private void ProcessUserAction(UserActionCommands action, string[] args)
        {
            try
            {
                this.SystemMessage("<LOC>Unknown command; some chat commands are not enabled in private messages.", new object[0]);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void RefreshToolstrip()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RefreshToolstrip();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    this.btnAddFriend.Enabled = false;
                    this.btnRemoveFriend.Enabled = false;
                    this.btnClanInvite.Enabled = false;
                    this.btnIgnore.Enabled = false;
                    this.btnAddFriend.Enabled = !this.ChatTarget.IsFriend;
                    this.btnRemoveFriend.Enabled = this.ChatTarget.IsFriend;
                    this.btnClanInvite.Enabled = (!this.ChatTarget.IsInClan && User.Current.IsInClan) && ClanMember.Current.CanTargetAbility(ClanAbility.Invite, ClanMember.Current);
                    this.btnIgnore.Enabled = !this.ChatTarget.IsIgnored;
                    if ((((((base.MainForm.DlgSelectGame != null) && !base.MainForm.DlgSelectGame.Disposing) && !base.MainForm.DlgSelectGame.IsDisposed) || (!User.Current.IsAdmin && !base.MainForm.IsGameCurrent)) || (base.MainForm.IsInGame || (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable))) || ((base.MainForm.DlgTeamGame != null) && (((base.MainForm.DlgTeamGame.Team.FindMember(this.ChatTarget.Name) != null) || (base.MainForm.DlgTeamGame.Team.TeamMembers.Count >= TeamGame.MAX_TEAM_MEMBERS)) || !base.MainForm.DlgTeamGame.Team.TeamLeader.IsSelf)))
                    {
                        this.btnInviteToTeam.Enabled = false;
                    }
                    else
                    {
                        this.btnInviteToTeam.Enabled = true;
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void SendMessage()
        {
            this.SendMessage(this.textBoxMsg.Text);
            this.textBoxMsg.Text = "";
        }

        internal void SendMessage(string text)
        {
            if ((text != null) && (text.Trim().Length > 0))
            {
                this.ChatHistory.AddFirst(text);
                while (this.ChatHistory.Count > Program.Settings.Chat.ChatHistoryLength)
                {
                    this.ChatHistory.RemoveLast();
                }
                this.HistoryIndex = -1;
                this.AddChat(User.Current, text);
                this.gvPrivateChat.MoveLastVisible();
                if (this.ChatTarget.Online)
                {
                    Messaging.SendCustomCommand(this.mChatTarget.Name, CustomCommands.Whisper, new object[] { text });
                }
                else
                {
                    base.MainForm.PingPlayer(this.ChatTarget.Name);
                    this.SystemMessage("<LOC>{0} is offline and will not receive messages.", new object[] { this.ChatTarget.Name });
                }
            }
        }

        public override void SetSkin(string name)
        {
            base.SetSkin(name);
            this.msQuickButtons.BackgroundImage = SkinManager.GetImage(@"Dialog\ButtonStrip\bottom.png");
            this.msQuickButtons.Height = this.msQuickButtons.BackgroundImage.Height;
            this.btnMore.DropDown.BackgroundImage = DrawUtil.ResizeImage(SkinManager.GetImage("brushbg.png"), this.msQuickButtons.Items[0].Size);
            this.btnAddFriend.Image = SkinManager.GetImage(@"Dialog\PrivateMessage\add_friend.png");
            this.btnClanInvite.Image = SkinManager.GetImage(@"Dialog\PrivateMessage\clan_invite.png");
            this.btnIgnore.Image = SkinManager.GetImage(@"Dialog\PrivateMessage\ignore.png");
            this.btnRemoveFriend.Image = SkinManager.GetImage(@"Dialog\PrivateMessage\remove_friend.png");
            this.btnInviteToTeam.Image = SkinManager.GetImage(@"Dialog\PrivateMessage\team_invite.png");
            this.btnMore.Image = SkinManager.GetImage("nav-more.png");
            foreach (ToolStripItem item in this.msQuickButtons.Items)
            {
                item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                item.Height = this.msQuickButtons.BackgroundImage.Height;
            }
        }

        protected override void StyleApplication(object sender, PropertyChangedEventArgs e)
        {
            base.StyleApplication(sender, e);
            this.gvPrivateChat.Appearance.Empty.BackColor = Program.Settings.StylePreferences.MasterBackColor;
            this.gvPrivateChat.Appearance.Row.BackColor = Program.Settings.StylePreferences.MasterBackColor;
            this.gvPrivateChat.Appearance.HideSelectionRow.BackColor = Program.Settings.StylePreferences.MasterBackColor;
        }

        private void StyleChatLine(TextLine line)
        {
            if (line != null)
            {
                User tag = line.Tag as User;
                if (tag != null)
                {
                    if (tag.IsSystem)
                    {
                        if (tag.Equals(User.System))
                        {
                            line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.SystemColor, Program.Settings.Chat.Appearance.SystemFont);
                        }
                        else if (tag.Equals(User.Event))
                        {
                            line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.EventColor, Program.Settings.Chat.Appearance.EventFont);
                        }
                        else if (tag.Equals(User.Error))
                        {
                            line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.ErrorColor, Program.Settings.Chat.Appearance.ErrorFont);
                        }
                    }
                    else if (tag.IsCurrent)
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.PrivateMessaging.SelfColor, Program.Settings.Chat.Appearance.PrivateMessaging.SelfFont);
                    }
                    else
                    {
                        line.Effect = new FontColorEffect(Program.Settings.Chat.Appearance.PrivateMessaging.OtherColor, Program.Settings.Chat.Appearance.PrivateMessaging.OtherFont);
                    }
                }
            }
        }

        private void StyleChatroom(object sender, PropertyChangedEventArgs e)
        {
            foreach (ChatLine line in this.mChatLines)
            {
                this.StyleChatLine(line);
            }
            this.gvPrivateChat.Invalidate();
        }

        internal void SystemMessage(string msg, params object[] args)
        {
            if ((args != null) && (args.Length > 0))
            {
                this.AddChat(User.System, string.Format(Loc.Get(msg), args));
            }
            else
            {
                this.AddChat(User.System, Loc.Get(msg));
            }
        }

        private void textBoxMsg_KeyDown(object sender, KeyEventArgs e)
        {
            IEnumerator enumerator;
            int num;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (this.HistoryIndex < this.ChatHistory.Count)
                    {
                        this.HistoryIndex++;
                        enumerator = this.ChatHistory.GetEnumerator();
                        num = 0;
                        while (enumerator.MoveNext())
                        {
                            if (num == this.HistoryIndex)
                            {
                                this.textBoxMsg.Text = enumerator.Current.ToString();
                                this.textBoxMsg.Select(0, 0);
                                e.Handled = true;
                                break;
                            }
                            num++;
                        }
                    }
                    break;

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
                            break;
                        }
                        this.textBoxMsg.Text = "";
                    }
                    break;

                case Keys.Escape:
                    this.textBoxMsg.Text = "";
                    this.HistoryIndex = -1;
                    break;

                case Keys.Return:
                    this.SendMessage();
                    this.HistoryIndex = -1;
                    break;
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        protected override bool BottomMenuStrip
        {
            get
            {
                return true;
            }
        }

        public LinkedList<string> ChatHistory
        {
            get
            {
                return this.mChatHistory;
            }
        }

        public BindingList<ChatLine> ChatLines
        {
            get
            {
                return this.mChatLines;
            }
        }

        public User ChatTarget
        {
            get
            {
                return this.mChatTarget;
            }
            internal set
            {
                this.mChatTarget = value;
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return false;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return this.mShowWithoutActivation;
            }
        }
    }
}

