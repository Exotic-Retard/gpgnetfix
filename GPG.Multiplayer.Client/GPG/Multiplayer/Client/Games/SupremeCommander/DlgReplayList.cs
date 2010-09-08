namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.Utils;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    public class DlgReplayList : DlgBase
    {
        private SkinButton btnCancel;
        private SkinButton btnOK;
        private GridColumn colGameDate;
        private GridColumn colGameType;
        private GridColumn colMap;
        private GridColumn colPlayerName;
        private GridColumn colPlayers;
        private GridColumn colTitle;
        private IContainer components = null;
        private GPGDataGrid dgReplays;
        private GridView gvReplays;
        private GPGLabel lPercentDownloaded;
        private MappedObjectList<ReplayInfo> mData;

        private DlgReplayList()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            WaitCallback callBack = null;
            if (this.gvReplays.GetSelectedRows().Length > 0)
            {
                ReplayInfo row = this.gvReplays.GetRow(this.gvReplays.GetSelectedRows()[0]) as ReplayInfo;
                base.Enabled = false;
                if (callBack == null)
                {
                    callBack = delegate (object o) {
                        try
                        {
                            ReplayInfo info = (o as object[])[0] as ReplayInfo;
                            if (info != null)
                            {
                                string address = ConfigSettings.GetString("BaseReplayURL", "http://gpgnet.gaspowered.com/replays/") + info.Location;
                                WebClient client = new WebClient();
                                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Gas Powered Games\SupremeCommander\replays\" + User.Current.Name;
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                path = path + @"\" + info.Location.Replace("/", ".");
                                if (!System.IO.File.Exists(path))
                                {
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.client_DownloadProgressChanged);
                                    client.DownloadFile(address, path + ".gzip");
                                    FileStream stream = new FileStream(path + ".gzip", FileMode.Open);
                                    FileStream stream2 = new FileStream(path, FileMode.Create);
                                    GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress);
                                    byte[] buffer = new byte[0x100];
                                    for (int j = stream3.Read(buffer, 0, 0x100); j > 0; j = stream3.Read(buffer, 0, 0x100))
                                    {
                                        stream2.Write(buffer, 0, j);
                                    }
                                    stream3.Close();
                                    stream.Close();
                                    stream2.Close();
                                }
                                if (System.IO.File.Exists(path))
                                {
                                    Process.Start(path);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            ErrorLog.WriteLine(exception);
                        }
                        base.BeginInvoke((VGen0)delegate {
                            base.Close();
                        });
                    };
                }
                ThreadQueue.QueueUserWorkItem(callBack, new object[] { row });
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            VGen1 method = null;
            if (!base.IsDisposed && !base.Disposing)
            {
                if (method == null)
                {
                    method = delegate (object objpercent) {
                        int num = (int) objpercent;
                        this.lPercentDownloaded.Visible = true;
                        this.lPercentDownloaded.Text = Loc.Get("<LOC>Downloading:") + " " + num.ToString() + "%";
                    };
                }
                base.BeginInvoke(method, new object[] { e.ProgressPercentage });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgReplays = new GPGDataGrid();
            this.gvReplays = new GridView();
            this.colPlayerName = new GridColumn();
            this.colTitle = new GridColumn();
            this.colMap = new GridColumn();
            this.colGameDate = new GridColumn();
            this.colGameType = new GridColumn();
            this.colPlayers = new GridColumn();
            this.btnCancel = new SkinButton();
            this.btnOK = new SkinButton();
            this.lPercentDownloaded = new GPGLabel();
            this.dgReplays.BeginInit();
            this.gvReplays.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.dgReplays.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dgReplays.CustomizeStyle = false;
            this.dgReplays.EmbeddedNavigator.Name = "";
            this.dgReplays.Location = new Point(0x1f, 0x5e);
            this.dgReplays.MainView = this.gvReplays;
            this.dgReplays.Name = "dgReplays";
            this.dgReplays.ShowOnlyPredefinedDetails = true;
            this.dgReplays.Size = new Size(0x381, 0x12d);
            this.dgReplays.TabIndex = 7;
            this.dgReplays.ViewCollection.AddRange(new BaseView[] { this.gvReplays });
            this.gvReplays.ActiveFilterString = "";
            this.gvReplays.Appearance.Preview.ForeColor = Color.Silver;
            this.gvReplays.Appearance.Preview.Options.UseForeColor = true;
            this.gvReplays.Columns.AddRange(new GridColumn[] { this.colPlayerName, this.colTitle, this.colMap, this.colGameDate, this.colGameType, this.colPlayers });
            this.gvReplays.GridControl = this.dgReplays;
            this.gvReplays.GroupPanelText = "<LOC>Select a replay to watch.";
            this.gvReplays.Name = "gvReplays";
            this.gvReplays.OptionsBehavior.Editable = false;
            this.gvReplays.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReplays.OptionsView.AutoCalcPreviewLineCount = true;
            this.gvReplays.OptionsView.RowAutoHeight = true;
            this.gvReplays.OptionsView.ShowPreview = true;
            this.gvReplays.PreviewFieldName = "GameInfo";
            this.colPlayerName.Caption = "<LOC>Player Name";
            this.colPlayerName.FieldName = "PlayerName";
            this.colPlayerName.Name = "colPlayerName";
            this.colPlayerName.Visible = true;
            this.colPlayerName.VisibleIndex = 5;
            this.colTitle.Caption = "<LOC>Title";
            this.colTitle.FieldName = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.Visible = true;
            this.colTitle.VisibleIndex = 3;
            this.colMap.Caption = "<LOC>MapName";
            this.colMap.FieldName = "MapName";
            this.colMap.Name = "colMap";
            this.colMap.Visible = true;
            this.colMap.VisibleIndex = 0;
            this.colGameDate.Caption = "<LOC>Game Date";
            this.colGameDate.FieldName = "CreateDate";
            this.colGameDate.Name = "colGameDate";
            this.colGameDate.Visible = true;
            this.colGameDate.VisibleIndex = 1;
            this.colGameType.Caption = "<LOC>Game Type";
            this.colGameType.FieldName = "GameType";
            this.colGameType.Name = "colGameType";
            this.colGameType.Visible = true;
            this.colGameType.VisibleIndex = 2;
            this.colPlayers.Caption = "<LOC>Players Involved";
            this.colPlayers.FieldName = "Players";
            this.colPlayers.Name = "colPlayers";
            this.colPlayers.Visible = true;
            this.colPlayers.VisibleIndex = 4;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.AutoStyle = true;
            this.btnCancel.BackColor = Color.Black;
            this.btnCancel.DialogResult = DialogResult.OK;
            this.btnCancel.DisabledForecolor = Color.Gray;
            this.btnCancel.DrawEdges = true;
            this.btnCancel.FocusColor = Color.Yellow;
            this.btnCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.btnCancel.IsStyled = true;
            this.btnCancel.Location = new Point(0x33f, 0x18f);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x61, 0x1a);
            this.btnCancel.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnCancel.TabIndex = 0x17;
            this.btnCancel.Text = "<LOC>Cancel";
            this.btnCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.btnCancel.TextPadding = new Padding(0);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.AutoStyle = true;
            this.btnOK.BackColor = Color.Black;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.DisabledForecolor = Color.Gray;
            this.btnOK.DrawEdges = true;
            this.btnOK.FocusColor = Color.Yellow;
            this.btnOK.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.HorizontalScalingMode = ScalingModes.Tile;
            this.btnOK.IsStyled = true;
            this.btnOK.Location = new Point(0x2d5, 0x18f);
            this.btnOK.MaximumSize = new Size(100, 0x1a);
            this.btnOK.MinimumSize = new Size(100, 0x1a);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(100, 0x1a);
            this.btnOK.SkinBasePath = @"Controls\Button\Round Edge";
            this.btnOK.TabIndex = 0x18;
            this.btnOK.Text = "<LOC>OK";
            this.btnOK.TextAlign = ContentAlignment.MiddleCenter;
            this.btnOK.TextPadding = new Padding(0);
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.lPercentDownloaded.AutoSize = true;
            this.lPercentDownloaded.AutoStyle = true;
            this.lPercentDownloaded.Font = new Font("Arial", 9.75f);
            this.lPercentDownloaded.ForeColor = Color.White;
            this.lPercentDownloaded.IgnoreMouseWheel = false;
            this.lPercentDownloaded.IsStyled = false;
            this.lPercentDownloaded.Location = new Point(0x1c, 0x4b);
            this.lPercentDownloaded.Name = "lPercentDownloaded";
            this.lPercentDownloaded.Size = new Size(0x7f, 0x10);
            this.lPercentDownloaded.TabIndex = 0x19;
            this.lPercentDownloaded.Text = "<LOC>Downloading:";
            this.lPercentDownloaded.TextStyle = TextStyles.Default;
            this.lPercentDownloaded.Visible = false;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(960, 0x1e8);
            base.Controls.Add(this.lPercentDownloaded);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.dgReplays);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgReplayList";
            this.Text = "<LOC>Select a replay to watch.";
            base.Controls.SetChildIndex(this.dgReplays, 0);
            base.Controls.SetChildIndex(this.btnOK, 0);
            base.Controls.SetChildIndex(this.btnCancel, 0);
            base.Controls.SetChildIndex(this.lPercentDownloaded, 0);
            this.dgReplays.EndInit();
            this.gvReplays.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public static void ShowPlayerReplayList(int playerid)
        {
            DlgReplayList list = new DlgReplayList();
            list.mData = DataAccess.GetObjects<ReplayInfo>("GetPlayerReplays1", new object[] { playerid });
            list.dgReplays.DataSource = list.mData;
            list.Show();
        }
    }
}

