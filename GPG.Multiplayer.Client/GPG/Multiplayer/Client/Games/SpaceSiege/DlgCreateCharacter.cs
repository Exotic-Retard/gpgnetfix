namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
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

    public class DlgCreateCharacter : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGTextBox gpgTextBoxName;
        private PlayerCharacter mCharacter;
        private GPGPanel SelectedCharacterColorPanel;
        private GPGPanel SelectedHeadPanel;
        private GPGPanel SelectedHRVColorPanel;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgCreateCharacter()
        {
            this.components = null;
            this.SelectedCharacterColorPanel = null;
            this.SelectedHRVColorPanel = null;
            this.SelectedHeadPanel = null;
            this.InitializeComponent();
            this.mCharacter = new PlayerCharacter();
            this.RefreshCharacterDisplay();
        }

        public DlgCreateCharacter(PlayerCharacter character)
        {
            this.components = null;
            this.SelectedCharacterColorPanel = null;
            this.SelectedHRVColorPanel = null;
            this.SelectedHeadPanel = null;
            this.InitializeComponent();
            this.mCharacter = character;
            this.RefreshCharacterDisplay();
        }

        private void characterHead2_Paint(object sender, PaintEventArgs e)
        {
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
            this.gpgTextBoxName = new GPGTextBox();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxName.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x143, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x1a, 0x60);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x90, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 8;
            this.gpgLabel1.Text = "<LOC>Character Name";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxName.Location = new Point(0x19, 0x73);
            this.gpgTextBoxName.Name = "gpgTextBoxName";
            this.gpgTextBoxName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxName.Properties.MaxLength = 0x16;
            this.gpgTextBoxName.Size = new Size(0x14e, 20);
            this.gpgTextBoxName.TabIndex = 9;
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
            this.skinButtonCancel.Location = new Point(0x9c, 0x8d);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x7d, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 10;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
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
            this.skinButtonOK.Location = new Point(0x19, 0x8d);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x7d, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 11;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x17e, 0xfc);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.gpgTextBoxName);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x17e, 0xfc);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x17e, 0xfc);
            base.Name = "DlgCreateCharacter";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Create a New Character";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxName, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxName.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.gpgTextBoxName.Text = this.Character.CharacterName;
            base.OnLoad(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void RefreshCharacterDisplay()
        {
        }

        private void SelectCharacterColor(object sender, EventArgs e)
        {
            if (this.SelectedCharacterColorPanel != null)
            {
                this.SelectedCharacterColorPanel.DrawBorder = false;
            }
            GPGPanel panel = sender as GPGPanel;
            panel.DrawBorder = true;
            this.SelectedCharacterColorPanel = panel;
            this.Character.CharacterColor = panel.BackColor;
            this.RefreshCharacterDisplay();
        }

        private void SelectHead(object sender, EventArgs e)
        {
            if (this.SelectedHeadPanel != null)
            {
                this.SelectedHeadPanel.DrawBorder = false;
            }
            GPGPanel panel = sender as GPGPanel;
            panel.DrawBorder = true;
            this.SelectedHeadPanel = panel;
            this.Character.Head = Convert.ToInt32(panel.Tag);
            this.RefreshCharacterDisplay();
        }

        private void SelectHRVColor(object sender, EventArgs e)
        {
            if (this.SelectedHRVColorPanel != null)
            {
                this.SelectedHRVColorPanel.DrawBorder = false;
            }
            GPGPanel panel = sender as GPGPanel;
            panel.DrawBorder = true;
            this.SelectedHRVColorPanel = panel;
            this.Character.RobotColor = panel.BackColor;
            this.RefreshCharacterDisplay();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (this.gpgTextBoxName.Text.Length < 1)
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Your Character must have a name.", new object[0]);
                }
                else if (!(TextUtil.IsAlphaNumericString(this.gpgTextBoxName.Text) && (this.gpgTextBoxName.Text.IndexOf('|') < 0)))
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Name contains illegal characters.", new object[0]);
                }
                else
                {
                    string str3;
                    StreamWriter writer;
                    base.SetStatus("<LOC>Creating Character", new object[0]);
                    this.Character.CharacterName = this.gpgTextBoxName.Text;
                    string filePath = this.Character.FilePath;
                    string path = PlayerCharacter.CharacterFilePath + @"\characters.gpgnet";
                    if (File.Exists(path))
                    {
                        StreamReader reader = new StreamReader(path);
                        str3 = reader.ReadToEnd();
                        reader.Close();
                        str3 = str3 + this.Character.CharacterName + ",0,0\x000e";
                        writer = new StreamWriter(path);
                        writer.Write(str3);
                        writer.Flush();
                        writer.Close();
                    }
                    else
                    {
                        str3 = this.Character.CharacterName + ",0,0\x000e";
                        writer = new StreamWriter(path);
                        writer.Write(str3);
                        writer.Flush();
                        writer.Close();
                    }
                    if ((((filePath != null) && (this.Character.FilePath != filePath)) && File.Exists(this.Character.FilePath)) && File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                DlgMessage.ShowDialog(base.MainForm, "<LOC>Errror", "<LOC>An error occured while creating your character, please try again.");
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public PlayerCharacter Character
        {
            get
            {
                return this.mCharacter;
            }
        }
    }
}

