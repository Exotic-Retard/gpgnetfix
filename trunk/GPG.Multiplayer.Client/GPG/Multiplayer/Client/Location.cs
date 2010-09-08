namespace GPG.Multiplayer.Client
{
    using System;
    using System.Drawing;

    public class Location
    {
        private string mDescription;
        private Image mIcon;
        private LocationTypes mLocationType;
        private string mValue;

        public Location(LocationTypes type, string value)
        {
            this.mLocationType = type;
            this.mValue = value;
            switch (this.LocationType)
            {
            }
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

        public Image Icon
        {
            get
            {
                return this.mIcon;
            }
            set
            {
                this.mIcon = value;
            }
        }

        public LocationTypes LocationType
        {
            get
            {
                return this.mLocationType;
            }
            set
            {
                this.mLocationType = value;
            }
        }

        public string Value
        {
            get
            {
                return this.mValue;
            }
            set
            {
                this.mValue = value;
            }
        }
    }
}

