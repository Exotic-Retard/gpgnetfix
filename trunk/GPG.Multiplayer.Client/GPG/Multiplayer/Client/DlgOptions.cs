namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class DlgOptions : DlgBase
    {
        private IContainer components;
        private GPGPanel gpgPanel1;
        private GPGOptionsEditor OptionsEditor;
        private bool RestoredDefaults;
        private SkinButton skinButtonApply;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonDefaults;
        private SkinButton skinButtonOK;

        public DlgOptions() : this(Program.MainForm)
        {
        }

        public DlgOptions(FrmMain mainForm) : base(mainForm)
        {
            EventHandler handler = null;
            this.RestoredDefaults = false;
            this.components = null;
            this.InitializeComponent();
            base.Size = base.Size;
            this.OptionsEditor.BorderColor = Program.Settings.StylePreferences.HighlightColor1;
            this.OptionsEditor.Settings = Program.Settings;
            if (handler == null)
            {
                handler = delegate (object s, EventArgs e) {
                    this.skinButtonApply.Enabled = true;
                };
            }
            this.OptionsEditor.PropertyChanged += handler;
            this.skinButtonApply.Refresh();
        }

        private static void _OverwriteSettings(object sourceRoot, object destRoot)
        {
            try
            {
                foreach (PropertyInfo info in sourceRoot.GetType().GetProperties())
                {
                    if (info.GetCustomAttributes(typeof(OptionsRootAttribute), false).Length > 0)
                    {
                        _OverwriteSettings(info.GetValue(sourceRoot, null), info.GetValue(destRoot, null));
                    }
                    else if (info.CanWrite && (info.GetSetMethod() != null))
                    {
                        object obj2 = info.GetValue(sourceRoot, null);
                        object obj3 = info.GetValue(destRoot, null);
                        if (((obj2 != null) && !obj2.Equals(obj3)) && (obj2 != obj3))
                        {
                            destRoot.GetType().GetProperty(info.Name).SetValue(destRoot, obj2, null);
                        }
                    }
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
            this.OptionsEditor = new GPGOptionsEditor();
            this.skinButtonOK = new SkinButton();
            this.skinButtonApply = new SkinButton();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonDefaults = new SkinButton();
            this.gpgPanel1 = new GPGPanel();
            this.gpgPanel1.SuspendLayout();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.OptionsEditor.BackColor = Color.Black;
            this.OptionsEditor.BorderColor = Color.White;
            this.OptionsEditor.Dock = DockStyle.Fill;
            this.OptionsEditor.Location = new Point(0, 0);
            this.OptionsEditor.Margin = new Padding(3, 4, 3, 4);
            this.OptionsEditor.Name = "OptionsEditor";
            this.OptionsEditor.Padding = new Padding(2);
            this.OptionsEditor.Settings = null;
            this.OptionsEditor.Size = new Size(620, 0x167);
            this.OptionsEditor.TabIndex = 4;
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
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
            this.skinButtonOK.Location = new Point(0x146, 0x1ce);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x5f, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonOK.TabIndex = 7;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonApply.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonApply.AutoStyle = true;
            this.skinButtonApply.BackColor = Color.Black;
            this.skinButtonApply.DialogResult = DialogResult.OK;
            this.skinButtonApply.DisabledForecolor = Color.Gray;
            this.skinButtonApply.DrawEdges = true;
            this.skinButtonApply.Enabled = false;
            this.skinButtonApply.FocusColor = Color.Yellow;
            this.skinButtonApply.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonApply.ForeColor = Color.White;
            this.skinButtonApply.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonApply.IsStyled = true;
            this.skinButtonApply.Location = new Point(0x1ab, 0x1ce);
            this.skinButtonApply.Name = "skinButtonApply";
            this.skinButtonApply.Size = new Size(0x5f, 0x1a);
            this.skinButtonApply.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonApply.TabIndex = 8;
            this.skinButtonApply.Text = "<LOC>Apply";
            this.skinButtonApply.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonApply.TextPadding = new Padding(0);
            this.skinButtonApply.Click += new EventHandler(this.skinButtonApply_Click);
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
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
            this.skinButtonCancel.Location = new Point(0x210, 0x1ce);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x5f, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonCancel.TabIndex = 9;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonDefaults.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonDefaults.AutoStyle = true;
            this.skinButtonDefaults.BackColor = Color.Black;
            this.skinButtonDefaults.DialogResult = DialogResult.OK;
            this.skinButtonDefaults.DisabledForecolor = Color.Gray;
            this.skinButtonDefaults.DrawEdges = true;
            this.skinButtonDefaults.FocusColor = Color.Yellow;
            this.skinButtonDefaults.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDefaults.ForeColor = Color.White;
            this.skinButtonDefaults.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDefaults.IsStyled = true;
            this.skinButtonDefaults.Location = new Point(0x18, 0x1ce);
            this.skinButtonDefaults.Name = "skinButtonDefaults";
            this.skinButtonDefaults.Size = new Size(0x77, 0x1a);
            this.skinButtonDefaults.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonDefaults.TabIndex = 10;
            this.skinButtonDefaults.Text = "<LOC>Restore Defaults";
            this.skinButtonDefaults.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDefaults.TextPadding = new Padding(0);
            this.skinButtonDefaults.Click += new EventHandler(this.skinButtonDefaults_Click);
            this.gpgPanel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanel1.Controls.Add(this.OptionsEditor);
            this.gpgPanel1.Location = new Point(12, 0x53);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(620, 0x167);
            this.gpgPanel1.TabIndex = 11;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.Black;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x284, 0x227);
            base.Controls.Add(this.gpgPanel1);
            base.Controls.Add(this.skinButtonDefaults);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.skinButtonApply);
            base.Controls.Add(this.skinButtonOK);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x1d4, 0x177);
            base.Name = "DlgOptions";
            this.Text = "<LOC>Options";
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonApply, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonDefaults, 0);
            base.Controls.SetChildIndex(this.gpgPanel1, 0);
            this.gpgPanel1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static void OverwriteSettings(ProgramSettings source, ProgramSettings destination)
        {
            if (source.GetType() != destination.GetType())
            {
                ErrorLog.WriteLine("Types do not match between settings objects; source: {0}, destination: {1}", new object[] { source.GetType(), destination.GetType() });
                DlgMessage.ShowDialog(Loc.Get("<LOC>An error occured while saving settings, some changes may not take effect and it is recommended you restart the application."));
            }
            _OverwriteSettings(source, destination);
        }

        private void skinButtonApply_Click(object sender, EventArgs e)
        {
            OverwriteSettings(this.OptionsEditor.Settings, Program.Settings);
            Program.Settings.Save();
            if (this.RestoredDefaults && (new DlgYesNo(base.MainForm, Loc.Get("<LOC>Restart required"), Loc.Get("<LOC>The application must be restarted for the changes to take effect, do you want to restart now?")).ShowDialog() == DialogResult.Yes))
            {
                Application.Restart();
            }
            base.MainForm.RefreshChat();
            base.MainForm.CheckGameOptions();
            this.skinButtonApply.Enabled = false;
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonDefaults_Click(object sender, EventArgs e)
        {
            try
            {
                OverwriteSettings(new UserPrefs(), this.OptionsEditor.Settings);
                this.OptionsEditor.PropertyGrid.Rows.Clear();
                this.OptionsEditor.PropertyGrid.SelectedObject = null;
                this.OptionsEditor.PropertyGrid.SelectedObject = this.OptionsEditor.TreeView.SelectedNode.Tag;
                this.OptionsEditor.Localize();
                this.skinButtonApply.Enabled = true;
                this.RestoredDefaults = true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            OverwriteSettings(this.OptionsEditor.Settings, Program.Settings);
            Program.Settings.Save();
            if (this.RestoredDefaults && (new DlgYesNo(base.MainForm, Loc.Get("<LOC>Restart required"), Loc.Get("<LOC>The application must be restarted for the changes to take effect, do you want to restart now?")).ShowDialog() == DialogResult.Yes))
            {
                Application.Restart();
            }
            base.MainForm.RefreshChat();
            base.DialogResult = DialogResult.OK;
            base.Close();
        }
    }
}

