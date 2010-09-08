namespace GPG.Multiplayer.Client.Clans
{
    using DevExpress.Utils;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DlgClanRequests<type> : DlgBase where type: class, IUserRequest
    {
        private IContainer components;
        private GPGPanel gpgPanelRequests;
        private bool ListChanged;
        private MappedObjectList<type> mRequestList;
        private IUserRequest mSelectedRequest;
        private SkinButton skinButtonClose;

        public event UserRequestEventHandler AcceptRequest;

        public event UserRequestEventHandler RejectRequest;

        public DlgClanRequests(FrmMain mainform, MappedObjectList<type> requests)
        {
            this.components = null;
            this.ListChanged = false;
            this.InitializeComponent();
            base.MainForm = mainform;
            this.mRequestList = requests;
            if (typeof(type) == typeof(ClanRequest))
            {
                this.Text = "<LOC>Clan Requests";
            }
            else
            {
                this.Text = "<LOC>Clan Invites";
            }
            this.RefreshList();
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
            this.gpgPanelRequests = new GPGPanel();
            this.skinButtonClose = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0xc1, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgPanelRequests.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelRequests.AutoScroll = true;
            this.gpgPanelRequests.BackColor = Color.Transparent;
            this.gpgPanelRequests.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelRequests.BorderThickness = 2;
            this.gpgPanelRequests.DrawBorder = false;
            this.gpgPanelRequests.Location = new Point(12, 0x53);
            this.gpgPanelRequests.Name = "gpgPanelRequests";
            this.gpgPanelRequests.Size = new Size(0xe4, 120);
            base.ttDefault.SetSuperTip(this.gpgPanelRequests, null);
            this.gpgPanelRequests.TabIndex = 4;
            this.skinButtonClose.Anchor = AnchorStyles.Bottom;
            this.skinButtonClose.AutoStyle = true;
            this.skinButtonClose.BackColor = Color.Black;
            this.skinButtonClose.ButtonState = 0;
            this.skinButtonClose.DialogResult = DialogResult.OK;
            this.skinButtonClose.DisabledForecolor = Color.Gray;
            this.skinButtonClose.DrawColor = Color.White;
            this.skinButtonClose.DrawEdges = true;
            this.skinButtonClose.FocusColor = Color.Yellow;
            this.skinButtonClose.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClose.ForeColor = Color.White;
            this.skinButtonClose.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClose.IsStyled = true;
            this.skinButtonClose.Location = new Point(0x58, 0xd1);
            this.skinButtonClose.Name = "skinButtonClose";
            this.skinButtonClose.Size = new Size(0x54, 0x19);
            this.skinButtonClose.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClose, null);
            this.skinButtonClose.TabIndex = 8;
            this.skinButtonClose.TabStop = true;
            this.skinButtonClose.Text = "<LOC>Close";
            this.skinButtonClose.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClose.TextPadding = new Padding(0);
            this.skinButtonClose.Click += new EventHandler(this.skinButtonClose_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonClose;
            base.ClientSize = new Size(0xfc, 0x123);
            base.Controls.Add(this.skinButtonClose);
            base.Controls.Add(this.gpgPanelRequests);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0xfc, 0x4b0);
            this.MinimumSize = new Size(0xfc, 0x123);
            base.Name = "DlgClanRequests";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "";
            base.Controls.SetChildIndex(this.gpgPanelRequests, 0);
            base.Controls.SetChildIndex(this.skinButtonClose, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.ListChanged)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.OnClosing(e);
        }

        private void RefreshList()
        {
            this.gpgPanelRequests.Controls.Clear();
            int num = 0;
            int num2 = 15;
            foreach (IUserRequest request in this.RequestList)
            {
                ClanRequestPicker picker = new ClanRequestPicker(base.MainForm, request);
                picker.Location = new Point(0, (picker.Height + num2) * num);
                picker.Accept += new EventHandler(this.reqPicker_Accept);
                picker.Reject += new EventHandler(this.reqPicker_Reject);
                this.gpgPanelRequests.Controls.Add(picker);
                num++;
            }
            this.Localize();
        }

        private void reqPicker_Accept(object sender, EventArgs e)
        {
            if (this.AcceptRequest != null)
            {
                ClanRequestPicker picker = sender as ClanRequestPicker;
                this.mSelectedRequest = picker.Request;
                if (this.AcceptRequest(this, new UserRequestEventArgs(this.SelectedRequest)))
                {
                    this.RequestList.Remove(picker.Request as type);
                    this.RequestList.RemoveIndexes(picker.Request as type);
                    this.ListChanged = true;
                    base.MainForm.RefreshClanInviteCount();
                    base.MainForm.RefreshClanRequestCount();
                    if (this.RequestList.Count < 1)
                    {
                        base.Close();
                    }
                    else
                    {
                        this.RefreshList();
                    }
                }
            }
        }

        private void reqPicker_Reject(object sender, EventArgs e)
        {
            if (this.RejectRequest != null)
            {
                ClanRequestPicker picker = sender as ClanRequestPicker;
                if (this.RejectRequest(this, new UserRequestEventArgs(picker.Request)))
                {
                    this.RequestList.Remove(picker.Request as type);
                    this.RequestList.RemoveIndexes(picker.Request as type);
                    this.ListChanged = true;
                    this.RefreshList();
                    base.MainForm.RefreshClanInviteCount();
                    base.MainForm.RefreshClanRequestCount();
                    if (this.RequestList.Count < 1)
                    {
                        base.Close();
                    }
                }
            }
        }

        private void skinButtonClose_Click(object sender, EventArgs e)
        {
            if (this.ListChanged)
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.Cancel;
            }
            base.Close();
        }

        public MappedObjectList<type> RequestList
        {
            get
            {
                return this.mRequestList;
            }
        }

        public IUserRequest SelectedRequest
        {
            get
            {
                return this.mSelectedRequest;
            }
        }
    }
}

