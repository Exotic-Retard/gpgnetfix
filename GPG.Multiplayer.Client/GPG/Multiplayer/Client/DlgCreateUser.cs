namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;

    public class DlgCreateUser : DlgBase
    {
        private string Canada_Provinces = Loc.Get("<LOC>Unspecified;Alberta;British Columbia;Manitoba;Nunavut;New Brunswick;Newfoundland and Labrador;Northwest Territories;Nova Scotia;Ontario;Prince Edward Island;Quebec;Saskatchewan;Yukon");
        private IContainer components = null;
        private string Countries = Loc.Get("<LOC id=_6b85496f6596bfc54028d8a54ceade67>Unspecified;Afghanistan;Albania;Algeria;Andorra;Angola;Antigua and Barbuda;Argentina;Armenia;Australia;Austria;Azerbaijan;Bahamas;Bahrain;Bangladesh;Barbados;Belarus;Belgium;Belize;Benin;Bhutan;Bolivia;Bosnia and Herzegovina;Botswana;Brazil;Brunei;Bulgaria;Burkina Faso;Burundi;Cambodia;Cameroon;Canada;Cape Verde;Central African Republic;Chad;Chile;China;Colombia;Comoros;Congo (Brazzaville);Congo, Democratic Republic of the;Costa Rica;C\x00f4te d'Ivoire;Croatia;Cuba;Cyprus;Czech Republic;Denmark;Djibouti;Dominica;Dominican Republic;East Timor (Timor Timur);Ecuador;Egypt;El Salvador;Equatorial Guinea;Eritrea;Estonia;Ethiopia;Fiji;Finland;France;Gabon;Gambia, The;Georgia;Germany;Ghana;Greece;Grenada;Guatemala;Guinea;Guinea-Bissau;Guyana;Haiti;Honduras;Hungary;Iceland;India;Indonesia;Iran;Iraq;Ireland;Israel;Italy;Jamaica;Japan;Jordan;Kazakhstan;Kenya;Kiribati;Korea, North;Korea, South;Kuwait;Kyrgyzstan ;Laos;Latvia;Lebanon;Lesotho;Liberia;Libya;Liechtenstein;Lithuania;Luxembourg;Macedonia, Former Yugoslav Republic of;Madagascar;Malawi;Malaysia;Maldives;Mali;Malta;Marshall Islands;Mauritania;Mauritius;Mexico;Micronesia, Federated States of;Moldova;Monaco;Mongolia;Montenegro;Morocco;Mozambique;Myanmar (Burma);Namibia;Nauru;Nepal;Netherlands;New Zealand;Nicaragua;Niger;Nigeria;Norway;Oman;Pakistan;Palau;Panama;Papua New Guinea;Paraguay;Peru;Philippines;Poland;Portugal;Qatar;Romania;Russia;Rwanda;Saint Kitts and Nevis;Saint Lucia;Saint Vincent and The Grenadines;Samoa;San Marino;Sao Tome and Principe;Saudi Arabia;Senegal;Serbia;Seychelles;Sierra Leone;Singapore;Slovakia;Slovenia;Solomon Islands;Somalia;South Africa;Spain;Sri Lanka;Sudan;Suriname;Swaziland;Sweden;Switzerland;Syria;Taiwan;Tanzania;Thailand;Togo;Tonga;Trinidad and Tobago;Tunisia;Turkey;Turkmenistan;Tuvalu;Uganda;Ukraine;United Arab Emirates;United Kingdom;United States;Uruguay;Uzbekistan;Vanuatu;Vatican City;Venezuela;Vietnam;Western Sahara;Yemen;Zambia;Zimbabwe");
        private GPGCheckBox gpgCheckBoxMailer;
        public GPGDropDownList gpgDropDownListCountries;
        public GPGDropDownList gpgDropDownListState;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel10;
        private GPGLabel gpgLabel11;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        public GPGTextBox gpgTextBoxAddr1;
        public GPGTextBox gpgTextBoxAddr2;
        public GPGTextBox gpgTextBoxFName;
        public GPGTextBox gpgTextBoxLName;
        public GPGTextBox gpgTextBoxPostal;
        private GPGLabel labelError;
        private GPGLabel lConfirm;
        private GPGLabel lEmail;
        private GPGLabel lPassword;
        private GPGLabel lUsername;
        public SkinButton skinButtonCancel;
        public SkinButton skinButtonOK;
        private System.Timers.Timer StatusTimer = new System.Timers.Timer(1000.0);
        public GPGTextBox tbConfirmPassword;
        public GPGTextBox tbEmailAddress;
        public GPGTextBox tbPassword;
        public GPGTextBox tbUsername;
        private string US_States = Loc.Get("<LOC>Unspecified;Alabama;Alaska;Arizona;Arkansas;California;Colorado;Connecticut;Delaware;Florida;Georgia;Hawaii;Idaho;Illinois;Indiana;Iowa;Kansas;Kentucky;Louisiana;Maine;Maryland;Massachusetts;Michigan;Minnesota;Mississippi;Missouri;Montana;Nebraska;Nevada;New Hampshire;New Jersey;New Mexico;New York;North Carolina;North Dakota;Ohio;Oklahoma;Oregon;Pennsylvania;Rhode Island;South Carolina;South Dakota;Tennessee;Texas;Utah;Vermont;Virginia;Washington;West Virginia;Wisconsin;Wyoming");

        public DlgCreateUser()
        {
            this.InitializeComponent();
            Loc.LocObject(this);
            this.StatusTimer.Elapsed += new ElapsedEventHandler(this.StatusTimer_Elapsed);
            this.gpgDropDownListCountries.DataSource = this.Countries.Split(";".ToCharArray());
            this.gpgDropDownListCountries.SelectedIndex = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoCreateUser()
        {
            bool flag = !TextUtil.IsAlphaNumericString(this.tbUsername.Text);
            this.labelError.Text = "";
            if (this.tbUsername.Text.Length == 0)
            {
                this.Error("<LOC>Your username cannot be blank.");
            }
            else if ((this.tbUsername.Text.Length < 3) || (this.tbUsername.Text.Length > 0x16))
            {
                this.Error("<LOC>Your username must be 3-22 characters in length.");
            }
            else if (flag)
            {
                this.Error("<LOC>Your username contains invalid characters, it may only contain letters, numbers, dashes and underbars.");
            }
            else if (this.tbConfirmPassword.Text != this.tbPassword.Text)
            {
                this.Error("<LOC>Your passwords do not match.");
            }
            else if (this.tbPassword.Text.Length < 6)
            {
                this.Error("<LOC>Your password must be at least 6 characters long.");
            }
            else if (((this.tbEmailAddress.Text.IndexOf("@") < 0) || (this.tbEmailAddress.Text.IndexOf(".") < 0)) || (this.tbEmailAddress.Text.Length < 8))
            {
                this.Error("<LOC>Your email address does not appear to be valid. Please provide a valid email address.");
            }
            else if (Profanity.ContainsProfanity(this.tbUsername.Text))
            {
                this.Error("<LOC>Usernames cannot contain profanity. Please enter a valid username.");
            }
            else
            {
                this.SetStatus("<LOC>Creating User");
                this.SetEnabled(false);
                this.StatusTimer.Start();
                ThreadQueue.Quazal.Enqueue(typeof(User), "CreateLogin", this, "FinishCreate", new object[] { this.tbUsername.Text, this.tbPassword.Text, this.tbEmailAddress.Text, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort });
            }
        }

        private void EndLogin(bool result)
        {
            this.StatusTimer.Stop();
            if (result && (User.Current != null))
            {
                int mailer = 0;
                if (this.gpgCheckBoxMailer.Checked)
                {
                    mailer = 1;
                }
                string firstname = this.gpgTextBoxFName.Text;
                string lastname = this.gpgTextBoxLName.Text;
                string addr1 = this.gpgTextBoxAddr1.Text;
                string addr2 = this.gpgTextBoxAddr2.Text;
                string postal = this.gpgTextBoxPostal.Text;
                object state = this.gpgDropDownListState.SelectedValue;
                object country = this.gpgDropDownListCountries.SelectedValue;
                ThreadQueue.Quazal.Enqueue(delegate {
                    if (!DataAccess.ExecuteQuery("CreatePrincipalInfo", new object[] { firstname, lastname, addr1, addr2, postal, state, country, mailer }))
                    {
                        ErrorLog.WriteLine("Error creating principal_info for {0}", new object[] { User.Current });
                    }
                    if (!new QuazalQuery("CreateHonorPoints", new object[0]).ExecuteNonQuery())
                    {
                        ErrorLog.WriteLine("Error creating honor_points for {0}", new object[] { User.Current });
                    }
                    if (!new QuazalQuery("CreatePlayerDisplayAwards", new object[0]).ExecuteNonQuery())
                    {
                        ErrorLog.WriteLine("Error creating player_display_awards for {0}", new object[] { User.Current });
                    }
                }, new object[0]);
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                QuazalError error = QuazalErrorCodes.GetError();
                if (error.errorcode != -1)
                {
                    this.Error(error.message);
                }
                else
                {
                    this.Error("<LOC>Unable to login at this time, please try again later.");
                    Thread.Sleep(0x7d0);
                    base.DialogResult = DialogResult.Cancel;
                }
            }
        }

        private void Error(string error)
        {
            this.labelError.TextStyle = TextStyles.Error;
            this.labelError.Text = Loc.Get(error);
            this.labelError.Visible = true;
            this.labelError.Refresh();
        }

        private void FinishCreate(bool result)
        {
            if (result)
            {
                this.SetStatus("<LOC>Logging In");
                ThreadQueue.Quazal.Enqueue(typeof(User), "Login", this, "EndLogin", new object[] { this.tbUsername.Text, this.tbPassword.Text, Program.Settings.Login.DefaultServer, Program.Settings.Login.DefaultPort });
            }
            else
            {
                this.StatusTimer.Stop();
                this.SetEnabled(true);
                QuazalError error = QuazalErrorCodes.GetError();
                if (error.errorcode != -1)
                {
                    this.Error(error.message);
                }
                else
                {
                    this.Error("<LOC>We were unable to create an account.  Please choose a different username and password.");
                }
            }
        }

        private void gpgDropDownListCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = (string) this.gpgDropDownListCountries.SelectedValue;
            if (selectedValue == Loc.Get("<LOC>United States"))
            {
                this.gpgDropDownListState.DataSource = this.US_States.Split(";".ToCharArray());
                this.gpgDropDownListState.SelectedIndex = 0;
                this.gpgDropDownListState.Enabled = true;
            }
            else if (selectedValue == Loc.Get("<LOC>Canada"))
            {
                this.gpgDropDownListState.DataSource = this.Canada_Provinces.Split(";".ToCharArray());
                this.gpgDropDownListState.SelectedIndex = 0;
                this.gpgDropDownListState.Enabled = true;
            }
            else
            {
                this.gpgDropDownListState.DataSource = null;
                this.gpgDropDownListState.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            this.tbUsername = new GPGTextBox();
            this.lUsername = new GPGLabel();
            this.lPassword = new GPGLabel();
            this.tbPassword = new GPGTextBox();
            this.lConfirm = new GPGLabel();
            this.tbConfirmPassword = new GPGTextBox();
            this.lEmail = new GPGLabel();
            this.tbEmailAddress = new GPGTextBox();
            this.labelError = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.gpgTextBoxLName = new GPGTextBox();
            this.gpgLabel8 = new GPGLabel();
            this.gpgTextBoxPostal = new GPGTextBox();
            this.gpgLabel9 = new GPGLabel();
            this.gpgTextBoxAddr1 = new GPGTextBox();
            this.gpgTextBoxFName = new GPGTextBox();
            this.gpgLabel10 = new GPGLabel();
            this.gpgLabel11 = new GPGLabel();
            this.gpgTextBoxAddr2 = new GPGTextBox();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgCheckBoxMailer = new GPGCheckBox();
            this.skinButtonOK = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.gpgDropDownListCountries = new GPGDropDownList();
            this.gpgDropDownListState = new GPGDropDownList();
            this.tbUsername.Properties.BeginInit();
            this.tbPassword.Properties.BeginInit();
            this.tbConfirmPassword.Properties.BeginInit();
            this.tbEmailAddress.Properties.BeginInit();
            this.gpgTextBoxLName.Properties.BeginInit();
            this.gpgTextBoxPostal.Properties.BeginInit();
            this.gpgTextBoxAddr1.Properties.BeginInit();
            this.gpgTextBoxFName.Properties.BeginInit();
            this.gpgTextBoxAddr2.Properties.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.tbUsername.Location = new Point(0x1d, 0x89);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Properties.Appearance.BackColor = Color.Black;
            this.tbUsername.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbUsername.Properties.Appearance.ForeColor = Color.White;
            this.tbUsername.Properties.Appearance.Options.UseBackColor = true;
            this.tbUsername.Properties.Appearance.Options.UseBorderColor = true;
            this.tbUsername.Properties.Appearance.Options.UseForeColor = true;
            this.tbUsername.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbUsername.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbUsername.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbUsername.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbUsername.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbUsername.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbUsername.Properties.BorderStyle = BorderStyles.Simple;
            this.tbUsername.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbUsername.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbUsername.Properties.MaxLength = 0x16;
            this.tbUsername.Size = new Size(0xe7, 20);
            this.tbUsername.TabIndex = 0;
            this.lUsername.AutoSize = true;
            this.lUsername.AutoStyle = true;
            this.lUsername.Font = new Font("Arial", 9.75f);
            this.lUsername.ForeColor = Color.White;
            this.lUsername.IgnoreMouseWheel = false;
            this.lUsername.IsStyled = false;
            this.lUsername.Location = new Point(0x1a, 0x68);
            this.lUsername.Name = "lUsername";
            this.lUsername.Size = new Size(0x72, 0x10);
            this.lUsername.TabIndex = 5;
            this.lUsername.Text = "<LOC>*Username";
            this.lUsername.TextStyle = TextStyles.Title;
            this.lPassword.AutoSize = true;
            this.lPassword.AutoStyle = true;
            this.lPassword.Font = new Font("Arial", 9.75f);
            this.lPassword.ForeColor = Color.White;
            this.lPassword.IgnoreMouseWheel = false;
            this.lPassword.IsStyled = false;
            this.lPassword.Location = new Point(0x1a, 0xd8);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new Size(0x70, 0x10);
            this.lPassword.TabIndex = 6;
            this.lPassword.Text = "<LOC>*Password";
            this.lPassword.TextStyle = TextStyles.Title;
            this.tbPassword.Location = new Point(0x1d, 0xf9);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Properties.Appearance.BackColor = Color.Black;
            this.tbPassword.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbPassword.Properties.Appearance.ForeColor = Color.White;
            this.tbPassword.Properties.Appearance.Options.UseBackColor = true;
            this.tbPassword.Properties.Appearance.Options.UseBorderColor = true;
            this.tbPassword.Properties.Appearance.Options.UseForeColor = true;
            this.tbPassword.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbPassword.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbPassword.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbPassword.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbPassword.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbPassword.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbPassword.Properties.BorderStyle = BorderStyles.Simple;
            this.tbPassword.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbPassword.Properties.PasswordChar = '*';
            this.tbPassword.Size = new Size(0xe7, 20);
            this.tbPassword.TabIndex = 2;
            this.lConfirm.AutoSize = true;
            this.lConfirm.AutoStyle = true;
            this.lConfirm.Font = new Font("Arial", 9.75f);
            this.lConfirm.ForeColor = Color.White;
            this.lConfirm.IgnoreMouseWheel = false;
            this.lConfirm.IsStyled = false;
            this.lConfirm.Location = new Point(0x1a, 0x110);
            this.lConfirm.Name = "lConfirm";
            this.lConfirm.Size = new Size(160, 0x10);
            this.lConfirm.TabIndex = 8;
            this.lConfirm.Text = "<LOC>*Confirm Password";
            this.lConfirm.TextStyle = TextStyles.Title;
            this.tbConfirmPassword.Location = new Point(0x1d, 0x123);
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.Properties.Appearance.BackColor = Color.Black;
            this.tbConfirmPassword.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbConfirmPassword.Properties.Appearance.ForeColor = Color.White;
            this.tbConfirmPassword.Properties.Appearance.Options.UseBackColor = true;
            this.tbConfirmPassword.Properties.Appearance.Options.UseBorderColor = true;
            this.tbConfirmPassword.Properties.Appearance.Options.UseForeColor = true;
            this.tbConfirmPassword.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbConfirmPassword.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbConfirmPassword.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbConfirmPassword.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbConfirmPassword.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbConfirmPassword.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbConfirmPassword.Properties.BorderStyle = BorderStyles.Simple;
            this.tbConfirmPassword.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbConfirmPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbConfirmPassword.Properties.PasswordChar = '*';
            this.tbConfirmPassword.Size = new Size(0xe7, 20);
            this.tbConfirmPassword.TabIndex = 3;
            this.lEmail.AutoSize = true;
            this.lEmail.AutoStyle = true;
            this.lEmail.Font = new Font("Arial", 9.75f);
            this.lEmail.ForeColor = Color.White;
            this.lEmail.IgnoreMouseWheel = false;
            this.lEmail.IsStyled = false;
            this.lEmail.Location = new Point(0x1a, 160);
            this.lEmail.Name = "lEmail";
            this.lEmail.Size = new Size(140, 0x10);
            this.lEmail.TabIndex = 10;
            this.lEmail.Text = "<LOC>*Email Address";
            this.lEmail.TextStyle = TextStyles.Title;
            this.tbEmailAddress.Location = new Point(0x1d, 0xc1);
            this.tbEmailAddress.Name = "tbEmailAddress";
            this.tbEmailAddress.Properties.Appearance.BackColor = Color.Black;
            this.tbEmailAddress.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbEmailAddress.Properties.Appearance.ForeColor = Color.White;
            this.tbEmailAddress.Properties.Appearance.Options.UseBackColor = true;
            this.tbEmailAddress.Properties.Appearance.Options.UseBorderColor = true;
            this.tbEmailAddress.Properties.Appearance.Options.UseForeColor = true;
            this.tbEmailAddress.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbEmailAddress.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbEmailAddress.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbEmailAddress.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbEmailAddress.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbEmailAddress.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbEmailAddress.Properties.BorderStyle = BorderStyles.Simple;
            this.tbEmailAddress.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbEmailAddress.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbEmailAddress.Size = new Size(0xe7, 20);
            this.tbEmailAddress.TabIndex = 1;
            this.labelError.AutoStyle = true;
            this.labelError.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelError.ForeColor = Color.Red;
            this.labelError.IgnoreMouseWheel = false;
            this.labelError.IsStyled = false;
            this.labelError.Location = new Point(0x1a, 0x179);
            this.labelError.Name = "labelError";
            this.labelError.Size = new Size(0x183, 0x3b);
            this.labelError.TabIndex = 14;
            this.labelError.Text = "<LOC>This is a test error.  This is not usually visible.  It only happens when a login error occurs.";
            this.labelError.TextStyle = TextStyles.Status;
            this.labelError.Visible = false;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 8f, FontStyle.Italic);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(0x1a, 120);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xd0, 14);
            this.gpgLabel1.TabIndex = 15;
            this.gpgLabel1.Text = "<LOC>Must be 3-22 characters in length";
            this.gpgLabel1.TextStyle = TextStyles.Descriptor;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 8f, FontStyle.Italic);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x1a, 0xe8);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0xbc, 14);
            this.gpgLabel2.TabIndex = 0x10;
            this.gpgLabel2.Text = "<LOC>Must be 6 or more characters";
            this.gpgLabel2.TextStyle = TextStyles.Descriptor;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 8f, FontStyle.Italic);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x1a, 0xb0);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0xf1, 14);
            this.gpgLabel3.TabIndex = 0x11;
            this.gpgLabel3.Text = "<LOC>Valid email required for password resets";
            this.gpgLabel3.TextStyle = TextStyles.Descriptor;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 8f, FontStyle.Italic);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(12, 80);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0xab, 14);
            this.gpgLabel4.TabIndex = 0x12;
            this.gpgLabel4.Text = "<LOC>* Indicates a required field";
            this.gpgLabel4.TextStyle = TextStyles.Descriptor;
            this.gpgTextBoxLName.Location = new Point(0x141, 0xa5);
            this.gpgTextBoxLName.Name = "gpgTextBoxLName";
            this.gpgTextBoxLName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxLName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxLName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxLName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxLName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxLName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxLName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxLName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxLName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxLName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxLName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxLName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxLName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxLName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxLName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxLName.Size = new Size(0xe7, 20);
            this.gpgTextBoxLName.TabIndex = 6;
            this.gpgLabel8.AutoSize = true;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.Font = new Font("Arial", 9.75f);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x13e, 0x92);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x71, 0x10);
            this.gpgLabel8.TabIndex = 0x19;
            this.gpgLabel8.Text = "<LOC>Last Name";
            this.gpgLabel8.TextStyle = TextStyles.Title;
            this.gpgTextBoxPostal.Location = new Point(0x141, 0x113);
            this.gpgTextBoxPostal.Name = "gpgTextBoxPostal";
            this.gpgTextBoxPostal.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPostal.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPostal.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPostal.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPostal.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPostal.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPostal.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPostal.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPostal.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPostal.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPostal.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPostal.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPostal.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPostal.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPostal.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPostal.Size = new Size(0x70, 20);
            this.gpgTextBoxPostal.TabIndex = 9;
            this.gpgLabel9.AutoSize = true;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.Font = new Font("Arial", 9.75f);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0x13e, 0x100);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x79, 0x10);
            this.gpgLabel9.TabIndex = 0x17;
            this.gpgLabel9.Text = "<LOC>Postal Code";
            this.gpgLabel9.TextStyle = TextStyles.Title;
            this.gpgTextBoxAddr1.Location = new Point(0x141, 0xcf);
            this.gpgTextBoxAddr1.Name = "gpgTextBoxAddr1";
            this.gpgTextBoxAddr1.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxAddr1.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxAddr1.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxAddr1.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxAddr1.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxAddr1.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxAddr1.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxAddr1.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxAddr1.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxAddr1.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxAddr1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxAddr1.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxAddr1.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxAddr1.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxAddr1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxAddr1.Size = new Size(0xe7, 20);
            this.gpgTextBoxAddr1.TabIndex = 7;
            this.gpgTextBoxFName.Location = new Point(0x141, 0x7b);
            this.gpgTextBoxFName.Name = "gpgTextBoxFName";
            this.gpgTextBoxFName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxFName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxFName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxFName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxFName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxFName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxFName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxFName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxFName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxFName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxFName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxFName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxFName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxFName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxFName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxFName.Size = new Size(0xe7, 20);
            this.gpgTextBoxFName.TabIndex = 5;
            this.gpgLabel10.AutoSize = true;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.Font = new Font("Arial", 9.75f);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(0x13e, 0x68);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0x72, 0x10);
            this.gpgLabel10.TabIndex = 20;
            this.gpgLabel10.Text = "<LOC>First Name";
            this.gpgLabel10.TextStyle = TextStyles.Title;
            this.gpgLabel11.AutoSize = true;
            this.gpgLabel11.AutoStyle = true;
            this.gpgLabel11.Font = new Font("Arial", 9.75f);
            this.gpgLabel11.ForeColor = Color.White;
            this.gpgLabel11.IgnoreMouseWheel = false;
            this.gpgLabel11.IsStyled = false;
            this.gpgLabel11.Location = new Point(0x13e, 0xbc);
            this.gpgLabel11.Name = "gpgLabel11";
            this.gpgLabel11.Size = new Size(0x62, 0x10);
            this.gpgLabel11.TabIndex = 0x15;
            this.gpgLabel11.Text = "<LOC>Address";
            this.gpgLabel11.TextStyle = TextStyles.Title;
            this.gpgTextBoxAddr2.Location = new Point(0x141, 0xe9);
            this.gpgTextBoxAddr2.Name = "gpgTextBoxAddr2";
            this.gpgTextBoxAddr2.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxAddr2.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxAddr2.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxAddr2.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxAddr2.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxAddr2.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxAddr2.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxAddr2.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxAddr2.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxAddr2.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxAddr2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxAddr2.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxAddr2.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxAddr2.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxAddr2.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxAddr2.Size = new Size(0xe7, 20);
            this.gpgTextBoxAddr2.TabIndex = 8;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x1b6, 0x100);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x8e, 0x10);
            this.gpgLabel5.TabIndex = 0x1d;
            this.gpgLabel5.Text = "<LOC>State / Province";
            this.gpgLabel5.TextStyle = TextStyles.Title;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x13f, 0x12a);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x5f, 0x10);
            this.gpgLabel6.TabIndex = 0x20;
            this.gpgLabel6.Text = "<LOC>Country";
            this.gpgLabel6.TextStyle = TextStyles.Title;
            this.gpgCheckBoxMailer.CheckAlign = ContentAlignment.TopLeft;
            this.gpgCheckBoxMailer.Checked = true;
            this.gpgCheckBoxMailer.CheckState = CheckState.Checked;
            this.gpgCheckBoxMailer.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgCheckBoxMailer.Location = new Point(0x1d, 0x13f);
            this.gpgCheckBoxMailer.Name = "gpgCheckBoxMailer";
            this.gpgCheckBoxMailer.Size = new Size(0xe7, 0x37);
            this.gpgCheckBoxMailer.TabIndex = 4;
            this.gpgCheckBoxMailer.Text = "<LOC>Send me exclusive Gas Powered Games updates via email";
            this.gpgCheckBoxMailer.TextAlign = ContentAlignment.TopLeft;
            this.gpgCheckBoxMailer.UseVisualStyleBackColor = true;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Black;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(0x19d, 0x179);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x4b, 0x17);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 12;
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
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(0x1f2, 0x179);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x4b, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 13;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.gpgDropDownListCountries.BackColor = Color.Black;
            this.gpgDropDownListCountries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.gpgDropDownListCountries.DropDownWidth = 0x10f;
            this.gpgDropDownListCountries.FlatStyle = FlatStyle.Flat;
            this.gpgDropDownListCountries.Font = new Font("Verdana", 7f);
            this.gpgDropDownListCountries.FormattingEnabled = true;
            this.gpgDropDownListCountries.Location = new Point(0x141, 0x13d);
            this.gpgDropDownListCountries.MaxDropDownItems = 15;
            this.gpgDropDownListCountries.Name = "gpgDropDownListCountries";
            this.gpgDropDownListCountries.Size = new Size(0xe7, 20);
            this.gpgDropDownListCountries.TabIndex = 11;
            this.gpgDropDownListCountries.SelectedIndexChanged += new EventHandler(this.gpgDropDownListCountries_SelectedIndexChanged);
            this.gpgDropDownListState.BackColor = Color.Black;
            this.gpgDropDownListState.DropDownStyle = ComboBoxStyle.DropDownList;
            this.gpgDropDownListState.DropDownWidth = 0xd3;
            this.gpgDropDownListState.FlatStyle = FlatStyle.Flat;
            this.gpgDropDownListState.Font = new Font("Verdana", 7f);
            this.gpgDropDownListState.FormattingEnabled = true;
            this.gpgDropDownListState.IntegralHeight = false;
            this.gpgDropDownListState.ItemHeight = 12;
            this.gpgDropDownListState.Location = new Point(0x1b9, 0x113);
            this.gpgDropDownListState.MaxDropDownItems = 15;
            this.gpgDropDownListState.Name = "gpgDropDownListState";
            this.gpgDropDownListState.Size = new Size(0x6f, 20);
            this.gpgDropDownListState.TabIndex = 10;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x24b, 470);
            base.Controls.Add(this.gpgDropDownListState);
            base.Controls.Add(this.gpgDropDownListCountries);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.gpgCheckBoxMailer);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgTextBoxAddr2);
            base.Controls.Add(this.gpgTextBoxLName);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgTextBoxPostal);
            base.Controls.Add(this.gpgLabel9);
            base.Controls.Add(this.gpgTextBoxAddr1);
            base.Controls.Add(this.gpgTextBoxFName);
            base.Controls.Add(this.gpgLabel10);
            base.Controls.Add(this.gpgLabel11);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.labelError);
            base.Controls.Add(this.tbEmailAddress);
            base.Controls.Add(this.lEmail);
            base.Controls.Add(this.tbConfirmPassword);
            base.Controls.Add(this.lConfirm);
            base.Controls.Add(this.tbPassword);
            base.Controls.Add(this.tbUsername);
            base.Controls.Add(this.lUsername);
            base.Controls.Add(this.lPassword);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MaximumSize = new Size(0x24b, 470);
            this.MinimumSize = new Size(0x24b, 470);
            base.Name = "DlgCreateUser";
            this.Text = "<LOC>Create a new account";
            base.Controls.SetChildIndex(this.lPassword, 0);
            base.Controls.SetChildIndex(this.lUsername, 0);
            base.Controls.SetChildIndex(this.tbUsername, 0);
            base.Controls.SetChildIndex(this.tbPassword, 0);
            base.Controls.SetChildIndex(this.lConfirm, 0);
            base.Controls.SetChildIndex(this.tbConfirmPassword, 0);
            base.Controls.SetChildIndex(this.lEmail, 0);
            base.Controls.SetChildIndex(this.tbEmailAddress, 0);
            base.Controls.SetChildIndex(this.labelError, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgLabel11, 0);
            base.Controls.SetChildIndex(this.gpgLabel10, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxFName, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxAddr1, 0);
            base.Controls.SetChildIndex(this.gpgLabel9, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPostal, 0);
            base.Controls.SetChildIndex(this.gpgLabel8, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxLName, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxAddr2, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgCheckBoxMailer, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.gpgDropDownListCountries, 0);
            base.Controls.SetChildIndex(this.gpgDropDownListState, 0);
            this.tbUsername.Properties.EndInit();
            this.tbPassword.Properties.EndInit();
            this.tbConfirmPassword.Properties.EndInit();
            this.tbEmailAddress.Properties.EndInit();
            this.gpgTextBoxLName.Properties.EndInit();
            this.gpgTextBoxPostal.Properties.EndInit();
            this.gpgTextBoxAddr1.Properties.EndInit();
            this.gpgTextBoxFName.Properties.EndInit();
            this.gpgTextBoxAddr2.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.tbUsername.Select();
            this.tbUsername.SelectionStart = this.tbUsername.Text.Length;
        }

        private void SetEnabled(bool status)
        {
            this.tbUsername.Enabled = status;
            this.tbPassword.Enabled = status;
            this.tbConfirmPassword.Enabled = status;
            this.tbEmailAddress.Enabled = status;
            this.gpgCheckBoxMailer.Enabled = status;
            this.gpgTextBoxAddr1.Enabled = status;
            this.gpgTextBoxAddr2.Enabled = status;
            this.gpgTextBoxFName.Enabled = status;
            this.gpgTextBoxLName.Enabled = status;
            this.gpgTextBoxPostal.Enabled = status;
            this.gpgDropDownListCountries.Enabled = status;
            this.gpgDropDownListState.Enabled = status;
            this.skinButtonOK.Enabled = status;
        }

        private void SetStatus(string status)
        {
            this.labelError.TextStyle = TextStyles.Status;
            this.labelError.Text = Loc.Get(status);
            this.labelError.Visible = true;
            this.labelError.Refresh();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            this.StatusTimer.Stop();
            base.DialogResult = DialogResult.Cancel;
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            this.DoCreateUser();
        }

        private void StatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            VGen0 method = null;
            if (base.Disposing || base.IsDisposed)
            {
                this.StatusTimer.Stop();
            }
            else
            {
                if (method == null)
                {
                    method = delegate {
                        this.SetStatus(this.labelError.Text + ".");
                    };
                }
                base.BeginInvoke(method);
            }
        }
    }
}

