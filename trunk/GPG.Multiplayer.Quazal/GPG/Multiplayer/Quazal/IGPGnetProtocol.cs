namespace GPG.Multiplayer.Quazal
{
    using System;

    public interface IGPGnetProtocol
    {
        bool AddPrivateURL(string address, int port, string mContainerName);
        bool AddQuery(string queryname, string query);
        bool AddResult(int gameID, string playerName, string faction, int kills, int deaths, int built, double damageDone, double damageReceived, string teamName, double energyConsumed, double energyProduced, double massConsumed, double massProduced, string result);
        bool AddUnit(int gameID, string playerName, string unitID, int built, int lost, int killed, double damageDone, double damageReceived);
        bool AdhocExecuteCommand(string SQL);
        bool AdhocExecuteReader(string SQL);
        bool ChangePassword(string password);
        bool CommandMessageChatroom(string mContainername, string Message);
        bool CommandMessagePlayer(string playerName, string message);
        bool CreateChatroom(string ChatroomName);
        bool CreateGame(string description, string mapName, int rating, int p, string gameName, string version, string customData, string mContainerName);
        bool CreateGameReport(int gameID, string newmap, double duration, string gametype);
        bool CreateUser(string username, string password, string email, string server, int port);
        bool DeletePersistantMessage(int messageId, string mContainername);
        bool EndGame();
        bool ExecuteQuery(string queryname, string[] paramlist);
        string FindPlayerByID(int playerid);
        string GetChannelOperator(string mContainername);
        string GetColumns(string Delimiter);
        SNatMessage GetNatMessage();
        string GetNextMessage(string mContainername);
        string GetNextPersistentMessage(string mContainername);
        string GetNextRow(string Delimiter);
        bool GetPersistentMessages(string mContainername);
        string GetQueryDataset(string queryname, string[] paramlist, bool useMaster);
        bool IsConnected();
        bool JoinChatroom(string ChatroomName);
        string JoinGame(string GameName);
        bool LaunchSession(string stationURL);
        bool LeaveChatroom(string mContainername);
        bool LeaveGame();
        bool LoginUser(string username, string password, string server, int port);
        void Logout();
        bool MessagePlayer(string playerName, string message);
        bool PersistMessageChatroom(string mContainername, string message);
        int ProbeStations();
        object ReceiveNatMessage(string address, byte[] message, int size);
        object RegisterAdapter(int header);
        bool RegisterAddress(int port);
        void RegisterNatResponse(MatchmakingHelper.NatMessageDelegatePtr natMessageDelegatePtr);
        bool RegisterUDPConnection();
        bool ReportGame(int GameID, int Kills, int Deaths, ResultsInfo[] results);
        bool ResetPassword(string username, string email);
        bool StartGame();
        bool SubmitGameReport();
        bool TextMessageChatroom(string mContainername, string Message);
        bool UpdateGame(string mGameName, string mMapName, int mRating, int p, string gameName, string mVersion, string mCustomData, string mContainerName);
    }
}

