namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PnlContentComment : UserControl
    {
        private IContainer components = null;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabelComment;
        private GPGLabel gpgLabelHeader;
        private GPGLabel gpgLabelLastUpdate;
        private GPGPictureBox gpgPictureBoxDelete;
        private GPGPictureBox gpgPictureBoxEdit;
        private ContentComment mComment;
        private IAdditionalContent mContent;
        private ToolTip toolTip;

        public event EventHandler DeleteComment;

        public event EventHandler UpdateComment;

        public PnlContentComment(IAdditionalContent content, ContentComment comment)
        {
            this.InitializeComponent();
            this.mContent = content;
            this.mComment = comment;
        }

        internal void BindToComment()
        {
            this.gpgLabelHeader.Text = string.Format(Loc.Get("<LOC>On {0} {1} posted:"), this.Comment.CreateDate.ToShortDateString(), this.Comment.PlayerName);
            this.gpgLabelComment.Text = this.Comment.CommentText;
            if (this.Comment.EditCount > 0)
            {
                if (!this.gpgLabelLastUpdate.Visible)
                {
                    this.gpgLabelComment.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    base.Height += this.gpgLabelLastUpdate.Height;
                    this.gpgLabelComment.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
                }
                this.gpgLabelLastUpdate.Visible = true;
                this.gpgLabelLastUpdate.Text = string.Format(Loc.Get("<LOC>Last edited by {0} on {1}; edited {2} time(s) in total"), this.Comment.LastEditByName, this.Comment.LastEditDate.ToShortDateString(), this.Comment.EditCount);
            }
            else
            {
                this.gpgLabelLastUpdate.Visible = false;
                this.gpgLabelComment.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                base.Height -= this.gpgLabelLastUpdate.Height;
                this.gpgLabelComment.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            }
            this.gpgPictureBoxDelete.Visible = ((this.Comment.PlayerID == User.Current.ID) || this.Content.CurrentUserIsOwner) || User.Current.IsAdmin;
            this.gpgPictureBoxEdit.Visible = this.Comment.PlayerID == User.Current.ID;
            if (this.gpgPictureBoxDelete.Visible)
            {
                this.gpgPictureBoxDelete.BringToFront();
                this.toolTip.SetToolTip(this.gpgPictureBoxDelete, Loc.Get("<LOC>Delete Comment"));
            }
            if (this.gpgPictureBoxEdit.Visible)
            {
                this.gpgPictureBoxEdit.BringToFront();
                this.toolTip.SetToolTip(this.gpgPictureBoxEdit, Loc.Get("<LOC>Edit Comment"));
            }
            this.ResizeToComment();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgPictureBoxDelete_Click(object sender, EventArgs e)
        {
            if (((new DlgYesNo(Program.MainForm, "<LOC>Confirm Delete", "<LOC>Are you sure you want to delete this comment?").ShowDialog() == DialogResult.Yes) && DataAccess.ExecuteQuery("DeleteContentComment", new object[] { this.Comment.ID })) && (this.DeleteComment != null))
            {
                this.DeleteComment(this.Comment, EventArgs.Empty);
            }
        }

        private void gpgPictureBoxEdit_Click(object sender, EventArgs e)
        {
            DlgPostComment comment = new DlgPostComment(this.Comment.CommentText);
            if ((comment.ShowDialog() == DialogResult.OK) && new QuazalQuery("UpdateContentComment", new object[] { comment.Comment, this.Comment.ID }).ExecuteNonQuery())
            {
                this.Comment.LastEditDate = DateTime.Now;
                this.Comment.CommentText = comment.Comment;
                ContentComment comment1 = this.Comment;
                comment1.EditCount++;
                this.Comment.LastEditByID = User.Current.ID;
                this.Comment.LastEditByName = User.Current.Name;
                this.BindToComment();
                if (this.UpdateComment != null)
                {
                    this.UpdateComment(this.Comment, EventArgs.Empty);
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PnlContentComment));
            this.gpgLabelHeader = new GPGLabel();
            this.gpgLabelComment = new GPGLabel();
            this.gpgPictureBoxDelete = new GPGPictureBox();
            this.toolTip = new ToolTip(this.components);
            this.gpgPictureBoxEdit = new GPGPictureBox();
            this.gpgLabelLastUpdate = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            ((ISupportInitialize) this.gpgPictureBoxDelete).BeginInit();
            ((ISupportInitialize) this.gpgPictureBoxEdit).BeginInit();
            base.SuspendLayout();
            this.gpgLabelHeader.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelHeader.AutoSize = true;
            this.gpgLabelHeader.AutoStyle = true;
            this.gpgLabelHeader.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabelHeader.ForeColor = Color.DarkGray;
            this.gpgLabelHeader.IgnoreMouseWheel = false;
            this.gpgLabelHeader.IsStyled = false;
            this.gpgLabelHeader.Location = new Point(3, 1);
            this.gpgLabelHeader.Name = "gpgLabelHeader";
            this.gpgLabelHeader.Size = new Size(0x39, 12);
            this.gpgLabelHeader.TabIndex = 0;
            this.gpgLabelHeader.Text = "gpgLabel1";
            this.gpgLabelHeader.TextStyle = TextStyles.Custom;
            this.gpgLabelComment.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelComment.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelComment.AutoStyle = true;
            this.gpgLabelComment.BackColor = Color.Transparent;
            this.gpgLabelComment.Font = new Font("Arial", 9.75f);
            this.gpgLabelComment.ForeColor = Color.White;
            this.gpgLabelComment.IgnoreMouseWheel = false;
            this.gpgLabelComment.IsStyled = false;
            this.gpgLabelComment.Location = new Point(12, 0x12);
            this.gpgLabelComment.Name = "gpgLabelComment";
            this.gpgLabelComment.Size = new Size(460, 0x19);
            this.gpgLabelComment.TabIndex = 1;
            this.gpgLabelComment.Text = "gpgLabel1";
            this.gpgLabelComment.TextStyle = TextStyles.Default;
            this.gpgPictureBoxDelete.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxDelete.Cursor = Cursors.Hand;
            this.gpgPictureBoxDelete.Image = (Image) manager.GetObject("gpgPictureBoxDelete.Image");
            this.gpgPictureBoxDelete.Location = new Point(0x1d1, 2);
            this.gpgPictureBoxDelete.Name = "gpgPictureBoxDelete";
            this.gpgPictureBoxDelete.Size = new Size(0x10, 0x10);
            this.gpgPictureBoxDelete.SizeMode = PictureBoxSizeMode.AutoSize;
            this.gpgPictureBoxDelete.TabIndex = 2;
            this.gpgPictureBoxDelete.TabStop = false;
            this.gpgPictureBoxDelete.Visible = false;
            this.gpgPictureBoxDelete.Click += new EventHandler(this.gpgPictureBoxDelete_Click);
            this.gpgPictureBoxEdit.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxEdit.Cursor = Cursors.Hand;
            this.gpgPictureBoxEdit.Image = (Image) manager.GetObject("gpgPictureBoxEdit.Image");
            this.gpgPictureBoxEdit.Location = new Point(0x1bf, 2);
            this.gpgPictureBoxEdit.Name = "gpgPictureBoxEdit";
            this.gpgPictureBoxEdit.Size = new Size(0x10, 0x10);
            this.gpgPictureBoxEdit.SizeMode = PictureBoxSizeMode.AutoSize;
            this.gpgPictureBoxEdit.TabIndex = 3;
            this.gpgPictureBoxEdit.TabStop = false;
            this.gpgPictureBoxEdit.Visible = false;
            this.gpgPictureBoxEdit.Click += new EventHandler(this.gpgPictureBoxEdit_Click);
            this.gpgLabelLastUpdate.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabelLastUpdate.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelLastUpdate.AutoSize = true;
            this.gpgLabelLastUpdate.AutoStyle = true;
            this.gpgLabelLastUpdate.Font = new Font("Verdana", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabelLastUpdate.ForeColor = Color.DarkGray;
            this.gpgLabelLastUpdate.IgnoreMouseWheel = false;
            this.gpgLabelLastUpdate.IsStyled = false;
            this.gpgLabelLastUpdate.Location = new Point(3, 0x2a);
            this.gpgLabelLastUpdate.Name = "gpgLabelLastUpdate";
            this.gpgLabelLastUpdate.Size = new Size(0x39, 12);
            this.gpgLabelLastUpdate.TabIndex = 4;
            this.gpgLabelLastUpdate.Text = "gpgLabel1";
            this.gpgLabelLastUpdate.TextStyle = TextStyles.Custom;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(3, 0x3d);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x1dd, 1);
            this.gpgLabel1.TabIndex = 5;
            this.gpgLabel1.TextStyle = TextStyles.Default;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.gpgLabelLastUpdate);
            base.Controls.Add(this.gpgPictureBoxEdit);
            base.Controls.Add(this.gpgPictureBoxDelete);
            base.Controls.Add(this.gpgLabelComment);
            base.Controls.Add(this.gpgLabelHeader);
            base.Name = "PnlContentComment";
            base.Size = new Size(0x1e3, 0x43);
            ((ISupportInitialize) this.gpgPictureBoxDelete).EndInit();
            ((ISupportInitialize) this.gpgPictureBoxEdit).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.BindToComment();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (base.Parent != null)
            {
                this.BackColor = base.Parent.BackColor;
            }
        }

        internal void ResizeToComment()
        {
            if (this.gpgLabelComment != null)
            {
                int height = this.gpgLabelComment.Height;
                int num2 = ((int) DrawUtil.MeasureString(this.gpgLabelComment.CreateGraphics(), this.gpgLabelComment.Text, this.gpgLabelComment.Font, (float) this.gpgLabelComment.Width).Height) + 6;
                base.Height += num2 - height;
            }
        }

        public ContentComment Comment
        {
            get
            {
                return this.mComment;
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

