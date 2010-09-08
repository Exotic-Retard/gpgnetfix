namespace GPG.UI.Controls.Docking
{
    using GPG;
    using GPG.UI.Controls.Skinning;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DockAnchor : PictureBox
    {
        private MouseEventHandler _Mouse_ButtonUp;
        private MouseEventHandler _Mouse_Moved;
        private const int ANCHOR_OFFSET = 8;
        private IContainer components;
        private bool ContainsMouse;
        private DockPanel mContainerPanel;
        private DockStyles mOrientation;

        public DockAnchor(DockStyles orientation)
        {
            this.InitializeComponent();
            this.mOrientation = orientation;
            base.SizeMode = PictureBoxSizeMode.AutoSize;
            this.BackColor = Color.Transparent;
            base.Visible = false;
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

        private void Mouse_ButtonUp(object sender, MouseEventArgs e)
        {
            if (base.Visible && this.ContainsMouse)
            {
                this.OnMouseUp(e);
            }
        }

        private void Mouse_Moved(object sender, MouseEventArgs e)
        {
            if (base.Visible)
            {
                bool containsMouse = this.ContainsMouse;
                if (this.ContainerPanel.TopLevelControl.Bounds.Contains(e.Location))
                {
                    Point pt = this.ContainerPanel.PointToClient(e.Location);
                    this.ContainsMouse = base.Bounds.Contains(pt);
                }
                else
                {
                    this.ContainsMouse = false;
                }
                if (containsMouse && !this.ContainsMouse)
                {
                    this.OnMouseLeave(e);
                }
                else if (this.ContainsMouse && !containsMouse)
                {
                    this.OnMouseEnter(e);
                }
                if (this.ContainsMouse)
                {
                    this.OnMouseMove(e);
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.Controller.IsDragging)
            {
                Cursor.Current = Cursors.Hand;
                this.ContainerPanel.BeginDockPreview(this.Orientation);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.Controller.IsDragging)
            {
                Cursor.Current = Cursors.Default;
                this.ContainerPanel.EndDockPreview();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.Controller.IsPreviewing)
            {
                Cursor.Current = Cursors.Default;
                this.ContainerPanel.EndDockPreview();
            }
            this.Controller.ActivePanel.DockTo(this.ContainerPanel, this.Orientation);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.mContainerPanel = base.Parent as DockPanel;
            this.PositionAnchor();
            base.Parent.SizeChanged += delegate (object s, EventArgs ea) {
                this.PositionAnchor();
            };
            this._Mouse_Moved = new MouseEventHandler(this.Mouse_Moved);
            this._Mouse_ButtonUp = new MouseEventHandler(this.Mouse_ButtonUp);
            this.Controller.BeginDockDrag += delegate (object s, EventArgs ea) {
                GlobalHook.Mouse.Moved += this._Mouse_Moved;
                GlobalHook.Mouse.ButtonUp += this._Mouse_ButtonUp;
            };
            this.Controller.EndDockDrag += delegate (object s, EventArgs ea) {
                GlobalHook.Mouse.Moved -= this._Mouse_Moved;
                GlobalHook.Mouse.ButtonUp -= this._Mouse_ButtonUp;
            };
        }

        private void PositionAnchor()
        {
            Rectangle clientRectangle = this.ContainerPanel.ClientRectangle;
            switch (this.Orientation)
            {
                case DockStyles.Left:
                    base.Image = SkinManager.Docking.GetImage("anchor_left");
                    base.Location = new Point(clientRectangle.X + 8, ((clientRectangle.Height / 2) + clientRectangle.Y) - (base.Height / 2));
                    this.Anchor = AnchorStyles.Left;
                    return;

                case DockStyles.Right:
                    base.Image = SkinManager.Docking.GetImage("anchor_right");
                    base.Location = new Point(clientRectangle.Width - (base.Width + (clientRectangle.X + 8)), ((clientRectangle.Height / 2) + clientRectangle.Y) - (base.Height / 2));
                    return;

                case DockStyles.Top:
                    base.Image = SkinManager.Docking.GetImage("anchor_top");
                    base.Location = new Point(((clientRectangle.Width - clientRectangle.X) / 2) - (base.Width / 2), clientRectangle.Y + 8);
                    return;

                case DockStyles.Bottom:
                    base.Image = SkinManager.Docking.GetImage("anchor_bottom");
                    base.Location = new Point(((clientRectangle.Width - clientRectangle.X) / 2) - (base.Width / 2), (clientRectangle.Height + clientRectangle.Y) - (base.Height + 8));
                    return;

                case DockStyles.Fill:
                    break;

                case DockStyles.Tabbed:
                    base.Image = SkinManager.Docking.GetImage("anchor_tab");
                    base.Location = new Point(((clientRectangle.Width / 2) + clientRectangle.X) - (base.Width / 2), ((clientRectangle.Height / 2) + clientRectangle.Y) - (base.Height / 2));
                    break;

                default:
                    return;
            }
        }

        public DockPanel ContainerPanel
        {
            get
            {
                return this.mContainerPanel;
            }
        }

        public DockController Controller
        {
            get
            {
                return this.ContainerPanel.Controller;
            }
        }

        public DockStyles Orientation
        {
            get
            {
                return this.mOrientation;
            }
        }
    }
}

