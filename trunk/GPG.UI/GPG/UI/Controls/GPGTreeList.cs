namespace GPG.UI.Controls
{
    using DevExpress.XtraTreeList;
    using DevExpress.XtraTreeList.Nodes;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class GPGTreeList : TreeList
    {
        private IContainer components;

        public event NodeDelegate OnNewNode;

        public GPGTreeList()
        {
            this.InitializeComponent();
        }

        public GPGTreeList(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
        }

        public override TreeListNode AppendNode(object nodeData, TreeListNode parentNode)
        {
            TreeListNode node = base.AppendNode(nodeData, parentNode);
            if (this.OnNewNode != null)
            {
                this.OnNewNode(node);
            }
            return node;
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
    }
}

