namespace GPG.UI.Controls
{
    using System;

    public class GPGMatchup
    {
        private GPGDragItem mPlayer1;
        private GPGDragItem mPlayer2;
        private bool mRepeat;
        private int mRound = 1;

        public GPGDragItem Player1
        {
            get
            {
                return this.mPlayer1;
            }
            set
            {
                this.mPlayer1 = value;
            }
        }

        public GPGDragItem Player2
        {
            get
            {
                return this.mPlayer2;
            }
            set
            {
                this.mPlayer2 = value;
            }
        }

        public bool Repeat
        {
            get
            {
                return this.mRepeat;
            }
            set
            {
                this.mRepeat = value;
            }
        }

        public int Round
        {
            get
            {
                return this.mRound;
            }
            set
            {
                this.mRound = value;
            }
        }
    }
}

