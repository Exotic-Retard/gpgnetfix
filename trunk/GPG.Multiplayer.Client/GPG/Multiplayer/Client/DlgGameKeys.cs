namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgGameKeys : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabelError;
        private GPGTextBox gpgTextBoxAddBeta;
        private GroupBox groupBox2;
        private ListBox listBoxBetaKeys;
        private SkinButton skinButtonAddBetaKey;
        private SkinButton skinButtonOK;
        private SkinButton skinButtonRemoveBetaKey;

        public DlgGameKeys(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.RefreshKeys();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Error(string error)
        {
            this.gpgLabelError.Text = Loc.Get(error);
            this.gpgLabelError.Visible = true;
        }

        private void InitializeComponent()
        {
            this.groupBox2 = new GroupBox();
            this.listBoxBetaKeys = new ListBox();
            this.skinButtonOK = new SkinButton();
            this.gpgTextBoxAddBeta = new GPGTextBox();
            this.skinButtonAddBetaKey = new SkinButton();
            this.gpgLabelError = new GPGLabel();
            this.skinButtonRemoveBetaKey = new SkinButton();
            this.groupBox2.SuspendLayout();
            this.gpgTextBoxAddBeta.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.listBoxBetaKeys);
            this.groupBox2.Location = new Point(12, 0x53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x1bf, 0x7d);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "<LOC>CD Keys";
            this.listBoxBetaKeys.BackColor = Color.Black;
            this.listBoxBetaKeys.BorderStyle = BorderStyle.None;
            this.listBoxBetaKeys.Dock = DockStyle.Fill;
            this.listBoxBetaKeys.ForeColor = Color.White;
            this.listBoxBetaKeys.FormattingEnabled = true;
            this.listBoxBetaKeys.Location = new Point(3, 0x10);
            this.listBoxBetaKeys.Name = "listBoxBetaKeys";
            this.listBoxBetaKeys.Size = new Size(0x1b9, 0x68);
            this.listBoxBetaKeys.TabIndex = 1;
            this.skinButtonOK.Anchor = AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0xc1, 0x119);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x67, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 9;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.gpgTextBoxAddBeta.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgTextBoxAddBeta.Location = new Point(12, 0xd9);
            this.gpgTextBoxAddBeta.Name = "gpgTextBoxAddBeta";
            this.gpgTextBoxAddBeta.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxAddBeta.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxAddBeta.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxAddBeta.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxAddBeta.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxAddBeta.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxAddBeta.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxAddBeta.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxAddBeta.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxAddBeta.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxAddBeta.Size = new Size(0x11c, 20);
            this.gpgTextBoxAddBeta.TabIndex = 9;
            this.skinButtonAddBetaKey.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonAddBetaKey.AutoStyle = true;
            this.skinButtonAddBetaKey.BackColor = Color.Black;
            this.skinButtonAddBetaKey.DialogResult = DialogResult.OK;
            this.skinButtonAddBetaKey.DisabledForecolor = Color.Gray;
            this.skinButtonAddBetaKey.DrawEdges = true;
            this.skinButtonAddBetaKey.FocusColor = Color.Yellow;
            this.skinButtonAddBetaKey.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAddBetaKey.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAddBetaKey.IsStyled = true;
            this.skinButtonAddBetaKey.Location = new Point(0x12d, 0xd9);
            this.skinButtonAddBetaKey.Name = "skinButtonAddBetaKey";
            this.skinButtonAddBetaKey.Size = new Size(0x4c, 20);
            this.skinButtonAddBetaKey.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonAddBetaKey.TabIndex = 0;
            this.skinButtonAddBetaKey.Text = "<LOC>Add";
            this.skinButtonAddBetaKey.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAddBetaKey.TextPadding = new Padding(0);
            this.skinButtonAddBetaKey.Click += new EventHandler(this.skinButtonAddBetaKey_Click);
            this.gpgLabelError.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelError.AutoStyle = true;
            this.gpgLabelError.Font = new Font("Arial", 9.75f);
            this.gpgLabelError.ForeColor = Color.Red;
            this.gpgLabelError.IgnoreMouseWheel = false;
            this.gpgLabelError.IsStyled = false;
            this.gpgLabelError.Location = new Point(14, 240);
            this.gpgLabelError.Name = "gpgLabelError";
            this.gpgLabelError.Size = new Size(0x1be, 0x26);
            this.gpgLabelError.TabIndex = 11;
            this.gpgLabelError.Text = "gpgLabel1";
            this.gpgLabelError.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelError.TextStyle = TextStyles.Error;
            this.gpgLabelError.Visible = false;
            this.skinButtonRemoveBetaKey.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonRemoveBetaKey.AutoStyle = true;
            this.skinButtonRemoveBetaKey.BackColor = Color.Black;
            this.skinButtonRemoveBetaKey.DialogResult = DialogResult.OK;
            this.skinButtonRemoveBetaKey.DisabledForecolor = Color.Gray;
            this.skinButtonRemoveBetaKey.DrawEdges = true;
            this.skinButtonRemoveBetaKey.FocusColor = Color.Yellow;
            this.skinButtonRemoveBetaKey.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRemoveBetaKey.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRemoveBetaKey.IsStyled = true;
            this.skinButtonRemoveBetaKey.Location = new Point(0x17f, 0xd9);
            this.skinButtonRemoveBetaKey.Name = "skinButtonRemoveBetaKey";
            this.skinButtonRemoveBetaKey.Size = new Size(0x4c, 20);
            this.skinButtonRemoveBetaKey.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonRemoveBetaKey.TabIndex = 12;
            this.skinButtonRemoveBetaKey.Text = "<LOC>Remove";
            this.skinButtonRemoveBetaKey.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRemoveBetaKey.TextPadding = new Padding(0);
            this.skinButtonRemoveBetaKey.Click += new EventHandler(this.skinButtonRemoveBetaKey_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonOK;
            base.ClientSize = new Size(0x1d9, 370);
            base.Controls.Add(this.skinButtonRemoveBetaKey);
            base.Controls.Add(this.gpgTextBoxAddBeta);
            base.Controls.Add(this.gpgLabelError);
            base.Controls.Add(this.skinButtonAddBetaKey);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.skinButtonOK);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0x1d9, 370);
            this.MinimumSize = new Size(0x1d9, 370);
            base.Name = "DlgGameKeys";
            this.Text = "<LOC>Game Keys";
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.groupBox2, 0);
            base.Controls.SetChildIndex(this.skinButtonAddBetaKey, 0);
            base.Controls.SetChildIndex(this.gpgLabelError, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxAddBeta, 0);
            base.Controls.SetChildIndex(this.skinButtonRemoveBetaKey, 0);
            this.groupBox2.ResumeLayout(false);
            this.gpgTextBoxAddBeta.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void RefreshKeys()
        {
            this.listBoxBetaKeys.Items.Clear();
            foreach (GameKey key in GameKey.BetaKeys)
            {
                this.listBoxBetaKeys.Items.Add(key);
            }
            this.listBoxBetaKeys.Refresh();
        }

        private void skinButtonAddBetaKey_Click(object sender, EventArgs e)
        {
            if ((this.gpgTextBoxAddBeta.Text == null) || (this.gpgTextBoxAddBeta.Text.Length < 1))
            {
                this.Error(Loc.Get("<LOC>Please enter a valid CD key to continue.."));
            }
            else if (this.gpgTextBoxAddBeta.Text.Replace("-", "").Length < 0x10)
            {
                this.Error(Loc.Get("<LOC>You have entered an invalid CD key. Please try again."));
            }
            else if (this.gpgTextBoxAddBeta.Text.Replace("-", "").Length > 0x10)
            {
                this.Error(Loc.Get("<LOC>Invalid CD key."));
            }
            else
            {
                int num;
                string text = this.gpgTextBoxAddBeta.Text;
                if (text.IndexOf("-") < 0)
                {
                    for (num = 1; num < 4; num++)
                    {
                        text = text.Insert(num * 4, "-");
                    }
                }
                if (DataAccess.ExecuteQuery("AddBetaKey", new object[] { this.gpgTextBoxAddBeta.Text }))
                {
                    this.gpgTextBoxAddBeta.Text = "";
                    GameKey.BetaKeys = DataAccess.GetObjects<GameKey>("GetBetaKeys", new object[0]);
                    for (num = 0; num < GameKey.BetaKeys.Count; num++)
                    {
                        GameKey.BetaKeys.IndexObject(GameKey.BetaKeys[num]);
                    }
                    this.RefreshKeys();
                }
                else
                {
                    this.Error(Loc.Get("<LOC>Invalid CD key."));
                }
            }
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void skinButtonRemoveBetaKey_Click(object sender, EventArgs e)
        {
            if ((this.listBoxBetaKeys.SelectedIndex >= 0) && (this.listBoxBetaKeys.SelectedItem is GameKey))
            {
                string keyCharacters = (this.listBoxBetaKeys.SelectedItem as GameKey).KeyCharacters;
                DataAccess.ExecuteQuery("RemoveBetaKey", new object[] { keyCharacters });
                if (GameKey.BetaKeys.RemoveByIndex("chars", keyCharacters))
                {
                    this.RefreshKeys();
                }
            }
            else
            {
                this.Error(Loc.Get("<LOC>No CD key selected."));
            }
        }
    }
}

