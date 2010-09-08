namespace GPG.Multiplayer.Client.SolutionsLib
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgSolutionLink : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGTextBox gpgTextBoxSolution;
        private int mSolutionID;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgSolutionLink(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.mSolutionID = 0;
            this.InitializeComponent();
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
            this.gpgTextBoxSolution = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgTextBoxSolution.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgTextBoxSolution.Location = new Point(12, 0x63);
            this.gpgTextBoxSolution.Name = "gpgTextBoxSolution";
            this.gpgTextBoxSolution.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxSolution.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxSolution.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxSolution.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxSolution.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxSolution.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxSolution.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxSolution.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxSolution.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxSolution.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxSolution.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxSolution.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxSolution.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxSolution.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxSolution.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxSolution.Size = new Size(0xe8, 20);
            this.gpgTextBoxSolution.TabIndex = 7;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 80);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x9d, 0x10);
            this.gpgLabel1.TabIndex = 8;
            this.gpgLabel1.Text = "<LOC>Enter a solution ID";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x53, 150);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x56, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 9;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0xaf, 150);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x56, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 10;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x111, 0xef);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextBoxSolution);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0x111, 0xef);
            this.MinimumSize = new Size(0x111, 0xef);
            base.Name = "DlgSolutionLink";
            this.Text = "<LOC>Solution Link";
            base.Controls.SetChildIndex(this.gpgTextBoxSolution, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            this.gpgTextBoxSolution.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextBoxSolution.Select();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            int num;
            if (int.TryParse(this.gpgTextBoxSolution.Text, out num))
            {
                this.mSolutionID = num;
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
            else
            {
                base.Error(this.gpgTextBoxSolution, "<LOC>Please enter a valid numeric solution ID.", new object[0]);
            }
        }

        public int SolutionID
        {
            get
            {
                return this.mSolutionID;
            }
        }
    }
}

