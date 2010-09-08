namespace GPG.Multiplayer.Client.Vaulting
{
    using System;

    internal interface IFTPInfo
    {
        bool AlreadyUploaded();
        string GetFTPDirectory();
        string GetFTPPass();
        string GetFTPServer();
        string GetFTPUser();
        bool UploadFTP();
    }
}

