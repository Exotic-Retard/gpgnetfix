namespace GPG.Multiplayer.Client.Vaulting
{
    using DevExpress.XtraGrid.Views.Grid;
    using GPG.Logging;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Drawing;

    public interface IAdditionalContent
    {
        QuazalQuery CreateSearchQuery(int page, int pageSize);
        string CreateUploadFile();
        QuazalQuery CreateVersionQuery();
        bool DeleteMyContent(bool allVersions);
        bool DeleteMyUpload(bool allVersions);
        ContentOptions GetDetailsView();
        string GetDirectURL();
        ContentOptions GetDownloadOptions();
        string GetDownloadPath();
        ContentOptions GetUploadOptions();
        bool IsContentUnique();
        void MergeWithLocalVersion(IAdditionalContent localVersion);
        void Run();
        bool SaveDownload(IStatusProvider statusProvider);
        bool SaveDownloadData();
        void SetGridView(GridView view);

        bool CanRun { get; }

        bool CanVersion { get; }

        int[] ContentDependencies { get; }

        GPG.Multiplayer.Client.Vaulting.ContentType ContentType { get; set; }

        bool CurrentUserCanDownload { get; }

        bool CurrentUserCanUpload { get; }

        bool CurrentUserIsOwner { get; }

        int CurrentVersion { get; set; }

        string Description { get; set; }

        int Downloads { get; set; }

        int DownloadSize { get; set; }

        string DownloadVolunteerEffort { get; }

        bool Enabled { get; set; }

        bool HasVolunteeredForDownload { get; set; }

        int ID { get; set; }

        bool IsDirectHTTPDownload { get; }

        string LocalFilePath { get; set; }

        string Name { get; set; }

        int OwnerID { get; set; }

        string OwnerName { get; set; }

        float Rating { get; }

        int RatingCount { get; set; }

        Image RatingImageLarge { get; }

        Image RatingImageSmall { get; }

        int RatingTotal { get; set; }

        string RunImagePath { get; }

        string RunTooltip { get; }

        string SearchKeywords { get; set; }

        int TypeID { get; }

        int Version { get; set; }

        DateTime VersionDate { get; set; }

        string VersionNotes { get; set; }
    }
}

