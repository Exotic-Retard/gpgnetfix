namespace GPG.Multiplayer.Client.Vaulting
{
    using System;

    public class ContentOperationCallbackArgs
    {
        private IVaultOperation mActiveOperation;
        private object[] mArgs;
        private bool mCompletedSuccessfully;
        private IAdditionalContent mContent;

        public ContentOperationCallbackArgs(IAdditionalContent content, IVaultOperation activityMonitor) : this(content, activityMonitor, true, new object[0])
        {
        }

        public ContentOperationCallbackArgs(IAdditionalContent content, IVaultOperation activeOperation, bool success, params object[] args)
        {
            this.mArgs = null;
            this.mContent = content;
            this.mActiveOperation = activeOperation;
            this.mCompletedSuccessfully = success;
            this.mArgs = args;
        }

        public IVaultOperation ActiveOperation
        {
            get
            {
                return this.mActiveOperation;
            }
        }

        public object[] Args
        {
            get
            {
                return this.mArgs;
            }
            set
            {
                this.mArgs = value;
            }
        }

        public bool CompletedSuccessfully
        {
            get
            {
                return this.mCompletedSuccessfully;
            }
        }

        public IAdditionalContent Content
        {
            get
            {
                return this.mContent;
            }
        }
    }
}

