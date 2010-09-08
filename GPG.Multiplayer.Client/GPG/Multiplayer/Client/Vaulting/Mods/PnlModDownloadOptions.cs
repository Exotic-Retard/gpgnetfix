namespace GPG.Multiplayer.Client.Vaulting.Mods
{
    using DevExpress.XtraEditors.Controls;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class PnlModDownloadOptions : PnlBase
    {
        private IContainer components = null;
        private GPGCheckBox gpgCheckBoxUIOnly;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGTextBox gpgTextBoxDev;
        private GPGTextBox gpgTextBoxGuid;
        private GPG.Multiplayer.Client.Vaulting.Mods.Mod mMod;

        public PnlModDownloadOptions(GPG.Multiplayer.Client.Vaulting.Mods.Mod mod)
        {
            this.InitializeComponent();
            this.mMod = mod;
            this.CriteriaChanged(null, null);
        }

        private void CriteriaChanged(object sender, EventArgs e)
        {
            if (this.Mod != null)
            {
                this.Mod.DeveloperName = this.gpgTextBoxDev.Text;
                this.Mod.UIOnly = this.ParseCheckbox(this.gpgCheckBoxUIOnly);
                this.Mod.Guid = this.gpgTextBoxGuid.Text;
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

        private void InitializeComponent()
        {
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxDev = new GPGTextBox();
            this.gpgCheckBoxUIOnly = new GPGCheckBox();
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxGuid = new GPGTextBox();
            this.gpgTextBoxDev.Properties.BeginInit();
            this.gpgTextBoxGuid.Properties.BeginInit();
            base.SuspendLayout();
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0, 0);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x90, 0x10);
            this.gpgLabel2.TabIndex = 0x15;
            this.gpgLabel2.Text = "<LOC>Developer Name";
            this.gpgLabel2.TextStyle = TextStyles.Bold;
            this.gpgTextBoxDev.Location = new Point(0, 0x12);
            this.gpgTextBoxDev.Name = "gpgTextBoxDev";
            this.gpgTextBoxDev.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxDev.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxDev.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxDev.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxDev.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxDev.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxDev.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxDev.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxDev.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxDev.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxDev.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxDev.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxDev.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxDev.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxDev.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxDev.Properties.MaxLength = 0x100;
            this.gpgTextBoxDev.Size = new Size(0xf6, 20);
            this.gpgTextBoxDev.TabIndex = 20;
            this.gpgTextBoxDev.EditValueChanged += new EventHandler(this.CriteriaChanged);
            this.gpgCheckBoxUIOnly.AutoSize = true;
            this.gpgCheckBoxUIOnly.Checked = true;
            this.gpgCheckBoxUIOnly.CheckState = CheckState.Indeterminate;
            this.gpgCheckBoxUIOnly.Location = new Point(0x110, 0x13);
            this.gpgCheckBoxUIOnly.Name = "gpgCheckBoxUIOnly";
            this.gpgCheckBoxUIOnly.Size = new Size(0x5e, 0x11);
            this.gpgCheckBoxUIOnly.TabIndex = 0x16;
            this.gpgCheckBoxUIOnly.Text = "<LOC>UI Only";
            this.gpgCheckBoxUIOnly.ThreeState = true;
            this.gpgCheckBoxUIOnly.UsesBG = false;
            this.gpgCheckBoxUIOnly.UseVisualStyleBackColor = true;
            this.gpgCheckBoxUIOnly.CheckStateChanged += new EventHandler(this.CriteriaChanged);
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0, 0x2c);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x8b, 0x10);
            this.gpgLabel1.TabIndex = 0x18;
            this.gpgLabel1.Text = "<LOC>Unique ID (UID)";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.gpgTextBoxGuid.Location = new Point(0, 0x3e);
            this.gpgTextBoxGuid.Name = "gpgTextBoxGuid";
            this.gpgTextBoxGuid.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxGuid.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxGuid.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxGuid.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxGuid.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxGuid.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxGuid.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxGuid.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxGuid.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxGuid.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxGuid.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxGuid.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxGuid.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxGuid.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxGuid.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxGuid.Properties.MaxLength = 0x100;
            this.gpgTextBoxGuid.Size = new Size(0x1cf, 20);
            this.gpgTextBoxGuid.TabIndex = 0x17;
            this.gpgTextBoxGuid.EditValueChanged += new EventHandler(this.CriteriaChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextBoxGuid);
            base.Controls.Add(this.gpgCheckBoxUIOnly);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxDev);
            base.Name = "PnlModDownloadOptions";
            base.Size = new Size(0x1d2, 0x55);
            this.gpgTextBoxDev.Properties.EndInit();
            this.gpgTextBoxGuid.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool? ParseCheckbox(CheckBox check)
        {
            switch (check.CheckState)
            {
                case CheckState.Unchecked:
                    return false;

                case CheckState.Checked:
                    return true;

                case CheckState.Indeterminate:
                    return null;
            }
            return null;
        }

        public GPG.Multiplayer.Client.Vaulting.Mods.Mod Mod
        {
            get
            {
                return this.mMod;
            }
        }
    }
}

