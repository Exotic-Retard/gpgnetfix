namespace GPG.Multiplayer.Client.Volunteering
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.SolutionsLib;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgVolunteer : DlgBase
    {
        internal static Dictionary<int, DlgVolunteer> ActiveForms = new Dictionary<int, DlgVolunteer>();
        private IContainer components;
        private GPGCheckBox gpgCheckBoxDay1;
        private GPGCheckBox gpgCheckBoxDay2;
        private GPGCheckBox gpgCheckBoxDay3;
        private GPGCheckBox gpgCheckBoxDay4;
        private GPGCheckBox gpgCheckBoxDay5;
        private GPGCheckBox gpgCheckBoxDay6;
        private GPGCheckBox gpgCheckBoxDay7;
        private GPGCheckBox gpgCheckBoxLegalAgree;
        private GPGCheckBox gpgCheckBoxTime1;
        private GPGCheckBox gpgCheckBoxTime2;
        private GPGCheckBox gpgCheckBoxTime3;
        private GPGGroupBox gpgGroupBox1;
        private GPGGroupBox gpgGroupBox2;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel13;
        private GPGLabel gpgLabel15;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel9;
        private GPGLabel gpgLabelBeginDate;
        private GPGLabel gpgLabelDays;
        private GPGLabel gpgLabelDaysAvailable;
        private GPGLabel gpgLabelDescription;
        private GPGLabel gpgLabelEndDate;
        private GPGLabel gpgLabelHeadOfEffort;
        private GPGLabel gpgLabelKBLink;
        private GPGLabel gpgLabelLegalKB;
        private GPGLabel gpgLabelTimes;
        private GPGLabel gpgLabelTimesAvailable;
        private GPGLabel gpgLabelTimeZone;
        private GPGPanel gpgPanelInput;
        private GPGRichLabel gpgRichLabelLegal;
        private GPGTextBox gpgTextBoxEmail;
        private bool HasVolunteered;
        private VolunteerEffort mEffort;
        private SkinButton skinButtonOK;

        public DlgVolunteer(VolunteerEffort effort)
        {
            WaitCallback callBack = null;
            this.components = null;
            this.HasVolunteered = false;
            this.InitializeComponent();
            this.mEffort = effort;
            string str = "";
            int[] desiredDays = this.Effort.DesiredDays;
            for (int i = 0; i < desiredDays.Length; i++)
            {
                switch (desiredDays[i])
                {
                    case 1:
                        str = str + Loc.Get("<LOC>Monday") + ", ";
                        break;

                    case 2:
                        str = str + Loc.Get("<LOC>Tuesday") + ", ";
                        break;

                    case 3:
                        str = str + Loc.Get("<LOC>Wednesday") + ", ";
                        break;

                    case 4:
                        str = str + Loc.Get("<LOC>Thursday") + ", ";
                        break;

                    case 5:
                        str = str + Loc.Get("<LOC>Friday") + ", ";
                        break;

                    case 6:
                        str = str + Loc.Get("<LOC>Saturday") + ", ";
                        break;

                    case 7:
                        str = str + Loc.Get("<LOC>Sunday") + ", ";
                        break;
                }
            }
            str = str.TrimEnd(", ".ToCharArray());
            string str2 = "";
            foreach (int num2 in this.Effort.DesiredTimes)
            {
                switch (num2)
                {
                    case 1:
                        str2 = str2 + Loc.Get("<LOC>Mornings") + ", ";
                        break;

                    case 2:
                        str2 = str2 + Loc.Get("<LOC>Afternoons") + ", ";
                        break;

                    case 3:
                        str2 = str2 + Loc.Get("<LOC>Evenings") + ", ";
                        break;
                }
            }
            str2 = str2.TrimEnd(", ".ToCharArray());
            this.gpgLabelBeginDate.Text = this.Effort.BeginDate.ToShortDateString();
            this.gpgLabelDays.Text = str;
            this.gpgLabelDescription.Text = this.Effort.Description;
            this.gpgLabelEndDate.Text = this.Effort.EndDate.ToShortDateString();
            this.gpgLabelHeadOfEffort.Text = this.Effort.HeadOfEffort;
            this.gpgLabelTimes.Text = str2;
            this.gpgLabelTimeZone.Text = this.Effort.TimeZone;
            if (callBack == null)
            {
                callBack = delegate (object s) {
                    VGen0 method = null;
                    Solution legal = Solution.Lookup(this.Effort.LegalKB);
                    string email = User.Current.Email;
                    if (!base.Disposing && !base.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                try
                                {
                                    this.gpgRichLabelLegal.Rtf = legal.Description;
                                    this.gpgTextBoxEmail.Text = email;
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            };
                        }
                        base.BeginInvoke(method);
                    }
                };
            }
            ThreadPool.QueueUserWorkItem(callBack);
            ActiveForms.Add(this.Effort.ID, this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgCheckBoxLegalAgree_CheckedChanged(object sender, EventArgs e)
        {
            this.gpgPanelInput.Visible = this.gpgCheckBoxLegalAgree.Checked;
        }

        private void gpgLabelKBLink_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowDlgSolution(this.Effort.DetailKB);
        }

        private void gpgLabelLegalKB_Click(object sender, EventArgs e)
        {
            base.MainForm.ShowDlgSolution(this.Effort.LegalKB);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgVolunteer));
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabelDescription = new GPGLabel();
            this.gpgLabelKBLink = new GPGLabel();
            this.gpgLabelBeginDate = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelEndDate = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabelTimeZone = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabelTimes = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.gpgLabelDays = new GPGLabel();
            this.gpgLabel13 = new GPGLabel();
            this.gpgLabelHeadOfEffort = new GPGLabel();
            this.gpgLabel15 = new GPGLabel();
            this.gpgGroupBox1 = new GPGGroupBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgPanelInput = new GPGPanel();
            this.gpgTextBoxEmail = new GPGTextBox();
            this.gpgLabel4 = new GPGLabel();
            this.gpgCheckBoxTime3 = new GPGCheckBox();
            this.gpgCheckBoxTime2 = new GPGCheckBox();
            this.gpgCheckBoxTime1 = new GPGCheckBox();
            this.gpgLabelTimesAvailable = new GPGLabel();
            this.skinButtonOK = new SkinButton();
            this.gpgCheckBoxDay7 = new GPGCheckBox();
            this.gpgCheckBoxDay6 = new GPGCheckBox();
            this.gpgCheckBoxDay5 = new GPGCheckBox();
            this.gpgCheckBoxDay4 = new GPGCheckBox();
            this.gpgCheckBoxDay3 = new GPGCheckBox();
            this.gpgCheckBoxDay2 = new GPGCheckBox();
            this.gpgCheckBoxDay1 = new GPGCheckBox();
            this.gpgLabelDaysAvailable = new GPGLabel();
            this.gpgCheckBoxLegalAgree = new GPGCheckBox();
            this.gpgGroupBox2 = new GPGGroupBox();
            this.gpgRichLabelLegal = new GPGRichLabel();
            this.gpgLabelLegalKB = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgGroupBox1.SuspendLayout();
            this.gpgPanelInput.SuspendLayout();
            this.gpgTextBoxEmail.Properties.BeginInit();
            this.gpgGroupBox2.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x2f4, 0x39);
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
            this.gpgLabel1.Location = new Point(6, 0x16);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x73, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>Description";
            this.gpgLabel1.TextStyle = TextStyles.Header1;
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDescription.AutoSize = true;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(12, 0x26);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelDescription, null);
            this.gpgLabelDescription.TabIndex = 8;
            this.gpgLabelDescription.Text = "gpgLabel2";
            this.gpgLabelDescription.TextStyle = TextStyles.Default;
            this.gpgLabelKBLink.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelKBLink.AutoSize = true;
            this.gpgLabelKBLink.AutoStyle = true;
            this.gpgLabelKBLink.Font = new Font("Arial", 9.75f);
            this.gpgLabelKBLink.ForeColor = Color.White;
            this.gpgLabelKBLink.IgnoreMouseWheel = false;
            this.gpgLabelKBLink.IsStyled = false;
            this.gpgLabelKBLink.Location = new Point(0x20a, 0x16);
            this.gpgLabelKBLink.Name = "gpgLabelKBLink";
            this.gpgLabelKBLink.Size = new Size(0xc9, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelKBLink, null);
            this.gpgLabelKBLink.TabIndex = 9;
            this.gpgLabelKBLink.Text = "<LOC>Click here for more details";
            this.gpgLabelKBLink.TextStyle = TextStyles.Link;
            this.gpgLabelKBLink.Click += new EventHandler(this.gpgLabelKBLink_Click);
            this.gpgLabelBeginDate.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelBeginDate.AutoSize = true;
            this.gpgLabelBeginDate.AutoStyle = true;
            this.gpgLabelBeginDate.Font = new Font("Arial", 9.75f);
            this.gpgLabelBeginDate.ForeColor = Color.White;
            this.gpgLabelBeginDate.IgnoreMouseWheel = false;
            this.gpgLabelBeginDate.IsStyled = false;
            this.gpgLabelBeginDate.Location = new Point(12, 0x52);
            this.gpgLabelBeginDate.Name = "gpgLabelBeginDate";
            this.gpgLabelBeginDate.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelBeginDate, null);
            this.gpgLabelBeginDate.TabIndex = 11;
            this.gpgLabelBeginDate.Text = "gpgLabel2";
            this.gpgLabelBeginDate.TextStyle = TextStyles.Default;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(6, 0x42);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x72, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 10;
            this.gpgLabel3.Text = "<LOC>Begin Date";
            this.gpgLabel3.TextStyle = TextStyles.Header1;
            this.gpgLabelEndDate.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelEndDate.AutoSize = true;
            this.gpgLabelEndDate.AutoStyle = true;
            this.gpgLabelEndDate.Font = new Font("Arial", 9.75f);
            this.gpgLabelEndDate.ForeColor = Color.White;
            this.gpgLabelEndDate.IgnoreMouseWheel = false;
            this.gpgLabelEndDate.IsStyled = false;
            this.gpgLabelEndDate.Location = new Point(0x11e, 0x52);
            this.gpgLabelEndDate.Name = "gpgLabelEndDate";
            this.gpgLabelEndDate.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelEndDate, null);
            this.gpgLabelEndDate.TabIndex = 13;
            this.gpgLabelEndDate.Text = "gpgLabel4";
            this.gpgLabelEndDate.TextStyle = TextStyles.Default;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(280, 0x42);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x68, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 12;
            this.gpgLabel5.Text = "<LOC>End Date";
            this.gpgLabel5.TextStyle = TextStyles.Header1;
            this.gpgLabelTimeZone.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelTimeZone.AutoSize = true;
            this.gpgLabelTimeZone.AutoStyle = true;
            this.gpgLabelTimeZone.Font = new Font("Arial", 9.75f);
            this.gpgLabelTimeZone.ForeColor = Color.White;
            this.gpgLabelTimeZone.IgnoreMouseWheel = false;
            this.gpgLabelTimeZone.IsStyled = false;
            this.gpgLabelTimeZone.Location = new Point(0x210, 0x52);
            this.gpgLabelTimeZone.Name = "gpgLabelTimeZone";
            this.gpgLabelTimeZone.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelTimeZone, null);
            this.gpgLabelTimeZone.TabIndex = 15;
            this.gpgLabelTimeZone.Text = "gpgLabel6";
            this.gpgLabelTimeZone.TextStyle = TextStyles.Default;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoSize = true;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x20a, 0x42);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(110, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 14;
            this.gpgLabel7.Text = "<LOC>Time Zone";
            this.gpgLabel7.TextStyle = TextStyles.Header1;
            this.gpgLabelTimes.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelTimes.AutoSize = true;
            this.gpgLabelTimes.AutoStyle = true;
            this.gpgLabelTimes.Font = new Font("Arial", 9.75f);
            this.gpgLabelTimes.ForeColor = Color.White;
            this.gpgLabelTimes.IgnoreMouseWheel = false;
            this.gpgLabelTimes.IsStyled = false;
            this.gpgLabelTimes.Location = new Point(12, 0x7f);
            this.gpgLabelTimes.Name = "gpgLabelTimes";
            this.gpgLabelTimes.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelTimes, null);
            this.gpgLabelTimes.TabIndex = 0x11;
            this.gpgLabelTimes.Text = "gpgLabel8";
            this.gpgLabelTimes.TextStyle = TextStyles.Default;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoSize = true;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.Font = new Font("Arial", 9.75f);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(6, 0x6f);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0xc4, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel9, null);
            this.gpgLabel9.TabIndex = 0x10;
            this.gpgLabel9.Text = "<LOC>Desired Time(s) Available";
            this.gpgLabel9.TextStyle = TextStyles.Header1;
            this.gpgLabelDays.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDays.AutoSize = true;
            this.gpgLabelDays.AutoStyle = true;
            this.gpgLabelDays.Font = new Font("Arial", 9.75f);
            this.gpgLabelDays.ForeColor = Color.White;
            this.gpgLabelDays.IgnoreMouseWheel = false;
            this.gpgLabelDays.IsStyled = false;
            this.gpgLabelDays.Location = new Point(0x11e, 0x7f);
            this.gpgLabelDays.Name = "gpgLabelDays";
            this.gpgLabelDays.Size = new Size(0x4a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelDays, null);
            this.gpgLabelDays.TabIndex = 0x15;
            this.gpgLabelDays.Text = "gpgLabel12";
            this.gpgLabelDays.TextStyle = TextStyles.Default;
            this.gpgLabel13.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel13.AutoSize = true;
            this.gpgLabel13.AutoStyle = true;
            this.gpgLabel13.Font = new Font("Arial", 9.75f);
            this.gpgLabel13.ForeColor = Color.White;
            this.gpgLabel13.IgnoreMouseWheel = false;
            this.gpgLabel13.IsStyled = false;
            this.gpgLabel13.Location = new Point(280, 0x6f);
            this.gpgLabel13.Name = "gpgLabel13";
            this.gpgLabel13.Size = new Size(0xbf, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel13, null);
            this.gpgLabel13.TabIndex = 20;
            this.gpgLabel13.Text = "<LOC>Desired Day(s) Available";
            this.gpgLabel13.TextStyle = TextStyles.Header1;
            this.gpgLabelHeadOfEffort.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelHeadOfEffort.AutoSize = true;
            this.gpgLabelHeadOfEffort.AutoStyle = true;
            this.gpgLabelHeadOfEffort.Font = new Font("Arial", 9.75f);
            this.gpgLabelHeadOfEffort.ForeColor = Color.White;
            this.gpgLabelHeadOfEffort.IgnoreMouseWheel = false;
            this.gpgLabelHeadOfEffort.IsStyled = false;
            this.gpgLabelHeadOfEffort.Location = new Point(0x11e, 0x26);
            this.gpgLabelHeadOfEffort.Name = "gpgLabelHeadOfEffort";
            this.gpgLabelHeadOfEffort.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelHeadOfEffort, null);
            this.gpgLabelHeadOfEffort.TabIndex = 0x17;
            this.gpgLabelHeadOfEffort.Text = "gpgLabel2";
            this.gpgLabelHeadOfEffort.TextStyle = TextStyles.Default;
            this.gpgLabel15.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel15.AutoSize = true;
            this.gpgLabel15.AutoStyle = true;
            this.gpgLabel15.Font = new Font("Arial", 9.75f);
            this.gpgLabel15.ForeColor = Color.White;
            this.gpgLabel15.IgnoreMouseWheel = false;
            this.gpgLabel15.IsStyled = false;
            this.gpgLabel15.Location = new Point(280, 0x16);
            this.gpgLabel15.Name = "gpgLabel15";
            this.gpgLabel15.Size = new Size(0x80, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel15, null);
            this.gpgLabel15.TabIndex = 0x16;
            this.gpgLabel15.Text = "<LOC>Head of Effort";
            this.gpgLabel15.TextStyle = TextStyles.Header1;
            this.gpgGroupBox1.Controls.Add(this.gpgLabel2);
            this.gpgGroupBox1.Controls.Add(this.gpgPanelInput);
            this.gpgGroupBox1.Controls.Add(this.gpgCheckBoxLegalAgree);
            this.gpgGroupBox1.Location = new Point(12, 0x15f);
            this.gpgGroupBox1.Name = "gpgGroupBox1";
            this.gpgGroupBox1.Size = new Size(0x317, 240);
            base.ttDefault.SetSuperTip(this.gpgGroupBox1, null);
            this.gpgGroupBox1.TabIndex = 0x18;
            this.gpgGroupBox1.TabStop = false;
            this.gpgGroupBox1.Text = "<LOC>Your Information";
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.Gainsboro;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x143, 9);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x1ce, 50);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 0x21;
            this.gpgLabel2.Text = manager.GetString("gpgLabel2.Text");
            this.gpgLabel2.TextStyle = TextStyles.Descriptor;
            this.gpgPanelInput.Controls.Add(this.gpgTextBoxEmail);
            this.gpgPanelInput.Controls.Add(this.gpgLabel4);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxTime3);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxTime2);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxTime1);
            this.gpgPanelInput.Controls.Add(this.gpgLabelTimesAvailable);
            this.gpgPanelInput.Controls.Add(this.skinButtonOK);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxDay7);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxDay6);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxDay5);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxDay4);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxDay3);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxDay2);
            this.gpgPanelInput.Controls.Add(this.gpgCheckBoxDay1);
            this.gpgPanelInput.Controls.Add(this.gpgLabelDaysAvailable);
            this.gpgPanelInput.Location = new Point(6, 0x3e);
            this.gpgPanelInput.Name = "gpgPanelInput";
            this.gpgPanelInput.Size = new Size(0x30b, 0xab);
            base.ttDefault.SetSuperTip(this.gpgPanelInput, null);
            this.gpgPanelInput.TabIndex = 1;
            this.gpgPanelInput.Visible = false;
            this.gpgTextBoxEmail.Location = new Point(3, 0x90);
            this.gpgTextBoxEmail.Name = "gpgTextBoxEmail";
            this.gpgTextBoxEmail.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxEmail.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxEmail.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxEmail.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxEmail.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxEmail.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxEmail.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxEmail.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxEmail.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxEmail.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxEmail.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxEmail.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxEmail.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxEmail.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxEmail.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxEmail.Properties.MaxLength = 0xff;
            this.gpgTextBoxEmail.Size = new Size(0x1b6, 20);
            this.gpgTextBoxEmail.TabIndex = 0x25;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.Gainsboro;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0, 0x7d);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x87, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 0x24;
            this.gpgLabel4.Text = "<LOC>Email Address";
            this.gpgLabel4.TextStyle = TextStyles.Header1;
            this.gpgCheckBoxTime3.AutoSize = true;
            this.gpgCheckBoxTime3.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxTime3.Location = new Point(0x133, 0x17);
            this.gpgCheckBoxTime3.Name = "gpgCheckBoxTime3";
            this.gpgCheckBoxTime3.Size = new Size(0x77, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxTime3, null);
            this.gpgCheckBoxTime3.TabIndex = 0x23;
            this.gpgCheckBoxTime3.Text = "<LOC>Evenings";
            this.gpgCheckBoxTime3.UsesBG = false;
            this.gpgCheckBoxTime3.UseVisualStyleBackColor = true;
            this.gpgCheckBoxTime2.AutoSize = true;
            this.gpgCheckBoxTime2.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxTime2.Location = new Point(160, 0x17);
            this.gpgCheckBoxTime2.Name = "gpgCheckBoxTime2";
            this.gpgCheckBoxTime2.Size = new Size(130, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxTime2, null);
            this.gpgCheckBoxTime2.TabIndex = 0x22;
            this.gpgCheckBoxTime2.Text = "<LOC>Afternoons";
            this.gpgCheckBoxTime2.UsesBG = false;
            this.gpgCheckBoxTime2.UseVisualStyleBackColor = true;
            this.gpgCheckBoxTime1.AutoSize = true;
            this.gpgCheckBoxTime1.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxTime1.Location = new Point(3, 0x17);
            this.gpgCheckBoxTime1.Name = "gpgCheckBoxTime1";
            this.gpgCheckBoxTime1.Size = new Size(0x77, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxTime1, null);
            this.gpgCheckBoxTime1.TabIndex = 0x21;
            this.gpgCheckBoxTime1.Text = "<LOC>Mornings";
            this.gpgCheckBoxTime1.UsesBG = false;
            this.gpgCheckBoxTime1.UseVisualStyleBackColor = true;
            this.gpgLabelTimesAvailable.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelTimesAvailable.AutoSize = true;
            this.gpgLabelTimesAvailable.AutoStyle = true;
            this.gpgLabelTimesAvailable.Font = new Font("Arial", 9.75f);
            this.gpgLabelTimesAvailable.ForeColor = Color.Gainsboro;
            this.gpgLabelTimesAvailable.IgnoreMouseWheel = false;
            this.gpgLabelTimesAvailable.IsStyled = false;
            this.gpgLabelTimesAvailable.Location = new Point(0, 4);
            this.gpgLabelTimesAvailable.Name = "gpgLabelTimesAvailable";
            this.gpgLabelTimesAvailable.Size = new Size(0x94, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelTimesAvailable, null);
            this.gpgLabelTimesAvailable.TabIndex = 0x20;
            this.gpgLabelTimesAvailable.Text = "<LOC>Time(s) Available";
            this.gpgLabelTimesAvailable.TextStyle = TextStyles.Header1;
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
            this.skinButtonOK.Location = new Point(0x28b, 0x8a);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x7d, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 0x1f;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>Submit";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.gpgCheckBoxDay7.AutoSize = true;
            this.gpgCheckBoxDay7.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxDay7.Location = new Point(0x133, 0x5f);
            this.gpgCheckBoxDay7.Name = "gpgCheckBoxDay7";
            this.gpgCheckBoxDay7.Size = new Size(0x6f, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay7, null);
            this.gpgCheckBoxDay7.TabIndex = 30;
            this.gpgCheckBoxDay7.Text = "<LOC>Sunday";
            this.gpgCheckBoxDay7.UsesBG = false;
            this.gpgCheckBoxDay7.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay6.AutoSize = true;
            this.gpgCheckBoxDay6.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxDay6.Location = new Point(160, 0x5f);
            this.gpgCheckBoxDay6.Name = "gpgCheckBoxDay6";
            this.gpgCheckBoxDay6.Size = new Size(120, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay6, null);
            this.gpgCheckBoxDay6.TabIndex = 0x1d;
            this.gpgCheckBoxDay6.Text = "<LOC>Saturday";
            this.gpgCheckBoxDay6.UsesBG = false;
            this.gpgCheckBoxDay6.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay5.AutoSize = true;
            this.gpgCheckBoxDay5.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxDay5.Location = new Point(3, 0x5f);
            this.gpgCheckBoxDay5.Name = "gpgCheckBoxDay5";
            this.gpgCheckBoxDay5.Size = new Size(0x67, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay5, null);
            this.gpgCheckBoxDay5.TabIndex = 0x1c;
            this.gpgCheckBoxDay5.Text = "<LOC>Friday";
            this.gpgCheckBoxDay5.UsesBG = false;
            this.gpgCheckBoxDay5.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay4.AutoSize = true;
            this.gpgCheckBoxDay4.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxDay4.Location = new Point(0x1d5, 0x48);
            this.gpgCheckBoxDay4.Name = "gpgCheckBoxDay4";
            this.gpgCheckBoxDay4.Size = new Size(0x79, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay4, null);
            this.gpgCheckBoxDay4.TabIndex = 0x1b;
            this.gpgCheckBoxDay4.Text = "<LOC>Thursday";
            this.gpgCheckBoxDay4.UsesBG = false;
            this.gpgCheckBoxDay4.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay3.AutoSize = true;
            this.gpgCheckBoxDay3.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxDay3.Location = new Point(0x133, 0x48);
            this.gpgCheckBoxDay3.Name = "gpgCheckBoxDay3";
            this.gpgCheckBoxDay3.Size = new Size(0x86, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay3, null);
            this.gpgCheckBoxDay3.TabIndex = 0x1a;
            this.gpgCheckBoxDay3.Text = "<LOC>Wednesday";
            this.gpgCheckBoxDay3.UsesBG = false;
            this.gpgCheckBoxDay3.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay2.AutoSize = true;
            this.gpgCheckBoxDay2.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxDay2.Location = new Point(160, 0x48);
            this.gpgCheckBoxDay2.Name = "gpgCheckBoxDay2";
            this.gpgCheckBoxDay2.Size = new Size(0x74, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay2, null);
            this.gpgCheckBoxDay2.TabIndex = 0x19;
            this.gpgCheckBoxDay2.Text = "<LOC>Tuesday";
            this.gpgCheckBoxDay2.UsesBG = false;
            this.gpgCheckBoxDay2.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay1.AutoSize = true;
            this.gpgCheckBoxDay1.ForeColor = Color.Gainsboro;
            this.gpgCheckBoxDay1.Location = new Point(3, 0x48);
            this.gpgCheckBoxDay1.Name = "gpgCheckBoxDay1";
            this.gpgCheckBoxDay1.Size = new Size(0x70, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay1, null);
            this.gpgCheckBoxDay1.TabIndex = 0x18;
            this.gpgCheckBoxDay1.Text = "<LOC>Monday";
            this.gpgCheckBoxDay1.UsesBG = false;
            this.gpgCheckBoxDay1.UseVisualStyleBackColor = true;
            this.gpgLabelDaysAvailable.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDaysAvailable.AutoSize = true;
            this.gpgLabelDaysAvailable.AutoStyle = true;
            this.gpgLabelDaysAvailable.Font = new Font("Arial", 9.75f);
            this.gpgLabelDaysAvailable.ForeColor = Color.Gainsboro;
            this.gpgLabelDaysAvailable.IgnoreMouseWheel = false;
            this.gpgLabelDaysAvailable.IsStyled = false;
            this.gpgLabelDaysAvailable.Location = new Point(0, 0x35);
            this.gpgLabelDaysAvailable.Name = "gpgLabelDaysAvailable";
            this.gpgLabelDaysAvailable.Size = new Size(0x8f, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelDaysAvailable, null);
            this.gpgLabelDaysAvailable.TabIndex = 0x17;
            this.gpgLabelDaysAvailable.Text = "<LOC>Day(s) Available";
            this.gpgLabelDaysAvailable.TextStyle = TextStyles.Header1;
            this.gpgCheckBoxLegalAgree.AutoSize = true;
            this.gpgCheckBoxLegalAgree.Location = new Point(6, 0x1f);
            this.gpgCheckBoxLegalAgree.Name = "gpgCheckBoxLegalAgree";
            this.gpgCheckBoxLegalAgree.Size = new Size(0xff, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxLegalAgree, null);
            this.gpgCheckBoxLegalAgree.TabIndex = 0;
            this.gpgCheckBoxLegalAgree.Text = "<LOC>I agree to the legal notice above";
            this.gpgCheckBoxLegalAgree.UsesBG = false;
            this.gpgCheckBoxLegalAgree.UseVisualStyleBackColor = true;
            this.gpgCheckBoxLegalAgree.CheckedChanged += new EventHandler(this.gpgCheckBoxLegalAgree_CheckedChanged);
            this.gpgGroupBox2.Controls.Add(this.gpgRichLabelLegal);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelLegalKB);
            this.gpgGroupBox2.Controls.Add(this.gpgLabel1);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelDescription);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelHeadOfEffort);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelKBLink);
            this.gpgGroupBox2.Controls.Add(this.gpgLabel15);
            this.gpgGroupBox2.Controls.Add(this.gpgLabel3);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelDays);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelBeginDate);
            this.gpgGroupBox2.Controls.Add(this.gpgLabel13);
            this.gpgGroupBox2.Controls.Add(this.gpgLabel5);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelEndDate);
            this.gpgGroupBox2.Controls.Add(this.gpgLabel7);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelTimes);
            this.gpgGroupBox2.Controls.Add(this.gpgLabelTimeZone);
            this.gpgGroupBox2.Controls.Add(this.gpgLabel9);
            this.gpgGroupBox2.Location = new Point(12, 0x53);
            this.gpgGroupBox2.Name = "gpgGroupBox2";
            this.gpgGroupBox2.Size = new Size(0x317, 0x106);
            base.ttDefault.SetSuperTip(this.gpgGroupBox2, null);
            this.gpgGroupBox2.TabIndex = 0x19;
            this.gpgGroupBox2.TabStop = false;
            this.gpgGroupBox2.Text = "<LOC>Effort Details";
            this.gpgRichLabelLegal.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgRichLabelLegal.AutoStyle = true;
            this.gpgRichLabelLegal.BackColor = Color.White;
            this.gpgRichLabelLegal.BorderStyle = BorderStyle.None;
            this.gpgRichLabelLegal.IsStyled = false;
            this.gpgRichLabelLegal.Location = new Point(9, 0xb3);
            this.gpgRichLabelLegal.Name = "gpgRichLabelLegal";
            this.gpgRichLabelLegal.ReadOnly = true;
            this.gpgRichLabelLegal.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.gpgRichLabelLegal.Size = new Size(0x308, 0x4d);
            base.ttDefault.SetSuperTip(this.gpgRichLabelLegal, null);
            this.gpgRichLabelLegal.TabIndex = 0x1a;
            this.gpgRichLabelLegal.Text = "";
            this.gpgLabelLegalKB.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelLegalKB.AutoSize = true;
            this.gpgLabelLegalKB.AutoStyle = true;
            this.gpgLabelLegalKB.Font = new Font("Arial", 9.75f);
            this.gpgLabelLegalKB.ForeColor = Color.White;
            this.gpgLabelLegalKB.IgnoreMouseWheel = false;
            this.gpgLabelLegalKB.IsStyled = false;
            this.gpgLabelLegalKB.Location = new Point(6, 160);
            this.gpgLabelLegalKB.Name = "gpgLabelLegalKB";
            this.gpgLabelLegalKB.Size = new Size(0x7a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelLegalKB, null);
            this.gpgLabelLegalKB.TabIndex = 0x19;
            this.gpgLabelLegalKB.Text = "<LOC>Legal Notice";
            this.gpgLabelLegalKB.TextStyle = TextStyles.Link;
            this.gpgLabelLegalKB.Click += new EventHandler(this.gpgLabelLegalKB_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x32f, 0x287);
            base.Controls.Add(this.gpgGroupBox2);
            base.Controls.Add(this.gpgGroupBox1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x32f, 0x287);
            this.MinimumSize = new Size(0x32f, 0x287);
            base.Name = "DlgVolunteer";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Volunteer Effort Details";
            base.Controls.SetChildIndex(this.gpgGroupBox1, 0);
            base.Controls.SetChildIndex(this.gpgGroupBox2, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgGroupBox1.ResumeLayout(false);
            this.gpgGroupBox1.PerformLayout();
            this.gpgPanelInput.ResumeLayout(false);
            this.gpgPanelInput.PerformLayout();
            this.gpgTextBoxEmail.Properties.EndInit();
            this.gpgGroupBox2.ResumeLayout(false);
            this.gpgGroupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ActiveForms.Remove(this.Effort.ID);
            base.OnClosing(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.HasVolunteered)
            {
                base.DialogResult = DialogResult.OK;
            }
            base.OnFormClosing(e);
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.skinButtonOK.Enabled = false;
                base.ClearErrors();
                bool flag = false;
                if ((this.gpgTextBoxEmail.Text == null) || (this.gpgTextBoxEmail.Text.Length < 1))
                {
                    base.Error(this.gpgTextBoxEmail, "<LOC>Field may not be blank.", new object[0]);
                    flag = true;
                }
                if (!((((!this.Effort.RequiresDays || this.gpgCheckBoxDay1.Checked) || (this.gpgCheckBoxDay2.Checked || this.gpgCheckBoxDay3.Checked)) || ((this.gpgCheckBoxDay4.Checked || this.gpgCheckBoxDay5.Checked) || this.gpgCheckBoxDay6.Checked)) || this.gpgCheckBoxDay7.Checked))
                {
                    base.Error(this.gpgLabelDaysAvailable, "<LOC>At least one day must be checked.", new object[0]);
                    flag = true;
                }
                if (!(((!this.Effort.RequiresTimes || this.gpgCheckBoxTime1.Checked) || this.gpgCheckBoxTime2.Checked) || this.gpgCheckBoxTime3.Checked))
                {
                    base.Error(this.gpgLabelTimesAvailable, "<LOC>At least one time must be checked.", new object[0]);
                    flag = true;
                }
                if (flag)
                {
                    this.skinButtonOK.Enabled = true;
                }
                else if ((this.Effort.MinimumAge > 0) && (new DlgYesNo(base.MainForm, "<LOC>Age Restriction", string.Format("<LOC>To participate in this volunteer effort you must be at least {0} years old. Are you at least {0} years old?", this.Effort.MinimumAge)).ShowDialog() != DialogResult.Yes))
                {
                    base.Close();
                }
                else
                {
                    int num;
                    string str = "";
                    CheckBox[] boxArray = new CheckBox[] { this.gpgCheckBoxTime1, this.gpgCheckBoxTime2, this.gpgCheckBoxTime3 };
                    for (num = 0; num < boxArray.Length; num++)
                    {
                        if (boxArray[num].Checked)
                        {
                            str = str + boxArray[num].Name.Replace("gpgCheckBoxTime", "") + ",";
                        }
                    }
                    str = str.TrimEnd(new char[] { ',' });
                    string str2 = "";
                    boxArray = new CheckBox[] { this.gpgCheckBoxDay1, this.gpgCheckBoxDay2, this.gpgCheckBoxDay3, this.gpgCheckBoxDay4, this.gpgCheckBoxDay5, this.gpgCheckBoxDay6, this.gpgCheckBoxDay7 };
                    for (num = 0; num < boxArray.Length; num++)
                    {
                        if (boxArray[num].Checked)
                        {
                            str2 = str2 + boxArray[num].Name.Replace("gpgCheckBoxDay", "") + ",";
                        }
                    }
                    str2 = str2.TrimEnd(new char[] { ',' });
                    if (!DataAccess.ExecuteQuery("AddVolunteerToEffort", new object[] { this.Effort.ID, str, str2, this.gpgTextBoxEmail.Text }))
                    {
                        base.Error(this.skinButtonOK, "<LOC>An error has occured, please try again later.", new object[0]);
                        this.skinButtonOK.Enabled = true;
                    }
                    else
                    {
                        base.SetStatus("<LOC>Thank you for volunteering!", new object[0]);
                        if (!(((base.MainForm.DlgActiveEfforts == null) || base.MainForm.DlgActiveEfforts.Disposing) || base.MainForm.DlgActiveEfforts.IsDisposed))
                        {
                            base.MainForm.DlgActiveEfforts.RefreshEfforts();
                        }
                        this.HasVolunteered = true;
                        if (callBack == null)
                        {
                            callBack = delegate (object s) {
                                VGen0 method = null;
                                try
                                {
                                    Thread.Sleep(0xbb8);
                                    if (!base.Disposing && !base.IsDisposed)
                                    {
                                        if (method == null)
                                        {
                                            method = delegate {
                                                base.DialogResult = DialogResult.OK;
                                                base.Close();
                                            };
                                        }
                                        base.BeginInvoke(method);
                                    }
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            };
                        }
                        ThreadPool.QueueUserWorkItem(callBack);
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        public override bool AllowRestoreWindow
        {
            get
            {
                return false;
            }
        }

        public VolunteerEffort Effort
        {
            get
            {
                return this.mEffort;
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return true;
            }
        }
    }
}

