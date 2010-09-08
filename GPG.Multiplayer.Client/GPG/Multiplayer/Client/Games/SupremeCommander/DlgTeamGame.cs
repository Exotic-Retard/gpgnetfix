namespace GPG.Multiplayer.Client.Games.SupremeCommander
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
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games.SupremeCommander.icons;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgTeamGame : DlgBase
    {
        private Dictionary<int, int> ChatRowHeights;
        private Dictionary<int, Rectangle> ChatRowPoints;
        private GridColumn colIcon;
        private GridColumn colPlayer;
        private GridColumn colText;
        private IContainer components;
        private bool ForceLeave;
        private bool GameRunning;
        private GridColumn gcVisible;
        private GPGChatGrid gpgChatGrid;
        private GPGGroupBox gpgGroupBox1;
        private GPGGroupBox gpgGroupBoxMembers;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabelFaction;
        private GPGLabel gpgLabelName;
        private GPGLabel gpgLabeNonPreflMapName;
        private GPGLabel gpgLabePreflMapName;
        private GPGPictureBox gpgPictureBoxNonPrefMap;
        private GPGPictureBox gpgPictureBoxPrefMap;
        private GridView gvChat;
        private int HistoryIndex;
        private bool IsValidatingMaps;
        private LinkedList<string> mChatHistory;
        private BindingList<ChatLine> mChatLines;
        private List<PnlTeamGameMember> MemberPanels;
        private bool mFirstChatDraw;
        private TeamGame mTeam;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        private SkinButton skinButtonChangeMap;
        private SkinButton skinButtonLaunch;
        private SkinButton skinButtonLeaveTeam;
        private SkinLabel skinLabel1;
        private SkinLabel skinLabel2;
        private SkinLabel skinLabel3;
        private GPGTextBox textBoxMsg;

        public DlgTeamGame()
        {
            this.components = null;
            this.MemberPanels = new List<PnlTeamGameMember>(TeamGame.MAX_TEAM_MEMBERS);
            this.ForceLeave = false;
            this.IsValidatingMaps = false;
            this.GameRunning = false;
            this.mChatLines = new BindingList<ChatLine>();
            this.ChatRowPoints = new Dictionary<int, Rectangle>();
            this.ChatRowHeights = new Dictionary<int, int>();
            this.mFirstChatDraw = true;
            this.mChatHistory = new LinkedList<string>();
            this.HistoryIndex = -1;
            try
            {
                this.InitializeComponent();
                this.mTeam = new TeamGame(new TeamGame.TeamGameMember(User.Current.Name, User.Current.ID));
                if ((Program.Settings.SupcomPrefs.RankedGames.Maps == null) || (Program.Settings.SupcomPrefs.RankedGames.Maps.Length < 1))
                {
                    DlgSupcomMapOptions options = new DlgSupcomMapOptions(this.Team.GameType, true);
                    SupcomMapInfo[] array = new SupcomMapInfo[options.Maps.Count];
                    options.Maps.CopyTo(array, 0);
                    options.Dispose();
                    options = null;
                    Program.Settings.SupcomPrefs.RankedGames.Maps = array;
                }
                this.Construct();
                Program.Settings.SupcomPrefs.RankedGames.MapsChanged += new PropertyChangedEventHandler(this.BindToPrefs);
                Program.Settings.SupcomPrefs.RankedGames.FactionChanged += new PropertyChangedEventHandler(this.BindToPrefs);
                this.SystemMessage("<LOC>You have formed a new team.", new object[0]);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public DlgTeamGame(TeamGame team)
        {
            this.components = null;
            this.MemberPanels = new List<PnlTeamGameMember>(TeamGame.MAX_TEAM_MEMBERS);
            this.ForceLeave = false;
            this.IsValidatingMaps = false;
            this.GameRunning = false;
            this.mChatLines = new BindingList<ChatLine>();
            this.ChatRowPoints = new Dictionary<int, Rectangle>();
            this.ChatRowHeights = new Dictionary<int, int>();
            this.mFirstChatDraw = true;
            this.mChatHistory = new LinkedList<string>();
            this.HistoryIndex = -1;
            try
            {
                this.InitializeComponent();
                this.mTeam = team;
                this.Construct();
                Program.Settings.SupcomPrefs.RankedGames.FactionChanged += new PropertyChangedEventHandler(this.BindToPrefs);
                this.RefreshMaps();
                this.SystemMessage("<LOC>You have joined the team.", new object[0]);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
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
                        scroll = !this.gvChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvChat);
                    };
                }
                base.Invoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                scroll = !this.gvChat.IsFocusedView && GPGGridView.IsMaxScrolled(this.gvChat);
            }
            message = Profanity.MaskProfanity(message);
            ChatLine line = new ChatLine(this.gpgChatGrid);
            line.Tag = user;
            line.PlayerInfo = user.Name;
            line.Text = message;
            line.TextFont = this.gpgChatGrid.Font;
            line.TextColor = Program.Settings.Chat.Appearance.DefaultColor;
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
                                this.gvChat.MoveLastVisible();
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
                    this.gvChat.MoveLastVisible();
                }
            }
        }

        internal void AddMember(string name, int id)
        {
            if (this.Team.TeamMembers.Count >= TeamGame.MAX_TEAM_MEMBERS)
            {
                Messaging.SendCustomCommand(name, CustomCommands.TeamGameFull, new object[0]);
            }
            else if (this.GameLaunched)
            {
                Messaging.SendCustomCommand(name, CustomCommands.TeamGameUnavailable, new object[0]);
            }
            else
            {
                TeamGame.TeamGameMember member = new TeamGame.TeamGameMember(this.Team, name, id);
                Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameMember, new object[] { member.ToDataString() });
                base.Invoke(delegate {
                    this.UpdateMember(member);
                    this.MainForm.RefreshPMWindows();
                });
                this.ValidateMaps();
                Messaging.SendCustomCommand(name, CustomCommands.TeamGame, new object[] { this.Team.ToDataString() });
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (this.Team.GetSelf().IsTeamLeader)
                {
                    Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameDisband, new object[0]);
                    ThreadQueue.Quazal.WaitUntilEmpty(0xbb8);
                    if (this.GameLaunched)
                    {
                        this.CancelTeamAutomatch();
                    }
                }
                else
                {
                    Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameLeave, new object[0]);
                    ThreadQueue.Quazal.WaitUntilEmpty(0xbb8);
                }
                Application.ApplicationExit -= new EventHandler(this.Application_ApplicationExit);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void Automatch_GameLaunch(object sender, EventArgs e)
        {
            this.UpdateAutomatchStatus("", new object[0]);
        }

        internal void Automatch_OnExit()
        {
            this.EndAutomatch();
        }

        private void Automatch_OnStatusChanged(string text, params object[] args)
        {
            this.UpdateAutomatchStatus(text, new object[0]);
            if ((args != null) && (args.Length > 0))
            {
            }
        }

        private void BindToPrefs(object sender, PropertyChangedEventArgs e)
        {
            if (!this.IsValidatingMaps)
            {
                try
                {
                    if (this.Team.TeamLeader.IsSelf)
                    {
                        this.Team.PreferredMap = "none";
                        this.Team.NonPreferredMap = "none";
                        if (Program.Settings.SupcomPrefs.RankedGames.Maps != null)
                        {
                            foreach (SupcomMapInfo info in Program.Settings.SupcomPrefs.RankedGames.Maps)
                            {
                                if (!(!info.Priority.HasValue ? true : !info.Priority.Value))
                                {
                                    this.Team.PreferredMap = info.MapID;
                                }
                                else if (!(!info.Priority.HasValue ? true : info.Priority.Value))
                                {
                                    this.Team.NonPreferredMap = info.MapID;
                                }
                            }
                        }
                        if (this.ValidateMaps())
                        {
                            Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameMap, new object[] { this.Team.PreferredMap, this.Team.NonPreferredMap });
                            this.RefreshMaps();
                        }
                    }
                    SupcomAutomatch.GetSupcomAutomatch().Faction = Program.Settings.SupcomPrefs.RankedGames.Faction;
                    SupcomLookups._Factions any = SupcomLookups._Factions.Any;
                    if (Program.Settings.SupcomPrefs.RankedGames.Faction != "random")
                    {
                        any = (SupcomLookups._Factions) Enum.Parse(typeof(SupcomLookups._Factions), Program.Settings.SupcomPrefs.RankedGames.Faction.TrimStart(new char[] { '/' }), true);
                    }
                    if (this.Team.GetSelf().Faction != any)
                    {
                        this.Team.GetSelf().Faction = any;
                        this.UpdateMember(this.Team.GetSelf());
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        protected override void BindToSettings()
        {
            base.BindToSettings();
            Program.Settings.Chat.Appearance.ChatLineSpacingChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.gvChat.RowSeparatorHeight = Program.Settings.Chat.Appearance.ChatLineSpacing;
            };
            Program.Settings.Chat.Appearance.PrivateMessaging.AppearanceChanged += new PropertyChangedEventHandler(this.StyleChatroom);
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

        internal void CancelTeamAutomatch()
        {
            SupcomAutomatch.GetSupcomAutomatch().RemoveMatch();
            this.EndAutomatch();
            this.skinButtonLaunch.Enabled = false;
            Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameAbort, new object[0]);
            this.UpdateAutomatchStatus("<LOC>Game search cancelled.", new object[0]);
        }

        internal void ChangeMaps(string preferred, string nonPreferred)
        {
            this.Team.PreferredMap = preferred;
            this.Team.NonPreferredMap = nonPreferred;
            this.RefreshMaps();
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

        private void Construct()
        {
            this.textBoxMsg.KeyDown += new KeyEventHandler(this.textBoxMsg_KeyDown);
            base.KeyDown += new KeyEventHandler(this.FrmPrivateChat_KeyDown);
            this.gvChat.MouseDown += new MouseEventHandler(this.gvChat_MouseDown);
            this.gvChat.MouseMove += new MouseEventHandler(this.gvChat_MouseMove);
            this.gvChat.MouseUp += new MouseEventHandler(this.gvChat_MouseUp);
            this.gvChat.Columns["IsVisible"].FilterInfo = new ColumnFilterInfo("[IsVisible] = true");
            this.gvChat.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvChat_CustomDrawCell);
            this.gvChat.CalcRowHeight += new RowHeightEventHandler(this.gvChat_CalcRowHeight);
            this.gpgChatGrid.DataSource = this.ChatLines;
            this.CheckControl(this);
            GPGGridView.ScrollBarClick(this.gvChat, delegate (object s, MouseEventArgs e) {
                int[] selectedRows = this.gvChat.GetSelectedRows();
                for (int j = 0; j < selectedRows.Length; j++)
                {
                    this.gvChat.UnselectRow(selectedRows[j]);
                }
            });
            this.gvChat.RowSeparatorHeight = Program.Settings.Chat.Appearance.ChatLineSpacing;
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

        internal void DisbandTeam()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.DisbandTeam();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.ForceLeave = true;
                this.SystemMessage("<LOC>Your team game has been disbanded.", new object[0]);
                base.MainForm.SystemMessage("<LOC>Your team game has been disbanded.", new object[0]);
                base.MainForm.RefreshPMWindows();
                base.Close();
            }
        }

        private void DisbandTeam(PnlTeamGameMember sender, TeamGame.TeamGameMember member)
        {
            this.OnDisbandTeam();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EndAutomatch()
        {
            this.skinButtonLaunch.Text = Loc.Get("<LOC>Launch");
            this.GameLaunched = false;
            this.GameRunning = false;
            this.skinButtonLeaveTeam.Enabled = true;
            if (this.Team.TeamLeader.IsSelf)
            {
                foreach (PnlTeamGameMember member in this.MemberPanels)
                {
                    member.EnableButtons();
                    if ((member.TeamMember != null) && member.TeamMember.IsSelf)
                    {
                        member.Enable();
                    }
                }
            }
            else
            {
                foreach (PnlTeamGameMember member in this.MemberPanels)
                {
                    if ((member.TeamMember != null) && member.TeamMember.IsSelf)
                    {
                        member.Enable();
                        member.EnableButtons();
                        break;
                    }
                }
            }
            SupcomAutomatch.GetSupcomAutomatch().OnStatusChanged -= new StringEventHandler2(this.Automatch_OnStatusChanged);
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                VGen0 method = null;
                Thread.Sleep(0xbb8);
                this.UpdateAutomatchStatus("", new object[0]);
                if (this.Team.TeamLeader.IsSelf)
                {
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.UpdateLaunchAbility();
                                this.skinButtonChangeMap.Enabled = this.Team.TeamLeader.IsSelf;
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (!(base.Disposing || base.IsDisposed))
                    {
                        this.UpdateLaunchAbility();
                        this.skinButtonChangeMap.Enabled = this.Team.TeamLeader.IsSelf;
                    }
                }
            });
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

        private void GameLaunchedChanged()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.GameLaunchedChanged();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                if (this.GameLaunched)
                {
                    this.OnLaunchTeamAutomatch();
                }
                foreach (PnlTeamGameMember member in this.MemberPanels)
                {
                    if (member.TeamMember.IsSelf)
                    {
                        if (this.GameLaunched)
                        {
                            member.Disable();
                        }
                        else
                        {
                            member.Enable();
                        }
                        break;
                    }
                }
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
            GridLevelNode node = new GridLevelNode();
            GridLevelNode node2 = new GridLevelNode();
            this.gpgGroupBoxMembers = new GPGGroupBox();
            this.skinLabel2 = new SkinLabel();
            this.skinLabel3 = new SkinLabel();
            this.skinLabel1 = new SkinLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelFaction = new GPGLabel();
            this.gpgLabelName = new GPGLabel();
            this.gpgPictureBoxPrefMap = new GPGPictureBox();
            this.skinButtonLaunch = new SkinButton();
            this.skinButtonChangeMap = new SkinButton();
            this.textBoxMsg = new GPGTextBox();
            this.gpgChatGrid = new GPGChatGrid();
            this.gvChat = new GridView();
            this.colIcon = new GridColumn();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.colPlayer = new GridColumn();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.colText = new GridColumn();
            this.gcVisible = new GridColumn();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabePreflMapName = new GPGLabel();
            this.gpgLabeNonPreflMapName = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgPictureBoxNonPrefMap = new GPGPictureBox();
            this.gpgGroupBox1 = new GPGGroupBox();
            this.skinButtonLeaveTeam = new SkinButton();
            this.gpgGroupBoxMembers.SuspendLayout();
            ((ISupportInitialize) this.gpgPictureBoxPrefMap).BeginInit();
            this.textBoxMsg.Properties.BeginInit();
            this.gpgChatGrid.BeginInit();
            this.gvChat.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxNonPrefMap).BeginInit();
            this.gpgGroupBox1.SuspendLayout();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgGroupBoxMembers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgGroupBoxMembers.Controls.Add(this.skinLabel2);
            this.gpgGroupBoxMembers.Controls.Add(this.skinLabel3);
            this.gpgGroupBoxMembers.Controls.Add(this.skinLabel1);
            this.gpgGroupBoxMembers.Controls.Add(this.gpgLabel2);
            this.gpgGroupBoxMembers.Controls.Add(this.gpgLabel1);
            this.gpgGroupBoxMembers.Controls.Add(this.gpgLabelFaction);
            this.gpgGroupBoxMembers.Controls.Add(this.gpgLabelName);
            this.gpgGroupBoxMembers.Location = new Point(12, 0x53);
            this.gpgGroupBoxMembers.Name = "gpgGroupBoxMembers";
            this.gpgGroupBoxMembers.Size = new Size(0x239, 0xf2);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxMembers, null);
            this.gpgGroupBoxMembers.TabIndex = 7;
            this.gpgGroupBoxMembers.TabStop = false;
            this.gpgGroupBoxMembers.Text = "<LOC>Team Members";
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Transparent;
            this.skinLabel2.DrawEdges = false;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(0xcb, 0x16);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0xa6, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 5;
            this.skinLabel2.Text = "<LOC>Faction";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(0);
            this.skinLabel3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel3.AutoStyle = false;
            this.skinLabel3.BackColor = Color.Transparent;
            this.skinLabel3.DrawEdges = true;
            this.skinLabel3.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel3.ForeColor = Color.White;
            this.skinLabel3.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel3.IsStyled = false;
            this.skinLabel3.Location = new Point(0x16e, 0x16);
            this.skinLabel3.Name = "skinLabel3";
            this.skinLabel3.Size = new Size(0xc3, 20);
            this.skinLabel3.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel3, null);
            this.skinLabel3.TabIndex = 6;
            this.skinLabel3.Text = "<LOC>Ready";
            this.skinLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel3.TextPadding = new Padding(0);
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(9, 0x16);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0xc4, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 4;
            this.skinLabel1.Text = "<LOC>Name";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(0);
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x1a8, 0x1a);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x4c, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 3;
            this.gpgLabel2.Text = "<LOC>Ping";
            this.gpgLabel2.TextStyle = TextStyles.Bold;
            this.gpgLabel2.Visible = false;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x16b, 0x1a);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x57, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 2;
            this.gpgLabel1.Text = "<LOC>Ready";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.gpgLabelFaction.AutoSize = true;
            this.gpgLabelFaction.AutoStyle = true;
            this.gpgLabelFaction.Font = new Font("Arial", 9.75f);
            this.gpgLabelFaction.ForeColor = Color.White;
            this.gpgLabelFaction.IgnoreMouseWheel = false;
            this.gpgLabelFaction.IsStyled = false;
            this.gpgLabelFaction.Location = new Point(200, 0x1a);
            this.gpgLabelFaction.Name = "gpgLabelFaction";
            this.gpgLabelFaction.Size = new Size(0x5d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelFaction, null);
            this.gpgLabelFaction.TabIndex = 1;
            this.gpgLabelFaction.Text = "<LOC>Faction";
            this.gpgLabelFaction.TextStyle = TextStyles.Bold;
            this.gpgLabelName.AutoSize = true;
            this.gpgLabelName.AutoStyle = true;
            this.gpgLabelName.Font = new Font("Arial", 9.75f);
            this.gpgLabelName.ForeColor = Color.White;
            this.gpgLabelName.IgnoreMouseWheel = false;
            this.gpgLabelName.IsStyled = false;
            this.gpgLabelName.Location = new Point(6, 0x1a);
            this.gpgLabelName.Name = "gpgLabelName";
            this.gpgLabelName.Size = new Size(0x54, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelName, null);
            this.gpgLabelName.TabIndex = 0;
            this.gpgLabelName.Text = "<LOC>Name";
            this.gpgLabelName.TextStyle = TextStyles.Bold;
            this.gpgPictureBoxPrefMap.Location = new Point(6, 0x23);
            this.gpgPictureBoxPrefMap.Name = "gpgPictureBoxPrefMap";
            this.gpgPictureBoxPrefMap.Size = new Size(0x80, 0x80);
            this.gpgPictureBoxPrefMap.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.gpgPictureBoxPrefMap, null);
            this.gpgPictureBoxPrefMap.TabIndex = 8;
            this.gpgPictureBoxPrefMap.TabStop = false;
            this.skinButtonLaunch.Anchor = AnchorStyles.Bottom;
            this.skinButtonLaunch.AutoStyle = true;
            this.skinButtonLaunch.BackColor = Color.Black;
            this.skinButtonLaunch.DialogResult = DialogResult.OK;
            this.skinButtonLaunch.DisabledForecolor = Color.Gray;
            this.skinButtonLaunch.DrawEdges = true;
            this.skinButtonLaunch.Enabled = false;
            this.skinButtonLaunch.FocusColor = Color.Yellow;
            this.skinButtonLaunch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLaunch.ForeColor = Color.White;
            this.skinButtonLaunch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLaunch.IsStyled = true;
            this.skinButtonLaunch.Location = new Point(0x1d3, 0x227);
            this.skinButtonLaunch.Name = "skinButtonLaunch";
            this.skinButtonLaunch.Size = new Size(130, 0x1a);
            this.skinButtonLaunch.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonLaunch, null);
            this.skinButtonLaunch.TabIndex = 9;
            this.skinButtonLaunch.Text = "<LOC>Launch";
            this.skinButtonLaunch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLaunch.TextPadding = new Padding(0);
            this.skinButtonLaunch.Click += new EventHandler(this.skinButtonLaunch_Click);
            this.skinButtonChangeMap.AutoStyle = true;
            this.skinButtonChangeMap.BackColor = Color.Black;
            this.skinButtonChangeMap.DialogResult = DialogResult.OK;
            this.skinButtonChangeMap.DisabledForecolor = Color.Gray;
            this.skinButtonChangeMap.DrawEdges = true;
            this.skinButtonChangeMap.Enabled = false;
            this.skinButtonChangeMap.FocusColor = Color.Yellow;
            this.skinButtonChangeMap.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonChangeMap.ForeColor = Color.White;
            this.skinButtonChangeMap.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonChangeMap.IsStyled = true;
            this.skinButtonChangeMap.Location = new Point(0x40, 210);
            this.skinButtonChangeMap.Name = "skinButtonChangeMap";
            this.skinButtonChangeMap.Size = new Size(0x9d, 0x1a);
            this.skinButtonChangeMap.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonChangeMap, null);
            this.skinButtonChangeMap.TabIndex = 10;
            this.skinButtonChangeMap.Text = "<LOC>Set Map Preferences";
            this.skinButtonChangeMap.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonChangeMap.TextPadding = new Padding(0);
            this.skinButtonChangeMap.Click += new EventHandler(this.skinButtonChangeMap_Click);
            this.textBoxMsg.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.textBoxMsg.Location = new Point(12, 0x205);
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
            this.textBoxMsg.Size = new Size(850, 20);
            this.textBoxMsg.TabIndex = 11;
            this.gpgChatGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgChatGrid.CustomizeStyle = true;
            this.gpgChatGrid.EmbeddedNavigator.Name = "";
            this.gpgChatGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgChatGrid.IgnoreMouseWheel = false;
            node.RelationName = "Level1";
            node2.RelationName = "Level2";
            this.gpgChatGrid.LevelTree.Nodes.AddRange(new GridLevelNode[] { node, node2 });
            this.gpgChatGrid.Location = new Point(12, 0x151);
            this.gpgChatGrid.MainView = this.gvChat;
            this.gpgChatGrid.Name = "gpgChatGrid";
            this.gpgChatGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3 });
            this.gpgChatGrid.ShowOnlyPredefinedDetails = true;
            this.gpgChatGrid.Size = new Size(850, 170);
            this.gpgChatGrid.TabIndex = 12;
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
            this.gvChat.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
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
            this.gvChat.Appearance.RowSeparator.BackColor = Color.White;
            this.gvChat.Appearance.RowSeparator.BackColor2 = Color.White;
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
            this.gvChat.Columns.AddRange(new GridColumn[] { this.colIcon, this.colPlayer, this.colText, this.gcVisible });
            this.gvChat.GridControl = this.gpgChatGrid;
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
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(3, 0x10);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x83, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 13;
            this.gpgLabel3.Text = "<LOC>Thumbs Up";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabel3.TextStyle = TextStyles.Bold;
            this.gpgLabePreflMapName.AutoStyle = true;
            this.gpgLabePreflMapName.Font = new Font("Arial", 9.75f);
            this.gpgLabePreflMapName.ForeColor = Color.White;
            this.gpgLabePreflMapName.IgnoreMouseWheel = false;
            this.gpgLabePreflMapName.IsStyled = false;
            this.gpgLabePreflMapName.Location = new Point(3, 0xa6);
            this.gpgLabePreflMapName.Name = "gpgLabePreflMapName";
            this.gpgLabePreflMapName.Size = new Size(0x83, 0x29);
            base.ttDefault.SetSuperTip(this.gpgLabePreflMapName, null);
            this.gpgLabePreflMapName.TabIndex = 14;
            this.gpgLabePreflMapName.Text = "Map Name";
            this.gpgLabePreflMapName.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabePreflMapName.TextStyle = TextStyles.Default;
            this.gpgLabeNonPreflMapName.AutoStyle = true;
            this.gpgLabeNonPreflMapName.Font = new Font("Arial", 9.75f);
            this.gpgLabeNonPreflMapName.ForeColor = Color.White;
            this.gpgLabeNonPreflMapName.IgnoreMouseWheel = false;
            this.gpgLabeNonPreflMapName.IsStyled = false;
            this.gpgLabeNonPreflMapName.Location = new Point(140, 0xa6);
            this.gpgLabeNonPreflMapName.Name = "gpgLabeNonPreflMapName";
            this.gpgLabeNonPreflMapName.Size = new Size(0x80, 0x29);
            base.ttDefault.SetSuperTip(this.gpgLabeNonPreflMapName, null);
            this.gpgLabeNonPreflMapName.TabIndex = 0x11;
            this.gpgLabeNonPreflMapName.Text = "Map Name";
            this.gpgLabeNonPreflMapName.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabeNonPreflMapName.TextStyle = TextStyles.Default;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x89, 0x10);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x83, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x10;
            this.gpgLabel5.Text = "<LOC>Thumbs down";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabel5.TextStyle = TextStyles.Bold;
            this.gpgPictureBoxNonPrefMap.Location = new Point(140, 0x23);
            this.gpgPictureBoxNonPrefMap.Name = "gpgPictureBoxNonPrefMap";
            this.gpgPictureBoxNonPrefMap.Size = new Size(0x80, 0x80);
            this.gpgPictureBoxNonPrefMap.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.gpgPictureBoxNonPrefMap, null);
            this.gpgPictureBoxNonPrefMap.TabIndex = 15;
            this.gpgPictureBoxNonPrefMap.TabStop = false;
            this.gpgGroupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgGroupBox1.Controls.Add(this.gpgLabel3);
            this.gpgGroupBox1.Controls.Add(this.gpgLabeNonPreflMapName);
            this.gpgGroupBox1.Controls.Add(this.gpgPictureBoxPrefMap);
            this.gpgGroupBox1.Controls.Add(this.gpgLabel5);
            this.gpgGroupBox1.Controls.Add(this.skinButtonChangeMap);
            this.gpgGroupBox1.Controls.Add(this.gpgPictureBoxNonPrefMap);
            this.gpgGroupBox1.Controls.Add(this.gpgLabePreflMapName);
            this.gpgGroupBox1.Location = new Point(0x24b, 0x53);
            this.gpgGroupBox1.Name = "gpgGroupBox1";
            this.gpgGroupBox1.Size = new Size(0x113, 0xf2);
            base.ttDefault.SetSuperTip(this.gpgGroupBox1, null);
            this.gpgGroupBox1.TabIndex = 0x12;
            this.gpgGroupBox1.TabStop = false;
            this.gpgGroupBox1.Text = "<LOC>Current Map Preferences";
            this.skinButtonLeaveTeam.Anchor = AnchorStyles.Bottom;
            this.skinButtonLeaveTeam.AutoStyle = true;
            this.skinButtonLeaveTeam.BackColor = Color.Black;
            this.skinButtonLeaveTeam.DialogResult = DialogResult.OK;
            this.skinButtonLeaveTeam.DisabledForecolor = Color.Gray;
            this.skinButtonLeaveTeam.DrawEdges = true;
            this.skinButtonLeaveTeam.FocusColor = Color.Yellow;
            this.skinButtonLeaveTeam.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLeaveTeam.ForeColor = Color.White;
            this.skinButtonLeaveTeam.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLeaveTeam.IsStyled = true;
            this.skinButtonLeaveTeam.Location = new Point(0x14b, 0x227);
            this.skinButtonLeaveTeam.Name = "skinButtonLeaveTeam";
            this.skinButtonLeaveTeam.Size = new Size(130, 0x1a);
            this.skinButtonLeaveTeam.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonLeaveTeam, null);
            this.skinButtonLeaveTeam.TabIndex = 10;
            this.skinButtonLeaveTeam.Text = "<LOC>Disband";
            this.skinButtonLeaveTeam.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLeaveTeam.TextPadding = new Padding(0);
            this.skinButtonLeaveTeam.Click += new EventHandler(this.skinButtonLeaveTeam_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x369, 640);
            base.Controls.Add(this.skinButtonLeaveTeam);
            base.Controls.Add(this.gpgGroupBox1);
            base.Controls.Add(this.gpgChatGrid);
            base.Controls.Add(this.textBoxMsg);
            base.Controls.Add(this.skinButtonLaunch);
            base.Controls.Add(this.gpgGroupBoxMembers);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x369, 640);
            base.Name = "DlgTeamGame";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Team Selection";
            base.Controls.SetChildIndex(this.gpgGroupBoxMembers, 0);
            base.Controls.SetChildIndex(this.skinButtonLaunch, 0);
            base.Controls.SetChildIndex(this.textBoxMsg, 0);
            base.Controls.SetChildIndex(this.gpgChatGrid, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox1, 0);
            base.Controls.SetChildIndex(this.skinButtonLeaveTeam, 0);
            this.gpgGroupBoxMembers.ResumeLayout(false);
            this.gpgGroupBoxMembers.PerformLayout();
            ((ISupportInitialize) this.gpgPictureBoxPrefMap).EndInit();
            this.textBoxMsg.Properties.EndInit();
            this.gpgChatGrid.EndInit();
            this.gvChat.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimMemoEdit3.EndInit();
            this.rimTextEdit.EndInit();
            ((ISupportInitialize) this.gpgPictureBoxNonPrefMap).EndInit();
            this.gpgGroupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InviteMember(PnlTeamGameMember sender, TeamGame.TeamGameMember member)
        {
            if (this.GameRunning)
            {
                this.ErrorMessage("<LOC>You must quit your current game first.", new object[0]);
            }
            else
            {
                DialogResult result;
                if (this.GameLaunched)
                {
                    string msg = "<LOC>Inviting a new member right now will abort your teams current game search. Do you really want to invite a new member?";
                    if (new DlgYesNo(base.MainForm, "<LOC>Invite Member", msg).ShowDialog() == DialogResult.Yes)
                    {
                        this.CancelTeamAutomatch();
                    }
                    else
                    {
                        return;
                    }
                }
                string playerName = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>Enter the name of the player you want to invite to your team."), Loc.Get("<LOC>Invite"), false, out result);
                if (result == DialogResult.OK)
                {
                    if ((playerName == null) || (playerName.Length < 3))
                    {
                        this.ErrorMessage("<LOC>Player name cannot be less than {0} characters.", new object[] { 3 });
                    }
                    else if ((playerName != null) && (playerName.Length > 0))
                    {
                        this.OnInviteMember(playerName);
                    }
                }
            }
        }

        private void KickMember(PnlTeamGameMember sender, TeamGame.TeamGameMember member)
        {
            if (this.GameRunning)
            {
                this.ErrorMessage("<LOC>You must quit your current game first.", new object[0]);
            }
            else
            {
                string str;
                if (this.GameLaunched)
                {
                    str = "<LOC>Are you sure you want to kick this team member? Kicking right now will abort your teams current game search.";
                }
                else
                {
                    str = "<LOC>Are you sure you want to kick this team member?";
                }
                if (new DlgYesNo(base.MainForm, "<LOC>Kick Member", str).ShowDialog() == DialogResult.Yes)
                {
                    Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameKick, new object[] { member.Name });
                    this.RemoveMember(member.Name, "<LOC>{0} has been kicked out of your team game.", new object[] { member.Name });
                }
            }
        }

        internal void LaunchTeamAutomatch()
        {
            this.GameLaunched = true;
            Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameLaunch, new object[0]);
            this.skinButtonLaunch.Text = Loc.Get("<LOC>Abort");
            this.skinButtonChangeMap.Enabled = false;
            base.MainForm.PlayRankedTeamGame(this.Team.GameType, new List<string>(this.Team.GetAllMemberNames()), this.Team.TeamIDs);
            SupcomAutomatch.GetSupcomAutomatch().OnStatusChanged += new StringEventHandler2(this.Automatch_OnStatusChanged);
        }

        private void LeaveTeam(PnlTeamGameMember sender, TeamGame.TeamGameMember member)
        {
            this.OnLeaveTeam();
        }

        private void MemberChanged(PnlTeamGameMember sender, TeamGame.TeamGameMember member)
        {
            this.OnMemberChanged(member);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.MainForm.EnableGameButtons();
            base.MainForm.RefreshPMWindows();
            base.OnClosed(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                if (!(!this.GameRunning || this.ForceLeave))
                {
                    this.ErrorMessage("<LOC>You must quit your current game first.", new object[0]);
                    e.Cancel = true;
                }
                else
                {
                    if (!this.ForceLeave)
                    {
                        if (this.Team.TeamLeader.IsSelf)
                        {
                            if (this.Team.TeamMembers.Count > 1)
                            {
                                if (new DlgYesNo(base.MainForm, "<LOC>Question", "<LOC>Are you sure you want to disband this team?").ShowDialog() == DialogResult.Yes)
                                {
                                    Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameDisband, new object[0]);
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                        else if (new DlgYesNo(base.MainForm, "<LOC>Question", "<LOC>Are you sure you want to leave this team?").ShowDialog() == DialogResult.Yes)
                        {
                            Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameLeave, new object[0]);
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    base.OnClosing(e);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnDisbandTeam()
        {
            if (this.GameRunning)
            {
                this.ErrorMessage("<LOC>You must quit your current game first.", new object[0]);
            }
            else if (new DlgYesNo(base.MainForm, "<LOC>Question", "<LOC>Are you sure you want to disband this team?").ShowDialog() == DialogResult.Yes)
            {
                if (this.GameLaunched)
                {
                    this.CancelTeamAutomatch();
                }
                this.ForceLeave = true;
                Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameDisband, new object[0]);
                base.Close();
            }
        }

        internal void OnInviteMember(string playerName)
        {
            if (this.GameRunning)
            {
                this.ErrorMessage("<LOC>You must quit your current game first.", new object[0]);
            }
            else if (this.Team.FindMember(playerName) != null)
            {
                this.SystemMessage("<LOC>{0} is already on your team.", new object[] { playerName });
            }
            else if (this.Team.TeamMembers.Count >= TeamGame.MAX_TEAM_MEMBERS)
            {
                this.SystemMessage("<LOC>Unable to invite more members, your team is already full.", new object[0]);
            }
            else
            {
                User user;
                if (base.MainForm.TryFindUser(playerName, true, out user))
                {
                    if ((user != null) && user.Online)
                    {
                        Messaging.SendCustomCommand(playerName, CustomCommands.TeamGameInvite, new object[0]);
                        this.SystemMessage("<LOC>Invitation sent.", new object[0]);
                    }
                    else
                    {
                        this.SystemMessage("<LOC>{0} is offline and cannot join your team at this time.", new object[] { playerName });
                    }
                }
                else
                {
                    this.SystemMessage("<LOC>Unable to locate player {0}", new object[] { playerName });
                }
            }
        }

        internal void OnLaunchTeamAutomatch()
        {
            if ((((Chatroom.Current != null) && (Chatroom.CurrentName != null)) && (Chatroom.CurrentName.Length > 0)) && (Chatroom.CurrentPopulation > 0))
            {
                base.MainForm.BeforeTeamGameChatroom = Chatroom.CurrentName;
            }
            else
            {
                base.MainForm.BeforeTeamGameChatroom = null;
            }
        }

        private void OnLeaveTeam()
        {
            if (this.GameRunning)
            {
                this.ErrorMessage("<LOC>You must quit your current game first.", new object[0]);
            }
            else
            {
                string str;
                if (this.GameLaunched)
                {
                    str = "<LOC>Are you sure you want to leave this team? Leaving now will abort your teams current game search.";
                }
                else
                {
                    str = "<LOC>Are you sure you want to leave this team?";
                }
                if (new DlgYesNo(base.MainForm, "<LOC>Leave Team", str).ShowDialog() == DialogResult.Yes)
                {
                    this.ForceLeave = true;
                    Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameLeave, new object[0]);
                    base.Close();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Exception exception;
            try
            {
                Application.ApplicationExit += new EventHandler(this.Application_ApplicationExit);
                base.MainForm.DisableGameButtons();
                if (this.Team.TeamLeader.IsSelf)
                {
                    this.skinButtonLeaveTeam.Text = Loc.Get("<LOC>Disband");
                }
                else
                {
                    this.skinButtonLeaveTeam.Text = Loc.Get("<LOC>Leave");
                }
                base.OnLoad(e);
                this.skinButtonChangeMap.Enabled = this.Team.TeamLeader.IsSelf;
                int bottom = this.gpgLabelName.Bottom;
                int num2 = 4;
                int num3 = 8;
                PnlTeamGameMember member = null;
                for (int i = 0; i < TeamGame.MAX_TEAM_MEMBERS; i++)
                {
                    TeamGame.TeamGameMember member2 = null;
                    if (i < this.Team.TeamMembers.Count)
                    {
                        member2 = this.Team.TeamMembers[i];
                    }
                    PnlTeamGameMember member3 = new PnlTeamGameMember(this.Team);
                    member3.Top = num3 + bottom;
                    member3.Left = num2;
                    if (((member2 != null) && member2.IsSelf) && (Program.Settings.SupcomPrefs.RankedGames.Faction != "random"))
                    {
                        SupcomLookups._Factions any = SupcomLookups._Factions.Any;
                        try
                        {
                            any = (SupcomLookups._Factions) Enum.Parse(typeof(SupcomLookups._Factions), Program.Settings.SupcomPrefs.RankedGames.Faction.TrimStart(new char[] { '/' }), true);
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            ErrorLog.WriteLine(exception);
                            Program.Settings.SupcomPrefs.RankedGames.Faction = "random";
                        }
                        member2.Faction = any;
                    }
                    member3.SetPlayer(member2);
                    if (member2 != null)
                    {
                        member2.BeginPing();
                    }
                    member3.InviteClick += new PnlTeamGameMember.TeamGameMemberEventHandler(this.InviteMember);
                    member3.DisbandClick += new PnlTeamGameMember.TeamGameMemberEventHandler(this.DisbandTeam);
                    member3.KickClick += new PnlTeamGameMember.TeamGameMemberEventHandler(this.KickMember);
                    member3.LeaveClick += new PnlTeamGameMember.TeamGameMemberEventHandler(this.LeaveTeam);
                    member3.MemberChanged += new PnlTeamGameMember.TeamGameMemberEventHandler(this.MemberChanged);
                    if (((!this.Team.TeamLeader.IsSelf && (member2 != null)) && member2.IsSelf) && (member2.Faction != SupcomLookups._Factions.Any))
                    {
                        Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameMember, new object[] { member2.ToDataString() });
                    }
                    this.gpgGroupBoxMembers.Controls.Add(member3);
                    this.MemberPanels.Add(member3);
                    bottom += num3 + member3.Height;
                    member = member3;
                }
                if (member == null)
                {
                }
                this.BindToPrefs(null, null);
                base.MainForm.RefreshPMWindows();
            }
            catch (Exception exception2)
            {
                exception = exception2;
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnMemberChanged(TeamGame.TeamGameMember member)
        {
            if (member.IsSelf)
            {
                string str;
                if (member.Faction == SupcomLookups._Factions.Any)
                {
                    str = "random";
                }
                else
                {
                    str = "/" + member.Faction.ToString().ToLower();
                }
                if (Program.Settings.SupcomPrefs.RankedGames.Faction != str)
                {
                    Program.Settings.SupcomPrefs.RankedGames.Faction = str;
                }
                Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameMember, new object[] { member.ToDataString() });
                this.UpdateLaunchAbility();
            }
        }

        public void OnMessageRecieved(string sender, string message)
        {
            this.AddChat(User.MakeFakeUser(sender), message);
        }

        internal void OnTeamGameStart()
        {
            this.GameRunning = true;
            this.skinButtonLeaveTeam.Enabled = false;
            this.skinButtonLaunch.Enabled = false;
            if (this.Team.TeamLeader.IsSelf)
            {
                foreach (PnlTeamGameMember member in this.MemberPanels)
                {
                    member.DisableButtons();
                    if ((member.TeamMember != null) && member.TeamMember.IsSelf)
                    {
                        member.Disable();
                    }
                }
            }
            else
            {
                foreach (PnlTeamGameMember member in this.MemberPanels)
                {
                    if ((member.TeamMember != null) && member.TeamMember.IsSelf)
                    {
                        member.Disable();
                        member.DisableButtons();
                        break;
                    }
                }
            }
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                Thread.Sleep(0xbb8);
                base.BeginInvoke(delegate {
                    this.Team.GetSelf().IsReady = false;
                    this.OnMemberChanged(this.Team.GetSelf());
                    this.UpdateMember(this.Team.GetSelf());
                });
            });
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

        internal void ReceiveKick()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.ReceiveKick();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.ForceLeave = true;
                this.SystemMessage("<LOC>You have been kicked from your team game.", new object[0]);
                base.MainForm.SystemMessage("<LOC>You have been kicked from your team game.", new object[0]);
                base.MainForm.RefreshPMWindows();
                base.Close();
            }
        }

        private void RefreshMaps()
        {
            try
            {
                if (this.Team.PreferredMap == "none")
                {
                    this.gpgPictureBoxPrefMap.Image = SupremeCommanderIcons.random;
                    this.gpgLabePreflMapName.Text = Loc.Get("<LOC>No Preference");
                }
                else
                {
                    this.gpgPictureBoxPrefMap.Image = SupcomMaps.ResourceManager.GetObject(this.Team.PreferredMap) as Image;
                    this.gpgLabePreflMapName.Text = SupcomLookups.TranslateMapCode(this.Team.PreferredMap);
                }
                if (this.Team.NonPreferredMap == "none")
                {
                    this.gpgPictureBoxNonPrefMap.Image = SupremeCommanderIcons.random;
                    this.gpgLabeNonPreflMapName.Text = Loc.Get("<LOC>No Preference");
                }
                else
                {
                    this.gpgPictureBoxNonPrefMap.Image = SupcomMaps.ResourceManager.GetObject(this.Team.NonPreferredMap) as Image;
                    this.gpgLabeNonPreflMapName.Text = SupcomLookups.TranslateMapCode(this.Team.NonPreferredMap);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void RemoveMember(string teamMember, string msg, params object[] args)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RemoveMember(teamMember, msg, args);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                TeamGame.TeamGameMember item = this.Team.FindMember(teamMember);
                if (item != null)
                {
                    if (this.GameLaunched)
                    {
                        this.CancelTeamAutomatch();
                    }
                    item.EndPing();
                    this.Team.TeamMembers.Remove(item);
                    this.SystemMessage(msg, args);
                    this.SystemMessage("<LOC>Team game mode is now set to {0}.", new object[] { Loc.Get(this.Team.GameType) });
                    SupcomAutomatch.GetSupcomAutomatch().Kind = this.Team.GameType;
                    bool flag = false;
                    for (int i = 0; i < this.MemberPanels.Count; i++)
                    {
                        if (flag && (i > 0))
                        {
                            this.MemberPanels[i - 1].SetPlayer(this.MemberPanels[i].TeamMember);
                            this.MemberPanels[i].SetPlayer(null);
                        }
                        else if (!(this.MemberPanels[i].IsOpen || !this.MemberPanels[i].TeamMember.Equals(item)))
                        {
                            this.MemberPanels[i].SetPlayer(null);
                            flag = true;
                        }
                    }
                    this.UpdateLaunchAbility();
                    base.MainForm.RefreshPMWindows();
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
                this.gvChat.MoveLastVisible();
                Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameMessage, new object[] { text });
            }
        }

        private void skinButtonChangeMap_Click(object sender, EventArgs e)
        {
            new DlgSupcomMapOptions(this.Team.GameType, true).Show();
        }

        private void skinButtonLaunch_Click(object sender, EventArgs e)
        {
            if (!this.GameLaunched)
            {
                this.LaunchTeamAutomatch();
            }
            else
            {
                this.CancelTeamAutomatch();
            }
        }

        private void skinButtonLeaveTeam_Click(object sender, EventArgs e)
        {
            if (this.Team.TeamLeader.IsSelf)
            {
                this.OnDisbandTeam();
            }
            else
            {
                this.OnLeaveTeam();
            }
        }

        protected override void StyleApplication(object sender, PropertyChangedEventArgs e)
        {
            base.StyleApplication(sender, e);
            this.gvChat.Appearance.Empty.BackColor = Program.Settings.StylePreferences.MasterBackColor;
            this.gvChat.Appearance.Row.BackColor = Program.Settings.StylePreferences.MasterBackColor;
            this.gvChat.Appearance.HideSelectionRow.BackColor = Program.Settings.StylePreferences.MasterBackColor;
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
            this.gvChat.Invalidate();
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

        internal void UpdateAutomatchStatus(string text, params object[] args)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.SetStatus(text, args);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                base.SetStatus(text, args);
            }
            int length = 0;
            if (args != null)
            {
                length = args.Length;
            }
            object[] destinationArray = new object[1 + length];
            destinationArray[0] = text;
            if (args != null)
            {
                Array.ConstrainedCopy(args, 0, destinationArray, 1, args.Length);
            }
            Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameStatus, destinationArray);
        }

        private void UpdateLaunchAbility()
        {
            try
            {
                if (!this.Team.TeamLeader.IsSelf)
                {
                    this.skinButtonLaunch.Enabled = false;
                }
                else
                {
                    foreach (TeamGame.TeamGameMember member in this.Team)
                    {
                        if ((member != null) && !(this.GameLaunched || member.IsReady))
                        {
                            this.skinButtonLaunch.Enabled = false;
                            return;
                        }
                    }
                    this.skinButtonLaunch.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        internal void UpdateMember(TeamGame.TeamGameMember member)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.UpdateMember(member);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                bool flag = this.Team.FindMember(member.Name) == null;
                this.Team.UpdateMember(member);
                if (flag && (this.Team.TeamMembers.Count <= TeamGame.MAX_TEAM_MEMBERS))
                {
                    this.MemberPanels[this.Team.TeamMembers.Count - 1].SetPlayer(member);
                    this.SystemMessage("<LOC>{0} has joined the team.", new object[] { member.Name });
                    this.SystemMessage("<LOC>Team game mode is now set to {0}.", new object[] { Loc.Get(this.Team.GameType) });
                    SupcomAutomatch.GetSupcomAutomatch().Kind = this.Team.GameType;
                }
                else
                {
                    foreach (PnlTeamGameMember member2 in this.MemberPanels)
                    {
                        if (!(member2.IsOpen || !member2.TeamMember.Equals(member)))
                        {
                            member2.SetPlayer(member);
                        }
                    }
                }
                this.UpdateLaunchAbility();
            }
        }

        internal void UpdateMember(string teamMember)
        {
            this.UpdateMember(TeamGame.TeamGameMember.FromDataString(this.Team, teamMember));
        }

        private bool ValidateMaps()
        {
            OGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        return this.ValidateMaps();
                    };
                }
                return (bool) base.Invoke(method);
            }
            if (!base.Disposing && !base.IsDisposed)
            {
                try
                {
                    this.IsValidatingMaps = true;
                    bool flag = false;
                    DlgSupcomMapOptions options = new DlgSupcomMapOptions(this.Team.GameType, true);
                    if (!(!(this.Team.PreferredMap != "none") || options.Maps.Contains(new SupcomMapInfo(this.Team.PreferredMap))))
                    {
                        this.SystemMessage("<LOC>Map: {0} is not supported by this many players.", new object[] { SupcomLookups.TranslateMapCode(this.Team.PreferredMap) });
                        this.Team.PreferredMap = "none";
                        flag = true;
                    }
                    if (!(!(this.Team.NonPreferredMap != "none") || options.Maps.Contains(new SupcomMapInfo(this.Team.NonPreferredMap))))
                    {
                        this.SystemMessage("<LOC>Map: {0} is not supported by this many players.", new object[] { SupcomLookups.TranslateMapCode(this.Team.NonPreferredMap) });
                        this.Team.NonPreferredMap = "none";
                        flag = true;
                    }
                    Program.Settings.SupcomPrefs.RankedGames.MapsChanged -= new PropertyChangedEventHandler(this.BindToPrefs);
                    options.SaveMapPrefs();
                    Program.Settings.SupcomPrefs.RankedGames.MapsChanged += new PropertyChangedEventHandler(this.BindToPrefs);
                    if (flag)
                    {
                        options.Dispose();
                        options = null;
                        Messaging.SendCustomCommand(this.Team.GetOtherMemberNames(), CustomCommands.TeamGameMap, new object[] { this.Team.PreferredMap, this.Team.NonPreferredMap });
                        this.RefreshMaps();
                        return false;
                    }
                    options.Dispose();
                    options = null;
                    return true;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return false;
                }
                finally
                {
                    this.IsValidatingMaps = false;
                }
            }
            return false;
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return false;
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

        internal bool GameLaunched
        {
            get
            {
                return this.Team.HasLaunched;
            }
            set
            {
                this.Team.HasLaunched = value;
                this.GameLaunchedChanged();
            }
        }

        internal TeamGame Team
        {
            get
            {
                return this.mTeam;
            }
        }
    }
}

