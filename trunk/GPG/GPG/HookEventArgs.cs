namespace GPG
{
    using System;

    public class HookEventArgs
    {
        private int mCode;
        private bool mContinue = true;
        private IntPtr mLParam;
        private IntPtr mWParam;

        public HookEventArgs(int code, IntPtr wParam, IntPtr lParam)
        {
            this.mCode = code;
            this.mWParam = wParam;
            this.mLParam = lParam;
        }

        public int Code
        {
            get
            {
                return this.mCode;
            }
        }

        internal bool Continue
        {
            get
            {
                return this.mContinue;
            }
            set
            {
                this.mContinue = value;
            }
        }

        public IntPtr LParam
        {
            get
            {
                return this.mLParam;
            }
        }

        public IntPtr WParam
        {
            get
            {
                return this.mWParam;
            }
        }
    }
}

