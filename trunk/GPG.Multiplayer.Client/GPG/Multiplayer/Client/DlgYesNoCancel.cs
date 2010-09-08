namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgYesNoCancel : DlgBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabelQuestion;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonNo;
        private SkinButton skinButtonYes;

        public DlgYesNoCancel(string caption, string msg)
        {
            this.InitializeComponent();
            this.Text = Loc.Get(caption);
            this.gpgLabelQuestion.Text = Loc.Get(msg);
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
            this.skinButtonCancel = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x14f, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabelQuestion.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelQuestion.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelQuestion.AutoStyle = true;
            this.gpgLabelQuestion.Font = new Font("Arial", 9.75f);
            this.gpgLabelQuestion.ForeColor = Color.White;
            this.gpgLabelQuestion.IgnoreMouseWheel = false;
            this.gpgLabelQuestion.IsStyled = false;
            this.gpgLabelQuestion.Location = new Point(12, 0x4e);
            this.gpgLabelQuestion.Name = "gpgLabelQuestion";
            this.gpgLabelQuestion.Size = new Size(370, 0x91);
            base.ttDefault.SetSuperTip(this.gpgLabelQuestion, null);
            this.gpgLabelQuestion.TabIndex = 7;
            this.gpgLabelQuestion.Text = "gpgLabel1";
            this.gpgLabelQuestion.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelQuestion.TextStyle = TextStyles.Default;
            this.skinButtonYes.Anchor = AnchorStyles.Bottom;
            this.skinButtonYes.AutoStyle = true;
            this.skinButtonYes.BackColor = Color.Black;
            this.skinButtonYes.ButtonState = 0;
            this.skinButtonYes.DialogResult = DialogResult.OK;
            this.skinButtonYes.DisabledForecolor = Color.Gray;
            this.skinButtonYes.DrawColor = Color.White;
            this.skinButtonYes.DrawEdges = true;
            this.skinButtonYes.FocusColor = Color.Yellow;
            this.skinButtonYes.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonYes.ForeColor = Color.White;
            this.skinButtonYes.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonYes.IsStyled = true;
            this.skinButtonYes.Location = new Point(0x4e, 0xf2);
            this.skinButtonYes.Name = "skinButtonYes";
            this.skinButtonYes.Size = new Size(0x4b, 0x17);
            this.skinButtonYes.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonYes, null);
            this.skinButtonYes.TabIndex = 12;
            this.skinButtonYes.TabStop = true;
            this.skinButtonYes.Text = "<LOC>Yes";
            this.skinButtonYes.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonYes.TextPadding = new Padding(0);
            this.skinButtonYes.Click += new EventHandler(this.skinButtonYes_Click);
            this.skinButtonNo.Anchor = AnchorStyles.Bottom;
            this.skinButtonNo.AutoStyle = true;
            this.skinButtonNo.BackColor = Color.Black;
            this.skinButtonNo.ButtonState = 0;
            this.skinButtonNo.DialogResult = DialogResult.OK;
            this.skinButtonNo.DisabledForecolor = Color.Gray;
            this.skinButtonNo.DrawColor = Color.White;
            this.skinButtonNo.DrawEdges = true;
            this.skinButtonNo.FocusColor = Color.Yellow;
            this.skinButtonNo.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonNo.ForeColor = Color.White;
            this.skinButtonNo.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonNo.IsStyled = true;
            this.skinButtonNo.Location = new Point(0x9f, 0xf2);
            this.skinButtonNo.Name = "skinButtonNo";
            this.skinButtonNo.Size = new Size(0x4b, 0x17);
            this.skinButtonNo.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonNo, null);
            this.skinButtonNo.TabIndex = 11;
            this.skinButtonNo.TabStop = true;
            this.skinButtonNo.Text = "<LOC>No";
            this.skinButtonNo.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonNo.TextPadding = new Padding(0);
            this.skinButtonNo.Click += new EventHandler(this.skinButtonNo_Click);
            this.skinButtonCancel.Anchor = AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(240, 0xf2);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x4b, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 13;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonLater_Click);
            base.AcceptButton = this.skinButtonYes;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonNo;
            base.ClientSize = new Size(0x18a, 0x142);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonYes);
            base.Controls.Add(this.skinButtonNo);
            base.Controls.Add(this.gpgLabelQuestion);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x18a, 0x142);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x18a, 0x142);
            base.Name = "DlgYesNoCancel";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "DlgClanInvite";
            base.Controls.SetChildIndex(this.gpgLabelQuestion, 0);
            base.Controls.SetChildIndex(this.skinButtonNo, 0);
            base.Controls.SetChildIndex(this.skinButtonYes, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonLater_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
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
    }
}

