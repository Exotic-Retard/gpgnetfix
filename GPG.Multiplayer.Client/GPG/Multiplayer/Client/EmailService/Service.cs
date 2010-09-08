namespace GPG.Multiplayer.Client.EmailService
{
    using GPG.Multiplayer.Client.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Web.Services;
    using System.Web.Services.Description;
    using System.Web.Services.Protocols;

    [DebuggerStepThrough, WebServiceBinding(Name="ServiceSoap", Namespace="http://tempuri.org/"), DesignerCategory("code"), GeneratedCode("System.Web.Services", "2.0.50727.1433")]
    public class Service : SoapHttpClientProtocol
    {
        private SendOrPostCallback ReportErrorOperationCompleted;
        private SendOrPostCallback SendEmailOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event ReportErrorCompletedEventHandler ReportErrorCompleted;

        public event SendEmailCompletedEventHandler SendEmailCompleted;

        public Service()
        {
            this.Url = Settings.Default.GPG_Multiplayer_Client_EmailService_Service;
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

        private bool IsLocalFileSystemWebService(string url)
        {
            if ((url == null) || (url == string.Empty))
            {
                return false;
            }
            Uri uri = new Uri(url);
            return ((uri.Port >= 0x400) && (string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0));
        }

        private void OnReportErrorOperationCompleted(object arg)
        {
            if (this.ReportErrorCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.ReportErrorCompleted(this, new AsyncCompletedEventArgs(args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnSendEmailOperationCompleted(object arg)
        {
            if (this.SendEmailCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.SendEmailCompleted(this, new AsyncCompletedEventArgs(args.Error, args.Cancelled, args.UserState));
            }
        }

        [SoapDocumentMethod("http://tempuri.org/ReportError", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void ReportError(string username, string log, string exception)
        {
            base.Invoke("ReportError", new object[] { username, log, exception });
        }

        public void ReportErrorAsync(string username, string log, string exception)
        {
            this.ReportErrorAsync(username, log, exception, null);
        }

        public void ReportErrorAsync(string username, string log, string exception, object userState)
        {
            if (this.ReportErrorOperationCompleted == null)
            {
                this.ReportErrorOperationCompleted = new SendOrPostCallback(this.OnReportErrorOperationCompleted);
            }
            base.InvokeAsync("ReportError", new object[] { username, log, exception }, this.ReportErrorOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/SendEmail", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void SendEmail(string destemail, string content, string replyemail, string subject)
        {
            base.Invoke("SendEmail", new object[] { destemail, content, replyemail, subject });
        }

        public void SendEmailAsync(string destemail, string content, string replyemail, string subject)
        {
            this.SendEmailAsync(destemail, content, replyemail, subject, null);
        }

        public void SendEmailAsync(string destemail, string content, string replyemail, string subject, object userState)
        {
            if (this.SendEmailOperationCompleted == null)
            {
                this.SendEmailOperationCompleted = new SendOrPostCallback(this.OnSendEmailOperationCompleted);
            }
            base.InvokeAsync("SendEmail", new object[] { destemail, content, replyemail, subject }, this.SendEmailOperationCompleted, userState);
        }

        public string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if (!((!this.IsLocalFileSystemWebService(base.Url) || this.useDefaultCredentialsSetExplicitly) || this.IsLocalFileSystemWebService(value)))
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

