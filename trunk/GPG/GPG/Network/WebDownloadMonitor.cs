namespace GPG.Network
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.CompilerServices;

    public class WebDownloadMonitor : IProgressMonitor
    {
        private WebClient Client;

        public event EventHandler Finished;

        public event ProgressChangeEventHandler ProgressChanged;

        public WebDownloadMonitor(WebClient webClient)
        {
            this.Client = webClient;
            this.Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.Client_DownloadProgressChanged);
            this.Client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Client_DownloadFileCompleted);
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (this.Finished != null)
            {
                this.Finished(this, EventArgs.Empty);
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, e.ProgressPercentage);
            }
        }
    }
}

