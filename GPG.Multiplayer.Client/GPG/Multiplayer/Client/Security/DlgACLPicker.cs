namespace GPG.Multiplayer.Client.Security
{
    using DevExpress.Utils;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal.Security;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgACLPicker : DlgBase
    {
        private IContainer components = null;
        private AccessControlList mSelectedACL;
        private AccessControlListMember mSelectedMember;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonClear;
        private SkinButton skinButtonOK;
        private TreeView treeViewACL;

        public DlgACLPicker()
        {
            this.InitializeComponent();
            this.RefreshACLs();
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
            this.treeViewACL = new TreeView();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonOK = new SkinButton();
            this.skinButtonClear = new SkinButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x11d, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.treeViewACL.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeViewACL.BackColor = Color.FromArgb(0x33, 0x33, 0x33);
            this.treeViewACL.BorderStyle = BorderStyle.None;
            this.treeViewACL.Location = new Point(6, 60);
            this.treeViewACL.Name = "treeViewACL";
            this.treeViewACL.Size = new Size(0x14d, 0x148);
            base.ttDefault.SetSuperTip(this.treeViewACL, null);
            this.treeViewACL.TabIndex = 8;
            this.treeViewACL.AfterSelect += new TreeViewEventHandler(this.treeViewACL_AfterSelect);
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Transparent;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(230, 0x18a);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x67, 0x1a);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 9;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonOK.AutoStyle = true;
            this.skinButtonOK.BackColor = Color.Transparent;
            this.skinButtonOK.ButtonState = 0;
            this.skinButtonOK.DialogResult = DialogResult.OK;
            this.skinButtonOK.DisabledForecolor = Color.Gray;
            this.skinButtonOK.DrawColor = Color.White;
            this.skinButtonOK.DrawEdges = true;
            this.skinButtonOK.FocusColor = Color.Yellow;
            this.skinButtonOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonOK.ForeColor = Color.White;
            this.skinButtonOK.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonOK.IsStyled = true;
            this.skinButtonOK.Location = new Point(12, 0x18a);
            this.skinButtonOK.Name = "skinButtonOK";
            this.skinButtonOK.Size = new Size(0x67, 0x1a);
            this.skinButtonOK.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonOK, null);
            this.skinButtonOK.TabIndex = 10;
            this.skinButtonOK.TabStop = true;
            this.skinButtonOK.Text = "<LOC>OK";
            this.skinButtonOK.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonOK.TextPadding = new Padding(0);
            this.skinButtonOK.Click += new EventHandler(this.skinButtonOK_Click);
            this.skinButtonClear.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonClear.AutoStyle = true;
            this.skinButtonClear.BackColor = Color.Transparent;
            this.skinButtonClear.ButtonState = 0;
            this.skinButtonClear.DialogResult = DialogResult.OK;
            this.skinButtonClear.DisabledForecolor = Color.Gray;
            this.skinButtonClear.DrawColor = Color.White;
            this.skinButtonClear.DrawEdges = true;
            this.skinButtonClear.FocusColor = Color.Yellow;
            this.skinButtonClear.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonClear.ForeColor = Color.White;
            this.skinButtonClear.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonClear.IsStyled = true;
            this.skinButtonClear.Location = new Point(0x79, 0x18a);
            this.skinButtonClear.Name = "skinButtonClear";
            this.skinButtonClear.Size = new Size(0x67, 0x1a);
            this.skinButtonClear.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonClear, null);
            this.skinButtonClear.TabIndex = 11;
            this.skinButtonClear.TabStop = true;
            this.skinButtonClear.Text = "<LOC>Clear ACL";
            this.skinButtonClear.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonClear.TextPadding = new Padding(0);
            this.skinButtonClear.Click += new EventHandler(this.skinButtonClear_Click);
            base.AcceptButton = this.skinButtonOK;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x158, 0x1e3);
            base.Controls.Add(this.skinButtonClear);
            base.Controls.Add(this.skinButtonOK);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.treeViewACL);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x158, 0x1e3);
            base.Name = "DlgACLPicker";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "Access Control Browser";
            base.Controls.SetChildIndex(this.treeViewACL, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonOK, 0);
            base.Controls.SetChildIndex(this.skinButtonClear, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
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

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void skinButtonClear_Click(object sender, EventArgs e)
        {
            this.mSelectedACL = null;
            this.mSelectedMember = null;
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void skinButtonOK_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void treeViewACL_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.mSelectedACL = null;
            this.mSelectedMember = null;
            if (e.Node != null)
            {
                if (e.Node.Tag is AccessControlList)
                {
                    this.mSelectedACL = e.Node.Tag as AccessControlList;
                }
                else if (e.Node.Tag is AccessControlListMember)
                {
                    this.mSelectedMember = e.Node.Tag as AccessControlListMember;
                    this.mSelectedACL = e.Node.Parent.Tag as AccessControlList;
                }
            }
        }

        public AccessControlList SelectedACL
        {
            get
            {
                return this.mSelectedACL;
            }
        }

        public AccessControlListMember SelectedMember
        {
            get
            {
                return this.mSelectedMember;
            }
        }
    }
}

