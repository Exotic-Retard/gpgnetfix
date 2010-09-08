namespace GPG.Multiplayer.Client.map
{
    using DevExpress.Utils;
    using GPG;
    using GPG.DataAccess;
    using GPG.Logging;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Threading;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;

    public class DlgWorldMap : DlgBase
    {
        public SkinButton btnOptOut;
        public SkinButton btnRefreshGames;
        private GPGCheckBox cbAllPlayers;
        private GPGCheckBox cbClanmates;
        private GPGCheckBox cbFriends;
        private GPGCheckBox cbGames;
        private GPGCheckBox cbOnline;
        private GPGCheckBox cbRegistrations;
        private GPGCheckBox cbReports;
        private GPGCheckBox cbTop10;
        private ContextMenuStrip cmEarthMap;
        private IContainer components = null;
        public static DlgWorldMap CurrentMap = null;
        private EarthMap earthMap1;
        private FlowLayoutPanel lpChecks;
        private Hashtable mCommonIP = Hashtable.Synchronized(new Hashtable());
        private byte[] mData = new byte[0];
        private DlgWorldMap mDlgWorldMap = null;
        private ToolStripMenuItem miHideLocation;
        private ToolStripMenuItem miRefreshGamesList;
        private ToolStripMenuItem miToggleBoundaries;
        private string mOptInText = Loc.Get("<LOC>Show my location");
        private string mOptOutText = Loc.Get("<LOC>Hide my location");
        private MapLocation mSelfLocation = null;
        private Thread mThread = null;
        private Thread mTicker = null;
        private ArrayList mTickerItems = ArrayList.Synchronized(new ArrayList());
        private GPGPanel pBottomBG;
        private GPGPictureBox pbTicker;
        private StateTimer refreshTimer;

        public DlgWorldMap()
        {
            this.InitializeComponent();
            this.earthMap1.MapScale = ((float) this.earthMap1.Width) / 21600f;
            this.mDlgWorldMap = this;
            this.btnOptOut.Enabled = false;
            this.btnOptOut.Tag = 0;
            this.miHideLocation.Enabled = false;
            this.miHideLocation.Tag = 0;
            this.cbAllPlayers.Checked = false;
            this.cbOnline.Checked = false;
            this.cbFriends.Checked = false;
            this.cbClanmates.Checked = false;
            this.cbRegistrations.Checked = false;
            this.cbReports.Checked = true;
            this.cbGames.Checked = false;
            this.cbTop10.Checked = false;
            this.miRefreshGamesList.Text = Loc.Get(this.miRefreshGamesList.Text);
            this.miToggleBoundaries.Text = Loc.Get(this.miToggleBoundaries.Text);
            this.miToggleBoundaries.Visible = User.Current.IsAdmin;
            this.pBottomBG.BackgroundImage = SkinManager.GetImage("brushbg.png");
            this.lpChecks.BackgroundImage = SkinManager.GetImage("brushbg.png");
            this.earthMap1.WatermarkLeft = DrawUtil.GetTransparentImage(0.5f, Resources.logo_supreme);
            this.earthMap1.WatermarkRight = DrawUtil.GetTransparentImage(0.5f, Resources.logo_gpgnet);
            this.Text = Loc.Get("<LOC>GPGnet World Map");
            CurrentMap = this;
        }

        private void btnOptOut_Click(object sender, EventArgs e)
        {
            if (((int) this.btnOptOut.Tag) == 1)
            {
                this.mSelfLocation.HideLocation = false;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "IPOptIn", new object[0] });
                this.btnOptOut.Text = this.mOptOutText;
                this.btnOptOut.Tag = 0;
                this.earthMap1.RedrawMap();
            }
            else
            {
                this.mSelfLocation.HideLocation = true;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "IPOptOut", new object[0] });
                this.btnOptOut.Text = this.mOptInText;
                this.btnOptOut.Tag = 1;
                this.earthMap1.RedrawMap();
            }
        }

        private void btnRefreshGames_Click(object sender, EventArgs e)
        {
            this.ShowCustomGames();
        }

        private void CalcFilter()
        {
            List<string> list = new List<string>();
            if (!this.cbFriends.Checked)
            {
                list.Add("Friend");
            }
            if (!this.cbClanmates.Checked)
            {
                list.Add("Clan");
            }
            if (!this.cbOnline.Checked)
            {
                list.Add("Online");
            }
            if (!this.cbAllPlayers.Checked)
            {
                list.Add("Pop");
            }
            if (!this.cbReports.Checked)
            {
                list.Add("GameResult");
            }
            if (!this.cbTop10.Checked)
            {
                list.Add("Top10");
            }
            if (!this.cbRegistrations.Checked)
            {
                list.Add("Registration");
            }
            if (!this.cbGames.Checked)
            {
                list.Add("CustomGame");
            }
            this.earthMap1.Filters = list;
            this.earthMap1.Invalidate();
        }

        private void cbFriends_Paint(object sender, PaintEventArgs e)
        {
        }

        private void cbOnline_CheckedChanged(object sender, EventArgs e)
        {
            this.CalcFilter();
        }

        private void cbOnline_Click(object sender, EventArgs e)
        {
            this.CalcFilter();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgWorldMap_Load(object sender, EventArgs e)
        {
        }

        private void DlgWorldMap_SizeChanged(object sender, EventArgs e)
        {
            this.earthMap1.Refresh();
        }

        public static void DoTicker(object omap)
        {
            DlgWorldMap map = null;
            try
            {
                int num;
                bool flag;
                map = omap as DlgWorldMap;
                TickerItem item = null;
                goto Label_0784;
            Label_0025:
                Thread.Sleep(0x3e8);
                Bitmap bitmap = null;
                if (!map.Visible || (map.mTickerItems.Count <= 0))
                {
                    goto Label_0776;
                }
                VGen2 method = null;
                int position = -map.pbTicker.Width;
                goto Label_076D;
            Label_0096:
                num = Environment.TickCount;
                Brush brush = new SolidBrush(Color.White);
                if (((bitmap == null) || (map.mTickerItems[map.mTickerItems.Count - 1] != item)) && ((map.pbTicker.Width > 0) && (map.pbTicker.Height > 0)))
                {
                    int num3;
                    TickerItem item2;
                    item = map.mTickerItems[map.mTickerItems.Count - 1] as TickerItem;
                    bitmap = new Bitmap(map.pbTicker.Width, map.pbTicker.Height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    float x = 0f;
                    for (num3 = 0; num3 < map.mTickerItems.Count; num3++)
                    {
                        item2 = map.mTickerItems[num3] as TickerItem;
                        x += graphics.MeasureString(item2.LogTime.ToShortTimeString(), map.Font).Width + 4f;
                        x += 40f;
                        x += graphics.MeasureString(item2.Loser, map.Font).Width + 4f;
                        x += graphics.MeasureString(item2.Witty, map.Font).Width + 4f;
                        x += 40f;
                        x += graphics.MeasureString(item2.Winner, map.Font).Width + 20f;
                        x += 30f;
                    }
                    if (x < map.pbTicker.Width)
                    {
                        x = map.pbTicker.Width;
                    }
                    graphics.Dispose();
                    try
                    {
                        bitmap = new Bitmap((int) x, map.pbTicker.Height);
                    }
                    catch
                    {
                        bitmap = null;
                        Thread.Sleep(100);
                        graphics.Dispose();
                        goto Label_076D;
                    }
                    graphics = Graphics.FromImage(bitmap);
                    x = 0f;
                    for (num3 = 0; num3 < map.mTickerItems.Count; num3++)
                    {
                        item2 = map.mTickerItems[num3] as TickerItem;
                        graphics.DrawString(item2.LogTime.ToShortTimeString(), map.Font, brush, x, 10f);
                        x += graphics.MeasureString(item2.LogTime.ToShortTimeString(), map.Font).Width + 4f;
                        if (item2.LoseFaction == "aeon")
                        {
                            graphics.DrawImage(Resources.AeonLoss, x, (float) 0f, (float) 32f, (float) 32f);
                        }
                        else if (item2.LoseFaction == "cybran")
                        {
                            graphics.DrawImage(Resources.CybranLoss, x, (float) 0f, (float) 32f, (float) 32f);
                        }
                        else
                        {
                            graphics.DrawImage(Resources.UefLoss, x, (float) 0f, (float) 32f, (float) 32f);
                        }
                        x += 40f;
                        graphics.DrawString(item2.Loser, map.Font, brush, x, 10f);
                        x += graphics.MeasureString(item2.Loser, map.Font).Width + 4f;
                        graphics.DrawString(item2.Witty, map.Font, brush, x, 10f);
                        x += graphics.MeasureString(item2.Witty, map.Font).Width + 4f;
                        if (item2.WinFaction == "aeon")
                        {
                            graphics.DrawImage(Resources.AeonVictory, x, (float) 0f, (float) 32f, (float) 32f);
                        }
                        else if (item2.WinFaction == "cybran")
                        {
                            graphics.DrawImage(Resources.CybranVictory, x, (float) 0f, (float) 32f, (float) 32f);
                        }
                        else
                        {
                            graphics.DrawImage(Resources.UefVictory, x, (float) 0f, (float) 32f, (float) 32f);
                        }
                        x += 40f;
                        graphics.DrawString(item2.Winner, map.Font, brush, x, 10f);
                        x += graphics.MeasureString(item2.Winner, map.Font).Width + 20f;
                        x += 30f;
                    }
                    graphics.Dispose();
                }
                if (bitmap == null)
                {
                    Thread.Sleep(0x3e8);
                }
                else
                {
                    if (position > bitmap.Width)
                    {
                        position = -map.pbTicker.Width;
                        while (map.mTickerItems.Count > 10)
                        {
                            map.mTickerItems.RemoveAt(0);
                        }
                        item = null;
                    }
                    try
                    {
                        if ((!map.IsDisposed && !map.Disposing) && map.Visible)
                        {
                            if (method == null)
                            {
                                method = delegate (object objbitmap, object objpos) {
                                    if ((map.pbTicker.Width > 0) && (map.pbTicker.Height > 0))
                                    {
                                        int num = (int) objpos;
                                        Bitmap image = (Bitmap) objbitmap;
                                        if ((map.pbTicker.Image == null) || (map.pbTicker.Image.Width != map.pbTicker.Width))
                                        {
                                            map.pbTicker.Image = new Bitmap(map.pbTicker.Width, map.pbTicker.Height);
                                        }
                                        Graphics graphics = Graphics.FromImage(map.pbTicker.Image);
                                        Brush brush = new SolidBrush(Color.Black);
                                        Rectangle rect = new Rectangle(0, 0, map.pbTicker.Width, map.pbTicker.Height);
                                        Rectangle srcRect = new Rectangle(position, 0, map.pbTicker.Width, map.pbTicker.Height);
                                        graphics.FillRectangle(brush, rect);
                                        graphics.DrawImage(image, rect, srcRect, GraphicsUnit.Pixel);
                                        brush.Dispose();
                                        brush.Dispose();
                                        graphics.Dispose();
                                        map.pbTicker.Invalidate();
                                    }
                                };
                            }
                            map.Invoke(method, new object[] { bitmap, position });
                        }
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                        if (exception is ThreadInterruptedException)
                        {
                            throw;
                        }
                    }
                    brush.Dispose();
                    position += 2;
                    int num4 = Environment.TickCount - num;
                    if (num4 > 40)
                    {
                        num4 = 40;
                    }
                    Thread.Sleep((int) (50 - num4));
                }
            Label_076D:
                flag = true;
                goto Label_0096;
            Label_0776:
                Thread.Sleep(0x3e8);
            Label_0784:
                flag = true;
                goto Label_0025;
            }
            catch (ThreadInterruptedException exception2)
            {
                try
                {
                    ErrorLog.WriteLine(exception2);
                    map.pbTicker.Image = null;
                }
                catch
                {
                }
            }
        }

        private void earthMap1_MouseClick(object sender, MouseEventArgs e)
        {
            WaitCallback callBack = null;
            try
            {
                MapLocation selectedLocation = this.earthMap1.SelectedLocation;
                if (selectedLocation != null)
                {
                    string[] strArray = selectedLocation.GetSingleInfo().Split(new char[] { ' ' });
                    string playerName = strArray[strArray.Length - 1];
                    if (selectedLocation.Classification == "CustomGame")
                    {
                        GameItem tag = selectedLocation.Tag as GameItem;
                        if (((tag.Password != null) && (tag.Password.Length > 0)) && (tag.Password != DlgAskQuestion.AskQuestion(base.MainForm, Loc.Get("<LOC>What is the password?"), true)))
                        {
                            DlgMessage.ShowDialog(Loc.Get("<LOC>That password is not correct."));
                        }
                        else
                        {
                            base.MainForm.BeforeGameChatroom = Chatroom.CurrentName;
                            base.MainForm.mGameName = tag.Description;
                            base.MainForm.mGameURL = tag.URL;
                            base.MainForm.mHostName = tag.PlayerName;
                            base.Visible = false;
                            ThreadQueue.Quazal.Enqueue(typeof(Chatroom), "Leave", base.MainForm, "OnJoinGame", new object[0]);
                            int num = 0;
                            while (Chatroom.InChatroom)
                            {
                                Thread.Sleep(10);
                                Application.DoEvents();
                                num++;
                                if (num > 300)
                                {
                                    break;
                                }
                            }
                            base.MainForm.SetStatusButtons(1);
                        }
                    }
                    else if (!((selectedLocation.Classification == "GameResult") && ConfigSettings.GetBool("WebStatsEnabled", false)))
                    {
                        base.MainForm.OnViewPlayerProfile(playerName);
                    }
                    else
                    {
                        if (callBack == null)
                        {
                            callBack = delegate (object objname) {
                                User user;
                                VGen1 method = null;
                                string str = (objname as object[])[0].ToString();
                                if (DataAccess.TryGetObject<User>("GetPlayerDetails", out user, new object[] { str }))
                                {
                                    if (method == null)
                                    {
                                        method = delegate (object userid) {
                                            base.MainForm.ShowWebStats((int) userid);
                                        };
                                    }
                                    base.BeginInvoke(method, new object[] { user.ID });
                                }
                            };
                        }
                        ThreadQueue.QueueUserWorkItem(callBack, new object[] { playerName });
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        private bool FindLocation(uint ipaddress, out float latitide, out float longitude)
        {
            uint num;
            return this.FindLocation(ipaddress, out latitide, out longitude, out num);
        }

        private bool FindLocation(uint ipaddress, out float latitide, out float longitude, out uint commonip)
        {
            commonip = 0;
            latitide = 0f;
            longitude = 0f;
            int num = 3;
            int num2 = 0x18;
            int num3 = this.mData.Length / num2;
            int num4 = num3 / 2;
            int num5 = num3 / 4;
            while (true)
            {
                int startIndex = num4 * num2;
                uint num7 = BitConverter.ToUInt32(this.mData, startIndex);
                uint num8 = BitConverter.ToUInt32(this.mData, startIndex + 4);
                if ((num7 <= ipaddress) && (num8 >= ipaddress))
                {
                    latitide = (float) BitConverter.ToDouble(this.mData, startIndex + 8);
                    longitude = (float) BitConverter.ToDouble(this.mData, startIndex + 0x10);
                    string key = ((float) latitide).ToString() + ((float) longitude).ToString();
                    if (this.mCommonIP.ContainsKey(key))
                    {
                        commonip = (uint) this.mCommonIP[key];
                    }
                    else
                    {
                        this.mCommonIP.Add(key, ipaddress);
                        commonip = ipaddress;
                    }
                    return true;
                }
                if (num7 < ipaddress)
                {
                    num4 += num5;
                }
                else
                {
                    num4 -= num5;
                }
                if (num5 == 1)
                {
                    num--;
                }
                if (num <= 0)
                {
                    return false;
                }
                num5 /= 2;
                if (num5 == 0)
                {
                    num5 = 1;
                }
            }
        }

        private string GenWittyText()
        {
            Random random = new Random();
            string str = Loc.Get("<LOC>{0} was crushed by {1}");
            switch (random.Next(5))
            {
                case 0:
                    str = Loc.Get("<LOC>{0} was destroyed by {1}");
                    break;

                case 1:
                    str = Loc.Get("<LOC>{0} was terminated by {1}");
                    break;

                case 2:
                    str = Loc.Get("<LOC>{0} was devastated by {1}");
                    break;

                case 3:
                    str = Loc.Get("<LOC>{0} was smashed by {1}");
                    break;
            }
            return str.Replace("{0}", "").Replace("{1}", "");
        }

        public void GetData()
        {
            this.btnOptOut.Enabled = false;
            this.miHideLocation.Enabled = false;
            this.btnRefreshGames.Enabled = false;
            this.earthMap1.ClearLocations();
            if (this.mThread != null)
            {
                this.mThread.Interrupt();
            }
            if (this.mTicker != null)
            {
                this.mTicker.Interrupt();
            }
            this.earthMap1.Focus();
            this.SetCBColors();
            this.mThread = new Thread(new ParameterizedThreadStart(DlgWorldMap.ProcessData));
            this.mThread.IsBackground = true;
            this.mThread.Start(this);
            this.mTicker = new Thread(new ParameterizedThreadStart(DlgWorldMap.DoTicker));
            this.mTicker.Priority = ThreadPriority.BelowNormal;
            this.mTicker.IsBackground = true;
            this.mTicker.Start(this);
        }

        private List<Image> GetNuke()
        {
            return new List<Image>();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.earthMap1 = new EarthMap();
            this.cmEarthMap = new ContextMenuStrip(this.components);
            this.miRefreshGamesList = new ToolStripMenuItem();
            this.miHideLocation = new ToolStripMenuItem();
            this.miToggleBoundaries = new ToolStripMenuItem();
            this.refreshTimer = new StateTimer();
            this.cbRegistrations = new GPGCheckBox();
            this.btnRefreshGames = new SkinButton();
            this.btnOptOut = new SkinButton();
            this.pBottomBG = new GPGPanel();
            this.lpChecks = new FlowLayoutPanel();
            this.cbReports = new GPGCheckBox();
            this.cbGames = new GPGCheckBox();
            this.cbFriends = new GPGCheckBox();
            this.cbClanmates = new GPGCheckBox();
            this.cbTop10 = new GPGCheckBox();
            this.cbOnline = new GPGCheckBox();
            this.cbAllPlayers = new GPGCheckBox();
            this.pbTicker = new GPGPictureBox();
            this.cmEarthMap.SuspendLayout();
            this.refreshTimer.BeginInit();
            this.pBottomBG.SuspendLayout();
            this.lpChecks.SuspendLayout();
            ((ISupportInitialize) this.pbTicker).BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.earthMap1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.earthMap1.BackColor = Color.Silver;
            this.earthMap1.ContextMenuStrip = this.cmEarthMap;
            this.earthMap1.Location = new Point(7, 0x3b);
            this.earthMap1.MapScale = 0.07f;
            this.earthMap1.MapX = 0f;
            this.earthMap1.MapY = 0f;
            this.earthMap1.Name = "earthMap1";
            this.earthMap1.ShowBorders = false;
            this.earthMap1.Size = new Size(0x377, 0x1a6);
            base.ttDefault.SetSuperTip(this.earthMap1, null);
            this.earthMap1.TabIndex = 7;
            this.earthMap1.Text = "earthMap1";
            this.earthMap1.WatermarkLeft = null;
            this.earthMap1.WatermarkRight = null;
            this.earthMap1.MouseClick += new MouseEventHandler(this.earthMap1_MouseClick);
            this.cmEarthMap.Items.AddRange(new ToolStripItem[] { this.miRefreshGamesList, this.miHideLocation, this.miToggleBoundaries });
            this.cmEarthMap.Name = "cmEarthMap";
            this.cmEarthMap.Size = new Size(0xef, 70);
            base.ttDefault.SetSuperTip(this.cmEarthMap, null);
            this.miRefreshGamesList.Name = "miRefreshGamesList";
            this.miRefreshGamesList.Size = new Size(0xee, 0x16);
            this.miRefreshGamesList.Text = "<LOC>Refresh Games List";
            this.miRefreshGamesList.Click += new EventHandler(this.miRefreshGamesList_Click);
            this.miHideLocation.Name = "miHideLocation";
            this.miHideLocation.Size = new Size(0xee, 0x16);
            this.miHideLocation.Text = "<LOC>Hide my location";
            this.miHideLocation.Click += new EventHandler(this.miHideLocation_Click);
            this.miToggleBoundaries.Name = "miToggleBoundaries";
            this.miToggleBoundaries.Size = new Size(0xee, 0x16);
            this.miToggleBoundaries.Text = "<LOC>Toggle country boundaries";
            this.miToggleBoundaries.Click += new EventHandler(this.miToggleBoundaries_Click);
            this.refreshTimer.Enabled = true;
            this.refreshTimer.ParentForm = null;
            this.refreshTimer.State = null;
            this.refreshTimer.SynchronizingObject = this;
            this.refreshTimer.Elapsed += new ElapsedEventHandler(this.refreshTimer_Elapsed);
            this.cbRegistrations.Anchor = AnchorStyles.Bottom;
            this.cbRegistrations.AutoSize = true;
            this.cbRegistrations.Checked = true;
            this.cbRegistrations.CheckState = CheckState.Checked;
            this.cbRegistrations.ForeColor = Color.Red;
            this.cbRegistrations.Location = new Point(15, 0x1a6);
            this.cbRegistrations.Name = "cbRegistrations";
            this.cbRegistrations.Size = new Size(0x8e, 0x11);
            base.ttDefault.SetSuperTip(this.cbRegistrations, null);
            this.cbRegistrations.TabIndex = 0x11;
            this.cbRegistrations.Text = "<LOC>Registrations";
            this.cbRegistrations.UsesBG = true;
            this.cbRegistrations.UseVisualStyleBackColor = true;
            this.cbRegistrations.Visible = false;
            this.cbRegistrations.Click += new EventHandler(this.cbOnline_Click);
            this.cbRegistrations.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.btnRefreshGames.Anchor = AnchorStyles.Bottom;
            this.btnRefreshGames.AutoStyle = true;
            this.btnRefreshGames.BackColor = Color.Black;
            this.btnRefreshGames.DialogResult = DialogResult.OK;
            this.btnRefreshGames.DisabledForecolor = Color.Gray;
            this.btnRefreshGames.DrawEdges = true;
            this.btnRefreshGames.FocusColor = Color.Yellow;
            this.btnRefreshGames.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnRefreshGames.ForeColor = Color.White;
            this.btnRefreshGames.HorizontalScalingMode = ScalingModes.Tile;
            this.btnRefreshGames.IsStyled = true;
            this.btnRefreshGames.Location = new Point(0x3b, 0x14d);
            this.btnRefreshGames.Name = "btnRefreshGames";
            this.btnRefreshGames.Size = new Size(0xb6, 0x1c);
            this.btnRefreshGames.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnRefreshGames, null);
            this.btnRefreshGames.TabIndex = 0x17;
            this.btnRefreshGames.Text = "<LOC>Refresh Games List";
            this.btnRefreshGames.TextAlign = ContentAlignment.MiddleCenter;
            this.btnRefreshGames.TextPadding = new Padding(0);
            this.btnRefreshGames.Visible = false;
            this.btnRefreshGames.Click += new EventHandler(this.btnRefreshGames_Click);
            this.btnOptOut.Anchor = AnchorStyles.Bottom;
            this.btnOptOut.AutoStyle = true;
            this.btnOptOut.BackColor = Color.Black;
            this.btnOptOut.DialogResult = DialogResult.OK;
            this.btnOptOut.DisabledForecolor = Color.Gray;
            this.btnOptOut.DrawEdges = true;
            this.btnOptOut.FocusColor = Color.Yellow;
            this.btnOptOut.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.btnOptOut.ForeColor = Color.White;
            this.btnOptOut.HorizontalScalingMode = ScalingModes.Tile;
            this.btnOptOut.IsStyled = true;
            this.btnOptOut.Location = new Point(0x3b, 0x16f);
            this.btnOptOut.Name = "btnOptOut";
            this.btnOptOut.Size = new Size(0xb6, 0x1c);
            this.btnOptOut.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.btnOptOut, null);
            this.btnOptOut.TabIndex = 0x18;
            this.btnOptOut.Text = "<LOC>Hide my location";
            this.btnOptOut.TextAlign = ContentAlignment.MiddleCenter;
            this.btnOptOut.TextPadding = new Padding(0);
            this.btnOptOut.Visible = false;
            this.btnOptOut.Click += new EventHandler(this.btnOptOut_Click);
            this.pBottomBG.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.pBottomBG.Controls.Add(this.lpChecks);
            this.pBottomBG.Controls.Add(this.pbTicker);
            this.pBottomBG.Location = new Point(5, 0x1e1);
            this.pBottomBG.Name = "pBottomBG";
            this.pBottomBG.Size = new Size(890, 0x63);
            base.ttDefault.SetSuperTip(this.pBottomBG, null);
            this.pBottomBG.TabIndex = 0x19;
            this.pBottomBG.Paint += new PaintEventHandler(this.pBottomBG_Paint);
            this.lpChecks.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lpChecks.BackColor = Color.Transparent;
            this.lpChecks.Controls.Add(this.cbReports);
            this.lpChecks.Controls.Add(this.cbGames);
            this.lpChecks.Controls.Add(this.cbFriends);
            this.lpChecks.Controls.Add(this.cbClanmates);
            this.lpChecks.Controls.Add(this.cbTop10);
            this.lpChecks.Controls.Add(this.cbOnline);
            this.lpChecks.Controls.Add(this.cbAllPlayers);
            this.lpChecks.Location = new Point(8, 0x2f);
            this.lpChecks.Name = "lpChecks";
            this.lpChecks.Size = new Size(0x36d, 0x45);
            base.ttDefault.SetSuperTip(this.lpChecks, null);
            this.lpChecks.TabIndex = 0x1c;
            this.cbReports.Checked = true;
            this.cbReports.CheckState = CheckState.Checked;
            this.cbReports.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.cbReports.Location = new Point(2, 2);
            this.cbReports.Margin = new Padding(2);
            this.cbReports.Name = "cbReports";
            this.cbReports.Padding = new Padding(2);
            this.cbReports.Size = new Size(0xaf, 0x17);
            base.ttDefault.SetSuperTip(this.cbReports, null);
            this.cbReports.TabIndex = 0x1f;
            this.cbReports.Text = "<LOC> Game Reports";
            this.cbReports.UsesBG = true;
            this.cbReports.UseVisualStyleBackColor = true;
            this.cbReports.Click += new EventHandler(this.cbOnline_Click);
            this.cbReports.Paint += new PaintEventHandler(this.cbFriends_Paint);
            this.cbReports.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.cbGames.Checked = true;
            this.cbGames.CheckState = CheckState.Checked;
            this.cbGames.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.cbGames.ForeColor = Color.FromArgb(0xcc, 0xcc, 0x98);
            this.cbGames.Location = new Point(0xb5, 2);
            this.cbGames.Margin = new Padding(2);
            this.cbGames.Name = "cbGames";
            this.cbGames.Padding = new Padding(2);
            this.cbGames.Size = new Size(0xaf, 0x17);
            base.ttDefault.SetSuperTip(this.cbGames, null);
            this.cbGames.TabIndex = 0x22;
            this.cbGames.Text = "<LOC>Custom Games";
            this.cbGames.UsesBG = true;
            this.cbGames.UseVisualStyleBackColor = true;
            this.cbGames.Click += new EventHandler(this.cbOnline_Click);
            this.cbGames.Paint += new PaintEventHandler(this.cbFriends_Paint);
            this.cbGames.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.cbFriends.Checked = true;
            this.cbFriends.CheckState = CheckState.Checked;
            this.cbFriends.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.cbFriends.ForeColor = Color.FromArgb(0xff, 0x9a, 0x66);
            this.cbFriends.Location = new Point(360, 2);
            this.cbFriends.Margin = new Padding(2);
            this.cbFriends.Name = "cbFriends";
            this.cbFriends.Padding = new Padding(2);
            this.cbFriends.Size = new Size(0xaf, 0x17);
            base.ttDefault.SetSuperTip(this.cbFriends, null);
            this.cbFriends.TabIndex = 0x1c;
            this.cbFriends.Text = "<LOC>Friends";
            this.cbFriends.UsesBG = true;
            this.cbFriends.UseVisualStyleBackColor = true;
            this.cbFriends.Click += new EventHandler(this.cbOnline_Click);
            this.cbFriends.Paint += new PaintEventHandler(this.cbFriends_Paint);
            this.cbFriends.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.cbClanmates.Checked = true;
            this.cbClanmates.CheckState = CheckState.Checked;
            this.cbClanmates.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.cbClanmates.ForeColor = Color.FromArgb(0xcc, 0x33, 0x35);
            this.cbClanmates.Location = new Point(0x21b, 2);
            this.cbClanmates.Margin = new Padding(2);
            this.cbClanmates.Name = "cbClanmates";
            this.cbClanmates.Padding = new Padding(2);
            this.cbClanmates.Size = new Size(0xaf, 0x17);
            base.ttDefault.SetSuperTip(this.cbClanmates, null);
            this.cbClanmates.TabIndex = 0x1d;
            this.cbClanmates.Text = "<LOC>Clanmates";
            this.cbClanmates.UsesBG = true;
            this.cbClanmates.UseVisualStyleBackColor = true;
            this.cbClanmates.Click += new EventHandler(this.cbOnline_Click);
            this.cbClanmates.Paint += new PaintEventHandler(this.cbFriends_Paint);
            this.cbClanmates.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.cbTop10.Checked = true;
            this.cbTop10.CheckState = CheckState.Checked;
            this.cbTop10.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.cbTop10.ForeColor = Color.FromArgb(0x98, 0xcc, 0xfc);
            this.cbTop10.Location = new Point(2, 0x1d);
            this.cbTop10.Margin = new Padding(2);
            this.cbTop10.Name = "cbTop10";
            this.cbTop10.Padding = new Padding(2);
            this.cbTop10.Size = new Size(0xaf, 0x17);
            base.ttDefault.SetSuperTip(this.cbTop10, null);
            this.cbTop10.TabIndex = 30;
            this.cbTop10.Text = "<LOC>Top 10";
            this.cbTop10.UsesBG = true;
            this.cbTop10.UseVisualStyleBackColor = true;
            this.cbTop10.Click += new EventHandler(this.cbOnline_Click);
            this.cbTop10.Paint += new PaintEventHandler(this.cbFriends_Paint);
            this.cbTop10.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.cbOnline.Checked = true;
            this.cbOnline.CheckState = CheckState.Checked;
            this.cbOnline.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.cbOnline.ForeColor = Color.FromArgb(3, 0xfe, 0);
            this.cbOnline.Location = new Point(0xb5, 0x1d);
            this.cbOnline.Margin = new Padding(2);
            this.cbOnline.Name = "cbOnline";
            this.cbOnline.Padding = new Padding(2);
            this.cbOnline.Size = new Size(0xaf, 0x17);
            base.ttDefault.SetSuperTip(this.cbOnline, null);
            this.cbOnline.TabIndex = 0x20;
            this.cbOnline.Text = "<LOC>Online";
            this.cbOnline.UsesBG = true;
            this.cbOnline.UseVisualStyleBackColor = true;
            this.cbOnline.Click += new EventHandler(this.cbOnline_Click);
            this.cbOnline.Paint += new PaintEventHandler(this.cbFriends_Paint);
            this.cbOnline.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.cbAllPlayers.Checked = true;
            this.cbAllPlayers.CheckState = CheckState.Checked;
            this.cbAllPlayers.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.cbAllPlayers.ForeColor = Color.FromArgb(0xff, 0xff, 0x35);
            this.cbAllPlayers.Location = new Point(360, 0x1d);
            this.cbAllPlayers.Margin = new Padding(2);
            this.cbAllPlayers.Name = "cbAllPlayers";
            this.cbAllPlayers.Padding = new Padding(2);
            this.cbAllPlayers.Size = new Size(0xaf, 0x17);
            base.ttDefault.SetSuperTip(this.cbAllPlayers, null);
            this.cbAllPlayers.TabIndex = 0x21;
            this.cbAllPlayers.Text = "<LOC>All Players";
            this.cbAllPlayers.UsesBG = true;
            this.cbAllPlayers.UseVisualStyleBackColor = true;
            this.cbAllPlayers.Click += new EventHandler(this.cbOnline_Click);
            this.cbAllPlayers.Paint += new PaintEventHandler(this.cbFriends_Paint);
            this.cbAllPlayers.CheckedChanged += new EventHandler(this.cbOnline_CheckedChanged);
            this.pbTicker.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.pbTicker.Location = new Point(1, 9);
            this.pbTicker.Name = "pbTicker";
            this.pbTicker.Size = new Size(0x378, 0x23);
            base.ttDefault.SetSuperTip(this.pbTicker, null);
            this.pbTicker.TabIndex = 0x1a;
            this.pbTicker.TabStop = false;
            this.pbTicker.Paint += new PaintEventHandler(this.pbTicker_Paint);
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(900, 0x261);
            base.Controls.Add(this.btnOptOut);
            base.Controls.Add(this.btnRefreshGames);
            base.Controls.Add(this.cbRegistrations);
            base.Controls.Add(this.earthMap1);
            base.Controls.Add(this.pBottomBG);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x2ed, 600);
            base.Name = "DlgWorldMap";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>World Map";
            base.SizeChanged += new EventHandler(this.DlgWorldMap_SizeChanged);
            base.Load += new EventHandler(this.DlgWorldMap_Load);
            base.Controls.SetChildIndex(this.pBottomBG, 0);
            base.Controls.SetChildIndex(this.earthMap1, 0);
            base.Controls.SetChildIndex(this.cbRegistrations, 0);
            base.Controls.SetChildIndex(this.btnRefreshGames, 0);
            base.Controls.SetChildIndex(this.btnOptOut, 0);
            this.cmEarthMap.ResumeLayout(false);
            this.refreshTimer.EndInit();
            this.pBottomBG.ResumeLayout(false);
            this.lpChecks.ResumeLayout(false);
            ((ISupportInitialize) this.pbTicker).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void miHideLocation_Click(object sender, EventArgs e)
        {
            if (((int) this.miHideLocation.Tag) == 1)
            {
                this.mSelfLocation.HideLocation = false;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "IPOptIn", new object[0] });
                this.miHideLocation.Text = this.mOptOutText;
                this.miHideLocation.Tag = 0;
                this.earthMap1.RedrawMap();
            }
            else
            {
                this.mSelfLocation.HideLocation = true;
                ThreadQueue.Quazal.Enqueue(typeof(DataAccess), "ExecuteQuery", null, null, new object[] { "IPOptOut", new object[0] });
                this.miHideLocation.Text = this.mOptInText;
                this.miHideLocation.Tag = 1;
                this.earthMap1.RedrawMap();
            }
        }

        private void miRefreshGamesList_Click(object sender, EventArgs e)
        {
            this.ShowCustomGames();
        }

        private void miToggleBoundaries_Click(object sender, EventArgs e)
        {
            this.earthMap1.ShowBorders = !this.earthMap1.ShowBorders;
            this.earthMap1.Invalidate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.mThread != null)
            {
                this.mThread.Interrupt();
                this.mThread = null;
            }
            if (this.mTicker != null)
            {
                this.mTicker.Interrupt();
                this.mTicker = null;
            }
            e.Cancel = true;
            base.Visible = false;
        }

        private void pBottomBG_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Control control = sender as Control;
            Pen pen = new Pen(Color.White);
            graphics.DrawLine(pen, 0, 0, control.Width - 1, 0);
            pen.Dispose();
        }

        private void pbTicker_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Control control = sender as Control;
            Brush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, control.Height), Color.Black, Color.White);
            Pen pen = new Pen(brush);
            graphics.DrawLine(pen, 0, 0, control.Width - 1, 0);
            graphics.DrawLine(pen, 0, control.Height - 1, control.Width - 1, control.Height - 1);
            graphics.DrawLine(pen, 0, 0, 0, control.Height);
            graphics.DrawLine(pen, control.Width - 1, 0, control.Width - 1, control.Height - 1);
            pen.Dispose();
            brush.Dispose();
        }

        public static void ProcessData(object omap)
        {
            StringEventHandler handler = null;
            VGen0 method = null;
            VGen1 gen2 = null;
            VGen2 gen3 = null;
            VGen1 gen4 = null;
            VGen1 gen5 = null;
            VGen1 gen6 = null;
            VGen1 gen7 = null;
            VGen1 gen8 = null;
            VGen1 gen9 = null;
            VGen1 gen10 = null;
            VGen0 gen11 = null;
            VGen1 gen12 = null;
            DlgWorldMap map = omap as DlgWorldMap;
            try
            {
                string[] strArray;
                uint num;
                Exception exception;
                float num2;
                float num3;
                MapLocation location;
                string str2;
                FileStream stream;
                int num4;
                int num5;
                DataList list2;
                byte[] buffer2;
                string str3;
                WebClient client;
                byte[] buffer3;
                int num7;
                string str4;
                bool flag2;
                string sourcePath = Application.StartupPath + @"\map\encoded.bin.gzip";
                if (handler == null)
                {
                    handler = delegate (string text) {
                        map.SetStatus(Loc.Get("Decompressing map data: ") + text, new object[0]);
                    };
                }
                Compression.OnDecompress += handler;
                map.mData = Compression.DecompressFile(sourcePath);
                map.SetStatus(Loc.Get("<LOC>Fetching locations."), new object[0]);
                if (method == null)
                {
                    method = delegate {
                        map.SetCBColors();
                    };
                }
                map.BeginInvoke(method);
                DataList queryDataSafe = DataAccess.GetQueryDataSafe("GetSelfIP", new object[0]);
                foreach (DataRecord record in queryDataSafe)
                {
                    map.SetStatus(Loc.Get("<LOC>Loading Location: ") + record["player_name"].ToString(), new object[0]);
                    strArray = record["ip_address"].ToString().Split(new char[] { '.' });
                    if (strArray.Length == 4)
                    {
                        try
                        {
                            num = ((Convert.ToUInt32(strArray[0]) * 0x100) * 0x100) * 0x100;
                            num += (Convert.ToUInt32(strArray[1]) * 0x100) * 0x100;
                            num += Convert.ToUInt32(strArray[2]) * 0x100;
                            num += Convert.ToUInt32(strArray[3]);
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            ErrorLog.WriteLine(exception);
                            continue;
                        }
                        num2 = 0f;
                        num3 = 0f;
                        map.FindLocation(num, out num2, out num3);
                        location = new MapLocation();
                        location.HideLocation = record["ip_opt_out"].ToString() == "1";
                        location.MapColor = Color.White;
                        location.Latitude = num2;
                        location.Longitude = num3;
                        location.DataType = "Self";
                        location.Classification = "Self";
                        location.LocationType = GPG.UI.LocationTypes.Image;
                        location.MapImage = Resources.TackSelf;
                        location.Information = record["player_name"];
                        if (gen2 == null)
                        {
                            gen2 = delegate (object objlocation) {
                                MapLocation mapLocation = objlocation as MapLocation;
                                if (mapLocation != null)
                                {
                                    map.earthMap1.AddLocation(mapLocation);
                                    map.earthMap1.Invalidate();
                                }
                            };
                        }
                        map.BeginInvoke(gen2, new object[] { location });
                        try
                        {
                            if (gen3 == null)
                            {
                                gen3 = delegate (object objopt, object objloc) {
                                    try
                                    {
                                        map.mSelfLocation = objloc as MapLocation;
                                        map.miHideLocation.Tag = Convert.ToInt32(objopt);
                                        if (((int) map.miHideLocation.Tag) == 1)
                                        {
                                            map.miHideLocation.Text = map.mOptInText;
                                        }
                                        else
                                        {
                                            map.miHideLocation.Text = map.mOptOutText;
                                        }
                                        map.miHideLocation.Enabled = true;
                                    }
                                    catch (Exception exception)
                                    {
                                        ErrorLog.WriteLine(exception);
                                    }
                                };
                            }
                            map.BeginInvoke(gen3, new object[] { record["ip_opt_out"], location });
                        }
                        catch (Exception exception2)
                        {
                            ErrorLog.WriteLine(exception2);
                        }
                    }
                }
                if (User.Current.IsAdmin)
                {
                    Hashtable hashtable = new Hashtable();
                    str2 = Application.StartupPath + @"\map\data.bin";
                    stream = new FileStream(str2, FileMode.Create);
                    num4 = 0;
                    num5 = 500;
                    for (list2 = DataAccess.GetQueryDataSafe("GetIPAddresses", new object[] { num4, num5 }); list2.Count > 0; list2 = DataAccess.GetQueryDataSafe("GetIPAddresses", new object[] { num4, num5 }))
                    {
                        map.SetStatus(Loc.Get("Loading Location (admins get realtime data and thus map takes longer for you than regular folks.)\r\n") + list2[0][0].ToString(), new object[0]);
                        foreach (DataRecord record in list2)
                        {
                            strArray = record[0].ToString().Split(new char[] { '.' });
                            if (strArray.Length == 4)
                            {
                                uint num6;
                                try
                                {
                                    num = ((Convert.ToUInt32(strArray[0]) * 0x100) * 0x100) * 0x100;
                                    num += (Convert.ToUInt32(strArray[1]) * 0x100) * 0x100;
                                    num += Convert.ToUInt32(strArray[2]) * 0x100;
                                    num += Convert.ToUInt32(strArray[3]);
                                }
                                catch (Exception exception5)
                                {
                                    exception = exception5;
                                    ErrorLog.WriteLine(exception);
                                    continue;
                                }
                                if (map.FindLocation(num, out num2, out num3, out num6) && ((num2 != 0f) && (num3 != 0f)))
                                {
                                    byte[] bytes = BitConverter.GetBytes(num6);
                                    if (hashtable.ContainsKey(bytes))
                                    {
                                        if (((byte) hashtable[bytes]) < 0xff)
                                        {
                                            hashtable[bytes] = ((byte) hashtable[bytes]) + 1;
                                        }
                                    }
                                    else
                                    {
                                        hashtable.Add(bytes, (byte) 1);
                                    }
                                    location = new MapLocation();
                                    location.MapColor = Color.FromArgb(50, 0xff, 0xff, 0);
                                    location.Classification = "Pop";
                                    location.Latitude = num2;
                                    location.Longitude = num3;
                                    if (gen4 == null)
                                    {
                                        gen4 = delegate (object objlocation) {
                                            MapLocation mapLocation = objlocation as MapLocation;
                                            if (mapLocation != null)
                                            {
                                                mapLocation.MapColor = Color.FromArgb(50, map.cbAllPlayers.ForeColor);
                                                map.earthMap1.AddLocation(mapLocation);
                                                map.earthMap1.Invalidate();
                                            }
                                        };
                                    }
                                    map.BeginInvoke(gen4, new object[] { location });
                                }
                            }
                        }
                        num4 += num5;
                    }
                    foreach (byte[] buffer in hashtable.Keys)
                    {
                        stream.Write(buffer, 0, 4);
                        stream.WriteByte((byte) hashtable[buffer]);
                    }
                    stream.Close();
                    buffer2 = Compression.CompressFile(str2);
                }
                else
                {
                    try
                    {
                        str3 = ConfigSettings.GetString("WorldMapData", "http://gpgnet.gaspowered.com/gpgnetdata/worldmap/worldmap.data.enc");
                        client = new WebClient();
                        buffer3 = Compression.DecompressData(client.DownloadData(str3));
                        for (num7 = 0; num7 < (buffer3.Length / 5); num7++)
                        {
                            num = BitConverter.ToUInt32(buffer3, num7 * 5);
                            byte num8 = buffer3[(num7 * 5) + 4];
                            if (map.FindLocation(num, out num2, out num3) && ((num2 != 0f) && (num3 != 0f)))
                            {
                                for (int i = 0; i < num8; i++)
                                {
                                    location = new MapLocation();
                                    location.MapColor = Color.FromArgb(50, 0xff, 0xff, 0);
                                    location.Classification = "Pop";
                                    location.Latitude = num2;
                                    location.Longitude = num3;
                                    if (gen5 == null)
                                    {
                                        gen5 = delegate (object objlocation) {
                                            MapLocation mapLocation = objlocation as MapLocation;
                                            if (mapLocation != null)
                                            {
                                                mapLocation.MapColor = Color.FromArgb(50, map.cbAllPlayers.ForeColor);
                                                map.earthMap1.AddLocation(mapLocation);
                                                map.earthMap1.Invalidate();
                                            }
                                        };
                                    }
                                    map.BeginInvoke(gen5, new object[] { location });
                                }
                            }
                        }
                    }
                    catch (Exception exception6)
                    {
                        exception = exception6;
                        ErrorLog.WriteLine(exception);
                    }
                }
                if (User.Current.IsAdmin)
                {
                    str2 = Application.StartupPath + @"\map\onlinedata.bin";
                    stream = new FileStream(str2, FileMode.Create);
                    num4 = 0;
                    num5 = 500;
                    for (list2 = DataAccess.GetQueryDataSafe("GetOnlineIPAddresses", new object[] { num4, num5 }); list2.Count > 0; list2 = DataAccess.GetQueryDataSafe("GetOnlineIPAddresses", new object[] { num4, num5 }))
                    {
                        map.SetStatus(Loc.Get("Loading Location (admins get realtime data and thus map takes longer for you than regular folks.)\r\n") + list2[0][0].ToString(), new object[0]);
                        foreach (DataRecord record in list2)
                        {
                            strArray = record[0].ToString().Split(new char[] { '.' });
                            if (strArray.Length == 4)
                            {
                                try
                                {
                                    num = ((Convert.ToUInt32(strArray[0]) * 0x100) * 0x100) * 0x100;
                                    num += (Convert.ToUInt32(strArray[1]) * 0x100) * 0x100;
                                    num += Convert.ToUInt32(strArray[2]) * 0x100;
                                    num += Convert.ToUInt32(strArray[3]);
                                }
                                catch (Exception exception7)
                                {
                                    exception = exception7;
                                    ErrorLog.WriteLine(exception);
                                    continue;
                                }
                                stream.Write(BitConverter.GetBytes(num), 0, 4);
                                if (map.FindLocation(num, out num2, out num3) && ((num2 != 0f) && (num3 != 0f)))
                                {
                                    location = new MapLocation();
                                    location.MapColor = Color.FromArgb(70, 0, 0xff, 0);
                                    location.Classification = "Online";
                                    location.Latitude = num2;
                                    location.Longitude = num3;
                                    if (gen6 == null)
                                    {
                                        gen6 = delegate (object objlocation) {
                                            MapLocation mapLocation = objlocation as MapLocation;
                                            if (mapLocation != null)
                                            {
                                                mapLocation.MapColor = Color.FromArgb(0x7d, map.cbOnline.ForeColor);
                                                map.earthMap1.AddLocation(mapLocation);
                                                map.earthMap1.Invalidate();
                                            }
                                        };
                                    }
                                    map.BeginInvoke(gen6, new object[] { location });
                                }
                            }
                        }
                        num4 += num5;
                    }
                    stream.Close();
                    buffer2 = Compression.CompressFile(str2);
                }
                else
                {
                    try
                    {
                        str3 = ConfigSettings.GetString("WorldMapOnlineData", "http://gpgnet.gaspowered.com/gpgnetdata/worldmap/worldmaponline.data.enc");
                        client = new WebClient();
                        buffer3 = Compression.DecompressData(client.DownloadData(str3));
                        for (num7 = 0; num7 < (buffer3.Length / 4); num7++)
                        {
                            num = BitConverter.ToUInt32(buffer3, num7 * 4);
                            if (map.FindLocation(num, out num2, out num3) && ((num2 != 0f) && (num3 != 0f)))
                            {
                                location = new MapLocation();
                                location.MapColor = Color.FromArgb(70, 0, 0xff, 0);
                                location.Classification = "Online";
                                location.Latitude = num2;
                                location.Longitude = num3;
                                if (gen7 == null)
                                {
                                    gen7 = delegate (object objlocation) {
                                        MapLocation mapLocation = objlocation as MapLocation;
                                        if (mapLocation != null)
                                        {
                                            mapLocation.MapColor = Color.FromArgb(0x7d, map.cbOnline.ForeColor);
                                            map.earthMap1.AddLocation(mapLocation);
                                            map.earthMap1.Invalidate();
                                        }
                                    };
                                }
                                map.BeginInvoke(gen7, new object[] { location });
                            }
                        }
                    }
                    catch (Exception exception8)
                    {
                        exception = exception8;
                        ErrorLog.WriteLine(exception);
                    }
                }
                queryDataSafe = DataAccess.GetQueryDataSafe("GetFriendsIPAddresses", new object[0]);
                foreach (DataRecord record in queryDataSafe)
                {
                    map.SetStatus(Loc.Get("<LOC>Loading Location: ") + record[0].ToString(), new object[0]);
                    strArray = record["ip_address"].ToString().Split(new char[] { '.' });
                    if (strArray.Length == 4)
                    {
                        try
                        {
                            num = ((Convert.ToUInt32(strArray[0]) * 0x100) * 0x100) * 0x100;
                            num += (Convert.ToUInt32(strArray[1]) * 0x100) * 0x100;
                            num += Convert.ToUInt32(strArray[2]) * 0x100;
                            num += Convert.ToUInt32(strArray[3]);
                        }
                        catch (Exception exception9)
                        {
                            exception = exception9;
                            ErrorLog.WriteLine(exception);
                            continue;
                        }
                        map.FindLocation(num, out num2, out num3);
                        location = new MapLocation();
                        str4 = Loc.Get("<LOC>Offline");
                        location.MapColor = Color.LightGreen;
                        if (record["connections"].ToString() != "0")
                        {
                            str4 = Loc.Get("<LOC>Online");
                        }
                        location.HideLocation = record["ip_opt_out"].ToString() == "1";
                        location.Latitude = num2;
                        location.Longitude = num3;
                        location.DataType = "Friend";
                        location.Classification = "Friend";
                        location.LocationType = GPG.UI.LocationTypes.Image;
                        location.MapImage = Resources.TackFriend;
                        location.Information = record["name"];
                        location.ExtendedInformation = location.Information + "\r\n" + str4;
                        if (gen8 == null)
                        {
                            gen8 = delegate (object objlocation) {
                                MapLocation mapLocation = objlocation as MapLocation;
                                if (mapLocation != null)
                                {
                                    mapLocation.MapColor = map.cbFriends.ForeColor;
                                    map.earthMap1.AddLocation(mapLocation);
                                    map.earthMap1.Invalidate();
                                }
                            };
                        }
                        map.BeginInvoke(gen8, new object[] { location });
                    }
                }
                queryDataSafe = DataAccess.GetQueryDataSafe("GetClanIPAddresses", new object[0]);
                foreach (DataRecord record in queryDataSafe)
                {
                    map.SetStatus(Loc.Get("<LOC>Loading Location: ") + record[0].ToString(), new object[0]);
                    strArray = record["ip_address"].ToString().Split(new char[] { '.' });
                    if (strArray.Length == 4)
                    {
                        try
                        {
                            num = ((Convert.ToUInt32(strArray[0]) * 0x100) * 0x100) * 0x100;
                            num += (Convert.ToUInt32(strArray[1]) * 0x100) * 0x100;
                            num += Convert.ToUInt32(strArray[2]) * 0x100;
                            num += Convert.ToUInt32(strArray[3]);
                        }
                        catch (Exception exception10)
                        {
                            exception = exception10;
                            ErrorLog.WriteLine(exception);
                            continue;
                        }
                        map.FindLocation(num, out num2, out num3);
                        location = new MapLocation();
                        str4 = Loc.Get("<LOC>Offline");
                        location.MapColor = Color.FromArgb(0xff, 0xff, 0, 0);
                        if (record["connections"].ToString() != "0")
                        {
                            str4 = Loc.Get("<LOC>Online");
                        }
                        location.HideLocation = record["ip_opt_out"].ToString() == "1";
                        location.Latitude = num2;
                        location.Longitude = num3;
                        location.DataType = "Clan";
                        location.Classification = "Clan";
                        location.LocationType = GPG.UI.LocationTypes.Image;
                        location.MapImage = Resources.TackClan;
                        location.Information = record["name"];
                        location.ExtendedInformation = location.Information + "\r\n" + str4;
                        if (gen9 == null)
                        {
                            gen9 = delegate (object objlocation) {
                                MapLocation mapLocation = objlocation as MapLocation;
                                if (mapLocation != null)
                                {
                                    mapLocation.MapColor = map.cbClanmates.ForeColor;
                                    map.earthMap1.AddLocation(mapLocation);
                                    map.earthMap1.Invalidate();
                                }
                            };
                        }
                        map.BeginInvoke(gen9, new object[] { location });
                    }
                }
                map.SetCBColors();
                if (ConfigSettings.GetBool("DoOldGameList", false))
                {
                    queryDataSafe = DataAccess.GetQueryDataSafe("GetTop10Addresses", new object[0]);
                }
                else
                {
                    queryDataSafe = DataAccess.GetQueryDataSafe("GetTop10Addresses", new object[] { GameInformation.SelectedGame.GameID });
                }
                foreach (DataRecord record in queryDataSafe)
                {
                    map.SetStatus(Loc.Get("<LOC>Loading Location: ") + record[0].ToString(), new object[0]);
                    strArray = record["ip_address"].ToString().Split(new char[] { '.' });
                    if (strArray.Length == 4)
                    {
                        try
                        {
                            num = ((Convert.ToUInt32(strArray[0]) * 0x100) * 0x100) * 0x100;
                            num += (Convert.ToUInt32(strArray[1]) * 0x100) * 0x100;
                            num += Convert.ToUInt32(strArray[2]) * 0x100;
                            num += Convert.ToUInt32(strArray[3]);
                        }
                        catch (Exception exception11)
                        {
                            exception = exception11;
                            ErrorLog.WriteLine(exception);
                            continue;
                        }
                        num2 = 0f;
                        num3 = 0f;
                        map.FindLocation(num, out num2, out num3);
                        location = new MapLocation();
                        if (record["connections"].ToString() == "0")
                        {
                            location.MapColor = Color.LightSkyBlue;
                        }
                        else
                        {
                            location.MapColor = Color.FromArgb(0x4b, Color.LightSkyBlue);
                        }
                        location.HideLocation = record["ip_opt_out"].ToString() == "1";
                        location.Latitude = num2;
                        location.Longitude = num3;
                        location.DataType = "Top10";
                        location.Classification = "Top10";
                        location.LocationType = GPG.UI.LocationTypes.Image;
                        location.MapImage = Resources.TackTop10;
                        location.Information = "#" + record["rank"] + " " + record["description"];
                        location.ExtendedInformation = location.Information + "\r\n" + Loc.Get("<LOC>Rating") + " " + record["rating"] + "\r\n" + Loc.Get("<LOC>Wins") + " " + record["wins"] + "\r\n" + Loc.Get("<LOC>Losses") + " " + record["losses"] + "\r\n" + Loc.Get("<LOC>Draws") + " " + record["draws"];
                        if (gen10 == null)
                        {
                            gen10 = delegate (object objlocation) {
                                MapLocation mapLocation = objlocation as MapLocation;
                                if (mapLocation != null)
                                {
                                    mapLocation.MapColor = map.cbTop10.ForeColor;
                                    map.earthMap1.AddLocation(mapLocation);
                                    map.earthMap1.Invalidate();
                                    mapLocation.Animate = true;
                                }
                            };
                        }
                        map.BeginInvoke(gen10, new object[] { location });
                    }
                }
                if (gen11 == null)
                {
                    gen11 = delegate {
                        map.earthMap1.Invalidate();
                        map.earthMap1.Refresh();
                        map.earthMap1.Focus();
                        map.refreshTimer.Interval = ConfigSettings.GetInt("MapDrawRate", 500);
                        map.refreshTimer.Stop();
                    };
                }
                map.BeginInvoke(gen11);
                map.ShowCustomGames();
                int num10 = Convert.ToInt32(DataAccess.GetQueryDataSafe("GetMaxGameReportID", new object[0])[0][0]);
                goto Label_19A1;
            Label_1400:;
                queryDataSafe = DataAccess.GetQueryDataSafe("GatResultsWithIP", new object[] { num10 });
                int num11 = 0;
                MapLocation location2 = null;
                string str5 = "";
                string str6 = "";
                string str7 = "";
                foreach (DataRecord record in queryDataSafe)
                {
                    string str8 = record["name"].ToString();
                    string str9 = record["rank"].ToString();
                    bool flag = record["won_game"].ToString() == "1";
                    int num12 = Convert.ToInt32(record["game_id"]);
                    string str10 = record["faction"].ToString();
                    if (num10 < (num12 + 1))
                    {
                        num10 = num12 + 1;
                    }
                    strArray = record["ip_address"].ToString().Split(new char[] { '.' });
                    location = null;
                    if (strArray.Length == 4)
                    {
                        try
                        {
                            num = ((Convert.ToUInt32(strArray[0]) * 0x100) * 0x100) * 0x100;
                            num += (Convert.ToUInt32(strArray[1]) * 0x100) * 0x100;
                            num += Convert.ToUInt32(strArray[2]) * 0x100;
                            num += Convert.ToUInt32(strArray[3]);
                        }
                        catch (Exception exception12)
                        {
                            exception = exception12;
                            ErrorLog.WriteLine(exception);
                            continue;
                        }
                        map.FindLocation(num, out num2, out num3);
                        location = new MapLocation();
                        location.HideLocation = record["ip_opt_out"].ToString() == "1";
                        location.MapColor = Color.FromArgb(0xff, 140, 140, 140);
                        location.Latitude = num2;
                        location.Longitude = num3;
                        location.DataType = "GameResult";
                        location.Classification = "GameResult";
                        location.LocationType = GPG.UI.LocationTypes.Image;
                        location.Information = str8;
                        if (flag)
                        {
                            if (str10.ToUpper() == "AEON")
                            {
                                location.MapImage = Resources.AeonVictory;
                                location.MapColor = Color.FromArgb(0xff, 0x19, 0xad, 0x5c);
                            }
                            else if (str10.ToUpper() == "CYBRAN")
                            {
                                location.MapImage = Resources.CybranVictory;
                                location.MapColor = Color.FromArgb(0xff, 220, 0x4d, 0x4e);
                            }
                            else
                            {
                                location.MapImage = Resources.UefVictory;
                                location.MapColor = Color.FromArgb(0xff, 0xae, 0xbc, 0xff);
                            }
                            str5 = str8;
                            str6 = str10;
                        }
                        else if (str10.ToUpper() == "AEON")
                        {
                            location.MapImage = Resources.AeonLoss;
                        }
                        else if (str10.ToUpper() == "CYBRAN")
                        {
                            location.MapImage = Resources.CybranLoss;
                        }
                        else
                        {
                            location.MapImage = Resources.UefLoss;
                        }
                        if (gen12 == null)
                        {
                            gen12 = delegate (object objlocation) {
                                MapLocation mapLocation = objlocation as MapLocation;
                                mapLocation.Visible = map.cbReports.Checked;
                                if (mapLocation != null)
                                {
                                    map.earthMap1.AddLocation(mapLocation);
                                    map.earthMap1.Invalidate();
                                    mapLocation.Animate = true;
                                }
                            };
                        }
                        map.BeginInvoke(gen12, new object[] { location });
                    }
                    if (num11 == num12)
                    {
                        string str11 = map.GenWittyText();
                        TickerItem item = new TickerItem();
                        item.Winner = str5;
                        item.Loser = "#" + str9 + " " + str8;
                        item.WinFaction = str6.ToLower();
                        item.LoseFaction = str10.ToLower();
                        item.Witty = str11;
                        map.mTickerItems.Add(item);
                        if (location != null)
                        {
                            location.ExtendedInformation = location.Information + "\r\n" + item.Loser + "\r\n" + item.Witty.Trim() + "\r\n" + item.Winner;
                        }
                        if (location2 != null)
                        {
                            location2.ExtendedInformation = location2.Information + "\r\n" + item.Loser + "\r\n" + item.Witty.Trim() + "\r\n" + item.Winner;
                        }
                    }
                    else
                    {
                        str5 = "#" + str9 + " " + str8;
                        str6 = str10;
                        str7 = record["ip_address"].ToString();
                        num11 = num12;
                        location2 = location;
                    }
                }
                if (User.Current.IsAdmin)
                {
                    Thread.Sleep(ConfigSettings.GetInt("AdminMapRefreshRate", 0x2710));
                }
                else
                {
                    Thread.Sleep(ConfigSettings.GetInt("MapRefreshRate", 0x1d4c0));
                }
                while (!map.Visible)
                {
                    Thread.Sleep(0x2710);
                }
            Label_19A1:
                flag2 = true;
                goto Label_1400;
            }
            catch (Exception exception3)
            {
                ErrorLog.WriteLine(exception3);
            }
        }

        private void refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.refreshTimer.Stop();
            if (base.Visible)
            {
                this.earthMap1.Refresh();
            }
            this.refreshTimer.Start();
        }

        private void SetCBColors()
        {
        }

        private void ShowCustomGames()
        {
            this.miRefreshGamesList.Enabled = false;
            this.earthMap1.ClearLocations("CustomGame");
            ThreadPool.QueueUserWorkItem(delegate (object o) {
                MappedObjectList<GameItem> objects = null;
                Exception exception;
                VGen1 method = null;
                VGen0 gen2 = null;
                if (ConfigSettings.GetBool("DoOldGameList", false))
                {
                    objects = DataAccess.GetObjects<GameItem>("GetGamesWithIP", new object[0]);
                }
                else
                {
                    objects = DataAccess.GetObjects<GameItem>("GetGamesWithIP2", new object[] { GameInformation.SelectedGame.GameID });
                }
                foreach (GameItem item in objects)
                {
                    if (item.Description.IndexOf("AUTOMATCH") == 0)
                    {
                        continue;
                    }
                    string str = item.MapName.Split(" ".ToCharArray(), 2)[0];
                    string[] strArray = item.URL.Split("=;".ToCharArray());
                    if (strArray.Length > 2)
                    {
                        string[] strArray2 = strArray[1].Split(new char[] { '.' });
                        if (strArray2.Length == 4)
                        {
                            uint num;
                            try
                            {
                                num = ((Convert.ToUInt32(strArray2[0]) * 0x100) * 0x100) * 0x100;
                                num += (Convert.ToUInt32(strArray2[1]) * 0x100) * 0x100;
                                num += Convert.ToUInt32(strArray2[2]) * 0x100;
                                num += Convert.ToUInt32(strArray2[3]);
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                ErrorLog.WriteLine(exception);
                                continue;
                            }
                            float latitide = 0f;
                            float longitude = 0f;
                            this.FindLocation(num, out latitide, out longitude);
                            MapLocation location = new MapLocation {
                                HideLocation = item.IPOptOut == 1,
                                MapColor = Color.FromArgb(0xff, 0xe4, 0xc3, 130),
                                Latitude = latitide,
                                Longitude = longitude,
                                DataType = "CustomGame",
                                Classification = "CustomGame",
                                LocationType = GPG.UI.LocationTypes.Image,
                                Information = item.Description,
                                ExtendedInformation = item.Description + "\r\n" + item.MapName + "\r\n" + item.PlayerName + "\r\n" + item.NumPlayers,
                                Tag = item
                            };
                            Image image = item.MapImage;
                            if (image != null)
                            {
                                try
                                {
                                    Graphics graphics = Graphics.FromImage(image);
                                    if (item.HasPassword)
                                    {
                                        graphics.DrawImage(DrawUtil.GetTransparentImage(0.8f, DrawUtil.AdjustColors(0.2f, 0.2f, 0.2f, Resources.key)), new Rectangle(60, 90, 150, 80));
                                    }
                                    graphics.Dispose();
                                    location.DrawImageBorder = true;
                                    location.MapImage = image;
                                    if (method == null)
                                    {
                                        method = delegate (object objlocation) {
                                            MapLocation mapLocation = objlocation as MapLocation;
                                            if (mapLocation != null)
                                            {
                                                mapLocation.MapColor = this.cbGames.ForeColor;
                                                this.earthMap1.AddLocation(mapLocation);
                                                this.earthMap1.Invalidate();
                                                mapLocation.Animate = true;
                                            }
                                        };
                                    }
                                    base.BeginInvoke(method, new object[] { location });
                                }
                                catch (Exception exception2)
                                {
                                    exception = exception2;
                                    ErrorLog.WriteLine(exception);
                                }
                            }
                        }
                    }
                }
                try
                {
                    if (gen2 == null)
                    {
                        gen2 = delegate {
                            base.SetStatus("", new object[0]);
                            this.miRefreshGamesList.Enabled = true;
                        };
                    }
                    base.BeginInvoke(gen2);
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    ErrorLog.WriteLine(exception);
                }
            });
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x20a)
            {
                this.earthMap1.Focus();
            }
        }
    }
}

