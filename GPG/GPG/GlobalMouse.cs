namespace GPG
{
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class GlobalMouse : GlobalHook
    {
        private bool LeftDown;
        private bool RightDown;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;

        public event MouseEventHandler ButtonDown;

        public event MouseEventHandler ButtonUp;

        public event MouseEventHandler Moved;

        public GlobalMouse() : base(HookTypes.WH_MOUSE_LL)
        {
        }

        private void OnButtonDown(MouseEventArgs e)
        {
            if (this.ButtonDown != null)
            {
                this.ButtonDown(this, e);
            }
        }

        private void OnButtonUp(MouseEventArgs e)
        {
            if (this.ButtonUp != null)
            {
                this.ButtonUp(this, e);
            }
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            if (this.Moved != null)
            {
                this.Moved(this, e);
            }
        }

        protected override int OnProc(int code, IntPtr wParam, IntPtr lParam)
        {
            MouseButtons none;
            MSLLHOOKSTRUCT structure = new MSLLHOOKSTRUCT();
            Marshal.PtrToStructure(lParam, structure);
            MouseEventArgs e = null;
            switch (wParam.ToInt32())
            {
                case 0x200:
                    none = MouseButtons.None;
                    if (!this.LeftDown)
                    {
                        if (this.RightDown)
                        {
                            none = MouseButtons.Right;
                        }
                        break;
                    }
                    none = MouseButtons.Left;
                    break;

                case 0x201:
                    this.LeftDown = true;
                    e = new MouseEventArgs(MouseButtons.Left, 0, structure.pt.X, structure.pt.Y, 0);
                    this.OnButtonDown(e);
                    goto Label_0158;

                case 0x202:
                    this.LeftDown = false;
                    e = new MouseEventArgs(MouseButtons.Left, 0, structure.pt.X, structure.pt.Y, 0);
                    this.OnButtonUp(e);
                    goto Label_0158;

                case 0x204:
                    this.RightDown = true;
                    e = new MouseEventArgs(MouseButtons.Right, 0, structure.pt.X, structure.pt.Y, 0);
                    this.OnButtonDown(e);
                    goto Label_0158;

                case 0x205:
                    this.RightDown = false;
                    e = new MouseEventArgs(MouseButtons.Right, 0, structure.pt.X, structure.pt.Y, 0);
                    this.OnButtonUp(e);
                    goto Label_0158;

                default:
                    goto Label_0158;
            }
            e = new MouseEventArgs(none, 0, structure.pt.X, structure.pt.Y, 0);
            this.OnMouseMove(e);
        Label_0158:
            return base.OnProc(code, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MSLLHOOKSTRUCT
        {
            public GlobalMouse.POINT pt;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator Point(GlobalMouse.POINT p)
            {
                return new Point(p.X, p.Y);
            }

            public static implicit operator GlobalMouse.POINT(Point p)
            {
                return new GlobalMouse.POINT(p.X, p.Y);
            }
        }
    }
}

