namespace GPG
{
    using GPG.Threading;
    using SpeechLib;
    using System;

    public static class Speech
    {
        private static int _Voice = 0;
        private static int mMaxDuration = 0;
        private static SpVoice SpeechObj = null;
        private static ThreadQueue SpeechQueue = ThreadQueue.CreateQueue(true);

        public static void Speak(string text)
        {
        }

        public static void Speak(string text, bool overlap)
        {
        }

        public static int MaxDuration
        {
            get
            {
                return 1;
            }
            set
            {
            }
        }

        public static int Rate
        {
            get
            {
                return 1;
            }
            set
            {
            }
        }

        public static int Voice
        {
            get
            {
                return 1;
            }
            set
            {
            }
        }

        public static int Volume
        {
            get
            {
                return 1;
            }
            set
            {
            }
        }
    }
}

