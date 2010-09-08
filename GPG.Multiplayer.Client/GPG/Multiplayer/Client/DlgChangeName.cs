namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Clans;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgChangeName : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGTextBox gpgTextBoxNewName1;
        private GPGTextBox gpgTextBoxNewName2;
        private GPGTextBox gpgTextBoxPwd;
        private GPGTextBox gpgTextBoxUsername;
        private bool InProgress = false;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgChangeName()
        {
            this.InitializeComponent();
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
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxUsername = new GPGTextBox();
            this.gpgTextBoxPwd = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxNewName1 = new GPGTextBox();
            this.gpgLabel3 = new GPGLabel();
            this.gpgTextBoxNewName2 = new GPGTextBox();
            this.gpgLabel4 = new GPGLabel();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgLabel5 = new GPGLabel();
            this.gpgTextBoxUsername.Properties.BeginInit();
            this.gpgTextBoxPwd.Properties.BeginInit();
            this.gpgTextBoxNewName1.Properties.BeginInit();
            this.gpgTextBoxNewName2.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(9, 0x7a);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x9b, 0x10);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>Current Username";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxUsername.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxUsername.Location = new Point(12, 0x8d);
            this.gpgTextBoxUsername.Name = "gpgTextBoxUsername";
            this.gpgTextBoxUsername.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxUsername.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxUsername.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxUsername.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxUsername.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxUsername.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxUsername.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxUsername.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxUsername.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxUsername.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxUsername.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxUsername.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxUsername.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxUsername.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxUsername.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxUsername.Size = new Size(0x132, 20);
            this.gpgTextBoxUsername.TabIndex = 8;
            this.gpgTextBoxPwd.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxPwd.Location = new Point(12, 0xc4);
            this.gpgTextBoxPwd.Name = "gpgTextBoxPwd";
            this.gpgTextBoxPwd.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPwd.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPwd.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPwd.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPwd.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPwd.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPwd.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPwd.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPwd.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPwd.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPwd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPwd.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPwd.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPwd.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPwd.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPwd.Properties.PasswordChar = '*';
            this.gpgTextBoxPwd.Size = new Size(0x132, 20);
            this.gpgTextBoxPwd.TabIndex = 10;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(9, 0xb1);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x6b, 0x10);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "<LOC>Password";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgTextBoxNewName1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxNewName1.Location = new Point(12, 0x10b);
            this.gpgTextBoxNewName1.Name = "gpgTextBoxNewName1";
            this.gpgTextBoxNewName1.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxNewName1.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxNewName1.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxNewName1.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxNewName1.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxNewName1.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxNewName1.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxNewName1.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxNewName1.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxNewName1.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxNewName1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxNewName1.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxNewName1.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxNewName1.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxNewName1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxNewName1.Size = new Size(0x132, 20);
            this.gpgTextBoxNewName1.TabIndex = 12;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(9, 0xf8);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x8a, 0x10);
            this.gpgLabel3.TabIndex = 11;
            this.gpgLabel3.Text = "<LOC>New Username";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgTextBoxNewName2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxNewName2.Location = new Point(12, 320);
            this.gpgTextBoxNewName2.Name = "gpgTextBoxNewName2";
            this.gpgTextBoxNewName2.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxNewName2.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxNewName2.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxNewName2.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxNewName2.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxNewName2.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxNewName2.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxNewName2.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxNewName2.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxNewName2.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxNewName2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxNewName2.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxNewName2.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxNewName2.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxNewName2.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxNewName2.Size = new Size(0x132, 20);
            this.gpgTextBoxNewName2.TabIndex = 14;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(9, 0x12d);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0xba, 0x10);
            this.gpgLabel4.TabIndex = 13;
            this.gpgLabel4.Text = "<LOC>Confirm New Username";
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x9b, 0x179);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x62, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 15;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x103, 0x179);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x62, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 0x10;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 8f, FontStyle.Italic);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0xac, 80);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0xba, 0x20);
            this.gpgLabel5.TabIndex = 0x13;
            this.gpgLabel5.Text = "<LOC>Changing username will require an application restart";
            this.gpgLabel5.TextStyle = TextStyles.Descriptor;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x171, 480);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgTextBoxNewName2);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgTextBoxNewName1);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgTextBoxPwd);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxUsername);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgChangeName";
            this.Text = "<LOC>Change Username";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxUsername, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPwd, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxNewName1, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxNewName2, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            this.gpgTextBoxUsername.Properties.EndInit();
            this.gpgTextBoxPwd.Properties.EndInit();
            this.gpgTextBoxNewName1.Properties.EndInit();
            this.gpgTextBoxNewName2.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.InProgress)
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.InProgress = true;
                this.skinButtonOK.Enabled = false;
                this.skinButtonCancel.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                base.ClearErrors();
                bool flag = false;
                if ((this.gpgTextBoxUsername.Text == null) || (this.gpgTextBoxUsername.Text.Length < 3))
                {
                    base.Error(this.gpgTextBoxUsername, "<LOC>Name must be at least {0} characters.", new object[] { 3 });
                    flag = true;
                }
                if (this.gpgTextBoxUsername.Text.Length > 0x16)
                {
                    base.Error(this.gpgTextBoxUsername, "<LOC>Name cannot exceed {0} characters.", new object[] { 0x16 });
                    flag = true;
                }
                if ((this.gpgTextBoxPwd.Text == null) || (this.gpgTextBoxPwd.Text.Length < 6))
                {
                    base.Error(this.gpgTextBoxPwd, "<LOC>Password must be at least {0} characters.", new object[] { 6 });
                    flag = true;
                }
                if (this.gpgTextBoxNewName1.Text != this.gpgTextBoxNewName2.Text)
                {
                    base.Error(new Control[] { this.gpgTextBoxNewName1, this.gpgTextBoxNewName2 }, "<LOC>Names do not match, please re-enter name.", new object[0]);
                    flag = true;
                }
                else
                {
                    if ((this.gpgTextBoxNewName1.Text == null) || (this.gpgTextBoxNewName1.Text.Length < 3))
                    {
                        base.Error(new Control[] { this.gpgTextBoxNewName1, this.gpgTextBoxNewName2 }, "<LOC>Name must be at least {0} characters.", new object[] { 3 });
                        flag = true;
                    }
                    if (this.gpgTextBoxNewName1.Text.Length > 0x16)
                    {
                        base.Error(new Control[] { this.gpgTextBoxNewName1, this.gpgTextBoxNewName2 }, "<LOC>Name cannot exceed {0} characters.", new object[] { 0x16 });
                        flag = true;
                    }
                    if (!TextUtil.IsAlphaNumericString(this.gpgTextBoxNewName1.Text))
                    {
                        base.Error(new Control[] { this.gpgTextBoxNewName1, this.gpgTextBoxNewName2 }, "<LOC>Your username contains invalid characters, it may only contain letters, numbers, dashes and underbars.", new object[0]);
                        flag = true;
                    }
                    if (Profanity.ContainsProfanity(this.gpgTextBoxNewName1.Text))
                    {
                        base.Error(new Control[] { this.gpgTextBoxNewName1, this.gpgTextBoxNewName2 }, "<LOC>Usernames cannot contain profanity. Please enter a valid username.", new object[0]);
                        flag = true;
                    }
                }
                if (!flag)
                {
                    base.SetStatus("<LOC>Logging In...", new object[0]);
                    Thread.Sleep(500);
                    if (!User.Login(this.gpgTextBoxUsername.Text, this.gpgTextBoxPwd.Text, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort))
                    {
                        base.ClearStatus();
                        base.Error(new Control[] { this.gpgTextBoxUsername, this.gpgTextBoxPwd }, "<LOC>Invalid username/password.", new object[0]);
                    }
                    else if (DataAccess.GetQueryData("GetPlayerIDFromName", new object[] { this.gpgTextBoxNewName1.Text }).Count > 0)
                    {
                        base.ClearStatus();
                        base.Error(new Control[] { this.gpgTextBoxNewName1, this.gpgTextBoxNewName2 }, "<LOC>This name is already in use, please choose a unique name.", new object[0]);
                    }
                    else
                    {
                        base.SetStatus("<LOC>Updating Name...", new object[0]);
                        Thread.Sleep(500);
                        if (!DataAccess.ExecuteQuery("ChangeName", new object[] { this.gpgTextBoxNewName1.Text }))
                        {
                            base.ClearStatus();
                            base.Error(new Control[] { this.gpgTextBoxNewName1, this.gpgTextBoxNewName2 }, "<LOC>This name is already in use, please choose a unique name.", new object[0]);
                        }
                        else
                        {
                            DataAccess.ExecuteQuery("LogNameChange", new object[] { this.gpgTextBoxUsername.Text, this.gpgTextBoxNewName1.Text });
                            if (!new QuazalQuery("ChangeLadderName", new object[] { this.gpgTextBoxNewName1.Text, User.Current.ID }).ExecuteNonQuery())
                            {
                                ErrorLog.WriteLine("Failed to change entity name on ladders.", new object[0]);
                            }
                            User.Current.Name = this.gpgTextBoxNewName1.Text;
                            List<string> list = new List<string>();
                            MappedObjectList<User> objects = DataAccess.GetObjects<User>("GetFriendsByPlayerID", new object[] { User.Current.ID });
                            foreach (User user in objects)
                            {
                                if (user.Online)
                                {
                                    list.Add(user.Name);
                                }
                            }
                            if (User.Current.IsInClan)
                            {
                                Clan clan = DataAccess.GetObject<Clan>("GetClanByMember2", new object[] { User.Current.Name });
                                MappedObjectList<ClanMember> list3 = DataAccess.GetObjects<ClanMember>("GetClanMembers", new object[] { clan.ID });
                                foreach (ClanMember member in list3)
                                {
                                    if (!(!member.Online || list.Contains(member.Name)))
                                    {
                                        list.Add(member.Name);
                                    }
                                }
                            }
                            Messaging.SendCustomCommand(list.ToArray(), CustomCommands.ChangeName, new object[0]);
                            if (!ThreadQueue.Quazal.WaitUntilEmpty(0x1770))
                            {
                                ErrorLog.WriteLine("Error informing all clan members/friends of name change", new object[0]);
                            }
                            Messaging.SendCustomCommand(list.ToArray(), CustomCommands.OnlineStatus, new object[] { 0 });
                            if (!ThreadQueue.Quazal.WaitUntilEmpty(0x1770))
                            {
                                ErrorLog.WriteLine("Error informing all clan members/friends of name change", new object[0]);
                            }
                            Program.Settings.Login.DefaultUsername = "";
                            Program.Settings.Login.DefaultPassword = "";
                            Program.Settings.Save();
                            base.SetStatus("<LOC>Logging Out...", new object[0]);
                            Thread.Sleep(500);
                            User.Logout();
                            base.SetStatus("<LOC>Restarting...", new object[0]);
                            Thread.Sleep(500);
                            Application.Restart();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                base.Error("<LOC>An error occured changing name, please try again later.", new object[0]);
            }
            finally
            {
                if (User.LoggedIn)
                {
                    base.SetStatus("<LOC>Logging Out...", new object[0]);
                    Thread.Sleep(500);
                    User.Logout();
                }
                base.ClearStatus();
                this.Cursor = Cursors.Default;
                this.skinButtonOK.Enabled = true;
                this.skinButtonCancel.Enabled = true;
                this.InProgress = false;
            }
        }
    }
}

