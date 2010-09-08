namespace GPG.Multiplayer.Client.Vaulting.Tools
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class PnlToolDownloadOptions : PnlBase
    {
        private ComboBox comboBoxReleaseQual;
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGTextBox gpgTextBoxDev;
        private GPG.Multiplayer.Client.Vaulting.Tools.Tool mTool;

        public PnlToolDownloadOptions(GPG.Multiplayer.Client.Vaulting.Tools.Tool tool)
        {
            int index;
            this.components = null;
            this.InitializeComponent();
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Any"), "<LOC>Any"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Unspecified"), "<LOC>Unspecified"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Untested"), "<LOC>Untested"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Alpha"), "<LOC>Alpha"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Beta"), "<LOC>Beta"));
            this.comboBoxReleaseQual.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Release"), "<LOC>Release"));
            this.gpgTextBoxDev.Text = tool.DeveloperName;
            if (tool.Quality == null)
            {
                index = 0;
            }
            else
            {
                index = this.comboBoxReleaseQual.Items.IndexOf(tool.Quality);
            }
            if (index < 0)
            {
                index = 0;
            }
            this.comboBoxReleaseQual.SelectedIndex = index;
            this.mTool = tool;
            this.CriteriaChanged(null, null);
        }

        private void CriteriaChanged(object sender, EventArgs e)
        {
            if (this.Tool != null)
            {
                this.Tool.DeveloperName = this.gpgTextBoxDev.Text;
                if (this.comboBoxReleaseQual.SelectedIndex > 0)
                {
                    this.Tool.Quality = (this.comboBoxReleaseQual.SelectedItem as MultiVal<string, string>).Value2;
                }
                else
                {
                    this.Tool.Quality = null;
                }
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
            this.gpgLabel1 = new GPGLabel();
            this.comboBoxReleaseQual = new ComboBox();
            this.gpgTextBoxDev.Properties.BeginInit();
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
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x10c, 0);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x8e, 0x10);
            this.gpgLabel1.TabIndex = 11;
            this.gpgLabel1.Text = "<LOC>Release Quality";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.comboBoxReleaseQual.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxReleaseQual.FormattingEnabled = true;
            this.comboBoxReleaseQual.Location = new Point(0x10f, 0x11);
            this.comboBoxReleaseQual.Name = "comboBoxReleaseQual";
            this.comboBoxReleaseQual.Size = new Size(0xc0, 0x15);
            this.comboBoxReleaseQual.TabIndex = 0x18;
            this.comboBoxReleaseQual.SelectedIndexChanged += new EventHandler(this.CriteriaChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.comboBoxReleaseQual);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxDev);
            base.Controls.Add(this.gpgLabel1);
            base.Name = "PnlToolDownloadOptions";
            base.Size = new Size(0x1d2, 0x2b);
            this.gpgTextBoxDev.Properties.EndInit();
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

        public GPG.Multiplayer.Client.Vaulting.Tools.Tool Tool
        {
            get
            {
                return this.mTool;
            }
        }
    }
}

