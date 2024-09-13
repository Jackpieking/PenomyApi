using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.Artworks;

public sealed class ArtworkReportAttachedMedia : EntityWithId<long>
{
    public long ArtworkReportedId { get; set; }

    public string FileName { get; set; }

    public ArtworkReportAttachedMediaType MediaType { get; set; }

    public int UploadOrder { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public ArtworkReport ArtworkReport { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 32;

        public const int StorageUrlLength = 256;
    }
    #endregion
}
