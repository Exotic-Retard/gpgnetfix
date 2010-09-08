namespace GPG.Logging
{
    using System;

    public class LogCategory
    {
        public string Name = "Event";

        public static LogCategory Get(string name)
        {
            return new LogCategory { Name = name };
        }
    }
}

