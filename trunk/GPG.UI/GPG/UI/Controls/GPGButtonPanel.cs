namespace GPG.UI.Controls
{
    using GPG.UI.Controls.Skinning;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGButtonPanel : Control, ISkinResourceConsumer
    {
        private IContainer components;
        private Image mBackgroundImage;
        private List<PanelButton> mButtons = new List<PanelButton>();
        private SkinManager mManager = SkinManager.Default;

        public GPGButtonPanel()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public void AddButton(PanelButton btn)
        {
            this.mButtons.Add(btn);
        }

        public void AddButton(Image img, EventHandler onClick)
        {
            PanelButton btn = new PanelButton(img);
            btn.Click += onClick;
            this.AddButton(btn);
        }

        public void AddButton(Image img, string tooltip, EventHandler onClick)
        {
            PanelButton btn = new PanelButton(img, tooltip);
            btn.Click += onClick;
            this.AddButton(btn);
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

        protected override void OnPaint(PaintEventArgs pe)
        {
            using (List<PanelButton>.Enumerator enumerator = this.Buttons.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    PanelButton current = enumerator.Current;
                }
            }
        }

        public void RefreshSkin()
        {
            this.Refresh();
        }

        public Image BackgroundImage
        {
            get
            {
                return this.mBackgroundImage;
            }
            set
            {
                this.mBackgroundImage = value;
            }
        }

        public List<PanelButton> Buttons
        {
            get
            {
                return this.mButtons;
            }
        }

        public SkinManager Manager
        {
            get
            {
                return this.mManager;
            }
            set
            {
                this.mManager = value;
            }
        }

        public string ResourceKey
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}

