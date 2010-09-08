namespace GPG.Multiplayer.Client
{
    using DevExpress.UserSkins;
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    [Serializable]
    public class StylePrefs : ProgramSettings
    {
        private static readonly Color[] AllowedBackColors = new Color[] { Color.Transparent, Color.FromArgb(0x33, 0x33, 0x33), Color.FromArgb(0x24, 0x23, 0x23), Color.FromArgb(0xcc, 0xcc, 0xff) };
        private int mDialogMenuLeft = 10;
        private int mDialogMenuTop = 0x22;
        private int mFormBorderCurve = 15;
        private Color mHighlightColor1 = Color.FromArgb(0xa5, 0xa5, 0xfe);
        private Color mHighlightColor2 = Color.Black;
        private Color mHighlightColor3 = Color.FromArgb(0xcc, 0xcc, 0xff);
        private Color mMasterBackColor = Color.Black;
        private Font mMasterFont = new Font("Verdana", 8f);
        private Color mMasterForeColor = Color.White;
        private Font mMenuFont = new Font("Verdana", 8f, FontStyle.Bold);
        private Color mMenuForeColor = Color.White;
        private int mMenuLeft = 30;
        private int mMenuTop = 0x2b;
        private int mNavInsetFromRight = 90;
        private int mNavTop = 4;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DialogMenuLeftChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DialogMenuTopChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FormBorderCurveChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler HighlightColor1Changed;

        [field: NonSerialized]
        public event PropertyChangedEventHandler HighlightColor2Changed;

        [field: NonSerialized]
        public event PropertyChangedEventHandler HighlightColor3Changed;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MasterBackColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MasterFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MasterForeColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MenuFontChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MenuForeColorChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MenuLeftChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler MenuTopChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler NavInsetFromRightChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler NavTopChanged;

        private void box_Paint(object sender, PaintEventArgs e)
        {
            GPGTextBox box = sender as GPGTextBox;
            Rectangle clientRectangle = box.ClientRectangle;
            clientRectangle.X++;
            clientRectangle.Y++;
            clientRectangle.Width -= 3;
            clientRectangle.Height -= 3;
            Pen pen = new Pen(this.HighlightColor1);
            e.Graphics.DrawRectangle(pen, clientRectangle);
            pen.Dispose();
        }

        public void CustomizeView(GridView gridView)
        {
            GPGDataGrid gridControl = gridView.GridControl as GPGDataGrid;
            try
            {
                string path = Application.StartupPath + @"\grids\" + gridView.Name + ".xml";
                if (File.Exists(path))
                {
                    gridView.RestoreLayoutFromXml(path);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            gridView.Layout += new EventHandler(this.gridView_Layout);
            if (!gridControl.CustomizeStyle)
            {
                gridView.Appearance.Reset();
                gridView.OptionsView.EnableAppearanceEvenRow = true;
                gridView.OptionsView.EnableAppearanceOddRow = true;
                gridView.OptionsView.RowAutoHeight = true;
                gridView.OptionsView.ShowVertLines = false;
                gridView.OptionsView.ShowHorzLines = false;
                gridView.OptionsView.ShowIndicator = false;
                gridView.OptionsCustomization.AllowRowSizing = false;
                gridView.PaintStyleName = "Web";
                gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
                gridView.FocusRectStyle = DrawFocusRectStyle.None;
                foreach (AppearanceObject obj2 in gridView.Appearance)
                {
                    this.SetAppearance(obj2, Program.Settings.Appearance.Text.MasterFont, Color.Black, Color.Black, Color.White);
                }
                this.SetAppearance(gridView.Appearance.HeaderPanel, new Font(this.MasterFont, FontStyle.Bold), Color.FromArgb(0xa4, 0xa4, 0xa4), Color.Black, Color.White);
                gridView.Appearance.HeaderPanel.GradientMode = LinearGradientMode.Vertical;
                this.SetAppearance(gridView.Appearance.GroupPanel, Color.Black, Color.Black, Color.White);
                this.SetAppearance(gridView.Appearance.ColumnFilterButton, Color.Black, Color.Black, Color.White);
                if (gridControl.LevelTree.HasChildren && (gridView.DetailLevel == 0))
                {
                    Font font = new Font(Program.Settings.Appearance.Text.MasterFont, FontStyle.Bold);
                    this.SetAppearance(gridView.Appearance.EvenRow, font, Color.FromArgb(0x51, 0x56, 0x87), Color.FromArgb(0x38, 0x3d, 0x67), Color.White);
                    gridView.Appearance.EvenRow.GradientMode = LinearGradientMode.Vertical;
                    this.SetAppearance(gridView.Appearance.OddRow, font, Color.FromArgb(0x51, 0x56, 0x87), Color.FromArgb(0x38, 0x3d, 0x67), Color.White);
                    gridView.Appearance.OddRow.GradientMode = LinearGradientMode.Vertical;
                    this.SetAppearance(gridView.Appearance.HideSelectionRow, font, Color.FromArgb(0x51, 0x56, 0x87), Color.FromArgb(0x38, 0x3d, 0x67), Color.White);
                    gridView.Appearance.HideSelectionRow.GradientMode = LinearGradientMode.Vertical;
                    this.SetAppearance(gridView.Appearance.SelectedRow, font, Color.FromArgb(0x51, 0x56, 0x87), Color.FromArgb(0x38, 0x3d, 0x67), Color.Black);
                    gridView.Appearance.SelectedRow.GradientMode = LinearGradientMode.Vertical;
                    this.SetAppearance(gridView.Appearance.FocusedRow, font, Color.FromArgb(0x51, 0x56, 0x87), Color.FromArgb(0x38, 0x3d, 0x67), Color.Black);
                    gridView.Appearance.FocusedRow.GradientMode = LinearGradientMode.Vertical;
                }
                else if (gridView.DetailLevel >= 1)
                {
                    this.SetAppearance(gridView.Appearance.SelectedRow, Color.FromArgb(0x40, 0x40, 0x40), Color.Black, Color.White);
                    this.SetAppearance(gridView.Appearance.FocusedRow, Color.FromArgb(0x40, 0x40, 0x40), Color.Black, Color.White);
                }
                else
                {
                    this.SetAppearance(gridView.Appearance.EvenRow, Color.FromArgb(0x40, 0x40, 0x40), Color.Black, Color.White);
                    this.SetAppearance(gridView.Appearance.OddRow, Color.Black, Color.Black, Color.White);
                    this.SetAppearance(gridView.Appearance.Row, Color.Black, Color.Black, Color.White);
                    this.SetAppearance(gridView.Appearance.HideSelectionRow, Color.FromArgb(0xc0, 0xc0, 0xff), Color.FromArgb(0x40, 0x40, 80), Color.White);
                    this.SetAppearance(gridView.Appearance.SelectedRow, Color.FromArgb(0xc0, 0xc0, 0xff), Color.FromArgb(0x40, 0x40, 80), Color.Black);
                    this.SetAppearance(gridView.Appearance.FocusedRow, Color.FromArgb(0xc0, 0xc0, 0xff), Color.FromArgb(0x40, 0x40, 80), Color.Black);
                    this.SetAppearance(gridView.Appearance.Preview, Color.Black, Color.Black, Color.Gray);
                }
                gridView.Appearance.Empty.BackColor = Color.Transparent;
            }
        }

        private void gridView_Layout(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + @"\grids\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\grids\");
                }
                GridView view = sender as GridView;
                if (view.Name == "gvChat")
                {
                    view.SaveLayoutToXml(Application.StartupPath + @"\grids\" + view.Name + ".xml");
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void Register()
        {
            BonusSkins.Register();
            GPGButton.OnStyleControl += new EventHandler(this.StyleControl);
            GPGDataGrid.OnStyleControl += new EventHandler(this.StyleControl);
            GPGTextArea.OnStyleControl += new EventHandler(this.StyleControl);
            GPGTextBox.OnStyleControl += new EventHandler(this.StyleControl);
            GPGLabel.OnStyleControl += new EventHandler(this.StyleControl);
            GPGRichLabel.OnStyleControl += new EventHandler(this.StyleControl);
            GPGStatRow.OnStyleControl += new EventHandler(this.StyleControl);
            GPGComboBox.OnStyleControl += new EventHandler(this.StyleControl);
            GPGContextMenu.OnStyleControl += new EventHandler(this.StyleControl);
            GPGTabControl.OnStyleControl += new EventHandler(this.StyleControl);
            GPGChatGrid.OnStyleControl += new EventHandler(this.StyleControl);
            GPGMenuStrip.OnStyleControl += new EventHandler(this.StyleControl);
            GPGDropDownList.OnStyleControl += new EventHandler(this.StyleControl);
            FrmBase.OnStyleControl += new EventHandler(this.StyleControl);
        }

        private void SetAppearance(AppearanceObject appearance, Color bgColor1, Color bgColor2, Color foreColor)
        {
            this.SetAppearance(appearance, this.MasterFont, bgColor1, bgColor2, foreColor);
        }

        private void SetAppearance(AppearanceObject appearance, Font font, Color bgColor1, Color bgColor2, Color foreColor)
        {
            appearance.Font = font;
            appearance.BackColor = bgColor1;
            appearance.BackColor2 = bgColor2;
            appearance.ForeColor = foreColor;
        }

        private void StyleAppearance(object obj)
        {
            foreach (PropertyInfo info in obj.GetType().GetProperties())
            {
                if (info.PropertyType == typeof(AppearanceObject))
                {
                    AppearanceObject obj2 = info.GetValue(obj, null) as AppearanceObject;
                    obj2.Font = this.MasterFont;
                }
            }
        }

        public void StyleChildControl(Control control)
        {
            if ((!(control is RichTextBox) && !(control is GPGLabel)) && !(control is GPGScrollLabel))
            {
                if (!(control is IStyledControl) || (control as IStyledControl).AutoStyle)
                {
                    control.Font = Program.Settings.Appearance.Text.MasterFont;
                    control.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                    if (this.MasterBackColor == Color.Transparent)
                    {
                        this.MasterBackColor = Color.Black;
                    }
                    if (Array.IndexOf<Color>(AllowedBackColors, control.BackColor) < 0)
                    {
                        control.BackColor = this.MasterBackColor;
                    }
                }
                foreach (Control control2 in control.Controls)
                {
                    this.StyleChildControl(control2);
                }
            }
        }

        public void StyleControl(Control control)
        {
            this.StyleControl(control, EventArgs.Empty);
        }

        public void StyleControl(object sender, EventArgs e)
        {
            if (!(sender is IStyledControl) || (sender as IStyledControl).AutoStyle)
            {
                if (!((!(sender is Control) || (sender is GPGDataGrid)) || (sender is GridView)))
                {
                    Control control = sender as Control;
                    this.StyleChildControl(control);
                }
                if (sender is GPGLabel)
                {
                    if (!(sender as GPGLabel).IsStyled)
                    {
                        (sender as GPGLabel).TextStyleChanged += new PropertyChangedEventHandler(this.StyleControl);
                    }
                    GPGLabel label = sender as GPGLabel;
                    if (Array.IndexOf<Color>(AllowedBackColors, label.BackColor) < 0)
                    {
                        label.BackColor = Color.Transparent;
                    }
                    if (label.AutoStyle)
                    {
                        switch (label.TextStyle)
                        {
                            case TextStyles.Default:
                                label.Font = Program.Settings.Appearance.Text.MasterFont;
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Custom:
                                return;

                            case TextStyles.Colored:
                                label.Font = Program.Settings.Appearance.Text.MasterFont;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.ColoredBold:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont, FontStyle.Bold);
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Bold:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont, FontStyle.Bold);
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Italic:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont, FontStyle.Italic);
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Small:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont.FontFamily, Program.Settings.Appearance.Text.MasterFont.Size - 2f);
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Large:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont.FontFamily, Program.Settings.Appearance.Text.MasterFont.Size + 2f);
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Info:
                                label.Font = Program.Settings.Appearance.Text.MasterFont;
                                label.ForeColor = Program.Settings.Appearance.Text.InfoColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Header1:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont.FontFamily, 10f, FontStyle.Bold);
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Header2:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont.FontFamily, 12f, FontStyle.Bold);
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Header3:
                                label.Font = new Font(Program.Settings.Appearance.Text.MasterFont.FontFamily, 14f, FontStyle.Bold);
                                label.ForeColor = Program.Settings.Appearance.Text.MasterColor;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Title:
                                label.ForeColor = Program.Settings.Appearance.Text.TitleColor;
                                label.Font = Program.Settings.Appearance.Text.TitleFont;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Link:
                                label.ForeColor = Program.Settings.Appearance.Text.LinkColor;
                                label.Font = Program.Settings.Appearance.Text.LinkFont;
                                label.Cursor = Cursors.Hand;
                                return;

                            case TextStyles.Error:
                                label.ForeColor = Program.Settings.Appearance.Text.ErrorColor;
                                label.Font = Program.Settings.Appearance.Text.ErrorFont;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Status:
                                label.ForeColor = Program.Settings.Appearance.Text.StatusColor;
                                label.Font = Program.Settings.Appearance.Text.StatusFont;
                                label.Cursor = Cursors.Default;
                                return;

                            case TextStyles.Descriptor:
                                label.ForeColor = Program.Settings.Appearance.Text.DescriptorColor;
                                label.Font = Program.Settings.Appearance.Text.DescriptorFont;
                                label.Cursor = Cursors.Default;
                                return;
                        }
                    }
                }
                else if (sender is GPGButton)
                {
                    GPGButton button = sender as GPGButton;
                    button.LookAndFeel.UseDefaultLookAndFeel = false;
                    button.LookAndFeel.SkinName = "London Liquid Sky";
                    button.ForeColor = Color.Black;
                }
                else if (sender is GPGDataGrid)
                {
                    GPGDataGrid grid = sender as GPGDataGrid;
                    if (!grid.CustomizeStyle)
                    {
                        grid.LookAndFeel.Reset();
                        grid.LookAndFeel.ResetParentLookAndFeel();
                    }
                }
                else if (sender is BaseView)
                {
                    BaseView view = sender as BaseView;
                    this.StyleAppearance(view.Appearance);
                    if (view is GridView)
                    {
                        GridView gridView = sender as GridView;
                        this.CustomizeView(gridView);
                    }
                }
                else if (sender is GPGTextArea)
                {
                    GPGTextArea area = sender as GPGTextArea;
                    area.LookAndFeel.UseDefaultLookAndFeel = false;
                    area.LookAndFeel.SkinName = "London Liquid Sky";
                    area.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x20, 0x20, 0x20);
                    area.Properties.AppearanceFocused.ForeColor = this.MasterForeColor;
                    area.Properties.AppearanceFocused.BorderColor = this.HighlightColor3;
                    area.BorderColor = this.HighlightColor3;
                }
                else if (sender is GPGComboBox)
                {
                    GPGComboBox box = sender as GPGComboBox;
                    box.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x20, 0x20, 0x20);
                    box.Properties.AppearanceFocused.ForeColor = this.MasterForeColor;
                    box.Properties.AppearanceFocused.BorderColor = this.HighlightColor3;
                    box.BorderColor = this.HighlightColor3;
                }
                else if (sender is GPGDropDownList)
                {
                    GPGDropDownList list = sender as GPGDropDownList;
                    list.FocusBackColor = Color.FromArgb(0x20, 0x20, 0x20);
                    list.ForeColor = this.MasterForeColor;
                    list.FocusBorderColor = this.HighlightColor3;
                    list.BorderColor = this.HighlightColor3;
                }
                else if (sender is GPGTextBox)
                {
                    GPGTextBox box2 = sender as GPGTextBox;
                    box2.LookAndFeel.UseDefaultLookAndFeel = false;
                    box2.LookAndFeel.SkinName = "London Liquid Sky";
                    this.StyleAppearance(box2.Properties);
                    box2.BorderStyle = BorderStyles.Simple;
                    box2.Paint += new PaintEventHandler(this.box_Paint);
                    box2.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x20, 0x20, 0x20);
                    box2.Properties.AppearanceFocused.ForeColor = this.MasterForeColor;
                    box2.Properties.AppearanceFocused.BorderColor = this.HighlightColor3;
                    box2.Properties.Appearance.BorderColor = this.HighlightColor1;
                }
                else if (sender is GPGContextMenu)
                {
                    GPGContextMenu menu = sender as GPGContextMenu;
                }
                else if (sender is GPGTabControl)
                {
                    GPGTabControl control2 = sender as GPGTabControl;
                    control2.LookAndFeel.UseDefaultLookAndFeel = false;
                    control2.LookAndFeel.SkinName = "London Liquid Sky";
                    this.StyleAppearance(control2);
                    this.StyleAppearance(control2.AppearancePage);
                    control2.AppearancePage.Header.BackColor = this.HighlightColor1;
                    control2.AppearancePage.Header.BackColor2 = this.HighlightColor2;
                    control2.AppearancePage.HeaderActive.BackColor = this.HighlightColor3;
                    control2.AppearancePage.HeaderActive.BackColor2 = this.HighlightColor1;
                }
                else if (sender is GPGMenuStrip)
                {
                    GPGMenuStrip strip = sender as GPGMenuStrip;
                    strip.ForeColor = this.MenuForeColor;
                    strip.Font = this.MenuFont;
                }
                else if (sender is FrmBase)
                {
                    FrmBase base2 = sender as FrmBase;
                }
            }
        }

        [Category("<LOC>Layout"), Description("<LOC>"), DisplayName("<LOC>Dialog Menu Left")]
        public int DialogMenuLeft
        {
            get
            {
                return this.mDialogMenuLeft;
            }
            set
            {
                this.mDialogMenuLeft = value;
                if (this.DialogMenuLeftChanged != null)
                {
                    this.DialogMenuLeftChanged(this, new PropertyChangedEventArgs("DialogMenuLeft"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Layout"), DisplayName("<LOC>Dialog Menu Top")]
        public int DialogMenuTop
        {
            get
            {
                return this.mDialogMenuTop;
            }
            set
            {
                this.mDialogMenuTop = value;
                if (this.DialogMenuTopChanged != null)
                {
                    this.DialogMenuTopChanged(this, new PropertyChangedEventArgs("DialogMenuTop"));
                }
            }
        }

        [Category("<LOC>Layout"), DisplayName("<LOC>Form Border Curve"), Description("<LOC>")]
        public int FormBorderCurve
        {
            get
            {
                return this.mFormBorderCurve;
            }
            set
            {
                this.mFormBorderCurve = value;
                if (this.FormBorderCurveChanged != null)
                {
                    this.FormBorderCurveChanged(this, new PropertyChangedEventArgs("FormBorderCurve"));
                }
            }
        }

        [DisplayName("<LOC>Highlight color 1"), Description("<LOC>"), Category("<LOC>Colors")]
        public Color HighlightColor1
        {
            get
            {
                return this.mHighlightColor1;
            }
            set
            {
                if (value != Color.Transparent)
                {
                    this.mHighlightColor1 = value;
                    if (this.HighlightColor1Changed != null)
                    {
                        this.HighlightColor1Changed(this, new PropertyChangedEventArgs("HighlightColor1"));
                    }
                }
            }
        }

        [Description("<LOC>"), DisplayName("<LOC>Highlight color 2"), Category("<LOC>Colors")]
        public Color HighlightColor2
        {
            get
            {
                return this.mHighlightColor2;
            }
            set
            {
                if (value != Color.Transparent)
                {
                    this.mHighlightColor2 = value;
                    if (this.HighlightColor2Changed != null)
                    {
                        this.HighlightColor2Changed(this, new PropertyChangedEventArgs("HighlightColor2"));
                    }
                }
            }
        }

        [DisplayName("<LOC>Highlight color 3"), Description("<LOC>"), Category("<LOC>Colors")]
        public Color HighlightColor3
        {
            get
            {
                return this.mHighlightColor3;
            }
            set
            {
                if (value != Color.Transparent)
                {
                    this.mHighlightColor3 = value;
                    if (this.HighlightColor3Changed != null)
                    {
                        this.HighlightColor3Changed(this, new PropertyChangedEventArgs("HighlightColor3"));
                    }
                }
            }
        }

        [DisplayName("<LOC>Background Color"), Category("<LOC>Colors"), Description("<LOC>The background color of the application")]
        public Color MasterBackColor
        {
            get
            {
                return this.mMasterBackColor;
            }
            set
            {
                if (value != Color.Transparent)
                {
                    this.mMasterBackColor = value;
                    if (this.MasterBackColorChanged != null)
                    {
                        this.MasterBackColorChanged(this, new PropertyChangedEventArgs("MasterBackColor"));
                    }
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Fonts"), DisplayName("<LOC>Master Font")]
        public Font MasterFont
        {
            get
            {
                return this.mMasterFont;
            }
            set
            {
                if (value != null)
                {
                    float emSize = 16f;
                    if (value.SizeInPoints > emSize)
                    {
                        value = new Font(value.FontFamily, emSize, value.Style, GraphicsUnit.Point);
                    }
                    this.mMasterFont = value;
                    if (this.MasterFontChanged != null)
                    {
                        this.MasterFontChanged(this, new PropertyChangedEventArgs("MasterFont"));
                    }
                }
            }
        }

        [Description("<LOC>The foreground color of the application"), DisplayName("<LOC>Foreground Color"), Category("<LOC>Colors")]
        public Color MasterForeColor
        {
            get
            {
                return this.mMasterForeColor;
            }
            set
            {
                this.mMasterForeColor = value;
                if (this.MasterForeColorChanged != null)
                {
                    this.MasterForeColorChanged(this, new PropertyChangedEventArgs("MasterForeColor"));
                }
            }
        }

        [Category("<LOC>Fonts"), Description("<LOC>"), DisplayName("<LOC>Menu Font")]
        public Font MenuFont
        {
            get
            {
                if (this.mMenuFont == null)
                {
                    return this.MasterFont;
                }
                return this.mMenuFont;
            }
            set
            {
                if (value != null)
                {
                    this.mMenuFont = value;
                    if (this.MenuFontChanged != null)
                    {
                        this.MenuFontChanged(this, new PropertyChangedEventArgs("MenuFont"));
                    }
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Colors"), DisplayName("<LOC>Menu Forecolor")]
        public Color MenuForeColor
        {
            get
            {
                return this.mMenuForeColor;
            }
            set
            {
                this.mMenuForeColor = value;
                if (this.MenuForeColorChanged != null)
                {
                    this.MenuForeColorChanged(this, new PropertyChangedEventArgs("MenuForeColor"));
                }
            }
        }

        [Category("<LOC>Layout"), DisplayName("<LOC>Menu Left"), Description("<LOC>")]
        public int MenuLeft
        {
            get
            {
                return this.mMenuLeft;
            }
            set
            {
                this.mMenuLeft = value;
                if (this.MenuLeftChanged != null)
                {
                    this.MenuLeftChanged(this, new PropertyChangedEventArgs("MenuLeft"));
                }
            }
        }

        [Category("<LOC>Layout"), Description("<LOC>"), DisplayName("<LOC>Menu Top")]
        public int MenuTop
        {
            get
            {
                return this.mMenuTop;
            }
            set
            {
                this.mMenuTop = value;
                if (this.MenuTopChanged != null)
                {
                    this.MenuTopChanged(this, new PropertyChangedEventArgs("MenuTop"));
                }
            }
        }

        [Description("<LOC>"), Category("<LOC>Layout"), DisplayName("<LOC>Nav Inset From Right")]
        public int NavInsetFromRight
        {
            get
            {
                return this.mNavInsetFromRight;
            }
            set
            {
                this.mNavInsetFromRight = value;
                if (this.NavInsetFromRightChanged != null)
                {
                    this.NavInsetFromRightChanged(this, new PropertyChangedEventArgs("NavInsetFromRight"));
                }
            }
        }

        [Category("<LOC>Layout"), Description("<LOC>"), DisplayName("<LOC>Nav Top")]
        public int NavTop
        {
            get
            {
                return this.mNavTop;
            }
            set
            {
                this.mNavTop = value;
                if (this.NavTopChanged != null)
                {
                    this.NavTopChanged(this, new PropertyChangedEventArgs("NavTop"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>"), DisplayName("<LOC>Skin Name")]
        public string SkinName
        {
            get
            {
                return Program.Settings.SkinName;
            }
            set
            {
                Program.Settings.SkinName = value;
            }
        }
    }
}

