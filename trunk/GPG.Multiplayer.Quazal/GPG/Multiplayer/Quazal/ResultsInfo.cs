namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto, Pack=2)]
    public class ResultsInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        private string mPlayerName = "";
        //private EventHandler PlayerNameChanged;
        private int mWonGame;
        //private EventHandler WonGameChanged;
        public string PlayerName
        {
            get
            {
                return this.mPlayerName;
            }
            set
            {
                if (this.mPlayerName != value)
                {
                    this.mPlayerName = value;
                    if (this.PlayerNameChanged != null)
                    {
                        this.PlayerNameChanged(this, new EventArgs());
                    }
                }
            }
        }
        public int WonGame
        {
            get
            {
                return this.mWonGame;
            }
            set
            {
                if (this.mWonGame != value)
                {
                    this.mWonGame = value;
                    if (this.WonGameChanged != null)
                    {
                        this.WonGameChanged(this, new EventArgs());
                    }
                }
            }
        }
        public event EventHandler PlayerNameChanged;
        public event EventHandler WonGameChanged;
    }
}

