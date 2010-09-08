namespace GPG.Multiplayer.Client.Vaulting.Maps
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgMapDiagnostics : DlgBase
    {
        private IContainer components;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGTextArea gpgTextAreaValidate;
        private GPGTextArea gpgTextAreaVersion;
        private GPGTextArea gpgTextAreaXml;
        private ContentType mMapType;
        private string mSelectedMap;
        private SkinButton skinButtonDeleteDownload;
        private SkinButton skinButtonSelectMap;
        private SkinButton testAll;
        private SkinButton testDownload;
        private SkinButton testValidate;
        private SkinButton testVersion;
        private SkinButton testXml;

        public DlgMapDiagnostics()
        {
            this.components = null;
            this.InitializeComponent();
            this.SetStrings();
        }

        public DlgMapDiagnostics(string filePath)
        {
            this.components = null;
            this.InitializeComponent();
            this.SetStrings();
            try
            {
                if ((filePath != null) && (filePath.Length >= 1))
                {
                    if (!Path.HasExtension(filePath))
                    {
                        foreach (string str in Directory.GetFiles(filePath))
                        {
                            if (str.EndsWith("_scenario.lua"))
                            {
                                filePath = str;
                                break;
                            }
                        }
                    }
                    this.mSelectedMap = filePath;
                    this.Text = string.Format(Loc.Get("Diagnosing map: {0}"), Path.GetFileName(this.SelectedMap));
                    this.TestAll();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void ClearAll()
        {
            this.gpgTextAreaValidate.Text = "";
            this.gpgTextAreaVersion.Text = "";
            this.gpgTextAreaXml.Text = "";
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgMapDiagnostics));
            this.gpgTextAreaXml = new GPGTextArea();
            this.gpgLabel1 = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgTextAreaValidate = new GPGTextArea();
            this.gpgLabel3 = new GPGLabel();
            this.gpgTextAreaVersion = new GPGTextArea();
            this.testValidate = new SkinButton();
            this.testXml = new SkinButton();
            this.testVersion = new SkinButton();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.testAll = new SkinButton();
            this.testDownload = new SkinButton();
            this.skinButtonDeleteDownload = new SkinButton();
            this.gpgLabel7 = new GPGLabel();
            this.skinButtonSelectMap = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgTextAreaXml.Properties.BeginInit();
            this.gpgTextAreaValidate.Properties.BeginInit();
            this.gpgTextAreaVersion.Properties.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x2e6, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgTextAreaXml.BorderColor = Color.White;
            this.gpgTextAreaXml.Location = new Point(0x111, 0xf2);
            this.gpgTextAreaXml.Name = "gpgTextAreaXml";
            this.gpgTextAreaXml.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaXml.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaXml.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaXml.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaXml.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaXml.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaXml.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaXml.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaXml.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaXml.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaXml.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaXml.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaXml.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaXml.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaXml.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaXml.Size = new Size(0xff, 210);
            this.gpgTextAreaXml.TabIndex = 7;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(270, 220);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xa1, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 8;
            this.gpgLabel1.Text = "<LOC>Scenario file to Xml";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(13, 220);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(170, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 10;
            this.gpgLabel2.Text = "<LOC>Validate scenario file";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.gpgTextAreaValidate.BorderColor = Color.White;
            this.gpgTextAreaValidate.Location = new Point(12, 0xf2);
            this.gpgTextAreaValidate.Name = "gpgTextAreaValidate";
            this.gpgTextAreaValidate.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaValidate.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaValidate.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaValidate.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaValidate.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaValidate.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaValidate.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaValidate.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaValidate.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaValidate.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaValidate.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaValidate.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaValidate.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaValidate.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaValidate.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaValidate.Size = new Size(0xff, 210);
            this.gpgTextAreaValidate.TabIndex = 9;
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoSize = true;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x213, 220);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(0xa7, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel3, null);
            this.gpgLabel3.TabIndex = 12;
            this.gpgLabel3.Text = "<LOC>Version scenario file";
            this.gpgLabel3.TextStyle = TextStyles.Default;
            this.gpgTextAreaVersion.BorderColor = Color.White;
            this.gpgTextAreaVersion.Location = new Point(0x216, 0xf2);
            this.gpgTextAreaVersion.Name = "gpgTextAreaVersion";
            this.gpgTextAreaVersion.Properties.Appearance.BackColor = Color.Black;
            this.gpgTextAreaVersion.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            this.gpgTextAreaVersion.Properties.Appearance.ForeColor = Color.White;
            this.gpgTextAreaVersion.Properties.Appearance.Options.UseBackColor = true;
            this.gpgTextAreaVersion.Properties.Appearance.Options.UseBorderColor = true;
            this.gpgTextAreaVersion.Properties.Appearance.Options.UseForeColor = true;
            this.gpgTextAreaVersion.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            this.gpgTextAreaVersion.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            this.gpgTextAreaVersion.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gpgTextAreaVersion.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gpgTextAreaVersion.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.gpgTextAreaVersion.Properties.AppearanceFocused.Options.UseBorderColor = true;
            this.gpgTextAreaVersion.Properties.BorderStyle = BorderStyles.Simple;
            this.gpgTextAreaVersion.Properties.LookAndFeel.SkinName = "London Liquid Sky";
            this.gpgTextAreaVersion.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgTextAreaVersion.Size = new Size(0xff, 210);
            this.gpgTextAreaVersion.TabIndex = 11;
            this.testValidate.AutoStyle = true;
            this.testValidate.BackColor = Color.Transparent;
            this.testValidate.ButtonState = 0;
            this.testValidate.DialogResult = DialogResult.OK;
            this.testValidate.DisabledForecolor = Color.Gray;
            this.testValidate.DrawColor = Color.White;
            this.testValidate.DrawEdges = true;
            this.testValidate.FocusColor = Color.Yellow;
            this.testValidate.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.testValidate.ForeColor = Color.White;
            this.testValidate.HorizontalScalingMode = ScalingModes.Tile;
            this.testValidate.IsStyled = true;
            this.testValidate.Location = new Point(0x3f, 0x25b);
            this.testValidate.Name = "testValidate";
            this.testValidate.Size = new Size(0x7d, 0x1a);
            this.testValidate.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.testValidate, null);
            this.testValidate.TabIndex = 13;
            this.testValidate.TabStop = true;
            this.testValidate.Text = "<LOC>Run Test";
            this.testValidate.TextAlign = ContentAlignment.MiddleCenter;
            this.testValidate.TextPadding = new Padding(0);
            this.testValidate.Click += new EventHandler(this.testValidate_Click);
            this.testXml.AutoStyle = true;
            this.testXml.BackColor = Color.Transparent;
            this.testXml.ButtonState = 0;
            this.testXml.DialogResult = DialogResult.OK;
            this.testXml.DisabledForecolor = Color.Gray;
            this.testXml.DrawColor = Color.White;
            this.testXml.DrawEdges = true;
            this.testXml.FocusColor = Color.Yellow;
            this.testXml.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.testXml.ForeColor = Color.White;
            this.testXml.HorizontalScalingMode = ScalingModes.Tile;
            this.testXml.IsStyled = true;
            this.testXml.Location = new Point(0x14b, 0x25b);
            this.testXml.Name = "testXml";
            this.testXml.Size = new Size(0x7d, 0x1a);
            this.testXml.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.testXml, null);
            this.testXml.TabIndex = 14;
            this.testXml.TabStop = true;
            this.testXml.Text = "<LOC>Run Test";
            this.testXml.TextAlign = ContentAlignment.MiddleCenter;
            this.testXml.TextPadding = new Padding(0);
            this.testXml.Click += new EventHandler(this.testXml_Click);
            this.testVersion.AutoStyle = true;
            this.testVersion.BackColor = Color.Transparent;
            this.testVersion.ButtonState = 0;
            this.testVersion.DialogResult = DialogResult.OK;
            this.testVersion.DisabledForecolor = Color.Gray;
            this.testVersion.DrawColor = Color.White;
            this.testVersion.DrawEdges = true;
            this.testVersion.FocusColor = Color.Yellow;
            this.testVersion.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.testVersion.ForeColor = Color.White;
            this.testVersion.HorizontalScalingMode = ScalingModes.Tile;
            this.testVersion.IsStyled = true;
            this.testVersion.Location = new Point(0x259, 0x25b);
            this.testVersion.Name = "testVersion";
            this.testVersion.Size = new Size(0x7d, 0x1a);
            this.testVersion.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.testVersion, null);
            this.testVersion.TabIndex = 15;
            this.testVersion.TabStop = true;
            this.testVersion.Text = "<LOC>Run Test";
            this.testVersion.TextAlign = ContentAlignment.MiddleCenter;
            this.testVersion.TextPadding = new Padding(0);
            this.testVersion.Click += new EventHandler(this.testVersion_Click);
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.BackColor = Color.Transparent;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(9, 0x1ca);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0xeb, 0x8e);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 0x10;
            this.gpgLabel4.Text = manager.GetString("gpgLabel4.Text");
            this.gpgLabel4.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabel4.TextStyle = TextStyles.Default;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.Transparent;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(270, 0x1ca);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0xeb, 0x8e);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 0x11;
            this.gpgLabel5.Text = manager.GetString("gpgLabel5.Text");
            this.gpgLabel5.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabel5.TextStyle = TextStyles.Default;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.BackColor = Color.Transparent;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x213, 0x1ca);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0xeb, 0x8e);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 0x12;
            this.gpgLabel6.Text = manager.GetString("gpgLabel6.Text");
            this.gpgLabel6.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabel6.TextStyle = TextStyles.Default;
            this.testAll.AutoStyle = true;
            this.testAll.BackColor = Color.Transparent;
            this.testAll.ButtonState = 0;
            this.testAll.DialogResult = DialogResult.OK;
            this.testAll.DisabledForecolor = Color.Gray;
            this.testAll.DrawColor = Color.White;
            this.testAll.DrawEdges = true;
            this.testAll.FocusColor = Color.Yellow;
            this.testAll.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.testAll.ForeColor = Color.White;
            this.testAll.HorizontalScalingMode = ScalingModes.Tile;
            this.testAll.IsStyled = true;
            this.testAll.Location = new Point(0x247, 0x73);
            this.testAll.Name = "testAll";
            this.testAll.Size = new Size(0xc1, 0x1a);
            this.testAll.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.testAll, null);
            this.testAll.TabIndex = 0x13;
            this.testAll.TabStop = true;
            this.testAll.Text = "<LOC>Run all tests";
            this.testAll.TextAlign = ContentAlignment.MiddleCenter;
            this.testAll.TextPadding = new Padding(0);
            this.testAll.Click += new EventHandler(this.testAll_Click);
            this.testDownload.AutoStyle = true;
            this.testDownload.BackColor = Color.Transparent;
            this.testDownload.ButtonState = 0;
            this.testDownload.DialogResult = DialogResult.OK;
            this.testDownload.DisabledForecolor = Color.Gray;
            this.testDownload.DrawColor = Color.White;
            this.testDownload.DrawEdges = true;
            this.testDownload.FocusColor = Color.Yellow;
            this.testDownload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.testDownload.ForeColor = Color.White;
            this.testDownload.HorizontalScalingMode = ScalingModes.Tile;
            this.testDownload.IsStyled = true;
            this.testDownload.Location = new Point(0x247, 0x93);
            this.testDownload.Name = "testDownload";
            this.testDownload.Size = new Size(0xc1, 0x1a);
            this.testDownload.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.testDownload, null);
            this.testDownload.TabIndex = 20;
            this.testDownload.TabStop = true;
            this.testDownload.Text = "<LOC>Test Download";
            this.testDownload.TextAlign = ContentAlignment.MiddleCenter;
            this.testDownload.TextPadding = new Padding(0);
            this.testDownload.Click += new EventHandler(this.testDownload_Click);
            this.skinButtonDeleteDownload.AutoStyle = true;
            this.skinButtonDeleteDownload.BackColor = Color.Transparent;
            this.skinButtonDeleteDownload.ButtonState = 0;
            this.skinButtonDeleteDownload.DialogResult = DialogResult.OK;
            this.skinButtonDeleteDownload.DisabledForecolor = Color.Gray;
            this.skinButtonDeleteDownload.DrawColor = Color.White;
            this.skinButtonDeleteDownload.DrawEdges = true;
            this.skinButtonDeleteDownload.FocusColor = Color.Yellow;
            this.skinButtonDeleteDownload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDeleteDownload.ForeColor = Color.White;
            this.skinButtonDeleteDownload.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDeleteDownload.IsStyled = true;
            this.skinButtonDeleteDownload.Location = new Point(0x247, 0xb3);
            this.skinButtonDeleteDownload.Name = "skinButtonDeleteDownload";
            this.skinButtonDeleteDownload.Size = new Size(0xc1, 0x1a);
            this.skinButtonDeleteDownload.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonDeleteDownload, null);
            this.skinButtonDeleteDownload.TabIndex = 0x15;
            this.skinButtonDeleteDownload.TabStop = true;
            this.skinButtonDeleteDownload.Text = "<LOC>Delete Download Test";
            this.skinButtonDeleteDownload.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDeleteDownload.TextPadding = new Padding(0);
            this.skinButtonDeleteDownload.Click += new EventHandler(this.skinButtonDeleteDownload_Click);
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.Transparent;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(12, 0x53);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x235, 0x89);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 0x16;
            this.gpgLabel7.Text = manager.GetString("gpgLabel7.Text");
            this.gpgLabel7.TextStyle = TextStyles.Default;
            this.skinButtonSelectMap.AutoStyle = true;
            this.skinButtonSelectMap.BackColor = Color.Transparent;
            this.skinButtonSelectMap.ButtonState = 0;
            this.skinButtonSelectMap.DialogResult = DialogResult.OK;
            this.skinButtonSelectMap.DisabledForecolor = Color.Gray;
            this.skinButtonSelectMap.DrawColor = Color.White;
            this.skinButtonSelectMap.DrawEdges = true;
            this.skinButtonSelectMap.FocusColor = Color.Yellow;
            this.skinButtonSelectMap.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSelectMap.ForeColor = Color.White;
            this.skinButtonSelectMap.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSelectMap.IsStyled = true;
            this.skinButtonSelectMap.Location = new Point(0x247, 0x53);
            this.skinButtonSelectMap.Name = "skinButtonSelectMap";
            this.skinButtonSelectMap.Size = new Size(0xc1, 0x1a);
            this.skinButtonSelectMap.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSelectMap, null);
            this.skinButtonSelectMap.TabIndex = 0x17;
            this.skinButtonSelectMap.TabStop = true;
            this.skinButtonSelectMap.Text = "<LOC>Select Map";
            this.skinButtonSelectMap.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSelectMap.TextPadding = new Padding(0);
            this.skinButtonSelectMap.Click += new EventHandler(this.skinButtonSelectMap_Click);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x321, 690);
            base.Controls.Add(this.skinButtonSelectMap);
            base.Controls.Add(this.gpgLabel7);
            base.Controls.Add(this.skinButtonDeleteDownload);
            base.Controls.Add(this.testDownload);
            base.Controls.Add(this.testAll);
            base.Controls.Add(this.gpgLabel6);
            base.Controls.Add(this.gpgLabel5);
            base.Controls.Add(this.gpgLabel4);
            base.Controls.Add(this.testVersion);
            base.Controls.Add(this.testXml);
            base.Controls.Add(this.testValidate);
            base.Controls.Add(this.gpgLabel3);
            base.Controls.Add(this.gpgTextAreaVersion);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.gpgTextAreaValidate);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgTextAreaXml);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x321, 690);
            this.MinimumSize = new Size(0x321, 690);
            base.Name = "DlgMapDiagnostics";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Map Upload Diagnostics Tool";
            base.Controls.SetChildIndex(this.gpgTextAreaXml, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.gpgTextAreaValidate, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.gpgTextAreaVersion, 0);
            base.Controls.SetChildIndex(this.gpgLabel3, 0);
            base.Controls.SetChildIndex(this.testValidate, 0);
            base.Controls.SetChildIndex(this.testXml, 0);
            base.Controls.SetChildIndex(this.testVersion, 0);
            base.Controls.SetChildIndex(this.gpgLabel4, 0);
            base.Controls.SetChildIndex(this.gpgLabel5, 0);
            base.Controls.SetChildIndex(this.gpgLabel6, 0);
            base.Controls.SetChildIndex(this.testAll, 0);
            base.Controls.SetChildIndex(this.testDownload, 0);
            base.Controls.SetChildIndex(this.skinButtonDeleteDownload, 0);
            base.Controls.SetChildIndex(this.gpgLabel7, 0);
            base.Controls.SetChildIndex(this.skinButtonSelectMap, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgTextAreaXml.Properties.EndInit();
            this.gpgTextAreaValidate.Properties.EndInit();
            this.gpgTextAreaVersion.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SetStrings()
        {
            this.gpgLabel4.Text = Loc.Get("<LOC>This step will generally validate the integrity of a scenario file, ensuring its syntax is correct, and it is not missing required fields. The output will show any errors that are found, or will notify you if everything is OK.");
            this.gpgLabel5.Text = Loc.Get("<LOC>This step translates the scenario file into Xml so it can be parsed into GPGnet. Certain characters or syntax can cause improper loading of XML. The output for this step will return valid Xml or any errors that are found.");
            this.gpgLabel6.Text = Loc.Get("<LOC>This step adds the required version information and corresponding directory structure to the scenario file and puts it into the state it will be downloaded in.  The output will show any errors that are found, or will show the transformed scenario file if everything is OK");
            this.gpgLabel7.Text = Loc.Get("<LOC>The Vault uploads map files in three main steps: it validates scenario files, transforms them to Xml and loads them into GPGnet, and then stamps them with additional information for use with the vault. The tests for these three steps are shown below. You can run all three and then test the download process with the buttons on the right. Be sure to delete any download tests when you are done.");
            Loc.LocObject(this);
        }

        private void skinButtonDeleteDownload_Click(object sender, EventArgs e)
        {
            try
            {
                IAdditionalContent content;
                this.Cursor = Cursors.WaitCursor;
                base.ClearErrors();
                if (!this.MapType.CreateInstance().FromLocalFile(this.SelectedMap, out content))
                {
                    base.Error(this.skinButtonDeleteDownload, "<LOC>Unable to load map, check output below for any signs of error.", new object[0]);
                }
                else
                {
                    CustomMap map = content as CustomMap;
                    map.Version = 0x270f;
                    string path = map.GetDownloadPath() + @"\";
                    if (Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.Delete(Path.GetDirectoryName(path), true);
                    }
                }
            }
            catch (Exception exception)
            {
                base.Error(this.testDownload, exception.Message, new object[0]);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void skinButtonSelectMap_Click(object sender, EventArgs e)
        {
            base.ClearErrors();
            this.ClearAll();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Scenario files(*_scenario.lua)|*_scenario.lua";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.mSelectedMap = dialog.FileName;
                this.Text = string.Format(Loc.Get("Diagnosing map: {0}"), Path.GetFileName(this.SelectedMap));
            }
        }

        private void TestAll()
        {
            this.TestValidate();
            this.TestXml();
            this.TestVersion();
        }

        private void testAll_Click(object sender, EventArgs e)
        {
            this.TestAll();
        }

        private void testDownload_Click(object sender, EventArgs e)
        {
            try
            {
                IAdditionalContent content;
                string dir;
                this.Cursor = Cursors.WaitCursor;
                base.ClearErrors();
                if (!this.MapType.CreateInstance().FromLocalFile(this.SelectedMap, out content))
                {
                    base.Error(this.testDownload, "<LOC>Unable to load map, check output below for any signs of error.", new object[0]);
                }
                else
                {
                    CustomMap map = content as CustomMap;
                    map.Version = 0x270f;
                    string sourceFileName = map.CreateUploadFile();
                    dir = map.GetDownloadPath();
                    string path = dir + @"\content.partial";
                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }
                    File.Move(sourceFileName, path);
                    map.SaveDownload(this);
                    base.ClearStatus();
                    ThreadPool.QueueUserWorkItem(delegate (object s) {
                        Process.Start(dir);
                    });
                }
            }
            catch (Exception exception)
            {
                base.Error(this.testDownload, exception.Message, new object[0]);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void TestValidate()
        {
            string str;
            this.gpgTextAreaValidate.Text = "";
            if (LuaUtil.VerifyScenario(this.SelectedMap, out str))
            {
                this.gpgTextAreaValidate.Text = Loc.Get("<LOC>Map validated successfully");
            }
            else
            {
                this.gpgTextAreaValidate.Text = str;
            }
        }

        private void testValidate_Click(object sender, EventArgs e)
        {
            this.TestValidate();
        }

        private void TestVersion()
        {
            this.gpgTextAreaVersion.Text = LuaUtil.VersionScenario(this.SelectedMap, 0x270f);
        }

        private void testVersion_Click(object sender, EventArgs e)
        {
            this.TestVersion();
        }

        private void TestXml()
        {
            this.gpgTextAreaXml.Text = LuaUtil.ScenarioToXml(this.SelectedMap);
        }

        private void testXml_Click(object sender, EventArgs e)
        {
            this.TestXml();
        }

        private ContentType MapType
        {
            get
            {
                if (this.mMapType == null)
                {
                    foreach (ContentType type in ContentType.All)
                    {
                        if (type.Name == "Maps")
                        {
                            this.mMapType = type;
                            break;
                        }
                    }
                }
                return this.mMapType;
            }
        }

        public string SelectedMap
        {
            get
            {
                return this.mSelectedMap;
            }
        }
    }
}

