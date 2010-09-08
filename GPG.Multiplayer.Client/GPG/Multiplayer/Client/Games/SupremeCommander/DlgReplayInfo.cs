namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
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

    public class DlgReplayInfo : DlgBase
    {
        private static Dictionary<string, DlgReplayInfo> ActiveWindows = new Dictionary<string, DlgReplayInfo>();
        private int CommentCount;
        private IContainer components;
        private int DeletedComment;
        private GPGGroupBox gpgGroupBoxComments;
        private GPGGroupBox gpgGroupBoxRate;
        private GPGLabel gpgLabel;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabelDate;
        private GPGLabel gpgLabelDescription;
        private GPGLabel gpgLabelDownloads;
        private GPGLabel gpgLabelMap;
        private GPGLabel gpgLabelNoComments;
        private GPGLabel gpgLabelOpponent;
        private GPGLabel gpgLabelPlayerName;
        private GPGLabel gpgLabelRating;
        private GPGLabel gpgLabelRatingCount;
        private GPGPanel gpgPanelActions;
        private GPGPanel gpgPanelComments;
        private GPGPictureBox gpgPictureBoxStars;
        private GPGRadioButton gpgRadioButton1;
        private GPGRadioButton gpgRadioButton10;
        private GPGRadioButton gpgRadioButton2;
        private GPGRadioButton gpgRadioButton3;
        private GPGRadioButton gpgRadioButton4;
        private GPGRadioButton gpgRadioButton5;
        private GPGRadioButton gpgRadioButton6;
        private GPGRadioButton gpgRadioButton7;
        private GPGRadioButton gpgRadioButton8;
        private GPGRadioButton gpgRadioButton9;
        private GPGTextBox gpgTextBoxLink;
        private ReplayInfo mReplay;
        private static Dictionary<int, int> PlayerRatings = null;
        private SkinButton skinButtonCopyLink;
        private SkinButton skinButtonDownload;
        private SkinButton skinButtonPostComment;
        private SkinButton skinButtonSubmitRating;
        private SkinLabel skinLabelTitle;

        static DlgReplayInfo()
        {
            DataList list = DataAccess.GetQueryDataSet("GetPlayerReplayRatings", false, new object[0]);
            PlayerRatings = new Dictionary<int, int>(list.Count);
            foreach (DataRecord record in list)
            {
                PlayerRatings.Add(int.Parse(record["replay_id"]), int.Parse(record["rating"]));
            }
        }

        public DlgReplayInfo()
        {
            this.CommentCount = -1;
            this.DeletedComment = -1;
            this.components = null;
            this.InitializeComponent();
        }

        public DlgReplayInfo(ReplayInfo replay)
        {
            this.CommentCount = -1;
            this.DeletedComment = -1;
            this.components = null;
            this.InitializeComponent();
            this.mReplay = replay;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgReplayInfo_Load(object sender, EventArgs e)
        {
            this.UpdateRatingSection();
            this.UpdateRatingStars();
            this.UpdateComments();
            this.gpgLabelDate.Text = this.Replay.CreateDate;
            this.gpgLabelMap.Text = this.Replay.MapName;
            this.gpgLabelOpponent.Text = this.Replay.Opponent;
            base.ttDefault.SetToolTip(this.gpgLabelOpponent, this.Replay.Opponent);
            this.gpgLabelPlayerName.Text = this.Replay.PlayerName;
            this.gpgLabelDownloads.Text = this.Replay.Downloads.ToString();
            if ((this.Replay.GameInfo != null) && (this.Replay.GameInfo.Length > 0))
            {
                this.gpgLabelDescription.Text = this.Replay.GameInfo;
            }
            else
            {
                this.gpgLabelDescription.Text = Loc.Get("<LOC>No Description");
            }
            this.skinLabelTitle.Text = this.Replay.Title;
            this.gpgTextBoxLink.Text = string.Format("replay:{0}", this.Replay.Location);
            this.gpgTextBoxLink.SelectAll();
            this.Replay.DownloadsChanged += new EventHandler(this.Replay_DownloadsChanged);
        }

        private void gpgPanelComments_SizeChanged(object sender, EventArgs e)
        {
            this.LayoutComments();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgReplayInfo));
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelPlayerName = new GPGLabel();
            this.gpgTextBoxLink = new GPGTextBox();
            this.gpgLabel = new GPGLabel();
            this.gpgLabelOpponent = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.skinLabelTitle = new SkinLabel();
            this.gpgPictureBoxStars = new GPGPictureBox();
            this.gpgLabelMap = new GPGLabel();
            this.gpgLabelDate = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.skinButtonDownload = new SkinButton();
            this.skinButtonCopyLink = new SkinButton();
            this.gpgGroupBoxRate = new GPGGroupBox();
            this.skinButtonSubmitRating = new SkinButton();
            this.gpgRadioButton10 = new GPGRadioButton();
            this.gpgRadioButton9 = new GPGRadioButton();
            this.gpgRadioButton8 = new GPGRadioButton();
            this.gpgRadioButton7 = new GPGRadioButton();
            this.gpgRadioButton6 = new GPGRadioButton();
            this.gpgRadioButton5 = new GPGRadioButton();
            this.gpgRadioButton4 = new GPGRadioButton();
            this.gpgRadioButton3 = new GPGRadioButton();
            this.gpgRadioButton2 = new GPGRadioButton();
            this.gpgRadioButton1 = new GPGRadioButton();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelDescription = new GPGLabel();
            this.gpgPanelActions = new GPGPanel();
            this.gpgLabelRating = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabelDownloads = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabelRatingCount = new GPGLabel();
            this.gpgGroupBoxComments = new GPGGroupBox();
            this.skinButtonPostComment = new SkinButton();
            this.gpgPanelComments = new GPGPanel();
            this.gpgLabelNoComments = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxLink.Properties.BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxStars).BeginInit();
            this.gpgGroupBoxRate.SuspendLayout();
            this.gpgPanelActions.SuspendLayout();
            this.gpgGroupBoxComments.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x21f, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x9c);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x57, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>Player";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.gpgLabelPlayerName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelPlayerName.AutoSize = true;
            this.gpgLabelPlayerName.AutoStyle = true;
            this.gpgLabelPlayerName.Font = new Font("Arial", 9.75f);
            this.gpgLabelPlayerName.ForeColor = Color.ForestGreen;
            this.gpgLabelPlayerName.IgnoreMouseWheel = false;
            this.gpgLabelPlayerName.IsStyled = false;
            this.gpgLabelPlayerName.Location = new Point(12, 0xac);
            this.gpgLabelPlayerName.Name = "gpgLabelPlayerName";
            this.gpgLabelPlayerName.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelPlayerName, null);
            this.gpgLabelPlayerName.TabIndex = 8;
            this.gpgLabelPlayerName.Text = "gpgLabel2";
            this.gpgLabelPlayerName.TextStyle = TextStyles.ColoredBold;
            this.gpgTextBoxLink.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextBoxLink.Location = new Point(0, 0x16);
            this.gpgTextBoxLink.Name = "gpgTextBoxLink";
            this.gpgTextBoxLink.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxLink.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxLink.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxLink.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxLink.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxLink.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxLink.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxLink.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxLink.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxLink.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxLink.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxLink.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxLink.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxLink.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxLink.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxLink.Properties.ReadOnly = true;
            this.gpgTextBoxLink.Size = new Size(0x15b, 20);
            this.gpgTextBoxLink.TabIndex = 9;
            this.gpgLabel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel.AutoSize = true;
            this.gpgLabel.AutoStyle = true;
            this.gpgLabel.Font = new Font("Arial", 9.75f);
            this.gpgLabel.ForeColor = Color.White;
            this.gpgLabel.IgnoreMouseWheel = false;
            this.gpgLabel.IsStyled = false;
            this.gpgLabel.Location = new Point(-3, 3);
            this.gpgLabel.Name = "gpgLabel";
            this.gpgLabel.Size = new Size(0x69, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel, null);
            this.gpgLabel.TabIndex = 10;
            this.gpgLabel.Text = "<LOC>Chat Link";
            this.gpgLabel.TextStyle = TextStyles.Default;
            this.gpgLabelOpponent.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelOpponent.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelOpponent.AutoStyle = true;
            this.gpgLabelOpponent.Font = new Font("Arial", 9.75f);
            this.gpgLabelOpponent.ForeColor = Color.Crimson;
            this.gpgLabelOpponent.IgnoreMouseWheel = false;
            this.gpgLabelOpponent.IsStyled = false;
            this.gpgLabelOpponent.Location = new Point(12, 0xd4);
            this.gpgLabelOpponent.Name = "gpgLabelOpponent";
            this.gpgLabelOpponent.Size = new Size(0x132, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelOpponent, null);
            this.gpgLabelOpponent.TabIndex = 12;
            this.gpgLabelOpponent.Text = "gpgLabel2";
            this.gpgLabelOpponent.TextStyle = TextStyles.ColoredBold;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(12, 0xc4);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(130, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 11;
            this.gpgLabel4.Text = "<LOC>Other Players";
            this.gpgLabel4.TextStyle = TextStyles.Bold;
            this.skinLabelTitle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabelTitle.AutoStyle = false;
            this.skinLabelTitle.BackColor = Color.Transparent;
            this.skinLabelTitle.DrawEdges = true;
            this.skinLabelTitle.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelTitle.ForeColor = Color.White;
            this.skinLabelTitle.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelTitle.IsStyled = false;
            this.skinLabelTitle.Location = new Point(6, 0x53);
            this.skinLabelTitle.Name = "skinLabelTitle";
            this.skinLabelTitle.Size = new Size(590, 20);
            this.skinLabelTitle.SkinBasePath = @"Controls\Background Label\Rectangle";
            base.ttDefault.SetSuperTip(this.skinLabelTitle, null);
            this.skinLabelTitle.TabIndex = 13;
            this.skinLabelTitle.Text = "Title";
            this.skinLabelTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.skinLabelTitle.TextPadding = new Padding(0);
            this.gpgPictureBoxStars.Image = (Image) manager.GetObject("gpgPictureBoxStars.Image");
            this.gpgPictureBoxStars.Location = new Point(15, 0x84);
            this.gpgPictureBoxStars.Name = "gpgPictureBoxStars";
            this.gpgPictureBoxStars.Size = new Size(0x6f, 0x17);
            base.ttDefault.SetSuperTip(this.gpgPictureBoxStars, null);
            this.gpgPictureBoxStars.TabIndex = 0x1b;
            this.gpgPictureBoxStars.TabStop = false;
            this.gpgLabelMap.Anchor = AnchorStyles.Top;
            this.gpgLabelMap.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMap.AutoSize = true;
            this.gpgLabelMap.AutoStyle = true;
            this.gpgLabelMap.Font = new Font("Arial", 9.75f);
            this.gpgLabelMap.ForeColor = Color.White;
            this.gpgLabelMap.IgnoreMouseWheel = false;
            this.gpgLabelMap.IsStyled = false;
            this.gpgLabelMap.Location = new Point(0x137, 0xac);
            this.gpgLabelMap.Name = "gpgLabelMap";
            this.gpgLabelMap.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelMap, null);
            this.gpgLabelMap.TabIndex = 15;
            this.gpgLabelMap.Text = "gpgLabel2";
            this.gpgLabelMap.TextStyle = TextStyles.Default;
            this.gpgLabelDate.Anchor = AnchorStyles.Top;
            this.gpgLabelDate.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDate.AutoSize = true;
            this.gpgLabelDate.AutoStyle = true;
            this.gpgLabelDate.Font = new Font("Arial", 9.75f);
            this.gpgLabelDate.ForeColor = Color.White;
            this.gpgLabelDate.IgnoreMouseWheel = false;
            this.gpgLabelDate.IsStyled = false;
            this.gpgLabelDate.Location = new Point(0x137, 0x84);
            this.gpgLabelDate.Name = "gpgLabelDate";
            this.gpgLabelDate.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelDate, null);
            this.gpgLabelDate.TabIndex = 0x11;
            this.gpgLabelDate.Text = "gpgLabel2";
            this.gpgLabelDate.TextStyle = TextStyles.Default;
            this.gpgLabel6.Anchor = AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x137, 0x74);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x4d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x10;
            this.gpgLabel6.Text = "<LOC>Date";
            this.gpgLabel6.TextStyle = TextStyles.Bold;
            this.skinButtonDownload.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonDownload.AutoStyle = true;
            this.skinButtonDownload.BackColor = Color.Black;
            this.skinButtonDownload.ButtonState = 0;
            this.skinButtonDownload.DialogResult = DialogResult.OK;
            this.skinButtonDownload.DisabledForecolor = Color.Gray;
            this.skinButtonDownload.DrawColor = Color.White;
            this.skinButtonDownload.DrawEdges = true;
            this.skinButtonDownload.FocusColor = Color.Yellow;
            this.skinButtonDownload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDownload.ForeColor = Color.White;
            this.skinButtonDownload.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDownload.IsStyled = true;
            this.skinButtonDownload.Location = new Point(0x1b1, 0x15);
            this.skinButtonDownload.Name = "skinButtonDownload";
            this.skinButtonDownload.Size = new Size(0x88, 0x15);
            this.skinButtonDownload.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonDownload, null);
            this.skinButtonDownload.TabIndex = 20;
            this.skinButtonDownload.TabStop = true;
            this.skinButtonDownload.Text = "<LOC>Download";
            this.skinButtonDownload.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDownload.TextPadding = new Padding(0);
            this.skinButtonDownload.Click += new EventHandler(this.skinButtonDownload_Click);
            this.skinButtonCopyLink.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCopyLink.AutoStyle = true;
            this.skinButtonCopyLink.BackColor = Color.Black;
            this.skinButtonCopyLink.ButtonState = 0;
            this.skinButtonCopyLink.DialogResult = DialogResult.OK;
            this.skinButtonCopyLink.DisabledForecolor = Color.Gray;
            this.skinButtonCopyLink.DrawColor = Color.White;
            this.skinButtonCopyLink.DrawEdges = true;
            this.skinButtonCopyLink.FocusColor = Color.Yellow;
            this.skinButtonCopyLink.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCopyLink.ForeColor = Color.White;
            this.skinButtonCopyLink.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCopyLink.IsStyled = true;
            this.skinButtonCopyLink.Location = new Point(0x161, 0x15);
            this.skinButtonCopyLink.Name = "skinButtonCopyLink";
            this.skinButtonCopyLink.Size = new Size(0x4a, 0x15);
            this.skinButtonCopyLink.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCopyLink, null);
            this.skinButtonCopyLink.TabIndex = 0x15;
            this.skinButtonCopyLink.TabStop = true;
            this.skinButtonCopyLink.Text = "<LOC>Copy";
            this.skinButtonCopyLink.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCopyLink.TextPadding = new Padding(0);
            this.skinButtonCopyLink.Click += new EventHandler(this.skinButtonCopyLink_Click);
            this.gpgGroupBoxRate.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgGroupBoxRate.Controls.Add(this.skinButtonSubmitRating);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton10);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton9);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton8);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton7);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton6);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton5);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton4);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton3);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton2);
            this.gpgGroupBoxRate.Controls.Add(this.gpgRadioButton1);
            this.gpgGroupBoxRate.Location = new Point(15, 0x1c8);
            this.gpgGroupBoxRate.Name = "gpgGroupBoxRate";
            this.gpgGroupBoxRate.Size = new Size(0x23f, 50);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxRate, null);
            this.gpgGroupBoxRate.TabIndex = 0x16;
            this.gpgGroupBoxRate.TabStop = false;
            this.gpgGroupBoxRate.Text = "<LOC>Rate this replay";
            this.skinButtonSubmitRating.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.skinButtonSubmitRating.AutoStyle = true;
            this.skinButtonSubmitRating.BackColor = Color.Black;
            this.skinButtonSubmitRating.ButtonState = 0;
            this.skinButtonSubmitRating.DialogResult = DialogResult.OK;
            this.skinButtonSubmitRating.DisabledForecolor = Color.Gray;
            this.skinButtonSubmitRating.DrawColor = Color.White;
            this.skinButtonSubmitRating.DrawEdges = true;
            this.skinButtonSubmitRating.FocusColor = Color.Yellow;
            this.skinButtonSubmitRating.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSubmitRating.ForeColor = Color.White;
            this.skinButtonSubmitRating.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSubmitRating.IsStyled = true;
            this.skinButtonSubmitRating.Location = new Point(0x1c8, 0x11);
            this.skinButtonSubmitRating.Name = "skinButtonSubmitRating";
            this.skinButtonSubmitRating.Size = new Size(0x71, 0x15);
            this.skinButtonSubmitRating.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSubmitRating, null);
            this.skinButtonSubmitRating.TabIndex = 10;
            this.skinButtonSubmitRating.TabStop = true;
            this.skinButtonSubmitRating.Text = "<LOC>Submit";
            this.skinButtonSubmitRating.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSubmitRating.TextPadding = new Padding(0);
            this.skinButtonSubmitRating.Click += new EventHandler(this.skinButtonSubmitRating_Click);
            this.gpgRadioButton10.Anchor = AnchorStyles.Top;
            this.gpgRadioButton10.AutoSize = true;
            this.gpgRadioButton10.Location = new Point(0x17d, 20);
            this.gpgRadioButton10.Name = "gpgRadioButton10";
            this.gpgRadioButton10.Size = new Size(0x51, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton10, null);
            this.gpgRadioButton10.TabIndex = 9;
            this.gpgRadioButton10.Text = "<LOC>10";
            this.gpgRadioButton10.UseVisualStyleBackColor = true;
            this.gpgRadioButton9.Anchor = AnchorStyles.Top;
            this.gpgRadioButton9.AutoSize = true;
            this.gpgRadioButton9.Location = new Point(340, 20);
            this.gpgRadioButton9.Name = "gpgRadioButton9";
            this.gpgRadioButton9.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton9, null);
            this.gpgRadioButton9.TabIndex = 8;
            this.gpgRadioButton9.Text = "<LOC>9";
            this.gpgRadioButton9.UseVisualStyleBackColor = true;
            this.gpgRadioButton8.Anchor = AnchorStyles.Top;
            this.gpgRadioButton8.AutoSize = true;
            this.gpgRadioButton8.Location = new Point(0x12d, 20);
            this.gpgRadioButton8.Name = "gpgRadioButton8";
            this.gpgRadioButton8.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton8, null);
            this.gpgRadioButton8.TabIndex = 7;
            this.gpgRadioButton8.Text = "<LOC>8";
            this.gpgRadioButton8.UseVisualStyleBackColor = true;
            this.gpgRadioButton7.Anchor = AnchorStyles.Top;
            this.gpgRadioButton7.AutoSize = true;
            this.gpgRadioButton7.Location = new Point(260, 20);
            this.gpgRadioButton7.Name = "gpgRadioButton7";
            this.gpgRadioButton7.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton7, null);
            this.gpgRadioButton7.TabIndex = 6;
            this.gpgRadioButton7.Text = "<LOC>7";
            this.gpgRadioButton7.UseVisualStyleBackColor = true;
            this.gpgRadioButton6.Anchor = AnchorStyles.Top;
            this.gpgRadioButton6.AutoSize = true;
            this.gpgRadioButton6.Location = new Point(0xdd, 20);
            this.gpgRadioButton6.Name = "gpgRadioButton6";
            this.gpgRadioButton6.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton6, null);
            this.gpgRadioButton6.TabIndex = 5;
            this.gpgRadioButton6.Text = "<LOC>6";
            this.gpgRadioButton6.UseVisualStyleBackColor = true;
            this.gpgRadioButton5.Anchor = AnchorStyles.Top;
            this.gpgRadioButton5.AutoSize = true;
            this.gpgRadioButton5.Checked = true;
            this.gpgRadioButton5.Location = new Point(180, 20);
            this.gpgRadioButton5.Name = "gpgRadioButton5";
            this.gpgRadioButton5.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton5, null);
            this.gpgRadioButton5.TabIndex = 4;
            this.gpgRadioButton5.TabStop = true;
            this.gpgRadioButton5.Text = "<LOC>5";
            this.gpgRadioButton5.UseVisualStyleBackColor = true;
            this.gpgRadioButton4.Anchor = AnchorStyles.Top;
            this.gpgRadioButton4.AutoSize = true;
            this.gpgRadioButton4.Location = new Point(0x8d, 20);
            this.gpgRadioButton4.Name = "gpgRadioButton4";
            this.gpgRadioButton4.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton4, null);
            this.gpgRadioButton4.TabIndex = 3;
            this.gpgRadioButton4.Text = "<LOC>4";
            this.gpgRadioButton4.UseVisualStyleBackColor = true;
            this.gpgRadioButton3.Anchor = AnchorStyles.Top;
            this.gpgRadioButton3.AutoSize = true;
            this.gpgRadioButton3.Location = new Point(100, 20);
            this.gpgRadioButton3.Name = "gpgRadioButton3";
            this.gpgRadioButton3.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton3, null);
            this.gpgRadioButton3.TabIndex = 2;
            this.gpgRadioButton3.Text = "<LOC>3";
            this.gpgRadioButton3.UseVisualStyleBackColor = true;
            this.gpgRadioButton2.Anchor = AnchorStyles.Top;
            this.gpgRadioButton2.AutoSize = true;
            this.gpgRadioButton2.Location = new Point(0x3d, 20);
            this.gpgRadioButton2.Name = "gpgRadioButton2";
            this.gpgRadioButton2.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton2, null);
            this.gpgRadioButton2.TabIndex = 1;
            this.gpgRadioButton2.Text = "<LOC>2";
            this.gpgRadioButton2.UseVisualStyleBackColor = true;
            this.gpgRadioButton1.Anchor = AnchorStyles.Top;
            this.gpgRadioButton1.AutoSize = true;
            this.gpgRadioButton1.Location = new Point(20, 20);
            this.gpgRadioButton1.Name = "gpgRadioButton1";
            this.gpgRadioButton1.Size = new Size(0x4a, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButton1, null);
            this.gpgRadioButton1.TabIndex = 0;
            this.gpgRadioButton1.Text = "<LOC>1";
            this.gpgRadioButton1.UseVisualStyleBackColor = true;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(12, 0xed);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x73, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 0x18;
            this.gpgLabel3.Text = "<LOC>Description";
            this.gpgLabel3.TextStyle = TextStyles.Bold;
            this.gpgLabelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(12, 0xfd);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x242, 0x23);
            base.ttDefault.SetSuperTip(this.gpgLabelDescription, null);
            this.gpgLabelDescription.TabIndex = 0;
            this.gpgLabelDescription.Text = "gpgLabel7";
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            this.gpgPanelActions.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgPanelActions.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelActions.BorderThickness = 2;
            this.gpgPanelActions.Controls.Add(this.gpgLabel);
            this.gpgPanelActions.Controls.Add(this.gpgTextBoxLink);
            this.gpgPanelActions.Controls.Add(this.skinButtonDownload);
            this.gpgPanelActions.Controls.Add(this.skinButtonCopyLink);
            this.gpgPanelActions.DrawBorder = false;
            this.gpgPanelActions.Location = new Point(15, 0x198);
            this.gpgPanelActions.Name = "gpgPanelActions";
            this.gpgPanelActions.Size = new Size(0x23f, 0x2d);
            base.ttDefault.SetSuperTip(this.gpgPanelActions, null);
            this.gpgPanelActions.TabIndex = 0x19;
            this.gpgLabelRating.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelRating.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelRating.AutoSize = true;
            this.gpgLabelRating.AutoStyle = true;
            this.gpgLabelRating.Font = new Font("Arial", 9.75f);
            this.gpgLabelRating.ForeColor = Color.Gold;
            this.gpgLabelRating.IgnoreMouseWheel = false;
            this.gpgLabelRating.IsStyled = false;
            this.gpgLabelRating.Location = new Point(12, 0x1c6);
            this.gpgLabelRating.Name = "gpgLabelRating";
            this.gpgLabelRating.Size = new Size(0x61, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelRating, null);
            this.gpgLabelRating.TabIndex = 0x1a;
            this.gpgLabelRating.Text = "gpgLabelRating";
            this.gpgLabelRating.TextStyle = TextStyles.ColoredBold;
            this.gpgLabelRating.Visible = false;
            this.gpgLabel5.Anchor = AnchorStyles.Top;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x137, 0x9c);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x4b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 14;
            this.gpgLabel5.Text = "<LOC>Map";
            this.gpgLabel5.TextStyle = TextStyles.Bold;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0x74);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x57, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 0x1c;
            this.gpgLabel2.Text = "<LOC>Rating";
            this.gpgLabel2.TextStyle = TextStyles.Title;
            this.gpgLabelDownloads.Anchor = AnchorStyles.Top;
            this.gpgLabelDownloads.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDownloads.AutoSize = true;
            this.gpgLabelDownloads.AutoStyle = true;
            this.gpgLabelDownloads.Font = new Font("Arial", 9.75f);
            this.gpgLabelDownloads.ForeColor = Color.White;
            this.gpgLabelDownloads.IgnoreMouseWheel = false;
            this.gpgLabelDownloads.IsStyled = false;
            this.gpgLabelDownloads.Location = new Point(0x137, 0xd4);
            this.gpgLabelDownloads.Name = "gpgLabelDownloads";
            this.gpgLabelDownloads.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelDownloads, null);
            this.gpgLabelDownloads.TabIndex = 30;
            this.gpgLabelDownloads.Text = "gpgLabel2";
            this.gpgLabelDownloads.TextStyle = TextStyles.Default;
            this.gpgLabel8.Anchor = AnchorStyles.Top;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoSize = true;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.Font = new Font("Arial", 9.75f);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x137, 0xc4);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x71, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel8, null);
            this.gpgLabel8.TabIndex = 0x1d;
            this.gpgLabel8.Text = "<LOC>Downloads";
            this.gpgLabel8.TextStyle = TextStyles.Bold;
            this.gpgLabelRatingCount.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelRatingCount.AutoSize = true;
            this.gpgLabelRatingCount.AutoStyle = true;
            this.gpgLabelRatingCount.Font = new Font("Arial", 9.75f);
            this.gpgLabelRatingCount.ForeColor = Color.Gold;
            this.gpgLabelRatingCount.IgnoreMouseWheel = false;
            this.gpgLabelRatingCount.IsStyled = false;
            this.gpgLabelRatingCount.Location = new Point(0x7c, 0x8a);
            this.gpgLabelRatingCount.Name = "gpgLabelRatingCount";
            this.gpgLabelRatingCount.Size = new Size(110, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelRatingCount, null);
            this.gpgLabelRatingCount.TabIndex = 0x1f;
            this.gpgLabelRatingCount.Text = "based on 0 votes.";
            this.gpgLabelRatingCount.TextStyle = TextStyles.Colored;
            this.gpgGroupBoxComments.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgGroupBoxComments.Controls.Add(this.skinButtonPostComment);
            this.gpgGroupBoxComments.Controls.Add(this.gpgPanelComments);
            this.gpgGroupBoxComments.Controls.Add(this.gpgLabelNoComments);
            this.gpgGroupBoxComments.Location = new Point(15, 0x123);
            this.gpgGroupBoxComments.Name = "gpgGroupBoxComments";
            this.gpgGroupBoxComments.Size = new Size(0x23f, 0x71);
            base.ttDefault.SetSuperTip(this.gpgGroupBoxComments, null);
            this.gpgGroupBoxComments.TabIndex = 0x20;
            this.gpgGroupBoxComments.TabStop = false;
            this.gpgGroupBoxComments.Text = "<LOC>Comments";
            this.skinButtonPostComment.Anchor = AnchorStyles.Bottom;
            this.skinButtonPostComment.AutoStyle = true;
            this.skinButtonPostComment.BackColor = Color.Black;
            this.skinButtonPostComment.ButtonState = 0;
            this.skinButtonPostComment.DialogResult = DialogResult.OK;
            this.skinButtonPostComment.DisabledForecolor = Color.Gray;
            this.skinButtonPostComment.DrawColor = Color.White;
            this.skinButtonPostComment.DrawEdges = true;
            this.skinButtonPostComment.FocusColor = Color.Yellow;
            this.skinButtonPostComment.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonPostComment.ForeColor = Color.White;
            this.skinButtonPostComment.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonPostComment.IsStyled = true;
            this.skinButtonPostComment.Location = new Point(0xcf, 0x58);
            this.skinButtonPostComment.Name = "skinButtonPostComment";
            this.skinButtonPostComment.Size = new Size(0x9c, 0x15);
            this.skinButtonPostComment.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonPostComment, null);
            this.skinButtonPostComment.TabIndex = 1;
            this.skinButtonPostComment.TabStop = true;
            this.skinButtonPostComment.Text = "<LOC>Post a Comment";
            this.skinButtonPostComment.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonPostComment.TextPadding = new Padding(0);
            this.skinButtonPostComment.Click += new EventHandler(this.skinButtonPostComment_Click);
            this.gpgPanelComments.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelComments.AutoScroll = true;
            this.gpgPanelComments.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelComments.BorderThickness = 2;
            this.gpgPanelComments.DrawBorder = false;
            this.gpgPanelComments.Location = new Point(7, 12);
            this.gpgPanelComments.Name = "gpgPanelComments";
            this.gpgPanelComments.Size = new Size(0x232, 70);
            base.ttDefault.SetSuperTip(this.gpgPanelComments, null);
            this.gpgPanelComments.TabIndex = 0;
            this.gpgPanelComments.SizeChanged += new EventHandler(this.gpgPanelComments_SizeChanged);
            this.gpgLabelNoComments.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelNoComments.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNoComments.AutoStyle = true;
            this.gpgLabelNoComments.Font = new Font("Arial", 9.75f);
            this.gpgLabelNoComments.ForeColor = Color.White;
            this.gpgLabelNoComments.IgnoreMouseWheel = false;
            this.gpgLabelNoComments.IsStyled = false;
            this.gpgLabelNoComments.Location = new Point(7, 14);
            this.gpgLabelNoComments.Name = "gpgLabelNoComments";
            this.gpgLabelNoComments.Size = new Size(0x232, 0x38);
            base.ttDefault.SetSuperTip(this.gpgLabelNoComments, null);
            this.gpgLabelNoComments.TabIndex = 2;
            this.gpgLabelNoComments.Text = "<LOC>No comments posted; be the first to post a comment!";
            this.gpgLabelNoComments.TextStyle = TextStyles.Default;
            this.gpgLabelNoComments.Visible = false;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x25a, 0x234);
            base.Controls.Add(this.gpgGroupBoxComments);
            base.Controls.Add(this.gpgLabelRatingCount);
            base.Controls.Add(this.gpgLabelDownloads);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgPictureBoxStars);
            base.Controls.Add(this.gpgLabelRating);
            base.Controls.Add(this.gpgPanelActions);
            base.Controls.Add(this.gpgLabelDescription);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgGroupBoxRate);
            base.Controls.Add(this.gpgLabelDate);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabelMap);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.skinLabelTitle);
            base.Controls.Add(this.gpgLabelOpponent);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgLabelPlayerName);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x25a, 0x234);
            base.Name = "DlgReplayInfo";
            base.Opacity = 0.999;
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Replay Info";
            base.Load += new EventHandler(this.DlgReplayInfo_Load);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgLabelPlayerName, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgLabelOpponent, 0);
            base.Controls.SetChildIndex(this.skinLabelTitle, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgLabelMap, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgLabelDate, 0);
            base.Controls.SetChildIndex(this.gpgGroupBoxRate, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgLabelDescription, 0);
            base.Controls.SetChildIndex(this.gpgPanelActions, 0);
            base.Controls.SetChildIndex(this.gpgLabelRating, 0);
            base.Controls.SetChildIndex(this.gpgPictureBoxStars, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabel8, 0);
            base.Controls.SetChildIndex(this.gpgLabelDownloads, 0);
            base.Controls.SetChildIndex(this.gpgLabelRatingCount, 0);
            base.Controls.SetChildIndex(this.gpgGroupBoxComments, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxLink.Properties.EndInit();
            ((ISupportInitialize) this.gpgPictureBoxStars).EndInit();
            this.gpgGroupBoxRate.ResumeLayout(false);
            this.gpgGroupBoxRate.PerformLayout();
            this.gpgPanelActions.ResumeLayout(false);
            this.gpgPanelActions.PerformLayout();
            this.gpgGroupBoxComments.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LayoutComments()
        {
            if (this.gpgPanelComments != null)
            {
                int num = 6;
                foreach (PnlReplayComment comment in this.gpgPanelComments.Controls)
                {
                    comment.Width = this.gpgPanelComments.ClientSize.Width;
                    comment.ResizeComment();
                    comment.Top = num;
                    num += comment.Height;
                }
            }
        }

        protected override void LoadLayoutData()
        {
            if ((base.LayoutData != null) && (base.LayoutData is ReplayInfo))
            {
                this.mReplay = base.LayoutData as ReplayInfo;
            }
            base.LoadLayoutData();
        }

        private void Replay_DownloadsChanged(object sender, EventArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.gpgLabelDownloads.Text = this.Replay.Downloads.ToString();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.gpgLabelDownloads.Text = this.Replay.Downloads.ToString();
            }
        }

        protected override void SaveLayoutData()
        {
            base.LayoutData = this.Replay;
        }

        private void skinButtonCopyLink_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.gpgTextBoxLink.Text);
            this.gpgTextBoxLink.Select();
            this.gpgTextBoxLink.SelectAll();
        }

        private void skinButtonDownload_Click(object sender, EventArgs e)
        {
            this.Replay.Download();
        }

        private void skinButtonPostComment_Click(object sender, EventArgs e)
        {
            DlgPostComment comment = new DlgPostComment();
            if ((comment.ShowDialog() == DialogResult.OK) && DataAccess.ExecuteQuery("PostReplayComment", new object[] { this.Replay.ID, comment.Comment }))
            {
                this.UpdateComments();
            }
        }

        private void skinButtonSubmitRating_Click(object sender, EventArgs e)
        {
            int rating = 0;
            foreach (Control control in this.gpgGroupBoxRate.Controls)
            {
                if (((control is GPGRadioButton) && (control as GPGRadioButton).Checked) && !int.TryParse(control.Name.Replace("gpgRadioButton", ""), out rating))
                {
                    return;
                }
            }
            ThreadQueue.QueueUserWorkItem(delegate (object s) {
                DataAccess.ExecuteQuery("RateReplay", new object[] { this.Replay.ID, rating });
            }, new object[0]);
            PlayerRatings[this.Replay.ID] = rating;
            ReplayInfo replay = this.Replay;
            replay.RatingCount++;
            ReplayInfo info2 = this.Replay;
            info2.RatingTotal += rating;
            this.UpdateRatingSection();
            this.UpdateRatingStars();
        }

        private void UpdateComments()
        {
            WaitCallback callBack = null;
            MappedObjectList<ReplayComment> comments = DataAccess.GetObjects<ReplayComment>("GetReplayComments", new object[] { this.Replay.ID });
            if ((this.CommentCount > -1) && (comments.Count != this.CommentCount))
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        VGen0 method = null;
                        try
                        {
                            int num = 0;
                            do
                            {
                                Thread.Sleep(0x3e8);
                                num++;
                                comments = DataAccess.GetObjects<ReplayComment>("GetReplayComments", new object[] { this.Replay.ID });
                            }
                            while ((comments.Count != this.CommentCount) && (num < 2));
                            if (comments.Count == this.CommentCount)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.UpdateComments(comments);
                                    };
                                }
                                this.BeginInvoke(method);
                            }
                            else
                            {
                                ErrorLog.WriteLine("Did insert on master db and failed to get progogated data after 3000ms.", new object[0]);
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                            ErrorLog.WriteLine("Did insert on master db and failed to get progogated data after 3000ms.", new object[0]);
                        }
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
            else
            {
                this.UpdateComments(comments);
            }
        }

        private void UpdateComments(MappedObjectList<ReplayComment> comments)
        {
            EventHandler handler = null;
            try
            {
                this.gpgPanelComments.SuspendLayout();
                this.gpgPanelComments.Controls.Clear();
                bool flag = false;
                if (comments.Count > 0)
                {
                    this.gpgLabelNoComments.Visible = false;
                    this.gpgPanelComments.Visible = true;
                    this.gpgPanelComments.BringToFront();
                    foreach (ReplayComment comment in comments)
                    {
                        if ((this.DeletedComment <= 0) || (comment.ID != this.DeletedComment))
                        {
                            PnlReplayComment comment2 = new PnlReplayComment(this.Replay, comment);
                            this.gpgPanelComments.Controls.Add(comment2);
                            comment2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                            if (handler == null)
                            {
                                handler = delegate (object s, EventArgs e) {
                                    this.DeletedComment = (s as ReplayComment).ID;
                                    this.CommentCount--;
                                    this.UpdateComments();
                                };
                            }
                            comment2.DeleteComment += handler;
                            flag = true;
                        }
                    }
                    this.LayoutComments();
                }
                else
                {
                    this.gpgPanelComments.Visible = false;
                    this.gpgLabelNoComments.Visible = true;
                    this.gpgLabelNoComments.BringToFront();
                }
                if (!flag)
                {
                    this.gpgPanelComments.Visible = false;
                    this.gpgLabelNoComments.Visible = true;
                    this.gpgLabelNoComments.BringToFront();
                }
                this.gpgPanelComments.ResumeLayout(true);
                this.gpgGroupBoxComments.Refresh();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                this.DeletedComment = -1;
            }
        }

        private void UpdateRatingSection()
        {
            if (PlayerRatings.ContainsKey(this.Replay.ID))
            {
                int num = (this.gpgGroupBoxRate.Height - this.gpgLabelRating.Height) + (this.gpgGroupBoxRate.Top - this.gpgPanelActions.Bottom);
                base.Height -= num;
                this.gpgGroupBoxRate.Visible = false;
                this.gpgPanelActions.Top += num;
                this.gpgLabelRating.Top += num;
                this.gpgLabelRating.Text = string.Format(Loc.Get("<LOC>You rated this replay {0}."), PlayerRatings[this.Replay.ID]);
                this.gpgLabelRating.Visible = true;
            }
            else
            {
                this.gpgGroupBoxRate.Visible = true;
                this.gpgLabelRating.Visible = false;
            }
        }

        private void UpdateRatingStars()
        {
            if (this.Replay.Rating == 0f)
            {
                this.gpgPictureBoxStars.Visible = false;
                this.gpgLabelRatingCount.Left = this.gpgPictureBoxStars.Left;
                this.gpgLabelRatingCount.Text = Loc.Get("<LOC>This replay has not been rated.");
            }
            else
            {
                int num = (int) (this.gpgPictureBoxStars.Image.Width * (this.Replay.Rating / 10f));
                this.gpgPictureBoxStars.Width = num;
                this.gpgPictureBoxStars.Height = this.gpgPictureBoxStars.Image.Height;
                this.gpgPictureBoxStars.Visible = true;
                this.gpgLabelRatingCount.Left = this.gpgPictureBoxStars.Right + 4;
                this.gpgLabelRatingCount.Text = string.Format(Loc.Get("<LOC>based on {0} vote(s)."), this.Replay.RatingCount);
                base.ttDefault.SetToolTip(this.gpgPictureBoxStars, this.Replay.Rating.ToString());
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return true;
            }
        }

        public ReplayInfo Replay
        {
            get
            {
                return this.mReplay;
            }
        }

        public override string SingletonName
        {
            get
            {
                return base.SingletonName;
            }
        }
    }
}

