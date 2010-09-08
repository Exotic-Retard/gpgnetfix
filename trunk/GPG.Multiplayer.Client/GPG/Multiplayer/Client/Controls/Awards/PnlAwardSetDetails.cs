namespace GPG.Multiplayer.Client.Controls.Awards
{
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Statistics;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PnlAwardSetDetails : PnlBase
    {
        private IContainer components = null;
        private GPGLabel gpgLabelAwardSetName;
        private AwardSet mAward;
        private int mDegree;
        private SkinLabel skinLabelCategory;

        public PnlAwardSetDetails(AwardSet award, int degree)
        {
            this.InitializeComponent();
            this.mAward = award;
            this.mDegree = degree;
            this.BindToAward();
        }

        public void BindToAward()
        {
            this.skinLabelCategory.Text = this.Award.Name;
            this.gpgLabelAwardSetName.Text = this.Award.Description;
            int num = 4;
            int num2 = this.gpgLabelAwardSetName.Bottom + 8;
            int bottom = 0;
            int width = 0;
            foreach (GPG.Multiplayer.Statistics.Award award in this.Award.Awards)
            {
                PnlAwardDegree degree = new PnlAwardDegree(award, this.Degree);
                degree.Top = num2 + ((degree.Height + num) * (award.AwardDegree - 1));
                degree.Left = num;
                if (degree.Bottom > bottom)
                {
                    bottom = degree.Bottom;
                }
                width = degree.Width;
                base.Controls.Add(degree);
            }
            base.Height = bottom + num;
            base.Width = width + (num * 2);
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
            this.skinLabelCategory = new SkinLabel();
            this.gpgLabelAwardSetName = new GPGLabel();
            base.SuspendLayout();
            this.skinLabelCategory.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabelCategory.AutoStyle = false;
            this.skinLabelCategory.BackColor = Color.Transparent;
            this.skinLabelCategory.DrawEdges = true;
            this.skinLabelCategory.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelCategory.ForeColor = Color.White;
            this.skinLabelCategory.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelCategory.IsStyled = false;
            this.skinLabelCategory.Location = new Point(2, 2);
            this.skinLabelCategory.Name = "skinLabelCategory";
            this.skinLabelCategory.Size = new Size(0x11b, 20);
            this.skinLabelCategory.SkinBasePath = @"Controls\Background Label\BlueGradient";
            this.skinLabelCategory.TabIndex = 0;
            this.skinLabelCategory.Text = "skinLabel1";
            this.skinLabelCategory.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabelCategory.TextPadding = new Padding(0);
            this.gpgLabelAwardSetName.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelAwardSetName.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelAwardSetName.AutoStyle = true;
            this.gpgLabelAwardSetName.BackColor = Color.Black;
            this.gpgLabelAwardSetName.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelAwardSetName.ForeColor = Color.White;
            this.gpgLabelAwardSetName.IgnoreMouseWheel = false;
            this.gpgLabelAwardSetName.IsStyled = false;
            this.gpgLabelAwardSetName.Location = new Point(2, 0x18);
            this.gpgLabelAwardSetName.Name = "gpgLabelAwardSetName";
            this.gpgLabelAwardSetName.Size = new Size(0x11b, 14);
            this.gpgLabelAwardSetName.TabIndex = 1;
            this.gpgLabelAwardSetName.Text = "gpgLabel1";
            this.gpgLabelAwardSetName.TextStyle = TextStyles.Custom;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gpgLabelAwardSetName);
            base.Controls.Add(this.skinLabelCategory);
            this.DoubleBuffered = true;
            base.Name = "PnlAwardSetDetails";
            base.Size = new Size(0x11f, 0xf4);
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            base.OnPaint(e);
            using (Pen pen = new Pen(Program.Settings.StylePreferences.HighlightColor3, 4f))
            {
                e.Graphics.DrawRectangle(pen, base.ClientRectangle);
            }
        }

        public AwardSet Award
        {
            get
            {
                return this.mAward;
            }
        }

        public int Degree
        {
            get
            {
                return this.mDegree;
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

