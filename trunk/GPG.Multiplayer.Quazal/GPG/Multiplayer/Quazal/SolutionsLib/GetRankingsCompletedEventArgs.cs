namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough, DesignerCategory("code")]
    public class GetRankingsCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal GetRankingsCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public PlayerRanking[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (PlayerRanking[]) this.results[0];
            }
        }
    }
}

