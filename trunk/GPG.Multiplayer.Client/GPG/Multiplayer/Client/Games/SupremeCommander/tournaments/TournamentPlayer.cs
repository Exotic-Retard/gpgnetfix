namespace GPG.Multiplayer.Client.Games.SupremeCommander.tournaments
{
    using System;
    using System.Collections.Generic;

    public class TournamentPlayer : IComparable
    {
        public int Draws;
        public int Losses;
        public List<TournamentMatchup> Matchups = new List<TournamentMatchup>();
        public string Name;
        public int Seed;
        public int Standing;
        public int Wins;

        public int CompareTo(object obj)
        {
            if (obj != this)
            {
                TournamentPlayer player = obj as TournamentPlayer;
                if (obj != null)
                {
                    if (this.Score > player.Score)
                    {
                        return -2;
                    }
                    if (this.Score < player.Score)
                    {
                        return 2;
                    }
                    if (this.Seed > player.Seed)
                    {
                        return 1;
                    }
                    if (this.Seed < player.Seed)
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
            return string.Concat(new object[] { this.Standing, ".".PadRight(4, ' '), this.Name, " (", this.Wins.ToString(), "-", this.Losses.ToString(), "-", this.Draws.ToString(), ")" });
        }

        public float Score
        {
            get
            {
                return (this.Wins + (0.5f * this.Draws));
            }
        }
    }
}

