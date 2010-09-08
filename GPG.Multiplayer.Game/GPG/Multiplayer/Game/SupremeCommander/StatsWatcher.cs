namespace GPG.Multiplayer.Game.SupremeCommander
{
    using GPG.Logging;
    using System;
    using System.IO;
    using System.Threading;

    public class StatsWatcher
    {
        private string mLogDir = "";
        private SupComGameManager mManager;
        private bool mProcessing;
        private int mStatsCount = 1;
        private Thread mThread;
        public static bool SaveStats = false;
        public static string StatsLocation = @"c:\temp\";
        public string Xml = "";

        public StatsWatcher(SupComGameManager manager)
        {
            this.mManager = manager;
            this.mManager.OnStatsXML += new StatsXML(this.manager_OnStatsXML);
        }

        private void manager_OnStatsXML(string xml)
        {
            this.Xml = xml;
            if (SaveStats)
            {
                try
                {
                    if (Directory.Exists(StatsLocation))
                    {
                        if (this.mStatsCount == 1)
                        {
                            int num = 1;
                            this.mLogDir = StatsLocation + @"\Game" + num.ToString() + @"\";
                            while (Directory.Exists(this.mLogDir))
                            {
                                num++;
                                this.mLogDir = StatsLocation + @"\Game" + num.ToString() + @"\";
                            }
                            Directory.CreateDirectory(this.mLogDir);
                        }
                        StreamWriter writer = new StreamWriter(this.mLogDir + "Stats" + this.mStatsCount.ToString() + ".xml");
                        writer.Write(xml);
                        writer.Close();
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                this.mStatsCount++;
            }
        }

        private void ProcessStats()
        {
            this.mProcessing = true;
            while (this.mProcessing)
            {
                try
                {
                    Thread.Sleep(0xea60);
                    this.mManager.GetStats();
                    continue;
                }
                catch (ThreadInterruptedException exception)
                {
                    EventLog.WriteLine("The thread was woken up: " + exception.Message, LogCategory.Get("StatsWatcher"), new object[0]);
                    continue;
                }
            }
        }

        public void Start()
        {
            this.mThread = new Thread(new ThreadStart(this.ProcessStats));
            this.mThread.Start();
        }

        public void Stop()
        {
            if (this.mProcessing)
            {
                this.mProcessing = false;
                this.mThread.Interrupt();
            }
        }
    }
}

