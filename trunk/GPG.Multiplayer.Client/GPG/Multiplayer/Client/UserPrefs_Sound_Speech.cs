namespace GPG.Multiplayer.Client
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Sound_Speech
    {
        private bool mEnableSpeech = false;
        private int mSpeechRate = 0;
        private int mVoice = 0;
        private int mVolume = 100;

        [field: NonSerialized]
        public event PropertyChangedEventHandler EnableSpeechChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler SpeechRateChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler VoiceChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler VolumeChanged;

        [Description(""), Category("<LOC>Misc"), DisplayName("<LOC>Enable Speech")]
        public bool EnableSpeech
        {
            get
            {
                return this.mEnableSpeech;
            }
            set
            {
                this.mEnableSpeech = value;
                if (this.mEnableSpeechChanged != null)
                {
                    this.mEnableSpeechChanged(this, new PropertyChangedEventArgs("EnableSpeech"));
                }
            }
        }

        [DisplayName("<LOC>Rate of Speech"), Category("<LOC>Misc"), Description("<LOC>Speed of voice messages.")]
        public int SpeechRate
        {
            get
            {
                return this.mSpeechRate;
            }
            set
            {
                this.mSpeechRate = value;
                if (this.mSpeechRateChanged != null)
                {
                    this.mSpeechRateChanged(this, new PropertyChangedEventArgs("SpeechRate"));
                }
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>Chooses actor voice -- by default, a value of 0 - 3."), DisplayName("<LOC>Voice")]
        public int Voice
        {
            get
            {
                return this.mVoice;
            }
            set
            {
                this.mVoice = value;
                if (this.mVoiceChanged != null)
                {
                    this.mVoiceChanged(this, new PropertyChangedEventArgs("Voice"));
                }
            }
        }

        [Description(""), Category("<LOC>Misc"), DisplayName("<LOC>Volume")]
        public int Volume
        {
            get
            {
                return this.mVolume;
            }
            set
            {
                this.mVolume = value;
                if (this.mVolumeChanged != null)
                {
                    this.mVolumeChanged(this, new PropertyChangedEventArgs("Volume"));
                }
            }
        }
    }
}

