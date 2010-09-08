namespace GPG.Multiplayer.Client.Vaulting.Maps
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class PnlMapUploadOptions : PnlBase
    {
        private IContainer components = null;
        private GPGCheckBox gpgCheckBoxCustomRules;
        private GPGCheckBox gpgCheckBoxMission;
        private GPGCheckBox gpgCheckBoxRush;
        private GPGCheckBox gpgCheckBoxSeparate;
        private GPGCheckBox gpgCheckBoxWater;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabelMapDesc;
        private GPGLabel gpgLabelMapName;
        private GPGLabel gpgLabelMapSize;
        private GPGLabel gpgLabelMaxPlayers;
        private GPGPanel gpgPanel1;
        private GPGTextBox gpgTextBoxTerrain;
        private CustomMap mMap;
        private PictureBox pictureBoxPreview;
        private SkinButton skinButtonTestMap;

        public PnlMapUploadOptions(CustomMap map)
        {
            this.InitializeComponent();
            this.gpgLabel7.Text = Loc.Get("<LOC id=_8124c3c89bfc4f3bca49659c2cb4ef3b>It is recommended you test your map as it will be after being downloaded through the vault to ensure it works. Click the button below to access the map author testing and diagnostics tool.");
            this.gpgLabelMapName.Text = map.MapName;
            this.gpgLabelMapSize.Text = string.Format(Loc.Get("<LOC>{0}x{1}  ({2})"), map.Width, map.Height, map.SizeDisplay);
            this.gpgLabelMaxPlayers.Text = map.MaxPlayers.ToString();
            this.pictureBoxPreview.Image = map.PreviewImage128;
            this.gpgTextBoxTerrain.Text = map.TerrainType;
            this.gpgCheckBoxWater.Checked = map.HasWater.GetValueOrDefault();
            this.gpgCheckBoxSeparate.Checked = map.IsSeparated.GetValueOrDefault();
            this.gpgCheckBoxRush.Checked = map.IsRushMap.GetValueOrDefault();
            this.gpgCheckBoxCustomRules.Checked = map.HasCustomRuleset.GetValueOrDefault();
            this.gpgCheckBoxMission.Checked = map.IsMission.GetValueOrDefault();
            this.gpgLabelMapDesc.Text = map.MapDescription;
            this.mMap = map;
            CustomMap.PreviewImageLoaded += new EventHandler(this.CustomMap_PreviewImageLoaded);
            base.Disposed += new EventHandler(this.PnlMapDetailsView_Disposed);
            this.MapAttributeChanged(null, null);
        }

        private void CustomMap_PreviewImageLoaded(object sender, EventArgs e)
        {
            VGen0 method = null;
            CustomMap map;
            if ((!base.Disposing && !base.IsDisposed) && (this.Map != null))
            {
                map = sender as CustomMap;
                if (this.Map.ID == map.ID)
                {
                    if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.pictureBoxPreview.Image = map.PreviewImage128;
                            };
                        }
                        base.BeginInvoke(method);
                    }
                    else if (!(base.Disposing || base.IsDisposed))
                    {
                        this.pictureBoxPreview.Image = map.PreviewImage128;
                    }
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
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelMapSize = new GPGLabel();
            this.gpgLabelMaxPlayers = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.pictureBoxPreview = new PictureBox();
            this.gpgLabel4 = new GPGLabel();
            this.gpgCheckBoxWater = new GPGCheckBox();
            this.gpgCheckBoxSeparate = new GPGCheckBox();
            this.gpgCheckBoxRush = new GPGCheckBox();
            this.gpgCheckBoxCustomRules = new GPGCheckBox();
            this.gpgCheckBoxMission = new GPGCheckBox();
            this.gpgTextBoxTerrain = new GPGTextBox();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabelMapDesc = new GPGLabel();
            this.gpgPanel1 = new GPGPanel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabelMapName = new GPGLabel();
            this.skinButtonTestMap = new SkinButton();
            this.gpgLabel7 = new GPGLabel();
            ((ISupportInitialize) this.pictureBoxPreview).BeginInit();
            this.gpgTextBoxTerrain.Properties.BeginInit();
            this.gpgPanel1.SuspendLayout();
            base.SuspendLayout();
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x85, 0x2f);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x1c9, 0x11);
            this.gpgLabel1.TabIndex = 0;
            this.gpgLabel1.Text = "<LOC>Map Size";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgLabelMapSize.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMapSize.AutoSize = true;
            this.gpgLabelMapSize.AutoStyle = true;
            this.gpgLabelMapSize.Font = new Font("Arial", 9.75f);
            this.gpgLabelMapSize.ForeColor = Color.White;
            this.gpgLabelMapSize.IgnoreMouseWheel = false;
            this.gpgLabelMapSize.IsStyled = false;
            this.gpgLabelMapSize.Location = new Point(0x85, 0x45);
            this.gpgLabelMapSize.Name = "gpgLabelMapSize";
            this.gpgLabelMapSize.Size = new Size(0x43, 0x10);
            this.gpgLabelMapSize.TabIndex = 1;
            this.gpgLabelMapSize.Text = "gpgLabel2";
            this.gpgLabelMapSize.TextStyle = TextStyles.Default;
            this.gpgLabelMaxPlayers.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMaxPlayers.AutoSize = true;
            this.gpgLabelMaxPlayers.AutoStyle = true;
            this.gpgLabelMaxPlayers.Font = new Font("Arial", 9.75f);
            this.gpgLabelMaxPlayers.ForeColor = Color.White;
            this.gpgLabelMaxPlayers.IgnoreMouseWheel = false;
            this.gpgLabelMaxPlayers.IsStyled = false;
            this.gpgLabelMaxPlayers.Location = new Point(0x110, 0x45);
            this.gpgLabelMaxPlayers.Name = "gpgLabelMaxPlayers";
            this.gpgLabelMaxPlayers.Size = new Size(0x43, 0x10);
            this.gpgLabelMaxPlayers.TabIndex = 3;
            this.gpgLabelMaxPlayers.Text = "gpgLabel2";
            this.gpgLabelMaxPlayers.TextStyle = TextStyles.Default;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x110, 0x2f);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x7b, 0x11);
            this.gpgLabel3.TabIndex = 2;
            this.gpgLabel3.Text = "<LOC>Max Players";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x1aa, 0x2f);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x79, 0x11);
            this.gpgLabel2.TabIndex = 10;
            this.gpgLabel2.Text = "<LOC>Terrain Type";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.pictureBoxPreview.Location = new Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new Size(0x80, 0x80);
            this.pictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreview.TabIndex = 11;
            this.pictureBoxPreview.TabStop = false;
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel4.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x85, 0x67);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x1c9, 0x11);
            this.gpgLabel4.TabIndex = 12;
            this.gpgLabel4.Text = "<LOC>Parameters";
            this.gpgLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel4.TextStyle = TextStyles.Custom;
            this.gpgCheckBoxWater.AutoSize = true;
            this.gpgCheckBoxWater.ForeColor = Color.White;
            this.gpgCheckBoxWater.Location = new Point(0x87, 0x7d);
            this.gpgCheckBoxWater.Name = "gpgCheckBoxWater";
            this.gpgCheckBoxWater.Size = new Size(110, 0x11);
            this.gpgCheckBoxWater.TabIndex = 4;
            this.gpgCheckBoxWater.Text = "<LOC>Has Water";
            this.gpgCheckBoxWater.UsesBG = false;
            this.gpgCheckBoxWater.UseVisualStyleBackColor = true;
            this.gpgCheckBoxWater.CheckedChanged += new EventHandler(this.MapAttributeChanged);
            this.gpgCheckBoxSeparate.AutoSize = true;
            this.gpgCheckBoxSeparate.ForeColor = Color.White;
            this.gpgCheckBoxSeparate.Location = new Point(0x126, 0x7d);
            this.gpgCheckBoxSeparate.Name = "gpgCheckBoxSeparate";
            this.gpgCheckBoxSeparate.Size = new Size(0xa4, 0x11);
            this.gpgCheckBoxSeparate.TabIndex = 5;
            this.gpgCheckBoxSeparate.Text = "<LOC>Has Player Separation";
            this.gpgCheckBoxSeparate.UsesBG = false;
            this.gpgCheckBoxSeparate.UseVisualStyleBackColor = true;
            this.gpgCheckBoxSeparate.CheckedChanged += new EventHandler(this.MapAttributeChanged);
            this.gpgCheckBoxRush.AutoSize = true;
            this.gpgCheckBoxRush.ForeColor = Color.White;
            this.gpgCheckBoxRush.Location = new Point(0x87, 0x92);
            this.gpgCheckBoxRush.Name = "gpgCheckBoxRush";
            this.gpgCheckBoxRush.Size = new Size(0x85, 0x11);
            this.gpgCheckBoxRush.TabIndex = 6;
            this.gpgCheckBoxRush.Text = "<LOC>Is Rushing Map";
            this.gpgCheckBoxRush.UsesBG = false;
            this.gpgCheckBoxRush.UseVisualStyleBackColor = true;
            this.gpgCheckBoxRush.CheckedChanged += new EventHandler(this.MapAttributeChanged);
            this.gpgCheckBoxCustomRules.AutoSize = true;
            this.gpgCheckBoxCustomRules.ForeColor = Color.White;
            this.gpgCheckBoxCustomRules.Location = new Point(0x126, 0x92);
            this.gpgCheckBoxCustomRules.Name = "gpgCheckBoxCustomRules";
            this.gpgCheckBoxCustomRules.Size = new Size(160, 0x11);
            this.gpgCheckBoxCustomRules.TabIndex = 7;
            this.gpgCheckBoxCustomRules.Text = "<LOC>Uses Custom Ruleset";
            this.gpgCheckBoxCustomRules.UsesBG = false;
            this.gpgCheckBoxCustomRules.UseVisualStyleBackColor = true;
            this.gpgCheckBoxCustomRules.CheckedChanged += new EventHandler(this.MapAttributeChanged);
            this.gpgCheckBoxMission.AutoSize = true;
            this.gpgCheckBoxMission.ForeColor = Color.White;
            this.gpgCheckBoxMission.Location = new Point(0x87, 0xa7);
            this.gpgCheckBoxMission.Name = "gpgCheckBoxMission";
            this.gpgCheckBoxMission.Size = new Size(0x72, 0x11);
            this.gpgCheckBoxMission.TabIndex = 8;
            this.gpgCheckBoxMission.Text = "<LOC>Is a Mission";
            this.gpgCheckBoxMission.UsesBG = false;
            this.gpgCheckBoxMission.UseVisualStyleBackColor = true;
            this.gpgCheckBoxMission.CheckedChanged += new EventHandler(this.MapAttributeChanged);
            this.gpgTextBoxTerrain.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxTerrain.Location = new Point(0x1ad, 0x45);
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
            this.gpgTextBoxTerrain.Size = new Size(0xa1, 20);
            this.gpgTextBoxTerrain.TabIndex = 9;
            this.gpgTextBoxTerrain.EditValueChanged += new EventHandler(this.MapAttributeChanged);
            this.gpgLabel5.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel5.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0, 0xbb);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(590, 0x11);
            this.gpgLabel5.TabIndex = 13;
            this.gpgLabel5.Text = "<LOC>Map Description";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.gpgLabelMapDesc.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelMapDesc.AutoStyle = true;
            this.gpgLabelMapDesc.Font = new Font("Arial", 9.75f);
            this.gpgLabelMapDesc.ForeColor = Color.White;
            this.gpgLabelMapDesc.IgnoreMouseWheel = false;
            this.gpgLabelMapDesc.IsStyled = false;
            this.gpgLabelMapDesc.Location = new Point(0, 0);
            this.gpgLabelMapDesc.Name = "gpgLabelMapDesc";
            this.gpgLabelMapDesc.Size = new Size(0x231, 15);
            this.gpgLabelMapDesc.TabIndex = 14;
            this.gpgLabelMapDesc.Text = "gpgLabel6";
            this.gpgLabelMapDesc.TextStyle = TextStyles.Default;
            this.gpgPanel1.AutoScroll = true;
            this.gpgPanel1.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel1.BorderThickness = 2;
            this.gpgPanel1.Controls.Add(this.gpgLabelMapDesc);
            this.gpgPanel1.DrawBorder = false;
            this.gpgPanel1.Location = new Point(0, 0xcf);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(0x243, 0x31);
            this.gpgPanel1.TabIndex = 15;
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x85, 0);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x1c9, 0x11);
            this.gpgLabel6.TabIndex = 0x10;
            this.gpgLabel6.Text = "<LOC>Map Name";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabelMapName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMapName.AutoSize = true;
            this.gpgLabelMapName.AutoStyle = true;
            this.gpgLabelMapName.Font = new Font("Arial", 9.75f);
            this.gpgLabelMapName.ForeColor = Color.White;
            this.gpgLabelMapName.IgnoreMouseWheel = false;
            this.gpgLabelMapName.IsStyled = false;
            this.gpgLabelMapName.Location = new Point(0x85, 0x11);
            this.gpgLabelMapName.Name = "gpgLabelMapName";
            this.gpgLabelMapName.Size = new Size(0x43, 0x10);
            this.gpgLabelMapName.TabIndex = 0x11;
            this.gpgLabelMapName.Text = "gpgLabel2";
            this.gpgLabelMapName.TextStyle = TextStyles.Default;
            this.skinButtonTestMap.Anchor = AnchorStyles.Bottom;
            this.skinButtonTestMap.AutoStyle = true;
            this.skinButtonTestMap.BackColor = Color.Transparent;
            this.skinButtonTestMap.ButtonState = 0;
            this.skinButtonTestMap.DialogResult = DialogResult.OK;
            this.skinButtonTestMap.DisabledForecolor = Color.Gray;
            this.skinButtonTestMap.DrawColor = Color.White;
            this.skinButtonTestMap.DrawEdges = true;
            this.skinButtonTestMap.FocusColor = Color.Yellow;
            this.skinButtonTestMap.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonTestMap.ForeColor = Color.White;
            this.skinButtonTestMap.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonTestMap.IsStyled = true;
            this.skinButtonTestMap.Location = new Point(0xea, 0x13f);
            this.skinButtonTestMap.Name = "skinButtonTestMap";
            this.skinButtonTestMap.Size = new Size(0x7d, 0x1a);
            this.skinButtonTestMap.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonTestMap.TabIndex = 0x12;
            this.skinButtonTestMap.TabStop = true;
            this.skinButtonTestMap.Text = "<LOC>Test Map";
            this.skinButtonTestMap.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonTestMap.TextPadding = new Padding(0);
            this.skinButtonTestMap.Click += new EventHandler(this.skinButtonTestMap_Click);
            this.gpgLabel7.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(3, 0x103);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x240, 0x39);
            this.gpgLabel7.TabIndex = 0x13;
            this.gpgLabel7.Text = "";
            this.gpgLabel7.TextAlign = ContentAlignment.BottomCenter;
            this.gpgLabel7.TextStyle = TextStyles.Bold;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.skinButtonTestMap);
            base.Controls.Add(this.gpgLabelMapName);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgPanel1);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.pictureBoxPreview);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxTerrain);
            base.Controls.Add(this.gpgCheckBoxMission);
            base.Controls.Add(this.gpgCheckBoxCustomRules);
            base.Controls.Add(this.gpgCheckBoxRush);
            base.Controls.Add(this.gpgCheckBoxSeparate);
            base.Controls.Add(this.gpgCheckBoxWater);
            base.Controls.Add(this.gpgLabelMaxPlayers);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabelMapSize);
            base.Controls.Add(this.gpgLabel1);
            base.Name = "PnlMapUploadOptions";
            base.Size = new Size(590, 0x164);
            ((ISupportInitialize) this.pictureBoxPreview).EndInit();
            this.gpgTextBoxTerrain.Properties.EndInit();
            this.gpgPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void MapAttributeChanged(object sender, EventArgs e)
        {
            if (this.Map != null)
            {
                this.Map.TerrainType = this.gpgTextBoxTerrain.Text;
                this.Map.HasWater = new bool?(this.gpgCheckBoxWater.Checked);
                this.Map.IsSeparated = new bool?(this.gpgCheckBoxSeparate.Checked);
                this.Map.IsRushMap = new bool?(this.gpgCheckBoxRush.Checked);
                this.Map.HasCustomRuleset = new bool?(this.gpgCheckBoxCustomRules.Checked);
                this.Map.IsMission = new bool?(this.gpgCheckBoxMission.Checked);
            }
        }

        private void PnlMapDetailsView_Disposed(object sender, EventArgs e)
        {
            CustomMap.PreviewImageLoaded -= new EventHandler(this.CustomMap_PreviewImageLoaded);
        }

        private void skinButtonTestMap_Click(object sender, EventArgs e)
        {
            new DlgMapDiagnostics(this.Map.LocalFilePath).Show();
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

