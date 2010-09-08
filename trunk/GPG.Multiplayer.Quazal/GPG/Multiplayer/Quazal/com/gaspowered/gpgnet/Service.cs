namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet
{
    using GPG.Multiplayer.Quazal.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.Services;
    using System.Web.Services.Description;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;

    [DebuggerStepThrough, WebServiceBinding(Name="ServiceSoap", Namespace="http://gpgnet.gaspowered.com/"), DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.1433")]
    public class Service : SoapHttpClientProtocol
    {
        private SendOrPostCallback HelloWorldOperationCompleted;
        private SendOrPostCallback SubmitGameReportOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event HelloWorldCompletedEventHandler HelloWorldCompleted;

        public event SubmitGameReportCompletedEventHandler SubmitGameReportCompleted;

        public Service()
        {
            this.Url = Settings.Default.GPG_Multiplayer_Quazal_com_gaspowered_gpgnet_Service;
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

        private void OnHelloWorldOperationCompleted(object arg)
        {
            if (this.HelloWorldCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnSubmitGameReportOperationCompleted(object arg)
        {
            if (this.SubmitGameReportCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.SubmitGameReportCompleted(this, new SubmitGameReportCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        [SoapDocumentMethod("http://gpgnet.gaspowered.com/SubmitGameReport", RequestNamespace="http://gpgnet.gaspowered.com/", ResponseNamespace="http://gpgnet.gaspowered.com/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public bool SubmitGameReport(string key, string playername, [XmlElement(DataType="base64Binary")] byte[] gamedata)
        {
            return (bool) base.Invoke("SubmitGameReport", new object[] { key, playername, gamedata })[0];
        }

        public void SubmitGameReportAsync(string key, string playername, byte[] gamedata)
        {
            this.SubmitGameReportAsync(key, playername, gamedata, null);
        }

        public void SubmitGameReportAsync(string key, string playername, byte[] gamedata, object userState)
        {
            if (this.SubmitGameReportOperationCompleted == null)
            {
                this.SubmitGameReportOperationCompleted = new SendOrPostCallback(this.OnSubmitGameReportOperationCompleted);
            }
            base.InvokeAsync("SubmitGameReport", new object[] { key, playername, gamedata }, this.SubmitGameReportOperationCompleted, userState);
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

