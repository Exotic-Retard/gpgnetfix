namespace GPG.Multiplayer.Client.Clans
{
    using GPG.Logging;
    using System;
    using System.Collections.Generic;

    public class SeniorityComparer : IComparer<ClanMember>
    {
        public int Compare(ClanMember x, ClanMember y)
        {
            try
            {
                if (x.Rank < y.Rank)
                {
                    return -1;
                }
                if (y.Rank >= x.Rank)
                {
                    DateTime dateJoined = x.DateJoined;
                    DateTime time2 = y.DateJoined;
                    if (dateJoined == time2)
                    {
                        return 0;
                    }
                    if (dateJoined > time2)
                    {
                        return -1;
                    }
                }
                return 1;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return -1;
            }
        }
    }
}

