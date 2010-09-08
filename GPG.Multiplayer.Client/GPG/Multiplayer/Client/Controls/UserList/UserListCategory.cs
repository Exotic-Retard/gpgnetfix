namespace GPG.Multiplayer.Client.Controls.UserList
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Clans;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class UserListCategory : IEnumerable<UserListRow>, IEnumerable, IDisposable
    {
        private Image BackgroundImage;
        private static Font CategoryFont = new Font("Verdana", 8f, FontStyle.Bold);
        private string CategoryName;
        private Rectangle ClientBounds;
        private PointF DisplayNameLocation;
        private Rectangle ExpandContractImageBounds;
        internal object LayoutMutex = new object();
        private UserListCategories mCategory;
        private Image mExpandContractImage;
        private bool mIsExpanded = true;
        private PnlUserList mParent;
        private bool mVisible = false;
        private SortedList<string, UserListRow> UserRows = new SortedList<string, UserListRow>();

        public event EventHandler ExpandContractChanged;

        public event MouseEventHandler MouseDown;

        public event MouseEventHandler MouseMove;

        public event MouseEventHandler MouseUp;

        public event CategoryRowEventHandler RowAdded;

        public event CategoryRowEventHandler RowRemoved;

        public event EventHandler VisibleChanged;

        public UserListCategory(PnlUserList parent, UserListCategories category, UserListStyles style)
        {
            this.mParent = parent;
            this.mExpandContractImage = UserListImages.contract;
            this.mCategory = category;
            switch (style)
            {
                case UserListStyles.Chatroom:
                    switch (category)
                    {
                        case UserListCategories.Online:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_online.png");
                            break;

                        case UserListCategories.Offline:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_offline.png");
                            break;

                        case UserListCategories.Away:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_away.png");
                            break;

                        case UserListCategories.Admin:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_admin.png");
                            break;

                        case UserListCategories.Moderator:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_moderator.png");
                            break;

                        case UserListCategories.Friend:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_friends.png");
                            break;

                        case UserListCategories.Clan:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_clan.png");
                            break;

                        case UserListCategories.Speaking:
                            this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_online.png");
                            break;
                    }
                    this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_offline.png");
                    break;

                case UserListStyles.OnlineOffline:
                    if (category != UserListCategories.Online)
                    {
                        this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_offline.png");
                        break;
                    }
                    this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_online.png");
                    break;

                case UserListStyles.Clan:
                    this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_clan.png");
                    break;

                default:
                    this.BackgroundImage = SkinManager.GetImage(@"Controls\UserList\category_black.png");
                    break;
            }
            switch (category)
            {
                case UserListCategories.Online:
                    this.CategoryName = Loc.Get("<LOC>Online");
                    break;

                case UserListCategories.Offline:
                    this.CategoryName = Loc.Get("<LOC>Offline");
                    break;

                case UserListCategories.Away:
                    this.CategoryName = Loc.Get("<LOC>Away");
                    break;

                case UserListCategories.Admin:
                    this.CategoryName = Loc.Get("<LOC>Administrators");
                    break;

                case UserListCategories.Moderator:
                    this.CategoryName = Loc.Get("<LOC>Chat Moderators");
                    break;

                case UserListCategories.Friend:
                    this.CategoryName = Loc.Get("<LOC>Friends");
                    break;

                case UserListCategories.Clan:
                    this.CategoryName = Loc.Get("<LOC>Clan Members");
                    break;

                case UserListCategories.Speaking:
                    this.CategoryName = Loc.Get("<LOC>Speaking");
                    break;

                case UserListCategories.Clan_Rank0:
                    this.CategoryName = ClanRanking.All[0].Description;
                    break;

                case UserListCategories.Clan_Rank1:
                    this.CategoryName = ClanRanking.All[1].Description;
                    break;

                case UserListCategories.Clan_Rank2:
                    this.CategoryName = ClanRanking.All[2].Description;
                    break;

                case UserListCategories.Clan_Rank3:
                    this.CategoryName = ClanRanking.All[3].Description;
                    break;

                case UserListCategories.Clan_Rank4:
                    this.CategoryName = ClanRanking.All[4].Description;
                    break;

                case UserListCategories.Clan_Rank5:
                    this.CategoryName = ClanRanking.All[5].Description;
                    break;
            }
            this.Parent.MouseMove += new MouseEventHandler(this.Parent_MouseMove);
            this.Parent.MouseDown += new MouseEventHandler(this.Parent_MouseDown);
            this.Parent.MouseUp += new MouseEventHandler(this.Parent_MouseUp);
        }

        public void AddRow(UserListRow row)
        {
            this.UserRows[row.Member.Name] = row;
            this.Visible = this.UserRows.Count > 0;
            if (this.RowAdded != null)
            {
                this.RowAdded(this, row);
            }
        }

        public void Clear()
        {
            this.UserRows.Clear();
            this.Visible = this.UserRows.Count > 0;
        }

        public void Contract()
        {
            this.mIsExpanded = false;
            this.mExpandContractImage = UserListImages.expand;
            if (this.ExpandContractChanged != null)
            {
                this.ExpandContractChanged(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            this.Parent.MouseMove -= new MouseEventHandler(this.Parent_MouseMove);
            this.Parent.MouseDown -= new MouseEventHandler(this.Parent_MouseDown);
            this.Parent.MouseUp -= new MouseEventHandler(this.Parent_MouseUp);
        }

        public void Expand()
        {
            this.mIsExpanded = true;
            this.mExpandContractImage = UserListImages.contract;
            if (this.ExpandContractChanged != null)
            {
                this.ExpandContractChanged(this, EventArgs.Empty);
            }
        }

        public void ExpandOrContract()
        {
            if (this.IsExpanded)
            {
                this.Contract();
            }
            else
            {
                this.Expand();
            }
        }

        public IEnumerator<UserListRow> GetEnumerator()
        {
            return this.UserRows.Values.GetEnumerator();
        }

        private void gpgPictureBoxExpandContract_Click(object sender, EventArgs e)
        {
            this.ExpandOrContract();
        }

        private void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && this.ExpandContractImageBounds.Contains(e.Location))
            {
                this.ExpandOrContract();
            }
            if (this.MouseDown != null)
            {
                this.MouseDown(this, e);
            }
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(this, e);
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
                if (!this.ClientBounds.IsEmpty && this.Visible)
                {
                    for (int i = 0; i < this.ClientBounds.Width; i += this.BackgroundImage.Width)
                    {
                        g.DrawImage(this.BackgroundImage, new Rectangle(i, this.ClientBounds.Top, this.BackgroundImage.Width, this.BackgroundImage.Height));
                    }
                    g.DrawImage(this.ExpandContractImage, this.ExpandContractImageBounds);
                    g.DrawString(this.DisplayName, CategoryFont, Brushes.White, this.DisplayNameLocation);
                    if (this.IsExpanded)
                    {
                        foreach (UserListRow row in this)
                        {
                            row.Paint(clip, g);
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

        public void RemoveRow(UserListRow row)
        {
            this.UserRows.Remove(row.Member.Name);
            this.Visible = this.UserRows.Count > 0;
            if (this.RowRemoved != null)
            {
                this.RowRemoved(this, row);
            }
        }

        public void SetBounds(int top)
        {
            try
            {
                int x = 0;
                this.ClientBounds = new Rectangle(x, top, this.Parent.Width, 20);
                this.ExpandContractImageBounds = new Rectangle(x + 8, top + 6, this.ExpandContractImage.Width, this.ExpandContractImage.Height);
                this.DisplayNameLocation = new PointF((float) (x + 0x1b), (float) (top + 3));
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.UserRows.Values.GetEnumerator();
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

        public int Count
        {
            get
            {
                return this.UserRows.Count;
            }
        }

        public string DisplayName
        {
            get
            {
                return string.Format("{0} ({1})", this.CategoryName, this.Count);
            }
        }

        public Image ExpandContractImage
        {
            get
            {
                return this.mExpandContractImage;
            }
        }

        public int Height
        {
            get
            {
                return this.ClientBounds.Height;
            }
        }

        public bool IsExpanded
        {
            get
            {
                return this.mIsExpanded;
            }
        }

        public int Left
        {
            get
            {
                return this.ClientBounds.Left;
            }
        }

        public PnlUserList Parent
        {
            get
            {
                return this.mParent;
            }
        }

        public SortedList<string, UserListRow> Rows
        {
            get
            {
                return this.UserRows;
            }
        }

        public int Top
        {
            get
            {
                return this.ClientBounds.Top;
            }
        }

        public bool Visible
        {
            get
            {
                return this.mVisible;
            }
            set
            {
                this.mVisible = value;
                if (this.VisibleChanged != null)
                {
                    this.VisibleChanged(this, EventArgs.Empty);
                }
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

