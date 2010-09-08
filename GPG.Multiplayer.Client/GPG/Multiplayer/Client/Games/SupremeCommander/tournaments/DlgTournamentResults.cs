namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgTournamentResults : DlgBase
    {
        private IContainer components;
        private GPGBorderPanel gpgBorderPanel1;
        private GPGBorderPanel gpgBorderPanel2;
        private GPGLabel gpgLabel1;
        private ListBox lbParticipants;
        private GPGLabel lStartTimeCaption;
        private int MaxRound;
        private List<TournamentPlayer> Players;
        private GPGTextArea tbResults;

        public DlgTournamentResults(int tournamentID)
        {
            WaitCallback callBack = null;
            this.MaxRound = -1;
            this.Players = new List<TournamentPlayer>();
            this.components = null;
            this.InitializeComponent();
            this.tbResults.Text = "Select a player to view their tournament results.";
            if (callBack == null)
            {
                callBack = delegate (object o) {
                    TournamentMatchup current;
                    DataList queryData = DataAccess.GetQueryData("Tournament Results Data", new object[] { tournamentID });
                    foreach (DataRecord record in queryData)
                    {
                        int num = (Convert.ToInt32(record["wins"]) + Convert.ToInt32(record["losses"])) + Convert.ToInt32(record["draws"]);
                        if (num > this.MaxRound)
                        {
                            this.MaxRound = num;
                        }
                    }
                    foreach (DataRecord record in queryData)
                    {
                        if (Convert.ToInt32(record["round"]) == this.MaxRound)
                        {
                            TournamentPlayer item = new TournamentPlayer {
                                Wins = Convert.ToInt32(record["wins"]),
                                Losses = Convert.ToInt32(record["losses"]),
                                Draws = Convert.ToInt32(record["draws"]),
                                Seed = Convert.ToInt32(record["seed"]),
                                Name = record["player_name"]
                            };
                            this.Players.Add(item);
                        }
                    }
                    List<TournamentMatchup> list2 = new List<TournamentMatchup>();
                    foreach (DataRecord record in queryData)
                    {
                        int num2 = Convert.ToInt32(record["round"]);
                        if (num2 <= this.MaxRound)
                        {
                            TournamentMatchup matchup2;
                            int num3 = Convert.ToInt32(record["match_id"]);
                            string str = record["player_name"];
                            if (num3 >= 0)
                            {
                                bool flag = true;
                                foreach (DataRecord record2 in queryData)
                                {
                                    int num4 = Convert.ToInt32(record2["round"]);
                                    int num5 = Convert.ToInt32(record2["match_id"]);
                                    string str2 = record2["player_name"];
                                    bool flag2 = true;
                                    foreach (TournamentMatchup matchup in list2)
                                    {
                                        if (matchup.Round == num2)
                                        {
                                            if ((matchup.Name == str) && (matchup.OpponentName == str2))
                                            {
                                                flag2 = false;
                                                flag = false;
                                            }
                                            if ((matchup.OpponentName == str) && (matchup.Name == str2))
                                            {
                                                flag2 = false;
                                                flag = false;
                                            }
                                        }
                                    }
                                    if (flag2 && (((num4 == num2) && (num5 == num3)) && (str2 != str)))
                                    {
                                        flag = false;
                                        matchup2 = new TournamentMatchup {
                                            Round = num2,
                                            Wins = Convert.ToInt32(record["wins"]),
                                            Losses = Convert.ToInt32(record["losses"]),
                                            Draws = Convert.ToInt32(record["draws"]),
                                            OpponentName = str2,
                                            Name = str
                                        };
                                        list2.Add(matchup2);
                                        matchup2 = new TournamentMatchup {
                                            Round = num2,
                                            Wins = Convert.ToInt32(record2["wins"]),
                                            Losses = Convert.ToInt32(record2["losses"]),
                                            Draws = Convert.ToInt32(record2["draws"]),
                                            OpponentName = str,
                                            Name = str2
                                        };
                                        list2.Add(matchup2);
                                    }
                                }
                                if (flag)
                                {
                                    matchup2 = new TournamentMatchup {
                                        Round = num2,
                                        Wins = Convert.ToInt32(record["wins"]),
                                        Losses = Convert.ToInt32(record["losses"]),
                                        Draws = Convert.ToInt32(record["draws"]),
                                        OpponentName = "BYE",
                                        Name = str
                                    };
                                    list2.Add(matchup2);
                                }
                            }
                            else
                            {
                                matchup2 = new TournamentMatchup {
                                    Round = num2,
                                    Wins = Convert.ToInt32(record["wins"]),
                                    Losses = Convert.ToInt32(record["losses"]),
                                    Draws = Convert.ToInt32(record["draws"]),
                                    OpponentName = "BYE",
                                    Name = str
                                };
                                list2.Add(matchup2);
                            }
                        }
                    }
                    this.Players.Sort();
                    int num6 = 1;
                    foreach (TournamentPlayer player in this.Players)
                    {
                        player.Standing = num6;
                        num6++;
                        using (List<TournamentMatchup>.Enumerator enumerator3 = list2.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                if (player.Name == current.Name)
                                {
                                    player.Matchups.Add(current);
                                }
                            }
                        }
                    }
                    foreach (TournamentPlayer player in this.Players)
                    {
                        player.Matchups.Sort();
                        if (player.Matchups.Count != 0)
                        {
                            for (int j = player.Matchups.Count - 1; j > 0; j--)
                            {
                                current = player.Matchups[j];
                                TournamentMatchup matchup4 = player.Matchups[j - 1];
                                if (current.Wins > matchup4.Wins)
                                {
                                    current.Result = "Won";
                                }
                                if (current.Losses > matchup4.Losses)
                                {
                                    current.Result = "Lost";
                                }
                                if (current.Draws > matchup4.Draws)
                                {
                                    current.Result = "Drew";
                                }
                            }
                            TournamentMatchup matchup5 = player.Matchups[0];
                            if (matchup5.Wins > 0)
                            {
                                matchup5.Result = "Won";
                            }
                            if (matchup5.Losses > 0)
                            {
                                matchup5.Result = "Lost";
                            }
                            if (matchup5.Draws > 0)
                            {
                                matchup5.Result = "Drew";
                            }
                        }
                    }
                    this.Invoke((VGen0)delegate {
                        this.lbParticipants.Items.AddRange(this.Players.ToArray());
                    });
                };
            }
            ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.gpgBorderPanel1 = new GPGBorderPanel();
            this.tbResults = new GPGTextArea();
            this.gpgLabel1 = new GPGLabel();
            this.gpgBorderPanel2 = new GPGBorderPanel();
            this.lbParticipants = new ListBox();
            this.lStartTimeCaption = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgBorderPanel1.SuspendLayout();
            this.tbResults.Properties.BeginInit();
            this.gpgBorderPanel2.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x222, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgBorderPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgBorderPanel1.Controls.Add(this.tbResults);
            this.gpgBorderPanel1.GPGBorderStyle = GPGBorderStyle.Web;
            this.gpgBorderPanel1.Location = new Point(0xf5, 0x60);
            this.gpgBorderPanel1.Name = "gpgBorderPanel1";
            this.gpgBorderPanel1.Size = new Size(0x155, 0x1da);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel1, null);
            this.gpgBorderPanel1.TabIndex = 9;
            this.tbResults.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tbResults.BorderColor = Color.White;
            this.tbResults.Location = new Point(3, 3);
            this.tbResults.Name = "tbResults";
            this.tbResults.Properties.Appearance.BackColor = Color.Black;
            this.tbResults.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbResults.Properties.Appearance.ForeColor = Color.White;
            this.tbResults.Properties.Appearance.Options.UseBackColor = true;
            this.tbResults.Properties.Appearance.Options.UseBorderColor = true;
            this.tbResults.Properties.Appearance.Options.UseForeColor = true;
            this.tbResults.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbResults.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbResults.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbResults.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbResults.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbResults.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbResults.Properties.BorderStyle = BorderStyles.NoBorder;
            this.tbResults.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbResults.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbResults.Properties.ReadOnly = true;
            this.tbResults.Size = new Size(0x14f, 450);
            this.tbResults.TabIndex = 0;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0xf2, 80);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x8e, 13);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 0x11;
            this.gpgLabel1.Text = "<LOC>Player Results";
            this.gpgLabel1.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel1.TextStyle = TextStyles.ColoredBold;
            this.gpgBorderPanel2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgBorderPanel2.Controls.Add(this.lbParticipants);
            this.gpgBorderPanel2.GPGBorderStyle = GPGBorderStyle.Rectangle;
            this.gpgBorderPanel2.Location = new Point(0x16, 0x60);
            this.gpgBorderPanel2.Name = "gpgBorderPanel2";
            this.gpgBorderPanel2.Size = new Size(0xd9, 0x1da);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel2, null);
            this.gpgBorderPanel2.TabIndex = 10;
            this.lbParticipants.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lbParticipants.BackColor = Color.FromArgb(40, 40, 40);
            this.lbParticipants.BorderStyle = BorderStyle.None;
            this.lbParticipants.ForeColor = Color.White;
            this.lbParticipants.FormattingEnabled = true;
            this.lbParticipants.Location = new Point(4, 3);
            this.lbParticipants.Name = "lbParticipants";
            this.lbParticipants.Size = new Size(210, 0x1d4);
            base.ttDefault.SetSuperTip(this.lbParticipants, null);
            this.lbParticipants.TabIndex = 0x11;
            this.lbParticipants.SelectedValueChanged += new EventHandler(this.lbParticipants_SelectedValueChanged);
            this.lStartTimeCaption.AutoGrowDirection = GrowDirections.None;
            this.lStartTimeCaption.AutoSize = true;
            this.lStartTimeCaption.AutoStyle = true;
            this.lStartTimeCaption.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lStartTimeCaption.ForeColor = Color.FromArgb(0xff, 0xcb, 0);
            this.lStartTimeCaption.IgnoreMouseWheel = false;
            this.lStartTimeCaption.IsStyled = false;
            this.lStartTimeCaption.Location = new Point(0x17, 80);
            this.lStartTimeCaption.Name = "lStartTimeCaption";
            this.lStartTimeCaption.Size = new Size(0x71, 13);
            base.ttDefault.SetSuperTip(this.lStartTimeCaption, null);
            this.lStartTimeCaption.TabIndex = 0x13;
            this.lStartTimeCaption.Text = "<LOC>Standings";
            this.lStartTimeCaption.TextAlign = ContentAlignment.TopRight;
            this.lStartTimeCaption.TextStyle = TextStyles.ColoredBold;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x25d, 640);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.lStartTimeCaption);
            base.Controls.Add(this.gpgBorderPanel2);
            base.Controls.Add(this.gpgBorderPanel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(300, 300);
            base.Name = "DlgTournamentResults";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Tournament Results";
            base.Controls.SetChildIndex(this.gpgBorderPanel1, 0);
            base.Controls.SetChildIndex(this.gpgBorderPanel2, 0);
            base.Controls.SetChildIndex(this.lStartTimeCaption, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgBorderPanel1.ResumeLayout(false);
            this.tbResults.Properties.EndInit();
            this.gpgBorderPanel2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lbParticipants_SelectedValueChanged(object sender, EventArgs e)
        {
            TournamentPlayer selectedItem = this.lbParticipants.SelectedItem as TournamentPlayer;
            if (selectedItem != null)
            {
                this.tbResults.Text = selectedItem.ToString() + "\r\n";
                this.tbResults.Text = this.tbResults.Text + "Original Seed: " + selectedItem.Seed.ToString() + "\r\n\r\n";
                foreach (TournamentMatchup matchup in selectedItem.Matchups)
                {
                    this.tbResults.Text = this.tbResults.Text + "Round " + matchup.Round.ToString() + "\r\n";
                    this.tbResults.Text = this.tbResults.Text + matchup.ToString() + "\r\n\r\n";
                }
            }
            else
            {
                this.tbResults.Text = "Select a player to view their tournament results.";
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }
    }
}

