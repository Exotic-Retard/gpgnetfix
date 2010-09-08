namespace GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault
{
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

    [DebuggerStepThrough, DesignerCategory("code"), WebServiceBinding(Name="ServiceSoap", Namespace="http://tempuri.org/"), GeneratedCode("System.Web.Services", "2.0.50727.1433")]
    public class Service : SoapHttpClientProtocol
    {
        private SendOrPostCallback CancelUploadOperationCompleted;
        private SendOrPostCallback DeleteContentOperationCompleted;
        private SendOrPostCallback FinalizeUploadOperationCompleted;
        private SendOrPostCallback GetPreviewImageOperationCompleted;
        private SendOrPostCallback RequestContentOperationCompleted;
        private SendOrPostCallback SubmitContentChunkOperationCompleted;
        private bool useDefaultCredentialsSetExplicitly;

        public event CancelUploadCompletedEventHandler CancelUploadCompleted;

        public event DeleteContentCompletedEventHandler DeleteContentCompleted;

        public event FinalizeUploadCompletedEventHandler FinalizeUploadCompleted;

        public event GetPreviewImageCompletedEventHandler GetPreviewImageCompleted;

        public event RequestContentCompletedEventHandler RequestContentCompleted;

        public event SubmitContentChunkCompletedEventHandler SubmitContentChunkCompleted;

        public Service()
        {
            this.Url = "http://thevault.gaspowered.com/vault/Service.asmx";
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

        [SoapDocumentMethod("http://tempuri.org/CancelUpload", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void CancelUpload(Guid sessionId, string server)
        {
            base.Invoke("CancelUpload", new object[] { sessionId, server });
        }

        public void CancelUploadAsync(Guid sessionId, string server)
        {
            this.CancelUploadAsync(sessionId, server, null);
        }

        public void CancelUploadAsync(Guid sessionId, string server, object userState)
        {
            if (this.CancelUploadOperationCompleted == null)
            {
                this.CancelUploadOperationCompleted = new SendOrPostCallback(this.OnCancelUploadOperationCompleted);
            }
            base.InvokeAsync("CancelUpload", new object[] { sessionId, server }, this.CancelUploadOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/DeleteContent", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public void DeleteContent(string server, string contentType, string name, int version, bool allVersions)
        {
            base.Invoke("DeleteContent", new object[] { server, contentType, name, version, allVersions });
        }

        public void DeleteContentAsync(string server, string contentType, string name, int version, bool allVersions)
        {
            this.DeleteContentAsync(server, contentType, name, version, allVersions, null);
        }

        public void DeleteContentAsync(string server, string contentType, string name, int version, bool allVersions, object userState)
        {
            if (this.DeleteContentOperationCompleted == null)
            {
                this.DeleteContentOperationCompleted = new SendOrPostCallback(this.OnDeleteContentOperationCompleted);
            }
            base.InvokeAsync("DeleteContent", new object[] { server, contentType, name, version, allVersions }, this.DeleteContentOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/FinalizeUpload", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public ContentUploadResult FinalizeUpload(Guid sessionId, string server, string contentType, string name, int version)
        {
            return (ContentUploadResult) base.Invoke("FinalizeUpload", new object[] { sessionId, server, contentType, name, version })[0];
        }

        public void FinalizeUploadAsync(Guid sessionId, string server, string contentType, string name, int version)
        {
            this.FinalizeUploadAsync(sessionId, server, contentType, name, version, null);
        }

        public void FinalizeUploadAsync(Guid sessionId, string server, string contentType, string name, int version, object userState)
        {
            if (this.FinalizeUploadOperationCompleted == null)
            {
                this.FinalizeUploadOperationCompleted = new SendOrPostCallback(this.OnFinalizeUploadOperationCompleted);
            }
            base.InvokeAsync("FinalizeUpload", new object[] { sessionId, server, contentType, name, version }, this.FinalizeUploadOperationCompleted, userState);
        }

        [return: XmlElement(DataType="base64Binary")]
        [SoapDocumentMethod("http://tempuri.org/GetPreviewImage", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public byte[] GetPreviewImage(string server, string contentType, string name, int version)
        {
            return (byte[]) base.Invoke("GetPreviewImage", new object[] { server, contentType, name, version })[0];
        }

        public void GetPreviewImageAsync(string server, string contentType, string name, int version)
        {
            this.GetPreviewImageAsync(server, contentType, name, version, null);
        }

        public void GetPreviewImageAsync(string server, string contentType, string name, int version, object userState)
        {
            if (this.GetPreviewImageOperationCompleted == null)
            {
                this.GetPreviewImageOperationCompleted = new SendOrPostCallback(this.OnGetPreviewImageOperationCompleted);
            }
            base.InvokeAsync("GetPreviewImage", new object[] { server, contentType, name, version }, this.GetPreviewImageOperationCompleted, userState);
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

        private void OnCancelUploadOperationCompleted(object arg)
        {
            if (this.CancelUploadCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.CancelUploadCompleted(this, new AsyncCompletedEventArgs(args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnDeleteContentOperationCompleted(object arg)
        {
            if (this.DeleteContentCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.DeleteContentCompleted(this, new AsyncCompletedEventArgs(args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnFinalizeUploadOperationCompleted(object arg)
        {
            if (this.FinalizeUploadCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.FinalizeUploadCompleted(this, new FinalizeUploadCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnGetPreviewImageOperationCompleted(object arg)
        {
            if (this.GetPreviewImageCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.GetPreviewImageCompleted(this, new GetPreviewImageCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnRequestContentOperationCompleted(object arg)
        {
            if (this.RequestContentCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.RequestContentCompleted(this, new RequestContentCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        private void OnSubmitContentChunkOperationCompleted(object arg)
        {
            if (this.SubmitContentChunkCompleted != null)
            {
                InvokeCompletedEventArgs args = (InvokeCompletedEventArgs) arg;
                this.SubmitContentChunkCompleted(this, new SubmitContentChunkCompletedEventArgs(args.Results, args.Error, args.Cancelled, args.UserState));
            }
        }

        [SoapDocumentMethod("http://tempuri.org/RequestContent", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public ChunkDownloadResult RequestContent(string server, string contentType, string name, int version, long position, int chunkSize)
        {
            return (ChunkDownloadResult) base.Invoke("RequestContent", new object[] { server, contentType, name, version, position, chunkSize })[0];
        }

        public void RequestContentAsync(string server, string contentType, string name, int version, long position, int chunkSize)
        {
            this.RequestContentAsync(server, contentType, name, version, position, chunkSize, null);
        }

        public void RequestContentAsync(string server, string contentType, string name, int version, long position, int chunkSize, object userState)
        {
            if (this.RequestContentOperationCompleted == null)
            {
                this.RequestContentOperationCompleted = new SendOrPostCallback(this.OnRequestContentOperationCompleted);
            }
            base.InvokeAsync("RequestContent", new object[] { server, contentType, name, version, position, chunkSize }, this.RequestContentOperationCompleted, userState);
        }

        [SoapDocumentMethod("http://tempuri.org/SubmitContentChunk", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
        public ChunkUploadResult SubmitContentChunk(Guid sessionId, string server, long position, int dataSize, [XmlElement(DataType="base64Binary")] byte[] data)
        {
            return (ChunkUploadResult) base.Invoke("SubmitContentChunk", new object[] { sessionId, server, position, dataSize, data })[0];
        }

        public void SubmitContentChunkAsync(Guid sessionId, string server, long position, int dataSize, byte[] data)
        {
            this.SubmitContentChunkAsync(sessionId, server, position, dataSize, data, null);
        }

        public void SubmitContentChunkAsync(Guid sessionId, string server, long position, int dataSize, byte[] data, object userState)
        {
            if (this.SubmitContentChunkOperationCompleted == null)
            {
                this.SubmitContentChunkOperationCompleted = new SendOrPostCallback(this.OnSubmitContentChunkOperationCompleted);
            }
            base.InvokeAsync("SubmitContentChunk", new object[] { sessionId, server, position, dataSize, data }, this.SubmitContentChunkOperationCompleted, userState);
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

