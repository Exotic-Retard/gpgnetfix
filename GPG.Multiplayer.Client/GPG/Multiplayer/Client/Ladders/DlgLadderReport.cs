namespace GPG.Multiplayer.Client.Ladders
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.LadderService;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgLadderReport : DlgBase
    {
        private IContainer components = null;
        private LadderGameSession[] GameSessions = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabelDate;
        private GPGLabel gpgLabelLadderName;
        private GPGLabel gpgLabelOpponent;
        private GPGRadioButton gpgRadioButtonDraw;
        private GPGRadioButton gpgRadioButtonLoss;
        private GPGRadioButton gpgRadioButtonNoDC;
        private GPGRadioButton gpgRadioButtonWin;
        private GPGRadioButton gpgRadioButtonYesDC;
        private GPGTextArea gpgTextAreaComment;
        private LadderGameSession InfoSession = null;
        private LadderGameSession OpponentSession = null;
        private PictureBox pictureBoxRate1;
        private PictureBox pictureBoxRate2;
        private PictureBox pictureBoxRate3;
        private PictureBox pictureBoxRate4;
        private PictureBox pictureBoxRate5;
        private LadderGameSession PlayerSession = null;
        private SkinButton skinButtonDeleteRating;
        private SkinButton skinButtonSubmit;
        private SkinGroupPanel skinGroupPanel1;
        private SkinGroupPanel skinGroupPanel2;
        private int? UserRating = null;

        public DlgLadderReport(LadderGameSession[] gameSessions)
        {
            this.InitializeComponent();
            this.GameSessions = gameSessions;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgLadderReport));
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabelLadderName = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelDate = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabelOpponent = new GPGLabel();
            this.skinGroupPanel1 = new SkinGroupPanel();
            this.gpgRadioButtonDraw = new GPGRadioButton();
            this.gpgRadioButtonLoss = new GPGRadioButton();
            this.gpgRadioButtonWin = new GPGRadioButton();
            this.skinGroupPanel2 = new SkinGroupPanel();
            this.gpgRadioButtonNoDC = new GPGRadioButton();
            this.gpgRadioButtonYesDC = new GPGRadioButton();
            this.skinButtonSubmit = new SkinButton();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgTextAreaComment = new GPGTextArea();
            this.gpgLabel7 = new GPGLabel();
            this.skinButtonDeleteRating = new SkinButton();
            this.pictureBoxRate1 = new PictureBox();
            this.pictureBoxRate5 = new PictureBox();
            this.pictureBoxRate4 = new PictureBox();
            this.pictureBoxRate3 = new PictureBox();
            this.pictureBoxRate2 = new PictureBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.skinGroupPanel1.SuspendLayout();
            this.skinGroupPanel2.SuspendLayout();
            this.gpgTextAreaComment.Properties.BeginInit();
            ((ISupportInitialize) this.pictureBoxRate1).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate5).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate4).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate3).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate2).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x261, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 80);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x284, 0x2c);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC id=_0f8857421151baeb20c1996b4daccb45>This form is to report the outcome of the game described below. Please report accurately.";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0x7c);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x7f, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 8;
            this.gpgLabel2.Text = "<LOC>Ladder Name";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgLabelLadderName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelLadderName.AutoSize = true;
            this.gpgLabelLadderName.AutoStyle = true;
            this.gpgLabelLadderName.Font = new Font("Arial", 9.75f);
            this.gpgLabelLadderName.ForeColor = Color.White;
            this.gpgLabelLadderName.IgnoreMouseWheel = false;
            this.gpgLabelLadderName.IsStyled = false;
            this.gpgLabelLadderName.Location = new Point(12, 140);
            this.gpgLabelLadderName.Name = "gpgLabelLadderName";
            this.gpgLabelLadderName.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelLadderName, null);
            this.gpgLabelLadderName.TabIndex = 9;
            this.gpgLabelLadderName.Text = "gpgLabel3";
            this.gpgLabelLadderName.TextStyle = TextStyles.Default;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(470, 0x7c);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x74, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 10;
            this.gpgLabel3.Text = "<LOC>Game Date";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgLabelDate.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDate.AutoSize = true;
            this.gpgLabelDate.AutoStyle = true;
            this.gpgLabelDate.Font = new Font("Arial", 9.75f);
            this.gpgLabelDate.ForeColor = Color.White;
            this.gpgLabelDate.IgnoreMouseWheel = false;
            this.gpgLabelDate.IsStyled = false;
            this.gpgLabelDate.Location = new Point(470, 140);
            this.gpgLabelDate.Name = "gpgLabelDate";
            this.gpgLabelDate.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelDate, null);
            this.gpgLabelDate.TabIndex = 11;
            this.gpgLabelDate.Text = "gpgLabel3";
            this.gpgLabelDate.TextStyle = TextStyles.Default;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0xf4, 0x7c);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x90, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 12;
            this.gpgLabel4.Text = "<LOC>Opponent Name";
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.gpgLabelOpponent.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelOpponent.AutoSize = true;
            this.gpgLabelOpponent.AutoStyle = true;
            this.gpgLabelOpponent.Font = new Font("Arial", 9.75f);
            this.gpgLabelOpponent.ForeColor = Color.White;
            this.gpgLabelOpponent.IgnoreMouseWheel = false;
            this.gpgLabelOpponent.IsStyled = false;
            this.gpgLabelOpponent.Location = new Point(0xf4, 140);
            this.gpgLabelOpponent.Name = "gpgLabelOpponent";
            this.gpgLabelOpponent.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelOpponent, null);
            this.gpgLabelOpponent.TabIndex = 13;
            this.gpgLabelOpponent.Text = "gpgLabel3";
            this.gpgLabelOpponent.TextStyle = TextStyles.Default;
            this.skinGroupPanel1.AutoStyle = false;
            this.skinGroupPanel1.BackColor = Color.Black;
            this.skinGroupPanel1.Controls.Add(this.gpgRadioButtonDraw);
            this.skinGroupPanel1.Controls.Add(this.gpgRadioButtonLoss);
            this.skinGroupPanel1.Controls.Add(this.gpgRadioButtonWin);
            this.skinGroupPanel1.CutCorner = true;
            this.skinGroupPanel1.HeaderImage = (Image) manager.GetObject("skinGroupPanel1.HeaderImage");
            this.skinGroupPanel1.IsStyled = true;
            this.skinGroupPanel1.Location = new Point(15, 0xb9);
            this.skinGroupPanel1.Name = "skinGroupPanel1";
            this.skinGroupPanel1.Size = new Size(0x282, 0x52);
            base.ttDefault.SetSuperTip(this.skinGroupPanel1, null);
            this.skinGroupPanel1.TabIndex = 14;
            this.skinGroupPanel1.Text = "<LOC>The outcome of the game was...";
            this.skinGroupPanel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinGroupPanel1.TextPadding = new Padding(8, 0, 0, 0);
            this.gpgRadioButtonDraw.AutoSize = true;
            this.gpgRadioButtonDraw.Location = new Point(0x130, 0x2d);
            this.gpgRadioButtonDraw.Name = "gpgRadioButtonDraw";
            this.gpgRadioButtonDraw.Size = new Size(0x99, 0x16);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonDraw, null);
            this.gpgRadioButtonDraw.TabIndex = 0x29;
            this.gpgRadioButtonDraw.TabStop = true;
            this.gpgRadioButtonDraw.Text = "<LOC>A Draw";
            this.gpgRadioButtonDraw.UseVisualStyleBackColor = true;
            this.gpgRadioButtonLoss.AutoSize = true;
            this.gpgRadioButtonLoss.Location = new Point(160, 0x2d);
            this.gpgRadioButtonLoss.Name = "gpgRadioButtonLoss";
            this.gpgRadioButtonLoss.Size = new Size(0x8a, 0x16);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonLoss, null);
            this.gpgRadioButtonLoss.TabIndex = 40;
            this.gpgRadioButtonLoss.TabStop = true;
            this.gpgRadioButtonLoss.Text = "<LOC>I Lost";
            this.gpgRadioButtonLoss.UseVisualStyleBackColor = true;
            this.gpgRadioButtonWin.AutoSize = true;
            this.gpgRadioButtonWin.Location = new Point(13, 0x2d);
            this.gpgRadioButtonWin.Name = "gpgRadioButtonWin";
            this.gpgRadioButtonWin.Size = new Size(0x8d, 0x16);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonWin, null);
            this.gpgRadioButtonWin.TabIndex = 0x27;
            this.gpgRadioButtonWin.TabStop = true;
            this.gpgRadioButtonWin.Text = "<LOC>I Won";
            this.gpgRadioButtonWin.UseVisualStyleBackColor = true;
            this.skinGroupPanel2.AutoStyle = false;
            this.skinGroupPanel2.BackColor = Color.Black;
            this.skinGroupPanel2.Controls.Add(this.gpgRadioButtonNoDC);
            this.skinGroupPanel2.Controls.Add(this.gpgRadioButtonYesDC);
            this.skinGroupPanel2.CutCorner = true;
            this.skinGroupPanel2.HeaderImage = (Image) manager.GetObject("skinGroupPanel2.HeaderImage");
            this.skinGroupPanel2.IsStyled = true;
            this.skinGroupPanel2.Location = new Point(15, 0x11a);
            this.skinGroupPanel2.Name = "skinGroupPanel2";
            this.skinGroupPanel2.Size = new Size(0x282, 0x52);
            base.ttDefault.SetSuperTip(this.skinGroupPanel2, null);
            this.skinGroupPanel2.TabIndex = 15;
            this.skinGroupPanel2.Text = "<LOC>Was there a disconnect?";
            this.skinGroupPanel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinGroupPanel2.TextPadding = new Padding(8, 0, 0, 0);
            this.gpgRadioButtonNoDC.AutoSize = true;
            this.gpgRadioButtonNoDC.Location = new Point(160, 0x2c);
            this.gpgRadioButtonNoDC.Name = "gpgRadioButtonNoDC";
            this.gpgRadioButtonNoDC.Size = new Size(0x70, 0x16);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonNoDC, null);
            this.gpgRadioButtonNoDC.TabIndex = 0x29;
            this.gpgRadioButtonNoDC.TabStop = true;
            this.gpgRadioButtonNoDC.Text = "<LOC>No";
            this.gpgRadioButtonNoDC.UseVisualStyleBackColor = true;
            this.gpgRadioButtonYesDC.AutoSize = true;
            this.gpgRadioButtonYesDC.Location = new Point(13, 0x2c);
            this.gpgRadioButtonYesDC.Name = "gpgRadioButtonYesDC";
            this.gpgRadioButtonYesDC.Size = new Size(120, 0x16);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonYesDC, null);
            this.gpgRadioButtonYesDC.TabIndex = 40;
            this.gpgRadioButtonYesDC.TabStop = true;
            this.gpgRadioButtonYesDC.Text = "<LOC>Yes";
            this.gpgRadioButtonYesDC.UseVisualStyleBackColor = true;
            this.skinButtonSubmit.AutoStyle = true;
            this.skinButtonSubmit.BackColor = Color.Transparent;
            this.skinButtonSubmit.ButtonState = 0;
            this.skinButtonSubmit.DialogResult = DialogResult.OK;
            this.skinButtonSubmit.DisabledForecolor = Color.Gray;
            this.skinButtonSubmit.DrawColor = Color.White;
            this.skinButtonSubmit.DrawEdges = true;
            this.skinButtonSubmit.FocusColor = Color.Yellow;
            this.skinButtonSubmit.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSubmit.ForeColor = Color.White;
            this.skinButtonSubmit.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSubmit.IsStyled = true;
            this.skinButtonSubmit.Location = new Point(0x110, 0x23e);
            this.skinButtonSubmit.Name = "skinButtonSubmit";
            this.skinButtonSubmit.Size = new Size(0x7d, 0x1a);
            this.skinButtonSubmit.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSubmit, null);
            this.skinButtonSubmit.TabIndex = 0x10;
            this.skinButtonSubmit.TabStop = true;
            this.skinButtonSubmit.Text = "<LOC>Submit";
            this.skinButtonSubmit.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSubmit.TextPadding = new Padding(0);
            this.skinButtonSubmit.Click += new EventHandler(this.skinButtonSubmit_Click);
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(12, 0x179);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(170, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x11;
            this.gpgLabel5.Text = "<LOC>I rate my opponent...";
            this.gpgLabel5.TextStyle = TextStyles.Default;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(12, 0x1a2);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0xe0, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x12;
            this.gpgLabel6.Text = "<LOC>Comments on your opponent?";
            this.gpgLabel6.TextStyle = TextStyles.Default;
            this.gpgTextAreaComment.BorderColor = Color.White;
            this.gpgTextAreaComment.Location = new Point(15, 0x1b5);
            this.gpgTextAreaComment.Name = "gpgTextAreaComment";
            this.gpgTextAreaComment.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaComment.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaComment.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaComment.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaComment.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaComment.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaComment.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaComment.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaComment.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaComment.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaComment.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaComment.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaComment.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaComment.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaComment.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaComment.Size = new Size(0x281, 0x60);
            this.gpgTextAreaComment.TabIndex = 0x13;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoSize = true;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(15, 0x218);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x21a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 20;
            this.gpgLabel7.Text = "<LOC id=_18d96f2f882df8b30e71fbeff580cde6>Please be responsible with your reputation rating... what goes around comes around!";
            this.gpgLabel7.TextStyle = TextStyles.Descriptor;
            this.skinButtonDeleteRating.AutoStyle = true;
            this.skinButtonDeleteRating.BackColor = Color.Transparent;
            this.skinButtonDeleteRating.ButtonState = 0;
            this.skinButtonDeleteRating.DialogResult = DialogResult.OK;
            this.skinButtonDeleteRating.DisabledForecolor = Color.Gray;
            this.skinButtonDeleteRating.DrawColor = Color.White;
            this.skinButtonDeleteRating.DrawEdges = false;
            this.skinButtonDeleteRating.FocusColor = Color.Yellow;
            this.skinButtonDeleteRating.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDeleteRating.ForeColor = Color.White;
            this.skinButtonDeleteRating.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonDeleteRating.IsStyled = true;
            this.skinButtonDeleteRating.Location = new Point(0x72, 0x18a);
            this.skinButtonDeleteRating.Name = "skinButtonDeleteRating";
            this.skinButtonDeleteRating.Size = new Size(0x18, 0x12);
            this.skinButtonDeleteRating.SkinBasePath = @"Dialog\ContentManager\BtnDeleteRating";
            base.ttDefault.SetSuperTip(this.skinButtonDeleteRating, null);
            this.skinButtonDeleteRating.TabIndex = 0x1a;
            this.skinButtonDeleteRating.TabStop = true;
            this.skinButtonDeleteRating.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDeleteRating.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonDeleteRating, "<LOC>Delete Rating");
            this.skinButtonDeleteRating.Click += new EventHandler(this.skinButtonDeleteRating_Click);
            this.pictureBoxRate1.Location = new Point(15, 0x189);
            this.pictureBoxRate1.Margin = new Padding(0);
            this.pictureBoxRate1.Name = "pictureBoxRate1";
            this.pictureBoxRate1.Size = new Size(20, 20);
            base.ttDefault.SetSuperTip(this.pictureBoxRate1, null);
            this.pictureBoxRate1.TabIndex = 0x15;
            this.pictureBoxRate1.TabStop = false;
            this.pictureBoxRate1.Tag = "1";
            this.pictureBoxRate1.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate1.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate1.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.pictureBoxRate5.Location = new Point(0x5b, 0x189);
            this.pictureBoxRate5.Margin = new Padding(0);
            this.pictureBoxRate5.Name = "pictureBoxRate5";
            this.pictureBoxRate5.Size = new Size(20, 20);
            base.ttDefault.SetSuperTip(this.pictureBoxRate5, null);
            this.pictureBoxRate5.TabIndex = 0x19;
            this.pictureBoxRate5.TabStop = false;
            this.pictureBoxRate5.Tag = "5";
            this.pictureBoxRate5.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate5.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate5.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.pictureBoxRate4.Location = new Point(0x48, 0x189);
            this.pictureBoxRate4.Margin = new Padding(0);
            this.pictureBoxRate4.Name = "pictureBoxRate4";
            this.pictureBoxRate4.Size = new Size(20, 20);
            base.ttDefault.SetSuperTip(this.pictureBoxRate4, null);
            this.pictureBoxRate4.TabIndex = 0x18;
            this.pictureBoxRate4.TabStop = false;
            this.pictureBoxRate4.Tag = "4";
            this.pictureBoxRate4.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate4.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate4.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.pictureBoxRate3.Location = new Point(0x35, 0x189);
            this.pictureBoxRate3.Margin = new Padding(0);
            this.pictureBoxRate3.Name = "pictureBoxRate3";
            this.pictureBoxRate3.Size = new Size(20, 20);
            base.ttDefault.SetSuperTip(this.pictureBoxRate3, null);
            this.pictureBoxRate3.TabIndex = 0x17;
            this.pictureBoxRate3.TabStop = false;
            this.pictureBoxRate3.Tag = "3";
            this.pictureBoxRate3.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate3.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate3.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.pictureBoxRate2.Location = new Point(0x22, 0x189);
            this.pictureBoxRate2.Margin = new Padding(0);
            this.pictureBoxRate2.Name = "pictureBoxRate2";
            this.pictureBoxRate2.Size = new Size(20, 20);
            base.ttDefault.SetSuperTip(this.pictureBoxRate2, null);
            this.pictureBoxRate2.TabIndex = 0x16;
            this.pictureBoxRate2.TabStop = false;
            this.pictureBoxRate2.Tag = "2";
            this.pictureBoxRate2.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate2.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate2.MouseEnter += new EventHandler(this.StarMouseEnter);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x29c, 0x289);
            base.Controls.Add(this.skinButtonDeleteRating);
            base.Controls.Add(this.pictureBoxRate1);
            base.Controls.Add(this.pictureBoxRate5);
            base.Controls.Add(this.pictureBoxRate4);
            base.Controls.Add(this.pictureBoxRate3);
            base.Controls.Add(this.pictureBoxRate2);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.gpgTextAreaComment);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.skinButtonSubmit);
            base.Controls.Add(this.skinGroupPanel2);
            base.Controls.Add(this.skinGroupPanel1);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgLabelOpponent);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabelDate);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabelLadderName);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x29c, 0x289);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x29c, 0x224);
            base.Name = "DlgLadderReport";
            base.Opacity = 1.0;
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Challenge Ladder Report Form";
            base.Controls.SetChildIndex(this.gpgLabelLadderName, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgLabelDate, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgLabelOpponent, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.skinGroupPanel1, 0);
            base.Controls.SetChildIndex(this.skinGroupPanel2, 0);
            base.Controls.SetChildIndex(this.skinButtonSubmit, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgTextAreaComment, 0);
            base.Controls.SetChildIndex(this.gpgLabel7, 0);
            base.Controls.SetChildIndex(this.pictureBoxRate2, 0);
            base.Controls.SetChildIndex(this.pictureBoxRate3, 0);
            base.Controls.SetChildIndex(this.pictureBoxRate4, 0);
            base.Controls.SetChildIndex(this.pictureBoxRate5, 0);
            base.Controls.SetChildIndex(this.pictureBoxRate1, 0);
            base.Controls.SetChildIndex(this.skinButtonDeleteRating, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.skinGroupPanel1.ResumeLayout(false);
            this.skinGroupPanel1.PerformLayout();
            this.skinGroupPanel2.ResumeLayout(false);
            this.skinGroupPanel2.PerformLayout();
            this.gpgTextAreaComment.Properties.EndInit();
            ((ISupportInitialize) this.pictureBoxRate1).EndInit();
            ((ISupportInitialize) this.pictureBoxRate5).EndInit();
            ((ISupportInitialize) this.pictureBoxRate4).EndInit();
            ((ISupportInitialize) this.pictureBoxRate3).EndInit();
            ((ISupportInitialize) this.pictureBoxRate2).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.GameSessions == null)
            {
                ErrorLog.WriteLine("Unable to determine players in game session", new object[0]);
                base.DialogResult = DialogResult.Abort;
                base.Close();
            }
            else
            {
                LadderGameSession session = null;
                LadderGameSession session2 = null;
                if (this.GameSessions.Length == 2)
                {
                    if (this.GameSessions[0].EntityID == User.Current.ID)
                    {
                        session = this.GameSessions[0];
                        session2 = this.GameSessions[1];
                    }
                    else
                    {
                        session = this.GameSessions[1];
                        session2 = this.GameSessions[0];
                    }
                }
                else if (this.GameSessions.Length == 1)
                {
                    if (this.GameSessions[0].EntityID == User.Current.ID)
                    {
                        session = this.GameSessions[0];
                    }
                    else
                    {
                        session2 = this.GameSessions[0];
                    }
                }
                else
                {
                    ErrorLog.WriteLine("Unable to determine players in game session", new object[0]);
                    base.DialogResult = DialogResult.Abort;
                    base.Close();
                    return;
                }
                this.InfoSession = session;
                if (this.InfoSession == null)
                {
                    this.InfoSession = session2;
                }
                if (this.InfoSession == null)
                {
                    ErrorLog.WriteLine("Unable to determine players in game session", new object[0]);
                    base.DialogResult = DialogResult.Abort;
                    base.Close();
                }
                else
                {
                    this.gpgLabelDate.Text = DateTime.SpecifyKind(this.InfoSession.StartDate, DateTimeKind.Utc).ToLocalTime().ToString();
                    this.gpgLabelLadderName.Text = this.InfoSession.LadderInstance.Description;
                    if (session2 != null)
                    {
                        this.gpgLabelOpponent.Text = session2.EntityName;
                    }
                    else
                    {
                        this.gpgLabelOpponent.Text = Loc.Get("<LOC>Undetermined");
                    }
                    PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
                    for (int i = 0; i < boxArray.Length; i++)
                    {
                        boxArray[i].Image = VaultImages.star_empty;
                    }
                    this.PlayerSession = session;
                    this.OpponentSession = session2;
                    if ((this.PlayerSession != null) && (this.OpponentSession != null))
                    {
                        base.MainForm.SetLastLadderGameType(this.InfoSession.LadderInstanceID, this.PlayerSession.IsAutomatch || (this.PlayerSession.StartRank < this.OpponentSession.StartRank));
                    }
                }
            }
        }

        private void skinButtonDeleteRating_Click(object sender, EventArgs e)
        {
            this.UserRating = null;
            this.skinButtonDeleteRating.Visible = false;
            PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
            for (int i = 0; i < boxArray.Length; i++)
            {
                boxArray[i].Image = VaultImages.star_empty;
            }
        }

        private void skinButtonSubmit_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            bool flag = false;
            if (!((this.gpgRadioButtonWin.Checked || this.gpgRadioButtonLoss.Checked) || this.gpgRadioButtonDraw.Checked))
            {
                base.Error(new Control[] { this.gpgRadioButtonWin, this.gpgRadioButtonLoss, this.gpgRadioButtonDraw }, "<LOC>You must choose an outcome for this game.", new object[0]);
                flag = true;
            }
            if (!(this.gpgRadioButtonYesDC.Checked || this.gpgRadioButtonNoDC.Checked))
            {
                base.Error(new Control[] { this.gpgRadioButtonYesDC, this.gpgRadioButtonNoDC }, "<LOC>You must specify whether there was a disconnect this game.", new object[0]);
                flag = true;
            }
            if (!flag)
            {
                if (!new QuazalQuery("SubmitLadderGameReport", new object[] { this.InfoSession.LadderInstanceID, this.InfoSession.GameID, this.PlayerSession.EntityID, DateTime.UtcNow, this.gpgRadioButtonWin.Checked, this.gpgRadioButtonLoss.Checked, this.gpgRadioButtonDraw.Checked, this.gpgRadioButtonYesDC.Checked }).ExecuteNonQuery())
                {
                    ErrorLog.WriteLine("Failed to submit ladder game report for game: {0}", new object[] { this.InfoSession.GameID });
                    base.DialogResult = DialogResult.Abort;
                    base.Close();
                }
                else
                {
                    if (!new QuazalQuery("FlagLadderGameSessionAsReported", new object[] { this.InfoSession.GameID, this.PlayerSession.EntityID }).ExecuteNonQuery())
                    {
                        ErrorLog.WriteLine("Failed to flag session as reported for game: {0}", new object[] { this.InfoSession.GameID });
                    }
                    if ((this.UserRating.HasValue && (this.OpponentSession != null)) && (this.PlayerSession != null))
                    {
                        if (new QuazalQuery("UpdateLadderReputationRating", new object[] { this.UserRating.Value, this.OpponentSession.EntityID }).ExecuteNonQuery())
                        {
                            if (!new QuazalQuery("UpdateLadderReputationRater", new object[] { this.UserRating.Value, this.PlayerSession.EntityID }).ExecuteNonQuery())
                            {
                                ErrorLog.WriteLine("Failed to save reputation rater.", new object[] { this.InfoSession.GameID });
                            }
                        }
                        else
                        {
                            ErrorLog.WriteLine("Failed to save reputation rating.", new object[] { this.InfoSession.GameID });
                        }
                    }
                    if (((((this.gpgTextAreaComment.Text != null) && (this.gpgTextAreaComment.Text.Length > 0)) && (this.OpponentSession != null)) && (this.PlayerSession != null)) && !new QuazalQuery("CreateLadderParticipantComment", new object[] { this.OpponentSession.EntityID, this.PlayerSession.EntityID, this.InfoSession.GameID, this.gpgTextAreaComment.Text, DateTime.UtcNow }).ExecuteNonQuery())
                    {
                        ErrorLog.WriteLine("Failed to create opponent comment.", new object[] { this.InfoSession.GameID });
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
        }

        private void StarMouseClick(object sender, EventArgs e)
        {
            int num = Convert.ToInt32((sender as Control).Tag);
            if (!this.UserRating.HasValue || (this.UserRating.Value != num))
            {
                this.UserRating = new int?(num);
                this.skinButtonDeleteRating.Visible = true;
            }
        }

        private void StarMouseEnter(object sender, EventArgs e)
        {
            PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
            int num = Convert.ToInt32((sender as Control).Tag);
            if (this.UserRating.HasValue && (this.UserRating.Value == num))
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
            for (int i = 0; i < boxArray.Length; i++)
            {
                if (i < num)
                {
                    boxArray[i].Image = VaultImages.star;
                }
                else
                {
                    boxArray[i].Image = VaultImages.star_empty;
                }
            }
        }

        private void StarMouseLeave(object sender, EventArgs e)
        {
            PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
            for (int i = 0; i < boxArray.Length; i++)
            {
                if (this.UserRating.HasValue && (i < this.UserRating.Value))
                {
                    boxArray[i].Image = VaultImages.star;
                }
                else
                {
                    boxArray[i].Image = VaultImages.star_empty;
                }
            }
        }
    }
}

