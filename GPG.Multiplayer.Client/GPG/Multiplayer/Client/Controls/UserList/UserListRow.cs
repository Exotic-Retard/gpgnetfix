namespace GPG.Multiplayer.Client.Controls.UserList
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Statistics;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class UserListRow : IDisposable
    {
        private static Font AdminFont = new Font(Program.Settings.Chat.Appearance.DefaultFont, FontStyle.Bold);
        private Rectangle AvatarBounds;
        private static Dictionary<int, Image> AvatarCache = new Dictionary<int, Image>();
        private Image AvatarImage;
        private Rectangle Award1Bounds;
        private Image Award1Image;
        private Rectangle Award2Bounds;
        private Image Award2Image;
        private Rectangle Award3Bounds;
        private Image Award3Image;
        private RectangleF? ClanLabelBounds;
        private Rectangle ClanRankBounds;
        private Image ClanRankImage;
        private Rectangle ClientBounds;
        private static Font DefaultFont = Program.Settings.Chat.Appearance.DefaultFont;
        private UserListCategory LastCategory;
        private UserListCategories mCategory;
        private GPG.Multiplayer.Client.Clans.ClanMember mClanMember;
        private bool mIsSelected;
        private UserListRow mNextRow;
        private PnlUserList mParent;
        private UserListRow mPreviousRow;
        private UserListStyles mStyle;
        private GPG.Multiplayer.Quazal.User mUser;
        private Color NameColor;
        private Font NameFont;
        private PointF NameLabelLocation;
        private PlayerDisplayAwards PlayerAwards;
        private UserStatus PlayerStatus;
        private Rectangle StatusBounds;
        private Image StatusImage;

        public event MouseEventHandler MouseDown;

        public event MouseEventHandler MouseMove;

        public event MouseEventHandler MouseUp;

        public UserListRow(PnlUserList parent, GPG.Multiplayer.Client.Clans.ClanMember clanMember, UserListStyles style, UserListCategories initialCategory)
        {
            this.PlayerAwards = null;
            this.PlayerStatus = null;
            this.mCategory = UserListCategories.Online;
            this.mPreviousRow = null;
            this.mNextRow = null;
            this.mIsSelected = false;
            this.LastCategory = null;
            this.ClanRankImage = null;
            this.Award1Image = null;
            this.Award2Image = null;
            this.Award3Image = null;
            this.AvatarImage = null;
            this.StatusImage = null;
            this.ClanLabelBounds = null;
            this.mParent = parent;
            this.mClanMember = clanMember;
            this.mStyle = style;
            this.mCategory = initialCategory;
            this.Parent.MouseMove += new MouseEventHandler(this.Parent_MouseMove);
            this.Parent.MouseDown += new MouseEventHandler(this.Parent_MouseDown);
            this.Parent.MouseUp += new MouseEventHandler(this.Parent_MouseUp);
            this.BindToClanMember(clanMember);
        }

        public UserListRow(PnlUserList parent, GPG.Multiplayer.Quazal.User user, UserListStyles style, UserListCategories initialCategory)
        {
            this.PlayerAwards = null;
            this.PlayerStatus = null;
            this.mCategory = UserListCategories.Online;
            this.mPreviousRow = null;
            this.mNextRow = null;
            this.mIsSelected = false;
            this.LastCategory = null;
            this.ClanRankImage = null;
            this.Award1Image = null;
            this.Award2Image = null;
            this.Award3Image = null;
            this.AvatarImage = null;
            this.StatusImage = null;
            this.ClanLabelBounds = null;
            this.mParent = parent;
            this.mUser = user;
            this.mStyle = style;
            this.mCategory = initialCategory;
            this.Parent.MouseMove += new MouseEventHandler(this.Parent_MouseMove);
            this.Parent.MouseDown += new MouseEventHandler(this.Parent_MouseDown);
            this.Parent.MouseUp += new MouseEventHandler(this.Parent_MouseUp);
            this.BindToUser(this.User);
        }

        public void BindToClanMember(GPG.Multiplayer.Client.Clans.ClanMember member)
        {
            this.BindToClanMember(member, this.Category);
        }

        public void BindToClanMember(GPG.Multiplayer.Client.Clans.ClanMember member, UserListCategories initialCategory)
        {
            this.mClanMember = member;
            this.mCategory = initialCategory;
            try
            {
                if (!member.Online)
                {
                    this.NameColor = Program.Settings.Chat.Appearance.UnavailableColor;
                    this.NameFont = DefaultFont;
                }
                else
                {
                    this.NameColor = Program.Settings.Chat.Appearance.DefaultColor;
                    this.NameFont = DefaultFont;
                }
                this.ClanRankImage = member.GetRanking().Image;
                if (member.Online)
                {
                    this.PlayerStatus = UserStatus.None;
                }
                else
                {
                    this.PlayerStatus = UserStatus.Offline;
                }
                if (this.PlayerStatus != null)
                {
                    this.StatusImage = this.PlayerStatus.Icon;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void BindToUser(GPG.Multiplayer.Quazal.User user)
        {
            this.BindToUser(user, this.Category);
        }

        public void BindToUser(GPG.Multiplayer.Quazal.User user, UserListCategories initialCategory)
        {
            this.mUser = user;
            this.mCategory = initialCategory;
            this.Award1Image = null;
            this.Award2Image = null;
            this.Award3Image = null;
            try
            {
                this.NameColor = Program.Settings.Chat.Appearance.DefaultColor;
                this.NameFont = DefaultFont;
                this.PlayerAwards = new PlayerDisplayAwards(user);
                if (!AvatarCache.ContainsKey(this.PlayerAwards.Avatar.ID))
                {
                    AvatarCache[this.PlayerAwards.Avatar.ID] = DrawUtil.CopyImage(this.PlayerAwards.Avatar.Image);
                }
                this.AvatarImage = AvatarCache[this.PlayerAwards.Avatar.ID];
                if (this.PlayerAwards.Award1Specified)
                {
                    this.Award1Image = this.PlayerAwards.Award1.SmallImage;
                }
                if (this.PlayerAwards.Award2Specified)
                {
                    this.Award2Image = this.PlayerAwards.Award2.SmallImage;
                }
                if (this.PlayerAwards.Award3Specified)
                {
                    this.Award3Image = this.PlayerAwards.Award3.SmallImage;
                }
                if (!(((!user.IsAway && !user.IsDND) && !user.IsIgnored) && user.Online))
                {
                    this.NameColor = Program.Settings.Chat.Appearance.UnavailableColor;
                    this.NameFont = DefaultFont;
                }
                else if (user.IsAdmin || ((Chatroom.InChatroom && !Chatroom.Current.IsPersistent) && user.IsChannelOperator))
                {
                    this.NameColor = Program.Settings.Chat.Appearance.AdminColor;
                    this.NameFont = AdminFont;
                }
                else if (user.IsModerator)
                {
                    this.NameColor = Program.Settings.Chat.Appearance.ModeratorColor;
                    this.NameFont = Program.Settings.Chat.Appearance.ModeratorFont;
                }
                else
                {
                    this.NameColor = Program.Settings.Chat.Appearance.DefaultColor;
                    this.NameFont = DefaultFont;
                }
                if (this.NameColor == Color.Empty)
                {
                    this.NameColor = Program.Settings.Chat.Appearance.DefaultColor;
                }
                if (this.NameFont == null)
                {
                    this.NameFont = DefaultFont;
                }
                this.PlayerStatus = UserStatus.GetStatus(user);
                if (this.PlayerStatus != null)
                {
                    this.StatusImage = this.PlayerStatus.Icon;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void Dispose()
        {
            this.Parent.MouseMove -= new MouseEventHandler(this.Parent_MouseMove);
            this.Parent.MouseDown -= new MouseEventHandler(this.Parent_MouseDown);
            this.Parent.MouseUp -= new MouseEventHandler(this.Parent_MouseUp);
        }

        private void LabelClan_Click(object sender, EventArgs e)
        {
            Program.MainForm.OnViewClanProfileByName(this.User.ClanName);
        }

        public void MoveToCategory(UserListCategory category)
        {
            if (this.LastCategory != null)
            {
                this.LastCategory.RemoveRow(this);
            }
            this.mCategory = category.Category;
            category.AddRow(this);
            this.LastCategory = category;
        }

        private void OnMouseDown(MouseEventArgs e)
        {
            this.mIsSelected = true;
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                this.Parent.Cursor = Cursors.Default;
                this.Parent.ttDefault.UseFading = true;
                this.Parent.ttDefault.ShowAlways = true;
                if (this.Style == UserListStyles.Clan)
                {
                    if (this.ClanRankBounds.Contains(e.Location))
                    {
                        if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is Rectangle) && (((Rectangle) this.Parent.ttDefault.Tag) != this.ClanRankBounds)))
                        {
                            this.Parent.ttDefault.Tag = this.ClanRankBounds;
                            this.Parent.ttDefault.Show(Loc.Get(this.ClanMember.GetRanking().Description), this.Parent.gpgPanelBody, this.ClanRankBounds.X, this.ClanRankBounds.Y - 20, 0x1388);
                        }
                    }
                    else if ((this.PlayerStatus != null) && this.StatusBounds.Contains(e.Location))
                    {
                        if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is Rectangle) && (((Rectangle) this.Parent.ttDefault.Tag) != this.StatusBounds)))
                        {
                            this.Parent.ttDefault.Tag = this.StatusBounds;
                            this.Parent.ttDefault.Show(this.PlayerStatus.Description, this.Parent.gpgPanelBody, this.StatusBounds.X, this.StatusBounds.Y - 20, 0x1388);
                        }
                    }
                    else
                    {
                        this.Parent.ttDefault.Tag = null;
                        this.Parent.ttDefault.Hide(this.Parent.gpgPanelBody);
                    }
                }
                else if ((Program.Settings.Awards.ShowAwards && this.PlayerAwards.Award1Specified) && this.Award1Bounds.Contains(e.Location))
                {
                    if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is Rectangle) && (((Rectangle) this.Parent.ttDefault.Tag) != this.Award1Bounds)))
                    {
                        this.Parent.ttDefault.Tag = this.Award1Bounds;
                        this.Parent.ttDefault.Show(Loc.Get(this.PlayerAwards.Award1.AchievementDescription), this.Parent.gpgPanelBody, this.Award1Bounds.X, this.Award1Bounds.Y - 20, 0x1388);
                    }
                }
                else if ((Program.Settings.Awards.ShowAwards && this.PlayerAwards.Award2Specified) && this.Award2Bounds.Contains(e.Location))
                {
                    if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is Rectangle) && (((Rectangle) this.Parent.ttDefault.Tag) != this.Award2Bounds)))
                    {
                        this.Parent.ttDefault.Tag = this.Award2Bounds;
                        this.Parent.ttDefault.Show(Loc.Get(this.PlayerAwards.Award2.AchievementDescription), this.Parent.gpgPanelBody, this.Award2Bounds.X, this.Award2Bounds.Y - 20, 0x1388);
                    }
                }
                else if ((Program.Settings.Awards.ShowAwards && this.PlayerAwards.Award3Specified) && this.Award3Bounds.Contains(e.Location))
                {
                    if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is Rectangle) && (((Rectangle) this.Parent.ttDefault.Tag) != this.Award3Bounds)))
                    {
                        this.Parent.ttDefault.Tag = this.Award3Bounds;
                        this.Parent.ttDefault.Show(Loc.Get(this.PlayerAwards.Award3.AchievementDescription), this.Parent.gpgPanelBody, this.Award3Bounds.X, this.Award3Bounds.Y - 20, 0x1388);
                    }
                }
                else if (Program.Settings.Awards.ShowAvatars && this.AvatarBounds.Contains(e.Location))
                {
                    if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is Rectangle) && (((Rectangle) this.Parent.ttDefault.Tag) != this.AvatarBounds)))
                    {
                        this.Parent.ttDefault.Tag = this.AvatarBounds;
                        this.Parent.ttDefault.Show(Loc.Get(this.PlayerAwards.Avatar.Description), this.Parent.gpgPanelBody, this.AvatarBounds.X, this.AvatarBounds.Y - 20, 0x1388);
                    }
                }
                else if ((this.PlayerStatus != null) && this.StatusBounds.Contains(e.Location))
                {
                    if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is Rectangle) && (((Rectangle) this.Parent.ttDefault.Tag) != this.StatusBounds)))
                    {
                        this.Parent.ttDefault.Tag = this.StatusBounds;
                        this.Parent.ttDefault.Show(this.PlayerStatus.Description, this.Parent.gpgPanelBody, this.StatusBounds.X, this.StatusBounds.Y - 20, 0x1388);
                    }
                }
                else if (this.ClanLabelBounds.HasValue && this.ClanLabelBounds.Value.Contains((float) e.X, (float) e.Y))
                {
                    if (this.Style == UserListStyles.OnlineOffline)
                    {
                        this.Parent.Cursor = Cursors.Hand;
                    }
                    if ((this.Parent.ttDefault.Tag == null) || ((this.Parent.ttDefault.Tag is RectangleF) && (((RectangleF) this.Parent.ttDefault.Tag) != this.ClanLabelBounds.Value)))
                    {
                        this.Parent.ttDefault.Tag = this.ClanLabelBounds.Value;
                        this.Parent.ttDefault.Show(Loc.Get(this.User.ClanName), this.Parent.gpgPanelBody, (int) this.ClanLabelBounds.Value.X, ((int) this.ClanLabelBounds.Value.Y) - 20, 0x1388);
                    }
                }
                else
                {
                    this.Parent.ttDefault.Tag = null;
                    this.Parent.ttDefault.Hide(this.Parent.gpgPanelBody);
                }
                if (this.MouseMove != null)
                {
                    this.MouseMove(this, e);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void OnMouseUp(MouseEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(this, e);
            }
        }

        public void Paint(Rectangle clip, Graphics g)
        {
            try
            {
                if (this.Visible && !this.ClientBounds.IsEmpty)
                {
                    SolidBrush brush;
                    if (this.Style == UserListStyles.Clan)
                    {
                        if (this.IsSelected)
                        {
                            using (brush = new SolidBrush(Color.FromArgb(0x2e, 0x2e, 0x49)))
                            {
                                g.FillRectangle(brush, this.ClientBounds);
                            }
                        }
                        if (this.ClanRankImage != null)
                        {
                            g.DrawImage(this.ClanRankImage, this.ClanRankBounds);
                        }
                        using (brush = new SolidBrush(this.NameColor))
                        {
                            g.DrawString(this.ClanMember.Name, this.NameFont, brush, this.NameLabelLocation);
                        }
                        if (this.StatusImage != null)
                        {
                            g.DrawImage(this.StatusImage, this.StatusBounds, new Rectangle(10, 0, 20, 20), GraphicsUnit.Pixel);
                        }
                    }
                    else
                    {
                        if (!this.ClanLabelBounds.HasValue)
                        {
                            SizeF ef = DrawUtil.MeasureString(g, this.User.Name, this.NameFont);
                            SizeF ef2 = DrawUtil.MeasureString(g, this.User.ClanAbbreviation, Program.Settings.Chat.Appearance.ClanTagFont);
                            this.ClanLabelBounds = new RectangleF((this.NameLabelLocation.X + ef.Width) + 5f, this.NameLabelLocation.Y + 2f, ef2.Width + 6f, ef2.Height);
                        }
                        if (this.IsSelected)
                        {
                            using (brush = new SolidBrush(Color.FromArgb(0x2e, 0x2e, 0x49)))
                            {
                                g.FillRectangle(brush, this.ClientBounds);
                            }
                        }
                        if (Program.Settings.Awards.ShowAwards)
                        {
                            if (this.Award1Image != null)
                            {
                                g.DrawImage(this.Award1Image, this.Award1Bounds);
                            }
                            if (this.Award2Image != null)
                            {
                                g.DrawImage(this.Award2Image, this.Award2Bounds);
                            }
                            if (this.Award3Image != null)
                            {
                                g.DrawImage(this.Award3Image, this.Award3Bounds);
                            }
                        }
                        if (Program.Settings.Awards.ShowAvatars && (this.AvatarImage != null))
                        {
                            lock (this.AvatarImage)
                            {
                                g.DrawImage(this.AvatarImage, this.AvatarBounds);
                            }
                        }
                        using (brush = new SolidBrush(this.NameColor))
                        {
                            g.DrawString(this.User.Name, this.NameFont, brush, this.NameLabelLocation);
                        }
                        using (brush = new SolidBrush(Program.Settings.Chat.Appearance.ClanColor))
                        {
                            g.DrawString(this.User.ClanAbbreviation, Program.Settings.Chat.Appearance.ClanTagFont, brush, this.ClanLabelBounds.Value);
                        }
                        if (this.StatusImage != null)
                        {
                            g.DrawImage(this.StatusImage, this.StatusBounds, new Rectangle(10, 0, 20, 20), GraphicsUnit.Pixel);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void Parent_MouseDown(object sender, MouseEventArgs e)
        {
            if ((((this.Style == UserListStyles.OnlineOffline) && this.User.IsInClan) && this.ClanLabelBounds.HasValue) && this.ClanLabelBounds.Value.Contains((float) e.X, (float) e.Y))
            {
                Program.MainForm.OnViewClanProfileByName(this.User.ClanName);
            }
            if (this.Visible && this.ClientBounds.Contains(e.Location))
            {
                this.OnMouseDown(e);
            }
        }

        private void Parent_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Visible && this.ClientBounds.Contains(e.Location))
            {
                this.OnMouseMove(e);
            }
        }

        private void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.Visible && this.ClientBounds.Contains(e.Location))
            {
                this.OnMouseUp(e);
            }
        }

        public void SetBounds(int top)
        {
            try
            {
                int x = 0;
                this.ClientBounds = new Rectangle(x, top, this.Parent.ClientSize.Width, 0x18);
                if (this.Style == UserListStyles.Clan)
                {
                    if (this.ClanRankImage != null)
                    {
                        this.ClanRankBounds = new Rectangle(x + 2, top + 2, this.ClanRankImage.Width, this.ClanRankImage.Height);
                    }
                    this.NameLabelLocation = new PointF((float) (x + 0x2d), (float) (top + 4));
                }
                else
                {
                    if (Program.Settings.Awards.ShowAwards)
                    {
                        if (this.Award1Image != null)
                        {
                            this.Award1Bounds = new Rectangle(x + 2, top + 2, this.Award1Image.Width, this.Award1Image.Height);
                        }
                        if (this.Award2Image != null)
                        {
                            this.Award2Bounds = new Rectangle(x + 0x16, top + 2, this.Award2Image.Width, this.Award2Image.Height);
                        }
                        if (this.Award3Image != null)
                        {
                            this.Award3Bounds = new Rectangle(x + 0x2a, top + 2, this.Award3Image.Width, this.Award3Image.Height);
                        }
                    }
                    if (Program.Settings.Awards.ShowAvatars)
                    {
                        Image image;
                        if (Program.Settings.Awards.ShowAwards)
                        {
                            if (this.AvatarImage != null)
                            {
                                lock ((image = this.AvatarImage))
                                {
                                    this.AvatarBounds = new Rectangle(x + 0x42, top + 2, this.AvatarImage.Width, this.AvatarImage.Height);
                                }
                            }
                        }
                        else if (this.AvatarImage != null)
                        {
                            lock ((image = this.AvatarImage))
                            {
                                this.AvatarBounds = new Rectangle(x + 2, top + 2, this.AvatarImage.Width, this.AvatarImage.Height);
                            }
                        }
                    }
                    if (Program.Settings.Awards.ShowAwards && Program.Settings.Awards.ShowAvatars)
                    {
                        this.NameLabelLocation = new PointF((float) (x + 0x6d), (float) (top + 4));
                    }
                    else if (Program.Settings.Awards.ShowAwards)
                    {
                        this.NameLabelLocation = new PointF((float) (x + 0x41), (float) (top + 4));
                    }
                    else if (Program.Settings.Awards.ShowAvatars)
                    {
                        this.NameLabelLocation = new PointF((float) (x + 0x2d), (float) (top + 4));
                    }
                    else
                    {
                        this.NameLabelLocation = new PointF((float) (x + 2), (float) (top + 4));
                    }
                    this.ClanLabelBounds = null;
                }
                this.StatusBounds = new Rectangle(this.ClientBounds.Width - 0x16, top + 2, 20, 20);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public bool BoundsSet
        {
            get
            {
                return !this.ClientBounds.IsEmpty;
            }
        }

        public UserListCategories Category
        {
            get
            {
                return this.mCategory;
            }
        }

        public GPG.Multiplayer.Client.Clans.ClanMember ClanMember
        {
            get
            {
                return this.mClanMember;
            }
        }

        public int Height
        {
            get
            {
                return this.ClientBounds.Height;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.mIsSelected;
            }
            set
            {
                this.mIsSelected = value;
            }
        }

        public int Left
        {
            get
            {
                return this.ClientBounds.Left;
            }
        }

        public IUser Member
        {
            get
            {
                if (this.Style == UserListStyles.Clan)
                {
                    return this.ClanMember;
                }
                return this.User;
            }
        }

        public UserListRow NextRow
        {
            get
            {
                return this.mNextRow;
            }
            set
            {
                this.mNextRow = value;
            }
        }

        public PnlUserList Parent
        {
            get
            {
                return this.mParent;
            }
        }

        public UserListRow PreviousRow
        {
            get
            {
                return this.mPreviousRow;
            }
            set
            {
                this.mPreviousRow = value;
            }
        }

        public UserListStyles Style
        {
            get
            {
                return this.mStyle;
            }
        }

        public int Top
        {
            get
            {
                return this.ClientBounds.Top;
            }
        }

        public GPG.Multiplayer.Quazal.User User
        {
            get
            {
                return this.mUser;
            }
        }

        public bool Visible
        {
            get
            {
                return (((this.LastCategory != null) && this.LastCategory.Visible) && this.LastCategory.IsExpanded);
            }
        }

        public int Width
        {
            get
            {
                return this.ClientBounds.Width;
            }
        }
    }
}

