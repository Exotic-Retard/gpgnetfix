namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PnlReplayComment : UserControl
    {
        private IContainer components = null;
        private GPGLabel gpgLabelComment;
        private GPGLabel gpgLabelHeader;
        private GPGPictureBox gpgPictureBoxDelete;
        private ReplayComment mComment;
        private ReplayInfo mReplay;
        private ToolTip toolTip;

        public event EventHandler DeleteComment;

        public PnlReplayComment(ReplayInfo replay, ReplayComment comment)
        {
            this.InitializeComponent();
            this.mReplay = replay;
            this.mComment = comment;
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
            if (((new DlgYesNo(Program.MainForm, "<LOC>Confirm Delete", "<LOC>Are you sure you want to delete this comment?").ShowDialog() == DialogResult.Yes) && DataAccess.ExecuteQuery("DeleteReplayComment", new object[] { this.Comment.ID })) && (this.DeleteComment != null))
            {
                this.DeleteComment(this.Comment, EventArgs.Empty);
            }
        }

        private void gpgPictureBoxDelete_MouseEnter(object sender, EventArgs e)
        {
            this.gpgPictureBoxDelete.Image = Resources.delete_comment_over;
        }

        private void gpgPictureBoxDelete_MouseLeave(object sender, EventArgs e)
        {
            this.gpgPictureBoxDelete.Image = Resources.delete_comment;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.gpgLabelHeader = new GPGLabel();
            this.gpgLabelComment = new GPGLabel();
            this.gpgPictureBoxDelete = new GPGPictureBox();
            this.toolTip = new ToolTip(this.components);
            ((ISupportInitialize) this.gpgPictureBoxDelete).BeginInit();
            base.SuspendLayout();
            this.gpgLabelHeader.AutoSize = true;
            this.gpgLabelHeader.AutoStyle = true;
            this.gpgLabelHeader.Font = new Font("Arial", 9.75f);
            this.gpgLabelHeader.ForeColor = Color.White;
            this.gpgLabelHeader.IgnoreMouseWheel = false;
            this.gpgLabelHeader.IsStyled = false;
            this.gpgLabelHeader.Location = new Point(3, 0);
            this.gpgLabelHeader.Name = "gpgLabelHeader";
            this.gpgLabelHeader.Size = new Size(0x43, 0x10);
            this.gpgLabelHeader.TabIndex = 0;
            this.gpgLabelHeader.Text = "gpgLabel1";
            this.gpgLabelHeader.TextStyle = TextStyles.Bold;
            this.gpgLabelComment.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelComment.AutoStyle = true;
            this.gpgLabelComment.BackColor = Color.Black;
            this.gpgLabelComment.Font = new Font("Arial", 9.75f);
            this.gpgLabelComment.ForeColor = Color.DarkGray;
            this.gpgLabelComment.IgnoreMouseWheel = false;
            this.gpgLabelComment.IsStyled = false;
            this.gpgLabelComment.Location = new Point(0x11, 0x10);
            this.gpgLabelComment.Name = "gpgLabelComment";
            this.gpgLabelComment.Size = new Size(0x1f9, 20);
            this.gpgLabelComment.TabIndex = 1;
            this.gpgLabelComment.Text = "gpgLabel1";
            this.gpgLabelComment.TextStyle = TextStyles.Colored;
            this.gpgPictureBoxDelete.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.gpgPictureBoxDelete.Cursor = Cursors.Hand;
            this.gpgPictureBoxDelete.Image = Resources.delete_comment;
            this.gpgPictureBoxDelete.Location = new Point(0x20c, 2);
            this.gpgPictureBoxDelete.Name = "gpgPictureBoxDelete";
            this.gpgPictureBoxDelete.Size = new Size(0x10, 0x10);
            this.gpgPictureBoxDelete.SizeMode = PictureBoxSizeMode.AutoSize;
            this.gpgPictureBoxDelete.TabIndex = 2;
            this.gpgPictureBoxDelete.TabStop = false;
            this.gpgPictureBoxDelete.Visible = false;
            this.gpgPictureBoxDelete.MouseLeave += new EventHandler(this.gpgPictureBoxDelete_MouseLeave);
            this.gpgPictureBoxDelete.Click += new EventHandler(this.gpgPictureBoxDelete_Click);
            this.gpgPictureBoxDelete.MouseEnter += new EventHandler(this.gpgPictureBoxDelete_MouseEnter);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.Controls.Add(this.gpgPictureBoxDelete);
            base.Controls.Add(this.gpgLabelComment);
            base.Controls.Add(this.gpgLabelHeader);
            base.Name = "PnlReplayComment";
            base.Size = new Size(0x21e, 0x26);
            ((ISupportInitialize) this.gpgPictureBoxDelete).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgLabelHeader.Text = string.Format(Loc.Get("<LOC>On {0} {1} posted:"), this.Comment.CreateDate.ToShortDateString(), this.Comment.PlayerName);
            this.gpgLabelComment.Text = this.Comment.Comment;
            this.gpgPictureBoxDelete.Visible = ((this.Comment.PlayerID == User.Current.ID) || (this.Replay.PlayerID == User.Current.ID)) || User.Current.IsAdmin;
            if (this.gpgPictureBoxDelete.Visible)
            {
                this.gpgPictureBoxDelete.BringToFront();
                this.toolTip.SetToolTip(this.gpgPictureBoxDelete, Loc.Get("<LOC>Delete Comment"));
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (base.Parent != null)
            {
                this.BackColor = base.Parent.BackColor;
            }
        }

        internal void ResizeComment()
        {
            if (this.gpgLabelComment != null)
            {
                int height = this.gpgLabelComment.Height;
                int num2 = (int) DrawUtil.MeasureString(this.gpgLabelComment.CreateGraphics(), this.gpgLabelComment.Text, this.gpgLabelComment.Font, (float) this.gpgLabelComment.Width).Height;
                base.Height += num2 - height;
            }
        }

        public ReplayComment Comment
        {
            get
            {
                return this.mComment;
            }
        }

        public ReplayInfo Replay
        {
            get
            {
                return this.mReplay;
            }
        }
    }
}

