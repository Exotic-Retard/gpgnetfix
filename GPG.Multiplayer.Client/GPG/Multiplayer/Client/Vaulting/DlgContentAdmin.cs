namespace GPG.Multiplayer.Client.Vaulting
{
    using DevExpress.Utils;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Security;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault;
    using GPG.Multiplayer.Quazal.Security;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgContentAdmin : DlgBase
    {
        private IContainer components = null;
        private IAdditionalContent mContent;
        private SkinButton skinButtonContentDownACL;
        private SkinButton skinButtonDelete;
        private SkinButton skinButtonDisable;
        private SkinButton skinButtonTypeDownACL;
        private SkinButton skinButtonTypeUpACL;
        private SkinLabel skinLabel2;
        private static Service WebService = new Service();

        public DlgContentAdmin(IAdditionalContent content)
        {
            this.InitializeComponent();
            this.mContent = content;
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
            this.skinButtonDelete = new SkinButton();
            this.skinButtonTypeDownACL = new SkinButton();
            this.skinButtonDisable = new SkinButton();
            this.skinButtonContentDownACL = new SkinButton();
            this.skinButtonTypeUpACL = new SkinButton();
            this.skinLabel2 = new SkinLabel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x184, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.skinButtonDelete.AutoStyle = true;
            this.skinButtonDelete.BackColor = Color.Transparent;
            this.skinButtonDelete.ButtonState = 0;
            this.skinButtonDelete.DialogResult = DialogResult.OK;
            this.skinButtonDelete.DisabledForecolor = Color.Gray;
            this.skinButtonDelete.DrawColor = Color.White;
            this.skinButtonDelete.DrawEdges = true;
            this.skinButtonDelete.FocusColor = Color.Yellow;
            this.skinButtonDelete.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDelete.ForeColor = Color.White;
            this.skinButtonDelete.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDelete.IsStyled = true;
            this.skinButtonDelete.Location = new Point(12, 220);
            this.skinButtonDelete.Name = "skinButtonDelete";
            this.skinButtonDelete.Size = new Size(0xb6, 0x1a);
            this.skinButtonDelete.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonDelete, null);
            this.skinButtonDelete.TabIndex = 7;
            this.skinButtonDelete.TabStop = true;
            this.skinButtonDelete.Text = "Delete this content";
            this.skinButtonDelete.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDelete.TextPadding = new Padding(0);
            this.skinButtonDelete.Click += new EventHandler(this.skinButtonDelete_Click);
            this.skinButtonTypeDownACL.AutoStyle = true;
            this.skinButtonTypeDownACL.BackColor = Color.Transparent;
            this.skinButtonTypeDownACL.ButtonState = 0;
            this.skinButtonTypeDownACL.DialogResult = DialogResult.OK;
            this.skinButtonTypeDownACL.DisabledForecolor = Color.Gray;
            this.skinButtonTypeDownACL.DrawColor = Color.White;
            this.skinButtonTypeDownACL.DrawEdges = true;
            this.skinButtonTypeDownACL.FocusColor = Color.Yellow;
            this.skinButtonTypeDownACL.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonTypeDownACL.ForeColor = Color.White;
            this.skinButtonTypeDownACL.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonTypeDownACL.IsStyled = true;
            this.skinButtonTypeDownACL.Location = new Point(12, 0x89);
            this.skinButtonTypeDownACL.Name = "skinButtonTypeDownACL";
            this.skinButtonTypeDownACL.Size = new Size(0x105, 0x1a);
            this.skinButtonTypeDownACL.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonTypeDownACL, null);
            this.skinButtonTypeDownACL.TabIndex = 8;
            this.skinButtonTypeDownACL.TabStop = true;
            this.skinButtonTypeDownACL.Text = "Change content type download ACL";
            this.skinButtonTypeDownACL.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonTypeDownACL.TextPadding = new Padding(0);
            this.skinButtonTypeDownACL.Click += new EventHandler(this.skinButtonTypeDownACL_Click);
            this.skinButtonDisable.AutoStyle = true;
            this.skinButtonDisable.BackColor = Color.Transparent;
            this.skinButtonDisable.ButtonState = 0;
            this.skinButtonDisable.DialogResult = DialogResult.OK;
            this.skinButtonDisable.DisabledForecolor = Color.Gray;
            this.skinButtonDisable.DrawColor = Color.White;
            this.skinButtonDisable.DrawEdges = true;
            this.skinButtonDisable.FocusColor = Color.Yellow;
            this.skinButtonDisable.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDisable.ForeColor = Color.White;
            this.skinButtonDisable.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonDisable.IsStyled = true;
            this.skinButtonDisable.Location = new Point(12, 0xfc);
            this.skinButtonDisable.Name = "skinButtonDisable";
            this.skinButtonDisable.Size = new Size(0xb6, 0x1a);
            this.skinButtonDisable.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonDisable, null);
            this.skinButtonDisable.TabIndex = 9;
            this.skinButtonDisable.TabStop = true;
            this.skinButtonDisable.Text = "Disable this content";
            this.skinButtonDisable.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDisable.TextPadding = new Padding(0);
            this.skinButtonDisable.Click += new EventHandler(this.skinButtonDisable_Click);
            this.skinButtonContentDownACL.AutoStyle = true;
            this.skinButtonContentDownACL.BackColor = Color.Transparent;
            this.skinButtonContentDownACL.ButtonState = 0;
            this.skinButtonContentDownACL.DialogResult = DialogResult.OK;
            this.skinButtonContentDownACL.DisabledForecolor = Color.Gray;
            this.skinButtonContentDownACL.DrawColor = Color.White;
            this.skinButtonContentDownACL.DrawEdges = true;
            this.skinButtonContentDownACL.FocusColor = Color.Yellow;
            this.skinButtonContentDownACL.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonContentDownACL.ForeColor = Color.White;
            this.skinButtonContentDownACL.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonContentDownACL.IsStyled = true;
            this.skinButtonContentDownACL.Location = new Point(12, 0x69);
            this.skinButtonContentDownACL.Name = "skinButtonContentDownACL";
            this.skinButtonContentDownACL.Size = new Size(0x105, 0x1a);
            this.skinButtonContentDownACL.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonContentDownACL, null);
            this.skinButtonContentDownACL.TabIndex = 9;
            this.skinButtonContentDownACL.TabStop = true;
            this.skinButtonContentDownACL.Text = "Change content download ACL";
            this.skinButtonContentDownACL.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonContentDownACL.TextPadding = new Padding(0);
            this.skinButtonContentDownACL.Click += new EventHandler(this.skinButtonContentDownACL_Click);
            this.skinButtonTypeUpACL.AutoStyle = true;
            this.skinButtonTypeUpACL.BackColor = Color.Transparent;
            this.skinButtonTypeUpACL.ButtonState = 0;
            this.skinButtonTypeUpACL.DialogResult = DialogResult.OK;
            this.skinButtonTypeUpACL.DisabledForecolor = Color.Gray;
            this.skinButtonTypeUpACL.DrawColor = Color.White;
            this.skinButtonTypeUpACL.DrawEdges = true;
            this.skinButtonTypeUpACL.FocusColor = Color.Yellow;
            this.skinButtonTypeUpACL.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonTypeUpACL.ForeColor = Color.White;
            this.skinButtonTypeUpACL.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonTypeUpACL.IsStyled = true;
            this.skinButtonTypeUpACL.Location = new Point(12, 0xa9);
            this.skinButtonTypeUpACL.Name = "skinButtonTypeUpACL";
            this.skinButtonTypeUpACL.Size = new Size(0x105, 0x1a);
            this.skinButtonTypeUpACL.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonTypeUpACL, null);
            this.skinButtonTypeUpACL.TabIndex = 9;
            this.skinButtonTypeUpACL.TabStop = true;
            this.skinButtonTypeUpACL.Text = "Change content type upload ACL";
            this.skinButtonTypeUpACL.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonTypeUpACL.TextPadding = new Padding(0);
            this.skinButtonTypeUpACL.Click += new EventHandler(this.skinButtonTypeUpACL_Click);
            this.skinLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Transparent;
            this.skinLabel2.DrawEdges = true;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(6, 60);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0x1b3, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 0x12;
            this.skinLabel2.Text = "Content Administration";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(0);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1bf, 0x16b);
            base.Controls.Add(this.skinButtonTypeUpACL);
            base.Controls.Add(this.skinButtonContentDownACL);
            base.Controls.Add(this.skinLabel2);
            base.Controls.Add(this.skinButtonDisable);
            base.Controls.Add(this.skinButtonTypeDownACL);
            base.Controls.Add(this.skinButtonDelete);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x1bf, 0x16b);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x1bf, 0x16b);
            base.Name = "DlgContentAdmin";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Content Administration";
            base.Controls.SetChildIndex(this.skinButtonDelete, 0);
            base.Controls.SetChildIndex(this.skinButtonTypeDownACL, 0);
            base.Controls.SetChildIndex(this.skinButtonDisable, 0);
            base.Controls.SetChildIndex(this.skinLabel2, 0);
            base.Controls.SetChildIndex(this.skinButtonContentDownACL, 0);
            base.Controls.SetChildIndex(this.skinButtonTypeUpACL, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            AccessControlList list;
            if (User.Current.IsAdmin || (AccessControlList.GetByName("ContentAdmins", out list) && list.HasAccess()))
            {
                base.OnLoad(e);
            }
            else
            {
                base.Close();
            }
        }

        private void skinButtonContentDownACL_Click(object sender, EventArgs e)
        {
            DlgACLPicker picker = new DlgACLPicker();
            if (picker.ShowDialog() == DialogResult.OK)
            {
                int iD = 0;
                if (picker.SelectedACL != null)
                {
                    iD = picker.SelectedACL.ID;
                }
                if (new QuazalQuery("SetContentDownloadACL", new object[] { iD, this.Content.ID }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("ACL updated.");
                }
                else
                {
                    DlgMessage.ShowDialog("Error updating, ACL has not been changed.");
                }
            }
        }

        private void skinButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!new QuazalQuery("DeleteContent", new object[] { this.Content.ID, this.Content.ID, this.Content.ID, this.Content.ID, this.Content.ID, this.Content.ID, this.Content.ID, this.Content.ID, this.Content.ID }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("Error deleting content.");
                    return;
                }
                WebService.DeleteContent(AdditionalContent.VaultServerKey, this.Content.ContentType.Name, this.Content.Name, this.Content.Version, true);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                DlgMessage.ShowDialog("Error deleting content.");
                return;
            }
            DlgMessage.ShowDialog("Content has been deleted permanently.");
        }

        private void skinButtonDisable_Click(object sender, EventArgs e)
        {
            if (this.Content.Enabled)
            {
                if (new QuazalQuery("SetContentVisibility", new object[] { 0, this.Content.ID }).ExecuteNonQuery())
                {
                    this.Content.Enabled = false;
                    this.skinButtonDisable.Text = "Enable this content";
                    DlgMessage.ShowDialog("Content has been temporarily disabled.");
                }
                else
                {
                    DlgMessage.ShowDialog("Error disabling content.");
                }
            }
            else if (new QuazalQuery("SetContentVisibility", new object[] { 1, this.Content.ID }).ExecuteNonQuery())
            {
                this.Content.Enabled = false;
                this.skinButtonDisable.Text = "Disable this content";
                DlgMessage.ShowDialog("Content has been re-enabled.");
            }
            else
            {
                DlgMessage.ShowDialog("Error enabling content.");
            }
        }

        private void skinButtonTypeDownACL_Click(object sender, EventArgs e)
        {
            DlgACLPicker picker = new DlgACLPicker();
            if (picker.ShowDialog() == DialogResult.OK)
            {
                int iD = 0;
                if (picker.SelectedACL != null)
                {
                    iD = picker.SelectedACL.ID;
                }
                if (new QuazalQuery("SetContentTypeDownloadACL", new object[] { iD, this.Content.TypeID }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("ACL updated.");
                }
                else
                {
                    DlgMessage.ShowDialog("Error updating, ACL has not been changed.");
                }
            }
        }

        private void skinButtonTypeUpACL_Click(object sender, EventArgs e)
        {
            DlgACLPicker picker = new DlgACLPicker();
            if (picker.ShowDialog() == DialogResult.OK)
            {
                int iD = 0;
                if (picker.SelectedACL != null)
                {
                    iD = picker.SelectedACL.ID;
                }
                if (new QuazalQuery("SetContentTypeUploadACL", new object[] { iD, this.Content.TypeID }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("ACL updated.");
                }
                else
                {
                    DlgMessage.ShowDialog("Error updating, ACL has not been changed.");
                }
            }
        }

        public IAdditionalContent Content
        {
            get
            {
                return this.mContent;
            }
        }
    }
}

