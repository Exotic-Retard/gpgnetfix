namespace GPG.Multiplayer.Client.Clans
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ClanRequestPicker : UserControl
    {
        private IContainer components = null;
        private GPGLabel gpgLabelDate;
        private GPGLabel gpgLinkLabelName;
        private FrmMain mMainForm = null;
        private IUserRequest mRequest;
        private SkinButton skinButtonAccept;
        private SkinButton skinButtonReject;

        public event EventHandler Accept;

        public event EventHandler Reject;

        public ClanRequestPicker(FrmMain mainForm, IUserRequest request)
        {
            this.InitializeComponent();
            this.MainForm = mainForm;
            this.mRequest = request;
            this.gpgLinkLabelName.Text = request.Description;
            this.gpgLabelDate.Text = request.RequestDate.ToShortDateString();
            this.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgLinkLabelName_LinkClicked(object sender, EventArgs e)
        {
            if (this.Request is ClanInvite)
            {
                this.MainForm.OnViewClanProfileByPlayer(this.Request.RequestorName);
            }
            else
            {
                this.MainForm.OnViewPlayerProfile(this.Request.RequestorName);
            }
        }

        private void InitializeComponent()
        {
            this.gpgLabelDate = new GPGLabel();
            this.skinButtonReject = new SkinButton();
            this.skinButtonAccept = new SkinButton();
            this.gpgLinkLabelName = new GPGLabel();
            base.SuspendLayout();
            this.gpgLabelDate.AutoSize = true;
            this.gpgLabelDate.AutoStyle = true;
            this.gpgLabelDate.Font = new Font("Arial", 8f);
            this.gpgLabelDate.ForeColor = Color.White;
            this.gpgLabelDate.IgnoreMouseWheel = false;
            this.gpgLabelDate.IsStyled = false;
            this.gpgLabelDate.Location = new Point(0x13, 0x19);
            this.gpgLabelDate.Name = "gpgLabelDate";
            this.gpgLabelDate.Size = new Size(0x39, 14);
            this.gpgLabelDate.TabIndex = 1;
            this.gpgLabelDate.Text = "gpgLabel1";
            this.gpgLabelDate.TextStyle = TextStyles.Small;
            this.skinButtonReject.AutoStyle = true;
            this.skinButtonReject.BackColor = Color.Black;
            this.skinButtonReject.DialogResult = DialogResult.OK;
            this.skinButtonReject.DisabledForecolor = Color.Gray;
            this.skinButtonReject.DrawEdges = true;
            this.skinButtonReject.FocusColor = Color.Yellow;
            this.skinButtonReject.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonReject.ForeColor = Color.White;
            this.skinButtonReject.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonReject.IsStyled = true;
            this.skinButtonReject.Location = new Point(0x77, 0x2c);
            this.skinButtonReject.Name = "skinButtonReject";
            this.skinButtonReject.Size = new Size(90, 0x10);
            this.skinButtonReject.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonReject.TabIndex = 8;
            this.skinButtonReject.Text = "<LOC>Reject";
            this.skinButtonReject.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonReject.TextPadding = new Padding(0);
            this.skinButtonReject.Click += new EventHandler(this.skinButtonReject_Click);
            this.skinButtonAccept.AutoStyle = true;
            this.skinButtonAccept.BackColor = Color.Black;
            this.skinButtonAccept.DialogResult = DialogResult.OK;
            this.skinButtonAccept.DisabledForecolor = Color.Gray;
            this.skinButtonAccept.DrawEdges = true;
            this.skinButtonAccept.FocusColor = Color.Yellow;
            this.skinButtonAccept.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAccept.ForeColor = Color.White;
            this.skinButtonAccept.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAccept.IsStyled = true;
            this.skinButtonAccept.Location = new Point(20, 0x2c);
            this.skinButtonAccept.Name = "skinButtonAccept";
            this.skinButtonAccept.Size = new Size(90, 0x10);
            this.skinButtonAccept.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonAccept.TabIndex = 7;
            this.skinButtonAccept.Text = "<LOC>Accept";
            this.skinButtonAccept.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAccept.TextPadding = new Padding(0);
            this.skinButtonAccept.Click += new EventHandler(this.skinButtonAccept_Click);
            this.gpgLinkLabelName.AutoSize = true;
            this.gpgLinkLabelName.AutoStyle = true;
            this.gpgLinkLabelName.Font = new Font("Arial", 9.75f);
            this.gpgLinkLabelName.ForeColor = Color.White;
            this.gpgLinkLabelName.IgnoreMouseWheel = false;
            this.gpgLinkLabelName.IsStyled = false;
            this.gpgLinkLabelName.Location = new Point(0x11, 9);
            this.gpgLinkLabelName.Name = "gpgLinkLabelName";
            this.gpgLinkLabelName.Size = new Size(0x43, 0x10);
            this.gpgLinkLabelName.TabIndex = 9;
            this.gpgLinkLabelName.Text = "gpgLabel1";
            this.gpgLinkLabelName.TextStyle = TextStyles.Link;
            this.gpgLinkLabelName.Click += new EventHandler(this.gpgLinkLabelName_LinkClicked);
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgLinkLabelName);
            base.Controls.Add(this.skinButtonReject);
            base.Controls.Add(this.skinButtonAccept);
            base.Controls.Add(this.gpgLabelDate);
            base.Name = "ClanRequestPicker";
            base.Size = new Size(0xe2, 0x48);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(new SolidBrush(Program.Settings.StylePreferences.HighlightColor3), 4f))
            {
                e.Graphics.DrawRectangle(pen, base.ClientRectangle);
            }
        }

        private void skinButtonAccept_Click(object sender, EventArgs e)
        {
            if (this.Accept != null)
            {
                this.Accept(this, EventArgs.Empty);
            }
        }

        private void skinButtonReject_Click(object sender, EventArgs e)
        {
            if (this.Reject != null)
            {
                this.Reject(this, EventArgs.Empty);
            }
        }

        public FrmMain MainForm
        {
            get
            {
                return this.mMainForm;
            }
            set
            {
                this.mMainForm = value;
            }
        }

        public IUserRequest Request
        {
            get
            {
                return this.mRequest;
            }
        }
    }
}

