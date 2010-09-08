namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgGameLobby : DlgBase, IGameLobby
    {
        private ChatPanel chatPanel;
        private IContainer components = null;
        private SpaceSiegeGame mGame;
        private bool OverrideClose = false;
        private PnlLobbyParticipant pnlLobbyParticipant1;
        private PnlLobbyParticipant pnlLobbyParticipant2;
        private PnlLobbyParticipant pnlLobbyParticipant3;
        private PnlLobbyParticipant pnlLobbyParticipant4;

        private DlgGameLobby(SpaceSiegeGame game)
        {
            this.InitializeComponent();
            this.mGame = game;
            this.chatPanel.AddChatParticipant(User.Current);
            for (int i = 0; i < this.ParticipantPanels.Length; i++)
            {
                this.ParticipantPanels[i].Game = game;
                this.ParticipantPanels[i].Postition = i;
                this.ParticipantPanels[i].CharacterChanged += new LobbyParticipantEventHandler(this.DlgGameLobby_CharacterChanged);
                this.ParticipantPanels[i].ReadyStateChanged += new LobbyParticipantEventHandler(this.DlgGameLobby_ReadyStateChanged);
                this.ParticipantPanels[i].OpenPosition += new LobbyParticipantEventHandler(this.DlgGameLobby_OpenPosition);
                this.ParticipantPanels[i].ClosePosition += new LobbyParticipantEventHandler(this.DlgGameLobby_ClosePosition);
                this.ParticipantPanels[i].LeaveLobby += new LobbyParticipantEventHandler(this.DlgGameLobby_LeaveLobby);
                this.ParticipantPanels[i].KickParticipant += new LobbyParticipantEventHandler(this.DlgGameLobby_KickParticipant);
                this.ParticipantPanels[i].DisbandLobby += new LobbyParticipantEventHandler(this.DlgGameLobby_DisbandLobby);
                this.ParticipantPanels[i].InviteParticipant += new LobbyParticipantEventHandler(this.DlgGameLobby_InviteParticipant);
                this.ParticipantPanels[i].OccupyPosition += new LobbyParticipantEventHandler(this.DlgGameLobby_OccupyPosition);
                this.ParticipantPanels[i].LaunchGame += new LobbyParticipantEventHandler(this.DlgGameLobby_LaunchGame);
                this.ParticipantPanels[i].ErrorMessage += new StringEventHandler(this.DlgGameLobby_ErrorMessage);
            }
        }

        private void ChangePosition(SpaceSiegeGameParticipant participant, int newPosition)
        {
            this.ParticipantPanels[participant.Position].SetParticipant(null);
            this.ParticipantPanels[newPosition].SetParticipant(participant);
        }

        private void chatPanel_SendChatMessage(string text)
        {
            this.SendMessage(SpaceSiegeLobbyMessages.ChatMessage, new object[] { text });
        }

        private ITextEffect chatPanel_StyleChatLine(ChatPanel sender, User user, TextLine line)
        {
            if (user.ID == User.Current.ID)
            {
                return new FontColorEffect(Program.Settings.Chat.Appearance.SelfColor, Program.Settings.Chat.Appearance.SelfFont);
            }
            if (user.ID == this.Game.Host.ID)
            {
                return new FontColorEffect(Program.Settings.Chat.Appearance.GameColor, Program.Settings.Chat.Appearance.DefaultFont);
            }
            if (user.IsSystem)
            {
                if (user.Equals(User.Error))
                {
                    return new FontColorEffect(Program.Settings.Chat.Appearance.ErrorColor, Program.Settings.Chat.Appearance.ErrorFont);
                }
                if (user.Equals(User.Event))
                {
                    return new FontColorEffect(Program.Settings.Chat.Appearance.EventColor, Program.Settings.Chat.Appearance.EventFont);
                }
                if (user.Equals(User.System))
                {
                    return new FontColorEffect(Program.Settings.Chat.Appearance.SystemColor, Program.Settings.Chat.Appearance.SystemFont);
                }
                if (user.Equals(User.Game))
                {
                    return new FontColorEffect(Program.Settings.Chat.Appearance.GameColor, Program.Settings.Chat.Appearance.GameFont);
                }
            }
            return null;
        }

        private void ClosePosition(int position)
        {
            this.Game.PositionStates[position] = GamePositionStates.Closed;
            this.ParticipantPanels[position].IsOpen = false;
            this.ParticipantPanels[position].SetParticipant(null);
            this.UpdateHostedGameRecord();
        }

        private void DisbandLobby()
        {
            new QuazalQuery("RemoveSpaceSiegeGame", new object[0]).ExecuteNonQuery();
            this.SendMessage(SpaceSiegeLobbyMessages.Disband, new object[0]);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgGameLobby_AfterShown(FrmBase form)
        {
            this.chatPanel.Refresh();
        }

        private void DlgGameLobby_CharacterChanged(PnlLobbyParticipant sender)
        {
            this.UpdateHostedGameRecord();
            this.RefreshParticipant();
        }

        private void DlgGameLobby_ClosePosition(PnlLobbyParticipant sender)
        {
            if (sender.Participant != null)
            {
                this.KickParticipant(sender.Participant);
            }
            this.ClosePosition(sender.Postition);
            this.SendMessage(SpaceSiegeLobbyMessages.ClosePosition, new object[] { sender.Postition });
        }

        private void DlgGameLobby_DisbandLobby(PnlLobbyParticipant sender)
        {
            if (new DlgYesNo(base.MainForm, "<LOC>Confirm", "<LOC>Do you really want to disband this game lobby?").ShowDialog() == DialogResult.Yes)
            {
                this.DisbandLobby();
                this.Game.Members.Clear();
                base.Close();
            }
        }

        private void DlgGameLobby_ErrorMessage(string text)
        {
            this.chatPanel.ErrorMessage(text, new object[0]);
        }

        private void DlgGameLobby_InviteParticipant(PnlLobbyParticipant sender)
        {
            DialogResult result;
            string name = DlgAskQuestion.AskQuestion(base.MainForm, "<LOC>Please enter the name of the player to invite", "<LOC>Invite", false, out result);
            if (((result == DialogResult.OK) && (name != null)) && (name.Length > 0))
            {
                if ((name == null) || (name.Length < 3))
                {
                    this.chatPanel.ErrorMessage("<LOC>Player name cannot be less than {0} characters.", new object[] { 3 });
                }
                else if (this.Game.FindMember(name) != null)
                {
                    this.chatPanel.SystemMessage("<LOC>{0} is already in your game.", new object[] { name });
                }
                else if (this.Game.AvailablePositions < 1)
                {
                    this.chatPanel.SystemMessage("<LOC>Unable to invite more members, your game is already full.", new object[0]);
                }
                else
                {
                    User user;
                    if (base.MainForm.TryFindUser(name, true, out user))
                    {
                        if ((user != null) && user.Online)
                        {
                            this.SendMessage(name, SpaceSiegeLobbyMessages.Invite, new object[0]);
                            this.chatPanel.SystemMessage("<LOC>Invitation sent.", new object[0]);
                        }
                        else
                        {
                            this.chatPanel.SystemMessage("<LOC>{0} is offline and cannot join your game at this time.", new object[] { name });
                        }
                    }
                    else
                    {
                        this.chatPanel.SystemMessage("<LOC>Unable to locate player {0}", new object[] { name });
                    }
                }
            }
        }

        private void DlgGameLobby_KickParticipant(PnlLobbyParticipant sender)
        {
            if (sender.Participant != null)
            {
                this.KickParticipant(sender.Participant);
            }
        }

        private void DlgGameLobby_LaunchGame(PnlLobbyParticipant sender)
        {
            this.RemoveGameFromList();
            this.SendMessage(SpaceSiegeLobbyMessages.ChatMessage, new object[] { "I have launched the game." });
            this.Game.HasLaunched = true;
            sender.Participant.IsReady = false;
            sender.SetParticipant(sender.Participant);
            GameInformation.SelectedGame.UserForcedCommandLineArgs = string.Format("character={0} players={1}", (this.Game.GetSelf() as SpaceSiegeGameParticipant).Character.CharacterName, this.Game.Members.Count);
            GPGnetSelectedGame.ProfileName = (this.Game.GetSelf() as SpaceSiegeGameParticipant).Character.CharacterName;
            base.MainForm.GameHosted += new EventHandler(this.MainForm_GameHosted);
            base.MainForm.HostGame(this.Game.ID);
        }

        private void DlgGameLobby_LeaveLobby(PnlLobbyParticipant sender)
        {
            if (new DlgYesNo(base.MainForm, "<LOC>Confirm", "<LOC>Do you really want to leave this game lobby?").ShowDialog() == DialogResult.Yes)
            {
                this.LeaveLobby();
            }
        }

        private void DlgGameLobby_OccupyPosition(PnlLobbyParticipant sender)
        {
            this.SendMessage(SpaceSiegeLobbyMessages.ChangePosition, new object[] { sender.Postition });
            this.ChangePosition(this.Game.GetSelf() as SpaceSiegeGameParticipant, sender.Postition);
        }

        private void DlgGameLobby_OpenPosition(PnlLobbyParticipant sender)
        {
            this.OpenPosition(sender.Postition);
            this.SendMessage(SpaceSiegeLobbyMessages.OpenPosition, new object[] { sender.Postition });
        }

        private void DlgGameLobby_ReadyStateChanged(PnlLobbyParticipant sender)
        {
            this.RefreshParticipant();
            this.UpdateLaunchAbility();
        }

        void IGameLobby.Close()
        {
            base.Close();
        }

        public static DlgGameLobby HostGame()
        {
            SpaceSiegeGame game = new SpaceSiegeGame(new SpaceSiegeGameParticipant(User.Current.Name, User.Current.ID));
            game.Host.Position = 0;
            DlgGameLobby lobby = new DlgGameLobby(game);
            lobby.chatPanel.gpgChatGrid.Refresh();
            return lobby;
        }

        private void InitializeComponent()
        {
            this.chatPanel = new ChatPanel();
            this.pnlLobbyParticipant1 = new PnlLobbyParticipant();
            this.pnlLobbyParticipant2 = new PnlLobbyParticipant();
            this.pnlLobbyParticipant3 = new PnlLobbyParticipant();
            this.pnlLobbyParticipant4 = new PnlLobbyParticipant();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x2f3, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.chatPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.chatPanel.Location = new Point(0x10, 0x109);
            this.chatPanel.Name = "chatPanel";
            this.chatPanel.ShowSlashCommands = false;
            this.chatPanel.Size = new Size(0x30d, 0xf1);
            base.ttDefault.SetSuperTip(this.chatPanel, null);
            this.chatPanel.TabIndex = 7;
            this.chatPanel.SendChatMessage += new StringEventHandler(this.chatPanel_SendChatMessage);
            this.chatPanel.StyleChatLine += new ChatLineStyleEventHandler(this.chatPanel_StyleChatLine);
            this.pnlLobbyParticipant1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlLobbyParticipant1.BackColor = Color.Transparent;
            this.pnlLobbyParticipant1.Game = null;
            this.pnlLobbyParticipant1.IsOpen = true;
            this.pnlLobbyParticipant1.Location = new Point(11, 0x4c);
            this.pnlLobbyParticipant1.Name = "pnlLobbyParticipant1";
            this.pnlLobbyParticipant1.Postition = -1;
            this.pnlLobbyParticipant1.Size = new Size(0x312, 0x26);
            base.ttDefault.SetSuperTip(this.pnlLobbyParticipant1, null);
            this.pnlLobbyParticipant1.TabIndex = 11;
            this.pnlLobbyParticipant2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlLobbyParticipant2.BackColor = Color.Transparent;
            this.pnlLobbyParticipant2.Game = null;
            this.pnlLobbyParticipant2.IsOpen = true;
            this.pnlLobbyParticipant2.Location = new Point(11, 120);
            this.pnlLobbyParticipant2.Name = "pnlLobbyParticipant2";
            this.pnlLobbyParticipant2.Postition = -1;
            this.pnlLobbyParticipant2.Size = new Size(0x312, 0x26);
            base.ttDefault.SetSuperTip(this.pnlLobbyParticipant2, null);
            this.pnlLobbyParticipant2.TabIndex = 12;
            this.pnlLobbyParticipant3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlLobbyParticipant3.BackColor = Color.Transparent;
            this.pnlLobbyParticipant3.Game = null;
            this.pnlLobbyParticipant3.IsOpen = true;
            this.pnlLobbyParticipant3.Location = new Point(11, 0xa4);
            this.pnlLobbyParticipant3.Name = "pnlLobbyParticipant3";
            this.pnlLobbyParticipant3.Postition = -1;
            this.pnlLobbyParticipant3.Size = new Size(0x312, 0x26);
            base.ttDefault.SetSuperTip(this.pnlLobbyParticipant3, null);
            this.pnlLobbyParticipant3.TabIndex = 13;
            this.pnlLobbyParticipant4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pnlLobbyParticipant4.BackColor = Color.Transparent;
            this.pnlLobbyParticipant4.Game = null;
            this.pnlLobbyParticipant4.IsOpen = true;
            this.pnlLobbyParticipant4.Location = new Point(11, 0xd0);
            this.pnlLobbyParticipant4.Name = "pnlLobbyParticipant4";
            this.pnlLobbyParticipant4.Postition = -1;
            this.pnlLobbyParticipant4.Size = new Size(0x312, 0x26);
            base.ttDefault.SetSuperTip(this.pnlLobbyParticipant4, null);
            this.pnlLobbyParticipant4.TabIndex = 14;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x32e, 0x22f);
            base.Controls.Add(this.pnlLobbyParticipant4);
            base.Controls.Add(this.pnlLobbyParticipant3);
            base.Controls.Add(this.pnlLobbyParticipant2);
            base.Controls.Add(this.pnlLobbyParticipant1);
            base.Controls.Add(this.chatPanel);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x32e, 0x22f);
            base.Name = "DlgGameLobby";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Space Siege Game Lobby";
            base.AfterShown += new BaseFormEvent(this.DlgGameLobby_AfterShown);
            base.Controls.SetChildIndex(this.chatPanel, 0);
            base.Controls.SetChildIndex(this.pnlLobbyParticipant1, 0);
            base.Controls.SetChildIndex(this.pnlLobbyParticipant2, 0);
            base.Controls.SetChildIndex(this.pnlLobbyParticipant3, 0);
            base.Controls.SetChildIndex(this.pnlLobbyParticipant4, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static DlgGameLobby JoinGame(SpaceSiegeGame game)
        {
            DlgGameLobby lobby = new DlgGameLobby(game);
            lobby.chatPanel.gpgChatGrid.Refresh();
            return lobby;
        }

        private void KickParticipant(SpaceSiegeGameParticipant participant)
        {
            this.SendMessage(SpaceSiegeLobbyMessages.Kick, new object[] { participant.Name });
            this.RemoveParticipant(participant.Name);
            this.chatPanel.SystemMessage("<LOC>You have kicked {0} from the game lobby.", new object[] { participant.Name });
        }

        private void LeaveLobby()
        {
            this.SendMessage(SpaceSiegeLobbyMessages.Leave, new object[0]);
            this.chatPanel.SystemEvent("<LOC>You have left the game lobby.", new object[0]);
            base.MainForm.DestroyGameLobby();
            base.MainForm.SystemMessage("<LOC>You have left the game lobby.", new object[0]);
        }

        private void MainForm_GameHosted(object sender, EventArgs e)
        {
            base.MainForm.GameHosted -= new EventHandler(this.MainForm_GameHosted);
            this.SendMessage(this.Game.GetOtherMemberNames(), SpaceSiegeLobbyMessages.Launch, new object[0]);
            this.OverrideClose = true;
            base.MainForm.DestroyGameLobby();
        }

        private void OnAcceptInvite(string senderName, int senderId)
        {
            if (this.Game.AvailablePositions < 1)
            {
                this.SendMessage(senderName, SpaceSiegeLobbyMessages.Full, new object[0]);
            }
            else
            {
                SpaceSiegeGameParticipant item = new SpaceSiegeGameParticipant(this.Game, senderName, senderId);
                item.Position = this.Game.NextAvailablePosition();
                this.SendMessage(SpaceSiegeLobbyMessages.ParticipantInfo, new object[] { item.ToDataString() });
                this.Game.Members.Add(item);
                this.ParticipantPanels[item.Position].SetParticipant(item);
                this.SendMessage(senderName, SpaceSiegeLobbyMessages.Game, new object[] { this.Game.ToDataString() });
                this.chatPanel.SystemEvent("{0} has joined the game lobby.", new object[] { senderName });
                this.UpdateLaunchAbility();
                this.UpdateHostedGameRecord();
            }
        }

        private void OnChatMessage(string name, string chatMessage)
        {
            if (this.Game.FindMember(name) == null)
            {
                if (name == User.System.Name)
                {
                    this.chatPanel.SystemMessage(chatMessage, new object[0]);
                }
                else if (name == User.Event.Name)
                {
                    this.chatPanel.SystemEvent(chatMessage, new object[0]);
                }
                else if (name == User.Error.Name)
                {
                    this.chatPanel.ErrorMessage(chatMessage, new object[0]);
                }
            }
            else
            {
                this.chatPanel.AddChat(name, chatMessage);
            }
            this.chatPanel.gpgChatGrid.Refresh();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.RemoveGameFromList();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if ((!this.OverrideClose && (this.Game != null)) && (this.Game.Members.Count > 1))
            {
                if (this.Game.GetSelf().IsHost && (this.Game.Members.Count > 1))
                {
                    if (new DlgYesNo(base.MainForm, "<LOC>Confirm", "<LOC>Do you really want to disband this game lobby?").ShowDialog() == DialogResult.Yes)
                    {
                        this.DisbandLobby();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                this.SendMessage(SpaceSiegeLobbyMessages.Leave, new object[0]);
            }
            if (!e.Cancel)
            {
                base.MainForm.EnableGameButtons();
            }
            base.OnClosing(e);
        }

        private void OnDisband()
        {
            this.chatPanel.SystemEvent("<LOC>The game lobby has been disbanded.", new object[0]);
            base.MainForm.DestroyGameLobby();
            base.MainForm.SystemMessage("<LOC>The game lobby has been disbanded.", new object[0]);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RefreshDropDownItems();
        }

        private void OnMemberLeave(string name)
        {
            this.chatPanel.SystemEvent("<LOC>{0} has left the game lobby.", new object[] { name });
            this.RemoveParticipant(name);
        }

        private void OnRecieveKick(string name)
        {
            if (name == this.Game.GetSelf().Name)
            {
                this.chatPanel.SystemEvent("<LOC>You have been kicked out of the game lobby.", new object[0]);
                base.MainForm.DestroyGameLobby();
                base.MainForm.SystemMessage("<LOC>You have been kicked out of the game lobby.", new object[0]);
            }
            else
            {
                this.chatPanel.SystemEvent("<LOC>{0} has been kicked out of the game lobby.", new object[] { name });
                this.RemoveParticipant(name);
            }
        }

        private void OnRecieveParticipant(SpaceSiegeGameParticipant participant)
        {
            SpaceSiegeGameParticipant participant2 = this.Game.FindMember(participant.Name) as SpaceSiegeGameParticipant;
            if (participant2 == null)
            {
                this.Game.Members.Add(participant);
                this.ParticipantPanels[participant.Position].SetParticipant(participant);
                this.chatPanel.SystemEvent("{0} has joined the game lobby.", new object[] { participant.Name });
                this.UpdateLaunchAbility();
                this.UpdateHostedGameRecord();
            }
            else
            {
                participant2.MergeParticipants(participant);
                this.ParticipantPanels[participant2.Position].SetParticipant(participant2);
                this.UpdateHostedGameRecord();
                this.UpdateLaunchAbility();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.chatPanel.SystemMessage("<LOC>You have joined the game lobby", new object[0]);
        }

        private void OpenPosition(int position)
        {
            this.Game.PositionStates[position] = GamePositionStates.Open;
            this.ParticipantPanels[position].IsOpen = true;
            this.ParticipantPanels[position].SetParticipant(null);
            this.UpdateHostedGameRecord();
        }

        public void RecieveMessage(string senderName, int senderId, uint messageType, string[] args)
        {
            try
            {
                string str;
                string[] strArray;
                int num;
                int num3;
                switch (((SpaceSiegeLobbyMessages) messageType))
                {
                    case SpaceSiegeLobbyMessages.Kick:
                    {
                        string name = args[0];
                        this.OnRecieveKick(name);
                        return;
                    }
                    case SpaceSiegeLobbyMessages.Disband:
                        this.OnDisband();
                        return;

                    case SpaceSiegeLobbyMessages.Leave:
                        this.OnMemberLeave(senderName);
                        return;

                    case SpaceSiegeLobbyMessages.ParticipantInfo:
                    {
                        SpaceSiegeGameParticipant participant = GPGnetGameParticipant.FromDataString(this.Game, args[0]) as SpaceSiegeGameParticipant;
                        this.OnRecieveParticipant(participant);
                        return;
                    }
                    case SpaceSiegeLobbyMessages.Launch:
                        EventLog.WriteLine("Received launch command from host", new object[0]);
                        this.chatPanel.SystemEvent("<LOC>The host has launched the game.", new object[0]);
                        this.Game.HasLaunched = true;
                        this.Game.GetSelf().IsReady = false;
                        this.ParticipantPanels[this.Game.GetSelf().Position].SetParticipant(this.Game.GetSelf() as SpaceSiegeGameParticipant);
                        GameInformation.SelectedGame.UserForcedCommandLineArgs = string.Format("character={0} players={1}", (this.Game.GetSelf() as SpaceSiegeGameParticipant).Character.CharacterName, this.Game.Members.Count);
                        GPGnetSelectedGame.ProfileName = (this.Game.GetSelf() as SpaceSiegeGameParticipant).Character.CharacterName;
                        base.MainForm.JoinGame(this.Game.ID);
                        this.OverrideClose = true;
                        base.MainForm.DestroyGameLobby();
                        return;

                    case SpaceSiegeLobbyMessages.AcceptInvite:
                        this.OnAcceptInvite(senderName, senderId);
                        return;

                    case SpaceSiegeLobbyMessages.DeclineInvite:
                        str = args[0];
                        if (args.Length <= 2)
                        {
                            goto Label_0113;
                        }
                        strArray = new string[args.Length - 2];
                        num = 2;
                        goto Label_00FD;

                    case SpaceSiegeLobbyMessages.ChangePosition:
                    {
                        int newPosition = int.Parse(args[0]);
                        this.ChangePosition(this.Game.FindMember(senderName) as SpaceSiegeGameParticipant, newPosition);
                        return;
                    }
                    case SpaceSiegeLobbyMessages.OpenPosition:
                        num3 = int.Parse(args[0]);
                        this.OpenPosition(num3);
                        return;

                    case SpaceSiegeLobbyMessages.ClosePosition:
                        num3 = int.Parse(args[0]);
                        this.ClosePosition(num3);
                        return;

                    case SpaceSiegeLobbyMessages.ChatMessage:
                        str = args[0];
                        if (args.Length > 2)
                        {
                            strArray = new string[args.Length - 2];
                            num = 2;
                            while (num < args.Length)
                            {
                                strArray[num - 2] = args[num];
                                num++;
                            }
                            str = string.Format(str, (object[]) strArray);
                        }
                        this.OnChatMessage(senderName, str);
                        return;

                    default:
                        goto Label_02E4;
                }
            Label_00EE:
                strArray[num - 2] = args[num];
                num++;
            Label_00FD:
                if (num < args.Length)
                {
                    goto Label_00EE;
                }
                str = string.Format(str, (object[]) strArray);
            Label_0113:
                this.chatPanel.SystemMessage(str, new object[0]);
                return;
            Label_02E4:;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void RefreshDropDownItems()
        {
            foreach (PnlLobbyParticipant participant in this.ParticipantPanels)
            {
                if (this.Game.PositionStates[participant.Postition] == GamePositionStates.Open)
                {
                    participant.IsOpen = true;
                }
                else
                {
                    participant.IsOpen = false;
                }
                participant.SetParticipant(this.Game.FindMemberAtPosition(participant.Postition) as SpaceSiegeGameParticipant);
                participant.Refresh();
            }
        }

        private void RefreshParticipant()
        {
            this.SendMessage(SpaceSiegeLobbyMessages.ParticipantInfo, new object[] { this.Game.GetSelf().ToDataString() });
        }

        private void RemoveGameFromList()
        {
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                new QuazalQuery("RemoveSpaceSiegeGame", new object[0]).ExecuteNonQuery();
            }, new object[0]);
        }

        private void RemoveParticipant(string name)
        {
            SpaceSiegeGameParticipant item = this.Game.FindMember(name) as SpaceSiegeGameParticipant;
            this.ParticipantPanels[item.Position].SetParticipant(null);
            this.Game.Members.Remove(item);
            this.UpdateLaunchAbility();
            this.UpdateHostedGameRecord();
        }

        public void SendMessage(SpaceSiegeLobbyMessages message, params object[] args)
        {
            this.SendMessage(this.Game.GetAllMemberNames(), message, args);
        }

        public void SendMessage(string recipient, SpaceSiegeLobbyMessages message, params object[] args)
        {
            this.SendMessage(new string[] { recipient }, message, args);
        }

        public void SendMessage(string[] recipients, SpaceSiegeLobbyMessages message, params object[] args)
        {
            List<object> list = new List<object>();
            list.Add((uint) message);
            if (list != null)
            {
                list.AddRange(args);
            }
            Messaging.SendCustomCommand(recipients, CustomCommands.GameLobbyMessage, list.ToArray());
            this.chatPanel.gpgChatGrid.Refresh();
        }

        internal void UpdateHostedGameRecord()
        {
            ThreadPool.QueueUserWorkItem(delegate (object state) {
                try
                {
                    if (this.Game.GetSelf().IsHost)
                    {
                        int num = 0;
                        foreach (GamePositionStates states in this.Game.PositionStates)
                        {
                            if (states == GamePositionStates.Open)
                            {
                                num++;
                            }
                        }
                        int num2 = 0;
                        int num3 = 0;
                        foreach (SpaceSiegeGameParticipant participant in this.Game.Members)
                        {
                            if (participant.Character != null)
                            {
                                num2 += participant.Character.Upgrades;
                                num3++;
                            }
                        }
                        int num4 = 0;
                        if (num3 > 0)
                        {
                            num4 = num2 / num3;
                        }
                        new QuazalQuery("UpdateSpaceSiegeGame", new object[] { this.Game.Members.Count, num, num4 }).ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        private void UpdateLaunchAbility()
        {
            if (this.Game.Host.IsSelf)
            {
                this.Game.CanLaunch = true;
                foreach (SpaceSiegeGameParticipant participant in this.Game.Members)
                {
                    if (!participant.IsReady)
                    {
                        this.Game.CanLaunch = false;
                        break;
                    }
                }
                this.ParticipantPanels[this.Game.Host.Position].SetParticipant(this.ParticipantPanels[this.Game.Host.Position].Participant);
            }
        }

        public SpaceSiegeGame Game
        {
            get
            {
                return this.mGame;
            }
        }

        private PnlLobbyParticipant[] ParticipantPanels
        {
            get
            {
                return new PnlLobbyParticipant[] { this.pnlLobbyParticipant1, this.pnlLobbyParticipant2, this.pnlLobbyParticipant3, this.pnlLobbyParticipant4 };
            }
        }
    }
}

