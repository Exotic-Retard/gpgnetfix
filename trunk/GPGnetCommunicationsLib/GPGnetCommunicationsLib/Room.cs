namespace GPGnetCommunicationsLib
{
    using System;
    using System.Collections.Generic;

    public class Room
    {
        private string mKind;
        private string mName;
        private Credentials mOwner;
        private string mPassword;
        private int mRoomID;
        private List<Credentials> mUsers = new List<Credentials>();

        public string Kind
        {
            get
            {
                return this.mKind;
            }
            set
            {
                this.mKind = value;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public Credentials Owner
        {
            get
            {
                return this.mOwner;
            }
            set
            {
                this.mOwner = value;
            }
        }

        public string Password
        {
            get
            {
                return this.mPassword;
            }
            set
            {
                this.mPassword = value;
            }
        }

        public int RoomID
        {
            get
            {
                return this.mRoomID;
            }
            set
            {
                this.mRoomID = value;
            }
        }

        public List<Credentials> Users
        {
            get
            {
                return this.mUsers;
            }
            set
            {
                this.mUsers = value;
            }
        }
    }
}

