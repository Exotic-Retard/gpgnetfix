namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using GPG.Logging;
    using GPG.Multiplayer.Client.Games;
    using System;
    using System.Drawing;

    public class SpaceSiegeGameParticipant : GPGnetGameParticipant
    {
        private PlayerCharacter mCharacter;

        public SpaceSiegeGameParticipant(SpaceSiegeGame game) : base(game)
        {
        }

        public SpaceSiegeGameParticipant(string name, int id) : base(name, id)
        {
        }

        public SpaceSiegeGameParticipant(SpaceSiegeGame game, string name, int id) : base(game, name, id)
        {
        }

        protected override void FromDataString(string[] tokens)
        {
            base.FromDataString(tokens);
            try
            {
                if (tokens.Length > 4)
                {
                    this.mCharacter = new PlayerCharacter();
                    this.Character.CharacterName = tokens[4];
                    this.Character.CharacterColor = Color.FromArgb(int.Parse(tokens[5]));
                    this.Character.Upgrades = int.Parse(tokens[6]);
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public override void MergeParticipants(GPGnetGameParticipant participant)
        {
            base.MergeParticipants(participant);
            if (this.Character == null)
            {
                this.mCharacter = (participant as SpaceSiegeGameParticipant).Character;
            }
            else
            {
                this.mCharacter.CharacterName = (participant as SpaceSiegeGameParticipant).Character.CharacterName;
                this.mCharacter.CharacterColor = (participant as SpaceSiegeGameParticipant).Character.CharacterColor;
            }
        }

        public override string ToDataString()
        {
            if (this.Character != null)
            {
                return (base.ToDataString() + ";" + this.Character.CharacterName + ";" + this.Character.CharacterColor.ToArgb().ToString() + ";" + this.Character.Upgrades.ToString());
            }
            return base.ToDataString();
        }

        public PlayerCharacter Character
        {
            get
            {
                return this.mCharacter;
            }
            set
            {
                this.mCharacter = value;
            }
        }
    }
}

