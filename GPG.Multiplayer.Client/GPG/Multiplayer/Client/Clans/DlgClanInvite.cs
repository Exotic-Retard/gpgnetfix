namespace GPG.Multiplayer.Client.Clans
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgClanInvite : DlgBase
    {
        private IContainer components = null;
        private GPGButton gpgButtonAccept;
        private GPGButton gpgButtonCancel;
        private GPGButton gpgButtonLater;
        private GPGLabel gpgLabelQuestion;

        public DlgClanInvite(string caption, string msg)
        {
            this.InitializeComponent();
            this.Text = msg;
            this.gpgLabelQuestion.Text = msg;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgButtonAccept_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Yes;
            base.Close();
        }

        private void gpgButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
            base.Close();
        }

        private void gpgButtonLater_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.None;
            base.Close();
        }

        private void InitializeComponent()
        {
            this.gpgButtonCancel = new GPGButton();
            this.gpgButtonAccept = new GPGButton();
            this.gpgLabelQuestion = new GPGLabel();
            this.gpgButtonLater = new GPGButton();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgButtonCancel.Appearance.ForeColor = Color.Black;
            this.gpgButtonCancel.Appearance.Options.UseForeColor = true;
            this.gpgButtonCancel.DialogResult = DialogResult.Cancel;
            this.gpgButtonCancel.Location = new Point(0x9f, 0xa2);
            this.gpgButtonCancel.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgButtonCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgButtonCancel.Name = "gpgButtonCancel";
            this.gpgButtonCancel.Size = new Size(0x4b, 0x17);
            this.gpgButtonCancel.TabIndex = 9;
            this.gpgButtonCancel.Text = "<LOC>No";
            this.gpgButtonCancel.UseVisualStyleBackColor = true;
            this.gpgButtonCancel.Click += new EventHandler(this.gpgButtonCancel_Click);
            this.gpgButtonAccept.Appearance.ForeColor = Color.Black;
            this.gpgButtonAccept.Appearance.Options.UseForeColor = true;
            this.gpgButtonAccept.Location = new Point(240, 0xa2);
            this.gpgButtonAccept.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgButtonAccept.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgButtonAccept.Name = "gpgButtonAccept";
            this.gpgButtonAccept.Size = new Size(0x4b, 0x17);
            this.gpgButtonAccept.TabIndex = 8;
            this.gpgButtonAccept.Text = "<LOC>Yes";
            this.gpgButtonAccept.UseVisualStyleBackColor = true;
            this.gpgButtonAccept.Click += new EventHandler(this.gpgButtonAccept_Click);
            this.gpgLabelQuestion.AutoStyle = true;
            this.gpgLabelQuestion.Font = new Font("Arial", 9.75f);
            this.gpgLabelQuestion.ForeColor = Color.White;
            this.gpgLabelQuestion.IgnoreMouseWheel = false;
            this.gpgLabelQuestion.Location = new Point(12, 0x4e);
            this.gpgLabelQuestion.Name = "gpgLabelQuestion";
            this.gpgLabelQuestion.Size = new Size(370, 0x3e);
            this.gpgLabelQuestion.TabIndex = 7;
            this.gpgLabelQuestion.Text = "gpgLabel1";
            this.gpgLabelQuestion.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelQuestion.TextStyle = TextStyles.Default;
            this.gpgButtonLater.Appearance.ForeColor = Color.Black;
            this.gpgButtonLater.Appearance.Options.UseForeColor = true;
            this.gpgButtonLater.Location = new Point(0x4e, 0xa2);
            this.gpgButtonLater.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgButtonLater.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgButtonLater.Name = "gpgButtonLater";
            this.gpgButtonLater.Size = new Size(0x4b, 0x17);
            this.gpgButtonLater.TabIndex = 10;
            this.gpgButtonLater.Text = "<LOC>Later";
            this.gpgButtonLater.UseVisualStyleBackColor = true;
            this.gpgButtonLater.Click += new EventHandler(this.gpgButtonLater_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x18a, 0x107);
            base.Controls.Add(this.gpgButtonLater);
            base.Controls.Add(this.gpgButtonCancel);
            base.Controls.Add(this.gpgButtonAccept);
            base.Controls.Add(this.gpgLabelQuestion);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0x18a, 0x107);
            this.MinimumSize = new Size(0x18a, 0x107);
            base.Name = "DlgClanInvite";
            this.Text = "DlgClanInvite";
            base.Controls.SetChildIndex(this.gpgLabelQuestion, 0);
            base.Controls.SetChildIndex(this.gpgButtonAccept, 0);
            base.Controls.SetChildIndex(this.gpgButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgButtonLater, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

