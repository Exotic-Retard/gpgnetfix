namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public class PlayerInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        private string _Icon;
        private double _Rating;
        private int _Wins;
        private int _Losses;
        private int _Draws;
        [MarshalAs(UnmanagedType.LPStr)]
        private string _Description;
        [MarshalAs(UnmanagedType.LPStr)]
        private string _Postalcode;
        public override string ToString()
        {
            return this.Description;
        }

        public string Icon
        {
            get
            {
                return this._Icon;
            }
            set
            {
                this._Icon = value;
            }
        }
        public Image GetIcon()
        {
            if (this.Icon != null)
            {
                try
                {
                    new MemoryStream(Convert.FromBase64String(this.Icon));
                    return null;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public double Rating
        {
            get
            {
                return this._Rating;
            }
            set
            {
                this.Rating = value;
            }
        }
        public int Wins
        {
            get
            {
                return this._Wins;
            }
            set
            {
                this.Wins = value;
            }
        }
        public int Losses
        {
            get
            {
                return this._Losses;
            }
            set
            {
                this.Losses = value;
            }
        }
        public int Draws
        {
            get
            {
                return this._Draws;
            }
            set
            {
                this.Draws = value;
            }
        }
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
            }
        }
        public string Postalcode
        {
            get
            {
                return this._Postalcode;
            }
            set
            {
                this._Postalcode = value;
            }
        }
    }
}

