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
    public class VaultUploadOperation : IVaultOperation, IActivityMonitor, IProgressMonitor, IStatusProvider, IDisposable
    {
        public static List<IVaultOperation> ActiveOperations = new List<IVaultOperation>();
        private int mChunkSize;
        private IAdditionalContent mContent;
        private long mFileLength;
        [NonSerialized]
        private bool mIsDisposed;
        private bool mIsOperationFinished;
        private bool mIsProgressFinished;
        private string mLastStatus;
        private long mPosition;
        private Guid mSessionID;
        private int Progress;
        [NonSerialized]
        private bool SubmittingChunk;
        private string UploadFile;
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

        public VaultUploadOperation(IAdditionalContent content) : this(content, 0L)
        {
        }

        public VaultUploadOperation(IAdditionalContent content, long position)
        {
            this.WS = new Service();
            this.mSessionID = Guid.NewGuid();
            this.mChunkSize = ConfigSettings.GetInt("VaultUpChunkSize", 0x7d000);
            this.Progress = 0;
            this.SubmittingChunk = false;
            this.UploadFile = null;
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (new DlgYesNo(Program.MainForm, "<LOC>Warning", "<LOC>The vault is in the process of uploading content. Do you really want to close the application and cancel this operation?").ShowDialog() == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                this.WS.CancelUploadAsync(this.SessionID, AdditionalContent.VaultServerKey);
            }
        }

        private void monitor_CancelOperation(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void OnUploadComplete()
        {
            this.SetStatus("<LOC>Finalizing upload...", new object[0]);
            this.WS.FinalizeUploadCompleted += new FinalizeUploadCompletedEventHandler(this.WS_FinalizeUploadCompleted);
            this.WS.FinalizeUploadAsync(this.SessionID, AdditionalContent.VaultServerKey, this.Content.ContentType.Name, this.Content.Name, this.Content.Version);
        }

        private void Program_Closing(object sender, EventArgs e)
        {
        }

        public void Resume()
        {
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
            this.SubmitNextChunk();
        }

        public void Stop()
        {
            if (this.SubmittingChunk)
            {
                this.WS.CancelAsync(this.SessionID);
            }
            this.SubmittingChunk = false;
            Program.MainForm.FormClosing -= new FormClosingEventHandler(this.MainForm_FormClosing);
            this.mIsOperationFinished = true;
            if (this.mOperationFinished != null)
            {
                this.mOperationFinished(new ContentOperationCallbackArgs(this.Content, this, false, new object[0]));
            }
        }

        private void SubmitNextChunk()
        {
            Exception exception;
            this.SubmittingChunk = true;
            FileStream stream = null;
            try
            {
                if (this.UploadFile == null)
                {
                    this.SetStatus("<LOC>Packaging data...", new object[0]);
                    try
                    {
                        this.UploadFile = this.Content.CreateUploadFile();
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        this.SetStatus(exception.Message, new object[0]);
                        this.Stop();
                        return;
                    }
                }
                if ((this.UploadFile == null) || !File.Exists(this.UploadFile))
                {
                    this.SetStatus("<LOC>Upload operation failed: unable to locate upload file at location {0}, ensure it exists and is not in use.", new object[] { this.UploadFile });
                    if (this.mOperationFinished != null)
                    {
                        this.mOperationFinished(new ContentOperationCallbackArgs(this.Content, this, false, new object[0]));
                    }
                }
                stream = File.OpenRead(this.UploadFile);
                this.mFileLength = stream.Length;
                byte[] buffer = new byte[this.ChunkSize];
                stream.Position = this.Position;
                int dataSize = stream.Read(buffer, 0, this.ChunkSize);
                bool userState = stream.Position == stream.Length;
                this.SetStatus("<LOC>Uploading file: {0} / {1} kb", new object[] { this.Position / 0x400L, this.FileLength / 0x400L });
                this.WS.SubmitContentChunkCompleted += new SubmitContentChunkCompletedEventHandler(this.WS_SubmitContentChunkCompleted);
                this.WS.SubmitContentChunkAsync(this.SessionID, AdditionalContent.VaultServerKey, this.Position, dataSize, buffer, userState);
            }
            catch (Exception exception2)
            {
                exception = exception2;
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

        private void WS_FinalizeUploadCompleted(object sender, FinalizeUploadCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    this.SetStatus("<LOC>Upload operation has been cancelled.", new object[0]);
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
                        this.SetStatus("<LOC>Error: An unexpected error occurred while uploading, please try again.", new object[0]);
                    }
                    this.Stop();
                }
                else
                {
                    string checksum = e.Result.Checksum;
                    this.Content.DownloadSize = e.Result.SizeInKB;
                    string str2 = "";
                    string str3 = "";
                    foreach (int num in this.Content.ContentDependencies)
                    {
                        str2 = str2 + str3 + num.ToString();
                        str3 = ",";
                    }
                    if (!DataAccess.ExecuteQuery("UploadContent", new object[] { this.Content.ContentType.ID, this.Content.Name, this.Content.Description, this.Content.DownloadSize, this.Content.SearchKeywords, this.Content.Version, this.Content.VersionNotes, checksum, this.Content.DownloadVolunteerEffort, str2 }))
                    {
                        ErrorLog.WriteLine("Upload query failed for general content: {0}.", new object[] { this.Content.Name });
                        this.SetStatus("<LOC>An error occured during upload.", new object[0]);
                        IAdditionalContent content = this.Content;
                        content.Version--;
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
                            number = DataAccess.GetNumber("GetContentID", new object[] { this.Content.ContentType.ID, this.Content.Name, this.Content.Version });
                            num3++;
                        }
                        while ((number <= 0) && (num3 < ConfigSettings.GetInt("InsertPollDuration", 12)));
                        if (number <= 0)
                        {
                            ErrorLog.WriteLine("Unable to retrieve recently uploaded content ID: {0}.", new object[] { this.Content.Name });
                            this.WS.DeleteContentAsync(AdditionalContent.VaultServerKey, this.Content.ContentType.Name, this.Content.Name, 1, false);
                            this.SetStatus("<LOC>An error occured during upload.", new object[0]);
                            IAdditionalContent content2 = this.Content;
                            content2.Version--;
                            this.Stop();
                        }
                        else
                        {
                            this.Content.ID = number;
                            if (!this.Content.SaveDownloadData())
                            {
                                ErrorLog.WriteLine("Upload query failed for additional content: {0}.", new object[] { this.Content.Name });
                                DataAccess.ExecuteQuery("DeleteUploadedContent", new object[] { number });
                                this.WS.DeleteContentAsync(AdditionalContent.VaultServerKey, this.Content.ContentType.Name, this.Content.Name, this.Content.Version, false);
                                this.SetStatus("<LOC>An error occured during upload.", new object[0]);
                                IAdditionalContent content3 = this.Content;
                                content3.Version--;
                                this.Stop();
                            }
                            else
                            {
                                this.Content.OwnerID = User.Current.ID;
                                this.SetStatus("<LOC>Upload complete.", new object[0]);
                                Program.MainForm.FormClosing -= new FormClosingEventHandler(this.MainForm_FormClosing);
                                this.mIsOperationFinished = true;
                                if (this.mOperationFinished != null)
                                {
                                    this.mOperationFinished(new ContentOperationCallbackArgs(this.Content, this, true, new object[0]));
                                }
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

        private void WS_SubmitContentChunkCompleted(object sender, SubmitContentChunkCompletedEventArgs e)
        {
            if (this.SubmittingChunk)
            {
                this.WS.SubmitContentChunkCompleted -= new SubmitContentChunkCompletedEventHandler(this.WS_SubmitContentChunkCompleted);
                this.SubmittingChunk = false;
                if (e.Cancelled)
                {
                    this.SetStatus("<LOC>Upload operation has been cancelled.", new object[0]);
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
                        this.SetStatus("<LOC>Error: An unexpected error occurred while uploading, please try again.", new object[0]);
                    }
                    this.Stop();
                }
                else
                {
                    bool userState = (bool) e.UserState;
                    this.mPosition = e.Result.Position + e.Result.DataSize;
                    if (userState)
                    {
                        this.mIsProgressFinished = true;
                        if (this.mFinished != null)
                        {
                            this.mFinished(this, EventArgs.Empty);
                        }
                        this.OnUploadComplete();
                    }
                    else
                    {
                        this.Progress = (int) ((((float) this.Position) / ((float) this.FileLength)) * 100f);
                        if (this.mProgressChanged != null)
                        {
                            this.mProgressChanged(this, this.Progress);
                        }
                        this.SubmitNextChunk();
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

        public long FileLength
        {
            get
            {
                return this.mFileLength;
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

