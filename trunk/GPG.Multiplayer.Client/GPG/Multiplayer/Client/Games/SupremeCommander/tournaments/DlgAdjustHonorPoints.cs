namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgAdjustHonorPoints : DlgBase
    {
        private ComboBox comboBoxName;
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel2;
        private NumericUpDown numericUpDownPoints;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonOK;

        public DlgAdjustHonorPoints(string[] names)
        {
            this.InitializeComponent();
            if ((names != null) && (names.Length > 0))
            {
                foreach (string str in names)
                {
                    this.comboBoxName.Items.Add(str);
                }
                this.comboBoxName.SelectedIndex = 0;
            }
            else
            {
                base.Close();
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
            this.gpgLabel1 = new GPGLabel();
            this.numericUpDownPoints = new NumericUpDown();
            this.gpgLabel2 = new GPGLabel();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.comboBoxName = new ComboBox();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.numericUpDownPoints.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x1a1, 0x39);
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
            this.gpgLabel1.Location = new Point(13, 0x65);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x7d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 8;
            this.gpgLabel1.Text = "<LOC>Player Name";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.numericUpDownPoints.Location = new Point(0x10, 0xb6);
            int[] bits = new int[4];
            bits[0] = 10;
            this.numericUpDownPoints.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownPoints.Minimum = new decimal(bits);
            this.numericUpDownPoints.Name = "numericUpDownPoints";
            this.numericUpDownPoints.Size = new Size(60, 20);
            base.ttDefault.SetSuperTip(this.numericUpDownPoints, null);
            this.numericUpDownPoints.TabIndex = 9;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownPoints.Value = new decimal(bits);
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(13, 0xa3);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x16c, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 10;
            this.gpgLabel2.Text = "<LOC>Points to Remove (1 - 10, please use good judgement)";
            this.gpgLabel2.TextStyle = TextStyles.Default;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
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
            this.skinButtonCancel.Location = new Point(0x169, 0x11c);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x67, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 11;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
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
            this.skinButtonOK.Location = new Point(0xff, 0x11c);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x67, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 12;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.comboBoxName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxName.FormattingEnabled = true;
            this.comboBoxName.Location = new Point(0x10, 0x79);
            this.comboBoxName.Name = "comboBoxName";
            this.comboBoxName.Size = new Size(0xd5, 0x15);
            base.ttDefault.SetSuperTip(this.comboBoxName, null);
            this.comboBoxName.TabIndex = 13;
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x1dc, 0x175);
            base.Controls.Add(this.comboBoxName);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.gpgLabel2);
            base.Controls.Add(this.numericUpDownPoints);
            base.Controls.Add(this.gpgLabel1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x1dc, 0x175);
            this.MinimumSize = new Size(0x1dc, 0x175);
            base.Name = "DlgAdjustHonorPoints";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Remove Honor Points";
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.numericUpDownPoints, 0);
            base.Controls.SetChildIndex(this.gpgLabel2, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.comboBoxName, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.numericUpDownPoints.EndInit();
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
            try
            {
                string selectedItem = (string) this.comboBoxName.SelectedItem;
                if ((selectedItem == null) || (selectedItem.Length < 1))
                {
                    base.Error(this.comboBoxName, "You must provide a player name", new object[0]);
                }
                else
                {
                    int @int = new QuazalQuery("GetPlayerIDFromName", new object[] { selectedItem }).GetInt();
                    if (@int <= 0)
                    {
                        base.Error(this.comboBoxName, "Unable to locate {0}", new object[] { selectedItem });
                    }
                    else
                    {
                        int num2 = (int) this.numericUpDownPoints.Value;
                        if (new DlgYesNo(base.MainForm, "Confirm", string.Format(Loc.Get("<LOC>Are you sure you want to remove {0} honor points from {1}?"), num2, selectedItem)).ShowDialog() == DialogResult.Yes)
                        {
                            if (new QuazalQuery("RemoveHonorPoints", new object[] { num2, @int }).ExecuteNonQuery())
                            {
                                DlgMessage.ShowDialog("<LOC>Honor points successfully adjusted, informing player.");
                                Messaging.SendCustomCommand(selectedItem, CustomCommands.SystemMessage, new object[] { "<LOC>You have lost {0} tournament honor points.", num2 });
                                base.DialogResult = DialogResult.OK;
                                base.Close();
                            }
                            else
                            {
                                DlgMessage.ShowDialog("<LOC>An error occured adjusting this players honor points.");
                                base.DialogResult = DialogResult.Cancel;
                                base.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                DlgMessage.ShowDialog("<LOC>An error occured adjusting this players honor points.");
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        public string PlayerName
        {
            get
            {
                return (string) this.comboBoxName.SelectedItem;
            }
        }

        public int PointsRemoved
        {
            get
            {
                return (int) this.numericUpDownPoints.Value;
            }
        }
    }
}

