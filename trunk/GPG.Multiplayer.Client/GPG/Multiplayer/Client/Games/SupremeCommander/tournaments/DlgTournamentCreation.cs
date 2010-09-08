namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraScheduler;
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
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
    using System.Windows.Forms;

    public class DlgTournamentCreation : DlgBase
    {
        private SkinLabel backLabelTitle;
        private GPGComboBox cbDate;
        private GPGDropDownList cbFaction;
        private GPGDropDownList cbMap;
        private GPGDropDownList cbNumberOfRounds;
        private GPGComboBoxPanel cbpCalander;
        private GPGDropDownList cbRestriction;
        private GPGDropDownList cbRoundLength;
        private GPGDropDownList cbType;
        private IContainer components = null;
        private DateNavigator dTournamentDate;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel10;
        private GPGLabel gpgLabel11;
        private GPGLabel gpgLabel12;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabel9;
        private GPGLabel lMaxPlayers;
        private GPGLabel lSize;
        public static Image mAeonImage = Image.FromFile(Application.StartupPath + @"\Skins\Default\Controls\Background Label\Tournaments\miniaeon.png");
        public static Image mAnyImage = Image.FromFile(Application.StartupPath + @"\Skins\Default\Controls\Background Label\Tournaments\miniany.png");
        public static Image mAnyMap = Image.FromFile(Application.StartupPath + @"\Skins\Default\Controls\Background Label\Tournaments\laddermap.png");
        public static Image mCybranImage = Image.FromFile(Application.StartupPath + @"\Skins\Default\Controls\Background Label\Tournaments\minicybran.png");
        private bool mModified = false;
        private int mTournamentID = 0;
        public static Image mUEFImage = Image.FromFile(Application.StartupPath + @"\Skins\Default\Controls\Background Label\Tournaments\miniuef.png");
        private GPGPictureBox pbFaction;
        private GPGPictureBox pbMap;
        public SkinButton skinButtonCancel;
        public SkinButton skinButtonOK;
        private GPGTextBox tbAssistantTDs;
        private GPGTextArea tbDescription;
        private GPGTextArea tbLegal;
        private GPGTextBox tbName;
        private GPGTextBox tbWebURL;
        private TimeEdit tTournamentTime;

        public DlgTournamentCreation()
        {
            this.InitializeComponent();
            this.pbFaction.Image = mAnyImage;
            this.pbMap.Image = mAnyImage;
            this.cbMap.Items.Add("Ladder Maps");
            this.pbMap.Image = mAnyMap;
            this.lMaxPlayers.Text = "Max Players: Variable";
            this.lSize.Text = "Size: Variable";
            this.cbDate.EditValue = DateTime.Now;
            foreach (SupcomMap map in SupcomMapList.Maps)
            {
                this.cbMap.Items.Add(map);
            }
            DataList queryData = DataAccess.GetQueryData("Tournament Filters", new object[0]);
            foreach (DataRecord record in queryData)
            {
                this.cbRestriction.Items.Add(record["query_name"]);
            }
        }

        private void cbDate_QueryPopUp(object sender, CancelEventArgs e)
        {
            this.tTournamentTime.EditValue = this.cbDate.EditValue;
            try
            {
                this.dTournamentDate.DateTime = DateTime.Parse(this.cbDate.EditValue.ToString());
            }
            catch
            {
            }
        }

        private void cbFaction_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.cbFaction.Text == "Aeon")
            {
                this.pbFaction.Image = mAeonImage;
            }
            else if (this.cbFaction.Text == "Cybran")
            {
                this.pbFaction.Image = mCybranImage;
            }
            else if (this.cbFaction.Text == "UEF")
            {
                this.pbFaction.Image = mUEFImage;
            }
            else
            {
                this.pbFaction.Image = mAnyImage;
            }
        }

        private void cbMap_SelectedValueChanged(object sender, EventArgs e)
        {
            this.pbMap.Image = mAnyMap;
            this.lMaxPlayers.Text = "Max Players: Variable";
            this.lSize.Text = "Size: Variable";
            SupcomMap selectedItem = this.cbMap.SelectedItem as SupcomMap;
            if (selectedItem != null)
            {
                this.pbMap.Image = selectedItem.Image;
                this.lMaxPlayers.Text = "Max Players: " + selectedItem.MaxPlayers.ToString();
                this.lSize.Text = "Size: " + selectedItem.MapSize;
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

        private void dTournamentDate_Click(object sender, EventArgs e)
        {
            this.SetDateTime();
        }

        private void dTournamentDate_EditDateModified(object sender, EventArgs e)
        {
        }

        private object GetCustomFilter()
        {
            if (this.cbRestriction.Text.ToUpper() != "NONE")
            {
                return this.cbRestriction.Text;
            }
            return "TQ - No Restriction";
        }

        private string GetMySQLDateTime(DateTime date)
        {
            return (date.Year.ToString() + "-" + date.Month.ToString().PadLeft(2, '0') + "-" + date.Day.ToString().PadLeft(2, '0') + " " + date.Hour.ToString().PadLeft(2, '0') + ":" + date.Minute.ToString().PadLeft(2, '0') + ":" + date.Second.ToString().PadLeft(2, '0'));
        }

        private int GetNumRounds()
        {
            int num = -1;
            try
            {
                num = Convert.ToInt32(this.cbNumberOfRounds.SelectedItem.ToString());
            }
            catch
            {
            }
            return num;
        }

        private int GetRoundMinutes()
        {
            int num = -1;
            try
            {
                num = Convert.ToInt32(this.cbRoundLength.SelectedItem.ToString().Replace(" minutes", ""));
            }
            catch
            {
            }
            return num;
        }

        private void gpgTextBox3_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.backLabelTitle = new SkinLabel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.tbName = new GPGTextBox();
            this.tbDescription = new GPGTextArea();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.tbLegal = new GPGTextArea();
            this.gpgLabel5 = new GPGLabel();
            this.tbWebURL = new GPGTextBox();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.gpgLabel10 = new GPGLabel();
            this.gpgLabel11 = new GPGLabel();
            this.tbAssistantTDs = new GPGTextBox();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.cbType = new GPGDropDownList();
            this.cbFaction = new GPGDropDownList();
            this.cbNumberOfRounds = new GPGDropDownList();
            this.cbRoundLength = new GPGDropDownList();
            this.cbMap = new GPGDropDownList();
            this.pbFaction = new GPGPictureBox();
            this.pbMap = new GPGPictureBox();
            this.lSize = new GPGLabel();
            this.lMaxPlayers = new GPGLabel();
            this.cbDate = new GPGComboBox();
            this.cbpCalander = new GPGComboBoxPanel();
            this.dTournamentDate = new DateNavigator();
            this.tTournamentTime = new TimeEdit();
            this.cbRestriction = new GPGDropDownList();
            this.gpgLabel12 = new GPGLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.tbName.Properties.BeginInit();
            this.tbDescription.Properties.BeginInit();
            this.tbLegal.Properties.BeginInit();
            this.tbWebURL.Properties.BeginInit();
            this.tbAssistantTDs.Properties.BeginInit();
            ((ISupportInitialize) this.pbFaction).BeginInit();
            ((ISupportInitialize) this.pbMap).BeginInit();
            this.cbDate.Properties.BeginInit();
            this.cbpCalander.BeginInit();
            this.cbpCalander.SuspendLayout();
            this.dTournamentDate.BeginInit();
            this.tTournamentTime.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x2ef, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.backLabelTitle.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.backLabelTitle.AutoStyle = false;
            this.backLabelTitle.BackColor = Color.Transparent;
            this.backLabelTitle.DrawEdges = true;
            this.backLabelTitle.Font = new Font("Arial", 20f, FontStyle.Bold);
            this.backLabelTitle.ForeColor = Color.White;
            this.backLabelTitle.HorizontalScalingMode = ScalingModes.Tile;
            this.backLabelTitle.IsStyled = false;
            this.backLabelTitle.Location = new Point(7, 0x3e);
            this.backLabelTitle.Name = "backLabelTitle";
            this.backLabelTitle.Size = new Size(0x31c, 0x3b);
            this.backLabelTitle.SkinBasePath = @"Controls\Background Label\Tournaments";
            base.ttDefault.SetSuperTip(this.backLabelTitle, null);
            this.backLabelTitle.TabIndex = 8;
            this.backLabelTitle.Text = "<LOC>Create New Tournament";
            this.backLabelTitle.TextAlign = ContentAlignment.MiddleLeft;
            this.backLabelTitle.TextPadding = new Padding(10, 0, 0, 0);
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(7, 0x98);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xa6, 0x17);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 9;
            this.gpgLabel1.Text = "<LOC>Tournament Name";
            this.gpgLabel1.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(7, 0xb3);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0xa6, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 10;
            this.gpgLabel2.Text = "<LOC>Description";
            this.gpgLabel2.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.tbName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.tbName.Location = new Point(0xb3, 0x98);
            this.tbName.Name = "tbName";
            this.tbName.Properties.Appearance.BackColor = Color.Black;
            this.tbName.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbName.Properties.Appearance.ForeColor = Color.White;
            this.tbName.Properties.Appearance.Options.UseBackColor = true;
            this.tbName.Properties.Appearance.Options.UseBorderColor = true;
            this.tbName.Properties.Appearance.Options.UseForeColor = true;
            this.tbName.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbName.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbName.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbName.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbName.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbName.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbName.Properties.BorderStyle = BorderStyles.Simple;
            this.tbName.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbName.Size = new Size(0x11d, 20);
            this.tbName.TabIndex = 11;
            this.tbDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tbDescription.BorderColor = Color.White;
            this.tbDescription.Location = new Point(0xb3, 0xb2);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Properties.Appearance.BackColor = Color.Black;
            this.tbDescription.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbDescription.Properties.Appearance.ForeColor = Color.White;
            this.tbDescription.Properties.Appearance.Options.UseBackColor = true;
            this.tbDescription.Properties.Appearance.Options.UseBorderColor = true;
            this.tbDescription.Properties.Appearance.Options.UseForeColor = true;
            this.tbDescription.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbDescription.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbDescription.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbDescription.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbDescription.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbDescription.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbDescription.Properties.BorderStyle = BorderStyles.Simple;
            this.tbDescription.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbDescription.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbDescription.Size = new Size(0x25f, 0x3b);
            this.tbDescription.TabIndex = 12;
            this.gpgLabel3.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(470, 0x98);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0xb2, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 13;
            this.gpgLabel3.Text = "<LOC>Tournament Type";
            this.gpgLabel3.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgLabel4.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(7, 0xf4);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0xa6, 0x2e);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 15;
            this.gpgLabel4.Text = "<LOC>Legal (use with prizes)";
            this.gpgLabel4.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.tbLegal.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.tbLegal.BorderColor = Color.White;
            this.tbLegal.Location = new Point(0xb3, 0xf4);
            this.tbLegal.Name = "tbLegal";
            this.tbLegal.Properties.Appearance.BackColor = Color.Black;
            this.tbLegal.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbLegal.Properties.Appearance.ForeColor = Color.White;
            this.tbLegal.Properties.Appearance.Options.UseBackColor = true;
            this.tbLegal.Properties.Appearance.Options.UseBorderColor = true;
            this.tbLegal.Properties.Appearance.Options.UseForeColor = true;
            this.tbLegal.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbLegal.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbLegal.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbLegal.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbLegal.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbLegal.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbLegal.Properties.BorderStyle = BorderStyles.Simple;
            this.tbLegal.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbLegal.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbLegal.Size = new Size(0x25f, 0x3b);
            this.tbLegal.TabIndex = 0x10;
            this.gpgLabel5.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(7, 310);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0xa6, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x11;
            this.gpgLabel5.Text = "<LOC>Website URL";
            this.gpgLabel5.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel5.TextStyle = TextStyles.Default;
            this.tbWebURL.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.tbWebURL.Location = new Point(0xb3, 310);
            this.tbWebURL.Name = "tbWebURL";
            this.tbWebURL.Properties.Appearance.BackColor = Color.Black;
            this.tbWebURL.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbWebURL.Properties.Appearance.ForeColor = Color.White;
            this.tbWebURL.Properties.Appearance.Options.UseBackColor = true;
            this.tbWebURL.Properties.Appearance.Options.UseBorderColor = true;
            this.tbWebURL.Properties.Appearance.Options.UseForeColor = true;
            this.tbWebURL.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbWebURL.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbWebURL.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbWebURL.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbWebURL.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbWebURL.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbWebURL.Properties.BorderStyle = BorderStyles.Simple;
            this.tbWebURL.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbWebURL.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbWebURL.Size = new Size(0x25f, 20);
            this.tbWebURL.TabIndex = 0x12;
            this.gpgLabel6.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(7, 0x150);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0xa6, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x1b;
            this.gpgLabel6.Text = "<LOC>Faction Restriction";
            this.gpgLabel6.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel6.TextStyle = TextStyles.Default;
            this.gpgLabel7.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x144, 0x150);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0xad, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 0x1c;
            this.gpgLabel7.Text = "<LOC>Map Restriction";
            this.gpgLabel7.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel7.TextStyle = TextStyles.Default;
            this.gpgLabel8.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.Font = new Font("Arial", 9.75f);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(7, 0x184);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0xa6, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel8, null);
            this.gpgLabel8.TabIndex = 0x1d;
            this.gpgLabel8.Text = "<LOC>Tournament Date";
            this.gpgLabel8.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel8.TextStyle = TextStyles.Default;
            this.gpgLabel9.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.Font = new Font("Arial", 9.75f);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(15, 440);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x9b, 0x13);
            base.ttDefault.SetSuperTip(this.gpgLabel9, null);
            this.gpgLabel9.TabIndex = 30;
            this.gpgLabel9.Text = "<LOC>Round Length";
            this.gpgLabel9.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel9.TextStyle = TextStyles.Default;
            this.gpgLabel10.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel10.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel10.AutoStyle = true;
            this.gpgLabel10.Font = new Font("Arial", 9.75f);
            this.gpgLabel10.ForeColor = Color.White;
            this.gpgLabel10.IgnoreMouseWheel = false;
            this.gpgLabel10.IsStyled = false;
            this.gpgLabel10.Location = new Point(7, 0x19f);
            this.gpgLabel10.Name = "gpgLabel10";
            this.gpgLabel10.Size = new Size(0xa6, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel10, null);
            this.gpgLabel10.TabIndex = 0x1f;
            this.gpgLabel10.Text = "<LOC>Number of Rounds";
            this.gpgLabel10.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel10.TextStyle = TextStyles.Default;
            this.gpgLabel11.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel11.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel11.AutoStyle = true;
            this.gpgLabel11.Font = new Font("Arial", 9.75f);
            this.gpgLabel11.ForeColor = Color.White;
            this.gpgLabel11.IgnoreMouseWheel = false;
            this.gpgLabel11.IsStyled = false;
            this.gpgLabel11.Location = new Point(12, 0x1d2);
            this.gpgLabel11.Name = "gpgLabel11";
            this.gpgLabel11.Size = new Size(0xa1, 0x13);
            base.ttDefault.SetSuperTip(this.gpgLabel11, null);
            this.gpgLabel11.TabIndex = 0x20;
            this.gpgLabel11.Text = "<LOC>Assistant TDs";
            this.gpgLabel11.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel11.TextStyle = TextStyles.Default;
            this.tbAssistantTDs.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.tbAssistantTDs.Location = new Point(0xb3, 0x1d1);
            this.tbAssistantTDs.Name = "tbAssistantTDs";
            this.tbAssistantTDs.Properties.Appearance.BackColor = Color.Black;
            this.tbAssistantTDs.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.tbAssistantTDs.Properties.Appearance.ForeColor = Color.White;
            this.tbAssistantTDs.Properties.Appearance.Options.UseBackColor = true;
            this.tbAssistantTDs.Properties.Appearance.Options.UseBorderColor = true;
            this.tbAssistantTDs.Properties.Appearance.Options.UseForeColor = true;
            this.tbAssistantTDs.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.tbAssistantTDs.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.tbAssistantTDs.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.tbAssistantTDs.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.tbAssistantTDs.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.tbAssistantTDs.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.tbAssistantTDs.Properties.BorderStyle = BorderStyles.Simple;
            this.tbAssistantTDs.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.tbAssistantTDs.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbAssistantTDs.Size = new Size(0x194, 20);
            this.tbAssistantTDs.TabIndex = 0x21;
            this.tbAssistantTDs.EditValueChanged += new EventHandler(this.gpgTextBox3_EditValueChanged);
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
            this.skinButtonCancel.Location = new Point(0x2b5, 0x1d3);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5d, 0x1c);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 0x23;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
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
            this.skinButtonOK.Location = new Point(0x253, 0x1d3);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5d, 0x1c);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 0x22;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>Create";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.cbType.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.cbType.BackColor = Color.Black;
            this.cbType.BorderColor = Color.White;
            this.cbType.DoValidate = true;
            this.cbType.FlatStyle = FlatStyle.Flat;
            this.cbType.FocusBackColor = Color.Empty;
            this.cbType.FocusBorderColor = Color.Empty;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] { "Swiss Style", "Manual Brackets" });
            this.cbType.Location = new Point(0x28e, 0x98);
            this.cbType.Name = "cbType";
            this.cbType.Size = new Size(0x84, 0x15);
            base.ttDefault.SetSuperTip(this.cbType, null);
            this.cbType.TabIndex = 0x25;
            this.cbFaction.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbFaction.BackColor = Color.Black;
            this.cbFaction.BorderColor = Color.White;
            this.cbFaction.DoValidate = true;
            this.cbFaction.FlatStyle = FlatStyle.Flat;
            this.cbFaction.FocusBackColor = Color.Empty;
            this.cbFaction.FocusBorderColor = Color.Empty;
            this.cbFaction.FormattingEnabled = true;
            this.cbFaction.Items.AddRange(new object[] { "Any Faction", "Random Faction", "Aeon", "Cybran", "UEF" });
            this.cbFaction.Location = new Point(0xb3, 0x150);
            this.cbFaction.Name = "cbFaction";
            this.cbFaction.Size = new Size(0x84, 0x15);
            base.ttDefault.SetSuperTip(this.cbFaction, null);
            this.cbFaction.TabIndex = 0x26;
            this.cbFaction.SelectedValueChanged += new EventHandler(this.cbFaction_SelectedValueChanged);
            this.cbNumberOfRounds.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbNumberOfRounds.BackColor = Color.Black;
            this.cbNumberOfRounds.BorderColor = Color.White;
            this.cbNumberOfRounds.DoValidate = true;
            this.cbNumberOfRounds.FlatStyle = FlatStyle.Flat;
            this.cbNumberOfRounds.FocusBackColor = Color.Empty;
            this.cbNumberOfRounds.FocusBorderColor = Color.Empty;
            this.cbNumberOfRounds.FormattingEnabled = true;
            this.cbNumberOfRounds.Items.AddRange(new object[] { "Automatic", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            this.cbNumberOfRounds.Location = new Point(0xb3, 0x19c);
            this.cbNumberOfRounds.Name = "cbNumberOfRounds";
            this.cbNumberOfRounds.Size = new Size(0xb8, 0x15);
            base.ttDefault.SetSuperTip(this.cbNumberOfRounds, null);
            this.cbNumberOfRounds.TabIndex = 40;
            this.cbRoundLength.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbRoundLength.BackColor = Color.Black;
            this.cbRoundLength.BorderColor = Color.White;
            this.cbRoundLength.DoValidate = true;
            this.cbRoundLength.FlatStyle = FlatStyle.Flat;
            this.cbRoundLength.FocusBackColor = Color.Empty;
            this.cbRoundLength.FocusBorderColor = Color.Empty;
            this.cbRoundLength.FormattingEnabled = true;
            this.cbRoundLength.Items.AddRange(new object[] { "Until all games are finished", "60 minutes", "50 minutes", "40 minutes", "30 minutes", "20 minutes", "10 minutes" });
            this.cbRoundLength.Location = new Point(0xb3, 0x1b6);
            this.cbRoundLength.Name = "cbRoundLength";
            this.cbRoundLength.Size = new Size(0xb8, 0x15);
            base.ttDefault.SetSuperTip(this.cbRoundLength, null);
            this.cbRoundLength.TabIndex = 0x29;
            this.cbMap.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbMap.BackColor = Color.Black;
            this.cbMap.BorderColor = Color.White;
            this.cbMap.DisplayMember = "MapCheckName";
            this.cbMap.DoValidate = true;
            this.cbMap.FlatStyle = FlatStyle.Flat;
            this.cbMap.FocusBackColor = Color.Empty;
            this.cbMap.FocusBorderColor = Color.Empty;
            this.cbMap.FormattingEnabled = true;
            this.cbMap.Location = new Point(0x1f7, 0x14f);
            this.cbMap.Name = "cbMap";
            this.cbMap.Size = new Size(0xa3, 0x15);
            base.ttDefault.SetSuperTip(this.cbMap, null);
            this.cbMap.TabIndex = 0x2a;
            this.cbMap.Text = "Any Map";
            this.cbMap.SelectedValueChanged += new EventHandler(this.cbMap_SelectedValueChanged);
            this.pbFaction.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.pbFaction.Location = new Point(0x13e, 0x14b);
            this.pbFaction.Name = "pbFaction";
            this.pbFaction.Size = new Size(50, 50);
            base.ttDefault.SetSuperTip(this.pbFaction, null);
            this.pbFaction.TabIndex = 0x2b;
            this.pbFaction.TabStop = false;
            this.pbMap.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.pbMap.Location = new Point(0x2a0, 0x14f);
            this.pbMap.MaximumSize = new Size(200, 200);
            this.pbMap.Name = "pbMap";
            this.pbMap.Size = new Size(0x7d, 0x7d);
            this.pbMap.SizeMode = PictureBoxSizeMode.StretchImage;
            base.ttDefault.SetSuperTip(this.pbMap, null);
            this.pbMap.TabIndex = 0x2c;
            this.pbMap.TabStop = false;
            this.lSize.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lSize.AutoGrowDirection = GrowDirections.None;
            this.lSize.AutoStyle = true;
            this.lSize.Font = new Font("Arial", 9.75f);
            this.lSize.ForeColor = Color.White;
            this.lSize.IgnoreMouseWheel = false;
            this.lSize.IsStyled = false;
            this.lSize.Location = new Point(0x1ed, 0x16d);
            this.lSize.Name = "lSize";
            this.lSize.Size = new Size(0xad, 0x10);
            base.ttDefault.SetSuperTip(this.lSize, null);
            this.lSize.TabIndex = 0x2d;
            this.lSize.Text = "Size: 64, 64";
            this.lSize.TextAlign = ContentAlignment.TopRight;
            this.lSize.TextStyle = TextStyles.Default;
            this.lMaxPlayers.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.lMaxPlayers.AutoGrowDirection = GrowDirections.None;
            this.lMaxPlayers.AutoStyle = true;
            this.lMaxPlayers.Font = new Font("Arial", 9.75f);
            this.lMaxPlayers.ForeColor = Color.White;
            this.lMaxPlayers.IgnoreMouseWheel = false;
            this.lMaxPlayers.IsStyled = false;
            this.lMaxPlayers.Location = new Point(0x1ed, 0x17d);
            this.lMaxPlayers.Name = "lMaxPlayers";
            this.lMaxPlayers.Size = new Size(0xad, 0x10);
            base.ttDefault.SetSuperTip(this.lMaxPlayers, null);
            this.lMaxPlayers.TabIndex = 0x2e;
            this.lMaxPlayers.Text = "Max Players: 8";
            this.lMaxPlayers.TextAlign = ContentAlignment.TopRight;
            this.lMaxPlayers.TextStyle = TextStyles.Default;
            this.cbDate.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbDate.BorderColor = Color.White;
            this.cbDate.Location = new Point(0xb3, 0x182);
            this.cbDate.Name = "cbDate";
            this.cbDate.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.cbDate.Properties.NullText = "None";
            this.cbDate.Properties.PopupControl = this.cbpCalander;
            this.cbDate.Size = new Size(0xb8, 20);
            this.cbDate.TabIndex = 0x2f;
            this.cbDate.QueryPopUp += new CancelEventHandler(this.cbDate_QueryPopUp);
            this.cbpCalander.Appearance.BackColor = Color.Black;
            this.cbpCalander.Appearance.ForeColor = Color.White;
            this.cbpCalander.Appearance.Options.UseBackColor = true;
            this.cbpCalander.Appearance.Options.UseForeColor = true;
            this.cbpCalander.Controls.Add(this.dTournamentDate);
            this.cbpCalander.Controls.Add(this.tTournamentTime);
            this.cbpCalander.Location = new Point(0xb3, 0x19c);
            this.cbpCalander.LookAndFeel.SkinName = "London Liquid Sky";
            this.cbpCalander.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.cbpCalander.LookAndFeel.UseDefaultLookAndFeel = false;
            this.cbpCalander.Name = "cbpCalander";
            this.cbpCalander.Size = new Size(0xb8, 0xcc);
            base.ttDefault.SetSuperTip(this.cbpCalander, null);
            this.cbpCalander.TabIndex = 0x30;
            this.cbpCalander.Text = "gpgComboBoxPanel1";
            this.dTournamentDate.DateTime = new DateTime(0x7d7, 4, 0x10, 0, 0, 0, 0);
            this.dTournamentDate.Dock = DockStyle.Fill;
            this.dTournamentDate.Location = new Point(0, 20);
            this.dTournamentDate.Name = "dTournamentDate";
            this.dTournamentDate.Size = new Size(0xb8, 0xb8);
            base.ttDefault.SetSuperTip(this.dTournamentDate, null);
            this.dTournamentDate.TabIndex = 1;
            this.dTournamentDate.Click += new EventHandler(this.dTournamentDate_Click);
            this.dTournamentDate.EditDateModified += new EventHandler(this.dTournamentDate_EditDateModified);
            this.tTournamentTime.Dock = DockStyle.Top;
            this.tTournamentTime.EditValue = new DateTime(0x7d7, 4, 0x10, 0, 0, 0, 0);
            this.tTournamentTime.Location = new Point(0, 0);
            this.tTournamentTime.Name = "tTournamentTime";
            this.tTournamentTime.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.tTournamentTime.Size = new Size(0xb8, 20);
            this.tTournamentTime.TabIndex = 0;
            this.tTournamentTime.EditValueChanged += new EventHandler(this.tTournamentTime_EditValueChanged);
            this.tTournamentTime.Click += new EventHandler(this.tTournamentTime_Click);
            this.cbRestriction.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbRestriction.BackColor = Color.Black;
            this.cbRestriction.BorderColor = Color.White;
            this.cbRestriction.DoValidate = true;
            this.cbRestriction.FlatStyle = FlatStyle.Flat;
            this.cbRestriction.FocusBackColor = Color.Empty;
            this.cbRestriction.FocusBorderColor = Color.Empty;
            this.cbRestriction.FormattingEnabled = true;
            this.cbRestriction.Location = new Point(0x1f7, 0x1af);
            this.cbRestriction.Name = "cbRestriction";
            this.cbRestriction.Size = new Size(0xa3, 0x15);
            base.ttDefault.SetSuperTip(this.cbRestriction, null);
            this.cbRestriction.TabIndex = 50;
            this.cbRestriction.Text = "Any Map";
            this.gpgLabel12.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel12.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel12.AutoStyle = true;
            this.gpgLabel12.Font = new Font("Arial", 9.75f);
            this.gpgLabel12.ForeColor = Color.White;
            this.gpgLabel12.IgnoreMouseWheel = false;
            this.gpgLabel12.IsStyled = false;
            this.gpgLabel12.Location = new Point(0x171, 0x1b0);
            this.gpgLabel12.Name = "gpgLabel12";
            this.gpgLabel12.Size = new Size(0x80, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel12, null);
            this.gpgLabel12.TabIndex = 0x31;
            this.gpgLabel12.Text = "<LOC>Custom Restriction";
            this.gpgLabel12.TextAlign = ContentAlignment.TopRight;
            this.gpgLabel12.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(810, 540);
            base.Controls.Add(this.cbRestriction);
            base.Controls.Add(this.gpgLabel12);
            base.Controls.Add(this.cbpCalander);
            base.Controls.Add(this.cbDate);
            base.Controls.Add(this.lMaxPlayers);
            base.Controls.Add(this.lSize);
            base.Controls.Add(this.pbMap);
            base.Controls.Add(this.cbMap);
            base.Controls.Add(this.cbRoundLength);
            base.Controls.Add(this.cbNumberOfRounds);
            base.Controls.Add(this.cbFaction);
            base.Controls.Add(this.cbType);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.tbAssistantTDs);
            base.Controls.Add(this.gpgLabel10);
            base.Controls.Add(this.gpgLabel8);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.tbWebURL);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.tbLegal);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.tbDescription);
            base.Controls.Add(this.tbName);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.backLabelTitle);
            base.Controls.Add(this.gpgLabel9);
            base.Controls.Add(this.gpgLabel11);
            base.Controls.Add(this.pbFaction);
            base.Controls.Add(this.gpgLabel7);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(810, 540);
            base.Name = "DlgTournamentCreation";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Create New Tournament";
            base.Controls.SetChildIndex(this.gpgLabel7, 0);
            base.Controls.SetChildIndex(this.pbFaction, 0);
            base.Controls.SetChildIndex(this.gpgLabel11, 0);
            base.Controls.SetChildIndex(this.gpgLabel9, 0);
            base.Controls.SetChildIndex(this.backLabelTitle, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.tbName, 0);
            base.Controls.SetChildIndex(this.tbDescription, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.tbLegal, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.tbWebURL, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.gpgLabel8, 0);
            base.Controls.SetChildIndex(this.gpgLabel10, 0);
            base.Controls.SetChildIndex(this.tbAssistantTDs, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.cbType, 0);
            base.Controls.SetChildIndex(this.cbFaction, 0);
            base.Controls.SetChildIndex(this.cbNumberOfRounds, 0);
            base.Controls.SetChildIndex(this.cbRoundLength, 0);
            base.Controls.SetChildIndex(this.cbMap, 0);
            base.Controls.SetChildIndex(this.pbMap, 0);
            base.Controls.SetChildIndex(this.lSize, 0);
            base.Controls.SetChildIndex(this.lMaxPlayers, 0);
            base.Controls.SetChildIndex(this.cbDate, 0);
            base.Controls.SetChildIndex(this.cbpCalander, 0);
            base.Controls.SetChildIndex(this.gpgLabel12, 0);
            base.Controls.SetChildIndex(this.cbRestriction, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.tbName.Properties.EndInit();
            this.tbDescription.Properties.EndInit();
            this.tbLegal.Properties.EndInit();
            this.tbWebURL.Properties.EndInit();
            this.tbAssistantTDs.Properties.EndInit();
            ((ISupportInitialize) this.pbFaction).EndInit();
            ((ISupportInitialize) this.pbMap).EndInit();
            this.cbDate.Properties.EndInit();
            this.cbpCalander.EndInit();
            this.cbpCalander.ResumeLayout(false);
            this.dTournamentDate.EndInit();
            this.tTournamentTime.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadTournament(int tournamentid)
        {
            base.SetStatus(Loc.Get("<LOC>Loading tournament") + " " + tournamentid.ToString(), new object[0]);
            this.SetControlStatus(false);
            this.skinButtonOK.Text = Loc.Get("<LOC>Update");
            this.mTournamentID = tournamentid;
            this.mModified = true;
            ThreadQueue.QueueUserWorkItem(delegate (object o) {
                VGen1 method = null;
                int num = (int) (o as object[])[0];
                DataList queryData = DataAccess.GetQueryData("Tournament Modify", new object[] { num });
                foreach (DataRecord record in queryData)
                {
                    if (method == null)
                    {
                        method = delegate (object p) {
                            DataRecord record = p as DataRecord;
                            this.tbName.Text = record["name"];
                            this.tbDescription.Text = record["description"];
                            this.tbLegal.Text = record["legal"];
                            this.cbType.Text = record["kind"];
                            this.tbWebURL.Text = record["url"];
                            this.cbDate.EditValue = DlgTournamentRegistration.MySQLToDateTime(record["tournament_date"]);
                            this.cbNumberOfRounds.Text = record["round_count"];
                            this.cbRoundLength.Text = record["round_length"] + " minutes";
                            this.cbFaction.Text = record["faction"];
                            this.cbMap.Text = record["map"];
                            if ((record["custom_filter_query_name"].Trim() != "") && (record["custom_filter_query_name"].ToUpper() != "(NULL)"))
                            {
                                this.cbRestriction.Text = record["custom_filter_query_name"];
                            }
                            else
                            {
                                this.cbRestriction.Text = "TQ - No Restriction";
                            }
                            this.SetControlStatus(true);
                            base.SetStatus("", new object[0]);
                        };
                    }
                    base.Invoke(method, new object[] { record });
                }
            }, new object[] { tournamentid });
        }

        public void SetControlStatus(bool status)
        {
            foreach (Control control in base.Controls)
            {
                if ((((control is GPGTextBox) || (control is GPGDropDownList)) || (control is SkinButton)) || (control is GPGTextArea))
                {
                    control.Enabled = status;
                }
            }
        }

        private void SetDateTime()
        {
            DateTime date = this.dTournamentDate.DateTime.Date;
            DateTime time2 = DateTime.Parse(this.tTournamentTime.EditValue.ToString());
            date = date.Add(time2.TimeOfDay);
            this.cbDate.EditValue = date;
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            WaitCallback callback2 = null;
            this.tbName.ErrorText = "";
            bool flag = false;
            if (this.tbName.Text.Trim() == "")
            {
                this.tbName.ErrorText = Loc.Get("<LOC>You must enter a name.");
                flag = true;
            }
            if (this.tbDescription.Text.Trim() == "")
            {
                this.tbDescription.ErrorText = Loc.Get("<LOC>You must provide a description for the tournament.");
                flag = true;
            }
            if (!flag)
            {
                this.skinButtonOK.Enabled = false;
                this.skinButtonCancel.Enabled = false;
                if (!this.mModified)
                {
                    if (callBack == null)
                    {
                        callBack = delegate (object o) {
                            VGen0 method = null;
                            int num = -1;
                            try
                            {
                                num = Convert.ToInt32(DataAccess.GetQueryData("Tournament Get Most Recent", new object[0])[0][0]);
                            }
                            catch
                            {
                            }
                            DataAccess.ExecuteQuery("Tournament New2", new object[] { this.tbName.Text, this.cbType.SelectedItem.ToString(), this.tbDescription.Text, this.tbLegal.Text, this.tbWebURL.Text, this.cbFaction.SelectedItem.ToString(), this.cbMap.SelectedItem.ToString(), this.GetRoundMinutes(), this.GetNumRounds(), this.GetMySQLDateTime(DateTime.Parse(this.cbDate.EditValue.ToString()).ToUniversalTime()), this.GetCustomFilter() });
                            int num2 = Convert.ToInt32(DataAccess.GetQueryData("Tournament Get Most Recent", new object[0])[0][0]);
                            if (num != num2)
                            {
                                foreach (string str in this.tbAssistantTDs.Text.Split(new char[] { ',' }))
                                {
                                    DataAccess.ExecuteQuery("Tournament New TD", new object[] { num2, str.Trim() });
                                }
                                if (method == null)
                                {
                                    method = delegate {
                                        DlgMessage.ShowDialog(Loc.Get("<LOC>Tournament Data Saved."));
                                        base.Close();
                                    };
                                }
                                base.BeginInvoke(method);
                            }
                            else
                            {
                                base.BeginInvoke(delegate {
                                    DlgMessage.ShowDialog(Loc.Get("<LOC>An error occured when trying to save the tournament."));
                                });
                            }
                        };
                    }
                    ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                }
                else
                {
                    if (callback2 == null)
                    {
                        callback2 = delegate (object o) {
                            VGen0 method = null;
                            DataAccess.ExecuteQuery("Tournament Update2", new object[] { this.tbName.Text, this.cbType.SelectedItem.ToString(), this.tbDescription.Text, this.tbLegal.Text, this.tbWebURL.Text, this.cbFaction.SelectedItem.ToString(), this.cbMap.SelectedItem.ToString(), this.GetRoundMinutes(), this.GetNumRounds(), this.GetMySQLDateTime(DateTime.Parse(this.cbDate.EditValue.ToString()).ToUniversalTime()), this.GetCustomFilter(), this.mTournamentID });
                            int mTournamentID = this.mTournamentID;
                            try
                            {
                                foreach (string str in this.tbAssistantTDs.Text.Split(new char[] { ',' }))
                                {
                                    DataAccess.ExecuteQuery("Tournament New TD", new object[] { mTournamentID, str.Trim() });
                                }
                                if (method == null)
                                {
                                    method = delegate {
                                        DlgMessage.ShowDialog(Loc.Get("<LOC>Tournament Data Saved."));
                                        base.Close();
                                    };
                                }
                                base.BeginInvoke(method);
                            }
                            catch
                            {
                                base.BeginInvoke(delegate {
                                    DlgMessage.ShowDialog(Loc.Get("<LOC>An error occured when trying to save the tournament."));
                                });
                            }
                        };
                    }
                    ThreadQueue.QueueUserWorkItem(callback2, new object[0]);
                }
            }
        }

        private void tTournamentTime_Click(object sender, EventArgs e)
        {
            this.SetDateTime();
        }

        private void tTournamentTime_EditValueChanged(object sender, EventArgs e)
        {
        }
    }
}

