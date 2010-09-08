namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Web.Services", "2.0.50727.1433")]
    public class GetServerStatsCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal GetServerStatsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public ServerStat[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (ServerStat[]) this.results[0];
            }
        }
    }
}

