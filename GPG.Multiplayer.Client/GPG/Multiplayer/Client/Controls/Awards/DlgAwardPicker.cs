namespace GPG.Multiplayer.Client.Controls.Awards
{
    using DevExpress.Utils;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Statistics;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgAwardPicker : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGPanel gpgPanelAwards;
        private bool IsInitialized = false;
        private User mPlayer;
        private Award mSelectedAward = null;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonClear;

        public DlgAwardPicker(User player)
        {
            this.InitializeComponent();
            this.mPlayer = player;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DoLayout()
        {
            this.gpgPanelAwards.Controls.Clear();
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                try
                {
                    Dictionary<AwardSet, Award> dictionary = new Dictionary<AwardSet, Award>(AwardSet.AllAwardSets.Count);
                    foreach (AwardSet set in AwardSet.AllAwardSets.Values)
                    {
                        dictionary.Add(set, null);
                    }
                    foreach (PlayerAward award in PlayerAward.GetPlayerAwards(this.Player.Name))
                    {
                        if ((award.IsAchieved && (this.Player.Award1 != award.AwardID)) && (((this.Player.Award2 != award.AwardID) && (this.Player.Award3 != award.AwardID)) && ((dictionary[award.Award.AwardSet] == null) || (dictionary[award.Award.AwardSet].AwardDegree < award.Award.AwardDegree))))
                        {
                            dictionary[award.Award.AwardSet] = award.Award;
                        }
                    }
                    int left = 0;
                    int top = 0;
                    using (Dictionary<AwardSet, Award>.Enumerator enumerator2 = dictionary.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            KeyValuePair<AwardSet, Award> award = enumerator2.Current;
                            if (award.Value != null)
                            {
                                Image awardImage = award.Value.LargeImage;
                                base.Invoke(delegate {
                                    PictureBox box = new PictureBox {
                                        Cursor = Cursors.Hand,
                                        Size = new Size(0x30, 0x30),
                                        SizeMode = PictureBoxSizeMode.AutoSize,
                                        Image = awardImage,
                                        Left = left,
                                        Top = top,
                                        Tag = award
                                    };
                                    this.gpgPanelAwards.Controls.Add(box);
                                    box.Click += delegate (object s1, EventArgs e1) {
                                        PictureBox box = s1 as PictureBox;
                                        KeyValuePair<AwardSet, Award> tag = (KeyValuePair<AwardSet, Award>) (s1 as Control).Tag;
                                        this.mSelectedAward = tag.Value;
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    };
                                    box.MouseEnter += delegate (object s1, EventArgs e1) {
                                        int awardDegree;
                                        PictureBox box = s1 as PictureBox;
                                        KeyValuePair<AwardSet, Award> tag = (KeyValuePair<AwardSet, Award>) (s1 as Control).Tag;
                                        if (tag.Value == null)
                                        {
                                            awardDegree = 0;
                                        }
                                        else
                                        {
                                            awardDegree = tag.Value.AwardDegree;
                                        }
                                        DlgAwardSetDetails.Singleton.BindToAward(tag.Key, awardDegree);
                                        Point point = this.gpgPanelAwards.PointToScreen(box.Location);
                                        point.Offset(0, -(DlgAwardSetDetails.Singleton.Height + 4));
                                        DlgAwardSetDetails.Singleton.Location = point;
                                        DlgAwardSetDetails.Singleton.Show();
                                    };
                                    box.MouseLeave += delegate (object s1, EventArgs e1) {
                                        DlgAwardSetDetails.Singleton.Hide();
                                    };
                                    left += box.Width + 4;
                                    if ((left + box.Width) > this.gpgPanelAwards.Width)
                                    {
                                        left = 0;
                                        top += box.Height + 4;
                                    }
                                });
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        private void InitializeComponent()
        {
            this.skinButtonCancel = new SkinButton();
            this.gpgPanelAwards = new GPGPanel();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonClear = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(400, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Transparent;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x142, 0x16f);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x7d, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 7;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgPanelAwards.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelAwards.AutoScroll = true;
            this.gpgPanelAwards.BackColor = Color.Transparent;
            this.gpgPanelAwards.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelAwards.BorderThickness = 2;
            this.gpgPanelAwards.DrawBorder = false;
            this.gpgPanelAwards.Location = new Point(12, 0x67);
            this.gpgPanelAwards.Name = "gpgPanelAwards";
            this.gpgPanelAwards.Size = new Size(0x1b3, 0x102);
            base.ttDefault.SetSuperTip(this.gpgPanelAwards, null);
            this.gpgPanelAwards.TabIndex = 8;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x54);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x94, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 9;
            this.gpgLabel1.Text = "<LOC>Available Awards";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.skinButtonClear.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonClear.AutoStyle = true;
            this.skinButtonClear.BackColor = Color.Transparent;
            this.skinButtonClear.ButtonState = 0;
            this.skinButtonClear.DialogResult = DialogResult.OK;
            this.skinButtonClear.DisabledForecolor = Color.Gray;
            this.skinButtonClear.DrawColor = Color.White;
            this.skinButtonClear.DrawEdges = true;
            this.skinButtonClear.FocusColor = Color.Yellow;
            this.skinButtonClear.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClear.ForeColor = Color.White;
            this.skinButtonClear.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClear.IsStyled = true;
            this.skinButtonClear.Location = new Point(0xa3, 0x16f);
            this.skinButtonClear.Name = "skinButtonClear";
            this.skinButtonClear.Size = new Size(0x99, 0x1a);
            this.skinButtonClear.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClear, null);
            this.skinButtonClear.TabIndex = 8;
            this.skinButtonClear.TabStop = true;
            this.skinButtonClear.Text = "<LOC>Clear Award";
            this.skinButtonClear.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClear.TextPadding = new Padding(0);
            this.skinButtonClear.Click += new EventHandler(this.skinButtonClear_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1cb, 0x1c8);
            base.Controls.Add(this.skinButtonClear);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgPanelAwards);
            base.Controls.Add(this.skinButtonCancel);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x135, 0x113);
            base.Name = "DlgAwardPicker";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Select An Award";
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgPanelAwards, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonClear, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoLayout();
            this.IsInitialized = true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.IsInitialized)
            {
                this.DoLayout();
            }
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonClear_Click(object sender, EventArgs e)
        {
            this.mSelectedAward = null;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        public User Player
        {
            get
            {
                return this.mPlayer;
            }
        }

        public Award SelectedAward
        {
            get
            {
                return this.mSelectedAward;
            }
        }
    }
}

