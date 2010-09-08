namespace GPG.Multiplayer.Client.Games
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
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgNoBetaKey : DlgBase
    {
        private SkinButton btnChatOnly;
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabelError;
        private GPGTextBox gpgTextBoxAddBeta;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgNoBetaKey(FrmMain mainForm) : base(mainForm)
        {
            WaitCallback callBack = null;
            this.components = null;
            this.InitializeComponent();
            string str = "";
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\THQ\Gas Powered Games\" + GameInformation.SelectedGame.GameDescription);
                if (key != null)
                {
                    str = (string) key.GetValue("KEY", "");
                }
                if ((str == null) || (str == ""))
                {
                    key = Registry.LocalMachine.OpenSubKey(@"Software\THQ\Gas Powered Games\" + GameInformation.SelectedGame.GameDescription);
                    if (key != null)
                    {
                        str = (string) key.GetValue("KEY", "");
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            switch (str)
            {
                case null:
                case "":
                    str = "";
                    break;
            }
            if (str.IndexOf("-") < 0)
            {
                for (int i = (str.Length / 4) - 1; i >= 1; i--)
                {
                    str = str.Insert(i * 4, "-");
                }
            }
            this.gpgTextBoxAddBeta.Text = str;
            if (GameInformation.SelectedGame.GameID == -1)
            {
                base.DialogResult = DialogResult.OK;
            }
            if (callBack == null)
            {
                callBack = delegate (object o) {
                    try
                    {
                        DataList queryData = DataAccess.GetQueryData("Check For Other Keys", new object[0]);
                        if (queryData.Count > 0)
                        {
                            string names = "";
                            string curstr = "";
                            foreach (DataRecord record in queryData)
                            {
                                names = names + curstr + record["name"];
                                curstr = ", ";
                            }
                            mainForm.Invoke((VGen0)delegate {
                                try
                                {
                                    DlgMessage.ShowDialog(Loc.Get("<LOC>We have detected that you have other accounts with the same email that already have CD keys attached.  You may want to quit, and relog into GPGnet under one of these accounts to get the full functionality out of your games.  These accounts are: ") + names);
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            });
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                };
            }
            ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
        }

        private void btnChatOnly_Click(object sender, EventArgs e)
        {
            GameInformation.SelectedGame = GameInformation.GPGNetChat;
            base.DialogResult = DialogResult.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Error(string error)
        {
            this.gpgLabelError.Text = Loc.Get(error);
            this.gpgLabelError.Visible = true;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgNoBetaKey));
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgTextBoxAddBeta = new GPGTextBox();
            this.gpgLabelError = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.btnChatOnly = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxAddBeta.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x1e3, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = System.Drawing.Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x4b);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x20f, 0x66);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = manager.GetString("gpgLabel1.Text");
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = System.Drawing.Color.Black;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = System.Drawing.Color.Gray;
            this.skinButtonOK.DrawColor = System.Drawing.Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = System.Drawing.Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = System.Drawing.Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x7d, 240);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x8b, 0x16);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 8;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = System.Drawing.Color.Black;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = System.Drawing.Color.Gray;
            this.skinButtonCancel.DrawColor = System.Drawing.Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = System.Drawing.Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = System.Drawing.Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(270, 240);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(140, 0x16);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 9;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Quit";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgTextBoxAddBeta.Location = new Point(0x7d, 0xd6);
            this.gpgTextBoxAddBeta.Name = "gpgTextBoxAddBeta";
            this.gpgTextBoxAddBeta.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.gpgTextBoxAddBeta.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxAddBeta.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.gpgTextBoxAddBeta.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxAddBeta.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxAddBeta.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.FromArgb(0, 0, 0);
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxAddBeta.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxAddBeta.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxAddBeta.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxAddBeta.Size = new Size(0x11d, 20);
            this.gpgTextBoxAddBeta.TabIndex = 10;
            this.gpgLabelError.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelError.AutoStyle = false;
            this.gpgLabelError.Font = new Font("Arial", 9.75f);
            this.gpgLabelError.ForeColor = System.Drawing.Color.Red;
            this.gpgLabelError.IgnoreMouseWheel = false;
            this.gpgLabelError.IsStyled = false;
            this.gpgLabelError.Location = new Point(0x36, 290);
            this.gpgLabelError.Name = "gpgLabelError";
            this.gpgLabelError.Size = new Size(0x1b4, 0x17);
            base.ttDefault.SetSuperTip(this.gpgLabelError, null);
            this.gpgLabelError.TabIndex = 11;
            this.gpgLabelError.Text = "gpgLabel2";
            this.gpgLabelError.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelError.TextStyle = TextStyles.Default;
            this.gpgLabelError.Visible = false;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = System.Drawing.Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0xb1);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x206, 0x24);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 12;
            this.gpgLabel2.Text = "<LOC id=_84b726cb531d9c9a261d7bd723de3a73>Enter your CD key exactly as it was provided to you (e.g. XXXX-XXXX-XXXX-XXXX-XXXX).";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.btnChatOnly.AutoStyle = true;
            this.btnChatOnly.BackColor = System.Drawing.Color.Black;
            this.btnChatOnly.ButtonState = 0;
            this.btnChatOnly.DialogResult = DialogResult.OK;
            this.btnChatOnly.DisabledForecolor = System.Drawing.Color.Gray;
            this.btnChatOnly.DrawColor = System.Drawing.Color.White;
            this.btnChatOnly.DrawEdges = true;
            this.btnChatOnly.FocusColor = System.Drawing.Color.Yellow;
            this.btnChatOnly.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnChatOnly.ForeColor = System.Drawing.Color.White;
            this.btnChatOnly.HorizontalScalingMode = ScalingModes.Tile;
            this.btnChatOnly.IsStyled = true;
            this.btnChatOnly.Location = new Point(0x7d, 0x10c);
            this.btnChatOnly.Name = "btnChatOnly";
            this.btnChatOnly.Size = new Size(0x11d, 0x16);
            this.btnChatOnly.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnChatOnly, null);
            this.btnChatOnly.TabIndex = 13;
            this.btnChatOnly.TabStop = true;
            this.btnChatOnly.Text = "<LOC>I don't know my key but want to chat.";
            this.btnChatOnly.TextAlign = ContentAlignment.MiddleCenter;
            this.btnChatOnly.TextPadding = new Padding(0);
            this.btnChatOnly.Click += new EventHandler(this.btnChatOnly_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x21e, 0x161);
            base.Controls.Add(this.btnChatOnly);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabelError);
            base.Controls.Add(this.gpgTextBoxAddBeta);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x21e, 0x161);
            base.Name = "DlgNoBetaKey";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>CD Key Required";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxAddBeta, 0);
            base.Controls.SetChildIndex(this.gpgLabelError, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.btnChatOnly, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxAddBeta.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            string text;
            int num;
            if (ConfigSettings.GetBool("DoOldGameList", false))
            {
                text = this.gpgTextBoxAddBeta.Text;
                if (text.IndexOf("-") < 0)
                {
                    for (num = (text.Length / 4) - 1; num >= 1; num--)
                    {
                        text = text.Insert(num * 4, "-");
                    }
                    this.gpgTextBoxAddBeta.Text = text;
                }
                if ((this.gpgTextBoxAddBeta.Text == null) || (this.gpgTextBoxAddBeta.Text.Length < 1))
                {
                    this.Error(Loc.Get("<LOC>CD key cannot be blank."));
                }
                else if (this.gpgTextBoxAddBeta.Text.Replace("-", "").Length < 12)
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key (Too Short)."));
                }
                else if (this.gpgTextBoxAddBeta.Text.Replace("-", "").Length > 30)
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key (Too Long)."));
                }
                else if (!DataAccess.GetBool("BetaKeyExists", new object[] { this.gpgTextBoxAddBeta.Text }))
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key (Doesn't Exist)."));
                }
                else if (DataAccess.GetBool("IsBetaKeyInUse", new object[] { this.gpgTextBoxAddBeta.Text }))
                {
                    this.Error(Loc.Get("<LOC>That key is already in use."));
                }
                else if (DataAccess.ExecuteQuery("AddBetaKey", new object[] { this.gpgTextBoxAddBeta.Text }))
                {
                    GameKey.BetaKeys = DataAccess.GetObjects<GameKey>("GetBetaKeys", new object[0]);
                    for (num = 0; num < GameKey.BetaKeys.Count; num++)
                    {
                        GameKey.BetaKeys.IndexObject(GameKey.BetaKeys[num]);
                    }
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key."));
                }
            }
            else
            {
                text = this.gpgTextBoxAddBeta.Text;
                if (text.IndexOf("-") < 0)
                {
                    for (num = (text.Length / 4) - 1; num >= 1; num--)
                    {
                        text = text.Insert(num * 4, "-");
                    }
                    this.gpgTextBoxAddBeta.Text = text;
                }
                if ((this.gpgTextBoxAddBeta.Text == null) || (this.gpgTextBoxAddBeta.Text.Length < 1))
                {
                    this.Error(Loc.Get("<LOC>CD key cannot be blank."));
                }
                else if (this.gpgTextBoxAddBeta.Text.Replace("-", "").Length < 12)
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key (Too Short)."));
                }
                else if (this.gpgTextBoxAddBeta.Text.Replace("-", "").Length > 30)
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key (Too Long)."));
                }
                else if (!DataAccess.GetBool("BetaKeyExists2", new object[] { this.gpgTextBoxAddBeta.Text, GameInformation.SelectedGame.GameID }))
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key (Doesn't exist)."));
                }
                else if (DataAccess.GetBool("IsBetaKeyInUse2", new object[] { this.gpgTextBoxAddBeta.Text, GameInformation.SelectedGame.GameID }))
                {
                    this.Error(Loc.Get("<LOC>That key is already in use."));
                }
                else if (DataAccess.ExecuteQuery("AddBetaKey2", new object[] { this.gpgTextBoxAddBeta.Text, GameInformation.SelectedGame.GameID }))
                {
                    GameKey.BetaKeys = DataAccess.GetObjects<GameKey>("GetBetaKeys", new object[0]);
                    for (num = 0; num < GameKey.BetaKeys.Count; num++)
                    {
                        GameKey.BetaKeys.IndexObject(GameKey.BetaKeys[num]);
                    }
                    GameInformation.LoadGamesFromDB();
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key (Unable to add)."));
                }
            }
        }
    }
}

