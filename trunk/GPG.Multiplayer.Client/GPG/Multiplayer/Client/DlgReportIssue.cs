namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgReportIssue : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGRadioButton gpgRadioButtonCheat;
        private GPGRadioButton gpgRadioButtonDisconnect;
        private GPGRadioButton gpgRadioButtonExploit;
        private GPGTextArea gpgTextAreaDetails;
        private GPGTextBox gpgTextBoxPlayer;
        private GPGTextBox gpgTextBoxReplay;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgReportIssue()
        {
            this.InitializeComponent();
            this.gpgLabel4.Text = Loc.Get("<LOC>GPG does not correct individual game problems, but analyzes user input to systematically locate bugs, cheats, and exploits. In some cases, your game may be corrected as the result of this analysis. Thank you for your time.");
            if (GameInformation.SelectedGame.IsSpaceSiege)
            {
                this.gpgLabel5.Visible = false;
                this.gpgTextBoxReplay.Visible = false;
            }
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgReportIssue));
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextAreaDetails = new GPGTextArea();
            this.gpgLabel2 = new GPGLabel();
            this.gpgRadioButtonDisconnect = new GPGRadioButton();
            this.gpgRadioButtonCheat = new GPGRadioButton();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.gpgLabel3 = new GPGLabel();
            this.gpgTextBoxPlayer = new GPGTextBox();
            this.gpgRadioButtonExploit = new GPGRadioButton();
            this.gpgLabel4 = new GPGLabel();
            this.gpgTextBoxReplay = new GPGTextBox();
            this.gpgLabel5 = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextAreaDetails.Properties.BeginInit();
            this.gpgTextBoxPlayer.Properties.BeginInit();
            this.gpgTextBoxReplay.Properties.BeginInit();
            base.SuspendLayout();
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
            this.gpgLabel1.Location = new Point(13, 0x87);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xa9, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>Select an issue type";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextAreaDetails.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgTextAreaDetails.BorderColor = Color.White;
            this.gpgTextAreaDetails.Location = new Point(12, 0x13b);
            this.gpgTextAreaDetails.Name = "gpgTextAreaDetails";
            this.gpgTextAreaDetails.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaDetails.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaDetails.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaDetails.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaDetails.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaDetails.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaDetails.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaDetails.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaDetails.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaDetails.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaDetails.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaDetails.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaDetails.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaDetails.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaDetails.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaDetails.Properties.MaxLength = 0x1000;
            this.gpgTextAreaDetails.Size = new Size(0x268, 0x7c);
            this.gpgTextAreaDetails.TabIndex = 8;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0x128);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x182, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "<LOC>Please describe this issue in detail (max 4096 characters)";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgRadioButtonDisconnect.AutoSize = true;
            this.gpgRadioButtonDisconnect.Location = new Point(0x116, 0x9a);
            this.gpgRadioButtonDisconnect.Name = "gpgRadioButtonDisconnect";
            this.gpgRadioButtonDisconnect.Size = new Size(0x81, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonDisconnect, null);
            this.gpgRadioButtonDisconnect.TabIndex = 10;
            this.gpgRadioButtonDisconnect.Text = "<LOC>Disconnect";
            this.gpgRadioButtonDisconnect.UseVisualStyleBackColor = true;
            this.gpgRadioButtonCheat.AutoSize = true;
            this.gpgRadioButtonCheat.Location = new Point(0x94, 0x9a);
            this.gpgRadioButtonCheat.Name = "gpgRadioButtonCheat";
            this.gpgRadioButtonCheat.Size = new Size(0x65, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonCheat, null);
            this.gpgRadioButtonCheat.TabIndex = 11;
            this.gpgRadioButtonCheat.Text = "<LOC>Cheat";
            this.gpgRadioButtonCheat.UseVisualStyleBackColor = true;
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
            this.skinButtonCancel.Location = new Point(0x20f, 0x1cb);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x65, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 12;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Transparent;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawColor = Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(420, 0x1cb);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x65, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 13;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(12, 0xba);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0xa8, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 14;
            this.gpgLabel3.Text = "<LOC>Other Players Name";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgTextBoxPlayer.Location = new Point(12, 0xcd);
            this.gpgTextBoxPlayer.Name = "gpgTextBoxPlayer";
            this.gpgTextBoxPlayer.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPlayer.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPlayer.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPlayer.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPlayer.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPlayer.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPlayer.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPlayer.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPlayer.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPlayer.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPlayer.Size = new Size(0x134, 20);
            this.gpgTextBoxPlayer.TabIndex = 15;
            this.gpgRadioButtonExploit.AutoSize = true;
            this.gpgRadioButtonExploit.Checked = true;
            this.gpgRadioButtonExploit.Location = new Point(12, 0x9a);
            this.gpgRadioButtonExploit.Name = "gpgRadioButtonExploit";
            this.gpgRadioButtonExploit.Size = new Size(0x69, 0x11);
            base.ttDefault.SetSuperTip(this.gpgRadioButtonExploit, null);
            this.gpgRadioButtonExploit.TabIndex = 0x10;
            this.gpgRadioButtonExploit.TabStop = true;
            this.gpgRadioButtonExploit.Text = "<LOC>Exploit";
            this.gpgRadioButtonExploit.UseVisualStyleBackColor = true;
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(13, 0x42);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x268, 60);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 0x11;
            this.gpgLabel4.Text = manager.GetString("gpgLabel4.Text");
            this.gpgLabel4.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.gpgTextBoxReplay.Location = new Point(12, 0x105);
            this.gpgTextBoxReplay.Name = "gpgTextBoxReplay";
            this.gpgTextBoxReplay.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxReplay.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxReplay.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxReplay.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxReplay.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxReplay.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxReplay.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxReplay.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxReplay.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxReplay.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxReplay.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxReplay.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxReplay.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxReplay.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxReplay.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxReplay.Size = new Size(410, 20);
            this.gpgTextBoxReplay.TabIndex = 0x13;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(12, 0xf2);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x138, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x12;
            this.gpgLabel5.Text = "<LOC>Replay Link (Found on the replay info screen)";
            this.gpgLabel5.TextStyle = TextStyles.Default;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(640, 0x217);
            base.Controls.Add(this.gpgTextBoxReplay);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgRadioButtonExploit);
            base.Controls.Add(this.gpgTextBoxPlayer);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.gpgRadioButtonCheat);
            base.Controls.Add(this.gpgRadioButtonDisconnect);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextAreaDetails);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(640, 0x217);
            base.Name = "DlgReportIssue";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Report an Issue";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextAreaDetails, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgRadioButtonDisconnect, 0);
            base.Controls.SetChildIndex(this.gpgRadioButtonCheat, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPlayer, 0);
            base.Controls.SetChildIndex(this.gpgRadioButtonExploit, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxReplay, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextAreaDetails.Properties.EndInit();
            this.gpgTextBoxPlayer.Properties.EndInit();
            this.gpgTextBoxReplay.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if ((this.gpgTextBoxPlayer.Text == null) || (this.gpgTextBoxPlayer.Text.Length < 1))
            {
                base.Error(this.gpgTextBoxPlayer, "You must provide a player name", new object[0]);
            }
            else
            {
                int @int = new QuazalQuery("GetPlayerIDFromName", new object[] { this.gpgTextBoxPlayer.Text }).GetInt();
                if (@int <= 0)
                {
                    base.Error(this.gpgTextBoxPlayer, "Unable to locate {0}", new object[] { this.gpgTextBoxPlayer.Text });
                }
                else if (new QuazalQuery("ReportIssue", new object[] { @int, DataAccess.FormatBool(this.gpgRadioButtonExploit.Checked), DataAccess.FormatBool(this.gpgRadioButtonCheat.Checked), DataAccess.FormatBool(this.gpgRadioButtonDisconnect.Checked), this.gpgTextBoxReplay.Text, this.gpgTextAreaDetails.Text }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("<LOC>This issue has been reported.");
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                else
                {
                    DlgMessage.ShowDialog("<LOC>An error occured while reporting this issue.");
                    base.DialogResult = DialogResult.Cancel;
                    base.Close();
                }
            }
        }
    }
}

