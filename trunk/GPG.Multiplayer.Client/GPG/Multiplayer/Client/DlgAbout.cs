namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class DlgAbout : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabelVersion;
        private GPGPanel gpgPanelFeedback;
        private GPGPanel gpgPanelThank;
        private GPGPictureBox gpgPictureBox1;
        private SkinButton skinButtonOK;

        public DlgAbout(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gpgLabelVersion.Text = string.Format(Loc.Get("<LOC>v{0}"), Assembly.GetExecutingAssembly().GetName().Version.ToString());
            this.gpgLabelVersion.Text = this.gpgLabelVersion.Text + " " + Loc.Get("<LOC>RETAIL");
            this.gpgLabel1.Text = Loc.Get("<LOC>(c) 2007, 2008 Gas Powered Games Corp. All rights reserved.\r\nGas Powered Games and GPGnet are the exclusive trademarks of Gas Powered Games Corp.  All rights reserved. All other trademarks, logos and copyrights are the property of their respective owners.");
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgAbout));
            this.gpgPanelFeedback = new GPGPanel();
            this.skinButtonOK = new SkinButton();
            this.gpgLabelVersion = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgPictureBox1 = new GPGPictureBox();
            this.gpgPanelThank = new GPGPanel();
            this.gpgLabel2 = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgPanelFeedback.SuspendLayout();
            ((ISupportInitialize) this.gpgPictureBox1).BeginInit();
            this.gpgPanelThank.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x1f8, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgPanelFeedback.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelFeedback.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelFeedback.BorderThickness = 2;
            this.gpgPanelFeedback.Controls.Add(this.skinButtonOK);
            this.gpgPanelFeedback.Controls.Add(this.gpgLabelVersion);
            this.gpgPanelFeedback.Controls.Add(this.gpgLabel1);
            this.gpgPanelFeedback.Controls.Add(this.gpgPictureBox1);
            this.gpgPanelFeedback.DrawBorder = false;
            this.gpgPanelFeedback.Location = new Point(12, 0x3e);
            this.gpgPanelFeedback.Name = "gpgPanelFeedback";
            this.gpgPanelFeedback.Size = new Size(540, 0xe4);
            base.ttDefault.SetSuperTip(this.gpgPanelFeedback, null);
            this.gpgPanelFeedback.TabIndex = 8;
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
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
            this.skinButtonOK.Location = new Point(0x1b0, 0xc2);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5f, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 0x1c;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.gpgLabelVersion.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelVersion.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelVersion.AutoSize = true;
            this.gpgLabelVersion.AutoStyle = true;
            this.gpgLabelVersion.Font = new Font("Arial", 9.75f);
            this.gpgLabelVersion.ForeColor = Color.White;
            this.gpgLabelVersion.IgnoreMouseWheel = false;
            this.gpgLabelVersion.IsStyled = false;
            this.gpgLabelVersion.Location = new Point(12, 0x4f);
            this.gpgLabelVersion.Name = "gpgLabelVersion";
            this.gpgLabelVersion.Size = new Size(0x39, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelVersion, null);
            this.gpgLabelVersion.TabIndex = 15;
            this.gpgLabelVersion.Text = "v 1.0.0.0";
            this.gpgLabelVersion.TextStyle = TextStyles.Info;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x74);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x203, 0x4b);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 14;
            this.gpgLabel1.Text = manager.GetString("gpgLabel1.Text");
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.gpgPictureBox1.Dock = DockStyle.Top;
            this.gpgPictureBox1.Image = Resources.gpg_banner_white;
            this.gpgPictureBox1.Location = new Point(0, 0);
            this.gpgPictureBox1.Name = "gpgPictureBox1";
            this.gpgPictureBox1.Size = new Size(540, 0x3a);
            this.gpgPictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.gpgPictureBox1, null);
            this.gpgPictureBox1.TabIndex = 13;
            this.gpgPictureBox1.TabStop = false;
            this.gpgPanelThank.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelThank.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelThank.BorderThickness = 2;
            this.gpgPanelThank.Controls.Add(this.gpgLabel2);
            this.gpgPanelThank.DrawBorder = false;
            this.gpgPanelThank.Location = new Point(12, 0x53);
            this.gpgPanelThank.Name = "gpgPanelThank";
            this.gpgPanelThank.Size = new Size(540, 0xcf);
            base.ttDefault.SetSuperTip(this.gpgPanelThank, null);
            this.gpgPanelThank.TabIndex = 8;
            this.gpgPanelThank.Visible = false;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Dock = DockStyle.Fill;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0, 0);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(540, 0xcf);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 0;
            this.gpgLabel2.Text = "<LOC>Thank you for your feedback!";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabel2.TextStyle = TextStyles.Default;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonOK;
            base.ClientSize = new Size(0x233, 0x161);
            base.Controls.Add(this.gpgPanelFeedback);
            base.Controls.Add(this.gpgPanelThank);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x233, 0x161);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x233, 0x161);
            base.Name = "DlgAbout";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>About GPGnet: Supreme Commander";
            base.Controls.SetChildIndex(this.gpgPanelThank, 0);
            base.Controls.SetChildIndex(this.gpgPanelFeedback, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgPanelFeedback.ResumeLayout(false);
            this.gpgPanelFeedback.PerformLayout();
            ((ISupportInitialize) this.gpgPictureBox1).EndInit();
            this.gpgPanelThank.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

