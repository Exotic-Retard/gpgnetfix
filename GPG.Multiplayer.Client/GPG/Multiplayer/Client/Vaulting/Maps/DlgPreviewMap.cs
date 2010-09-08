namespace GPG.Multiplayer.Client.Vaulting.Maps
{
    using GPG;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgPreviewMap : Form
    {
        private IContainer components = null;
        private GPGLabel gpgLabelDesc;
        private CustomMap mMap;
        private PictureBox pictureBoxPreview;
        private SkinLabel skinLabelDescHeader;

        public DlgPreviewMap(CustomMap map)
        {
            this.InitializeComponent();
            this.SetMap(map);
        }

        private void CloseEvent(object sender, EventArgs e)
        {
            base.Hide();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GlobalMouseAction(object sender, MouseEventArgs e)
        {
            if (!base.Bounds.Contains(e.Location))
            {
                base.Hide();
            }
        }

        private void InitializeComponent()
        {
            this.gpgLabelDesc = new GPGLabel();
            this.pictureBoxPreview = new PictureBox();
            this.skinLabelDescHeader = new SkinLabel();
            ((ISupportInitialize) this.pictureBoxPreview).BeginInit();
            base.SuspendLayout();
            this.gpgLabelDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelDesc.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelDesc.AutoStyle = true;
            this.gpgLabelDesc.Cursor = Cursors.Hand;
            this.gpgLabelDesc.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabelDesc.ForeColor = Color.White;
            this.gpgLabelDesc.IgnoreMouseWheel = false;
            this.gpgLabelDesc.IsStyled = false;
            this.gpgLabelDesc.Location = new Point(130, 0x19);
            this.gpgLabelDesc.Name = "gpgLabelDesc";
            this.gpgLabelDesc.Size = new Size(0x176, 0x69);
            this.gpgLabelDesc.TabIndex = 0x16;
            this.gpgLabelDesc.Text = "gpgLabel1";
            this.gpgLabelDesc.TextStyle = TextStyles.Custom;
            this.gpgLabelDesc.Click += new EventHandler(this.CloseEvent);
            this.pictureBoxPreview.Cursor = Cursors.Hand;
            this.pictureBoxPreview.Location = new Point(2, 2);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new Size(0x80, 0x80);
            this.pictureBoxPreview.TabIndex = 20;
            this.pictureBoxPreview.TabStop = false;
            this.pictureBoxPreview.Click += new EventHandler(this.CloseEvent);
            this.skinLabelDescHeader.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabelDescHeader.AutoStyle = false;
            this.skinLabelDescHeader.BackColor = Color.Transparent;
            this.skinLabelDescHeader.Cursor = Cursors.Hand;
            this.skinLabelDescHeader.DrawEdges = true;
            this.skinLabelDescHeader.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabelDescHeader.ForeColor = Color.White;
            this.skinLabelDescHeader.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabelDescHeader.IsStyled = false;
            this.skinLabelDescHeader.Location = new Point(130, 2);
            this.skinLabelDescHeader.Name = "skinLabelDescHeader";
            this.skinLabelDescHeader.Size = new Size(0x176, 20);
            this.skinLabelDescHeader.SkinBasePath = @"Controls\Background Label\BlueGradient";
            this.skinLabelDescHeader.TabIndex = 0x15;
            this.skinLabelDescHeader.Text = "<LOC>Description";
            this.skinLabelDescHeader.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabelDescHeader.TextPadding = new Padding(0);
            this.skinLabelDescHeader.Click += new EventHandler(this.CloseEvent);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            base.ClientSize = new Size(0x1fa, 0x84);
            base.Controls.Add(this.gpgLabelDesc);
            base.Controls.Add(this.skinLabelDescHeader);
            base.Controls.Add(this.pictureBoxPreview);
            this.Cursor = Cursors.Hand;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "DlgPreviewMap";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "DlgPreviewMap";
            base.TopMost = true;
            ((ISupportInitialize) this.pictureBoxPreview).EndInit();
            base.ResumeLayout(false);
        }

        protected override void OnClick(EventArgs e)
        {
            base.Hide();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Brushes.White, 4f))
            {
                e.Graphics.DrawRectangle(pen, base.ClientRectangle);
            }
        }

        public void SetMap(CustomMap map)
        {
            this.skinLabelDescHeader.Text = Loc.Get(this.skinLabelDescHeader.Text);
            this.gpgLabelDesc.Text = Loc.Get(map.Description);
            this.pictureBoxPreview.Image = map.PreviewImage128;
            this.mMap = map;
        }

        public CustomMap Map
        {
            get
            {
                return this.mMap;
            }
        }
    }
}

