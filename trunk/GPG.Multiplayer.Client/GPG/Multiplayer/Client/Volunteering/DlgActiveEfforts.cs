namespace GPG.Multiplayer.Client.Volunteering
{
    using DevExpress.Utils;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgActiveEfforts : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabelNoVolunteer;
        private GPGPanel gpgPanelEfforts;
        private SkinButton skinButtonClearStatus;
        private SkinButton skinButtonRefresh;

        public DlgActiveEfforts()
        {
            this.InitializeComponent();
            this.RefreshEfforts();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void effort_Volunteered(object sender, EventArgs e)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RefreshEfforts();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.RefreshEfforts();
            }
        }

        private void InitializeComponent()
        {
            this.gpgLabelNoVolunteer = new GPGLabel();
            this.gpgPanelEfforts = new GPGPanel();
            this.skinButtonRefresh = new SkinButton();
            this.skinButtonClearStatus = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x10c, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabelNoVolunteer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelNoVolunteer.AutoStyle = true;
            this.gpgLabelNoVolunteer.Font = new Font("Arial", 9.75f);
            this.gpgLabelNoVolunteer.ForeColor = Color.White;
            this.gpgLabelNoVolunteer.IgnoreMouseWheel = false;
            this.gpgLabelNoVolunteer.IsStyled = false;
            this.gpgLabelNoVolunteer.Location = new Point(30, 0x54);
            this.gpgLabelNoVolunteer.Name = "gpgLabelNoVolunteer";
            this.gpgLabelNoVolunteer.Size = new Size(260, 0x138);
            base.ttDefault.SetSuperTip(this.gpgLabelNoVolunteer, null);
            this.gpgLabelNoVolunteer.TabIndex = 7;
            this.gpgLabelNoVolunteer.Text = "<LOC>There are no volunteer opportunities available at this time.";
            this.gpgLabelNoVolunteer.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelNoVolunteer.TextStyle = TextStyles.Default;
            this.gpgPanelEfforts.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelEfforts.AutoScroll = true;
            this.gpgPanelEfforts.Location = new Point(12, 0x53);
            this.gpgPanelEfforts.Name = "gpgPanelEfforts";
            this.gpgPanelEfforts.Size = new Size(0x12f, 0x116);
            base.ttDefault.SetSuperTip(this.gpgPanelEfforts, null);
            this.gpgPanelEfforts.TabIndex = 8;
            this.gpgPanelEfforts.Visible = false;
            this.skinButtonRefresh.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonRefresh.AutoStyle = true;
            this.skinButtonRefresh.BackColor = Color.Black;
            this.skinButtonRefresh.DialogResult = DialogResult.OK;
            this.skinButtonRefresh.DisabledForecolor = Color.Gray;
            this.skinButtonRefresh.DrawEdges = true;
            this.skinButtonRefresh.FocusColor = Color.Yellow;
            this.skinButtonRefresh.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRefresh.ForeColor = Color.White;
            this.skinButtonRefresh.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonRefresh.IsStyled = true;
            this.skinButtonRefresh.Location = new Point(190, 0x16f);
            this.skinButtonRefresh.Name = "skinButtonRefresh";
            this.skinButtonRefresh.Size = new Size(0x7d, 0x1a);
            this.skinButtonRefresh.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonRefresh, null);
            this.skinButtonRefresh.TabIndex = 9;
            this.skinButtonRefresh.Text = "<LOC>Refresh";
            this.skinButtonRefresh.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRefresh.TextPadding = new Padding(0);
            this.skinButtonRefresh.Click += new EventHandler(this.skinButtonRefresh_Click);
            this.skinButtonClearStatus.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonClearStatus.AutoStyle = true;
            this.skinButtonClearStatus.BackColor = Color.Black;
            this.skinButtonClearStatus.DialogResult = DialogResult.OK;
            this.skinButtonClearStatus.DisabledForecolor = Color.Gray;
            this.skinButtonClearStatus.DrawEdges = true;
            this.skinButtonClearStatus.FocusColor = Color.Yellow;
            this.skinButtonClearStatus.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClearStatus.ForeColor = Color.White;
            this.skinButtonClearStatus.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClearStatus.IsStyled = true;
            this.skinButtonClearStatus.Location = new Point(12, 0x16f);
            this.skinButtonClearStatus.Name = "skinButtonClearStatus";
            this.skinButtonClearStatus.Size = new Size(0xa5, 0x1a);
            this.skinButtonClearStatus.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClearStatus, null);
            this.skinButtonClearStatus.TabIndex = 10;
            this.skinButtonClearStatus.Text = "<LOC>Reset Volunteer Status";
            this.skinButtonClearStatus.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClearStatus.TextPadding = new Padding(0);
            this.skinButtonClearStatus.Click += new EventHandler(this.skinButtonClearStatus_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x147, 0x1c8);
            base.Controls.Add(this.skinButtonClearStatus);
            base.Controls.Add(this.skinButtonRefresh);
            base.Controls.Add(this.gpgPanelEfforts);
            base.Controls.Add(this.gpgLabelNoVolunteer);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x147, 0x1c8);
            base.Name = "DlgActiveEfforts";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Current Volunteer Efforts";
            base.Controls.SetChildIndex(this.gpgLabelNoVolunteer, 0);
            base.Controls.SetChildIndex(this.gpgPanelEfforts, 0);
            base.Controls.SetChildIndex(this.skinButtonRefresh, 0);
            base.Controls.SetChildIndex(this.skinButtonClearStatus, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void label_Click(object sender, EventArgs e)
        {
            try
            {
                if (DlgVolunteer.ActiveForms.ContainsKey(((sender as GPGLabel).Tag as VolunteerEffort).ID))
                {
                    DlgVolunteer.ActiveForms[((sender as GPGLabel).Tag as VolunteerEffort).ID].BringToFront();
                }
                else
                {
                    new DlgVolunteer((sender as GPGLabel).Tag as VolunteerEffort).Show();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void Populate(MappedObjectList<VolunteerEffort> efforts)
        {
            this.gpgPanelEfforts.Controls.Clear();
            int num = 4;
            GPGLabel label = null;
            foreach (VolunteerEffort effort in efforts)
            {
                if (effort.HasVolunteered)
                {
                    continue;
                }
                GPGLabel label2 = new GPGLabel();
                label2.AutoSize = false;
                label2.Width = this.gpgPanelEfforts.Width - (num * 2);
                label2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
                label2.Text = effort.Description;
                label2.Tag = effort;
                label2.Click += new EventHandler(this.label_Click);
                label2.TextStyle = TextStyles.Link;
                label2.Left = num;
                if (label == null)
                {
                    label2.Top = num;
                }
                else
                {
                    label2.Top = label.Bottom + num;
                }
                label = label2;
                this.gpgPanelEfforts.Controls.Add(label2);
                effort.Volunteered += new EventHandler(this.effort_Volunteered);
            }
            if (this.gpgPanelEfforts.Controls.Count == 0)
            {
                this.gpgLabelNoVolunteer.Show();
                this.gpgPanelEfforts.Hide();
            }
            else
            {
                this.gpgPanelEfforts.Show();
                this.gpgLabelNoVolunteer.Hide();
            }
            base.ClearStatus();
            this.skinButtonRefresh.Enabled = true;
        }

        public void RefreshEfforts()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        base.SetStatus("<LOC>Refreshing...", new object[0]);
                        this.skinButtonRefresh.Enabled = false;
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                base.SetStatus("<LOC>Refreshing...", new object[0]);
                this.skinButtonRefresh.Enabled = false;
            }
            ThreadQueue.QueueUserWorkItem(delegate (object s) {
                MappedObjectList<VolunteerEffort> efforts = DataAccess.GetObjects<VolunteerEffort>("GetActiveVolunteerEfforts", new object[0]);
                base.BeginInvoke((VGen0)delegate {
                    this.Populate(efforts);
                });
            }, new object[0]);
        }

        private void skinButtonClearStatus_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            if (new DlgYesNo(base.MainForm, "<LOC>Confirmation", "<LOC>This will clear you of all volunteer efforts. At this point you can re-volunteer for any opportunities you may have been a part of. Do you want to reset your volunteer status?").ShowDialog() == DialogResult.Yes)
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        DataAccess.ExecuteQuery("ResetVolunteerStatus", new object[0]);
                        this.RefreshEfforts();
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
            }
        }

        private void skinButtonRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshEfforts();
        }
    }
}

