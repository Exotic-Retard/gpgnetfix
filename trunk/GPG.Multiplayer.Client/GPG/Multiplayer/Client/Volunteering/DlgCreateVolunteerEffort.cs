namespace GPG.Multiplayer.Client.Volunteering
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgCreateVolunteerEffort : DlgBase
    {
        private ComboBox comboBoxTimeZone;
        private IContainer components = null;
        private DateTimePicker dateTimePickerBegin;
        private DateTimePicker dateTimePickerEnd;
        private GPGCheckBox gpgCheckBoxDay1;
        private GPGCheckBox gpgCheckBoxDay2;
        private GPGCheckBox gpgCheckBoxDay3;
        private GPGCheckBox gpgCheckBoxDay4;
        private GPGCheckBox gpgCheckBoxDay5;
        private GPGCheckBox gpgCheckBoxDay6;
        private GPGCheckBox gpgCheckBoxDay7;
        private GPGCheckBox gpgCheckBoxMinAge;
        private GPGCheckBox gpgCheckBoxRequireDay;
        private GPGCheckBox gpgCheckBoxRequireTime;
        private GPGCheckBox gpgCheckBoxTime1;
        private GPGCheckBox gpgCheckBoxTime2;
        private GPGCheckBox gpgCheckBoxTime3;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel12;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGTextBox gpgTextBoxDescription;
        private GPGTextBox gpgTextBoxDetailKB;
        private GPGTextBox gpgTextBoxHeadOfEffort;
        private GPGTextBox gpgTextBoxLegalKB;
        private NumericUpDown numericUpDownAge;
        private SkinButton skinButtonOK;

        public DlgCreateVolunteerEffort()
        {
            this.InitializeComponent();
            this.comboBoxTimeZone.DropDownWidth = this.comboBoxTimeZone.Width;
            SortedList<int, string> list = new SortedList<int, string>();
            string str = null;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones");
            string[] subKeyNames = key.GetSubKeyNames();
            foreach (string str2 in subKeyNames)
            {
                RegistryKey key2 = key.OpenSubKey(str2);
                object obj2 = key2.GetValue("Index", null);
                object obj3 = key2.GetValue("Display", null);
                if ((obj2 != null) && (obj3 != null))
                {
                    list.Add((int) obj2, (string) obj3);
                    if (((string) obj3) == "(GMT-08:00) Pacific Time (US & Canada)")
                    {
                        str = (string) obj3;
                    }
                }
            }
            string[] array = new string[list.Values.Count];
            list.Values.CopyTo(array, 0);
            this.comboBoxTimeZone.Items.AddRange(array);
            if (str != null)
            {
                this.comboBoxTimeZone.SelectedIndex = Array.IndexOf<string>(array, str);
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

        private void gpgCheckBoxMinAge_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDownAge.Enabled = this.gpgCheckBoxMinAge.Checked;
        }

        private void InitializeComponent()
        {
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxDescription = new GPGTextBox();
            this.gpgTextBoxLegalKB = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxDetailKB = new GPGTextBox();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.dateTimePickerBegin = new DateTimePicker();
            this.dateTimePickerEnd = new DateTimePicker();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgCheckBoxDay7 = new GPGCheckBox();
            this.gpgCheckBoxDay6 = new GPGCheckBox();
            this.gpgCheckBoxDay5 = new GPGCheckBox();
            this.gpgCheckBoxDay4 = new GPGCheckBox();
            this.gpgCheckBoxDay3 = new GPGCheckBox();
            this.gpgCheckBoxDay2 = new GPGCheckBox();
            this.gpgCheckBoxDay1 = new GPGCheckBox();
            this.gpgLabel12 = new GPGLabel();
            this.gpgTextBoxHeadOfEffort = new GPGTextBox();
            this.skinButtonOK = new SkinButton();
            this.comboBoxTimeZone = new ComboBox();
            this.gpgCheckBoxTime3 = new GPGCheckBox();
            this.gpgCheckBoxTime2 = new GPGCheckBox();
            this.gpgCheckBoxTime1 = new GPGCheckBox();
            this.gpgCheckBoxMinAge = new GPGCheckBox();
            this.numericUpDownAge = new NumericUpDown();
            this.gpgCheckBoxRequireTime = new GPGCheckBox();
            this.gpgCheckBoxRequireDay = new GPGCheckBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxDescription.Properties.BeginInit();
            this.gpgTextBoxLegalKB.Properties.BeginInit();
            this.gpgTextBoxDetailKB.Properties.BeginInit();
            this.gpgTextBoxHeadOfEffort.Properties.BeginInit();
            this.numericUpDownAge.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x259, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = System.Drawing.Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 80);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x49, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "Description";
            this.gpgLabel1.TextStyle = TextStyles.Header1;
            this.gpgTextBoxDescription.Location = new Point(15, 0x63);
            this.gpgTextBoxDescription.Name = "gpgTextBoxDescription";
            this.gpgTextBoxDescription.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.gpgTextBoxDescription.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxDescription.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.gpgTextBoxDescription.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxDescription.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxDescription.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxDescription.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxDescription.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.FromArgb(0, 0, 0);
            this.gpgTextBoxDescription.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxDescription.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxDescription.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxDescription.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxDescription.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxDescription.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxDescription.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxDescription.Size = new Size(0x267, 20);
            this.gpgTextBoxDescription.TabIndex = 8;
            this.gpgTextBoxLegalKB.Location = new Point(15, 150);
            this.gpgTextBoxLegalKB.Name = "gpgTextBoxLegalKB";
            this.gpgTextBoxLegalKB.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.gpgTextBoxLegalKB.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxLegalKB.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.gpgTextBoxLegalKB.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxLegalKB.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxLegalKB.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxLegalKB.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxLegalKB.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.FromArgb(0, 0, 0);
            this.gpgTextBoxLegalKB.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxLegalKB.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxLegalKB.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxLegalKB.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxLegalKB.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxLegalKB.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxLegalKB.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxLegalKB.Size = new Size(0x75, 20);
            this.gpgTextBoxLegalKB.TabIndex = 10;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = System.Drawing.Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0x83);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x3d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "Legal KB";
            this.gpgLabel2.TextStyle = TextStyles.Header1;
            this.gpgTextBoxDetailKB.Location = new Point(0xa9, 150);
            this.gpgTextBoxDetailKB.Name = "gpgTextBoxDetailKB";
            this.gpgTextBoxDetailKB.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.gpgTextBoxDetailKB.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxDetailKB.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.gpgTextBoxDetailKB.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxDetailKB.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxDetailKB.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxDetailKB.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxDetailKB.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.FromArgb(0, 0, 0);
            this.gpgTextBoxDetailKB.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxDetailKB.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxDetailKB.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxDetailKB.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxDetailKB.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxDetailKB.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxDetailKB.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxDetailKB.Size = new Size(0x74, 20);
            this.gpgTextBoxDetailKB.TabIndex = 12;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = System.Drawing.Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0xa6, 0x83);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(70, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 11;
            this.gpgLabel3.Text = "Details KB";
            this.gpgLabel3.TextStyle = TextStyles.Header1;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = System.Drawing.Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(13, 0xbd);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x48, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 13;
            this.gpgLabel4.Text = "Begin Date";
            this.gpgLabel4.TextStyle = TextStyles.Header1;
            this.dateTimePickerBegin.Format = DateTimePickerFormat.Short;
            this.dateTimePickerBegin.Location = new Point(0x10, 0xd0);
            this.dateTimePickerBegin.Name = "dateTimePickerBegin";
            this.dateTimePickerBegin.Size = new Size(0x74, 20);
            base.ttDefault.SetSuperTip(this.dateTimePickerBegin, null);
            this.dateTimePickerBegin.TabIndex = 14;
            this.dateTimePickerEnd.Format = DateTimePickerFormat.Short;
            this.dateTimePickerEnd.Location = new Point(0xa9, 0xd0);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new Size(0x74, 20);
            base.ttDefault.SetSuperTip(this.dateTimePickerEnd, null);
            this.dateTimePickerEnd.TabIndex = 0x10;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = System.Drawing.Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0xa6, 0xbd);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x3e, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 15;
            this.gpgLabel5.Text = "End Date";
            this.gpgLabel5.TextStyle = TextStyles.Header1;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = System.Drawing.Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x13d, 0xbd);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x44, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x12;
            this.gpgLabel6.Text = "Time Zone";
            this.gpgLabel6.TextStyle = TextStyles.Header1;
            this.gpgCheckBoxDay7.AutoSize = true;
            this.gpgCheckBoxDay7.Location = new Point(320, 350);
            this.gpgCheckBoxDay7.Name = "gpgCheckBoxDay7";
            this.gpgCheckBoxDay7.Size = new Size(0x6f, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay7, null);
            this.gpgCheckBoxDay7.TabIndex = 0x25;
            this.gpgCheckBoxDay7.Text = "<LOC>Sunday";
            this.gpgCheckBoxDay7.UsesBG = false;
            this.gpgCheckBoxDay7.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay6.AutoSize = true;
            this.gpgCheckBoxDay6.Location = new Point(0xad, 350);
            this.gpgCheckBoxDay6.Name = "gpgCheckBoxDay6";
            this.gpgCheckBoxDay6.Size = new Size(120, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay6, null);
            this.gpgCheckBoxDay6.TabIndex = 0x24;
            this.gpgCheckBoxDay6.Text = "<LOC>Saturday";
            this.gpgCheckBoxDay6.UsesBG = false;
            this.gpgCheckBoxDay6.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay5.AutoSize = true;
            this.gpgCheckBoxDay5.Location = new Point(0x10, 350);
            this.gpgCheckBoxDay5.Name = "gpgCheckBoxDay5";
            this.gpgCheckBoxDay5.Size = new Size(0x67, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay5, null);
            this.gpgCheckBoxDay5.TabIndex = 0x23;
            this.gpgCheckBoxDay5.Text = "<LOC>Friday";
            this.gpgCheckBoxDay5.UsesBG = false;
            this.gpgCheckBoxDay5.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay4.AutoSize = true;
            this.gpgCheckBoxDay4.Location = new Point(0x1e2, 0x147);
            this.gpgCheckBoxDay4.Name = "gpgCheckBoxDay4";
            this.gpgCheckBoxDay4.Size = new Size(0x79, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay4, null);
            this.gpgCheckBoxDay4.TabIndex = 0x22;
            this.gpgCheckBoxDay4.Text = "<LOC>Thursday";
            this.gpgCheckBoxDay4.UsesBG = false;
            this.gpgCheckBoxDay4.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay3.AutoSize = true;
            this.gpgCheckBoxDay3.Location = new Point(320, 0x147);
            this.gpgCheckBoxDay3.Name = "gpgCheckBoxDay3";
            this.gpgCheckBoxDay3.Size = new Size(0x86, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay3, null);
            this.gpgCheckBoxDay3.TabIndex = 0x21;
            this.gpgCheckBoxDay3.Text = "<LOC>Wednesday";
            this.gpgCheckBoxDay3.UsesBG = false;
            this.gpgCheckBoxDay3.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay2.AutoSize = true;
            this.gpgCheckBoxDay2.Location = new Point(0xad, 0x147);
            this.gpgCheckBoxDay2.Name = "gpgCheckBoxDay2";
            this.gpgCheckBoxDay2.Size = new Size(0x74, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay2, null);
            this.gpgCheckBoxDay2.TabIndex = 0x20;
            this.gpgCheckBoxDay2.Text = "<LOC>Tuesday";
            this.gpgCheckBoxDay2.UsesBG = false;
            this.gpgCheckBoxDay2.UseVisualStyleBackColor = true;
            this.gpgCheckBoxDay1.AutoSize = true;
            this.gpgCheckBoxDay1.Location = new Point(0x10, 0x147);
            this.gpgCheckBoxDay1.Name = "gpgCheckBoxDay1";
            this.gpgCheckBoxDay1.Size = new Size(0x70, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxDay1, null);
            this.gpgCheckBoxDay1.TabIndex = 0x1f;
            this.gpgCheckBoxDay1.Text = "<LOC>Monday";
            this.gpgCheckBoxDay1.UsesBG = false;
            this.gpgCheckBoxDay1.UseVisualStyleBackColor = true;
            this.gpgLabel12.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel12.AutoSize = true;
            this.gpgLabel12.AutoStyle = true;
            this.gpgLabel12.Font = new Font("Arial", 9.75f);
            this.gpgLabel12.ForeColor = System.Drawing.Color.White;
            this.gpgLabel12.IgnoreMouseWheel = false;
            this.gpgLabel12.IsStyled = false;
            this.gpgLabel12.Location = new Point(0x13c, 0x83);
            this.gpgLabel12.Name = "gpgLabel12";
            this.gpgLabel12.Size = new Size(0x56, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel12, null);
            this.gpgLabel12.TabIndex = 0x27;
            this.gpgLabel12.Text = "Head of Effort";
            this.gpgLabel12.TextStyle = TextStyles.Header1;
            this.gpgTextBoxHeadOfEffort.Location = new Point(0x13f, 150);
            this.gpgTextBoxHeadOfEffort.Name = "gpgTextBoxHeadOfEffort";
            this.gpgTextBoxHeadOfEffort.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            this.gpgTextBoxHeadOfEffort.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxHeadOfEffort.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            this.gpgTextBoxHeadOfEffort.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxHeadOfEffort.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxHeadOfEffort.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxHeadOfEffort.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxHeadOfEffort.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.FromArgb(0, 0, 0);
            this.gpgTextBoxHeadOfEffort.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxHeadOfEffort.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxHeadOfEffort.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxHeadOfEffort.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxHeadOfEffort.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxHeadOfEffort.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxHeadOfEffort.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxHeadOfEffort.Size = new Size(310, 20);
            this.gpgTextBoxHeadOfEffort.TabIndex = 0x26;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = System.Drawing.Color.Black;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = System.Drawing.Color.Gray;
            this.skinButtonOK.DrawColor = System.Drawing.Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = System.Drawing.Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = System.Drawing.Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x1f9, 0x17e);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x7d, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 40;
            this.skinButtonOK.Text = "Create";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.comboBoxTimeZone.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxTimeZone.FormattingEnabled = true;
            this.comboBoxTimeZone.ItemHeight = 13;
            this.comboBoxTimeZone.Location = new Point(320, 0xcf);
            this.comboBoxTimeZone.MaxDropDownItems = 0x10;
            this.comboBoxTimeZone.Name = "comboBoxTimeZone";
            this.comboBoxTimeZone.Size = new Size(310, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxTimeZone, null);
            this.comboBoxTimeZone.TabIndex = 0x29;
            this.gpgCheckBoxTime3.AutoSize = true;
            this.gpgCheckBoxTime3.Location = new Point(320, 0x10c);
            this.gpgCheckBoxTime3.Name = "gpgCheckBoxTime3";
            this.gpgCheckBoxTime3.Size = new Size(0x4d, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxTime3, null);
            this.gpgCheckBoxTime3.TabIndex = 0x2c;
            this.gpgCheckBoxTime3.Text = "Evenings";
            this.gpgCheckBoxTime3.UsesBG = false;
            this.gpgCheckBoxTime3.UseVisualStyleBackColor = true;
            this.gpgCheckBoxTime2.AutoSize = true;
            this.gpgCheckBoxTime2.Location = new Point(0xad, 0x10c);
            this.gpgCheckBoxTime2.Name = "gpgCheckBoxTime2";
            this.gpgCheckBoxTime2.Size = new Size(0x58, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxTime2, null);
            this.gpgCheckBoxTime2.TabIndex = 0x2b;
            this.gpgCheckBoxTime2.Text = "Afternoons";
            this.gpgCheckBoxTime2.UsesBG = false;
            this.gpgCheckBoxTime2.UseVisualStyleBackColor = true;
            this.gpgCheckBoxTime1.AutoSize = true;
            this.gpgCheckBoxTime1.Location = new Point(0x10, 0x10c);
            this.gpgCheckBoxTime1.Name = "gpgCheckBoxTime1";
            this.gpgCheckBoxTime1.Size = new Size(0x4d, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxTime1, null);
            this.gpgCheckBoxTime1.TabIndex = 0x2a;
            this.gpgCheckBoxTime1.Text = "Mornings";
            this.gpgCheckBoxTime1.UsesBG = false;
            this.gpgCheckBoxTime1.UseVisualStyleBackColor = true;
            this.gpgCheckBoxMinAge.AutoSize = true;
            this.gpgCheckBoxMinAge.Location = new Point(0x10, 0x187);
            this.gpgCheckBoxMinAge.Name = "gpgCheckBoxMinAge";
            this.gpgCheckBoxMinAge.Size = new Size(0x67, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxMinAge, null);
            this.gpgCheckBoxMinAge.TabIndex = 0x2d;
            this.gpgCheckBoxMinAge.Text = "Minimum Age";
            this.gpgCheckBoxMinAge.UsesBG = false;
            this.gpgCheckBoxMinAge.UseVisualStyleBackColor = true;
            this.gpgCheckBoxMinAge.CheckedChanged += new EventHandler(this.gpgCheckBoxMinAge_CheckedChanged);
            this.numericUpDownAge.Enabled = false;
            this.numericUpDownAge.Location = new Point(0x7d, 0x184);
            this.numericUpDownAge.Name = "numericUpDownAge";
            this.numericUpDownAge.Size = new Size(0x2d, 20);
            base.ttDefault.SetSuperTip(this.numericUpDownAge, null);
            this.numericUpDownAge.TabIndex = 0x2f;
            int[] bits = new int[4];
            bits[0] = 0x12;
            this.numericUpDownAge.Value = new decimal(bits);
            this.gpgCheckBoxRequireTime.AutoSize = true;
            this.gpgCheckBoxRequireTime.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgCheckBoxRequireTime.Location = new Point(0x10, 0xf5);
            this.gpgCheckBoxRequireTime.Name = "gpgCheckBoxRequireTime";
            this.gpgCheckBoxRequireTime.Size = new Size(0x87, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxRequireTime, null);
            this.gpgCheckBoxRequireTime.TabIndex = 0x30;
            this.gpgCheckBoxRequireTime.Text = "Desired Time(s):";
            this.gpgCheckBoxRequireTime.UsesBG = false;
            this.gpgCheckBoxRequireTime.UseVisualStyleBackColor = true;
            this.gpgCheckBoxRequireDay.AutoSize = true;
            this.gpgCheckBoxRequireDay.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.gpgCheckBoxRequireDay.Location = new Point(0x10, 0x130);
            this.gpgCheckBoxRequireDay.Name = "gpgCheckBoxRequireDay";
            this.gpgCheckBoxRequireDay.Size = new Size(0x80, 0x11);
            base.ttDefault.SetSuperTip(this.gpgCheckBoxRequireDay, null);
            this.gpgCheckBoxRequireDay.TabIndex = 0x31;
            this.gpgCheckBoxRequireDay.Text = "Desired Day(s):";
            this.gpgCheckBoxRequireDay.UsesBG = false;
            this.gpgCheckBoxRequireDay.UseVisualStyleBackColor = true;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(660, 0x1d7);
            base.Controls.Add(this.gpgCheckBoxRequireDay);
            base.Controls.Add(this.gpgCheckBoxRequireTime);
            base.Controls.Add(this.numericUpDownAge);
            base.Controls.Add(this.gpgCheckBoxMinAge);
            base.Controls.Add(this.gpgCheckBoxTime3);
            base.Controls.Add(this.gpgCheckBoxTime2);
            base.Controls.Add(this.gpgCheckBoxTime1);
            base.Controls.Add(this.comboBoxTimeZone);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgLabel12);
            base.Controls.Add(this.gpgTextBoxHeadOfEffort);
            base.Controls.Add(this.gpgCheckBoxDay7);
            base.Controls.Add(this.gpgCheckBoxDay6);
            base.Controls.Add(this.gpgCheckBoxDay5);
            base.Controls.Add(this.gpgCheckBoxDay4);
            base.Controls.Add(this.gpgCheckBoxDay3);
            base.Controls.Add(this.gpgCheckBoxDay2);
            base.Controls.Add(this.gpgCheckBoxDay1);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.dateTimePickerEnd);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.dateTimePickerBegin);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgTextBoxDetailKB);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgTextBoxLegalKB);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxDescription);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(660, 0x1d7);
            this.MinimumSize = new Size(660, 0x1d7);
            base.Name = "DlgCreateVolunteerEffort";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Create Volunteer Effort";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxDescription, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxLegalKB, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxDetailKB, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.dateTimePickerBegin, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.dateTimePickerEnd, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxDay1, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxDay2, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxDay3, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxDay4, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxDay5, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxDay6, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxDay7, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxHeadOfEffort, 0);
            base.Controls.SetChildIndex(this.gpgLabel12, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.comboBoxTimeZone, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxTime1, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxTime2, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxTime3, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxMinAge, 0);
            base.Controls.SetChildIndex(this.numericUpDownAge, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxRequireTime, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxRequireDay, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxDescription.Properties.EndInit();
            this.gpgTextBoxLegalKB.Properties.EndInit();
            this.gpgTextBoxDetailKB.Properties.EndInit();
            this.gpgTextBoxHeadOfEffort.Properties.EndInit();
            this.numericUpDownAge.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            try
            {
                int num;
                this.Cursor = Cursors.WaitCursor;
                this.skinButtonOK.Enabled = false;
                base.ClearErrors();
                bool flag = false;
                if ((this.gpgTextBoxDescription.Text == null) || (this.gpgTextBoxDescription.Text.Length < 1))
                {
                    base.Error(this.gpgTextBoxDescription, "Field cannot be blank", new object[0]);
                    flag = true;
                }
                if ((this.gpgTextBoxHeadOfEffort.Text == null) || (this.gpgTextBoxHeadOfEffort.Text.Length < 1))
                {
                    base.Error(this.gpgTextBoxHeadOfEffort, "Field cannot be blank", new object[0]);
                    flag = true;
                }
                if ((this.gpgTextBoxDetailKB.Text == null) || (this.gpgTextBoxDetailKB.Text.Length < 1))
                {
                    base.Error(this.gpgTextBoxDetailKB, "Field cannot be blank", new object[0]);
                    flag = true;
                }
                else if (!int.TryParse(this.gpgTextBoxDetailKB.Text, out num))
                {
                    base.Error(this.gpgTextBoxDetailKB, "Not a valid number", new object[0]);
                    flag = true;
                }
                if ((this.gpgTextBoxLegalKB.Text == null) || (this.gpgTextBoxLegalKB.Text.Length < 1))
                {
                    base.Error(this.gpgTextBoxLegalKB, "Field cannot be blank", new object[0]);
                    flag = true;
                }
                else if (!int.TryParse(this.gpgTextBoxLegalKB.Text, out num))
                {
                    base.Error(this.gpgTextBoxLegalKB, "Not a valid number", new object[0]);
                    flag = true;
                }
                if (!((((!this.gpgCheckBoxRequireDay.Checked || this.gpgCheckBoxDay1.Checked) || (this.gpgCheckBoxDay2.Checked || this.gpgCheckBoxDay3.Checked)) || ((this.gpgCheckBoxDay4.Checked || this.gpgCheckBoxDay5.Checked) || this.gpgCheckBoxDay6.Checked)) || this.gpgCheckBoxDay7.Checked))
                {
                    base.Error(this.gpgCheckBoxRequireDay, "At least one day must be checked", new object[0]);
                    flag = true;
                }
                if (!(((!this.gpgCheckBoxRequireTime.Checked || this.gpgCheckBoxTime1.Checked) || this.gpgCheckBoxTime2.Checked) || this.gpgCheckBoxTime3.Checked))
                {
                    base.Error(this.gpgCheckBoxRequireTime, "At least one time must be checked", new object[0]);
                    flag = true;
                }
                if (flag)
                {
                    this.skinButtonOK.Enabled = true;
                }
                else
                {
                    int num2;
                    string str = "";
                    CheckBox[] boxArray = new CheckBox[] { this.gpgCheckBoxTime1, this.gpgCheckBoxTime2, this.gpgCheckBoxTime3 };
                    for (num2 = 0; num2 < boxArray.Length; num2++)
                    {
                        if (boxArray[num2].Checked)
                        {
                            str = str + boxArray[num2].Name.Replace("gpgCheckBoxTime", "") + ",";
                        }
                    }
                    str = str.TrimEnd(new char[] { ',' });
                    string str2 = "";
                    boxArray = new CheckBox[] { this.gpgCheckBoxDay1, this.gpgCheckBoxDay2, this.gpgCheckBoxDay3, this.gpgCheckBoxDay4, this.gpgCheckBoxDay5, this.gpgCheckBoxDay6, this.gpgCheckBoxDay7 };
                    for (num2 = 0; num2 < boxArray.Length; num2++)
                    {
                        if (boxArray[num2].Checked)
                        {
                            str2 = str2 + boxArray[num2].Name.Replace("gpgCheckBoxDay", "") + ",";
                        }
                    }
                    str2 = str2.TrimEnd(new char[] { ',' });
                    int num3 = 0;
                    if (this.gpgCheckBoxMinAge.Checked)
                    {
                        num3 = (int) this.numericUpDownAge.Value;
                    }
                    if (!DataAccess.ExecuteQuery("CreateVolunteerEffort", new object[] { this.gpgTextBoxDescription.Text, this.gpgTextBoxLegalKB.Text, this.gpgTextBoxDetailKB.Text, DataAccess.FormatDate(this.dateTimePickerBegin.Value), DataAccess.FormatDate(this.dateTimePickerEnd.Value), this.comboBoxTimeZone.SelectedItem, DataAccess.FormatBool(this.gpgCheckBoxRequireTime.Checked), str, DataAccess.FormatBool(this.gpgCheckBoxRequireDay.Checked), str2, this.gpgTextBoxHeadOfEffort.Text, num3 }))
                    {
                        base.Error(this.skinButtonOK, "An error occured adding record to database.", new object[0]);
                        this.skinButtonOK.Enabled = true;
                    }
                    else
                    {
                        base.SetStatus("Volunteer Effort Created Successfully.", new object[0]);
                        if (callBack == null)
                        {
                            callBack = delegate (object s) {
                                VGen0 method = null;
                                Thread.Sleep(0xbb8);
                                if (!base.Disposing && !base.IsDisposed)
                                {
                                    if (method == null)
                                    {
                                        method = delegate {
                                            base.Close();
                                        };
                                    }
                                    base.BeginInvoke(method);
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
    }
}

