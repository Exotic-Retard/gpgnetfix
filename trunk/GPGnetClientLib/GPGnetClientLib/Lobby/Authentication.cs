namespace GPGnetClientLib.Lobby
{
    using GPGnetClientLib;
    using GPGnetClientLib.ClientHandlers;
    using GPGnetCommunicationsLib;
    using System;
    using System.Threading;

    public static class Authentication
    {
        private static bool sChangePasswordResult;
        private static bool sChangingPassword;
        private static bool sCreateLoginResult;
        private static bool sCreatingLogin;
        private static bool sEmailingUsername;
        private static bool sEmailUsernameResult;
        private static bool sLogginIn;
        private static bool sLoginResult;
        private static bool sResetPasswordResult;
        private static bool sResettingPassword;

        public static bool ChangePassword(string oldpass, string newpass)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.ChangePassword
            };
            command.SetParams(new object[] { oldpass, newpass });
            sChangingPassword = true;
            sChangePasswordResult = false;
            GPGnetClientLib.ClientHandlers.ChangePassword.OnChangePassword += new ChangePasswordResponse(Authentication.ChangePassword_OnChangePassword);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sChangingPassword && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sChangePasswordResult;
        }

        private static void ChangePassword_OnChangePassword(bool result)
        {
            sChangePasswordResult = result;
            sChangingPassword = false;
        }

        public static bool CreateLogin(string name, string password, string email)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.CreateLogin
            };
            command.SetParams(new object[] { name, password, email });
            sCreatingLogin = true;
            sCreateLoginResult = false;
            GPGnetClientLib.ClientHandlers.CreateLogin.OnCreateLogin += new CreateLoginResponse(Authentication.CreateLogin_OnCreateLogin);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sCreatingLogin && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sCreateLoginResult;
        }

        private static void CreateLogin_OnCreateLogin(bool createdlogin)
        {
            sCreateLoginResult = createdlogin;
            sCreatingLogin = false;
        }

        public static bool EmailUsername(string cdkey)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.EmailUsername
            };
            command.SetParams(new object[] { cdkey });
            sEmailingUsername = true;
            sEmailUsernameResult = false;
            GPGnetClientLib.ClientHandlers.EmailUsername.OnEmailUsername += new EmailUsernameResponse(Authentication.EmailUsername_OnEmailUsername);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sEmailingUsername && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sEmailUsernameResult;
        }

        private static void EmailUsername_OnEmailUsername(bool result)
        {
            sEmailUsernameResult = result;
            sEmailingUsername = false;
        }

        public static bool Login(string name, string password)
        {
            if (LoggedIn)
            {
                throw new Exception("You are already logged in and must first logout.");
            }
            CommandMessage command = new CommandMessage {
                CommandName = Commands.Login
            };
            command.SetParams(new object[] { name, password });
            sLogginIn = true;
            sLoginResult = false;
            GPGnetClientLib.ClientHandlers.Login.OnLogin += new LoginResponse(Authentication.Login_OnLogin);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sLogginIn && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sLoginResult;
        }

        private static void Login_OnLogin(bool loggedin, Credentials credentials)
        {
            sLoginResult = loggedin;
            sLogginIn = false;
        }

        public static bool Logout()
        {
            ClientManager.GetManager().Disconnect();
            return true;
        }

        public static bool ResetPassword(string username, string email)
        {
            CommandMessage command = new CommandMessage {
                CommandName = Commands.ResetPassword
            };
            command.SetParams(new object[] { username, email });
            sResettingPassword = true;
            sResetPasswordResult = false;
            GPGnetClientLib.ClientHandlers.ResetPassword.OnResetPassword += new ResetPasswordResponse(Authentication.ResetPassword_OnResetPassword);
            ClientManager.GetManager().MessageServer(command);
            int tickCount = Environment.TickCount;
            while (sResettingPassword && ((tickCount + GPGnetClientLib.Lobby.Lobby.Timeout) > Environment.TickCount))
            {
                Thread.Sleep(10);
            }
            return sResetPasswordResult;
        }

        private static void ResetPassword_OnResetPassword(bool result)
        {
            sResetPasswordResult = result;
            sResettingPassword = false;
        }

        public static bool LoggedIn
        {
            get
            {
                return sLoginResult;
            }
        }
    }
}

