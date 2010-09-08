namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgYesNo : DlgBase
    {
        private IContainer components;
        private GPGCheckBox gpgCheckBoxShowAgain;
        private GPGLabel gpgLabelQuestion;
        private bool mDoNotShowAgainCheck;
        private SkinButton skinButtonNo;
        private SkinButton skinButtonYes;

        public DlgYesNo(FrmMain mainForm, string caption, string msg) : base(mainForm)
        {
            this.components = null;
            this.mDoNotShowAgainCheck = false;
            this.InitializeComponent();
            this.Text = caption;
            this.gpgLabelQuestion.Text = msg;
        }

        public DlgYesNo(FrmMain mainForm, string caption, string msg, Size size) : base(mainForm)
        {
            this.components = null;
            this.mDoNotShowAgainCheck = false;
            this.InitializeComponent();
            this.Text = caption;
            this.gpgLabelQuestion.Text = msg;
            this.MaximumSize = size;
            base.Size = size;
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
            this.skinButtonNo = new SkinButton();
            this.skinButtonYes = new SkinButton();
            this.gpgCheckBoxShowAgain = new GPGCheckBox();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabelQuestion.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelQuestion.AutoStyle = true;
            this.gpgLabelQuestion.Font = new Font("Arial", 9.75f);
            this.gpgLabelQuestion.ForeColor = Color.White;
            this.gpgLabelQuestion.IgnoreMouseWheel = false;
            this.gpgLabelQuestion.IsStyled = false;
            this.gpgLabelQuestion.Location = new Point(12, 80);
            this.gpgLabelQuestion.Name = "gpgLabelQuestion";
            this.gpgLabelQuestion.Size = new Size(370, 0x42);
            this.gpgLabelQuestion.TabIndex = 4;
            this.gpgLabelQuestion.Text = "gpgLabel1";
            this.gpgLabelQuestion.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelQuestion.TextStyle = TextStyles.Default;
            this.skinButtonNo.Anchor = AnchorStyles.Bottom;
            this.skinButtonNo.AutoStyle = true;
            this.skinButtonNo.BackColor = Color.Black;
            this.skinButtonNo.DialogResult = DialogResult.OK;
            this.skinButtonNo.DisabledForecolor = Color.Gray;
            this.skinButtonNo.DrawEdges = true;
            this.skinButtonNo.FocusColor = Color.Yellow;
            this.skinButtonNo.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNo.ForeColor = Color.White;
            this.skinButtonNo.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNo.IsStyled = true;
            this.skinButtonNo.Location = new Point(0xc6, 0xb3);
            this.skinButtonNo.Name = "skinButtonNo";
            this.skinButtonNo.Size = new Size(0x4b, 0x17);
            this.skinButtonNo.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonNo.TabIndex = 7;
            this.skinButtonNo.Text = "<LOC>No";
            this.skinButtonNo.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNo.TextPadding = new Padding(0);
            this.skinButtonNo.Click += new EventHandler(this.skinButtonNo_Click);
            this.skinButtonYes.Anchor = AnchorStyles.Bottom;
            this.skinButtonYes.AutoStyle = true;
            this.skinButtonYes.BackColor = Color.Black;
            this.skinButtonYes.DialogResult = DialogResult.OK;
            this.skinButtonYes.DisabledForecolor = Color.Gray;
            this.skinButtonYes.DrawEdges = true;
            this.skinButtonYes.FocusColor = Color.Yellow;
            this.skinButtonYes.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonYes.ForeColor = Color.White;
            this.skinButtonYes.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonYes.IsStyled = true;
            this.skinButtonYes.Location = new Point(0x75, 0xb3);
            this.skinButtonYes.Name = "skinButtonYes";
            this.skinButtonYes.Size = new Size(0x4b, 0x17);
            this.skinButtonYes.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonYes.TabIndex = 8;
            this.skinButtonYes.Text = "<LOC>Yes";
            this.skinButtonYes.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonYes.TextPadding = new Padding(0);
            this.skinButtonYes.Click += new EventHandler(this.skinButtonYes_Click);
            this.gpgCheckBoxShowAgain.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgCheckBoxShowAgain.AutoSize = true;
            this.gpgCheckBoxShowAgain.Location = new Point(12, 0x95);
            this.gpgCheckBoxShowAgain.Name = "gpgCheckBoxShowAgain";
            this.gpgCheckBoxShowAgain.Size = new Size(0xc6, 0x11);
            this.gpgCheckBoxShowAgain.TabIndex = 9;
            this.gpgCheckBoxShowAgain.Text = "<LOC>Do not show this again";
            this.gpgCheckBoxShowAgain.UseVisualStyleBackColor = true;
            this.gpgCheckBoxShowAgain.Visible = false;
            base.AcceptButton = this.skinButtonYes;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonNo;
            base.ClientSize = new Size(0x18a, 0x107);
            base.Controls.Add(this.gpgCheckBoxShowAgain);
            base.Controls.Add(this.skinButtonYes);
            base.Controls.Add(this.skinButtonNo);
            base.Controls.Add(this.gpgLabelQuestion);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x18a, 0x107);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x18a, 0x107);
            base.Name = "DlgYesNo";
            this.Text = "DlgYesNo";
            base.Controls.SetChildIndex(this.gpgLabelQuestion, 0);
            base.Controls.SetChildIndex(this.skinButtonNo, 0);
            base.Controls.SetChildIndex(this.skinButtonYes, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxShowAgain, 0);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonNo_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.No;
            base.Close();
        }

        private void skinButtonYes_Click(object sender, EventArgs e)
        {
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

        public bool DoNotShowAgainCheck
        {
            get
            {
                return this.mDoNotShowAgainCheck;
            }
            set
            {
                this.mDoNotShowAgainCheck = value;
                this.gpgCheckBoxShowAgain.Visible = value;
            }
        }

        public bool DoNotShowAgainValue
        {
            get
            {
                return this.gpgCheckBoxShowAgain.Checked;
            }
        }
    }
}

