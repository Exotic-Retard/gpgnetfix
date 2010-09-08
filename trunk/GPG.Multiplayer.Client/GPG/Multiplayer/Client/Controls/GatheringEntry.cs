namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.UI;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    internal class GatheringEntry : Panel
    {
        private int mPoplation;
        private Chatroom mRoom;
        private LinkLabel NameLink = new LinkLabel();
        private Label PopulationLabel = new Label();

        public event StringEventHandler GatheringClick;

        public GatheringEntry(Chatroom room)
        {
            this.mRoom = room;
            base.Height = 0x16;
            this.NameLink.Height = 0x16;
            this.NameLink.AutoSize = true;
            this.NameLink.Text = room.Description;
            this.NameLink.Location = DrawUtil.CenterV(this, this.NameLink);
            this.NameLink.VisitedLinkColor = Color.WhiteSmoke;
            this.NameLink.LinkColor = Color.WhiteSmoke;
            this.NameLink.Font = Program.Settings.Appearance.Text.MasterFont;
            this.NameLink.Click += new EventHandler(this.NameLink_Click);
            base.Controls.Add(this.NameLink);
            this.PopulationLabel.AutoSize = true;
            this.PopulationLabel.Height = 0x16;
            this.PopulationLabel.Font = Program.Settings.Appearance.Text.MasterFont;
            this.PopulationLabel.Location = new Point(this.NameLink.Width + 6, DrawUtil.CenterV(this, this.PopulationLabel).Y);
            base.Controls.Add(this.PopulationLabel);
            this.Population = this.Room.Population;
        }

        private void NameLink_Click(object sender, EventArgs e)
        {
            if (this.GatheringClick != null)
            {
                this.GatheringClick(this.mRoom.Description);
            }
        }

        public int Population
        {
            get
            {
                return this.mPoplation;
            }
            set
            {
                this.mPoplation = value;
                this.Room.Population = value;
                if (this.mPoplation >= Chatroom.MaxRoomSize)
                {
                    this.PopulationLabel.ForeColor = Color.Red;
                }
                else if (this.Room.Population < 20)
                {
                    this.PopulationLabel.ForeColor = Color.Blue;
                }
                else if (this.Room.Population < 40)
                {
                    this.PopulationLabel.ForeColor = Color.Yellow;
                }
                else if (this.Room.Population < 60)
                {
                    this.PopulationLabel.ForeColor = Color.Orange;
                }
                else
                {
                    this.PopulationLabel.ForeColor = Color.Red;
                }
                if (this.mPoplation >= Chatroom.MaxRoomSize)
                {
                    this.PopulationLabel.Text = Loc.Get("<LOC>(FULL)");
                }
                else
                {
                    this.PopulationLabel.Text = string.Format("({0})", this.Population);
                }
                this.PopulationLabel.Refresh();
            }
        }

        public Chatroom Room
        {
            get
            {
                return this.mRoom;
            }
        }
    }
}

