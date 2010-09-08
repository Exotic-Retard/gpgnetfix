namespace GPG.Multiplayer.Client.Clans
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgCreateClan : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGTextBox gpgTextBoxAbbr;
        private GPGTextBox gpgTextBoxName;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgCreateClan(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gpgTextBoxName.Focus();
        }

        private void ClearSavedClanData()
        {
            DataAccess.ExecuteQuery("ClearClanRequests", new object[0]);
            DataAccess.ExecuteQuery("ClearClanInvites", new object[0]);
        }

        private void CreateClan(string name, string abbr)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.OnCreateClan), new object[] { name, abbr });
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
            this.gpgTextBoxName = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.gpgTextBoxAbbr = new GPGTextBox();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgTextBoxName.Properties.BeginInit();
            this.gpgTextBoxAbbr.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgTextBoxName.Location = new Point(0x17, 0x72);
            this.gpgTextBoxName.Name = "gpgTextBoxName";
            this.gpgTextBoxName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxName.Properties.MaxLength = 0x16;
            this.gpgTextBoxName.Size = new Size(0x141, 20);
            this.gpgTextBoxName.TabIndex = 1;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(20, 0x5f);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x54, 0x10);
            this.gpgLabel1.TabIndex = 5;
            this.gpgLabel1.Text = "<LOC>Name";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 8f, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(200, 0x61);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x90, 14);
            this.gpgLabel2.TabIndex = 6;
            this.gpgLabel2.Text = "<LOC>(Max 22 Characters)";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 8f, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(200, 0x98);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x8a, 14);
            this.gpgLabel3.TabIndex = 9;
            this.gpgLabel3.Text = "<LOC>(Max 3 Characters)";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(20, 150);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(120, 0x10);
            this.gpgLabel4.TabIndex = 8;
            this.gpgLabel4.Text = "<LOC>Abbreviation";
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.gpgTextBoxAbbr.Location = new Point(0x17, 0xa9);
            this.gpgTextBoxAbbr.Name = "gpgTextBoxAbbr";
            this.gpgTextBoxAbbr.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxAbbr.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxAbbr.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxAbbr.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxAbbr.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxAbbr.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxAbbr.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxAbbr.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxAbbr.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxAbbr.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxAbbr.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxAbbr.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxAbbr.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxAbbr.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxAbbr.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxAbbr.Properties.MaxLength = 3;
            this.gpgTextBoxAbbr.Size = new Size(0x141, 20);
            this.gpgTextBoxAbbr.TabIndex = 2;
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
            this.skinButtonOK.Location = new Point(0xae, 0xd8);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x4b, 0x17);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 11;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
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
            this.skinButtonCancel.Location = new Point(0x10d, 0xd8);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x4b, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 12;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(380, 0x12b);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgTextBoxAbbr);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextBoxName);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(380, 0x12b);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(380, 0x12b);
            base.Name = "DlgCreateClan";
            this.Text = "<LOC>Create New Clan";
            base.Controls.SetChildIndex(this.gpgTextBoxName, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxAbbr, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            this.gpgTextBoxName.Properties.EndInit();
            this.gpgTextBoxAbbr.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void OnCreateClan(object s)
        {
            VGen0 method = null;
            VGen0 gen2 = null;
            VGen0 gen3 = null;
            object[] objArray = s as object[];
            string name = (string) objArray[0];
            string str = (string) objArray[1];
            try
            {
                if (!DataAccess.ExecuteQuery("CreateClan", new object[] { name, str }))
                {
                    base.Error(this.skinButtonOK, "<LOC>Unable to create clan.", new object[0]);
                    this.skinButtonCancel.Enabled = true;
                    this.skinButtonOK.Enabled = true;
                }
                else
                {
                    int number = 0;
                    int tickCount = Environment.TickCount;
                    do
                    {
                        number = DataAccess.GetNumber("GetClanIDByName", new object[] { name });
                    }
                    while (((number < 1) && this.Sleep()) && ((Environment.TickCount - tickCount) < 0x1388));
                    if (number < 1)
                    {
                        DataAccess.ExecuteQuery("RemoveClanByName", new object[] { name });
                        base.Error(this.skinButtonOK, "<LOC>An error occured while creating the clan, please try again later", new object[0]);
                        this.skinButtonCancel.Enabled = true;
                        this.skinButtonOK.Enabled = true;
                    }
                    else if (!DataAccess.ExecuteQuery("JoinClan", new object[] { User.Current.ID, number, ClanRanking.MaxValue.Seniority }))
                    {
                        if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    DataAccess.ExecuteQuery("RemoveClanByName", new object[] { name });
                                    this.Error(this.skinButtonOK, "<LOC>Unable to join created clan.", new object[0]);
                                    this.skinButtonCancel.Enabled = true;
                                    this.skinButtonOK.Enabled = true;
                                };
                            }
                            base.BeginInvoke(method);
                        }
                    }
                    else
                    {
                        User.Current.ClanName = name;
                        User.Current.ClanAbbreviation = str;
                        base.SetStatus("<LOC>Creating clan chatroom...", new object[0]);
                        if (!DataAccess.ExecuteQuery("CreateClanChatroom", new object[] { name, name }))
                        {
                            base.Error(this.skinButtonOK, "<LOC>Clan created, unable to create clan chatroom.", new object[0]);
                        }
                        this.ClearSavedClanData();
                        base.ClearStatus();
                        base.SetStatus("<LOC>Clan created successfully.", new object[0]);
                        Thread.Sleep(0x3e8);
                        if (!base.Disposing && !base.IsDisposed)
                        {
                            if (gen3 == null)
                            {
                                gen3 = delegate {
                                    base.DialogResult = DialogResult.OK;
                                    base.Close();
                                };
                            }
                            base.BeginInvoke(gen3);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                if (gen2 == null)
                {
                    gen2 = delegate {
                        DataAccess.ExecuteQuery("RemoveClanByName", new object[] { name });
                        this.Error(this.skinButtonOK, "<LOC>An error occured while creating the clan, please try again later", new object[0]);
                        this.skinButtonCancel.Enabled = true;
                        this.skinButtonOK.Enabled = true;
                    };
                }
                base.BeginInvoke(gen2);
            }
            finally
            {
                base.ClearStatus();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextBoxName.Select();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            this.TryCreateClan();
        }

        private bool Sleep()
        {
            Thread.Sleep(500);
            return true;
        }

        private bool TryCreateClan()
        {
            try
            {
                base.ClearErrors();
                if (((User.Current.IsInClan || (Clan.Current != null)) || (ClanMember.Current != null)) || (DataAccess.GetCount("Count_ClanMemberByID", new object[] { User.Current.ID }) > 0))
                {
                    base.Error(this.skinButtonOK, "<LOC>You are already in a clan and cannot create a new one at this time.", new object[0]);
                    return false;
                }
                string text = this.gpgTextBoxName.Text;
                string str2 = this.gpgTextBoxAbbr.Text;
                if (text.Trim().Length < 1)
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Your clan name cannot be blank.", new object[0]);
                    return false;
                }
                if (text.Length > 0x16)
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Your clan name cannot be longer than 22 characters.", new object[0]);
                    return false;
                }
                if (str2.Trim().Length < 1)
                {
                    base.Error(this.gpgTextBoxAbbr, "<LOC>Your clan abbreviation cannot be blank.", new object[0]);
                    return false;
                }
                if (str2.Length > 3)
                {
                    base.Error(this.gpgTextBoxAbbr, "<LOC>Your clan abbreviation cannot be more than three characters.", new object[0]);
                    return false;
                }
                if (!TextUtil.IsAlphaNumericString(text, new char[] { ' ', '_', '-' }))
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Contains invalid character(s).", new object[0]);
                    return false;
                }
                if (!TextUtil.IsAlphaNumericString(str2, new char[] { '_' }))
                {
                    base.Error(this.gpgTextBoxAbbr, "<LOC>Contains invalid character(s).", new object[0]);
                    return false;
                }
                if (Profanity.ContainsProfanity(text))
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Your clan name and/or clan abbreviation cannot contain profanity.", new object[0]);
                    return false;
                }
                if (Profanity.ContainsProfanity(str2))
                {
                    base.Error(this.gpgTextBoxAbbr, "<LOC>Your clan name and/or clan abbreviation cannot contain profanity.", new object[0]);
                    return false;
                }
                if (DataAccess.GetCount("Count_ClanByName", new object[] { text }) > 0)
                {
                    base.Error(this.gpgTextBoxName, "<LOC id=_a3cc078a121c4fb310f48e2d39d25748>Sorry, that clan name is already taken.", new object[0]);
                    return false;
                }
                if (DataAccess.GetCount("Count_ClanByAbbr", new object[] { str2 }) > 0)
                {
                    base.Error(this.gpgTextBoxAbbr, "<LOC>Sorry, that clan abbreviation is already taken.", new object[0]);
                    return false;
                }
                base.SetStatus("<LOC>Creating clan...", new object[0]);
                this.skinButtonCancel.Enabled = false;
                this.skinButtonOK.Enabled = false;
                this.CreateClan(text, str2);
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                base.Error(this.skinButtonOK, "<LOC>An error occured while creating the clan, please try again later", new object[0]);
                return false;
            }
        }
    }
}

