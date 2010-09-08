namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgPostComment : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGTextArea gpgTextAreaComment;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgPostComment() : base(Program.MainForm)
        {
            this.components = null;
            this.InitializeComponent();
        }

        public DlgPostComment(string editText) : base(Program.MainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.Text = Loc.Get("<LOC>Editing a comment");
            this.gpgTextAreaComment.Text = editText;
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
            this.gpgTextAreaComment = new GPGTextArea();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextAreaComment.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x182, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgTextAreaComment.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgTextAreaComment.BorderColor = Color.White;
            this.gpgTextAreaComment.EditValue = "";
            this.gpgTextAreaComment.Location = new Point(12, 0x67);
            this.gpgTextAreaComment.Name = "gpgTextAreaComment";
            this.gpgTextAreaComment.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaComment.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaComment.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaComment.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaComment.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaComment.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaComment.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaComment.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaComment.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaComment.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaComment.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaComment.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaComment.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaComment.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaComment.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaComment.Properties.MaxLength = 0xa00;
            this.gpgTextAreaComment.Size = new Size(0x198, 0x98);
            this.gpgTextAreaComment.TabIndex = 7;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(9, 0x54);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x94, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 8;
            this.gpgLabel1.Text = "<LOC>Post a Comment";
            this.gpgLabel1.TextStyle = TextStyles.Title;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
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
            this.skinButtonCancel.Location = new Point(0x134, 0x113);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x7d, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 9;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawColor = Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0xb1, 0x113);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x7d, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 10;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>Submit";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1bd, 0x16c);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextAreaComment);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x119, 0x124);
            base.Name = "DlgPostComment";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Post a Comment";
            base.Controls.SetChildIndex(this.gpgTextAreaComment, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextAreaComment.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextAreaComment.Select();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            if ((this.gpgTextAreaComment.Text == null) || (this.gpgTextAreaComment.Text.Length < 1))
            {
                base.Error(this.gpgTextAreaComment, "<LOC>Your comment cannot be blank.", new object[0]);
            }
            else if (Profanity.ContainsProfanity(this.gpgTextAreaComment.Text))
            {
                base.Error(this.gpgTextAreaComment, "<LOC>Your comment may not contain profanity.", new object[0]);
            }
            else
            {
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        public string Comment
        {
            get
            {
                return this.gpgTextAreaComment.Text;
            }
        }
    }
}

