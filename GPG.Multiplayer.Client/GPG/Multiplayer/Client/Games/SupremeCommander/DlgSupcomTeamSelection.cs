namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgSupcomTeamSelection : DlgBase
    {
        public SkinButton btnCancel;
        public SkinButton btnInvite;
        public SkinButton btnOK;
        private IContainer components = null;
        private GPGDataGrid gpgTeamGrid;
        private GridView gridView1;
        private static object mInviteMutex = new object();
        private BindingList<IUser> mPendingUsers = new BindingList<IUser>();
        private BindingList<IUser> mUsers = new BindingList<IUser>();
        private GridColumn PlayerName;
        private static DlgSupcomTeamSelection sDlgSupcom = null;
        public static FrmMain sFrmMain = null;
        public static bool sIsConfirmed = false;
        public static bool sIsHosting = false;
        public static bool sIsJoining = false;
        private static string sLeader = "";
        public static List<int> sPlayerIDs = null;

        private DlgSupcomTeamSelection()
        {
            this.InitializeComponent();
            Loc.LocObject(this);
            this.gpgTeamGrid.DataSource = this.mUsers;
            this.mUsers.Add(User.Current);
            User.LoggingOut += new EventHandler(this.User_LoggingOut);
        }

        private void AbortGame()
        {
            sIsJoining = false;
            foreach (IUser user in this.mUsers)
            {
                if (user.Name != User.Current.Name)
                {
                    Messaging.SendCustomCommand(user.Name, CustomCommands.AutomatchEndAlliance, new object[0]);
                }
            }
            if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
            {
                sFrmMain.CancelRankedGame();
            }
            sFrmMain.EnableGameButtons();
            sLeader = "";
            sDlgSupcom = null;
            base.Close();
        }

        public void AddUser(IUser user)
        {
            int @int = ConfigSettings.GetInt("MaxTeamSize", 4);
            if (this.mUsers.Count < @int)
            {
                bool flag = true;
                foreach (IUser user2 in this.mUsers)
                {
                    if (user2.Name == user.Name)
                    {
                        flag = false;
                    }
                }
                foreach (IUser user2 in this.mPendingUsers)
                {
                    if (user2.Name == user.Name)
                    {
                        flag = false;
                    }
                }
                if (user.Online)
                {
                    if (flag)
                    {
                        this.mPendingUsers.Add(user);
                    }
                    Messaging.SendCustomCommand(user.Name, CustomCommands.AutomatchRequestAlliance, new object[0]);
                }
            }
            else
            {
                DlgMessage.ShowDialog(sFrmMain, Loc.Get("<LOC>Unable to add player."), Loc.Get("<LOC>You can only invite {0} people to a team.", new object[] { @int }));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.AbortGame();
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {
            string str = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>Enter the name of the player you want to invite to your team."), Loc.Get("<LOC>Invite"), false);
            MappedObjectList<User> objects = DataAccess.GetObjects<User>("GetPlayerDetails", new object[] { str });
            if (objects.Count > 0)
            {
                this.AddUser(objects[0]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.mUsers.Count > 1)
            {
                this.StatusMessageAllies("<LOC>Beginning Arranged Team Search.");
                List<string> allies = new List<string>();
                List<int> list2 = new List<int>();
                foreach (IUser user in this.mUsers)
                {
                    allies.Add(user.Name);
                    list2.Add(user.ID);
                }
                list2.Sort();
                sPlayerIDs = list2;
                string teamname = "";
                foreach (int num in list2)
                {
                    teamname = teamname + num.ToString() + " ";
                }
                teamname = teamname.Trim();
                string kind = allies.Count.ToString() + "v" + allies.Count.ToString();
                sFrmMain.PlayRankedGame(false, kind, allies, teamname);
                SupcomAutomatch.GetSupcomAutomatch().OnStatusChanged += new StringEventHandler2(this.DlgSupcomTeamSelection_OnStatusChanged);
                sFrmMain.StatusButtonRankedGameCancel.Click += new EventHandler(this.StatusButtonRankedGameCancel_Click);
                SupcomAutomatch.GetSupcomAutomatch().OnExit += new EventHandler(DlgSupcomTeamSelection.DlgSupcomTeamSelection_OnExit);
                this.StatusMessageAllies("AUTOSEARCH");
                this.btnInvite.Enabled = false;
                this.btnOK.Enabled = false;
            }
            else
            {
                DlgMessage.Show(sFrmMain, Loc.Get("<LOC>You need to recruit some allies first."));
            }
        }

        private static void cancel_Click(object sender, EventArgs e)
        {
            SkinStatusButton button = sender as SkinStatusButton;
            string name = button.Tag.ToString();
            if (sFrmMain != null)
            {
                sFrmMain.RemoveMiddleButton(button);
                sFrmMain.SetStatus(Loc.Get("<LOC>Arranged Team Canceled."), 0x1388, new object[0]);
            }
            LeaveTeam(name);
        }

        private static void Create()
        {
            if (sDlgSupcom == null)
            {
                sDlgSupcom = new DlgSupcomTeamSelection();
                sFrmMain.ClearStatus();
            }
        }

        private static void DisplayUsers()
        {
            try
            {
                string str = "<LOC>Arranged Team Setup: ";
                string str2 = "";
                string str3 = "";
                foreach (IUser user in sDlgSupcom.mUsers)
                {
                    str2 = str2 + str3 + user.Name;
                    str3 = ", ";
                }
                str2 = str2.TrimEnd(new char[] { ',', ' ' });
                foreach (IUser user2 in sDlgSupcom.mUsers)
                {
                    if (user2.ID != User.Current.ID)
                    {
                        Messaging.SendCustomCommand(user2.Name, CustomCommands.AutomatchTeamMembers, new object[] { str, str2 });
                    }
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

        private static void DlgSupcomTeamSelection_OnExit(object sender, EventArgs e)
        {
            SupcomAutomatch.GetSupcomAutomatch().OnExit -= new EventHandler(DlgSupcomTeamSelection.DlgSupcomTeamSelection_OnExit);
            sFrmMain.ClearStatus();
            sLeader = "";
            sIsJoining = false;
            sFrmMain.LeaveChat();
            sFrmMain.JoinChat("");
            if (sDlgSupcom != null)
            {
                try
                {
                    sDlgSupcom.Close();
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
            sDlgSupcom = null;
        }

        private void DlgSupcomTeamSelection_OnStatusChanged(string text, params object[] args)
        {
            this.StatusMessageAllies(text);
            if (!((bool) args[0]))
            {
                base.Visible = false;
            }
        }

        private void InitializeComponent()
        {
            this.gpgTeamGrid = new GPGDataGrid();
            this.gridView1 = new GridView();
            this.PlayerName = new GridColumn();
            this.btnInvite = new SkinButton();
            this.btnOK = new SkinButton();
            this.btnCancel = new SkinButton();
            this.gpgTeamGrid.BeginInit();
            this.gridView1.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgTeamGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgTeamGrid.CustomizeStyle = false;
            this.gpgTeamGrid.EmbeddedNavigator.Name = "";
            this.gpgTeamGrid.Location = new Point(0x24, 0x53);
            this.gpgTeamGrid.MainView = this.gridView1;
            this.gpgTeamGrid.Name = "gpgTeamGrid";
            this.gpgTeamGrid.ShowOnlyPredefinedDetails = true;
            this.gpgTeamGrid.Size = new Size(290, 0xd8);
            this.gpgTeamGrid.TabIndex = 7;
            this.gpgTeamGrid.ViewCollection.AddRange(new BaseView[] { this.gridView1 });
            this.gridView1.ActiveFilterString = "";
            this.gridView1.Columns.AddRange(new GridColumn[] { this.PlayerName });
            this.gridView1.GridControl = this.gpgTeamGrid;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.PlayerName.Caption = "<LOC>Team Players";
            this.PlayerName.FieldName = "Name";
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.OptionsColumn.ReadOnly = true;
            this.PlayerName.Visible = true;
            this.PlayerName.VisibleIndex = 0;
            this.btnInvite.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnInvite.AutoStyle = true;
            this.btnInvite.BackColor = Color.Black;
            this.btnInvite.DialogResult = DialogResult.None;
            this.btnInvite.DisabledForecolor = Color.Gray;
            this.btnInvite.DrawEdges = true;
            this.btnInvite.FocusColor = Color.Yellow;
            this.btnInvite.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnInvite.ForeColor = Color.White;
            this.btnInvite.HorizontalScalingMode = ScalingModes.Tile;
            this.btnInvite.IsStyled = true;
            this.btnInvite.Location = new Point(0x23, 0x131);
            this.btnInvite.Name = "btnInvite";
            this.btnInvite.Size = new Size(0x5d, 0x1c);
            this.btnInvite.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnInvite.TabIndex = 0x17;
            this.btnInvite.Text = "<LOC>Manual Invite";
            this.btnInvite.TextAlign = ContentAlignment.MiddleCenter;
            this.btnInvite.TextPadding = new Padding(0);
            this.btnInvite.Click += new EventHandler(this.btnInvite_Click);
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.AutoStyle = true;
            this.btnOK.BackColor = Color.Black;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.DisabledForecolor = Color.Gray;
            this.btnOK.DrawEdges = true;
            this.btnOK.FocusColor = Color.Yellow;
            this.btnOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.HorizontalScalingMode = ScalingModes.Tile;
            this.btnOK.IsStyled = true;
            this.btnOK.Location = new Point(0x86, 0x131);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x5d, 0x1c);
            this.btnOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnOK.TabIndex = 0x18;
            this.btnOK.Text = "<LOC>Launch";
            this.btnOK.TextAlign = ContentAlignment.MiddleCenter;
            this.btnOK.TextPadding = new Padding(0);
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.AutoStyle = true;
            this.btnCancel.BackColor = Color.Black;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.DisabledForecolor = Color.Gray;
            this.btnCancel.DrawEdges = true;
            this.btnCancel.FocusColor = Color.Yellow;
            this.btnCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCancel.IsStyled = true;
            this.btnCancel.Location = new Point(0xe9, 0x131);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x5d, 0x1c);
            this.btnCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnCancel.TabIndex = 0x19;
            this.btnCancel.Text = "<LOC>Cancel";
            this.btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCancel.TextPadding = new Padding(0);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x16a, 0x18c);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnInvite);
            base.Controls.Add(this.gpgTeamGrid);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgSupcomTeamSelection";
            this.Text = "<LOC>Team Selection";
            base.Controls.SetChildIndex(this.gpgTeamGrid, 0);
            base.Controls.SetChildIndex(this.btnInvite, 0);
            base.Controls.SetChildIndex(this.btnOK, 0);
            base.Controls.SetChildIndex(this.btnCancel, 0);
            this.gpgTeamGrid.EndInit();
            this.gridView1.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static void LeaveTeam()
        {
            if (sLeader != "")
            {
                LeaveTeam(sLeader);
            }
        }

        private static void LeaveTeam(string name)
        {
            sIsJoining = false;
            Messaging.SendCustomCommand(name, CustomCommands.AutomatchCancelAlliance, new object[0]);
            sLeader = "";
            sDlgSupcom = null;
            sFrmMain.EnableGameButtons();
        }

        protected override bool OnCloseButton()
        {
            this.AbortGame();
            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            sIsHosting = false;
            sIsJoining = false;
            base.OnClosed(e);
            sFrmMain.ClearStatus();
            SupcomAutomatch.GetSupcomAutomatch().OnStatusChanged -= new StringEventHandler2(this.DlgSupcomTeamSelection_OnStatusChanged);
            SupcomAutomatch.GetSupcomAutomatch().OnExit -= new EventHandler(DlgSupcomTeamSelection.DlgSupcomTeamSelection_OnExit);
            sDlgSupcom = null;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        public static void ProcessCommand(FrmMain main, CustomCommands cmd, int senderID, string senderName, string[] args)
        {
            sFrmMain = main;
            if (!(main.IsDisposed || main.Disposing))
            {
                main.Invoke((VGen0)delegate {
                    Create();
                });
            }
            if (sDlgSupcom != null)
            {
                CustomCommands commands = cmd;
                int num = senderID;
                string str = senderName;
                string[] strArray = args;
                try
                {
                    int num2;
                    switch (commands)
                    {
                        case CustomCommands.AutomatchRequestAlliance:
                            lock (mInviteMutex)
                            {
                                if ((((sLeader == "") && !sIsHosting) && !sIsJoining) && !(main.IsDisposed || main.Disposing))
                                {
                                    main.BeginInvoke((VGen2)delegate (object objname, object objmain) {
                                        FrmMain mainForm = (FrmMain) objmain;
                                        if (((new DlgYesNo(mainForm, Loc.Get("<LOC>Arranged Team Request"), objname.ToString() + Loc.Get("<LOC> has invited you to play in a rated team game.  Do you wish to join?")).ShowDialog() == DialogResult.Yes) && (SupcomAutomatch.GetSupcomAutomatch().State == SupcomAutoState.Unavailable)) && !((!(sLeader == "") || sIsHosting) || sIsJoining))
                                        {
                                            sIsConfirmed = false;
                                            sIsJoining = true;
                                            Messaging.SendCustomCommand(objname.ToString(), CustomCommands.AutomatchConfirmAlliance, new object[0]);
                                            mainForm.SetStatus(Loc.Get("<LOC>Arranged Team Leader: ") + objname.ToString(), new object[0]);
                                            mainForm.DisableGameButtons();
                                            SkinStatusButton cancel = new SkinStatusButton();
                                            cancel.Click += new EventHandler(DlgSupcomTeamSelection.cancel_Click);
                                            cancel.Text = Loc.Get("<LOC>Leave Team");
                                            cancel.Tag = objname.ToString();
                                            mainForm.AddMiddleButton(cancel);
                                            sLeader = objname.ToString();
                                            ThreadPool.QueueUserWorkItem(delegate (object o) {
                                                Thread.Sleep(ConfigSettings.GetInt("TeamJoinTimeout", 0x1388));
                                                cancel.BeginInvoke((VGen1)delegate (object ocancel) {
                                                    if (!sIsConfirmed && (ocancel is SkinStatusButton))
                                                    {
                                                        (ocancel as SkinStatusButton).PerformClick();
                                                    }
                                                }, new object[] { o });
                                            }, cancel);
                                        }
                                    }, new object[] { str, main });
                                }
                            }
                            return;

                        case CustomCommands.AutomatchAcknowledgeTeam:
                            sIsConfirmed = true;
                            return;

                        default:
                            if (commands != CustomCommands.AutomatchConfirmAlliance)
                            {
                                goto Label_025A;
                            }
                            for (num2 = sDlgSupcom.mPendingUsers.Count - 1; num2 >= 0; num2--)
                            {
                                if (sDlgSupcom.mPendingUsers[num2].Name == str)
                                {
                                    int @int = ConfigSettings.GetInt("MaxTeamSize", 4);
                                    if (sDlgSupcom.mUsers.Count < @int)
                                    {
                                        sDlgSupcom.mUsers.Add(sDlgSupcom.mPendingUsers[num2]);
                                        Messaging.SendCustomCommand(senderName, CustomCommands.AutomatchAcknowledgeTeam, new object[0]);
                                        sDlgSupcom.mPendingUsers.RemoveAt(num2);
                                    }
                                    else
                                    {
                                        Messaging.SendCustomCommand(sDlgSupcom.mPendingUsers[num2].Name, CustomCommands.AutomatchEndAlliance, new object[0]);
                                        sDlgSupcom.mPendingUsers.RemoveAt(num2);
                                    }
                                    break;
                                }
                            }
                            break;
                    }
                    DisplayUsers();
                    return;
                Label_025A:
                    if (commands == CustomCommands.AutomatchCancelAlliance)
                    {
                        sIsJoining = false;
                        for (num2 = sDlgSupcom.mPendingUsers.Count - 1; num2 >= 0; num2--)
                        {
                            if (sDlgSupcom.mPendingUsers[num2].Name == str)
                            {
                                sDlgSupcom.mPendingUsers.RemoveAt(num2);
                                DisplayUsers();
                                break;
                            }
                        }
                        for (num2 = sDlgSupcom.mUsers.Count - 1; num2 >= 0; num2--)
                        {
                            if (sDlgSupcom.mUsers[num2].Name == str)
                            {
                                sDlgSupcom.mUsers.RemoveAt(num2);
                                DisplayUsers();
                                break;
                            }
                        }
                        if (SupcomAutomatch.GetSupcomAutomatch().State != SupcomAutoState.Unavailable)
                        {
                            sDlgSupcom.BeginInvoke((VGen0)delegate {
                                sDlgSupcom.AbortGame();
                            });
                        }
                    }
                    else
                    {
                        switch (commands)
                        {
                            case CustomCommands.AutomatchEndAlliance:
                                sIsJoining = false;
                                if (!(main.IsDisposed || main.Disposing))
                                {
                                    main.BeginInvoke((VGen2)delegate (object objname, object objmain) {
                                        FrmMain mainForm = (FrmMain) objmain;
                                        DlgMessage.Show(mainForm, Loc.Get("<LOC>The team leader has canceled the arranged team game."));
                                        mainForm.ClearStatus();
                                        mainForm.EnableGameButtons();
                                    }, new object[] { str, main });
                                }
                                sLeader = "";
                                sDlgSupcom = null;
                                break;

                            case CustomCommands.AutomatchStatusMessage:
                                if (strArray[0] == "AUTOSEARCH")
                                {
                                    SupcomAutomatch.GetSupcomAutomatch().OnExit += new EventHandler(DlgSupcomTeamSelection.DlgSupcomTeamSelection_OnExit);
                                }
                                else if (!(main.IsDisposed || main.Disposing))
                                {
                                    main.BeginInvoke((VGen3)delegate (object objname, object objmain, object objargs) {
                                        FrmMain curmain = (FrmMain) objmain;
                                        if (objargs.ToString().IndexOf("launch") >= 0)
                                        {
                                            curmain.ClearStatus();
                                        }
                                        curmain.SetStatus(Loc.Get(objargs.ToString()), new object[0]);
                                    }, new object[] { str, main, strArray[0] });
                                }
                                break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        public static DlgSupcomTeamSelection ShowSelection()
        {
            Create();
            sDlgSupcom.Show();
            sIsHosting = true;
            sFrmMain.DisableGameButtons();
            return sDlgSupcom;
        }

        private void StatusButtonRankedGameCancel_Click(object sender, EventArgs e)
        {
            this.AbortGame();
        }

        private void StatusMessageAllies(string message)
        {
            foreach (IUser user in this.mUsers)
            {
                if (user.ID != User.Current.ID)
                {
                    Messaging.SendCustomCommand(user.Name, CustomCommands.AutomatchStatusMessage, new object[] { message });
                }
            }
        }

        private void User_LoggingOut(object sender, EventArgs e)
        {
            if (sLeader != "")
            {
                Messaging.SendCustomCommand(sLeader, CustomCommands.AutomatchCancelAlliance, new object[0]);
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        private static bool IsVisible
        {
            get
            {
                return ((sDlgSupcom != null) && sDlgSupcom.Visible);
            }
        }
    }
}

