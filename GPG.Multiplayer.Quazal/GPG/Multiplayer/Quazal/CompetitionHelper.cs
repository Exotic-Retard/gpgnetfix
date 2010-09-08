namespace GPG.Multiplayer.Quazal
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class CompetitionHelper
    {
        public bool AddResult(int gameID, string playerName, string faction, int kills, int deaths, int built, double damageDone, double damageReceived, string teamName, double energyConsumed, double energyProduced, double massConsumed, double massProduced, string result)
        {
            if (Lobby.sProtocol != null)
            {
                return Lobby.sProtocol.AddResult(gameID, playerName, faction, kills, deaths, built, damageDone, damageReceived, teamName, energyConsumed, energyProduced, massConsumed, massProduced, result);
            }
            return mAddResult(gameID, playerName, faction, kills, deaths, built, damageDone, damageReceived, teamName, energyConsumed, energyProduced, massConsumed, massProduced, result);
        }

        public bool AddUnit(int gameID, string playerName, string unitID, int built, int lost, int killed, double damageDone, double damageReceived)
        {
            if (Lobby.sProtocol != null)
            {
                return Lobby.sProtocol.AddUnit(gameID, playerName, unitID, built, lost, killed, damageDone, damageReceived);
            }
            return mAddUnit(gameID, playerName, unitID, built, lost, killed, damageDone, damageReceived);
        }

        public bool CreateGameReport(int gameID, string map, double duration, string gametype)
        {
            string newmap = map;
            switch (newmap)
            {
                case null:
                case "":
                    newmap = "SCMP_018";
                    break;
            }
            if (Lobby.sProtocol != null)
            {
                return Lobby.sProtocol.CreateGameReport(gameID, newmap, duration, gametype);
            }
            return mCreateGameReport(gameID, newmap, duration, gametype);
        }

        [DllImport("MultiplayerBackend.dll", EntryPoint="AddResult", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mAddResult(int gameID, [MarshalAs(UnmanagedType.LPStr)] string playerName, [MarshalAs(UnmanagedType.LPStr)] string faction, int kills, int deaths, int built, double damageDone, double damageReceived, [MarshalAs(UnmanagedType.LPStr)] string teamName, double energyConsumed, double energyProduced, double massConsumed, double massProduced, [MarshalAs(UnmanagedType.LPStr)] string result);
        [DllImport("MultiplayerBackend.dll", EntryPoint="AddUnit", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mAddUnit(int gameID, [MarshalAs(UnmanagedType.LPStr)] string playerName, [MarshalAs(UnmanagedType.LPStr)] string unitID, int built, int lost, int killed, double damageDone, double damageReceived);
        public static IntPtr MarshalArray(ResultsInfo[] MyArray)
        {
            int num = Marshal.SizeOf(typeof(ResultsInfo));
            IntPtr ptr = Marshal.AllocCoTaskMem(num * MyArray.Length);
            int num2 = 0;
            foreach (ResultsInfo info in MyArray)
            {
                int num4 = ptr.ToInt32() + (num2 * num);
                IntPtr ptr2 = new IntPtr(num4);
                Marshal.StructureToPtr(info, ptr2, false);
                num2++;
            }
            return ptr;
        }

        [DllImport("MultiplayerBackend.dll", EntryPoint="CreateGameReport", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mCreateGameReport(int gameID, [MarshalAs(UnmanagedType.LPStr)] string map, double duration, [MarshalAs(UnmanagedType.LPStr)] string gametype);
        [DllImport("MultiplayerBackend.dll", EntryPoint="ReportGame", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mReportGame(int GameID, int Kills, int Deaths, int ArraySize, [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr)] string[] results);
        [DllImport("MultiplayerBackend.dll", EntryPoint="SubmitGameReport", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mSubmitGameReport();
        public bool ReportGame(int GameID, int Kills, int Deaths, ResultsInfo[] results)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.ReportGame(GameID, Kills, Deaths, results);
                }
                ArrayList list = new ArrayList();
                foreach (ResultsInfo info in results)
                {
                    list.Add(info.PlayerName);
                    list.Add(info.WonGame.ToString());
                }
                flag = mReportGame(GameID, Kills, Deaths, list.Count, list.ToArray(typeof(string)) as string[]);
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        public bool SubmitGameReport()
        {
            if (Lobby.sProtocol != null)
            {
                return Lobby.sProtocol.SubmitGameReport();
            }
            return mSubmitGameReport();
        }
    }
}

