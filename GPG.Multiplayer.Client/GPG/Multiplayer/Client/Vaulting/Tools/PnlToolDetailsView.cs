namespace GPG.Multiplayer.Client.Vaulting.Tools
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PnlToolDetailsView : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabelDeveloper;
        private GPGLabel gpgLabelExeName;
        private GPGLabel gpgLabelNumberFiles;
        private GPGLabel gpgLabelQuality;
        private GPGLabel gpgLabelWebsite;
        private GPG.Multiplayer.Client.Vaulting.Tools.Tool mTool;

        public PnlToolDetailsView(GPG.Multiplayer.Client.Vaulting.Tools.Tool tool)
        {
            this.InitializeComponent();
            this.gpgLabelWebsite.Font = new Font(Program.Settings.StylePreferences.MasterFont, FontStyle.Underline);
            this.gpgLabelWebsite.ForeColor = Program.Settings.Chat.Links.WebColor;
            this.gpgLabelWebsite.Cursor = Cursors.Hand;
            this.gpgLabelDeveloper.Text = tool.DeveloperName;
            this.gpgLabelExeName.Text = tool.ExeName;
            this.gpgLabelNumberFiles.Text = tool.NumberOfFiles.ToString();
            this.gpgLabelQuality.Text = tool.QualityDisplay;
            this.gpgLabelWebsite.Text = tool.Website;
            this.mTool = tool;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgLabelWebsite_Click(object sender, EventArgs e)
        {
            Program.MainForm.ShowWebPage(this.Tool.Website);
        }

        private void InitializeComponent()
        {
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelNumberFiles = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabelWebsite = new GPGLabel();
            this.gpgLabelDeveloper = new GPGLabel();
            this.gpgLabelQuality = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelExeName = new GPGLabel();
            base.SuspendLayout();
            this.gpgLabel5.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel5.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0xf5, 0x29);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(200, 0x11);
            this.gpgLabel5.TabIndex = 0x22;
            this.gpgLabel5.Text = "<LOC>Release Quality";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0xf5, 0x54);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(250, 0x11);
            this.gpgLabel2.TabIndex = 0x1f;
            this.gpgLabel2.Text = "<LOC>Product Website";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0, 0x54);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x2db, 0x11);
            this.gpgLabel1.TabIndex = 30;
            this.gpgLabel1.Text = "<LOC>Developer Name";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgLabelNumberFiles.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNumberFiles.AutoSize = true;
            this.gpgLabelNumberFiles.AutoStyle = true;
            this.gpgLabelNumberFiles.Font = new Font("Arial", 9.75f);
            this.gpgLabelNumberFiles.ForeColor = Color.White;
            this.gpgLabelNumberFiles.IgnoreMouseWheel = false;
            this.gpgLabelNumberFiles.IsStyled = false;
            this.gpgLabelNumberFiles.Location = new Point(0, 0x3d);
            this.gpgLabelNumberFiles.Name = "gpgLabelNumberFiles";
            this.gpgLabelNumberFiles.Size = new Size(0x43, 0x10);
            this.gpgLabelNumberFiles.TabIndex = 0x1d;
            this.gpgLabelNumberFiles.Text = "gpgLabel2";
            this.gpgLabelNumberFiles.TextStyle = TextStyles.Default;
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0, 0x29);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x2db, 0x11);
            this.gpgLabel6.TabIndex = 0x1c;
            this.gpgLabel6.Text = "<LOC>Number of files";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabelWebsite.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelWebsite.AutoSize = true;
            this.gpgLabelWebsite.AutoStyle = true;
            this.gpgLabelWebsite.Font = new Font("Arial", 9.75f);
            this.gpgLabelWebsite.ForeColor = Color.White;
            this.gpgLabelWebsite.IgnoreMouseWheel = false;
            this.gpgLabelWebsite.IsStyled = false;
            this.gpgLabelWebsite.Location = new Point(0xf4, 0x68);
            this.gpgLabelWebsite.Name = "gpgLabelWebsite";
            this.gpgLabelWebsite.Size = new Size(0x43, 0x10);
            this.gpgLabelWebsite.TabIndex = 0x23;
            this.gpgLabelWebsite.Text = "gpgLabel2";
            this.gpgLabelWebsite.TextStyle = TextStyles.Custom;
            this.gpgLabelWebsite.Click += new EventHandler(this.gpgLabelWebsite_Click);
            this.gpgLabelDeveloper.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDeveloper.AutoSize = true;
            this.gpgLabelDeveloper.AutoStyle = true;
            this.gpgLabelDeveloper.Font = new Font("Arial", 9.75f);
            this.gpgLabelDeveloper.ForeColor = Color.White;
            this.gpgLabelDeveloper.IgnoreMouseWheel = false;
            this.gpgLabelDeveloper.IsStyled = false;
            this.gpgLabelDeveloper.Location = new Point(0, 0x68);
            this.gpgLabelDeveloper.Name = "gpgLabelDeveloper";
            this.gpgLabelDeveloper.Size = new Size(0x43, 0x10);
            this.gpgLabelDeveloper.TabIndex = 0x24;
            this.gpgLabelDeveloper.Text = "gpgLabel2";
            this.gpgLabelDeveloper.TextStyle = TextStyles.Default;
            this.gpgLabelQuality.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelQuality.AutoSize = true;
            this.gpgLabelQuality.AutoStyle = true;
            this.gpgLabelQuality.Font = new Font("Arial", 9.75f);
            this.gpgLabelQuality.ForeColor = Color.White;
            this.gpgLabelQuality.IgnoreMouseWheel = false;
            this.gpgLabelQuality.IsStyled = false;
            this.gpgLabelQuality.Location = new Point(0xf4, 0x3d);
            this.gpgLabelQuality.Name = "gpgLabelQuality";
            this.gpgLabelQuality.Size = new Size(0x43, 0x10);
            this.gpgLabelQuality.TabIndex = 0x25;
            this.gpgLabelQuality.Text = "gpgLabel2";
            this.gpgLabelQuality.TextStyle = TextStyles.Default;
            this.gpgLabel3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0, 0);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x2db, 0x11);
            this.gpgLabel3.TabIndex = 0x26;
            this.gpgLabel3.Text = "<LOC>Executable Name";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.gpgLabelExeName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelExeName.AutoSize = true;
            this.gpgLabelExeName.AutoStyle = true;
            this.gpgLabelExeName.Font = new Font("Arial", 9.75f);
            this.gpgLabelExeName.ForeColor = Color.White;
            this.gpgLabelExeName.IgnoreMouseWheel = false;
            this.gpgLabelExeName.IsStyled = false;
            this.gpgLabelExeName.Location = new Point(0, 20);
            this.gpgLabelExeName.Name = "gpgLabelExeName";
            this.gpgLabelExeName.Size = new Size(0x43, 0x10);
            this.gpgLabelExeName.TabIndex = 0x27;
            this.gpgLabelExeName.Text = "gpgLabel2";
            this.gpgLabelExeName.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLabelExeName);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabelQuality);
            base.Controls.Add(this.gpgLabelDeveloper);
            base.Controls.Add(this.gpgLabelWebsite);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgLabelNumberFiles);
            base.Controls.Add(this.gpgLabel6);
            base.Name = "PnlToolDetailsView";
            base.Size = new Size(0x2db, 0x81);
            base.ResumeLayout(false);
            base.PerformLayout();
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

