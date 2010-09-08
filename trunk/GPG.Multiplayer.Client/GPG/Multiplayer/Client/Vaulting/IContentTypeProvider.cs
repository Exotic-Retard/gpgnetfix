namespace GPG.Multiplayer.Client.Vaulting
{
    using System;
    using System.Runtime.InteropServices;

    public interface IContentTypeProvider
    {
        IAdditionalContent CreateEmptyInstance();
        bool FromID(int contentId, out IAdditionalContent content);
        bool FromLocalFile(string file, out IAdditionalContent content);
        IAdditionalContent[] GetMyDownloads();
        IAdditionalContent[] GetMyUploads();

        GPG.Multiplayer.Client.Vaulting.ContentType ContentType { get; set; }
    }
}

