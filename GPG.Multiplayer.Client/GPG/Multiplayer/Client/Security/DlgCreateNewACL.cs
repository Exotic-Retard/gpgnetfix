namespace GPG.Multiplayer.Client.Security
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgCreateNewACL : DlgBase
    {
        private ComboBox comboBoxInclusionType;
        private ComboBox comboBoxListType;
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGTextArea gpgTextAreaDescription;
        private GPGTextBox gpgTextBoxName;
        private GPGTextBox gpgTextBoxQueryName;
        private AccessControlList mACL;
        private NumericUpDown numericUpDownAccessLevel;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private SkinLabel skinLabel2;

        public DlgCreateNewACL()
        {
            this.InitializeComponent();
            this.comboBoxListType.Items.Add(new MultiVal<string, AccessControlListTypes>("Member List", AccessControlListTypes.MemberList));
            this.comboBoxListType.Items.Add(new MultiVal<string, AccessControlListTypes>("Stored Query", AccessControlListTypes.QueriedList));
            this.comboBoxInclusionType.Items.Add(new MultiVal<string, InclusionTypes>("Include", InclusionTypes.Include));
            this.comboBoxInclusionType.Items.Add(new MultiVal<string, InclusionTypes>("Exclude", InclusionTypes.Exclude));
            this.comboBoxInclusionType.Items.Add(new MultiVal<string, InclusionTypes>("Member Defined", InclusionTypes.MemberDefined));
            this.comboBoxListType.SelectedIndex = 0;
            this.comboBoxInclusionType.SelectedIndex = 0;
        }

        private void comboBoxListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((AccessControlListTypes) (this.comboBoxListType.SelectedItem as MultiVal<string, AccessControlListTypes>).Value2) == AccessControlListTypes.QueriedList)
            {
                this.gpgTextBoxQueryName.Enabled = true;
            }
            else
            {
                this.gpgTextBoxQueryName.Enabled = false;
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
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxName = new GPGTextBox();
            this.skinLabel2 = new SkinLabel();
            this.comboBoxListType = new ComboBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxQueryName = new GPGTextBox();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.comboBoxInclusionType = new ComboBox();
            this.numericUpDownAccessLevel = new NumericUpDown();
            this.gpgLabel5 = new GPGLabel();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgLabel6 = new GPGLabel();
            this.gpgTextAreaDescription = new GPGTextArea();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxName.Properties.BeginInit();
            this.gpgTextBoxQueryName.Properties.BeginInit();
            this.numericUpDownAccessLevel.BeginInit();
            this.gpgTextAreaDescription.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x217, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x1f, 0x57);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x2a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "Name";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxName.Location = new Point(0x22, 0x6a);
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
            this.gpgTextBoxName.Size = new Size(0xf1, 20);
            this.gpgTextBoxName.TabIndex = 0;
            this.skinLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Transparent;
            this.skinLabel2.DrawEdges = true;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(6, 0x37);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0x247, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 0x12;
            this.skinLabel2.Text = "New Access Control List";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(0);
            this.comboBoxListType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxListType.FormattingEnabled = true;
            this.comboBoxListType.Location = new Point(0x135, 0x6a);
            this.comboBoxListType.Name = "comboBoxListType";
            this.comboBoxListType.Size = new Size(0xf1, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxListType, null);
            this.comboBoxListType.TabIndex = 2;
            this.comboBoxListType.SelectedIndexChanged += new EventHandler(this.comboBoxListType_SelectedIndexChanged);
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x132, 0x57);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x3d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 20;
            this.gpgLabel2.Text = "List Type";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgTextBoxQueryName.Location = new Point(0x135, 0x9b);
            this.gpgTextBoxQueryName.Name = "gpgTextBoxQueryName";
            this.gpgTextBoxQueryName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxQueryName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxQueryName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxQueryName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxQueryName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxQueryName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxQueryName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxQueryName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxQueryName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxQueryName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxQueryName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxQueryName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxQueryName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxQueryName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxQueryName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxQueryName.Size = new Size(0xf1, 20);
            this.gpgTextBoxQueryName.TabIndex = 3;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x132, 0x88);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x7b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 0x15;
            this.gpgLabel3.Text = "Stored Query Name";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x132, 0xb8);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 0x18;
            this.gpgLabel4.Text = "Inclusion Type";
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.comboBoxInclusionType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxInclusionType.FormattingEnabled = true;
            this.comboBoxInclusionType.Location = new Point(0x135, 0xcb);
            this.comboBoxInclusionType.Name = "comboBoxInclusionType";
            this.comboBoxInclusionType.Size = new Size(0xf1, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxInclusionType, null);
            this.comboBoxInclusionType.TabIndex = 4;
            this.numericUpDownAccessLevel.Location = new Point(0x135, 0xfc);
            this.numericUpDownAccessLevel.Name = "numericUpDownAccessLevel";
            this.numericUpDownAccessLevel.Size = new Size(120, 20);
            base.ttDefault.SetSuperTip(this.numericUpDownAccessLevel, null);
            this.numericUpDownAccessLevel.TabIndex = 5;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x132, 0xe9);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x55, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x1a;
            this.gpgLabel5.Text = "Access Level";
            this.gpgLabel5.TextStyle = TextStyles.Default;
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
            this.skinButtonOK.Location = new Point(0x183, 0x13a);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5b, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 6;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "Create";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
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
            this.skinButtonCancel.Location = new Point(0x1ec, 0x13a);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5b, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 7;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x1f, 0x87);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x49, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x1d;
            this.gpgLabel6.Text = "Description";
            this.gpgLabel6.TextStyle = TextStyles.Default;
            this.gpgTextAreaDescription.BorderColor = Color.White;
            this.gpgTextAreaDescription.Location = new Point(0x22, 0x9a);
            this.gpgTextAreaDescription.Name = "gpgTextAreaDescription";
            this.gpgTextAreaDescription.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaDescription.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaDescription.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaDescription.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaDescription.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaDescription.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaDescription.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaDescription.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaDescription.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaDescription.Size = new Size(0xf1, 0x76);
            this.gpgTextAreaDescription.TabIndex = 1;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x252, 0x193);
            base.Controls.Add(this.gpgTextAreaDescription);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.numericUpDownAccessLevel);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.comboBoxInclusionType);
            base.Controls.Add(this.gpgTextBoxQueryName);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.comboBoxListType);
            base.Controls.Add(this.skinLabel2);
            base.Controls.Add(this.gpgTextBoxName);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x252, 0x193);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x252, 0x193);
            base.Name = "DlgCreateNewACL";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Create New ACL";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxName, 0);
            base.Controls.SetChildIndex(this.skinLabel2, 0);
            base.Controls.SetChildIndex(this.comboBoxListType, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxQueryName, 0);
            base.Controls.SetChildIndex(this.comboBoxInclusionType, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.numericUpDownAccessLevel, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgTextAreaDescription, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxName.Properties.EndInit();
            this.gpgTextBoxQueryName.Properties.EndInit();
            this.numericUpDownAccessLevel.EndInit();
            this.gpgTextAreaDescription.Properties.EndInit();
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
            base.ClearErrors();
            string text = this.gpgTextAreaDescription.Text;
            string name = this.gpgTextBoxName.Text;
            if ((name == null) || (name.Length < 1))
            {
                base.Error(this.gpgTextBoxName, "Field cannot be blank", new object[0]);
            }
            else
            {
                AccessControlListTypes listType = (this.comboBoxListType.SelectedItem as MultiVal<string, AccessControlListTypes>).Value2;
                string tag = this.gpgTextBoxQueryName.Text;
                if ((listType == AccessControlListTypes.QueriedList) && ((tag == null) || (tag.Length < 1)))
                {
                    base.Error(this.gpgTextBoxQueryName, "Field cannot be blank", new object[0]);
                }
                else
                {
                    if (listType != AccessControlListTypes.QueriedList)
                    {
                        tag = "";
                    }
                    InclusionTypes inclusionType = (this.comboBoxInclusionType.SelectedItem as MultiVal<string, InclusionTypes>).Value2;
                    int access = (int) this.numericUpDownAccessLevel.Value;
                    this.mACL = AccessControlList.Create(name, text, listType, inclusionType, access, tag);
                    if (this.ACL == null)
                    {
                        base.Error(this.skinButtonOK, "An error occured creating ACL", new object[0]);
                    }
                    else
                    {
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                }
            }
        }

        public AccessControlList ACL
        {
            get
            {
                return this.mACL;
            }
        }
    }
}

