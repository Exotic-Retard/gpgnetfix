namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgSaveQuery : DlgBase
    {
        private GPGButton btnCancel;
        private GPGButton btnOK;
        private IContainer components = null;
        private GPGLabel lQueryName;
        public GPGTextBox tbQueryname;

        public DlgSaveQuery()
        {
            this.InitializeComponent();
            Loc.LocObject(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
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
            this.btnOK = new GPGButton();
            this.btnCancel = new GPGButton();
            this.tbQueryname = new GPGTextBox();
            this.lQueryName = new GPGLabel();
            this.tbQueryname.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.btnOK.Appearance.ForeColor = Color.Black;
            this.btnOK.Appearance.Options.UseForeColor = true;
            this.btnOK.Location = new Point(0xc7, 0x7d);
            this.btnOK.LookAndFeel.SkinName = "London Liquid Sky";
            this.btnOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "<LOC>OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Appearance.ForeColor = Color.Black;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x76, 0x7d);
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 0x13;
            this.btnCancel.Text = "<LOC>Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.tbQueryname.Location = new Point(0x2b, 0x63);
            this.tbQueryname.Name = "tbQueryname";
            this.tbQueryname.Properties.Appearance.BackColor = Color.Black;
            this.tbQueryname.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbQueryname.Properties.Appearance.ForeColor = Color.White;
            this.tbQueryname.Properties.Appearance.Options.UseBackColor = true;
            this.tbQueryname.Properties.Appearance.Options.UseBorderColor = true;
            this.tbQueryname.Properties.Appearance.Options.UseForeColor = true;
            this.tbQueryname.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbQueryname.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbQueryname.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbQueryname.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbQueryname.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbQueryname.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbQueryname.Properties.BorderStyle = BorderStyles.Simple;
            this.tbQueryname.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbQueryname.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbQueryname.Size = new Size(0xe7, 20);
            this.tbQueryname.TabIndex = 0x12;
            this.lQueryName.AutoSize = true;
            this.lQueryName.AutoStyle = true;
            this.lQueryName.Font = new Font("Arial", 9.75f);
            this.lQueryName.ForeColor = Color.White;
            this.lQueryName.IgnoreMouseWheel = false;
            this.lQueryName.IsStyled = false;
            this.lQueryName.Location = new Point(40, 80);
            this.lQueryName.Name = "lQueryName";
            this.lQueryName.Size = new Size(0x51, 0x10);
            this.lQueryName.TabIndex = 0x15;
            this.lQueryName.Text = "Query Name";
            this.lQueryName.TextStyle = TextStyles.Default;
            base.AcceptButton = this.btnOK;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x13a, 0xd6);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.tbQueryname);
            base.Controls.Add(this.lQueryName);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgSaveQuery";
            this.Text = "Name your query";
            base.Controls.SetChildIndex(this.lQueryName, 0);
            base.Controls.SetChildIndex(this.tbQueryname, 0);
            base.Controls.SetChildIndex(this.btnCancel, 0);
            base.Controls.SetChildIndex(this.btnOK, 0);
            this.tbQueryname.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

