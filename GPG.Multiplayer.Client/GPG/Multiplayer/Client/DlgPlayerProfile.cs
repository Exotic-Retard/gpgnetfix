namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgPlayerProfile : DlgBase
    {
        private SkinLabel backLabelFriends;
        private SkinLabel backLabelStats;
        private MenuItem ciChat_IgnorePlayer;
        private MenuItem ciChat_InviteFriend;
        private MenuItem ciChat_InviteToClan;
        private MenuItem ciChat_RemoveFriend;
        private MenuItem ciChat_RequestClanInvite;
        private MenuItem ciChat_UnignorePlayer;
        private MenuItem ciChat_ViewClan;
        private MenuItem ciChat_ViewPlayer;
        private MenuItem ciChat_WebStats;
        private MenuItem ciChat_WhisperPlayer;
        private IContainer components;
        private Dictionary<string, TextContainer> FriendContainerLookup;
        private BoundContainerList FriendContainers;
        private GridColumn gcFriend_Count;
        private GridColumn gcFriend_Icon;
        private GridColumn gcFriend_Text;
        private GridColumn gcFriend_Title;
        private GPGContextMenu gpgContextMenuChat;
        private GPGChatGrid gpgGridFriends;
        private GPGLabel gpgLabelDescription;
        private SkinLabel gpgLabelName;
        private SkinLabel gpgLinkLabelClan;
        private GPGPanel gpgPanel2;
        private SplitContainer gpgPanelEdit;
        private GPGPanel gpgPanelStats;
        private GPGScrollPanel gpgScrollPanelFriends;
        private GPGStatRow gpgStatRow1;
        private GPGStatRow gpgStatRow2;
        private GPGStatRow gpgStatRow3;
        private GPGTextArea gpgTextAreaDescription;
        private GridView gvFriend_Container;
        private GridView gvFriend_Member;
        private PlayerView mCurrentPlayer;
        private PlayerRating mCurrentStats;
        private bool mDescriptionModified;
        private MenuItem menuItem10;
        private MenuItem menuItem8;
        private GridView mSelectedParticipantView;
        private LinkedList<PlayerView> mViewList;
        private PictureBox pictureBoxClanIcon;
        private PictureBox pictureBoxClanRank;
        private PictureBox pictureBoxPlayerIcon;
        private Dictionary<string, PlayerView> PlayerViewLookups;
        private RepositoryItemPictureEdit repositoryItemPictureEdit4;
        private RepositoryItemTextEdit repositoryItemTextEdit2;
        private SkinButton skinButtonCancelEdit;
        private SkinButton skinButtonClose;
        private SkinButton skinButtonEdit;
        private SkinButton skinButtonLast;
        private SkinButton skinButtonNext;
        private SkinButton skinButtonSaveEdit;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private SplitContainer splitContainerFriends;

        public DlgPlayerProfile(FrmMain mainform) : base(mainform)
        {
            this.components = null;
            this.mCurrentStats = null;
            this.mViewList = new LinkedList<PlayerView>();
            this.PlayerViewLookups = new Dictionary<string, PlayerView>();
            this.FriendContainers = new BoundContainerList();
            this.FriendContainerLookup = new Dictionary<string, TextContainer>();
            this.mSelectedParticipantView = null;
            this.mDescriptionModified = false;
            this.InitializeComponent();
            this.gvFriend_Member.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gvFriend_Member_CustomDrawCell);
            this.gvFriend_Member.MouseDown += new MouseEventHandler(this.SetCurrentGridView);
            this.gvFriend_Member.MouseUp += new MouseEventHandler(this.ChatGridMouseDown);
            this.gvFriend_Member.DoubleClick += new EventHandler(this.gvFriend_Member_DoubleClick);
        }

        private void AddFriendParticipant(User friend)
        {
            TextLine line = new TextLine(this.gpgGridFriends);
            line.Tag = friend;
            line.TextFont = Program.Settings.Chat.Appearance.DefaultFont;
            line.TextColor = Program.Settings.Chat.Appearance.FriendsColor;
            string str = null;
            if (friend.Online)
            {
                str = "Online";
            }
            else
            {
                str = "Offline";
                line.TextColor = Program.Settings.Chat.Appearance.UnavailableColor;
            }
            line.AddSegment(new TextSegment(friend.Name, true));
            if (friend.IsInClan)
            {
                line.AddSegment(new TextSegment(friend.ClanAbbreviation, Program.Settings.Chat.Appearance.ClanColor, Program.Settings.Chat.Appearance.ClanTagFont));
            }
            this.FriendContainerLookup[str].Add(line);
        }

        private void CalcGridHeight()
        {
            int rowCount = this.gvFriend_Container.RowCount;
            for (int i = 0; i < this.gvFriend_Container.RowCount; i++)
            {
                BaseView detailView = this.gvFriend_Container.GetDetailView(this.gvFriend_Container.GetVisibleRowHandle(i), 0);
                if (detailView != null)
                {
                    rowCount += detailView.RowCount;
                }
            }
            this.gpgGridFriends.Height = rowCount * 20;
        }

        private void CancelEdits()
        {
            this.gpgLabelDescription.Visible = true;
            this.gpgTextAreaDescription.Visible = false;
            this.skinButtonEdit.Visible = true;
            this.skinButtonCancelEdit.Visible = false;
            this.skinButtonSaveEdit.Visible = false;
            this.mDescriptionModified = false;
            this.gpgPanelEdit.Height -= this.gpgTextAreaDescription.Height;
            this.gpgPanelEdit.Panel2Collapsed = true;
        }

        private void ChatGridMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TextLine row = (sender as GridView).GetRow((sender as GridView).FocusedRowHandle) as TextLine;
                if ((row != null) && ((row.Tag != null) && (row.Tag is User)))
                {
                    User tag = row.Tag as User;
                    this.ciChat_WhisperPlayer.Visible = !User.Current.Equals(tag) && tag.Online;
                    this.ciChat_ViewPlayer.Visible = true;
                    this.ciChat_IgnorePlayer.Visible = !tag.IsCurrent && !tag.IsIgnored;
                    this.ciChat_UnignorePlayer.Visible = !tag.IsCurrent && tag.IsIgnored;
                    this.ciChat_WebStats.Visible = ConfigSettings.GetBool("WebStatsEnabled", false);
                    this.ciChat_InviteToClan.Visible = (User.Current.IsInClan && !tag.IsInClan) && !tag.IsCurrent;
                    this.ciChat_RequestClanInvite.Visible = (!User.Current.IsInClan && tag.IsInClan) && !tag.IsCurrent;
                    this.ciChat_ViewClan.Visible = tag.IsInClan;
                    this.ciChat_InviteFriend.Visible = !tag.IsFriend && !tag.IsCurrent;
                    this.ciChat_RemoveFriend.Visible = tag.IsFriend && !tag.IsCurrent;
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
                    this.gpgContextMenuChat.Show((sender as GridView).GridControl, e.Location);
                }
            }
        }

        private void ciChat_IgnorePlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.IgnorePlayer(this.SelectedChatParticipant.Name);
            }
            else
            {
                base.MainForm.IgnorePlayer(this.CurrentPlayer.Player.Name);
            }
        }

        private void ciChat_InviteFriend_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.InviteAsFriend(this.SelectedChatParticipant.Name);
            }
            else
            {
                base.MainForm.InviteAsFriend(this.CurrentPlayer.Player.Name);
            }
        }

        private void ciChat_InviteToClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.InviteToClan(this.SelectedChatParticipant.Name);
            }
            else
            {
                base.MainForm.InviteToClan(this.CurrentPlayer.Player.Name);
            }
        }

        private void ciChat_RemoveFriend_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.RemoveFriend(this.SelectedChatParticipant.Name, this.SelectedChatParticipant.ID);
            }
            else
            {
                base.MainForm.RemoveFriend(this.CurrentPlayer.Player.Name, this.CurrentPlayer.Player.ID);
            }
        }

        private void ciChat_RequestClanInvite_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.RequestClanInvite(this.SelectedChatParticipant.Name);
            }
            else
            {
                base.MainForm.RequestClanInvite(this.CurrentPlayer.Player.Name);
            }
        }

        private void ciChat_UnignorePlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.UnignorePlayer(this.SelectedChatParticipant.Name);
            }
            else
            {
                base.MainForm.UnignorePlayer(this.CurrentPlayer.Player.Name);
            }
        }

        private void ciChat_ViewClan_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.OnViewClanProfileByPlayer(this.SelectedChatParticipant.Name);
            }
            else
            {
                base.MainForm.OnViewClanProfileByName(this.CurrentPlayer.Player.ClanName);
            }
        }

        private void ciChat_ViewPlayer_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                this.SetTargetPlayer(this.SelectedChatParticipant.Name);
                this.Construct();
            }
        }

        private void ciChat_WebStats_Click(object sender, EventArgs e)
        {
            if (this.SelectedChatParticipant != null)
            {
                base.MainForm.ShowWebStats(this.SelectedChatParticipant.ID);
            }
        }

        private void ciChat_WhisperPlayer_Click(object sender, EventArgs e)
        {
            if ((this.SelectedChatParticipant != null) && this.SelectedChatParticipant.Online)
            {
                base.MainForm.OnSendWhisper(this.SelectedChatParticipant.Name, null);
            }
            else
            {
                base.MainForm.OnSendWhisper(this.CurrentPlayer.Player.Name, null);
            }
        }

        internal void Construct()
        {
            if (!this.ViewList.Contains(this.CurrentPlayer))
            {
                this.ViewList.AddLast(new LinkedListNode<PlayerView>(this.CurrentPlayer));
            }
            this.skinButtonNext.Visible = !this.ViewList.Last.Value.Equals(this.CurrentPlayer);
            this.skinButtonLast.Visible = !this.ViewList.First.Value.Equals(this.CurrentPlayer);
            User player = this.CurrentPlayer.Player;
            MappedObjectList<User> friends = this.CurrentPlayer.Friends;
            this.Text = string.Format(Loc.Get("<LOC>Profile: {0}"), player.Name);
            this.Refresh();
            this.gpgLabelName.Text = player.Name;
            this.pictureBoxPlayerIcon.Image = player.PlayerIcon;
            if (this.pictureBoxPlayerIcon.Image == null)
            {
                this.pictureBoxPlayerIcon.Visible = false;
            }
            if (((player.Description == null) || (player.Description == "")) || (player.Description == "(null)"))
            {
                this.gpgLabelDescription.Text = Loc.Get("<LOC>(no description)");
                this.gpgTextAreaDescription.Text = "";
            }
            else
            {
                this.gpgLabelDescription.Text = player.Description;
                this.gpgTextAreaDescription.Text = player.Description;
            }
            this.ResizeDescriptionText();
            using (Graphics graphics = base.CreateGraphics())
            {
                this.gpgLabelDescription.Height = Convert.ToInt32(DrawUtil.MeasureString(graphics, this.gpgLabelDescription.Text, this.gpgLabelDescription.Font, (float) this.gpgLabelDescription.Width).Height) + 10;
            }
            if (player.Equals(User.Current))
            {
                this.skinButtonEdit.Visible = true;
                this.gpgPanelEdit.Visible = true;
                this.gpgPanelEdit.Height -= this.gpgTextAreaDescription.Height;
                this.gpgPanelEdit.Panel2Collapsed = true;
            }
            else
            {
                this.skinButtonEdit.Visible = false;
                this.gpgPanelEdit.Visible = false;
            }
            int num = 4;
            if (player.IsInClan)
            {
                this.pictureBoxClanIcon.Visible = true;
                this.pictureBoxClanRank.Visible = true;
                this.gpgLinkLabelClan.Visible = true;
                this.gpgLinkLabelClan.Text = player.ClanAbbreviation;
                this.gpgLinkLabelClan.Location = new Point(this.gpgLabelName.TextWidth + 6, this.gpgLabelName.Top);
                this.gpgLinkLabelClan.Width = this.gpgLinkLabelClan.TextWidth + 4;
                this.gpgLinkLabelClan.BringToFront();
                this.gpgLinkLabelClan.ForeColor = Program.Settings.Chat.Appearance.ClanColor;
                string imageSource = ClanRanking.FindBySeniority(player.Rank).ImageSource;
                object obj2 = ClanImages.ResourceManager.GetObject(imageSource);
                this.pictureBoxClanRank.Image = obj2 as Image;
                this.pictureBoxClanIcon.Visible = false;
                if (this.pictureBoxPlayerIcon.Image != null)
                {
                    this.pictureBoxPlayerIcon.Left = ((this.gpgLinkLabelClan.Width - this.pictureBoxClanRank.Width) - num) - this.pictureBoxPlayerIcon.Width;
                }
                this.gpgLinkLabelClan.Refresh();
            }
            else
            {
                this.pictureBoxClanIcon.Visible = false;
                this.pictureBoxClanRank.Visible = false;
                this.gpgLinkLabelClan.Visible = false;
                this.pictureBoxPlayerIcon.Left = this.gpgLinkLabelClan.Width - this.pictureBoxPlayerIcon.Width;
            }
            if (friends.Count > 0)
            {
                this.gvFriend_Member.DetailHeight = 0x1f40;
                this.gpgScrollPanelFriends.Visible = true;
                this.FriendContainers = new BoundContainerList();
                this.FriendContainerLookup = new Dictionary<string, TextContainer>();
                this.gvFriend_Container.Columns["Count"].FilterInfo = new ColumnFilterInfo("[Count] > 0");
                this.gpgScrollPanelFriends.ChildControl = this.gpgGridFriends;
                this.FriendContainerLookup["Online"] = new TextContainer(Loc.Get("<LOC>Friends Online"));
                this.FriendContainers.Add(this.FriendContainerLookup["Online"]);
                this.FriendContainerLookup["Offline"] = new TextContainer(Loc.Get("<LOC>Friends Offline"));
                this.FriendContainers.Add(this.FriendContainerLookup["Offline"]);
                foreach (User user2 in friends)
                {
                    this.AddFriendParticipant(user2);
                }
                this.FriendContainerLookup["Online"].Sort();
                this.FriendContainerLookup["Offline"].Sort();
                this.gpgGridFriends.DataSource = null;
                this.gpgGridFriends.DataSource = this.FriendContainers;
            }
            else
            {
                this.backLabelFriends.Text = Loc.Get("<LOC>0 Friends Found");
                this.gpgScrollPanelFriends.Visible = false;
            }
            this.mCurrentStats = this.CurrentPlayer.Player.Rating_1v1;
            this.LoadStats();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
        }

        private void gpgLabelDescription_TextChanged(object sender, EventArgs e)
        {
            this.ResizeDescriptionText();
        }

        private void gpgLabelDescription_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void gpgLabelName_DoubleClick(object sender, EventArgs e)
        {
            if (!(this.CurrentPlayer.Player.Equals(User.Current) || !this.CurrentPlayer.Player.Online))
            {
                base.MainForm.OnSendWhisper(this.CurrentPlayer.Player.Name, null);
            }
        }

        private void gpgLabelName_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    User player = this.CurrentPlayer.Player;
                    this.ciChat_WhisperPlayer.Visible = !User.Current.Equals(player) && player.Online;
                    this.ciChat_ViewPlayer.Visible = false;
                    this.ciChat_IgnorePlayer.Visible = !player.IsCurrent && !player.IsIgnored;
                    this.ciChat_UnignorePlayer.Visible = !player.IsCurrent && player.IsIgnored;
                    this.ciChat_InviteToClan.Visible = (User.Current.IsInClan && !player.IsInClan) && !player.IsCurrent;
                    this.ciChat_RequestClanInvite.Visible = (!User.Current.IsInClan && player.IsInClan) && !player.IsCurrent;
                    this.ciChat_ViewClan.Visible = player.IsInClan;
                    this.ciChat_InviteFriend.Visible = !player.IsFriend && !player.IsCurrent;
                    this.ciChat_RemoveFriend.Visible = player.IsFriend && !player.IsCurrent;
                    int num = -1;
                    for (int i = 0; i < this.gpgContextMenuChat.MenuItems.Count; i++)
                    {
                        if (this.gpgContextMenuChat.MenuItems[i].Text == "-")
                        {
                            if ((num < 0) || (this.gpgContextMenuChat.MenuItems[num].Text == "-"))
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
                    this.gpgContextMenuChat.Show(this.gpgLabelName, e.Location);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void gpgLinkLabelClan_Click(object sender, EventArgs e)
        {
            base.MainForm.OnViewClanProfileByPlayer(this.CurrentPlayer.Player.Name);
        }

        private void gpgTextAreaDescription_KeyDown(object sender, KeyEventArgs e)
        {
            this.mDescriptionModified = true;
            if ((e.KeyCode == Keys.S) && !((!e.Control || e.Alt) || e.Shift))
            {
                this.SaveEdits();
            }
        }

        private void gvFriend_Container_RowCountChanged(object sender, EventArgs e)
        {
            this.CalcGridHeight();
        }

        private void gvFriend_Member_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Text")
            {
                object row = (sender as GridView).GetRow(e.RowHandle);
                if (row is TextLine)
                {
                    TextLine line = row as TextLine;
                    if (line != null)
                    {
                        float left = e.Bounds.Left;
                        foreach (TextSegment segment in line.TextSegments)
                        {
                            Font textFont;
                            Color textColor;
                            if (segment.TextFont != null)
                            {
                                textFont = segment.TextFont;
                            }
                            else
                            {
                                textFont = Program.Settings.Chat.Appearance.DefaultFont;
                            }
                            if (segment.TextColor != Color.Empty)
                            {
                                textColor = segment.TextColor;
                            }
                            else if (line.TextColor != Color.Empty)
                            {
                                textColor = line.TextColor;
                            }
                            else
                            {
                                textColor = Program.Settings.Chat.Appearance.DefaultColor;
                            }
                            int num2 = (e.Bounds.Height - textFont.Height) / 2;
                            using (Brush brush = new SolidBrush(textColor))
                            {
                                e.Graphics.DrawString(segment.Text, textFont, brush, (PointF) new Point((int) left, e.Bounds.Y + num2));
                                left += e.Graphics.MeasureString(segment.Text, textFont).Width;
                            }
                        }
                        e.Handled = true;
                    }
                }
            }
        }

        private void gvFriend_Member_DoubleClick(object sender, EventArgs e)
        {
            if ((this.SelectedChatParticipant != null) && this.SelectedChatParticipant.Online)
            {
                base.MainForm.OnSendWhisper(this.SelectedChatParticipant.Name, null);
            }
        }

        private void gvFriend_Member_RowCountChanged(object sender, EventArgs e)
        {
            this.CalcGridHeight();
        }

        private void InitializeComponent()
        {
            GridLevelNode node = new GridLevelNode();
            this.gvFriend_Member = new GridView();
            this.gcFriend_Icon = new GridColumn();
            this.repositoryItemPictureEdit4 = new RepositoryItemPictureEdit();
            this.gcFriend_Text = new GridColumn();
            this.gpgGridFriends = new GPGChatGrid();
            this.gvFriend_Container = new GridView();
            this.gcFriend_Title = new GridColumn();
            this.gcFriend_Count = new GridColumn();
            this.repositoryItemTextEdit2 = new RepositoryItemTextEdit();
            this.splitContainer1 = new SplitContainer();
            this.splitContainer2 = new SplitContainer();
            this.gpgLinkLabelClan = new SkinLabel();
            this.gpgLabelName = new SkinLabel();
            this.pictureBoxClanIcon = new PictureBox();
            this.pictureBoxClanRank = new PictureBox();
            this.pictureBoxPlayerIcon = new PictureBox();
            this.gpgPanelStats = new GPGPanel();
            this.gpgPanel2 = new GPGPanel();
            this.gpgStatRow3 = new GPGStatRow();
            this.gpgStatRow2 = new GPGStatRow();
            this.gpgLabelDescription = new GPGLabel();
            this.gpgStatRow1 = new GPGStatRow();
            this.backLabelStats = new SkinLabel();
            this.gpgPanelEdit = new SplitContainer();
            this.skinButtonEdit = new SkinButton();
            this.skinButtonCancelEdit = new SkinButton();
            this.skinButtonSaveEdit = new SkinButton();
            this.gpgTextAreaDescription = new GPGTextArea();
            this.splitContainerFriends = new SplitContainer();
            this.backLabelFriends = new SkinLabel();
            this.gpgScrollPanelFriends = new GPGScrollPanel();
            this.skinButtonClose = new SkinButton();
            this.skinButtonNext = new SkinButton();
            this.skinButtonLast = new SkinButton();
            this.gpgContextMenuChat = new GPGContextMenu();
            this.ciChat_WhisperPlayer = new MenuItem();
            this.ciChat_WebStats = new MenuItem();
            this.ciChat_ViewPlayer = new MenuItem();
            this.ciChat_IgnorePlayer = new MenuItem();
            this.ciChat_UnignorePlayer = new MenuItem();
            this.menuItem10 = new MenuItem();
            this.ciChat_InviteFriend = new MenuItem();
            this.ciChat_RemoveFriend = new MenuItem();
            this.menuItem8 = new MenuItem();
            this.ciChat_InviteToClan = new MenuItem();
            this.ciChat_RequestClanInvite = new MenuItem();
            this.ciChat_ViewClan = new MenuItem();
            this.gvFriend_Member.BeginInit();
            this.repositoryItemPictureEdit4.BeginInit();
            this.gpgGridFriends.BeginInit();
            this.gvFriend_Container.BeginInit();
            this.repositoryItemTextEdit2.BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.gpgLabelName.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxClanIcon).BeginInit();
            ((ISupportInitialize) this.pictureBoxClanRank).BeginInit();
            ((ISupportInitialize) this.pictureBoxPlayerIcon).BeginInit();
            this.gpgPanelStats.SuspendLayout();
            this.gpgPanel2.SuspendLayout();
            this.gpgPanelEdit.Panel1.SuspendLayout();
            this.gpgPanelEdit.Panel2.SuspendLayout();
            this.gpgPanelEdit.SuspendLayout();
            this.gpgTextAreaDescription.Properties.BeginInit();
            this.splitContainerFriends.Panel1.SuspendLayout();
            this.splitContainerFriends.Panel2.SuspendLayout();
            this.splitContainerFriends.SuspendLayout();
            this.gpgScrollPanelFriends.SuspendLayout();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gvFriend_Member.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvFriend_Member.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvFriend_Member.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvFriend_Member.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvFriend_Member.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvFriend_Member.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvFriend_Member.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvFriend_Member.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvFriend_Member.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvFriend_Member.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.Empty.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.Empty.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Member.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvFriend_Member.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Member.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvFriend_Member.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Member.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvFriend_Member.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Member.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Member.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvFriend_Member.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvFriend_Member.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.FocusedCell.Options.UseFont = true;
            this.gvFriend_Member.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.FocusedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvFriend_Member.Appearance.FocusedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvFriend_Member.Appearance.FocusedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvFriend_Member.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.FocusedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Member.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.FocusedRow.Options.UseFont = true;
            this.gvFriend_Member.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvFriend_Member.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvFriend_Member.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvFriend_Member.Appearance.FooterPanel.Options.UseFont = true;
            this.gvFriend_Member.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.GroupButton.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvFriend_Member.Appearance.GroupButton.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvFriend_Member.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvFriend_Member.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvFriend_Member.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvFriend_Member.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.GroupPanel.BackColor2 = Color.White;
            this.gvFriend_Member.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvFriend_Member.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.GroupPanel.Options.UseFont = true;
            this.gvFriend_Member.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvFriend_Member.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvFriend_Member.Appearance.GroupRow.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.GroupRow.Options.UseFont = true;
            this.gvFriend_Member.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvFriend_Member.Appearance.HeaderPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvFriend_Member.Appearance.HeaderPanel.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvFriend_Member.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvFriend_Member.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.HideSelectionRow.BackColor = Color.Gray;
            this.gvFriend_Member.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvFriend_Member.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Member.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvFriend_Member.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.HorzLine.BackColor = Color.Yellow;
            this.gvFriend_Member.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.Preview.BackColor = Color.White;
            this.gvFriend_Member.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvFriend_Member.Appearance.Preview.ForeColor = Color.Purple;
            this.gvFriend_Member.Appearance.Preview.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.Preview.Options.UseFont = true;
            this.gvFriend_Member.Appearance.Preview.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.Row.BackColor = Color.Black;
            this.gvFriend_Member.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvFriend_Member.Appearance.Row.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.Row.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.Row.Options.UseFont = true;
            this.gvFriend_Member.Appearance.Row.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.RowSeparator.BackColor = Color.White;
            this.gvFriend_Member.Appearance.RowSeparator.BackColor2 = Color.White;
            this.gvFriend_Member.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.SelectedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvFriend_Member.Appearance.SelectedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvFriend_Member.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvFriend_Member.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.SelectedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Member.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvFriend_Member.Appearance.SelectedRow.Options.UseFont = true;
            this.gvFriend_Member.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvFriend_Member.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvFriend_Member.Appearance.TopNewRow.Options.UseFont = true;
            this.gvFriend_Member.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvFriend_Member.Appearance.VertLine.BackColor = Color.Yellow;
            this.gvFriend_Member.Appearance.VertLine.Options.UseBackColor = true;
            this.gvFriend_Member.BorderStyle = BorderStyles.NoBorder;
            this.gvFriend_Member.Columns.AddRange(new GridColumn[] { this.gcFriend_Icon, this.gcFriend_Text });
            this.gvFriend_Member.FocusRectStyle = DrawFocusRectStyle.None;
            this.gvFriend_Member.GridControl = this.gpgGridFriends;
            this.gvFriend_Member.HorzScrollVisibility = ScrollVisibility.Never;
            this.gvFriend_Member.LevelIndent = 0;
            this.gvFriend_Member.Name = "gvFriend_Member";
            this.gvFriend_Member.OptionsBehavior.SmartVertScrollBar = false;
            this.gvFriend_Member.OptionsLayout.Columns.AddNewColumns = false;
            this.gvFriend_Member.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gvFriend_Member.OptionsLayout.Columns.StoreLayout = false;
            this.gvFriend_Member.OptionsLayout.StoreDataSettings = false;
            this.gvFriend_Member.OptionsLayout.StoreVisualOptions = false;
            this.gvFriend_Member.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvFriend_Member.OptionsView.ShowColumnHeaders = false;
            this.gvFriend_Member.OptionsView.ShowDetailButtons = false;
            this.gvFriend_Member.OptionsView.ShowGroupPanel = false;
            this.gvFriend_Member.OptionsView.ShowHorzLines = false;
            this.gvFriend_Member.OptionsView.ShowIndicator = false;
            this.gvFriend_Member.OptionsView.ShowPreviewLines = false;
            this.gvFriend_Member.OptionsView.ShowVertLines = false;
            this.gvFriend_Member.ScrollStyle = ScrollStyleFlags.None;
            this.gvFriend_Member.VertScrollVisibility = ScrollVisibility.Never;
            this.gvFriend_Member.RowCountChanged += new EventHandler(this.gvFriend_Member_RowCountChanged);
            this.gcFriend_Icon.Caption = "gcIcon";
            this.gcFriend_Icon.ColumnEdit = this.repositoryItemPictureEdit4;
            this.gcFriend_Icon.FieldName = "Icon";
            this.gcFriend_Icon.Name = "gcFriend_Icon";
            this.gcFriend_Icon.OptionsColumn.AllowEdit = false;
            this.gcFriend_Icon.OptionsColumn.AllowSize = false;
            this.gcFriend_Icon.OptionsColumn.FixedWidth = true;
            this.gcFriend_Icon.Visible = true;
            this.gcFriend_Icon.VisibleIndex = 0;
            this.gcFriend_Icon.Width = 40;
            this.repositoryItemPictureEdit4.Name = "repositoryItemPictureEdit4";
            this.repositoryItemPictureEdit4.SizeMode = PictureSizeMode.Stretch;
            this.gcFriend_Text.Caption = "gcText";
            this.gcFriend_Text.FieldName = "Text";
            this.gcFriend_Text.Name = "gcFriend_Text";
            this.gcFriend_Text.OptionsColumn.AllowEdit = false;
            this.gcFriend_Text.Visible = true;
            this.gcFriend_Text.VisibleIndex = 1;
            this.gpgGridFriends.CustomizeStyle = false;
            this.gpgGridFriends.Dock = DockStyle.Top;
            this.gpgGridFriends.EmbeddedNavigator.Name = "";
            this.gpgGridFriends.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgGridFriends.IgnoreMouseWheel = true;
            node.LevelTemplate = this.gvFriend_Member;
            node.RelationName = "Members";
            this.gpgGridFriends.LevelTree.Nodes.AddRange(new GridLevelNode[] { node });
            this.gpgGridFriends.Location = new Point(0, 0);
            this.gpgGridFriends.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgGridFriends.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgGridFriends.MainView = this.gvFriend_Container;
            this.gpgGridFriends.Name = "gpgGridFriends";
            this.gpgGridFriends.RepositoryItems.AddRange(new RepositoryItem[] { this.repositoryItemPictureEdit4, this.repositoryItemTextEdit2 });
            this.gpgGridFriends.ShowOnlyPredefinedDetails = true;
            this.gpgGridFriends.Size = new Size(0xbc, 0x105);
            this.gpgGridFriends.TabIndex = 5;
            this.gpgGridFriends.ViewCollection.AddRange(new BaseView[] { this.gvFriend_Container, this.gvFriend_Member });
            this.gvFriend_Container.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvFriend_Container.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvFriend_Container.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvFriend_Container.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvFriend_Container.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvFriend_Container.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvFriend_Container.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvFriend_Container.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvFriend_Container.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvFriend_Container.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.Empty.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.Empty.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Container.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvFriend_Container.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Container.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvFriend_Container.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Container.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvFriend_Container.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Container.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Container.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvFriend_Container.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvFriend_Container.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.FocusedCell.Options.UseFont = true;
            this.gvFriend_Container.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.FocusedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvFriend_Container.Appearance.FocusedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvFriend_Container.Appearance.FocusedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvFriend_Container.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.FocusedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Container.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.FocusedRow.Options.UseFont = true;
            this.gvFriend_Container.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvFriend_Container.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvFriend_Container.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvFriend_Container.Appearance.FooterPanel.Options.UseFont = true;
            this.gvFriend_Container.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.GroupButton.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvFriend_Container.Appearance.GroupButton.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvFriend_Container.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvFriend_Container.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvFriend_Container.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvFriend_Container.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.GroupPanel.BackColor2 = Color.White;
            this.gvFriend_Container.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvFriend_Container.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.GroupPanel.Options.UseFont = true;
            this.gvFriend_Container.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvFriend_Container.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvFriend_Container.Appearance.GroupRow.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.GroupRow.Options.UseFont = true;
            this.gvFriend_Container.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvFriend_Container.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvFriend_Container.Appearance.HeaderPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvFriend_Container.Appearance.HeaderPanel.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvFriend_Container.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvFriend_Container.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.HideSelectionRow.BackColor = Color.Gray;
            this.gvFriend_Container.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvFriend_Container.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvFriend_Container.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvFriend_Container.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.HorzLine.BackColor = Color.Yellow;
            this.gvFriend_Container.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.Preview.BackColor = Color.White;
            this.gvFriend_Container.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvFriend_Container.Appearance.Preview.ForeColor = Color.Purple;
            this.gvFriend_Container.Appearance.Preview.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.Preview.Options.UseFont = true;
            this.gvFriend_Container.Appearance.Preview.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.Row.BackColor = Color.FromArgb(0x10, 0x21, 80);
            this.gvFriend_Container.Appearance.Row.BackColor2 = Color.FromArgb(6, 0x10, 0x29);
            this.gvFriend_Container.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvFriend_Container.Appearance.Row.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.Row.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Container.Appearance.Row.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.Row.Options.UseFont = true;
            this.gvFriend_Container.Appearance.Row.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.RowSeparator.BackColor = Color.White;
            this.gvFriend_Container.Appearance.RowSeparator.BackColor2 = Color.White;
            this.gvFriend_Container.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.SelectedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvFriend_Container.Appearance.SelectedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvFriend_Container.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvFriend_Container.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.SelectedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvFriend_Container.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvFriend_Container.Appearance.SelectedRow.Options.UseFont = true;
            this.gvFriend_Container.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvFriend_Container.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvFriend_Container.Appearance.TopNewRow.Options.UseFont = true;
            this.gvFriend_Container.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvFriend_Container.Appearance.VertLine.BackColor = Color.Yellow;
            this.gvFriend_Container.Appearance.VertLine.Options.UseBackColor = true;
            this.gvFriend_Container.BorderStyle = BorderStyles.NoBorder;
            this.gvFriend_Container.Columns.AddRange(new GridColumn[] { this.gcFriend_Title, this.gcFriend_Count, this.gcFriend_Title, this.gcFriend_Count });
            this.gvFriend_Container.FocusRectStyle = DrawFocusRectStyle.None;
            this.gvFriend_Container.GridControl = this.gpgGridFriends;
            this.gvFriend_Container.HorzScrollVisibility = ScrollVisibility.Never;
            this.gvFriend_Container.LevelIndent = 0;
            this.gvFriend_Container.Name = "gvFriend_Container";
            this.gvFriend_Container.OptionsBehavior.SmartVertScrollBar = false;
            this.gvFriend_Container.OptionsCustomization.AllowGroup = false;
            this.gvFriend_Container.OptionsCustomization.AllowRowSizing = true;
            this.gvFriend_Container.OptionsDetail.ShowDetailTabs = false;
            this.gvFriend_Container.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvFriend_Container.OptionsView.ShowChildrenInGroupPanel = true;
            this.gvFriend_Container.OptionsView.ShowColumnHeaders = false;
            this.gvFriend_Container.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
            this.gvFriend_Container.OptionsView.ShowGroupPanel = false;
            this.gvFriend_Container.OptionsView.ShowHorzLines = false;
            this.gvFriend_Container.OptionsView.ShowIndicator = false;
            this.gvFriend_Container.OptionsView.ShowVertLines = false;
            this.gvFriend_Container.ScrollStyle = ScrollStyleFlags.None;
            this.gvFriend_Container.VertScrollVisibility = ScrollVisibility.Never;
            this.gvFriend_Container.RowCountChanged += new EventHandler(this.gvFriend_Container_RowCountChanged);
            this.gcFriend_Title.Caption = "gcTitle";
            this.gcFriend_Title.FieldName = "Title";
            this.gcFriend_Title.Name = "gcFriend_Title";
            this.gcFriend_Title.OptionsColumn.AllowEdit = false;
            this.gcFriend_Title.Visible = true;
            this.gcFriend_Title.VisibleIndex = 1;
            this.gcFriend_Count.Caption = "gcCount";
            this.gcFriend_Count.FieldName = "Count";
            this.gcFriend_Count.Name = "gcFriend_Count";
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            this.splitContainer1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainer1.FixedPanel = FixedPanel.Panel2;
            this.splitContainer1.Location = new Point(12, 0x53);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel1, null);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainerFriends);
            base.ttDefault.SetSuperTip(this.splitContainer1.Panel2, null);
            this.splitContainer1.Size = new Size(0x22f, 0x11f);
            this.splitContainer1.SplitterDistance = 0x16f;
            base.ttDefault.SetSuperTip(this.splitContainer1, null);
            this.splitContainer1.TabIndex = 4;
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.FixedPanel = FixedPanel.Panel1;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Margin = new Padding(0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = Orientation.Horizontal;
            this.splitContainer2.Panel1.Controls.Add(this.gpgLinkLabelClan);
            this.splitContainer2.Panel1.Controls.Add(this.gpgLabelName);
            base.ttDefault.SetSuperTip(this.splitContainer2.Panel1, null);
            this.splitContainer2.Panel2.AutoScroll = true;
            this.splitContainer2.Panel2.Controls.Add(this.gpgPanelStats);
            this.splitContainer2.Panel2.Controls.Add(this.gpgPanelEdit);
            base.ttDefault.SetSuperTip(this.splitContainer2.Panel2, null);
            this.splitContainer2.Size = new Size(0x16f, 0x11f);
            this.splitContainer2.SplitterDistance = 0x19;
            this.splitContainer2.SplitterWidth = 1;
            base.ttDefault.SetSuperTip(this.splitContainer2, null);
            this.splitContainer2.TabIndex = 4;
            this.gpgLinkLabelClan.AutoStyle = false;
            this.gpgLinkLabelClan.BackColor = Color.Black;
            this.gpgLinkLabelClan.Cursor = Cursors.Hand;
            this.gpgLinkLabelClan.DrawEdges = false;
            this.gpgLinkLabelClan.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.gpgLinkLabelClan.ForeColor = Color.Red;
            this.gpgLinkLabelClan.HorizontalScalingMode = ScalingModes.Tile;
            this.gpgLinkLabelClan.IsStyled = false;
            this.gpgLinkLabelClan.Location = new Point(0x33, 0);
            this.gpgLinkLabelClan.Margin = new Padding(0);
            this.gpgLinkLabelClan.Name = "gpgLinkLabelClan";
            this.gpgLinkLabelClan.Size = new Size(0x2f, 20);
            this.gpgLinkLabelClan.SkinBasePath = @"Controls\Background Label\Rectangle";
            base.ttDefault.SetSuperTip(this.gpgLinkLabelClan, null);
            this.gpgLinkLabelClan.TabIndex = 10;
            this.gpgLinkLabelClan.Text = "WWW";
            this.gpgLinkLabelClan.TextAlign = ContentAlignment.TopCenter;
            this.gpgLinkLabelClan.TextPadding = new Padding(0);
            this.gpgLinkLabelClan.Click += new EventHandler(this.gpgLinkLabelClan_Click);
            this.gpgLabelName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelName.AutoStyle = false;
            this.gpgLabelName.BackColor = Color.Black;
            this.gpgLabelName.Controls.Add(this.pictureBoxClanIcon);
            this.gpgLabelName.Controls.Add(this.pictureBoxClanRank);
            this.gpgLabelName.Controls.Add(this.pictureBoxPlayerIcon);
            this.gpgLabelName.DrawEdges = true;
            this.gpgLabelName.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.gpgLabelName.ForeColor = Color.White;
            this.gpgLabelName.HorizontalScalingMode = ScalingModes.Tile;
            this.gpgLabelName.IsStyled = false;
            this.gpgLabelName.Location = new Point(0, 0);
            this.gpgLabelName.Name = "gpgLabelName";
            this.gpgLabelName.Size = new Size(350, 20);
            this.gpgLabelName.SkinBasePath = @"Controls\Background Label\Rectangle";
            base.ttDefault.SetSuperTip(this.gpgLabelName, null);
            this.gpgLabelName.TabIndex = 5;
            this.gpgLabelName.Text = "Test";
            this.gpgLabelName.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabelName.TextPadding = new Padding(0);
            this.gpgLabelName.DoubleClick += new EventHandler(this.gpgLabelName_DoubleClick);
            this.gpgLabelName.MouseUp += new MouseEventHandler(this.gpgLabelName_MouseUp);
            this.pictureBoxClanIcon.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pictureBoxClanIcon.Location = new Point(0x108, 0);
            this.pictureBoxClanIcon.Name = "pictureBoxClanIcon";
            this.pictureBoxClanIcon.Size = new Size(40, 20);
            this.pictureBoxClanIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pictureBoxClanIcon, null);
            this.pictureBoxClanIcon.TabIndex = 12;
            this.pictureBoxClanIcon.TabStop = false;
            this.pictureBoxClanRank.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pictureBoxClanRank.Location = new Point(310, 0);
            this.pictureBoxClanRank.Name = "pictureBoxClanRank";
            this.pictureBoxClanRank.Size = new Size(40, 20);
            this.pictureBoxClanRank.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pictureBoxClanRank, null);
            this.pictureBoxClanRank.TabIndex = 11;
            this.pictureBoxClanRank.TabStop = false;
            this.pictureBoxPlayerIcon.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pictureBoxPlayerIcon.Location = new Point(0xd9, 0);
            this.pictureBoxPlayerIcon.Name = "pictureBoxPlayerIcon";
            this.pictureBoxPlayerIcon.Size = new Size(40, 20);
            this.pictureBoxPlayerIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pictureBoxPlayerIcon, null);
            this.pictureBoxPlayerIcon.TabIndex = 9;
            this.pictureBoxPlayerIcon.TabStop = false;
            this.gpgPanelStats.AutoScroll = true;
            this.gpgPanelStats.Controls.Add(this.gpgPanel2);
            this.gpgPanelStats.Dock = DockStyle.Fill;
            this.gpgPanelStats.Location = new Point(0, 150);
            this.gpgPanelStats.Name = "gpgPanelStats";
            this.gpgPanelStats.Size = new Size(0x16f, 0x6f);
            base.ttDefault.SetSuperTip(this.gpgPanelStats, null);
            this.gpgPanelStats.TabIndex = 6;
            this.gpgPanel2.AutoScroll = true;
            this.gpgPanel2.Controls.Add(this.gpgStatRow3);
            this.gpgPanel2.Controls.Add(this.gpgStatRow2);
            this.gpgPanel2.Controls.Add(this.gpgLabelDescription);
            this.gpgPanel2.Controls.Add(this.gpgStatRow1);
            this.gpgPanel2.Controls.Add(this.backLabelStats);
            this.gpgPanel2.Dock = DockStyle.Fill;
            this.gpgPanel2.Location = new Point(0, 0);
            this.gpgPanel2.Name = "gpgPanel2";
            this.gpgPanel2.Size = new Size(0x16f, 0x6f);
            base.ttDefault.SetSuperTip(this.gpgPanel2, null);
            this.gpgPanel2.TabIndex = 10;
            this.gpgStatRow3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgStatRow3.DataBackColor = Color.Black;
            this.gpgStatRow3.DataForeColor = Color.Yellow;
            this.gpgStatRow3.DesignerItemCount = 3;
            this.gpgStatRow3.HeaderBackColor = Color.Gray;
            this.gpgStatRow3.HeaderForeColor = Color.White;
            this.gpgStatRow3.Location = new Point(0, 0x9a);
            this.gpgStatRow3.Margin = new Padding(0);
            this.gpgStatRow3.Name = "gpgStatRow3";
            this.gpgStatRow3.Size = new Size(0x12b, 0x43);
            base.ttDefault.SetSuperTip(this.gpgStatRow3, null);
            this.gpgStatRow3.TabIndex = 10;
            this.gpgStatRow3.Text = "gpgStatRow3";
            this.gpgStatRow2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgStatRow2.DataBackColor = Color.Black;
            this.gpgStatRow2.DataForeColor = Color.Yellow;
            this.gpgStatRow2.DesignerItemCount = 3;
            this.gpgStatRow2.HeaderBackColor = Color.Gray;
            this.gpgStatRow2.HeaderForeColor = Color.White;
            this.gpgStatRow2.Location = new Point(0, 0x68);
            this.gpgStatRow2.Margin = new Padding(0);
            this.gpgStatRow2.Name = "gpgStatRow2";
            this.gpgStatRow2.Size = new Size(0x12b, 50);
            base.ttDefault.SetSuperTip(this.gpgStatRow2, null);
            this.gpgStatRow2.TabIndex = 9;
            this.gpgStatRow2.Text = "gpgStatRow2";
            this.gpgLabelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(-3, 0);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x12e, 0x18);
            base.ttDefault.SetSuperTip(this.gpgLabelDescription, null);
            this.gpgLabelDescription.TabIndex = 5;
            this.gpgLabelDescription.Text = "(no description)";
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            this.gpgLabelDescription.VisibleChanged += new EventHandler(this.gpgLabelDescription_VisibleChanged);
            this.gpgLabelDescription.TextChanged += new EventHandler(this.gpgLabelDescription_TextChanged);
            this.gpgStatRow1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgStatRow1.DataBackColor = Color.Black;
            this.gpgStatRow1.DataForeColor = Color.Yellow;
            this.gpgStatRow1.DesignerItemCount = 3;
            this.gpgStatRow1.HeaderBackColor = Color.Gray;
            this.gpgStatRow1.HeaderForeColor = Color.White;
            this.gpgStatRow1.Location = new Point(0, 0x2c);
            this.gpgStatRow1.Margin = new Padding(0);
            this.gpgStatRow1.Name = "gpgStatRow1";
            this.gpgStatRow1.Size = new Size(0x12b, 60);
            base.ttDefault.SetSuperTip(this.gpgStatRow1, null);
            this.gpgStatRow1.TabIndex = 7;
            this.gpgStatRow1.Text = "gpgStatRow1";
            this.backLabelStats.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.backLabelStats.AutoStyle = false;
            this.backLabelStats.BackColor = Color.Transparent;
            this.backLabelStats.DrawEdges = true;
            this.backLabelStats.Font = new Font("Arial", 12f, FontStyle.Bold);
            this.backLabelStats.ForeColor = Color.White;
            this.backLabelStats.HorizontalScalingMode = ScalingModes.Tile;
            this.backLabelStats.IsStyled = false;
            this.backLabelStats.Location = new Point(0, 0x18);
            this.backLabelStats.Margin = new Padding(0);
            this.backLabelStats.Name = "backLabelStats";
            this.backLabelStats.Size = new Size(0x12b, 20);
            this.backLabelStats.SkinBasePath = @"Controls\Background Label\Rectangle";
            base.ttDefault.SetSuperTip(this.backLabelStats, null);
            this.backLabelStats.TabIndex = 6;
            this.backLabelStats.Text = "<LOC>Statistics";
            this.backLabelStats.TextAlign = ContentAlignment.MiddleLeft;
            this.backLabelStats.TextPadding = new Padding(0);
            this.gpgPanelEdit.Dock = DockStyle.Top;
            this.gpgPanelEdit.FixedPanel = FixedPanel.Panel1;
            this.gpgPanelEdit.IsSplitterFixed = true;
            this.gpgPanelEdit.Location = new Point(0, 0);
            this.gpgPanelEdit.Name = "gpgPanelEdit";
            this.gpgPanelEdit.Orientation = Orientation.Horizontal;
            this.gpgPanelEdit.Panel1.Controls.Add(this.skinButtonEdit);
            this.gpgPanelEdit.Panel1.Controls.Add(this.skinButtonCancelEdit);
            this.gpgPanelEdit.Panel1.Controls.Add(this.skinButtonSaveEdit);
            base.ttDefault.SetSuperTip(this.gpgPanelEdit.Panel1, null);
            this.gpgPanelEdit.Panel2.Controls.Add(this.gpgTextAreaDescription);
            base.ttDefault.SetSuperTip(this.gpgPanelEdit.Panel2, null);
            this.gpgPanelEdit.Size = new Size(0x16f, 150);
            this.gpgPanelEdit.SplitterDistance = 0x19;
            base.ttDefault.SetSuperTip(this.gpgPanelEdit, null);
            this.gpgPanelEdit.TabIndex = 15;
            this.skinButtonEdit.AutoStyle = true;
            this.skinButtonEdit.BackColor = Color.Black;
            this.skinButtonEdit.DialogResult = DialogResult.OK;
            this.skinButtonEdit.DisabledForecolor = Color.Gray;
            this.skinButtonEdit.DrawEdges = true;
            this.skinButtonEdit.FocusColor = Color.Yellow;
            this.skinButtonEdit.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonEdit.ForeColor = Color.White;
            this.skinButtonEdit.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonEdit.IsStyled = true;
            this.skinButtonEdit.Location = new Point(3, 3);
            this.skinButtonEdit.Name = "skinButtonEdit";
            this.skinButtonEdit.Size = new Size(0x51, 20);
            this.skinButtonEdit.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonEdit, null);
            this.skinButtonEdit.TabIndex = 10;
            this.skinButtonEdit.Text = "<LOC>Edit";
            this.skinButtonEdit.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonEdit.TextPadding = new Padding(0);
            this.skinButtonEdit.Click += new EventHandler(this.skinButtonEdit_Click);
            this.skinButtonCancelEdit.AutoStyle = true;
            this.skinButtonCancelEdit.BackColor = Color.Black;
            this.skinButtonCancelEdit.DialogResult = DialogResult.OK;
            this.skinButtonCancelEdit.DisabledForecolor = Color.Gray;
            this.skinButtonCancelEdit.DrawEdges = true;
            this.skinButtonCancelEdit.FocusColor = Color.Yellow;
            this.skinButtonCancelEdit.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancelEdit.ForeColor = Color.White;
            this.skinButtonCancelEdit.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancelEdit.IsStyled = true;
            this.skinButtonCancelEdit.Location = new Point(90, 3);
            this.skinButtonCancelEdit.Name = "skinButtonCancelEdit";
            this.skinButtonCancelEdit.Size = new Size(0x51, 20);
            this.skinButtonCancelEdit.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancelEdit, null);
            this.skinButtonCancelEdit.TabIndex = 14;
            this.skinButtonCancelEdit.Text = "<LOC>Cancel";
            this.skinButtonCancelEdit.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancelEdit.TextPadding = new Padding(0);
            this.skinButtonCancelEdit.Visible = false;
            this.skinButtonCancelEdit.Click += new EventHandler(this.skinButtonCancelEdit_Click);
            this.skinButtonSaveEdit.AutoStyle = true;
            this.skinButtonSaveEdit.BackColor = Color.Black;
            this.skinButtonSaveEdit.DialogResult = DialogResult.OK;
            this.skinButtonSaveEdit.DisabledForecolor = Color.Gray;
            this.skinButtonSaveEdit.DrawEdges = true;
            this.skinButtonSaveEdit.FocusColor = Color.Yellow;
            this.skinButtonSaveEdit.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSaveEdit.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSaveEdit.IsStyled = true;
            this.skinButtonSaveEdit.Location = new Point(3, 3);
            this.skinButtonSaveEdit.Name = "skinButtonSaveEdit";
            this.skinButtonSaveEdit.Size = new Size(0x51, 20);
            this.skinButtonSaveEdit.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSaveEdit, null);
            this.skinButtonSaveEdit.TabIndex = 13;
            this.skinButtonSaveEdit.Text = "<LOC>Save";
            this.skinButtonSaveEdit.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSaveEdit.TextPadding = new Padding(0);
            this.skinButtonSaveEdit.Visible = false;
            this.skinButtonSaveEdit.Click += new EventHandler(this.skinButtonSaveEdit_Click);
            this.gpgTextAreaDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextAreaDescription.Location = new Point(0, 0);
            this.gpgTextAreaDescription.Margin = new Padding(0);
            this.gpgTextAreaDescription.Name = "gpgTextAreaDescription";
            this.gpgTextAreaDescription.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaDescription.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaDescription.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaDescription.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaDescription.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaDescription.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaDescription.Size = new Size(350, 0x79);
            this.gpgTextAreaDescription.TabIndex = 3;
            this.gpgTextAreaDescription.Visible = false;
            this.gpgTextAreaDescription.KeyDown += new KeyEventHandler(this.gpgTextAreaDescription_KeyDown);
            this.splitContainerFriends.Dock = DockStyle.Fill;
            this.splitContainerFriends.FixedPanel = FixedPanel.Panel1;
            this.splitContainerFriends.IsSplitterFixed = true;
            this.splitContainerFriends.Location = new Point(0, 0);
            this.splitContainerFriends.Margin = new Padding(3, 0, 0, 0);
            this.splitContainerFriends.Name = "splitContainerFriends";
            this.splitContainerFriends.Orientation = Orientation.Horizontal;
            this.splitContainerFriends.Panel1.Controls.Add(this.backLabelFriends);
            base.ttDefault.SetSuperTip(this.splitContainerFriends.Panel1, null);
            this.splitContainerFriends.Panel2.Controls.Add(this.gpgScrollPanelFriends);
            base.ttDefault.SetSuperTip(this.splitContainerFriends.Panel2, null);
            this.splitContainerFriends.Size = new Size(0xbc, 0x11f);
            this.splitContainerFriends.SplitterDistance = 0x19;
            this.splitContainerFriends.SplitterWidth = 1;
            base.ttDefault.SetSuperTip(this.splitContainerFriends, null);
            this.splitContainerFriends.TabIndex = 0;
            this.backLabelFriends.AutoStyle = false;
            this.backLabelFriends.BackColor = Color.Transparent;
            this.backLabelFriends.Dock = DockStyle.Top;
            this.backLabelFriends.DrawEdges = true;
            this.backLabelFriends.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.backLabelFriends.ForeColor = Color.White;
            this.backLabelFriends.HorizontalScalingMode = ScalingModes.Tile;
            this.backLabelFriends.IsStyled = false;
            this.backLabelFriends.Location = new Point(0, 0);
            this.backLabelFriends.Margin = new Padding(0);
            this.backLabelFriends.Name = "backLabelFriends";
            this.backLabelFriends.Size = new Size(0xbc, 20);
            this.backLabelFriends.SkinBasePath = @"Controls\Background Label\Round Top";
            base.ttDefault.SetSuperTip(this.backLabelFriends, null);
            this.backLabelFriends.TabIndex = 0;
            this.backLabelFriends.Text = "<LOC>Friends";
            this.backLabelFriends.TextAlign = ContentAlignment.BottomCenter;
            this.backLabelFriends.TextPadding = new Padding(0);
            this.gpgScrollPanelFriends.AutoScroll = true;
            this.gpgScrollPanelFriends.BackColor = Color.Black;
            this.gpgScrollPanelFriends.ChildControl = this.gpgGridFriends;
            this.gpgScrollPanelFriends.Controls.Add(this.gpgGridFriends);
            this.gpgScrollPanelFriends.Dock = DockStyle.Fill;
            this.gpgScrollPanelFriends.Location = new Point(0, 0);
            this.gpgScrollPanelFriends.Name = "gpgScrollPanelFriends";
            this.gpgScrollPanelFriends.Size = new Size(0xbc, 0x105);
            base.ttDefault.SetSuperTip(this.gpgScrollPanelFriends, null);
            this.gpgScrollPanelFriends.TabIndex = 6;
            this.skinButtonClose.Anchor = AnchorStyles.Bottom;
            this.skinButtonClose.AutoStyle = true;
            this.skinButtonClose.BackColor = Color.Black;
            this.skinButtonClose.DialogResult = DialogResult.OK;
            this.skinButtonClose.DisabledForecolor = Color.Gray;
            this.skinButtonClose.DrawEdges = true;
            this.skinButtonClose.FocusColor = Color.Yellow;
            this.skinButtonClose.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClose.ForeColor = Color.White;
            this.skinButtonClose.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClose.IsStyled = true;
            this.skinButtonClose.Location = new Point(240, 0x178);
            this.skinButtonClose.Name = "skinButtonClose";
            this.skinButtonClose.Size = new Size(0x68, 0x1a);
            this.skinButtonClose.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClose, null);
            this.skinButtonClose.TabIndex = 9;
            this.skinButtonClose.Text = "<LOC>Close";
            this.skinButtonClose.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClose.TextPadding = new Padding(0);
            this.skinButtonClose.Click += new EventHandler(this.skinButtonClose_Click);
            this.skinButtonNext.Anchor = AnchorStyles.Bottom;
            this.skinButtonNext.AutoStyle = true;
            this.skinButtonNext.BackColor = Color.Black;
            this.skinButtonNext.DialogResult = DialogResult.OK;
            this.skinButtonNext.DisabledForecolor = Color.Gray;
            this.skinButtonNext.DrawEdges = true;
            this.skinButtonNext.FocusColor = Color.Yellow;
            this.skinButtonNext.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNext.ForeColor = Color.White;
            this.skinButtonNext.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNext.IsStyled = true;
            this.skinButtonNext.Location = new Point(0x16a, 0x178);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(0x68, 0x1a);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonNext, null);
            this.skinButtonNext.TabIndex = 10;
            this.skinButtonNext.Text = "<LOC>Next";
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.skinButtonLast.Anchor = AnchorStyles.Bottom;
            this.skinButtonLast.AutoStyle = true;
            this.skinButtonLast.BackColor = Color.Black;
            this.skinButtonLast.DialogResult = DialogResult.OK;
            this.skinButtonLast.DisabledForecolor = Color.Gray;
            this.skinButtonLast.DrawEdges = true;
            this.skinButtonLast.FocusColor = Color.Yellow;
            this.skinButtonLast.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLast.ForeColor = Color.White;
            this.skinButtonLast.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLast.IsStyled = true;
            this.skinButtonLast.Location = new Point(0x74, 0x178);
            this.skinButtonLast.Name = "skinButtonLast";
            this.skinButtonLast.Size = new Size(0x68, 0x1a);
            this.skinButtonLast.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonLast, null);
            this.skinButtonLast.TabIndex = 11;
            this.skinButtonLast.Text = "<LOC>Last";
            this.skinButtonLast.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLast.TextPadding = new Padding(0);
            this.skinButtonLast.Click += new EventHandler(this.skinButtonLast_Click);
            this.gpgContextMenuChat.MenuItems.AddRange(new MenuItem[] { this.ciChat_WhisperPlayer, this.ciChat_WebStats, this.ciChat_ViewPlayer, this.ciChat_IgnorePlayer, this.ciChat_UnignorePlayer, this.menuItem10, this.ciChat_InviteFriend, this.ciChat_RemoveFriend, this.menuItem8, this.ciChat_InviteToClan, this.ciChat_RequestClanInvite, this.ciChat_ViewClan });
            this.ciChat_WhisperPlayer.Index = 0;
            this.ciChat_WhisperPlayer.Text = "<LOC>Send private message";
            this.ciChat_WhisperPlayer.Click += new EventHandler(this.ciChat_WhisperPlayer_Click);
            this.ciChat_WebStats.Index = 1;
            this.ciChat_WebStats.Text = "<LOC>View web stats";
            this.ciChat_WebStats.Click += new EventHandler(this.ciChat_WebStats_Click);
            this.ciChat_ViewPlayer.Index = 2;
            this.ciChat_ViewPlayer.Text = "<LOC>View this player's profile";
            this.ciChat_ViewPlayer.Click += new EventHandler(this.ciChat_ViewPlayer_Click);
            this.ciChat_IgnorePlayer.Index = 3;
            this.ciChat_IgnorePlayer.Text = "<LOC>Ignore Player";
            this.ciChat_IgnorePlayer.Click += new EventHandler(this.ciChat_IgnorePlayer_Click);
            this.ciChat_UnignorePlayer.Index = 4;
            this.ciChat_UnignorePlayer.Text = "<LOC>Unignore Player";
            this.ciChat_UnignorePlayer.Click += new EventHandler(this.ciChat_UnignorePlayer_Click);
            this.menuItem10.Index = 5;
            this.menuItem10.Text = "-";
            this.ciChat_InviteFriend.Index = 6;
            this.ciChat_InviteFriend.Text = "<LOC>Invite player to join Friends list";
            this.ciChat_InviteFriend.Click += new EventHandler(this.ciChat_InviteFriend_Click);
            this.ciChat_RemoveFriend.Index = 7;
            this.ciChat_RemoveFriend.Text = "<LOC>Remove player from Friends list";
            this.ciChat_RemoveFriend.Click += new EventHandler(this.ciChat_RemoveFriend_Click);
            this.menuItem8.Index = 8;
            this.menuItem8.Text = "-";
            this.ciChat_InviteToClan.Index = 9;
            this.ciChat_InviteToClan.Text = "<LOC>Invite this player to join clan";
            this.ciChat_InviteToClan.Click += new EventHandler(this.ciChat_InviteToClan_Click);
            this.ciChat_RequestClanInvite.Index = 10;
            this.ciChat_RequestClanInvite.Text = "<LOC>Request to join this player's clan";
            this.ciChat_RequestClanInvite.Click += new EventHandler(this.ciChat_RequestClanInvite_Click);
            this.ciChat_ViewClan.Index = 11;
            this.ciChat_ViewClan.Text = "<LOC>View this clan's profile";
            this.ciChat_ViewClan.Click += new EventHandler(this.ciChat_ViewClan_Click);
            base.AcceptButton = this.skinButtonClose;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonClose;
            base.ClientSize = new Size(0x247, 0x1bf);
            base.Controls.Add(this.skinButtonLast);
            base.Controls.Add(this.skinButtonNext);
            base.Controls.Add(this.skinButtonClose);
            base.Controls.Add(this.splitContainer1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x178, 0x178);
            base.Name = "DlgPlayerProfile";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "DlgPlayerProfile";
            base.Controls.SetChildIndex(this.splitContainer1, 0);
            base.Controls.SetChildIndex(this.skinButtonClose, 0);
            base.Controls.SetChildIndex(this.skinButtonNext, 0);
            base.Controls.SetChildIndex(this.skinButtonLast, 0);
            this.gvFriend_Member.EndInit();
            this.repositoryItemPictureEdit4.EndInit();
            this.gpgGridFriends.EndInit();
            this.gvFriend_Container.EndInit();
            this.repositoryItemTextEdit2.EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.gpgLabelName.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxClanIcon).EndInit();
            ((ISupportInitialize) this.pictureBoxClanRank).EndInit();
            ((ISupportInitialize) this.pictureBoxPlayerIcon).EndInit();
            this.gpgPanelStats.ResumeLayout(false);
            this.gpgPanel2.ResumeLayout(false);
            this.gpgPanelEdit.Panel1.ResumeLayout(false);
            this.gpgPanelEdit.Panel2.ResumeLayout(false);
            this.gpgPanelEdit.ResumeLayout(false);
            this.gpgTextAreaDescription.Properties.EndInit();
            this.splitContainerFriends.Panel1.ResumeLayout(false);
            this.splitContainerFriends.Panel2.ResumeLayout(false);
            this.splitContainerFriends.ResumeLayout(false);
            this.gpgScrollPanelFriends.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        internal void LoadStats()
        {
            this.gpgStatRow1.Items.Clear();
            this.gpgStatRow2.Items.Clear();
            this.gpgStatRow3.Items.Clear();
            this.gpgStatRow1.AddItem("<LOC>Rank", this.CurrentStats.Rank);
            this.gpgStatRow1.AddItem("<LOC>Rating", this.CurrentStats.Rating);
            this.gpgStatRow1.AddItem("<LOC>Total Games", this.CurrentStats.TotalGames);
            this.gpgStatRow2.AddItem("<LOC>Wins", this.CurrentStats.Wins);
            this.gpgStatRow2.AddItem("<LOC>Losses", this.CurrentStats.Losses);
            this.gpgStatRow2.AddItem("<LOC>Draws", this.CurrentStats.Draws);
            this.gpgStatRow2.AddItem("<LOC>Win %", string.Format("{0}%", this.CurrentStats.WinPercentage));
            this.gpgStatRow3.AddItem("<LOC>Disconnects", this.CurrentStats.Disconnects);
            this.gpgStatRow3.AddItem("<LOC>Disconnect %", string.Format("{0}%", this.CurrentStats.DisconnectPercentage));
            this.gpgStatRow1.Localize();
            this.gpgStatRow2.Localize();
            this.gpgStatRow3.Localize();
            this.gpgStatRow1.Refresh();
            this.gpgStatRow2.Refresh();
            this.gpgStatRow3.Refresh();
        }

        protected override void Localize()
        {
            base.Localize();
            this.gpgContextMenuChat.Localize();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if ((this.DescriptionModified && (new DlgYesNo(base.MainForm, "<LOC>Save Changes", "<LOC>Do you want to save the changes to your profile?").ShowDialog(this) == DialogResult.Yes)) && !this.SaveEdits())
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if ((this.gpgLabelName != null) && (this.backLabelFriends != null))
            {
                this.gpgLabelName.Refresh();
                this.backLabelFriends.Refresh();
            }
        }

        private void ResizeDescriptionText()
        {
            using (Graphics graphics = base.CreateGraphics())
            {
                this.gpgLabelDescription.Height = ((int) DrawUtil.MeasureString(graphics, this.gpgLabelDescription.Text, this.gpgLabelDescription.Font, (float) this.gpgLabelDescription.Width).Height) + 10;
            }
            this.backLabelStats.Top = this.gpgLabelDescription.Bottom;
            this.gpgStatRow1.Top = this.backLabelStats.Bottom;
            this.gpgStatRow2.Top = this.gpgStatRow1.Bottom;
            this.gpgStatRow3.Top = this.gpgStatRow2.Bottom;
        }

        private bool SaveEdits()
        {
            base.ClearErrors();
            if (Profanity.ContainsProfanity(this.gpgTextAreaDescription.Text))
            {
                base.Error(this.gpgTextAreaDescription, "<LOC>Your profile may not contain profanity.\r\nPlease enter a valid profile description.", new object[0]);
                return false;
            }
            DataAccess.ExecuteQuery("CreatePlayerInfo", new object[0]);
            if (!DataAccess.ExecuteQuery("SetPlayerDescription", new object[] { this.gpgTextAreaDescription.Text }))
            {
                base.Error(this.gpgTextAreaDescription, "<LOC>Failed to save description", new object[0]);
                return false;
            }
            this.gpgLabelDescription.Text = this.gpgTextAreaDescription.Text;
            User.Current.Description = this.gpgTextAreaDescription.Text;
            this.CurrentPlayer.Player.Description = this.gpgTextAreaDescription.Text;
            this.gpgLabelDescription.Visible = true;
            this.gpgTextAreaDescription.Visible = false;
            this.skinButtonEdit.Visible = true;
            this.skinButtonCancelEdit.Visible = false;
            this.skinButtonSaveEdit.Visible = false;
            this.mDescriptionModified = false;
            this.gpgPanelEdit.Height -= this.gpgTextAreaDescription.Height;
            this.gpgPanelEdit.Panel2Collapsed = true;
            return true;
        }

        private void SetCurrentGridView(object sender, MouseEventArgs e)
        {
            if (sender is GridView)
            {
                this.mSelectedParticipantView = sender as GridView;
            }
        }

        internal bool SetTargetPlayer(string playerName)
        {
            User record = null;
            if (this.PlayerViewLookups.ContainsKey(playerName))
            {
                this.mCurrentPlayer = this.PlayerViewLookups[playerName];
                return true;
            }
            if (!Chatroom.GatheringParticipants.TryFindByIndex("name", playerName, out record) && ((User.CurrentFriends == null) || !User.CurrentFriends.TryFindByIndex("name", playerName, out record)))
            {
                if (playerName == User.Current.Name)
                {
                    record = User.Current;
                }
                else
                {
                    MappedObjectList<User> objects = DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { playerName });
                    if (objects.Count > 0)
                    {
                        record = objects[0];
                    }
                }
            }
            if (record != null)
            {
                MappedObjectList<User> friends = DataAccess.GetObjects<User>("GetFriendsByPlayerID", new object[] { record.ID });
                if (record.Description == null)
                {
                    record.Description = DataAccess.GetString("GetPlayerDescription", new object[] { record.ID });
                }
                this.mCurrentPlayer = new PlayerView(record, friends);
                this.PlayerViewLookups.Add(playerName, this.CurrentPlayer);
                return true;
            }
            base.MainForm.ErrorMessage("<LOC>Unable to locate {0}", new object[] { playerName });
            if (this.ViewList.Count < 1)
            {
                base.Dispose();
            }
            return false;
        }

        private void skinButtonCancelEdit_Click(object sender, EventArgs e)
        {
            this.CancelEdits();
        }

        private void skinButtonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void skinButtonEdit_Click(object sender, EventArgs e)
        {
            this.gpgTextAreaDescription.Text = this.gpgLabelDescription.Text;
            this.gpgLabelDescription.Visible = true;
            this.gpgTextAreaDescription.Visible = true;
            this.skinButtonEdit.Visible = false;
            this.skinButtonCancelEdit.Visible = true;
            this.skinButtonSaveEdit.Visible = true;
            this.gpgPanelEdit.Panel2Collapsed = false;
            this.gpgPanelEdit.Height += this.gpgTextAreaDescription.Height;
        }

        private void skinButtonLast_Click(object sender, EventArgs e)
        {
            this.mCurrentPlayer = this.ViewList.Find(this.CurrentPlayer).Previous.Value;
            this.Construct();
        }

        private void skinButtonNext_Click(object sender, EventArgs e)
        {
            this.mCurrentPlayer = this.ViewList.Find(this.CurrentPlayer).Next.Value;
            this.Construct();
        }

        private void skinButtonSaveEdit_Click(object sender, EventArgs e)
        {
            this.SaveEdits();
        }

        public override bool AllowRestoreWindow
        {
            get
            {
                return false;
            }
        }

        public PlayerView CurrentPlayer
        {
            get
            {
                return this.mCurrentPlayer;
            }
        }

        public PlayerRating CurrentStats
        {
            get
            {
                return this.mCurrentStats;
            }
        }

        public bool DescriptionModified
        {
            get
            {
                return this.mDescriptionModified;
            }
        }

        public IUser SelectedChatParticipant
        {
            get
            {
                try
                {
                    if (this.SelectedParticipantView != null)
                    {
                        int[] selectedRows = this.SelectedParticipantView.GetSelectedRows();
                        if (selectedRows.Length > 0)
                        {
                            TextLine row = this.SelectedParticipantView.GetRow(selectedRows[0]) as TextLine;
                            if ((row != null) && ((row.Tag != null) && (row.Tag is IUser)))
                            {
                                return (row.Tag as IUser);
                            }
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

        public LinkedList<PlayerView> ViewList
        {
            get
            {
                return this.mViewList;
            }
        }
    }
}

