namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough]
    public class GetSolutionDetailsCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal GetSolutionDetailsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public string author
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[2];
            }
        }

        public string[] keywords
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string[]) this.results[3];
            }
        }

        public string Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[0];
            }
        }

        public string title
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (string) this.results[1];
            }
        }
    }
}

