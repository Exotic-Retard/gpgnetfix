namespace GPG.Multiplayer.Client.Controls.Awards
{
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Statistics;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class PnlAwardDegree : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabelDescription;
        private GPGPictureBox gpgPictureBoxAward;
        private GPG.Multiplayer.Statistics.Award mAward;
        private int mCurrentDegree;

        public PnlAwardDegree(GPG.Multiplayer.Statistics.Award award, int currentDegree)
        {
            this.InitializeComponent();
            this.gpgLabelDescription.Text = award.AchievementDescription;
            if (award.AwardDegree <= currentDegree)
            {
                this.gpgLabelDescription.ForeColor = Color.FromArgb(220, 220, 220);
            }
            else if (award.AwardDegree == (currentDegree + 1))
            {
                this.gpgLabelDescription.ForeColor = Color.FromArgb(0xcc, 0xcc, 0xff);
                this.gpgLabelDescription.Font = new Font(this.gpgLabelDescription.Font, FontStyle.Bold);
            }
            else
            {
                this.gpgLabelDescription.ForeColor = Color.Gray;
            }
            this.mAward = award;
            this.mCurrentDegree = currentDegree;
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
            this.gpgLabelDescription = new GPGLabel();
            this.gpgPictureBoxAward = new GPGPictureBox();
            ((ISupportInitialize) this.gpgPictureBoxAward).BeginInit();
            base.SuspendLayout();
            this.gpgLabelDescription.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelDescription.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDescription.AutoStyle = true;
            this.gpgLabelDescription.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabelDescription.ForeColor = Color.White;
            this.gpgLabelDescription.IgnoreMouseWheel = false;
            this.gpgLabelDescription.IsStyled = false;
            this.gpgLabelDescription.Location = new Point(0x36, 0);
            this.gpgLabelDescription.Name = "gpgLabelDescription";
            this.gpgLabelDescription.Size = new Size(0x123, 0x30);
            this.gpgLabelDescription.TabIndex = 1;
            this.gpgLabelDescription.Text = "gpgLabel1";
            this.gpgLabelDescription.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabelDescription.TextStyle = TextStyles.Custom;
            this.gpgPictureBoxAward.Location = new Point(0, 0);
            this.gpgPictureBoxAward.Name = "gpgPictureBoxAward";
            this.gpgPictureBoxAward.Size = new Size(0x30, 0x30);
            this.gpgPictureBoxAward.TabIndex = 0;
            this.gpgPictureBoxAward.TabStop = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gpgLabelDescription);
            base.Controls.Add(this.gpgPictureBoxAward);
            this.DoubleBuffered = true;
            base.Name = "PnlAwardDegree";
            base.Size = new Size(0x159, 0x30);
            ((ISupportInitialize) this.gpgPictureBoxAward).EndInit();
            base.ResumeLayout(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                VGen0 method = null;
                Image awardImage;
                if (this.Award.AwardDegree > this.CurrentDegree)
                {
                    awardImage = this.Award.AwardSet.BaseImage;
                }
                else
                {
                    awardImage = this.Award.LargeImage;
                }
                if (!base.Disposing && !base.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.gpgPictureBoxAward.Image = awardImage;
                            this.Height = awardImage.Height;
                        };
                    }
                    base.BeginInvoke(method);
                }
            });
        }

        public GPG.Multiplayer.Statistics.Award Award
        {
            get
            {
                return this.mAward;
            }
        }

        public int CurrentDegree
        {
            get
            {
                return this.mCurrentDegree;
            }
        }

        protected override bool ScaleChildren
        {
            get
            {
                return false;
            }
        }
    }
}

