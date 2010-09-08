namespace GPG.Multiplayer.Client.Security
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal.Security;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgManageACLs : DlgBase
    {
        public ToolStripMenuItem btnMore;
        public ToolStripMenuItem btnNewACL;
        private IContainer components = null;
        private bool FirstPaint = true;
        private GPGContextMenu gpgContextMenuACL;
        private GPGLabel gpgLabel2;
        private GPGLabel gpgLabel4;
        private GPGLabel gpgLabel5;
        private GPGLabel gpgLabel6;
        private GPGLabel gpgLabel7;
        private GPGLabel gpgLabel8;
        private GPGLabel gpgLabelACLAccessLevel;
        private GPGLabel gpgLabelACLDescription;
        private GPGLabel gpgLabelACLInclusionType;
        private GPGLabel gpgLabelACLListType;
        private GPGLabel gpgLabelACLName;
        private GPGLabel gpgLabelACLQuery;
        private GPGLabel gpgLabelNoSelection;
        private GPGPanel gpgPanelACL;
        private List<ToolStripItem> mCustomPaint = new List<ToolStripItem>();
        private MenuItem miACL_AddMember;
        private MenuItem miACL_Delete;
        private GPGMenuStrip msQuickButtons;
        private Color PanelBackColor = Color.FromArgb(0x33, 0x33, 0x33);
        private AccessControlList SelectedACL = null;
        private AccessControlListMember SelectedMember = null;
        private SkinButton skinButtonAddMember;
        private SkinLabel skinLabel2;
        private Color SplitColor = Color.FromArgb(0xcc, 0xcc, 0xff);
        private SplitContainer splitContainerMain;
        private bool ToolstripSizeChanged = true;
        private TreeView treeViewACL;

        public DlgManageACLs()
        {
            this.InitializeComponent();
            this.InitializeToolstrip();
        }

        private void AddACL(AccessControlList acl)
        {
            VGen0 method = null;
            TreeNode aclNode = new TreeNode(acl.Name, 0, 0);
            aclNode.Name = acl.Name;
            aclNode.Tag = acl;
            foreach (AccessControlListMember member in acl.Members)
            {
                TreeNode node = new TreeNode(member.MemberName, 1, 1);
                node.Tag = member;
                aclNode.Nodes.Add(node);
            }
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.treeViewACL.Nodes.Add(aclNode);
                    };
                }
                base.Invoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.treeViewACL.Nodes.Add(aclNode);
            }
        }

        private void BindToSelectedACL()
        {
            if (this.SelectedACL != null)
            {
                this.gpgLabelACLAccessLevel.Text = this.SelectedACL.AccessLevel.ToString();
                this.gpgLabelACLDescription.Text = this.SelectedACL.Description;
                this.gpgLabelACLInclusionType.Text = this.SelectedACL.InclusionType.ToString();
                this.gpgLabelACLListType.Text = this.SelectedACL.ListType.ToString();
                this.gpgLabelACLName.Text = this.SelectedACL.Name;
                if (this.SelectedACL.ListType == AccessControlListTypes.QueriedList)
                {
                    this.gpgLabelACLQuery.Text = this.SelectedACL.Tag;
                }
                else
                {
                    this.gpgLabelACLQuery.Text = "No query associated";
                }
                this.gpgPanelACL.BringToFront();
            }
        }

        private void BindToSelectedMember()
        {
        }

        private void btnNewACL_Click(object sender, EventArgs e)
        {
            DlgCreateNewACL wacl = new DlgCreateNewACL();
            if (wacl.ShowDialog() == DialogResult.OK)
            {
                this.AddACL(wacl.ACL);
            }
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgManageACLs));
            this.treeViewACL = new TreeView();
            this.splitContainerMain = new SplitContainer();
            this.skinLabel2 = new SkinLabel();
            this.gpgPanelACL = new GPGPanel();
            this.skinButtonAddMember = new SkinButton();
            this.gpgLabelACLDescription = new GPGLabel();
            this.gpgLabel8 = new GPGLabel();
            this.gpgLabelACLQuery = new GPGLabel();
            this.gpgLabel7 = new GPGLabel();
            this.gpgLabelACLAccessLevel = new GPGLabel();
            this.gpgLabel6 = new GPGLabel();
            this.gpgLabelACLInclusionType = new GPGLabel();
            this.gpgLabel5 = new GPGLabel();
            this.gpgLabelACLListType = new GPGLabel();
            this.gpgLabel4 = new GPGLabel();
            this.gpgLabelACLName = new GPGLabel();
            this.gpgLabel2 = new GPGLabel();
            this.gpgLabelNoSelection = new GPGLabel();
            this.msQuickButtons = new GPGMenuStrip(this.components);
            this.btnNewACL = new ToolStripMenuItem();
            this.btnMore = new ToolStripMenuItem();
            this.gpgContextMenuACL = new GPGContextMenu();
            this.miACL_Delete = new MenuItem();
            this.miACL_AddMember = new MenuItem();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.gpgPanelACL.SuspendLayout();
            this.msQuickButtons.SuspendLayout();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x240, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.treeViewACL.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeViewACL.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewACL.BorderStyle = BorderStyle.None;
            this.treeViewACL.Location = new Point(0, 20);
            this.treeViewACL.Name = "treeViewACL";
            this.treeViewACL.Size = new Size(0xd1, 370);
            base.ttDefault.SetSuperTip(this.treeViewACL, null);
            this.treeViewACL.TabIndex = 7;
            this.treeViewACL.AfterSelect += new TreeViewEventHandler(this.treeViewACL_AfterSelect);
            this.treeViewACL.MouseUp += new MouseEventHandler(this.treeViewACL_MouseUp);
            this.splitContainerMain.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.splitContainerMain.BackColor = Color.FromArgb(0xcc, 0xcc, 0xff);
            this.splitContainerMain.Location = new Point(6, 60);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Panel1.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerMain.Panel1.Controls.Add(this.skinLabel2);
            this.splitContainerMain.Panel1.Controls.Add(this.treeViewACL);
            base.ttDefault.SetSuperTip(this.splitContainerMain.Panel1, null);
            this.splitContainerMain.Panel2.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.splitContainerMain.Panel2.Controls.Add(this.gpgPanelACL);
            this.splitContainerMain.Panel2.Controls.Add(this.gpgLabelNoSelection);
            base.ttDefault.SetSuperTip(this.splitContainerMain.Panel2, null);
            this.splitContainerMain.Size = new Size(0x26e, 390);
            this.splitContainerMain.SplitterDistance = 0xce;
            base.ttDefault.SetSuperTip(this.splitContainerMain, null);
            this.splitContainerMain.TabIndex = 9;
            this.skinLabel2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.skinLabel2.AutoStyle = false;
            this.skinLabel2.BackColor = Color.Transparent;
            this.skinLabel2.DrawEdges = true;
            this.skinLabel2.Font = new Font("Verdana", 10f, FontStyle.Bold);
            this.skinLabel2.ForeColor = Color.White;
            this.skinLabel2.HorizontalScalingMode = ScalingModes.Tile;
            this.skinLabel2.IsStyled = false;
            this.skinLabel2.Location = new Point(0, 0);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new Size(0xd1, 20);
            this.skinLabel2.SkinBasePath = @"Controls\Background Label\BlueGradient";
            base.ttDefault.SetSuperTip(this.skinLabel2, null);
            this.skinLabel2.TabIndex = 0x11;
            this.skinLabel2.Text = "Access Control Explorer";
            this.skinLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.skinLabel2.TextPadding = new Padding(0);
            this.gpgPanelACL.BackColor = Color.Black;
            this.gpgPanelACL.Controls.Add(this.skinButtonAddMember);
            this.gpgPanelACL.Controls.Add(this.gpgLabelACLDescription);
            this.gpgPanelACL.Controls.Add(this.gpgLabel8);
            this.gpgPanelACL.Controls.Add(this.gpgLabelACLQuery);
            this.gpgPanelACL.Controls.Add(this.gpgLabel7);
            this.gpgPanelACL.Controls.Add(this.gpgLabelACLAccessLevel);
            this.gpgPanelACL.Controls.Add(this.gpgLabel6);
            this.gpgPanelACL.Controls.Add(this.gpgLabelACLInclusionType);
            this.gpgPanelACL.Controls.Add(this.gpgLabel5);
            this.gpgPanelACL.Controls.Add(this.gpgLabelACLListType);
            this.gpgPanelACL.Controls.Add(this.gpgLabel4);
            this.gpgPanelACL.Controls.Add(this.gpgLabelACLName);
            this.gpgPanelACL.Controls.Add(this.gpgLabel2);
            this.gpgPanelACL.Dock = DockStyle.Fill;
            this.gpgPanelACL.Location = new Point(0, 0);
            this.gpgPanelACL.Name = "gpgPanelACL";
            this.gpgPanelACL.Size = new Size(0x19c, 390);
            base.ttDefault.SetSuperTip(this.gpgPanelACL, null);
            this.gpgPanelACL.TabIndex = 1;
            this.skinButtonAddMember.AutoStyle = true;
            this.skinButtonAddMember.BackColor = Color.Transparent;
            this.skinButtonAddMember.ButtonState = 0;
            this.skinButtonAddMember.DialogResult = DialogResult.OK;
            this.skinButtonAddMember.DisabledForecolor = Color.Gray;
            this.skinButtonAddMember.DrawColor = Color.White;
            this.skinButtonAddMember.DrawEdges = true;
            this.skinButtonAddMember.FocusColor = Color.Yellow;
            this.skinButtonAddMember.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonAddMember.ForeColor = Color.White;
            this.skinButtonAddMember.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonAddMember.IsStyled = true;
            this.skinButtonAddMember.Location = new Point(0x19, 0x158);
            this.skinButtonAddMember.Name = "skinButtonAddMember";
            this.skinButtonAddMember.Size = new Size(0x7d, 0x1a);
            this.skinButtonAddMember.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonAddMember, null);
            this.skinButtonAddMember.TabIndex = 12;
            this.skinButtonAddMember.TabStop = true;
            this.skinButtonAddMember.Text = "Add Member";
            this.skinButtonAddMember.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonAddMember.TextPadding = new Padding(0);
            this.skinButtonAddMember.Click += new EventHandler(this.skinButtonAddMember_Click);
            this.gpgLabelACLDescription.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelACLDescription.AutoSize = true;
            this.gpgLabelACLDescription.AutoStyle = true;
            this.gpgLabelACLDescription.Font = new Font("Arial", 9.75f);
            this.gpgLabelACLDescription.ForeColor = Color.White;
            this.gpgLabelACLDescription.IgnoreMouseWheel = false;
            this.gpgLabelACLDescription.IsStyled = false;
            this.gpgLabelACLDescription.Location = new Point(0x16, 0x51);
            this.gpgLabelACLDescription.Name = "gpgLabelACLDescription";
            this.gpgLabelACLDescription.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelACLDescription, null);
            this.gpgLabelACLDescription.TabIndex = 11;
            this.gpgLabelACLDescription.Text = "gpgLabel3";
            this.gpgLabelACLDescription.TextStyle = TextStyles.Default;
            this.gpgLabel8.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel8.AutoSize = true;
            this.gpgLabel8.AutoStyle = true;
            this.gpgLabel8.Font = new Font("Arial", 9.75f);
            this.gpgLabel8.ForeColor = Color.White;
            this.gpgLabel8.IgnoreMouseWheel = false;
            this.gpgLabel8.IsStyled = false;
            this.gpgLabel8.Location = new Point(0x16, 0x41);
            this.gpgLabel8.Name = "gpgLabel8";
            this.gpgLabel8.Size = new Size(0x49, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel8, null);
            this.gpgLabel8.TabIndex = 10;
            this.gpgLabel8.Text = "Description";
            this.gpgLabel8.TextStyle = TextStyles.Bold;
            this.gpgLabelACLQuery.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelACLQuery.AutoSize = true;
            this.gpgLabelACLQuery.AutoStyle = true;
            this.gpgLabelACLQuery.Font = new Font("Arial", 9.75f);
            this.gpgLabelACLQuery.ForeColor = Color.White;
            this.gpgLabelACLQuery.IgnoreMouseWheel = false;
            this.gpgLabelACLQuery.IsStyled = false;
            this.gpgLabelACLQuery.Location = new Point(0x16, 0x117);
            this.gpgLabelACLQuery.Name = "gpgLabelACLQuery";
            this.gpgLabelACLQuery.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelACLQuery, null);
            this.gpgLabelACLQuery.TabIndex = 9;
            this.gpgLabelACLQuery.Text = "gpgLabel3";
            this.gpgLabelACLQuery.TextStyle = TextStyles.Default;
            this.gpgLabel7.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel7.AutoSize = true;
            this.gpgLabel7.AutoStyle = true;
            this.gpgLabel7.Font = new Font("Arial", 9.75f);
            this.gpgLabel7.ForeColor = Color.White;
            this.gpgLabel7.IgnoreMouseWheel = false;
            this.gpgLabel7.IsStyled = false;
            this.gpgLabel7.Location = new Point(0x16, 0x107);
            this.gpgLabel7.Name = "gpgLabel7";
            this.gpgLabel7.Size = new Size(0x7b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel7, null);
            this.gpgLabel7.TabIndex = 8;
            this.gpgLabel7.Text = "Stored Query Name";
            this.gpgLabel7.TextStyle = TextStyles.Bold;
            this.gpgLabelACLAccessLevel.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelACLAccessLevel.AutoSize = true;
            this.gpgLabelACLAccessLevel.AutoStyle = true;
            this.gpgLabelACLAccessLevel.Font = new Font("Arial", 9.75f);
            this.gpgLabelACLAccessLevel.ForeColor = Color.White;
            this.gpgLabelACLAccessLevel.IgnoreMouseWheel = false;
            this.gpgLabelACLAccessLevel.IsStyled = false;
            this.gpgLabelACLAccessLevel.Location = new Point(0x16, 230);
            this.gpgLabelACLAccessLevel.Name = "gpgLabelACLAccessLevel";
            this.gpgLabelACLAccessLevel.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelACLAccessLevel, null);
            this.gpgLabelACLAccessLevel.TabIndex = 7;
            this.gpgLabelACLAccessLevel.Text = "gpgLabel3";
            this.gpgLabelACLAccessLevel.TextStyle = TextStyles.Default;
            this.gpgLabel6.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel6.AutoSize = true;
            this.gpgLabel6.AutoStyle = true;
            this.gpgLabel6.Font = new Font("Arial", 9.75f);
            this.gpgLabel6.ForeColor = Color.White;
            this.gpgLabel6.IgnoreMouseWheel = false;
            this.gpgLabel6.IsStyled = false;
            this.gpgLabel6.Location = new Point(0x16, 0xd6);
            this.gpgLabel6.Name = "gpgLabel6";
            this.gpgLabel6.Size = new Size(0x55, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel6, null);
            this.gpgLabel6.TabIndex = 6;
            this.gpgLabel6.Text = "Access Level";
            this.gpgLabel6.TextStyle = TextStyles.Bold;
            this.gpgLabelACLInclusionType.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelACLInclusionType.AutoSize = true;
            this.gpgLabelACLInclusionType.AutoStyle = true;
            this.gpgLabelACLInclusionType.Font = new Font("Arial", 9.75f);
            this.gpgLabelACLInclusionType.ForeColor = Color.White;
            this.gpgLabelACLInclusionType.IgnoreMouseWheel = false;
            this.gpgLabelACLInclusionType.IsStyled = false;
            this.gpgLabelACLInclusionType.Location = new Point(0x16, 0xb1);
            this.gpgLabelACLInclusionType.Name = "gpgLabelACLInclusionType";
            this.gpgLabelACLInclusionType.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelACLInclusionType, null);
            this.gpgLabelACLInclusionType.TabIndex = 5;
            this.gpgLabelACLInclusionType.Text = "gpgLabel3";
            this.gpgLabelACLInclusionType.TextStyle = TextStyles.Default;
            this.gpgLabel5.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel5.AutoSize = true;
            this.gpgLabel5.AutoStyle = true;
            this.gpgLabel5.Font = new Font("Arial", 9.75f);
            this.gpgLabel5.ForeColor = Color.White;
            this.gpgLabel5.IgnoreMouseWheel = false;
            this.gpgLabel5.IsStyled = false;
            this.gpgLabel5.Location = new Point(0x16, 0xa1);
            this.gpgLabel5.Name = "gpgLabel5";
            this.gpgLabel5.Size = new Size(0x5b, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel5, null);
            this.gpgLabel5.TabIndex = 4;
            this.gpgLabel5.Text = "Inclusion Type";
            this.gpgLabel5.TextStyle = TextStyles.Bold;
            this.gpgLabelACLListType.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelACLListType.AutoSize = true;
            this.gpgLabelACLListType.AutoStyle = true;
            this.gpgLabelACLListType.Font = new Font("Arial", 9.75f);
            this.gpgLabelACLListType.ForeColor = Color.White;
            this.gpgLabelACLListType.IgnoreMouseWheel = false;
            this.gpgLabelACLListType.IsStyled = false;
            this.gpgLabelACLListType.Location = new Point(0x16, 0x7f);
            this.gpgLabelACLListType.Name = "gpgLabelACLListType";
            this.gpgLabelACLListType.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelACLListType, null);
            this.gpgLabelACLListType.TabIndex = 3;
            this.gpgLabelACLListType.Text = "gpgLabel3";
            this.gpgLabelACLListType.TextStyle = TextStyles.Default;
            this.gpgLabel4.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel4.AutoSize = true;
            this.gpgLabel4.AutoStyle = true;
            this.gpgLabel4.Font = new Font("Arial", 9.75f);
            this.gpgLabel4.ForeColor = Color.White;
            this.gpgLabel4.IgnoreMouseWheel = false;
            this.gpgLabel4.IsStyled = false;
            this.gpgLabel4.Location = new Point(0x16, 0x6f);
            this.gpgLabel4.Name = "gpgLabel4";
            this.gpgLabel4.Size = new Size(0x3d, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel4, null);
            this.gpgLabel4.TabIndex = 2;
            this.gpgLabel4.Text = "List Type";
            this.gpgLabel4.TextStyle = TextStyles.Bold;
            this.gpgLabelACLName.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelACLName.AutoSize = true;
            this.gpgLabelACLName.AutoStyle = true;
            this.gpgLabelACLName.Font = new Font("Arial", 9.75f);
            this.gpgLabelACLName.ForeColor = Color.White;
            this.gpgLabelACLName.IgnoreMouseWheel = false;
            this.gpgLabelACLName.IsStyled = false;
            this.gpgLabelACLName.Location = new Point(0x16, 0x24);
            this.gpgLabelACLName.Name = "gpgLabelACLName";
            this.gpgLabelACLName.Size = new Size(0x43, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabelACLName, null);
            this.gpgLabelACLName.TabIndex = 1;
            this.gpgLabelACLName.Text = "gpgLabel3";
            this.gpgLabelACLName.TextStyle = TextStyles.Default;
            this.gpgLabel2.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel2.AutoSize = true;
            this.gpgLabel2.AutoStyle = true;
            this.gpgLabel2.Font = new Font("Arial", 9.75f);
            this.gpgLabel2.ForeColor = Color.White;
            this.gpgLabel2.IgnoreMouseWheel = false;
            this.gpgLabel2.IsStyled = false;
            this.gpgLabel2.Location = new Point(0x16, 20);
            this.gpgLabel2.Name = "gpgLabel2";
            this.gpgLabel2.Size = new Size(0x2a, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel2, null);
            this.gpgLabel2.TabIndex = 0;
            this.gpgLabel2.Text = "Name";
            this.gpgLabel2.TextStyle = TextStyles.Bold;
            this.gpgLabelNoSelection.AutoGrowDirection = GrowDirections.None;
            this.gpgLabelNoSelection.AutoStyle = true;
            this.gpgLabelNoSelection.Dock = DockStyle.Fill;
            this.gpgLabelNoSelection.Font = new Font("Arial", 9.75f);
            this.gpgLabelNoSelection.ForeColor = Color.White;
            this.gpgLabelNoSelection.IgnoreMouseWheel = false;
            this.gpgLabelNoSelection.IsStyled = false;
            this.gpgLabelNoSelection.Location = new Point(0, 0);
            this.gpgLabelNoSelection.Name = "gpgLabelNoSelection";
            this.gpgLabelNoSelection.Size = new Size(0x19c, 390);
            base.ttDefault.SetSuperTip(this.gpgLabelNoSelection, null);
            this.gpgLabelNoSelection.TabIndex = 0;
            this.gpgLabelNoSelection.Text = "No access control item selected, click the menu bar below to create one or in the explorer to select one.";
            this.gpgLabelNoSelection.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabelNoSelection.TextStyle = TextStyles.Default;
            this.msQuickButtons.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.msQuickButtons.AutoSize = false;
            this.msQuickButtons.BackgroundImage = (Image) manager.GetObject("msQuickButtons.BackgroundImage");
            this.msQuickButtons.Dock = DockStyle.None;
            this.msQuickButtons.GripMargin = new Padding(0);
            this.msQuickButtons.ImageScalingSize = new Size(0x2d, 0x2d);
            this.msQuickButtons.Items.AddRange(new ToolStripItem[] { this.btnNewACL, this.btnMore });
            this.msQuickButtons.Location = new Point(60, 0x1bf);
            this.msQuickButtons.Name = "msQuickButtons";
            this.msQuickButtons.Padding = new Padding(0, 0, 10, 0);
            this.msQuickButtons.RenderMode = ToolStripRenderMode.Professional;
            this.msQuickButtons.ShowItemToolTips = true;
            this.msQuickButtons.Size = new Size(0x20c, 0x34);
            base.ttDefault.SetSuperTip(this.msQuickButtons, null);
            this.msQuickButtons.TabIndex = 13;
            this.btnNewACL.AutoSize = false;
            this.btnNewACL.AutoToolTip = true;
            this.btnNewACL.Image = (Image) manager.GetObject("btnNewACL.Image");
            this.btnNewACL.ImageScaling = ToolStripItemImageScaling.None;
            this.btnNewACL.Name = "btnNewACL";
            this.btnNewACL.ShortcutKeys = Keys.F8;
            this.btnNewACL.Size = new Size(0x25, 0x34);
            this.btnNewACL.ToolTipText = "<LOC>Create New Access Control List";
            this.btnNewACL.Click += new EventHandler(this.btnNewACL_Click);
            this.btnMore.AutoSize = false;
            this.btnMore.AutoToolTip = true;
            this.btnMore.Image = (Image) manager.GetObject("btnMore.Image");
            this.btnMore.ImageScaling = ToolStripItemImageScaling.None;
            this.btnMore.Name = "btnMore";
            this.btnMore.ShortcutKeys = Keys.F6;
            this.btnMore.Size = new Size(20, 0x34);
            this.btnMore.ToolTipText = "<LOC>More...";
            this.btnMore.Visible = false;
            this.gpgContextMenuACL.MenuItems.AddRange(new MenuItem[] { this.miACL_Delete, this.miACL_AddMember });
            this.miACL_Delete.Index = 0;
            this.miACL_Delete.Text = "Delete";
            this.miACL_Delete.Click += new EventHandler(this.miACL_Delete_Click);
            this.miACL_AddMember.Index = 1;
            this.miACL_AddMember.Text = "Add Member";
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x27b, 0x1f3);
            base.Controls.Add(this.splitContainerMain);
            base.Controls.Add(this.msQuickButtons);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x27b, 0x1f3);
            base.Name = "DlgManageACLs";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Access Control Manager";
            base.Controls.SetChildIndex(this.msQuickButtons, 0);
            base.Controls.SetChildIndex(this.splitContainerMain, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.gpgPanelACL.ResumeLayout(false);
            this.gpgPanelACL.PerformLayout();
            this.msQuickButtons.ResumeLayout(false);
            this.msQuickButtons.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializeToolstrip()
        {
            this.msQuickButtons.Paint += new PaintEventHandler(this.msQuickButtons_Paint);
            this.msQuickButtons.SizeChanged += new EventHandler(this.msQuickButtons_SizeChanged);
            this.msQuickButtons.BackgroundImage = SkinManager.GetImage(@"Dialog\ButtonStrip\bottom.png");
            this.msQuickButtons.Height = this.msQuickButtons.BackgroundImage.Height;
            this.btnMore.DropDown.BackgroundImage = DrawUtil.ResizeImage(SkinManager.GetImage("brushbg.png"), this.msQuickButtons.Items[0].Size);
            foreach (ToolStripItem item in this.msQuickButtons.Items)
            {
                item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                item.Height = this.msQuickButtons.BackgroundImage.Height;
            }
            this.btnNewACL.Image = SkinManager.GetImage(@"Dialog\ACLManager\btn_new_acl.png");
            this.btnMore.Image = SkinManager.GetImage("nav-more.png");
            foreach (ToolStripItem item in this.msQuickButtons.Items)
            {
                item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                item.Height = this.msQuickButtons.BackgroundImage.Height;
            }
            this.msQuickButtons.Refresh();
        }

        private void item_Paint(object sender, PaintEventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            if (item.Image != null)
            {
                e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
                e.Graphics.DrawImage(item.BackgroundImage, new Rectangle(this.msQuickButtons.BackgroundImage.Width, 0, item.Bounds.Width, item.Bounds.Height), new Rectangle(0, 0, item.Bounds.Width, item.Bounds.Height), GraphicsUnit.Pixel);
                if (item.Enabled)
                {
                    if (item.Selected)
                    {
                        e.Graphics.DrawImage(DrawUtil.AdjustColors(0.8f, 0.8f, 1f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        e.Graphics.DrawImage(item.Image, new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                    }
                }
                else
                {
                    e.Graphics.DrawImage(DrawUtil.GetTransparentImage(0.5f, item.Image), new Rectangle(2, 10, item.Image.Width, item.Image.Height), new Rectangle(0, 0, item.Image.Width, item.Image.Height), GraphicsUnit.Pixel);
                }
            }
        }

        private void miACL_Delete_Click(object sender, EventArgs e)
        {
            AccessControlList selectedACL;
            if ((this.SelectedMember != null) && (this.SelectedACL != null))
            {
                if (this.SelectedACL.RemoveMember(this.SelectedMember))
                {
                    selectedACL = this.SelectedACL;
                    foreach (TreeNode node in this.treeViewACL.Nodes.Find(this.SelectedACL.Name, false))
                    {
                        node.Remove();
                    }
                    this.AddACL(selectedACL);
                }
                else
                {
                    DlgMessage.ShowDialog("Error deleting access control list member", "Error");
                }
            }
            else if (this.SelectedACL != null)
            {
                if (this.SelectedACL.Delete())
                {
                    selectedACL = this.SelectedACL;
                    foreach (TreeNode node in this.treeViewACL.Nodes.Find(this.SelectedACL.Name, false))
                    {
                        node.Remove();
                    }
                    if (this.treeViewACL.Nodes.Count < 1)
                    {
                        this.gpgLabelNoSelection.BringToFront();
                    }
                }
                else
                {
                    DlgMessage.ShowDialog("Error deleting access control list", "Error");
                }
            }
        }

        private void msQuickButtons_Paint(object sender, PaintEventArgs e)
        {
            if (this.ToolstripSizeChanged)
            {
                int num2;
                foreach (ToolStripItem item in this.btnMore.DropDownItems)
                {
                    item.BackgroundImage = this.msQuickButtons.BackgroundImage;
                }
                int count = this.btnMore.DropDown.Items.Count;
                for (num2 = 0; num2 < count; num2++)
                {
                    this.msQuickButtons.Items.Insert(this.msQuickButtons.Items.Count - 1, this.btnMore.DropDown.Items[0]);
                }
                int num3 = 0;
                foreach (ToolStripItem item in this.msQuickButtons.Items)
                {
                    if (!this.mCustomPaint.Contains(item))
                    {
                        this.mCustomPaint.Add(item);
                        item.Paint += new PaintEventHandler(this.item_Paint);
                    }
                    if (item != this.btnMore)
                    {
                        int num4 = ((item.Bounds.Right + this.btnMore.Width) + item.Padding.Right) + this.btnMore.Padding.Horizontal;
                        if (num4 > this.msQuickButtons.Width)
                        {
                            num3++;
                        }
                    }
                }
                if (num3 > 0)
                {
                    this.btnMore.Visible = true;
                    this.btnMore.DropDownDirection = ToolStripDropDownDirection.AboveRight;
                    this.btnMore.DropDown.AutoSize = false;
                    this.btnMore.DropDown.Width = this.msQuickButtons.Items[0].Width;
                    this.btnMore.DropDown.Height = (this.msQuickButtons.Items[0].Height * num3) + 3;
                    this.btnMore.DropDown.Padding = new Padding(0);
                    this.btnMore.DropDown.Margin = new Padding(0);
                    for (num2 = 0; num2 < num3; num2++)
                    {
                        this.btnMore.DropDownItems.Add(this.msQuickButtons.Items[(this.msQuickButtons.Items.Count - 1) - (num3 - num2)]);
                    }
                    foreach (ToolStripItem item in this.btnMore.DropDownItems)
                    {
                        item.BackgroundImage = this.btnMore.DropDown.BackgroundImage;
                    }
                }
                else if (this.ToolstripSizeChanged)
                {
                    this.btnMore.Visible = false;
                }
                this.ToolstripSizeChanged = false;
            }
        }

        private void msQuickButtons_SizeChanged(object sender, EventArgs e)
        {
            this.ToolstripSizeChanged = true;
        }

        private void OnFirstPaint()
        {
            this.splitContainerMain.BackColor = this.SplitColor;
            this.splitContainerMain.Panel1.BackColor = this.PanelBackColor;
            this.treeViewACL.BackColor = this.PanelBackColor;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.gpgLabelNoSelection.BringToFront();
            this.RefreshACLs();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.FirstPaint)
            {
                this.FirstPaint = false;
                this.OnFirstPaint();
            }
        }

        private void RefreshACLs()
        {
            this.treeViewACL.Nodes.Clear();
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                foreach (AccessControlList list in AccessControlList.All.Values)
                {
                    this.AddACL(list);
                }
            });
            if ((this.treeViewACL.Nodes.Count > 0) && (this.treeViewACL.SelectedNode == null))
            {
                this.treeViewACL.SelectedNode = this.treeViewACL.Nodes[0];
            }
        }

        private void skinButtonAddMember_Click(object sender, EventArgs e)
        {
            if (this.SelectedACL != null)
            {
                DlgAddMember member = new DlgAddMember(this.SelectedACL);
                if (member.ShowDialog() == DialogResult.OK)
                {
                    AccessControlList selectedACL = this.SelectedACL;
                    bool isExpanded = false;
                    foreach (TreeNode node in this.treeViewACL.Nodes.Find(this.SelectedACL.Name, false))
                    {
                        node.Remove();
                        isExpanded = node.IsExpanded;
                    }
                    this.AddACL(selectedACL);
                    if (isExpanded)
                    {
                        foreach (TreeNode node in this.treeViewACL.Nodes.Find(this.SelectedACL.Name, false))
                        {
                            node.Expand();
                        }
                    }
                }
            }
        }

        private void treeViewACL_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.SelectedACL = null;
            this.SelectedMember = null;
            if (e.Node != null)
            {
                if (e.Node.Tag is AccessControlList)
                {
                    this.SelectedACL = e.Node.Tag as AccessControlList;
                    this.BindToSelectedACL();
                }
                else if (e.Node.Tag is AccessControlListMember)
                {
                    this.SelectedMember = e.Node.Tag as AccessControlListMember;
                    this.SelectedACL = e.Node.Parent.Tag as AccessControlList;
                    this.BindToSelectedMember();
                    this.BindToSelectedACL();
                }
                else
                {
                    this.gpgLabelNoSelection.BringToFront();
                }
            }
        }

        private void treeViewACL_MouseUp(object sender, MouseEventArgs e)
        {
            TreeNode nodeAt = this.treeViewACL.GetNodeAt(e.Location);
            if (nodeAt != null)
            {
                this.treeViewACL.SelectedNode = nodeAt;
            }
            if ((e.Button == MouseButtons.Right) && (this.treeViewACL.SelectedNode != null))
            {
                this.gpgContextMenuACL.Show(this.treeViewACL, e.Location);
            }
        }

        protected override bool BottomMenuStrip
        {
            get
            {
                return true;
            }
        }
    }
}

