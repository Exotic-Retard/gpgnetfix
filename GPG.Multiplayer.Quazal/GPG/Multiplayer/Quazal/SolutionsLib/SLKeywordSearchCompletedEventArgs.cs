﻿namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;

    [GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough, DesignerCategory("code")]
    public class SLKeywordSearchCompletedEventArgs : AsyncCompletedEventArgs
    {
        private object[] results;

        internal SLKeywordSearchCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
        {
            this.results = results;
        }

        public SolutionListItem[] Result
        {
            get
            {
                base.RaiseExceptionIfNecessary();
                return (SolutionListItem[]) this.results[0];
            }
        }
    }
}

