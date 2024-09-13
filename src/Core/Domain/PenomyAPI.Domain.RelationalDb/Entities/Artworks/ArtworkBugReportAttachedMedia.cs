using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class ArtworkBugReportAttachedMedia : EntityWithId<long>
{
    public long BugReportId { get; set; }

    public string FileName { get; set; }

    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public ArtworkBugReport BugReport { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 32;

        public const int StorageUrlLength = 256;
    }
    #endregion
}
