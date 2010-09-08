namespace GPG.Multiplayer.Client
{
    using GPG.Multiplayer.LadderService;
    using System;
    using System.Collections.Generic;

    internal class LadderMenuItemSorter : IComparer<LadderInstance>
    {
        public int Compare(LadderInstance x, LadderInstance y)
        {
            if (x.ID == y.ID)
            {
                return 0;
            }
            if (y.SortOrder < x.SortOrder)
            {
                return 1;
            }
            return -1;
        }
    }
}

