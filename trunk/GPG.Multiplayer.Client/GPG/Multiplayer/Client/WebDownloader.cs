namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class WebDownloader
    {
        public event AsyncCompletedEventHandler OnDownloadCompleted;

        public void BeginDownloadFile(string weburl, string destfile, bool showprogress)
        {
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                WebClient client = new WebClient();
                if (weburl != "")
                {
                    EventLog.WriteLine("Download File", new object[0]);
                    EventLog.WriteLine(destfile, new object[0]);
                    EventLog.WriteLine(weburl, new object[0]);
                    try
                    {
                        if (showprogress)
                        {
                            Program.MainForm.Invoke((VGen1)delegate (object objclient) {
                                try
                                {
                                    new DlgDownloadProgress(Loc.Get("<LOC>Downloading"), objclient as WebClient).Show();
                                }
                                catch (Exception exception)
                                {
                                    ErrorLog.WriteLine(exception);
                                }
                            }, new object[] { client });
                        }
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.client_DownloadFileCompleted);
                        client.DownloadFileAsync(new Uri(weburl), destfile);
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                    }
                }
            });
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (this.OnDownloadCompleted != null)
            {
                this.OnDownloadCompleted(sender, e);
            }
        }
    }
}

