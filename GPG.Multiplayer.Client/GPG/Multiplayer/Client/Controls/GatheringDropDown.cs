namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GatheringDropDown : GPGComboBox, IGatheringDisplay
    {
        private Dictionary<string, GatheringEntry> Entries;
        public const int LineHeight = 0x16;
        private Chatroom mCurrentRoom;
        private Panel mFillPanel;
        private int mRoomCount;
        private GPGComboBoxPanel PopupPanel;

        public event StringEventHandler GatheringSelected;

        public GatheringDropDown()
        {
            PropertyChangedEventHandler handler = null;
            PropertyChangedEventHandler handler2 = null;
            PropertyChangedEventHandler handler3 = null;
            this.mFillPanel = null;
            this.PopupPanel = new GPGComboBoxPanel();
            this.Entries = new Dictionary<string, GatheringEntry>();
            this.mRoomCount = 0;
            this.PopupPanel.Location = new Point(0, 0);
            this.PopupPanel.BackColor = this.BackColor;
            this.mFillPanel = new Panel();
            this.mFillPanel.BackColor = this.BackColor;
            this.PopupPanel.Controls.Add(this.mFillPanel);
            this.mFillPanel.Dock = DockStyle.Fill;
            base.Properties.PopupControl = this.PopupPanel;
            if (handler == null)
            {
                handler = delegate (object s, PropertyChangedEventArgs e) {
                    this.Refresh();
                };
            }
            Program.Settings.Appearance.Chat.ChatroomFontChanged += handler;
            if (handler2 == null)
            {
                handler2 = delegate (object s, PropertyChangedEventArgs e) {
                    this.Refresh();
                };
            }
            Program.Settings.Appearance.Chat.ChatroomColorChanged += handler2;
            if (handler3 == null)
            {
                handler3 = delegate (object s, PropertyChangedEventArgs e) {
                    this.PopupPanel.BackColor = this.BackColor;
                    this.mFillPanel.BackColor = this.BackColor;
                    base.Properties.Appearance.BackColor = this.BackColor;
                    base.Properties.Appearance.BackColor2 = this.BackColor;
                    this.Refresh();
                };
            }
            Program.Settings.StylePreferences.MasterBackColorChanged += handler3;
        }

        private void GatheringDropDown_GatheringClick(string text)
        {
            this.PopupForm.BackColor = Color.Black;
            this.PopupForm.HidePopupForm();
            if (this.GatheringSelected != null)
            {
                this.GatheringSelected(text);
            }
        }

        private void GatheringParticipants_ListChanged(object sender, ListChangedEventArgs e)
        {
            VGen0 method = null;
            this.CurrentRoom.Population = Chatroom.CurrentPopulation;
            if (!base.Disposing && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.Refresh();
                    };
                }
                base.BeginInvoke(method);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.CurrentRoom != null)
            {
                int num = (e.ClipRectangle.Height - this.Font.Height) / 2;
                using (SolidBrush brush = new SolidBrush(this.ForeColor))
                {
                    e.Graphics.DrawString(this.CurrentRoom.Description, this.Font, brush, new PointF(4f, (float) (e.ClipRectangle.Y + num)));
                }
                if ((Chatroom.CurrentPopulation >= 0) && (this.CurrentRoom != Chatroom.None))
                {
                    Color blue;
                    if (Chatroom.CurrentPopulation < 20)
                    {
                        blue = Color.Blue;
                    }
                    else if (Chatroom.CurrentPopulation < 40)
                    {
                        blue = Color.Yellow;
                    }
                    else if (Chatroom.CurrentPopulation < 60)
                    {
                        blue = Color.Orange;
                    }
                    else
                    {
                        blue = Color.Red;
                    }
                    using (SolidBrush brush2 = new SolidBrush(blue))
                    {
                        e.Graphics.DrawString(string.Format("({0})", Chatroom.CurrentPopulation), this.Font, brush2, new PointF(e.Graphics.MeasureString(this.CurrentRoom.Description, this.Font).Width + 10f, (float) (e.ClipRectangle.Y + num)));
                    }
                }
            }
        }

        public void RefreshGatherings(Chatroom[] rooms, bool clearFirst)
        {
            if (clearFirst)
            {
                this.mFillPanel.Controls.Clear();
                this.Entries.Clear();
            }
            this.PopupPanel.Height = (0x16 * rooms.Length) + 4;
            if (this.PopupForm != null)
            {
                this.PopupForm.Height = (0x16 * rooms.Length) + 8;
            }
            for (int i = 0; i < rooms.Length; i++)
            {
                if (this.Entries.ContainsKey(rooms[i].Description))
                {
                    this.Entries[rooms[i].Description].Population = rooms[i].Population;
                }
                else
                {
                    this.Entries[rooms[i].Description] = new GatheringEntry(rooms[i]);
                    this.Entries[rooms[i].Description].Location = new Point(4, ((this.Entries.Count - 1) * 0x16) + 4);
                    this.Entries[rooms[i].Description].GatheringClick += new StringEventHandler(this.GatheringDropDown_GatheringClick);
                    this.mFillPanel.Controls.Add(this.Entries[rooms[i].Description]);
                }
            }
            this.mRoomCount = rooms.Length;
        }

        public void RefreshGatherings(MappedObjectList<Chatroom> rooms, bool clearFirst)
        {
            this.RefreshGatherings(rooms.ToArray(), clearFirst);
        }

        public override Color BackColor
        {
            get
            {
                return Program.Settings.StylePreferences.MasterBackColor;
            }
            set
            {
            }
        }

        public Chatroom CurrentRoom
        {
            get
            {
                return this.mCurrentRoom;
            }
            set
            {
                VGen0 method = null;
                if (value != null)
                {
                    this.mCurrentRoom = value;
                    if (base.InvokeRequired)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.Refresh();
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else
                    {
                        this.Refresh();
                    }
                    Chatroom.GatheringParticipants.ListChanged += new ListChangedEventHandler(this.GatheringParticipants_ListChanged);
                }
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return Program.Settings.Chat.Appearance.ChatroomFont;
            }
            set
            {
            }
        }

        public override Color ForeColor
        {
            get
            {
                return Program.Settings.Chat.Appearance.ChatroomColor;
            }
            set
            {
            }
        }

        public int RoomCount
        {
            get
            {
                return this.mRoomCount;
            }
        }
    }
}

