namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DebuggerStepThrough, GeneratedCode("System.Web.Services", "2.0.50727.1433"), DesignerCategory("code")]
    public class HelloWorldCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal HelloWorldCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
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

