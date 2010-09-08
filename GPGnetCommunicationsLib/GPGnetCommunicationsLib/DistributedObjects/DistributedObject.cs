namespace GPGnetCommunicationsLib.DistributedObjects
{
    using GPGnetCommunicationsLib;
    using System;

    public class DistributedObject
    {
        private StateObject mOwner;
        private int mTransactionID;
        private bool mUpdating;

        public DistributedObject(StateObject owner)
        {
            this.mOwner = owner;
        }
    }
}

