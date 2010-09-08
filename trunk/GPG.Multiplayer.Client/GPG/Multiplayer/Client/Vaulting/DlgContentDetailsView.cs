namespace GPG.Multiplayer.Client.Vaulting
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games.SupremeCommander;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class DlgContentDetailsView : DlgBase
    {
        private static Dictionary<int, DlgContentDetailsView> ActiveViews = new Dictionary<int, DlgContentDetailsView>();
        private IContainer components = null;
        private GPGPanel gpgPanelContainer;
        private IAdditionalContent mContent;
        private PnlContentDetailsView mDetailView;

        public event EventHandler OnDownloadComplete;

        public DlgContentDetailsView(IAdditionalContent content)
        {
            this.InitializeComponent();
            this.Text = string.Format(Loc.Get("<LOC>{0} Details"), content.ContentType.SingularDisplayName);
            this.mContent = content;
            this.mDetailView = new PnlContentDetailsView(this);
            this.mDetailView.Location = new Point(4, 4);
            this.gpgPanelContainer.Controls.Add(this.DetailView);
            this.mDetailView.BindToMyContent(this.Content);
            ActiveViews[this.Content.ID] = this;
        }

        private void AdditionalContent_FinishDelete(ContentOperationCallbackArgs e)
        {
        }

        public static DlgContentDetailsView CreateOrGetExisting(IAdditionalContent content)
        {
            if (content == null)
            {
                return null;
            }
            if (ActiveViews.ContainsKey(content.ID))
            {
                return ActiveViews[content.ID];
            }
            return new DlgContentDetailsView(content);
        }

        public static DlgContentDetailsView CreateOrGetExisting(int contentId)
        {
            if (ActiveViews.ContainsKey(contentId))
            {
                return ActiveViews[contentId];
            }
            return new DlgContentDetailsView(AdditionalContent.GetByID(contentId));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DownloadCompleted(ContentOperationCallbackArgs e)
        {
            base.SetStatus("<LOC>Download Complete.", 0xbb8, new object[0]);
            try
            {
                SupcomMapList.RefreshMaps();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            if (this.OnDownloadComplete != null)
            {
                this.OnDownloadComplete(this, EventArgs.Empty);
            }
        }

        private void DownloadStarted(ContentOperationCallbackArgs e)
        {
            base.SetStatus("<LOC>Downloading {0}...", 0xbb8, new object[] { e.Content.ContentType.SingularDisplayName });
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgContentDetailsView));
            this.gpgPanelContainer = new GPGPanel();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x358, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgPanelContainer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPanelContainer.AutoScroll = true;
            this.gpgPanelContainer.BackgroundImage = (Image) manager.GetObject("gpgPanelContainer.BackgroundImage");
            this.gpgPanelContainer.Location = new Point(8, 0x47);
            this.gpgPanelContainer.Name = "gpgPanelContainer";
            this.gpgPanelContainer.Size = new Size(0x385, 0x1ed);
            base.ttDefault.SetSuperTip(this.gpgPanelContainer, null);
            this.gpgPanelContainer.TabIndex = 7;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x393, 600);
            base.Controls.Add(this.gpgPanelContainer);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(400, 300);
            base.Name = "DlgContentDetailsView";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "DlgContentDetailsView";
            base.Controls.SetChildIndex(this.gpgPanelContainer, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (ActiveViews.ContainsKey(this.Content.ID))
            {
                ActiveViews.Remove(this.Content.ID);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            AdditionalContent.BeginDownloadContent += new ContentOperationCallback(this.DownloadStarted);
            AdditionalContent.FinishDownloadContent += new ContentOperationCallback(this.DownloadCompleted);
            AdditionalContent.FinishDeleteMyContent += new ContentOperationCallback(this.AdditionalContent_FinishDelete);
            base.OnLoad(e);
        }

        public override bool AllowMultipleInstances
        {
            get
            {
                return true;
            }
        }

        public override bool AllowRestoreWindow
        {
            get
            {
                return false;
            }
        }

        public IAdditionalContent Content
        {
            get
            {
                return this.mContent;
            }
        }

        public PnlContentDetailsView DetailView
        {
            get
            {
                return this.mDetailView;
            }
        }

        protected override bool RememberLayout
        {
            get
            {
                return false;
            }
        }
    }
}

