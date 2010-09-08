namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Drawing;
    using System.IO;

    public class PlayerCharacter : MappedObject
    {
        [FieldMap("character_color")]
        private int mCharacterColor;
        [FieldMap("character_name")]
        private string mCharacterName;
        private string mFilePath;
        [FieldMap("guid")]
        private string mGuid;
        private int mHead;
        [FieldMap("space_siege_character_id")]
        private int mID;
        [FieldMap("player_id")]
        private int mPlayerID;
        [FieldMap("player_name")]
        private string mPlayerName;
        [FieldMap("robot_color")]
        private int mRobotColor;
        [FieldMap("time_played")]
        private string mTimePlayed;
        private int mUpgrades;

        public PlayerCharacter()
        {
            this.mCharacterName = "new_character";
            this.mUpgrades = 0;
            this.mHead = 1;
            this.mTimePlayed = "0";
            this.mCharacterColor = Color.Red.ToArgb();
            this.mRobotColor = Color.Red.ToArgb();
        }

        public PlayerCharacter(DataRecord record) : base(record)
        {
            this.mCharacterName = "new_character";
            this.mUpgrades = 0;
            this.mHead = 1;
            this.mTimePlayed = "0";
            this.mCharacterColor = Color.Red.ToArgb();
            this.mRobotColor = Color.Red.ToArgb();
        }

        public static PlayerCharacter FromString(string data)
        {
            PlayerCharacter character = new PlayerCharacter();
            string[] strArray = data.Split(new char[] { ',' });
            if (strArray.Length > 2)
            {
                character.CharacterName = strArray[0];
                try
                {
                    character.Upgrades = Convert.ToInt32(strArray[2]);
                }
                catch
                {
                }
                try
                {
                    character.TimePlayed = strArray[1].ToString();
                }
                catch
                {
                }
            }
            return character;
        }

        public void Save()
        {
            string contents = "";
            File.WriteAllText(this.FilePath, contents);
        }

        public void SaveAs(string filePath)
        {
            this.mFilePath = filePath;
            this.Save();
        }

        public Color CharacterColor
        {
            get
            {
                return Color.FromArgb(this.mCharacterColor);
            }
            set
            {
                this.mCharacterColor = value.ToArgb();
            }
        }

        public static string CharacterFilePath
        {
            get
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + ConfigSettings.GetString("SS Save Location", @"\My Games\Space Siege\Save\MultiPlayer\");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public string CharacterName
        {
            get
            {
                return this.mCharacterName;
            }
            set
            {
                this.mCharacterName = value;
            }
        }

        public string FilePath
        {
            get
            {
                return this.mFilePath;
            }
        }

        public string Guid
        {
            get
            {
                return this.mGuid;
            }
            set
            {
                this.mGuid = value;
            }
        }

        public int Head
        {
            get
            {
                return this.mHead;
            }
            set
            {
                this.mHead = value;
            }
        }

        public int ID
        {
            get
            {
                return this.mID;
            }
        }

        public int PlayerID
        {
            get
            {
                return this.mPlayerID;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
        }

        public Color RobotColor
        {
            get
            {
                return Color.FromArgb(this.mRobotColor);
            }
            set
            {
                this.mRobotColor = value.ToArgb();
            }
        }

        public string TimePlayed
        {
            get
            {
                return this.mTimePlayed;
            }
            set
            {
                this.mTimePlayed = value;
            }
        }

        public int Upgrades
        {
            get
            {
                return this.mUpgrades;
            }
            set
            {
                this.mUpgrades = value;
            }
        }
    }
}

