namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class DlgOpenSaveReplay : DlgBase
    {
        private IContainer components = null;
        private GPGCheckBox gpgCheckBoxRemember;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabelHeader;
        private ReplayDownloadActions mAction = ReplayDownloadActions.None;
        private string mDownloadFile;
        private string mSavePath;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonSave;
        private SkinButton skinButtonSaveAs;
        private SkinButton skinButtonWatch;

        public DlgOpenSaveReplay(string filename)
        {
            this.InitializeComponent();
            this.mDownloadFile = filename;
            this.gpgLabelHeader.Text = string.Format(Loc.Get("<LOC>You have downloaded a Supreme Commander replay file to {0}. What would you like to do with it?"), filename);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgCheckBoxRemember_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.SupcomPrefs.Replays.RememberDownloadAction = this.gpgCheckBoxRemember.Checked;
        }

        private void InitializeComponent()
        {
            this.gpgLabelHeader = new GPGLabel();
            this.skinButtonWatch = new SkinButton();
            this.skinButtonSaveAs = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgCheckBoxRemember = new GPGCheckBox();
            this.gpgLabel2 = new GPGLabel();
            this.skinButtonSave = new SkinButton();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabelHeader.AutoStyle = true;
            this.gpgLabelHeader.Font = new Font("Arial", 9.75f);
            this.gpgLabelHeader.ForeColor = Color.White;
            this.gpgLabelHeader.IgnoreMouseWheel = false;
            this.gpgLabelHeader.IsStyled = false;
            this.gpgLabelHeader.Location = new Point(13, 0x4f);
            this.gpgLabelHeader.Name = "gpgLabelHeader";
            this.gpgLabelHeader.Size = new Size(460, 0x5b);
            this.gpgLabelHeader.TabIndex = 7;
            this.gpgLabelHeader.Text = "You have downloaded a Supreme Commander replay file, what would you like to do with it?";
            this.gpgLabelHeader.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabelHeader.TextStyle = TextStyles.Default;
            this.skinButtonWatch.Anchor = AnchorStyles.Bottom;
            this.skinButtonWatch.AutoStyle = true;
            this.skinButtonWatch.BackColor = Color.Black;
            this.skinButtonWatch.DialogResult = DialogResult.OK;
            this.skinButtonWatch.DisabledForecolor = Color.Gray;
            this.skinButtonWatch.DrawEdges = true;
            this.skinButtonWatch.FocusColor = Color.Yellow;
            this.skinButtonWatch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonWatch.ForeColor = Color.White;
            this.skinButtonWatch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonWatch.IsStyled = true;
            this.skinButtonWatch.Location = new Point(0x16, 0xe9);
            this.skinButtonWatch.Name = "skinButtonWatch";
            this.skinButtonWatch.Size = new Size(0x6a, 0x1a);
            this.skinButtonWatch.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonWatch.TabIndex = 8;
            this.skinButtonWatch.Text = "<LOC>Watch Now";
            this.skinButtonWatch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonWatch.TextPadding = new Padding(0);
            this.skinButtonWatch.Click += new EventHandler(this.skinButtonWatch_Click);
            this.skinButtonSaveAs.Anchor = AnchorStyles.Bottom;
            this.skinButtonSaveAs.AutoStyle = true;
            this.skinButtonSaveAs.BackColor = Color.Black;
            this.skinButtonSaveAs.DialogResult = DialogResult.OK;
            this.skinButtonSaveAs.DisabledForecolor = Color.Gray;
            this.skinButtonSaveAs.DrawEdges = true;
            this.skinButtonSaveAs.FocusColor = Color.Yellow;
            this.skinButtonSaveAs.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSaveAs.ForeColor = Color.White;
            this.skinButtonSaveAs.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSaveAs.IsStyled = true;
            this.skinButtonSaveAs.Location = new Point(0xf6, 0xe9);
            this.skinButtonSaveAs.Name = "skinButtonSaveAs";
            this.skinButtonSaveAs.Size = new Size(0x6a, 0x1a);
            this.skinButtonSaveAs.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonSaveAs.TabIndex = 9;
            this.skinButtonSaveAs.Text = "<LOC>Save As...";
            this.skinButtonSaveAs.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSaveAs.TextPadding = new Padding(0);
            this.skinButtonSaveAs.Click += new EventHandler(this.skinButtonSaveAs_Click);
            this.skinButtonCancel.Anchor = AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x166, 0xe9);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x6a, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 10;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgCheckBoxRemember.AutoSize = true;
            this.gpgCheckBoxRemember.Location = new Point(13, 0xad);
            this.gpgCheckBoxRemember.Name = "gpgCheckBoxRemember";
            this.gpgCheckBoxRemember.Size = new Size(0x15f, 0x11);
            this.gpgCheckBoxRemember.TabIndex = 11;
            this.gpgCheckBoxRemember.Text = "<LOC>Remember my preference, and do not ask again.";
            this.gpgCheckBoxRemember.UseVisualStyleBackColor = true;
            this.gpgCheckBoxRemember.CheckedChanged += new EventHandler(this.gpgCheckBoxRemember_CheckedChanged);
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(13, 190);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x123, 0x22);
            this.gpgLabel2.TabIndex = 12;
            this.gpgLabel2.Text = "<LOC>This dialog can be re-enabled via Tools->Options->SupremeCommander";
            this.gpgLabel2.TextStyle = TextStyles.Info;
            this.skinButtonSave.Anchor = AnchorStyles.Bottom;
            this.skinButtonSave.AutoStyle = true;
            this.skinButtonSave.BackColor = Color.Black;
            this.skinButtonSave.DialogResult = DialogResult.OK;
            this.skinButtonSave.DisabledForecolor = Color.Gray;
            this.skinButtonSave.DrawEdges = true;
            this.skinButtonSave.FocusColor = Color.Yellow;
            this.skinButtonSave.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSave.ForeColor = Color.White;
            this.skinButtonSave.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSave.IsStyled = true;
            this.skinButtonSave.Location = new Point(0x86, 0xe9);
            this.skinButtonSave.Name = "skinButtonSave";
            this.skinButtonSave.Size = new Size(0x6a, 0x1a);
            this.skinButtonSave.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonSave.TabIndex = 10;
            this.skinButtonSave.Text = "<LOC>Save";
            this.skinButtonSave.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSave.TextPadding = new Padding(0);
            this.skinButtonSave.Click += new EventHandler(this.skinButtonSave_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1e4, 0x142);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgCheckBoxRemember);
            base.Controls.Add(this.gpgLabelHeader);
            base.Controls.Add(this.skinButtonWatch);
            base.Controls.Add(this.skinButtonSave);
            base.Controls.Add(this.skinButtonSaveAs);
            base.Controls.Add(this.skinButtonCancel);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x1e4, 0x142);
            this.MinimumSize = new Size(0x1e4, 0x142);
            base.Name = "DlgOpenSaveReplay";
            this.Text = "Download Complete";
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonSaveAs, 0);
            base.Controls.SetChildIndex(this.skinButtonSave, 0);
            base.Controls.SetChildIndex(this.skinButtonWatch, 0);
            base.Controls.SetChildIndex(this.gpgLabelHeader, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxRemember, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Program.Settings.SupcomPrefs.Replays.RememberDownloadAction)
            {
                switch (Program.Settings.SupcomPrefs.Replays.DefaultDownloadAction)
                {
                    case ReplayDownloadActions.SaveAs:
                        this.SelectSaveAs();
                        base.Close();
                        break;

                    case ReplayDownloadActions.Watch:
                        this.SelectWatch();
                        base.Close();
                        break;
                }
            }
            base.OnLoad(e);
        }

        private void SelectSaveAs()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            if (GameInformation.SelectedGame.ExeName == "ForgedAlliance.exe")
            {
                dialog.DefaultExt = ".SCFAReplay";
                dialog.Filter = Loc.Get("<LOC>Supreme Commander Forged Alliance|*.SCFAReplay");
            }
            else
            {
                dialog.DefaultExt = ".SupremeCommanderReplay";
                dialog.Filter = Loc.Get("<LOC>Supreme Commander Replay|*.SupremeCommanderReplay");
            }
            dialog.FileName = this.DownloadFile;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.mSavePath = dialog.FileName;
                this.mAction = ReplayDownloadActions.SaveAs;
                if (Program.Settings.SupcomPrefs.Replays.RememberDownloadAction)
                {
                    Program.Settings.SupcomPrefs.Replays.DefaultDownloadAction = this.Action;
                }
                Program.Settings.SupcomPrefs.Replays.ReplaysDirectory = Path.GetDirectoryName(this.SavePath);
                base.DialogResult = DialogResult.OK;
            }
        }

        private void SelectWatch()
        {
            this.mAction = ReplayDownloadActions.Watch;
            if (Program.Settings.SupcomPrefs.Replays.RememberDownloadAction)
            {
                Program.Settings.SupcomPrefs.Replays.DefaultDownloadAction = this.Action;
            }
            base.DialogResult = DialogResult.OK;
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void skinButtonSave_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Ignore;
        }

        private void skinButtonSaveAs_Click(object sender, EventArgs e)
        {
            this.SelectSaveAs();
        }

        private void skinButtonWatch_Click(object sender, EventArgs e)
        {
            this.SelectWatch();
        }

        internal ReplayDownloadActions Action
        {
            get
            {
                return this.mAction;
            }
        }

        public string DownloadFile
        {
            get
            {
                return this.mDownloadFile;
            }
            set
            {
                this.mDownloadFile = value;
            }
        }

        public string SavePath
        {
            get
            {
                return this.mSavePath;
            }
        }
    }
}

