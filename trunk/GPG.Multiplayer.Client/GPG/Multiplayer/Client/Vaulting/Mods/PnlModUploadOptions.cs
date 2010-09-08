namespace GPG.Multiplayer.Client.Vaulting.Mods
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class PnlModUploadOptions : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel10;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGLabel gpgLabelCopyright;
        private GPGLabel gpgLabelDeveloper;
        private GPGLabel gpgLabelExclusive;
        private GPGLabel gpgLabelFileCount;
        private GPGLabel gpgLabelGuid;
        private GPGLabel gpgLabelUIOnly;
        private GPGLabel gpgLabelWebsite;
        private GPGPanel gpgPanelConflicts;
        private GPGPanel gpgPanelRequirements;
        private GPG.Multiplayer.Client.Vaulting.Mods.Mod mMod;
        private PictureBox pictureBoxPreview;

        public PnlModUploadOptions(GPG.Multiplayer.Client.Vaulting.Mods.Mod mod)
        {
            this.InitializeComponent();
            this.mMod = mod;
            this.gpgLabelCopyright.Text = mod.Copyright;
            this.gpgLabelDeveloper.Text = mod.DeveloperName;
            this.gpgLabelExclusive.Text = this.ParseBool(mod.Exclusive);
            this.gpgLabelFileCount.Text = mod.NumberOfFiles.ToString();
            this.gpgLabelGuid.Text = mod.Guid;
            this.gpgLabelUIOnly.Text = this.ParseBool(mod.UIOnly.Value);
            this.gpgLabelWebsite.Text = mod.Website;
            this.pictureBoxPreview.Image = mod.PreviewImage50;
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
            if (this.Mod != null)
            {
                Program.MainForm.ShowWebPage(this.Mod.Website);
            }
        }

        private void InitializeComponent()
        {
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelDeveloper = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabelGuid = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabelCopyright = new GPGLabel();
            this.gpgLabelWebsite = new GPGLabel();
            this.gpgLabel10 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabelUIOnly = new GPGLabel();
            this.gpgLabelExclusive = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelFileCount = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgPanelRequirements = new GPGPanel();
            this.pictureBoxPreview = new PictureBox();
            this.gpgPanelConflicts = new GPGPanel();
            this.gpgLabel9 = new GPGLabel();
            ((ISupportInitialize) this.pictureBoxPreview).BeginInit();
            base.SuspendLayout();
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x49, 0x2b);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x205, 0x11);
            this.gpgLabel1.TabIndex = 0;
            this.gpgLabel1.Text = "<LOC>Developer";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgLabelDeveloper.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDeveloper.AutoSize = true;
            this.gpgLabelDeveloper.AutoStyle = true;
            this.gpgLabelDeveloper.Font = new Font("Arial", 9.75f);
            this.gpgLabelDeveloper.ForeColor = Color.White;
            this.gpgLabelDeveloper.IgnoreMouseWheel = false;
            this.gpgLabelDeveloper.IsStyled = false;
            this.gpgLabelDeveloper.Location = new Point(0x48, 0x3f);
            this.gpgLabelDeveloper.Name = "gpgLabelDeveloper";
            this.gpgLabelDeveloper.Size = new Size(0x43, 0x10);
            this.gpgLabelDeveloper.TabIndex = 1;
            this.gpgLabelDeveloper.Text = "gpgLabel2";
            this.gpgLabelDeveloper.TextStyle = TextStyles.Default;
            this.gpgLabel4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel4.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(4, 0x5c);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x24a, 0x11);
            this.gpgLabel4.TabIndex = 12;
            this.gpgLabel4.Text = "<LOC>Copyright";
            this.gpgLabel4.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel4.TextStyle = TextStyles.Custom;
            this.gpgLabel5.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel5.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(4, 0xbb);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(590, 0x11);
            this.gpgLabel5.TabIndex = 13;
            this.gpgLabel5.Text = "<LOC>UI Only";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.gpgLabel6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel6.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(4, 0);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(590, 0x11);
            this.gpgLabel6.TabIndex = 0x10;
            this.gpgLabel6.Text = "<LOC>Icon";
            this.gpgLabel6.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel6.TextStyle = TextStyles.Custom;
            this.gpgLabelGuid.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelGuid.AutoSize = true;
            this.gpgLabelGuid.AutoStyle = true;
            this.gpgLabelGuid.Font = new Font("Arial", 9.75f);
            this.gpgLabelGuid.ForeColor = Color.White;
            this.gpgLabelGuid.IgnoreMouseWheel = false;
            this.gpgLabelGuid.IsStyled = false;
            this.gpgLabelGuid.Location = new Point(0x48, 0x11);
            this.gpgLabelGuid.Name = "gpgLabelGuid";
            this.gpgLabelGuid.Size = new Size(0x43, 0x10);
            this.gpgLabelGuid.TabIndex = 0x11;
            this.gpgLabelGuid.Text = "gpgLabel2";
            this.gpgLabelGuid.TextStyle = TextStyles.Default;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel8.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x49, 0);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x7b, 0x11);
            this.gpgLabel8.TabIndex = 20;
            this.gpgLabel8.Text = "<LOC>Unique ID";
            this.gpgLabel8.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel8.TextStyle = TextStyles.Custom;
            this.gpgLabelCopyright.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelCopyright.AutoStyle = true;
            this.gpgLabelCopyright.Font = new Font("Arial", 9.75f);
            this.gpgLabelCopyright.ForeColor = Color.White;
            this.gpgLabelCopyright.IgnoreMouseWheel = false;
            this.gpgLabelCopyright.IsStyled = false;
            this.gpgLabelCopyright.Location = new Point(3, 0x6d);
            this.gpgLabelCopyright.Name = "gpgLabelCopyright";
            this.gpgLabelCopyright.Size = new Size(0x248, 15);
            this.gpgLabelCopyright.TabIndex = 0x15;
            this.gpgLabelCopyright.Text = "gpgLabel6";
            this.gpgLabelCopyright.TextStyle = TextStyles.Default;
            this.gpgLabelWebsite.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelWebsite.AutoSize = true;
            this.gpgLabelWebsite.AutoStyle = true;
            this.gpgLabelWebsite.Font = new Font("Arial", 9.75f);
            this.gpgLabelWebsite.ForeColor = Color.White;
            this.gpgLabelWebsite.IgnoreMouseWheel = false;
            this.gpgLabelWebsite.IsStyled = false;
            this.gpgLabelWebsite.Location = new Point(3, 0x9c);
            this.gpgLabelWebsite.Name = "gpgLabelWebsite";
            this.gpgLabelWebsite.Size = new Size(0x43, 0x10);
            this.gpgLabelWebsite.TabIndex = 0x17;
            this.gpgLabelWebsite.Text = "gpgLabel6";
            this.gpgLabelWebsite.TextStyle = TextStyles.Link;
            this.gpgLabelWebsite.Click += new EventHandler(this.gpgLabelWebsite_Click);
            this.gpgLabel10.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel10.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel10.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(4, 0x8b);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0x24a, 0x11);
            this.gpgLabel10.TabIndex = 0x16;
            this.gpgLabel10.Text = "<LOC>Website";
            this.gpgLabel10.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel10.TextStyle = TextStyles.Custom;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x85, 0xbb);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x7b, 0x11);
            this.gpgLabel2.TabIndex = 0x15;
            this.gpgLabel2.Text = "<LOC>Exclusive";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.gpgLabelUIOnly.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelUIOnly.AutoSize = true;
            this.gpgLabelUIOnly.AutoStyle = true;
            this.gpgLabelUIOnly.Font = new Font("Arial", 9.75f);
            this.gpgLabelUIOnly.ForeColor = Color.White;
            this.gpgLabelUIOnly.IgnoreMouseWheel = false;
            this.gpgLabelUIOnly.IsStyled = false;
            this.gpgLabelUIOnly.Location = new Point(3, 0xcc);
            this.gpgLabelUIOnly.Name = "gpgLabelUIOnly";
            this.gpgLabelUIOnly.Size = new Size(0x43, 0x10);
            this.gpgLabelUIOnly.TabIndex = 0x18;
            this.gpgLabelUIOnly.Text = "gpgLabel2";
            this.gpgLabelUIOnly.TextStyle = TextStyles.Default;
            this.gpgLabelExclusive.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelExclusive.AutoSize = true;
            this.gpgLabelExclusive.AutoStyle = true;
            this.gpgLabelExclusive.Font = new Font("Arial", 9.75f);
            this.gpgLabelExclusive.ForeColor = Color.White;
            this.gpgLabelExclusive.IgnoreMouseWheel = false;
            this.gpgLabelExclusive.IsStyled = false;
            this.gpgLabelExclusive.Location = new Point(0x85, 0xcc);
            this.gpgLabelExclusive.Name = "gpgLabelExclusive";
            this.gpgLabelExclusive.Size = new Size(0x43, 0x10);
            this.gpgLabelExclusive.TabIndex = 0x19;
            this.gpgLabelExclusive.Text = "gpgLabel2";
            this.gpgLabelExclusive.TextStyle = TextStyles.Default;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x113, 0xbb);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x7b, 0x11);
            this.gpgLabel3.TabIndex = 0x1a;
            this.gpgLabel3.Text = "<LOC>Number of Files";
            this.gpgLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.gpgLabelFileCount.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelFileCount.AutoSize = true;
            this.gpgLabelFileCount.AutoStyle = true;
            this.gpgLabelFileCount.Font = new Font("Arial", 9.75f);
            this.gpgLabelFileCount.ForeColor = Color.White;
            this.gpgLabelFileCount.IgnoreMouseWheel = false;
            this.gpgLabelFileCount.IsStyled = false;
            this.gpgLabelFileCount.Location = new Point(0x112, 0xcc);
            this.gpgLabelFileCount.Name = "gpgLabelFileCount";
            this.gpgLabelFileCount.Size = new Size(0x43, 0x10);
            this.gpgLabelFileCount.TabIndex = 0x1b;
            this.gpgLabelFileCount.Text = "gpgLabel2";
            this.gpgLabelFileCount.TextStyle = TextStyles.Default;
            this.gpgLabel7.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel7.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(3, 0xec);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x24a, 0x11);
            this.gpgLabel7.TabIndex = 0x1c;
            this.gpgLabel7.Text = "<LOC>Required Mods";
            this.gpgLabel7.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel7.TextStyle = TextStyles.Custom;
            this.gpgPanelRequirements.AutoScroll = true;
            this.gpgPanelRequirements.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelRequirements.BorderThickness = 2;
            this.gpgPanelRequirements.DrawBorder = false;
            this.gpgPanelRequirements.Location = new Point(5, 0x100);
            this.gpgPanelRequirements.Name = "gpgPanelRequirements";
            this.gpgPanelRequirements.Size = new Size(0x1f9, 0x30);
            this.gpgPanelRequirements.TabIndex = 0x1d;
            this.pictureBoxPreview.Location = new Point(11, 0x1d);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new Size(50, 50);
            this.pictureBoxPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBoxPreview.TabIndex = 11;
            this.pictureBoxPreview.TabStop = false;
            this.gpgPanelConflicts.AutoScroll = true;
            this.gpgPanelConflicts.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelConflicts.BorderThickness = 2;
            this.gpgPanelConflicts.DrawBorder = false;
            this.gpgPanelConflicts.Location = new Point(5, 0x152);
            this.gpgPanelConflicts.Name = "gpgPanelConflicts";
            this.gpgPanelConflicts.Size = new Size(0x1f9, 0x30);
            this.gpgPanelConflicts.TabIndex = 0x1f;
            this.gpgLabel9.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel9.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(3, 0x13e);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x24a, 0x11);
            this.gpgLabel9.TabIndex = 30;
            this.gpgLabel9.Text = "<LOC>Conflicting Mods";
            this.gpgLabel9.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel9.TextStyle = TextStyles.Custom;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgPanelConflicts);
            base.Controls.Add(this.gpgLabel9);
            base.Controls.Add(this.gpgPanelRequirements);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.gpgLabelFileCount);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabelExclusive);
            base.Controls.Add(this.gpgLabelUIOnly);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabelWebsite);
            base.Controls.Add(this.gpgLabel10);
            base.Controls.Add(this.gpgLabelCopyright);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgLabelGuid);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.pictureBoxPreview);
            base.Controls.Add(this.gpgLabelDeveloper);
            base.Controls.Add(this.gpgLabel1);
            base.Name = "PnlModUploadOptions";
            base.Size = new Size(590, 0x189);
            ((ISupportInitialize) this.pictureBoxPreview).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ModAttributeChanged(object sender, EventArgs e)
        {
            if (this.Mod != null)
            {
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                try
                {
                    while (this.Mod == null)
                    {
                        Thread.Sleep(100);
                    }
                    GPG.Multiplayer.Client.Vaulting.Mods.Mod mod = this.Mod;
                    int top = 0;
                    foreach (string requirement in mod.Requirements)
                    {
                        if ((requirement != null) && (requirement.Length >= 1))
                        {
                            GPG.Multiplayer.Client.Vaulting.Mods.Mod reqMod = new QuazalQuery("GetModByGuid", new object[] { requirement }).GetObject<GPG.Multiplayer.Client.Vaulting.Mods.Mod>();
                            base.Invoke((VGen0)delegate {
                                GPGLabel label = new GPGLabel {
                                    AutoSize = true
                                };
                                if (reqMod != null)
                                {
                                    label.Text = string.Format(Loc.Get("<LOC>Name: {0}, UID: {1}"), reqMod.Name, reqMod.Guid);
                                    label.TextStyle = TextStyles.Default;
                                }
                                else
                                {
                                    label.Text = string.Format(Loc.Get("<LOC>NOT FOUND, UID: {0}"), requirement);
                                    label.TextStyle = TextStyles.ColoredBold;
                                    label.ForeColor = Color.Red;
                                }
                                label.Left = 0;
                                label.Top = top;
                                this.gpgPanelRequirements.Controls.Add(label);
                                top += label.Height;
                            });
                        }
                    }
                    top = 0;
                    foreach (string conflict in mod.Conflicts)
                    {
                        if ((conflict != null) && (conflict.Length >= 1))
                        {
                            GPG.Multiplayer.Client.Vaulting.Mods.Mod conflictMod = new QuazalQuery("GetModByGuid", new object[] { conflict }).GetObject<GPG.Multiplayer.Client.Vaulting.Mods.Mod>();
                            base.Invoke((VGen0)delegate {
                                GPGLabel label = new GPGLabel {
                                    AutoSize = true
                                };
                                if (conflictMod != null)
                                {
                                    label.Text = string.Format(Loc.Get("<LOC>Name: {0}, UID: {1}"), conflictMod.Name, conflictMod.Guid);
                                    label.TextStyle = TextStyles.Default;
                                }
                                else
                                {
                                    label.Text = string.Format(Loc.Get("<LOC>NOT FOUND, UID: {0}"), conflict);
                                    label.TextStyle = TextStyles.ColoredBold;
                                    label.ForeColor = Color.Red;
                                }
                                label.Left = 0;
                                label.Top = top;
                                this.gpgPanelConflicts.Controls.Add(label);
                                top += label.Height;
                            });
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        private string ParseBool(bool value)
        {
            if (value)
            {
                return Loc.Get("<LOC>Yes");
            }
            return Loc.Get("<LOC>No");
        }

        public GPG.Multiplayer.Client.Vaulting.Mods.Mod Mod
        {
            get
            {
                return this.mMod;
            }
        }
    }
}

