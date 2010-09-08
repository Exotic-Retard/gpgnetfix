namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class PnlContentDetailsView : PnlBase
    {
        private int CommentCount;
        private IContainer components;
        internal Control CurrentDetailView;
        private int DeletedComment;
        private SkinGroupPanel gpgGroupBoxMyContentGeneral;
        private GPGLabel gpgLabel1;
        private GPGLabel gpgLabel11;
        private GPGLabel gpgLabel13;
        private GPGLabel gpgLabel15;
        private GPGLabel gpgLabel17;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel3;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel9;
        private GPGLabel gpgLabelContentDesc;
        private GPGLabel gpgLabelContentDownloads;
        private GPGLabel gpgLabelContentName;
        private GPGLabel gpgLabelContentOwnerName;
        private GPGLabel gpgLabelContentVDate;
        private GPGLabel gpgLabelContentVersion;
        private GPGLabel gpgLabelContentVNotes;
        private GPGLabel gpgLabelHeader;
        private GPGLabel gpgLabelNoComments;
        private GPGLabel gpgLabelRateThis;
        private GPGLabel gpgLabelRatingSaved;
        private GPGLabel gpgLabelVoteCount;
        private GPGPanel gpgPanel1;
        private GPGPanel gpgPanel2;
        private GPGPanel gpgPanel3;
        private GPGPanel gpgPanelComments;
        private GPGPanel gpgPanelDependencies;
        private GPGPanel gpgPanelMyContentGeneral;
        private IAdditionalContent mContent;
        private DlgBase mParentDialog;
        private PictureBox pictureBoxContentRating;
        private PictureBox pictureBoxHeader;
        private PictureBox pictureBoxRate1;
        private PictureBox pictureBoxRate2;
        private PictureBox pictureBoxRate3;
        private PictureBox pictureBoxRate4;
        private PictureBox pictureBoxRate5;
        private static Dictionary<int, int> PlayerRatings = null;
        private SkinButton skinButtonChatLink;
        private SkinButton skinButtonDelete;
        private SkinButton skinButtonDeleteRating;
        private SkinButton skinButtonDownload;
        private SkinButton skinButtonExplore;
        private SkinButton skinButtonFlag;
        private SkinButton skinButtonPostCommentBottom;
        private SkinButton skinButtonPostCommentTop;
        private SkinButton skinButtonRun;
        private SkinButton skinButtonUpdate;
        private SkinGroupPanel skinGroupPanel1;
        private const int VPAD = 6;

        static PnlContentDetailsView()
        {
            DataList data = new QuazalQuery("GetPlayerContentRatings", new object[0]).GetData();
            PlayerRatings = new Dictionary<int, int>(data.Count);
            foreach (DataRecord record in data)
            {
                PlayerRatings.Add(int.Parse(record["content_id"]), int.Parse(record["rating"]));
            }
        }

        public PnlContentDetailsView()
        {
            this.components = null;
            this.CurrentDetailView = null;
            this.CommentCount = -1;
            this.DeletedComment = -1;
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public PnlContentDetailsView(DlgBase parentForm)
        {
            this.components = null;
            this.CurrentDetailView = null;
            this.CommentCount = -1;
            this.DeletedComment = -1;
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.mParentDialog = parentForm;
        }

        public PnlContentDetailsView(DlgBase parentForm, IAdditionalContent content)
        {
            this.components = null;
            this.CurrentDetailView = null;
            this.CommentCount = -1;
            this.DeletedComment = -1;
            this.InitializeComponent();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.mParentDialog = parentForm;
            this.BindToMyContent(content);
        }

        private void AdditionalContent_BeginCheckForUpdates(ContentOperationCallbackArgs e)
        {
            if ((!base.Disposing && !base.IsDisposed) && e.CompletedSuccessfully)
            {
                this.RefreshActionButtons();
            }
        }

        private void AdditionalContent_FinishCheckForUpdates(ContentOperationCallbackArgs e)
        {
            if ((!base.Disposing && !base.IsDisposed) && e.CompletedSuccessfully)
            {
                this.RefreshActionButtons();
            }
        }

        private void AdditionalContent_FinishDelete(ContentOperationCallbackArgs e)
        {
            if ((!base.Disposing && !base.IsDisposed) && e.CompletedSuccessfully)
            {
                this.RefreshActionButtons();
            }
        }

        public void BindToMyContent(IAdditionalContent content)
        {
            int num = 6;
            this.mContent = content;
            this.LoadDependencies();
            this.skinButtonRun.Visible = content.CanRun;
            this.gpgGroupBoxMyContentGeneral.Visible = true;
            this.gpgLabelContentDesc.Text = content.Description;
            this.gpgLabelContentDownloads.Text = content.Downloads.ToString();
            this.gpgLabelContentName.Text = content.Name;
            this.gpgLabelContentOwnerName.Text = content.OwnerName;
            this.gpgLabelContentVDate.Text = content.VersionDate.ToShortDateString();
            this.gpgLabelContentVersion.Text = content.Version.ToString();
            this.gpgLabelContentVNotes.Text = content.VersionNotes;
            this.pictureBoxContentRating.Image = content.RatingImageSmall;
            this.SetDownloadsLabel();
            this.pictureBoxHeader.Image = VaultImages.ResourceManager.GetObject("header_" + content.ContentType.Name.ToLower()) as Image;
            this.gpgLabelHeader.Text = content.ContentType.DisplayName;
            this.skinButtonRun.SkinBasePath = content.RunImagePath;
            if (content.CanRun)
            {
                base.ttDefault.SetToolTip(this.skinButtonRun, Loc.Get(content.RunTooltip));
            }
            else
            {
                base.ttDefault.SetToolTip(this.skinButtonRun, "");
            }
            if (this.CurrentDetailView != null)
            {
                this.gpgGroupBoxMyContentGeneral.Controls.Remove(this.CurrentDetailView);
                this.gpgGroupBoxMyContentGeneral.Height -= this.CurrentDetailView.Height + (num * 2);
            }
            ContentOptions detailsView = content.GetDetailsView();
            if (detailsView.HasOptions)
            {
                detailsView.OptionsControl.Top = this.gpgPanelMyContentGeneral.Bottom + num;
                detailsView.OptionsControl.Left = this.gpgPanelMyContentGeneral.Left;
                this.gpgGroupBoxMyContentGeneral.Controls.Add(detailsView.OptionsControl);
                this.gpgGroupBoxMyContentGeneral.Height += detailsView.OptionsControl.Height + (num * 2);
                this.CurrentDetailView = detailsView.OptionsControl;
                if (this.CurrentDetailView.Width > this.gpgPanelMyContentGeneral.Width)
                {
                    this.CurrentDetailView.Width = this.gpgPanelMyContentGeneral.Width;
                }
            }
            this.RefreshActionButtons();
            PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
            for (int i = 0; i < boxArray.Length; i++)
            {
                if (!AdditionalContent.RatingEnabled)
                {
                    boxArray[i].Image = VaultImages.star_gray;
                }
                else if (this.UserRating.HasValue && (i < this.UserRating.Value))
                {
                    boxArray[i].Image = VaultImages.star;
                }
                else
                {
                    boxArray[i].Image = VaultImages.star_empty;
                }
            }
            this.skinButtonDeleteRating.Visible = this.UserRating.HasValue;
            this.CommentCount = -1;
            this.DeletedComment = -1;
            this.UpdateComments();
            this.skinButtonPostCommentTop.Top = this.gpgGroupBoxMyContentGeneral.Bottom + num;
            this.gpgPanelComments.Top = this.skinButtonPostCommentTop.Bottom + num;
            this.gpgLabelNoComments.Top = this.skinButtonPostCommentTop.Bottom + num;
            this.skinButtonPostCommentBottom.Top = this.gpgPanelComments.Bottom + num;
            base.Height = this.skinButtonPostCommentBottom.Bottom;
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
            if ((!base.Disposing && !base.IsDisposed) && e.CompletedSuccessfully)
            {
                this.RefreshActionButtons();
            }
        }

        private void DownloadStarted(ContentOperationCallbackArgs e)
        {
            this.RefreshActionButtons();
        }

        private void gpgLabelContentOwnerName_Click(object sender, EventArgs e)
        {
            Program.MainForm.OnViewPlayerProfile(this.gpgLabelContentOwnerName.Text);
        }

        private void gpgPanelComments_SizeChanged(object sender, EventArgs e)
        {
            this.LayoutComments();
        }

        private void InitializeComponent()
        {
            this.skinButtonDeleteRating = new SkinButton();
            this.skinButtonRun = new SkinButton();
            this.skinButtonExplore = new SkinButton();
            this.pictureBoxRate5 = new PictureBox();
            this.pictureBoxRate4 = new PictureBox();
            this.pictureBoxRate3 = new PictureBox();
            this.pictureBoxRate2 = new PictureBox();
            this.gpgLabel3 = new GPGLabel();
            this.gpgLabelRateThis = new GPGLabel();
            this.gpgLabelRatingSaved = new GPGLabel();
            this.skinButtonDownload = new SkinButton();
            this.skinButtonDelete = new SkinButton();
            this.skinButtonUpdate = new SkinButton();
            this.skinButtonChatLink = new SkinButton();
            this.pictureBoxRate1 = new PictureBox();
            this.gpgPanelComments = new GPGPanel();
            this.skinButtonPostCommentBottom = new SkinButton();
            this.gpgLabelNoComments = new GPGLabel();
            this.skinButtonPostCommentTop = new SkinButton();
            this.gpgLabelHeader = new GPGLabel();
            this.gpgGroupBoxMyContentGeneral = new SkinGroupPanel();
            this.gpgPanelMyContentGeneral = new GPGPanel();
            this.gpgPanelDependencies = new GPGPanel();
            this.gpgLabel1 = new GPGLabel();
            this.gpgPanel3 = new GPGPanel();
            this.gpgLabelContentName = new GPGLabel();
            this.gpgPanel2 = new GPGPanel();
            this.gpgLabelContentDesc = new GPGLabel();
            this.gpgPanel1 = new GPGPanel();
            this.gpgLabelContentVNotes = new GPGLabel();
            this.gpgLabelVoteCount = new GPGLabel();
            this.gpgLabel15 = new GPGLabel();
            this.gpgLabel13 = new GPGLabel();
            this.gpgLabel11 = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.pictureBoxContentRating = new PictureBox();
            this.gpgLabelContentOwnerName = new GPGLabel();
            this.gpgLabel17 = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabel9 = new GPGLabel();
            this.gpgLabelContentDownloads = new GPGLabel();
            this.gpgLabelContentVersion = new GPGLabel();
            this.gpgLabelContentVDate = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.pictureBoxHeader = new PictureBox();
            this.skinGroupPanel1 = new SkinGroupPanel();
            this.skinButtonFlag = new SkinButton();
            ((ISupportInitialize) this.pictureBoxRate5).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate4).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate3).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate2).BeginInit();
            ((ISupportInitialize) this.pictureBoxRate1).BeginInit();
            this.gpgGroupBoxMyContentGeneral.SuspendLayout();
            this.gpgPanelMyContentGeneral.SuspendLayout();
            this.gpgPanel3.SuspendLayout();
            this.gpgPanel2.SuspendLayout();
            this.gpgPanel1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxContentRating).BeginInit();
            ((ISupportInitialize) this.pictureBoxHeader).BeginInit();
            this.skinGroupPanel1.SuspendLayout();
            base.SuspendLayout();
            this.skinButtonDeleteRating.AutoStyle = true;
            this.skinButtonDeleteRating.BackColor = Color.Transparent;
            this.skinButtonDeleteRating.ButtonState = 0;
            this.skinButtonDeleteRating.DialogResult = DialogResult.OK;
            this.skinButtonDeleteRating.DisabledForecolor = Color.Gray;
            this.skinButtonDeleteRating.DrawColor = Color.White;
            this.skinButtonDeleteRating.DrawEdges = false;
            this.skinButtonDeleteRating.FocusColor = Color.Yellow;
            this.skinButtonDeleteRating.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDeleteRating.ForeColor = Color.White;
            this.skinButtonDeleteRating.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonDeleteRating.IsStyled = true;
            this.skinButtonDeleteRating.Location = new Point(0x2b3, 0x2b);
            this.skinButtonDeleteRating.Name = "skinButtonDeleteRating";
            this.skinButtonDeleteRating.Size = new Size(0x18, 0x12);
            this.skinButtonDeleteRating.SkinBasePath = @"Dialog\ContentManager\BtnDeleteRating";
            this.skinButtonDeleteRating.TabIndex = 13;
            this.skinButtonDeleteRating.TabStop = true;
            this.skinButtonDeleteRating.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDeleteRating.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonDeleteRating, "<LOC>Delete Rating");
            this.skinButtonDeleteRating.Click += new EventHandler(this.skinButtonDeleteRating_Click);
            this.skinButtonRun.AutoStyle = true;
            this.skinButtonRun.BackColor = Color.Black;
            this.skinButtonRun.ButtonState = 0;
            this.skinButtonRun.CausesValidation = false;
            this.skinButtonRun.DialogResult = DialogResult.OK;
            this.skinButtonRun.DisabledForecolor = Color.Gray;
            this.skinButtonRun.DrawColor = Color.White;
            this.skinButtonRun.DrawEdges = false;
            this.skinButtonRun.FocusColor = Color.Yellow;
            this.skinButtonRun.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonRun.ForeColor = Color.White;
            this.skinButtonRun.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonRun.IsStyled = true;
            this.skinButtonRun.Location = new Point(0xfe, 0x1d);
            this.skinButtonRun.Margin = new Padding(0);
            this.skinButtonRun.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonRun.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonRun.Name = "skinButtonRun";
            this.skinButtonRun.Size = new Size(0x25, 0x25);
            this.skinButtonRun.SkinBasePath = @"Dialog\ContentManager\BtnPlay";
            this.skinButtonRun.TabIndex = 12;
            this.skinButtonRun.TabStop = true;
            this.skinButtonRun.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonRun.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonRun, "<LOC>Launch Content");
            this.skinButtonRun.Click += new EventHandler(this.skinButtonRun_Click);
            this.skinButtonExplore.AutoStyle = true;
            this.skinButtonExplore.BackColor = Color.Black;
            this.skinButtonExplore.ButtonState = 0;
            this.skinButtonExplore.CausesValidation = false;
            this.skinButtonExplore.DialogResult = DialogResult.OK;
            this.skinButtonExplore.DisabledForecolor = Color.Gray;
            this.skinButtonExplore.DrawColor = Color.White;
            this.skinButtonExplore.DrawEdges = false;
            this.skinButtonExplore.FocusColor = Color.Yellow;
            this.skinButtonExplore.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonExplore.ForeColor = Color.White;
            this.skinButtonExplore.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonExplore.IsStyled = true;
            this.skinButtonExplore.Location = new Point(0xd6, 0x1d);
            this.skinButtonExplore.Margin = new Padding(0);
            this.skinButtonExplore.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonExplore.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonExplore.Name = "skinButtonExplore";
            this.skinButtonExplore.Size = new Size(0x25, 0x25);
            this.skinButtonExplore.SkinBasePath = @"Dialog\ContentManager\BtnBrowseTo";
            this.skinButtonExplore.TabIndex = 11;
            this.skinButtonExplore.TabStop = true;
            this.skinButtonExplore.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonExplore.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonExplore, "<LOC>View in Windows Explorer");
            this.skinButtonExplore.Click += new EventHandler(this.skinButtonExplore_Click);
            this.pictureBoxRate5.Location = new Point(800, 0x2a);
            this.pictureBoxRate5.Margin = new Padding(0);
            this.pictureBoxRate5.Name = "pictureBoxRate5";
            this.pictureBoxRate5.Size = new Size(20, 20);
            this.pictureBoxRate5.TabIndex = 10;
            this.pictureBoxRate5.TabStop = false;
            this.pictureBoxRate5.Tag = "5";
            this.pictureBoxRate5.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate5.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate5.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.pictureBoxRate4.Location = new Point(0x30d, 0x2a);
            this.pictureBoxRate4.Margin = new Padding(0);
            this.pictureBoxRate4.Name = "pictureBoxRate4";
            this.pictureBoxRate4.Size = new Size(20, 20);
            this.pictureBoxRate4.TabIndex = 9;
            this.pictureBoxRate4.TabStop = false;
            this.pictureBoxRate4.Tag = "4";
            this.pictureBoxRate4.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate4.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate4.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.pictureBoxRate3.Location = new Point(0x2fa, 0x2a);
            this.pictureBoxRate3.Margin = new Padding(0);
            this.pictureBoxRate3.Name = "pictureBoxRate3";
            this.pictureBoxRate3.Size = new Size(20, 20);
            this.pictureBoxRate3.TabIndex = 8;
            this.pictureBoxRate3.TabStop = false;
            this.pictureBoxRate3.Tag = "3";
            this.pictureBoxRate3.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate3.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate3.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.pictureBoxRate2.Location = new Point(0x2e7, 0x2a);
            this.pictureBoxRate2.Margin = new Padding(0);
            this.pictureBoxRate2.Name = "pictureBoxRate2";
            this.pictureBoxRate2.Size = new Size(20, 20);
            this.pictureBoxRate2.TabIndex = 7;
            this.pictureBoxRate2.TabStop = false;
            this.pictureBoxRate2.Tag = "2";
            this.pictureBoxRate2.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate2.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate2.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.gpgLabel3.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel3.AutoStyle = true;
            this.gpgLabel3.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel3.Font = new Font("Arial", 9.75f);
            this.gpgLabel3.ForeColor = Color.White;
            this.gpgLabel3.IgnoreMouseWheel = false;
            this.gpgLabel3.IsStyled = false;
            this.gpgLabel3.Location = new Point(0x1b6, 0x18);
            this.gpgLabel3.Name = "gpgLabel3";
            this.gpgLabel3.Size = new Size(2, 0x2d);
            this.gpgLabel3.TabIndex = 5;
            this.gpgLabel3.TextStyle = TextStyles.Custom;
            this.gpgLabelRateThis.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelRateThis.AutoSize = true;
            this.gpgLabelRateThis.AutoStyle = true;
            this.gpgLabelRateThis.Font = new Font("Arial", 9.75f);
            this.gpgLabelRateThis.ForeColor = Color.White;
            this.gpgLabelRateThis.IgnoreMouseWheel = false;
            this.gpgLabelRateThis.IsStyled = false;
            this.gpgLabelRateThis.Location = new Point(0x1d2, 0x2b);
            this.gpgLabelRateThis.Name = "gpgLabelRateThis";
            this.gpgLabelRateThis.Size = new Size(0x6b, 0x10);
            this.gpgLabelRateThis.TabIndex = 4;
            this.gpgLabelRateThis.Text = "Rate this content";
            this.gpgLabelRateThis.TextStyle = TextStyles.Bold;
            this.gpgLabelRatingSaved.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelRatingSaved.AutoStyle = true;
            this.gpgLabelRatingSaved.Font = new Font("Arial", 9.75f);
            this.gpgLabelRatingSaved.ForeColor = Color.White;
            this.gpgLabelRatingSaved.IgnoreMouseWheel = false;
            this.gpgLabelRatingSaved.IsStyled = false;
            this.gpgLabelRatingSaved.Location = new Point(0x263, 0x18);
            this.gpgLabelRatingSaved.Name = "gpgLabelRatingSaved";
            this.gpgLabelRatingSaved.Size = new Size(0xd3, 0x10);
            this.gpgLabelRatingSaved.TabIndex = 3;
            this.gpgLabelRatingSaved.Text = "<LOC>Your rating has been saved.";
            this.gpgLabelRatingSaved.TextAlign = ContentAlignment.MiddleRight;
            this.gpgLabelRatingSaved.TextStyle = TextStyles.Small;
            this.gpgLabelRatingSaved.Visible = false;
            this.skinButtonDownload.AutoStyle = true;
            this.skinButtonDownload.BackColor = Color.Black;
            this.skinButtonDownload.ButtonState = 0;
            this.skinButtonDownload.CausesValidation = false;
            this.skinButtonDownload.DialogResult = DialogResult.OK;
            this.skinButtonDownload.DisabledForecolor = Color.Gray;
            this.skinButtonDownload.DrawColor = Color.White;
            this.skinButtonDownload.DrawEdges = false;
            this.skinButtonDownload.FocusColor = Color.Yellow;
            this.skinButtonDownload.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDownload.ForeColor = Color.White;
            this.skinButtonDownload.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonDownload.IsStyled = true;
            this.skinButtonDownload.Location = new Point(0xae, 0x1d);
            this.skinButtonDownload.Margin = new Padding(0);
            this.skinButtonDownload.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonDownload.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonDownload.Name = "skinButtonDownload";
            this.skinButtonDownload.Size = new Size(0x25, 0x25);
            this.skinButtonDownload.SkinBasePath = @"Dialog\ContentManager\BtnDownload";
            this.skinButtonDownload.TabIndex = 2;
            this.skinButtonDownload.TabStop = true;
            this.skinButtonDownload.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDownload.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonDownload, "<LOC>Download");
            this.skinButtonDownload.Click += new EventHandler(this.skinButtonDownload_Click);
            this.skinButtonDownload.SizeChanged += new EventHandler(this.ResizeActionButton);
            this.skinButtonDelete.AutoStyle = true;
            this.skinButtonDelete.BackColor = Color.Black;
            this.skinButtonDelete.ButtonState = 0;
            this.skinButtonDelete.CausesValidation = false;
            this.skinButtonDelete.DialogResult = DialogResult.OK;
            this.skinButtonDelete.DisabledForecolor = Color.Gray;
            this.skinButtonDelete.DrawColor = Color.White;
            this.skinButtonDelete.DrawEdges = false;
            this.skinButtonDelete.FocusColor = Color.Yellow;
            this.skinButtonDelete.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonDelete.ForeColor = Color.White;
            this.skinButtonDelete.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonDelete.IsStyled = true;
            this.skinButtonDelete.Location = new Point(0x86, 0x1d);
            this.skinButtonDelete.Margin = new Padding(0);
            this.skinButtonDelete.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonDelete.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonDelete.Name = "skinButtonDelete";
            this.skinButtonDelete.Size = new Size(0x25, 0x25);
            this.skinButtonDelete.SkinBasePath = @"Dialog\ContentManager\BtnDelete";
            this.skinButtonDelete.TabIndex = 2;
            this.skinButtonDelete.TabStop = true;
            this.skinButtonDelete.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonDelete.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonDelete, "<LOC>Delete");
            this.skinButtonDelete.Click += new EventHandler(this.skinButtonDelete_Click);
            this.skinButtonDelete.SizeChanged += new EventHandler(this.ResizeActionButton);
            this.skinButtonUpdate.AutoStyle = true;
            this.skinButtonUpdate.BackColor = Color.Black;
            this.skinButtonUpdate.ButtonState = 0;
            this.skinButtonUpdate.CausesValidation = false;
            this.skinButtonUpdate.DialogResult = DialogResult.OK;
            this.skinButtonUpdate.DisabledForecolor = Color.Gray;
            this.skinButtonUpdate.DrawColor = Color.White;
            this.skinButtonUpdate.DrawEdges = false;
            this.skinButtonUpdate.FocusColor = Color.Yellow;
            this.skinButtonUpdate.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonUpdate.ForeColor = Color.White;
            this.skinButtonUpdate.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonUpdate.IsStyled = true;
            this.skinButtonUpdate.Location = new Point(0x5e, 0x1d);
            this.skinButtonUpdate.Margin = new Padding(0);
            this.skinButtonUpdate.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonUpdate.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonUpdate.Name = "skinButtonUpdate";
            this.skinButtonUpdate.Size = new Size(0x25, 0x25);
            this.skinButtonUpdate.SkinBasePath = @"Dialog\ContentManager\BtnUpdate";
            this.skinButtonUpdate.TabIndex = 2;
            this.skinButtonUpdate.TabStop = true;
            this.skinButtonUpdate.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonUpdate.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonUpdate, "<LOC>Look for Updates");
            this.skinButtonUpdate.Click += new EventHandler(this.skinButtonUpdate_Click);
            this.skinButtonUpdate.SizeChanged += new EventHandler(this.ResizeActionButton);
            this.skinButtonChatLink.AutoStyle = true;
            this.skinButtonChatLink.BackColor = Color.Black;
            this.skinButtonChatLink.ButtonState = 0;
            this.skinButtonChatLink.CausesValidation = false;
            this.skinButtonChatLink.DialogResult = DialogResult.OK;
            this.skinButtonChatLink.DisabledForecolor = Color.Gray;
            this.skinButtonChatLink.DrawColor = Color.White;
            this.skinButtonChatLink.DrawEdges = false;
            this.skinButtonChatLink.FocusColor = Color.Yellow;
            this.skinButtonChatLink.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonChatLink.ForeColor = Color.White;
            this.skinButtonChatLink.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonChatLink.IsStyled = true;
            this.skinButtonChatLink.Location = new Point(14, 0x1d);
            this.skinButtonChatLink.Margin = new Padding(0);
            this.skinButtonChatLink.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonChatLink.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonChatLink.Name = "skinButtonChatLink";
            this.skinButtonChatLink.Size = new Size(0x25, 0x25);
            this.skinButtonChatLink.SkinBasePath = @"Dialog\ContentManager\BtnChatLink";
            this.skinButtonChatLink.TabIndex = 1;
            this.skinButtonChatLink.TabStop = true;
            this.skinButtonChatLink.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonChatLink.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonChatLink, "<LOC>Link to Chat");
            this.skinButtonChatLink.Click += new EventHandler(this.skinButtonChatLink_Click);
            this.skinButtonChatLink.SizeChanged += new EventHandler(this.ResizeActionButton);
            this.pictureBoxRate1.Location = new Point(0x2d4, 0x2a);
            this.pictureBoxRate1.Margin = new Padding(0);
            this.pictureBoxRate1.Name = "pictureBoxRate1";
            this.pictureBoxRate1.Size = new Size(20, 20);
            this.pictureBoxRate1.TabIndex = 0;
            this.pictureBoxRate1.TabStop = false;
            this.pictureBoxRate1.Tag = "1";
            this.pictureBoxRate1.MouseLeave += new EventHandler(this.StarMouseLeave);
            this.pictureBoxRate1.Click += new EventHandler(this.StarMouseClick);
            this.pictureBoxRate1.MouseEnter += new EventHandler(this.StarMouseEnter);
            this.gpgPanelComments.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgPanelComments.BackColor = Color.Transparent;
            this.gpgPanelComments.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelComments.BorderThickness = 2;
            this.gpgPanelComments.DrawBorder = false;
            this.gpgPanelComments.Location = new Point(3, 0x18e);
            this.gpgPanelComments.Name = "gpgPanelComments";
            this.gpgPanelComments.Size = new Size(0x362, 0x10);
            this.gpgPanelComments.TabIndex = 3;
            this.skinButtonPostCommentBottom.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.skinButtonPostCommentBottom.AutoStyle = true;
            this.skinButtonPostCommentBottom.BackColor = Color.Black;
            this.skinButtonPostCommentBottom.ButtonState = 0;
            this.skinButtonPostCommentBottom.DialogResult = DialogResult.OK;
            this.skinButtonPostCommentBottom.DisabledForecolor = Color.Gray;
            this.skinButtonPostCommentBottom.DrawColor = Color.White;
            this.skinButtonPostCommentBottom.DrawEdges = true;
            this.skinButtonPostCommentBottom.FocusColor = Color.Yellow;
            this.skinButtonPostCommentBottom.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonPostCommentBottom.ForeColor = Color.White;
            this.skinButtonPostCommentBottom.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonPostCommentBottom.IsStyled = true;
            this.skinButtonPostCommentBottom.Location = new Point(3, 0x1a6);
            this.skinButtonPostCommentBottom.Name = "skinButtonPostCommentBottom";
            this.skinButtonPostCommentBottom.Size = new Size(0x90, 0x18);
            this.skinButtonPostCommentBottom.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonPostCommentBottom.TabIndex = 2;
            this.skinButtonPostCommentBottom.TabStop = true;
            this.skinButtonPostCommentBottom.Text = "<LOC>Post a Comment";
            this.skinButtonPostCommentBottom.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonPostCommentBottom.TextPadding = new Padding(0);
            this.skinButtonPostCommentBottom.Visible = false;
            this.skinButtonPostCommentBottom.Click += new EventHandler(this.PostCommentClick);
            this.gpgLabelNoComments.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelNoComments.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNoComments.AutoStyle = true;
            this.gpgLabelNoComments.BackColor = Color.Transparent;
            this.gpgLabelNoComments.Font = new Font("Arial", 9.75f);
            this.gpgLabelNoComments.ForeColor = Color.White;
            this.gpgLabelNoComments.IgnoreMouseWheel = false;
            this.gpgLabelNoComments.IsStyled = false;
            this.gpgLabelNoComments.Location = new Point(1, 0x1a1);
            this.gpgLabelNoComments.Name = "gpgLabelNoComments";
            this.gpgLabelNoComments.Size = new Size(0x35f, 0x17);
            this.gpgLabelNoComments.TabIndex = 1;
            this.gpgLabelNoComments.Text = "<LOC>No comments, be the first to post one!";
            this.gpgLabelNoComments.TextAlign = ContentAlignment.TopCenter;
            this.gpgLabelNoComments.TextStyle = TextStyles.Default;
            this.skinButtonPostCommentTop.AutoStyle = true;
            this.skinButtonPostCommentTop.BackColor = Color.Black;
            this.skinButtonPostCommentTop.ButtonState = 0;
            this.skinButtonPostCommentTop.DialogResult = DialogResult.OK;
            this.skinButtonPostCommentTop.DisabledForecolor = Color.Gray;
            this.skinButtonPostCommentTop.DrawColor = Color.White;
            this.skinButtonPostCommentTop.DrawEdges = true;
            this.skinButtonPostCommentTop.FocusColor = Color.Yellow;
            this.skinButtonPostCommentTop.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonPostCommentTop.ForeColor = Color.White;
            this.skinButtonPostCommentTop.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonPostCommentTop.IsStyled = true;
            this.skinButtonPostCommentTop.Location = new Point(3, 0x16d);
            this.skinButtonPostCommentTop.Name = "skinButtonPostCommentTop";
            this.skinButtonPostCommentTop.Size = new Size(0x90, 0x18);
            this.skinButtonPostCommentTop.SkinBasePath = @"Controls\Button\Round Edge";
            this.skinButtonPostCommentTop.TabIndex = 0;
            this.skinButtonPostCommentTop.TabStop = true;
            this.skinButtonPostCommentTop.Text = "<LOC>Post a Comment";
            this.skinButtonPostCommentTop.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonPostCommentTop.TextPadding = new Padding(0);
            this.skinButtonPostCommentTop.Click += new EventHandler(this.PostCommentClick);
            this.gpgLabelHeader.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelHeader.AutoSize = true;
            this.gpgLabelHeader.AutoStyle = true;
            this.gpgLabelHeader.BackColor = Color.Transparent;
            this.gpgLabelHeader.Font = new Font("Verdana", 14f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabelHeader.ForeColor = Color.White;
            this.gpgLabelHeader.IgnoreMouseWheel = false;
            this.gpgLabelHeader.IsStyled = false;
            this.gpgLabelHeader.Location = new Point(8, 12);
            this.gpgLabelHeader.Name = "gpgLabelHeader";
            this.gpgLabelHeader.Size = new Size(0x79, 0x17);
            this.gpgLabelHeader.TabIndex = 5;
            this.gpgLabelHeader.Text = "gpgLabel1";
            this.gpgLabelHeader.TextStyle = TextStyles.Custom;
            this.gpgGroupBoxMyContentGeneral.AutoStyle = false;
            this.gpgGroupBoxMyContentGeneral.BackColor = Color.Black;
            this.gpgGroupBoxMyContentGeneral.Controls.Add(this.gpgPanelMyContentGeneral);
            this.gpgGroupBoxMyContentGeneral.CutCorner = false;
            this.gpgGroupBoxMyContentGeneral.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgGroupBoxMyContentGeneral.HeaderImage = GroupPanelImages.blue_gradient;
            this.gpgGroupBoxMyContentGeneral.IsStyled = true;
            this.gpgGroupBoxMyContentGeneral.Location = new Point(0, 0x8e);
            this.gpgGroupBoxMyContentGeneral.Margin = new Padding(4, 3, 4, 3);
            this.gpgGroupBoxMyContentGeneral.Name = "gpgGroupBoxMyContentGeneral";
            this.gpgGroupBoxMyContentGeneral.Size = new Size(0x36b, 0xd9);
            this.gpgGroupBoxMyContentGeneral.TabIndex = 6;
            this.gpgGroupBoxMyContentGeneral.Text = "<LOC>General Info";
            this.gpgGroupBoxMyContentGeneral.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgGroupBoxMyContentGeneral.TextPadding = new Padding(8, 0, 0, 0);
            this.gpgPanelMyContentGeneral.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelMyContentGeneral.BorderThickness = 2;
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgPanelDependencies);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel1);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgPanel3);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgPanel2);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgPanel1);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabelVoteCount);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel15);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel13);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel11);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel5);
            this.gpgPanelMyContentGeneral.Controls.Add(this.pictureBoxContentRating);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabelContentOwnerName);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel17);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel7);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel9);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabelContentDownloads);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabelContentVersion);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabelContentVDate);
            this.gpgPanelMyContentGeneral.Controls.Add(this.gpgLabel2);
            this.gpgPanelMyContentGeneral.DrawBorder = false;
            this.gpgPanelMyContentGeneral.Location = new Point(2, 0x16);
            this.gpgPanelMyContentGeneral.Name = "gpgPanelMyContentGeneral";
            this.gpgPanelMyContentGeneral.Size = new Size(0x365, 190);
            this.gpgPanelMyContentGeneral.TabIndex = 0x15;
            this.gpgPanelDependencies.AutoScroll = true;
            this.gpgPanelDependencies.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanelDependencies.BorderThickness = 2;
            this.gpgPanelDependencies.DrawBorder = false;
            this.gpgPanelDependencies.Location = new Point(3, 0x8e);
            this.gpgPanelDependencies.Name = "gpgPanelDependencies";
            this.gpgPanelDependencies.Size = new Size(0x362, 0x30);
            this.gpgPanelDependencies.TabIndex = 0x19;
            this.gpgLabel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel1.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(3, 0x7a);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0x361, 0x11);
            this.gpgLabel1.TabIndex = 0x18;
            this.gpgLabel1.Text = "<LOC>Dependencies";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel1.TextStyle = TextStyles.Custom;
            this.gpgPanel3.AutoScroll = true;
            this.gpgPanel3.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel3.BorderThickness = 2;
            this.gpgPanel3.Controls.Add(this.gpgLabelContentName);
            this.gpgPanel3.DrawBorder = false;
            this.gpgPanel3.Location = new Point(3, 0x16);
            this.gpgPanel3.Name = "gpgPanel3";
            this.gpgPanel3.Size = new Size(0xc5, 0x1a);
            this.gpgPanel3.TabIndex = 0x17;
            this.gpgLabelContentName.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelContentName.AutoStyle = true;
            this.gpgLabelContentName.Font = new Font("Arial", 9.75f);
            this.gpgLabelContentName.ForeColor = Color.White;
            this.gpgLabelContentName.IgnoreMouseWheel = false;
            this.gpgLabelContentName.IsStyled = false;
            this.gpgLabelContentName.Location = new Point(0, 0);
            this.gpgLabelContentName.Name = "gpgLabelContentName";
            this.gpgLabelContentName.Size = new Size(0xb1, 15);
            this.gpgLabelContentName.TabIndex = 1;
            this.gpgLabelContentName.Text = "gpgLabel4";
            this.gpgLabelContentName.TextStyle = TextStyles.Default;
            this.gpgPanel2.AutoScroll = true;
            this.gpgPanel2.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel2.BorderThickness = 2;
            this.gpgPanel2.Controls.Add(this.gpgLabelContentDesc);
            this.gpgPanel2.DrawBorder = false;
            this.gpgPanel2.Location = new Point(3, 0x4a);
            this.gpgPanel2.Name = "gpgPanel2";
            this.gpgPanel2.Size = new Size(0x1a8, 0x2d);
            this.gpgPanel2.TabIndex = 0x16;
            this.gpgLabelContentDesc.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabelContentDesc.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelContentDesc.AutoStyle = true;
            this.gpgLabelContentDesc.Font = new Font("Arial", 9.75f);
            this.gpgLabelContentDesc.ForeColor = Color.White;
            this.gpgLabelContentDesc.IgnoreMouseWheel = false;
            this.gpgLabelContentDesc.IsStyled = false;
            this.gpgLabelContentDesc.Location = new Point(0, 0);
            this.gpgLabelContentDesc.Name = "gpgLabelContentDesc";
            this.gpgLabelContentDesc.Size = new Size(0x194, 15);
            this.gpgLabelContentDesc.TabIndex = 0x12;
            this.gpgLabelContentDesc.Text = "gpgLabel3";
            this.gpgLabelContentDesc.TextStyle = TextStyles.Default;
            this.gpgPanel1.AutoScroll = true;
            this.gpgPanel1.BorderColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.gpgPanel1.BorderThickness = 2;
            this.gpgPanel1.Controls.Add(this.gpgLabelContentVNotes);
            this.gpgPanel1.DrawBorder = false;
            this.gpgPanel1.Location = new Point(0x1bc, 0x4a);
            this.gpgPanel1.Name = "gpgPanel1";
            this.gpgPanel1.Size = new Size(0x1a8, 0x2d);
            this.gpgPanel1.TabIndex = 0x15;
            this.gpgLabelContentVNotes.AutoGrowDirection = GrowDirections.Vertical;
            this.gpgLabelContentVNotes.AutoStyle = true;
            this.gpgLabelContentVNotes.Font = new Font("Arial", 9.75f);
            this.gpgLabelContentVNotes.ForeColor = Color.White;
            this.gpgLabelContentVNotes.IgnoreMouseWheel = false;
            this.gpgLabelContentVNotes.IsStyled = false;
            this.gpgLabelContentVNotes.Location = new Point(0, 0);
            this.gpgLabelContentVNotes.Name = "gpgLabelContentVNotes";
            this.gpgLabelContentVNotes.Size = new Size(0x194, 15);
            this.gpgLabelContentVNotes.TabIndex = 0x13;
            this.gpgLabelContentVNotes.Text = "gpgLabel4";
            this.gpgLabelContentVNotes.TextStyle = TextStyles.Default;
            this.gpgLabelVoteCount.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelVoteCount.AutoStyle = true;
            this.gpgLabelVoteCount.Font = new Font("Arial", 9.75f);
            this.gpgLabelVoteCount.ForeColor = Color.White;
            this.gpgLabelVoteCount.IgnoreMouseWheel = false;
            this.gpgLabelVoteCount.IsStyled = false;
            this.gpgLabelVoteCount.Location = new Point(0x2e9, 0x23);
            this.gpgLabelVoteCount.Name = "gpgLabelVoteCount";
            this.gpgLabelVoteCount.Size = new Size(0x6d, 0x10);
            this.gpgLabelVoteCount.TabIndex = 20;
            this.gpgLabelVoteCount.Text = "Based on n votes";
            this.gpgLabelVoteCount.TextStyle = TextStyles.Small;
            this.gpgLabel15.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel15.AutoStyle = true;
            this.gpgLabel15.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel15.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel15.ForeColor = Color.White;
            this.gpgLabel15.IgnoreMouseWheel = false;
            this.gpgLabel15.IsStyled = false;
            this.gpgLabel15.Location = new Point(0x2ea, 0);
            this.gpgLabel15.Name = "gpgLabel15";
            this.gpgLabel15.Size = new Size(0x72, 0x11);
            this.gpgLabel15.TabIndex = 12;
            this.gpgLabel15.Text = "<LOC>Rating";
            this.gpgLabel15.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel15.TextStyle = TextStyles.Custom;
            this.gpgLabel13.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel13.AutoStyle = true;
            this.gpgLabel13.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel13.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel13.ForeColor = Color.White;
            this.gpgLabel13.IgnoreMouseWheel = false;
            this.gpgLabel13.IsStyled = false;
            this.gpgLabel13.Location = new Point(0x260, 0);
            this.gpgLabel13.Name = "gpgLabel13";
            this.gpgLabel13.Size = new Size(0x85, 0x11);
            this.gpgLabel13.TabIndex = 10;
            this.gpgLabel13.Text = "<LOC>Downloads";
            this.gpgLabel13.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel13.TextStyle = TextStyles.Custom;
            this.gpgLabel11.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel11.AutoStyle = true;
            this.gpgLabel11.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel11.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel11.ForeColor = Color.White;
            this.gpgLabel11.IgnoreMouseWheel = false;
            this.gpgLabel11.IsStyled = false;
            this.gpgLabel11.Location = new Point(0x1ba, 0);
            this.gpgLabel11.Name = "gpgLabel11";
            this.gpgLabel11.Size = new Size(0x7f, 0x11);
            this.gpgLabel11.TabIndex = 8;
            this.gpgLabel11.Text = "<LOC>Version Date";
            this.gpgLabel11.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel11.TextStyle = TextStyles.Custom;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel5.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0xca, -5);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x6f, 0x16);
            this.gpgLabel5.TabIndex = 2;
            this.gpgLabel5.Text = "<LOC>Owner";
            this.gpgLabel5.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel5.TextStyle = TextStyles.Custom;
            this.pictureBoxContentRating.Location = new Point(0x2ec, 0x16);
            this.pictureBoxContentRating.Name = "pictureBoxContentRating";
            this.pictureBoxContentRating.Size = new Size(0x34, 10);
            this.pictureBoxContentRating.TabIndex = 0x11;
            this.pictureBoxContentRating.TabStop = false;
            this.gpgLabelContentOwnerName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelContentOwnerName.AutoSize = true;
            this.gpgLabelContentOwnerName.AutoStyle = true;
            this.gpgLabelContentOwnerName.Font = new Font("Arial", 9.75f);
            this.gpgLabelContentOwnerName.ForeColor = Color.White;
            this.gpgLabelContentOwnerName.IgnoreMouseWheel = false;
            this.gpgLabelContentOwnerName.IsStyled = false;
            this.gpgLabelContentOwnerName.Location = new Point(0xc9, 20);
            this.gpgLabelContentOwnerName.Name = "gpgLabelContentOwnerName";
            this.gpgLabelContentOwnerName.Size = new Size(0x43, 0x10);
            this.gpgLabelContentOwnerName.TabIndex = 3;
            this.gpgLabelContentOwnerName.Text = "gpgLabel4";
            this.gpgLabelContentOwnerName.TextStyle = TextStyles.Link;
            this.gpgLabelContentOwnerName.Click += new EventHandler(this.gpgLabelContentOwnerName_Click);
            this.gpgLabel17.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel17.AutoStyle = true;
            this.gpgLabel17.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel17.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel17.ForeColor = Color.White;
            this.gpgLabel17.IgnoreMouseWheel = false;
            this.gpgLabel17.IsStyled = false;
            this.gpgLabel17.Location = new Point(0x1ba, 0x33);
            this.gpgLabel17.Name = "gpgLabel17";
            this.gpgLabel17.Size = new Size(0xc0, 0x11);
            this.gpgLabel17.TabIndex = 14;
            this.gpgLabel17.Text = "<LOC>Version Notes";
            this.gpgLabel17.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel17.TextStyle = TextStyles.Custom;
            this.gpgLabel7.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel7.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(4, 0x33);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x361, 0x11);
            this.gpgLabel7.TabIndex = 4;
            this.gpgLabel7.Text = "<LOC>Description";
            this.gpgLabel7.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel7.TextStyle = TextStyles.Custom;
            this.gpgLabel9.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel9.AutoStyle = true;
            this.gpgLabel9.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel9.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel9.ForeColor = Color.White;
            this.gpgLabel9.IgnoreMouseWheel = false;
            this.gpgLabel9.IsStyled = false;
            this.gpgLabel9.Location = new Point(0x151, 0);
            this.gpgLabel9.Name = "gpgLabel9";
            this.gpgLabel9.Size = new Size(0x63, 0x11);
            this.gpgLabel9.TabIndex = 6;
            this.gpgLabel9.Text = "<LOC>Version";
            this.gpgLabel9.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel9.TextStyle = TextStyles.Custom;
            this.gpgLabelContentDownloads.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelContentDownloads.AutoSize = true;
            this.gpgLabelContentDownloads.AutoStyle = true;
            this.gpgLabelContentDownloads.Font = new Font("Arial", 9.75f);
            this.gpgLabelContentDownloads.ForeColor = Color.White;
            this.gpgLabelContentDownloads.IgnoreMouseWheel = false;
            this.gpgLabelContentDownloads.IsStyled = false;
            this.gpgLabelContentDownloads.Location = new Point(0x25f, 20);
            this.gpgLabelContentDownloads.Name = "gpgLabelContentDownloads";
            this.gpgLabelContentDownloads.Size = new Size(0x43, 0x10);
            this.gpgLabelContentDownloads.TabIndex = 11;
            this.gpgLabelContentDownloads.Text = "gpgLabel4";
            this.gpgLabelContentDownloads.TextStyle = TextStyles.Default;
            this.gpgLabelContentVersion.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelContentVersion.AutoSize = true;
            this.gpgLabelContentVersion.AutoStyle = true;
            this.gpgLabelContentVersion.Font = new Font("Arial", 9.75f);
            this.gpgLabelContentVersion.ForeColor = Color.White;
            this.gpgLabelContentVersion.IgnoreMouseWheel = false;
            this.gpgLabelContentVersion.IsStyled = false;
            this.gpgLabelContentVersion.Location = new Point(0x150, 20);
            this.gpgLabelContentVersion.Name = "gpgLabelContentVersion";
            this.gpgLabelContentVersion.Size = new Size(0x43, 0x10);
            this.gpgLabelContentVersion.TabIndex = 7;
            this.gpgLabelContentVersion.Text = "gpgLabel4";
            this.gpgLabelContentVersion.TextStyle = TextStyles.Default;
            this.gpgLabelContentVDate.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelContentVDate.AutoSize = true;
            this.gpgLabelContentVDate.AutoStyle = true;
            this.gpgLabelContentVDate.Font = new Font("Arial", 9.75f);
            this.gpgLabelContentVDate.ForeColor = Color.White;
            this.gpgLabelContentVDate.IgnoreMouseWheel = false;
            this.gpgLabelContentVDate.IsStyled = false;
            this.gpgLabelContentVDate.Location = new Point(0x1b9, 20);
            this.gpgLabelContentVDate.Name = "gpgLabelContentVDate";
            this.gpgLabelContentVDate.Size = new Size(0x43, 0x10);
            this.gpgLabelContentVDate.TabIndex = 9;
            this.gpgLabelContentVDate.Text = "gpgLabel4";
            this.gpgLabelContentVDate.TextStyle = TextStyles.Default;
            this.gpgLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.gpgLabel2.Font = new Font("Verdana", 6.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(4, -5);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x361, 0x16);
            this.gpgLabel2.TabIndex = 0;
            this.gpgLabel2.Text = "<LOC>Name";
            this.gpgLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.gpgLabel2.TextStyle = TextStyles.Custom;
            this.pictureBoxHeader.Location = new Point(0, 0);
            this.pictureBoxHeader.Name = "pictureBoxHeader";
            this.pictureBoxHeader.Size = new Size(600, 0x19);
            this.pictureBoxHeader.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxHeader.TabIndex = 4;
            this.pictureBoxHeader.TabStop = false;
            this.skinGroupPanel1.AutoStyle = false;
            this.skinGroupPanel1.BackColor = Color.Black;
            this.skinGroupPanel1.Controls.Add(this.skinButtonFlag);
            this.skinGroupPanel1.Controls.Add(this.skinButtonDeleteRating);
            this.skinGroupPanel1.Controls.Add(this.skinButtonRun);
            this.skinGroupPanel1.Controls.Add(this.skinButtonChatLink);
            this.skinGroupPanel1.Controls.Add(this.skinButtonExplore);
            this.skinGroupPanel1.Controls.Add(this.pictureBoxRate1);
            this.skinGroupPanel1.Controls.Add(this.pictureBoxRate5);
            this.skinGroupPanel1.Controls.Add(this.skinButtonUpdate);
            this.skinGroupPanel1.Controls.Add(this.pictureBoxRate4);
            this.skinGroupPanel1.Controls.Add(this.skinButtonDelete);
            this.skinGroupPanel1.Controls.Add(this.pictureBoxRate3);
            this.skinGroupPanel1.Controls.Add(this.skinButtonDownload);
            this.skinGroupPanel1.Controls.Add(this.pictureBoxRate2);
            this.skinGroupPanel1.Controls.Add(this.gpgLabelRatingSaved);
            this.skinGroupPanel1.Controls.Add(this.gpgLabel3);
            this.skinGroupPanel1.Controls.Add(this.gpgLabelRateThis);
            this.skinGroupPanel1.CutCorner = false;
            this.skinGroupPanel1.Font = new Font("Verdana", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.skinGroupPanel1.HeaderImage = GroupPanelImages.blue_gradient;
            this.skinGroupPanel1.IsStyled = true;
            this.skinGroupPanel1.Location = new Point(0, 0x3a);
            this.skinGroupPanel1.Margin = new Padding(4, 3, 4, 3);
            this.skinGroupPanel1.Name = "skinGroupPanel1";
            this.skinGroupPanel1.Size = new Size(0x36b, 0x4e);
            this.skinGroupPanel1.TabIndex = 7;
            this.skinGroupPanel1.Text = "<LOC>File Tasks";
            this.skinGroupPanel1.TextAlign = ContentAlignment.MiddleLeft;
            this.skinGroupPanel1.TextPadding = new Padding(8, 0, 0, 0);
            this.skinButtonFlag.AutoStyle = true;
            this.skinButtonFlag.BackColor = Color.Black;
            this.skinButtonFlag.ButtonState = 0;
            this.skinButtonFlag.CausesValidation = false;
            this.skinButtonFlag.DialogResult = DialogResult.OK;
            this.skinButtonFlag.DisabledForecolor = Color.Gray;
            this.skinButtonFlag.DrawColor = Color.White;
            this.skinButtonFlag.DrawEdges = false;
            this.skinButtonFlag.FocusColor = Color.Yellow;
            this.skinButtonFlag.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonFlag.ForeColor = Color.White;
            this.skinButtonFlag.HorizontalScalingMode = ScalingModes.Center;
            this.skinButtonFlag.IsStyled = true;
            this.skinButtonFlag.Location = new Point(0x36, 0x1d);
            this.skinButtonFlag.Margin = new Padding(0);
            this.skinButtonFlag.MaximumSize = new Size(0x25, 0x25);
            this.skinButtonFlag.MinimumSize = new Size(0x25, 0x25);
            this.skinButtonFlag.Name = "skinButtonFlag";
            this.skinButtonFlag.Size = new Size(0x25, 0x25);
            this.skinButtonFlag.SkinBasePath = @"Dialog\ContentManager\BtnFlag";
            this.skinButtonFlag.TabIndex = 2;
            this.skinButtonFlag.TabStop = true;
            this.skinButtonFlag.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonFlag.TextPadding = new Padding(0);
            base.ttDefault.SetToolTip(this.skinButtonFlag, "<LOC id=_533a096c4d416becb438669a9f139e29>Flag this content as broken or inappropriate to be reviewed for further action");
            this.skinButtonFlag.Click += new EventHandler(this.skinButtonFlag_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            base.Controls.Add(this.skinGroupPanel1);
            base.Controls.Add(this.gpgGroupBoxMyContentGeneral);
            base.Controls.Add(this.gpgPanelComments);
            base.Controls.Add(this.gpgLabelHeader);
            base.Controls.Add(this.skinButtonPostCommentBottom);
            base.Controls.Add(this.pictureBoxHeader);
            base.Controls.Add(this.gpgLabelNoComments);
            base.Controls.Add(this.skinButtonPostCommentTop);
            this.MinimumSize = new Size(0x36b, 330);
            base.Name = "PnlContentDetailsView";
            base.Size = new Size(0x36b, 700);
            ((ISupportInitialize) this.pictureBoxRate5).EndInit();
            ((ISupportInitialize) this.pictureBoxRate4).EndInit();
            ((ISupportInitialize) this.pictureBoxRate3).EndInit();
            ((ISupportInitialize) this.pictureBoxRate2).EndInit();
            ((ISupportInitialize) this.pictureBoxRate1).EndInit();
            this.gpgGroupBoxMyContentGeneral.ResumeLayout(false);
            this.gpgPanelMyContentGeneral.ResumeLayout(false);
            this.gpgPanelMyContentGeneral.PerformLayout();
            this.gpgPanel3.ResumeLayout(false);
            this.gpgPanel2.ResumeLayout(false);
            this.gpgPanel1.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxContentRating).EndInit();
            ((ISupportInitialize) this.pictureBoxHeader).EndInit();
            this.skinGroupPanel1.ResumeLayout(false);
            this.skinGroupPanel1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LayoutComments()
        {
            if (this.gpgPanelComments != null)
            {
                int num = 6;
                foreach (PnlContentComment comment in this.gpgPanelComments.Controls)
                {
                    comment.ResizeToComment();
                    comment.Width = this.gpgPanelComments.ClientSize.Width;
                    comment.Top = num;
                    comment.BringToFront();
                    num += comment.Height + 6;
                }
                this.gpgPanelComments.Height = num;
                this.skinButtonPostCommentTop.Top = this.gpgGroupBoxMyContentGeneral.Bottom + 6;
                this.gpgPanelComments.Top = this.skinButtonPostCommentTop.Bottom + 6;
                this.gpgLabelNoComments.Top = this.skinButtonPostCommentTop.Bottom + 6;
                this.skinButtonPostCommentBottom.Top = this.gpgPanelComments.Bottom + 6;
                base.Height = this.skinButtonPostCommentBottom.Bottom;
            }
        }

        private void LoadDependencies()
        {
            this.gpgPanelDependencies.Controls.Clear();
            if ((this.Content.ContentDependencies != null) && (this.Content.ContentDependencies.Length >= 1))
            {
                ThreadPool.QueueUserWorkItem(delegate (object state) {
                    try
                    {
                        Dictionary<int, IAdditionalContent> dictionary = new Dictionary<int, IAdditionalContent>(AdditionalContent.MyContent.Count);
                        foreach (IAdditionalContent content in AdditionalContent.MyContent)
                        {
                            dictionary.Add(content.ID, content);
                        }
                        int top = 0;
                        foreach (int dependency in this.Content.ContentDependencies)
                        {
                            DependencyState dependencyState;
                            IAdditionalContent dependentContent;
                            if (dictionary.ContainsKey(dependency))
                            {
                                dependencyState = DependencyState.FoundLocal;
                                dependentContent = dictionary[dependency];
                            }
                            else
                            {
                                dependentContent = AdditionalContent.LookupDependency(dependency);
                                if (dependentContent != null)
                                {
                                    dependencyState = DependencyState.NotLocal;
                                }
                                else
                                {
                                    dependencyState = DependencyState.NotInVault;
                                }
                            }
                            base.Invoke(delegate {
                                GPGLabel label = new GPGLabel {
                                    AutoSize = true
                                };
                                if (dependencyState == DependencyState.FoundLocal)
                                {
                                    label.Text = dependentContent.Name;
                                    label.TextStyle = TextStyles.Default;
                                }
                                else if (dependencyState == DependencyState.NotLocal)
                                {
                                    label.Text = string.Format("Click here to download dependency: {0}", dependentContent.Name);
                                    label.TextStyle = TextStyles.Link;
                                    label.Tag = dependency;
                                    label.Click += delegate (object sender, EventArgs e1) {
                                        AdditionalContent.DownloadContent((int) (sender as Control).Tag);
                                    };
                                }
                                else
                                {
                                    label.Text = string.Format("Dependency not found in vault, ID: {0}", dependency);
                                    label.TextStyle = TextStyles.ColoredBold;
                                    label.ForeColor = Color.Red;
                                }
                                label.Left = 0;
                                label.Top = top;
                                this.gpgPanelDependencies.Controls.Add(label);
                                top += label.Height;
                            });
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                });
            }
        }

        protected override void Localize()
        {
            base.Localize();
        }

        protected override void OnLoad(EventArgs e)
        {
            AccessControlList list;
            EventHandler handler = null;
            base.OnLoad(e);
            this.pictureBoxHeader.Controls.Add(this.gpgLabelHeader);
            AdditionalContent.BeginDownloadContent += new ContentOperationCallback(this.DownloadStarted);
            AdditionalContent.FinishDownloadContent += new ContentOperationCallback(this.DownloadCompleted);
            AdditionalContent.FinishDeleteMyContent += new ContentOperationCallback(this.AdditionalContent_FinishDelete);
            AdditionalContent.BeginCheckForUpdates += new ContentOperationCallback(this.AdditionalContent_BeginCheckForUpdates);
            AdditionalContent.FinishCheckForUpdates += new ContentOperationCallback(this.AdditionalContent_FinishCheckForUpdates);
            if (User.Current.IsAdmin || (AccessControlList.GetByName("ContentAdmins", out list) && list.HasAccess()))
            {
                this.gpgLabelHeader.Cursor = Cursors.Hand;
                if (handler == null)
                {
                    handler = delegate (object s, EventArgs e1) {
                        new DlgContentAdmin(this.Content).ShowDialog();
                    };
                }
                this.gpgLabelHeader.Click += handler;
            }
        }

        private void PostCommentClick(object sender, EventArgs e)
        {
            DlgPostComment comment = new DlgPostComment();
            if ((comment.ShowDialog() == DialogResult.OK) && DataAccess.ExecuteQuery("PostContentComment", new object[] { this.Content.ID, comment.Comment }))
            {
                this.CommentCount++;
                this.UpdateComments();
            }
        }

        private void RefreshActionButtons()
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.RefreshActionButtons();
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.skinButtonChatLink.Width = 0x25;
                this.skinButtonDelete.Width = 0x25;
                this.skinButtonDownload.Width = 0x25;
                this.skinButtonUpdate.Width = 0x25;
                this.skinButtonChatLink.Enabled = true;
                this.skinButtonDelete.Enabled = AdditionalContent.DownloadExistsLocal(this.Content) || AdditionalContent.MyContentExists(this.Content);
                this.skinButtonDownload.Enabled = ((this.Content.CurrentUserCanDownload && AdditionalContent.DownloadsEnabled) && (!AdditionalContent.CheckingforUpdates && !AdditionalContent.IsDownloadingContent(this.Content))) && !AdditionalContent.DownloadExistsLocal(this.Content);
                this.skinButtonUpdate.Enabled = (((this.Content.CurrentUserCanDownload && this.Content.CanVersion) && (!AdditionalContent.CheckingforUpdates && AdditionalContent.DownloadsEnabled)) && !AdditionalContent.IsDownloadingContent(this.Content)) && AdditionalContent.DownloadExistsLocal(this.Content);
                this.skinButtonPostCommentTop.Enabled = AdditionalContent.CommentsEnabled;
                this.skinButtonPostCommentBottom.Enabled = AdditionalContent.CommentsEnabled;
                this.skinButtonRun.Enabled = (this.Content.CanRun && Directory.Exists(this.Content.GetDownloadPath())) && ((!AdditionalContent.IsDownloadingContent(this.Content) || (AdditionalContent.DownloadTarget == null)) || (AdditionalContent.DownloadTarget.ID != this.Content.ID));
                this.skinButtonExplore.Enabled = Directory.Exists(this.Content.GetDownloadPath());
            }
        }

        private void ResizeActionButton(object sender, EventArgs e)
        {
            (sender as Control).Width = 0x25;
        }

        private void SetDownloadsLabel()
        {
            if (this.Content.RatingCount > 0)
            {
                base.ttDefault.SetToolTip(this.pictureBoxContentRating, this.Content.Rating.ToString());
                this.gpgLabelVoteCount.Text = string.Format(Loc.Get("<LOC>Based on {0} votes"), this.Content.RatingCount);
                this.gpgLabelVoteCount.Visible = true;
            }
            else
            {
                base.ttDefault.SetToolTip(this.pictureBoxContentRating, "");
                this.gpgLabelVoteCount.Visible = false;
            }
        }

        private void skinButtonChatLink_Click(object sender, EventArgs e)
        {
            if (Chatroom.InChatroom)
            {
                DialogResult result;
                string str = DlgAskQuestion.AskQuestion(base.MainForm, "<LOC>Enter chat a message to go along with this chat link.", "<LOC>Chat Message", false, out result);
                if (result == DialogResult.OK)
                {
                    if ((str != null) && (str.Length > 0))
                    {
                        base.MainForm.SendMessage(string.Format("{0} : {1}", str, ContentLinkMask.FormatLink(this.Content)));
                    }
                    else
                    {
                        base.MainForm.SendMessage(ContentLinkMask.FormatLink(this.Content));
                    }
                }
            }
            else
            {
                base.MainForm.ErrorMessage("<LOC>You are not in a chatroom, you must join a chatroom to share this replay link. Click here to join chat:'{0}'", new object[] { base.MainForm.MainChatroom });
            }
        }

        private void skinButtonDelete_Click(object sender, EventArgs e)
        {
            if (new DlgYesNo(base.MainForm, "<LOC>Confirm", string.Format("<LOC>Do you really want to delete this {0} from your content?", this.Content.ContentType.SingularDisplayName)).ShowDialog() == DialogResult.Yes)
            {
                AdditionalContent.DeleteMyContent(this.Content, false);
            }
        }

        private void skinButtonDeleteRating_Click(object sender, EventArgs e)
        {
            if (AdditionalContent.RatingEnabled)
            {
                int? userRating = this.UserRating;
                if (userRating.HasValue)
                {
                    IAdditionalContent content = this.Content;
                    content.RatingCount--;
                    IAdditionalContent content2 = this.Content;
                    userRating = this.UserRating;
                    content2.RatingTotal -= userRating.Value;
                }
                this.UserRating = null;
                this.pictureBoxContentRating.Image = this.Content.RatingImageSmall;
                this.SetDownloadsLabel();
                this.skinButtonDeleteRating.Visible = false;
                this.gpgLabelRatingSaved.Visible = false;
                PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
                for (int i = 0; i < boxArray.Length; i++)
                {
                    boxArray[i].Image = VaultImages.star_empty;
                }
            }
        }

        private void skinButtonDownload_Click(object sender, EventArgs e)
        {
            AdditionalContent.DownloadContent(this.Content, this.ParentDialog);
        }

        private void skinButtonExplore_Click(object sender, EventArgs e)
        {
            AdditionalContent.ViewInExplorer(this.Content);
        }

        private void skinButtonFlag_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string str = DlgAskQuestion.AskQuestion(base.MainForm, "<LOC>Please enter a description as to why you are flagging this content for review.", "<LOC>Enter Description", false, out result);
            if (result == DialogResult.OK)
            {
                if (new QuazalQuery("FlagContent", new object[] { this.Content.ID, str }).ExecuteNonQuery())
                {
                    DlgMessage.ShowDialog("<LOC>Content has been flagged and will be reviewed for further action.");
                }
                else
                {
                    DlgMessage.ShowDialog("<LOC>You have already flagged this content.");
                }
                this.skinButtonFlag.Enabled = false;
            }
        }

        private void skinButtonRun_Click(object sender, EventArgs e)
        {
            if (this.Content.CanRun && Directory.Exists(this.Content.GetDownloadPath()))
            {
                this.Content.Run();
            }
        }

        private void skinButtonUpdate_Click(object sender, EventArgs e)
        {
            AdditionalContent.CheckForUpdates(this.Content, this.ParentDialog);
        }

        private void StarMouseClick(object sender, EventArgs e)
        {
            if (AdditionalContent.RatingEnabled)
            {
                int num = Convert.ToInt32((sender as Control).Tag);
                if (!this.UserRating.HasValue || (this.UserRating.Value != num))
                {
                    if (this.UserRating.HasValue)
                    {
                        IAdditionalContent content = this.Content;
                        content.RatingTotal += num - this.UserRating.Value;
                    }
                    else
                    {
                        IAdditionalContent content2 = this.Content;
                        content2.RatingCount++;
                        IAdditionalContent content3 = this.Content;
                        content3.RatingTotal += num;
                    }
                    this.UserRating = new int?(num);
                    this.pictureBoxContentRating.Image = this.Content.RatingImageSmall;
                    this.SetDownloadsLabel();
                    this.skinButtonDeleteRating.Visible = true;
                    this.gpgLabelRatingSaved.Visible = true;
                }
            }
        }

        private void StarMouseEnter(object sender, EventArgs e)
        {
            if (AdditionalContent.RatingEnabled)
            {
                PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
                int num = Convert.ToInt32((sender as Control).Tag);
                if (this.UserRating.HasValue && (this.UserRating.Value == num))
                {
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
                for (int i = 0; i < boxArray.Length; i++)
                {
                    if (i < num)
                    {
                        boxArray[i].Image = VaultImages.star;
                    }
                    else
                    {
                        boxArray[i].Image = VaultImages.star_empty;
                    }
                }
            }
        }

        private void StarMouseLeave(object sender, EventArgs e)
        {
            if (AdditionalContent.RatingEnabled)
            {
                PictureBox[] boxArray = new PictureBox[] { this.pictureBoxRate1, this.pictureBoxRate2, this.pictureBoxRate3, this.pictureBoxRate4, this.pictureBoxRate5 };
                for (int i = 0; i < boxArray.Length; i++)
                {
                    if (this.UserRating.HasValue && (i < this.UserRating.Value))
                    {
                        boxArray[i].Image = VaultImages.star;
                    }
                    else
                    {
                        boxArray[i].Image = VaultImages.star_empty;
                    }
                }
            }
        }

        private void UpdateComments()
        {
            WaitCallback callBack = null;
            MappedObjectList<ContentComment> comments = new QuazalQuery("GetContentComments", new object[] { this.Content.ID }).GetObjects<ContentComment>();
            if ((this.CommentCount > -1) && (comments.Count != this.CommentCount))
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        VGen0 method = null;
                        try
                        {
                            int num = 0;
                            do
                            {
                                Thread.Sleep(0x3e8);
                                num++;
                                comments = new QuazalQuery("GetContentComments", new object[] { this.Content.ID }).GetObjects<ContentComment>();
                            }
                            while ((comments.Count != this.CommentCount) && (num < 2));
                            if (comments.Count == this.CommentCount)
                            {
                                if (method == null)
                                {
                                    method = delegate {
                                        this.UpdateComments(comments);
                                    };
                                }
                                this.BeginInvoke(method);
                            }
                            else
                            {
                                ErrorLog.WriteLine("Did insert on master db and failed to get propogated data after 3000ms.", new object[0]);
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                            ErrorLog.WriteLine("Did insert on master db and failed to get propogated data after 3000ms.", new object[0]);
                        }
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
            else
            {
                this.CommentCount = comments.Count;
                this.UpdateComments(comments);
            }
        }

        private void UpdateComments(MappedObjectList<ContentComment> comments)
        {
            EventHandler handler = null;
            EventHandler handler2 = null;
            try
            {
                this.gpgPanelComments.Controls.Clear();
                this.gpgPanelComments.Refresh();
                bool flag = false;
                if (comments.Count > 0)
                {
                    this.gpgLabelNoComments.Visible = false;
                    this.gpgPanelComments.Visible = true;
                    foreach (ContentComment comment in comments)
                    {
                        if ((this.DeletedComment <= 0) || (comment.ID != this.DeletedComment))
                        {
                            PnlContentComment comment2 = new PnlContentComment(this.Content, comment);
                            this.gpgPanelComments.Controls.Add(comment2);
                            if (handler == null)
                            {
                                handler = delegate (object s, EventArgs e) {
                                    this.DeletedComment = (s as ContentComment).ID;
                                    this.CommentCount--;
                                    this.UpdateComments();
                                };
                            }
                            comment2.DeleteComment += handler;
                            if (handler2 == null)
                            {
                                handler2 = delegate (object s, EventArgs e) {
                                    this.LayoutComments();
                                };
                            }
                            comment2.UpdateComment += handler2;
                            flag = true;
                        }
                    }
                    this.LayoutComments();
                }
                else
                {
                    this.gpgPanelComments.Visible = false;
                    this.skinButtonPostCommentBottom.Visible = false;
                    this.gpgLabelNoComments.Visible = true;
                    this.gpgLabelNoComments.BringToFront();
                }
                if (!flag)
                {
                    this.gpgPanelComments.Visible = false;
                    this.gpgLabelNoComments.Visible = true;
                    this.gpgLabelNoComments.BringToFront();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
            finally
            {
                this.DeletedComment = -1;
            }
        }

        public IAdditionalContent Content
        {
            get
            {
                return this.mContent;
            }
        }

        public DlgBase ParentDialog
        {
            get
            {
                return this.mParentDialog;
            }
        }

        protected override bool ScaleChildren
        {
            get
            {
                return false;
            }
        }

        public int? UserRating
        {
            get
            {
                if (PlayerRatings.ContainsKey(this.Content.ID))
                {
                    return new int?(PlayerRatings[this.Content.ID]);
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    new QuazalQuery("RateContent", new object[] { this.Content.ID, value.Value }).ExecuteNonQueryAsync();
                    PlayerRatings[this.Content.ID] = value.Value;
                }
                else
                {
                    new QuazalQuery("DeleteContentRating", new object[] { this.Content.ID }).ExecuteNonQueryAsync();
                    if (PlayerRatings.ContainsKey(this.Content.ID))
                    {
                        PlayerRatings.Remove(this.Content.ID);
                    }
                }
            }
        }
    }
}

