﻿namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.replay
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough]
    public class SubmitGameCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal SubmitGameCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[0];
            }
        }
    }
}

