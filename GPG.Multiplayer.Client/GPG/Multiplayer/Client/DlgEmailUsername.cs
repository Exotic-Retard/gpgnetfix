namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgEmailUsername : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private SkinButton skinButtonReset;
        private GPGTextBox tbCDKey;

        public DlgEmailUsername()
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

        private void DlgEmailUsername_AfterShown(FrmBase form)
        {
            this.skinButtonOK.Visible = false;
        }

        private void InitializeComponent()
        {
            this.skinButtonCancel = new SkinButton();
            this.skinButtonReset = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.tbCDKey = new GPGTextBox();
            this.skinButtonReset.SuspendLayout();
            this.tbCDKey.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
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
            this.skinButtonCancel.Location = new Point(0x112, 0x8a);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x61, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 0x13;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonReset.AutoStyle = true;
            this.skinButtonReset.BackColor = Color.Black;
            this.skinButtonReset.Controls.Add(this.skinButtonOK);
            this.skinButtonReset.DialogResult = DialogResult.OK;
            this.skinButtonReset.DisabledForecolor = Color.Gray;
            this.skinButtonReset.DrawEdges = true;
            this.skinButtonReset.FocusColor = Color.Yellow;
            this.skinButtonReset.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonReset.ForeColor = Color.White;
            this.skinButtonReset.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonReset.IsStyled = true;
            this.skinButtonReset.Location = new Point(0x8f, 0x8a);
            this.skinButtonReset.Name = "skinButtonReset";
            this.skinButtonReset.Size = new Size(0x7d, 0x1a);
            this.skinButtonReset.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonReset.TabIndex = 0x12;
            this.skinButtonReset.Text = "<LOC>Email Username";
            this.skinButtonReset.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonReset.TextPadding = new Padding(0);
            this.skinButtonReset.Click += new EventHandler(this.skinButtonReset_Click);
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
            this.skinButtonOK.Location = new Point(4, 0);
            this.skinButtonOK.MaximumSize = new Size(100, 0x1a);
            this.skinButtonOK.MinimumSize = new Size(100, 0x1a);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(100, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 20;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x1f, 0x5d);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x5f, 0x10);
            this.gpgLabel1.TabIndex = 14;
            this.gpgLabel1.Text = "<LOC>CD Key";
            this.gpgLabel1.TextStyle = TextStyles.Title;
            this.tbCDKey.Location = new Point(0x22, 0x70);
            this.tbCDKey.Name = "tbCDKey";
            this.tbCDKey.Properties.Appearance.BackColor = Color.Black;
            this.tbCDKey.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbCDKey.Properties.Appearance.ForeColor = Color.White;
            this.tbCDKey.Properties.Appearance.Options.UseBackColor = true;
            this.tbCDKey.Properties.Appearance.Options.UseBorderColor = true;
            this.tbCDKey.Properties.Appearance.Options.UseForeColor = true;
            this.tbCDKey.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbCDKey.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbCDKey.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbCDKey.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbCDKey.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbCDKey.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbCDKey.Properties.BorderStyle = BorderStyles.Simple;
            this.tbCDKey.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbCDKey.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbCDKey.Size = new Size(0x150, 20);
            this.tbCDKey.TabIndex = 15;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x19c, 0xe2);
            base.Controls.Add(this.skinButtonReset);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.tbCDKey);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x19c, 0xe2);
            this.MinimumSize = new Size(0x19c, 0xe2);
            base.Name = "DlgEmailUsername";
            this.Text = "<LOC>Email Username";
            base.AfterShown += new BaseFormEvent(this.DlgEmailUsername_AfterShown);
            base.Controls.SetChildIndex(this.tbCDKey, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonReset, 0);
            this.skinButtonReset.ResumeLayout(false);
            this.tbCDKey.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
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
            base.ClearErrors();
            if ((this.tbCDKey.Text.Length < 0x10) || (this.tbCDKey.Text.Length > 30))
            {
                base.Error(this.tbCDKey, "<LOC>Invalid CD key.", new object[0]);
            }
            else
            {
                string username = this.tbCDKey.Text + "_t_e_m_p_";
                string pwd = this.tbCDKey.Text + "_p_a_s_s_";
                if (!User.Login(username, pwd, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort))
                {
                    User.CreateLogin(username, pwd, "dummy@gaspowered.com", Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort);
                }
                User.Login(username, pwd, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort);
                if (User.ResetPassword(this.tbCDKey.Text.Replace("-", ""), "dummy@gaspowered.com"))
                {
                    this.skinButtonReset.Hide();
                    this.skinButtonCancel.Hide();
                    this.skinButtonOK.Show();
                    this.skinButtonOK.Visible = true;
                    this.skinButtonOK.BringToFront();
                    this.skinButtonOK.Refresh();
                    DlgMessage.ShowDialog(Loc.Get("<LOC>An email has been sent to the user registered with this CD Key."));
                    base.AcceptButton = this.skinButtonOK;
                    base.CancelButton = this.skinButtonOK;
                    base.DialogResult = DialogResult.OK;
                }
                else
                {
                    DlgMessage.ShowDialog(Loc.Get("<LOC>Failed to email account name."));
                }
            }
        }
    }
}

