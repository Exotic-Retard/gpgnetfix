namespace GPG.Multiplayer.Client.Vaulting.Maps
{
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PnlMapDetailsView : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabelCustomRules;
        private GPGLabel gpgLabelMapDesc;
        private GPGLabel gpgLabelMapName;
        private GPGLabel gpgLabelMapSize;
        private GPGLabel gpgLabelMaxPlayers;
        private GPGLabel gpgLabelMission;
        private GPGLabel gpgLabelRush;
        private GPGLabel gpgLabelSeparation;
        private GPGLabel gpgLabelTerrain;
        private GPGLabel gpgLabelWater;
        private GPGPanel gpgPanel1;
        private CustomMap mMap;
        private PictureBox pictureBoxPreview;

        public PnlMapDetailsView(CustomMap map)
        {
            this.InitializeComponent();
            this.mMap = map;
            CustomMap.PreviewImageLoaded += new EventHandler(this.CustomMap_PreviewImageLoaded);
            base.Disposed += new EventHandler(this.PnlMapDetailsView_Disposed);
            this.gpgLabelMapName.Text = map.MapName;
            this.pictureBoxPreview.Image = map.PreviewImage128;
            this.gpgLabelMapSize.Text = map.SizeDisplay;
            this.gpgLabelMaxPlayers.Text = map.MaxPlayers.ToString();
            this.gpgLabelTerrain.Text = map.TerrainType;
            this.gpgLabelMapDesc.Text = map.MapDescription;
            if (!(!map.HasCustomRuleset.HasValue ? true : !map.HasCustomRuleset.Value))
            {
                this.gpgLabelCustomRules.Text = "<LOC>Custom Rules";
            }
            else
            {
                this.gpgLabelCustomRules.Text = "<LOC>Standard Rules";
            }
            if (!(!map.IsMission.HasValue ? true : !map.IsMission.Value))
            {
                this.gpgLabelMission.Text = "<LOC>Is a Mission";
            }
            else
            {
                this.gpgLabelMission.Text = "<LOC>Not a Mission";
            }
            if (!(!map.IsRushMap.HasValue ? true : !map.IsRushMap.Value))
            {
                this.gpgLabelRush.Text = "<LOC>Is a Rushing Map";
            }
            else
            {
                this.gpgLabelRush.Text = "<LOC>Not a Rushing Map";
            }
            if (!(!map.IsSeparated.HasValue ? true : !map.IsSeparated.Value))
            {
                this.gpgLabelSeparation.Text = "<LOC>Has Player Separation";
            }
            else
            {
                this.gpgLabelSeparation.Text = "<LOC>No Player Separation";
            }
            if (!(!map.HasWater.HasValue ? true : !map.HasWater.Value))
            {
                this.gpgLabelWater.Text = "<LOC>Has Water";
            }
            else
            {
                this.gpgLabelWater.Text = "<LOC>Does Not Have Water";
            }
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
            this.gpgLabel4 = new GPGLabel();
            this.pictureBoxPreview = new PictureBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabelMaxPlayers = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelMapSize = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelWater = new GPGLabel();
            this.gpgLabelRush = new GPGLabel();
            this.gpgLabelMission = new GPGLabel();
            this.gpgLabelCustomRules = new GPGLabel();
            this.gpgLabelSeparation = new GPGLabel();
            this.gpgLabelTerrain = new GPGLabel();
            this.gpgPanel1 = new GPGPanel();
            this.gpgLabelMapDesc = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabelMapName = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            ((ISupportInitialize) this.pictureBoxPreview).BeginInit();
            this.gpgPanel1.SuspendLayout();
            base.SuspendLayout();
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel4.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x8b, 0x63);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x252, 0x11);
            this.gpgLabel4.TabIndex = 0x19;
            this.gpgLabel4.Text = "<LOC>Parameters";
            this.gpgLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel4.TextStyle = TextStyles.Custom;
            this.pictureBoxPreview.Location = new Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new Size(0x80, 0x80);
            this.pictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreview.TabIndex = 0x18;
            this.pictureBoxPreview.TabStop = false;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x1ac, 0x2b);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x79, 0x11);
            this.gpgLabel2.TabIndex = 0x17;
            this.gpgLabel2.Text = "<LOC>Terrain Type";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgLabelMaxPlayers.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMaxPlayers.AutoSize = true;
            this.gpgLabelMaxPlayers.AutoStyle = true;
            this.gpgLabelMaxPlayers.Font = new Font("Arial", 9.75f);
            this.gpgLabelMaxPlayers.ForeColor = Color.White;
            this.gpgLabelMaxPlayers.IgnoreMouseWheel = false;
            this.gpgLabelMaxPlayers.IsStyled = false;
            this.gpgLabelMaxPlayers.Location = new Point(0x110, 0x41);
            this.gpgLabelMaxPlayers.Name = "gpgLabelMaxPlayers";
            this.gpgLabelMaxPlayers.Size = new Size(0x43, 0x10);
            this.gpgLabelMaxPlayers.TabIndex = 0x10;
            this.gpgLabelMaxPlayers.Text = "gpgLabel2";
            this.gpgLabelMaxPlayers.TextStyle = TextStyles.Default;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x110, 0x2b);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x7b, 0x11);
            this.gpgLabel3.TabIndex = 15;
            this.gpgLabel3.Text = "<LOC>Max Players";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.gpgLabelMapSize.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMapSize.AutoSize = true;
            this.gpgLabelMapSize.AutoStyle = true;
            this.gpgLabelMapSize.Font = new Font("Arial", 9.75f);
            this.gpgLabelMapSize.ForeColor = Color.White;
            this.gpgLabelMapSize.IgnoreMouseWheel = false;
            this.gpgLabelMapSize.IsStyled = false;
            this.gpgLabelMapSize.Location = new Point(0x8b, 0x41);
            this.gpgLabelMapSize.Name = "gpgLabelMapSize";
            this.gpgLabelMapSize.Size = new Size(0x43, 0x10);
            this.gpgLabelMapSize.TabIndex = 14;
            this.gpgLabelMapSize.Text = "gpgLabel2";
            this.gpgLabelMapSize.TextStyle = TextStyles.Default;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x8b, 0x2b);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x252, 0x11);
            this.gpgLabel1.TabIndex = 13;
            this.gpgLabel1.Text = "<LOC>Map Size";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgLabelWater.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelWater.AutoSize = true;
            this.gpgLabelWater.AutoStyle = true;
            this.gpgLabelWater.Font = new Font("Arial", 9.75f);
            this.gpgLabelWater.ForeColor = Color.White;
            this.gpgLabelWater.IgnoreMouseWheel = false;
            this.gpgLabelWater.IsStyled = false;
            this.gpgLabelWater.Location = new Point(0x8b, 0x79);
            this.gpgLabelWater.Name = "gpgLabelWater";
            this.gpgLabelWater.Size = new Size(0x43, 0x10);
            this.gpgLabelWater.TabIndex = 0x1a;
            this.gpgLabelWater.Text = "gpgLabel2";
            this.gpgLabelWater.TextStyle = TextStyles.Default;
            this.gpgLabelRush.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelRush.AutoSize = true;
            this.gpgLabelRush.AutoStyle = true;
            this.gpgLabelRush.Font = new Font("Arial", 9.75f);
            this.gpgLabelRush.ForeColor = Color.White;
            this.gpgLabelRush.IgnoreMouseWheel = false;
            this.gpgLabelRush.IsStyled = false;
            this.gpgLabelRush.Location = new Point(0x8b, 0x8e);
            this.gpgLabelRush.Name = "gpgLabelRush";
            this.gpgLabelRush.Size = new Size(0x43, 0x10);
            this.gpgLabelRush.TabIndex = 0x1b;
            this.gpgLabelRush.Text = "gpgLabel2";
            this.gpgLabelRush.TextStyle = TextStyles.Default;
            this.gpgLabelMission.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMission.AutoSize = true;
            this.gpgLabelMission.AutoStyle = true;
            this.gpgLabelMission.Font = new Font("Arial", 9.75f);
            this.gpgLabelMission.ForeColor = Color.White;
            this.gpgLabelMission.IgnoreMouseWheel = false;
            this.gpgLabelMission.IsStyled = false;
            this.gpgLabelMission.Location = new Point(0x8b, 0xa3);
            this.gpgLabelMission.Name = "gpgLabelMission";
            this.gpgLabelMission.Size = new Size(0x43, 0x10);
            this.gpgLabelMission.TabIndex = 0x1c;
            this.gpgLabelMission.Text = "gpgLabel2";
            this.gpgLabelMission.TextStyle = TextStyles.Default;
            this.gpgLabelCustomRules.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelCustomRules.AutoSize = true;
            this.gpgLabelCustomRules.AutoStyle = true;
            this.gpgLabelCustomRules.Font = new Font("Arial", 9.75f);
            this.gpgLabelCustomRules.ForeColor = Color.White;
            this.gpgLabelCustomRules.IgnoreMouseWheel = false;
            this.gpgLabelCustomRules.IsStyled = false;
            this.gpgLabelCustomRules.Location = new Point(0x153, 0x8e);
            this.gpgLabelCustomRules.Name = "gpgLabelCustomRules";
            this.gpgLabelCustomRules.Size = new Size(0x43, 0x10);
            this.gpgLabelCustomRules.TabIndex = 30;
            this.gpgLabelCustomRules.Text = "gpgLabel2";
            this.gpgLabelCustomRules.TextStyle = TextStyles.Default;
            this.gpgLabelSeparation.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelSeparation.AutoSize = true;
            this.gpgLabelSeparation.AutoStyle = true;
            this.gpgLabelSeparation.Font = new Font("Arial", 9.75f);
            this.gpgLabelSeparation.ForeColor = Color.White;
            this.gpgLabelSeparation.IgnoreMouseWheel = false;
            this.gpgLabelSeparation.IsStyled = false;
            this.gpgLabelSeparation.Location = new Point(0x153, 0x79);
            this.gpgLabelSeparation.Name = "gpgLabelSeparation";
            this.gpgLabelSeparation.Size = new Size(0x43, 0x10);
            this.gpgLabelSeparation.TabIndex = 0x1d;
            this.gpgLabelSeparation.Text = "gpgLabel2";
            this.gpgLabelSeparation.TextStyle = TextStyles.Default;
            this.gpgLabelTerrain.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelTerrain.AutoSize = true;
            this.gpgLabelTerrain.AutoStyle = true;
            this.gpgLabelTerrain.Font = new Font("Arial", 9.75f);
            this.gpgLabelTerrain.ForeColor = Color.White;
            this.gpgLabelTerrain.IgnoreMouseWheel = false;
            this.gpgLabelTerrain.IsStyled = false;
            this.gpgLabelTerrain.Location = new Point(0x1ab, 0x41);
            this.gpgLabelTerrain.Name = "gpgLabelTerrain";
            this.gpgLabelTerrain.Size = new Size(0x43, 0x10);
            this.gpgLabelTerrain.TabIndex = 0x1f;
            this.gpgLabelTerrain.Text = "gpgLabel2";
            this.gpgLabelTerrain.TextStyle = TextStyles.Default;
            this.gpgPanel1.AutoScroll = true;
            this.gpgPanel1.Controls.Add(this.gpgLabelMapDesc);
            this.gpgPanel1.Location = new Point(0, 0xd1);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(0x2db, 0x23);
            this.gpgPanel1.TabIndex = 0x21;
            this.gpgLabelMapDesc.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelMapDesc.AutoStyle = true;
            this.gpgLabelMapDesc.Font = new Font("Arial", 9.75f);
            this.gpgLabelMapDesc.ForeColor = Color.White;
            this.gpgLabelMapDesc.IgnoreMouseWheel = false;
            this.gpgLabelMapDesc.IsStyled = false;
            this.gpgLabelMapDesc.Location = new Point(0, 0);
            this.gpgLabelMapDesc.Name = "gpgLabelMapDesc";
            this.gpgLabelMapDesc.Size = new Size(0x2c7, 15);
            this.gpgLabelMapDesc.TabIndex = 14;
            this.gpgLabelMapDesc.Text = "gpgLabel6";
            this.gpgLabelMapDesc.TextStyle = TextStyles.Default;
            this.gpgLabel5.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel5.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0, 0xbc);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(730, 0x11);
            this.gpgLabel5.TabIndex = 0x20;
            this.gpgLabel5.Text = "<LOC>Map Description";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.gpgLabelMapName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelMapName.AutoSize = true;
            this.gpgLabelMapName.AutoStyle = true;
            this.gpgLabelMapName.Font = new Font("Arial", 9.75f);
            this.gpgLabelMapName.ForeColor = Color.White;
            this.gpgLabelMapName.IgnoreMouseWheel = false;
            this.gpgLabelMapName.IsStyled = false;
            this.gpgLabelMapName.Location = new Point(0x8b, 0x11);
            this.gpgLabelMapName.Name = "gpgLabelMapName";
            this.gpgLabelMapName.Size = new Size(0x43, 0x10);
            this.gpgLabelMapName.TabIndex = 0x23;
            this.gpgLabelMapName.Text = "gpgLabel2";
            this.gpgLabelMapName.TextStyle = TextStyles.Default;
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x8b, 0);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x252, 0x11);
            this.gpgLabel6.TabIndex = 0x22;
            this.gpgLabel6.Text = "<LOC>Map Name";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLabelMapName);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgPanel1);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabelTerrain);
            base.Controls.Add(this.gpgLabelCustomRules);
            base.Controls.Add(this.gpgLabelSeparation);
            base.Controls.Add(this.gpgLabelMission);
            base.Controls.Add(this.gpgLabelRush);
            base.Controls.Add(this.gpgLabelWater);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.pictureBoxPreview);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabelMaxPlayers);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabelMapSize);
            base.Controls.Add(this.gpgLabel1);
            base.Name = "PnlMapDetailsView";
            base.Size = new Size(0x2db, 0xf6);
            ((ISupportInitialize) this.pictureBoxPreview).EndInit();
            this.gpgPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PnlMapDetailsView_Disposed(object sender, EventArgs e)
        {
            CustomMap.PreviewImageLoaded -= new EventHandler(this.CustomMap_PreviewImageLoaded);
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

