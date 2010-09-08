namespace GPG.Multiplayer.Client.Games
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;

    public class DlgManageGames : DlgBase
    {
        public SkinButton btnLocateGame;
        private IContainer components = null;
        private GPGBorderPanel gpgBorderPanel1;
        private GPGBorderPanel gpgBorderPanel2;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private ListBox lbGames;
        public SkinButton sbCancel;
        public SkinButton sbSave;
        public SkinButton skinButton1;
        private SkinLabel skinLabel1;
        private GPGTextBox tbCommandLineArgs;
        private GPGTextBox tbGameLocation;

        public DlgManageGames()
        {
            this.InitializeComponent();
            this.lbGames.Items.AddRange(GameInformation.Games.ToArray());
            this.lbGames.DisplayMember = "GameDescription";
            this.lbGames.Sorted = true;
        }

        private void btnLocateGame_Click(object sender, EventArgs e)
        {
            GameInformation selectedItem = this.lbGames.SelectedItem as GameInformation;
            if (selectedItem != null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = selectedItem.GameDescription + " (" + selectedItem.ExeName + ")|" + selectedItem.ExeName + "|All Executables (*.exe)|*.exe";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.tbGameLocation.Text = dialog.FileName;
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

        private void DlgManageGames_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.gpgLabel2 = new GPGLabel();
            this.tbGameLocation = new GPGTextBox();
            this.gpgLabel3 = new GPGLabel();
            this.tbCommandLineArgs = new GPGTextBox();
            this.sbCancel = new SkinButton();
            this.sbSave = new SkinButton();
            this.skinButton1 = new SkinButton();
            this.gpgBorderPanel2 = new GPGBorderPanel();
            this.skinLabel1 = new SkinLabel();
            this.lbGames = new ListBox();
            this.gpgBorderPanel1 = new GPGBorderPanel();
            this.btnLocateGame = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.tbGameLocation.Properties.BeginInit();
            this.tbCommandLineArgs.Properties.BeginInit();
            this.gpgBorderPanel2.SuspendLayout();
            this.gpgBorderPanel1.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x2c3, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(13, 12);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x8a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "<LOC>Game Location";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.tbGameLocation.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbGameLocation.Location = new Point(0x10, 0x1f);
            this.tbGameLocation.Name = "tbGameLocation";
            this.tbGameLocation.Properties.Appearance.BackColor = Color.Black;
            this.tbGameLocation.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbGameLocation.Properties.Appearance.ForeColor = Color.White;
            this.tbGameLocation.Properties.Appearance.Options.UseBackColor = true;
            this.tbGameLocation.Properties.Appearance.Options.UseBorderColor = true;
            this.tbGameLocation.Properties.Appearance.Options.UseForeColor = true;
            this.tbGameLocation.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbGameLocation.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbGameLocation.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbGameLocation.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbGameLocation.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbGameLocation.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbGameLocation.Properties.BorderStyle = BorderStyles.Simple;
            this.tbGameLocation.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbGameLocation.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbGameLocation.Size = new Size(0x1a7, 20);
            this.tbGameLocation.TabIndex = 10;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(13, 0x43);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0xcc, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 11;
            this.gpgLabel3.Text = "<LOC>Command Line Arguments";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.tbCommandLineArgs.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbCommandLineArgs.Location = new Point(0x10, 0x56);
            this.tbCommandLineArgs.Name = "tbCommandLineArgs";
            this.tbCommandLineArgs.Properties.Appearance.BackColor = Color.Black;
            this.tbCommandLineArgs.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbCommandLineArgs.Properties.Appearance.ForeColor = Color.White;
            this.tbCommandLineArgs.Properties.Appearance.Options.UseBackColor = true;
            this.tbCommandLineArgs.Properties.Appearance.Options.UseBorderColor = true;
            this.tbCommandLineArgs.Properties.Appearance.Options.UseForeColor = true;
            this.tbCommandLineArgs.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbCommandLineArgs.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbCommandLineArgs.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbCommandLineArgs.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbCommandLineArgs.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbCommandLineArgs.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbCommandLineArgs.Properties.BorderStyle = BorderStyles.Simple;
            this.tbCommandLineArgs.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbCommandLineArgs.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbCommandLineArgs.Properties.ReadOnly = true;
            this.tbCommandLineArgs.Size = new Size(0x1cd, 20);
            this.tbCommandLineArgs.TabIndex = 12;
            this.sbCancel.AutoStyle = true;
            this.sbCancel.BackColor = Color.Black;
            this.sbCancel.ButtonState = 0;
            this.sbCancel.DialogResult = DialogResult.OK;
            this.sbCancel.DisabledForecolor = Color.Gray;
            this.sbCancel.DrawColor = Color.White;
            this.sbCancel.DrawEdges = true;
            this.sbCancel.FocusColor = Color.Yellow;
            this.sbCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.sbCancel.ForeColor = Color.White;
            this.sbCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.sbCancel.IsStyled = true;
            this.sbCancel.Location = new Point(0x91, 0x7c);
            this.sbCancel.Name = "sbCancel";
            this.sbCancel.Size = new Size(0x7b, 0x1c);
            this.sbCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.sbCancel, null);
            this.sbCancel.TabIndex = 0x19;
            this.sbCancel.TabStop = true;
            this.sbCancel.Text = "<LOC>Cancel";
            this.sbCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.sbCancel.TextPadding = new Padding(0);
            this.sbCancel.Click += new EventHandler(this.sbCancel_Click);
            this.sbSave.AutoStyle = true;
            this.sbSave.BackColor = Color.Black;
            this.sbSave.ButtonState = 0;
            this.sbSave.DialogResult = DialogResult.OK;
            this.sbSave.DisabledForecolor = Color.Gray;
            this.sbSave.DrawColor = Color.White;
            this.sbSave.DrawEdges = true;
            this.sbSave.FocusColor = Color.Yellow;
            this.sbSave.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.sbSave.ForeColor = Color.White;
            this.sbSave.HorizontalScalingMode = ScalingModes.Tile;
            this.sbSave.IsStyled = true;
            this.sbSave.Location = new Point(0x10, 0x7c);
            this.sbSave.Name = "sbSave";
            this.sbSave.Size = new Size(0x7b, 0x1c);
            this.sbSave.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.sbSave, null);
            this.sbSave.TabIndex = 0x18;
            this.sbSave.TabStop = true;
            this.sbSave.Text = "<LOC>Save Changes";
            this.sbSave.TextAlign = ContentAlignment.MiddleCenter;
            this.sbSave.TextPadding = new Padding(0);
            this.sbSave.Click += new EventHandler(this.sbSave_Click);
            this.skinButton1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButton1.AutoStyle = true;
            this.skinButton1.BackColor = Color.Black;
            this.skinButton1.ButtonState = 0;
            this.skinButton1.DialogResult = DialogResult.OK;
            this.skinButton1.DisabledForecolor = Color.Gray;
            this.skinButton1.DrawColor = Color.White;
            this.skinButton1.DrawEdges = true;
            this.skinButton1.Enabled = false;
            this.skinButton1.FocusColor = Color.Yellow;
            this.skinButton1.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButton1.ForeColor = Color.White;
            this.skinButton1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButton1.IsStyled = true;
            this.skinButton1.Location = new Point(0x1a, 0x180);
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.Size = new Size(0xe3, 0x1c);
            this.skinButton1.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButton1, null);
            this.skinButton1.TabIndex = 0x1a;
            this.skinButton1.TabStop = true;
            this.skinButton1.Text = "<LOC>Add New Game";
            this.skinButton1.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButton1.TextPadding = new Padding(0);
            this.skinButton1.Visible = false;
            this.gpgBorderPanel2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgBorderPanel2.Controls.Add(this.skinLabel1);
            this.gpgBorderPanel2.Controls.Add(this.lbGames);
            this.gpgBorderPanel2.GPGBorderStyle = GPGBorderStyle.Rectangle;
            this.gpgBorderPanel2.Location = new Point(0x1a, 0x53);
            this.gpgBorderPanel2.Name = "gpgBorderPanel2";
            this.gpgBorderPanel2.Size = new Size(0xe2, 0x126);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel2, null);
            this.gpgBorderPanel2.TabIndex = 0x1b;
            this.gpgBorderPanel2.Text = "gpgBorderPanel2";
            this.skinLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(3, 3);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(220, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 13;
            this.skinLabel1.Text = "<LOC>Games";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(0);
            this.lbGames.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lbGames.BackColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.lbGames.BorderStyle = BorderStyle.None;
            this.lbGames.ForeColor = Color.White;
            this.lbGames.FormattingEnabled = true;
            this.lbGames.Location = new Point(3, 0x18);
            this.lbGames.Name = "lbGames";
            this.lbGames.Size = new Size(220, 260);
            base.ttDefault.SetSuperTip(this.lbGames, null);
            this.lbGames.TabIndex = 12;
            this.lbGames.SelectedValueChanged += new EventHandler(this.lbGames_SelectedValueChanged);
            this.gpgBorderPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgBorderPanel1.Controls.Add(this.btnLocateGame);
            this.gpgBorderPanel1.Controls.Add(this.gpgLabel2);
            this.gpgBorderPanel1.Controls.Add(this.tbGameLocation);
            this.gpgBorderPanel1.Controls.Add(this.tbCommandLineArgs);
            this.gpgBorderPanel1.Controls.Add(this.sbCancel);
            this.gpgBorderPanel1.Controls.Add(this.gpgLabel3);
            this.gpgBorderPanel1.Controls.Add(this.sbSave);
            this.gpgBorderPanel1.GPGBorderStyle = GPGBorderStyle.Web;
            this.gpgBorderPanel1.Location = new Point(0x102, 0x56);
            this.gpgBorderPanel1.Name = "gpgBorderPanel1";
            this.gpgBorderPanel1.Size = new Size(0x1e8, 0x123);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel1, null);
            this.gpgBorderPanel1.TabIndex = 0x1c;
            this.gpgBorderPanel1.Text = "gpgBorderPanel1";
            this.btnLocateGame.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnLocateGame.AutoStyle = true;
            this.btnLocateGame.BackColor = Color.Black;
            this.btnLocateGame.ButtonState = 0;
            this.btnLocateGame.DialogResult = DialogResult.OK;
            this.btnLocateGame.DisabledForecolor = Color.Gray;
            this.btnLocateGame.DrawColor = Color.White;
            this.btnLocateGame.DrawEdges = true;
            this.btnLocateGame.FocusColor = Color.Yellow;
            this.btnLocateGame.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnLocateGame.ForeColor = Color.White;
            this.btnLocateGame.HorizontalScalingMode = ScalingModes.Tile;
            this.btnLocateGame.IsStyled = true;
            this.btnLocateGame.Location = new Point(0x1bc, 0x1f);
            this.btnLocateGame.Name = "btnLocateGame";
            this.btnLocateGame.Size = new Size(0x21, 20);
            this.btnLocateGame.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnLocateGame, null);
            this.btnLocateGame.TabIndex = 0x1a;
            this.btnLocateGame.TabStop = true;
            this.btnLocateGame.Text = "...";
            this.btnLocateGame.TextAlign = ContentAlignment.MiddleCenter;
            this.btnLocateGame.TextPadding = new Padding(0);
            this.btnLocateGame.Click += new EventHandler(this.btnLocateGame_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2fe, 480);
            base.Controls.Add(this.gpgBorderPanel1);
            base.Controls.Add(this.gpgBorderPanel2);
            base.Controls.Add(this.skinButton1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgManageGames";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Manage Games";
            base.Load += new EventHandler(this.DlgManageGames_Load);
            base.Controls.SetChildIndex(this.skinButton1, 0);
            base.Controls.SetChildIndex(this.gpgBorderPanel2, 0);
            base.Controls.SetChildIndex(this.gpgBorderPanel1, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.tbGameLocation.Properties.EndInit();
            this.tbCommandLineArgs.Properties.EndInit();
            this.gpgBorderPanel2.ResumeLayout(false);
            this.gpgBorderPanel1.ResumeLayout(false);
            this.gpgBorderPanel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void lbGames_Load(object sender, EventArgs e)
        {
        }

        private void lbGames_SelectedValueChanged(object sender, EventArgs e)
        {
            GameInformation selectedItem = this.lbGames.SelectedItem as GameInformation;
            if (selectedItem != null)
            {
                this.tbCommandLineArgs.Text = selectedItem.ForcedCommandLineArgs;
                this.tbGameLocation.Text = selectedItem.GameLocation;
                this.sbSave.Enabled = true;
                this.sbCancel.Enabled = true;
            }
            else
            {
                this.tbCommandLineArgs.Text = "";
                this.tbGameLocation.Text = "";
                this.sbSave.Enabled = false;
                this.sbCancel.Enabled = false;
            }
        }

        private void sbCancel_Click(object sender, EventArgs e)
        {
            this.lbGames.SelectedIndex = -1;
            base.Close();
        }

        private void sbSave_Click(object sender, EventArgs e)
        {
            GameInformation selectedItem = this.lbGames.SelectedItem as GameInformation;
            if (selectedItem != null)
            {
                if (File.Exists(this.tbGameLocation.Text) || (this.tbGameLocation.Text == ""))
                {
                    selectedItem.GameLocation = this.tbGameLocation.Text;
                    selectedItem.ForcedCommandLineArgs = this.tbCommandLineArgs.Text;
                    GameInformation.SaveGames();
                }
                else
                {
                    DlgMessage.ShowDialog(Loc.Get("<LOC>Unable to save.  This is not a valid file location."));
                }
            }
        }
    }
}

