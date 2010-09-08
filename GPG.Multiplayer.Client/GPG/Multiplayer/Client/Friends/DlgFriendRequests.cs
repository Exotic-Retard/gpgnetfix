namespace GPG.Multiplayer.Client.Friends
{
    using DevExpress.Utils;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgFriendRequests : DlgBase
    {
        private IContainer components = null;
        private GPGPanel gpgPanelRequests;
        private bool ListChanged = false;
        private MappedObjectList<FriendRequest> mRequestList;
        private SkinButton skinButtonClose;

        public DlgFriendRequests(FrmMain mainForm, MappedObjectList<FriendRequest> requests)
        {
            this.InitializeComponent();
            base.MainForm = mainForm;
            this.mRequestList = requests;
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
            this.skinButtonClose.TabIndex = 7;
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
            base.Name = "DlgFriendRequests";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Friend Requests";
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
            foreach (FriendRequest request in this.RequestList)
            {
                FriendRequestPicker picker = new FriendRequestPicker(base.MainForm, request);
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
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FriendRequestPicker picker = sender as FriendRequestPicker;
                base.MainForm.OnAddFriend(picker.Request.RequestorName, picker.Request.RequestorID);
                DataAccess.ExecuteQuery("RemoveFriendRequest", new object[] { picker.Request.RequestorID });
                this.RequestList.Remove(picker.Request);
                this.ListChanged = true;
                base.MainForm.RefreshFriendInvites();
                if (this.RequestList.Count < 1)
                {
                    base.Close();
                }
                else
                {
                    this.RefreshList();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void reqPicker_Reject(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FriendRequestPicker picker = sender as FriendRequestPicker;
                if (DataAccess.ExecuteQuery("RemoveFriendRequest", new object[] { picker.Request.RequestorID }))
                {
                    this.RequestList.Remove(picker.Request);
                    base.MainForm.SystemMessage("<LOC>A rejection notification has been sent.", new object[0]);
                    Messaging.SendCustomCommand(picker.Request.RequestorName, CustomCommands.SystemMessage, new object[] { "<LOC>{0} has turned down your Friends invitation.", User.Current.Name });
                    this.ListChanged = true;
                    base.MainForm.RefreshFriendInvites();
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
            finally
            {
                this.Cursor = Cursors.Default;
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

        public MappedObjectList<FriendRequest> RequestList
        {
            get
            {
                return this.mRequestList;
            }
        }
    }
}

