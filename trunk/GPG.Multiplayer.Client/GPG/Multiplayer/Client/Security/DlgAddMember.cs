namespace GPG.Multiplayer.Client.Security
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgAddMember : DlgBase
    {
        private ComboBox comboBoxInclusionType;
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGTextBox gpgTextBoxID;
        private GPGTextBox gpgTextBoxName;
        private AccessControlList mACL;
        private AccessControlListMember mMember;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private SkinLabel skinLabel2;

        public DlgAddMember(AccessControlList acl)
        {
            this.InitializeComponent();
            this.mACL = acl;
            this.comboBoxInclusionType.Items.Add(new MultiVal<string, InclusionTypes>("Include", InclusionTypes.Include));
            this.comboBoxInclusionType.Items.Add(new MultiVal<string, InclusionTypes>("Exclude", InclusionTypes.Exclude));
            this.comboBoxInclusionType.Items.Add(new MultiVal<string, InclusionTypes>("Member Defined", InclusionTypes.MemberDefined));
            this.comboBoxInclusionType.SelectedIndex = 0;
            if (acl.InclusionType == InclusionTypes.MemberDefined)
            {
                this.comboBoxInclusionType.Enabled = true;
            }
            else
            {
                this.comboBoxInclusionType.Enabled = false;
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
            this.skinLabel2 = new SkinLabel();
            this.gpgTextBoxName = new GPGTextBox();
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxID = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.comboBoxInclusionType = new ComboBox();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxName.Properties.BeginInit();
            this.gpgTextBoxID.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x155, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.skinLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Transparent;
            this.skinLabel2.DrawEdges = true;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(6, 0x4c);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0x185, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 0x13;
            this.skinLabel2.Text = "New Access Control List Member";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(0);
            this.gpgTextBoxName.Location = new Point(0x9d, 0x76);
            this.gpgTextBoxName.Name = "gpgTextBoxName";
            this.gpgTextBoxName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxName.Size = new Size(0xe7, 20);
            this.gpgTextBoxName.TabIndex = 0x15;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x9a, 0x63);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x2a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 20;
            this.gpgLabel1.Text = "Name";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxID.Location = new Point(12, 0x76);
            this.gpgTextBoxID.Name = "gpgTextBoxID";
            this.gpgTextBoxID.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxID.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxID.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxID.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxID.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxID.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxID.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxID.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxID.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxID.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxID.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxID.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxID.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxID.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxID.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxID.Size = new Size(0x41, 20);
            this.gpgTextBoxID.TabIndex = 0x17;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(9, 0x63);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(20, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 0x16;
            this.gpgLabel2.Text = "ID";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x67, 0x7a);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x1b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 0x18;
            this.gpgLabel3.Text = "OR";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(12, 0x9c);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x142, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 0x1a;
            this.gpgLabel4.Text = "Inclusion Type (if list is inclusion type MemberDefined)";
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.comboBoxInclusionType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxInclusionType.FormattingEnabled = true;
            this.comboBoxInclusionType.Location = new Point(15, 0xaf);
            this.comboBoxInclusionType.Name = "comboBoxInclusionType";
            this.comboBoxInclusionType.Size = new Size(0xf1, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxInclusionType, null);
            this.comboBoxInclusionType.TabIndex = 0x19;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Transparent;
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
            this.skinButtonCancel.Location = new Point(0x129, 0xeb);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5b, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 30;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Transparent;
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
            this.skinButtonOK.Location = new Point(0xc0, 0xeb);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5b, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 0x1d;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(400, 0x144);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.comboBoxInclusionType);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgTextBoxID);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxName);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.skinLabel2);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(400, 0x144);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(400, 0x144);
            base.Name = "DlgAddMember";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Add ACL Member";
            base.Controls.SetChildIndex(this.skinLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxName, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxID, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.comboBoxInclusionType, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxName.Properties.EndInit();
            this.gpgTextBoxID.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            int @int;
            base.ClearErrors();
            if ((this.gpgTextBoxID.Text != null) && (this.gpgTextBoxID.Text.Length > 0))
            {
                if (!int.TryParse(this.gpgTextBoxID.Text, out @int))
                {
                    base.Error(this.gpgTextBoxID, "Invalid number", new object[0]);
                    return;
                }
                if (!new QuazalQuery("DoesPlayerExist", new object[] { @int }).GetBool())
                {
                    base.Error(this.gpgTextBoxName, "Player does not exist", new object[0]);
                    return;
                }
            }
            else if ((this.gpgTextBoxName.Text != null) && (this.gpgTextBoxName.Text.Length > 0))
            {
                @int = new QuazalQuery("GetPlayerIDFromName", new object[] { this.gpgTextBoxName.Text }).GetInt();
                if (@int <= 0)
                {
                    base.Error(this.gpgTextBoxName, "Player does not exist", new object[0]);
                    return;
                }
            }
            else
            {
                base.Error(this.gpgTextBoxID, "An ID or name is required", new object[0]);
                return;
            }
            if (this.ACL.InclusionType == InclusionTypes.MemberDefined)
            {
                this.mMember = this.ACL.AddMember(@int, (this.comboBoxInclusionType.SelectedItem as MultiVal<string, InclusionTypes>).Value2);
            }
            else
            {
                this.mMember = this.ACL.AddMember(@int);
            }
            if (this.Member == null)
            {
                base.Error(this.skinButtonOK, "An error occured adding member", new object[0]);
            }
            else
            {
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        public AccessControlList ACL
        {
            get
            {
                return this.mACL;
            }
        }

        public AccessControlListMember Member
        {
            get
            {
                return this.mMember;
            }
        }
    }
}

