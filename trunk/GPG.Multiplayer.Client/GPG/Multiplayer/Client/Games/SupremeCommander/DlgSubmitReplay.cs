namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.IO.Compression;
    using System.Reflection;
    using System.Windows.Forms;

    public class DlgSubmitReplay : DlgBase
    {
        private SkinButton btnCancel;
        private SkinButton btnOK;
        private IContainer components = null;
        private GPGCheckBox gpgCheckBoxShowAgain;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel10;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGLabel lGameType;
        private GPGLabel lMapName;
        private GPGLabel lOpponent;
        private int mGameLength;
        private SupcomLookups._GameTypes mGameType;
        private SupcomLookups._Factions mOpponentFaction = SupcomLookups._Factions.Any;
        private SupcomLookups._Factions mPlayerFaction = SupcomLookups._Factions.Any;
        private byte[] mReplay = null;
        private GPGTextArea tbGameInformation;
        private GPGTextBox tbKeywords;
        private GPGTextBox tbTitle;

        public DlgSubmitReplay()
        {
            EventLog.WriteLine("Creating replay dialog.", new object[0]);
            this.InitializeComponent();
            EventLog.DoStackTrace();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            if (Profanity.ContainsProfanity(this.tbGameInformation.Text))
            {
                base.Error(this.tbGameInformation, "<LOC>Game description and title cannot contain profanity.", new object[0]);
            }
            else if (Profanity.ContainsProfanity(this.tbTitle.Text))
            {
                base.Error(this.tbGameInformation, "<LOC>Game description and title cannot contain profanity.", new object[0]);
            }
            else
            {
                ThreadQueue.QueueUserWorkItem(delegate (object o) {
                    try
                    {
                        object[] objArray = o as object[];
                        string str = objArray[0].ToString();
                        string str2 = objArray[1].ToString();
                        string str3 = objArray[2].ToString();
                        string str4 = objArray[3].ToString();
                        string str5 = objArray[4].ToString();
                        string str6 = objArray[5].ToString();
                        int num = (int) objArray[7];
                        int num2 = (int) objArray[8];
                        int num3 = (int) objArray[9];
                        int num4 = (int) objArray[10];
                        byte[] buffer = objArray[6] as byte[];
                        MemoryStream stream = new MemoryStream();
                        GZipStream stream2 = new GZipStream(stream, CompressionMode.Compress, true);
                        stream2.Write(buffer, 0, buffer.Length);
                        stream2.Close();
                        stream.Position = 0L;
                        byte[] buffer2 = new byte[(int) stream.Length];
                        stream.Read(buffer2, 0, (int) stream.Length);
                        string str7 = Game.SubmitGame(User.Current.Name, buffer2);
                        stream.Close();
                        string str8 = str3;
                        if ((((str8 != "Custom Game") && (str8 != "1v1 Ranked")) && ((str8 != "2v2 Ranked") && (str8 != "3v3 Ranked"))) && (str8 != "4v4 Ranked"))
                        {
                            str8 = "Custom Game";
                        }
                        string str9 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                        if (ConfigSettings.GetBool("DoOldGameList", false))
                        {
                            DataAccess.ExecuteQuery("SubmitSupcomReplay1", new object[] { str, str2, str8, str4, str5, str6, str7, str9, num, num2, num3, num4 });
                        }
                        else
                        {
                            DataAccess.ExecuteQuery("SubmitSupcomReplay2", new object[] { str, str2, str8, str4, str5, str6, str7, str9, GPGnetSelectedGame.SelectedGame.GameID, num, num2, num3, num4, GPGnetSelectedGame.SelectedGame.GameID });
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }, new object[] { SupcomLookups.TranslateMapName(this.lMapName.Text), this.lOpponent.Text, this.lGameType.Text, this.tbTitle.Text, this.tbGameInformation.Text, this.tbKeywords.Text, this.mReplay, (int) this.GameType, (int) this.PlayerFaction, (int) this.OpponentFaction, this.GameLength });
                base.Close();
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

        private void gpgCheckBoxShowAgain_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.SupcomPrefs.Replays.ShowReplayDialog = !this.gpgCheckBoxShowAgain.Checked;
        }

        private void InitializeComponent()
        {
            this.tbTitle = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.lMapName = new GPGLabel();
            this.lOpponent = new GPGLabel();
            this.lGameType = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.tbKeywords = new GPGTextBox();
            this.gpgLabel9 = new GPGLabel();
            this.tbGameInformation = new GPGTextArea();
            this.btnOK = new SkinButton();
            this.btnCancel = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.gpgCheckBoxShowAgain = new GPGCheckBox();
            this.gpgLabel10 = new GPGLabel();
            this.tbTitle.Properties.BeginInit();
            this.tbKeywords.Properties.BeginInit();
            this.tbGameInformation.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.tbTitle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbTitle.EditValue = "Unknown vs Unknown";
            this.tbTitle.Location = new Point(0xc4, 0xed);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Properties.Appearance.BackColor = Color.Black;
            this.tbTitle.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbTitle.Properties.Appearance.ForeColor = Color.White;
            this.tbTitle.Properties.Appearance.Options.UseBackColor = true;
            this.tbTitle.Properties.Appearance.Options.UseBorderColor = true;
            this.tbTitle.Properties.Appearance.Options.UseForeColor = true;
            this.tbTitle.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbTitle.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbTitle.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbTitle.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbTitle.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbTitle.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbTitle.Properties.BorderStyle = BorderStyles.Simple;
            this.tbTitle.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbTitle.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbTitle.Size = new Size(0x13b, 20);
            this.tbTitle.TabIndex = 8;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x24, 80);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x87, 0x10);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "<LOC>Submit Replay";
            this.gpgLabel2.TextStyle = TextStyles.Header1;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x24, 0x95);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x75, 0x10);
            this.gpgLabel3.TabIndex = 10;
            this.gpgLabel3.Text = "<LOC>Map Name:";
            this.gpgLabel3.TextStyle = TextStyles.Title;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x24, 0xb1);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x86, 0x10);
            this.gpgLabel4.TabIndex = 11;
            this.gpgLabel4.Text = "<LOC>Other Players:";
            this.gpgLabel4.TextStyle = TextStyles.Title;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x24, 0xd3);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x79, 0x10);
            this.gpgLabel5.TabIndex = 12;
            this.gpgLabel5.Text = "<LOC>Game Type:";
            this.gpgLabel5.TextStyle = TextStyles.Title;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0xee, 0x106);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0, 0x10);
            this.gpgLabel6.TabIndex = 13;
            this.gpgLabel6.TextStyle = TextStyles.Default;
            this.lMapName.AutoSize = true;
            this.lMapName.AutoStyle = true;
            this.lMapName.Font = new Font("Arial", 9.75f);
            this.lMapName.ForeColor = Color.White;
            this.lMapName.IgnoreMouseWheel = false;
            this.lMapName.IsStyled = false;
            this.lMapName.Location = new Point(0xc1, 0x95);
            this.lMapName.Name = "lMapName";
            this.lMapName.Size = new Size(90, 0x10);
            this.lMapName.TabIndex = 14;
            this.lMapName.Text = "Unknown Map";
            this.lMapName.TextStyle = TextStyles.Default;
            this.lOpponent.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lOpponent.AutoStyle = true;
            this.lOpponent.Font = new Font("Arial", 9.75f);
            this.lOpponent.ForeColor = Color.White;
            this.lOpponent.IgnoreMouseWheel = false;
            this.lOpponent.IsStyled = false;
            this.lOpponent.Location = new Point(0xc1, 0xb1);
            this.lOpponent.Name = "lOpponent";
            this.lOpponent.Size = new Size(0x13e, 0x2c);
            this.lOpponent.TabIndex = 15;
            this.lOpponent.Text = "Unknown";
            this.lOpponent.TextStyle = TextStyles.Default;
            this.lGameType.AutoSize = true;
            this.lGameType.AutoStyle = true;
            this.lGameType.Font = new Font("Arial", 9.75f);
            this.lGameType.ForeColor = Color.White;
            this.lGameType.IgnoreMouseWheel = false;
            this.lGameType.IsStyled = false;
            this.lGameType.Location = new Point(0xc1, 0xd3);
            this.lGameType.Name = "lGameType";
            this.lGameType.Size = new Size(0x85, 0x10);
            this.lGameType.TabIndex = 0x10;
            this.lGameType.Text = "<LOC>Ranked Game";
            this.lGameType.TextStyle = TextStyles.Default;
            this.gpgLabel7.AutoSize = true;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x24, 0xf1);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x75, 0x10);
            this.gpgLabel7.TabIndex = 0x11;
            this.gpgLabel7.Text = "<LOC>Game Title:";
            this.gpgLabel7.TextStyle = TextStyles.Title;
            this.gpgLabel8.AutoSize = true;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.Font = new Font("Arial", 9.75f);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x24, 0x10c);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x95, 0x10);
            this.gpgLabel8.TabIndex = 0x12;
            this.gpgLabel8.Text = "<LOC>Keyword Search:";
            this.gpgLabel8.TextStyle = TextStyles.Title;
            this.tbKeywords.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbKeywords.EditValue = "";
            this.tbKeywords.Location = new Point(0xc4, 0x108);
            this.tbKeywords.Name = "tbKeywords";
            this.tbKeywords.Properties.Appearance.BackColor = Color.Black;
            this.tbKeywords.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbKeywords.Properties.Appearance.ForeColor = Color.White;
            this.tbKeywords.Properties.Appearance.Options.UseBackColor = true;
            this.tbKeywords.Properties.Appearance.Options.UseBorderColor = true;
            this.tbKeywords.Properties.Appearance.Options.UseForeColor = true;
            this.tbKeywords.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbKeywords.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbKeywords.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbKeywords.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbKeywords.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbKeywords.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbKeywords.Properties.BorderStyle = BorderStyles.Simple;
            this.tbKeywords.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbKeywords.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbKeywords.Size = new Size(0x13b, 20);
            this.tbKeywords.TabIndex = 0x13;
            this.gpgLabel9.AutoSize = true;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.Font = new Font("Arial", 9.75f);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0x24, 0x126);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x9c, 0x10);
            this.gpgLabel9.TabIndex = 20;
            this.gpgLabel9.Text = "<LOC>Game Information:";
            this.gpgLabel9.TextStyle = TextStyles.Title;
            this.tbGameInformation.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tbGameInformation.Location = new Point(0xc4, 0x126);
            this.tbGameInformation.Name = "tbGameInformation";
            this.tbGameInformation.Properties.Appearance.BackColor = Color.Black;
            this.tbGameInformation.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbGameInformation.Properties.Appearance.ForeColor = Color.White;
            this.tbGameInformation.Properties.Appearance.Options.UseBackColor = true;
            this.tbGameInformation.Properties.Appearance.Options.UseBorderColor = true;
            this.tbGameInformation.Properties.Appearance.Options.UseForeColor = true;
            this.tbGameInformation.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbGameInformation.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbGameInformation.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbGameInformation.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbGameInformation.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbGameInformation.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbGameInformation.Properties.BorderStyle = BorderStyles.Simple;
            this.tbGameInformation.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbGameInformation.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbGameInformation.Properties.MaxLength = 0xa00;
            this.tbGameInformation.Size = new Size(0x13b, 0x60);
            this.tbGameInformation.TabIndex = 0x15;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.AutoStyle = true;
            this.btnOK.BackColor = Color.Black;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.DisabledForecolor = Color.Gray;
            this.btnOK.DrawEdges = true;
            this.btnOK.FocusColor = Color.Yellow;
            this.btnOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.HorizontalScalingMode = ScalingModes.Tile;
            this.btnOK.IsStyled = true;
            this.btnOK.Location = new Point(0x134, 0x1b6);
            this.btnOK.MaximumSize = new Size(100, 0x1a);
            this.btnOK.MinimumSize = new Size(100, 0x1a);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(100, 0x1a);
            this.btnOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnOK.TabIndex = 0x16;
            this.btnOK.Text = "<LOC>OK";
            this.btnOK.TextAlign = ContentAlignment.MiddleCenter;
            this.btnOK.TextPadding = new Padding(0);
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.AutoStyle = true;
            this.btnCancel.BackColor = Color.Black;
            this.btnCancel.DialogResult = DialogResult.OK;
            this.btnCancel.DisabledForecolor = Color.Gray;
            this.btnCancel.DrawEdges = true;
            this.btnCancel.FocusColor = Color.Yellow;
            this.btnCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCancel.IsStyled = true;
            this.btnCancel.Location = new Point(0x19e, 0x1b6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x61, 0x1a);
            this.btnCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "<LOC>Cancel";
            this.btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCancel.TextPadding = new Padding(0);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x24, 0x6c);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x1db, 0x39);
            this.gpgLabel1.TabIndex = 0x17;
            this.gpgLabel1.Text = "<LOC>You can optionally save a replay of your game into the GPGnet archives.  To submit your replay, please fill out some additional information about your game.";
            this.gpgLabel1.TextStyle = TextStyles.Descriptor;
            this.gpgCheckBoxShowAgain.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgCheckBoxShowAgain.Location = new Point(0x27, 0x18c);
            this.gpgCheckBoxShowAgain.Name = "gpgCheckBoxShowAgain";
            this.gpgCheckBoxShowAgain.Size = new Size(0xf4, 0x19);
            this.gpgCheckBoxShowAgain.TabIndex = 0x18;
            this.gpgCheckBoxShowAgain.Text = "<LOC>Do not show this again.";
            this.gpgCheckBoxShowAgain.UseVisualStyleBackColor = true;
            this.gpgCheckBoxShowAgain.CheckedChanged += new EventHandler(this.gpgCheckBoxShowAgain_CheckedChanged);
            this.gpgLabel10.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.Font = new Font("Arial", 9.75f);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(0x27, 0x1a1);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0xf4, 50);
            this.gpgLabel10.TabIndex = 0x19;
            this.gpgLabel10.Text = "<LOC id=_b8bd5693e2dd4404941e76ed2a0d684d> (This dialog can be re-enabled via menu item Tools-> Options-> Supreme Commander)";
            this.gpgLabel10.TextStyle = TextStyles.Descriptor;
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x228, 0x20f);
            base.Controls.Add(this.gpgLabel10);
            base.Controls.Add(this.gpgCheckBoxShowAgain);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.tbGameInformation);
            base.Controls.Add(this.gpgLabel9);
            base.Controls.Add(this.tbKeywords);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.lGameType);
            base.Controls.Add(this.lOpponent);
            base.Controls.Add(this.lMapName);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.tbTitle);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x228, 0x20f);
            this.MinimumSize = new Size(0x228, 0x20f);
            base.Name = "DlgSubmitReplay";
            this.Text = "<LOC>Submit Replay";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.tbTitle, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.lMapName, 0);
            base.Controls.SetChildIndex(this.lOpponent, 0);
            base.Controls.SetChildIndex(this.lGameType, 0);
            base.Controls.SetChildIndex(this.gpgLabel7, 0);
            base.Controls.SetChildIndex(this.gpgLabel8, 0);
            base.Controls.SetChildIndex(this.tbKeywords, 0);
            base.Controls.SetChildIndex(this.gpgLabel9, 0);
            base.Controls.SetChildIndex(this.tbGameInformation, 0);
            base.Controls.SetChildIndex(this.btnOK, 0);
            base.Controls.SetChildIndex(this.btnCancel, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxShowAgain, 0);
            base.Controls.SetChildIndex(this.gpgLabel10, 0);
            this.tbTitle.Properties.EndInit();
            this.tbKeywords.Properties.EndInit();
            this.tbGameInformation.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void SetFile(string filename)
        {
            this.mReplay = File.ReadAllBytes(filename);
        }

        public void SetMap(string mapname)
        {
            this.lMapName.Text = SupcomLookups.TranslateMapCode(mapname);
        }

        public void SetOpponent(string opponentname)
        {
            this.lOpponent.Text = opponentname;
            this.tbTitle.Text = User.Current.Name + ", " + opponentname;
        }

        public int GameLength
        {
            get
            {
                return this.mGameLength;
            }
            set
            {
                this.mGameLength = value;
            }
        }

        internal SupcomLookups._GameTypes GameType
        {
            get
            {
                return this.mGameType;
            }
            set
            {
                this.mGameType = value;
                this.lGameType.Text = SupcomLookups.TranslateGameType(value);
            }
        }

        internal SupcomLookups._Factions OpponentFaction
        {
            get
            {
                return this.mOpponentFaction;
            }
            set
            {
                this.mOpponentFaction = value;
            }
        }

        internal SupcomLookups._Factions PlayerFaction
        {
            get
            {
                return this.mPlayerFaction;
            }
            set
            {
                this.mPlayerFaction = value;
            }
        }
    }
}

