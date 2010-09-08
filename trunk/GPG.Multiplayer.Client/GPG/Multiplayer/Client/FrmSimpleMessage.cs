namespace GPG.Multiplayer.Client
{
    using DevExpress.LookAndFeel;
    using DevExpress.XtraEditors;
    using GPG;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class FrmSimpleMessage : FrmNotify
    {
        private IContainer components = null;
        private GroupControl gcMessage;
        private GPGLabel gpgLabel1;
        private static object[] mClickArgs = null;
        private static Delegate mClickDelegate = null;
        private bool mLoaded = false;
        private static FrmSimpleMessage mMessage = null;
        private const int PAD = 30;
        private const int TASKBAR_HEIGHT = 30;

        private FrmSimpleMessage()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static void DoNotification(string caption, string message)
        {
            DoNotification(caption, message, null, new object[0]);
        }

        public static void DoNotification(string caption, string message, Delegate onClick, params object[] clickArgs)
        {
            VGen0 method = null;
            if ((Program.MainForm.InvokeRequired && !Program.MainForm.Disposing) && !Program.MainForm.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        DoNotification(caption, message, onClick, clickArgs);
                    };
                }
                Program.MainForm.BeginInvoke(method);
            }
            else if (!Program.MainForm.Disposing && !Program.MainForm.IsDisposed)
            {
                mClickArgs = clickArgs;
                mClickDelegate = onClick;
                mMessage = new FrmSimpleMessage();
                if (onClick != null)
                {
                    mMessage.Cursor = Cursors.Hand;
                }
                else
                {
                    mMessage.Cursor = Cursors.Default;
                }
                mMessage.gcMessage.Text = Loc.Get(caption);
                mMessage.gpgLabel1.Text = Loc.Get(message);
                mMessage.gpgLabel1.Click += new EventHandler(FrmSimpleMessage.gcMessage_Click);
                mMessage.gcMessage.Click += new EventHandler(FrmSimpleMessage.gcMessage_Click);
                mMessage.ShowMessage();
            }
        }

        private static void gcMessage_Click(object sender, EventArgs e)
        {
            if (mClickDelegate != null)
            {
                mMessage.Invoke((VGen0)delegate {
                    mClickDelegate.DynamicInvoke(ClickArgs);
                });
            }
        }

        private void InitializeComponent()
        {
            this.gcMessage = new GroupControl();
            this.gpgLabel1 = new GPGLabel();
            this.gcMessage.BeginInit();
            this.gcMessage.SuspendLayout();
            base.SuspendLayout();
            this.gcMessage.Appearance.BackColor = Color.FromArgb(0x1b, 0x2e, 0x4a);
            this.gcMessage.Appearance.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gcMessage.Appearance.Options.UseBackColor = true;
            this.gcMessage.Appearance.Options.UseFont = true;
            this.gcMessage.AppearanceCaption.BackColor = Color.FromArgb(0x9f, 0xb8, 0xdd);
            this.gcMessage.AppearanceCaption.BackColor2 = Color.FromArgb(0x5c, 0x77, 0x9e);
            this.gcMessage.AppearanceCaption.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gcMessage.AppearanceCaption.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gcMessage.AppearanceCaption.Options.UseBackColor = true;
            this.gcMessage.AppearanceCaption.Options.UseFont = true;
            this.gcMessage.Controls.Add(this.gpgLabel1);
            this.gcMessage.Dock = DockStyle.Fill;
            this.gcMessage.Location = new Point(0, 0);
            this.gcMessage.LookAndFeel.SkinName = "The Asphalt World";
            this.gcMessage.LookAndFeel.Style = LookAndFeelStyle.Office2003;
            this.gcMessage.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcMessage.Name = "gcMessage";
            this.gcMessage.Size = new Size(0xda, 0x71);
            this.gcMessage.TabIndex = 0;
            this.gcMessage.Text = "Unknown Message";
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Dock = DockStyle.Fill;
            this.gpgLabel1.Font = new Font("Arial", 9.75f);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(2, 0x16);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xd6, 0x59);
            this.gpgLabel1.TabIndex = 0;
            this.gpgLabel1.Text = "Unknown Message";
            this.gpgLabel1.TextAlign = ContentAlignment.MiddleCenter;
            this.gpgLabel1.TextStyle = TextStyles.Default;
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0xda, 0x71);
            base.Controls.Add(this.gcMessage);
            base.Name = "FrmSimpleMessage";
            base.ShowInTaskbar = false;
            this.Text = "FrmSimpleMessage";
            this.gcMessage.EndInit();
            this.gcMessage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!this.mLoaded)
            {
                this.mLoaded = true;
                base.Left = (Screen.PrimaryScreen.Bounds.Width - base.Width) - 30;
                base.Top = ((Screen.PrimaryScreen.Bounds.Height - base.Height) - 30) - 30;
            }
        }

        public static object[] ClickArgs
        {
            get
            {
                return mClickArgs;
            }
        }
    }
}

