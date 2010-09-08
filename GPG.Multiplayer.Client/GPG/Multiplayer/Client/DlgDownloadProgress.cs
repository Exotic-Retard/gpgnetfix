namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Network;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DlgDownloadProgress : DlgBase
    {
        private IContainer components = null;
        private GPGCheckBox gpgCheckBoxAutoClose;
        private GPGLabel gpgLabelETA;
        private GPGLabel gpgLabelProgress;
        private GPGLabel gpgLabelRecieved;
        private GPGLabel gpgLabelSpeed;
        private long LastBytes = 0L;
        private int LastRefresh = Environment.TickCount;
        private int LastTick = Environment.TickCount;
        private WebClient mClient;
        private string mTarget;
        private ProgressMeter progressMeter;
        private const int RefreshInterval = 0x3e8;
        private SkinButton skinButtonCancel;
        private int? StartTime = null;

        public event EventHandler DownloadCancelled;

        public DlgDownloadProgress(string target, WebClient client)
        {
            this.InitializeComponent();
            this.mTarget = target;
            this.mClient = client;
            this.ChangeProgress(0);
            this.Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.Client_DownloadProgressChanged);
            this.Client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Client_DownloadFileCompleted);
            this.progressMeter.Monitor = new WebDownloadMonitor(this.Client);
        }

        private void ChangeProgress(int progress)
        {
            this.Text = string.Format(Loc.Get("<LOC>Downloading: {0} - {1}%"), this.Target, progress);
            this.gpgLabelProgress.Text = string.Format(Loc.Get("<LOC>Downloading: {0} - {1}%"), this.Target, progress);
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            VGen0 method = null;
            try
            {
                if ((!base.IsDisposed && !base.Disposing) && base.Visible)
                {
                    if (method == null)
                    {
                        method = delegate {
                            try
                            {
                                if (Program.Settings.Miscellaneous.AutoCloseDownloadDialog)
                                {
                                    base.Close();
                                }
                                else
                                {
                                    this.gpgLabelProgress.Text = Loc.Get("<LOC>Download Complete.");
                                    this.Text = Loc.Get("<LOC>Download Complete.");
                                    this.skinButtonCancel.Text = Loc.Get("<LOC>Close");
                                    this.ChangeProgress(100);
                                }
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    Program.MainForm.BeginInvoke(method);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            VGen0 method = null;
            try
            {
                if (((Environment.TickCount - this.LastRefresh) >= 0x3e8) && (!base.Disposing && !base.IsDisposed))
                {
                    if (method == null)
                    {
                        method = delegate {
                            try
                            {
                                if (!this.StartTime.HasValue)
                                {
                                    this.StartTime = new int?(Environment.TickCount);
                                }
                                float bytesReceived = e.BytesReceived;
                                float totalBytesToReceive = e.TotalBytesToReceive;
                                string str = Loc.Get("<LOC>Bytes");
                                if (totalBytesToReceive > 1000f)
                                {
                                    bytesReceived /= 1000f;
                                    totalBytesToReceive /= 1000f;
                                    str = Loc.Get("<LOC>KBs");
                                }
                                if (totalBytesToReceive > 1000f)
                                {
                                    bytesReceived /= 1000f;
                                    totalBytesToReceive /= 1000f;
                                    str = Loc.Get("<LOC>MBs");
                                }
                                if (totalBytesToReceive > 1000f)
                                {
                                    bytesReceived /= 1000f;
                                    totalBytesToReceive /= 1000f;
                                    str = Loc.Get("<LOC>GBs");
                                }
                                this.gpgLabelRecieved.Text = string.Format(Loc.Get("<LOC>Received: {0} {2} / {1} {2}."), Math.Round((double) bytesReceived, 2), Math.Round((double) totalBytesToReceive, 2), str);
                                float num3 = ((float) e.BytesReceived) / (((Environment.TickCount - this.StartTime.Value) / 0x3e8) + 1E-06f);
                                float num4 = ((float) (e.TotalBytesToReceive - e.BytesReceived)) / num3;
                                str = Loc.Get("<LOC>bps");
                                if (num3 > 1000f)
                                {
                                    num3 /= 1000f;
                                    str = Loc.Get("<LOC>kbps");
                                }
                                if (num3 > 1000f)
                                {
                                    num3 /= 1000f;
                                    str = Loc.Get("<LOC>mbps");
                                }
                                this.gpgLabelSpeed.Text = string.Format(Loc.Get("<LOC>Transfer Rate: {0} {1}"), Math.Round((double) num3, 2), str);
                                TimeSpan span = TimeSpan.FromSeconds((double) num4);
                                if (span.Hours > 0)
                                {
                                    this.gpgLabelETA.Text = string.Format(Loc.Get("<LOC>Estimated Time Remaining: {0}hr {1}min {2}sec"), span.Hours, span.Minutes, span.Seconds);
                                }
                                else if (span.Minutes > 0)
                                {
                                    this.gpgLabelETA.Text = string.Format(Loc.Get("<LOC>Estimated Time Remaining: {0}min {1}sec"), span.Minutes, span.Seconds);
                                }
                                else
                                {
                                    this.gpgLabelETA.Text = string.Format(Loc.Get("<LOC>Estimated Time Remaining: {0}sec"), span.Seconds);
                                }
                                this.ChangeProgress(e.ProgressPercentage);
                                this.LastRefresh = Environment.TickCount;
                                this.LastBytes = e.BytesReceived;
                            }
                            catch (Exception exception)
                            {
                                ErrorLog.WriteLine(exception);
                            }
                        };
                    }
                    base.BeginInvoke(method);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
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

        private void gpgCheckBoxAutoClose_CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.Miscellaneous.AutoCloseDownloadDialog = this.gpgCheckBoxAutoClose.Checked;
        }

        private void gpgLabelProgress_TextChanged(object sender, EventArgs e)
        {
            base.ttDefault.SetToolTip(this.gpgLabelProgress, this.gpgLabelProgress.Text);
        }

        private void InitializeComponent()
        {
            this.progressMeter = new ProgressMeter();
            this.skinButtonCancel = new SkinButton();
            this.gpgLabelProgress = new GPGLabel();
            this.gpgLabelRecieved = new GPGLabel();
            this.gpgLabelSpeed = new GPGLabel();
            this.gpgCheckBoxAutoClose = new GPGCheckBox();
            this.gpgLabelETA = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x155, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.progressMeter.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.progressMeter.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.progressMeter.BorderWidth = 2f;
            this.progressMeter.EmptyColor = Color.Gray;
            this.progressMeter.FillColor = Color.ForestGreen;
            this.progressMeter.Location = new Point(12, 0x73);
            this.progressMeter.Monitor = null;
            this.progressMeter.Name = "progressMeter";
            this.progressMeter.Progress = 0;
            this.progressMeter.RefreshInterval = 500;
            this.progressMeter.Size = new Size(0x178, 0x11);
            base.ttDefault.SetSuperTip(this.progressMeter, null);
            this.progressMeter.TabIndex = 7;
            this.progressMeter.Text = "progressMeter1";
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
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
            this.skinButtonCancel.Location = new Point(0x11a, 0xd9);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x6a, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 8;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgLabelProgress.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelProgress.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelProgress.AutoStyle = true;
            this.gpgLabelProgress.Font = new Font("Arial", 9.75f);
            this.gpgLabelProgress.ForeColor = Color.White;
            this.gpgLabelProgress.IgnoreMouseWheel = false;
            this.gpgLabelProgress.IsStyled = false;
            this.gpgLabelProgress.Location = new Point(12, 80);
            this.gpgLabelProgress.Name = "gpgLabelProgress";
            this.gpgLabelProgress.Size = new Size(0x178, 0x20);
            base.ttDefault.SetSuperTip(this.gpgLabelProgress, null);
            this.gpgLabelProgress.TabIndex = 9;
            this.gpgLabelProgress.TextAlign = ContentAlignment.BottomLeft;
            this.gpgLabelProgress.TextStyle = TextStyles.Default;
            this.gpgLabelProgress.TextChanged += new EventHandler(this.gpgLabelProgress_TextChanged);
            this.gpgLabelRecieved.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelRecieved.AutoSize = true;
            this.gpgLabelRecieved.AutoStyle = true;
            this.gpgLabelRecieved.Font = new Font("Arial", 9.75f);
            this.gpgLabelRecieved.ForeColor = Color.White;
            this.gpgLabelRecieved.IgnoreMouseWheel = false;
            this.gpgLabelRecieved.IsStyled = false;
            this.gpgLabelRecieved.Location = new Point(12, 0x8d);
            this.gpgLabelRecieved.Name = "gpgLabelRecieved";
            this.gpgLabelRecieved.Size = new Size(0, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelRecieved, null);
            this.gpgLabelRecieved.TabIndex = 10;
            this.gpgLabelRecieved.TextStyle = TextStyles.Default;
            this.gpgLabelSpeed.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelSpeed.AutoSize = true;
            this.gpgLabelSpeed.AutoStyle = true;
            this.gpgLabelSpeed.Font = new Font("Arial", 9.75f);
            this.gpgLabelSpeed.ForeColor = Color.White;
            this.gpgLabelSpeed.IgnoreMouseWheel = false;
            this.gpgLabelSpeed.IsStyled = false;
            this.gpgLabelSpeed.Location = new Point(12, 0x9d);
            this.gpgLabelSpeed.Name = "gpgLabelSpeed";
            this.gpgLabelSpeed.Size = new Size(0, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelSpeed, null);
            this.gpgLabelSpeed.TabIndex = 12;
            this.gpgLabelSpeed.TextStyle = TextStyles.Default;
            this.gpgCheckBoxAutoClose.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgCheckBoxAutoClose.AutoSize = true;
            this.gpgCheckBoxAutoClose.Location = new Point(12, 0xc9);
            this.gpgCheckBoxAutoClose.Name = "gpgCheckBoxAutoClose";
            this.gpgCheckBoxAutoClose.Size = new Size(0x103, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxAutoClose, null);
            this.gpgCheckBoxAutoClose.TabIndex = 13;
            this.gpgCheckBoxAutoClose.Text = "<LOC>Close this dialog when completed";
            this.gpgCheckBoxAutoClose.UsesBG = false;
            this.gpgCheckBoxAutoClose.UseVisualStyleBackColor = true;
            this.gpgCheckBoxAutoClose.CheckedChanged += new EventHandler(this.gpgCheckBoxAutoClose_CheckedChanged);
            this.gpgLabelETA.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelETA.AutoSize = true;
            this.gpgLabelETA.AutoStyle = true;
            this.gpgLabelETA.Font = new Font("Arial", 9.75f);
            this.gpgLabelETA.ForeColor = Color.White;
            this.gpgLabelETA.IgnoreMouseWheel = false;
            this.gpgLabelETA.IsStyled = false;
            this.gpgLabelETA.Location = new Point(12, 0xad);
            this.gpgLabelETA.Name = "gpgLabelETA";
            this.gpgLabelETA.Size = new Size(0, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelETA, null);
            this.gpgLabelETA.TabIndex = 14;
            this.gpgLabelETA.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(400, 0x12e);
            base.Controls.Add(this.gpgLabelETA);
            base.Controls.Add(this.gpgCheckBoxAutoClose);
            base.Controls.Add(this.gpgLabelSpeed);
            base.Controls.Add(this.gpgLabelRecieved);
            base.Controls.Add(this.gpgLabelProgress);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.progressMeter);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(400, 0x12e);
            this.MinimumSize = new Size(400, 0x12e);
            base.Name = "DlgDownloadProgress";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "";
            base.Controls.SetChildIndex(this.progressMeter, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgLabelProgress, 0);
            base.Controls.SetChildIndex(this.gpgLabelRecieved, 0);
            base.Controls.SetChildIndex(this.gpgLabelSpeed, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxAutoClose, 0);
            base.Controls.SetChildIndex(this.gpgLabelETA, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            if (this.DownloadCancelled != null)
            {
                this.DownloadCancelled(this, EventArgs.Empty);
            }
            this.Client.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(this.Client_DownloadProgressChanged);
            this.Client.DownloadFileCompleted -= new AsyncCompletedEventHandler(this.Client_DownloadFileCompleted);
            base.Close();
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        public override bool AllowRestoreWindow
        {
            get
            {
                return false;
            }
        }

        public WebClient Client
        {
            get
            {
                return this.mClient;
            }
        }

        public bool IsFinished
        {
            get
            {
                return (this.progressMeter.Progress >= 100);
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return true;
            }
        }

        public string Target
        {
            get
            {
                return this.mTarget;
            }
        }
    }
}

