namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.DataAccess;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Runtime.CompilerServices;

    public interface IGatheringDisplay
    {
        event StringEventHandler GatheringSelected;

        event EventHandler Popup;

        void Refresh();
        void RefreshGatherings(Chatroom[] rooms, bool clearFirst);
        void RefreshGatherings(MappedObjectList<Chatroom> rooms, bool clearFirst);

        Chatroom CurrentRoom { get; set; }

        int RoomCount { get; }
    }
}

