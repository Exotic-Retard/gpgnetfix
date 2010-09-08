namespace GPG.Multiplayer.Client.Vaulting
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Vaulting.Applications;
    using GPG.Multiplayer.Quazal;
    using GPG.Network;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    internal class VaultFTPUploadOperation : IVaultOperation, IActivityMonitor, IProgressMonitor, IStatusProvider, IDisposable
    {
        private string mChksum;
        private IAdditionalContent mContent;
        private bool mIsOperationFinished;
        private bool mIsProgressFinished;
        private string mLastStatus;
        private int mOutCount;
        private long mPosition;
        private Thread mWorkThread;

        public event EventHandler Finished;

        public event ContentOperationCallback OperationFinished;

        public event ProgressChangeEventHandler ProgressChanged;

        [field: NonSerialized]
        public event StatusProviderEventHandler StatusChanged;

        public VaultFTPUploadOperation(IAdditionalContent content) : this(content, 0L)
        {
        }

        public VaultFTPUploadOperation(IAdditionalContent content, long position)
        {
            this.mContent = null;
            this.mPosition = 0L;
            this.mWorkThread = null;
            this.mChksum = "";
            this.mOutCount = 0;
            this.StatusChanged = null;
            this.mContent = content;
            this.mPosition = position;
            VaultUploadOperation.ActiveOperations.Insert(0, this);
        }

        private void CompleteUpload()
        {
            try
            {
                string str = "";
                string str2 = "";
                foreach (int num in this.mContent.ContentDependencies)
                {
                    str = str + str2 + num.ToString();
                    str2 = ",";
                }
                if (!DataAccess.ExecuteQuery("UploadContent", new object[] { this.mContent.ContentType.ID, this.mContent.Name, this.mContent.Description, this.mContent.DownloadSize, this.mContent.SearchKeywords, this.mContent.Version, this.mContent.VersionNotes, this.mChksum, this.mContent.DownloadVolunteerEffort, str }))
                {
                    ErrorLog.WriteLine("Upload query failed for general content: {0}.", new object[] { this.mContent.Name });
                    this.SetStatus("<LOC>An error occured during upload.", new object[0]);
                    this.mContent.Version--;
                    this.Stop();
                }
                else
                {
                    int number = -1;
                    int num3 = 0;
                    do
                    {
                        if (num3 > 0)
                        {
                            Thread.Sleep(500);
                        }
                        number = DataAccess.GetNumber("GetContentID", new object[] { this.mContent.ContentType.ID, this.mContent.Name, this.mContent.Version });
                        num3++;
                    }
                    while ((number <= 0) && (num3 < ConfigSettings.GetInt("InsertPollDuration", 12)));
                    if (number <= 0)
                    {
                        ErrorLog.WriteLine("Unable to retrieve recently uploaded content ID: {0}.", new object[] { this.mContent.Name });
                        this.SetStatus("<LOC>An error occured during upload.", new object[0]);
                        this.mContent.Version--;
                        this.Stop();
                    }
                    else
                    {
                        this.mContent.ID = number;
                        if (!this.mContent.SaveDownloadData())
                        {
                            ErrorLog.WriteLine("Upload query failed for additional content: {0}.", new object[] { this.mContent.Name });
                            DataAccess.ExecuteQuery("DeleteUploadedContent", new object[] { number });
                            this.SetStatus("<LOC>An error occured during upload.", new object[0]);
                            this.mContent.Version--;
                            this.Stop();
                        }
                        else
                        {
                            this.mContent.OwnerID = User.Current.ID;
                            this.SetStatus("<LOC>Upload complete.", new object[0]);
                            this.mIsOperationFinished = true;
                            if (this.OperationFinished != null)
                            {
                                this.OperationFinished(new ContentOperationCallbackArgs(this.mContent, this, true, new object[0]));
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                this.SetStatus("<LOC>An error occured during upload.", new object[0]);
                this.Stop();
            }
        }

        public ActivityMonitor CreateActivityMonitor()
        {
            ActivityMonitor monitor = new ActivityMonitor(this.mContent, this);
            monitor.CancelOperation += new EventHandler(this.monitor_CancelOperation);
            return monitor;
        }

        public void Dispose()
        {
            this.Stop();
        }

        private void DoWork()
        {
            IFTPInfo mContent = this.mContent as IFTPInfo;
            if (((this.mContent.GetDirectURL() != null) && (this.mContent.GetDirectURL() != "")) && mContent.AlreadyUploaded())
            {
                if (this.OperationFinished != null)
                {
                    this.OperationFinished(new ContentOperationCallbackArgs(this.mContent, this, false, new object[0]));
                }
                if (this.Finished != null)
                {
                    this.Finished(this, EventArgs.Empty);
                }
                this.SetStatus("Already Uploaded.", new object[0]);
                this.mWorkThread = null;
            }
            this.SetStatus("Creating FTP Upload Info", new object[0]);
            this.mPosition = 0L;
            this.IncPosition();
            string path = Path.GetTempPath() + "upload.txt";
            string str2 = Path.GetTempPath() + "upload.bat";
            try
            {
                this.IncPosition();
                StreamWriter writer = new StreamWriter(path, false);
                writer.WriteLine(mContent.GetFTPUser());
                writer.WriteLine(mContent.GetFTPPass());
                writer.WriteLine("bin");
                writer.WriteLine("hash");
                if (mContent.GetFTPDirectory() != "")
                {
                    writer.WriteLine("cd " + mContent.GetFTPDirectory());
                }
                this.SetStatus("Creating Upload File", new object[0]);
                this.IncPosition();
                string fileName = this.mContent.CreateUploadFile();
                this.mContent.DownloadSize = Convert.ToInt32((long) (new FileInfo(fileName).Length / 0x3e8L));
                this.mChksum = AppUtils.ChkSumFile(fileName);
                FileInfo info2 = new FileInfo(fileName);
                writer.WriteLine("put \"" + info2.Name + "\"");
                writer.WriteLine("quit");
                writer.Close();
                writer = new StreamWriter(str2, false);
                writer.WriteLine("cd " + Path.GetTempPath());
                writer.WriteLine("ftp -s:upload.txt " + mContent.GetFTPServer());
                writer.WriteLine("pause");
                writer.Close();
                this.SetStatus("Performing FTP Upload", new object[0]);
                this.IncPosition();
                ProcessStartInfo startInfo = new ProcessStartInfo(str2);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo).WaitForExit();
                this.IncPosition();
                File.Delete(path);
                this.IncPosition();
                this.CompleteUpload();
                if (this.Finished != null)
                {
                    this.Finished(this, EventArgs.Empty);
                }
                this.SetStatus("Upload Complete", new object[0]);
                this.mWorkThread = null;
            }
            catch (ThreadInterruptedException)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        private void IncPosition()
        {
            this.mPosition += 1L;
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, Convert.ToInt32((long) ((this.mPosition / 6L) * 100L)));
            }
        }

        private void monitor_CancelOperation(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.mOutCount++;
            this.SetStatus(this.mOutCount.ToString() + ". " + e.Data, new object[0]);
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
            this.SetStatus("Creating Upload File", new object[0]);
            string contentfile = this.mContent.CreateUploadFile();
            Program.MainForm.Invoke((VGen0)delegate {
                Clipboard.SetText(contentfile);
                this.DoWork();
                this.CompleteUpload();
                if (this.Finished != null)
                {
                    this.Finished(this, EventArgs.Empty);
                }
                this.SetStatus("Upload Complete", new object[0]);
            });
        }

        public void Stop()
        {
            if (this.mWorkThread != null)
            {
                this.mWorkThread.Interrupt();
                this.mWorkThread = null;
            }
        }

        public bool IsOperationFinished
        {
            get
            {
                return this.mIsOperationFinished;
            }
            set
            {
                this.mIsOperationFinished = value;
            }
        }

        public bool IsProgressFinished
        {
            get
            {
                return this.mIsProgressFinished;
            }
            set
            {
                this.mIsProgressFinished = value;
            }
        }

        public string LastStatus
        {
            get
            {
                return this.mLastStatus;
            }
            set
            {
                this.mLastStatus = value;
            }
        }
    }
}

