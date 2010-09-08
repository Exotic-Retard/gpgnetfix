namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class PopupPrefs
    {
        private double mDuration = 8.0;
        private double mFadeTimePercent = 0.1;

        [field: NonSerialized]
        public event PropertyChangedEventHandler DurationChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler FadeTimePercentChanged;

        [Description("<LOC>How long a popup message should last."), Category("<LOC>Misc"), DisplayName("<LOC>Duration")]
        public double Duration
        {
            get
            {
                return this.mDuration;
            }
            set
            {
                this.mDuration = value;
                if (this.DurationChanged != null)
                {
                    this.DurationChanged(this, new PropertyChangedEventArgs("Duration"));
                }
            }
        }

        [Description("<LOC>Sets the percentage of the message duration spent on fading in and out."), Category("<LOC>Misc"), DisplayName("<LOC>Fade Time Percent")]
        public double FadeTimePercent
        {
            get
            {
                return this.mFadeTimePercent;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentException("The fade time must >= 0");
                }
                if (value > 0.5)
                {
                    throw new ArgumentException("The fade time must be <= 0.5)");
                }
                this.mFadeTimePercent = value;
                if (this.FadeTimePercentChanged != null)
                {
                    this.FadeTimePercentChanged(this, new PropertyChangedEventArgs("FadeTimePercent"));
                }
            }
        }
    }
}

