namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Timers;
    using System.Windows.Forms;

    public class DlgYesNoLater : DlgBase
    {
        private StateTimer CloseTimer;
        private IContainer components;
        private GPGLabel gpgLabelQuestion;
        private SkinButton skinButtonLater;
        private SkinButton skinButtonNo;
        private SkinButton skinButtonYes;

        public DlgYesNoLater(FrmMain mainForm, string caption, string msg, bool expires) : base(mainForm)
        {
            this.components = null;
            this.CloseTimer = null;
            this.InitializeComponent();
            this.Text = Loc.Get(caption);
            this.gpgLabelQuestion.Text = Loc.Get(msg);
            if (expires)
            {
                this.CloseTimer = new StateTimer((double) (Program.Settings.Chat.PopupTimeout * 0x3e8));
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

        private void InitializeComponent()
        {
            this.gpgLabelQuestion = new GPGLabel();
            this.skinButtonYes = new SkinButton();
            this.skinButtonNo = new SkinButton();
            this.skinButtonLater = new SkinButton();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgLabelQuestion.AutoStyle = true;
            this.gpgLabelQuestion.Font = new Font("Arial", 9.75f);
            this.gpgLabelQuestion.ForeColor = Color.White;
            this.gpgLabelQuestion.IgnoreMouseWheel = false;
            this.gpgLabelQuestion.IsStyled = false;
            this.gpgLabelQuestion.Location = new Point(12, 0x4e);
            this.gpgLabelQuestion.Name = "gpgLabelQuestion";
            this.gpgLabelQuestion.Size = new Size(370, 0x3e);
            this.gpgLabelQuestion.TabIndex = 7;
            this.gpgLabelQuestion.Text = "gpgLabel1";
            this.gpgLabelQuestion.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelQuestion.TextStyle = TextStyles.Default;
            this.skinButtonYes.AutoStyle = true;
            this.skinButtonYes.BackColor = Color.Black;
            this.skinButtonYes.DialogResult = DialogResult.OK;
            this.skinButtonYes.DisabledForecolor = Color.Gray;
            this.skinButtonYes.DrawEdges = true;
            this.skinButtonYes.FocusColor = Color.Yellow;
            this.skinButtonYes.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonYes.ForeColor = Color.White;
            this.skinButtonYes.IsStyled = true;
            this.skinButtonYes.Location = new Point(0x4e, 0xa8);
            this.skinButtonYes.Name = "skinButtonYes";
            this.skinButtonYes.Size = new Size(0x4b, 0x17);
            this.skinButtonYes.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonYes.TabIndex = 12;
            this.skinButtonYes.Text = "<LOC>Accept";
            this.skinButtonYes.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonYes.TextPadding = new Padding(0);
            this.skinButtonYes.Click += new EventHandler(this.skinButtonYes_Click);
            this.skinButtonNo.AutoStyle = true;
            this.skinButtonNo.BackColor = Color.Black;
            this.skinButtonNo.DialogResult = DialogResult.OK;
            this.skinButtonNo.DisabledForecolor = Color.Gray;
            this.skinButtonNo.DrawEdges = true;
            this.skinButtonNo.FocusColor = Color.Yellow;
            this.skinButtonNo.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNo.ForeColor = Color.White;
            this.skinButtonNo.IsStyled = true;
            this.skinButtonNo.Location = new Point(0x9f, 0xa8);
            this.skinButtonNo.Name = "skinButtonNo";
            this.skinButtonNo.Size = new Size(0x4b, 0x17);
            this.skinButtonNo.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonNo.TabIndex = 11;
            this.skinButtonNo.Text = "<LOC>Reject";
            this.skinButtonNo.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNo.TextPadding = new Padding(0);
            this.skinButtonNo.Click += new EventHandler(this.skinButtonNo_Click);
            this.skinButtonLater.AutoStyle = true;
            this.skinButtonLater.BackColor = Color.Black;
            this.skinButtonLater.DialogResult = DialogResult.OK;
            this.skinButtonLater.DisabledForecolor = Color.Gray;
            this.skinButtonLater.DrawEdges = true;
            this.skinButtonLater.FocusColor = Color.Yellow;
            this.skinButtonLater.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonLater.ForeColor = Color.White;
            this.skinButtonLater.IsStyled = true;
            this.skinButtonLater.Location = new Point(240, 0xa8);
            this.skinButtonLater.Name = "skinButtonLater";
            this.skinButtonLater.Size = new Size(0x4b, 0x17);
            this.skinButtonLater.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonLater.TabIndex = 13;
            this.skinButtonLater.Text = "<LOC>Later";
            this.skinButtonLater.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonLater.TextPadding = new Padding(0);
            this.skinButtonLater.Click += new EventHandler(this.skinButtonLater_Click);
            base.AcceptButton = this.skinButtonYes;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonNo;
            base.ClientSize = new Size(0x18a, 0x107);
            base.Controls.Add(this.skinButtonLater);
            base.Controls.Add(this.skinButtonYes);
            base.Controls.Add(this.skinButtonNo);
            base.Controls.Add(this.gpgLabelQuestion);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0x18a, 0x107);
            this.MinimumSize = new Size(0x18a, 0x107);
            base.Name = "DlgYesNoLater";
            this.Text = "DlgClanInvite";
            base.Controls.SetChildIndex(this.gpgLabelQuestion, 0);
            base.Controls.SetChildIndex(this.skinButtonNo, 0);
            base.Controls.SetChildIndex(this.skinButtonYes, 0);
            base.Controls.SetChildIndex(this.skinButtonLater, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            ElapsedEventHandler handler = null;
            base.OnLoad(e);
            if (this.CloseTimer != null)
            {
                this.CloseTimer.AutoReset = false;
                if (handler == null)
                {
                    handler = delegate (object sender, ElapsedEventArgs e1) {
                        VGen0 method = null;
                        if (((base.InvokeRequired && !base.Disposing) && !base.IsDisposed) && (base.Handle != IntPtr.Zero))
                        {
                            if (method == null)
                            {
                                method = delegate {
                                    if (this.CloseTimer != null)
                                    {
                                        this.CloseTimer.Stop();
                                        this.CloseTimer = null;
                                    }
                                    base.DialogResult = DialogResult.Ignore;
                                    base.Close();
                                };
                            }
                            base.BeginInvoke(method);
                        }
                        else if ((!base.Disposing && !base.IsDisposed) && (base.Handle != IntPtr.Zero))
                        {
                            if (this.CloseTimer != null)
                            {
                                this.CloseTimer.Stop();
                                this.CloseTimer = null;
                            }
                            base.DialogResult = DialogResult.Ignore;
                            base.Close();
                        }
                    };
                }
                this.CloseTimer.Elapsed += handler;
                this.CloseTimer.Start();
            }
        }

        private void skinButtonLater_Click(object sender, EventArgs e)
        {
            if (this.CloseTimer != null)
            {
                this.CloseTimer.Stop();
                this.CloseTimer = null;
            }
            base.DialogResult = DialogResult.Ignore;
            base.Close();
        }

        private void skinButtonNo_Click(object sender, EventArgs e)
        {
            if (this.CloseTimer != null)
            {
                this.CloseTimer.Stop();
                this.CloseTimer = null;
            }
            base.DialogResult = DialogResult.No;
            base.Close();
        }

        private void skinButtonYes_Click(object sender, EventArgs e)
        {
            if (this.CloseTimer != null)
            {
                this.CloseTimer.Stop();
                this.CloseTimer = null;
            }
            base.DialogResult = DialogResult.Yes;
            base.Close();
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

