namespace GPG.Multiplayer.Client.Games.SpaceSiege
{
    using GPG.Multiplayer.Client.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PnlCharacterDisplay : PnlBase
    {
        private IContainer components = null;
        private Bitmap HRVImage = null;
        private PlayerCharacter mCharacter;
        private Bitmap PlayerImage = null;

        public PnlCharacterDisplay()
        {
            this.InitializeComponent();
        }

        public void BindToCharacter(PlayerCharacter character)
        {
            this.mCharacter = character;
            if (character == null)
            {
                this.PlayerImage = null;
                this.HRVImage = null;
            }
            else
            {
                string str = "red";
                if (character.CharacterColor.ToArgb() == Color.Blue.ToArgb())
                {
                    str = "blue";
                }
                else if (character.CharacterColor.ToArgb() == Color.Green.ToArgb())
                {
                    str = "green";
                }
                else if (character.CharacterColor.ToArgb() == Color.Magenta.ToArgb())
                {
                    str = "magenta";
                }
                string str2 = "red";
                if (character.RobotColor.ToArgb() == Color.Blue.ToArgb())
                {
                    str2 = "blue";
                }
                else if (character.RobotColor.ToArgb() == Color.Green.ToArgb())
                {
                    str2 = "green";
                }
                else if (character.RobotColor.ToArgb() == Color.Magenta.ToArgb())
                {
                    str2 = "magenta";
                }
                this.PlayerImage = (Bitmap) SpaceSiegeImages.ResourceManager.GetObject(string.Format("player_{0}_{1}", str, character.Head));
                this.HRVImage = (Bitmap) SpaceSiegeImages.ResourceManager.GetObject(string.Format("hrv_{0}", str2));
            }
            this.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Transparent;
            base.Name = "PnlCharacterDisplay";
            base.Size = new Size(0x14c, 0x17f);
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.PlayerImage != null)
            {
                e.Graphics.DrawImage(this.PlayerImage, 12, 14, this.PlayerImage.Width, this.PlayerImage.Height);
            }
            if (this.HRVImage != null)
            {
                e.Graphics.DrawImage(this.HRVImage, 0x76, 0x6b, this.HRVImage.Width, this.HRVImage.Height);
            }
        }

        public PlayerCharacter Character
        {
            get
            {
                return this.mCharacter;
            }
        }

        protected override bool ScaleChildren
        {
            get
            {
                return false;
            }
        }
    }
}

