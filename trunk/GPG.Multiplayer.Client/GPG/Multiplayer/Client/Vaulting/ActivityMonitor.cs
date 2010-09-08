namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class ActivityMonitor : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabelName;
        private GPGLabel gpgLabelStatus;
        private GPGPanel gpgPanel1;
        private GPGPanel gpgPanel2;
        private GPGPictureBox gpgPictureBoxContentType;
        private ImageList imageListContentTypes;
        private Guid LastStatusID = Guid.Empty;
        private IAdditionalContent mContent;
        private IVaultOperation mOperation;
        private ProgressMeter progressMeter;
        private SkinButton skinButtonRemove;

        public event EventHandler CancelOperation;

        public event EventHandler RemoveMonitor;

        public ActivityMonitor(IAdditionalContent content, IVaultOperation operation)
        {
            this.InitializeComponent();
            this.mContent = content;
            this.mOperation = operation;
            this.SetStatus(this.Operation.LastStatus, new object[0]);
            if (this.Operation.IsProgressFinished)
            {
                this.OnProgressFinished();
            }
            if (this.Operation.IsOperationFinished)
            {
                this.OnOperationFinished();
            }
            this.Operation.OperationFinished += new ContentOperationCallback(this.Operation_OperationFinished);
            this.gpgLabelName.Text = content.Name;
            this.gpgPictureBoxContentType.Image = this.imageListContentTypes.Images[this.Content.ContentType.ImageIndex];
            this.progressMeter.Monitor = operation;
            operation.StatusChanged += new StatusProviderEventHandler(this.operation_StatusChanged);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
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
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(ActivityMonitor));
            this.gpgPanel1 = new GPGPanel();
            this.gpgLabelName = new GPGLabel();
            this.progressMeter = new ProgressMeter();
            this.gpgPanel2 = new GPGPanel();
            this.gpgLabelStatus = new GPGLabel();
            this.skinButtonRemove = new SkinButton();
            this.gpgPictureBoxContentType = new GPGPictureBox();
            this.imageListContentTypes = new ImageList(this.components);
            this.gpgPanel1.SuspendLayout();
            this.gpgPanel2.SuspendLayout();
            ((ISupportInitialize) this.gpgPictureBoxContentType).BeginInit();
            base.SuspendLayout();
            this.gpgPanel1.AutoScroll = true;
            this.gpgPanel1.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel1.BorderThickness = 2;
            this.gpgPanel1.Controls.Add(this.gpgLabelName);
            this.gpgPanel1.DrawBorder = false;
            this.gpgPanel1.Location = new Point(0x1c, 5);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(0xc4, 0x19);
            this.gpgPanel1.TabIndex = 0;
            this.gpgLabelName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelName.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelName.AutoStyle = true;
            this.gpgLabelName.Font = new Font("Arial", 9.75f);
            this.gpgLabelName.ForeColor = Color.White;
            this.gpgLabelName.IgnoreMouseWheel = false;
            this.gpgLabelName.IsStyled = false;
            this.gpgLabelName.Location = new Point(0, 4);
            this.gpgLabelName.Name = "gpgLabelName";
            this.gpgLabelName.Size = new Size(0xb0, 15);
            this.gpgLabelName.TabIndex = 0;
            this.gpgLabelName.Text = "gpgLabelName";
            this.gpgLabelName.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabelName.TextStyle = TextStyles.Bold;
            this.progressMeter.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.progressMeter.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.progressMeter.BorderWidth = 2f;
            this.progressMeter.EmptyColor = Color.Gray;
            this.progressMeter.FillColor = Color.ForestGreen;
            this.progressMeter.Location = new Point(230, 9);
            this.progressMeter.Monitor = null;
            this.progressMeter.Name = "progressMeter";
            this.progressMeter.Progress = 0;
            this.progressMeter.RefreshInterval = 500;
            this.progressMeter.Size = new Size(0x110, 0x11);
            this.progressMeter.TabIndex = 1;
            this.progressMeter.Text = "progressMeter1";
            this.gpgPanel2.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPanel2.AutoScroll = true;
            this.gpgPanel2.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel2.BorderThickness = 2;
            this.gpgPanel2.Controls.Add(this.gpgLabelStatus);
            this.gpgPanel2.DrawBorder = false;
            this.gpgPanel2.Location = new Point(0x1fc, 5);
            this.gpgPanel2.Name = "gpgPanel2";
            this.gpgPanel2.Size = new Size(0x103, 0x19);
            this.gpgPanel2.TabIndex = 2;
            this.gpgLabelStatus.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelStatus.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelStatus.AutoStyle = true;
            this.gpgLabelStatus.Font = new Font("Arial", 9.75f);
            this.gpgLabelStatus.ForeColor = Color.White;
            this.gpgLabelStatus.IgnoreMouseWheel = false;
            this.gpgLabelStatus.IsStyled = false;
            this.gpgLabelStatus.Location = new Point(0, 4);
            this.gpgLabelStatus.Name = "gpgLabelStatus";
            this.gpgLabelStatus.Size = new Size(0xef, 15);
            this.gpgLabelStatus.TabIndex = 1;
            this.gpgLabelStatus.Text = "Idle";
            this.gpgLabelStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabelStatus.TextStyle = TextStyles.Default;
            this.skinButtonRemove.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.skinButtonRemove.AutoStyle = true;
            this.skinButtonRemove.BackColor = Color.Transparent;
            this.skinButtonRemove.ButtonState = 0;
            this.skinButtonRemove.DialogResult = DialogResult.OK;
            this.skinButtonRemove.DisabledForecolor = Color.Gray;
            this.skinButtonRemove.DrawColor = Color.White;
            this.skinButtonRemove.DrawEdges = false;
            this.skinButtonRemove.FocusColor = Color.Yellow;
            this.skinButtonRemove.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRemove.ForeColor = Color.White;
            this.skinButtonRemove.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonRemove.IsStyled = true;
            this.skinButtonRemove.Location = new Point(0x305, 8);
            this.skinButtonRemove.Name = "skinButtonRemove";
            this.skinButtonRemove.Size = new Size(0x18, 0x12);
            this.skinButtonRemove.SkinBasePath = @"Dialog\ContentManager\BtnDeleteRating";
            this.skinButtonRemove.TabIndex = 14;
            this.skinButtonRemove.TabStop = true;
            this.skinButtonRemove.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRemove.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonRemove, "<LOC>Remove");
            this.skinButtonRemove.Visible = false;
            this.skinButtonRemove.Click += new EventHandler(this.skinButtonRemove_Click);
            this.gpgPictureBoxContentType.Location = new Point(6, 10);
            this.gpgPictureBoxContentType.Name = "gpgPictureBoxContentType";
            this.gpgPictureBoxContentType.Size = new Size(0x10, 0x10);
            this.gpgPictureBoxContentType.TabIndex = 15;
            this.gpgPictureBoxContentType.TabStop = false;
            this.imageListContentTypes.ImageStream = (ImageListStreamer) manager.GetObject("imageListContentTypes.ImageStream");
            this.imageListContentTypes.TransparentColor = Color.Transparent;
            this.imageListContentTypes.Images.SetKeyName(0, "icon_map_sm_trans.png");
            this.imageListContentTypes.Images.SetKeyName(1, "icon_mod_sm_trans.png");
            this.imageListContentTypes.Images.SetKeyName(2, "icon_mag_glass_sm.png");
            this.imageListContentTypes.Images.SetKeyName(3, "icon_saved_sm.png");
            this.imageListContentTypes.Images.SetKeyName(4, "icon_my_upload_sm.png");
            this.imageListContentTypes.Images.SetKeyName(5, "icon_avail_upload_sm.png");
            this.imageListContentTypes.Images.SetKeyName(6, "icon_replay_sm.png");
            this.imageListContentTypes.Images.SetKeyName(7, "icon_missing_sm.png");
            this.imageListContentTypes.Images.SetKeyName(8, "icon_tools_sm_trans.png");
            this.imageListContentTypes.Images.SetKeyName(9, "icon_video_sm_trans.png");
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgPictureBoxContentType);
            base.Controls.Add(this.skinButtonRemove);
            base.Controls.Add(this.gpgPanel2);
            base.Controls.Add(this.progressMeter);
            base.Controls.Add(this.gpgPanel1);
            this.DoubleBuffered = true;
            base.Name = "ActivityMonitor";
            base.Size = new Size(0x323, 0x23);
            this.gpgPanel1.ResumeLayout(false);
            this.gpgPanel2.ResumeLayout(false);
            ((ISupportInitialize) this.gpgPictureBoxContentType).EndInit();
            base.ResumeLayout(false);
        }

        internal void OnOperationFinished()
        {
            this.skinButtonRemove.Visible = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Brushes.Silver, 2f))
            {
                e.Graphics.DrawRectangle(pen, base.ClientRectangle);
            }
        }

        internal void OnProgressFinished()
        {
            this.progressMeter.Progress = 100;
        }

        private void Operation_OperationFinished(ContentOperationCallbackArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.OnOperationFinished();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.OnOperationFinished();
            }
        }

        private void operation_StatusChanged(IStatusProvider sender, string status)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.operation_StatusChanged(sender, status);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.gpgLabelStatus.Text = Loc.Get(status);
            }
        }

        public void SetStatus(string status, params object[] args)
        {
            this.SetStatus(status, 0, args);
        }

        public void SetStatus(string status, int timeout, params object[] args)
        {
            VGen0 method = null;
            WaitCallback callBack = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.SetStatus(status, timeout, args);
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!base.Disposing && !base.IsDisposed)
            {
                if ((args != null) && (args.Length > 0))
                {
                    this.gpgLabelStatus.Text = string.Format(status, args);
                }
                else
                {
                    this.gpgLabelStatus.Text = status;
                }
                if (timeout > 0)
                {
                    this.LastStatusID = Guid.NewGuid();
                    if (callBack == null)
                    {
                        callBack = delegate (object s) {
                            Guid lastStatusID = this.LastStatusID;
                            Thread.Sleep(timeout);
                            if (!((!(this.LastStatusID == lastStatusID) || this.Disposing) || this.IsDisposed))
                            {
                                this.gpgLabelStatus.Text = "";
                            }
                        };
                    }
                    ThreadPool.QueueUserWorkItem(callBack);
                }
            }
        }

        private void skinButtonRemove_Click(object sender, EventArgs e)
        {
            if (this.RemoveMonitor != null)
            {
                this.RemoveMonitor(this, EventArgs.Empty);
            }
        }

        public IAdditionalContent Content
        {
            get
            {
                return this.mContent;
            }
        }

        public IVaultOperation Operation
        {
            get
            {
                return this.mOperation;
            }
        }
    }
}

