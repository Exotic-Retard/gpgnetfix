namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.replay
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

    [GeneratedCode("System.Web.Services", "2.0.50727.1433"), WebServiceBinding(Name="ServiceSoap", Namespace="http://tempuri.org/"), DebuggerStepThrough, DesignerCategory("code")]
    public class Service : SoapHttpClientProtocol
    {
        private SendOrPostCallback HelloWorldOperationCompleted;
        private SendOrPostCallback SubmitGameOperationCompleted;
        private SendOrPostCallback SubmitGPGNetDataOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event HelloWorldCompletedEventHandler HelloWorldCompleted;

        public event SubmitGameCompletedEventHandler SubmitGameCompleted;

        public event SubmitGPGNetDataCompletedEventHandler SubmitGPGNetDataCompleted;

        public Service()
        {
            this.Url = Settings.Default.GPG_Multiplayer_Quazal_com_gaspowered_gpgnet1_Service;
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

        [SoapDocumentMethod("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
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

        private void OnSubmitGameOperationCompleted(object arg)
        {
            if (this.SubmitGameCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.SubmitGameCompleted(this, new SubmitGameCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnSubmitGPGNetDataOperationCompleted(object arg)
        {
            if (this.SubmitGPGNetDataCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.SubmitGPGNetDataCompleted(this, new SubmitGPGNetDataCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        [SoapDocumentMethod("http://tempuri.org/SubmitGame", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string SubmitGame(string playername, [XmlElement(DataType="base64Binary")] byte[] data)
        {
            return (string) base.Invoke("SubmitGame", new object[] { playername, data })[0];
        }

        public void SubmitGameAsync(string playername, byte[] data)
        {
            this.SubmitGameAsync(playername, data, null);
        }

        public void SubmitGameAsync(string playername, byte[] data, object userState)
        {
            if (this.SubmitGameOperationCompleted == null)
            {
                this.SubmitGameOperationCompleted = new SendOrPostCallback(this.OnSubmitGameOperationCompleted);
            }
            base.InvokeAsync("SubmitGame", new object[] { playername, data }, this.SubmitGameOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/SubmitGPGNetData", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public string SubmitGPGNetData(string username, string password, string subpath, string filename, [XmlElement(DataType="base64Binary")] byte[] data)
        {
            return (string) base.Invoke("SubmitGPGNetData", new object[] { username, password, subpath, filename, data })[0];
        }

        public void SubmitGPGNetDataAsync(string username, string password, string subpath, string filename, byte[] data)
        {
            this.SubmitGPGNetDataAsync(username, password, subpath, filename, data, null);
        }

        public void SubmitGPGNetDataAsync(string username, string password, string subpath, string filename, byte[] data, object userState)
        {
            if (this.SubmitGPGNetDataOperationCompleted == null)
            {
                this.SubmitGPGNetDataOperationCompleted = new SendOrPostCallback(this.OnSubmitGPGNetDataOperationCompleted);
            }
            base.InvokeAsync("SubmitGPGNetData", new object[] { username, password, subpath, filename, data }, this.SubmitGPGNetDataOperationCompleted, userState);
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

