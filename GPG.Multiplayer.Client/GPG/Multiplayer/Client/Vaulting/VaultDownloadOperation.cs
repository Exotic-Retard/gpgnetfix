namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.Quazal.com.gaspowered.gpgnet.vault;
    using GPG.Network;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class VaultDownloadOperation : IVaultOperation, IActivityMonitor, IProgressMonitor, IStatusProvider, IDisposable
    {
        public static List<IVaultOperation> ActiveOperations = new List<IVaultOperation>();
        [NonSerialized]
        private string AppCloseWarning;
        private int mChunkSize;
        private IAdditionalContent mContent;
        [NonSerialized]
        private bool mIsDisposed;
        private bool mIsOperationFinished;
        private bool mIsProgressFinished;
        private string mLastStatus;
        private long mPosition;
        private Guid mSessionID;
        private int Progress;
        [NonSerialized]
        private bool RetrievingChunk;
        [NonSerialized]
        private bool ShowAppCloseWarning;
        [NonSerialized]
        private Service WS;

        [field: NonSerialized]
        public event EventHandler Finished;

        [field: NonSerialized]
        public event ContentOperationCallback OperationFinished;

        [field: NonSerialized]
        public event ProgressChangeEventHandler ProgressChanged;

        [field: NonSerialized]
        public event StatusProviderEventHandler StatusChanged;

        public VaultDownloadOperation(IAdditionalContent content) : this(content, 0L)
        {
        }

        public VaultDownloadOperation(IAdditionalContent content, long position)
        {
            this.WS = new Service();
            this.mSessionID = Guid.NewGuid();
            this.mChunkSize = ConfigSettings.GetInt("VaultDownChunkSize", 0x7d000);
            this.Progress = 0;
            this.ShowAppCloseWarning = false;
            this.AppCloseWarning = "";
            this.RetrievingChunk = false;
            this.mProgressChanged = null;
            this.mFinished = null;
            this.mStatusChanged = null;
            this.mIsDisposed = false;
            this.mLastStatus = "<LOC>Idle";
            this.mIsProgressFinished = false;
            this.mIsOperationFinished = false;
            this.mOperationFinished = null;
            this.mContent = content;
            this.mPosition = position;
            ActiveOperations.Insert(0, this);
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
            this.mIsDisposed = true;
        }

        private void GetNextChunk()
        {
            this.RetrievingChunk = true;
            this.SetStatus("<LOC>Downloading file: {0} / {1} kb", new object[] { this.Position / 0x400L, this.Content.DownloadSize });
            this.WS.RequestContentCompleted += new RequestContentCompletedEventHandler(this.WS_RequestContentCompleted);
            this.WS.Timeout = 0x1770;
            this.WS.RequestContentAsync(AdditionalContent.VaultServerKey, this.Content.ContentType.Name, this.Content.Name, this.Content.Version, this.Position, this.ChunkSize, this.SessionID);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.ShowAppCloseWarning)
            {
                if (new DlgYesNo(Program.MainForm, "<LOC>Warning", this.AppCloseWarning + " <LOC>Do you really want to close the application and cancel this operation?").ShowDialog() == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    string downloadPath = this.Content.GetDownloadPath();
                    string str2 = downloadPath + @"\content.partial";
                    foreach (string str3 in Directory.GetFiles(downloadPath, "*.*", SearchOption.TopDirectoryOnly))
                    {
                        if (!(str3 == str2))
                        {
                            File.Delete(str3);
                        }
                    }
                    foreach (string str4 in Directory.GetDirectories(downloadPath))
                    {
                        Directory.Delete(str4, true);
                    }
                }
            }
        }

        private void monitor_CancelOperation(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void OnDownloadComplete()
        {
            this.ShowAppCloseWarning = true;
            this.AppCloseWarning = "<LOC>The vault is in the process of extracting the files out of a downloaded file.";
            if (!this.Content.SaveDownload(this))
            {
                this.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                this.mIsOperationFinished = true;
                if (this.mOperationFinished != null)
                {
                    this.mOperationFinished(new ContentOperationCallbackArgs(this.Content, this, false, new object[0]));
                }
            }
            else if (!new QuazalQuery("DownloadContent", new object[] { this.Content.ID, DataAccess.FormatDate(DateTime.Now.ToUniversalTime()) }).ExecuteNonQuery())
            {
                ErrorLog.WriteLine("Unable to create download record of content {0}.", new object[] { this.Content.Name });
                this.SetStatus("<LOC>An error occurred while downloading file, please try again.", 0xbb8, new object[0]);
                this.mIsOperationFinished = true;
                if (this.mOperationFinished != null)
                {
                    this.mOperationFinished(new ContentOperationCallbackArgs(this.Content, this, false, new object[0]));
                }
            }
            else
            {
                IAdditionalContent content = this.Content;
                content.Downloads++;
                new QuazalQuery("IncrementContentDownload", new object[] { this.Content.ID }).ExecuteNonQueryAsync();
                this.SetStatus("<LOC>Download Complete.", 0xbb8, new object[0]);
                Program.MainForm.FormClosing -= new FormClosingEventHandler(this.MainForm_FormClosing);
                this.mIsOperationFinished = true;
                if (this.mOperationFinished != null)
                {
                    this.mOperationFinished(new ContentOperationCallbackArgs(this.Content, this, true, new object[0]));
                }
            }
        }

        private void Program_Closing(object sender, EventArgs e)
        {
        }

        public void Resume()
        {
            WaitCallback callBack = null;
            WaitCallback callback2 = null;
            if (!this.IsProgressFinished)
            {
                if (callBack == null)
                {
                    callBack = delegate (object s) {
                        this.WS = new Service();
                        ActiveOperations.Add(this);
                        this.Start();
                    };
                }
                ThreadPool.QueueUserWorkItem(callBack);
            }
            else if (!this.IsOperationFinished)
            {
                if (callback2 == null)
                {
                    callback2 = delegate (object s) {
                        string path = this.Content.GetDownloadPath();
                        string str2 = path + @"\content.partial";
                        foreach (string str3 in Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly))
                        {
                            if (!(str3 == str2))
                            {
                                File.Delete(str3);
                            }
                        }
                        foreach (string str4 in Directory.GetDirectories(path))
                        {
                            Directory.Delete(str4, true);
                        }
                        this.WS = new Service();
                        ActiveOperations.Add(this);
                        this.OnDownloadComplete();
                    };
                }
                ThreadPool.QueueUserWorkItem(callback2);
            }
        }

        public void SetStatus(string status, params object[] args)
        {
            this.SetStatus(status, 0, args);
        }

        public void SetStatus(string status, int timeout, params object[] args)
        {
            VGen0 method = null;
            if (this.mStatusChanged != null)
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
                            this.mStatusChanged(this, status);
                        };
                    }
                    Program.MainForm.BeginInvoke(method);
                }
                else if (!(Program.MainForm.Disposing || Program.MainForm.IsDisposed))
                {
                    this.mStatusChanged(this, status);
                }
            }
        }

        public void Start()
        {
            Program.MainForm.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            string downloadPath = this.Content.GetDownloadPath();
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }
            string path = downloadPath + @"\content.partial";
            if (File.Exists(path))
            {
                if (FileUtil.SizeInKB(path) == this.Content.DownloadSize)
                {
                    this.OnDownloadComplete();
                    return;
                }
                FileStream stream = null;
                try
                {
                    stream = File.OpenRead(path);
                    this.mPosition = stream.Length;
                    stream.Close();
                    stream = null;
                    this.Progress = (int) ((((float) this.Position) / ((float) (this.Content.DownloadSize * 0x400))) * 100f);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
            this.GetNextChunk();
        }

        public void Stop()
        {
            EventLog.WriteLine("Stopping download oepration for content: {0}", new object[] { this.Content.Name });
            if (this.RetrievingChunk)
            {
                this.WS.CancelAsync(this.SessionID);
            }
            this.RetrievingChunk = false;
            this.ShowAppCloseWarning = false;
            Program.MainForm.FormClosing -= new FormClosingEventHandler(this.MainForm_FormClosing);
            this.mIsOperationFinished = true;
            if (this.mOperationFinished != null)
            {
                this.mOperationFinished(new ContentOperationCallbackArgs(this.Content, this, false, new object[0]));
            }
        }

        private void WS_RequestContentCompleted(object sender, RequestContentCompletedEventArgs e)
        {
            if (this.RetrievingChunk)
            {
                this.WS.RequestContentCompleted -= new RequestContentCompletedEventHandler(this.WS_RequestContentCompleted);
                this.RetrievingChunk = false;
                if (e.Cancelled)
                {
                    this.SetStatus("<LOC>Download operation has been cancelled.", new object[0]);
                    this.Stop();
                }
                else if (e.Error != null)
                {
                    ErrorLog.WriteLine(e.Error);
                    if (e.Error.Data.Contains("FriendlyMessage"))
                    {
                        this.SetStatus((string) e.Error.Data["FriendlyMessage"], new object[0]);
                    }
                    else
                    {
                        this.SetStatus("<LOC>Error: An unexpected error occurred while downloading, please try again.", new object[0]);
                    }
                    this.Stop();
                }
                else
                {
                    string path = this.Content.GetDownloadPath() + @"\content.partial";
                    FileStream stream = null;
                    try
                    {
                        if (File.Exists(path))
                        {
                            stream = File.OpenWrite(path);
                        }
                        else
                        {
                            stream = File.Create(path);
                        }
                        stream.Position = e.Result.Position;
                        stream.Write(e.Result.Data, 0, e.Result.DataSize);
                        this.mPosition = stream.Position;
                        long length = stream.Length;
                        stream.Close();
                        stream = null;
                        if (e.Result.IsEOF)
                        {
                            this.mIsProgressFinished = true;
                            if (this.mFinished != null)
                            {
                                this.mFinished(this, EventArgs.Empty);
                            }
                            this.OnDownloadComplete();
                        }
                        else
                        {
                            this.Progress = (int) ((((float) length) / ((float) (this.Content.DownloadSize * 0x400))) * 100f);
                            if (this.mProgressChanged != null)
                            {
                                this.mProgressChanged(this, this.Progress);
                            }
                            this.GetNextChunk();
                        }
                    }
                    catch (Exception exception)
                    {
                        this.SetStatus("<LOC>An error occured during download, please try again.", new object[0]);
                        ErrorLog.WriteLine(exception);
                        this.Stop();
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }
                }
            }
        }

        public int ChunkSize
        {
            get
            {
                return this.mChunkSize;
            }
        }

        public IAdditionalContent Content
        {
            get
            {
                return this.mContent;
            }
        }

        public bool IsDisposed
        {
            get
            {
                return this.mIsDisposed;
            }
        }

        public bool IsOperationFinished
        {
            get
            {
                return this.mIsOperationFinished;
            }
        }

        public bool IsProgressFinished
        {
            get
            {
                return this.mIsProgressFinished;
            }
        }

        public string LastStatus
        {
            get
            {
                return this.mLastStatus;
            }
        }

        public long Position
        {
            get
            {
                return this.mPosition;
            }
        }

        public Guid SessionID
        {
            get
            {
                return this.mSessionID;
            }
        }
    }
}

