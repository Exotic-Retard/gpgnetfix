namespace GPG.Multiplayer.Client.SolutionsLib
{
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class SearchResultEntry : UserControl, IStyledControl
    {
        private IContainer components = null;
        private GPGLabel gpgLabelLink;
        private bool mSelected = false;
        private Color mSelectedColor = Color.FromArgb(0xcc, 0xcc, 0xff);
        private GPG.Multiplayer.Client.SolutionsLib.Solution mSolution;
        private Color mUnselectedColor = Color.Black;

        public event EventHandler SolutionSelected;

        public SearchResultEntry(GPG.Multiplayer.Client.SolutionsLib.Solution solution)
        {
            this.InitializeComponent();
            this.mSolution = solution;
            this.gpgLabelLink.Text = string.Format("{0} - {1}", solution.ID, solution.Title);
            this.Unselect();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void gpgLabelLink_BackColorChanged(object sender, EventArgs e)
        {
            (sender as Control).BackColor = this.BackColor;
        }

        private void gpgLabelLink_Click(object sender, EventArgs e)
        {
            if (this.SolutionSelected != null)
            {
                this.SolutionSelected(this, EventArgs.Empty);
            }
        }

        private void InitializeComponent()
        {
            this.gpgLabelLink = new GPGLabel();
            base.SuspendLayout();
            this.gpgLabelLink.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgLabelLink.AutoStyle = true;
            this.gpgLabelLink.Font = new Font("Arial", 9.75f);
            this.gpgLabelLink.ForeColor = Color.White;
            this.gpgLabelLink.IgnoreMouseWheel = false;
            this.gpgLabelLink.IsStyled = false;
            this.gpgLabelLink.Location = new Point(3, 0);
            this.gpgLabelLink.Name = "gpgLabelLink";
            this.gpgLabelLink.Size = new Size(380, 0x1f);
            this.gpgLabelLink.TabIndex = 0;
            this.gpgLabelLink.Text = "gpgLabel1";
            this.gpgLabelLink.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabelLink.TextStyle = TextStyles.Link;
            this.gpgLabelLink.Click += new EventHandler(this.gpgLabelLink_Click);
            this.gpgLabelLink.BackColorChanged += new EventHandler(this.gpgLabelLink_BackColorChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gpgLabelLink);
            base.Name = "SearchResultEntry";
            base.Size = new Size(0x1b1, 0x1f);
            base.ResumeLayout(false);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (this.SolutionSelected != null)
            {
                this.SolutionSelected(this, EventArgs.Empty);
            }
        }

        public void Select()
        {
            this.mSelected = true;
            this.BackColor = this.SelectedColor;
            foreach (Control control in base.Controls)
            {
                control.BackColor = this.SelectedColor;
            }
            base.Select();
        }

        public void Unselect()
        {
            this.mSelected = true;
            this.BackColor = this.UnselectedColor;
            foreach (Control control in base.Controls)
            {
                control.BackColor = this.UnselectedColor;
            }
        }

        public bool AutoStyle
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public bool IsStyled
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public bool Selected
        {
            get
            {
                return this.mSelected;
            }
        }

        public Color SelectedColor
        {
            get
            {
                return this.mSelectedColor;
            }
            set
            {
                this.mSelectedColor = value;
            }
        }

        public GPG.Multiplayer.Client.SolutionsLib.Solution Solution
        {
            get
            {
                return this.mSolution;
            }
        }

        public Color UnselectedColor
        {
            get
            {
                return this.mUnselectedColor;
            }
            set
            {
                this.mUnselectedColor = value;
            }
        }
    }
}

