namespace GPG.UI.Controls
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using GPG.Logging;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGTextList : UserControl
    {
        private IContainer components;
        private ListBoxControl listBoxTextLines;
        private Control mAnchorControl;
        private int mMaxLines = 6;
        private int mSelectedIndex;
        private ITextVal[] mTextLines;
        private Point Origin;
        private bool OriginSet;
        private int ParentHeight;

        public event EventHandler<TextValEventArgs> ValueSelected;

        public GPGTextList()
        {
            this.InitializeComponent();
            this.listBoxTextLines.Click += new EventHandler(this.listBoxTextLines_Click);
            this.listBoxTextLines.KeyDown += new KeyEventHandler(this.this_KeyDown);
            base.KeyDown += new KeyEventHandler(this.this_KeyDown);
            base.Height = this.listBoxTextLines.ItemHeight;
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
            this.listBoxTextLines = new ListBoxControl();
            ((ISupportInitialize) this.listBoxTextLines).BeginInit();
            base.SuspendLayout();
            this.listBoxTextLines.Appearance.BackColor = Color.DimGray;
            this.listBoxTextLines.Appearance.BorderColor = Color.FromArgb(0x80, 0x80, 0xff);
            this.listBoxTextLines.Appearance.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.listBoxTextLines.Appearance.ForeColor = Color.FromArgb(0xff, 0xff, 0x80);
            this.listBoxTextLines.Appearance.Options.UseBackColor = true;
            this.listBoxTextLines.Appearance.Options.UseBorderColor = true;
            this.listBoxTextLines.Appearance.Options.UseFont = true;
            this.listBoxTextLines.Appearance.Options.UseForeColor = true;
            this.listBoxTextLines.Appearance.Options.UseTextOptions = true;
            this.listBoxTextLines.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
            this.listBoxTextLines.BorderStyle = BorderStyles.Simple;
            this.listBoxTextLines.Dock = DockStyle.Fill;
            this.listBoxTextLines.HotTrackItems = true;
            this.listBoxTextLines.ItemHeight = 20;
            this.listBoxTextLines.Location = new Point(0, 0);
            this.listBoxTextLines.Margin = new Padding(0, 0, 0, 0);
            this.listBoxTextLines.Name = "listBoxTextLines";
            this.listBoxTextLines.Size = new Size(0x22d, 0x79);
            this.listBoxTextLines.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = Color.Black;
            base.Controls.Add(this.listBoxTextLines);
            base.Margin = new Padding(0, 0, 0, 0);
            base.Name = "GPGTextList";
            base.Size = new Size(0x22d, 0x79);
            ((ISupportInitialize) this.listBoxTextLines).EndInit();
            base.ResumeLayout(false);
        }

        private void listBoxTextLines_Click(object sender, EventArgs e)
        {
            this.OnValueSelected();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (!this.OriginSet)
            {
                this.Origin = new Point(base.Location.X, base.Location.Y + base.Height);
                this.OriginSet = true;
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.ParentHeight = base.Parent.ClientSize.Height;
            base.Parent.SizeChanged += new EventHandler(this.Parent_SizeChanged);
        }

        private void OnValueSelected()
        {
            if (this.ValueSelected != null)
            {
                this.ValueSelected(this, new TextValEventArgs(this.listBoxTextLines.SelectedItem as ITextVal));
            }
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            int num = base.Parent.ClientSize.Height - this.ParentHeight;
            this.Origin = new Point(this.Origin.X, this.Origin.Y + num);
            this.ParentHeight = base.Parent.ClientSize.Height;
        }

        private void this_KeyDown(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (keyCode != Keys.Return)
            {
                if (keyCode != Keys.Escape)
                {
                    if ((keyCode == Keys.Down) && (this.listBoxTextLines.SelectedIndex == (this.listBoxTextLines.Items.Count - 1)))
                    {
                        this.AnchorControl.Select();
                        if (this.AnchorControl is GPGTextBox)
                        {
                            (this.AnchorControl as GPGTextBox).Select((this.AnchorControl as GPGTextBox).Text.Length, 0);
                        }
                    }
                    return;
                }
            }
            else
            {
                this.OnValueSelected();
                return;
            }
            this.AnchorControl.Select();
            if (this.AnchorControl is GPGTextBox)
            {
                (this.AnchorControl as GPGTextBox).Select((this.AnchorControl as GPGTextBox).Text.Length, 0);
            }
        }

        public Control AnchorControl
        {
            get
            {
                return this.mAnchorControl;
            }
            set
            {
                this.mAnchorControl = value;
            }
        }

        public int MaxLines
        {
            get
            {
                return this.mMaxLines;
            }
            set
            {
                this.mMaxLines = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.listBoxTextLines.SelectedIndex;
            }
            set
            {
                this.listBoxTextLines.SelectedIndex = value;
            }
        }

        public ITextVal SelectedValue
        {
            get
            {
                if ((this.listBoxTextLines.SelectedIndex >= 0) && (this.listBoxTextLines.SelectedItem is ITextVal))
                {
                    return (this.listBoxTextLines.SelectedItem as ITextVal);
                }
                return null;
            }
        }

        public ITextVal[] TextLines
        {
            get
            {
                return this.mTextLines;
            }
            set
            {
                this.mTextLines = value;
                if (this.AnchorControl != null)
                {
                    this.listBoxTextLines.Items.Clear();
                    if (this.TextLines != null)
                    {
                        int num = 2;
                        if (this.TextLines.Length > this.MaxLines)
                        {
                            base.Height = (this.MaxLines * this.listBoxTextLines.ItemHeight) + num;
                        }
                        else
                        {
                            base.Height = (this.TextLines.Length * this.listBoxTextLines.ItemHeight) + num;
                        }
                        base.Location = new Point(base.Left, this.Origin.Y - base.Height);
                        for (int i = 0; i < this.TextLines.Length; i++)
                        {
                            this.listBoxTextLines.Items.Add(this.TextLines[i]);
                        }
                        this.listBoxTextLines.SelectedIndex = this.listBoxTextLines.Items.Count;
                    }
                }
                else
                {
                    base.Visible = false;
                    ErrorLog.WriteLine("GPGTextList {0} has no AnchorControl set, and will not display.", new object[] { base.Name });
                }
            }
        }
    }
}

