using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkChapterReportAttachedMedia : EntityWithId<long>
{
    public long ChapterReportedId { get; set; }

    public string FileName { get; set; }

    public ArtworkReportAttachedMediaType MediaType { get; set; }

    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public ArtworkChapterReport ChapterReport { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 32;

        public const int StorageUrlLength = 256;
    }
    #endregion
}
