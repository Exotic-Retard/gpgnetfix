namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class DlgAskQuestion : DlgBase
    {
        private SkinButton btnCancel;
        private SkinButton btnOK;
        private IContainer components;
        private GPGLabel lQuestion;
        private bool mCanceled;
        private GPGTextBox tbAnswer;

        public DlgAskQuestion(FrmMain mainForm) : base(mainForm)
        {
            this.mCanceled = false;
            this.components = null;
            this.InitializeComponent();
            this.tbAnswer.Properties.MaxLength = 0x7d0;
            this.Text = "";
        }

        public static string AskQuestion(FrmMain mainForm, string question)
        {
            return AskQuestion(mainForm, question, false);
        }

        public static string AskQuestion(FrmMain mainForm, string question, bool isPassword)
        {
            return AskQuestion(mainForm, question, null, isPassword);
        }

        public static string AskQuestion(FrmMain mainForm, string question, string caption, bool isPassword)
        {
            DialogResult result;
            return AskQuestion(mainForm, question, caption, isPassword, out result);
        }

        public static string AskQuestion(FrmMain mainForm, string question, string caption, bool isPassword, out DialogResult result)
        {
            DlgAskQuestion question2 = new DlgAskQuestion(mainForm);
            if (caption != null)
            {
                question2.Text = Loc.Get(caption);
            }
            question2.lQuestion.Text = question;
            if (isPassword)
            {
                question2.tbAnswer.Properties.PasswordChar = '*';
            }
            result = question2.ShowDialog();
            if (result == DialogResult.OK)
            {
                return question2.tbAnswer.Text;
            }
            return "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.mCanceled = true;
            base.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void btnOK_Paint(object sender, PaintEventArgs e)
        {
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
            this.btnOK = new SkinButton();
            this.lQuestion = new GPGLabel();
            this.tbAnswer = new GPGTextBox();
            this.btnCancel = new SkinButton();
            this.tbAnswer.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
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
            this.btnOK.Location = new Point(0xb1, 0xa9);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x68, 0x1b);
            this.btnOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnOK.TabIndex = 0x21;
            this.btnOK.Text = "<LOC>OK";
            this.btnOK.TextAlign = ContentAlignment.MiddleCenter;
            this.btnOK.TextPadding = new Padding(0);
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnOK.Paint += new PaintEventHandler(this.btnOK_Paint);
            this.lQuestion.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lQuestion.AutoStyle = true;
            this.lQuestion.Font = new Font("Arial", 9.75f);
            this.lQuestion.ForeColor = Color.White;
            this.lQuestion.IgnoreMouseWheel = false;
            this.lQuestion.IsStyled = false;
            this.lQuestion.Location = new Point(12, 0x4c);
            this.lQuestion.Name = "lQuestion";
            this.lQuestion.Size = new Size(0x195, 0x3a);
            this.lQuestion.TabIndex = 0x20;
            this.lQuestion.Text = "Question";
            this.lQuestion.TextAlign = ContentAlignment.MiddleCenter;
            this.lQuestion.TextStyle = TextStyles.Default;
            this.tbAnswer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.tbAnswer.Location = new Point(0x2c, 0x89);
            this.tbAnswer.Name = "tbAnswer";
            this.tbAnswer.Properties.Appearance.BackColor = Color.Black;
            this.tbAnswer.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbAnswer.Properties.Appearance.ForeColor = Color.White;
            this.tbAnswer.Properties.Appearance.Options.UseBackColor = true;
            this.tbAnswer.Properties.Appearance.Options.UseBorderColor = true;
            this.tbAnswer.Properties.Appearance.Options.UseForeColor = true;
            this.tbAnswer.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbAnswer.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbAnswer.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbAnswer.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbAnswer.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbAnswer.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbAnswer.Properties.BorderStyle = BorderStyles.Simple;
            this.tbAnswer.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbAnswer.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbAnswer.Properties.MaxLength = 50;
            this.tbAnswer.Size = new Size(0x15b, 20);
            this.tbAnswer.TabIndex = 0x22;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.AutoStyle = true;
            this.btnCancel.BackColor = Color.Black;
            this.btnCancel.DialogResult = DialogResult.OK;
            this.btnCancel.DisabledForecolor = Color.Gray;
            this.btnCancel.DrawEdges = true;
            this.btnCancel.FocusColor = Color.Yellow;
            this.btnCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCancel.IsStyled = true;
            this.btnCancel.Location = new Point(0x11f, 0xa9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x68, 0x1b);
            this.btnCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnCancel.TabIndex = 0x1f;
            this.btnCancel.Text = "<LOC>Cancel";
            this.btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCancel.TextPadding = new Padding(0);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            base.AcceptButton = this.btnOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x1ad, 0x103);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.lQuestion);
            base.Controls.Add(this.tbAnswer);
            base.Controls.Add(this.btnCancel);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x1ad, 0x103);
            base.Name = "DlgAskQuestion";
            this.Text = "DlgAskQuestion";
            base.Controls.SetChildIndex(this.btnCancel, 0);
            base.Controls.SetChildIndex(this.tbAnswer, 0);
            base.Controls.SetChildIndex(this.lQuestion, 0);
            base.Controls.SetChildIndex(this.btnOK, 0);
            this.tbAnswer.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tbAnswer.Select();
        }

        public bool Canceled
        {
            get
            {
                return this.mCanceled;
            }
        }
    }
}

