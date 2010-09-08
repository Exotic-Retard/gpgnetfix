namespace GPG.Multiplayer.Quazal
{
    using GPG.Logging;
    using System;
    using System.Runtime.InteropServices;

    public class LobbyExtension
    {
        public bool ChangePassword(string password)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.ChangePassword(password);
                }
                flag = mChangePassword(password);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                flag = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }

        [DllImport("MultiplayerBackend.dll", EntryPoint="ChangePassword", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mChangePassword([MarshalAs(UnmanagedType.LPStr)] string password);
        [DllImport("MultiplayerBackend.dll", EntryPoint="ResetPassword", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool mResetPassword([MarshalAs(UnmanagedType.LPStr)] string username, [MarshalAs(UnmanagedType.LPStr)] string email);
        public bool ResetPassword(string username, string email)
        {
            bool flag;
            Lobby.Lock();
            try
            {
                Lobby.IncDBCallCount(new object[0]);
                if (Lobby.sProtocol != null)
                {
                    return Lobby.sProtocol.ResetPassword(username, email);
                }
                flag = mResetPassword(username, email);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                flag = false;
            }
            finally
            {
                Lobby.Unlock();
            }
            return flag;
        }
    }
}

