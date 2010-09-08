namespace GPG.Multiplayer.Client
{
    using GPG.Logging;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class WelcomePage : UserControl
    {
        public bool CanNavigate = true;
        private IContainer components = null;
        private string mHomePage = null;
        private FrmMain mMainForm = null;
        private WebBrowser wbWelcome;

        public event EventHandler OnJoinChat;

        public event WebLinkClick OnWebLinkClick;

        public WelcomePage()
        {
            this.InitializeComponent();
            this.BackColor = Program.Settings.StylePreferences.MasterBackColor;
        }

        public void ChangeURL(string url)
        {
            this.wbWelcome.Navigate(url);
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
            this.wbWelcome = new WebBrowser();
            base.SuspendLayout();
            this.wbWelcome.AllowWebBrowserDrop = false;
            this.wbWelcome.Dock = DockStyle.Fill;
            this.wbWelcome.Location = new Point(0, 0);
            this.wbWelcome.MinimumSize = new Size(20, 20);
            this.wbWelcome.Name = "wbWelcome";
            this.wbWelcome.Size = new Size(0x29a, 0x1e5);
            this.wbWelcome.TabIndex = 0;
            this.wbWelcome.Url = new Uri("", UriKind.Relative);
            this.wbWelcome.Navigating += new WebBrowserNavigatingEventHandler(this.wbWelcome_Navigating);
            base.AutoScaleMode = AutoScaleMode.None;
            base.Controls.Add(this.wbWelcome);
            base.Name = "WelcomePage";
            base.Size = new Size(0x29a, 0x1e5);
            base.ResumeLayout(false);
        }

        private void JoinChat()
        {
            if (this.OnJoinChat != null)
            {
                this.OnJoinChat(this, EventArgs.Empty);
            }
        }

        internal void RefreshPage()
        {
            this.wbWelcome.Refresh();
        }

        private void wbWelcome_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                string str = e.Url.Segments[e.Url.Segments.Length - 1];
                string str2 = e.Url.ToString();
                if (!this.CanNavigate)
                {
                    if ((str == "chat") || (str.IndexOf("mi") == 0))
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                    if (str == "chat")
                    {
                        this.JoinChat();
                    }
                    else
                    {
                        ChatLink link;
                        if (str2.IndexOf("player:") >= 0)
                        {
                            link = new ChatLink(ChatLink.Player);
                            link.Url = str2.Replace("player:", "");
                            link.OnClick();
                        }
                        else if (str2.IndexOf("content:") >= 0)
                        {
                            link = new ChatLink(ChatLink.Content);
                            link.Url = str2.Replace("content:", "");
                            link.OnClick();
                        }
                        else if (str2.IndexOf("replay:") >= 0)
                        {
                            link = new ChatLink(ChatLink.Replay);
                            link.Url = str2.Replace("replay:", "");
                            link.OnClick();
                        }
                        else if (this.OnWebLinkClick != null)
                        {
                            e.Cancel = this.OnWebLinkClick(e.Url.ToString(), e);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public string CurrentUrl
        {
            get
            {
                if (this.wbWelcome.Url != null)
                {
                    return this.wbWelcome.Url.ToString();
                }
                return null;
            }
        }

        public string HomePage
        {
            get
            {
                return this.mHomePage;
            }
            set
            {
                this.mHomePage = value;
            }
        }

        public FrmMain MainForm
        {
            get
            {
                return this.mMainForm;
            }
            set
            {
                this.mMainForm = value;
            }
        }
    }
}

