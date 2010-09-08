namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgLocateExe : DlgBase
    {
        private IContainer components = null;
        private string FileName = null;
        private GPGLabel gpgLabelQuestion;
        private bool mScanning = false;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonFullScan;
        private SkinButton skinButtonManual;

        public DlgLocateExe(FrmMain mainForm, string fileName)
        {
            this.InitializeComponent();
            base.MainForm = mainForm;
            this.FileName = fileName;
            this.gpgLabelQuestion.Text = string.Format(Loc.Get("<LOC id=_2313da81afb678aa3bee11b5f7cc9a34>The executable file -- {0} -- was not found. Please locate it via one of the options below in order to continue."), this.FileName);
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
            this.gpgLabelQuestion = new GPGLabel();
            this.skinButtonManual = new SkinButton();
            this.skinButtonFullScan = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgLabelQuestion.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelQuestion.AutoStyle = true;
            this.gpgLabelQuestion.Font = new Font("Arial", 9.75f);
            this.gpgLabelQuestion.ForeColor = Color.White;
            this.gpgLabelQuestion.IgnoreMouseWheel = false;
            this.gpgLabelQuestion.IsStyled = false;
            this.gpgLabelQuestion.Location = new Point(12, 80);
            this.gpgLabelQuestion.Name = "gpgLabelQuestion";
            this.gpgLabelQuestion.Size = new Size(0x1d9, 0x2d);
            this.gpgLabelQuestion.TabIndex = 8;
            this.gpgLabelQuestion.Text = "The executable file for {0} is missing, how would you like to locate it?";
            this.gpgLabelQuestion.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelQuestion.TextStyle = TextStyles.Default;
            this.skinButtonManual.Anchor = AnchorStyles.Bottom;
            this.skinButtonManual.AutoStyle = true;
            this.skinButtonManual.BackColor = Color.Black;
            this.skinButtonManual.DialogResult = DialogResult.OK;
            this.skinButtonManual.DisabledForecolor = Color.Gray;
            this.skinButtonManual.DrawEdges = true;
            this.skinButtonManual.FocusColor = Color.Yellow;
            this.skinButtonManual.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonManual.ForeColor = Color.White;
            this.skinButtonManual.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonManual.IsStyled = true;
            this.skinButtonManual.Location = new Point(0x30, 140);
            this.skinButtonManual.Name = "skinButtonManual";
            this.skinButtonManual.Size = new Size(0x71, 0x21);
            this.skinButtonManual.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonManual.TabIndex = 9;
            this.skinButtonManual.Text = "<LOC>Browse...";
            this.skinButtonManual.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonManual.TextPadding = new Padding(0);
            this.skinButtonManual.Click += new EventHandler(this.skinButtonManual_Click);
            this.skinButtonFullScan.Anchor = AnchorStyles.Bottom;
            this.skinButtonFullScan.AutoStyle = true;
            this.skinButtonFullScan.BackColor = Color.Black;
            this.skinButtonFullScan.DialogResult = DialogResult.OK;
            this.skinButtonFullScan.DisabledForecolor = Color.Gray;
            this.skinButtonFullScan.DrawEdges = true;
            this.skinButtonFullScan.FocusColor = Color.Yellow;
            this.skinButtonFullScan.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonFullScan.ForeColor = Color.White;
            this.skinButtonFullScan.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonFullScan.IsStyled = true;
            this.skinButtonFullScan.Location = new Point(0xb3, 140);
            this.skinButtonFullScan.Name = "skinButtonFullScan";
            this.skinButtonFullScan.Size = new Size(0x91, 0x21);
            this.skinButtonFullScan.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonFullScan.TabIndex = 10;
            this.skinButtonFullScan.Text = "<LOC>Scan Hard Drive(s)";
            this.skinButtonFullScan.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonFullScan.TextPadding = new Padding(0);
            this.skinButtonFullScan.Click += new EventHandler(this.skinButtonFullScan_Click);
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
            this.skinButtonCancel.Location = new Point(0x157, 140);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x71, 0x21);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 11;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            base.AcceptButton = this.skinButtonManual;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1f0, 0xff);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonFullScan);
            base.Controls.Add(this.skinButtonManual);
            base.Controls.Add(this.gpgLabelQuestion);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x1f0, 0xff);
            this.MinimumSize = new Size(0x1f0, 0xff);
            base.Name = "DlgLocateExe";
            this.Text = "<LOC>Locate Supreme Commander";
            base.Controls.SetChildIndex(this.gpgLabelQuestion, 0);
            base.Controls.SetChildIndex(this.skinButtonManual, 0);
            base.Controls.SetChildIndex(this.skinButtonFullScan, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ScanDirectories(string[] dirs)
        {
            VGen1 method = null;
            foreach (string str in dirs)
            {
                if (!this.Scanning)
                {
                    break;
                }
                if (method == null)
                {
                    method = delegate (object d) {
                        this.gpgLabelQuestion.Text = (string) d;
                        this.gpgLabelQuestion.Refresh();
                    };
                }
                base.Invoke(method, new object[] { str });
                if (File.Exists(string.Format(@"{0}\{1}", str, this.FileName)))
                {
                    GameInformation.SelectedGame.GameLocation = string.Format(@"{0}\{1}", str, this.FileName);
                    this.Scanning = false;
                }
                else
                {
                    string[] directories = Directory.GetDirectories(str);
                    if (directories.Length > 0)
                    {
                        this.ScanDirectories(directories);
                    }
                }
            }
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            if (this.Scanning)
            {
                this.Scanning = false;
                this.Text = Loc.Get("<LOC>Search Cancelled");
                this.gpgLabelQuestion.Text = string.Format(Loc.Get("<LOC>The executable file {0} was not automatically found, how would you like to locate it?"), this.FileName);
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        private void skinButtonFullScan_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            try
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        VGen1 method = null;
                        try
                        {
                            this.Scanning = true;
                            DriveInfo[] drives = DriveInfo.GetDrives();
                            List<string> list = new List<string>(drives.Length);
                            for (int j = 0; j < drives.Length; j++)
                            {
                                if (drives[j].IsReady)
                                {
                                    list.Add(drives[j].RootDirectory.Name);
                                }
                            }
                            this.ScanDirectories(list.ToArray());
                            if ((GameInformation.SelectedGame.GameLocation == null) || (GameInformation.SelectedGame.GameLocation.Length < 1))
                            {
                                if (!base.Disposing && !base.IsDisposed)
                                {
                                    if (method == null)
                                    {
                                        method = delegate (object objfilename) {
                                            this.gpgLabelQuestion.Text = string.Format(Loc.Get("<LOC>Unable to locate {0}"), objfilename.ToString());
                                        };
                                    }
                                    base.Invoke(method, new object[] { this.FileName });
                                }
                                else
                                {
                                    base.DialogResult = DialogResult.OK;
                                    base.Close();
                                }
                            }
                            else
                            {
                                base.DialogResult = DialogResult.OK;
                                base.Close();
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void skinButtonManual_Click(object sender, EventArgs e)
        {
            if (this.Scanning)
            {
                this.Scanning = false;
                this.Text = Loc.Get("<LOC>Search Cancelled");
                this.gpgLabelQuestion.Text = string.Format(Loc.Get("<LOC>The executable file {0} was not automatically found, how would you like to locate it?"), this.FileName);
                Thread.Sleep(200);
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = Loc.Get("<LOC>Locate Executable");
            dialog.Filter = string.Format("{0}|{0}", this.FileName);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                GameInformation.SelectedGame.GameLocation = dialog.FileName;
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private bool Scanning
        {
            get
            {
                return this.mScanning;
            }
            set
            {
                VGen0 method = null;
                VGen0 gen2 = null;
                this.mScanning = value;
                if (this.Scanning)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.Text = Loc.Get("<LOC>Searching...");
                            this.Refresh();
                        };
                    }
                    base.BeginInvoke(method);
                }
                else
                {
                    if (gen2 == null)
                    {
                        gen2 = delegate {
                            this.Text = Loc.Get("<LOC>Locate Supreme Commander");
                            this.Refresh();
                        };
                    }
                    base.BeginInvoke(gen2);
                }
            }
        }
    }
}

