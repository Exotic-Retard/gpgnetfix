namespace GPG.Multiplayer.Client.Vaulting.Videos
{
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PnlVideoDownloadOptions : PnlBase
    {
        private ComboBox comboBoxFormat;
        private ComboBox comboBoxQuality;
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPG.Multiplayer.Client.Vaulting.Videos.Video mVideo;

        public PnlVideoDownloadOptions(GPG.Multiplayer.Client.Vaulting.Videos.Video video)
        {
            this.InitializeComponent();
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Any"), "<LOC>Any"));
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Low"), "<LOC>Low"));
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>Medium"), "<LOC>Medium"));
            this.comboBoxQuality.Items.Add(new MultiVal<string, string>(Loc.Get("<LOC>High"), "<LOC>High"));
            this.comboBoxQuality.SelectedIndex = 0;
            this.comboBoxFormat.Items.Add(Loc.Get("<LOC>Any"));
            foreach (string str in video.ContentType.FileExtensions)
            {
                this.comboBoxFormat.Items.Add(str);
            }
            this.comboBoxFormat.SelectedIndex = 0;
            this.mVideo = video;
        }

        private void CriteriaChanged(object sender, EventArgs e)
        {
            if (this.Video != null)
            {
                if (this.comboBoxQuality.SelectedIndex > 0)
                {
                    this.Video.Quality = (this.comboBoxQuality.SelectedItem as MultiVal<string, string>).Value2;
                }
                else
                {
                    this.Video.Quality = null;
                }
                if (this.comboBoxFormat.SelectedIndex > 0)
                {
                    this.Video.VideoFormat = (string) this.comboBoxFormat.SelectedItem;
                }
                else
                {
                    this.Video.VideoFormat = null;
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
            this.comboBoxQuality = new ComboBox();
            this.comboBoxFormat = new ComboBox();
            this.gpgLabel2 = new GPGLabel();
            base.SuspendLayout();
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x94, 0);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x80, 0x10);
            this.gpgLabel1.TabIndex = 11;
            this.gpgLabel1.Text = "<LOC>Video Quality";
            this.gpgLabel1.TextStyle = TextStyles.Bold;
            this.comboBoxQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxQuality.FormattingEnabled = true;
            this.comboBoxQuality.Location = new Point(0x97, 0x11);
            this.comboBoxQuality.Name = "comboBoxQuality";
            this.comboBoxQuality.Size = new Size(0xc0, 0x15);
            this.comboBoxQuality.TabIndex = 0x18;
            this.comboBoxQuality.SelectedIndexChanged += new EventHandler(this.CriteriaChanged);
            this.comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Location = new Point(0, 0x11);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new Size(0x7d, 0x15);
            this.comboBoxFormat.TabIndex = 0x1a;
            this.comboBoxFormat.SelectedIndexChanged += new EventHandler(this.CriteriaChanged);
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(-3, 0);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x80, 0x10);
            this.gpgLabel2.TabIndex = 0x19;
            this.gpgLabel2.Text = "<LOC>Video Format";
            this.gpgLabel2.TextStyle = TextStyles.Bold;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.comboBoxFormat);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.comboBoxQuality);
            base.Controls.Add(this.gpgLabel1);
            base.Name = "PnlVideoDownloadOptions";
            base.Size = new Size(0x1d2, 0x2b);
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

        public GPG.Multiplayer.Client.Vaulting.Videos.Video Video
        {
            get
            {
                return this.mVideo;
            }
        }
    }
}

