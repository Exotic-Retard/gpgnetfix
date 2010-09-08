namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using DevExpress.Utils;
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgBrackets : DlgBase
    {
        private SkinLabel backLabelTitle;
        private IContainer components = null;
        private GPGBracketGrid gpgBracketGrid1;
        private GPGPanel linkRounds;
        private int mMaxRound = 0;
        private SkinButton skinButtonEnd;
        private SkinButton skinButtonLast;
        private SkinButton skinButtonNext;
        private SkinButton skinButtonStart;

        public DlgBrackets()
        {
            this.InitializeComponent();
            this.SetRoundLinks(0);
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
            this.gpgBracketGrid1 = new GPGBracketGrid();
            this.linkRounds = new GPGPanel();
            this.skinButtonEnd = new SkinButton();
            this.skinButtonStart = new SkinButton();
            this.skinButtonNext = new SkinButton();
            this.skinButtonLast = new SkinButton();
            this.backLabelTitle = new SkinLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x32b, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgBracketGrid1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgBracketGrid1.CurrentRound = 0;
            this.gpgBracketGrid1.Location = new Point(0x18, 110);
            this.gpgBracketGrid1.Name = "gpgBracketGrid1";
            this.gpgBracketGrid1.Size = new Size(0x33b, 0x150);
            base.ttDefault.SetSuperTip(this.gpgBracketGrid1, null);
            this.gpgBracketGrid1.TabIndex = 7;
            this.gpgBracketGrid1.Text = "gpgBracketGrid1";
            this.linkRounds.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.linkRounds.BackColor = Color.DarkGray;
            this.linkRounds.Location = new Point(0x67, 0x1c1);
            this.linkRounds.Margin = new Padding(0);
            this.linkRounds.Name = "linkRounds";
            this.linkRounds.Size = new Size(0x29d, 0x16);
            base.ttDefault.SetSuperTip(this.linkRounds, null);
            this.linkRounds.TabIndex = 0x17;
            this.skinButtonEnd.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonEnd.AutoStyle = true;
            this.skinButtonEnd.BackColor = Color.Black;
            this.skinButtonEnd.ButtonState = 0;
            this.skinButtonEnd.DialogResult = DialogResult.OK;
            this.skinButtonEnd.DisabledForecolor = Color.Gray;
            this.skinButtonEnd.DrawColor = Color.White;
            this.skinButtonEnd.DrawEdges = false;
            this.skinButtonEnd.FocusColor = Color.Yellow;
            this.skinButtonEnd.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonEnd.ForeColor = Color.White;
            this.skinButtonEnd.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonEnd.IsStyled = true;
            this.skinButtonEnd.Location = new Point(0x32c, 0x1c1);
            this.skinButtonEnd.Name = "skinButtonEnd";
            this.skinButtonEnd.Size = new Size(40, 0x16);
            this.skinButtonEnd.SkinBasePath = @"Controls\Button\Last";
            base.ttDefault.SetSuperTip(this.skinButtonEnd, null);
            this.skinButtonEnd.TabIndex = 0x16;
            this.skinButtonEnd.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonEnd.TextPadding = new Padding(0);
            this.skinButtonEnd.Click += new EventHandler(this.skinButtonEnd_Click);
            this.skinButtonStart.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonStart.AutoStyle = true;
            this.skinButtonStart.BackColor = Color.Black;
            this.skinButtonStart.ButtonState = 0;
            this.skinButtonStart.DialogResult = DialogResult.OK;
            this.skinButtonStart.DisabledForecolor = Color.Gray;
            this.skinButtonStart.DrawColor = Color.White;
            this.skinButtonStart.DrawEdges = false;
            this.skinButtonStart.FocusColor = Color.Yellow;
            this.skinButtonStart.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonStart.ForeColor = Color.White;
            this.skinButtonStart.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonStart.IsStyled = true;
            this.skinButtonStart.Location = new Point(0x18, 0x1c1);
            this.skinButtonStart.Name = "skinButtonStart";
            this.skinButtonStart.Size = new Size(40, 0x16);
            this.skinButtonStart.SkinBasePath = @"Controls\Button\First";
            base.ttDefault.SetSuperTip(this.skinButtonStart, null);
            this.skinButtonStart.TabIndex = 0x15;
            this.skinButtonStart.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonStart.TextPadding = new Padding(0);
            this.skinButtonStart.Click += new EventHandler(this.skinButtonStart_Click);
            this.skinButtonNext.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonNext.AutoStyle = true;
            this.skinButtonNext.BackColor = Color.Black;
            this.skinButtonNext.ButtonState = 0;
            this.skinButtonNext.DialogResult = DialogResult.OK;
            this.skinButtonNext.DisabledForecolor = Color.Gray;
            this.skinButtonNext.DrawColor = Color.White;
            this.skinButtonNext.DrawEdges = false;
            this.skinButtonNext.FocusColor = Color.Yellow;
            this.skinButtonNext.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNext.ForeColor = Color.White;
            this.skinButtonNext.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNext.IsStyled = true;
            this.skinButtonNext.Location = new Point(0x304, 0x1c1);
            this.skinButtonNext.Name = "skinButtonNext";
            this.skinButtonNext.Size = new Size(40, 0x16);
            this.skinButtonNext.SkinBasePath = @"Controls\Button\Next";
            base.ttDefault.SetSuperTip(this.skinButtonNext, null);
            this.skinButtonNext.TabIndex = 20;
            this.skinButtonNext.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNext.TextPadding = new Padding(0);
            this.skinButtonNext.Click += new EventHandler(this.skinButtonNext_Click);
            this.skinButtonLast.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonLast.AutoStyle = true;
            this.skinButtonLast.BackColor = Color.Black;
            this.skinButtonLast.ButtonState = 0;
            this.skinButtonLast.DialogResult = DialogResult.OK;
            this.skinButtonLast.DisabledForecolor = Color.Gray;
            this.skinButtonLast.DrawColor = Color.White;
            this.skinButtonLast.DrawEdges = false;
            this.skinButtonLast.FocusColor = Color.Yellow;
            this.skinButtonLast.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLast.ForeColor = Color.White;
            this.skinButtonLast.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonLast.IsStyled = true;
            this.skinButtonLast.Location = new Point(0x40, 0x1c1);
            this.skinButtonLast.Name = "skinButtonLast";
            this.skinButtonLast.Size = new Size(40, 0x16);
            this.skinButtonLast.SkinBasePath = @"Controls\Button\Previous";
            base.ttDefault.SetSuperTip(this.skinButtonLast, null);
            this.skinButtonLast.TabIndex = 0x13;
            this.skinButtonLast.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLast.TextPadding = new Padding(0);
            this.skinButtonLast.Click += new EventHandler(this.skinButtonLast_Click);
            this.backLabelTitle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.backLabelTitle.AutoStyle = false;
            this.backLabelTitle.BackColor = Color.Transparent;
            this.backLabelTitle.DrawEdges = true;
            this.backLabelTitle.Font = new Font("Arial", 20f, FontStyle.Bold);
            this.backLabelTitle.ForeColor = Color.White;
            this.backLabelTitle.HorizontalScalingMode = ScalingModes.Tile;
            this.backLabelTitle.IsStyled = false;
            this.backLabelTitle.Location = new Point(7, 0x3e);
            this.backLabelTitle.Name = "backLabelTitle";
            this.backLabelTitle.Size = new Size(0x358, 0x2a);
            this.backLabelTitle.SkinBasePath = @"Controls\Background Label\Brackets";
            base.ttDefault.SetSuperTip(this.backLabelTitle, null);
            this.backLabelTitle.TabIndex = 0x18;
            this.backLabelTitle.Text = "<LOC>Tournament Results";
            this.backLabelTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.backLabelTitle.TextPadding = new Padding(10, 0, 0, 0);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(870, 0x219);
            base.Controls.Add(this.backLabelTitle);
            base.Controls.Add(this.linkRounds);
            base.Controls.Add(this.skinButtonEnd);
            base.Controls.Add(this.skinButtonStart);
            base.Controls.Add(this.skinButtonNext);
            base.Controls.Add(this.skinButtonLast);
            base.Controls.Add(this.gpgBracketGrid1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgBrackets";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Tournament Brackets";
            base.Controls.SetChildIndex(this.gpgBracketGrid1, 0);
            base.Controls.SetChildIndex(this.skinButtonLast, 0);
            base.Controls.SetChildIndex(this.skinButtonNext, 0);
            base.Controls.SetChildIndex(this.skinButtonStart, 0);
            base.Controls.SetChildIndex(this.skinButtonEnd, 0);
            base.Controls.SetChildIndex(this.linkRounds, 0);
            base.Controls.SetChildIndex(this.backLabelTitle, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void link_Click(object sender, EventArgs e)
        {
            SkinLabel label = sender as SkinLabel;
            if (label.Tag != null)
            {
                int num = Convert.ToInt32(label.Tag);
                this.gpgBracketGrid1.CurrentRound = num;
                this.SetRoundLinks(this.mMaxRound);
            }
        }

        public void LoadTournament(int id)
        {
            base.SetStatus(Loc.Get("<LOC>Loading bracket data..."), new object[0]);
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                int num;
                string str;
                this.mMaxRound = 0;
                int num3 = Convert.ToInt32((o as object[])[0]);
                DataList queryData = DataAccess.GetQueryData("Tournament Round", new object[] { num3 });
                this.gpgBracketGrid1.Matchups.Clear();
                int count = 0;
                List<string> list2 = new List<string>();
                foreach (DataRecord record in queryData)
                {
                    num = Convert.ToInt32(record["match_id"]);
                    int num5 = Convert.ToInt32(record["round"]);
                    str = num5.ToString() + " " + num.ToString();
                    if (!list2.Contains(str))
                    {
                        list2.Add(str);
                    }
                    if (num5 > this.mMaxRound)
                    {
                        this.mMaxRound = num5;
                    }
                }
                count = list2.Count;
                this.SetRoundLinks(this.mMaxRound);
                int num2 = 0;
                while (num2 <= count)
                {
                    this.gpgBracketGrid1.Matchups.Add(new GPGMatchup());
                    num2++;
                }
                foreach (DataRecord record in queryData)
                {
                    VGen0 method = null;
                    GPGDragItem ditem = new GPGDragItem {
                        Label = record["name"],
                        Wins = Convert.ToInt32(record["wins"]),
                        Losses = Convert.ToInt32(record["losses"]),
                        Draws = Convert.ToInt32(record["draws"])
                    };
                    try
                    {
                        ditem.Seed = Convert.ToInt32(record["seed"]);
                    }
                    catch
                    {
                    }
                    num = Convert.ToInt32(record["match_id"]);
                    int roundid = Convert.ToInt32(record["round"]);
                    str = roundid.ToString() + " " + num.ToString();
                    int index = -1;
                    for (num2 = 0; num2 < list2.Count; num2++)
                    {
                        if (list2[num2] == str)
                        {
                            index = num2;
                        }
                    }
                    if (index >= 0)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                if (this.gpgBracketGrid1.Matchups[index].Player1 == null)
                                {
                                    this.gpgBracketGrid1.Matchups[index].Player1 = ditem;
                                }
                                else
                                {
                                    this.gpgBracketGrid1.Matchups[index].Player2 = ditem;
                                }
                                this.gpgBracketGrid1.Matchups[index].Round = roundid;
                            };
                        }
                        base.Invoke(method);
                    }
                }
                base.Invoke((VGen0)delegate {
                    this.gpgBracketGrid1.SetupRounds();
                    this.gpgBracketGrid1.Invalidate();
                    base.SetStatus(Loc.Get("<LOC>Viewing tournament results."), new object[0]);
                });
            }, new object[] { id });
        }

        private void SetRoundLinks(int numrounds)
        {
            base.Invoke((VGen1)delegate (object o) {
                SkinLabel label;
                int num = Convert.ToInt32(o);
                this.mMaxRound = num;
                this.linkRounds.SuspendLayout();
                this.linkRounds.Controls.Clear();
                int width = 100;
                int x = 0;
                for (int j = 1; j <= num; j++)
                {
                    label = new SkinLabel {
                        SkinBasePath = @"Controls\BackgroundLabel\BlackBar",
                        DrawEdges = false,
                        Size = new Size(width, this.linkRounds.Height),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Location = new Point(x, 0),
                        Tag = j,
                        Text = "Round " + j.ToString()
                    };
                    label.Click += new EventHandler(this.link_Click);
                    if (j != this.gpgBracketGrid1.CurrentRound)
                    {
                        label.ForeColor = Color.FromArgb(0x7a, 0x7a, 0xf4);
                        label.Font = new Font("Verdana", 10f, FontStyle.Underline);
                    }
                    else
                    {
                        label.Font = new Font("Verdana", 10f);
                    }
                    x += width;
                    this.linkRounds.Controls.Add(label);
                }
                label = new SkinLabel {
                    SkinBasePath = @"Controls\BackgroundLabel\BlackBar",
                    DrawEdges = false,
                    Size = new Size(0x640, this.linkRounds.Height),
                    Location = new Point(x, 0)
                };
                this.linkRounds.Controls.Add(label);
                this.linkRounds.ResumeLayout();
            }, new object[] { numrounds });
        }

        private void skinButtonEnd_Click(object sender, EventArgs e)
        {
            this.gpgBracketGrid1.CurrentRound = this.mMaxRound;
            this.SetRoundLinks(this.mMaxRound);
        }

        private void skinButtonLast_Click(object sender, EventArgs e)
        {
            this.gpgBracketGrid1.CurrentRound--;
            this.SetRoundLinks(this.mMaxRound);
        }

        private void skinButtonNext_Click(object sender, EventArgs e)
        {
            this.gpgBracketGrid1.CurrentRound++;
            this.SetRoundLinks(this.mMaxRound);
        }

        private void skinButtonStart_Click(object sender, EventArgs e)
        {
            this.gpgBracketGrid1.CurrentRound = 1;
            this.SetRoundLinks(this.mMaxRound);
        }
    }
}

