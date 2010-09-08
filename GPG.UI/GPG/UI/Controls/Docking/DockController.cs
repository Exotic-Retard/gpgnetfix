namespace GPG.UI.Controls.Docking
{
    using GPG;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public sealed class DockController
    {
        private MouseEventHandler GlobalButtonUp;
        private MouseEventHandler GlobalMove;
        internal static bool IsInitialized;
        private DockPanel mActivePanel;
        private List<DockContainerForm> mContainers = new List<DockContainerForm>();
        private System.Type mContainerType = typeof(DockContainerForm);
        private bool mIsDragging;
        private bool mIsPreviewing;
        private List<DockPanel> mPanels = new List<DockPanel>();
        private Size mSnapshotSize = new Size(0x40, 0x40);
        private DockSnapshotForm SnapshotForm;
        private int SpawnIndex = 1;

        internal event EventHandler BeginDockDrag;

        internal event EventHandler EndDockDrag;

        public DockController()
        {
            this.Construct();
        }

        public type AddPanel<type>() where type: DockPanel
        {
            return (this.AddPanel(typeof(type), true) as type);
        }

        public DockPanel AddPanel()
        {
            return this.AddPanel(typeof(DockPanel), true);
        }

        public DockPanel AddPanel(System.Type panelType)
        {
            return this.AddPanel(panelType, panelType.IsSubclassOf(typeof(DockPanel)));
        }

        private DockPanel AddPanel(System.Type panelType, bool valid)
        {
            if (!valid)
            {
                throw new ArgumentException("Parameter 'panelType' must derive from GPG.UI.Controls.Docking.DockPanel");
            }
            if ((this.Panels.Count < 1) || (this.Containers.Count < 1))
            {
                GlobalHook.Mouse.Install();
            }
            DockPanel item = Activator.CreateInstance(panelType) as DockPanel;
            item.Controller = this;
            item.Title = string.Format("Dock Panel {0}", this.SpawnIndex);
            this.Panels.Add(item);
            new DockContainerForm(this, item).Show();
            this.SpawnIndex++;
            return item;
        }

        internal void BeginPreviewSnapshot(Rectangle region)
        {
            this.mIsPreviewing = true;
            Image snapshot = DrawUtil.ResizeImage(DrawUtil.Snapshot(this.ActivePanel), region.Size);
            this.SnapshotForm.Location = region.Location;
            this.SnapshotForm.Size = region.Size;
            this.SnapshotForm.Opacity = 0.75;
            this.SnapshotForm.SetSnapshot(snapshot);
            this.SnapshotForm.Refresh();
        }

        private void Construct()
        {
            IsInitialized = true;
            this.GlobalButtonUp = delegate (object s, MouseEventArgs e) {
                if (e.Button == MouseButtons.Left)
                {
                    this.OnEndDockDrag();
                }
            };
            this.GlobalMove = delegate (object s, MouseEventArgs e) {
                if (!this.IsPreviewing)
                {
                    this.SnapshotForm.Location = e.Location;
                }
            };
        }

        internal void DestroyContainer(DockContainerForm container)
        {
            this.Containers.Remove(container);
            container.Dispose();
            container = null;
            if ((this.Panels.Count < 1) && (this.Containers.Count < 1))
            {
                GlobalHook.Mouse.Uninstall();
            }
        }

        internal void DestroyPanel(DockPanel panel)
        {
            this.Panels.Remove(panel);
            panel.Dispose();
            panel = null;
            if ((this.Panels.Count < 1) && (this.Containers.Count < 1))
            {
                GlobalHook.Mouse.Uninstall();
            }
        }

        internal void EndPreviewSnapshot()
        {
            this.SnapshotForm.Size = this.SnapshotSize;
            this.SnapshotForm.Location = Cursor.Position;
            Image snapshot = DrawUtil.ResizeImage(DrawUtil.Snapshot(this.ActivePanel), this.SnapshotSize);
            this.SnapshotForm.Opacity = 0.5;
            this.SnapshotForm.SetSnapshot(snapshot);
            this.SnapshotForm.Refresh();
            this.mIsPreviewing = false;
        }

        internal void OnBeginDockDrag(DockPanel panel)
        {
            this.SnapshotForm = new DockSnapshotForm(DrawUtil.ResizeImage(DrawUtil.Snapshot(panel), this.SnapshotSize), Cursor.Position);
            this.SnapshotForm.Show();
            this.mActivePanel = panel;
            this.mIsDragging = true;
            foreach (DockPanel panel2 in this.Panels)
            {
                if (!panel2.Equals(panel))
                {
                    panel2.ShowAnchors();
                }
            }
            if (this.BeginDockDrag != null)
            {
                this.BeginDockDrag(this, EventArgs.Empty);
            }
            GlobalHook.Mouse.Moved += this.GlobalMove;
            GlobalHook.Mouse.ButtonUp += this.GlobalButtonUp;
        }

        internal void OnEndDockDrag()
        {
            GlobalHook.Mouse.Moved -= this.GlobalMove;
            GlobalHook.Mouse.ButtonUp -= this.GlobalButtonUp;
            this.SnapshotForm.Close();
            this.SnapshotForm = null;
            this.mIsDragging = false;
            foreach (DockPanel panel in this.Panels)
            {
                if (!panel.Equals(this.ActivePanel))
                {
                    panel.HideAnchors();
                }
            }
            this.mActivePanel = null;
            if (this.EndDockDrag != null)
            {
                this.EndDockDrag(this, EventArgs.Empty);
            }
        }

        public DockPanel ActivePanel
        {
            get
            {
                return this.mActivePanel;
            }
        }

        public List<DockContainerForm> Containers
        {
            get
            {
                return this.mContainers;
            }
        }

        public System.Type ContainerType
        {
            get
            {
                return this.mContainerType;
            }
            set
            {
                if (!value.IsSubclassOf(typeof(DockContainerForm)))
                {
                    throw new ArgumentException("Parameter 'value' must derive from GPG.UI.Controls.Docking.DockContainerForm");
                }
                this.mContainerType = value;
            }
        }

        internal bool IsDragging
        {
            get
            {
                return this.mIsDragging;
            }
        }

        public bool IsPreviewing
        {
            get
            {
                return this.mIsPreviewing;
            }
        }

        public List<DockPanel> Panels
        {
            get
            {
                return this.mPanels;
            }
        }

        public Size SnapshotSize
        {
            get
            {
                return this.mSnapshotSize;
            }
            set
            {
                this.mSnapshotSize = value;
            }
        }
    }
}

