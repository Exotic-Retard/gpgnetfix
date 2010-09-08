namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Security;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DlgCreateChannel : DlgBase
    {
        private IContainer components;
        private GPGCheckBox gpgCheckBoxPwd;
        private GPGCheckBox gpgCheckBoxVisible;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGTextBox gpgTextBoxMaxPop;
        private GPGTextBox gpgTextBoxName;
        private GPGTextBox gpgTextBoxPwd;
        public SkinButton skinButtonCancel;
        public SkinButton skinButtonOK;

        public event EventHandler OnError;

        public DlgCreateChannel(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gpgTextBoxMaxPop.Text = Chatroom.MaxRoomSize.ToString();
        }

        public DlgCreateChannel(FrmMain mainForm, string name) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            this.gpgTextBoxName.Text = name;
            this.gpgTextBoxMaxPop.Text = Chatroom.MaxRoomSize.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgCheckBoxPwd_CheckedChanged(object sender, EventArgs e)
        {
            this.gpgTextBoxPwd.Visible = this.gpgCheckBoxPwd.Checked;
        }

        private void InitializeComponent()
        {
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxName = new GPGTextBox();
            this.gpgTextBoxPwd = new GPGTextBox();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgCheckBoxPwd = new GPGCheckBox();
            this.gpgCheckBoxVisible = new GPGCheckBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxMaxPop = new GPGTextBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxName.Properties.BeginInit();
            this.gpgTextBoxPwd.Properties.BeginInit();
            this.gpgTextBoxMaxPop.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x153, 0x39);
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
            this.gpgLabel1.Location = new Point(9, 80);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(220, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>Enter a name for this channel";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxName.Location = new Point(12, 0x63);
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
            this.gpgTextBoxName.Properties.MaxLength = 60;
            this.gpgTextBoxName.Size = new Size(0x15f, 20);
            this.gpgTextBoxName.TabIndex = 0;
            this.gpgTextBoxPwd.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgTextBoxPwd.Location = new Point(12, 0xf5);
            this.gpgTextBoxPwd.Name = "gpgTextBoxPwd";
            this.gpgTextBoxPwd.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPwd.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPwd.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPwd.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPwd.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPwd.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPwd.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPwd.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPwd.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPwd.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPwd.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPwd.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPwd.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPwd.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPwd.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPwd.Properties.MaxLength = 0x10;
            this.gpgTextBoxPwd.Properties.PasswordChar = '*';
            this.gpgTextBoxPwd.Size = new Size(0x15f, 20);
            this.gpgTextBoxPwd.TabIndex = 4;
            this.gpgTextBoxPwd.Visible = false;
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
            this.skinButtonOK.Location = new Point(0xa2, 0x10f);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x63, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 5;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
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
            this.skinButtonCancel.Location = new Point(0x10b, 0x10f);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x63, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 6;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgCheckBoxPwd.AutoSize = true;
            this.gpgCheckBoxPwd.Location = new Point(12, 0xde);
            this.gpgCheckBoxPwd.Name = "gpgCheckBoxPwd";
            this.gpgCheckBoxPwd.Size = new Size(0x13d, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxPwd, null);
            this.gpgCheckBoxPwd.TabIndex = 3;
            this.gpgCheckBoxPwd.Text = "<LOC>Enable password protection for this channel";
            this.gpgCheckBoxPwd.UsesBG = false;
            this.gpgCheckBoxPwd.UseVisualStyleBackColor = true;
            this.gpgCheckBoxPwd.CheckedChanged += new EventHandler(this.gpgCheckBoxPwd_CheckedChanged);
            this.gpgCheckBoxVisible.AutoSize = true;
            this.gpgCheckBoxVisible.Checked = true;
            this.gpgCheckBoxVisible.CheckState = CheckState.Checked;
            this.gpgCheckBoxVisible.Location = new Point(12, 0xc7);
            this.gpgCheckBoxVisible.Name = "gpgCheckBoxVisible";
            this.gpgCheckBoxVisible.Size = new Size(0x11c, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxVisible, null);
            this.gpgCheckBoxVisible.TabIndex = 2;
            this.gpgCheckBoxVisible.Text = "<LOC>Make this channel visible to the public";
            this.gpgCheckBoxVisible.UsesBG = false;
            this.gpgCheckBoxVisible.UseVisualStyleBackColor = true;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(9, 0x8b);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x13d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 15;
            this.gpgLabel2.Text = "<LOC>Max number of players allowed in this channel";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgTextBoxMaxPop.EditValue = "100";
            this.gpgTextBoxMaxPop.Location = new Point(12, 0x9e);
            this.gpgTextBoxMaxPop.Name = "gpgTextBoxMaxPop";
            this.gpgTextBoxMaxPop.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxMaxPop.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxMaxPop.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxMaxPop.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxMaxPop.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxMaxPop.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxMaxPop.Properties.Appearance.Options.UseTextOptions = true;
            this.gpgTextBoxMaxPop.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
            this.gpgTextBoxMaxPop.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxMaxPop.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxMaxPop.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxMaxPop.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxMaxPop.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxMaxPop.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxMaxPop.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxMaxPop.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxMaxPop.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxMaxPop.Properties.MaxLength = 3;
            this.gpgTextBoxMaxPop.Size = new Size(0x2d, 20);
            this.gpgTextBoxMaxPop.TabIndex = 1;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x18e, 360);
            base.Controls.Add(this.gpgTextBoxMaxPop);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgCheckBoxVisible);
            base.Controls.Add(this.gpgCheckBoxPwd);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgTextBoxPwd);
            base.Controls.Add(this.gpgTextBoxName);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x18e, 360);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x18e, 360);
            base.Name = "DlgCreateChannel";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Create Channel";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxName, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPwd, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxPwd, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxVisible, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxMaxPop, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxName.Properties.EndInit();
            this.gpgTextBoxPwd.Properties.EndInit();
            this.gpgTextBoxMaxPop.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextBoxName.Select();
            this.gpgTextBoxName.SelectionStart = this.gpgTextBoxName.Text.Length;
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                base.Errors.Clear();
                int maxPop = Chatroom.MaxRoomSize;
                bool flag = false;
                if ((this.gpgTextBoxName.Text == null) || (this.gpgTextBoxName.Text.Length < 1))
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Channel name may not be blank", new object[0]);
                    flag = true;
                }
                if (this.gpgTextBoxName.Text.Length > 0x20)
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Channel name may not exceed {0} characters.", new object[] { 0x20 });
                    flag = true;
                }
                if (Profanity.ContainsProfanity(this.gpgTextBoxName.Text))
                {
                    base.Error(this.gpgTextBoxName, "<LOC>Channel name may not contain profanity.", new object[] { 0x20 });
                    flag = true;
                }
                if (this.gpgCheckBoxPwd.Checked && ((this.gpgTextBoxPwd.Text == null) || (this.gpgTextBoxPwd.Text.Length < 1)))
                {
                    base.Error(this.gpgCheckBoxPwd, "<LOC>Password may not be blank", new object[0]);
                    flag = true;
                }
                try
                {
                    maxPop = int.Parse(this.gpgTextBoxMaxPop.Text);
                }
                catch
                {
                    base.Error(this.gpgTextBoxMaxPop, "<LOC>{0} is not a valid number.", new object[] { this.gpgTextBoxMaxPop.Text });
                    flag = true;
                }
                if (maxPop < 1)
                {
                    base.Error(this.gpgTextBoxMaxPop, "<LOC>Maximum Population must be a positive number.", new object[0]);
                    flag = true;
                }
                if (maxPop > Chatroom.MaxRoomSize)
                {
                    base.Error(this.gpgTextBoxMaxPop, "<LOC>Maximum Population must be less than {0}", new object[] { Chatroom.MaxRoomSize });
                    flag = true;
                }
                if (!flag && new QuazalQuery("CustomChannelExists", new object[] { this.gpgTextBoxName.Text }).GetBool())
                {
                    base.Error(this.gpgTextBoxName, "<LOC>A channel by this name already exists.", new object[0]);
                    flag = true;
                }
                if (!flag)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        base.SetStatus("<LOC>Creating Channel...", new object[0]);
                        base.MainForm.JoinChatSynchronous(this.gpgTextBoxName.Text);
                        base.SetStatus("<LOC>Customizing Channel...", new object[0]);
                        string pwd = "";
                        if (this.gpgCheckBoxPwd.Checked)
                        {
                            pwd = new Hash().Encrypt(this.gpgTextBoxPwd.Text);
                        }
                        byte visible = 0;
                        if (this.gpgCheckBoxVisible.Checked)
                        {
                            visible = 1;
                        }
                        ThreadQueue.QueueUserWorkItem(delegate (object state) {
                            VGen0 method = null;
                            if (!DataAccess.ExecuteQuery("CreateCustomChannel", new object[] { this.gpgTextBoxName.Text, visible, pwd, maxPop }))
                            {
                                this.Error(this.gpgTextBoxName, "<LOC>Error creating channel.", new object[0]);
                            }
                            else
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    };
                                }
                                this.Invoke(method);
                            }
                        }, new object[0]);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        base.ClearStatus();
                    }
                    if (flag && (this.OnError != null))
                    {
                        this.OnError(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }
    }
}

