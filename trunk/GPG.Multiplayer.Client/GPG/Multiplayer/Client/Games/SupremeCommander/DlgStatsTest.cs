namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Game.SupremeCommander;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgStatsTest : DlgBase
    {
        private GPGButton btnStats;
        private IContainer components = null;
        private SupComGameManager mLastManager = null;
        private GPGTextArea txtStats;

        public DlgStatsTest()
        {
            this.InitializeComponent();
            Loc.LocObject(this);
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            if ((base.MainForm != null) && (base.MainForm.GetSupcomGameManager() != null))
            {
                if (this.mLastManager != base.MainForm.GetSupcomGameManager())
                {
                    this.mLastManager = base.MainForm.GetSupcomGameManager();
                    this.mLastManager.OnStatsXML += new StatsXML(this.DlgStatsTest_OnStatsXML);
                }
                this.mLastManager.GetStats();
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

        private void DlgStatsTest_OnStatsXML(string xml)
        {
            base.Invoke(delegate (object innerxml) {
                this.txtStats.Text = innerxml.ToString();
            }, new object[] { xml });
        }

        private void InitializeComponent()
        {
            this.txtStats = new GPGTextArea();
            this.btnStats = new GPGButton();
            this.txtStats.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.txtStats.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtStats.Location = new Point(12, 0x53);
            this.txtStats.Name = "txtStats";
            this.txtStats.Properties.Appearance.BackColor = Color.Black;
            this.txtStats.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.txtStats.Properties.Appearance.ForeColor = Color.White;
            this.txtStats.Properties.Appearance.Options.UseBackColor = true;
            this.txtStats.Properties.Appearance.Options.UseBorderColor = true;
            this.txtStats.Properties.Appearance.Options.UseForeColor = true;
            this.txtStats.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.txtStats.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.txtStats.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.txtStats.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.txtStats.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtStats.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.txtStats.Properties.BorderStyle = BorderStyles.Simple;
            this.txtStats.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.txtStats.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.txtStats.Size = new Size(0x268, 0x134);
            this.txtStats.TabIndex = 7;
            this.btnStats.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnStats.Location = new Point(0x206, 0x18a);
            this.btnStats.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnStats.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnStats.Name = "btnStats";
            this.btnStats.Size = new Size(110, 0x17);
            this.btnStats.TabIndex = 8;
            this.btnStats.Text = "Fetch Stats";
            this.btnStats.UseVisualStyleBackColor = false;
            this.btnStats.Click += new EventHandler(this.btnStats_Click);
            base.AutoScaleDimensions = new SizeF(7f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(640, 480);
            base.Controls.Add(this.btnStats);
            base.Controls.Add(this.txtStats);
            base.Location = new Point(0, 0);
            base.Name = "DlgStatsTest";
            this.Text = "DlgStatsTest";
            base.Controls.SetChildIndex(this.txtStats, 0);
            base.Controls.SetChildIndex(this.btnStats, 0);
            this.txtStats.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

