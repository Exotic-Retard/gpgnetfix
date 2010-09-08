namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using DevExpress.Utils;
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgTournamentParticipants : DlgBase
    {
        private IContainer components;
        private GPGBorderPanel gpgBorderPanel1;
        private ListBox lbParticipants;
        private GPGTextList lbPlayers;

        public DlgTournamentParticipants(int tournamentID)
        {
            WaitCallback callBack = null;
            this.components = null;
            this.InitializeComponent();
            this.lbParticipants.Sorted = true;
            if (callBack == null)
            {
                callBack = delegate (object o) {
                    DataList queryData = DataAccess.GetQueryData("Tournament Participants", new object[] { tournamentID });
                    using (List<DataRecord>.Enumerator enumerator = queryData.GetEnumerator())
                    {
                        VGen0 method = null;
                        while (enumerator.MoveNext())
                        {
                            DataRecord row = enumerator.Current;
                            if (method == null)
                            {
                                method = delegate {
                                    this.lbParticipants.Items.Add(row["name"]);
                                };
                            }
                            this.Invoke(method);
                        }
                    }
                };
            }
            ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
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
            this.lbPlayers = new GPGTextList();
            this.gpgBorderPanel1 = new GPGBorderPanel();
            this.lbParticipants = new ListBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgBorderPanel1.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x103, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.lbPlayers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lbPlayers.AnchorControl = null;
            this.lbPlayers.AutoScroll = true;
            this.lbPlayers.BackColor = Color.Black;
            this.lbPlayers.Location = new Point(0x13, 80);
            this.lbPlayers.Margin = new Padding(0);
            this.lbPlayers.MaxLines = 6;
            this.lbPlayers.Name = "lbPlayers";
            this.lbPlayers.SelectedIndex = -1;
            this.lbPlayers.Size = new Size(0x117, 0x14b);
            base.ttDefault.SetSuperTip(this.lbPlayers, null);
            this.lbPlayers.TabIndex = 7;
            this.lbPlayers.TextLines = null;
            this.gpgBorderPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgBorderPanel1.Controls.Add(this.lbParticipants);
            this.gpgBorderPanel1.GPGBorderStyle = GPGBorderStyle.Web;
            this.gpgBorderPanel1.Location = new Point(0x17, 0x5d);
            this.gpgBorderPanel1.Name = "gpgBorderPanel1";
            this.gpgBorderPanel1.Size = new Size(0x110, 0x144);
            base.ttDefault.SetSuperTip(this.gpgBorderPanel1, null);
            this.gpgBorderPanel1.TabIndex = 8;
            this.lbParticipants.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lbParticipants.BackColor = Color.Black;
            this.lbParticipants.BorderStyle = BorderStyle.None;
            this.lbParticipants.ForeColor = Color.White;
            this.lbParticipants.FormattingEnabled = true;
            this.lbParticipants.Location = new Point(3, 3);
            this.lbParticipants.Name = "lbParticipants";
            this.lbParticipants.Size = new Size(0x10a, 0x12b);
            base.ttDefault.SetSuperTip(this.lbParticipants, null);
            this.lbParticipants.TabIndex = 11;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x13e, 480);
            base.Controls.Add(this.gpgBorderPanel1);
            base.Controls.Add(this.lbPlayers);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgTournamentParticipants";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Tournament Participants";
            base.Controls.SetChildIndex(this.lbPlayers, 0);
            base.Controls.SetChildIndex(this.gpgBorderPanel1, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgBorderPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }
    }
}

