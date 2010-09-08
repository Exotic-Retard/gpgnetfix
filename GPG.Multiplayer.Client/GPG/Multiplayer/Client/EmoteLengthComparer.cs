namespace GPG.Multiplayer.Client
{
    using System;
    using System.Collections.Generic;

    internal class EmoteLengthComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (x > y)
            {
                return -1;
            }
            return 1;
        }
    }
}

