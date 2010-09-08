namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    [Serializable]
    public class ReplayInfo : MappedObject
    {
        private static Dictionary<Guid, string> ActiveDownloads = new Dictionary<Guid, string>();
        [FieldMap("create_date")]
        private string mCreateDate = "";
        [FieldMap("downloads")]
        private int mDownloads = 0;
        [FieldMap("description")]
        private string mGameInfo = "";
        [FieldMap("game_length")]
        private int mGameLength = 0;
        [FieldMap("game_type")]
        private int mGameType = 0;
        [FieldMap("replay_id")]
        private int mID = 0;
        [FieldMap("keywords")]
        private string mKeywords = "";
        [FieldMap("location")]
        private string mLocation = "";
        [FieldMap("mapname")]
        private string mMapName = "";
        [FieldMap("opponent")]
        private string mOpponent = "";
        [FieldMap("opponent_faction")]
        private int mOpponentFaction = 0;
        [FieldMap("player_faction")]
        private int mPlayerFaction = 0;
        [FieldMap("principal_id")]
        private int mPlayerID = -1;
        [FieldMap("name")]
        private string mPlayerName = "";
        private Image mRatimgImage = null;
        [FieldMap("rating_count")]
        private int mRatingCount = 0;
        [FieldMap("rating_total")]
        private float mRatingTotal = 0f;
        [FieldMap("title")]
        private string mTitle = "";
        [FieldMap("version")]
        private string mVersion = "";

        [field: NonSerialized]
        public event EventHandler DownloadsChanged = null;

        public ReplayInfo(DataRecord record) : base(record)
        {

        }

        private static void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                Guid userState = (Guid) e.UserState;
                string filename = ActiveDownloads[userState];
                ActiveDownloads.Remove(userState);
                ReceieveReplay(filename);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public void Download()
        {
            Download(this);
        }

        public static void Download(ReplayInfo replay)
        {
            Download(replay, replay.Location);
        }

        public static void Download(string location)
        {
            Download(null, location);
        }

        private static void Download(ReplayInfo replay, string location)
        {
            VGen0 method = null;
            WaitCallback callBack = null;
            try
            {
                WebClient client;
                if (Program.MainForm.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            Download(replay, location);
                        };
                    }
                    Program.MainForm.BeginInvoke(method);
                }
                else
                {
                    string uriString = ConfigSettings.GetString("BaseReplayURL", "http://gpgnet.gaspowered.com/replays/") + location;
                    client = new WebClient();
                    string replaysDirectory = Program.Settings.SupcomPrefs.Replays.ReplaysDirectory;
                    if (!Directory.Exists(replaysDirectory))
                    {
                        Directory.CreateDirectory(replaysDirectory);
                    }
                    replaysDirectory = replaysDirectory.TrimEnd(new char[] { '\\' }) + @"\" + location.Replace("/", ".");
                    if (System.IO.File.Exists(replaysDirectory))
                    {
                        ReceieveReplay(replaysDirectory);
                    }
                    else
                    {
                        if (callBack == null)
                        {
                            callBack = delegate (object s) {
                                if (replay != null)
                                {
                                    replay.Downloads++;
                                }
                                DataAccess.ExecuteQuery("IncrementReplayDownload", new object[] { location });
                            };
                        }
                        ThreadQueue.QueueUserWorkItem(callBack, new object[0]);
                        DlgDownloadProgress dlg = new DlgDownloadProgress(location, client);
                        dlg.DownloadCancelled += delegate (object s, EventArgs e) {
                            dlg.Client.CancelAsync();
                            client.DownloadFileCompleted -= new AsyncCompletedEventHandler(ReplayInfo.client_DownloadFileCompleted);
                        };
                        dlg.Show();
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(ReplayInfo.client_DownloadFileCompleted);
                        Guid userToken = Guid.NewGuid();
                        ActiveDownloads[userToken] = replaysDirectory;
                        client.DownloadFileAsync(new Uri(uriString), replaysDirectory + ".gzip", userToken);
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private static void ReceieveReplay(string filename)
        {
            //Exception exception;
            VGen0 method = null;
            try
            {
                if (!System.IO.File.Exists(filename) && System.IO.File.Exists(filename + ".gzip"))
                {
                    FileStream stream = new FileStream(filename + ".gzip", FileMode.Open);
                    FileStream stream2 = new FileStream(filename, FileMode.Create);
                    GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress);
                    byte[] buffer = new byte[0x100];
                    for (int i = stream3.Read(buffer, 0, 0x100); i > 0; i = stream3.Read(buffer, 0, 0x100))
                    {
                        stream2.Write(buffer, 0, i);
                    }
                    stream3.Close();
                    stream.Close();
                    stream2.Close();
                    try
                    {
                        if (System.IO.File.Exists(filename + ".gzip"))
                        {
                            System.IO.File.Delete(filename + ".gzip");
                        }
                    }
                    catch (Exception exception1)
                    {
                        ErrorLog.WriteLine(exception1);
                    }
                }
                if (method == null)
                {
                    method = delegate {
                        //Exception exception;
                        WaitCallback callBack = null;
                        DlgOpenSaveReplay replay = new DlgOpenSaveReplay(filename);
                        switch (replay.ShowDialog())
                        {
                            case DialogResult.OK:
                                switch (replay.Action)
                                {
                                    case ReplayDownloadActions.SaveAs:
                                        try
                                        {
                                            if (System.IO.File.Exists(filename))
                                            {
                                                System.IO.File.Move(filename, replay.SavePath);
                                            }
                                        }
                                        catch (Exception exception1)
                                        {
                                            ErrorLog.WriteLine(exception1);
                                        }
                                        break;

                                    case ReplayDownloadActions.Watch:
                                        if ((System.IO.File.Exists(filename) && (GameInformation.SelectedGame.GameLocation != null)) && (GameInformation.SelectedGame.GameLocation.Length > 0))
                                        {
                                            if (callBack == null)
                                            {
                                                callBack = delegate (object s) {
                                                    try
                                                    {
                                                        Process process = new Process {
                                                            StartInfo = new ProcessStartInfo(GameInformation.SelectedGame.GameLocation, "/replay \"" + filename + "\"")
                                                        };
                                                        process.Start();
                                                        process.WaitForExit();
                                                        if (System.IO.File.Exists(filename))
                                                        {
                                                            System.IO.File.Delete(filename);
                                                        }
                                                    }
                                                    catch (Exception exception)
                                                    {
                                                        ErrorLog.WriteLine(exception);
                                                    }
                                                };
                                            }
                                            ThreadPool.QueueUserWorkItem(callBack);
                                        }
                                        else
                                        {
                                            DlgMessage.Show(Program.MainForm, "<LOC>Error", "<LOC>Unable to locate Supreme Commander executable. Ensure it is set in Settings->SupremeCommander.");
                                        }
                                        break;
                                }
                                break;

                            case DialogResult.Cancel:
                                try
                                {
                                    if (System.IO.File.Exists(filename))
                                    {
                                        System.IO.File.Delete(filename);
                                    }
                                }
                                catch (Exception exception2)
                                {
                                    ErrorLog.WriteLine(exception2);
                                }
                                break;
                        }
                    };
                }
                Program.MainForm.BeginInvoke(method);
            }
            catch (Exception exception2)
            {
                ErrorLog.WriteLine(exception2);
            }
        }

        public string ChatLink
        {
            get
            {
                return Loc.Get("<LOC>Link to Chat");
            }
        }

        public string CreateDate
        {
            get
            {
                return this.mCreateDate;
            }
        }

        public int Downloads
        {
            get
            {
                return this.mDownloads;
            }
            set
            {
                this.mDownloads = value;
                if (this.DownloadsChanged != null)
                {
                    this.DownloadsChanged(this, EventArgs.Empty);
                }
            }
        }

        public string GameInfo
        {
            get
            {
                return this.mGameInfo;
            }
        }

        public TimeSpan GameLength
        {
            get
            {
                return TimeSpan.FromSeconds((double) this.mGameLength);
            }
        }

        public string GameType
        {
            get
            {
                return SupcomLookups.TranslateGameType(this.mGameType);
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public string Keywords
        {
            get
            {
                return this.mKeywords;
            }
        }

        public string Location
        {
            get
            {
                return this.mLocation;
            }
        }

        public string MapName
        {
            get
            {
                if (SupcomLookups.MapNameLookup.ContainsKey(this.mMapName))
                {
                    return SupcomLookups.MapNameLookup[this.mMapName];
                }
                return this.mMapName;
            }
        }

        public string Opponent
        {
            get
            {
                return this.mOpponent;
            }
        }

        internal SupcomLookups._Factions OpponentFaction
        {
            get
            {
                return (SupcomLookups._Factions) this.mOpponentFaction;
            }
        }

        internal SupcomLookups._Factions PlayerFaction
        {
            get
            {
                return (SupcomLookups._Factions) this.mPlayerFaction;
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
        }

        public float Rating
        {
            get
            {
                if (this.RatingCount > 0)
                {
                    return (this.RatingTotal / ((float) this.RatingCount));
                }
                return 0f;
            }
        }

        public int RatingCount
        {
            get
            {
                return this.mRatingCount;
            }
            set
            {
                this.mRatingCount = value;
                this.mRatimgImage = null;
            }
        }

        public Image RatingImage
        {
            get
            {
                if (this.mRatimgImage == null)
                {
                    if (this.Rating == 0f)
                    {
                        return new Bitmap(1, 1);
                    }
                    Image stars = Resources.stars;
                    stars = DrawUtil.ResizeImage(stars, stars.Width / 2, stars.Height / 2);
                    int width = (int) (stars.Width * (this.Rating / 10f));
                    return DrawUtil.CropImageTo(stars, width, stars.Height);
                }
                return this.mRatimgImage;
            }
        }

        public float RatingTotal
        {
            get
            {
                return this.mRatingTotal;
            }
            set
            {
                this.mRatingTotal = value;
                this.mRatimgImage = null;
            }
        }

        public string Title
        {
            get
            {
                return this.mTitle;
            }
        }

        public string Version
        {
            get
            {
                return this.mVersion;
            }
        }
    }
}

