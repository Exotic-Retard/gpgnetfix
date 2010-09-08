namespace GPG.UI.Controls
{
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.Logging;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGChatGrid : GPGDataGrid, IFilterControl, IWdnProcControl
    {
        private IContainer components;
        private Dictionary<string, bool> mFilters = new Dictionary<string, bool>();
        private bool mIgnoreMouseWheel;

        public static  event EventHandler OnStyleControl;

        public event VGen1 OnWndProc;

        public GPGChatGrid()
        {
            this.InitializeComponent();
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

        protected override void OnLeave(EventArgs e)
        {
        }

        protected override void RegisterView(BaseView gv)
        {
            base.RegisterView(gv);
        }

        protected override void WndProc(ref Message m)
        {
            if (!this.IgnoreMouseWheel || (m.Msg != 0x20a))
            {
                try
                {
                    base.WndProc(ref m);
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
            else if (this.OnWndProc != null)
            {
                this.OnWndProc((Message) m);
            }
        }

        public Dictionary<string, bool> Filters
        {
            get
            {
                return this.mFilters;
            }
        }

        public bool IgnoreMouseWheel
        {
            get
            {
                return this.mIgnoreMouseWheel;
            }
            set
            {
                this.mIgnoreMouseWheel = value;
            }
        }

        public bool IsFiltered
        {
            get
            {
                return (this.Filters.Count > 0);
            }
        }

        public float VScrollPosition
        {
            get
            {
                if (base.Scroller == null)
                {
                    return -1f;
                }
                return (float) base.Scroller.DeltaY;
            }
        }

        public float VScrollPositionF
        {
            get
            {
                if (base.Scroller == null)
                {
                    return -1f;
                }
                return (base.Scroller.DeltaY * 0.01f);
            }
        }
    }
}

