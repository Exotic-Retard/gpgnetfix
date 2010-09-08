namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using GPG.Multiplayer.Quazal.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Web.Services;
    using System.Web.Services.Description;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;

    [GeneratedCode("System.Web.Services", "2.0.50727.1433"), DebuggerStepThrough, DesignerCategory("code"), WebServiceBinding(Name="ServiceSoap", Namespace="http://gpgnet.gaspowered.com/")]
    public class Service : SoapHttpClientProtocol
    {
        private SendOrPostCallback GetRankingsOperationCompleted;
        private SendOrPostCallback GetServerStatsOperationCompleted;
        private SendOrPostCallback GetSolutionDetailsOperationCompleted;
        private SendOrPostCallback HelloWorldOperationCompleted;
        private SendOrPostCallback SLKeywordSearchOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event GetRankingsCompletedEventHandler GetRankingsCompleted;

        public event GetServerStatsCompletedEventHandler GetServerStatsCompleted;

        public event GetSolutionDetailsCompletedEventHandler GetSolutionDetailsCompleted;

        public event HelloWorldCompletedEventHandler HelloWorldCompleted;

        public event SLKeywordSearchCompletedEventHandler SLKeywordSearchCompleted;

        public Service()
        {
            this.Url = Settings.Default.GPG_Multiplayer_Quazal_SolutionsLib_Service;
            if (this.IsLocalFileSystemWebService(this.Url))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        [SoapDocumentMethod("http://gpgnet.gaspowered.com/GetRankings", RequestNamespace="http://gpgnet.gaspowered.com/", ResponseNamespace="http://gpgnet.gaspowered.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public PlayerRanking[] GetRankings(string username, string password, string ladderType, int startRank, int endRank)
        {
            return (PlayerRanking[]) base.Invoke("GetRankings", new object[] { username, password, ladderType, startRank, endRank })[0];
        }

        public void GetRankingsAsync(string username, string password, string ladderType, int startRank, int endRank)
        {
            this.GetRankingsAsync(username, password, ladderType, startRank, endRank, null);
        }

        public void GetRankingsAsync(string username, string password, string ladderType, int startRank, int endRank, object userState)
        {
            if (this.GetRankingsOperationCompleted == null)
            {
                this.GetRankingsOperationCompleted = new SendOrPostCallback(this.OnGetRankingsOperationCompleted);
            }
            base.InvokeAsync("GetRankings", new object[] { username, password, ladderType, startRank, endRank }, this.GetRankingsOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://gpgnet.gaspowered.com/GetServerStats", RequestNamespace="http://gpgnet.gaspowered.com/", ResponseNamespace="http://gpgnet.gaspowered.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public ServerStat[] GetServerStats(string username, string password)
        {
            return (ServerStat[]) base.Invoke("GetServerStats", new object[] { username, password })[0];
        }

        public void GetServerStatsAsync(string username, string password)
        {
            this.GetServerStatsAsync(username, password, null);
        }

        public void GetServerStatsAsync(string username, string password, object userState)
        {
            if (this.GetServerStatsOperationCompleted == null)
            {
                this.GetServerStatsOperationCompleted = new SendOrPostCallback(this.OnGetServerStatsOperationCompleted);
            }
            base.InvokeAsync("GetServerStats", new object[] { username, password }, this.GetServerStatsOperationCompleted, userState);
        }

        [return: XmlElement("rtf")]
        [SoapDocumentMethod("http://gpgnet.gaspowered.com/GetSolutionDetails", RequestNamespace="http://gpgnet.gaspowered.com/", ResponseNamespace="http://gpgnet.gaspowered.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string GetSolutionDetails(int solutionid, out string title, out string author, out string[] keywords)
        {
            object[] objArray = base.Invoke("GetSolutionDetails", new object[] { solutionid });
            title = (string) objArray[1];
            author = (string) objArray[2];
            keywords = (string[]) objArray[3];
            return (string) objArray[0];
        }

        public void GetSolutionDetailsAsync(int solutionid)
        {
            this.GetSolutionDetailsAsync(solutionid, null);
        }

        public void GetSolutionDetailsAsync(int solutionid, object userState)
        {
            if (this.GetSolutionDetailsOperationCompleted == null)
            {
                this.GetSolutionDetailsOperationCompleted = new SendOrPostCallback(this.OnGetSolutionDetailsOperationCompleted);
            }
            base.InvokeAsync("GetSolutionDetails", new object[] { solutionid }, this.GetSolutionDetailsOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://gpgnet.gaspowered.com/HelloWorld", RequestNamespace="http://gpgnet.gaspowered.com/", ResponseNamespace="http://gpgnet.gaspowered.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string HelloWorld()
        {
            return (string) base.Invoke("HelloWorld", new object[0])[0];
        }

        public void HelloWorldAsync()
        {
            this.HelloWorldAsync(null);
        }

        public void HelloWorldAsync(object userState)
        {
            if (this.HelloWorldOperationCompleted == null)
            {
                this.HelloWorldOperationCompleted = new SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            base.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if ((url == null) || (url == string.Empty))
            {
                return false;
            }
            Uri uri = new Uri(url);
            return ((uri.Port >= 0x400) && (string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0));
        }

        private void OnGetRankingsOperationCompleted(object arg)
        {
            if (this.GetRankingsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.GetRankingsCompleted(this, new GetRankingsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnGetServerStatsOperationCompleted(object arg)
        {
            if (this.GetServerStatsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.GetServerStatsCompleted(this, new GetServerStatsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnGetSolutionDetailsOperationCompleted(object arg)
        {
            if (this.GetSolutionDetailsCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.GetSolutionDetailsCompleted(this, new GetSolutionDetailsCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnHelloWorldOperationCompleted(object arg)
        {
            if (this.HelloWorldCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnSLKeywordSearchOperationCompleted(object arg)
        {
            if (this.SLKeywordSearchCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.SLKeywordSearchCompleted(this, new SLKeywordSearchCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        [SoapDocumentMethod("http://gpgnet.gaspowered.com/SLKeywordSearch", RequestNamespace="http://gpgnet.gaspowered.com/", ResponseNamespace="http://gpgnet.gaspowered.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public SolutionListItem[] SLKeywordSearch(string[] keywords)
        {
            return (SolutionListItem[]) base.Invoke("SLKeywordSearch", new object[] { keywords })[0];
        }

        public void SLKeywordSearchAsync(string[] keywords)
        {
            this.SLKeywordSearchAsync(keywords, null);
        }

        public void SLKeywordSearchAsync(string[] keywords, object userState)
        {
            if (this.SLKeywordSearchOperationCompleted == null)
            {
                this.SLKeywordSearchOperationCompleted = new SendOrPostCallback(this.OnSLKeywordSearchOperationCompleted);
            }
            base.InvokeAsync("SLKeywordSearch", new object[] { keywords }, this.SLKeywordSearchOperationCompleted, userState);
        }

        public string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly) && !this.IsLocalFileSystemWebService(value))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
    }
}

