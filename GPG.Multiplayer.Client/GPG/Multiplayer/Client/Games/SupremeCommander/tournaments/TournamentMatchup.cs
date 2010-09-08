namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using System;

    public class TournamentMatchup : IComparable
    {
        public int Draws;
        public int Losses;
        public string Name;
        public string OpponentName;
        public string Result;
        public int Round;
        public int Wins;

        public int CompareTo(object obj)
        {
            if (obj != this)
            {
                TournamentMatchup matchup = obj as TournamentMatchup;
                if (obj != null)
                {
                    if (this.Round > matchup.Round)
                    {
                        return 1;
                    }
                    if (this.Round < matchup.Round)
                    {
                        return -1;
                    }
                    return 0;
                }
            }
            return 0;
        }

        public override string ToString()
        {
            return (this.Name + " " + this.Result + " vs " + this.OpponentName);
        }
    }
}

