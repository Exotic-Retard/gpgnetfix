namespace GPG.Multiplayer.Plugin
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public interface IPersistentData
    {
        event GetMessage OnGetMessage;

        List<List<string>> AdhocData(string query);
        List<List<string>> AdhocData(string query, out List<string> columns);
        object[] CustomCommand(string commandname, params object[] data);
        int Execute(string queryName, params object[] data);
        List<List<string>> GetData(string queryName, params object[] data);
        List<string> GetRow(string queryName, params object[] data);
        bool PlayGame(string gamename, string gametype);
        bool SendMessage(string targetPlayerName, string command, params object[] args);
    }
}

