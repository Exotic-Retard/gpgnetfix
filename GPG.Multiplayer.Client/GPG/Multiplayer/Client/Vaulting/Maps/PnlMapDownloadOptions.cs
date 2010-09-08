namespace GPG.Multiplayer.Client.Vaulting.Maps
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

    public class PnlMapDownloadOptions : PnlBase
    {
        private ComboBox comboBoxMapSizeType;
        private ComboBox comboBoxMaxPlayerType;
        private IContainer components = null;
        private GPGCheckBox gpgCheckBoxCustomRules;
        private GPGCheckBox gpgCheckBoxMission;
        private GPGCheckBox gpgCheckBoxRush;
        private GPGCheckBox gpgCheckBoxSeparate;
        private GPGCheckBox gpgCheckBoxWater;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGTextBox gpgTextBoxTerrain;
        private CustomMap mMap;
        private NumericUpDown numericUpDownMapSize;
        private NumericUpDown numericUpDownMaxPlayers;

        public PnlMapDownloadOptions(CustomMap map)
        {
            this.InitializeComponent();
            this.comboBoxMapSizeType.SelectedIndex = 1;
            this.comboBoxMaxPlayerType.SelectedIndex = 1;
            this.mMap = map;
            this.CriteriaChanged(null, null);
        }

        private void CriteriaChanged(object sender, EventArgs e)
        {
            if (this.Map != null)
            {
                this.Map.SizeComparisonType = (string) this.comboBoxMapSizeType.SelectedItem;
                this.Map.MaxPlayerComparisonType = (string) this.comboBoxMaxPlayerType.SelectedItem;
                this.Map.Size = (int) this.numericUpDownMapSize.Value;
                this.Map.MaxPlayers = (int) this.numericUpDownMaxPlayers.Value;
                this.Map.TerrainType = this.gpgTextBoxTerrain.Text;
                this.Map.HasWater = this.ParseCheckbox(this.gpgCheckBoxWater);
                this.Map.IsSeparated = this.ParseCheckbox(this.gpgCheckBoxSeparate);
                this.Map.IsRushMap = this.ParseCheckbox(this.gpgCheckBoxRush);
                this.Map.HasCustomRuleset = this.ParseCheckbox(this.gpgCheckBoxCustomRules);
                this.Map.IsMission = this.ParseCheckbox(this.gpgCheckBoxMission);
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
            this.gpgTextBoxTerrain = new GPGTextBox();
            this.gpgCheckBoxMission = new GPGCheckBox();
            this.gpgCheckBoxCustomRules = new GPGCheckBox();
            this.gpgCheckBoxRush = new GPGCheckBox();
            this.gpgCheckBoxSeparate = new GPGCheckBox();
            this.gpgCheckBoxWater = new GPGCheckBox();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.numericUpDownMaxPlayers = new NumericUpDown();
            this.comboBoxMapSizeType = new ComboBox();
            this.comboBoxMaxPlayerType = new ComboBox();
            this.numericUpDownMapSize = new NumericUpDown();
            this.gpgTextBoxTerrain.Properties.BeginInit();
            this.numericUpDownMaxPlayers.BeginInit();
            this.numericUpDownMapSize.BeginInit();
            base.SuspendLayout();
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x12e, 4);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x79, 0x10);
            this.gpgLabel2.TabIndex = 0x15;
            this.gpgLabel2.Text = "<LOC>Terrain Type";
            this.gpgLabel2.TextStyle = TextStyles.Bold;
            this.gpgTextBoxTerrain.Location = new Point(0x131, 0x16);
            this.gpgTextBoxTerrain.Name = "gpgTextBoxTerrain";
            this.gpgTextBoxTerrain.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxTerrain.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxTerrain.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxTerrain.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxTerrain.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxTerrain.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxTerrain.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxTerrain.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxTerrain.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxTerrain.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxTerrain.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxTerrain.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxTerrain.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxTerrain.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxTerrain.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxTerrain.Properties.MaxLength = 0x100;
            this.gpgTextBoxTerrain.Size = new Size(160, 20);
            this.gpgTextBoxTerrain.TabIndex = 20;
            this.gpgTextBoxTerrain.EditValueChanged += new EventHandler(this.CriteriaChanged);
            this.gpgCheckBoxMission.AutoSize = true;
            this.gpgCheckBoxMission.Checked = true;
            this.gpgCheckBoxMission.CheckState = CheckState.Indeterminate;
            this.gpgCheckBoxMission.ForeColor = Color.White;
            this.gpgCheckBoxMission.Location = new Point(0, 0x39);
            this.gpgCheckBoxMission.Name = "gpgCheckBoxMission";
            this.gpgCheckBoxMission.Size = new Size(0x72, 0x11);
            this.gpgCheckBoxMission.TabIndex = 0x13;
            this.gpgCheckBoxMission.Text = "<LOC>Is a Mission";
            this.gpgCheckBoxMission.ThreeState = true;
            this.gpgCheckBoxMission.UsesBG = false;
            this.gpgCheckBoxMission.UseVisualStyleBackColor = true;
            this.gpgCheckBoxMission.CheckStateChanged += new EventHandler(this.CriteriaChanged);
            this.gpgCheckBoxCustomRules.AutoSize = true;
            this.gpgCheckBoxCustomRules.Checked = true;
            this.gpgCheckBoxCustomRules.CheckState = CheckState.Indeterminate;
            this.gpgCheckBoxCustomRules.ForeColor = Color.White;
            this.gpgCheckBoxCustomRules.Location = new Point(0xa7, 80);
            this.gpgCheckBoxCustomRules.Name = "gpgCheckBoxCustomRules";
            this.gpgCheckBoxCustomRules.Size = new Size(160, 0x11);
            this.gpgCheckBoxCustomRules.TabIndex = 0x12;
            this.gpgCheckBoxCustomRules.Text = "<LOC>Uses Custom Ruleset";
            this.gpgCheckBoxCustomRules.ThreeState = true;
            this.gpgCheckBoxCustomRules.UsesBG = false;
            this.gpgCheckBoxCustomRules.UseVisualStyleBackColor = true;
            this.gpgCheckBoxCustomRules.CheckStateChanged += new EventHandler(this.CriteriaChanged);
            this.gpgCheckBoxRush.AutoSize = true;
            this.gpgCheckBoxRush.Checked = true;
            this.gpgCheckBoxRush.CheckState = CheckState.Indeterminate;
            this.gpgCheckBoxRush.ForeColor = Color.White;
            this.gpgCheckBoxRush.Location = new Point(0, 80);
            this.gpgCheckBoxRush.Name = "gpgCheckBoxRush";
            this.gpgCheckBoxRush.Size = new Size(0x85, 0x11);
            this.gpgCheckBoxRush.TabIndex = 0x11;
            this.gpgCheckBoxRush.Text = "<LOC>Is Rushing Map";
            this.gpgCheckBoxRush.ThreeState = true;
            this.gpgCheckBoxRush.UsesBG = false;
            this.gpgCheckBoxRush.UseVisualStyleBackColor = true;
            this.gpgCheckBoxRush.CheckStateChanged += new EventHandler(this.CriteriaChanged);
            this.gpgCheckBoxSeparate.AutoSize = true;
            this.gpgCheckBoxSeparate.Checked = true;
            this.gpgCheckBoxSeparate.CheckState = CheckState.Indeterminate;
            this.gpgCheckBoxSeparate.ForeColor = Color.White;
            this.gpgCheckBoxSeparate.Location = new Point(0x131, 0x39);
            this.gpgCheckBoxSeparate.Name = "gpgCheckBoxSeparate";
            this.gpgCheckBoxSeparate.Size = new Size(0xa4, 0x11);
            this.gpgCheckBoxSeparate.TabIndex = 0x10;
            this.gpgCheckBoxSeparate.Text = "<LOC>Has Player Separation";
            this.gpgCheckBoxSeparate.ThreeState = true;
            this.gpgCheckBoxSeparate.UsesBG = false;
            this.gpgCheckBoxSeparate.UseVisualStyleBackColor = true;
            this.gpgCheckBoxSeparate.CheckStateChanged += new EventHandler(this.CriteriaChanged);
            this.gpgCheckBoxWater.AutoSize = true;
            this.gpgCheckBoxWater.Checked = true;
            this.gpgCheckBoxWater.CheckState = CheckState.Indeterminate;
            this.gpgCheckBoxWater.ForeColor = Color.White;
            this.gpgCheckBoxWater.Location = new Point(0xa7, 0x39);
            this.gpgCheckBoxWater.Name = "gpgCheckBoxWater";
            this.gpgCheckBoxWater.Size = new Size(110, 0x11);
            this.gpgCheckBoxWater.TabIndex = 15;
            this.gpgCheckBoxWater.Text = "<LOC>Has Water";
            this.gpgCheckBoxWater.ThreeState = true;
            this.gpgCheckBoxWater.UsesBG = false;
            this.gpgCheckBoxWater.UseVisualStyleBackColor = true;
            this.gpgCheckBoxWater.CheckStateChanged += new EventHandler(this.CriteriaChanged);
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0xa4, 4);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x7b, 0x10);
            this.gpgLabel3.TabIndex = 13;
            this.gpgLabel3.Text = "<LOC>Max Players";
            this.gpgLabel3.TextStyle = TextStyles.Bold;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(-3, 4);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x69, 0x10);
            this.gpgLabel1.TabIndex = 11;
            this.gpgLabel1.Text = "<LOC>Map Size";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.numericUpDownMaxPlayers.Location = new Point(0xde, 0x16);
            int[] bits = new int[4];
            bits[0] = 8;
            this.numericUpDownMaxPlayers.Maximum = new decimal(bits);
            this.numericUpDownMaxPlayers.Name = "numericUpDownMaxPlayers";
            this.numericUpDownMaxPlayers.Size = new Size(0x2f, 20);
            this.numericUpDownMaxPlayers.TabIndex = 0x17;
            this.numericUpDownMaxPlayers.ValueChanged += new EventHandler(this.CriteriaChanged);
            this.comboBoxMapSizeType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMapSizeType.FormattingEnabled = true;
            this.comboBoxMapSizeType.Items.AddRange(new object[] { "=", ">", "<" });
            this.comboBoxMapSizeType.Location = new Point(0, 0x15);
            this.comboBoxMapSizeType.Name = "comboBoxMapSizeType";
            this.comboBoxMapSizeType.Size = new Size(0x31, 0x15);
            this.comboBoxMapSizeType.TabIndex = 0x18;
            this.comboBoxMapSizeType.SelectedIndexChanged += new EventHandler(this.CriteriaChanged);
            this.comboBoxMaxPlayerType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMaxPlayerType.FormattingEnabled = true;
            this.comboBoxMaxPlayerType.Items.AddRange(new object[] { "=", ">", "<" });
            this.comboBoxMaxPlayerType.Location = new Point(0xa7, 0x15);
            this.comboBoxMaxPlayerType.Name = "comboBoxMaxPlayerType";
            this.comboBoxMaxPlayerType.Size = new Size(0x31, 0x15);
            this.comboBoxMaxPlayerType.TabIndex = 0x19;
            this.comboBoxMaxPlayerType.SelectedIndexChanged += new EventHandler(this.CriteriaChanged);
            bits = new int[4];
            bits[0] = 0x100;
            this.numericUpDownMapSize.Increment = new decimal(bits);
            this.numericUpDownMapSize.Location = new Point(0x37, 0x16);
            bits = new int[4];
            bits[0] = 0x2000;
            this.numericUpDownMapSize.Maximum = new decimal(bits);
            this.numericUpDownMapSize.Name = "numericUpDownMapSize";
            this.numericUpDownMapSize.Size = new Size(0x4e, 20);
            this.numericUpDownMapSize.TabIndex = 0x1a;
            this.numericUpDownMapSize.ValueChanged += new EventHandler(this.CriteriaChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.numericUpDownMapSize);
            base.Controls.Add(this.comboBoxMaxPlayerType);
            base.Controls.Add(this.comboBoxMapSizeType);
            base.Controls.Add(this.numericUpDownMaxPlayers);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxTerrain);
            base.Controls.Add(this.gpgCheckBoxMission);
            base.Controls.Add(this.gpgCheckBoxCustomRules);
            base.Controls.Add(this.gpgCheckBoxRush);
            base.Controls.Add(this.gpgCheckBoxSeparate);
            base.Controls.Add(this.gpgCheckBoxWater);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel1);
            base.Name = "PnlMapDownloadOptions";
            base.Size = new Size(0x1d2, 0x67);
            this.gpgTextBoxTerrain.Properties.EndInit();
            this.numericUpDownMaxPlayers.EndInit();
            this.numericUpDownMapSize.EndInit();
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

        public CustomMap Map
        {
            get
            {
                return this.mMap;
            }
        }
    }
}

