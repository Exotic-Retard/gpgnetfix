namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough, DesignerCategory("code")]
    public class SubmitContentChunkCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal SubmitContentChunkCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public ChunkUploadResult Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (ChunkUploadResult) this.results[0];
            }
        }
    }
}

