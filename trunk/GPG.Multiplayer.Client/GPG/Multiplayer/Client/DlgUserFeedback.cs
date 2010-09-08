namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgUserFeedback : DlgBase
    {
        private IContainer components;
        private GPGDropDownList gpgDropDownListCategory;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabelHeader;
        private GPGLabel gpgLabelMax;
        private GPGPanel gpgPanelFeedback;
        private GPGPanel gpgPanelThank;
        private GPGPictureBox gpgPictureBox1;
        private GPGTextArea gpgTextAreaDescription;
        private static TextVal[] mCategories = null;
        private SkinButton skinButtonAccept;
        private SkinButton skinButtonCancel;

        public DlgUserFeedback(FrmMain mainForm) : base(mainForm)
        {
            this.components = null;
            this.InitializeComponent();
            try
            {
                this.gpgLabelMax.Text = string.Format(Loc.Get("<LOC>max {0} char."), this.gpgTextAreaDescription.Properties.MaxLength);
                this.gpgDropDownListCategory.Items.AddRange(Categories);
                if (this.gpgDropDownListCategory.Items.Count > 0)
                {
                    this.gpgDropDownListCategory.SelectedIndex = 0;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
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
            this.gpgLabelHeader = new GPGLabel();
            this.gpgTextAreaDescription = new GPGTextArea();
            this.gpgPanelFeedback = new GPGPanel();
            this.gpgPictureBox1 = new GPGPictureBox();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonAccept = new SkinButton();
            this.gpgLabel1 = new GPGLabel();
            this.gpgDropDownListCategory = new GPGDropDownList();
            this.gpgLabelMax = new GPGLabel();
            this.gpgPanelThank = new GPGPanel();
            this.gpgLabel2 = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextAreaDescription.Properties.BeginInit();
            this.gpgPanelFeedback.SuspendLayout();
            ((ISupportInitialize) this.gpgPictureBox1).BeginInit();
            this.gpgPanelThank.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(480, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabelHeader.AutoStyle = true;
            this.gpgLabelHeader.Font = new Font("Arial", 9.75f);
            this.gpgLabelHeader.ForeColor = Color.White;
            this.gpgLabelHeader.IgnoreMouseWheel = false;
            this.gpgLabelHeader.IsStyled = false;
            this.gpgLabelHeader.Location = new Point(0, 0x47);
            this.gpgLabelHeader.Name = "gpgLabelHeader";
            this.gpgLabelHeader.Size = new Size(0x182, 0x22);
            base.ttDefault.SetSuperTip(this.gpgLabelHeader, null);
            this.gpgLabelHeader.TabIndex = 4;
            this.gpgLabelHeader.Text = "<LOC id=_d181c106980cad8270e13183a13da680>Make a suggestion or report a problem. Please provide as much detail as possible. English only, please.";
            this.gpgLabelHeader.TextStyle = TextStyles.Default;
            this.gpgTextAreaDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgTextAreaDescription.BorderColor = Color.White;
            this.gpgTextAreaDescription.Location = new Point(3, 0x6c);
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
            this.gpgTextAreaDescription.Properties.MaxLength = 0x800;
            this.gpgTextAreaDescription.Size = new Size(0x1ef, 0x9d);
            this.gpgTextAreaDescription.TabIndex = 5;
            this.gpgPanelFeedback.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelFeedback.Controls.Add(this.gpgPictureBox1);
            this.gpgPanelFeedback.Controls.Add(this.skinButtonCancel);
            this.gpgPanelFeedback.Controls.Add(this.skinButtonAccept);
            this.gpgPanelFeedback.Controls.Add(this.gpgLabel1);
            this.gpgPanelFeedback.Controls.Add(this.gpgDropDownListCategory);
            this.gpgPanelFeedback.Controls.Add(this.gpgLabelMax);
            this.gpgPanelFeedback.Controls.Add(this.gpgLabelHeader);
            this.gpgPanelFeedback.Controls.Add(this.gpgTextAreaDescription);
            this.gpgPanelFeedback.Location = new Point(12, 0x3e);
            this.gpgPanelFeedback.Name = "gpgPanelFeedback";
            this.gpgPanelFeedback.Size = new Size(0x204, 0x13a);
            base.ttDefault.SetSuperTip(this.gpgPanelFeedback, null);
            this.gpgPanelFeedback.TabIndex = 8;
            this.gpgPictureBox1.Dock = DockStyle.Top;
            this.gpgPictureBox1.Image = Resources.gpg_banner_white;
            this.gpgPictureBox1.Location = new Point(0, 0);
            this.gpgPictureBox1.Name = "gpgPictureBox1";
            this.gpgPictureBox1.Size = new Size(0x204, 0x3a);
            this.gpgPictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.gpgPictureBox1, null);
            this.gpgPictureBox1.TabIndex = 13;
            this.gpgPictureBox1.TabStop = false;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x1ad, 0x11f);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x54, 0x18);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 12;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonAccept.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonAccept.AutoStyle = true;
            this.skinButtonAccept.BackColor = Color.Black;
            this.skinButtonAccept.DialogResult = DialogResult.OK;
            this.skinButtonAccept.DisabledForecolor = Color.Gray;
            this.skinButtonAccept.DrawColor = Color.White;
            this.skinButtonAccept.DrawEdges = true;
            this.skinButtonAccept.FocusColor = Color.Yellow;
            this.skinButtonAccept.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAccept.ForeColor = Color.White;
            this.skinButtonAccept.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAccept.IsStyled = true;
            this.skinButtonAccept.Location = new Point(0x153, 0x11f);
            this.skinButtonAccept.Name = "skinButtonAccept";
            this.skinButtonAccept.Size = new Size(0x54, 0x18);
            this.skinButtonAccept.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonAccept, null);
            this.skinButtonAccept.TabIndex = 11;
            this.skinButtonAccept.Text = "<LOC>Submit";
            this.skinButtonAccept.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAccept.TextPadding = new Padding(0);
            this.skinButtonAccept.Click += new EventHandler(this.skinButtonAccept_Click);
            this.gpgLabel1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(4, 0x10c);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xa1, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 10;
            this.gpgLabel1.Text = "<LOC>Feedback category";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgDropDownListCategory.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgDropDownListCategory.BackColor = Color.Black;
            this.gpgDropDownListCategory.BorderColor = Color.Black;
            this.gpgDropDownListCategory.DoValidate = true;
            this.gpgDropDownListCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            this.gpgDropDownListCategory.FlatStyle = FlatStyle.Flat;
            this.gpgDropDownListCategory.FocusBackColor = Color.White;
            this.gpgDropDownListCategory.FocusBorderColor = Color.White;
            this.gpgDropDownListCategory.ForeColor = Color.White;
            this.gpgDropDownListCategory.FormattingEnabled = true;
            this.gpgDropDownListCategory.Location = new Point(3, 0x11f);
            this.gpgDropDownListCategory.Name = "gpgDropDownListCategory";
            this.gpgDropDownListCategory.Size = new Size(0x101, 0x15);
            base.ttDefault.SetSuperTip(this.gpgDropDownListCategory, null);
            this.gpgDropDownListCategory.TabIndex = 9;
            this.gpgLabelMax.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabelMax.AutoStyle = true;
            this.gpgLabelMax.Enabled = false;
            this.gpgLabelMax.Font = new Font("Arial", 8f, FontStyle.Italic);
            this.gpgLabelMax.ForeColor = Color.White;
            this.gpgLabelMax.IgnoreMouseWheel = false;
            this.gpgLabelMax.IsStyled = false;
            this.gpgLabelMax.Location = new Point(430, 0x49);
            this.gpgLabelMax.Name = "gpgLabelMax";
            this.gpgLabelMax.Size = new Size(0x53, 14);
            base.ttDefault.SetSuperTip(this.gpgLabelMax, null);
            this.gpgLabelMax.TabIndex = 8;
            this.gpgLabelMax.Text = "max 2048 char.";
            this.gpgLabelMax.TextStyle = TextStyles.Default;
            this.gpgPanelThank.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelThank.Controls.Add(this.gpgLabel2);
            this.gpgPanelThank.Location = new Point(12, 0x53);
            this.gpgPanelThank.Name = "gpgPanelThank";
            this.gpgPanelThank.Size = new Size(0x204, 0x125);
            base.ttDefault.SetSuperTip(this.gpgPanelThank, null);
            this.gpgPanelThank.TabIndex = 8;
            this.gpgPanelThank.Visible = false;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Dock = DockStyle.Fill;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0, 0);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x204, 0x125);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 0;
            this.gpgLabel2.Text = "<LOC>Thank you for your feedback!";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabel2.TextStyle = TextStyles.Default;
            base.AcceptButton = this.skinButtonAccept;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x21b, 0x1b7);
            base.Controls.Add(this.gpgPanelFeedback);
            base.Controls.Add(this.gpgPanelThank);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x21b, 0x1b7);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x21b, 0x1b7);
            base.Name = "DlgUserFeedback";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Feedback";
            base.Controls.SetChildIndex(this.gpgPanelThank, 0);
            base.Controls.SetChildIndex(this.gpgPanelFeedback, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextAreaDescription.Properties.EndInit();
            this.gpgPanelFeedback.ResumeLayout(false);
            this.gpgPanelFeedback.PerformLayout();
            ((ISupportInitialize) this.gpgPictureBox1).EndInit();
            this.gpgPanelThank.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgTextAreaDescription.Select();
        }

        private void skinButtonAccept_Click(object sender, EventArgs e)
        {
            try
            {
                base.ClearErrors();
                if ((this.gpgTextAreaDescription.Text != null) && (this.gpgTextAreaDescription.Text.Length > 0))
                {
                    this.gpgPanelFeedback.Visible = false;
                    this.gpgPanelThank.Visible = true;
                    this.Refresh();
                    int tickCount = Environment.TickCount;
                    int num2 = 0;
                    if (((this.gpgDropDownListCategory.SelectedIndex >= 0) && (this.gpgDropDownListCategory.SelectedItem != null)) && (this.gpgDropDownListCategory.SelectedItem is TextVal))
                    {
                        num2 = Convert.ToInt32((this.gpgDropDownListCategory.SelectedItem as TextVal).Value);
                    }
                    DataAccess.ExecuteQuery("AddUserFeedback", new object[] { this.gpgTextAreaDescription.Text, num2 });
                    int millisecondsTimeout = 750 - (Environment.TickCount - tickCount);
                    if (millisecondsTimeout > 0)
                    {
                        Thread.Sleep(millisecondsTimeout);
                    }
                    base.Close();
                }
                else
                {
                    base.Error(this.gpgTextAreaDescription, "<LOC>Description may not be blank.", new object[0]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                base.Close();
            }
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        public static TextVal[] Categories
        {
            get
            {
                try
                {
                    if (mCategories == null)
                    {
                        DataList queryData = DataAccess.GetQueryData("GetFeedbackCategories", new object[0]);
                        mCategories = new TextVal[queryData.Count];
                        int index = 0;
                        foreach (DataRecord record in queryData)
                        {
                            mCategories[index] = new TextVal(record["description"], Convert.ToInt32(record["feedback_category_id"]));
                            index++;
                        }
                    }
                    return mCategories;
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                    return new TextVal[0];
                }
            }
        }
    }
}

