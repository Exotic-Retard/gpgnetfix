namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgMessage : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabelMsg;
        private static FrmMain sFrmMain = null;
        private SkinButton skinButtonOk;

        public DlgMessage(FrmMain mainForm, string message) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gpgLabelMsg.Text = Loc.Get(message);
        }

        public DlgMessage(FrmMain mainForm, string caption, string message) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.Text = caption;
            this.gpgLabelMsg.Text = Loc.Get(message);
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
            this.gpgLabelMsg = new GPGLabel();
            this.skinButtonOk = new SkinButton();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.gpgLabelMsg.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelMsg.AutoStyle = true;
            this.gpgLabelMsg.Font = new Font("Arial", 9.75f);
            this.gpgLabelMsg.ForeColor = Color.White;
            this.gpgLabelMsg.IgnoreMouseWheel = false;
            this.gpgLabelMsg.IsStyled = false;
            this.gpgLabelMsg.Location = new Point(12, 80);
            this.gpgLabelMsg.Name = "gpgLabelMsg";
            this.gpgLabelMsg.Size = new Size(0x15f, 0x6c);
            this.gpgLabelMsg.TabIndex = 7;
            this.gpgLabelMsg.Text = "gpgLabel1";
            this.gpgLabelMsg.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelMsg.TextStyle = TextStyles.Default;
            this.skinButtonOk.Anchor = AnchorStyles.Bottom;
            this.skinButtonOk.AutoStyle = true;
            this.skinButtonOk.BackColor = Color.Black;
            this.skinButtonOk.DialogResult = DialogResult.None;
            this.skinButtonOk.DisabledForecolor = Color.Gray;
            this.skinButtonOk.DrawEdges = true;
            this.skinButtonOk.FocusColor = Color.Yellow;
            this.skinButtonOk.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOk.ForeColor = Color.White;
            this.skinButtonOk.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOk.IsStyled = true;
            this.skinButtonOk.Location = new Point(0x86, 0xd0);
            this.skinButtonOk.Name = "skinButtonOk";
            this.skinButtonOk.Size = new Size(0x68, 0x1a);
            this.skinButtonOk.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOk.TabIndex = 8;
            this.skinButtonOk.Text = "<LOC>OK";
            this.skinButtonOk.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOk.TextPadding = new Padding(0);
            this.skinButtonOk.Click += new EventHandler(this.skinButtonOk_Click);
            base.AcceptButton = this.skinButtonOk;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonOk;
            base.ClientSize = new Size(0x177, 0x129);
            base.Controls.Add(this.skinButtonOk);
            base.Controls.Add(this.gpgLabelMsg);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x177, 0x129);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x177, 0x129);
            base.Name = "DlgMessage";
            this.Text = "";
            base.Controls.SetChildIndex(this.gpgLabelMsg, 0);
            base.Controls.SetChildIndex(this.skinButtonOk, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static void RegistermainForm(FrmMain mainForm)
        {
            sFrmMain = mainForm;
        }

        public static void Show(FrmMain mainForm, string message)
        {
            Show(mainForm, null, message);
        }

        public static void Show(FrmMain mainForm, string caption, string message)
        {
            VGen0 method = null;
            if ((mainForm.InvokeRequired && !mainForm.Disposing) && !mainForm.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        new DlgMessage(mainForm, caption, message).Show();
                    };
                }
                mainForm.BeginInvoke(method);
            }
            else if (!(mainForm.Disposing || mainForm.IsDisposed))
            {
                new DlgMessage(mainForm, caption, message).Show();
            }
        }

        public static DialogResult ShowDialog(string message)
        {
            if (sFrmMain == null)
            {
                sFrmMain = Program.MainForm;
            }
            return ShowDialog(sFrmMain, message);
        }

        public static DialogResult ShowDialog(FrmMain mainForm, string message)
        {
            return ShowDialog(mainForm, null, message);
        }

        public static DialogResult ShowDialog(string message, string caption)
        {
            if (sFrmMain == null)
            {
                sFrmMain = Program.MainForm;
            }
            return ShowDialog(sFrmMain, caption, message);
        }

        public static DialogResult ShowDialog(FrmMain mainForm, string caption, string message)
        {
            try
            {
                if (sFrmMain == null)
                {
                    sFrmMain = mainForm;
                }
                if (sFrmMain == null)
                {
                    sFrmMain = Program.MainForm;
                }
                if (!((((sFrmMain == null) || !sFrmMain.InvokeRequired) || sFrmMain.Disposing) || sFrmMain.IsDisposed))
                {
                    return (DialogResult) sFrmMain.Invoke((OGen2)delegate (object objform, object objmessage) {
                        DlgMessage curmessage = new DlgMessage((FrmMain) objform, objmessage.ToString());
                        return curmessage.ShowDialog();
                    }, new object[] { sFrmMain, message });
                }
                if ((sFrmMain == null) || (!sFrmMain.Disposing && !sFrmMain.IsDisposed))
                {
                    DlgMessage message2 = new DlgMessage(sFrmMain, caption, message);
                    return message2.ShowDialog();
                }
                return DialogResult.Cancel;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return DialogResult.Cancel;
            }
        }

        private void skinButtonOk_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
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

