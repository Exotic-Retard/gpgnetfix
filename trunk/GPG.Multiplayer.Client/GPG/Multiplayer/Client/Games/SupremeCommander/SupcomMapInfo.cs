namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable]
    public class SupcomMapInfo
    {
        private string mDescription;
        private string mMapID;
        private int mMaxPlayers;
        private bool? mPriority;
        private bool mSelectable;
        [NonSerialized]
        private Image mThumbnail;

        public SupcomMapInfo()
        {
            this.mPriority = null;
            this.mMaxPlayers = 0;
            this.mSelectable = true;
        }

        public SupcomMapInfo(string mapID)
        {
            this.mPriority = null;
            this.mMaxPlayers = 0;
            this.mSelectable = true;
            this.mMapID = mapID;
        }

        public override bool Equals(object obj)
        {
            return ((obj is SupcomMapInfo) && ((obj as SupcomMapInfo).MapID == this.MapID));
        }

        public override int GetHashCode()
        {
            return this.MapID.GetHashCode();
        }

        public override string ToString()
        {
            return this.Description;
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
            set
            {
                this.mDescription = value;
            }
        }

        public string MapID
        {
            get
            {
                return this.mMapID;
            }
            set
            {
                this.mMapID = value;
            }
        }

        public int MaxPlayers
        {
            get
            {
                return this.mMaxPlayers;
            }
            set
            {
                this.mMaxPlayers = value;
            }
        }

        public bool? Priority
        {
            get
            {
                return this.mPriority;
            }
            set
            {
                this.mPriority = value;
            }
        }

        public bool Selectable
        {
            get
            {
                return this.mSelectable;
            }
            set
            {
                this.mSelectable = value;
            }
        }

        [Browsable(false)]
        public Image Thumbnail
        {
            get
            {
                return this.mThumbnail;
            }
            set
            {
                this.mThumbnail = value;
            }
        }
    }
}

