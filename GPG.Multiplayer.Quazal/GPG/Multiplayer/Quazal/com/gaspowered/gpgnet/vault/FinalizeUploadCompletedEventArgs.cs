namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough]
    public class FinalizeUploadCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal FinalizeUploadCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public ContentUploadResult Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (ContentUploadResult) this.results[0];
            }
        }
    }
}

