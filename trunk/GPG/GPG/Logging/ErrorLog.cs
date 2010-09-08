namespace GPG.Logging
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class ErrorLog
    {
        public static  event LogEvent OnLogErrorEvent;

        public static Exception WriteLine(Exception ex)
        {
            if (ex != null)
            {
                WriteLine("{0}\r\nSTACK TRACE: {1}", new object[] { ex.Message, ex.StackTrace });
                if (ex.InnerException != null)
                {
                    WriteLineInner(ex.InnerException);
                }
                if (OnLogErrorEvent != null)
                {
                    OnLogErrorEvent("EXCEPTION", "Error", ex);
                }
                return ex;
            }
            WriteLine("An error occured with no details.", new object[0]);
            if (OnLogErrorEvent != null)
            {
                OnLogErrorEvent("An error occured with no details.", "Error", null);
            }
            return ex;
        }

        public static void WriteLine(string text, params object[] args)
        {
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                Console.WriteLine(string.Format("ERROR: " + text, args));
                if (OnLogErrorEvent != null)
                {
                    OnLogErrorEvent(text, "Error", args);
                }
            });
        }

        private static void WriteLineInner(Exception ex)
        {
            ThreadPool.QueueUserWorkItem(delegate (object s) {
                Console.WriteLine(string.Format("INNER ERROR: {0}\r\nSTACK TRACE: {1}", ex.Message, ex.StackTrace));
                if (OnLogErrorEvent != null)
                {
                    OnLogErrorEvent("INNER EXCEPTION", "Error", ex);
                }
                if (ex.InnerException != null)
                {
                    WriteLineInner(ex.InnerException);
                }
            });
        }
    }
}

