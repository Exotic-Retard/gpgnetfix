namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SkinGatheringDisplay : SkinDropDown, IGatheringDisplay
    {
        private const int LPAD = 6;
        private Chatroom mCurrentRoom;
        private FrmMain mMainForm;
        private System.Windows.Forms.ToolTip ToolTip;

        public event StringEventHandler GatheringSelected;

        public event EventHandler Popup;

        public SkinGatheringDisplay()
        {
            EventHandler handler = null;
            this.ToolTip = new System.Windows.Forms.ToolTip();
            this.mCurrentRoom = null;
            base.TextAlign = ContentAlignment.MiddleLeft;
            this.HorizontalScalingMode = ScalingModes.Stretch;
            base.Height = Program.Settings.Appearance.Chat.ChatroomListHeight;
            base.Width = Program.Settings.Appearance.Chat.ChatroomListWidth;
            if (handler == null)
            {
                handler = delegate (object s, EventArgs e) {
                    if (this.Popup != null)
                    {
                        this.Popup(this, e);
                    }
                };
            }
            base.Menu.Popup += handler;
            this.BindToSettings();
        }

        private void BindToSettings()
        {
            Program.Settings.Appearance.Chat.ShowChatroomIconsChanged += delegate (object s, PropertyChangedEventArgs e) {
                foreach (SkinMenuItem item in base.Menu.MenuItems)
                {
                    if (Program.Settings.Appearance.Chat.ShowChatroomIcons)
                    {
                        item.Icon = this.GetPopulationImage((item.Tag as Chatroom).Population);
                    }
                    else
                    {
                        item.Icon = null;
                    }
                }
                this.CalculateName();
                this.Refresh();
            };
            Program.Settings.Appearance.Chat.ChatroomIconSizeChanged += delegate (object s, PropertyChangedEventArgs e) {
                foreach (SkinMenuItem item in base.Menu.MenuItems)
                {
                    if (Program.Settings.Appearance.Chat.ShowChatroomIcons)
                    {
                        item.Icon = this.GetPopulationImage((item.Tag as Chatroom).Population);
                    }
                    else
                    {
                        item.Icon = null;
                    }
                }
                this.CalculateName();
                this.Refresh();
            };
            Program.Settings.Appearance.Chat.ChatroomColorChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.Refresh();
            };
            Program.Settings.Appearance.Chat.ChatroomFontChanged += delegate (object s, PropertyChangedEventArgs e) {
                this.CalculateName();
                this.Refresh();
            };
            Program.Settings.Appearance.Menus.DropDownItemHeightChanged += delegate (object s, PropertyChangedEventArgs e) {
                MenuItem[] dest = new MenuItem[base.Menu.MenuItems.Count];
                base.Menu.MenuItems.CopyTo(dest, 0);
                base.Menu.MenuItems.Clear();
                foreach (SkinMenuItem item in dest)
                {
                    item.ClearSkins();
                }
                base.Menu.MenuItems.AddRange(dest);
            };
            Program.Settings.Appearance.Menus.DropDownItemWidthChanged += delegate (object s, PropertyChangedEventArgs e) {
                MenuItem[] dest = new MenuItem[base.Menu.MenuItems.Count];
                base.Menu.MenuItems.CopyTo(dest, 0);
                base.Menu.MenuItems.Clear();
                foreach (SkinMenuItem item in dest)
                {
                    item.ClearSkins();
                }
                base.Menu.MenuItems.AddRange(dest);
            };
            Program.Settings.Appearance.Chat.ChatroomListHeightChanged += delegate (object s, PropertyChangedEventArgs e) {
                base.Height = Program.Settings.Appearance.Chat.ChatroomListHeight;
                if (base.Parent != null)
                {
                    base.Location = DrawUtil.Center(base.Parent, this);
                }
            };
            Program.Settings.Appearance.Chat.ChatroomListWidthChanged += delegate (object s, PropertyChangedEventArgs e) {
                base.Width = Program.Settings.Appearance.Chat.ChatroomListWidth;
                if (base.Parent != null)
                {
                    this.CalculateName();
                    base.Location = DrawUtil.Center(base.Parent, this);
                }
            };
        }

        private void CalculateName()
        {
            VGen0 method = null;
            try
            {
                if ((this.CurrentRoom != null) && base.SkinLoaded)
                {
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.CalculateName();
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (!base.Disposing && !base.IsDisposed)
                    {
                        string description;
                        if (this.CurrentRoom.Population >= 0)
                        {
                            description = string.Format("{0} ({1})", this.CurrentRoom.Description, this.CurrentRoom.Population);
                        }
                        else
                        {
                            description = this.CurrentRoom.Description;
                        }
                        using (Graphics graphics = base.CreateGraphics())
                        {
                            int width = base.RightImage.Width;
                            int left = this.TextPadding.Left;
                            int num3 = (width + left) + 4;
                            int num4 = Convert.ToInt32(DrawUtil.MeasureString(graphics, description, this.Font).Width);
                            if (num4 > (base.Width - num3))
                            {
                                this.ToolTip.SetToolTip(this, description);
                                int num5 = Convert.ToInt32(DrawUtil.MeasureString(graphics, "...", this.Font).Width);
                                int startIndex = this.CurrentRoom.Description.Length - 1;
                                while (num4 > (base.Width - (num3 + num5)))
                                {
                                    if (startIndex < 0)
                                    {
                                        break;
                                    }
                                    char ch = description[startIndex];
                                    num4 -= Convert.ToInt32(DrawUtil.MeasureString(graphics, ch.ToString(), this.Font).Width);
                                    description = description.Remove(startIndex, 1);
                                    startIndex--;
                                }
                                description = description.Insert(startIndex + 1, "...");
                            }
                            else
                            {
                                this.ToolTip.RemoveAll();
                            }
                        }
                        base.Text = description;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void CharoomPopulationChanged(object sender, ListChangedEventArgs e)
        {
            this.CurrentRoom.Population = Chatroom.CurrentPopulation;
            this.CalculateName();
        }

        private Image GetPopulationImage(int pop)
        {
            Image img = null;
            if (pop < 10)
            {
                img = PopulationIcons.empty;
            }
            else if (pop < 40)
            {
                img = PopulationIcons.low;
            }
            else if (pop < 70)
            {
                img = PopulationIcons.medium;
            }
            else if (pop < 80)
            {
                img = PopulationIcons.full;
            }
            else
            {
                img = PopulationIcons.full;
            }
            if (img != null)
            {
                img = DrawUtil.ResizeImage(img, Program.Settings.Chat.Appearance.ChatroomIconSize);
            }
            return img;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Program.Settings.Appearance.Chat.ShowChatroomIcons && (this.CurrentRoom != null))
            {
                base.Icon = this.GetPopulationImage(this.CurrentRoom.Population);
            }
            else
            {
                base.Icon = null;
            }
            base.OnPaint(e);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            EventHandler handler = null;
            base.OnParentChanged(e);
            if (base.Parent != null)
            {
                base.Location = DrawUtil.Center(base.Parent, this);
                if (handler == null)
                {
                    handler = delegate (object s, EventArgs e1) {
                        base.Location = DrawUtil.Center(base.Parent, this);
                    };
                }
                base.Parent.SizeChanged += handler;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            int width;
            base.Height = Program.Settings.Appearance.Chat.ChatroomListHeight;
            if (base.RightImage != null)
            {
                width = base.RightImage.Width;
            }
            else
            {
                width = 40;
            }
            int left = this.TextPadding.Left;
            int num3 = (width + left) + 4;
            if (base.Width < num3)
            {
                base.Width = num3;
            }
            this.CalculateName();
        }

        public void RefreshGatherings(Chatroom[] rooms, bool clearFirst)
        {
            EventHandler handler = null;
            EventHandler handler2 = null;
            EventHandler handler3 = null;
            bool flag = clearFirst || (this.RoomCount < 1);
            if (clearFirst)
            {
                base.Menu.MenuItems.Clear();
            }
            for (int i = 0; i < rooms.Length; i++)
            {
                if (Chatroom.InChatroom && rooms[i].Equals(Chatroom.Current))
                {
                    rooms[i].Population = Chatroom.CurrentPopulation;
                }
                if (base.Menu.MenuItems.ContainsKey(rooms[i].Description))
                {
                    base.Menu.MenuItems[rooms[i].Description].Text = string.Format("{0} ({1})", rooms[i].Description, rooms[i].Population);
                    if (Program.Settings.Chat.Appearance.ShowChatroomIcons)
                    {
                        (base.Menu.MenuItems[rooms[i].Description] as SkinMenuItem).Icon = this.GetPopulationImage(rooms[i].Population);
                    }
                }
                else
                {
                    SkinMenuItem item = new SkinMenuItem(string.Format("{0} ({1})", rooms[i].Description, rooms[i].Population));
                    item.Tag = rooms[i];
                    if (Program.Settings.Chat.Appearance.ShowChatroomIcons)
                    {
                        item.Icon = this.GetPopulationImage(rooms[i].Population);
                    }
                    item.SkinBasePath = @"Controls\Menus\DropDownItem";
                    item.Name = rooms[i].Description;
                    if (handler == null)
                    {
                        handler = delegate (object s, EventArgs e) {
                            if (this.GatheringSelected != null)
                            {
                                this.GatheringSelected(((s as MenuItem).Tag as Chatroom).Description);
                            }
                        };
                    }
                    item.Click += handler;
                    base.Menu.MenuItems.Add(item);
                }
            }
            if (flag && ConfigSettings.GetBool("ShowChannels", false))
            {
                SkinMenuItem item2 = new SkinMenuItem(Loc.Get("<LOC>Select Channel..."));
                item2.SkinBasePath = @"Controls\Menus\DropDownItem";
                if (handler2 == null)
                {
                    handler2 = delegate (object s, EventArgs e) {
                        this.MainForm.ShowDlgSelectChannel();
                    };
                }
                item2.Click += handler2;
                base.Menu.MenuItems.Add(item2);
                SkinMenuItem item3 = new SkinMenuItem(Loc.Get("<LOC>Create Channel..."));
                item3.SkinBasePath = @"Controls\Menus\DropDownItem";
                if (handler3 == null)
                {
                    handler3 = delegate (object s, EventArgs e) {
                        new DlgCreateChannel(this.MainForm).ShowDialog();
                    };
                }
                item3.Click += handler3;
                base.Menu.MenuItems.Add(item3);
            }
        }

        public void RefreshGatherings(MappedObjectList<Chatroom> rooms, bool clearFirst)
        {
            this.RefreshGatherings(rooms.ToArray(), clearFirst);
        }

        [Browsable(false)]
        public Chatroom CurrentRoom
        {
            get
            {
                return this.mCurrentRoom;
            }
            set
            {
                if (value != null)
                {
                    this.mCurrentRoom = value;
                    if (this.CurrentRoom.Population < 0)
                    {
                        Chatroom.GatheringParticipants.ListChanged -= new ListChangedEventHandler(this.CharoomPopulationChanged);
                    }
                    else
                    {
                        this.CharoomPopulationChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
                        Chatroom.GatheringParticipants.ListChanged += new ListChangedEventHandler(this.CharoomPopulationChanged);
                    }
                    this.CalculateName();
                }
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return Program.Settings.Appearance.Chat.ChatroomFont;
            }
            set
            {
            }
        }

        public override Color ForeColor
        {
            get
            {
                return Program.Settings.Appearance.Chat.ChatroomColor;
            }
            set
            {
            }
        }

        public override ScalingModes HorizontalScalingMode
        {
            get
            {
                return ScalingModes.Stretch;
            }
            set
            {
            }
        }

        private int ICON_HEIGHT
        {
            get
            {
                return Program.Settings.Chat.Appearance.ChatroomIconSize.Height;
            }
        }

        private int ICON_WIDTH
        {
            get
            {
                return Program.Settings.Chat.Appearance.ChatroomIconSize.Width;
            }
        }

        public FrmMain MainForm
        {
            get
            {
                return this.mMainForm;
            }
            internal set
            {
                this.mMainForm = value;
            }
        }

        public int RoomCount
        {
            get
            {
                return base.Menu.MenuItems.Count;
            }
        }

        public override string SkinBasePath
        {
            get
            {
                return @"Controls\Button\ChatroomList";
            }
            set
            {
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public override Padding TextPadding
        {
            get
            {
                if (Program.Settings.Appearance.Chat.ShowChatroomIcons)
                {
                    return new Padding(this.ICON_WIDTH + 12, 0, 0, 0);
                }
                return new Padding(6, 0, 0, 0);
            }
            set
            {
            }
        }
    }
}

