namespace GPG.Multiplayer.Client
{
    using GPG.Multiplayer.Client.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgTermsOfService : DlgBase
    {
        private IContainer components;
        private GPGWebBrowser gpgWebBrowserAgreement;
        public SkinButton skinButtonAccept;
        public SkinButton skinButtonDecline;
        private SkinLabel skinLabel1;

        public DlgTermsOfService(FrmMain mainForm, string url) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gpgWebBrowserAgreement.Navigate(url);
            this.skinButtonAccept.Enabled = true;
            base.BringToFront();
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
            this.skinButtonAccept = new SkinButton();
            this.skinButtonDecline = new SkinButton();
            this.skinLabel1 = new SkinLabel();
            this.gpgWebBrowserAgreement = new GPGWebBrowser();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.skinButtonAccept.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonAccept.AutoStyle = true;
            this.skinButtonAccept.BackColor = Color.Black;
            this.skinButtonAccept.DialogResult = DialogResult.OK;
            this.skinButtonAccept.DisabledForecolor = Color.Gray;
            this.skinButtonAccept.DrawEdges = true;
            this.skinButtonAccept.Enabled = false;
            this.skinButtonAccept.FocusColor = Color.Yellow;
            this.skinButtonAccept.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAccept.ForeColor = Color.White;
            this.skinButtonAccept.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAccept.IsStyled = true;
            this.skinButtonAccept.Location = new Point(0xd5, 0x1b4);
            this.skinButtonAccept.Name = "skinButtonAccept";
            this.skinButtonAccept.Size = new Size(0x6d, 0x1a);
            this.skinButtonAccept.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonAccept.TabIndex = 8;
            this.skinButtonAccept.Text = "<LOC>Accept";
            this.skinButtonAccept.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAccept.TextPadding = new Padding(0);
            this.skinButtonAccept.Click += new EventHandler(this.skinButtonAccept_Click);
            this.skinButtonDecline.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonDecline.AutoStyle = true;
            this.skinButtonDecline.BackColor = Color.Black;
            this.skinButtonDecline.DialogResult = DialogResult.OK;
            this.skinButtonDecline.DisabledForecolor = Color.Gray;
            this.skinButtonDecline.DrawEdges = true;
            this.skinButtonDecline.FocusColor = Color.Yellow;
            this.skinButtonDecline.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDecline.ForeColor = Color.White;
            this.skinButtonDecline.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDecline.IsStyled = true;
            this.skinButtonDecline.Location = new Point(0x148, 0x1b4);
            this.skinButtonDecline.Name = "skinButtonDecline";
            this.skinButtonDecline.Size = new Size(0x6d, 0x1a);
            this.skinButtonDecline.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonDecline.TabIndex = 9;
            this.skinButtonDecline.Text = "<LOC>Decline";
            this.skinButtonDecline.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDecline.TextPadding = new Padding(0);
            this.skinButtonDecline.Click += new EventHandler(this.skinButtonDecline_Click);
            this.skinLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(13, 0x54);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0x1a8, 0x1c);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\Rectangle";
            this.skinLabel1.TabIndex = 10;
            this.skinLabel1.Text = "<LOC>Supreme Commander";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleCenter;
            this.skinLabel1.TextPadding = new Padding(0);
            this.gpgWebBrowserAgreement.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgWebBrowserAgreement.Location = new Point(13, 0x76);
            this.gpgWebBrowserAgreement.MinimumSize = new Size(20, 20);
            this.gpgWebBrowserAgreement.Name = "gpgWebBrowserAgreement";
            this.gpgWebBrowserAgreement.Size = new Size(0x1a8, 0x12f);
            this.gpgWebBrowserAgreement.TabIndex = 11;
            base.AcceptButton = this.skinButtonAccept;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonDecline;
            base.ClientSize = new Size(0x1c1, 0x20e);
            base.Controls.Add(this.gpgWebBrowserAgreement);
            base.Controls.Add(this.skinLabel1);
            base.Controls.Add(this.skinButtonDecline);
            base.Controls.Add(this.skinButtonAccept);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x1c1, 0x20e);
            base.Name = "DlgTermsOfService";
            this.Text = "";
            base.Controls.SetChildIndex(this.skinButtonAccept, 0);
            base.Controls.SetChildIndex(this.skinButtonDecline, 0);
            base.Controls.SetChildIndex(this.skinLabel1, 0);
            base.Controls.SetChildIndex(this.gpgWebBrowserAgreement, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonAccept_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void skinButtonDecline_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }
    }
}

