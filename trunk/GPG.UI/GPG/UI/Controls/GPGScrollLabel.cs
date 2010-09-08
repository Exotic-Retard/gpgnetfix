namespace GPG.UI.Controls
{
    using GPG.Multiplayer.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGScrollLabel : UserControl
    {
        private IContainer components;
        private GPGLabel gpgLabelText;
        private GPGScrollPanel gpgScrollPanelLabel;
        private const int PAD = 0x1a;

        public GPGScrollLabel()
        {
            MouseEventHandler handler = null;
            this.InitializeComponent();
            this.gpgScrollPanelLabel.ChildControl = this.gpgLabelText;
            if (handler == null)
            {
                handler = delegate (object sender, MouseEventArgs e) {
                    this.gpgLabelText.Select();
                };
            }
            this.gpgLabelText.MouseDown += handler;
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
            this.gpgScrollPanelLabel = new GPGScrollPanel();
            this.gpgLabelText = new GPGLabel();
            this.gpgScrollPanelLabel.SuspendLayout();
            base.SuspendLayout();
            this.gpgScrollPanelLabel.AutoScroll = true;
            this.gpgScrollPanelLabel.BackColor = Color.Black;
            this.gpgScrollPanelLabel.ChildControl = this.gpgLabelText;
            this.gpgScrollPanelLabel.Controls.Add(this.gpgLabelText);
            this.gpgScrollPanelLabel.Dock = DockStyle.Fill;
            this.gpgScrollPanelLabel.Location = new Point(0, 0);
            this.gpgScrollPanelLabel.Margin = new Padding(0);
            this.gpgScrollPanelLabel.Name = "gpgScrollPanelLabel";
            this.gpgScrollPanelLabel.Size = new Size(0x86, 0x2c);
            this.gpgScrollPanelLabel.TabIndex = 1;
            this.gpgLabelText.Font = new System.Drawing.Font("Arial", 9.75f);
            this.gpgLabelText.ForeColor = Color.White;
            this.gpgLabelText.IgnoreMouseWheel = false;
            this.gpgLabelText.Location = new Point(0, 0);
            this.gpgLabelText.Margin = new Padding(0);
            this.gpgLabelText.Name = "gpgLabelText";
            this.gpgLabelText.Size = new Size(0x43, 0x10);
            this.gpgLabelText.TabIndex = 0;
            this.gpgLabelText.Text = "gpgLabel1";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gpgScrollPanelLabel);
            base.Margin = new Padding(0);
            base.Name = "GPGScrollLabel";
            base.Size = new Size(0x86, 0x2c);
            this.gpgScrollPanelLabel.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public override Color BackColor
        {
            get
            {
                return this.gpgLabelText.BackColor;
            }
            set
            {
                this.gpgLabelText.BackColor = value;
                this.gpgScrollPanelLabel.BackColor = value;
            }
        }

        public override System.Drawing.Font Font
        {
            get
            {
                return this.gpgLabelText.Font;
            }
            set
            {
                this.gpgLabelText.Font = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return this.gpgLabelText.ForeColor;
            }
            set
            {
                this.gpgLabelText.ForeColor = value;
            }
        }

        public override string Text
        {
            get
            {
                return this.gpgLabelText.Text;
            }
            set
            {
                int num;
                this.gpgLabelText.Text = value;
                using (Graphics graphics = base.CreateGraphics())
                {
                    int num2;
                    Convert.ToInt32(Math.Round((double) graphics.MeasureString(this.gpgLabelText.Text, this.Font, new SizeF((float) (base.Width - 0x1a), float.MaxValue), StringFormat.GenericDefault, out num2, out num).Width, MidpointRounding.AwayFromZero));
                }
                this.gpgLabelText.Size = new Size(base.Width - 0x1a, (num * this.Font.Height) + num);
                this.gpgLabelText.Refresh();
            }
        }
    }
}

