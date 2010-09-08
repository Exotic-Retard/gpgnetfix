namespace GPG.Multiplayer.Quazal.SolutionsLib
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("System.Xml", "2.0.50727.1433"), XmlType(Namespace="http://gpgnet.gaspowered.com/")]
    public class PlayerRanking
    {
        private int disconnectsField;
        private int drawsField;
        private int lossesField;
        private string playerNameField;
        private int principalIDField;
        private int rankField;
        private double ratingField;
        private int winsField;

        public int Disconnects
        {
            get
            {
                return this.disconnectsField;
            }
            set
            {
                this.disconnectsField = value;
            }
        }

        public int Draws
        {
            get
            {
                return this.drawsField;
            }
            set
            {
                this.drawsField = value;
            }
        }

        public int Losses
        {
            get
            {
                return this.lossesField;
            }
            set
            {
                this.lossesField = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.playerNameField;
            }
            set
            {
                this.playerNameField = value;
            }
        }

        public int PrincipalID
        {
            get
            {
                return this.principalIDField;
            }
            set
            {
                this.principalIDField = value;
            }
        }

        public int Rank
        {
            get
            {
                return this.rankField;
            }
            set
            {
                this.rankField = value;
            }
        }

        public double Rating
        {
            get
            {
                return this.ratingField;
            }
            set
            {
                this.ratingField = value;
            }
        }

        public int Wins
        {
            get
            {
                return this.winsField;
            }
            set
            {
                this.winsField = value;
            }
        }
    }
}

