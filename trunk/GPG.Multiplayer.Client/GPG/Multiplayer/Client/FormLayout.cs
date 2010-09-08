namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    [Serializable]
    public class FormLayout
    {
        private Rectangle mBounds;
        private System.Type mFormType;
        private object mSaveLoadData = null;
        private bool mWasOpenOnExit = false;
        private FormWindowState mWindowState;

        internal FormLayout(DlgBase form)
        {
            if (Program.IsClosing || ((!form.Disposing && !form.IsDisposed) && (form.WindowState != FormWindowState.Minimized)))
            {
                this.mBounds = form.Bounds;
                if (this.mBounds.X == -32000)
                {
                    this.mBounds.X = 0;
                }
                if (this.mBounds.Y == -32000)
                {
                    this.mBounds.Y = 0;
                }
                if (form.WindowState == FormWindowState.Minimized)
                {
                    this.mWindowState = FormWindowState.Normal;
                }
                else
                {
                    this.mWindowState = form.WindowState;
                }
                this.mSaveLoadData = form.LayoutData;
                this.mWasOpenOnExit = (Program.IsClosing && form.AllowRestoreWindow) && form.Visible;
                this.mFormType = form.GetType();
            }
        }

        internal void Restore(DlgBase form)
        {
            try
            {
                form.Bounds = this.Bounds;
                form.WindowState = this.WindowState;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return this.mBounds;
            }
        }

        public System.Type FormType
        {
            get
            {
                return this.mFormType;
            }
        }

        public object SaveLoadData
        {
            get
            {
                return this.mSaveLoadData;
            }
        }

        public bool WasOpenOnExit
        {
            get
            {
                return this.mWasOpenOnExit;
            }
            set
            {
                this.mWasOpenOnExit = value;
            }
        }

        public FormWindowState WindowState
        {
            get
            {
                return this.mWindowState;
            }
        }
    }
}

