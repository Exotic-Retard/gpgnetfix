namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough]
    public class RequestContentCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal RequestContentCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public ChunkDownloadResult Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (ChunkDownloadResult) this.results[0];
            }
        }
    }
}

