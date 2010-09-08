namespace GPG.Multiplayer.Client
{
    using System;

    public class UserRequestEventArgs : EventArgs
    {
        private IUserRequest mRequest;

        public UserRequestEventArgs(IUserRequest request)
        {
            this.mRequest = request;
        }

        public IUserRequest Request
        {
            get
            {
                return this.mRequest;
            }
        }
    }
}

