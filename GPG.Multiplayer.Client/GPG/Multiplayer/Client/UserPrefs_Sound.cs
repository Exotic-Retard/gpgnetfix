namespace GPG.Multiplayer.Client
{
    using GPG;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Sound
    {
        private UserPrefs_Sound_Speech mSpeech;

        [Browsable(false), OptionsRoot("<LOC>Speech")]
        public UserPrefs_Sound_Speech Speech
        {
            get
            {
                if (this.mSpeech == null)
                {
                    this.mSpeech = new UserPrefs_Sound_Speech();
                }
                return this.mSpeech;
            }
        }
    }
}

