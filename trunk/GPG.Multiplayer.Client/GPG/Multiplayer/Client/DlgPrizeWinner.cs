namespace GPG.Multiplayer.Client
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgPrizeWinner : DlgBase
    {
        private string Canada_Provinces = Loc.Get("<LOC>Unspecified;Alberta;British Columbia;Manitoba;Nunavut;New Brunswick;Newfoundland and Labrador;Northwest Territories;Nova Scotia;Ontario;Prince Edward Island;Quebec;Saskatchewan;Yukon");
        private IContainer components = null;
        private string Countries = Loc.Get("<LOC id=_6b85496f6596bfc54028d8a54ceade67>Unspecified;Afghanistan;Albania;Algeria;Andorra;Angola;Antigua and Barbuda;Argentina;Armenia;Australia;Austria;Azerbaijan;Bahamas;Bahrain;Bangladesh;Barbados;Belarus;Belgium;Belize;Benin;Bhutan;Bolivia;Bosnia and Herzegovina;Botswana;Brazil;Brunei;Bulgaria;Burkina Faso;Burundi;Cambodia;Cameroon;Canada;Cape Verde;Central African Republic;Chad;Chile;China;Colombia;Comoros;Congo (Brazzaville);Congo, Democratic Republic of the;Costa Rica;C\x00f4te d'Ivoire;Croatia;Cuba;Cyprus;Czech Republic;Denmark;Djibouti;Dominica;Dominican Republic;East Timor (Timor Timur);Ecuador;Egypt;El Salvador;Equatorial Guinea;Eritrea;Estonia;Ethiopia;Fiji;Finland;France;Gabon;Gambia, The;Georgia;Germany;Ghana;Greece;Grenada;Guatemala;Guinea;Guinea-Bissau;Guyana;Haiti;Honduras;Hungary;Iceland;India;Indonesia;Iran;Iraq;Ireland;Israel;Italy;Jamaica;Japan;Jordan;Kazakhstan;Kenya;Kiribati;Korea, North;Korea, South;Kuwait;Kyrgyzstan ;Laos;Latvia;Lebanon;Lesotho;Liberia;Libya;Liechtenstein;Lithuania;Luxembourg;Macedonia, Former Yugoslav Republic of;Madagascar;Malawi;Malaysia;Maldives;Mali;Malta;Marshall Islands;Mauritania;Mauritius;Mexico;Micronesia, Federated States of;Moldova;Monaco;Mongolia;Montenegro;Morocco;Mozambique;Myanmar (Burma);Namibia;Nauru;Nepal;Netherlands;New Zealand;Nicaragua;Niger;Nigeria;Norway;Oman;Pakistan;Palau;Panama;Papua New Guinea;Paraguay;Peru;Philippines;Poland;Portugal;Qatar;Romania;Russia;Rwanda;Saint Kitts and Nevis;Saint Lucia;Saint Vincent and The Grenadines;Samoa;San Marino;Sao Tome and Principe;Saudi Arabia;Senegal;Serbia;Seychelles;Sierra Leone;Singapore;Slovakia;Slovenia;Solomon Islands;Somalia;South Africa;Spain;Sri Lanka;Sudan;Suriname;Swaziland;Sweden;Switzerland;Syria;Taiwan;Tanzania;Thailand;Togo;Tonga;Trinidad and Tobago;Tunisia;Turkey;Turkmenistan;Tuvalu;Uganda;Ukraine;United Arab Emirates;United Kingdom;United States;Uruguay;Uzbekistan;Vanuatu;Vatican City;Venezuela;Vietnam;Western Sahara;Yemen;Zambia;Zimbabwe");
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
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGLabel gpgLabelEmail1;
        private GPGTextBox gpgTextBoxAddress1;
        private GPGTextBox gpgTextBoxAddress2;
        private GPGTextBox gpgTextBoxCity;
        private GPGTextBox gpgTextBoxEmail1;
        private GPGTextBox gpgTextBoxEmail2;
        private GPGTextBox gpgTextBoxFirstName;
        private GPGTextBox gpgTextBoxLastName;
        private GPGTextBox gpgTextBoxPayPal;
        private GPGTextBox gpgTextBoxPhone;
        private GPGTextBox gpgTextBoxPostalCode;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;
        private string US_States = Loc.Get("<LOC>Unspecified;Alabama;Alaska;Arizona;Arkansas;California;Colorado;Connecticut;Delaware;Florida;Georgia;Hawaii;Idaho;Illinois;Indiana;Iowa;Kansas;Kentucky;Louisiana;Maine;Maryland;Massachusetts;Michigan;Minnesota;Mississippi;Missouri;Montana;Nebraska;Nevada;New Hampshire;New Jersey;New Mexico;New York;North Carolina;North Dakota;Ohio;Oklahoma;Oregon;Pennsylvania;Rhode Island;South Carolina;South Dakota;Tennessee;Texas;Utah;Vermont;Virginia;Washington;West Virginia;Wisconsin;Wyoming");

        public DlgPrizeWinner()
        {
            this.InitializeComponent();
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

        private void DlgPrizeWinner_Load(object sender, EventArgs e)
        {
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
            this.gpgLabel1 = new GPGLabel();
            this.gpgTextBoxFirstName = new GPGTextBox();
            this.gpgTextBoxLastName = new GPGTextBox();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextBoxEmail1 = new GPGTextBox();
            this.gpgLabelEmail1 = new GPGLabel();
            this.gpgTextBoxEmail2 = new GPGTextBox();
            this.gpgLabel4 = new GPGLabel();
            this.gpgTextBoxPhone = new GPGTextBox();
            this.gpgLabel5 = new GPGLabel();
            this.gpgTextBoxPayPal = new GPGTextBox();
            this.gpgLabel6 = new GPGLabel();
            this.gpgTextBoxAddress1 = new GPGTextBox();
            this.gpgLabel7 = new GPGLabel();
            this.gpgTextBoxAddress2 = new GPGTextBox();
            this.gpgLabel8 = new GPGLabel();
            this.gpgTextBoxCity = new GPGTextBox();
            this.gpgLabel9 = new GPGLabel();
            this.gpgLabel10 = new GPGLabel();
            this.gpgLabel11 = new GPGLabel();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.gpgDropDownListState = new GPGDropDownList();
            this.gpgDropDownListCountries = new GPGDropDownList();
            this.gpgLabel3 = new GPGLabel();
            this.gpgTextBoxPostalCode = new GPGTextBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextBoxFirstName.Properties.BeginInit();
            this.gpgTextBoxLastName.Properties.BeginInit();
            this.gpgTextBoxEmail1.Properties.BeginInit();
            this.gpgTextBoxEmail2.Properties.BeginInit();
            this.gpgTextBoxPhone.Properties.BeginInit();
            this.gpgTextBoxPayPal.Properties.BeginInit();
            this.gpgTextBoxAddress1.Properties.BeginInit();
            this.gpgTextBoxAddress2.Properties.BeginInit();
            this.gpgTextBoxCity.Properties.BeginInit();
            this.gpgTextBoxPostalCode.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x24b, 0x39);
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
            this.gpgLabel1.Location = new Point(12, 80);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x72, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 7;
            this.gpgLabel1.Text = "<LOC>First Name";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgTextBoxFirstName.Location = new Point(13, 100);
            this.gpgTextBoxFirstName.Name = "gpgTextBoxFirstName";
            this.gpgTextBoxFirstName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxFirstName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxFirstName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxFirstName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxFirstName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxFirstName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxFirstName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxFirstName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxFirstName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxFirstName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxFirstName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxFirstName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxFirstName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxFirstName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxFirstName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxFirstName.Size = new Size(0x116, 20);
            this.gpgTextBoxFirstName.TabIndex = 8;
            this.gpgTextBoxLastName.Location = new Point(13, 0x99);
            this.gpgTextBoxLastName.Name = "gpgTextBoxLastName";
            this.gpgTextBoxLastName.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxLastName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxLastName.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxLastName.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxLastName.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxLastName.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxLastName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxLastName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxLastName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxLastName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxLastName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxLastName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxLastName.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxLastName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxLastName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxLastName.Size = new Size(0x116, 20);
            this.gpgTextBoxLastName.TabIndex = 10;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(12, 0x85);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x71, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 9;
            this.gpgLabel2.Text = "<LOC>Last Name";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgTextBoxEmail1.Location = new Point(13, 210);
            this.gpgTextBoxEmail1.Name = "gpgTextBoxEmail1";
            this.gpgTextBoxEmail1.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxEmail1.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxEmail1.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxEmail1.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxEmail1.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxEmail1.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxEmail1.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxEmail1.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxEmail1.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxEmail1.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxEmail1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxEmail1.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxEmail1.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxEmail1.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxEmail1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxEmail1.Size = new Size(0x116, 20);
            this.gpgTextBoxEmail1.TabIndex = 12;
            this.gpgLabelEmail1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelEmail1.AutoSize = true;
            this.gpgLabelEmail1.AutoStyle = true;
            this.gpgLabelEmail1.Font = new Font("Arial", 9.75f);
            this.gpgLabelEmail1.ForeColor = Color.White;
            this.gpgLabelEmail1.IgnoreMouseWheel = false;
            this.gpgLabelEmail1.IsStyled = false;
            this.gpgLabelEmail1.Location = new Point(12, 190);
            this.gpgLabelEmail1.Name = "gpgLabelEmail1";
            this.gpgLabelEmail1.Size = new Size(0x87, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelEmail1, null);
            this.gpgLabelEmail1.TabIndex = 11;
            this.gpgLabelEmail1.Text = "<LOC>Email Address";
            this.gpgLabelEmail1.TextStyle = TextStyles.Default;
            this.gpgTextBoxEmail2.Location = new Point(11, 270);
            this.gpgTextBoxEmail2.Name = "gpgTextBoxEmail2";
            this.gpgTextBoxEmail2.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxEmail2.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxEmail2.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxEmail2.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxEmail2.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxEmail2.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxEmail2.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxEmail2.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxEmail2.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxEmail2.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxEmail2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxEmail2.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxEmail2.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxEmail2.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxEmail2.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxEmail2.Size = new Size(0x116, 20);
            this.gpgTextBoxEmail2.TabIndex = 14;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(10, 250);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0xb7, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 13;
            this.gpgLabel4.Text = "<LOC>Confirm Email Address";
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.gpgTextBoxPhone.Location = new Point(11, 0x148);
            this.gpgTextBoxPhone.Name = "gpgTextBoxPhone";
            this.gpgTextBoxPhone.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPhone.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPhone.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPhone.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPhone.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPhone.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPhone.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPhone.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPhone.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPhone.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPhone.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPhone.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPhone.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPhone.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPhone.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPhone.Size = new Size(0x116, 20);
            this.gpgTextBoxPhone.TabIndex = 0x10;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(10, 0x134);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x9e, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 15;
            this.gpgLabel5.Text = "<LOC>Telephone Number";
            this.gpgLabel5.TextStyle = TextStyles.Default;
            this.gpgTextBoxPayPal.Location = new Point(11, 0x183);
            this.gpgTextBoxPayPal.Name = "gpgTextBoxPayPal";
            this.gpgTextBoxPayPal.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPayPal.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPayPal.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPayPal.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPayPal.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPayPal.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPayPal.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPayPal.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPayPal.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPayPal.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPayPal.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPayPal.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPayPal.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPayPal.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPayPal.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPayPal.Size = new Size(0x116, 20);
            this.gpgTextBoxPayPal.TabIndex = 0x12;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(10, 0x16f);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x8e, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x11;
            this.gpgLabel6.Text = "<LOC>Paypal Account";
            this.gpgLabel6.TextStyle = TextStyles.Default;
            this.gpgTextBoxAddress1.Location = new Point(0x146, 100);
            this.gpgTextBoxAddress1.Name = "gpgTextBoxAddress1";
            this.gpgTextBoxAddress1.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxAddress1.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxAddress1.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxAddress1.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxAddress1.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxAddress1.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxAddress1.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxAddress1.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxAddress1.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxAddress1.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxAddress1.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxAddress1.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxAddress1.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxAddress1.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxAddress1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxAddress1.Size = new Size(0x116, 20);
            this.gpgTextBoxAddress1.TabIndex = 20;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoSize = true;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x145, 80);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x89, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 0x13;
            this.gpgLabel7.Text = "<LOC>Address Line 1";
            this.gpgLabel7.TextStyle = TextStyles.Default;
            this.gpgTextBoxAddress2.Location = new Point(0x146, 0x99);
            this.gpgTextBoxAddress2.Name = "gpgTextBoxAddress2";
            this.gpgTextBoxAddress2.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxAddress2.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxAddress2.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxAddress2.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxAddress2.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxAddress2.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxAddress2.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxAddress2.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxAddress2.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxAddress2.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxAddress2.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxAddress2.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxAddress2.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxAddress2.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxAddress2.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxAddress2.Size = new Size(0x116, 20);
            this.gpgTextBoxAddress2.TabIndex = 0x16;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoSize = true;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.Font = new Font("Arial", 9.75f);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x145, 0x85);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0xd9, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel8, null);
            this.gpgLabel8.TabIndex = 0x15;
            this.gpgLabel8.Text = "<LOC>Address Line 2 (if applicable)";
            this.gpgLabel8.TextStyle = TextStyles.Default;
            this.gpgTextBoxCity.Location = new Point(0x146, 210);
            this.gpgTextBoxCity.Name = "gpgTextBoxCity";
            this.gpgTextBoxCity.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxCity.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxCity.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxCity.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxCity.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxCity.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxCity.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxCity.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxCity.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxCity.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxCity.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxCity.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxCity.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxCity.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxCity.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxCity.Size = new Size(0x116, 20);
            this.gpgTextBoxCity.TabIndex = 0x18;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoSize = true;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.Font = new Font("Arial", 9.75f);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0x145, 190);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x49, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel9, null);
            this.gpgLabel9.TabIndex = 0x17;
            this.gpgLabel9.Text = "<LOC>City";
            this.gpgLabel9.TextStyle = TextStyles.Default;
            this.gpgLabel10.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel10.AutoSize = true;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.Font = new Font("Arial", 9.75f);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(0x145, 250);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0x86, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel10, null);
            this.gpgLabel10.TabIndex = 0x19;
            this.gpgLabel10.Text = "<LOC>State/Province";
            this.gpgLabel10.TextStyle = TextStyles.Default;
            this.gpgLabel11.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel11.AutoSize = true;
            this.gpgLabel11.AutoStyle = true;
            this.gpgLabel11.Font = new Font("Arial", 9.75f);
            this.gpgLabel11.ForeColor = Color.White;
            this.gpgLabel11.IgnoreMouseWheel = false;
            this.gpgLabel11.IsStyled = false;
            this.gpgLabel11.Location = new Point(0x145, 0x135);
            this.gpgLabel11.Name = "gpgLabel11";
            this.gpgLabel11.Size = new Size(0x5f, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel11, null);
            this.gpgLabel11.TabIndex = 0x1b;
            this.gpgLabel11.Text = "<LOC>Country";
            this.gpgLabel11.TextStyle = TextStyles.Default;
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
            this.skinButtonCancel.Location = new Point(0x217, 420);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(100, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 0x1f;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
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
            this.skinButtonOK.Location = new Point(0x1ad, 420);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(100, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 30;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.gpgDropDownListState.BackColor = Color.Black;
            this.gpgDropDownListState.BorderColor = Color.DimGray;
            this.gpgDropDownListState.DoValidate = true;
            this.gpgDropDownListState.DropDownStyle = ComboBoxStyle.DropDownList;
            this.gpgDropDownListState.DropDownWidth = 0xd3;
            this.gpgDropDownListState.FlatStyle = FlatStyle.Flat;
            this.gpgDropDownListState.FocusBackColor = Color.White;
            this.gpgDropDownListState.FocusBorderColor = Color.White;
            this.gpgDropDownListState.Font = new Font("Verdana", 7f);
            this.gpgDropDownListState.FormattingEnabled = true;
            this.gpgDropDownListState.IntegralHeight = false;
            this.gpgDropDownListState.ItemHeight = 12;
            this.gpgDropDownListState.Location = new Point(0x146, 270);
            this.gpgDropDownListState.MaxDropDownItems = 15;
            this.gpgDropDownListState.Name = "gpgDropDownListState";
            this.gpgDropDownListState.Size = new Size(0x8d, 20);
            base.ttDefault.SetSuperTip(this.gpgDropDownListState, null);
            this.gpgDropDownListState.TabIndex = 0x19;
            this.gpgDropDownListCountries.BackColor = Color.Black;
            this.gpgDropDownListCountries.BorderColor = Color.DimGray;
            this.gpgDropDownListCountries.DoValidate = true;
            this.gpgDropDownListCountries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.gpgDropDownListCountries.DropDownWidth = 0x10f;
            this.gpgDropDownListCountries.FlatStyle = FlatStyle.Flat;
            this.gpgDropDownListCountries.FocusBackColor = Color.White;
            this.gpgDropDownListCountries.FocusBorderColor = Color.White;
            this.gpgDropDownListCountries.Font = new Font("Verdana", 7f);
            this.gpgDropDownListCountries.FormattingEnabled = true;
            this.gpgDropDownListCountries.Location = new Point(0x148, 0x148);
            this.gpgDropDownListCountries.MaxDropDownItems = 15;
            this.gpgDropDownListCountries.Name = "gpgDropDownListCountries";
            this.gpgDropDownListCountries.Size = new Size(0x113, 20);
            base.ttDefault.SetSuperTip(this.gpgDropDownListCountries, null);
            this.gpgDropDownListCountries.TabIndex = 0x1b;
            this.gpgDropDownListCountries.SelectedIndexChanged += new EventHandler(this.gpgDropDownListCountries_SelectedIndexChanged);
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(470, 250);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0x79, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 0x21;
            this.gpgLabel3.Text = "<LOC>Postal Code";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgTextBoxPostalCode.Location = new Point(0x1d9, 270);
            this.gpgTextBoxPostalCode.Name = "gpgTextBoxPostalCode";
            this.gpgTextBoxPostalCode.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextBoxPostalCode.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextBoxPostalCode.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextBoxPostalCode.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextBoxPostalCode.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextBoxPostalCode.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextBoxPostalCode.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextBoxPostalCode.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextBoxPostalCode.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextBoxPostalCode.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextBoxPostalCode.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextBoxPostalCode.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextBoxPostalCode.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextBoxPostalCode.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextBoxPostalCode.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextBoxPostalCode.Size = new Size(130, 20);
            this.gpgTextBoxPostalCode.TabIndex = 0x1a;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x286, 0x1fd);
            base.Controls.Add(this.gpgTextBoxPostalCode);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgDropDownListState);
            base.Controls.Add(this.gpgDropDownListCountries);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.gpgLabel11);
            base.Controls.Add(this.gpgLabel10);
            base.Controls.Add(this.gpgTextBoxCity);
            base.Controls.Add(this.gpgLabel9);
            base.Controls.Add(this.gpgTextBoxAddress2);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgTextBoxAddress1);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.gpgTextBoxPayPal);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgTextBoxPhone);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgTextBoxEmail2);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgTextBoxEmail1);
            base.Controls.Add(this.gpgLabelEmail1);
            base.Controls.Add(this.gpgTextBoxLastName);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextBoxFirstName);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgPrizeWinner";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Prize Winner Information";
            base.Load += new EventHandler(this.DlgPrizeWinner_Load);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxFirstName, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxLastName, 0);
            base.Controls.SetChildIndex(this.gpgLabelEmail1, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxEmail1, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxEmail2, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPhone, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPayPal, 0);
            base.Controls.SetChildIndex(this.gpgLabel7, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxAddress1, 0);
            base.Controls.SetChildIndex(this.gpgLabel8, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxAddress2, 0);
            base.Controls.SetChildIndex(this.gpgLabel9, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxCity, 0);
            base.Controls.SetChildIndex(this.gpgLabel10, 0);
            base.Controls.SetChildIndex(this.gpgLabel11, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.gpgDropDownListCountries, 0);
            base.Controls.SetChildIndex(this.gpgDropDownListState, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgTextBoxPostalCode, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextBoxFirstName.Properties.EndInit();
            this.gpgTextBoxLastName.Properties.EndInit();
            this.gpgTextBoxEmail1.Properties.EndInit();
            this.gpgTextBoxEmail2.Properties.EndInit();
            this.gpgTextBoxPhone.Properties.EndInit();
            this.gpgTextBoxPayPal.Properties.EndInit();
            this.gpgTextBoxAddress1.Properties.EndInit();
            this.gpgTextBoxAddress2.Properties.EndInit();
            this.gpgTextBoxCity.Properties.EndInit();
            this.gpgTextBoxPostalCode.Properties.EndInit();
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
            bool flag = false;
            if ((this.gpgTextBoxFirstName.Text == null) || (this.gpgTextBoxFirstName.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxFirstName, "<LOC>Required Field", new object[0]);
            }
            if ((this.gpgTextBoxLastName.Text == null) || (this.gpgTextBoxLastName.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxLastName, "<LOC>Required Field", new object[0]);
            }
            if ((this.gpgTextBoxEmail1.Text == null) || (this.gpgTextBoxEmail1.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxEmail1, "<LOC>Required Field", new object[0]);
            }
            if ((this.gpgTextBoxEmail2.Text == null) || (this.gpgTextBoxEmail2.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxEmail2, "<LOC>Required Field", new object[0]);
            }
            if (this.gpgTextBoxEmail1.Text != this.gpgTextBoxEmail2.Text)
            {
                flag = true;
                base.Error(new Control[] { this.gpgTextBoxEmail1, this.gpgTextBoxEmail2 }, "<LOC>Email entries do not match", new object[0]);
            }
            if ((this.gpgTextBoxPhone.Text == null) || (this.gpgTextBoxPhone.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxPhone, "<LOC>Required Field", new object[0]);
            }
            if ((this.gpgTextBoxPayPal.Text == null) || (this.gpgTextBoxPayPal.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxPayPal, "<LOC>Required Field", new object[0]);
            }
            if ((this.gpgTextBoxAddress1.Text == null) || (this.gpgTextBoxAddress1.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxAddress1, "<LOC>Required Field", new object[0]);
            }
            if ((this.gpgTextBoxCity.Text == null) || (this.gpgTextBoxCity.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxCity, "<LOC>Required Field", new object[0]);
            }
            if ((this.gpgTextBoxPostalCode.Text == null) || (this.gpgTextBoxPostalCode.Text.Length < 1))
            {
                flag = true;
                base.Error(this.gpgTextBoxPostalCode, "<LOC>Required Field", new object[0]);
            }
            if (!flag)
            {
                if (new QuazalQuery("SubmitPrizeInfo", new object[] { this.gpgTextBoxFirstName.Text, this.gpgTextBoxLastName.Text, this.gpgTextBoxEmail1.Text, this.gpgTextBoxAddress1.Text, this.gpgTextBoxAddress2.Text, this.gpgTextBoxCity.Text, (string) this.gpgDropDownListState.SelectedItem, this.gpgTextBoxPostalCode.Text, (string) this.gpgDropDownListCountries.SelectedItem, this.gpgTextBoxPhone.Text, this.gpgTextBoxPayPal.Text }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("<LOC>Prize redemption information has been successfully submitted.");
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
                else
                {
                    DlgMessage.ShowDialog("<LOC>You have already submitted this information.");
                    base.DialogResult = DialogResult.Cancel;
                    base.Close();
                }
            }
        }
    }
}

