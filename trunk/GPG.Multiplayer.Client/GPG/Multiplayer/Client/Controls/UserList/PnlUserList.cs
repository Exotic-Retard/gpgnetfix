namespace GPG.Multiplayer.Client.Controls.UserList
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Admin;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.Plugin;
    using GPG.Multiplayer.Quazal;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class PnlUserList : PnlBase
    {
        private Queue<UserListRow> AddedRows = new Queue<UserListRow>();
        private MenuItem ciChat_Ban;
        private MenuItem ciChat_Demote;
        private MenuItem ciChat_IgnorePlayer;
        private MenuItem ciChat_InviteFriend;
        private MenuItem ciChat_InviteToClan;
        private MenuItem ciChat_Kick;
        private MenuItem ciChat_LeaveClan;
        private MenuItem ciChat_Promote;
        private MenuItem ciChat_RemoveClanMember;
        private MenuItem ciChat_RemoveFriend;
        private MenuItem ciChat_RequestClanInvite;
        private MenuItem ciChat_TeamInvite;
        private MenuItem ciChat_UnignorePlayer;
        private MenuItem ciChat_ViewClan;
        private MenuItem ciChat_ViewPlayer;
        private MenuItem ciChat_ViewRank;
        private MenuItem ciChat_WebStats;
        private MenuItem ciChat_WhisperPlayer;
        private IContainer components = null;
        private GPGContextMenu gpgContextMenuChat;
        internal GPGPanel gpgPanelBody;
        private UserListRow LastClick1 = null;
        private UserListRow LastClick2 = null;
        private object LayoutMutex = new object();
        private bool mAutoRefresh = false;
        private Dictionary<UserListCategories, UserListCategory> mCategories = new Dictionary<UserListCategories, UserListCategory>();
        private MappedObjectList<ClanMember> mClanMembers;
        private MenuItem menuItem10;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem8;
        private MenuItem miRankedChallenge;
        private bool mIsBound = false;
        private MenuItem miToggleChallengeDeny;
        private MenuItem miViewReplays;
        private bool mPerformingLayout = false;
        private UserListRow mSelectedRow;
        private UserListStyles mStyle = UserListStyles.None;
        private Dictionary<User, UserListRow> mUserRows = new Dictionary<User, UserListRow>();
        private Queue<UserListRow> RemovedRows = new Queue<UserListRow>();
        private Queue<User> UpdatedUsers = new Queue<User>();

        public event EventHandler RequestRefresh;

        public PnlUserList()
        {
            this.InitializeComponent();
            this.InitializePlugins();
        }

        public void AddUser(User user)
        {
            this.AddUser(user, false);
        }

        public void AddUser(User user, bool block)
        {
            this.AddUser(user, block, true);
        }

        public void AddUser(User user, bool block, bool refresh)
        {
            WaitCallback callBack = null;
            if (((!block && !base.InvokeRequired) && !base.Disposing) && !base.IsDisposed)
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        this.AddUser(user);
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
            else if ((!base.Disposing && !base.IsDisposed) && this.IsBound)
            {
                if (this.UserRows.ContainsKey(user))
                {
                    this.UpdateUser(user);
                }
                else
                {
                    UserListRow item = null;
                    UserListCategories initialCategory = this.DetermineUserCategory(user);
                    if (this.Style == UserListStyles.Chatroom)
                    {
                        int index = Chatroom.GatheringParticipants.IndexOf(user);
                        if (index >= 0)
                        {
                            Chatroom.GatheringParticipants.IndexObject(user);
                            Chatroom.GatheringParticipants[index] = user;
                        }
                        else
                        {
                            Chatroom.GatheringParticipants.IndexObject(user);
                            Chatroom.GatheringParticipants.Add(user);
                        }
                    }
                    item = new UserListRow(this, user, this.Style, initialCategory);
                    item.MouseDown += new MouseEventHandler(this.row_MouseDown);
                    item.MouseUp += new MouseEventHandler(this.row_MouseUp);
                    this.AddedRows.Enqueue(item);
                    if (refresh && this.AutoRefresh)
                    {
                        this.RefreshData();
                    }
                }
            }
        }

        public void AddUsers(MappedObjectList<User> users)
        {
            this.AddUsers(users, false);
        }

        public void AddUsers(MappedObjectList<User> users, bool block)
        {
            foreach (User user in users)
            {
                this.AddUser(user, block, false);
            }
            if (this.AutoRefresh)
            {
                this.RefreshData();
            }
        }

        private void AppearanceSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            this.UpdatedUsers.Clear();
            foreach (User user in this.UserRows.Keys)
            {
                this.UpdatedUsers.Enqueue(user);
            }
            this.OnRequestRefresh();
        }

        private void AwardVisibilityChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnRequestRefresh();
        }

        public void BindToList(UserListStyles style)
        {
            this.mIsBound = false;
            this.ClearData();
            foreach (UserListCategory category in this.Categories.Values)
            {
                category.ExpandContractChanged += new EventHandler(this.category_ExpandContractChanged);
                category.VisibleChanged += new EventHandler(this.category_VisibleChanged);
                category.Dispose();
            }
            this.mCategories.Clear();
            this.mStyle = style;
            switch (style)
            {
                case UserListStyles.Chatroom:
                    this.Categories.Add(UserListCategories.Admin, new UserListCategory(this, UserListCategories.Admin, style));
                    this.Categories.Add(UserListCategories.Moderator, new UserListCategory(this, UserListCategories.Moderator, style));
                    this.Categories.Add(UserListCategories.Speaking, new UserListCategory(this, UserListCategories.Speaking, style));
                    this.Categories.Add(UserListCategories.Friend, new UserListCategory(this, UserListCategories.Friend, style));
                    this.Categories.Add(UserListCategories.Clan, new UserListCategory(this, UserListCategories.Clan, style));
                    this.Categories.Add(UserListCategories.Online, new UserListCategory(this, UserListCategories.Online, style));
                    this.Categories.Add(UserListCategories.Away, new UserListCategory(this, UserListCategories.Away, style));
                    break;

                case UserListStyles.OnlineOffline:
                    this.Categories.Add(UserListCategories.Online, new UserListCategory(this, UserListCategories.Online, style));
                    this.Categories.Add(UserListCategories.Offline, new UserListCategory(this, UserListCategories.Offline, style));
                    break;

                case UserListStyles.Clan:
                    this.Categories.Add(UserListCategories.Clan_Rank0, new UserListCategory(this, UserListCategories.Clan_Rank0, style));
                    this.Categories.Add(UserListCategories.Clan_Rank1, new UserListCategory(this, UserListCategories.Clan_Rank1, style));
                    this.Categories.Add(UserListCategories.Clan_Rank2, new UserListCategory(this, UserListCategories.Clan_Rank2, style));
                    this.Categories.Add(UserListCategories.Clan_Rank3, new UserListCategory(this, UserListCategories.Clan_Rank3, style));
                    this.Categories.Add(UserListCategories.Clan_Rank4, new UserListCategory(this, UserListCategories.Clan_Rank4, style));
                    this.Categories.Add(UserListCategories.Clan_Rank5, new UserListCategory(this, UserListCategories.Clan_Rank5, style));
                    break;
            }
            foreach (UserListCategory category in this.Categories.Values)
            {
                category.ExpandContractChanged += new EventHandler(this.category_ExpandContractChanged);
                category.VisibleChanged += new EventHandler(this.category_VisibleChanged);
            }
            this.mIsBound = true;
        }

        private void category_ExpandContractChanged(object sender, EventArgs e)
        {
            this.OnRequestRefresh();
        }

        private void category_VisibleChanged(object sender, EventArgs e)
        {
            this.OnRequestRefresh();
        }

        private void ciChat_Ban_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.BanUser(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_Demote_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.DemoteClanMember(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_IgnorePlayer_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.IgnorePlayer(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_InviteFriend_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.InviteAsFriend(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_InviteToClan_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.InviteToClan(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_Kick_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.OnSendKick(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_LeaveClan_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            Program.MainForm.OnLeaveClan();
        }

        private void ciChat_Promote_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.PromoteClanMember(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_RemoveClanMember_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.RemoveClanMember(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_RemoveFriend_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.RemoveFriend(this.SelectedChatParticipant.Name, this.SelectedChatParticipant.ID);
            }
        }

        private void ciChat_RequestClanInvite_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.RequestClanInvite(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_TeamInvite_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (ConfigSettings.GetBool("UseTeamLobby", false))
            {
                if (this.SelectedChatParticipant != null)
                {
                    base.MainForm.InviteToTeamGame(this.SelectedChatParticipant.Name);
                }
            }
            else if (!DlgSupcomTeamSelection.sIsJoining && (this.SelectedChatParticipant != null))
            {
                DlgSupcomTeamSelection.ShowSelection().AddUser(this.SelectedChatParticipant);
            }
        }

        private void ciChat_UnignorePlayer_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.UnignorePlayer(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_ViewClan_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.OnViewClanProfileByPlayer(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_ViewPlayer_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.OnViewPlayerProfile(this.SelectedChatParticipant.Name);
                this.FoolMenu();
            }
        }

        private void ciChat_ViewRank_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.FindOnLadder(this.SelectedChatParticipant.Name);
            }
        }

        private void ciChat_WebStats_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.ShowWebStats(this.SelectedChatParticipant.ID);
            }
        }

        private void ciChat_WhisperPlayer_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if ((this.SelectedChatParticipant != null) && this.SelectedChatParticipant.Online)
            {
                base.MainForm.OnSendWhisper(this.SelectedChatParticipant.Name, null);
            }
        }

        public void ClearData()
        {
            this.mSelectedRow = null;
            this.RemovedRows.Clear();
            this.AddedRows.Clear();
            this.UpdatedUsers.Clear();
            this.mUserRows.Clear();
            foreach (UserListCategory category in this.Categories.Values)
            {
                foreach (UserListRow row in category.Rows.Values)
                {
                    this.DisposeRow(row);
                }
                category.Clear();
            }
        }

        private void ContextMenuItemClick(object sender, EventArgs e)
        {
            this.OnClick(EventArgs.Empty);
        }

        private UserListCategories DetermineUserCategory(ClanMember member)
        {
            return (UserListCategories) Enum.Parse(typeof(UserListCategories), "Clan_Rank" + member.Rank.ToString());
        }

        private UserListCategories DetermineUserCategory(User user)
        {
            UserListCategories online = UserListCategories.Online;
            switch (this.Style)
            {
                case UserListStyles.Chatroom:
                    if (!Program.Settings.Chat.ShowSpeaking || !base.MainForm.IsSpeaking(user))
                    {
                        if (user.IsAdmin)
                        {
                            return UserListCategories.Admin;
                        }
                        if (user.IsModerator)
                        {
                            return UserListCategories.Moderator;
                        }
                        if (user.IsClanMate)
                        {
                            return UserListCategories.Clan;
                        }
                        if (user.IsFriend)
                        {
                            return UserListCategories.Friend;
                        }
                        if (user.IsAway)
                        {
                            return UserListCategories.Away;
                        }
                        return UserListCategories.Online;
                    }
                    return UserListCategories.Speaking;

                case UserListStyles.OnlineOffline:
                    if (!user.Online)
                    {
                        return UserListCategories.Offline;
                    }
                    return UserListCategories.Online;
            }
            return online;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DisposeRow(UserListRow row)
        {
            row.MouseDown -= new MouseEventHandler(this.row_MouseDown);
            row.MouseUp -= new MouseEventHandler(this.row_MouseUp);
            row.Dispose();
        }

        private void FoolMenu()
        {
            try
            {
                if (ConfigSettings.GetBool("FakeChatMenuOut", true))
                {
                    Form form = new Form();
                    form.Width = 1;
                    form.Height = 1;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Show();
                    form.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gpgContextMenuChat_Popup(object sender, EventArgs e)
        {
            try
            {
                UserListRow selectedRow = this.SelectedRow;
                IUser selectedUser = this.SelectedUser;
                if ((selectedRow == null) || (selectedUser == null))
                {
                    foreach (MenuItem item in this.gpgContextMenuChat.MenuItems)
                    {
                        item.Visible = false;
                    }
                }
                else
                {
                    this.ciChat_WhisperPlayer.Visible = !User.Current.Equals(selectedUser) && selectedUser.Online;
                    this.ciChat_ViewPlayer.Visible = true;
                    this.ciChat_IgnorePlayer.Visible = !selectedUser.IsCurrent && !selectedUser.IsIgnored;
                    this.ciChat_UnignorePlayer.Visible = !selectedUser.IsCurrent && selectedUser.IsIgnored;
                    this.ciChat_WebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
                    this.ciChat_InviteToClan.Visible = (User.Current.IsInClan && !selectedUser.IsInClan) && !selectedUser.IsCurrent;
                    this.ciChat_RequestClanInvite.Visible = (!User.Current.IsInClan && selectedUser.IsInClan) && !selectedUser.IsCurrent;
                    this.ciChat_ViewClan.Visible = selectedUser.IsInClan;
                    this.ciChat_LeaveClan.Visible = User.Current.Equals(selectedUser) && User.Current.IsInClan;
                    this.ciChat_Promote.Visible = (selectedUser.IsClanMate && (ClanMember.Current != null)) && ClanMember.Current.CanTargetAbility(ClanAbility.Promote, selectedUser.Name);
                    this.ciChat_Demote.Visible = (selectedUser.IsClanMate && (ClanMember.Current != null)) && ClanMember.Current.CanTargetAbility(ClanAbility.Demote, selectedUser.Name);
                    this.ciChat_RemoveClanMember.Visible = (selectedUser.IsClanMate && (ClanMember.Current != null)) && ClanMember.Current.CanTargetAbility(ClanAbility.Remove, selectedUser.Name);
                    this.ciChat_InviteFriend.Visible = ((selectedUser is User) && !(selectedUser as User).IsFriend) && !selectedUser.IsCurrent;
                    this.ciChat_RemoveFriend.Visible = ((selectedUser is User) && (selectedUser as User).IsFriend) && !selectedUser.IsCurrent;
                    if ((((((base.MainForm.DlgSelectGame != null) && !base.MainForm.DlgSelectGame.Disposing) && !base.MainForm.DlgSelectGame.IsDisposed) || (!User.Current.IsAdmin && !base.MainForm.IsGameCurrent)) || (base.MainForm.IsInGame || (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable))) || ((base.MainForm.DlgTeamGame != null) && ((selectedUser.IsCurrent || (base.MainForm.DlgTeamGame.Team.TeamMembers.Count >= TeamGame.MAX_TEAM_MEMBERS)) || !base.MainForm.DlgTeamGame.Team.TeamLeader.IsSelf)))
                    {
                        this.ciChat_TeamInvite.Visible = false;
                    }
                    else
                    {
                        this.ciChat_TeamInvite.Visible = true;
                    }
                    if ((Chatroom.Current != null) && Chatroom.Current.IsClanRoom)
                    {
                        this.ciChat_Ban.Visible = (((User.Current.IsAdmin || User.Current.IsModerator) || ClanMember.Current.CanTargetAbility(ClanAbility.Ban, selectedUser.Name)) && (!selectedUser.IsCurrent && (selectedUser is User))) && !(selectedUser as User).IsAdmin;
                        this.ciChat_Kick.Visible = (((User.Current.IsAdmin || User.Current.IsModerator) || ClanMember.Current.CanTargetAbility(ClanAbility.Kick, selectedUser.Name)) && (!selectedUser.IsCurrent && (selectedUser is User))) && !(selectedUser as User).IsAdmin;
                    }
                    else
                    {
                        this.ciChat_Ban.Visible = (((Chatroom.InChatroom && Chatroom.GatheringParticipants.ContainsIndex("name", selectedUser.Name)) && ((User.Current.IsAdmin || User.Current.IsModerator) || (!Chatroom.Current.IsPersistent && User.Current.IsChannelOperator))) && (!selectedUser.IsCurrent && (selectedUser is User))) && !(selectedUser as User).IsAdmin;
                        this.ciChat_Kick.Visible = this.ciChat_Ban.Visible;
                    }
                    int num = -1;
                    for (int i = 0; i < this.gpgContextMenuChat.MenuItems.Count; i++)
                    {
                        if (this.gpgContextMenuChat.MenuItems[i].Text == "-")
                        {
                            if (this.gpgContextMenuChat.MenuItems[num].Text == "-")
                            {
                                this.gpgContextMenuChat.MenuItems[i].Visible = false;
                            }
                            else if (i == (this.gpgContextMenuChat.MenuItems.Count - 1))
                            {
                                this.gpgContextMenuChat.MenuItems[i].Visible = false;
                            }
                            else
                            {
                                this.gpgContextMenuChat.MenuItems[i].Visible = true;
                            }
                        }
                        if (this.gpgContextMenuChat.MenuItems[i].Visible)
                        {
                            num = i;
                        }
                    }
                    if ((num >= 0) && (this.gpgContextMenuChat.MenuItems[num].Text == "-"))
                    {
                        this.gpgContextMenuChat.MenuItems[num].Visible = false;
                    }
                    if (selectedUser != null)
                    {
                        if (User.Current.Name == selectedUser.Name)
                        {
                            this.miRankedChallenge.Visible = false;
                            this.miToggleChallengeDeny.Visible = true;
                            if (base.MainForm.AllowDirectChallenge)
                            {
                                this.miToggleChallengeDeny.Text = Loc.Get("<LOC>Auto Deny Ranked Challenges");
                            }
                            else
                            {
                                this.miToggleChallengeDeny.Text = Loc.Get("<LOC>Allow Ranked Challenges");
                            }
                        }
                        else
                        {
                            this.miRankedChallenge.Visible = true;
                            this.miToggleChallengeDeny.Visible = false;
                        }
                    }
                    if (!ConfigSettings.GetBool("AllowDirectChallenge", false))
                    {
                        this.miRankedChallenge.Visible = false;
                        this.miToggleChallengeDeny.Visible = false;
                    }
                    if (!base.MainForm.btnPlayNow.Visible)
                    {
                        this.miRankedChallenge.Visible = false;
                        base.MainForm.AllowDirectChallenge = false;
                        this.miToggleChallengeDeny.Visible = false;
                        this.ciChat_ViewRank.Visible = false;
                        this.ciChat_WebStats.Visible = false;
                        this.ciChat_TeamInvite.Visible = false;
                    }
                    if (!base.MainForm.btnReplayVault.Visible)
                    {
                        this.miViewReplays.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gpgPanelBody_DoubleClick(object sender, EventArgs e)
        {
            if ((this.SelectedUser != null) && (this.LastClick1 == this.LastClick2))
            {
                base.MainForm.OnSendWhisper(this.SelectedUser.Name, null);
            }
        }

        private void gpgPanelBody_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void gpgPanelBody_MouseLeave(object sender, EventArgs e)
        {
            base.ttDefault.Tag = null;
            base.ttDefault.Hide(this.gpgPanelBody);
        }

        private void gpgPanelBody_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void gpgPanelBody_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        private void gpgPanelBody_Paint(object sender, PaintEventArgs e)
        {
            if (!this.PerformingLayout)
            {
                foreach (UserListCategory category in this.Categories.Values)
                {
                    category.Paint(e.ClipRectangle, e.Graphics);
                }
            }
        }

        private void InitializeComponent()
        {
            this.gpgPanelBody = new GPGPanel();
            this.gpgContextMenuChat = new GPGContextMenu();
            this.ciChat_WhisperPlayer = new MenuItem();
            this.ciChat_IgnorePlayer = new MenuItem();
            this.ciChat_UnignorePlayer = new MenuItem();
            this.ciChat_ViewRank = new MenuItem();
            this.ciChat_WebStats = new MenuItem();
            this.ciChat_ViewPlayer = new MenuItem();
            this.miViewReplays = new MenuItem();
            this.menuItem10 = new MenuItem();
            this.ciChat_InviteFriend = new MenuItem();
            this.ciChat_RemoveFriend = new MenuItem();
            this.menuItem8 = new MenuItem();
            this.ciChat_InviteToClan = new MenuItem();
            this.ciChat_RequestClanInvite = new MenuItem();
            this.ciChat_ViewClan = new MenuItem();
            this.ciChat_LeaveClan = new MenuItem();
            this.menuItem3 = new MenuItem();
            this.ciChat_Kick = new MenuItem();
            this.ciChat_Ban = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.ciChat_TeamInvite = new MenuItem();
            this.miRankedChallenge = new MenuItem();
            this.miToggleChallengeDeny = new MenuItem();
            this.ciChat_Promote = new MenuItem();
            this.ciChat_Demote = new MenuItem();
            this.ciChat_RemoveClanMember = new MenuItem();
            base.SuspendLayout();
            this.gpgPanelBody.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanelBody.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelBody.BorderThickness = 2;
            this.gpgPanelBody.DrawBorder = false;
            this.gpgPanelBody.Location = new Point(0, 0);
            this.gpgPanelBody.Name = "gpgPanelBody";
            this.gpgPanelBody.Size = new Size(0xdf, 0x1a3);
            this.gpgPanelBody.TabIndex = 0;
            this.gpgPanelBody.DoubleClick += new EventHandler(this.gpgPanelBody_DoubleClick);
            this.gpgPanelBody.MouseLeave += new EventHandler(this.gpgPanelBody_MouseLeave);
            this.gpgPanelBody.MouseDown += new MouseEventHandler(this.gpgPanelBody_MouseDown);
            this.gpgPanelBody.MouseMove += new MouseEventHandler(this.gpgPanelBody_MouseMove);
            this.gpgPanelBody.Paint += new PaintEventHandler(this.gpgPanelBody_Paint);
            this.gpgPanelBody.MouseUp += new MouseEventHandler(this.gpgPanelBody_MouseUp);
            this.gpgContextMenuChat.MenuItems.AddRange(new MenuItem[] { 
                this.ciChat_WhisperPlayer, this.ciChat_IgnorePlayer, this.ciChat_UnignorePlayer, this.ciChat_ViewRank, this.ciChat_WebStats, this.ciChat_ViewPlayer, this.miViewReplays, this.menuItem10, this.ciChat_InviteFriend, this.ciChat_RemoveFriend, this.menuItem8, this.ciChat_InviteToClan, this.ciChat_RequestClanInvite, this.ciChat_ViewClan, this.ciChat_Promote, this.ciChat_Demote, 
                this.ciChat_RemoveClanMember, this.ciChat_LeaveClan, this.menuItem3, this.ciChat_Kick, this.ciChat_Ban, this.menuItem2, this.ciChat_TeamInvite, this.miRankedChallenge, this.miToggleChallengeDeny
             });
            this.gpgContextMenuChat.Popup += new EventHandler(this.gpgContextMenuChat_Popup);
            this.ciChat_WhisperPlayer.Index = 0;
            this.ciChat_WhisperPlayer.Text = "<LOC>Send private message";
            this.ciChat_WhisperPlayer.Click += new EventHandler(this.ciChat_WhisperPlayer_Click);
            this.ciChat_IgnorePlayer.Index = 1;
            this.ciChat_IgnorePlayer.Text = "<LOC>Ignore Player";
            this.ciChat_IgnorePlayer.Click += new EventHandler(this.ciChat_IgnorePlayer_Click);
            this.ciChat_UnignorePlayer.Index = 2;
            this.ciChat_UnignorePlayer.Text = "<LOC>Unignore Player";
            this.ciChat_UnignorePlayer.Click += new EventHandler(this.ciChat_UnignorePlayer_Click);
            this.ciChat_ViewRank.Index = 3;
            this.ciChat_ViewRank.Text = "<LOC>View in ranking ladder";
            this.ciChat_ViewRank.Click += new EventHandler(this.ciChat_ViewRank_Click);
            this.ciChat_WebStats.Index = 4;
            this.ciChat_WebStats.Text = "<LOC>View web stats";
            this.ciChat_WebStats.Click += new EventHandler(this.ciChat_WebStats_Click);
            this.ciChat_ViewPlayer.Index = 5;
            this.ciChat_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChat_ViewPlayer.Click += new EventHandler(this.ciChat_ViewPlayer_Click);
            this.miViewReplays.Index = 6;
            this.miViewReplays.Text = "<LOC>View this player's Replays";
            this.miViewReplays.Click += new EventHandler(this.miViewReplays_Click);
            this.menuItem10.Index = 7;
            this.menuItem10.Text = "-";
            this.ciChat_InviteFriend.Index = 8;
            this.ciChat_InviteFriend.Text = "<LOC>Invite player to join Friends list";
            this.ciChat_InviteFriend.Click += new EventHandler(this.ciChat_InviteFriend_Click);
            this.ciChat_RemoveFriend.Index = 9;
            this.ciChat_RemoveFriend.Text = "<LOC>Remove player from Friends list";
            this.ciChat_RemoveFriend.Click += new EventHandler(this.ciChat_RemoveFriend_Click);
            this.menuItem8.Index = 10;
            this.menuItem8.Text = "-";
            this.ciChat_InviteToClan.Index = 11;
            this.ciChat_InviteToClan.Text = "<LOC>Invite this player to join clan";
            this.ciChat_InviteToClan.Click += new EventHandler(this.ciChat_InviteToClan_Click);
            this.ciChat_RequestClanInvite.Index = 12;
            this.ciChat_RequestClanInvite.Text = "<LOC>Request to join this player's clan";
            this.ciChat_RequestClanInvite.Click += new EventHandler(this.ciChat_RequestClanInvite_Click);
            this.ciChat_ViewClan.Index = 13;
            this.ciChat_ViewClan.Text = "<LOC>View this clan's profile";
            this.ciChat_ViewClan.Click += new EventHandler(this.ciChat_ViewClan_Click);
            this.ciChat_LeaveClan.Index = 0x11;
            this.ciChat_LeaveClan.Text = "<LOC>Leave clan";
            this.ciChat_LeaveClan.Click += new EventHandler(this.ciChat_LeaveClan_Click);
            this.menuItem3.Index = 0x12;
            this.menuItem3.Text = "-";
            this.ciChat_Kick.Index = 0x13;
            this.ciChat_Kick.Text = "<LOC>Kick";
            this.ciChat_Kick.Click += new EventHandler(this.ciChat_Kick_Click);
            this.ciChat_Ban.Index = 20;
            this.ciChat_Ban.Text = "<LOC>Ban";
            this.ciChat_Ban.Click += new EventHandler(this.ciChat_Ban_Click);
            this.menuItem2.Index = 0x15;
            this.menuItem2.Text = "-";
            this.ciChat_TeamInvite.Index = 0x16;
            this.ciChat_TeamInvite.Text = "<LOC>Invite to Arranged Team";
            this.ciChat_TeamInvite.Click += new EventHandler(this.ciChat_TeamInvite_Click);
            this.miRankedChallenge.Index = 0x17;
            this.miRankedChallenge.Text = "<LOC>Ranked Challenge";
            this.miRankedChallenge.Click += new EventHandler(this.miRankedChallenge_Click);
            this.miToggleChallengeDeny.Index = 0x18;
            this.miToggleChallengeDeny.Text = "<LOC>Auto Deny Ranked Challenges";
            this.miToggleChallengeDeny.Click += new EventHandler(this.miToggleChallengeDeny_Click);
            this.ciChat_Promote.Index = 14;
            this.ciChat_Promote.Text = "<LOC>Promote";
            this.ciChat_Promote.Click += new EventHandler(this.ciChat_Promote_Click);
            this.ciChat_Demote.Index = 15;
            this.ciChat_Demote.Text = "<LOC>Demote";
            this.ciChat_Demote.Click += new EventHandler(this.ciChat_Demote_Click);
            this.ciChat_RemoveClanMember.Index = 0x10;
            this.ciChat_RemoveClanMember.Text = "<LOC>Remove from Clan";
            this.ciChat_RemoveClanMember.Click += new EventHandler(this.ciChat_RemoveClanMember_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            base.Controls.Add(this.gpgPanelBody);
            base.Name = "PnlUserList";
            base.Size = new Size(0xdf, 0x1a3);
            base.PreviewKeyDown += new PreviewKeyDownEventHandler(this.PnlUserList_PreviewKeyDown);
            base.ResumeLayout(false);
        }

        private void InitializePlugins()
        {
            Plugins.LoadPlugins();
            foreach (PluginInfo info in Plugins.GetPlugins())
            {
                if ((info.Location == PluginLocation.Both) || (info.Location == PluginLocation.UserList))
                {
                    string menuCaption = info.MenuCaption;
                    if (menuCaption.IndexOf(",") < 0)
                    {
                        menuCaption = "Plugins," + menuCaption;
                    }
                    string[] strArray = menuCaption.Split(new char[] { ',' });
                    Menu.MenuItemCollection menuItems = this.gpgContextMenuChat.MenuItems;
                    MenuItem item = null;
                    for (int i = 0; i < (strArray.Length - 1); i++)
                    {
                        MenuItem current;
                        string str2 = strArray[i];
                        bool flag = false;
                        using (IEnumerator enumerator2 = menuItems.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                current = (MenuItem) enumerator2.Current;
                                if (current.Text.ToUpper() == str2.ToUpper())
                                {
                                    flag = true;
                                    menuItems = current.MenuItems;
                                    current.Visible = true;
                                    item = current;
                                    goto Label_0129;
                                }
                            }
                        }
                    Label_0129:
                        if (!flag)
                        {
                            current = new MenuItem(str2);
                            if (item == null)
                            {
                                this.gpgContextMenuChat.MenuItems.Add(current);
                            }
                            else
                            {
                                item.MenuItems.Add(current);
                            }
                            item = current;
                        }
                    }
                    string text = strArray[strArray.Length - 1];
                    MenuItem item3 = new MenuItem(text);
                    item3.Tag = info;
                    item3.Click += new EventHandler(this.pluginitem_Click);
                    item.MenuItems.Add(item3);
                }
            }
        }

        protected override void Localize()
        {
            base.Localize();
            this.gpgContextMenuChat.Localize();
        }

        private void miRankedChallenge_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.DirectChallengePlayer(this.SelectedChatParticipant.Name);
                base.MainForm.Focus();
            }
        }

        private void miToggleChallengeDeny_Click(object sender, EventArgs e)
        {
            base.MainForm.ToggleDirectChallenge();
            this.FoolMenu();
        }

        private void miViewReplays_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.ShowDlgSearchReplays(this.SelectedChatParticipant.Name);
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            this.OnRequestRefresh();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.VerticalScroll.SmallChange = 0x18;
            this.ContextMenu = this.gpgContextMenuChat;
            this.gpgPanelBody.ContextMenu = this.gpgContextMenuChat;
            base.OnLoad(e);
            Program.Settings.Awards.ShowAvatarsChanged += new PropertyChangedEventHandler(this.AwardVisibilityChanged);
            Program.Settings.Awards.ShowAwardsChanged += new PropertyChangedEventHandler(this.AwardVisibilityChanged);
            Program.Settings.Appearance.Chat.ColorsChanged += new PropertyChangedEventHandler(this.AppearanceSettingsChanged);
            Program.Settings.Appearance.Chat.FontsChanged += new PropertyChangedEventHandler(this.AppearanceSettingsChanged);
            foreach (MenuItem item in this.gpgContextMenuChat.MenuItems)
            {
                item.Click += new EventHandler(this.ContextMenuItemClick);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.BackColor = Color.FromArgb(0x24, 0x23, 0x23);
            this.gpgPanelBody.BackColor = Color.FromArgb(0x24, 0x23, 0x23);
            base.OnPaint(e);
        }

        private void OnRequestRefresh()
        {
            if (this.Style == UserListStyles.Chatroom)
            {
                if (this.RequestRefresh != null)
                {
                    this.RequestRefresh(this, EventArgs.Empty);
                }
            }
            else if (!this.PerformingLayout)
            {
                this.RefreshData();
            }
        }

        public void PerformCategoryLayout()
        {
            VGen0 method = null;
            int top = 0;
            try
            {
                UserListRow row;
                UserListRow row2;
                if (this.PerformingLayout)
                {
                    return;
                }
                this.mPerformingLayout = true;
                int num = 0;
                if (this.Style != UserListStyles.Clan)
                {
                    goto Label_0175;
                }
                this.ClearData();
                if (this.ClanMembers != null)
                {
                    foreach (ClanMember member in this.ClanMembers)
                    {
                        row = new UserListRow(this, member, UserListStyles.Clan, this.DetermineUserCategory(member));
                        row.MouseDown += new MouseEventHandler(this.row_MouseDown);
                        row.MouseUp += new MouseEventHandler(this.row_MouseUp);
                        row.MoveToCategory(this.Categories[row.Category]);
                    }
                }
                goto Label_0273;
            Label_0101:
                row = this.RemovedRows.Dequeue();
                this.Categories[row.Category].RemoveRow(row);
                this.UserRows.Remove(row.User);
                Chatroom.GatheringParticipants.RemoveByIndex("name", row.User.Name);
                this.DisposeRow(row);
                if (this.SelectedRow == row)
                {
                    this.mSelectedRow = null;
                }
            Label_0175:
                if (this.RemovedRows.Count > 0)
                {
                    goto Label_0101;
                }
                while (this.AddedRows.Count > 0)
                {
                    row = this.AddedRows.Dequeue();
                    this.UserRows[row.User] = row;
                    row.MoveToCategory(this.Categories[row.Category]);
                }
                while (this.UpdatedUsers.Count > 0)
                {
                    User key = this.UpdatedUsers.Dequeue();
                    if (this.UserRows.ContainsKey(key))
                    {
                        row = this.UserRows[key];
                        UserListCategories categories = this.DetermineUserCategory(key);
                        Chatroom.GatheringParticipants.ReplaceIntoByIndex("name", key.Name, key);
                        if (categories != row.Category)
                        {
                            row.MoveToCategory(this.Categories[categories]);
                        }
                        row.BindToUser(key);
                    }
                }
            Label_0273:
                row2 = null;
                foreach (UserListCategory category in this.Categories.Values)
                {
                    if (category.Visible && (category.Count >= 1))
                    {
                        category.SetBounds(top);
                        top += category.Height + num;
                        foreach (UserListRow row in category)
                        {
                            if (category.IsExpanded)
                            {
                                row.SetBounds(top);
                                row.PreviousRow = row2;
                                row.NextRow = null;
                                if (row2 != null)
                                {
                                    row2.NextRow = row;
                                }
                                row2 = row;
                                top += row.Height + num;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                this.mPerformingLayout = false;
            }
            if ((base.IsHandleCreated && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.gpgPanelBody.Height = top;
                        this.gpgPanelBody.Invalidate();
                        this.gpgPanelBody.Update();
                    };
                }
                base.Invoke(method);
            }
        }

        private void pluginitem_Click(object sender, EventArgs e)
        {
            this.FoolMenu();
            if (this.SelectedChatParticipant != null)
            {
                MenuItem item = sender as MenuItem;
                IFormPlugin plugin = (item.Tag as PluginInfo).CreatePlugin();
                UserControl control = plugin as UserControl;
                if (control != null)
                {
                    DlgBase base2 = new DlgBase();
                    base2.Show();
                    base2.Width = control.Width + 40;
                    base2.Height = control.Height + 100;
                    base2.Text = plugin.FormTitle;
                    control.Top = 50;
                    control.Left = 20;
                    base2.Controls.Add(control);
                    control.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                    control.Width = base2.Width - 40;
                    control.Height = base2.Height - 100;
                    plugin.SetUserInformation(this.SelectedChatParticipant.Name, this.SelectedChatParticipant.ID);
                }
            }
        }

        private void PnlUserList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if ((this.SelectedRow != null) && (this.SelectedRow.PreviousRow != null))
                    {
                        this.SelectedRow.IsSelected = false;
                        this.mSelectedRow = this.SelectedRow.PreviousRow;
                        this.SelectedRow.IsSelected = true;
                        if (!this.VisibleBounds.Contains(1, this.SelectedRow.Top))
                        {
                            if ((base.VerticalScroll.Value - (base.VerticalScroll.SmallChange * 4)) < 0)
                            {
                                base.VerticalScroll.Value = 0;
                            }
                            else
                            {
                                VScrollProperties verticalScroll = base.VerticalScroll;
                                verticalScroll.Value -= base.VerticalScroll.SmallChange * 4;
                            }
                        }
                        this.gpgPanelBody.Invalidate();
                        this.gpgPanelBody.Update();
                        break;
                    }
                    break;

                case Keys.Down:
                    if ((this.SelectedRow != null) && (this.SelectedRow.NextRow != null))
                    {
                        this.SelectedRow.IsSelected = false;
                        this.mSelectedRow = this.SelectedRow.NextRow;
                        this.SelectedRow.IsSelected = true;
                        if (!this.VisibleBounds.Contains(1, this.SelectedRow.Top + this.SelectedRow.Height))
                        {
                            if ((base.VerticalScroll.Value + (base.VerticalScroll.SmallChange * 4)) > base.VerticalScroll.Maximum)
                            {
                                base.VerticalScroll.Value = base.VerticalScroll.Maximum;
                            }
                            else
                            {
                                VScrollProperties properties2 = base.VerticalScroll;
                                properties2.Value += base.VerticalScroll.SmallChange * 4;
                            }
                        }
                        this.gpgPanelBody.Invalidate();
                        this.gpgPanelBody.Update();
                        break;
                    }
                    break;

                case Keys.Return:
                    if (this.SelectedUser != null)
                    {
                        base.MainForm.OnSendWhisper(this.SelectedUser.Name, null);
                    }
                    break;
            }
        }

        public void RefreshData()
        {
            this.PerformCategoryLayout();
        }

        public void RemoveUser(User user)
        {
            if (this.UserRows.ContainsKey(user))
            {
                UserListRow item = this.UserRows[user];
                this.RemovedRows.Enqueue(item);
                if (this.AutoRefresh)
                {
                    this.RefreshData();
                }
            }
        }

        private void row_MouseDown(object sender, MouseEventArgs e)
        {
            UserListRow row = sender as UserListRow;
            this.LastClick2 = this.LastClick1;
            this.LastClick1 = row;
            if (this.SelectedRow != row)
            {
                if (this.SelectedRow != null)
                {
                    this.SelectedRow.IsSelected = false;
                }
                this.mSelectedRow = row;
                this.gpgPanelBody.Invalidate();
                this.gpgPanelBody.Update();
            }
        }

        private void row_MouseUp(object sender, MouseEventArgs e)
        {
            if (((this.SelectedRow != null) && (this.SelectedUser != null)) && (e.Button == MouseButtons.Right))
            {
                this.gpgContextMenuChat.Show(this.gpgPanelBody, e.Location);
            }
        }

        public void UpdateUser(User user)
        {
            this.UpdatedUsers.Enqueue(user);
            if (this.AutoRefresh)
            {
                this.RefreshData();
            }
        }

        [Browsable(true)]
        public bool AutoRefresh
        {
            get
            {
                return this.mAutoRefresh;
            }
            set
            {
                this.mAutoRefresh = value;
            }
        }

        public Dictionary<UserListCategories, UserListCategory> Categories
        {
            get
            {
                return this.mCategories;
            }
        }

        public MappedObjectList<ClanMember> ClanMembers
        {
            get
            {
                if (this.mClanMembers == null)
                {
                    return Clan.CurrentMembers;
                }
                return this.mClanMembers;
            }
            set
            {
                this.mClanMembers = value;
                if (this.AutoRefresh)
                {
                    this.RefreshData();
                }
            }
        }

        public bool IsBound
        {
            get
            {
                return this.mIsBound;
            }
        }

        public bool PerformingLayout
        {
            get
            {
                return this.mPerformingLayout;
            }
        }

        protected override bool ScaleChildren
        {
            get
            {
                return false;
            }
        }

        public IUser SelectedChatParticipant
        {
            get
            {
                return this.SelectedUser;
            }
        }

        public UserListRow SelectedRow
        {
            get
            {
                return this.mSelectedRow;
            }
        }

        public IUser SelectedUser
        {
            get
            {
                if (this.SelectedRow != null)
                {
                    return this.SelectedRow.Member;
                }
                return null;
            }
        }

        [Browsable(true)]
        public UserListStyles Style
        {
            get
            {
                return this.mStyle;
            }
            set
            {
                this.BindToList(value);
            }
        }

        public Dictionary<User, UserListRow> UserRows
        {
            get
            {
                return this.mUserRows;
            }
        }

        public Rectangle VisibleBounds
        {
            get
            {
                return new Rectangle(0, base.VerticalScroll.Value, base.Width, base.Height);
            }
        }
    }
}

