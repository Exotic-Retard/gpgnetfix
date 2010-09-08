namespace GPG.Multiplayer.Client.Controls.Awards
{
    using GPG;
    using GPG.Multiplayer.Statistics;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class AwardSetImage : PictureBox
    {
        private IContainer components;

        public AwardSetImage()
        {
            this.components = null;
            this.InitializeComponent();
            base.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public AwardSetImage(Award award, AwardImageTypes type) : this()
        {
            WaitCallback callBack = null;
            if (callBack == null)
            {
                callBack = delegate (object s) {
                    VGen0 method = null;
                    Image awardImage;
                    if (type == AwardImageTypes.Large)
                    {
                        awardImage = award.LargeImage;
                    }
                    else
                    {
                        awardImage = award.SmallImage;
                    }
                    if (!this.Disposing && !this.IsDisposed)
                    {
                        if (method == null)
                        {
                            method = delegate {
                                this.Image = awardImage;
                            };
                        }
                        this.BeginInvoke(method);
                    }
                };
            }
            ThreadPool.QueueUserWorkItem(callBack);
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
            this.components = new Container();
        }
    }
}

