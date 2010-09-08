namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Logging;
    using GPG.Multiplayer.Client.EmailService;
    using GPG.Multiplayer.Client.Registration;
    using GPG.Multiplayer.Quazal;
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;

    internal static class Program
    {
        public static string[] CommandLineArgs = null;
        private static bool IsReported = false;
        private static System.Timers.Timer MemRefreshTimer = new System.Timers.Timer(30000.0);
        private static bool mIsClosing = false;
        private static FrmMain mMainForm;
        private static UserPrefs mSettings = UserPrefs.Load();

        public static  event EventHandler Closing;

        public static  event PropertyChangedEventHandler SettingsChanged;

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            VGen0 method = null;
            string error = e.Exception.ToString();
            if (!IsReported)
            {
                IsReported = true;
                ErrorLog.WriteLine(e.Exception);
                Service service = new Service();
                string defaultUsername = Settings.Login.DefaultUsername;
                if (defaultUsername == null)
                {
                    defaultUsername = "Unknown User";
                }
                defaultUsername = defaultUsername + " with GPGnet version " + Assembly.GetEntryAssembly().GetName().Version.ToString(4);
                string log = "";
                BindingList<LogData> mLogList = DlgLogWatcher.sLog.mLogList;
                for (int i = mLogList.Count - 1; (i >= 0) && (i > (mLogList.Count - 300)); i--)
                {
                    log = mLogList[i].DateTime.ToString() + " " + mLogList[i].LogType + ": " + mLogList[i].Description + "\r\n" + log;
                }
                service.ReportError(defaultUsername, log, e.Exception.ToString());
            }
            if (MainForm != null)
            {
                if (method == null)
                {
                    method = delegate {
                        DlgYesNo no = new DlgYesNo(MainForm, Loc.Get("<LOC>Error"), "A serious error has occured in GPGnet and it is recommended you restart.  You can attempt continue, but GPGnet may behave erratically.  Would you like to continue?\r\n\r\n" + error);
                        if (no.ShowDialog() != DialogResult.Yes)
                        {
                            User.Logout();
                            Application.Exit();
                        }
                    };
                }
                MainForm.Invoke(method);
            }
        }

        public static bool HasArg(string name)
        {
            try
            {
                if (CommandLineArgs != null)
                {
                    foreach (string str in CommandLineArgs)
                    {
                        if ((-1 < str.ToUpper().IndexOf(name.ToUpper())) && (str.ToUpper().IndexOf(name.ToUpper()) < 2))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [STAThread]
        private static void Main(string[] initParams)
        {
            Exception exception;
            Exception exception3;
            Application.ThreadException += new ThreadExceptionEventHandler(Program.Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmSplash splash = new FrmSplash();
            splash.Show();
            splash.Refresh();
            DataAccess.sMainThreadID = Thread.CurrentThread.ManagedThreadId;
            bool flag = true;
            string name = "";
            bool flag2 = false;
            foreach (string str2 in initParams)
            {
                switch (str2)
                {
                    case "/version":
                    {
                        string str3 = Assembly.GetEntryAssembly().GetName().Version.ToString(4);
                        StreamWriter writer = new StreamWriter("getversion.bat");
                        writer.WriteLine("set newver=" + str3);
                        writer.Close();
                        return;
                    }
                    case "/multiple":
                        flag = false;
                        break;
                }
                if (str2.IndexOf("/loc") == 0)
                {
                    name = str2.Split(new char[] { '=' })[1];
                }
                if (str2.IndexOf("/register") == 0)
                {
                    flag2 = true;
                }
            }
            try
            {
                if (!true)
                {
                    foreach (Process process in Process.GetProcesses())
                    {
                        if ((process.Id != Process.GetCurrentProcess().Id) && (process.ProcessName == Process.GetCurrentProcess().ProcessName))
                        {
                            try
                            {
                                DlgMessage.ShowDialog(Loc.Get("<LOC>GPGnet is already running."), Loc.Get("<LOC>Unable to start"));
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                            }
                            return;
                        }
                    }
                }
            }
            catch (Exception exception4)
            {
                exception = exception4;
            }
            CommandLineArgs = initParams;
            bool flag3 = true;
            try
            {
                try
                {
                }
                catch (Exception exception5)
                {
                    exception = exception5;
                    ErrorLog.WriteLine(exception);
                }
                try
                {
                    Settings.StylePreferences.Register();
                    DlgLogWatcher.InitLog();
                }
                catch (Exception exception2)
                {
                    ErrorLog.WriteLine(exception2);
                }
                try
                {
                    string str4 = @"Software\GPG\GPGnet";
                    if (Registry.LocalMachine.OpenSubKey(str4) == null)
                    {
                        Registry.LocalMachine.CreateSubKey(str4);
                    }
                    Registry.LocalMachine.OpenSubKey(str4, true).SetValue("GPGNetPath", Application.ExecutablePath);
                }
                catch (Exception exception7)
                {
                    exception = exception7;
                    ErrorLog.WriteLine(exception);
                }
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                Version version2 = Settings.Version;
                if ((Settings.Version == null) || !Settings.Version.Equals(version))
                {
                    GPG.Logging.EventLog.WriteLine("Found older user settings from v. {0}, re-initializing to current settings from v. {1}", new object[] { Settings.Version, Assembly.GetAssembly(typeof(UserPrefs)).GetName().Version });
                    try
                    {
                        string defaultUsername = Settings.Login.DefaultUsername;
                        string defaultPassword = Settings.Login.DefaultPassword;
                        Settings = ProgramSettings.Merge(Settings, new UserPrefs()) as UserPrefs;
                        GPG.Logging.EventLog.WriteLine("Merged settings file from version {0} to current ({1})", new object[] { version2, version });
                    }
                    catch (Exception exception8)
                    {
                        exception3 = exception8;
                        ErrorLog.WriteLine("Failed to merge settings file from version {0} to current ({1})", new object[] { version2, version });
                        ErrorLog.WriteLine(exception3);
                        mSettings = new UserPrefs();
                    }
                    Settings.Save();
                }
                MemRefreshTimer.AutoReset = true;
                MemRefreshTimer.Elapsed += delegate (object s, ElapsedEventArgs e) {
                    RefreshMemUsage();
                };
                MemRefreshTimer.Start();
                if (name == "")
                {
                    Loc.SetLoc();
                }
                else
                {
                    Loc.SetLoc(name);
                }
                if (flag2)
                {
                    DlgRegisterSupcom supcom = new DlgRegisterSupcom();
                    Application.Run(supcom);
                    return;
                }
                FrmMain mainForm = new FrmMain();
                mMainForm = mainForm;
                if (!(mainForm.Disposing || mainForm.IsDisposed))
                {
                    Application.Run(mainForm);
                }
                else
                {
                    flag3 = false;
                }
            }
            catch (Exception exception9)
            {
                exception3 = exception9;
                ErrorLog.WriteLine(exception3);
            }
            finally
            {
                try
                {
                    if (flag3)
                    {
                        mIsClosing = true;
                        if (Closing != null)
                        {
                            Closing(null, EventArgs.Empty);
                        }
                        Settings.Save();
                        User.Logout();
                    }
                }
                catch (Exception exception10)
                {
                    exception3 = exception10;
                    ErrorLog.WriteLine(exception3);
                }
            }
            if (FrmSplash.sFrmSplash != null)
            {
                FrmSplash.sFrmSplash.Close();
            }
        }

        public static void RefreshMemUsage()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                currentProcess.MaxWorkingSet = currentProcess.MaxWorkingSet;
                currentProcess.MinWorkingSet = currentProcess.MinWorkingSet;
            }
            catch (Exception)
            {
            }
        }

        public static bool IsClosing
        {
            get
            {
                return mIsClosing;
            }
        }

        public static FrmMain MainForm
        {
            get
            {
                return mMainForm;
            }
        }

        public static UserPrefs Settings
        {
            get
            {
                return mSettings;
            }
            set
            {
                mSettings = value;
                if (SettingsChanged != null)
                {
                    SettingsChanged(mSettings, new PropertyChangedEventArgs("Settings"));
                }
            }
        }
    }
}

