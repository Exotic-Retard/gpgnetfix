namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.Network;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;

    public class VaultDirectDownloadOperation : IVaultOperation, IActivityMonitor, IProgressMonitor, IStatusProvider, IDisposable
    {
        private IAdditionalContent mContent;
        private bool mDownloading;
        private string mLastStatus;
        private long mPosition;
        private WebClient mWebClient;

        public event EventHandler Finished;

        [field: NonSerialized]
        public event ContentOperationCallback OperationFinished;

        public event ProgressChangeEventHandler ProgressChanged;

        public event StatusProviderEventHandler StatusChanged;

        public VaultDirectDownloadOperation(IAdditionalContent content) : this(content, 0L)
        {
        }

        public VaultDirectDownloadOperation(IAdditionalContent content, long position)
        {
            this.mDownloading = false;
            this.mWebClient = new WebClient();
            this.OperationFinished = null;
            this.mLastStatus = "";
            this.mContent = content;
            this.mPosition = position;
            VaultDownloadOperation.ActiveOperations.Insert(0, this);
            this.mWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.mWebClient_DownloadFileCompleted);
            this.mWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.mWebClient_DownloadProgressChanged);
        }

        public ActivityMonitor CreateActivityMonitor()
        {
            ActivityMonitor monitor = new ActivityMonitor(this.Content, this);
            monitor.CancelOperation += new EventHandler(this.monitor_CancelOperation);
            return monitor;
        }

        public void Dispose()
        {
            this.Stop();
        }

        private void monitor_CancelOperation(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void mWebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.SetStatus("<LOC>Finalizing Download.", new object[0]);
            this.mDownloading = false;
            this.OnDownloadComplete();
            this.SetStatus("<LOC>Download Complete.", 0xbb8, new object[0]);
            if (this.Finished != null)
            {
                this.Finished(this, EventArgs.Empty);
            }
        }

        private void mWebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.mPosition = e.BytesReceived;
            this.mContent.DownloadSize = Convert.ToInt32((long) (e.TotalBytesToReceive / 0x3e8L));
            this.SetStatus("<LOC>Downloading file: {0} / {1} kb", new object[] { this.Position / 0x400L, this.Content.DownloadSize });
            int progress = (int) ((((float) this.Position) / ((float) (this.Content.DownloadSize * 0x400))) * 100f);
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, progress);
            }
        }

        private void OnDownloadComplete()
        {
            if (!this.Content.SaveDownload(this))
            {
                this.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                if (this.OperationFinished != null)
                {
                    this.OperationFinished(new ContentOperationCallbackArgs(this.Content, this, false, new object[0]));
                }
            }
            else if (!new QuazalQuery("DownloadContent", new object[] { this.Content.ID, DataAccess.FormatDate(DateTime.Now.ToUniversalTime()) }).ExecuteNonQuery())
            {
                ErrorLog.WriteLine("Unable to create download record of content {0}.", new object[] { this.Content.Name });
                this.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                if (this.OperationFinished != null)
                {
                    this.OperationFinished(new ContentOperationCallbackArgs(this.Content, this, false, new object[0]));
                }
            }
            else
            {
                IAdditionalContent content = this.Content;
                content.Downloads++;
                new QuazalQuery("IncrementContentDownload", new object[] { this.Content.ID }).ExecuteNonQueryAsync();
                this.SetStatus("<LOC>Download Complete.", 0xbb8, new object[0]);
                if (this.OperationFinished != null)
                {
                    this.OperationFinished(new ContentOperationCallbackArgs(this.Content, this, true, new object[0]));
                }
            }
        }

        public void Resume()
        {
            this.Start();
        }

        public void SetStatus(string status, params object[] args)
        {
            this.SetStatus(status, 0, args);
        }

        public void SetStatus(string status, int timeout, params object[] args)
        {
            VGen0 method = null;
            if (this.StatusChanged != null)
            {
                if ((args != null) && (args.Length > 0))
                {
                    status = string.Format(status, args);
                }
                this.mLastStatus = status;
                if ((Program.MainForm.InvokeRequired && !Program.MainForm.Disposing) && !Program.MainForm.IsDisposed)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.StatusChanged(this, status);
                        };
                    }
                    Program.MainForm.BeginInvoke(method);
                }
                else if (!(Program.MainForm.Disposing || Program.MainForm.IsDisposed))
                {
                    this.StatusChanged(this, status);
                }
            }
        }

        public void Start()
        {
            if (!this.mDownloading)
            {
                int num;
                this.mDownloading = true;
                string downloadPath = this.mContent.GetDownloadPath();
                string[] strArray = this.mContent.GetDirectURL().Split(new char[] { '/' });
                string[] strArray2 = downloadPath.Split(new char[] { '\\' });
                string str2 = "";
                for (num = 0; num < (strArray2.Length - 1); num++)
                {
                    str2 = str2 + strArray2[num] + @"\";
                }
                downloadPath = str2 + strArray[strArray.Length - 1];
                strArray = downloadPath.Split(@"\".ToCharArray());
                string path = "";
                for (num = 0; num < (strArray.Length - 1); num++)
                {
                    path = path + strArray[num] + @"\";
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                this.mWebClient.DownloadFileAsync(new Uri(this.mContent.GetDirectURL()), downloadPath);
            }
        }

        public void Stop()
        {
            if (this.mDownloading)
            {
                this.mWebClient.CancelAsync();
                this.SetStatus("<LOC>Download operation has been cancelled.", new object[0]);
            }
        }

        public IAdditionalContent Content
        {
            get
            {
                return this.mContent;
            }
        }

        public bool IsOperationFinished
        {
            get
            {
                return this.IsProgressFinished;
            }
        }

        public bool IsProgressFinished
        {
            get
            {
                return !this.mDownloading;
            }
        }

        public string LastStatus
        {
            get
            {
                return "Idle";
            }
        }

        public long Position
        {
            get
            {
                return this.mPosition;
            }
        }
    }
}

