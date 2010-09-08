namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using DevExpress.Utils;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgManageTournament : DlgBase
    {
        private ToolStripMenuItem assignDrawToolStripMenuItem;
        private ToolStripMenuItem assignWinToolStripMenuItem;
        public SkinButton btnAutoPair;
        public SkinButton btnBeginNoLaunch;
        public SkinButton btnBroadcastPairings;
        public SkinButton btnCustomRoom;
        public SkinButton btnEject;
        public SkinButton btnEjectOffline;
        public SkinButton btnInject;
        public SkinButton btnLaunchChat;
        public SkinButton btnLocatePlayer;
        public SkinButton btnRandomRecs;
        public SkinButton btnRandomResults;
        public SkinButton btnResetScores;
        public SkinButton btnRound;
        public SkinButton btnSetNoResult;
        public SkinButton btnStandings;
        public SkinButton btnTournament;
        private GPGDropDownList cbMap;
        private ContextMenuStrip cmsGridOptions;
        private IContainer components = null;
        public static DlgManageTournament Current = null;
        private GPGDragList dragPlayers;
        private ToolStripMenuItem forcefullyEndGameToolStripMenuItem;
        private GPGBorderPanel gpgBorderPanel2;
        private GPGLabel gpgLabel7;
        private ToolStripMenuItem launchGameToolStripMenuItem;
        private ToolStripMenuItem makePlayer2WinnerToolStripMenuItem;
        private ToolStripMenuItem manuallSetMatchupRecordsToolStripMenuItem;
        private DataRecord mDetails = null;
        private bool mIgnoreReset = false;
        private int mRound = 1;
        private int mTournamentID = 0;
        private ToolStripMenuItem resetRoundResultsToolStripMenuItem;
        public SkinButton skinButton1;
        public SkinButton skinButtonHonorPoints;
        private SkinLabel skinLabel1;
        private SkinLabel skinLabel2;
        private bool TournamentOver = false;

        public DlgManageTournament()
        {
            this.InitializeComponent();
            Current = this;
            this.btnAutoPair.Enabled = false;
            this.btnRound.Enabled = false;
            this.btnEject.Enabled = false;
            this.btnTournament.Enabled = false;
            if (User.Current.Name.ToUpper() != "IN-AGENT911")
            {
                this.btnRandomResults.Visible = false;
                this.btnRandomRecs.Visible = false;
                this.btnResetScores.Visible = false;
                this.btnSetNoResult.Visible = false;
            }
            foreach (SupcomMap map in SupcomMapList.Maps)
            {
                this.cbMap.Items.Add(map);
            }
        }

        private void BackoutResults(GPGDragItem player1, GPGDragItem player2)
        {
            if (player1 != null)
            {
                if (player1.Won == 1)
                {
                    player1.Wins--;
                }
                if (player1.Won == 0)
                {
                    player1.Losses--;
                }
                if (player1.Won == 2)
                {
                    player1.Draws--;
                }
                player1.Won = -1;
                player1.PlayerReport = "Backed out result";
            }
            if (player2 != null)
            {
                if (player2.Won == 1)
                {
                    player2.Wins--;
                }
                if (player2.Won == 0)
                {
                    player2.Losses--;
                }
                if (player2.Won == 2)
                {
                    player2.Draws--;
                }
                player2.Won = -1;
                player2.PlayerReport = "Backed out result";
            }
        }

        private void btnAutoPair_Click(object sender, EventArgs e)
        {
            int num2;
            this.dragPlayers.Items.Sort();
            Dictionary<float, List<GPGDragItem>> dictionary = new Dictionary<float, List<GPGDragItem>>();
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                if (dictionary.ContainsKey(item.FractionalWins))
                {
                    dictionary[item.FractionalWins].Add(item);
                }
                else
                {
                    dictionary.Add(item.FractionalWins, new List<GPGDragItem>());
                    dictionary[item.FractionalWins].Add(item);
                }
            }
            List<GPGDragItem> list = new List<GPGDragItem>();
            foreach (KeyValuePair<float, List<GPGDragItem>> pair in dictionary)
            {
                int num = (int) Math.Floor((double) (((double) pair.Value.Count) / 2.0));
                num2 = 0;
                while (num2 < num)
                {
                    list.Add(pair.Value[num2]);
                    if ((num2 + num) < pair.Value.Count)
                    {
                        list.Add(pair.Value[num2 + num]);
                    }
                    num2++;
                }
                if ((pair.Value.Count % 2) == 1)
                {
                    list.Add(pair.Value[pair.Value.Count - 1]);
                }
            }
            bool @bool = ConfigSettings.GetBool("CheckDupes2", false);
            int num3 = 0;
            int index = 0;
            try
            {
                while (@bool && (num3 < 500))
                {
                    num3++;
                    @bool = false;
                    index = list.Count - 1;
                    num2 = 0;
                    while (num2 < this.dragPlayers.Matchups.Count)
                    {
                        GPGDragItem item2;
                        GPGDragItem item3;
                        if (index >= 0)
                        {
                            item2 = list[index];
                            index--;
                        }
                        else
                        {
                            goto Label_0324;
                        }
                        if (index >= 0)
                        {
                            item3 = list[index];
                            index--;
                        }
                        else
                        {
                            goto Label_0324;
                        }
                        foreach (GPGMatchup matchup in this.dragPlayers.DupeMatchups)
                        {
                            if ((matchup.Player1 == null) || (matchup.Player2 == null))
                            {
                                continue;
                            }
                            if ((((matchup.Player1.PlayerName == item2.PlayerName) && (matchup.Player2.PlayerName == item3.PlayerName)) || ((matchup.Player1.PlayerName == item3.PlayerName) && (matchup.Player2.PlayerName == item2.PlayerName))) && (index >= 0))
                            {
                                list.Reverse(index, 2);
                                @bool = true;
                            }
                        }
                    Label_0324:
                        num2++;
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            index = list.Count - 1;
            for (num2 = 0; num2 < this.dragPlayers.Matchups.Count; num2++)
            {
                if (index >= 0)
                {
                    this.dragPlayers.Matchups[num2].Player1 = list[index];
                    list[index].IsMatched = true;
                    index--;
                }
                if (index >= 0)
                {
                    this.dragPlayers.Matchups[num2].Player2 = list[index];
                    list[index].IsMatched = true;
                    index--;
                }
            }
            this.dragPlayers.Invalidate();
        }

        private void btnBeginNoLaunch_Click(object sender, EventArgs e)
        {
            if (this.btnRound.ButtonState == 0)
            {
                this.mDetails.SetValue("status", "Playing Round " + this.Round.ToString());
                DataAccess.QueueExecuteQuery("Tournament Status", new object[] { "Playing Round", this.Round, this.mTournamentID });
                this.UpdateBrackets();
                this.SetButtonState();
            }
            else
            {
                DlgMessage.ShowDialog("You are not yet in a state where you can begin games.  The round has to be begun first.");
            }
        }

        private void btnBroadcastPairings_Click(object sender, EventArgs e)
        {
            foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
            {
                if ((matchup.Player1 != null) && (matchup.Player2 != null))
                {
                    base.MainForm.SystemMessage(matchup.Player1.LabelNoReport + " vs. " + matchup.Player2.LabelNoReport, new object[0]);
                    Messaging.SendGathering(matchup.Player1.LabelNoReport + " vs. " + matchup.Player2.LabelNoReport);
                    if (!User.Current.IsAdmin)
                    {
                        Thread.Sleep(ConfigSettings.GetInt("TDMessageSleep", 500));
                    }
                }
            }
        }

        private void btnCustomRoom_Click(object sender, EventArgs e)
        {
            string channel = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>What custom chatroom would you like for this tournament?"));
            if (channel != "")
            {
                base.MainForm.LeaveChat();
                base.MainForm.CreateChannelIfNonExist(channel);
                base.MainForm.JoinChat(channel);
                foreach (GPGDragItem item in this.dragPlayers.Items)
                {
                    DataRecord record = item.Value as DataRecord;
                    ThreadQueue.QueueUserWorkItem(delegate (object o) {
                        string recipient = (o as object[])[0].ToString();
                        string str2 = (o as object[])[1].ToString();
                        Messaging.SendCustomCommand(recipient, CustomCommands.TournamentChatroom, new object[] { str2 });
                    }, new object[] { record["name"], channel });
                }
            }
        }

        private void btnEject_Click(object sender, EventArgs e)
        {
            string name = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>Who would you like to eject?"));
            this.EjectPlayer(name);
        }

        private void btnEjectOffline_Click(object sender, EventArgs e)
        {
            DataList queryData = DataAccess.GetQueryData("Tournament Online Status", new object[] { this.mTournamentID });
            foreach (DataRecord record in queryData)
            {
                if (record["connected"] == "0")
                {
                    DlgYesNo no = new DlgYesNo(base.MainForm, Loc.Get("<LOC>Eject"), Loc.Get("<LOC>Would you like to eject this offline player from the tournament?") + "\r\n" + record["name"]);
                    this.EjectPlayer(record["name"]);
                }
            }
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            string name = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>Who would you like to add to the tournament?"));
            if (name != "")
            {
                this.InjectPlayer(name);
            }
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
        }

        private void btnLaunchChat_Click(object sender, EventArgs e)
        {
            DlgYesNo no = new DlgYesNo(base.MainForm, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>This will launch everybody who is still in your chatroom.  You should only do so if you have already hit the main Launch Games in a round.  Continue?"));
            if (no.ShowDialog() == DialogResult.Yes)
            {
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    int num = 0;
                    foreach (User user in Chatroom.GatheringParticipants)
                    {
                        if (user.Name.ToUpper() == matchup.Player1.PlayerName.ToUpper())
                        {
                            num++;
                        }
                    }
                    if (num == 2)
                    {
                        this.LaunchSelectedGame(matchup);
                    }
                }
            }
        }

        private void btnLocatePlayer_Click(object sender, EventArgs e)
        {
            this.dragPlayers.HighlightPlayer = DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>Enter the player you wish to find.  They will appear in green."));
        }

        private void btnRandomRecs_Click(object sender, EventArgs e)
        {
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                Thread.Sleep(10);
                item.RandomizeRecord();
            }
            this.dragPlayers.Invalidate();
        }

        private void btnRandomResults_Click(object sender, EventArgs e)
        {
            this.RandomResults();
        }

        private void btnResetScores_Click(object sender, EventArgs e)
        {
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                item.Wins = 0;
                item.Losses = 0;
                item.Draws = 0;
            }
            this.dragPlayers.Invalidate();
        }

        private void btnRound_Click(object sender, EventArgs e)
        {
            if (this.btnRound.ButtonState == 0)
            {
                this.mDetails.SetValue("status", "Playing Round " + this.Round.ToString());
                DataAccess.QueueExecuteQuery("Tournament Status", new object[] { "Playing Round", this.Round, this.mTournamentID });
                this.LaunchGames();
                this.UpdateBrackets();
            }
            else if (this.btnRound.ButtonState == 2)
            {
                this.mDetails.SetValue("status", "Setup Round " + this.Round.ToString());
                DataAccess.QueueExecuteQuery("Tournament Status", new object[] { "Setup Round", this.Round, this.mTournamentID });
                foreach (GPGDragItem item in this.dragPlayers.Items)
                {
                    item.PlayerReport = "No Report";
                    item.Won = -1;
                }
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    if (matchup.Player1 != null)
                    {
                        matchup.Player1.IsMatched = false;
                        matchup.Player1 = null;
                    }
                    if (matchup.Player2 != null)
                    {
                        matchup.Player2.IsMatched = false;
                        matchup.Player2 = null;
                    }
                }
                this.dragPlayers.Invalidate();
                this.UpdateBrackets();
            }
            else
            {
                int num = 0;
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    if ((matchup.Player1 != null) && (matchup.Player2 != null))
                    {
                        num++;
                        if (((matchup.Player1.PlayerReport == "NoReport") || (matchup.Player1.PlayerReport == "Playing")) && ((matchup.Player2.PlayerReport == "NoReport") || (matchup.Player2.PlayerReport == "Playing")))
                        {
                            DlgMessage.ShowDialog(base.MainForm, matchup.Player1.PlayerName + " vs " + matchup.Player2.PlayerName + " needs to be resolved before you can end the round.");
                            return;
                        }
                    }
                }
                if (num == 0)
                {
                    DlgYesNo no = new DlgYesNo(base.MainForm, Loc.Get("<LOC>DANGER"), Loc.Get("<LOC>You have no matchups.  Ending the tournament round may corrupt your tournament.  Are you sure you wish to continue?"));
                    if (no.ShowDialog() == DialogResult.No)
                    {
                        return;
                    }
                }
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    if ((matchup.Player1 != null) && (matchup.Player2 != null))
                    {
                        if ((matchup.Player1.Won == 0) && (matchup.Player2.Won == -1))
                        {
                            matchup.Player2.Won = 1;
                            GPGDragItem item1 = matchup.Player2;
                            item1.Wins++;
                            base.MainForm.SystemMessage(matchup.Player2.Label + " did not report and was assigned a win.", new object[0]);
                            Messaging.SendGathering(matchup.Player2.Label + " did not report and was assigned a win.");
                        }
                        if ((matchup.Player1.Won == 1) && (matchup.Player2.Won == -1))
                        {
                            matchup.Player2.Won = 0;
                            GPGDragItem item2 = matchup.Player2;
                            item2.Losses++;
                            base.MainForm.SystemMessage(matchup.Player2.Label + " did not report and was assigned a loss.", new object[0]);
                            Messaging.SendGathering(matchup.Player2.Label + " did not report and was assigned a loss.");
                        }
                        if ((matchup.Player1.Won == 2) && (matchup.Player2.Won == -1))
                        {
                            matchup.Player2.Won = 2;
                            GPGDragItem item3 = matchup.Player2;
                            item3.Draws++;
                            base.MainForm.SystemMessage(matchup.Player2.Label + " did not report and was assigned a draw.", new object[0]);
                            Messaging.SendGathering(matchup.Player2.Label + " did not report and was assigned a draw.");
                        }
                        if ((matchup.Player2.Won == 0) && (matchup.Player1.Won == -1))
                        {
                            matchup.Player1.Won = 1;
                            GPGDragItem item4 = matchup.Player1;
                            item4.Wins++;
                            base.MainForm.SystemMessage(matchup.Player1.Label + " did not report and was assigned a win.", new object[0]);
                            Messaging.SendGathering(matchup.Player1.Label + " did not report and was assigned a win.");
                        }
                        if ((matchup.Player2.Won == 1) && (matchup.Player1.Won == -1))
                        {
                            matchup.Player1.Won = 0;
                            GPGDragItem item5 = matchup.Player1;
                            item5.Losses++;
                            base.MainForm.SystemMessage(matchup.Player1.Label + " did not report and was assigned a loss.", new object[0]);
                            Messaging.SendGathering(matchup.Player1.Label + " did not report and was assigned a loss.");
                        }
                        if ((matchup.Player2.Won == 2) && (matchup.Player1.Won == -1))
                        {
                            matchup.Player1.Won = 2;
                            GPGDragItem item6 = matchup.Player1;
                            item6.Draws++;
                            base.MainForm.SystemMessage(matchup.Player1.Label + " did not report and was assigned a draw.", new object[0]);
                            Messaging.SendGathering(matchup.Player1.Label + " did not report and was assigned a draw.");
                        }
                    }
                }
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    if ((matchup.Player1 != null) && (((matchup.Player1.Wins + matchup.Player1.Losses) + matchup.Player1.Draws) != this.Round))
                    {
                        DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>Player " + matchup.Player1.PlayerName + " needs to be resolved before you can end the round."));
                        return;
                    }
                    if ((matchup.Player2 != null) && (((matchup.Player2.Wins + matchup.Player2.Losses) + matchup.Player2.Draws) != this.Round))
                    {
                        DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>Player " + matchup.Player2.PlayerName + " needs to be resolved before you can end the round."));
                        return;
                    }
                }
                DataAccess.QueueExecuteQuery("Tournament Status", new object[] { "Completed Round", this.Round, this.mTournamentID });
                this.mDetails.SetValue("status", "Completed Round " + this.Round.ToString());
                this.UpdateBrackets();
                this.Round++;
            }
            this.SetButtonState();
        }

        private void btnSetNoResult_Click(object sender, EventArgs e)
        {
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                item.PlayerReport = "No Report";
            }
            this.dragPlayers.Invalidate();
        }

        private void btnStandings_Click(object sender, EventArgs e)
        {
            DataList queryData = DataAccess.GetQueryData("Tournament Participation", new object[] { this.mTournamentID });
            base.MainForm.SystemMessage("Here are the tournament standings.", new object[0]);
            Messaging.SendGathering("Here are the tournament standings.");
            this.dragPlayers.Items.Sort();
            for (int i = this.dragPlayers.Items.Count - 1; i >= 0; i--)
            {
                GPGDragItem item = this.dragPlayers.Items[i];
                string str = "None";
                foreach (DataRecord record in queryData)
                {
                    if (record["name"] == item.PlayerName)
                    {
                        try
                        {
                            str = ((int) Convert.ToDouble(record["rating"])).ToString();
                        }
                        catch
                        {
                        }
                    }
                }
                if (str.ToUpper() == "(NULL)")
                {
                    str = "No Rating";
                }
                int num2 = this.dragPlayers.Items.Count - i;
                base.MainForm.SystemMessage("Current Position: #" + num2.ToString() + " Original Seed: " + item.LabelNoReport + " Current Rating: " + str + string.Format(" ({0})", item.HonorPoints), new object[0]);
                Messaging.SendGathering("Current Position: #" + num2.ToString() + " Original Seed: " + item.LabelNoReport + " Current Rating: " + str + string.Format(" ({0})", item.HonorPoints));
                if (!User.Current.IsAdmin)
                {
                    Thread.Sleep(ConfigSettings.GetInt("TDMessageSleep", 500));
                }
                num2++;
            }
        }

        private void btnTournament_Click(object sender, EventArgs e)
        {
            if (this.btnTournament.ButtonState == 1)
            {
                this.EndTournament();
            }
            else
            {
                this.mDetails.SetValue("status", "Setup Round " + this.Round.ToString());
                DataAccess.QueueExecuteQuery("Tournament Status", new object[] { "Setup Round", this.Round, this.mTournamentID });
                this.SetButtonState();
            }
        }

        private void CheckBadLaunches()
        {
            foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
            {
                if ((matchup.Player1 == null) || (matchup.Player2 == null))
                {
                    continue;
                }
                if ((matchup.Player1.PlayerReport == "victory") && (matchup.Player2.PlayerReport == "victory"))
                {
                    GPGDragItem item1 = matchup.Player1;
                    item1.Wins--;
                    GPGDragItem item2 = matchup.Player2;
                    item2.Wins--;
                    matchup.Player1.PlayerReport = "Disconnect";
                    matchup.Player2.PlayerReport = "Disconnect";
                    break;
                }
                if ((matchup.Player1.PlayerReport == "Playing") && (matchup.Player2.PlayerReport == "Playing"))
                {
                    if (ConfigSettings.GetBool("BackoutDrawsOnFailure", false))
                    {
                        GPGDragItem item3 = matchup.Player1;
                        item3.Draws--;
                        GPGDragItem item4 = matchup.Player2;
                        item4.Draws--;
                    }
                    if (ConfigSettings.GetBool("BackoutLossesOnFailure", true))
                    {
                        GPGDragItem item5 = matchup.Player1;
                        item5.Losses--;
                        GPGDragItem item6 = matchup.Player2;
                        item6.Losses--;
                    }
                    matchup.Player1.PlayerReport = "Failed Game";
                    matchup.Player2.PlayerReport = "Failed Game";
                    break;
                }
            }
        }

        private void clearResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.dragPlayers.SelectedMatchup != null) && ((this.dragPlayers.SelectedMatchup.Player1 != null) && (this.dragPlayers.SelectedMatchup.Player2 != null)))
                {
                    GPGDragItem item = this.dragPlayers.SelectedMatchup.Player1;
                    GPGDragItem item2 = this.dragPlayers.SelectedMatchup.Player2;
                    this.BackoutResults(item, item2);
                }
            }
            catch
            {
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

        private void DlgManageTournament_Load(object sender, EventArgs e)
        {
        }

        private void EjectPlayer(string name)
        {
            GPGDragItem item = null;
            foreach (GPGDragItem item2 in this.dragPlayers.Items)
            {
                if (item2.PlayerName.ToUpper() == name.ToUpper())
                {
                    item = item2;
                    break;
                }
            }
            if (item != null)
            {
                DataAccess.QueueExecuteQuery("Tournament Eject Player", new object[] { this.mTournamentID, name });
                this.dragPlayers.Items.Remove(item);
                this.dragPlayers.Invalidate();
            }
            else
            {
                DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>Unable to find this player in the tournament."));
            }
        }

        private void EndTournament()
        {
            if (!this.TournamentOver)
            {
                for (int i = this.dragPlayers.Items.Count - 1; i >= 0; i--)
                {
                    GPGDragItem item = this.dragPlayers.Items[i];
                    if (item.HonorPoints < 10)
                    {
                        item.HonorPoints++;
                    }
                }
                if (new QuazalQuery("HonorFinalRoundParticipants", new object[] { this.mTournamentID, this.Round }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("<LOC>This tournament has ended and honor points have been adjusted for participants who completed it.");
                }
                else
                {
                    DlgMessage.ShowDialog("<LOC>Error updating honor points for final round participants.", "<LOC>Error");
                }
                this.btnTournament.Enabled = false;
                this.TournamentOver = true;
            }
        }

        private void forcefullyEndGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgYesNo no = new DlgYesNo(base.MainForm, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>Are you sure you want to end this game?"));
            if ((no.ShowDialog() == DialogResult.Yes) && (this.dragPlayers.SelectedMatchup != null))
            {
                if (this.dragPlayers.SelectedMatchup.Player1 != null)
                {
                    Messaging.SendCustomCommand(this.dragPlayers.SelectedMatchup.Player1.PlayerName, CustomCommands.TournamentEndMatch, new object[0]);
                }
                if (this.dragPlayers.SelectedMatchup.Player2 != null)
                {
                    Messaging.SendCustomCommand(this.dragPlayers.SelectedMatchup.Player2.PlayerName, CustomCommands.TournamentEndMatch, new object[0]);
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.gpgBorderPanel2 = new GPGBorderPanel();
            this.dragPlayers = new GPGDragList();
            this.cmsGridOptions = new ContextMenuStrip(this.components);
            this.assignWinToolStripMenuItem = new ToolStripMenuItem();
            this.makePlayer2WinnerToolStripMenuItem = new ToolStripMenuItem();
            this.assignDrawToolStripMenuItem = new ToolStripMenuItem();
            this.resetRoundResultsToolStripMenuItem = new ToolStripMenuItem();
            this.launchGameToolStripMenuItem = new ToolStripMenuItem();
            this.manuallSetMatchupRecordsToolStripMenuItem = new ToolStripMenuItem();
            this.forcefullyEndGameToolStripMenuItem = new ToolStripMenuItem();
            this.skinLabel2 = new SkinLabel();
            this.skinLabel1 = new SkinLabel();
            this.btnTournament = new SkinButton();
            this.btnEject = new SkinButton();
            this.btnRound = new SkinButton();
            this.btnAutoPair = new SkinButton();
            this.btnCustomRoom = new SkinButton();
            this.btnBroadcastPairings = new SkinButton();
            this.btnStandings = new SkinButton();
            this.btnEjectOffline = new SkinButton();
            this.btnBeginNoLaunch = new SkinButton();
            this.btnInject = new SkinButton();
            this.btnLocatePlayer = new SkinButton();
            this.btnLaunchChat = new SkinButton();
            this.btnResetScores = new SkinButton();
            this.btnSetNoResult = new SkinButton();
            this.btnRandomRecs = new SkinButton();
            this.btnRandomResults = new SkinButton();
            this.skinButton1 = new SkinButton();
            this.cbMap = new GPGDropDownList();
            this.gpgLabel7 = new GPGLabel();
            this.skinButtonHonorPoints = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgBorderPanel2.SuspendLayout();
            this.cmsGridOptions.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x424, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgBorderPanel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgBorderPanel2.Controls.Add(this.dragPlayers);
            this.gpgBorderPanel2.Controls.Add(this.skinLabel2);
            this.gpgBorderPanel2.Controls.Add(this.skinLabel1);
            this.gpgBorderPanel2.GPGBorderStyle = GPGBorderStyle.Rectangle;
            this.gpgBorderPanel2.Location = new Point(0x15, 0x53);
            this.gpgBorderPanel2.Name = "gpgBorderPanel2";
            this.gpgBorderPanel2.Padding = new Padding(2);
            this.gpgBorderPanel2.Size = new Size(0x437, 0x1c7);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel2, null);
            this.gpgBorderPanel2.TabIndex = 8;
            this.gpgBorderPanel2.Text = "gpgBorderPanel2";
            this.dragPlayers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dragPlayers.ContextMenuStrip = this.cmsGridOptions;
            this.dragPlayers.HighlightPlayer = "";
            this.dragPlayers.ItemHeight = 15;
            this.dragPlayers.ItemWidth = 200;
            this.dragPlayers.Location = new Point(2, 0x15);
            this.dragPlayers.Name = "dragPlayers";
            this.dragPlayers.Size = new Size(0x433, 0x1b0);
            base.ttDefault.SetSuperTip(this.dragPlayers, null);
            this.dragPlayers.TabIndex = 15;
            this.dragPlayers.Text = "gpgDragList1";
            this.cmsGridOptions.Items.AddRange(new ToolStripItem[] { this.assignWinToolStripMenuItem, this.makePlayer2WinnerToolStripMenuItem, this.assignDrawToolStripMenuItem, this.resetRoundResultsToolStripMenuItem, this.launchGameToolStripMenuItem, this.manuallSetMatchupRecordsToolStripMenuItem, this.forcefullyEndGameToolStripMenuItem });
            this.cmsGridOptions.Name = "cmsGridOptions";
            this.cmsGridOptions.Size = new Size(0xde, 0x9e);
            base.ttDefault.SetSuperTip(this.cmsGridOptions, null);
            this.assignWinToolStripMenuItem.Name = "assignWinToolStripMenuItem";
            this.assignWinToolStripMenuItem.Size = new Size(0xdd, 0x16);
            this.assignWinToolStripMenuItem.Text = "Make Player 1 Winner";
            this.assignWinToolStripMenuItem.Visible = false;
            this.assignWinToolStripMenuItem.Click += new EventHandler(this.setPlayer1WinnerToolStripMenuItem_Click);
            this.makePlayer2WinnerToolStripMenuItem.Name = "makePlayer2WinnerToolStripMenuItem";
            this.makePlayer2WinnerToolStripMenuItem.Size = new Size(0xdd, 0x16);
            this.makePlayer2WinnerToolStripMenuItem.Text = "Make Player 2 Winner";
            this.makePlayer2WinnerToolStripMenuItem.Visible = false;
            this.makePlayer2WinnerToolStripMenuItem.Click += new EventHandler(this.setPlayer2WinnerToolStripMenuItem_Click);
            this.assignDrawToolStripMenuItem.Name = "assignDrawToolStripMenuItem";
            this.assignDrawToolStripMenuItem.Size = new Size(0xdd, 0x16);
            this.assignDrawToolStripMenuItem.Text = "Assign Draw";
            this.assignDrawToolStripMenuItem.Visible = false;
            this.assignDrawToolStripMenuItem.Click += new EventHandler(this.setDrawToolStripMenuItem_Click);
            this.resetRoundResultsToolStripMenuItem.Name = "resetRoundResultsToolStripMenuItem";
            this.resetRoundResultsToolStripMenuItem.Size = new Size(0xdd, 0x16);
            this.resetRoundResultsToolStripMenuItem.Text = "Clear Round Results";
            this.resetRoundResultsToolStripMenuItem.Visible = false;
            this.resetRoundResultsToolStripMenuItem.Click += new EventHandler(this.clearResultToolStripMenuItem_Click);
            this.launchGameToolStripMenuItem.Name = "launchGameToolStripMenuItem";
            this.launchGameToolStripMenuItem.Size = new Size(0xdd, 0x16);
            this.launchGameToolStripMenuItem.Text = "Launch Game";
            this.launchGameToolStripMenuItem.Click += new EventHandler(this.launchGameToolStripMenuItem_Click);
            this.manuallSetMatchupRecordsToolStripMenuItem.Name = "manuallSetMatchupRecordsToolStripMenuItem";
            this.manuallSetMatchupRecordsToolStripMenuItem.Size = new Size(0xdd, 0x16);
            this.manuallSetMatchupRecordsToolStripMenuItem.Text = "Manually Set Matchup Records";
            this.manuallSetMatchupRecordsToolStripMenuItem.Click += new EventHandler(this.manuallSetMatchupRecordsToolStripMenuItem_Click);
            this.forcefullyEndGameToolStripMenuItem.Name = "forcefullyEndGameToolStripMenuItem";
            this.forcefullyEndGameToolStripMenuItem.Size = new Size(0xdd, 0x16);
            this.forcefullyEndGameToolStripMenuItem.Text = "Forcefully End Game";
            this.forcefullyEndGameToolStripMenuItem.Click += new EventHandler(this.forcefullyEndGameToolStripMenuItem_Click);
            this.skinLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Transparent;
            this.skinLabel2.DrawEdges = true;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(0xd8, 2);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0x35e, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 14;
            this.skinLabel2.Text = "<LOC>Paired Players";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(0);
            this.skinLabel1.AutoStyle = false;
            this.skinLabel1.BackColor = Color.Transparent;
            this.skinLabel1.DrawEdges = true;
            this.skinLabel1.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel1.ForeColor = Color.White;
            this.skinLabel1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel1.IsStyled = false;
            this.skinLabel1.Location = new Point(2, 2);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new Size(0xc9, 20);
            this.skinLabel1.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel1, null);
            this.skinLabel1.TabIndex = 12;
            this.skinLabel1.Text = "<LOC>Unpaired Players";
            this.skinLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel1.TextPadding = new Padding(0);
            this.btnTournament.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnTournament.AutoStyle = true;
            this.btnTournament.BackColor = Color.Black;
            this.btnTournament.ButtonState = 0;
            this.btnTournament.DialogResult = DialogResult.None;
            this.btnTournament.DisabledForecolor = Color.Gray;
            this.btnTournament.DrawColor = Color.White;
            this.btnTournament.DrawEdges = true;
            this.btnTournament.FocusColor = Color.Yellow;
            this.btnTournament.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnTournament.ForeColor = Color.White;
            this.btnTournament.HorizontalScalingMode = ScalingModes.Tile;
            this.btnTournament.IsStyled = true;
            this.btnTournament.Location = new Point(0x3cc, 0x21e);
            this.btnTournament.Name = "btnTournament";
            this.btnTournament.Size = new Size(130, 0x16);
            this.btnTournament.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnTournament, null);
            this.btnTournament.TabIndex = 0x1d;
            this.btnTournament.TabStop = true;
            this.btnTournament.Text = "<LOC>Begin Tournament";
            this.btnTournament.TextAlign = ContentAlignment.MiddleCenter;
            this.btnTournament.TextPadding = new Padding(0);
            this.btnTournament.Click += new EventHandler(this.btnTournament_Click);
            this.btnEject.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnEject.AutoStyle = true;
            this.btnEject.BackColor = Color.Black;
            this.btnEject.ButtonState = 0;
            this.btnEject.DialogResult = DialogResult.None;
            this.btnEject.DisabledForecolor = Color.Gray;
            this.btnEject.DrawColor = Color.White;
            this.btnEject.DrawEdges = true;
            this.btnEject.FocusColor = Color.Yellow;
            this.btnEject.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnEject.ForeColor = Color.White;
            this.btnEject.HorizontalScalingMode = ScalingModes.Tile;
            this.btnEject.IsStyled = true;
            this.btnEject.Location = new Point(0x344, 0x21e);
            this.btnEject.Name = "btnEject";
            this.btnEject.Size = new Size(130, 0x16);
            this.btnEject.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnEject, null);
            this.btnEject.TabIndex = 30;
            this.btnEject.TabStop = true;
            this.btnEject.Text = "<LOC>Eject Player";
            this.btnEject.TextAlign = ContentAlignment.MiddleCenter;
            this.btnEject.TextPadding = new Padding(0);
            this.btnEject.Click += new EventHandler(this.btnEject_Click);
            this.btnRound.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnRound.AutoStyle = true;
            this.btnRound.BackColor = Color.Black;
            this.btnRound.ButtonState = 0;
            this.btnRound.DialogResult = DialogResult.None;
            this.btnRound.DisabledForecolor = Color.Gray;
            this.btnRound.DrawColor = Color.White;
            this.btnRound.DrawEdges = true;
            this.btnRound.FocusColor = Color.Yellow;
            this.btnRound.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnRound.ForeColor = Color.White;
            this.btnRound.HorizontalScalingMode = ScalingModes.Tile;
            this.btnRound.IsStyled = true;
            this.btnRound.Location = new Point(0x235, 0x21e);
            this.btnRound.Name = "btnRound";
            this.btnRound.Size = new Size(130, 0x16);
            this.btnRound.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnRound, null);
            this.btnRound.TabIndex = 0x1f;
            this.btnRound.TabStop = true;
            this.btnRound.Text = "<LOC>Begin Round";
            this.btnRound.TextAlign = ContentAlignment.MiddleCenter;
            this.btnRound.TextPadding = new Padding(0);
            this.btnRound.Click += new EventHandler(this.btnRound_Click);
            this.btnAutoPair.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnAutoPair.AutoStyle = true;
            this.btnAutoPair.BackColor = Color.Black;
            this.btnAutoPair.ButtonState = 0;
            this.btnAutoPair.DialogResult = DialogResult.None;
            this.btnAutoPair.DisabledForecolor = Color.Gray;
            this.btnAutoPair.DrawColor = Color.White;
            this.btnAutoPair.DrawEdges = true;
            this.btnAutoPair.FocusColor = Color.Yellow;
            this.btnAutoPair.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnAutoPair.ForeColor = Color.White;
            this.btnAutoPair.HorizontalScalingMode = ScalingModes.Tile;
            this.btnAutoPair.IsStyled = true;
            this.btnAutoPair.Location = new Point(0x1ad, 0x21e);
            this.btnAutoPair.Name = "btnAutoPair";
            this.btnAutoPair.Size = new Size(130, 0x16);
            this.btnAutoPair.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnAutoPair, null);
            this.btnAutoPair.TabIndex = 0x20;
            this.btnAutoPair.TabStop = true;
            this.btnAutoPair.Text = "<LOC>Auto Pair";
            this.btnAutoPair.TextAlign = ContentAlignment.MiddleCenter;
            this.btnAutoPair.TextPadding = new Padding(0);
            this.btnAutoPair.Click += new EventHandler(this.btnAutoPair_Click);
            this.btnCustomRoom.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCustomRoom.AutoStyle = true;
            this.btnCustomRoom.BackColor = Color.Black;
            this.btnCustomRoom.ButtonState = 0;
            this.btnCustomRoom.DialogResult = DialogResult.None;
            this.btnCustomRoom.DisabledForecolor = Color.Gray;
            this.btnCustomRoom.DrawColor = Color.White;
            this.btnCustomRoom.DrawEdges = true;
            this.btnCustomRoom.FocusColor = Color.Yellow;
            this.btnCustomRoom.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCustomRoom.ForeColor = Color.White;
            this.btnCustomRoom.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCustomRoom.IsStyled = true;
            this.btnCustomRoom.Location = new Point(0x15, 0x21e);
            this.btnCustomRoom.Name = "btnCustomRoom";
            this.btnCustomRoom.Size = new Size(130, 0x16);
            this.btnCustomRoom.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnCustomRoom, null);
            this.btnCustomRoom.TabIndex = 0x22;
            this.btnCustomRoom.TabStop = true;
            this.btnCustomRoom.Text = "<LOC>Custom Room";
            this.btnCustomRoom.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCustomRoom.TextPadding = new Padding(0);
            this.btnCustomRoom.Click += new EventHandler(this.btnCustomRoom_Click);
            this.btnBroadcastPairings.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBroadcastPairings.AutoStyle = true;
            this.btnBroadcastPairings.BackColor = Color.Black;
            this.btnBroadcastPairings.ButtonState = 0;
            this.btnBroadcastPairings.DialogResult = DialogResult.None;
            this.btnBroadcastPairings.DisabledForecolor = Color.Gray;
            this.btnBroadcastPairings.DrawColor = Color.White;
            this.btnBroadcastPairings.DrawEdges = true;
            this.btnBroadcastPairings.FocusColor = Color.Yellow;
            this.btnBroadcastPairings.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnBroadcastPairings.ForeColor = Color.White;
            this.btnBroadcastPairings.HorizontalScalingMode = ScalingModes.Tile;
            this.btnBroadcastPairings.IsStyled = true;
            this.btnBroadcastPairings.Location = new Point(0x125, 0x21e);
            this.btnBroadcastPairings.Name = "btnBroadcastPairings";
            this.btnBroadcastPairings.Size = new Size(130, 0x16);
            this.btnBroadcastPairings.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnBroadcastPairings, null);
            this.btnBroadcastPairings.TabIndex = 0x23;
            this.btnBroadcastPairings.TabStop = true;
            this.btnBroadcastPairings.Text = "<LOC>Broadcast Parings";
            this.btnBroadcastPairings.TextAlign = ContentAlignment.MiddleCenter;
            this.btnBroadcastPairings.TextPadding = new Padding(0);
            this.btnBroadcastPairings.Click += new EventHandler(this.btnBroadcastPairings_Click);
            this.btnStandings.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnStandings.AutoStyle = true;
            this.btnStandings.BackColor = Color.Black;
            this.btnStandings.ButtonState = 0;
            this.btnStandings.DialogResult = DialogResult.None;
            this.btnStandings.DisabledForecolor = Color.Gray;
            this.btnStandings.DrawColor = Color.White;
            this.btnStandings.DrawEdges = true;
            this.btnStandings.FocusColor = Color.Yellow;
            this.btnStandings.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnStandings.ForeColor = Color.White;
            this.btnStandings.HorizontalScalingMode = ScalingModes.Tile;
            this.btnStandings.IsStyled = true;
            this.btnStandings.Location = new Point(0x9d, 0x21e);
            this.btnStandings.Name = "btnStandings";
            this.btnStandings.Size = new Size(130, 0x16);
            this.btnStandings.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnStandings, null);
            this.btnStandings.TabIndex = 0x24;
            this.btnStandings.TabStop = true;
            this.btnStandings.Text = "<LOC>Broadcast Standings";
            this.btnStandings.TextAlign = ContentAlignment.MiddleCenter;
            this.btnStandings.TextPadding = new Padding(0);
            this.btnStandings.Click += new EventHandler(this.btnStandings_Click);
            this.btnEjectOffline.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnEjectOffline.AutoStyle = true;
            this.btnEjectOffline.BackColor = Color.Black;
            this.btnEjectOffline.ButtonState = 0;
            this.btnEjectOffline.DialogResult = DialogResult.None;
            this.btnEjectOffline.DisabledForecolor = Color.Gray;
            this.btnEjectOffline.DrawColor = Color.White;
            this.btnEjectOffline.DrawEdges = true;
            this.btnEjectOffline.FocusColor = Color.Yellow;
            this.btnEjectOffline.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnEjectOffline.ForeColor = Color.White;
            this.btnEjectOffline.HorizontalScalingMode = ScalingModes.Tile;
            this.btnEjectOffline.IsStyled = true;
            this.btnEjectOffline.Location = new Point(0x125, 570);
            this.btnEjectOffline.Name = "btnEjectOffline";
            this.btnEjectOffline.Size = new Size(130, 0x16);
            this.btnEjectOffline.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnEjectOffline, null);
            this.btnEjectOffline.TabIndex = 0x29;
            this.btnEjectOffline.TabStop = true;
            this.btnEjectOffline.Text = "<LOC>Eject Offline";
            this.btnEjectOffline.TextAlign = ContentAlignment.MiddleCenter;
            this.btnEjectOffline.TextPadding = new Padding(0);
            this.btnEjectOffline.Click += new EventHandler(this.btnEjectOffline_Click);
            this.btnBeginNoLaunch.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBeginNoLaunch.AutoStyle = true;
            this.btnBeginNoLaunch.BackColor = Color.Black;
            this.btnBeginNoLaunch.ButtonState = 0;
            this.btnBeginNoLaunch.DialogResult = DialogResult.None;
            this.btnBeginNoLaunch.DisabledForecolor = Color.Gray;
            this.btnBeginNoLaunch.DrawColor = Color.White;
            this.btnBeginNoLaunch.DrawEdges = true;
            this.btnBeginNoLaunch.FocusColor = Color.Yellow;
            this.btnBeginNoLaunch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnBeginNoLaunch.ForeColor = Color.White;
            this.btnBeginNoLaunch.HorizontalScalingMode = ScalingModes.Tile;
            this.btnBeginNoLaunch.IsStyled = true;
            this.btnBeginNoLaunch.Location = new Point(0x15, 570);
            this.btnBeginNoLaunch.Name = "btnBeginNoLaunch";
            this.btnBeginNoLaunch.Size = new Size(130, 0x16);
            this.btnBeginNoLaunch.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnBeginNoLaunch, null);
            this.btnBeginNoLaunch.TabIndex = 0x2a;
            this.btnBeginNoLaunch.TabStop = true;
            this.btnBeginNoLaunch.Text = "<LOC>Begin (No Launc)";
            this.btnBeginNoLaunch.TextAlign = ContentAlignment.MiddleCenter;
            this.btnBeginNoLaunch.TextPadding = new Padding(0);
            this.btnBeginNoLaunch.Click += new EventHandler(this.btnBeginNoLaunch_Click);
            this.btnInject.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnInject.AutoStyle = true;
            this.btnInject.BackColor = Color.Black;
            this.btnInject.ButtonState = 0;
            this.btnInject.DialogResult = DialogResult.None;
            this.btnInject.DisabledForecolor = Color.Gray;
            this.btnInject.DrawColor = Color.White;
            this.btnInject.DrawEdges = true;
            this.btnInject.FocusColor = Color.Yellow;
            this.btnInject.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnInject.ForeColor = Color.White;
            this.btnInject.HorizontalScalingMode = ScalingModes.Tile;
            this.btnInject.IsStyled = true;
            this.btnInject.Location = new Point(0x2bd, 0x21e);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new Size(130, 0x16);
            this.btnInject.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnInject, null);
            this.btnInject.TabIndex = 0x2b;
            this.btnInject.TabStop = true;
            this.btnInject.Text = "<LOC>Inject Player";
            this.btnInject.TextAlign = ContentAlignment.MiddleCenter;
            this.btnInject.TextPadding = new Padding(0);
            this.btnInject.Click += new EventHandler(this.btnInject_Click);
            this.btnLocatePlayer.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnLocatePlayer.AutoStyle = true;
            this.btnLocatePlayer.BackColor = Color.Black;
            this.btnLocatePlayer.ButtonState = 0;
            this.btnLocatePlayer.DialogResult = DialogResult.None;
            this.btnLocatePlayer.DisabledForecolor = Color.Gray;
            this.btnLocatePlayer.DrawColor = Color.White;
            this.btnLocatePlayer.DrawEdges = true;
            this.btnLocatePlayer.FocusColor = Color.Yellow;
            this.btnLocatePlayer.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnLocatePlayer.ForeColor = Color.White;
            this.btnLocatePlayer.HorizontalScalingMode = ScalingModes.Tile;
            this.btnLocatePlayer.IsStyled = true;
            this.btnLocatePlayer.Location = new Point(0x1ad, 570);
            this.btnLocatePlayer.Name = "btnLocatePlayer";
            this.btnLocatePlayer.Size = new Size(130, 0x16);
            this.btnLocatePlayer.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnLocatePlayer, null);
            this.btnLocatePlayer.TabIndex = 0x2a;
            this.btnLocatePlayer.TabStop = true;
            this.btnLocatePlayer.Text = "<LOC>Locate Player";
            this.btnLocatePlayer.TextAlign = ContentAlignment.MiddleCenter;
            this.btnLocatePlayer.TextPadding = new Padding(0);
            this.btnLocatePlayer.Click += new EventHandler(this.btnLocatePlayer_Click);
            this.btnLaunchChat.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnLaunchChat.AutoStyle = true;
            this.btnLaunchChat.BackColor = Color.Black;
            this.btnLaunchChat.ButtonState = 0;
            this.btnLaunchChat.DialogResult = DialogResult.None;
            this.btnLaunchChat.DisabledForecolor = Color.Gray;
            this.btnLaunchChat.DrawColor = Color.White;
            this.btnLaunchChat.DrawEdges = true;
            this.btnLaunchChat.FocusColor = Color.Yellow;
            this.btnLaunchChat.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnLaunchChat.ForeColor = Color.White;
            this.btnLaunchChat.HorizontalScalingMode = ScalingModes.Tile;
            this.btnLaunchChat.IsStyled = true;
            this.btnLaunchChat.Location = new Point(0x9d, 570);
            this.btnLaunchChat.Name = "btnLaunchChat";
            this.btnLaunchChat.Size = new Size(130, 0x16);
            this.btnLaunchChat.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnLaunchChat, null);
            this.btnLaunchChat.TabIndex = 0x2b;
            this.btnLaunchChat.TabStop = true;
            this.btnLaunchChat.Text = "<LOC>Force Launch Chat";
            this.btnLaunchChat.TextAlign = ContentAlignment.MiddleCenter;
            this.btnLaunchChat.TextPadding = new Padding(0);
            this.btnLaunchChat.Click += new EventHandler(this.btnLaunchChat_Click);
            this.btnResetScores.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnResetScores.AutoStyle = true;
            this.btnResetScores.BackColor = Color.Black;
            this.btnResetScores.ButtonState = 0;
            this.btnResetScores.DialogResult = DialogResult.None;
            this.btnResetScores.DisabledForecolor = Color.Gray;
            this.btnResetScores.DrawColor = Color.White;
            this.btnResetScores.DrawEdges = true;
            this.btnResetScores.FocusColor = Color.Yellow;
            this.btnResetScores.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnResetScores.ForeColor = Color.White;
            this.btnResetScores.HorizontalScalingMode = ScalingModes.Tile;
            this.btnResetScores.IsStyled = true;
            this.btnResetScores.Location = new Point(0x123, 0x17);
            this.btnResetScores.Name = "btnResetScores";
            this.btnResetScores.Size = new Size(130, 0x16);
            this.btnResetScores.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnResetScores, null);
            this.btnResetScores.TabIndex = 0x2b;
            this.btnResetScores.TabStop = true;
            this.btnResetScores.Text = "Reset Scores";
            this.btnResetScores.TextAlign = ContentAlignment.MiddleCenter;
            this.btnResetScores.TextPadding = new Padding(0);
            this.btnSetNoResult.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnSetNoResult.AutoStyle = true;
            this.btnSetNoResult.BackColor = Color.Black;
            this.btnSetNoResult.ButtonState = 0;
            this.btnSetNoResult.DialogResult = DialogResult.None;
            this.btnSetNoResult.DisabledForecolor = Color.Gray;
            this.btnSetNoResult.DrawColor = Color.White;
            this.btnSetNoResult.DrawEdges = true;
            this.btnSetNoResult.FocusColor = Color.Yellow;
            this.btnSetNoResult.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnSetNoResult.ForeColor = Color.White;
            this.btnSetNoResult.HorizontalScalingMode = ScalingModes.Tile;
            this.btnSetNoResult.IsStyled = true;
            this.btnSetNoResult.Location = new Point(0x1ab, 0x17);
            this.btnSetNoResult.Name = "btnSetNoResult";
            this.btnSetNoResult.Size = new Size(130, 0x16);
            this.btnSetNoResult.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnSetNoResult, null);
            this.btnSetNoResult.TabIndex = 0x2c;
            this.btnSetNoResult.TabStop = true;
            this.btnSetNoResult.Text = "Set No Result";
            this.btnSetNoResult.TextAlign = ContentAlignment.MiddleCenter;
            this.btnSetNoResult.TextPadding = new Padding(0);
            this.btnRandomRecs.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnRandomRecs.AutoStyle = true;
            this.btnRandomRecs.BackColor = Color.Black;
            this.btnRandomRecs.ButtonState = 0;
            this.btnRandomRecs.DialogResult = DialogResult.None;
            this.btnRandomRecs.DisabledForecolor = Color.Gray;
            this.btnRandomRecs.DrawColor = Color.White;
            this.btnRandomRecs.DrawEdges = true;
            this.btnRandomRecs.FocusColor = Color.Yellow;
            this.btnRandomRecs.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnRandomRecs.ForeColor = Color.White;
            this.btnRandomRecs.HorizontalScalingMode = ScalingModes.Tile;
            this.btnRandomRecs.IsStyled = true;
            this.btnRandomRecs.Location = new Point(0x9b, 0x17);
            this.btnRandomRecs.Name = "btnRandomRecs";
            this.btnRandomRecs.Size = new Size(130, 0x16);
            this.btnRandomRecs.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnRandomRecs, null);
            this.btnRandomRecs.TabIndex = 0x2a;
            this.btnRandomRecs.TabStop = true;
            this.btnRandomRecs.Text = "Random Records";
            this.btnRandomRecs.TextAlign = ContentAlignment.MiddleCenter;
            this.btnRandomRecs.TextPadding = new Padding(0);
            this.btnRandomResults.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnRandomResults.AutoStyle = true;
            this.btnRandomResults.BackColor = Color.Black;
            this.btnRandomResults.ButtonState = 0;
            this.btnRandomResults.DialogResult = DialogResult.None;
            this.btnRandomResults.DisabledForecolor = Color.Gray;
            this.btnRandomResults.DrawColor = Color.White;
            this.btnRandomResults.DrawEdges = true;
            this.btnRandomResults.FocusColor = Color.Yellow;
            this.btnRandomResults.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnRandomResults.ForeColor = Color.White;
            this.btnRandomResults.HorizontalScalingMode = ScalingModes.Tile;
            this.btnRandomResults.IsStyled = true;
            this.btnRandomResults.Location = new Point(0x13, 0x17);
            this.btnRandomResults.Name = "btnRandomResults";
            this.btnRandomResults.Size = new Size(130, 0x16);
            this.btnRandomResults.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnRandomResults, null);
            this.btnRandomResults.TabIndex = 0x29;
            this.btnRandomResults.TabStop = true;
            this.btnRandomResults.Text = "Random Results";
            this.btnRandomResults.TextAlign = ContentAlignment.MiddleCenter;
            this.btnRandomResults.TextPadding = new Padding(0);
            this.skinButton1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButton1.AutoStyle = true;
            this.skinButton1.BackColor = Color.Black;
            this.skinButton1.ButtonState = 0;
            this.skinButton1.DialogResult = DialogResult.None;
            this.skinButton1.DisabledForecolor = Color.Gray;
            this.skinButton1.DrawColor = Color.White;
            this.skinButton1.DrawEdges = true;
            this.skinButton1.FocusColor = Color.Yellow;
            this.skinButton1.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButton1.ForeColor = Color.White;
            this.skinButton1.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButton1.IsStyled = true;
            this.skinButton1.Location = new Point(0x235, 570);
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.Size = new Size(130, 0x16);
            this.skinButton1.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButton1, null);
            this.skinButton1.TabIndex = 0x2b;
            this.skinButton1.TabStop = true;
            this.skinButton1.Text = "<LOC>Force Round";
            this.skinButton1.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButton1.TextPadding = new Padding(0);
            this.skinButton1.Click += new EventHandler(this.skinButton1_Click);
            this.cbMap.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.cbMap.BackColor = Color.Black;
            this.cbMap.BorderColor = Color.White;
            this.cbMap.DisplayMember = "MapCheckName";
            this.cbMap.DoValidate = true;
            this.cbMap.FlatStyle = FlatStyle.Flat;
            this.cbMap.FocusBackColor = Color.Empty;
            this.cbMap.FocusBorderColor = Color.Empty;
            this.cbMap.FormattingEnabled = true;
            this.cbMap.Location = new Point(880, 0x239);
            this.cbMap.Name = "cbMap";
            this.cbMap.Size = new Size(220, 0x15);
            base.ttDefault.SetSuperTip(this.cbMap, null);
            this.cbMap.TabIndex = 0x2d;
            this.cbMap.Text = "Any Map";
            this.gpgLabel7.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x2bd, 570);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0xad, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 0x2c;
            this.gpgLabel7.Text = "<LOC>Map Restriction";
            this.gpgLabel7.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel7.TextStyle = TextStyles.Default;
            this.skinButtonHonorPoints.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonHonorPoints.AutoStyle = true;
            this.skinButtonHonorPoints.BackColor = Color.Black;
            this.skinButtonHonorPoints.ButtonState = 0;
            this.skinButtonHonorPoints.DialogResult = DialogResult.None;
            this.skinButtonHonorPoints.DisabledForecolor = Color.Gray;
            this.skinButtonHonorPoints.DrawColor = Color.White;
            this.skinButtonHonorPoints.DrawEdges = true;
            this.skinButtonHonorPoints.FocusColor = Color.Yellow;
            this.skinButtonHonorPoints.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonHonorPoints.ForeColor = Color.White;
            this.skinButtonHonorPoints.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonHonorPoints.IsStyled = true;
            this.skinButtonHonorPoints.Location = new Point(0x15, 0x256);
            this.skinButtonHonorPoints.Name = "skinButtonHonorPoints";
            this.skinButtonHonorPoints.Size = new Size(130, 0x16);
            this.skinButtonHonorPoints.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonHonorPoints, null);
            this.skinButtonHonorPoints.TabIndex = 0x23;
            this.skinButtonHonorPoints.TabStop = true;
            this.skinButtonHonorPoints.Text = "<LOC>Honor Points";
            this.skinButtonHonorPoints.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonHonorPoints.TextPadding = new Padding(0);
            this.skinButtonHonorPoints.Click += new EventHandler(this.skinButtonHonorPoints_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x45f, 0x295);
            base.Controls.Add(this.skinButtonHonorPoints);
            base.Controls.Add(this.cbMap);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.skinButton1);
            base.Controls.Add(this.btnLaunchChat);
            base.Controls.Add(this.btnLocatePlayer);
            base.Controls.Add(this.btnInject);
            base.Controls.Add(this.btnBeginNoLaunch);
            base.Controls.Add(this.btnEjectOffline);
            base.Controls.Add(this.btnStandings);
            base.Controls.Add(this.btnBroadcastPairings);
            base.Controls.Add(this.btnCustomRoom);
            base.Controls.Add(this.btnAutoPair);
            base.Controls.Add(this.btnRound);
            base.Controls.Add(this.btnEject);
            base.Controls.Add(this.btnTournament);
            base.Controls.Add(this.gpgBorderPanel2);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x45f, 0x27b);
            base.Name = "DlgManageTournament";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Tournament Manager";
            base.Load += new EventHandler(this.DlgManageTournament_Load);
            base.Controls.SetChildIndex(this.gpgBorderPanel2, 0);
            base.Controls.SetChildIndex(this.btnTournament, 0);
            base.Controls.SetChildIndex(this.btnEject, 0);
            base.Controls.SetChildIndex(this.btnRound, 0);
            base.Controls.SetChildIndex(this.btnAutoPair, 0);
            base.Controls.SetChildIndex(this.btnCustomRoom, 0);
            base.Controls.SetChildIndex(this.btnBroadcastPairings, 0);
            base.Controls.SetChildIndex(this.btnStandings, 0);
            base.Controls.SetChildIndex(this.btnEjectOffline, 0);
            base.Controls.SetChildIndex(this.btnBeginNoLaunch, 0);
            base.Controls.SetChildIndex(this.btnInject, 0);
            base.Controls.SetChildIndex(this.btnLocatePlayer, 0);
            base.Controls.SetChildIndex(this.btnLaunchChat, 0);
            base.Controls.SetChildIndex(this.skinButton1, 0);
            base.Controls.SetChildIndex(this.gpgLabel7, 0);
            base.Controls.SetChildIndex(this.cbMap, 0);
            base.Controls.SetChildIndex(this.skinButtonHonorPoints, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgBorderPanel2.ResumeLayout(false);
            this.cmsGridOptions.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InjectPlayer(string name)
        {
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                if (item.PlayerName.ToUpper() == name.ToUpper())
                {
                    DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>This player already exists in the tournament."));
                    return;
                }
            }
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                if (DataAccess.GetQueryData("GetPlayerIDFromName", new object[] { name }).Count > 0)
                {
                    DataAccess.ExecuteQuery("Tournament Inject Player", new object[] { this.mTournamentID, name });
                    DataList queryData = DataAccess.GetQueryData("Tournament Participation", new object[] { this.mTournamentID });
                    using (List<DataRecord>.Enumerator enumerator = queryData.GetEnumerator())
                    {
                        VGen0 method = null;
                        while (enumerator.MoveNext())
                        {
                            DataRecord record = enumerator.Current;
                            if (record["name"].ToUpper() == name.ToUpper())
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        GPGDragItem item = new GPGDragItem {
                                            Label = name,
                                            Value = record,
                                            Seed = this.dragPlayers.Items.Count + 1,
                                            Icon = SkinManager.GetImage(@"Supcom\icon_player_sm.png")
                                        };
                                        this.dragPlayers.Items.Add(item);
                                        this.dragPlayers.Invalidate();
                                    };
                                }
                                this.Invoke(method);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    this.Invoke(delegate {
                        DlgMessage.ShowDialog(Loc.Get("<LOC>This is not a valid player name."));
                    });
                }
            }, new object[0]);
        }

        private void LaunchGames()
        {
            Exception exception;
            WaitCallback callback2 = null;
            try
            {
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    this.dragPlayers.DupeMatchups.Add(matchup);
                }
            }
            catch (Exception exception1)
            {
                exception = exception1;
                ErrorLog.WriteLine(exception);
            }
            WaitCallback callBack = null;
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                try
                {
                    int num = 0;
                    int num2 = -1;
                    for (int i = 0; i < this.dragPlayers.Matchups.Count; i++)
                    {
                        if ((item == this.dragPlayers.Matchups[i].Player1) || (item == this.dragPlayers.Matchups[i].Player2))
                        {
                            num2 = i;
                            break;
                        }
                        if (item == this.dragPlayers.Matchups[i].Player2)
                        {
                            num = 0x1388;
                        }
                    }
                    if ((this.dragPlayers.Matchups[num2].Player1 != null) || (this.dragPlayers.Matchups[num2].Player2 != null))
                    {
                        if ((this.dragPlayers.Matchups[num2].Player1 == null) || (this.dragPlayers.Matchups[num2].Player2 == null))
                        {
                            item.Won = 1;
                            item.Wins++;
                            item.PlayerReport = "Won via Bye";
                        }
                        DataRecord record = item.Value as DataRecord;
                        string str = string.Concat(new object[] { "TOURNY-", this.mTournamentID.ToString(), "-", this.Round, "-", num2 });
                        if (callBack == null)
                        {
                            if (callback2 == null)
                            {
                                callback2 = delegate (object o) {
                                    try
                                    {
                                        string recipient = (o as object[])[0].ToString();
                                        string str2 = (o as object[])[1].ToString();
                                        int num = Convert.ToInt32((o as object[])[2]);
                                        if (ConfigSettings.GetBool("Get Tourn Map Old Way", false))
                                        {
                                            Messaging.SendCustomCommand(recipient, CustomCommands.TournamentLaunchGame, new object[] { str2, this.mTournamentID, num.ToString(), this.mDetails["faction"], this.mDetails["map"] });
                                        }
                                        else
                                        {
                                            Messaging.SendCustomCommand(recipient, CustomCommands.TournamentLaunchGame, new object[] { str2, this.mTournamentID, num.ToString(), this.mDetails["faction"], this.cbMap.SelectedItem.ToString() });
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        ErrorLog.WriteLine(exception);
                                    }
                                };
                            }
                            callBack = callback2;
                        }
                        ThreadQueue.QueueUserWorkItem(callBack, new object[] { record["name"], str, num });
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void launchGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LaunchSelectedGame(this.dragPlayers.SelectedMatchup);
        }

        private void LaunchSelectedGame(GPGMatchup matchup)
        {
            WaitCallback callBack = null;
            WaitCallback callback2 = null;
            WaitCallback callback3 = null;
            WaitCallback callback4 = null;
            if (matchup != null)
            {
                if ((matchup.Player1 == null) || (matchup.Player2 == null))
                {
                    DlgMessage.ShowDialog(base.MainForm, Loc.Get("<LOC>Warning"), Loc.Get("<LOC>This matchup is not yet complete and you cannot launch."));
                }
                else
                {
                    int num = 0;
                    int num2 = -1;
                    for (int i = 0; i < this.dragPlayers.Matchups.Count; i++)
                    {
                        if (this.dragPlayers.Matchups[i] == matchup)
                        {
                            num2 = i;
                            break;
                        }
                    }
                    string str = string.Concat(new object[] { "TOURNY-", this.mTournamentID.ToString(), "-", this.Round, "-", num2 });
                    if (callBack == null)
                    {
                        if (callback3 == null)
                        {
                            callback3 = delegate (object o) {
                                string recipient = (o as object[])[0].ToString();
                                string str2 = (o as object[])[1].ToString();
                                int num = Convert.ToInt32((o as object[])[2]);
                                if (ConfigSettings.GetBool("Get Tourn Map Old Way", false))
                                {
                                    Messaging.SendCustomCommand(recipient, CustomCommands.TournamentLaunchGame, new object[] { str2, this.mTournamentID, num.ToString(), this.mDetails["faction"], this.mDetails["map"] });
                                }
                                else
                                {
                                    Messaging.SendCustomCommand(recipient, CustomCommands.TournamentLaunchGame, new object[] { str2, this.mTournamentID, num.ToString(), this.mDetails["faction"], this.cbMap.SelectedItem.ToString() });
                                }
                            };
                        }
                        callBack = callback3;
                    }
                    matchup.Player1.PlayerReport = "No Report";
                    matchup.Player2.PlayerReport = "No Report";
                    ThreadQueue.QueueUserWorkItem(callBack, new object[] { matchup.Player1.PlayerName, str, num });
                    num = 0x1388;
                    if (callback2 == null)
                    {
                        if (callback4 == null)
                        {
                            callback4 = delegate (object o) {
                                string recipient = (o as object[])[0].ToString();
                                string str2 = (o as object[])[1].ToString();
                                int num = Convert.ToInt32((o as object[])[2]);
                                if (ConfigSettings.GetBool("Get Tourn Map Old Way", false))
                                {
                                    Messaging.SendCustomCommand(recipient, CustomCommands.TournamentLaunchGame, new object[] { str2, this.mTournamentID, num.ToString(), this.mDetails["faction"], this.mDetails["map"] });
                                }
                                else
                                {
                                    Messaging.SendCustomCommand(recipient, CustomCommands.TournamentLaunchGame, new object[] { str2, this.mTournamentID, num.ToString(), this.mDetails["faction"], this.cbMap.SelectedItem.ToString() });
                                }
                            };
                        }
                        callback2 = callback4;
                    }
                    ThreadQueue.QueueUserWorkItem(callback2, new object[] { matchup.Player2.PlayerName, str, num });
                }
            }
        }

        public void LoadTournament(int tournamentid)
        {
            base.SetStatus(Loc.Get("<LOC>Loading Tournament") + ": " + tournamentid.ToString(), new object[0]);
            this.mTournamentID = tournamentid;
            int seed = 1;
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                Exception exception;
                VGen1 gen3 = null;
                VGen1 method = null;
                try
                {
                    EventLog.WriteLine("Fetching tournament details.", LogCategory.Get("DlgmanageTournament"), new object[0]);
                    VGen1 gen = null;
                    DataList queryData = DataAccess.GetQueryData("Tournament Details", new object[] { this.mTournamentID });
                    this.mDetails = queryData[0];
                    using (IEnumerator enumerator = this.cbMap.Items.GetEnumerator())
                    {
                        VGen0 gen2 = null;
                        while (enumerator.MoveNext())
                        {
                            SupcomMap map = (SupcomMap) enumerator.Current;
                            if (map.ToString() == this.mDetails["map"])
                            {
                                if (gen2 == null)
                                {
                                    gen2 = delegate {
                                        this.cbMap.SelectedItem = map;
                                    };
                                }
                                this.Invoke(gen2);
                            }
                        }
                    }
                    try
                    {
                        this.Round = Convert.ToInt32(queryData[0]["round"]);
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        ErrorLog.WriteLine(exception);
                        this.Round = 1;
                    }
                    if (this.Round < 1)
                    {
                        this.Round = 1;
                    }
                    EventLog.WriteLine("got data", LogCategory.Get("DlgmanageTournament"), new object[0]);
                    queryData = DataAccess.GetQueryData("Tournament Participation", new object[] { this.mTournamentID });
                    foreach (DataRecord record in queryData)
                    {
                        EventLog.WriteLine("Analyzing Record", LogCategory.Get("DlgmanageTournament"), new object[0]);
                        GPGDragItem item = new GPGDragItem {
                            Label = record["name"],
                            Value = record,
                            Seed = seed,
                            Icon = SkinManager.GetImage(@"Supcom\icon_player_sm.png")
                        };
                        EventLog.WriteLine("Item information: " + item.Label, LogCategory.Get("DlgmanageTournament"), new object[0]);
                        if (gen == null)
                        {
                            if (gen3 == null)
                            {
                                gen3 = delegate (object oitem) {
                                    GPGDragItem item = oitem as GPGDragItem;
                                    this.dragPlayers.Items.Add(item);
                                };
                            }
                            gen = gen3;
                        }
                        this.Invoke(gen, new object[] { item });
                        seed++;
                    }
                    DataList list2 = DataAccess.GetQueryData("Tournament Player States", new object[] { this.mTournamentID, this.mDetails["round"] });
                    if (method == null)
                    {
                        method = delegate (object orounddata) {
                            EventLog.WriteLine("Checking Matchups", LogCategory.Get("DlgmanageTournament"), new object[0]);
                            this.dragPlayers.CheckMatchup();
                            DataList list = orounddata as DataList;
                            foreach (DataRecord record in list)
                            {
                                foreach (GPGDragItem item in this.dragPlayers.Items)
                                {
                                    DataRecord record2 = item.Value as DataRecord;
                                    if (record2["principal_id"] == record["principal_id"])
                                    {
                                        int result = -1;
                                        int num2 = 0;
                                        int num3 = 0;
                                        int num4 = 0;
                                        EventLog.WriteLine("Found a matchup for two players: " + record2["principal_id"] + " " + record["principal_id"], LogCategory.Get("DlgmanageTournament"), new object[0]);
                                        if (int.TryParse(record["match_id"], out result))
                                        {
                                            EventLog.WriteLine("Got a result: " + result, LogCategory.Get("DlgmanageTournament"), new object[0]);
                                            if (result >= 0)
                                            {
                                                if (this.dragPlayers.Matchups[result].Player1 == null)
                                                {
                                                    item.IsMatched = true;
                                                    this.dragPlayers.Matchups[result].Player1 = item;
                                                }
                                                else
                                                {
                                                    item.IsMatched = true;
                                                    this.dragPlayers.Matchups[result].Player2 = item;
                                                }
                                            }
                                        }
                                        if (int.TryParse(record["wins"], out num2))
                                        {
                                            EventLog.WriteLine("Got wins: " + num2, LogCategory.Get("DlgmanageTournament"), new object[0]);
                                            item.Wins = num2;
                                        }
                                        if (int.TryParse(record["losses"], out num3))
                                        {
                                            EventLog.WriteLine("Got losses: " + num3, LogCategory.Get("DlgmanageTournament"), new object[0]);
                                            item.Losses = num3;
                                        }
                                        if (int.TryParse(record["draws"], out num4))
                                        {
                                            EventLog.WriteLine("Got draws: " + num4, LogCategory.Get("DlgmanageTournament"), new object[0]);
                                            item.Draws = num4;
                                        }
                                    }
                                }
                            }
                        };
                    }
                    this.Invoke(method, new object[] { list2 });
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    ErrorLog.WriteLine(exception);
                }
                this.Invoke(delegate {
                    this.Text = Loc.Get("<LOC>Tournament Manager") + ": " + this.mDetails["name"];
                    this.dragPlayers.Invalidate();
                    this.SetButtonState();
                });
            }, new object[0]);
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                VGen0 method = null;
                VGen0 gen3 = null;
                try
                {
                    while (!base.IsDisposed && !base.Disposing)
                    {
                        Thread.Sleep(ConfigSettings.GetInt("TournamentManagerRefreshRate", 0x2710));
                        if (!base.IsDisposed && !base.Disposing)
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    foreach (GPGDragItem item in this.dragPlayers.Items)
                                    {
                                        item.PingStatus = "Offline";
                                    }
                                    foreach (KeyValuePair<string, string> pair in TournamentCommands.sRespondingUsers)
                                    {
                                        foreach (GPGDragItem item in this.dragPlayers.Items)
                                        {
                                            if (item.PlayerName == pair.Key)
                                            {
                                                item.PingStatus = pair.Value;
                                            }
                                        }
                                    }
                                    TournamentCommands.sRespondingUsers.Clear();
                                };
                            }
                            base.Invoke(method);
                        }
                        using (List<GPGDragItem>.Enumerator enumerator = this.dragPlayers.Items.GetEnumerator())
                        {
                            VGen0 gen = null;
                            while (enumerator.MoveNext())
                            {
                                GPGDragItem item = enumerator.Current;
                                Thread.Sleep(250);
                                if (!base.IsDisposed && !base.Disposing)
                                {
                                    if (gen == null)
                                    {
                                        gen = delegate {
                                            Messaging.SendCustomCommand(item.PlayerName, CustomCommands.TournamentRequestStatus, new object[0]);
                                        };
                                    }
                                    base.Invoke(gen);
                                }
                            }
                        }
                        if (!base.IsDisposed && !base.Disposing)
                        {
                            if (gen3 == null)
                            {
                                gen3 = delegate {
                                    this.dragPlayers.Invalidate();
                                };
                            }
                            base.Invoke(gen3);
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void manuallSetMatchupRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.dragPlayers.SelectedMatchup != null) && ((this.dragPlayers.SelectedMatchup.Player1 != null) && (this.dragPlayers.SelectedMatchup.Player2 != null)))
                {
                    GPGDragItem item = this.dragPlayers.SelectedMatchup.Player1;
                    GPGDragItem item2 = this.dragPlayers.SelectedMatchup.Player2;
                    DlgSetResults results = new DlgSetResults();
                    results.lPlayer1.Text = item.Label;
                    results.lPlayer2.Text = item2.Label;
                    results.tbPlayer1Wins.Text = item.Wins.ToString();
                    results.tbPlayer1Losses.Text = item.Losses.ToString();
                    results.tbPlayer1Draws.Text = item.Draws.ToString();
                    results.tbPlayer2Wins.Text = item2.Wins.ToString();
                    results.tbPlayer2Losses.Text = item2.Losses.ToString();
                    results.tbPlayer2Draws.Text = item2.Draws.ToString();
                    if (results.ShowDialog() == DialogResult.OK)
                    {
                        int wins = item.Wins;
                        int losses = item.Losses;
                        int draws = item.Draws;
                        int num4 = item2.Wins;
                        int num5 = item2.Losses;
                        int num6 = item2.Draws;
                        item.Wins = Convert.ToInt32(results.tbPlayer1Wins.Text);
                        item.Losses = Convert.ToInt32(results.tbPlayer1Losses.Text);
                        item.Draws = Convert.ToInt32(results.tbPlayer1Draws.Text);
                        item2.Wins = Convert.ToInt32(results.tbPlayer2Wins.Text);
                        item2.Losses = Convert.ToInt32(results.tbPlayer2Losses.Text);
                        item2.Draws = Convert.ToInt32(results.tbPlayer2Draws.Text);
                        if ((wins + 1) == item.Wins)
                        {
                            item.PlayerReport = "TD Assigned Win";
                        }
                        else if ((losses + 1) == item.Losses)
                        {
                            item.PlayerReport = "TD Assigned Loss";
                        }
                        else if ((draws + 1) == item.Draws)
                        {
                            item.PlayerReport = "TD Assigned Draw";
                        }
                        else
                        {
                            item.PlayerReport = "TD Assigned Game";
                        }
                        if ((num4 + 1) == item2.Wins)
                        {
                            item2.PlayerReport = "TD Assigned Win";
                        }
                        else if ((num5 + 1) == item2.Losses)
                        {
                            item2.PlayerReport = "TD Assigned Loss";
                        }
                        else if ((num6 + 1) == item2.Draws)
                        {
                            item2.PlayerReport = "TD Assigned Draw";
                        }
                        else
                        {
                            item.PlayerReport = "TD Assigned Game";
                        }
                        base.MainForm.SystemMessage("Game Result:     " + item.Label + "     " + item2.Label, new object[0]);
                        Messaging.SendGathering("Game Result:     " + item.Label + "     " + item2.Label);
                        this.dragPlayers.Invalidate();
                        this.UpdateBrackets();
                    }
                }
            }
            catch
            {
            }
        }

        public void RandomResults()
        {
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                using (List<GPGDragItem>.Enumerator enumerator = this.dragPlayers.Items.GetEnumerator())
                {
                    VGen0 method = null;
                    while (enumerator.MoveNext())
                    {
                        GPGDragItem item = enumerator.Current;
                        Thread.Sleep(500);
                        if (method == null)
                        {
                            method = delegate {
                                int num = new Random().Next(0, 3);
                                string status = "Playing";
                                switch (num)
                                {
                                    case 1:
                                        status = "victory";
                                        break;

                                    case 2:
                                        status = "defeat";
                                        break;
                                }
                                DataRecord record = item.Value as DataRecord;
                                this.UpdateStats(Convert.ToInt32(record["principal_id"]), status);
                            };
                        }
                        base.Invoke(method);
                    }
                }
            });
        }

        private void SetButtonState()
        {
            this.btnAutoPair.Enabled = false;
            this.btnRound.Enabled = false;
            this.btnEject.Enabled = false;
            this.btnTournament.Enabled = false;
            this.btnRound.ButtonState = 0;
            this.btnRound.Text = Loc.Get("<LOC>Begin Games");
            this.btnTournament.ButtonState = 0;
            this.btnTournament.Text = Loc.Get("<LOC>Begin Tournament");
            this.dragPlayers.State = GPGDragState.NoMove;
            if (this.mDetails != null)
            {
                if (this.mDetails["status"] == "Complete")
                {
                    base.SetStatus(Loc.Get("<LOC>This tournament is complete and cannot be modified."), new object[0]);
                }
                else if (this.mDetails["status"] == "Registration")
                {
                    base.SetStatus(Loc.Get("<LOC>This tournament has not yet begun."), new object[0]);
                    this.btnTournament.Enabled = true;
                    this.btnEject.Enabled = true;
                }
                else if (this.mDetails["status"].IndexOf("Setup Round") == 0)
                {
                    base.SetStatus(this.mDetails["status"], new object[0]);
                    this.btnRound.Enabled = true;
                    this.btnEject.Enabled = true;
                    this.btnAutoPair.Enabled = true;
                    this.dragPlayers.State = GPGDragState.Open;
                }
                else if (this.mDetails["status"].IndexOf("Playing Round") == 0)
                {
                    base.SetStatus(this.mDetails["status"], new object[0]);
                    this.btnEject.Enabled = true;
                    this.dragPlayers.State = GPGDragState.LockedDown;
                    this.btnRound.ButtonState = 1;
                    this.btnRound.Text = Loc.Get("<LOC>End Round");
                    this.btnRound.Enabled = true;
                }
                else if (this.mDetails["status"].IndexOf("Completed Round") == 0)
                {
                    base.SetStatus(this.mDetails["status"], new object[0]);
                    this.btnTournament.ButtonState = 1;
                    this.btnTournament.Text = Loc.Get("<LOC>End Tournament");
                    this.btnTournament.Enabled = true;
                    this.btnRound.ButtonState = 2;
                    this.btnRound.Text = Loc.Get("<LOC>Make Brackets");
                    this.btnRound.Enabled = true;
                }
                else
                {
                    this.btnAutoPair.Enabled = true;
                    this.btnRound.Enabled = true;
                    this.btnEject.Enabled = true;
                    this.btnTournament.Enabled = true;
                }
            }
            this.btnBeginNoLaunch.Enabled = this.btnRound.ButtonState == 0;
            this.UpdateBrackets();
        }

        private void setDrawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.dragPlayers.SelectedMatchup != null) && ((this.dragPlayers.SelectedMatchup.Player1 != null) && (this.dragPlayers.SelectedMatchup.Player2 != null)))
                {
                    GPGDragItem item = this.dragPlayers.SelectedMatchup.Player1;
                    GPGDragItem item2 = this.dragPlayers.SelectedMatchup.Player2;
                    this.BackoutResults(item, item2);
                    item.Won = 2;
                    item.Draws++;
                    item.PlayerReport = "Manual Draw";
                    item2.Won = 2;
                    item2.Draws++;
                    item2.PlayerReport = "Manual Draw";
                }
            }
            catch
            {
            }
        }

        private void setPlayer1WinnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.dragPlayers.SelectedMatchup != null) && ((this.dragPlayers.SelectedMatchup.Player1 != null) && (this.dragPlayers.SelectedMatchup.Player2 != null)))
                {
                    GPGDragItem item = this.dragPlayers.SelectedMatchup.Player1;
                    GPGDragItem item2 = this.dragPlayers.SelectedMatchup.Player2;
                    this.BackoutResults(item, item2);
                    item.Won = 1;
                    item.Wins++;
                    item.PlayerReport = "Manual Win";
                    item2.Won = 0;
                    item2.Losses++;
                    item2.PlayerReport = "Manual Loss";
                }
            }
            catch
            {
            }
        }

        private void setPlayer2WinnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if ((this.dragPlayers.SelectedMatchup != null) && ((this.dragPlayers.SelectedMatchup.Player1 != null) && (this.dragPlayers.SelectedMatchup.Player2 != null)))
                {
                    GPGDragItem item = this.dragPlayers.SelectedMatchup.Player1;
                    GPGDragItem item2 = this.dragPlayers.SelectedMatchup.Player2;
                    this.BackoutResults(item, item2);
                    item2.Won = 1;
                    item2.Wins++;
                    item2.PlayerReport = "Manual Win";
                    item.Won = 0;
                    item.Losses++;
                    item.PlayerReport = "Manual Loss";
                }
            }
            catch
            {
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.mIgnoreReset = true;
            string s = DlgAskQuestion.AskQuestion(base.MainForm, "The current round is " + this.Round.ToString() + ".  Manually forcing a round number may have unexpectec consequencies.  Continue at your own risk.");
            if (s != "")
            {
                int result = 0;
                if (int.TryParse(s, out result))
                {
                    this.Round = result;
                }
            }
            this.mIgnoreReset = false;
        }

        private void skinButtonHonorPoints_Click(object sender, EventArgs e)
        {
            int num;
            string[] names = new string[this.dragPlayers.Items.Count];
            for (num = 0; num < this.dragPlayers.Items.Count; num++)
            {
                names[num] = this.dragPlayers.Items[num].PlayerName;
            }
            if (names.Length < 1)
            {
                DlgMessage.ShowDialog("<LOC>There are no tournament participants to adjust honor points for.");
            }
            else
            {
                GPGDragItem item;
                for (num = this.dragPlayers.Items.Count - 1; num >= 0; num--)
                {
                    item = this.dragPlayers.Items[num];
                    int honorPoints = item.HonorPoints;
                }
                DlgAdjustHonorPoints points = new DlgAdjustHonorPoints(names);
                if (points.ShowDialog() == DialogResult.OK)
                {
                    for (num = this.dragPlayers.Items.Count - 1; num >= 0; num--)
                    {
                        item = this.dragPlayers.Items[num];
                        if (item.PlayerName.ToLower() == points.PlayerName.ToLower())
                        {
                            item.HonorPoints -= points.PointsRemoved;
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateBrackets()
        {
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                int num = -1;
                for (int i = 0; i < this.dragPlayers.Matchups.Count; i++)
                {
                    if ((item == this.dragPlayers.Matchups[i].Player1) || (item == this.dragPlayers.Matchups[i].Player2))
                    {
                        num = i;
                        break;
                    }
                }
                DataRecord record = item.Value as DataRecord;
                int num3 = Convert.ToInt32(record["principal_id"]);
                int wins = item.Wins;
                int losses = item.Losses;
                int draws = item.Draws;
                string str = "playing";
                string str2 = "";
                int seed = item.Seed;
                DataAccess.QueueExecuteQuery("Tournament Round Update", new object[] { this.mTournamentID, num3, this.Round, wins, losses, draws, num, str, str2, seed });
            }
        }

        public void UpdateItemStatus(string playername, string status)
        {
            foreach (GPGDragItem item in this.dragPlayers.Items)
            {
                if (item.PlayerName == playername)
                {
                    item.PingStatus = status;
                }
            }
            this.dragPlayers.Invalidate();
        }

        public void UpdateStats(int principalid, string status)
        {
            base.Invoke(delegate {
                DataRecord record;
                if (ConfigSettings.GetBool("IgnoreTourneyNoReports", true))
                {
                    foreach (GPGDragItem item in this.dragPlayers.Items)
                    {
                        record = item.Value as DataRecord;
                        if ((record["principal_id"] == principalid.ToString()) && (item.PlayerReport.ToUpper() != "NO REPORT"))
                        {
                            return;
                        }
                    }
                }
                if (status.ToUpper() == "VICTORY")
                {
                    foreach (GPGDragItem item in this.dragPlayers.Items)
                    {
                        record = item.Value as DataRecord;
                        if (record["principal_id"] == principalid.ToString())
                        {
                            item.PlayerReport = status;
                            item.Won = 1;
                            item.Wins++;
                        }
                    }
                }
                else if (status.ToUpper() == "DRAW")
                {
                    foreach (GPGDragItem item in this.dragPlayers.Items)
                    {
                        record = item.Value as DataRecord;
                        if (record["principal_id"] == principalid.ToString())
                        {
                            item.PlayerReport = status;
                            item.Won = 2;
                            item.Draws++;
                        }
                    }
                }
                else
                {
                    foreach (GPGDragItem item in this.dragPlayers.Items)
                    {
                        record = item.Value as DataRecord;
                        if (record["principal_id"] == principalid.ToString())
                        {
                            item.Won = 0;
                            item.Losses++;
                            item.PlayerReport = status;
                            foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                            {
                                if ((matchup.Player1 == null) || (matchup.Player2 == null))
                                {
                                    continue;
                                }
                                if (((matchup.Player1 == item) || (matchup.Player2 != item)) && ((matchup.Player1.Won == 0) && (matchup.Player2.Won == 0)))
                                {
                                    GPGDragItem item2;
                                    GPGDragItem item3;
                                    EventLog.WriteLine("Checking matchups: " + matchup.Player1.Label + " " + matchup.Player2.Label, new object[0]);
                                    if ((matchup.Player1.PlayerReport.ToUpper() == "PLAYING") && (matchup.Player2.PlayerReport.ToUpper() == "DEFEAT"))
                                    {
                                        EventLog.WriteLine("Player1 was playing.  He is now victorious because 2 was defeated.", new object[0]);
                                        item2 = matchup.Player1;
                                        item2.Losses--;
                                        item2.Wins++;
                                        matchup.Player1.Won = 1;
                                        matchup.Player2.Won = 0;
                                        break;
                                    }
                                    if ((matchup.Player2.PlayerReport.ToUpper() == "PLAYING") && (matchup.Player1.PlayerReport.ToUpper() == "DEFEAT"))
                                    {
                                        EventLog.WriteLine("Player2 was playing.  He is now victorious because 1 was defeated.", new object[0]);
                                        item3 = matchup.Player2;
                                        item3.Losses--;
                                        item3.Wins++;
                                        matchup.Player2.Won = 1;
                                        matchup.Player1.Won = 0;
                                        break;
                                    }
                                    if ((((matchup.Player1.PlayerReport.ToUpper() != "VICTORY") && (matchup.Player2.PlayerReport.ToUpper() != "VICTORY")) && (matchup.Player1.PlayerReport.ToUpper() != "NO REPORT")) && (matchup.Player2.PlayerReport.ToUpper() != "NO REPORT"))
                                    {
                                        if ((matchup.Player1.PlayerReport.ToUpper() == "PLAYING") && (matchup.Player2.PlayerReport.ToUpper() == "PLAYING"))
                                        {
                                            EventLog.WriteLine("Both players are playing.  Ignoring draw condition.", new object[0]);
                                        }
                                        else
                                        {
                                            EventLog.WriteLine("A draw condition was assigned", new object[0]);
                                            item2 = matchup.Player1;
                                            item2.Losses--;
                                            item3 = matchup.Player2;
                                            item3.Losses--;
                                            item2.Draws++;
                                            item3.Draws++;
                                            matchup.Player1.Won = 2;
                                            matchup.Player2.Won = 2;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    if ((matchup.Player1 == null) || (matchup.Player2 == null))
                    {
                        continue;
                    }
                    DataRecord record2 = matchup.Player1.Value as DataRecord;
                    DataRecord record3 = matchup.Player2.Value as DataRecord;
                    if (((record2["principal_id"] == principalid.ToString()) || (record3["rincipal_id"] == principalid.ToString())) && ((matchup.Player1.PlayerReport != "No Report") && (matchup.Player2.PlayerReport != "No Report")))
                    {
                        string str = "";
                        if ((matchup.Player1.PlayerReport == "defeat") && (matchup.Player2.PlayerReport == "defeat"))
                        {
                            str = "This is a draw.";
                        }
                        if ((matchup.Player1.PlayerReport == "victory") && (matchup.Player2.PlayerReport == "victory"))
                        {
                            str = "This is a disconnect.";
                        }
                        if ((matchup.Player1.PlayerReport == "Playing") && (matchup.Player2.PlayerReport == "Playing"))
                        {
                            str = "This is a failed game.";
                        }
                        this.MainForm.SystemMessage("Game Result:     " + matchup.Player1.Label + "     " + matchup.Player2.Label + "     " + str, new object[0]);
                        Messaging.SendGathering("Game Result:     " + matchup.Player1.Label + "     " + matchup.Player2.Label + "     " + str);
                    }
                }
                this.CheckBadLaunches();
                this.UpdateBrackets();
                this.dragPlayers.Invalidate();
            });
        }

        public int Round
        {
            get
            {
                return this.mRound;
            }
            set
            {
                if (value != this.mRound)
                {
                    this.mRound = value;
                    if (!this.mIgnoreReset)
                    {
                        this.dragPlayers.Resetmatchups();
                    }
                }
                foreach (GPGMatchup matchup in this.dragPlayers.Matchups)
                {
                    matchup.Round = this.mRound;
                }
            }
        }
    }
}

