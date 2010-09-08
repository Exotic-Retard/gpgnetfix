namespace GPG.Logging
{
    using System;
    using System.Runtime.CompilerServices;

    public static class AuditLog
    {
        public static  event LogEvent OnLogEvent;

        public static void WriteLine(string text, params object[] args)
        {
            WriteLine(text, LogCategory.Get("AUDIT"), args);
        }

        public static void WriteLine(string text, LogCategory classification, params object[] args)
        {
            try
            {
                Console.WriteLine(string.Format(classification.Name + ": " + text, args));
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

