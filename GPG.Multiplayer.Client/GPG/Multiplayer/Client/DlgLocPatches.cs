namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;

    public class DlgLocPatches : DlgBase
    {
        private ComboBox comboBoxLocale;
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGTextBox gpgTextBoxLangFile;
        private GPGTextBox gpgTextBoxPatchUrl;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonChooseFile;
        private SkinButton skinButtonOK;

        public DlgLocPatches()
        {
            this.InitializeComponent();
            this.comboBoxLocale.SelectedIndex = 0;
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
            this.gpgTextBoxPatchUrl = new GPGTextBox();
            this.comboBoxLocale = new ComboBox();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxLangFile = new GPGTextBox();
            this.gpgLabel3 = new GPGLabel();
            this.skinButtonChooseFile = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxPatchUrl.Properties.BeginInit();
            this.gpgTextBoxLangFile.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x1a8, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgTextBoxPatchUrl.Location = new Point(12, 0xd5);
            this.gpgTextBoxPatchUrl.Name = "gpgTextBoxPatchUrl";
            this.gpgTextBoxPatchUrl.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPatchUrl.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPatchUrl.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPatchUrl.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPatchUrl.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPatchUrl.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPatchUrl.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPatchUrl.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPatchUrl.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPatchUrl.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPatchUrl.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPatchUrl.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPatchUrl.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPatchUrl.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPatchUrl.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPatchUrl.Size = new Size(0x180, 20);
            this.gpgTextBoxPatchUrl.TabIndex = 7;
            this.comboBoxLocale.DropDownHeight = 120;
            this.comboBoxLocale.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxLocale.FormattingEnabled = true;
            this.comboBoxLocale.IntegralHeight = false;
            this.comboBoxLocale.Items.AddRange(new object[] { "CN", "CZ", "DE", "ES", "FR", "IT", "KR", "PL", "RU" });
            this.comboBoxLocale.Location = new Point(12, 0x63);
            this.comboBoxLocale.Name = "comboBoxLocale";
            this.comboBoxLocale.Size = new Size(100, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxLocale, null);
            this.comboBoxLocale.TabIndex = 8;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 80);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x57, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 9;
            this.gpgLabel1.Text = "Select Locale";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0xc2);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x6a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 10;
            this.gpgLabel2.Text = "Enter Patch URL";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgTextBoxLangFile.Location = new Point(12, 0x9f);
            this.gpgTextBoxLangFile.Name = "gpgTextBoxLangFile";
            this.gpgTextBoxLangFile.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxLangFile.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxLangFile.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxLangFile.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxLangFile.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxLangFile.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxLangFile.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxLangFile.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxLangFile.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxLangFile.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxLangFile.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxLangFile.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxLangFile.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxLangFile.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxLangFile.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxLangFile.Properties.ReadOnly = true;
            this.gpgTextBoxLangFile.Size = new Size(0x180, 20);
            this.gpgTextBoxLangFile.TabIndex = 11;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(12, 140);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0xac, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 12;
            this.gpgLabel3.Text = "Locate Latest Language File";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.skinButtonChooseFile.AutoStyle = true;
            this.skinButtonChooseFile.BackColor = Color.Transparent;
            this.skinButtonChooseFile.ButtonState = 0;
            this.skinButtonChooseFile.DialogResult = DialogResult.OK;
            this.skinButtonChooseFile.DisabledForecolor = Color.Gray;
            this.skinButtonChooseFile.DrawColor = Color.White;
            this.skinButtonChooseFile.DrawEdges = true;
            this.skinButtonChooseFile.FocusColor = Color.Yellow;
            this.skinButtonChooseFile.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonChooseFile.ForeColor = Color.White;
            this.skinButtonChooseFile.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonChooseFile.IsStyled = true;
            this.skinButtonChooseFile.Location = new Point(0x195, 0x9f);
            this.skinButtonChooseFile.Name = "skinButtonChooseFile";
            this.skinButtonChooseFile.Size = new Size(0x15, 20);
            this.skinButtonChooseFile.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonChooseFile, null);
            this.skinButtonChooseFile.TabIndex = 13;
            this.skinButtonChooseFile.TabStop = true;
            this.skinButtonChooseFile.Text = "...";
            this.skinButtonChooseFile.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonChooseFile.TextPadding = new Padding(0);
            this.skinButtonChooseFile.Click += new EventHandler(this.skinButtonChooseFile_Click);
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Transparent;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x178, 0x11f);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5f, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 14;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Transparent;
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
            this.skinButtonOK.Location = new Point(0x113, 0x11f);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5f, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 15;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1e3, 0x178);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonChooseFile);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgTextBoxLangFile);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextBoxPatchUrl);
            base.Controls.Add(this.comboBoxLocale);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgLocPatches";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Update Loc Patches";
            base.Controls.SetChildIndex(this.comboBoxLocale, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPatchUrl, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxLangFile, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.skinButtonChooseFile, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxPatchUrl.Properties.EndInit();
            this.gpgTextBoxLangFile.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;
            if ((GameInformation.SelectedGame.GameLocation != null) && (GameInformation.SelectedGame.GameLocation.Length > 0))
            {
                dialog.InitialDirectory = GameInformation.SelectedGame.GameLocation;
            }
            else
            {
                dialog.InitialDirectory = @"C:\";
            }
            dialog.Filter = string.Format("{0} Loc Files (loc_{0}.scd)|loc_{0}.scd", (string) this.comboBoxLocale.SelectedItem);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.gpgTextBoxLangFile.Text = dialog.FileName;
            }
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxLocale.SelectedIndex < 0)
                {
                    base.Error(this.comboBoxLocale, "You must select a locale", new object[0]);
                }
                else if (!(((this.gpgTextBoxLangFile.Text != null) && (this.gpgTextBoxLangFile.Text.Length >= 1)) && File.Exists(this.gpgTextBoxLangFile.Text)))
                {
                    base.Error(this.gpgTextBoxLangFile, "You must select the latest language file", new object[0]);
                }
                else if ((this.gpgTextBoxPatchUrl.Text == null) || (this.gpgTextBoxPatchUrl.Text.Length < 1))
                {
                    base.Error(this.gpgTextBoxPatchUrl, "You must provide the url to the patch file", new object[0]);
                }
                else
                {
                    string selectedItem = (string) this.comboBoxLocale.SelectedItem;
                    string str2 = Encryption.MD5(this.gpgTextBoxLangFile.Text);
                    string text = this.gpgTextBoxPatchUrl.Text;
                    string str4 = string.Format("%{0}.scd", selectedItem);
                    if (new QuazalQuery("UpdateLanguagePatch", new object[] { text, str2, str4 }).ExecuteNonQuery())
                    {
                        DlgMessage.ShowDialog("Language patch successfully updated");
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                    else
                    {
                        DlgMessage.ShowDialog("Language patch update failed");
                        base.DialogResult = DialogResult.Cancel;
                        base.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                DlgMessage.ShowDialog("Language patch update failed");
            }
        }
    }
}

