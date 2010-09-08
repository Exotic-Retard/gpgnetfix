namespace GPG.UI.Controls.Docking
{
    using GPG.UI.Controls;
    using GPG.UI.Controls.Skinning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DockTabGroup : UserControl, IDockableContainer, ISkinResourceConsumer
    {
        private IContainer components;
        private GPGPanel gpgPanelContainer;
        private GPGPanel gpgPanelTabs;
        private DockPanel mActivePanel;
        private Image mBorderImage;
        private Control mContainerControl;
        private DockContainerForm mContainerForm;
        private TabOrientations mOrientation;
        private List<DockPanel> mPanels;
        private int mTabHeight;

        public DockTabGroup(TabOrientations orientation)
        {
            EventHandler handler = null;
            this.mOrientation = TabOrientations.Bottom;
            this.mPanels = new List<DockPanel>();
            this.InitializeComponent();
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mOrientation = orientation;
            this.RefreshSkin();
            if (handler == null)
            {
                handler = delegate (object s, EventArgs e) {
                    this.ArrangePanels();
                };
            }
            base.SizeChanged += handler;
            this.ArrangePanels();
        }

        public void AddTab(DockPanel panel)
        {
            TabButton button = new TabButton(this.Orientation, panel);
            button.Left = this.Panels.Count * button.Width;
            this.Panels.Add(panel);
            button.MouseDown += new MouseEventHandler(this.TabClick);
            foreach (TabButton button2 in this.gpgPanelTabs.Controls)
            {
                button2.Unselect();
            }
            this.gpgPanelContainer.Controls.Add(panel);
            this.gpgPanelTabs.Controls.Add(button);
            this.EnsureTabsFit();
            this.SwapPanels(panel);
        }

        private void ArrangePanels()
        {
            if (this.Orientation != TabOrientations.Bottom)
            {
                throw new NotImplementedException(string.Format("Tab orientation {0} not yet implemented", this.Orientation));
            }
            this.gpgPanelTabs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gpgPanelTabs.Height = this.TabHeight;
            this.gpgPanelContainer.Left = this.BorderImage.Width;
            this.gpgPanelContainer.Top = this.BorderImage.Height;
            this.gpgPanelContainer.Width = base.Width - (this.BorderImage.Width * 2);
            this.gpgPanelContainer.Height = base.Height - (this.gpgPanelTabs.Height + (this.BorderImage.Height * 2));
            this.gpgPanelContainer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DockTo(IDockableContainer target, DockStyles orientation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private void EnsureTabsFit()
        {
            int num = 0;
            foreach (TabButton button in this.gpgPanelTabs.Controls)
            {
                num += button.Width;
            }
            if (num > this.gpgPanelTabs.Width)
            {
                this.gpgPanelTabs.Height = this.TabHeight + 20;
            }
            else
            {
                this.gpgPanelTabs.Height = this.TabHeight;
            }
        }

        private void InitializeComponent()
        {
            this.gpgPanelTabs = new GPGPanel();
            this.gpgPanelContainer = new GPGPanel();
            base.SuspendLayout();
            this.gpgPanelTabs.AutoScroll = true;
            this.gpgPanelTabs.Location = new Point(0x3a, 0x7d);
            this.gpgPanelTabs.Margin = new Padding(0);
            this.gpgPanelTabs.Name = "gpgPanelTabs";
            this.gpgPanelTabs.Size = new Size(200, 0x38);
            this.gpgPanelTabs.TabIndex = 0;
            this.gpgPanelContainer.Location = new Point(3, 3);
            this.gpgPanelContainer.Margin = new Padding(0);
            this.gpgPanelContainer.Name = "gpgPanelContainer";
            this.gpgPanelContainer.Size = new Size(0x12e, 0x5c);
            this.gpgPanelContainer.TabIndex = 1;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.gpgPanelTabs);
            base.Controls.Add(this.gpgPanelContainer);
            base.Name = "DockTabGroup";
            base.Size = new Size(0x151, 0xd4);
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int num = base.Width / this.BorderImage.Width;
            if ((base.Width % this.BorderImage.Width) > 0)
            {
                num++;
            }
            int num2 = (base.Height - this.gpgPanelTabs.Height) / this.BorderImage.Height;
            if ((base.Height % this.BorderImage.Height) > 0)
            {
                num2++;
            }
            for (int i = 0; i < num; i++)
            {
                e.Graphics.DrawImageUnscaled(this.BorderImage, i * this.BorderImage.Width, 0);
                e.Graphics.DrawImageUnscaled(this.BorderImage, i * this.BorderImage.Width, this.gpgPanelContainer.Bottom);
            }
            for (int j = 0; j < num2; j++)
            {
                e.Graphics.DrawImageUnscaled(this.BorderImage, 0, j * this.BorderImage.Height);
                e.Graphics.DrawImageUnscaled(this.BorderImage, this.gpgPanelContainer.Right, j * this.BorderImage.Height);
            }
            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.EnsureTabsFit();
        }

        public void RefreshSkin()
        {
            this.mTabHeight = this.Manager.GetImage(string.Format("tab_{0}_active_mid", this.Orientation.ToString().ToLower())).Height;
            this.mBorderImage = this.Manager.GetImage("tab_border");
        }

        public void RemoveTab(DockPanel panel)
        {
            if (this.Panels.Count > 1)
            {
                int index = 0;
                while (index < this.Panels.Count)
                {
                    if (this.Panels[index].Equals(panel))
                    {
                        break;
                    }
                    index++;
                }
                if (index >= 0)
                {
                    if ((this.ActivePanel != null) && this.ActivePanel.Equals(panel))
                    {
                        if (index == 0)
                        {
                            this.mActivePanel = this.Panels[1];
                        }
                        else
                        {
                            this.mActivePanel = this.Panels[index - 1];
                        }
                    }
                    this.SwapPanels(this.ActivePanel);
                    this.Panels.RemoveAt(index);
                    TabButton button = null;
                    foreach (TabButton button2 in this.gpgPanelTabs.Controls)
                    {
                        if (button2.Panel.Equals(panel))
                        {
                            button = button2;
                        }
                        else if (button2.Panel.Equals(this.ActivePanel))
                        {
                            button2.Select();
                        }
                        if (button != null)
                        {
                            button2.Left -= button.Width;
                        }
                    }
                    if (button != null)
                    {
                        this.gpgPanelTabs.Controls.Remove(button);
                    }
                }
            }
        }

        private void SwapPanels(DockPanel panel)
        {
            foreach (DockPanel panel2 in this.Panels)
            {
                if (!panel2.Equals(panel))
                {
                    panel2.Hide();
                }
            }
            panel.Show();
            panel.BringToFront();
            this.mActivePanel = panel;
        }

        private void TabClick(object sender, MouseEventArgs e)
        {
            TabButton button = sender as TabButton;
            if ((this.ActivePanel != null) && !this.ActivePanel.Equals(button.Panel))
            {
                foreach (TabButton button2 in this.gpgPanelTabs.Controls)
                {
                    if (!button2.Equals(button))
                    {
                        button2.Unselect();
                    }
                }
                this.SwapPanels(button.Panel);
            }
        }

        public void Undock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DockPanel ActivePanel
        {
            get
            {
                return this.mActivePanel;
            }
        }

        public Image BorderImage
        {
            get
            {
                return this.mBorderImage;
            }
            set
            {
                this.mBorderImage = value;
            }
        }

        public Control ContainerControl
        {
            get
            {
                return this.mContainerControl;
            }
            set
            {
                this.mContainerControl = value;
            }
        }

        public DockContainerForm ContainerForm
        {
            get
            {
                return this.mContainerForm;
            }
            set
            {
                this.mContainerForm = value;
            }
        }

        public DockStyles DockStyle
        {
            get
            {
                return DockStyles.Tabbed;
            }
            set
            {
            }
        }

        public SkinManager Manager
        {
            get
            {
                return SkinManager.Docking;
            }
            set
            {
            }
        }

        public TabOrientations Orientation
        {
            get
            {
                return this.mOrientation;
            }
            set
            {
                if (value != this.mOrientation)
                {
                    this.mOrientation = value;
                    this.ArrangePanels();
                }
            }
        }

        public List<DockPanel> Panels
        {
            get
            {
                return this.mPanels;
            }
        }

        public string ResourceKey
        {
            get
            {
                return "TabGroup";
            }
            set
            {
            }
        }

        internal Control RootContainer
        {
            get
            {
                return this.gpgPanelContainer;
            }
        }

        public int TabHeight
        {
            get
            {
                return this.mTabHeight;
            }
        }
    }
}

