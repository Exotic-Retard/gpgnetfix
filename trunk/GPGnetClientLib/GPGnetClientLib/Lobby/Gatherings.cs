namespace GPGnetClientLib.Lobby
{
    using GPGnetClientLib;
    using GPGnetClientLib.ClientHandlers;
    using GPGnetCommunicationsLib;
    using System;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class Gatherings
    {
        private static bool sCreateRoomResult;
        private static bool sCreatingRoom;
        private static IPEndPoint sEP1;
        private static IPEndPoint sEP2;
        private static bool sGettingStunResults;
        private static bool sJoiningRoom;
        private static bool sJoinRoomResult;
        private static bool sLeaveRoomResult;
        private static bool sLeavingRoom;

        public static  event ChatroomMessage OnChatroomMessage;

        public static  event GPGnetClientLib.Lobby.PrivateMessage OnPrivateMessage;

        private static void client_OnStunResults(IPEndPoint ep1, IPEndPoint ep2)
        {
            sEP1 = ep1;
            sEP2 = ep2;
            sGettingStunResults = false;
        }

        public static bool CreateRoom(string kind, string description, string password)
        {
            return CreateRoom(kind, description, password, new string[] { "" });
        }

        public static bool CreateRoom(string kind, string description, string password, string[] buffer)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.CreateRoom
            };
            command.SetParams(new object[] { kind, description, password, buffer });
            sCreatingRoom = true;
            sCreateRoomResult = false;
            GPGnetClientLib.ClientHandlers.CreateRoom.OnCreateGathering += new CreateGatheringResponse(Gatherings.CreateRoom_OnCreateGathering);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sCreatingRoom && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sCreateRoomResult;
        }

        private static void CreateRoom_OnCreateGathering(bool succeeded)
        {
            sCreateRoomResult = succeeded;
            sCreatingRoom = false;
        }

        public static bool GetStunResults(int selfport, out IPEndPoint ep1, out IPEndPoint ep2)
        {
            UDPStunClient client = new UDPStunClient();
            client.OnStunResults += new StunResults(Gatherings.client_OnStunResults);
            sGettingStunResults = true;
            client.CheckNAT(selfport, ClientManager.GetHostName());
            int tickCount = Environment.TickCount;
            while (sGettingStunResults && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            bool flag = !sGettingStunResults;
            ep1 = sEP1;
            ep2 = sEP2;
            return flag;
        }

        public static void Initialize()
        {
            TextMessageRoom.OnTextMessage += new TextMessageResponse(Gatherings.TextMessageRoom_OnTextMessage);
            TextMessageUser.OnTextUserMessage += new TextMessageUserResponse(Gatherings.TextMessageUser_OnTextUserMessage);
        }

        public static bool JoinRoom(string description, string password)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.JoinRoom
            };
            command.SetParams(new object[] { description, password });
            sJoiningRoom = true;
            sJoinRoomResult = false;
            GPGnetClientLib.ClientHandlers.JoinRoom.OnJoinGathering += new JoinGatheringResponse(Gatherings.JoinRoom_OnJoinGathering);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sJoiningRoom && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sJoinRoomResult;
        }

        private static void JoinRoom_OnJoinGathering(bool succeeded)
        {
            sJoinRoomResult = succeeded;
            sJoiningRoom = false;
        }

        public static bool LeaveRoom(string description)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.LeaveRoom
            };
            command.SetParams(new object[] { description });
            sLeavingRoom = true;
            sLeaveRoomResult = false;
            GPGnetClientLib.ClientHandlers.LeaveRoom.OnLeaveGathering += new LeaveGatheringResponse(Gatherings.LeaveRoom_OnLeaveGathering);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sLeavingRoom && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sLeaveRoomResult;
        }

        private static void LeaveRoom_OnLeaveGathering(bool succeeded)
        {
            sLeaveRoomResult = succeeded;
            sLeavingRoom = false;
        }

        public static void PrivateMessage(string username, string message)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.TextMessageUser
            };
            command.SetParams(new object[] { username, message });
            ClientManager.GetManager().MessageServer(command);
        }

        public static void TextMessage(string roomname, string message)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.TextMessageRoom
            };
            command.SetParams(new object[] { roomname, message });
            ClientManager.GetManager().MessageServer(command);
        }

        private static void TextMessageRoom_OnTextMessage(string roomname, string message, Credentials sourceuser)
        {
            ClientManager.LogData("<" + sourceuser.Name + ":" + roomname + "> " + message);
            if (OnChatroomMessage != null)
            {
                OnChatroomMessage(sourceuser, roomname, message);
            }
        }

        private static void TextMessageUser_OnTextUserMessage(string message, Credentials sourceuser)
        {
            if (OnPrivateMessage != null)
            {
                OnPrivateMessage(sourceuser, message);
            }
        }
    }
}

