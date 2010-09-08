namespace GPG.Multiplayer.Client.Games
{
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgGameUpdate : DlgBase
    {
        private IContainer components;
        private GPGCheckBox gpgCheckBoxNoShow;
        private GPGLabel gpgLabelMessage;
        private SkinButton skinButtonClose;

        public DlgGameUpdate(FrmMain mainForm, string gameName) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gpgLabelMessage.Text = string.Format(Loc.Get("<LOC>An updated version of {0} is available and is being automatically downloaded. {0} will be disabled until the download is complete."), gameName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgCheckBoxNoShow_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.SupcomPrefs.ShowPatchMsg = !this.gpgCheckBoxNoShow.Checked;
        }

        private void InitializeComponent()
        {
            this.gpgLabelMessage = new GPGLabel();
            this.skinButtonClose = new SkinButton();
            this.gpgCheckBoxNoShow = new GPGCheckBox();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgLabelMessage.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelMessage.AutoStyle = true;
            this.gpgLabelMessage.Font = new Font("Arial", 9.75f);
            this.gpgLabelMessage.ForeColor = Color.White;
            this.gpgLabelMessage.IgnoreMouseWheel = false;
            this.gpgLabelMessage.Location = new Point(12, 0x4c);
            this.gpgLabelMessage.MaximumSize = new Size(0x213, 0xfb);
            this.gpgLabelMessage.Name = "gpgLabelMessage";
            this.gpgLabelMessage.Size = new Size(0x14e, 0x6c);
            this.gpgLabelMessage.TabIndex = 7;
            this.gpgLabelMessage.Text = "gpgLabel1";
            this.gpgLabelMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelMessage.TextStyle = TextStyles.Default;
            this.skinButtonClose.Anchor = AnchorStyles.Bottom;
            this.skinButtonClose.DialogResult = DialogResult.OK;
            this.skinButtonClose.DrawEdges = true;
            this.skinButtonClose.FocusColor = Color.Yellow;
            this.skinButtonClose.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClose.ForeColor = Color.White;
            this.skinButtonClose.Location = new Point(0x8a, 220);
            this.skinButtonClose.Name = "skinButtonClose";
            this.skinButtonClose.Size = new Size(0x55, 0x1a);
            this.skinButtonClose.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonClose.TabIndex = 8;
            this.skinButtonClose.Text = "<LOC>OK";
            this.skinButtonClose.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClose.TextPadding = new Padding(0);
            this.skinButtonClose.Click += new EventHandler(this.skinButtonClose_Click);
            this.gpgCheckBoxNoShow.AutoSize = true;
            this.gpgCheckBoxNoShow.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgCheckBoxNoShow.Location = new Point(15, 0xbb);
            this.gpgCheckBoxNoShow.Name = "gpgCheckBoxNoShow";
            this.gpgCheckBoxNoShow.Size = new Size(150, 0x12);
            this.gpgCheckBoxNoShow.TabIndex = 9;
            this.gpgCheckBoxNoShow.Text = "<LOC>Do not show again";
            this.gpgCheckBoxNoShow.UseVisualStyleBackColor = true;
            this.gpgCheckBoxNoShow.CheckedChanged += new EventHandler(this.gpgCheckBoxNoShow_CheckedChanged);
            base.AcceptButton = this.skinButtonClose;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonClose;
            base.ClientSize = new Size(0x165, 300);
            base.Controls.Add(this.gpgCheckBoxNoShow);
            base.Controls.Add(this.skinButtonClose);
            base.Controls.Add(this.gpgLabelMessage);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0x165, 300);
            this.MinimumSize = new Size(0x165, 300);
            base.Name = "DlgGameUpdate";
            this.Text = "<LOC>Game Update";
            base.Controls.SetChildIndex(this.gpgLabelMessage, 0);
            base.Controls.SetChildIndex(this.skinButtonClose, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxNoShow, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

