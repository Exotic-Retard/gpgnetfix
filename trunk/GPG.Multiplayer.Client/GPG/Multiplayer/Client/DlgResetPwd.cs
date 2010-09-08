namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgResetPwd : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabelConfirm;
        private GPGTextBox gpgTextBoxEmail;
        private GPGTextBox gpgTextBoxUser;
        private char[] PwdCharacters = new char[] { 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 
            'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 
            'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9'
         };
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private SkinButton skinButtonReset;

        public DlgResetPwd()
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
            this.gpgTextBoxUser = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxEmail = new GPGTextBox();
            this.skinButtonReset = new SkinButton();
            this.gpgLabelConfirm = new GPGLabel();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.gpgTextBoxUser.Properties.BeginInit();
            this.gpgTextBoxEmail.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x18, 0x65);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x6d, 0x10);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>Username";
            this.gpgLabel1.TextStyle = TextStyles.Title;
            this.gpgTextBoxUser.Location = new Point(0x1b, 120);
            this.gpgTextBoxUser.Name = "gpgTextBoxUser";
            this.gpgTextBoxUser.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxUser.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxUser.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxUser.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxUser.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxUser.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxUser.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxUser.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxUser.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxUser.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxUser.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxUser.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxUser.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxUser.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxUser.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxUser.Size = new Size(0x150, 20);
            this.gpgTextBoxUser.TabIndex = 8;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x18, 0x9d);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x87, 0x10);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "<LOC>Email Address";
            this.gpgLabel2.TextStyle = TextStyles.Title;
            this.gpgTextBoxEmail.Location = new Point(0x1b, 0xb0);
            this.gpgTextBoxEmail.Name = "gpgTextBoxEmail";
            this.gpgTextBoxEmail.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxEmail.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxEmail.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxEmail.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxEmail.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxEmail.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxEmail.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxEmail.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxEmail.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxEmail.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxEmail.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxEmail.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxEmail.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxEmail.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxEmail.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxEmail.Size = new Size(0x150, 20);
            this.gpgTextBoxEmail.TabIndex = 10;
            this.skinButtonReset.AutoStyle = true;
            this.skinButtonReset.BackColor = Color.Black;
            this.skinButtonReset.DialogResult = DialogResult.OK;
            this.skinButtonReset.DisabledForecolor = Color.Gray;
            this.skinButtonReset.DrawEdges = true;
            this.skinButtonReset.FocusColor = Color.Yellow;
            this.skinButtonReset.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonReset.ForeColor = Color.White;
            this.skinButtonReset.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonReset.IsStyled = true;
            this.skinButtonReset.Location = new Point(0x87, 0xd3);
            this.skinButtonReset.Name = "skinButtonReset";
            this.skinButtonReset.Size = new Size(0x7d, 0x1a);
            this.skinButtonReset.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonReset.TabIndex = 11;
            this.skinButtonReset.Text = "<LOC>Reset Password";
            this.skinButtonReset.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonReset.TextPadding = new Padding(0);
            this.skinButtonReset.Click += new EventHandler(this.skinButtonReset_Click);
            this.gpgLabelConfirm.AutoStyle = true;
            this.gpgLabelConfirm.Font = new Font("Arial", 9.75f);
            this.gpgLabelConfirm.ForeColor = Color.White;
            this.gpgLabelConfirm.IgnoreMouseWheel = false;
            this.gpgLabelConfirm.IsStyled = false;
            this.gpgLabelConfirm.Location = new Point(0x18, 0xd3);
            this.gpgLabelConfirm.Name = "gpgLabelConfirm";
            this.gpgLabelConfirm.Size = new Size(0x153, 0x35);
            this.gpgLabelConfirm.TabIndex = 12;
            this.gpgLabelConfirm.Text = "<LOC>Your password was reset and sent to the email address used at registration. Please check your email account.";
            this.gpgLabelConfirm.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabelConfirm.TextStyle = TextStyles.Status;
            this.gpgLabelConfirm.Visible = false;
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
            this.skinButtonCancel.Location = new Point(0x10a, 0xd3);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x61, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 13;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
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
            this.skinButtonOK.Location = new Point(0x93, 0x110);
            this.skinButtonOK.MaximumSize = new Size(100, 0x1a);
            this.skinButtonOK.MinimumSize = new Size(100, 0x1a);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(100, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 14;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Visible = false;
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x187, 0x162);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonReset);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxEmail);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextBoxUser);
            base.Controls.Add(this.gpgLabelConfirm);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x187, 0x162);
            base.Name = "DlgResetPwd";
            this.Text = "<LOC>Reset Password";
            base.Controls.SetChildIndex(this.gpgLabelConfirm, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxUser, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxEmail, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.skinButtonReset, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            this.gpgTextBoxUser.Properties.EndInit();
            this.gpgTextBoxEmail.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextBoxUser.Select();
            this.gpgTextBoxUser.SelectionStart = this.gpgTextBoxUser.Text.Length;
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void skinButtonReset_Click(object sender, EventArgs e)
        {
            this.skinButtonReset.Enabled = false;
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                VGen0 method = null;
                VGen0 gen2 = null;
                VGen0 gen3 = null;
                string username = this.gpgTextBoxUser.Text + "_t_e_m_p_";
                string pwd = this.gpgTextBoxUser.Text + "_p_a_s_s_";
                if (!User.Login(username, pwd, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort) && User.CreateLogin(username, pwd, this.gpgTextBoxEmail.Text, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort))
                {
                    User.Login(username, pwd, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort);
                }
                DataList queryData = null;
                if (User.GetProtocol() == null)
                {
                    queryData = DataAccess.GetQueryData("CheckResetInfo", new object[] { this.gpgTextBoxUser.Text, this.gpgTextBoxEmail.Text });
                }
                else
                {
                    queryData = new DataList();
                    queryData.Add(new DataRecord(new string[0], new string[0]));
                }
                if (queryData.Count > 0)
                {
                    if (User.ResetPassword(this.gpgTextBoxUser.Text, this.gpgTextBoxEmail.Text))
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.skinButtonReset.Hide();
                                this.skinButtonCancel.Hide();
                                this.skinButtonOK.Show();
                                this.gpgLabelConfirm.Show();
                                base.AcceptButton = this.skinButtonOK;
                                base.CancelButton = this.skinButtonOK;
                            };
                        }
                        base.Invoke(method);
                    }
                    else
                    {
                        if (gen2 == null)
                        {
                            gen2 = delegate {
                                DlgMessage.ShowDialog(Loc.Get("<LOC>Your account name and email address do not match."));
                                this.skinButtonReset.Enabled = true;
                            };
                        }
                        base.Invoke(gen2);
                    }
                }
                else
                {
                    if (gen3 == null)
                    {
                        gen3 = delegate {
                            DlgMessage.ShowDialog(Loc.Get("<LOC>Your account name and email address do not match."));
                            this.skinButtonReset.Enabled = true;
                        };
                    }
                    base.Invoke(gen3);
                }
            }, new object[0]);
        }
    }
}

