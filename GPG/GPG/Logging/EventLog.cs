namespace GPG.Logging
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class EventLog
    {
        public static  event LogEvent OnLogEvent;

        public static void DoStackTrace()
        {
            try
            {
                WriteLine("*** PERFORMING STACK TRACE *** " + Thread.CurrentThread.ManagedThreadId.ToString(), new object[0]);
                StackTrace trace = new StackTrace(true);
                foreach (StackFrame frame in trace.GetFrames())
                {
                    WriteLine(frame.ToString().Replace("\r", "").Replace("\n", ""), new object[0]);
                }
            }
            catch
            {
            }
        }

        public static void WriteLine(string text, params object[] args)
        {
            WriteLine(text, LogCategory.Get("Event"), args);
        }

        public static void WriteLine(string text, LogCategory classification, params object[] args)
        {
            try
            {
                if (args != null)
                {
                    int length = args.Length;
                }
                if (OnLogEvent != null)
                {
                    OnLogEvent(text, classification.Name, args);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }
    }
}

