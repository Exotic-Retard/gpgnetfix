namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.1433"), DesignerCategory("code"), DebuggerStepThrough]
    public class GetPreviewImageCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal GetPreviewImageCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public byte[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (byte[]) this.results[0];
            }
        }
    }
}

