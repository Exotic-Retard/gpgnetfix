namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using GPG.Multiplayer.Client.Games;
    using System;

    public class SpaceSiegeGame : GPGnetGame
    {
        public SpaceSiegeGame()
        {
        }

        public SpaceSiegeGame(SpaceSiegeGameParticipant leader) : base(leader)
        {
        }

        public override GPGnetGameParticipant CreateMember()
        {
            return new SpaceSiegeGameParticipant(this);
        }

        protected override void FromDataString(string[] tokens)
        {
            base.FromDataString(tokens);
        }

        public override string ToDataString()
        {
            return base.ToDataString();
        }

        public override int MaxGameParticipants
        {
            get
            {
                return 4;
            }
        }
    }
}

