using PenomyAPI.Domain.RelationalDb.Entities.Base;

namespace PenomyAPI.Domain.RelationalDb.Entities.ArtworkCreation;

public sealed class ArtworkChapterMedia : EntityWithId<long>
{
    public long ChapterId { get; set; }

    public int UploadOrder { get; set; }

    public ArtworkChapterMediaType MediaType { get; set; }

    public string FileName { get; set; }

    public string StorageUrl { get; set; }

    #region Navigation
    public ArtworkChapter ArtworkChapter { get; set; }
    #endregion

    #region MetaData
    public static class MetaData
    {
        public const int FileNameLength = 32;

        public const int StorageUrlLength = 256;
    }
    #endregion
}
