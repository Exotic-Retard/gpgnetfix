namespace GPG.Multiplayer.Client
{
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class UserPrefs_Appearance_Layout
    {
        private Dictionary<string, FormLayout> mFormLayouts = new Dictionary<string, FormLayout>();
        private bool mRememberLayout = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler RememberLayoutChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler RestoreWindowsChanged;

        [Browsable(false)]
        public Dictionary<string, FormLayout> FormLayouts
        {
            get
            {
                return this.mFormLayouts;
            }
            set
            {
                this.mFormLayouts = value;
            }
        }

        [Category("<LOC>Misc"), Description("<LOC>If true, the program will the size and locations of appropriate windows between uses."), DisplayName("<LOC>Remember Layout")]
        public bool RememberLayout
        {
            get
            {
                return (ConfigSettings.GetBool("RememberLayout", true) && this.mRememberLayout);
            }
            set
            {
                this.mRememberLayout = value;
                if (this.mRememberLayoutChanged != null)
                {
                    this.mRememberLayoutChanged(this, new PropertyChangedEventArgs("RememberLayout"));
                }
            }
        }
    }
}

