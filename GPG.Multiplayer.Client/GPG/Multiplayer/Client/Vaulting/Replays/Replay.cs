namespace GPG.Multiplayer.Client.Vaulting.Replays
{
    using DevExpress.XtraGrid.Views.Grid;
    using GPG.Logging;
    using GPG.Multiplayer.Client.Vaulting;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Replay : AdditionalContent, IAdditionalContent, IContentTypeProvider
    {
        public IAdditionalContent CreateEmptyInstance()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public QuazalQuery CreateSearchQuery(int page, int pageSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string CreateUploadFile()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public QuazalQuery CreateVersionQuery()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool DeleteMyContent(bool allVersions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool DeleteMyUpload(bool allVersions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool FromID(int contentId, out IAdditionalContent content)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool FromLocalFile(string file, out IAdditionalContent content)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ContentOptions GetDetailsView()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetDirectURL()
        {
            return "";
        }

        public ContentOptions GetDownloadOptions()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetDownloadPath()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IAdditionalContent[] GetMyDownloads()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IAdditionalContent[] GetMyUploads()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ContentOptions GetUploadOptions()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsContentUnique()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MergeWithLocalVersion(IAdditionalContent localVersion)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Run()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool SaveDownload(IStatusProvider statusProvider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool SaveDownloadData()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetGridView(GridView view)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool CanRun
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool CanVersion
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public int[] ContentDependencies
        {
            get
            {
                return new int[0];
            }
        }

        public bool IsDirectHTTPDownload
        {
            get
            {
                return false;
            }
        }

        public string RunImagePath
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string RunTooltip
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}

